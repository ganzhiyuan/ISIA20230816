using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ISIA.COMMON;
using TAP.Data.Client;

namespace ISIA.UI.MANAGEMENT
{

    public delegate void gridtable(DataTable dataTable);//定义委托

    public partial class FrmAdd : XtraForm
    {
        public event gridtable gridtable;//定义委托事件
        
        
        List<DevExpress.XtraGrid.Columns.GridColumn> gridColumns = new List<DevExpress.XtraGrid.Columns.GridColumn>();

        public FrmAdd()
        {
            InitializeComponent();
            
        }

        public FrmAdd(List<DevExpress.XtraGrid.Columns.GridColumn> gridColumn) 
        {
            InitializeComponent();
            
            gridColumns = gridColumn;
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Name",typeof(System.String));
            dataTable.Columns.Add("Value", typeof(System.String));
            for (int i = 0; i < gridColumn.Count; i++)
            {

                dataTable.Rows.Add(gridColumn[i].FieldName.ToString(),"");

            }
            
            gridView.GridControl.DataSource = dataTable;
            gridView.Columns["Name"].OptionsColumn.AllowEdit = false;
            gridView.Columns["Name"].OptionsColumn.ReadOnly = true;
            gridView.OptionsBehavior.Editable = true;
            
            //

            gridView.CustomRowCellEdit += GridView_CustomRowCellEdit;

        }

        private void GridView_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            

            //判断原数据的ColumnEditName是否有值，有值则获取原字段绑定的RepositoryItems的控件类型
            //传入helper类中处理返回一个控件
            if (e.Column.FieldName == "Value" && !string.IsNullOrEmpty(gridColumns[e.RowHandle].ColumnEditName.ToString()))
            {
                
                    string repo = gridColumns[e.RowHandle].ColumnEdit.GetType().Name.ToString();

                    RepHelper model = new RepHelper();

                    e.RepositoryItem = model.GetRepItem(repo, gridColumns[e.RowHandle].ColumnEditName.ToString());
                

            }


            /*RepositoryItemComboBox edit = new RepositoryItemComboBox();

            gridView.GridControl.RepositoryItems.Add(edit);
            if (e.Column.FieldName == "Value" && e.RowHandle == 8)
            {
                e.RepositoryItem = edit;
            }*/
        }

        private void windowsUIButtonPanel_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {

            
            if (e.Button.Properties.Caption.ToString() == "OK")
            {
                DataView dv = (DataView)gridView.DataSource;
                DataTable dataTable =  dv.ToTable();
                //DataView行转列
                DataTable dataTable1 = new DataTable();
                dataTable1.Rows.Add();
                foreach (DataRow row in dataTable.Rows)
                {
                    dataTable1.Columns.Add(row["Name"].ToString());
                    
                    dataTable1.Rows[0][row["Name"].ToString()] = row["Value"].ToString();
                }
                //


                gridtable(dataTable1);//调用委托

                /*args.CATEGORY = dataTable1.Rows[0]["CATEGORY"].ToString();
                args.SUBCATEGORY = dataTable1.Rows[0]["SUBCATEGORY"].ToString();
                args.NAME = dataTable1.Rows[0]["NAME"].ToString();
                args.USED = dataTable1.Rows[0]["USED"].ToString();
                args.CUSTOM01 = dataTable1.Rows[0]["CUSTOM01"].ToString();
                args.CUSTOM02 = dataTable1.Rows[0]["CUSTOM02"].ToString();
                args.CUSTOM03 = dataTable1.Rows[0]["CUSTOM03"].ToString();
                args.CUSTOM04 = dataTable1.Rows[0]["CUSTOM04"].ToString();
                args.CUSTOM05 = dataTable1.Rows[0]["CUSTOM05"].ToString();
                args.CUSTOM06 = dataTable1.Rows[0]["CUSTOM06"].ToString();
                args.CUSTOM07 = dataTable1.Rows[0]["CUSTOM07"].ToString();


                int Res = bs.ExecuteModify("NewTCODE", args.getPack());

                if (Res == 1)
                {
                    XtraMessageBox.Show("Adding data succeeded ", " ");
                }*/
                
                this.Dispose();
            }
            else if (e.Button.Properties.Caption.ToString() == "Cancel"){
                this.Dispose();
            }
        }
    }
}
