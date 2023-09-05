﻿using DevExpress.PerformanceTests.PerfomanceMonitor;
using DevExpress.XtraEditors;
using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.UI;

namespace ISIA.UI.MANAGEMENT
{

    
    public partial class FrmDataTableManagement : DockUIBase1T1
    {
        #region Feild
        BizDataClient bs = null;
        AwrArgsPack args = null;

        DataSet ds = new DataSet();
        DataSet dsPartition = new DataSet();

        #endregion
        public FrmDataTableManagement()
        {
            InitializeComponent();
            args = new AwrArgsPack();
            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.CreateDataTableManagement");
            
        }

        public void closeSqlPar() {
            showPartition();
        }

        private void FrmDataTableManagement_Load(object sender, EventArgs e)
        {
            base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
        }

        public DataSet LoadData()
        {
            ds = bs.ExecuteDataSet("GetDataTable");

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
            gridTableName.DataSource = ds.Tables[0];
            gridViewTableName.Columns[0].Width = 20;
            gridViewTableName.Columns[1].BestFit();

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
        }

        private void txtSave_Click(object sender, EventArgs e)
        {
           
            

            TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.WARNING, "Success.");
            btnSelect_Click(null, null);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
        }

        private void btnDel_Click(object sender, EventArgs e)
        {

            if (gridViewPartition.DataRowCount <= 0 )
            {

                TAP.UI.TAPMsgBox.Instance.ShowMessage("Error" ,EnumMsgType.WARNING, "Please select partition!");
                return;
            }
            else
            {
                FrmDropPartitionin frmDropPartitionin = new FrmDropPartitionin();
                frmDropPartitionin.partitionName = gridViewPartition.GetFocusedDataRow()[0].ToString();
                frmDropPartitionin.objectName = args.TABLENAME;
                frmDropPartitionin.MyEvent += closeSqlPar;
                frmDropPartitionin.StartPosition = FormStartPosition.CenterScreen;
                frmDropPartitionin.Show(this);
            }

        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
           
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
            FrmAddPartitionin frmAdd = new FrmAddPartitionin();

            DateTime dateTime = DateTime.Now.Date;
            DateTime dateTimeNow = new DateTime(dateTime.Year, dateTime.Month, 1);
            int year = dateTimeNow.Year;
            int month = dateTimeNow.Month;
            string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
            if (dsPartition.Tables[0].Rows.Count > 0 )
            {
                string tname = args.TABLENAME;
                string[] parts = tname.Split('_');
                string result = string.Join("_", parts.Skip(3));
                result = result + "_{0}";
                frmAdd.partitionName = string.Format( result, res);
            }
            else
            {
                string parName = "ACTIVE_SESS_HISTORY_ _{0}";
                frmAdd.partitionName = string.Format(parName, res);
            }
            string upperBound = "TO_DATE('{0}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN')";
            frmAdd.upperBound = string.Format(upperBound, dateTimeNow);
            frmAdd.objectName = args.TABLENAME;
            frmAdd.StartPosition = FormStartPosition.CenterScreen;
            frmAdd.MyEvent += closeSqlPar;
            frmAdd.Show(this);
            
        }

        private void gridViewTableName_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            showPartition();
        }

        private void showPartition()
        {

            DataRow rowTable = gridViewTableName.GetFocusedDataRow();

            string tableName = rowTable["TABLE_NAME"].ToString();

            args.TABLENAME = tableName;

            DataSet dsPartitioning = bs.ExecuteDataSet("getPartitioning", args.getPack());

            gridPartitioning.DataSource = dsPartitioning.Tables[0];

            gridViewPartitioning.BestFitColumns();

            dsPartition = bs.ExecuteDataSet("getPartition", args.getPack());

            gridPartition.DataSource = dsPartition.Tables[0];

            gridViewPartition.BestFitColumns();
        }

    }
}
