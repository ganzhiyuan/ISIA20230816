using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.UI;
using ISIA.UI.BASE;
using Steema.TeeChart;
using System.Xml;
using Steema.TeeChart.Tools;
using Steema.TeeChart.Styles;
using DevExpress.XtraEditors.Controls;
using System.Linq;
using Analysis.Correlation;
using DevExpress.XtraGrid.Views.Printing;
using System.Drawing;
using TAP;
using System.Collections;

namespace ISIA.UI.ANALYSIS
{
    public partial class FrmWorkloadSqlCorrelationAnalysis : DockUIBase1T1
    {
        //define bs
        BizDataClient bs = null;
        //used when linkage to sql influence frm
        private DataRow _FocusedRowDr;

        BizDataClient BsForGettingSqlInfluence = new BizDataClient("ISIA.BIZ.ANALYSIS.DLL", "ISIA.BIZ.ANALYSIS.SqlInfluenceAnalysis");

        AwrArgsPack argument = null;


        #region getset
        public BizDataClient Bs { get => bs; set => bs = value; }
        public DataRow FocusedRowDr { get => _FocusedRowDr; set => _FocusedRowDr = value; }
        #endregion

        public FrmWorkloadSqlCorrelationAnalysis()
        {
            InitializeComponent();
            Bs = new BizDataClient("ISIA.BIZ.ANALYSIS.DLL", "ISIA.BIZ.ANALYSIS.WorkloadSqlCorrelationAnalysis");
            this.InitializeControls();
            //initialize bs
        }

        #region event
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                this.AsyncGetWorkloadAndSqlComparisonData();
            }
            catch (Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(TAP.UI.EnumMsgType.CONFIRM, ex.Message);
            }
        }

        public override void ExecuteCommand(ArgumentPack arguments)
        {
            foreach (string tmpstr in arguments.ArgumentNames)
            {
                if (tmpstr == "_hashTable")
                {
                    Hashtable hashtable = (Hashtable)arguments["_hashTable"].ArgumentValue;
                    DataTable tmpdt = (DataTable)hashtable["dt"];
                    argument = new AwrArgsPack();
                    argument.WorkloadSqlParm = (string)hashtable["workloadParm"];
                    argument.StartTime = (string)hashtable["startTime"];
                    argument.EndTime = (string)hashtable["endTime"];
                    argument.DBName = (string)hashtable["DbName"];
                    DisplayChart(tmpdt.DataSet);
                }
            }
        }
        private void gridView1_MouseUp(object sender, MouseEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = this.gridView1.CalcHitInfo(e.Location);
            if (hi.InRow && e.Button == MouseButtons.Right)
            {
                this.popupMenu1.ShowPopup(Control.MousePosition);
            }
        }


        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            FocusedRowDr = this.gridView1.GetDataRow(e.FocusedRowHandle) as DataRow;
        }

        /*
         *  use when jump to sql influence frm.
         */
        private void barButtonItem1SqlInfluence_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                argument.WorkloadSqlParm = (string)FocusedRowDr["SQL_PARM"];
                DataSet resultDs = BsForGettingSqlInfluence.ExecuteDataSet("GetSqlInfluenceData", argument.getPack());

                Hashtable ht = new Hashtable();
                ht.Add("DS", resultDs);
                ht.Add("SQL_PARM", argument.WorkloadSqlParm);
                base.OpenUI("SQLINFLUENCEANALYSIS", "ANALYSIS", "Sql Influence Analysis", null, ht);
            }
            catch (Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(TAP.UI.EnumMsgType.CONFIRM, ex.Message);
            }

        }

        #endregion

        #region Initialize

        public static string TIME_SELECTION = "A";


        private void InitializeControls()
        {
            //init date
            InitializeDatePeriod();
            //init dbname
            InitializeDbName();
            //init workload
            InitializeWorkloadParm();
        }

        private void InitializeDatePeriod()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@".\ISIA.config");
            XmlNodeList nodeList = doc.SelectNodes("configuration/TAP.ISIA.Configuration/WX/Shift");
            this.dateStart.DateTime = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + " " + nodeList[0][TIME_SELECTION].Attributes["StartTime"].Value);
            this.dateEnd.DateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + nodeList[0][TIME_SELECTION].Attributes["EndTime"].Value);

        }

        private void InitializeDbName()
        {
            AwrArgsPack args = new AwrArgsPack();
            DataSet ds = Bs.ExecuteDataSet("GetDBName", args.getPack());
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                this.comboBoxDBName.Properties.Items.Add(dr["DbName"]);
            }
        }



        private void InitializeWorkloadParm()
        {
            foreach (KeyValuePair<string, string> pair in AwrArgsPack.WorkloadSqlRelationMapping)
            {
                this.comboBoxEditWorkloadSql.Properties.Items.Add(pair.Key);
            }
        }

        #endregion

        #region btnSelect_Click Event

        private void AsyncGetWorkloadAndSqlComparisonData()
        {
            //handle argument
            argument = HandleArgument();
            //async load data 
            this.BeginAsyncCall("LoadWorkloadSqlData", "DisplayChart", EnumDataObject.DATASET, null);
        }

        private AwrArgsPack HandleArgument()
        {
            AwrArgsPack argument = new AwrArgsPack();
            //date period handling 
            object startTime = dateStart.EditValue;
            object endTime = dateEnd.EditValue;
            if (startTime == null || endTime == null)
            {
                string errMessage = "Please select StartTime or EndTime";
                throw new Exception(errMessage);
            }
            DateTime startDateTime = (DateTime)dateStart.EditValue;
            DateTime endDateTime = (DateTime)dateEnd.EditValue;

            if (startDateTime > endDateTime)
            {
                argument.StartTime = endDateTime.ToString("yyyyMMddHHmmss");
                argument.EndTime = startDateTime.ToString("yyyyMMddHHmmss");
            }
            argument.StartTime = startDateTime.ToString("yyyyMMddHHmmss");
            argument.EndTime = endDateTime.ToString("yyyyMMddHHmmss");

            //combobox edit db name 
            string dbName = comboBoxDBName.Text;
            if (string.IsNullOrEmpty(dbName))
            {
                string errMessage = "Please select DB_NAME";
                throw new Exception(errMessage);
            }
            argument.DBName = dbName;



            //combobox edit workload parm
            string workloadSql = comboBoxEditWorkloadSql.Text;
            if (string.IsNullOrEmpty(workloadSql))
            {
                string errMessage = "Please select Workload parm";
                throw new Exception(errMessage);
            }
            argument.WorkloadSqlParm = workloadSql;

            return argument;
        }
        public DataSet LoadWorkloadSqlData()
        {
            return Bs.ExecuteDataSet("GetWorkloadSqlCorrelationData", argument.getPack());
        }

        public void DisplayChart(DataSet ds)
        {
            DataTable dt = ds.Tables[0];
            #region unuse
            ////linkage test
            //List<object> sqlList = (from r in dt.AsEnumerable() select r["sql_id"]).Distinct().ToList();
            //List<object> snapIdList = (from r in dt.AsEnumerable() select r["snap_id"]).Distinct().ToList();
            //List<SqlCorrelationValue> result = new List<SqlCorrelationValue>();
            //foreach (string sqlId in sqlList)
            //{
            //    DateTime beginTime = DateTime.Now; 
            //    //error snap_id
            //    bool isValid = true;
            //    //workload parm handle
            //    List<object> workloadParm = new List<object>();
            //    List<double> workloadParmDouble = new List<double>();
            //    DataTable dataFilter = dt.AsEnumerable().Where(r => r.Field<string>("sql_Id") == sqlId).CopyToDataTable();
            //    if (dataFilter.Rows.Count <= 2)
            //    {
            //        continue;
            //    }
            //    if (workloadParm.Count <= 0)
            //    {
            //        workloadParm = (from r in dataFilter.AsEnumerable() select r[awrArgsPack.WorkloadSqlParm]).ToList();
            //        foreach (object ele in workloadParm)
            //        {
            //            try
            //            {
            //                workloadParmDouble.Add(((double)((Decimal)ele)));
            //            }
            //            catch (Exception ex)
            //            {
            //                isValid = false;
            //                break;
            //            }
            //        }
            //    }
            //    //sql param handle
            //    List<double> sqlParmDouble = new List<double>();
            //    List<object> sqlParm = (from r in dataFilter.AsEnumerable() select r[AwrArgsPack.WorkloadSqlRelationMapping[awrArgsPack.WorkloadSqlParm]]).ToList();
            //    foreach (object ele in sqlParm)
            //    {
            //        try
            //        {
            //            sqlParmDouble.Add(((double)((Decimal)ele)));
            //        }
            //        catch (Exception ex)
            //        {
            //            isValid = false;
            //            break;
            //        }
            //    }
            //    if (!isValid)
            //    {
            //        continue;
            //    }
            //    //calculate correlation 
            //    double correlationResult = CorrelationExecuter.Calculate(workloadParmDouble.ToArray(), sqlParmDouble.ToArray());
            //    DateTime endTime = DateTime.Now;
            //    TimeSpan oTime = endTime.Subtract(beginTime);
            //    Console.WriteLine(oTime.TotalSeconds);
            //    result.Add(new SqlCorrelationValue(sqlId, System.Math.Abs(correlationResult), sqlParmDouble.Count));
            //}
            //result = result.OrderByDescending(a => a.CorrelationValue).ToList();
            #endregion
            List<int> invalidIndexWorkload = new List<int>();
            List<object> workloadParm = (from r in dt.AsEnumerable() select r[argument.WorkloadSqlParm]).ToList();
            List<double> workloadParmDouble = new List<double>();
            List<SqlCorrelationValue> result = new List<SqlCorrelationValue>();

            for (int i = 0; i < workloadParm.Count; i++)
            {
                try
                {
                    workloadParmDouble.Add(((double)((decimal)workloadParm[i])));
                }
                catch (Exception ex)
                {
                    invalidIndexWorkload.Add(i);
                    workloadParmDouble.Add(-1);
                }
            }
            foreach (string sqlParm in AwrArgsPack.SqlParmsList)
            {
                List<int> invalidIndexSql = new List<int>();

                List<object> snapIdList = (from r in dt.AsEnumerable() select r[sqlParm]).ToList();
                List<double> sqlParmDouble = new List<double>();
                for (int i = 0; i < snapIdList.Count; i++)
                {
                    try
                    {
                        sqlParmDouble.Add(((double)((decimal)snapIdList[i])));
                    }
                    catch (Exception ex)
                    {
                        invalidIndexSql.Add(i);
                        sqlParmDouble.Add(-1);
                    }
                }
                invalidIndexSql.AddRange(invalidIndexWorkload);
                foreach (int index in invalidIndexSql)
                {
                    sqlParmDouble.Remove(index);
                    workloadParmDouble.Remove(index);
                }
                double correlationResult = CorrelationExecuter.Calculate(workloadParmDouble.ToArray(), sqlParmDouble.ToArray());
                result.Add(new SqlCorrelationValue(argument.WorkloadSqlParm, System.Math.Abs(correlationResult), sqlParm, correlationResult));
            }
            result = result.OrderByDescending(a => a.CorrelationValueAbs).ToList();
            int rank = 1;
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Ranking = rank++;
            }

            // dt generate
            DataTable resultDt = new DataTable();
            resultDt.Columns.Add("Ranking", typeof(string));

            resultDt.Columns.Add("WORKLOAD_PARM", typeof(string));
            resultDt.Columns.Add("SQL_PARM", typeof(string));
            resultDt.Columns.Add("R", typeof(string));

            foreach (SqlCorrelationValue ele in result)
            {
                resultDt.Rows.Add(ele.Ranking, ele.WorkloadParm, ele.SqlParm, ele.CorrelationValue);
            }
            //show grid
            this.gridView1.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView1_RowCellStyle);
            this.gridControlWorkloadSqlCorrelation.DataSource = null;
            this.gridView1.Columns.Clear();
            this.gridControlWorkloadSqlCorrelation.DataSource = resultDt;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.BestFitColumns();
        }

        //R column show color
        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            //更多可以通过逻辑筛选
            //列名为 gridColumn2的单元列
            if (e.Column.FieldName.Equals("R"))
            {
                var v = e.CellValue;
                if (v != null)
                {//当单元列名称为  gridColumn2  值大于 standard时，单元列背景颜色变红

                    e.Appearance.BackColor = GetCorrelationColor(double.Parse(v.ToString()));

                }
            }
        }

        private Color GetCorrelationColor(double value)
        {
            Color color = new Color();

            if (value >= 0.9) color = System.Drawing.Color.FromArgb(152, 39, 1);

            else if (value >= 0.8) color = System.Drawing.Color.FromArgb(179, 47, 2);

            else if (value >= 0.7) color = System.Drawing.Color.FromArgb(210, 54, 2);

            else if (value >= 0.6) color = System.Drawing.Color.FromArgb(249, 64, 2);

            else if (value >= 0.5) color = System.Drawing.Color.FromArgb(253, 87, 32);

            else if (value >= 0.4) color = System.Drawing.Color.FromArgb(253, 113, 66);

            else if (value >= 0.3) color = System.Drawing.Color.FromArgb(254, 142, 105);

            else if (value >= 0.2) color = System.Drawing.Color.FromArgb(254, 171, 143);

            else if (value >= 0.1) color = System.Drawing.Color.FromArgb(254, 216, 203);

            else if (value > 0) color = System.Drawing.Color.FromArgb(254, 241, 236);

            else if (value == 0) color = System.Drawing.Color.FromArgb(255, 255, 255);

            else if (value >= -0.1) color = System.Drawing.Color.FromArgb(230, 242, 255);

            else if (value >= -0.2) color = System.Drawing.Color.FromArgb(199, 226, 254);

            else if (value >= -0.3) color = System.Drawing.Color.FromArgb(159, 206, 253);

            else if (value >= -0.4) color = System.Drawing.Color.FromArgb(128, 190, 153);

            else if (value >= -0.5) color = System.Drawing.Color.FromArgb(86, 169, 152);

            else if (value >= -0.6) color = System.Drawing.Color.FromArgb(40, 146, 251);

            else if (value >= -0.7) color = System.Drawing.Color.FromArgb(4, 125, 247);

            else if (value >= -0.8) color = System.Drawing.Color.FromArgb(4, 104, 204);

            else if (value >= -0.9) color = System.Drawing.Color.FromArgb(3, 83, 163);

            else if (value >= -1) color = System.Drawing.Color.FromArgb(2, 64, 125);

            return color;

        }
        private class SqlCorrelationValue
        {
            private int _Ranking;
            private string _WorkloadParm;
            private string _SqlParm;
            private double _CorrelationValueAbs;
            private double _CorrelationValue;

            public SqlCorrelationValue(string workloadParm, double correlationAbs, string sqlParm, double correlationValue)
            {
                this.WorkloadParm = workloadParm;
                this.CorrelationValue = correlationValue;
                this.SqlParm = sqlParm;
                this.CorrelationValueAbs = correlationAbs;
            }

            public int Ranking { get => _Ranking; set => _Ranking = value; }
            public string WorkloadParm { get => _WorkloadParm; set => _WorkloadParm = value; }
            public string SqlParm { get => _SqlParm; set => _SqlParm = value; }
            public double CorrelationValueAbs { get => _CorrelationValueAbs; set => _CorrelationValueAbs = value; }
            public double CorrelationValue { get => _CorrelationValue; set => _CorrelationValue = value; }




            #endregion
        }


    }
}
