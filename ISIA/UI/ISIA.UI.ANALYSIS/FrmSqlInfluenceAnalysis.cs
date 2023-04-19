using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.UI;
using ISIA.UI.BASE;
using Steema.TeeChart;
using System.Xml;
using Steema.TeeChart.Tools;
using Steema.TeeChart.Styles;
using DevExpress.XtraEditors.Controls;
using System.Linq;
using Analysis.Correlation;
using DevExpress.XtraGrid.Views.Printing;
using System.Drawing;
using TAP;
using System.Collections;

namespace ISIA.UI.ANALYSIS
{
    public partial class FrmSqlInfluenceAnalysis : DockUIBase1T1
    {

        TChart currentChart = null;

        //define bs
        BizDataClient bs = null;

        AwrArgsPack argument = new AwrArgsPack();


        #region getset
        public BizDataClient Bs { get => bs; set => bs = value; }
        #endregion

        public FrmSqlInfluenceAnalysis()
        {
            InitializeComponent();
            Bs = new BizDataClient("ISIA.BIZ.ANALYSIS.DLL", "ISIA.BIZ.ANALYSIS.SqlInfluenceAnalysis");
            this.InitializeControls();
            dtpStartTime.DateTime = DateTime.Now.AddDays(-1);
            dtpEndTime.DateTime = DateTime.Now;
            //initialize bs
        }

        #region event
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                this.AsyncGetWorkloadAndSqlComparisonData();
            }
            catch (Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(TAP.UI.EnumMsgType.CONFIRM, ex.Message);
            }
        }

        public override void ExecuteCommand(ArgumentPack arguments)
        {
            foreach (string tmpstr in arguments.ArgumentNames)
            {
                if (tmpstr == "_hashTable")
                {
                    Hashtable hashtable = (Hashtable)arguments["_hashTable"].ArgumentValue;
                    DataSet ds = (DataSet)hashtable["DS"];
                    argument.WorkloadSqlParm  = (string)hashtable["SQL_PARM"];
                    awrArgsPack = new AwrArgsPack();
                    DisplayChart(ds);
                }
            }
        }

        #endregion

        #region Initialize

        public static string TIME_SELECTION = "A";


        private void InitializeControls()
        {
            //init date
            InitializeDatePeriod();
            //init dbname
           
            //init workload
        }

        private void InitializeDatePeriod()
        {
            /*XmlDocument doc = new XmlDocument();
            doc.Load(@".\ISIA.config");
            XmlNodeList nodeList = doc.SelectNodes("configuration/TAP.ISIA.Configuration/WX/Shift");
            this.dtpStartTime.DateTime = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + " " + nodeList[0][TIME_SELECTION].Attributes["StartTime"].Value);
            this.dtpEndTime.DateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + nodeList[0][TIME_SELECTION].Attributes["EndTime"].Value);
*/
        }      

        #endregion

        #region btnSelect_Click Event

        AwrArgsPack awrArgsPack = null;
        private void AsyncGetWorkloadAndSqlComparisonData()
        {
            //handle argument
            if (!base.ValidateUserInput(this.lcSerachOptions)) return;
            awrArgsPack = HandleArgument();
            //async load data 
            this.BeginAsyncCall("LoadWorkloadSqlData", "DisplayChart", EnumDataObject.DATASET, null);
        }

        private AwrArgsPack HandleArgument()
        {
             argument = new AwrArgsPack();

            //date period handling 
            object startTime = dtpStartTime.DateTime;
            object endTime = dtpEndTime.DateTime;
            if (startTime == null || endTime == null)
            {
                string errMessage = "Please select StartTime or EndTime";
                throw new Exception(errMessage);
            }
            DateTime startDateTime = dtpStartTime.DateTime;
            DateTime endDateTime = dtpEndTime.DateTime;

            if (startDateTime > endDateTime)
            {
                argument.StartTime = endDateTime.ToString("yyyyMMddHHmmss");
                argument.EndTime = startDateTime.ToString("yyyyMMddHHmmss");
            }
            argument.StartTime = startDateTime.ToString("yyyyMMddHHmmss");
            argument.EndTime = endDateTime.ToString("yyyyMMddHHmmss");

            //combobox edit db name 
            string dbName =string.IsNullOrEmpty(cmbDbName.Text) ? "" : cmbDbName.Text.Split('(')[0];
            if (string.IsNullOrEmpty(dbName))
            {
                string errMessage = "Please select DB_NAME";
                throw new Exception(errMessage);
            }
            argument.DBName = dbName;
            argument.DBID = cmbDbName.EditValue.ToString();


            //combobox edit workload parm
            string sqlParm = cmbParameterName.Text;
            if (string.IsNullOrEmpty(sqlParm))
            {
                string errMessage = "Please select Workload parm";
                throw new Exception(errMessage);
            }
            argument.WorkloadSqlParm = sqlParm;

            argument.INSTANCE_NUMBER = cmbInstance.Text.ToString();

            return argument;
        }
        public DataSet LoadWorkloadSqlData()
        {
            return Bs.ExecuteDataSet("GetSqlInfluenceData", awrArgsPack.getPack());
        }

        public void DisplayChart(DataSet ds)
        {

            DataTable dt=ConvertDs(ds);
            this.lcPeriod.Series.Clear();
            this.lcPeriod.ContextMenuStrip = this.contextMenuStrip1;
            this.lcPeriod.Legend.LegendStyle = LegendStyles.Series;
            this.lcPeriod.Header.Text = "SQL Influence TOP10";
            Steema.TeeChart.Styles.Bar bar1 = new Steema.TeeChart.Styles.Bar(lcPeriod.Chart);
            this.lcPeriod.Axes.Bottom.Title.Text = "SQL ID";  //设置X轴标题
            this.lcPeriod.Axes.Left.Title.Text = "Parm Value";//设置Y轴标题
            var markstip = new MarksTip(lcPeriod.Chart);

            //tChart1.Chart.Panning.Allow = ScrollModes.None;
            //tChart1.Chart.Panel.Gradient.Visible = false;
            //tChart1.Chart.Panel.Color = Color.White;
            //tChart1.Chart.Walls.Back.Visible = false;
            //tChart1.Chart.Header.Visible = false;
            //tChart1.Chart.Legend.Visible = false;
            //tChart1.Chart.Aspect.View3D = false;

            void Bar_GetSeriesMark(Series Series, GetSeriesMarkEventArgs e)
            {
                //e.MarkText = $"{dt1.Rows[e.ValueIndex]["Name"]} is {dt1.Rows[e.ValueIndex]["NUM"]}";
                e.MarkText = "SQL_ID :" + $"{dt.Rows[e.ValueIndex]["SQL_ID"]}" + "\r\n" + "VALUE :" + $"{ dt.Rows[e.ValueIndex][argument.WorkloadSqlParm]}";
            }
            bar1.Legend.Text = argument.WorkloadSqlParm;
            bar1.Marks.Visible = false;
            bar1.LabelMember = "SQL_ID";
            bar1.YValues.DataMember = argument.WorkloadSqlParm;
            bar1.DataSource = dt;
            bar1.GetSeriesMark += Bar_GetSeriesMark;//提示信息事件

            //this.tChart1.Series[0].DataSource = ds.Tables[0];
            //foreach(DataRow dr in dt.Rows)
            //{
            //    bar1.Add(++count, (double?)dr[argument.WorkloadSqlParm]);
            //}

            //this.tChart1.Legend.Shadow.Color = Color.Cyan;//图例
            //this.tChart1.Legend.Shadow.Width = 4;
        }
      
        private DataTable ConvertDs(DataSet ds)
        {
            //DataTable dtResult = new DataTable();

            //dtResult.Columns.Add("sql_id", typeof(string));
            //dtResult.Columns.Add(argument.WorkloadSqlParm, typeof(double));
            ////dtResult.Columns.Add("text", typeof(string));
            //int count = 0;
            //foreach(DataRow dr in ds.Tables[0].Rows)
            //{
            //    string sqlId = (string)dr["SQL_ID"];
            //    //string sqlText = (string)dr["SQL_TEXT"];
            //    double value = (double)((decimal)dr[argument.WorkloadSqlParm]);
            //    dtResult.Rows.Add(sqlId, value);
            //    count++;
            //    if (count >= 10)
            //    {
            //        break;
            //    }
            //}
            return ds.Tables[0];
        }


        #endregion

        private void editChartToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            currentChart.ShowEditor();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            currentChart = (sender as ContextMenuStrip).SourceControl as TChart;
        }



    }
}
