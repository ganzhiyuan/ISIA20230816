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
    public partial class FrmMessageGroupManagement : DockUIBase1T1
    {
        public FrmMessageGroupManagement()
        {
            InitializeComponent();
            InitializeComboBox();
            bs = new BizDataClient("ISIA.BIZ.ADMINISTRATOE.DLL", "ISIA.BIZ.ADMINISTRATOE.MessageGroupManagement");
        }

        #region Feild
        List<int> focusedRowHandles = new List<int>();
        ComboBoxControl ComboBoxControl = new ComboBoxControl();

        ArgumentPack retVal = new ArgumentPack();
        CodeModelUnit codeModelUnit = new CodeModelUnit();
        CommonArgsPack args = new CommonArgsPack();
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
            string messageType = cboMessageType.Text;
            args = new CommonArgsPack();
            args.MessageType = messageType;
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

            gridView1.Columns["MESSAGETYPE"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["MESSAGENAME"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["UPDATETIME"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["UPDATEUSER"].OptionsColumn.AllowEdit = false;
            gridView1.BestFitColumns();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ComboBoxControl.SetCrossLang(this._translator);
                if (!base.ValidateUserInput(this.layoutControl2)) return;
                if (string.IsNullOrEmpty(txtMessageType.Text) || string.IsNullOrEmpty(txtMessageName.Text))
                {
                    TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "MessageName or MessageType Cannot be empty..");
                    return;
                }
                DialogResult dialog = ComboBoxControl.IsInsert(this.Text);
                if (dialog.ToString() == "Yes")
                {
                    string messageType = txtMessageType.Text;
                    string messageName = txtMessageName.Text;
                    string custom1 = custom01.Text;
                    string custom2 = custom02.Text;
                    string custom3 = custom03.Text;
                    string custom4 = custom04.Text;
                    string custom5 = custom05.Text;
                    args = new CommonArgsPack();
                    args.MessageType = messageType;
                    args.MessageName = messageName;
                    args.Custom01 = custom1;
                    args.Custom02 = custom2;
                    args.Custom03 = custom3;
                    args.Custom04 = custom4;
                    args.Custom05 = custom5;
                    args.Custom06 = "";
                    args.Custom07 = "";
                    args.Custom08 = "";
                    args.Custom09 = "";
                    args.UpdateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    args.UpdateUser = TAP.UI.InfoBase._USER_INFO.Name;

                    int AddCount = bs.ExecuteModify("SaveMessageGroup", args.getPack());
                    if (AddCount == 0)
                    {
                        TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Insert failure..");
                    }
                    else
                    {
                        TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Insert complete..");
                    }
                }

                InitializeComboBox();
                gridControl1.DataSource = null;
            }
            catch (Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());

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
                    List<CommonArgsPack> listArgs = new List<CommonArgsPack>();
                    for (int i = 0; i < selectRowNum.Count; i++)
                    {
                        DataRow tmpRow = gridView1.GetDataRow(selectRowNum[i]);
                        string messageType = tmpRow["MESSAGETYPE"].ToString();
                        string messageName = tmpRow["MESSAGENAME"].ToString();
                        //  string custom1 = tmpRow["custom01"].ToString();
                        //  string custom2 = tmpRow["custom02"].ToString();
                        //  string custom3 = tmpRow["custom03"].ToString();
                        //  string custom4 = tmpRow["custom04"].ToString();
                        //  string custom5 = tmpRow["custom05"].ToString();
                        args = new CommonArgsPack();
                        args.MessageType = messageType;
                        args.MessageName = messageName;
                        //  args.Custom01 = custom1;
                        //  args.Custom02 = custom2;
                        //  args.Custom03 = custom3;
                        //  args.Custom04 = custom4;
                        //  args.Custom05 = custom5;
                        //  args.Custom06 = "";
                        //  args.Custom07 = "";
                        //  args.Custom08 = "";
                        //  args.Custom09 = "";
                        listArgs.Add(args);
                    }

                    ArgumentPack tmpap = new ArgumentPack();
                    tmpap.AddArgument("arguments", typeof(List<CommonArgsPack>), listArgs);
                    int SaveCount = bs.ExecuteModify("DeleteGroupMember", tmpap);
                    if (SaveCount > 0)
                        TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Delete Completed!");
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
                    List<CommonArgsPack> listArgs = new List<CommonArgsPack>();
                    for (int i = 0; i < focusedRowHandles.Count; i++)
                    {
                        DataRow tmpRow = gridView1.GetDataRow(focusedRowHandles[i]);
                        string messageType = tmpRow["MESSAGETYPE"].ToString();
                        string messageName = tmpRow["MESSAGENAME"].ToString();
                        string custom1 = tmpRow["custom01"].ToString();
                        string custom2 = tmpRow["custom02"].ToString();
                        string custom3 = tmpRow["custom03"].ToString();
                        string custom4 = tmpRow["custom04"].ToString();
                        string custom5 = tmpRow["custom05"].ToString();
                        string custom6 = tmpRow["custom06"].ToString();
                        string custom7 = tmpRow["custom07"].ToString();
                        string custom8 = tmpRow["custom08"].ToString();
                        string custom9 = tmpRow["custom09"].ToString();
                        args = new CommonArgsPack();
                        args.MessageType = messageType;
                        args.MessageName = messageName;
                        args.Custom01 = custom1;
                        args.Custom02 = custom2;
                        args.Custom03 = custom3;
                        args.Custom04 = custom4;
                        args.Custom05 = custom5;
                        args.Custom06 = custom6;
                        args.Custom07 = custom7;
                        args.Custom08 = custom8;
                        args.Custom09 = custom9;
                        args.UpdateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        args.UpdateUser = TAP.UI.InfoBase._USER_INFO.Name;
                        listArgs.Add(args);
                    }

                    ArgumentPack tmpap = new ArgumentPack();
                    tmpap.AddArgument("arguments", typeof(List<CommonArgsPack>), listArgs);
                    int SaveCount = bs.ExecuteModify("UpdateGroupMember", tmpap);
                    if (SaveCount > 0)
                        TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Update Completed!");

                    focusedRowHandles.Clear();
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
                cboMessageType.changeUpdateStatus(true);
                cboMessageType.Text = "";
                txtMessageType.Text = "";
                txtMessageName.Text = "";
                custom01.Text = "";
                custom02.Text = "";
                custom03.Text = "";
                custom04.Text = "";
                custom05.Text = "";
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