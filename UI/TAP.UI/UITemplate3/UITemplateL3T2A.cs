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
    /// This class is UI templage Type 2 of Layer 3.
    /// </summary>
    [Designer(typeof(TAP.UI.UIBaseDesigner), typeof(IRootDesigner))]
    public partial class UITemplateL3T2A : UIBase
    {
        /// <summary>
        /// This creates instance of this object
        /// </summary>
        public UITemplateL3T2A()
        {
            InitializeComponent();
        }
    }
}
