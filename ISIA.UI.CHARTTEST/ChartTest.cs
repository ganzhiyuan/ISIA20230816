using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.UI;
using TAP.Data.DataBase.Communicators;
using Steema.TeeChart;
using DevExpress.XtraLayout.Customization;
using Steema.TeeChart.Export;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;
using static DevExpress.XtraEditors.RoundedSkinPanel;
using DevExpress.XtraReports.Parameters;

namespace ISIA.UI.CHARTTEST
{
    public partial class ChartTest : UIBase
    {
        #region Const

        private const string _DBA_HIST_SYSMETRIC_SUMMARY = "DBA_HIST_SYSMETRIC_SUMMARY";
        private const string _DBA_HIST_SYSSTAT = "DBA_HIST_SYSSTAT";
        private const string _STATISTIC = "STATISTIC";
        private const string _METRIC = "METRIC";
        private const string _RAW = "RAW_";

        #endregion


        public ChartTest()
        {
            InitializeComponent();
        }

        private TChart ChartDesign(TChart chart)
        {
            //차트 디자인 하기.
            //1. 기본 디자인
            //2. 차트 사이즈

            //X축 Lable Format
            chart.Axes.Bottom.Labels.DateTimeFormat = "yyyy-MM-dd";

            //Chart SIZE
            //this.tChart1.Size = new System.Drawing.Size(662, 388);

            //chart.

            return chart;
        }

        private DataTable GetParameterInfo()
        {

            DBCommunicator db = new DBCommunicator();
            DataTable returnDt = new DataTable();
            StringBuilder selectSQL = new StringBuilder();
            
            try
            {
                //데이터가 있는 DB명을 찾음.
                selectSQL.Append("SELECT B.DBNAME, A.* FROM TAPCTPARAMETERRULESPEC A, TAPCTDATABASE B, TAPCTPARAMETERDEF C ");
                selectSQL.Append("WHERE  A.DBID = B.DBID(+) AND A.PARAMETERID = C.PARAMETERID(+) AND ");
                selectSQL.Append("A.CHARTUSED = 'YES' AND A.ISALIVE = 'YES' ");

                returnDt = db.Select(selectSQL.ToString()).Tables[0];

                return returnDt;
            }
            catch(System.Exception ex)
            {                
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
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
                string startDate = DateTime.Now.AddDays(0-int.Parse(days)).ToString("yyyyMMdd");
                string endDate = DateTime.Now.ToString("yyyyMMdd");
                
                //shiftTime 은 Config에서 가져오게 해야함.
                string startShiftTime = "073000";
                string endShiftTime = "073000";

                //parameter 별로 가져오는 테이블 및 컬럼이 다를 수 있음.
                //추후 변경시 수정 해야 함. 2023-03-16 kimseoil
                if (parameterType.Contains(_STATISTIC))
                {
                    selectSQL.AppendFormat("SELECT D.SNAP_ID, D.DBID, D.INSTANCE_NUMBER, D.STAT_ID AS PARAMETERID, D.STAT_NAME AS PARAMETERNAME, VALUE, BEGIN_INTERVAL_TIME, END_INTERVAL_TIME FROM {0} D, RAW_DBA_HIST_SNAPSHOT_ISFA T WHERE D.SNAP_ID = T.SNAP_ID ", tableName);
                    selectSQL.Append("AND D.DBID = T.DBID AND D.INSTANCE_NUMBER = T.INSTANCE_NUMBER ");
                    //아래부터 조건들
                    selectSQL.AppendFormat("AND T.DBID = {0} AND T.INSTANCE_NUMBER = {1} AND T.END_INTERVAL_TIME >= '{2}' AND T.END_INTERVAL_TIME < '{3}' ", dbID, instance_number, startDate+startShiftTime, endDate+endShiftTime);
                    selectSQL.AppendFormat("AND D.STAT_ID = {0} ", parameterId);
                    selectSQL.Append("ORDER BY D.SNAP_ID ");
                }
                else if(parameterType.Contains(_METRIC))
                {
                    selectSQL.AppendFormat("SELECT D.SNAP_ID, D.DBID, D.INSTANCE_NUMBER, D.METRIC_ID AS PARAMETERID, D.METRIC_NAME AS PARAMETERNAME, MAXVAL AS VALUE, BEGIN_INTERVAL_TIME, END_INTERVAL_TIME FROM {0} D, RAW_DBA_HIST_SNAPSHOT_ISFA T WHERE D.SNAP_ID = T.SNAP_ID ", tableName);
                    selectSQL.Append("AND D.DBID = T.DBID AND D.INSTANCE_NUMBER = T.INSTANCE_NUMBER ");
                    //아래부터 조건들
                    selectSQL.AppendFormat("AND T.DBID = {0} AND T.INSTANCE_NUMBER = {1} AND T.END_INTERVAL_TIME >= '{2}' AND T.END_INTERVAL_TIME < '{3}' ", dbID, instance_number, startDate + startShiftTime, endDate + endShiftTime);
                    selectSQL.AppendFormat("AND D.METRIC_ID = {0} ", parameterId);
                    selectSQL.Append("ORDER BY D.SNAP_ID ");
                }

                returnDt = db.Select(selectSQL.ToString()).Tables[0];

                return returnDt;
            }
            catch (System.Exception ex) 
            { 
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());                
                throw ex;
            }
        }


        //DB TABLE을 찾는 기능
        private string MakeRawDataTableName(string type, string dbName)
        {
            string returnStr = string.Empty;

            try
            {
                if(type.Contains(_STATISTIC))
                {
                    returnStr = _RAW + _DBA_HIST_SYSSTAT + "_"+ dbName;
                }
                else if(type.Contains(_METRIC))
                {
                    returnStr = _RAW + _DBA_HIST_SYSMETRIC_SUMMARY + "_" + dbName;
                }

                return returnStr;
            }
            catch(System.Exception ex)
            {
                throw ex;
            }
        }

        private void MakeChart(TChart chart, DataTable dataTable)
        {
            



                //Line line = new Line();
                //line.DataSource = dt;
                //line.YValues.DataMember = dt.TableName;
                //line.XValues.DataMember = "BEGIN_TIME";
                //line.ShowInLegend = true;
                //line.Legend.Text = dt.TableName;
                //line.Title = dt.TableName;
                //line.Color =
                //    System.Drawing.Color.FromArgb(color.R1, color.G1, color.S1);
                //line.Legend.BorderRound = 20;
                //line.XValues.DateTime = true;

            //chart.Series.Add(line);
        }


        private void ChangeChartTitle(TChart chart, string title)
        {
            try
            {
                chart.Header.Lines = new string[] { title };
                chart.Header.Font.Name = "Tahoma";
                chart.Header.Font.Color = Color.Black;
                chart.Header.Font.Size = 10;
                chart.Header.TextAlign = StringAlignment.Center;
                chart.Header.Alignment = StringAlignment.Center;
            }
            catch(System.Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
            }
        }
        
        private void SaveChartImage(TChart chart, string filepath)
        {
            try
            {
                //Chart 저장 이미지 크기는 tChart의 Size를 조정해서 처리.
                Steema.TeeChart.Export.ImageExport export = new ImageExport(chart.Chart);
                PNGFormat png = export.PNG;

                png.Save(filepath);
            }
            catch(System.Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
            }
        }

        private void ChartServiceRun()
        {
            //테스트 완료 후 비동기 방식으로 변경.

            DataTable parameterSpec = new DataTable();

            try
            {
                ChartDesign(tChart1);

                parameterSpec = GetParameterInfo();

                foreach (DataRow dataRow in parameterSpec.Rows)
                {
                    ParameterInfo parameterInfo = new ParameterInfo(dataRow);

                    DataTable rawDatatable = GetRawData(parameterInfo);







                }


                //ChangeChartTitle(tChart1, )
                    }
            catch(System.Exception ex)
            {
                
            }

        }

        #region Event
        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                this.ChartServiceRun();
            }
            catch(System.Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
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

        public ParameterInfo()
        {
            DBNAME = string.Empty;
            
        }
        public ParameterInfo(DataRow dataRow)
        {
            foreach(DataColumn dc in dataRow.Table.Columns)
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



    }


}
