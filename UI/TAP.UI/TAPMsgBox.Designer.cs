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
            this.richTextBoxDetail = new DevExpress.XtraRichEdit.RichEditControl();
            this.tapPanel3 = new TAP.UIControls.BasicControls.TPanel();
            this.tapButtonOK = new DevExpress.XtraEditors.SimpleButton();
            this.tapButtonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.tapButtonDetail = new DevExpress.XtraEditors.SimpleButton();
            this.tapButtonNo = new DevExpress.XtraEditors.SimpleButton();
            this.tapButtonYes = new DevExpress.XtraEditors.SimpleButton();
            this.tapPanel2 = new TAP.UIControls.BasicControls.TPanel();
            this.richTextBoxMessage = new DevExpress.XtraRichEdit.RichEditControl();
            this.tapPanel1 = new TAP.UIControls.BasicControls.TPanel();
            this.tapPictureBox1 = new DevExpress.XtraEditors.PictureEdit();
            this.tapPanel4.SuspendLayout();
            this.tapPanel3.SuspendLayout();
            this.tapPanel2.SuspendLayout();
            this.tapPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tapPictureBox1.Properties)).BeginInit();
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
            this.richTextBoxDetail.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple;
            this.richTextBoxDetail.Appearance.Text.BackColor = System.Drawing.Color.WhiteSmoke;
            this.richTextBoxDetail.Appearance.Text.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.richTextBoxDetail.Appearance.Text.Options.UseBackColor = true;
            this.richTextBoxDetail.Appearance.Text.Options.UseFont = true;
            this.richTextBoxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxDetail.LayoutUnit = DevExpress.XtraRichEdit.DocumentLayoutUnit.Pixel;
            this.richTextBoxDetail.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxDetail.Name = "richTextBoxDetail";
            this.richTextBoxDetail.Options.DocumentSaveOptions.CurrentFormat = DevExpress.XtraRichEdit.DocumentFormat.PlainText;
            this.richTextBoxDetail.ReadOnly = true;
            this.richTextBoxDetail.Size = new System.Drawing.Size(492, 183);
            this.richTextBoxDetail.TabIndex = 1;
            this.richTextBoxDetail.Views.SimpleView.BackColor = System.Drawing.Color.WhiteSmoke;
            // 
            // tapPanel3
            // 
            this.tapPanel3.ControlID = "tapPanel3";
            this.tapPanel3.Controls.Add(this.tapButtonOK);
            this.tapPanel3.Controls.Add(this.tapButtonCancel);
            this.tapPanel3.Controls.Add(this.tapButtonDetail);
            this.tapPanel3.Controls.Add(this.tapButtonNo);
            this.tapPanel3.Controls.Add(this.tapButtonYes);
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
            // tapButtonOK
            // 
            this.tapButtonOK.ImageOptions.Image = global::TAP.UI.Properties.Resources.apply_16x16;
            this.tapButtonOK.Location = new System.Drawing.Point(428, 5);
            this.tapButtonOK.Name = "tapButtonOK";
            this.tapButtonOK.Size = new System.Drawing.Size(75, 23);
            this.tapButtonOK.TabIndex = 9;
            this.tapButtonOK.Text = "OK";
            this.tapButtonOK.Click += new System.EventHandler(this.tapButtonOK_Click);
            // 
            // tapButtonCancel
            // 
            this.tapButtonCancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("tapButtonCancel.ImageOptions.Image")));
            this.tapButtonCancel.Location = new System.Drawing.Point(321, 5);
            this.tapButtonCancel.Name = "tapButtonCancel";
            this.tapButtonCancel.Size = new System.Drawing.Size(100, 25);
            this.tapButtonCancel.TabIndex = 8;
            this.tapButtonCancel.Text = "Cancel";
            this.tapButtonCancel.Click += new System.EventHandler(this.tapButtonCancel_Click);
            // 
            // tapButtonDetail
            // 
            this.tapButtonDetail.ImageOptions.Image = global::TAP.UI.Properties.Resources.listbox_16x16;
            this.tapButtonDetail.Location = new System.Drawing.Point(215, 4);
            this.tapButtonDetail.Name = "tapButtonDetail";
            this.tapButtonDetail.Size = new System.Drawing.Size(100, 25);
            this.tapButtonDetail.TabIndex = 7;
            this.tapButtonDetail.Text = "Detail";
            this.tapButtonDetail.Click += new System.EventHandler(this.tapButtonDetail_Click);
            // 
            // tapButtonNo
            // 
            this.tapButtonNo.ImageOptions.Image = global::TAP.UI.Properties.Resources.cancel_16x162;
            this.tapButtonNo.Location = new System.Drawing.Point(109, 6);
            this.tapButtonNo.Name = "tapButtonNo";
            this.tapButtonNo.Size = new System.Drawing.Size(100, 25);
            this.tapButtonNo.TabIndex = 6;
            this.tapButtonNo.Text = "No";
            this.tapButtonNo.Click += new System.EventHandler(this.tapButtonNo_Click);
            // 
            // tapButtonYes
            // 
            this.tapButtonYes.ImageOptions.Image = global::TAP.UI.Properties.Resources.iconsetsymbols3_16x16;
            this.tapButtonYes.Location = new System.Drawing.Point(3, 5);
            this.tapButtonYes.Name = "tapButtonYes";
            this.tapButtonYes.Size = new System.Drawing.Size(100, 25);
            this.tapButtonYes.TabIndex = 5;
            this.tapButtonYes.Text = "Yes";
            this.tapButtonYes.Click += new System.EventHandler(this.tapButtonYes_Click);
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
            this.richTextBoxMessage.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple;
            this.richTextBoxMessage.Appearance.Text.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.richTextBoxMessage.Appearance.Text.Options.UseFont = true;
            this.richTextBoxMessage.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.richTextBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxMessage.LayoutUnit = DevExpress.XtraRichEdit.DocumentLayoutUnit.Pixel;
            this.richTextBoxMessage.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxMessage.Name = "richTextBoxMessage";
            this.richTextBoxMessage.Options.DocumentSaveOptions.CurrentFormat = DevExpress.XtraRichEdit.DocumentFormat.PlainText;
            this.richTextBoxMessage.Options.VerticalScrollbar.Visibility = DevExpress.XtraRichEdit.RichEditScrollbarVisibility.Hidden;
            this.richTextBoxMessage.ReadOnly = true;
            this.richTextBoxMessage.Size = new System.Drawing.Size(492, 100);
            this.richTextBoxMessage.TabIndex = 1;
            this.richTextBoxMessage.Views.SimpleView.BackColor = System.Drawing.Color.White;
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
            this.tapPictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tapPictureBox1.Location = new System.Drawing.Point(0, 0);
            this.tapPictureBox1.Name = "tapPictureBox1";
            this.tapPictureBox1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.tapPictureBox1.Properties.NullText = " ";
            this.tapPictureBox1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.tapPictureBox1.Size = new System.Drawing.Size(492, 50);
            this.tapPictureBox1.TabIndex = 1;
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TAPMsgBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TAPMsgBox";
            this.tapPanel4.ResumeLayout(false);
            this.tapPanel3.ResumeLayout(false);
            this.tapPanel2.ResumeLayout(false);
            this.tapPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tapPictureBox1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TAP.UIControls.BasicControls.TPanel tapPanel1;
        private TAP.UIControls.BasicControls.TPanel tapPanel2;
        private TAP.UIControls.BasicControls.TPanel tapPanel3;
        private TAP.UIControls.BasicControls.TPanel tapPanel4;
        private DevExpress.XtraRichEdit.RichEditControl richTextBoxMessage;
        private DevExpress.XtraRichEdit.RichEditControl richTextBoxDetail;
        private DevExpress.XtraEditors.PictureEdit tapPictureBox1;
        private DevExpress.XtraEditors.SimpleButton tapButtonCancel;
        private DevExpress.XtraEditors.SimpleButton tapButtonDetail;
        private DevExpress.XtraEditors.SimpleButton tapButtonNo;
        private DevExpress.XtraEditors.SimpleButton tapButtonYes;
        private DevExpress.XtraEditors.SimpleButton tapButtonOK;
    }
}