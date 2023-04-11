
namespace ISIA.UI.TREND
{
    partial class FrmWorkLoadTreadShowSqlText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmWorkLoadTreadShowSqlText));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnCopy = new TAP.UIControls.BasicControlsDEV.TButton();
            this.SqlView = new TAP.UIControls.BasicControlsDEV.THtmlView();
            this.tButton1 = new TAP.UIControls.BasicControlsDEV.TButton();
            this.txtSqlId = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSqlId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.White;
            this.layoutControl1.Controls.Add(this.btnCopy);
            this.layoutControl1.Controls.Add(this.SqlView);
            this.layoutControl1.Controls.Add(this.tButton1);
            this.layoutControl1.Controls.Add(this.txtSqlId);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(863, 441);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnCopy
            // 
            this.btnCopy.Appearance.BackColor = System.Drawing.Color.White;
            this.btnCopy.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.btnCopy.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.btnCopy.Appearance.Options.UseBackColor = true;
            this.btnCopy.Appearance.Options.UseBorderColor = true;
            this.btnCopy.Appearance.Options.UseFont = true;
            this.btnCopy.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.btnCopy.AppearanceHovered.Options.UseBackColor = true;
            this.btnCopy.AppearancePressed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.btnCopy.AppearancePressed.Options.UseBackColor = true;
            this.btnCopy.CommandType = TAP.UIControls.BasicControlsDEV.EnumCommandType.SEARCH;
            this.btnCopy.ControlID = "btnSelect";
            this.btnCopy.FlatStyle = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.btnCopy.IconColorType = TAP.UIControls.BasicControlsDEV.EnumColorType.BLACK;
            this.btnCopy.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.ImageOptions.Image")));
            this.btnCopy.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnCopy.IsRequired = false;
            this.btnCopy.Location = new System.Drawing.Point(770, 12);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(2);
            this.btnCopy.MaximumSize = new System.Drawing.Size(70, 25);
            this.btnCopy.MinimumSize = new System.Drawing.Size(70, 25);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.NeedToTranslate = true;
            this.btnCopy.RepresentativeValue = "Copy";
            this.btnCopy.Size = new System.Drawing.Size(70, 25);
            this.btnCopy.StyleController = this.layoutControl1;
            this.btnCopy.TabIndex = 115;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // SqlView
            // 
            this.SqlView.AcceptsEscape = false;
            this.SqlView.AcceptsReturn = false;
            this.SqlView.AcceptsTab = false;
            this.SqlView.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple;
            this.SqlView.ControlID = "SqlView";
            this.SqlView.DragDropMode = DevExpress.XtraRichEdit.DragDropMode.Manual;
            this.SqlView.IsRequired = false;
            this.SqlView.LayoutUnit = DevExpress.XtraRichEdit.DocumentLayoutUnit.Pixel;
            this.SqlView.Location = new System.Drawing.Point(83, 41);
            this.SqlView.Name = "SqlView";
            this.SqlView.NeedToTranslate = false;
            this.SqlView.Options.DocumentSaveOptions.CurrentFormat = DevExpress.XtraRichEdit.DocumentFormat.PlainText;
            this.SqlView.ReadOnly = true;
            this.SqlView.RepresentativeValue = "";
            this.SqlView.Size = new System.Drawing.Size(768, 322);
            this.SqlView.TabIndex = 113;
            // 
            // tButton1
            // 
            this.tButton1.Appearance.BackColor = System.Drawing.Color.White;
            this.tButton1.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.tButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.tButton1.Appearance.Options.UseBackColor = true;
            this.tButton1.Appearance.Options.UseBorderColor = true;
            this.tButton1.Appearance.Options.UseFont = true;
            this.tButton1.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.tButton1.AppearanceHovered.Options.UseBackColor = true;
            this.tButton1.AppearancePressed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.tButton1.AppearancePressed.Options.UseBackColor = true;
            this.tButton1.CommandType = TAP.UIControls.BasicControlsDEV.EnumCommandType.SEARCH;
            this.tButton1.ControlID = "btnSelect";
            this.tButton1.FlatStyle = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.tButton1.IconColorType = TAP.UIControls.BasicControlsDEV.EnumColorType.BLACK;
            this.tButton1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("tButton1.ImageOptions.Image")));
            this.tButton1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.tButton1.IsRequired = false;
            this.tButton1.Location = new System.Drawing.Point(781, 404);
            this.tButton1.Margin = new System.Windows.Forms.Padding(2);
            this.tButton1.MaximumSize = new System.Drawing.Size(70, 25);
            this.tButton1.MinimumSize = new System.Drawing.Size(70, 25);
            this.tButton1.Name = "tButton1";
            this.tButton1.NeedToTranslate = true;
            this.tButton1.RepresentativeValue = "Close";
            this.tButton1.Size = new System.Drawing.Size(70, 25);
            this.tButton1.StyleController = this.layoutControl1;
            this.tButton1.TabIndex = 112;
            this.tButton1.Text = "Close";
            this.tButton1.UseVisualStyleBackColor = true;
            this.tButton1.Click += new System.EventHandler(this.tButton1_Click);
            // 
            // txtSqlId
            // 
            this.txtSqlId.Location = new System.Drawing.Point(83, 12);
            this.txtSqlId.Name = "txtSqlId";
            this.txtSqlId.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtSqlId.Properties.Appearance.Options.UseBackColor = true;
            this.txtSqlId.Properties.ReadOnly = true;
            this.txtSqlId.Size = new System.Drawing.Size(683, 20);
            this.txtSqlId.StyleController = this.layoutControl1;
            this.txtSqlId.TabIndex = 4;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.emptySpaceItem2,
            this.layoutControlItem3,
            this.layoutControlItem2,
            this.layoutControlItem5,
            this.emptySpaceItem3});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(863, 441);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txtSqlId;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(758, 29);
            this.layoutControlItem1.Text = "SQL_ID";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(59, 14);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 355);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(843, 37);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 392);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(769, 29);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.tButton1;
            this.layoutControlItem3.Location = new System.Drawing.Point(769, 392);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(74, 29);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.SqlView;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 29);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(843, 326);
            this.layoutControlItem2.Text = "SQL_TEXT";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(59, 14);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btnCopy;
            this.layoutControlItem5.Location = new System.Drawing.Point(758, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(74, 29);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(832, 0);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(11, 29);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // FrmWorkLoadTreadShowSqlText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 441);
            this.Controls.Add(this.layoutControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmWorkLoadTreadShowSqlText";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WorkLoadTreadShowSqlText";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSqlId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.TextEdit txtSqlId;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        public TAP.UIControls.BasicControlsDEV.TButton tButton1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private TAP.UIControls.BasicControlsDEV.THtmlView SqlView;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        public TAP.UIControls.BasicControlsDEV.TButton btnCopy;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
    }
}