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
        IPagedList<DbInfo> list;
        BizDataClient bs = null;
        List<DbInfo> stDataList = null;


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

        private async Task<List<T>> GetDashBoardData<T>() where T : new()
        {
            return await Task.Factory.StartNew(() =>
            {
                DataSet ds= bs.ExecuteDataSet("GetDashBoardData", new AwrArgsPack().getPack());
                List<T> restlt=Utils.DataTableToList<T>(ds.Tables[0]);
                return restlt;
            }
            );
        }
            
        private async void FrmISIADashBoard_Load(object sender, EventArgs e)
        {
            stDataList= await GetDashBoardData<DbInfo>();
            list = await GetPagedAsyncList(stDataList);
            btnPre.Enabled = list.HasPreviousPage;
            btnNext.Enabled = list.HasNextPage;
            gridControl1.DataSource = list.ToList();
            pageInfoLable.Text = string.Format("Page {0}/{1}", pageNumber, list.PageCount);
        }

        private async void btnPre_Click(object sender, EventArgs e)
        {
            if (list.HasPreviousPage)
            {
                list = await GetPagedAsyncList(stDataList,--pageNumber);
                btnPre.Enabled = list.HasPreviousPage;
                btnNext.Enabled = list.HasNextPage;
                gridControl1.DataSource = list.ToList();
                pageInfoLable.Text = string.Format("Page {0}/{1}", pageNumber, list.PageCount);
            }
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            if (list.HasNextPage)
            {
                list = await GetPagedAsyncList(stDataList,++pageNumber);
                btnPre.Enabled = list.HasPreviousPage;
                btnNext.Enabled = list.HasNextPage;
                gridControl1.DataSource = list.ToList();
                pageInfoLable.Text = string.Format("Page {0}/{1}", pageNumber, list.PageCount);

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

        public class DbInfo
        {

            public DbInfo()
            {

            }

            public DbInfo(string version, string dbName, int retentionDays, string cdbName, string retentionPeriod, string uploadInterval, string minTime, string maxTime, int cnt, string targetType)
            {
                this.version = version;
                this.dbName = dbName;
                this.retentionDays = retentionDays;
                this.cdbName = cdbName;
                this.retentionPeriod = retentionPeriod;
                this.uploadInterval = uploadInterval;
                this.minTime = minTime;
                this.maxTime = maxTime;
                this.cnt = cnt;
                this.targetType = targetType;
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
        }
        public class Student
        {
            private string name1;
            private int age1;
            private int height1;
            private string startTime1;
            private string endTime1;
            private string idCard1;
            private int process1;
            private int condition1;

            public Student() { }
            public Student(string name, int age, int height, string startTime, string endTime, string idCard, int process, int condition)
            {
                this.name = name;
                this.age1 = age;
                this.height1 = height;
                this.startTime1 = startTime;
                this.endTime1 = endTime;
                this.idCard1 = idCard;
                this.process1 = process;
                this.condition1 = condition;
            }

            public string name { get => name1; set => name1 = value; }
            public int age { get => age1; set => age1 = value; }
            public int height { get => height1; set => height1 = value; }
            public string startTime { get => startTime1; set => startTime1 = value; }
            public string endTime { get => endTime1; set => endTime1 = value; }
            public string idCard { get => idCard1; set => idCard1 = value; }
            public int process { get => process1; set => process1 = value; }
            public int condition { get => condition1; set => condition1 = value; }
        }

        private void bandedGridView1_AfterPrintRow(object sender, DevExpress.XtraGrid.Views.Printing.PrintRowEventArgs e)
        {
            DataRow list=bandedGridView1.GetDataRow(0);
        }
    }
}