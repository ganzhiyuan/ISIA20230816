using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;
using ISIA.UI.TREND.Dto;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using System;
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
        AwrCommonArgsPack args = new AwrCommonArgsPack();
        List<DPIDto> list = new List<DPIDto>();
        List<DPIDto> list2 = new List<DPIDto>();
        List<DPIDto> list3 = new List<DPIDto>();

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

        }

        private void FrmMainForm_Load(object sender, EventArgs e)
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
            //comboBoxEdit1_SelectedIndexChanged(null, null);
            //CreateCharts();
        }


        private async void btnSelect_Click(object sender, EventArgs e)
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

                if (!base.ValidateUserInput(this.lcSerachOptions)) return;
                //base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);

                await QueryDataForAllTChartsAsyncSheet1();
                await QueryDataForAllTChartsAsyncSheet2();
                await QueryDataForAllTChartsAsyncSheet3();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private async Task QueryDataForAllTChartsAsyncSheet2()
        {
            // 禁用查询按钮
            btnSelect.Enabled = false;
            var charts2 = flowLayoutPanel2.Controls.OfType<TChart>().ToArray();
            for (int i = 0; i < list2.Count(); i++)
            {
                ShowWaitIcon(charts2.ElementAt(i));
                await Task.Run(() =>
                {
                    // 查询每个TChart控件的数据
                    QueryDataForTChart(charts2.ElementAt(i), list2[i]);
                });
                HideWaitIcon(charts2.ElementAt(i), list2[i].HeaderText);
            }
            // 启用查询按钮
            btnSelect.Enabled = true;
        }
        private async Task QueryDataForAllTChartsAsyncSheet3()
        {
            // 禁用查询按钮
            btnSelect.Enabled = false;
            var charts3 = flowLayoutPanel3.Controls.OfType<TChart>().ToArray();
            for (int i = 0; i < list3.Count(); i++)
            {
                ShowWaitIcon(charts3.ElementAt(i));
                await Task.Run(() =>
                {
                    // 查询每个TChart控件的数据
                    QueryDataForTChart(charts3.ElementAt(i), list3[i]);
                });
                HideWaitIcon(charts3.ElementAt(i), list3[i].HeaderText);
            }
            // 启用查询按钮
            btnSelect.Enabled = true;
        }

        private async Task QueryDataForAllTChartsAsyncSheet1()
        {
            // 禁用查询按钮
            btnSelect.Enabled = false;
            var charts1 = flowLayoutPanel1.Controls.OfType<TChart>().ToArray();
            for (int i = 0; i < list.Count(); i++)
            {
                ShowWaitIcon(charts1.ElementAt(i));
                await Task.Run(() =>
                {
                    // 查询每个TChart控件的数据
                    QueryDataForTChart(charts1.ElementAt(i),list[i]);
                });
                HideWaitIcon(charts1.ElementAt(i),list[i].HeaderText);
            }
            // 启用查询按钮
            btnSelect.Enabled = true;
        }


        private void QueryDataForTChart(TChart tChart,DPIDto dto)
        {
            tChart.Series.Clear();
            // 实际的查询逻辑
            args.StartTimeKey = dtpStartTime.DateTime.ToString("yyyy-MM-dd");
            args.EndTimeKey = dtpEndTime.DateTime.ToString("yyyy-MM-dd");
            args.DbName = cmbDbName.Text.Split('(')[0];
            args.DbId = cmbDbName.EditValue.ToString();
            args.InstanceNumber = cmbInstance.EditValue.ToString();
            args.SnapId = "";
            
            dataSet = bs.ExecuteDataSet(dto.DPIFileName, args.getPack());
            for (int i = 0; i < dto.FileNameParament.Count(); i++)
            {
                Line line = CreateLine(dataSet.Tables[0], dto,i);
                tChart.Series.Add(line);
            }
            // 将查询到的数据设置到TChart控件中
            
        }
        private Line CreateLine(DataTable dstable, DPIDto dto, int i)
        {
            Line line = new Line();

            line.DataSource = dstable;
            line.XValues.DataMember = dto.Xvalue;
            line.Legend.Visible = false;
            string str = dto.FileNameParament[i].ToString();
            line.YValues.DataMember = str;
            line.Color = Color.OrangeRed;

            line.Pointer.HorizSize = 1;
            line.Pointer.VertSize = 1;
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
                FileNameParament = new List<string>{"%usr","%sys","%wio","%idle"}
            };
            list.Add(dto);
            //18
            //dto = new DPIDto
            //{
            //    DPIFileName = "GetTimeModel",
            //    Xvalue = "TIMESTAMP",
            //    HeaderText = "DB&Background Times(s)",
            //    FileNameParament = new List<string> { "DB time", "DB CPU", "background", "background cpu" }
            //};
            //list.Add(dto);
            ////1
            //dto = new DPIDto
            //{
            //    DPIFileName = "GetLoadSQL",
            //    Xvalue = "TIMESTAMP",
            //    HeaderText = "Lgical/Physical Reads",
            //    FileNameParament = new List<string> { "Logical reads", "Physical reads" }
            //};
            //list.Add(dto);
            ////1
            //dto = new DPIDto
            //{
            //    DPIFileName = "GetLoadSQL",
            //    Xvalue = "TIMESTAMP",
            //    HeaderText = "User Call/Execute Count",
            //    FileNameParament = new List<string> { "User calls", "Executes" }
            //};
            //list.Add(dto);
            ////62
            //dto = new DPIDto
            //{
            //    DPIFileName = "GetResource",
            //    Xvalue = "TIMESTAMP",
            //    HeaderText = "Session/Active Count",
            //    FileNameParament = new List<string> { "sessions_curr", "sessions_max", "active_session_cnt" }
            //};
            //list.Add(dto);
            ////12
            //dto = new DPIDto
            //{
            //    DPIFileName = "GetWait5_01",
            //    Xvalue = "TIMESTAMP",
            //    HeaderText = "Top 5 Wait Events(time(s))",
            //    FileNameParament = new List<string> { "time_1", "time_2", "time_3", "time_4", "time_5" }
            //};
            //list.Add(dto);
            //68
            /*dto = new DPIDto
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
            //61
            dto = new DPIDto
            {
                DPIFileName = "GetWaitstat",
                Xvalue = "TIMESTAMP",
                HeaderText = "Wait count by class",
                FileNameParament = new List<string> { "data block", "segment header", "undo block", "undo header" }
            };
            list.Add(dto);
            //64
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
            //71
            dto = new DPIDto
            {
                DPIFileName = "GetSga",
                Xvalue = "TIMESTAMP",
                HeaderText = "SGA",
                FileNameParament = new List<string> { "buf.cache(M)", "shared.pool(M)", "java.pool(M)", "large.pool(M)", "streams.pool(M)" }
            };
            //71
            dto = new DPIDto
            {
                DPIFileName = "GetSga",
                Xvalue = "TIMESTAMP",
                HeaderText = "shared pool",
                FileNameParament = new List<string> { "sqlarea(M)", "lib.cache(M)", "others(M)", "free(M)" }
            };
            //54
            dto = new DPIDto
            {
                DPIFileName = "GetPga",
                Xvalue = "TIMESTAMP",
                HeaderText = "PGA Statistics",
                FileNameParament = new List<string> { "sqlarea(M)", "lib.cache(M)", "others(M)", "free(M)" }
            };

            //55
            dto = new DPIDto
            {
                DPIFileName = "GetWorkarea_raw",
                Xvalue = "TIMESTAMP",
                HeaderText = "PGA Memory/Disk sort",
                FileNameParament = new List<string> { "one_exe", "mul_exe", "tot_exe", "opt_exe" }
            };
            //03
            dto = new DPIDto
            {
                DPIFileName = "GetRacGcEfficiency",
                Xvalue = "TIMESTAMP",
                HeaderText = "Buffer Access{local/remote/disk}%",
                FileNameParament = new List<string> { "Buffer access - local cache%", "Buffer access - remote cache%", "Buffer access - disk%" }
            };*/
            //--sheet2
            //01
            /*dto = new DPIDto
            {
                DPIFileName = "GetLoadSQL",
                Xvalue = "TIMESTAMP",
                HeaderText = "transaction(per sec)",
                FileNameParament = new List<string> { "Transactions" }
            };
            list2.Add(dto);
            //01
            dto = new DPIDto
            {
                DPIFileName = "GetLoadSQL",
                Xvalue = "TIMESTAMP",
                HeaderText = "Redo Size(Bytes)",
                FileNameParament = new List<string> { "Redo size" }
            };
            list2.Add(dto);
            //01
            dto = new DPIDto
            {
                DPIFileName = "GetLoadSQL",
                Xvalue = "TIMESTAMP",
                HeaderText = "Parse Count(Per Sec)",
                FileNameParament = new List<string> { "Parses(total)" }
            };
            list2.Add(dto);
            //01
            dto = new DPIDto
            {
                DPIFileName = "GetLoadSQL",
                Xvalue = "TIMESTAMP",
                HeaderText = "Hard Parse Count(per sec)",
                FileNameParament = new List<string> { "Parses(hard)" }
            };
            list2.Add(dto);
            //18
            dto = new DPIDto
            {
                DPIFileName = "GetTimeModel",
                Xvalue = "TIMESTAMP",
                HeaderText = "DB Time(s)",
                FileNameParament = new List<string> { "DB time" }
            };
            list2.Add(dto);
            //72
            dto = new DPIDto
            {
                DPIFileName = "GetLog_switch_f",
                Xvalue = "TIMESTAMP",
                HeaderText = "Log Switches",
                FileNameParament = new List<string> { "Log Switch interval <= 5 min" }
            };
            list2.Add(dto);*/

            //54
            /*dto = new DPIDto
            {
                DPIFileName = "GetPga_hit",
                Xvalue = "TIMESTAMP",
                HeaderText = "PGA Cache Hit(%)",
                FileNameParament = new List<string> { "PGA Cache Hit %" }
            };
            list2.Add(dto);*/
        }

        //------------------------------------------------

        //private async void LoadChartDataAsync(IEnumerable<TChart> charts)
        //{
        //    if (!charts.Any())
        //    {
        //        MessageBox.Show("No charts found");
        //        return;
        //    }

        //    // Show wait icon on each TChart control
        //    foreach (var chart in charts)
        //    {
        //        ShowWaitIcon(chart);
        //    }

        //    try
        //    {
        //        // Load data for each TChart control asynchronously
        //        var tasks = new List<Task>();
        //        foreach (var chart in charts)
        //        {
        //            tasks.Add(LoadDataAsync(chart));
        //        }
        //        await Task.WhenAll(tasks); // Wait for all tasks to complete
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        // Hide wait icon on each TChart control
        //        foreach (var chart in charts)
        //        {
        //            HideWaitIcon(chart);
        //        }
        //    }
        //}

        //private async Task LoadDataAsync(TChart chart)
        //{
        //    // Show wait icon on TChart control
        //    ShowWaitIcon(chart);

        //    try
        //    {
        //        // Simulate data loading by waiting for a random time
        //        var rnd = new Random();
        //        await Task.Delay(rnd.Next(1000, 3000));

        //        // Bind data to TChart control
        //        var data = new double[10];
        //        for (int i = 0; i < data.Length; i++)
        //        {
        //            data[i] = rnd.NextDouble() * 100;
        //        }
        //        //chart.Series[0].Add(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        // Hide wait icon on TChart control
        //        HideWaitIcon(chart);
        //    }
        //}
        //private void ShowWaitIcon(TChart chart)
        //{
        //    chart.Header.Text = "加载中...";
        //}

        //private void HideWaitIcon(TChart chart)
        //{
        //    chart.Header.Text = "";
        //}


    }
}
