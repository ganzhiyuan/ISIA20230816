using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using TAP.Fressage;

namespace TAP.UI
{
    /// <summary>
    /// This is backgruond working form
    /// </summary>
    public partial class FormInitializing : Form
    {
        #region Delegate

        /// <summary>
        /// This is event handler of AbortProcessing event
        /// </summary>
        /// <param name="form"></param>
        public delegate void AbortProcessingHandler(Form form);

        /// <summary>
        /// Abort handler
        /// </summary>
        public event AbortProcessingHandler AbortProcessing;

        #endregion

        #region Fields

        //private volatile int _processTime;
        //private Thread _countThread;
        private long _total;
        //private NeutralConverter _converter;
        //private TemplateConverter _translator;

        #endregion

        #region Methods

        /// <summary>
        /// This aborts background work
        /// </summary>
        public void OnAbortProcessing()
        {
            var handler = AbortProcessing;
            if (handler != null) handler(this);
        }

        /// <summary>
        /// This display working progress
        /// </summary>
        public void ViewProcessing()
        {
            this.DisplayMessage();
        }

        #region Private Message

        private void DisplayMessage()
        {
            if( this._total.Equals(0L))
                return;

            this.lblTotal.Text = AsyncMessage.Progress.ToString() + "/" + this._total.ToString();
            int tmpCUrrentProgress = (int) ( (   Convert.ToDecimal(AsyncMessage.Progress) / Convert.ToDecimal(this._total )) * 100 );

            if (tmpCUrrentProgress > 100)
                tmpCUrrentProgress = 100;

            this.progressBar1.Value = tmpCUrrentProgress;
            this.lblComment.Text = AsyncMessage.Message;
        }

        #endregion

        #endregion

        /// <summary>
        /// This creates instance of FormProcessing
        /// </summary>
        /// <param name="total">Total job count</param>
        public FormInitializing(long total)
        {
            InitializeComponent();

            string tmpTitleImage
                = System.IO.Path.Combine(Base.IO.FileBase.Instance.GetAPPDirectory(TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.AppDirectory), "IMAGEs");

            tmpTitleImage = System.IO.Path.Combine(tmpTitleImage, "Title.png");

            //if (System.IO.File.Exists(tmpTitleImage))
            //    this.pictureBox1.Image = Image.FromFile(tmpTitleImage);

            this._total = total;
            this.lblTotal.Text = "0/" + this._total.ToString();
            this.lblComment.Text = "Now Preparing";
            //EnumLanguage tmpLang = (EnumLanguage)Enum.Parse(typeof(EnumLanguage), TAP.App.Base.AppConfig.ConfigManager.DefinedCollection["UserLang"]);
            //this._converter = new NeutralConverter(tmpLang, EnumUseFor.TEXT, false);
            //this._translator = new TemplateConverter(tmpLang, false);
            //this.btnClose.Text = this._converter.ConvertPhrase(this.btnClose.Text);

            string tmpAppName = string.Empty;
            //if (TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MDIDisplayName != null &&
            //    TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MDIDisplayName.Length > 0)
            //    tmpAppName = "<" + TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MDIDisplayName + ">";
            //else
            tmpAppName = "Application";
            this.tLabel1.Text = "Now initializing...";
            //this.tLabel1.Text = _translator.ConvertGeneralTemplate(EnumVerbs.INITIALIZE, EnumGeneralTemplateType.ING, tmpAppName);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            OnAbortProcessing();
        }
    }
}
