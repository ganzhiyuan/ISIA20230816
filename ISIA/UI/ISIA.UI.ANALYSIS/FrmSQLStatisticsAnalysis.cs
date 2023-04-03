
using DevExpress.XtraEditors.Controls;
using ISIA.COMMON;
using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;
using ISIA.UI.ANALYSIS.Dto;
using Steema.TeeChart;
using Steema.TeeChart.Components;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP;
using TAP.Data.Client;
using TAP.UI;
using TAP.UIControls.BasicControlsDEV;
using EnumDataObject = TAP.UI.EnumDataObject;

namespace ISIA.UI.ANALYSIS
{
    public partial class FrmSQLStatisticsAnalysis : DockUIBase1T1
    {

        #region Feild
        BizDataClient bs = null;
        DataSet dataSet = new DataSet();
        List<SqlStatRowDto> returnList = null;
        DataSet dataSet1 = new DataSet();
        protected PointF _pStart;
        protected PointF _pEnd;
        private bool bfirst = false;
        List<SnapshotDto> snaplist = new List<SnapshotDto>();

        AwrCommonArgsPack args = new AwrCommonArgsPack();

        ComboBoxControl CBC = new ComboBoxControl();
        #endregion
        public FrmSQLStatisticsAnalysis()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.SQLAnalysisChart");

            tcmbday.Text = "D";
            tcmbday.Items.Add("D");
            tcmbday.Items.Add("W");
            tcmbday.Items.Add("M");
            /*this.dateStart.DateTime = DateTime.Now.AddDays(-1);
            this.dateEnd.DateTime = DateTime.Now;*/
        }

        public DataSet LoadData()
        {
            
            /*if (dataSet.Tables.Count == 0)
            {*/
                dataSet1.Tables.Clear();
                args.DbId = string.IsNullOrEmpty(cmbDbName.Text) ? "" : cmbDbName.Text.Split('(')[1];
                args.DbId = args.DbId.Substring(0, args.DbId.Length - 1);
                args.DbName = string.IsNullOrEmpty(cmbDbName.Text) ? "" : cmbDbName.Text.Split('(')[0];
                args.StartTimeKey = dateStart.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                args.EndTimeKey = dateEnd.DateTime.ToString("yyyy-MM-dd HH:mm:ss");


                List<string> itemList = cboParaName.Text.Split(',').ToArray().ToList();
                dataSet = bs.ExecuteDataSet("GetSnap", args.getPack());
                if (dataSet.Tables[0].Rows.Count == 0)
                {
                    return dataSet = null;
                }

                DataTable dataTable = ConvertDTToListRef(dataSet.Tables[0]);
                List<SqlShow> list = DataTableExtend.GetList<SqlShow>(dataTable);
                //list = list.Where(x => itemList.Contains(x.PARAMENT_NAME)).ToList();
                List<SqlShow> list1 = (from d in list where cboParaName.Text.ToString().Contains(d.PARAMENT_NAME) select d).ToList();

                DataTable dt = DataTableExtend.ConvertToDataSet(list1).Tables[0];
                dataSet1.Tables.Add(dt.Copy());
                dataSet1.Tables[0].TableName = "TABLE";
                return dataSet;
            /*}
            else
            {
                dataSet1.Tables.Clear();
                //dataSet = bs.ExecuteDataSet("GetSnap");
                //DataTable dt = ConvertDTToListRef(dataSet.Tables[0]);
                //dataSet1.Tables.Add(dt.Copy());
                dataSet1.Tables.Add(dataSet.Tables[0].Copy());
                dataSet1.Tables[0].TableName = "TABLE";
                return dataSet;
            }*/
            
        }


        public void DisplayData(DataSet ds)
        {
            if (dataSet == null)
            {
                return;
            }

            CreateTeeChart(dataSet1.Tables[0]);
            dataSet.Tables.Clear();
           
        }

        private void CreateTeeChart(DataTable dsTable)
        {
            panelControl2.Controls.Clear();

            ChartLayout chartLayout1 = new ChartLayout();


            panelControl2.Controls.Add(chartLayout1);

            chartLayout1.Dock = DockStyle.Fill;

            chartLayout1.Charts.Clear();
            chartLayout1.Refresh();
            IEnumerable<IGrouping<string, DataRow>> result = dsTable.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["PARAMENT_NAME"].ToString());
            if (result != null && result.Count() > 0)
            {
                foreach (IGrouping<string, DataRow> rows in result)
                {
                    DataTable dataTable = rows.ToArray().CopyToDataTable();
                    dataTable.TableName = rows.Key;
                    if (dataTable.Rows.Count > 0)
                    {
                        dataSet1.Tables.Add(dataTable);
                    }
                }
            }
            if (dataSet1.Tables.Count > 1)
            {

                foreach (DataTable dt in dataSet1.Tables)
                {
                    if (dt.TableName != "TABLE")
                    {
                        Line line = CreateLine(dt);
                        chartLayout1.Add(dt.TableName).Series.Add(line);
                        
                    }
                }
            }

            foreach (TChart chart in chartLayout1.Charts)
            {
                //chart.Legend.Visible = true;
                //chart.Legend.LegendStyle = LegendStyles.Series;
                chart.Axes.Bottom.Labels.DateTimeFormat = "MM-dd HH:MI";
                chart.Axes.Bottom.Labels.ExactDateTime = true;//x轴显示横坐标为时间
                chart.MouseDown += tChart1_MouseDown;
                chart.MouseUp += tChart1_MouseUp;
                //chart.Panning.Allow = ScrollModes.None;
                //chart.Zoom.Direction = ZoomDirections.None;
                chart.Panning.Allow = ScrollModes.None;
            }

            
            return;
        }



        private Line CreateLine(DataTable dstable)
        {
            Line line = new Line();
           
            line.DataSource = dstable;
            line.YValues.DataMember = "PARAMENT_VALUE";
            line.XValues.DataMember = "END_INTERVAL_TIME";
            line.Legend.Visible = false;
            line.Color = Color.Red;
            //line.ColorEachLine = true;
            //line.Legend.Text = dstable.TableName;
            line.Legend.BorderRound = 10;
            line.XValues.DateTime = true;
            return line;
        }
        /*private void BindData()
        {
           
        }*/


        


        private DataTable ConvertDTToListRef(DataTable dt)
        {
            List<SqlstatDto> list = DataTableExtend.GetList<SqlstatDto>(dt);
            SqlstatDto dto = new SqlstatDto();
            PropertyInfo[] fields = dto.GetType().GetProperties();
            List<string> listTotal = new List<string>();
            for (int i = 0; i < fields.Length; i++)
            {
                string ss = fields[i].Name;
                if (ss.Contains("TOTAL"))
                {
                    listTotal.Add(ss);
                }
            }
            returnList = new List<SqlStatRowDto>();

            foreach (SqlstatDto item in list)
            {
                PropertyInfo[] proInfo = item.GetType().GetProperties();
                foreach (var s in listTotal)
                {
                    SqlStatRowDto rowDto = new SqlStatRowDto();
                    rowDto.DBID = item.DBID;
                    //rowDto.SQL_ID = item.SQL_ID;
                    rowDto.PARAMENT_NAME = s;
                    rowDto.END_INTERVAL_TIME = item.END_INTERVAL_TIME;
                    rowDto.SNAP_ID = item.SNAP_ID;
                    foreach (PropertyInfo para in proInfo)
                    {
                        if (s == para.Name)
                        {
                            rowDto.PARAMENT_VALUE = Convert.ToDecimal(para.GetValue(item, null).ToString());
                        }
                    }
                    returnList.Add(rowDto);
                }

            }
            DataTable dt1 = DataTableExtend.ConvertToDataSet<SqlStatRowDto>(returnList).Tables[0];
            return dt1;
        }


        private void tChart1_MouseUp(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Left && bfirst)
            {
                _pEnd.X = (float)e.X;
                _pEnd.Y = (float)e.Y;
                bfirst = false;
                if (_pStart != _pEnd)
                {
                    SerachDataPoint(_pStart, _pEnd , sender as TChart);
                }
            }
        }

        private void tChart1_MouseDown(object sender, MouseEventArgs e )
        {
            if (e.Button == MouseButtons.Left)
            {
                _pStart.X = (float)e.X;
                _pStart.Y = (float)e.Y;

                bfirst = true;
            }

        }

        private void SerachDataPoint(PointF pStart, PointF pEnd , TChart chart)
        {

            try
            {
                snaplist = new List<SnapshotDto>();
                float minX;
                float minY;
                float maxX;
                float maxY;
                if (pStart.X < pEnd.X)
                {
                    minX = pStart.X;
                    maxX = pEnd.X;
                }
                else
                {
                    minX = pEnd.X;
                    maxX = pStart.X;
                }
                if (pStart.Y < pEnd.Y)
                {
                    minY = pStart.Y;
                    maxY = pEnd.Y;
                }
                else
                {
                    minY = pEnd.Y;
                    maxY = pStart.Y;
                }

                foreach (Line line in chart.Chart.Series)
                {
                    for (int i = 0; i < line.Count; i++)
                    {
                        if (line.CalcXPos(i) >= minX && line.CalcXPos(i) < maxX && line.CalcYPos(i) >= minY && line.CalcYPos(i) <= maxY)
                        {
                            SnapshotDto dto = new SnapshotDto();
                            //dto.SQL_ID = ((System.Data.DataTable)line.DataSource).TableName; //snap_id
                            dto.PARAMENT_NAME = ((System.Data.DataTable)line.DataSource).TableName;
                            //double value = line[i].Y;//VALUE
                            //dto.Value = line[i].Y.ToString();//value
                            dto.PARAMENT_VALUE = (decimal)line[i].Y;//value
                            //int xValue = Convert.ToInt32(line[i].X);//ROWNUM


                            DataTable dt1 = line.DataSource as DataTable;
                            dto.SNAP_ID = (decimal)dt1.Rows[i]["SNAP_ID"];//SQL_ID
                            dto.DBID = (decimal)dt1.Rows[i]["DBID"];//SQL_ID
                            dto.END_INTERVAL_TIME = (DateTime)dt1.Rows[i]["END_INTERVAL_TIME"];
                            snaplist.Add(dto);
                        }
                    }
                }
                if (!snaplist.Any())
                {
                    return;
                }


                this._DataTable = DataTableExtend.ConvertToDataSet<SnapshotDto>(snaplist).Tables[0];

                PopupGrid popupGrid = new PopupGrid(this._DataTable);
                popupGrid.StartPosition = FormStartPosition.CenterScreen;
                popupGrid.ShowDialog();
                DataTable dt = popupGrid._DataTable;

                if (popupGrid.linkage == true)
                {
                    base.OpenUI("SQLANALYSISBYSQL_ID", "AWR", "SQLANALYSISBYSQL_ID", dt);
                }
            }
            catch 
            {
                
            }
            
            
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

                    //dataSet.Tables.Add(tmpdt.Copy());

                    List<SqlShow> list = DataTableExtend.GetList<SqlShow>(tmpdt);
                    List<string> a = list.Select(x => x.PARAMENT_NAME).Distinct().ToList();
                    string[] b = a.ToArray();
                    string para = string.Join(",",b);

                    DataRow row1 = tmpdt.Rows[0];
                    DataRow row2 = tmpdt.Rows[tmpdt.Rows.Count - 1];

                    

                    SelectedComboBox(cboParaName, para);
                    SelectedDBComboBox(cmbDbName, row1["DBID"].ToString());
                    dateStart.DateTime = (DateTime)row1["END_INTERVAL_TIME"];
                    dateEnd.DateTime = (DateTime)row2["END_INTERVAL_TIME"];

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
            tbnSeach_Click(null, null);


        }

        private void tbnSeach_Click(object sender, EventArgs e)
        {
            
            


            try
            {
                
                
                //ComboBoxControl.SetCrossLang(this._translator);
                if (!base.ValidateUserInput(this.lcSerachOptions)) return;
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void tcmbday_EditValueChanged(object sender, EventArgs e)
        {
            if (tcmbday.Text == "D")
            {
                this.dateStart.DateTime = DateTime.Now.AddDays(-1);
                this.dateEnd.DateTime = DateTime.Now;
            }
            else if (tcmbday.Text == "W")
            {
                this.dateStart.DateTime = DateTime.Now.AddDays(-7);
                this.dateEnd.DateTime = DateTime.Now;
            }
            else if (tcmbday.Text == "M")
            {
                this.dateStart.DateTime = DateTime.Now.AddMonths(-1);
                this.dateEnd.DateTime = DateTime.Now;
            }
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
                ComboBox.Properties.Items.Add(item[2].ToString());
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
