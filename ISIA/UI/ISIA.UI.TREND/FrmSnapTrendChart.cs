using ISIA.COMMON;
using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;
using ISIA.UI.TREND.Dto;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;
using System;
using System.Collections;
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
    public partial class FrmSnapTrendChart : DockUIBase1T1
    {

        protected PointF _pStart;
        protected PointF _pEnd;
        List<SqlStatRowDto> returnList = null;
        private bool _dragPoint = false;
        private bool _PointMap = false;
        private bool bfirst = false;
        AwrCommonArgsPack args = new AwrCommonArgsPack();
        BizDataClient bs;
        DataSet dataSet = null;
        DataSet dataSet1 = new DataSet();
        List<Series> series = new List<Series>();
        List<SnapshotDto> snaplist = new List<SnapshotDto>();

        public FrmSnapTrendChart()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.SnapTrendChart");
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
        public DataSet LoadData()
        {
            try
            {
                dataSet1.Tables.Clear();
                
                
                args.StartTimeKey = dateStart.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                args.EndTimeKey = dateEnd.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                args.ParameterName = cboParaName.Text;
                dataSet = bs.ExecuteDataSet("GetSnap");



                DataTable dt = ConvertDTToListRef(dataSet.Tables[0]);
                dataSet1.Tables.Add(dt.Copy());
                dataSet1.Tables[0].TableName = "TABLE";


                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }
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
            returnList = new List<SqlStatRowDto>();

            foreach (SqlstatDto item in list)
            {
                PropertyInfo[] proInfo = item.GetType().GetProperties();
                foreach (var s in listTotal)
                {
                    SqlStatRowDto rowDto = new SqlStatRowDto();
                    rowDto.DBID = item.DBID;
                    rowDto.SQL_ID = item.SQL_ID;
                    rowDto.PARAMENT_NAME = s;
                    rowDto.END_INTERVAL_TIME = item.END_INTERVAL_TIME;
                    foreach (PropertyInfo para in proInfo)
                    {
                        if (s == para.Name)
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


        public void DisplayData(DataSet dataSet)
        {
            if (dataSet == null)
            {
                return;
            }
            CreateTeeChart(dataSet1.Tables[0]);
        }
        private void tChart1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && bfirst)
            {
                _pEnd.X = (float)e.X;
                _pEnd.Y = (float)e.Y;
                bfirst = false;
                if (_pStart != _pEnd)
                {
                    SerachDataPoint(_pStart, _pEnd);
                }
            }
        }

        private void SerachDataPoint(PointF pStart, PointF pEnd)
        {
            snaplist = new List<SnapshotDto>();
            float minX;
            float minY;
            float maxX;
            float maxY;
            if (pStart.X < pEnd.X)
            {
                minX = pStart.X;
                maxX = pEnd.X;
            }
            else
            {
                minX = pEnd.X;
                maxX = pStart.X;
            }
            if (pStart.Y < pEnd.Y)
            {
                minY = pStart.Y;
                maxY = pEnd.Y;
            }
            else
            {
                minY = pEnd.Y;
                maxY = pStart.Y;
            }

            foreach (Line line in tChart1.Chart.Series)
            {
                for (int i = 0; i < line.Count; i++)
                {
                    if (line.CalcXPos(i)>=minX&&line.CalcXPos(i)<maxX&&line.CalcYPos(i)>=minY&&line.CalcYPos(i)<=maxY)
                    {
                        SnapshotDto dto = new SnapshotDto();
                        //dto.SQL_ID = ((System.Data.DataTable)line.DataSource).TableName; //snap_id
                        dto.PARAMENT_NAME = ((System.Data.DataTable)line.DataSource).TableName; //snap_id
                        //double value = line[i].Y;//VALUE
                        //dto.Value = line[i].Y.ToString();//value
                        dto.PARAMENT_VALUE = line[i].Y.ToString();//value
                        int xValue = Convert.ToInt32(line[i].X);//ROWNUM

                        
                        DataTable dt1 = line.DataSource as DataTable;
                        dto.SNAP_ID = dt1.Rows[i+1]["SNAP_ID"].ToString();//SQL_ID
                        dto.END_INTERVAL_TIME = (DateTime)dt1.Rows[i + 1]["END_INTERVAL_TIME"];
                        snaplist.Add(dto);
                    }
                }
            }
            if (!snaplist.Any())
            {
                return;
            }


            this._DataTable = DataTableExtend.ConvertToDataSet<SnapshotDto>(snaplist).Tables[0];

            PopupGrid popupGrid = new PopupGrid(this._DataTable);
            popupGrid.StartPosition = FormStartPosition.CenterScreen;
            popupGrid.ShowDialog();
            DataTable dt =  popupGrid._DataTable;

            if (popupGrid.linkage == true)
            {
                base.OpenUI("SQLFULLTEXTQUERYTREND", "TREND", "SQLFullTextQueryTrend", dt);
            }
            
        }

        //TChart chart = new TChart();
        private void tChart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left )
            {
                _pStart.X = (float)e.X;
                _pStart.Y = (float)e.Y;

                bfirst = true;
            }



        }




        private void CreateTeeChart(DataTable dsTable)
        {
            tChart1.Refresh();
            tChart1.Series.Clear();
            
            /*var cuTool = new CursorTool(tChart1.Chart)
            {
                Style = CursorToolStyles.Both,
                FollowMouse = true,

            };*/
            var markstip = new MarksTip(tChart1.Chart);
            /* cuTool.Pen.Color = Color.Red;
             tChart1.MouseEnter += Chart_MouseEnter;
             tChart1.MouseLeave += Chart_MouseLeave;
             void Chart_MouseEnter(object sender, EventArgs e)
             {
                 cuTool.Pen.Visible = true;
             }
             void Chart_MouseLeave(object sender, EventArgs e)
             {
                 cuTool.Pen.Visible = false;
             }*/

            //chart.Chart.Series.Chart.GetASeries().Legend.Text.ToString();
            //tChart1.ContextMenuStrip = contextMenuStrip1;
            
            tChart1.Dock = DockStyle.Fill;
            tChart1.Legend.LegendStyle = LegendStyles.Series;//Legend显示样式以Series名字显示
            tChart1.Header.Text = "TEECHART";//teechart标题 
            tChart1.Legend.Visible = true;
            IEnumerable<IGrouping<string, DataRow>> result = dsTable.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["PARAMENT_NAME"].ToString());
            if (result != null && result.Count() > 0)
            {
                foreach (IGrouping<string, DataRow> rows in result)
                {
                    DataTable dataTable = rows.ToArray().CopyToDataTable();
                    dataTable.TableName = rows.Key;
                    if (dataTable.Rows.Count > 0)
                    {
                        dataSet1.Tables.Add(dataTable);
                    }
                }
            }
            if (dataSet1.Tables.Count > 1)
            {
                int i = 0;
                foreach (DataTable dt in dataSet1.Tables)
                {
                    if (dt.TableName != "TABLE")
                    {
                        
                        i++;
                        Line line = CreateLine(dt,i);
                        series.Add(line);
                        tChart1.Series.Add(line);
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
            
            tChart1.Axes.Bottom.Labels.DateTimeFormat = "MM-dd HH:MI";
            tChart1.Axes.Bottom.Labels.ExactDateTime = true;//x轴显示横坐标为时间
            /*line.Legend.Visible = true;*/
            return;
        }



        private Line CreateLine(DataTable dstable ,int i)
        {
            Line line = new Line();
            /*var nearpoint = new NearestPoint(chart.Chart) {
                Series = line,
                Style = NearestPointStyles.None,
                Direction = NearestPointDirection.Both,
                Size = 1
            };
            nearpoint.Pen.Color = Color.Red;*/
            //nearpoint.Pen.Visible = false;
            /*line.Pointer.Style = PointerStyles.Circle;
            line.Pointer.Visible = true;*/
            //line.Pointer.HorizSize = 120;

            //line.ClickPointer += Line_ClickPointer;
            //line.GetSeriesMark += Line_GetSeriesMark;
            void Line_GetSeriesMark(Series series, GetSeriesMarkEventArgs e)
            {
                e.MarkText = "PARAMENT_NAME :" + $"{dstable.Rows[e.ValueIndex]["PARAMENT_NAME"]}" + "\r\n" + "VALUE :" + $"{dstable.Rows[e.ValueIndex]["PARAMENT_VALUE"]}" + "\r\n" + "TIME :" + $"{ dstable.Rows[e.ValueIndex]["END_INTERVAL_TIME"]}";
            }
            line.DataSource = dstable;
            line.YValues.DataMember = "PARAMENT_VALUE";
            line.XValues.DataMember = "END_INTERVAL_TIME";
            line.Legend.Visible = true;
            line.Color = GetRandomColor(i);


            line.Legend.Text = dstable.TableName;
            line.Legend.BorderRound = 10;
            line.XValues.DateTime = true;
            line.GetSeriesMark += Line_GetSeriesMark;
            return line;
        }

        public Color GetRandomColor(int i )
        {
             DataTable da = new DataTable();
            da.Columns.Add("R",typeof(Int32));
            da.Columns.Add("G",typeof(Int32));
            da.Columns.Add("B",typeof(Int32));

            da.Rows.Add(52, 152, 219);
            da.Rows.Add(26, 188, 156);
            da.Rows.Add(249, 202, 36);
            da.Rows.Add(10, 189, 227);
            da.Rows.Add(197, 108, 240);
            da.Rows.Add(30, 144, 255);
            da.Rows.Add(255, 247, 153);
            da.Rows.Add(255, 177, 66);
            da.Rows.Add(238, 255, 66);
            da.Rows.Add(177, 213, 200);
            da.Rows.Add(250, 192, 61);
            da.Rows.Add(255, 168, 1);
            da.Rows.Add(204, 174, 98);
            da.Rows.Add(84, 109, 229);
            da.Rows.Add(210, 57, 24);
            da.Rows.Add(56, 103, 214);
            da.Rows.Add(225, 95, 65);
            da.Rows.Add(184, 53, 112);
            da.Rows.Add(230, 0, 18);
            da.Rows.Add(93, 163, 157);
            da.Rows.Add(95, 39, 205);
            da.Rows.Add(83, 61, 205);
            da.Rows.Add(51, 205, 64);
            da.Rows.Add(205, 159, 23);
            da.Rows.Add(109, 173, 205);
            da.Rows.Add(39, 205, 86);
            da.Rows.Add(205, 51, 72);
            da.Rows.Add(57, 190, 205);
            da.Rows.Add(45, 205, 64);
            da.Rows.Add(49, 35, 205);
            da.Rows.Add(203, 59, 205);
            da.Rows.Add(45, 149, 214);
            da.Rows.Add(214, 200, 41);

            return Color.FromArgb((Int32)da.Rows[i]["R"], (Int32)da.Rows[i]["G"], (Int32)da.Rows[i]["B"]);
        }


        private void editChartToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            tChart1.ShowEditor();
        }
    }
}
