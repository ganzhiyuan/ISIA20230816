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
using Steema.TeeChart.Styles;
using Steema.TeeChart;
using System.Drawing;
using System.IO;
using TAP.WinService;

namespace ISIA.CHART.SERVICE
{
    class ChartService : TAP.WinService.ServiceBase
    {
        #region Const

        private const string _DBA_HIST_SYSMETRIC_SUMMARY = "DBA_HIST_SYSMETRIC_SUMMARY";
        private const string _DBA_HIST_SYSSTAT = "DBA_HIST_SYSSTAT";
        private const string _DBA_HIST_SNAPSHOT = "DBA_HIST_SNAPSHOT";
        private const string _STATISTIC = "STATISTIC";
        private const string _METRIC = "METRIC";
        private const string _RAW = "RAW_";
        private const string _SPECUPPER = "SPEC UPPER LIMIT";
        private const string _SPECLOWER = "SPEC LOWER LIMIT";
        private const string _CONTROLUPPER = "CONTROL UPPER LIMIT";
        private const string _CONTROLLOWER = "CONTROL LOWER LIMIT";
        private const string _CHART_SERVICE = "CHART SERVICE";

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

        private TChartHelper chartHelper = new TChartHelper();

        private TChart chart = new TChart();

        #endregion

        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            ChartServiceRun();
        }

        #region DataLoad Methods

        private DataTable GetParameterInfo()
        {

            DBCommunicator db = new DBCommunicator();            
            DataTable returnDt = new DataTable();
            StringBuilder selectSQL = new StringBuilder();

            try
            {
                //데이터가 있는 DB명을 찾음.
                selectSQL.Append("SELECT B.DBNAME, C.PARAMETERTYPE, A.* FROM TAPCTPARAMETERRULESPEC A, TAPCTDATABASE B, TAPCTPARAMETERDEF C ");
                selectSQL.Append("WHERE  A.DBID = B.DBID(+) AND A.PARAMETERID = C.PARAMETERID(+) AND ");
                selectSQL.Append("A.CHARTUSED = 'YES' AND A.ISALIVE = 'YES' ");

                returnDt = db.Select(selectSQL.ToString()).Tables[0];

                return returnDt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
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

        private void SaveChartImageData(ParameterInfo parameterInfo, string path)
        {
            DBCommunicator db = new DBCommunicator();            
            DataTable returnDt = new DataTable();
            StringBuilder selectSQL = new StringBuilder();

            //DETECTIONFLAG 는 성원경부장님이 작업 한거에 따라 DB를 참고하여 처리.
            string defectionflag = "NO";

            try
            {
                selectSQL.Append("INSERT INTO ISIA.TAPAWRCHARTSERVICE(");
                selectSQL.Append("DBID, INSTANCE_NUMBER, REPORTDATE, ");
                selectSQL.Append("PARAMETERID, PARAMETERNAME, RULENAME, ");
                selectSQL.Append("RULENO, STARTTIMEKEY, ENDTIMEKEY, ");
                selectSQL.Append("IMAGEPATH, DETECTIONFLAG, UPDATETIME, ");
                selectSQL.Append("UPDATEUSER, INSERTTIME, INSERTUSER, ");
                selectSQL.Append("SEQUENCES, ISALIVE) ");
                selectSQL.AppendFormat("VALUES( {0}, {1}, '{2}',", parameterInfo.DBID, parameterInfo.INSTANCE_NUMBER, DateTime.Now.ToString("yyyyMMdd"));
                selectSQL.AppendFormat(" {0}, '{1}', '{2}',", parameterInfo.PARAMETERID, parameterInfo.PARAMETERNAME, parameterInfo.RULENAME);
                selectSQL.AppendFormat(" {0}, '{1}', '{2}',", parameterInfo.RULENO, _startTimeKey, _endTimeKey);
                selectSQL.AppendFormat(" '{0}', '{1}', '{2}',", path, defectionflag, DateTime.Now.ToString("yyyyMMddHHmmss"));
                selectSQL.AppendFormat(" '{0}', '{1}', '{2}',", _CHART_SERVICE, DateTime.Now.ToString("yyyyMMddHHmmss"), _CHART_SERVICE);
                selectSQL.AppendFormat(" {0}, '{1}') ", "0", "YES");

                int resultCount = db.Save(new string[] { selectSQL.ToString() });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        private string MakeDataBaseInsetFileName(string filename)
        {
            string path = string.Empty;
            try
            {
                path = string.Format(@"{0}/{1}/", DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("dd"));

                return path + filename;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

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

        private void MakeChart(TChart chart, DataTable dataTable)
        {

            Line line = new Line();
            line.DataSource = dataTable;
            line.YValues.DataMember = "MEASURE_VALUE";
            line.XValues.DataMember = "END_INTERVAL_TIME";
            line.Legend.Visible = false;
            line.XValues.DateTime = true;
            line.LinePen.Width = 3;

            //MARK VISIBLE
            //line.Pointer.Visible = true;

            chart.Series.Add(line);

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
        private void ChartServiceRun()
        {
            //테스트 완료 후 비동기 방식으로 변경.

            DataTable parameterSpec = new DataTable();
            
            try
            {

                int imageWidth = int.Parse(TAP.App.Base.AppConfig.ConfigManager.HostCollection["IMAGESIZE"]["Width"].ToString());
                int imageHeight = int.Parse(TAP.App.Base.AppConfig.ConfigManager.HostCollection["IMAGESIZE"]["Height"].ToString());


                parameterSpec = GetParameterInfo();

                foreach (DataRow dataRow in parameterSpec.Rows)
                {
                    ParameterInfo parameterInfo = new ParameterInfo(dataRow);

                    DataTable rawDatatable = GetRawData(parameterInfo);

                    //초기화 및 디자인
                    chartHelper.ChartDesign(chart);

                    //요구사항에따라 TITLE 변경하기
                    chartHelper.ChangeChartTitle(chart, parameterInfo.PARAMETERNAME);

                    MakeChart(chart, rawDatatable);

                    #region Create Limit Line
                    if (parameterInfo.SPECLIMITUSED.Contains("YES"))
                    {
                        if (parameterInfo.SPECUPPERLIMIT != null)
                        {
                            chartHelper.ChartYLimitLine(chart, _SPECUPPER, Color.Red, double.Parse(parameterInfo.SPECUPPERLIMIT), 2);
                        }
                        if (parameterInfo.SPECLOWERLIMIT != null)
                        {
                            chartHelper.ChartYLimitLine(chart, _SPECLOWER, Color.Red, double.Parse(parameterInfo.SPECLOWERLIMIT), 2);
                        }
                    }
                    else
                    {
                        if (parameterInfo.CONTROLUPPERLIMIT != null)
                        {
                            chartHelper.ChartYLimitLine(chart, _CONTROLUPPER, Color.Red, double.Parse(parameterInfo.CONTROLUPPERLIMIT), 2);
                        }
                        if (parameterInfo.CONTROLLOWERLIMIT != null)
                        {
                            chartHelper.ChartYLimitLine(chart, _CONTROLLOWER, Color.Red, double.Parse(parameterInfo.CONTROLLOWERLIMIT), 2);
                        }
                    }
                    #endregion


                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + parameterInfo.PARAMETERNAME + ".png";

                    //IMAGE NAME 정의 해야 함. 저장.
                    _imageFileName = MakeFilePath(fileName);

                    chartHelper.SaveChartImage(chart, _imageFileName, imageWidth, imageHeight);

                    //ImageFile 경로 수정 필요함. FTP 경로.

                    SaveChartImageData(parameterInfo, MakeDataBaseInsetFileName(fileName));
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.SaveLog(ServiceBase.ERROR_LOG, ServiceName, ex.ToString());
            }

        }

        #endregion

    }


    public class ParameterInfo
    {

        #region const
        private const string _DBNAME = "DBNAME";
        private const string _PARAMETERTYPE = "PARAMETERTYPE";
        private const string _DBID = "DBID";
        private const string _INSTANCE_NUMBER = "INSTANCE_NUMBER";
        private const string _PARAMETERID = "PARAMETERID";
        private const string _PARAMETERNAME = "PARAMETERNAME";
        private const string _RULENAME = "RULENAME";
        private const string _RULENO = "RULENO";
        private const string _DAYS = "DAYS";
        private const string _SPECUPPERLIMIT = "SPECUPPERLIMIT";
        private const string _SPECLOWERLIMIT = "SPECLOWERLIMIT";
        private const string _CONTROLUPPERLIMIT = "CONTROLUPPERLIMIT";
        private const string _CONTROLLOWERLIMIT = "CONTROLLOWERLIMIT";
        private const string _CHARTUSED = "CHARTUSED";
        private const string _MAILUSED = "MAILUSED";
        private const string _MMSUSED = "MMSUSED";
        private const string _SPECLIMITUSED = "SPECLIMITUSED";
        #endregion

        #region Properties

        public string DBNAME { get; set; }
        public string DBID { get; set; }
        public string INSTANCE_NUMBER { get; set; }
        public string PARAMETERID { get; set; }
        public string PARAMETERTYPE { get; set; }
        public string PARAMETERNAME { get; set; }
        public string RULENAME { get; set; }
        public string RULENO { get; set; }
        public string DAYS { get; set; }
        public string SPECUPPERLIMIT { get; set; }
        public string SPECLOWERLIMIT { get; set; }
        public string CONTROLUPPERLIMIT { get; set; }
        public string CONTROLLOWERLIMIT { get; set; }
        public string CHARTUSED { get; set; }
        public string MAILUSED { get; set; }
        public string MMSUSED { get; set; }
        public string SPECLIMITUSED { get; set; }

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
                    case _DBNAME:
                        DBNAME = dataRow[_DBNAME].ToString();
                        break;
                    case _DBID:
                        DBID = dataRow[_DBID].ToString();
                        break;
                    case _INSTANCE_NUMBER:
                        INSTANCE_NUMBER = dataRow[_INSTANCE_NUMBER].ToString();
                        break;
                    case _PARAMETERID:
                        PARAMETERID = dataRow[_PARAMETERID].ToString();
                        break;
                    case _PARAMETERTYPE:
                        PARAMETERTYPE = dataRow[_PARAMETERTYPE].ToString();
                        break;
                    case _PARAMETERNAME:
                        PARAMETERNAME = dataRow[_PARAMETERNAME].ToString();
                        break;
                    case _RULENAME:
                        RULENAME = dataRow[_RULENAME].ToString();
                        break;
                    case _RULENO:
                        RULENO = dataRow[_RULENO].ToString();
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
                }
            }
        }

        #endregion
    }
}
