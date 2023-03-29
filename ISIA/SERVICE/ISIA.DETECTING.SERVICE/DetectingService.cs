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

        private const int _MailCount = 50;

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
        protected string _FileName;
        protected string _imageFileName;

        protected string _measureDate;
        protected string _measureYesterDay;
        protected string _tableName;
        protected string _snapTableName;
        protected string _rawDataWithStatement;
        protected string _insertStatement;
        // 메일 수신자 메일 주소
        protected string _mailAddress;
        // 하루치 Summary 완료후에, 전 날짜의 데이타와
        // 비교하여 중복된 데이타 있으면 제거
        // TRUE : 중복제거 하지 않음
        // FALSE : 중복제거
        protected string _isDuplicatedAllow;

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
            DataTable dtRuleList = new DataTable();
            DataTable dtSPCRuleParaList = new DataTable();

            try
            {
                // 1. RuleOut 데이타를 처리 하기전에, 금일 처리한 데이타가 존재하면 삭제 먼저 진행한다.
                ExecSPCRuleOutDelete();

                // 일단 Nelson Rules 8개만 적용
                int ruleChk = 8;

                // 실제로 사용할 Rule만 적용
                // ISALIVE = 'YES'
                dtRuleList = GetAvailableRuleList();

                // 공통으로 사용할 INSERT 문 생성
                _insertStatement += "INSERT INTO ISIA.TAPCTOUTOFCONTROLDATASUM ( ";
                _insertStatement += "SNAP_ID, DBID, INSTANCE_NUMBER, PARAMETERID, ";
                _insertStatement += "MEASURE_VAL, STARTTIMEKEY, ENDTIMEKEY, ";
                _insertStatement += "RULENO, MEASURE_DATE, ";
                _insertStatement += "INSERTTIME, INSERTUSER, ISALIVE) ";

                _mailAddress = TAP.App.Base.AppConfig.ConfigManager.DefinedCollection["MAIL_ADDRESS"].ToString();
                _isDuplicatedAllow = TAP.App.Base.AppConfig.ConfigManager.DefinedCollection["ISDUPLICATEDALLOW"].ToString();

                foreach (DataRow drRule in dtRuleList.Rows)
                {
                    dtSPCRuleParaList = GetSpcRuleParaList(drRule["RULENO"].ToString());

                    foreach (DataRow drRulePara in dtSPCRuleParaList.Rows)
                    {
                        ParameterInfo parameterInfo = new ParameterInfo(drRulePara);
                        ExecSpcRuleOutDetect(drRule["RULENO"].ToString(), parameterInfo);
                    }
                }

                // Summary 완료후에 메일 발송
                this.SendMail();
                // Summary 완료후에 중복 데이타 삭제
                // 중복 허용 Flag가 False이면 작업 진행
                if (_isDuplicatedAllow.Equals("FALSE"))
                {
                    this.DeleteForDuplicatedSummaryRows();
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

        private DataTable GetAvailableRuleList()
        {
            DBCommunicator db = new DBCommunicator();
            DataTable returnDt = new DataTable();
            StringBuilder selectSQL = new StringBuilder();

            try
            {
                selectSQL.Append("SELECT RULENO, RULENAME, RULETEXT ");
                selectSQL.Append("FROM  ");
                selectSQL.Append("     TAPCTSPCRULESPEC ");
                selectSQL.Append("WHERE 1=1 ");
                selectSQL.Append("AND ISALIVE = 'YES' ");

                returnDt = db.Select(selectSQL.ToString()).Tables[0];


                return returnDt;
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
                selectSQL.Append("AND PRS.ISALIVE = 'YES' ");
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
            _measureYesterDay = DateTime.Now.AddDays(0 - 1).ToString("yyyyMMdd");

            string startShiftTime = TAP.App.Base.AppConfig.ConfigManager.DefinedCollection["STARTTIME"].ToString();
            string endShiftTime = TAP.App.Base.AppConfig.ConfigManager.DefinedCollection["ENDTIME"].ToString();

            _startTimeKey = startDate + startShiftTime;
            _endTimeKey = _measureDate + endShiftTime;

            // 공통으로 사용할 WITH 문 생성
            _rawDataWithStatement = makeRawDataWithStatement(parameterInfo);

            try
            {
                // 일단은 Rule Number 1~8로 고정
                // 추후 개선 예정
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
                    selectSQL.Append(" SELECT LAG (VALUE) OVER (PARTITION BY T.DBID, T.INSTANCE_NUMBER, D.STAT_ID, T.STARTUP_TIME ORDER BY END_INTERVAL_TIME) PREV_VAL, ");
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
                ruleNo1SQL.Append("TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') INSERTTIME, 'DETECTING_BATCH' INSERTUSER, 'YES' ISALIVE ");
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

            StringBuilder tmpHeadSQL = new StringBuilder();
            StringBuilder tmpBodySQL = new StringBuilder();
            StringBuilder tmpTailSQL = new StringBuilder();
            StringBuilder targetToUCLSQL = new StringBuilder();
            StringBuilder LCLTotargetSQL = new StringBuilder();

            ruleNo2SQL.Append(_insertStatement);
            ruleNo2SQL.Append(_rawDataWithStatement);

            ruleNo2SQL.Append(" SELECT SNAP_ID, DBID, INSTANCE_NUMBER, PARAMETERID, ");
            ruleNo2SQL.Append(" MEASURE_VALUE, TO_CHAR(BEGIN_INTERVAL_TIME, 'YYYYMMDDHH24MISSFF3') STARTTIMEKEY, TO_CHAR(END_INTERVAL_TIME, 'YYYYMMDDHH24MISSFF3') ENDTIMEKEY, ");
            ruleNo2SQL.AppendFormat("'{0}' RULENO, '{1}' MEASURE_DATE, ", ruleNo, _measureDate);
            ruleNo2SQL.Append("TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') INSERTTIME, 'DETECTING_BATCH' INSERTUSER, 'YES' ISALIVE ");
            ruleNo2SQL.Append("FROM ");
            ruleNo2SQL.Append("( ");

            tmpHeadSQL.Append("SELECT * ");
            tmpHeadSQL.Append("FROM ");
            tmpHeadSQL.Append("(");
            tmpHeadSQL.Append("SELECT XX.*, ROW_NUMBER() OVER(PARTITION BY XX.DBID, XX.INSTANCE_NUMBER, XX.PARAMETERID, XX.DIFF_N ORDER BY XX.END_INTERVAL_TIME DESC) CON_OOC_CNT ");
            tmpHeadSQL.Append("FROM(");
            tmpHeadSQL.Append("SELECT A.*, NVL(RN2, 0) RN2, (RN - RN2) DIFF_N ");
            tmpHeadSQL.Append("FROM ");
            tmpHeadSQL.Append("         (SELECT R.*, ROW_NUMBER() OVER(PARTITION BY R.DBID, R.INSTANCE_NUMBER, R.PARAMETERID ORDER BY R.END_INTERVAL_TIME DESC) RN ");
            tmpHeadSQL.Append("FROM RAW_TABLE R ");
            tmpHeadSQL.Append(") A, ");
            tmpHeadSQL.Append("(SELECT F.*, ROW_NUMBER() OVER(PARTITION BY F.DBID, F.INSTANCE_NUMBER, F.PARAMETERID ORDER BY F.END_INTERVAL_TIME DESC) RN2 ");
            tmpHeadSQL.Append("FROM RAW_TABLE F ");

            targetToUCLSQL.Append(tmpHeadSQL.ToString());
            LCLTotargetSQL.Append(tmpHeadSQL.ToString());

            // +3 SIGMA에 존재  TARGET 보다 크고, UCL 보다 작다
            if (parameterInfo.PARAVAL1.Equals("2"))
            {
                tmpBodySQL.AppendFormat("WHERE {0} < F.MEASURE_VALUE AND F.MEASURE_VALUE < {1} ", parameterInfo.TARGET, parameterInfo.CONTROLUPPERLIMIT);
            }
            // -3 SIGMA에 존재  TARGET 보다 작고, LCL 보다 크다 
            else if (parameterInfo.PARAVAL1.Equals("3"))
            {
                tmpBodySQL.AppendFormat("WHERE {0} < F.MEASURE_VALUE AND F.MEASURE_VALUE < {1} ", parameterInfo.CONTROLLOWERLIMIT, parameterInfo.TARGET);
            }

            targetToUCLSQL.AppendFormat("WHERE {0} < F.MEASURE_VALUE AND F.MEASURE_VALUE < {1} ", parameterInfo.TARGET, parameterInfo.CONTROLUPPERLIMIT);
            LCLTotargetSQL.AppendFormat("WHERE {0} < F.MEASURE_VALUE AND F.MEASURE_VALUE < {1} ", parameterInfo.CONTROLLOWERLIMIT, parameterInfo.TARGET);

            tmpTailSQL.Append(") B ");
            tmpTailSQL.Append("WHERE A.SNAP_ID = B.SNAP_ID(+) ");
            tmpTailSQL.Append("AND A.DBID = B.DBID(+) ");
            tmpTailSQL.Append("AND A.INSTANCE_NUMBER = B.INSTANCE_NUMBER(+) ");
            tmpTailSQL.Append("AND A.PARAMETERID = B.PARAMETERID(+) ");
            tmpTailSQL.Append(") XX ");
            tmpTailSQL.Append("WHERE DIFF_N IS NOT NULL ");
            tmpTailSQL.Append(") ");
            // 연속 OutOfCount 갯수
            tmpTailSQL.AppendFormat("WHERE CON_OOC_CNT >= {0} ", parameterInfo.NVALUE);

            targetToUCLSQL.Append(tmpTailSQL.ToString());
            LCLTotargetSQL.Append(tmpTailSQL.ToString());


            // ALL
            // +3 SIGMA에 존재  TARGET 보다 크고, UCL 보다 작다
            // -3 SIGMA에 존재  TARGET 보다 작고, LCL 보다 크다 
            if (parameterInfo.PARAVAL1.Equals("1"))
            {
                ruleNo2SQL.Append(targetToUCLSQL);

                ruleNo2SQL.Append("UNION ALL ");

                ruleNo2SQL.Append(LCLTotargetSQL);
            }
            else
            {
                ruleNo2SQL.Append(tmpHeadSQL.ToString());
                ruleNo2SQL.Append(tmpBodySQL.ToString());
                ruleNo2SQL.Append(tmpTailSQL.ToString());
            }

            ruleNo2SQL.Append(" ) ");

            return ruleNo2SQL;
        }

        private StringBuilder MakeSqlByRuleNo3(string ruleNo, ParameterInfo parameterInfo)
        {
            StringBuilder ruleNo3SQL = new StringBuilder();

            StringBuilder tmpHeadSQL = new StringBuilder();
            StringBuilder tmpBodySQL = new StringBuilder();
            StringBuilder tmpTailSQL = new StringBuilder();
            StringBuilder increaseSQL = new StringBuilder();
            StringBuilder decreaseSQL = new StringBuilder();

            ruleNo3SQL.Append(_insertStatement);
            ruleNo3SQL.Append(_rawDataWithStatement);

            ruleNo3SQL.Append(" SELECT SNAP_ID, DBID, INSTANCE_NUMBER, PARAMETERID, ");
            ruleNo3SQL.Append(" MEASURE_VALUE, TO_CHAR(BEGIN_INTERVAL_TIME, 'YYYYMMDDHH24MISSFF3') STARTTIMEKEY, TO_CHAR(END_INTERVAL_TIME, 'YYYYMMDDHH24MISSFF3') ENDTIMEKEY, ");
            ruleNo3SQL.AppendFormat("'{0}' RULENO, '{1}' MEASURE_DATE, ", ruleNo, _measureDate);
            ruleNo3SQL.Append("TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') INSERTTIME, 'DETECTING_BATCH' INSERTUSER, 'YES' ISALIVE ");
            ruleNo3SQL.Append("FROM ");
            ruleNo3SQL.Append("( ");

            tmpHeadSQL.Append("SELECT * ");
            tmpHeadSQL.Append("FROM ");
            tmpHeadSQL.Append("( ");
            tmpHeadSQL.Append("SELECT XX.*, ROW_NUMBER() OVER(PARTITION BY XX.DBID, XX.INSTANCE_NUMBER, XX.PARAMETERID, XX.DIFF_VAL ORDER BY XX.END_INTERVAL_TIME) CON_OOC_CNT ");
            tmpHeadSQL.Append("FROM( ");
            tmpHeadSQL.Append("SELECT A.*, RN2, (RN - RN2) AS DIFF_VAL ");
            tmpHeadSQL.Append("FROM ");
            tmpHeadSQL.Append("(SELECT R.*, ROW_NUMBER() OVER(PARTITION BY R.DBID, R.INSTANCE_NUMBER, R.PARAMETERID ORDER BY R.END_INTERVAL_TIME) RN ");
            tmpHeadSQL.Append("FROM RAW_TABLE R ");
            tmpHeadSQL.Append(") A, ");
            tmpHeadSQL.Append("( ");
            tmpHeadSQL.Append("SELECT T.*, ROW_NUMBER() OVER(ORDER BY T.END_INTERVAL_TIME) RN2 ");
            tmpHeadSQL.Append("FROM( ");
            tmpHeadSQL.Append("SELECT S.*, (MEASURE_VALUE - PREV_MEASURE_VALUE) DIFF_MEASURE_VALUE, CASE WHEN(MEASURE_VALUE - PREV_MEASURE_VALUE) > 0 THEN 1 ELSE 0 END OOC_CHK ");
            tmpHeadSQL.Append("FROM( ");
            tmpHeadSQL.Append("SELECT F.*, LAG(MEASURE_VALUE) OVER(PARTITION BY F.DBID, F.INSTANCE_NUMBER, F.PARAMETERID ORDER BY F.END_INTERVAL_TIME) PREV_MEASURE_VALUE ");
            tmpHeadSQL.Append("FROM RAW_TABLE F ");
            tmpHeadSQL.Append(") S ");
            tmpHeadSQL.Append(") T ");

            increaseSQL.Append(tmpHeadSQL.ToString());
            decreaseSQL.Append(tmpHeadSQL.ToString());

            // 연속 증가
            if (parameterInfo.PARAVAL1.Equals("2"))
            {
                tmpBodySQL.Append("WHERE OOC_CHK = 1 ");
            }
            // 연속 감소
            else if (parameterInfo.PARAVAL1.Equals("3"))
            {
                tmpBodySQL.Append("WHERE OOC_CHK = 0 ");
            }

            increaseSQL.Append("WHERE OOC_CHK = 1 ");
            decreaseSQL.Append("WHERE OOC_CHK = 0 ");


            tmpTailSQL.Append(") B ");
            tmpTailSQL.Append("WHERE A.SNAP_ID = B.SNAP_ID(+) ");
            tmpTailSQL.Append("AND A.DBID = B.DBID(+) ");
            tmpTailSQL.Append("AND A.INSTANCE_NUMBER = B.INSTANCE_NUMBER(+) ");
            tmpTailSQL.Append("AND A.PARAMETERID = B.PARAMETERID(+) ");
            tmpTailSQL.Append(") XX ");
            tmpTailSQL.Append("WHERE RN2 IS NOT NULL ");
            tmpTailSQL.Append(") ");
            tmpTailSQL.AppendFormat("WHERE CON_OOC_CNT >= {0} ", parameterInfo.NVALUE);

            increaseSQL.Append(tmpTailSQL.ToString());
            decreaseSQL.Append(tmpTailSQL.ToString());

            // ALL
            // 연속 증가
            // 연속 감소
            if (parameterInfo.PARAVAL1.Equals("1"))
            {
                ruleNo3SQL.Append(increaseSQL);

                ruleNo3SQL.Append("UNION ALL ");

                ruleNo3SQL.Append(decreaseSQL);
            }
            else
            {
                ruleNo3SQL.Append(tmpHeadSQL.ToString());
                ruleNo3SQL.Append(tmpBodySQL.ToString());
                ruleNo3SQL.Append(tmpTailSQL.ToString());
            }

            ruleNo3SQL.Append(" ) ");


            return ruleNo3SQL;
        }

        private StringBuilder MakeSqlByRuleNo4(string ruleNo, ParameterInfo parameterInfo)
        {
            StringBuilder ruleNo4SQL = new StringBuilder();

            ruleNo4SQL.Append(_insertStatement);
            ruleNo4SQL.Append(_rawDataWithStatement);

            ruleNo4SQL.Append(" SELECT SNAP_ID, DBID, INSTANCE_NUMBER, PARAMETERID, ");
            ruleNo4SQL.Append(" MEASURE_VALUE, TO_CHAR(BEGIN_INTERVAL_TIME, 'YYYYMMDDHH24MISSFF3') STARTTIMEKEY, TO_CHAR(END_INTERVAL_TIME, 'YYYYMMDDHH24MISSFF3') ENDTIMEKEY, ");
            ruleNo4SQL.AppendFormat("'{0}' RULENO, '{1}' MEASURE_DATE, ", ruleNo, _measureDate);
            ruleNo4SQL.Append("TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') INSERTTIME, 'DETECTING_BATCH' INSERTUSER, 'YES' ISALIVE ");
            ruleNo4SQL.Append("FROM ");
            ruleNo4SQL.Append("( ");

            ruleNo4SQL.Append("SELECT * ");
            ruleNo4SQL.Append("FROM ");
            ruleNo4SQL.Append("( ");
            ruleNo4SQL.Append("SELECT F_C.*, ROW_NUMBER() OVER(PARTITION BY F_C.DBID, F_C.INSTANCE_NUMBER, F_C.PARAMETERID, DIFF_VAL ORDER BY F_C.END_INTERVAL_TIME) CON_OOC_CNT ");
            ruleNo4SQL.Append("FROM ");
            ruleNo4SQL.Append("( ");
            ruleNo4SQL.Append("SELECT F_A.*, RN3, (F_A.RN - RN3) AS DIFF_VAL ");
            ruleNo4SQL.Append("FROM(SELECT R.*, ROW_NUMBER() OVER(PARTITION BY R.DBID, R.INSTANCE_NUMBER, R.PARAMETERID ORDER BY R.END_INTERVAL_TIME) RN ");
            ruleNo4SQL.Append("FROM RAW_TABLE R ");
            ruleNo4SQL.Append(") F_A, ");
            ruleNo4SQL.Append("( ");
            ruleNo4SQL.Append("SELECT BB.*, ROW_NUMBER() OVER(PARTITION BY BB.DBID, BB.INSTANCE_NUMBER, BB.PARAMETERID, BB.OOC_CHK2 ORDER BY BB.END_INTERVAL_TIME) RN3 ");
            ruleNo4SQL.Append("FROM ");
            ruleNo4SQL.Append("( ");
            ruleNo4SQL.Append("SELECT AA.*, CASE WHEN AA.OOC_CHK = AA.OOC_VAL THEN 1 ELSE 0 END OOC_CHK2 ");
            ruleNo4SQL.Append("FROM ");
            ruleNo4SQL.Append("( ");
            ruleNo4SQL.Append("SELECT A.*, RN2, (RN - RN2) AS DIFF_VAL, CASE WHEN MOD(RN, 2) = 0 THEN 0 ELSE 1 END OOC_VAL, B.OOC_CHK ");
            ruleNo4SQL.Append("FROM ");
            ruleNo4SQL.Append("(SELECT R.*, ROW_NUMBER() OVER(PARTITION BY R.DBID, R.INSTANCE_NUMBER, R.PARAMETERID ORDER BY R.END_INTERVAL_TIME) RN ");
            ruleNo4SQL.Append("FROM RAW_TABLE R ");
            ruleNo4SQL.Append(") A, ");
            ruleNo4SQL.Append("( ");
            ruleNo4SQL.Append("SELECT T.*, ROW_NUMBER() OVER(PARTITION BY T.DBID, T.INSTANCE_NUMBER, T.PARAMETERID ORDER BY T.END_INTERVAL_TIME) RN2 ");
            ruleNo4SQL.Append("FROM( ");
            ruleNo4SQL.Append("SELECT S.*, (MEASURE_VALUE - PREV_MEASURE_VALUE) DIFF_MEASURE_VALUE, CASE WHEN(MEASURE_VALUE - PREV_MEASURE_VALUE) > 0 THEN 1 ELSE 0 END OOC_CHK ");
            ruleNo4SQL.Append("FROM( ");
            ruleNo4SQL.Append("SELECT F.*, LAG(MEASURE_VALUE) OVER(PARTITION BY F.DBID, F.INSTANCE_NUMBER, F.PARAMETERID ORDER BY F.END_INTERVAL_TIME) PREV_MEASURE_VALUE ");
            ruleNo4SQL.Append("FROM RAW_TABLE F ");
            ruleNo4SQL.Append(") S ");
            ruleNo4SQL.Append(") T ");
            ruleNo4SQL.Append(") B ");
            ruleNo4SQL.Append("WHERE A.SNAP_ID = B.SNAP_ID(+) ");
            ruleNo4SQL.Append("AND A.DBID = B.DBID(+) ");
            ruleNo4SQL.Append("AND A.INSTANCE_NUMBER = B.INSTANCE_NUMBER(+) ");
            ruleNo4SQL.Append("AND A.PARAMETERID = B.PARAMETERID(+) ");
            ruleNo4SQL.Append(") AA ");
            ruleNo4SQL.Append(") BB ");
            ruleNo4SQL.Append("WHERE OOC_CHK2 = 1 ");
            ruleNo4SQL.Append(") F_B ");
            ruleNo4SQL.Append("WHERE F_A.SNAP_ID = F_B.SNAP_ID(+) ");
            ruleNo4SQL.Append("AND F_A.DBID = F_B.DBID(+) ");
            ruleNo4SQL.Append("AND F_A.INSTANCE_NUMBER = F_B.INSTANCE_NUMBER(+) ");
            ruleNo4SQL.Append("AND F_A.PARAMETERID = F_B.PARAMETERID(+) ");
            ruleNo4SQL.Append(") F_C ");
            ruleNo4SQL.Append("WHERE F_C.RN3 IS NOT NULL ");
            ruleNo4SQL.Append(") ");
            ruleNo4SQL.AppendFormat("WHERE CON_OOC_CNT >= {0} ", parameterInfo.NVALUE);
            ruleNo4SQL.Append(") ");


            return ruleNo4SQL;
        }

        private StringBuilder MakeSqlByRuleNo5(string ruleNo, ParameterInfo parameterInfo)
        {
            StringBuilder ruleNo5SQL = new StringBuilder();
            StringBuilder ucl2lcl2SQL = new StringBuilder();
            StringBuilder oocCHKSQL = new StringBuilder();

            ucl2lcl2SQL.Append("SELECT R.*, ROW_NUMBER() OVER(PARTITION BY R.DBID, R.INSTANCE_NUMBER, R.PARAMETERID ORDER BY R.END_INTERVAL_TIME) RN, ");
            ucl2lcl2SQL.AppendFormat("({0} - ({1} * {2})) AS LCL2,", parameterInfo.TARGET, parameterInfo.STDVALUE, parameterInfo.MVALUE);
            ucl2lcl2SQL.AppendFormat("({0} + ({1} * {2})) AS UCL2 ", parameterInfo.TARGET, parameterInfo.STDVALUE, parameterInfo.MVALUE);
            ucl2lcl2SQL.Append(" FROM RAW_TABLE R ");

            oocCHKSQL.Append("SELECT AA.*, ");

            // +- M sigma 밖에 위치
            if (parameterInfo.PARAVAL1.Equals("1"))
            {
                oocCHKSQL.Append("CASE WHEN (AA.MEASURE_VALUE < AA.LCL2 OR AA.MEASURE_VALUE > AA.UCL2) THEN 1 ");
            }
            // + M sigma 밖에 위치
            else if (parameterInfo.PARAVAL1.Equals("2"))
            {
                oocCHKSQL.Append("CASE WHEN AA.MEASURE_VALUE > AA.UCL2 THEN 1 ");
            }
            // - M sigma 밖에 위치
            else if (parameterInfo.PARAVAL1.Equals("3"))
            {
                oocCHKSQL.Append("CASE WHEN AA.MEASURE_VALUE < AA.LCL2 THEN 1 ");
            }

            oocCHKSQL.Append(" ELSE 0 END OOC_CHK ");
            oocCHKSQL.Append(" FROM ");
            oocCHKSQL.Append(" ( ");

            oocCHKSQL.Append(ucl2lcl2SQL.ToString());

            oocCHKSQL.Append(" ) AA ");


            ruleNo5SQL.Append(_insertStatement);
            ruleNo5SQL.Append(_rawDataWithStatement);

            ruleNo5SQL.Append(" SELECT SNAP_ID, DBID, INSTANCE_NUMBER, PARAMETERID, ");
            ruleNo5SQL.Append(" MEASURE_VALUE, TO_CHAR(BEGIN_INTERVAL_TIME, 'YYYYMMDDHH24MISSFF3') STARTTIMEKEY, TO_CHAR(END_INTERVAL_TIME, 'YYYYMMDDHH24MISSFF3') ENDTIMEKEY, ");
            ruleNo5SQL.AppendFormat("'{0}' RULENO, '{1}' MEASURE_DATE, ", ruleNo, _measureDate);
            ruleNo5SQL.Append("TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') INSERTTIME, 'DETECTING_BATCH' INSERTUSER, 'YES' ISALIVE ");
            ruleNo5SQL.Append("FROM ");
            ruleNo5SQL.Append("( ");

            ruleNo5SQL.Append("SELECT XX.*,  ");
            ruleNo5SQL.Append("( ");
            ruleNo5SQL.Append("SELECT SUM(OOC_CHK) ");
            ruleNo5SQL.Append("FROM ");
            ruleNo5SQL.Append(" ( ");

            ruleNo5SQL.Append(oocCHKSQL.ToString());

            ruleNo5SQL.Append(" ) OO ");
            ruleNo5SQL.Append("WHERE 1 = 1 ");
            ruleNo5SQL.AppendFormat("AND (RN >= XX.RN - ({0}-1) AND RN <= XX.RN)", parameterInfo.NVALUE);
            ruleNo5SQL.Append(") OOC_SUM ");
            ruleNo5SQL.Append("FROM ");
            ruleNo5SQL.Append("( ");

            ruleNo5SQL.Append(ucl2lcl2SQL.ToString());

            ruleNo5SQL.Append(") XX ");
            ruleNo5SQL.Append(") ZZ ");
            ruleNo5SQL.Append("WHERE 1 = 1 ");
            ruleNo5SQL.AppendFormat("AND RN >= {0} AND OOC_SUM >= {1} ", parameterInfo.NVALUE, parameterInfo.PARAVAL2);


            return ruleNo5SQL;
        }

        private StringBuilder MakeSqlByRuleNo6(string ruleNo, ParameterInfo parameterInfo)
        {
            StringBuilder ruleNo6SQL = new StringBuilder();
            StringBuilder ucl2lcl2SQL = new StringBuilder();
            StringBuilder oocCHKSQL = new StringBuilder();

            ucl2lcl2SQL.Append("SELECT R.*, ROW_NUMBER() OVER(PARTITION BY R.DBID, R.INSTANCE_NUMBER, R.PARAMETERID ORDER BY R.END_INTERVAL_TIME) RN, ");
            ucl2lcl2SQL.AppendFormat("({0} - ({1} * {2})) AS LCL2,", parameterInfo.TARGET, parameterInfo.STDVALUE, parameterInfo.MVALUE);
            ucl2lcl2SQL.AppendFormat("({0} + ({1} * {2})) AS UCL2 ", parameterInfo.TARGET, parameterInfo.STDVALUE, parameterInfo.MVALUE);
            ucl2lcl2SQL.Append(" FROM RAW_TABLE R ");

            oocCHKSQL.Append("SELECT AA.*, ");

            // +- M sigma 밖에 위치
            if (parameterInfo.PARAVAL1.Equals("1"))
            {
                oocCHKSQL.Append("CASE WHEN (AA.MEASURE_VALUE < AA.LCL2 OR AA.MEASURE_VALUE > AA.UCL2) THEN 1 ");
            }
            // + M sigma 밖에 위치
            else if (parameterInfo.PARAVAL1.Equals("2"))
            {
                oocCHKSQL.Append("CASE WHEN AA.MEASURE_VALUE > AA.UCL2 THEN 1 ");
            }
            // - M sigma 밖에 위치
            else if (parameterInfo.PARAVAL1.Equals("3"))
            {
                oocCHKSQL.Append("CASE WHEN AA.MEASURE_VALUE < AA.LCL2 THEN 1 ");
            }

            oocCHKSQL.Append(" ELSE 0 END OOC_CHK ");
            oocCHKSQL.Append(" FROM ");
            oocCHKSQL.Append(" ( ");

            oocCHKSQL.Append(ucl2lcl2SQL.ToString());

            oocCHKSQL.Append(" ) AA ");


            ruleNo6SQL.Append(_insertStatement);
            ruleNo6SQL.Append(_rawDataWithStatement);

            ruleNo6SQL.Append(" SELECT SNAP_ID, DBID, INSTANCE_NUMBER, PARAMETERID, ");
            ruleNo6SQL.Append(" MEASURE_VALUE, TO_CHAR(BEGIN_INTERVAL_TIME, 'YYYYMMDDHH24MISSFF3') STARTTIMEKEY, TO_CHAR(END_INTERVAL_TIME, 'YYYYMMDDHH24MISSFF3') ENDTIMEKEY, ");
            ruleNo6SQL.AppendFormat("'{0}' RULENO, '{1}' MEASURE_DATE, ", ruleNo, _measureDate);
            ruleNo6SQL.Append("TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') INSERTTIME, 'DETECTING_BATCH' INSERTUSER, 'YES' ISALIVE ");
            ruleNo6SQL.Append("FROM ");
            ruleNo6SQL.Append("( ");

            ruleNo6SQL.Append("SELECT XX.*,  ");
            ruleNo6SQL.Append("( ");
            ruleNo6SQL.Append("SELECT SUM(OOC_CHK) ");
            ruleNo6SQL.Append("FROM ");
            ruleNo6SQL.Append(" ( ");

            ruleNo6SQL.Append(oocCHKSQL.ToString());

            ruleNo6SQL.Append(" ) OO ");
            ruleNo6SQL.Append("WHERE 1 = 1 ");
            ruleNo6SQL.AppendFormat("AND (RN >= XX.RN - ({0}-1) AND RN <= XX.RN)", parameterInfo.NVALUE);
            ruleNo6SQL.Append(") OOC_SUM ");
            ruleNo6SQL.Append("FROM ");
            ruleNo6SQL.Append("( ");

            ruleNo6SQL.Append(ucl2lcl2SQL.ToString());

            ruleNo6SQL.Append(") XX ");
            ruleNo6SQL.Append(") ZZ ");
            ruleNo6SQL.Append("WHERE 1 = 1 ");
            ruleNo6SQL.AppendFormat("AND RN >= {0} AND OOC_SUM >= {1} ", parameterInfo.NVALUE, parameterInfo.PARAVAL2);


            return ruleNo6SQL;
        }

        private StringBuilder MakeSqlByRuleNo7(string ruleNo, ParameterInfo parameterInfo)
        {
            StringBuilder ruleNo7SQL = new StringBuilder();

            ruleNo7SQL.Append(_insertStatement);
            ruleNo7SQL.Append(_rawDataWithStatement);

            ruleNo7SQL.Append(" SELECT SNAP_ID, DBID, INSTANCE_NUMBER, PARAMETERID, ");
            ruleNo7SQL.Append(" MEASURE_VALUE, TO_CHAR(BEGIN_INTERVAL_TIME, 'YYYYMMDDHH24MISSFF3') STARTTIMEKEY, TO_CHAR(END_INTERVAL_TIME, 'YYYYMMDDHH24MISSFF3') ENDTIMEKEY, ");
            ruleNo7SQL.AppendFormat("'{0}' RULENO, '{1}' MEASURE_DATE, ", ruleNo, _measureDate);
            ruleNo7SQL.Append("TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') INSERTTIME, 'DETECTING_BATCH' INSERTUSER, 'YES' ISALIVE ");
            ruleNo7SQL.Append("FROM ");
            ruleNo7SQL.Append("( ");

            ruleNo7SQL.Append("SELECT XX.*, ROW_NUMBER() OVER(PARTITION BY XX.DBID, XX.INSTANCE_NUMBER, XX.PARAMETERID, XX.DIFF_VAL ORDER BY XX.END_INTERVAL_TIME) CON_OOC_CNT ");
            ruleNo7SQL.Append("FROM ( ");
            ruleNo7SQL.Append("SELECT A.*, RN2, (RN - RN2) AS DIFF_VAL ");
            ruleNo7SQL.Append("FROM ");
            ruleNo7SQL.Append("(SELECT R.*, ROW_NUMBER() OVER(PARTITION BY R.DBID, R.INSTANCE_NUMBER, R.PARAMETERID ORDER BY R.END_INTERVAL_TIME) RN ");
            ruleNo7SQL.Append("FROM RAW_TABLE R ");
            ruleNo7SQL.Append(") A, ");
            ruleNo7SQL.Append("( ");
            ruleNo7SQL.Append("SELECT AA.*, ROW_NUMBER() OVER(PARTITION BY AA.DBID, AA.INSTANCE_NUMBER, AA.PARAMETERID ORDER BY AA.END_INTERVAL_TIME) RN2 ");
            ruleNo7SQL.Append("FROM ");
            ruleNo7SQL.Append("( ");
            ruleNo7SQL.Append("SELECT R.*, ");

            ruleNo7SQL.AppendFormat("({0} - ({1} * {2})) AS LCL2,", parameterInfo.TARGET, parameterInfo.STDVALUE, parameterInfo.MVALUE);
            ruleNo7SQL.AppendFormat("({0} + ({1} * {2})) AS UCL2 ", parameterInfo.TARGET, parameterInfo.STDVALUE, parameterInfo.MVALUE);

            ruleNo7SQL.Append("FROM RAW_TABLE R ");
            ruleNo7SQL.Append(") AA ");
            // M sigma 내에 위치
            ruleNo7SQL.Append("WHERE AA.MEASURE_VALUE > LCL2 AND AA.MEASURE_VALUE < UCL2 ");
            ruleNo7SQL.Append(") B ");
            ruleNo7SQL.Append("WHERE A.SNAP_ID = B.SNAP_ID(+) ");
            ruleNo7SQL.Append("AND A.DBID = B.DBID(+) ");
            ruleNo7SQL.Append("AND A.INSTANCE_NUMBER = B.INSTANCE_NUMBER(+) ");
            ruleNo7SQL.Append("AND A.PARAMETERID = B.PARAMETERID(+) ");
            ruleNo7SQL.Append(") XX ");
            ruleNo7SQL.Append("WHERE RN2 IS NOT NULL ");
            ruleNo7SQL.Append(") ");
            ruleNo7SQL.AppendFormat("WHERE CON_OOC_CNT >= {0} ", parameterInfo.NVALUE);


            return ruleNo7SQL;
        }

        private StringBuilder MakeSqlByRuleNo8(string ruleNo, ParameterInfo parameterInfo)
        {
            StringBuilder ruleNo8SQL = new StringBuilder();

            ruleNo8SQL.Append(_insertStatement);
            ruleNo8SQL.Append(_rawDataWithStatement);

            ruleNo8SQL.Append(" SELECT SNAP_ID, DBID, INSTANCE_NUMBER, PARAMETERID, ");
            ruleNo8SQL.Append(" MEASURE_VALUE, TO_CHAR(BEGIN_INTERVAL_TIME, 'YYYYMMDDHH24MISSFF3') STARTTIMEKEY, TO_CHAR(END_INTERVAL_TIME, 'YYYYMMDDHH24MISSFF3') ENDTIMEKEY, ");
            ruleNo8SQL.AppendFormat("'{0}' RULENO, '{1}' MEASURE_DATE, ", ruleNo, _measureDate);
            ruleNo8SQL.Append("TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') INSERTTIME, 'DETECTING_BATCH' INSERTUSER, 'YES' ISALIVE ");
            ruleNo8SQL.Append("FROM ");
            ruleNo8SQL.Append("( ");

            ruleNo8SQL.Append("SELECT XX.*, ROW_NUMBER() OVER(PARTITION BY XX.DBID, XX.INSTANCE_NUMBER, XX.PARAMETERID, XX.DIFF_VAL ORDER BY XX.END_INTERVAL_TIME) CON_OOC_CNT ");
            ruleNo8SQL.Append("FROM ( ");
            ruleNo8SQL.Append("SELECT A.*, RN2, (RN - RN2) AS DIFF_VAL ");
            ruleNo8SQL.Append("FROM ");
            ruleNo8SQL.Append("(SELECT R.*, ROW_NUMBER() OVER(PARTITION BY R.DBID, R.INSTANCE_NUMBER, R.PARAMETERID ORDER BY R.END_INTERVAL_TIME) RN ");
            ruleNo8SQL.Append("FROM RAW_TABLE R ");
            ruleNo8SQL.Append(") A, ");
            ruleNo8SQL.Append("( ");
            ruleNo8SQL.Append("SELECT AA.*, ROW_NUMBER() OVER(PARTITION BY AA.DBID, AA.INSTANCE_NUMBER, AA.PARAMETERID ORDER BY AA.END_INTERVAL_TIME) RN2 ");
            ruleNo8SQL.Append("FROM ");
            ruleNo8SQL.Append("( ");
            ruleNo8SQL.Append("SELECT R.*, ");

            ruleNo8SQL.AppendFormat("({0} - ({1} * {2})) AS LCL2,", parameterInfo.TARGET, parameterInfo.STDVALUE, parameterInfo.MVALUE);
            ruleNo8SQL.AppendFormat("({0} + ({1} * {2})) AS UCL2 ", parameterInfo.TARGET, parameterInfo.STDVALUE, parameterInfo.MVALUE);

            ruleNo8SQL.Append("FROM RAW_TABLE R ");
            ruleNo8SQL.Append(") AA ");
            // M sigma 밖에 위치
            ruleNo8SQL.Append("WHERE (AA.MEASURE_VALUE < LCL2 OR AA.MEASURE_VALUE > UCL2) ");
            ruleNo8SQL.Append(") B ");
            ruleNo8SQL.Append("WHERE A.SNAP_ID = B.SNAP_ID(+) ");
            ruleNo8SQL.Append("AND A.DBID = B.DBID(+) ");
            ruleNo8SQL.Append("AND A.INSTANCE_NUMBER = B.INSTANCE_NUMBER(+) ");
            ruleNo8SQL.Append("AND A.PARAMETERID = B.PARAMETERID(+) ");
            ruleNo8SQL.Append(") XX ");
            ruleNo8SQL.Append("WHERE RN2 IS NOT NULL ");
            ruleNo8SQL.Append(") ");
            ruleNo8SQL.AppendFormat("WHERE CON_OOC_CNT >= {0} ", parameterInfo.NVALUE);


            return ruleNo8SQL;
        }

        // 금일 Summary(RuleOut) 데이타 중에서
        // 어제 Summary(RuleOut) 데이타가 존재하면 삭제한다.
        private void DeleteForDuplicatedSummaryRows()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder deleteSQL = new StringBuilder();

            try
            {
                deleteSQL.Append("DELETE ISIA.TAPCTOUTOFCONTROLDATASUM TD ");
                deleteSQL.AppendFormat("WHERE MEASURE_DATE = '{0}' ", _measureDate);
                deleteSQL.Append("AND EXISTS (SELECT 1 ");
                deleteSQL.Append(" FROM ( ");
                deleteSQL.Append("SELECT B.* ");
                deleteSQL.Append("FROM TAPCTPARAMETERRULESPEC A, ");
                deleteSQL.Append("TAPCTOUTOFCONTROLDATASUM B ");
                deleteSQL.AppendFormat("WHERE B.MEASURE_DATE >= TO_CHAR(TO_DATE('{0}', 'YYYYMMDD') - A.DAYS, 'YYYYMMDD') ", _measureYesterDay);
                deleteSQL.AppendFormat("AND B.MEASURE_DATE <= '{0}' ", _measureYesterDay);
                deleteSQL.Append("AND B.DBID = A.DBID ");
                deleteSQL.Append("AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER ");
                deleteSQL.Append("AND B.PARAMETERID = A.PARAMETERID ");
                deleteSQL.Append("AND B.RULENO = A.RULENO ");
                deleteSQL.Append(") ");
                deleteSQL.Append("WHERE 1=1 ");
                deleteSQL.Append("AND DBID = TD.DBID ");
                deleteSQL.Append("AND INSTANCE_NUMBER = TD.INSTANCE_NUMBER ");
                deleteSQL.Append("AND PARAMETERID = TD.PARAMETERID ");
                deleteSQL.Append("AND RULENO = TD.RULENO ");
                deleteSQL.Append("AND SNAP_ID = TD.SNAP_ID) ");

                int resultCount = db.Save(new string[] { deleteSQL.ToString() });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        // 금일 Summary(RuleOut) 데이타 중에서
        // 어제 Summary(RuleOut) 데이타가 존재하면 삭제한다.
        // 특정 Paramter 만 삭제
        private void DeleteForDuplicatedSummaryRows(String ParamterId)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder deleteSQL = new StringBuilder();

            try
            {
                deleteSQL.Append("DELETE ISIA.TAPCTOUTOFCONTROLDATASUM TD ");
                deleteSQL.AppendFormat("WHERE MEASURE_DATE = '{0}' ", _measureDate);
                deleteSQL.AppendFormat("AND PARAMETERID = '{0}' ", ParamterId);
                deleteSQL.Append("AND EXISTS (SELECT 1 ");
                deleteSQL.Append(" FROM ( ");
                deleteSQL.Append("SELECT B.* ");
                deleteSQL.Append("FROM TAPCTPARAMETERRULESPEC A, ");
                deleteSQL.Append("TAPCTOUTOFCONTROLDATASUM B ");
                deleteSQL.AppendFormat("WHERE B.MEASURE_DATE >= TO_CHAR(TO_DATE('{0}', 'YYYYMMDD') - A.DAYS, 'YYYYMMDD') ", _measureYesterDay);
                deleteSQL.AppendFormat("AND B.MEASURE_DATE <= '{0}' ", _measureYesterDay);
                deleteSQL.Append("AND B.DBID = A.DBID ");
                deleteSQL.Append("AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER ");
                deleteSQL.Append("AND B.PARAMETERID = A.PARAMETERID ");
                deleteSQL.Append("AND B.RULENO = A.RULENO ");
                deleteSQL.Append(") ");
                deleteSQL.Append("WHERE 1=1 ");
                deleteSQL.Append("AND DBID = TD.DBID ");
                deleteSQL.Append("AND INSTANCE_NUMBER = TD.INSTANCE_NUMBER ");
                deleteSQL.Append("AND PARAMETERID = TD.PARAMETERID ");
                deleteSQL.Append("AND RULENO = TD.RULENO ");
                deleteSQL.Append("AND SNAP_ID = TD.SNAP_ID) ");

                int resultCount = db.Save(new string[] { deleteSQL.ToString() });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private static String ReturnZeroIfNull(String value)
        {
            if (value == null)
                return "0";
            return value;
        }

        private DataTable GetmailData()
        {
            DBCommunicator db = new DBCommunicator();
            DataTable returnDt = new DataTable();
            StringBuilder selectSQL = new StringBuilder();

            try
            {
                //selectSQL.Append("SELECT ROWNUM RN, ");
                selectSQL.Append("SELECT ");
                selectSQL.Append("DBID, INSTANCE_NUMBER, PARAMETERID, ");
                selectSQL.Append("(SELECT PARAMETERNAME FROM TAPCTPARAMETERDEF WHERE PARAMETERID = TD.PARAMETERID) PARAMETERNAME, ");
                selectSQL.Append("RULENO, ");
                selectSQL.Append("(SELECT RULENAME FROM TAPCTSPCRULESPEC WHERE RULENO = TD.RULENO) RULENAME,  ");
                selectSQL.Append("(SELECT RULETEXT FROM TAPCTSPCRULESPEC WHERE RULENO = TD.RULENO) RULETEXT, ");
                selectSQL.Append("SNAP_ID, MEASURE_DATE,  ");
                selectSQL.Append("STARTTIMEKEY, ENDTIMEKEY, MEASURE_VAL,  ");
                selectSQL.Append("COMMENTS ");
                selectSQL.Append("FROM ISIA.TAPCTOUTOFCONTROLDATASUM TD ");
                selectSQL.AppendFormat("WHERE MEASURE_DATE = '{0}' ", _measureDate);
                selectSQL.Append("AND EXISTS (SELECT 1 ");
                selectSQL.Append("FROM TAPCTPARAMETERRULESPEC ");
                selectSQL.Append("WHERE MAILUSED = 'YES' ");
                selectSQL.Append("AND DBID = TD.DBID ");
                selectSQL.Append("AND INSTANCE_NUMBER = TD.INSTANCE_NUMBER ");
                selectSQL.Append("AND PARAMETERID = TD.PARAMETERID ");
                selectSQL.Append("AND RULENO = TD.RULENO) ");

                returnDt = db.Select(selectSQL.ToString()).Tables[0];


                return returnDt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetmailDataForEliminateDuplicatedRows()
        {
            DBCommunicator db = new DBCommunicator();
            DataTable returnDt = new DataTable();
            StringBuilder selectSQL = new StringBuilder();

            try
            {
                selectSQL.Append("WITH YD AS ( ");
                selectSQL.Append("SELECT B.* ");
                selectSQL.Append("FROM TAPCTPARAMETERRULESPEC A, ");
                selectSQL.Append("TAPCTOUTOFCONTROLDATASUM B ");
                selectSQL.AppendFormat("WHERE B.MEASURE_DATE >= TO_CHAR(TO_DATE('{0}', 'YYYYMMDD') - A.DAYS, 'YYYYMMDD') ", _measureYesterDay);
                selectSQL.AppendFormat("AND B.MEASURE_DATE <= '{0}' ", _measureYesterDay);
                selectSQL.Append("AND B.DBID = A.DBID ");
                selectSQL.Append("AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER ");
                selectSQL.Append("AND B.PARAMETERID = A.PARAMETERID ");
                selectSQL.Append("AND B.RULENO = A.RULENO ");
                selectSQL.Append(") ");
                //selectSQL.Append("SELECT ROWNUM RN, ");
                selectSQL.Append("SELECT ");
                selectSQL.Append("DBID, INSTANCE_NUMBER, PARAMETERID, ");
                selectSQL.Append("(SELECT PARAMETERNAME FROM TAPCTPARAMETERDEF WHERE PARAMETERID = TD.PARAMETERID) PARAMETERNAME, ");
                selectSQL.Append("RULENO, ");
                selectSQL.Append("(SELECT RULENAME FROM TAPCTSPCRULESPEC WHERE RULENO = TD.RULENO) RULENAME,  ");
                selectSQL.Append("(SELECT RULETEXT FROM TAPCTSPCRULESPEC WHERE RULENO = TD.RULENO) RULETEXT, ");
                selectSQL.Append("SNAP_ID, MEASURE_DATE,  ");
                selectSQL.Append("STARTTIMEKEY, ENDTIMEKEY, MEASURE_VAL,  ");
                selectSQL.Append("COMMENTS ");
                selectSQL.Append("FROM ISIA.TAPCTOUTOFCONTROLDATASUM TD ");
                selectSQL.AppendFormat("WHERE MEASURE_DATE = '{0}' ", _measureDate);
                selectSQL.Append("AND EXISTS (SELECT 1 ");
                selectSQL.Append("FROM TAPCTPARAMETERRULESPEC ");
                selectSQL.Append("WHERE MAILUSED = 'YES' ");
                selectSQL.Append("AND DBID = TD.DBID ");
                selectSQL.Append("AND INSTANCE_NUMBER = TD.INSTANCE_NUMBER ");
                selectSQL.Append("AND PARAMETERID = TD.PARAMETERID ");
                selectSQL.Append("AND RULENO = TD.RULENO) ");
                selectSQL.Append("AND NOT EXISTS (SELECT 1 ");
                selectSQL.Append("FROM  YD ");
                selectSQL.Append("WHERE 1 = 1 ");
                selectSQL.Append("AND DBID = TD.DBID ");
                selectSQL.Append("AND INSTANCE_NUMBER = TD.INSTANCE_NUMBER ");
                selectSQL.Append("AND PARAMETERID = TD.PARAMETERID ");
                selectSQL.Append("AND RULENO = TD.RULENO ");
                selectSQL.Append("AND SNAP_ID = TD.SNAP_ID) ");

                returnDt = db.Select(selectSQL.ToString()).Tables[0];


                return returnDt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void SendMail()
        {
            // 메일 그룹별 메일 수신 리스트 가져와서 처리 하는 부분은
            // 추후 테이블 생성되면 구현 예정
            
            DataTable mailGroupDt = GetMailGroup();
            mailGroupDt.DefaultView.RowFilter = "MAILADDRESS <> '' AND MAILADDRESS IS NOT NULL";
            DataTable tmpdt = mailGroupDt.DefaultView.ToTable();
            Mail mail = new TAP.Base.Communication.Mail();
            var mailGroupList = (from d in tmpdt.AsEnumerable() select d.Field<string>("MAILADDRESS")).ToList();


            DataTable mailData;

            if (_isDuplicatedAllow.Equals("TRUE"))
            {
                mailData = GetmailData();
            }
            else
            {
                mailData = GetmailDataForEliminateDuplicatedRows();
            }
            StringBuilder sbMailBody = SetMailForm(mailData);

            String bodyHTML = RuleOutReportMailBody();
            String messageBody = bodyHTML.Replace("<!--BODY-->", sbMailBody.ToString());

            int cnt = mailGroupList.Count;
            List<string> tmpmaillist;
            for (int i = 0; i <= cnt; i += _MailCount)
            {
                if (i <= cnt - _MailCount)
                {
                    tmpmaillist = mailGroupList.GetRange(i, _MailCount);

                }
                else
                {
                    tmpmaillist = mailGroupList.GetRange(i, cnt - i);
                }

                if (tmpmaillist.Count > 0)
                {
                    mail.SendMail("SPC OOC DETECTING DATA - " + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), "DETECTING_MAIL_SERVICE", messageBody, tmpmaillist);
                }

            }
        }
        private DataTable GetMailGroup()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpsb = new StringBuilder();

            tmpsb.Append("SELECT  MAILADDRESS FROM TAPUTUSERS U,(");
            tmpsb.Append(" SELECT USERID, USERNAME FROM TAPUTMAILGROUP G, TAPUTMAILGROUPMEMBER N WHERE G.NAME = N.GROUPNAME  AND G.REGION = N.REGION AND G.NAME in (");
            tmpsb.Append(" SELECT NAME FROM TAPCTCODES WHERE CATEGORY = 'MAIL' AND SUBCATEGORY = 'SPC_DETECT' AND ISALIVE = 'YES') )GN WHERE U.NAME = GN.USERID");

            DataTable mailGroupDt = db.Select(tmpsb.ToString()).Tables[0];

            //mailGroupDt.Rows.Add(new object[] { "syg823@iset-da.com" });
            return mailGroupDt;
        }

        private StringBuilder SetMailForm(DataTable mailData)
        {
            StringBuilder sbMailBody = new StringBuilder();
            StringBuilder sbMailList = new StringBuilder();

            int dtIdx = 0;

            StringBuilder sImagePath = new StringBuilder();
            foreach (DataRow drTemp in mailData.Rows)
            {
                dtIdx++;

                string beforeParack = "style='background-color: #FFCBCB';";

                #region -------- dataList 셋팅

                if (dtIdx % 2 == 0)
                    sbMailList.AppendFormat("	<tr {0}>", beforeParack);
                else
                    sbMailList.AppendFormat("	<tr class=\"alt\" >");


                sbMailList.AppendFormat("	<tr {0}>", beforeParack);
                sbMailList.AppendFormat("		<td align=\"center\">{0}</td>", dtIdx);
                sbMailList.AppendFormat("		<td align=\"center\">{0}</td>", drTemp["DBID"]);
                sbMailList.AppendFormat("		<td align=\"center\">{0}</td>", drTemp["INSTANCE_NUMBER"]);
                sbMailList.AppendFormat("		<td align=\"center\">{0}</td>", drTemp["PARAMETERID"]);
                sbMailList.AppendFormat("		<td align=\"center\">{0}</td>", drTemp["PARAMETERNAME"]);
                sbMailList.AppendFormat("		<td align=\"center\">{0}</td>", drTemp["RULENO"]);
                sbMailList.AppendFormat("		<td align=\"center\">{0}</td>", drTemp["RULENAME"]);
                sbMailList.AppendFormat("		<td align=\"center\">{0}</td>", drTemp["RULETEXT"]);
                sbMailList.AppendFormat("		<td align=\"center\">{0}</td>", drTemp["SNAP_ID"]);
                sbMailList.AppendFormat("		<td align=\"center\">{0}</td>", drTemp["MEASURE_DATE"]);
                sbMailList.AppendFormat("		<td align=\"center\">{0}</td>", drTemp["STARTTIMEKEY"]);
                sbMailList.AppendFormat("		<td align=\"center\">{0}</td>", drTemp["ENDTIMEKEY"]);
                sbMailList.AppendFormat("		<td align=\"center\">{0}</td>", drTemp["MEASURE_VAL"]);
                sbMailList.AppendFormat("		<td align=\"center\">{0}</td>", drTemp["COMMENTS"]);
                sbMailList.AppendFormat("	</tr>");
                #endregion
            }
            #region -------mail form
            sbMailBody.AppendFormat("<center><font size=\"2\"><b><u>SPC RULEOUT Report</u></b></font></center>");
            sbMailBody.AppendFormat("<br>");
            sbMailBody.AppendFormat("<font size=\"2\"><b>&lt;RULE OUT Summary List&gt;</b></font>");
            sbMailBody.AppendFormat("<table>");
            sbMailBody.AppendFormat("	<tr>");
            sbMailBody.AppendFormat("		<th style=\"width:40px;\">No</th>");
            sbMailBody.AppendFormat("		<th style=\"width:60px;\">DBID</th>");
            sbMailBody.AppendFormat("		<th style=\"width:90px;\">INSTANCE_NUMBER</th>");
            sbMailBody.AppendFormat("		<th style=\"width:90px;\">PARAMETERID</th>");
            sbMailBody.AppendFormat("		<th style=\"width:140px;\">PARAMETERNAME</th>");
            sbMailBody.AppendFormat("		<th style=\"width:40px;\">RULENO</th>");
            sbMailBody.AppendFormat("		<th style=\"width:90px;\">RULENAME</th>");
            sbMailBody.AppendFormat("		<th style=\"width:140px;\">RULETEXT</th>");
            sbMailBody.AppendFormat("		<th style=\"width:90px;\">SNAP_ID</th>");
            sbMailBody.AppendFormat("		<th style=\"width:60px;\">MEASURE_DATE</th>");
            sbMailBody.AppendFormat("		<th style=\"width:90px;\">STARTTIMEKEY</th>");
            sbMailBody.AppendFormat("		<th style=\"width:90px;\">ENDTIMEKEY</th>");
            sbMailBody.AppendFormat("		<th style=\"width:60px;\">MEASURE_VAL</th>");
            sbMailBody.AppendFormat("		<th style=\"width:200px;\">COMMENTS</th>");
            sbMailBody.AppendFormat("	</tr>");

            sbMailBody.Append(sbMailList);

            sbMailBody.AppendFormat("</table>	");

            #endregion

            return sbMailBody;
        }

        private string RuleOutReportMailBody()
        {
            StringBuilder mailBody = new StringBuilder();

            mailBody.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\"> ");
            mailBody.AppendLine("<html> ");
            mailBody.AppendLine("<head> ");
            mailBody.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"> ");
            mailBody.AppendLine("<style> ");
            mailBody.AppendLine(" ");
            mailBody.AppendLine("table ");
            mailBody.AppendLine("{ ");
            mailBody.AppendLine("    border-collapse:collapse; ");
            mailBody.AppendLine("    border: 1px black solid; ");
            mailBody.AppendLine("    font-size: 11px; ");
            mailBody.AppendLine("} ");
            mailBody.AppendLine(" ");
            mailBody.AppendLine("th ");
            mailBody.AppendLine("{ ");
            mailBody.AppendLine("    background-color: #FFFFCC; ");
            mailBody.AppendLine("    border-bottom: 1px black solid; ");
            mailBody.AppendLine("    border-right: 1px gray solid; ");
            mailBody.AppendLine("    font-size: 11px; ");
            mailBody.AppendLine("} ");
            mailBody.AppendLine(" ");
            mailBody.AppendLine("td ");
            mailBody.AppendLine("{ ");
            mailBody.AppendLine("    border-right: 1px gray solid; ");
            mailBody.AppendLine("    border-bottom: 1px gray solid; ");
            mailBody.AppendLine("}  ");
            mailBody.AppendLine(" ");
            mailBody.AppendLine(".alt ");
            mailBody.AppendLine("{ ");
            mailBody.AppendLine("    background-color: #EEEEEE; ");
            mailBody.AppendLine("} ");
            mailBody.AppendLine(" ");
            mailBody.AppendLine("</style> ");
            mailBody.AppendLine("<title></title> ");
            mailBody.AppendLine(" ");
            mailBody.AppendLine("</head> ");
            mailBody.AppendLine("<body> ");
            mailBody.AppendLine("<!--BODY-->  ");
            mailBody.AppendLine("</body> ");
            mailBody.AppendLine("</html> ");

            return mailBody.ToString();
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
