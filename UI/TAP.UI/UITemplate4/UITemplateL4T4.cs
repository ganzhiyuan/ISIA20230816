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

namespace TAP.UI.UITemplate4
{
    /// <summary>
    /// This class is UI templage Vectical-Type#4.
    /// </summary>
    [Designer(typeof(TAP.UI.UIBaseDesigner), typeof(IRootDesigner))]
    public partial class UITemplateL4T4 : UIBase
    {
        /// <summary>
        /// This is UI Template 4LT4
        /// </summary>
        public UITemplateL4T4()
        {
            InitializeComponent();
        }

        private void BaseA_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
