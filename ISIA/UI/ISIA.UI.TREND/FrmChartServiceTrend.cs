using DevExpress.Charts.Native;
using DevExpress.XtraEditors;
using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.UI;
using static DevExpress.XtraBars.Docking2010.Views.BaseRegistrator;

namespace ISIA.UI.TREND
{
    public partial class FrmChartServiceTrend : DockUIBase1T1
    {
        //#region Constant
        //#endregion

        //#region Field

        //List<Image> _imageList;
        //public delegate void DownLoadFileHandler(string[] fileNames);

        //public event DownLoadFileHandler DownloadCompleted;

        //private System.ComponentModel.BackgroundWorker bgDownload;
        //private ISIA.UI.COMMON.FrmProgress _frmProgress = new ISIA.UI.COMMON.FrmProgress();
        //private string[] _downLoadFileNames;
        //private bool _reserveData = false;
        //private int _cancelIndex = 0;
        //BizDataClient _bs = null;

        //#endregion

        //#region Property
        //#endregion

        //#region Create and Disposer
        //public FrmChartServiceTrend()
        //{
        //    InitializeComponent();
        //    InitializeBizDataClient();
        //    InitializeControl();
        //}
        //#endregion

        //#region Initialize
        //private void InitializeControl() { InitializeDate(); }

        //private void InitializeBackgroundWorker()
        //{
        //    this.bgDownload = new System.ComponentModel.BackgroundWorker();
        //    this.bgDownload.WorkerReportsProgress = true;
        //    this.bgDownload.WorkerSupportsCancellation = true;
        //    this.bgDownload.DoWork += new DoWorkEventHandler(this.bgDownload_DoWork);
        //    this.bgDownload.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bgDownload_RunWorkerCompleted);
        //    this.bgDownload.ProgressChanged += new ProgressChangedEventHandler(this.bgDownload_ProressChanged);
        //}

        //private void InitializeBizDataClient()
        //{
        //    try
        //    {
        //        _bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISFA.BIZ.TREND.ChartServiceTrend");
        //    }
        //    catch (Exception ex)
        //    {
        //        TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "InitializeBizDataClient()", ex.ToString());
        //    }
        //}

        //private void InitializeDate()
        //{
        //    try
        //    {
        //        this.dtpStartTime.DateTime = DateTime.Now;
        //    }
        //    catch (Exception ex)
        //    {
        //        TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "InitializeDate()", ex.ToString());
        //    }
        //}
        //#endregion

        //#region Method
        ///* Image Load 부분.
        //private DevExpress.XtraEditors.PictureEdit LoadChartImage(int i, int pointWidth, int pointHeight, int widthSize, int heightSize, string chartName, string Isexc, string st_ymd)
        //{
        //    //string datetime = dateStart.DateTime.ToString("yyyyMMdd");
        //    string datetime = st_ymd;
        //    string year = datetime.Substring(0, 4);
        //    string month = datetime.Substring(4, 2);
        //    string day = datetime.Substring(6, 2);
        //    string path = rootDir + "\\" + cboCategory.Text + "\\" + year + "\\" + month + "\\" + day;
        //    path = path + "\\" + chartName;
        //    //string path = rootDir  + chartName ;

        //    if (!File.Exists(path))
        //    {
        //        return null;
        //    }
        //    Bitmap bmp = new Bitmap(path);
        //    CheckForIllegalCrossThreadCalls = false;
        //    DevExpress.XtraEditors.PictureEdit chartImage = new DevExpress.XtraEditors.PictureEdit();

        //    //Pen pen = new Pen(Color.Red, 3);
        //    ////chartImage.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
        //    ////chartImage.Properties.Appearance.BorderColor = Color.Red;
        //    //Graphics g =  Graphics.FromImage(bmp);
        //    //DevExpress.Utils.Drawing.GraphicsCache g1 = new DevExpress.Utils.Drawing.GraphicsCache(g);
        //    // //g.DrawRectangle(pen, new Rectangle(0, 0, Math.Abs(bmp.Width), Math.Abs(bmp.Height)));
        //    //chartImage.Properties.Appearance.FillRectangle(g1, new Rectangle(0, 0, Math.Abs(bmp.Width), Math.Abs(bmp.Height)));

        //    chartImage.Location = new System.Drawing.Point(pointWidth, pointHeight);
        //    chartImage.Name = chartName;
        //    chartImage.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
        //    chartImage.Size = new System.Drawing.Size(widthSize, heightSize);
        //    chartImage.TabIndex = i;
        //    chartImage.Image = bmp;
        //    chartImage.Tag = i.ToString();

        //    if (Isexc.Equals("Y"))
        //    {
        //        chartImage.Properties.Appearance.BorderColor = Color.Blue;
        //        chartImage.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;

        //    }
        //    chartImage.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
        //    chartImage.Properties.ContextMenuStrip = this.MenuStrip1;
        //    return chartImage;
        //}
        //*/
        //private string GetChartTag(string chartName)
        //{
        //    DevExpress.XtraEditors.PictureEdit chart = (DevExpress.XtraEditors.PictureEdit)this.panel4.Controls[chartName];
        //    string tag = (string)chart.Tag;
        //    return tag;
        //}

        //private PictureEdit GetChart(string chartName)
        //{
        //    DevExpress.XtraEditors.PictureEdit chart = (DevExpress.XtraEditors.PictureEdit)this.panel4.Controls[chartName];
        //    return chart;
        //}

        //private void _frmProgress_Cancel(object sender, ISIA.UI.COMMON.FrmProgress.CancelEventArgs e)
        //{
        //    _reserveData = e.ReserveResult;
        //    bgDownload.CancelAsync();
        //}


        //public DataSet LoadData()
        //{

        //    ChartServiceArgsPack chartServiceArgs = new ChartServiceArgsPack();

        //    chartServiceArgs.ReportDate = dtpStartTime.Text;
        //    chartServiceArgs.DbId = cmbDbName.Text;
        //    chartServiceArgs.Instance_Number = cmbInstance.Text;
        //    chartServiceArgs.RuleName = cmbRuleName.Text;
        //    chartServiceArgs.RuleNo = cmbRuleNo.Text;
        //    chartServiceArgs.ParameterName = cmbParameterName.Text;

        //    //ResargsPack.FabId = cboFab.Text;
        //    //ResargsPack.TechId = cboTech.Text;
        //    //ResargsPack.LotCd = cboLotcode.Text;


        //    //DataSet ds = bs.ExecuteDataSet("GetImageData", args.getPack());
        //    return new DataSet();

        //    //DB에서 파일 List를 가져온다.

        //    //FTP로 해당 파일을 읽는다.





        //}

        //public void DisplayData(DataSet dataSet)
        //{
        //    if (dataSet == null || dataSet.Tables[0].Rows.Count == 0)
        //    {
        //        //ShowNoData();
        //        return;
        //    }
        //    //CheckLayout();
        //    //CreateChart();
        //}


        //#endregion

        //#region BackGroundWorker

        //private void bgDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    _frmProgress.SetValue(100, "");
        //    _frmProgress.Close();

        //    if (e.Cancelled)
        //    {
        //        if (_reserveData)
        //            Array.Resize(ref _downLoadFileNames, _cancelIndex);
        //        else
        //            _downLoadFileNames = new string[0];
        //    }

        //    if (DownloadCompleted != null)
        //        DownloadCompleted(_downLoadFileNames);
        //}

        //private void bgDownload_ProressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    _frmProgress.SetValue(e.ProgressPercentage, e.UserState.ToString());
        //}

        //private void bgDownload_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    //FTP 연결하여 데이터 다운로드.
        //}

        //#endregion

        //#region Event Handlers
        //private void btnSelect_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "Search()", ex.ToString());
        //    }

        //    #endregion
        //}
    }
}
