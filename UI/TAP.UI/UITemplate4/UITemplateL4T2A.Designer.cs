namespace TAP.UI.UITemplate4
{
    partial class UITemplateL4T2A
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
        /// This method draws template.
        /// </summary>
        virtual protected new void InitializeComponent()
        {
            this.BaseA = new TAP.UIControls.BasicControls.TPanel();
            this.BaseB = new TAP.UIControls.BasicControls.TPanel();
            this.BaseC = new TAP.UIControls.BasicControls.TPanel();
            this.BaseD = new TAP.UIControls.BasicControls.TPanel();
            this.tPanelBottomBase.SuspendLayout();
            this.tPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tPanelBottomBase
            // 
            this.tPanelBottomBase.Location = new System.Drawing.Point(0, 821);
            this.tPanelBottomBase.Size = new System.Drawing.Size(743, 20);
            // 
            // tPanelTop
            // 
            this.tPanelTop.Size = new System.Drawing.Size(743, 20);
            // 
            // tPanelMain
            // 
            this.tPanelMain.Controls.Add(this.BaseD);
            this.tPanelMain.Controls.Add(this.BaseC);
            this.tPanelMain.Controls.Add(this.BaseB);
            this.tPanelMain.Controls.Add(this.BaseA);
            this.tPanelMain.Size = new System.Drawing.Size(743, 801);
            // 
            // BaseA
            // 
            this.BaseA.ControlID = "BaseA";
            this.BaseA.Dock = System.Windows.Forms.DockStyle.Top;
            this.BaseA.IsRequired = false;
            this.BaseA.Location = new System.Drawing.Point(0, 0);
            this.BaseA.Name = "BaseA";
            this.BaseA.NeedToTranslate = true;
            this.BaseA.Padding = new System.Windows.Forms.Padding(3);
            this.BaseA.RepresentativeValue = "BaseA [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms.Bo" +
    "rderStyle.None";
            this.BaseA.Size = new System.Drawing.Size(743, 100);
            this.BaseA.TabIndex = 0;
            // 
            // BaseB
            // 
            this.BaseB.ControlID = "BaseB";
            this.BaseB.Dock = System.Windows.Forms.DockStyle.Top;
            this.BaseB.IsRequired = false;
            this.BaseB.Location = new System.Drawing.Point(0, 100);
            this.BaseB.Name = "BaseB";
            this.BaseB.NeedToTranslate = true;
            this.BaseB.Padding = new System.Windows.Forms.Padding(3);
            this.BaseB.RepresentativeValue = "BaseB [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms.Bo" +
    "rderStyle.None";
            this.BaseB.Size = new System.Drawing.Size(743, 100);
            this.BaseB.TabIndex = 1;
            // 
            // BaseC
            // 
            this.BaseC.ControlID = "BaseC";
            this.BaseC.Dock = System.Windows.Forms.DockStyle.Top;
            this.BaseC.IsRequired = false;
            this.BaseC.Location = new System.Drawing.Point(0, 200);
            this.BaseC.Name = "BaseC";
            this.BaseC.NeedToTranslate = true;
            this.BaseC.Padding = new System.Windows.Forms.Padding(3);
            this.BaseC.RepresentativeValue = "BaseC [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms.Bo" +
    "rderStyle.None";
            this.BaseC.Size = new System.Drawing.Size(743, 100);
            this.BaseC.TabIndex = 2;
            // 
            // BaseD
            // 
            this.BaseD.ControlID = "BaseD";
            this.BaseD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BaseD.IsRequired = false;
            this.BaseD.Location = new System.Drawing.Point(0, 300);
            this.BaseD.Name = "BaseD";
            this.BaseD.NeedToTranslate = true;
            this.BaseD.Padding = new System.Windows.Forms.Padding(3);
            this.BaseD.RepresentativeValue = "BaseD [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms.Bo" +
    "rderStyle.None";
            this.BaseD.Size = new System.Drawing.Size(743, 501);
            this.BaseD.TabIndex = 3;
            // 
            // UITemplateL4T2A
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 841);
            this.Name = "UITemplateL4T2A";
            this.Text = "UITemplateV3";
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

        /// <summary>
        /// Base D
        /// </summary>
        protected UIControls.BasicControls.TPanel BaseD;


    }
}