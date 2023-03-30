using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using TAP.Fressage;
using TAP.UI;
using TAP.Models.User;
using TAP;
using TAP.Data.Client;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System.Collections.Generic;
using ISIA.UI.BASE;

namespace ISIA.UI.ADMINISTRATOE
{
    public partial class UserUpdate : Form
    {
        public UserUpdate()
        {
            InitializeComponent();
            InitializeLanguage();
            InitializeCboLuage();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.GroupUserManagement");

        }

        #region Feild            
        DataRow dr;
        BizDataClient bs = null;
        private string _region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
        #endregion

        #region Method
        private void InitializeLanguage()
        {
            FormSetLanguage formSetLanguage = new FormSetLanguage();
            List<dynamic> labels = new List<dynamic>();
            labels.Add(labelControl1);
            labels.Add(labelControl3);
            labels.Add(labelControl6);
            labels.Add(labelControl5);
            labels.Add(labelControl12);
            labels.Add(labelControl11);
            //labels.Add(labelControl10);
            labels.Add(labelControl9);
            labels.Add(labelControl7);
            labels.Add(PASSWORD);
            labels.Add(labelControl22);
            labels.Add(labelControl21);
            labels.Add(labelControl20);
            labels.Add(labelControl19);
            labels.Add(labelControl13);
            labels.Add(btnUpdate);
            labels.Add(btnCancel);
            labels.Add(tRadISALIVEyes);
            labels.Add(tRadISALIVEno);
            formSetLanguage.SetLanguage(labels);
        }
        public void Receipt(DataRow dr1)
        {
            dr = dr1;
            ShowData();
        }

        private void InitializeCboLuage()
        {
            cboLANGUAGE.Properties.Items.Add("EN");
            cboLANGUAGE.Properties.Items.Add("CN");
            cboLANGUAGE.Properties.Items.Add("KR");
        }
        #endregion

        #region Event
        private void ShowData()
        {
            txtName.Text = dr["NAME"].ToString();
            txtDEPARTMENT.Text = dr["DEPARTMENT"].ToString();
            txtPOSITION.Text = dr["POSITION"].ToString();
            txtUSERNAME.Text = dr["USERNAME"].ToString();
            txtUSERMIDDLENAME.Text = dr["USERMIDDLENAME"].ToString();
            txtUSERLASTNAME.Text = dr["USERLASTNAME"].ToString();
            //cboMOBILEROLE.Text = dr["CONTACTNO"].ToString();
            txtMOBILENO.Text = dr["MOBILENO"].ToString();
            txtMAILADDRESS.Text = dr["MAILADDRESS"].ToString();

            if (dr["PASSWORD"].ToString() == string.Empty || dr["PASSWORD"].ToString() == "")
                txtPASSWORD.Text = dr["PASSWORD"].ToString();
            else
                txtPASSWORD.Text = TAP.Base.Crypto.Crypto.decryptAES128(dr["PASSWORD"].ToString());

            txtUSERGROUPNAME.Text = dr["USERGROUPNAME"].ToString();
            cboLANGUAGE.Text = dr["LANGUAGE"].ToString();
            txtCURRENTMODEL.Text = dr["CURRENTMODEL"].ToString();
            txtDESCRIPTION.Text = dr["DESCRIPTION"].ToString();
            string addISALIVE = dr["ISALIVE"].ToString();
            if (addISALIVE == "YES")
            {
                tRadISALIVEyes.Checked = true;
            }
            else
            {
                tRadISALIVEno.Checked = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string addName = txtName.Text;
            string addREGION = _region;
            string addFACILITY = "T1";
            string addDEPARTMENT = txtDEPARTMENT.Text;
            string addPOSITION = txtPOSITION.Text;
            string addUSERNAME = txtUSERNAME.Text;
            string addUSERMIDDLENAME = txtUSERMIDDLENAME.Text;
            string addUSERLASTNAME = txtUSERLASTNAME.Text;
            //string addCONTACTNO = cboMOBILEROLE.Text;
            string addMOBILENO = txtMOBILENO.Text;
            string addMAILADDRESS = txtMAILADDRESS.Text;
            string addPASSWORD = txtPASSWORD.Text;
            string addUSERGROUPNAME = txtUSERGROUPNAME.Text;
            string addLANGUAGE = cboLANGUAGE.Text;
            string addCURRENTMODEL = txtCURRENTMODEL.Text;
            string addDESCRIPTION = txtDESCRIPTION.Text;
            string addISALIVE = "";

            if (tRadISALIVEyes.Checked)
            {
                addISALIVE = "YES";
            }
            else
            {
                addISALIVE = "NO";
            }

            EnumLanguage tmpLang = (EnumLanguage)Enum.Parse(typeof(EnumLanguage), InfoBase._USER_INFO.Language);

            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            string tmpExecutablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";

            DirectoryInfo info = new DirectoryInfo(tmpExecutablePath);
            String path = info.Parent.FullName;

            string fressageFilePath = Path.Combine(path + "\\FX", "mnls", TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.LocalFile);

            NeutralConverter _converter = new NeutralConverter(tmpLang, EnumUseFor.TEXT, false, fressageFilePath);
            TemplateConverter _translator = new TemplateConverter(tmpLang, false, fressageFilePath);

            string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.UPDATE, EnumGeneralTemplateType.CONFIRM, "");
            DialogResult dialog = TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.CONFIRM, tmpMessage);
            if (dialog.ToString() == "Yes")
            {
                try
                {
                    CommonArgsPack args = new CommonArgsPack();
                    args.Name = addName;
                    args.Region = addREGION;
                    args.Facility = addFACILITY;
                    args.Department = addDEPARTMENT;
                    args.Position = addPOSITION;
                    args.UserName = addUSERNAME;
                    args.UserMiddleName = addUSERMIDDLENAME;
                    args.UserLastName = addUSERLASTNAME;
                    //args.Contactno = addCONTACTNO;
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
                    args.IsAlive = addISALIVE;
                    args.UpdateUser = InfoBase._USER_INFO.UserName;
                    args.UpdateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    int SaveCount = bs.ExecuteModify("UpdateUser", args.getPack());
                    if (SaveCount == 0)
                    {
                        TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "UpdateCount failure..");
                    }
                    else
                    {
                        TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "UpdateCount complete..");
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
                }
            }
            else { return; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion        
    }
}
