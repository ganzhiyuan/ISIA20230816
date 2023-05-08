using DevExpress.Utils.Win;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
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
using TAP.UIControls.BasicControlsDEV;

namespace ISIA.UI.MANAGEMENT
{
    public partial class FrmParameterSpecManagement : DockUIBase1T1
    {
        #region Feild
        BizDataClient bs = null;
        ParameterSpecManagementArgsPack args =null;

        DataSet ds = new DataSet();
        DataTable dtparameterid = null;
        DataTable dtruleid = null;

        #endregion
        public FrmParameterSpecManagement()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.ParameterSpecManagement");
            dtparameterid = bs.ExecuteDataTable("GetIdName");
            searchid.Properties.DataSource = dtparameterid;
            searchid.Properties.DisplayMember = "PARAMETERNAME";
            searchid.Properties.ValueMember = "PARAMETERID";
            
            //setting触发控件
            cmbRuleName.Setting();
            cmbAddDbName.Setting();
            //将cmbRuleName中value的值改变为description
            foreach (CheckedListBoxItem items in cmbRuleName.Properties.Items)
            {
                items.Value = items.Description.ToString();
            }

            //dateStart.DateTime = DateTime.Now.AddDays(-1);
            //dateEnd.DateTime = DateTime.Now;
        }

        public DataSet LoadData()
        {

            args = new ParameterSpecManagementArgsPack();
            args.DBID = cmbDbName.EditValue.ToString();
            args.PARAMETERNAME = txtParam.Text;
            args.RULENAME = cmbRuleMain.Text;
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
            GridViewStyle();
        }

        private void GridViewStyle()
        {
            foreach (GridColumn item in gridView1.Columns)
            {
                if (item.FieldName == "ID"  || item.FieldName == "ROWID")
                    item.Visible = false;
            }
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
            if (string.IsNullOrEmpty(searchid.Text))
            {
                searchid.BackColor = Color.Orange;
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
            args.INSTANCE_NUMBER = cmbInstance.Text.ToString();
            args.PARAMETERID = searchid.EditValue.ToString();
            args.PARAMETERNAME = searchid.Text.ToString();
            args.RULENAME = cmbRuleName.Text.Split('(')[0];
            args.RULENO = (cmbRuleName.Text.Split('(')[1]).Substring(0, cmbRuleName.Text.Split('(')[1].Length - 1);
            args.DAYS = txtAddDays.Text;
            args.SPECUPPERLIMIT = txtSpecUpper.EditValue.ToString();
            args.SPECLOWERLIMIT = txtSpecLow.EditValue.ToString();
            args.CONTROLLOWERLIMIT = txtControlLow.EditValue.ToString();
            args.CONTROLUPPERLIMIT = txtControlUpper.EditValue.ToString();
            args.TARGET = sptarget.EditValue.ToString();
            args.STD_VALUE = spstdvalue.Text.ToString();
            args.PARAVAL1 = spparaval1.Text.ToString();
            args.PARAVAL2 = spparaval2.Text.ToString();
            args.PARAVAL3 = spparaval3.Text.ToString();
            args.PARAVAL4 = spparaval4.Text.ToString();
            args.PARAVAL5 = spparaval5.Text.ToString();
            args.ISALIVE = rdoIsalive.Properties.Items[rdoIsalive.SelectedIndex].Value.ToString();
            args.CHARTUSED = rdoIsalive.Properties.Items[rdoChartused.SelectedIndex].Value.ToString();
            args.MAILUSED = rdoIsalive.Properties.Items[rdoIlused.SelectedIndex].Value.ToString();
            args.MMSUSED = rdoIsalive.Properties.Items[rdoMMsused.SelectedIndex].Value.ToString();
            args.SPECLIMITUSED = rdoIsalive.Properties.Items[rdoSpec.SelectedIndex].Value.ToString();
            args.DETECTINGUSED = rdodetecting.Properties.Items[rdoSpec.SelectedIndex].Value.ToString();
            

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
                cmbInstance.Text = dr["INSTANCE_NUMBER"].ToString();
                //cmbAddDbName.EditValue = dr["DBID"];
                SelectedDBComboBox(cmbAddDbName, dr["DBID"].ToString());
                searchid.EditValue = dr["PARAMETERID"].ToString();
                //cmbRuleName.Text= dr["RULENAME"].ToString();
                //cmbRuleName.EditValue= dr["RULENO"].ToString();
                SelectedRuleNOComboBox(cmbRuleName, dr["RULENO"].ToString(), dr["RULENAME"].ToString());

                txtAddDays.Text = dr["DAYS"].ToString();
                txtSpecUpper.Text = dr["SPECUPPERLIMIT"].ToString();
                txtSpecLow.Text = dr["SPECLOWERLIMIT"].ToString();
                txtControlLow.Text = dr["CONTROLLOWERLIMIT"].ToString();
                txtControlUpper.Text = dr["CONTROLUPPERLIMIT"].ToString();
                spstdvalue.Text = dr["STD_VALUE"].ToString();
                spparaval1.Text = dr["PARAVAL1"].ToString();
                spparaval2.Text = dr["PARAVAL2"].ToString();
                spparaval3.Text = dr["PARAVAL3"].ToString();
                spparaval4.Text = dr["PARAVAL4"].ToString();
                spparaval5.Text = dr["PARAVAL5"].ToString();
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


        public void SelectedDBComboBox(TCheckComboBox ComboBox, string str)
        {
            
            
            foreach (CheckedListBoxItem item in ComboBox.Properties.Items)
            {

                if (item.Value.ToString().Contains(str))
                {
                    item.CheckState = CheckState.Checked;
                }
                else {
                    item.CheckState = CheckState.Unchecked;
                }
            }
        }


        public void SelectedRuleNOComboBox(TCheckComboBox ComboBox, string ruleno , string rulename)
        {


            foreach (CheckedListBoxItem item in ComboBox.Properties.Items)
            {

                if (item.Value.ToString().Contains(ruleno) && item.Value.ToString().Contains(rulename))
                {
                    item.CheckState = CheckState.Checked;
                }
                else
                {
                    item.CheckState = CheckState.Unchecked;
                }
            }
        }


    }
}
