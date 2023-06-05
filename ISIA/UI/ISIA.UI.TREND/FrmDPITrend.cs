using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;
using ISIA.UI.TREND.Dto;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.UI;

namespace ISIA.UI.TREND
{
    public partial class FrmDPITrend : DockUIBase1T1
    {

        BizDataClient bs;
        DataSet dataSet = new DataSet();
        List<DPIDto> list = new List<DPIDto>();
        List<DPIDto> list2 = new List<DPIDto>();
        List<DPIDto> list3 = new List<DPIDto>();
        List<Color> colors = new List<Color> { Color.FromArgb(74, 126, 187), Color.FromArgb(190, 75, 72), Color.FromArgb(152,185,84),
            Color.FromArgb(125,96,160), Color.FromArgb(70,170,197), Color.FromArgb(218,129,55)  };

        CancellationTokenSource cts = new CancellationTokenSource();

        public FrmDPITrend()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.PerformaceEvaluationTrend");
            dtpStartTime.DateTime = DateTime.Now.AddDays(-1);
            dtpEndTime.DateTime = DateTime.Now;
            Init();
        }


        //public DataSet LoadData()
        //{            

        //    return dataSet;
        //}
        //public void DisplayData(DataSet ds)
        //{
        //    if (dataSet == null)
        //    {
        //        return;
        //    }
        //    //ConvertData(dataSet);

        //    //CreateTeeChart(dataSet.Tables[0]);
        //    dataSet.Tables.Clear();

        //}



        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 设置子控件的宽度和高度
            int width = flowLayoutPanel1.ClientSize.Width / Convert.ToInt32(comboBoxEdit1.EditValue);
            int height = flowLayoutPanel1.ClientSize.Height / Convert.ToInt32(comboBoxEdit1.EditValue);

            foreach (var chart in flowLayoutPanel1.Controls.OfType<TChart>().ToArray())
            {
                chart.Width = width-10;
                chart.Height = height-10;
            }
            foreach (var chart in flowLayoutPanel2.Controls.OfType<TChart>().ToArray())
            {
                chart.Width = width - 10;
                chart.Height = height - 10;
            }
            foreach (var chart in flowLayoutPanel3.Controls.OfType<TChart>().ToArray())
            {
                chart.Width = width - 10;
                chart.Height = height - 10;
            }

        }

        private void FrmMainForm_Load(object sender, EventArgs e)
        {
            //CreateChart();
            //comboBoxEdit1_SelectedIndexChanged(null, null);
            //CreateCharts();
        }

        private void CreateChart()
        {
            if (string.IsNullOrEmpty(cmbInstance.Text))
            {
                return;
            }
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();
            flowLayoutPanel3.Controls.Clear();
            //Instancenumber多选时，每个Instancenumber生成不同chart
            string[] instanceNum = cmbInstance.Text.Split(',');
            //instanceNum = new string[] { "1", "1" };
            foreach (var item in instanceNum)
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    // 创建 TChart 控件
                    TChart tChart = new TChart();
                    tChart.Tag = i;
                    tChart.Width = 300;
                    tChart.Height = 200;
                    // 设置每个 TChart 控件的其他属性或数据

                    // 将 TChart 控件添加到 FlowLayoutPanel 中
                    flowLayoutPanel1.Controls.Add(tChart);
                }
                for (int i = 0; i < list3.Count(); i++)
                {
                    // 创建 TChart 控件
                    TChart tChart = new TChart();
                    tChart.Tag = i;
                    tChart.Width = 300;
                    tChart.Height = 200;
                    // 设置每个 TChart 控件的其他属性或数据

                    // 将 TChart 控件添加到 FlowLayoutPanel 中
                    flowLayoutPanel3.Controls.Add(tChart);
                }
            }
            
            for (int i = 0; i < list2.Count(); i++)
            {
                // 创建 TChart 控件
                TChart tChart = new TChart();
                tChart.Tag = i;
                tChart.Width = 300;
                tChart.Height = 200;
                // 设置每个 TChart 控件的其他属性或数据

                // 将 TChart 控件添加到 FlowLayoutPanel 中
                flowLayoutPanel2.Controls.Add(tChart);
            }
           
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            //try
            //{
            CreateChart();
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

            if (!base.ValidateUserInput(this.lcSerachOptions)) return;

            //base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            cts = new CancellationTokenSource();
            btnSelect.Enabled = false;
            Task.Factory.StartNew(() => QueryDataSheet1(cts.Token), cts.Token);
            Task.Factory.StartNew(() => QueryDataSheet2(cts.Token), cts.Token);
            Task.Factory.StartNew(() => QueryDataSheet3(cts.Token), cts.Token);
            //QueryDataSheet1();
            //QueryDataSheet2();
            //QueryDataSheet3();
            btnSelect.Enabled = true;

           
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }
        public DataSet LoadData()
        {

            return null;
        }
        public void DisplayData(DataSet ds)
        {
        }
        private void QueryDataSheet1(CancellationToken token)
        {
            var charts1 = flowLayoutPanel1.Controls.OfType<TChart>().ToArray();
            string[] instanceNum = cmbInstance.Text.Split(',');
            //instanceNum = new string[] { "1", "1" };
            int count = list.Count();
            int temp = 0;
            //Thread[] threads = new Thread[count];
            for (int i = 0; i < count; i++)
            {
                int chartIndex = i;
                foreach (string item in instanceNum)
                {
                    int tempi = temp;
                    ShowWaitIcon(charts1.ElementAt(tempi));
                    Task.Factory.StartNew(() => QueryDataForTChart1(charts1.ElementAt(tempi), list[chartIndex], item, token), token);

                    temp++;
                    
                }


            }

        }

        private void QueryDataSheet2(CancellationToken token)
        {
            var charts1 = flowLayoutPanel2.Controls.OfType<TChart>().ToArray();
            int count = list2.Count();
            //Thread[] threads = new Thread[count];
            for (int i = 0; i < count; i++)
            {
                int chartIndex = i; 
                ShowWaitIcon(charts1.ElementAt(chartIndex));
                Task.Factory.StartNew(() => QueryDataForTChart2(charts1.ElementAt(chartIndex), list2[chartIndex], token), token);
            }
            //for (int i = 0; i < count; i++)
            //{
            //    int chartIndex = i;
            //    ShowWaitIcon(charts1.ElementAt(chartIndex));
            //    //QueryDataForTChart(charts1.ElementAt(chartIndex), list[chartIndex]);
            //    var thread = (new Thread(() => QueryDataForTChart(charts1.ElementAt(chartIndex), list2[chartIndex])));
            //    //var threadToJoin = threads[chartIndex];
            //    thread.Start();

            //    //threadToJoin.Start();
            //    //threads[chartIndex].Join();
            //    //threadToJoin.Join();
            //}
        }
        private void QueryDataSheet3(CancellationToken token)
        {
            var charts1 = flowLayoutPanel3.Controls.OfType<TChart>().ToArray();
            int count = list3.Count();
            string[] instanceNum = cmbInstance.Text.Split(',');
            //Thread[] threads = new Thread[count];
            for (int i = 0; i < count; i++)
            {
                int chartIndex = i; 
                ShowWaitIcon(charts1.ElementAt(chartIndex));
                foreach (string item in instanceNum)
                {
                    Task.Factory.StartNew(() => QueryDataForTChart1(charts1.ElementAt(chartIndex), list3[chartIndex], item, token));
                }
            }
            //for (int i = 0; i < count; i++)
            //{
            //    int chartIndex = i;
            //    ShowWaitIcon(charts1.ElementAt(chartIndex));
            //    //QueryDataForTChart(charts1.ElementAt(chartIndex), list[chartIndex]);
            //    var thread = (new Thread(() => QueryDataForTChart(charts1.ElementAt(chartIndex), list3[chartIndex])));
            //    //var threadToJoin = threads[chartIndex];
            //    thread.Start();

            //    //threadToJoin.Start();
            //    //threads[chartIndex].Join();
            //    //threadToJoin.Join();
            //}
        }

        private void QueryDataForTChart1(TChart tChart, DPIDto dto,string insNum, CancellationToken token)
        {
            try
            {
                tChart.Invoke((MethodInvoker)delegate
                {
                    tChart.Series.Clear();
                });

                token.ThrowIfCancellationRequested();

                AwrCommonArgsPack args = new AwrCommonArgsPack();
                // 实际的查询逻辑
                args.StartTimeKey = dtpStartTime.DateTime.ToString("yyyy-MM-dd");
                args.EndTimeKey = dtpEndTime.DateTime.ToString("yyyy-MM-dd");
                args.DbName = cmbDbName.Text.Split('(')[0];
                args.DbId = cmbDbName.EditValue.ToString();
                args.InstanceNumber = insNum;
                args.SnapId = "";
                args.ChartName = dto.HeaderText;
                Thread.Sleep(10);
                DataSet dst = bs.ExecuteDataSet(dto.DPIFileName, args.getPack());
                for (int i = 0; i < dto.FileNameList.Count(); i++)
                {
                    token.ThrowIfCancellationRequested();

                    Line line = CreateLine(dst.Tables[0], dto, i);
                    tChart.Invoke((MethodInvoker)delegate
                    {
                        if (dto.YRValueType == 1)
                        {
                            Axis yAxis = tChart.Axes.Right;

                        // 设置Y轴标签的显示格式为百分比（保留两位小数）
                        yAxis.Labels.ValueFormat = "0.00%";
                        }
                        if (dto.YLValueType == 1)
                        {
                            Axis yAxis = tChart.Axes.Left;
                            yAxis.Labels.ValueFormat = "0.00%";
                        }
                        tChart.Axes.Right.Visible = true;
                        tChart.Axes.Left.Visible = true;
                        tChart.Legend.Visible = true;
                        tChart.Legend.LegendStyle = LegendStyles.Series;
                        tChart.Legend.Alignment = Steema.TeeChart.LegendAlignments.Bottom;
                        tChart.Series.Add(line);
                    });
                }
                // 将查询到的数据设置到TChart控件中
                tChart.Invoke((MethodInvoker)delegate
                {
                    HideWaitIcon(tChart, dto.HeaderText + "-" + args.DbName + "-" + insNum);
                });
                token.ThrowIfCancellationRequested();
            }
            catch (OperationCanceledException)
            {
                // Ignore the exception
            }
        }
        private void QueryDataForTChart(TChart tChart,DPIDto dto)
        {
            tChart.Invoke((MethodInvoker)delegate
            {
                tChart.Series.Clear();
            });
            AwrCommonArgsPack args = new AwrCommonArgsPack();
            // 实际的查询逻辑
            args.StartTimeKey = dtpStartTime.DateTime.ToString("yyyy-MM-dd");
            args.EndTimeKey = dtpEndTime.DateTime.ToString("yyyy-MM-dd");
            args.DbName = cmbDbName.Text.Split('(')[0];
            args.DbId = cmbDbName.EditValue.ToString();
            args.InstanceNumber = cmbInstance.EditValue.ToString();
            args.SnapId = "";
            args.ChartName = dto.HeaderText;
            Thread.Sleep(10);
            DataSet dst = bs.ExecuteDataSet(dto.DPIFileName, args.getPack());
            for (int i = 0; i < dto.FileNameList.Count(); i++)
            {
                Line line = CreateLine(dst.Tables[0], dto,i);
                tChart.Invoke((MethodInvoker)delegate
                {
                    if (dto.YRValueType==1)
                    {
                        Axis yAxis = tChart.Axes.Right;

                        // 设置Y轴标签的显示格式为百分比（保留两位小数）
                        yAxis.Labels.ValueFormat = "0.00%";
                    }
                    if (dto.YLValueType==1)
                    {
                        Axis yAxis = tChart.Axes.Left;
                        yAxis.Labels.ValueFormat = "0.00%";
                    }
                    tChart.Axes.Right.Visible = true;
                    tChart.Axes.Left.Visible = true;
                    tChart.Legend.Visible = true;
                    tChart.Legend.LegendStyle = LegendStyles.Series;
                    tChart.Legend.Alignment = Steema.TeeChart.LegendAlignments.Bottom;
                    tChart.Series.Add(line);
                });
            }
            // 将查询到的数据设置到TChart控件中
            tChart.Invoke((MethodInvoker)delegate
            {
                HideWaitIcon(tChart, dto.HeaderText);
            });
        }
        private void QueryDataForTChart2(TChart tChart, DPIDto dto, CancellationToken token)
        {
            try
            {
                tChart.Invoke((MethodInvoker)delegate
                {
                    tChart.Series.Clear();
                });

                token.ThrowIfCancellationRequested();

                AwrCommonArgsPack args = new AwrCommonArgsPack();
                // 实际的查询逻辑
                args.StartTimeKey = dtpStartTime.DateTime.ToString("yyyy-MM-dd");
                args.EndTimeKey = dtpEndTime.DateTime.ToString("yyyy-MM-dd");
                args.DbName = cmbDbName.Text.Split('(')[0];
                args.DbId = cmbDbName.EditValue.ToString();
                args.InstanceNumber = cmbInstance.Text;
                args.SnapId = "";
                args.ChartName = dto.HeaderText;
                Thread.Sleep(10);
                DataSet dst = bs.ExecuteDataSet(dto.DPIFileName, args.getPack());
                dst.Tables[0].TableName = "TABLE";
                IEnumerable<IGrouping<string, DataRow>> res = dst.Tables[0].Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["INSTANCE_NUMBER"].ToString());
                foreach (IGrouping<string, DataRow> data in res)
                {
                    DataTable dataTable = data.ToArray().CopyToDataTable();
                    dataTable.TableName = data.Key;
                    if (dataTable.Rows.Count > 0)
                    {
                        dst.Tables.Add(dataTable);
                    }
                }
                string[] ins = cmbInstance.Text.Split(',').ToArray();

                for (int i = 0; i < ins.Length; i++)
                {
                    token.ThrowIfCancellationRequested();

                    Line line;

                    if (dst.Tables[ins[i].ToString()] == null)
                    {
                        line = CreateLine2(dst.Tables[0], dto);
                        i = ins.Length;
                    }
                    else
                    {
                        line = CreateLine2(dst.Tables[ins[i].ToString()], dto);
                    }



                    tChart.Invoke((MethodInvoker)delegate
                    {
                        if (dto.YRValueType == 1)
                        {
                            Axis yAxis = tChart.Axes.Right;

                        // 设置Y轴标签的显示格式为百分比（保留两位小数）
                        yAxis.Labels.ValueFormat = "0.00%";
                        }
                        if (dto.YLValueType == 1)
                        {
                            Axis yAxis = tChart.Axes.Left;
                            yAxis.Labels.ValueFormat = "0.00%";
                        }
                        tChart.Axes.Right.Visible = true;
                        tChart.Axes.Left.Visible = true;
                        tChart.Legend.Visible = true;
                        tChart.Legend.LegendStyle = LegendStyles.Series;
                        tChart.Legend.Alignment = Steema.TeeChart.LegendAlignments.Bottom;
                        tChart.Series.Add(line);
                    });
                }
                // 将查询到的数据设置到TChart控件中
                tChart.Invoke((MethodInvoker)delegate
                {
                    HideWaitIcon(tChart, dto.HeaderText);
                });
                token.ThrowIfCancellationRequested();
            }
            catch (OperationCanceledException)
            {
                // Ignore the exception
            }
        }
        private void QueryDataForTChart3(TChart tChart, DPIDto dto)
        {
            tChart.Series.Clear();
            AwrCommonArgsPack args = new AwrCommonArgsPack();
            // 实际的查询逻辑
            args.StartTimeKey = dtpStartTime.DateTime.ToString("yyyy-MM-dd");
            args.EndTimeKey = dtpEndTime.DateTime.ToString("yyyy-MM-dd");
            args.DbName = cmbDbName.Text.Split('(')[0];
            args.DbId = cmbDbName.EditValue.ToString();
            args.InstanceNumber = cmbInstance.EditValue.ToString();
            args.SnapId = "";
            args.ChartName = dto.HeaderText;

            DataSet dst = bs.ExecuteDataSet(dto.DPIFileName, args.getPack());
            for (int i = 0; i < dto.FileNameList.Count(); i++)
            {
                Line line = CreateLine(dst.Tables[0], dto, i);
                tChart.Series.Add(line);
            }
            // 将查询到的数据设置到TChart控件中
            HideWaitIcon(tChart, dto.HeaderText + "-" + args.DbName + "-" + args.InstanceNumber);
        }

        private Line CreateLine2(DataTable dstable, DPIDto dto)
        {
            Line line = new Line();

            line.DataSource = dstable;
            //line.XValues.DataMember = dto.Xvalue;
            line.LabelMember = dto.Xvalue;
            line.Legend.Visible = false;
            string str = dto.FileNameList[0].FileNameParament;
            if (dto.FileNameList[0].IsLeftY)
            {
                line.VertAxis = Steema.TeeChart.Styles.VerticalAxis.Left;
            }
            else
            {
                line.VertAxis = Steema.TeeChart.Styles.VerticalAxis.Right;
            }
            line.YValues.DataMember = str;
            line.Color = colors[0];
            line.Legend.Visible = true;
            line.Legend.Text = dto.FileNameList[0].FileNameParament;
            line.Pointer.HorizSize = 1;
            line.Pointer.VertSize = 1;
            line.Legend.BorderRound = 10;
            line.Pointer.Style = PointerStyles.Circle;
            line.Pointer.Visible = true;
            line.XValues.DateTime = true;
            return line;
        }
        private Line CreateLine(DataTable dstable, DPIDto dto, int i)
        {
            Line line = new Line();

            line.DataSource = dstable;
            //line.XValues.DataMember = dto.Xvalue;
            line.LabelMember = dto.Xvalue;
            line.Legend.Visible = false;
            string str = dto.FileNameList[i].FileNameParament;
            if (dto.FileNameList[i].IsLeftY)
            {
                line.VertAxis = Steema.TeeChart.Styles.VerticalAxis.Left;
            }
            else
            {
                line.VertAxis = Steema.TeeChart.Styles.VerticalAxis.Right;
            }
            line.YValues.DataMember = str;
            line.Color = colors[i];
            line.Legend.Visible = true;
            line.Legend.Text = dto.FileNameList[i].FileNameParament;
            line.Pointer.HorizSize = 1;
            line.Pointer.VertSize = 1;
            line.Legend.BorderRound = 10;
            line.Pointer.Style = PointerStyles.Circle;
            line.Pointer.Visible = true;
            line.XValues.DateTime = true;
            return line;
        }


        private void ShowWaitIcon(TChart chart)
        {
            chart.Header.Text = "Loading...";
        }

        private void HideWaitIcon(TChart chart,string s)
        {
            chart.Header.Text = s;
        }

        private void Init()
        {
            //19
             DPIDto dto = new DPIDto
             {
                 DPIFileName = "GetOsstat",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "CP Usage(%)",
                 YLValueType = 1,
                 YRValueType=1,
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "%usr", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "%sys", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "%wio", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "%idle", IsLeftY = false,  }
                 }
             };
             list.Add(dto);
             //18
             dto = new DPIDto
             {
                 DPIFileName = "GetTimeModel",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "DB&Background Times(s)",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "DB time", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "DB CPU", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "background", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "background cpu", IsLeftY = true  }
                 }
             };
             list.Add(dto);
             //1
             dto = new DPIDto
             {
                 DPIFileName = "GetLoadSQL",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "Lgical/Physical Reads",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "Logical reads", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "Physical reads", IsLeftY = false },
                 }
             };
             list.Add(dto);
             //1
             dto = new DPIDto
             {
                 DPIFileName = "GetLoadSQL",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "User Call/Execute Count",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "User calls", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "Executes", IsLeftY = false },
                 }
             };
             list.Add(dto);
             //62
             dto = new DPIDto
             {
                 DPIFileName = "GetResource",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "Session/Active Count",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "sessions_curr", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "sessions_max", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "active_session_cnt", IsLeftY = false },
                 }
             };
             list.Add(dto);
             //12
             dto = new DPIDto
             {
                 DPIFileName = "GetWait5_01",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "Top 5 Wait Events(time(s))",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "time_1", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "time_2", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "time_3", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "time_4", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "time_5", IsLeftY = true },
                 }
             };
             list.Add(dto);
             //68--无法查询
             dto = new DPIDto
             {
                 DPIFileName = "GetLatch_01",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "Top wait time latch:Avg. wait time(ms)",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "rank1_wait_t", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank2_wait_t", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank3_wait_t", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank4_wait_t", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank5_wait_t", IsLeftY = true },
                 }
             };
             list.Add(dto);
             //66
             dto = new DPIDto
             {
                 DPIFileName = "GetEnq_01",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "Avg.Top-5 En queue Wait Time(s)",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "rank1_wait_tm", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank2_wait_tm", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank3_wait_tm", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank4_wait_tm", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank5_wait_tm", IsLeftY = true },
                 }
             };
             list.Add(dto);
             //61 --无法查询
             dto = new DPIDto
             {
                 DPIFileName = "GetWaitstat",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "Wait count by class",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "DataBlock", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "SegmentHeader", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "UndoBlock", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "UndoHeader", IsLeftY = true },
                 }
             };
             list.Add(dto);
             //64 --无法查询
             dto = new DPIDto
             {
                 DPIFileName = "GetBuffer_pool",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "Cache Hit%",
                 YLValueType = 1,
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "free buffer wait", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "%Hit", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "buffer busy wait", IsLeftY = true },
                 }
             };
             list.Add(dto);
             //74
             dto = new DPIDto
             {
                 DPIFileName = "GetSession_cache",
                 Xvalue = "TIMESTAMP",
                 YRValueType=1,
                 HeaderText = "Session Cached cursor Statistics",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "Parse requests", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "Cursor cache hits", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "ReParsed requests", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "Cursor cache hit%", IsLeftY = true },
                 }
             };
             list.Add(dto);
             //71--无法查询
             dto = new DPIDto
             {
                 DPIFileName = "GetSga",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "SGA",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "buf.cache(M)", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "shared.pool(M)", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "java.pool(M)", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "large.pool(M)", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "streams.pool(M)", IsLeftY = true },
                 }
             };
             list.Add(dto);
             //71--无法查询
             dto = new DPIDto
             {
                 DPIFileName = "GetSga",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "shared pool",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "sqlarea(M)", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "lib.cache(M)", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "others(M)", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "free(M)", IsLeftY = true },
                 }
             };
             list.Add(dto);
             //54--无法查询
             dto = new DPIDto
             {
                 DPIFileName = "GetPga",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "PGA Statistics",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "sqlarea(M)", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "lib.cache(M)", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "others(M)", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "free(M)", IsLeftY = false },
                 }
             };
             list.Add(dto);

             //55--无法查询
             dto = new DPIDto
             {
                 DPIFileName = "GetWorkarea_raw",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "PGA Memory/Disk sort",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "one_exe", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "mul_exe", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "tot_exe", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "opt_exe", IsLeftY = true },
                 }
             };
             list.Add(dto);
             //03--无法查询
             dto = new DPIDto
             {
                 DPIFileName = "GetRacGcEfficiency",
                 Xvalue = "TIMESTAMP",
                 YLValueType=1,
                 HeaderText = "Buffer Access{local/remote/disk}%",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "Buffer access - local cache%", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "Buffer access - remote cache%", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "Buffer access - disk%", IsLeftY = true },
                 }
             };
             list.Add(dto);
            //--sheet2
            //01
            DPIDto dto1 = new DPIDto
            {
                DPIFileName = "GetLoadSQL",
                Xvalue = "TIMESTAMP",
                HeaderText = "transaction(per sec)",
                FileNameList = new List<DPIAboutY> {
                    new DPIAboutY { FileNameParament = "Transactions", IsLeftY = true },
                }
            };
            list2.Add(dto1);
            //01
            dto1 = new DPIDto
            {
                DPIFileName = "GetLoadSQL",
                Xvalue = "TIMESTAMP",
                HeaderText = "Redo Size(Bytes)",
                FileNameList = new List<DPIAboutY> {
                    new DPIAboutY { FileNameParament = "Redo size", IsLeftY = true },
                }
            };
            list2.Add(dto1);
            //01
            dto1 = new DPIDto
            {
                DPIFileName = "GetLoadSQL",
                Xvalue = "TIMESTAMP",
                HeaderText = "Parse Count(Per Sec)",
                FileNameList = new List<DPIAboutY> {
                    new DPIAboutY { FileNameParament = "Parses(total)", IsLeftY = true },
                }
            };
            list2.Add(dto1);
            //01
            dto1 = new DPIDto
            {
                DPIFileName = "GetLoadSQL",
                Xvalue = "TIMESTAMP",
                HeaderText = "Hard Parse Count(per sec)",
                FileNameList = new List<DPIAboutY> {
                    new DPIAboutY { FileNameParament = "Parses(hard)", IsLeftY = true },
                }
            };
            list2.Add(dto1);
            //18
            dto1 = new DPIDto
            {
                DPIFileName = "GetTimeModel",
                Xvalue = "TIMESTAMP",
                HeaderText = "DB Time(s)",
                FileNameList = new List<DPIAboutY> {
                    new DPIAboutY { FileNameParament = "DB time", IsLeftY = true },
                }
            };
            list2.Add(dto1);
            //72
            /*dto1 = new DPIDto
            {
                DPIFileName = "GetLog_switch_f",
                Xvalue = "TIMESTAMP",
                HeaderText = "Log Switches",
                FileNameList = new List<DPIAboutY> {
                    new DPIAboutY { FileNameParament = "Log Switch interval <= 5 min", IsLeftY = true },
                }
            };
            list2.Add(dto1);*/

            //54
            dto1 = new DPIDto
            {
                DPIFileName = "GetPga_hit",
                Xvalue = "TIMESTAMP",
                HeaderText = "PGA Cache Hit(%)",
                YLValueType = 1,
                FileNameList = new List<DPIAboutY> {
                    new DPIAboutY { FileNameParament = "PGA Cache Hit%", IsLeftY = true },
                }
            };
            list2.Add(dto1);

            //04
            dto1 = new DPIDto
            {
                DPIFileName = "GetRacChar",
                Xvalue = "TIMESTAMP",
                HeaderText = "Avg Global Cache cr block receive time(ms)",
                FileNameList = new List<DPIAboutY> {
                    new DPIAboutY { FileNameParament = "Avg GC cr blk rcv time(ms)", IsLeftY = true },
                }
            };
            list2.Add(dto1);

            //04
            dto1 = new DPIDto
            {
                DPIFileName = "GetRacChar",
                Xvalue = "TIMESTAMP",
                HeaderText = "Avg Global Cache current block receive time(ms)",
                FileNameList = new List<DPIAboutY> {
                    new DPIAboutY { FileNameParament = "Avg GC cur blk rcv time(ms)", IsLeftY = true },
                }
            };
            list2.Add(dto1);

            //02
            dto1 = new DPIDto
            {
                DPIFileName = "GetRacLoadSql",
                Xvalue = "TIMESTAMP",
                HeaderText = "Estimated interconnect traffic(mbytes/s)",
                FileNameList = new List<DPIAboutY> {
                    new DPIAboutY { FileNameParament = "Interconnect traffic(Mb)", IsLeftY = true },
                }
            };
            list2.Add(dto1);

            //02
            dto1 = new DPIDto
            {
                DPIFileName = "GetRacLoadSql",
                Xvalue = "TIMESTAMP",
                HeaderText = "global cache blocks lost",
                FileNameList = new List<DPIAboutY> {
                    new DPIAboutY { FileNameParament = "gc lost", IsLeftY = true },
                }
            };
            list2.Add(dto1);

            //sheet3
            //29
             DPIDto dto3 = new DPIDto
             {
                 DPIFileName = "GetSqlElap_01",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "Top Elapsed Time SQL:Elapsed Time per exec",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "rank1_elap_per_exec", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank2_elap_per_exec", IsLeftY = true },
                 }
             };
             list3.Add(dto3);
             //29
             dto3 = new DPIDto
             {
                 DPIFileName = "GetSqlElap_01",
                 Xvalue = "TIMESTAMP",
                 YLValueType=1,
                 HeaderText = "Top Elapsed Time SQL:elaped time per DBTime(%)",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "rank1_elap_per_dbtim", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank2_elap_per_dbtim", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank3_elap_per_dbtim", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank4_elap_per_dbtim", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank5_elap_per_dbtim", IsLeftY = true },
                 }
             };
             list3.Add(dto3);
             //31
             dto3 = new DPIDto
             {
                 DPIFileName = "GetSql_cpu_01",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "Top CPU Time SQL:cpu Time per exec",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "rank1_cput_per_exec", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank2_cput_per_exec", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank3_cput_per_exec", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank4_cput_per_exec", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank5_cput_per_exec", IsLeftY = true },
                 }
             };
             list3.Add(dto3);
             //31
             dto3 = new DPIDto
             {
                 DPIFileName = "GetSql_cpu_01",
                 Xvalue = "TIMESTAMP",
                 YLValueType=1,
                 HeaderText = "Top CPU Time SQL:cpu time per DBTime(%)",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "RANK1_ELAP_PER_DBTIM", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank2_elap_per_dbtim", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank3_elap_per_dbtim", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank4_elap_per_dbtim", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank5_elap_per_dbtim", IsLeftY = true },
                 }
             };
             list3.Add(dto3);

             //33
             dto3 = new DPIDto
             {
                 DPIFileName = "GetSql_get_01",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "Top Buffer Get SQL:buffer gets per exec",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "rank1_bufget_per_exec", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank2_bufget_per_exec", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank3_bufget_per_exec", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank4_bufget_per_exec", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank5_bufget_per_exec", IsLeftY = true },
                 }
             };
             list3.Add(dto3);

             //33
             dto3 = new DPIDto
             {
                 DPIFileName = "GetSql_get_01",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "Top Buffer Get SQL",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "rank1_bufget", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank2_bufget", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank3_bufget", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank4_bufget", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank5_bufget", IsLeftY = true },
                 }
             };
             list3.Add(dto3);

             //33
             dto3 = new DPIDto
             {
                 DPIFileName = "GetSql_get_01",
                 Xvalue = "TIMESTAMP",
                 YLValueType=1,
                 HeaderText = "Top Buffer Get SQL:buffer gets per Total(%)",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "rank1_bufget_per_tot", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank2_bufget_per_tot", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank3_bufget_per_tot", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank4_bufget_per_tot", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank5_bufget_per_tot", IsLeftY = true },
                 }
             };
             list3.Add(dto3);
             //35
             dto3 = new DPIDto
             {
                 DPIFileName = "GetSql_read_01",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "Top Disk Read SQL:disk read per exec",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "rank1_phyrds_per_exec", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank2_phyrds_per_exec", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank3_phyrds_per_exec", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank4_phyrds_per_exec", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank5_phyrds_per_exec", IsLeftY = true },
                 }
             };
             list3.Add(dto3);
             //35
             dto3 = new DPIDto
             {
                 DPIFileName = "GetSql_read_01",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "Top Disk Read SQL",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "rank1_phyrds", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank2_phyrds", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank3_phyrds", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank4_phyrds", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank5_phyrds", IsLeftY = true },
                 }
             };
             list3.Add(dto3);
             //35
             dto3 = new DPIDto
             {
                 DPIFileName = "GetSql_read_01",
                 Xvalue = "TIMESTAMP",
                 YLValueType=1,
                 HeaderText = "Top Disk Read Sql:disk read per Total(%)",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "rank1_phyrds_per_tot", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank2_phyrds_per_tot", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank3_phyrds_per_tot", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank4_phyrds_per_tot", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank5_phyrds_per_tot", IsLeftY = true },
                 }
             };
             list3.Add(dto3);
             //39
             dto3 = new DPIDto
             {
                 DPIFileName = "GetSql_parse_01",
                 Xvalue = "TIMESTAMP",
                 YLValueType = 1,
                 HeaderText = "Top Parse SQL:parse count per Total(%)",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "rank1_parse_per_tot", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank2_parse_per_tot", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank3_parse_per_tot", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank4_parse_per_tot", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank5_parse_per_tot", IsLeftY = true },
                 }
             };
             list3.Add(dto3);
             //41
             dto3 = new DPIDto
             {
                 DPIFileName = "GetSql_clu_wait_01",
                 Xvalue = "TIMESTAMP",
                 YLValueType=1,
                 HeaderText = "Top Cluster Wait SQL:cluster wait time per elaped time(%)",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "rank1_cput", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank2_cput", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank3_cput", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank4_cput", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank5_cput", IsLeftY = true },
                 }
             };
             list3.Add(dto3);
             //80
             dto3 = new DPIDto
             {
                 DPIFileName = "GetSQL_literal_get_01",
                 Xvalue = "TIMESTAMP",
                 HeaderText = "Top Literal SQL: buffer gets",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "rank1_bufget", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank2_bufget", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank3_bufget", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank4_bufget", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank5_bufget", IsLeftY = true },
                 }
             };
             list3.Add(dto3);
             //80
             dto3 = new DPIDto
             {
                 DPIFileName = "GetSQL_literal_get_01",
                 Xvalue = "TIMESTAMP",
                 YLValueType=1,
                 HeaderText = "Top Literal SQL: buffer gets per Total(%)",
                 FileNameList = new List<DPIAboutY> {
                     new DPIAboutY { FileNameParament = "rank1_bufget_per_tot", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank2_bufget_per_tot", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank3_bufget_per_tot", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank4_bufget_per_tot", IsLeftY = true },
                     new DPIAboutY { FileNameParament = "rank5_bufget_per_tot", IsLeftY = true },
                 }
             };
             list3.Add(dto3);
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            //if(xtraTabControl1.SelectedTabPageIndex==1)
            //{
            //    Task.Factory.StartNew(() => QueryDataSheet2());
            //}

            //if (xtraTabControl1.SelectedTabPageIndex == 2)
            //{
            //    Task.Factory.StartNew(() => QueryDataSheet3());
            //}
        }

        private void xtraTabControl1_CustomHeaderButtonClick(object sender, DevExpress.XtraTab.ViewInfo.CustomHeaderButtonEventArgs e)
        {
            cts.Cancel();
            var charts1 = GetTchartList(flowLayoutPanel1);
            SetTchartList(charts1);
            var charts2 = GetTchartList(flowLayoutPanel2);
            SetTchartList(charts2);
            var charts3 = GetTchartList(flowLayoutPanel3);
            SetTchartList(charts3);
        }

        private void FrmDPITrend_FormClosed(object sender, FormClosedEventArgs e)
        {
            cts.Cancel();
        }

        private TChart[] GetTchartList(FlowLayoutPanel flp)
        {
            return flp.Controls.OfType<TChart>().ToArray();
        }
        private void SetTchartList(TChart[] charts)
        {
            foreach (var item in charts)
            {
                item.Header.Text = "Cessation.";
            }
        }
    }
}
