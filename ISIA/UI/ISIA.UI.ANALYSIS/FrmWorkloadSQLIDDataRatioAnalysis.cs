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

namespace ISIA.UI.ANALYSIS
{
    public partial class FrmWorkloadSQLIDDataRatioAnalysis : DockUIBase1T1
    {
        //contextmenutrip appoint an exact chart
        TChart currentChart = null;
        //define bs
        BizDataClient bs = null;

        DataSet DataSet = null;

        DataSet dsPARADEF = null;

        DataTable dtparameterid = null;

        #region getset
        public BizDataClient Bs { get => bs; set => bs = value; }
        #endregion

        public FrmWorkloadSQLIDDataRatioAnalysis()
        {
            InitializeComponent();
            Bs = new BizDataClient("ISIA.BIZ.ANALYSIS.DLL", "ISIA.BIZ.ANALYSIS.WorkloadSqlRelationAnalysis");


            dtparameterid = Bs.ExecuteDataTable("GetIdName");
            searchid.Properties.DataSource = dtparameterid;
            searchid.Properties.DisplayMember = "PARAMETERNAME";
            searchid.Properties.ValueMember = "PARAMETERID";

            dtpStartTime.DateTime = DateTime.Now.AddDays(-1);
            dtpEndTime.DateTime = DateTime.Now;
            tcmbday.Text = "D";
            tcmbday.Items.Add("D");
            tcmbday.Items.Add("W");
            tcmbday.Items.Add("M");

        }

        #region event
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tLUCKDbname.Text))
                {
                    string errMessage = "Please select DB_NAME";
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, TAP.UI.EnumMsgType.WARNING, errMessage);
                    return;
                }
                if (string.IsNullOrEmpty(cmbParameterName.Text))
                {
                    string errMessage = "Please select ParameterName";
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, TAP.UI.EnumMsgType.WARNING, errMessage);
                    return;
                }

                if (!base.ValidateUserInput(this.lcSerachOptions)) return;

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
                //InitializeSqlId();
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
            }
            catch (Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(TAP.UI.EnumMsgType.CONFIRM, ex.Message);

            }
        }


        private void dateEnd_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //InitializeSqlId();
            }
            catch (Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(TAP.UI.EnumMsgType.CONFIRM, ex.Message);

            }
        }

        #endregion

        #region Initialize

        public static string TIME_SELECTION = "A";








        private void InitializeSqlId()
        {
            /*AwrArgsPack args = new AwrArgsPack();


            sqlCollection = SLUEParamentName.PARAMETERNAME.Split(',').ToList<string>();*/

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





            argument.StartTime = dtpStartTime.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            argument.EndTime = dtpEndTime.DateTime.ToString("yyyy-MM-dd HH:mm:ss");

            //combobox edit db name 
            string dbName = tLUCKDbname.Text.Split('(')[0];

            argument.DBName = dbName;
            argument.DBID = tLUCKDbname.EditValue.ToString();
            //SQLID handling 

            //argument.SqlIdList = SLUEParamentName.PARAMETERNAME.Split(',').ToList<object>();


            //combobox edit workload parm
            string workloadSql = cmbParameterName.Text;
            if (string.IsNullOrEmpty(dbName))
            {
                string errMessage = "Please select Workload parm";
                throw new Exception(errMessage);
            }


            argument.WorkloadSqlParm = workloadSql;


            argument.INSTANCE_NUMBER = cmbInstance.Text.ToString();

            //
            if (searchid.Text != null)
            {
                argument.PARADEF = searchid.Text.ToString();
            }


            return argument;
        }
        public DataSet LoadWorkloadSqlData()
        {
            DataSet = Bs.ExecuteDataSet("GetWorkpara", awrArgsPack.getPack());

            if (searchid.Text != null)
            {
                dsPARADEF = Bs.ExecuteDataSet("GetParmDEF", awrArgsPack.getPack());
                dsPARADEF.Tables[0].TableName = "TABLE";
            }


            if (DataSet.Tables[0].Rows.Count != null)
            {
                DataSet.Tables[0].TableName = DataSet.Tables[0].Rows[0]["PARAMETER"].ToString();
            }



            return DataSet;
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
            chart.Header.Text = " ";
            //Legend set
            chart.Legend.LegendStyle = LegendStyles.Series;
            chart.Legend.Visible = true;
            chart.Legend.CheckBoxes = false;

            //tool tip
            MarksTip marksTip = new MarksTip(chart.Chart);
            marksTip.Active = true;
            marksTip.MouseDelay = 100;
            marksTip.MouseAction = MarksTipMouseAction.Move;
            marksTip.Style = MarksStyles.XY;




            Line line = CreateLine(ds.Tables[0]);
            chart.Series.Add(line);



            if (searchid.Text != null)
            {
                IEnumerable<IGrouping<string, DataRow>> result = dsPARADEF.Tables[0].Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["PARAMETER"].ToString());
                if (result != null && result.Count() > 0)
                {
                    foreach (IGrouping<string, DataRow> rows in result)
                    {
                        DataTable dataTable = rows.ToArray().CopyToDataTable();
                        dataTable.TableName = rows.Key;
                        if (dataTable.Rows.Count > 0)
                        {
                            dsPARADEF.Tables.Add(dataTable);
                        }
                    }
                }

                if (dsPARADEF.Tables.Count > 1)
                {
                    foreach (DataTable dtPARAMETER in dsPARADEF.Tables)
                    {
                        if (dtPARAMETER.TableName != "TABLE")
                        {
                            Line lineSQL = CreateYLine(dtPARAMETER);
                            chart.Series.Add(lineSQL);
                        }
                    }
                }
            }





            chart.Axes.Bottom.Labels.DateTimeFormat = "yyyyMMdd hh:mm";
            chart.Axes.Bottom.Labels.ExactDateTime = true;
            chart.Axes.Bottom.Ticks.Width = 0;
            chart.Axes.Left.Visible = true;
            chart.Axes.Right.Visible = true;


            dpnlRight_Container.Controls.Add(chart);
        }

        private Line CreateLine(DataTable dt)
        {
            Line line = new Line();
            line.DataSource = dt;
            line.YValues.DataMember = "VALUE";
            line.XValues.DataMember = "END_INTERVAL_TIME";
            line.ShowInLegend = true;
            line.Legend.Text = dt.TableName;
            line.Title = dt.TableName;

            line.Legend.BorderRound = 20;
            line.XValues.DateTime = true;

            line.GetSeriesMark += Line_GetSeriesMark;
            void Line_GetSeriesMark(Series series, GetSeriesMarkEventArgs e)
            {
                e.MarkText = "PARAMETER_NAME :" + $"{dt.Rows[e.ValueIndex]["PARAMETER"]}" + "\r\n" + "VALUE :" + $"{dt.Rows[e.ValueIndex]["VALUE"]}" + "\r\n" + "TIME :" + $"{ dt.Rows[e.ValueIndex]["END_INTERVAL_TIME"]}";
            }
            return line;
        }

        private Line CreateYLine(DataTable dt)
        {
            Line line = new Line();
            line.DataSource = dt;
            line.VertAxis = VerticalAxis.Right;
            line.YValues.DataMember = "VALUE";
            line.XValues.DataMember = "END_INTERVAL_TIME";
            line.ShowInLegend = true;
            line.Legend.Text = dt.TableName;
            line.Title = dt.TableName;

            line.Legend.BorderRound = 20;
            line.XValues.DateTime = true;

            line.GetSeriesMark += Line_GetSeriesMark;
            void Line_GetSeriesMark(Series series, GetSeriesMarkEventArgs e)
            {
                e.MarkText = "PARAMETER_NAME :" + $"{dt.Rows[e.ValueIndex]["PARAMETER"]}" + "\r\n" + "VALUE :" + $"{dt.Rows[e.ValueIndex]["VALUE"]}" + "\r\n" + "TIME :" + $"{ dt.Rows[e.ValueIndex]["END_INTERVAL_TIME"]}";
            }
            return line;
        }


        #endregion

        #region textEditSqlId_EditValueChanged

        private List<string> sqlCollection = new List<string>();

        #endregion
        private void editChartToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            currentChart.ShowEditor();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            currentChart = (sender as ContextMenuStrip).SourceControl as TChart;
        }


        private void tcmbday_EditValueChanged(object sender, EventArgs e)
        {
            if (tcmbday.Text == "D")
            {
                this.dtpStartTime.DateTime = DateTime.Now.AddDays(-1);
                this.dtpEndTime.DateTime = DateTime.Now;
            }
            else if (tcmbday.Text == "W")
            {
                this.dtpStartTime.DateTime = DateTime.Now.AddDays(-7);
                this.dtpEndTime.DateTime = DateTime.Now;
            }
            else if (tcmbday.Text == "M")
            {
                this.dtpStartTime.DateTime = DateTime.Now.AddMonths(-1);
                this.dtpEndTime.DateTime = DateTime.Now;
            }
        }

    }
}
