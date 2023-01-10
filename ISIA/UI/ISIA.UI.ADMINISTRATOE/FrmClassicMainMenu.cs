using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using TAP;
using TAP.UI;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using TAP.Models.Factories.Facilities;
using TAP.UIControls.BasicControlsDEV;
using TAP.Data.Client;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System.Linq;
using ISIA.UI.BASE;

namespace ISIA.UI.ADMINISTRATOE
{
    public partial class FrmClassicMainMenu : DockUIBase1T1
    {
        public FrmClassicMainMenu()
        {
            InitializeComponent();
            InitializeComTextBox();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.MainMenu");
        }

        #region Feild
        List<int> focusedRowHandles = new List<int>();
        BizDataClient bs = null;
        MainMenuArgsPack args = new MainMenuArgsPack();
        DataSet ds = new DataSet();

        ComboBoxControl ComboBoxControl = new ComboBoxControl();
        #endregion

        #region Method

        public void InitializeComTextBox() {
            txtMDI.Text = "ISIA";
            //txtName.Text = "";
            txtSite.Text = "CELL";
            txtFacility.Text = "ALL";

        }
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

        public DataSet LoadData()
        {
            args.FACILITY = cboFacitily.Text;
            args.MDI = cboMdi.Text;

            try
            {
                ds = bs.ExecuteDataSet("GetMainMenu", args.getPack());

                return ds;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
      
        public void GridViewStyle()
        {
            
            //添加单选框
            gridView1.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            gridView1.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
            //设置单元格可以编辑
            gridView1.OptionsBehavior.Editable = true;
            //指定列不可编辑
            gridView1.Columns["FACILITY"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["INSERTTIME"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["INSERTUSER"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["ISALIVE"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTEVENT"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTEVENTCODE"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTEVENTCOMMENT"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTEVENTFLAG"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTEVENTTIME"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTJOBCODE"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["MDI"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["MODELLEVELS"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["NAME"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["REGION"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["SEQUENCES"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["UPDATETIME"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["UPDATEUSER"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["SHORTCUTKEY"].OptionsColumn.AllowEdit = false;

            gridView1.Columns["ISALIVE"].Visible = false;
      
           
            //修改列名
            gridView1.Columns["FACILITY"].Caption = "BUILDING";
            gridView1.Columns["REGION"].Caption = "SITE";
            //宽度自适应大小
            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.BestFitColumns();
        }

        public void InitializePanlComTextBox(Control control)
        {
        
            nbxIcon.Text = 0.ToString();
            Control.ControlCollection controlCollection = control.Controls;

            foreach (Control item in controlCollection)
            {
                if (item is TAP.UIControls.BasicControlsDEV.TTextBox)
                {
                    if (item.Name.ToString() != "txtSite" && item.Name.ToString() != "txtMDI" && item.Name.ToString() != "txtFacility")
                    {
                      
                        item.Text = "";
                    }
                    

                }

            }

        }
        #endregion

        #region Event
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                ComboBoxControl.SetCrossLang(this._translator);
                if (!base.ValidateUserInput(this.layoutControl1)) return;

                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ComboBoxControl.SetCrossLang(this._translator);
            if (!base.ValidateUserInput(this.layoutControl3)) return;


            if (string.IsNullOrEmpty(txtSite.Text))
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "You must input Region.");
                return;
            }
            if (string.IsNullOrEmpty(txtMDI.Text))
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "You must input MDI.");
                return;
            }
            if (string.IsNullOrEmpty(txtFacility.Text))
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "You must input Facility.");
                return;
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "You must input Name.");
                return;
            }
            if (string.IsNullOrEmpty(txtDisoalyName.Text))
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "You must input DisoalyName.");
                return;
            }

            DialogResult dialog = ComboBoxControl.IsInsert(this.Text);
            if (dialog.ToString() == "Yes")
            {
                try
                {
                    int SaveCount = 0;

                    args.MDI = txtMDI.Text;
                    args.NAME = txtName.Text;
                    args.REGION = txtSite.Text;
                    args.FACILITY = txtFacility.Text;
                    args.DISPLAYNAME = txtDisoalyName.Text;
                    args.SHORTCUTKEY = "NONE";
                    args.SHORTCUTDISPLAYSTRING = txtSHORTCUTDISPLAYSTRING.Text;
                    args.SHORTCUTCHARACTER = txtSHORTCUTCHARACTER.Text;
                    args.SHORTCUTKEYS = txtSHORTCUTKEYS.Text;
                    args.ICON = nbxIcon.Text;
                    //args.CURRENTMODEL = txtCurrentModel.Text;
                    args.DESCRIPTION = txtDescription.Text;

                    args.INSERTTIME = DateTime.Now.ToString("yyyyMMddHHmmss");
                    args.INSERTUSER = InfoBase._USER_INFO.UserName;
                    args.ISALIVE = "YES";
                

                    SaveCount = bs.ExecuteModify("SaveMainMenu", args.getPack());
                    InitializePanlComTextBox(layoutControl3);
                    if (SaveCount > 0)
                    {
                        TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.INFORMATION, "Insert completed.");
                    }

                    base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            ComboBoxControl.SetCrossLang(this._translator);
            DialogResult dialog = ComboBoxControl.IsUpdate(this.Text);
            if (dialog.ToString() == "Yes")
            {
                
                if (focusedRowHandles.Count <= 0)
                    return;
                
                try
                {
                    for (int i = 0; i < focusedRowHandles.Distinct().ToList().Count; i++)                    
                    {
                        DataRow tmpRow = gridView1.GetDataRow(focusedRowHandles.Distinct().ToList()[i]);
                        args.MDI = tmpRow["MDI"].ToString();
                        args.NAME = tmpRow["NAME"].ToString();
                        args.REGION = tmpRow["REGION"].ToString();
                        args.FACILITY = tmpRow["FACILITY"].ToString();

                        args.DISPLAYNAME = tmpRow["DISPLAYNAME"].ToString();
                        //args.SHORTCUTKEY = tmpRow["SHORTCUTKEY"].ToString();
                        args.SHORTCUTCHARACTER = tmpRow["SHORTCUTCHARACTER"].ToString();
                        args.SHORTCUTDISPLAYSTRING = tmpRow["SHORTCUTDISPLAYSTRING"].ToString();
                        args.SHORTCUTKEYS = tmpRow["SHORTCUTKEYS"].ToString();
                        args.ICON = tmpRow["ICON"].ToString();
                        args.CURRENTMODEL = tmpRow["CURRENTMODEL"].ToString();
                        args.DESCRIPTION = tmpRow["DESCRIPTION"].ToString();

                        args.UPDATETIME1 = DateTime.Now.ToString("yyyyMMddHHmmss");
                        args.UPDATEUSER = InfoBase._USER_INFO.UserName;

                        bs.ExecuteModify("UpdateMainMenu", args.getPack());

                    }
                    focusedRowHandles.Clear();
                    base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
                }
                catch (System.Exception ex)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
                }
            }
            else
            {
               // base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
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
                    int DeleteCount = 0;
                    if (gridView1.SelectedRowsCount <= 0)
                        return;
                    foreach (int rowhandel in gridView1.GetSelectedRows())
                    {
                        selectRowNum.Add(rowhandel);
                    }

                    for (int i = 0; i < selectRowNum.Count; i++)
                    {
                        DataRow tmpRow = gridView1.GetDataRow(selectRowNum[i]);

                        string FACILITY = tmpRow["FACILITY"].ToString();
                        string MDI = tmpRow["MDI"].ToString();
                        string NAME = tmpRow["NAME"].ToString();
                        string REGION = tmpRow["REGION"].ToString();

                        args.FACILITY = FACILITY;
                        args.MDI = MDI;
                        args.NAME = NAME;
                        args.REGION = REGION;

                        DeleteCount = bs.ExecuteModify("DeleteMainMenu", args.getPack());
                    }
                    if (DeleteCount > 0)
                    {
                        TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Delete completed.");
                    }

                    base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);

                }
                catch (System.Exception ex)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
                }
            }
            else
            {
               // base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
                return;
            }
        }
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


        #endregion

    }
}
