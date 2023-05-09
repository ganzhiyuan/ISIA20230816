
namespace ISIA.UI.ANALYSIS
{
    partial class FrmRankingofSQLShowSqlText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRankingofSQLShowSqlText));
            Steema.TeeChart.Margins margins1 = new Steema.TeeChart.Margins();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnCopy = new TAP.UIControls.BasicControlsDEV.TButton();
            this.textsqltext = new TAP.UIControls.BasicControlsDEV.THtmlView();
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
            this.layoutControl3 = new DevExpress.XtraLayout.LayoutControl();
            this.tChartSqlText = new Steema.TeeChart.TChart();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
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
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl3)).BeginInit();
            this.layoutControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.White;
            this.layoutControl1.Controls.Add(this.btnCopy);
            this.layoutControl1.Controls.Add(this.textsqltext);
            this.layoutControl1.Controls.Add(this.tButton1);
            this.layoutControl1.Controls.Add(this.txtSqlId);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.layoutControl1.Location = new System.Drawing.Point(0, 602);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1520, 300);
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
            this.btnCopy.Location = new System.Drawing.Point(1360, 12);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCopy.MaximumSize = new System.Drawing.Size(93, 31);
            this.btnCopy.MinimumSize = new System.Drawing.Size(93, 31);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.NeedToTranslate = true;
            this.btnCopy.RepresentativeValue = "Copy";
            this.btnCopy.Size = new System.Drawing.Size(93, 31);
            this.btnCopy.StyleController = this.layoutControl1;
            this.btnCopy.TabIndex = 115;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // textsqltext
            // 
            this.textsqltext.AcceptsEscape = false;
            this.textsqltext.AcceptsReturn = false;
            this.textsqltext.AcceptsTab = false;
            this.textsqltext.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple;
            this.textsqltext.ControlID = "SqlView";
            this.textsqltext.DragDropMode = DevExpress.XtraRichEdit.DragDropMode.Manual;
            this.textsqltext.IsRequired = false;
            this.textsqltext.LayoutUnit = DevExpress.XtraRichEdit.DocumentLayoutUnit.Pixel;
            this.textsqltext.Location = new System.Drawing.Point(95, 47);
            this.textsqltext.Margin = new System.Windows.Forms.Padding(4);
            this.textsqltext.Name = "textsqltext";
            this.textsqltext.NeedToTranslate = false;
            this.textsqltext.Options.DocumentSaveOptions.CurrentFormat = DevExpress.XtraRichEdit.DocumentFormat.PlainText;
            this.textsqltext.ReadOnly = true;
            this.textsqltext.RepresentativeValue = "";
            this.textsqltext.Size = new System.Drawing.Size(1766, 219);
            this.textsqltext.TabIndex = 113;
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
            this.tButton1.Location = new System.Drawing.Point(1769, 321);
            this.tButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tButton1.MaximumSize = new System.Drawing.Size(93, 31);
            this.tButton1.MinimumSize = new System.Drawing.Size(93, 31);
            this.tButton1.Name = "tButton1";
            this.tButton1.NeedToTranslate = true;
            this.tButton1.RepresentativeValue = "Close";
            this.tButton1.Size = new System.Drawing.Size(93, 31);
            this.tButton1.StyleController = this.layoutControl1;
            this.tButton1.TabIndex = 112;
            this.tButton1.Text = "Close";
            this.tButton1.UseVisualStyleBackColor = true;
            this.tButton1.Click += new System.EventHandler(this.tButton1_Click);
            // 
            // txtSqlId
            // 
            this.txtSqlId.Location = new System.Drawing.Point(95, 12);
            this.txtSqlId.Margin = new System.Windows.Forms.Padding(4);
            this.txtSqlId.Name = "txtSqlId";
            this.txtSqlId.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtSqlId.Properties.Appearance.Options.UseBackColor = true;
            this.txtSqlId.Properties.ReadOnly = true;
            this.txtSqlId.Size = new System.Drawing.Size(1261, 24);
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
            this.Root.Size = new System.Drawing.Size(1520, 300);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txtSqlId;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1348, 35);
            this.layoutControlItem1.Text = "SQL_ID";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(71, 18);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 214);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(1500, 31);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 245);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(1403, 35);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.tButton1;
            this.layoutControlItem3.Location = new System.Drawing.Point(1403, 245);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(97, 35);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.textsqltext;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 35);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(1500, 179);
            this.layoutControlItem2.Text = "SQL_TEXT";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(71, 18);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btnCopy;
            this.layoutControlItem5.Location = new System.Drawing.Point(1348, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(97, 35);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(1445, 0);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(55, 35);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControl3
            // 
            this.layoutControl3.Controls.Add(this.tChartSqlText);
            this.layoutControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl3.Location = new System.Drawing.Point(0, 0);
            this.layoutControl3.Margin = new System.Windows.Forms.Padding(4);
            this.layoutControl3.Name = "layoutControl3";
            this.layoutControl3.Root = this.layoutControlGroup2;
            this.layoutControl3.Size = new System.Drawing.Size(1520, 602);
            this.layoutControl3.TabIndex = 2;
            this.layoutControl3.Text = "layoutControl3";
            // 
            // tChartSqlText
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Bottom.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Bottom.Labels.Brush.Color = System.Drawing.Color.White;
            this.tChartSqlText.Axes.Bottom.Labels.Brush.Solid = true;
            this.tChartSqlText.Axes.Bottom.Labels.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Bottom.Labels.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Bottom.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChartSqlText.Axes.Bottom.Labels.Font.Brush.Solid = true;
            this.tChartSqlText.Axes.Bottom.Labels.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Bottom.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Bottom.Labels.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Bottom.Labels.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.Axes.Bottom.Labels.Font.Size = 9;
            this.tChartSqlText.Axes.Bottom.Labels.Font.SizeFloat = 9F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Bottom.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Axes.Bottom.Labels.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Axes.Bottom.Labels.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Bottom.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Bottom.Labels.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Bottom.Labels.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Bottom.Title.Angle = 0;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Bottom.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Bottom.Title.Brush.Color = System.Drawing.Color.Silver;
            this.tChartSqlText.Axes.Bottom.Title.Brush.Solid = true;
            this.tChartSqlText.Axes.Bottom.Title.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Bottom.Title.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Bottom.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tChartSqlText.Axes.Bottom.Title.Font.Brush.Solid = true;
            this.tChartSqlText.Axes.Bottom.Title.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Bottom.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Bottom.Title.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Bottom.Title.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.Axes.Bottom.Title.Font.Size = 11;
            this.tChartSqlText.Axes.Bottom.Title.Font.SizeFloat = 11F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Bottom.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Axes.Bottom.Title.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Axes.Bottom.Title.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Bottom.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Bottom.Title.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Bottom.Title.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Depth.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Depth.Labels.Brush.Color = System.Drawing.Color.White;
            this.tChartSqlText.Axes.Depth.Labels.Brush.Solid = true;
            this.tChartSqlText.Axes.Depth.Labels.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Depth.Labels.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Depth.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChartSqlText.Axes.Depth.Labels.Font.Brush.Solid = true;
            this.tChartSqlText.Axes.Depth.Labels.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Depth.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Depth.Labels.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Depth.Labels.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.Axes.Depth.Labels.Font.Size = 9;
            this.tChartSqlText.Axes.Depth.Labels.Font.SizeFloat = 9F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Depth.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Axes.Depth.Labels.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Axes.Depth.Labels.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Depth.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Depth.Labels.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Depth.Labels.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Depth.Title.Angle = 0;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Depth.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Depth.Title.Brush.Color = System.Drawing.Color.Silver;
            this.tChartSqlText.Axes.Depth.Title.Brush.Solid = true;
            this.tChartSqlText.Axes.Depth.Title.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Depth.Title.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Depth.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tChartSqlText.Axes.Depth.Title.Font.Brush.Solid = true;
            this.tChartSqlText.Axes.Depth.Title.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Depth.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Depth.Title.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Depth.Title.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.Axes.Depth.Title.Font.Size = 11;
            this.tChartSqlText.Axes.Depth.Title.Font.SizeFloat = 11F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Depth.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Axes.Depth.Title.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Axes.Depth.Title.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Depth.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Depth.Title.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Depth.Title.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.DepthTop.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Axes.DepthTop.Labels.Brush.Color = System.Drawing.Color.White;
            this.tChartSqlText.Axes.DepthTop.Labels.Brush.Solid = true;
            this.tChartSqlText.Axes.DepthTop.Labels.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.DepthTop.Labels.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.Axes.DepthTop.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChartSqlText.Axes.DepthTop.Labels.Font.Brush.Solid = true;
            this.tChartSqlText.Axes.DepthTop.Labels.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.DepthTop.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.DepthTop.Labels.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.DepthTop.Labels.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.Axes.DepthTop.Labels.Font.Size = 9;
            this.tChartSqlText.Axes.DepthTop.Labels.Font.SizeFloat = 9F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.DepthTop.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Axes.DepthTop.Labels.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Axes.DepthTop.Labels.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.DepthTop.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.DepthTop.Labels.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.DepthTop.Labels.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.DepthTop.Title.Angle = 0;
            // 
            // 
            // 
            this.tChartSqlText.Axes.DepthTop.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Axes.DepthTop.Title.Brush.Color = System.Drawing.Color.Silver;
            this.tChartSqlText.Axes.DepthTop.Title.Brush.Solid = true;
            this.tChartSqlText.Axes.DepthTop.Title.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.DepthTop.Title.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.Axes.DepthTop.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tChartSqlText.Axes.DepthTop.Title.Font.Brush.Solid = true;
            this.tChartSqlText.Axes.DepthTop.Title.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.DepthTop.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.DepthTop.Title.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.DepthTop.Title.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.Axes.DepthTop.Title.Font.Size = 11;
            this.tChartSqlText.Axes.DepthTop.Title.Font.SizeFloat = 11F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.DepthTop.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Axes.DepthTop.Title.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Axes.DepthTop.Title.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.DepthTop.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.DepthTop.Title.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.DepthTop.Title.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Left.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Left.Labels.Brush.Color = System.Drawing.Color.White;
            this.tChartSqlText.Axes.Left.Labels.Brush.Solid = true;
            this.tChartSqlText.Axes.Left.Labels.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Left.Labels.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Left.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChartSqlText.Axes.Left.Labels.Font.Brush.Solid = true;
            this.tChartSqlText.Axes.Left.Labels.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Left.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Left.Labels.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Left.Labels.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.Axes.Left.Labels.Font.Size = 9;
            this.tChartSqlText.Axes.Left.Labels.Font.SizeFloat = 9F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Left.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Axes.Left.Labels.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Axes.Left.Labels.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Left.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Left.Labels.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Left.Labels.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Left.Title.Angle = 90;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Left.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Left.Title.Brush.Color = System.Drawing.Color.Silver;
            this.tChartSqlText.Axes.Left.Title.Brush.Solid = true;
            this.tChartSqlText.Axes.Left.Title.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Left.Title.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Left.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tChartSqlText.Axes.Left.Title.Font.Brush.Solid = true;
            this.tChartSqlText.Axes.Left.Title.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Left.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Left.Title.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Left.Title.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.Axes.Left.Title.Font.Size = 11;
            this.tChartSqlText.Axes.Left.Title.Font.SizeFloat = 11F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Left.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Axes.Left.Title.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Axes.Left.Title.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Left.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Left.Title.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Left.Title.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Right.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Right.Labels.Brush.Color = System.Drawing.Color.White;
            this.tChartSqlText.Axes.Right.Labels.Brush.Solid = true;
            this.tChartSqlText.Axes.Right.Labels.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Right.Labels.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Right.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChartSqlText.Axes.Right.Labels.Font.Brush.Solid = true;
            this.tChartSqlText.Axes.Right.Labels.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Right.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Right.Labels.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Right.Labels.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.Axes.Right.Labels.Font.Size = 9;
            this.tChartSqlText.Axes.Right.Labels.Font.SizeFloat = 9F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Right.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Axes.Right.Labels.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Axes.Right.Labels.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Right.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Right.Labels.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Right.Labels.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Right.Title.Angle = 270;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Right.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Right.Title.Brush.Color = System.Drawing.Color.Silver;
            this.tChartSqlText.Axes.Right.Title.Brush.Solid = true;
            this.tChartSqlText.Axes.Right.Title.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Right.Title.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Right.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tChartSqlText.Axes.Right.Title.Font.Brush.Solid = true;
            this.tChartSqlText.Axes.Right.Title.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Right.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Right.Title.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Right.Title.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.Axes.Right.Title.Font.Size = 11;
            this.tChartSqlText.Axes.Right.Title.Font.SizeFloat = 11F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Right.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Axes.Right.Title.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Axes.Right.Title.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Right.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Right.Title.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Right.Title.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Top.Labels.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Top.Labels.Brush.Color = System.Drawing.Color.White;
            this.tChartSqlText.Axes.Top.Labels.Brush.Solid = true;
            this.tChartSqlText.Axes.Top.Labels.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Top.Labels.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Top.Labels.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChartSqlText.Axes.Top.Labels.Font.Brush.Solid = true;
            this.tChartSqlText.Axes.Top.Labels.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Top.Labels.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Top.Labels.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Top.Labels.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.Axes.Top.Labels.Font.Size = 9;
            this.tChartSqlText.Axes.Top.Labels.Font.SizeFloat = 9F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Top.Labels.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Axes.Top.Labels.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Axes.Top.Labels.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Top.Labels.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Top.Labels.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Top.Labels.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Top.Title.Angle = 0;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Top.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Top.Title.Brush.Color = System.Drawing.Color.Silver;
            this.tChartSqlText.Axes.Top.Title.Brush.Solid = true;
            this.tChartSqlText.Axes.Top.Title.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Top.Title.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.Axes.Top.Title.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tChartSqlText.Axes.Top.Title.Font.Brush.Solid = true;
            this.tChartSqlText.Axes.Top.Title.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Top.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Top.Title.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Top.Title.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.Axes.Top.Title.Font.Size = 11;
            this.tChartSqlText.Axes.Top.Title.Font.SizeFloat = 11F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Top.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Axes.Top.Title.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Axes.Top.Title.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Axes.Top.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Axes.Top.Title.Shadow.Brush.Solid = true;
            this.tChartSqlText.Axes.Top.Title.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Footer.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Footer.Brush.Color = System.Drawing.Color.Silver;
            this.tChartSqlText.Footer.Brush.Solid = true;
            this.tChartSqlText.Footer.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Footer.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.Footer.Font.Brush.Color = System.Drawing.Color.Red;
            this.tChartSqlText.Footer.Font.Brush.Solid = true;
            this.tChartSqlText.Footer.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Footer.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Footer.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.Footer.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.Footer.Font.Size = 8;
            this.tChartSqlText.Footer.Font.SizeFloat = 8F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Footer.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Footer.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Footer.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Footer.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Footer.Shadow.Brush.Solid = true;
            this.tChartSqlText.Footer.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Header.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Header.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.tChartSqlText.Header.Brush.Solid = true;
            this.tChartSqlText.Header.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Header.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.Header.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChartSqlText.Header.Font.Brush.Solid = true;
            this.tChartSqlText.Header.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Header.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Header.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.Header.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.Header.Font.Size = 12;
            this.tChartSqlText.Header.Font.SizeFloat = 12F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Header.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Header.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Header.ImageBevel.Brush.Visible = true;
            this.tChartSqlText.Header.Lines = new string[] {
        ""};
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Header.Shadow.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.tChartSqlText.Header.Shadow.Brush.Solid = true;
            this.tChartSqlText.Header.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Legend.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Legend.Brush.Color = System.Drawing.Color.White;
            this.tChartSqlText.Legend.Brush.Solid = true;
            this.tChartSqlText.Legend.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Legend.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.Legend.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tChartSqlText.Legend.Font.Brush.Solid = true;
            this.tChartSqlText.Legend.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Legend.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Legend.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.Legend.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.Legend.Font.Size = 9;
            this.tChartSqlText.Legend.Font.SizeFloat = 9F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Legend.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Legend.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Legend.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Legend.Shadow.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tChartSqlText.Legend.Shadow.Brush.Solid = true;
            this.tChartSqlText.Legend.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Legend.Symbol.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Legend.Symbol.Shadow.Brush.Solid = true;
            this.tChartSqlText.Legend.Symbol.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Legend.Title.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Legend.Title.Brush.Color = System.Drawing.Color.White;
            this.tChartSqlText.Legend.Title.Brush.Solid = true;
            this.tChartSqlText.Legend.Title.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.Legend.Title.Font.Bold = true;
            // 
            // 
            // 
            this.tChartSqlText.Legend.Title.Font.Brush.Color = System.Drawing.Color.Black;
            this.tChartSqlText.Legend.Title.Font.Brush.Solid = true;
            this.tChartSqlText.Legend.Title.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Legend.Title.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Legend.Title.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.Legend.Title.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.Legend.Title.Font.Size = 8;
            this.tChartSqlText.Legend.Title.Font.SizeFloat = 8F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Legend.Title.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Legend.Title.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Legend.Title.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Legend.Title.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Legend.Title.Shadow.Brush.Solid = true;
            this.tChartSqlText.Legend.Title.Shadow.Brush.Visible = true;
            this.tChartSqlText.Location = new System.Drawing.Point(12, 12);
            this.tChartSqlText.Margin = new System.Windows.Forms.Padding(4);
            this.tChartSqlText.Name = "tChartSqlText";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Panel.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Panel.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tChartSqlText.Panel.Brush.Solid = true;
            this.tChartSqlText.Panel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Panel.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Panel.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Panel.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Panel.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Panel.Shadow.Brush.Solid = true;
            this.tChartSqlText.Panel.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            margins1.Bottom = 100;
            margins1.Left = 100;
            margins1.Right = 100;
            margins1.Top = 100;
            this.tChartSqlText.Printer.Margins = margins1;
            this.tChartSqlText.Size = new System.Drawing.Size(1496, 578);
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.SubFooter.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.SubFooter.Brush.Color = System.Drawing.Color.Silver;
            this.tChartSqlText.SubFooter.Brush.Solid = true;
            this.tChartSqlText.SubFooter.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.SubFooter.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.SubFooter.Font.Brush.Color = System.Drawing.Color.Red;
            this.tChartSqlText.SubFooter.Font.Brush.Solid = true;
            this.tChartSqlText.SubFooter.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.SubFooter.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.SubFooter.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.SubFooter.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.SubFooter.Font.Size = 8;
            this.tChartSqlText.SubFooter.Font.SizeFloat = 8F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.SubFooter.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.SubFooter.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.SubFooter.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.SubFooter.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.SubFooter.Shadow.Brush.Solid = true;
            this.tChartSqlText.SubFooter.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.SubHeader.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.SubHeader.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.tChartSqlText.SubHeader.Brush.Solid = true;
            this.tChartSqlText.SubHeader.Brush.Visible = true;
            // 
            // 
            // 
            this.tChartSqlText.SubHeader.Font.Bold = false;
            // 
            // 
            // 
            this.tChartSqlText.SubHeader.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tChartSqlText.SubHeader.Font.Brush.Solid = true;
            this.tChartSqlText.SubHeader.Font.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.SubHeader.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.SubHeader.Font.Shadow.Brush.Solid = true;
            this.tChartSqlText.SubHeader.Font.Shadow.Brush.Visible = true;
            this.tChartSqlText.SubHeader.Font.Size = 12;
            this.tChartSqlText.SubHeader.Font.SizeFloat = 12F;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.SubHeader.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.SubHeader.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.SubHeader.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.SubHeader.Shadow.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.tChartSqlText.SubHeader.Shadow.Brush.Solid = true;
            this.tChartSqlText.SubHeader.Shadow.Brush.Visible = true;
            this.tChartSqlText.TabIndex = 4;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Walls.Back.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Walls.Back.Brush.Color = System.Drawing.Color.Silver;
            this.tChartSqlText.Walls.Back.Brush.Solid = true;
            this.tChartSqlText.Walls.Back.Brush.Visible = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Walls.Back.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Walls.Back.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Walls.Back.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Walls.Back.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Walls.Back.Shadow.Brush.Solid = true;
            this.tChartSqlText.Walls.Back.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Walls.Bottom.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Walls.Bottom.Brush.Color = System.Drawing.Color.White;
            this.tChartSqlText.Walls.Bottom.Brush.Solid = true;
            this.tChartSqlText.Walls.Bottom.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Walls.Bottom.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Walls.Bottom.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Walls.Bottom.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Walls.Bottom.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Walls.Bottom.Shadow.Brush.Solid = true;
            this.tChartSqlText.Walls.Bottom.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Walls.Left.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Walls.Left.Brush.Color = System.Drawing.Color.LightYellow;
            this.tChartSqlText.Walls.Left.Brush.Solid = true;
            this.tChartSqlText.Walls.Left.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Walls.Left.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Walls.Left.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Walls.Left.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Walls.Left.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Walls.Left.Shadow.Brush.Solid = true;
            this.tChartSqlText.Walls.Left.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Walls.Right.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            // 
            // 
            // 
            this.tChartSqlText.Walls.Right.Brush.Color = System.Drawing.Color.LightYellow;
            this.tChartSqlText.Walls.Right.Brush.Solid = true;
            this.tChartSqlText.Walls.Right.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Walls.Right.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
            this.tChartSqlText.Walls.Right.ImageBevel.Brush.Solid = true;
            this.tChartSqlText.Walls.Right.ImageBevel.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Walls.Right.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
            this.tChartSqlText.Walls.Right.Shadow.Brush.Solid = true;
            this.tChartSqlText.Walls.Right.Shadow.Brush.Visible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChartSqlText.Zoom.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.tChartSqlText.Zoom.Brush.Solid = true;
            this.tChartSqlText.Zoom.Brush.Visible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem6});
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(1520, 602);
            this.layoutControlGroup2.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.tChartSqlText;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(1500, 582);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // FrmRankingofSQLShowSqlText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1520, 902);
            this.Controls.Add(this.layoutControl3);
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmRankingofSQLShowSqlText";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RankingofSQLShowSqlText";
            this.Load += new System.EventHandler(this.FrmWorkLoadTreadShowSqlText_Load);
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
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl3)).EndInit();
            this.layoutControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
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
        private TAP.UIControls.BasicControlsDEV.THtmlView textsqltext;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        public TAP.UIControls.BasicControlsDEV.TButton btnCopy;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraLayout.LayoutControl layoutControl3;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private Steema.TeeChart.TChart tChartSqlText;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
    }
}