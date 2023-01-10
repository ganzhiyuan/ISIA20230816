namespace TAP.UI.MDI
{
    partial class FormWithLauncher
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
            this.imageListMainMenu = new System.Windows.Forms.ImageList(this.components);
            this.ribbonMenu = new System.Windows.Forms.Ribbon();
            this.tabMDIList = new TAP.UIControls.BasicControls.TTabControl();
            this.tPanel2 = new TAP.UIControls.BasicControls.TPanel();
            this.tPanel1 = new TAP.UIControls.BasicControls.TPanel();
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
            this.ribbonMenu.CaptionBarVisible = false;
            this.ribbonMenu.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(1)), true);
            this.ribbonMenu.Location = new System.Drawing.Point(0, 0);
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
            this.ribbonMenu.Size = new System.Drawing.Size(1264, 120);
            this.ribbonMenu.TabIndex = 20;
            this.ribbonMenu.TabsMargin = new System.Windows.Forms.Padding(12, 2, 20, 0);
            this.ribbonMenu.Text = "mainMenu";
            this.ribbonMenu.ThemeColor = System.Windows.Forms.RibbonTheme.Black;
            this.ribbonMenu.Click += new System.EventHandler(this.ribbonMenu_Click);
            // 
            // tabMDIList
            // 
            this.tabMDIList.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.tabMDIList.AllowDrop = true;
            this.tabMDIList.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabMDIList.BackTabColor = System.Drawing.Color.White;
            this.tabMDIList.ClosingMessage = null;
            this.tabMDIList.ControlID = "tabMDIList";
            this.tabMDIList.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabMDIList.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.tabMDIList.HeaderColor = System.Drawing.Color.White;
            this.tabMDIList.HorizontalLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.tabMDIList.IsRequired = false;
            this.tabMDIList.ItemSize = new System.Drawing.Size(240, 20);
            this.tabMDIList.Location = new System.Drawing.Point(0, 123);
            this.tabMDIList.Name = "tabMDIList";
            this.tabMDIList.NeedToTranslate = true;
            this.tabMDIList.RepresentativeValue = "";
            this.tabMDIList.SelectedIndex = 0;
            this.tabMDIList.ShowClosingButton = false;
            this.tabMDIList.ShowClosingMessage = false;
            this.tabMDIList.Size = new System.Drawing.Size(1264, 25);
            this.tabMDIList.TabIndex = 22;
            this.tabMDIList.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tabMDIList.SelectedIndexChanged += new System.EventHandler(this.tabMDIList_SelectedIndexChanged);
            // 
            // tPanel2
            // 
            this.tPanel2.BackColor = System.Drawing.Color.White;
            this.tPanel2.ControlID = "tPanel2";
            this.tPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tPanel2.IsRequired = false;
            this.tPanel2.Location = new System.Drawing.Point(0, 120);
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
            this.tPanel1.Location = new System.Drawing.Point(0, 921);
            this.tPanel1.Name = "tPanel1";
            this.tPanel1.NeedToTranslate = true;
            this.tPanel1.RepresentativeValue = "tPanel1 [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms." +
    "BorderStyle.None";
            this.tPanel1.Size = new System.Drawing.Size(1264, 1);
            this.tPanel1.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 922);
            this.Controls.Add(this.tabMDIList);
            this.Controls.Add(this.tPanel2);
            this.Controls.Add(this.ribbonMenu);
            this.Controls.Add(this.tPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "mina OIP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageListMainMenu;
        private UIControls.BasicControls.TPanel tPanel1;
        private System.Windows.Forms.Ribbon ribbonMenu;
        private UIControls.BasicControls.TPanel tPanel2;
        private UIControls.BasicControls.TTabControl tabMDIList;
    }
}

