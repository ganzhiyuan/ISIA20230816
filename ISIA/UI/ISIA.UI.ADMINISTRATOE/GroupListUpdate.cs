using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TAP;
using TAP.Data.Client;
using TAP.Fressage;
using TAP.Models.User;
using TAP.UI;
using ISIA.UI.BASE;

namespace ISIA.UI.ADMINISTRATOE
{
    public partial class GroupListUpdate : Form
    {
        public GroupListUpdate()
        {
            InitializeComponent();
            InitializeLanguage();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.GroupUserManagement");
        }

        #region Feild
        DataSet ds;
        BizDataClient bs = null;
        #endregion
     
        #region Method
        private void InitializeLanguage()
        {
            FormSetLanguage formSetLanguage = new FormSetLanguage();
            List<dynamic> labels = new List<dynamic>();
            labels.Add(labelControl1);
            labels.Add(labelControl2);
            labels.Add(labelControl3);
            labels.Add(labelControl4);
            labels.Add(labelControl5);
            labels.Add(labelControl6);
            labels.Add(labelControl13);
            labels.Add(btnSave);
            labels.Add(btnCancel);
            labels.Add(tRadISALIVEyes);
            labels.Add(tRadISALIVEno);
            formSetLanguage.SetLanguage(labels);
        }
        public void Receipt(DataSet ds1)
        {
            ds = ds1;
            ShowData();
        }

        private void ShowData()
        {
            if (ds == null)
            {
                return;
            }

            txtGroupName.Text = ds.Tables[0].Rows[0]["NAME"].ToString();
            txtREGION.Text = ds.Tables[0].Rows[0]["REGION"].ToString();
            txtFACILITY.Text = ds.Tables[0].Rows[0]["FACILITY"].ToString();
            txtDESCRIPTION.Text = ds.Tables[0].Rows[0]["DESCRIPTION"].ToString();
            txtCURRENTMODEL.Text = ds.Tables[0].Rows[0]["CURRENTMODEL"].ToString();
            txtLEVELS.Text = ds.Tables[0].Rows[0]["LEVELS"].ToString();

            string rtobtn = ds.Tables[0].Rows[0]["ISALIVE"].ToString();
            if (rtobtn == "YES")
            {
                tRadISALIVEyes.Checked = true;
            }
            else
            {
                tRadISALIVEno.Checked = true;
            }
        }
        #endregion

        #region Event

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

                    //userDefaultInfo.Region = addREGION;
                    //userDefaultInfo.Facility = addFACILITY;
                    //userDefaultInfo.UserID = addGroupName;
                    //userDefaultInfo.UserGroup = addGroupName;

                    //userGroupModel = new UserGroupModel(userDefaultInfo);
                    //userGroupModel.Levels = addLEVELS;
                    //userGroupModel.CurrentModel = EnumFlagYN.YES;
                    //userGroupModel.Description = addDESCRIPTION;
                    //int UpdateCount = userGroupModel.Save(addGroupName);
                    CommonArgsPack args = new CommonArgsPack();
                    //args.GroupName = addGroupName;
                    args.Region = addREGION;
                    args.Facility = addFACILITY;
                    args.UpdateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    args.UpdateUser = InfoBase._USER_INFO.UserName; ;
                    args.CurrentModel = EnumFlagYN.YES.ToString();
                    args.Description = addDESCRIPTION;
                    args.IsAlive = addISALIVE;
                    args.Levels = addLEVELS;
                    args.Name = addGroupName;

                    int UpdateCount = bs.ExecuteModify("UpdateGroup", args.getPack());

                    if (UpdateCount == 0)
                    {
                        TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "UpdateCount failure..");
                    }
                    else
                    {
                        TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "UpdateCount complete..");
                        this.Close();
                    }
                }
                else { return; }
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
        #endregion
    }
}
