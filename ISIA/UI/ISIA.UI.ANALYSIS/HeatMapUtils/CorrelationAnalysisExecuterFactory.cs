using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analysis.Correlation
{
   public class CorrelationAnalysisExecuterFactory
    {
        public static CorrelationExecuter GetDefaultExecuter(Config config)
        {
            CorrelationExecuter correlationExecuter = new CorrelationExecuter();
            //create and config executer
            correlationExecuter.DataSet = config.DataSet;
            correlationExecuter.AxisX = generateAxisDic(config.AxisItems);
            correlationExecuter.AxisY = generateAxisDic(config.AxisItems);
            correlationExecuter.DataMap = new double[config.AxisItems.Length, config.AxisItems.Length];
           
            return correlationExecuter;
            

        }
        public static SortedDictionary<string, int> generateAxisDic(string[] items)
        {
            SortedDictionary<string, int> keyValuePairs = new SortedDictionary<string, int>();
            int count = 0;
            foreach (string item in items)
            {
                keyValuePairs.Add(item, count++);
            }
            return keyValuePairs;
        }

    }
}
