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

namespace TAP.UI.UITemplateComplex
{
    /// <summary>
    /// This class is UI templage Vectical-Type#X-MINA
    /// </summary>
    [Designer(typeof(TAP.UI.UIBaseDesigner), typeof(IRootDesigner))]
    public partial class UITemplateXMINALCD : UIBase
    {
        /// <summary>
        /// This creates instance of this.
        /// </summary>
        public UITemplateXMINALCD()
        {
            InitializeComponent();
        }
    }
}
