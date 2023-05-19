using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.UI;
using ISIA.UI.BASE;
using DevExpress.XtraEditors.Controls;
using TAP.UIControls.BasicControlsDEV;
using Steema.TeeChart;
using Steema.TeeChart.Tools;
using Steema.TeeChart.Styles;
using System.Threading;
using ISIA.UI.TREND.UIService.UIServiceImpl.Trend.UI.FrmWorkload;
using Series = Steema.TeeChart.Styles.Series;
using ISIA.COMMON;
using ISIA.UI.TREND.Dto;
using System.Collections;
using EnumDataObject = TAP.UI.EnumDataObject;

namespace ISIA.UI.TREND
{
    public partial class FrmWorkloadTrendChart : DockUIBase1T1
    {
        

        protected PointF _pStart;
        protected PointF _pEnd;

       
        private bool bfirst = false;
        AwrArgsPack args = new AwrArgsPack();
        BizDataClient bs;
        DataSet dataSet = null;
        object[] result = new object[2];
        List<Series> series = new List<Series>();
        List<WorkloadDto> workloadList = new List<WorkloadDto>();
        public FrmWorkloadTrendChart()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.WorkloadTrendChart");

            dtpStartTime.DateTime = DateTime.Now.AddDays(-1);
            dtpEndTime.DateTime = DateTime.Now;
            //new InitializationUIService(this, null, new AwrArgsPack()).Run();
        }



        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {

                if (!base.ValidateUserInput(this.lcSerachOptions)) return;

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
                
                
                
                //args.DbId = args.DbId.Substring(0, args.DbId.Length - 1);
                args.DBName = string.IsNullOrEmpty(cmbDbName.Text) ? "" : cmbDbName.Text.Split('(')[0];
                args.StartTime = dtpStartTime.DateTime.ToString("yyyyMMddHHmmss");
                args.EndTime = dtpEndTime.DateTime.ToString("yyyyMMddHHmmss");
                args.GroupingDateFormat = "yyyyMMdd";
                dataSet = bs.ExecuteDataSet("GetWorkloadDataByParams", args.getPack());

                
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
            ConvertData(dataSet);
            CreateTeeChart();
            gridviewStyle();
        }


        private void CreateTeeChart() {


            this.splitContainerControl1.Panel1.Controls.Clear();
            //this.tChart1.Zoom.Direction = ZoomDirections.None;
            TChart tChart1 = new TChart();

            this.splitContainerControl1.Panel1.Controls.Add(tChart1);

            
            tChart1.Dock = DockStyle.Fill;
            //Header set
            //this.tChart1.Header.Text = "WORKLOAD";
            //Legend set
            tChart1.Legend.LegendStyle = LegendStyles.Series;
            tChart1.Legend.Visible = true;
            //this.tChart1.Legend.CheckBoxes = true;

            

            DataTable[] tables = (DataTable[])result[0];
            //XAXIS MULTILINE CONTROL
            tChart1.Axes.Bottom.Labels.MultiLine = true;
            //tool tip
            MarksTip marksTip = new MarksTip(tChart1.Chart);
            marksTip.Active = true;
            marksTip.MouseDelay = 100;
            marksTip.MouseAction = MarksTipMouseAction.Move;
            //marksTip.Style = MarksStyles.XY;
            //tChart1.MouseMove += tChart_MouseMove;
            /*marksTip.GetText += new MarksTipGetTextEventHandler(marksTip_GetText);*/
            //Color color = new Color();

            foreach (DataTable dt in tables)
            {
                //color.getRandomColor();
                Line line = CreateLine(dt);
                tChart1.Series.Add(line);
                tChart1.Axes.Bottom.Labels.DateTimeFormat = "yyyyMMdd hh:mm";
                tChart1.Axes.Bottom.Labels.ExactDateTime = true;
                tChart1.Axes.Bottom.Ticks.Width = 0;
            }
            //this.splitContainerControl1.Panel1.Controls.Add(this.tChart1);

            tChart1.MouseDown += tChart1_MouseDown;
            tChart1.MouseUp += tChart1_MouseUp;



        }


        public void  ConvertData(DataSet data)
        {
            DataSet ds = data;
            //object[] result = new object[2];
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
            /*line.Color =
                System.Drawing.Color.FromArgb(color.R1, color.G1, color.S1);*/
            line.Legend.BorderRound = 20;
            line.XValues.DateTime = true;
            line.GetSeriesMark += Line_GetSeriesMark;


            void Line_GetSeriesMark(Series series, GetSeriesMarkEventArgs e)
            {
                e.MarkText = "PARAMENT_NAME :" + $"{dt.Columns[0].ColumnName}" + "\r\n" + "VALUE :" + $"{dt.Rows[e.ValueIndex][0]}" + "\r\n" + "TIME :" + $"{ dt.Rows[e.ValueIndex][1]}";
            }
            return line;
        }

        private void gridviewStyle() {

            
            //grid display
            this.gridControlWorkloadData.DataSource = null;
            this.gridView1.Columns.Clear();
            this.gridControlWorkloadData.DataSource = (DataTable)result[1];
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.Columns[4].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridView1.Columns[4].DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gridView1.Columns[5].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridView1.Columns[5].DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gridView1.BestFitColumns();
        }

        private void tCheckComboBoxParmType_EditValueChanged(object sender, EventArgs e)
        {


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
                    SerachDataPoint(_pStart, _pEnd, sender as TChart);
                }
            }
        }

        private void SerachDataPoint(PointF pStart, PointF pEnd , TChart chart)
        {
            workloadList = new List<WorkloadDto>();
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

            foreach (Line line in chart.Series)
            {
                for (int i = 0; i < line.Count; i++)
                {
                    if (line.CalcXPos(i) >= minX && line.CalcXPos(i) < maxX && line.CalcYPos(i) >= minY && line.CalcYPos(i) <= maxY)
                    {
                        WorkloadDto dto = new WorkloadDto();
                        dto.WorkloadParm = ((System.Data.DataTable)line.DataSource).TableName; //snap_id
                        //double value = line[i].Y;//VALUE
                        dto.WorkloadValue = line[i].Y.ToString();//value
                        int xValue = Convert.ToInt32(line[i].X);//ROWNUM
                        dto.Time = DateTime.FromBinary((long)line[i].X);
                        DataTable dt1 = line.DataSource as DataTable;
                        dto.Time = (DateTime)dt1.Rows[i + 1]["BEGIN_TIME"];//SQL_ID
                        dto.DbName = cmbDbName.Text.Split('(')[0];
                        workloadList.Add(dto);
                    }
                }
            }
            if (!workloadList.Any())
            {
                return;
            }
            Hashtable ht = new Hashtable();
            this._DataTable = DataTableExtend.ConvertToDataSet<WorkloadDto>(workloadList).Tables[0];
            FrmWorkloadDataGridView frm = new FrmWorkloadDataGridView(_DataTable);
            frm.ShowDialog();

            //tmpUIDefaultInfo = new UIBasicDefaultInfo();
            //tmpUIDefaultInfo.Region = InfoBase._USER_INFO.Region;
            //tmpUIDefaultInfo.Facility = InfoBase._USER_INFO.Facility;

            DataTable dtForNext = frm.ResultForNextPageDt;
            if (frm.args == null)
            {
                return;
            }
            ht.Add("dt", dtForNext);
            ht.Add("startTime", frm.args.StartTime);
            ht.Add("endTime", frm.args.EndTime);
            ht.Add("workloadParm", frm.args.WorkloadSqlParm);
            ht.Add("DbName", frm.args.DBName);
            base.OpenUI("WORKLOADSQLCORRELATIONANALYSIS", "AWR", "Correlation between Workload and SQL", ht);



        }

        private void tChart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _pStart.X = (float)e.X;
                _pStart.Y = (float)e.Y;

                bfirst = true;
            }



        }


        private void comboBoxEditGroupUnit_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBoxEditGroupUnit.Text == "DAY")
            {
                this.dtpStartTime.DateTime = DateTime.Now.AddDays(-1);
                this.dtpEndTime.DateTime = DateTime.Now;
            }
            else if (comboBoxEditGroupUnit.Text == "WEEK")
            {
                this.dtpStartTime.DateTime = DateTime.Now.AddDays(-7);
                this.dtpEndTime.DateTime = DateTime.Now;
            }
            else if (comboBoxEditGroupUnit.Text == "MONTH")
            {
                this.dtpStartTime.DateTime = DateTime.Now.AddMonths(-1);
                this.dtpEndTime.DateTime = DateTime.Now;
            }
            else if (comboBoxEditGroupUnit.Text == "QUARTER")
            {
                this.dtpStartTime.DateTime = DateTime.Now.AddMonths(-3);
                this.dtpEndTime.DateTime = DateTime.Now;
            }
            else if (comboBoxEditGroupUnit.Text == "YEAR")
            {
                this.dtpStartTime.DateTime = DateTime.Now.AddYears(-1);
                this.dtpEndTime.DateTime = DateTime.Now;
            }
        }
    }
}
