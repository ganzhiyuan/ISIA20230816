using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.UI;
using ISIA.UI.BASE;
using DevExpress.XtraEditors.Controls;
using TAP.UIControls.BasicControlsDEV;
using UIHelper.UIServiceImpl.Analysis.UI.FrmOrclParmsTrend;
using ISIA.UI.ANALYSIS.UIHelper.UIServiceImpl.Analysis.UI.FrmOrclParmsTrend;

namespace ISIA.UI.ANALYSIS
{
    public partial class FrmOrclParmsTrendChart : DockUIBase1T1
    {
        public FrmOrclParmsTrendChart()
        {
            InitializeComponent();
            new InitializationUIService(this, null).Run();
            new InitializationComboxParmTypeUIService(this, null).Run();
        }

     

        private void btnSelect_Click(object sender, EventArgs e)
        {
            new SerchUiService(this, e).Run();
        }

      
        private void tCheckComboBoxParmType_EditValueChanged(object sender, EventArgs e)
        {
            new ShowComboxParmNamesUIService(this, e).Run();

        }
    }
}
