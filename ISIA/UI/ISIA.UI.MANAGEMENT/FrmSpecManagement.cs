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
    public partial class FrmSpecManagement : DockUIBase1T1
    {
        #region Feild
        BizDataClient bs = null;
        SpecManagementArgsPack args =null;

        DataSet ds = new DataSet();

        #endregion
        public FrmSpecManagement()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.SpecManagement");
            
        }

        public DataSet LoadData()
        {

            args = new SpecManagementArgsPack();
            args.RULENAME = txtMainRULENAME.Text;
            args.RULETEXT = txtMainRULETEXT.Text;
            args.RULENO = txtMainRULENO.Text;
            args.INSERTTIME = DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmmss");
            args.UPDATETIME = DateTime.Now.AddHours(1).ToString("yyyyMMddHHmmss");
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
            if (string.IsNullOrEmpty(txtAddRULENAME.Text))
            {
                txtAddRULENAME.BackColor = Color.Orange;
                return;
            }
            if (string.IsNullOrEmpty(txtAddRuleNo.Text))
            {

                txtAddRuleNo.BackColor = Color.Orange;
                return;
            }
            if (string.IsNullOrEmpty(txtAddSequences.Text))
            {
                txtAddSequences.BackColor = Color.Orange;
                return;
            }
            if (string.IsNullOrEmpty(txtAddRULETEXT.Text))
            {
                txtAddRULETEXT.BackColor = Color.Orange;
                return;
            }
            args = new SpecManagementArgsPack();
            args.RULENAME = txtAddRULENAME.Text;
            args.RULENO = txtAddRuleNo.Text;
            args.SEQUENCES = txtAddSequences.Text;
            args.RULETEXT = txtAddRULETEXT.Text;
            args.ISALIVE= rdoIsalive.Properties.Items[rdoIsalive.SelectedIndex].Value.ToString();
            DataSet dst = bs.ExecuteDataSet("CheckTcode", args.getPack());
            if (dst==null||dst.Tables==null||dst.Tables[0].Rows.Count==0)
            {
                args.INSERTUSER = TAP.UI.InfoBase._USER_INFO.Name;
                args.INSERTTIME = DateTime.Now.ToString("yyyyMMddHHmmss");
                int i = bs.ExecuteModify("SaveTCode", args.getPack());
            }
            else
            {
                args.UPDATETIME= DateTime.Now.ToString("yyyyMMddHHmmss");
                args.UPDATEUSER = TAP.UI.InfoBase._USER_INFO.Name;
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
                txtAddRULENAME.Text = dr["RULENAME"].ToString();
                txtAddRuleNo.Text = dr["RULENO"].ToString();
                txtAddRULETEXT.Text= dr["RULETEXT"].ToString();
                txtAddSequences.Text= dr["SEQUENCES"].ToString();
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
                        args = new SpecManagementArgsPack();
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
