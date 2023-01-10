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
    /// This class is UI templage Vectical-Type#3.
    /// </summary>
    [Designer(typeof(TAP.UI.UIBaseDesigner), typeof(IRootDesigner))]
    public partial class UITemplateL4T3 : UIBase
    {
        /// <summary>
        /// This creates instance of this object
        /// </summary>
        public UITemplateL4T3()
        {
            InitializeComponent();
        }
    }
}
