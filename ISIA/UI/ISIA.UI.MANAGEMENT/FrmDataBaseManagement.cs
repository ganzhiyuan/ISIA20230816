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
    public partial class FrmDataBaseManagement : DockUIBase1T1
    {
        #region Feild
        BizDataClient bs = null;
        DataBaseManagementArgsPack args =null;

        DataSet ds = new DataSet();

        #endregion
        public FrmDataBaseManagement()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.DataBaseManagement");
            dateStart.DateTime = DateTime.Now.AddDays(-1);
            dateEnd.DateTime = DateTime.Now;
            dtStart.DateTime = DateTime.Now.AddDays(-1);
            dtEnd.DateTime = DateTime.Now;
        }

        public DataSet LoadData()
        {

            args = new DataBaseManagementArgsPack();
            args.CATEGORY = txtMainCategory.Text;
            args.CUSTOM01 = txtMainCustom.Text;
            args.NAME = txtMainName.Text;
            args.SUBCATEGORY = txtMainSub.Text;
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
            gridControl1.DataSource = ds.Tables[0];
            gridView1.BestFitColumns();
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridControl1.DataSource != null)
                {
                    SaveFileDialog fileDialog = new SaveFileDialog();
                    fileDialog.Title = "Excel";
                    fileDialog.Filter = "Excel(*.xls)|*.xls";
                    DialogResult dialogResult = fileDialog.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        gridView1.OptionsPrint.AutoWidth = false;
                        try
                        {
                            gridView1.ExportToXls(fileDialog.FileName);

                        }
                        catch
                        {
                            TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.INFORMATION, "  Excel is opening.");
                        }
                    }
                }
                else
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.INFORMATION, "  Export Failed.");
                }
            }
            catch (System.Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
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
            args.ISALIVE= rdoIsalive.Properties.Items[rdoUsed.SelectedIndex].Value.ToString();
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
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle) as DataRow;
            if (dr!=null)
            {
                tabPane1.SelectedPage = tabNavigationPage2;
                txtCATEGORY.Text = dr["CATEGORY"].ToString();
                txtCustome.Text = dr["CUSTOM01"].ToString();
                txtDESCRIPTION.Text= dr["DESCRIPTION"].ToString();
                txtName.Text= dr["NAME"].ToString();
                txtSEQUENCES.Text = dr["SEQUENCES"].ToString();
                txtSUBCATEGORY.Text = dr["SUBCATEGORY"].ToString();
                if (dr["USED"].ToString()=="YES")
                {
                    rdoUsed.SelectedIndex = 0;
                }
                else
                {
                    rdoUsed.SelectedIndex = 1;
                }
                if (dr["ISALIVE"].ToString() == "YES")
                {
                    rdoIsalive.SelectedIndex = 0;
                }
                else
                {
                    rdoIsalive.SelectedIndex = 1;
                }

            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            int[] i = gridView1.GetSelectedRows();
            if (i==null&&i.Count()<1)
            {
                return;
            }
            string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.UPDATE, EnumGeneralTemplateType.CONFIRM, "");
            DialogResult dialog = TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.CONFIRM, tmpMessage);
            if (dialog.ToString() == "Yes")
            {
                foreach (var item in i)
                {
                    DataRow dr = gridView1.GetDataRow(i[0]) as DataRow;
                    if (dr != null)
                    {
                        args = new DataBaseManagementArgsPack();
                        args.ROWID = dr["ROWID"].ToString();
                        bs.ExecuteModify("DelteTCODE", args.getPack());
                    }

                }
                TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.WARNING, "Success.");
                btnSelect_Click(null, null);
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle) as DataRow;
            if (dr != null)
            {
                btnUpdate_Click(null, null);
            }
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
