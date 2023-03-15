using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using TAP.Fressage;

namespace TAP.UI
{
    /// <summary>
    /// This class is base of message box.
    /// </summary>
    public partial class TAPMsgBox : Form
    {
        #region Fields

        private static TAPMsgBox instance;
        private NeutralConverter _converter;

        #endregion

        #region Properties

        /// <summary>
        /// Static accessor of instance
        /// </summary>
        public static TAPMsgBox Instance
        {
            get
            {
                if (instance == null)
                    instance = new TAPMsgBox();

                return instance;
            }
        }

        #endregion

        #region Creator

        /// <summary>
        /// This creator creates instance of this.
        /// </summary>
        private TAPMsgBox()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method display user message.
        /// </summary>
        /// <param name="title">Title of message box</param>
        /// <param name="messageType">Message type</param>
        /// <param name="message">Message</param>
        /// <param name="detail">Detail message</param>
        public DialogResult ShowMessage(string title, EnumMsgType messageType, string message, List<string> detail)
        {
            #region Show

            string tmpDetail = string.Empty;

            try
            {
                if (detail != null && detail.Count > 0)
                {
                    for (int i = 0; i < detail.Count; i++)
                    {
                        tmpDetail += detail[i] + "\r\n";
                    }
                }

                this.Initialize(title, messageType, message, tmpDetail);
                this.ShowDialog();

                return this.DialogResult;
            }
            catch
            {
                return this.DialogResult;
            }

            #endregion
        }

        /// <summary>
        /// This method display user message.
        /// </summary>
        /// <param name="title">Title of message box</param>
        /// <param name="messageType">Message type</param>
        /// <param name="message">Message</param>
        /// <param name="detail">Detail message</param>
        public DialogResult ShowMessage(string title, EnumMsgType messageType, string message, string detail)
        {
            #region Show

            try
            {
                this.Initialize(title, messageType, message, detail);
                this.ShowDialog();

                return this.DialogResult;
            }
            catch
            {
                return this.DialogResult;
            }

            #endregion
        }

        /// <summary>
        /// This method display user message.
        /// </summary>
        /// <param name="title">Title of message box</param>
        /// <param name="messageType">Message type</param>
        /// <param name="message">Message</param>
        public DialogResult ShowMessage(string title, EnumMsgType messageType, string message)
        {
            #region Show

            try
            {
                this.Initialize(title, messageType, message, string.Empty);
                this.ShowDialog();

                return this.DialogResult;
            }
            catch
            {
                return this.DialogResult;
            }

            #endregion
        }

        /// <summary>
        /// This method display user message.
        /// </summary>
        /// <param name="messageType">Message type</param>
        /// <param name="message">Message</param>
        public DialogResult ShowMessage(EnumMsgType messageType, string message)
        {
            #region Show

            try
            {
                this.Initialize("TAP Message", messageType, message, string.Empty);
                this.ShowDialog();

                return this.DialogResult;
            }
            catch
            {
                return this.DialogResult;
            }

            #endregion
        }

        private Icon GetIconByMessageType(EnumMsgType messageType)
        {
            Icon icon = ConvertBitmap2Ico(Properties.Resources.info_16x16);
            //Bitmap bitmap = new Bitmap(@"C:\Users\HP\Desktop\BoxÃ· æÕº±Í\confirm.ico");
            //Icon icon = ConvertBitmap2Ico(bitmap);

            switch (messageType)
            {
                case EnumMsgType.INFORMATION:
                    icon = ConvertBitmap2Ico(Properties.Resources.info_16x16);
                    break;
                case EnumMsgType.CONFIRM:
                    icon = ConvertBitmap2Ico(Properties.Resources.iconsetsymbols3_16x16);
                    break;
                case EnumMsgType.WARNING:
                    icon = ConvertBitmap2Ico(Properties.Resources.warning_16x16);
                    break;
                case EnumMsgType.ERROR:
                    icon = ConvertBitmap2Ico(Properties.Resources.cancel_16x16);
                    break;
                default:
                    icon = ConvertBitmap2Ico(Properties.Resources.info_16x16);
                    break;
            }
            return icon;
        }

        private Icon ConvertBitmap2Ico(Bitmap bitmap)
        {
            Bitmap icoBitmap = new Bitmap(bitmap, new Size(16, 16));
            IntPtr hIco = icoBitmap.GetHicon();
            Icon icon = Icon.FromHandle(hIco);

            return icon;
        }

        private void Initialize(string title, EnumMsgType messageType, string message, string detail)
        {
            #region Initialize

            this.Text = title;
            this.richTextBoxMessage.Text = message;
            this.Icon = GetIconByMessageType(messageType);
            //this.Icon = Properties.Resources.Information;
            this.richTextBoxDetail.Text = detail;

            this.tapPanel4.Visible = false;
            this.Height = 211;

            if (object.Equals(this._converter, null))
            {
                //EnumLanguage tmpLang = (EnumLanguage)Enum.Parse(typeof(EnumLanguage), TAP.App.Base.AppConfig.ConfigManager.DefinedCollection["UserLang"]);
                EnumLanguage tmpLang = (EnumLanguage)Enum.Parse(typeof(EnumLanguage), InfoBase._USER_INFO.Language);
                this._converter = new NeutralConverter(tmpLang, EnumUseFor.TEXT, false, TapBase.Instance.FressagePath);
            }

            this.tapButtonCancel.Text = this._converter.ConvertPhrase(this.tapButtonCancel.Text);
            this.tapButtonDetail.Text = this._converter.ConvertPhrase(this.tapButtonDetail.Text);
            this.tapButtonNo.Text = this._converter.ConvertPhrase(this.tapButtonNo.Text);
            this.tapButtonOK.Text = this._converter.ConvertPhrase(this.tapButtonOK.Text);
            this.tapButtonYes.Text = this._converter.ConvertPhrase(this.tapButtonYes.Text);

            switch (messageType)
            {
                case EnumMsgType.INFORMATION:
                case EnumMsgType.WARNING:
                    this.tapButtonCancel.Visible = false;
                    this.tapButtonDetail.Visible = false;
                    this.tapButtonNo.Visible = false;
                    this.tapButtonOK.Visible = true;
                    this.tapButtonYes.Visible = false;

                    this.tapButtonOK.Width = 100;
                    this.tapButtonOK.Height = 25;
                    this.tapButtonOK.Location = new Point(this.tapPanel3.Width / 2 - 50, 5);
                    break;

                case EnumMsgType.CONFIRM:
                    this.tapButtonCancel.Visible = false;
                    this.tapButtonDetail.Visible = false;
                    this.tapButtonNo.Visible = true;
                    this.tapButtonOK.Visible = false;
                    this.tapButtonYes.Visible = true;

                    this.tapButtonYes.Width = 100;
                    this.tapButtonYes.Height = 25;
                    this.tapButtonYes.Location = new Point(this.tapPanel3.Width / 2 - 100, 5);

                    this.tapButtonNo.Width = 100;
                    this.tapButtonNo.Height = 25;
                    this.tapButtonNo.Location = new Point(this.tapPanel3.Width / 2, 5);
                    break;

                case EnumMsgType.ERROR:
                    this.tapButtonCancel.Visible = false;
                    this.tapButtonDetail.Visible = true;
                    this.tapButtonNo.Visible = false;
                    this.tapButtonOK.Visible = true;
                    this.tapButtonYes.Visible = false;

                    this.tapButtonOK.Width = 100;
                    this.tapButtonOK.Height = 25;
                    this.tapButtonOK.Location = new Point(this.tapPanel3.Width / 2 - 100, 5);

                    this.tapButtonDetail.Width = 100;
                    this.tapButtonDetail.Height = 25;
                    this.tapButtonDetail.Location = new Point(this.tapPanel3.Width / 2, 5);
                    break;
            }

            #endregion
        }
        /// <summary>
        /// This method display user message.
        /// </summary>
        /// <param name="title">Title of message box</param>
        /// <param name="messageType">Message type</param>
        /// <param name="enumMessage_Text">enumMessage_Text</param>
        public DialogResult ShowMessage(string title, EnumMsgType messageType, EnumMessage_text enumMessage_Text)
        {
            #region Show
            try
            {
                switch (enumMessage_Text)
                {
                    case EnumMessage_text.SAVE:
                        this.Initialize(title, EnumMsgType.INFORMATION, "Save Complete!", string.Empty);
                        this.ShowDialog();
                        break;
                    case EnumMessage_text.DELETE:
                        this.Initialize(title, EnumMsgType.INFORMATION, "Delete Complete!", string.Empty);
                        this.ShowDialog();
                        break;
                    case EnumMessage_text.INSERT:
                        this.Initialize(title, EnumMsgType.INFORMATION, "Insert Complete!", string.Empty);
                        this.ShowDialog();
                        break;
                    case EnumMessage_text.UPDATE:
                        this.Initialize(title, EnumMsgType.INFORMATION, "Update Complete!", string.Empty);
                        this.ShowDialog();
                        break;
                }
                return this.DialogResult;
            }
            catch
            {
                return this.DialogResult;
            }

            #endregion
        }
        #endregion

        #region Event Handlers

        private void tapButtonOK_Click(object sender, EventArgs e)
        {
            #region tapButtonOK_Click

            this.DialogResult = DialogResult.OK;
            this.Close();

            #endregion
        }

        private void tapButtonCancel_Click(object sender, EventArgs e)
        {
            #region tapButtonOK_Click

            this.DialogResult = DialogResult.Cancel;
            this.Close();

            #endregion
        }

        private void tapButtonYes_Click(object sender, EventArgs e)
        {
            #region tapButtonOK_Click

            this.DialogResult = DialogResult.Yes;
            this.Close();

            #endregion
        }

        private void tapButtonNo_Click(object sender, EventArgs e)
        {
            #region tapButtonOK_Click

            this.DialogResult = DialogResult.No;
            this.Close();

            #endregion
        }

        private void tapButtonDetail_Click(object sender, EventArgs e)
        {
            #region tapButtonOK_Click

            if (this.tapPanel4.Visible == false)
            {
                this.tapPanel4.Visible = true;
                this.Height = 400;
            }
            else
            {
                this.tapPanel4.Visible = false;
                this.Height = 211;
            }

            #endregion
        }


        #endregion
    }
}