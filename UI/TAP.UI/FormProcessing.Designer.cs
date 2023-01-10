namespace TAP.UI
{
    partial class FormProcessing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcessing));
            this.lblTotal = new TAP.UIControls.BasicControls.TLabel();
            this.tButton1 = new TAP.UIControls.BasicControls.TButton();
            this.btnClose = new TAP.UIControls.BasicControls.TButton();
            this.lblCurrent = new TAP.UIControls.BasicControls.TLabel();
            this.tLabel1 = new TAP.UIControls.BasicControls.TLabel();
            this.lblComment = new TAP.UIControls.BasicControls.TLabel();
            this.lblPercent = new TAP.UIControls.BasicControls.TLabel();
            this.progressBar1 = new TAP.UIControls.BasicControls.TSolidProgressBar();
            this.SuspendLayout();
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.ControlID = "lblTotal";
            this.lblTotal.ForeColor = System.Drawing.Color.White;
            this.lblTotal.IsRequired = false;
            this.lblTotal.Location = new System.Drawing.Point(374, 9);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.NeedToTranslate = true;
            this.lblTotal.RepresentativeValue = "";
            this.lblTotal.Size = new System.Drawing.Size(0, 13);
            this.lblTotal.TabIndex = 0;
            // 
            // tButton1
            // 
            this.tButton1.BackColor = System.Drawing.Color.Transparent;
            this.tButton1.CommandType = TAP.UIControls.BasicControls.EnumCommandType.OTHERS;
            this.tButton1.ControlID = "tButton1";
            this.tButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tButton1.Image = ((System.Drawing.Image)(resources.GetObject("tButton1.Image")));
            this.tButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tButton1.IsRequired = false;
            this.tButton1.Location = new System.Drawing.Point(141, 203);
            this.tButton1.Name = "tButton1";
            this.tButton1.NeedToTranslate = true;
            this.tButton1.RepresentativeValue = "    tButton1";
            this.tButton1.Size = new System.Drawing.Size(64, 27);
            this.tButton1.TabIndex = 1;
            this.tButton1.Text = "    tButton1";
            this.tButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tButton1.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.CommandType = TAP.UIControls.BasicControls.EnumCommandType.CLOSE;
            this.btnClose.ControlID = "buttonLoad";
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(74)))));
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.IsRequired = false;
            this.btnClose.Location = new System.Drawing.Point(380, 25);
            this.btnClose.Name = "btnClose";
            this.btnClose.NeedToTranslate = true;
            this.btnClose.RepresentativeValue = "            ";
            this.btnClose.Size = new System.Drawing.Size(25, 25);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "            ";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblCurrent
            // 
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.ControlID = "lblCurrent";
            this.lblCurrent.ForeColor = System.Drawing.Color.White;
            this.lblCurrent.IsRequired = false;
            this.lblCurrent.Location = new System.Drawing.Point(359, 9);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.NeedToTranslate = true;
            this.lblCurrent.RepresentativeValue = "";
            this.lblCurrent.Size = new System.Drawing.Size(0, 13);
            this.lblCurrent.TabIndex = 13;
            // 
            // tLabel1
            // 
            this.tLabel1.AutoSize = true;
            this.tLabel1.ControlID = "tLabel1";
            this.tLabel1.ForeColor = System.Drawing.Color.White;
            this.tLabel1.IsRequired = false;
            this.tLabel1.Location = new System.Drawing.Point(13, 9);
            this.tLabel1.Name = "tLabel1";
            this.tLabel1.NeedToTranslate = true;
            this.tLabel1.RepresentativeValue = "Now Processing";
            this.tLabel1.Size = new System.Drawing.Size(82, 13);
            this.tLabel1.TabIndex = 14;
            this.tLabel1.Text = "Now Processing";
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.ControlID = "lblComment";
            this.lblComment.ForeColor = System.Drawing.Color.White;
            this.lblComment.IsRequired = false;
            this.lblComment.Location = new System.Drawing.Point(16, 55);
            this.lblComment.Name = "lblComment";
            this.lblComment.NeedToTranslate = true;
            this.lblComment.RepresentativeValue = "";
            this.lblComment.Size = new System.Drawing.Size(0, 13);
            this.lblComment.TabIndex = 15;
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.ControlID = "lblPercent";
            this.lblPercent.ForeColor = System.Drawing.Color.White;
            this.lblPercent.IsRequired = false;
            this.lblPercent.Location = new System.Drawing.Point(359, 55);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.NeedToTranslate = true;
            this.lblPercent.RepresentativeValue = "";
            this.lblPercent.Size = new System.Drawing.Size(0, 13);
            this.lblPercent.TabIndex = 16;
            // 
            // progressBar1
            // 
            this.progressBar1.BarColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.progressBar1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(74)))));
            this.progressBar1.FillStyle = TAP.UIControls.BasicControls.TSolidProgressBar.FillStyles.Solid;
            this.progressBar1.Location = new System.Drawing.Point(12, 25);
            this.progressBar1.Maximum = 100;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(362, 10);
            this.progressBar1.Step = 10;
            this.progressBar1.TabIndex = 19;
            this.progressBar1.Value = 0;
            // 
            // FormProcessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(74)))));
            this.ClientSize = new System.Drawing.Size(414, 90);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblPercent);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.tLabel1);
            this.Controls.Add(this.lblCurrent);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tButton1);
            this.Controls.Add(this.lblTotal);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormProcessing";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Processing";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UIControls.BasicControls.TLabel lblTotal;
        private UIControls.BasicControls.TButton tButton1;
        private UIControls.BasicControls.TButton btnClose;
        private UIControls.BasicControls.TLabel lblCurrent;
        private UIControls.BasicControls.TLabel tLabel1;
        private UIControls.BasicControls.TLabel lblComment;
        private UIControls.BasicControls.TLabel lblPercent;
        private UIControls.BasicControls.TSolidProgressBar progressBar1;
    }
}