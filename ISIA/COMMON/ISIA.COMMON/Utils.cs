using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIA.COMMON
{
    public static class Utils
    {        
        /// <summary>
        /// Split, Merge Query String. AA/BB/CC or AA,BB,CC >> 'AA','BB','CC'
        /// </summary>
        /// <param name="str">ex)AA/BB/CC or AA,BB,CC</param>
        /// <param name="splitValue">Split char value ex) '/', ','</param>
        /// <returns></returns>
        public static string MakeSqlQueryIn(string str, char splitValue)
        {
            string strTemp = string.Empty;

            if (str.Contains(splitValue))
            {
                string[] sArray = str.Split(splitValue);

                for (int j = 0; j < sArray.Length; j++)
                {
                    if (sArray[j] != "")
                    {
                        strTemp = strTemp + "'" + sArray[j].Trim(' ') + "' ,";
                    }
                }
                strTemp = strTemp.Substring(0, strTemp.Length - 1);
            }
            else
            {
                strTemp = "'" + str + "'";
            }
            return strTemp;            
        }
        
        public static string MakeSqlQueryIn2(string str)
        {
            string strTemp = string.Empty;


            if (str.Contains(","))
            {
                string[] sArray = str.Split(',');

                for (int j = 0; j < sArray.Length; j++)
                {
                    if (sArray[j] != "")
                    {
                        strTemp = strTemp + "'" + sArray[j].Trim(' ') + "' ,";
                    }
                }
                strTemp = strTemp.Substring(0, strTemp.Length - 1);
            }
            else
            {
                strTemp = "'" + str + "'";
            }
            return strTemp;
        }

        public static string MakeSqlQueryIn2(List<object> str)
        {
            string strTemp = string.Empty;


            if (str.Count>=2)
            {

                for (int j = 0; j < str.Count; j++)
                {
                    if (str[j].ToString() != "")
                    {
                        if (str[j].ToString().Contains("'"))
                        {
                            string specialStr =str[j].ToString();
                            StringBuilder sb = new StringBuilder(specialStr);
                            sb.Insert(specialStr.IndexOf("'"),"'");
                            strTemp = strTemp + "'" + sb.ToString().Trim(' ') + "' ,";
                            continue;
                        }
                        strTemp = strTemp + "'" + str[j].ToString().Trim(' ') + "' ,";
                    }
                }
                strTemp = strTemp.Substring(0, strTemp.Length - 1);
            }
            else
            {
                if (str[0].ToString().Contains("'"))
                {
                    string specialStr = str[0].ToString();
                    StringBuilder sb = new StringBuilder(specialStr);
                    sb.Insert(specialStr.IndexOf("'"), "'");
                    strTemp = strTemp + "'" + sb.ToString().Trim(' ') + "' ,";
                }
                strTemp = "'" + str[0].ToString() + "'";
            }
            return strTemp;
        }

        public static DataSet DataRowToDataSet(DataRow dr)
        {

            if (dr == null) return null;

            DataSet tmpds = new DataSet();

            DataTable tmpdt = dr.Table.Clone(); 
            tmpdt.ImportRow(dr);
            tmpds.Tables.Add(tmpdt);

            return tmpds;
        }


        public static DataSet DataTableToDataSet(DataTable dt)
        {

            if (dt == null) return null;

            DataSet tmpds = new DataSet();

            tmpds.Tables.Add(dt);

            return tmpds;
        }


        public static DataSet ArrayToDataSet(string[] tmpArr, string colName)
        {
            if (tmpArr == null || tmpArr.Length == 0) return null;

            DataSet tmpds = new DataSet();
            DataTable tmpdt = new DataTable();
            tmpdt.Columns.Add(colName, typeof(string));

            foreach (string item in tmpArr)
            {
                tmpdt.Rows.Add(item);
            }

            tmpds.Tables.Add(tmpdt);

            return tmpds;
        }

        public static List<T> DataTableToList<T>(this DataTable dataTable) where T : new()
        {
            var dataList = new List<T>();

            //Define what attributes to be read from the class
            const System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase;

            //Read Attribute Names and Types
            var objFieldNames = typeof(T).GetProperties(flags).Cast<System.Reflection.PropertyInfo>().
                Select(item => new
                {
                    Name = item.Name,
                    Type = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType
                }).ToList();

            //Read Datatable column names and types
            var dtlFieldNames = dataTable.Columns.Cast<DataColumn>().
                Select(item => new
                {
                    Name = item.ColumnName,
                    Type = item.DataType
                }).ToList();

            foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
            {
                var classObj = new T();

                foreach (var dtField in dtlFieldNames)
                {
                    System.Reflection.PropertyInfo propertyInfos = classObj.GetType().GetProperty(dtField.Name, flags);

                    var field = objFieldNames.Find(x => x.Name.ToUpper() == dtField.Name.ToUpper());

                    if (field != null)
                    {
                        if (propertyInfos.PropertyType == typeof(DateTime))
                        {
                            propertyInfos.SetValue
                            (classObj, convertToDateTime(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(Nullable<DateTime>))
                        {
                            propertyInfos.SetValue
                            (classObj, convertToDateTime(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(int))
                        {
                            propertyInfos.SetValue
                            (classObj, ConvertToInt(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(long))
                        {
                            propertyInfos.SetValue
                            (classObj, ConvertToLong(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(decimal))
                        {
                            propertyInfos.SetValue
                            (classObj, ConvertToDecimal(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(String))
                        {
                            if (dataRow[dtField.Name].GetType() == typeof(DateTime))
                            {
                                propertyInfos.SetValue
                                (classObj, ConvertToDateString(dataRow[dtField.Name]), null);
                            }
                            else
                            {
                                propertyInfos.SetValue
                                (classObj, ConvertToString(dataRow[dtField.Name]), null);
                            }
                        }
                        else
                        {
                            propertyInfos.SetValue
                                (classObj, Convert.ChangeType(dataRow[dtField.Name], propertyInfos.PropertyType), null);
                        }
                    }
                }
                dataList.Add(classObj);
            }
            return dataList;
        }
        public static DateTime? ChenageDateTime(string str)
        {

            if (str.Length == 14)
            {
                return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
            }
            if (str.Length > 14)
            {
                return DateTime.ParseExact(str.Substring(0,14), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
            }
            if (str.Length == 8)
            {
                return DateTime.ParseExact(str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            }
            else
            {
                return null;
            }

        }

        public static T DataRowToList<T>(this DataRow dataRow) where T : new()
        {
            //Define what attributes to be read from the class
            const System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase;

            //Read Attribute Names and Types
            var objFieldNames = typeof(T).GetProperties(flags).Cast<System.Reflection.PropertyInfo>().
                Select(item => new
                {
                    Name = item.Name,
                    Type = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType
                }).ToList();

            //Read Datatable column names and types
            
            var dtlFieldNames = dataRow.Table.Columns.Cast<DataColumn>().
                Select(item => new
                {
                    Name = item.ColumnName,
                    Type = item.DataType
                }).ToList();

                var classObj = new T();

                foreach (var dtField in dtlFieldNames)
                {
                    System.Reflection.PropertyInfo propertyInfos = classObj.GetType().GetProperty(dtField.Name, flags);

                    var field = objFieldNames.Find(x => x.Name.ToUpper() == dtField.Name.ToUpper());

                    if (field != null)
                    {

                        if (propertyInfos.PropertyType == typeof(DateTime))
                        {
                            propertyInfos.SetValue
                            (classObj, convertToDateTime(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(Nullable<DateTime>))
                        {
                            propertyInfos.SetValue
                            (classObj, convertToDateTime(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(int))
                        {
                            propertyInfos.SetValue
                            (classObj, ConvertToInt(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(long))
                        {
                            propertyInfos.SetValue
                            (classObj, ConvertToLong(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(decimal))
                        {
                            propertyInfos.SetValue
                            (classObj, ConvertToDecimal(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(String))
                        {
                            if (dataRow[dtField.Name].GetType() == typeof(DateTime))
                            {
                                propertyInfos.SetValue
                                (classObj, ConvertToDateString(dataRow[dtField.Name]), null);
                            }
                            else
                            {
                                propertyInfos.SetValue
                                (classObj, ConvertToString(dataRow[dtField.Name]), null);
                            }
                        }
                        else
                        {
                            propertyInfos.SetValue
                                (classObj, Convert.ChangeType(dataRow[dtField.Name], propertyInfos.PropertyType), null);
                        }
                }                
            }
            return classObj;
        }



        private static string ConvertToDateString(object date)
        {
            if (date == null)
                return string.Empty;

            return date == null ? string.Empty : Convert.ToDateTime(date).ConvertDate();
        }

        private static string ConvertToString(object value)
        {
            return Convert.ToString(ReturnEmptyIfNull(value));
        }

        private static int ConvertToInt(object value)
        {
            return Convert.ToInt32(ReturnZeroIfNull(value));
        }

        private static long ConvertToLong(object value)
        {
            return Convert.ToInt64(ReturnZeroIfNull(value));
        }

        private static decimal ConvertToDecimal(object value)
        {
            return Convert.ToDecimal(ReturnZeroIfNull(value));
        }

        private static DateTime convertToDateTime(object date)
        {
            return Convert.ToDateTime(ReturnDateTimeMinIfNull(date));
        }

        public static string ConvertDate(this DateTime datetTime, bool excludeHoursAndMinutes = false)
        {
            if (datetTime != DateTime.MinValue)
            {
                if (excludeHoursAndMinutes)
                    return datetTime.ToString("yyyy-MM-dd");
                return datetTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            return null;
        }
        public static object ReturnEmptyIfNull(this object value)
        {
            if (value == DBNull.Value)
                return string.Empty;
            if (value == null)
                return string.Empty;
            return value;
        }
        public static object ReturnZeroIfNull(this object value)
        {
            if (value == DBNull.Value)
                return 0;
            if (value == null)
                return 0;
            return value;
        }
        public static object ReturnDateTimeMinIfNull(this object value)
        {
            if (value == DBNull.Value)
                return DateTime.MinValue;
            if (value == null)
                return DateTime.MinValue;
            return DateTime.ParseExact(value.ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture); ;
        }


    }
}
