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
using TAP.UIControls.BasicControlsDEV;

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

/*        public override void ExecuteCommand1(ArgumentPack arguments)
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
        }*/

        public override void ExecuteCommand(ArgumentPack arguments)
        {
            DataTable tmpdt;

            Hashtable hashtable = new Hashtable();

            foreach (string tmpstr in arguments.ArgumentNames)
            {
                if (tmpstr == "_hashTable")
                {

                    hashtable = (Hashtable)arguments["_hashTable"].ArgumentValue;
                    
                    if (hashtable["DBID"] != null)
                    {
                        SelectedComboBox(cmbDbName, hashtable["DBID"].ToString());
                        //SelectedDBComboBox(cmbDbName, hashtable["DBID"].ToString());
                    }
                    if (hashtable["INSTANCE_NUMBER"] != null)
                    {
                        SelectedDBComboBox(cmbInstance, hashtable["INSTANCE_NUMBER"].ToString());
                    }
                    if (hashtable["PARAMETERNAME"] != null)
                    {
                        SelectedDBComboBox(cmbParameterName, hashtable["PARAMETERNAME"].ToString());
                    }
                    
                    
                    if (hashtable["StartTime"] != null)
                    {
                        DateTime STARTTIMETEXT = DateTime.ParseExact(hashtable["StartTime"].ToString() ,"yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        dtpStartTime.EditValue = STARTTIMETEXT;
                    }
                    if (hashtable["EndTime"] != null)
                    {
                        DateTime ENDTIMETEXT = DateTime.ParseExact(hashtable["EndTime"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        dtpEndTime.EditValue = ENDTIMETEXT;
                    }

                 /*   dateStart.DateTime = (DateTime)row1["END_INTERVAL_TIME"];
                    dateEnd.DateTime = (DateTime)row2["END_INTERVAL_TIME"];
                    string STARTTIME = "";
                    string ENDTIME = "";
                    if (hashtable["STARTTIME"] != null)
                    {
                        STARTTIME = hashtable["STARTTIME"].ToString();
                    }
                    if (hashtable["ENDTIME"] != null)
                    {
                        ENDTIME = hashtable["ENDTIME"].ToString();
                    }
                    //date
                    if (STARTTIME != null && STARTTIME != "" && ENDTIME != null && ENDTIME != "")
                    {
                        tDateTimePickerDEV1.UseDatePickValue = true;

                        DateTime STARTTIMETEXT = DateTime.ParseExact(STARTTIME, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        DateTime ENDTIMETEXT = DateTime.ParseExact(ENDTIME, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        tDateTimePickerDEV1.RepresentativeValue = STARTTIMETEXT.ToString();
                        tDateTimePickerDEV1.EndRepresentativeValue = ENDTIMETEXT.ToString();
                    }
*/

                }
                if (tmpstr == "LinkTable")
                {
                    //DataTable dt = (DataTable)arguments["LinkTable"].ArgumentValue;
                    //CBC.AfterComboBoxLinkValue(dt, this.tabPane2);
                }
                if (tmpstr == "_dataTable")
                {
                    //tRadWafer.Checked = true;

                    /*tmpdt = (DataTable)arguments["_dataTable"].ArgumentValue;

                    //dataSet.Tables.Add(tmpdt.Copy());

                    List<SqlShow> list = DataTableExtend.GetList<SqlShow>(tmpdt);
                    List<string> a = list.Select(x => x.PARAMENT_NAME).Distinct().ToList();
                    string[] b = a.ToArray();
                    string para = string.Join(",", b);

                    DataRow row1 = tmpdt.Rows[0];
                    DataRow row2 = tmpdt.Rows[tmpdt.Rows.Count - 1];



                    SelectedComboBox(cmbParameterName, para);
                    SelectedDBComboBox(cmbDbName, row1["DBID"].ToString());
                    dtpStartTime.DateTime = (DateTime)row1["END_INTERVAL_TIME"];
                    dtpEndTime.DateTime = (DateTime)row2["END_INTERVAL_TIME"];*/

                    /*cmbDbName.SelectedText = row1["PARAMENT_NAME"].ToString();
                    dateStart.DateTime = (DateTime)row1["END_INTERVAL_TIME"];
                    dateEnd.DateTime = (DateTime)row2["END_INTERVAL_TIME"];
                    cmbDbName.EditValue = row1["DBID"];*/
                    //args.DbId = string.IsNullOrEmpty(cmbDbName.Text) ? "" : cmbDbName.Text.Split('(')[1];
                    //args.DbId = args.DbId.Substring(0, args.DbId.Length - 1);


                    //if (tmpdt.Rows.Count > 0)
                    //{
                    /*if (tmpdt.Columns.Contains("FAB"))
                    {
                        CBC.SelectedComboBox(cboFab, tmpdt.Rows[0]["FAB"].ToString());
                    }
                    else if (tmpdt.Columns.Contains("FACILITY"))
                    {
                        CBC.SelectedComboBox(cboFab, tmpdt.Rows[0]["FACILITY"].ToString());
                    }
                    if (tmpdt.Columns.Contains("TECH"))
                    {
                        CBC.SelectedComboBox(cboTech, tmpdt.Rows[0]["TECH"].ToString());
                    }
                    if (tmpdt.Columns.Contains("LOT_CD"))
                    {
                        CBC.SelectedComboBox(cboLotcode, tmpdt.Rows[0]["LOT_CD"].ToString());
                    }
                    else if (tmpdt.Columns.Contains("LOTCODE"))
                    {
                        CBC.SelectedComboBox(cboLotcode, tmpdt.Rows[0]["LOTCODE"].ToString());
                    }
                    if (tmpdt.Columns.Contains("OPERATION"))
                    {
                        //cboOper.Text = Utils.DataTableDistincByColumns(tmpdt, "OPERATION");
                        CBC.SelectedComboBox(cboOper, Utils.DataTableDistincByColumns(tmpdt, "OPERATION"));
                    }
                    if (tmpdt.Columns.Contains("WF_ID"))
                    {
                        CBC.SelectedComboBox(cboWafer, Utils.DataTableDistincByColumns(tmpdt, "WF_ID"));

                    }
                    if (tmpdt.Columns.Contains("STRATTime"))
                    {
                        string STARTTIME = "";
                        string ENDTIME = "";
                        STARTTIME = tmpdt.Rows[0]["STRATTime"].ToString();
                        ENDTIME = tmpdt.Rows[0]["ENDTime"].ToString();
                        tDateTimePickerDEV1.UseDatePickValue = true;

                        DateTime STARTTIMETEXT = DateTime.ParseExact(STARTTIME, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        DateTime ENDTIMETEXT = DateTime.ParseExact(ENDTIME, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        tDateTimePickerDEV1.RepresentativeValue = STARTTIMETEXT.ToString();
                        tDateTimePickerDEV1.EndRepresentativeValue = ENDTIMETEXT.ToString();

                    }
                    CBC.SelectedComboBox(cboPrmt, "Yield");
                    CBC.SelectedComboBox(cboLegend, "LOT");*/

                    /*DataTable  dataTable =  tmpdt.DefaultView.ToTable(true,"WF_ID");

                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i<dataTable.Rows.Count; i++) {

                       stringBuilder.Append(dataTable.Rows[i]["WF_ID"].ToString());
                        if (i < dataTable.Rows.Count -1) {
                            stringBuilder.Append(",");
                        }
                    }
                    CBC.SelectedComboBox(cboWafer, stringBuilder.ToString());*/



                    /*if (tmpdt.Columns.Contains("PARAMETERNAME"))
                    {
                        strPara = Utils.DataTableDistincByColumns(tmpdt, "PARAMETERNAME");
                    }
                    if (tmpdt.Columns.Contains("EDATE"))
                    {
                        Date = tmpdt.Rows[0]["EDATE"].ToString();
                    }
                    if (tmpdt.Columns.Contains("OPER"))
                    {
                        strOper = Utils.DataTableDistincByColumns(tmpdt, "OPER");
                    }
                    if (tmpdt.Columns.Contains("_legend"))
                    {
                        strCat = Utils.DataTableDistincByColumns(tmpdt, "_legend");
                    }
                    if (tmpdt.Columns.Contains("OPERATION"))
                    {
                        strOperation = Utils.DataTableDistincByColumns(tmpdt, "OPERATION");
                    }
                    if (tmpdt.Columns.Contains("DEFECT_CLASS"))
                    {
                        strDefectClass = Utils.DataTableDistincByColumns(tmpdt, "DEFECT_CLASS");
                    }
                    if (tmpdt.Columns.Contains("LOT"))
                    {
                        strLotID = Utils.DataTableDistincByColumns(tmpdt, "LOT");
                    }
                    else if (tmpdt.Columns.Contains("LOT_ID"))
                    {
                        strLotID = Utils.DataTableDistincByColumns(tmpdt, "LOT_ID");
                    }
*/
                    //Set ComboBoxText
                    /*cboLotid.Text = strLotID;
                    cboDefectClass.Text = strDefectClass;
                    cboOper.Text = strOperation;
                    cboCategory.Text = strCat;
                    cboPrmt.Text = strPara;
                    dateStart.Text = Date;*/
                    //prm = 1;

                    //}
                }

            }
            btnSelect_Click(null, null);


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

            DataTable dt=ds.Tables[0];
            this.chart1.Series.Clear();
            this.chart1.ContextMenuStrip = this.contextMenuStrip1;
            this.chart1.Legend.LegendStyle = LegendStyles.Series;
            this.chart1.Header.Text = "SQL Influence TOP10";
            Steema.TeeChart.Styles.Bar bar1 = new Steema.TeeChart.Styles.Bar(chart1.Chart);
            this.chart1.Axes.Bottom.Title.Text = "SQL ID";  //设置X轴标题
            this.chart1.Axes.Left.Title.Text = "Parm Value";//设置Y轴标题
            var markstip = new MarksTip(chart1.Chart);

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



            chart1.ClickSeries += Chart1_ClickSeries;//数据点击事件

            //this.tChart1.Series[0].DataSource = ds.Tables[0];
            //foreach(DataRow dr in dt.Rows)
            //{
            //    bar1.Add(++count, (double?)dr[argument.WorkloadSqlParm]);
            //}

            //this.tChart1.Legend.Shadow.Color = Color.Cyan;//图例
            //this.tChart1.Legend.Shadow.Width = 4;
        }

        private void Chart1_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            DataTable dt = (DataTable)s.DataSource;//获取序列数据
            //valueIndex.ToString();//获取点下标,从0开始
            awrArgsPack.SQLID = dt.Rows[valueIndex]["SQL_ID"].ToString();
            awrArgsPack.StartTime = DateTime.Now.AddDays(-59).ToString();
            DataTable dtsqlid =  Bs.ExecuteDataTable("Getsqlid", awrArgsPack.getPack());

            DataTable sqlidtext =  Bs.ExecuteDataTable("Getsqlidtext", awrArgsPack.getPack());

            FrmRankingofSQLShowSqlText frmsql = new FrmRankingofSQLShowSqlText(dtsqlid , awrArgsPack.SQLID , sqlidtext.Rows[0]["SQL_TEXT"].ToString() ,awrArgsPack.WorkloadSqlParm);
            frmsql.Show();

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


        public void SelectedDBComboBox(TCheckComboBox ComboBox, string str)
        {
            ComboBox.Setting();
            if (str == "")
            {
                ComboBox.CheckAll();
                return;
            }

            DataTable data = (DataTable)ComboBox.Properties.DataSource;

            foreach (DataRow item in data.Rows)
            {
                ComboBox.Properties.Items.Add(item[0].ToString());
            }


            foreach (CheckedListBoxItem item in ComboBox.Properties.Items)
            {

                if (item.Value.ToString().Contains(str))
                {
                    item.CheckState = CheckState.Checked;
                }
            }
        }

        public void SelectedComboBox(TCheckComboBox ComboBox, string str)
        {
            ComboBox.Setting();
            if (str == "")
            {
                ComboBox.CheckAll();
                return;
            }
            foreach (CheckedListBoxItem item in ComboBox.Properties.Items)
            {
                if (str.Contains(item.Value.ToString()))
                {
                    item.CheckState = CheckState.Checked;
                }
            }
        }




    }
}
