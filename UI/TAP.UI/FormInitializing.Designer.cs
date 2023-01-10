namespace TAP.UI
{
    partial class FormInitializing
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
            this.lblComment = new TAP.UIControls.BasicControls.TLabel();
            this.tLabel1 = new TAP.UIControls.BasicControls.TLabel();
            this.lblCurrent = new TAP.UIControls.BasicControls.TLabel();
            this.lblTotal = new TAP.UIControls.BasicControls.TLabel();
            this.tLabel3 = new TAP.UIControls.BasicControls.TLabel();
            this.progressBar1 = new TAP.UIControls.BasicControls.TSolidProgressBar();
            this.SuspendLayout();
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.BackColor = System.Drawing.Color.Transparent;
            this.lblComment.ControlID = "lblComment";
            this.lblComment.ForeColor = System.Drawing.Color.White;
            this.lblComment.IsRequired = false;
            this.lblComment.Location = new System.Drawing.Point(182, 38);
            this.lblComment.Name = "lblComment";
            this.lblComment.NeedToTranslate = true;
            this.lblComment.RepresentativeValue = "";
            this.lblComment.Size = new System.Drawing.Size(0, 13);
            this.lblComment.TabIndex = 15;
            // 
            // tLabel1
            // 
            this.tLabel1.AutoSize = true;
            this.tLabel1.BackColor = System.Drawing.Color.Transparent;
            this.tLabel1.ControlID = "tLabel1";
            this.tLabel1.ForeColor = System.Drawing.Color.White;
            this.tLabel1.IsRequired = false;
            this.tLabel1.Location = new System.Drawing.Point(12, 9);
            this.tLabel1.Name = "tLabel1";
            this.tLabel1.NeedToTranslate = true;
            this.tLabel1.RepresentativeValue = "Now Initializing";
            this.tLabel1.Size = new System.Drawing.Size(78, 13);
            this.tLabel1.TabIndex = 14;
            this.tLabel1.Text = "Now Initializing";
            // 
            // lblCurrent
            // 
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrent.ControlID = "lblCurrent";
            this.lblCurrent.ForeColor = System.Drawing.Color.White;
            this.lblCurrent.IsRequired = false;
            this.lblCurrent.Location = new System.Drawing.Point(356, 9);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.NeedToTranslate = true;
            this.lblCurrent.RepresentativeValue = "";
            this.lblCurrent.Size = new System.Drawing.Size(0, 13);
            this.lblCurrent.TabIndex = 13;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblTotal.ControlID = "lblTotal";
            this.lblTotal.ForeColor = System.Drawing.Color.White;
            this.lblTotal.IsRequired = false;
            this.lblTotal.Location = new System.Drawing.Point(362, 9);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.NeedToTranslate = true;
            this.lblTotal.RepresentativeValue = "";
            this.lblTotal.Size = new System.Drawing.Size(0, 13);
            this.lblTotal.TabIndex = 0;
            // 
            // tLabel3
            // 
            this.tLabel3.BackColor = System.Drawing.Color.Transparent;
            this.tLabel3.ControlID = "tLabel2";
            this.tLabel3.ForeColor = System.Drawing.Color.White;
            this.tLabel3.IsRequired = false;
            this.tLabel3.Location = new System.Drawing.Point(0, 63);
            this.tLabel3.Name = "tLabel3";
            this.tLabel3.NeedToTranslate = true;
            this.tLabel3.RepresentativeValue = "TAP FX CIM Launcher";
            this.tLabel3.Size = new System.Drawing.Size(400, 12);
            this.tLabel3.TabIndex = 17;
            this.tLabel3.Text = "TAP FX CIM Launcher";
            this.tLabel3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // progressBar1
            // 
            this.progressBar1.BarColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.progressBar1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(74)))));
            this.progressBar1.FillStyle = TAP.UIControls.BasicControls.TSolidProgressBar.FillStyles.Solid;
            this.progressBar1.Location = new System.Drawing.Point(3, 25);
            this.progressBar1.Maximum = 100;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(390, 10);
            this.progressBar1.Step = 10;
            this.progressBar1.TabIndex = 18;
            this.progressBar1.Value = 0;
            // 
            // FormInitializing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(74)))));
            this.ClientSize = new System.Drawing.Size(400, 90);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.tLabel3);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.tLabel1);
            this.Controls.Add(this.lblCurrent);
            this.Controls.Add(this.lblTotal);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormInitializing";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Processing";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UIControls.BasicControls.TLabel lblTotal;
        private UIControls.BasicControls.TLabel lblCurrent;
        private UIControls.BasicControls.TLabel tLabel1;
        private UIControls.BasicControls.TLabel lblComment;
        private UIControls.BasicControls.TLabel tLabel3;
        private UIControls.BasicControls.TSolidProgressBar progressBar1;
    }
}