using DevExpress.XtraEditors;
using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;

namespace ISIA.UI.ANALYSIS
{
    public partial class FrmPartitionJobAndAlertInfo : DockUIBase1T1
    {

        public string JobRowCount;
        public string AlertRowCount;
        BizDataClient bs = new BizDataClient("ISIA.BIZ.ANALYSIS.DLL", "ISIA.BIZ.ANALYSIS.PartitionJobAndAlertInfo");
        PartitionJobAlertArgsPack arg = new PartitionJobAlertArgsPack();


        public FrmPartitionJobAndAlertInfo()
        {
            InitializeComponent();
            JobRowCount = comboBoxEditJobCount.Text;
            AlertRowCount = comboBoxEditErrorCount.Text;
            timerAlert.Enabled = true;
            timerJob.Enabled = true;
            timerAlert.Interval = int.Parse(comboBoxEditErrorInterval.Text) * 1000;
            timerJob.Interval = int.Parse(comboBoxEditJobInterval.Text) * 1000;
        }


        private void timerAlert_Tick(object sender, EventArgs e)
        {
            ShowAlertMessage();
        }

        private void timerJob_Tick(object sender, EventArgs e)
        {
            ShowJobMessage();
        }



        private void ShowJobMessage()
        {
            arg.RowAlertCount = AlertRowCount;
            arg.RowJobCount = JobRowCount;
            DataSet ds = bs.ExecuteDataSet("GetJobInfo", arg.getPack());
            gridControlJob.DataSource = ds.Tables[0];

        }
        private async void ShowAlertMessage()
        { Console.WriteLine($"Main Thread - {Thread.CurrentThread.ManagedThreadId}");
            arg.RowAlertCount = AlertRowCount;
            arg.RowJobCount = JobRowCount;
            DataSet ds = await GetAlertData();

            Console.WriteLine($"Async Thread - {Thread.CurrentThread.ManagedThreadId}");
            gridControlError.DataSource = ds.Tables[0];

        }

        private   Task<DataSet> GetAlertData()
        {
            return  Task.Factory.StartNew(() =>
            {
                Thread.Sleep(10000);

                return bs.ExecuteDataSet("GetAlertInfo", arg.getPack());
            });
        }

        private void RowCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            JobRowCount = comboBoxEditJobCount.Text;
            AlertRowCount = comboBoxEditErrorCount.Text;
        }

        private void TimerInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerAlert.Interval = int.Parse(comboBoxEditErrorInterval.Text) * 1000;
            timerJob.Interval = int.Parse(comboBoxEditJobInterval.Text) * 1000;
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            string errMessage = gridView1.GetRowCellValue(e != null ? e.RowHandle : 0, "MESSAGE_TEXT").ToString();
            PopupMemo MessageFrm = new PopupMemo(errMessage,  "Alert Log Message");
            MessageFrm.ShowDialog();

        }
    }
}