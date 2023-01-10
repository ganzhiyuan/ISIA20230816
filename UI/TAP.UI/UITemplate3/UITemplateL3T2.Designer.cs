namespace TAP.UI.UITemplate3
{
    partial class UITemplateL3T2
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
            this.BaseA = new TAP.UIControls.BasicControls.TPanel();
            this.BaseB = new TAP.UIControls.BasicControls.TPanel();
            this.BaseC = new TAP.UIControls.BasicControls.TPanel();
            this.tPanelBottomBase.SuspendLayout();
            this.tPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tPanelMain
            // 
            this.tPanelMain.Controls.Add(this.BaseC);
            this.tPanelMain.Controls.Add(this.BaseB);
            this.tPanelMain.Controls.Add(this.BaseA);
            // 
            // BaseA
            // 
            this.BaseA.ControlID = "BaseA";
            this.BaseA.Dock = System.Windows.Forms.DockStyle.Left;
            this.BaseA.IsRequired = false;
            this.BaseA.Location = new System.Drawing.Point(0, 0);
            this.BaseA.Name = "BaseA";
            this.BaseA.NeedToTranslate = true;
            this.BaseA.Padding = new System.Windows.Forms.Padding(3);
            this.BaseA.RepresentativeValue = "BaseA [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms.Bo" +
    "rderStyle.None";
            this.BaseA.Size = new System.Drawing.Size(300, 687);
            this.BaseA.TabIndex = 0;
            // 
            // BaseB
            // 
            this.BaseB.ControlID = "BaseB";
            this.BaseB.Dock = System.Windows.Forms.DockStyle.Left;
            this.BaseB.IsRequired = false;
            this.BaseB.Location = new System.Drawing.Point(300, 0);
            this.BaseB.Name = "BaseB";
            this.BaseB.NeedToTranslate = true;
            this.BaseB.Padding = new System.Windows.Forms.Padding(3);
            this.BaseB.RepresentativeValue = "BaseB [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms.Bo" +
    "rderStyle.None";
            this.BaseB.Size = new System.Drawing.Size(300, 687);
            this.BaseB.TabIndex = 1;
            // 
            // BaseC
            // 
            this.BaseC.ControlID = "BaseC";
            this.BaseC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BaseC.IsRequired = false;
            this.BaseC.Location = new System.Drawing.Point(600, 0);
            this.BaseC.Name = "BaseC";
            this.BaseC.NeedToTranslate = true;
            this.BaseC.Padding = new System.Windows.Forms.Padding(3);
            this.BaseC.RepresentativeValue = "BaseC [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms.Bo" +
    "rderStyle.None";
            this.BaseC.Size = new System.Drawing.Size(412, 687);
            this.BaseC.TabIndex = 2;
            // 
            // UITemplateL3T2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UITemplateL3T2";
            this.Text = "UITemplateL3T2";
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
        /// Base C
        /// </summary>
        protected UIControls.BasicControls.TPanel BaseC;
    }
}