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


namespace TAP.UI.UITemplate5
{
    /// <summary>
    /// This class is UI templage Type 2 of Layer 5
    /// </summary>
    [Designer(typeof(TAP.UI.UIBaseDesigner), typeof(IRootDesigner))]
    public partial class UITemplateL5T2 : UIBase
    {
        /// <summary>
        /// This creates intance of this object.
        /// </summary>
        public UITemplateL5T2()
        {
            InitializeComponent();
        }
    }
}
