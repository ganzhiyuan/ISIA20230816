using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using TAP.Models;
using TAP.Models.Codes;
using TAP.Models.User;
using TAP.Models.UIBasic;
using System.IO;
using Newtonsoft.Json;

namespace TAP.UI.MDI
{
    /// <summary>
    /// This is login form
    /// </summary>
    public partial class FormLogin2 : Form
    {
        #region Properties

        /// <summary>
        /// User info
        /// </summary>
        public UserDefaultInfo _UserDefaultInfo;
        private List<TmpUser> _UserHistory = new List<TmpUser>();
        private const string _UserHistoryFileName = "UserHistory.json";
        #endregion

        /// <summary>
        /// This creates FormLogin2
        /// </summary>
        public FormLogin2()
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
                this.BindRegion();
                this.textBoxUserID.Select();
                this.LoadUserHistory();
                this.textBoxUserID.Text = _UserHistory.Count > 0 ? _UserHistory[0].UserID : string.Empty;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void BindRegion()
        {
            #region Bind Region

            try
            {
                this.comboBoxRegion.BeginUpdate();
                this.comboBoxRegion.Items.Add(TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region);
                this.comboBoxRegion.EndUpdate();

                this.comboBoxRegion.SelectedItem = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void BindFacility(string region)
        {
            #region Bind Facility

            FacilityCodeModel tmpFacilities = null;

            try
            {
                tmpFacilities = new FacilityCodeModel(region, EnumFlagYN.YES);

                this.comboBoxFacility.BeginUpdate();
                this.comboBoxFacility.DataSource = null;
                this.comboBoxFacility.DataSource = tmpFacilities.CodeList.CreateSeqenceArray();
                this.comboBoxFacility.EndUpdate();
                //seoil 실수 같아 바꿈.
                //this.comboBoxRegion.SelectedItem = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Facility;
                this.comboBoxFacility.SelectedItem = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Facility;
                
                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Log in

        private void LogIn()
        {
            #region Log In

            UserModel tmpUser = null;

            try
            {
                if(this.textBoxUserID.Text == string.Empty)
                {
                    throw new Exception("Insert user name please");
                }

                if (this.textBoxPassword.Text == string.Empty)
                {
                    throw new Exception("Insert password please");
                }

                this.SetControlEnable(false);

                #region Meke Default Info

                _UserDefaultInfo = new UserDefaultInfo();
                _UserDefaultInfo.Region = this.comboBoxRegion.Text;
                _UserDefaultInfo.Facility = "T1";
                _UserDefaultInfo.UserID = this.textBoxUserID.Text.ToUpper();
                _UserDefaultInfo.User = Form1._CURR_USER;

                #endregion

                #region Check user login

                //사용자 권한정보 없이 사용자 정보만 로드한다. 
                tmpUser = new UserModel(_UserDefaultInfo, null, EnumFlagYN.NO);

                //if (object.Equals(tmpUser, null) || tmpUser.Name.Length == 0)
                if (tmpUser.BindData == EnumFlagYN.NO)
                {
                    this.SetControlEnable(true);
                    this.textBoxPassword.Text = "";
                    throw new Exception(string.Format("Assigned user ID '{0}' does not exist. Check your ID again please.", this.textBoxUserID.Text));
                }

                if (!tmpUser.Password.Equals(TAP.Base.Crypto.Crypto.encryptAES128(this.textBoxPassword.Text)))
                {
                    this.SetControlEnable(true);
                    this.textBoxPassword.Text = "";
                    this.textBoxPassword.Focus();
                    throw new Exception("Assigned password does match to registration information. Check your password again please.");
                }

                #endregion

                InfoBase._USER_INFO = tmpUser;
                this.DialogResult = DialogResult.OK;

                RememberUserID();
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
                this.comboBoxRegion.Enabled = enable;
                this.comboBoxFacility.Enabled = enable;
                this.textBoxPassword.Enabled = enable;
                this.textBoxUserID.Enabled = enable;
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
                this.LogIn();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.SetControlEnable(true);
            }

            #endregion

        }

        private void FormLogin2_Load(object sender, EventArgs e)
        {
            #region FormLogin2_Load

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

        private void comboBoxRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region FormLogin2_Load

            try
            {
                this.BindFacility(this.comboBoxRegion.SelectedItem.ToString());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            #endregion
        }

        private void textBoxPassword_Enter_Downed()
        {
            #region FormLogin2_Load

            try
            {
                this.LogIn();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            #endregion
        }

        private void textBoxUserID_Enter_Downed()
        {
            #region Focuse Password

            try
            {
                if(textBoxPassword.TextLength < 1)
                {
                    textBoxPassword.Focus();
                }
                else
                {
                    #region FormLogin2_Load
                        this.LogIn();
                    #endregion
                }

            }
            catch(System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
        }

        private void RememberUserID()
        {
            if (textBoxUserID.TextLength < 1 || textBoxPassword.TextLength < 1)
            {
                return;
            }
            else
            {
                string tmpUserID = textBoxUserID.Text;
                if (_UserHistory.Find(u => u.UserID == tmpUserID) != null)
                {
                    _UserHistory.Find(u => u.UserID == tmpUserID).LoginTime = DateTime.Now;
                }
                else
                {
                    _UserHistory.Add(new TmpUser { UserID = tmpUserID, LoginTime= DateTime.Now });
                }
                using (StreamWriter sw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), _UserHistoryFileName), false))
                {
                    _UserHistory = _UserHistory.OrderBy(u => u.LoginTime).ToList();
                    sw.Write(JsonConvert.SerializeObject(_UserHistory));
                }
            }
        }

        private void LoadUserHistory()
        {
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), _UserHistoryFileName)))
            {
                return;
            }
            else
            {
                using (StreamReader sr = File.OpenText(Path.Combine(Directory.GetCurrentDirectory(), _UserHistoryFileName)))
                {
                    string json = sr.ReadToEnd();
                    _UserHistory = JsonConvert.DeserializeObject<List<TmpUser>>(json);
                    _UserHistory = _UserHistory.OrderByDescending(u => u.LoginTime).ToList();
                }
                this.textBoxUserID.AutoCompleteCustomSource.Clear();
                this.textBoxUserID.AutoCompleteCustomSource.AddRange( _UserHistory.ConvertAll(u=>u.ToString()).ToArray());
            }
        }

        class TmpUser
        {
            public string  UserID { get; set; }
            public DateTime LoginTime { get; set; }

            public override string ToString()
            {
                return UserID;
            }
        }
    }


}