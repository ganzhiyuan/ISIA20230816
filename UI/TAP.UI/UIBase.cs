using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;

using TAP;
using TAP.Models.UIBasic;
using TAP.Models.Codes;
using TAP.Models.User;
using TAP.Models.Factories;
using TAP.Models.Factories.BasicInfo;
using TAP.Models.Factories.Facilities;
using TAP.Fressage;

using TAP.Remoting;
using TAP.Remoting.Messages;

using TAP.UIControls;
using TAP.UIControls.BasicControls;
using DevExpress.XtraLayout;

namespace TAP.UI
{
    /// <summary>
    /// This class is base of UI TAP-based.
    /// </summary>
    [Designer(typeof(TAP.UI.UIBaseDesigner), typeof(IRootDesigner))]
    public class UIBase : Form
    {
        //Fressage 실행
        //권한설정
        //UI 정보 설정
        //폼간 명령이동

        /// <summary>
        /// This creates instance of UIBase
        /// </summary>
        public UIBase()
        {
            this.InitializeComponent();
        }

        #region Constant

        private const int _default_Panel_height = 25;
        private const int _default_blankSize = 100;
        private const int _default_Control_height = 21;
        private const int _default_Control_Width = 294;
        private const int _default_Blank_width = 3;
        private const int _default_width_Display_Count = 3;

        /// <summary>
        /// Facility combobox name
        /// </summary>
        public const string _COMBOBOX_FACILITY_NAME = "cboFacility";

        /// <summary>
        /// Tech combobox name
        /// </summary>
        public const string _COMBOBOX_TECH_NAME = "cboTech";

        /// <summary>
        /// Lot Code combobox name
        /// </summary>
        public const string _COMBOBOX_LOTCODE_NAME = "cboLotCode";

        /// <summary>
        /// Device combobox name
        /// </summary>
        public const string _COMBOBOX_DEVICE_NAME = "cboDevice";

        /// <summary>
        /// Main operation combobox name
        /// </summary>
        public const string _COMBOBOX_MAINOP_NAME = "cboMainOP";

        /// <summary>
        /// Oeration combobox name
        /// </summary>
        public const string _COMBOBOX_OPEARTION_NAME = "cboOperation";

        /// <summary>
        /// Part combobox name
        /// </summary>
        public const string _COMBOBOX_PART_NAME = "cboPart";

        /// <summary>
        /// Parameter combobox name
        /// </summary>
        public const string _COMBOBOX_PARAMETER_NAME = "cboParameter";

        /// <summary>
        /// Recipe combobox name
        /// </summary>
        public const string _COMBOBOX_RECIPE_NAME = "cboRecipe";

        /// <summary>
        /// Bay combobox name
        /// </summary>
        public const string _COMBOBOX_BAY_NAME = "cboBay";

        /// <summary>
        /// Area combobox name
        /// </summary>
        public const string _COMBOBOX_AREA_NAME = "cboArea";

        /// <summary>
        /// Line combobox name
        /// </summary>
        public const string _COMBOBOX_LINE_NAME = "cboLine";

        /// <summary>
        /// Main Equipment combobox name
        /// </summary>
        public const string _COMBOBOX_MAINEQUIPMENT_NAME = "cboMainEQ";

        /// <summary>
        /// Equipment combobox name
        /// </summary>
        public const string _COMBOBOX_EQUIPMENT_NAME = "cboEquipment";


        #endregion

        #region Fields

        /// <summary>
        /// Converter for cross-language
        /// </summary>
        protected NeutralConverter _converter;

        /// <summary>
        /// Translator of cross-language
        /// </summary>
        protected TemplateConverter _translator;

        /// <summary>
        /// UI model information
        /// </summary>
        protected UIBasicModel _ui;

        /// <summary>
        /// Container for management controls
        /// </summary>
        protected ContainerBasicModelSet _containers;

        /// <summary>
        /// Bottom panel
        /// </summary>
        public TPanel tPanelBottomBase;

        /// <summary>
        /// Label on bottom panel
        /// </summary>
        public TLabel tLabel1;

        /// <summary>
        /// If 'YES', this UI applys UI Automation of TAPFX
        /// </summary>
        protected EnumFlagYN _useUIAutomation = EnumFlagYN.NO;

        private UILog _uiLog;

        private TPanel tPanel1;

        #region Bool Async Call

        private bool _isBusy;
        private bool _isBackGroundCall;
        private bool _continue;
        private bool _showProgress;
        //private bool _asyncEnd;

        private object _lockObject = new object();

        private string _asyncMethod;
        private string _callBackMethod;
        //private string _1stStepParameterValues;
        //private string _2ndSteopParameterValues;
        //private string _1stStepStoreValues;
        //private string _2ndStepStoreValues;

        private object[] _asyncParameters;
        private TPanel tPanel5;
        private TLabel tLabelStatus;
        private TPanel tPanel4;
        private TPanel tPanel3;
        private TPanel tPanel2;

        private DateTime _processStartTime;

        private EnumDataObject _dataObject;
        private DataSet _dataSet;
        private Models.ModelSet _modelSet;
        private List<Models.Model> _modelList;
        private string _resultString;

        private Thread _workThread;
        private bool _assign = false;

        #endregion

        #region Excel

        /// <summary>
        /// Path for saving excel file
        /// </summary>
        protected string _excelFilePath;

        #endregion

        #region With Tab

        private TTabControl tabControl;

        private TLabel lblTitle;
        private TPanel pnlButtons;
        private TPictureBox picClose;
        private TPictureBox pic;
        private TPictureBox tPictureBox1;

        private TSolidProgressBar progressBar1;
        private TabPage tabPage;

        /// <summary>
        /// Top panel
        /// </summary>
        public TPanel tPanelTop;
        private System.Windows.Forms.Timer timerCurrent;
        private TLabel tLabelTime;
        private IContainer components;
        private TPictureBox picBookMark;
        private DevExpress.XtraBars.PopupMenu PopMenuBase;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;

        /// <summary>
        /// Main panel
        /// </summary>
        public TPanel tPanelMain;

        #endregion

        #endregion

        #region Property

        /// <summary>
        /// UI informtaion
        /// </summary>
        [Category(UIControlHeader._WRAPPED_PROPERTY), Description("Value of Selected item")]
        public UIBasicModel UIInformation { get { return this._ui; } set { this._ui = value; } }

        /// <summary>
        /// If 'YES', this UI applys UI Automation of TAPFX
        /// </summary>
        [Category(UIControlHeader._WRAPPED_PROPERTY), Description("Value of Selected item")]
        public EnumFlagYN UseUIAutomation { get { return this._useUIAutomation; } set { this._useUIAutomation = value; } }

        /// <summary>
        /// if 'YES', form is in progressing
        /// </summary>
        public bool InProgress { get { return this._continue; } }

        /// <summary>
        /// UI base argument
        /// </summary>
        public string UIBaseArgument { get; set; }

        /// <summary>
        /// UI Log
        /// </summary>
        public UILog _UILog
        {
            get
            {
                if (this._uiLog == null)
                    this._uiLog = new UILog();

                return this._uiLog;
            }
        }

        /// <summary>
        /// Tab control related with this UI
        /// </summary>
        public TTabControl TabControl { set { this.tabControl = value; } }

        /// <summary>
        /// Tab page related with this UI
        /// </summary>
        public TabPage TabPage { get { return this.tabPage; } set { this.tabPage = value; } }

        /// <summary>
        /// UI Title
        /// </summary>
        public string UITitle {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = (string)value; }
        }

        /// <summary>
        /// BookMark Assign
        /// </summary>
        public bool BookMarkAssign
        {
            get { return this._assign; }
            set
            {
                this._assign = value;
                if (_assign)
                    picBookMark.Image = Properties.Resources.star16;
                else
                    picBookMark.Image = Properties.Resources.destar16;
            }
        }

        public Type ServiceType { get => _serviceType; set => _serviceType = value; }
        public object ServiceObj { get => _serviceObj; set => _serviceObj = value; }
        #endregion

        #region Delegate

        /// <summary>
        /// Refresh UI Status delegate
        /// </summary>
        /// <param name="message">Message</param>
        public delegate void RefreshUIStatusEventHandler(string message);

        /// <summary>
        /// Work status handler delegate
        /// </summary>
        /// <param name="msgType">Message type</param>
        /// <param name="message">Message</param>
        public delegate void SetWorkStatusHandler(EnumMsgType msgType, string message);

        /// <summary>
        /// BookMark Status delegate
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="uiBasicModel"></param>
        public delegate void SetBookMarkEventHandler(bool flag, UIBasicModel uiBasicModel);

        /// <summary>
        /// Asynchronous callback delegate
        /// </summary>
        /// <param name="callMethodName"></param>
        protected delegate void AsyncCallEndEventHandler(string callMethodName);

        private delegate void SetProgress(bool visible, int vlaue);
        private delegate void SetUIDelegate();
        private delegate void BeginTaskDelegate();

        #endregion

        #region Event

        /// <summary>
        /// Refresh UI Status event handler
        /// </summary>
        public event RefreshUIStatusEventHandler RefreshUIStatusHandler;

        /// <summary>
        /// BookMark Status event handler
        /// </summary>
        public event SetBookMarkEventHandler SetBookMarkHandler;

        /// <summary>
        /// Asynchronous working event handler
        /// </summary>
        protected event EventHandler AsyncCallCancel;

        /// <summary>
        /// Asynchronous callback event handler
        /// </summary>
        protected event AsyncCallEndEventHandler AsyncCallEnd;



        #endregion

        #region Abstract Method

        /// <summary>
        /// Write code for "Search" in this method. Button assigned command type "Search" will call this method.
        /// </summary>
        virtual protected void Search()
        {
        }

        /// <summary>
        /// Write code for Main action in this method. Button assigned command type "Command" will call this method.
        /// </summary>
        virtual protected void Command()
        {
        }

        /// <summary>
        /// Write code for "Delete" in this method. Button assigned command type "Delete" will call this method.
        /// </summary>
        virtual protected void Delete()
        {
        }

        /// <summary>
        /// Write code for "Add" in this method. Button assigned command type "Add" will call this method.
        /// </summary>
        virtual protected void Add()
        {
        }

        /// <summary>
        /// This method initialize UI. This method loads controls of all containers and sets events as default.
        /// </summary>
        virtual protected void Initialize()
        {
            #region Initialize

            try
            {
                foreach (ContainerBasicModel tmpContainer in this._ui.Containers.Models)
                {
                    tmpContainer.LoadNestModel();
                }

                this._containers = this._ui.Containers;
                this.InitializeUI();
                this.SetEvents();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// Write excuting default action code in this method. Other UI will call this command for executing this UI.
        /// </summary>
        /// <param name="arguments"></param>
        virtual public void ExecuteCommand(ArgumentPack arguments)
        {
        }

        /// <summary>
        /// Write assigning code that assigns event handler to control in this method.
        /// </summary>
        virtual protected void SetEvents()
        {
        }

        /// <summary>
        /// Wirte code for context menu in this method.
        /// </summary>
        /// <param name="menuName"></param>
        virtual protected void ExecuteContextMenu(string menuName)
        {
        }

        /// <summary>
        /// Write code for opening context menu.
        /// </summary>
        /// <param name="contextMenu">Instance of context menu object</param>
        virtual protected void OpenContextMenu(ContextMenuStrip contextMenu)
        {
        }

        #endregion

        #region Initialize

        /// <summary>
        /// This method Initializes UI
        /// </summary>
        protected void InitializeUI()
        {
            #region Initialize UI

            try
            {
                if (this._useUIAutomation == EnumFlagYN.YES)
                    this.InitializeUIAutomation();
                else
                    this.InitializeUIManually();

                this.tLabelStatus.Text = this._converter.ConvertPhrase("ready");


                if (this.UIInformation != null)
                {
                    if (this.BookMarkCheck())
                    {
                        BookMarkAssign = true;
                    }
                    else
                    {
                        BookMarkAssign = false;
                    }
                }
                else
                {
                    picBookMark.Visible = false;
                    picClose.Visible = false;
                }
                //서버 관련
                //ServiceAdapter.EndExecute += new ServiceAdapterEventHandler(ServiceAdapter_EndExecute);

                return;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method initialize UI in manually type UI
        /// </summary>
        protected void InitializeUIManually()
        {
            #region InitializeUIManually

            try
            {
                //EnumLanguage tmpLang = (EnumLanguage)Enum.Parse(typeof(EnumLanguage), TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.UserLanguage);
                EnumLanguage tmpLang = (EnumLanguage)Enum.Parse(typeof(EnumLanguage), InfoBase._USER_INFO.Language);

                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                string tmpExecutablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";

                string fressageFilePath = Path.Combine(tmpExecutablePath, "mnls", TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.LocalFile);
                this._converter = new NeutralConverter(tmpLang, EnumUseFor.TEXT, false, fressageFilePath);
                this._translator = new TemplateConverter(tmpLang, false, fressageFilePath);

                foreach (System.Windows.Forms.Control tmpControl in this.Controls)
                {
                    #region Windows Controls

                    try
                    {
                        if (tmpControl.GetType().Equals(typeof(TPanel)))
                            this.ConvertPanel(tmpControl);
                        else if (tmpControl.GetType().Equals(typeof(TGroupBox)))
                        {
                            this.SetText(tmpControl);
                            this.ConvertGroup(tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(TTitledGroupBox)))
                        {
                            this.SetText(tmpControl);
                            this.ConvertGroup(tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(System.Windows.Forms.Panel)))
                        {
                            this.SetText(tmpControl);
                            this.ConvertGroup(tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(TCollapsiblePanel)))
                        {
                            this._converter.ConvertPhrase(((TCollapsiblePanel)tmpControl).HeaderText);
                            this.ConvertGroup(tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(TTabControl)))
                            this.ConvertTap(tmpControl);
                        else if (tmpControl.GetType().Equals(typeof(TListView)))
                            this.ConvertListView(tmpControl);
                        else if (tmpControl.GetType().Equals(typeof(TTreeView)))
                            this.ConvertTreeView(tmpControl);
                        else if (tmpControl.GetType().Equals(typeof(TComboBox)))
                            this.ConvertComboBox(tmpControl);
                        else
                            this.SetText(tmpControl);
                    }
                    catch
                    {
                        continue;
                    }

                    #endregion
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method applys UI Automation of TAPFX
        /// </summary>
        protected void InitializeUIAutomation()
        {
            #region Apply Text

            try
            {
                if (!object.Equals(this.UIInformation, null))
                {
                    EnumLanguage tmpLang = (EnumLanguage)Enum.Parse(typeof(EnumLanguage), InfoBase._USER_INFO.Language);
                    this._converter = new NeutralConverter(tmpLang, EnumUseFor.TEXT, false);

                    //Convert form title
                    this.Text = this._converter.ConvertPhrase(this.UIInformation.Name);

                    foreach (System.Windows.Forms.Control tmpControl in this.Controls)
                    {
                        try
                        {
                            if (tmpControl.GetType().Equals(typeof(TPanel)))
                            {
                                if (this.UIInformation.Containers.Contains(tmpControl.Name))
                                    this.AutomateControls((TPanel)tmpControl);

                                this.ConvertGroup(tmpControl);
                            }
                            else if (tmpControl.GetType().Equals(typeof(TGroupBox)))
                            {
                                this.SetText(tmpControl);
                                this.ConvertGroup(tmpControl);
                            }
                            else if (tmpControl.GetType().Equals(typeof(System.Windows.Forms.Panel)))
                            {
                                this.SetText(tmpControl);
                                this.ConvertGroup(tmpControl);
                            }
                            else if (tmpControl.GetType().Equals(typeof(TTabControl)))
                                this.ConvertTap(tmpControl);
                            else if (tmpControl.GetType().Equals(typeof(TListView)))
                                this.ConvertListView(tmpControl);
                            else if (tmpControl.GetType().Equals(typeof(TTreeView)))
                                this.ConvertTreeView(tmpControl);
                            else if (tmpControl.GetType().Equals(typeof(TComboBox)))
                                continue;
                            else
                                this.SetText(tmpControl);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    this.ApplyOtherControls();
                }

                InfoBase.FormTypeName = Base.Assemblies.AssemblyBase.Instance.FixAssemblyName(this.ToString());
                InfoBase.FormInstanceID = this.GetHashCode().ToString();
                InfoBase.WorkStateReceiverPortNo = TAP.Base.Configuration.ConfigurationManager.Instance.MessagingSection.Protocols["TCP"].ReceiverPort;
                InfoBase.UserID = "BETA_USER";
                InfoBase.CurrentCommand = string.Empty;

                return;
            }
            catch //(System.Exception ex)
            {
                //throw ex;
            }

            #endregion
        }

        private void ConvertControl()
        {

        }

        /// <summary>
        /// This method sets fressage code
        /// </summary>
        /// <param name="control"></param>
        protected void SetText(System.Windows.Forms.Control control)
        {
            #region Set Text

            try
            {
                //if (this._ui.Containers.Contains(control.Name))
                //{
                //    if ((int)this._ui.Containers[container].UIFunctions[control.Name].Authority > (int)InfoBase._USER_INFO.Authorities[this._ui.Name].Authority)
                //        control.Enabled = false;
                //}

                if (control.GetType() == typeof(TAP.UIControls.BasicControls.TSolidProgressBar))
                    return;

                if (((IUIControl)control).NeedToTranslate)
                {
                    if (control.GetType().Equals(typeof(TIconLabel)))
                        ((TIconLabel)control).Label = this._converter.ConvertPhrase(((TIconLabel)control).Label);
                    else if (control.GetType().Equals(typeof(TAP.UIControls.Sheets.TSheet)))
                        ((TAP.UIControls.Sheets.TSheet)control).FressageConverter = this._converter;
                    else
                        control.Text = this._converter.ConvertPhrase(control.Text);
                }

                //SEOIL 2019-01-28 Not Suppot For ControlAuthority
                //this.SetControlAuthority(control);

                return;
            }
            catch//(System.Exception ex)
            {
                //throw ex;
            }

            #endregion
        }

        private void SetControlAuthority(System.Windows.Forms.Control control)
        {
            #region Set Control Authority

            bool tmpHasAuthority = false;
            UIFunctionBasicModel tmpFunctionModel = null;

            try
            {
                tmpFunctionModel = this.FindControlInfo(control.Name);

                if (tmpFunctionModel == null || tmpFunctionModel.AllowedToEveryone == EnumFlagYN.YES)
                    return;

                foreach (string tmpAuthName in tmpFunctionModel.Authorities.Names)
                {
                    //UIAuthorityBasicModel tmpAuthority = tmpFunctionModel.Authorities[tmpAuthName];

                    #region Find in User Info

                    if (tmpAuthName == InfoBase._USER_INFO.Name)
                    {
                        tmpHasAuthority = true;
                        break;
                    }

                    #endregion

                    #region Find in User Group

                    for (int i = 0; i < InfoBase._USER_GROUP.Count; i++)
                    {
                        if (tmpAuthName == InfoBase._USER_GROUP[i].Name)
                        {
                            tmpHasAuthority = true;
                            break;
                        }
                    }

                    #endregion
                }

                control.Enabled = tmpHasAuthority;

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void ConvertPanel(System.Windows.Forms.Control control)
        {
            #region Convert Panel

            try
            {
                foreach (System.Windows.Forms.Control tmpControl in control.Controls)
                {
                    try
                    {
                        if (tmpControl.GetType().Equals(typeof(TPanel)))
                            this.ConvertPanel(tmpControl);
                        else if (tmpControl.GetType().Equals(typeof(GroupBox)) || tmpControl.GetType().Equals(typeof(TGroupBox)))
                        {
                            this.SetText(tmpControl);
                            this.ConvertGroup(tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(TTitledGroupBox)))
                        {
                            this.SetText(tmpControl);
                            this.ConvertGroup(tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(TCollapsiblePanel)))
                        {
                            this._converter.ConvertPhrase(((TCollapsiblePanel)tmpControl).HeaderText);
                            this.ConvertGroup(tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(TTabControl))
                            //|| tmpControl.GetType().Equals(typeof(TTabControl))
                            )
                            this.ConvertTap(tmpControl);
                        else if (tmpControl.GetType().Equals(typeof(TListView))
                            //|| tmpControl.GetType().Equals(typeof(TListView))
                            )
                            this.ConvertListView(tmpControl);
                        else if (tmpControl.GetType().Equals(typeof(TTreeView))
                            // || tmpControl.GetType().Equals(typeof(TTreeView))
                            )
                            this.ConvertTreeView(tmpControl);
                        else if (tmpControl.GetType().Equals(typeof(ComboBox)) || tmpControl.GetType().Equals(typeof(TComboBox)))
                            this.ConvertComboBox(tmpControl);
                        // Add By LiuShi For DEVDockPanel 2020-09-09
                        else if (tmpControl.GetType().Equals(typeof(DevExpress.XtraEditors.XtraUserControl))
                            || tmpControl.GetType().Equals(typeof(DevExpress.XtraBars.Docking.ControlContainer))
                                    || tmpControl.GetType().Equals(typeof(DevExpress.XtraBars.Docking.DockPanel)))
                            this.ConvertDevExpressContols(tmpControl);

                        //END 2020-08-06 Seoil Add.
                        else
                            this.SetText(tmpControl);
                    }
                    catch
                    {
                        continue;
                    }
                }

                return;
            }
            catch// (System.Exception ex)
            {
                //throw ex;
            }

            #endregion
        }

        private void ConvertDevExpressContols(System.Windows.Forms.Control control)
        {
            #region Convert DevExpressContol

            try
            {
                foreach (System.Windows.Forms.Control tmpControl in control.Controls)
                {
                    try
                    {
                        if (tmpControl.GetType().Equals(typeof(DevExpress.XtraBars.Docking.ControlContainer))
                            || tmpControl.GetType().Equals(typeof(DevExpress.XtraBars.Docking.DockPanel)))
                        {
                            this.ConvertDevExpressContols(tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(TableLayoutPanel)))
                        {
                            this.ConvertDevExpressContols(tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(TPanel)))
                        {
                            this.ConvertDevExpressContols(tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(DevExpress.XtraBars.Navigation.TabPane)))
                        {
                            this.ConvertDevExpressContols(tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(DevExpress.XtraBars.Navigation.TabNavigationPage)))
                        {
                            //Page Caption Change 추가안함.
                            this.ConvertDevExpressContols(tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(DevExpress.XtraNavBar.NavBarControl)))
                        {
                            //NavBarGroup Caption Change 추가안함.
                            this.ConvertDevExpressContols(tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(DevExpress.XtraNavBar.NavBarGroupControlContainer)))
                        {
                            this.ConvertDevExpressContols(tmpControl);
                        }
                        //add by liushi 2020-09-09
                        else if (tmpControl.GetType().Equals(typeof(DevExpress.XtraLayout.LayoutControl)))
                        {
                            this.ConvertDevExpressContols(tmpControl);
                        }
                        //else if (tmpControl.GetType().Equals(typeof(TAP.UIControls.BasicControlsDEV.TRadionButton)))
                        //{
                        //    this.ConvertRadioGroup(tmpControl);
                        //}
                        //Unimplemented status  
                        else
                            this.SetText(tmpControl);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch//(System.Exception ex)
            {
                //throw ex;
            }

            #endregion
        }

        private void ConvertGroup(System.Windows.Forms.Control control)
        {
            #region Convert Group

            try
            {
                foreach (System.Windows.Forms.Control tmpControl in control.Controls)
                {
                    try
                    {
                        if (tmpControl.GetType().Equals(typeof(TPanel)))
                        {
                            if (this._useUIAutomation == EnumFlagYN.YES)
                            {
                                if (this.UIInformation.Containers.Contains(tmpControl.Name))
                                    this.AutomateControls((TPanel)tmpControl);
                            }
                            else
                                this.ConvertPanel(tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(GroupBox)) || tmpControl.GetType().Equals(typeof(TGroupBox)))
                        {
                            this.SetText(tmpControl);
                            this.ConvertGroup(tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(TTitledGroupBox)))
                        {
                            this.SetText(tmpControl);
                            this.ConvertGroup(tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(TTabControl))
                            //|| tmpControl.GetType().Equals(typeof(TTabControl))
                            )
                            this.ConvertTap(tmpControl);
                        else if (tmpControl.GetType().Equals(typeof(TListView))
                            //|| tmpControl.GetType().Equals(typeof(TListView))
                            )
                            this.ConvertListView(tmpControl);
                        else if (tmpControl.GetType().Equals(typeof(TTreeView))
                            // || tmpControl.GetType().Equals(typeof(TTreeView))
                            )
                            this.ConvertTreeView(tmpControl);
                        else if (tmpControl.GetType().Equals(typeof(ComboBox)) || tmpControl.GetType().Equals(typeof(TComboBox)))
                            this.ConvertComboBox(tmpControl);
                        else
                            this.SetText(tmpControl);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch//(System.Exception ex)
            {
                //throw ex;
            }

            #endregion
        }

        private void ConvertTap(System.Windows.Forms.Control control)
        {
            #region Convert Tap

            try
            {
                foreach (System.Windows.Forms.TabPage tmpPage in ((System.Windows.Forms.TabControl)control).TabPages)
                {
                    try
                    {
                        if (this._converter.CurrentLanguage == EnumLanguage.CN)
                            tmpPage.Font = new System.Drawing.Font("NSimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));

                        tmpPage.Text = this._converter.ConvertPhrase(tmpPage.Text);
                        this.ConvertGroup(tmpPage);
                    }
                    catch
                    {
                        continue;
                    }
                }

                return;
            }
            catch
            {

            }

            #endregion
        }

        private void ConvertRadioGroup(System.Windows.Forms.Control control)
        {
            #region Convert Tap

            try
            {
                if (((IUIControl)control).NeedToTranslate)
                {
                    foreach (DevExpress.XtraEditors.Controls.RadioGroupItem radioGroupItem in ((DevExpress.XtraEditors.RadioGroup)control).Properties.Items)
                    {
                        try
                        {
                            radioGroupItem.Description = this._converter.ConvertPhrase(radioGroupItem.Description);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                return;
            }
            catch
            {

            }

            #endregion
        }

        private void ConvertListView(System.Windows.Forms.Control control)
        {
            #region Convert List View

            try
            {
                foreach (System.Windows.Forms.ColumnHeader tmpColulmn in ((System.Windows.Forms.ListView)control).Columns)
                {
                    try
                    {
                        tmpColulmn.Text = this._converter.ConvertPhrase(tmpColulmn.Text);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch
            {
            }

            #endregion
        }

        private void ConvertTreeView(System.Windows.Forms.Control control)
        {
            #region Convert Tree View

            try
            {
                if (control.GetType().Equals(typeof(System.Windows.Forms.TreeView)))
                {
                    foreach (System.Windows.Forms.TreeNode tmpNode in ((System.Windows.Forms.TreeView)control).Nodes)
                    {
                        try
                        {
                            tmpNode.Text = this._converter.ConvertPhrase(tmpNode.Text);

                            if (tmpNode.Nodes.Count > 0)
                                this.ConvertTreeNode(tmpNode);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }

                return;
            }
            catch
            {
            }

            #endregion
        }

        private void ConvertTreeNode(System.Windows.Forms.TreeNode node)
        {
            #region Convert Tree Node

            try
            {
                foreach (System.Windows.Forms.TreeNode tmpNode in ((System.Windows.Forms.TreeNode)node).Nodes)
                {
                    try
                    {
                        tmpNode.Text = this._converter.ConvertPhrase(tmpNode.Text);

                        if (tmpNode.Nodes.Count > 0)
                            this.ConvertTreeNode(tmpNode);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch
            {
                //
            }


            #endregion
        }

        private void ConvertComboBox(System.Windows.Forms.Control comboBox)
        {
            #region Convert ComboBox

            try
            {
                if (((TAP.UIControls.BasicControls.TComboBox)comboBox).BuiltInDispay == EnumBuitInDispay.CODE &&
                    ((TAP.UIControls.BasicControls.TComboBox)comboBox).BuiltInCategory.Length > 0 &&
                    ((TAP.UIControls.BasicControls.TComboBox)comboBox).BuiltInSubCategory.Length > 0)
                    ((TAP.UIControls.BasicControls.TComboBox)comboBox).BindBuiltInData();

                return;
            }
            catch //(System.Exception ex)
            {
                //throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method translates item text of context menu
        /// </summary>
        /// <param name="context">Context menu</param>
        protected void ConvertContextMenu(System.Windows.Forms.ContextMenuStrip context)
        {
            #region Convert ComboBox

            try
            {
                foreach (ToolStripMenuItem tmp in context.Items)
                {
                    tmp.Text = this._converter.ConvertPhrase(tmp.Text);
                }

                return;
            }
            catch //(System.Exception ex)
            {
                //throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method is not used.
        /// </summary>
        virtual protected void ApplyOtherControls()
        {
        }

        private UIFunctionBasicModel FindControlInfo(string controlName)
        {
            #region Find Control Info

            UIFunctionBasicModel retVal = null;

            try
            {
                foreach (string tmpContainerName in this.UIInformation.Containers.Names)
                {
                    foreach (string tmpFuncName in this.UIInformation.Containers[tmpContainerName].UIFunctions.Names)
                    {
                        UIFunctionBasicModel tmpFuncModel = this.UIInformation.Containers[tmpContainerName].UIFunctions[tmpFuncName];

                        if (tmpFuncModel.ControlTypeName == controlName)
                        {
                            retVal = tmpFuncModel;
                            return retVal;
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

        #endregion

        #region Public Methods

        /// <summary>
        /// This method collects authority for this UI.
        /// </summary>
        /// <returns>Numeric value for authority</returns>
        protected int GetUserAuthority()
        {
            #region Get user authority

            try
            {
                return 0;
                //return TAP.UI.InfoBase._USER_INFO.GetAuthorityForUI(this._ui.MDI, this._ui.MainMenu, this._ui.Name);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is not used
        /// </summary>
        /// <param name="assemblyName">Assembly name</param>
        /// <param name="className">Class name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="tmpPack">Argument list</param>
        protected void CallRemoteMethod(string assemblyName, string className, string methodName, ArgumentPack[] tmpPack)
        {
            #region Call Remote Method

            //TAP.Workflow.Test.TestExcuter tmp = new TAP.Workflow.Test.TestExcuter();
            //ResponseMessage retVal = tmp.Execute(assemblyName, className, methodName, tmpPack);

            //if (!retVal.Success)
            //    throw new Exception(retVal.ErrorMessage);

            #endregion
        }

        #endregion

        #region UI Automation

        private void AutomateControls(TPanel tmpContainer)
        {
            #region Automate controls

            ContainerBasicModel tmpModel = null;

            try
            {
                if (this._ui.Containers.Contains(tmpContainer.Name))
                {
                    tmpModel = this._containers[tmpContainer.Name];

                    TGroupBox tmpGroupBox = new TGroupBox();
                    tmpGroupBox.Text = this._converter.ConvertPhrase(this.UIInformation.Containers[tmpContainer.Name].Title.ToUpper());
                    //tmpGroupBox.Height = tmpContainer.Height - 3;
                    tmpGroupBox.Dock = DockStyle.Fill;
                    tmpContainer.Controls.Add(tmpGroupBox);

                    TPanel tmpInternalPanel = new TPanel();
                    tmpInternalPanel.Dock = DockStyle.Fill;
                    tmpGroupBox.Controls.Add(tmpInternalPanel);

                    //TEMP
                    //switch (tmpModel.DisplayLayout)
                    //{
                    //    case UtilEnumDisplay.Horizontal: this.BindHorizontalControls(this.UIInformation.Containers[tmpContainer.Name].UIFunctions, tmpInternalPanel, 0); break;
                    //    case UtilEnumDisplay.Vertical: this.BindVeticalControls(this.UIInformation.Containers[tmpContainer.Name].UIFunctions, tmpInternalPanel); break;
                    //    case UtilEnumDisplay.Complex: this.BindComplexControls(this.UIInformation.Containers[tmpContainer.Name].UIFunctions, tmpInternalPanel, tmpModel.ControlsOnRow); break;
                    //}
                }

                return;
            }
            catch (System.Exception ex)
            {
                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.APPLY, EnumGeneralTemplateType.FAIL, "<UI Automation>");
                //string tmpMessage = string.Format("Exception occured during automaing cotainer '{0}'.", tmpContainer.Name);
                throw new Exception(tmpMessage + ":" + ex.ToString());
            }

            #endregion
        }

        #region Lay out

        private void BindHorizontalControls(UIFunctionBasicModelSet models, TPanel tmpContainer, int height)
        {
            #region Bind Horizontal Control

            int tmpStartPoint = 0;
            int tmpEndPoint = 0;
            string[] tmpControls = null;

            try
            {
                tmpControls = models.CreateSeqenceArray();

                for (int i = 0; i < tmpControls.Length; i++)
                {
                    UIFunctionBasicModel tmpFunctionModel = models[tmpControls[i]];

                    switch (tmpFunctionModel.ControlType)
                    {
                        case EnumUIControlType.COMBOBOX:
                        case EnumUIControlType.TEXTBOX:
                        case EnumUIControlType.NUMBERBOX:
                        case EnumUIControlType.DATETIME_PICKER:
                        case EnumUIControlType.DOUBLE_DATETIME_PICKER:

                            #region Combobox and Text Box

                            TPanel tmpControl = null;

                            if (tmpFunctionModel.ControlType == EnumUIControlType.COMBOBOX)
                                tmpControl = this.AddComboBox(tmpFunctionModel, _default_Control_Width);
                            else if (tmpFunctionModel.ControlType == EnumUIControlType.DATETIME_PICKER)
                                tmpControl = this.AddDateTimePicker(tmpFunctionModel, _default_Control_Width);
                            else if (tmpFunctionModel.ControlType == EnumUIControlType.NUMBERBOX)
                                tmpControl = this.AddNumberBox(tmpFunctionModel, _default_Control_Width);
                            else if (tmpFunctionModel.ControlType == EnumUIControlType.DOUBLE_DATETIME_PICKER)
                                tmpControl = this.AddDoubleDateTimePicker(tmpFunctionModel, _default_Control_Width);
                            else
                                tmpControl = this.AddTextBox(tmpFunctionModel, _default_Control_Width);

                            //tmpControl.Dock = DockStyle.Left;
                            tmpContainer.Controls.Add(tmpControl);
                            tmpControl.Location = new Point(tmpStartPoint, height);
                            tmpStartPoint += tmpControl.Width + _default_Blank_width;
                            break;

                        #endregion

                        case EnumUIControlType.BUTTON:

                            #region Button

                            TButton tmpButton = this.AddButton(tmpFunctionModel, _default_Control_Width);
                            tmpButton.Dock = DockStyle.Right;
                            tmpButton.Location = new Point(tmpContainer.Width - tmpEndPoint, height);
                            tmpEndPoint += tmpButton.Width + _default_Control_Width;
                            tmpContainer.Controls.Add(tmpButton);
                            break;
                        #endregion

                        case EnumUIControlType.SHEET:
                        case EnumUIControlType.SQUAREBOARD:
                        case EnumUIControlType.SQUAREMAP:

                            #region DefaultSheet, Map, Board

                            TPanel tmpPanel = null;

                            if (tmpFunctionModel.ControlType == EnumUIControlType.SQUAREMAP)
                                tmpPanel = this.AddMap(tmpFunctionModel, _default_Control_Width);
                            else if (tmpFunctionModel.ControlType == EnumUIControlType.SQUAREBOARD)
                                tmpPanel = this.AddBoard(tmpFunctionModel, _default_Control_Width);
                            //else if (tmpFunctionModel.ControlType == EnumUIControlType.SHEET)
                            //    tmpPanel = this.AddDefaultSheet(tmpFunctionModel, _default_Control_Width);

                            tmpPanel.Dock = DockStyle.Fill;
                            tmpContainer.Controls.Add(tmpPanel);
                            break;

                            #endregion
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

        private void BindVeticalControls(UIFunctionBasicModelSet models, TPanel tmpContainer)
        {
            #region Bind Vetical Controls

            int tmpStartPoint = 0;
            int tmpEndPoint = 0;
            string[] tmpControls = null;

            try
            {
                tmpControls = models.CreateSeqenceArray();

                for (int i = 0; i < tmpControls.Length; i++)
                {
                    UIFunctionBasicModel tmpFunctionModel = models[tmpControls[i]];

                    switch (tmpFunctionModel.ControlType)
                    {
                        case EnumUIControlType.COMBOBOX:
                        case EnumUIControlType.TEXTBOX:
                        case EnumUIControlType.NUMBERBOX:
                        case EnumUIControlType.DATETIME_PICKER:
                        case EnumUIControlType.DOUBLE_DATETIME_PICKER:

                            #region Combobox and Text Box

                            TPanel tmpControl = null;

                            if (tmpFunctionModel.ControlType == EnumUIControlType.COMBOBOX)
                                tmpControl = this.AddComboBox(tmpFunctionModel, _default_Control_Width);
                            else if (tmpFunctionModel.ControlType == EnumUIControlType.DATETIME_PICKER)
                                tmpControl = this.AddDateTimePicker(tmpFunctionModel, _default_Control_Width);
                            else if (tmpFunctionModel.ControlType == EnumUIControlType.NUMBERBOX)
                                tmpControl = this.AddNumberBox(tmpFunctionModel, _default_Control_Width);
                            else if (tmpFunctionModel.ControlType == EnumUIControlType.DOUBLE_DATETIME_PICKER)
                                tmpControl = this.AddDoubleDateTimePicker(tmpFunctionModel, _default_Control_Width);
                            else
                                tmpControl = this.AddTextBox(tmpFunctionModel, _default_Control_Width);

                            //tmpControl.Dock = DockStyle.Left;
                            tmpContainer.Controls.Add(tmpControl);
                            tmpControl.Location = new Point(0, tmpStartPoint);
                            tmpStartPoint += _default_Panel_height;
                            break;

                        #endregion

                        case EnumUIControlType.BUTTON:

                            #region Button

                            TButton tmpButton = this.AddButton(tmpFunctionModel, _default_Control_Width);
                            tmpButton.Dock = DockStyle.Right;
                            tmpButton.Location = new Point(0, tmpContainer.Width - tmpEndPoint);
                            tmpEndPoint += tmpButton.Width + _default_Control_Width;
                            tmpContainer.Controls.Add(tmpButton);
                            break;
                        #endregion

                        case EnumUIControlType.SHEET:
                        case EnumUIControlType.SQUAREBOARD:
                        case EnumUIControlType.SQUAREMAP:

                            #region DefaultSheet, Map, Board

                            TPanel tmpPanel = null;

                            if (tmpFunctionModel.ControlType == EnumUIControlType.SQUAREMAP)
                                tmpPanel = this.AddMap(tmpFunctionModel, _default_Control_Width);
                            else if (tmpFunctionModel.ControlType == EnumUIControlType.SQUAREBOARD)
                                tmpPanel = this.AddBoard(tmpFunctionModel, _default_Control_Width);
                            //else if (tmpFunctionModel.ControlType == EnumUIControlType.SHEET)
                            //    tmpPanel = this.AddDefaultSheet(tmpFunctionModel, _default_Control_Width);

                            tmpPanel.Dock = DockStyle.Fill;
                            tmpContainer.Controls.Add(tmpPanel);
                            break;

                            #endregion
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

        private void BindComplexControls(UIFunctionBasicModelSet models, TPanel tmpContainer, int controlsOneRow)
        {
            #region Bind Complex Controls

            int tmpStartPoint = 0;
            int tmpTotalControlCount = 0;
            int tmpCircle = 1;
            string[] tmpControls = null;
            TPanel tmpSubPanel = null;
            UIFunctionBasicModelSet tmpFunctions = null;

            try
            {
                //그룹박스에 패널을 삽입한다. 
                tmpSubPanel = new TPanel();
                tmpSubPanel.Dock = DockStyle.Fill;
                tmpContainer.Controls.Add(tmpSubPanel);

                tmpControls = models.CreateSeqenceArray();

                if (controlsOneRow < 1)
                    controlsOneRow = 1;

                while (tmpTotalControlCount < models.Count)
                {
                    if (!object.Equals(tmpFunctions, null))
                    {
                        tmpFunctions.Dispose();
                        tmpFunctions = null;
                    }

                    tmpFunctions = new UIFunctionBasicModelSet();

                    //for (int k = tmpStartPoint; k < _default_width_Display_Count * tmpCircle; k++)
                    for (int k = tmpStartPoint; k < controlsOneRow * tmpCircle; k++)
                    {
                        if (models.Count >= k + 1)
                        {
                            UIFunctionBasicModel tmpUIFunction = models[tmpControls[k]];
                            tmpFunctions.Add(tmpUIFunction);

                            //Double로 시작하는 컨트롤의 경우 두칸을 차지한다.
                            //if (Enum.GetName(typeof(EnumUIControlType), models[tmpControls[k]].ControlType).StartsWith("DOUBLE"))
                            //    k++;
                        }

                        tmpTotalControlCount++;

                    }

                    //tmpSubPanel: 컨테이너로 그룹박스 대신 그룹박스 내의 패널을 던진다. 
                    this.BindHorizontalControls(tmpFunctions, tmpSubPanel, _default_Panel_height * (tmpCircle - 1) + 5);
                    tmpStartPoint += controlsOneRow;
                    //tmpStartPoint += _default_width_Display_Count;
                    tmpCircle++;
                }

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Add Controls

        /// <summary>
        /// This method draw controls that is not designed from UI-Modeler.
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="models">Model</param>
        /// <param name="enabels">If "TRUE", this control is enabled</param>
        protected void DrawCustomControls(string containerName, Models.ModelSet models, bool enabels)
        {
            #region DrawPartControls

            int tmpHeight = 0;
            TAP.UIControls.IUIControl tmpManagedContainer = null;
            TPanel tmpContainerPanel = null;

            try
            {
                tmpManagedContainer = this.FindControl("C");
                tmpContainerPanel = (TPanel)((TGroupBox)((TPanel)tmpManagedContainer).Controls[0]).Controls[0];

                tmpContainerPanel.Controls.Clear();

                foreach (string tmpPartName in models.Names)
                {
                    //Panel
                    TPanel tmpPanel = new TPanel();
                    tmpPanel.Size = new Size(_default_Control_Width, _default_Panel_height);

                    //Label
                    TAP.UIControls.BasicControls.TLabel tmpLabel = new TLabel();
                    tmpLabel.Text = this._converter.ConvertPhrase(tmpPartName);
                    tmpLabel.Dock = DockStyle.Left;
                    tmpLabel.AutoSize = true;
                    tmpLabel.Font = new Font("Tahoma", 8.0f, FontStyle.Regular);
                    tmpPanel.Controls.Add(tmpLabel);

                    //Text Box
                    TTextBox tmpTextBox = new TTextBox();
                    tmpTextBox.Name = tmpPartName;
                    tmpTextBox.Size = new Size(_default_Control_Width - _default_blankSize, _default_Control_height);
                    tmpTextBox.Location = new Point(_default_blankSize, 0);
                    tmpTextBox.Font = new Font("Tahoma", 8.0f, FontStyle.Regular);
                    tmpTextBox.Enabled = enabels;
                    tmpPanel.Controls.Add(tmpTextBox);

                    tmpContainerPanel.Controls.Add(tmpPanel);
                    tmpPanel.Location = new Point(0, tmpHeight);
                    tmpHeight += _default_Panel_height;

                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private TPanel AddTextBox(UIFunctionBasicModel function, int width)
        {
            #region Add Text box

            TPanel retVal = null;

            try
            {
                //Panel
                retVal = new TPanel();
                retVal.Size = new Size(width, _default_Panel_height);

                //Label
                TAP.UIControls.BasicControls.TLabel tmpLabel = new TLabel();
                tmpLabel.Text = this._converter.ConvertPhrase(function.Text.ToUpper());
                tmpLabel.Dock = DockStyle.Left;
                tmpLabel.AutoSize = true;
                tmpLabel.Font = new Font("Tahoma", 8.0f, FontStyle.Regular);
                retVal.Controls.Add(tmpLabel);

                //Text Box
                TTextBox tmpTextBox = new TTextBox();
                tmpTextBox.Name = function.Name;
                tmpTextBox.Size = new Size(width - _default_blankSize, _default_Control_height);
                //tmpTextBox.Dock = DockStyle.Right;
                tmpTextBox.Location = new Point(_default_blankSize, 0);
                tmpTextBox.Font = new Font("Tahoma", 8.0f, FontStyle.Regular);
                tmpTextBox.Enabled = function.Enables == EnumFlagYN.YES ? true : false;
                tmpTextBox.Visible = function.Visibles == EnumFlagYN.YES ? true : false;
                retVal.Controls.Add(tmpTextBox);

                #region Set Context Menu

                if (function.ContextMenu.Length > 0 && this._containers.Contains(function.ContextMenu))
                    tmpTextBox.ContextMenuStrip = this.MakeContextMenu(function.ContextMenu);

                #endregion

                #region Set Events

                //switch (function.CommandType)
                //{
                //    case EnumCommandType.SEARCH:
                //        tmpTextBox.KeyDown += new KeyEventHandler(TextBoxSearchEventHandler);
                //        break;
                //}

                #endregion

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private TPanel AddComboBox(UIFunctionBasicModel function, int width)
        {
            #region Add Text box

            TPanel retVal = null;

            try
            {
                //Panel
                retVal = new TPanel();
                retVal.Size = new Size(width, _default_Panel_height);

                //Label
                TAP.UIControls.BasicControls.TLabel tmpLabel = new TLabel();
                tmpLabel.Text = this._converter.ConvertPhrase(function.Text.ToUpper());
                tmpLabel.Dock = DockStyle.Left;
                tmpLabel.AutoSize = true;
                tmpLabel.Font = new Font("Tahoma", 8.0f, FontStyle.Regular);
                retVal.Controls.Add(tmpLabel);

                //Combobox
                TComboBox tmpComboBox = new TComboBox();
                tmpComboBox.Name = function.Name;
                tmpComboBox.Size = new Size(width - _default_blankSize, _default_Control_height);
                //tmpComboBox.Dock = DockStyle.Right;
                tmpComboBox.Location = new Point(_default_blankSize, 0);
                tmpComboBox.Font = new Font("Tahoma", 8.0f, FontStyle.Regular);
                tmpComboBox.Enabled = function.Enables == EnumFlagYN.YES ? true : false;
                tmpComboBox.Visible = function.Visibles == EnumFlagYN.YES ? true : false;
                retVal.Controls.Add(tmpComboBox);

                #region Set Context Menu

                if (function.ContextMenu.Length > 0 && this._containers.Contains(function.ContextMenu))
                    tmpComboBox.ContextMenuStrip = this.MakeContextMenu(function.ContextMenu);

                #endregion

                #region Set Events

                //switch (function.CommandType)
                //{
                //    case EnumCommandType.SEARCH:
                //        tmpComboBox.TextChanged +=new EventHandler(ComboBoxSearch_TextChanged);
                //        break;
                //}

                #endregion

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private TButton AddButton(UIFunctionBasicModel function, int width)
        {
            #region Add Text box

            TButton retVal = null;

            try
            {
                //Button
                retVal = new TButton();
                retVal.Name = function.Name;
                //tmpButton.Size = new Size(width - _default_blankSize, _default_Control_height);
                retVal.Dock = DockStyle.Right;
                retVal.Font = new Font("Tahoma", 8.0f, FontStyle.Regular);
                retVal.Text = this._converter.ConvertPhrase(function.Text.ToUpper());

                #region Set Context Menu

                if (function.ContextMenu.Length > 0 && this._containers.Contains(function.ContextMenu))
                    retVal.ContextMenuStrip = this.MakeContextMenu(function.ContextMenu);

                #endregion

                #region Events

                switch (function.CommandType)
                {
                    case TAP.Models.UIBasic.EnumCommandType.EXIT:
                        retVal.Click += new EventHandler(ButtonExitEventHandler);
                        retVal.CommandType = TAP.UIControls.BasicControls.EnumCommandType.CLOSE;
                        break;
                    case TAP.Models.UIBasic.EnumCommandType.CLEAR:
                        retVal.Click += new EventHandler(ButtonClearEventHandler);
                        retVal.CommandType = TAP.UIControls.BasicControls.EnumCommandType.CANCEL;
                        break;
                    case TAP.Models.UIBasic.EnumCommandType.DELETE:
                        retVal.Click += new EventHandler(ButtonDeleteEventHandler);
                        retVal.CommandType = TAP.UIControls.BasicControls.EnumCommandType.DELETE;
                        break;
                    case TAP.Models.UIBasic.EnumCommandType.COMMAND:
                        retVal.Click += new EventHandler(ButtonCommandEventHandler);
                        retVal.CommandType = TAP.UIControls.BasicControls.EnumCommandType.OTHERS;
                        break;
                    case TAP.Models.UIBasic.EnumCommandType.SEARCH:
                        retVal.Click += new EventHandler(ButtonSearchEventHandler);
                        retVal.CommandType = TAP.UIControls.BasicControls.EnumCommandType.SEARCH;
                        break;
                    case TAP.Models.UIBasic.EnumCommandType.ADD:
                        retVal.Click += new EventHandler(ButtonBoxAddEventHandler);
                        retVal.CommandType = TAP.UIControls.BasicControls.EnumCommandType.ADD;
                        break;
                }

                #endregion

                //if (function.Events != EnumSupportedEvents.NONE)
                //{
                //    switch (function.Events)
                //    {
                //        case EnumSupportedEvents.BUTTON_CLICK:
                //            retVal.Click += new EventHandler(ButtonClicked); break;
                //    }
                //}

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #region NOT USED

        //private TPanel AddDefaultSheet(UIFunctionBasicModel function, int width)
        //{
        //    #region Add DefaultSheet

        //    TPanel retVal = null;
        //    AdvdSpread tmpDefaultSheet = null;

        //    try
        //    {
        //        retVal = new TPanel();
        //        retVal.Dock = DockStyle.Fill;
        //        tmpDefaultSheet = new AdvdSpread();
        //        tmpDefaultSheet.ControlID = function.Name;
        //        tmpDefaultSheet.Dock = DockStyle.Fill;

        //        //if( function.ColumnNameList.Length > 0 )
        //        //    tmpDefaultSheet.SetColumns(function.ColumnNameList);

        //        //this.ConvertDefaultSheetColumns(tmpDefaultSheet);

        //        //tmpDefaultSheet.SetRowCount(0);
        //        //retVal.Controls.Add(tmpDefaultSheet);

        //        //if (function.Events != EnumSupportedEvents.NONE)
        //        //{
        //        //    switch (function.Events)
        //        //    {
        //        //        case EnumSupportedEvents.SHEET_SELECTCELL:
        //        //            tmpDefaultSheet.KeyDown += new KeyEventHandler(ButtonClicked); break;
        //        //    }
        //        //}

        //        #region Set Context Menu

        //        if (function.ContextMenu.Length > 0 && this._containers.Contains(function.ContextMenu))
        //            retVal.ContextMenuStrip = this.MakeContextMenu(function.ContextMenu);

        //        #endregion

        //        return retVal;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }

        //    #endregion
        //}

        #endregion

        private TPanel AddMap(UIFunctionBasicModel function, int width)
        {
            #region Add DefaultSheet

            //TPanel retVal = null;
            //SquareMap tmpMap = null;

            //try
            //{
            //    retVal = new TPanel();
            //    retVal.Dock = DockStyle.Fill;

            //    tmpMap = new SquareMap();
            //    tmpMap.Name = function.Name;
            //    tmpMap.Dock = DockStyle.Fill;
            //    retVal.Controls.Add(tmpMap);
            //   // tmpMap.GetSelectedCell += new TAP.Controls.Map.SquareMap.ControlClickEventHandler(SquareMapGetSelectedCell);

            //    #region Set Context Menu

            //    if (function.ContextMenu.Length > 0 && this._containers.Contains(function.ContextMenu))
            //        retVal.ContextMenuStrip = this.MakeContextMenu(function.ContextMenu);

            //    #endregion

            //    return retVal;
            //}
            //catch (System.Exception ex)
            //{
            //    throw ex;
            //}

            return null;

            #endregion
        }

        private TPanel AddBoard(UIFunctionBasicModel function, int width)
        {
            #region Add DefaultSheet

            //TPanel retVal = null;
            //SquareBoard tmpBoard = null;

            //try
            //{
            //    retVal = new TPanel();
            //    retVal.Dock = DockStyle.Fill;

            //    tmpBoard = new SquareBoard();
            //    tmpBoard.Name = function.Name;
            //    tmpBoard.Dock = DockStyle.Fill;
            //    retVal.Controls.Add(tmpBoard);

            //    #region Set Context Menu

            //    if (function.ContextMenu.Length > 0 && this._containers.Contains(function.ContextMenu))
            //        retVal.ContextMenuStrip = this.MakeContextMenu(function.ContextMenu);

            //    #endregion

            //    return retVal;

            return null;
            //}
            //catch (System.Exception ex)
            //{
            //    throw ex;
            //}

            #endregion
        }

        private TPanel AddNumberBox(UIFunctionBasicModel function, int width)
        {
            #region Add Text box

            TPanel retVal = null;

            try
            {
                //Panel
                retVal = new TPanel();
                retVal.Size = new Size(width, _default_Panel_height);

                //Label
                TAP.UIControls.BasicControls.TLabel tmpLabel = new TLabel();
                tmpLabel.Text = this._converter.ConvertPhrase(function.Text.ToUpper());
                tmpLabel.Dock = DockStyle.Left;
                tmpLabel.AutoSize = true;
                tmpLabel.Font = new Font("Tahoma", 8.0f, FontStyle.Regular);
                retVal.Controls.Add(tmpLabel);

                //Text Box
                TNumericBox tmpTextBox = new TNumericBox();
                tmpTextBox.Name = function.Name;
                //tmpTextBox.Dock = DockStyle.Right;
                tmpTextBox.Location = new Point(_default_blankSize, 0);
                tmpTextBox.Font = new Font("Tahoma", 8.0f, FontStyle.Regular);
                tmpTextBox.Size = new Size(width - _default_blankSize, _default_Control_height);
                retVal.Controls.Add(tmpTextBox);

                #region Set Context Menu

                if (function.ContextMenu.Length > 0 && this._containers.Contains(function.ContextMenu))
                    tmpTextBox.ContextMenuStrip = this.MakeContextMenu(function.ContextMenu);

                #endregion

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private TPanel AddDateTimePicker(UIFunctionBasicModel function, int width)
        {
            #region Add Text box

            TPanel retVal = null;

            try
            {
                //Panel
                retVal = new TPanel();
                retVal.Size = new Size(width, _default_Panel_height);

                //Label
                TAP.UIControls.BasicControls.TLabel tmpLabel = new TLabel();
                tmpLabel.Text = this._converter.ConvertPhrase(function.Text.ToUpper());
                tmpLabel.Dock = DockStyle.Left;
                tmpLabel.AutoSize = true;
                tmpLabel.Font = new Font("Tahoma", 8.0f, FontStyle.Regular);
                retVal.Controls.Add(tmpLabel);

                //Text Box
                TDateTimePicker tmpTextBox = new TDateTimePicker();
                tmpTextBox.Name = function.Name;
                tmpTextBox.Size = new Size(width - _default_blankSize, _default_Panel_height);
                tmpTextBox.Location = new Point(_default_blankSize, 0);
                //tmpTextBox.Dock = DockStyle.Right;
                tmpTextBox.Font = new Font("Tahoma", 8.0f, FontStyle.Regular);
                tmpTextBox.Size = new Size(width - _default_blankSize, _default_Panel_height);
                retVal.Controls.Add(tmpTextBox);

                #region Set Context Menu

                if (function.ContextMenu.Length > 0 && this._containers.Contains(function.ContextMenu))
                    tmpTextBox.ContextMenuStrip = this.MakeContextMenu(function.ContextMenu);

                #endregion

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private TPanel AddDoubleDateTimePicker(UIFunctionBasicModel function, int width)
        {
            #region Add Text box

            //int tmpWidth = 0;
            TPanel retVal = null;

            try
            {
                //tmpWidth = width * 2;

                //Panel
                retVal = new TPanel();
                retVal.Size = new Size(width, _default_Panel_height);

                //Label
                TAP.UIControls.BasicControls.TLabel tmpLabel = new TLabel();
                tmpLabel.Text = this._converter.ConvertPhrase(function.Text.ToUpper());
                tmpLabel.Dock = DockStyle.Left;
                tmpLabel.AutoSize = true;
                tmpLabel.Font = new Font("Tahoma", 8.0f, FontStyle.Regular);
                retVal.Controls.Add(tmpLabel);

                //Text Box
                TDateTimePickerSE tmpTextBox = new TDateTimePickerSE();
                tmpTextBox.Name = function.Name;
                tmpTextBox.Size = new Size(width - _default_blankSize, _default_Panel_height);
                //tmpTextBox.Location = new Point(_default_blankSize, 0);
                tmpTextBox.Font = new Font("Tahoma", 8.0f, FontStyle.Regular);
                tmpTextBox.Location = new Point(_default_blankSize, 0);
                //tmpTextBox.Dock = DockStyle.Right;
                //tmpTextBox.Size = new Size(tmpWidth - _default_blankSize, _default_Panel_height);
                retVal.Controls.Add(tmpTextBox);

                #region Set Context Menu

                if (function.ContextMenu.Length > 0 && this._containers.Contains(function.ContextMenu))
                    tmpTextBox.ContextMenuStrip = this.MakeContextMenu(function.ContextMenu);

                #endregion

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private System.Windows.Forms.ContextMenuStrip MakeContextMenu(string containerName)
        {
            #region Make Context menu

            TContextMenu tmpMenu = null;
            ContainerBasicModel tmpContainer = null;

            try
            {
                tmpContainer = this._containers[containerName];

                if (!object.Equals(tmpContainer, null))
                {
                    tmpMenu = new TContextMenu();
                    tmpMenu.Name = containerName;

                    string[] tmpFunctionNames = tmpContainer.UIFunctions.CreateSeqenceArray();

                    for (int i = 0; i < tmpFunctionNames.Length; i++)
                    {
                        UIFunctionBasicModel tmpFunction = tmpContainer.UIFunctions[tmpFunctionNames[i]];

                        if (tmpFunction.Name.IndexOf("SPREATOR") < 0)
                        {
                            TMenuItem tmpMenuItem = new TMenuItem();
                            tmpMenuItem.Name = tmpFunction.Name;
                            //tmpMenuItem.ControlID = tmpFunction.Name;
                            tmpMenuItem.Text = _converter.ConvertPhrase(tmpFunction.Text);
                            tmpMenuItem.Click += new EventHandler(ContextMenuItem_Click);

                            tmpMenu.Items.Add((ToolStripMenuItem)tmpMenuItem);
                        }
                        else
                            tmpMenu.Items.Add("-");
                    }

                    tmpMenu.Opening += new System.ComponentModel.CancelEventHandler(ContextMenu_Opening);
                }

                return (System.Windows.Forms.ContextMenuStrip)tmpMenu;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Find Control

        /// <summary>
        /// This method finds control using specified control ID.
        /// </summary>
        /// <param name="controlID">Control ID</param>
        /// <returns>Control</returns>
        protected TAP.UIControls.IUIControl FindControl(string controlID)
        {
            #region Find Control

            try
            {
                foreach (System.Windows.Forms.Control tmpPanel in this.Controls)
                {
                    if (tmpPanel.Name.Equals(controlID))
                        return (TAP.UIControls.IUIControl)tmpPanel;

                    if (tmpPanel.GetType().Equals(typeof(TPanel)))
                    {
                        if (this._containers != null)
                        {
                            #region UI-Automation Case

                            if (this._containers.Contains(tmpPanel.Name))
                            {
                                foreach (TAP.UIControls.IUIControl tmpGroupBox in ((TPanel)tmpPanel).Controls)
                                {
                                    if (tmpGroupBox.GetType().Equals(typeof(TGroupBox)))
                                    {
                                        foreach (TAP.UIControls.IUIControl tmpSubPanel in ((TGroupBox)tmpGroupBox).Controls)
                                        {
                                            TAP.UIControls.IUIControl tmpControl = this.FindControl((TPanel)tmpSubPanel, controlID);

                                            if (!object.Equals(tmpControl, null))
                                                return tmpControl;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //TAP.UIControls.IUIControl tmpControl = this.FindControlOnUmManagedPanel((TPanel)tmpPanel, controlID);

                                //if (!object.Equals(tmpControl, null))
                                //    return tmpControl;
                            }

                            #endregion
                        }
                        else
                        {
                            #region Maual Case

                            TAP.UIControls.IUIControl retVal = null;

                            foreach (TAP.UIControls.IUIControl tmpSubControl in ((TPanel)tmpPanel).Controls)
                            {
                                if (tmpSubControl.GetType().Equals(typeof(TGroupBox)))
                                {
                                    retVal = this.FindControl((TGroupBox)tmpSubControl, controlID);

                                    if (retVal == null)
                                        continue;
                                    else
                                        return retVal;
                                }
                                else if (tmpSubControl.GetType().Equals(typeof(TPanel)))
                                {
                                    retVal = this.FindControl((TPanel)tmpSubControl, controlID);

                                    if (retVal == null)
                                        continue;
                                    else
                                        return retVal;
                                }
                                else if (string.Compare(controlID, tmpSubControl.ControlID, true).Equals(0))
                                    return tmpSubControl;
                            }

                            #endregion
                        }
                    }
                }

                return null;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method finds control in specified panel using specified control ID.
        /// </summary>
        /// <param name="panel">Panel</param>
        /// <param name="controlID">Control ID</param>
        /// <returns>Control</returns>
        private TAP.UIControls.IUIControl FindControlOnUnManagedPanel(TPanel panel, string controlID)
        {
            #region Find Control

            try
            {
                foreach (System.Windows.Forms.Control tmpControls in panel.Controls)
                {
                    if (tmpControls.Name.Equals(controlID))
                        return (TAP.UIControls.IUIControl)tmpControls;

                    if (tmpControls.GetType().Equals(typeof(TPanel)))
                    {
                        if (this._containers != null)
                        {
                            #region UI-Automation Case
                            if (this._containers.Contains(tmpControls.Name))
                            {
                                foreach (TAP.UIControls.IUIControl tmpGroupBox in ((TPanel)tmpControls).Controls)
                                {
                                    if (tmpGroupBox.GetType().Equals(typeof(TGroupBox)))
                                    {
                                        foreach (TAP.UIControls.IUIControl tmpSubPanel in ((TGroupBox)tmpGroupBox).Controls)
                                        {
                                            TAP.UIControls.IUIControl tmpControl = this.FindControl((TPanel)tmpSubPanel, controlID);

                                            if (!object.Equals(tmpControl, null))
                                                return tmpControl;
                                        }
                                    }
                                }
                            }
                            else
                            {
                            }

                            #endregion
                        }
                        else
                        {
                        }
                    }
                }

                return null;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private TAP.UIControls.IUIControl FindControl(TPanel panel, string controlID)
        {
            #region Find Control

            TAP.UIControls.IUIControl retVal = null;

            try
            {
                foreach (TAP.UIControls.IUIControl tmpControl in panel.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TPanel)))
                    {
                        retVal = this.FindControl((TPanel)tmpControl, controlID);

                        if (object.Equals(retVal, null))
                            continue;
                        else
                            return retVal;
                    }
                    else if (tmpControl.GetType().Equals(typeof(TGroupBox)))
                    {
                        retVal = this.FindControl((TGroupBox)tmpControl, controlID);

                        if (object.Equals(retVal, null))
                            continue;
                        else
                            return retVal;
                    }
                    else if (string.Compare(controlID, tmpControl.ControlID, true).Equals(0))
                        return tmpControl;
                }

                return null;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private TAP.UIControls.IUIControl FindControl(TGroupBox panel, string controlID)
        {
            #region Find Control

            TAP.UIControls.IUIControl retVal = null;

            try
            {
                foreach (TAP.UIControls.IUIControl tmpControl in panel.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TPanel)))
                    {
                        retVal = this.FindControl((TPanel)tmpControl, controlID);

                        if (object.Equals(retVal, null))
                            continue;
                        else
                            return retVal;
                    }
                    else if (tmpControl.GetType().Equals(typeof(TGroupBox)))
                    {
                        retVal = this.FindControl((TGroupBox)tmpControl, controlID);

                        if (object.Equals(retVal, null))
                            continue;
                        else
                            return retVal;
                    }
                    else if (string.Compare(controlID, tmpControl.ControlID, true).Equals(0))
                        return tmpControl;
                }

                return null;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #endregion

        #region Default Command

        /// <summary>
        /// This methods closes current UI.
        /// </summary>
        virtual public void Exit()
        {
            try
            {
                this.CancelAsync();

                if (AsyncCallEnd != null)
                    AsyncCallEnd(_asyncMethod);

                this.tabPage.Dispose();

                if (!this.tabControl.HasChildren)
                    this.tabControl.Visible = false;
            }
            catch
            {
            }
        }

        /// <summary>
        /// This method removes value on all controls.
        /// </summary>
        virtual protected void ClearControls()
        {
            #region Clear Controls

            try
            {
                foreach (System.Windows.Forms.Control tmpControl in this.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TPanel))
                        && this._containers.Contains(tmpControl.Name))
                    {
                        //관리 패널인 경우
                        this.ClearControls((TPanel)tmpControl);
                    }
                    else //관리 패널이 아닌경우: 패널내에서 다시 관리 패널을 찾는다. 
                        this.ClearControlsOnUnManagedPanel((TPanel)tmpControl, string.Empty);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method revmoes value on specified panel.
        /// </summary>
        /// <param name="panel">Panel</param>
        protected void ClearControls(TPanel panel)
        {
            #region Clear Controls

            try
            {
                foreach (IUIControl tmpControl in panel.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TGroupBox)))
                    {
                        foreach (IUIControl tmpSubControl in ((TGroupBox)tmpControl).Controls)
                        {
                            if (tmpSubControl.GetType().Equals(typeof(TPanel)))
                                this.ClearControls(panel.Name, (TPanel)tmpSubControl);
                            else
                                this.ClearControl(tmpSubControl);
                        }
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

        /// <summary>
        /// This method revmoes value on specified container.
        /// </summary>
        /// <param name="containerName">Container name</param>
        protected void ClearControls(string containerName)
        {
            #region Clear Controls

            try
            {
                foreach (System.Windows.Forms.Control tmpPanel in this.Controls)
                {
                    if (tmpPanel.GetType().Equals(typeof(TPanel))
                        && this._containers.Contains(tmpPanel.Name))
                    {
                        if (tmpPanel.Name.Equals(containerName))
                        {
                            this.ClearControls((TPanel)tmpPanel);
                            break;
                        }
                    }
                    else if (tmpPanel.GetType().Equals(typeof(TPanel)))
                        this.ClearControlsOnUnManagedPanel((TPanel)tmpPanel, containerName);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method delete value on panel has specified name.
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="panel">Panel</param>
        protected void ClearControls(string containerName, TPanel panel)
        {
            #region Clear Controls

            try
            {
                foreach (TAP.UIControls.IUIControl tmpControl in panel.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TAP.UIControls.BasicControls.TPanel)))
                        this.ClearControls(containerName, (TPanel)tmpControl);
                    else
                    {
                        if (tmpControl.ControlID.Length.Equals(0) ||
                            !this._containers[containerName].UIFunctions.Contains(tmpControl.ControlID))
                        {
                            TAP.UIControls.IUIControl tmpCustomControl = this.FindControl(tmpControl.ControlID);

                            if (tmpCustomControl.GetType().Equals(typeof(TTextBox)))
                                ((TTextBox)tmpCustomControl).Text = "";
                        }
                        else
                        {
                            TAP.Models.UIBasic.EnumUIControlType tmpControlType = this._containers[containerName].UIFunctions[tmpControl.ControlID].ControlType;

                            switch (tmpControlType)
                            {
                                //case EnumUIControlType.CHECKBOX:
                                //    ((TAPChec)tmpControl).SelectedIndex = 0; break;
                                case EnumUIControlType.COMBOBOX:
                                    ((TComboBox)tmpControl).SelectedIndex = 0; break;
                                case EnumUIControlType.NUMBERBOX:
                                    ((TNumericBox)tmpControl).Value = 0; break;
                                //case EnumUIControlType.SHEET:
                                //    ((DefaultSheet)tmpControl).SetRowCount(0); break;
                                case EnumUIControlType.TEXTBOX:
                                    ((TTextBox)tmpControl).Text = string.Empty; break;
                                    //case EnumUIControlType.SQUAREBOARD:
                                    //    ((TAP.Controls.Board.SquareBoard)tmpControl).Clear(); break;
                            }
                        }
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

        private void ClearControlsOnUnManagedPanel(TPanel panel, string panelName)
        {
            #region Clear Controls

            try
            {
                foreach (System.Windows.Forms.Control tmpControl in panel.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TPanel))
                        && this._containers.Contains(tmpControl.Name))
                    {
                        //특정 패널이 지정되지 않은 경우
                        if (panelName.Length.Equals(0))
                            this.ClearControls((TPanel)tmpControl);
                        else
                        {
                            //특정 패널이 지정된 경우
                            if (panelName.Equals(tmpControl.Name))
                            {
                                this.ClearControls((TPanel)tmpControl);
                                break;
                            }
                        }
                    }
                    else
                        this.ClearControlsOnUnManagedPanel((TPanel)tmpControl, panelName);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void ClearControl(IUIControl control)
        {
            #region Clear Control

            try
            {
                if (control.GetType().Equals(typeof(TTextBox)))
                    ((TTextBox)control).Text = "";
                else if (control.GetType().Equals(typeof(TNumericBox)))
                    ((TNumericBox)control).Value = 0;
                else if (control.GetType().Equals(typeof(TCheckBox)))
                    ((TCheckBox)control).Checked = false;
                else if (control.GetType().Equals(typeof(TListBox)))
                    ((TListBox)control).Items.Clear();
                else if (control.GetType().Equals(typeof(TListView)))
                    ((TListView)control).Items.Clear();
                else if (control.GetType().Equals(typeof(TPictureBox)))
                    ((TPictureBox)control).Image = null;
                else if (control.GetType().Equals(typeof(TRichTextBox)))
                    ((TRichTextBox)control).Text = "";

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// This method collects string value of specified control.
        /// </summary>
        /// <param name="controlName">Control name</param>
        /// <returns>String typed value of specified control</returns>
        protected string GetControlStringValue(string controlName)
        {
            #region GetControlStringValue

            string retVal = string.Empty;
            TAP.UIControls.IUIControl tmpControl = null;

            try
            {
                tmpControl = this.FindControl(controlName);

                if (!object.Equals(tmpControl, null))
                    retVal = tmpControl.RepresentativeValue.ToString().Length.Equals(0) ? TAP.Models.Model._ARGUMENTVALUE_ALL : tmpControl.RepresentativeValue.ToString();

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method sets string typed value on specified control.
        /// </summary>
        /// <param name="controlName">Control name</param>
        /// <param name="value">Value</param>
        protected void SetControlValue(string controlName, object value)
        {
            #region GetControlStringValue

            string retVal = string.Empty;
            TAP.UIControls.IUIControl tmpControl = null;

            try
            {
                tmpControl = this.FindControl(controlName);

                if (!object.Equals(tmpControl, null))
                    tmpControl.RepresentativeValue = value;

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        //protected void ConvertDefaultSheetColumns(AdvdSpread spread, int sheetIndex)
        //{
        //    #region Convert DefaultSheet Columns

        //    //string[] tmpNewColumlnHeaderString = null;

        //    //try
        //    //{
        //    //    tmpNewColumlnHeaderString = new string[spread.Sheets[sheetIndex].ColumnCount];

        //    //    for (int i = 0; i < spread.Sheets[sheetIndex].ColumnCount; i++)
        //    //    {
        //    //        spread.Sheets[sheetIndex].ColumnHeader.Cells[0, i].Text = this._converter.ConvertPhrase(spread.Sheets[sheetIndex].ColumnHeader.Cells[0, i].Text);
        //    //    }

        //    //    //sheet.ColulmnHeaderStrings = tmpNewColumlnHeaderString;

        //    //    return;
        //    //}
        //    //catch
        //    //{
        //    //    //throw ex;
        //    //}


        //    #endregion
        //}

        #endregion

        #region Event Handlers

        /// <summary>
        /// This event handler is for button excutes "Exit" command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        virtual protected void ButtonExitEventHandler(object sender, EventArgs e)
        {
            #region TextBoxExtEventHandler

            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.Exit();
            }
            catch (System.Exception ex)
            {
                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.CLOSE, EnumGeneralTemplateType.FAIL, "Application");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMessage, ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            #endregion
        }

        /// <summary>
        /// This event handler is for button excutes "Clear" command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        virtual protected void ButtonClearEventHandler(object sender, EventArgs e)
        {
            #region TextBoxExtEventHandler

            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.ClearControls();
            }
            catch (System.Exception ex)
            {
                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.INITIALIZE, EnumGeneralTemplateType.FAIL, "<Control>");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMessage, ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            #endregion
        }

        /// <summary>
        /// This event handler is for button excutes "Delete" command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        virtual protected void ButtonDeleteEventHandler(object sender, EventArgs e)
        {
            #region TextBoxExtEventHandler

            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.Delete();
            }
            catch (System.Exception ex)
            {
                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.DELETE, EnumGeneralTemplateType.FAIL, "Data");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMessage, ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            #endregion
        }

        /// <summary>
        /// This event handler is for button excutes "Main" command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        virtual protected void ButtonCommandEventHandler(object sender, EventArgs e)
        {
            #region TextBoxExtEventHandler

            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.Command();
            }
            catch (System.Exception ex)
            {
                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.EXECUTE, EnumGeneralTemplateType.FAIL, "User command");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMessage, ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            #endregion
        }

        /// <summary>
        /// This event handler is for button excutes "Search" command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        virtual protected void ButtonSearchEventHandler(object sender, EventArgs e)
        {
            #region TextBoxExtEventHandler

            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.Search();
            }
            catch (System.Exception ex)
            {
                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.SEARCH, EnumGeneralTemplateType.FAIL, "Data");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMessage, ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            #endregion
        }

        /// <summary>
        /// This event handler is for button excutes "Add" command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        virtual protected void ButtonBoxAddEventHandler(object sender, EventArgs e)
        {
            #region TextBoxExtEventHandler

            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.Add();
            }
            catch (System.Exception ex)
            {
                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.ADD, EnumGeneralTemplateType.FAIL, "Data");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMessage, ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            #endregion
        }

        /// <summary>
        /// This event handler is for textbox excutes "Search" command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        virtual protected void TextBoxSearchEventHandler(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            #region TextBoxExtEventHandler

            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (e.KeyValue.Equals(13))
                    this.Search();
            }
            catch (System.Exception ex)
            {
                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.SEARCH, EnumGeneralTemplateType.FAIL, "Data");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMessage, ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            #endregion
        }

        /// <summary>
        /// This event handler is for context menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        virtual protected void ContextMenuItem_Click(object sender, EventArgs e)
        {
            #region TextBoxExtEventHandler

            this.Cursor = Cursors.WaitCursor;

            try
            {
                //TAPMenuItem tmpSelectedMenu = (TAPMenuItem)sender;
                //this.ExecuteContextMenu(tmpSelectedMenu.Name);

                return;
            }
            catch (System.Exception ex)
            {
                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.EXECUTE, EnumGeneralTemplateType.FAIL, "User command");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMessage, ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            #endregion
        }

        /// <summary>
        /// This event will be excuted when context menu is open.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        virtual protected void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            #region TextBoxExtEventHandler

            this.Cursor = Cursors.WaitCursor;

            try
            {
                //TAPMenuItem tmpSelectedMenu = (ContextMenuStrip)sender;
                this.OpenContextMenu((ContextMenuStrip)sender);

                return;
            }
            catch (System.Exception ex)
            {
                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.OPEN, EnumGeneralTemplateType.FAIL, "<Context Menu>");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMessage, ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            #endregion
        }

        private void tButtonStop_Click(object sender, EventArgs e)
        {
            #region tButtonStop_Click

            try
            {
                CancelAsync();

                if (AsyncCallEnd != null)
                    AsyncCallEnd(_asyncMethod);
            }
            catch
            {
            }

            #endregion
        }

        private void UIBase_Activated(object sender, EventArgs e)
        {
            #region Code

            //try
            //{
            //    this.tabControl.SelectedTab = this.tabPage;

            //    if (!this.tabControl.Visible)
            //        this.tabControl.Visible = true;
            //}
            //catch
            //{

            //}

            #endregion
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            #region Code

            try
            {
                this.Close();
                this.Exit();
            }
            catch (System.Exception ex)
            {
                string tmpMsg = _translator.ConvertGeneralTemplate(Fressage.EnumVerbs.CLOSE, Fressage.EnumGeneralTemplateType.FAIL, "UI");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMsg, ex.ToString());
            }

            #endregion
        }

        private void pic_Click(object sender, EventArgs e)
        {
            #region Code

            try
            {
                this.ExportsViaExcel();
            }
            catch (System.Exception ex)
            {
                string tmpMsg = _translator.ConvertGeneralTemplate(Fressage.EnumVerbs.EXECUTE, Fressage.EnumGeneralTemplateType.FAIL, "User Command");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMsg, ex.ToString());
            }

            #endregion
        }
        private void picBookMark_Click(object sender, EventArgs e)
        {
            #region Code

            try
            {
                if (this._assign)
                    this.BookMarkAssign = false;
                else
                    this.BookMarkAssign = true;

                SetBookMarkHandler(this.BookMarkAssign, this.UIInformation);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// Current Time Update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TimerStart(object sender, EventArgs e)
        {
            #region Current Time Update

            try
            {
                string time = DateTime.Now.ToString("HH:mm:ss");
                this.tLabelTime.Text = time;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Async

        /// <summary>
        /// This method starts asynchronous working
        /// </summary>
        /// <param name="asyncMethod">Asynchronous method</param>
        /// <param name="callBackMethod">Callback method</param>
        protected void BeginAsyncCall(string asyncMethod, string callBackMethod)
        {
            BeginAsyncCall(asyncMethod, callBackMethod, EnumDataObject.MODELSET, null);
        }

        /// <summary>
        /// This method starts asynchronous working
        /// </summary>
        /// <param name="asyncMethod">Asynchronous method</param>
        /// <param name="callBackMethod">Callback method</param>
        /// <param name="dataObject">Object type</param>
        protected void BeginAsyncCall(string asyncMethod, string callBackMethod, EnumDataObject dataObject)
        {
            BeginAsyncCall(asyncMethod, callBackMethod, dataObject, null);
        }

        /// <summary>
        /// This method starts asynchronous working
        /// </summary>
        /// <param name="asyncMethod">Asynchronous method</param>
        /// <param name="callBackMethod">Callback method</param>
        /// <param name="dataObject">Object type</param>
        /// <param name="parameters">Parameters</param>
        protected void BeginAsyncCall(string asyncMethod, string callBackMethod, EnumDataObject dataObject, params object[] parameters)
        {
            #region Begin AsyncCall

            this._dataObject = dataObject;

            if (!this._isBusy)
            {
                this._asyncParameters = parameters;
                this._isBackGroundCall = true;
                this._asyncMethod = asyncMethod;
                this._callBackMethod = callBackMethod;

                #region Store Data for Undo

                //Store data for Undo
                //if (_1ststepParamvalues != string.Empty && _1ststepStorevalues != string.Empty)
                //{
                //    _2ndstepParamvalues = _1ststepParamvalues;
                //    _2ndstepStorevalues = _1ststepStorevalues;
                //}

                //List<IParamAvailable> lstParams
                //    = ((IParamAvailableControlAgent)_frmAgent.ControlAgents[typeof(IParamAvailable)])._paramControls;
                //Dictionary<string, object> dicParams = new Dictionary<string, object>();
                //foreach (IParamAvailable prmt in lstParams)
                //{
                //    if (prmt.ParamName != string.Empty)
                //    {
                //        string key = prmt.SubParamName != string.Empty
                //            ? string.Format("{0}.{1}", prmt.ParamName, prmt.SubParamName) : prmt.ParamName;

                //        if (!dicParams.ContainsKey(key))
                //            dicParams.Add(key, prmt.ParamValue);
                //    }
                //}

                //_1ststepParamvalues = Utility.ToXMLString(dicParams);
                //_1ststepStorevalues = Utility.ToXMLString(_frmAgent.GetStoreControlValues());

                #endregion

                //Show progress
                Thread tmpThread = new Thread(new ThreadStart(this.StartProgress));
                tmpThread.Start();

                //Set process time
                this._processStartTime = DateTime.Now;

                string tmpExecuteMsg = tmpExecuteMsg = _translator.ConvertGeneralTemplate(EnumVerbs.EXECUTE, EnumGeneralTemplateType.ING, "Command");
                this.SetWorkStatus(EnumMsgType.INFORMATION, tmpExecuteMsg);

                this._isBusy = true;

                this._workThread = new Thread(new ThreadStart(BeginTask));
                this._workThread.IsBackground = true;
                this._workThread.Start();
            }

            #endregion
        }

        private Type _serviceType = null;
        private object _serviceObj = null;
        public void BeginAsyncCallByType(string asyncMethod, string callBackMethod, EnumDataObject dataObject, Type serviceType, object obj, params object[] parameters)
        {
            #region Begin AsyncCall

            this._dataObject = dataObject;

            if (!this._isBusy)
            {
                this._asyncParameters = parameters;
                this._isBackGroundCall = true;
                this._asyncMethod = asyncMethod;
                this._callBackMethod = callBackMethod;

                #region Store Data for Undo

                //Store data for Undo
                //if (_1ststepParamvalues != string.Empty && _1ststepStorevalues != string.Empty)
                //{
                //    _2ndstepParamvalues = _1ststepParamvalues;
                //    _2ndstepStorevalues = _1ststepStorevalues;
                //}

                //List<IParamAvailable> lstParams
                //    = ((IParamAvailableControlAgent)_frmAgent.ControlAgents[typeof(IParamAvailable)])._paramControls;
                //Dictionary<string, object> dicParams = new Dictionary<string, object>();
                //foreach (IParamAvailable prmt in lstParams)
                //{
                //    if (prmt.ParamName != string.Empty)
                //    {
                //        string key = prmt.SubParamName != string.Empty
                //            ? string.Format("{0}.{1}", prmt.ParamName, prmt.SubParamName) : prmt.ParamName;

                //        if (!dicParams.ContainsKey(key))
                //            dicParams.Add(key, prmt.ParamValue);
                //    }
                //}

                //_1ststepParamvalues = Utility.ToXMLString(dicParams);
                //_1ststepStorevalues = Utility.ToXMLString(_frmAgent.GetStoreControlValues());

                #endregion

                //Show progress
                Thread tmpThread = new Thread(new ThreadStart(this.StartProgress));
                tmpThread.Start();

                //Set process time
                this._processStartTime = DateTime.Now;

                string tmpExecuteMsg = tmpExecuteMsg = _translator.ConvertGeneralTemplate(EnumVerbs.EXECUTE, EnumGeneralTemplateType.ING, "Command");
                this.SetWorkStatus(EnumMsgType.INFORMATION, tmpExecuteMsg);

                this._isBusy = true;
                ServiceType = serviceType;
                ServiceObj = obj;
                this._workThread = new Thread(new ThreadStart(BeginTaskByType));
                this._workThread.IsBackground = true;
                this._workThread.Start();
            }

            #endregion
        }

        private void BeginTask()
        {
            #region Begin Task

            MethodInfo tmpMethod = null;

            try
            {
                tmpMethod = this.GetType().GetMethod(_asyncMethod);

                if (tmpMethod == null)
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.FIND, EnumGeneralTemplateType.CANNOT, "<" + _asyncMethod + ">");
                    throw new Exception(string.Format(tmpMsg, _asyncMethod));
                }

                //_asyncEnd = false;

                switch (this._dataObject)
                {
                    case EnumDataObject.MODELSET: this._modelSet = (Models.ModelSet)tmpMethod.Invoke(this, _asyncParameters); break;
                    case EnumDataObject.MODELLIST: this._modelList = (List<Models.Model>)tmpMethod.Invoke(this, _asyncParameters); break;
                    case EnumDataObject.DATASET: this._dataSet = (DataSet)tmpMethod.Invoke(this, _asyncParameters); break;
                    case EnumDataObject.STRING: this._resultString = (string)tmpMethod.Invoke(this, _asyncParameters); break;
                    case EnumDataObject.NONE: tmpMethod.Invoke(this, _asyncParameters); break;
                }

                this._asyncParameters = null;

                this.SetUIData();
                // _asyncEnd = true;
            }
            catch (ThreadAbortException te)
            {
                string tmpTe = te.ToString();
            }
            catch (System.Exception ex)
            {
                this.EndProgressBar();
                //this._asyncEnd = true;

                string tmpErrorMsg = _translator.ConvertGeneralTemplate(EnumVerbs.EXECUTE, EnumGeneralTemplateType.FAIL, "Command");

                this.SetWorkStatus(EnumMsgType.INFORMATION, tmpErrorMsg);
                MessageBox.Show(ex.ToString());
            }

            #endregion
        }

        private void BeginTaskByType()
        {
            #region Begin Task

            MethodInfo tmpMethod = null;

            try
            {
                tmpMethod = ServiceType.GetMethod(_asyncMethod);

                if (tmpMethod == null)
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.FIND, EnumGeneralTemplateType.CANNOT, "<" + _asyncMethod + ">");
                    throw new Exception(string.Format(tmpMsg, _asyncMethod));
                }

                //_asyncEnd = false;

                switch (this._dataObject)
                {
                    case EnumDataObject.MODELSET: this._modelSet = (Models.ModelSet)tmpMethod.Invoke(ServiceObj, _asyncParameters); break;
                    case EnumDataObject.MODELLIST: this._modelList = (List<Models.Model>)tmpMethod.Invoke(ServiceObj, _asyncParameters); break;
                    case EnumDataObject.DATASET: this._dataSet = (DataSet)tmpMethod.Invoke(ServiceObj, _asyncParameters); break;
                    case EnumDataObject.STRING: this._resultString = (string)tmpMethod.Invoke(ServiceObj, _asyncParameters); break;
                    case EnumDataObject.NONE: tmpMethod.Invoke(ServiceObj, _asyncParameters); break;
                }

                this._asyncParameters = null;

                this.SetUIDataByType();
                // _asyncEnd = true;
            }
            catch (ThreadAbortException te)
            {
                this._isBusy = false;

                string tmpTe = te.ToString();
            }
            catch (System.Exception ex)
            {
                this._isBusy = false;

                this.EndProgressBar();
                //this._asyncEnd = true;

                string tmpErrorMsg = _translator.ConvertGeneralTemplate(EnumVerbs.EXECUTE, EnumGeneralTemplateType.FAIL, "Command");

                this.SetWorkStatus(EnumMsgType.INFORMATION, tmpErrorMsg);
                MessageBox.Show(ex.ToString());
            }

            #endregion
        }

        private void SetUIData()
        {
            #region Set UI Data

            string tmpResultCount = string.Empty;
            string tmpResultTime = string.Empty;
            string tmpResultText = string.Empty;

            if (this.InvokeRequired)
                this.Invoke(new SetUIDelegate(SetUIData), null);
            else
            {
                try
                {
                    MethodInfo tmpMethod = this.GetType().GetMethod(_callBackMethod);

                    #region Ending Async

                    if (tmpMethod == null)
                    {
                        string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.FIND, EnumGeneralTemplateType.CANNOT, "<" + _callBackMethod + ">");
                        throw new Exception(string.Format(tmpMsg, _callBackMethod));
                    }

                    string tmpElapsedTime = Base.DateTimes.DateTimeBase.Instance.GetTimeLagByString(this._processStartTime, DateTime.Now);

                    if (TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.UILogging == true)
                        this.SaveUIUsageLog();

                    this._processStartTime = DateTime.MinValue;

                    tmpResultCount = " {0:#,##0} objects";
                    tmpResultTime = string.Format(" Elapsed Time: {0}", tmpElapsedTime);

                    switch (this._dataObject)
                    {
                        case EnumDataObject.MODELSET:
                            tmpResultCount = _modelSet != null && _modelSet.Count > 0 ? string.Format(tmpResultCount, _modelSet.Count) : string.Empty;
                            tmpResultText = _translator.ConvertGeneralTemplate(EnumVerbs.LOAD, EnumGeneralTemplateType.ED, tmpResultCount) + "/"; break;
                        case EnumDataObject.MODELLIST:
                            tmpResultCount = _modelList != null && _modelList.Count > 0 ? string.Format(tmpResultCount, _modelList.Count) : string.Empty;
                            tmpResultText = _translator.ConvertGeneralTemplate(EnumVerbs.LOAD, EnumGeneralTemplateType.ED, tmpResultCount) + "/"; break;
                        case EnumDataObject.DATASET:
                            tmpResultCount = _dataSet != null && _dataSet.Tables[0].Rows.Count > 0 ? string.Format(tmpResultCount, _dataSet.Tables[0].Rows.Count) : string.Empty;
                            tmpResultText = _translator.ConvertGeneralTemplate(EnumVerbs.LOAD, EnumGeneralTemplateType.ED, tmpResultCount) + "/"; break;
                        case EnumDataObject.STRING:
                            tmpResultCount = _resultString != null && _resultString.Length > 0 ? string.Format(tmpResultCount, _resultString.Length) : string.Empty;
                            tmpResultText = _translator.ConvertGeneralTemplate(EnumVerbs.LOAD, EnumGeneralTemplateType.ED, tmpResultCount) + "/"; break;
                        case EnumDataObject.NONE:
                            tmpResultText = string.Format(" ElapsedTime : {0}", tmpElapsedTime);
                            break;
                    }

                    tmpResultText = tmpResultCount;
                    tmpResultText += ", ";
                    tmpResultText += _converter.ConvertPhrase(tmpResultTime);

                    #endregion

                    //Call Displaying Method
                    switch (this._dataObject)
                    {
                        case EnumDataObject.MODELSET: tmpMethod.Invoke(this, new object[] { this._modelSet }); break;
                        case EnumDataObject.MODELLIST: tmpMethod.Invoke(this, new object[] { this._modelList }); break;
                        case EnumDataObject.DATASET: tmpMethod.Invoke(this, new object[] { this._dataSet }); break;
                        case EnumDataObject.STRING: tmpMethod.Invoke(this, new object[] { this._resultString }); break;
                        case EnumDataObject.NONE: tmpMethod.Invoke(this, null); break;
                    }

                    this.SetWorkStatus(EnumMsgType.INFORMATION, tmpResultText);

                    this._isBusy = false;

                    #region For Future

                    #region ### Update Default values ###
                    // KWON: DB에 있는 Default항목 정의에 따라 변경되도록(즉 코드 수정작업을 최소화도록) 수정할 것
                    // SetDefaultValues - 실행이 끝난 다음 지정된 값을 Default로 변경작업 처리한다.
                    //if (DefaultOptions.Options != null)
                    //{
                    //    List<IParamAvailable> list
                    //        = ((IParamAvailableControlAgent)_frmAgent.ControlAgents[typeof(IParamAvailable)])._paramControls;
                    //    for (int i = 0; i < list.Count; i++)
                    //    {
                    //        if (DefaultOptions.Contains(list[i].ParamName))
                    //        {
                    //            Linkage.LinkageAgent.DefaultSet(list[i].ParamName, list[i].ParamValue);
                    //        }
                    //    }
                    //}
                    #endregion

                    // Chart생성을 위한 Mode인 경우 Chart를 여기서 생성한다.
                    //if (_isServiceMode)
                    //{
                    //    if (_ds != null && _ds.Tables.Count > 0)
                    //    {
                    //        ((IChartControlAgent)_frmAgent.ControlAgents[typeof(IChart)]).Save(_chartSize.Width, _chartSize.Height, _chartFileName);
                    //    }
                    //}

                    #endregion

                    return;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    this._modelSet = null;
                    this._modelList = null;
                    this._dataSet = null;
                    this.EndProgressBar();
                    this._isBackGroundCall = false;
                }
            }

            #endregion
        }

        private void SetUIDataByType()
        {
            #region Set UI Data

            string tmpResultCount = string.Empty;
            string tmpResultTime = string.Empty;
            string tmpResultText = string.Empty;

            if (this.InvokeRequired)
                this.Invoke(new SetUIDelegate(SetUIDataByType), null);
            else
            {
                try
                {
                    MethodInfo tmpMethod = ServiceType.GetMethod(_callBackMethod);

                    #region Ending Async

                    if (tmpMethod == null)
                    {
                        string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.FIND, EnumGeneralTemplateType.CANNOT, "<" + _callBackMethod + ">");
                        throw new Exception(string.Format(tmpMsg, _callBackMethod));
                    }

                    string tmpElapsedTime = Base.DateTimes.DateTimeBase.Instance.GetTimeLagByString(this._processStartTime, DateTime.Now);

                    if (TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.UILogging == true)
                        this.SaveUIUsageLog();

                    this._processStartTime = DateTime.MinValue;

                    tmpResultCount = " {0:#,##0} objects";
                    tmpResultTime = string.Format(" Elapsed Time: {0}", tmpElapsedTime);

                    switch (this._dataObject)
                    {
                        case EnumDataObject.MODELSET:
                            tmpResultCount = _modelSet != null && _modelSet.Count > 0 ? string.Format(tmpResultCount, _modelSet.Count) : string.Empty;
                            tmpResultText = _translator.ConvertGeneralTemplate(EnumVerbs.LOAD, EnumGeneralTemplateType.ED, tmpResultCount) + "/"; break;
                        case EnumDataObject.MODELLIST:
                            tmpResultCount = _modelList != null && _modelList.Count > 0 ? string.Format(tmpResultCount, _modelList.Count) : string.Empty;
                            tmpResultText = _translator.ConvertGeneralTemplate(EnumVerbs.LOAD, EnumGeneralTemplateType.ED, tmpResultCount) + "/"; break;
                        case EnumDataObject.DATASET:
                            tmpResultCount = _dataSet != null && _dataSet.Tables[0].Rows.Count > 0 ? string.Format(tmpResultCount, _dataSet.Tables[0].Rows.Count) : string.Empty;
                            tmpResultText = _translator.ConvertGeneralTemplate(EnumVerbs.LOAD, EnumGeneralTemplateType.ED, tmpResultCount) + "/"; break;
                        case EnumDataObject.STRING:
                            tmpResultCount = _resultString != null && _resultString.Length > 0 ? string.Format(tmpResultCount, _resultString.Length) : string.Empty;
                            tmpResultText = _translator.ConvertGeneralTemplate(EnumVerbs.LOAD, EnumGeneralTemplateType.ED, tmpResultCount) + "/"; break;
                        case EnumDataObject.NONE:
                            tmpResultText = string.Format(" ElapsedTime : {0}", tmpElapsedTime);
                            break;
                    }

                    tmpResultText = tmpResultCount;
                    tmpResultText += ", ";
                    tmpResultText += _converter.ConvertPhrase(tmpResultTime);

                    #endregion

                    //Call Displaying Method
                    switch (this._dataObject)
                    {
                        case EnumDataObject.MODELSET: tmpMethod.Invoke(ServiceObj, new object[] { this._modelSet }); break;
                        case EnumDataObject.MODELLIST: tmpMethod.Invoke(ServiceObj, new object[] { this._modelList }); break;
                        case EnumDataObject.DATASET: tmpMethod.Invoke(ServiceObj, new object[] { this._dataSet }); break;
                        case EnumDataObject.STRING: tmpMethod.Invoke(ServiceObj, new object[] { this._resultString }); break;
                        case EnumDataObject.NONE: tmpMethod.Invoke(ServiceObj, null); break;
                    }

                    this.SetWorkStatus(EnumMsgType.INFORMATION, tmpResultText);

                    this._isBusy = false;

                    #region For Future

                    #region ### Update Default values ###
                    // KWON: DB에 있는 Default항목 정의에 따라 변경되도록(즉 코드 수정작업을 최소화도록) 수정할 것
                    // SetDefaultValues - 실행이 끝난 다음 지정된 값을 Default로 변경작업 처리한다.
                    //if (DefaultOptions.Options != null)
                    //{
                    //    List<IParamAvailable> list
                    //        = ((IParamAvailableControlAgent)_frmAgent.ControlAgents[typeof(IParamAvailable)])._paramControls;
                    //    for (int i = 0; i < list.Count; i++)
                    //    {
                    //        if (DefaultOptions.Contains(list[i].ParamName))
                    //        {
                    //            Linkage.LinkageAgent.DefaultSet(list[i].ParamName, list[i].ParamValue);
                    //        }
                    //    }
                    //}
                    #endregion

                    // Chart생성을 위한 Mode인 경우 Chart를 여기서 생성한다.
                    //if (_isServiceMode)
                    //{
                    //    if (_ds != null && _ds.Tables.Count > 0)
                    //    {
                    //        ((IChartControlAgent)_frmAgent.ControlAgents[typeof(IChart)]).Save(_chartSize.Width, _chartSize.Height, _chartFileName);
                    //    }
                    //}

                    #endregion

                    return;
                }
                catch (System.Exception ex)
                {
                    this._isBusy = false;
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    this._modelSet = null;
                    this._modelList = null;
                    this._dataSet = null;
                    this.EndProgressBar();
                    this._isBackGroundCall = false;
                }
            }

            #endregion
        }

        /// <summary>
        /// This method cancels async job.
        /// </summary>
        public void CancelAsync()
        {
            #region CancelAsync

            try
            {
                if (_workThread != null && _workThread.IsAlive)
                {
                    // 작업 스레드 취소
                    _workThread.Abort();

                    this._isBusy = false;

                    // 비동기 작업이 종료되었음을 표시합니다.
                    _isBackGroundCall = false;

                    EndProgressBar();

                    // 서버에 작업 취소를 요청한다.
                    CallCancelService();

                    // State Panel 표시
                    string tmpStsMsg = _translator.ConvertGeneralTemplate(EnumVerbs.CANCEL, EnumGeneralTemplateType.ED, "Command");
                    SetWorkStatus(EnumMsgType.INFORMATION, tmpStsMsg);

                    // 커서 초기화 처리
                    if (this.UIInformation != null)
                        ServiceAdapter_EndExecute(this.UIInformation.Name, "");

                    // Cancel 이벤트 호출
                    if (AsyncCallCancel != null)
                        AsyncCallCancel(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage("Error", EnumMsgType.ERROR, ex.ToString());
            }
            finally
            {
            }

            #endregion
        }

        #endregion

        #region For Progress

        /// <summary>
        /// This method starts progress bar
        /// </summary>
        public void StartProgress()
        {
            #region Start Progress

            this._showProgress = true;
            this._isBackGroundCall = true;

            Thread tmpThread = new Thread(new ThreadStart(this.StartProgressBar));
            tmpThread.Start();

            #endregion
        }

        /// <summary>
        /// This method ends progress bar
        /// </summary>
        public void EndProgress()
        {
            #region EndProgress

            _showProgress = false;
            _isBackGroundCall = false;

            this.EndProgressBar();

            #endregion
        }

        /// <summary>
        /// This method displays status message
        /// </summary>
        /// <param name="msgType">Message type</param>
        /// <param name="message">Message</param>
        public void SetWorkStatus(EnumMsgType msgType, string message)
        {
            #region Set Work Status

            if (!this.tLabelStatus.IsDisposed)
            {
                if (this.tLabelStatus.InvokeRequired)
                    this.Invoke(new SetWorkStatusHandler(this.SetWorkStatus), msgType, message);
                else
                    this.tLabelStatus.Text = message;
            }

            #endregion
        }

        /// <summary>
        /// This method refrehs UI Status
        /// </summary>
        /// <param name="message">Message</param>
        public void RefreshUIStatus(string message)
        {
            #region RefreshUIStatus

            this.SetWorkStatus(EnumMsgType.INFORMATION, message);

            if (RefreshUIStatusHandler != null)
                this.RefreshUIStatusHandler(message);

            #endregion
        }

        private void SetProgressValue(bool visible, int value)
        {
            #region Set Progress Value

            try
            {
                if (this.progressBar1.InvokeRequired)
                    this.Invoke(new SetProgress(SetProgressValue), new object[] { visible, value });
                else
                {
                    //this.progressBar1.PerformStep();
                    this.progressBar1.Value = value;
                    this.progressBar1.Visible = true;
                }
            }
            catch
            {
                //
            }

            #endregion
        }

        private void StartProgressBar()
        {
            #region Set Progress

            lock (_lockObject)
            {
                if (!this.progressBar1.IsDisposed)
                {
                    if (this._showProgress && this._isBackGroundCall)
                    {
                        this._continue = true;

                        this.SetProgressValue(true, 50);

                        while (_continue)
                        {
                            for (int i = 10; i < 100; i = i + 3)
                            {
                                if (this.progressBar1.IsDisposed)
                                {
                                    this._continue = false;
                                    return;
                                }
                                else
                                {
                                    this.SetProgressValue(true, i);

                                    if (!this._continue)
                                        break;

                                    Thread.Sleep(50);
                                }
                            }
                        }

                        this.SetProgressValue(false, 100);
                    }
                }
                else
                    return;
            }

            #endregion
        }

        private void EndProgressBar()
        {
            this._continue = false;
        }


        #endregion

        #region UI Usage Log

        private void SaveUIUsageLog()
        {
            //로그는 컨트롤 단위로 기록해야 하기 때문에
            //컨트롤이 클릭되었을 때, 해당 컨트롤을 어떻게 찾을 것인가에 대한 방법이 필요
            #region Save UI Usage Log

            //UIDefaultInfo tmpDefultInfo = null;
            //UIFunctionModel tmpModel = null;

            try
            {

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Commnunication with server

        //서버에 작업 취소를 요청
        private void CallCancelService()
        {
            #region CallCancelService

            //나중에 추가하자
            //if (_serviceUrl != "")
            //{
            //    ServiceAdapter cancelAdapter = new ServiceAdapter();
            //    RequestMessage cancelMsg = new RequestMessage();
            //    cancelMsg.ServiceRequester.UserId = UserInfo.UserId;
            //    cancelMsg.ServiceRequester.UserIP = UserInfo.Ip;
            //    cancelMsg.CancelId = string.Format("{0}_{1}", UserInfo.Ip, _instanceId);
            //    cancelMsg.AddBizCompService("", "", ""); // Add Dummy Service

            //    cancelAdapter.Cancel(_serviceUrl, cancelMsg);
            //}

            #endregion
        }

        private void ServiceAdapter_EndExecute(string instanceID, string serviceUrl)
        {
            if (this.UIInformation.Name.Equals(instanceID))
            {
                //_serviceUrl = ""; // clear
                //_adapterBusy = false;
                SetCursor();
            }
        }

        //private bool _adapterBusy = false;

        private void SetCursor()
        {
            //if (this.InvokeRequired)
            //{
            //    this.Invoke(new SetUIDelegate(SetCursor), null);
            //}
            //else
            //{
            //    this.Cursor = _adapterBusy ? Cursors.WaitCursor : Cursors.Default;
            //    this.pnlCondition.Cursor = _adapterBusy ? Cursors.WaitCursor : Cursors.Default;
            //}
        }

        #endregion

        #region Validation

        /// <summary>
        /// This method validate user's input 
        /// </summary>
        /// <param name="panel">Panel</param>
        /// <returns>If 'TRUE', user's input is valid.</returns>
        protected bool ValidateUserInput(TPanel panel)
        {
            #region Validation

            int tmpErrorCount = 0;

            try
            {
                foreach (IUIControl tmpControl in panel.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TGroupBox)))
                    {
                        tmpErrorCount = this.ValidateUserInputForGroupBox((TGroupBox)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TTitledGroupBox)))
                    {
                        tmpErrorCount = this.ValidateUserInputForTitledGroupBox((TTitledGroupBox)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TPanel)))
                    {
                        tmpErrorCount = this.ValidateUserInputForPanel((TPanel)tmpControl);
                    }
                    else
                        tmpErrorCount += ValidateUserInput(tmpControl);
                }

                if (tmpErrorCount > 0)
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.INPUT, EnumGeneralTemplateType.ORDER, "All required options");
                    TAPMsgBox.Instance.ShowMessage("Warning", EnumMsgType.WARNING, tmpMsg);
                    return false;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method validate user's input 
        /// </summary>
        /// <param name="groupBox">Group box</param>
        /// <returns>If 'TRUE', user's input is valid.</returns>
        protected bool ValidateUserInput(TGroupBox groupBox)
        {
            #region Validation

            int tmpErrorCount = 0;

            try
            {
                foreach (IUIControl tmpControl in groupBox.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TGroupBox)))
                    {
                        tmpErrorCount = this.ValidateUserInputForGroupBox((TGroupBox)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TTitledGroupBox)))
                    {
                        tmpErrorCount = this.ValidateUserInputForTitledGroupBox((TTitledGroupBox)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TPanel)))
                    {
                        tmpErrorCount = this.ValidateUserInputForPanel((TPanel)tmpControl);
                    }
                    else
                        tmpErrorCount += ValidateUserInput(tmpControl);
                }

                if (tmpErrorCount > 0)
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.INPUT, EnumGeneralTemplateType.ORDER, "All required options");
                    TAPMsgBox.Instance.ShowMessage("Warning", EnumMsgType.WARNING, tmpMsg);
                    return false;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method validate user's input 
        /// </summary>
        /// <param name="titledGroupBox">Group box</param>
        /// <returns>If 'TRUE', user's input is valid.</returns>
        protected bool ValidateUserInput(TTitledGroupBox titledGroupBox)
        {
            #region Validation

            int tmpErrorCount = 0;

            try
            {
                foreach (IUIControl tmpControl in titledGroupBox.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TGroupBox)))
                    {
                        tmpErrorCount = this.ValidateUserInputForGroupBox((TGroupBox)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TTitledGroupBox)))
                    {
                        tmpErrorCount = this.ValidateUserInputForTitledGroupBox((TTitledGroupBox)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TPanel)))
                    {
                        tmpErrorCount = this.ValidateUserInputForPanel((TPanel)tmpControl);
                    }
                    else
                        tmpErrorCount += ValidateUserInput(tmpControl);
                }

                if (tmpErrorCount > 0)
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.INPUT, EnumGeneralTemplateType.ORDER, "All required options");
                    TAPMsgBox.Instance.ShowMessage("Warning", EnumMsgType.WARNING, tmpMsg);
                    return false;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method validate user's input 
        /// </summary>
        /// <param name="titledGroupBox">Group box</param>
        /// <returns>If 'TRUE', user's input is valid.</returns>
        protected bool ValidateUserInput(LayoutControl layoutControl)
        {
            #region Validation

            int tmpErrorCount = 0;

            try
            {
                foreach (Control tmpControl in layoutControl.Controls)
                {
                    if (tmpControl is TAP.UIControls.IUIControl)
                    {
                        if (tmpControl.GetType().Equals(typeof(TAP.UIControls.BasicControlsDEV.TCheckComboBox)))
                        {
                            tmpErrorCount += ValidateUserInput((TAP.UIControls.IUIControl)tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(TAP.UIControls.BasicControlsDEV.TComboBox)))
                        {
                            tmpErrorCount += ValidateUserInput((TAP.UIControls.IUIControl)tmpControl);
                        }
                    }
                }

                if (tmpErrorCount > 0)
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.INPUT, EnumGeneralTemplateType.ORDER, "All required options");
                    TAPMsgBox.Instance.ShowMessage("Warning", EnumMsgType.WARNING, tmpMsg);
                    return false;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        protected bool ValidateUserInput(TAP.UIControls.BasicControlsDEV.TGroupBox groupBox)
        {
            #region Validation

            int tmpErrorCount = 0;

            try
            {
                foreach (Control tmpControl in groupBox.Controls)
                {
                    if (tmpControl is TAP.UIControls.IUIControl)
                    {
                        if (tmpControl.GetType().Equals(typeof(TAP.UIControls.BasicControlsDEV.TCheckComboBox)))
                        {
                            tmpErrorCount += ValidateUserInput((TAP.UIControls.IUIControl)tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(TAP.UIControls.BasicControlsDEV.TComboBox)))
                        {
                            tmpErrorCount += ValidateUserInput((TAP.UIControls.IUIControl)tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(TAP.UIControls.BasicControlsDEV.TTextBox)))
                        {
                            tmpErrorCount += ValidateUserInput((TAP.UIControls.IUIControl)tmpControl);
                        }
                    }
                }

                if (tmpErrorCount > 0)
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.INPUT, EnumGeneralTemplateType.ORDER, "All required options");
                    TAPMsgBox.Instance.ShowMessage("Warning", EnumMsgType.WARNING, tmpMsg);
                    return false;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }


        protected bool ValidateUserInput(TAP.UIControls.BasicControlsDEV.TPanel tpanel)
        {
            #region Validation

            int tmpErrorCount = 0;

            try
            {
                foreach (Control tmpControl in tpanel.Controls)
                {
                    if (tmpControl is TAP.UIControls.IUIControl)
                    {
                        if (tmpControl.GetType().Equals(typeof(TAP.UIControls.BasicControlsDEV.TCheckComboBox)))
                        {
                            tmpErrorCount += ValidateUserInput((TAP.UIControls.IUIControl)tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(TAP.UIControls.BasicControlsDEV.TComboBox)))
                        {
                            tmpErrorCount += ValidateUserInput((TAP.UIControls.IUIControl)tmpControl);
                        }
                        else if (tmpControl.GetType().Equals(typeof(TAP.UIControls.BasicControlsDEV.TTextBox)))
                        {
                            tmpErrorCount += ValidateUserInput((TAP.UIControls.IUIControl)tmpControl);
                        }
                    }
                }

                if (tmpErrorCount > 0)
                {
                    string tmpMsg = _translator.ConvertGeneralTemplate(EnumVerbs.INPUT, EnumGeneralTemplateType.ORDER, "All required options");
                    TAPMsgBox.Instance.ShowMessage("Warning", EnumMsgType.WARNING, tmpMsg);
                    return false;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private int ValidateUserInputForGroupBox(TGroupBox groupBox)
        {
            #region ValidateUserInputForGroupBox

            int retVal = 0;

            try
            {
                foreach (IUIControl tmpControl in groupBox.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TPanel)))
                    {
                        retVal = this.ValidateUserInputForPanel((TPanel)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TTitledGroupBox)))
                    {
                        retVal = this.ValidateUserInputForTitledGroupBox((TTitledGroupBox)tmpControl);
                    }
                    else
                        retVal += ValidateUserInput(tmpControl);
                }

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private int ValidateUserInputForTitledGroupBox(TTitledGroupBox groupBox)
        {
            #region ValidateUserInputForGroupBox

            int retVal = 0;

            try
            {
                foreach (IUIControl tmpControl in groupBox.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TPanel)))
                    {
                        retVal = this.ValidateUserInputForPanel((TPanel)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TGroupBox)))
                    {
                        retVal = this.ValidateUserInputForGroupBox((TGroupBox)tmpControl);
                    }
                    else
                        retVal += ValidateUserInput(tmpControl);
                }

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private int ValidateUserInputForPanel(TPanel panel)
        {
            #region Validation

            int retVal = 0;

            try
            {
                foreach (IUIControl tmpControl in panel.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TGroupBox)))
                    {
                        retVal = this.ValidateUserInputForGroupBox((TGroupBox)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TTitledGroupBox)))
                    {
                        retVal = this.ValidateUserInputForTitledGroupBox((TTitledGroupBox)tmpControl);
                    }
                    else
                        retVal += ValidateUserInput(tmpControl);
                }

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private int ValidateUserInput(IUIControl control)
        {
            #region ValidateUserInput

            int retVal = 0;

            try
            {
                if (!((System.Windows.Forms.Control)control).Enabled)
                    return retVal;

                if (control.IsRequired)
                {
                    if (object.Equals(control.RepresentativeValue, null))
                    {
                        control.SetBackColor(Color.Orange);
                        retVal++;
                    }
                    else if (control.RepresentativeValue.ToString().Length == 0)
                    {
                        control.SetBackColor(Color.Orange);
                        retVal++;
                    }
                    else if (control.GetType() == typeof(TNumericBox) &&
                        (decimal)control.RepresentativeValue == (decimal)0)
                    {
                        control.SetBackColor(Color.Orange);
                        retVal++;
                    }
                    //else if (TapBase.Instance.IsNumeric(control.RepresentativeValue.ToString())
                    //    && int.Parse(control.RepresentativeValue.ToString()) < 0)
                    //{
                    //    control.SetBackColor(Color.Orange);
                    //    retVal++;
                    //}
                    else
                        control.SetBackColor(Color.White);

                }

                return retVal;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Clear

        /// <summary>
        /// This method clears user's input 
        /// </summary>
        /// <param name="panel">Panel</param>
        /// <returns>If 'TRUE', user's input is valid.</returns>
        protected void ClearUserInput(TPanel panel)
        {
            #region Validation

            try
            {
                foreach (IUIControl tmpControl in panel.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TGroupBox)))
                    {
                        this.ClearUserInputForGroupBox((TGroupBox)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TTitledGroupBox)))
                    {
                        this.ClearUserInputForTitledGroupBox((TTitledGroupBox)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TPanel)))
                    {
                        this.ClearUserInputForPanel((TPanel)tmpControl);
                    }
                    else
                        ClearUserInput(tmpControl);
                }

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method clears user's input 
        /// </summary>
        /// <param name="groupBox">Group box</param>
        /// <returns>If 'TRUE', user's input is valid.</returns>
        protected void ClearUserInput(TGroupBox groupBox)
        {
            #region Validation

            try
            {
                foreach (IUIControl tmpControl in groupBox.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TGroupBox)))
                    {
                        this.ClearUserInputForGroupBox((TGroupBox)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TTitledGroupBox)))
                    {
                        this.ClearUserInputForTitledGroupBox((TTitledGroupBox)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TPanel)))
                    {
                        this.ClearUserInputForPanel((TPanel)tmpControl);
                    }
                    else
                        ValidateUserInput(tmpControl);
                }

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method clears user's input 
        /// </summary>
        /// <param name="titledGroupBox">Group box</param>
        /// <returns>If 'TRUE', user's input is valid.</returns>
        protected void ClearUserInput(TTitledGroupBox titledGroupBox)
        {
            #region Validation

            try
            {
                foreach (IUIControl tmpControl in titledGroupBox.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TGroupBox)))
                    {
                        this.ClearUserInputForGroupBox((TGroupBox)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TTitledGroupBox)))
                    {
                        this.ClearUserInputForTitledGroupBox((TTitledGroupBox)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TPanel)))
                    {
                        this.ClearUserInputForPanel((TPanel)tmpControl);
                    }
                    else
                        ClearUserInput(tmpControl);
                }

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void ClearUserInputForGroupBox(TGroupBox groupBox)
        {
            #region ValidateUserInputForGroupBox

            try
            {
                foreach (IUIControl tmpControl in groupBox.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TPanel)))
                    {
                        this.ClearUserInputForPanel((TPanel)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TTitledGroupBox)))
                    {
                        this.ClearUserInputForTitledGroupBox((TTitledGroupBox)tmpControl);
                    }
                    else
                        ClearUserInput(tmpControl);
                }

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void ClearUserInputForTitledGroupBox(TTitledGroupBox groupBox)
        {
            #region ValidateUserInputForGroupBox

            try
            {
                foreach (IUIControl tmpControl in groupBox.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TPanel)))
                    {
                        this.ClearUserInputForPanel((TPanel)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TGroupBox)))
                    {
                        this.ClearUserInputForGroupBox((TGroupBox)tmpControl);
                    }
                    else
                        ClearUserInput(tmpControl);
                }

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void ClearUserInputForPanel(TPanel panel)
        {
            #region Validation

            try
            {
                foreach (IUIControl tmpControl in panel.Controls)
                {
                    if (tmpControl.GetType().Equals(typeof(TGroupBox)))
                    {
                        this.ClearUserInputForGroupBox((TGroupBox)tmpControl);
                    }
                    else if (tmpControl.GetType().Equals(typeof(TTitledGroupBox)))
                    {
                        this.ClearUserInputForTitledGroupBox((TTitledGroupBox)tmpControl);
                    }
                    else
                        ClearUserInput(tmpControl);
                }

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void ClearUserInput(IUIControl control)
        {
            #region Code

            try
            {
                if (!((System.Windows.Forms.Control)control).Enabled)
                    return;

                if (control.GetType() == typeof(TTextBox))
                    ((TTextBox)control).Text = "";
                else if (control.GetType() == typeof(TComboBox))
                    ((TComboBox)control).SelectedIndex = 0;
                else if (control.GetType() == typeof(TNumericBox))
                    ((TNumericBox)control).Value = (decimal)0;

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Utilities

        #region Excel

        //UI의 Event Handler에서 ExportsViaExcel()를 호출하고,
        //ExportToExcelStart()를 Overrding 한다. 

        /// <summary>
        /// This method exports data via excel.
        /// </summary>
        protected void ExportsViaExcel()
        {
            #region Exports via Excel

            try
            {
                SaveFileDialog tmpSaveFilediagLog = new SaveFileDialog();
                tmpSaveFilediagLog.Filter = "Excel file (*.xls)|*.xls||";

                if (tmpSaveFilediagLog.ShowDialog() == DialogResult.OK)
                {
                    _excelFilePath = tmpSaveFilediagLog.FileName;
                    this.BeginAsyncCall("ExportToExcelStart", "ExportToExcelEnd", EnumDataObject.NONE);
                }

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method makes excel object using UI data. Developer must edit this.
        /// </summary>
        virtual public void ExportToExcelStart()
        {
            #region Exports via Excel

            //try
            //{
            //    this.tSheet1.ExportToExcel(this._excelFilePath);
            //    return;
            //}
            //catch (System.Exception ex)
            //{
            //    throw ex;
            //}

            #endregion
        }

        /// <summary>
        /// This is call back method of Excel-Export.
        /// </summary>
        public void ExportToExcelEnd()
        {
            #region Exports via Excel

            try
            {
                string tmpMsg = _translator.ConvertGeneralTemplate(Fressage.EnumVerbs.SAVE, Fressage.EnumGeneralTemplateType.COMPLETE, "Excel file");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.INFORMATION, tmpMsg);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #endregion

        #region Combobox Processing

        #region Basic Info

        /// <summary>
        /// This method binds comboox of TECH.
        /// </summary>
        /// <param name="includeAll">If 'TRUE', combobox includes 'ALL' item</param>
        protected void BindTechCombobox(bool includeAll)
        {
            #region Bind Tech Combobox

            string tmpFacilityValue = string.Empty;
            TComboBox tmpFacility = null;
            TComboBox tmp = null;

            TAP.Models.Codes.TechCodeModel tmpTechs = null;

            try
            {
                tmpFacility = (TComboBox)this.FindControl(_COMBOBOX_FACILITY_NAME);
                tmp = (TComboBox)this.FindControl(_COMBOBOX_TECH_NAME);

                #region Validation

                if (tmpFacility == null || tmpFacility.SelectedIndex < 0)
                    tmpFacilityValue = InfoBase._USER_INFO.Facility;
                else
                    tmpFacilityValue = tmpFacility.SelectedText;

                if (tmp == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.TECH, true);
                    return;
                }

                #endregion

                tmpTechs = new Models.Codes.TechCodeModel(tmpFacilityValue, EnumFlagYN.YES);
                tmp.BindData(tmpTechs.CodeList, includeAll);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method binds comboox of Lot Code.
        /// </summary>
        /// <param name="includeAll">If 'TRUE', combobox includes 'ALL' item</param>
        protected void BindLotCodeCombobox(bool includeAll)
        {
            #region Bind Tech Combobox

            string tmpFacilityValue = string.Empty;

            TComboBox tmpFacility = null;
            TComboBox tmpTech = null;
            TComboBox tmpCombo = null;

            TAP.Models.Factories.BasicInfo.BasicInfoDefaultInfo tmpDefaultInfo = null;
            TAP.Models.Factories.BasicInfo.LotCodeBasicInfoSet tmp = null;

            try
            {
                tmpFacility = (TComboBox)this.FindControl(_COMBOBOX_FACILITY_NAME);
                tmpTech = (TComboBox)this.FindControl(_COMBOBOX_TECH_NAME);
                tmpCombo = (TComboBox)this.FindControl(_COMBOBOX_LOTCODE_NAME);

                #region Validation

                if (tmpFacility == null || tmpFacility.SelectedIndex < 0)
                    tmpFacilityValue = InfoBase._USER_INFO.Facility;
                else
                    tmpFacilityValue = tmpFacility.SelectedText;

                if (tmpTech == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.TECH, true);
                    return;
                }

                if (tmpTech.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.TECH, false);
                    return;
                }

                if (tmpCombo == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.LOTCODE, true);
                    return;
                }

                #endregion

                tmpDefaultInfo = new BasicInfoDefaultInfo();
                tmpDefaultInfo.Region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
                tmpDefaultInfo.Facility = tmpFacilityValue;
                tmpDefaultInfo.Tech = tmpTech.SelectedText;

                tmp = new LotCodeBasicInfoSet();
                tmp.LoadModels(tmpDefaultInfo);

                tmpCombo.BindData(tmp, includeAll);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method binds comboox of Device.
        /// </summary>
        /// <param name="includeAll">If 'TRUE', combobox includes 'ALL' item</param>
        protected void BindDeviceCombobox(bool includeAll)
        {
            #region Bind Tech Combobox

            string tmpFacilityValue = string.Empty;

            TComboBox tmpFacility = null;
            TComboBox tmpTech = null;
            TComboBox tmpLotCode = null;
            TComboBox tmpCombo = null;

            TAP.Models.Factories.BasicInfo.BasicInfoDefaultInfo tmpDefaultInfo = null;
            TAP.Models.Factories.BasicInfo.DeviceBasicInfoSet tmp = null;

            try
            {
                tmpFacility = (TComboBox)this.FindControl(_COMBOBOX_FACILITY_NAME);
                tmpTech = (TComboBox)this.FindControl(_COMBOBOX_TECH_NAME);
                tmpLotCode = (TComboBox)this.FindControl(_COMBOBOX_LOTCODE_NAME);
                tmpCombo = (TComboBox)this.FindControl(_COMBOBOX_DEVICE_NAME);

                #region Validation

                if (tmpFacility == null || tmpFacility.SelectedIndex < 0)
                    tmpFacilityValue = InfoBase._USER_INFO.Facility;
                else
                    tmpFacilityValue = tmpFacility.SelectedText;

                if (tmpTech == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.TECH, true);
                    return;
                }

                if (tmpTech.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.TECH, false);
                    return;
                }

                if (tmpLotCode == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.LOTCODE, true);
                    return;
                }

                if (tmpLotCode.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.LOTCODE, false);
                    return;
                }

                if (tmpCombo == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.DEVICE, true);
                    return;
                }

                #endregion

                tmpDefaultInfo = new BasicInfoDefaultInfo();
                tmpDefaultInfo.Region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
                tmpDefaultInfo.Facility = tmpFacilityValue;
                tmpDefaultInfo.Tech = tmpTech.SelectedText;
                tmpDefaultInfo.LotCode = tmpLotCode.SelectedText;

                tmp = new DeviceBasicInfoSet();
                tmp.LoadModels(tmpDefaultInfo);

                tmpCombo.BindData(tmp, false);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method binds comboox of Main operation.
        /// </summary>
        /// <param name="includeAll">If 'TRUE', combobox includes 'ALL' item</param>
        protected void BindMainOperationCombobox(bool includeAll)
        {
            #region Bind Tech Combobox

            string tmpFacilityValue = string.Empty;

            TComboBox tmpFacility = null;
            TComboBox tmpTech = null;
            TComboBox tmpLotCode = null;
            TComboBox tmpDevice = null;
            TComboBox tmpCombo = null;

            TAP.Models.Factories.BasicInfo.BasicInfoDefaultInfo tmpDefaultInfo = null;
            TAP.Models.Factories.BasicInfo.MainOperationBasicInfoSet tmp = null;

            try
            {
                tmpFacility = (TComboBox)this.FindControl(_COMBOBOX_FACILITY_NAME);
                tmpTech = (TComboBox)this.FindControl(_COMBOBOX_TECH_NAME);
                tmpLotCode = (TComboBox)this.FindControl(_COMBOBOX_LOTCODE_NAME);
                tmpDevice = (TComboBox)this.FindControl(_COMBOBOX_DEVICE_NAME);
                tmpCombo = (TComboBox)this.FindControl(_COMBOBOX_MAINOP_NAME);

                #region Validation

                if (tmpFacility == null || tmpFacility.SelectedIndex < 0)
                    tmpFacilityValue = InfoBase._USER_INFO.Facility;
                else
                    tmpFacilityValue = tmpFacility.SelectedText;

                if (tmpTech == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.TECH, true);
                    return;
                }

                if (tmpTech.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.TECH, false);
                    return;
                }

                if (tmpLotCode == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.LOTCODE, true);
                    return;
                }

                if (tmpLotCode.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.LOTCODE, false);
                    return;
                }

                if (tmpDevice == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.DEVICE, true);
                    return;
                }

                if (tmpDevice.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.DEVICE, false);
                    return;
                }

                if (tmpCombo == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.MAINOPERATION, false);
                    return;
                }

                #endregion

                tmpDefaultInfo = new BasicInfoDefaultInfo();
                tmpDefaultInfo.Region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
                tmpDefaultInfo.Facility = tmpFacilityValue;
                tmpDefaultInfo.Tech = tmpTech.SelectedText;
                tmpDefaultInfo.LotCode = tmpLotCode.SelectedText;
                tmpDefaultInfo.Device = tmpDevice.SelectedText;

                tmp = new MainOperationBasicInfoSet();
                tmp.LoadModels(tmpDefaultInfo);

                tmpCombo.BindData(tmp, false);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method binds comboox of Operation.
        /// </summary>
        /// <param name="includeAll">If 'TRUE', combobox includes 'ALL' item</param>
        protected void BindOperationCombobox(bool includeAll)
        {
            #region Bind Tech Combobox

            string tmpFacilityValue = string.Empty;

            TComboBox tmpFacility = null;
            TComboBox tmpTech = null;
            TComboBox tmpLotCode = null;
            TComboBox tmpDevice = null;
            TComboBox tmpMainOP = null;
            TComboBox tmpCombo = null;

            TAP.Models.Factories.BasicInfo.BasicInfoDefaultInfo tmpDefaultInfo = null;
            TAP.Models.Factories.BasicInfo.OperationBasicInfoSet tmp = null;

            try
            {
                tmpFacility = (TComboBox)this.FindControl(_COMBOBOX_FACILITY_NAME);
                tmpTech = (TComboBox)this.FindControl(_COMBOBOX_TECH_NAME);
                tmpLotCode = (TComboBox)this.FindControl(_COMBOBOX_LOTCODE_NAME);
                tmpLotCode = (TComboBox)this.FindControl(_COMBOBOX_DEVICE_NAME);
                tmpMainOP = (TComboBox)this.FindControl(_COMBOBOX_MAINOP_NAME);
                tmpCombo = (TComboBox)this.FindControl(_COMBOBOX_OPEARTION_NAME);

                #region Validation

                if (tmpFacility == null || tmpFacility.SelectedIndex < 0)
                    tmpFacilityValue = InfoBase._USER_INFO.Facility;
                else
                    tmpFacilityValue = tmpFacility.SelectedText;

                if (tmpTech == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.TECH, true);
                    return;
                }

                if (tmpTech.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.TECH, false);
                    return;
                }

                if (tmpLotCode == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.LOTCODE, true);
                    return;
                }

                if (tmpLotCode.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.LOTCODE, false);
                    return;
                }

                if (tmpDevice == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.DEVICE, true);
                    return;
                }

                if (tmpDevice.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.DEVICE, false);
                    return;
                }

                if (tmpMainOP == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.MAINOPERATION, true);
                    return;
                }

                if (tmpMainOP.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.MAINOPERATION, false);
                    return;
                }
                if (tmpCombo == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel), (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.OPERATION, false);
                    return;
                }

                #endregion

                tmpDefaultInfo = new BasicInfoDefaultInfo();
                tmpDefaultInfo.Region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
                tmpDefaultInfo.Facility = tmpFacilityValue;
                tmpDefaultInfo.Tech = tmpTech.SelectedText;
                tmpDefaultInfo.LotCode = tmpLotCode.SelectedText;
                tmpDefaultInfo.Device = tmpDevice.SelectedText;
                tmpDefaultInfo.MainOperation = tmpMainOP.SelectedText;

                tmp = new OperationBasicInfoSet();
                tmp.LoadModels(tmpDefaultInfo);

                tmpCombo.BindData(tmp, false);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Facility Basic

        /// <summary>
        /// This method binds comboox of Line.
        /// </summary>
        /// <param name="includeAll">If 'TRUE', combobox includes 'ALL' item</param>
        protected void BindLineCombobox(bool includeAll)
        {
            #region Bind Tech Combobox

            string tmpFacilityValue = string.Empty;

            TComboBox tmpFacility = null;
            TComboBox tmpCombo = null;

            TAP.Models.Factories.Facilities.FacilityDefaultInfo tmpDefaultInfo = null;
            TAP.Models.Factories.Facilities.LineModelSet tmp = null;

            try
            {
                tmpFacility = (TComboBox)this.FindControl(_COMBOBOX_FACILITY_NAME);
                tmpCombo = (TComboBox)this.FindControl(_COMBOBOX_LINE_NAME);

                #region Validation

                if (tmpFacility == null || tmpFacility.SelectedIndex < 0)
                    tmpFacilityValue = InfoBase._USER_INFO.Facility;
                else
                    tmpFacilityValue = tmpFacility.SelectedText;

                if (tmpCombo == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.LINE, true);
                    return;
                }

                #endregion

                tmpDefaultInfo = new FacilityDefaultInfo();
                tmpDefaultInfo.Region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
                tmpDefaultInfo.Facility = tmpFacilityValue;

                tmp = new LineModelSet();
                tmp.LoadModels(tmpDefaultInfo);

                tmpCombo.BindData(tmp, includeAll);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method binds comboox of Area.
        /// </summary>
        /// <param name="includeAll">If 'TRUE', combobox includes 'ALL' item</param>
        protected void BindAreaCombobox(bool includeAll)
        {
            #region Bind Tech Combobox

            string tmpFacilityValue = string.Empty;

            TComboBox tmpFacility = null;
            TComboBox tmpLine = null;
            TComboBox tmpCombo = null;

            TAP.Models.Factories.Facilities.FacilityDefaultInfo tmpDefaultInfo = null;
            TAP.Models.Factories.Facilities.AreaModelSet tmp = null;

            try
            {
                tmpFacility = (TComboBox)this.FindControl(_COMBOBOX_FACILITY_NAME);
                tmpLine = (TComboBox)this.FindControl(_COMBOBOX_LINE_NAME);
                tmpCombo = (TComboBox)this.FindControl(_COMBOBOX_AREA_NAME);

                #region Validation

                if (tmpFacility == null || tmpFacility.SelectedIndex < 0)
                    tmpFacilityValue = InfoBase._USER_INFO.Facility;
                else
                    tmpFacilityValue = tmpFacility.SelectedText;

                if (tmpLine == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.LINE, true);
                    return;
                }

                if (tmpLine.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.LINE, false);
                    return;
                }

                if (tmpCombo == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.AREA, true);
                    return;
                }

                #endregion

                tmpDefaultInfo = new FacilityDefaultInfo();
                tmpDefaultInfo.Region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
                tmpDefaultInfo.Facility = tmpFacilityValue;
                tmpDefaultInfo.Line = tmpLine.SelectedText;

                tmp = new AreaModelSet();
                tmp.LoadModels(tmpDefaultInfo);

                tmpCombo.BindData(tmp, includeAll);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method binds comboox of Bay.
        /// </summary>
        /// <param name="includeAll">If 'TRUE', combobox includes 'ALL' item</param>
        protected void BindBayCombobox(bool includeAll)
        {
            #region Bind Tech Combobox

            string tmpFacilityValue = string.Empty;

            TComboBox tmpFacility = null;
            TComboBox tmpLine = null;
            TComboBox tmpArea = null;
            TComboBox tmpCombo = null;

            TAP.Models.Factories.Facilities.FacilityDefaultInfo tmpDefaultInfo = null;
            //TAP.Models.Factories.Facilities.BayModelSet tmp = null;

            try
            {
                tmpFacility = (TComboBox)this.FindControl(_COMBOBOX_FACILITY_NAME);
                tmpLine = (TComboBox)this.FindControl(_COMBOBOX_LINE_NAME);
                tmpArea = (TComboBox)this.FindControl(_COMBOBOX_AREA_NAME);
                tmpCombo = (TComboBox)this.FindControl(_COMBOBOX_BAY_NAME);

                #region Validation

                if (tmpFacility == null || tmpFacility.SelectedIndex < 0)
                    tmpFacilityValue = InfoBase._USER_INFO.Facility;
                else
                    tmpFacilityValue = tmpFacility.SelectedText;

                if (tmpLine == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.LINE, true);
                    return;
                }

                if (tmpLine.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.LINE, false);
                    return;
                }

                if (tmpArea == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.AREA, true);
                    return;
                }

                if (tmpArea.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.AREA, false);
                    return;
                }

                if (tmpCombo == null)
                {
                    //this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.BAY, true);
                    //return;
                }

                #endregion

                tmpDefaultInfo = new FacilityDefaultInfo();
                tmpDefaultInfo.Region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
                tmpDefaultInfo.Facility = tmpFacilityValue;
                tmpDefaultInfo.Line = tmpLine.SelectedText;
                tmpDefaultInfo.Area = tmpArea.SelectedText;

                //tmp = new BayModelSet();
                //tmp.LoadModels(tmpDefaultInfo);

                //tmpCombo.BindData(tmp, includeAll);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method binds comboox of Main equipment.
        /// </summary>
        /// <param name="includeAll">If 'TRUE', combobox includes 'ALL' item</param>
        protected void BindMainEquipmentCombobox(bool includeAll)
        {
            #region Bind Tech Combobox

            string tmpFacilityValue = string.Empty;

            TComboBox tmpFacility = null;
            TComboBox tmpBay = null;
            TComboBox tmpArea = null;
            TComboBox tmpLine = null;
            TComboBox tmpCombo = null;

            TAP.Models.Factories.Facilities.FacilityDefaultInfo tmpDefaultInfo = null;
            TAP.Models.Factories.Facilities.MainEquipmentModelSet tmp = null;

            try
            {
                tmpFacility = (TComboBox)this.FindControl(_COMBOBOX_FACILITY_NAME);
                tmpLine = (TComboBox)this.FindControl(_COMBOBOX_LINE_NAME);
                tmpArea = (TComboBox)this.FindControl(_COMBOBOX_AREA_NAME);
                tmpBay = (TComboBox)this.FindControl(_COMBOBOX_BAY_NAME);
                tmpCombo = (TComboBox)this.FindControl(_COMBOBOX_MAINEQUIPMENT_NAME);

                #region Validation

                if (tmpFacility == null || tmpFacility.SelectedIndex < 0)
                    tmpFacilityValue = InfoBase._USER_INFO.Facility;
                else
                    tmpFacilityValue = tmpFacility.SelectedText;

                if (tmpLine == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.LINE, true);
                    return;
                }

                if (tmpLine.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.LINE, false);
                    return;
                }

                if (tmpArea == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.AREA, true);
                    return;
                }

                if (tmpArea.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.AREA, false);
                    return;
                }

                if (tmpBay == null)
                {
                    //this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.BAY, true);
                    //return;
                }

                if (tmpBay.SelectedIndex < 0)
                {
                    //this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.BAY, false);
                    //return;
                }

                if (tmpCombo == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.MAINEQUIPMENT, true);
                    return;
                }

                #endregion

                tmpDefaultInfo = new FacilityDefaultInfo();
                tmpDefaultInfo.Region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
                tmpDefaultInfo.Facility = tmpFacilityValue;
                tmpDefaultInfo.Line = tmpLine.SelectedText;
                tmpDefaultInfo.Area = tmpArea.SelectedText;
                //tmpDefaultInfo.Bay = tmpBay.SelectedText;

                tmp = new MainEquipmentModelSet();
                tmp.LoadModels(tmpDefaultInfo);

                tmpCombo.BindData(tmp, includeAll);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// This method binds comboox of equipment.
        /// </summary>
        /// <param name="includeAll">If 'TRUE', combobox includes 'ALL' item</param>
        protected void BindEquipmentCombobox(bool includeAll)
        {
            #region Bind Tech Combobox

            string tmpFacilityValue = string.Empty;

            TComboBox tmpFacility = null;
            TComboBox tmpLine = null;
            TComboBox tmpArea = null;
            TComboBox tmpBay = null;
            TComboBox tmpMainEQ = null;
            TComboBox tmpCombo = null;

            TAP.Models.Factories.Facilities.FacilityDefaultInfo tmpDefaultInfo = null;
            TAP.Models.Factories.Facilities.EquipmentModelSet tmp = null;

            try
            {
                tmpFacility = (TComboBox)this.FindControl(_COMBOBOX_FACILITY_NAME);
                tmpLine = (TComboBox)this.FindControl(_COMBOBOX_LINE_NAME);
                tmpArea = (TComboBox)this.FindControl(_COMBOBOX_AREA_NAME);
                tmpBay = (TComboBox)this.FindControl(_COMBOBOX_BAY_NAME);
                tmpMainEQ = (TComboBox)this.FindControl(_COMBOBOX_MAINEQUIPMENT_NAME);
                tmpCombo = (TComboBox)this.FindControl(_COMBOBOX_EQUIPMENT_NAME);

                #region Validation

                if (tmpFacility == null || tmpFacility.SelectedIndex < 0)
                    tmpFacilityValue = InfoBase._USER_INFO.Facility;
                else
                    tmpFacilityValue = tmpFacility.SelectedText;

                if (tmpLine == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.LINE, true);
                    return;
                }

                if (tmpLine.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.LINE, false);
                    return;
                }

                if (tmpArea == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.AREA, true);
                    return;
                }

                if (tmpArea.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.AREA, false);
                    return;
                }

                if (tmpBay == null)
                {
                    //this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.BAY, true);
                    //return;
                }

                if (tmpBay.SelectedIndex < 0)
                {
                    //this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.BAY, false);
                    //return;
                }

                if (tmpMainEQ == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.MAINEQUIPMENT, true);
                    return;
                }

                if (tmpMainEQ.SelectedIndex < 0)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.MAINEQUIPMENT, false);
                    return;
                }
                if (tmpCombo == null)
                {
                    this.MakeComboxErrorMessage(typeof(TAP.Models.Factories.Facilities.FacilityModel), (int)FacilityDefaultInfo.HierarchicalDefaultInfo.EQUIPMENT, true);
                    return;
                }

                #endregion

                tmpDefaultInfo = new FacilityDefaultInfo();
                tmpDefaultInfo.Region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
                tmpDefaultInfo.Facility = tmpFacilityValue;
                tmpDefaultInfo.Line = tmpLine.SelectedText;
                tmpDefaultInfo.Area = tmpArea.SelectedText;
                //tmpDefaultInfo.Bay = tmpBay.SelectedText;
                tmpDefaultInfo.MainEquipment = tmpMainEQ.SelectedText;

                tmp = new EquipmentModelSet();
                tmp.LoadModels(tmpDefaultInfo);

                tmpCombo.BindData(tmp, includeAll);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        private void MakeComboxErrorMessage(Type modelGroup, int comboType, bool isNull)
        {
            #region Make Combox Error Message

            string retVal = string.Empty;

            try
            {
                if (modelGroup == typeof(TAP.Models.Factories.BasicInfo.BasicInfoModel))
                {
                    #region Basic Info

                    if (isNull)
                    {
                        #region NULL CASE

                        switch (comboType)
                        {
                            case (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.FACILITY:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, "Facility <Combobox>"); break;
                            case (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.TECH:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, "Tech <Combobox>"); break;
                            case (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.LOTCODE:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, "Lot code <Combobox>"); break;
                            case (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.DEVICE:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, "Device <Combobox>"); break;
                            case (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.MAINOPERATION:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, "Main operation <Combobox>"); break;
                            case (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.OPERATION:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, "Operation <Combobox>"); break;
                        }

                        #endregion
                    }
                    else
                    {
                        #region NO SELECTED CASE

                        switch (comboType)
                        {
                            case (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.FACILITY:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.SELECT, EnumGeneralTemplateType.ORDER, "Facility"); break;
                            case (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.TECH:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.SELECT, EnumGeneralTemplateType.ORDER, "Tech"); break;
                            case (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.LOTCODE:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.SELECT, EnumGeneralTemplateType.ORDER, "Lot Code"); break; ;
                            case (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.DEVICE:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.SELECT, EnumGeneralTemplateType.ORDER, "Device"); break; ;
                            case (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.MAINOPERATION:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.SELECT, EnumGeneralTemplateType.ORDER, "Main Operation"); break; ;
                            case (int)BasicInfoDefaultInfo.HierarchicalDefaultInfo.OPERATION:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.SELECT, EnumGeneralTemplateType.ORDER, "Opeartion"); break; ;
                        }

                        #endregion
                    }

                    #endregion
                }
                else if (modelGroup == typeof(TAP.Models.Factories.Facilities.FacilityModel))
                {
                    #region Basic Info

                    if (isNull)
                    {
                        #region NULL CASE

                        switch (comboType)
                        {
                            case (int)FacilityDefaultInfo.HierarchicalDefaultInfo.FACILITY:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, "Facility <Combobox>"); break;
                            case (int)FacilityDefaultInfo.HierarchicalDefaultInfo.LINE:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, "Line <Combobox>"); break;
                            case (int)FacilityDefaultInfo.HierarchicalDefaultInfo.AREA:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, "Area <Combobox>"); break;
                            //case (int)FacilityDefaultInfo.HierarchicalDefaultInfo.BAY:
                            //    retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, "Bay <Combobox>"); break;
                            case (int)FacilityDefaultInfo.HierarchicalDefaultInfo.MAINEQUIPMENT:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, "Main equipment <Combobox>"); break;
                            case (int)FacilityDefaultInfo.HierarchicalDefaultInfo.EQUIPMENT:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.EXIST, EnumGeneralTemplateType.NORMALNOT, "Equipment <Combobox>"); break;
                        }

                        #endregion
                    }
                    else
                    {
                        #region NO SELECTED CASE

                        switch (comboType)
                        {
                            case (int)FacilityDefaultInfo.HierarchicalDefaultInfo.FACILITY:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.SELECT, EnumGeneralTemplateType.ORDER, "Facility"); break;
                            case (int)FacilityDefaultInfo.HierarchicalDefaultInfo.LINE:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.SELECT, EnumGeneralTemplateType.ORDER, "Line"); break;
                            case (int)FacilityDefaultInfo.HierarchicalDefaultInfo.AREA:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.SELECT, EnumGeneralTemplateType.ORDER, "Area"); break; ;
                            //case (int)FacilityDefaultInfo.HierarchicalDefaultInfo.BAY:
                            //    retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.SELECT, EnumGeneralTemplateType.ORDER, "Bay"); break; ;
                            case (int)FacilityDefaultInfo.HierarchicalDefaultInfo.MAINEQUIPMENT:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.SELECT, EnumGeneralTemplateType.ORDER, "Main equipment"); break; ;
                            case (int)FacilityDefaultInfo.HierarchicalDefaultInfo.EQUIPMENT:
                                retVal = this._translator.ConvertGeneralTemplate(EnumVerbs.SELECT, EnumGeneralTemplateType.ORDER, "Equipment"); break; ;
                        }

                        #endregion
                    }

                    #endregion
                }

                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, retVal);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region PopupMenu

        #region Methods
        public void OpenUI(string menu)
        {
            MainMenuBasicModel tmpMainMenu = null;
            UIBasicModel tmpUI = null;

            try
            {
                string mdiName = this.UIInformation.MDI;
                string mainMenu = this.UIInformation.MainMenu;

                tmpMainMenu = InfoBase._MDI_INFO[this.UIInformation.MDI].MainMenus[this.UIInformation.MainMenu];

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

                Assembly a = null;
                Type tmpType = null;
                object tmpObject = null;
                Form tmpNewForm = null;

                a = Assembly.LoadFile(Path.Combine(TapBase.Instance.ApplicationPath, tmpUI.AssemblyFileName));

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

                //??
                tmpObject = Activator.CreateInstance(tmpType);

                //??
                tmpNewForm = (Form)tmpObject;
                tmpNewForm.MdiParent = this.ParentForm;
                tmpNewForm.FormBorderStyle = FormBorderStyle.None;
                tmpNewForm.Dock = DockStyle.Fill;

                tmpNewForm.Name = "223";
                ((TAP.UI.UIBase)tmpNewForm).UIInformation = tmpUI;
                //((TAP.UI.UIBase)tmpNewForm).TabControl = form.tabMDIList;
                //((TAP.UI.UIBase)tmpNewForm).UITitle = form.MakeUITitle(InfoBase._MDI_INFO[form._mdiName].MainMenus[mainMenu].DisplayName, tmpUI.DisplayName);
                ((TAP.UI.UIBase)tmpNewForm).UITitle = "123";

                tmpNewForm.Show();

                //AgumentPack Data ??? ??
                //ArgumentPack tmpPack = new ArgumentPack();
                //tmpPack.AddArgument("TEST", typeof(string), "Hello");
                //((TAP.UI.UIBase)tmpNewForm).ExecuteCommand(tmpPack);


                return;
            }
            catch (System.Exception ex)
            {
                string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.OPEN, EnumGeneralTemplateType.FAIL, "menu");
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, tmpMessage, ex.ToString());
            }
        }
        #region Fields

        #endregion
        protected virtual void SetPopupMenuItem(List<LinkInfo> listInfo)
        {
            foreach (var item in listInfo)
            {
                DevExpress.XtraBars.BarSubItem barSubItem = new DevExpress.XtraBars.BarSubItem();
                barSubItem.Caption = item.GROUPNAME;
                foreach (var info in item.list)
                {
                    DevExpress.XtraBars.BarButtonItem barButtonItem = new DevExpress.XtraBars.BarButtonItem();
                    barButtonItem.Caption = info.TAGETUINAME;
                    barButtonItem.Tag = info.UI;
                    barSubItem.LinksPersistInfo.Add(new DevExpress.XtraBars.LinkPersistInfo(barButtonItem));
                    barButtonItem.ItemClick += BarButtonItem_ItemClick;
                }
                barManager.Items.Add(barSubItem);
                PopMenuBase.LinksPersistInfo.Add(
                new DevExpress.XtraBars.LinkPersistInfo(barSubItem));
            }
        }

        private void BarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenUI(e.Item.Tag.ToString());
        }

        #endregion

        #region Event

        //public delegate void PopupMenuClick(string frmUrl);

        //public event PopupMenuClick popupMenuClick;

        //protected virtual void popupMenuInit(string frmUrl)
        //{
        //    if (this.popupMenuClick!=null)
        //    {
        //        this.popupMenuClick(frmUrl);
        //    }
        //}

        #endregion

        #endregion

        /// <summary>
        /// This method initialized components
        /// </summary>
        protected void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UIBase));
            this.tPanelMain = new TAP.UIControls.BasicControls.TPanel();
            this.tPanelTop = new TAP.UIControls.BasicControls.TPanel();
            this.pnlButtons = new TAP.UIControls.BasicControls.TPanel();
            this.picBookMark = new TAP.UIControls.BasicControls.TPictureBox();
            this.pic = new TAP.UIControls.BasicControls.TPictureBox();
            this.picClose = new TAP.UIControls.BasicControls.TPictureBox();
            this.lblTitle = new TAP.UIControls.BasicControls.TLabel();
            this.tPanelBottomBase = new TAP.UIControls.BasicControls.TPanel();
            this.tPanel5 = new TAP.UIControls.BasicControls.TPanel();
            this.tLabelStatus = new TAP.UIControls.BasicControls.TLabel();
            this.tPanel4 = new TAP.UIControls.BasicControls.TPanel();
            this.tLabelTime = new TAP.UIControls.BasicControls.TLabel();
            this.tPanel3 = new TAP.UIControls.BasicControls.TPanel();
            this.tPictureBox1 = new TAP.UIControls.BasicControls.TPictureBox();
            this.tPanel2 = new TAP.UIControls.BasicControls.TPanel();
            this.tPanel1 = new TAP.UIControls.BasicControls.TPanel();
            this.progressBar1 = new TAP.UIControls.BasicControls.TSolidProgressBar();
            this.tLabel1 = new TAP.UIControls.BasicControls.TLabel();
            this.timerCurrent = new System.Windows.Forms.Timer(this.components);
            this.PopMenuBase = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.tPanelTop.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBookMark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
            this.tPanelBottomBase.SuspendLayout();
            this.tPanel5.SuspendLayout();
            this.tPanel4.SuspendLayout();
            this.tPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tPictureBox1)).BeginInit();
            this.tPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PopMenuBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            this.SuspendLayout();
            // 
            // tPanelMain
            // 
            this.tPanelMain.ControlID = "tPanelMain";
            this.tPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tPanelMain.IsRequired = false;
            this.tPanelMain.Location = new System.Drawing.Point(0, 20);
            this.tPanelMain.Name = "tPanelMain";
            this.tPanelMain.NeedToTranslate = true;
            this.tPanelMain.RepresentativeValue = "tPanelMain [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.For" +
    "ms.BorderStyle.None";
            this.tPanelMain.Size = new System.Drawing.Size(1012, 687);
            this.tPanelMain.TabIndex = 3;
            // 
            // tPanelTop
            // 
            this.tPanelTop.ControlID = "tPanelTop";
            this.tPanelTop.Controls.Add(this.pnlButtons);
            this.tPanelTop.Controls.Add(this.lblTitle);
            this.tPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.tPanelTop.IsRequired = false;
            this.tPanelTop.Location = new System.Drawing.Point(0, 0);
            this.tPanelTop.Name = "tPanelTop";
            this.tPanelTop.NeedToTranslate = true;
            this.tPanelTop.RepresentativeValue = "tPanelTop [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Form" +
    "s.BorderStyle.None";
            this.tPanelTop.Size = new System.Drawing.Size(1012, 20);
            this.tPanelTop.TabIndex = 2;
            // 
            // pnlButtons
            // 
            this.pnlButtons.ControlID = "pnlButtons";
            this.pnlButtons.Controls.Add(this.picBookMark);
            this.pnlButtons.Controls.Add(this.pic);
            this.pnlButtons.Controls.Add(this.picClose);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.IsRequired = false;
            this.pnlButtons.Location = new System.Drawing.Point(744, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.NeedToTranslate = true;
            this.pnlButtons.RepresentativeValue = "pnlButtons [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.For" +
    "ms.BorderStyle.None";
            this.pnlButtons.Size = new System.Drawing.Size(268, 20);
            this.pnlButtons.TabIndex = 1;
            // 
            // picBookMark
            // 
            this.picBookMark.ControlID = "pic";
            this.picBookMark.Dock = System.Windows.Forms.DockStyle.Right;
            this.picBookMark.Image = global::TAP.UI.Properties.Resources.destar16;
            this.picBookMark.IsRequired = false;
            this.picBookMark.Location = new System.Drawing.Point(208, 0);
            this.picBookMark.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.picBookMark.Name = "picBookMark";
            this.picBookMark.NeedToTranslate = true;
            this.picBookMark.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.picBookMark.RepresentativeValue = global::TAP.UI.Properties.Resources.destar16;
            this.picBookMark.RollOverImage = null;
            this.picBookMark.Size = new System.Drawing.Size(20, 20);
            this.picBookMark.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBookMark.TabIndex = 2;
            this.picBookMark.TabStop = false;
            this.picBookMark.Click += new System.EventHandler(this.picBookMark_Click);
            // 
            // pic
            // 
            this.pic.ControlID = "pic";
            this.pic.Dock = System.Windows.Forms.DockStyle.Right;
            this.pic.Image = global::TAP.UI.Properties.Resources.xls1;
            this.pic.IsRequired = false;
            this.pic.Location = new System.Drawing.Point(228, 0);
            this.pic.Name = "pic";
            this.pic.NeedToTranslate = true;
            this.pic.RepresentativeValue = global::TAP.UI.Properties.Resources.xls1;
            this.pic.RollOverImage = global::TAP.UI.Properties.Resources.xls_rollover;
            this.pic.Size = new System.Drawing.Size(20, 20);
            this.pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pic.TabIndex = 1;
            this.pic.TabStop = false;
            this.pic.Visible = false;
            this.pic.Click += new System.EventHandler(this.pic_Click);
            // 
            // picClose
            // 
            this.picClose.ControlID = "tPictureBox1";
            this.picClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.picClose.Image = global::TAP.UI.Properties.Resources.close;
            this.picClose.IsRequired = false;
            this.picClose.Location = new System.Drawing.Point(248, 0);
            this.picClose.Name = "picClose";
            this.picClose.NeedToTranslate = true;
            this.picClose.RepresentativeValue = global::TAP.UI.Properties.Resources.close;
            this.picClose.RollOverImage = global::TAP.UI.Properties.Resources.closeUI_rollover;
            this.picClose.Size = new System.Drawing.Size(20, 20);
            this.picClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picClose.TabIndex = 0;
            this.picClose.TabStop = false;
            this.picClose.Click += new System.EventHandler(this.picClose_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.ControlID = "lblTitle";
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(74)))));
            this.lblTitle.IsRequired = false;
            this.lblTitle.Location = new System.Drawing.Point(1, 4);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.NeedToTranslate = true;
            this.lblTitle.RepresentativeValue = "Title";
            this.lblTitle.Size = new System.Drawing.Size(27, 13);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title";
            // 
            // tPanelBottomBase
            // 
            this.tPanelBottomBase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.tPanelBottomBase.ControlID = "tPanelBottomBase";
            this.tPanelBottomBase.Controls.Add(this.tPanel5);
            this.tPanelBottomBase.Controls.Add(this.tPanel4);
            this.tPanelBottomBase.Controls.Add(this.tPanel3);
            this.tPanelBottomBase.Controls.Add(this.tPanel2);
            this.tPanelBottomBase.Controls.Add(this.tPanel1);
            this.tPanelBottomBase.Controls.Add(this.tLabel1);
            this.tPanelBottomBase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tPanelBottomBase.IsRequired = false;
            this.tPanelBottomBase.Location = new System.Drawing.Point(0, 707);
            this.tPanelBottomBase.Name = "tPanelBottomBase";
            this.tPanelBottomBase.NeedToTranslate = true;
            this.tPanelBottomBase.RepresentativeValue = "tPanelBottomBase [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windo" +
    "ws.Forms.BorderStyle.None";
            this.tPanelBottomBase.Size = new System.Drawing.Size(1012, 20);
            this.tPanelBottomBase.TabIndex = 0;
            // 
            // tPanel5
            // 
            this.tPanel5.ControlID = "tPanel5";
            this.tPanel5.Controls.Add(this.tLabelStatus);
            this.tPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tPanel5.IsRequired = false;
            this.tPanel5.Location = new System.Drawing.Point(110, 0);
            this.tPanel5.Name = "tPanel5";
            this.tPanel5.NeedToTranslate = true;
            this.tPanel5.RepresentativeValue = "tPanel5 [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms." +
    "BorderStyle.None";
            this.tPanel5.Size = new System.Drawing.Size(756, 20);
            this.tPanel5.TabIndex = 5;
            // 
            // tLabelStatus
            // 
            this.tLabelStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.tLabelStatus.ControlID = "tLabelStatus";
            this.tLabelStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLabelStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(74)))));
            this.tLabelStatus.IsRequired = false;
            this.tLabelStatus.Location = new System.Drawing.Point(0, 0);
            this.tLabelStatus.Name = "tLabelStatus";
            this.tLabelStatus.NeedToTranslate = true;
            this.tLabelStatus.RepresentativeValue = "Ready";
            this.tLabelStatus.Size = new System.Drawing.Size(756, 20);
            this.tLabelStatus.TabIndex = 0;
            this.tLabelStatus.Text = "Ready";
            this.tLabelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tPanel4
            // 
            this.tPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.tPanel4.ControlID = "tPanel4";
            this.tPanel4.Controls.Add(this.tLabelTime);
            this.tPanel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.tPanel4.IsRequired = false;
            this.tPanel4.Location = new System.Drawing.Point(866, 0);
            this.tPanel4.Name = "tPanel4";
            this.tPanel4.NeedToTranslate = true;
            this.tPanel4.RepresentativeValue = "tPanel4 [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms." +
    "BorderStyle.None";
            this.tPanel4.Size = new System.Drawing.Size(126, 20);
            this.tPanel4.TabIndex = 4;
            // 
            // tLabelTime
            // 
            this.tLabelTime.ControlID = "tLabelTime";
            this.tLabelTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLabelTime.IsRequired = false;
            this.tLabelTime.Location = new System.Drawing.Point(0, 0);
            this.tLabelTime.Name = "tLabelTime";
            this.tLabelTime.NeedToTranslate = true;
            this.tLabelTime.RepresentativeValue = "00:00:00";
            this.tLabelTime.Size = new System.Drawing.Size(126, 20);
            this.tLabelTime.TabIndex = 0;
            this.tLabelTime.Text = "00:00:00";
            this.tLabelTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tPanel3
            // 
            this.tPanel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.tPanel3.ControlID = "tPanel3";
            this.tPanel3.Controls.Add(this.tPictureBox1);
            this.tPanel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.tPanel3.IsRequired = false;
            this.tPanel3.Location = new System.Drawing.Point(992, 0);
            this.tPanel3.Name = "tPanel3";
            this.tPanel3.NeedToTranslate = true;
            this.tPanel3.RepresentativeValue = "tPanel3 [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms." +
    "BorderStyle.None";
            this.tPanel3.Size = new System.Drawing.Size(20, 20);
            this.tPanel3.TabIndex = 3;
            // 
            // tPictureBox1
            // 
            this.tPictureBox1.ControlID = "tPictureBox1";
            this.tPictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tPictureBox1.Image = global::TAP.UI.Properties.Resources.btnStop;
            this.tPictureBox1.IsRequired = false;
            this.tPictureBox1.Location = new System.Drawing.Point(0, 0);
            this.tPictureBox1.Name = "tPictureBox1";
            this.tPictureBox1.NeedToTranslate = true;
            this.tPictureBox1.RepresentativeValue = global::TAP.UI.Properties.Resources.btnStop;
            this.tPictureBox1.RollOverImage = global::TAP.UI.Properties.Resources.btnStop_rollover;
            this.tPictureBox1.Size = new System.Drawing.Size(20, 20);
            this.tPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.tPictureBox1.TabIndex = 0;
            this.tPictureBox1.TabStop = false;
            this.tPictureBox1.Click += new System.EventHandler(this.tButtonStop_Click);
            // 
            // tPanel2
            // 
            this.tPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.tPanel2.ControlID = "tPanel2";
            this.tPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.tPanel2.IsRequired = false;
            this.tPanel2.Location = new System.Drawing.Point(100, 0);
            this.tPanel2.Name = "tPanel2";
            this.tPanel2.NeedToTranslate = true;
            this.tPanel2.RepresentativeValue = "tPanel2 [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms." +
    "BorderStyle.None";
            this.tPanel2.Size = new System.Drawing.Size(10, 20);
            this.tPanel2.TabIndex = 2;
            // 
            // tPanel1
            // 
            this.tPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tPanel1.ControlID = "tPanel1";
            this.tPanel1.Controls.Add(this.progressBar1);
            this.tPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tPanel1.IsRequired = false;
            this.tPanel1.Location = new System.Drawing.Point(0, 0);
            this.tPanel1.Name = "tPanel1";
            this.tPanel1.NeedToTranslate = true;
            this.tPanel1.RepresentativeValue = "tPanel1 [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms." +
    "BorderStyle.None";
            this.tPanel1.Size = new System.Drawing.Size(100, 20);
            this.tPanel1.TabIndex = 1;
            // 
            // progressBar1
            // 
            this.progressBar1.BarColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.progressBar1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.FillStyle = TAP.UIControls.BasicControls.TSolidProgressBar.FillStyles.Solid;
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.Maximum = 100;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 20);
            this.progressBar1.Step = 10;
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Value = 0;
            // 
            // tLabel1
            // 
            this.tLabel1.AutoSize = true;
            this.tLabel1.ControlID = "tLabel1";
            this.tLabel1.IsRequired = false;
            this.tLabel1.Location = new System.Drawing.Point(7, -2);
            this.tLabel1.Name = "tLabel1";
            this.tLabel1.NeedToTranslate = true;
            this.tLabel1.RepresentativeValue = "tLabel1";
            this.tLabel1.Size = new System.Drawing.Size(42, 13);
            this.tLabel1.TabIndex = 0;
            this.tLabel1.Text = "tLabel1";
            // 
            // PopMenuBase
            // 
            this.PopMenuBase.Manager = this.barManager;
            this.PopMenuBase.Name = "PopMenuBase";
            // 
            // barManager
            // 
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1012, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 727);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1012, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 727);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1012, 0);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 727);
            // 
            // UIBase
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1012, 727);
            this.Controls.Add(this.tPanelMain);
            this.Controls.Add(this.tPanelTop);
            this.Controls.Add(this.tPanelBottomBase);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UIBase";
            this.Activated += new System.EventHandler(this.UIBase_Activated);
            this.Load += new System.EventHandler(this.UI_Load);
            this.tPanelTop.ResumeLayout(false);
            this.tPanelTop.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBookMark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
            this.tPanelBottomBase.ResumeLayout(false);
            this.tPanelBottomBase.PerformLayout();
            this.tPanel5.ResumeLayout(false);
            this.tPanel4.ResumeLayout(false);
            this.tPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tPictureBox1)).EndInit();
            this.tPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PopMenuBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// Disposer
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// This event hander will be executed when from load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UI_Load(object sender, EventArgs e)
        {
            #region Code

            try
            {
                this.Initialize();
                this.LoadTime();
            }
            catch (System.Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "Initiaze UI Failed", ex.ToString());
            }

            #endregion
        }

        private void LoadTime()
        {
            #region Current Time Load

            try
            {
                System.Windows.Forms.Timer timer = timerCurrent;
                timer.Tick += new EventHandler(TimerStart);
                timer.Start();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private bool BookMarkCheck()
        {
            #region Code


            TAP.Data.Client.DataClient tmpDBC = new Data.Client.DataClient();
            string tmpMainMenu = string.Empty;
            string tmpUIName = string.Empty;
            string tmpUser = string.Empty;
            string tmpRegion = string.Empty;
            string tmpFacility = string.Empty;
            string tmpQuery = string.Empty;
            string tmpMdiName = string.Empty;
            string tmpSql = string.Empty;

            try
            {

                tmpMainMenu = this.UIInformation.MainMenu;
                tmpUIName = this.UIInformation.Name;
                tmpUser = InfoBase._USER_INFO.Name;
                tmpRegion = InfoBase._USER_INFO.Region;
                tmpFacility = InfoBase._USER_INFO.Facility;
                tmpMdiName = this.UIInformation.MDI;

                tmpSql = string.Format("SELECT * FROM TAPSTBUIBOOKMARK " +
                                       "WHERE NAME = '{0}' " +
                                         "AND MDI = '{1}' " +
                                         "AND REGION = '{2}' " +
                                         "AND FACILITY = '{3}' " +
                                         "AND MAINMENU = '{4}' " +
                                         "AND UINAME = '{5}'",
                                         tmpUser, tmpMdiName, tmpRegion, tmpFacility, tmpMainMenu, tmpUIName);

                DataSet ds = tmpDBC.SelectData(tmpSql, "TAPSTBUIBOOKMARK");
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (System.Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "BookMark Check Error", ex.ToString());
                return false;
            }
            #endregion
        }
    }
}
