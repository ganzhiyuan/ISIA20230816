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
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;

namespace ISIA.UI.TREND
{
    public partial class FrmMainForm : DockUIBase1T1
    {

        BizDataClient bs;
        public FrmMainForm()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.PerformaceEvaluationTrend");
        }

        private async void LoadDataAsync(TChart tChart)
        {
            // 异步加载数据
            var data = await Task.Run(() => GetData());

            // 将数据绑定到 TChart 中
            tChart.Series.Clear();
            var series = new Bar(); // 这里以 Bar 为例，你也可以根据需要选择其他类型的图表
            foreach (var item in data)
            {
                series.Add(item.Value, item.Key);
            }
            tChart.Series.Add(series);
        }

        private Dictionary<string, double> GetData()
        {
            // TODO: 获取数据的代码

            return null;
        }

        // 定时器回调函数
        private void TimerCallback(object state)
        {
            // 遍历所有 TChart 控件
            foreach (var tChart in this.Controls.OfType<TChart>())
            {
                // 使用 Invoke 方法调用 UI 线程上的方法
                tChart.Invoke((Action)(() =>
                {
                    // 调用异步加载方法
                    LoadDataAsync(tChart);
                }));
            }
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
            for (int i = 0; i < 5; i++)
            {
                // 创建 TChart 控件
                TChart tChart = new TChart();
                tChart.Width = 200;
                tChart.Height = 150;
                // 设置每个 TChart 控件的其他属性或数据

                // 将 TChart 控件添加到 FlowLayoutPanel 中
                flowLayoutPanel1.Controls.Add(tChart);
            }
            comboBoxEdit1_SelectedIndexChanged(null, null);
        }
    }
}
