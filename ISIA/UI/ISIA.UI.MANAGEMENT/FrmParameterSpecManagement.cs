﻿using DevExpress.Utils.Win;
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
        ParameterSpecManagementArgsPack args = null;

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
            //控件值为空
            txtSpecUpper.EditValue = null;
            txtSpecLow.EditValue = null;
            txtControlLow.EditValue = null;
            txtControlUpper.EditValue = null;
            spstdvalue.EditValue = null;
            sptarget.EditValue = null;

            spparaval1.EditValue = null;
            spparaval2.EditValue = null;
            spparaval3.EditValue = null;
            spparaval4.EditValue = null;
            spparaval5.EditValue = null;

            //dateStart.DateTime = DateTime.Now.AddDays(-1);
            //dateEnd.DateTime = DateTime.Now;
        }

        public DataSet LoadData()
        {

            args = new ParameterSpecManagementArgsPack();
            args.DBID = cmbDbName.EditValue.ToString();
            args.PARAMETERNAME = txtParam.Text;

            if (string.IsNullOrEmpty(cmbRuleMain.Text))
            {

            }
            else
            {
                args.RULENAME = cmbRuleMain.Text.Split('(')[0];
                args.RULENO = (cmbRuleMain.Text.Split('(')[1]).Substring(0, cmbRuleMain.Text.Split('(')[1].Length - 1);
            }

            
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
                if (item.FieldName == "ID" || item.FieldName == "ROWID")
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
                if (!base.ValidateUserInput(this.layoutControl3)) return;
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
            if (searchid.EditValue == null)
            {
                searchid.BackColor = Color.Orange;
                return;
            }

            if (string.IsNullOrEmpty(txtAddDays.Text))
            {
                txtAddDays.BackColor = Color.Orange;

                return;
            }
            
            /*if (string.IsNullOrEmpty(txtControlLow.Text))
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
            }*/
            args = new ParameterSpecManagementArgsPack();
            args.DBID = cmbAddDbName.EditValue.ToString();
            args.INSTANCE_NUMBER = cmbInstance.Text.ToString();
            args.PARAMETERID = searchid.EditValue.ToString();
            args.PARAMETERNAME = searchid.Text.ToString();
            args.RULENAME = cmbRuleName.Text.Split('(')[0];
            args.RULENO = (cmbRuleName.Text.Split('(')[1]).Substring(0, cmbRuleName.Text.Split('(')[1].Length - 1);
            args.DAYS = txtAddDays.Text;
            args.SPECUPPERLIMIT = txtSpecUpper.Text.ToString();
            args.SPECLOWERLIMIT = txtSpecLow.Text.ToString();
            args.CONTROLLOWERLIMIT = txtControlLow.Text.ToString();
            args.CONTROLUPPERLIMIT = txtControlUpper.Text.ToString();
            args.TARGET = sptarget.Text.ToString();
            args.STD_VALUE = spstdvalue.Text.ToString();
            args.PARAVAL1 = spparaval1.Text.ToString();
            args.PARAVAL2 = spparaval2.Text.ToString();
            args.PARAVAL3 = spparaval3.Text.ToString();
            args.PARAVAL4 = spparaval4.Text.ToString();
            args.PARAVAL5 = spparaval5.Text.ToString();
            args.ISALIVE = rdoIsalive.Properties.Items[rdoIsalive.SelectedIndex].Value.ToString();
            args.CHARTUSED = rdoChartused.Properties.Items[rdoChartused.SelectedIndex].Value.ToString();
            args.MAILUSED = rdoIlused.Properties.Items[rdoIlused.SelectedIndex].Value.ToString();
            //args.MMSUSED = rdoIsalive.Properties.Items[rdoMMsused.SelectedIndex].Value.ToString();
            //args.SPECLIMITUSED = rdoIsalive.Properties.Items[rdoSpec.SelectedIndex].Value.ToString();
            args.DETECTINGUSED = rdodetecting.Properties.Items[rdodetecting.SelectedIndex].Value.ToString();


            DataSet dst = bs.ExecuteDataSet("CheckTcode", args.getPack());
            if (dst == null || dst.Tables == null || dst.Tables[0].Rows.Count == 0)
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
            if (dr != null)
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
                txtSpecUpper.EditValue = dr["SPECUPPERLIMIT"];
                txtSpecLow.EditValue = dr["SPECLOWERLIMIT"];
                txtControlLow.EditValue = dr["CONTROLLOWERLIMIT"];
                txtControlUpper.EditValue = dr["CONTROLUPPERLIMIT"];
                spstdvalue.EditValue = dr["STD_VALUE"];
                sptarget.EditValue = dr["TARGET"];

                spparaval1.EditValue = dr["PARAVAL1"];
                spparaval2.EditValue = dr["PARAVAL2"];
                spparaval3.EditValue = dr["PARAVAL3"];
                spparaval4.EditValue = dr["PARAVAL4"];
                spparaval5.EditValue = dr["PARAVAL5"];
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
                if (dr["DETECTINGUSED"].ToString() == "YES")
                {
                    rdodetecting.SelectedIndex = 0;
                }
                else
                {
                    rdodetecting.SelectedIndex = 1;
                }

                /*if (dr["MMSUSED"].ToString() == "YES")
                {
                    rdoMMsused.SelectedIndex = 0;
                }
                else
                {
                    rdoMMsused.SelectedIndex = 1;
                }*/
                /*if (dr["SPECLIMITUSED"].ToString() == "YES")
                {
                    rdoSpec.SelectedIndex = 0;
                }
                else
                {
                    rdoSpec.SelectedIndex = 1;
                }*/


            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            int[] i = gridView1.GetSelectedRows();
            if (i == null && i.Count() < 1)
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
                else
                {
                    item.CheckState = CheckState.Unchecked;
                }
            }
        }


        public void SelectedRuleNOComboBox(TCheckComboBox ComboBox, string ruleno, string rulename)
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

        private void txtControlUpper_DoubleClick(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(cmbAddDbName.Text))
            {
                cmbAddDbName.BackColor = Color.Orange;
                return;
            }
            if (string.IsNullOrEmpty(cmbRuleName.Text))
            {
                cmbRuleName.BackColor = Color.Orange;
                return;
            }
            if (searchid.EditValue == null)
            {
                searchid.BackColor = Color.Orange;
                return;
            }
            

            Spec spec = new Spec();
            spec.DBID = cmbAddDbName.EditValue.ToString();
            spec.DBNAME = cmbAddDbName.Text.Split('(')[0];
            spec.INSTANCE_NUMBER = cmbInstance.Text.ToString();
            spec.PARAMETERID = searchid.EditValue.ToString();
            spec.PARAMETERNAME = searchid.Text.ToString();
            spec.RULENAME = cmbRuleName.Text.Split('(')[0];
            spec.RULENO = (cmbRuleName.Text.Split('(')[1]).Substring(0, cmbRuleName.Text.Split('(')[1].Length - 1);
            spec.CONTROLLOWERLIMIT = txtControlLow.Text.ToString();
            spec.CONTROLUPPERLIMIT = txtControlUpper.Text.ToString();
            spec.TARGET = sptarget.Text.ToString();
            spec.STD_VALUE = spstdvalue.Text.ToString();
            FrmParameterspecdata frm = new FrmParameterspecdata(spec);
            frm.MYDELE += new SPECdele(setFormTextValue);
            frm.ShowDialog();


        }


        public void setFormTextValue(Spec spec)
        {
            txtControlLow.EditValue = string.IsNullOrEmpty(spec.CONTROLUPPERLIMIT) == true ? null : spec.CONTROLUPPERLIMIT;
            txtControlUpper.EditValue = string.IsNullOrEmpty(spec.CONTROLLOWERLIMIT) == true ? null : spec.CONTROLLOWERLIMIT;
            spstdvalue.EditValue = string.IsNullOrEmpty(spec.STD_VALUE) == true ? null : spec.STD_VALUE;
            sptarget.EditValue = string.IsNullOrEmpty(spec.TARGET) == true ? null : spec.TARGET;

        }

    }

    public class Spec {

        public string DBNAME { get; set; } = string.Empty;
        public string DBID { get; set; } = string.Empty;
        public string INSTANCE_NUMBER { get; set; } = string.Empty;
        public string PARAMETERID { get; set; } = string.Empty;
        public string PARAMETERNAME { get; set; } = string.Empty;
        public string PARAMETERTYPE { get; set; } = string.Empty;
        public string RULENAME { get; set; } = string.Empty;
        public string RULENO { get; set; } = string.Empty;
        public string CONTROLLOWERLIMIT { get; set; } = string.Empty;
        public string CONTROLUPPERLIMIT { get; set; } = string.Empty;
        public string TARGET { get; set; } = string.Empty;
        public string STD_VALUE { get; set; } = string.Empty;





    }
}
