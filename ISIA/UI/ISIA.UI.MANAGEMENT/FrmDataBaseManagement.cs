using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
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
/*using TAP.Models;
using TAP.Models.Factories.Facilities;*/
using TAP.UI;
using ISIA.UI.BASE;
using DevExpress.XtraTab;
using DevExpress.XtraCharts;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;

namespace ISIA.UI.MANAGEMENT
{
    public partial class FrmDataBaseManagement : DockUIBase1T1
    {

        #region Feild
        BizDataClient bs = null;
        ComboBoxControl ComboBoxControl = new ComboBoxControl();
        DataBaseManagementArgsPack args = new DataBaseManagementArgsPack();

        DataSet ds = new DataSet();
        private DataSet ds_NUM;
        private DataSet ds_OUTPUT;
        #endregion


        public FrmDataBaseManagement()
        {
            InitializeComponent();
            InitializeComboBox();
            /*tDateTimePickerSE1.StartDate = DateTime.ParseExact(DateTime.Now.AddYears(-1).ToString("yyyy"), "yyyy", System.Globalization.CultureInfo.CurrentCulture);
            tDateTimePickerSE1.EndDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy"), "yyyy", System.Globalization.CultureInfo.CurrentCulture);
            */
            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.DataBaseManagement");
        }


        #region mothod

        private void InitializeComboBox()
        {

        }




        public DataSet LoadData()
        {
            
            /*string startTime = tDateTimePickerSE1.StartDateString.Substring(0, 14);
            string endTime = tDateTimePickerSE1.EndDateString.Substring(0, 14);
            string start = tDateTimePickerSE1.StartDateString.Substring(0, 8);
            string end = tDateTimePickerSE1.StartDateString.Substring(0, 8);
            args.Workshop = cboworkshop.Text;
            args.FACILITY = cbofacility.Text;
            args.Process_Type = cboprocess.Text;
            args.Report_SatrtDate = startTime;
            args.Report_EndDate = endTime;
            args.Report_Satrt = start;
            args.Report_End = end;*/

            /* ds = bs.ExecuteDataSet("GetBMPMINFO", args.getPack());
             ds_NUM = bs.ExecuteDataSet("GetBMPMNUM", args.getPack());
             ds_OUTPUT = bs.ExecuteDataSet("GetOUTPUT", args.getPack());*/

            ds = bs.ExecuteDataSet("GetDB");
            
            return ds;
        }

        public void DisplayData(DataSet ds)
        {
            if (ds == null)
            {
                return;
            }
            GridViewDataBinding();
            GridViewStyle(gridView1);
        }

        public void GridViewDataBinding()
        {
            gridControl1.DataSource = null;

            gridView1.Columns.Clear();

            gridControl1.DataSource = ds.Tables[0];

            DevExpress.XtraGrid.Columns.GridColumn gridColumn = new DevExpress.XtraGrid.Columns.GridColumn();

            gridView1.Columns.Add(gridColumn);
            gridColumn.Visible = true;
            gridColumn.Caption = "DELETE";
            gridColumn.FieldName = "OPERATION";
            RepositoryItemButtonEdit edit = new RepositoryItemButtonEdit();

            edit.ButtonClick += Edit_ButtonClick;

            edit.Buttons[0].Kind = ButtonPredefines.Delete;
            edit.Tag = "DELETE";
            edit.Buttons[0].Caption = "DELETE";
            edit.TextEditStyle = TextEditStyles.HideTextEditor;
            gridControl1.RepositoryItems.Add(edit);
            gridColumn.ColumnEdit = edit;

            gridColumn.ShowButtonMode = (DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum)ShowButtonModeEnum.ShowAlways;




        }


        public void GridViewStyle(GridView gridView)
        {
            gridView1.OptionsBehavior.Editable = true; 

            gridView1.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;

            gridView1.Columns["ID"].Visible = false;

            
        }

        private void Edit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Delete)
            {
                if (XtraMessageBox.Show("Do you wish to remove this row?", "Warring", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    gridView1.DeleteRow(gridView1.FocusedRowHandle);
                    DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    args.ROWID = dataRow["ID"].ToString();

                    int Res = bs.ExecuteModify("DelteTCODE", args.getPack());

                    if (Res == 1)
                    {
                        XtraMessageBox.Show("Delete data succeeded ", " ");
                    }
                    else {
                        XtraMessageBox.Show("Delete data failed ", " ");
                    }
                    
                }
            }
        }
    

        /*         private RepositoryItemButtonEdit CreateRepositoryItemButtonEdit(Dictionary<object, string> dicButtons)
                  {
                      RepositoryItemButtonEdit repositoryBtn = new RepositoryItemButtonEdit();
                      repositoryBtn.AppearanceDisabled.Options.UseTextOptions = true;
                      repositoryBtn.AppearanceDisabled.TextOptions.HAlignment = HorzAlignment.Near;
                      repositoryBtn.AutoHeight = false;
                      repositoryBtn.TextEditStyle = TextEditStyles.HideTextEditor;
                      repositoryBtn.ButtonsStyle = BorderStyles.UltraFlat;
                      repositoryBtn.Buttons.Clear();
                     EditorButton btn = null;
                     foreach (KeyValuePair<object, string> item in dicButtons)
                     {
                         btn = new EditorButton();
                         btn.Kind = ButtonPredefines.Glyph;
                         btn.Caption = item.Value;
                         btn.Tag = item.Key;
                         repositoryBtn.Buttons.Add(btn);
                     }
                     return repositoryBtn;
                 }*/

        private DataTable DataConversionGridview(DataTable dcc, DataTable dd, DataTable db)
        {
            DataTable NewDt = new DataTable();
            NewDt.Columns.Add("TIMEKEY", typeof(string));
            NewDt.Columns.Add("BMTIME", typeof(long));
            NewDt.Columns.Add("PMTIME", typeof(long));
            NewDt.Columns.Add("TOTALTIME", typeof(long));
            NewDt.Columns.Add("OUTPUT", typeof(int));

            NewDt.Columns.Add("BMPRICE", typeof(double));
            NewDt.Columns.Add("PMPRICE", typeof(double));
            NewDt.Columns.Add("TOTALPRICE", typeof(double));

            NewDt.Columns.Add("BMNUM", typeof(int));
            NewDt.Columns.Add("PMNUM", typeof(int));
            NewDt.Columns.Add("TOTALNUM", typeof(int));
            for (int i = 0; i < dcc.Rows.Count; i++)
            {
                string yue = dcc.Rows[i]["MON"].ToString();
                string value = dcc.Rows[i]["BMTIME"].ToString();
                string value1 = dcc.Rows[i]["PMTIME"].ToString();
                string totalprice = dcc.Rows[i]["TOTALPRICE"].ToString();
                string bmprice = dcc.Rows[i]["BMPRICE"].ToString();
                string pmprice = dcc.Rows[i]["PMPRICE"].ToString();
                string num = dcc.Rows[i]["BMCNT"].ToString();
                DataRow row = NewDt.NewRow();
                row["TIMEKEY"] = yue;
                row["BMTIME"] = long.Parse(value);
                row["PMTIME"] = long.Parse(value1);
                row["TOTALTIME"] = long.Parse(value) + long.Parse(value1);
                string output = string.Empty;
                for (int m = 0; m < db.Rows.Count; m++)
                {
                    if (db.Rows[m]["MON"].ToString() == yue)
                    {
                        output = db.Rows[m]["VALUE"].ToString();
                        break;
                    }
                }
                row["OUTPUT"] = int.Parse(output);

                row["BMPRICE"] = double.Parse(bmprice);
                row["PMPRICE"] = double.Parse(pmprice);
                row["TOTALPRICE"] = totalprice;
                row["BMNUM"] = int.Parse(num);

                string icount = string.Empty;
                for (int j = 0; j < dd.Rows.Count; j++)
                {
                    if (dd.Rows[j]["MON"].ToString() == yue)
                    {
                        icount = dd.Rows[j]["COUNT"].ToString();

                        break;
                    }

                    //var ss = dd.AsEnumerable();
                    //var icount = ss.Where(x => x.Field<string>("MON").Equals(yue)).FirstOrDefault()["COUNT"].ToString();
                    string num1 = dd.Rows[j]["COUNT"].ToString();
                    string date = dd.Rows[j]["MON"].ToString();
                    row["PMNUM"] = int.Parse(num1);
                    row["TOTALNUM"] = int.Parse(num) + int.Parse(num1);
                }
                //var icount = lists.First(x => x.mon.Equals(yue));
                row["PMNUM"] = int.Parse(icount);
                row["TOTALNUM"] = int.Parse(num) + int.Parse(icount);
                NewDt.Rows.Add(row);
            }



            return NewDt;
        }

        private void ChartBand(DataTable dcc, DataTable dd, DataTable a)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BMTYPE", typeof(string));
            dt.Columns.Add("月份", typeof(string));
            dt.Columns.Add("VALUE", typeof(long));
            dt.Columns.Add("BMTIME", typeof(int));
            dt.Columns.Add("TOTALTIME", typeof(long));
            dt.Columns.Add("OUTPUT", typeof(int));
            dt.Columns.Add("TIME", typeof(string));
            dt.Columns.Add("Trade_Date", typeof(string));
            dt.Columns.Add("PRICETYPE", typeof(string));
            dt.Columns.Add("PRICE", typeof(double));
            dt.Columns.Add("TOTALPRICE", typeof(double));
            dt.Columns.Add("PEOPLETYPE", typeof(string));
            dt.Columns.Add("NUM", typeof(int));
            dt.Columns.Add("MON", typeof(string));
            dt.Columns.Add("TOTALNUM", typeof(int));
            for (int i = 0; i < dcc.Rows.Count; i++)
            {
                string yue = dcc.Rows[i]["MON"].ToString();
                string value = dcc.Rows[i]["BMTIME"].ToString();
                string value1 = dcc.Rows[i]["PMTIME"].ToString();
                string totalprice = dcc.Rows[i]["TOTALPRICE"].ToString();
                string bmprice = dcc.Rows[i]["BMPRICE"].ToString();
                string pmprice = dcc.Rows[i]["PMPRICE"].ToString();
                string num = dcc.Rows[i]["BMCNT"].ToString();
                string num1 = string.Empty;
                string output = string.Empty;

                for (int j = 0; j < dd.Rows.Count; j++)
                {
                    if (dd.Rows[j]["MON"].ToString() == yue)
                    {
                        num1 = dd.Rows[j]["COUNT"].ToString();
                    }
                }


                for (int k = 0; k < a.Rows.Count; k++)
                {
                    if (a.Rows[k]["MON"].ToString() == yue)
                    {
                        output = a.Rows[k]["VALUE"].ToString();
                    }
                }
                for (int m = 0; m < 13; m++)
                {
                    DataRow row = dt.NewRow();
                    row["月份"] = yue;
                    row["VALUE"] = long.Parse(value);
                    row["BMTYPE"] = "BM总时间";
                    row["PRICETYPE"] = "BM总金额";
                    row["PEOPLETYPE"] = "BM总人次";
                    row["TOTALTIME"] = long.Parse(value) + long.Parse(value1);
                    //row["TIME"] = time;
                    row["OUTPUT"] = int.Parse(output);
                    row["TOTALPRICE"] = totalprice;
                    row["PRICE"] = double.Parse(bmprice);
                    //row["MON"] = yue;
                    row["NUM"] = int.Parse(num);
                    row["TOTALNUM"] = int.Parse(num) + int.Parse(num1);
                    dt.Rows.Add(row);

                    DataRow row1 = dt.NewRow();
                    row1["月份"] = yue;
                    row1["VALUE"] = long.Parse(value1);
                    row1["BMTYPE"] = "PM总时间";
                    row1["PRICETYPE"] = "PM总金额";
                    row1["PEOPLETYPE"] = "PM总人次";
                    row1["TOTALTIME"] = long.Parse(value) + long.Parse(value1);
                    //row1["TIME"] = time;
                    row1["OUTPUT"] = output;
                    row["TOTALPRICE"] = totalprice;
                    row1["PRICE"] = double.Parse(pmprice);
                    //row1["MON"] = yue1;
                    row1["NUM"] = int.Parse(num1);
                    row1["TOTALNUM"] = int.Parse(num) + int.Parse(num1);
                    dt.Rows.Add(row1);
                }


            }
            //第一张柱状图 BMPM时间
           /* ChartControl newChart = new ChartControl();

            newChart.Legend.Name = "Default Legend";
            newChart.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            newChart.Size = new System.Drawing.Size(100, 250);
            newChart.TabIndex = 0;
            newChart.DataSource = dt;
            newChart.Dock = DockStyle.Top;
            newChart.SeriesDataMember = "BMTYPE";
            newChart.SeriesTemplate.ArgumentDataMember = "月份";
            newChart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "VALUE" });
            newChart.SeriesTemplate.View = new StackedBarSeriesView();
            newChart.SeriesTemplate.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            newChart.Titles.Clear();
            ChartTitle titles = new ChartTitle();            //声明标题 

            titles.Text = "BMPM时间";                            //名称 
            titles.TextColor = System.Drawing.Color.Black;   //颜色 
            titles.Indent = 1;                                //设置距离  值越小柱状图就越大 
            titles.Font = new Font("Tahoma", 10, FontStyle.Bold);            //设置字体 
            titles.Dock = ChartTitleDockStyle.Top;           //设置对齐方式 
            titles.Indent = 0;
            titles.Alignment = StringAlignment.Center;       //居中对齐 
            newChart.Titles.Add(titles);
            //TotalTime折线图
            Series series1 = new Series("TotalTime", ViewType.Line);
            series1.ArgumentScaleType = ScaleType.Qualitative;
            series1.ValueScaleType = ScaleType.Numerical;

            series1.DataSource = dt;
            series1.ArgumentDataMember = "月份";
            series1.ValueDataMembers[0] = "TOTALTIME";
            newChart.Series.Add(series1);
            //OUTPUT 折线图
            Series series4 = new Series("OUTPUT(MPCS) ", ViewType.Line);
            series4.ArgumentScaleType = ScaleType.Qualitative;
            series4.ValueScaleType = ScaleType.Numerical;

            series4.DataSource = dt;
            series4.ArgumentDataMember = "月份";
            series4.ValueDataMembers[0] = "OUTPUT";
            newChart.Series.Add(series4);

            //第二张柱状图 BMPM金额
            ChartControl newChart1 = new ChartControl();
            newChart1.Legend.Name = "Default Legend";
            newChart1.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            newChart1.Size = new System.Drawing.Size(100, 250);
            newChart1.TabIndex = 0;
            newChart1.DataSource = dt;
            newChart1.Dock = DockStyle.Top;
            newChart1.SeriesDataMember = "PRICETYPE";
            newChart1.SeriesTemplate.ArgumentDataMember = "月份";
            newChart1.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "PRICE" });
            newChart1.SeriesTemplate.View = new StackedBarSeriesView();
            newChart1.SeriesTemplate.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            newChart1.Titles.Clear();

            ChartTitle titles1 = new ChartTitle();

            titles1.Text = "BMPM金额";                            //名称 
            titles1.TextColor = System.Drawing.Color.Black;   //颜色 
            titles1.Indent = 1;                                //设置距离  值越小柱状图就越大 
            titles1.Font = new Font("Tahoma", 10, FontStyle.Bold);            //设置字体 
            titles1.Dock = ChartTitleDockStyle.Top;           //设置对齐方式 
            titles1.Indent = 0;
            titles1.Alignment = StringAlignment.Center;       //居中对齐 
            newChart1.Titles.Add(titles1);


            ChartTitle titles4 = new ChartTitle();
            titles4.TextColor = System.Drawing.Color.Black;
            titles4.Alignment = StringAlignment.Near;
            titles4.Font = new Font("Tahoma", 10, FontStyle.Bold);
            titles4.Visibility = DevExpress.Utils.DefaultBoolean.Default;
            titles4.Text = "￥";
            newChart1.Titles.Add(titles4);
            //totalprice折线图
            Series series2 = new Series("TotalPrice", ViewType.Line);
            series2.ArgumentScaleType = ScaleType.Qualitative;
            series2.ValueScaleType = ScaleType.Numerical;

            series2.DataSource = dt;
            series2.ArgumentDataMember = "月份";
            series2.ValueDataMembers[0] = "TOTALPRICE";
            newChart1.Series.Add(series2);
            //OUTPUT折线图
            Series series5 = new Series("OUTPUT(MPCS) ", ViewType.Line);
            series5.ArgumentScaleType = ScaleType.Qualitative;
            series5.ValueScaleType = ScaleType.Numerical;

            series5.DataSource = dt;
            series5.ArgumentDataMember = "月份";
            series5.ValueDataMembers[0] = "OUTPUT";
            newChart1.Series.Add(series5);


            //第三张柱状图 BMPM人次
            ChartControl newChart2 = new ChartControl();
            newChart2.Legend.Name = "Default Legend";
            newChart2.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            newChart2.Size = new System.Drawing.Size(100, 250);
            newChart2.TabIndex = 0;
            newChart2.DataSource = dt;
            newChart2.Dock = DockStyle.Top;
            newChart2.SeriesDataMember = "PEOPLETYPE";
            newChart2.SeriesTemplate.ArgumentDataMember = "月份";
            newChart2.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "NUM" });

            newChart2.SeriesTemplate.View = new StackedBarSeriesView();
            newChart2.SeriesTemplate.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            newChart2.Titles.Clear();


            ChartTitle titles2 = new ChartTitle();
            titles2.Text = "BMPM维修人次";                            //名称 
            titles2.TextColor = System.Drawing.Color.Black;   //颜色 
            titles2.Indent = 1;                                //设置距离  值越小柱状图就越大 




            titles2.Font = new Font("Tahoma", 10, FontStyle.Bold);            //设置字体 
            titles2.Dock = ChartTitleDockStyle.Top;           //设置对齐方式 
            titles2.Indent = 0;
            titles2.Alignment = StringAlignment.Center;       //居中对齐 
            newChart2.Titles.Add(titles2);
            //totalnum折线图
            Series series3 = new Series("TotalNum", ViewType.Line);
            series3.ArgumentScaleType = ScaleType.Qualitative;
            series3.ValueScaleType = ScaleType.Numerical;

            series3.DataSource = dt;
            series3.ArgumentDataMember = "月份";
            series3.ValueDataMembers[0] = "TOTALNUM";

            //第二个X,Y坐标 OUTPUT折线图
            SecondaryAxisX myAxisX = new SecondaryAxisX("月份");
            SecondaryAxisY myAxisY = new SecondaryAxisY("OUTPUT");

            ((XYDiagram)newChart2.Diagram).SecondaryAxesX.Add(myAxisX);
            ((XYDiagram)newChart2.Diagram).SecondaryAxesY.Add(myAxisY);

            ((LineSeriesView)series3.View).AxisX = myAxisX;
            ((LineSeriesView)series3.View).AxisY = myAxisY;

            Series series6 = new Series("OUTPUT(MPCS) ", ViewType.Line);


            //series6.DataSource = dt;
            //series6.ArgumentDataMember = "月份";
            //series6.ValueDataMembers[0] = "OUTPUT";

            newChart2.Series.Add(series3);
            *//*Series series6 = new Series("OUTPUT(MPCS) ", ViewType.Line);
            series6.ArgumentScaleType = ScaleType.Qualitative;
            series6.ValueScaleType = ScaleType.Numerical;

            series6.DataSource = dt;
            series6.ArgumentDataMember = "TIME";
            series6.ValueDataMembers[0] = "OUTPUT";*//*
            newChart2.Series.Add(series6);

            this.splitContainerControl1.Panel1.Controls.Add(newChart2);
            this.splitContainerControl1.Panel1.Controls.Add(newChart1);
            this.splitContainerControl1.Panel1.Controls.Add(newChart);


            splitContainerControl1.Panel1.AutoScroll = true;*/
        }


        /*public void GridViewStyle(GridView gridView)
        {
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.BestFitColumns();
            if (gridView.Columns.Contains(gridView.Columns.ColumnByFieldName("TIMEKEY")))
            {
                gridView.Columns["TIMEKEY"].Caption = "月份";
            }
            if (gridView.Columns.Contains(gridView.Columns.ColumnByFieldName("BMTIME")))
            {
                gridView.Columns["BMTIME"].Caption = "BM总时间";
            }
            if (gridView.Columns.Contains(gridView.Columns.ColumnByFieldName("PMTIME")))
            {
                gridView.Columns["PMTIME"].Caption = "PM总时间";
            }
            if (gridView.Columns.Contains(gridView.Columns.ColumnByFieldName("BMPRICE")))
            {
                gridView.Columns["BMPRICE"].Caption = "BM金额";
            }
            if (gridView.Columns.Contains(gridView.Columns.ColumnByFieldName("PMPRICE")))
            {
                gridView.Columns["PMPRICE"].Caption = "PM金额";
            }
            if (gridView.Columns.Contains(gridView.Columns.ColumnByFieldName("BMNUM")))
            {
                gridView.Columns["BMNUM"].Caption = "BM人次";
            }
            if (gridView.Columns.Contains(gridView.Columns.ColumnByFieldName("PMNUM")))
            {
                gridView.Columns["PMNUM"].Caption = "PM人次";
            }

        }*/

        #endregion

        #region event
        private void tbnSeach_Click(object sender, EventArgs e)
        {
            try
            {
                //ComboBoxControl.SetCrossLang(this._translator);
                if (!base.ValidateUserInput(this.layoutControl1)) return;
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }








        #endregion

        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {



            DataRowView dataRowView = (DataRowView)e.Row;
            //dataRowView.Row.ItemArray;
            args.CATEGORY = dataRowView.Row["CATEGORY"].ToString();
            args.SUBCATEGORY = dataRowView.Row["SUBCATEGORY"].ToString();
            args.NAME = dataRowView.Row["NAME"].ToString();
            args.USED = dataRowView.Row["USED"].ToString();
            args.CUSTOM01 = dataRowView.Row["CUSTOM01"].ToString();
            args.CUSTOM02 = dataRowView.Row["CUSTOM02"].ToString();
            args.CUSTOM03 = dataRowView.Row["CUSTOM03"].ToString();
            args.CUSTOM04 = dataRowView.Row["CUSTOM04"].ToString();
            args.CUSTOM05 = dataRowView.Row["CUSTOM05"].ToString();
            args.CUSTOM06 = dataRowView.Row["CUSTOM06"].ToString();
            args.CUSTOM07 = dataRowView.Row["CUSTOM07"].ToString();

            


            if (string.IsNullOrEmpty(dataRowView.Row["ID"].ToString()))
            {

                for (int i = 0; i < dataRowView.Row.ItemArray.Length; i++)
                {
                    if (string.IsNullOrEmpty(dataRowView.Row.ItemArray[i].ToString()) && i != 0)
                    {
                        XtraMessageBox.Show("Please enter all values", "Warring", MessageBoxButtons.YesNo);
                        dataRowView.Row.Delete();
                        return;

                    }
                    
                }

                int Res = bs.ExecuteModify("NewTCODE", args.getPack());

                if (Res == 1)
                {
                    XtraMessageBox.Show("Adding data succeeded ", " ");
                }
                return;

            }

            else {
                args.ROWID = dataRowView.Row["ID"].ToString();
                int Res = bs.ExecuteModify("UpdateTCODE", args.getPack());
            }

            
            //string a = "qq";


        }
 
    }
}
