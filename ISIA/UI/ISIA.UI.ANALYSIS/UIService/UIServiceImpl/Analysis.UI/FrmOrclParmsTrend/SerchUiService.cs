using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.ANALYSIS;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;
using TAP.Base.Mathematics;
using TAP.UI;

namespace UIHelper.UIServiceImpl.Analysis.UI.FrmOrclParmsTrend
{
    public class SerchUiService : CommonUIService<FrmOrclParmsTrendChart, object, AwrArgsPack>
    {

        private double[][] _ClusterInput = null;

        private SortedDictionary<int, List<DataTable>> _ClusteringDt = new SortedDictionary<int, List<DataTable>>();

        public SerchUiService(FrmOrclParmsTrendChart frm, object args, AwrArgsPack argsPack) : base(frm, args, argsPack)
        {

        }

        public override AwrArgsPack HandleArugument(FrmOrclParmsTrendChart frm)
        {
            //param_name arguments
            List<object> paramList = frm.tCheckComboBoxParmNames.Properties.Items.GetCheckedValues();
            if (paramList == null || paramList.Count <= 0)
            {
                string errMessage = "Please select Param Names";
                throw new Exception(errMessage);
            }
            EventArgPack.ParamNamesList = paramList;

            //time argument checked
            object startTime = frm.dateStart.EditValue;
            object endTime = frm.dateEnd.EditValue;
            if (startTime == null || endTime == null)
            {
                string errMessage = "Please select StartTime or EndTime";
                throw new Exception(errMessage);
            }
            DateTime startDateTime = (DateTime)frm.dateStart.EditValue;
            DateTime endDateTime = (DateTime)frm.dateEnd.EditValue;

            if (startDateTime > endDateTime)
            {
                EventArgPack.StartTime = endDateTime.ToString("yyyyMMdd");
                EventArgPack.EndTime = startDateTime.ToString("yyyyMMdd");
            }
            EventArgPack.StartTime = startDateTime.ToString("yyyyMMdd");
            EventArgPack.EndTime = endDateTime.ToString("yyyyMMdd");
            EventArgPack.GroupingDateFormat = "yyyyMMdd";

            //xasix_interval check
            if (frm.checkEditHour.Checked)
            {
                EventArgPack.StartTime = EventArgPack.StartTime + "00";
                EventArgPack.EndTime = EventArgPack.EndTime + "23";
                EventArgPack.GroupingDateFormat = "yyyyMMddHH24";
            }
            else if (frm.checkEditMin.Checked)
            {
                EventArgPack.StartTime = EventArgPack.StartTime + "000";
                EventArgPack.EndTime = EventArgPack.EndTime + "235";
                EventArgPack.GroupingDateFormat = "yyyyMMddHH24mi";
            }
            object clusterNum = frm.tCheckComboBoxClusteringCnt.Properties.Items.GetCheckedValues()[0];
            EventArgPack.ClustersNumber = (int)clusterNum;
            EventArgPack.ClustersNumber = EventArgPack.ClustersNumber < paramList.Count ? EventArgPack.ClustersNumber : paramList.Count;
            return base.EventArgPack;
        }

        public override object GetData(AwrArgsPack args)
        {

            DataSet ds = Bs.ExecuteDataSet("GetParmDailyTrendData", args.getPack());
            return ds;
        }

        public override object ConvertData(object data)
        {
            DataSet ds = (DataSet)data;

            return ds.Tables;
        }

        public override void DisplayData(FrmOrclParmsTrendChart frm, object data)
        {
            frm.splitContainerControl1.Panel1.Controls.Clear();
            TChart chart = new TChart();
            chart.Series.Clear();
            //show tooltip
            MarksTip marksTip = new MarksTip(chart.Chart);
            marksTip.Active = true;
            marksTip.MouseDelay = 100;
            marksTip.MouseAction = MarksTipMouseAction.Move;
            marksTip.Style = MarksStyles.XY;
            chart.MouseMove += new MouseEventHandler(tChart_MouseMove);
            marksTip.GetText += new MarksTipGetTextEventHandler(marksTip_GetText);
            chart.ContextMenuStrip = frm.contextMenuStrip1;
            //
            chart.Dock = DockStyle.Fill;
            chart.Legend.LegendStyle = LegendStyles.Series;
            chart.Header.Text = "PARAMETER TREND";
            chart.Legend.Visible = true;
            DataTableCollection tables = (DataTableCollection)data;
            //axex show multi-line
            chart.Axes.Bottom.Labels.MultiLine = true;
            //Line Color Selection 
            Color color = new Color();
            //clusterInput init 
            ClusterInput = new double[tables.Count][];
            int cycleIndex = 0;
            foreach (DataTable dt in tables)
            {
                color.getRandomColor();
                Line line = CreateLine(dt, color);
                chart.Series.Add(line);
                chart.Axes.Bottom.Labels.DateTimeFormat = "yyyyMMdd hh:mm";
                chart.Axes.Bottom.Labels.ExactDateTime = true;
                chart.Axes.Bottom.Ticks.Width = 0;
                Type tp = dt.Columns[dt.TableName].DataType;
                //handle clustering 
                List<Decimal> columns = dt.AsEnumerable().Select(r => r.Field<Decimal>(dt.TableName)).ToList<Decimal>();
                List<double> res = new List<double>();
                columns.ForEach(ele => res.Add(Double.Parse(ele.ToString())));
                ClusterInput[cycleIndex++] = res.ToArray();
            }
            //add main chart to panel1
            frm.splitContainerControl1.Panel1.Controls.Add(chart);
            //display clustering chart 
            Statistics st = new Statistics();
            int[] clusteringResult = st.KMeans(ClusterInput, EventArgPack.ClustersNumber);
            cycleIndex = 0;
            foreach (DataTable dt in tables)
            {
                int index = clusteringResult[cycleIndex++];
                if (!ClusteringDt.ContainsKey(index))
                {
                    List<DataTable> clusteredTables = new List<DataTable>();
                    ClusteringDt.Add(index, clusteredTables);
                }
                ClusteringDt[index].Add(dt);
            }
            frm.chartLayout1.Charts.Clear();
            foreach (var pair in ClusteringDt)
            {
                List<DataTable> tempDt = pair.Value;
                TChart chartCluster = frm.chartLayout1.Add(pair.Key.ToString());
                chartCluster.Size = new System.Drawing.Size(800, 400);
                chartCluster.Axes.Bottom.Labels.MultiLine = true;
                chartCluster.ContextMenuStrip = frm.contextMenuStrip1;
                //
                //show tooltip
                MarksTip marksTipCluster = new MarksTip(chartCluster.Chart);
                marksTipCluster.Active = true;
                marksTipCluster.MouseDelay = 100;
                marksTipCluster.MouseAction = MarksTipMouseAction.Move;
                marksTipCluster.Style = MarksStyles.XY;
                chartCluster.MouseMove += new MouseEventHandler(tChart_MouseMove);
                marksTipCluster.GetText += new MarksTipGetTextEventHandler(marksTip_GetText);
                foreach (DataTable dt in tempDt)
                {
                    color.getRandomColor();
                    Line line = CreateLine(dt, color, chartCluster);
                    chartCluster.Series.Add(line);
                    chartCluster.Axes.Bottom.Labels.DateTimeFormat = "yyyyMMdd hh:mm";
                    chartCluster.Axes.Bottom.Labels.ExactDateTime = true;
                    chartCluster.Axes.Bottom.Ticks.Width = 0;
                }
            }
            //layout loacation
            int locationWidth = 0;
            int locationLength = 0;
            foreach (TChart ele in frm.chartLayout1.Charts)
            {
                ele.Location = new System.Drawing.Point(locationWidth, locationLength);
                locationWidth += 800;
                if (locationWidth >= 1200)
                {
                    locationWidth = 0;
                    locationLength += 420;
                }
                ele.Size = new System.Drawing.Size(800, 400);
            }
        }
        public DataSet LoadData()
        {
            return (DataSet)GetData(HandleArugument(FrmWork));
        }

        public void Display(DataSet ds)
        {
            if (ds == null)
            {
                return;
            }
            DisplayData(FrmWork, ConvertData(ds));
        }

        public override void RunAsync()
        {
            try
            {
                FrmWork.BeginAsyncCallByType("LoadData", "Display", EnumDataObject.DATASET, this.GetType(), this, null);
            }
            catch (Exception ex)
            {
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }



        public override int GetHashCode()
        {
            return base.GetHashCode();
        }



        public override string ToString()
        {
            return base.ToString();
        }

        private Line CreateLine(DataTable dt, Color color)
        {
            Line line = new Line();
            line.DataSource = dt;
            line.YValues.DataMember = dt.TableName;
            line.XValues.DataMember = "BEGIN_TIME";
            line.ShowInLegend = true;
            line.Legend.Text = dt.TableName;
            line.Title = dt.TableName;
            line.Color =
                System.Drawing.Color.FromArgb(color.R1, color.G1, color.S1);
            line.Legend.BorderRound = 20;
            line.XValues.DateTime = true;
            return line;
        }

        private Line CreateLine(DataTable dt, Color color, TChart chart)
        {
            Line line = new Line(chart.Chart);
            line.DataSource = dt;
            line.YValues.DataMember = dt.TableName;
            line.XValues.DataMember = "BEGIN_TIME";
            line.ShowInLegend = true;
            line.Legend.Text = dt.TableName;
            line.Title = dt.TableName;
            line.Color =
                System.Drawing.Color.FromArgb(color.R1, color.G1, color.S1);
            line.Legend.BorderRound = 20;
            line.XValues.DateTime = true;
            return line;
        }
        private class Color
        {
            int R;
            int G;
            int S;

            Random r = new Random();

            public int R1 { get => R; set => R = value; }
            public int G1 { get => G; set => G = value; }
            public int S1 { get => S; set => S = value; }

            public static int MAX = 255;
            public void getRandomColor()
            {
                R1 = r.Next(0, MAX);
                G1 = r.Next(0, MAX);
                S1 = r.Next(0, MAX);
            }

        }
        private int SeriesIndex = -1;

        public double[][] ClusterInput { get => _ClusterInput; set => _ClusterInput = value; }
        public SortedDictionary<int, List<DataTable>> ClusteringDt { get => _ClusteringDt; set => _ClusteringDt = value; }
        void tChart_MouseMove(object sender, MouseEventArgs e)
        {
            TChart tmpChart = sender as TChart;
            for (int i = 0; i < tmpChart.Series.Count; i++)
            {
                if (tmpChart[i].Clicked(e.X, e.Y) != -1)
                {
                    SeriesIndex = i;
                    break;
                }
                SeriesIndex = -1;
            }
        }
        void marksTip_GetText(Steema.TeeChart.Tools.MarksTip sender, Steema.TeeChart.Tools.MarksTipGetTextEventArgs e)
        {
            MarksTip marks = sender as MarksTip;
            if (e.Text.Contains("Date") || SeriesIndex == -1)
            {
                return;
            }
            var strPrmt = marks.Chart.Series[SeriesIndex].Legend.Text;
            int indexPosition = e.Text.IndexOf(":") + 2;
            e.Text = "Parm: " + strPrmt + "\n" +
            "Date: " + e.Text.Substring(0, indexPosition) + "\n" + "Value:" +
            e.Text.Substring(indexPosition + 1);
        }
    }
}
