namespace ISIA.UI.BASE
{
    partial class DockUIBase1T4
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private new void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.xtraUserControl1 = new DevExpress.XtraEditors.XtraUserControl();
            this.dpnlRight = new DevExpress.XtraBars.Docking.DockPanel();
            this.dpnlRight_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.dpnlLeft = new DevExpress.XtraBars.Docking.DockPanel();
            this.dpnlLeft_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.tPanelBottomBase.SuspendLayout();
            this.tPanelMain.SuspendLayout();
            this.xtraUserControl1.SuspendLayout();
            this.dpnlRight.SuspendLayout();
            this.dpnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // tPanelBottomBase
            // 
            this.tPanelBottomBase.Location = new System.Drawing.Point(0, 599);
            this.tPanelBottomBase.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tPanelBottomBase.Size = new System.Drawing.Size(849, 15);
            // 
            // tLabel1
            // 
            this.tLabel1.Location = new System.Drawing.Point(5, -2);
            this.tLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            // 
            // tPanelTop
            // 
            this.tPanelTop.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tPanelTop.Size = new System.Drawing.Size(849, 15);
            // 
            // tPanelMain
            // 
            this.tPanelMain.Controls.Add(this.xtraUserControl1);
            this.tPanelMain.Location = new System.Drawing.Point(0, 15);
            this.tPanelMain.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tPanelMain.Size = new System.Drawing.Size(849, 584);
            // 
            // xtraUserControl1
            // 
            this.xtraUserControl1.Controls.Add(this.dpnlRight);
            this.xtraUserControl1.Controls.Add(this.dpnlLeft);
            this.xtraUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraUserControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraUserControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.xtraUserControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.xtraUserControl1.Name = "xtraUserControl1";
            this.xtraUserControl1.Size = new System.Drawing.Size(849, 584);
            this.xtraUserControl1.TabIndex = 0;
            // 
            // dpnlRight
            // 
            this.dpnlRight.Appearance.BackColor = System.Drawing.Color.White;
            this.dpnlRight.Appearance.Options.UseBackColor = true;
            this.dpnlRight.Controls.Add(this.dpnlRight_Container);
            this.dpnlRight.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dpnlRight.ID = new System.Guid("9fea1709-589f-4b72-80a4-5e492eb07ae8");
            this.dpnlRight.Location = new System.Drawing.Point(250, 0);
            this.dpnlRight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dpnlRight.Name = "dpnlRight";
            this.dpnlRight.OriginalSize = new System.Drawing.Size(799, 200);
            this.dpnlRight.SavedSizeFactor = 0D;
            this.dpnlRight.Size = new System.Drawing.Size(599, 584);
            this.dpnlRight.Text = "dpnlRight";
            // 
            // dpnlRight_Container
            // 
            this.dpnlRight_Container.Location = new System.Drawing.Point(4, 1);
            this.dpnlRight_Container.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dpnlRight_Container.Name = "dpnlRight_Container";
            this.dpnlRight_Container.Size = new System.Drawing.Size(591, 578);
            this.dpnlRight_Container.TabIndex = 0;
            // 
            // dpnlLeft
            // 
            this.dpnlLeft.Appearance.BackColor = System.Drawing.Color.White;
            this.dpnlLeft.Appearance.Options.UseBackColor = true;
            this.dpnlLeft.Controls.Add(this.dpnlLeft_Container);
            this.dpnlLeft.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dpnlLeft.FloatSize = new System.Drawing.Size(10, 10);
            this.dpnlLeft.ID = new System.Guid("19033d06-40d4-4ecd-914d-24c32beba911");
            this.dpnlLeft.Location = new System.Drawing.Point(0, 0);
            this.dpnlLeft.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dpnlLeft.Name = "dpnlLeft";
            this.dpnlLeft.Options.ShowCloseButton = false;
            this.dpnlLeft.Options.ShowMaximizeButton = false;
            this.dpnlLeft.OriginalSize = new System.Drawing.Size(333, 153);
            this.dpnlLeft.SavedSizeFactor = 0D;
            this.dpnlLeft.Size = new System.Drawing.Size(250, 584);
            // 
            // dpnlLeft_Container
            // 
            this.dpnlLeft_Container.Location = new System.Drawing.Point(4, 1);
            this.dpnlLeft_Container.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dpnlLeft_Container.Name = "dpnlLeft_Container";
            this.dpnlLeft_Container.Size = new System.Drawing.Size(241, 579);
            this.dpnlLeft_Container.TabIndex = 0;
            // 
            // dockManager1
            // 
            this.dockManager1.AllowGlyphSkinning = true;
            this.dockManager1.DockingOptions.ShowCaptionOnMouseHover = true;
            this.dockManager1.Form = this.xtraUserControl1;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dpnlLeft,
            this.dpnlRight});
            this.dockManager1.Style = DevExpress.XtraBars.Docking2010.Views.DockingViewStyle.Classic;
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl"});
            // 
            // DockUIBase1T4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 614);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "DockUIBase1T4";
            this.Text = "DockUIBase1T4";
            this.tPanelBottomBase.ResumeLayout(false);
            this.tPanelBottomBase.PerformLayout();
            this.tPanelMain.ResumeLayout(false);
            this.xtraUserControl1.ResumeLayout(false);
            this.dpnlRight.ResumeLayout(false);
            this.dpnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected DevExpress.XtraEditors.XtraUserControl xtraUserControl1;
        protected DevExpress.XtraBars.Docking.ControlContainer dpnlLeft_Container;
        protected DevExpress.XtraBars.Docking.DockManager dockManager1;
        protected DevExpress.XtraBars.Docking.DockPanel dpnlLeft;
        public DevExpress.XtraBars.Docking.DockPanel dpnlRight;
        public DevExpress.XtraBars.Docking.ControlContainer dpnlRight_Container;
    }
}