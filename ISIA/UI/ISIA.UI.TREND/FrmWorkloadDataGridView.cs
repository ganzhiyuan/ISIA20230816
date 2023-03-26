using DevExpress.XtraEditors;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.UI;

namespace ISIA.UI.TREND
{
    public partial class FrmWorkloadDataGridView : DevExpress.XtraEditors.XtraForm
    {

        private DataTable _ResultForNextPageDt;

        private DataRow _FocusedRowDr;

        private DataTable _IncomingDt;

        public AwrArgsPack args = null;

        private string _NextMenuName;

        private string _NextMainMenuName;

        private string _NextMenuDisplayName;



        BizDataClient BsForGettingSqlPaemCorrelation = new BizDataClient("ISIA.BIZ.ANALYSIS.DLL", "ISIA.BIZ.ANALYSIS.WorkloadSqlCorrelationAnalysis");

        BizDataClient BsForGettingSqlInfluence = new BizDataClient("ISIA.BIZ.ANALYSIS.DLL", "ISIA.BIZ.ANALYSIS.SqlInfluenceAnalysis");


        public FrmWorkloadDataGridView(DataTable dt)
        {
            InitializeComponent();
            IncomingDt = dt;
            this.gridControlWorkloadData.DataSource = null;
            this.gridView1.Columns.Clear();
            this.gridControlWorkloadData.DataSource = IncomingDt;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.BestFitColumns();
        }

        public DataTable ResultForNextPageDt { get => _ResultForNextPageDt; set => _ResultForNextPageDt = value; }
        public DataRow FocusedRowDr { get => _FocusedRowDr; set => _FocusedRowDr = value; }
        public DataTable IncomingDt { get => _IncomingDt; set => _IncomingDt = value; }
        public string NextMenuName { get => _NextMenuName; set => _NextMenuName = value; }
        public string NextMainMenuName { get => _NextMainMenuName; set => _NextMainMenuName = value; }
        public string NextMenuDisplayName { get => _NextMenuDisplayName; set => _NextMenuDisplayName = value; }

        private void gridView1_MouseUp(object sender, MouseEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = this.gridView1.CalcHitInfo(e.Location);
            if (hi.InRow && e.Button == MouseButtons.Right)
            {
                this.popupMenu1.ShowPopup(Control.MousePosition);
            }
        }


        private void barButtonItemCorrelation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //GET data 
                args = new AwrArgsPack();
                args.DBName = FocusedRowDr.Field<string>("DbName");
                args.WorkloadSqlParm = AwrArgsPack.WorkloadRealParmMapping[FocusedRowDr.Field<string>("WorkloadParm")];
                List<DateTime> dateTimes = IncomingDt.AsEnumerable().Select(x => x.Field<DateTime>("Time")).ToList();
                dateTimes.Sort();
                args.StartTime = dateTimes[0].ToString("yyyyMMdd")+"000000";
                args.EndTime = dateTimes[dateTimes.Count - 1].ToString("yyyyMMdd")+"235959";
                DataSet ds = BsForGettingSqlPaemCorrelation.ExecuteDataSet("GetWorkloadSqlCorrelationData", args.getPack());
                ResultForNextPageDt = ds.Tables[0];
                NextMainMenuName = "ANALYSIS";
                NextMenuDisplayName = "Sql Parm Correlation Analysis";
                NextMenuName = "WORKLOADSQLCORRELATIONANALYSIS";
                this.Close();
            }
            catch(Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(TAP.UI.EnumMsgType.CONFIRM, ex.Message);
            }
        }

        private void barButtonItemSqlTopTen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //GET data 
                args = new AwrArgsPack();
                args.DBName = FocusedRowDr.Field<string>("DbName");
                args.WorkloadSqlParm = "PHYSICAL_READ_REQUESTS_DELTA";
                List<DateTime> dateTimes = IncomingDt.AsEnumerable().Select(x => x.Field<DateTime>("Time")).ToList();
                dateTimes.Sort();
                args.StartTime = dateTimes[0].ToString("yyyyMMdd") + "000000";
                args.EndTime = dateTimes[dateTimes.Count - 1].ToString("yyyyMMdd") + "235959";
                DataSet ds = BsForGettingSqlInfluence.ExecuteDataSet("GetSqlInfluenceData", args.getPack());
                ResultForNextPageDt = ds.Tables[0];
                NextMainMenuName = "ANALYSIS";
                NextMenuDisplayName = "Sql Influence Analysis";
                NextMenuName = "SQLINFLUENCEANALYSIS";
                this.Close();
            }
            catch (Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(TAP.UI.EnumMsgType.CONFIRM, ex.Message);
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            FocusedRowDr = gridView1.GetDataRow(e.FocusedRowHandle) as DataRow;
        }
    }
}