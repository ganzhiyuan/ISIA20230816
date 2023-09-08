
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
using TAP.UI;
using ISIA.COMMON;

namespace ISIA.UI.MANAGEMENT
{
    public partial class FrmAddDataBase : UIBase
    {
        Int32 ltTimer = 0;
        bool finish = false;


        //定义client
        BizDataClient bs = null;
        BizDataClient bsDatabaseManagement = null;


        //定义argumentPack
        CreateDataBaseArgsPack args = null;
        CodeManagementArgsPack argsDataBase = null;



        //定义常量
        public const string PRIVATE_PERMISSION = "private";
        public const string YES = "yes";
        public const string NO = "no";

        //控制
        private bool isAlreadyCreateDbLink = false;
        private bool isAlreadyCreateTables = false;
        //定义实体类
        public static DbVersionEntity dbVersionEntity = null;
        public DbLinkEntity dbLinkEntity = new DbLinkEntity();


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
            bsDatabaseManagement = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.CodeManagement");


        }

        public FrmAddDataBase(bool allowAnimation, WizardStyle style)
        {
            InitializeComponent();
            //zenm Icon = DevExpress.Utils.ResourceImageHelper.CreateIconFromResourcesEx("DevExpress.XtraWizard.Demos.AppIcon.ico", typeof(frmMain).Assembly);
            wizardControl.AllowTransitionAnimation = allowAnimation;
            wizardControl.WizardStyle = style;
            //memoEdit1.Text = Info.LongText;

        }

        private void CheckedDbLink(WizardPageChangedEventArgs e)
        {
            dbLinkProcessmemoEdit.AppendLine("start...");
            dbLinkProcessmemoEdit.AppendLine("now checking if the DbLink Name Already exists...");
            args = new CreateDataBaseArgsPack();
            //all db_link_name is upper
            args.DBLinkName = dbLinkEntity.DbLink;
            DataSet ds = bs.ExecuteDataSet("GetDBLink", args.getPack());
            if (ds.Tables[0].Rows.Count > 0)
            {
                dbLinkProcessmemoEdit.AppendLine("The DBLink Name already exists");
                dbLinkProcessmemoEdit.AppendLine("Tns description like ");
                dbLinkProcessmemoEdit.AppendLine(ds.Tables[0].Rows[0].Field<string>("HOST"));
                if (XtraMessageBox.Show(this, "DBLink Name already exists! Do you want to drop and recreate the DB link?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    throw new Exception();
                }
                dbLinkProcessmemoEdit.AppendLine("now droping existed dblink... ");
                int res = bs.ExecuteModify("DropDBlink", args.getPack());
                if (res == -1)
                {
                    dbLinkProcessmemoEdit.AppendLine("drop success... ");
                }
            }
        }

        private void CreateDbLink(WizardPageChangedEventArgs e)
        {
            args.Script = dbLinkEntity.generateDbLinkCreationScript();
            int res = bs.ExecuteModify("CreateDBLink", args.getPack());
            if (res == -1)
            {
                e.Page.AllowNext = true;
                dbLinkProcessmemoEdit.AppendLine("create success... ");
            }

        }
        private void GenerateDbLink(WizardPageChangedEventArgs e)
        {
            try
            {
                dbLinkProcessmemoEdit.Clear();
                e.Page.AllowNext = false;
                e.Page.AllowBack = false;
                CheckedDbLink(e);
                CreateDbLink(e);
                isAlreadyCreateDbLink = true;

            }
            catch (Exception ex)
            {
                dbLinkProcessmemoEdit.AppendLine(ex.Message);
            }
            finally
            {
                dbLinkProcessmemoEdit.AppendLine("process end...");
                e.Page.AllowBack = true;
            }


        }


        private void wizardControl1_SelectedPageChangingAsync(object sender, DevExpress.XtraWizard.WizardPageChangingEventArgs e)
        {
            if (e.Page == dbLinkProcesswizardPage)
            {
                //GenerateDbLink(e);
            }
            if (e.Page == wpdatatable)
            {
                listBoxdatatable.Items.Clear();
                List<string> dvalues = DataBaseInfo.datatablelist;
                foreach (string item in dvalues)
                {
                    listBoxdatatable.Items.Add(string.Format(item, dblinkname));
                }

            }

            if (e.Page == wpprocedures)
            {
                listBoxprocedure.Items.Clear();
                List<string> pvalues = DataBaseInfo.procedures;
                foreach (string item in pvalues)
                {
                    listBoxprocedure.Items.Add(string.Format(item, dblinkname));
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
                    if (item.Contains("IP") && !item.Contains("Port"))
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
            if (e.Page == wpbcreate)
            {
                wpbcreate.AllowNext = false;
                wpbcreate.AllowBack = false;
                wpbcreate.DescriptionText = "Program is being created";

                memoinfo.Lines.Initialize();
                memoinfo.AppendLine("Loading...");


                wpbcreate.BeginInvoke(new Action(() =>
               {
                   CreateDBlinkAsync();
                   CreateDataTableAsync();
                   CreateProceduresAsync();
               }));


                //string a = "asdqa";
            }

            if (e.Page == wpfinish)
            {
                wpfinish.AllowBack = false;

            }

        }

        #region CreateDBlinkAsync
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
                string tablename = string.Format(table, dblinkname);
                memoinfo.AppendLine(tablename + " is being created ...... ");

                args.DataTableName = tablename;

                DateTime dateTime = DateTime.Now.Date;
                DateTime dateTime1 = new DateTime(dateTime.Year, dateTime.Month, 1);
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
                else
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

            memoinfo.AppendLine("End of the create datatable ");


        }

        #region Procedures
        public void CreateProceduresAsync()
        {
            memoinfo.AppendLine("Creating  Procedures ...... ");

            foreach (string procedures in DataBaseInfo.procedures)
            {

                string procedurename = string.Format(procedures, dblinkname);
                memoinfo.AppendLine("Procedure " + procedurename + " is being created ...... ");

                args.ProcedureName = procedurename;

                BizDataClient bs1 = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.CreateProceduresManagement");

                if (bs1.ExecuteDataTable("GetProcedures", args.getPack()).Rows.Count > 0)
                {
                    DialogResult result = XtraMessageBox.Show(this, "This Procedure already exists . Choose yes if you still use it ,\n otherwise choose no and will be covered.",
                  "Warring", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {


                    }
                    else if (result == DialogResult.No)
                    {

                        int res1 = bs1.ExecuteModify("DropProcedures", args.getPack());//删除已存在的存储过程

                        if (res1 == -1)
                        {
                            string input = procedurename;
                            int lastUnderscoreIndex = input.LastIndexOf("_");
                            if (lastUnderscoreIndex != -1)
                            {
                                input = input.Substring(0, lastUnderscoreIndex);
                            }

                            string output = input.Replace("_", "");
                            string methodName = "Create" + output;
                            int restable = bs1.ExecuteModify(methodName, args.getPack());//create procedure
                            if (restable != -1) return;



                            memoinfo.AppendLine(procedurename + " cerate successfully .");
                        }
                    }
                }
                else
                {


                    string input = procedurename;
                    int lastUnderscoreIndex = input.LastIndexOf("_");
                    if (lastUnderscoreIndex != -1)
                    {
                        input = input.Substring(0, lastUnderscoreIndex);
                    }

                    string output = input.Replace("_", "");
                    string methodName = "Create" + output;
                    int restable = bs1.ExecuteModify(methodName, args.getPack());//create procedure
                    if (restable != -1) return;

                    memoinfo.AppendLine(procedurename + " cerate successfully .");

                }

            }
            memoinfo.AppendLine("End of the create procedures ");
            memoinfo.AppendLine("End of the program is run");
            wpbcreate.DescriptionText = "End of the program";
            wpbcreate.AllowNext = true;
        }
        #endregion



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

        private void wizardControl1_NextClick_1(object sender, WizardCommandButtonClickEventArgs e)
        {
            if (e.Page == welcomeWizardPage1)
            {
                ValidateWelcomeWizardPrams(e);
            }
            if (e.Page == wizardDBLink)
            {
                ValidateDblinkWizardPrams(e);
                generateDbLinkEntity();
                dblinkConfirmmemoEdit.AppendText(dbLinkEntity.generateDbLinkCreationScript());
            }
            if (e.Page == wpConfirmDbLink)
            {
                if (XtraMessageBox.Show(this, "Do you want to create DBLink?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) e.Handled = true;
            }
            if (e.Page == tableCreationwizardPage)
            {
                if (XtraMessageBox.Show(this, "Do you want to create New Table?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) e.Handled = true;
            }
            if (e.Page == tableCreationwizardPage)
            {
                if (XtraMessageBox.Show(this, "Do you want to create New Table?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) e.Handled = true;
            }
        }
        private void ValidateWelcomeWizardPrams(WizardCommandButtonClickEventArgs e)
        {
            bool canNext = true;
            dbNamelabelControl.ForeColor = Color.Black;
            iplabelControl.ForeColor = Color.Black;
            retentionDaylabelControl.ForeColor = Color.Black;
            userIdlabelControl.ForeColor = Color.Black;
            pwdlabelControl.ForeColor = Color.Black;
            if (string.IsNullOrEmpty(dbNameTextbox.Text))
            {
                dbNamelabelControl.ForeColor = Color.Red;
                canNext = false;
            }
            if (string.IsNullOrEmpty(ipTextBox.Text))
            {
                iplabelControl.ForeColor = Color.Red;
                canNext = false;
            }
            if (string.IsNullOrEmpty(retentionTextBox.Text))
            {
                retentionDaylabelControl.ForeColor = Color.Red;
                canNext = false;
            }
            if (string.IsNullOrEmpty(userIDtextEdit.Text))
            {
                userIdlabelControl.ForeColor = Color.Red;
                canNext = false;
            }
            if (string.IsNullOrEmpty(passwordtextEdit.Text))
            {
                pwdlabelControl.ForeColor = Color.Red;
                canNext = false;
            }
            if (canNext == false)
            {
                e.Handled = true;
            }
            else
            {
                dbLinkTextBox.Text = dbNameTextbox.Text;
            }
        }

        private void ValidateDblinkWizardPrams(WizardCommandButtonClickEventArgs e)
        {
            bool canNext = true;
            dbLinklabelControl.ForeColor = Color.Black;
            acessPermissionlabelControl.ForeColor = Color.Black;
            tnsDescriptionlabelControl.ForeColor = Color.Black;
            if (string.IsNullOrEmpty(dbLinkTextBox.Text))
            {
                dbLinklabelControl1.ForeColor = Color.Red;
                canNext = false;
            }
            if (string.IsNullOrEmpty(permissioncomboBoxEdit.Text))
            {
                acessPermissionlabelControl.ForeColor = Color.Red;
                canNext = false;
            }
            if (string.IsNullOrEmpty(tnsDescriptiontextEdit.Text))
            {
                tnsDescriptionlabelControl.ForeColor = Color.Red;
                canNext = false;
            }
            if (canNext == false)
            {
                e.Handled = true;
            }
        }

        private void generateDbLinkEntity()
        {
            dbLinkEntity.DbLink = dbLinkTextBox.Text.ToUpper();
            dbLinkEntity.DbName = dbNameTextbox.Text;
            dbLinkEntity.Description = descriptionTextBox.Text;
            dbLinkEntity.Ip = ipTextBox.Text;
            dbLinkEntity.Permission = permissioncomboBoxEdit.Text;
            dbLinkEntity.UserId = userIDtextEdit.Text;
            dbLinkEntity.Pwd = passwordtextEdit.Text;
            dbLinkEntity.RetentionDay = retentionTextBox.Text;
            dbLinkEntity.TnsDescription = tnsDescriptiontextEdit.Text;
        }

        public class DbLinkEntity
        {

            private string dbName;
            private string ip;
            private string description;
            private string retentionDay;
            private string userId;
            private string pwd;
            private string dbLink;
            private string permission;
            private string tnsDescription;

            public string DbName { get => dbName; set => dbName = value; }
            public string Ip { get => ip; set => ip = value; }
            public string Description { get => description; set => description = value; }
            public string RetentionDay { get => retentionDay; set => retentionDay = value; }
            public string UserId { get => userId; set => userId = value; }
            public string Pwd { get => pwd; set => pwd = value; }
            public string DbLink { get => dbLink; set => dbLink = value; }
            public string Permission { get => permission; set => permission = value; }
            public string TnsDescription { get => tnsDescription; set => tnsDescription = value; }

            public string generateDbLinkCreationScript()
            {
                string permissionStr = permission.Equals(PRIVATE_PERMISSION) ? "" : permission;
                StringBuilder scriptStr = new StringBuilder("");
                scriptStr.AppendLine($"CREATE {permissionStr} DATABASE LINK {dbLink}");
                scriptStr.AppendLine($" CONNECT TO {userId}");
                scriptStr.AppendLine($" IDENTIFIED BY {pwd}");
                scriptStr.AppendLine($" using");
                scriptStr.AppendLine($" '{tnsDescription}'");
                return scriptStr.ToString();
            }
        }

        public class DbTableCreateEntity
        {
            private string dbName;
            private string tableName;
            private string isPartitioning;
            private string partitionUnit;
            private string retentionMonth;
            private string dataTableSpace;
            private string indexTableSpace;
            private List<ColumnMsg> tableColumns;
            private string later12CIndexString;
            private string pre12CIndexString;

            public const string YES = "Y";
            public const string NO = "N";





            public DbTableCreateEntity()
            {

            }

            public string GenerateTableCreationScript()
            {
                StringBuilder script = new StringBuilder("");
                //drop part
                script.AppendLine($"Drop table ISIA.RAW_{tableName}_{dbName} cascade constraints;");
                //create part
                script.AppendLine($"CREATE TABLE ISIA.RAW_{tableName}_{dbName}");
                script.AppendLine($"(");
                //添加column信息
                foreach (ColumnMsg msg in tableColumns)
                {
                    //type中有些特殊的类型，进行特殊组装
                    string type = null;
                    if (msg.Type.Equals("VARCHAR2"))
                    {
                        type = $"VARCHAR2({msg.DataLength} BYTE)";
                    }
                    else if (msg.Type.Equals("RAW"))
                    {
                        type = $"RAW({msg.DataLength})";
                    }
                    else
                    {
                        type = msg.Type;
                    }

                    //有一些表中自带begin_time和end_time所以这部分的先不加，下面操作中集中处理begin_time，end_time.
                    if (msg.Column.Equals("begin_time".ToUpper()) || msg.Column.Equals("end_time".ToUpper())) continue;
                    script.AppendLine($"{msg.Column} {type},");
                }
                if (later12CIndexString.Contains("begin_time".ToUpper()))
                {
                    script.AppendLine($"BEGIN_TIME DATE,");
                    script.AppendLine($"END_TIME DATE");
                }
                else
                {
                    script.AppendLine($"INSERT_TIME DATE");
                }
                script.AppendLine($")");
                script.AppendLine($"TABLESPACE {dataTableSpace}");
                script.AppendLine($"PCTFREE 10");
                script.AppendLine($"INITRANS 1");
                script.AppendLine($"MAXTRANS 255");
                script.AppendLine("NOCOMPRESS");
                script.AppendLine(@"STORAGE    (
                                         INITIAL          64K
                                         NEXT             1M
                                         MINEXTENTS       1
                                         MAXEXTENTS       UNLIMITED
                                         PCTINCREASE      0
                                         BUFFER_POOL      DEFAULT)");
                AddLobProPertyForSpecialTabel(script);
                script.AppendLine("");

                //partition part
                if (!string.IsNullOrEmpty(isPartitioning) && isPartitioning.Equals(YES))
                {
                    string partitionBy = "begin_time".ToUpper();
                    if (!later12CIndexString.Contains("begin_time".ToUpper()))
                    {
                        partitionBy = "INSERT_TIME".ToUpper();
                    }
                    script.AppendLine($"PARTITION BY RANGE ({partitionBy}) ");
                    script.AppendLine($"INTERVAL (NUMTOYMINTERVAL(1, 'MONTH'))");
                    script.AppendLine("(");
                    for (int i = 0; i < int.Parse(retentionMonth); i++)
                    {
                        DateTime now1 = DateTime.Now;
                        DateTime now = now1.AddMonths(i);
                        DateTime next = now.AddMonths(1);
                        string tabletime_suffix = now.ToString("yyyyMM");
                        string toptime = new DateTime(next.Year, next.Month, 1).ToString("yyyy-MM-dd HH:mm:ss");
                        script.AppendLine($"PARTITION {tableName}_{dbName}_{tabletime_suffix} VALUES LESS THAN (TO_DATE(' {toptime}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))");
                        script.AppendLine($"TABLESPACE {dataTableSpace}");
                        AddLobProPertyForSpecialTabelPartition(script);
                        script.AppendLine($"PCTFREE 10");
                        script.AppendLine($"INITRANS 1");
                        script.AppendLine($"MAXTRANS 255");
                        script.AppendLine("NOCOMPRESS");
                        script.AppendLine(@"STORAGE    (
                                         INITIAL          64K
                                         NEXT             1M
                                         MINEXTENTS       1
                                         MAXEXTENTS       UNLIMITED
                                         PCTINCREASE      0
                                         BUFFER_POOL      DEFAULT),");
                    }
                    script.Remove(script.Length - 3, 1);
                    script.AppendLine("");

                    script.AppendLine(")");
                    script.AppendLine("NOCACHE;");
                    //index part
                    script.AppendLine($"CREATE UNIQUE INDEX ISIA.RAW_{tableName}_{dbName}_PK ON ISIA.RAW_{tableName}_{dbName}");
                    if (dbVersionEntity.IsLater12CIndex)
                    {
                        script.AppendLine($"({Later12CIndexString})");
                    }
                    else
                    {
                        script.AppendLine($"({Pre12CIndexString})");
                    }
                    script.AppendLine($"  TABLESPACE {indexTableSpace}");
                    script.AppendLine(@" PCTFREE    10
                                          INITRANS   2
                                          MAXTRANS   255
                                          STORAGE    (
                                          BUFFER_POOL      DEFAULT ) ");

                    script.AppendLine($"LOCAL (");
                    for (int i = 0; i < int.Parse(retentionMonth); i++)
                    {
                        DateTime now1 = DateTime.Now;
                        DateTime now = now1.AddMonths(i);
                        DateTime next = now.AddMonths(1);
                        string tabletime_suffix = now.ToString("yyyyMM");
                        script.AppendLine($" PARTITION {tableName}_{dbName}_{tabletime_suffix}");
                        script.AppendLine($"INITRANS 2");
                        script.AppendLine($"MAXTRANS 255");
                        script.AppendLine($"TABLESPACE {indexTableSpace}");
                        script.AppendLine("NOCOMPRESS");
                        script.AppendLine(@"STORAGE    (
                                         INITIAL          64K
                                         NEXT             1M
                                         MINEXTENTS       1
                                         MAXEXTENTS       UNLIMITED
                                         BUFFER_POOL      DEFAULT),");
                    }
                    script.Remove(script.Length - 3, 1);
                    script.AppendLine(");");
                }
                else
                {
                    script.AppendLine(";");
                    script.AppendLine($"CREATE UNIQUE INDEX ISIA.RAW_{tableName}_{dbName}_PK ON ISIA.RAW_{tableName}_{dbName}");
                    if (dbVersionEntity.IsLater12CIndex)
                    {
                        script.AppendLine($"({Later12CIndexString})");
                    }
                    else
                    {
                        script.AppendLine($"({Pre12CIndexString})");
                    }
                    script.AppendLine($"  TABLESPACE {indexTableSpace}");
                    script.AppendLine(@" PCTFREE    10
                                          INITRANS   2
                                          MAXTRANS   255
                                          STORAGE    (
                                          BUFFER_POOL      DEFAULT ) ;");
                }
                if (!tableName.Equals("DBA_HIST_SGASTAT"))
                {
                    script.AppendFormat(@"ALTER TABLE ISIA.RAW_{0}_{1} ADD (
                                      CONSTRAINT RAW_{0}_{1}_PK
                                      PRIMARY KEY
                                      ({2})
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE); ", tableName, DbName, dbVersionEntity.IsLater12CIndex ? later12CIndexString : pre12CIndexString);

                }
                return script.ToString();
            }

            private void AddLobProPertyForSpecialTabel(StringBuilder script)
            {
                if (tableName.Equals("DBA_HIST_SQLTEXT"))
                {
                    script.AppendLine("LOB (SQL_TEXT) STORE AS SECUREFILE (");
                    script.AppendLine($"TABLESPACE  {dataTableSpace}");
                    script.AppendLine(@" ENABLE      STORAGE IN ROW
                                         CHUNK       8192
                                         RETENTION
                                         NOCACHE
                                         LOGGING)");
                }
                if (tableName.Equals("DBA_HIST_SQL_PLAN"))
                {
                    script.AppendLine("LOB (OTHER_XML) STORE AS SECUREFILE (");
                    script.AppendLine($"TABLESPACE  {dataTableSpace}");
                    script.AppendLine(@" ENABLE      STORAGE IN ROW
                                         CHUNK       8192
                                         RETENTION
                                         NOCACHE
                                         LOGGING)");
                }
            }

            private void AddLobProPertyForSpecialTabelPartition(StringBuilder script)
            {
                if (tableName.Equals("DBA_HIST_SQLTEXT"))
                {
                    script.AppendFormat(@"LOB (SQL_TEXT) STORE AS SECUREFILE (
                                          TABLESPACE  {0}
                                          ENABLE      STORAGE IN ROW
                                          CHUNK       8192
                                          RETENTION
                                          NOCACHE
                                          LOGGING
                                          STORAGE    (
                                                      INITIAL          104K
                                                      NEXT             1M
                                                      MINEXTENTS       1
                                                      MAXEXTENTS       UNLIMITED
                                                      PCTINCREASE      0
                                                      BUFFER_POOL      DEFAULT))", dataTableSpace);
                }
                if (tableName.Equals("DBA_HIST_SQL_PLAN"))
                {
                    script.AppendFormat(@"LOB (OTHER_XML) STORE AS SECUREFILE (
                                         TABLESPACE  {0}
                                         ENABLE      STORAGE IN ROW
                                         CHUNK       8192
                                         RETENTION
                                         NOCACHE
                                         LOGGING
                                         STORAGE    (
                                                     INITIAL          104K
                                                     NEXT             1M
                                                     MINEXTENTS       1
                                                     MAXEXTENTS       UNLIMITED
                                                     PCTINCREASE      0
                                                     BUFFER_POOL      DEFAULT))", dataTableSpace);
                }
            }

            public static DbTableCreateEntity generateByGridRow(DataRow dr)
            {
                DbTableCreateEntity ele = new DbTableCreateEntity();
                ele.TableName = dr.Field<string>("Table Name");
                ele.IsPartitioning = dr.Field<string>("Partitioning Yn");
                ele.PartitionUnit = dr.Field<string>("Partitioning Unit");
                ele.RetentionMonth = dr.Field<decimal?>("Retention Month") + "";
                ele.dataTableSpace = dr.Field<string>("Data TBSP");
                ele.indexTableSpace = dr.Field<string>("Index TBSP");
                return ele;
            }

            public string TableName { get => tableName; set => tableName = value; }
            public string IsPartitioning { get => isPartitioning; set => isPartitioning = value; }
            public string PartitionUnit { get => partitionUnit; set => partitionUnit = value; }
            public string RetentionMonth { get => retentionMonth; set => retentionMonth = value; }
            public string DataTableSpace { get => dataTableSpace; set => dataTableSpace = value; }
            public string IndexTableSpace { get => indexTableSpace; set => indexTableSpace = value; }
            public List<ColumnMsg> TableColumns { get => tableColumns; set => tableColumns = value; }
            public string DbName { get => dbName; set => dbName = value; }
            public string Later12CIndexString { get => later12CIndexString; set => later12CIndexString = value; }
            public string Pre12CIndexString { get => pre12CIndexString; set => pre12CIndexString = value; }
        }
        public class ColumnMsg
        {
            private string column;
            private string type;
            private Decimal dataLength;

            public string Column { get => column; set => column = value; }
            public string Type { get => type; set => type = value; }
            public Decimal DataLength { get => dataLength; set => dataLength = value; }
        }

        public class DbVersionEntity
        {

            private string dbId;
            private string serviceName;
            private string version;
            private string instanceCount;
            private string isPdb;
            private bool isLater12CIndex;

            public string DBID { get => dbId; set => dbId = value; }
            public string SERVICENAME { get => serviceName; set => serviceName = value; }
            public string VERSION { get => version; set => version = value; }
            public string INSTANCECOUNT { get => instanceCount; set => instanceCount = value; }
            public string ISPDB { get => isPdb; set => isPdb = value; }
            public bool IsLater12CIndex { get => isLater12CIndex; set => isLater12CIndex = value; }

            public void validateVersion()
            {
                if (int.Parse(version.Substring(0, 2)) >= 12)
                {
                    IsLater12CIndex = true;
                    return;
                }
                IsLater12CIndex = false;
            }
        }

        public class DataBaseInfo
        {

            public static List<string> datatablelist;

            public static List<string> procedures;

            public static List<string> databaseinfo;

            static DataBaseInfo()
            {
                datatablelist = new List<string>();
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
                datatablelist.Add("RAW_DBA_HIST_SQLSTAT_{0}");//sqlindex重复
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


                procedures = new List<string>();
                procedures.Add("GET_DATA_{0}");
                /*procedures.Add("GET_DATA1");
                procedures.Add("GET_DATA2");
                procedures.Add("GET_DATA3");*/

                databaseinfo = new List<string>();
                databaseinfo.Add("User ID : ");
                databaseinfo.Add("IP : ");
                databaseinfo.Add("IP Port : ");
                databaseinfo.Add("Service Name : ");
                databaseinfo.Add("DB link :");
                databaseinfo.Add("Oracle Edition : 11c");

            }

        }

        private void wizardControl_SelectedPageChanged(object sender, WizardPageChangedEventArgs e)
        {
            if (e.Page == dbLinkProcesswizardPage && !isAlreadyCreateDbLink)
            {
                GenerateDbLink(e);
            }
            if (e.Page == tableCreationwizardPage)
            {
                gridControl1.DataSource = null;
                ShowToCreateTableMessage(e);
            }
            if (e.Page == tableCreateProcesswizardPage && !isAlreadyCreateTables)
            {
                //Todo根据grid表格生成table
                GenerateTable(e);
            }
        }
        private void ShowToCreateTableMessage(WizardPageChangedEventArgs e)
        {
            DataSet ds = bs.ExecuteDataSet("GetToCreateAwrTableMessage", args.getPack());
            gridControl1.DataSource = ds.Tables[0];
        }
        private async void GenerateTable(WizardPageChangedEventArgs e)
        {
            try
            {
                tableCreateProcessmemoEdit.Clear();
                e.Page.AllowNext = false;
                e.Page.AllowBack = false;
                DataTable gridTable = ((DataView)gridView1.DataSource).ToTable();
                tableCreateProcessmemoEdit.AppendLine("start...");
                tableCreateProcessmemoEdit.AppendLine("now checking version...");
                //获取目标数据库版本
                await CheckTargetDbLinkVersion();
                foreach (DataRow dr in gridTable.Rows)
                {
                    //根据gridrow 获取建表属性
                    DbTableCreateEntity tableEntity = DbTableCreateEntity.generateByGridRow(dr);
                    tableEntity.DbName = dbNameTextbox.Text.ToUpper();
                    CreateDataBaseArgsPack createDataBaseArgsPack = new CreateDataBaseArgsPack();
                    createDataBaseArgsPack.DataTableName = tableEntity.TableName;
                    List<ColumnMsg> columns = await GetData<ColumnMsg>(createDataBaseArgsPack, "GetTableColumnDesc");
                    tableEntity.TableColumns = columns;
                    tableEntity.Pre12CIndexString = bs.ExecuteDataSet("GetTableIndexKey", createDataBaseArgsPack.getPack()).Tables[0].Rows[0].Field<string>("prev12ckeycolumns".ToUpper());
                    tableEntity.Later12CIndexString = bs.ExecuteDataSet("GetTableIndexKey", createDataBaseArgsPack.getPack()).Tables[0].Rows[0].Field<string>("later12ckeycolumns".ToUpper());
                    tableCreateProcessmemoEdit.AppendLine($"generate [{tableEntity.TableName}] creation script...");
                    //生成建表语句
                    createDataBaseArgsPack.Script = tableEntity.GenerateTableCreationScript();
                    tableCreateProcessmemoEdit.AppendLine($"calling server...");
                    //执行命令
                    bs.ExecuteModify("CreateTable", createDataBaseArgsPack.getPack());
                    tableCreateProcessmemoEdit.AppendLine($"table RAW_{tableEntity.TableName}_{tableEntity.DbName} is successfully created.");
                    tableCreateProcessmemoEdit.AppendLine($"");

                }
                e.Page.AllowNext = true;
                isAlreadyCreateTables = true;
            }
            catch (Exception ex)
            {
                tableCreateProcessmemoEdit.AppendLine(ex.Message);
            }
            finally
            {
                tableCreateProcessmemoEdit.AppendLine("process end...");
                e.Page.AllowBack = true;
            }

        }
        private async Task CheckTargetDbLinkVersion()
        {
            CreateDataBaseArgsPack createDataBaseArgsPack = new CreateDataBaseArgsPack();
            createDataBaseArgsPack.DBLinkName = dbLinkEntity.DbLink;
            dbVersionEntity = (await GetData<DbVersionEntity>(createDataBaseArgsPack, "GetDbLinkVersion"))[0];
            dbVersionEntity.validateVersion();
        }
        private async Task<List<T>> GetData<T>(CreateDataBaseArgsPack argsPack = null, string func = "GetDashBoardData") where T : new()
        {
            return await Task.Factory.StartNew(() =>
            {
                DataSet ds = bs.ExecuteDataSet(func, argsPack == null ? new CreateDataBaseArgsPack().getPack() : argsPack.getPack());
                if (ds == null)
                {
                    return new List<T>();
                }
                List<T> restlt = Utils.DataTableToList<T>(ds.Tables[0]);
                return restlt;
            }
            );
        }
    }
}
