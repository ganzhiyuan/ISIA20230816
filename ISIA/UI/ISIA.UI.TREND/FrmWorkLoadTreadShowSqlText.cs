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
        public FrmWorkLoadTreadShowSqlText(DataTable dt)
        {
            InitializeComponent();
            gridControl1.DataSource = dt;
            gridView1.BestFitColumns();
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

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(e.FocusedRowHandle) as DataRow;
            if (dr == null)
            {
                return;
            }
            this.txtSqlId.Text = dr["SQL_ID"].ToString();
            SqlView.TextChangeBindSQLType(dr["SQL_TEXT"].ToString());
        }
    }
}
