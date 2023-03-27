using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Models.SystemBasic;
using TAP.Base.Communication;
using TAP.Data.DataBase.Communicators;
using System.Data;
using System.Text.RegularExpressions;
using ISIA.COMMON;
using System.IO;

namespace ISIA.DETECTING.SERVICE
{
    class DetectingService : TAP.WinService.ServiceBase
    {
        #region Const

        private const string _DBA_HIST_SYSMETRIC_SUMMARY = "DBA_HIST_SYSMETRIC_SUMMARY";
        private const string _DBA_HIST_SYSSTAT = "DBA_HIST_SYSSTAT";
        private const string _DBA_HIST_SNAPSHOT = "DBA_HIST_SNAPSHOT";

        private const string _STATISTIC = "STATISTIC";
        private const string _METRIC = "METRIC";
        private const string _RAW = "RAW_";
        private const string _DETECTING_SERVICE = "DETECTING SERVICE";

        #endregion

        #region Fields

        /// <summary>
        /// SQL serch start timekey
        /// </summary>
        protected string _startTimeKey;
        /// <summary>
        /// SQL serch end timekey
        /// </summary>
        protected string _endTimeKey;
        /// <summary>
        /// ImageFile FullPath
        /// </summary>
        protected string _imageFileName;

        protected string _measureDate;
        protected string _tableName;
        protected string _snapTableName;
        protected string _rawDataWithStatement;
        protected string _insertStatement;

        #endregion

        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            DetetingServiceRun();
        }

        #region DataLoad Methods

        
        private DataTable GetRawData(ParameterInfo parameterInfo)
        {
            DataTable returnDt = new DataTable();
            DBCommunicator db = new DBCommunicator();
            StringBuilder selectSQL = new StringBuilder();

            try
            {
                string dbName = parameterInfo.DBNAME; //
                string parameterType = parameterInfo.PARAMETERTYPE; //
                string dbID = parameterInfo.DBID; //
                string instance_number = parameterInfo.INSTANCE_NUMBER; //
                string parameterId = parameterInfo.PARAMETERID; //
                string days = parameterInfo.DAYS;

                string tableName = MakeRawDataTableName(parameterInfo.PARAMETERTYPE, parameterInfo.DBNAME);
                string snapTableName = MakeRawSnapTableName(parameterInfo.DBNAME);
                string startDate = DateTime.Now.AddDays(0 - int.Parse(days)).ToString("yyyyMMdd");
                string endDate = DateTime.Now.ToString("yyyyMMdd");

                string startShiftTime = TAP.App.Base.AppConfig.ConfigManager.DefinedCollection["STARTTIME"].ToString();
                string endShiftTime = TAP.App.Base.AppConfig.ConfigManager.DefinedCollection["ENDTIME"].ToString();

                _startTimeKey = startDate + startShiftTime;
                _endTimeKey = endDate + endShiftTime;

                //parameter 별로 가져오는 테이블 및 컬럼이 다를 수 있음.
                //추후 변경시 수정 해야 함. 2023-03-16 kimseoil
                if (parameterType.Contains(_STATISTIC))
                {
                    // DBA_HIST_SYSSTAT 테이블 집계 값들은 누적치 이기 때문에
                    // 이전 SNAPSHOT 집계 값과의 차이를 기준으로 계산
                    selectSQL.Append("SELECT SNAP_ID, DBID, INSTANCE_NUMBER, STAT_ID AS PARAMETERID, STAT_NAME AS PARAMETERNAME, (VALUE - PREV_VAL) AS MEASURE_VALUE, BEGIN_INTERVAL_TIME, END_INTERVAL_TIME ");
                    selectSQL.Append("FROM ");
                    selectSQL.Append("( ");
                    selectSQL.Append(" SELECT LAG (VALUE) OVER (PARTITION BY T.DBID, T.INSTANCE_NUMBER, D.STAT_ID, T.STARTUP_TIME ORDER BY BEGIN_INTERVAL_TIME) PREV_VAL, ");
                    selectSQL.Append("        D.*, T.BEGIN_INTERVAL_TIME, T.END_INTERVAL_TIME ");
                    selectSQL.AppendFormat(" FROM {0} D, {1} T ", tableName, snapTableName);
                    selectSQL.Append("WHERE D.SNAP_ID = T.SNAP_ID AND D.DBID = T.DBID AND D.INSTANCE_NUMBER = T.INSTANCE_NUMBER ");
                    //아래부터 조건들
                    selectSQL.AppendFormat("AND T.DBID = {0} AND T.INSTANCE_NUMBER = {1} AND T.END_INTERVAL_TIME >= TO_DATE('{2}', 'YYYYMMDDHH24MISS') AND T.END_INTERVAL_TIME < TO_DATE('{3}', 'YYYYMMDDHH24MISS') ", dbID, instance_number, _startTimeKey, _endTimeKey);
                    selectSQL.AppendFormat("AND D.STAT_ID = {0} ", parameterId);
                    selectSQL.Append(") ");
                    selectSQL.Append(" WHERE PREV_VAL IS NOT NULL ");
                    selectSQL.Append(" ORDER BY SNAP_ID ");
                }
                else if (parameterType.Contains(_METRIC))
                {
                    // DBA_HIST_SYSMETRIC_SUMMARY 테이블 집계 값들은 MIN, MAX, AVG, STD_VAL중에서 AVG기준으로 계산
                    selectSQL.AppendFormat("SELECT D.SNAP_ID, D.DBID, D.INSTANCE_NUMBER, D.METRIC_ID AS PARAMETERID, D.METRIC_NAME AS PARAMETERNAME, AVERAGE AS MEASURE_VALUE, BEGIN_INTERVAL_TIME, END_INTERVAL_TIME FROM {0} D, {1} T WHERE D.SNAP_ID = T.SNAP_ID ", tableName, snapTableName);
                    selectSQL.Append("AND D.DBID = T.DBID AND D.INSTANCE_NUMBER = T.INSTANCE_NUMBER ");
                    //아래부터 조건들
                    selectSQL.AppendFormat("AND T.DBID = {0} AND T.INSTANCE_NUMBER = {1} AND T.END_INTERVAL_TIME >= TO_DATE('{2}', 'YYYYMMDDHH24MISS') AND T.END_INTERVAL_TIME < TO_DATE('{3}', 'YYYYMMDDHH24MISS') ", dbID, instance_number, _startTimeKey, _endTimeKey);
                    selectSQL.AppendFormat("AND D.METRIC_ID = {0} ", parameterId);
                    selectSQL.Append("ORDER BY D.SNAP_ID ");
                }

                returnDt = db.Select(selectSQL.ToString()).Tables[0];

                //returnDt = db.Select(selectSQL.ToString()).Tables[0];

                return returnDt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Nomal Methods
                
        private string MakeRawDataTableName(string type, string dbName)
        {
            string returnStr = string.Empty;

            try
            {
                if (_STATISTIC.Contains(type))
                {
                    returnStr = _RAW + _DBA_HIST_SYSSTAT + "_" + dbName;
                }
                else if (_METRIC.Contains(type))
                {
                    returnStr = _RAW + _DBA_HIST_SYSMETRIC_SUMMARY + "_" + dbName;
                }

                return returnStr;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private string MakeRawSnapTableName(string dbName)
        {
            string returnStr = string.Empty;

            try
            {
                returnStr = _RAW + _DBA_HIST_SNAPSHOT + "_" + dbName;

                return returnStr;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private string MakeFilePath(string filename)
        {
            string strReturn = string.Empty;
            string root = string.Empty;
            string path = string.Empty;
            try
            {
                TAP.Base.IO.FileBase fileBase = new TAP.Base.IO.FileBase();

                root = TAP.App.Base.AppConfig.ConfigManager.DefinedCollection["SAVEPATH"].ToString();
                path = string.Format(@"{0}\{1}\", DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("dd"));

                if (!Directory.Exists(root))
                    Directory.CreateDirectory(root);

                strReturn = Path.Combine(root, path);

                if (!Directory.Exists(strReturn))
                    Directory.CreateDirectory(strReturn);

                return strReturn + filename;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        private void DetetingServiceRun()
        {
            DataTable dtSPCRuleParaList = new DataTable();

            try
            {
                // 1. RuleOut 데이타를 처리 하기전에, 금일 처리한 데이타가 존재하면 삭제 먼저 진행한다.
                ExecSPCRuleOutDelete();

                // 일단 Nelson Rules 8개만 적용
                int ruleChk = 8;
                for (int i = 1; i < (ruleChk + 1); i++)
                {
                    dtSPCRuleParaList = GetSpcRuleParaList(i.ToString());

                    foreach (DataRow drRulePara in dtSPCRuleParaList.Rows)
                    {
                        ParameterInfo parameterInfo = new ParameterInfo(drRulePara);
                        ExecSpcRuleOutDetect(i.ToString(), parameterInfo);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private void ExecSPCRuleOutDelete()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder deleteSQL = new StringBuilder();

            try
            {
                deleteSQL.Append("DELETE ISIA.TAPCTOUTOFCONTROLDATASUM ");
                deleteSQL.Append("WHERE 1=1 ");
                deleteSQL.AppendFormat("AND MEASURE_DATE LIKE '{0}%' ", DateTime.Now.ToString("yyyyMMdd"));

                int resultCount = db.Save(new string[] { deleteSQL.ToString() });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetSpcRuleParaList(string ruleNo)
        {
            DBCommunicator db = new DBCommunicator();
            DataTable returnDt = new DataTable();
            StringBuilder selectSQL = new StringBuilder();

            try
            {
                selectSQL.Append("SELECT PRS.DBID, DB.DBNAME, PRS.INSTANCE_NUMBER, PD.PARAMETERTYPE, PRS.PARAMETERID, PRS.PARAMETERNAME, ");
                selectSQL.Append("       PRS.RULENO, PRS.RULENAME, RS.RULETEXT, RS.N_VALUE, RS.M_VALUE, ");
                selectSQL.Append("       PRS.DAYS, PRS.SPECUPPERLIMIT, PRS.SPECLOWERLIMIT, ");
                selectSQL.Append("       PRS.CONTROLUPPERLIMIT, PRS.CONTROLLOWERLIMIT, PRS.CHARTUSED, PRS.MAILUSED, ");
                selectSQL.Append("       PRS.MMSUSED, PRS.SPECLIMITUSED, PRS.TARGET, PRS.STD_VALUE, ");
                selectSQL.Append("       PRS.PARAVAL1, PRS.PARAVAL2, PRS.PARAVAL3, PRS.PARAVAL4, PRS.PARAVAL5 ");
                selectSQL.Append("FROM  ");
                selectSQL.Append("     TAPCTPARAMETERDEF PD,  ");
                selectSQL.Append("     TAPCTPARAMETERRULESPEC PRS, ");
                selectSQL.Append("     TAPCTSPCRULESPEC RS, ");
                selectSQL.Append("     TAPCTDATABASE DB ");
                selectSQL.Append("WHERE PD.PARAMETERID(+) = PRS.PARAMETERID ");
                selectSQL.Append("AND RS.RULENO(+) = PRS.RULENO ");
                selectSQL.Append("AND DB.DBID(+) = PRS.DBID ");
                selectSQL.AppendFormat("AND PRS.RULENO = {0} ", ruleNo);

                returnDt = db.Select(selectSQL.ToString()).Tables[0];


                return returnDt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void ExecSpcRuleOutDetect(string ruleNo, ParameterInfo parameterInfo)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder finalSQL = new StringBuilder();

            _tableName = MakeRawDataTableName(parameterInfo.PARAMETERTYPE, parameterInfo.DBNAME);
            _snapTableName = MakeRawSnapTableName(parameterInfo.DBNAME);
            string startDate = DateTime.Now.AddDays(0 - int.Parse(parameterInfo.DAYS)).ToString("yyyyMMdd");
            _measureDate = DateTime.Now.ToString("yyyyMMdd");

            string startShiftTime = TAP.App.Base.AppConfig.ConfigManager.DefinedCollection["STARTTIME"].ToString();
            string endShiftTime = TAP.App.Base.AppConfig.ConfigManager.DefinedCollection["ENDTIME"].ToString();

            _startTimeKey = startDate + startShiftTime;
            _endTimeKey = _measureDate + endShiftTime;

            // 공통으로 사용할 WITH 문 생성
            _rawDataWithStatement = makeRawDataWithStatement(parameterInfo);

            // 공통으로 사용할 INSERT 문 생성
            _insertStatement += "INSERT INTO ISIA.TAPCTOUTOFCONTROLDATASUM ( ";
            _insertStatement += "SNAP_ID, DBID, INSTANCE_NUMBER, PARAMETERID, ";
            _insertStatement += "MEASURE_VAL, STARTTIMEKEY, ENDTIMEKEY, ";
            _insertStatement += "RULENO, MEASURE_DATE, ";
            _insertStatement += "INSERTTIME, INSERTUSER, ISALIVE) ";

            try
            {
                if (ruleNo.Equals("1"))
                {
                    finalSQL = MakeSqlByRuleNo1(ruleNo, parameterInfo);
                }
                else if (ruleNo.Equals("2"))
                {
                    finalSQL = MakeSqlByRuleNo2(ruleNo, parameterInfo);
                }
                else if (ruleNo.Equals("3"))
                {
                    finalSQL = MakeSqlByRuleNo3(ruleNo, parameterInfo);
                }
                else if (ruleNo.Equals("4"))
                {
                    finalSQL = MakeSqlByRuleNo4(ruleNo, parameterInfo);
                }
                else if (ruleNo.Equals("5"))
                {
                    finalSQL = MakeSqlByRuleNo5(ruleNo, parameterInfo);
                }
                else if (ruleNo.Equals("6"))
                {
                    finalSQL = MakeSqlByRuleNo6(ruleNo, parameterInfo);
                }
                else if (ruleNo.Equals("7"))
                {
                    finalSQL = MakeSqlByRuleNo7(ruleNo, parameterInfo);
                }
                else if (ruleNo.Equals("8"))
                {
                    finalSQL = MakeSqlByRuleNo8(ruleNo, parameterInfo);
                }

                int resultCount = db.Save(new string[] { finalSQL.ToString() });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        private string makeRawDataWithStatement(ParameterInfo parameterInfo)
        {
            StringBuilder selectSQL = new StringBuilder();

            try
            {
                selectSQL.Append(" WITH RAW_TABLE AS (");

                if (parameterInfo.PARAMETERTYPE.Contains(_STATISTIC))
                {
                    // DBA_HIST_SYSSTAT 테이블 집계 값들은 누적치 이기 때문에
                    // 이전 SNAPSHOT 집계 값과의 차이를 기준으로 계산
                    selectSQL.Append("SELECT SNAP_ID, DBID, INSTANCE_NUMBER, STAT_ID AS PARAMETERID, STAT_NAME AS PARAMETERNAME, (VALUE - PREV_VAL) AS MEASURE_VALUE, BEGIN_INTERVAL_TIME, END_INTERVAL_TIME ");
                    selectSQL.Append("FROM ");
                    selectSQL.Append("( ");
                    selectSQL.Append(" SELECT LAG (VALUE) OVER (PARTITION BY T.DBID, T.INSTANCE_NUMBER, D.STAT_ID, T.STARTUP_TIME ORDER BY BEGIN_INTERVAL_TIME) PREV_VAL, ");
                    selectSQL.Append("        D.*, T.BEGIN_INTERVAL_TIME, T.END_INTERVAL_TIME ");
                    selectSQL.AppendFormat(" FROM {0} D, {1} T ", _tableName, _snapTableName);
                    selectSQL.Append("WHERE D.SNAP_ID = T.SNAP_ID AND D.DBID = T.DBID AND D.INSTANCE_NUMBER = T.INSTANCE_NUMBER ");
                    //아래부터 조건들
                    selectSQL.AppendFormat("AND T.DBID = {0} AND T.INSTANCE_NUMBER = {1} AND T.END_INTERVAL_TIME >= TO_DATE('{2}', 'YYYYMMDDHH24MISS') AND T.END_INTERVAL_TIME < TO_DATE('{3}', 'YYYYMMDDHH24MISS') ", parameterInfo.DBID, parameterInfo.INSTANCE_NUMBER, _startTimeKey, _endTimeKey);
                    selectSQL.AppendFormat("AND D.STAT_ID = {0} ", parameterInfo.PARAMETERID);
                    selectSQL.Append(") ");
                    selectSQL.Append(" WHERE PREV_VAL IS NOT NULL ");
                }
                else if (parameterInfo.PARAMETERTYPE.Contains(_METRIC))
                {
                    // DBA_HIST_SYSMETRIC_SUMMARY 테이블 집계 값들은 MIN, MAX, AVG, STD_VAL중에서 AVG기준으로 계산
                    selectSQL.AppendFormat("SELECT D.SNAP_ID, D.DBID, D.INSTANCE_NUMBER, D.METRIC_ID AS PARAMETERID, D.METRIC_NAME AS PARAMETERNAME, AVERAGE AS MEASURE_VALUE, BEGIN_INTERVAL_TIME, END_INTERVAL_TIME FROM {0} D, {1} T WHERE D.SNAP_ID = T.SNAP_ID ", _tableName, _snapTableName);
                    selectSQL.Append("AND D.DBID = T.DBID AND D.INSTANCE_NUMBER = T.INSTANCE_NUMBER ");
                    //아래부터 조건들
                    selectSQL.AppendFormat("AND T.DBID = {0} AND T.INSTANCE_NUMBER = {1} AND T.END_INTERVAL_TIME >= TO_DATE('{2}', 'YYYYMMDDHH24MISS') AND T.END_INTERVAL_TIME < TO_DATE('{3}', 'YYYYMMDDHH24MISS') ", parameterInfo.DBID, parameterInfo.INSTANCE_NUMBER, _startTimeKey, _endTimeKey);
                    selectSQL.AppendFormat("AND D.METRIC_ID = {0} ", parameterInfo.PARAMETERID);
                }

                selectSQL.Append(") ");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return selectSQL.ToString();
        }

        private StringBuilder MakeSqlByRuleNo1(string ruleNo, ParameterInfo parameterInfo)
        {
            StringBuilder ruleNo1SQL = new StringBuilder();

            try
            {
                ruleNo1SQL.Append(_insertStatement);
                ruleNo1SQL.Append(_rawDataWithStatement);

                ruleNo1SQL.Append(" SELECT SNAP_ID, DBID, INSTANCE_NUMBER, PARAMETERID, ");
                ruleNo1SQL.Append(" MEASURE_VALUE, TO_CHAR(BEGIN_INTERVAL_TIME, 'YYYYMMDDHH24MISSFF3') STARTTIMEKEY, TO_CHAR(END_INTERVAL_TIME, 'YYYYMMDDHH24MISSFF3') ENDTIMEKEY, ");
                ruleNo1SQL.AppendFormat("'{0}' RULENO, '{1}' MEASURE_DATE, ", ruleNo, _measureDate);
                ruleNo1SQL.AppendFormat("TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') INSERTTIME, '{0}' INSERTUSER, 'YES' ISALIVE ");
                ruleNo1SQL.Append("FROM ");
                ruleNo1SQL.Append("( ");
                ruleNo1SQL.Append("SELECT ROWNUM RN, A.* ");
                ruleNo1SQL.Append("FROM RAW_TABLE A ");
                ruleNo1SQL.AppendFormat("WHERE (MEASURE_VALUE < {0} OR MEASURE_VALUE > {1}) ", parameterInfo.CONTROLLOWERLIMIT, parameterInfo.CONTROLUPPERLIMIT);
                ruleNo1SQL.Append("ORDER BY SNAP_ID ");
                ruleNo1SQL.Append(") ");
                ruleNo1SQL.AppendFormat("WHERE RN >= {0} ", parameterInfo.NVALUE);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return ruleNo1SQL;
        }

        private StringBuilder MakeSqlByRuleNo2(string ruleNo, ParameterInfo parameterInfo)
        {
            StringBuilder ruleNo2SQL = new StringBuilder();


            return ruleNo2SQL;
        }

        private StringBuilder MakeSqlByRuleNo3(string ruleNo, ParameterInfo parameterInfo)
        {
            StringBuilder ruleNo3SQL = new StringBuilder();


            return ruleNo3SQL;
        }

        private StringBuilder MakeSqlByRuleNo4(string ruleNo, ParameterInfo parameterInfo)
        {
            StringBuilder ruleNo4SQL = new StringBuilder();


            return ruleNo4SQL;
        }

        private StringBuilder MakeSqlByRuleNo5(string ruleNo, ParameterInfo parameterInfo)
        {
            StringBuilder ruleNo5SQL = new StringBuilder();


            return ruleNo5SQL;
        }

        private StringBuilder MakeSqlByRuleNo6(string ruleNo, ParameterInfo parameterInfo)
        {
            StringBuilder ruleNo6SQL = new StringBuilder();


            return ruleNo6SQL;
        }

        private StringBuilder MakeSqlByRuleNo7(string ruleNo, ParameterInfo parameterInfo)
        {
            StringBuilder ruleNo7SQL = new StringBuilder();


            return ruleNo7SQL;
        }

        private StringBuilder MakeSqlByRuleNo8(string ruleNo, ParameterInfo parameterInfo)
        {
            StringBuilder ruleNo8SQL = new StringBuilder();


            return ruleNo8SQL;
        }

        #endregion

    }

    public class ParameterInfo
    {

        #region const
        private const string _DBID = "DBID";
        private const string _DBNAME = "DBNAME";
        private const string _INSTANCE_NUMBER = "INSTANCE_NUMBER";
        private const string _PARAMETERTYPE = "PARAMETERTYPE";        
        private const string _PARAMETERID = "PARAMETERID";
        private const string _PARAMETERNAME = "PARAMETERNAME";
        private const string _RULENO = "RULENO";
        private const string _RULENAME = "RULENAME";

        private const string _RULETEXT = "RULETEXT";
        private const string _NVALUE = "N_VALUE";
        private const string _MVALUE = "M_VALUE";

        private const string _DAYS = "DAYS";
        private const string _SPECUPPERLIMIT = "SPECUPPERLIMIT";
        private const string _SPECLOWERLIMIT = "SPECLOWERLIMIT";
        private const string _CONTROLUPPERLIMIT = "CONTROLUPPERLIMIT";
        private const string _CONTROLLOWERLIMIT = "CONTROLLOWERLIMIT";
        private const string _CHARTUSED = "CHARTUSED";
        private const string _MAILUSED = "MAILUSED";
        private const string _MMSUSED = "MMSUSED";
        private const string _SPECLIMITUSED = "SPECLIMITUSED";

        private const string _TARGET = "TARGET";
        private const string _STDVALUE = "STD_VALUE";
        private const string _PARAVAL1 = "PARAVAL1";
        private const string _PARAVAL2 = "PARAVAL2";
        private const string _PARAVAL3 = "PARAVAL3";
        private const string _PARAVAL4 = "PARAVAL4";
        private const string _PARAVAL5 = "PARAVAL5";
        #endregion

        #region Properties
        public string DBID { get; set; }
        public string DBNAME { get; set; }        
        public string INSTANCE_NUMBER { get; set; }
        public string PARAMETERTYPE { get; set; }
        public string PARAMETERID { get; set; }        
        public string PARAMETERNAME { get; set; }
        public string RULENO { get; set; }
        public string RULENAME { get; set; }
        public string RULETEXT { get; set; }
        public string NVALUE { get; set; }
        public string MVALUE { get; set; }
        public string DAYS { get; set; }
        public string SPECUPPERLIMIT { get; set; }
        public string SPECLOWERLIMIT { get; set; }
        public string CONTROLUPPERLIMIT { get; set; }
        public string CONTROLLOWERLIMIT { get; set; }
        public string CHARTUSED { get; set; }
        public string MAILUSED { get; set; }
        public string MMSUSED { get; set; }
        public string SPECLIMITUSED { get; set; }
        public string TARGET { get; set; }
        public string STDVALUE { get; set; }
        public string PARAVAL1 { get; set; }
        public string PARAVAL2 { get; set; }
        public string PARAVAL3 { get; set; }
        public string PARAVAL4 { get; set; }
        public string PARAVAL5 { get; set; }


        #endregion

        #region Creator

        public ParameterInfo()
        {
            DBNAME = string.Empty;
        }

        public ParameterInfo(DataRow dataRow)
        {
            foreach (DataColumn dc in dataRow.Table.Columns)
            {
                switch (dc.ColumnName)
                {
                    case _DBID:
                        DBID = dataRow[_DBID].ToString();
                        break;
                    case _DBNAME:
                        DBNAME = dataRow[_DBNAME].ToString();
                        break;
                    case _INSTANCE_NUMBER:
                        INSTANCE_NUMBER = dataRow[_INSTANCE_NUMBER].ToString();
                        break;
                    case _PARAMETERTYPE:
                        PARAMETERTYPE = dataRow[_PARAMETERTYPE].ToString();
                        break;
                    case _PARAMETERID:
                        PARAMETERID = dataRow[_PARAMETERID].ToString();
                        break;                    
                    case _PARAMETERNAME:
                        PARAMETERNAME = dataRow[_PARAMETERNAME].ToString();
                        break;
                    case _RULENO:
                        RULENO = dataRow[_RULENO].ToString();
                        break;
                    case _RULENAME:
                        RULENAME = dataRow[_RULENAME].ToString();
                        break;
                    case _RULETEXT:
                        RULETEXT = dataRow[_RULETEXT].ToString();
                        break;
                    case _NVALUE:
                        NVALUE = dataRow[_NVALUE].ToString();
                        break;
                    case _MVALUE:
                        MVALUE = dataRow[_MVALUE].ToString();
                        break;
                    case _DAYS:
                        DAYS = dataRow[_DAYS].ToString();
                        break;
                    case _SPECUPPERLIMIT:
                        SPECUPPERLIMIT = dataRow[_SPECUPPERLIMIT].ToString();
                        break;
                    case _SPECLOWERLIMIT:
                        SPECLOWERLIMIT = dataRow[_SPECLOWERLIMIT].ToString();
                        break;
                    case _CONTROLUPPERLIMIT:
                        CONTROLUPPERLIMIT = dataRow[_CONTROLUPPERLIMIT].ToString();
                        break;
                    case _CONTROLLOWERLIMIT:
                        CONTROLLOWERLIMIT = dataRow[_CONTROLLOWERLIMIT].ToString();
                        break;
                    case _CHARTUSED:
                        CHARTUSED = dataRow[_CHARTUSED].ToString();
                        break;
                    case _MAILUSED:
                        MAILUSED = dataRow[_MAILUSED].ToString();
                        break;
                    case _MMSUSED:
                        MMSUSED = dataRow[_MMSUSED].ToString();
                        break;
                    case _SPECLIMITUSED:
                        SPECLIMITUSED = dataRow[_SPECLIMITUSED].ToString();
                        break;
                    case _TARGET:
                        TARGET = dataRow[_TARGET].ToString();
                        break;
                    case _STDVALUE:
                        STDVALUE = dataRow[_STDVALUE].ToString();
                        break;
                    case _PARAVAL1:
                        PARAVAL1 = dataRow[_PARAVAL1].ToString();
                        break;
                    case _PARAVAL2:
                        PARAVAL2 = dataRow[_PARAVAL2].ToString();
                        break;
                    case _PARAVAL3:
                        PARAVAL3 = dataRow[_PARAVAL3].ToString();
                        break;
                    case _PARAVAL4:
                        PARAVAL4 = dataRow[_PARAVAL4].ToString();
                        break;
                    case _PARAVAL5:
                        PARAVAL5 = dataRow[_PARAVAL5].ToString();
                        break;
                }
            }
        }

        #endregion
    }
}
