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
using System.Collections;

namespace ISIA.UI.MANAGEMENT
{
    public partial class FrmDataBaseManagement : DockUIBase1T1
    {
        public FrmDataBaseManagement()
        {
            InitializeComponent();
            InitializeComboBox();
            /*tDateTimePickerSE1.StartDate = DateTime.ParseExact(DateTime.Now.AddYears(-1).ToString("yyyy"), "yyyy", System.Globalization.CultureInfo.CurrentCulture);
            tDateTimePickerSE1.EndDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy"), "yyyy", System.Globalization.CultureInfo.CurrentCulture);
            */
            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.DataBaseManagement");

        }


        #region Field


        //修改数据的行数索引保存
  
        BizDataClient bs = null;
        #endregion

        #region Event

        //Filter删选条件
        private void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            txtFilter_Data();
        }
        private void txtFilter_Data()
        {
            if (chkFilter.Checked)
            {
                string filertxt = txtFilter.Text;
                this.cboPrmt.SetFilter(TAP.UIControls.EnumFilterType.Include, filertxt.Split(','));
                this.cbooperdesc.SetFilter(TAP.UIControls.EnumFilterType.Include, filertxt.Split(','));
            }
            else
            {
                this.cboPrmt.SetFilter(TAP.UIControls.EnumFilterType.Include, null);
                this.cbooperdesc.SetFilter(TAP.UIControls.EnumFilterType.Include, null);
            }
        }
        private void txtFilter_EditValueChanged(object sender, EventArgs e)
        {
            if (chkFilter.Checked)
            {
                string filertxt = txtFilter.Text;
                this.cboPrmt.SetFilter(TAP.UIControls.EnumFilterType.Include, filertxt.Split(','));
                this.cbooperdesc.SetFilter(TAP.UIControls.EnumFilterType.Include, filertxt.Split(','));
            }
        }

        //焦点变色
        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //focusedRowHandles.Add(this.gridView1.FocusedRowHandle);

            this.gridView1.RowCellStyle += gridView1_RowCellStyle;
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.CancelAsync();
        }

        #endregion

        #region Method

        public override void ExecuteCommand(ArgumentPack arguments)
        {
            Hashtable hashtable;
            foreach (string tmpstr in arguments.ArgumentNames)
            {
                if (tmpstr == "hashtable")
                {
                    hashtable = (Hashtable)arguments["hashtable"].ArgumentValue;
                    /*if (hashtable["FACILITY"] != null)
                    {
                        ComboBoxControl.SelectedComboBox(cboFab, hashtable["FACILITY"].ToString());
                    }
                    if (hashtable["TECH"] != null)
                    {
                        ComboBoxControl.SelectedComboBox(cboTech, hashtable["TECH"].ToString());
                    }
                    if (hashtable["LOTCODE"] != null)
                    {
                        ComboBoxControl.SelectedComboBox(cboLotcode, hashtable["LOTCODE"].ToString());
                    }
                    if (hashtable["OPERATION"] != null)
                    {
                        ComboBoxControl.SelectedComboBox(cboOper, hashtable["OPERATION"].ToString());
                    }
                    if (hashtable["PRODUCT"] != null)
                    {
                        ComboBoxControl.SelectedComboBox(cboProd, hashtable["PRODUCT"].ToString());
                    }*/

                }
                if (tmpstr == "LinkTable")
                {
                    DataTable dt = (DataTable)arguments["LinkTable"].ArgumentValue;
                    //ComboBoxControl.AfterComboBoxLinkValue(dt, this.tabPane1);
                }

            }
            btnSelect_Click(null, null);
        }

        //添加 Add
        private void btnSave_Click(object sender, EventArgs e)
        {
            /*ComboBoxControl.SetCrossLang(this._translator);
            if (!ComboBoxControl.CheckComboBoxValue(this.Text, this.AddOptionControls)) return;
            IxMessageBox im = new IxMessageBox(this._translator);
            DialogResult dialog = im.IsInsert(this.Text);
            if (dialog.ToString() == "Yes")
            {
                try
                {
                    string fab = cboFab2.Text;
                    string tech = cboTech2.Text;
                    string lotch = cboLot2.Text;
                    string prod = cboProd2.Text;

                    var operArray = cboOper2.Text.Split(',').AsEnumerable().Select(x => x.Split('-')).Select(x => x.FirstOrDefault().Trim());
                    string oper = string.Join(",", operArray);

                    string area = cboArea2.Text;
                    string prmt = cboPrmt2.Text;
                    string operdesc = txtOperDesc2.Text;
                    string specUpper = txtSpecUpper2.Text;
                    string specLpw = txtSpecLower2.Text;
                    string ctrlUpper = txtCtrlUpper2.Text;
                    string ctrlLower = txtCtrlLower2.Text;
                    string chartyn = cbochartyn2.Text;
                    args.FabId = fab;
                    args.TechId = tech;
                    args.LotCd = lotch;
                    args.ProdId = prod;
                    args.OperId = oper;
                    args.AreaCd = area;
                    args.PrmtNm = prmt;
                    args.OperDesc = operdesc;
                    args.SpecUpperLimit = specUpper;
                    args.SpecLowerLimit = specLpw;
                    args.CtrlUpperLimit = ctrlUpper;
                    args.CtrlLowerLimit = ctrlLower;
                    args.ChartYN = chartyn;

                    int ds = bs.ExecuteModify("SaveSpec", args.getPack());
                    base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
                }
                catch (System.Exception ex)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
                }
            }
            else
            {
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
                return;
            }*/
        }

        //Select 查询
        private void btnSelect_Click(object sender, EventArgs e)
        {
            //ComboBoxControl.SetCrossLang(this._translator);
            // if (!ComboBoxControl.ComboBoxCheckValue(this.Text, this.OptionControls)) return;
            base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);

        }

        //LoadData 查询数据返回dataset
        public DataSet LoadData()
        {
            string fab = cboFab.Text;
            string tech = cboTech.Text;
            string lotch = cboLotcode.Text;
            string prod = cboProd.Text;

            var operArray = cboOper.Text.Split(',').AsEnumerable().Select(x => x.Split('-')).Select(x => x.FirstOrDefault().Trim());
            string oper = string.Join(",", operArray);


            //string oper = cboOper.Text;
            string operdesc = cbooperdesc.Text;
            string prmt = cboPrmt.Text;


            /*args.FabId = fab;
            args.TechId = tech;
            args.LotCd = lotch;
            args.ProdId = prod;
            args.OperId = oper;
            args.OperDesc = operdesc;
            args.PrmtNm = prmt;*/
            DataSet ds = bs.ExecuteDataSet("GetSpec");

            return ds;
        }

        //DisplayData  gridview显示数据
        public void DisplayData(DataSet dataSet)
        {
            if (dataSet == null)
            {
                return;
            }
            gridControl1.DataSource = dataSet.Tables[0];
            gridViewStyle(gridView1);

        }

        //Style样式
        public void gridViewStyle(DevExpress.XtraGrid.Views.Grid.GridView gridView)
        {
            //显示复选框        
            gridView.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
            gridView.OptionsSelection.MultiSelect = true;
            gridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            gridView.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
            //设置单元格可以编辑
            gridView.OptionsBehavior.Editable = true;
            //显示滚动条
            gridView.OptionsView.ColumnAutoWidth = false;
            //列表宽度自适应内容
            gridView.BestFitColumns();
            //让各列头禁止移动
            gridView.OptionsCustomization.AllowColumnMoving = false;
            //让各列头禁止排序
            gridView.OptionsCustomization.AllowSort = false;
            //禁止各列头改变列宽
            gridView.OptionsCustomization.AllowColumnResizing = false;

        }

        //Delete 删除
        private void btnDelete_Click(object sender, EventArgs e)
        {
            /*if (gridView1.SelectedRowsCount <= 0)
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Please choose one line");
                return;
            }
            IxMessageBox im = new IxMessageBox(this._translator);
            DialogResult dialog = im.IsDelete(this.Text);
            if (dialog.ToString() == "Yes")
            {
                int DeleteCount = 0;
                List<int> selectRowNum = new List<int>();
                try
                {
                    foreach (int rowhandel in gridView1.GetSelectedRows())
                    {
                        selectRowNum.Add(rowhandel);
                    }

                    for (int i = 0; i < selectRowNum.Count; i++)
                    {
                        DataRow tmpRow = gridView1.GetDataRow(selectRowNum[i]); ;
                        string tmpFab = tmpRow["FAB"].ToString();
                        string tmpTech = tmpRow["TECH"].ToString();
                        string tmpLot = tmpRow["LOT_CD"].ToString();
                        string tmpProd = tmpRow["PROD"].ToString();
                        string tmpOper = tmpRow["OPER"].ToString();
                        string tmpPRMT = tmpRow["PRMT_ID"].ToString();
                        string tmpArea = tmpRow["AREA_CD"].ToString();
                        args.FabId = tmpFab;
                        args.TechId = tmpTech;
                        args.LotCd = tmpLot;
                        args.ProdId = tmpProd;
                        args.OperId = tmpOper;
                        args.PrmtNm = tmpPRMT;
                        args.AreaCd = tmpArea;
                        DeleteCount = bs.ExecuteModify("DeleteSpec", args.getPack());
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
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
                return;
            }*/
        }

        //Update 修改
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            /*IxMessageBox im = new IxMessageBox(this._translator);
            DialogResult dialog = im.IsUpdate(this.Text);
            if (dialog.ToString() == "Yes")
            {
                int UpdateCount = 0;
                try
                {
                    if (focusedRowHandles.Count <= 0)
                        return;
                    for (int i = 0; i < focusedRowHandles.Count; i++)
                    {
                        DataRow tmpRow = gridView1.GetDataRow(focusedRowHandles[i]);

                        string tmpFab = tmpRow["FAB"].ToString();
                        string tmpTech = tmpRow["TECH"].ToString();
                        string tmpLot = tmpRow["LOT_CD"].ToString();
                        string tmpProd = tmpRow["PROD"].ToString();
                        string tmpOper = tmpRow["OPER"].ToString();
                        string tmpPRMT = tmpRow["PRMT_ID"].ToString();
                        string tmpArea = tmpRow["AREA_CD"].ToString();
                        string tmpOperdesc = tmpRow["OPER_DESC"].ToString();
                        string tmpSpecUpper = tmpRow["SPEC_UPPER_LIMIT"].ToString();
                        string tmpSpecLpw = tmpRow["SPEC_LOWER_LIMIT"].ToString();
                        string tmpCtrlUpper = tmpRow["CTRL_UPPER_LIMIT"].ToString();
                        string tmpCtrlLower = tmpRow["CTRL_LOWER_LIMIT"].ToString();
                        string tmpUpdateDT = tmpRow["UPDATE_DT_TM"].ToString();
                        string tmpUpdateUserID = tmpRow["UPDATE_USER_ID"].ToString();
                        string tmpChartyn = tmpRow["CHART_YN"].ToString();
                        args.FabId = tmpFab;
                        args.TechId = tmpTech;
                        args.LotCd = tmpLot;
                        args.ProdId = tmpProd;
                        args.OperId = tmpOper;
                        args.AreaCd = tmpArea;
                        args.PrmtNm = tmpPRMT; ;
                        args.OperDesc = tmpOperdesc;
                        args.SpecUpperLimit = tmpSpecUpper;
                        args.SpecLowerLimit = tmpSpecLpw;
                        args.CtrlUpperLimit = tmpCtrlUpper;
                        args.CtrlLowerLimit = tmpCtrlLower;
                        args.UpdateDt = tmpUpdateDT;
                        args.UpdateUserId = tmpUpdateUserID;
                        args.ChartYN = tmpChartyn;
                        UpdateCount = bs.ExecuteModify("UpdateSpec", args.getPack());
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
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
                return;
            }*/
        }

        #endregion


        #region InitializeComboBox
        private void InitializeComboBox()
        {
            try
            {

                InitializeComboBoxChartYN2();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeComboBoxChartYN2()
        {
/*
            DataTable dataTable = new DataTable();
            DataColumn dc = dataTable.Columns.Add("cbochartyn2", Type.GetType("System.String"));
            List<string> Colummns = new List<string>();
            Colummns.Add("");
            Colummns.Add("Y");
            Colummns.Add("N");

            for (int i = 0; i < Colummns.Count; i++)
            {
                DataRow dr = dataTable.NewRow();
                dr["cbochartyn2"] = Colummns[i].ToString();
                dataTable.Rows.Add(dr);
            }
            cbochartyn2.BindData(dataTable, false);*/

        }

        #endregion

        private void btnImport_Click(object sender, EventArgs e)
        {
            
            /*OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Excel";
            fileDialog.Filter = "Excel(*.xls)|*.xls|Excel2017(*.xlsx)|*.xlsx";
            DialogResult dialogResult = fileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    string fileName = fileDialog.FileName;
                    DataTable dtSpec = ExcelToDataTable(fileName);

                    int saveCount = 0;
                    FDCArgsPack specArgs = new FDCArgsPack();
                    List<FDCArgsPack> specArgsPacks = new List<FDCArgsPack>();
                    foreach (DataRow tmpRow in dtSpec.Rows)
                    {
                        FDCArgsPack specArg = new FDCArgsPack();

                        string tmpFab = tmpRow["FAB"].ToString();
                        string tmpTech = tmpRow["TECH"].ToString();
                        string tmpLot = tmpRow["LOT_CD"].ToString();
                        string tmpProd = tmpRow["PROD"].ToString();
                        string tmpOper = tmpRow["OPER"].ToString();
                        string tmpPRMT = tmpRow["PRMT_ID"].ToString();
                        string tmpArea = tmpRow["AREA_CD"].ToString();
                        string tmpOperdesc = tmpRow["OPER_DESC"].ToString();
                        string tmpSpecUpper = tmpRow["SPEC_UPPER_LIMIT"].ToString();
                        string tmpSpecLpw = tmpRow["SPEC_LOWER_LIMIT"].ToString();
                        string tmpCtrlUpper = tmpRow["CTRL_UPPER_LIMIT"].ToString();
                        string tmpCtrlLower = tmpRow["CTRL_LOWER_LIMIT"].ToString();
                        string tmpUpdateDT = tmpRow["UPDATE_DT_TM"].ToString();
                        string tmpUpdateUserID = tmpRow["UPDATE_USER_ID"].ToString();
                        string tmpChartyn = tmpRow["CHART_YN"].ToString();
                        specArg.FabId = tmpFab;
                        specArg.TechId = tmpTech;
                        specArg.LotCd = tmpLot;
                        specArg.ProdId = tmpProd;
                        specArg.OperId = tmpOper;
                        specArg.AreaCd = tmpArea;
                        specArg.PrmtNm = tmpPRMT; ;
                        specArg.OperDesc = tmpOperdesc;
                        specArg.SpecUpperLimit = tmpSpecUpper;
                        specArg.SpecLowerLimit = tmpSpecLpw;
                        specArg.CtrlUpperLimit = tmpCtrlUpper;
                        specArg.CtrlLowerLimit = tmpCtrlLower;
                        specArg.UpdateDt = tmpUpdateDT;
                        specArg.UpdateUserId = tmpUpdateUserID;
                        specArg.ChartYN = tmpChartyn;

                        specArgsPacks.Add(specArg);
                    }

                    specArgs.SPECArgsPacks = specArgsPacks;

                    saveCount = bs.ExecuteModify("SaveSpecBatch", specArgs.getPack());
                    if (saveCount > 0)
                    {
                        TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.INFORMATION, "Total " + saveCount.ToString() + " Imported");
                    }

                }
                catch (System.Exception ex)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
                }
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            }*/
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable tmpDataTable = (DataTable)this.gridControl1.DataSource;
            if (tmpDataTable != null && tmpDataTable.Rows.Count > 0)
            {
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Title = "Excel";
                fileDialog.Filter = "Excel(*.xls)|*.xls|Excel2017(*.xlsx)|*.xlsx";
                DialogResult dialogResult = fileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    if (fileDialog.FilterIndex.Equals("1"))
                    {
                        gridControl1.ExportToXls(fileDialog.FileName);
                    }
                    else
                    {
                        gridControl1.ExportToXlsx(fileDialog.FileName);
                    }
                }
            }
            else
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.WARNING, "No Data!");
            }
        }


       /* public DataTable ExcelToDataTable(string fileName)
        {
           *//* DataTable dt = new DataTable();
            Excel.Application xlap = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook wb = null;
            xlap.Visible = false;
            try
            {
                wb = xlap.Workbooks.Open(fileName);
                Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
                int rowCount = ws.UsedRange.Cells.Rows.Count;
                int colCount = ws.UsedRange.Cells.Columns.Count;
                Range excelCell = ws.UsedRange;
                Object[,] values = (Object[,])excelCell.Value2;
                int intRows = values.GetLength(0);
                if (intRows != 0)
                {
                    int intCols = values.GetLength(1);
                    for (int i = 1; i <= intCols; i++)
                    {
                        dt.Columns.Add(new DataColumn((string)values[1, i]));
                    }
                    for (int i = 2; i <= intRows; i++)
                    {
                        DataRow dr = dt.NewRow();
                        for (int j = 1; j < colCount; j++)
                        {
                            dr[(String)values[1, j]] = values[i, j];
                        }
                        dt.Rows.Add(dr);
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                wb.Close();
                Utils.Kill(xlap);
            }*//*
        }*/
    }
}
