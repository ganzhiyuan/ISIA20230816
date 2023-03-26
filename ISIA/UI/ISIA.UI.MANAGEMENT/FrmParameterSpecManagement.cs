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
    public partial class FrmParameterSpecManagement : DockUIBase1T1
    {

//        #region Feild
//        BizDataClient bs = null;
//        //ComboBoxControl ComboBoxControl = new ComboBoxControl();
//        ParameterSpecManagementArgsPack args = new ParameterSpecManagementArgsPack();

//        DataSet ds = new DataSet();

//        DataTable DATABASETable = new DataTable();

//        DataTable ParameterTable = new DataTable();

//        #endregion


//        public FrmParameterSpecManagement()
//        {
//            InitializeComponent();
//            InitializeComboBox();
//            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.ParameterSpecManagement");
//        }


//        #region mothod

//        private void InitializeComboBox()
//        {

//        }


//        public DataSet LoadData()
//        {
            
            

//            ds = bs.ExecuteDataSet("GetDB");

//            DATABASETable = bs.ExecuteDataTable("GetTCODE");//查询TAPCTCODES表

//            ParameterTable = bs.ExecuteDataTable("GetParameterdef");//查询PARAMETER表

//            ds.Tables[0].Columns.Add(new DataColumn("DBNAME", typeof(System.String)));

//            ds.Tables[0].Columns["DBNAME"].SetOrdinal(2);

//            foreach (DataRow rows in ds.Tables[0].Rows)
//            {
//                string dbname = DATABASETable.Select( String.Format("DBID = '{0}'", rows["DBID"].ToString()))[0]["DBNAME"].ToString();
                
//                rows["DBNAME"] = dbname;

               
//            }

//            ds.Tables[0].Columns["RULENAME"].ColumnName = "Rule Space Name";


//            return ds;
//        }

//        public void DisplayData(DataSet ds)
//        {
//            if (ds == null)
//            {
//                return;
//            }
//            GridViewDataBinding();
//            GridViewStyle(gridView1);
//        }

//        public void GridViewDataBinding()
//        {
//            gridView1.GridControl.RepositoryItems.Clear();

//            gridControl1.DataSource = null;

//            gridView1.Columns.Clear();

//            gridControl1.DataSource = ds.Tables[0];


//            //gridview 内增加按钮
            
//            /*DevExpress.XtraGrid.Columns.GridColumn gridColumn = new DevExpress.XtraGrid.Columns.GridColumn();

//            gridView1.Columns.Add(gridColumn);
//            gridColumn.Visible = true;
//            gridColumn.Caption = "DELETE";
//            gridColumn.FieldName = "OPERATION";
//            RepositoryItemButtonEdit edit = new RepositoryItemButtonEdit();

//            edit.ButtonClick += Edit_ButtonClick;

//            edit.Buttons[0].Kind = ButtonPredefines.Delete;
//            edit.Tag = "DELETE";
//            edit.Buttons[0].Caption = "DELETE";
//            edit.TextEditStyle = TextEditStyles.HideTextEditor;
//            gridControl1.RepositoryItems.Add(edit);
//            gridColumn.ColumnEdit = edit;

//            gridColumn.ShowButtonMode = (DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum)ShowButtonModeEnum.ShowAlways;*/

//        }

        

//        public void GridViewStyle(GridView gridView)
//        {
//            gridView1.OptionsBehavior.Editable = true;

//            //gridView1.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;//编辑器模式
//            gridView1.OptionsBehavior.EditingMode = GridEditingMode.Default;

//            gridView1.Columns["DBID"].Visible = false;//设置DBID不展示

//            gridView1.Columns["RID"].Visible = false;

//            gridView1.Columns["PARAMETERID"].Visible = false;

//            //gridView1.Columns["RULETEXT"].OptionsColumn.AllowEdit = false;//设置列禁止编辑

//            gridView1.Columns["ID"].OptionsColumn.AllowEdit = false;//设置列禁止编辑

//            //gridView1.Columns["ID"].Visible = false;


//             //DBNAME创建combobox
//             RepositoryItemComboBox DBNAME = new RepositoryItemComboBox();
//             DBNAME.Name = "DBNAME";
//             List<string> lstDB = (from d in DATABASETable.AsEnumerable() select d.Field<string>("DBNAME")).ToList();
//             DBNAME.Items.AddRange(lstDB);
//             //DBNAME.TextEditStyle = TextEditStyles.DisableTextEditor;
//             DBNAME.TextEditStyle = TextEditStyles.Standard;
//             DBNAME.AutoComplete = true;
//             gridView.GridControl.RepositoryItems.Add(DBNAME);
//             gridView.Columns["DBNAME"].ColumnEdit = DBNAME;
//             gridView.Columns["DBNAME"].ColumnEditName = DBNAME.ToString();

//             //ParameterName创建combobox
//             RepositoryItemComboBox PARAMETERNAME = new RepositoryItemComboBox();
//             PARAMETERNAME.Name = "PARAMETERNAME";
//             PARAMETERNAME.AutoComplete = true;
//             List<string> lstPara = (from d in ParameterTable.AsEnumerable() select d.Field<string>("PARAMETERNAME")).Distinct<string>().ToList();
//             PARAMETERNAME.Items.AddRange(lstPara);
//            //RULENAME.TextEditStyle = TextEditStyles.DisableTextEditor;
//             PARAMETERNAME.TextEditStyle = TextEditStyles.Standard;
//             gridView.GridControl.RepositoryItems.Add(PARAMETERNAME);
//             gridView.Columns["PARAMETERNAME"].ColumnEdit = PARAMETERNAME;
//             gridView.Columns["PARAMETERNAME"].ColumnEditName = PARAMETERNAME.ToString();

//             //RULENAME创建combobox
//             RepositoryItemComboBox RULENAME = new RepositoryItemComboBox();
//             RULENAME.Name = "RULENAME";
//             RULENAME.AutoComplete = true;
//             List<string> lstRulename = (from d in ds.Tables[0].AsEnumerable() select d.Field<string>("Rule Space Name")).Distinct<string>().ToList();
//             RULENAME.Items.AddRange(lstRulename);
//             //RULENAME.TextEditStyle = TextEditStyles.DisableTextEditor;
//             RULENAME.TextEditStyle = TextEditStyles.Standard;
//             gridView.GridControl.RepositoryItems.Add(RULENAME);
//             gridView.Columns["Rule Space Name"].ColumnEdit = RULENAME;
//             gridView.Columns["Rule Space Name"].ColumnEditName = RULENAME.ToString();


            
//              //创建数字输入控件
//              RepositoryItemSpinEdit Rulenno = new RepositoryItemSpinEdit();
//              Rulenno.Name = "Ruleno";
//              Rulenno.EditMask = "d";
//              Rulenno.MinValue = 1;
//              Rulenno.MaxValue = 9999999999999999;
//              Rulenno.MaxLength = 1;
//              gridView.GridControl.RepositoryItems.Add(Rulenno);
//              gridView.Columns["RULENO"].ColumnEdit = Rulenno;
//              gridView.Columns["RULENO"].ColumnEditName = Rulenno.ToString();


//              RepositoryItemSpinEdit Day = new RepositoryItemSpinEdit();
//              Day.Name = "Day";
//              Rulenno.EditMask = "d";
//              Rulenno.MinValue = 1;
//              Rulenno.MaxValue = 9999999999999999;
//              Rulenno.MaxLength = 1;
//              gridView.GridControl.RepositoryItems.Add(Day);
//              gridView.Columns["DAYS"].ColumnEdit = Day;
//              gridView.Columns["DAYS"].ColumnEditName = Day.ToString();



//            RepositoryItemSpinEdit SPECUPPERLIMIT = new RepositoryItemSpinEdit();
//            SPECUPPERLIMIT.Name = "SPECUPPERLIMIT";
//            SPECUPPERLIMIT.EditMask = "d";
//            SPECUPPERLIMIT.MinValue = 1;
//            SPECUPPERLIMIT.MaxValue = 9999999999999999;
//            SPECUPPERLIMIT.MaxLength = 1;
//            gridView.GridControl.RepositoryItems.Add(SPECUPPERLIMIT);
//            gridView.Columns["SPECUPPERLIMIT"].ColumnEdit = SPECUPPERLIMIT;
//            gridView.Columns["SPECUPPERLIMIT"].ColumnEditName = SPECUPPERLIMIT.ToString();

//            RepositoryItemSpinEdit SPECLOWERLIMIT = new RepositoryItemSpinEdit();
//            SPECLOWERLIMIT.Name = "SPECLOWERLIMIT";
//            SPECLOWERLIMIT.EditMask = "d";
//            SPECLOWERLIMIT.MinValue = 1;
//            SPECLOWERLIMIT.MaxValue = 9999999999999999;
//            SPECLOWERLIMIT.MaxLength = 1;
//            gridView.GridControl.RepositoryItems.Add(SPECLOWERLIMIT);
//            gridView.Columns["SPECLOWERLIMIT"].ColumnEdit = SPECLOWERLIMIT;
//            gridView.Columns["SPECLOWERLIMIT"].ColumnEditName = SPECLOWERLIMIT.ToString();

//            RepositoryItemSpinEdit CONTROLUPPERLIMIT = new RepositoryItemSpinEdit();
//            CONTROLUPPERLIMIT.Name = "CONTROLUPPERLIMIT";
//            CONTROLUPPERLIMIT.EditMask = "d";
//            CONTROLUPPERLIMIT.MinValue = 1;
//            CONTROLUPPERLIMIT.MaxValue = 9999999999999999;
//            CONTROLUPPERLIMIT.MaxLength = 1;
//            gridView.GridControl.RepositoryItems.Add(CONTROLUPPERLIMIT);
//            gridView.Columns["CONTROLUPPERLIMIT"].ColumnEdit = CONTROLUPPERLIMIT;
//            gridView.Columns["CONTROLUPPERLIMIT"].ColumnEditName = CONTROLUPPERLIMIT.ToString();

//            RepositoryItemSpinEdit CONTROLLOWERLIMIT = new RepositoryItemSpinEdit();
//            CONTROLLOWERLIMIT.Name = "CONTROLLOWERLIMIT";
//            CONTROLLOWERLIMIT.EditMask = "d";
//            CONTROLLOWERLIMIT.MinValue = 1;
//            CONTROLLOWERLIMIT.MaxValue = 9999999999999999;
//            CONTROLLOWERLIMIT.MaxLength = 1;
//            gridView.GridControl.RepositoryItems.Add(CONTROLLOWERLIMIT);
//            gridView.Columns["CONTROLLOWERLIMIT"].ColumnEdit = CONTROLLOWERLIMIT;
//            gridView.Columns["CONTROLLOWERLIMIT"].ColumnEditName = CONTROLLOWERLIMIT.ToString();

//            /*
//             RepositoryItemSpinEdit Nvalue = new RepositoryItemSpinEdit();
//             Nvalue.Name = "Nvalue";
//             Nvalue.EditMask = "n";
//             gridView.GridControl.RepositoryItems.Add(Nvalue);
//             gridView.Columns["N_VALUE"].ColumnEdit = Nvalue;
//             gridView.Columns["N_VALUE"].ColumnEditName = Nvalue.ToString();

//             RepositoryItemSpinEdit Mvalue = new RepositoryItemSpinEdit();
//             Mvalue.Name = "Mvalue";
//             Mvalue.EditMask = "n";
//             gridView.GridControl.RepositoryItems.Add(Mvalue);
//             gridView.Columns["M_VALUE"].ColumnEdit = Mvalue;
//             gridView.Columns["M_VALUE"].ColumnEditName = Mvalue.ToString();
//            */


//            RepositoryItemCheckEdit CHARTUSED = new RepositoryItemCheckEdit();
//            CHARTUSED.Name = "CHARTUSED";
//            CHARTUSED.ValueChecked = "YES";
//            CHARTUSED.ValueUnchecked = "NO";
//            gridView.GridControl.RepositoryItems.Add(CHARTUSED);
//            gridView.Columns["CHARTUSED"].ColumnEdit = CHARTUSED;
//            gridView.Columns["CHARTUSED"].ColumnEditName = CHARTUSED.ToString();

//            RepositoryItemCheckEdit MAILUSED = new RepositoryItemCheckEdit();
//            MAILUSED.Name = "MAILUSED";
//            MAILUSED.ValueChecked = "YES";
//            MAILUSED.ValueUnchecked = "NO";
//            gridView.GridControl.RepositoryItems.Add(MAILUSED);
//            gridView.Columns["MAILUSED"].ColumnEdit = MAILUSED;
//            gridView.Columns["MAILUSED"].ColumnEditName = MAILUSED.ToString();

//            RepositoryItemCheckEdit MMSUSED = new RepositoryItemCheckEdit();
//            MMSUSED.Name = "MMSUSED";
//            MMSUSED.ValueChecked = "YES";
//            MMSUSED.ValueUnchecked = "NO";
//            gridView.GridControl.RepositoryItems.Add(MMSUSED);
//            gridView.Columns["MMSUSED"].ColumnEdit = MMSUSED;
//            gridView.Columns["MMSUSED"].ColumnEditName = MMSUSED.ToString();

//            RepositoryItemCheckEdit SPECLIMITUSED = new RepositoryItemCheckEdit();
//            SPECLIMITUSED.Name = "SPECLIMITUSED";
//            SPECLIMITUSED.ValueChecked = "YES";
//            SPECLIMITUSED.ValueUnchecked = "NO";
//            gridView.GridControl.RepositoryItems.Add(SPECLIMITUSED);
//            gridView.Columns["SPECLIMITUSED"].ColumnEdit = SPECLIMITUSED;
//            gridView.Columns["SPECLIMITUSED"].ColumnEditName = SPECLIMITUSED.ToString();


//            //ISLIVE创建checkbox控件
//            RepositoryItemCheckEdit Islive = new RepositoryItemCheckEdit();
//            Islive.Name = "Islive";
//            Islive.ValueChecked = "YES";
//            Islive.ValueUnchecked = "NO";
//            gridView.GridControl.RepositoryItems.Add(Islive);
//            gridView.Columns["ISALIVE"].ColumnEdit = Islive;
//            gridView.Columns["ISALIVE"].ColumnEditName = Islive.ToString();

//        }

       

//        private void Edit_ButtonClick(object sender, ButtonPressedEventArgs e)
//        {
//            if (e.Button.Kind == ButtonPredefines.Delete)
//            {
//                if (XtraMessageBox.Show("Do you wish to remove this row?", "Warring", MessageBoxButtons.YesNo) == DialogResult.Yes)
//                {
//                    gridView1.DeleteRow(gridView1.FocusedRowHandle);
//                    DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
//                    args.ROWID = dataRow["ID"].ToString();

//                    int Res = bs.ExecuteModify("DelteTCODE", args.getPack());

//                    if (Res == 1)
//                    {
//                        XtraMessageBox.Show("Delete data succeeded ", " ");
//                    }
//                    else {
//                        XtraMessageBox.Show("Delete data failed ", " ");
//                    }
                    
//                }
//            }
//        }

//        /*         private RepositoryItemButtonEdit CreateRepositoryItemButtonEdit(Dictionary<object, string> dicButtons)
//                  {
//                      RepositoryItemButtonEdit repositoryBtn = new RepositoryItemButtonEdit();
//                      repositoryBtn.AppearanceDisabled.Options.UseTextOptions = true;
//                      repositoryBtn.AppearanceDisabled.TextOptions.HAlignment = HorzAlignment.Near;
//                      repositoryBtn.AutoHeight = false;
//                      repositoryBtn.TextEditStyle = TextEditStyles.HideTextEditor;
//                      repositoryBtn.ButtonsStyle = BorderStyles.UltraFlat;
//                      repositoryBtn.Buttons.Clear();
//                     EditorButton btn = null;
//                     foreach (KeyValuePair<object, string> item in dicButtons)
//                     {
//                         btn = new EditorButton();
//                         btn.Kind = ButtonPredefines.Glyph;
//                         btn.Caption = item.Value;
//                         btn.Tag = item.Key;
//                         repositoryBtn.Buttons.Add(btn);
//                     }
//                     return repositoryBtn;
//                 }*/


//        #endregion

//        #region event
//        private void tbnSeach_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                //ComboBoxControl.SetCrossLang(this._translator);
//                if (!base.ValidateUserInput(this.layoutControl1)) return;
//                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.ToString());
//            }
//        }



//        #endregion

//        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
//        {

//            DataRowView dataRowView = (DataRowView)e.Row;
//            //dataRowView.Row.ItemArray;
//            /*args.CATEGORY = dataRowView.Row["CATEGORY"].ToString();
//            args.SUBCATEGORY = dataRowView.Row["SUBCATEGORY"].ToString();
//            args.NAME = dataRowView.Row["NAME"].ToString();
//            args.USED = dataRowView.Row["USED"].ToString();
//            args.CUSTOM01 = dataRowView.Row["CUSTOM01"].ToString();
//            args.CUSTOM02 = dataRowView.Row["CUSTOM02"].ToString();
//            args.CUSTOM03 = dataRowView.Row["CUSTOM03"].ToString();
//            args.CUSTOM04 = dataRowView.Row["CUSTOM04"].ToString();
//            args.CUSTOM05 = dataRowView.Row["CUSTOM05"].ToString();
//            args.CUSTOM06 = dataRowView.Row["CUSTOM06"].ToString();
//            args.CUSTOM07 = dataRowView.Row["CUSTOM07"].ToString();*/

///*            if (string.IsNullOrEmpty(dataRowView.Row["ID"].ToString()))
//            {

//                for (int i = 0; i < dataRowView.Row.ItemArray.Length; i++)
//                {
//                    if (string.IsNullOrEmpty(dataRowView.Row.ItemArray[i].ToString()) && i != 0)
//                    {
//                        XtraMessageBox.Show("Please enter all values", "Warring", MessageBoxButtons.YesNo);
//                        dataRowView.Row.Delete();
//                        return;
//                    }
//                }

//                int Res = bs.ExecuteModify("NewTCODE", args.getPack());

//                if (Res == 1)
//                {
//                    XtraMessageBox.Show("Adding data succeeded ", " ");
//                }
//                return;

//            }*/
//                args.ROWID = dataRowView.Row["ID"].ToString();
//                int Res = bs.ExecuteModify("UpdateTCODE", args.getPack());

//                if (Res == 1)
//                {
//                    XtraMessageBox.Show("Update data succeeded ", " ");
//                }

//        }


//        private void gridControl1_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
//        {

//            if (e.Button.ButtonType == NavigatorButtonType.Append)
//            {


//                //DevExpress.XtraGrid.Columns.GridColumn[] gridColumns = gridView1.Columns.ToArray();

//                /*List<RepositoryItemCollection> itemCollections = new List<RepositoryItemCollection>();//RepositoryItems添加集合

//                itemCollections.Add(gridControl1.RepositoryItems);*/

//                //ts[9].ColumnEdit.GetType().Name.ToString();//获取repository类型

//                /*List<DevExpress.XtraGrid.Columns.GridColumn> ts = gridView1.Columns.ToList();
//                //ts[9].ColumnEditName.ToString();

//                ts.Remove(ts[0]);

//                FrmAdd frmAdd = new FrmAdd(ts);
//                frmAdd.StartPosition = FormStartPosition.CenterParent;
//                frmAdd.gridtable += new gridtable(FrmAdd_gridtable);
//                frmAdd.ShowDialog();

//                if (frmAdd.IsDisposed)
//                {

//                    if (!string.IsNullOrEmpty(args.ROWID))
//                    {
//                        int Res = bs.ExecuteModify("NewTCODE", args.getPack());

//                        if (Res == 1)
//                        {
//                            XtraMessageBox.Show("Adding data succeeded ", " ");
//                        }
//                    }
//                    tbnSeach_Click(null, null);
//                }
//                else {
//                    return;
//                }*/

//            }
//            else if (e.Button.ButtonType == NavigatorButtonType.Remove) {

//                DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
//                args.ROWID = dataRow["RID"].ToString();
//                //gridView1.DeleteRow(gridView1.FocusedRowHandle);

//                int Res = bs.ExecuteModify("DelteParameterSpec", args.getPack());

//                if (Res == 1)
//                {
//                    XtraMessageBox.Show("Delete data succeeded ", " ");
//                }
//                else
//                {
//                    XtraMessageBox.Show("Delete data failed ", " ");
//                }

//            }
//            else if (e.Button.ButtonType == NavigatorButtonType.EndEdit)
//            {
//                //DataRowView dataRow1 = (DataRowView)gridView1.GetFocusedRow(); 
//                DataRow dataRow = gridView1.GetFocusedDataRow();
//                //DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);

//                args.DBID = DATABASETable.Select(String.Format("DBNAME = '{0}'", dataRow["DBNAME"].ToString()))[0]["DBID"].ToString();
//                args.PARAMETERID = ParameterTable.Select(String.Format("PARAMETERNAME = '{0}'", dataRow["PARAMETERNAME"].ToString()))[0]["PARAMETERID"].ToString();
//                args.PARAMETERNAME = dataRow["PARAMETERNAME"].ToString();
//                args.RULENAME = dataRow["Rule Space Name"].ToString();
//                args.RULENO = dataRow["RULENO"].ToString();
//                args.DAYS = dataRow["DAYS"].ToString();
//                args.SPECUPPERLIMIT = dataRow["SPECUPPERLIMIT"].ToString();
//                args.SPECLOWERLIMIT = dataRow["SPECLOWERLIMIT"].ToString();
//                args.CONTROLUPPERLIMIT = dataRow["CONTROLUPPERLIMIT"].ToString();
//                args.CONTROLLOWERLIMIT = dataRow["CONTROLLOWERLIMIT"].ToString();
//                args.CHARTUSED = dataRow["CHARTUSED"].ToString();
//                args.MAILUSED = dataRow["MAILUSED"].ToString();
//                args.MMSUSED = dataRow["MMSUSED"].ToString();
//                args.SPECLIMITUSED = dataRow["SPECLIMITUSED"].ToString();
//                args.ISALIVE = dataRow["ISALIVE"].ToString();
//                args.ROWID = dataRow["RID"].ToString();

//                if (string.IsNullOrEmpty(dataRow["RID"].ToString()))
//                {

//                    int Res = bs.ExecuteModify("NewParameterSpec", args.getPack());

//                    if (Res == 1)
//                    {
//                        XtraMessageBox.Show("Add Data Succeeded ", " ");
//                    }
//                    else
//                    {
//                        return;
//                    }

//                    tbnSeach_Click(null, null);

//                }
//                else
//                {
//                    int Res = bs.ExecuteModify("UpdateParameterSpec", args.getPack());

//                    if (Res == 1)
//                    {
//                        XtraMessageBox.Show("Update Data Succeeded ", " ");
//                    }
//                    else
//                    {
//                        return;
//                    }

//                    tbnSeach_Click(null, null);

//                }


//            }

//        }

//        private void FrmAdd_gridtable(DataTable dataTable)
//        {
//            /*args.CATEGORY = dataTable.Rows[0]["CATEGORY"].ToString();
//            args.SUBCATEGORY = dataTable.Rows[0]["SUBCATEGORY"].ToString();
//            args.NAME = dataTable.Rows[0]["NAME"].ToString();
//            args.USED = dataTable.Rows[0]["USED"].ToString();
//            args.CUSTOM01 = dataTable.Rows[0]["CUSTOM01"].ToString();
//            args.CUSTOM02 = dataTable.Rows[0]["CUSTOM02"].ToString();
//            args.CUSTOM03 = dataTable.Rows[0]["CUSTOM03"].ToString();
//            args.CUSTOM04 = dataTable.Rows[0]["CUSTOM04"].ToString();
//            args.CUSTOM05 = dataTable.Rows[0]["CUSTOM05"].ToString();
//            args.CUSTOM06 = dataTable.Rows[0]["CUSTOM06"].ToString();
//            args.CUSTOM07 = dataTable.Rows[0]["CUSTOM07"].ToString();*/


//            /*int Res = bs.ExecuteModify("NewTCODE", args.getPack());

//            if (Res == 1)
//            {
//                XtraMessageBox.Show("Adding data succeeded ", " ");
//            }*/

//        }

    }
}
