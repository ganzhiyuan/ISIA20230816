using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.UI;
using TAP.UIControls.BasicControlsDEV;
using ISIA.UI.BASE;

namespace ISIA.UI.ADMINISTRATOE
{
    public partial class FrmClassicUI : DockUIBase1T1
    {
        public FrmClassicUI()
        {
            InitializeComponent();
            InitializeComTextBox();
            // this.tpanelDetails.Visible = false;
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.ClassicUI");

        }

        #region Feild
        List<int> focusedRowHandles = new List<int>();

        BizDataClient bs = null;
        ClassicUIArgsPack args = new ClassicUIArgsPack();
        DataSet ds = new DataSet();
        DataTable dataTable;

        ComboBoxControl ComboBoxControl = new ComboBoxControl();
        #endregion

        #region method
        public void InitializeComTextBox()
        {
            //txtAddMDI.ReadOnly = true;
            //txtAddRegion.ReadOnly = true;
            //txtAddFacility.ReadOnly = true;

            txtAddMDI.Text = "ISIA";
            //txtName.Text = "";
            txtAddRegion.Text = "CELL";
            txtAddFacility.Text = "ALL";

        }
        public DataSet LoadData()
        {
            args.Facility = cboFacility.Text;
            args.MDI = cboMDI.Text;
            args.MainMenu = cboMainMenu.Text;

            try
            {
                ds = bs.ExecuteDataSet("GetUI", args.getPack());

                return ds;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
        public void DisplayData(DataSet ds)
        {
            if (ds == null)
            {
                return;
            }

            dataTable = ds.Tables[0];
            ds.Tables[0].Columns["FACILITY"].ColumnName = "BUILDING";
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();
            gridControl1.DataSource = ds.Tables[0];

            GridViewStyle();
        }

        private void Save()
        {

            ComboBoxControl.SetCrossLang(this._translator);
            if (!base.ValidateUserInput(this.layoutControl3)) return;

            if (string.IsNullOrEmpty(txtAddRegion.Text))
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "You must input Region.");
                return;
            }
            if (string.IsNullOrEmpty(txtAddMDI.Text))
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "You must input MDI.");
                return;
            }
            if (string.IsNullOrEmpty(txtAddFacility.Text))
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "You must input Facility.");
                return;
            }
            if (string.IsNullOrEmpty(txtAddName.Text))
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "You must input Name.");
                return;
            }
            if (string.IsNullOrEmpty(txtAddDispalyName.Text))
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "You must input DisoalyName.");
                return;
            }

            DialogResult dialog = ComboBoxControl.IsInsert(this.Text);
            if (dialog.ToString() == "Yes")
            {
                string facility = txtAddFacility.Text;
                string MDI = txtAddMDI.Text;
                string mainMenu = txtAddMainMenu.Text;
                string subMenu = txtAddSubMenu.Text;
                string name = txtAddName.Text;
                string region = txtAddRegion.Text;
                string uiLayout = txtAddUILayout.Text;
                string displayName = txtAddDispalyName.Text;
                string assmblyName = txtAddAssemblyName.Text;
                string assmlyFileName = txtAddAssFileName.Text;
                //string lastEventTime = txtAddLastEventTime.Text;
                string imageName = txtAddImageName.Text;
                string smallImageName = txtAddSmallImageName.Text;
                string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                args.Facility = facility;
                args.MDI = MDI;
                args.MainMenu = mainMenu;
                args.SubMenu = subMenu;
                args.Name = name;
                args.Region = region;
                args.UILayout = uiLayout;
                args.DisplayName = displayName;
                args.AssemblyName = assmblyName;
                args.AssemblyFileName = assmlyFileName;
                //args.LastEventTime = lastEventTime;
                args.ImageName = imageName;
                args.SmallImageName = smallImageName;
                args.InsertTime = nowTime;
                args.InsertUser = InfoBase._USER_INFO.UserName;
                args.ISALIVE = "YES";

                try
                {
                    bs.ExecuteModify("AddUI", args.getPack());

                    bs.ExecuteModify("AddTapstbuiAuthorty", args.getPack());

                    InitializePanlComTextBox(layoutControl3);

                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.INFORMATION, "Insert completed.");
                    base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
                }
                catch
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "Insert failed.");
                }
            }
            else
            {
                return;
            }

        }
        private DateTime StringToDateTime(string strTime, DateTime dateTime)
        {
            DateTime.TryParse(
                        strTime.Substring(0, 4) + "-" +
                        strTime.Substring(4, 2) + "-" +
                        strTime.Substring(6, 2) + " " +
                        strTime.Substring(8, 2) + ":" +
                        strTime.Substring(10, 2) + ":" +
                        strTime.Substring(12, 2), out dateTime);

            return dateTime;
        }
        private  bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }

        public void GridViewStyle()
        {
            gridView1.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            gridView1.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;

            gridView1.OptionsCustomization.AllowColumnMoving = false;
            gridView1.OptionsCustomization.AllowSort = false;
            //gridView1.OptionsCustomization.AllowColumnResizing = true;


            gridView1.Columns["CURRENTMODEL"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["BUILDING"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["INSERTTIME"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["INSERTUSER"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["ISALIVE"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTEVENT"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTEVENTCODE"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTEVENTFLAG"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTEVENTTIME"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTJOBCODE"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["MAINMENU"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["MDI"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["MODELLEVELS"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["NAME"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["PANELNAME"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["REGION"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["SEQUENCES"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["SHORTCUTCHARACTER"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["SHORTCUTDISPLAYSTRING"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["SHORTCUTKEY"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["SHORTCUTKEYS"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["UPDATETIME"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["UPDATEUSER"].OptionsColumn.AllowEdit = false;

            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.BestFitColumns();

            gridView1.OptionsBehavior.Editable = true;

            gridView1.Columns["ISALIVE"].Visible = false;
        }
        #endregion

        #region event

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
            Save();
    
           
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

                        string FACILITY = tmpRow["BUILDING"].ToString();
                        string MDI = tmpRow["MDI"].ToString();
                        string NAME = tmpRow["NAME"].ToString();
                        string REGION = tmpRow["REGION"].ToString();
                        string MAINMENU = tmpRow["MAINMENU"].ToString();


                        args.Facility = FACILITY;
                        args.MDI = MDI;
                        args.Name = NAME;
                        args.Region = REGION;
                        args.MainMenu = MAINMENU;

                        DeleteCount = bs.ExecuteModify("DeleteBUI", args.getPack());
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
               // LoadData();
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
                        args.Name = tmpRow["NAME"].ToString();
                        args.Region = tmpRow["REGION"].ToString();
                        args.Facility = tmpRow["BUILDING"].ToString();
                        args.MainMenu = tmpRow["MAINMENU"].ToString();
                        args.SubMenu = tmpRow["SUBMENU"].ToString();


                        args.UILayout = tmpRow["UILayout"].ToString();
                        args.UIType = tmpRow["UIType"].ToString();
                        args.DisplayName = tmpRow["DisplayName"].ToString();
                        args.AssemblyFileName = tmpRow["AssemblyFileName"].ToString();
                        args.AssemblyName = tmpRow["AssemblyName"].ToString();
                        args.SubType = tmpRow["SubType"].ToString();
                        args.ImageName = tmpRow["ImageName"].ToString();
                        args.SmallImageName = tmpRow["SmallImageName"].ToString();
                        if (IsInt(tmpRow["Icon"].ToString()) && tmpRow["Icon"].ToString() != "")
                            args.Icon = int.Parse(tmpRow["Icon"].ToString());
                        else
                        {
                            TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "Icon must be a number.");
                            return;
                        }

                   
                        args.Description = tmpRow["Description"].ToString();
                        args.LastEventComment = tmpRow["LastEventComment"].ToString();
                        args.UpdateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        args.UpdateUser = InfoBase._USER_INFO.UserName;

                        bs.ExecuteModify("UpdateUI", args.getPack());

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

        public void InitializePanlComTextBox(Control control)
        {
            Control.ControlCollection controlCollection = control.Controls;

            foreach (Control item in controlCollection)
            {
                if (item is TAP.UIControls.BasicControlsDEV.TTextBox)
                {
                    if (item.Name.ToString() != "txtAddMDI" && item.Name.ToString() != "txtAddRegion" && item.Name.ToString() != "txtAddFacility")
                    {

                        item.Text = "";
                    }


                }

            }

        }

 
        #endregion


    }
}
