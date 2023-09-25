using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TAP.Models.UIBasic;

using System.Reflection;
using System.IO;

namespace ISIA.UI.BASE
{

    public partial class PopupMemo : DevExpress.XtraEditors.XtraForm
    {


        public string MsgText { get; set; }
        public string Subject { get; set; }
       

        public PopupMemo()
        {
            InitializeComponent();
        }

        public PopupMemo(string text, string subject):this()
        {
            MsgText = text;
            Subject = subject;
            memoEdit.AppendLine(MsgText);
            this.Text = Subject;
            this.ForeColor = Color.Black;
            this.StartPosition = FormStartPosition.CenterScreen;

        }
    }
}
