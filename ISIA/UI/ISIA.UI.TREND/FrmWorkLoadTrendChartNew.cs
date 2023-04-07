using ISIA.COMMON;
using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;
using ISIA.UI.TREND.Dto;
using Steema.TeeChart;
using Steema.TeeChart.Components;
using Steema.TeeChart.Styles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.UI;

namespace ISIA.UI.TREND
{
    public partial class FrmWorkLoadTrendChartNew : DockUIBase1T1
    {

        DataSet dataSet = new DataSet();
        DataSet dataSetTB = new DataSet();
        protected PointF _pStart;
        protected PointF _pEnd;
        private bool bfirst = false;
        BizDataClient bs;
        AwrArgsPack args = new AwrArgsPack();
        object[] result = new object[2];


        public FrmWorkLoadTrendChartNew()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.WorkloadTrendChart");
            dtpStartTime.DateTime = DateTime.Now.AddYears(-1);
            dtpEndTime.DateTime = DateTime.Now;
        }

        private void comboBoxEditGroupUnit_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBoxEditGroupUnit.Text == "DAY")
            {
                this.dtpStartTime.DateTime = DateTime.Now.AddDays(-1);
                this.dtpEndTime.DateTime = DateTime.Now;
            }
            else if (comboBoxEditGroupUnit.Text == "WEEK")
            {
                this.dtpStartTime.DateTime = DateTime.Now.AddDays(-7);
                this.dtpEndTime.DateTime = DateTime.Now;
            }
            else if (comboBoxEditGroupUnit.Text == "MONTH")
            {
                this.dtpStartTime.DateTime = DateTime.Now.AddMonths(-1);
                this.dtpEndTime.DateTime = DateTime.Now;
            }
            else if (comboBoxEditGroupUnit.Text == "QUARTER")
            {
                this.dtpStartTime.DateTime = DateTime.Now.AddMonths(-3);
                this.dtpEndTime.DateTime = DateTime.Now;
            }
            else if (comboBoxEditGroupUnit.Text == "YEAR")
            {
                this.dtpStartTime.DateTime = DateTime.Now.AddYears(-1);
                this.dtpEndTime.DateTime = DateTime.Now;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cmbDbName.Text))
                {
                    string errMessage = "Please select DB_NAME";
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, TAP.UI.EnumMsgType.WARNING, errMessage);
                    return;
                }
                if (string.IsNullOrEmpty(cmbInstance.Text))
                {
                    string errMessage = "Please select Instance";
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, TAP.UI.EnumMsgType.WARNING, errMessage);
                    return;
                }

                //ComboBoxControl.SetCrossLang(this._translator);
                if (!base.ValidateUserInput(this.lcSerachOptions)) return;
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public DataSet LoadData()
        {
            args.StartTime = dtpStartTime.DateTime.ToString("yyyy-MM-dd");
            args.EndTime = dtpEndTime.DateTime.ToString("yyyy-MM-dd");

            dataSet = bs.ExecuteDataSet("GetWorkLoadTrend", args.getPack());
            return dataSet;
        }
        public void DisplayData(DataSet ds)
        {
            if (dataSet == null)
            {
                return;
            }
            //ConvertData(dataSet);

            CreateTeeChart(dataSet.Tables[0]);
            dataSet.Tables.Clear();

        }
        private DataTable ConvertDTToListRef(List<WorkLoadInfo> list)
        {
            WorkLoadInfo dto = new WorkLoadInfo();
            PropertyInfo[] fields = dto.GetType().GetProperties();
            List<string> listTotal = new List<string>();
            List<string> workLoadsPara = new List<string>();
            foreach (var item in AwrArgsPack.WorkloadParamNamesList)
            {
                workLoadsPara.Add(item.ToUpper());
            }
            for (int i = 0; i < fields.Length; i++)
            {
                
                string ss = fields[i].Name;
                if (workLoadsPara.Contains(ss))
                {
                    listTotal.Add(ss);
                }
            }
            List<SqlStatRowDto> returnList = new List<SqlStatRowDto>();

            foreach (WorkLoadInfo item in list)
            {
                PropertyInfo[] proInfo = item.GetType().GetProperties();
                foreach (var s in listTotal)
                {
                    SqlStatRowDto rowDto = new SqlStatRowDto();
                    //rowDto.SQL_ID = item.SQL_ID;
                    rowDto.PARAMENT_NAME = s;
                    rowDto.END_INTERVAL_TIME = item.END_TIME;
                    //rowDto.SNAP_ID = item.SNAP_ID;
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
        private void CreateTeeChart(DataTable dsTable)
        {
            List<WorkLoadInfo> workLoadList = DataTableExtend.GetList<WorkLoadInfo>(dsTable);

            panelControl1.Controls.Clear();

            ChartLayout chartLayout1 = new ChartLayout();


            panelControl1.Controls.Add(chartLayout1);

            chartLayout1.Dock = DockStyle.Fill;

            chartLayout1.Charts.Clear();
            chartLayout1.Refresh();
            dataSetTB = new DataSet();
            DataTable dataTable = ConvertDTToListRef(workLoadList);
            List<SqlShow> list = DataTableExtend.GetList<SqlShow>(dataTable);
            DataTable dtKeyValue = DataTableExtend.ConvertToDataSet(list).Tables[0];
            IEnumerable<IGrouping<string, DataRow>> result = dtKeyValue.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["PARAMENT_NAME"].ToString());
            if (result != null && result.Count() > 0)
            {
                foreach (IGrouping<string, DataRow> rows in result)
                {
                    DataTable dataTable1 = rows.ToArray().CopyToDataTable();
                    dataTable1.TableName = rows.Key;
                    if (dataTable1.Rows.Count > 0)
                    {
                        dataSetTB.Tables.Add(dataTable1);
                    }
                }
            }
            if (dataSetTB.Tables.Count > 1)
            {

                foreach (DataTable dt in dataSetTB.Tables)
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
                chart.Axes.Bottom.Labels.DateTimeFormat = "MM-dd";
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
            line.Color = Color.OrangeRed;
            //line.ColorEachLine = true;
            //line.Legend.Text = dstable.TableName;
            line.Legend.BorderRound = 10; 
            line.Pointer.Style = PointerStyles.Circle;
            line.Pointer.Visible = true;
            line.Pointer.Color = Color.Black;
            //line.Pointer.SizeDouble = 1;
            line.XValues.DateTime = true;
            return line;
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
                    SerachDataPoint(_pStart, _pEnd, sender as TChart);
                }
            }
        }

        private void tChart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _pStart.X = (float)e.X;
                _pStart.Y = (float)e.Y;

                bfirst = true;
            }

        }
        private void SerachDataPoint(PointF pStart, PointF pEnd, TChart chart)
        {

            try
            {
                List<SnapshotDto> snaplist = new List<SnapshotDto>();
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
                            //dto.SNAP_ID = (decimal)dt1.Rows[i]["SNAP_ID"];//SQL_ID
                            //dto.DBID = (decimal)dt1.Rows[i]["DBID"];//SQL_ID
                            dto.END_INTERVAL_TIME = (DateTime)dt1.Rows[i]["END_INTERVAL_TIME"];
                            snaplist.Add(dto);
                        }
                    }
                }
                if (!snaplist.Any())
                {
                    return;
                }


                //this._DataTable = DataTableExtend.ConvertToDataSet<SnapshotDto>(snaplist).Tables[0];

                //PopupGrid popupGrid = new PopupGrid(this._DataTable);
                //popupGrid.StartPosition = FormStartPosition.CenterScreen;
                //popupGrid.ShowDialog();
                //DataTable dt = popupGrid._DataTable;

                //if (popupGrid.linkage == true)
                //{
                //    base.OpenUI("SQLANALYSISBYSQL_ID", "AWR", "SQLANALYSISBYSQL_ID", dt);
                //}
            }
            catch
            {

            }


        }
    }

    public class WorkLoadInfo
    {
        public decimal PARAMENT_VALUE { get; set; }
        public DateTime END_TIME { get; set; }
        public decimal CPU_UTIL_PCT { get; set; }
        public decimal CPU_UTIL_PCT_MAX { get; set; }
        public decimal LOGICAL_READS_PSEC { get; set; }
        public decimal PHYSICAL_READS_PSEC { get; set; }
        public decimal PHYSICAL_WRITES_PSEC { get; set; }
        public decimal EXECS_PSEC_AVG { get; set; }
        public decimal EXECS_PSEC_MAX { get; set; }
        public decimal USER_CALLS_PSEC { get; set; }
        public decimal HARD_PARSE_CNT_PSEC { get; set; }
        public decimal DB_BLOCK_CHANGES_PSEC { get; set; }
        public decimal SQL_SERVICE_RESPONSE_TIME { get; set; }
        public decimal COMMIT_PSEC_AVG { get; set; }
        public decimal REDO_MB_PSEC_AVG { get; set; }
        public decimal DLM_MB_PSEC { get; set; }
        public decimal NET_MB_TO_CLIENT_PSEC { get; set; }
        public decimal NET_MB_FROM_CLIENT_PSEC { get; set; }
        public decimal NET_MB_FROM_DBLINK_PSEC { get; set; }
        public decimal NET_MB_TO_DBLINK_PSEC { get; set; }
    }
}
