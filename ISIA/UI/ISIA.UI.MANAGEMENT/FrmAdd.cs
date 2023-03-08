using DevExpress.XtraBars;
using DevExpress.XtraEditors;
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
using TAP.Data.Client;

namespace ISIA.UI.MANAGEMENT
{
    public partial class FrmAdd : XtraForm
    {

        DataBaseManagementArgsPack args = new DataBaseManagementArgsPack();
        BizDataClient bs = null;
        public FrmAdd()
        {
            InitializeComponent();
            
        }

        public FrmAdd(List<DevExpress.XtraGrid.Columns.GridColumn> gridColumn) 
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.DataBaseManagement");
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Name",typeof(System.String));
            dataTable.Columns.Add("Value", typeof(System.String));
            for (int i = 0; i < gridColumn.Count; i++)
            {

                dataTable.Rows.Add(gridColumn[i].FieldName.ToString(),"");

            }
            
            gridView.GridControl.DataSource = dataTable;
            gridView.Columns["Name"].OptionsColumn.AllowEdit = false;
            gridView.OptionsBehavior.Editable = true;

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


                args.CATEGORY = dataTable1.Rows[0]["CATEGORY"].ToString();
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
                }
                /*string a = "aa";
                XtraMessageBox.Show("add date ok");*/
                this.Dispose();
            }
            else if (e.Button.Properties.Caption.ToString() == "Cancel"){
                this.Dispose();
            }
        }
    }
}
