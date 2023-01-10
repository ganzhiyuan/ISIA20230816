using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TAP.UI;

namespace ISIA.UI.BASE
{
    [Designer(typeof(TAP.UI.UIBaseDesigner), typeof(IRootDesigner))]
    public partial class TemplateSpec : UIBase
    {
        public TemplateSpec()
        {
            InitializeComponent();
        }
    }
}
