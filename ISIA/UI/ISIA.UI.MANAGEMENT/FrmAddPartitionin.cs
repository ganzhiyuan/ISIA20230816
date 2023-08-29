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

        public string partitionName { get; set; } 
        public string upperBound { get; set; }

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
    }
}
