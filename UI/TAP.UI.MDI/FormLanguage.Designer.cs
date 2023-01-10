namespace TAP.UI.MDI
{
    partial class FormLanguage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLanguage));
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonLogIn = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel13 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.cboLanguage = new TAP.UIControls.BasicControlsDEV.TComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel5.SuspendLayout();
            this.panel13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel9.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboLanguage.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(309, 10);
            this.panel1.TabIndex = 0;
            // 
            // buttonLogIn
            // 
            this.buttonLogIn.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonLogIn.Location = new System.Drawing.Point(134, 0);
            this.buttonLogIn.Name = "buttonLogIn";
            this.buttonLogIn.Size = new System.Drawing.Size(75, 25);
            this.buttonLogIn.TabIndex = 2;
            this.buttonLogIn.Text = "Ok";
            this.buttonLogIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonLogIn.UseVisualStyleBackColor = true;
            this.buttonLogIn.Click += new System.EventHandler(this.buttonLogIn_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonCancel.Location = new System.Drawing.Point(59, 0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 25);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 10);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(50, 112);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(259, 10);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(50, 112);
            this.panel4.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Transparent;
            this.panel5.Controls.Add(this.panel13);
            this.panel5.Controls.Add(this.panel9);
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(50, 10);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(209, 112);
            this.panel5.TabIndex = 4;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.pictureBox1);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel13.Location = new System.Drawing.Point(0, 50);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(209, 62);
            this.panel13.TabIndex = 4;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::TAP.UI.MDI.Properties.Resources.ISEM1;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(209, 62);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.buttonCancel);
            this.panel9.Controls.Add(this.buttonLogIn);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(0, 25);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(209, 25);
            this.panel9.TabIndex = 3;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.cboLanguage);
            this.panel7.Controls.Add(this.label3);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(209, 25);
            this.panel7.TabIndex = 1;
            // 
            // cboLanguage
            // 
            this.cboLanguage.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.cboLanguage.BuiltInCategory = "";
            this.cboLanguage.BuiltInDispay = TAP.UIControls.EnumBuitInDispay.NONE;
            this.cboLanguage.BuiltInIncludeAll = false;
            this.cboLanguage.BuiltInSubCategory = "";
            this.cboLanguage.ControlID = "cboLanguage";
            this.cboLanguage.DisplayMember = null;
            this.cboLanguage.Dock = System.Windows.Forms.DockStyle.Right;
            this.cboLanguage.DominantControl = "";
            this.cboLanguage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboLanguage.DropDownHeight = 100;
            this.cboLanguage.EditValue = "";
            this.cboLanguage.IsColorData = false;
            this.cboLanguage.IsRequired = false;
            this.cboLanguage.Location = new System.Drawing.Point(59, 0);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.NeedToTranslate = true;
            this.cboLanguage.ParameterID = null;
            this.cboLanguage.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.cboLanguage.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.cboLanguage.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboLanguage.Properties.Items.AddRange(new object[] {
            "<image=#cn> CN",
            "<image=#en> EN",
            "<image=#kr> KR"});
            this.cboLanguage.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboLanguage.Properties.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboLanguage_Properties_KeyDown);
            this.cboLanguage.RepresentativeValue = "";
            this.cboLanguage.Size = new System.Drawing.Size(150, 20);
            this.cboLanguage.Sql = "";
            this.cboLanguage.SqlDataBase = "";
            this.cboLanguage.SqlType = TAP.UIControls.EnumSqlType.Biz;
            this.cboLanguage.TabIndex = 0;
            this.cboLanguage.ValueMember = null;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.ForeColor = System.Drawing.Color.Lavender;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Language";
            // 
            // FormLanguage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(64)))), ((int)(((byte)(74)))));
            this.ClientSize = new System.Drawing.Size(309, 122);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLanguage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TAP - login";
            this.Load += new System.EventHandler(this.FormLanguage_Load);
            this.panel5.ResumeLayout(false);
            this.panel13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboLanguage.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonLogIn;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel7;
        private UIControls.BasicControlsDEV.TComboBox cboLanguage;
    }
}