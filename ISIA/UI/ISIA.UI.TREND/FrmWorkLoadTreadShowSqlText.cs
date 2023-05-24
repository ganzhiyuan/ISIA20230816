using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using ISIA.INTERFACE.ARGUMENTSPACK;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
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
    public partial class FrmWorkLoadTreadShowSqlText : Form
    {
        public string colName { get; set; }
        public string DbName { get; set; }
        public DataTable dt { get; set; }
        public FrmWorkLoadTreadShowSqlText(DataTable dt,string colName,string DBName, List<DataSet> listDs,string groupUnit)
        {
            InitializeComponent();
            this.dt = dt;
            gridControl1.DataSource = dt;
            gridView1.BestFitColumns();
            this.colName = colName;
            this.DbName = DBName;
            if (groupUnit == "DAY")
            {
                string[] str = colName.Split('_');
                string colNm = string.Empty;
                for (int i = 0; i < str.Length - 1; i++)
                {
                    colNm += str[i];
                    colNm += "_";
                }
                tChartSqlText.Header.Text = colNm.TrimEnd('_');
            }
            else
            {
                tChartSqlText.Header.Text = colName;
            }
            List<Line> list = CreateLine(listDs);
            foreach (var item in list)
            {
                tChartSqlText.Series.Add(item);
            }

            foreach (Line series in tChartSqlText.Series)
            {
                series.Visible = false;

            }
        }

        private void tButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(txtSqlId.Text);
            TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, TAP.UI.EnumMsgType.WARNING, "Success copied.");
        }
        private void tButton2_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(SqlView.Text);
            TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, TAP.UI.EnumMsgType.WARNING, "Success copied.");
        }

        private void btModule_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(txtMOdule.Text);
            TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, TAP.UI.EnumMsgType.WARNING, "Success copied.");

        }

        private void btAction_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(txtAction.Text);
            TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, TAP.UI.EnumMsgType.WARNING, "Success copied.");

        }

        private void btParsing_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(txtParsing.Text);
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
            this.txtMOdule.Text = dr["module"].ToString();
            this.txtAction.Text = dr["action"].ToString();
            this.txtParsing.Text = dr["parsing_schema_name"].ToString();

        }

        private void FrmWorkLoadTreadShowSqlText_Load(object sender, EventArgs e)
        {

            var gridView = gridControl1.MainView as GridView;
            var columns = gridView.Columns;
            foreach (GridColumn item in gridView1.Columns)
            {
                if (item.FieldName != "SQL_TEXT")
                {
                    item.Fixed = FixedStyle.Left;
                }
                //item.OptionsColumn.AllowEdit = false;
            }

            tChartSqlText.Axes.Bottom.Labels.DateTimeFormat = "MM-dd";
            tChartSqlText.Axes.Bottom.Labels.ExactDateTime = true;//x轴显示横坐标为时间
            tChartSqlText.Axes.Left.Minimum = 0; //设置左侧轴的最小值为0
            tChartSqlText.Axes.Left.AutomaticMinimum = false;
            tChartSqlText.Axes.Right.Minimum = 0; //设置右
            tChartSqlText.Panning.Allow = ScrollModes.None;

        }
        Dictionary<int, DataSet> dic = new Dictionary<int, DataSet>();

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            dic = new Dictionary<int, DataSet>();
            int[] selectedRows = gridView1.GetSelectedRows();
            List<string> listStr = new List<string>();
            foreach (var item in selectedRows)
            {
                DataRow row = gridView1.GetDataRow(item);
                listStr.Add(row["sql_id"].ToString());
            }

            foreach (Line item in tChartSqlText.Series)
            {
                if (listStr.Contains(item.Title))
                {
                    item.Visible = true;
                }
                else
                {
                    item.Visible = false;
                }
            }

            // 遍历选中多行数据，并获取相应的数据
            //foreach (int rowHandle in selectedRows)
            //{
            //    DataRow row = gridView1.GetDataRow(rowHandle);
            //    AwrArgsPack args = new AwrArgsPack();
            //    args.StartTime = DateTime.Now.AddDays(-60).ToString("yyyy-MM-dd HH:mm:ss");
            //    args.DBName = DbName;
            //    args.ParamNamesString = this.colName;
            //    args.ParamType = row["SQL_ID"].ToString();
            //    DataSet dataSet = bs.ExecuteDataSet("GetWorkloadNaerTwoM", args.getPack());
            //    dic.Add(rowHandle, dataSet);
            //}
            //List<Line> list = CreateLine(dic);
            //foreach (var item in list)
            //{
            //    tChartSqlText.Series.Add(item);
            //}
        }

        private List<Line> CreateLine(List<DataSet> listDs)
        {
            List<Line> list = new List<Line>();
            foreach (var dstable in listDs)
            {
                if (dstable==null||dstable.Tables==null|| dstable.Tables[0].Rows.Count==0)
                {
                    continue;
                }
                Line line = new Line();

                line.DataSource = dstable.Tables[0];
                line.YValues.DataMember = colName;
                line.XValues.DataMember = "WORKDATE";
                line.Legend.Visible = false;
                line.Color = Color.OrangeRed;
                line.Title = dstable.Tables[0].Rows[0]["SQL_ID"].ToString();
                line.Pointer.HorizSize = 1;
                line.Pointer.VertSize = 1;

                //line.ColorEachLine = true;
                //line.Legend.Text = dstable.TableName;
                line.Legend.BorderRound = 10;
                line.Pointer.Style = PointerStyles.Circle;
                line.Pointer.Visible = true;
                //line.Pointer.Color = Color.OrangeRed;
                //line.Pointer.SizeDouble = 1;
                line.XValues.DateTime = true;
                list.Add(line);
            }
            
            return list;
        }

    }
}
