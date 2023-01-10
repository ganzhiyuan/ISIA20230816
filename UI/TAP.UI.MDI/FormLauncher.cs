using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using TAP;
using TAP.Fressage;
using TAP.Models;
using TAP.Models.UIBasic;
using TAP.Models.User;
using TAP.UI;
using TAP.UIControls.BasicControls;

namespace TAP.UI.MDI
{
    public partial class FormLauncher : Form
    {
        #region Field

        public static string _ExecutableDirectory;
        private bool _isSuccessInit;

        /// <summary>
        /// User info
        /// </summary>
        protected UserDefaultInfo _userDefaultInfo;

        private UILog _uiLog;

        private TAP.Fressage.NeutralConverter _conveter;

        private TAP.Fressage.TemplateConverter _translator;

        private string _imgDir;

        #endregion

        public FormLauncher()
        {
            InitializeComponent();
        }

        private void Initialize()
        {
            #region Initialize

            try
            {
                #region User Log in

                this._uiLog = new UILog();
                FormLogin tmpLogIn = new FormLogin();

                if (tmpLogIn.ShowDialog().Equals(DialogResult.OK))
                {
                    this._userDefaultInfo = tmpLogIn._UserDefaultInfo;

                    using (var bl = new TAP.UI.BackgroundProcessor(this.DoInitialize, this, 4L, true))
                    {
                        bl.Start();
                    }

                    if (!_isSuccessInit)
                    {
                        this.Dispose();
                        this.Close();
                    }

                    this._imgDir = Path.Combine(_ExecutableDirectory, "images");
                    this.InitializeAppLink(); 
                }
                else
                {
                    tmpLogIn.Close();
                    tmpLogIn.Dispose();
                    this.Close();
                }

                #endregion
            }
            catch(System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void DoInitialize()
        {
            #region Save Model

            try
            {
                #region Load User Information

                AsyncMessage.Message = "Loading user information....";
                this.LoadUserInformation();
                _uiLog.WriteLog("LAUNCHER", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Loading user information....OK");
                AsyncMessage.Progress++;

                #endregion

                #region Set Executable Directory

                AsyncMessage.Message = "Initializing Application executing environment";
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                _ExecutableDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";

                _uiLog.WriteLog("LAUNCHER", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Initializing Application executing environment.....OK");
                AsyncMessage.Progress++;

                #endregion

                #region Download MNLS files

                AsyncMessage.Message = "Setting user language....";
                this.DownloadCrossLanguageFile();

                EnumLanguage tmpUserLang = (EnumLanguage)Enum.Parse(typeof(EnumLanguage), TAP.UI.InfoBase._USER_INFO.Language);

                string fressageFilePath = Path.Combine(_ExecutableDirectory, "mnls", TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.LocalFile);
                TapBase.Instance.FressagePath = fressageFilePath;
                this._conveter = new NeutralConverter(tmpUserLang, EnumUseFor.TEXT, false, fressageFilePath);
                this._translator = new TemplateConverter(tmpUserLang, false, fressageFilePath);

                _uiLog.WriteLog("LAUNCHER", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Setting user language....OK");
                AsyncMessage.Progress++;

                #endregion

                #region Load MDI Information

                AsyncMessage.Message = "Loading MDI Information....";
                this.LoadMDIInformation();
                _uiLog.WriteLog("LAUNCHER", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Loading MDI information....OK");
                AsyncMessage.Progress++;

                #endregion

                _isSuccessInit = true;
                return;
            }
            catch (System.Exception ex)
            {
                _uiLog.WriteLog("MDI", "E", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, ex.ToString());
                MessageBox.Show(ex.ToString());
                _isSuccessInit = false;
            }

            #endregion
        }

        private void LoadUserInformation()
        {
            #region Load User Information

            try
            {
                InfoBase._USER_INFO = new UserModel(this._userDefaultInfo, null, EnumFlagYN.YES);

                //this.lblUserNam.Text = InfoBase._USER_INFO.UserName;
                //this.lblIP.Text = InfoBase._USER_INFO.IPAddress;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void LoadMDIInformation()
        {
            #region Load MDI Information

            UIBasicDefaultInfo tmpUIDefaultInfo = null;

            try
            {
                tmpUIDefaultInfo = new UIBasicDefaultInfo();
                tmpUIDefaultInfo.Region = InfoBase._USER_INFO.Region;
                tmpUIDefaultInfo.Facility = InfoBase._USER_INFO.Facility;

                InfoBase._MDI_INFO = new MDIBasicModelSet();
                InfoBase._MDI_INFO.LoadModels(tmpUIDefaultInfo);

                return;
            }
            catch(System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void DownloadCrossLanguageFile()
        {
            #region DownloadCrossLanguageFile

            string tmpServerFile = string.Empty;
            string tmpLocalFile = string.Empty;

            try
            {
                tmpServerFile = TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.ServerDirectory;
                tmpLocalFile = TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.LocalFile;

                if (tmpServerFile.Length.Equals(0))
                {
                    //string tmpMessage = NeutralTranslator.Instance.TranslateSentence(EnumMsgTemplate.SET_EDNOT, "Fressage data");
                    //tmpMessage += "\r\n";
                    //tmpMessage += NeutralTranslator.Instance.TranslateSentence( EnumMsgTemplate.CONTACT_ORDER, "Administrator");

                    //string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.EDNOT, "Fressage server");
                    //TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, tmpMessage);
                    return;
                }

                if (File.Exists(tmpLocalFile))
                    File.Delete(tmpLocalFile);

                System.Net.WebClient tmpWebClient = new System.Net.WebClient();
                tmpWebClient.DownloadFile(tmpServerFile, tmpLocalFile);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void InitializeAppLink()
        {
            #region Initialize

            try
            {
                for(int i = 0; i <TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps.Count;i++)
                {
                    string tmpKey = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[i].Key;
                    TAP.UIControls.BasicControls.TPanel tmpPanel = null;

                    switch(tmpKey)
                    {
                        case "MES": tmpPanel = this.pnlMESBody; break;
                        case "ADMIN": tmpPanel = this.pnlAdminBody; break;
                    }

                    this.InitializeSubAppLink(tmpKey, tmpPanel);
                }
            }
            catch(System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void InitializeSubAppLink(string app, TAP.UIControls.BasicControls.TPanel panel)
        {
            #region Code

            int tmpMargin = 2;
            int tmpSpace = 10;

            try
            {
                for(int i = 0; i < TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[app].SubApps.Count; i++)
                {
                    TAP.UIControls.BasicControls.TPictureBox tmpPic = new UIControls.BasicControls.TPictureBox();

                    tmpPic.Width = 120;
                    tmpPic.Height = 60;
                    tmpPic.Location = new Point(tmpMargin, tmpMargin + (i * (tmpPic.Height + tmpSpace)));
                    tmpPic.Name = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[app].SubApps[i].Key;
                    
                    if(TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[app].SubApps[i].Enabled == true)
                        tmpPic.Tag = app;
                    else
                        tmpPic.Tag = "NA";

                    if(TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[app].SubApps[i].Image.Length > 0)
                    {
                        tmpPic.Image
                            = Image.FromFile(System.IO.Path.Combine(_imgDir, TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[app].SubApps[i].Image));

                        tmpPic.SizeMode = PictureBoxSizeMode.StretchImage;
                    }

                    if (TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[app].SubApps[i].HoverImage.Length > 0)
                    {
                        tmpPic.RollOverImage
                            = Image.FromFile(System.IO.Path.Combine(_imgDir, TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[app].SubApps[i].HoverImage));
                    }

                    tmpPic.Click += tmpPic_Click;
                    panel.Controls.Add(tmpPic);
                }
            }
            catch(System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        void tmpPic_Click(object sender, EventArgs e)
        {
            #region Code

            string tmpApp = string.Empty;
            string tmpKey = string.Empty;

            try
            {
                tmpApp = ((TAP.UIControls.BasicControls.TPictureBox)sender).Tag.ToString();
                tmpKey = ((TAP.UIControls.BasicControls.TPictureBox)sender).Name;

                if (tmpApp == "NA")
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate("Install", EnumGeneralTemplateType.EDNOT, "This application");
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, tmpMsg);
                }
                else
                {
                    Form1 tmpMDI = new Form1(tmpApp, tmpKey);
                    tmpMDI.Show();
                }

                return;
            }
            catch//(System.Exception ex)
            {

            }

            #endregion
        }

        private void FormLauncher_Load(object sender, EventArgs e)
        {
            #region Code

            try
            {
                this.Initialize();
            }
            catch (System.Exception ex)
            {
                if (!object.Equals(this._translator, null))
                {
                    string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.INITIALIZE, EnumGeneralTemplateType.FAIL, "UI");
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMessage, ex.ToString());
                }
                else
                    MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            #endregion
        }


    }
}
