using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using TAP;
using TAP.Models.Codes;
using TAP.UI;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;
using TAP.UIControls.BasicControlsDEV;
using ISIA.INTERFACE.ARGUMENTSPACK;
using TAP.Data.Client;
using ISIA.UI.BASE;

namespace ISIA.UI.ADMINISTRATOE
{
    public partial class FrmCodeManagement : DockUIBase1T1
    {
        public FrmCodeManagement()
        {
            InitializeComponent();
            InitializeComboBox();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.CodeManagement");
        }

        #region Feild
        List<int> focusedRowHandles = new List<int>();
        ComboBoxControl ComboBoxControl = new ComboBoxControl();

        ArgumentPack retVal = new ArgumentPack();
        CodeModelUnit codeModelUnit = new CodeModelUnit();
        PMArgsPack args = new PMArgsPack();
        BizDataClient bs = null;
        #endregion

        #region Method   
        public void DisplayData(DataSet ds)
        {
            if (ds == null)
            {
                return;
            }

            gridControl1.DataSource = null;
            gridView1.Columns.Clear();
            gridControl1.DataSource = ds.Tables[0];
            GridViewStyle();
        }

        public DataSet LoadModelData()
        {
            DataTable dt = new DataTable();
            string caterory = cboCategory.Text;
            string subCaterory = cboSubcategory.Text;
            args = new PMArgsPack();
            args.PMCategory = caterory;
            args.PMItem = subCaterory;
            DataSet ds = bs.ExecuteDataSet("GetSpec", args.getPack());
            //gridControl1.DataSource = ds.Tables[0]; 
            dt = ds.Tables[0];

            DataSet dataSet = new DataSet();
            DataTable dtNew = dt.Copy();
            dataSet.Tables.Add(dtNew);

            return dataSet;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                ComboBoxControl.SetCrossLang(this._translator);
                if (!base.ValidateUserInput(this.layoutControl1)) return;

                //LoadModelData();
                //focusedRowHandles.Clear();
                //GridViewStyle();
                base.BeginAsyncCall("LoadModelData", "DisplayData", EnumDataObject.DATASET);
            }
            catch (Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
            }
        }

        public void GridViewStyle()
        {
            gridView1.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            gridView1.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
            gridView1.OptionsBehavior.Editable = true;
            gridView1.OptionsView.ColumnAutoWidth = false;

            //gridView1.OptionsCustomization.AllowColumnMoving = false;
            //gridView1.OptionsCustomization.AllowSort = false;
            //gridView1.OptionsCustomization.AllowColumnResizing = true;

            gridView1.Columns["INSERTTIME"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["UPDATETIME"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["INSERTUSER"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["UPDATEUSER"].OptionsColumn.AllowEdit = false;
            gridView1.BestFitColumns();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ComboBoxControl.SetCrossLang(this._translator);
            if (!base.ValidateUserInput(this.layoutControl2)) return;

            DialogResult dialog = ComboBoxControl.IsInsert(this.Text);
            if (dialog.ToString() == "Yes")
            {
                try
                {
                    int SaveCount = 0;

                    //  codeModelUnit = new CodeModelUnit(this.txtCategory.Text,this.txtSubCategory.Text,this.txtName.Text , this.txtDescription.Text,EnumFlagYN.YES, InfoBase._USER_INFO.UserName, 0);
                    codeModelUnit = new CodeModelUnit(this.txtCategory.Text, this.txtSubCategory.Text, this.txtName.Text, this.txtDescription.Text, EnumFlagYN.YES, custom01.Text, custom02.Text, custom03.Text, custom04.Text, "", InfoBase._USER_INFO.UserName, 0);
                    SaveCount = codeModelUnit.Save(InfoBase._USER_INFO.UserName, "CREATE", TAP.Models.EnumEventFlag.D);

                    if (SaveCount > 0)
                    {
                        TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.INFORMATION, "Insert completed.");
                    }

                    InitializeComboBox();
                    //DataTable dt = codeModelUnit.LoadModelDataList(retVal);
                    //gridControl1.DataSource = dt;  
                    gridControl1.DataSource = null;
                }
                catch (System.Exception ex)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
                }
            }
            else
            {
                return;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gridView1.SelectedRowsCount <= 0)
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Please choose one line");
                return;
            }
            ComboBoxControl.SetCrossLang(this._translator);
            if (!base.ValidateUserInput(this.layoutControl1)) return;

            DialogResult dialog = ComboBoxControl.IsDelete(this.Text);
            if (dialog.ToString() == "Yes")
            {
                List<int> selectRowNum = new List<int>();
                try
                {
                    if (gridView1.SelectedRowsCount <= 0)
                        return;
                    foreach (int rowhandel in gridView1.GetSelectedRows())
                    {
                        selectRowNum.Add(rowhandel);
                    }

                    for (int i = 0; i < selectRowNum.Count; i++)
                    {
                        DataRow tmpRow = gridView1.GetDataRow(selectRowNum[i]);
                        string name = tmpRow["NAME"].ToString();
                        string category = tmpRow["CATEGORY"].ToString();
                        string subCategory = tmpRow["SUBCATEGORY"].ToString();
                        string updateUser = tmpRow["UPDATEUSER"].ToString();
                        string description = tmpRow["DESCRIPTION"].ToString();
                        int sequences = Int32.Parse(tmpRow["SEQUENCES"].ToString());
                        string custom1 = tmpRow["custom01"].ToString();
                        string custom2 = tmpRow["custom02"].ToString();
                        string custom3 = tmpRow["custom03"].ToString();
                        string custom4 = tmpRow["custom04"].ToString();

                        //          codeModelUnit = new CodeModelUnit(category, subCategory, name, description, EnumFlagYN.NO, updateUser, sequences);
                                 codeModelUnit = new CodeModelUnit(category, subCategory, name, description, EnumFlagYN.NO, custom1, custom2, custom3, custom4, "", updateUser, sequences);
                        codeModelUnit.LastEvent = "DELETE";
                        codeModelUnit.LastEventCode = "DELETE_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        codeModelUnit.LastEventTime = DateTime.ParseExact(DateTime.Now.ToString("yyyyMMddHHmmss"), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        codeModelUnit.LastJobCode = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        codeModelUnit.Delete(InfoBase._USER_INFO.UserName);
                        //operCodeModel.CodeList[name].Delete(InfoBase._USER_INFO.UserName);
                    }
                    //LoadModelData();    
                    base.BeginAsyncCall("LoadModelData", "DisplayData", EnumDataObject.DATASET);
                }
                catch (System.Exception ex)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
                }
            }
            else
            {
                //LoadModelData();
                base.BeginAsyncCall("LoadModelData", "DisplayData", EnumDataObject.DATASET);
                return;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ComboBoxControl.SetCrossLang(this._translator);
            if (gridView1.DataSource == null || gridView1.DataRowCount == 0) return;

            DialogResult dialog = ComboBoxControl.IsUpdate(this.Text);
            if (dialog.ToString() == "Yes")
            {
                if (focusedRowHandles.Count <= 0)
                    return;
                try
                {
                    for (int i = 0; i < focusedRowHandles.Count; i++)
                    {
                        DataRow tmpRow = gridView1.GetDataRow(focusedRowHandles[i]);

                        string CATEGORY = tmpRow["CATEGORY"].ToString();
                        string SUBCATEGORY = tmpRow["SUBCATEGORY"].ToString();
                        string NAME = tmpRow["NAME"].ToString();
                        string DESCRIPTION = tmpRow["DESCRIPTION"].ToString();
                        string UPDATEUSER = tmpRow["UPDATEUSER"].ToString();
                        int SEQUENCES = Int32.Parse(tmpRow["SEQUENCES"].ToString());
                        string custom1 = tmpRow["custom01"].ToString();
                        string custom2 = tmpRow["custom02"].ToString();
                        string custom3 = tmpRow["custom03"].ToString();
                        string custom4 = tmpRow["custom04"].ToString();
                        //codeModelUnit = new CodeModelUnit(CATEGORY, SUBCATEGORY, NAME, DESCRIPTION, EnumFlagYN.YES, UPDATEUSER, SEQUENCES);
                        codeModelUnit = new CodeModelUnit(CATEGORY, SUBCATEGORY, NAME, DESCRIPTION, EnumFlagYN.YES, custom1, custom2, custom3, custom4, "", UPDATEUSER, SEQUENCES);
                        codeModelUnit.LastEvent = "MODIFY";
                        codeModelUnit.LastEventCode = "MODIFY_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        codeModelUnit.LastEventTime = DateTime.ParseExact(DateTime.Now.ToString("yyyyMMddHHmmss"), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        codeModelUnit.LastJobCode = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        codeModelUnit.Save(InfoBase._USER_INFO.UserName);
                    }
                    focusedRowHandles.Clear();
                    //LoadModelData();
                    base.BeginAsyncCall("LoadModelData", "DisplayData", EnumDataObject.DATASET);
                }
                catch (System.Exception ex)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
                }
            }
            else
            {
                //LoadModelData();
                base.BeginAsyncCall("LoadModelData", "DisplayData", EnumDataObject.DATASET);
                return;
            }
        }
        #endregion

        #region InitializeComboBox       
        private void InitializeComboBox()
        {
            try
            {
                cboCategory.changeUpdateStatus(true);
                cboSubcategory.changeUpdateStatus(true);
                cboCategory.Text = "";
                cboSubcategory.Text = "";
                txtDescription.Text = "";
                txtCategory.Text = "";
                txtSubCategory.Text = "";
                txtName.Text = "";
                custom01.Text = "";
                custom02.Text = "";
                custom03.Text = "";
                custom04.Text = "";
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            focusedRowHandles.Add(this.gridView1.FocusedRowHandle);
            this.gridView1.RowCellStyle += gridView1_RowCellStyle;
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (focusedRowHandles.Count > 0)
            {
                for (int i = 0; i < focusedRowHandles.Count; i++)
                {
                    if (e.RowHandle == focusedRowHandles[i])
                    {
                        e.Appearance.BackColor = Color.SeaShell;
                    }
                }
            }
        }

    }
}