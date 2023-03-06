using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analysis.Correlation
{
    public class Config
    {
        private static string projectDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

        private DataSet dataSet = null;

        private string[] axisItems;

        //parampair暂时用不到
        private List<ParmPairs> parmPairsList = new List<ParmPairs>();
        public DataSet DataSet { get => dataSet; set => dataSet = value; }
        public List<ParmPairs> ParmPairsList { get => parmPairsList; set => parmPairsList = value; }
        public string[] AxisItems { get => axisItems; set => axisItems = value; }

        public Config Add(ParmPairs parm)
        {
            parmPairsList.Add(parm);
            return this;
        }

        public Config(DataSet ds)
        {

            this.DataSet = ds;
            DataColumnCollection dataColumnCollection = DataSet.Tables[0].Columns;
            List<string> colunms = new List<string>();
            foreach (DataColumn dataColumn in dataColumnCollection)
            {
                colunms.Add(dataColumn.ColumnName);
            }
            AxisItems = colunms.ToArray();
        }
        public Config(DataSet ds, List<string> columns)
        {
            this.DataSet = ds;
            AxisItems = columns.ToArray();
            foreach (string str in AxisItems)
            {
                str.Trim();
            }
        }
    }
}
