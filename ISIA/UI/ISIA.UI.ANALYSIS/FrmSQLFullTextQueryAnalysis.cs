using DevExpress.XtraCharts;
using ISIA.UI.BASE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;

namespace ISIA.UI.ANALYSIS
{
    public partial class FrmSQLFullTextQueryAnalysis : DockUIBase1T1
    {

        #region Feild
        BizDataClient bs = null;
        DataTable tableMain = null;
        #endregion
        public FrmSQLFullTextQueryAnalysis()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.ANALYSIS.DLL", "ISIA.BIZ.ANALYSIS.SQLFullTextQueryAnalysis");
        }

        private void FrmSQLFullTextQueryAnalysis_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            chartControl1.Series.Clear();
            DataClient tmpDataClient = new DataClient();
            string tmpMainMenuSql = "SELECT rownum, t.DBID,t.SQL_ID,t.CPU_TIME_DELTA FROM raw_dba_hist_sqlstat_isfa T where rownum<16 ";
            tableMain = tmpDataClient.SelectData(tmpMainMenuSql, "raw_dba_hist_sqlstat_isfa").Tables[0];
            tableMain.TableName = "主表";

            foreach (DataRow row in tableMain.Rows)
            {
                //Series series1 = new Series("日志统计", ViewType.RadarArea);
                Series series1 = new Series(row["SQL_ID"].ToString(), ViewType.Bar);

                chartControl1.Series.Add(series1);
                SeriesPoint point = null;
                if (row["DBID"] != null)
                {
                    point = new SeriesPoint(row["DBID"].ToString());
                    double[] vals = { Convert.ToDouble(row["CPU_TIME_DELTA"]) };
                    point.Values = vals;
                    series1.Points.Add(point);
                }
            }
        }


        private void chartControl1_MouseDown(object sender, MouseEventArgs e)
        {
            ChartHitInfo hi = chartControl1.CalcHitInfo(e.Location);

            // Obtain the series point under the test point.
            SeriesPoint point = hi.SeriesPoint;

            // Check whether the series point was clicked or not.
            if (point != null)
            {
                string s = hi.HitObject.ToString();
                var sql_id = tableMain.AsEnumerable().Where(x => x.Field<string>("SQL_ID") == s).ToList();

                DataClient tmpDataClient = new DataClient();
                string tmpMainMenuSql = string.Format("SELECT T.Sql_Text FROM raw_dba_hist_sqltext_isfa T where t.sql_id='{0}' ", sql_id[0]["SQL_ID"].ToString());
                DataTable tableMain1 = tmpDataClient.SelectData(tmpMainMenuSql, "raw_dba_hist_sqltext_isfa").Tables[0];
                tableMain1.TableName = "sql";

                memoEdit1.Text = tableMain1.Rows[0][0].ToString();

                // Obtain the series point argument.
                //string argument = "Argument: " + point.Argument.ToString();

                //// Obtain series point values.
                //string values = "Value(s): " + point.Values[0].ToString();
                //if (point.Values.Length > 1)
                //{
                //    for (int i = 1; i < point.Values.Length; i++)
                //    {
                //        values = values + ", " + point.Values.ToString();
                //    }
                //}

                //// Show the tooltip.
                //toolTipController1.ShowHint(argument + "\n" + values, "SeriesPoint Data");
            }
            else
            {
                // Hide the tooltip.
                toolTipController1.HideHint();
            }
        }
    }
}
