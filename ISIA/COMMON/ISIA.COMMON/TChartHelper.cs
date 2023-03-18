using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steema.TeeChart;
using Steema.TeeChart.Export;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;

namespace ISIA.COMMON
{
    public class TChartHelper
    {
        public TChartHelper()
        {

        }


        #region Tchart Methods

        public virtual TChart ChartDesign(TChart chart)
        {
            //차트 디자인 하기.
            //1. 기본 디자인
            //2. 차트 사이즈

            //초기화
            chart = new TChart();

            //X축 Lable Format
            chart.Axes.Bottom.Labels.DateTimeFormat = "yyyy-MM-dd";

            //Chart SIZE
            chart.Chart.Width = 1920;
            chart.Chart.Height = 1080; 

            //chart.

            return chart;
        }

        public virtual void ChangeChartTitle(TChart chart, string title)
        {
            try
            {
                chart.Header.Lines = new string[] { title };
                chart.Header.Font.Name = "Tahoma";
                chart.Header.Font.Color = Color.Black;
                chart.Header.Font.Size = 10;
                chart.Header.TextAlign = StringAlignment.Center;
                chart.Header.Alignment = StringAlignment.Center;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public virtual void SaveChartImage(TChart chart, string filepath, int width, int height)
        {
            try
            {

                Steema.TeeChart.Export.ImageExport export = new ImageExport(chart.Chart);                
                PNGFormat png = export.PNG;
                png.Height = height;
                png.Width = width;

                png.Save(filepath);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public virtual void ChartYLimitLine(TChart chart, string label, Color color, double value, int width)
        {

            ColorLine colorLiney = new ColorLine();
            colorLiney.Axis = chart.Axes.Left;
            colorLiney.AllowDrag = false;
            colorLiney.Pen.Color = color;
            colorLiney.Pen.Width = width;
            colorLiney.Pen.Style = DashStyle.Solid;
            colorLiney.Pen.EndCap = LineCap.Round;
            colorLiney.Pen.DashCap = DashCap.Round;
            colorLiney.Value = value;

            chart.Tools.Add(colorLiney);
        }

        #endregion

    }
}
