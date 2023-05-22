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
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Base.Configuration;
using TAP.Data.Client;
using TAP.Fressage;
using TAP.FTP;
using TAP.UI;

namespace ISIA.UI.TREND
{
    public partial class FrmChartServiceTrend : DockUIBase1T1
    {
        #region Constant
        #endregion

        #region Field

        private string _uRL;

        private string _localDir;
        private string _appName;
        private string _appDirectory;

        List<Image> _imageList = new List<Image>();
        public delegate void DownLoadFileHandler(string[] fileNames);

        public event DownLoadFileHandler DownloadCompleted;

        private System.ComponentModel.BackgroundWorker bgDownload;
        private ISIA.UI.COMMON.FrmProgress _frmProgress = new ISIA.UI.COMMON.FrmProgress();
        private string[] _downLoadFileNames;
        private bool _reserveData = false;
        private int _cancelIndex = 0;
        BizDataClient _bs = null;
        private int showCol = 5;
        List<DevExpress.XtraEditors.PictureEdit> allChartList = new List<DevExpress.XtraEditors.PictureEdit>();
        DevExpress.XtraEditors.PictureEdit currentChart;

        #endregion

        #region Property
        #endregion

        #region Create and Disposer
        public FrmChartServiceTrend()
        {
            InitializeComponent();
            InitializeURL();
            InitializeBizDataClient();
            InitializeControl();
        }
        #endregion

        #region Initialize
        private void InitializeControl() { InitializeDate(); }

        private void InitializeBackgroundWorker()
        {
            this.bgDownload = new System.ComponentModel.BackgroundWorker();
            this.bgDownload.WorkerReportsProgress = true;
            this.bgDownload.WorkerSupportsCancellation = true;
            this.bgDownload.DoWork += new DoWorkEventHandler(this.bgDownload_DoWork);
            this.bgDownload.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bgDownload_RunWorkerCompleted);
            this.bgDownload.ProgressChanged += new ProgressChangedEventHandler(this.bgDownload_ProressChanged);
        }

        private void InitializeURL()
        {
            #region Code

            try
            {

                //_localDir = Directory.GetParent(FormRibbon_ISEM._ExecutableDirectory.Trim('\\')).FullName;
                _localDir = Directory.GetCurrentDirectory();
                _uRL = TAP.App.Base.AppConfig.ConfigManager.HostCollection["ChartService"]["URL"];
                
                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void InitializeBizDataClient()
        {
            try
            {
                _bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.ChartServiceTrend");
            }
            catch (Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "InitializeBizDataClient()", ex.ToString());
            }
        }

        private void InitializeDate()
        {
            try
            {
                this.dtpStartTime.DateTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "InitializeDate()", ex.ToString());
            }
        }
        #endregion

        #region Method
        /* Image Load 부분.
        private DevExpress.XtraEditors.PictureEdit LoadChartImage(int i, int pointWidth, int pointHeight, int widthSize, int heightSize, string chartName, string Isexc, string st_ymd)
        {
            //string datetime = dateStart.DateTime.ToString("yyyyMMdd");
            string datetime = st_ymd;
            string year = datetime.Substring(0, 4);
            string month = datetime.Substring(4, 2);
            string day = datetime.Substring(6, 2);
            string path = rootDir + "\\" + cboCategory.Text + "\\" + year + "\\" + month + "\\" + day;
            path = path + "\\" + chartName;
            //string path = rootDir  + chartName ;

            if (!File.Exists(path))
            {
                return null;
            }
            Bitmap bmp = new Bitmap(path);
            CheckForIllegalCrossThreadCalls = false;
            DevExpress.XtraEditors.PictureEdit chartImage = new DevExpress.XtraEditors.PictureEdit();

            //Pen pen = new Pen(Color.Red, 3);
            ////chartImage.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            ////chartImage.Properties.Appearance.BorderColor = Color.Red;
            //Graphics g =  Graphics.FromImage(bmp);
            //DevExpress.Utils.Drawing.GraphicsCache g1 = new DevExpress.Utils.Drawing.GraphicsCache(g);
            // //g.DrawRectangle(pen, new Rectangle(0, 0, Math.Abs(bmp.Width), Math.Abs(bmp.Height)));
            //chartImage.Properties.Appearance.FillRectangle(g1, new Rectangle(0, 0, Math.Abs(bmp.Width), Math.Abs(bmp.Height)));

            chartImage.Location = new System.Drawing.Point(pointWidth, pointHeight);
            chartImage.Name = chartName;
            chartImage.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            chartImage.Size = new System.Drawing.Size(widthSize, heightSize);
            chartImage.TabIndex = i;
            chartImage.Image = bmp;
            chartImage.Tag = i.ToString();

            if (Isexc.Equals("Y"))
            {
                chartImage.Properties.Appearance.BorderColor = Color.Blue;
                chartImage.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;

            }
            chartImage.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            chartImage.Properties.ContextMenuStrip = this.MenuStrip1;
            return chartImage;
        }
        
        private string GetChartTag(string chartName)
        {
            DevExpress.XtraEditors.PictureEdit chart = (DevExpress.XtraEditors.PictureEdit)this.panel4.Controls[chartName];
            string tag = (string)chart.Tag;
            return tag;
        }

        private PictureEdit GetChart(string chartName)
        {
            DevExpress.XtraEditors.PictureEdit chart = (DevExpress.XtraEditors.PictureEdit)this.panel4.Controls[chartName];
            return chart;
        }
        */
        private void _frmProgress_Cancel(object sender, ISIA.UI.COMMON.FrmProgress.CancelEventArgs e)
        {
            _reserveData = e.ReserveResult;
            bgDownload.CancelAsync();
        }
        private async Task<List<Image>> GetImagesAsync(DataTable imageData)
        {
            List<Image> images = new List<Image>();
            HttpClient client = new HttpClient();

            foreach (DataRow row in imageData.Rows)
            {
                try
                {
                    string imageFileName = row["IMAGEPATH"].ToString();

                    string url = _uRL + '/' + imageFileName;


                    // Download the image and convert it to an Image object
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Ensure the request was successful
                    response.EnsureSuccessStatusCode();

                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    {
                        Image image = Image.FromStream(stream);
                        images.Add(image);
                    }
                }
                catch (System.Exception ex)
                {
                    if (TAP.App.Base.AppConfig.ConfigManager.HostCollection["ChartService"]["IgnoreExcetion"] == "false")
                        throw ex;
                    else
                        continue;
                }
            }
            
            return images;
        }
        private void ShowNoData()
        {
            this.Invoke(new EventHandler(delegate
            {
                Label Maglabel = new Label();
                Maglabel.Text = _translator.ConvertGeneralTemplate(EnumVerbs.FIND, EnumGeneralTemplateType.CANNOT, "image");
                int with = (this.pnlImage.Width - Maglabel.Width) / 2;
                int height = (this.pnlImage.Height - Maglabel.Height) / 2;
                Maglabel.Location = new System.Drawing.Point(with, height);
                Maglabel.AutoSize = true;
                this.pnlImage.Controls.Add(Maglabel);
            }));
        }

        public DataSet LoadData()
        {
            ComboBoxControl comboBoxUtil = new ComboBoxControl();
            ChartServiceArgsPack chartServiceArgs = new ChartServiceArgsPack();
                        
            chartServiceArgs.ReportDate = dtpStartTime.DateTime.ToString("yyyyMMdd");
            chartServiceArgs.DbId = comboBoxUtil.SelectedTCheckComboBoxValue(cmbDbName);
            chartServiceArgs.Instance_Number = cmbInstance.Text;
            chartServiceArgs.RuleName = cmbRuleName.Text;
            chartServiceArgs.RuleNo = cmbRuleNo.Text;
            chartServiceArgs.ParameterName = cmbParameterName.Text;
            
            DataSet ds = _bs.ExecuteDataSet("GetImageData", chartServiceArgs.getPack());

            _imageList.Clear();

            return ds;

        }

        public async void DisplayData(DataSet dataSet)
        {
            _imageList = await GetImagesAsync(dataSet.Tables[0]);

            if (_imageList.Count < 1)
            {
                ShowNoData();
                return;
            }
            CheckLayout();
            CreateChart(dataSet);
        }

        private void CheckLayout()
        {
            try
            {                
                if (int.Parse(txtShowCol.Text) <= 0)
                {
                    showCol = 1;
                    txtShowCol.Text = "1";
                }
                else if (int.Parse(txtShowCol.Text) > 4)
                {
                    showCol = 4;
                    txtShowCol.Text = "4";
                }
                else
                {
                    showCol = int.Parse(txtShowCol.Text);
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateChart(DataSet dataSet)
        {
            //Chart이미지를 불러와 바인딩하는 부분 수정하기.

            ClearChart();
            CheckLayout();
            int chartCount = _imageList.Count;

            int panelwidth = pnlImage.Width - 20;
            int Pointwidth = 0;
            int Pointheight = 0;

            int widthsize = (panelwidth - (4 + 1) * Convert.ToInt32(5.3)) / 4;
            int sizeheight = widthsize - 40;
            int rowCount = _imageList.Count;
            int tmpMin = Math.Min(rowCount, showCol);
            if (tmpMin < rowCount)
            {
                tmpMin = rowCount;
            }
            if (showCol == 1)
            {
                widthsize = (panelwidth - (showCol + 1) * 8) / showCol;
                sizeheight = widthsize - 40;
                widthsize = widthsize * 11 / 20;
                sizeheight = sizeheight * 1 / 2;
                for (int i = 0; i < tmpMin; i++)
                {
                    if (i != 0)
                    {
                        if (allChartList[i - 1] == null)
                        {
                            //Chart Name 부분 정의해서 수정하기.
                            allChartList.Add(LoadChartImage(i, Pointwidth, Pointheight, widthsize, sizeheight, dataSet.Tables[0].Rows[i]["PARAMETERNAME"].ToString(), dataSet.Tables[0].Rows[i]["DETECTIONFLAG"].ToString(), _imageList[i]));
                            continue;
                        }
                        if ((panelwidth - (allChartList[i - 1].Location.X) - 8 * (i + 1)) > widthsize * 3 * 2)
                        {
                            Pointwidth = Pointwidth + (panelwidth - widthsize) / 2 + widthsize;
                        }
                        else
                        {
                            Pointheight = 10 + sizeheight + widthsize * 1 / 10 + allChartList[i - 1].Location.Y;
                            Pointwidth = (panelwidth - widthsize) / 2;
                        }
                    }
                    else
                    {
                        Pointheight = 0;
                        Pointwidth = (panelwidth - widthsize) / 2;
                    }

                    allChartList.Add(LoadChartImage(i, Pointwidth, Pointheight, widthsize, sizeheight, dataSet.Tables[0].Rows[i]["PARAMETERNAME"].ToString(), dataSet.Tables[0].Rows[i]["DETECTIONFLAG"].ToString(), _imageList[i]));

                }
            }
            else if (showCol == 2)
            {
                widthsize = widthsize * 3 / 2;
                sizeheight = sizeheight * 3 / 2;
                int newspace = (panelwidth - widthsize * showCol) / (showCol + 1);

                for (int i = 0; i < tmpMin; i++)
                {
                    if (i != 0)
                    {
                        if (allChartList[i - 1] == null)
                        {
                            allChartList.Add(LoadChartImage(i, Pointwidth, Pointheight, widthsize, sizeheight, dataSet.Tables[0].Rows[i]["PARAMETERNAME"].ToString(), dataSet.Tables[0].Rows[i]["DETECTIONFLAG"].ToString(), _imageList[i]));
                            continue;
                        }
                        if ((panelwidth - (allChartList[i - 1].Location.X) - newspace * (showCol + 1)) > widthsize)
                        {
                            Pointwidth = Pointwidth + newspace + widthsize;
                        }
                        else
                        {
                            Pointheight = 15 + sizeheight + widthsize * 1 / 10 + allChartList[i - 1].Location.Y;
                            Pointwidth = newspace;
                        }
                    }
                    else
                    {
                        Pointheight = 0;
                        Pointwidth = newspace;
                    }

                    allChartList.Add(LoadChartImage(i, Pointwidth, Pointheight, widthsize, sizeheight, dataSet.Tables[0].Rows[i]["PARAMETERNAME"].ToString(), dataSet.Tables[0].Rows[i]["DETECTIONFLAG"].ToString(), _imageList[i]));
                }
            }
            else
            {
                int newspace = (panelwidth - widthsize * showCol) / (showCol + 1);

                for (int i = 0; i < tmpMin; i++)
                {
                    if (i != 0)
                    {
                        if (allChartList[i - 1] == null)
                        {
                            allChartList.Add(LoadChartImage(i, Pointwidth, Pointheight, widthsize, sizeheight, dataSet.Tables[0].Rows[i]["PARAMETERNAME"].ToString(), dataSet.Tables[0].Rows[i]["DETECTIONFLAG"].ToString(), _imageList[i]));
                            continue;
                        }
                        if ((panelwidth - (allChartList[i - 1].Location.X) - newspace * (showCol + 1)) > widthsize)
                        {
                            Pointwidth = Pointwidth + newspace + widthsize;
                        }
                        else
                        {
                            Pointheight = 10 + sizeheight + 8 + allChartList[i - 1].Location.Y;
                            Pointwidth = newspace;
                        }
                    }
                    else
                    {
                        Pointheight = 0;
                        Pointwidth = newspace;
                    }
                    allChartList.Add(LoadChartImage(i, Pointwidth, Pointheight, widthsize, sizeheight, dataSet.Tables[0].Rows[i]["PARAMETERNAME"].ToString(), dataSet.Tables[0].Rows[i]["DETECTIONFLAG"].ToString(), _imageList[i]));

                }
            }

            for (int i = 0; i < allChartList.Count; i++)
            {
                AddChartToPanel(i);
            }
        }

        private void AddChartToPanel(int i)
        {
            this.Invoke(new EventHandler(delegate
            {
                this.pnlImage.Controls.Add(allChartList[i]);

            }));
        }

        private void ClearChart()
        {
            this.pnlImage.Controls.Clear();
            allChartList.Clear();
        }

        private DevExpress.XtraEditors.PictureEdit LoadChartImage(int i, int pointWidth, int pointHeight, int widthSize, int heightSize, string chartName, string Isexc, Image image)
        {            
            DevExpress.XtraEditors.PictureEdit chartImage = new DevExpress.XtraEditors.PictureEdit();

            chartImage.Location = new System.Drawing.Point(pointWidth, pointHeight);
            chartImage.Name = chartName;
            chartImage.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            chartImage.Size = new System.Drawing.Size(widthSize, heightSize);
            chartImage.TabIndex = i;
            chartImage.Image = image;
            chartImage.Tag = i.ToString();

            if (Isexc.Equals("YES"))
            {
                chartImage.Properties.Appearance.BorderColor = Color.Blue;
                chartImage.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;

            }
            chartImage.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
           // chartImage.Properties.ContextMenuStrip = this.MenuStrip1;
            return chartImage;
        }

        #endregion

        #region BackGroundWorker

        private void bgDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _frmProgress.SetValue(100, "");
            _frmProgress.Close();

            if (e.Cancelled)
            {
                if (_reserveData)
                    Array.Resize(ref _downLoadFileNames, _cancelIndex);
                else
                    _downLoadFileNames = new string[0];
            }

            if (DownloadCompleted != null)
                DownloadCompleted(_downLoadFileNames);
        }

        private void bgDownload_ProressChanged(object sender, ProgressChangedEventArgs e)
        {
            _frmProgress.SetValue(e.ProgressPercentage, e.UserState.ToString());
        }

        private void bgDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            //FTP 연결하여 데이터 다운로드.
        }

        #endregion

        #region Event Handlers
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!base.ValidateUserInput(this.lcSerachOptions)) return;

                BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            }
            catch (System.Exception ex)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "Search()", ex.ToString());
            }

            #endregion
        }
    }
}
