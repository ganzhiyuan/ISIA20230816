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

using DevExpress.XtraEditors.Controls;
using TAP.UIControls.BasicControlsDEV;
using UIHelper;
using Steema.TeeChart;
using ISIA.UI.ANALYSIS.UIService.UIServiceImpl.ANALYSIS.UI.ParameterClusteringAnalysis;
using ISIA.UI.BASE;
using TAP.UI;

namespace ISIA.UI.ANALYSIS
{
    

    public partial class FrmParameterClusteringAnalysis : DockUIBase1T1
    {
        TChart currentChart = null;
        

        public FrmParameterClusteringAnalysis()
        {
            InitializeComponent();

            
            
            /*new InitializationUIService(this, null, new AwrArgsPack()).Run();
            new InitializationComboxParmTypeUIService(this, null, null).Run();*/
            dtpStartTime.DateTime = DateTime.Now.AddDays(-1);
            dtpEndTime.DateTime = DateTime.Now;

            

        }


        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                
                new SerchUiService(this, e, new AwrArgsPack()).RunAsync();
            }
            catch (Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(TAP.UI.EnumMsgType.CONFIRM, ex.Message);
            }
        }


        private void tCheckComboBoxParmType_EditValueChanged(object sender, EventArgs e)
        {
            new ShowComboxParmNamesUIService(this, e, null).Run();
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
