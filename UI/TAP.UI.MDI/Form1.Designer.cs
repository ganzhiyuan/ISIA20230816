namespace TAP.UI.MDI
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            this.imageListMainMenu = new System.Windows.Forms.ImageList(this.components);
            this.ribbonMenu = new System.Windows.Forms.Ribbon();
            this.tPanel2 = new TAP.UIControls.BasicControls.TPanel();
            this.tPanel1 = new TAP.UIControls.BasicControls.TPanel();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barMenu = new DevExpress.XtraBars.Bar();
            this.btnLoginInfo = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenuUser = new DevExpress.XtraBars.PopupMenu(this.components);
            this.btnPassword = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonLogout = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonClose = new DevExpress.XtraBars.BarButtonItem();
            this.btnBookMark = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenuBookMark = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barBottom = new DevExpress.XtraBars.Bar();
            this.barSite = new DevExpress.XtraBars.BarStaticItem();
            this.barFactory = new DevExpress.XtraBars.BarStaticItem();
            this.barUser = new DevExpress.XtraBars.BarStaticItem();
            this.barDate = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.btnBookMarkDelete = new DevExpress.XtraBars.BarButtonItem();
            this.btnUIAdd = new DevExpress.XtraBars.BarButtonItem();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemHypertextLabel1 = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemToggleSwitch1 = new DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.tPanel3 = new TAP.UIControls.BasicControls.TPanel();
            this.timerCurrent = new System.Windows.Forms.Timer(this.components);
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuBookMark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            this.SuspendLayout();
            // 
            // imageListMainMenu
            // 
            this.imageListMainMenu.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMainMenu.ImageStream")));
            this.imageListMainMenu.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMainMenu.Images.SetKeyName(0, "s_IconBase.png");
            this.imageListMainMenu.Images.SetKeyName(1, "s_hierachical.PNG");
            this.imageListMainMenu.Images.SetKeyName(2, "s_h.png");
            this.imageListMainMenu.Images.SetKeyName(3, "s_01_region.png");
            this.imageListMainMenu.Images.SetKeyName(4, "s_02_facility.png");
            this.imageListMainMenu.Images.SetKeyName(5, "s_03_tech.png");
            this.imageListMainMenu.Images.SetKeyName(6, "s_04_lotCode.png");
            this.imageListMainMenu.Images.SetKeyName(7, "s_05_device.png");
            this.imageListMainMenu.Images.SetKeyName(8, "s_06_mainop.png");
            this.imageListMainMenu.Images.SetKeyName(9, "s_07_operation.png");
            this.imageListMainMenu.Images.SetKeyName(10, "s_08_Parameter.png");
            this.imageListMainMenu.Images.SetKeyName(11, "s_09_Part.png");
            this.imageListMainMenu.Images.SetKeyName(12, "s_10_recipe.png");
            this.imageListMainMenu.Images.SetKeyName(13, "s_11_interlock.png");
            this.imageListMainMenu.Images.SetKeyName(14, "s_12_grade.png");
            this.imageListMainMenu.Images.SetKeyName(15, "s_03_Bay.png");
            this.imageListMainMenu.Images.SetKeyName(16, "s_04_Area.png");
            this.imageListMainMenu.Images.SetKeyName(17, "s_05_Line.png");
            this.imageListMainMenu.Images.SetKeyName(18, "s_06_mainEquipment.png");
            this.imageListMainMenu.Images.SetKeyName(19, "s_07_equipment.png");
            this.imageListMainMenu.Images.SetKeyName(20, "s_08_chamber.png");
            this.imageListMainMenu.Images.SetKeyName(21, "s_09_port.png");
            this.imageListMainMenu.Images.SetKeyName(22, "s_02_fggroup.png");
            this.imageListMainMenu.Images.SetKeyName(23, "s_03_fgcode.png");
            this.imageListMainMenu.Images.SetKeyName(24, "s_04_grade.png");
            this.imageListMainMenu.Images.SetKeyName(25, "s_05_revision.png");
            // 
            // ribbonMenu
            // 
            this.ribbonMenu.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ribbonMenu.CaptionBarVisible = false;
            this.ribbonMenu.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(1)), true);
            this.ribbonMenu.Location = new System.Drawing.Point(0, 24);
            this.ribbonMenu.Minimized = false;
            this.ribbonMenu.Name = "ribbonMenu";
            // 
            // 
            // 
            this.ribbonMenu.OrbDropDown.BorderRoundness = 8;
            this.ribbonMenu.OrbDropDown.Location = new System.Drawing.Point(0, 0);
            this.ribbonMenu.OrbDropDown.Name = "";
            this.ribbonMenu.OrbDropDown.Size = new System.Drawing.Size(527, 447);
            this.ribbonMenu.OrbDropDown.TabIndex = 0;
            this.ribbonMenu.OrbImage = null;
            this.ribbonMenu.OrbStyle = System.Windows.Forms.RibbonOrbStyle.Office_2013;
            this.ribbonMenu.OrbVisible = false;
            // 
            // 
            // 
            this.ribbonMenu.QuickAcessToolbar.Enabled = false;
            this.ribbonMenu.QuickAcessToolbar.Visible = false;
            this.ribbonMenu.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.ribbonMenu.Size = new System.Drawing.Size(1264, 140);
            this.ribbonMenu.TabIndex = 20;
            this.ribbonMenu.TabsMargin = new System.Windows.Forms.Padding(12, 2, 20, 0);
            this.ribbonMenu.Text = "mainMenu";
            this.ribbonMenu.ThemeColor = System.Windows.Forms.RibbonTheme.Black;
            this.ribbonMenu.Visible = false;
            // 
            // tPanel2
            // 
            this.tPanel2.BackColor = System.Drawing.Color.White;
            this.tPanel2.ControlID = "tPanel2";
            this.tPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tPanel2.IsRequired = false;
            this.tPanel2.Location = new System.Drawing.Point(0, 164);
            this.tPanel2.Name = "tPanel2";
            this.tPanel2.NeedToTranslate = true;
            this.tPanel2.RepresentativeValue = "tPanel2 [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms." +
    "BorderStyle.None";
            this.tPanel2.Size = new System.Drawing.Size(1264, 3);
            this.tPanel2.TabIndex = 21;
            // 
            // tPanel1
            // 
            this.tPanel1.ControlID = "tPanel1";
            this.tPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tPanel1.IsRequired = false;
            this.tPanel1.Location = new System.Drawing.Point(0, 911);
            this.tPanel1.Name = "tPanel1";
            this.tPanel1.NeedToTranslate = true;
            this.tPanel1.RepresentativeValue = "tPanel1 [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms." +
    "BorderStyle.None";
            this.tPanel1.Size = new System.Drawing.Size(1264, 1);
            this.tPanel1.TabIndex = 13;
            // 
            // barManager
            // 
            this.barManager.AllowCustomization = false;
            this.barManager.AllowItemAnimatedHighlighting = false;
            this.barManager.AllowMoveBarOnToolbar = false;
            this.barManager.AllowQuickCustomization = false;
            this.barManager.AllowShowToolbarsPopup = false;
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barMenu,
            this.barBottom});
            this.barManager.CloseButtonAffectAllTabs = false;
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.DockWindowTabFont = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.barManager.Form = this;
            this.barManager.Images = this.imageListMainMenu;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnLoginInfo,
            this.barButtonLogout,
            this.barButtonClose,
            this.btnBookMark,
            this.btnBookMarkDelete,
            this.btnUIAdd,
            this.btnPassword,
            this.barSite,
            this.barHeaderItem1,
            this.barFactory,
            this.barUser,
            this.barDate,
            this.barStaticItem1});
            this.barManager.MainMenu = this.barMenu;
            this.barManager.MaxItemId = 52;
            this.barManager.OptionsLayout.AllowAddNewItems = false;
            this.barManager.OptionsStubGlyphs.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.barManager.OptionsStubGlyphs.UseFont = true;
            this.barManager.OptionsStubGlyphs.VerticalAlignment = System.Drawing.StringAlignment.Far;
            this.barManager.PopupMenuAlignment = DevExpress.XtraBars.PopupMenuAlignment.Left;
            this.barManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemHypertextLabel1,
            this.repositoryItemTextEdit2,
            this.repositoryItemToggleSwitch1});
            this.barManager.ShowScreenTipsInToolbars = false;
            this.barManager.ShowShortcutInScreenTips = false;
            this.barManager.StatusBar = this.barBottom;
            // 
            // barMenu
            // 
            this.barMenu.BarName = "Main menu";
            this.barMenu.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.barMenu.DockCol = 0;
            this.barMenu.DockRow = 0;
            this.barMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnLoginInfo, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnBookMark, true)});
            this.barMenu.OptionsBar.AllowQuickCustomization = false;
            this.barMenu.OptionsBar.DrawBorder = false;
            this.barMenu.OptionsBar.DrawDragBorder = false;
            this.barMenu.OptionsBar.RotateWhenVertical = false;
            this.barMenu.OptionsBar.UseWholeRow = true;
            this.barMenu.Text = "Main menu";
            // 
            // btnLoginInfo
            // 
            this.btnLoginInfo.ActAsDropDown = true;
            this.btnLoginInfo.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnLoginInfo.AllowRightClickInMenu = false;
            this.btnLoginInfo.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            this.btnLoginInfo.Caption = "LoginUser";
            this.btnLoginInfo.ContentHorizontalAlignment = DevExpress.XtraBars.BarItemContentAlignment.Far;
            this.btnLoginInfo.DropDownControl = this.popupMenuUser;
            this.btnLoginInfo.Id = 22;
            this.btnLoginInfo.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnLoginInfo.ImageOptions.SvgImage")));
            this.btnLoginInfo.Name = "btnLoginInfo";
            this.btnLoginInfo.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // popupMenuUser
            // 
            this.popupMenuUser.DrawMenuRightIndent = DevExpress.Utils.DefaultBoolean.True;
            this.popupMenuUser.DrawMenuSideStrip = DevExpress.Utils.DefaultBoolean.True;
            this.popupMenuUser.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPassword),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonLogout),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonClose)});
            this.popupMenuUser.Manager = this.barManager;
            this.popupMenuUser.Name = "popupMenuUser";
            // 
            // btnPassword
            // 
            this.btnPassword.Caption = "Change Password";
            this.btnPassword.Id = 45;
            this.btnPassword.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnPassword.ImageOptions.Image")));
            this.btnPassword.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnPassword.ImageOptions.LargeImage")));
            this.btnPassword.Name = "btnPassword";
            this.btnPassword.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPassword_ItemClick);
            // 
            // barButtonLogout
            // 
            this.barButtonLogout.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barButtonLogout.AllowRightClickInMenu = false;
            this.barButtonLogout.Caption = "Logout";
            this.barButtonLogout.Id = 24;
            this.barButtonLogout.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonLogout.ImageOptions.Image")));
            this.barButtonLogout.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonLogout.ImageOptions.LargeImage")));
            this.barButtonLogout.Name = "barButtonLogout";
            this.barButtonLogout.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barButtonLogout.Tag = "Logout";
            this.barButtonLogout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PopupMenuBarButtion_Click);
            // 
            // barButtonClose
            // 
            this.barButtonClose.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
            this.barButtonClose.AllowRightClickInMenu = false;
            this.barButtonClose.Caption = "Close";
            this.barButtonClose.Id = 25;
            this.barButtonClose.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonClose.ImageOptions.Image")));
            this.barButtonClose.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonClose.ImageOptions.LargeImage")));
            this.barButtonClose.Name = "barButtonClose";
            this.barButtonClose.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barButtonClose.Tag = "Close";
            this.barButtonClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PopupMenuBarButtion_Click);
            // 
            // btnBookMark
            // 
            this.btnBookMark.ActAsDropDown = true;
            this.btnBookMark.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnBookMark.AllowRightClickInMenu = false;
            this.btnBookMark.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            this.btnBookMark.Caption = "BookMark";
            this.btnBookMark.Description = "BookMark";
            this.btnBookMark.DropDownControl = this.popupMenuBookMark;
            this.btnBookMark.GroupIndex = 2;
            this.btnBookMark.Hint = "BookMark";
            this.btnBookMark.Id = 31;
            this.btnBookMark.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnBookMark.ImageOptions.Image")));
            this.btnBookMark.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnBookMark.ImageOptions.LargeImage")));
            this.btnBookMark.Name = "btnBookMark";
            this.btnBookMark.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            toolTipTitleItem2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            toolTipTitleItem2.Text = "BookMark";
            superToolTip2.Items.Add(toolTipTitleItem2);
            this.btnBookMark.SuperTip = superToolTip2;
            this.btnBookMark.ItemDoubleClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnBookMark_ItemDoubleClick);
            // 
            // popupMenuBookMark
            // 
            this.popupMenuBookMark.Manager = this.barManager;
            this.popupMenuBookMark.Name = "popupMenuBookMark";
            this.popupMenuBookMark.CloseUp += new System.EventHandler(this.popupMenuBookMark_CloseUp);
            // 
            // barBottom
            // 
            this.barBottom.BarAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.barBottom.BarAppearance.Normal.Options.UseFont = true;
            this.barBottom.BarName = "Custom 3";
            this.barBottom.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.barBottom.DockCol = 0;
            this.barBottom.DockRow = 0;
            this.barBottom.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.barBottom.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSite),
            new DevExpress.XtraBars.LinkPersistInfo(this.barFactory),
            new DevExpress.XtraBars.LinkPersistInfo(this.barUser),
            new DevExpress.XtraBars.LinkPersistInfo(this.barDate),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1)});
            this.barBottom.OptionsBar.AllowQuickCustomization = false;
            this.barBottom.OptionsBar.AutoPopupMode = DevExpress.XtraBars.BarAutoPopupMode.None;
            this.barBottom.OptionsBar.DrawDragBorder = false;
            this.barBottom.OptionsBar.UseWholeRow = true;
            this.barBottom.Text = "Custom 3";
            // 
            // barSite
            // 
            this.barSite.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barSite.AllowRightClickInMenu = false;
            this.barSite.Caption = "Site ID : ";
            this.barSite.Id = 46;
            this.barSite.ImageOptions.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
            this.barSite.Name = "barSite";
            this.barSite.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // barFactory
            // 
            this.barFactory.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barFactory.AllowRightClickInMenu = false;
            this.barFactory.Caption = "Factory : ";
            this.barFactory.Id = 48;
            this.barFactory.Name = "barFactory";
            this.barFactory.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // barUser
            // 
            this.barUser.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barUser.AllowRightClickInMenu = false;
            this.barUser.Caption = "User ID : ";
            this.barUser.Id = 49;
            this.barUser.Name = "barUser";
            this.barUser.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // barDate
            // 
            this.barDate.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barDate.AllowRightClickInMenu = false;
            this.barDate.ContentHorizontalAlignment = DevExpress.XtraBars.BarItemContentAlignment.Far;
            this.barDate.Id = 50;
            this.barDate.Name = "barDate";
            this.barDate.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1264, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 912);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1264, 25);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 888);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1264, 24);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 888);
            // 
            // btnBookMarkDelete
            // 
            this.btnBookMarkDelete.ActAsDropDown = true;
            this.btnBookMarkDelete.AllowAllUp = true;
            this.btnBookMarkDelete.AllowRightClickInMenu = false;
            this.btnBookMarkDelete.Caption = "Delete";
            this.btnBookMarkDelete.DropDownEnabled = false;
            this.btnBookMarkDelete.Id = 37;
            this.btnBookMarkDelete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnBookMarkDelete.ImageOptions.Image")));
            this.btnBookMarkDelete.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnBookMarkDelete.ImageOptions.LargeImage")));
            this.btnBookMarkDelete.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnBookMarkDelete.ImageOptions.SvgImage")));
            this.btnBookMarkDelete.Name = "btnBookMarkDelete";
            this.btnBookMarkDelete.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnBookMarkDelete.RibbonStyle = ((DevExpress.XtraBars.Ribbon.RibbonItemStyles)(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) 
            | DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText)));
            this.btnBookMarkDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnBookMarkDelete_ItemClick);
            // 
            // btnUIAdd
            // 
            this.btnUIAdd.Caption = "Current UI Add";
            this.btnUIAdd.CloseSubMenuOnClick = false;
            this.btnUIAdd.DropDownEnabled = false;
            this.btnUIAdd.Enabled = false;
            this.btnUIAdd.Id = 44;
            this.btnUIAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnUIAdd.ImageOptions.Image")));
            this.btnUIAdd.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnUIAdd.ImageOptions.LargeImage")));
            this.btnUIAdd.Name = "btnUIAdd";
            this.btnUIAdd.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnUIAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUIAdd_ItemClick);
            // 
            // barHeaderItem1
            // 
            this.barHeaderItem1.Caption = "barHeaderItem1";
            this.barHeaderItem1.Id = 47;
            this.barHeaderItem1.Name = "barHeaderItem1";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // repositoryItemHypertextLabel1
            // 
            this.repositoryItemHypertextLabel1.Name = "repositoryItemHypertextLabel1";
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // repositoryItemToggleSwitch1
            // 
            this.repositoryItemToggleSwitch1.AutoHeight = false;
            this.repositoryItemToggleSwitch1.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.repositoryItemToggleSwitch1.GlyphVerticalAlignment = DevExpress.Utils.VertAlignment.Center;
            this.repositoryItemToggleSwitch1.Name = "repositoryItemToggleSwitch1";
            this.repositoryItemToggleSwitch1.OffText = "";
            this.repositoryItemToggleSwitch1.OnText = "Delete";
            this.repositoryItemToggleSwitch1.ShowText = false;
            // 
            // xtraTabbedMdiManager1
            // 
            this.xtraTabbedMdiManager1.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeaderAndOnMouseHover;
            this.xtraTabbedMdiManager1.CloseTabOnMiddleClick = DevExpress.XtraTabbedMdi.CloseTabOnMiddleClick.Never;
            this.xtraTabbedMdiManager1.FloatOnDoubleClick = DevExpress.Utils.DefaultBoolean.True;
            this.xtraTabbedMdiManager1.FloatOnDrag = DevExpress.Utils.DefaultBoolean.True;
            this.xtraTabbedMdiManager1.HeaderButtons = ((DevExpress.XtraTab.TabButtons)((DevExpress.XtraTab.TabButtons.Prev | DevExpress.XtraTab.TabButtons.Next)));
            this.xtraTabbedMdiManager1.MdiParent = this;
            this.xtraTabbedMdiManager1.PageAdded += new DevExpress.XtraTabbedMdi.MdiTabPageEventHandler(this.xtraTabbedMdiManager1_PageAdded);
            this.xtraTabbedMdiManager1.PageRemoved += new DevExpress.XtraTabbedMdi.MdiTabPageEventHandler(this.xtraTabbedMdiManager1_PageRemoved);
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnBookMarkDelete)});
            this.popupMenu1.Manager = this.barManager;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // tPanel3
            // 
            this.tPanel3.BackgroundImage = global::TAP.UI.MDI.Properties.Resources.winpac_main;
            this.tPanel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tPanel3.ControlID = "tPanel3";
            this.tPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tPanel3.IsRequired = false;
            this.tPanel3.Location = new System.Drawing.Point(0, 167);
            this.tPanel3.Name = "tPanel3";
            this.tPanel3.NeedToTranslate = true;
            this.tPanel3.RepresentativeValue = "tPanel3 [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms." +
    "BorderStyle.None";
            this.tPanel3.Size = new System.Drawing.Size(1264, 744);
            this.tPanel3.TabIndex = 26;
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.AllowRightClickInMenu = false;
            this.barStaticItem1.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring;
            this.barStaticItem1.Id = 51;
            this.barStaticItem1.Name = "barStaticItem1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 937);
            this.Controls.Add(this.tPanel3);
            this.Controls.Add(this.tPanel2);
            this.Controls.Add(this.ribbonMenu);
            this.Controls.Add(this.tPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "mina OIP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuBookMark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageListMainMenu;
        private UIControls.BasicControls.TPanel tPanel1;
        private System.Windows.Forms.Ribbon ribbonMenu;
        private UIControls.BasicControls.TPanel tPanel2;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar barMenu;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel repositoryItemHypertextLabel1;
        private DevExpress.XtraBars.BarButtonItem btnLoginInfo;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraBars.PopupMenu popupMenuUser;
        private DevExpress.XtraBars.BarButtonItem barButtonLogout;
        private DevExpress.XtraBars.BarButtonItem barButtonClose;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager1;
        private DevExpress.XtraBars.BarButtonItem btnBookMark;
        private DevExpress.XtraBars.PopupMenu popupMenuBookMark;
        private DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch repositoryItemToggleSwitch1;
        private DevExpress.XtraBars.BarButtonItem btnBookMarkDelete;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private UIControls.BasicControls.TPanel tPanel3;
        private DevExpress.XtraBars.BarButtonItem btnUIAdd;
        private DevExpress.XtraBars.BarButtonItem btnPassword;
        private DevExpress.XtraBars.Bar barBottom;
        private DevExpress.XtraBars.BarStaticItem barSite;
        private DevExpress.XtraBars.BarStaticItem barFactory;
        private DevExpress.XtraBars.BarStaticItem barUser;
        private DevExpress.XtraBars.BarStaticItem barDate;
        private DevExpress.XtraBars.BarHeaderItem barHeaderItem1;
        private System.Windows.Forms.Timer timerCurrent;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
    }
}

