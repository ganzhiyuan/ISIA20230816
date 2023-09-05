using DevExpress.XtraEditors;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Data;
using System.Text;
using TAP.Data.Client;
using TAP.UI;

namespace ISIA.UI.MANAGEMENT
{
    

    public partial class FrmShowPartitioninSql : XtraForm
    {

        public delegate void MyDelegate();
        public event MyDelegate MyEvent;

        BizDataClient bs = null;
        AwrArgsPack args = null;


        public StringBuilder partitionsql { get; set; } 

        public FrmShowPartitioninSql()
        {
            InitializeComponent();
            args = new AwrArgsPack();
            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.CreateDataTableManagement");
        }

        private void FrmShowPartitioninSql_Load(object sender, EventArgs e)
        {
            memoEditpratitionsql.Text = partitionsql.ToString();
        }

        private void tButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tButton2_Click(object sender, EventArgs e)
        {
            args.PartitionSql = partitionsql;
            MyEvent?.Invoke();
            this.Close();
            return;
            int ds = bs.ExecuteModify("AlterPartition", args.getPack());

            if (ds == -1)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage("Success", EnumMsgType.CONFIRM, "Drop Partition success!");
                
                this.Close();
                
            }
            else
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage("Warring", EnumMsgType.WARNING, "Drop Partition fail!");
                this.Close();
            }

            
        }

        private void FrmShowPartitioninSql_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            
        }
    }
}
