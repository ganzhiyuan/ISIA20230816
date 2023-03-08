using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using TAP;
using TAP.Data.Client;
using TAP.Fressage;
using TAP.Models;
using TAP.Models.UIBasic;
using TAP.Models.User;
using TAP.UI;
using TAP.UIControls.BasicControls;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Controls;
using DevExpress.XtraBars.Forms;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraTab.ViewInfo;

namespace TAP.UI.MDI
{
    /// <summary>
    /// This is main MDI
    /// </summary>
    public partial class FormRibbon_ISEM : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        #region Contants

        /// <summary>
        /// Independent menu name
        /// </summary>
        protected const string _INDEPENDENT_MENU = "INDEPENDENT";
        
        /// <summary>
        /// Default user name
        /// </summary>
        public const string _CURR_USER = "TAP_UI";

        #endregion

        #region Field

        private bool _isSuccessInit;

        /// <summary>
        /// Main menu name
        /// </summary>
        protected string _mainMenu;

        /// <summary>
        /// Application directory
        /// </summary>
        protected string _appDirectory;

        private string _appName;

        private string _mdiName;

        private string _displayName;

        private System.Drawing.Rectangle _tabArea;

        private UILog _uiLog;

        private TAP.Fressage.NeutralConverter _conveter;

        private TAP.Fressage.TemplateConverter _translator;

        private List<UIBasicModel> _openedUIs;

        public static string _ExecutableDirectory;

        protected UserDefaultInfo _userDefaultInfo;

        private string _imgDir;

        private delegate void DoSetUserInfo(TextBox tbx, string txt);

        private bool _bookmark;

        private int[] _intSpreator;

        #endregion

        /// <summary>
        /// This creates instance of Form1
        /// </summary>
        public FormRibbon_ISEM()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appName">Application name</param>
        /// <param name="mdiName">MDI Name</param>
        public FormRibbon_ISEM(string appName, string mdiName)
        {
            InitializeComponent();

            _appName = appName;
            _mdiName = mdiName;
        }

        #region Initialize

        private void Initialize()
        {
            #region Initialize

            try
            {
                this._uiLog = new UILog();
                FormLogin2 tmpLogIn = new FormLogin2();

                if (tmpLogIn.ShowDialog().Equals(DialogResult.OK))
                {
                    this._userDefaultInfo = tmpLogIn._UserDefaultInfo;

                    _appName = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MDIName;
                    _mdiName = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MDIName;
                    _displayName = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MDIDisplayName;
                    this.Text = _displayName;
                    using (var bl = new TAP.UI.BackgroundProcessor(this.DoInitialize, this, 9L, true))
                    {
                        bl.Start();
                    }

                    if (!_isSuccessInit)
                    {
                        this.Dispose();
                        this.Close();
                        return;
                    }

                    //this._imgDir = Path.Combine(_ExecutableDirectory, "images");
                    //this.InitializeAppLink();
                }
                else
                {
                    tmpLogIn.Close();
                    tmpLogIn.Dispose();
                    this.Close();
                    return;
                }

                //_displayName = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[_appName].SubApps[_mdiName].DisplayName;
                //this.Text = _displayName;
                
                this.SetIcon();

                UICallBase.OpenChildUI += new UICallBase.OpenChildUIEventHandler(UICallBase_OpenChildUI);
                
                //this.tabMDIList.DrawMode = TabDrawMode.OwnerDrawFixed;
                //this.tabMDIList.DrawItem += new DrawItemEventHandler(tabMDIList_DrawItem);
                                
#if SYS
                //using (var bl = new TAP.UI.BackgroundProcessor(this.DoInitialize, this, 7L, true))
                //{
                //    bl.Start();
                //}

                //if (!_isSuccessInit)
                //{
                //    this.Dispose();
                //    this.Close();
                //    return;
                //}

                this.UserLogOn(true);
                //this.tabMDIList.Visible = false;

                _openedUIs = new List<UIBasicModel>();
                
                this.LoadTheme();

                //seoil
                this.InitializeXtraBar();

                //ADD KH 20190102 - Initial MDI Status
                if (TAP.App.Base.AppConfig.ConfigManager.HostCollection["MDIInit"]["Status"] == "FULL")
                    this.WindowState = FormWindowState.Maximized;
                else
                {
                    this.WindowState = FormWindowState.Normal;

                    if (TAP.Base.BIZ.BIZBase.Instance.IsNumeric(TAP.App.Base.AppConfig.ConfigManager.HostCollection["MDIInit"]["Width"]) &&
                        TAP.Base.BIZ.BIZBase.Instance.IsNumeric(TAP.App.Base.AppConfig.ConfigManager.HostCollection["MDIInit"]["Height"]))
                    {
                        this.Width = int.Parse(TAP.App.Base.AppConfig.ConfigManager.HostCollection["MDIInit"]["Width"]);
                        this.Height = int.Parse(TAP.App.Base.AppConfig.ConfigManager.HostCollection["MDIInit"]["Height"]);
                    }
                    else
                    {
                        this.Width = 800;
                        this.Height = 600;
                    }
                }

                #region Activate Startup UI

                if (TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.StartupUI.Enabled == true)
                {
                    string tmpStartupMainMenu = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.StartupUI.StartUpMainMenu.ToUpper();
                    string tmpStartupMenu = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.StartupUI.StartUpMenu.ToUpper();

                    if (tmpStartupMainMenu.Length > 0 && tmpStartupMenu.Length > 0)
                    {
                        this.OpenSYSUI(tmpStartupMainMenu, tmpStartupMenu);
                    }
                }

                #endregion
#endif
#if APP
                //»ç¿ëÀÚ Á¤º¸°¡ ÇÊ¿äÇÑ °æ¿ì°¡ ÀÖ±â ¶§¹®¿¡ ÀÓÀÇÀÇ »ç¿ëÀÚ Á¤º¸¸¦ ¸¸µç´Ù. 

                #region Create Fake User Infomation

                //UserModel tmpUser = null;
                this._userDefaultInfo = new UserDefaultInfo();
                _userDefaultInfo.Region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
                _userDefaultInfo.Facility = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Facility;
                _userDefaultInfo.UserID = "TAPAPP";
                _userDefaultInfo.User = "TAPAPP";
                this.LoadUserInformation();

                InfoBase._USER_INFO.Language = TAP.App.Base.AppConfig.ConfigManager.DefinedCollection["UserLang"];

                #endregion


                //Set Executable directory
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                this._executableDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
                this._appDirectory = Base.IO.FileBase.Instance.GetAPPDirectory(TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MDIName);

                //Download fressage file
                if( TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.NeedToDownLoad)
                    this.DownloadCrossLanguageFile();

                string fressageFilePath = Path.Combine(this._executableDirectory, "mnls", TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.LocalFile);
                EnumLanguage tmpUserLang = (EnumLanguage)Enum.Parse(typeof(EnumLanguage), TAP.App.Base.AppConfig.ConfigManager.DefinedCollection["UserLang"]);

                this._conveter = new NeutralConverter(tmpUserLang, EnumUseFor.TEXT, false, fressageFilePath);
                this._translator = new TemplateConverter(tmpUserLang, false, fressageFilePath);
                //NeutralTranslator.CreateInstance(tmpUserLang, false);

                this.Text = _MDI_DISPLAY_NAME;

                this.DrawMenu();
#endif
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        //void tabMDIList_DrawItem(object sender, DrawItemEventArgs e)
        //{
        //    #region Code

        //    string tmpText;

        //    Font tmpNormalFont = null;
        //    Font tmpSelectedFont = null;

        //    SolidBrush tmpNormalBrush = null;
        //    SolidBrush tmpSelectedBrush = null;
        //    SolidBrush tmpForeBrush = null;

        //    try
        //    {
        //        Graphics g = e.Graphics;

        //        tmpNormalFont = new Font("Tahoma", 8.25f);
        //        tmpSelectedFont = new Font("Tahoma", 8.25f);

        //        tmpNormalBrush = new SolidBrush(Color.FromArgb(255, 255, 255));
        //        tmpSelectedBrush = new SolidBrush(Color.RoyalBlue);
        //        tmpForeBrush = new SolidBrush(Color.Black);
                
        //        //tmpText = this.tabMDIList.TabPages[e.Index].Text;

        //        StringFormat tmpFormat = new StringFormat();
        //        tmpFormat.Alignment = StringAlignment.Center;

               
        //        if(this.tabMDIList.TabPages[e.Index] == this.tabMDIList.SelectedTab)
        //        {
        //            g.FillRectangle(tmpSelectedBrush, e.Bounds);
                    
        //            Rectangle r = e.Bounds;
        //            r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
        //            g.DrawString(tmpText, tmpSelectedFont, tmpForeBrush, r, tmpFormat);
        //        }
        //        else
        //        {
        //            g.FillRectangle(tmpNormalBrush, e.Bounds);

        //            Rectangle r = e.Bounds;
        //            r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
        //            g.DrawString(tmpText, tmpNormalFont, tmpForeBrush, r, tmpFormat);

        //        }

        //        return;
        //    }
        //    catch(System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        tmpNormalFont.Dispose();
        //        tmpSelectedFont.Dispose();

        //        tmpNormalBrush.Dispose();
        //        tmpSelectedBrush.Dispose();
        //        tmpForeBrush.Dispose();
        //    }

        //    #endregion
        //}

        private void DoInitialize()
        {
            #region Save Model

            try
            {
                #region Load User Information

                AsyncMessage.Message = "Loading user information....";
                this.LoadUserInformation();
                _uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Loading user information....OK");
                AsyncMessage.Progress++;

                #endregion

                #region Set Executable Directory

                AsyncMessage.Message = "Initializing Application executing environment";
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                _ExecutableDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";

                _uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Initializing Application executing environment.....OK");
                AsyncMessage.Progress++;

                #endregion

                #region Download DLLS

                AsyncMessage.Message = "Check last application files";

                try
                {
                    if (TAP.Base.Configuration.ConfigurationManager.Instance.RemoteDeploySection.ClientDeployInfo.NeedToDeploy)
                    {
                        Deploy deploy = new Deploy();
                        deploy.DeployFiles();
                        _uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Check last application files.....OK");
                        AsyncMessage.Progress++;
                    }
                }
                catch(System.Exception ex)
                {
                    if (TAP.Base.Configuration.ConfigurationManager.Instance.RemoteDeploySection.ClientDeployInfo.IgnoreExcetion == false)
                    throw ex;
                }

                #endregion 

                #region Download MNLS files

                AsyncMessage.Message = "Setting user language....";
                this.DownloadCrossLanguageFile();

                EnumLanguage tmpUserLang = (EnumLanguage)Enum.Parse(typeof(EnumLanguage), TAP.UI.InfoBase._USER_INFO.Language);

                string fressageFilePath = Path.Combine(_ExecutableDirectory, "mnls", TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.LocalFile);
                TapBase.Instance.FressagePath = fressageFilePath;
                this._conveter = new NeutralConverter(tmpUserLang, EnumUseFor.TEXT, false, fressageFilePath);
                this._translator = new TemplateConverter(tmpUserLang, false, fressageFilePath);

                _uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Setting user language....OK");
                AsyncMessage.Progress++;

                #endregion

                #region Load MDI Information

                AsyncMessage.Message = "Loading MDI Information....";
                this.LoadMDIInformation();
                _uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Loading MDI information....OK");
                AsyncMessage.Progress++;

                #endregion

                #region Set Application Directory

                //this._appDirectory = Base.IO.FileBase.Instance.GetAPPDirectory(TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[_appName].SubApps[_mdiName].AppDirectory);
                this._appDirectory = Base.IO.FileBase.Instance.GetAPPDirectory(TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.AppDirectory);
                this._appDirectory = Directory.GetCurrentDirectory();
                TapBase.Instance.ApplicationPath = _appDirectory;

                _uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Initializing Application executing environment.....OK");
                AsyncMessage.Progress++;

                #endregion

                #region Set MNLS

                //EnumLanguage tmpUserLang = (EnumLanguage)Enum.Parse(typeof(EnumLanguage), TAP.UI.InfoBase._USER_INFO.Language);

                //string fressageFilePath = Path.Combine(FormLauncher._ExecutableDirectory, "mnls", TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.LocalFile);
                //this._conveter = new NeutralConverter(tmpUserLang, EnumUseFor.TEXT, false, fressageFilePath);
                //this._translator = new TemplateConverter(tmpUserLang, false, fressageFilePath);

                //_uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Setting user language....OK");
                //AsyncMessage.Progress++;

                #endregion

                #region Initialize Resource

                AsyncMessage.Message = _translator.ConvertGeneralTemplate(EnumVerbs.INITIALIZE, EnumGeneralTemplateType.ING, "Resources");
                this.SetImageList();
                _uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Initializing Resources....OK");
                AsyncMessage.Progress++;

                #endregion

                #region Initialize MDI

                AsyncMessage.Message = _translator.ConvertGeneralTemplate(EnumVerbs.INITIALIZE, EnumGeneralTemplateType.ING, "MDI");
                this.LoadMenu();
                _uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Initializing MDI....OK");
                AsyncMessage.Progress++;

                #endregion

                #region Set User Group

                AsyncMessage.Message = _translator.ConvertGeneralTemplate(EnumVerbs.SET, EnumGeneralTemplateType.ING, "User Group Information");
                this.LoadUserGroupInformation();
                _uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Initializing User Group Information....OK");
                AsyncMessage.Progress++;

                #endregion

                #region Set Authority

                AsyncMessage.Message = _translator.ConvertGeneralTemplate(EnumVerbs.SET, EnumGeneralTemplateType.ING, "User Authority");
                this.DrawMenu();
                //this.SetMainButtons();
                _uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Initializing User Authority....OK");
                AsyncMessage.Progress++;

                #endregion

                _isSuccessInit = true;
                return;

                //#region Set Application Directory

                //this._appDirectory = Base.IO.FileBase.Instance.GetAPPDirectory(TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[_appName].SubApps[_mdiName].AppDirectory);
                //TapBase.Instance.ApplicationPath = _appDirectory;

                //_uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Initializing Application executing environment.....OK");
                //AsyncMessage.Progress++;

                //#endregion

                //#region Set MNLS

                //EnumLanguage tmpUserLang = (EnumLanguage)Enum.Parse(typeof(EnumLanguage), TAP.UI.InfoBase._USER_INFO.Language);

                //string fressageFilePath = Path.Combine(FormLauncher._ExecutableDirectory, "mnls", TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.LocalFile);
                //this._conveter = new NeutralConverter(tmpUserLang, EnumUseFor.TEXT, false, fressageFilePath);
                //this._translator = new TemplateConverter(tmpUserLang, false, fressageFilePath);
                
                //_uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Setting user language....OK");
                //AsyncMessage.Progress++;

                //#endregion

                //#region Initialize Resource

                //AsyncMessage.Message = _translator.ConvertGeneralTemplate(EnumVerbs.INITIALIZE, EnumGeneralTemplateType.ING, "Resources");
                //this.SetImageList();
                //_uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Initializing Resources....OK");
                //AsyncMessage.Progress++;

                //#endregion

                //#region Initialize MDI

                //AsyncMessage.Message = _translator.ConvertGeneralTemplate(EnumVerbs.INITIALIZE, EnumGeneralTemplateType.ING, "MDI");
                //this.LoadMenu();
                //_uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Initializing MDI....OK");
                //AsyncMessage.Progress++;

                //#endregion

                //#region Set User Group

                //AsyncMessage.Message = _translator.ConvertGeneralTemplate(EnumVerbs.SET, EnumGeneralTemplateType.ING, "User Group Information");
                //this.LoadUserGroupInformation();
                //_uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Initializing User Group Information....OK");
                //AsyncMessage.Progress++;

                //#endregion

                //#region Set Authority

                //AsyncMessage.Message = _translator.ConvertGeneralTemplate(EnumVerbs.SET, EnumGeneralTemplateType.ING, "User Authority");
                //this.DrawMenu();
                //_uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Initializing User Authority....OK");
                //AsyncMessage.Progress++;

                //#endregion

                //_isSuccessInit = true;
                //return;
            }
            catch (System.Exception ex)
            {
                _uiLog.WriteLog("MDI", "E", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, ex.ToString());
                MessageBox.Show(ex.ToString());
                _isSuccessInit = false;
            }

            #endregion
        }

        private void LoadMenu()
        {
            #region Private void UIs

            MDIBasicModel tmpMDIInfo = null;
            //MainMenu tmpMainMenu = null;
            //UIBasicDefaultInfo tmpUIDefaultInfo = null;

            string tmpRegion = string.Empty;
            string tmpFacility = string.Empty;
            string tmpMainMenuSql = string.Empty;
            string tmpUISql = string.Empty;

            DataSet tmpDsMainMenu = null;
            DataSet tmpDsUI = null;

            TAP.Data.Client.DataClient tmpDataClient = null;

            try
            {
                //tmpUIDefaultInfo = new UIBasicDefaultInfo();
                //tmpUIDefaultInfo.Region = InfoBase._USER_INFO.Region;
                //tmpUIDefaultInfo.Facility = InfoBase._USER_INFO.Facility;

                //tmpMDIInfo = InfoBase._MDI_INFO[TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[_appName].SubApps[_mdiName].Key.ToUpper()];

                tmpMDIInfo = InfoBase._MDI_INFO[TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MDIName];
                if (tmpMDIInfo.MainMenus == null || tmpMDIInfo.MainMenus.Count ==0)
                {
                    //tmpMDIInfo.LoadNestModel();
                    tmpRegion = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
                    tmpFacility = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Facility;

                    tmpMainMenuSql = "SELECT * FROM TAPSTBMAINMENU WHERE MDI = '{0}' AND REGION = '{1}' AND FACILITY = '{2}'  AND ISALIVE = 'YES' ";
                    tmpUISql = "SELECT * FROM TAPSTBUI WHERE  MDI = '{0}' AND REGION = '{1}' AND FACILITY = '{2}'  AND ISALIVE = 'YES'  ";
                    //tmpUISql = "SELECT * FROM TAPSTBUI WHERE  MDI = '{0}' AND REGION = '{1}' AND FACILITY = 'ALL'  AND ISALIVE = 'YES'  ";

                    tmpMainMenuSql = string.Format(tmpMainMenuSql, tmpMDIInfo.Name, tmpRegion, tmpFacility);
                    tmpUISql = string.Format(tmpUISql, tmpMDIInfo.Name, tmpRegion, tmpFacility);

                    tmpDataClient = new DataClient();
                    tmpDsMainMenu = tmpDataClient.SelectData(tmpMainMenuSql, "MAINMENU");
                    tmpDsUI = tmpDataClient.SelectData(tmpUISql, "UI");

                    for(int m = 0; m < tmpDsMainMenu.Tables["MAINMENU"].Rows.Count; m++)
                    {
                        #region Make Main Menu Model

                        UIBasicDefaultInfo tmpBasicInfo = new UIBasicDefaultInfo();
                        tmpBasicInfo.Region = tmpRegion;
                        tmpBasicInfo.Facility = tmpFacility;
                        tmpBasicInfo.MDI = tmpMDIInfo.Name;
                        tmpBasicInfo.MainMenu = tmpDsMainMenu.Tables["MAINMENU"].Rows[m]["NAME"].ToString();

                        MainMenuBasicModel tmpMainMenu
                            = new MainMenuBasicModel(tmpBasicInfo, tmpDsMainMenu.Tables["MAINMENU"].Rows[m]);

                        DataRow[] tmpUIData
                            = tmpDsUI.Tables["UI"].Select(string.Format("MAINMENU = '{0}' ", tmpMainMenu.Name));

                        for(int u = 0; u < tmpUIData.Length;u++)
                        {
                            #region Make UI Model

                            UIBasicDefaultInfo tmpUIBasicInfo = (UIBasicDefaultInfo)tmpBasicInfo.Copy();
                            tmpUIBasicInfo.UI = tmpUIData[u]["NAME"].ToString();

                            UIBasicModel tmpUI = new UIBasicModel(tmpUIBasicInfo, tmpUIData[u]);

                            tmpMainMenu.UIs.Add(tmpUI);

                            #endregion
                        }

                        tmpMDIInfo.MainMenus.Add(tmpMainMenu);


                        //MainMenuBasicModel tmpMainMenu = new MainMenuBasicModel()

                        #endregion
                    }

                }

                


                //tmpUIDefaultInfo.MDI = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[_appName].SubApps[_mdiName].Key.ToUpper();

                #region Load Models

                //tmpPack = new ArgumentPack();
                //tmpPack.AddArgument(Model._ARGUMENTKEY_TYPE, typeof(int), 1);
                //tmpPack.AddArgument(Model._ARGUMENTKEY_USER, typeof(string), _CURR_USER);
                //tmpPack.AddArgument(UIBasicModel._ARGUMENTKEY_REGION, typeof(string), tmpUIDefaultInfo.Region);
                //tmpPack.AddArgument(UIBasicModel._ARGUMENTKEY_FACILITY, typeof(string), tmpUIDefaultInfo.Facility);
                ////tmpPack.AddArgument(UIBasicModel._ARGUMENTKEY_MDI, typeof(string), tmpUIDefaultInfo.MDI);
                //tmpPack.AddArgument(Model._ARGUMENTKEY_IDENTIFICATION, typeof(string), tmpUIDefaultInfo.MDI);

                //if (InfoBase._MDI_INFO == null)
                //    InfoBase._MDI_INFO = new MDIBasicModelSet();

                //InfoBase._MDI_INFO.LoadModels(tmpUIDefaultInfo)


                //MainMenu tmpMainMenu = new MainMenu();

                //InfoBase._MENU_INFO = new MainMenuBasicModelSet();
                //InfoBase._MENU_INFO.LoadModels(tmpUIDefaultInfo);

                #endregion

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void LoadUserInformation()
        {
            #region Load User Information

            try
            {
                InfoBase._USER_INFO = new UserModel(this._userDefaultInfo, null, EnumFlagYN.YES);
                //this.txtUserName.Text = InfoBase._USER_INFO.UserName;
                //this.lblIP.Text = InfoBase._USER_INFO.IPAddress;
            }
            catch (System.Exception ex)
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

        private void LoadUserGroupInformation()
        {
            #region Load User Group Information

            UserDefaultInfo tmpUserDefaultInfo = null;
            UserGroupModelSet tmp = null;
            List<UserGroupModel> retVal = null;

            //seoil 2019-01-16 Groupmember °¡Á®¿À´Â ºÎºÐ Models ¿¡¼­ º¯°æÇØ¾ßÇÒµí.
            GroupMemberModelSet tmpGroupMember = null;
            TAP.Data.Client.DataClient tmpDBC = new Data.Client.DataClient();
            string tmpstr = "";
            try
            {
                if (InfoBase._USER_INFO == null)
                    return;

                tmpUserDefaultInfo = new UserDefaultInfo();
                tmpUserDefaultInfo.Region = InfoBase._USER_INFO.Region;
                //tmpUserDefaultInfo.Facility = InfoBase._USER_INFO.Facility;
                tmpUserDefaultInfo.Facility = "ALL";

                if (TAP.Base.Configuration.ConfigurationManager.Instance.DatabaseSection.Connections["DEFAULT"].DBMS == "ORACLE")
                    tmpstr = string.Format("SELECT USERGROUP FROM( SELECT USERGROUP FROM TAPUTGROUPMEMBER WHERE NAME = '{0}' ORDER BY USERGROUP ) WHERE rownum = 1", InfoBase._USER_INFO.Name);
                else if (TAP.Base.Configuration.ConfigurationManager.Instance.DatabaseSection.Connections["DEFAULT"].DBMS == "MSSQL")
                    tmpstr = string.Format("SELECT TOP 1 USERGROUP FROM TAPUTGROUPMEMBER WHERE NAME = '{0}'", InfoBase._USER_INFO.Name);
                else
                    tmpstr = string.Format("SELECT TOP 1 USERGROUP FROM TAPUTGROUPMEMBER WHERE NAME = '{0}'", InfoBase._USER_INFO.Name);

                DataTable dt = tmpDBC.SelectData(tmpstr,"USERGROUP").Tables[0];

                if (dt.Rows.Count == 0)
                {
                    return;
                }
                else
                {
                    tmpUserDefaultInfo.UserGroup = string.IsNullOrEmpty(dt.Rows[0][0].ToString()) ? "" : dt.Rows[0][0].ToString();
                }
                

                tmpGroupMember = new GroupMemberModelSet();
                tmpGroupMember.LoadModels(tmpUserDefaultInfo);


                if (tmpGroupMember.Count == 0)
                    return;

                foreach (GroupMemberModel tmpMember in tmpGroupMember.Models)
                {
                    if (tmpMember.Name == InfoBase._USER_INFO.Name)
                        InfoBase._USER_GROUP_MEMBER = tmpMember;
                }



                #region Muduri Ver

                //UserDefaultInfo tmpUserDefaultInfo = null;
                //UserGroupModelSet tmp = null;
                //List<UserGroupModel> retVal = null;
                //try
                //{
                //if (InfoBase._USER_INFO == null)
                //    return;

                //tmpUserDefaultInfo = new UserDefaultInfo();
                //tmpUserDefaultInfo.Region = InfoBase._USER_INFO.Region;
                //tmpUserDefaultInfo.Facility = InfoBase._USER_INFO.Facility;
                //tmp = new UserGroupModelSet();
                //tmp.LoadModels(tmpUserDefaultInfo);

                //retVal = new List<UserGroupModel>();

                //foreach( string tmpGrpName in tmp.Names)
                //{
                //    UserGroupModel tmpGroup = tmp[tmpGrpName];

                //    if (tmpGroup.Members == null || tmpGroup.Members.Count == 0)
                //        tmpGroup.LoadNestModel();

                //    if (tmpGroup.Members.Contains(InfoBase._USER_INFO.Name))
                //        retVal.Add(tmpGroup);
                //}
                //    return;
                //}
                //catch (System.Exception ex)
                //{
                //    throw ex;
                //}
                #endregion

                return;
            }
            catch(System.Exception ex)
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
                //tmpUIDefaultInfo.Facility = InfoBase._USER_INFO.Facility;
                tmpUIDefaultInfo.Facility = "ALL";

                InfoBase._MDI_INFO = new MDIBasicModelSet();
                InfoBase._MDI_INFO.LoadModels(tmpUIDefaultInfo);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void DrawMenu()
        {
            #region Draw Menu

            try
            {
               // this.CreateDefaultMenu();
                this.CreateMainMenu();
                //this.CreateLastMenu();

                this.CreateBookMarkMenu();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void CreateDefaultMenu()
        {
            #region Create Default Menu

            try
            {
                ToolStripMenuItem tmpMenu = new ToolStripMenuItem(this._conveter.ConvertPhrase("Opened Windows"));
                //this.menuStrip1.MdiWindowListItem = tmpMenu;
                //this.menuStrip1.Items.Add(tmpMenu);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void CreateLastMenu()
        {
            #region Create Last Menu

            try
            {
                ToolStripMenuItem tmpMenu = new ToolStripMenuItem(this._conveter.ConvertPhrase( "Help"));
                //this.menuStrip1.Items.Add(tmpMenu);

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
           
            #endregion
        }

        private void CreateMainMenu()
        {
            #region Create Main Menu

            string[] tmpMainMenuNames = null;
            CheckForIllegalCrossThreadCalls = false; //ÀÓ½Ã¹æÆí
            try
            {
#if SYS
                tmpMainMenuNames = InfoBase._MDI_INFO[this._mdiName].MainMenus.CreateSeqenceArray();

                //tmpMainMenuNames = InfoBase._MENU_INFO.CreateSeqenceArray();

                for (int i = 0; i < tmpMainMenuNames.Length; i++)
                {
                    if (!InfoBase._MDI_INFO[this._mdiName].MainMenus.IsContains(tmpMainMenuNames[i]))
                        continue; 

                    MainMenuBasicModel tmpMainMenuModel
                        = InfoBase._MDI_INFO[this._mdiName].MainMenus[tmpMainMenuNames[i]];

                    //MainMenuBasicModel tmpMainMenuModel = InfoBase._MENU_INFO[tmpMainMenuNames[i]];

                    if (tmpMainMenuModel.Name.Equals(_INDEPENDENT_MENU))
                        continue;

                    //RibbonTab tmpNewTab = new RibbonTab(this._conveter.ConvertPhrase(tmpMainMenuModel.DisplayName));
                    //tmpNewTab.Tag = tmpMainMenuModel.Name;
                    //tmpNewTab.Panels.Add(this.CreateMenu(tmpMainMenuModel));

                    //seoil start
                    
                    //BarSubItem tmpBarMenu = new BarSubItem(barManager, this._conveter.ConvertPhrase(tmpMainMenuModel.DisplayName)); //UpperString tmpMainMenuModel.DisplayName
                    //tmpBarMenu.Tag = tmpMainMenuModel.Name;
                    
                    //tmpBarMenu.AddItems(this.CreateMenuBarItem(tmpMainMenuModel));

                    //seoil ribbon
                    //Category->Page->Group->Item --Category Not Used.
                    RibbonPage tmpRibbonPage = new RibbonPage(this._conveter.ConvertPhrase(tmpMainMenuModel.DisplayName));//Create Page
                    tmpRibbonPage.Tag = tmpMainMenuModel.Name;
                    RibbonPageGroup tmpRibbonPageGroup = CreateMenuBarItemRibbon(tmpMainMenuModel);//Page->Group
                    tmpRibbonPageGroup.ShowCaptionButton = false;

                    //Spreator Function

                    if (_intSpreator.Length > 0)
                    {
                        foreach (int j in _intSpreator)
                        {
                            if(tmpRibbonPageGroup.ItemLinks.Count > j)
                                tmpRibbonPageGroup.ItemLinks[j].BeginGroup = true;
                        }
                    }


                    //The menu will not be displayed without the UI.
                    if (tmpRibbonPageGroup.ItemLinks.Count > 0)
                    {
                        tmpRibbonPage.Groups.Add(tmpRibbonPageGroup);
                        ribbonControl1.Pages.Add(tmpRibbonPage);
                    }
                    //seoil end

                    //this.ribbonMenu.Tabs.Add(tmpNewTab);

                    //ToolStripMenuItem tmpMainMenu = new ToolStripMenuItem(this._conveter.ConvertPhrase( tmpMainMenuModel.DisplayName));
                    //tmpMainMenu.Name = tmpMainMenuModel.Name;
                    ////tmpMainMenu.ShortcutKeys = Enum.IsDefined( typeof(Keys), tmpMainMenu.ShortcutKeys) ? (Keys)Enum.Parse(typeof(Keys), tmpMainMenuModel.ShortCutKeys) : Keys.None;
                    //tmpMainMenu.ShortcutKeyDisplayString = tmpMainMenuModel.ShortCutDisplayString;
                    //tmpMainMenu.ShowShortcutKeys = true;
                    ////tmpMainMenu.Click += new EventHandler(MainMenu_Click);
                    //tmpMainMenu.DropDownOpening += new EventHandler(MainMenu_Click);

                    //this.CreateMenu(tmpMainMenuModel, ref tmpMainMenu);
                    //this.menuStrip1.Items.Add(tmpMainMenu);
                }
#endif
#if APP
                for (int i = 0; i < TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus.Count; i++)
                {
                    string tmpDisplayName = this._conveter.ConvertPhrase(TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[i].DisplayName);
                    ToolStripMenuItem tmpMainMenu = new ToolStripMenuItem(tmpDisplayName);
                    tmpMainMenu.Name = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[i].Key;
                    tmpMainMenu.ShowShortcutKeys = true;
                    tmpMainMenu.Click += new EventHandler(MainMenu_Click);

                    this.CreateMenu(tmpMainMenu.Name, ref tmpMainMenu);
                    this.menuStrip1.Items.Add(tmpMainMenu);
                }
#endif
                //this.ribbonMenu.Minimized = true;
                CheckForIllegalCrossThreadCalls = true; //ÀÓ½Ã¹æÆí
                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void CreateBookMarkMenu()
        {
            #region Create BookMark Menu

            string[] tmpMainMenuNames = null;
            string tmpSql = null;
            TAP.Data.Client.DataClient tmpDBC = new Data.Client.DataClient();
            BarButtonItem tmpBar = null;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));

            try
            {
                popupMenuBookMark.ClearLinks();
                tmpBar = new BarButtonItem();
                tmpBar.Caption = this._conveter.ConvertPhrase("CURRENT UI ADD");
                tmpBar.CloseSubMenuOnClick = false;
                tmpBar.DropDownEnabled = false;
                tmpBar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnUIAdd.ImageOptions.Image")));
                tmpBar.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnUIAdd.ImageOptions.LargeImage")));
                tmpBar.Name = "btnUIAdd";
                tmpBar.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                tmpBar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUIAdd_ItemClick);


                popupMenuBookMark.AddItem(tmpBar);

                tmpSql = string.Format("SELECT * FROM TAPSTBUIBOOKMARK " +
                                       "WHERE NAME = '{0}' " +
                                         "AND MDI = '{1}' "  +
                                         "AND REGION = '{2}' " +
                                         "AND FACILITY = '{3}'",
                                         InfoBase._USER_INFO.Name, this._mdiName,InfoBase._USER_INFO.Region, InfoBase._USER_INFO.Facility);

                DataSet ds = tmpDBC.SelectData(tmpSql, "TAPSTBUIBOOKMARK");
                
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tmpMainMenuNames = InfoBase._MDI_INFO[this._mdiName].MainMenus.CreateSeqenceArray();

                    for (int i = 0; i < tmpMainMenuNames.Length; i++)
                    {
                        MainMenuBasicModel tmpMainMenuModel
                            = InfoBase._MDI_INFO[this._mdiName].MainMenus[tmpMainMenuNames[i]];

                        if (tmpMainMenuModel.Name.Equals(_INDEPENDENT_MENU))
                            continue;
                        if (tmpMainMenuModel.Name != dr["MAINMENU"].ToString())
                            continue;

                        popupMenuBookMark.AddItems(this.CreateBookMarkBarItem(tmpMainMenuModel, dr["UINAME"].ToString()));
                    }
                }
                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }


        //Create Menu For SYS
        // private void CreateMenu(MainMenuBasicModel mainMenuModel, ref ToolStripMenuItem mainMenuItem)
        private RibbonPanel CreateMenu(MainMenuBasicModel mainMenuModel)
        {
            #region Create Menu

            string[] tmpMenuNames = null;
            RibbonPanel retVal = null;
            
            try
            {
                if (mainMenuModel.UIs == null || mainMenuModel.UIs.Count.Equals(0))
                    mainMenuModel.LoadNestModel();

                tmpMenuNames = mainMenuModel.UIs.CreateSeqenceArray();

                retVal = new RibbonPanel();
                retVal.Text = mainMenuModel.DisplayName;

                for( int i = 0; i<  tmpMenuNames.Length; i++)
                {
                    UIBasicModel tmpUIModel = mainMenuModel.UIs[ tmpMenuNames[i] ];

                    if( this.CheckUserAuthority( tmpUIModel ))
                    {
                        //UI¿¡ ´ëÇÑ ±ÇÇÑ ¼³Á¤ÀÌ ÀÖ´Â °æ¿ì¸¸....

                        if (tmpUIModel.Name.IndexOf("SPREATOR") < 0)
                        {
                            RibbonButton tmpNewButton = new RibbonButton();

                            if (this.imageListMainMenu.Images.Count > 0 && this.imageListMainMenu.Images.Count > tmpUIModel.Icon)
                                tmpNewButton.Image = this.imageListMainMenu.Images[tmpUIModel.Icon];

                            tmpNewButton.Text = this._conveter.ConvertPhrase(tmpUIModel.DisplayName);

                            tmpNewButton.Tag = tmpUIModel.Name;

                            tmpNewButton.TextAlignment = RibbonItem.RibbonItemTextAlignment.Center;
                            tmpNewButton.MinimumSize = new Size(80, 70);
                            

                            //this.SetAuthority(tmpUIModel);
                            
                            tmpNewButton.Click += new EventHandler(Menu_Click);

                            retVal.Items.Add(tmpNewButton);

                            #region OLD

                            //ToolStripMenuItem tmpMenu = null;

                            //if (!object.Equals(this.imageListMainMenu.Images, null) &&
                            //    this.imageListMainMenu.Images.Count > 0 &&
                            //    this.imageListMainMenu.Images.Count > tmpUIModel.Icon)
                            //{
                            //    tmpMenu
                            //    = new ToolStripMenuItem(this._conveter.ConvertPhrase( tmpUIModel.DisplayName), this.imageListMainMenu.Images[tmpUIModel.Icon]);
                            //}
                            //else
                            //{
                            //    tmpMenu
                            //    = new ToolStripMenuItem(this._conveter.ConvertPhrase( tmpUIModel.DisplayName));
                            //}

                            //tmpMenu.Name = tmpUIModel.Name;

                            ////tmpMenu.ShortcutKeys = tmpUIModel.ShortCutKeys;
                            //tmpMenu.ShortcutKeyDisplayString = tmpUIModel.ShortCutDisplayString;
                            //tmpMenu.ShowShortcutKeys = true;


                            //tmpMenu.Click += new EventHandler(Menu_Click);

                            //mainMenuItem.DropDownItems.Add(tmpMenu);

                            #endregion
                        }
                            //ToolStripMenuItem tmpMenu = new ToolStripMenuItem("-");
                            //mainMenuItem.DropDownItems.Add("-");
                        
                    }
                }

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }
        //seoil

        private RibbonPageGroup CreateMenuBarItemRibbon(MainMenuBasicModel mainMenuModel)
        {
            #region Create Menu
            string[] tmpMenuNames = null;
            List<BarItem> tmpBarItem = new List<BarItem>();
            List<int> tmpSpreator = new List<int>();
            _intSpreator = null;
            int tmpIndex = 0;

            RibbonPageGroup tmpRibbonPageGroup = new RibbonPageGroup(this._conveter.ConvertPhrase(mainMenuModel.DisplayName));

            try
            {
                if (mainMenuModel.UIs == null || mainMenuModel.UIs.Count.Equals(0))
                    mainMenuModel.LoadNestModel();

                tmpMenuNames = mainMenuModel.UIs.CreateSeqenceArray();

                DataTable dtSubMenu = LoadSubMenu(mainMenuModel.Name);

                if (dtSubMenu.Rows.Count > 0)
                {
                    // add submenu
                    foreach (DataRow item in dtSubMenu.Rows)
                    {
                        BarSubItem tmpBarSubItem = new BarSubItem();
                        tmpBarSubItem.Name = item["NAME"].ToString();
                        tmpBarSubItem.Caption = this._conveter.ConvertPhrase(item["DISPLAYNAME"].ToString());
                        tmpBarSubItem.AllowRightClickInMenu = false;
                        tmpBarSubItem.Alignment = BarItemLinkAlignment.Default;
                        if (item["IMAGE"].ToString().ToUpper().Contains("SVG"))
                        {
                            tmpBarSubItem.ImageOptions.SvgImage = DevExpress.Utils.Svg.SvgImage.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", item["IMAGE"].ToString()));
                        }
                        else
                        {
                            tmpBarSubItem.ImageOptions.LargeImage = Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", item["IMAGE"].ToString()));
                        }
                        
                        tmpRibbonPageGroup.ItemLinks.Add(tmpBarSubItem);
                        this.ribbonControl1.Items.Add(tmpBarSubItem);
                        tmpBarSubItem.ItemClick += new ItemClickEventHandler(barSubItem1_ItemClick);
                    }

                    //add ui into submenu or mainmenu
                    for (int i = 0; i < tmpMenuNames.Length; i++)
                    {
                        UIBasicModel tmpUIModel = mainMenuModel.UIs[tmpMenuNames[i]];

                        if (this.CheckUserAuthority(tmpUIModel))
                        {
                            if (tmpUIModel.Name.IndexOf("SPREATOR") < 0)
                            {
                                BarButtonItem tmpNewButton = new BarButtonItem();
                                tmpNewButton.Name = mainMenuModel.Name;
                                tmpNewButton.Caption = this._conveter.ConvertPhrase(tmpUIModel.DisplayName);
                                tmpNewButton.Tag = tmpUIModel.Name;
                                tmpNewButton.AllowRightClickInMenu = false;
                                tmpNewButton.Alignment = BarItemLinkAlignment.Default;
                                tmpNewButton.ItemClick += new ItemClickEventHandler(MenuBarButtion_Click);


                                #region Load Image

                                if (tmpUIModel.ImageName != null && tmpUIModel.ImageName.Length > 0)
                                {

                                    if (File.Exists(Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", tmpUIModel.ImageName)))
                                        tmpNewButton.ImageOptions.LargeImage =
                                        Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", tmpUIModel.ImageName));

                                    if (tmpUIModel.SmallImageName != null && tmpUIModel.SmallImageName.Length > 0)
                                    {
                                        if (File.Exists(Path.Combine(this._appDirectory, "IMAGES", "MENU", "SMALL", tmpUIModel.ImageName)))
                                            tmpNewButton.ImageOptions.Image =
                                                Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "SMALL", tmpUIModel.ImageName));
                                    }
                                    else
                                    {
                                        if (File.Exists(Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", tmpUIModel.ImageName)))
                                            tmpNewButton.ImageOptions.Image =
                                            Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", tmpUIModel.ImageName));
                                    }
                                }
                                else
                                {                                    
                                    //tmpNewButton.ImageOptions.LargeImage =
                                    //    Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", "NoImage.png"));

                                    //tmpNewButton.ImageOptions.Image =
                                    //    Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "SMALL", "NoImage.png"));

                                }
                                #endregion

                                if (dtSubMenu.Select(string.Format("NAME = '{0}'", tmpUIModel.SubMenu)).Length > 0)
                                {
                                    this.ribbonControl1.Items.Add(tmpNewButton);
                                    BarSubItem tmp = this.ribbonControl1.Items.Where(s => s.Name == tmpUIModel.SubMenu).First() as BarSubItem;
                                    tmp.ItemLinks.Add(tmpNewButton);
                                }
                                else
                                {
                                    tmpRibbonPageGroup.ItemLinks.Add(tmpNewButton);
                                }

                                tmpIndex++;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < tmpMenuNames.Length; i++)
                    {
                        UIBasicModel tmpUIModel = mainMenuModel.UIs[tmpMenuNames[i]];

                        if (this.CheckUserAuthority(tmpUIModel))
                        {
                            if (tmpUIModel.Name.IndexOf("SPREATOR") < 0)
                            {
                                BarButtonItem tmpNewButton = new BarButtonItem();
                                tmpNewButton.Name = mainMenuModel.Name;
                                tmpNewButton.Caption = this._conveter.ConvertPhrase(tmpUIModel.DisplayName);
                                tmpNewButton.Tag = tmpUIModel.Name;
                                tmpNewButton.AllowRightClickInMenu = false;
                                tmpNewButton.Alignment = BarItemLinkAlignment.Default;
                                tmpNewButton.ItemClick += new ItemClickEventHandler(MenuBarButtion_Click);

                                #region Load Image

                                if (tmpUIModel.ImageName != null && tmpUIModel.ImageName.Length > 0)
                                {

                                    if (File.Exists(Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", tmpUIModel.ImageName)))
                                        tmpNewButton.ImageOptions.LargeImage =
                                        Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", tmpUIModel.ImageName));

                                    if (tmpUIModel.SmallImageName != null && tmpUIModel.SmallImageName.Length > 0)
                                    {
                                        if (File.Exists(Path.Combine(this._appDirectory, "IMAGES", "MENU", "SMALL", tmpUIModel.ImageName)))
                                            tmpNewButton.ImageOptions.Image =
                                                Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "SMALL", tmpUIModel.ImageName));
                                    }
                                    else
                                    {
                                        if (File.Exists(Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", tmpUIModel.ImageName)))
                                            tmpNewButton.ImageOptions.Image =
                                            Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", tmpUIModel.ImageName));
                                    }
                                }
                                else
                                {
                                    //tmpNewButton.ImageOptions.LargeImage =
                                    //    Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", "NoImage.png"));

                                    //tmpNewButton.ImageOptions.Image =
                                    //    Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "SMALL", "NoImage.png"));

                                }
                                #endregion

                                tmpRibbonPageGroup.ItemLinks.Add(tmpNewButton);
                                tmpIndex++;
                            }
                        }
                    }
                }          

                _intSpreator = tmpSpreator.ToArray();
                return tmpRibbonPageGroup;
                } 
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private BarItem[] CreateMenuBarItem(MainMenuBasicModel mainMenuModel)
        {
            #region Create Menu
            string[] tmpMenuNames = null;
            BarItem[] retVal = null;
            List<BarItem> tmpBarItem = new List<BarItem>();
            List<int> tmpSpreator = new List<int>();
            _intSpreator = null;
            int tmpIndex = 0;

            try
            {
                if (mainMenuModel.UIs == null || mainMenuModel.UIs.Count.Equals(0))
                    mainMenuModel.LoadNestModel();

                tmpMenuNames = mainMenuModel.UIs.CreateSeqenceArray();

                //retVal = new RibbonPanel();
                //retVal.Text = mainMenuModel.DisplayName;

                for (int i = 0; i < tmpMenuNames.Length; i++)
                {
                    UIBasicModel tmpUIModel = mainMenuModel.UIs[tmpMenuNames[i]];

                    if (this.CheckUserAuthority(tmpUIModel))
                    {
                        //UI¿¡ ´ëÇÑ ±ÇÇÑ ¼³Á¤ÀÌ ÀÖ´Â °æ¿ì¸¸....

                        if (tmpUIModel.Name.IndexOf("SPREATOR") < 0)
                        {
                            BarButtonItem tmpNewButton = new BarButtonItem();

                            //if (this.imageListMainMenu.Images.Count > 0 && this.imageListMainMenu.Images.Count > tmpUIModel.Icon)
                            //    tmpNewButton.ImageIndex = tmpUIModel.Icon;

                            #region Load Image

                            if (tmpUIModel.ImageName != null && tmpUIModel.ImageName.Length > 0)
                            {

                                if (File.Exists(Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", tmpUIModel.ImageName)))
                                    tmpNewButton.ImageOptions.LargeImage =
                                    Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", tmpUIModel.ImageName));

                                if (tmpUIModel.SmallImageName != null && tmpUIModel.SmallImageName.Length > 0)
                                {
                                    if (File.Exists(Path.Combine(this._appDirectory, "IMAGES", "MENU", "SMALL", tmpUIModel.ImageName)))
                                        tmpNewButton.ImageOptions.Image =
                                            Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "SMALL", tmpUIModel.ImageName));
                                }
                                else
                                {
                                    if (File.Exists(Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", tmpUIModel.ImageName)))
                                        tmpNewButton.ImageOptions.Image =
                                        Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", tmpUIModel.ImageName));
                                }
                            }
                            else
                            {
                                tmpNewButton.ImageOptions.LargeImage =
                                    Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", "NoImage.png"));

                                tmpNewButton.ImageOptions.Image =
                                    Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "SMALL", "NoImage.png"));

                            }

                            #endregion

                            tmpNewButton.Name = mainMenuModel.Name;
                            tmpNewButton.Caption = this._conveter.ConvertPhrase(tmpUIModel.DisplayName);
                            tmpNewButton.Tag = tmpUIModel.Name;
                            tmpNewButton.AllowRightClickInMenu = false;
                            tmpNewButton.Alignment = BarItemLinkAlignment.Default;
                            tmpNewButton.ItemClick += new ItemClickEventHandler(MenuBarButtion_Click);
                            //tmpNewButton.MinimumSize = new Size(80, 70);

                            //this.SetAuthority(tmpUIModel);

                            tmpBarItem.Add(tmpNewButton);

                            //UI Use Description For SPREATOR
                            //if (tmpUIModel.Description.IndexOf("_SPREATOR") >= 0 )
                            //{
                            //    tmpSpreator.Add(tmpIndex);
                            //}

                            tmpIndex++;

                            #region OLD

                            //ToolStripMenuItem tmpMenu = null;

                            //if (!object.Equals(this.imageListMainMenu.Images, null) &&
                            //    this.imageListMainMenu.Images.Count > 0 &&
                            //    this.imageListMainMenu.Images.Count > tmpUIModel.Icon)
                            //{
                            //    tmpMenu
                            //    = new ToolStripMenuItem(this._conveter.ConvertPhrase( tmpUIModel.DisplayName), this.imageListMainMenu.Images[tmpUIModel.Icon]);
                            //}
                            //else
                            //{
                            //    tmpMenu
                            //    = new ToolStripMenuItem(this._conveter.ConvertPhrase( tmpUIModel.DisplayName));
                            //}

                            //tmpMenu.Name = tmpUIModel.Name;

                            ////tmpMenu.ShortcutKeys = tmpUIModel.ShortCutKeys;
                            //tmpMenu.ShortcutKeyDisplayString = tmpUIModel.ShortCutDisplayString;
                            //tmpMenu.ShowShortcutKeys = true;


                            //tmpMenu.Click += new EventHandler(Menu_Click);

                            //mainMenuItem.DropDownItems.Add(tmpMenu);

                            #endregion
                        }
                        else
                        {
                            tmpSpreator.Add(tmpIndex);
                        }
                        
                    }
                }

                _intSpreator = tmpSpreator.ToArray();
                retVal = tmpBarItem.ToArray();
                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }
        private BarItem[] CreateBookMarkBarItem(MainMenuBasicModel mainMenuModel, string uiName)
        {
            #region Create BookMark Menu
            string[] tmpMenuNames = null;
            BarItem[] retVal = null;
            List<BarItem> tmpBarItem = new List<BarItem>();
            try
            {
                if (mainMenuModel.UIs == null || mainMenuModel.UIs.Count.Equals(0))
                    mainMenuModel.LoadNestModel();

                tmpMenuNames = mainMenuModel.UIs.CreateSeqenceArray();
                                
                for (int i = 0; i < tmpMenuNames.Length; i++)
                {
                    if (tmpMenuNames[i].Equals(uiName))
                    {
                        UIBasicModel tmpUIModel = mainMenuModel.UIs[tmpMenuNames[i]];

                        if (this.CheckUserAuthority(tmpUIModel))
                        {
                            //UI¿¡ ´ëÇÑ ±ÇÇÑ ¼³Á¤ÀÌ ÀÖ´Â °æ¿ì¸¸....

                            if (tmpUIModel.Name.IndexOf("SPREATOR") < 0)
                            {
                                BarButtonItem tmpNewButton = new BarButtonItem();
                                //if (this.imageListMainMenu.Images.Count > 0 && this.imageListMainMenu.Images.Count > tmpUIModel.Icon)
                                //    tmpNewButton.ImageIndex = tmpUIModel.Icon;

                                #region Load Image

                                if (tmpUIModel.ImageName != null && tmpUIModel.ImageName.Length > 0)
                                {

                                    if (File.Exists(Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", tmpUIModel.ImageName)))
                                        tmpNewButton.ImageOptions.LargeImage =
                                        Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", tmpUIModel.ImageName));

                                    if (tmpUIModel.SmallImageName != null && tmpUIModel.SmallImageName.Length > 0)
                                    {
                                        if (File.Exists(Path.Combine(this._appDirectory, "IMAGES", "MENU", "SMALL", tmpUIModel.ImageName)))
                                            tmpNewButton.ImageOptions.Image =
                                                Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "SMALL", tmpUIModel.ImageName));
                                    }
                                    else
                                    {
                                        if (File.Exists(Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", tmpUIModel.ImageName)))
                                            tmpNewButton.ImageOptions.Image =
                                            Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", tmpUIModel.ImageName));
                                    }
                                }
                                else
                                {
                                    //tmpNewButton.ImageOptions.LargeImage =
                                    //    Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "BIG", "NoImage.png"));

                                    //tmpNewButton.ImageOptions.Image =
                                    //    Image.FromFile(System.IO.Path.Combine(this._appDirectory, "IMAGES", "MENU", "SMALL", "NoImage.png"));

                                }

                                #endregion

                                tmpNewButton.Name = tmpUIModel.MainMenu;
                                tmpNewButton.Caption = this._conveter.ConvertPhrase(tmpUIModel.DisplayName);
                                tmpNewButton.Tag = tmpUIModel.Name;
                                tmpNewButton.Description = tmpUIModel.AssemblyName;
                                tmpNewButton.AllowRightClickInMenu = false;

                                tmpNewButton.DropDownControl = popupMenu1;

                                tmpNewButton.Alignment = BarItemLinkAlignment.Default;

                                //tmpNewButton.MinimumSize = new Size(80, 70);
                                //this.SetAuthority(tmpUIModel);

                                tmpNewButton.ItemClick += new ItemClickEventHandler(MenuBarButtion_Click);
                                tmpBarItem.Add(tmpNewButton);
                                                               
                            }
                            else
                            {
                                BarButtonItem tmpNewButton = new BarButtonItem();
                                tmpNewButton.Caption = "-";
                                tmpBarItem.Add(tmpNewButton);
                            }
                        }
                    }
                }
                retVal = tmpBarItem.ToArray();
                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }
        
        private void SetAuthority(UIBasicModel uiModel)
        {
            #region Set Authority

            try
            {
                if (uiModel.Containers == null || uiModel.Containers.Count == 0)
                    uiModel.LoadNestModel();

                if (uiModel.Containers.Count == 0)
                    return;

                foreach(string tmpContainerName in uiModel.Containers.Names)
                {
                    ContainerBasicModel tmpContainerModel = uiModel.Containers[tmpContainerName];

                    if (tmpContainerModel.UIFunctions == null || tmpContainerModel.UIFunctions.Count == 0)
                        tmpContainerModel.LoadNestModel();

                    if (tmpContainerModel.UIFunctions.Count == 0)
                        continue;

                    foreach(string tmpFuncName in tmpContainerModel.UIFunctions.Names)
                    {
                        UIFunctionBasicModel tmpFuncModel = tmpContainerModel.UIFunctions[tmpFuncName];

                        if (tmpFuncModel.AllowedToEveryone == EnumFlagYN.YES)
                            continue;

                        if (tmpFuncModel.Authorities == null || tmpFuncModel.Authorities.Count == 0)
                            tmpFuncModel.LoadNestModel();
                    }
                }
            }
            catch(System.Exception ex )
            {
                throw ex;
            }

            #endregion
        }

        //Create Menu For APP
        private void CreateMenu(string menuName, ref ToolStripMenuItem mainMenuItem)
        {
            #region Create Menu

            string tmpSql = string.Empty;

            try
            {

                for (int i = 0; i < TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[menuName].Menus.Count; i++)
                {
                    string tmpDisplayName = this._conveter.ConvertPhrase(TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[menuName].Menus[i].DisplayName);
                    ToolStripMenuItem tmpMenu = null;


                    if (tmpDisplayName != "-")
                    {
                        if (!object.Equals(this.imageListMainMenu.Images, null) &&
                            this.imageListMainMenu.Images.Count > 0 &&
                            this.imageListMainMenu.Images.Count > TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[menuName].Menus[i].Icon &&
                            TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[menuName].Menus[i].Icon > -1)
                        {
                            tmpMenu
                            = new ToolStripMenuItem(tmpDisplayName,
                           this.imageListMainMenu.Images[TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[menuName].Menus[i].Icon]);
                        }
                        else
                        {
                            tmpMenu = new ToolStripMenuItem(tmpDisplayName);
                        }

                        tmpMenu.Name = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[menuName].Menus[i].Key;
                        tmpMenu.ShowShortcutKeys = true;
                        tmpMenu.Click += new EventHandler(Menu_Click);
                        mainMenuItem.DropDownItems.Add(tmpMenu);
                    }
                    else
                        mainMenuItem.DropDownItems.Add("-");


                }

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void UserLogOn(bool logon)
        {
            #region User Log on

            try
            {
                if (logon)
                {
                    InfoBase._USER_INFO.ChangeOneModelLastEvent(
                        "LOGON", EnumEventFlag.N, DateTime.Now, this._mdiName);
                }
                else
                {
                    InfoBase._USER_INFO.ChangeOneModelLastEvent(
                        "LOGOUT", EnumEventFlag.N, DateTime.Now, this._mdiName);
                }
                
                InfoBase._USER_INFO.Save(InfoBase._USER_INFO.Name);

                return;
            }
            catch(System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void SetIcon()
        {
            #region Code

            try
            {
                switch(this._mdiName)
                {
                    case "OIP": this.Icon = TAP.UI.MDI.Properties.Resources.oip; break;
                    case "MODELER": this.Icon = TAP.UI.MDI.Properties.Resources.modelerR1; break;
                    case "ADMIN": this.Icon = TAP.UI.MDI.Properties.Resources.admin; break;
                    case "ISEM": this.Icon = TAP.UI.MDI.Properties.Resources.ISEM_B_64; break;
                    default: this.Icon = TAP.UI.MDI.Properties.Resources.tap_ico; break;
                }

                return;
            }
            catch(System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void InitializeXtraBar()
        {
            #region Code

            try
            {
                _bookmark = false;

                btnLoginInfo.Caption = InfoBase._USER_INFO.Name.ToString(); //top
                barButtonLogout.Caption = this._conveter.ConvertPhrase("LOGOUT");
                barButtonClose.Caption = this._conveter.ConvertPhrase("CLOSE");
                btnBookMarkDelete.Caption = this._conveter.ConvertPhrase("DELETE");
                btnBookMark.Caption = this._conveter.ConvertPhrase("BOOKMARK");
                btnPassword.Caption = this._conveter.ConvertPhrase("CHANGE PASSWORD");
                //btnBottomLoginInfo.Caption = InfoBase._USER_INFO.Name.ToString(); //bottom
            }
            catch(System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Open UI

        private void OpenSYSUI(string mainMenu, string menu)
        {
            #region Open UI

            Form tmpForm = null;
            MainMenuBasicModel tmpMainMenu = null;
            UIBasicModel tmpUI = null;

            try
            {
                tmpMainMenu = InfoBase._MDI_INFO[this._mdiName].MainMenus[mainMenu];
                //tmpMainMenu = InfoBase._MENU_INFO[mainMenu];

                if (object.Equals(tmpMainMenu, null))
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, string.Format("Main menu '{0}'>", mainMenu));
                    throw new Exception(tmpMsg);
                }

                tmpUI = tmpMainMenu.UIs[menu];

                if (object.Equals(tmpMainMenu, null))
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, string.Format("<User Interface model '{0}'>", menu));
                    throw new Exception(tmpMsg);
                }

                //ÇØ´ç UI°¡ ÀÌ¹Ì ¿­·ÁÀÖ´ÂÁö Ã¼Å©
                tmpForm = this.CheckOpendForms(tmpUI);

                if (object.Equals(tmpForm, null))
                {
                    //ÇØ´ç UI°¡ ¿­·ÁÀÖÁö ¾ÊÀ¸¸é ·Îµå

                    Assembly a = null;
                    Type tmpType = null;
                    object tmpObject = null;
                    Form tmpNewForm = null;

                    a = Assembly.LoadFile( Path.Combine(this._appDirectory, tmpUI.AssemblyFileName));

                    if (object.Equals(a, null))
                    {
                        string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, string.Format("Assembly file '{0}'>", tmpUI.AssemblyFileName));
                        throw new Exception(tmpMsg);
                    }

                    tmpType = a.GetType(tmpUI.AssemblyName);

                    if (object.Equals(tmpType, null))
                    {
                        string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, string.Format("Assembly '{0}'>", tmpUI.AssemblyName));
                        throw new Exception(tmpMsg);
                    }

                    //»ý¼º
                    tmpObject = Activator.CreateInstance(tmpType);

                    //½ÇÇà
                    tmpNewForm = (Form)tmpObject;
                    tmpNewForm.MdiParent = this;
                    tmpNewForm.FormBorderStyle = FormBorderStyle.None;
                    tmpNewForm.Dock = DockStyle.Fill;

                    //seoil
                    ((TAP.UI.UIBase)tmpNewForm).SetBookMarkHandler += new TAP.UI.UIBase.SetBookMarkEventHandler(BookMarkModify);
                    

                    //tmpNewForm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
#if MODELER
                    this.Text = "TAP Modeler::" + this._conveter.ConvertPhrase(InfoBase._MENU_INFO[mainMenu].UIs[menu].DisplayName);
#endif
#if WORKFLOW
                    this.Text = "TAP Workflower::" + this._conveter.ConvertPhrase(InfoBase._MENU_INFO[mainMenu].UIs[menu].DisplayName);
#endif
                    tmpNewForm.Name = this.MakeUITitle(InfoBase._MDI_INFO[this._mdiName].MainMenus[mainMenu].DisplayName, tmpUI.DisplayName);
                    ((TAP.UI.UIBase)tmpNewForm).UIInformation = tmpUI;
                    //((TAP.UI.UIBase)tmpNewForm).TabControl = this.tabMDIList;
                    //((TAP.UI.UIBase)tmpNewForm).UITitle = this.MakeUITitle(InfoBase._MDI_INFO[this._mdiName].MainMenus[mainMenu].DisplayName, tmpUI.DisplayName);
                    ((TAP.UI.UIBase)tmpNewForm).UITitle = this.MakeUITitle(InfoBase._MDI_INFO[this._mdiName].MainMenus[mainMenu].UIs[menu]);

                    //TabPage tmpTabPage = new TabPage();
                    // tmpTabPage.Parent = this.tabMDIList;
                    // tmpTabPage.Text = this._conveter.ConvertPhrase(tmpUI.DisplayName);
                    // tmpTabPage.ImageIndex = tmpUI.Icon;
                    // tmpTabPage.Show();

                    //_tabArea = this.tabMDIList.GetTabRect(tabMDIList.TabPages.Count - 1);

                    //((TAP.UI.UIBase)tmpNewForm).TabPage = tmpTabPage;
                    //this.tabMDIList.SelectedTab = tmpTabPage;

                    TAP.Remoting.Caller.CallerInfo.UserID = InfoBase._USER_INFO.UserName;
                    TAP.Remoting.Caller.CallerInfo.ClientPort = TAP.Base.Configuration.ConfigurationManager.Instance.RemoteAdapterSection.LocalPort;
                    TAP.Remoting.Caller.CallerInfo.MDIName = this._mdiName;
                    TAP.Remoting.Caller.CallerInfo.FunctionName = tmpUI.AssemblyName;

                    //TabPage tmpNewPage = new TabPage(this._conveter.ConvertPhrase(tmpUI.DisplayName));
                    //tmpNewPage.Tag = mainMenu + "." + menu;
                    //tmpNewPage.Controls.Add(tmpNewForm);
                    //this.tapMDI.TabPages.Add(tmpNewPage);
                    //this.tapMDI.SelectedTab = tmpNewPage;
                    
                    tmpNewForm.Show();
                    this.xtraTabbedMdiManager1.Pages[this.xtraTabbedMdiManager1.Pages.Count - 1].Text = this._conveter.ConvertPhrase(tmpUI.DisplayName);
                    this.xtraTabbedMdiManager1.Pages[this.xtraTabbedMdiManager1.Pages.Count - 1].ShowCloseButton = DevExpress.Utils.DefaultBoolean.True;
                    if (tmpUI.AssemblyName.Equals("TAP.Modeler.FormModelList"))
                    {
                        switch (menu)
                        {
                            case "USER":
                                ArgumentPack tmpPack = new ArgumentPack();
                                tmpPack.AddArgument( "MODEL", typeof(ModelSet), new TAP.Models.User.UserModelSet());
                                ((TAP.UI.UIBase)tmpNewForm).ExecuteCommand(tmpPack);
                                break;
                        }
                    }
                }
                else
                    tmpForm.Activate();

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void OpenAPPUI(string mainMenu, string menu)
        {
            #region Open UI

            //DataRow[] tmpCurrMenu = null;
            string tmpAssemblyName = string.Empty;
            string tmpAssemblyFileName = string.Empty;
            string tmpArguments = string.Empty;
            Form tmpForm = null;
            ArgumentPack tmpPack = null;

            try
            {
                //tmpCurrMenu = this._menus.Tables[0].Select(" MENUNAME = '" + menu + "' ");

                //if (tmpCurrMenu.Length.Equals(0))
                //    throw new Exception(string.Format("TAP User intface model '{0}' does not exist.", menu));

                //ÇØ´ç UI°¡ ÀÌ¹Ì ¿­·ÁÀÖ´ÂÁö Ã¼Å©
                tmpPack = new ArgumentPack();
                tmpAssemblyFileName = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[mainMenu].Menus[menu].AssemblyFileName;
                tmpAssemblyName = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[mainMenu].Menus[menu].AssemblyName;
                tmpPack.AddArgument("A_ARGUMENT", typeof(string), TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[mainMenu].Menus[menu].Argument);
                tmpForm = this.CheckOpendForms(tmpAssemblyName);

                tmpArguments = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[mainMenu].Menus[menu].Argument;

                string tmpDisplayName = this._conveter.ConvertPhrase(TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[mainMenu].Menus[menu].DisplayName);

                if (object.Equals(tmpForm, null))
                {
                    //ÇØ´ç UI°¡ ¿­·ÁÀÖÁö ¾ÊÀ¸¸é ·Îµå

                    Assembly a = null;
                    Type tmpType = null;
                    object tmpObject = null;
                    Form tmpNewForm = null;

                    //MessageBox.Show(this._appDirectory);
                    //MessageBox.Show(tmpAssemblyFileName);
                    //MessageBox.Show(Path.Combine(this._appDirectory, tmpAssemblyFileName));

                    a = Assembly.LoadFile(Path.Combine(this._appDirectory, tmpAssemblyFileName));

                    if (object.Equals(a, null))
                    {
                        string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, string.Format("Assembly file <'{0}'>", tmpAssemblyFileName));
                        throw new Exception(tmpMsg);
                    }

                    tmpType = a.GetType(tmpAssemblyName);

                    if (object.Equals(tmpType, null))
                    {
                        string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, string.Format("Assembly <'{0}'>", tmpAssemblyName));
                        throw new Exception(tmpMsg);
                    }
                       
                    //»ý¼º
                    tmpObject = Activator.CreateInstance(tmpType);

                    //½ÇÇà
                    tmpNewForm = (Form)tmpObject;
                    tmpNewForm.MdiParent = this;

                    if (mainMenu.Equals("SETUP"))
                    {
                        tmpNewForm.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                        tmpNewForm.Dock = DockStyle.None;
                        tmpNewForm.StartPosition = FormStartPosition.CenterParent;
                    }
                    else
                    {
                        tmpNewForm.FormBorderStyle = FormBorderStyle.None;
                        tmpNewForm.Dock = DockStyle.Fill;
                        ((UIBase)tmpNewForm).UIBaseArgument = tmpArguments;
                    }

                    //tmpNewForm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                    tmpNewForm.Text = tmpDisplayName;

                    if (tmpPack["A_ARGUMENT"].ArgumentValue.ToString().Length > 0)
                        ((UIBase)tmpNewForm).ExecuteCommand(tmpPack);

                    tmpNewForm.Show();

                    //if (tmpUI.AssemblyName.Equals("TAP.Modeler.FormModelList"))
                    //{
                    //    switch (menu)
                    //    {
                    //        case "USERMODELER":
                    //            ArgumentPack tmpPack = new ArgumentPack();
                    //            tmpPack.AddArgument("MODEL", typeof(ModelSet), new TAP.Models.User.UserUserModelSet());
                    //            ((TAP.UI.UIBase)tmpNewForm).ExecuteCommand(tmpPack);
                    //            break;
                    //    }
                    //}
                }
                else
                    tmpForm.Activate();

                this.Text = tmpDisplayName + "::" + _displayName;

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        //FOR SYS
        private Form CheckOpendForms(UIBasicModel tmpUI)
        {
            #region Check Opend Form

            try
            {
                _openedUIs.Add(tmpUI);
                _openedUIs.Remove(tmpUI);

                for (int i = 0; i < this.MdiChildren.Length; i++)
                {
                    if (this.MdiChildren[i].GetType().ToString().Equals(tmpUI.AssemblyName))
                        return this.MdiChildren[i];
                }

                return null;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void BookMarkModify(bool falg, UIBasicModel tmpUI)
        {
            #region Code SQL

            TAP.Data.Client.DataClient tmpDBC = new Data.Client.DataClient();
            string tmpMainMenu = string.Empty;
            string tmpUIName = string.Empty;
            string tmpUser = string.Empty;
            string tmpRegion = string.Empty;
            string tmpFacility = string.Empty;
            string tmpQuery = string.Empty;
            string tmpMdiName = string.Empty;
            try
            {
                tmpMainMenu = tmpUI.MainMenu;
                tmpUIName = tmpUI.Name;
                tmpUser = InfoBase._USER_INFO.Name;
                tmpRegion = InfoBase._USER_INFO.Region;
                tmpFacility = InfoBase._USER_INFO.Facility;
                tmpMdiName = tmpUI.MDI;

                if (falg)
                {
                    tmpQuery = string.Format("INSERT INTO TAPSTBUIBOOKMARK(" +
                                                                    "NAME, " +
                                                                    "MDI, " +
                                                                    "MAINMENU, " +
                                                                    "UINAME, " +
                                                                    "REGION, " +
                                                                    "FACILITY" +
                                                                    ")" +
                                                                    "VALUES(" +
                                                                    "'{0}', " +
                                                                    "'{1}', " +
                                                                    "'{2}', " +
                                                                    "'{3}', " +
                                                                    "'{4}', " +
                                                                    "'{5}'" +
                                                                    ")", tmpUser, tmpMdiName, tmpMainMenu, tmpUIName, tmpRegion, tmpFacility);

                    tmpDBC.ModifyData(tmpQuery);
                }
                else
                {
                    tmpQuery = string.Format("DELETE TAPSTBUIBOOKMARK WHERE " +
                                                                        "NAME = '{0}' AND " +
                                                                        "MDI = '{1}' AND " +
                                                                        "MAINMENU = '{2}' AND " +
                                                                        "UINAME = '{3}' AND " +
                                                                        "REGION = '{4}' AND " +
                                                                        "FACILITY = '{5}'"
                                                                        , tmpUser, tmpMdiName, tmpMainMenu, tmpUIName, tmpRegion, tmpFacility);

                    tmpDBC.ModifyData(tmpQuery);
                }

                this.CreateBookMarkMenu();
            
            }
            catch (System.Exception ex)
            {
                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMAL, "BookMark");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMessage, ex.ToString());
            }

            #endregion
        }

        private void BookAssginOpendForms(string MainMenu, string UIName,string tmpAssmblyName)
        {
            #region BookAssgin Opend Form

            bool tmpCheck = true;
            TAP.Data.Client.DataClient tmpDBC = new Data.Client.DataClient();
            string tmpMainMenu = string.Empty;
            string tmpUIName = string.Empty;
            string tmpUser = string.Empty;
            string tmpRegion = string.Empty;
            string tmpFacility = string.Empty;
            string tmpQuery = string.Empty;
            string tmpMdiName = string.Empty;


            try
            {
                for (int i = 0; i < this.MdiChildren.Length; i++)
                {
                    if (this.MdiChildren[i].GetType().ToString().Equals(tmpAssmblyName))
                    {
                        ((TAP.UI.UIBase)this.MdiChildren[i]).BookMarkAssign = false;
                        tmpCheck = true;
                    }
                }
                if(tmpCheck)
                {
                    _bookmark = true;
                }

                tmpMainMenu = MainMenu;
                tmpUIName = UIName;
                tmpUser = InfoBase._USER_INFO.Name;
                tmpRegion = InfoBase._USER_INFO.Region;
                tmpFacility = InfoBase._USER_INFO.Facility;
                tmpMdiName = this._mdiName;

                tmpQuery = string.Format("DELETE TAPSTBUIBOOKMARK WHERE " +
                                                                    "NAME = '{0}' AND " +
                                                                    "MDI = '{1}' AND " +
                                                                    "MAINMENU = '{2}' AND " +
                                                                    "UINAME = '{3}' AND " +
                                                                    "REGION = '{4}' AND " +
                                                                    "FACILITY = '{5}'"
                                                                    , tmpUser, tmpMdiName, tmpMainMenu, tmpUIName, tmpRegion, tmpFacility);

                tmpDBC.ModifyData(tmpQuery);

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        //FOR APP
        private Form CheckOpendForms(string assemblyName)
        {
            #region Check Opend Form

            try
            {
                for (int i = 0; i < this.MdiChildren.Length; i++)
                {
                    if (this.MdiChildren[i].GetType().ToString().Equals(assemblyName))
                        return this.MdiChildren[i];
                }

                return null;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }


            #endregion
        }

        private void ActivateUI(string mainMenu, string menu, ArgumentPack arguments)
        {
            #region Open Child Open UI

            Form tmpForm = null;
            MainMenuBasicModel tmpMainMenu = null;
            UIBasicModel tmpUI = null;

            try
            {
                tmpMainMenu = InfoBase._MDI_INFO[this._mdiName].MainMenus[mainMenu];
                //tmpMainMenu = InfoBase._MENU_INFO[mainMenu];

                if (object.Equals(tmpMainMenu, null))
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, string.Format("Main menu model '{0}'>", mainMenu));
                    throw new Exception(tmpMsg);
                }

                tmpUI = tmpMainMenu.UIs[menu];

                if (object.Equals(tmpMainMenu, null))
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, string.Format("menu model '{0}'>", menu));
                    throw new Exception(tmpMsg);
                }

                if (FindActiveUI(tmpUI, arguments))
                    return;

                //Assembly a = Assembly.LoadFile(this._executableDirectory + tmpUI.AssemblyFileName);
                Assembly a = Assembly.LoadFile(Path.Combine(this._appDirectory, tmpUI.AssemblyFileName));

                if (object.Equals(a, null))
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, string.Format("Assembly file '{0}'>", tmpUI.AssemblyFileName));
                    throw new Exception(tmpMsg);
                }

                Type tmpType = a.GetType(tmpUI.AssemblyName);

                if (object.Equals(tmpType, null))
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, string.Format("Assembly '{0}'>", tmpUI.AssemblyName));
                    throw new Exception(tmpMsg);
                }

                tmpForm = (Form)Activator.CreateInstance(tmpType);
                tmpForm.MdiParent = this;
                tmpForm.FormBorderStyle = FormBorderStyle.None;
                tmpForm.Dock = DockStyle.Fill;
                tmpForm.Text = this._conveter.ConvertPhrase(tmpUI.DisplayName);

                ((TAP.UI.UIBase)tmpForm).UIInformation = tmpUI;
                //((TAP.UI.UIBase)tmpForm).TabControl = this.tabMDIList;
                ((TAP.UI.UIBase)tmpForm).UITitle = this.MakeUITitle(InfoBase._MDI_INFO[this._mdiName].MainMenus[mainMenu].DisplayName, tmpUI.DisplayName);

                //TabPage tmpTabPage = new TabPage();
                //tmpTabPage.Parent = this.tabMDIList;
                //tmpTabPage.Text = this._conveter.ConvertPhrase(tmpUI.DisplayName);
                //tmpTabPage.ImageIndex = tmpUI.Icon;
                //tmpTabPage.Show();

                //_tabArea = this.tabMDIList.GetTabRect(tabMDIList.TabPages.Count - 1);

                //((TAP.UI.UIBase)tmpForm).TabPage = tmpTabPage;
                //this.tabMDIList.SelectedTab = tmpTabPage;

                TAP.Remoting.Caller.CallerInfo.UserID = InfoBase._USER_INFO.UserName;
                TAP.Remoting.Caller.CallerInfo.ClientPort = TAP.Base.Configuration.ConfigurationManager.Instance.RemoteAdapterSection.LocalPort;
                TAP.Remoting.Caller.CallerInfo.MDIName = this._mdiName;
                TAP.Remoting.Caller.CallerInfo.FunctionName = tmpUI.AssemblyName;

                //tmpForm.WindowState = FormWindowState.Maximized;
                //this.Text = _displayName + "::" + this._conveter.ConvertPhrase(InfoBase._MENU_INFO[mainMenu].UIs[menu].DisplayName);

               // ((TAP.UI.UIBase)tmpForm).UIInformation = tmpUI;

                tmpForm.Show();

                if (!object.Equals(arguments, null))
                    ((UIBase)tmpForm).ExecuteCommand(arguments);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void ActivateAppUI(string mainMenu, string menu, ArgumentPack arguments)
        {
            #region Open Child Open UI

            string tmpAssemblyName = string.Empty;
            string tmpAssemblyFileName = string.Empty;
            Form tmpForm = null;

            try
            {
                //ÇØ´ç UI°¡ ÀÌ¹Ì ¿­·ÁÀÖ´ÂÁö Ã¼Å©
                tmpAssemblyFileName = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[mainMenu].Menus[menu].AssemblyFileName;
                tmpAssemblyName = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[mainMenu].Menus[menu].AssemblyName;
                tmpForm = this.CheckOpendForms(tmpAssemblyName);

                if (FindActiveUI(tmpAssemblyName, arguments))
                    return;

                Assembly a = Assembly.LoadFile(Path.Combine(_appDirectory, tmpAssemblyFileName));

                if (object.Equals(a, null))
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, string.Format("Assembly file '{0}'>", tmpAssemblyFileName));
                    throw new Exception(tmpMsg);
                }

                Type tmpType = a.GetType(tmpAssemblyName);

                if (object.Equals(tmpType, null))
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, string.Format("Assembly '{0}'>", tmpAssemblyName));
                    throw new Exception(tmpMsg);
                }

                tmpForm = (Form)Activator.CreateInstance(tmpType);
                tmpForm.MdiParent = this;
                tmpForm.FormBorderStyle = FormBorderStyle.None;
                tmpForm.Dock = DockStyle.Fill;
                tmpForm.Text = this.Text = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[mainMenu].Menus[menu].DisplayName;
                //tmpForm.WindowState = FormWindowState.Maximized;
                this.Text = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[mainMenu].Menus[menu].DisplayName + "::" + _displayName;

                //((TAP.UI.UIBase)tmpForm).UIInformation = tmpUI;

                tmpForm.Show();

                if (!object.Equals(arguments, null))
                    ((UIBase)tmpForm).ExecuteCommand(arguments);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private bool FindActiveUI(UIBasicModel ui, ArgumentPack arguments)
        {
            #region Find Active UI

            bool retVal = false;

            try
            {
                foreach (Form tmpForm in this.MdiChildren)
                {
                    if (((UIBase)tmpForm).UIInformation.Equals(ui))
                    {
                        tmpForm.Activate();

                        if (!object.Equals(arguments, null))
                            ((TAP.UI.UIBase) tmpForm).ExecuteCommand(arguments);

                        retVal = true;
                        break;
                    }
                }

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }


            #endregion
        }

        private bool FindActiveUI(string assembly, ArgumentPack arguments)
        {
            #region Find Active UI

            bool retVal = false;

            try
            {
                foreach (Form tmpForm in this.MdiChildren)
                {
                    if (tmpForm.GetType().ToString().Equals( assembly))
                    {
                        tmpForm.Activate();

                        if (!object.Equals(arguments, null))
                            ((TAP.UI.UIBase)tmpForm).ExecuteCommand(arguments);

                        retVal = true;
                        break;
                    }
                }

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void UICallBase_OpenChildUI(string mainMenu, string menu, ArgumentPack arguments)
        {
            #region UICallBase_OpenChildUI

            try
            {
#if SYS
                this.ActivateUI(mainMenu, menu, arguments);
#endif
#if APP
                this.ActivateAppUI(mainMenu, menu, arguments);
#endif
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private string MakeUITitle(string mainMenu, string menu)
        {
            #region Code

            string retVal = string.Empty;

            try
            {
                //retVal += "[" + TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region + " / " + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Facility + "]";
                //retVal += " ";
                //retVal += "<" + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.ProductName + "> ¢º ";
                retVal += "<" + this._conveter.ConvertPhrase(mainMenu) + "> ¢º ";
                retVal += "<" + this._conveter.ConvertPhrase(menu) + ">";

                retVal += "<" + this._conveter.ConvertPhrase(menu) + ">";

                //retVal += "[" + TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region + " / " + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Facility + "]";
                //retVal += " ";
                //retVal += "<" + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[this._appName].SubApps[this._mdiName].DisplayName + "> ¢º ";
                //retVal += "<" + this._conveter.ConvertPhrase(mainMenu) + "> ¢º ";
                //retVal += "<" + this._conveter.ConvertPhrase(menu) + ">";

                return retVal;
            }
            catch(System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private string MakeUITitle(UIBasicModel uiModel)
        {
            #region Code

            string retVal = string.Empty;

            try
            {
                //retVal += "[" + TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region + " / " + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Facility + "]";
                //retVal += " ";
                //retVal += "<" + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.ProductName + "> ¢º ";
                retVal += "<" + this._conveter.ConvertPhrase(uiModel.MainMenu) + "> ¢º ";
                retVal += "<" + this._conveter.ConvertPhrase(uiModel.Name) + "> _ ";
                retVal += "<" + uiModel.Description + ">";

                //retVal += "[" + TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region + " / " + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Facility + "]";
                //retVal += " ";
                //retVal += "<" + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[this._appName].SubApps[this._mdiName].DisplayName + "> ¢º ";
                //retVal += "<" + this._conveter.ConvertPhrase(mainMenu) + "> ¢º ";
                //retVal += "<" + this._conveter.ConvertPhrase(menu) + ">";

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Temp


        private void LoadTheme()
        {
            #region Code

            try
            {
                //string content = System.IO.File.ReadAllText(@"D:\SRC\_bin\FX\theme\ribbonTheme_SilverBlack.xml");
                string content = System.IO.File.ReadAllText(System.IO.Path.Combine(_ExecutableDirectory, "theme\\ribbonTheme_SilverBlack.xml"));
                Theme.ColorTable.ReadThemeXmlFile(content);
                
                this.Refresh();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }
       

        #endregion
        
        #region Close

        private bool CloseApp()
        {
            #region Clsoe App

            string tmpMsg = string.Empty;

            try
            {
                tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.CLOSE, EnumGeneralTemplateType.CONFIRM, _displayName);

                if (TAPMsgBox.Instance.ShowMessage("Confirm", EnumMsgType.CONFIRM, tmpMsg).Equals(DialogResult.Yes))
                {
                    for (int i = 0; i < this.MdiChildren.Length; i++)
                    {
                        try
                        {
                            if (this.MdiChildren[i].GetType().BaseType.BaseType == typeof(UIBase))
                                ((UIBase)this.MdiChildren[i]).Exit();
                            else
                                ((Form)this.MdiChildren[i]).Close();
                        }
                        catch (System.Exception ex)
                        {
                            tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.CLOSE, EnumGeneralTemplateType.FAIL, _displayName);
                            TAPMsgBox.Instance.ShowMessage("Error", EnumMsgType.ERROR, tmpMsg, ex.ToString());
                            continue;
                        }
                    }

                    if(this.WindowState == FormWindowState.Maximized)
                    {
                        TAP.App.Base.AppConfig.ConfigManager.HostCollection["MDIInit"]["Status"] = "FULL";
                        TAP.App.Base.AppConfig.ConfigManager.ChangeConfigSetup();
                    }
                    else
                    {
                        TAP.App.Base.AppConfig.ConfigManager.HostCollection["MDIInit"]["Status"] = "NORMAL";
                        TAP.App.Base.AppConfig.ConfigManager.HostCollection["MDIInit"]["Width"] = this.Width.ToString();
                        TAP.App.Base.AppConfig.ConfigManager.HostCollection["MDIInit"]["Height"] = this.Height.ToString();
                        TAP.App.Base.AppConfig.ConfigManager.ChangeConfigSetup();

                    }

                    this.UserLogOn(false);
                    return true;
                }
                else
                    return false;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Logout

        private bool LogoutApp()
        {
            #region Logout App

            string tmpMsg = string.Empty;

            try
            {
                tmpMsg = _translator.ConvertGeneralTemplate("LOGOUT", EnumGeneralTemplateType.CONFIRM, _displayName);
                
                if (TAPMsgBox.Instance.ShowMessage("Confirm", EnumMsgType.CONFIRM, tmpMsg).Equals(DialogResult.Yes))
                {
                    this.UserLogOn(false);
                    return true;
                }
                else
                    return false;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Image and Icon

        private void SetImageList()
        {
            #region Set Image List

            string tmpImagePath = string.Empty;

            try
            {
                if (this.imageListMainMenu == null)
                    this.imageListMainMenu = new ImageList();
                else
                    this.imageListMainMenu.Images.Clear();

                tmpImagePath = Path.Combine(this._appDirectory, "IMAGES");

                string[] tmpImages = Directory.GetFiles(tmpImagePath, "*.png");

                for (int i = 0; i < tmpImages.Length; i++)
                {
                    string tmpImageName = (i + 1).ToString().PadLeft(3, '0') + ".png";

                    Image tmpImage = null;

                    if (File.Exists(Path.Combine(tmpImagePath, tmpImageName)))
                    {
                        tmpImage = Image.FromFile(Path.Combine(tmpImagePath, tmpImageName));
                    }
                    else
                    {
                        tmpImageName = "noimage.png";
                        tmpImage = Image.FromFile(Path.Combine(tmpImagePath, tmpImageName));
                    }

                    this.imageListMainMenu.Images.Add(tmpImage);

                }

            }
            catch(System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Event Handler

        /// <summary>
        /// Main Menu click events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MainMenu_Click(object sender, EventArgs e)
        {
            #region Main Menu Click

            this.Cursor = Cursors.WaitCursor;

            try
            {
                this._mainMenu = ((ToolStripMenuItem)sender).Name;
            }
            catch (System.Exception ex)
            {
                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.SET, EnumGeneralTemplateType.FAIL, "Main menus");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMessage, ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            #endregion
        }

        /// <summary>
        /// Menu click events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Menu_Click(object sender, EventArgs e)
        {
            #region Menu_Click

            string tmpMainMenu = string.Empty;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //tmpMainMenu = this.ribbonMenu.ActiveTab.Tag.ToString();
                RibbonButton tmpMenu = (RibbonButton)sender;

#if SYS
       
                //this.OpenSYSUI(tmpMenu.)
                this.OpenSYSUI(tmpMainMenu, tmpMenu.Tag.ToString());
#endif
#if APP
                this.OpenAPPUI(this._mainMenu, tmpMenu.Name);
#endif
            }
            catch (System.Exception ex)
            {
                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.OPEN, EnumGeneralTemplateType.FAIL, "menu");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMessage, ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            #endregion
        }
        /// <summary>
        /// Menu click events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MenuBarButtion_Click(object sender, ItemClickEventArgs e)
        {
            #region Menu_Click

            string tmpMainMenu = string.Empty;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                tmpMainMenu = e.Item.Name.ToString();

#if SYS

                //this.OpenSYSUI(tmpMenu.)
                this.OpenSYSUI(tmpMainMenu, e.Item.Tag.ToString());
#endif
#if APP
                this.OpenAPPUI(this._mainMenu, tmpMenu.Name);
#endif
               
            }
            catch (System.Exception ex)
            {
                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.OPEN, EnumGeneralTemplateType.FAIL, "menu");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMessage, ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            #endregion
        }

        /// <summary>
        /// Logout, Close click events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopupMenuBarButtion_Click(object sender, ItemClickEventArgs e)
        {
            #region PopupMenu_Click Close,Logout

            string tmpPopupMenu = string.Empty;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                tmpPopupMenu = e.Item.Tag.ToString();

                if(tmpPopupMenu == "Close")
                {                    
                    this.Close();
                }
                else if (tmpPopupMenu == "Logout")
                {
                    if (LogoutApp())
                        Application.Restart();
                    else
                        return;
                }
            }
            catch(System.Exception ex)
            {
                throw ex;
            }     
            finally
            {
                this.Cursor = Cursors.Default;
            }

            #endregion
        }

        private void barSubItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            BarSubItemLink link = e.Item.Links[0] as BarSubItemLink;

            link.OpenMenu();

        }

        private void btnBookMark_ItemDoubleClick(object sender, ItemClickEventArgs e)
        {
            #region code
            try
            {
                for (int i = 0; i < popupMenuBookMark.ItemLinks.Count; i++)
                {
                    if (((BarButtonItem)popupMenuBookMark.ItemLinks[i].Item).ButtonStyle == BarButtonStyle.Default)
                        ((BarButtonItem)popupMenuBookMark.ItemLinks[i].Item).ButtonStyle = BarButtonStyle.DropDown;
                    else
                        ((BarButtonItem)popupMenuBookMark.ItemLinks[i].Item).ButtonStyle = BarButtonStyle.Default;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region Form1_Load

            this.Cursor = Cursors.WaitCursor;

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

        protected void SelectOpendMenu(object sender, EventArgs e)
        {
            //string KK = "KK";
        }
        
        private void btnBookMarkDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            #region code

            PopupMenu tmPopupMenu;
            BarButtonItemLink tmpObj;
            BarButtonItem tmpbtn;
            string tmpDes;

            try
            {
                tmPopupMenu = (PopupMenu)e.Link.LinkedObject;
                tmpObj = (BarButtonItemLink)tmPopupMenu.Activator;
                tmpbtn = tmpObj.Item;
                tmpDes = tmpbtn.Description;

                this.BookAssginOpendForms(tmpbtn.Name, tmpbtn.Tag.ToString(), tmpDes);

                popupMenu1.HidePopup();


            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                popupMenuBookMark.HidePopup();
            }
            #endregion
        }

        private void popupMenuBookMark_CloseUp(object sender, EventArgs e)
        {
            #region Code

            if (_bookmark)
                this.CreateBookMarkMenu();

            _bookmark = false;

            #endregion
        }
        
        private void btnUIAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            #region Code

            try
            {
                if (xtraTabbedMdiManager1.Pages.Count == 0)
                {
                    string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.FIND, EnumGeneralTemplateType.CANNOT, "CURRENT UI");
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMessage);
                    return;
                }
                if (xtraTabbedMdiManager1.SelectedPage != null)
                {
                    BookMarkModify(true, ((TAP.UI.UIBase)xtraTabbedMdiManager1.SelectedPage.MdiChild).UIInformation);
                }
            }
            catch(System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.ApplicationExitCall)
                {

                }
                else
                {
                    if (CloseApp())
                        e.Cancel = false;
                    else
                        e.Cancel = true;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPassword_ItemClick(object sender, ItemClickEventArgs e)
        {
            FormPassword tmpPassword = new FormPassword();

            tmpPassword.SetUserDefaultInfo(this._userDefaultInfo);

            if (tmpPassword.ShowDialog().Equals(DialogResult.OK))
            {
                InfoBase._USER_INFO.ChangeOneModelLastEvent(
                        "Change Password", EnumEventFlag.N, DateTime.Now, this._mdiName);
                InfoBase._USER_INFO.Save(this._userDefaultInfo.UserID);

                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.CHANGE, EnumGeneralTemplateType.COMPLETE, "< PassWord >");
                TAPMsgBox.Instance.ShowMessage("Result", EnumMsgType.INFORMATION, tmpMessage);                
            }
        }

        #endregion

        #region Utilities

        private bool CheckUserAuthority(UIBasicModel ui)
        {
            #region Check User Authority
            
            bool tmpFind = false;
            UIBasicDefaultInfo tmpUIBasic = null;
            UIAuthorityBasicModelSet tmpUIAuthority = null;
            
            try
            {
                tmpUIBasic = new UIBasicDefaultInfo();

                tmpUIBasic.MDI = ui.MDI;
                tmpUIBasic.MainMenu = ui.MainMenu;
                tmpUIBasic.UI = ui.Name;
                tmpUIBasic.Region = ui.Region;
                tmpUIBasic.Facility = ui.Facility;
                
                tmpUIAuthority = new UIAuthorityBasicModelSet();
                tmpUIAuthority.LoadModels(tmpUIBasic);

                                
                if (tmpUIAuthority.Count == 0)
                    return false;

                foreach (UIAuthorityBasicModel tmpAuthority in tmpUIAuthority.Models)
                {
                    if(tmpAuthority.MemberType.ToString() == "USERGROUP" && InfoBase._USER_GROUP_MEMBER != null)
                    {
                        if (tmpAuthority.Name == InfoBase._USER_GROUP_MEMBER.UserGroup)
                            tmpFind = true;
                    }
                    else
                    {
                        if (tmpAuthority.Name == InfoBase._USER_INFO.Name)
                            tmpFind = true;
                    }
                }
                    return tmpFind;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void xtraTabbedMdiManager1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                BaseTabHitInfo hint = xtraTabbedMdiManager1.CalcHitInfo(e.Location);

                //µã»÷ÓÐÐ§,ÇÒµã»÷ÔÚTabPage±êÌâÉÏ
                if (hint.IsValid && (hint.Page != null))
                {
                    //ÓÐÐ§×Ó´°Ìå
                    if (xtraTabbedMdiManager1.SelectedPage.MdiChild != null)
                    {
                        tContextMenu1.ShowPopup(Control.MousePosition);//ÏÔÊ¾µ¯³ö²Ëµ¥
                    }
                }
            }
        }

        private void barBtnCloseCurrentUI_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.ActiveMdiChild.Close();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void barBtnCloseAllUI_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (xtraTabbedMdiManager1.Pages.Count > 0)
                {
                    foreach (Form form in this.MdiChildren)
                    {
                        form.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void xtraTabbedMdiManager1_PageRemoved(object sender, DevExpress.XtraTabbedMdi.MdiTabPageEventArgs e)
        {
            try
            {
                string str = "test";
                
                //((DevExpress.XtraTabbedMdi.XtraTabbedMdiManager)sender);
                //sender
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void barBtnLanguage_ItemClick(object sender, ItemClickEventArgs e)
        {
            FormLanguage tmpLanguage = new FormLanguage();

            tmpLanguage.SetUserDefaultInfo(this._userDefaultInfo);

            if (tmpLanguage.ShowDialog().Equals(DialogResult.OK))
            {
                InfoBase._USER_INFO.ChangeOneModelLastEvent(
                        "Change Language", EnumEventFlag.N, DateTime.Now, this._mdiName);
                InfoBase._USER_INFO.Save(this._userDefaultInfo.UserID);

                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.CHANGE, EnumGeneralTemplateType.COMPLETE, "< Language >");
                TAPMsgBox.Instance.ShowMessage("Result", EnumMsgType.INFORMATION, tmpMessage);
            }
        }

        private DataTable LoadSubMenu(string mainMenu)
        {
            DataTable retVal;
            string tmpRegion = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
            string tmpFacility = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Facility;

            string tmpMainMenuSql = "SELECT * FROM TAPSTBSUBMENU WHERE MDI = '{0}' AND REGION = '{1}' AND FACILITY = '{2}'  AND MAINMENU = '{3}' AND ISALIVE = 'YES'  ORDER BY SEQUENCES";

            tmpMainMenuSql = string.Format(tmpMainMenuSql, _mdiName, tmpRegion,tmpFacility, mainMenu);

            DataClient tmpDataClient = new DataClient();
            retVal = tmpDataClient.SelectData(tmpMainMenuSql, "SUBMENU").Tables[0];

            return retVal;
        }



        #endregion





        //private void tabMDIList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    #region Code

        //    try
        //    {
        //        foreach(UIBase tmpUI in this.MdiChildren)
        //        {
        //            if (tmpUI.TabPage.Equals(this.tabMDIList.SelectedTab))
        //            {
        //                TAP.Remoting.Caller.CallerInfo.UserID = InfoBase._USER_INFO.UserName;
        //                TAP.Remoting.Caller.CallerInfo.ClientPort = TAP.Base.Configuration.ConfigurationManager.Instance.RemoteAdapterSection.LocalPort;
        //                TAP.Remoting.Caller.CallerInfo.MDIName = this._mdiName;
        //                TAP.Remoting.Caller.CallerInfo.FunctionName = tmpUI.GetType().ToString();

        //                tmpUI.Select();
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }

        //    #endregion
        //}
    }
}
