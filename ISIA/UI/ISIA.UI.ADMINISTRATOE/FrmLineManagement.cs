using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using TAP;
using TAP.UI;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using TAP.Models.Factories.Facilities;
using TAP.UIControls.BasicControlsDEV;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.IO;
using ISIA.UI.BASE;
using Spire.Pdf.Annotations;
using Spire.Pdf.Annotations.Appearance;
using EnumDataObject = TAP.UI.EnumDataObject;
using EnumMsgType = TAP.UI.EnumMsgType;

namespace ISIA.UI.ADMINISTRATOE
{
    public partial class FrmLineManagement : DockUIBase1T1
    {
        public FrmLineManagement()
        {
            InitializeComponent();
            InitializeComboBox();
        }

        #region Feild
        List<int> focusedRowHandles = new List<int>();

        ComboBoxControl ComboBoxControl = new ComboBoxControl();

        LineModel lineModel = new LineModel();
        ArgumentPack retVal = new ArgumentPack();
        FacilityDefaultInfo defaultInformation = new FacilityDefaultInfo();
        string region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
        PdfPageBase page;
        PdfDocument doc = new PdfDocument();
        string path;
        System.Drawing.Image qrCode;
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
            string strFab = GetchkcomboxEditValue(cboFab);

            DataTable dt = lineModel.LoadModelDataList(retVal);

            string sqlWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(strFab))
            {
                string fab = string.Format(" AND FACILITY IN({0}) ", strFab);
                sqlWhere = sqlWhere + fab;
            }

            DataRow[] drs = dt.Select(sqlWhere);
            DataTable dtNew = dt.Clone();
            foreach (DataRow dr in drs)
            {
                dtNew.ImportRow(dr);
            }
            //gridControl1.DataSource = dtNew;
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dtNew);

            return dataSet;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                ComboBoxControl.SetCrossLang(this._translator);

                if (!base.ValidateUserInput(this.layoutControl1)) return;

                //LoadModelData();
                //GridViewStyle();
                base.BeginAsyncCall("LoadModelData", "DisplayData", EnumDataObject.DATASET);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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

            //gridView1.OptionsCustomization.AllowColumnMoving = false;
            //gridView1.OptionsCustomization.AllowSort = false;
            //gridView1.OptionsCustomization.AllowColumnResizing = false;

            gridView1.Columns["REGION"].Visible = false;
            gridView1.Columns["NAME"].Caption = "WORKSHOP";
            gridView1.Columns["FACILITY"].Caption = "BUILDING";

            gridView1.Columns["SEQUENCES"].Visible = false;
            gridView1.Columns["ISALIVE"].Visible = false;
            gridView1.Columns["MODELLEVELS"].Visible = false;
            gridView1.Columns["INSERTTIME"].Visible = false;
            gridView1.Columns["UPDATETIME"].Visible = false;
            gridView1.Columns["INSERTUSER"].Visible = false;
            gridView1.Columns["UPDATEUSER"].Visible = false;

            gridView1.Columns["FACILITY"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["NAME"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTEVENTCOMMENT"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTEVENT"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTEVENTFLAG"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTEVENTTIME"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTEVENTCODE"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["LASTJOBCODE"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["CURRENTMODEL"].OptionsColumn.AllowEdit = false;
            gridView1.BestFitColumns();
            //for (int I = 0; I < gridView1.Columns.Count; I++)
            //{
            //    this.gridView1.BestFitColumns();
            //    this.gridView1.Columns[I].BestFit();//自动列宽
            //}
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ComboBoxControl.SetCrossLang(this._translator);
            if (!base.ValidateUserInput(this.layoutControl2)) return;

            if (string.IsNullOrEmpty(txtLine.Text))
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "You must input workshop.");
                txtLine.BackColor = Color.Orange;
                txtLine.Focus();
                return;
            }

            DialogResult dialog = ComboBoxControl.IsInsert(this.Text);
            if (dialog.ToString() == "Yes")
            {
                try
                {
                    int SaveCount = 0;

                    FacilityDefaultInfo facilityDefaultInfo = new FacilityDefaultInfo();
                    facilityDefaultInfo.Region = region;
                    facilityDefaultInfo.Facility = cboFab2.Text;
                    facilityDefaultInfo.Line= txtLine.Text;

                    lineModel = new LineModel(facilityDefaultInfo);
                    lineModel.Description = txtDescription.Text;
                    SaveCount=lineModel.Save(InfoBase._USER_INFO.UserName, "CREATE", TAP.Models.EnumEventFlag.D);

                    if (SaveCount > 0)
                    {
                        TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.INFORMATION, "Insert completed.");
                    }

                    InitializeComboBox();
                    txtDescription.Text = "";
                    cboFab2.Text = "";
                    txtLine.Text = "";
                    //DataTable dt = lineModel.LoadModelDataList(retVal);
                    //gridControl1.DataSource = dt;
                    gridControl1.DataSource = null;
                }
                catch (System.Exception ex)
                {
                    TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
                }
            }
            else
            {
                return;
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

                    for (int i = 0; i < selectRowNum.Count; i++)
                    {
                        DataRow tmpRow = gridView1.GetDataRow(selectRowNum[i]); ;
                        string Name = tmpRow["NAME"].ToString();
                        string Facility = tmpRow["FACILITY"].ToString();

                        FacilityDefaultInfo facilityDefaultInfo = new FacilityDefaultInfo();
                        facilityDefaultInfo.Region = region;
                        facilityDefaultInfo.Facility = Facility;
                        facilityDefaultInfo.Line = Name;

                        lineModel = new LineModel(facilityDefaultInfo);
                        lineModel.LastEvent = "DELETE";
                        lineModel.LastEventCode = "DELETE_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        lineModel.LastEventTime = DateTime.ParseExact(DateTime.Now.ToString("yyyyMMddHHmmss"), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        lineModel.LastJobCode = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        lineModel.Delete(InfoBase._USER_INFO.UserName);
                    }
                    //LoadModelData();    
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
                    for (int i = 0; i < focusedRowHandles.Count; i++)
                    {
                        DataRow tmpRow = gridView1.GetDataRow(focusedRowHandles[i]);

                        string NAME = tmpRow["NAME"].ToString();
                        string FACILITY = tmpRow["FACILITY"].ToString();
                        string WORKORDERNAME = tmpRow["WORKORDERNAME"].ToString();
                        string DESCRIPTION = tmpRow["DESCRIPTION"].ToString();
                        string LASTEVENTCOMMENT = tmpRow["LASTEVENTCOMMENT"].ToString();

                        FacilityDefaultInfo facilityDefaultInfo = new FacilityDefaultInfo();
                        facilityDefaultInfo.Region = region;
                        facilityDefaultInfo.Facility = FACILITY;
                        facilityDefaultInfo.Line = NAME;

                        lineModel = new LineModel(facilityDefaultInfo);
                        lineModel.Description = DESCRIPTION;
                        lineModel.LastEventComment = LASTEVENTCOMMENT;
                        lineModel.WorkOrderName = WORKORDERNAME;
                        lineModel.UpdateTime = DateTime.ParseExact(DateTime.Now.ToString("yyyyMMddHHmmss"), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        lineModel.LastEvent = "MODIFY";
                        lineModel.LastEventCode = "MODIFY_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        lineModel.LastEventTime = DateTime.ParseExact(DateTime.Now.ToString("yyyyMMddHHmmss"), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        lineModel.LastJobCode = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        lineModel.Save(InfoBase._USER_INFO.UserName);
                    }
                    focusedRowHandles.Clear();
                    //LoadModelData();
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
                this.cboFab.changeUpdateStatus(true);
                this.cboFab2.changeUpdateStatus(true);

                this.cboFab.Properties.Items.Clear();
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

        private string GetchkcomboxEditValue(CheckedComboBoxEdit checkedComboBoxEdit)
        {
            string[] sArray = checkedComboBoxEdit.Text.Replace(" ", "").Split(',');
            string strchkcbotext = "";
            for (int i = 0; i < sArray.Length; i++)
            {
                if (!(sArray[i] == null || sArray[i] == ""))
                {
                    strchkcbotext = strchkcbotext + "'" + sArray[i] + "',";
                }
            }
            if (strchkcbotext.Length > 0)
            {
                strchkcbotext = strchkcbotext.Substring(0, strchkcbotext.Length - 1);
            }
            return strchkcbotext;
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {
            if (gridControl1.DataSource == null) return;

            int[] rows = gridView1.GetSelectedRows();

            if (rows.Length == 0)
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "Please choose one line at least.");
                return;
            }

            try
            {
                doc = new PdfDocument();
                page = doc.Pages.Add();
                int rotation = (int)page.Rotation;
                rotation += (int)PdfPageRotateAngle.RotateAngle270;
                page.Rotation = (PdfPageRotateAngle)rotation;
                page.Canvas.TranslateTransform(400, 100);
                //旋转90度
                page.Canvas.RotateTransform(90);
                page.Canvas.DrawString("Line QR", new PdfFont(PdfFontFamily.Helvetica, 85f),
                                           new PdfSolidBrush(Color.Black), 130, 50);

                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Title = "PDF";
                fileDialog.Filter = "(*.pdf)|*.pdf";
                DialogResult dialogResult = fileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    if (rows.Length > 0)
                    {
                        foreach (int row in rows)
                        {
                            DataRow tmpRow = gridView1.GetDataRow(row);
                            string fab = tmpRow["FACILITY"].ToString();
                            string line = tmpRow["NAME"].ToString();
                            string area = "ALL";

                            string lineInfo = region + "_" + fab + "_" + line + "_" + area;
                            GetQRCode(lineInfo);
                            PdfImage image = PdfImage.FromImage(qrCode);

                            page = doc.Pages.Add();

                            rotation = (int)page.Rotation;
                            rotation += (int)PdfPageRotateAngle.RotateAngle270;
                            page.Rotation = (PdfPageRotateAngle)rotation;
                            page.Canvas.TranslateTransform(400, 100);
                            //旋转90度
                            page.Canvas.RotateTransform(90);

                            #region remove logo
                            PdfRubberStampAnnotation logoStamp = new PdfRubberStampAnnotation(new RectangleF(new PointF(378, 100), new SizeF(15, 350)));
                            PdfAppearance logoApprearance = new PdfAppearance(logoStamp);
                            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "white.PNG");

                            PdfImage backimage = PdfImage.FromFile(logoPath);
                            PdfTemplate template = new PdfTemplate(15, 350);
                            template.Graphics.DrawImage(backimage, 0, 0);

                            logoApprearance.Normal = template;
                            logoStamp.Appearance = logoApprearance;
                            page.AnnotationsWidget.Add(logoStamp);
                            #endregion


                            page.Canvas.DrawImage(image, new System.Drawing.Rectangle(-60, 8, 270, 270));

                            page.Canvas.DrawLine(PdfPens.Black, -70, 0, 640, 0);
                            page.Canvas.DrawLine(PdfPens.Black, -70, 270, 640, 270);
                            page.Canvas.DrawLine(PdfPens.Black, -70, 0, -70, 270);
                            page.Canvas.DrawLine(PdfPens.Black, 640, 0, 640, 270);

                            page.Canvas.DrawString("REGION : " + region, new PdfFont(PdfFontFamily.Helvetica, 30f),
                                               new PdfSolidBrush(Color.Black), 250, 50);

                            page.Canvas.DrawString("BUILDING : " + fab, new PdfFont(PdfFontFamily.Helvetica, 30f),
                                               new PdfSolidBrush(Color.Black), 250, 125);

                            page.Canvas.DrawString("WORKSHOP : " + line, new PdfFont(PdfFontFamily.Helvetica, 30f),
                                               new PdfSolidBrush(Color.Black), 250, 200);

                            //page.Canvas.DrawString("PROCESS : " + area, new PdfFont(PdfFontFamily.Helvetica, 30f),
                            //                   new PdfSolidBrush(Color.Black), 250, 200);
                        }
                        FileStream fs = new FileStream(fileDialog.FileName, FileMode.Create, FileAccess.Write);
                        doc.SaveToStream(fs);
                        fs.Close();
                        doc.Close();
                        TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.INFORMATION, "Export completed.");
                        base.BeginAsyncCall("LoadModelData", "DisplayData", EnumDataObject.DATASET);
                    }
                }
            }
            catch (Exception ex)
            {
                TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, ex.ToString());
            }
        }

        public void GetQRCode(string codeContent)
        {
            TAP.Base.QRCode.QRCodeEncoder QRCODE = new TAP.Base.QRCode.QRCodeEncoder();

            QRCODE.ErrorCorrection = TAP.Base.QRCode.ErrorCorrection.L;
            QRCODE.ModuleSize = 4;
            QRCODE.QuietZone = 16;
            //QRCODE.ECIAssignValue = 0;
            QRCODE.Encode(codeContent);

            path = System.IO.Directory.GetCurrentDirectory();
            qrCode = QRCODE.CreateQRCodeBitmap();
        }
    }
}