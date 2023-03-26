namespace TAP.UI.MDI
{
    partial class FormRibbon_ISEM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRibbon_ISEM));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            this.imageListMainMenu = new System.Windows.Forms.ImageList(this.components);
            this.tPanel2 = new TAP.UIControls.BasicControls.TPanel();
            this.tPanel1 = new TAP.UIControls.BasicControls.TPanel();
            this.btnLoginInfo = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenuUser = new DevExpress.XtraBars.PopupMenu(this.components);
            this.btnPassword = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnLanguage = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonLogout = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonClose = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnBookMark = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenuBookMark = new DevExpress.XtraBars.PopupMenu(this.components);
            this.btnBookMarkDelete = new DevExpress.XtraBars.BarButtonItem();
            this.btnUIAdd = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.barBtnCloseCurrentUI = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnCloseAllUI = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemHypertextLabel1 = new DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemToggleSwitch1 = new DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.tContextMenu1 = new TAP.UIControls.BasicControlsDEV.TContextMenu();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuBookMark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tContextMenu1)).BeginInit();
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
            // tPanel2
            // 
            this.tPanel2.BackColor = System.Drawing.Color.White;
            this.tPanel2.ControlID = "tPanel2";
            this.tPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tPanel2.IsRequired = false;
            this.tPanel2.Location = new System.Drawing.Point(0, 55);
            this.tPanel2.Name = "tPanel2";
            this.tPanel2.NeedToTranslate = true;
            this.tPanel2.RepresentativeValue = "tPanel2 [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms." +
    "BorderStyle.None";
            this.tPanel2.Size = new System.Drawing.Size(1272, 3);
            this.tPanel2.TabIndex = 21;
            // 
            // tPanel1
            // 
            this.tPanel1.ControlID = "tPanel1";
            this.tPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tPanel1.IsRequired = false;
            this.tPanel1.Location = new System.Drawing.Point(0, 940);
            this.tPanel1.Name = "tPanel1";
            this.tPanel1.NeedToTranslate = true;
            this.tPanel1.RepresentativeValue = "tPanel1 [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms." +
    "BorderStyle.None";
            this.tPanel1.Size = new System.Drawing.Size(1272, 1);
            this.tPanel1.TabIndex = 13;
            // 
            // btnLoginInfo
            // 
            this.btnLoginInfo.ActAsDropDown = true;
            this.btnLoginInfo.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnLoginInfo.AllowRightClickInMenu = false;
            this.btnLoginInfo.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            this.btnLoginInfo.Caption = "LoginUser";
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
            this.popupMenuUser.ItemLinks.Add(this.btnPassword);
            this.popupMenuUser.ItemLinks.Add(this.barBtnLanguage);
            this.popupMenuUser.ItemLinks.Add(this.barButtonLogout);
            this.popupMenuUser.ItemLinks.Add(this.barButtonClose);
            this.popupMenuUser.Name = "popupMenuUser";
            this.popupMenuUser.Ribbon = this.ribbonControl1;
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
            // barBtnLanguage
            // 
            this.barBtnLanguage.Caption = "Change Language";
            this.barBtnLanguage.Id = 40;
            this.barBtnLanguage.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barBtnLanguage.ImageOptions.Image")));
            this.barBtnLanguage.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barBtnLanguage.ImageOptions.LargeImage")));
            this.barBtnLanguage.Name = "barBtnLanguage";
            this.barBtnLanguage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnLanguage_ItemClick);
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
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Images = this.imageListMainMenu;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.btnLoginInfo,
            this.barButtonLogout,
            this.barButtonClose,
            this.btnBookMark,
            this.btnBookMarkDelete,
            this.btnUIAdd,
            this.btnPassword,
            this.barSubItem1,
            this.barBtnCloseCurrentUI,
            this.barBtnCloseAllUI,
            this.barButtonItem1,
            this.barBtnLanguage,
            this.ribbonControl1.SearchEditItem});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 43;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.OptionsPageCategories.ShowCaptions = false;
            this.ribbonControl1.PageHeaderItemLinks.Add(this.btnLoginInfo);
            this.ribbonControl1.PageHeaderItemLinks.Add(this.btnBookMark);
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemHypertextLabel1,
            this.repositoryItemTextEdit2,
            this.repositoryItemToggleSwitch1});
            this.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2013;
            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowQatLocationSelector = false;
            this.ribbonControl1.ShowToolbarCustomizeItem = false;
            this.ribbonControl1.Size = new System.Drawing.Size(1272, 55);
            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
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
            this.btnBookMark.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            toolTipTitleItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            toolTipTitleItem1.Text = "BookMark";
            superToolTip1.Items.Add(toolTipTitleItem1);
            this.btnBookMark.SuperTip = superToolTip1;
            this.btnBookMark.ItemDoubleClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnBookMark_ItemDoubleClick);
            // 
            // popupMenuBookMark
            // 
            this.popupMenuBookMark.Name = "popupMenuBookMark";
            this.popupMenuBookMark.Ribbon = this.ribbonControl1;
            this.popupMenuBookMark.CloseUp += new System.EventHandler(this.popupMenuBookMark_CloseUp);
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
            this.btnUIAdd.CloseSubMenuOnClickMode = DevExpress.Utils.DefaultBoolean.False;
            this.btnUIAdd.DropDownEnabled = false;
            this.btnUIAdd.Enabled = false;
            this.btnUIAdd.Id = 44;
            this.btnUIAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnUIAdd.ImageOptions.Image")));
            this.btnUIAdd.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnUIAdd.ImageOptions.LargeImage")));
            this.btnUIAdd.Name = "btnUIAdd";
            this.btnUIAdd.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnUIAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUIAdd_ItemClick);
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "barSubItem1";
            this.barSubItem1.Id = 43;
            this.barSubItem1.Name = "barSubItem1";
            this.barSubItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // barBtnCloseCurrentUI
            // 
            this.barBtnCloseCurrentUI.Caption = "Close Current UI";
            this.barBtnCloseCurrentUI.Id = 28;
            this.barBtnCloseCurrentUI.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barBtnCloseCurrentUI.ImageOptions.Image")));
            this.barBtnCloseCurrentUI.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barBtnCloseCurrentUI.ImageOptions.LargeImage")));
            this.barBtnCloseCurrentUI.Name = "barBtnCloseCurrentUI";
            this.barBtnCloseCurrentUI.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnCloseCurrentUI_ItemClick);
            // 
            // barBtnCloseAllUI
            // 
            this.barBtnCloseAllUI.Caption = "Close All Opened UIs";
            this.barBtnCloseAllUI.Id = 31;
            this.barBtnCloseAllUI.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barBtnCloseAllUI.ImageOptions.Image")));
            this.barBtnCloseAllUI.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barBtnCloseAllUI.ImageOptions.LargeImage")));
            this.barBtnCloseAllUI.Name = "barBtnCloseAllUI";
            this.barBtnCloseAllUI.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnCloseAllUI_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 33;
            this.barButtonItem1.Name = "barButtonItem1";
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
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Office 2013";
            // 
            // xtraTabbedMdiManager1
            // 
            this.xtraTabbedMdiManager1.AppearancePage.HeaderActive.BackColor = System.Drawing.SystemColors.HotTrack;
            this.xtraTabbedMdiManager1.AppearancePage.HeaderActive.Options.UseBackColor = true;
            this.xtraTabbedMdiManager1.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeaderAndOnMouseHover;
            this.xtraTabbedMdiManager1.CloseTabOnMiddleClick = DevExpress.XtraTabbedMdi.CloseTabOnMiddleClick.Never;
            this.xtraTabbedMdiManager1.FloatOnDoubleClick = DevExpress.Utils.DefaultBoolean.True;
            this.xtraTabbedMdiManager1.FloatOnDrag = DevExpress.Utils.DefaultBoolean.True;
            this.xtraTabbedMdiManager1.HeaderButtons = ((DevExpress.XtraTab.TabButtons)((DevExpress.XtraTab.TabButtons.Prev | DevExpress.XtraTab.TabButtons.Next)));
            this.xtraTabbedMdiManager1.MdiParent = this;
            this.xtraTabbedMdiManager1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.xtraTabbedMdiManager1_MouseUp);
            this.xtraTabbedMdiManager1.PageRemoved += new DevExpress.XtraTabbedMdi.MdiTabPageEventHandler(this.xtraTabbedMdiManager1_PageRemoved);
            // 
            // popupMenu1
            // 
            this.popupMenu1.ItemLinks.Add(this.btnBookMarkDelete);
            this.popupMenu1.Name = "popupMenu1";
            this.popupMenu1.Ribbon = this.ribbonControl1;
            // 
            // tContextMenu1
            // 
            this.tContextMenu1.ControlID = "tContextMenu1";
            this.tContextMenu1.IsRequired = false;
            this.tContextMenu1.ItemLinks.Add(this.barBtnCloseCurrentUI);
            this.tContextMenu1.ItemLinks.Add(this.barBtnCloseAllUI);
            this.tContextMenu1.Name = "tContextMenu1";
            this.tContextMenu1.NeedToTranslate = true;
            this.tContextMenu1.RepresentativeValue = "123";
            this.tContextMenu1.Ribbon = this.ribbonControl1;
            // 
            // FormRibbon_ISEM
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1272, 941);
            this.Controls.Add(this.tPanel2);
            this.Controls.Add(this.tPanel1);
            this.Controls.Add(this.ribbonControl1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FormRibbon_ISEM.IconOptions.Icon")));
            this.IsMdiContainer = true;
            this.Name = "FormRibbon_ISEM";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "mina OIP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuBookMark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHypertextLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tContextMenu1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageListMainMenu;
        private UIControls.BasicControls.TPanel tPanel1;
        private UIControls.BasicControls.TPanel tPanel2;
        private DevExpress.XtraBars.BarButtonItem btnLoginInfo;
        private DevExpress.XtraBars.PopupMenu popupMenuUser;
        private DevExpress.XtraBars.BarButtonItem barButtonLogout;
        private DevExpress.XtraBars.BarButtonItem barButtonClose;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager1;
        private DevExpress.XtraBars.BarButtonItem btnBookMark;
        private DevExpress.XtraBars.PopupMenu popupMenuBookMark;
        private DevExpress.XtraBars.BarButtonItem btnBookMarkDelete;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarButtonItem btnUIAdd;
        private DevExpress.XtraBars.BarButtonItem btnPassword;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemHypertextLabel repositoryItemHypertextLabel1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch repositoryItemToggleSwitch1;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem barBtnCloseCurrentUI;
        private DevExpress.XtraBars.BarButtonItem barBtnCloseAllUI;
        private UIControls.BasicControlsDEV.TContextMenu tContextMenu1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barBtnLanguage;
    }
}

