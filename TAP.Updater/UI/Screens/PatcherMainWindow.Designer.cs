namespace TAP.UPDATER
{
    partial class PatcherMainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatcherMainWindow));
            this.loggerDisplay = new System.Windows.Forms.Label();
            this.downloaderDisplay = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.speed = new System.Windows.Forms.Label();
            this.filecount = new System.Windows.Forms.Label();
            this.wholeProgressBar = new DevExpress.XtraEditors.ProgressBarControl();
            this.fileProgressBar = new DevExpress.XtraEditors.ProgressBarControl();
            this.starter = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.wholeProgressBar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileProgressBar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // loggerDisplay
            // 
            this.loggerDisplay.AutoSize = true;
            this.loggerDisplay.Location = new System.Drawing.Point(15, 91);
            this.loggerDisplay.Name = "loggerDisplay";
            this.loggerDisplay.Size = new System.Drawing.Size(41, 14);
            this.loggerDisplay.TabIndex = 5;
            this.loggerDisplay.Text = "logger";
            // 
            // downloaderDisplay
            // 
            this.downloaderDisplay.AutoEllipsis = true;
            this.downloaderDisplay.AutoSize = true;
            this.downloaderDisplay.Location = new System.Drawing.Point(15, 132);
            this.downloaderDisplay.Name = "downloaderDisplay";
            this.downloaderDisplay.Size = new System.Drawing.Size(71, 14);
            this.downloaderDisplay.TabIndex = 6;
            this.downloaderDisplay.Text = "downloader";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // speed
            // 
            this.speed.AutoSize = true;
            this.speed.Location = new System.Drawing.Point(15, 162);
            this.speed.Name = "speed";
            this.speed.Size = new System.Drawing.Size(40, 14);
            this.speed.TabIndex = 10;
            this.speed.Text = "speed";
            // 
            // filecount
            // 
            this.filecount.AutoEllipsis = true;
            this.filecount.AutoSize = true;
            this.filecount.Location = new System.Drawing.Point(64, 91);
            this.filecount.Name = "filecount";
            this.filecount.Size = new System.Drawing.Size(54, 14);
            this.filecount.TabIndex = 11;
            this.filecount.Text = "filecount";
            // 
            // wholeProgressBar
            // 
            this.wholeProgressBar.Location = new System.Drawing.Point(14, 109);
            this.wholeProgressBar.Name = "wholeProgressBar";
            this.wholeProgressBar.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.wholeProgressBar.Properties.LookAndFeel.SkinName = "Office 2013";
            this.wholeProgressBar.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.wholeProgressBar.Size = new System.Drawing.Size(400, 10);
            this.wholeProgressBar.TabIndex = 12;
            // 
            // fileProgressBar
            // 
            this.fileProgressBar.Location = new System.Drawing.Point(14, 149);
            this.fileProgressBar.Name = "fileProgressBar";
            this.fileProgressBar.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.fileProgressBar.Properties.LookAndFeel.SkinName = "Office 2013";
            this.fileProgressBar.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.fileProgressBar.Size = new System.Drawing.Size(400, 10);
            this.fileProgressBar.TabIndex = 13;
            // 
            // starter
            // 
            this.starter.Enabled = false;
            this.starter.Location = new System.Drawing.Point(323, 5);
            this.starter.Name = "starter";
            this.starter.Size = new System.Drawing.Size(91, 25);
            this.starter.TabIndex = 9;
            this.starter.Text = "Launch app!";
            this.starter.Click += new System.EventHandler(this.starter_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("groupControl1.CaptionImageOptions.Image")));
            this.groupControl1.Controls.Add(this.starter);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 191);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.ShowCaption = false;
            this.groupControl1.Size = new System.Drawing.Size(426, 35);
            this.groupControl1.TabIndex = 14;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.labelControl1.ImageOptions.Image = global::TAP.UPDATER.Properties.Resources.convert_32x32;
            this.labelControl1.Location = new System.Drawing.Point(116, 25);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(193, 36);
            this.labelControl1.TabIndex = 15;
            this.labelControl1.Text = "Intergrated System";
            // 
            // PatcherMainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 226);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.fileProgressBar);
            this.Controls.Add(this.wholeProgressBar);
            this.Controls.Add(this.filecount);
            this.Controls.Add(this.speed);
            this.Controls.Add(this.loggerDisplay);
            this.Controls.Add(this.downloaderDisplay);
            this.DoubleBuffered = true;
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("PatcherMainWindow.IconOptions.Icon")));
            this.IconOptions.ShowIcon = false;
            this.MinimizeBox = false;
            this.Name = "PatcherMainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.loadWindow);
            ((System.ComponentModel.ISupportInitialize)(this.wholeProgressBar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileProgressBar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label loggerDisplay;
        private System.Windows.Forms.Label downloaderDisplay;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label speed;
        private System.Windows.Forms.Label filecount;
        private DevExpress.XtraEditors.ProgressBarControl wholeProgressBar;
        private DevExpress.XtraEditors.ProgressBarControl fileProgressBar;
        private DevExpress.XtraEditors.SimpleButton starter;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}

