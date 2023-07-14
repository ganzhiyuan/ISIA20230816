
using DevExpress.XtraEditors;
using DevExpress.XtraWizard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using TAP.Data.Client;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System.Threading;

namespace ISIA.UI.MANAGEMENT
{
    public partial class FrmAddDataBase : XtraForm
    {
        Int32 ltTimer = 0;
        bool finish = false;

        BizDataClient bs = null;
        CreateDataBaseArgsPack args = null;

        DataSet ds = new DataSet();


        //创建dblink
        string userId = string.Empty;
        string password = string.Empty;
        string ip = string.Empty;
        string port = string.Empty;
        string serviceName = string.Empty;
        string dblinkname = string.Empty;
        bool dblinksta;

        //
        public FrmAddDataBase() : this(true, WizardStyle.Wizard97)
        {
            //InitializeComponent();

            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.CreateDataBaseManagement");
            welcomeWizardPage1.AllowNext = false;//next按钮是否启用属性

            
        }

        public FrmAddDataBase(bool allowAnimation, WizardStyle style)
        {
            InitializeComponent();
            //zenm Icon = DevExpress.Utils.ResourceImageHelper.CreateIconFromResourcesEx("DevExpress.XtraWizard.Demos.AppIcon.ico", typeof(frmMain).Assembly);
            wizardControl1.AllowTransitionAnimation = allowAnimation;
            wizardControl1.WizardStyle = style;
            //memoEdit1.Text = Info.LongText;

        }


        private  void wizardControl1_SelectedPageChangingAsync(object sender, DevExpress.XtraWizard.WizardPageChangingEventArgs e)
        {
            if (e.Page == wpfinish)
            {

            }
            if (e.Page == wpdatatable)
            {
                listBoxdatatable.Items.Clear();
                List<string> dvalues = DataBaseInfo.datatablelist;
                foreach (string item in dvalues)
                {
                    listBoxdatatable.Items.Add(string.Format( item, dblinkname));
                }
                
            }
            
            if (e.Page == wpprocedures)
            {
                listBoxprocedure.Items.Clear();
                List<string> pvalues = DataBaseInfo.procedures;
                foreach (string item in pvalues)
                {
                    listBoxprocedure.Items.Add(item);
                }

            }

            if (e.Page == wpdbinfo)
            {
                listBoxdbinfo.Items.Clear();
                List<string> dbvalues = DataBaseInfo.databaseinfo;
                foreach (string item in dbvalues)
                {
                    if (item.Contains("User ID"))
                    {
                        listBoxdbinfo.Items.Add(item + userId);
                    }
                    if (item.Contains("Service "))
                    {
                        listBoxdbinfo.Items.Add(item + serviceName);
                    }
                    if (item.Contains("IP") && !item.Contains("Port") )
                    {
                        listBoxdbinfo.Items.Add(item + ip);
                    }
                    if (item.Contains("Port"))
                    {
                        listBoxdbinfo.Items.Add(item + port);
                    }
                    if (item.Contains("DB Link"))
                    {
                        listBoxdbinfo.Items.Add(item + dblinkname);
                    }
                    if (item.Contains("Oracle"))
                    {
                        listBoxdbinfo.Items.Add(item);
                    }
                }
                foreach (var item in listBoxdatatable.Items)
                {
                    listBoxdbinfo.Items.Add(item.ToString()); 
                }
                foreach (var item in listBoxprocedure.Items)
                {
                    listBoxdbinfo.Items.Add(item.ToString());
                }

            }
            if (e.Page ==  wpbcreate)
            {
                wpbcreate.AllowNext = false;
                wpbcreate.AllowBack = false;
                wpbcreate.DescriptionText = "Program is being created";

                memoinfo.Lines.Initialize();
                memoinfo.AppendLine("Loading...");


                 wpbcreate.BeginInvoke(new Action( () =>
                {
                    CreateDBlinkAsync();
                    CreateDataTableAsync();
                 }));
                



                 string a = "asdqa";
            }

            if (e.Page == wpfinish)
            {
                wpfinish.AllowBack = false;

            }

        }

        #region CreateDBlink
        public void CreateDBlinkAsync()
        {
            ///1.创建dblink 
                args.UserID = userId;
                args.Password = password;
                args.IPAddress = ip;
                args.IPPort = port;
                args.ServiceName = serviceName;
                args.DBLinkName = dblinkname;

            if (dblinksta)
            {
                int res1 = bs.ExecuteModify("DropDBlink", args.getPack());
            }
                memoinfo.AppendLine(string.Format("Creating  DB LINK : {0}", dblinkname));

                int res = bs.ExecuteModify("CreateDBLink", args.getPack());

                if (res == -1)
                {
                    memoinfo.AppendLine(string.Format("DB LINK : {0} is created successfully ", dblinkname));
                }
                else
                {
                    memoinfo.AppendLine(string.Format("DB LINK : {0}  created fail ", dblinkname));
                }

                wpbcreate.DescriptionText = string.Format("Create DB LINK : {0}", dblinkname);

            //Thread.Sleep(5000);

                memoinfo.AppendLine("End of the program is run");
                wpbcreate.AllowNext = true;

        }
        #endregion

        public void CreateDataTableAsync()
        {


            /*if (dblinksta)
            {
                int res1 = bs.ExecuteModify("DropDBlink", args.getPack());
            }
            memoinfo.AppendLine(string.Format("Creating  DB LINK : {0}", dblinkname));*/
            memoinfo.AppendLine("Creating  DataTable ...... ");

            foreach (string table in DataBaseInfo.datatablelist)
            {
                string tablename = string.Format(table,dblinkname);
                memoinfo.AppendLine(tablename + " is being created ...... ");

                args.DataTableName = tablename;
                
                DateTime dateTime = DateTime.Now.Date;
                DateTime dateTime1 = new DateTime(dateTime.Year,dateTime.Month,1);
                args.DateNow = dateTime1;
                
                if (bs.ExecuteDataTable("GetDataTable", args.getPack()).Rows.Count > 0)
                {
                    DialogResult result = XtraMessageBox.Show(this, "This DataTable already exists . Choose yes if you still use it ,\n otherwise choose no and will be covered.",
                  "Warring", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {

                        
                    }
                    else if (result == DialogResult.No)
                    {

                        int res1 = bs.ExecuteModify("DropDataTable", args.getPack());//删除已存在的表

                        if (res1 == -1)
                        {
                            string input = tablename;
                            int lastUnderscoreIndex = input.LastIndexOf("_");
                            if (lastUnderscoreIndex != -1)
                            {
                                input = input.Substring(0, lastUnderscoreIndex);
                            }

                            string output = input.Replace("_", "");
                            string methodName = "Create" + output;
                            int restable = bs.ExecuteModify(methodName, args.getPack());//create table
                            if (restable != -1) return;

                            args.DataTablePKName = tablename + "_PK";//PK的名字

                            string IndexmethodName = "CreateIndex" + output;//index方法的名字
                            int resindex = bs.ExecuteModify(IndexmethodName, args.getPack());//create index
                            if (resindex != -1) return;

                            string PKmethodName = "CreatePK" + output;//PK方法的名字
                            int respk = bs.ExecuteModify(PKmethodName, args.getPack());//create PK
                            if (respk != -1) return;

                            memoinfo.AppendLine(tablename + " cerate successfully .");
                        }
                    }
                }
                else {
                    string input = tablename;
                    int lastUnderscoreIndex = input.LastIndexOf("_"); 
                    if (lastUnderscoreIndex != -1)
                    {
                        input = input.Substring(0, lastUnderscoreIndex); 
                    }

                    string output = input.Replace("_", "");
                    string methodName = "Create" + output;
                    int restable = bs.ExecuteModify(methodName, args.getPack());//create table
                    if (restable != -1) return;

                    args.DataTablePKName = tablename + "_PK";//PK的名字

                    string IndexmethodName = "CreateIndex" + output;//index方法的名字
                    int resindex = bs.ExecuteModify(IndexmethodName, args.getPack());//create index
                    if (resindex != -1) return;

                    string PKmethodName = "CreatePK" + output;//PK方法的名字
                    int respk = bs.ExecuteModify(PKmethodName, args.getPack());//create PK
                    if (respk != -1) return;

                    memoinfo.AppendLine(tablename + " cerate successfully .");

                } 

            }

            memoinfo.AppendLine("End of the create datatable ");


        }

        private void wizardControl1_NextClick(object sender, WizardCommandButtonClickEventArgs e)
        {
            
             if (e.Page == wizardDBLink)
             {
                //DBLINK页面，判断同名dblink是否存在，不存在则跳转下一页
                args = new CreateDataBaseArgsPack();
                
                if (textdblinkname.EditValue == null || textdblinkname.EditValue.ToString() == "")
                {
                    XtraMessageBox.Show(this, "Please input DB Link Name .  ",
                   "Warring", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Handled = true;
                    return;

                }
                else
                {
                    dblinkname = textdblinkname.EditValue.ToString().ToUpper();
                }

                args.DBLinkName = dblinkname;
                ds = bs.ExecuteDataSet("GetDBLink", args.getPack());

                if (ds.Tables[0].Rows.Count>0)
                {
                    DialogResult result = XtraMessageBox.Show(this, "DB Link Name already exists. Select yes if you still use it,\n otherwise select no  and re-type." +
                        "\n* If yes is selected, the DB Link Name will be overwritten ",
                  "Warring", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        dblinksta = true;
                        return;
                    }
                    else if (result == DialogResult.No)
                    {
                        dblinksta = false;
                        e.Handled = true;
                        return;
                    }
                    
                }

                
            }
        }

        private void sbPlay_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show(this, string.Format("Sorry, but we don’t have that song in our library...\r\nBut we agree with you that \"{0}\" is an excellent choice.", listBoxprocedure.SelectedValue),
                "XtraWizard", MessageBoxButtons.OK, MessageBoxIcon.Information);
           // wizardControl1.SelectedPage = wpLongText;
        }

        private void lbcPlay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void wizardControl1_FinishClick(object sender, CancelEventArgs e)
        {
            finish = true;

            //((ButtonBase)wizardControl1.Controls.Find("nextButton", true)[0]).Enabled = false;
            //((ButtonBase)wizardControl1.Controls.Find("nextButton", true)[0]).Enabled = false;
            this.Close();
        }

        private void wizardControl1_CancelClick(object sender, CancelEventArgs e)
        {
            this.Close();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (finish) return;
            if (XtraMessageBox.Show(this, "Do you want to exit ?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
        }


        #region 首页text文本改变字体颜色事件
        /// <summary>
        /// 首页text文本改变字体颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textEditTextChanged(object sender, EventArgs e)
        {
            TextEdit textEdit = sender as TextEdit;

            if (string.IsNullOrEmpty(textEdit.Text))
            {
                textEdit.ForeColor = Color.Gray;
            }
            else
            {
                textEdit.ForeColor = Color.Black;
            }
        }

        #endregion


        #region 首页test按钮事件
        private void btntestconn_Click(object sender, EventArgs e)
        {
            //userid
            if (textuser.EditValue == null || textuser.EditValue.ToString() == "")
            {
                XtraMessageBox.Show(this, "Please input User Id ",
               "Warring", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            else
            {
                userId = textuser.EditValue.ToString();
            }

            //password
            if (textpassword.EditValue == null || textpassword.EditValue.ToString() == "")
            {
                XtraMessageBox.Show(this, "Please input Password ",
               "Warring", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            else
            {
                password = textpassword.EditValue.ToString();
            }

            //string ip = string.Empty;
            if (textip.EditValue == null || textip.EditValue.ToString() == "")
            {
                XtraMessageBox.Show(this, "Please input IP ",
               "Warring", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            else
            {
                ip = textip.EditValue.ToString();
            }

            //string port = string.Empty;
            if (textport.EditValue == null || textport.EditValue.ToString() == "")
            {
                XtraMessageBox.Show(this, "Please input PORT ",
               "Warring", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            else
            {
                port = textport.EditValue.ToString();
            }

            
            if (textservicename.EditValue == null || textservicename.EditValue.ToString() == "")
            {
                XtraMessageBox.Show(this, "Please input Service Name ",
               "Warring", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            else
            {
                serviceName = textservicename.EditValue.ToString();
            }

            /*args.UserID = userId;
            args.Password = password;
            args.IPAddress = ip;
            args.IPPort = port;
            args.ServiceName = serviceName;*/

            string connStr = string.Format( "Data source=(DESCRIPTION=(ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1}))(CONNECT_DATA =(SERVICE_NAME = {2})));User ID={3};Password={4};", ip, port, serviceName, userId, password) ;

            try
            {
                using (OracleConnection conn = new OracleConnection(connStr))
                {
                    conn.Open();
                    Cursor.Current = Cursors.Default;
                    XtraMessageBox.Show("Test Connecting successfully . You can Next");
                    using (OracleCommand cmd = conn.CreateCommand())
                    {
                        /*using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                        }*/

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            welcomeWizardPage1.AllowNext = true;//next按钮是否启用属性

            //自定义messagebox图标的显示
            /*XtraMessageBoxArgs args = new XtraMessageBoxArgs()
            {
                Caption = "Successful",
                Text = "Database connection is successful",
                Buttons = new DialogResult[] { DialogResult.OK },
                Icon =  Icon.ExtractAssociatedIcon("path/to/my/icon.png") ,
            };
            XtraMessageBox.Show(args);
            XtraMessageBox.Show(this, string.Format("DataBase connection is successful"),
               "Successful", MessageBoxButtons.OK, MessageBoxIcon.None );*/
        }
        #endregion

        #region  text控件输入事件
        private void textport_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textip_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
        #endregion



    }


    public class DataBaseInfo { 
    
        public static List<string> datatablelist ;

        public static List<string> procedures;

        public static List<string> databaseinfo;

        static DataBaseInfo()
        {
            datatablelist = new List<string> ();
            datatablelist.Add("RAW_DBA_HIST_ACTIVE_SESS_HISTORY_{0}");
            datatablelist.Add("RAW_DBA_HIST_BUFFER_POOL_STAT_{0}");
            datatablelist.Add("RAW_DBA_HIST_CR_BLOCK_SERVER_{0}");
            datatablelist.Add("RAW_DBA_HIST_CURRENT_BLOCK_SERVER_{0}");
            datatablelist.Add("RAW_DBA_HIST_DLM_MISC_{0}");
            datatablelist.Add("RAW_DBA_HIST_ENQUEUE_STAT_{0}");
            datatablelist.Add("RAW_DBA_HIST_LATCH_MISSES_SUMMARY_{0}");
            datatablelist.Add("RAW_DBA_HIST_LATCH_{0}");
            datatablelist.Add("RAW_DBA_HIST_LIBRARYCACHE_{0}");
            datatablelist.Add("RAW_DBA_HIST_OSSTAT_{0}");
            datatablelist.Add("RAW_DBA_HIST_PARAMETER_{0}");
            datatablelist.Add("RAW_DBA_HIST_PGASTAT_{0}");
            datatablelist.Add("RAW_DBA_HIST_RESOURCE_LIMIT_{0}");
            datatablelist.Add("RAW_DBA_HIST_ROWCACHE_SUMMARY_{0}");
            datatablelist.Add("RAW_DBA_HIST_SEG_STAT_OBJ_{0}");
            datatablelist.Add("RAW_DBA_HIST_SEG_STAT_{0}");
            datatablelist.Add("RAW_DBA_HIST_SGASTAT_{0}");
            datatablelist.Add("RAW_DBA_HIST_SGA_{0}");
            datatablelist.Add("RAW_DBA_HIST_SNAPSHOT_{0}");
            datatablelist.Add("RAW_DBA_HIST_SQLBIND_{0}");
            datatablelist.Add("RAW_DBA_HIST_SQLSTAT_{0}");
            datatablelist.Add("RAW_DBA_HIST_SQLTEXT_{0}");
            datatablelist.Add("RAW_DBA_HIST_SQL_BIND_METADATA_{0}");
            datatablelist.Add("RAW_DBA_HIST_SQL_PLAN_{0}");
            datatablelist.Add("RAW_DBA_HIST_SQL_SUMMARY_{0}");
            datatablelist.Add("RAW_DBA_HIST_SQL_WORKAREA_HSTGRM_{0}");
            datatablelist.Add("RAW_DBA_HIST_SYSMETRIC_SUMMARY_{0}");
            datatablelist.Add("RAW_DBA_HIST_SYSSTAT_{0}");
            datatablelist.Add("RAW_DBA_HIST_SYSTEM_EVENT_{0}");
            datatablelist.Add("RAW_DBA_HIST_SYS_TIME_MODEL_{0}");
            datatablelist.Add("RAW_DBA_HIST_THREAD_{0}");
            datatablelist.Add("RAW_DBA_HIST_WAITSTAT_{0}");
            /*datatablelist.Add("RAW_V_LOG1_{0}");
            datatablelist.Add("RAW_V_LOG2_{0}");
            datatablelist.Add("RAW_V_LOG3_{0}");
            datatablelist.Add("RAW_V_LOG4_{0}");*/

            procedures = new List<string>();
            procedures.Add("GET_DATA");
            procedures.Add("GET_DATA1");
            procedures.Add("GET_DATA2");
            procedures.Add("GET_DATA3");

            databaseinfo = new List<string>();
            databaseinfo.Add("User ID : ");
            databaseinfo.Add("IP : ");
            databaseinfo.Add("IP Port : ");
            databaseinfo.Add("Service Name : ");
            databaseinfo.Add("DB link :");
            databaseinfo.Add("Oracle Edition : 11c");

        }

    }
}
