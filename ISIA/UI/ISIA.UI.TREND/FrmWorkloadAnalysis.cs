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

namespace ISIA.UI.TREND
{
    public partial class FrmWorkloadAnalysis : DockUIBase1T1
    {
        TChart currentChart = null;

        protected PointF _pStart;
        protected PointF _pEnd;

       
        private bool bfirst = false;
        EquipmentArgsPack args = new EquipmentArgsPack();
        BizDataClient bs;
        DataSet dataSet;
        List<Series> series = new List<Series>();
        List<WorkloadDto> workloadList = new List<WorkloadDto>();
        public FrmWorkloadAnalysis()
        {
            InitializeComponent();
            new InitializationUIService(this, null, new AwrArgsPack()).Run();
        }

        private int SeriesIndex = -1;

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                SearchUiService searchUiService = new SearchUiService(this, e, new AwrArgsPack());
                searchUiService.RunAsync();
            }
            catch (Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(TAP.UI.EnumMsgType.CONFIRM, ex.Message);
            }
        }

       


        private void tCheckComboBoxParmType_EditValueChanged(object sender, EventArgs e)
        {


        }



        private void editChartToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            currentChart.ShowEditor();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            currentChart = (sender as ContextMenuStrip).SourceControl as TChart;
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

            foreach (Line line in tChart1.Chart.Series)
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
                        dto.DbName = comboBoxDBName.Text;
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
            DataTable dtForNext = frm.ResultForNextPageDt;
            ht.Add("dt", dtForNext);
            ht.Add("startTime", frm.args.StartTime);
            ht.Add("endTime", frm.args.EndTime);
            ht.Add("workloadParm", frm.args.WorkloadSqlParm);

            base.OpenUI("WORKLOADSQLCORRELATIONANALYSIS", "ANALYSIS", "Sql Parm Correlation Analysis", null, ht);



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
    }
}
