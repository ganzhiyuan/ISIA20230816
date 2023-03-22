
using ISIA.UI.BASE;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;
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

namespace ISIA.UI.TREND
{
    public partial class FrmSQLFullTextQueryAnalysis : DockUIBase1T1
    {

        #region Feild
        BizDataClient bs = null;
        DataSet dataSet = null;
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
            //chartControl1.Series.Clear();
            DataClient tmpDataClient = new DataClient();
            string tmpMainMenuSql = "SELECT rownum, t.DBID,t.SQL_ID,t.CPU_TIME_DELTA FROM raw_dba_hist_sqlstat_isfa T";
            dataSet = tmpDataClient.SelectData(tmpMainMenuSql, "raw_dba_hist_sqlstat_isfa");
            dataSet.Tables[0].TableName = "TABLE";


            IEnumerable<IGrouping<string, DataRow>> result = dataSet.Tables[0].Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["SQL_ID"].ToString());
            if (result != null && result.Count() > 0)
            {
                foreach (IGrouping<string, DataRow> rows in result)
                {
                    DataTable dataTable = rows.ToArray().CopyToDataTable();
                    dataTable.TableName = rows.Key;
                    if (dataTable.Rows.Count > 0)
                    {
                        dataSet.Tables.Add(dataTable);
                    }
                }
            }
        

            CreateBar();
            /*foreach (DataRow row in tableMain.Rows)
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
            }*/
        }


        private void CreateBar()
        {

            tChart1.Series.Clear();
            tChart1.Legend.LegendStyle = LegendStyles.Values;
            var markstip = new MarksTip(tChart1.Chart);

            Bar bar = new Bar(tChart1.Chart);
            DataTable dt1 = new DataTable();
            dt1.Columns.AddRange(new DataColumn[2] {
            new DataColumn("Name", typeof(string)),
            new DataColumn("NUM", typeof(int)) });
            if (dataSet.Tables.Count > 1)
            {
                foreach (DataTable dt in dataSet.Tables)
                {
                    if (dt.TableName != "TABLE")
                    {
                        int num = Convert.ToInt32(dt.Compute("avg(CPU_TIME_DELTA)", ""));
                        dt1.Rows.Add(dt.TableName, num);
                    }
                }
            }

            dt1.DefaultView.Sort = "NUM DESC";
            dt1 = dt1.DefaultView.ToTable();

            DataTable dt2 = new DataTable();
            dt2.Columns.AddRange(new DataColumn[2] {
            new DataColumn("Name", typeof(string)),
            new DataColumn("NUM", typeof(int)) });

            for (int i = 0; i < 10; i++)
            {
                DataRow drow = dt1.Rows[i];
                
                dt2.Rows.Add(drow["Name"].ToString(), drow["NUM"].ToString());
            }

            /*if (dt1.Rows.Count > 10)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (i > 9)
                    {
                        dt1.Rows.Remove(dt1.Rows[i]);
                    }
                }
            }*/
            void Bar_GetSeriesMark(Series Series, GetSeriesMarkEventArgs e)
            {
                //e.MarkText = $"{dt1.Rows[e.ValueIndex]["Name"]} is {dt1.Rows[e.ValueIndex]["NUM"]}";
                e.MarkText = "NAME :" + $"{dt2.Rows[e.ValueIndex]["Name"]}" + "\r\n" + "VALUE :" + $"{ dt2.Rows[e.ValueIndex]["NUM"]}";
            }

            void TChart1_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
            {
                DataRow dataRow =  dt2.Rows[valueIndex];
                DataClient tmpDataClient = new DataClient();
                string tmpMainMenuSql = string.Format("SELECT T.Sql_Text FROM raw_dba_hist_sqltext_isfa T where t.sql_id='{0}' ", dataRow["NAME"].ToString());
                DataTable tableMain1 = tmpDataClient.SelectData(tmpMainMenuSql, "raw_dba_hist_sqltext_isfa").Tables[0];
                tableMain1.TableName = "sql";

                memoEdit1.Text = tableMain1.Rows[0][0].ToString();
            }
            bar.ColorEach = true;
            bar.DataSource = dt2;
            bar.YValues.DataMember = "NUM";

            bar.LabelMember = "Name";
            bar.Marks.Visible = false;
            bar.GetSeriesMark += Bar_GetSeriesMark;
            tChart1.ClickSeries += TChart1_ClickSeries;
            return;
        }

        

        private void chartControl1_MouseDown(object sender, MouseEventArgs e)
        {
            //ChartHitInfo hi = chartControl1.CalcHitInfo(e.Location);

            /*// Obtain the series point under the test point.
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
            }*/
        }
    }
}
