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
using ISIA.UI.ANALYSIS.UIHelper.UIServiceImpl.Analysis.UI.FrmWorkload;
using UIHelper;
using Steema.TeeChart;
using UIHelper.UIServiceImpl.Analysis.UI.FrmWorkload;
using Steema.TeeChart.Tools;
using Steema.TeeChart.Styles;
using System.Threading;

namespace ISIA.UI.ANALYSIS
{
    public partial class FrmWorkloadAnalysis : DockUIBase1T1
    {
        TChart currentChart = null;
        public FrmWorkloadAnalysis()
        {
            InitializeComponent();
            new InitializationUIService(this, null, new AwrArgsPack()).Run();

        }

        private int SeriesIndex = -1;

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                SearchUiService searchUiService = new SearchUiService(this, e, new AwrArgsPack());
                searchUiService.RunAsync();
                //tool tip 
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       


        private void tCheckComboBoxParmType_EditValueChanged(object sender, EventArgs e)
        {


        }



        private void editChartToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            currentChart.ShowEditor();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            currentChart = (sender as ContextMenuStrip).SourceControl as TChart;
        }


    }
}
