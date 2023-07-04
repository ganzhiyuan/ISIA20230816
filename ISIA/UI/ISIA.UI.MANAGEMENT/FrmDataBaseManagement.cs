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
        CodeManagementArgsPack args =null;

        DataSet ds = new DataSet();

        #endregion
        public FrmDataBaseManagement()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.CodeManagement");
            //dateStart.DateTime = DateTime.Now.AddDays(-1);
            //dateEnd.DateTime = DateTime.Now;
            //dtStart.DateTime = DateTime.Now.AddDays(-1);
            //dtEnd.DateTime = DateTime.Now;
        }

        public DataSet LoadData()
        {

            args = new CodeManagementArgsPack();
            args.DBID = cmbDbName.EditValue.ToString();
            args.DBNAME = cmbDbName.Text.Split('(')[0].ToString();
            args.DBLINKNAME = txtMainDBLINKNAME.Text;
            args.SERVICENAME = txtMainSERVICENAME.Text;
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
            if (string.IsNullOrEmpty(txtDBID.Text)|| txtDBID.Text=="0")
            {
                txtDBID.BackColor = Color.Orange;
                return;
            }
            if (string.IsNullOrEmpty(txtDBNAME.Text))
            {
                txtDBNAME.BackColor = Color.Orange;
                return;
            }
            if (string.IsNullOrEmpty(txtDBLINKNAME.Text))
            {
                txtDBLINKNAME.BackColor = Color.Orange;
                return;
            }
            if (string.IsNullOrEmpty(txtSERVICENAME.Text))
            {
                txtSERVICENAME.BackColor = Color.Orange;
                return;
            }
            if (string.IsNullOrEmpty(txtIPADDRESS.Text))
            {
                txtIPADDRESS.BackColor = Color.Orange;
                return;
            }
            args = new CodeManagementArgsPack();
            args.DBID = txtDBID.Text;
            args.SERVICENAME = txtSERVICENAME.Text;
            args.DESCRIPTION = txtDESCRIPTION.Text;
            args.DBLINKNAME = txtDBLINKNAME.Text;
            args.IPADDRESS = txtIPADDRESS.Text;
            args.DBNAME = txtDBNAME.Text;
            args.SERVICENAME = txtSERVICENAME.Text;
            args.INSTANTCNT = Convert.ToDecimal(txtINSTANTCNT.EditValue);
            args.SEQUENCES = Convert.ToDecimal(txtSEQUENCES.Text);
            args.ISALIVE= rdoIsalive.Properties.Items[rdoIsalive.SelectedIndex].Value.ToString();
            DataSet dst = bs.ExecuteDataSet("CheckTcode", args.getPack());
            if (dst==null||dst.Tables==null||dst.Tables[0].Rows.Count==0)
            {
                args.INSERTTIME = DateTime.Now.ToString("yyyyMMddHHmmss");
                args.INSERTUSER= TAP.UI.InfoBase._USER_INFO.Name; 
                int i = bs.ExecuteModify("SaveTCode", args.getPack());
            }
            else
            {
                args.UPDATETIME = DateTime.Now.ToString("yyyyMMddHHmmss");
                args.UPDATEUSER= TAP.UI.InfoBase._USER_INFO.Name; 
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
                args.DBID = txtDBID.Text;
                args.SERVICENAME = txtSERVICENAME.Text;
                args.DESCRIPTION = txtDESCRIPTION.Text;
                args.DBLINKNAME = txtDBLINKNAME.Text;
                args.IPADDRESS = txtIPADDRESS.Text;
                args.DBNAME = txtDBNAME.Text;
                args.SERVICENAME = txtSERVICENAME.Text;
                args.INSTANTCNT = Convert.ToDecimal(txtINSTANTCNT.EditValue);
                args.SEQUENCES = Convert.ToDecimal(txtSEQUENCES.Text);
                args.ISALIVE = rdoIsalive.Properties.Items[rdoIsalive.SelectedIndex].Value.ToString();

                tabPane1.SelectedPage = tabNavigationPage2;
                txtDBID.Text = dr["DBID"].ToString();
                txtSERVICENAME.Text = dr["SERVICENAME"].ToString();
                txtDESCRIPTION.Text= dr["DESCRIPTION"].ToString();
                txtDBLINKNAME.Text= dr["DBLINKNAME"].ToString();
                txtIPADDRESS.Text = dr["IPADDRESS"].ToString();
                txtDBNAME.Text = dr["DBNAME"].ToString();
                txtSERVICENAME.Text = dr["SERVICENAME"].ToString();
                txtINSTANTCNT.Text = dr["INSTANTCNT"].ToString();
                txtSEQUENCES.Text = dr["SEQUENCES"].ToString();

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
                        args = new CodeManagementArgsPack();
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

        private void tbadd_Click(object sender, EventArgs e)
        {
            FrmAddDataBase frmAddData = new FrmAddDataBase();
            frmAddData.ShowDialog();

        }
    }
}
