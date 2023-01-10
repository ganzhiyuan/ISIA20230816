using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using TAP.Data.DataBase.Communicators;
using TAP.Models.SystemBasic;

namespace ISIA.SERVICE
{
    class DailyRatioReropt
    {
        public string EqCompute()
        {
            GetExcelISheet(filepath, sheetname, sheetname2);
            GetEQHistory();
            GetEQ();
            GetEQnoChange();
            GetBMInfo();
            GetBMCompleteList();
            timeSpan = DateTime.Now - DateTime.Now.AddDays(-1);
            totalSpan = timeSpan.TotalSeconds;
            EqOperatingRatio();
            OperatingRatio(42, 44, 2, 29);
            CalcgBmDaily(60, 61);
            CalcCount("BM", 93, 94, 2, 26);
            CalcgTime("BM", 107, 108, 2, 26);
            CalcCount("PM", 121, 122, 2, 26);
            CalcgTime("PM", 135, 136, 2, 26);
            CalcgTime(CHECKDOWN, 149, 150, 2, 26);
            CalcgTime(IDLE, 163, 164, 2, 26);
            CalcgTime(PROCESSTEST, 177, 178, 2, 26);
            CalcgTime(RANDDTEST, 191, 192, 2, 26);
            CalcgTime(UTILITY, 205, 206, 2, 26);
            CalcgTime(MATERIAL, 219, 220, 2, 26);
            CalcgWorkShop(233, 234, 3, 12);
            BmReport(2, 3, 1);


            SpReport();
           	 
            SaveToExcel();

            return Savefilepath;
        }
        #region Feild            
        DataTable EqDtHistory = null;
        DataTable EqDt = null;
        DataTable noChangeEqDt = null;
        DataTable BmInfo = null;
        DataTable BmComList = null;
        private double totalSpan;
        private TimeSpan timeSpan;
        FileStream fs = null;
        Workbook workbook = null;
        Worksheet xSheet = null;
        Worksheet xSheet2 = null;
        Worksheet xSheet3 = null;
        string filepath = @"D:\Report\DailyEmail\EQUIPMENT OPERATING RATIO REPORT.XLSX";
        string Savefilepath = @"D:\Report\DailyEmail\" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "\\EQUIPMENT OPERATING RATIO REPORT(SP IF DATA) - " + DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + ".XLSX";
        string sheetname = "REPORT";
        string sheetname2 = "BM REPORT";
        string sheetname3 = "SP REPORT";

        DBCommunicator db = new DBCommunicator();
        private const string BMWAIT = "BM WAIT";
        private const string BMSTART = "BM START";
        private const string PMSTART = "PM START";
        private const string CHECKDOWN = "CHECK DOWN";
        private const string IDLE = "IDLE";
        private const string PROCESSTEST = "PROCESS TEST";
        private const string PROCESSDOWN = "PROCESS DOWN";
        private const string RANDDTEST = "R&D TEST";
        private const string UTILITY = "UTILITY DOWN";
        private const string MATERIAL = "MATERIAL TEST";
        private const string RUN = "RUN";
        private const string MODIFY = "EQUIPMENT MODIFY";
        DataTable EqOperatingRatioDt = null;
        string startTime = DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + "073000";
        string endTime = DateTime.Now.ToString("yyyyMMdd") + "073000";
        string stTime = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
																																		  
        StatusStartAndEnd EqStatusStartAndEnd = null;
        #endregion

        #region Method
		 public DataTable GetALM_RETGroupBy(string parameter_Name)
        {
            StringBuilder tmpSql = new StringBuilder();
#if MSSQL
            tmpSql.Append(" SELECT  RE.DATA, SUM(RE.D) AS D, SUM(RE.N) AS N FROM (");
            tmpSql.Append(" SELECT   WORKSHOP+LINE AS DATA, ");
            tmpSql.Append(" CASE SHIFT WHEN 'D'    THEN   SUM(PARAMETER_VALUE)  ELSE 0 END AS D,");
            tmpSql.Append(" CASE SHIFT WHEN 'N'    THEN   SUM(PARAMETER_VALUE)  ELSE 0 END AS N");
            tmpSql.Append(" FROM (");
            tmpSql.Append(" SELECT WORKSHOP,CATEGORY1 AS LINE ,SHIFT,");
            tmpSql.Append(" SUM(CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE not LIKE '%[^0-9.]%'");
            tmpSql.Append(" THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END) AS PARAMETER_VALUE");
            tmpSql.Append(" FROM TAPEQDATASUMM  WHERE 1=1");
            tmpSql.Append(" AND PROCESS='SP'  ");
            tmpSql.AppendFormat(" AND  PARAMETER_NAME IN ({0})", parameter_Name);
            tmpSql.AppendFormat(" AND DATE >= '{0}' AND DATE <= '{1}'", stTime, stTime);
            tmpSql.Append(" GROUP BY WORKSHOP,CATEGORY1,SHIFT ");
            tmpSql.Append(" ) R ");
            tmpSql.Append("  GROUP BY WORKSHOP,SHIFT  ,LINE");
            tmpSql.Append("  )RE  GROUP BY DATA ");
#endif
#if ORACLE
            tmpSql.Append(" SELECT  RE.DATA, SUM(RE.D) AS D, SUM(RE.N) AS N FROM (");
            tmpSql.Append(" SELECT   WORKSHOP||LINE AS DATA, ");
            tmpSql.Append(" CASE SHIFT WHEN 'D'    THEN   SUM(PARAMETER_VALUE)  ELSE 0 END AS D,");
            tmpSql.Append(" CASE SHIFT WHEN 'N'    THEN   SUM(PARAMETER_VALUE)  ELSE 0 END AS N");
            tmpSql.Append(" FROM (");
            tmpSql.Append(" SELECT WORKSHOP,CATEGORY1 AS LINE ,SHIFT,");
            tmpSql.Append(" SUM(CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$')=1 ");
            tmpSql.Append(" THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END) AS PARAMETER_VALUE");
            tmpSql.Append(" FROM TAPEQDATASUMM  WHERE 1=1");
            tmpSql.Append(" AND PROCESS='SP'  ");
            tmpSql.AppendFormat(" AND  PARAMETER_NAME IN ({0})", parameter_Name);
            tmpSql.AppendFormat(" AND \"DATE\" >= '{0}' AND \"DATE\" <= '{1}'", stTime, stTime);
            tmpSql.Append(" GROUP BY WORKSHOP,CATEGORY1,SHIFT ");
            tmpSql.Append(" ) R ");
            tmpSql.Append("  GROUP BY WORKSHOP,SHIFT  ,LINE");
            tmpSql.Append("  )RE  GROUP BY DATA ");
#endif
            DataTable dt = db.Select(tmpSql.ToString()).Tables[0];
            return dt;
        }
        public DataTable GetALM_RETGroupByWAFER_QTY(string parameter_Name)
        {
            StringBuilder tmpSql = new StringBuilder();
#if MSSQL
            tmpSql.Append(" SELECT  RE.DATA, SUM(RE.D) AS D, SUM(RE.N) AS N FROM (");
            tmpSql.Append(" SELECT   WORKSHOP+LINE AS DATA, ");
            tmpSql.Append(" CASE SHIFT WHEN 'D'    THEN   SUM(PARAMETER_VALUE)  ELSE 0 END AS D,");
            tmpSql.Append(" CASE SHIFT WHEN 'N'    THEN   SUM(PARAMETER_VALUE)  ELSE 0 END AS N");
            tmpSql.Append(" FROM (");
            tmpSql.Append(" SELECT WORKSHOP,CATEGORY1 AS LINE ,SHIFT,");
            tmpSql.Append(" MAX(CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE not LIKE '%[^0-9.]%'");
            tmpSql.Append(" THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END) AS PARAMETER_VALUE");
            tmpSql.Append(" FROM TAPEQDATASUMM  WHERE 1=1");
            tmpSql.Append(" AND PROCESS='SP'  ");
            tmpSql.Append(" AND  PARAMETER_NAME IN ('WAFER_QTY')");
            tmpSql.Append(" AND CATEGORY2='P4'");
            tmpSql.AppendFormat(" AND DATE >= '{0}' AND DATE <= '{1}'", stTime, stTime);
            tmpSql.Append(" GROUP BY WORKSHOP,CATEGORY1,SHIFT ");
            tmpSql.Append(" ) R ");
            tmpSql.Append("  GROUP BY WORKSHOP,SHIFT  ,LINE");
            tmpSql.Append("  )RE  GROUP BY DATA ");
#endif
#if ORACLE
tmpSql.Append(" SELECT  RE.DATA, SUM(RE.D) AS D, SUM(RE.N) AS N FROM (");
            tmpSql.Append(" SELECT   WORKSHOP||LINE AS DATA, ");
            tmpSql.Append(" CASE SHIFT WHEN 'D'    THEN   SUM(PARAMETER_VALUE)  ELSE 0 END AS D,");
            tmpSql.Append(" CASE SHIFT WHEN 'N'    THEN   SUM(PARAMETER_VALUE)  ELSE 0 END AS N");
            tmpSql.Append(" FROM (");
            tmpSql.Append(" SELECT WORKSHOP,CATEGORY1 AS LINE ,SHIFT,");
            tmpSql.Append(" SUM(CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$')=1 ");
            tmpSql.Append(" THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END) AS PARAMETER_VALUE");
            tmpSql.Append(" FROM TAPEQDATASUMM  WHERE 1=1");
            tmpSql.Append(" AND PROCESS='SP'  ");
            tmpSql.Append(" AND  PARAMETER_NAME IN ('WAFER_QTY')");
            tmpSql.Append(" AND CATEGORY2='P4'");
            tmpSql.AppendFormat(" AND \"DATE\" >= '{0}' AND \"DATE\" <= '{1}'", stTime, stTime);
            tmpSql.Append(" GROUP BY WORKSHOP,CATEGORY1,SHIFT ");
            tmpSql.Append(" ) R ");
            tmpSql.Append("  GROUP BY WORKSHOP,SHIFT  ,LINE");
            tmpSql.Append("  )RE  GROUP BY DATA ");
#endif
            DataTable dt = db.Select(tmpSql.ToString()).Tables[0];
            return dt;
        }
        public DataTable GetALM_RETGroupByShift()
        {
            StringBuilder tmpSql = new StringBuilder();
#if MSSQL

            tmpSql.Append(" SELECT  DATA, SUM(RE.D) AS D, SUM(RE.N) AS N FROM ( ");
            tmpSql.Append(" SELECT  '产量' as DATA, CASE SHIFT WHEN 'D'   ");
            tmpSql.Append(" THEN   SUM(PARAMETER_VALUE)  ELSE 0 END AS D, CASE SHIFT WHEN 'N'    THEN   SUM(PARAMETER_VALUE) ");
            tmpSql.Append(" ELSE 0 END AS N FROM ( ");
            tmpSql.Append(" SELECT WORKSHOP,CATEGORY1 AS LINE,SHIFT, MAX(CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 ");
            tmpSql.Append(" AND PARAMETER_VALUE not LIKE '%[^0-9.]%' THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6)) ");
            tmpSql.Append(" ELSE 0.0 END) AS PARAMETER_VALUE FROM TAPEQDATASUMM  WHERE 1=1 AND  PARAMETER_NAME IN ('WAFER_QTY')");
            tmpSql.Append(" AND PROCESS='SP'  ");
            tmpSql.Append(" AND CATEGORY2='P4'  ");
            tmpSql.AppendFormat(" AND DATE >= '{0}' AND DATE <= '{1}'", stTime, stTime);
            tmpSql.Append(" GROUP BY WORKSHOP,CATEGORY1,SHIFT  ) R  ");
            tmpSql.Append(" GROUP BY SHIFT  ");
            tmpSql.Append(" )RE  GROUP BY DATA ");
#endif
#if ORACLE
tmpSql.Append(" SELECT  DATA, SUM(RE.D) AS D, SUM(RE.N) AS N FROM ( ");
            tmpSql.Append(" SELECT  '产量' as DATA, CASE SHIFT WHEN 'D'   ");
            tmpSql.Append(" THEN   SUM(PARAMETER_VALUE)  ELSE 0 END AS D, CASE SHIFT WHEN 'N'    THEN   SUM(PARAMETER_VALUE) ");
            tmpSql.Append(" ELSE 0 END AS N FROM ( ");
            tmpSql.Append(" SELECT WORKSHOP,CATEGORY1 AS LINE,SHIFT, MAX(CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 ");
            tmpSql.Append(" THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6)) ");
            tmpSql.Append(" ELSE 0.0 END) AS PARAMETER_VALUE FROM TAPEQDATASUMM  WHERE 1=1 AND  PARAMETER_NAME IN ('WAFER_QTY')");
            tmpSql.Append(" AND PROCESS='SP'  ");
            tmpSql.Append(" AND CATEGORY2='P4'  ");
            tmpSql.AppendFormat(" AND \"DATE\" >= '{0}' AND \"DATE\" <= '{1}'", stTime, stTime);
            tmpSql.Append(" GROUP BY WORKSHOP,CATEGORY1,SHIFT  ) R  ");
            tmpSql.Append(" GROUP BY SHIFT  ");
            tmpSql.Append(" )RE  GROUP BY DATA ");
#endif
            DataTable dt = db.Select(tmpSql.ToString()).Tables[0];
            return dt;
        }
        public DataTable GetALM_RETGroupByProcessTime()
        {
            StringBuilder tmpSql = new StringBuilder();
#if MSSQL

            tmpSql.Append(" SELECT WORKSHOP,LINE,DATE,SHIFT ,PARAMETER_NAME,");
            tmpSql.Append(" SUM(DOWN_TIME+RUN_TIME+UP_TIME+PROCESS_TIME) AS VALUE");
            tmpSql.Append(" FROM (");
            tmpSql.Append(" SELECT WORKSHOP, CATEGORY1 AS LINE,DATE,SHIFT ,PARAMETER_NAME,");
            tmpSql.Append("  MAX(CASE PARAMETER_NAME WHEN 'DOWN_TIME'    THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS DOWN_TIME,");
            tmpSql.Append("  MIN(CASE PARAMETER_NAME WHEN 'RUN_TIME'     THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS RUN_TIME,");
            tmpSql.Append("  MIN(CASE PARAMETER_NAME WHEN 'UP_TIME'      THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS UP_TIME,");
            tmpSql.Append("  MAX(CASE PARAMETER_NAME WHEN 'PROCESS_TIME' THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS PROCESS_TIME");
            tmpSql.Append(" FROM TAPEQDATASUMM ");
            tmpSql.Append(" WHERE 1=1");
            tmpSql.Append(" AND PROCESS='SP'  ");
            tmpSql.Append("  AND  PARAMETER_NAME IN ('DOWN_TIME', 'UP_TIME','RUN_TIME', 'PROCESS_TIME')");
            tmpSql.AppendFormat(" AND DATE >= '{0}' AND DATE <= '{1}'", stTime, stTime);
            tmpSql.Append("  GROUP BY  WORKSHOP,CATEGORY1,DATE,SHIFT ,PARAMETER_NAME");
            tmpSql.Append("  ) A GROUP BY WORKSHOP,LINE,DATE,SHIFT ,PARAMETER_NAME");
            tmpSql.Append("  UNION ALL");
            tmpSql.Append("   SELECT ");
            tmpSql.Append("  WORKSHOP,CATEGORY1 AS LINE,DATE,SHIFT,PARAMETER_NAME,");
            tmpSql.Append(" MAX(CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END)");
            tmpSql.Append("  AS VALUE");
            tmpSql.Append("  FROM TAPEQDATASUMM");
            tmpSql.Append("  WHERE 1=1");
            tmpSql.Append(" AND PROCESS='SP'  ");
            tmpSql.Append("   AND  PARAMETER_NAME IN ( 'IDLE_TIME')");
            tmpSql.Append("  AND  CATEGORY2 ='P1'");
            tmpSql.AppendFormat(" AND DATE >= '{0}' AND DATE <= '{1}'", stTime, stTime);
            tmpSql.Append("    GROUP BY WORKSHOP,CATEGORY1,DATE,SHIFT,PARAMETER_NAME");
#endif
#if ORACLE
            tmpSql.Append(" SELECT WORKSHOP,LINE,\"DATE\",SHIFT ,PARAMETER_NAME,");
            tmpSql.Append(" SUM(DOWN_TIME+RUN_TIME+UP_TIME+PROCESS_TIME) AS VALUE");
            tmpSql.Append(" FROM (");
            tmpSql.Append(" SELECT WORKSHOP, CATEGORY1 AS LINE,\"DATE\",SHIFT ,PARAMETER_NAME,");
            tmpSql.Append("  MAX(CASE PARAMETER_NAME WHEN 'DOWN_TIME'    THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS DOWN_TIME,");
            tmpSql.Append("  MIN(CASE PARAMETER_NAME WHEN 'RUN_TIME'     THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS RUN_TIME,");
            tmpSql.Append("  MIN(CASE PARAMETER_NAME WHEN 'UP_TIME'      THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS UP_TIME,");
            tmpSql.Append("  MAX(CASE PARAMETER_NAME WHEN 'PROCESS_TIME' THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS PROCESS_TIME");
            tmpSql.Append(" FROM TAPEQDATASUMM ");
            tmpSql.Append(" WHERE 1=1");
            tmpSql.Append(" AND PROCESS='SP'  ");
            tmpSql.Append("  AND  PARAMETER_NAME IN ('DOWN_TIME', 'UP_TIME','RUN_TIME', 'PROCESS_TIME')");
            tmpSql.AppendFormat(" AND \"DATE\" >= '{0}' AND \"DATE\" <= '{1}'", stTime, stTime);
            tmpSql.Append("  GROUP BY  WORKSHOP,CATEGORY1,\"DATE\",SHIFT ,PARAMETER_NAME");
            tmpSql.Append("  ) A GROUP BY WORKSHOP,LINE,\"DATE\",SHIFT ,PARAMETER_NAME");
            tmpSql.Append("  UNION ALL");
            tmpSql.Append("   SELECT ");
            tmpSql.Append("  WORKSHOP,CATEGORY1 AS LINE,\"DATE\",SHIFT,PARAMETER_NAME,");
            tmpSql.Append(" MAX(CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END)");
            tmpSql.Append("  AS VALUE");
            tmpSql.Append("  FROM TAPEQDATASUMM");
            tmpSql.Append("  WHERE 1=1");
            tmpSql.Append(" AND PROCESS='SP'  ");
            tmpSql.Append("   AND  PARAMETER_NAME IN ( 'IDLE_TIME')");
            tmpSql.Append("  AND  CATEGORY2 ='P1'");
            tmpSql.AppendFormat(" AND \"DATE\" >= '{0}' AND \"DATE\" <= '{1}'", stTime, stTime);
            tmpSql.Append("    GROUP BY WORKSHOP,CATEGORY1,\"DATE\",SHIFT,PARAMETER_NAME");
#endif
            DataTable dt = db.Select(tmpSql.ToString()).Tables[0];
            return dt;
        }

        public DataTable GetSpAlmData(string workshop, string line)
        {
            StringBuilder tmpsql = new StringBuilder();
#if MSSQL
            tmpsql.Append(" SELECT ");
            tmpsql.Append(" SUBSTRING(RE.DATE,5,2)+'.'+SUBSTRING(RE.DATE,7,2)+RE.SHIFT AS DATA,");
            tmpsql.Append(" SUM(RE.[TABLE-L1-P1])AS [台面-L1-P1],SUM(RE.[TABLE-L1-P2])AS [台面-L1-P2],SUM(RE.[TABLE-L1-P3])AS [台面-L1-P3],SUM(RE.[TABLE-L1-P4])AS [台面-L1-P4],");
            tmpsql.Append(" SUM(RE.[CV-L1-P1])AS [进出片-L1-P1],SUM(RE.[CV-L1-P2])AS [进出片-L1-P2],SUM(RE.[CV-L1-P3])AS [进出片-L1-P3],SUM(RE.[CV-L1-P4])AS [进出片-L1-P4],");
            tmpsql.Append(" SUM(RE.[AOI-L1-P1])AS [AOI-L1-P1],SUM(RE.[AOI-L1-P2])AS [AOI-L1-P2],SUM(RE.[AOI-L1-P3])AS [AOI-L1-P3],SUM(RE.[AOI-L1-P4])AS [AOI-L1-P4],");
            tmpsql.Append(" SUM(RE.[DOOR-L1-P1])AS [门检-L1-P1],SUM(RE.[DOOR-L1-P2])AS [门检-L1-P2],SUM(RE.[DOOR-L1-P3])AS [门检-L1-P3],SUM(RE.[DOOR-L1-P4])AS [门检-L1-P4]");
            tmpsql.Append(" FROM ");
            tmpsql.Append(" (");
            tmpsql.Append(" SELECT ");
            tmpsql.Append(" WORKSHOP,CATEGORY1 AS LINE,");
            tmpsql.Append(" DATE,SHIFT,");
            tmpsql.Append("CATEGORY2 AS  PROCESS_TYPE,");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME LIKE 'TABLE%'AND    CATEGORY2='P1'     THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS [TABLE-L1-P1],");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME LIKE 'TABLE%'AND    CATEGORY2='P2'     THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS [TABLE-L1-P2],");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME LIKE 'TABLE%'AND    CATEGORY2='P3'     THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS [TABLE-L1-P3],");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME LIKE 'TABLE%'AND    CATEGORY2='P4'     THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS [TABLE-L1-P4],");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='CV'  AND    CATEGORY2='P1'    THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS [CV-L1-P1],");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='CV'  AND    CATEGORY2='P2'    THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS [CV-L1-P2],");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='CV'  AND    CATEGORY2='P3'    THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS [CV-L1-P3],");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='CV'  AND    CATEGORY2='P4'    THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS [CV-L1-P4],");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='AOI'  AND  CATEGORY2='P1'   THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS    [AOI-L1-P1],");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='AOI'  AND  CATEGORY2='P2'   THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS    [AOI-L1-P2],");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='AOI'  AND  CATEGORY2='P3'   THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS    [AOI-L1-P3],");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='AOI'  AND  CATEGORY2='P4'   THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS    [AOI-L1-P4],");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='DOOR'AND CATEGORY2='P1' THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS       [DOOR-L1-P1],");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='DOOR'AND CATEGORY2='P2' THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS       [DOOR-L1-P2],");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='DOOR'AND CATEGORY2='P3' THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS       [DOOR-L1-P3],");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='DOOR'AND CATEGORY2='P4' THEN  CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS       [DOOR-L1-P4]");
            tmpsql.Append(" FROM TAPEQDATASUMM  WHERE 1=1  ");
            tmpsql.Append(" AND PARAMETER_NAME IN('TABLE_A','TABLE_B','TABLE_C','TABLE_D','CV','AOI','DOOR')");
            tmpsql.AppendFormat(" AND DATE >= {0} AND DATE <= {1}", stTime, stTime);
            // tmpsql.Append("  AND DATE >= 20211115 AND DATE <= 20211115");
            tmpsql.AppendFormat(" AND WORKSHOP='{0}'", workshop);
            tmpsql.AppendFormat(" AND CATEGORY1='{0}'", line);
            tmpsql.Append(" GROUP BY WORKSHOP,CATEGORY1,DATE,SHIFT, ");
            tmpsql.Append(" CATEGORY2,PARAMETER_NAME  ");
            tmpsql.Append(") RE");
            tmpsql.Append(" GROUP BY ");
            tmpsql.Append(" RE.WORKSHOP,RE.DATE,RE.SHIFT");
#endif
#if ORACLE
            tmpsql.Append(" SELECT ");
            tmpsql.Append(" SUBSTR(RE.\"DATE\",5,2)||'.'||SUBSTR(RE.\"DATE\",7,2)||RE.SHIFT AS DATA,");
            tmpsql.Append(" SUM(RE.\"TABLE-L1-P1\")AS \"台面-L1-P1\",SUM(RE.\"TABLE-L1-P2\")AS \"台面-L1-P2\",SUM(RE.\"TABLE-L1-P3\")AS \"台面-L1-P3\",SUM(RE.\"TABLE-L1-P4\")AS \"台面-L1-P4\",");
            tmpsql.Append(" SUM(RE.\"CV-L1-P1\")AS \"进出片-L1-P1\",SUM(RE.\"CV-L1-P2\")AS \"进出片-L1-P2\",SUM(RE.\"CV-L1-P3\")AS \"进出片-L1-P3\",SUM(RE.\"CV-L1-P4\")AS \"进出片-L1-P4\",");
            tmpsql.Append(" SUM(RE.\"AOI-L1-P1\")AS \"AOI-L1-P1\",SUM(RE.\"AOI-L1-P2\")AS \"AOI-L1-P2\",SUM(RE.\"AOI-L1-P3\")AS \"AOI-L1-P3\",SUM(RE.\"AOI-L1-P4\")AS \"AOI-L1-P4\",");
            tmpsql.Append(" SUM(RE.\"DOOR-L1-P1\")AS \"门检-L1-P1\",SUM(RE.\"DOOR-L1-P2\")AS \"门检-L1-P2\",SUM(RE.\"DOOR-L1-P3\")AS \"门检-L1-P3\",SUM(RE.\"DOOR-L1-P4\")AS \"门检-L1-P4\"");
            tmpsql.Append(" FROM ");
            tmpsql.Append(" (");
            tmpsql.Append(" SELECT ");
            tmpsql.Append(" WORKSHOP,CATEGORY1 AS LINE,");
            tmpsql.Append(" \"DATE\",SHIFT,");
            tmpsql.Append("CATEGORY2 AS  PROCESS_TYPE,");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME LIKE 'TABLE%'AND    CATEGORY2='P1'     THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS \"TABLE-L1-P1\",");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME LIKE 'TABLE%'AND    CATEGORY2='P2'     THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS \"TABLE-L1-P2\",");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME LIKE 'TABLE%'AND    CATEGORY2='P3'     THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS \"TABLE-L1-P3\",");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME LIKE 'TABLE%'AND    CATEGORY2='P4'     THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS \"TABLE-L1-P4\",");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='CV'  AND    CATEGORY2='P1'    THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS \"CV-L1-P1\",");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='CV'  AND    CATEGORY2='P2'    THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS \"CV-L1-P2\",");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='CV'  AND    CATEGORY2='P3'    THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS \"CV-L1-P3\",");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='CV'  AND    CATEGORY2='P4'    THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS \"CV-L1-P4\",");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='AOI'  AND  CATEGORY2='P1'   THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS    \"AOI-L1-P1\",");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='AOI'  AND  CATEGORY2='P2'   THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS    \"AOI-L1-P2\",");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='AOI'  AND  CATEGORY2='P3'   THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS    \"AOI-L1-P3\",");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='AOI'  AND  CATEGORY2='P4'   THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS    \"AOI-L1-P4\",");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='DOOR'AND CATEGORY2='P1' THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS       \"DOOR-L1-P1\",");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='DOOR'AND CATEGORY2='P2' THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS       \"DOOR-L1-P2\",");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='DOOR'AND CATEGORY2='P3' THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS       \"DOOR-L1-P3\",");
            tmpsql.Append(" MAX(CASE  WHEN PARAMETER_NAME='DOOR'AND CATEGORY2='P4' THEN  CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,6))    ELSE 0.0 END ELSE 0 END) AS       \"DOOR-L1-P4\"");
            tmpsql.Append(" FROM TAPEQDATASUMM  WHERE 1=1  ");
            tmpsql.Append(" AND PARAMETER_NAME IN('TABLE_A','TABLE_B','TABLE_C','TABLE_D','CV','AOI','DOOR')");
            tmpsql.AppendFormat(" AND \"DATE\" >= {0} AND \"DATE\" <= {1}", stTime, stTime);
            // tmpsql.Append("  AND \"DATE\" >= 20211115 AND \"DATE\" <= 20211115");
            tmpsql.AppendFormat(" AND WORKSHOP='{0}'", workshop);
            tmpsql.AppendFormat(" AND CATEGORY1='{0}'", line);
            tmpsql.Append(" GROUP BY WORKSHOP,CATEGORY1,\"DATE\",SHIFT, ");
            tmpsql.Append(" CATEGORY2,PARAMETER_NAME  ");
            tmpsql.Append(") RE");
            tmpsql.Append(" GROUP BY ");
            tmpsql.Append(" RE.WORKSHOP,RE.\"DATE\",RE.SHIFT");
#endif
            DataTable ds = db.Select(tmpsql.ToString()).Tables[0];
            return ds;
        }
        private void CalcgBmDaily(int TitleIndex, int StartRowIndex)
        {
            DateTime waitTime = DateTime.MinValue;
            DateTime startTime = DateTime.MinValue;
            DateTime endTime = DateTime.MinValue;
            DataTable WriteExcelDt = new DataTable();
            //System.Int32
            WriteExcelDt.Columns.Add("TOP NO", Type.GetType("System.Int32"));
            WriteExcelDt.Columns.Add("PROCESS", Type.GetType("System.String"));
            WriteExcelDt.Columns.Add("WORK SHOP", Type.GetType("System.String"));
            WriteExcelDt.Columns.Add("EQUIPMENT NO", Type.GetType("System.String"));
            WriteExcelDt.Columns.Add("EQUIPMENT TYPE", Type.GetType("System.String"));
            WriteExcelDt.Columns.Add("WAIT", Type.GetType("System.String"));
            WriteExcelDt.Columns.Add("START", Type.GetType("System.String"));
            WriteExcelDt.Columns.Add("END", Type.GetType("System.String"));
            WriteExcelDt.Columns.Add("BM TIME(H)", Type.GetType("System.Double"));
            WriteExcelDt.Columns.Add("현상(现象)", Type.GetType("System.String"));
            WriteExcelDt.Columns.Add("원인(原因)", Type.GetType("System.String"));
            WriteExcelDt.Columns.Add("조치(措施)", Type.GetType("System.String"));
            for (int i = 0; i < BmInfo.Rows.Count; i++)
            {
                string process = BmInfo.Rows[i]["AREA"].ToString();
                string workshop = BmInfo.Rows[i]["LINE"].ToString();
                string equipment = BmInfo.Rows[i]["EQUIPMENT"].ToString();
                string equipmenttype = equipment.Substring(7, 1);//BmInfo.Rows[i]["EQUIPMENTTYPE"].ToString()
                string wait = OutTime(BmInfo.Rows[i]["INSERTTIME"].ToString(), waitTime).ToString("yyyy-MM-dd HH:mm:ss");
                string start = OutTime(BmInfo.Rows[i]["BMSTARTTIME"].ToString(), startTime).ToString("yyyy-MM-dd HH:mm:ss");
                string end = OutTime(BmInfo.Rows[i]["UPDATETIME"].ToString(), endTime).ToString("yyyy-MM-dd HH:mm:ss");
                string bmtime = GetBmTime(BmInfo.Rows[i]["INSERTTIME"].ToString(), BmInfo.Rows[i]["BMSTARTTIME"].ToString(), BmInfo.Rows[i]["UPDATETIME"].ToString());
                string phenomenon = BmInfo.Rows[i]["phenomenon"].ToString();
                string reason = BmInfo.Rows[i]["reason"].ToString();
                string measures = BmInfo.Rows[i]["measures"].ToString();
                WriteExcelDt.Rows.Add(new object[] { null, process, workshop, equipment, equipmenttype, wait, start, end, bmtime, phenomenon, reason, measures });
            }
            WriteExcelDt.DefaultView.Sort = "BM TIME(H) DESC";
            WriteExcelDt = WriteExcelDt.DefaultView.ToTable();
            int count = 10;
            if (WriteExcelDt.Rows.Count < 10) count = WriteExcelDt.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                WriteExcelDt.Rows[i]["TOP NO"] = i + 1;
            }
            WriteExcelDt.DefaultView.RowFilter = "[TOP NO] IS NOT NULL";
            WriteExcelDt = WriteExcelDt.DefaultView.ToTable();
            //key:WriteExcelDt column inde
            //value:excel column inde
            Dictionary<int, int> columnindex = new Dictionary<int, int>();
            columnindex.Add(0, 1);
            columnindex.Add(1, 2);
            columnindex.Add(2, 3);
            columnindex.Add(3, 5);
            columnindex.Add(4, 7);
            columnindex.Add(5, 9);
            columnindex.Add(6, 11);
            columnindex.Add(7, 13);
            columnindex.Add(8, 15);
            columnindex.Add(9, 16);
            columnindex.Add(10, 20);
            columnindex.Add(11, 24);
            for (int i = 0; i < WriteExcelDt.Rows.Count; i++)
            {

                for (int j = 0; j < WriteExcelDt.Columns.Count; j++)
                {
                    int newcolumnindex = columnindex[j];
                    CellRange cell = xSheet.Range[(i + StartRowIndex), newcolumnindex];
                    if (Regex.IsMatch(WriteExcelDt.Rows[i][j].ToString(), @"^[+-]?\d*[.]?\d*$") && !string.IsNullOrEmpty(WriteExcelDt.Rows[i][j].ToString()))
                    {
                        cell.NumberValue = Convert.ToDouble(WriteExcelDt.Rows[i][j].ToString());
                        if (j == 8)
                        {
                            cell.NumberFormat = "0.00";
                        }
                        else if (j == 5 || j == 6 || j == 7)
                        {
                            //cell.NumberFormat = "yyyy-m-d h:mm:ss";
                        }
                    }
                    else
                    {
                        cell.Value = WriteExcelDt.Rows[i][j].ToString();
                    }
                }
            }
        }
        private DataRow MaximumIntervalData(DataTable bmInfoDt)
        {
            List<double> list = new List<double>();
            for (int i = 0; i < bmInfoDt.Rows.Count; i++)
            {
                DateTime startTime = DateTime.MinValue;
                DateTime endTime = DateTime.MinValue;
                TimeSpan span;
                double cntsed = 0;
                startTime = OutTime(bmInfoDt.Rows[i]["INSERTTIME"].ToString(), startTime);
                endTime = OutTime(bmInfoDt.Rows[i]["UPDATETIME"].ToString(), endTime);
                span = endTime - startTime;
                cntsed += (double)Math.Round(span.TotalHours, 2);
                list.Add(cntsed);
            }
            if (list.Count == 0)
            {
                return null;
            }
            var v = list.Select((m, index) => new { index, m }).OrderByDescending(n => n.m).First().index;
            return bmInfoDt.Rows[v];
        }
        private void EqOperatingRatio()
        {
            EqOperatingRatioDt = new DataTable();
            EqOperatingRatioDt.Columns.Add("NAME", Type.GetType("System.String"));
            EqOperatingRatioDt.Columns.Add("AREA", Type.GetType("System.String"));
            EqOperatingRatioDt.Columns.Add("EQUIPMENTTYPE", Type.GetType("System.String"));
            EqOperatingRatioDt.Columns.Add("TIME", Type.GetType("System.Double"));//H
            EqOperatingRatioDt.Columns.Add("LINE", Type.GetType("System.String"));
            EqOperatingRatioDt.Columns.Add("EQSTATUS", Type.GetType("System.String"));
            EqOperatingRatioDt.Columns.Add("ENDTIME", Type.GetType("System.String"));
            //equipment与equipmenthistory取差集
            //24小时状态未改变的数据
            DataTable EqDtFilter = noChangeEqDt.DefaultView.ToTable();
            DataTable EqDtHistoryFilter = EqDtHistory.DefaultView.ToTable();
            var noChangeDtRow = from r in EqDtFilter.AsEnumerable()
                                where
                                    !(from rr in EqDtHistoryFilter.AsEnumerable() select rr.Field<string>("NAME")).Contains(
                                    r.Field<string>("NAME"))
                                select r;
            DataTable noChangeDt = null;

            if (noChangeDtRow.Any())
            {

                noChangeDt = noChangeDtRow.CopyToDataTable();
            }
            else
            {
                noChangeDt = EqDtFilter.Clone();
            }
            for (int i = 0; i < noChangeDt.Rows.Count; i++)
            {
                DataTable table = GetEQHistory(noChangeDt.Rows[i]["NAME"].ToString());
                string status = "";
                if (table.Rows.Count > 0 && string.IsNullOrEmpty(table.Rows[0]["EQUIPMENTSTATUS"].ToString()))
                {
                    status = table.Rows[0]["EQUIPMENTSTATUS"].ToString();
                }
                else
                {

                    status = noChangeDt.Rows[i]["EQUIPMENTSTATUS"].ToString();
                }
                if (status == CHECKDOWN)
                {
                    string eqstatus = GetEqCheckDownLastStatus(noChangeDt.Rows[i]["NAME"].ToString(), endTime).ToUpper();
                    if (!string.IsNullOrEmpty(eqstatus))
                    {
                        if (eqstatus.ToUpper() == BMSTART || eqstatus.ToUpper() == BMWAIT)
                        {
                            status = "BMCHECK";
                        }
                        else if (eqstatus.ToUpper() == PMSTART)
                        {
                            status = "PMCHECK";
                        }
                    }
                }
                EqOperatingRatioDt.Rows.Add(new object[] { noChangeDt.Rows[i]["NAME"].ToString(), noChangeDt.Rows[i]["AREA"].ToString(),
                    noChangeDt.Rows[i]["EQUIPMENTTYPE"].ToString(), timeSpan.TotalHours, noChangeDt.Rows[i]["LINE"].ToString(),
                  status  ,null });
            }
            TestOperatingRatio(EqOperatingRatioDt);
            for (int i = 0; i < EqOperatingRatioDt.Rows.Count; i++)
            {
                string CurrentStatus = EqOperatingRatioDt.Rows[i]["EQSTATUS"].ToString();
                //只算bm start- check down和之前的
                if (CurrentStatus == "CBM3" && i + 1 < EqOperatingRatioDt.Rows.Count && EqOperatingRatioDt.Rows[i + 1]["EQSTATUS"].ToString() == "CBM2" && EqOperatingRatioDt.Rows[i]["NAME"].ToString() == EqOperatingRatioDt.Rows[i + 1]["NAME"].ToString())
                {
                    if (!string.IsNullOrEmpty(EqOperatingRatioDt.Rows[i]["ENDTIME"].ToString()))
                    {
                        //tubecount
                        DataRow[] drs = EqDt.Select(string.Format("NAME='{0}'", EqOperatingRatioDt.Rows[i]["NAME"]));
                        if (drs.Length == 0)
                            continue;
                        string tubecount = drs[0]["TUBECOUNT"].ToString();
                        string tube = GetBMInfoTube(EqOperatingRatioDt.Rows[i + 1]["ENDTIME"].ToString(), EqOperatingRatioDt.Rows[i + 1]["NAME"].ToString());
                        tubecount = string.IsNullOrEmpty(tubecount) ? "1" : tubecount;
                        if (string.IsNullOrEmpty(tube) || tube.Contains("ALL"))
                        {
                            tube = tubecount;
                        }
                        else
                        {
                            if (tube.Contains("EQ1") || tube.Contains("EQ2") || tube.Contains("A") || tube.Contains("B"))
                            {
                                if (tube.Contains("EQ1") && tube.Contains("EQ2"))
                                {
                                    tube = tubecount;
                                }
                                else if (tube.Contains("A") && tube.Contains("B"))
                                {
                                    tube = tubecount;
                                }
                                else
                                {
                                    tube = ((Convert.ToInt32(tubecount) / 2) + (Convert.ToInt32(tube.Split(',').Length.ToString()) - 1)).ToString();
                                }
                            }
                            else
                            {
                                tube = tube.Split(',').Length.ToString();
                            }
                        }
                        double occupancyRate = Math.Round(Convert.ToInt32(tube) / Convert.ToDouble(tubecount), 2);
                        if (tube != tubecount)
                        {
                            EqOperatingRatioDt.Rows[i]["TIME"] = Math.Round((Convert.ToDouble(EqOperatingRatioDt.Rows[i]["TIME"]) * occupancyRate), 2).ToString();
                            EqOperatingRatioDt.Rows[i + 1]["TIME"] = Math.Round((Convert.ToDouble(EqOperatingRatioDt.Rows[i + 1]["TIME"]) * occupancyRate), 2).ToString();

                        }
                        i = i + 1;
                    }
                }
                else if (CurrentStatus == "CBUTILITY" || CurrentStatus == "CBM1" || CurrentStatus == "CBM3" || CurrentStatus == "CBPROCESS" || CurrentStatus == "BM WAIT" || CurrentStatus == "CBM2")
                {
                    if (!string.IsNullOrEmpty(EqOperatingRatioDt.Rows[i]["ENDTIME"].ToString()))
                    {
                        DataRow[] drs = EqDt.Select(string.Format("NAME='{0}'", EqOperatingRatioDt.Rows[i]["NAME"]));
                        if (drs.Length == 0)
                            continue;
                        string tubecount = drs[0]["TUBECOUNT"].ToString();
                        string tube = GetBMInfoTube(EqOperatingRatioDt.Rows[i]["ENDTIME"].ToString(), EqOperatingRatioDt.Rows[i]["NAME"].ToString());
                        tubecount = string.IsNullOrEmpty(tubecount) ? "1" : tubecount;
                        if (string.IsNullOrEmpty(tube) || tube.Contains("ALL"))
                        {
                            tube = tubecount;
                        }
                        else
                        {
                            if (tube.Contains("EQ1") || tube.Contains("EQ2") || tube.Contains("A") || tube.Contains("B"))
                            {
                                if (tube.Contains("EQ1") && tube.Contains("EQ2"))
                                {
                                    tube = tubecount;
                                }
                                else if (tube.Contains("A") && tube.Contains("B"))
                                {
                                    tube = tubecount;
                                }
                                else
                                {
                                    tube = ((Convert.ToInt32(tubecount) / 2) + (Convert.ToInt32(tube.Split(',').Length.ToString()) - 1)).ToString();
                                }
                            }
                            else
                            {
                                tube = tube.Split(',').Length.ToString();
                            }
                        }
                        if (tube != tubecount)
                        {
                            double occupancyRate = Math.Round(Convert.ToInt32(tube) / Convert.ToDouble(tubecount), 2);
                            EqOperatingRatioDt.Rows[i]["TIME"] = Math.Round((Convert.ToDouble(EqOperatingRatioDt.Rows[i]["TIME"]) * occupancyRate), 2).ToString();
                        }
                    }
                }

            }
        }
        private void GetEQ()
        {
            StringBuilder tmpsb = new StringBuilder();
            tmpsb.Append("SELECT  AREA,LINE,MAINEQUIPMENT,NAME,REGION,FACILITY,EQUIPMENTTYPE ,LASTEVENTTIME,EQUIPMENTSTATUS,CASE WHEN(TUBECOUNT=0 OR TUBECOUNT IS NULL) THEN 1 ELSE TUBECOUNT END AS TUBECOUNT FROM TAPFTEQUIPMENT ");
            tmpsb.Append("  WHERE ISALIVE='YES' AND EQUIPMENTTYPE NOT IN ('G','F','T')");
            tmpsb.Append(" order by  NAME,LASTEVENTTIME DESC");
            EqDt = db.Select(tmpsb.ToString()).Tables[0];
        }
        private void GetEQnoChange()
        {
            StringBuilder tmpsb = new StringBuilder();
            tmpsb.Append(" SELECT EQ.AREA,EQ.LINE,EQ.MAINEQUIPMENT,EQ.NAME,EQ.REGION,EQ.FACILITY,EQ.EQUIPMENTTYPE");
            tmpsb.Append(" ,HIS.LASTEVENTTIME,HIS.EQUIPMENTSTATUS,CASE WHEN(TUBECOUNT=0 OR TUBECOUNT IS NULL) ");
            tmpsb.Append(" THEN 1 ELSE TUBECOUNT END AS TUBECOUNT FROM (");
            tmpsb.Append(" SELECT HIIII.NAME,EQUIPMENTSTATUS,LASTEVENTTIME FROM TAPFTEQUIPMENTHISTORY HIIII,(");
            tmpsb.Append(" SELECT NAME,MAX(LASTEVENTTIME) MAXTIME FROM TAPFTEQUIPMENTHISTORY HI ,");
            tmpsb.Append(" (");
            tmpsb.Append(" SELECT NAME NAMES FROM TAPFTEQUIPMENT WHERE ISALIVE='YES' AND EQUIPMENTTYPE NOT IN ('G','F','T')");
            tmpsb.Append(" AND NAME NOT IN(");
            tmpsb.Append(" SELECT DISTINCT(NAME) FROM TAPFTEQUIPMENTHISTORY ");
            tmpsb.Append(" WHERE");
            tmpsb.AppendFormat(" LASTEVENTTIME  BETWEEN '{0}'AND '{1}') ", startTime, endTime);
            tmpsb.Append(" ) HII");
            tmpsb.Append(" WHERE HI.NAME = HII.NAMES");
            tmpsb.AppendFormat(" AND LASTEVENTTIME <'{0}'  ", startTime);
            tmpsb.Append(" GROUP BY NAME");
            tmpsb.Append(" )HIII");
            tmpsb.Append(" WHERE HIIII.NAME = HIII.NAME");
            tmpsb.Append(" AND HIIII.LASTEVENTTIME = HIII.MAXTIME");
            tmpsb.Append("  ) HIS ,TAPFTEQUIPMENT EQ WHERE HIS.NAME=EQ.NAME AND EQ.ISALIVE='YES'");
            noChangeEqDt = db.Select(tmpsb.ToString()).Tables[0];
        }
        private void GetEQHistory()
        {
            StringBuilder tmpsb = new StringBuilder();
            tmpsb.Append("SELECT AREA,LINE,MAINEQUIPMENT,NAME,REGION,FACILITY,EQUIPMENTTYPE ,LASTEVENTTIME,EQUIPMENTSTATUS FROM TAPFTEQUIPMENTHISTORY ");
            tmpsb.Append("  WHERE ISALIVE='YES' ");
            tmpsb.Append(" AND LASTEVENTTIME BETWEEN ");
            tmpsb.AppendFormat("'{0}'", startTime);
            tmpsb.Append(" AND ");
            tmpsb.AppendFormat(" '{0}'", endTime);
            tmpsb.Append(" AND  EQUIPMENTTYPE NOT IN ('G','F','T')");
            tmpsb.Append(" order by LASTEVENTTIME DESC");
            EqDtHistory = db.Select(tmpsb.ToString()).Tables[0];
        }
        private void GetBMInfo()
        {
            StringBuilder tmpsb = new StringBuilder();
            tmpsb.Append("SELECT  TIMEKEY,REGION,FACILITY,LINE,AREA,BAY,MAINEQUIPMENT,EQUIPMENT,PHENOMENON,REASON,MEASURES,BMSTARTTIME,INSERTTIME,UPDATETIME FROM TAPFTBMINFO ");
            tmpsb.Append(" WHERE UPDATETIME BETWEEN ");
            tmpsb.AppendFormat(" '{0}'", startTime);
            tmpsb.Append(" AND ");
            tmpsb.AppendFormat(" '{0}'", endTime);
            tmpsb.Append(" AND REASON IS NOT NULL ");
            tmpsb.Append(" AND REASON !='Other EQ Problem Cause BM'  ");
            tmpsb.Append(" AND REASON !='Process have Some Problem' ");
            tmpsb.Append(" AND (PHENOMENON is not null OR  MEASURES is not null) ");
            BmInfo = db.Select(tmpsb.ToString()).Tables[0];
        }
        private void GetBMCompleteList()
        {
            StringBuilder tmpsb = new StringBuilder();
#if MSSQL
            tmpsb.Append(" SELECT INFO.*,US.USERNAME AS[WORKERNAME] FROM( ");
            tmpsb.Append(" SELECT  B.LINE AS[WORK SHOP], B.AREA AS PROCESS, B.MAINEQUIPMENT, B.EQUIPMENT, B.PHENOMENON, ");
            tmpsb.Append(" B.REASON, B.MEASURES, B.TIMEKEY AS[BMWAIT TIME], B.BMSTARTTIME AS[BMSTART TIME], B.UPDATETIME AS[BMEND TIME], ");
            tmpsb.Append(" B.INSERTUSER AS[REQUEST USERID], U.USERNAME AS[REQUEST USERNAME], B.UPDATEUSER AS[WORKERID] FROM TAPFTBMINFO B, TAPUTUSERS U ");
            tmpsb.Append("WHERE U.NAME = B.INSERTUSER ");
            tmpsb.Append("AND  U.ISALIVE = 'YES' ");
            tmpsb.Append(") INFO  ,TAPUTUSERS US ");
            tmpsb.Append("WHERE INFO.[WORKERID] = US.NAME ");
            tmpsb.Append("AND  US.ISALIVE='YES'  ");
            tmpsb.Append(" AND INFO.[BMEND TIME] BETWEEN ");
            tmpsb.AppendFormat(" '{0}'", startTime);
            tmpsb.Append(" AND ");
            tmpsb.AppendFormat(" '{0}'", endTime);
            tmpsb.Append(" AND INFO.REASON IS NOT NULL ");
            tmpsb.Append(" AND INFO.REASON !='Other EQ Problem Cause BM'  ");
            tmpsb.Append(" AND INFO.REASON !='Process have Some Problem' ");
            tmpsb.Append(" AND (INFO.PHENOMENON !='' OR  INFO.MEASURES!='') ");
#endif
#if ORACLE
            tmpsb.Append(" SELECT INFO.*,US.USERNAME AS WORKERNAME FROM( ");
            tmpsb.Append(" SELECT  B.LINE AS \"WORK SHOP\", B.AREA AS PROCESS, B.MAINEQUIPMENT, B.EQUIPMENT, B.PHENOMENON, ");
            tmpsb.Append(" B.REASON, B.MEASURES, B.TIMEKEY AS \"BMWAIT TIME\", B.BMSTARTTIME AS \"BMSTART TIME\", B.UPDATETIME AS \"BMEND TIME\", ");
            tmpsb.Append(" B.INSERTUSER AS \"REQUEST USERID\", U.USERNAME AS \"REQUEST USERNAME\", B.UPDATEUSER AS WORKERID FROM TAPFTBMINFO B, TAPUTUSERS U ");
            tmpsb.Append("WHERE U.NAME = B.INSERTUSER ");
            tmpsb.Append("AND  U.ISALIVE = 'YES' ");
            tmpsb.Append(") INFO  ,TAPUTUSERS US ");
            tmpsb.Append("WHERE INFO.WORKERID = US.NAME ");
            tmpsb.Append("AND  US.ISALIVE='YES'  ");
            tmpsb.Append(" AND INFO.\"BMEND TIME\" BETWEEN ");
            tmpsb.AppendFormat(" '{0}'", startTime);
            tmpsb.Append(" AND ");
            tmpsb.AppendFormat(" '{0}'", endTime);
            tmpsb.Append(" AND INFO.REASON IS NOT NULL ");
            tmpsb.Append(" AND INFO.REASON !='Other EQ Problem Cause BM'  ");
            tmpsb.Append(" AND INFO.REASON !='Process have Some Problem' ");
            tmpsb.Append(" AND (INFO.PHENOMENON is not null OR  INFO.MEASURES is not null) ");
#endif
            BmComList = db.Select(tmpsb.ToString()).Tables[0];
        }
        private string GetBMInfoTube(string timekey, string equipment)
        {
            StringBuilder tmpsb = new StringBuilder();
#if MSSQL
            tmpsb.Append("SELECT top 1 TUBE FROM TAPFTBMINFO ");
            tmpsb.AppendFormat(" WHERE UPDATETIME <='{0}'AND EQUIPMENT='{1}'", timekey, equipment);
            tmpsb.Append(" AND REASON IS NOT NULL ");
            tmpsb.Append(" AND REASON !='Other EQ Problem Cause BM'  ");
            tmpsb.Append(" AND REASON !='Process have Some Problem' ");
            tmpsb.Append(" AND (PHENOMENON !='' OR  MEASURES!='') ");
            tmpsb.Append(" ORDER BY UPDATETIME DESC");
#endif
#if ORACLE
            tmpsb.Append("SELECT * FROM ( ");
            tmpsb.Append("SELECT TUBE FROM TAPFTBMINFO ");
            tmpsb.AppendFormat(" WHERE UPDATETIME <='{0}'AND EQUIPMENT='{1}'", timekey, equipment);
            tmpsb.Append(" AND REASON IS NOT NULL ");
            tmpsb.Append(" AND REASON !='Other EQ Problem Cause BM'  ");
            tmpsb.Append(" AND REASON !='Process have Some Problem' ");
            tmpsb.Append(" AND (PHENOMENON !='' OR  MEASURES!='') ");
            tmpsb.Append(" ORDER BY UPDATETIME DESC) WHERE ROWNUM=1 ");
#endif
            DataTable tubetable = db.Select(tmpsb.ToString()).Tables[0];
            if (tubetable.Rows.Count > 0)
            {
                return tubetable.Rows[0]["TUBE"].ToString();
            }
            else
            {
                return "";
            }

        }
        private DataTable GetEQHistory(string equipment)
        {
            StringBuilder tmpsb = new StringBuilder();
#if MSSQL
            tmpsb.Append("SELECT TOP 1 AREA,LINE,MAINEQUIPMENT,NAME,REGION,FACILITY,EQUIPMENTTYPE ,LASTEVENTTIME,EQUIPMENTSTATUS FROM TAPFTEQUIPMENTHISTORY ");
            tmpsb.Append("  WHERE ISALIVE='YES' ");
            tmpsb.Append(" AND  EQUIPMENTTYPE NOT IN ('G','F','T')");
            tmpsb.Append(" AND LASTEVENTTIME <= ");
            tmpsb.AppendFormat(" '{0}'", startTime);
            tmpsb.AppendFormat(" AND NAME='{0}'", equipment);
            tmpsb.Append(" ORDER BY LASTEVENTTIME DESC");
#endif
#if ORACLE
            tmpsb.Append("SELECT * FROM ( ");
            tmpsb.Append("SELECT AREA,LINE,MAINEQUIPMENT,NAME,REGION,FACILITY,EQUIPMENTTYPE ,LASTEVENTTIME,EQUIPMENTSTATUS FROM TAPFTEQUIPMENTHISTORY ");
            tmpsb.Append("  WHERE ISALIVE='YES' ");
            tmpsb.Append(" AND  EQUIPMENTTYPE NOT IN ('G','F','T')");
            tmpsb.Append(" AND LASTEVENTTIME <= ");
            tmpsb.AppendFormat(" '{0}'", startTime);
            tmpsb.AppendFormat(" AND NAME='{0}'", equipment);
            tmpsb.Append(" ORDER BY LASTEVENTTIME DESC) WHERE ROWNUM=1");
#endif
            return db.Select(tmpsb.ToString()).Tables[0];
        }
        public Worksheet GetExcelISheet(string filepath, string sheetname, string sheetname2)
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
                xSheet2 = workbook.Worksheets[sheetname2];
                xSheet3 = workbook.Worksheets[sheetname3];
                return xSheet;
            }
            catch (Exception EX)
            {
                throw EX;
            }

        }
        private void SaveToExcel()
        {
            //RUN
            xSheet.Range["A4"].Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(208)))), ((int)(((byte)(80)))));
            //BM
            xSheet.Range["D4"].Style.Color = Color.Red;
            //PM
            xSheet.Range["G4"].Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(155)))), ((int)(((byte)(213)))));
            //CHECK DOWN
            xSheet.Range["J4"].Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(125)))), ((int)(((byte)(49)))));
            //IDLE
            xSheet.Range["M4"].Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            //PROCESS
            xSheet.Range["P4"].Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(204)))));
            //RD TEST
            xSheet.Range["S4"].Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(48)))), ((int)(((byte)(160)))));
            //UTILITY DOWN
            xSheet.Range["V4"].Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(165)))), ((int)(((byte)(165)))));
            //MATERIAL TEST
            xSheet.Range["Y4"].Style.Color = Color.DodgerBlue;
            if (!Directory.Exists(Path.GetDirectoryName(Savefilepath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Savefilepath));
            }
            workbook.CalculateAllValue();
            workbook.SaveToFile(Savefilepath, ExcelVersion.Version2013);
            workbook.Worksheets[0].SaveToImage(Savefilepath.Replace("XLSX", "Jpeg"));
        }
        private void TestOperatingRatio(DataTable EqOperatingRatioDt)
        {
            EqStatusStartAndEnd = new StatusStartAndEnd();
            EqStatusStartAndEnd.StatusStartAndEndAdd = new List<StatusStartAndEnd>();
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = PMSTART, StatusEnd = CHECKDOWN, StatusName = "CPM" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = PMSTART, StatusEnd = IDLE, StatusName = "CPM" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = PMSTART, StatusEnd = UTILITY, StatusName = "CPM" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = PMSTART, StatusEnd = "", StatusName = "CPM" });

            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = CHECKDOWN, StatusEnd = RUN, StatusName = "CCHECK" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = CHECKDOWN, StatusEnd = IDLE, StatusName = "CCHECK" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = CHECKDOWN, StatusEnd = UTILITY, StatusName = "CCHECK" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = CHECKDOWN, StatusEnd = "", StatusName = "CCHECK" });

            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = RANDDTEST, StatusEnd = RUN, StatusName = "CR&D TEST" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = RANDDTEST, StatusEnd = IDLE, StatusName = "CR&D TEST" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = RANDDTEST, StatusEnd = UTILITY, StatusName = "CR&D TEST" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = RANDDTEST, StatusEnd = "", StatusName = "CR&D TEST" });

            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = BMWAIT, StatusEnd = UTILITY, StatusName = "CBUTILITY" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = BMWAIT, StatusEnd = CHECKDOWN, StatusName = "CBM1" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = BMWAIT, StatusEnd = BMSTART, StatusName = "CBM3" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = BMWAIT, StatusEnd = PROCESSDOWN, StatusName = "CBPROCESS" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = BMWAIT, StatusEnd = IDLE, StatusName = "BM WAIT" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = BMWAIT, StatusEnd = "", StatusName = "BM WAIT" });

            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = BMSTART, StatusEnd = UTILITY, StatusName = "CBUTILITY" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = BMSTART, StatusEnd = CHECKDOWN, StatusName = "CBM2" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = BMSTART, StatusEnd = PROCESSDOWN, StatusName = "CBPROCESS" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = BMSTART, StatusEnd = IDLE, StatusName = "BM START" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = BMSTART, StatusEnd = "", StatusName = "BM START" });

            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = MATERIAL, StatusEnd = RUN, StatusName = "CMATERIAL" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = MATERIAL, StatusEnd = IDLE, StatusName = "CMATERIAL" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = MATERIAL, StatusEnd = UTILITY, StatusName = "CMATERIAL" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = MATERIAL, StatusEnd = "", StatusName = "CMATERIAL" });

            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = PROCESSTEST, StatusEnd = RUN, StatusName = "CPROCESS" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = PROCESSTEST, StatusEnd = IDLE, StatusName = "CPROCESS" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = PROCESSTEST, StatusEnd = UTILITY, StatusName = "CPROCESS" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = PROCESSTEST, StatusEnd = "", StatusName = "CPROCESS" });

            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = PROCESSDOWN, StatusEnd = CHECKDOWN, StatusName = "CPROCESS" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = PROCESSDOWN, StatusEnd = IDLE, StatusName = "CPROCESS" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = PROCESSDOWN, StatusEnd = UTILITY, StatusName = "CPROCESS" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = PROCESSDOWN, StatusEnd = "", StatusName = "CPROCESS" });

            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = RUN, StatusEnd = IDLE, StatusName = "CRUN" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = RUN, StatusEnd = UTILITY, StatusName = "CRUN" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = RUN, StatusEnd = PMSTART, StatusName = "CRUN" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = RUN, StatusEnd = PROCESSTEST, StatusName = "CRUN" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = RUN, StatusEnd = PROCESSDOWN, StatusName = "CRUN" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = RUN, StatusEnd = CHECKDOWN, StatusName = "CRUN" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = RUN, StatusEnd = RANDDTEST, StatusName = "CRUN" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = RUN, StatusEnd = BMWAIT, StatusName = "CRUN" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = RUN, StatusEnd = BMSTART, StatusName = "CRUN" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = RUN, StatusEnd = MATERIAL, StatusName = "CRUN" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = RUN, StatusEnd = MODIFY, StatusName = "CRUN" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = RUN, StatusEnd = "", StatusName = "CRUN" });

            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = IDLE, StatusEnd = RUN, StatusName = "CIDLE" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = IDLE, StatusEnd = PMSTART, StatusName = "CIDLE" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = IDLE, StatusEnd = PROCESSTEST, StatusName = "CIDLE" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = IDLE, StatusEnd = PROCESSDOWN, StatusName = "CIDLE" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = IDLE, StatusEnd = CHECKDOWN, StatusName = "CIDLE" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = IDLE, StatusEnd = RANDDTEST, StatusName = "CIDLE" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = IDLE, StatusEnd = UTILITY, StatusName = "CIDLE" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = IDLE, StatusEnd = BMWAIT, StatusName = "CIDLE" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = IDLE, StatusEnd = BMSTART, StatusName = "CIDLE" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = IDLE, StatusEnd = MATERIAL, StatusName = "CIDLE" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = IDLE, StatusEnd = "", StatusName = "CIDLE" });

            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = UTILITY, StatusEnd = CHECKDOWN, StatusName = "CUTILITY" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = UTILITY, StatusEnd = IDLE, StatusName = "CUTILITY" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = UTILITY, StatusEnd = RUN, StatusName = "CUTILITY" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = UTILITY, StatusEnd = PMSTART, StatusName = "CUTILITY" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = UTILITY, StatusEnd = PROCESSTEST, StatusName = "CUTILITY" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = UTILITY, StatusEnd = PROCESSDOWN, StatusName = "CUTILITY" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = UTILITY, StatusEnd = RANDDTEST, StatusName = "CUTILITY" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = UTILITY, StatusEnd = BMWAIT, StatusName = "CUTILITY" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = UTILITY, StatusEnd = BMSTART, StatusName = "CUTILITY" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = UTILITY, StatusEnd = MATERIAL, StatusName = "CUTILITY" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = UTILITY, StatusEnd = "", StatusName = "CUTILITY" });

            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = MODIFY, StatusEnd = RUN, StatusName = "CMODIFY" });
            EqStatusStartAndEnd.StatusStartAndEndAdd.Add(new StatusStartAndEnd { StatusStart = MODIFY, StatusEnd = "", StatusName = "EQUIPMENT MODIFY" });
            if (EqDtHistory != null && EqDtHistory.Rows.Count > 0)
            {
                DataTable dat = EqDtHistory.DefaultView.ToTable(true, new string[] { "NAME" });
                var names = (from d in dat.AsEnumerable() select d.Field<string>("NAME")).ToList();
                DataSet EqDtHistoryDs = new DataSet();
                //将EqDtHistory表数据分成多个datatable(group by EQMTTYPE)
                for (int i = 0; i < names.Count; i++)
                {
                    DataTable table = new DataTable();
                    table = EqDtHistory.Clone();
                    table.TableName = names[i];
                    EqDtHistoryDs.Tables.Add(table);
                }
                for (int i = 0; i < EqDtHistory.Rows.Count; i++)
                {
                    if (EqDtHistoryDs.Tables[EqDtHistory.Rows[i]["NAME"].ToString()] != null)
                    {
                        EqDtHistoryDs.Tables[EqDtHistory.Rows[i]["NAME"].ToString()].Rows.Add(EqDtHistory.Rows[i].ItemArray);
                    }
                }
                for (int ii = 0; ii < EqDtHistoryDs.Tables.Count; ii++)
                {
                    DataTable EqHistory = EqDtHistoryDs.Tables[ii].DefaultView.ToTable();
                    EqHistory.DefaultView.Sort = " NAME ASC,LASTEVENTTIME ASC";
                    EqHistory = EqHistory.DefaultView.ToTable();
                    if (EqHistory.Rows.Count > 0)
                    {
                        GetStatusAndTime(EqHistory, EqOperatingRatioDt);
                    }
                }
            }
        }
        private void GetStatusAndTime(DataTable EqHistory, DataTable EqOperatingRatioDt)
        {
            DateTime stastartTime = DateTime.MinValue;
            DateTime staendtime = DateTime.MinValue;
            TimeSpan span;
            double cntsed = 0;
            string eqname;
            string area;
            string equipmenttype;
            string line;

            Dictionary<string, string> nextStatus = new Dictionary<string, string>();
            for (int i = 0; i < EqHistory.Rows.Count; i++)
            {
                //保存信息的状态名
                string EQSTATUS = "";
                //当前eq的状态
                string currentEqStatus = EqHistory.Rows[i]["EQUIPMENTSTATUS"].ToString().ToUpper();
                eqname = EqHistory.Rows[i]["NAME"].ToString();
                area = EqHistory.Rows[i]["AREA"].ToString();
                equipmenttype = EqHistory.Rows[i]["EQUIPMENTTYPE"].ToString();
                line = EqHistory.Rows[i]["LINE"].ToString();
                string eqtime = EqHistory.Rows[i]["LASTEVENTTIME"].ToString();

                if (i == 0)
                {//获取上一条eq信息
                    string eqstatus = GetEqLastStatus(EqHistory.Rows[i]["NAME"].ToString(), eqtime).ToUpper();
                    //获取上一条结束对应的状态                  
                    //status.count不等于1,为BM状态
                    //key是结束的状态，value是添加到EqOperatingRatioDt中EQSTATUS列的值
                    Dictionary<string, string> status = GetEqStatusEndValue(EqStatusStartAndEnd, eqstatus);
                    if (string.IsNullOrEmpty(eqstatus))
                    {
                        EQSTATUS = currentEqStatus;
                        stastartTime = OutTime(eqtime, stastartTime);
                        nextStatus = GetEqStatusEndValue(EqStatusStartAndEnd, currentEqStatus);
                        continue;
                    }
                    else
                    {
                        if (eqstatus == RUN || eqstatus == IDLE || eqstatus == UTILITY)
                        {
                            stastartTime = OutTime(startTime, stastartTime);
                            nextStatus = GetEqStatusEndValue(EqStatusStartAndEnd, eqstatus);
                        }
                        else
                        {
                            stastartTime = OutTime(startTime, stastartTime);
                            int index = -1;
                            for (int j = i; j < EqHistory.Rows.Count; j++)
                            {
                                if (status.Keys.Contains(EqHistory.Rows[j]["EQUIPMENTSTATUS"].ToString().ToUpper()))
                                {
                                    index = j;
                                    EQSTATUS = status[EqHistory.Rows[j]["EQUIPMENTSTATUS"].ToString().ToUpper()];
                                    break;
                                }
                            }
                            if (index == -1)
                            {
                                EQSTATUS = status.First().Value;
                                if (stastartTime != DateTime.MinValue)
                                {
                                    staendtime = OutTime(endTime, staendtime);
                                    span = staendtime - stastartTime;
                                    cntsed += (double)Math.Round(span.TotalHours, 2);
                                    EQSTATUS = GetNextStatus(EQSTATUS, i, EqHistory);
                                    EqOperatingRatioDt.Rows.Add(new object[] { eqname, area, equipmenttype, cntsed, line, EQSTATUS, staendtime.ToString("yyyyMMddHHmmss") });
                                    cntsed = 0;
                                    stastartTime = DateTime.MinValue;
                                    staendtime = DateTime.MinValue;
                                }
                                area = EqHistory.Rows[i]["AREA"].ToString();
                                equipmenttype = EqHistory.Rows[i]["EQUIPMENTTYPE"].ToString();
                                line = EqHistory.Rows[i]["LINE"].ToString();

                                break;
                            }
                            else
                            {
                                i = index;
                                eqtime = EqHistory.Rows[i]["LASTEVENTTIME"].ToString();
                                string EqStatus = EqHistory.Rows[i]["EQUIPMENTSTATUS"].ToString().ToUpper();
                                staendtime = OutTime(eqtime, staendtime);
                                nextStatus = GetEqStatusEndValue(EqStatusStartAndEnd, EqStatus);
                                span = staendtime - stastartTime;
                                cntsed += (double)Math.Round(span.TotalHours, 2);
                                EQSTATUS = GetNextStatus(EQSTATUS, i, EqHistory);
                                EqOperatingRatioDt.Rows.Add(new object[] { eqname, area, equipmenttype, cntsed, line, EQSTATUS, staendtime.ToString("yyyyMMddHHmmss") });
                                cntsed = 0;
                                stastartTime = DateTime.MinValue;
                                staendtime = DateTime.MinValue;
                                stastartTime = OutTime(eqtime, stastartTime);
                                continue;
                            }
                        }
                    }
                }

                if (nextStatus.Count() > 0)
                {
                    //if (string.IsNullOrEmpty(nextStatus.First().Key))
                    //{
                    //    string str1 = nextStatus.First().Value.Remove(0, 1);
                    //    if (currentEqStatus != str1)
                    //    {
                    //        if (stastartTime != DateTime.MinValue)
                    //        {
                    //            EQSTATUS = nextStatus.First().Value;
                    //            staendtime = OutTime(eqtime, staendtime);
                    //            span = staendtime - stastartTime;
                    //            cntsed += (double)Math.Round(span.TotalHours, 2);
                    //            stastartTime = OutTime(eqtime, stastartTime);
                    //            EQSTATUS = GetNextStatus(EQSTATUS, i, EqHistory);
                    //            EqOperatingRatioDt.Rows.Add(new object[] { eqname, area, equipmenttype, cntsed, line, EQSTATUS, staendtime.ToString("yyyyMMddhhmmss") });
                    //            cntsed = 0;
                    //            stastartTime = DateTime.MinValue;
                    //            staendtime = DateTime.MinValue;
                    //            nextStatus = GetEqStatusEndValue(EqStatusStartAndEnd, currentEqStatus);
                    //        }
                    //        if (stastartTime == DateTime.MinValue)
                    //        {
                    //            stastartTime = OutTime(eqtime, stastartTime);
                    //        }

                    //    }

                    //}
                    //else
                    if (nextStatus.Keys.Contains(currentEqStatus))
                    {
                        if (stastartTime != DateTime.MinValue)
                        {
                            EQSTATUS = nextStatus[currentEqStatus];
                            staendtime = OutTime(eqtime, staendtime);
                            span = staendtime - stastartTime;
                            cntsed += (double)Math.Round(span.TotalHours, 2);
                            stastartTime = OutTime(eqtime, stastartTime);
                            EQSTATUS = GetNextStatus(EQSTATUS, i, EqHistory);
                            EqOperatingRatioDt.Rows.Add(new object[] { eqname, area, equipmenttype, cntsed, line, EQSTATUS, staendtime.ToString("yyyyMMddHHmmss") });
                            cntsed = 0;
                            stastartTime = DateTime.MinValue;
                            staendtime = DateTime.MinValue;
                            nextStatus = GetEqStatusEndValue(EqStatusStartAndEnd, currentEqStatus);

                        }
                        if (stastartTime == DateTime.MinValue)
                        {
                            stastartTime = OutTime(eqtime, stastartTime);
                        }
                    }
                    if (i == EqHistory.Rows.Count - 1 && stastartTime != DateTime.MinValue)
                    {
                        if (nextStatus.Count() > 0)
                        {
                            //EQSTATUS = nextStatus.First().Value;
                            EQSTATUS = nextStatus[""];
                        }
                        staendtime = OutTime(endTime, staendtime);
                        span = staendtime - stastartTime;
                        cntsed += (double)Math.Round(span.TotalHours, 2);
                        EQSTATUS = GetNextStatus(EQSTATUS, i, EqHistory);
                        EqOperatingRatioDt.Rows.Add(new object[] { eqname, area, equipmenttype, cntsed, line, EQSTATUS, staendtime.ToString("yyyyMMddHHmmss") });
                        cntsed = 0;
                        stastartTime = DateTime.MinValue;
                        staendtime = DateTime.MinValue;
                    }
                }


            }
        }
        private string GetNextStatus(string EQSTATUS, int CurrentIndex, DataTable EqHistory)
        {
            EqHistory.DefaultView.Sort = "LASTEVENTTIME ASC";
            EqHistory = EqHistory.DefaultView.ToTable();
            if (EQSTATUS == "CBM3" && EqHistory.Rows.Count - 1 > CurrentIndex)
            {
                if (EqHistory.Rows[CurrentIndex + 1]["EQUIPMENTSTATUS"].ToString().ToUpper() == UTILITY)
                {
                    return EQSTATUS = "CBUTILITY";
                }
                if (EqHistory.Rows[CurrentIndex + 1]["EQUIPMENTSTATUS"].ToString().ToUpper() == PROCESSDOWN)
                {
                    return EQSTATUS = "CBPROCESS";
                }
            }
            else
            if (EQSTATUS == CHECKDOWN || EQSTATUS == "CCHECK")
            {
                string eqstatus = GetEqCheckDownLastStatus(EqHistory.Rows[CurrentIndex]["NAME"].ToString(), EqHistory.Rows[CurrentIndex]["LASTEVENTTIME"].ToString()).ToUpper();
                if (!string.IsNullOrEmpty(eqstatus))
                {
                    if (eqstatus.ToUpper() == BMSTART || eqstatus.ToUpper() == BMWAIT)
                    {
                        return EQSTATUS = "BMCHECK";
                    }
                    else if (eqstatus.ToUpper() == PMSTART)
                    {
                        return EQSTATUS = "PMCHECK";
                    }
                }
            }
            return EQSTATUS;
        }


        private Dictionary<string, string> GetEqStatusEndValue(StatusStartAndEnd statusStartAndEnd, string key)
        {
            Dictionary<string, string> strs = new Dictionary<string, string>();

            foreach (var item in statusStartAndEnd.StatusStartAndEndAdd)
            {
                if (item.StatusStart == key)
                {
                    strs.Add(item.StatusEnd, item.StatusName);

                }
            }
            return strs;
        }
        private DateTime OutTime(string strTime, DateTime dateTime)
        {
            if (!string.IsNullOrEmpty(strTime))
            {
                DateTime.TryParse(
                            strTime.Substring(0, 4) + "-" +
                            strTime.Substring(4, 2) + "-" +
                            strTime.Substring(6, 2) + " " +
                            strTime.Substring(8, 2) + ":" +
                            strTime.Substring(10, 2) + ":" +
                            strTime.Substring(12, 2), out dateTime);
            }
            return dateTime;

        }
        private string GetBmTime(string wait, string start, string end)
        {
            TimeSpan timeSpan = TimeSpan.Zero;
            TimeSpan endtimeSpan = TimeSpan.Zero;
            DateTime waitTime = DateTime.MinValue;
            DateTime startTime = DateTime.MinValue;
            DateTime endTime = DateTime.MinValue;
            waitTime = OutTime(wait, waitTime);
            startTime = OutTime(start, startTime);
            endTime = OutTime(end, endTime);

            if (waitTime == DateTime.MinValue && startTime != DateTime.MinValue)
            {
                endtimeSpan = (endTime - startTime);
            }
            else
            if (startTime == DateTime.MinValue && waitTime != DateTime.MinValue)
            {
                endtimeSpan = (endTime - waitTime);
            }
            else if (startTime != DateTime.MinValue && waitTime != DateTime.MinValue)
            {
                endtimeSpan = (endTime - waitTime);
            }
            timeSpan = endtimeSpan;
            return ((double)Math.Round(timeSpan.TotalHours, 2)).ToString();
        }
        private string GetEqLastStatus(string name, string lasteventtime)
        {
            StringBuilder tmpsb = new StringBuilder();
#if MSSQL
            tmpsb.Append("SELECT TOP (1)AREA,LINE,MAINEQUIPMENT,NAME,REGION,FACILITY,EQUIPMENTTYPE ,LASTEVENTTIME,EQUIPMENTSTATUS FROM TAPFTEQUIPMENTHISTORY ");
            tmpsb.AppendFormat(" WHERE NAME = '{0}'AND LASTEVENTTIME<'{1}'", name, lasteventtime);
            tmpsb.Append(" ORDER BY LASTEVENTTIME DESC");
#endif
#if ORACLE
            tmpsb.Append("SELECT * FROM ( ");
            tmpsb.Append("SELECT AREA,LINE,MAINEQUIPMENT,NAME,REGION,FACILITY,EQUIPMENTTYPE ,LASTEVENTTIME,EQUIPMENTSTATUS FROM TAPFTEQUIPMENTHISTORY ");
            tmpsb.AppendFormat(" WHERE NAME = '{0}'AND LASTEVENTTIME<'{1}'", name, lasteventtime);
            tmpsb.Append(" ORDER BY LASTEVENTTIME DESC) WHERE ROWNUM=1 ");
#endif
            DataTable EqDtHistory = db.Select(tmpsb.ToString()).Tables[0];
            string eqstatus = "";
            if (EqDtHistory.Rows.Count > 0)
            {
                eqstatus = EqDtHistory.Rows[0]["EQUIPMENTSTATUS"].ToString();
            }
            return eqstatus.ToUpper();
        }
        private string GetEqCheckDownLastStatus(string name, string lasteventtime)
        {
            StringBuilder tmpsb = new StringBuilder();
#if MSSQL
            tmpsb.Append("SELECT TOP (1)AREA,LINE,MAINEQUIPMENT,NAME,REGION,FACILITY,EQUIPMENTTYPE ,LASTEVENTTIME,EQUIPMENTSTATUS FROM TAPFTEQUIPMENTHISTORY ");
            tmpsb.AppendFormat(" WHERE NAME = '{0}'AND LASTEVENTTIME<'{1}'", name, lasteventtime);
            tmpsb.Append("  AND EQUIPMENTSTATUS !='CHECK DOWN'");
            tmpsb.Append(" ORDER BY LASTEVENTTIME DESC");
#endif
#if ORACLE
            tmpsb.Append("SELECT * FROM ( ");
            tmpsb.Append("SELECT AREA,LINE,MAINEQUIPMENT,NAME,REGION,FACILITY,EQUIPMENTTYPE ,LASTEVENTTIME,EQUIPMENTSTATUS FROM TAPFTEQUIPMENTHISTORY ");
            tmpsb.AppendFormat(" WHERE NAME = '{0}'AND LASTEVENTTIME<'{1}'", name, lasteventtime);
            tmpsb.Append("  AND EQUIPMENTSTATUS !='CHECK DOWN'");
            tmpsb.Append(" ORDER BY LASTEVENTTIME DESC) WHERE ROWNUM=1 ");
#endif
            DataTable EqDtHistory = db.Select(tmpsb.ToString()).Tables[0];
            string eqstatus = "";
            if (EqDtHistory.Rows.Count > 0)
            {
                eqstatus = EqDtHistory.Rows[0]["EQUIPMENTSTATUS"].ToString();
            }
            return eqstatus.ToUpper();
        }

        private void OperatingRatio(int TitleIndex, int StartRowIndex, int StartColIndex, int EndColIndex)
        {
            //获取excel中列的字段名和顺序
            Dictionary<string, int> OperatingRatioHeadDc = new Dictionary<string, int>();
            for (int i = StartColIndex; i < EndColIndex; i++)
            {
                string value = xSheet.Range[TitleIndex, i].Value.ToString();

                OperatingRatioHeadDc.Add(value.ToString(), i);
                //  IRow headrow = xSheet.GetRow(TitleIndex);
                //ICell headcell = headrow.GetCell(i);
                // OperatingRatioHeadDc.Add(headcell.ToString(), i);
            }
            foreach (var item in OperatingRatioHeadDc)
            {
                string sql = "";
                if (!item.Key.Contains("-"))
                {
                    sql = string.Format("AREA='{0}'", item.Key.Trim());
                }
                else
                {
                    string[] str = item.Key.Split('-');
                    if (item.Key.Contains("ALL"))
                    {
                        sql = string.Format(" EQUIPMENTTYPE='{0}'", str[0].Trim());

                    }
                    else
                    {
                        sql = string.Format("AREA='{0}' AND EQUIPMENTTYPE='{1}'", str[0].Trim(), str[1].Trim());
                    }
                }
                EqOperatingRatioDt.DefaultView.RowFilter = sql;
                DataTable FilterDt = EqOperatingRatioDt.DefaultView.ToTable();
                EqOperatingRatioDt.DefaultView.RowFilter = string.Empty;
                //    FilterDt.DefaultView.RowFilter = "EQSTATUS in( BM1,BM2,BM3,BM START,BM WAIT)";
                DataTable bmDt = FilterDt.DefaultView.ToTable();
                object bmsumtime = bmDt.Compute("SUM(TIME)", "EQSTATUS in('CBPROCESS','CBUTILITY','CBM1','CBM2','CBM3','BM START','BM WAIT')");
                object pmsumtime = bmDt.Compute("SUM(TIME)", "EQSTATUS in('CPM','PM START')");
                object bmchecksumtime = bmDt.Compute("SUM(TIME)", "EQSTATUS in('BMCHECK')");
                object pmchecksumtime = bmDt.Compute("SUM(TIME)", "EQSTATUS in('PMCHECK')");
                object checksumtime = bmDt.Compute("SUM(TIME)", "EQSTATUS in('CHECK DOWN','CCHECK')");
                object idlesumtime = bmDt.Compute("SUM(TIME)", "EQSTATUS  in('CIDLE','IDLE')");
                object processsumtime = bmDt.Compute("SUM(TIME)", "EQSTATUS in('CPROCESS','PROCESS TEST','PROCESS DOWN')");
                object developsumtime = bmDt.Compute("SUM(TIME)", "EQSTATUS in('CR&D TEST','R&D TEST') ");
                object utilitysumtime = bmDt.Compute("SUM(TIME)", "EQSTATUS in('CUTILITY','UTILITY','UTILITY DOWN')");
                object materiasumtime = bmDt.Compute("SUM(TIME)", "EQSTATUS in('CMATERIAL','MATERIAL','MATERIAL TEST')");
                object modifysumtime = bmDt.Compute("SUM(TIME)", "EQSTATUS in('CMODIFY','EQUIPMENT MODIFY')");
                double bmtime = bmsumtime == System.DBNull.Value ? 0 : (double)bmsumtime;
                double pmtime = pmsumtime == System.DBNull.Value ? 0 : (double)pmsumtime;
                double bmchecktime = bmchecksumtime == System.DBNull.Value ? 0 : (double)bmchecksumtime;
                double pmchecktime = pmchecksumtime == System.DBNull.Value ? 0 : (double)pmchecksumtime;
                double checktime = checksumtime == System.DBNull.Value ? 0 : (double)checksumtime;
                double idletime = idlesumtime == System.DBNull.Value ? 0 : (double)idlesumtime;
                double processtime = processsumtime == System.DBNull.Value ? 0 : (double)processsumtime;
                double developtime = developsumtime == System.DBNull.Value ? 0 : (double)developsumtime;
                double utilitytime = utilitysumtime == System.DBNull.Value ? 0 : (double)utilitysumtime;
                double materiatime = materiasumtime == System.DBNull.Value ? 0 : (double)materiasumtime;
                double modifytime = modifysumtime == System.DBNull.Value ? 0 : (double)modifysumtime;
                //获取数据总条数
                DataTable data = FilterDt.DefaultView.ToTable(true, new string[] { "NAME" });
                double totaltime = data.Rows.Count * timeSpan.TotalHours;
                //需要改
                double bm = totaltime == 0 ? 0 : bmtime / totaltime;
                double pm = totaltime == 0 ? 0 : pmtime / totaltime;
                double bmcheck = totaltime == 0 ? 0 : bmchecktime / totaltime;
                double pmcheck = totaltime == 0 ? 0 : pmchecktime / totaltime;
                double check = totaltime == 0 ? 0 : checktime / totaltime;
                double idle = totaltime == 0 ? 0 : idletime / totaltime;
                double process = totaltime == 0 ? 0 : processtime / totaltime;
                double develop = totaltime == 0 ? 0 : developtime / totaltime;
                double utility = totaltime == 0 ? 0 : utilitytime / totaltime;
                double materia = totaltime == 0 ? 0 : materiatime / totaltime;
                double modify = totaltime == 0 ? 0 : modifytime / totaltime;

                xSheet.Range[(StartRowIndex), item.Value].NumberValue = Convert.ToDouble(bm);
                xSheet.Range[(StartRowIndex + 1), item.Value].NumberValue = Convert.ToDouble(pm);
                xSheet.Range[(StartRowIndex + 2), item.Value].NumberValue = Convert.ToDouble(bmcheck);
                xSheet.Range[(StartRowIndex + 3), item.Value].NumberValue = Convert.ToDouble(pmcheck);
                xSheet.Range[(StartRowIndex + 4), item.Value].NumberValue = Convert.ToDouble(check);
                xSheet.Range[(StartRowIndex + 5), item.Value].NumberValue = Convert.ToDouble(idle);
                xSheet.Range[(StartRowIndex + 6), item.Value].NumberValue = Convert.ToDouble(process);
                xSheet.Range[(StartRowIndex + 7), item.Value].NumberValue = Convert.ToDouble(develop);
                xSheet.Range[(StartRowIndex + 8), item.Value].NumberValue = Convert.ToDouble(utility);
                xSheet.Range[(StartRowIndex + 9), item.Value].NumberValue = Convert.ToDouble(modify);
                xSheet.Range[(StartRowIndex + 10), item.Value].NumberValue = Convert.ToDouble(materia);

                xSheet.Range[(StartRowIndex), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 1), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 2), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 3), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 4), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 5), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 6), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 7), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 8), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 9), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 10), item.Value].NumberFormat = "0.00%";
            }

        }
        private void CalcCount(string status, int TitleIndex, int StartRowIndex, int StartColIndex, int EndColIndex)
        {
            //获取excel中列的字段名和顺序
            DataTable WriteExcelDt = new DataTable();
            for (int i = StartColIndex; i < EndColIndex; i++)
            {
                string value = xSheet.Range[TitleIndex, i].Value.ToString();
                WriteExcelDt.Columns.Add(value, Type.GetType("System.Int32"));
            }

            //获取excel中列的字段名和顺序
            Dictionary<string, int> OperatingRatioHeadDc = new Dictionary<string, int>();
            for (int i = StartColIndex; i < EndColIndex; i++)
            {
                string value = xSheet.Range[TitleIndex, i].Value.ToString();
                OperatingRatioHeadDc.Add(value, i);
            }
            foreach (var item in OperatingRatioHeadDc)
            {
                string sql = "";
                if (!item.Key.Contains("-"))
                {
                    sql = string.Format("AREA='{0}'", item.Key);
                }
                else
                {
                    string[] str = item.Key.Split('-');
                    if (item.Key.Contains("ALL"))
                    {
                        sql = string.Format(" EQUIPMENTTYPE='{0}'", str[0]);
                    }
                    else
                    {
                        sql = string.Format("AREA='{0}' AND EQUIPMENTTYPE='{1}'", str[0], str[1]);
                    }
                }
                DataTable dt = null;
                if (status == "BM")
                {
                    EqOperatingRatioDt.DefaultView.RowFilter = "EQSTATUS in('BM START','BM WAIT','CBM1','CBM2','CBM3')";
                    dt = EqOperatingRatioDt.DefaultView.ToTable();
                }
                else if (status == "PM")
                {
                    EqOperatingRatioDt.DefaultView.RowFilter = "EQSTATUS in('CPM','PM START')";
                    dt = EqOperatingRatioDt.DefaultView.ToTable();
                }
                dt.DefaultView.RowFilter = sql;
                DataTable FilterDt = dt.DefaultView.ToTable();
                EqOperatingRatioDt.DefaultView.RowFilter = string.Empty;
                //ChangeDt.DefaultView.ToTable(true, new string[] { "NAME" });
                FilterDt.DefaultView.RowFilter = string.Format("LINE='{0}'", "311");
                double WS311 = FilterDt.DefaultView.ToTable(true, new string[] { "NAME" }).Rows.Count;
                FilterDt.DefaultView.RowFilter = string.Format("LINE='{0}'", "321");
                double WS321 = FilterDt.DefaultView.ToTable(true, new string[] { "NAME" }).Rows.Count;
                FilterDt.DefaultView.RowFilter = string.Format("LINE='{0}'", "511");
                double WS511 = FilterDt.DefaultView.ToTable(true, new string[] { "NAME" }).Rows.Count;
                FilterDt.DefaultView.RowFilter = string.Format("LINE='{0}'", "521");
                double WS521 = FilterDt.DefaultView.ToTable(true, new string[] { "NAME" }).Rows.Count;
                FilterDt.DefaultView.RowFilter = string.Format("LINE='{0}'", "611");
                double WS611 = FilterDt.DefaultView.ToTable(true, new string[] { "NAME" }).Rows.Count;
                FilterDt.DefaultView.RowFilter = string.Format("LINE='{0}'", "631");
                double WS631 = FilterDt.DefaultView.ToTable(true, new string[] { "NAME" }).Rows.Count;
                FilterDt.DefaultView.RowFilter = string.Format("LINE='{0}'", "632");
                double WS632 = FilterDt.DefaultView.ToTable(true, new string[] { "NAME" }).Rows.Count;
                FilterDt.DefaultView.RowFilter = string.Format("LINE='{0}'", "651");
                double WS651 = FilterDt.DefaultView.ToTable(true, new string[] { "NAME" }).Rows.Count;
                xSheet.Range[(StartRowIndex), item.Value].NumberValue = WS311;
                xSheet.Range[(StartRowIndex + 1), item.Value].NumberValue = WS321;
                xSheet.Range[(StartRowIndex + 2), item.Value].NumberValue = WS511;
                xSheet.Range[(StartRowIndex + 3), item.Value].NumberValue = WS521;
                xSheet.Range[(StartRowIndex + 4), item.Value].NumberValue = WS611;
                xSheet.Range[(StartRowIndex + 5), item.Value].NumberValue = WS631;
                xSheet.Range[(StartRowIndex + 6), item.Value].NumberValue = WS632;
                xSheet.Range[(StartRowIndex + 7), item.Value].NumberValue = WS651;
                xSheet.Range[(StartRowIndex), item.Value].NumberFormat = "0";
                xSheet.Range[(StartRowIndex + 1), item.Value].NumberFormat = "0";
                xSheet.Range[(StartRowIndex + 2), item.Value].NumberFormat = "0";
                xSheet.Range[(StartRowIndex + 3), item.Value].NumberFormat = "0";
                xSheet.Range[(StartRowIndex + 4), item.Value].NumberFormat = "0";
                xSheet.Range[(StartRowIndex + 5), item.Value].NumberFormat = "0";
                xSheet.Range[(StartRowIndex + 6), item.Value].NumberFormat = "0";
                xSheet.Range[(StartRowIndex + 7), item.Value].NumberFormat = "0";
            }
        }
        private void CalcgTime(string status, int TitleIndex, int StartRowIndex, int StartColIndex, int EndColIndex)
        {
            //获取excel中列的字段名和顺序
            DataTable WriteExcelDt = new DataTable();
            for (int i = StartColIndex; i < EndColIndex; i++)
            {
                string value = xSheet.Range[TitleIndex, i].Value.ToString();
                WriteExcelDt.Columns.Add(value, Type.GetType("System.Double"));
            }

            //获取excel中列的字段名和顺序
            Dictionary<string, int> OperatingRatioHeadDc = new Dictionary<string, int>();
            for (int i = StartColIndex; i < EndColIndex; i++)
            {
                string value = xSheet.Range[TitleIndex, i].Value.ToString();

                OperatingRatioHeadDc.Add(value.ToString(), i);
            }
            foreach (var item in OperatingRatioHeadDc)
            {
                string sql = "";
                if (!item.Key.Contains("-"))
                {
                    sql = string.Format("AREA='{0}'", item.Key);
                }
                else
                {
                    string[] str = item.Key.Split('-');
                    if (item.Key.Contains("ALL"))
                    {
                        sql = string.Format(" EQUIPMENTTYPE='{0}'", str[0]);
                    }
                    else
                    {
                        sql = string.Format("AREA='{0}' AND EQUIPMENTTYPE='{1}'", str[0], str[1]);
                    }
                }
                DataTable dt = null;
                if (status == "BM")
                {
                    EqOperatingRatioDt.DefaultView.RowFilter = "EQSTATUS in('CBUTILITY','CBPROCESS','BM START','BM WAIT','CBM1','CBM2','CBM3')";
                    dt = EqOperatingRatioDt.DefaultView.ToTable();
                }
                else if (status == "PM")
                {
                    EqOperatingRatioDt.DefaultView.RowFilter = "EQSTATUS in('CPM','PM START')";
                    dt = EqOperatingRatioDt.DefaultView.ToTable();
                }
                else if (status == CHECKDOWN)
                {
                    EqOperatingRatioDt.DefaultView.RowFilter = "EQSTATUS in('CHECK DOWN','CCHECK')";
                    dt = EqOperatingRatioDt.DefaultView.ToTable();
                }
                else if (status == IDLE)
                {
                    EqOperatingRatioDt.DefaultView.RowFilter = "EQSTATUS  in('CIDLE', 'IDLE')";
                    dt = EqOperatingRatioDt.DefaultView.ToTable();
                }
                else if (status == PROCESSTEST)
                {
                    EqOperatingRatioDt.DefaultView.RowFilter = "EQSTATUS in('CPROCESS','PROCESS TEST','PROCESS DOWN')";
                    dt = EqOperatingRatioDt.DefaultView.ToTable();
                }
                else if (status == RANDDTEST)
                {
                    EqOperatingRatioDt.DefaultView.RowFilter = "EQSTATUS in('CR&D TEST','R&D TEST') ";
                    dt = EqOperatingRatioDt.DefaultView.ToTable();
                }
                else if (status == UTILITY)
                {
                    EqOperatingRatioDt.DefaultView.RowFilter = "EQSTATUS in('CUTILITY','UTILITY')";
                    dt = EqOperatingRatioDt.DefaultView.ToTable();
                }
                else if (status == MATERIAL)
                {
                    EqOperatingRatioDt.DefaultView.RowFilter = "EQSTATUS in('CMATERIAL','MATERIAL')";
                    dt = EqOperatingRatioDt.DefaultView.ToTable();
                }
                dt.DefaultView.RowFilter = sql;
                DataTable FilterDt = dt.DefaultView.ToTable();
                EqOperatingRatioDt.DefaultView.RowFilter = string.Empty;
                //ChangeDt.DefaultView.ToTable(true, new string[] { "NAME" });
                //获取筛选后的设备总数量
                EqDt.DefaultView.RowFilter = sql + "AND LINE= '311'"; double count311 = EqDt.DefaultView.ToTable().Rows.Count; EqDt.DefaultView.RowFilter = string.Empty;
                EqDt.DefaultView.RowFilter = sql + "AND LINE= '321'"; double count321 = EqDt.DefaultView.ToTable().Rows.Count; EqDt.DefaultView.RowFilter = string.Empty;
                EqDt.DefaultView.RowFilter = sql + "AND LINE= '511'"; double count511 = EqDt.DefaultView.ToTable().Rows.Count; EqDt.DefaultView.RowFilter = string.Empty;
                EqDt.DefaultView.RowFilter = sql + "AND LINE= '521'"; double count521 = EqDt.DefaultView.ToTable().Rows.Count; EqDt.DefaultView.RowFilter = string.Empty;
                EqDt.DefaultView.RowFilter = sql + "AND LINE= '611'"; double count611 = EqDt.DefaultView.ToTable().Rows.Count; EqDt.DefaultView.RowFilter = string.Empty;
                EqDt.DefaultView.RowFilter = sql + "AND LINE= '631'"; double count631 = EqDt.DefaultView.ToTable().Rows.Count; EqDt.DefaultView.RowFilter = string.Empty;
                EqDt.DefaultView.RowFilter = sql + "AND LINE= '632'"; double count632 = EqDt.DefaultView.ToTable().Rows.Count; EqDt.DefaultView.RowFilter = string.Empty;
                EqDt.DefaultView.RowFilter = sql + "AND LINE= '651'"; double count651 = EqDt.DefaultView.ToTable().Rows.Count; EqDt.DefaultView.RowFilter = string.Empty;

                object WS311 = FilterDt.Compute("SUM(TIME)", string.Format("LINE='{0}'", "311"));
                object WS321 = FilterDt.Compute("SUM(TIME)", string.Format("LINE='{0}'", "321"));
                object WS511 = FilterDt.Compute("SUM(TIME)", string.Format("LINE='{0}'", "511"));
                object WS521 = FilterDt.Compute("SUM(TIME)", string.Format("LINE='{0}'", "521"));
                object WS611 = FilterDt.Compute("SUM(TIME)", string.Format("LINE='{0}'", "611"));
                object WS631 = FilterDt.Compute("SUM(TIME)", string.Format("LINE='{0}'", "631"));
                object WS632 = FilterDt.Compute("SUM(TIME)", string.Format("LINE='{0}'", "632"));
                object WS651 = FilterDt.Compute("SUM(TIME)", string.Format("LINE='{0}'", "651"));

                double WS311time = WS311 == System.DBNull.Value ? 0 : (double)WS311;
                double WS321time = WS321 == System.DBNull.Value ? 0 : (double)WS321;
                double WS511time = WS511 == System.DBNull.Value ? 0 : (double)WS511;
                double WS521time = WS521 == System.DBNull.Value ? 0 : (double)WS521;
                double WS611time = WS611 == System.DBNull.Value ? 0 : (double)WS611;
                double WS631time = WS631 == System.DBNull.Value ? 0 : (double)WS631;
                double WS632time = WS632 == System.DBNull.Value ? 0 : (double)WS632;
                double WS651time = WS651 == System.DBNull.Value ? 0 : (double)WS651;

                double ra311 = count311 == 0 ? 0 : WS311time / (count311 * 24) * 24;
                double ra321 = count321 == 0 ? 0 : WS321time / (count321 * 24) * 24;
                double ra511 = count511 == 0 ? 0 : WS511time / (count511 * 24) * 24;
                double ra521 = count521 == 0 ? 0 : WS521time / (count521 * 24) * 24;
                double ra611 = count611 == 0 ? 0 : WS611time / (count611 * 24) * 24;
                double ra631 = count631 == 0 ? 0 : WS631time / (count631 * 24) * 24;
                double ra632 = count632 == 0 ? 0 : WS632time / (count632 * 24) * 24;
                double ra651 = count651 == 0 ? 0 : WS651time / (count651 * 24) * 24;

                xSheet.Range[(StartRowIndex), item.Value].NumberValue = ra311;
                xSheet.Range[(StartRowIndex + 1), item.Value].NumberValue = ra321;
                xSheet.Range[(StartRowIndex + 2), item.Value].NumberValue = ra511;
                xSheet.Range[(StartRowIndex + 3), item.Value].NumberValue = ra521;
                xSheet.Range[(StartRowIndex + 4), item.Value].NumberValue = ra611;
                xSheet.Range[(StartRowIndex + 5), item.Value].NumberValue = ra631;
                xSheet.Range[(StartRowIndex + 6), item.Value].NumberValue = ra632;
                xSheet.Range[(StartRowIndex + 7), item.Value].NumberValue = ra651;
                xSheet.Range[(StartRowIndex), item.Value].NumberFormat = "0.00";
                xSheet.Range[(StartRowIndex + 1), item.Value].NumberFormat = "0.00";
                xSheet.Range[(StartRowIndex + 2), item.Value].NumberFormat = "0.00";
                xSheet.Range[(StartRowIndex + 3), item.Value].NumberFormat = "0.00";
                xSheet.Range[(StartRowIndex + 4), item.Value].NumberFormat = "0.00";
                xSheet.Range[(StartRowIndex + 5), item.Value].NumberFormat = "0.00";
                xSheet.Range[(StartRowIndex + 6), item.Value].NumberFormat = "0.00";
                xSheet.Range[(StartRowIndex + 7), item.Value].NumberFormat = "0.00";

            }
        }
        private void CalcgWorkShop(int TitleIndex, int StartRowIndex, int StartColIndex, int EndColIndex)
        {    //获取excel中列的字段名和顺序
            DataTable WriteExcelDt = new DataTable();
            for (int i = StartColIndex; i < EndColIndex; i++)
            {
                string value = xSheet.Range[TitleIndex, i].Value.ToString();
                WriteExcelDt.Columns.Add(value.ToString(), Type.GetType("System.Int32"));
            }
            //获取excel中列的字段名和顺序
            Dictionary<string, int> OperatingRatioHeadDc = new Dictionary<string, int>();
            for (int i = StartColIndex; i < EndColIndex; i++)
            {
                string value = xSheet.Range[TitleIndex, i].Value.ToString();
                OperatingRatioHeadDc.Add(value.ToString(), i);
            }
            foreach (var item in OperatingRatioHeadDc)
            {
                string sql = "";
                if (item.Key.Equals("BM"))
                {
                    sql = "EQSTATUS in('CBUTILITY','CBPROCESS','CBM1','CBM2','CBM3','BM START','BM WAIT')";
                }
                if (item.Key.Equals("PM"))
                {
                    sql = "EQSTATUS in('CPM','PM START')";
                }
                if (item.Key.Equals("CHECK"))
                {
                    sql = "EQSTATUS in('CHECK DOWN','CCHECK')";
                }
                if (item.Key.Equals("IDLE"))
                {
                    sql = "EQSTATUS  in('CIDLE','IDLE')";
                }
                if (item.Key.Equals("PROCESS"))
                {
                    sql = "EQSTATUS in('CPROCESS','PROCESS TEST','PROCESS DOWN')";
                }
                if (item.Key.Equals("RD"))
                {
                    sql = "EQSTATUS in('CR&D TEST','R&D TEST') ";
                }
                if (item.Key.Equals("UTILITY"))
                {
                    sql = "EQSTATUS in('CUTILITY','UTILITY')";
                }
                if (item.Key.Equals("MATERIAL"))
                {
                    sql = "EQSTATUS in('CMATERIAL','MATERIAL')";
                }
                if (item.Key.ToUpper().Equals("MODIFY"))
                {
                    sql = "EQSTATUS in('CMODIFY','EQUIPMENT MODIFY')";
                }
                EqOperatingRatioDt.DefaultView.RowFilter = sql;
                DataTable FilterDt = EqOperatingRatioDt.DefaultView.ToTable();
                EqOperatingRatioDt.DefaultView.RowFilter = string.Empty;
                //获取数据总条数
                EqOperatingRatioDt.DefaultView.RowFilter = "LINE ='311'";
                DataTable dataWS311 = EqOperatingRatioDt.DefaultView.ToTable(true, new string[] { "NAME" });
                EqOperatingRatioDt.DefaultView.RowFilter = "LINE ='321'";
                DataTable dataWS321 = EqOperatingRatioDt.DefaultView.ToTable(true, new string[] { "NAME" });
                EqOperatingRatioDt.DefaultView.RowFilter = "LINE ='511'";
                DataTable dataWS511 = EqOperatingRatioDt.DefaultView.ToTable(true, new string[] { "NAME" });
                EqOperatingRatioDt.DefaultView.RowFilter = "LINE ='521'";
                DataTable dataWS521 = EqOperatingRatioDt.DefaultView.ToTable(true, new string[] { "NAME" });
                EqOperatingRatioDt.DefaultView.RowFilter = "LINE ='611'";
                DataTable dataWS611 = EqOperatingRatioDt.DefaultView.ToTable(true, new string[] { "NAME" });
                EqOperatingRatioDt.DefaultView.RowFilter = "LINE ='631'";
                DataTable dataWS631 = EqOperatingRatioDt.DefaultView.ToTable(true, new string[] { "NAME" });
                EqOperatingRatioDt.DefaultView.RowFilter = "LINE ='632'";
                DataTable dataWS632 = EqOperatingRatioDt.DefaultView.ToTable(true, new string[] { "NAME" });
                EqOperatingRatioDt.DefaultView.RowFilter = "LINE ='651'";
                DataTable dataWS651 = EqOperatingRatioDt.DefaultView.ToTable(true, new string[] { "NAME" });
                double totaltimeWS311 = dataWS311.Rows.Count * 24;
                double totaltimeWS321 = dataWS321.Rows.Count * 24;
                double totaltimeWS511 = dataWS511.Rows.Count * 24;
                double totaltimeWS521 = dataWS521.Rows.Count * 24;
                double totaltimeWS611 = dataWS611.Rows.Count * 24;
                double totaltimeWS631 = dataWS631.Rows.Count * 24;
                double totaltimeWS632 = dataWS632.Rows.Count * 24;
                double totaltimeWS651 = dataWS651.Rows.Count * 24;
                EqOperatingRatioDt.DefaultView.RowFilter = string.Empty;
                FilterDt.DefaultView.RowFilter = string.Format("LINE='{0}'", "311");
                object WS311 = FilterDt.DefaultView.ToTable().Compute("SUM(TIME)", "");
                FilterDt.DefaultView.RowFilter = string.Format("LINE='{0}'", "321");
                object WS321 = FilterDt.DefaultView.ToTable().Compute("SUM(TIME)", "");
                FilterDt.DefaultView.RowFilter = string.Format("LINE='{0}'", "511");
                object WS511 = FilterDt.DefaultView.ToTable().Compute("SUM(TIME)", "");
                FilterDt.DefaultView.RowFilter = string.Format("LINE='{0}'", "521");
                object WS521 = FilterDt.DefaultView.ToTable().Compute("SUM(TIME)", "");
                FilterDt.DefaultView.RowFilter = string.Format("LINE='{0}'", "611");
                object WS611 = FilterDt.DefaultView.ToTable().Compute("SUM(TIME)", "");
                FilterDt.DefaultView.RowFilter = string.Format("LINE='{0}'", "631");
                object WS631 = FilterDt.DefaultView.ToTable().Compute("SUM(TIME)", "");
                FilterDt.DefaultView.RowFilter = string.Format("LINE='{0}'", "632");
                object WS632 = FilterDt.DefaultView.ToTable().Compute("SUM(TIME)", "");
                FilterDt.DefaultView.RowFilter = string.Format("LINE='{0}'", "651");
                object WS651 = FilterDt.DefaultView.ToTable().Compute("SUM(TIME)", "");
                double WS311time = WS311 == System.DBNull.Value ? 0 : (double)WS311;
                double WS321time = WS321 == System.DBNull.Value ? 0 : (double)WS321;
                double WS511time = WS511 == System.DBNull.Value ? 0 : (double)WS511;
                double WS521time = WS521 == System.DBNull.Value ? 0 : (double)WS521;
                double WS611time = WS611 == System.DBNull.Value ? 0 : (double)WS611;
                double WS631time = WS631 == System.DBNull.Value ? 0 : (double)WS631;
                double WS632time = WS632 == System.DBNull.Value ? 0 : (double)WS632;
                double WS651time = WS651 == System.DBNull.Value ? 0 : (double)WS651;
                double opWS311 = totaltimeWS311 == 0 ? 0 : WS311time / totaltimeWS311;
                double opWS321 = totaltimeWS321 == 0 ? 0 : WS321time / totaltimeWS321;
                double opWS511 = totaltimeWS511 == 0 ? 0 : WS511time / totaltimeWS511;
                double opWS521 = totaltimeWS521 == 0 ? 0 : WS521time / totaltimeWS521;
                double opWS611 = totaltimeWS611 == 0 ? 0 : WS611time / totaltimeWS611;
                double opWS631 = totaltimeWS631 == 0 ? 0 : WS631time / totaltimeWS631;
                double opWS632 = totaltimeWS632 == 0 ? 0 : WS632time / totaltimeWS632;
                double opWS651 = totaltimeWS651 == 0 ? 0 : WS651time / totaltimeWS651;

                xSheet.Range[(StartRowIndex), item.Value].NumberValue = opWS311;
                xSheet.Range[(StartRowIndex + 1), item.Value].NumberValue = opWS321;
                xSheet.Range[(StartRowIndex + 2), item.Value].NumberValue = opWS511;
                xSheet.Range[(StartRowIndex + 3), item.Value].NumberValue = opWS521;
                xSheet.Range[(StartRowIndex + 4), item.Value].NumberValue = opWS611;
                xSheet.Range[(StartRowIndex + 5), item.Value].NumberValue = opWS631;
                xSheet.Range[(StartRowIndex + 6), item.Value].NumberValue = opWS632;
                xSheet.Range[(StartRowIndex + 7), item.Value].NumberValue = opWS651;
                xSheet.Range[(StartRowIndex), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 1), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 2), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 3), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 4), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 5), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 6), item.Value].NumberFormat = "0.00%";
                xSheet.Range[(StartRowIndex + 7), item.Value].NumberFormat = "0.00%";

            }

        }
        private void BmReport(int TitleIndex, int StartRowIndex, int StartColIndex)
        {
            DateTime time = DateTime.MinValue;
            for (int i = 0; i < BmComList.Columns.Count; i++)
            {
                xSheet2.Range[TitleIndex, i + StartColIndex].Value = BmComList.Columns[i].ColumnName;
            }
            for (int i = 0; i < BmComList.Rows.Count; i++)
            {
                for (int j = 0; j < BmComList.Columns.Count; j++)
                {
                    if (BmComList.Columns[j].ColumnName.ToUpper().Contains("TIME"))
                    {
                        if (!string.IsNullOrEmpty(BmComList.Rows[i][j].ToString()))
                        {
                            xSheet2.Range[i + StartRowIndex, j + StartColIndex].Value = OutTime(BmComList.Rows[i][j].ToString(), time).ToString("yyyy-MM-dd HH:mm:ss");
                            xSheet2.Range[i + StartRowIndex, j + StartColIndex].NumberFormat = "yyyy-MM-dd HH:mm:ss";
                        }
                        else
                        {
                            xSheet2.Range[i + StartRowIndex, j + StartColIndex].Value = BmComList.Rows[i][j].ToString();
                        }
                    }
                    else
                    {
                        xSheet2.Range[i + StartRowIndex, j + StartColIndex].Value = BmComList.Rows[i][j].ToString();
                    }
                }
            }
            xSheet2.Range.BorderInside(LineStyleType.Thin, Color.Black);
            xSheet2.Range.BorderAround(LineStyleType.Thin, Color.Black);
            xSheet2.AllocatedRange.AutoFitColumns();
            xSheet2.AllocatedRange.Style.HorizontalAlignment = HorizontalAlignType.Center;
            xSheet2.AllocatedRange.Style.WrapText = false;

        }
#region SP REPORT
        public void SpReport() {
#region SP report
            //SP REPORT (SHEET)
            DataTable planDtByLine = GetALM_RETGroupByWAFER_QTY("'WAFER_QTY'");
            DataTable planDtByShift = GetALM_RETGroupByShift();
            //台面TABLE
            DataTable dctabelDt = GetALM_RETGroupBy("'TABLE_A','TABLE_B','TABLE_C','TABLE_D'");
            //进出片 CV
            DataTable dccvDt = GetALM_RETGroupBy("'CV'");
            //aoi
            DataTable dcaoiDt = GetALM_RETGroupBy("'AOI'");
            //门检DOOR
            DataTable dcdoorDt = GetALM_RETGroupBy("'DOOR'");

            DataTable dtByTime = GetALM_RETGroupByProcessTime();
            //mark
            //   DataTable dcmarkDt = GetALM_RETGroupBy("'MARK_A','MARK_B','MARK_C','MARK_D'");
            //BREAK
            //   DataTable dcbreakDt = GetALM_RETGroupBy("'BREAK_A','BREAK_B','BREAK_C','BREAK_D'");
            //5号楼产量 
            SpReport(planDtByLine, 4, 8);
            //5号楼产量
            SpReport(planDtByShift, 4, 18, 1);
            //5号楼台面报警数量		
            SpReport(dctabelDt, 22, 8);
            //5号楼进出片报警数量		
            SpReport(dccvDt, 22, 18);
            //5号楼AOI报警数量		
            SpReport(dcaoiDt, 39, 8);
            //5号楼门检报警数量		
            SpReport(dcdoorDt, 39, 18);
            //■ SP报警总数汇总			
            SetExcelData();
            //sp机台嫁动时间（min）
            DataTable SpEqRatioTimeDt = SpEqTime(dtByTime, 96, 1);
            SpEqRatioTime(SpEqRatioTimeDt, 121, 1);
#endregion

        }
        public void SpReport(DataTable dt, int StartRowIndex, int StartColIndex, int rowCount = 6)
        {
            if (dt.Rows.Count == 0) return;
            if (rowCount == 6)
            {
                DataTable table = new DataTable();
                table.Columns.Add("DATA", typeof(String));
                table.Columns.Add("D", typeof(Int32));
                table.Columns.Add("N", typeof(Int32));
                table.Rows.Add(new object[] { "511一线", 0, 0 });
                table.Rows.Add(new object[] { "511二线", 0, 0 });
                table.Rows.Add(new object[] { "521一线", 0, 0 });
                table.Rows.Add(new object[] { "521二线", 0, 0 });
                table.Rows.Add(new object[] { "521三线", 0, 0 });
                table.Rows.Add(new object[] { "521四线", 0, 0 });

                foreach (DataRow tbrow in table.Rows)
                {
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        if (string.Equals(tbrow["DATA"], dtrow["DATA"]))
                        {
                            tbrow["D"] = dtrow["D"];
                            tbrow["N"] = dtrow["N"];
                        }
                    }

                }
                //白天
                xSheet3.Range[(StartRowIndex), StartColIndex].NumberValue = Convert.ToDouble(table.Rows[0]["D"]);
                xSheet3.Range[(StartRowIndex + 1), StartColIndex].NumberValue = Convert.ToDouble(table.Rows[1]["D"]);
                xSheet3.Range[(StartRowIndex + 2), StartColIndex].NumberValue = Convert.ToDouble(table.Rows[2]["D"]);
                xSheet3.Range[(StartRowIndex + 3), StartColIndex].NumberValue = Convert.ToDouble(table.Rows[3]["D"]);
                xSheet3.Range[(StartRowIndex + 4), StartColIndex].NumberValue = Convert.ToDouble(table.Rows[4]["D"]);
                xSheet3.Range[(StartRowIndex + 5), StartColIndex].NumberValue = Convert.ToDouble(table.Rows[5]["D"]);
                //晚上
                xSheet3.Range[(StartRowIndex), StartColIndex + 1].NumberValue = Convert.ToDouble(table.Rows[0]["N"]);
                xSheet3.Range[(StartRowIndex + 1), StartColIndex + 1].NumberValue = Convert.ToDouble(table.Rows[1]["N"]);
                xSheet3.Range[(StartRowIndex + 2), StartColIndex + 1].NumberValue = Convert.ToDouble(table.Rows[2]["N"]);
                xSheet3.Range[(StartRowIndex + 3), StartColIndex + 1].NumberValue = Convert.ToDouble(table.Rows[3]["N"]);
                xSheet3.Range[(StartRowIndex + 4), StartColIndex + 1].NumberValue = Convert.ToDouble(table.Rows[4]["N"]);
                xSheet3.Range[(StartRowIndex + 5), StartColIndex + 1].NumberValue = Convert.ToDouble(table.Rows[5]["N"]);
                //xSheet3.Range[(StartRowIndex), item.Value].NumberFormat = "0";
            }
            else if (rowCount == 1)
            {
                DataTable table = new DataTable();
                table.Columns.Add("DATA", typeof(String));
                table.Columns.Add("D", typeof(Int32));
                table.Columns.Add("N", typeof(Int32));
                table.Rows.Add(new object[] { "产量", 0, 0 });

                foreach (DataRow tbrow in table.Rows)
                {
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        if (string.Equals(tbrow["DATA"], dtrow["DATA"]))
                        {
                            tbrow["D"] = dtrow["D"];
                            tbrow["N"] = dtrow["N"];
                        }
                    }

                }
                //白天
                xSheet3.Range[(StartRowIndex), StartColIndex].NumberValue = Convert.ToDouble(table.Rows[0]["D"]);
                //晚上
                xSheet3.Range[(StartRowIndex), StartColIndex + 1].NumberValue = Convert.ToDouble(table.Rows[0]["N"]);
            }

        }
        public void SetExcelData()
        {
            DataTable dt_511_L1 = GetSpAlmData("511", "一线");
            DataTable dt_511_L2 = GetSpAlmData("511", "二线");
            DataTable dt_521_L1 = GetSpAlmData("521", "一线");
            DataTable dt_521_L2 = GetSpAlmData("521", "二线");
            DataTable dt_521_L3 = GetSpAlmData("521", "三线");
            DataTable dt_521_L4 = GetSpAlmData("521", "四线");
            TotalNumberOfAlarms(dt_521_L1, 54, 1);
            TotalNumberOfAlarms(dt_521_L2, 58, 1);
            TotalNumberOfAlarms(dt_521_L3, 62, 1);
            TotalNumberOfAlarms(dt_521_L4, 66, 1);
            TotalNumberOfAlarms(dt_511_L1, 70, 1);
            TotalNumberOfAlarms(dt_511_L2, 74, 1);
        }
        //sp报警总数汇总
        public void TotalNumberOfAlarms(DataTable dt, int StartRowIndex, int StartColIndex)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    xSheet3.Range[(StartRowIndex) + i, StartColIndex + j].Value = dt.Rows[i][j].ToString();
                }
            }
        }
        public DataTable SpEqTime(DataTable dt, int StartRowIndex, int StartColIndex)
        {
            if (dt.Rows.Count == 0) return null;

            DataTable table = new DataTable();
            table.Columns.Add("TIMETYPE", typeof(String));
            table.Columns.Add("521-1-D", typeof(Int32));
            table.Columns.Add("521-1-N", typeof(Int32));
            table.Columns.Add("521-2-D", typeof(Int32));
            table.Columns.Add("521-2-N", typeof(Int32));
            table.Columns.Add("521-3-D", typeof(Int32));
            table.Columns.Add("521-3-N", typeof(Int32));
            table.Columns.Add("521-4-D", typeof(Int32));
            table.Columns.Add("521-4-N", typeof(Int32));
            table.Columns.Add("511-1-D", typeof(Int32));
            table.Columns.Add("511-1-N", typeof(Int32));
            table.Columns.Add("511-2-D", typeof(Int32));
            table.Columns.Add("511-2-N", typeof(Int32));
            table.Rows.Add(new object[] { "UP_TIME", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            table.Rows.Add(new object[] { "RUN_TIME", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            table.Rows.Add(new object[] { "PROCESS_TIME", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            table.Rows.Add(new object[] { "IDLE_TIME", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            table.Rows.Add(new object[] { "DOWN_TIME", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataRow tarow in table.Rows)
                {
                    if (tarow["TIMETYPE"].ToString() == row["PARAMETER_NAME"].ToString())
                    {
                        string workshop = row["WORKSHOP"].ToString();
                        string line = row["LINE"].ToString().Substring(row["LINE"].ToString().Length-1,1);
                        if (line.Contains("一")) { line = "1"; }
                        else if (line.Contains("二")) { line = "2"; }
                        else if (line.Contains("三")) { line = "3"; }
                        else if (line.Contains("四")) { line = "4"; }
                        string shift = row["SHIFT"].ToString();
                        string parameter_name = row["PARAMETER_NAME"].ToString();
                        string value = row["VALUE"].ToString();
                        string columnName = workshop + "-" + line + "-" + shift;
                        double valuemin = Convert.ToDouble(value) / 60;
                        tarow[columnName] = Convert.ToInt32(valuemin);
                    }
                }
            }
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    xSheet3.Range[(StartRowIndex) + i, StartColIndex + j].Value = table.Rows[i][j].ToString();
                }
            }
            //UP_TIME 对应行删除
            table.Rows.RemoveAt(0);
            return table;

        }
        public void SpEqRatioTime(DataTable dt, int StartRowIndex, int StartColIndex)
        {
            if (dt == null || dt.Rows.Count == 0) return;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName.ToString() == "TIMETYPE")
                    {
                        xSheet3.Range[(StartRowIndex) + i, StartColIndex + j].Value = dt.Rows[i][j].ToString();
                    }
                    else
                    {
                        double value = Convert.ToDouble(dt.Rows[i][j]) / (timeSpan.TotalMinutes / 2);
                        xSheet3.Range[(StartRowIndex) + i, StartColIndex + j].NumberValue = value;
                        xSheet3.Range[(StartRowIndex) + i, StartColIndex + j].NumberFormat = "0.00%";
                    }
                }
            }

        }
#endregion


#endregion
        class StatusStartAndEnd
        {
            public string StatusStart { get; set; }
            public string StatusEnd { get; set; }
            public string StatusName { get; set; }

            public List<StatusStartAndEnd> StatusStartAndEndAdd { get; set; }
        }
    }
}
