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
    public partial class FrmShowPartitioninSql : XtraForm
    {

        public string partitionName { get; set; } 
        public string upperBound { get; set; }

        public FrmShowPartitioninSql()
        {
            InitializeComponent();
        }

        private void FrmAddPartitionin_Load(object sender, EventArgs e)
        {

        }

        private void tButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
