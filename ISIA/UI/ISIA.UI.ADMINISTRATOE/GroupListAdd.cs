using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using TAP;
using TAP.Data.Client;
using TAP.Models.User;
using TAP.UI;
using ISIA.UI.BASE;

namespace ISIA.UI.ADMINISTRATOE
{
    public partial class GroupListAdd : Form
    {
        public GroupListAdd()
        {
            InitializeComponent();
            InitializeLanguage();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.GroupUserManagement");
        }
        #region Feild     
        BizDataClient bs = null;
        #endregion
        #region Method
        private void InitializeLanguage()
        {
            FormSetLanguage formSetLanguage = new FormSetLanguage();
            List<dynamic>  lists = new List<dynamic>();
            lists.Add(labelControl1);
            lists.Add(labelControl2);
            lists.Add(labelControl3);
            lists.Add(labelControl4);
            lists.Add(labelControl5);
            lists.Add(labelControl6);
            lists.Add(btnSave);
            lists.Add(btnCancel);
            formSetLanguage.SetLanguage(lists);
        }
        #endregion
        #region Event     
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string addGroupName = txtGroupName.Text;
                string addREGION = txtREGION.Text;
                string addFACILITY = txtFACILITY.Text;
                string addDESCRIPTION = txtDESCRIPTION.Text;
                string addCURRENTMODEL = txtCURRENTMODEL.Text;
                uint addLEVELS = UInt32.Parse(txtLEVELS.Text);
                DataTable dt = bs.ExecuteDataTable("GetGroup");
                dt.DefaultView.RowFilter = string.Format(" NAME='{0}'", addGroupName);
                DataTable groupDt = dt.DefaultView.ToTable();
                if (groupDt.Rows.Count == 0)
                {

                }
                else
                {
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Added already exists..");
                    return;
                }
                CommonArgsPack args = new CommonArgsPack();
                args.Name = addGroupName;
                args.Region = addREGION;
                args.Facility = addFACILITY;
                args.InsertTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                args.CurrentModel = EnumFlagYN.YES.ToString();
                args.Description = addDESCRIPTION;
                args.Levels = addLEVELS;
                args.InsertUser = InfoBase._USER_INFO.UserName;
                int SaveCount = bs.ExecuteModify("SaveGroup", args.getPack());
                if (SaveCount == 0)
                {
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "Insert failure..");
                }
                else
                {
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Insert complete..");
                    this.Close();
                }
            }
            catch (System.Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
