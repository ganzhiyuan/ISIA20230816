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
    public partial class FrmMailGroupEdit : Form
    {
        public int isUpdate = 0;
        BizDataClient bs = null;
        public string _groupID = string.Empty;
        public MailGroupArgsPack args = new MailGroupArgsPack();

        public FrmMailGroupEdit()
        {
            InitializeComponent();
            isUpdate = 1;
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.MailGroupManagement");
        }
        public FrmMailGroupEdit(string groupName)
        {
            InitializeComponent();
            isUpdate = 0;
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.MailGroupManagement");

            MailGroupArgsPack argsPack = new MailGroupArgsPack();
            argsPack.NAME = groupName;
            DataSet ds = bs.ExecuteDataSet("GetMailGroup", argsPack.getPack());
            this.txtName.Text = ds.Tables[0].Rows[0]["NAME"].ToString();
            this.txtGrgion.Text= ds.Tables[0].Rows[0]["REGION"].ToString();
            this.txtDESCRIPTION.Text= ds.Tables[0].Rows[0]["DESCRIPTION"].ToString();
            this.txtName.Enabled = false;
            this.txtGrgion.Enabled = false;
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //List<CommonArgsPack> ArgsPacks = new List<CommonArgsPack>();
            args.REGION = txtGrgion.Text;
            args.NAME = txtName.Text;
            args.UPDATEUSER = InfoBase._USER_INFO.UserName;
            args.UPDATETIME = DateTime.Now.ToString("yyyyMMddHHmmss");
            args.DESCRIPTION = txtDESCRIPTION.Text;
            if (isUpdate==0)//调用修改
            {
                int i = bs.ExecuteModify("UpdateMailGroup",args.getPack());
            }
            else
            {

                args.INSERTUSER = InfoBase._USER_INFO.UserName;
                args.INSERTTIME = DateTime.Now.ToString("yyyyMMddHHmmss");
            
                int i = bs.ExecuteModify("InsertMailGroup", args.getPack());
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
