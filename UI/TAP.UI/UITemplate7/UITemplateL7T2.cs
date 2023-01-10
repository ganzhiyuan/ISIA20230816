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


namespace TAP.UI.UITemplate7
{
    /// <summary>
    /// This class is UI templage Type 2 of Layer 7
    /// </summary>
    [Designer(typeof(TAP.UI.UIBaseDesigner), typeof(IRootDesigner))]
    public partial class UITemplateL7T2 : UIBase
    {
        /// <summary>
        /// This creates intance of UITemplateB2
        /// </summary>
        public UITemplateL7T2()
        {
            InitializeComponent();
        }
    }
}
