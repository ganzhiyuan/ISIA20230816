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
    public partial class TemplateChart : UIBase
    {
        private bool _AutoSize = true;

        public TemplateChart()
        {
            InitializeComponent();
            SplitSplitterPositionChange(SplitA1A);
        }

        private void TemplateChart_ClientSizeChanged(object sender, EventArgs e)
        {
            SplitSplitterPositionChange(SplitA1A);
        }

        private void SplitSplitterPositionChange(DevExpress.XtraEditors.SplitContainerControl scc)
        {
            try
            {
                if (_AutoSize)
                {
                    int height = this.Size.Height;
                    double dheight = height / 1.8;
                    scc.SplitterPosition = (int)dheight;
                }
            }
            catch(System.Exception ex)
            {
                //메시지박스 추가 
            }
        }

        public void SetSplitAutoSize(bool Bool)
        {
            _AutoSize = Bool;
        }
    }
}
