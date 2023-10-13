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
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.Fressage;
using TAP.UI;

namespace ISIA.UI.MANAGEMENT
{
    public partial class FrmTableSpaceManagement : DockUIBase1T1
    {
        #region Feild
        BizDataClient bs = null;
        DataBaseManagementArgsPack args =null;

        DataSet ds = new DataSet();

        #endregion
        public FrmTableSpaceManagement()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.DataBaseManagement");
            //dateStart.DateTime = DateTime.Now.AddDays(-1);
            //dateEnd.DateTime = DateTime.Now;
            //dtStart.DateTime = DateTime.Now.AddDays(-1);
            //dtEnd.DateTime = DateTime.Now;
        }

        public DataSet LoadData()
        {

            args = new DataBaseManagementArgsPack();
            
            ds = bs.ExecuteDataSet("GetDB", args.getPack());

            return ds;
        }

        public void DisplayData(DataSet ds)
        {
            if (ds == null)
            {
                return;
            }
            GridViewDataBinding();
        }

        public void GridViewDataBinding()
        {


            
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!base.ValidateUserInput(this.layoutControl1)) return;
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

       

        private void txtSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCATEGORY.Text))
            {
                txtCATEGORY.BackColor = Color.Orange;
                return;
            }
            if (string.IsNullOrEmpty(txtSUBCATEGORY.Text))
            {
                txtSUBCATEGORY.BackColor = Color.Orange;
                return;
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                txtName.BackColor = Color.Orange;
                return;
            }
            if (string.IsNullOrEmpty(txtCustome.Text))
            {
                txtCustome.BackColor = Color.Orange;
                return;
            }
            args = new DataBaseManagementArgsPack();
            args.CATEGORY = txtCATEGORY.Text;
            args.CUSTOM01 = txtCustome.Text;
            args.DESCRIPTION = txtDESCRIPTION.Text;
            args.NAME = txtName.Text;
            args.SEQUENCES = txtSEQUENCES.Text;
            args.SUBCATEGORY = txtSUBCATEGORY.Text;
            args.USED= rdoUsed.Properties.Items[rdoUsed.SelectedIndex].Value.ToString(); 
            args.ISALIVE= rdoIsalive.Properties.Items[rdoIsalive.SelectedIndex].Value.ToString();
            DataSet dst = bs.ExecuteDataSet("CheckTcode", args.getPack());
            if (dst==null||dst.Tables==null||dst.Tables[0].Rows.Count==0)
            {
                int i = bs.ExecuteModify("SaveTCode", args.getPack());
            }
            else
            {
                int i = bs.ExecuteModify("UpdateTcode", args.getPack());
            }

            TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.WARNING, "Success.");
            btnSelect_Click(null, null);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            
        }

      
       
        private void txtCATEGORY_TextChanged(object sender, EventArgs e)
        {
            SetText(sender as TextEdit);
        }

        private void SetText(TextEdit txt)
        {
            if (!string.IsNullOrEmpty(txt.Text))
            {
                txt.BackColor = Color.White;
            }
        }
    }
}
