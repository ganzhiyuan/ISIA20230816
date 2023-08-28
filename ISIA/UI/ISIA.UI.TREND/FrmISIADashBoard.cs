using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.UI;

namespace ISIA.UI.TREND
{
    public partial class FrmISIADashBoard : UIBase
    {
        public FrmISIADashBoard()
        {
            InitializeComponent();
            initData();
        }
        private void initData()
        {
            DataTable dt = initDemoData();
            gridControl1.DataSource = dt;

        }

        private DataTable initDemoData()
        {
            DateTime currentTime = DateTime.Now;
            DateTime beforeTime = currentTime.AddHours(-1);
            string format = "yyyy-MM-dd HH:mm:ss";
            string endStr = currentTime.ToString(format);
            string startStr = beforeTime.ToString(format);

            DataTable dt = new DataTable(); dt.Columns.AddRange(new DataColumn[]
            { new DataColumn("name", typeof(string)),
                new DataColumn("age", typeof(Int32)),
                new DataColumn("height", typeof(Int32)),
                new DataColumn("startTime", typeof(String)),
                new DataColumn("endTime", typeof(String)),
                new DataColumn("idCard", typeof(String)),
                new DataColumn("process", typeof(Int32)),
                new DataColumn("condition", typeof(Int32))
                });
            dt.Rows.Add("John", 12, 175, startStr, endStr, "2231231231", 100, 1);
            dt.Rows.Add("mike", 16, 178, startStr, endStr, "22312312hhh31", 123, 1);
            dt.Rows.Add("kevein", 34, 187, startStr, endStr, "2231vvv231231", 666, 1);
            dt.Rows.Add("amy", 24, 167, startStr, endStr, "22312312fff31", 245, 0);
            dt.Rows.Add("John", 12, 175, startStr, endStr, "2231231231", 100, 1);
            dt.Rows.Add("mikfe", 16, 178, startStr, endStr, "22312312hhh31", 123, 1);
            dt.Rows.Add("kehvein", 34, 187, startStr, endStr, "2231vvv231231", 65, 1);
            dt.Rows.Add("ahjmy", 24, 167, startStr, endStr, "22312312fff31", 245, 0);
            dt.Rows.Add("Jjohn", 12, 175, startStr, endStr, "2231231231", 155, 1);
            dt.Rows.Add("mkike", 16, 178, startStr, endStr, "22312312hhh31", 13, 1);
            dt.Rows.Add("kekvein", 34, 187, startStr, endStr, "2231vvv231231", 68, 1);
            dt.Rows.Add("assmy", 24, 167, startStr, endStr, "22312312fff31", 200, 0);
            return dt;
        }

        private void labelControl2_Click(object sender, EventArgs e)
        {

        }
    }
}