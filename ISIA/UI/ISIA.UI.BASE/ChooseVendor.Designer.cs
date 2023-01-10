namespace ISIA.UI.BASE
{
    partial class ChooseVendor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseVendor));
            this.tPanel1 = new TAP.UIControls.BasicControlsDEV.TPanel();
            this.txtVendor = new TAP.UIControls.BasicControlsDEV.TTextBox();
            this.tLabel2 = new TAP.UIControls.BasicControlsDEV.TLabel();
            this.tLabel1 = new TAP.UIControls.BasicControlsDEV.TLabel();
            this.cboVendor = new TAP.UIControls.BasicControlsDEV.TComboBox();
            this.btnOK = new TAP.UIControls.BasicControlsDEV.TButton();
            this.btnCancel = new TAP.UIControls.BasicControlsDEV.TButton();
            ((System.ComponentModel.ISupportInitialize)(this.tPanel1)).BeginInit();
            this.tPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboVendor.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tPanel1
            // 
            this.tPanel1.Appearance.BackColor = System.Drawing.Color.White;
            this.tPanel1.Appearance.BackColor2 = System.Drawing.Color.White;
            this.tPanel1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tPanel1.Appearance.Options.UseBackColor = true;
            this.tPanel1.Appearance.Options.UseFont = true;
            this.tPanel1.ControlID = "tPanel1";
            this.tPanel1.Controls.Add(this.txtVendor);
            this.tPanel1.Controls.Add(this.tLabel2);
            this.tPanel1.Controls.Add(this.tLabel1);
            this.tPanel1.Controls.Add(this.cboVendor);
            this.tPanel1.IsRequired = false;
            this.tPanel1.Location = new System.Drawing.Point(12, 11);
            this.tPanel1.LookAndFeel.SkinMaskColor = System.Drawing.Color.White;
            this.tPanel1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.tPanel1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tPanel1.Name = "tPanel1";
            this.tPanel1.NeedToTranslate = true;
            this.tPanel1.RepresentativeValue = "tPanel1 [TAP.UIControls.BasicControlsDEV.TPanel]";
            this.tPanel1.Size = new System.Drawing.Size(259, 142);
            this.tPanel1.TabIndex = 31;
            // 
            // txtVendor
            // 
            this.txtVendor.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.txtVendor.ControlID = "txtPM";
            this.txtVendor.EditValue = "";
            this.txtVendor.Enabled = false;
            this.txtVendor.IsRequired = false;
            this.txtVendor.Location = new System.Drawing.Point(117, 95);
            this.txtVendor.Name = "txtVendor";
            this.txtVendor.NeedToTranslate = false;
            this.txtVendor.Properties.Appearance.BackColor = System.Drawing.Color.MistyRose;
            this.txtVendor.Properties.Appearance.Options.UseBackColor = true;
            this.txtVendor.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVendor.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtVendor.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.txtVendor.RepresentativeValue = "";
            this.txtVendor.Size = new System.Drawing.Size(123, 20);
            this.txtVendor.TabIndex = 3;
            // 
            // tLabel2
            // 
            this.tLabel2.ControlID = "tLabel2";
            this.tLabel2.IsRequired = false;
            this.tLabel2.Location = new System.Drawing.Point(14, 70);
            this.tLabel2.Name = "tLabel2";
            this.tLabel2.NeedToTranslate = true;
            this.tLabel2.RepresentativeValue = "You choosed Vendor is:";
            this.tLabel2.Size = new System.Drawing.Size(131, 14);
            this.tLabel2.TabIndex = 2;
            this.tLabel2.Text = "You choosed Vendor is:";
            // 
            // tLabel1
            // 
            this.tLabel1.ControlID = "tLabel1";
            this.tLabel1.IsRequired = false;
            this.tLabel1.Location = new System.Drawing.Point(15, 23);
            this.tLabel1.Name = "tLabel1";
            this.tLabel1.NeedToTranslate = true;
            this.tLabel1.RepresentativeValue = "Vendor";
            this.tLabel1.Size = new System.Drawing.Size(40, 14);
            this.tLabel1.TabIndex = 1;
            this.tLabel1.Text = "Vendor";
            // 
            // cboVendor
            // 
            this.cboVendor.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.cboVendor.BuiltInCategory = "";
            this.cboVendor.BuiltInDispay = TAP.UIControls.EnumBuitInDispay.NONE;
            this.cboVendor.BuiltInIncludeAll = false;
            this.cboVendor.BuiltInSubCategory = "";
            this.cboVendor.ControlID = "cboPMSchedule";
            this.cboVendor.DisplayMember = null;
            this.cboVendor.DominantControl = "";
            this.cboVendor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboVendor.DropDownHeight = 100;
            this.cboVendor.EditValue = "";
            this.cboVendor.IsColorData = false;
            this.cboVendor.IsRequired = false;
            this.cboVendor.Location = new System.Drawing.Point(117, 20);
            this.cboVendor.Name = "cboVendor";
            this.cboVendor.NeedToTranslate = true;
            this.cboVendor.ParameterID = "VENDOR";
            this.cboVendor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboVendor.RepresentativeValue = null;
            this.cboVendor.Size = new System.Drawing.Size(123, 20);
            this.cboVendor.Sql = "ISIA.BIZ.COMMON.ComboBoxData.GetVendor";
            this.cboVendor.SqlDataBase = "";
            this.cboVendor.SqlType = TAP.UIControls.EnumSqlType.Biz;
            this.cboVendor.TabIndex = 0;
            this.cboVendor.ValueMember = null;
            this.cboVendor.SelectedIndexChanged += new System.EventHandler(this.cboVendor_SelectedIndexChanged);
            // 
            // btnOK
            // 
            this.btnOK.Appearance.BackColor = System.Drawing.Color.White;
            this.btnOK.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.btnOK.Appearance.Options.UseBackColor = true;
            this.btnOK.Appearance.Options.UseBorderColor = true;
            this.btnOK.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.btnOK.AppearanceHovered.Options.UseBackColor = true;
            this.btnOK.AppearancePressed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.btnOK.AppearancePressed.Options.UseBackColor = true;
            this.btnOK.CommandType = TAP.UIControls.BasicControlsDEV.EnumCommandType.CONFIRM;
            this.btnOK.ControlID = "btnInstall";
            this.btnOK.FlatStyle = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.btnOK.IconColorType = TAP.UIControls.BasicControlsDEV.EnumColorType.BLACK;
            this.btnOK.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnInstall.ImageOptions.Image")));
            this.btnOK.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnOK.IsRequired = false;
            this.btnOK.Location = new System.Drawing.Point(112, 167);
            this.btnOK.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.btnOK.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnOK.Name = "btnOK";
            this.btnOK.NeedToTranslate = true;
            this.btnOK.RepresentativeValue = "  OK";
            this.btnOK.Size = new System.Drawing.Size(75, 22);
            this.btnOK.TabIndex = 32;
            this.btnOK.Text = "  OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.BackColor = System.Drawing.Color.White;
            this.btnCancel.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.btnCancel.Appearance.Options.UseBackColor = true;
            this.btnCancel.Appearance.Options.UseBorderColor = true;
            this.btnCancel.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.btnCancel.AppearanceHovered.Options.UseBackColor = true;
            this.btnCancel.AppearancePressed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.btnCancel.AppearancePressed.Options.UseBackColor = true;
            this.btnCancel.CommandType = TAP.UIControls.BasicControlsDEV.EnumCommandType.CANCEL;
            this.btnCancel.ControlID = "btnInstall";
            this.btnCancel.FlatStyle = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.btnCancel.IconColorType = TAP.UIControls.BasicControlsDEV.EnumColorType.BLACK;
            this.btnCancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageOptions.Image")));
            this.btnCancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnCancel.IsRequired = false;
            this.btnCancel.Location = new System.Drawing.Point(197, 167);
            this.btnCancel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.btnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.NeedToTranslate = true;
            this.btnCancel.RepresentativeValue = "Cancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 22);
            this.btnCancel.TabIndex = 33;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ChooseVendor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 200);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChooseVendor";
            this.Text = "ChooseVendor";
            ((System.ComponentModel.ISupportInitialize)(this.tPanel1)).EndInit();
            this.tPanel1.ResumeLayout(false);
            this.tPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboVendor.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TAP.UIControls.BasicControlsDEV.TPanel tPanel1;
        private TAP.UIControls.BasicControlsDEV.TTextBox txtVendor;
        private TAP.UIControls.BasicControlsDEV.TLabel tLabel2;
        private TAP.UIControls.BasicControlsDEV.TLabel tLabel1;
        private TAP.UIControls.BasicControlsDEV.TComboBox cboVendor;
        private TAP.UIControls.BasicControlsDEV.TButton btnOK;
        private TAP.UIControls.BasicControlsDEV.TButton btnCancel;
    }
}