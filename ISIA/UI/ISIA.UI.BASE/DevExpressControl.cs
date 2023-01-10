using DevExpress.XtraGrid.Views.Grid;
using System.Windows.Forms;
using System.Data;
using System;
using DevExpress.XtraEditors;
using System.Drawing;
using System.Collections;
using System.IO;
using TAP.UIControls.Charts;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Xml;
using TAP.Remoting.Messages;
using TAP.Remoting.Client;

namespace ISIA.UI.BASE
{
    public class DevExpressControl
    {        
        public class DateTimeFormat : IFormatProvider, ICustomFormatter
        {
            public string FormatDataTimeStr(string timeStr)
            {
                if (timeStr.Length == 14)
                {
                    return DateTime.ParseExact(timeStr, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy/MM/dd HH:mm:ss");
                }
                if (timeStr.Length == 8)
                {
                    return DateTime.ParseExact(timeStr, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy/MM/dd");
                }
                else
                {
                    return "";
                }
            }

            public object GetFormat(Type formatType)
            {
                if (formatType == typeof(ICustomFormatter))
                {
                    return this;
                }
                else return null;
            }

            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                return FormatDataTimeStr(arg.ToString());
            }
        }
    }
}
