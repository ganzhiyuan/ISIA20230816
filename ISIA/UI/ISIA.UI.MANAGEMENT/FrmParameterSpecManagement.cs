using DevExpress.Utils.Win;
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
    public partial class FrmParameterSpecManagement : DockUIBase1T1
    {
        #region Feild
        BizDataClient bs = null;
        ParameterSpecManagementArgsPack args =null;

        DataSet ds = new DataSet();

        #endregion
        public FrmParameterSpecManagement()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.ParameterSpecManagement");
            //dateStart.DateTime = DateTime.Now.AddDays(-1);
            //dateEnd.DateTime = DateTime.Now;
        }

        public DataSet LoadData()
        {

            args = new ParameterSpecManagementArgsPack();
            args.DBID = cmbDbName.EditValue.ToString();
            args.PARAMETERNAME = txtParam.Text;
            args.RULENAME = txtRuleName.Text;
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
        private void cmbDbName_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            Form popup = (sender as IPopupControl).PopupWindow as Form;
            PopupContainerControl container = popup.Controls.OfType<PopupContainerControl>().FirstOrDefault();
            CheckedListBoxControl listBox = container.Controls.OfType<CheckedListBoxControl>().FirstOrDefault();

            for (int i = 0; i < listBox.Items.Count; i++)
            {
                if (i != e.Index)
                {
                    listBox.Items[i].CheckState = CheckState.Unchecked;
                }

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
            if (string.IsNullOrEmpty(txtAddParaId.Text))
            {
                txtAddParaId.BackColor = Color.Orange;
                return;
            }
            if (string.IsNullOrEmpty(txtAddParaName.Text))
            {

                txtAddParaName.BackColor = Color.Orange;
                return;
            }
            if (string.IsNullOrEmpty(txtAddDays.Text))
            {
                txtAddDays.BackColor = Color.Orange;

                return;
            }
            if (string.IsNullOrEmpty(txtControlLow.Text))
            {
                txtControlLow.BackColor = Color.Orange;
                return;
            }
            if (string.IsNullOrEmpty(txtControlUpper.Text))
            {
                txtControlUpper.BackColor = Color.Orange;
                return;
            }
            if (string.IsNullOrEmpty(txtSpecLow.Text))
            {
                txtSpecLow.BackColor = Color.Orange;
                return;
            }
            if (string.IsNullOrEmpty(txtSpecUpper.Text))
            {
                txtSpecUpper.BackColor = Color.Orange;
                return;
            }
            args = new ParameterSpecManagementArgsPack();
            args.DBID = cmbAddDbName.EditValue.ToString();
            args.PARAMETERID = txtAddParaId.Text;
            args.PARAMETERNAME = txtAddParaName.Text;
            args.RULENAME = txtAddRuleNm.Text;
            args.RULENO = txtAddRuleNo.Text;
            args.DAYS = txtAddDays.Text;
            args.SPECUPPERLIMIT = txtSpecUpper.EditValue.ToString();
            args.SPECLOWERLIMIT = txtSpecLow.EditValue.ToString();
            args.CONTROLLOWERLIMIT = txtControlLow.EditValue.ToString();
            args.CONTROLUPPERLIMIT = txtControlUpper.EditValue.ToString();
            args.ISALIVE = rdoIsalive.Properties.Items[rdoIsalive.SelectedIndex].Value.ToString();
            args.CHARTUSED = rdoIsalive.Properties.Items[rdoChartused.SelectedIndex].Value.ToString();
            args.MAILUSED = rdoIsalive.Properties.Items[rdoIlused.SelectedIndex].Value.ToString();
            args.MMSUSED = rdoIsalive.Properties.Items[rdoMMsused.SelectedIndex].Value.ToString();
            args.SPECLIMITUSED = rdoIsalive.Properties.Items[rdoSpec.SelectedIndex].Value.ToString();

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
                cmbAddDbName.EditValue = dr["DBID"];
                txtAddParaId.Text = dr["PARAMETERID"].ToString();
                txtAddParaName.Text = dr["PARAMETERNAME"].ToString();
                txtAddRuleNm.Text= dr["RULENAME"].ToString();
                txtAddRuleNo.Text= dr["RULENO"].ToString();
                txtAddDays.Text = dr["DAYS"].ToString();
                txtSpecUpper.Text = dr["SPECUPPERLIMIT"].ToString();
                txtSpecLow.Text = dr["SPECLOWERLIMIT"].ToString();
                txtControlLow.Text = dr["CONTROLLOWERLIMIT"].ToString();
                txtControlUpper.Text = dr["CONTROLUPPERLIMIT"].ToString();
                if (dr["ISALIVE"].ToString() == "YES")
                {
                    rdoIsalive.SelectedIndex = 0;
                }
                else
                {
                    rdoIsalive.SelectedIndex = 1;
                }

                if (dr["CHARTUSED"].ToString() == "YES")
                {
                    rdoChartused.SelectedIndex = 0;
                }
                else
                {
                    rdoChartused.SelectedIndex = 1;
                }

                if (dr["MAILUSED"].ToString() == "YES")
                {
                    rdoIlused.SelectedIndex = 0;
                }
                else
                {
                    rdoIlused.SelectedIndex = 1;
                }

                if (dr["MMSUSED"].ToString() == "YES")
                {
                    rdoMMsused.SelectedIndex = 0;
                }
                else
                {
                    rdoMMsused.SelectedIndex = 1;
                }
                if (dr["SPECLIMITUSED"].ToString()=="YES")
                {
                    rdoSpec.SelectedIndex = 0;
                }
                else
                {
                    rdoSpec.SelectedIndex = 1;
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
                        args = new ParameterSpecManagementArgsPack();
                        args.ROWID = dr["ROWID"].ToString();
                        bs.ExecuteModify("DelteParameterSpec", args.getPack());
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
