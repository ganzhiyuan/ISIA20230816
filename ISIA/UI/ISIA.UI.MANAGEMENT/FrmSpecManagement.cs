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
    public partial class FrmSpecManagement : DockUIBase1T1
    {

        #region Feild
        BizDataClient bs = null;
        //ComboBoxControl ComboBoxControl = new ComboBoxControl();
        SpecManagementArgsPack args = new SpecManagementArgsPack();

        DataSet ds = new DataSet();

        DataTable DATABASETable = new DataTable();
        
        #endregion


        public FrmSpecManagement()
        {
            InitializeComponent();
            InitializeComboBox();
            /*tDateTimePickerSE1.StartDate = DateTime.ParseExact(DateTime.Now.AddYears(-1).ToString("yyyy"), "yyyy", System.Globalization.CultureInfo.CurrentCulture);
            tDateTimePickerSE1.EndDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy"), "yyyy", System.Globalization.CultureInfo.CurrentCulture);
            */
            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.SpecManagement");
        }


        #region mothod

        private void InitializeComboBox()
        {

        }


        public DataSet LoadData()
        {
            
            /* ds = bs.ExecuteDataSet("GetBMPMINFO", args.getPack());
             ds_NUM = bs.ExecuteDataSet("GetBMPMNUM", args.getPack());
             ds_OUTPUT = bs.ExecuteDataSet("GetOUTPUT", args.getPack());*/

            ds = bs.ExecuteDataSet("GetDB");

            DATABASETable = bs.ExecuteDataTable("GetTCODE");//查询TAPCTCODES表

            ds.Tables[0].Columns.Add(new DataColumn("DBNAME", typeof(System.String)));

            ds.Tables[0].Columns["DBNAME"].SetOrdinal(2);

            foreach (DataRow rows in ds.Tables[0].Rows)
            {
                string dbname = DATABASETable.Select( String.Format("DBID = '{0}'", rows["DBID"].ToString()))[0]["DBNAME"].ToString();
                
                rows["DBNAME"] = dbname;

                if (rows["M_VALUE"].ToString().Contains("If"))
                {
                    if (string.IsNullOrEmpty(rows["M_VALUE"].ToString()))
                    {

                        rows["RULETEXT"] = String.Format(rows["RULETEXT"].ToString(), rows["N_VALUE"].ToString());

                    }
                    else
                    {

                        rows["RULETEXT"] = String.Format(rows["RULETEXT"].ToString(), rows["N_VALUE"].ToString(), rows["M_VALUE"].ToString());

                    }
                }
                else
                {

                }

                
            }

            ds.Tables[0].Columns["RULENAME"].ColumnName = "Rule Space Name";


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
           

            gridView1.GridControl.RepositoryItems.Clear();

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

            //gridView1.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;//编辑器模式
            gridView1.OptionsBehavior.EditingMode = GridEditingMode.Default;

            gridView1.Columns["DBID"].Visible = false;//设置DBID不展示

            gridView1.Columns["RID"].Visible = false;

            //gridView1.Columns["RULETEXT"].OptionsColumn.AllowEdit = false;//设置列禁止编辑

            gridView1.Columns["RULETEXT"].OptionsColumn.AllowEdit = true;

            gridView1.Columns["ID"].OptionsColumn.AllowEdit = false;//设置列禁止编辑

            //gridView1.Columns["ID"].Visible = false;


            //DBNAME创建combobox
            RepositoryItemComboBox DBNAME = new RepositoryItemComboBox();
            DBNAME.Name = "DBNAME";
            List<string> lstDB = (from d in DATABASETable.AsEnumerable() select d.Field<string>("DBNAME")).ToList();
            DBNAME.Items.AddRange(lstDB);
            //DBNAME.TextEditStyle = TextEditStyles.DisableTextEditor;
            DBNAME.TextEditStyle = TextEditStyles.Standard;
            DBNAME.AutoComplete = true;
            gridView.GridControl.RepositoryItems.Add(DBNAME);
            gridView.Columns["DBNAME"].ColumnEdit = DBNAME;
            gridView.Columns["DBNAME"].ColumnEditName = DBNAME.ToString();

            //RULENAME创建combobox
            RepositoryItemComboBox RULENAME = new RepositoryItemComboBox();
            RULENAME.Name = "RULENAME";
            RULENAME.AutoComplete = true;
            List<string> lstRulename = (from d in ds.Tables[0].AsEnumerable() select d.Field<string>("Rule Space Name")).Distinct<string>().ToList();
            RULENAME.Items.AddRange(lstRulename);
            //RULENAME.TextEditStyle = TextEditStyles.DisableTextEditor;
            RULENAME.TextEditStyle = TextEditStyles.Standard;
            gridView.GridControl.RepositoryItems.Add(RULENAME);
            gridView.Columns["Rule Space Name"].ColumnEdit = RULENAME;
            gridView.Columns["Rule Space Name"].ColumnEditName = RULENAME.ToString();

            

            //创建数字输入控件
            RepositoryItemSpinEdit Rulenno = new RepositoryItemSpinEdit();
            Rulenno.Name = "Ruleno";
            Rulenno.EditMask = "d";
            Rulenno.MinValue = 1;
            Rulenno.MaxValue = 9999999999;
            Rulenno.MaxLength = 1;
            gridView.GridControl.RepositoryItems.Add(Rulenno);
            gridView.Columns["RULENO"].ColumnEdit = Rulenno;
            gridView.Columns["RULENO"].ColumnEditName = Rulenno.ToString();

            RepositoryItemSpinEdit Nvalue = new RepositoryItemSpinEdit();
            Nvalue.Name = "Nvalue";
            Nvalue.EditMask = "d";
            Nvalue.MinValue = 1;
            Nvalue.MaxValue = 9999999999;
            Nvalue.MaxLength = 1;
            gridView.GridControl.RepositoryItems.Add(Nvalue);
            gridView.Columns["N_VALUE"].ColumnEdit = Nvalue;
            gridView.Columns["N_VALUE"].ColumnEditName = Nvalue.ToString();

            RepositoryItemSpinEdit Mvalue = new RepositoryItemSpinEdit();
            Mvalue.Name = "Mvalue";
            Mvalue.EditMask = "d";
            Mvalue.MinValue = 1;
            Mvalue.MaxValue = 9999999999;
            Mvalue.MaxLength = 1;
            gridView.GridControl.RepositoryItems.Add(Mvalue);
            gridView.Columns["M_VALUE"].ColumnEdit = Mvalue;
            gridView.Columns["M_VALUE"].ColumnEditName = Mvalue.ToString();


            //ISLIVE创建checkbox控件
            RepositoryItemCheckEdit Islive = new RepositoryItemCheckEdit();
            Islive.Name = "Islive";
            Islive.ValueChecked = "YES";
            Islive.ValueUnchecked = "NO";
            gridView.GridControl.RepositoryItems.Add(Islive);
            gridView.Columns["ISALIVE"].ColumnEdit = Islive;
            gridView.Columns["ISALIVE"].ColumnEditName = Islive.ToString();

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

            string aa = "aaa";
            return;
            
            DataRowView dataRowView = (DataRowView)e.Row;
            //dataRowView.Row.ItemArray;
            /*args.CATEGORY = dataRowView.Row["CATEGORY"].ToString();
            args.SUBCATEGORY = dataRowView.Row["SUBCATEGORY"].ToString();
            args.NAME = dataRowView.Row["NAME"].ToString();
            args.USED = dataRowView.Row["USED"].ToString();
            args.CUSTOM01 = dataRowView.Row["CUSTOM01"].ToString();
            args.CUSTOM02 = dataRowView.Row["CUSTOM02"].ToString();
            args.CUSTOM03 = dataRowView.Row["CUSTOM03"].ToString();
            args.CUSTOM04 = dataRowView.Row["CUSTOM04"].ToString();
            args.CUSTOM05 = dataRowView.Row["CUSTOM05"].ToString();
            args.CUSTOM06 = dataRowView.Row["CUSTOM06"].ToString();
            args.CUSTOM07 = dataRowView.Row["CUSTOM07"].ToString();*/

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
                //string aa = "aa";
                
               /* //DevExpress.XtraGrid.Columns.GridColumn[] gridColumns = gridView1.Columns.ToArray();

                *//*List<RepositoryItemCollection> itemCollections = new List<RepositoryItemCollection>();//RepositoryItems添加集合

                itemCollections.Add(gridControl1.RepositoryItems);*//*

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

                    if (!string.IsNullOrEmpty(args.ROWID))
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
                }*/

            }
            else if (e.Button.ButtonType == NavigatorButtonType.Remove) {

                DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                args.ROWID = dataRow["RID"].ToString();
                //gridView1.DeleteRow(gridView1.FocusedRowHandle);

                int Res = bs.ExecuteModify("DelteSpec", args.getPack());

                if (Res == 1)
                {
                    XtraMessageBox.Show("Delete data succeeded ", " ");
                }
                else
                {
                    XtraMessageBox.Show("Delete data failed ", " ");
                }

            }
            else if (e.Button.ButtonType == NavigatorButtonType.EndEdit)
            {


                //DataRowView dataRow1 = (DataRowView)gridView1.GetFocusedRow(); 
                DataRow dataRow = gridView1.GetFocusedDataRow();
                //DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                args.DBID = DATABASETable.Select(String.Format("DBNAME = '{0}'", dataRow["DBNAME"].ToString()))[0]["DBID"].ToString();
                args.RULENAME = dataRow["Rule Space Name"].ToString();
                args.RULENO = dataRow["RULENO"].ToString();
                args.N_VALUE = dataRow["N_VALUE"].ToString();
                args.M_VALUE = dataRow["M_VALUE"].ToString();
                args.ISALIVE = dataRow["ISALIVE"].ToString();
                args.ROWID = dataRow["RID"].ToString();

                if (string.IsNullOrEmpty(dataRow["RID"].ToString()))
                {
                    
                    int Res = bs.ExecuteModify("NewSPEC", args.getPack());

                    if (Res == 1)
                    {
                        XtraMessageBox.Show("Add Data Succeeded ", " ");
                    }

                    tbnSeach_Click(null, null);
                }
                else
                {
                    int Res = bs.ExecuteModify("UpdateSpec", args.getPack());

                    if (Res == 1)
                    {
                        XtraMessageBox.Show("Update Data Succeeded ", " ");
                    }

                    tbnSeach_Click(null, null);

                }

                

                
            }

        }

        private void FrmAdd_gridtable(DataTable dataTable)
        {
            /*args.CATEGORY = dataTable.Rows[0]["CATEGORY"].ToString();
            args.SUBCATEGORY = dataTable.Rows[0]["SUBCATEGORY"].ToString();
            args.NAME = dataTable.Rows[0]["NAME"].ToString();
            args.USED = dataTable.Rows[0]["USED"].ToString();
            args.CUSTOM01 = dataTable.Rows[0]["CUSTOM01"].ToString();
            args.CUSTOM02 = dataTable.Rows[0]["CUSTOM02"].ToString();
            args.CUSTOM03 = dataTable.Rows[0]["CUSTOM03"].ToString();
            args.CUSTOM04 = dataTable.Rows[0]["CUSTOM04"].ToString();
            args.CUSTOM05 = dataTable.Rows[0]["CUSTOM05"].ToString();
            args.CUSTOM06 = dataTable.Rows[0]["CUSTOM06"].ToString();
            args.CUSTOM07 = dataTable.Rows[0]["CUSTOM07"].ToString();*/


            /*int Res = bs.ExecuteModify("NewTCODE", args.getPack());

            if (Res == 1)
            {
                XtraMessageBox.Show("Adding data succeeded ", " ");
            }*/

        }

    }
}
