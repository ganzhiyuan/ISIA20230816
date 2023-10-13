using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using ISIA.COMMON;
using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;
using PagedList;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.UI;
using Utils = ISIA.COMMON.Utils;

namespace ISIA.UI.TREND
{
    public partial class FrmISIADashBoard : UIBase
    {

        int pageNumber = 1;
        IPagedList<DbInfo> allDataPagedlist;
        IPagedList<DbInfo> searchPagedList;
        IPagedList<DbInfo> currentPagedList;



        BizDataClient bs = null;
        List<DbInfo> allDataList = null;
        List<DbInfo> searchList = null;
        List<DbInfo> currentList = null;

        public const string GET_DB_STATUS_FUNC = "GetDBFetchAwrDataStatus";
        public const string ERROR_FETCH_HOURS = "1";
        public const string DB_SNAP_FETCH_DAYS = "2";
        public const int TIMER_INTERVAL = 10000*60;




        public FrmISIADashBoard()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.ISIADashboard");
         
        }



       

        private async void FrmISIADashBoard_Load(object sender, EventArgs e)
        {
            allDataList = await GetDashBoardData<DbInfo>();
            List<DbInfo> errorDbList = await GetDashBoardData<DbInfo>(new AwrArgsPack() { StartTime = ERROR_FETCH_HOURS }, GET_DB_STATUS_FUNC);
            checkDbStatus(allDataList, errorDbList);
            allDataPagedlist = await GetPagedAsyncList(allDataList);
            WrapperGridView(allDataPagedlist);
            currentList = allDataList;
            currentPagedList = allDataPagedlist;
            WrapperLabelControl();
            bandedGridView1_RowClick(null, null);
            timer1.Interval = TIMER_INTERVAL;
            timer1.Enabled = true;

        }

        private async void btnPre_Click(object sender, EventArgs e)
        {
            if (currentPagedList.HasPreviousPage)
            {
                currentPagedList = await GetPagedAsyncList(currentList, --pageNumber);
                WrapperGridView(currentPagedList);
            }
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPagedList.HasNextPage)
            {
                currentPagedList = await GetPagedAsyncList(currentList, ++pageNumber);
                WrapperGridView(currentPagedList);
            }
        }

        private void bandedGridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.Name == "DBNAMEbandedGridColumn")
            {
                int rownum = e.RowHandle;
                int status = int.Parse(bandedGridView1.GetRowCellValue(rownum, "STATUS").ToString());
                if (status == 0)
                {
                    e.Appearance.ForeColor = Color.Red;
                }
                else
                {
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }

        private void labelControl2_Click(object sender, EventArgs e)
        {

        }
      




        private async void btnSelect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                allDataList = await GetDashBoardData<DbInfo>();
                List<DbInfo> errorDbList1 = await GetDashBoardData<DbInfo>(new AwrArgsPack() { StartTime = ERROR_FETCH_HOURS }, GET_DB_STATUS_FUNC);
                checkDbStatus(allDataList, errorDbList1);
                currentList = allDataList;
                currentPagedList = await GetPagedAsyncList(currentList, pageNumber = 1);
                WrapperGridView(currentPagedList);
                WrapperLabelControl();
                return;
            }
            AwrArgsPack argsPack = new AwrArgsPack();
            argsPack.DBName = textBox.Text;
            searchList = await GetDashBoardData<DbInfo>(argsPack);
            List<DbInfo> errorDbList = await GetDashBoardData<DbInfo>(new AwrArgsPack() { StartTime = ERROR_FETCH_HOURS }, GET_DB_STATUS_FUNC);
            checkDbStatus(searchList, errorDbList);
            searchPagedList = await GetPagedAsyncList(searchList);
            WrapperGridView(searchPagedList);
            //change current page list
            currentPagedList = searchPagedList;
            currentList = searchList;
            WrapperLabelControl();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnSelect_Click(null, null);
        }

      

        private void bandedGridView1_DoubleClick(object sender, EventArgs e)
        {
            DbInfo row = (DbInfo)bandedGridView1.GetFocusedRow();
            if (row is null)
            {
                return;
            }

            Hashtable dataLinkToWorkloadHashTable = new Hashtable();
            dataLinkToWorkloadHashTable.Add("DBNAME", row.DBNAME);
            base.OpenUI("WORKLOADTRENDCHART", "AWR", "WORKLOADTRENDCHART", null, dataLinkToWorkloadHashTable);
        }

        private void bandedGridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Column.FieldName.Equals("DBNAME") || e.Column.FieldName.Equals("STATUS"))
            {
                AwrArgsPack awrArgs = new AwrArgsPack();
                awrArgs.DBName = bandedGridView1.GetRowCellValue(e != null ? e.RowHandle : 0, "DBNAME").ToString();
                awrArgs.StartTime = DB_SNAP_FETCH_DAYS;
                ShowProcedureErrorMessage(awrArgs, e);
            }
        }

        private async Task<List<T>> GetDashBoardData<T>(AwrArgsPack argsPack = null, string func = "GetDashBoardData") where T : new()
        {
            return await Task.Factory.StartNew(() =>
            {
                DataSet ds = bs.ExecuteDataSet(func, argsPack == null ? new AwrArgsPack().getPack() : argsPack.getPack());
                if (ds == null)
                {
                    return new List<T>();
                }
                List<T> restlt = Utils.DataTableToList<T>(ds.Tables[0]);
                return restlt;
            }
            );
        }

        private void checkDbStatus(List<DbInfo> allDbs, List<DbInfo> errorDbs)
        {
            List<string> errorDbList = new List<string>();
            errorDbs.ForEach(db =>
            {
                int index = db.DBNAME.LastIndexOf('_');
                errorDbList.Add(db.DBNAME.Substring(++index));
            });
            allDbs.ForEach(db =>
            {
                if (errorDbList.Contains(db.DBNAME))
                {
                    db.STATUS = 0;
                }
            });
        }

        private void WrapperGridView<T>(IPagedList<T> pageList)
        {
            btnPre.Enabled = pageList.HasPreviousPage;
            btnNext.Enabled = pageList.HasNextPage;
            gridControl1.DataSource = pageList.ToList();
            pageInfoLable.Text = string.Format("Page {0}/{1}", pageNumber, pageList.PageCount);
        }

        private void WrapperLabelControl()
        {
            int dbCount = allDataList.Count;
            int pdbCount = allDataList.AsEnumerable<DbInfo>().Where(t => t.TARGETTYPE.Equals("pdb")).ToList().Count;
            labelControl1.AllowHtmlString = true;
            labelControl2.AllowHtmlString = true;
            labelControl2.Text = string.Format("<size=30><b>{0} DBS</b></size ><br><br><color = black><b>total count of dbs including pdbs</b></color>", dbCount);
            labelControl1.Text = string.Format("<size=30><b>{0} PDBS</b></size ><br><br><color = black><b>total count of pdbs</b></color>", pdbCount);
        }

        private void bandedGridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            AwrArgsPack awrArgs = new AwrArgsPack();
            awrArgs.DBName = bandedGridView1.GetRowCellValue(e != null ? e.RowHandle : 0, "DBNAME").ToString();
            awrArgs.StartTime = DB_SNAP_FETCH_DAYS;
            DataSet ds = bs.ExecuteDataSet("GetSnapAlreadyFetchCount", awrArgs.getPack());
            DisplayChart(ds);
            chartsimpleLabelItem.Text = string.Format(@"<b><color=red>        Condition: DB '{0}' Snapshot Count Uploaded Into ISIA System</color></b>", awrArgs.DBName);
        }
        private void ShowProcedureErrorMessage(AwrArgsPack arg, RowClickEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            if (bandedGridView1.GetRowCellValue(e != null ? e.RowHandle : 0, "STATUS").ToString().Equals("0"))
            {
                arg.StartTime = ERROR_FETCH_HOURS;
                DataSet ds = bs.ExecuteDataSet("GetFetchProcedureErrorMessage", arg.getPack());
                List<ProcedureMessage> procedureMessages = Utils.DataTableToList<ProcedureMessage>(ds.Tables[0]);
                procedureMessages = procedureMessages.Where(ele => ele.PROCESSNAME.Substring(ele.PROCESSNAME.LastIndexOf('_') + 1).Equals(arg.DBName)).ToList();
                PopupGrid popupGrid = new PopupGrid(Utils.ConvertToDataSet<ProcedureMessage>(procedureMessages).Tables[0]);
                popupGrid.Text = "ErrorDetail";
                popupGrid.StartPosition = FormStartPosition.CenterScreen;
                popupGrid.ShowDialog();
            }
        }

        public void DisplayChart(DataSet ds)
        {

            DataTable dt = ds.Tables[0];
            this.chart1.Series.Clear();
            this.chart1.ContextMenuStrip = this.contextMenuStrip1;
            this.chart1.Legend.LegendStyle = LegendStyles.Series;
            Steema.TeeChart.Styles.Bar bar1 = new Steema.TeeChart.Styles.Bar(chart1.Chart);
            var markstip = new MarksTip(chart1.Chart);

            void Bar_GetSeriesMark(Series Series, GetSeriesMarkEventArgs e)
            {
                //e.MarkText = $"{dt1.Rows[e.ValueIndex]["Name"]} is {dt1.Rows[e.ValueIndex]["NUM"]}";
                e.MarkText = "workdate :" + $"{dt.Rows[e.ValueIndex]["WORKDATE"]}" + "\r\n" + "VALUE :" + $"{ dt.Rows[e.ValueIndex]["COUNT"]}";
            }
            bar1.Marks.Visible = false;
            bar1.Legend.Visible = false;
            bar1.LabelMember = "WORKDATE";
            bar1.YValues.DataMember = "COUNT";
            bar1.DataSource = dt;
            bar1.GetSeriesMark += Bar_GetSeriesMark;//提示信息事件

        }
        private async Task<IPagedList<T>> GetPagedAsyncList<T>(List<T> data, int pageNumber = 1, int pagesize = 10)
        {
            return await Task.Factory.StartNew(() =>
            {
                PagedList<T> pagedList = new PagedList<T>(data, pageNumber, pagesize);
                return pagedList;
            }
            );
        }

        public class DbInfo
        {

            public DbInfo()
            {

            }


            private string version;
            private string dbName;
            private string retentionDays;
            private string cdbName;
            private string cdbId;
            private string retentionPeriod;
            private string uploadInterval;
            private string minTime;
            private string maxTime;
            private int cnt;
            private string targetType;
            private int status;
            private int instanceCount;
            private string intervalMinutes;



            public string VERSION { get => version; set => version = value; }
            public string DBNAME { get => dbName; set => dbName = value; }
            public string CDBNAME { get => cdbName; set => cdbName = value; }
            public string RETENTIONPERIOD { get => retentionPeriod; set => retentionPeriod = value; }
            public string UPLOADINTERVAL { get => uploadInterval; set => uploadInterval = value; }
            public string MINTIME { get => minTime; set => minTime = value; }
            public string MAXTIME { get => maxTime; set => maxTime = value; }
            public int CNT { get => cnt; set => cnt = value; }
            public string TARGETTYPE { get => targetType; set => targetType = value; }
            public int STATUS { get => status; set => status = value; }
            public int INSTANCECOUNT { get => instanceCount; set => instanceCount = value; }
            public string CDBID { get => cdbId; set => cdbId = value; }
            public string INTERVALMINUTES { get => intervalMinutes; set => intervalMinutes = value; }
            public string RETENTIONDAYS { get => retentionDays; set => retentionDays = value; }
        }

        public class ProcedureMessage
        {

            private string processName;
            private string startTime;
            private string endTime;
            private string errorMessage;

            public ProcedureMessage()
            {
            }

            public string PROCESSNAME { get => processName; set => processName = value; }
            public string STARTTIME { get => startTime; set => startTime = value; }
            public string ENDTIME { get => endTime; set => endTime = value; }
            public string ERRORMESSAGE { get => errorMessage; set => errorMessage = value; }
        }

        
    }
}