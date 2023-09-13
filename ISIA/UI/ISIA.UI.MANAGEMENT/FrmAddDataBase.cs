
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
        static BizDataClient bs = null;
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
        private bool isAlreadyCreateProcedure = false;
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

     

        private void lbcPlay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void wizardControl_FinishClick(object sender, CancelEventArgs e)
        {
            finish = true;

            //((ButtonBase)wizardControl1.Controls.Find("nextButton", true)[0]).Enabled = false;
            //((ButtonBase)wizardControl1.Controls.Find("nextButton", true)[0]).Enabled = false;
            this.Close();
        }

        private void wizardControl_CancelClick(object sender, CancelEventArgs e)
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
            dbLinkEntity.DbName = dbNameTextbox.Text.ToUpper();
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
        public class DbProcedureEntity
        {
            private string dbName;
            private string currentProcedureName;
            private string dataProcedurePrefix = "GET_DATA";
            private string summaryProcedurePrefix = "SUMMARY_WORKLOAD";
            private string dbLinkName;
            private List<string> fetchTableNames;
            private List<string> localTableNames;
            private bool isPdb;
            private bool isLater12c;

            public string GenerateSummaryWorkloadProcedureScript()
            {
                currentProcedureName = summaryProcedurePrefix + "_" + dbName.ToUpper();
                StringBuilder script = new StringBuilder("");
                script.AppendLine ($@"CREATE OR REPLACE PROCEDURE ISIA.SUMMARY_WORKLOAD_{dbName}
(PS_SUMM_PROC_DATE IN VARCHAR2)");
                script.AppendLine($@"IS");
                script.AppendLine($@"--
   LOG_REC     LOG_TAB%ROWTYPE;
--
   V_SUMM_PROC_DATE VARCHAR2(8);
--
BEGIN
   DBMS_OUTPUT.Enable(1000000);
--
   DBMS_APPLICATION_INFO.SET_MODULE('SUMMARY_WORKLOAD_{dbName}', 'Start');
--
   INIT_LOG_REC(LOG_REC);
--
   LOG_REC.PROCESS_NAME := 'SUMMARY_WORKLOAD_{dbName}';
   LOG_REC.START_TIME   := TO_CHAR(SYSTIMESTAMP, 'YYYYMMDDHH24MISSFF6');
   LOG_WRITE(LOG_REC);

   COMMIT;");
                script.AppendLine($@" -- 00 시에 실행될때는 전날의 Summary 작업 수행 
   IF TO_CHAR(SYSDATE, 'HH24') = '00' THEN
      V_SUMM_PROC_DATE := TO_CHAR(SYSDATE -1, 'YYYYMMDD');
   ELSE
      V_SUMM_PROC_DATE := PS_SUMM_PROC_DATE;   
   END IF;
--
   DBMS_APPLICATION_INFO.SET_MODULE('SUMMARY_WORKLOAD_{dbName}', 'Processing');");
                script.AppendLine($@"LOG_REC.TABLE_NAME := 'SUM_WORKLOAD';
   DELETE SUM_WORKLOAD WHERE DBID = (SELECT TO_NUMBER(DBID) FROM TAPCTDATABASE WHERE DBNAME = '{dbName}')
   AND WORKDATE = V_SUMM_PROC_DATE;");
                script.AppendLine($@"INSERT INTO SUM_WORKLOAD 
   (DBID,
    INSTANCE_NUMBER,
    SNAP_ID_MIN,
    WORKDATE,
    BEGIN_TIME,
    END_TIME,
    CPU_UTIL_PCT,
    CPU_UTIL_PCT_MAX,
    LOGICAL_READS_PSEC,
    PHYSICAL_READS_PSEC,
    PHYSICAL_WRITES_PSEC,
    EXECS_PSEC_AVG,
    EXECS_PSEC_MAX,
    USER_CALLS_PSEC,
    HARD_PARSE_CNT_PSEC,
    DB_BLOCK_CHANGES_PSEC,
    SQL_SERVICE_RESPONSE_TIME,
    COMMIT_PSEC_AVG,
    REDO_MB_PSEC_AVG,
    DLM_MB_PSEC,
    NET_MB_TO_CLIENT_PSEC,
    NET_MB_FROM_CLIENT_PSEC,
    NET_MB_FROM_DBLINK_PSEC,
    NET_MB_TO_DBLINK_PSEC,
    EXECUTIONS,
    ELAPSED_TIME,
    CPU_TIME,
    BUFFER_GETS,
    DISK_READS,
    PARSE_CALL)");
                //t1_sysmetric_summary

		script.AppendLine($@"WITH
	    t1_sysmetric_summary
	    AS
	        (  SELECT /*+ MATERIALIZE */
	                  MIN (begin_time)
	                      begin_time,
	                  MAX (end_time)
	                      end_time,
	                  dbid,
	                  snap_id,
	                  s.instance_number AS inst_id,
	                  MIN (NUM_INTERVAL)
	                      NUM_INTERVAL,
	                  SUM (
	                      DECODE (metric_name,
	                              'Host CPU Utilization (%)', average,
	                              0))
	                      CPU_Util_pct,
	                  SUM (
	                      DECODE (metric_name,
	                              'Host CPU Utilization (%)', maxval,
	                              0))
	                      CPU_Util_pct_max,
	                  SUM (
	                      DECODE (metric_name, 'Logical Reads Per Sec', average, 0))
	                      Logical_Reads_psec,
	                  SUM (
	                      DECODE (metric_name,
	                              'Physical Reads Per Sec', average,
	                              0))
	                      Physical_Reads_psec,
	                  SUM (
	                      DECODE (metric_name,
	                              'Physical Writes Per Sec', average,
	                              0))
	                      Physical_Writes_psec,
	                  SUM (DECODE (metric_name, 'Executions Per Sec', average, 0))
	                      Execs_psec_avg,
	                  SUM (DECODE (metric_name, 'Executions Per Sec', maxval, 0))
	                      Execs_psec_max,
	                  SUM (DECODE (metric_name, 'User Calls Per Sec', average, 0))
	                      User_Calls_psec,
	                  SUM (
	                      DECODE (metric_name,
	                              'DB Block Changes Per Sec', average,
	                              0))
	                      DB_Block_Changes_psec,
	                  SUM (
	                      DECODE (metric_name,
	                              'SQL Service Response Time', average,
	                              0))
	                      SQL_Service_Response_Time,
	                  SUM (
	                      DECODE (metric_name, 'User Commits Per Sec', average, 0))
	                      User_Commits_psec,
	                  SUM (
	                      DECODE (metric_name,
	                              'Redo Generated Per Sec', average,
	                              0))
	                      Redo_Generated_psec,
	                  SUM (
	                      DECODE (metric_name,
	                              'Hard Parse Count Per Sec', average,
	                              0))
	                      Hard_Parse_Cnt_psec
	             FROM (SELECT *
	                     FROM RAW_DBA_HIST_SYSMETRIC_SUMMARY_{dbName}
	                    WHERE     1 = 1
	                          --         and sm.dbid = mn.dbid and sm.group_id = mn.group_id  and sm.metric_id = mn.metric_id
	                      AND TO_CHAR(BEGIN_TIME, 'YYYYMMDD') = V_SUMM_PROC_DATE --<< 조회 기간 입력
	                  ) s
	            WHERE 1 = 1
	         --GROUP BY dbid, s.instance_number, snap_id, CON_DBID),
	         GROUP BY dbid, s.instance_number, snap_id),");

                script.AppendLine($@"t1_sysstat
	    AS
	        (SELECT /*+ MATERIALIZE */
	                dbid,
	                INSTANCE_NUMBER AS inst_id,
	                snap_id,
	                begin_time,
	                end_time,
	                (end_time - begin_time) * 60 * 60 * 24
	                snap_time,
	                ROUND (
	                    (  NET_Cnt_Client
	                     - LAG (NET_Cnt_Client, 1)
	                           OVER (PARTITION BY dbid, INSTANCE_NUMBER
	                                 ORDER BY snap_id)))
	                    NET_Cnt_Client,
	                ROUND (
	                    (  NET_B_To_Client
	                     - LAG (NET_B_To_Client, 1)
	                           OVER (PARTITION BY dbid, INSTANCE_NUMBER
	                                 ORDER BY snap_id)))
	                    NET_B_To_Client,
	                ROUND (
	                    (  NET_B_From_Client
	                     - LAG (NET_B_From_Client, 1)
	                           OVER (PARTITION BY dbid, INSTANCE_NUMBER
	                                 ORDER BY snap_id)))
	                    NET_B_From_Client,
	                ROUND (
	                    (  NET_Cnt_DBLink
	                     - LAG (NET_Cnt_DBLink, 1)
	                           OVER (PARTITION BY dbid, INSTANCE_NUMBER
	                                 ORDER BY snap_id)))
	                    NET_Cnt_DBLink,
	                ROUND (
	                    (  NET_B_From_DBLink
	                     - LAG (NET_B_From_DBLink, 1)
	                           OVER (PARTITION BY dbid, INSTANCE_NUMBER
	                                 ORDER BY snap_id)))
	                    NET_B_From_DBLink,
	                ROUND (
	                    (  NET_B_To_DBLink
	                     - LAG (NET_B_To_DBLink, 1)
	                           OVER (PARTITION BY dbid, INSTANCE_NUMBER
	                                 ORDER BY snap_id)))
	                    NET_B_To_DBLink,
	                ROUND (
	                    (  gc_recv
	                     - LAG (gc_recv, 1)
	                           OVER (PARTITION BY dbid, INSTANCE_NUMBER
	                                 ORDER BY snap_id)),
	                    2)
	                    gc_recv,
	                ROUND (
	                    (  gc_send
	                     - LAG (gc_send, 1)
	                           OVER (PARTITION BY dbid, INSTANCE_NUMBER
	                                 ORDER BY snap_id)),
	                    2)
	                    gc_send,
	                ROUND (
	                    (  gcs_msg_send
	                     - LAG (gcs_msg_send, 1)
	                           OVER (PARTITION BY dbid, INSTANCE_NUMBER
	                                 ORDER BY snap_id)),
	                    2)
	                    gcs_msg_send
	           FROM (  SELECT dbid,
	                          snap_id,
	                          INSTANCE_NUMBER,
	                          MIN (begin_time)
	                              AS begin_time,
	                          MAX (end_time)
	                              AS end_time,
	                          SUM (
	                              DECODE (
	                                  stat_name,
	                                  'SQL*Net roundtrips to/from client', VALUE,
	                                  0))
	                              NET_Cnt_Client,
	                          SUM (
	                              DECODE (
	                                  stat_name,
	                                  'bytes sent via SQL*Net to client', VALUE,
	                                  0))
	                              NET_B_To_Client,
	                          SUM (
	                              DECODE (
	                                  stat_name,
	                                  'bytes received via SQL*Net from client', VALUE,
	                                  0))
	                              NET_B_From_Client,
	                          SUM (
	                              DECODE (
	                                  stat_name,
	                                  'SQL*Net roundtrips to/from dblink', VALUE,
	                                  0))
	                              NET_Cnt_DBLink,
	                          SUM (
	                              DECODE (
	                                  stat_name,
	                                  'bytes received via SQL*Net from dblink', VALUE,
	                                  0))
	                              NET_B_From_DBLink,
	                          SUM (
	                              DECODE (
	                                  stat_name,
	                                  'bytes sent via SQL*Net to dblink', VALUE,
	                                  0))
	                              NET_B_To_DBLink,
	                          SUM (
	                              DECODE (stat_name,
	                                      'gc cr blocks received', VALUE,
	                                      'gc current blocks received', VALUE,
	                                      0))
	                              gc_recv          -- Global Cache blocks received
	                                     ,
	                          SUM (
	                              DECODE (stat_name,
	                                      'gc cr blocks served', VALUE,
	                                      'gc current blocks served', VALUE,
	                                      0))
	                              gc_send            -- Global Cache blocks served
	                                     ,
	                          SUM (
	                              DECODE (stat_name,
	                                      'gcs messages sent', VALUE,
	                                      'ges messages sent', VALUE,
	                                      0))
	                              gcs_msg_send            -- GCS/GES messages sent
	                     FROM (SELECT ss.dbid,
	                                  ss.instance_number,
	                                  ss.snap_id,
	                                  ss.VALUE,
	                                  ss.stat_name,
	                                  ss.begin_time,
	                                  ss.end_time
	                             FROM RAW_DBA_HIST_SYSSTAT_{dbName} ss
	                            WHERE 1 = 1
	                                  -- and ss.dbid = nm.dbid and ss.stat_id = nm.stat_id
	                             AND TO_CHAR(BEGIN_TIME, 'YYYYMMDD') = V_SUMM_PROC_DATE --<< 조회 기간 입력
	                          )        
	                    WHERE 1 = 1
	                 GROUP BY dbid, INSTANCE_NUMBER, snap_id) s),");
                //t2_sysmetric_summary
                script.AppendLine($@" t2_sysmetric_summary
	    AS
	        (  SELECT dbid,
	                  inst_id,
	                  MIN (snap_id)
	                      snap_id_min,
	                  TO_CHAR (BEGIN_TIME, 'YYYYMMDD')
	                      workdate,
	                  MIN (BEGIN_TIME)
	                      BEGIN_TIME,
	                  MAX (END_TIME)
	                      END_TIME,
	                  ROUND (AVG (CPU_Util_pct), 2)
	                      CPU_Util_pct,
	                  ROUND (MAX (CPU_Util_pct_max), 2)
	                      CPU_Util_pct_max,
	                  ROUND (AVG (LOGICAL_READS_PSEC))
	                      LOGICAL_READS_PSEC,
	                  ROUND (AVG (Physical_Reads_psec))
	                      PHYSICAL_READS_PSEC,
	                  ROUND (AVG (Physical_Writes_psec))
	                      Physical_Writes_psec,
	                  ROUND (AVG (Execs_psec_avg))
	                      Execs_psec_avg,
	                  ROUND (MAX (Execs_psec_max))
	                      Execs_psec_max,
	                  ROUND (AVG (USER_CALLS_PSEC))
	                      USER_CALLS_PSEC,
	                  ROUND (AVG (DB_BLOCK_CHANGES_PSEC))
	                      DB_BLOCK_CHANGES_PSEC,
	                  ROUND (AVG (SQL_Service_Response_Time), 4)
	                      SQL_Service_Response_Time,
	                  ROUND (AVG (User_Commits_psec), 2)
	                      Commit_psec_avg,
	                  ROUND (AVG (Redo_Generated_psec / 1024 / 1024), 2)
	                      Redo_mb_psec_avg,
	                  ROUND (AVG (Hard_Parse_Cnt_psec), 2)
	                      Hard_Parse_Cnt_psec
	             FROM t1_sysmetric_summary s
	            WHERE 1 = 1
	         GROUP BY dbid, inst_id, TO_CHAR (BEGIN_TIME, 'YYYYMMDD')),");
                //t2_sysstat
                script.AppendLine($@"t2_sysstat
	    AS
	        (  SELECT dbid,
	                  inst_id,
	                  MIN (snap_id)
	                      AS snap_id_min,
	                  TO_CHAR (BEGIN_TIME, 'YYYYMMDD')
	                      workdate,
	                  MIN (BEGIN_TIME)
	                      BEGINE_TIME,
	                  MAX (END_TIME)
	                      END_TIME,
	                  ROUND (
	                      AVG (
	                            ((gc_recv + gc_send) * 8192 + gcs_msg_send * 200)
	                          / 1024
	                          / 1024
	                          / SNAP_TIME),
	                      3)
	                      DLM_MB_psec,
	                  ROUND (AVG (NET_B_To_Client / 1024 / 1024 / SNAP_TIME), 3)
	                      NET_MB_To_Client_psec,
	                  ROUND (AVG (NET_B_From_Client / 1024 / 1024 / SNAP_TIME), 3)
	                      NET_MB_From_Client_psec,
	                  ROUND (AVG (NET_B_From_DBLink / 1024 / 1024 / SNAP_TIME), 3)
	                      NET_MB_From_DBLink_psec,
	                  ROUND (AVG (NET_B_To_DBLink / 1024 / 1024 / SNAP_TIME), 3)
	                      NET_MB_To_DBLink_psec
	             FROM t1_sysstat s
	            WHERE 1 = 1
	         GROUP BY dbid, inst_id, TO_CHAR (BEGIN_TIME, 'YYYYMMDD')),");
                script.AppendLine($@" t2_sqlstat
	      AS
	      (
			      SELECT dbid, INSTANCE_NUMBER inst_id, TO_CHAR (BEGIN_TIME, 'YYYYMMDD') WORKDATE,
					        SUM(EXECUTIONS_DELTA) EXECUTIONS, 
					        ROUND(SUM(ELAPSED_TIME_DELTA) / 1000000, 3) ELAPSED_TIME, 
					        ROUND(SUM(CPU_TIME_DELTA) / 1000000, 3) CPU_TIME, 
					        SUM(BUFFER_GETS_DELTA) BUFFER_GETS, 
					        SUM(DISK_READS_DELTA) DISK_READS,
					        SUM(PARSE_CALLS_DELTA) PARSE_CALL
						FROM RAW_DBA_HIST_SQLSTAT_{dbName}
						WHERE 1 = 1
						AND TO_CHAR(BEGIN_TIME, 'YYYYMMDD') = V_SUMM_PROC_DATE
						GROUP BY dbid, INSTANCE_NUMBER, TO_CHAR (BEGIN_TIME, 'YYYYMMDD')
	      )   ");
                script.AppendLine($@" SELECT sm.dbid,
			         sm.inst_id as INSTANCE_NUMBER,
			         sm.snap_id_min,
			         sm.workdate     AS workdate,
			         sm.begin_time,
			         sm.end_time,
			         sm.CPU_Util_pct,
			         sm.CPU_Util_pct_max,
			         sm.LOGICAL_READS_PSEC,
			         sm.PHYSICAL_READS_PSEC,
			         sm.Physical_Writes_psec,
			         sm.Execs_psec_avg,
			         sm.Execs_psec_max,
			         sm.USER_CALLS_PSEC,
			         sm.Hard_Parse_Cnt_psec,
			         sm.DB_BLOCK_CHANGES_PSEC,
			         sm.SQL_Service_Response_Time,
			         sm.Commit_psec_avg,
			         sm.Redo_mb_psec_avg,
			         ss.DLM_MB_psec,
			         ss.NET_MB_To_Client_psec,
			         ss.NET_MB_From_Client_psec,
			         ss.NET_MB_From_DBLink_psec,
			         ss.NET_MB_To_DBLink_psec,
			         qs.EXECUTIONS, 
               qs.ELAPSED_TIME, 
               qs.CPU_TIME, 
               qs.BUFFER_GETS, 
               qs.DISK_READS,
               qs.PARSE_CALL
			    FROM t2_sysmetric_summary sm, t2_sysstat ss, t2_sqlstat qs
			   WHERE sm.dbid = ss.dbid
	         AND sm.inst_id = ss.inst_id
	         AND sm.workdate = ss.workdate
	         AND sm.dbid = qs.dbid
	         AND sm.inst_id = qs.inst_id
	         AND sm.workdate = qs.workdate;");
                script.AppendLine($@"LOG_REC.END_TIME     := TO_CHAR(SYSTIMESTAMP, 'YYYYMMDDHH24MISSFF6');
   LOG_REC.SUCCESS_FLAG := 'Y';
   LOG_REC.RESV_FIELD1 := V_SUMM_PROC_DATE;
   LOG_WRITE(LOG_REC);
   
   COMMIT;
--
   DBMS_APPLICATION_INFO.SET_MODULE('SUMMARY_WORKLOAD_{dbName}', 'End');
--   
EXCEPTION
   WHEN OTHERS
   THEN
      ROLLBACK;

      LOG_REC.END_TIME     := TO_CHAR(SYSTIMESTAMP, 'YYYYMMDDHH24MISSFF6');
      LOG_REC.SUCCESS_FLAG := 'N';
      LOG_REC.ERROR_CODE   := SQLCODE ;
      LOG_REC.ERROR_MSG    := SUBSTRB(SQLERRM,1,4000);
      LOG_WRITE(LOG_REC);

      COMMIT;
END SUMMARY_WORKLOAD_{dbName};");
                return script.ToString();
            }
            public string GenerateGetDataProcedureScript()
            {
                currentProcedureName = dataProcedurePrefix + "_" + dbName.ToUpper();
                StringBuilder script = new StringBuilder("");
                //注意用户名固定，isia为用户名
                script.AppendLine($"CREATE OR REPLACE PROCEDURE ISIA.{currentProcedureName}");
                script.AppendLine(@"(PS_INTERVAL_TYPE IN VARCHAR2,
                                   PI_INTERVAL_PERIOD IN NUMBER,
                                   PD_PROC_END_TIME IN DATE)");
                script.AppendLine(@"IS");
                script.AppendFormat(@"TYPE SEG_STAT_TYPE IS REF CURSOR;
                                     SEG_STAT_CUR          SEG_STAT_TYPE;
                                     SEG_STAT_REC          RAW_DBA_HIST_SEG_STAT_{0}%ROWTYPE;
                                  --
                                     TYPE SQLSTAT_TYPE IS REF CURSOR;
                                     SQLSTAT_CUR          SQLSTAT_TYPE;
                                     SQLSTAT_REC          RAW_DBA_HIST_SQLSTAT_{0}%ROWTYPE;
                                  --
                                     LOG_REC     LOG_TAB%ROWTYPE;
                                  --
                                     MIN_SNAP_ID RAW_DBA_HIST_SNAPSHOT_{0}.SNAP_ID%TYPE;
                                     MAX_SNAP_ID RAW_DBA_HIST_SNAPSHOT_{0}.SNAP_ID%TYPE;
                                     V_START_TIME DATE;
                                     V_END_TIME   DATE;
                                  --
                                     V_SQL VARCHAR2(1000);", dbName);
                script.AppendLine($"");
                script.AppendLine(@"BEGIN");
                script.AppendFormat(@"DBMS_OUTPUT.Enable(1000000);
                                --
                                   DBMS_APPLICATION_INFO.SET_MODULE('{0}', 'Start');
                                --
                                   INIT_LOG_REC(LOG_REC);
                                --
                                   LOG_REC.PROCESS_NAME := '{0}';
                                   LOG_REC.START_TIME   := TO_CHAR(SYSTIMESTAMP, 'YYYYMMDDHH24MISSFF6');
                                   LOG_WRITE(LOG_REC);", currentProcedureName);
                script.AppendLine(@"COMMIT;");
                script.AppendLine(@"V_SQL := 'SELECT ';
                                   IF PS_INTERVAL_TYPE = 'D' THEN
                                      V_SQL := V_SQL || 'TRUNC(:END_TIME, ''DD'') - INTERVAL ''' || PI_INTERVAL_PERIOD || ''' DAY AS from_time, TRUNC(:END_TIME, ''DD'') AS to_time';
                                   ELSIF PS_INTERVAL_TYPE = 'H' THEN
                                      V_SQL := V_SQL || 'TRUNC(:END_TIME, ''HH24'') - INTERVAL ''' || PI_INTERVAL_PERIOD || ''' HOUR AS from_time, TRUNC(:END_TIME, ''HH24'') AS to_time';
                                   ELSE
                                      RAISE_APPLICATION_ERROR(-20011, 'Input Parameter(INTERVAL_TYPE) Exception');
                                   END IF;
                                   
                                   V_SQL := V_SQL || ' FROM DUAL';      
                                   
                                   EXECUTE IMMEDIATE V_SQL INTO V_START_TIME, V_END_TIME USING PD_PROC_END_TIME, PD_PROC_END_TIME;
                                   
                                   DBMS_OUTPUT.PUT_LINE(to_char(V_START_TIME, 'YYYYMMDDHH24MISS') || '##' || to_char(V_END_TIME, 'YYYYMMDDHH24MISS'));");
                if (isPdb)
                {
                    script.AppendLine(@"SELECT MIN(SNAP_ID), MAX(SNAP_ID) INTO MIN_SNAP_ID, MAX_SNAP_ID");
                    script.AppendLine($"FROM AWR_PDB_SNAPSHOT@{dbLinkName}");
                    script.AppendLine(@"WHERE BEGIN_INTERVAL_TIME >= V_START_TIME
                                      AND BEGIN_INTERVAL_TIME  < V_END_TIME;");
                }
                else
                {
                    script.AppendLine(@"SELECT MIN(SNAP_ID), MAX(SNAP_ID) INTO MIN_SNAP_ID, MAX_SNAP_ID");
                    script.AppendLine($"FROM DBA_HIST_SNAPSHOT@{dbLinkName}");
                    script.AppendLine(@"WHERE BEGIN_INTERVAL_TIME >= V_START_TIME
                                      AND BEGIN_INTERVAL_TIME  < V_END_TIME;");
                }
                script.AppendLine($"DBMS_APPLICATION_INFO.SET_MODULE('{currentProcedureName}', 'Processing');");
                //insert
                for (int i = 0; i < fetchTableNames.Count; i++)
                {
                    string fetchTableName = fetchTableNames[i];
                    string localTableName = localTableNames[i];
                    script.AppendLine($"   LOG_REC.TABLE_NAME := '{fetchTableName}';");
                    script.AppendLine($"DELETE RAW_{localTableName}_{dbName} WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID;");
                    script.AppendLine(GenerateInsertSting(localTableName, fetchTableName));
                    script.AppendLine($"DBMS_OUTPUT.PUT_LINE('Table {i + 1} End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6')); ");
                }
                //merge
                if (isLater12c)
                {
                    script.AppendFormat(@"  OPEN SEG_STAT_CUR FOR
      SELECT DISTINCT DBID, TS#, OBJ#, DATAOBJ#, CON_DBID
        FROM RAW_DBA_HIST_SEG_STAT_{0}
       WHERE SNAP_ID >= MIN_SNAP_ID
         AND SNAP_ID <= MAX_SNAP_ID;", DbName);
                    script.AppendLine("");
                    script.AppendLine("LOOP");
                    script.AppendLine(@" FETCH SEG_STAT_CUR INTO SEG_STAT_REC.DBID, SEG_STAT_REC.TS#, SEG_STAT_REC.OBJ#, SEG_STAT_REC.DATAOBJ#, SEG_STAT_REC.CON_DBID;
      EXIT WHEN SEG_STAT_CUR%NOTFOUND;");
                    string stat_obj = isPdb ? "AWR_PDB_SEG_STAT_OBJ" : "DBA_HIST_SEG_STAT_OBJ";
                    script.AppendLine($"LOG_REC.TABLE_NAME := '{stat_obj}';");
                    script.AppendFormat(@"MERGE INTO ISIA.RAW_DBA_HIST_SEG_STAT_OBJ_{0} d

                USING(SELECT *
                         FROM {2}@{1}

                        WHERE DBID = SEG_STAT_REC.DBID

                          AND TS# = SEG_STAT_REC.TS#
		                  AND OBJ# = SEG_STAT_REC.OBJ#
		                  AND DATAOBJ# = SEG_STAT_REC.DATAOBJ#
		                  AND CON_DBID = SEG_STAT_REC.CON_DBID) s

                   ON(d.DBID = s.DBID

                       AND d.TS# = s.TS#
		               AND d.OBJ# = s.OBJ#
		               AND d.DATAOBJ# = s.DATAOBJ#
		               AND d.CON_DBID = s.CON_DBID)

                       WHEN NOT MATCHED

                       THEN

                           INSERT(DBID,
                                                     TS#,
													 OBJ#,
													 DATAOBJ#,
													 OWNER,
                                                     OBJECT_NAME,
                                                     SUBOBJECT_NAME,
                                                     OBJECT_TYPE,
                                                     TABLESPACE_NAME,
                                                     PARTITION_TYPE,
                                                     CON_DBID,
                                                     CON_ID,
                                                     INSERT_TIME)

                           VALUES(S.DBID,
                                                     S.TS#,
													 S.OBJ#,
													 S.DATAOBJ#,
													 S.OWNER,
                                                     S.OBJECT_NAME,
                                                     S.SUBOBJECT_NAME,
                                                     S.OBJECT_TYPE,
                                                     S.TABLESPACE_NAME,
                                                     S.PARTITION_TYPE,
                                                     S.CON_DBID,
                                                     S.CON_ID,
                                                     SYSDATE);

                END LOOP;
                CLOSE SEG_STAT_CUR;

                DBMS_OUTPUT.PUT_LINE('Table 29 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6')); ", dbName, dbLinkName, stat_obj);
                    script.AppendLine();
                    string sqlStatTable = isPdb ? "AWR_PDB_SQLSTAT" : "DBA_HIST_SQLSTAT";
                    script.AppendLine($@" OPEN SQLSTAT_CUR FOR
      SELECT DISTINCT DBID, SQL_ID, CON_DBID
        FROM {sqlStatTable}@{DbLinkName}
       WHERE SNAP_ID >= MIN_SNAP_ID
         AND SNAP_ID <= MAX_SNAP_ID;

   LOOP
      FETCH SQLSTAT_CUR INTO SQLSTAT_REC.DBID, SQLSTAT_REC.SQL_ID, SQLSTAT_REC.CON_DBID;
      EXIT WHEN SQLSTAT_CUR%NOTFOUND;");
                    string sqltext = isPdb ? "AWR_PDB_SQLTEXT" : "DBA_HIST_SQLTEXT";
                    script.AppendLine($" LOG_REC.TABLE_NAME := '{sqltext}';");
                    script.AppendFormat(@" MERGE INTO ISIA.RAW_DBA_HIST_SQLTEXT_{0} d
	         USING (SELECT DBID,
	                       SQL_ID,
	                       SQL_TEXT,
	                       COMMAND_TYPE,
	                       CON_DBID,
	                       CON_ID
	                  FROM {2}@{1}
	                 WHERE DBID = SQLSTAT_REC.DBID
	                   AND SQL_ID = SQLSTAT_REC.SQL_ID
	                   AND CON_DBID = SQLSTAT_REC.CON_DBID) s
	            ON (d.SQL_ID = s.SQL_ID AND d.DBID = s.DBID AND d.CON_DBID = s.CON_DBID)
	    WHEN NOT MATCHED
	    THEN
	        INSERT     (DBID,
	                    SQL_ID,
	                    SQL_TEXT,
	                    COMMAND_TYPE,
	                    CON_DBID,
	                    CON_ID,
	                    INSERT_TIME)
	            VALUES (s.DBID,
	                    s.SQL_ID,
	                    s.SQL_TEXT,
	                    s.COMMAND_TYPE,
	                    s.CON_DBID,
	                    s.CON_ID,
	                    SYSDATE);", dbName, dbLinkName, sqltext);
                    script.AppendLine();
                    string sqlbind = isPdb ? "AWR_PDB_SQL_BIND_METADATA" : "DBA_HIST_SQL_BIND_METADATA";
                    script.AppendLine($" LOG_REC.TABLE_NAME := '{sqlbind}';");
                    script.AppendFormat(@" MERGE INTO ISIA.RAW_DBA_HIST_SQL_BIND_METADATA_{0} d
	    USING (
	      Select
	        DBID,
	        SQL_ID,
	        NAME,
	        POSITION,
	        DUP_POSITION,
	        DATATYPE,
	        DATATYPE_STRING,
	        CHARACTER_SID,
	        PRECISION,
	        SCALE,
	        MAX_LENGTH,
	        CON_DBID,
	        CON_ID
	      FROM {2}@{1}
	      WHERE DBID = SQLSTAT_REC.DBID
	        AND SQL_ID = SQLSTAT_REC.SQL_ID
	        AND CON_DBID = SQLSTAT_REC.CON_DBID) s
	    ON
	      (d.DBID = s.DBID and
	      d.SQL_ID = s.SQL_ID and
	      d.POSITION = s.POSITION and
	      d.CON_DBID = s.CON_DBID)
	    WHEN NOT MATCHED
	    THEN
	    INSERT (
	      DBID, SQL_ID, NAME,
	      POSITION, DUP_POSITION, DATATYPE,
	      DATATYPE_STRING, CHARACTER_SID, PRECISION,
	      SCALE, MAX_LENGTH, CON_DBID,
	      CON_ID,
	      INSERT_TIME)
	    VALUES (
	      s.DBID, s.SQL_ID, s.NAME,
	      s.POSITION, s.DUP_POSITION, s.DATATYPE,
	      s.DATATYPE_STRING, s.CHARACTER_SID, s.PRECISION,
	      s.SCALE, s.MAX_LENGTH, s.CON_DBID,
	      s.CON_ID,
	      SYSDATE);", dbName, dbLinkName, sqlbind);
                    script.AppendLine();
                    string sqlplan = isPdb ? "AWR_PDB_SQL_PLAN" : "DBA_HIST_SQL_PLAN";
                    script.AppendLine($" LOG_REC.TABLE_NAME := '{sqlplan}';");
                    script.AppendFormat(@" MERGE INTO ISIA.RAW_DBA_HIST_SQL_PLAN_{0} d
	     USING (SELECT DBID,
	                   SQL_ID,
	                   PLAN_HASH_VALUE,
	                   ID,
	                   OPERATION,
	                   OPTIONS,
	                   OBJECT_NODE,
	                   OBJECT#,
	                   OBJECT_OWNER,
	                   OBJECT_NAME,
	                   OBJECT_ALIAS,
	                   OBJECT_TYPE,
	                   OPTIMIZER,
	                   PARENT_ID,
	                   DEPTH,
	                   POSITION,
	                   SEARCH_COLUMNS,
	                   COST,
	                   CARDINALITY,
	                   BYTES,
	                   OTHER_TAG,
	                   PARTITION_START,
	                   PARTITION_STOP,
	                   PARTITION_ID,
	                   OTHER,
	                   DISTRIBUTION,
	                   CPU_COST,
	                   IO_COST,
	                   TEMP_SPACE,
	                   ACCESS_PREDICATES,
	                   FILTER_PREDICATES,
	                   PROJECTION,
	                   TIME,
	                   QBLOCK_NAME,
	                   REMARKS,
	                   TIMESTAMP,
	                   OTHER_XML,
	                   CON_DBID,
	                   CON_ID
	              FROM {2}@{1}
	              WHERE DBID = SQLSTAT_REC.DBID
	                AND SQL_ID = SQLSTAT_REC.SQL_ID
	                AND CON_DBID = SQLSTAT_REC.CON_DBID) s
	        ON (    d.DBID = s.DBID
	            AND d.SQL_ID = s.SQL_ID
	            AND d.PLAN_HASH_VALUE = s.PLAN_HASH_VALUE
	            AND d.ID = s.ID
	            AND d.CON_DBID = s.CON_DBID)
			 WHEN NOT MATCHED
			 THEN
			     INSERT     (DBID,
			                 SQL_ID,
			                 PLAN_HASH_VALUE,
			                 ID,
			                 OPERATION,
			                 OPTIONS,
			                 OBJECT_NODE,
			                 OBJECT#,
			                 OBJECT_OWNER,
			                 OBJECT_NAME,
			                 OBJECT_ALIAS,
			                 OBJECT_TYPE,
			                 OPTIMIZER,
			                 PARENT_ID,
			                 DEPTH,
			                 POSITION,
			                 SEARCH_COLUMNS,
			                 COST,
			                 CARDINALITY,
			                 BYTES,
			                 OTHER_TAG,
			                 PARTITION_START,
			                 PARTITION_STOP,
			                 PARTITION_ID,
			                 OTHER,
			                 DISTRIBUTION,
			                 CPU_COST,
			                 IO_COST,
			                 TEMP_SPACE,
			                 ACCESS_PREDICATES,
			                 FILTER_PREDICATES,
			                 PROJECTION,
			                 TIME,
			                 QBLOCK_NAME,
			                 REMARKS,
			                 TIMESTAMP,
			                 OTHER_XML,
			                 CON_DBID,
			                 CON_ID,
			                 INSERT_TIME)
			         VALUES (s.DBID,
			                 s.SQL_ID,
			                 s.PLAN_HASH_VALUE,
			                 s.ID,
			                 s.OPERATION,
			                 s.OPTIONS,
			                 s.OBJECT_NODE,
			                 s.OBJECT#,
			                 s.OBJECT_OWNER,
			                 s.OBJECT_NAME,
			                 s.OBJECT_ALIAS,
			                 s.OBJECT_TYPE,
			                 s.OPTIMIZER,
			                 s.PARENT_ID,
			                 s.DEPTH,
			                 s.POSITION,
			                 s.SEARCH_COLUMNS,
			                 s.COST,
			                 s.CARDINALITY,
			                 s.BYTES,
			                 s.OTHER_TAG,
			                 s.PARTITION_START,
			                 s.PARTITION_STOP,
			                 s.PARTITION_ID,
			                 s.OTHER,
			                 s.DISTRIBUTION,
			                 s.CPU_COST,
			                 s.IO_COST,
			                 s.TEMP_SPACE,
			                 s.ACCESS_PREDICATES,
			                 s.FILTER_PREDICATES,
			                 s.PROJECTION,
			                 s.TIME,
			                 s.QBLOCK_NAME,
			                 s.REMARKS,
			                 s.TIMESTAMP,
			                 s.OTHER_XML,
			                 s.CON_DBID,
			                 s.CON_ID,
			                 SYSDATE);
      
   END LOOP;
   CLOSE SQLSTAT_CUR;
   
   DBMS_OUTPUT.PUT_LINE('Table 30~32 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));
   
   LOG_REC.END_TIME     := TO_CHAR(SYSTIMESTAMP, 'YYYYMMDDHH24MISSFF6');
   LOG_REC.SUCCESS_FLAG := 'Y';
   LOG_REC.RESV_FIELD1 := TO_CHAR(V_START_TIME, 'YYYY/MM/DD HH24:MI:SS');
   LOG_REC.RESV_FIELD2 := TO_CHAR(V_END_TIME, 'YYYY/MM/DD HH24:MI:SS');
   LOG_REC.RESV_FIELD3 := TO_CHAR(MIN_SNAP_ID);
   LOG_REC.RESV_FIELD4 := TO_CHAR(MAX_SNAP_ID);
   LOG_WRITE(LOG_REC);
   
   COMMIT;", dbName, dbLinkName, sqlplan);
                }
                //11g merge
                else
                {
                    script.AppendFormat(@"  OPEN SEG_STAT_CUR FOR
      SELECT DISTINCT DBID, TS#, OBJ#, DATAOBJ#, CON_DBID
        FROM RAW_DBA_HIST_SEG_STAT_{0}
       WHERE SNAP_ID >= MIN_SNAP_ID
         AND SNAP_ID <= MAX_SNAP_ID;", DbName);
                    script.AppendLine("");
                    script.AppendLine("LOOP");
                    script.AppendLine(@" FETCH SEG_STAT_CUR INTO SEG_STAT_REC.DBID, SEG_STAT_REC.TS#, SEG_STAT_REC.OBJ#, SEG_STAT_REC.DATAOBJ#, SEG_STAT_REC.CON_DBID;
      EXIT WHEN SEG_STAT_CUR%NOTFOUND;");
                    string stat_obj = isPdb ? "AWR_PDB_SEG_STAT_OBJ" : "DBA_HIST_SEG_STAT_OBJ";
                    script.AppendLine($"LOG_REC.TABLE_NAME := '{stat_obj}';");
                    script.AppendFormat(@"MERGE INTO ISIA.RAW_DBA_HIST_SEG_STAT_OBJ_{0} d

                USING(SELECT *
                         FROM {2}@{1}

                        WHERE DBID = SEG_STAT_REC.DBID

                          AND TS# = SEG_STAT_REC.TS#
		                  AND OBJ# = SEG_STAT_REC.OBJ#
		                  AND DATAOBJ# = SEG_STAT_REC.DATAOBJ#
		                  ) s

                   ON(d.DBID = s.DBID

                       AND d.TS# = s.TS#
		               AND d.OBJ# = s.OBJ#
		               AND d.DATAOBJ# = s.DATAOBJ#
		               )

                       WHEN NOT MATCHED

                       THEN

                           INSERT(DBID,
                                                     TS#,
													 OBJ#,
													 DATAOBJ#,
													 OWNER,
                                                     OBJECT_NAME,
                                                     SUBOBJECT_NAME,
                                                     OBJECT_TYPE,
                                                     TABLESPACE_NAME,
                                                     PARTITION_TYPE,
                                                   
                                                     INSERT_TIME)

                           VALUES(S.DBID,
                                                     S.TS#,
													 S.OBJ#,
													 S.DATAOBJ#,
													 S.OWNER,
                                                     S.OBJECT_NAME,
                                                     S.SUBOBJECT_NAME,
                                                     S.OBJECT_TYPE,
                                                     S.TABLESPACE_NAME,
                                                     S.PARTITION_TYPE,

                                                     SYSDATE);

                END LOOP;
                CLOSE SEG_STAT_CUR;

                DBMS_OUTPUT.PUT_LINE('Table 29 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6')); ", dbName, dbLinkName, stat_obj);
                    script.AppendLine();
                    string sqlStatTable = isPdb ? "AWR_PDB_SQLSTAT" : "DBA_HIST_SQLSTAT";
                    script.AppendLine($@" OPEN SQLSTAT_CUR FOR
      SELECT DISTINCT DBID, SQL_ID, CON_DBID
        FROM {sqlStatTable}@{DbLinkName}
       WHERE SNAP_ID >= MIN_SNAP_ID
         AND SNAP_ID <= MAX_SNAP_ID;

   LOOP
      FETCH SQLSTAT_CUR INTO SQLSTAT_REC.DBID, SQLSTAT_REC.SQL_ID, SQLSTAT_REC.CON_DBID;
      EXIT WHEN SQLSTAT_CUR%NOTFOUND;");
                    string sqltext = isPdb ? "AWR_PDB_SQLTEXT" : "DBA_HIST_SQLTEXT";
                    script.AppendLine($" LOG_REC.TABLE_NAME := '{sqltext}';");
                    script.AppendFormat(@" MERGE INTO ISIA.RAW_DBA_HIST_SQLTEXT_{0} d
	         USING (SELECT DBID,
	                       SQL_ID,
	                       SQL_TEXT,
	                       COMMAND_TYPE,
	                     
	                  FROM {2}@{1}
	                 WHERE DBID = SQLSTAT_REC.DBID
	                   AND SQL_ID = SQLSTAT_REC.SQL_ID
	                   ) s
	            ON (d.SQL_ID = s.SQL_ID AND d.DBID = s.DBID )
	    WHEN NOT MATCHED
	    THEN
	        INSERT     (DBID,
	                    SQL_ID,
	                    SQL_TEXT,
	                    COMMAND_TYPE,
	                    
	                    INSERT_TIME)
	            VALUES (s.DBID,
	                    s.SQL_ID,
	                    s.SQL_TEXT,
	                    s.COMMAND_TYPE,
	                   
	                    SYSDATE);", dbName, dbLinkName, sqltext);
                    script.AppendLine();
                    string sqlbind = isPdb ? "AWR_PDB_SQL_BIND_METADATA" : "DBA_HIST_SQL_BIND_METADATA";
                    script.AppendLine($" LOG_REC.TABLE_NAME := '{sqlbind}';");
                    script.AppendFormat(@" MERGE INTO ISIA.RAW_DBA_HIST_SQL_BIND_METADATA_{0} d
	    USING (
	      Select
	        DBID,
	        SQL_ID,
	        NAME,
	        POSITION,
	        DUP_POSITION,
	        DATATYPE,
	        DATATYPE_STRING,
	        CHARACTER_SID,
	        PRECISION,
	        SCALE,
	        MAX_LENGTH
	       
	      FROM {2}@{1}
	      WHERE DBID = SQLSTAT_REC.DBID
	        AND SQL_ID = SQLSTAT_REC.SQL_ID
	        ) s
	    ON
	      (d.DBID = s.DBID and
	      d.SQL_ID = s.SQL_ID and
	      d.POSITION = s.POSITION
	     )
	    WHEN NOT MATCHED
	    THEN
	    INSERT (
	      DBID, SQL_ID, NAME,
	      POSITION, DUP_POSITION, DATATYPE,
	      DATATYPE_STRING, CHARACTER_SID, PRECISION,
	      SCALE, MAX_LENGTH, 
	      INSERT_TIME)
	    VALUES (
	      s.DBID, s.SQL_ID, s.NAME,
	      s.POSITION, s.DUP_POSITION, s.DATATYPE,
	      s.DATATYPE_STRING, s.CHARACTER_SID, s.PRECISION,
	      s.SCALE, s.MAX_LENGTH,
	      SYSDATE);", dbName, dbLinkName, sqlbind);
                    script.AppendLine();
                    string sqlplan = isPdb ? "AWR_PDB_SQL_PLAN" : "DBA_HIST_SQL_PLAN";
                    script.AppendLine($" LOG_REC.TABLE_NAME := '{sqlplan}';");
                    script.AppendFormat(@" MERGE INTO ISIA.RAW_DBA_HIST_SQL_PLAN_{0} d
	     USING (SELECT DBID,
	                   SQL_ID,
	                   PLAN_HASH_VALUE,
	                   ID,
	                   OPERATION,
	                   OPTIONS,
	                   OBJECT_NODE,
	                   OBJECT#,
	                   OBJECT_OWNER,
	                   OBJECT_NAME,
	                   OBJECT_ALIAS,
	                   OBJECT_TYPE,
	                   OPTIMIZER,
	                   PARENT_ID,
	                   DEPTH,
	                   POSITION,
	                   SEARCH_COLUMNS,
	                   COST,
	                   CARDINALITY,
	                   BYTES,
	                   OTHER_TAG,
	                   PARTITION_START,
	                   PARTITION_STOP,
	                   PARTITION_ID,
	                   OTHER,
	                   DISTRIBUTION,
	                   CPU_COST,
	                   IO_COST,
	                   TEMP_SPACE,
	                   ACCESS_PREDICATES,
	                   FILTER_PREDICATES,
	                   PROJECTION,
	                   TIME,
	                   QBLOCK_NAME,
	                   REMARKS,
	                   TIMESTAMP,
	                   OTHER_XML
	                   
	              FROM {2}@{1}
	              WHERE DBID = SQLSTAT_REC.DBID
	                AND SQL_ID = SQLSTAT_REC.SQL_ID
	                ) s
	        ON (    d.DBID = s.DBID
	            AND d.SQL_ID = s.SQL_ID
	            AND d.PLAN_HASH_VALUE = s.PLAN_HASH_VALUE
	            AND d.ID = s.ID
	            )
			 WHEN NOT MATCHED
			 THEN
			     INSERT     (DBID,
			                 SQL_ID,
			                 PLAN_HASH_VALUE,
			                 ID,
			                 OPERATION,
			                 OPTIONS,
			                 OBJECT_NODE,
			                 OBJECT#,
			                 OBJECT_OWNER,
			                 OBJECT_NAME,
			                 OBJECT_ALIAS,
			                 OBJECT_TYPE,
			                 OPTIMIZER,
			                 PARENT_ID,
			                 DEPTH,
			                 POSITION,
			                 SEARCH_COLUMNS,
			                 COST,
			                 CARDINALITY,
			                 BYTES,
			                 OTHER_TAG,
			                 PARTITION_START,
			                 PARTITION_STOP,
			                 PARTITION_ID,
			                 OTHER,
			                 DISTRIBUTION,
			                 CPU_COST,
			                 IO_COST,
			                 TEMP_SPACE,
			                 ACCESS_PREDICATES,
			                 FILTER_PREDICATES,
			                 PROJECTION,
			                 TIME,
			                 QBLOCK_NAME,
			                 REMARKS,
			                 TIMESTAMP,
			                 OTHER_XML,
			               
			                 INSERT_TIME)
			         VALUES (s.DBID,
			                 s.SQL_ID,
			                 s.PLAN_HASH_VALUE,
			                 s.ID,
			                 s.OPERATION,
			                 s.OPTIONS,
			                 s.OBJECT_NODE,
			                 s.OBJECT#,
			                 s.OBJECT_OWNER,
			                 s.OBJECT_NAME,
			                 s.OBJECT_ALIAS,
			                 s.OBJECT_TYPE,
			                 s.OPTIMIZER,
			                 s.PARENT_ID,
			                 s.DEPTH,
			                 s.POSITION,
			                 s.SEARCH_COLUMNS,
			                 s.COST,
			                 s.CARDINALITY,
			                 s.BYTES,
			                 s.OTHER_TAG,
			                 s.PARTITION_START,
			                 s.PARTITION_STOP,
			                 s.PARTITION_ID,
			                 s.OTHER,
			                 s.DISTRIBUTION,
			                 s.CPU_COST,
			                 s.IO_COST,
			                 s.TEMP_SPACE,
			                 s.ACCESS_PREDICATES,
			                 s.FILTER_PREDICATES,
			                 s.PROJECTION,
			                 s.TIME,
			                 s.QBLOCK_NAME,
			                 s.REMARKS,
			                 s.TIMESTAMP,
			                 s.OTHER_XML,
			                 
			                 SYSDATE);
      
   END LOOP;
   CLOSE SQLSTAT_CUR;
   
   DBMS_OUTPUT.PUT_LINE('Table 30~32 End Time : ' || TO_CHAR(SYSTIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6'));
   
   LOG_REC.END_TIME     := TO_CHAR(SYSTIMESTAMP, 'YYYYMMDDHH24MISSFF6');
   LOG_REC.SUCCESS_FLAG := 'Y';
   LOG_REC.RESV_FIELD1 := TO_CHAR(V_START_TIME, 'YYYY/MM/DD HH24:MI:SS');
   LOG_REC.RESV_FIELD2 := TO_CHAR(V_END_TIME, 'YYYY/MM/DD HH24:MI:SS');
   LOG_REC.RESV_FIELD3 := TO_CHAR(MIN_SNAP_ID);
   LOG_REC.RESV_FIELD4 := TO_CHAR(MAX_SNAP_ID);
   LOG_WRITE(LOG_REC);
   
   COMMIT;", dbName, dbLinkName, sqlplan);
                }
                script.AppendLine("");
                script.AppendLine($"DBMS_APPLICATION_INFO.SET_MODULE('{currentProcedureName}', 'End');");
                script.AppendFormat($@"EXCEPTION
   WHEN OTHERS
   THEN
      ROLLBACK;

                LOG_REC.END_TIME     := TO_CHAR(SYSTIMESTAMP, 'YYYYMMDDHH24MISSFF6');
                LOG_REC.SUCCESS_FLAG := 'N';
                LOG_REC.ERROR_CODE   := SQLCODE;
                LOG_REC.ERROR_MSG    := SUBSTRB(SQLERRM, 1, 4000);
                LOG_WRITE(LOG_REC);

                COMMIT;
                END {currentProcedureName};");

                return script.ToString();
            }

            private string GenerateInsertSting(string localTable, string fetchTable)
            {
                StringBuilder insertScript = new StringBuilder("");
                CreateDataBaseArgsPack arg = new CreateDataBaseArgsPack();
                string snapshotTable = IsPdb ? "AWR_PDB_SNAPSHOT" : "DBA_HIST_SNAPSHOT";
                insertScript.AppendFormat(@"
 select  
       case when '{2}' like '%SNAPSHOT' then
                 'INSERT INTO RAW_' || '{2}' || '_{4}' || ' VALUE (' || CHR(13) 
                 || LISTAGG(COLUMN_NAME, ', ') WITHIN GROUP (ORDER BY COLUMN_ID) || CHR(13) 
                 || ') ' || CHR(13)
            when '{2}' like '%SYSMETRIC_SUMMARY' then
                  'INSERT INTO RAW_' || '{2}' || '_{4}' || ' VALUE (' || CHR(13)
                 || LISTAGG(COLUMN_NAME, ', ') WITHIN GROUP (ORDER BY COLUMN_ID) || CHR(13)  
                 || ')' || CHR(13)
            else
                 'INSERT INTO RAW_' || '{2}'|| '_{4}' || ' VALUE (' || CHR(13)
                 || LISTAGG(COLUMN_NAME, ', ') WITHIN GROUP (ORDER BY COLUMN_ID) || CHR(13)  
                 || ', BEGIN_TIME, END_TIME) ' || CHR(13)
            end sql1,
      case when '{2}' like '%SNAPSHOT' then
                 ' (SELECT '
                 || LISTAGG(COLUMN_NAME, ', ') WITHIN GROUP (ORDER BY COLUMN_ID) || CHR(13)
           when '{2}' like '%SYSMETRIC_SUMMARY' then
                 ' (SELECT '
                 || LISTAGG('A.' || COLUMN_NAME, ', ') WITHIN GROUP (ORDER BY COLUMN_ID) || CHR(13)
            else
                 ' (SELECT '
                 || LISTAGG('A.' || COLUMN_NAME, ', ') WITHIN GROUP (ORDER BY COLUMN_ID) || CHR(13)
                 || ', TO_DATE(TO_CHAR(B.BEGIN_INTERVAL_TIME, ''YYYY-MM-DD HH24:MI:SS''), ''YYYY-MM-DD HH24:MI:SS''),' || CHR(13)
                 || 'TO_DATE(TO_CHAR(B.END_INTERVAL_TIME, ''YYYY-MM-DD HH24:MI:SS''), ''YYYY-MM-DD HH24:MI:SS'')' || CHR(13)
            end sql2,
       case when '{2}' like '%SNAPSHOT' then
                 ' FROM ' || '{3}' || '@' || '{0}' || CHR(13)
                 || ' WHERE SNAP_ID >= MIN_SNAP_ID AND SNAP_ID <= MAX_SNAP_ID);'
            else
                 ' FROM ' || '{3}' || '@' || '{0}' || ' A,' || CHR(13)
                 || ' {1}@' || '{0}' || ' B' || CHR(13)
                 || ' WHERE B.SNAP_ID >= MIN_SNAP_ID AND B.SNAP_ID <= MAX_SNAP_ID' || CHR(13)
                 || ' AND B.SNAP_ID = A.SNAP_ID' || CHR(13)
                 || ' AND B.DBID = A.DBID' || CHR(13)
                 || ' AND B.INSTANCE_NUMBER = A.INSTANCE_NUMBER);'
            end sql3           
from dba_tab_columns s
where table_name = '{2}'
and exists (select 1
              from dba_tab_columns@{0}
              where table_name =  '{3}'--:pdbtablename
              and COLUMN_NAME = s.COLUMN_NAME)", dbLinkName, snapshotTable, localTable, fetchTable, dbName);
                arg.Script = insertScript.ToString();
                StringBuilder insertStr = new StringBuilder("");
                insertStr.AppendLine(bs.ExecuteDataSet("GetInsertProcedureScript", arg.getPack()).Tables[0].Rows[0].Field<string>("sql1"));
                insertStr.AppendLine(bs.ExecuteDataSet("GetInsertProcedureScript", arg.getPack()).Tables[0].Rows[0].Field<string>("sql2"));
                insertStr.AppendLine(bs.ExecuteDataSet("GetInsertProcedureScript", arg.getPack()).Tables[0].Rows[0].Field<string>("sql3"));
                return insertStr.ToString();


            }

            public string DbName { get => dbName; set => dbName = value; }
            public string CurrentProcedureName { get => currentProcedureName; set => currentProcedureName = value; }
            public string DataProcedurePrefix { get => dataProcedurePrefix; set => dataProcedurePrefix = value; }
            public string SummaryProcedurePrefix { get => summaryProcedurePrefix; set => summaryProcedurePrefix = value; }
            public string DbLinkName { get => dbLinkName; set => dbLinkName = value; }
            public List<string> FetchTableNames { get => fetchTableNames; set => fetchTableNames = value; }
            public List<string> LocalTableNames { get => localTableNames; set => localTableNames = value; }
            public bool IsPdb { get => isPdb; set => isPdb = value; }
            public bool IsLater12c { get => isLater12c; set => isLater12c = value; }
        }


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
                dblinkConfirmmemoEdit.Clear();
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
            if (e.Page == tableCreateProcesswizardPage)
            {
                if (XtraMessageBox.Show(this, "Do you want to create procedure?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) e.Handled = true;
            }
            if (e.Page == procedureWizardPage)
            {
                wizardControl.SelectedPage = wpfinish;

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
            if (e.Page == procedureWizardPage&& !isAlreadyCreateProcedure)
            {
                //Todo根据grid表格生成table
                GenerateProcedure(e);
            }
        }

        private void GenerateProcedure(WizardPageChangedEventArgs e)
        {
            try
            {
                procedureCreateMemoEdit.Clear();
                e.Page.AllowNext = false;
                e.Page.AllowBack = false;
                procedureCreateMemoEdit.AppendLine("start...");
                //创建procedure entity
                procedureCreateMemoEdit.AppendLine($"now generating GET_DATA_{dbLinkEntity.DbName} procedure...");

                DbProcedureEntity procedureEntity = InitDbProcedureEntiy();
                CreateDataBaseArgsPack arg = new CreateDataBaseArgsPack();
                arg.Script= procedureEntity.GenerateGetDataProcedureScript();
                bs.ExecuteModify("CreateProcedure", arg.getPack());
                procedureCreateMemoEdit.AppendLine($"generation of GET_DATA_{dbLinkEntity.DbName} procedure success!!!");
                procedureCreateMemoEdit.AppendLine($"now insert new db info into table tapctdatabase");

                AddDbData();
                procedureCreateMemoEdit.AppendLine($"insertion success!!!");
                procedureCreateMemoEdit.AppendLine($"now generating SUMMARY_WORKLOAD_{dbLinkEntity.DbName} procedure...");

                arg.Script = procedureEntity.GenerateSummaryWorkloadProcedureScript();
                bs.ExecuteModify("CreateProcedure", arg.getPack());
                procedureCreateMemoEdit.AppendLine($"generation of SUMMARY_WORKLOAD_{dbLinkEntity.DbName} procedure success!!!");

                e.Page.AllowNext = true;
            }
            catch (Exception ex)
            {
                procedureCreateMemoEdit.AppendLine(ex.Message);
            }
            finally
            {
                procedureCreateMemoEdit.AppendLine("process end...");
                e.Page.AllowBack = true;
            }
        }
       
        private void AddDbData()
        {
            CodeManagementArgsPack arg = new CodeManagementArgsPack();
            arg.DBID = dbVersionEntity.DBID;
            arg.SERVICENAME = dbVersionEntity.SERVICENAME;
            arg.DESCRIPTION = dbLinkEntity.Description ;
            arg.DBLINKNAME = dbLinkEntity.DbLink;
            arg.IPADDRESS = dbLinkEntity.Ip;
            arg.DBNAME = dbLinkEntity.DbName;
            arg.INSTANTCNT = int.Parse(dbVersionEntity.INSTANCECOUNT);
            arg.ISALIVE = "YES";
            arg.RETENTIONDAYS = dbLinkEntity.RetentionDay;
            arg.INSERTTIME = DateTime.Now.ToString("yyyyMMddHHmmss");
            arg.INSERTUSER = TAP.UI.InfoBase._USER_INFO.Name;
            try
            {
                bsDatabaseManagement.ExecuteModify("DeleteDBInfoByDBName", arg.getPack());
            }
            catch
            {

            }
            bsDatabaseManagement.ExecuteModify("SaveDbInfo", arg.getPack());

        }

        private DbProcedureEntity InitDbProcedureEntiy()
        {
            DbProcedureEntity procedureEntity = new DbProcedureEntity();
            procedureEntity.DbName = dbLinkEntity.DbName;
            procedureEntity.DbLinkName = dbLinkEntity.DbLink;
            procedureEntity.IsLater12c = dbVersionEntity.IsLater12CIndex;
            procedureEntity.IsPdb = dbVersionEntity.ISPDB.Equals("yes") ? true : false;
            DataTable dt = bs.ExecuteDataSet("GetInsertTableAndPdbTableName").Tables[0];
            DataColumn columnTableName = dt.Columns["tablename".ToUpper()];

            if (procedureEntity.IsPdb)
            {
                DataColumn column = dt.Columns["pdbtablename".ToUpper()];
                procedureEntity.FetchTableNames = column.Table.AsEnumerable().Select(r => r.Field<string>(column)).ToList();
            }
            else
            {
                procedureEntity.FetchTableNames = columnTableName.Table.AsEnumerable().Select(r => r.Field<string>(columnTableName)).ToList();
            }
            procedureEntity.LocalTableNames = columnTableName.Table.AsEnumerable().Select(r => r.Field<string>(columnTableName)).ToList();
            return procedureEntity;
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
