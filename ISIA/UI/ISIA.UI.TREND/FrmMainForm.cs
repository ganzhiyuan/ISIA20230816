﻿using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;
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
        public FrmMainForm()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.PerformaceEvaluationTrend");
        }


        public DataSet LoadData()
        {            

            return dataSet;
        }
        public void DisplayData(DataSet ds)
        {
            if (dataSet == null)
            {
                return;
            }
            //ConvertData(dataSet);

            //CreateTeeChart(dataSet.Tables[0]);
            dataSet.Tables.Clear();

        }



        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 设置子控件的宽度和高度
            int width = flowLayoutPanel1.ClientSize.Width / Convert.ToInt32(comboBoxEdit1.EditValue);
            int height = flowLayoutPanel1.ClientSize.Height / Convert.ToInt32(comboBoxEdit1.EditValue);

            foreach (var chart in flowLayoutPanel1.Controls.OfType<TChart>())
            {
                chart.Width = width-10;
                chart.Height = height-10;
            }

        }

        private void FrmMainForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
                // 创建 TChart 控件
                TChart tChart = new TChart();
                tChart.Tag = i;
                tChart.Width = 200;
                tChart.Height = 150;
                // 设置每个 TChart 控件的其他属性或数据

                // 将 TChart 控件添加到 FlowLayoutPanel 中
                flowLayoutPanel1.Controls.Add(tChart);
            }
            comboBoxEdit1_SelectedIndexChanged(null, null);
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

                //ComboBoxControl.SetCrossLang(this._translator);
                if (!base.ValidateUserInput(this.lcSerachOptions)) return;
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);

                //var charts1 = flowLayoutPanel1.Controls.OfType<TChart>();
                //LoadChartDataAsync(charts1);


                await QueryDataForAllTCharts();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private async Task QueryDataForAllTCharts()
        {
            // 禁用查询按钮
            btnSelect.Enabled = false;
            var charts1 = flowLayoutPanel1.Controls.OfType<TChart>();
            foreach (TChart chart in charts1)
            {
                ShowWaitIcon(chart);
                await Task.Run(() =>
                {
                    // 查询每个TChart控件的数据
                    QueryDataForTChart(chart);
                });
                HideWaitIcon(chart);
            }

            // 使用异步方法查询数据
            

            // 启用查询按钮
            btnSelect.Enabled = true;
        }


        private void QueryDataForTChart(TChart tChart)
        {
            tChart.Series.Clear();
            // 实际的查询逻辑
            args.StartTimeKey = dtpStartTime.DateTime.ToString("yyyy-MM-dd");
            args.EndTimeKey = dtpEndTime.DateTime.ToString("yyyy-MM-dd");
            args.DbName = cmbDbName.Text.Split('(')[0];
            args.DbId = cmbDbName.EditValue.ToString();
            args.InstanceNumber = cmbInstance.EditValue.ToString();
            args.SnapId = "";

            dataSet = bs.ExecuteDataSet("GetOsstat", args.getPack());
            for (int i = 1; i <= 4; i++)
            {
                Line line = CreateLine(dataSet.Tables[0], i, "TIMESTAMP","");
                tChart.Series.Add(line);
            }
            // 将查询到的数据设置到TChart控件中
            
        }
        private Line CreateLine(DataTable dstable,decimal d,string Xvalue,string Yvalue)
        {
            Line line = new Line();

            line.DataSource = dstable;
            line.XValues.DataMember = "TIMESTAMP";
            line.Legend.Visible = false;
            if (d==1)
            {
                line.YValues.DataMember = "%usr";
                line.Color = Color.Red;
            }
            else if (d==2)
            {
                line.YValues.DataMember = "%sys";
                line.Color = Color.Blue;
            }
            else if (d==3)
            {
                line.YValues.DataMember = "%wio";
                line.Color = Color.Green;
            }
            else
            {
                line.YValues.DataMember = "%idle";
                line.Color = Color.OrangeRed;
            }

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
            chart.Header.Text = "加载中...";
        }

        private void HideWaitIcon(TChart chart)
        {
            chart.Header.Text = "";
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
