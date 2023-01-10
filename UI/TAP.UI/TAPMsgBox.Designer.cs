namespace TAP.UI
{
    partial class TAPMsgBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TAPMsgBox));
            this.tapPanel4 = new TAP.UIControls.BasicControls.TPanel();
            this.richTextBoxDetail = new System.Windows.Forms.RichTextBox();
            this.tapPanel3 = new TAP.UIControls.BasicControls.TPanel();
            this.tapButtonDetail = new TAP.UIControls.BasicControls.TButton();
            this.tapButtonYes = new TAP.UIControls.BasicControls.TButton();
            this.tapButtonNo = new TAP.UIControls.BasicControls.TButton();
            this.tapButtonCancel = new TAP.UIControls.BasicControls.TButton();
            this.tapButtonOK = new TAP.UIControls.BasicControls.TButton();
            this.tapPanel2 = new TAP.UIControls.BasicControls.TPanel();
            this.richTextBoxMessage = new System.Windows.Forms.RichTextBox();
            this.tapPanel1 = new TAP.UIControls.BasicControls.TPanel();
            this.tapPictureBox1 = new TAP.UIControls.BasicControls.TPictureBox();
            this.tapPanel4.SuspendLayout();
            this.tapPanel3.SuspendLayout();
            this.tapPanel2.SuspendLayout();
            this.tapPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tapPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tapPanel4
            // 
            this.tapPanel4.ControlID = "tapPanel4";
            this.tapPanel4.Controls.Add(this.richTextBoxDetail);
            this.tapPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tapPanel4.IsRequired = false;
            this.tapPanel4.Location = new System.Drawing.Point(0, 150);
            this.tapPanel4.Name = "tapPanel4";
            this.tapPanel4.NeedToTranslate = true;
            this.tapPanel4.RepresentativeValue = "tapPanel4 [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Form" +
    "s.BorderStyle.None";
            this.tapPanel4.Size = new System.Drawing.Size(492, 183);
            this.tapPanel4.TabIndex = 3;
            this.tapPanel4.Visible = false;
            // 
            // richTextBoxDetail
            // 
            this.richTextBoxDetail.BackColor = System.Drawing.Color.WhiteSmoke;
            this.richTextBoxDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxDetail.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxDetail.Name = "richTextBoxDetail";
            this.richTextBoxDetail.ReadOnly = true;
            this.richTextBoxDetail.Size = new System.Drawing.Size(492, 183);
            this.richTextBoxDetail.TabIndex = 0;
            this.richTextBoxDetail.Text = "";
            // 
            // tapPanel3
            // 
            this.tapPanel3.ControlID = "tapPanel3";
            this.tapPanel3.Controls.Add(this.tapButtonDetail);
            this.tapPanel3.Controls.Add(this.tapButtonYes);
            this.tapPanel3.Controls.Add(this.tapButtonNo);
            this.tapPanel3.Controls.Add(this.tapButtonCancel);
            this.tapPanel3.Controls.Add(this.tapButtonOK);
            this.tapPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tapPanel3.IsRequired = false;
            this.tapPanel3.Location = new System.Drawing.Point(0, 333);
            this.tapPanel3.Name = "tapPanel3";
            this.tapPanel3.NeedToTranslate = true;
            this.tapPanel3.RepresentativeValue = "tapPanel3 [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Form" +
    "s.BorderStyle.None";
            this.tapPanel3.Size = new System.Drawing.Size(492, 40);
            this.tapPanel3.TabIndex = 2;
            // 
            // tapButtonDetail
            // 
            this.tapButtonDetail.BackColor = System.Drawing.Color.Transparent;
            this.tapButtonDetail.CommandType = TAP.UIControls.BasicControls.EnumCommandType.VIEW;
            this.tapButtonDetail.ControlID = "tapButton2";
            this.tapButtonDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tapButtonDetail.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tapButtonDetail.Image = ((System.Drawing.Image)(resources.GetObject("tapButtonDetail.Image")));
            this.tapButtonDetail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tapButtonDetail.IsRequired = false;
            this.tapButtonDetail.Location = new System.Drawing.Point(196, 0);
            this.tapButtonDetail.Name = "tapButtonDetail";
            this.tapButtonDetail.NeedToTranslate = true;
            this.tapButtonDetail.RepresentativeValue = "      Detail";
            this.tapButtonDetail.Size = new System.Drawing.Size(100, 25);
            this.tapButtonDetail.TabIndex = 5;
            this.tapButtonDetail.Text = "      Detail";
            this.tapButtonDetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tapButtonDetail.UseVisualStyleBackColor = false;
            this.tapButtonDetail.Click += new System.EventHandler(this.tapButtonDetail_Click);
            // 
            // tapButtonYes
            // 
            this.tapButtonYes.BackColor = System.Drawing.Color.Transparent;
            this.tapButtonYes.CommandType = TAP.UIControls.BasicControls.EnumCommandType.CONFIRM;
            this.tapButtonYes.ControlID = "tapButton2";
            this.tapButtonYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tapButtonYes.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tapButtonYes.Image = ((System.Drawing.Image)(resources.GetObject("tapButtonYes.Image")));
            this.tapButtonYes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tapButtonYes.IsRequired = false;
            this.tapButtonYes.Location = new System.Drawing.Point(3, 3);
            this.tapButtonYes.Name = "tapButtonYes";
            this.tapButtonYes.NeedToTranslate = true;
            this.tapButtonYes.RepresentativeValue = "         Yes";
            this.tapButtonYes.Size = new System.Drawing.Size(100, 25);
            this.tapButtonYes.TabIndex = 4;
            this.tapButtonYes.Text = "         Yes";
            this.tapButtonYes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tapButtonYes.UseVisualStyleBackColor = false;
            this.tapButtonYes.Click += new System.EventHandler(this.tapButtonYes_Click);
            // 
            // tapButtonNo
            // 
            this.tapButtonNo.BackColor = System.Drawing.Color.Transparent;
            this.tapButtonNo.CommandType = TAP.UIControls.BasicControls.EnumCommandType.CANCEL;
            this.tapButtonNo.ControlID = "tapButton2";
            this.tapButtonNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tapButtonNo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tapButtonNo.Image = ((System.Drawing.Image)(resources.GetObject("tapButtonNo.Image")));
            this.tapButtonNo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tapButtonNo.IsRequired = false;
            this.tapButtonNo.Location = new System.Drawing.Point(129, 0);
            this.tapButtonNo.Name = "tapButtonNo";
            this.tapButtonNo.NeedToTranslate = true;
            this.tapButtonNo.RepresentativeValue = "          No";
            this.tapButtonNo.Size = new System.Drawing.Size(100, 25);
            this.tapButtonNo.TabIndex = 3;
            this.tapButtonNo.Text = "          No";
            this.tapButtonNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tapButtonNo.UseVisualStyleBackColor = false;
            this.tapButtonNo.Click += new System.EventHandler(this.tapButtonNo_Click);
            // 
            // tapButtonCancel
            // 
            this.tapButtonCancel.BackColor = System.Drawing.Color.Transparent;
            this.tapButtonCancel.CommandType = TAP.UIControls.BasicControls.EnumCommandType.CANCEL;
            this.tapButtonCancel.ControlID = "tapButtonCancel";
            this.tapButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tapButtonCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tapButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("tapButtonCancel.Image")));
            this.tapButtonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tapButtonCancel.IsRequired = false;
            this.tapButtonCancel.Location = new System.Drawing.Point(274, 3);
            this.tapButtonCancel.Name = "tapButtonCancel";
            this.tapButtonCancel.NeedToTranslate = true;
            this.tapButtonCancel.RepresentativeValue = "      Cancel";
            this.tapButtonCancel.Size = new System.Drawing.Size(100, 25);
            this.tapButtonCancel.TabIndex = 1;
            this.tapButtonCancel.Text = "      Cancel";
            this.tapButtonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tapButtonCancel.UseVisualStyleBackColor = false;
            this.tapButtonCancel.Click += new System.EventHandler(this.tapButtonCancel_Click);
            // 
            // tapButtonOK
            // 
            this.tapButtonOK.BackColor = System.Drawing.Color.Transparent;
            this.tapButtonOK.CommandType = TAP.UIControls.BasicControls.EnumCommandType.CONFIRM;
            this.tapButtonOK.ControlID = "tapButtonOK";
            this.tapButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tapButtonOK.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tapButtonOK.Image = ((System.Drawing.Image)(resources.GetObject("tapButtonOK.Image")));
            this.tapButtonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tapButtonOK.IsRequired = false;
            this.tapButtonOK.Location = new System.Drawing.Point(380, 7);
            this.tapButtonOK.Name = "tapButtonOK";
            this.tapButtonOK.NeedToTranslate = true;
            this.tapButtonOK.RepresentativeValue = "          OK";
            this.tapButtonOK.Size = new System.Drawing.Size(100, 25);
            this.tapButtonOK.TabIndex = 0;
            this.tapButtonOK.Text = "          OK";
            this.tapButtonOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tapButtonOK.UseVisualStyleBackColor = false;
            this.tapButtonOK.Click += new System.EventHandler(this.tapButtonOK_Click);
            // 
            // tapPanel2
            // 
            this.tapPanel2.ControlID = "tapPanel2";
            this.tapPanel2.Controls.Add(this.richTextBoxMessage);
            this.tapPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tapPanel2.IsRequired = false;
            this.tapPanel2.Location = new System.Drawing.Point(0, 50);
            this.tapPanel2.Name = "tapPanel2";
            this.tapPanel2.NeedToTranslate = true;
            this.tapPanel2.RepresentativeValue = "tapPanel2 [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Form" +
    "s.BorderStyle.None";
            this.tapPanel2.Size = new System.Drawing.Size(492, 100);
            this.tapPanel2.TabIndex = 1;
            // 
            // richTextBoxMessage
            // 
            this.richTextBoxMessage.BackColor = System.Drawing.Color.White;
            this.richTextBoxMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxMessage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxMessage.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxMessage.Name = "richTextBoxMessage";
            this.richTextBoxMessage.ReadOnly = true;
            this.richTextBoxMessage.Size = new System.Drawing.Size(492, 100);
            this.richTextBoxMessage.TabIndex = 0;
            this.richTextBoxMessage.Text = "";
            // 
            // tapPanel1
            // 
            this.tapPanel1.ControlID = "tapPanel1";
            this.tapPanel1.Controls.Add(this.tapPictureBox1);
            this.tapPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tapPanel1.IsRequired = false;
            this.tapPanel1.Location = new System.Drawing.Point(0, 0);
            this.tapPanel1.Name = "tapPanel1";
            this.tapPanel1.NeedToTranslate = true;
            this.tapPanel1.RepresentativeValue = "tapPanel1 [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Form" +
    "s.BorderStyle.None";
            this.tapPanel1.Size = new System.Drawing.Size(492, 50);
            this.tapPanel1.TabIndex = 0;
            // 
            // tapPictureBox1
            // 
            this.tapPictureBox1.ControlID = "tapPictureBox1";
            this.tapPictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tapPictureBox1.IsRequired = false;
            this.tapPictureBox1.Location = new System.Drawing.Point(0, 0);
            this.tapPictureBox1.Name = "tapPictureBox1";
            this.tapPictureBox1.NeedToTranslate = true;
            this.tapPictureBox1.RepresentativeValue = null;
            this.tapPictureBox1.RollOverImage = null;
            this.tapPictureBox1.Size = new System.Drawing.Size(492, 50);
            this.tapPictureBox1.TabIndex = 0;
            this.tapPictureBox1.TabStop = false;
            // 
            // TAPMsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(492, 373);
            this.Controls.Add(this.tapPanel4);
            this.Controls.Add(this.tapPanel3);
            this.Controls.Add(this.tapPanel2);
            this.Controls.Add(this.tapPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TAPMsgBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TAPMsgBox";
            this.tapPanel4.ResumeLayout(false);
            this.tapPanel3.ResumeLayout(false);
            this.tapPanel2.ResumeLayout(false);
            this.tapPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tapPictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TAP.UIControls.BasicControls.TPanel tapPanel1;
        private TAP.UIControls.BasicControls.TPanel tapPanel2;
        private TAP.UIControls.BasicControls.TPanel tapPanel3;
        private TAP.UIControls.BasicControls.TPictureBox tapPictureBox1;
        private System.Windows.Forms.RichTextBox richTextBoxMessage;
        private TAP.UIControls.BasicControls.TButton tapButtonYes;
        private TAP.UIControls.BasicControls.TButton tapButtonNo;
        private TAP.UIControls.BasicControls.TButton tapButtonCancel;
        private TAP.UIControls.BasicControls.TButton tapButtonOK;
        private TAP.UIControls.BasicControls.TPanel tapPanel4;
        private System.Windows.Forms.RichTextBox richTextBoxDetail;
        private TAP.UIControls.BasicControls.TButton tapButtonDetail;
    }
}