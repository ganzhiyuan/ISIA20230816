
namespace ISIA.UI.MANAGEMENT
{
    partial class FrmSQLOperation
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
        private void InitializeComponent()
        {
            this.tMemoEdit1 = new TAP.UIControls.BasicControlsDEV.TMemoEdit();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.xtraUserControl1.SuspendLayout();
            this.dpnlLeft_Container.SuspendLayout();
            this.dpnlLeft.SuspendLayout();
            this.dpnlRight.SuspendLayout();
            this.dpnlRight_Container.SuspendLayout();
            this.tPanelBottomBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PopMenuBase)).BeginInit();
            this.tPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tMemoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraUserControl1
            // 
            this.xtraUserControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.xtraUserControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xtraUserControl1.Appearance.ForeColor = System.Drawing.Color.Black;
            this.xtraUserControl1.Appearance.Options.UseBackColor = true;
            this.xtraUserControl1.Appearance.Options.UseFont = true;
            this.xtraUserControl1.Appearance.Options.UseForeColor = true;
            this.xtraUserControl1.LookAndFeel.SkinName = "Visual Studio 2013 Light";
            this.xtraUserControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.xtraUserControl1.Size = new System.Drawing.Size(1049, 641);
            // 
            // dpnlLeft_Container
            // 
            this.dpnlLeft_Container.Controls.Add(this.tMemoEdit1);
            this.dpnlLeft_Container.Location = new System.Drawing.Point(3, 1);
            this.dpnlLeft_Container.Size = new System.Drawing.Size(413, 637);
            // 
            // dpnlLeft
            // 
            this.dpnlLeft.Appearance.BackColor = System.Drawing.Color.White;
            this.dpnlLeft.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dpnlLeft.Appearance.ForeColor = System.Drawing.Color.Black;
            this.dpnlLeft.Appearance.Options.UseBackColor = true;
            this.dpnlLeft.Appearance.Options.UseFont = true;
            this.dpnlLeft.Appearance.Options.UseForeColor = true;
            this.dpnlLeft.Options.ShowCloseButton = false;
            this.dpnlLeft.Options.ShowMaximizeButton = false;
            this.dpnlLeft.OriginalSize = new System.Drawing.Size(420, 153);
            this.dpnlLeft.SavedSizeFactor = 1D;
            this.dpnlLeft.Size = new System.Drawing.Size(420, 641);
            // 
            // dpnlRight
            // 
            this.dpnlRight.Appearance.BackColor = System.Drawing.Color.White;
            this.dpnlRight.Appearance.Options.UseBackColor = true;
            this.dpnlRight.Location = new System.Drawing.Point(420, 0);
            this.dpnlRight.Options.AllowDockAsTabbedDocument = false;
            this.dpnlRight.Options.AllowDockBottom = false;
            this.dpnlRight.Options.AllowDockLeft = false;
            this.dpnlRight.Options.AllowDockRight = false;
            this.dpnlRight.Options.AllowDockTop = false;
            this.dpnlRight.Options.ShowAutoHideButton = false;
            this.dpnlRight.Options.ShowCloseButton = false;
            this.dpnlRight.Options.ShowMaximizeButton = false;
            this.dpnlRight.OriginalSize = new System.Drawing.Size(629, 200);
            this.dpnlRight.SavedSizeFactor = 1D;
            this.dpnlRight.Size = new System.Drawing.Size(629, 641);
            // 
            // dpnlRight_Container
            // 
            this.dpnlRight_Container.Controls.Add(this.gridControl1);
            this.dpnlRight_Container.Location = new System.Drawing.Point(3, 1);
            this.dpnlRight_Container.Size = new System.Drawing.Size(623, 637);
            // 
            // tPanelBottomBase
            // 
            this.tPanelBottomBase.Location = new System.Drawing.Point(0, 656);
            this.tPanelBottomBase.Size = new System.Drawing.Size(1049, 15);
            // 
            // tPanelTop
            // 
            this.tPanelTop.Size = new System.Drawing.Size(1049, 15);
            // 
            // tPanelMain
            // 
            this.tPanelMain.Size = new System.Drawing.Size(1049, 641);
            // 
            // tMemoEdit1
            // 
            this.tMemoEdit1.ControlID = "tMemoEdit1";
            this.tMemoEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tMemoEdit1.IsRequired = false;
            this.tMemoEdit1.Location = new System.Drawing.Point(0, 0);
            this.tMemoEdit1.Name = "tMemoEdit1";
            this.tMemoEdit1.NeedToTranslate = false;
            this.tMemoEdit1.RepresentativeValue = "";
            this.tMemoEdit1.Size = new System.Drawing.Size(413, 637);
            this.tMemoEdit1.TabIndex = 0;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(623, 637);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // FrmSQLOperation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1049, 671);
            this.Name = "FrmSQLOperation";
            this.Text = "FrmSQLOperation";
            this.xtraUserControl1.ResumeLayout(false);
            this.dpnlLeft_Container.ResumeLayout(false);
            this.dpnlLeft.ResumeLayout(false);
            this.dpnlRight.ResumeLayout(false);
            this.dpnlRight_Container.ResumeLayout(false);
            this.tPanelBottomBase.ResumeLayout(false);
            this.tPanelBottomBase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PopMenuBase)).EndInit();
            this.tPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tMemoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TAP.UIControls.BasicControlsDEV.TMemoEdit tMemoEdit1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}