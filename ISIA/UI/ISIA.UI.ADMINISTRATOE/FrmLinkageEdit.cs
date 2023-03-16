using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP;
using TAP.Data.Client;
using TAP.UI;

namespace ISIA.UI.ADMINISTRATOE
{
    public partial class FrmLinkageEdit : Form
    {
        public int isUpdate = 0;
        BizDataClient bs = null;
        public string _groupID = string.Empty;
        public CommonArgsPack args = new CommonArgsPack();

        public FrmLinkageEdit()
        {
            InitializeComponent();
            isUpdate = 1;
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.LinkageManagement");
        }
        public FrmLinkageEdit(string groupName)
        {
            InitializeComponent();
            isUpdate = 0;
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.LinkageManagement");
            //CommonArgsPack arguments = new CommonArgsPack();
            //arguments.GroupName = groupName;
            //DataSet dt = bs.ExecuteDataSet("GetCtLinkageByGroupName", arguments.getPack());


            DataClient tmpDataClient = new DataClient();
            string tmpMainMenuSql = string.Format("select GROUPID,GROUPNAME,TAGETUINAME,PARAMETERLIST,DESCRIPTION,ISALIVE  from tapctlinkage where GROUPNAME='{0}' and isalive = 'YES' and TAGETUI='0'", groupName);
            LoadEdit(tmpDataClient, tmpMainMenuSql);
        }
        public List<T> GetList<T>(DataTable table)
        {
            List<T> list = new List<T>();
            T t = default(T);
            PropertyInfo[] propertypes = null;
            string tempName = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                t = Activator.CreateInstance<T>();
                propertypes = t.GetType().GetProperties();
                foreach (PropertyInfo pro in propertypes)
                {
                    tempName = pro.Name;
                    if (table.Columns.Contains(tempName))
                    {
                        object value = row[tempName];
                        if (!value.ToString().Equals(""))
                        {
                            pro.SetValue(t, value, null);
                        }
                    }
                }
                list.Add(t);
            }
            return list.Count == 0 ? null : list;
        }
        public FrmLinkageEdit(int groupID)
        {
            InitializeComponent();
            isUpdate = 0;
            this.txtGroupName.Enabled = false;
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.LinkageManagement");
            //CommonArgsPack arguments = new CommonArgsPack();
            //arguments.GroupName = groupName;
            //DataSet dt = bs.ExecuteDataSet("GetCtLinkageByGroupName", arguments.getPack());


            DataClient tmpDataClient = new DataClient();
            string tmpMainMenuSql = string.Format("select GROUPID,GROUPNAME,TAGETUINAME,PARAMETERLIST,DESCRIPTION,ISALIVE  from tapctlinkage where GROUPID='{0}' and isalive = 'YES'", groupID.ToString());
            LoadEdit(tmpDataClient, tmpMainMenuSql);
        }

        private void LoadEdit(DataClient tmpDataClient, string tmpMainMenuSql)
        {
            DataTable retVal1 = tmpDataClient.SelectData(tmpMainMenuSql, "tapctlinkage").Tables[0];
            List<LinkInfo> linkList = GetList<LinkInfo>(retVal1);
            this._groupID = linkList[0].GROUPID;
            this.txtGroupName.Text = linkList[0].GROUPNAME;
            txtTagetuiName.Text = linkList[0].TAGETUINAME;
            txtParamentList.Text = linkList[0].PARAMETERLIST;
            txtDESCRIPTION.Text = linkList[0].DESCRIPTION;
            if (linkList[0].ISALIVE == "YES")
            {
                tRadISALIVEyes.Checked = true;
                tRadISALIVEno.Checked = false;
            }
            else
            {
                tRadISALIVEyes.Checked = false;
                tRadISALIVEno.Checked = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //List<CommonArgsPack> ArgsPacks = new List<CommonArgsPack>();
            args.GroupName = txtGroupName.Text;
            args.MessageName = txtTagetuiName.Text;
            args.PartName = txtParamentList.Text;
            args.UpdateUser = InfoBase._USER_INFO.UserName;
            args.UpdateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            if (tRadISALIVEyes.Checked)
            {
                args.IsAlive = "YES";
            }
            else
            {
                args.IsAlive = "NO";
            }
            args.Description = txtDESCRIPTION.Text;
            if (isUpdate==0)//调用修改
            {
                args.UserID = _groupID;
            }
            else
            {
                //新增需加入groupid
                DataTable dt = bs.ExecuteDataSet("GetCtLinkageByMaxID").Tables[0];
                int groupId = Convert.ToInt32(dt.Rows[0][0]);
                args.EqpGroup = (groupId+1).ToString();
                args.InsertUser = InfoBase._USER_INFO.UserName;
                args.InsertTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                //ArgumentPack tmpap = new ArgumentPack();
                //ArgsPacks.Add(args);
                //tmpap.AddArgument("arguments", typeof(List<CommonArgsPack>), ArgsPacks);
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
