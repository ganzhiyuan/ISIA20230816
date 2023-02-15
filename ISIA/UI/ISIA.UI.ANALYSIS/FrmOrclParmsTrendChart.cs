﻿using DevExpress.Utils;
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
using UIHelper;
using Steema.TeeChart;

namespace ISIA.UI.ANALYSIS
{
    public partial class FrmOrclParmsTrendChart : DockUIBase1T1
    {
        TChart currentChart = null;
        public FrmOrclParmsTrendChart()
        {
            InitializeComponent();
            new InitializationUIService(this, null, null).Run();
            new InitializationComboxParmTypeUIService(this, null, null).Run();
        }


        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                new SerchUiService(this, e, new AwrArgsPack()).Run();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
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
