using DevExpress.Utils;
using ISIA.COMMON;
using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;
using ISIA.UI.TREND.Dto;
using Steema.TeeChart;
using Steema.TeeChart.Components;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;
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
        DataSet ParamentRelationDS = new DataSet();
        public string groupUnit { get; set; }
        List<Color> colors = new List<Color> { Color.FromArgb(218,129,55),Color.Red, Color.FromArgb(74, 126, 187), Color.FromArgb(190, 75, 72), Color.FromArgb(152,185,84),
            Color.FromArgb(125,96,160), Color.FromArgb(70,170,197)  };


        public FrmWorkLoadTrendChartNew()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.WorkloadTrendChart");
            this.dtpStartTime.DateTime = DateTime.Now.AddYears(-1);
            this.dtpEndTime.DateTime = DateTime.Now;
            cmbGroupUnit.EditValue = "DAY";
        }

        private void comboBoxEditGroupUnit_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbGroupUnit.EditValue.ToString() == "DAY")
            {
                this.dtpStartTime.DateTime = DateTime.Now.AddYears(-1);
                this.dtpEndTime.DateTime = DateTime.Now;
            }
            else if (cmbGroupUnit.EditValue.ToString() == "INTERVAL")
            {
                this.dtpStartTime.DateTime = DateTime.Now.AddDays(-6);
                this.dtpEndTime.DateTime = DateTime.Now;
            }
            //else if (comboBoxEditGroupUnit.Text == "MONTH")
            //{
            //    this.dtpStartTime.DateTime = DateTime.Now.AddMonths(-1);
            //    this.dtpEndTime.DateTime = DateTime.Now;
            //}
            //else if (comboBoxEditGroupUnit.Text == "QUARTER")
            //{
            //    this.dtpStartTime.DateTime = DateTime.Now.AddMonths(-3);
            //    this.dtpEndTime.DateTime = DateTime.Now;
            //}
            //else if (comboBoxEditGroupUnit.Text == "YEAR")
            //{
            //    this.dtpStartTime.DateTime = DateTime.Now.AddYears(-1);
            //    this.dtpEndTime.DateTime = DateTime.Now;
            //}
            groupUnit = cmbGroupUnit.EditValue.ToString();
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
            args.StartTime = dtpStartTime.DateTime.ToString("yyyyMMdd");
            args.EndTime = dtpEndTime.DateTime.ToString("yyyyMMdd");
            args.DBName = cmbDbName.Text.Split('(')[0];
            args.DBID = cmbDbName.EditValue.ToString();
            args.INSTANCE_NUMBER = cmbInstance.EditValue.ToString();
            if (cmbGroupUnit.EditValue.ToString()=="DAY")
            {
                dataSet = bs.ExecuteDataSet("GetWorkLoadTrend", args.getPack());

            }
            else if (cmbGroupUnit.EditValue.ToString() == "INTERVAL")
            {
                dataSet = bs.ExecuteDataSet("GetWorkLoadTrendForInterval", args.getPack());

            }


            ParamentRelationDS = bs.ExecuteDataSet("GetParamentRelation");
            groupUnit = cmbGroupUnit.EditValue.ToString();
            //cmbLinePara.EditValue = null;
            //cmbLinePara.Text = "";
            return dataSet;
        }
        public void DisplayData(DataSet ds)
        {
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null)
            {
                return;
            }
            //ConvertData(dataSet);

            CreateTeeChart(dataSet.Tables[0]);
            //dataSet.Tables.Clear();

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
                    string ss = item.WORKDATE;
                    //string ss = item.BEGIN_TIME.ToString();
                    if (groupUnit == "INTERVAL")
                    {
                        //rowDto.END_INTERVAL_TIME = DateTime.ParseExact(ss, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        rowDto.END_INTERVAL_TIME = item.BEGIN_TIME;

                    }
                    else
                        rowDto.END_INTERVAL_TIME = DateTime.ParseExact(ss,"yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    rowDto.SNAP_ID = item.SNAP_ID_MIN;
                    rowDto.INSTANCE_NUMBER = item.INSTANCE_NUMBER;
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
            if (workLoadList == null || !workLoadList.Any())
            {
                return;
            }
            List<string> arrayStr = cmbLinePara.EditValue.ToString().Replace(", ", ",").Split(',').ToList();
            dataSetTB = new DataSet();
            DataTable dataTable = ConvertDTToListRef(workLoadList);
            List<SqlStatRowDto> list = DataTableExtend.GetList<SqlStatRowDto>(dataTable);
            list = list.Where(x => arrayStr.Contains(x.PARAMENT_NAME)).ToList();
            if (list.Any())
            {

                DataTable dt = DataTableExtend.ConvertToDataSet<SqlStatRowDto>(list).Tables[0];
                SetCharts( dt);
            }
            else
            {
                SetCharts(dataTable);
            }
            //SetCharts(chartLayout1, dataTable);
        }

        private void SetCharts(DataTable dataTable)
        {
            List<SqlShow> list = DataTableExtend.GetList<SqlShow>(dataTable);
            DataTable dtKeyValue = DataTableExtend.ConvertToDataSet(list).Tables[0];
            IEnumerable<IGrouping<string, DataRow>> result = dtKeyValue.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["PARAMENT_NAME"].ToString());
            if (result != null && result.Count() > 0)
            {
                foreach (IGrouping<string, DataRow> rows in result)
                {
                    DataTable dataTable1 = rows.ToArray().CopyToDataTable();
                    //if (rows.Key== "PHYSICAL_WRITES_PSEC")
                    //{
                    //    foreach (DataRow dataRow in dataTable1.Rows)
                    //    {
                    //        dataRow["PARAMENT_VALUE"] = Math.Round(Convert.ToDecimal(dataRow["PARAMENT_VALUE"]) / 8192,6);
                    //    }
                    //}
                    dataTable1.TableName = rows.Key;
                    if (dataTable1.Rows.Count > 0)
                    {
                        dataSetTB.Tables.Add(dataTable1);
                    }
                }
            }
            if (dataSetTB.Tables.Count > 0)
            {
                flowLayoutPanel1.Controls.Clear();
                int width = flowLayoutPanel1.ClientSize.Width / Convert.ToInt32(3);
                int height = flowLayoutPanel1.ClientSize.Height / Convert.ToInt32(3);
                for (int i = 0; i < dataSetTB.Tables.Count; i++)
                {
                    var maxValue = dataSetTB.Tables[i].AsEnumerable().Max(x => x.Field<decimal>("PARAMENT_VALUE"));
                    TChart tChart = CreateCharts(AwrArgsPack.WorkloadNewParmMapping[dataSetTB.Tables[i].TableName],width, height, i, maxValue);

                    // 将 TChart 控件添加到 FlowLayoutPanel 中
                    flowLayoutPanel1.Controls.Add(tChart);

                    List<SqlShow> listSqlShows = DataTableExtend.GetList<SqlShow>(dataSetTB.Tables[i]);
                    var listNum = listSqlShows.Select(x => x.INSTANCE_NUMBER).Distinct().ToList();
                    foreach (var item in listNum)
                    {
                        var listTem = listSqlShows.Where(x => x.INSTANCE_NUMBER == item).ToList();
                        DataTable dtTem = DataTableExtend.ConvertToDataSet(listTem).Tables[0];
                        Line line = CreateLine(dtTem, item);

                        tChart.Series.Add(line);
                    }

                    /*var cuTool = new Steema.TeeChart.Tools.CursorTool(tChart.Chart)
                    {
                        Style = CursorToolStyles.Vertical,
                        FollowMouse = false,
                    };
                    cuTool.Pen.Color = Color.Red;
                    cuTool.Pen.Visible = true;
                    
                    double dateTime = DateTime.Now.AddDays(-60).ToOADate();
                    cuTool.YValue = dateTime;*/

                   

                }
            }
        }

        private TChart CreateCharts(string strName,int width, int height, int i,decimal maxValue)
        {
            // 创建 TChart 控件
            TChart tChart = new TChart();
            var markstip = new MarksTip(tChart.Chart);
            if (strName.Contains("PSEC"))
            {
                strName = strName.Substring(0, strName.Length - 5);
            }
            tChart.Text = strName ;
            tChart.Tag = i;
            tChart.Width = width - 10;
            tChart.Height = height - 10;
            tChart.Axes.Bottom.Labels.DateTimeFormat = "MM-dd";
            tChart.Axes.Bottom.Labels.ExactDateTime = true;//x轴显示横坐标为时间
            tChart.Axes.Left.Minimum = 0; //设置左侧轴的最小值为0
            tChart.Axes.Left.Maximum = Convert.ToDouble(maxValue);
            tChart.Axes.Left.Automatic = false; // 禁用自动计算
            tChart.Axes.Left.Increment = CalculateIncrement(Convert.ToDouble(maxValue)); // 设置刻度增量
            tChart.Axes.Left.Maximum = Math.Round(Convert.ToDouble(maxValue)+ tChart.Axes.Left.Increment*2,2);
            //tChart.Axes.Left.AutomaticMinimum = false;
            //tChart.Axes.Right.Minimum = 0; //设置右侧轴的最小值为0
            tChart.MouseDown += tChart1_MouseDown;
            tChart.MouseUp += tChart1_MouseUp;
            tChart.ClickSeries += Chart_ClickSeries;
            //chart.Panning.Allow = ScrollModes.None;
            //chart.Zoom.Direction = ZoomDirections.None;
            tChart.Panning.Allow = ScrollModes.None;
            tChart.DoubleClick += TChart_DoubleClick;

            return tChart;
        }

        private void TChart_DoubleClick(object sender, EventArgs e)
        {
            TChart tchart = sender as TChart;
            string a =  tchart.Text;
            int width = flowLayoutPanel1.ClientSize.Width ;
            int height = flowLayoutPanel1.ClientSize.Height ;
            if (tchart.Width == width - 10 && tchart.Height == height - 10)
            {

                foreach (var chart in flowLayoutPanel1.Controls.OfType<TChart>().ToArray())
                {
                    chart.Width = flowLayoutPanel1.ClientSize.Width / Convert.ToInt32(3) - 10;
                    chart.Height = flowLayoutPanel1.ClientSize.Height / Convert.ToInt32(3) - 10;
                    chart.Visible = true;
                }
            }
            else
            {
                foreach (var chart in flowLayoutPanel1.Controls.OfType<TChart>().ToArray())
                {
                    if (chart.Text == tchart.Text)
                    {
                        chart.Width = width - 10;
                        chart.Height = height - 10;
                    }
                    else
                    {
                        chart.Visible = false;
                    }

                }
            }
            
        }

        private double CalculateIncrement(double maxValue)
        {
            double increment = 0.1;
            double range = maxValue - 0;

            while ((range / increment) > 10)
            {
                increment *= 2;
            }

            return increment;
        }

        private Line CreateLine(DataTable dstable,decimal d)
        {
            int i = Convert.ToInt32(d);

            if (d>6)
            {
                i = Convert.ToInt32(d) % 6;
            }
            Line line = new Line();

            line.DataSource = dstable;
            line.YValues.DataMember = "PARAMENT_VALUE";
            line.XValues.DataMember = "END_INTERVAL_TIME";
            line.Legend.Visible = false;
            line.Color = colors[i];

            
            line.Pointer.HorizSize = 1;
            line.Pointer.VertSize = 1;
            //line.ColorEachLine = true;
            //line.Legend.Text = dstable.TableName;
            line.Legend.BorderRound = 10; 
            line.Pointer.Style = PointerStyles.Circle;
            line.Pointer.Visible = true;
            //line.Pointer.Color = Color.OrangeRed;
            //line.Pointer.SizeDouble = 1;
            line.XValues.DateTime = true;
            line.GetSeriesMark += Line_GetSeriesMark;
            void Line_GetSeriesMark(Series series, GetSeriesMarkEventArgs e)
            {
                e.MarkText = "PARAMETER_NAME :" + $"{dstable.Rows[e.ValueIndex]["PARAMENT_NAME"]}" + "\r\n" + "VALUE :" + $"{dstable.Rows[e.ValueIndex]["PARAMENT_VALUE"]}" + "\r\n" + "TIME :" + $"{ dstable.Rows[e.ValueIndex]["END_INTERVAL_TIME"]}";
            }
            return line;
        }

        private void Chart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            if (string.IsNullOrEmpty(cmbCount.Text))
            {
                string errMessage = "Please select SQLId Count";
                TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, TAP.UI.EnumMsgType.WARNING, errMessage);
                return;
            }

            if (valueIndex >= 0 && valueIndex < s.Count)
            {
                // 从数据点中提取信息
                //MessageBox.Show(string.Format("您单击了 \"{0}\" 系列的值 {1}", ((System.Data.DataTable)s.DataSource).TableName, s.YValues[valueIndex]));
                var clickedSeries = s as Steema.TeeChart.Styles.Line;

                if (clickedSeries != null)
                {
                    var dataPoint = clickedSeries[valueIndex];
                    var dataSet = clickedSeries.DataSource as DataTable;

                    if (dataSet != null)
                    {
                        WaitDialogForm wdf = new WaitDialogForm("Tip", "PLEASE WAIT FOR A MOMENT");
                        try
                        {
                            // Access the data set associated with the clicked series here.
                            // ...
                            DateTime maxTime = Convert.ToDateTime(dataSet.Rows[valueIndex]["END_INTERVAL_TIME"]);
                            DateTime minTime = Convert.ToDateTime(dataSet.Rows[valueIndex]["END_INTERVAL_TIME"]);
                            //DateTime minTime = snaplist.Min(x => x.END_INTERVAL_TIME);
                            var snapId = dataSet.Rows[valueIndex]["SNAP_ID"].ToString();
                            string tbNm = dataSet.Rows[valueIndex]["PARAMENT_NAME"].ToString();
                            string instance_num= dataSet.Rows[valueIndex]["INSTANCE_NUMBER"].ToString();
                            DateTime datetime = Convert.ToDateTime(dataSet.Rows[valueIndex]["END_INTERVAL_TIME"]);

                            //string beingtime = dataSet.Rows[valueIndex]["INSTANCE_NUMBER"].ToString();
                            var temp = ParamentRelationDS.Tables[0].AsEnumerable().FirstOrDefault(x => x.Field<string>("CONFIG_ID").ToUpper() == tbNm.ToUpper());
                            if (temp==null)
                            {
                                return;
                            }
                            var result = temp.Field<string>("CONFIG_VALUE");
                            if (result == null)
                            {
                                return;
                            }
                            AwrArgsPack argsSel = new AwrArgsPack();
                            if (groupUnit == "DAY")
                            {
                                argsSel.StartTime = minTime.ToString("yyyy-MM-dd HH:mm:ss");
                                argsSel.EndTime = Convert.ToDateTime(argsSel.StartTime).AddDays(+1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                            {
                                //argsSel.StartTime = minTime.AddMinutes(-10).ToString("yyyy-MM-dd HH:mm:ss");
                                argsSel.StartTime = minTime.ToString("yyyy-MM-dd HH:mm:ss");
                                argsSel.EndTime = minTime.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            argsSel.ParamNamesString = result;
                            argsSel.ParamType = string.Join(",", snapId);
                            argsSel.DBName = args.DBName;
                            argsSel.INSTANCE_NUMBER = instance_num;
                            argsSel.ClustersNumber = Convert.ToInt32(cmbCount.Text);

                            DataSet dsRelation = null;
                            if (cmbGroupUnit.EditValue.ToString() == "DAY")
                            {
                                 dsRelation = bs.ExecuteDataSet("GetWorkLoadLagestDaySql", argsSel.getPack());
                            }
                            else
                            {
                                 dsRelation = bs.ExecuteDataSet("GetWorkLoadLagestSql", argsSel.getPack());
                            }
                            
                            if (dsRelation == null || dsRelation.Tables.Count == 0 || dsRelation.Tables[0].Rows.Count == 0)
                            {
                                return;
                            }
                            //var sqlidList = dsRelation.Tables[0].AsEnumerable().Select(x => x.Field<string>("SQL_ID")).ToArray();
                            //argsSel.WorkloadSqlParm = string.Join(",", sqlidList);
                            //DataSet dsSqlText = bs.ExecuteDataSet("GetSqlTextBySqlID", argsSel.getPack());
                            //if (dsSqlText == null || dsSqlText.Tables.Count == 0 || dsSqlText.Tables[0].Rows.Count == 0)
                            //{
                            //    return;
                            //}
                            //string sqlid = dsSqlText.Tables[0].Rows[0]["SQL_ID"].ToString();
                            //string sqlText = dsSqlText.Tables[0].Rows[0]["SQL_TEXT"].ToString();


                            List<DataSet> listDs = new List<DataSet>();
                            int errorCount = 0;
                            string tempDt = string.Empty;
                            foreach (DataRow row in dsRelation.Tables[0].Rows)
                            {
                                DateTime dtNow = DateTime.Now;
                                AwrArgsPack args = new AwrArgsPack();
                                if (groupUnit == "DAY")
                                {
                                    args.StartTime = dtNow.AddDays(-59).ToString("yyyy-MM-dd HH:mm:ss");
                                    args.PARADEF = "yyyy-MM-dd";
                                    tempDt = args.StartTime;
                                }
                                else
                                {
                                    args.StartTime = dtNow.AddDays(-6).ToString("yyyy-MM-dd HH:mm:ss");
                                    args.PARADEF = "yyyy-MM-dd HH24:mi";
                                    tempDt = args.StartTime;
                                }
                                args.EndTime = dtNow.ToString("yyyy-MM-dd HH:mm:ss");
                                args.DBName = argsSel.DBName;
                                args.ParamNamesString = result;
                                args.ParamType = row["SQL_ID"].ToString();
                                args.INSTANCE_NUMBER = instance_num;
                                DataSet dataSet1 = bs.ExecuteDataSet("GetWorkloadNaerTwoM", args.getPack());
                                if (dataSet1 == null || dataSet1.Tables == null || dataSet1.Tables[0].Rows.Count == 0)
                                {
                                    errorCount++;
                                    continue;
                                }
                                listDs.Add(dataSet1);
                            }
                            FrmWorkLoadTreadShowSqlText frm = null;
                            if (errorCount != dsRelation.Tables[0].Rows.Count)
                            {
                                wdf.Close();
                                frm = new FrmWorkLoadTreadShowSqlText(dsRelation.Tables[0], result, argsSel.DBName, listDs, groupUnit, tempDt , datetime);
                                frm.ShowDialog();
                            }
                            else
                            {
                                string errMessage = "Data is null.";
                                TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, TAP.UI.EnumMsgType.WARNING, errMessage);
                            }

                        }
                        finally
                        {
                            wdf.Close();
                        }
                    }
                }
            }
        }
        private void tChart1_MouseUp(object sender, MouseEventArgs e)
        {

            //if (e.Button == MouseButtons.Left && bfirst)
            //{
            //    _pEnd.X = (float)e.X;
            //    _pEnd.Y = (float)e.Y;
            //    bfirst = false;
            //    if (_pStart != _pEnd)
            //    {
            //        SerachDataPoint(_pStart, _pEnd, sender as TChart);
            //    }
            //}
        }

        private void tChart1_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    _pStart.X = (float)e.X;
            //    _pStart.Y = (float)e.Y;

            //    bfirst = true;
            //}

        }
        //private void SerachDataPoint(PointF pStart, PointF pEnd, TChart chart)
        //{
        //    List<SnapshotDto> snaplist = new List<SnapshotDto>();
        //    float minX;
        //    float minY;
        //    float maxX;
        //    float maxY;
        //    if (pStart.X < pEnd.X)
        //    {
        //        minX = pStart.X;
        //        maxX = pEnd.X;
        //    }
        //    else
        //    {
        //        minX = pEnd.X;
        //        maxX = pStart.X;
        //    }
        //    if (pStart.Y < pEnd.Y)
        //    {
        //        minY = pStart.Y;
        //        maxY = pEnd.Y;
        //    }
        //    else
        //    {
        //        minY = pEnd.Y;
        //        maxY = pStart.Y;
        //    }

        //    foreach (Line line in chart.Chart.Series)
        //    {
        //        for (int i = 0; i < line.Count; i++)
        //        {
        //            if (line.CalcXPos(i) >= minX && line.CalcXPos(i) < maxX && line.CalcYPos(i) >= minY && line.CalcYPos(i) <= maxY)
        //            {
        //                SnapshotDto dto = new SnapshotDto();
        //                //dto.SQL_ID = ((System.Data.DataTable)line.DataSource).TableName; //snap_id
        //                dto.PARAMENT_NAME = ((System.Data.DataTable)line.DataSource).TableName;
        //                //double value = line[i].Y;//VALUE
        //                //dto.Value = line[i].Y.ToString();//value
        //                dto.PARAMENT_VALUE = (decimal)line[i].Y;//value
        //                                                        //int xValue = Convert.ToInt32(line[i].X);//ROWNUM


        //                DataTable dt1 = line.DataSource as DataTable;
        //                dto.SNAP_ID = (decimal)dt1.Rows[i]["SNAP_ID"];//SQL_ID
        //                                                              //dto.DBID = (decimal)dt1.Rows[i]["DBID"];//SQL_ID
        //                dto.END_INTERVAL_TIME = (DateTime)dt1.Rows[i]["END_INTERVAL_TIME"];
        //                snaplist.Add(dto);
        //            }
        //        }
        //    }
        //    if (!snaplist.Any())
        //    {
        //        return;
        //    }
        //    DateTime maxTime = snaplist.Max(x => x.END_INTERVAL_TIME);
        //    DateTime minTime = snaplist.Min(x => x.END_INTERVAL_TIME);
        //    var snapId = snaplist.Select(x => x.SNAP_ID.ToString()).Distinct().ToArray();
        //    string tbNm = snaplist.FirstOrDefault().PARAMENT_NAME;
        //    var result = ParamentRelationDS.Tables[0].AsEnumerable().FirstOrDefault(x => x.Field<string>("CONFIG_ID").ToUpper() == tbNm.ToUpper()).Field<string>("CONFIG_VALUE");
        //    AwrArgsPack argsSel = new AwrArgsPack();
        //    argsSel.StartTime = minTime.ToString("yyyy-MM-dd");
        //    argsSel.EndTime = maxTime.ToString("yyyy-MM-dd");
        //    argsSel.ParamNamesString = result;
        //    argsSel.ParamType = string.Join(",", snapId);
        //    argsSel.DBName = args.DBName;
        //    DataSet dsRelation = bs.ExecuteDataSet("GetWorkLoadLagestSql", argsSel.getPack());
        //    if (dsRelation == null || dsRelation.Tables.Count == 0 || dsRelation.Tables[0].Rows.Count == 0)
        //    {
        //        return;
        //    }
        //    var sqlidList= dsRelation.Tables[0].AsEnumerable().Select(x => x.Field<string>("SQL_ID")).ToArray();
        //    argsSel.WorkloadSqlParm = string.Join(",", sqlidList);
        //    DataSet dsSqlText = bs.ExecuteDataSet("GetSqlTextBySqlID", argsSel.getPack());
        //    if (dsSqlText == null || dsSqlText.Tables.Count == 0 || dsSqlText.Tables[0].Rows.Count == 0)
        //    {
        //        return;
        //    }
        //    //string sqlid = dsSqlText.Tables[0].Rows[0]["SQL_ID"].ToString();
        //    //string sqlText = dsSqlText.Tables[0].Rows[0]["SQL_TEXT"].ToString();
        //    FrmWorkLoadTreadShowSqlText frm = new FrmWorkLoadTreadShowSqlText(dsSqlText.Tables[0]);
        //    frm.ShowDialog();



        //}

        //private void ClickDataPoint(TChart chart)
        //{
        //    //SnapshotDto dto = new SnapshotDto();
        //    ////dto.SQL_ID = ((System.Data.DataTable)line.DataSource).TableName; //snap_id
        //    //dto.PARAMENT_NAME = ((System.Data.DataTable)line.DataSource).TableName;
        //    ////double value = line[i].Y;//VALUE
        //    ////dto.Value = line[i].Y.ToString();//value
        //    //dto.PARAMENT_VALUE = (decimal)line[i].Y;//value
        //    //                                        //int xValue = Convert.ToInt32(line[i].X);//ROWNUM


        //    //DataTable dt1 = line.DataSource as DataTable;
        //    //dto.SNAP_ID = (decimal)dt1.Rows[i]["SNAP_ID"];//SQL_ID
        //    //                                              //dto.DBID = (decimal)dt1.Rows[i]["DBID"];//SQL_ID
        //    //dto.END_INTERVAL_TIME = (DateTime)dt1.Rows[i]["END_INTERVAL_TIME"];
        //}

        private void cmbLinePara_EditValueChanged(object sender, EventArgs e)
        {
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null)
            {
                return;
            }
            CreateTeeChart(dataSet.Tables[0]);
        }

        private void cmbDbName_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbDbName.Text))
            {
                cmbInstance.EditValue = 1;
                cmbInstance.Text = "1";
            }
        }

        private void FrmWorkLoadTrendChartNew_Load(object sender, EventArgs e)
        {
            cmbDbName.Setting();
            cmbDbName.Properties.Items[0].CheckState = CheckState.Checked;
            btnSelect_Click(null,null);
        }
    }

    public class WorkLoadInfo
    {
        public string WORKDATE { get; set; }
        public decimal SNAP_ID_MIN { get; set; }
        public decimal PARAMENT_VALUE { get; set; }
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
        public decimal EXECUTIONS { get; set; }
        public decimal ELAPSED_TIME { get; set; }
        public decimal CPU_TIME { get; set; }
        public decimal BUFFER_GETS { get; set; }
        public decimal DISK_READS { get; set; }
        public decimal PARSE_CALL { get; set; }

        public decimal INSTANCE_NUMBER { get; set; }
        public DateTime  BEGIN_TIME { get; set; }
    }
}
