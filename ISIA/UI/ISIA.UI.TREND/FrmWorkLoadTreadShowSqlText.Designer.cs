
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
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.txtSqlId = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.tButton1 = new TAP.UIControls.BasicControlsDEV.TButton();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.SqlView = new TAP.UIControls.BasicControlsDEV.THtmlView();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSqlId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
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
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.emptySpaceItem2,
            this.layoutControlItem3,
            this.layoutControlItem2});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(863, 441);
            this.Root.TextVisible = false;
            // 
            // txtSqlId
            // 
            this.txtSqlId.Location = new System.Drawing.Point(83, 12);
            this.txtSqlId.Name = "txtSqlId";
            this.txtSqlId.Size = new System.Drawing.Size(768, 20);
            this.txtSqlId.StyleController = this.layoutControl1;
            this.txtSqlId.TabIndex = 4;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txtSqlId;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(843, 24);
            this.layoutControlItem1.Text = "SQL_ID";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(59, 14);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 375);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(843, 17);
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
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.tButton1;
            this.layoutControlItem3.Location = new System.Drawing.Point(769, 392);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(74, 29);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
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
            this.SqlView.Location = new System.Drawing.Point(83, 36);
            this.SqlView.Name = "SqlView";
            this.SqlView.NeedToTranslate = false;
            this.SqlView.Options.DocumentSaveOptions.CurrentFormat = DevExpress.XtraRichEdit.DocumentFormat.PlainText;
            this.SqlView.ReadOnly = true;
            this.SqlView.RepresentativeValue = "";
            this.SqlView.Size = new System.Drawing.Size(768, 347);
            this.SqlView.TabIndex = 113;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.SqlView;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(843, 351);
            this.layoutControlItem2.Text = "SQL_TEXT";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(59, 14);
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
            this.Text = "WorkLoadTreadShowSqlText";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSqlId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
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
    }
}