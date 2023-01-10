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

namespace TAP.UI.UITemplate2
{
    /// <summary>
    /// This class is UI templage Type 1A of Layer 2.
    /// </summary>
    [Designer(typeof(TAP.UI.UIBaseDesigner), typeof(IRootDesigner))]
    public partial class UITemplate2T1A : UIBase
    {
        /// <summary>
        /// This creates instance of this object
        /// </summary>
        public UITemplate2T1A()
        {
            InitializeComponent();
        }
    }
}
