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
    public partial class FrmDropPartitionin : XtraForm
    {

        public string partitionName { get; set; } 
        public string objectName { get; set; }

        public FrmDropPartitionin()
        {
            InitializeComponent();
            textpartitionName.TextAlign = DevExpress.Utils.HorzAlignment.Near;
            textobjectName.TextAlign = DevExpress.Utils.HorzAlignment.Near;
        }

        private void FrmAddPartitionin_Load(object sender, EventArgs e)
        {
            
            textobjectName.Text = objectName;
            textpartitionName.Text = partitionName;
        }

        private void tButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }





        private void CheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckedListBoxControl checkedListBoxControl =  sender as CheckedListBoxControl;

            checkedListBoxControl.UnCheckAll();

            int index = checkedListBoxControl.SelectedIndex;

            checkedListBoxControl.SetItemChecked(index,true);
            
        }

        private void tCheckedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            tCheckedListBox1.UnCheckAll();

            int index = tCheckedListBox1.SelectedIndex;

            tCheckedListBox1.SetItemChecked(index, true);
        }
    }
}
