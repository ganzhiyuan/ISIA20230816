namespace ISIA.UI.BASE
{
    partial class PopupMemo
    {
        /// <summary> 
        /// ?? ???? ?????.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// ?? ?? ?? ???? ?????.
        /// </summary>
        /// <param name="disposing">???? ???? ???? ?? true??, ??? ??? false???.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region ?? ?? ?????? ??? ??

        /// <summary> 
        /// ???? ??? ??? ??????. 
        /// ? ???? ??? ?? ???? ???? ???.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.memoEdit = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.memoEdit);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "Office 2013";
            this.panelControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(643, 473);
            this.panelControl1.TabIndex = 0;
            // 
            // memoEdit
            // 
            this.memoEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoEdit.Location = new System.Drawing.Point(2, 2);
            this.memoEdit.Name = "memoEdit";
            this.memoEdit.Size = new System.Drawing.Size(639, 469);
            this.memoEdit.TabIndex = 0;
            // 
            // PopupMemo
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 473);
            this.Controls.Add(this.panelControl1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IconOptions.ShowIcon = false;
            this.LookAndFeel.SkinName = "Office 2013";
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "PopupMemo";
            this.ShowInTaskbar = false;
            this.Text = "Error Message";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.MemoEdit memoEdit;
    }
}
