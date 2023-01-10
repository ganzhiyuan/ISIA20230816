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
    /// This class is UI Template Type1 of Layer 4
    /// </summary>
    [Designer(typeof(TAP.UI.UIBaseDesigner), typeof(IRootDesigner))]
    public partial class UITemplateL4T1 : UIBase 
    {
        /// <summary>
        /// This creates intance of this object.
        /// </summary>
        public UITemplateL4T1()
        {
            InitializeComponent(); 
        }
    }
}
