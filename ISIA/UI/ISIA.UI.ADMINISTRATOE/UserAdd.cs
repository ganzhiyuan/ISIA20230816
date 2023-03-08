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
    public partial class UserAdd : Form
    {
        public UserAdd()
        {
            InitializeComponent();
            InitializeLanguage();
            InitializeCboLanguage();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.GroupUserManagement");
        }

        #region Feild            
        BizDataClient bs = null;
        private string _region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
        #endregion

        #region Method
        private void InitializeLanguage()
        {
            FormSetLanguage formSetLanguage = new FormSetLanguage();
            List<DevExpress.XtraEditors.LabelControl> labels = new List<DevExpress.XtraEditors.LabelControl>();
            labels.Add(labelControl1);
            labels.Add(labelControl3);
            labels.Add(labelControl6);
            labels.Add(labelControl5);
            labels.Add(labelControl12);
            labels.Add(labelControl11);
            labels.Add(labelControl10);
            labels.Add(labelControl9);
            labels.Add(labelControl7);
            labels.Add(PASSWORD);
            labels.Add(labelControl22);
            labels.Add(labelControl21);
            labels.Add(labelControl20);
            labels.Add(labelControl19);
            formSetLanguage.SetLanguage_LabelControl(labels);
            List<DevExpress.XtraEditors.SimpleButton> buttons = new List<DevExpress.XtraEditors.SimpleButton>();
            buttons.Add(btnSave);
            buttons.Add(btnCancel);
            formSetLanguage.SetLanguage_SimpleButton(buttons);

        }
        private void InitializeCboLanguage()
        {
            cboLANGUAGE.Properties.Items.Add("CN");
            cboLANGUAGE.Properties.Items.Add("EN");
            cboLANGUAGE.Properties.Items.Add("KR");
        }
        #endregion

        #region Event
        private void btnSave_Click(object sender, EventArgs e)
        {
            string addName = txtName.Text; 
            string addREGION = _region;
            string addFACILITY = "T1";
            string addDEPARTMENT = txtDEPARTMENT.Text;
            string addPOSITION = txtPOSITION.Text;
            string addUSERNAME = txtUSERNAME.Text;
            string addUSERMIDDLENAME = txtUSERMIDDLENAME.Text;
            string addUSERLASTNAME = txtUSERLASTNAME.Text;
            string addCONTACTNO = cboMOBILEROLE.Text;
            string addMOBILENO = txtMOBILENO.Text;
            string addMAILADDRESS = txtMAILADDRESS.Text;
            string addPASSWORD = txtPASSWORD.Text;
            string addUSERGROUPNAME = txtUSERGROUPNAME.Text;
            string addLANGUAGE = cboLANGUAGE.Text;
            string addCURRENTMODEL = txtCURRENTMODEL.Text;
            string addDESCRIPTION = txtDESCRIPTION.Text;

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Please write a name.");
                return;
            }



            try
            {
                DataTable dt = bs.ExecuteDataTable("GetUsers");
                DataRow[] drs = dt.Select("NAME='" + addName + "' AND FACILITY= '" + addFACILITY + "' AND REGION='" + addREGION + "'");

                DataTable dtNew = dt.Clone();
                foreach (DataRow dr in drs)
                {
                    dtNew.ImportRow(dr);
                }

                if (dtNew.Rows.Count == 0)
                {

                }
                else
                {
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Added already exists..");
                    return;
                }

                CommonArgsPack args = new CommonArgsPack();

                args.Name = addName;
                args.Region = addREGION;
                args.Facility = addFACILITY;
                args.Department = addDEPARTMENT;
                args.Position = addPOSITION;
                args.UserName = addUSERNAME.ToUpper();
                args.UserMiddleName = addUSERMIDDLENAME;
                args.UserLastName = addUSERLASTNAME;
                args.Contactno = addCONTACTNO;
                args.Mobileno = addMOBILENO;
                args.MailAddress = addMAILADDRESS;

                if (addPASSWORD != string.Empty || addPASSWORD != null)
                {
                    args.Password = TAP.Base.Crypto.Crypto.encryptAES128(addPASSWORD);
                }
                else
                {
                    args.Password = addPASSWORD;
                }
                args.UserGroupName = addUSERGROUPNAME;
                args.Language = addLANGUAGE;
                args.CurrentModel = EnumFlagYN.YES.ToString();
                args.Description = addDESCRIPTION;
                args.InsertUser = InfoBase._USER_INFO.UserName;
                args.InsertTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                args.IsAlive = "YES";
                args.Sequence = 0;
                int SaveCount = bs.ExecuteModify("SaveUser", args.getPack());

                if (SaveCount == 0)
                {
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Insert failure..");
                }
                else
                {
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Insert complete..");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
