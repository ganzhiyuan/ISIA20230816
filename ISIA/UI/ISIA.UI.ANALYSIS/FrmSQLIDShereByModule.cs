﻿using DevExpress.XtraGrid.Columns;
using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.UI;

namespace ISIA.UI.ANALYSIS
{
    public partial class FrmSQLIDShereByModule : DockUIBase1T1
    {

        AwrCommonArgsPack args = new AwrCommonArgsPack();
        BizDataClient bs;
        string[] columStr = new string[] {"MODULE", "SQL_ID" };

        public FrmSQLIDShereByModule()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.SnapTrendChart1");
            dateStart.DateTime = DateTime.Now.AddDays(-1);
            dateEnd.DateTime = DateTime.Now;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataSet LoadData()
        {
            try
            {
                args.StartTimeKey = dateStart.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                args.EndTimeKey = dateEnd.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                args.CommandName = cboModules.Text;
                if (args.CommandName.Contains("空"))
                {
                    args.CommandName=args.CommandName.Count()>1? args.CommandName.Remove(0,2).Trim(): args.CommandName.Remove(0, 1).Trim();
                    args.ChartUsed = "1";
                }
                else
                {
                    args.ChartUsed = string.Empty;
                }
                //args.CommandType = cboParaName.Text;
                args.DbName = string.IsNullOrEmpty(cmbDbName.Text)?"": cmbDbName.Text.Split('(')[0];
                DataSet dataSet = bs.ExecuteDataSet("GetSqlStatModuleAll", args.getPack());
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DisplayData(DataSet dataSet)
        {
            if (dataSet == null)
            {
                return;
            }
            gridView1.Columns.Clear();
            gridControl1.DataSource = null;
            gridControl1.DataSource = dataSet.Tables[0];
            gridView1.BestFitColumns();
            SetGridViewStyle();

        }

        private void SetGridViewStyle()
        {
            string[] s = cboParaName.Text.Replace(", ", ",").Split(',');
            if (string.IsNullOrEmpty(s[0]))
            {
                foreach (GridColumn item in gridView1.Columns)
                {
                    gridView1.Columns.ColumnByFieldName(item.FieldName).Visible = true;
                }
                return;
            }
            foreach (GridColumn item in gridView1.Columns)
            {
                if (!s.Contains(item.FieldName) && !columStr.Contains(item.FieldName))
                {
                    gridView1.Columns.ColumnByFieldName(item.FieldName).Visible = false;
                }
            }
        }

        private void gridView1_CellMerge(object sender, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            if (e.Column.FieldName == "MODULE")
            {
                e.Merge = true;
            }
        }
    }
}
