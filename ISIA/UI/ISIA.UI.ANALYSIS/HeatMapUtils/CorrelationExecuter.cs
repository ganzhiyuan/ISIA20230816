using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Analysis.Correlation
{
    public class CorrelationExecuter : IExecuter
    {
        private DataSet dataSet = null;

        //parampair暂时用不到
        private List<ParmPairs> parmPairsList = null;

        private SortedDictionary<string, int> axisX;

        private SortedDictionary<string, int> axisY;

        private Dictionary<string, double[]> columnDataMapping = new Dictionary<string, double[]>();

        private double[,] dataMap;

        private Dictionary<ParmPairs, Double> result = new Dictionary<ParmPairs, double>();
        public DataSet DataSet { get => dataSet; set => dataSet = value; }
        public List<ParmPairs> ParmPairsList { get => parmPairsList; set => parmPairsList = value; }
        //parampair暂时用不到
        internal Dictionary<ParmPairs, double> Result { get => result; set => result = value; }
        public SortedDictionary<string, int> AxisX { get => axisX; set => axisX = value; }
        public SortedDictionary<string, int> AxisY { get => axisY; set => axisY = value; }
        public double[,] DataMap { get => dataMap; set => dataMap = value; }

        public void Execute()
        {
            DataTable dataTable = dataSet.Tables[0];
            DataColumnCollection dataColumnCollection = dataTable.Columns;
            CheckColumn(dataColumnCollection);
            Calculate(dataTable);

        }

        public bool IsValid()
        {
            if (dataSet is null)
            {
                return false;
            }
            return true;
        }

        public void Calculate(DataTable dt)
        {

            foreach (var elementY in axisY)
            {

                foreach (var elementX in axisX)
                {
                    Double res = 0;
                    if (elementY.Key.Equals(elementX.Key))
                    {
                        res = 0;
                    }
                    else
                    {
                        double[] yParam = GetColumnDataMapping(elementY.Key);
                        double[] xParam = GetColumnDataMapping(elementX.Key);
                        
                        if (yParam is null)
                        {
                            //List<Double> yParmList = new List<Double>();
                            //foreach (DataRow row in dt.Rows)
                            //{
                            //    yParmList.Add(Double.Parse(row[elementY.Key].ToString()));
                            //}
                            //yParam = yParmList.ToArray();
                            yParam =dt.AsEnumerable().Select(x => x.Field<double>(elementY.Key)).ToArray();
                            columnDataMapping.Add(elementY.Key, yParam);
                        }
                        if (xParam is null)
                        {
                            //List<Double> xParmList = new List<Double>();
                            //foreach (DataRow row in dt.Rows)
                            //{
                            //    xParmList.Add(Double.Parse(row[elementX.Key].ToString()));
                            //}
                            //xParam = xParmList.ToArray();
                            xParam = dt.AsEnumerable().Select(x => x.Field<double>(elementX.Key)).ToArray();
                            try
                            {
                                columnDataMapping.Add(elementX.Key, xParam);
                            }
                            catch (ArgumentException ex)
                            {

                            }

                        }
                        res = Calculate(xParam, yParam);
                    }
                    dataMap[elementY.Value, elementX.Value] = res;


                }
            }
        }

        private double[] GetColumnDataMapping(string key)
        {
            try
            {
                double[] columnData = columnDataMapping[key];
                return columnData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private Double Calculate(Double[] x, Double[] y)
        {
            try
            {
                //if (x.Length != y.Length)
                //    throw new ArgumentException("values must be the same length");
                var avg1 = x.Average();
                var avg2 = y.Average();
                var sum1 = x.Zip(y, (x1, y1) => (x1 - avg1) * (y1 - avg2)).Sum();
                var sumSqr1 = x.Sum(val => Math.Pow((val - avg1), 2.0));
                var sumSqr2 = y.Sum(val => Math.Pow((val - avg2), 2.0));
                var result = (sumSqr1 * sumSqr2) == 0 ? 0 : sum1 / Math.Sqrt(sumSqr1 * sumSqr2);
                return result;
            }
            catch (Exception)
            {

                return 0;
            }

        }

        private void CheckColumn(DataColumnCollection dataColumnCollection)
        {
            KeyValuePair<string, int>[] pairs = axisX.Concat(axisY).ToArray();

            foreach (KeyValuePair<string, int> item in pairs)
            {
                string colunm = item.Key.ToString();
                if (!dataColumnCollection.Contains(colunm))
                {
                    throw new Exception("The column does not exist");
                }
            }
        }
    }
}
