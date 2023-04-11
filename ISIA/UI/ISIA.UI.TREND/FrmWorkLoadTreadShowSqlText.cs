using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISIA.UI.TREND
{
    public partial class FrmWorkLoadTreadShowSqlText : Form
    {
        public FrmWorkLoadTreadShowSqlText(string sqlid,string sqltext)
        {
            InitializeComponent();
            this.txtSqlId.Text = sqlid;
            SqlView.TextChangeBindSQLType(sqltext);
        }

        private void tButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(txtSqlId.SelectedText);
            TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, TAP.UI.EnumMsgType.WARNING, "Success copied.");
        }
    }
}
