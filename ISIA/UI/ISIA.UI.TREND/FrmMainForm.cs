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
    public partial class FrmMainForm : DockUIBase1T1
    {

        BizDataClient bs;
        DataSet dataSet = new DataSet();
        List<DPIDto> list = new List<DPIDto>();
        List<DPIDto> list2 = new List<DPIDto>();
        List<DPIDto> list3 = new List<DPIDto>();
        List<Color> colors = new List<Color> { Color.FromArgb(74, 126, 187), Color.FromArgb(190, 75, 72), Color.FromArgb(152,185,84),
            Color.FromArgb(125,96,160), Color.FromArgb(70,170,197), Color.FromArgb(218,129,55)  };

        public FrmMainForm()
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
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();
            flowLayoutPanel3.Controls.Clear();
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

        private async void btnSelect_Click(object sender, EventArgs e)
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

            btnSelect.Enabled = false;

            QueryDataSheet1();
            QueryDataSheet2();
            btnSelect.Enabled = true;
            btnSelect.Enabled = false;

            // 创建一组任务
            //Task[] tasks = new Task[3];
            //tasks[0] = Task.Run(() => QueryDataSheet1());
            //tasks[1] = Task.Run(() => QueryDataSheet2());
            //tasks[2] = Task.Run(() => QueryDataSheet3());

            //// 等待所有任务完成
            //await Task.WhenAll(tasks);

            ConcurrentBag<int> bag = new ConcurrentBag<int>();

            // 添加元素到ConcurrentBag中
            //for (int i = 0; i < 3; i++)
            //{
            //    bag.Add(i);
            //}

            //// 创建10个任务来消费ConcurrentBag中的元素
            //Task[] tasks = new Task[3];

            //tasks[0] = Task.Run(() =>
            //QueryDataSheet1());
            //tasks[1] = Task.Run(() =>
            //QueryDataSheet2());
            //tasks[2] = Task.Run(() =>
            //QueryDataSheet3());

            //await Task.WhenAll(tasks); // 等待所有任务完成

            //QueryDataSheet1();

            //QueryDataSheet2();
            //QueryDataSheet3();
            //Thread[] threadArray = new Thread[3];
            //for (int i = 0; i < 1; i++)
            //{
            //    threadArray[i] = new Thread(() => QueryDataSheet(i));
            //    threadArray[i].Start();
            //}

            // Wait for all threads to complete
            //for (int i = 0; i < 3; i++)
            //{
            //    threadArray[i].Join();
            //}
            //Thread td = new Thread(QueryDataSheet1);
            //td.Start();
            //Thread td2 = new Thread(QueryDataSheet2);
            //td2.Start();
            //Thread td3 = new Thread(QueryDataSheet3);
            //td3.Start();
            // 启用查询按钮
            btnSelect.Enabled = true;

                //测试
                //var charts1 = flowLayoutPanel1.Controls.OfType<TChart>().ToArray();
                //for (int i = 0; i < list.Count(); i++)
                //{
                //    ShowWaitIcon(charts1.ElementAt(i));
                //    int chartIndex = i;
                //    QueryDataForTChart(charts1.ElementAt(chartIndex), list[chartIndex]);
                //}

                //var charts1 = flowLayoutPanel2.Controls.OfType<TChart>().ToArray();
                //for (int i = 0; i < list2.Count(); i++)
                //{
                //    ShowWaitIcon(charts1.ElementAt(i));
                //    int chartIndex = i;
                //    QueryDataForTChart(charts1.ElementAt(chartIndex), list2[chartIndex]);
                //}

                //var charts1 = flowLayoutPanel3.Controls.OfType<TChart>().ToArray();
                //for (int i = 0; i < list3.Count(); i++)
                //{
                //    ShowWaitIcon(charts1.ElementAt(i));
                //    int chartIndex = i;
                //    QueryDataForTChart(charts1.ElementAt(chartIndex), list3[chartIndex]);
                //}
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
        private void QueryDataSheet1()
        {
            var charts1 = flowLayoutPanel1.Controls.OfType<TChart>().ToArray();
            int count = list.Count();
            Thread[] threads = new Thread[count];
            for (int i = 0; i < count; i++)
            {
                int chartIndex = i;
                ShowWaitIcon(charts1.ElementAt(chartIndex));
                //QueryDataForTChart(charts1.ElementAt(chartIndex), list[chartIndex]);
                threads[chartIndex] = (new Thread(() => QueryDataForTChart(charts1.ElementAt(chartIndex), list[chartIndex])));
                var threadToJoin = threads[chartIndex];
                threadToJoin.Start();
                //threads[chartIndex].Join();
                threadToJoin.Join();
            }
        }

        private void QueryDataSheet2()
        {
            var charts1 = flowLayoutPanel2.Controls.OfType<TChart>().ToArray();
            int count = list2.Count();
            Thread[] threads = new Thread[count];
            for (int i = 0; i < list2.Count(); i++)
            {
                int chartIndex = i;
                ShowWaitIcon(charts1.ElementAt(chartIndex));
                QueryDataForTChart(charts1.ElementAt(chartIndex), list2[chartIndex]);
                threads[chartIndex] = (new Thread(() => QueryDataForTChart(charts1.ElementAt(chartIndex), list2[chartIndex])));
                threads[chartIndex].Start();
                threads[chartIndex].Join();
            }
        }
        private void QueryDataSheet3()
        {
            var charts1 = flowLayoutPanel3.Controls.OfType<TChart>().ToArray();
            int count = list3.Count();
            Thread[] threads = new Thread[count];
            for (int i = 0; i < list3.Count(); i++)
            {
                int chartIndex = i;
                ShowWaitIcon(charts1.ElementAt(chartIndex));
                QueryDataForTChart(charts1.ElementAt(chartIndex), list3[chartIndex]);
                //threads[chartIndex] = (new Thread(() => QueryDataForTChart(charts1.ElementAt(chartIndex), list3[chartIndex])));
                //threads[chartIndex].Start();
                //threads[chartIndex].Join();
            }
        }


        private void QueryDataForTChart(TChart tChart,DPIDto dto)
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
            
            DataSet dst = bs.ExecuteDataSet(dto.DPIFileName, args.getPack());
            for (int i = 0; i < dto.FileNameParament.Count(); i++)
            {
                Line line = CreateLine(dst.Tables[0], dto,i);
                tChart.Series.Add(line);
            }
            // 将查询到的数据设置到TChart控件中
            HideWaitIcon(tChart, dto.HeaderText);
        }
        private void QueryDataForTChart2(TChart tChart, DPIDto dto)
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

            DataSet dst = bs.ExecuteDataSet(dto.DPIFileName, args.getPack());
            for (int i = 0; i < dto.FileNameParament.Count(); i++)
            {
                Line line = CreateLine(dst.Tables[0], dto, i);
                tChart.Series.Add(line);
            }
            // 将查询到的数据设置到TChart控件中
            HideWaitIcon(tChart, dto.HeaderText);
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

            DataSet dst = bs.ExecuteDataSet(dto.DPIFileName, args.getPack());
            for (int i = 0; i < dto.FileNameParament.Count(); i++)
            {
                Line line = CreateLine(dst.Tables[0], dto, i);
                tChart.Series.Add(line);
            }
            // 将查询到的数据设置到TChart控件中
            HideWaitIcon(tChart, dto.HeaderText);
        }
        private Line CreateLine(DataTable dstable, DPIDto dto, int i)
        {
            Line line = new Line();

            line.DataSource = dstable;
            //line.XValues.DataMember = dto.Xvalue;
            line.LabelMember = dto.Xvalue;
            line.Legend.Visible = false;
            string str = dto.FileNameParament[i].ToString();
            line.YValues.DataMember = str;
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
                FileNameParament = new List<string> { "%usr", "%sys", "%wio", "%idle" },
            };
            list.Add(dto);
            //18
            dto = new DPIDto
            {
                DPIFileName = "GetTimeModel",
                Xvalue = "TIMESTAMP",
                HeaderText = "DB&Background Times(s)",
                FileNameParament = new List<string> { "DB time", "DB CPU", "background", "background cpu" }
            };
            list.Add(dto);
            //1
            dto = new DPIDto
            {
                DPIFileName = "GetLoadSQL",
                Xvalue = "TIMESTAMP",
                HeaderText = "Lgical/Physical Reads",
                FileNameParament = new List<string> { "Logical reads", "Physical reads" }
            };
            list.Add(dto);
            //1
            dto = new DPIDto
            {
                DPIFileName = "GetLoadSQL",
                Xvalue = "TIMESTAMP",
                HeaderText = "User Call/Execute Count",
                FileNameParament = new List<string> { "User calls", "Executes" }
            };
            list.Add(dto);
            //62
            dto = new DPIDto
            {
                DPIFileName = "GetResource",
                Xvalue = "TIMESTAMP",
                HeaderText = "Session/Active Count",
                FileNameParament = new List<string> { "sessions_curr", "sessions_max", "active_session_cnt" }
            };
            list.Add(dto);
            //12
            dto = new DPIDto
            {
                DPIFileName = "GetWait5_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Top 5 Wait Events(time(s))",
                FileNameParament = new List<string> { "time_1", "time_2", "time_3", "time_4", "time_5" }
            };
            list.Add(dto);
            //68--无法查询
            dto = new DPIDto
            {
                DPIFileName = "GetLatch_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Top wait time latch:Avg. wait time(ms)",
                FileNameParament = new List<string> { "rank1WaitT", "rank2WaitT", "rank3WaitT", "rank4WaitT", "rank5WaitT" }
            };
            list.Add(dto);
            //66
            dto = new DPIDto
            {
                DPIFileName = "GetEnq_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Avg.Top-5 En queue Wait Time(s)",
                FileNameParament = new List<string> { "rank1_wait_tm", "rank2_wait_tm", "rank3_wait_tm", "rank4_wait_tm", "rank5_wait_tm" }
            };
            list.Add(dto);
            //61 --无法查询
            dto = new DPIDto
            {
                DPIFileName = "GetWaitstat",
                Xvalue = "TIMESTAMP",
                HeaderText = "Wait count by class",
                FileNameParament = new List<string> { "DataBlock", "SegmentHeader", "UndoBlock", "UndoHeader" }
            };
            list.Add(dto);
            //64 --无法查询
            dto = new DPIDto
            {
                DPIFileName = "GetBuffer_pool",
                Xvalue = "TIMESTAMP",
                HeaderText = "Cache Hit%",
                FileNameParament = new List<string> { "free buffer wait", "%Hit", "buffer busy wait" }
            };
            list.Add(dto);
            //74
            dto = new DPIDto
            {
                DPIFileName = "GetSession_cache",
                Xvalue = "TIMESTAMP",
                HeaderText = "Session Cached cursor Statistics",
                FileNameParament = new List<string> { "Parse requests", "Cursor cache hits", "ReParsed requests", "Cursor cache hit%" }
            };
            list.Add(dto);
            //71--无法查询
            dto = new DPIDto
            {
                DPIFileName = "GetSga",
                Xvalue = "TIMESTAMP",
                HeaderText = "SGA",
                FileNameParament = new List<string> { "buf.cache(M)", "shared.pool(M)", "java.pool(M)", "large.pool(M)", "streams.pool(M)" }
            };
            list.Add(dto);
            //71--无法查询
            dto = new DPIDto
            {
                DPIFileName = "GetSga",
                Xvalue = "TIMESTAMP",
                HeaderText = "shared pool",
                FileNameParament = new List<string> { "sqlarea(M)", "lib.cache(M)", "others(M)", "free(M)" }
            };
            list.Add(dto);
            //54--无法查询
            dto = new DPIDto
            {
                DPIFileName = "GetPga",
                Xvalue = "TIMESTAMP",
                HeaderText = "PGA Statistics",
                FileNameParament = new List<string> { "sqlarea(M)", "lib.cache(M)", "others(M)", "free(M)" }
            };
            list.Add(dto);

            //55--无法查询
            dto = new DPIDto
            {
                DPIFileName = "GetWorkarea_raw",
                Xvalue = "TIMESTAMP",
                HeaderText = "PGA Memory/Disk sort",
                FileNameParament = new List<string> { "one_exe", "mul_exe", "tot_exe", "opt_exe" }
            };
            list.Add(dto);
            //03--无法查询
            dto = new DPIDto
            {
                DPIFileName = "GetRacGcEfficiency",
                Xvalue = "TIMESTAMP",
                HeaderText = "Buffer Access{local/remote/disk}%",
                FileNameParament = new List<string> { "Buffer access - local cache%", "Buffer access - remote cache%", "Buffer access - disk%" }
            };
            list.Add(dto);
            //--sheet2
            //01
            DPIDto dto1 = new DPIDto
            {
                DPIFileName = "GetLoadSQL",
                Xvalue = "TIMESTAMP",
                HeaderText = "transaction(per sec)",
                FileNameParament = new List<string> { "Transactions" }
            };
            list2.Add(dto1);
            //01
            dto1 = new DPIDto
            {
                DPIFileName = "GetLoadSQL",
                Xvalue = "TIMESTAMP",
                HeaderText = "Redo Size(Bytes)",
                FileNameParament = new List<string> { "Redo size" }
            };
            list2.Add(dto1);
            //01
            dto1 = new DPIDto
            {
                DPIFileName = "GetLoadSQL",
                Xvalue = "TIMESTAMP",
                HeaderText = "Parse Count(Per Sec)",
                FileNameParament = new List<string> { "Parses(total)" }
            };
            list2.Add(dto1);
            //01
            dto1 = new DPIDto
            {
                DPIFileName = "GetLoadSQL",
                Xvalue = "TIMESTAMP",
                HeaderText = "Hard Parse Count(per sec)",
                FileNameParament = new List<string> { "Parses(hard)" }
            };
            list2.Add(dto1);
            //18
            dto1 = new DPIDto
            {
                DPIFileName = "GetTimeModel",
                Xvalue = "TIMESTAMP",
                HeaderText = "DB Time(s)",
                FileNameParament = new List<string> { "DB time" }
            };
            list2.Add(dto1);
            //72
            dto1 = new DPIDto
            {
                DPIFileName = "GetLog_switch_f",
                Xvalue = "TIMESTAMP",
                HeaderText = "Log Switches",
                FileNameParament = new List<string> { "Log Switch interval <= 5 min" }
            };
            list2.Add(dto1);

            //54
            dto1 = new DPIDto
            {
                DPIFileName = "GetPga_hit",
                Xvalue = "TIMESTAMP",
                HeaderText = "PGA Cache Hit(%)",
                FileNameParament = new List<string> { "PGA Cache Hit%" }
            };
            list2.Add(dto1);

            //04
            dto1 = new DPIDto
            {
                DPIFileName = "GetRacChar",
                Xvalue = "TIMESTAMP",
                HeaderText = "Avg Global Cache cr block receive time(ms)",
                FileNameParament = new List<string> { "Avg GC cr blk rcv time(ms)" }
            };
            list2.Add(dto1);

            //04
            dto1 = new DPIDto
            {
                DPIFileName = "GetRacChar",
                Xvalue = "TIMESTAMP",
                HeaderText = "Avg Global Cache current block receive time(ms)",
                FileNameParament = new List<string> { "Avg GC cur blk rcv time(ms)" }
            };
            list2.Add(dto1);

            //02
            dto1 = new DPIDto
            {
                DPIFileName = "GetRacLoadSql",
                Xvalue = "TIMESTAMP",
                HeaderText = "Estimated interconnect traffic(mbytes/s)",
                FileNameParament = new List<string> { "Interconnect traffic(Mb)" }
            };
            list2.Add(dto1);

            //02
            dto1 = new DPIDto
            {
                DPIFileName = "GetRacLoadSql",
                Xvalue = "TIMESTAMP",
                HeaderText = "global cache blocks lost",
                FileNameParament = new List<string> { "gc lost" }
            };
            list2.Add(dto1);

            //sheet3
            //29
            DPIDto dto3 = new DPIDto
            {
                DPIFileName = "GetSqlElap_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Top Elapsed Time SQL:Elapsed Time per exec",
                FileNameParament = new List<string> { "rank1_elap_per_exec", "rank2_elap_per_exec" }
            };
            list3.Add(dto3);
            //29
            dto3 = new DPIDto
            {
                DPIFileName = "GetSqlElap_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Top Elapsed Time SQL:elaped time per DBTime(%)",
                FileNameParament = new List<string> { "rank1_elap_per_dbtim", "rank2_elap_per_dbtim", "rank3_elap_per_dbtim", "rank4_elap_per_dbtim", "rank5_elap_per_dbtim" }
            };
            list3.Add(dto3);
            //31
            dto3 = new DPIDto
            {
                DPIFileName = "GetSql_cpu_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Top CPU Time SQL:cpu Time per exec",
                FileNameParament = new List<string> { "rank1_cput_per_exec", "rank2_cput_per_exec", "rank3_cput_per_exec", "rank4_cput_per_exec", "rank5_cput_per_exec" }
            };
            list3.Add(dto3);
            //31
            dto3 = new DPIDto
            {
                DPIFileName = "GetSql_cpu_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Top CPU Time SQL:cpu time per DBTime(%)",
                FileNameParament = new List<string> { "RANK1_ELAP_PER_DBTIM", "rank2_elap_per_dbtim", "rank3_elap_per_dbtim", "rank4_elap_per_dbtim", "rank5_elap_per_dbtim" }
            };
            list3.Add(dto3);

            //33
            dto3 = new DPIDto
            {
                DPIFileName = "GetSql_get_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Top Buffer Get SQL:buffer gets per exec",
                FileNameParament = new List<string> { "rank1_bufget_per_exec", "rank2_bufget_per_exec", "rank3_bufget_per_exec", "rank4_bufget_per_exec", "rank5_bufget_per_exec" }
            };
            list3.Add(dto3);

            //33
            dto3 = new DPIDto
            {
                DPIFileName = "GetSql_get_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Top Buffer Get SQL",
                FileNameParament = new List<string> { "rank1_bufget", "rank2_bufget", "rank3_bufget", "rank4_bufget", "rank5_bufget" }
            };
            list3.Add(dto3);

            //33
            dto3 = new DPIDto
            {
                DPIFileName = "GetSql_get_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Top Buffer Get SQL:buffer gets per Total(%)",
                FileNameParament = new List<string> { "rank1_bufget_per_tot", "rank2_bufget_per_tot", "rank3_bufget_per_tot", "rank4_bufget_per_tot", "rank5_bufget_per_tot" }
            };
            list3.Add(dto3);
            //35
            dto3 = new DPIDto
            {
                DPIFileName = "GetSql_read_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Top Disk Read SQL:disk read per exec",
                FileNameParament = new List<string> { "rank1_phyrds_per_exec", "rank2_phyrds_per_exec", "rank3_phyrds_per_exec", "rank4_phyrds_per_exec", "rank5_phyrds_per_exec" }
            };
            list3.Add(dto3);
            //35
            dto3 = new DPIDto
            {
                DPIFileName = "GetSql_read_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Top Disk Read SQL",
                FileNameParament = new List<string> { "rank1_phyrds", "rank2_phyrds", "rank3_phyrds", "rank4_phyrds", "rank5_phyrds" }
            };
            list3.Add(dto3);
            //35
            dto3 = new DPIDto
            {
                DPIFileName = "GetSql_read_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Top Disk Read Sql:disk read per Total(%)",
                FileNameParament = new List<string> { "rank1_phyrds_per_tot", "rank2_phyrds_per_tot", "rank3_phyrds_per_tot", "rank4_phyrds_per_tot", "rank5_phyrds_per_tot" }
            };
            list3.Add(dto3);
            //39
            dto3 = new DPIDto
            {
                DPIFileName = "GetSql_parse_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Top Parse SQL:parse count per Total(%)",
                FileNameParament = new List<string> { "rank1_parse_per_tot", "rank2_parse_per_tot", "rank3_parse_per_tot", "rank4_parse_per_tot", "rank5_parse_per_tot" }
            };
            list3.Add(dto3);
            //41
            dto3 = new DPIDto
            {
                DPIFileName = "GetSql_clu_wait_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Top Cluster Wait SQL:cluster wait time per elaped time(%)",
                FileNameParament = new List<string> { "rank1_cput", "rank2_cput", "rank3_cput", "rank4_cput", "rank5_cput" }
            };
            list3.Add(dto3);
            //80
            dto3 = new DPIDto
            {
                DPIFileName = "GetSQL_literal_get_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Top Literal SQL: buffer gets",
                FileNameParament = new List<string> { "rank1_bufget", "rank2_bufget", "rank3_bufget", "rank4_bufget", "rank5_bufget" }
            };
            list3.Add(dto3);
            //80
            dto3 = new DPIDto
            {
                DPIFileName = "GetSQL_literal_get_01",
                Xvalue = "TIMESTAMP",
                HeaderText = "Top Literal SQL: buffer gets per Total(%)",
                FileNameParament = new List<string> { "rank1_bufget_per_tot", "rank2_bufget_per_tot", "rank3_bufget_per_tot", "rank4_bufget_per_tot", "rank5_bufget_per_tot" }
            };
            list3.Add(dto3);
        }

       
    }
}
