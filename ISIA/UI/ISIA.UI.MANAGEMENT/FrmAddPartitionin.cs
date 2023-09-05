using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISIA.UI.MANAGEMENT
{
    public partial class FrmAddPartitionin : XtraForm
    {
        public delegate void MyDelegate();
        public event MyDelegate MyEvent;

        public string partitionName { get; set; } 
        public string upperBound { get; set; }
        public string objectName { get; set; }

        public FrmAddPartitionin()
        {
            InitializeComponent();
            textpartitionName.TextAlign = DevExpress.Utils.HorzAlignment.Near;
            textcolumsName.TextAlign = DevExpress.Utils.HorzAlignment.Near;
            textdataType.TextAlign = DevExpress.Utils.HorzAlignment.Near;
            textupperBound.TextAlign = DevExpress.Utils.HorzAlignment.Near;
        }

        private void FrmAddPartitionin_Load(object sender, EventArgs e)
        {
            
            string columsName = "BEGIN_TIME";
            string dateType = "DATE";
            textpartitionName.Text = partitionName;
            textcolumsName.Text = columsName;
            textdataType.Text = dateType;
            textupperBound.Text = upperBound;
        }

        private void tButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tButton2_Click(object sender, EventArgs e)
        {
            this.Close();
            /*StringBuilder stringBuilder = new StringBuilder();*/
            StringBuilder strsql = new StringBuilder();

            strsql.AppendFormat("ALTER TABLE {0} ADD PARTITION {1} VALUES LESS THAN ({2})", objectName, textpartitionName.Text, textupperBound.Text);

            FrmShowPartitioninSql frmShowPartitioninSql = new FrmShowPartitioninSql();
            frmShowPartitioninSql.partitionsql = strsql;
            frmShowPartitioninSql.StartPosition = FormStartPosition.CenterScreen;
            frmShowPartitioninSql.MyEvent += closeSqlPar;
            frmShowPartitioninSql.Show();

        }

        private void closeSqlPar()
        {
            MyEvent?.Invoke();
        }
    }
}
