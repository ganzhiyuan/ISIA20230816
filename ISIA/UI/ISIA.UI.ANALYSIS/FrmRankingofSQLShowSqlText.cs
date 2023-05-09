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

namespace ISIA.UI.ANALYSIS
{
    public partial class FrmRankingofSQLShowSqlText : Form
    {
        
        public FrmRankingofSQLShowSqlText(DataTable dtsqlid ,string sqlid,string sqlidtext , string yvaluename )
        {
            InitializeComponent();
            
            tChartSqlText.Header.Text = sqlid;
            //List<Line> list = CreateLine(listDs);
            Line line =  CreateLine(dtsqlid , yvaluename);
            this.txtSqlId.Text = sqlid;
            textsqltext.TextChangeBindSQLType(sqlidtext);
            tChartSqlText.Chart.Series.Add(line);
        }



        private void tButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(textsqltext.Text);
            TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, TAP.UI.EnumMsgType.WARNING, "Success copied.");
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            /*DataRow dr = gridView1.GetDataRow(e.FocusedRowHandle) as DataRow;
            if (dr == null)
            {
                return;
            }
            this.txtSqlId.Text = dr["SQL_ID"].ToString();
            SqlView.TextChangeBindSQLType(dr["SQL_TEXT"].ToString());*/

        }

        private void FrmWorkLoadTreadShowSqlText_Load(object sender, EventArgs e)
        {

            /*var gridView = gridControl1.MainView as GridView;
            var columns = gridView.Columns;
            foreach (GridColumn item in gridView1.Columns)
            {
                if (item.FieldName != "SQL_TEXT")
                {
                    item.Fixed = FixedStyle.Left;
                }
            }*/

            tChartSqlText.Axes.Bottom.Labels.DateTimeFormat = "MM-dd";
            tChartSqlText.Axes.Bottom.Labels.ExactDateTime = true;//x轴显示横坐标为时间
            tChartSqlText.Axes.Left.Minimum = 0; //设置左侧轴的最小值为0
            tChartSqlText.Axes.Left.AutomaticMinimum = false;
            tChartSqlText.Axes.Right.Minimum = 0; //设置右
            tChartSqlText.Panning.Allow = ScrollModes.None;
            
        }


        private Line CreateLine(DataTable dtsqlid , string yvaluename)
        {
            
                Line line = new Line();

                line.DataSource = dtsqlid;
                line.YValues.DataMember = yvaluename;
                line.XValues.DataMember = "END_INTERVAL_TIME";
                line.Legend.Visible = false;
                line.Color = Color.OrangeRed;
                //line.Title = dstable.Tables[0].Rows[0]["SQL_ID"].ToString();
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
            
            return line;
        }
    }
}
