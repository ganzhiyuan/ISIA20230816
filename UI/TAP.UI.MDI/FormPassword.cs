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
    public partial class FormPassword : Form
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
        public FormPassword()
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
                this.textBeforePw.Select();

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }
        
        #endregion

        #region PassWord Change

        private void ChangePassword()
        {
            #region ChangePassword

            UserModel tmpUser = null;

            try
            {
                if(this.textBeforePw.Text == string.Empty)
                {
                    throw new Exception("Insert Current Password please");
                }

                if (this.textAfterPw.Text == string.Empty)
                {
                    throw new Exception("Insert Change password please");
                }

                this.SetControlEnable(false);
                
                #region Check user login

                //사용자 권한정보 없이 사용자 정보만 로드한다. 
                tmpUser = new UserModel(_UserDefaultInfo, null, EnumFlagYN.NO);

                if (tmpUser.Password.Equals(TAP.Base.Crypto.Crypto.encryptAES128(this.textBeforePw.Text)))
                {
                    tmpUser.Password = TAP.Base.Crypto.Crypto.encryptAES128(this.textAfterPw.Text);
                }
                else
                {
                    this.SetControlEnable(true);
                    this.textBeforePw.Text = "";
                    throw new Exception("Assigned password does match to registration information. Check your password again please.");
                }

                #endregion

                InfoBase._USER_INFO = tmpUser;
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
                this.textAfterPw.Enabled = enable;
                this.textBeforePw.Enabled = enable;
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
                this.ChangePassword();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.SetControlEnable(true);
            }

            #endregion

        }

        private void FormPassword_Load(object sender, EventArgs e)
        {
            #region FormPassword_Load

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
                this.ChangePassword();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            #endregion
        }
    }
}