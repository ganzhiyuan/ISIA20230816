using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP;
using TAP.Data.Client;
/*using TAP.Models;
using TAP.Models.Factories.Facilities;*/
using TAP.UI;
using ISIA.UI.BASE;
using DevExpress.XtraTab;
//using DevExpress.XtraCharts;
//using DevExpress.XtraVerticalGrid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Base;

namespace ISIA.UI.MANAGEMENT
{
    public partial class FrmDataBaseManagementCopy : DockUIBase1T1
    {

        #region Feild
        BizDataClient bs = null;
        ComboBoxControl ComboBoxControl = new ComboBoxControl();
        DataBaseManagementArgsPack args = new DataBaseManagementArgsPack();

        DataSet ds = new DataSet();

        
        //private DataSet ds_NUM;
        //private DataSet ds_OUTPUT;
        #endregion


        public FrmDataBaseManagementCopy()
        {
            InitializeComponent();
            InitializeComboBox();
            /*tDateTimePickerSE1.StartDate = DateTime.ParseExact(DateTime.Now.AddYears(-1).ToString("yyyy"), "yyyy", System.Globalization.CultureInfo.CurrentCulture);
            tDateTimePickerSE1.EndDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy"), "yyyy", System.Globalization.CultureInfo.CurrentCulture);
            */
            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.DataBaseManagement");
        }


        #region mothod

        private void InitializeComboBox()
        {

        }




        public DataSet LoadData()
        {
            
            /*string startTime = tDateTimePickerSE1.StartDateString.Substring(0, 14);
            string endTime = tDateTimePickerSE1.EndDateString.Substring(0, 14);
            string start = tDateTimePickerSE1.StartDateString.Substring(0, 8);
            string end = tDateTimePickerSE1.StartDateString.Substring(0, 8);
            args.Workshop = cboworkshop.Text;
            args.FACILITY = cbofacility.Text;
            args.Process_Type = cboprocess.Text;
            args.Report_SatrtDate = startTime;
            args.Report_EndDate = endTime;
            args.Report_Satrt = start;
            args.Report_End = end;*/

            /* ds = bs.ExecuteDataSet("GetBMPMINFO", args.getPack());
             ds_NUM = bs.ExecuteDataSet("GetBMPMNUM", args.getPack());
             ds_OUTPUT = bs.ExecuteDataSet("GetOUTPUT", args.getPack());*/

            ds = bs.ExecuteDataSet("GetDB");
            
            return ds;
        }

        public void DisplayData(DataSet ds)
        {
            if (ds == null)
            {
                return;
            }
            GridViewDataBinding();
            GridViewStyle(gridView1);
        }

        public void GridViewDataBinding()
        {
            gridControl1.DataSource = null;

            gridView1.Columns.Clear();

            gridControl1.DataSource = ds.Tables[0];


            //gridview 内增加按钮
            
            /*DevExpress.XtraGrid.Columns.GridColumn gridColumn = new DevExpress.XtraGrid.Columns.GridColumn();

            gridView1.Columns.Add(gridColumn);
            gridColumn.Visible = true;
            gridColumn.Caption = "DELETE";
            gridColumn.FieldName = "OPERATION";
            RepositoryItemButtonEdit edit = new RepositoryItemButtonEdit();

            edit.ButtonClick += Edit_ButtonClick;

            edit.Buttons[0].Kind = ButtonPredefines.Delete;
            edit.Tag = "DELETE";
            edit.Buttons[0].Caption = "DELETE";
            edit.TextEditStyle = TextEditStyles.HideTextEditor;
            gridControl1.RepositoryItems.Add(edit);
            gridColumn.ColumnEdit = edit;

            gridColumn.ShowButtonMode = (DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum)ShowButtonModeEnum.ShowAlways;*/


            

        }

        

        public void GridViewStyle(GridView gridView)
        {
            gridView1.OptionsBehavior.Editable = true; 

            gridView1.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;

            

            gridView1.Columns["ID"].Visible = false;


            RepositoryItemComboBox edit = new RepositoryItemComboBox();
            edit.Name = "CUSTOM05";
            edit.Items.Add("aaa1");
            edit.Items.Add("bbbb");
            gridView.GridControl.RepositoryItems.Add(edit);

            gridView.Columns["CUSTOM05"].ColumnEdit = edit;
            gridView.Columns["CUSTOM05"].ColumnEditName = edit.ToString();

            RepositoryItemComboBox edit1 = new RepositoryItemComboBox();
            edit1.Name = "CUSTOM06";
            edit1.Items.Add("aaa2");
            edit1.Items.Add("bbbb");
            gridView.GridControl.RepositoryItems.Add(edit1);
            
            gridView.Columns["CUSTOM06"].ColumnEdit = edit1;
            gridView.Columns["CUSTOM06"].ColumnEditName = edit1.ToString();
        }

        private void Edit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Delete)
            {
                if (XtraMessageBox.Show("Do you wish to remove this row?", "Warring", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    gridView1.DeleteRow(gridView1.FocusedRowHandle);
                    DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    args.ROWID = dataRow["ID"].ToString();

                    int Res = bs.ExecuteModify("DelteTCODE", args.getPack());

                    if (Res == 1)
                    {
                        XtraMessageBox.Show("Delete data succeeded ", " ");
                    }
                    else {
                        XtraMessageBox.Show("Delete data failed ", " ");
                    }
                    
                }
            }
        }
    

        /*         private RepositoryItemButtonEdit CreateRepositoryItemButtonEdit(Dictionary<object, string> dicButtons)
                  {
                      RepositoryItemButtonEdit repositoryBtn = new RepositoryItemButtonEdit();
                      repositoryBtn.AppearanceDisabled.Options.UseTextOptions = true;
                      repositoryBtn.AppearanceDisabled.TextOptions.HAlignment = HorzAlignment.Near;
                      repositoryBtn.AutoHeight = false;
                      repositoryBtn.TextEditStyle = TextEditStyles.HideTextEditor;
                      repositoryBtn.ButtonsStyle = BorderStyles.UltraFlat;
                      repositoryBtn.Buttons.Clear();
                     EditorButton btn = null;
                     foreach (KeyValuePair<object, string> item in dicButtons)
                     {
                         btn = new EditorButton();
                         btn.Kind = ButtonPredefines.Glyph;
                         btn.Caption = item.Value;
                         btn.Tag = item.Key;
                         repositoryBtn.Buttons.Add(btn);
                     }
                     return repositoryBtn;
                 }*/

        

        

        #endregion

        #region event
        private void tbnSeach_Click(object sender, EventArgs e)
        {
            try
            {
                //ComboBoxControl.SetCrossLang(this._translator);
                if (!base.ValidateUserInput(this.layoutControl1)) return;
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }








        #endregion

        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {


            
            DataRowView dataRowView = (DataRowView)e.Row;
            //dataRowView.Row.ItemArray;
            args.CATEGORY = dataRowView.Row["CATEGORY"].ToString();
            args.SUBCATEGORY = dataRowView.Row["SUBCATEGORY"].ToString();
            args.NAME = dataRowView.Row["NAME"].ToString();
            args.USED = dataRowView.Row["USED"].ToString();
            args.CUSTOM01 = dataRowView.Row["CUSTOM01"].ToString();
            args.CUSTOM02 = dataRowView.Row["CUSTOM02"].ToString();
            args.CUSTOM03 = dataRowView.Row["CUSTOM03"].ToString();
            args.CUSTOM04 = dataRowView.Row["CUSTOM04"].ToString();
            args.CUSTOM05 = dataRowView.Row["CUSTOM05"].ToString();
            args.CUSTOM06 = dataRowView.Row["CUSTOM06"].ToString();
            args.CUSTOM07 = dataRowView.Row["CUSTOM07"].ToString();




/*            if (string.IsNullOrEmpty(dataRowView.Row["ID"].ToString()))
            {

                for (int i = 0; i < dataRowView.Row.ItemArray.Length; i++)
                {
                    if (string.IsNullOrEmpty(dataRowView.Row.ItemArray[i].ToString()) && i != 0)
                    {
                        XtraMessageBox.Show("Please enter all values", "Warring", MessageBoxButtons.YesNo);
                        dataRowView.Row.Delete();
                        return;

                    }

                }

                int Res = bs.ExecuteModify("NewTCODE", args.getPack());

                if (Res == 1)
                {
                    XtraMessageBox.Show("Adding data succeeded ", " ");
                }
                return;

            }*/

            
                args.ROWID = dataRowView.Row["ID"].ToString();
                int Res = bs.ExecuteModify("UpdateTCODE", args.getPack());

                if (Res == 1)
                {
                    XtraMessageBox.Show("Update data succeeded ", " ");
                }

        }




        private void gridControl1_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {

            if (e.Button.ButtonType == NavigatorButtonType.Append)
            {


                //DevExpress.XtraGrid.Columns.GridColumn[] gridColumns = gridView1.Columns.ToArray();

                /*List<RepositoryItemCollection> itemCollections = new List<RepositoryItemCollection>();//RepositoryItems添加集合

                itemCollections.Add(gridControl1.RepositoryItems);*/

                //ts[9].ColumnEdit.GetType().Name.ToString();//获取repository类型

                

                List<DevExpress.XtraGrid.Columns.GridColumn> ts = gridView1.Columns.ToList();
                //ts[9].ColumnEditName.ToString();

                ts.Remove(ts[0]);

                FrmAdd frmAdd = new FrmAdd(ts);
                frmAdd.StartPosition = FormStartPosition.CenterParent;
                frmAdd.gridtable += new gridtable(FrmAdd_gridtable);
                frmAdd.ShowDialog();

                if (frmAdd.IsDisposed)
                {

                    if (!string.IsNullOrEmpty(args.CATEGORY))
                    {
                        int Res = bs.ExecuteModify("NewTCODE", args.getPack());

                        if (Res == 1)
                        {
                            XtraMessageBox.Show("Adding data succeeded ", " ");
                        }
                    }
                    tbnSeach_Click(null, null);
                }
                else {
                    return;
                }

                

            }
            else if (e.Button.ButtonType == NavigatorButtonType.Remove) {

                DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                args.ROWID = dataRow["ID"].ToString();
                //gridView1.DeleteRow(gridView1.FocusedRowHandle);

                int Res = bs.ExecuteModify("DelteTCODE", args.getPack());

                if (Res == 1)
                {
                    XtraMessageBox.Show("Delete data succeeded ", " ");
                }
                else
                {
                    XtraMessageBox.Show("Delete data failed ", " ");
                }

            }



        }

        private void FrmAdd_gridtable(DataTable dataTable)
        {
            args.CATEGORY = dataTable.Rows[0]["CATEGORY"].ToString();
            args.SUBCATEGORY = dataTable.Rows[0]["SUBCATEGORY"].ToString();
            args.NAME = dataTable.Rows[0]["NAME"].ToString();
            args.USED = dataTable.Rows[0]["USED"].ToString();
            args.CUSTOM01 = dataTable.Rows[0]["CUSTOM01"].ToString();
            args.CUSTOM02 = dataTable.Rows[0]["CUSTOM02"].ToString();
            args.CUSTOM03 = dataTable.Rows[0]["CUSTOM03"].ToString();
            args.CUSTOM04 = dataTable.Rows[0]["CUSTOM04"].ToString();
            args.CUSTOM05 = dataTable.Rows[0]["CUSTOM05"].ToString();
            args.CUSTOM06 = dataTable.Rows[0]["CUSTOM06"].ToString();
            args.CUSTOM07 = dataTable.Rows[0]["CUSTOM07"].ToString();


            /*int Res = bs.ExecuteModify("NewTCODE", args.getPack());

            if (Res == 1)
            {
                XtraMessageBox.Show("Adding data succeeded ", " ");
            }*/

            
        }


    }
}
