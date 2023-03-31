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

namespace ISIA.UI.TREND
{
    public partial class FrmWorkloadSqlRelationAnalysis : DockUIBase1T1
    {
        //contextmenutrip appoint an exact chart
        TChart currentChart = null;
        //define bs
        BizDataClient bs = null;

        #region getset
        public BizDataClient Bs { get => bs; set => bs = value; }
        #endregion

        public FrmWorkloadSqlRelationAnalysis()
        {
            InitializeComponent();
            Bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.WorkloadSqlRelationAnalysis");
            this.InitializeControls();
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

        private void dateStart_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                InitializeSqlId();
            }
            catch (Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(TAP.UI.EnumMsgType.CONFIRM, ex.Message);

            }
        }
        private void comboBoxDBName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                InitializeSqlId();
            }catch(Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(TAP.UI.EnumMsgType.CONFIRM, ex.Message);

            }
        }


        private void dateEnd_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                InitializeSqlId();
            }
            catch (Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(TAP.UI.EnumMsgType.CONFIRM, ex.Message);

            }
        }

        private void textEditSqlId_EditValueChanged(object sender, EventArgs e)
        {

            FilteringCheckedListBoxSqlIds();
            
        }
        #endregion

        #region Initialize

        public static string TIME_SELECTION = "A";


        private void InitializeControls()
        {
            //init date
            InitializeDatePeriod();
            //init dbname
            InitializeDbName();
           
            //init workload
            InitializeWorkloadParm();
        }

        private void InitializeDatePeriod()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@".\ISIA.config");
            XmlNodeList nodeList = doc.SelectNodes("configuration/TAP.ISIA.Configuration/WX/Shift");
            this.dateStart.DateTime = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + " " + nodeList[0][TIME_SELECTION].Attributes["StartTime"].Value);
            this.dateEnd.DateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + nodeList[0][TIME_SELECTION].Attributes["EndTime"].Value);
            this.dateStart.EditValueChanged += new System.EventHandler(this.dateStart_EditValueChanged);
            this.dateEnd.EditValueChanged += new System.EventHandler(this.dateEnd_EditValueChanged);



        }

        private void InitializeDbName()
        {
            AwrArgsPack args = new AwrArgsPack();
            //DataSet ds = Bs.ExecuteDataSet("GetDBName", args.getPack());
            //DataTable dt = ds.Tables[0];
            //foreach (DataRow dr in dt.Rows)
            //{
            //    this.comboBoxDBName.Properties.Items.Add(dr["DbName"]);
            //}
        }

        private void InitializeSqlId()
        {
            AwrArgsPack args = new AwrArgsPack();
            args.StartTime = ((DateTime)dateStart.EditValue).ToString("yyyyMMddHHmmss");
            args.EndTime = ((DateTime)dateEnd.EditValue).ToString("yyyyMMddHHmmss");
            if (string.IsNullOrEmpty(comboBoxDBName.Text))
            {
                throw new Exception(" Please select db name first");
            }
            args.DBName = comboBoxDBName.Text.Split('(')[0];

            DataSet ds = Bs.ExecuteDataSet("GetSqlId", args.getPack());
            this.clbSqlIds.Items.Clear();
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                this.clbSqlIds.Items.Add(dr["SQL_ID"]);
            }
            //sqlcollection update.
            foreach(CheckedListBoxItem item in this.clbSqlIds.Items)
            {
                sqlCollection.Add((string)item.Value);
            }

        }

        private void InitializeWorkloadParm()
        {
            foreach (KeyValuePair<string, string> pair in AwrArgsPack.WorkloadSqlRelationMapping)
            {
                this.comboBoxEditWorkloadSql.Properties.Items.Add(pair.Key);
            }
        }

        #endregion

        #region btnSelect_Click Event

        AwrArgsPack awrArgsPack = null;
        private void AsyncGetWorkloadAndSqlComparisonData()
        {
            //handle argument
            awrArgsPack = HandleArgument();
            //async load data 
            this.BeginAsyncCall("LoadWorkloadSqlData", "DisplayChart", EnumDataObject.DATASET, null);
        }

        private AwrArgsPack HandleArgument()
        {
            AwrArgsPack argument = new AwrArgsPack();
            //date period handling 
            object startTime = dateStart.EditValue;
            object endTime = dateEnd.EditValue;
            if (startTime == null || endTime == null)
            {
                string errMessage = "Please select StartTime or EndTime";
                throw new Exception(errMessage);
            }
            DateTime startDateTime = (DateTime)dateStart.EditValue;
            DateTime endDateTime = (DateTime)dateEnd.EditValue;

            if (startDateTime > endDateTime)
            {
                argument.StartTime = endDateTime.ToString("yyyyMMddHHmmss");
                argument.EndTime = startDateTime.ToString("yyyyMMddHHmmss");
            }
            argument.StartTime = startDateTime.ToString("yyyyMMddHHmmss");
            argument.EndTime = endDateTime.ToString("yyyyMMddHHmmss");

            //combobox edit db name 
            string dbName = comboBoxDBName.Text.Split('(')[0];
            if (string.IsNullOrEmpty(dbName))
            {
                string errMessage = "Please select DB_NAME";
                throw new Exception(errMessage);
            }
            argument.DBName = dbName;

            //SQLID handling 

            argument.SqlIdList = clbSqlIds.Items.GetCheckedValues();


            //combobox edit workload parm
            string workloadSql = comboBoxEditWorkloadSql.Text;
            if (string.IsNullOrEmpty(dbName))
            {
                string errMessage = "Please select Workload parm";
                throw new Exception(errMessage);
            }
            argument.WorkloadSqlParm = workloadSql;

            return argument;
        }
        public DataSet LoadWorkloadSqlData()
        {
            return Bs.ExecuteDataSet("GetWorkloadSqlRelationData", awrArgsPack.getPack());
        }

        public void DisplayChart(DataSet ds)
        {
            dpnlRight_Container.Controls.Clear();
            TChart chart = new TChart();
            chart.Series.Clear();
            chart.ContextMenuStrip = contextMenuStrip1;
            chart.Axes.Bottom.Labels.MultiLine = true;

            chart.Dock = DockStyle.Fill;
            //Header set
            chart.Header.Text = "WORKLOAD_SQL_RELATION";
            //Legend set
            chart.Legend.LegendStyle = LegendStyles.Series;
            chart.Legend.Visible = true;
            chart.Legend.CheckBoxes = true;

            //tool tip
            MarksTip marksTip = new MarksTip(chart.Chart);
            marksTip.Active = true;
            marksTip.MouseDelay = 100;
            marksTip.MouseAction = MarksTipMouseAction.Move;
            marksTip.Style = MarksStyles.XY;
            chart.MouseMove += new MouseEventHandler(tChart_MouseMove);
            marksTip.GetText += new MarksTipGetTextEventHandler(marksTip_GetText);

            Color color = new Color();
            int index = 0;
            foreach (DataTable dt in ds.Tables)
            {
                color.getRandomColor();
                Line line = CreateLine(dt, color);
                if (index == 1)
                {
                    line.CustomVertAxis = chart.Axes.Right;
                }
                chart.Series.Add(line);
                chart.Axes.Bottom.Labels.DateTimeFormat = "yyyyMMdd hh:mm";
                chart.Axes.Bottom.Labels.ExactDateTime = true;
                chart.Axes.Bottom.Ticks.Width = 0;
                index++;
            }
            dpnlRight_Container.Controls.Add(chart);
        }
        private class Color
        {
            int R;
            int G;
            int S;

            Random r = new Random();

            public int R1 { get => R; set => R = value; }
            public int G1 { get => G; set => G = value; }
            public int S1 { get => S; set => S = value; }

            public static int MAX = 255;
            public void getRandomColor()
            {
                R1 = r.Next(0, MAX);
                G1 = r.Next(0, MAX);
                S1 = r.Next(0, MAX);
            }
        }
        private Line CreateLine(DataTable dt, Color color)
        {
            Line line = new Line();
            line.DataSource = dt;
            line.YValues.DataMember = dt.TableName;
            line.XValues.DataMember = "end_interval_time";
            line.ShowInLegend = true;
            line.Legend.Text = dt.TableName;
            line.Title = dt.TableName;
            line.Color =
                System.Drawing.Color.FromArgb(color.R1, color.G1, color.S1);
            line.Legend.BorderRound = 20;
            line.XValues.DateTime = true;
            return line;
        }
        private int SeriesIndex = -1;

        void tChart_MouseMove(object sender, MouseEventArgs e)
        {
            TChart tmpChart = sender as TChart;
            for (int i = 0; i < tmpChart.Series.Count; i++)
            {
                if (tmpChart[i].Clicked(e.X, e.Y) != -1)
                {
                    SeriesIndex = i;
                    break;
                }
                SeriesIndex = -1;
            }
        }
        void marksTip_GetText(Steema.TeeChart.Tools.MarksTip sender, Steema.TeeChart.Tools.MarksTipGetTextEventArgs e)
        {
            MarksTip marks = sender as MarksTip;
            if (e.Text.Contains("Date") || SeriesIndex == -1)
            {
                return;
            }
            var strPrmt = marks.Chart.Series[SeriesIndex].Legend.Text;
            int indexPosition = e.Text.IndexOf(":") + 2;
            e.Text = "Parm: " + strPrmt + "\n" +
            "Date: " + e.Text.Substring(0, indexPosition) + "\n" + "Value:" +
            e.Text.Substring(indexPosition + 1);
        }

        #endregion

        #region textEditSqlId_EditValueChanged

        private List<string> sqlCollection = new List<string>();

        private void FilteringCheckedListBoxSqlIds()
        {
            if (string.IsNullOrEmpty(textEditSqlId.Text) == false)
            {
                clbSqlIds.Items.Clear();
                foreach (string str in sqlCollection)
                {
                    if (str.Contains(textEditSqlId.Text))
                    {
                        clbSqlIds.Items.Add(str);
                    }
                }
            }
            else if (textEditSqlId.Text == "")
            {
                clbSqlIds.Items.Clear();
                foreach (string str in sqlCollection)
                {
                    clbSqlIds.Items.Add(str);

                }
            }
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
