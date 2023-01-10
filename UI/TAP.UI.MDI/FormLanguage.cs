using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using TAP.Models;
using TAP.Models.Codes;
using TAP.Models.User;
using TAP.Models.UIBasic;

namespace TAP.UI.MDI
{
    /// <summary>
    /// This is login form
    /// </summary>
    public partial class FormLanguage : Form
    {
        #region Properties

        /// <summary>
        /// User info
        /// </summary>
        public UserDefaultInfo _UserDefaultInfo;

        #endregion

        /// <summary>
        /// This creates FormLogin
        /// </summary>
        public FormLanguage()
        {
            InitializeComponent();
        }

        #region Initialize

        private void Initialize()
        {
            #region Initialize

            try
            {
#if WORKFLOW
                this.Text = "TAP Workflow::Login";
#endif
#if MODELER
                this.Text = "TAP Modeler::Login";
#endif

                string tempStr = "";

                if (InfoBase._USER_INFO.Language == "EN")
                    tempStr = "<image=#en> EN";
                else if (InfoBase._USER_INFO.Language == "KR")
                    tempStr = "<image=#kr> KR";
                else
                    tempStr = "<image=#cn> CN";

                this.cboLanguage.Text = tempStr;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }
        
        #endregion

        #region Language Change

        private void ChangeLanguage()
        {
            #region ChangeLanguage

            UserModel tmpUser = null;

            try
            {
                if(this.cboLanguage.Text == string.Empty)
                {
                    throw new Exception("Insert Language please");
                }


                this.SetControlEnable(false);
                
                #region Check user login

                //사용자 권한정보 없이 사용자 정보만 로드한다. 
                tmpUser = new UserModel(_UserDefaultInfo, null, EnumFlagYN.NO);
                
                #endregion

                InfoBase._USER_INFO = tmpUser;

                string tempLanguage = cboLanguage.Text.Substring(12);

                InfoBase._USER_INFO.Language = tempLanguage;
                this.DialogResult = DialogResult.OK;

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
              
            #endregion
        }

        private void SetControlEnable(bool enable)
        {
            #region Set Control Enable

            try
            {
                this.buttonCancel.Enabled = enable;
                this.buttonLogIn.Enabled = enable;

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        public void SetUserDefaultInfo(UserDefaultInfo userDefaultInfo)
        {
            this._UserDefaultInfo = userDefaultInfo;
        }

        #endregion

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            #region buttonCancel_Click

            try
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            #endregion
        }

        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            #region buttonLogIn_Click

            try
            {
                this.ChangeLanguage();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.SetControlEnable(true);
            }

            #endregion

        }

        private void FormLanguage_Load(object sender, EventArgs e)
        {
            #region FormLanguage_Load

            try
            {                
                this.Initialize();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            #endregion
        }

        
        private void textBoxPassword_Enter_Downed()
        {
            #region FormLogin_Load

            try
            {
                this.ChangeLanguage();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            #endregion
        }

        private void cboLanguage_Properties_KeyDown(object sender, KeyEventArgs e)
        {
            return;
        }
    }
}