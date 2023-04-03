using ISIA.COMMON;
using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;
using ISIA.UI.TREND.Dto;
using Steema.TeeChart;
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
    public partial class FrmSQLTotalStatisticsTrend : DockUIBase1T1
    {

        DataSet dataSet;
        AwrCommonArgsPack args = new AwrCommonArgsPack();
        BizDataClient bs;
        List<Series> series = new List<Series>();

        public FrmSQLTotalStatisticsTrend()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.SnapTrendChart1");
        }





        public DataSet LoadData()
        {
            try
            {
                
                args.ParameterName = "10402";
                args.SqlId = "18c2yb5aj919t";
                dataSet = bs.ExecuteDataSet("GetSqlstatPara", args.getPack());
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DisplayData(DataSet dataSet)
        {
            if (dataSet == null)
            {
                return;
            }
            DataTable dt = ConvertDTToListRef(dataSet.Tables[0]);
            CreateTeeChart(dt);
        }

        private DataTable ConvertDTToListRef(DataTable dt)
        {
            List<SqlstatDto> list = DataTableExtend.GetList<SqlstatDto>(dt);
            SqlstatDto dto = new SqlstatDto();
            PropertyInfo[] fields = dto.GetType().GetProperties();
            List<string> listTotal = new List<string>();
            for (int i = 0; i < fields.Length; i++)
            {
                string ss = fields[i].Name;
                if (ss.Contains("TOTAL"))
                {
                    listTotal.Add(ss);
                }
            }
            List<SqlStatRowDto> returnList = new List<SqlStatRowDto>();

            foreach (SqlstatDto item in list)
            {
                PropertyInfo[] proInfo = item.GetType().GetProperties(); 
                foreach (var s in listTotal)
                {
                    SqlStatRowDto rowDto = new SqlStatRowDto();
                    rowDto.DBID = item.DBID;
                    rowDto.SQL_ID = item.SQL_ID;
                    rowDto.PARAMENT_NAME = s;
                    foreach (PropertyInfo para in proInfo)
                    {
                        if (s==para.Name)
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
            var cuTool = new CursorTool(tChart1.Chart)
            {
                Style = CursorToolStyles.Both,
                FollowMouse = true,

            };
            var markstip = new MarksTip(tChart1.Chart);
            cuTool.Pen.Color = Color.Red;
            tChart1.MouseEnter += Chart_MouseEnter;
            tChart1.MouseLeave += Chart_MouseLeave;
            void Chart_MouseEnter(object sender, EventArgs e)
            {
                cuTool.Pen.Visible = true;
            }
            void Chart_MouseLeave(object sender, EventArgs e)
            {
                cuTool.Pen.Visible = false;
            }

            //chart.Chart.Series.Chart.GetASeries().Legend.Text.ToString();
            //chart.ContextMenuStrip = contextMenuStrip1;
            tChart1.Dock = DockStyle.Fill;
            tChart1.Legend.LegendStyle = LegendStyles.Series;//Legend显示样式以Series名字显示
            tChart1.Header.Text = "TEECHART";//teechart标题 
            tChart1.Legend.Visible = true;
            IEnumerable<IGrouping<string, DataRow>> result = dsTable.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["SNAP_ID"].ToString());
            if (result != null && result.Count() > 0)
            {
                foreach (IGrouping<string, DataRow> rows in result)
                {
                    DataTable dataTable = rows.ToArray().CopyToDataTable();
                    dataTable.TableName = rows.Key;
                    if (dataTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(dataTable);
                    }
                }
            }
            if (dataSet.Tables.Count > 1)
            {

                foreach (DataTable dt in dataSet.Tables)
                {
                    if (dt.TableName != "TABLE")
                    {
                        Bar bar = CreateLine(dt);
                        series.Add(bar);
                        tChart1.Series.Add(bar);
                    }
                }
            }
            /*string daytime =  radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Value.ToString();
            if (daytime == "day" )
            {
                chart.Axes.Bottom.Labels.DateTimeFormat = "MM-dd";//x轴横坐标值
            }
            else if (daytime == "hour")
            {
                chart.Axes.Bottom.Labels.DateTimeFormat = "MM-dd_HH";
            }
            else if (daytime == "min")
            {
                chart.Axes.Bottom.Labels.DateTimeFormat = "MM-dd_HH:MI";
            }*/
            //tChart1.Axes.Bottom.Labels.Angle = 1;

            //tChart1.Axes.Bottom.Labels.DateTimeFormat = "MM-dd HH:MI";
            //tChart1.Axes.Bottom.Labels.ExactDateTime = true;//x轴显示横坐标为时间
            //line.Legend.Visible = true;
            return;
        }



        private Bar CreateLine(DataTable dstable)
        {
            Bar bar = new Bar();
            /*var nearpoint = new NearestPoint(chart.Chart) {
                Series = line,
                Style = NearestPointStyles.None,
                Direction = NearestPointDirection.Both,
                Size = 1
            };
            nearpoint.Pen.Color = Color.Red;*/
            //nearpoint.Pen.Visible = false;
            //line.Pointer.Style = PointerStyles.Circle;
            //line.Pointer.Visible = true;
            //line.Pointer.HorizSize = 120;

            //line.ClickPointer += Line_ClickPointer;
            //line.GetSeriesMark += Line_GetSeriesMark;
            //void Line_GetSeriesMark(Series series, GetSeriesMarkEventArgs e)
            //{
            //    e.MarkText = "PARAMETER :" + $"{dstable.Rows[e.ValueIndex]["STAT_NAME"]}" + "\r\n" + "VALUE :" + $"{dstable.Rows[e.ValueIndex]["VALUE"]}" + "\r\n" + "TIME :" + $"{ dstable.Rows[e.ValueIndex]["BEGIN_INTERVAL_TIME"]}";
            //}
            bar.DataSource = dstable;
            bar.YValues.DataMember = cboParaName.Text;
            //line.XValues.DataMember = "ROWNUM";
            bar.ShowInLegend = true;
            bar.Legend.Text = dstable.TableName;
            bar.Legend.BorderRound = 10;
            //line.XValues.DateTime = true;

            return bar;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!base.ValidateUserInput(this.layoutControl1)) return;


                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
