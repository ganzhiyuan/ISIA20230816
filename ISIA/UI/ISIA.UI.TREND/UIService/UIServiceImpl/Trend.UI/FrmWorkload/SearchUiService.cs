using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ISIA.INTERFACE.ARGUMENTSPACK;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;
using TAP.UI;
using UIService;

namespace ISIA.UI.TREND.UIService.UIServiceImpl.Trend.UI.FrmWorkload
{
    public class SearchUiService : CommonUIService<FrmWorkloadAnalysis, object, AwrArgsPack>
    {

        public SearchUiService(FrmWorkloadAnalysis frm, object args, AwrArgsPack argsPack) : base(frm, args, argsPack)
        {

        }

        public override AwrArgsPack HandleArugument(FrmWorkloadAnalysis frm)
        {
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
                EventArgPack.StartTime = endDateTime.ToString("yyyyMMddHHmmss");
                EventArgPack.EndTime = startDateTime.ToString("yyyyMMddHHmmss");
            }
            EventArgPack.StartTime = startDateTime.ToString("yyyyMMddHHmmss");
            EventArgPack.EndTime = endDateTime.ToString("yyyyMMddHHmmss");
            EventArgPack.GroupingDateFormat = "yyyyMMdd";

            //grouping unit handling
            string groupingUnit = frm.comboBoxEditGroupUnit.Text;

            //xasix_interval check
            if (groupingUnit.Equals("HOUR"))
            {
                EventArgPack.GroupingDateFormat = "yyyyMMddHH24";
            }
            else if (groupingUnit.Equals("INTERVAL"))
            {
                EventArgPack.GroupingDateFormat = "yyyyMMddHH24mi";
            }
            //combobox edit db name 
            string dbName = frm.comboBoxDBName.Text;
            if (string.IsNullOrEmpty(dbName))
            {
                string errMessage = "Please select DB_NAME";
                throw new Exception(errMessage);
            }
            EventArgPack.DBName = dbName;

            return base.EventArgPack;
        }

        public override object GetData(AwrArgsPack args)
        {
            DataSet ds = Bs.ExecuteDataSet("GetWorkloadDataByParams", args.getPack());
            return ds;
        }

        public override object ConvertData(object data)
        {
            DataSet ds = (DataSet)data;
            object[] result = new object[2];
            DataTable originTable = ds.Tables[0];
            string[] paramStrings = AwrArgsPack.WorkloadParamNamesList.ToArray();
            DataTable[] tableSplit = new DataTable[AwrArgsPack.WorkloadParamNamesList.Count];
            for (int j = 0; j < AwrArgsPack.WorkloadParamNamesList.Count; j++)
            {
                tableSplit[j] = new DataTable();
                DataTable tempDt = tableSplit[j];
                string parmName = (string)paramStrings[j];
                tempDt.Columns.Add(parmName, originTable.Columns[parmName].DataType);
                tempDt.Columns.Add("BEGIN_TIME", originTable.Columns["BEGIN_TIME"].DataType);
                tempDt.TableName = parmName;
            }
            foreach (DataRow dr in originTable.Rows)
            {
                for (int j = 0; j < paramStrings.Length; j++)
                {
                    string parmName = (string)paramStrings[j];
                    DataTable tempDt = tableSplit[j];
                    tempDt.Rows.Add(dr[parmName], dr["BEGIN_TIME"]);
                }
            }
            result[0] = tableSplit;
            result[1] = originTable;
            return result;
        }
        public override void DisplayData(FrmWorkloadAnalysis frm, object data)
        {
            //Tfrm.tChart1 display
            frm.splitContainerControl1.Panel1.Controls.Clear();
            frm.tChart1.Zoom.Direction = ZoomDirections.None;

            frm.tChart1.Series.Clear();
            frm.tChart1.ContextMenuStrip = frm.contextMenuStrip1;
            frm.tChart1.Dock = DockStyle.Fill;
            //Header set
            frm.tChart1.Header.Text = "WORKLOAD";
            //Legend set
            frm.tChart1.Legend.LegendStyle = LegendStyles.Series;
            frm.tChart1.Legend.Visible = true;
            frm.tChart1.Legend.CheckBoxes = true;

            object[] res = (object[])data;
            DataTable[] tables = (DataTable[])res[0];
            //XAXIS MULTILINE CONTROL
            frm.tChart1.Axes.Bottom.Labels.MultiLine = true;
            //tool tip
            MarksTip marksTip = new MarksTip(frm.tChart1.Chart);
            marksTip.Active = true;
            marksTip.MouseDelay = 100;
            marksTip.MouseAction = MarksTipMouseAction.Move;
            marksTip.Style = MarksStyles.XY;
            frm.tChart1.MouseMove += new MouseEventHandler(tChart_MouseMove);
            marksTip.GetText += new MarksTipGetTextEventHandler(marksTip_GetText);
            Color color = new Color();

            foreach (DataTable dt in tables)
            {
                color.getRandomColor();
                Line line = CreateLine(dt, color);
                frm.tChart1.Series.Add(line);
                frm.tChart1.Axes.Bottom.Labels.DateTimeFormat = "yyyyMMdd hh:mm";
                frm.tChart1.Axes.Bottom.Labels.ExactDateTime = true;
                frm.tChart1.Axes.Bottom.Ticks.Width = 0;
            }
            frm.splitContainerControl1.Panel1.Controls.Add(frm.tChart1);
            //grid display
            frm.gridControlWorkloadData.DataSource = null;
            frm.gridView1.Columns.Clear();
            frm.gridControlWorkloadData.DataSource = (DataTable)res[1];
            frm.gridView1.OptionsBehavior.Editable = false;
            frm.gridView1.OptionsView.ColumnAutoWidth = false;
            frm.gridView1.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            frm.gridView1.Columns[4].DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            frm.gridView1.Columns[5].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            frm.gridView1.Columns[5].DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            frm.gridView1.BestFitColumns();
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
            public void getRandomColor()
            {
                R1 = r.Next(0, MAX);
                G1 = r.Next(0, MAX);
                S1 = r.Next(0, MAX);
            }
        }

        public DataSet LoadData()
        {
            return (DataSet)GetData(EventArgPack);
        }

        public void Display(DataSet ds)
        {
            if (ds == null)
            {
                return;
            }
            ConvertData(ds);
            DisplayData(FrmWork, ConvertData(ds));
        }

        public override void RunAsync()
        {

            HandleArugument(FrmWork);
            FrmWork.BeginAsyncCallByType("LoadData", "Display", EnumDataObject.DATASET, this.GetType(), this, null);
            //FrmWork.

        }
        private int SeriesIndex = -1;

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
