namespace ISIA.UI.BASE
{
    partial class TemplateSpec
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
        virtual protected new void InitializeComponent()
        {
            this.splitA = new DevExpress.XtraEditors.SplitContainerControl();
            this.SplitA1A = new DevExpress.XtraEditors.SplitContainerControl();
            this.tPanelBottomBase.SuspendLayout();
            this.tPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitA)).BeginInit();
            this.splitA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitA1A)).BeginInit();
            this.SplitA1A.SuspendLayout();
            this.SuspendLayout();
            // 
            // tPanelBottomBase
            // 
            this.tPanelBottomBase.Location = new System.Drawing.Point(0, 701);
            this.tPanelBottomBase.Size = new System.Drawing.Size(1008, 22);
            // 
            // tLabel1
            // 
            this.tLabel1.Location = new System.Drawing.Point(6, -2);
            // 
            // tPanelTop
            // 
            this.tPanelTop.Size = new System.Drawing.Size(1008, 22);
            // 
            // tPanelMain
            // 
            this.tPanelMain.Controls.Add(this.splitA);
            this.tPanelMain.Location = new System.Drawing.Point(0, 22);
            this.tPanelMain.Size = new System.Drawing.Size(1008, 679);
            // 
            // splitA
            // 
            this.splitA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitA.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.splitA.Location = new System.Drawing.Point(0, 0);
            this.splitA.Name = "splitA";
            this.splitA.Panel1.Controls.Add(this.SplitA1A);
            this.splitA.Panel1.Text = "Panel1";
            this.splitA.Panel2.Text = "Panel2";
            this.splitA.Size = new System.Drawing.Size(1008, 679);
            this.splitA.SplitterPosition = 300;
            this.splitA.TabIndex = 0;
            this.splitA.Text = "splitContainerControl1";
            // 
            // SplitA1A
            // 
            this.SplitA1A.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitA1A.Horizontal = false;
            this.SplitA1A.IsSplitterFixed = true;
            this.SplitA1A.Location = new System.Drawing.Point(0, 0);
            this.SplitA1A.Name = "SplitA1A";
            this.SplitA1A.Panel1.Text = "Panel1";
            this.SplitA1A.Panel2.Text = "Panel2";
            this.SplitA1A.Size = new System.Drawing.Size(696, 679);
            this.SplitA1A.SplitterPosition = 36;
            this.SplitA1A.TabIndex = 0;
            this.SplitA1A.Text = "splitContainerControl2";
            // 
            // TemplateSpec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 723);
            this.Name = "TemplateSpec";
            this.Text = "TemplateSpec";
            this.tPanelBottomBase.ResumeLayout(false);
            this.tPanelBottomBase.PerformLayout();
            this.tPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitA)).EndInit();
            this.splitA.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitA1A)).EndInit();
            this.SplitA1A.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.SplitContainerControl splitA;
        public DevExpress.XtraEditors.SplitContainerControl SplitA1A;
    }
}