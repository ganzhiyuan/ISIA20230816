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

namespace UIHelper.UIServiceImpl.Analysis.UI.FrmOrclParmsTrend
{
    public class SerchUiService : CommonUIService<FrmOrclParmsTrendChart, object, AwrArgsPack>
    {

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
                EventArgPack.GroupingDateFormat = "yyyyMMddHH24m";
            }


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
            //show x,y value
            MarksTip marksTip1 = new MarksTip(chart.Chart);
            marksTip1.Active = true;
            marksTip1.MouseDelay = 100;
            marksTip1.MouseAction = MarksTipMouseAction.Move;
            marksTip1.Style = MarksStyles.XY;

            chart.ContextMenuStrip = frm.contextMenuStrip1;
            //
            chart.Dock = DockStyle.Fill;
            chart.Legend.LegendStyle = LegendStyles.Series;
            chart.Header.Text = "PARAMETER TREND";
            chart.Legend.Visible = true;
            DataTableCollection tables = (DataTableCollection)data;

            chart.Axes.Bottom.Labels.MultiLine = true;
            Color color = new Color();

            foreach (DataTable dt in tables)
            {
                color.getRandomColor();
               Line line = CreateLine(dt,color);
                chart.Series.Add(line);
                chart.Axes.Bottom.Labels.DateTimeFormat = "yyyyMMdd hh:mm";
                chart.Axes.Bottom.Labels.ExactDateTime = true;
                chart.Axes.Bottom.Ticks.Width = 0;
            }
            frm.splitContainerControl1.Panel1.Controls.Add(chart);

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
            public void  getRandomColor()
            {
                R1 = r.Next(0, MAX);
                G1 = r.Next(0, MAX);
                S1= r.Next(0, MAX);
            }
        }
        
        

       
    }
}
