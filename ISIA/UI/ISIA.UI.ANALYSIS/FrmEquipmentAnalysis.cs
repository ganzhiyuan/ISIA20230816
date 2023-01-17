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

namespace ISIA.UI.ANALYSIS
{
    public partial class FrmEquipmentAnalysis : DockUIBase1T1
    {
        public FrmEquipmentAnalysis()
        {
            InitializeComponent();

            bs = new BizDataClient("ISEM.BIZ.REPORT.DLL", "ISEM.BIZ.REPORT.EquipmentLineProductionCapacity");
            InitializeDate();
        }

        #region Field 

        BizDataClient bs;
        CommonArgsPack args = new CommonArgsPack();
        DataSet dataSet;
        DataTable dtLinePlan;
        DataTable dtBMinfo;
        DataTable dt_eqinfo;
        DataTable dtLineSum;
        DataTable table = new DataTable();
        DataTable dataTableMonth = new DataTable();
        ComboBoxControl ComboBoxControl = new ComboBoxControl();
        DataTable mindata = new DataTable();
        int shift = 0;
        #endregion

        #region Method
        private void InitializeDate()
        {
            DateTime dt = Convert.ToDateTime((DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " 07:30"));

            this.dateStart.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.dateStart.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateStart.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.dateStart.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateStart.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm";
            this.dateStart.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.dateStart.Properties.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;

            this.dateEnd.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.dateEnd.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEnd.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.dateEnd.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEnd.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm";
            this.dateEnd.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.dateEnd.Properties.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;

            dateStart.DateTime = dt.AddDays(-1);
            dateEnd.DateTime = dt;
            toolTipController.Appearance.Font = new Font("Courier New", 9);
        }
        public DataSet LoadData()
        {
            try
            {
                string FAB = cboFab.Text;
                string Line = cboLine.Text;

                args.DateTimeStart = dateStart.DateTime.AddDays(-1).ToString("yyyyMMdd").Substring(0, 8);
                args.DateTimeEnd = dateEnd.DateTime.ToString("yyyyMMdd").Substring(0, 8);
                args.Facility = FAB;
                args.Line = Line;
                dataSet = bs.ExecuteDataSet("GetEquipmentProduction", args.getPack());
                dtLinePlan = bs.ExecuteDataSet("GetLineCapacityPlan", args.getPack()).Tables[0];
                dtBMinfo = bs.ExecuteDataSet("GetEQBMInfo", args.getPack()).Tables[0];
                dt_eqinfo = bs.ExecuteDataSet("GetEquipmentCapacity").Tables[0];
                dtLineSum = bs.ExecuteDataSet("GetLineCapacitySum", args.getPack()).Tables[0];

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
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();
            xtraTabPage1.Controls.Clear();
            xtraTabPage2.Controls.Clear();

            table = FilterDNTable(dataSet.Tables[0]);
            DataTable data = LineChartTable(table);
            gridControl1.DataSource = data;
            CreateChart(data);
            GridViewStyle(gridView1);

        }

        private DataTable BMTable(DataTable BMtable)
        {
            DataTable bmDt = new DataTable();
            bmDt.Columns.Add("EQUIPMENT", typeof(string));
            bmDt.Columns.Add("EQUIPMENTTYPE", typeof(string));
            bmDt.Columns.Add("PROCESS", typeof(string));
            bmDt.Columns.Add("BMCODE", typeof(string));
            bmDt.Columns.Add("BMTIMECOUNT(min)", typeof(int));
            bmDt.Columns.Add("LOSSTIME(min)", typeof(int));
            bmDt.Columns.Add("LOSS", typeof(int));
            for (int i = 0; i < BMtable.Rows.Count; i++)
            {
                string EQ = BMtable.Rows[i]["EQUIPMENT"].ToString();
                string EQTYPE = BMtable.Rows[i]["EQUIPMENTTYPE"].ToString();
                string PROCESS = BMtable.Rows[i]["PROCESS"].ToString();
                string BMCODE = BMtable.Rows[i]["BMCODE"].ToString();
                TimeSpan time = GetBmTime(BMtable.Rows[i]["BMWAITTIME"].ToString(), BMtable.Rows[i]["BMSTARTTIME"].ToString(), BMtable.Rows[i]["BMENDTIME"].ToString());
                string bmtimemin = ((int)Math.Round(time.TotalMinutes, 0)).ToString();
                string lossTime;
                string loss;
                if (BMtable.Rows[i]["PROLOSS"].ToString() != "YES")
                {
                    loss = "0";
                    lossTime = "0";
                }
                else
                {
                    DataRow[] drs = dt_eqinfo.Select(string.Format("NAME = '{0}'", BMtable.Rows[i]["EQUIPMENT"].ToString()));
                    if (drs.Length > 0)
                    {
                        int capacity = Convert.ToInt32(string.IsNullOrEmpty(drs[0]["CAPACITY"].ToString()) ? "0" : drs[0]["CAPACITY"].ToString());
                        int tubecount = Convert.ToInt32(string.IsNullOrEmpty(BMtable.Rows[i]["TUBECOUNT"].ToString()) ? "1" : BMtable.Rows[i]["TUBECOUNT"].ToString());

                        string tube = BMtable.Rows[i]["TUBE"].ToString();
                        if (string.IsNullOrEmpty(tube) || tube.Contains("ALL"))
                        {
                            BMtable.Rows[i]["TUBE"] = tubecount;
                        }
                        else
                        {
                            if (tube.Contains("EQ1") || tube.Contains("EQ2") || tube.Contains("A") || tube.Contains("B"))
                            {
                                if (tube.Contains("EQ1") && tube.Contains("EQ2"))
                                {
                                    BMtable.Rows[i]["TUBE"] = tubecount;
                                }
                                else if (tube.Contains("A") && tube.Contains("B"))
                                {
                                    BMtable.Rows[i]["TUBE"] = tubecount;
                                }
                                else
                                {
                                    BMtable.Rows[i]["TUBE"] = (Convert.ToInt32(tubecount) / 2) + (Convert.ToInt32(tube.Split(',').Length.ToString()) - 1);
                                }
                            }
                            else
                            {
                                BMtable.Rows[i]["TUBE"] = tube.Split(',').Length.ToString();
                            }
                        }
                        tube = BMtable.Rows[i]["TUBE"].ToString();

                        loss = ((capacity / 60) * Convert.ToInt32(bmtimemin) * Convert.ToInt32(tube) / Convert.ToInt32(tubecount)).ToString();
                        lossTime = (Convert.ToInt32(bmtimemin) * Convert.ToInt32(tube) / tubecount).ToString();
                    }
                    else
                    {
                        loss = "0";
                        lossTime = "0";
                    }
                }
                bmDt.Rows.Add(EQ, EQTYPE, PROCESS, BMCODE, bmtimemin, lossTime, loss);
            }
            return bmDt;
        }

        private TimeSpan GetBmTime(string wait, string start, string end)
        {
            TimeSpan timeSpan = TimeSpan.Zero;
            TimeSpan endtimeSpan = TimeSpan.Zero;
            DateTime waitTime = DateTime.MinValue;
            DateTime startTime = DateTime.MinValue;
            DateTime endTime = DateTime.MinValue;
            waitTime = OutTime(wait, waitTime);
            startTime = OutTime(start, startTime);
            endTime = OutTime(end, endTime);

            if (waitTime == DateTime.MinValue && startTime != DateTime.MinValue)
            {
                endtimeSpan = (endTime - startTime);
            }
            else
            if (startTime == DateTime.MinValue && waitTime != DateTime.MinValue)
            {
                endtimeSpan = (endTime - waitTime);
            }
            else if (startTime != DateTime.MinValue && waitTime != DateTime.MinValue)
            {
                endtimeSpan = (endTime - waitTime);
            }
            timeSpan = endtimeSpan;
            return timeSpan;
            //  return ((double)Math.Round(timeSpan.TotalMinutes, 0)).ToString();
        }
        public static DataTable DtSelectTop(int TopItem, DataTable oDT)
        {
            if (oDT.Rows.Count < TopItem) return oDT;

            DataTable NewTable = oDT.Clone();
            DataRow[] rows = oDT.Select("1=1");
            for (int i = 0; i < TopItem; i++)
            {
                NewTable.ImportRow((DataRow)rows[i]);
            }
            return NewTable;
        }
        private DateTime OutTime(string strTime, DateTime dateTime)
        {
            if (!string.IsNullOrEmpty(strTime))
            {
                DateTime.TryParse(
                            strTime.Substring(0, 4) + "-" +
                            strTime.Substring(4, 2) + "-" +
                            strTime.Substring(6, 2) + " " +
                            strTime.Substring(8, 2) + ":" +
                            strTime.Substring(10, 2) + ":" +
                            strTime.Substring(12, 2), out dateTime);
            }
            return dateTime;

        }

        private DataTable FilterDNTable(DataTable table)
        {
            int startDay = dateStart.DateTime.Day;
            int startHour = dateStart.DateTime.Hour;
            int startMinute = dateStart.DateTime.Minute;
            int endDay = dateEnd.DateTime.Day;
            int endHour = dateEnd.DateTime.Hour;
            int endMinute = dateEnd.DateTime.Minute;

            DataRow[] rows = table.Select("DATE LIKE '" + dateEnd.DateTime.ToString("yyyyMMdd") + "%' AND SHIFT= 'N'");
            foreach (DataRow item in rows)
            {
                table.Rows.Remove(item);
                item.Delete();
            }
            if (new TimeSpan(startDay, startHour, startMinute, 0) < new TimeSpan(startDay, 7, 30, 0))
            {
                DataRow[] drs = table.Select("DATE LIKE '" + dateStart.DateTime.AddDays(-1).ToString("yyyyMMdd") + "%' AND SHIFT= 'D'");
                foreach (DataRow item in drs)
                {
                    table.Rows.Remove(item);
                    item.Delete();
                }
            }
            else if (new TimeSpan(startDay, 7, 30, 0) <= new TimeSpan(startDay, startHour, startMinute, 0) && new TimeSpan(startDay, startHour, startMinute, 0) < new TimeSpan(startDay, 19, 30, 0))
            {
                DataRow[] drs = table.Select("DATE LIKE '" + dateStart.DateTime.AddDays(-1).ToString("yyyyMMdd") + "%'");
                foreach (DataRow item in drs)
                {
                    table.Rows.Remove(item);
                    item.Delete();
                }
            }
            else if (new TimeSpan(startDay, 19, 30, 0) <= new TimeSpan(startDay, startHour, startMinute, 0))
            {
                DataRow[] drs = table.Select("DATE LIKE '" + dateStart.DateTime.ToString("yyyyMMdd") + "%' AND SHIFT= 'D'");
                DataRow[] drs1 = table.Select("DATE LIKE '" + dateStart.DateTime.AddDays(-1).ToString("yyyyMMdd") + "%'");
                foreach (DataRow item in drs)
                {
                    table.Rows.Remove(item);
                    item.Delete();
                }
                foreach (DataRow item in drs1)
                {
                    table.Rows.Remove(item);
                    item.Delete();
                }
            }
            if (new TimeSpan(endDay, 19, 30, 0) > new TimeSpan(endDay, endHour, endMinute, 0) && new TimeSpan(endDay, 7, 30, 0) <= new TimeSpan(endDay, endHour, endMinute, 0))
            {
                DataRow[] drs = table.Select("DATE LIKE '" + dateEnd.DateTime.ToString("yyyyMMdd") + "%' AND SHIFT= 'D'");
                foreach (DataRow item in drs)
                {
                    table.Rows.Remove(item);
                    item.Delete();
                }
            }
            else if (new TimeSpan(endDay, 7, 30, 0) > new TimeSpan(endDay, endHour, endMinute, 0))
            {
                DataRow[] drs = table.Select("DATE LIKE '" + dateEnd.DateTime.AddDays(-1).ToString("yyyyMMdd") + "%' AND SHIFT= 'N'");
                DataRow[] drs1 = table.Select("DATE LIKE '" + dateEnd.DateTime.ToString("yyyyMMdd") + "%' AND SHIFT= 'D'");
                foreach (DataRow item in drs)
                {
                    table.Rows.Remove(item);
                    item.Delete();
                }
                foreach (DataRow item in drs1)
                {
                    table.Rows.Remove(item);
                    item.Delete();
                }
            }

            shift = table.DefaultView.ToTable(true, new string[] { "SHIFT", "DATE" }).Rows.Count;
            return table;

        }


        private DataTable LineChartTable(DataTable table)
        {
            DataTable chartData = new DataTable();
            chartData.Columns.Add("AREA", typeof(string));
            chartData.Columns.Add("PROCESS", typeof(string));
            chartData.Columns.Add("EQUIPMENTTYPE", typeof(string));
            chartData.Columns.Add("SHIFT", typeof(string));
            chartData.Columns.Add("QUANTITY", typeof(int));
            chartData.Columns.Add("RUNTIME", typeof(string));
            chartData.Columns.Add("EQPLAN", typeof(int));

            List<string> listArea = new List<string> { "WI", "TX-A-上", "TX-M", "TX-A-下", "DF-M", "LD", "PS-A", "PS-M", "OX-M", "RP-M", "PE-M", "LC", "SP", "TS" };
            List<string> listPro = new List<string> { "WI", "TX", "TX", "TX", "DF", "LD", "PS", "PS", "OX", "RP", "PE", "LC", "SP", "TS" };
            List<string> listType = new List<string> { "Q", "A", "M", "A", "M", "M", "A", "M", "M", "M", "M", "M", "M", "Q" };
            List<string> listShift = new List<string> { "每个shift", "每个shift", "每个shift", "每个shift", "每个shift", "每个shift", "每个shift", "每个shift", "每个shift", "每个shift", "每个shift", "每个shift", "每个小时", "每个shift" };
            //TimeSpan timeSpan = dateEnd.DateTime - dateStart.DateTime;

            for (int i = 0; i < 14; i++)
            {
                chartData.Rows.Add(listArea[i].ToString(), listPro[i].ToString(), listType[i].ToString(), listShift[i].ToString());
            }
            for (int i = 0; i < chartData.Rows.Count; i++)
            {
                string AREA = chartData.Rows[i]["AREA"].ToString();
                string PROCESS = chartData.Rows[i]["PROCESS"].ToString();
                string EQUIPMENTTYPE = chartData.Rows[i]["EQUIPMENTTYPE"].ToString();
                string SHIFT = chartData.Rows[i]["SHIFT"].ToString();
                if (AREA.ToUpper().Contains("TX-A") && EQUIPMENTTYPE == "A")
                {
                    //n%2=0为偶数，n%2<>0为奇数
                    string sql = string.Format("EQUIPMENTTYPE ='{0}' AND PROCESS ='{1}'", EQUIPMENTTYPE, PROCESS);
                    table.DefaultView.RowFilter = sql;
                    dtLineSum.DefaultView.RowFilter = sql;
                    DataTable FilterDt = table.DefaultView.ToTable();
                    DataTable dtCPSSum = dtLineSum.DefaultView.ToTable();
                    FilterDt.Columns.Add("NUMBER", typeof(int));
                    dtCPSSum.Columns.Add("NUMBER", typeof(int));
                    for (int j = 0; j < FilterDt.Rows.Count; j++)
                    {
                        string equipment = FilterDt.Rows[j]["EQUIPMENT"].ToString();
                        FilterDt.Rows[j]["NUMBER"] = equipment.Substring(equipment.Length - 2, 2);
                    }
                    for (int j = 0; j < dtCPSSum.Rows.Count; j++)
                    {
                        string equipment = dtCPSSum.Rows[j]["EQUIPMENT"].ToString();
                        dtCPSSum.Rows[j]["NUMBER"] = equipment.Substring(equipment.Length - 2, 2);
                    }
                    if (AREA == "TX-A-上")
                    {
                        FilterDt.DefaultView.RowFilter = "NUMBER%2<>0";
                        dtCPSSum.DefaultView.RowFilter = "NUMBER%2<>0";
                    }
                    else
                    {
                        FilterDt.DefaultView.RowFilter = "NUMBER%2=0";
                        dtCPSSum.DefaultView.RowFilter = "NUMBER%2=0";
                    }
                    FilterDt = FilterDt.DefaultView.ToTable();
                    dtCPSSum = dtCPSSum.DefaultView.ToTable();
                    double sum = FilterDt.AsEnumerable().Select(d => Convert.ToDouble(string.IsNullOrEmpty(d.Field<string>("CNT")) ? "0" : d.Field<string>("CNT"))).Sum();
                    // double sumCPS =( dtCPSSum.AsEnumerable().Select(d => Convert.ToDouble(d.Field<string>("CPS")) ? "0" : d.Field<string>("CPS"]).Sum())* shift;
                    double sumCPS = (dtCPSSum.AsEnumerable().Select(d => Convert.ToDouble(d["CPS"] == DBNull.Value ? 0 : d["CPS"])).Sum()) * shift;
                    chartData.Rows[i]["QUANTITY"] = sum;
                    chartData.Rows[i]["EQPLAN"] = sumCPS;
                    chartData.Rows[i]["RUNTIME"] = sum != 0 && sumCPS != 0 ? Math.Round(sum / sumCPS, 2) : 0;
                    table.DefaultView.RowFilter = string.Empty;
                }
                else if (EQUIPMENTTYPE == "A")
                {
                    string sql = string.Format("EQUIPMENTTYPE ='{0}' AND PROCESS ='{1}'", EQUIPMENTTYPE, PROCESS);
                    table.DefaultView.RowFilter = sql;
                    dtLineSum.DefaultView.RowFilter = sql;
                    DataTable FilterDt = table.DefaultView.ToTable();
                    DataTable dtCPSSum = dtLineSum.DefaultView.ToTable();
                    double sum = FilterDt.AsEnumerable().Select(d => Convert.ToDouble(string.IsNullOrEmpty(d.Field<string>("CNT")) ? "0" : d.Field<string>("CNT"))).Sum();
                    // double sumCPS = (dtCPSSum.AsEnumerable().Select(d => Convert.ToDouble(string.IsNullOrEmpty(d.Field<string>("CPS")) ? "0" : d.Field<string>("CPS"))).Sum()) * shift;
                    double sumCPS = (dtCPSSum.AsEnumerable().Select(d => Convert.ToDouble(d["CPS"] == DBNull.Value ? 0 : d["CPS"])).Sum()) * shift;
                    chartData.Rows[i]["QUANTITY"] = sum;
                    chartData.Rows[i]["EQPLAN"] = sumCPS;
                    chartData.Rows[i]["RUNTIME"] = sum != 0 && sumCPS != 0 ? Math.Round(sum / sumCPS, 2) : 0;

                    table.DefaultView.RowFilter = string.Empty;
                }
                else
                {
                    //状态为其他（除A外）
                    if (PROCESS.Contains("DF") || PROCESS.Contains("OX") || PROCESS.Contains("PE") || PROCESS.Contains("RP"))
                    {
                        string sql = string.Format("EQUIPMENTTYPE ='A' AND PROCESS ='{0}'", PROCESS);
                        string sqlcps = string.Format("EQUIPMENTTYPE ='{0}' AND PROCESS ='{1}'", EQUIPMENTTYPE, PROCESS);
                        table.DefaultView.RowFilter = sql;
                        dtLineSum.DefaultView.RowFilter = sqlcps;
                        DataTable FilterDt = table.DefaultView.ToTable();
                        DataTable dtCPSSum = dtLineSum.DefaultView.ToTable();
                        double sum = FilterDt.AsEnumerable().Select(d => Convert.ToDouble(string.IsNullOrEmpty(d.Field<string>("CNT")) ? "0" : d.Field<string>("CNT"))).Sum();
                        //double sumCPS = (dtCPSSum.AsEnumerable().Select(d => Convert.ToDouble(string.IsNullOrEmpty(d.Field<string>("CPS")) ? "0" : d.Field<string>("CPS"))).Sum()) * shift;
                        double sumCPS = (dtCPSSum.AsEnumerable().Select(d => Convert.ToDouble(d["CPS"] == DBNull.Value ? 0 : d["CPS"])).Sum()) * shift;
                        chartData.Rows[i]["QUANTITY"] = sum;
                        chartData.Rows[i]["EQPLAN"] = sumCPS;
                        chartData.Rows[i]["RUNTIME"] = sum != 0 && sumCPS != 0 ? Math.Round(sum / sumCPS, 2) : 0;

                        table.DefaultView.RowFilter = string.Empty;
                    }
                    else
                    {
                        string sql = string.Format("EQUIPMENTTYPE ='{0}' AND PROCESS ='{1}'", EQUIPMENTTYPE, PROCESS);
                        table.DefaultView.RowFilter = sql;
                        dtLineSum.DefaultView.RowFilter = sql;
                        DataTable FilterDt = table.DefaultView.ToTable();
                        DataTable dtCPSSum = dtLineSum.DefaultView.ToTable();
                        double sum = FilterDt.AsEnumerable().Select(d => Convert.ToDouble(string.IsNullOrEmpty(d.Field<string>("CNT")) ? "0" : d.Field<string>("CNT"))).Sum();
                        //double sumCPS = (dtCPSSum.AsEnumerable().Select(d => Convert.ToDouble(string.IsNullOrEmpty(d.Field<string>("CPS")) ? "0" : d.Field<string>("CPS"))).Sum()) * shift;
                        double sumCPS = (dtCPSSum.AsEnumerable().Select(d => Convert.ToDouble(d["CPS"] == DBNull.Value ? 0 : d["CPS"])).Sum()) * shift;
                        chartData.Rows[i]["QUANTITY"] = sum;
                        chartData.Rows[i]["EQPLAN"] = sumCPS;
                        chartData.Rows[i]["RUNTIME"] = sum != 0 && sumCPS != 0 ? Math.Round(sum / sumCPS, 2) : 0;
                        table.DefaultView.RowFilter = string.Empty;
                    }
                }
            }
            return chartData;
        }
        private void ProcessChartTable(DataRow dataRow)
        {
            string process = dataRow["PROCESS"].ToString();
            string AREA = dataRow["AREA"].ToString();
            string EQUIPMENTTYPE = dataRow["EQUIPMENTTYPE"].ToString();
            string sql;
            DataTable data = new DataTable();
            if (EQUIPMENTTYPE.Contains("M"))
            {
                if (process.Contains("DF") || process.Contains("OX") || process.Contains("PE") || process.Contains("RP"))
                {
                    sql = string.Format("PROCESS ='{0}' AND EQUIPMENTTYPE='A'", process);
                }
                else
                {
                    sql = string.Format("PROCESS ='{0}' AND EQUIPMENTTYPE='{1}'", process, EQUIPMENTTYPE);
                }
            }
            else
            {
                sql = string.Format("PROCESS ='{0}' AND EQUIPMENTTYPE='{1}'", process, EQUIPMENTTYPE);
            }
            table.DefaultView.RowFilter = sql;
            DataTable FilterDt = table.DefaultView.ToTable();
            FilterDt.Columns.Add("DATETIME", typeof(string));
            FilterDt.Columns.Add("NUMBER", typeof(int));
            for (int j = 0; j < FilterDt.Rows.Count; j++)
            {
                string equipment = FilterDt.Rows[j]["EQUIPMENT"].ToString();
                FilterDt.Rows[j]["NUMBER"] = equipment.Substring(equipment.Length - 2, 2);
            }
            if (AREA == "SP" && EQUIPMENTTYPE == "M")
            {
                FilterDt.DefaultView.RowFilter = sql;
                FilterDt = FilterDt.DefaultView.ToTable();
                data = FilterDt;

                data.Columns.Add("CPHVALUE", typeof(string));
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    string equipment = FilterDt.Rows[i]["DATE"].ToString().Substring(4, 2) + "/" + data.Rows[i]["DATE"].ToString().Substring(6, 2) + " " + data.Rows[i]["DATE"].ToString().Substring(8, 2);
                    data.Rows[i]["DATETIME"] = equipment;

                    double cnt = string.IsNullOrEmpty(data.Rows[i]["CNT"].ToString()) ? 0 : Convert.ToDouble(data.Rows[i]["CNT"].ToString());
                    double cph = string.IsNullOrEmpty(data.Rows[i]["CPH"].ToString()) ? 0 : Convert.ToDouble(data.Rows[i]["CPH"].ToString());
                    data.Rows[i]["CPHVALUE"] = cph != 0 && cnt != 0 ? Math.Round(cnt / cph, 2) : 0;
                }
            }
            else
            {
                if (AREA.ToUpper().Contains("TX-A") && EQUIPMENTTYPE == "A")
                {
                    if (AREA == "TX-A-上")
                    {
                        FilterDt.DefaultView.RowFilter = "NUMBER%2<>0";
                        FilterDt = FilterDt.DefaultView.ToTable();
                    }
                    else
                    {
                        FilterDt.DefaultView.RowFilter = "NUMBER%2=0";
                        FilterDt = FilterDt.DefaultView.ToTable();
                    }
                    FilterDt.DefaultView.RowFilter = sql;
                    FilterDt = FilterDt.DefaultView.ToTable();
                    data = CreateSummaryProcess(FilterDt);
                    //data = FilterDt;
                }
                else
                {
                    table.DefaultView.RowFilter = sql;
                    FilterDt = table.DefaultView.ToTable();
                    table.DefaultView.RowFilter = sql;
                    data = CreateSummaryProcess(FilterDt);
                    //data = FilterDt;
                    // data.Columns.Add("DATETIME", typeof(string));

                }
                data.Columns.Add("CPHVALUE", typeof(string));
                if (EQUIPMENTTYPE.Contains("M"))
                {
                    if (process.Contains("DF") || process.Contains("OX") || process.Contains("PE") || process.Contains("RP"))
                    {
                        string sqlcps = string.Format("EQUIPMENTTYPE ='{0}' AND PROCESS ='{1}'", EQUIPMENTTYPE, process);
                        dtLineSum.DefaultView.RowFilter = sqlcps;
                        DataTable dtCPSSum = dtLineSum.DefaultView.ToTable();
                        for (int i = 0; i < data.Rows.Count; i++)
                        {
                            string equipment = data.Rows[i]["DATETIME"].ToString().Substring(4, 2) + "/" + data.Rows[i]["DATETIME"].ToString().Substring(6, 2) + "-" + data.Rows[i]["SHIFT"].ToString();
                            data.Rows[i]["DATETIME"] = equipment;

                            double cnt = string.IsNullOrEmpty(data.Rows[i]["CNT"].ToString()) ? 0 : Convert.ToDouble(data.Rows[i]["CNT"].ToString());
                            double sumCPS = (dtCPSSum.AsEnumerable().Select(d => Convert.ToDouble(d["CPS"] == DBNull.Value ? 0 : d["CPS"])).Sum()) * shift;
                            data.Rows[i]["CPHVALUE"] = sumCPS != 0 && cnt != 0 ? Math.Round(cnt / sumCPS, 2) : 0;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < data.Rows.Count; i++)
                        {
                            string equipment = data.Rows[i]["DATETIME"].ToString().Substring(4, 2) + "/" + data.Rows[i]["DATETIME"].ToString().Substring(6, 2) + "-" + data.Rows[i]["SHIFT"].ToString();
                            data.Rows[i]["DATETIME"] = equipment;

                            double cnt = string.IsNullOrEmpty(data.Rows[i]["CNT"].ToString()) ? 0 : Convert.ToDouble(data.Rows[i]["CNT"].ToString());
                            double cph = string.IsNullOrEmpty(data.Rows[i]["CPS"].ToString()) ? 0 : Convert.ToDouble(data.Rows[i]["CPS"].ToString());
                            data.Rows[i]["CPHVALUE"] = cph != 0 && cnt != 0 ? Math.Round(cnt / cph, 2) : 0;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string equipment = data.Rows[i]["DATETIME"].ToString().Substring(4, 2) + "/" + data.Rows[i]["DATETIME"].ToString().Substring(6, 2) + "-" + data.Rows[i]["SHIFT"].ToString();
                        data.Rows[i]["DATETIME"] = equipment;

                        double cnt = string.IsNullOrEmpty(data.Rows[i]["CNT"].ToString()) ? 0 : Convert.ToDouble(data.Rows[i]["CNT"].ToString());
                        double cph = string.IsNullOrEmpty(data.Rows[i]["CPS"].ToString()) ? 0 : Convert.ToDouble(data.Rows[i]["CPS"].ToString());
                        data.Rows[i]["CPHVALUE"] = cph != 0 && cnt != 0 ? Math.Round(cnt / cph, 2) : 0;
                    }
                }
            }
            CreateProcessChart(data);
        }

        private DataTable CreateSummaryProcess(DataTable dt)
        {
            DataTable tmpdt = new DataTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["CNT"] = string.IsNullOrEmpty(dt.Rows[i]["CNT"].ToString()) ? 0 : Convert.ToInt32(dt.Rows[i]["CNT"].ToString());
                dt.Rows[i]["CPS"] = string.IsNullOrEmpty(dt.Rows[i]["CPS"].ToString()) ? 0 : Convert.ToInt32(dt.Rows[i]["CPS"].ToString());
            }

            var dtGroupBy = dt.AsEnumerable().GroupBy(r => new { PROCESS = r.Field<string>("PROCESS"), DATETIME = r.Field<string>("DATE"), SHIFT = r.Field<string>("SHIFT") }).Select(
                               g => new
                               {
                                   PROCESS = g.Key.PROCESS,
                                   //WORKSHOP = g.Key.WORKSHOP,
                                   DATETIME = g.Key.DATETIME,
                                   SHIFT = g.Key.SHIFT,
                                   CNT = g.Sum(r => Convert.ToInt32(r.Field<string>("CNT"))),
                                   CPS = g.Sum(r => r.Field<int>("CPS")),
                               });

            tmpdt = AsDataTable(dtGroupBy);
            return tmpdt;
        }

        public static DataTable AsDataTable<T>(IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
        private void CreateChart(DataTable dsTable)
        {
            dsTable.DefaultView.Sort = "EQPLAN  ASC";
            DataTable ascdt = dsTable.DefaultView.ToTable();
            mindata = ascdt.AsEnumerable().Take(2).CopyToDataTable();
            dsTable.DefaultView.Sort = "";

            xtraTabPage1.Controls.Clear();

            chartControl1.Series.Clear();
            chartControl1.Dock = DockStyle.Fill;
            chartControl1.RuntimeHitTesting = true;
            xtraTabPage1.Controls.Add(chartControl1);
            //添加 图表标题
            chartControl1.Titles.Clear();
            chartControl1.Titles.Add(new ChartTitle());
            chartControl1.Titles[0].Text = "";

            DataTable dataBar = new DataTable();

            dataBar.Columns.Add("RUNTIME", typeof(string));
            dataBar.Columns.Add("PROCESS", typeof(string));
            dataBar.Columns.Add("VALUE", typeof(int));

            DataTable dataLine = new DataTable();

            dataLine.Columns.Add("RUNTIME", typeof(string));
            dataLine.Columns.Add("PROCESS", typeof(string));
            dataLine.Columns.Add("VALUE", typeof(double));

            DataTable dataLinePlan = new DataTable();

            dataLinePlan.Columns.Add("RUNTIME", typeof(string));
            dataLinePlan.Columns.Add("PROCESS", typeof(string));
            dataLinePlan.Columns.Add("VALUE", typeof(int));
            DataTable filterDT_DN = FilterDNTable(dtLinePlan);
            int capacity = filterDT_DN.AsEnumerable().Select(d => Convert.ToInt32(string.IsNullOrEmpty(d.Field<string>("CAPACITY")) ? "0" : d.Field<string>("CAPACITY"))).Sum();

            for (int i = 0; i < dsTable.Rows.Count; i++)
            {
                dataLine.Rows.Add(new object[] { "RunTime", dsTable.Rows[i]["AREA"], dsTable.Rows[i]["RUNTIME"] });
            }
            for (int i = 0; i < dsTable.Rows.Count; i++)
            {
                dataBar.Rows.Add(new object[] { "RunTime", dsTable.Rows[i]["AREA"], dsTable.Rows[i]["QUANTITY"] });
                dataLinePlan.Rows.Add(new object[] { "RunTime", dsTable.Rows[i]["AREA"], capacity });
            }
            Series seriesBar = new Series("OutPut", ViewType.Bar);
            Series seriesPoint = new Series("RunTime", ViewType.Point);
            Series seriesLinePlan = new Series("Target", ViewType.Line);
            seriesBar.DataSource = dataBar;
            seriesPoint.DataSource = dataLine;
            seriesLinePlan.DataSource = dataLinePlan;

            seriesBar.ArgumentDataMember = "PROCESS";
            seriesBar.ValueDataMembers.AddRange(new string[] { "Value" });
            seriesBar.ValueScaleType = ScaleType.Numerical;
            seriesBar.View.Color = Color.Green;
            seriesBar.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

            seriesPoint.ArgumentDataMember = "PROCESS";
            seriesPoint.ValueDataMembers.AddRange(new string[] { "Value" });
            seriesPoint.ValueScaleType = ScaleType.Numerical;
            seriesPoint.View.Color = Color.Red;
            seriesPoint.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            seriesPoint.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;//用百分比表示
            seriesPoint.PointOptions.ValueNumericOptions.Precision = 2;

            seriesLinePlan.ArgumentDataMember = "PROCESS";
            seriesLinePlan.ValueDataMembers.AddRange(new string[] { "Value" });
            seriesLinePlan.ValueScaleType = ScaleType.Numerical;
            seriesLinePlan.View.Color = Color.Black;
            seriesLinePlan.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            ((LineSeriesView)seriesLinePlan.View).LineStyle.DashStyle = DashStyle.Dash;
            seriesLinePlan.Label.Font = new Font("宋体", 7f);

            chartControl1.Series.Add(seriesBar);
            chartControl1.Series.Add(seriesPoint);
            chartControl1.Series.Add(seriesLinePlan);

            SecondaryAxisY myAxis = new SecondaryAxisY(seriesPoint.Name);
            ((XYDiagram)chartControl1.Diagram).SecondaryAxesY.Clear();
            ((XYDiagram)chartControl1.Diagram).SecondaryAxesY.Add(myAxis);
            ((PointSeriesView)seriesPoint.View).AxisY = myAxis;
            myAxis.Title.Font = new Font("宋体", 9.0f);
            myAxis.Label.TextPattern = "{V:0.00%}";

            Color color = seriesPoint.View.Color;//设置坐标的颜色和图表线条颜色一致
            myAxis.Title.TextColor = color;
            myAxis.Label.TextColor = color;
            myAxis.Color = color;

            XYDiagram diagram = (XYDiagram)chartControl1.Diagram;
            //显示X轴的全部标签
            diagram.AxisX.QualitativeScaleOptions.AutoGrid = false;
            diagram.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            diagram.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            diagram.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;

            SideBySideBarSeriesLabel label = chartControl1.Series[0].Label as SideBySideBarSeriesLabel;
            if (label != null)
            {
                label.Position = BarSeriesLabelPosition.BottomInside;
            }

            xtraTabPage1.Controls.Add(chartControl1);
            this.chartControl1.BoundDataChanged += new DevExpress.XtraCharts.BoundDataChangedEventHandler(this.chartControl1_BoundDataChanged);

        }
        private void CreateProcessChart(DataTable dsTable)
        {
            xtraTabPage2.Controls.Clear();
            chartControl2.Series.Clear();
            chartControl2.Dock = DockStyle.Fill;
            chartControl2.RuntimeHitTesting = true;
            xtraTabPage2.Controls.Add(chartControl2);
            //添加 图表标题
            chartControl2.Titles.Clear();
            chartControl2.Titles.Add(new ChartTitle());
            if (dsTable.Rows.Count > 0)
            {
                chartControl2.Titles[0].Text = dsTable.Rows[0]["PROCESS"].ToString();
            }
            else
            {
                chartControl2.Titles[0].Text = "";
            }

            DataTable dataBar = new DataTable();

            dataBar.Columns.Add("RUNTIME", typeof(string));
            dataBar.Columns.Add("DATE", typeof(string));
            dataBar.Columns.Add("VALUE", typeof(double));

            DataTable dataLine = new DataTable();

            dataLine.Columns.Add("RUNTIME", typeof(string));
            dataLine.Columns.Add("DATE", typeof(string));
            dataLine.Columns.Add("VALUE", typeof(int));
            for (int i = 0; i < dsTable.Rows.Count; i++)
            {
                dataLine.Rows.Add(new object[] { "RunTime", dsTable.Rows[i]["DATETIME"], dsTable.Rows[i]["CNT"] });
            }
            for (int i = 0; i < dsTable.Rows.Count; i++)
            {
                dataBar.Rows.Add(new object[] { "RunTime", dsTable.Rows[i]["DATETIME"], dsTable.Rows[i]["CPHVALUE"] });
            }
            Series seriesPoint = new Series("RunTime", ViewType.Point);
            Series seriesBar = new Series("OutPut", ViewType.Bar);
            seriesPoint.DataSource = dataBar;
            seriesBar.DataSource = dataLine;

            seriesPoint.ArgumentDataMember = "DATE";
            seriesPoint.ValueDataMembers.AddRange(new string[] { "VALUE" });
            seriesPoint.ValueScaleType = ScaleType.Numerical;
            seriesPoint.View.Color = Color.Red;
            seriesPoint.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            seriesPoint.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;//用百分比表示
            seriesPoint.PointOptions.ValueNumericOptions.Precision = 2;

            seriesBar.ArgumentDataMember = "DATE";
            seriesBar.ValueDataMembers.AddRange(new string[] { "VALUE" });
            seriesBar.ValueScaleType = ScaleType.Numerical;
            seriesBar.View.Color = Color.Green;
            seriesBar.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

            chartControl2.Series.Add(seriesBar);
            chartControl2.Series.Add(seriesPoint);

            SecondaryAxisY myAxis = new SecondaryAxisY(seriesPoint.Name);
            ((XYDiagram)chartControl2.Diagram).SecondaryAxesY.Clear();
            ((XYDiagram)chartControl2.Diagram).SecondaryAxesY.Add(myAxis);
            ((PointSeriesView)seriesPoint.View).AxisY = myAxis;
            myAxis.Title.Font = new Font("宋体", 9.0f);
            myAxis.Label.TextPattern = "{V:0.00%}";

            Color color = seriesPoint.View.Color;//设置坐标的颜色和图表线条颜色一致
            myAxis.Title.TextColor = color;
            myAxis.Label.TextColor = color;
            myAxis.Color = color;

            XYDiagram diagram = (XYDiagram)chartControl2.Diagram;
            //显示X轴的全部标签
            diagram.AxisX.QualitativeScaleOptions.AutoGrid = false;
            diagram.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
            diagram.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
            diagram.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;

            SideBySideBarSeriesLabel label = chartControl2.Series[0].Label as SideBySideBarSeriesLabel;
            if (label != null)
            {
                label.Position = BarSeriesLabelPosition.BottomInside;
            }
            xtraTabPage2.Controls.Add(chartControl2);
        }

        public System.Collections.IList GetGridViewFilteredAndSortedData(DevExpress.XtraGrid.Views.Grid.GridView view)
        {
            return view.DataController.GetAllFilteredAndSortedRows();
        }
        public void ExportToExcel(string title, params IPrintable[] panels)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = title;
            saveFileDialog.Title = "导出Excel";
            saveFileDialog.Filter = "Excel文件(*.xlsx)|*.xlsx|Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.Cancel)
                return;
            string FileName = saveFileDialog.FileName;
            PrintingSystem ps = new PrintingSystem();
            CompositeLink link = new CompositeLink(ps);
            ps.Links.Add(link);
            foreach (IPrintable panel in panels)
            {
                link.Links.Add(CreatePrintableLink(panel));
            }
            link.Landscape = true;//横向           
            link.BreakSpace = 100;
            //判断是否有标题，有则设置         
            //link.CreateDocument(); //建立文档
            try
            {
                int count = 1;
                //在重复名称后加（序号）
                while (File.Exists(FileName))
                {
                    if (FileName.Contains(")."))
                    {
                        int start = FileName.LastIndexOf("(");
                        int end = FileName.LastIndexOf(").") - FileName.LastIndexOf("(") + 2;
                        FileName = FileName.Replace(FileName.Substring(start, end), string.Format("({0}).", count));
                    }
                    else
                    {
                        FileName = FileName.Replace(".", string.Format("({0}).", count));
                    }
                    count++;
                }
                if (FileName.LastIndexOf(".xlsx") >= FileName.Length - 5)
                {
                    XlsxExportOptions options = new XlsxExportOptions();
                    link.ExportToXlsx(FileName, options);
                }
                else
                {
                    XlsExportOptions options = new XlsExportOptions();
                    link.ExportToXls(FileName, options);
                }
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.INFORMATION, "Export successfully.");
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 创建打印Componet
        /// </summary>
        /// <param name="printable"></param>
        /// <returns></returns>
        PrintableComponentLink CreatePrintableLink(IPrintable printable)
        {
            ChartControl chart = printable as ChartControl;
            if (chart != null)
                chart.OptionsPrint.SizeMode = DevExpress.XtraCharts.Printing.PrintSizeMode.Zoom;
            PrintableComponentLink printableLink = new PrintableComponentLink() { Component = printable };
            return printableLink;
        }

        public void GridViewStyle(GridView gridView)
        {
            gridView.OptionsBehavior.Editable = false;
            //gridView.OptionsView.ColumnAutoWidth = false;
            //for (int i = 0; i < gridView1.Columns.Count; i++)
            //{
            //    this.gridView1.BestFitColumns();
            //    this.gridView1.Columns[i].BestFit();//自动列宽
            //}
            gridView.BestFitColumns();
            gridView.OptionsCustomization.AllowColumnMoving = false;
            gridView.OptionsCustomization.AllowSort = true;
            gridView.OptionsCustomization.AllowColumnResizing = false;
            if (gridView.Columns.Contains(gridView.Columns.ColumnByFieldName("RUNTIME")))
                gridView.Columns["RUNTIME"].Visible = false;
            if (gridView.Columns.Contains(gridView.Columns.ColumnByFieldName("EQPLAN")))
                gridView.Columns["EQPLAN"].Visible = false;
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
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            DataTable dt = gridControl1.DataSource as DataTable;
            DataTable dataTable = dt.Clone();
            var dataRowViews1 = GetGridViewFilteredAndSortedData(gridView1);
            foreach (DataRowView item in dataRowViews1)
            {
                dataTable.Rows.Add(item.Row.ItemArray);
            }

            DataRow currentDr = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);

            if (currentDr == null)
            {
                return;
            }

            ProcessChartTable(currentDr);
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridControl1.DataSource == null || (gridControl1.DataSource as DataTable).Rows.Count == 0)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Please Search Data");
                    return;
                }
                else
                {
                    ExportToExcel("", gridControl1);
                }
            }
            catch
            {
            }

        }

        #endregion
        ToolTipController toolTipController = new ToolTipController();

        private void chartControl1_MouseMove(object sender, MouseEventArgs e)
        {
            ChartHitInfo hitInfo = chartControl1.CalcHitInfo(e.Location);
            if (hitInfo.InSeries)
            {
                if (((Series)hitInfo.Series).Name.ToString() == "OutPut")
                {
                    StringBuilder builder = new StringBuilder();
                    if (hitInfo.InSeriesPoint)
                    {
                        string mouseX = hitInfo.SeriesPoint.Argument;
                        string[] mouseXs = mouseX.Split('-');
                        if (mouseXs.Length == 2)
                        {
                            string eqArea = mouseXs[0];
                            string eqType = mouseXs[1];
                            string filterStr = string.Format("PROCESS = '{0}' AND [EQUIPMENTTYPE] = '{1}' AND  [BMTIMECOUNT(min)] >=60", eqArea, eqType);
                            DataTable dtResult = BMTable(dtBMinfo);
                            dtResult.DefaultView.RowFilter = filterStr;
                            dtResult.DefaultView.Sort = "LOSS DESC";
                            DataTable dtResultFilter = dtResult.DefaultView.ToTable();
                            dtResultFilter = DtSelectTop(5, dtResultFilter);
                            builder.AppendLine(dtResultFilter.Columns["EQUIPMENT"].ColumnName.ToString().PadRight(15, ' ') + dtResultFilter.Columns["BMTIMECOUNT(min)"].ColumnName.PadRight(20, ' ') + dtResultFilter.Columns["LOSSTIME(min)"].ColumnName.ToString().PadRight(20, ' ') + dtResultFilter.Columns["LOSS"].ColumnName.ToString().PadRight(10, ' ') + dtResultFilter.Columns["BMCODE"].ColumnName.ToString().PadRight(45, ' '));//
                            for (int i = 0; i < dtResultFilter.Rows.Count; i++)
                            {
                                builder.AppendLine(dtResultFilter.Rows[i]["EQUIPMENT"].ToString().PadRight(15, ' ') + dtResultFilter.Rows[i]["BMTIMECOUNT(min)"].ToString().PadRight(20, ' ') + dtResultFilter.Rows[i]["LOSSTIME(min)"].ToString().PadRight(20, ' ') + dtResultFilter.Rows[i]["LOSS"].ToString().PadRight(10, ' ') + dtResultFilter.Rows[i]["BMCODE"].ToString().PadRight(45, ' '));
                            }
                        }
                        else if (mouseXs.Length == 1)
                        {
                            string eqArea = mouseXs[0];
                            string eqType = "";
                            if (mouseX == "WI" || mouseX == "TS")
                            {
                                eqType = "Q";
                            }
                            else if (mouseX == "LD" || mouseX == "LD" || mouseX == "SP")
                            {
                                eqType = "M";
                            }
                            else
                            {
                                eqType = "A";
                            }

                            string filterStr = string.Format("PROCESS = '{0}' AND [EQUIPMENTTYPE] = '{1}' AND  [BMTIMECOUNT(min)] >=60", eqArea, eqType);
                            DataTable dtResult = BMTable(dtBMinfo);
                            dtResult.DefaultView.RowFilter = filterStr;
                            dtResult.DefaultView.Sort = "LOSS DESC";
                            DataTable dtResultFilter = dtResult.DefaultView.ToTable();
                            dtResultFilter = DtSelectTop(5, dtResultFilter);
                            builder.AppendLine(dtResultFilter.Columns["EQUIPMENT"].ColumnName.ToString().PadRight(15, ' ') + dtResultFilter.Columns["BMTIMECOUNT(min)"].ColumnName.PadRight(20, ' ') + dtResultFilter.Columns["LOSSTIME(min)"].ColumnName.ToString().PadRight(20, ' ') + dtResultFilter.Columns["LOSS"].ColumnName.ToString().PadRight(10, ' ') + dtResultFilter.Columns["BMCODE"].ColumnName.ToString().PadRight(45, ' '));//
                            for (int i = 0; i < dtResultFilter.Rows.Count; i++)
                            {
                                builder.AppendLine(dtResultFilter.Rows[i]["EQUIPMENT"].ToString().PadRight(15, ' ') + dtResultFilter.Rows[i]["BMTIMECOUNT(min)"].ToString().PadRight(20, ' ') + dtResultFilter.Rows[i]["LOSSTIME(min)"].ToString().PadRight(20, ' ') + dtResultFilter.Rows[i]["LOSS"].ToString().PadRight(10, ' ') + dtResultFilter.Rows[i]["BMCODE"].ToString().PadRight(45, ' '));
                            }

                        }
                        else if (mouseXs.Length == 3)
                        {
                            string eqArea = mouseXs[0];
                            string eqType = mouseXs[1];
                            string filterStr = string.Format("PROCESS = '{0}' AND [EQUIPMENTTYPE] = '{1}' AND  [BMTIMECOUNT(min)] >=60", eqArea, eqType);
                            DataTable dtResult = BMTable(dtBMinfo);
                            dtResult.DefaultView.RowFilter = filterStr;

                            DataTable dtResultFilter = dtResult.DefaultView.ToTable();
                            dtResultFilter.Columns.Add("NUMBER", typeof(int));
                            for (int i = 0; i < dtResultFilter.Rows.Count; i++)
                            {
                                string equipment = dtResultFilter.Rows[i]["EQUIPMENT"].ToString();
                                dtResultFilter.Rows[i]["NUMBER"] = equipment.Substring(equipment.Length - 2, 2);
                            }
                            dtResultFilter.DefaultView.RowFilter = mouseX == "TX-A-上" ? "NUMBER%2<>0" : "NUMBER%2=0";
                            dtResultFilter.DefaultView.Sort = "LOSS DESC";
                            dtResultFilter = dtResultFilter.DefaultView.ToTable();

                            dtResultFilter = DtSelectTop(5, dtResultFilter);
                            builder.AppendLine(dtResultFilter.Columns["EQUIPMENT"].ColumnName.ToString().PadRight(15, ' ') + dtResultFilter.Columns["BMTIMECOUNT(min)"].ColumnName.PadRight(20, ' ') + dtResultFilter.Columns["LOSSTIME(min)"].ColumnName.ToString().PadRight(20, ' ') + dtResultFilter.Columns["LOSS"].ColumnName.ToString().PadRight(10, ' ') + dtResultFilter.Columns["BMCODE"].ColumnName.ToString().PadRight(45, ' '));//
                            for (int i = 0; i < dtResultFilter.Rows.Count; i++)
                            {
                                builder.AppendLine(dtResultFilter.Rows[i]["EQUIPMENT"].ToString().PadRight(15, ' ') + dtResultFilter.Rows[i]["BMTIMECOUNT(min)"].ToString().PadRight(20, ' ') + dtResultFilter.Rows[i]["LOSSTIME(min)"].ToString().PadRight(20, ' ') + dtResultFilter.Rows[i]["LOSS"].ToString().PadRight(10, ' ') + dtResultFilter.Rows[i]["BMCODE"].ToString().PadRight(45, ' '));
                            }
                        }

                        toolTipController.ShowHint(mouseX + ":\n" + builder.ToString(), chartControl1.PointToScreen(e.Location));
                    }
                }
                else
                {
                    toolTipController.HideHint();
                }

            }
            else
            {
                toolTipController.HideHint();
            }


        }
        private void chartControl1_MouseLeave(object sender, EventArgs e)
        {
            toolTipController.HideHint();
        }

        private void chartControl1_BoundDataChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < chartControl1.Series["OutPut"].Points.Count; i++)
            {
                string Points = chartControl1.Series["OutPut"].Points[i].Argument;
                if (Points.Contains(mindata.Rows[0]["AREA"].ToString()) || Points.Contains(mindata.Rows[1]["AREA"].ToString()))
                {
                    chartControl1.Series["OutPut"].Points[i].Color = Color.Orange;

                }
            }
        }
    }
}
