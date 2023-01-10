using GemBox.Spreadsheet;
using NPOI.SS.Formula.Functions;
using NPOI.SS.Formula.PTG;
using NPOI.Util;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Utilities.Collections;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using TAP.Base.Office;
using TAP.Data.DataBase.Communicators;
using TAP.Models.SystemBasic;
using static NPOI.HSSF.Util.HSSFColor;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Excel = Microsoft.Office.Interop.Excel;

namespace ISIA.SERVICE
{
    class DailyLamaRatio
    {
        public string EqCompute()
        {
            GetExcelISheet(filepath, sheetname);

            GetLama();

            SheetStyle();

            SaveToExcel();

            return Savefilepath;
        }
        #region Feild            
        private TimeSpan timeSpan;
        FileStream fs = null;
        Workbook workbook = null;
        Worksheet xSheet = null;
        string filepath = @"D:\Report\DailyEmail\EQUIPMENT LAMA RATIO.XLSX";
        string Savefilepath = @"D:\Report\DailyEmail\" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "\\EQUIPMENT LAMA RATIO - " + DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + ".XLSX";
        string SavefilepathHtml = @"D:\Report\DailyEmail\" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "\\EQUIPMENT LAMA RATIO - " + DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + ".html";
        string sheetname = "Sheet1";
        DBCommunicator db = new DBCommunicator();
        #endregion

        #region Method

        public void SheetStyle()
        {
            StringBuilder tmpSql = new StringBuilder();

            tmpSql.Append(" SELECT LINE, B.AREA, OUTPUT, READ_QTY, RATE, TARGET, MON_OUTPUT, MON_READ, MON_RATE FROM ( ");
            tmpSql.Append("SELECT LINE, AREA, SUM(REAL_TOTAL) AS OUTPUT, SUM(OK_QTY) AS READ_QTY,DECODE(SUM(REAL_TOTAL),0,0,ROUND((SUM(OK_QTY) / SUM(REAL_TOTAL)),5)) AS RATE, 0.9 AS TARGET ");
            tmpSql.Append(" , (SELECT SUM(O.REAL_TOTAL) FROM TAPEQP_LAMASUMMARY O WHERE SUBSTR(O.REPORT_DATE, 0, 6) = TO_CHAR(SYSDATE - 1, 'YYYYMM') AND O.LINE = DD.LINE AND O.AREA = DD.AREA) AS MON_OUTPUT ");
            tmpSql.Append(" , (SELECT SUM(O.OK_QTY) FROM TAPEQP_LAMASUMMARY O WHERE SUBSTR(O.REPORT_DATE, 0, 6) = TO_CHAR(SYSDATE - 1, 'YYYYMM') AND O.LINE = DD.LINE AND O.AREA = DD.AREA) AS MON_READ ");
            tmpSql.Append(" , (SELECT DECODE(SUM(O.REAL_TOTAL), 0, 0, ROUND((SUM(O.OK_QTY) / SUM(O.REAL_TOTAL)), 5)) FROM TAPEQP_LAMASUMMARY O WHERE SUBSTR(O.REPORT_DATE, 0, 6) = TO_CHAR(SYSDATE - 1, 'YYYYMM') AND O.LINE = DD.LINE AND O.AREA = DD.AREA) AS MON_RATE ");
            tmpSql.Append(" FROM TAPEQP_LAMASUMMARY DD ");
            tmpSql.Append(" WHERE REPORT_DATE = TO_CHAR(SYSDATE - 1, 'YYYYMMDD') ");
            tmpSql.Append(" GROUP BY LINE, AREA ) A,");
            tmpSql.Append(" (SELECT AREA, SEQ FROM( ");
            tmpSql.Append(" SELECT 'WI' as AREA, 0 as seq FROM DUAL ");
            tmpSql.Append(" union all ");
            tmpSql.Append(" SELECT 'TX' as AREA, 1 as seq FROM DUAL ");
            tmpSql.Append(" union all ");
            tmpSql.Append(" SELECT 'DF' as AREA, 2 as seq FROM DUAL ");
            tmpSql.Append(" union all ");
            tmpSql.Append(" SELECT 'LD' as AREA, 3 as seq FROM DUAL ");
            tmpSql.Append(" union all ");
            tmpSql.Append(" SELECT 'PS' as AREA, 4 as seq FROM DUAL ");
            tmpSql.Append(" union all ");
            tmpSql.Append(" SELECT 'OX' as AREA, 5 as seq FROM DUAL ");
            tmpSql.Append(" union all ");
            tmpSql.Append(" SELECT 'RP' as AREA, 6 as seq FROM DUAL ");
            tmpSql.Append(" union all ");
            tmpSql.Append(" SELECT 'PE' as AREA, 7 as seq FROM DUAL ");
            tmpSql.Append(" union all ");
            tmpSql.Append(" SELECT 'LC' as AREA, 8 as seq FROM DUAL ");
            tmpSql.Append(" union all ");
            tmpSql.Append(" SELECT 'SP' as AREA, 9 as seq FROM DUAL ");
            tmpSql.Append(" union all ");
            tmpSql.Append(" SELECT 'TS' as AREA, 10 as seq FROM DUAL ) ) B ");
            tmpSql.Append(" WHERE A.AREA = B.AREA ");
            tmpSql.Append(" ORDER BY A.LINE, B.SEQ ");

            DataTable temp = db.Select(tmpSql.ToString()).Tables[0];

            xSheet.InsertDataTable(temp, false, 8, 2);
            xSheet.Range["D6"].Value = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");

            StringBuilder tmpSql2 = new StringBuilder();

            tmpSql2.Append(" SELECT LINE, B.AREA, EQUIPMENT, SIDE, OUTPUT, READ_QTY, RATE, TARGET, MON_OUTPUT, MON_READ, MON_RATE FROM ( ");
            tmpSql2.Append(" SELECT LINE, AREA, NAME as EQUIPMENT, SIDE, SUM(REAL_TOTAL) AS OUTPUT, SUM(OK_QTY) AS READ_QTY, DECODE(SUM(REAL_TOTAL), 0, 0, ROUND((SUM(OK_QTY) / SUM(REAL_TOTAL)), 5)) AS RATE, 0.9 AS TARGET ");
            tmpSql2.Append(", (SELECT SUM(O.REAL_TOTAL) FROM TAPEQP_LAMASUMMARY O WHERE SUBSTR(O.REPORT_DATE, 0, 6) = TO_CHAR(SYSDATE - 1, 'YYYYMM') AND O.LINE = DD.LINE AND O.AREA = DD.AREA AND O.NAME = DD.NAME AND O.SIDE = DD.SIDE) AS MON_OUTPUT ");
            tmpSql2.Append(", (SELECT SUM(O.OK_QTY) FROM TAPEQP_LAMASUMMARY O WHERE SUBSTR(O.REPORT_DATE, 0, 6) = TO_CHAR(SYSDATE - 1, 'YYYYMM') AND O.LINE = DD.LINE AND O.AREA = DD.AREA AND O.NAME = DD.NAME AND O.SIDE = DD.SIDE) AS MON_READ ");
            tmpSql2.Append(", (SELECT DECODE(SUM(O.REAL_TOTAL), 0, 0, ROUND((SUM(O.OK_QTY) / SUM(O.REAL_TOTAL)), 5)) FROM TAPEQP_LAMASUMMARY O WHERE SUBSTR(O.REPORT_DATE, 0, 6) = TO_CHAR(SYSDATE - 1, 'YYYYMM') AND O.LINE = DD.LINE AND O.AREA = DD.AREA AND O.NAME = DD.NAME AND O.SIDE = DD.SIDE) AS MON_RATE ");
            tmpSql2.Append(" FROM TAPEQP_LAMASUMMARY DD ");
            tmpSql2.Append(" WHERE REPORT_DATE = TO_CHAR(SYSDATE - 1, 'YYYYMMDD') ");
            tmpSql2.Append(" GROUP BY LINE, AREA, NAME ,SIDE ) A,");
            tmpSql2.Append(" (SELECT AREA, SEQ FROM( ");
            tmpSql2.Append(" SELECT 'WI' as AREA, 0 as seq FROM DUAL ");
            tmpSql2.Append(" union all ");
            tmpSql2.Append(" SELECT 'TX' as AREA, 1 as seq FROM DUAL ");
            tmpSql2.Append(" union all ");
            tmpSql2.Append(" SELECT 'DF' as AREA, 2 as seq FROM DUAL ");
            tmpSql2.Append(" union all ");
            tmpSql2.Append(" SELECT 'LD' as AREA, 3 as seq FROM DUAL ");
            tmpSql2.Append(" union all ");
            tmpSql2.Append(" SELECT 'PS' as AREA, 4 as seq FROM DUAL ");
            tmpSql2.Append(" union all ");
            tmpSql2.Append(" SELECT 'OX' as AREA, 5 as seq FROM DUAL ");
            tmpSql2.Append(" union all ");
            tmpSql2.Append(" SELECT 'RP' as AREA, 6 as seq FROM DUAL ");
            tmpSql2.Append(" union all ");
            tmpSql2.Append(" SELECT 'PE' as AREA, 7 as seq FROM DUAL ");
            tmpSql2.Append(" union all ");
            tmpSql2.Append(" SELECT 'LC' as AREA, 8 as seq FROM DUAL ");
            tmpSql2.Append(" union all ");
            tmpSql2.Append(" SELECT 'SP' as AREA, 9 as seq FROM DUAL ");
            tmpSql2.Append(" union all ");
            tmpSql2.Append(" SELECT 'TS' as AREA, 10 as seq FROM DUAL ) ) B ");
            tmpSql2.Append(" WHERE A.AREA = B.AREA ");
            tmpSql2.Append(" ORDER BY A.LINE, B.SEQ, EQUIPMENT, SIDE ");

            DataTable temp2 = db.Select(tmpSql2.ToString()).Tables[0];

            xSheet.InsertDataTable(temp2, false, 34, 2);
            xSheet.Range["F32"].Value = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
        }

        public DataTable GetLama()
        {
            StringBuilder tmpSql = new StringBuilder();

            DataTable dt = new DataTable();
            
            string[] idArray = { "WI-511-Q-01",
                "WI-511-Q-02",
                "WI-511-Q-03", 
                "WI-521-Q-01",
                "WI-521-Q-02",
                "TX-511-A-01",
                "TX-511-A-02",
                "TX-511-A-03",
                "TX-511-A-04", 
                "TX-521-A-01", 
                "TX-521-A-02",
                "DF-511-A-01",
                "DF-511-A-02",
                "DF-521-A-01",
                "DF-521-A-02",
                "PS-511-A-01",
                "PS-511-A-02",
                "PS-511-A-03",
                "PS-511-A-04",
                "PS-511-A-05",
                "PS-511-A-06",
                "PS-521-A-01",
                "PS-521-A-02",
                "PS-521-A-03",
                "PS-521-A-04",
                "OX-511-A-01",
                "OX-511-A-02",
                "OX-521-A-01",
                "LD-511-M-01",
                "LD-511-M-02",
                "LD-511-M-03",
                "LD-521-M-01",
                "LD-521-M-02",
                "LC-511-M-01",
                "LC-511-M-02",
                "LC-521-M-01",
                "LC-521-M-02",
                "LC-521-M-03",
                "LC-521-M-04",
                "SP-511-M-01",
                "SP-511-M-02",
                "SP-521-M-01",
                "SP-521-M-02",
                "SP-521-M-03",
                "SP-521-M-04",
                "PE-511-A-01",
                "PE-511-A-02",
                "PE-521-A-01",
                "PE-521-A-02",
                "PE-521-A-03",
                "RP-511-A-01",
                "RP-511-A-02",
                "RP-521-A-01",
                "RP-521-A-02",
                "RP-521-A-03",
                "TS-511-Q-01",
                "TS-511-Q-02",
                "TS-511-Q-03",
                "TS-521-Q-01",
                "TS-521-Q-02",
                "TS-521-Q-03",
                "TS-521-Q-04"
            };
            //WI
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, TOTAL, OK_QTY, ROUND(OK_QTY / REAL_TOTAL, 5) AS OK_RATIO, REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, COUNT(*) AS TOTAL, SUM(OK_LAMA) AS OK_QTY, (SELECT MAX(TO_NUMBER(PARAMETER_VALUE)) FROM TAPIFTB_WIEQP S WHERE S.REPORT_DATE = A.REPORT_DATE ");
            tmpSql.Append("AND S.SIDE = A.SIDE ");
            tmpSql.Append("AND S.EQUIPMENT = A.EQUIPMENT ");
            tmpSql.Append("AND S.SHIFT = A.SHIFT ");
            tmpSql.Append("AND UPPER(S.PARAMETER_NAME) = 'TOTALQTYIN') AS REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT  DISTINCT(PARAMETER_VALUE), EQUIPMENT, SIDE, SHIFT, REPORT_DATE, CASE WHEN SUBSTR(PARAMETER_VALUE, 1, 1) = '3' THEN 1 ELSE 0 END AS OK_LAMA FROM TAPIFTB_WIEQP ");
            tmpSql.Append("WHERE PARAMETER_NAME = 'LAMA_ID' ");
            tmpSql.Append("AND REPORT_DATE = TO_CHAR(SYSDATE - 1, 'YYYYMMDD') ");
            tmpSql.Append(") A ");
            tmpSql.Append("GROUP BY EQUIPMENT, SIDE, SHIFT, REPORT_DATE)  ");
            tmpSql.Append("ORDER BY EQUIPMENT, SIDE ");

            DataTable temp = db.Select(tmpSql.ToString()).Tables[0];
            dt = temp;
            tmpSql.Clear();
            Console.WriteLine("WI SELECT OK");
            //TX
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, TOTAL, OK_QTY, ROUND(OK_QTY / REAL_TOTAL, 5) AS OK_RATIO, REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, COUNT(*) AS TOTAL, SUM(OK_LAMA) AS OK_QTY, FN_GET_TXOUTPUTQTY(A.REPORT_DATE, A.EQUIPMENT,A.SHIFT) ");
            tmpSql.Append(" AS REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT  DISTINCT(PARAMETER_VALUE), EQUIPMENT, 'ALL' AS SIDE, SHIFT, REPORT_DATE, CASE WHEN SUBSTR(PARAMETER_VALUE, 1, 1) = '3' THEN 1 ELSE 0 END AS OK_LAMA FROM TAPIFTB_TXROBO ");
            tmpSql.Append("WHERE UPPER(PARAMETER_NAME) IN('LAMA_A', 'LAMA_B', 'LAMA_C', 'LALAME', 'LBLAME', 'LCLAME') ");
            tmpSql.Append("AND REPORT_DATE = TO_CHAR(SYSDATE - 1, 'YYYYMMDD') ");
            tmpSql.Append(") A ");
            tmpSql.Append("GROUP BY EQUIPMENT, SIDE, SHIFT, REPORT_DATE)  ");
            tmpSql.Append("ORDER BY EQUIPMENT, SIDE ");

            DataTable temp1 = db.Select(tmpSql.ToString()).Tables[0];
            dt.Merge(temp1);
            tmpSql.Clear();
            Console.WriteLine("TX SELECT OK");

            //DF
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, TOTAL, OK_QTY, ROUND(OK_QTY / REAL_TOTAL, 5) AS OK_RATIO, REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, COUNT(*) AS TOTAL, SUM(OK_LAMA) AS OK_QTY, FN_GET_DFOUTPUTQTY(A.REPORT_DATE, A.EQUIPMENT,A.SHIFT) ");
            tmpSql.Append(" AS REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT  DISTINCT(PARAMETER_VALUE), EQUIPMENT, 'ALL' AS SIDE, SHIFT, REPORT_DATE, CASE WHEN SUBSTR(PARAMETER_VALUE, 1, 1) = '3' THEN 1 ELSE 0 END AS OK_LAMA FROM TAPIFTB_DFROBO ");
            tmpSql.Append("WHERE UPPER(PARAMETER_NAME)  = 'LAMA_ID' ");
            tmpSql.Append("AND REPORT_DATE = TO_CHAR(SYSDATE - 1, 'YYYYMMDD') ");
            tmpSql.Append(") A ");
            tmpSql.Append("GROUP BY EQUIPMENT, SIDE, SHIFT, REPORT_DATE)  ");
            tmpSql.Append("ORDER BY EQUIPMENT, SIDE ");

            DataTable temp9 = db.Select(tmpSql.ToString()).Tables[0];
            dt.Merge(temp9);
            tmpSql.Clear();
            Console.WriteLine("DF SELECT OK");

            //LD
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, TOTAL, OK_QTY, ROUND(OK_QTY / REAL_TOTAL, 5) AS OK_RATIO, REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, COUNT(*) AS TOTAL, SUM(OK_LAMA) AS OK_QTY, (SELECT MAX(TO_NUMBER(PARAMETER_VALUE)) FROM TAPIFTB_LDEQP S WHERE S.REPORT_DATE = A.REPORT_DATE ");
            tmpSql.Append("AND S.SIDE = A.SIDE ");
            tmpSql.Append("AND S.EQUIPMENT = A.EQUIPMENT ");
            tmpSql.Append("AND S.SHIFT = A.SHIFT ");
            tmpSql.Append("AND UPPER(S.PARAMETER_NAME) = 'PRODUCTION') AS REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT DISTINCT(PARAMETER_VALUE),  EQUIPMENT, SIDE, SHIFT, REPORT_DATE, CASE WHEN SUBSTR(PARAMETER_VALUE, 1, 1) = '3' THEN 1 ELSE 0 END AS OK_LAMA FROM TAPIFTB_LDEQP ");
            tmpSql.Append("WHERE PARAMETER_NAME = 'LOADLAMAID' ");
            tmpSql.Append("AND REPORT_DATE = TO_CHAR(SYSDATE - 1, 'YYYYMMDD') ");
            tmpSql.Append(") A ");
            tmpSql.Append("GROUP BY EQUIPMENT, SIDE, SHIFT, REPORT_DATE)  ");
            tmpSql.Append("ORDER BY EQUIPMENT, SIDE ");

            DataTable temp2 = db.Select(tmpSql.ToString()).Tables[0];
            dt.Merge(temp2);
            tmpSql.Clear();
            Console.WriteLine("LD SELECT OK");
            //PS
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, TOTAL, OK_QTY, ROUND(OK_QTY / REAL_TOTAL, 5) AS OK_RATIO, REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, COUNT(*) AS TOTAL, SUM(OK_LAMA) AS OK_QTY, (SELECT MAX(TO_number(PARAMETER_VALUE)) FROM TAPIFTB_PSROBO S WHERE S.REPORT_DATE = A.REPORT_DATE ");
            tmpSql.Append("AND S.EQUIPMENT = A.EQUIPMENT ");
            tmpSql.Append("AND S.SHIFT = A.SHIFT ");
            tmpSql.Append("AND UPPER(S.PARAMETER_NAME) = 'NOW_OUTPUT') AS REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT  DISTINCT(PARAMETER_VALUE), EQUIPMENT, 'ALL' AS SIDE, SHIFT, REPORT_DATE, CASE WHEN SUBSTR(PARAMETER_VALUE, 1, 1) = '3' THEN 1 ELSE 0 END AS OK_LAMA FROM TAPIFTB_PSROBO ");
            tmpSql.Append("WHERE PARAMETER_NAME IN('LAMA_A', 'LAMA_B', 'LAMA_A1', 'LAMA_B1') ");
            tmpSql.Append("AND REPORT_DATE = TO_CHAR(SYSDATE - 1, 'YYYYMMDD') ");
            tmpSql.Append(") A ");
            tmpSql.Append("GROUP BY EQUIPMENT, SIDE, SHIFT, REPORT_DATE)  ");
            tmpSql.Append("ORDER BY EQUIPMENT, SIDE ");

            DataTable temp3 = db.Select(tmpSql.ToString()).Tables[0];
            dt.Merge(temp3);
            tmpSql.Clear();
            Console.WriteLine("PS SELECT OK");

            //OX
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, TOTAL, OK_QTY, ROUND(OK_QTY / REAL_TOTAL, 5) AS OK_RATIO, REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, COUNT(*) AS TOTAL, SUM(OK_LAMA) AS OK_QTY, FN_GET_OXOUTPUTQTY(A.REPORT_DATE, A.EQUIPMENT,A.SHIFT) ");
            tmpSql.Append(" AS REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT  DISTINCT(PARAMETER_VALUE), EQUIPMENT, 'ALL' AS SIDE, SHIFT, REPORT_DATE, CASE WHEN SUBSTR(PARAMETER_VALUE, 1, 1) = '3' THEN 1 ELSE 0 END AS OK_LAMA FROM TAPIFTB_OXROBO ");
            tmpSql.Append("WHERE UPPER(PARAMETER_NAME)  = 'LAMA_ID' ");
            tmpSql.Append("AND REPORT_DATE = TO_CHAR(SYSDATE - 1, 'YYYYMMDD') ");
            tmpSql.Append(") A ");
            tmpSql.Append("GROUP BY EQUIPMENT, SIDE, SHIFT, REPORT_DATE)  ");
            tmpSql.Append("ORDER BY EQUIPMENT, SIDE ");

            DataTable temp10 = db.Select(tmpSql.ToString()).Tables[0];
            dt.Merge(temp10);
            tmpSql.Clear();
            Console.WriteLine("OX SELECT OK");

            //RP
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, TOTAL, OK_QTY, ROUND(OK_QTY / REAL_TOTAL, 5) AS OK_RATIO, REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, COUNT(*) AS TOTAL, SUM(OK_LAMA) AS OK_QTY, (SELECT MAX(TO_number(PARAMETER_VALUE)) FROM TAPIFTB_RPROBO S WHERE S.REPORT_DATE = A.REPORT_DATE ");
            tmpSql.Append("AND S.EQUIPMENT = A.EQUIPMENT ");
            tmpSql.Append("AND S.SHIFT = A.SHIFT ");
            tmpSql.Append("AND UPPER(S.PARAMETER_NAME) in ('OUTPUTDAY','OUTPUTNIGHT')) AS REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT  DISTINCT(PARAMETER_VALUE), EQUIPMENT, 'ALL' AS SIDE, SHIFT, REPORT_DATE, CASE WHEN SUBSTR(PARAMETER_VALUE, 1, 1) = '3' THEN 1 ELSE 0 END AS OK_LAMA FROM TAPIFTB_RPROBO ");
            tmpSql.Append("WHERE (PARAMETER_NAME LIKE 'arr_FirstloadCasWaferID%' or PARAMETER_NAME LIKE 'arr_SecondloadCasWaferID%') ");
            tmpSql.Append("AND REPORT_DATE = TO_CHAR(SYSDATE - 1, 'YYYYMMDD') ");
            tmpSql.Append(") A ");
            tmpSql.Append("GROUP BY EQUIPMENT, SIDE, SHIFT, REPORT_DATE)  ");
            tmpSql.Append("ORDER BY EQUIPMENT, SIDE ");

            DataTable temp4 = db.Select(tmpSql.ToString()).Tables[0];
            dt.Merge(temp4);
            tmpSql.Clear();
            Console.WriteLine("RP SELECT OK");
            //PE
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, TOTAL, OK_QTY, ROUND(OK_QTY / REAL_TOTAL, 5) AS OK_RATIO, REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, COUNT(*) AS TOTAL, SUM(OK_LAMA) AS OK_QTY, (SELECT MAX(TO_number(PARAMETER_VALUE)) FROM TAPIFTB_PEROBO S WHERE S.REPORT_DATE = A.REPORT_DATE ");
            tmpSql.Append("AND S.EQUIPMENT = A.EQUIPMENT ");
            tmpSql.Append("AND S.SHIFT = A.SHIFT ");
            tmpSql.Append("AND UPPER(S.PARAMETER_NAME) in ('OUTPUTDAY','OUTPUTNIGHT')) AS REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT  DISTINCT(PARAMETER_VALUE), EQUIPMENT, 'ALL' AS SIDE, SHIFT, REPORT_DATE, CASE WHEN SUBSTR(PARAMETER_VALUE, 1, 1) = '3' THEN 1 ELSE 0 END AS OK_LAMA FROM TAPIFTB_PEROBO ");
            tmpSql.Append("WHERE (PARAMETER_NAME LIKE 'arr_FirstloadCasWaferID%' or PARAMETER_NAME LIKE 'arr_SecondloadCasWaferID%') ");
            tmpSql.Append("AND REPORT_DATE = TO_CHAR(SYSDATE - 1, 'YYYYMMDD') ");
            tmpSql.Append(") A ");
            tmpSql.Append("GROUP BY EQUIPMENT, SIDE, SHIFT, REPORT_DATE)  ");
            tmpSql.Append("ORDER BY EQUIPMENT, SIDE ");

            DataTable temp5 = db.Select(tmpSql.ToString()).Tables[0];
            dt.Merge(temp5);
            tmpSql.Clear();
            Console.WriteLine("PE SELECT OK");
            //LC
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, TOTAL, OK_QTY, ROUND(OK_QTY / REAL_TOTAL, 5) AS OK_RATIO, REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, COUNT(*) AS TOTAL, SUM(OK_LAMA) AS OK_QTY, (SELECT MAX(TO_number(PARAMETER_VALUE)) FROM TAPIFTB_LCEQP S WHERE S.REPORT_DATE = A.REPORT_DATE ");
            tmpSql.Append("AND S.EQUIPMENT = A.EQUIPMENT ");
            tmpSql.Append("AND S.SHIFT = A.SHIFT ");
            tmpSql.Append("AND UPPER(S.PARAMETER_NAME) = 'PRODUCTION') AS REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT  DISTINCT(PARAMETER_VALUE), EQUIPMENT, 'ALL' AS SIDE, SHIFT, REPORT_DATE, CASE WHEN SUBSTR(PARAMETER_VALUE, 1, 1) = '3' THEN 1 ELSE 0 END AS OK_LAMA FROM TAPIFTB_LCEQP ");
            tmpSql.Append("WHERE PARAMETER_NAME = 'LOADLAMAID' ");
            tmpSql.Append("AND REPORT_DATE = TO_CHAR(SYSDATE - 1, 'YYYYMMDD') ");
            tmpSql.Append(") A ");
            tmpSql.Append("GROUP BY EQUIPMENT, SIDE, SHIFT, REPORT_DATE)  ");
            tmpSql.Append("ORDER BY EQUIPMENT, SIDE ");

            DataTable temp6 = db.Select(tmpSql.ToString()).Tables[0];
            dt.Merge(temp6);
            tmpSql.Clear();
            Console.WriteLine("LC SELECT OK");
            //SP
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, TOTAL, OK_QTY, ROUND(OK_QTY / REAL_TOTAL, 5) AS OK_RATIO, REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, COUNT(*) AS TOTAL, SUM(OK_LAMA) AS OK_QTY, (SELECT MAX(TO_number(PARAMETER_VALUE)) FROM TAPIFTB_SPEQP S WHERE S.REPORT_DATE = A.REPORT_DATE ");
            tmpSql.Append("AND S.WORKSHOP = A.WORKSHOP ");
            tmpSql.Append("AND S.LINE = A.LINE ");
            tmpSql.Append("AND S.SHIFT = A.SHIFT ");
            tmpSql.Append("AND UPPER(S.PARAMETER_NAME) = 'WAFER_QTY') AS REAL_TOTAL FROM( ");
            tmpSql.Append("SELECT DISTINCT(PARAMETER_VALUE), FN_GET_EQPID_IN_TAPIFTB_SPEQP(WORKSHOP, LINE) AS EQUIPMENT, 'ALL' AS SIDE, SHIFT, REPORT_DATE, LINE, WORKSHOP, CASE WHEN SUBSTR(PARAMETER_VALUE, 1, 1) = '3' THEN 1 ELSE 0 END AS OK_LAMA FROM TAPIFTB_SPEQP ");
            tmpSql.Append("WHERE PARAMETER_NAME = 'LAMAID' ");
            tmpSql.Append("AND REPORT_DATE = TO_CHAR(SYSDATE - 1, 'YYYYMMDD') ");
            tmpSql.Append(") A ");
            tmpSql.Append("GROUP BY EQUIPMENT, SIDE, SHIFT, REPORT_DATE, WORKSHOP, LINE)  ");
            tmpSql.Append("ORDER BY EQUIPMENT, SIDE ");

            DataTable temp7 = db.Select(tmpSql.ToString()).Tables[0];
            dt.Merge(temp7);
            tmpSql.Clear();
            Console.WriteLine("SP SELECT OK");

            //TS
            tmpSql.Append("SELECT REPORT_DATE, SHIFT, EQUIPMENT, SIDE, REAL_TOTAL AS TOTAL, OK_QTY, ROUND(OK_QTY / REAL_TOTAL, 5) AS OK_RATIO, REAL_TOTAL FROM( ");
            tmpSql.Append(" SELECT TO_CHAR (SYSDATE - 1, 'YYYYMMDD') AS REPORT_DATE, SHIFT, EQUIPMENT, 'ALL' AS SIDE, COUNT(WAFER_ID) AS REAL_TOTAL, SUM(DECODE(SUBSTR(WAFER_ID, 0, 1), '3', 1, 0))     AS OK_QTY ");
            tmpSql.Append(" FROM TAPEQP_TS_DBDATA ");
            tmpSql.Append(" WHERE TESTTIMEKEY >= TO_CHAR (SYSDATE - 1, 'YYYYMMDD') || '073000' AND TESTTIMEKEY < TO_CHAR(SYSDATE, 'YYYYMMDD') || '073000' ");
            tmpSql.Append(" GROUP BY REGION, FACILITY, AREA, LINE, EQUIPMENT, SHIFT) ");
            tmpSql.Append(" ORDER BY EQUIPMENT,SIDE");

            DataTable temp8 = db.Select(tmpSql.ToString()).Tables[0];
            dt.Merge(temp8);
            tmpSql.Clear();
            Console.WriteLine("TS SELECT OK");


            Console.WriteLine("SELECT COMPLETE");
            DataTable dt_temp = new DataTable();
            dt_temp.Columns.Add("REPORT_DATE", typeof(string));
            dt_temp.Columns.Add("SHIFT", typeof(string));
            dt_temp.Columns.Add("EQUIPMENT", typeof(string));
            dt_temp.Columns.Add("SIDE", typeof(string));
            dt_temp.Columns.Add("TOTAL", typeof(Int32));
            dt_temp.Columns.Add("OK_QTY", typeof(Int32));
            dt_temp.Columns.Add("OK_RATIO", typeof(Double));
            dt_temp.Columns.Add("REAL_TOTAL", typeof(Int32));

            foreach (string str in idArray)
            {
                if (str.Contains("WI") || str.Contains("LD"))
                {
                    dt_temp.Rows.Add(DateTime.Now.AddDays(-1).ToString("yyyyMMdd"), "D", str, "A", 0, 0, 0, 0);
                    dt_temp.Rows.Add(DateTime.Now.AddDays(-1).ToString("yyyyMMdd"), "D", str, "B", 0, 0, 0, 0);
                    dt_temp.Rows.Add(DateTime.Now.AddDays(-1).ToString("yyyyMMdd"), "N", str, "A", 0, 0, 0, 0);
                    dt_temp.Rows.Add(DateTime.Now.AddDays(-1).ToString("yyyyMMdd"), "N", str, "B", 0, 0, 0, 0);
                }
                else
                {
                    dt_temp.Rows.Add(DateTime.Now.AddDays(-1).ToString("yyyyMMdd"), "D", str, "ALL", 0, 0, 0, 0);
                    dt_temp.Rows.Add(DateTime.Now.AddDays(-1).ToString("yyyyMMdd"), "N", str, "ALL", 0, 0, 0, 0);
                }
            }


            for (int i = 0; i < dt_temp.Rows.Count; i++)
            {
                foreach (DataRow tempdr in dt.Rows)
                {

                    if (dt_temp.Rows[i]["REPORT_DATE"].ToString() == tempdr["REPORT_DATE"].ToString() &&
                        dt_temp.Rows[i]["SHIFT"].ToString() == tempdr["SHIFT"].ToString() &&
                         dt_temp.Rows[i]["EQUIPMENT"].ToString() == tempdr["EQUIPMENT"].ToString() &&
                         dt_temp.Rows[i]["SIDE"].ToString() == tempdr["SIDE"].ToString())
                    {
                        object total = tempdr["TOTAL"];
                        object ok_qty = tempdr["OK_QTY"];
                        object ok_ratio = tempdr["OK_RATIO"];
                        object real_total = tempdr["REAL_TOTAL"];

                        if (string.IsNullOrEmpty(tempdr["TOTAL"].ToString()))
                        {
                            total = 0;
                        }
                        if (string.IsNullOrEmpty(tempdr["OK_QTY"].ToString()))
                        {
                            ok_qty = 0;
                        }
                        if (string.IsNullOrEmpty(tempdr["OK_RATIO"].ToString()))
                        {
                            ok_ratio = 0;
                        }
                        if (string.IsNullOrEmpty(tempdr["REAL_TOTAL"].ToString()))
                        {
                            real_total = 0;
                        }

                        dt_temp.Rows[i]["TOTAL"] = total;
                        dt_temp.Rows[i]["OK_QTY"] = ok_qty;
                        dt_temp.Rows[i]["OK_RATIO"] = ok_ratio;
                        dt_temp.Rows[i]["REAL_TOTAL"] = real_total;
                        break;
                    }
                }
            }




            List<string> sqllist = new List<string>();
            foreach(DataRow dr in dt_temp.Rows)
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO TAPEQP_LAMASUMMARY( ");
                sql.Append("AREA, LINE, NAME, REAL_TOTAL, ");
                sql.Append("REGION, FACILITY, REPORT_DATE, ");
                sql.Append("SHIFT, SIDE, TOTAL, OK_QTY, OK_RATIO, INSERTTIME, ");
                sql.Append("UPDATETIME, INSERTUSER, UPDATEUSER) ");
                sql.Append("VALUES( ");
                sql.AppendFormat("'{0}','{1}','{2}',{3}", dr["EQUIPMENT"].ToString().Substring(0, 2), dr["EQUIPMENT"].ToString().Substring(3, 3), dr["EQUIPMENT"].ToString(),dr["REAL_TOTAL"].ToString());
                sql.AppendFormat(",'{0}','{1}','{2}'", "CELL", dr["EQUIPMENT"].ToString().Substring(3, 1), dr["REPORT_DATE"].ToString());
                sql.AppendFormat(",'{0}','{1}',{2}", dr["SHIFT"].ToString(), dr["SIDE"].ToString(), dr["TOTAL"].ToString());
                sql.AppendFormat(",{0},{1}", dr["OK_QTY"].ToString(), dr["OK_RATIO"].ToString());
                sql.Append(",TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS'),null,'Service',null)");

                sqllist.Add(sql.ToString());

                Console.WriteLine(sql.ToString());
            }

            db.Save(sqllist);
            Console.WriteLine("INSERT COMPLETE");
            return dt;
        }
        public Worksheet GetExcelISheet(string filepath, string sheetname)
        {
            fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            if (filepath.ToLower().IndexOf(".xlsx") > 0)
            {
                workbook = new Workbook();

            }
            else if (filepath.ToLower().IndexOf(".xls") > 0)
            {
                workbook = new Workbook();
            }
            fs.Close();
            try
            {
                workbook.LoadFromFile(filepath, true);
                workbook.ActiveSheetIndex = 0;
                xSheet = workbook.Worksheets[sheetname];
                return xSheet;
            }
            catch (Exception EX)
            {
                throw EX;
            }

        }
        private void SaveToExcel()
        {
            try
            {
                ////RUN
                //xSheet.Range["A4"].Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(208)))), ((int)(((byte)(80)))));
                ////BM
                //xSheet.Range["D4"].Style.Color = Color.Red;
                ////PM
                //xSheet.Range["G4"].Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(155)))), ((int)(((byte)(213)))));
                ////CHECK DOWN
                //xSheet.Range["J4"].Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(125)))), ((int)(((byte)(49)))));
                ////IDLE
                //xSheet.Range["M4"].Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
                ////PROCESS
                //xSheet.Range["P4"].Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(204)))));
                ////RD TEST
                //xSheet.Range["S4"].Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(48)))), ((int)(((byte)(160)))));
                ////UTILITY DOWN
                //xSheet.Range["V4"].Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(165)))), ((int)(((byte)(165)))));
                ////MATERIAL TEST
                //xSheet.Range["Y4"].Style.Color = Color.DodgerBlue;
                if (!Directory.Exists(Path.GetDirectoryName(Savefilepath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Savefilepath));
                }
                workbook.CalculateAllValue();
                workbook.SaveToFile(Savefilepath, ExcelVersion.Version2013);
                //workbook.SaveToHtml(SavefilepathHtml);

                workbook.Worksheets[0].SaveToImage(Savefilepath.Replace("XLSX", "Jpeg"));
                workbook.Dispose();

                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

                ExcelFile.Load(Savefilepath).Save(SavefilepathHtml, SaveOptions.HtmlDefault);

                //Excel.Application oXL = new Excel.Application(); //Excel application create
                //oXL.Visible = false; //true 설정하면 엑셀 작업되는 내용이 보인다.
                //oXL.Interactive = false; //false로 설정하면 사용자의 조작에 방해받지 않음.﻿
                //oXL.Interactive = true; //사용자의 조작 허용

                //Excel._Workbook oWB = oXL.Workbooks.Open(Savefilepath);//워크북생성
                //Excel._Worksheet oSheet = (Excel._Worksheet)oWB.ActiveSheet;//시트 가져오기
                //Excel.Range oRng = null; //각종 셀 처리 담당할 변수﻿

                //oSheet.SaveAs(SavefilepathHtml, xlHtml);
                //oWB.Close(false);
                //oXL.Quit();

                //if (oSheet != null)
                //    Marshal.ReleaseComObject(oSheet);
                //if (oWB != null)
                //    Marshal.ReleaseComObject(oWB);
                //if (oXL != null)
                //    Marshal.ReleaseComObject(oXL);
            }
            catch(System.Exception ex)
            {
                ex.ToString();
            }
        }
        #endregion
    }
}
