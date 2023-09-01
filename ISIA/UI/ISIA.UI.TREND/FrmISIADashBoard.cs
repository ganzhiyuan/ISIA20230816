using DevExpress.XtraEditors;
using ISIA.COMMON;
using ISIA.INTERFACE.ARGUMENTSPACK;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.UI;

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
        public const string errorFetchPeriodHour = "100";



        public FrmISIADashBoard()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.ISIADashboard");
        }

     

        //private void GetTestData()
        //{
        //    DateTime currentTime = DateTime.Now;
        //    DateTime beforeTime = currentTime.AddHours(-1);
        //    string format = "yyyy-MM-dd HH:mm:ss";
        //    string endStr = currentTime.ToString(format);
        //    string startStr = beforeTime.ToString(format);
        //    List<Student> stList = new List<Student>();
        //    for (int i = 0; i < 50; i++)
        //    {
        //        Student st = new Student("John" + i, i, i, startStr, endStr, "2231231231", i, i);
        //        stList.Add(st);
        //    }
        //    stDataList = stList;
        //}

        private async Task<List<T>> GetDashBoardData<T>(AwrArgsPack argsPack =null, string func= "GetDashBoardData") where T : new()
        {
            return await Task.Factory.StartNew(() =>
            {
                DataSet ds= bs.ExecuteDataSet(func, argsPack==null?new AwrArgsPack().getPack():argsPack.getPack());
                if (ds == null)
                {
                    return new List<T>();
                }
                List<T> restlt=Utils.DataTableToList<T>(ds.Tables[0]);
                return restlt;
            }
            );
        }
            
        private async void FrmISIADashBoard_Load(object sender, EventArgs e)
        {
            allDataList = await GetDashBoardData<DbInfo>();
            List<DbInfo> errorDbList = await GetDashBoardData<DbInfo>(new AwrArgsPack() {StartTime= errorFetchPeriodHour }, GET_DB_STATUS_FUNC);
            checkDbStatus(allDataList, errorDbList);
            allDataPagedlist = await GetPagedAsyncList(allDataList);
            WrapperGridView(allDataPagedlist);
            currentList = allDataList;
            currentPagedList = allDataPagedlist;
            WrapperLabelControl();


        }

        private async void btnPre_Click(object sender, EventArgs e)
        {
            if (currentPagedList.HasPreviousPage)
            {
                currentPagedList = await GetPagedAsyncList(currentList, --pageNumber);
                WrapperGridView(currentPagedList);
                //btnPre.Enabled = currentPagedList.HasPreviousPage;
                //btnNext.Enabled = currentPagedList.HasNextPage;
                //gridControl1.DataSource = currentPagedList.ToList();
                //pageInfoLable.Text = string.Format("Page {0}/{1}", pageNumber, currentPagedList.PageCount);
            }
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPagedList.HasNextPage)
            {
                currentPagedList = await GetPagedAsyncList(currentList, ++pageNumber);
                WrapperGridView(currentPagedList);
                //btnPre.Enabled = currentPagedList.HasPreviousPage;
                //btnNext.Enabled = currentPagedList.HasNextPage;
                //gridControl1.DataSource = currentPagedList.ToList();
                //pageInfoLable.Text = string.Format("Page {0}/{1}", pageNumber, currentPagedList.PageCount);

            }
        }

        private void bandedGridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {

        }

        private void labelControl2_Click(object sender, EventArgs e)
        {

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

        
       
      

        private void bandedGridView1_AfterPrintRow(object sender, DevExpress.XtraGrid.Views.Printing.PrintRowEventArgs e)
        {
            DataRow list=bandedGridView1.GetDataRow(0);
        }

        private async void btnSelect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                currentList = allDataList;
                currentPagedList= await GetPagedAsyncList(currentList, pageNumber=1);
                WrapperGridView(currentPagedList);
                return;
            }
            AwrArgsPack argsPack = new AwrArgsPack();
            argsPack.DBName = textBox.Text;
            searchList = await GetDashBoardData<DbInfo>(argsPack);
            searchPagedList = await GetPagedAsyncList(searchList);
            WrapperGridView(searchPagedList);
            //change current page list
            currentPagedList = searchPagedList;
            currentList = searchList;
        }

        private void checkDbStatus(List<DbInfo> allDbs, List<DbInfo> errorDbs) 
        {
            List<string> errorDbList = new List<string>();
            errorDbs.ForEach(db => 
            {
                int index=db.DBNAME.LastIndexOf('_');
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

        public class DbInfo
        {

            public DbInfo()
            {

            }


            private string version;
            private string dbName;
            private int retentionDays;
            private string cdbName;
            private string retentionPeriod;
            private string uploadInterval;
            private string minTime;
            private string maxTime;
            private int cnt;
            private string targetType;
            private int status;
            private int instanceCount;

            public string VERSION { get => version; set => version = value; }
            public string DBNAME { get => dbName; set => dbName = value; }
            public int RETENTIONDAYS { get => retentionDays; set => retentionDays = value; }
            public string CDBNAME { get => cdbName; set => cdbName = value; }
            public string RETENTIONPERIOD { get => retentionPeriod; set => retentionPeriod = value; }
            public string UPLOADINTERVAL { get => uploadInterval; set => uploadInterval = value; }
            public string MINTIME { get => minTime; set => minTime = value; }
            public string MAXTIME { get => maxTime; set => maxTime = value; }
            public int CNT { get => cnt; set => cnt = value; }
            public string TARGETTYPE { get => targetType; set => targetType = value; }
            public int STATUS { get => status; set => status = value; }
            public int INSTANCECOUNT { get => instanceCount; set => instanceCount = value; }

        }

    }
}