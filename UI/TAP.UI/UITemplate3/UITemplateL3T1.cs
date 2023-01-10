using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace TAP.UI.UITemplate3
{
    /// <summary>
    /// This class is UI templage Type 1 of Layer 3.
    /// </summary>
    [Designer(typeof(TAP.UI.UIBaseDesigner), typeof(IRootDesigner))]
    public partial class UITemplateL3T1 : UIBase
    {
        #region Fields


        #endregion

        #region Creator

        /// <summary>
        /// This creates instance of this object.
        /// </summary>
        public UITemplateL3T1()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private new void InitializeComponent()
        {
            this.BaseA = new TAP.UIControls.BasicControls.TPanel();
            this.BaseC = new TAP.UIControls.BasicControls.TPanel();
            this.BaseB = new TAP.UIControls.BasicControls.TPanel();
            this.tPanelBottomBase.SuspendLayout();
            this.tPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tPanelBottomBase
            // 
            this.tPanelBottomBase.Location = new System.Drawing.Point(0, 696);
            // 
            // tPanelMain
            // 
            this.tPanelMain.Controls.Add(this.BaseB);
            this.tPanelMain.Controls.Add(this.BaseC);
            this.tPanelMain.Controls.Add(this.BaseA);
            this.tPanelMain.Size = new System.Drawing.Size(1012, 676);
            // 
            // BaseA
            // 
            this.BaseA.ControlID = "A";
            this.BaseA.Dock = System.Windows.Forms.DockStyle.Top;
            this.BaseA.IsRequired = false;
            this.BaseA.Location = new System.Drawing.Point(0, 0);
            this.BaseA.Name = "BaseA";
            this.BaseA.NeedToTranslate = true;
            this.BaseA.Padding = new System.Windows.Forms.Padding(3);
            this.BaseA.RepresentativeValue = "BaseA [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms.Bo" +
    "rderStyle.None";
            this.BaseA.Size = new System.Drawing.Size(1012, 50);
            this.BaseA.TabIndex = 0;
            // 
            // BaseC
            // 
            this.BaseC.ControlID = "C";
            this.BaseC.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BaseC.IsRequired = false;
            this.BaseC.Location = new System.Drawing.Point(0, 576);
            this.BaseC.Name = "BaseC";
            this.BaseC.NeedToTranslate = true;
            this.BaseC.Padding = new System.Windows.Forms.Padding(3);
            this.BaseC.RepresentativeValue = "BaseC [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms.Bo" +
    "rderStyle.None";
            this.BaseC.Size = new System.Drawing.Size(1012, 100);
            this.BaseC.TabIndex = 1;
            // 
            // BaseB
            // 
            this.BaseB.ControlID = "tPanel1";
            this.BaseB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BaseB.IsRequired = false;
            this.BaseB.Location = new System.Drawing.Point(0, 50);
            this.BaseB.Name = "BaseB";
            this.BaseB.NeedToTranslate = true;
            this.BaseB.Padding = new System.Windows.Forms.Padding(3);
            this.BaseB.RepresentativeValue = "BaseB [TAP.UIControls.BasicControls.TPanel], BorderStyle: System.Windows.Forms.Bo" +
    "rderStyle.None";
            this.BaseB.Size = new System.Drawing.Size(1012, 526);
            this.BaseB.TabIndex = 2;
            // 
            // UITemplateL3T1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 716);
            this.Name = "UITemplateL3T1";
            this.Text = "UITemplateV2";
            this.tPanelBottomBase.ResumeLayout(false);
            this.tPanelBottomBase.PerformLayout();
            this.tPanelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}