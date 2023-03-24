
using ISIA.UI.BASE;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP;
using TAP.Data.Client;
using TAP.UI;

namespace ISIA.UI.TREND
{
    public partial class FrmSQLFullTextQueryAnalysis1 : DockUIBase1T1
    {

        #region Feild
        BizDataClient bs = null;
        DataSet dataSet = null;
        #endregion
        public FrmSQLFullTextQueryAnalysis1()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.ANALYSIS.DLL", "ISIA.BIZ.ANALYSIS.SQLFullTextQueryAnalysis");
        }

        public DataSet LoadData()
        {

            DataClient tmpDataClient = new DataClient();
            string tmpMainMenuSql = "SELECT rownum, t.DBID,t.SQL_ID,t.CPU_TIME_DELTA FROM raw_dba_hist_sqlstat_isfa T";
            dataSet = tmpDataClient.SelectData(tmpMainMenuSql, "raw_dba_hist_sqlstat_isfa");
            dataSet.Tables[0].TableName = "TABLE";


            IEnumerable<IGrouping<string, DataRow>> result = dataSet.Tables[0].Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["SQL_ID"].ToString());
            if (result != null && result.Count() > 0)
            {
                foreach (IGrouping<string, DataRow> rows in result)
                {
                    DataTable dataTable = rows.ToArray().CopyToDataTable();
                    dataTable.TableName = rows.Key;
                    if (dataTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(dataTable);
                    }
                }
            }


            
            
            return dataSet;
        }


        public void DisplayData(DataSet dataSet)
        {
            CreateBar();

        }

        /*private void BindData()
        {
            //chartControl1.Series.Clear();
            DataClient tmpDataClient = new DataClient();
            string tmpMainMenuSql = "SELECT rownum, t.DBID,t.SQL_ID,t.CPU_TIME_DELTA FROM raw_dba_hist_sqlstat_isfa T";
            dataSet = tmpDataClient.SelectData(tmpMainMenuSql, "raw_dba_hist_sqlstat_isfa");
            dataSet.Tables[0].TableName = "TABLE";


            IEnumerable<IGrouping<string, DataRow>> result = dataSet.Tables[0].Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["SQL_ID"].ToString());
            if (result != null && result.Count() > 0)
            {
                foreach (IGrouping<string, DataRow> rows in result)
                {
                    DataTable dataTable = rows.ToArray().CopyToDataTable();
                    dataTable.TableName = rows.Key;
                    if (dataTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(dataTable);
                    }
                }
            }
        

            CreateBar();
            *//*foreach (DataRow row in tableMain.Rows)
            {
                //Series series1 = new Series("日志统计", ViewType.RadarArea);
                Series series1 = new Series(row["SQL_ID"].ToString(), ViewType.Bar);

                chartControl1.Series.Add(series1);
                SeriesPoint point = null;
                if (row["DBID"] != null)
                {
                    point = new SeriesPoint(row["DBID"].ToString());
                    double[] vals = { Convert.ToDouble(row["CPU_TIME_DELTA"]) };
                    point.Values = vals;
                    series1.Points.Add(point);
                }
            }*//*
        }*/


        private void CreateBar()
        {

            tChart1.Series.Clear();
            tChart1.Legend.LegendStyle = LegendStyles.Values;
            var markstip = new MarksTip(tChart1.Chart);

            Bar bar = new Bar(tChart1.Chart);
            DataTable dt1 = new DataTable();
            dt1.Columns.AddRange(new DataColumn[2] {
            new DataColumn("Name", typeof(string)),
            new DataColumn("NUM", typeof(int)) });
            if (dataSet.Tables.Count > 1)
            {
                foreach (DataTable dt in dataSet.Tables)
                {
                    if (dt.TableName != "TABLE")
                    {
                        int num = Convert.ToInt32(dt.Compute("avg(CPU_TIME_DELTA)", ""));
                        dt1.Rows.Add(dt.TableName, num);
                    }
                }
            }

            dt1.DefaultView.Sort = "NUM DESC";
            dt1 = dt1.DefaultView.ToTable();

            DataTable dt2 = new DataTable();
            dt2.Columns.AddRange(new DataColumn[2] {
            new DataColumn("Name", typeof(string)),
            new DataColumn("NUM", typeof(int)) });

            for (int i = 0; i < 10; i++)
            {
                DataRow drow = dt1.Rows[i];
                
                dt2.Rows.Add(drow["Name"].ToString(), drow["NUM"].ToString());
            }

            /*if (dt1.Rows.Count > 10)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (i > 9)
                    {
                        dt1.Rows.Remove(dt1.Rows[i]);
                    }
                }
            }*/
            void Bar_GetSeriesMark(Series Series, GetSeriesMarkEventArgs e)
            {
                //e.MarkText = $"{dt1.Rows[e.ValueIndex]["Name"]} is {dt1.Rows[e.ValueIndex]["NUM"]}";
                e.MarkText = "SQL_ID :" + $"{dt2.Rows[e.ValueIndex]["Name"]}" + "\r\n" + "VALUE :" + $"{ dt2.Rows[e.ValueIndex]["NUM"]}";
            }

            void TChart1_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
            {
                DataRow dataRow =  dt2.Rows[valueIndex];
                DataClient tmpDataClient = new DataClient();
                string tmpMainMenuSql = string.Format("SELECT T.Sql_Text FROM raw_dba_hist_sqltext_isfa T where t.sql_id='{0}' ", dataRow["NAME"].ToString());
                DataTable tableMain1 = tmpDataClient.SelectData(tmpMainMenuSql, "raw_dba_hist_sqltext_isfa").Tables[0];
                tableMain1.TableName = "sql";

                memoEdit1.Text = tableMain1.Rows[0][0].ToString();
            }
            bar.ColorEach = true;
            bar.DataSource = dt2;
            bar.YValues.DataMember = "NUM";

            bar.LabelMember = "Name";
            bar.Marks.Visible = false;
            bar.GetSeriesMark += Bar_GetSeriesMark;//提示信息事件
            tChart1.ClickSeries += TChart1_ClickSeries;//点击chart事件
            return;
        }


        public override void ExecuteCommand(ArgumentPack arguments)
        {
            DataTable tmpdt;

            foreach (string tmpstr in arguments.ArgumentNames)
            {
                if (tmpstr == "hashtable")
                {
                    //Hashtable hashtable;
                    //hashtable = (Hashtable)arguments["hashtable"].ArgumentValue;
                    /*if (hashtable["FACILITY"] != null)
                    {
                        CBC.SelectedComboBox(cboFab, hashtable["FACILITY"].ToString());
                    }
                    if (hashtable["TECH"] != null)
                    {
                        CBC.SelectedComboBox(cboTech, hashtable["TECH"].ToString());
                    }
                    if (hashtable["LOTCODE"] != null)
                    {
                        CBC.SelectedComboBox(cboLotcode, hashtable["LOTCODE"].ToString());
                    }
                    if (hashtable["OPERATION"] != null)
                    {
                        cboOper.Text = hashtable["OPERATION"].ToString();
                    }
                    if (hashtable["PRODUCT"] != null)
                    {
                        CBC.SelectedComboBox(cboProd, hashtable["PRODUCT"].ToString());
                    }
                    if (hashtable["PRMT"] != null)
                    {
                        CBC.SelectedComboBox(cboPrmt, hashtable["PRMT"].ToString());
                    }
                    if (hashtable["CBO"] != null)
                    {
                        CBC.SelectedComboBox(cboLegend, hashtable["CBO"].ToString());
                    }

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
                    }*/


                }
                if (tmpstr == "LinkTable")
                {
                    //DataTable dt = (DataTable)arguments["LinkTable"].ArgumentValue;
                    //CBC.AfterComboBoxLinkValue(dt, this.tabPane2);
                }
                if (tmpstr == "_dataTable")
                {
                    //tRadWafer.Checked = true;

                    tmpdt = (DataTable)arguments["_dataTable"].ArgumentValue;



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
            tbnSeach_Click(null, null);


        }

        private void tbnSeach_Click(object sender, EventArgs e)
        {

            try
            {
                //ComboBoxControl.SetCrossLang(this._translator);
                //if (!base.ValidateUserInput(this.layoutControl1)) return;
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
