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
using Steema.TeeChart.Styles;
using Series = Steema.TeeChart.Styles.Series;
using Steema.TeeChart.Tools;
using System.Xml;
using System.Xml.Linq;
using EnumDataObject = TAP.UI.EnumDataObject;
using Analysis.Correlation;
using DevExpress.XtraCharts.Heatmap;

namespace ISIA.UI.ANALYSIS
{
    public partial class FrmCorrelationAnalysis : DockUIBase1T1
    {
        public FrmCorrelationAnalysis()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.ANALYSIS.DLL", "ISIA.BIZ.ANALYSIS.CorrelationAnalysis");
            bs1 = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.OrclParmsTrendChart");
            InitControls();
            this.dockManager1.AutoHideSpeed = 10000;
            this.dockManager1.AutoHiddenPanelShowMode = DevExpress.XtraBars.Docking.AutoHiddenPanelShowMode.MouseHover;
        }

        #region Field 
        BizDataClient bs;
        BizDataClient bs1;
        ComboBoxControl ComboBoxControl = new ComboBoxControl();
        DataSet dsTrend;
        DataSet dsHeat;
        List<string> listHeap = null;
        DataTable dtMetric;
        MonitorArgsPack args = new MonitorArgsPack();
        MonitorArgsPack argsType = new MonitorArgsPack();
        string[] data;
        TChart currentChart = null;
        private int SeriesIndex = -1;
        Line currentLine = null;
        DataSet dsParmType;
        DataSet dsDB;
        DataSet dsParm;
        #endregion

        #region Method
        public DataSet LoadData()
        {
            AwrArgsPack EventArgPack = new AwrArgsPack();
            try
            {

                /*var listDB = this.cboDB.Properties.Items.GetCheckedValues();
                var DATABASE = string.Join(",", listDB);*/

                var listPrmt = this.cboPrmt.Properties.Items.GetCheckedValues();
                var Params = string.Join(",", listPrmt);


                List<object> paramList = cboPrmt.Properties.Items.GetCheckedValues();
                if (paramList == null || paramList.Count <= 0)
                {
                    string errMessage = "Please select Param Names";
                    throw new Exception(errMessage);
                }
                EventArgPack.ParamNamesList = paramList;

                //time argument checked
                var startTime = this.dateStart.EditValue;
                var endTime = dateEnd.EditValue;
                if (startTime == null || endTime == null)
                {
                    string errMessage = "Please select StartTime or EndTime";
                    throw new Exception(errMessage);
                }
                DateTime startDateTime = (DateTime)dateStart.EditValue;
                DateTime endDateTime = (DateTime)dateEnd.EditValue;

                if (startDateTime > endDateTime)
                {
                    EventArgPack.StartTime = endDateTime.ToString("yyyyMMdd");
                    EventArgPack.EndTime = startDateTime.ToString("yyyyMMdd");

                }
                EventArgPack.StartTime = startDateTime.ToString("yyyyMMdd");
                EventArgPack.EndTime = endDateTime.ToString("yyyyMMdd");
                EventArgPack.GroupingDateFormat = "yyyyMMdd";

                //xasix_interval check
                if (rgType.SelectedIndex == 1)
                {
                    EventArgPack.StartTime = EventArgPack.StartTime + "00";
                    EventArgPack.EndTime = EventArgPack.EndTime + "23";
                    EventArgPack.GroupingDateFormat = "yyyyMMddHH24";
                }
                else if (rgType.SelectedIndex == 2)
                {
                    EventArgPack.StartTime = EventArgPack.StartTime + "000";
                    EventArgPack.EndTime = EventArgPack.EndTime + "235";
                    EventArgPack.GroupingDateFormat = "yyyyMMddHH24mi";
                }
                EventArgPack.DBName = cmbDbName.Text.Split('(')[0];

                dsTrend = bs.ExecuteDataSet("GetParmDailyTrendData", EventArgPack.getPack());

                DataTable dtTemp = MergeTable(dsTrend);
                dsHeat = new DataSet();
                dsHeat.Tables.Add(dtTemp);

                listHeap = Params.Split(',').ToList<string>();
                return dsHeat;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateHeatMap(DataSet ds, List<string> lists)
        {
            Config config = new Config(ds);
            CorrelationExecuter ex = CorrelationAnalysisExecuterFactory.GetDefaultExecuter(config);
            ex.Execute();

            HeatmapMatrixAdapter dataAdapter = new HeatmapMatrixAdapter();
            dataAdapter.XArguments = ex.AxisX.Keys.ToArray();
            dataAdapter.YArguments = ex.AxisY.Keys.ToArray();
            dataAdapter.Values = ex.DataMap;
            heatmap.DataAdapter = dataAdapter;

            Palette palette = new Palette("Custom") {

                Color.DarkBlue,
                Color.White,
                Color.Red

            };

            HeatmapRangeColorProvider colorProvider = new HeatmapRangeColorProvider()
            {
                Palette = palette,
                ApproximateColors = true
            };

            colorProvider.RangeStops.Add(new HeatmapRangeStop(-1, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(-0.1, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(-0.2, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(-0.3, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(-0.4, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(-0.5, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(-0.6, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(-0.7, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(-0.8, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(-0.9, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(0, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(0.1, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(0.2, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(0.3, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(0.4, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(0.5, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(0.6, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(0.7, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(0.8, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(0.9, HeatmapRangeStopType.Absolute));
            colorProvider.RangeStops.Add(new HeatmapRangeStop(1, HeatmapRangeStopType.Absolute));

            heatmap.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            heatmap.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            heatmap.Label.Visible = true;

            //启用热图工具提示
            heatmap.ToolTipEnabled = true;
            heatmap.ToolTipController = new DevExpress.Utils.ToolTipController { AllowHtmlText = true };
            heatmap.CustomizeHeatmapToolTip += OnHeatmapCustomizeHeatmapToolTip;

            //缩放，滚轮功能开启
            heatmap.EnableAxisXScrolling = true;
            heatmap.EnableAxisYScrolling = true;
            heatmap.EnableAxisXZooming = true;
            heatmap.EnableAxisYZooming = true;

            //X轴下面显示
            heatmap.AxisX.Alignment = HeatmapAxisLabelAlignment.Near;
            heatmap.AxisX.Name = "axisX";
            heatmap.AxisX.Visibility = DevExpress.Utils.DefaultBoolean.True;
            heatmap.AxisX.Title.Text = "X";
            heatmap.AxisX.AutoGrid = false;
            //上下错开显示
            // heatmap.AxisX.Label.Staggered = true;

            //Y轴左边显示
            heatmap.AxisY.Alignment = HeatmapAxisLabelAlignment.Near;
            heatmap.AxisY.Name = "axisY";
            heatmap.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.True;
            heatmap.AxisY.Title.Text = "Y";
            heatmap.AxisY.Reverse = true;

            //设置宽度和高度
            heatmap.Width = 600;
            heatmap.Height = 350;
            heatmap.ColorProvider = colorProvider;

            //heatmap.Titles.Add(new HeatmapTitle { Text = "correlation demo" });
            heatmap.Label.Visible = false;
        }

        private void OnHeatmapCustomizeHeatmapToolTip(object sender, CustomizeHeatmapToolTipEventArgs e)
        {
            double cellValue = (double)e.HeatmapCell.ColorValue;

            e.Title = " ";
            e.Text = $"value: <color=green>{cellValue}</color>";
        }
        public DataTable MergeTable(DataSet ds)
        {
            DataTable dtReturn = new DataTable();
            //合并后台的数据表
            foreach (DataTable dt in ds.Tables)
            {
                DataColumn col = new DataColumn(dt.TableName, typeof(double));
                dtReturn.Columns.Add(col);
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow drNew = dtReturn.NewRow();
                foreach (DataColumn coltmp in dtReturn.Columns)
                {
                    drNew[coltmp.ColumnName] = ds.Tables[coltmp.ColumnName].Rows[i][coltmp.ColumnName];
                }
                dtReturn.Rows.Add(drNew);
            }
            return dtReturn;
        }
        public void DisplayData(DataSet ds)
        {
            if (dsHeat == null)
            {
                return;
            }

            CreateHeatMap(dsHeat, listHeap);
        }
        private MonitorArgsPack GetXmlElement(string key)
        {
            MonitorArgsPack monitorArg = new MonitorArgsPack();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@".\ISIA.config");
                XmlNodeList nodeList = doc.SelectNodes("configuration/TAP.ISIA.Configuration/WX/Shift");
                foreach (XmlNode node in nodeList)
                {
                    monitorArg.StartTime = node[key].Attributes["StartTime"].Value;
                    monitorArg.EndTime = node[key].Attributes["EndTime"].Value;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return monitorArg;
        }

        private string GetDateTimeFormat()
        {
            var ret = string.Empty;
            switch (rgType.SelectedIndex)
            {
                case 0:
                    ret = "MM-dd";
                    break;
                case 1:
                    ret = "MM-dd-HH";
                    break;
                case 2:
                    ret = "MM-dd-HH:mm";
                    break;
                default:
                    break;
            }
            return ret;
        }

        private void InitControls()
        {
            AwrArgsPack awrArg = new AwrArgsPack();
            /*this.cboDB.Properties.Items.Clear();
            DataSet dsDB = bs1.ExecuteDataSet("GetDBName", awrArg.getPack());
            foreach (DataRow dr in dsDB.Tables[0].Rows)
            {
                this.cboDB.Properties.Items.Add(dr["name"]);
            }*/


            this.cboPrmtType.Properties.Items.Clear();
            DataSet dsParmType = bs1.ExecuteDataSet("GetParmType", awrArg.getPack());
            foreach (DataRow dr in dsParmType.Tables[0].Rows)
            {
                cboPrmtType.Properties.Items.Add(dr["parametertype"]);
            }


            this.cboShift.SelectedIndex = 0;
            MonitorArgsPack argsTimePack = new MonitorArgsPack();
            argsTimePack = GetXmlElement("A");
            try
            {
                if (argsTimePack != null)
                {
                    string startTime = argsTimePack.StartTime;
                    string endTime = argsTimePack.EndTime;

                    this.dateStart.DateTime = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + " " + startTime);
                    this.dateEnd.DateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + endTime);
                }

                this.cboPrmt.Properties.PopupFormSize = new Size(400, 600);
            }
            catch (Exception)
            {
                throw;
            }           
        }
        #endregion


        #region Event
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                ComboBoxControl.SetCrossLang(this._translator);
                if (!base.ValidateUserInput(this.layoutControl1)) return;
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            }
            catch (Exception)
            {
                throw;
            }
        }
     
        private void rgType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dsTrend == null || dsTrend.Tables.Count == 0)
            {
                return;
            }

        }

        private void cboShift_SelectedValueChanged(object sender, EventArgs e)
        {
            MonitorArgsPack argsTimePack = new MonitorArgsPack();
            var shift = this.cboShift.Text;
            if (string.IsNullOrWhiteSpace(shift))
            {
                return;
            }
            argsTimePack = GetXmlElement(shift);
            //if (argsTimePack != null)
            //{
            //    string startTime = argsTimePack.StartTime;
            //    string endTime = argsTimePack.EndTime;

            //    this.dateStart.DateTime = Convert.ToDateTime(this.dateStart.DateTime.ToString("yyyy-MM-dd") + " " + startTime);
            //    this.dateEnd.DateTime = Convert.ToDateTime(this.dateEnd.DateTime.ToString("yyyy-MM-dd") + " " + endTime);
            //}
        }

        private void cboPrmtType_EditValueChanged(object sender, EventArgs e)
        {
            AwrArgsPack awrArg = new AwrArgsPack();
            awrArg.ParamType = this.cboPrmtType.Text.Trim();
            this.cboPrmt.Properties.Items.Clear();
            dsParmType = bs1.ExecuteDataSet("GetParmNameByType", awrArg.getPack());
            foreach (DataRow dr in dsParmType.Tables[0].Rows)
            {
                this.cboPrmt.Properties.Items.Add(dr["parametername"]);
            }
        }

        #endregion
    }
}
