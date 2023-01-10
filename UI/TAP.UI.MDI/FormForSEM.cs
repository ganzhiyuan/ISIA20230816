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
    /// <summary>
    /// This is main MDI
    /// </summary>
    public partial class FormForSEM : Form
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

        #endregion

        /// <summary>
        /// This creates instance of Form1
        /// </summary>
        public FormForSEM()
        {
            InitializeComponent();
        }

        #region Initialize

        private void Initialize()
        {
            #region Initialize

            try
            {
                this._uiLog = new UILog();
                FormLogin tmpLogIn = new FormLogin();

                if (tmpLogIn.ShowDialog().Equals(DialogResult.OK))
                {
                    this._userDefaultInfo = tmpLogIn._UserDefaultInfo;

                    _appName = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MDIName;
                    _mdiName = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MDIName;
                    _displayName = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MDIDisplayName;
                    this.Text = _displayName;

                    using (var bl = new TAP.UI.BackgroundProcessor(this.DoInitialize, this, 8L, true))
                    {
                        bl.Start();
                    }

                    if (!_isSuccessInit)
                    {
                        this.Dispose();
                        this.Close();
                    }

                    //this._imgDir = Path.Combine(_ExecutableDirectory, "images");
                    //this.InitializeAppLink();
                }
                else
                {
                    tmpLogIn.Close();
                    tmpLogIn.Dispose();
                    this.Close();
                }

                //_displayName = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[_appName].SubApps[_mdiName].DisplayName;

                this.SetIcon();

                UICallBase.OpenChildUI += new UICallBase.OpenChildUIEventHandler(UICallBase_OpenChildUI);

                this.tabMDIList.DrawMode = TabDrawMode.OwnerDrawFixed;
                this.tabMDIList.DrawItem += new DrawItemEventHandler(tabMDIList_DrawItem);

                //Set User Info
                this.SetUserInfo(this.txtUserName, InfoBase._USER_INFO.UserName);
                this.SetUserInfo(this.txtUserPosition, InfoBase._USER_INFO.Position);
                this.SetUserInfo(this.txtDate, DateTime.Now.ToString("yyyy-MM-dd"));
                this.SetUserInfo(this.txtTime, DateTime.Now.ToString("HH:mm:ss"));

                this._uiLog = new UILog();

#if SYS
                //using (var bl = new TAP.UI.BackgroundProcessor(this.DoInitialize, this, 7L, true))
                //{
                //    bl.Start();
                //}

                //if (!_isSuccessInit)
                //{
                //    this.Dispose();
                //    this.Close();
                //}

                this.UserLogOn(true);
                this.tabMDIList.Visible = false;

                _openedUIs = new List<UIBasicModel>();

                this.LoadTheme();

                this.WindowState = FormWindowState.Maximized;

                this.label1.Text = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MDIDisplayName;
#endif
#if APP
                //사용자 정보가 필요한 경우가 있기 때문에 임의의 사용자 정보를 만든다. 

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

                #region Activate Startup UI

                if(TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.StartupUI.Enabled == true)
                {
                    string tmpStartupMainMenu = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.StartupUI.StartUpMainMenu.ToUpper();
                    string tmpStartupMenu = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.StartupUI.StartUpMenu.ToUpper();

                    if (tmpStartupMainMenu.Length > 0 && tmpStartupMenu.Length > 0)
                    {
                        this.OpenSYSUI(tmpStartupMainMenu, tmpStartupMenu);
                    }
                }

                #endregion

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void SetUserInfo(TextBox tbx, string txt)
        {
            #region Code

            try
            {
                if(tbx.InvokeRequired)
                {
                    DoSetUserInfo tmp = new DoSetUserInfo(SetUserInfo);
                    this.Invoke(tmp, tbx, txt);
                }
                else
                {
                    tbx.Text = txt;
                }

                return;
            }
            catch(System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        void tabMDIList_DrawItem(object sender, DrawItemEventArgs e)
        {
            #region Code

            string tmpText;

            Font tmpNormalFont = null;
            Font tmpSelectedFont = null;

            SolidBrush tmpNormalBrush = null;
            SolidBrush tmpSelectedBrush = null;
            SolidBrush tmpForeBrush = null;

            try
            {
                Graphics g = e.Graphics;

                tmpNormalFont = new Font("Tahoma", 8.25f);
                tmpSelectedFont = new Font("Tahoma", 8.25f);

                tmpNormalBrush = new SolidBrush(Color.FromArgb(255, 255, 255));
                tmpSelectedBrush = new SolidBrush(Color.RoyalBlue);
                tmpForeBrush = new SolidBrush(Color.Black);
                
                tmpText = this.tabMDIList.TabPages[e.Index].Text;

                StringFormat tmpFormat = new StringFormat();
                tmpFormat.Alignment = StringAlignment.Center;

               
                if(this.tabMDIList.TabPages[e.Index] == this.tabMDIList.SelectedTab)
                {
                    g.FillRectangle(tmpSelectedBrush, e.Bounds);
                    
                    Rectangle r = e.Bounds;
                    r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
                    g.DrawString(tmpText, tmpSelectedFont, tmpForeBrush, r, tmpFormat);
                }
                else
                {
                    g.FillRectangle(tmpNormalBrush, e.Bounds);

                    Rectangle r = e.Bounds;
                    r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
                    g.DrawString(tmpText, tmpNormalFont, tmpForeBrush, r, tmpFormat);

                }

                return;
            }
            catch(System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                tmpNormalFont.Dispose();
                tmpSelectedFont.Dispose();

                tmpNormalBrush.Dispose();
                tmpSelectedBrush.Dispose();
                tmpForeBrush.Dispose();
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
                this.SetMainButtons();
                _uiLog.WriteLog("MDI", "T", TapBase.Instance.MachineName, TapBase.Instance.IPAddress, "Initializing User Authority....OK");
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

        private void LoadMenu()
        {
            #region Private void UIs

            MDIBasicModel tmpMDIInfo = null;
            //MainMenu tmpMainMenu = null;
            //UIBasicDefaultInfo tmpUIDefaultInfo = null;

            try
            {
                //tmpUIDefaultInfo = new UIBasicDefaultInfo();
                //tmpUIDefaultInfo.Region = InfoBase._USER_INFO.Region;
                //tmpUIDefaultInfo.Facility = InfoBase._USER_INFO.Facility;

                //tmpMDIInfo = InfoBase._MDI_INFO[TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Apps[_appName].SubApps[_mdiName].Key.ToUpper()];
                tmpMDIInfo = InfoBase._MDI_INFO[TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MDIName];

                if (tmpMDIInfo.MainMenus == null || tmpMDIInfo.MainMenus.Count ==0)
                    tmpMDIInfo.LoadNestModel();

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

        private void LoadUserGroupInformation()
        {
            #region Load User Group Information

            UserDefaultInfo tmpUserDefaultInfo = null;
            UserGroupModelSet tmp = null;
            List<UserGroupModel> retVal = null;

            try
            {
                if (InfoBase._USER_INFO == null)
                    return;

                tmpUserDefaultInfo = new UserDefaultInfo();
                tmpUserDefaultInfo.Region = InfoBase._USER_INFO.Region;
                tmpUserDefaultInfo.Facility = InfoBase._USER_INFO.Facility;

                tmp = new UserGroupModelSet();
                tmp.LoadModels(tmpUserDefaultInfo);

                retVal = new List<UserGroupModel>();

                foreach( string tmpGrpName in tmp.Names)
                {
                    UserGroupModel tmpGroup = tmp[tmpGrpName];

                    if (tmpGroup.Members == null || tmpGroup.Members.Count == 0)
                        tmpGroup.LoadNestModel();

                    if (tmpGroup.Members.Contains(InfoBase._USER_INFO.Name))
                        retVal.Add(tmpGroup);
                }

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

            try
            {
#if SYS
                #region Using Config

                //for (int i = 0; i < TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus.Count; i++)
                //{
                //    string tmpDisplayName = this._conveter.ConvertPhrase(TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[i].DisplayName);
                //    ToolStripMenuItem tmpMainMenu = new ToolStripMenuItem(tmpDisplayName);
                //    tmpMainMenu.Name = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[i].Key;
                //    tmpMainMenu.ShowShortcutKeys = true;
                //    tmpMainMenu.Click += new EventHandler(MainMenu_Click);

                //    this.CreateMenu(tmpMainMenu.Name, ref tmpMainMenu);
                //    this.menuStrip1.Items.Add(tmpMainMenu);
                //}

                #endregion

                tmpMainMenuNames = InfoBase._MDI_INFO[this._mdiName].MainMenus.CreateSeqenceArray();

                //tmpMainMenuNames = InfoBase._MENU_INFO.CreateSeqenceArray();

                for (int i = 0; i < tmpMainMenuNames.Length; i++)
                {
                    MainMenuBasicModel tmpMainMenuModel
                        = InfoBase._MDI_INFO[this._mdiName].MainMenus[tmpMainMenuNames[i]];

                    //MainMenuBasicModel tmpMainMenuModel = InfoBase._MENU_INFO[tmpMainMenuNames[i]];

                    if (tmpMainMenuModel.Name.Equals(_INDEPENDENT_MENU))
                        continue;

                    if(tmpMainMenuModel.Name == "ADMIN")
                    {
                        if (InfoBase._USER_INFO.UserGroupName != "ADMIN")
                            continue;
                    }

                    #region Ribbon Menu

                    //RibbonTab tmpNewTab = new RibbonTab(this._conveter.ConvertPhrase(tmpMainMenuModel.DisplayName));
                    //tmpNewTab.Tag = tmpMainMenuModel.Name;
                    //tmpNewTab.Panels.Add(this.CreateMenu(tmpMainMenuModel));

                    //this.ribbonMenu.Tabs.Add(tmpNewTab);

                    #endregion

                    ToolStripMenuItem tmpMainMenu = new ToolStripMenuItem(this._conveter.ConvertPhrase(tmpMainMenuModel.DisplayName));
                    tmpMainMenu.Name = tmpMainMenuModel.Name;
                    tmpMainMenu.ShortcutKeyDisplayString = tmpMainMenuModel.ShortCutDisplayString;
                    tmpMainMenu.ShowShortcutKeys = true;
                    //tmpMainMenu.Click += new EventHandler(MainMenu_Click);
                    tmpMainMenu.DropDownOpening += new EventHandler(MainMenu_Click);

                    this.CreateMenu(tmpMainMenuModel, ref tmpMainMenu);
                    this.menuStrip1.Items.Add(tmpMainMenu);
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
                        //UI에 대한 권한 설정이 있는 경우만....

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
                        else
                        {
                            //ToolStripMenuItem tmpMenu = new ToolStripMenuItem("-");
                            //mainMenuItem.DropDownItems.Add("-");
                        }
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

        private void CreateMenu(MainMenuBasicModel mainMenuModel, ref ToolStripMenuItem mainMenuItem)
        {
            #region Code

            string[] tmpMenuNames = null;

            try
            {
                if (mainMenuModel.UIs == null || mainMenuModel.UIs.Count.Equals(0))
                    mainMenuModel.LoadNestModel();

                tmpMenuNames = mainMenuModel.UIs.CreateSeqenceArray();

                for (int i = 0; i < tmpMenuNames.Length; i++)
                {
                    UIBasicModel tmpUIModel = mainMenuModel.UIs[tmpMenuNames[i]];

                    if (this.CheckUserAuthority(tmpUIModel))
                    {
                        //UI에 대한 권한 설정이 있는 경우만....

                        if (tmpUIModel.Name.IndexOf("SPREATOR") < 0)
                        {
                            #region OLD

                            ToolStripMenuItem tmpMenu = null;

                            if (!object.Equals(this.imageListMainMenu.Images, null) &&
                                this.imageListMainMenu.Images.Count > 0 &&
                                this.imageListMainMenu.Images.Count > tmpUIModel.Icon)
                            {
                                tmpMenu
                                = new ToolStripMenuItem(this._conveter.ConvertPhrase(tmpUIModel.DisplayName), this.imageListMainMenu.Images[tmpUIModel.Icon]);
                            }
                            else
                            {
                                tmpMenu
                                = new ToolStripMenuItem(this._conveter.ConvertPhrase(tmpUIModel.DisplayName));
                            }

                            tmpMenu.Name = tmpUIModel.Name;

                            //tmpMenu.ShortcutKeys = tmpUIModel.ShortCutKeys;
                            tmpMenu.ShortcutKeyDisplayString = tmpUIModel.ShortCutDisplayString;
                            tmpMenu.ShowShortcutKeys = true;


                            tmpMenu.Click += new EventHandler(Menu_Click);

                            mainMenuItem.DropDownItems.Add(tmpMenu);

                            #endregion
                        }
                        else
                        {
                            ToolStripMenuItem tmpMenu = new ToolStripMenuItem("-");
                            mainMenuItem.DropDownItems.Add("-");
                        }
                    }
                }

                return;
            }
            catch(System.Exception ex)
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

                //해당 UI가 이미 열려있는지 체크
                tmpForm = this.CheckOpendForms(tmpUI);

                if (object.Equals(tmpForm, null))
                {
                    //해당 UI가 열려있지 않으면 로드

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

                    //생성
                    tmpObject = Activator.CreateInstance(tmpType);

                    //실행
                    tmpNewForm = (Form)tmpObject;
                    tmpNewForm.MdiParent = this;
                    tmpNewForm.FormBorderStyle = FormBorderStyle.None;
                    tmpNewForm.Dock = DockStyle.Fill;
                    //tmpNewForm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
#if MODELER
                    this.Text = "TAP Modeler::" + this._conveter.ConvertPhrase(InfoBase._MENU_INFO[mainMenu].UIs[menu].DisplayName);
#endif
#if WORKFLOW
                    this.Text = "TAP Workflower::" + this._conveter.ConvertPhrase(InfoBase._MENU_INFO[mainMenu].UIs[menu].DisplayName);
#endif

                    ((TAP.UI.UIBase)tmpNewForm).UIInformation = tmpUI;
                    ((TAP.UI.UIBase)tmpNewForm).TabControl = this.tabMDIList;
                    ((TAP.UI.UIBase)tmpNewForm).UITitle = this.MakeUITitle(InfoBase._MDI_INFO[this._mdiName].MainMenus[mainMenu].DisplayName, tmpUI.DisplayName);

                    TabPage tmpTabPage = new TabPage();
                    tmpTabPage.Parent = this.tabMDIList;
                    tmpTabPage.Text = this._conveter.ConvertPhrase(tmpUI.DisplayName);
                    tmpTabPage.ImageIndex = tmpUI.Icon;
                    tmpTabPage.Show();

                    _tabArea = this.tabMDIList.GetTabRect(tabMDIList.TabPages.Count - 1);

                    ((TAP.UI.UIBase)tmpNewForm).TabPage = tmpTabPage;
                    this.tabMDIList.SelectedTab = tmpTabPage;

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

                //해당 UI가 이미 열려있는지 체크
                tmpPack = new ArgumentPack();
                tmpAssemblyFileName = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[mainMenu].Menus[menu].AssemblyFileName;
                tmpAssemblyName = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[mainMenu].Menus[menu].AssemblyName;
                tmpPack.AddArgument("A_ARGUMENT", typeof(string), TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[mainMenu].Menus[menu].Argument);
                tmpForm = this.CheckOpendForms(tmpAssemblyName);

                tmpArguments = TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[mainMenu].Menus[menu].Argument;

                string tmpDisplayName = this._conveter.ConvertPhrase(TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MainMenus[mainMenu].Menus[menu].DisplayName);

                if (object.Equals(tmpForm, null))
                {
                    //해당 UI가 열려있지 않으면 로드

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
                       
                    //생성
                    tmpObject = Activator.CreateInstance(tmpType);

                    //실행
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

                ((TAP.UI.UIBase)tmpForm).UIInformation = tmpUI;
                ((TAP.UI.UIBase)tmpForm).TabControl = this.tabMDIList;
                ((TAP.UI.UIBase)tmpForm).UITitle = this.MakeUITitle(InfoBase._MDI_INFO[this._mdiName].MainMenus[mainMenu].DisplayName, tmpUI.DisplayName);

                TabPage tmpTabPage = new TabPage();
                tmpTabPage.Parent = this.tabMDIList;
                tmpTabPage.Text = this._conveter.ConvertPhrase(tmpUI.DisplayName);
                tmpTabPage.ImageIndex = tmpUI.Icon;
                tmpTabPage.Show();

                _tabArea = this.tabMDIList.GetTabRect(tabMDIList.TabPages.Count - 1);

                ((TAP.UI.UIBase)tmpForm).TabPage = tmpTabPage;
                this.tabMDIList.SelectedTab = tmpTabPage;

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
                //해당 UI가 이미 열려있는지 체크
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
                retVal += "[" + TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region + " / " + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.Facility + "]";
                retVal += " ";
                retVal += "<" + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.ProductName + "> ▶ ";
                retVal += "<" + this._conveter.ConvertPhrase(mainMenu) + "> ▶ ";
                retVal += "<" + this._conveter.ConvertPhrase(menu) + ">";

                return retVal;
            }
            catch(System.Exception ex)
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
                //Theme.ColorTable.ReadThemeXmlFile(content);
                
                //this.ribbonMenu.Refresh();
                //this.Refresh();
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
                //this.OpenSYSUI()
                //this.OpenAPPUI(this._mainMenu, tmpMenu.Name);

                //                tmpMainMenu = this.ribbonMenu.ActiveTab.Tag.ToString();
                //                RibbonButton tmpMenu = (RibbonButton)sender;

#if SYS
                this.OpenSYSUI(this._mainMenu, ((ToolStripMenuItem)sender).Name);
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

        #endregion

        #region Utilities

        private bool CheckUserAuthority(UIBasicModel ui)
        {
            #region Check User Authority
            
            //bool tmpFind = true;

            try
            {
                return true;
                
                //if(ui.au)

                //foreach (AuthorityModel tmpAuthority in InfoBase._USER_INFO.Authorities.Models)
                //{
                //    if (tmpAuthority.MDI.Equals(_MDI_NAME) &&
                //        tmpAuthority.MainMenu.Equals(ui.MainMenu) &&
                //        tmpAuthority.MenuName.Equals(ui.Name))
                //    {
                //        tmpFind = true;
                //        break;
                //    }
                //}

                //return tmpFind;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //if (CloseApp())
                //    e.Cancel = false;
                //else
                //    e.Cancel = true;
            }
            catch
            {
            }
        }

        private void tPictureBox1_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch
            {
            }
        }

        private void tabMDIList_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region Code

            try
            {
                foreach(UIBase tmpUI in this.MdiChildren)
                {
                    if (tmpUI.TabPage.Equals(this.tabMDIList.SelectedTab))
                    {
                        TAP.Remoting.Caller.CallerInfo.UserID = InfoBase._USER_INFO.UserName;
                        TAP.Remoting.Caller.CallerInfo.ClientPort = TAP.Base.Configuration.ConfigurationManager.Instance.RemoteAdapterSection.LocalPort;
                        TAP.Remoting.Caller.CallerInfo.MDIName = this._mdiName;
                        TAP.Remoting.Caller.CallerInfo.FunctionName = tmpUI.GetType().ToString();

                        tmpUI.Select();
                    }
                }
            }
            catch
            {

            }

            #endregion
        }

        private void ribbonMenu_Click(object sender, EventArgs e)
        {

        }

        #region Specified

        private delegate void DoSetMainButton(Button btn, string btnKey);

        private void SetMainButtons()
        {
            #region Code

            int tmpTopButtonCount = 7;
            int tmpBottomButtonCount = 10;
            int tmpRightButtonCount = 7;

            try
            {
                //Top Button
                for (int i = 0; i < tmpTopButtonCount; i++)
                {
                    string tmpButtonName = "BTNT" + (i + 1).ToString();

                    switch (tmpButtonName)
                    {
                        case "BTNT1": this.SetMainButton(this.btnT1, tmpButtonName); break;
                        case "BTNT2": this.SetMainButton(this.btnT2, tmpButtonName); break;
                        case "BTNT3": this.SetMainButton(this.btnT3, tmpButtonName); break;
                        case "BTNT4": this.SetMainButton(this.btnT4, tmpButtonName); break;
                        case "BTNT5": this.SetMainButton(this.btnT5, tmpButtonName); break;
                        case "BTNT6": this.SetMainButton(this.btnT6, tmpButtonName); break;
                        case "BTNT7": this.SetMainButton(this.btnT7, tmpButtonName); break;
                    }
                }

                //Bottom Button
                for (int i = 0; i < tmpBottomButtonCount; i++)
                {
                    string tmpButtonName = "BTNB" + (i + 1).ToString();

                    switch (tmpButtonName)
                    {
                        case "BTNB1": this.SetMainButton(this.btnB1, tmpButtonName); break;
                        case "BTNB2": this.SetMainButton(this.btnB2, tmpButtonName); break;
                        case "BTNB3": this.SetMainButton(this.btnB3, tmpButtonName); break;
                        case "BTNB4": this.SetMainButton(this.btnB4, tmpButtonName); break;
                        case "BTNB5": this.SetMainButton(this.btnB5, tmpButtonName); break;
                        case "BTNB6": this.SetMainButton(this.btnB6, tmpButtonName); break;
                        case "BTNB7": this.SetMainButton(this.btnB7, tmpButtonName); break;
                        case "BTNB8": this.SetMainButton(this.btnB8, tmpButtonName); break;
                        case "BTNB9": this.SetMainButton(this.btnB9, tmpButtonName); break;
                        case "BTNB10": this.SetMainButton(this.btnB10, tmpButtonName); break;
                    }
                }

                //Right Button
                for (int i = 0; i < tmpRightButtonCount; i++)
                {
                    string tmpButtonName = "BTNR" + (i + 1).ToString();

                    switch (tmpButtonName)
                    {
                        case "BTNR1": this.SetMainButton(this.btnR1, tmpButtonName); break;
                        case "BTNR2": this.SetMainButton(this.btnR2, tmpButtonName); break;
                        case "BTNR3": this.SetMainButton(this.btnR3, tmpButtonName); break;
                        case "BTNR4": this.SetMainButton(this.btnR4, tmpButtonName); break;
                        case "BTNR5": this.SetMainButton(this.btnR5, tmpButtonName); break;
                        case "BTNR6": this.SetMainButton(this.btnR6, tmpButtonName); break;
                        case "BTNR7": this.SetMainButton(this.btnR7, tmpButtonName); break;

                    }
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void SetMainButton(Button btn, string btnKey)
        {
            #region Code

            try
            {
                if (btn.InvokeRequired)
                {
                    DoSetMainButton tmp = new DoSetMainButton(SetMainButton);
                    this.Invoke(tmp, btn, btnKey);
                }
                else
                {
                    if (TAP.App.Base.AppConfig.ConfigManager.HostCollection[btnKey]["ENABLE"] == "TRUE")
                    {
                        btn.Enabled = true;
                        btn.Visible = true;
                        btn.Text = TAP.App.Base.AppConfig.ConfigManager.HostCollection[btnKey]["TEXT"];

                        if (TAP.App.Base.AppConfig.ConfigManager.HostCollection[btnKey]["IMAGE"].Length > 0)
                        {
                            string tmpImageFilePath = Path.Combine(_ExecutableDirectory, "Images", "MainButtons", TAP.App.Base.AppConfig.ConfigManager.HostCollection[btnKey]["IMAGE"]);

                            if (File.Exists(tmpImageFilePath))
                                btn.Image = Image.FromFile(tmpImageFilePath);
                        }

                        btn.Click += Btn_Click;
                        btn.MouseHover += Btn_MouseHover;
                        btn.MouseLeave += Btn_MouseLeave;
                        btn.Tag = btnKey;
                    }
                    else
                    {
                        btn.Enabled = false;
                        btn.Visible = false;
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

        private void Btn_MouseLeave(object sender, EventArgs e)
        {
            this.ssbDescription.Text = "";
        }

        private void Btn_MouseHover(object sender, EventArgs e)
        {
            #region Code

            string tmpDescription = string.Empty;

            try
            {

                if (TAP.App.Base.AppConfig.ConfigManager.HostCollection[((Button)sender).Tag.ToString()]["ENABLE"] == "TRUE")
                {
                    tmpDescription = TAP.App.Base.AppConfig.ConfigManager.HostCollection[((Button)sender).Tag.ToString()]["DESCRIPTION"];
                    this.ssbDescription.Text = tmpDescription;
                }
                return;
            }
            catch
            {
                //
            }

            #endregion
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            #region Code

            string tmpMainMenu = string.Empty;
            string tmpMenu = string.Empty;

            try
            {
                tmpMainMenu = TAP.App.Base.AppConfig.ConfigManager.HostCollection[((Button)sender).Tag.ToString()]["MAINMENU"];
                tmpMenu = TAP.App.Base.AppConfig.ConfigManager.HostCollection[((Button)sender).Tag.ToString()]["MENU"];

                this.OpenSYSUI(tmpMainMenu, tmpMenu);

                return;
            }
            catch (System.Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(this._displayName, EnumMsgType.WARNING,
                   _translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, tmpMenu + " UI"));
            }

            #endregion
        }

        #endregion

        #region Main Button Event Handers

        private void btn_MouseHover(object sender, EventArgs e)
        {
            this.ssbDescription.Text = ((Button)sender).Tag.ToString();
        }

        #endregion

        private void btnStart_Click(object sender, EventArgs e)
        {
            #region Code

            string tmpDescription = string.Empty;

            try
            {
                TAPMsgBox.Instance.ShowMessage(this._displayName, EnumMsgType.WARNING,
                   _translator.ConvertGeneralTemplate(EnumVerbs.APPLY, EnumGeneralTemplateType.EDNOT, "This function"));

                return;
            }
            catch(System.Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(this._displayName, EnumMsgType.WARNING,
                   _translator.ConvertGeneralTemplate(EnumVerbs.APPLY, EnumGeneralTemplateType.EDNOT, "This function"));
            }

            #endregion
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            #region Code

            string tmpDescription = string.Empty;

            try
            {
                TAPMsgBox.Instance.ShowMessage(this._displayName, EnumMsgType.WARNING,
                   _translator.ConvertGeneralTemplate(EnumVerbs.APPLY, EnumGeneralTemplateType.EDNOT, "This function"));

                return;
            }
            catch (System.Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(this._displayName, EnumMsgType.WARNING,
                   _translator.ConvertGeneralTemplate(EnumVerbs.APPLY, EnumGeneralTemplateType.EDNOT, "This function"));
            }

            #endregion

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            #region Code

            string tmpDescription = string.Empty;

            try
            {
                TAPMsgBox.Instance.ShowMessage(this._displayName, EnumMsgType.WARNING,
                   _translator.ConvertGeneralTemplate(EnumVerbs.APPLY, EnumGeneralTemplateType.EDNOT, "This function"));

                return;
            }
            catch (System.Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(this._displayName, EnumMsgType.WARNING,
                   _translator.ConvertGeneralTemplate(EnumVerbs.APPLY, EnumGeneralTemplateType.EDNOT, "This function"));
            }

            #endregion

        }

        private void btnBatch_Click(object sender, EventArgs e)
        {
            #region Code

            string tmpDescription = string.Empty;

            try
            {
                TAPMsgBox.Instance.ShowMessage(this._displayName, EnumMsgType.WARNING,
                   _translator.ConvertGeneralTemplate(EnumVerbs.APPLY, EnumGeneralTemplateType.EDNOT, "This function"));

                return;
            }
            catch (System.Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(this._displayName, EnumMsgType.WARNING,
                   _translator.ConvertGeneralTemplate(EnumVerbs.APPLY, EnumGeneralTemplateType.EDNOT, "This function"));
            }

            #endregion
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            #region Code

            string tmpDescription = string.Empty;

            try
            {
                TAPMsgBox.Instance.ShowMessage(this._displayName, EnumMsgType.WARNING,
                   _translator.ConvertGeneralTemplate(EnumVerbs.APPLY, EnumGeneralTemplateType.EDNOT, "This function"));

                return;
            }
            catch (System.Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(this._displayName, EnumMsgType.WARNING,
                   _translator.ConvertGeneralTemplate(EnumVerbs.APPLY, EnumGeneralTemplateType.EDNOT, "This function"));
            }

            #endregion
        }

        private void btnRecipe_Click(object sender, EventArgs e)
        {
            #region Code

            string tmpDescription = string.Empty;

            try
            {
                TAPMsgBox.Instance.ShowMessage(this._displayName, EnumMsgType.WARNING,
                   _translator.ConvertGeneralTemplate(EnumVerbs.APPLY, EnumGeneralTemplateType.EDNOT, "This function"));

                return;
            }
            catch (System.Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(this._displayName, EnumMsgType.WARNING,
                   _translator.ConvertGeneralTemplate(EnumVerbs.APPLY, EnumGeneralTemplateType.EDNOT, "This function"));
            }

            #endregion
        }

        private void btnAlarm_Click(object sender, EventArgs e)
        {
            #region Code

            string tmpDescription = string.Empty;

            try
            {
                TAPMsgBox.Instance.ShowMessage(this._displayName, EnumMsgType.WARNING,
                   _translator.ConvertGeneralTemplate(EnumVerbs.APPLY, EnumGeneralTemplateType.EDNOT, "This function"));

                return;
            }
            catch (System.Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(this._displayName, EnumMsgType.WARNING,
                   _translator.ConvertGeneralTemplate(EnumVerbs.APPLY, EnumGeneralTemplateType.EDNOT, "This function"));
            }

            #endregion
        }

        private void btmQuit_Click(object sender, EventArgs e)
        {
            try
            {
                if (CloseApp())
                    this.Close();
            }
            catch
            {
            }
        }

        private void btnMaximze_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
