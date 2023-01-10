namespace TAP.UI.UITemplate2
{
    partial class UITemplate2T1A
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
        private new void InitializeComponent()
        {
            this.BaseB = new TAP.UIControls.BasicControls.TPanel();
            this.tSplitter1 = new TAP.UIControls.BasicControls.TSplitter();
            this.BaseA = new TAP.UIControls.BasicControls.TPanel();
            this.tPanelBottomBase.SuspendLayout();
            this.tPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tPanelMain
            // 
            this.tPanelMain.Controls.Add(this.BaseA);
            this.tPanelMain.Controls.Add(this.tSplitter1);
            this.tPanelMain.Controls.Add(this.BaseB);
            // 
            // BaseB
            // 
            this.BaseB.ControlID = "BaseB";
            this.BaseB.Dock = System.Windows.Forms.DockStyle.Right;
            this.BaseB.IsRequired = false;
            this.BaseB.Location = new System.Drawing.Point(512, 0);
            this.BaseB.Name = "BaseB";
            this.BaseB.NeedToTranslate = true;
            this.BaseB.RepresentativeValue = "BaseB [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms.Bo" +
    "rderStyle.None";
            this.BaseB.Size = new System.Drawing.Size(500, 687);
            this.BaseB.TabIndex = 0;
            // 
            // tSplitter1
            // 
            this.tSplitter1.CommandType = TAP.UIControls.BasicControls.EnumCommandType.ADD;
            this.tSplitter1.ControlID = "tSplitter1";
            this.tSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tSplitter1.IsRequired = false;
            this.tSplitter1.Location = new System.Drawing.Point(509, 0);
            this.tSplitter1.Name = "tSplitter1";
            this.tSplitter1.NeedToTranslate = false;
            this.tSplitter1.RepresentativeValue = "";
            this.tSplitter1.Size = new System.Drawing.Size(3, 687);
            this.tSplitter1.TabIndex = 1;
            this.tSplitter1.TabStop = false;
            // 
            // BaseA
            // 
            this.BaseA.ControlID = "BaseA";
            this.BaseA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BaseA.IsRequired = false;
            this.BaseA.Location = new System.Drawing.Point(0, 0);
            this.BaseA.Name = "BaseA";
            this.BaseA.NeedToTranslate = true;
            this.BaseA.RepresentativeValue = "BaseA [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms.Bo" +
    "rderStyle.None";
            this.BaseA.Size = new System.Drawing.Size(509, 687);
            this.BaseA.TabIndex = 2;
            // 
            // UITemplate2T1A
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UITemplate2T1A";
            this.Text = "UITemplate2T1A";
            this.tPanelBottomBase.ResumeLayout(false);
            this.tPanelBottomBase.PerformLayout();
            this.tPanelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Base A
        /// </summary>
        protected UIControls.BasicControls.TPanel BaseA;

        /// <summary>
        /// Base B
        /// </summary>
        protected UIControls.BasicControls.TPanel BaseB;

        /// <summary>
        /// Splitter between A and B
        /// </summary>
        protected UIControls.BasicControls.TSplitter tSplitter1;
    }
}