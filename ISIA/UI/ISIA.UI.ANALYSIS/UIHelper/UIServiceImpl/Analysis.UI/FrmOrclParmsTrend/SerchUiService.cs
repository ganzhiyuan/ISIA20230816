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
            List<object> paramList = frm.tCheckComboBoxParmNames.Properties.Items.GetCheckedValues();
            if (paramList == null || paramList.Count <= 0)
            {
                string errMessage = "Please select Param Names";
                throw new Exception(errMessage);
            }
            EventArgPack.ParamNamesList = paramList;

            //time checked
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
            foreach(DataTable dt in tables)
            {
                Line line = CreateLine(dt);
                chart.Series.Add(line);
                chart.Axes.Bottom.Labels.DateTimeFormat = "yyyyMMdd";
                chart.Axes.Bottom.Labels.ExactDateTime = true;
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

        private Line CreateLine(DataTable dt)
        {
            Line line = new Line();
            line.DataSource = dt;
            line.YValues.DataMember = dt.TableName;
            line.XValues.DataMember = "BEGIN_TIME";
            line.ShowInLegend = true;
            line.Legend.Text = dt.TableName;
            line.Title = dt.TableName;
            line.Legend.BorderRound = 20;
            line.XValues.DateTime = true;
            return line;
        }
    }
}
