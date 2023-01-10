using NPOI.SS.Formula.Functions;
using NPOI.SS.Formula.PTG;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TAP.Data.DataBase.Communicators;
using TAP.Models.SystemBasic;
using TAP.WinService;
using static NPOI.HSSF.Util.HSSFColor;

namespace ISIA.SERVICE
{
    class SvcRPBODailySummary : TAP.WinService.ServiceBase
    {        
        string date = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
        string date2 = DateTime.Now.ToString("yyyyMMdd");
        string startTime = DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + "073000";
        string endTime = DateTime.Now.ToString("yyyyMMdd") + "073000";

        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            //SaveEqpStatusSummaryData();
            //TAPIFTB_DFROBO 
            Console.WriteLine("START DF");
            SaveWaferQtyByHour("HH_MES_TOTAL_PRODUCT_NUM", "TAPIFTB_DFROBO");
            SaveWaferQtyByHour2("HH_MES_TOTAL_PRODUCT_NUM", "TAPIFTB_DFROBO");
            //TAPIFTB_OXROBO
            Console.WriteLine("START OX");
            SaveWaferQtyByHour("HH_MES_TOTAL_PRODUCT_NUM", "TAPIFTB_OXROBO");
            SaveWaferQtyByHour2("HH_MES_TOTAL_PRODUCT_NUM", "TAPIFTB_OXROBO");
            //TAPIFTB_PEROBO
            Console.WriteLine("START PE");
            //SaveWaferQtyByHour("OutputDay", "TAPIFTB_PEROBO");
            //SaveWaferQtyByHour("OutputNight", "TAPIFTB_PEROBO");

            SaveWaferQtyByHourPERP("TAPIFTB_PEROBO");
            SaveWaferQtyByHourPERP2("TAPIFTB_PEROBO");

            //TAPIFTB_PSROBO
            Console.WriteLine("START PS");
            SaveWaferQtyByHour("NOW_OUTPUT", "TAPIFTB_PSROBO");
            SaveWaferQtyByHour2("NOW_OUTPUT", "TAPIFTB_PSROBO");
            //TAPIFTB_TXROBO
            Console.WriteLine("START TX");
            SaveWaferQtyByHour("NOW_OUTPUT", "TAPIFTB_TXROBO");
            SaveWaferQtyByHour2("NOW_OUTPUT", "TAPIFTB_TXROBO");
            SaveWaferQtyByHour("VS_CURRENT", "TAPIFTB_TXROBO");
            SaveWaferQtyByHour2("VS_CURRENT", "TAPIFTB_TXROBO");

            //TAPIFTB_RPROBO
            Console.WriteLine("START RP");
            //SaveWaferQtyByHour("OutputDay", "TAPIFTB_RPROBO");
            //SaveWaferQtyByHour("OutputNight", "TAPIFTB_RPROBO");
            SaveWaferQtyByHourPERP("TAPIFTB_RPROBO");
            SaveWaferQtyByHourPERP2("TAPIFTB_RPROBO");

            DeleteRobo("TAPIFTB_DFROBO");
            DeleteRobo("TAPIFTB_OXROBO");
            DeleteRobo("TAPIFTB_PEROBO");
            DeleteRobo("TAPIFTB_PSROBO");
            DeleteRobo("TAPIFTB_TXROBO");
            DeleteRobo("TAPIFTB_RPROBO");
        }

        private void SaveEqpStatusSummaryData()
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();
                string date = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
#if MSSQL
                tmpsb.Append("SELECT  WORKSHOP, LINE, REPORT_DATE, SHIFT,PROCESS_TYPE, PARAMETER_NAME, MAX((CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_SPEQP ");
                tmpsb.AppendFormat("WHERE  REPORT_DATE = '{0}' ", date);
                tmpsb.Append("GROUP BY WORKSHOP, LINE, REPORT_DATE, SHIFT,PROCESS_TYPE, PARAMETER_NAME ");
#endif
#if ORACLE               
                tmpsb.Append("SELECT AREA, LINE, NAME, REGION, FACILITY, SHIFT, OLDEQUIPMENTSTATUS, TRIM(TO_CHAR(SUM(RUNNINGTIME),999999)) as RUNNINGTIME FROM TAPEQP_STATUSHISTORY ");
                tmpsb.AppendFormat(" WHERE REPORT_DATE = '{0}' ", date);
                tmpsb.Append(" GROUP BY AREA, LINE, NAME, REGION, FACILITY, SHIFT, OLDEQUIPMENTSTATUS ");
#endif
                DataTable tmpdt = db.Select(tmpsb.ToString()).Tables[0];
                
                List<string> SqlList = new List<string>();

                foreach (DataRow dr in tmpdt.Rows)
                {
                    StringBuilder tmpInsert = new StringBuilder();
                    tmpInsert.Append("INSERT INTO  TAPEQDATASUMM (WORKSHOP, PROCESS, \"DATE\", ");
                    tmpInsert.Append("SHIFT, CATEGORY1, CATEGORY2, ");
                    tmpInsert.Append("CATEGORY3, CATEGORY4, CATEGORY5, ");
                    tmpInsert.Append("PARAMETER_NAME, PARAMETER_VALUE, INSERT_TIME) VALUES(");
                    tmpInsert.AppendFormat("'{0}',", dr["LINE"].ToString());
                    tmpInsert.AppendFormat("'{0}',", dr["AREA"].ToString());
                    tmpInsert.AppendFormat("'{0}',", date);
                    tmpInsert.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpInsert.AppendFormat("'{0}',", dr["NAME"].ToString());
                    tmpInsert.AppendFormat("'{0}',", "ALL");
                    tmpInsert.AppendFormat("'{0}',", "ALL");
                    tmpInsert.AppendFormat("'{0}',", "ALL");
                    tmpInsert.AppendFormat("'{0}',", "ALL");
                    tmpInsert.AppendFormat("'{0}',", dr["OLDEQUIPMENTSTATUS"].ToString()+"_TIME");
                    tmpInsert.AppendFormat("'{0}',", dr["RUNNINGTIME"].ToString());
                    tmpInsert.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    SqlList.Add(tmpInsert.ToString());
                }

                db.Save(SqlList);
                
            }            
            catch (Exception ex)
            {
                string error = ex.ToString();
                throw;
            }
        }

        private void SaveWaferQtyByHour(string ParamaterName, string TableName)
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();
                string date = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");

                tmpsb.AppendFormat("SELECT  SUBSTR(INSERT_TIME, 0, 10) AS WORKHOUR, WORKSHOP, EQUIPMENT, REPORT_DATE, SHIFT, PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, REPORT_DATE, SHIFT, PARAMETER_NAME, SUBSTR(INSERT_TIME, 0, 10)");
                Console.WriteLine(tmpsb.ToString());

                DataTable tmpdt = db.Select(tmpsb.ToString()).Tables[0];
                Console.WriteLine("Select Hour");
                List<string> SqlList = new List<string>();
                foreach (DataRow dr in tmpdt.Rows)
                {
                    StringBuilder tmpsb1 = new StringBuilder();
                    tmpsb1.Append("INSERT INTO  TAPEQDATASUMM (WORKSHOP, PROCESS, \"DATE\", ");
                    tmpsb1.Append("SHIFT, CATEGORY1, CATEGORY2, ");
                    tmpsb1.Append("CATEGORY3, CATEGORY4, CATEGORY5, ");
                    tmpsb1.Append("PARAMETER_NAME, PARAMETER_VALUE, INSERT_TIME) VALUES(");
                    tmpsb1.AppendFormat("'{0}',", dr["WORKSHOP"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString().Substring(0,2));
                    tmpsb1.AppendFormat("'{0}',", dr["REPORT_DATE"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "WAFER_QTY" + "_" + dr["WORKHOUR"].ToString().Substring(8, 2));
                    tmpsb1.AppendFormat("'{0}',", dr["PARAMETER_VALUE"].ToString());
                    tmpsb1.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    SqlList.Add(tmpsb1.ToString());
                }
                Console.WriteLine("save" + SqlList.Count().ToString());
                db.Save(SqlList);
                Console.WriteLine("save complete");
                //total
                StringBuilder tmpsb2 = new StringBuilder();

                tmpsb2.AppendFormat("SELECT  WORKSHOP, EQUIPMENT, REPORT_DATE, SHIFT, PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb2.Append("WHERE 1=1 ");
                tmpsb2.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb2.AppendFormat("AND PARAMETER_NAME = '{0}' ", ParamaterName);
                tmpsb2.Append("GROUP BY WORKSHOP, EQUIPMENT, REPORT_DATE, SHIFT, PARAMETER_NAME");
                Console.WriteLine(tmpsb2.ToString());
                DataTable tmpdt2 = db.Select(tmpsb2.ToString()).Tables[0];
                
                Console.WriteLine("Select shift");
                List<string> SqlList2 = new List<string>();
                foreach (DataRow dr in tmpdt2.Rows)
                {
                    StringBuilder tmpsb1 = new StringBuilder();
                    tmpsb1.Append("INSERT INTO  TAPEQDATASUMM (WORKSHOP, PROCESS, \"DATE\", ");
                    tmpsb1.Append("SHIFT, CATEGORY1, CATEGORY2, ");
                    tmpsb1.Append("CATEGORY3, CATEGORY4, CATEGORY5, ");
                    tmpsb1.Append("PARAMETER_NAME, PARAMETER_VALUE, INSERT_TIME) VALUES(");
                    tmpsb1.AppendFormat("'{0}',", dr["WORKSHOP"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString().Substring(0, 2));
                    tmpsb1.AppendFormat("'{0}',", dr["REPORT_DATE"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "WAFER_QTY");
                    tmpsb1.AppendFormat("'{0}',", dr["PARAMETER_VALUE"].ToString());
                    tmpsb1.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    SqlList2.Add(tmpsb1.ToString());
                }
                Console.WriteLine("save2" + SqlList2.Count().ToString());
                db.Save(SqlList2);
                Console.WriteLine("save2 complete");
            }
            catch (Exception ex)
            {
                this.SaveLog(ServiceBase.ERROR_LOG, "SvcRPBODailySummary - SaveWaferQtyByHour", string.Format("Error:{2}", ex.ToString()));
                throw;
            }
        }

        private void SaveWaferQtyByHour2(string ParamaterName, string TableName)
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();

                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_0730' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}073000' AND INSERT_TIME < '{1}083000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_0830' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}083000' AND INSERT_TIME < '{1}093000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_0930' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}093000' AND INSERT_TIME < '{1}103000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1030' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}103000' AND INSERT_TIME < '{1}113000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1130' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}113000' AND INSERT_TIME < '{1}123000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1230' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}123000' AND INSERT_TIME < '{1}133000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1330' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}133000' AND INSERT_TIME < '{1}143000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1430' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}143000' AND INSERT_TIME < '{1}153000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1530' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}153000' AND INSERT_TIME < '{1}163000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1630' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}163000' AND INSERT_TIME < '{1}173000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1730' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}173000' AND INSERT_TIME < '{1}183000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1830' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}183000' AND INSERT_TIME < '{1}193000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_1930' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}193000' AND INSERT_TIME < '{1}203000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_2030' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}203000' AND INSERT_TIME < '{1}213000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_2130' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}213000' AND INSERT_TIME < '{1}223000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_2230' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}223000' AND INSERT_TIME < '{1}233000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_2330' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}233000' AND INSERT_TIME < '{1}003000' ", date, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0030' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}003000' AND INSERT_TIME < '{1}013000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0130' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}013000' AND INSERT_TIME < '{1}023000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0230' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}023000' AND INSERT_TIME < '{1}033000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0330' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}033000' AND INSERT_TIME < '{1}043000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0430' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}043000' AND INSERT_TIME < '{1}053000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0530' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}053000' AND INSERT_TIME < '{1}063000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0630' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", ParamaterName);
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}063000' AND INSERT_TIME < '{1}073000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");

                DataTable tmpdt = db.Select(tmpsb.ToString()).Tables[0];

                List<string> SqlList = new List<string>();
                foreach (DataRow dr in tmpdt.Rows)
                {
                    StringBuilder tmpsb1 = new StringBuilder();
                    tmpsb1.Append("INSERT INTO  TAPEQDATASUMM (WORKSHOP, PROCESS, \"DATE\", ");
                    tmpsb1.Append("SHIFT, CATEGORY1, CATEGORY2, ");
                    tmpsb1.Append("CATEGORY3, CATEGORY4, CATEGORY5, ");
                    tmpsb1.Append("PARAMETER_NAME, PARAMETER_VALUE, INSERT_TIME) VALUES(");
                    tmpsb1.AppendFormat("'{0}',", dr["WORKSHOP"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString().Substring(0, 2));
                    tmpsb1.AppendFormat("'{0}',", dr["REPORT_DATE"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", dr["PARAMETER_NAME"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["PARAMETER_VALUE"].ToString());
                    tmpsb1.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    SqlList.Add(tmpsb1.ToString());
                }

                db.Save(SqlList);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SaveWaferQtyByHourPERP(string TableName)
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();
                string date = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");

                tmpsb.AppendFormat("SELECT  SUBSTR(INSERT_TIME, 0, 10) AS WORKHOUR, WORKSHOP, EQUIPMENT, REPORT_DATE, SHIFT, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) in ('OUTPUTDAY','OUTPUTNIGHT') ");
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, REPORT_DATE, SHIFT, SUBSTR(INSERT_TIME, 0, 10)");
                Console.WriteLine(tmpsb.ToString());

                DataTable tmpdt = db.Select(tmpsb.ToString()).Tables[0];
                Console.WriteLine("Select Hour");
                List<string> SqlList = new List<string>();
                foreach (DataRow dr in tmpdt.Rows)
                {
                    StringBuilder tmpsb1 = new StringBuilder();
                    tmpsb1.Append("INSERT INTO  TAPEQDATASUMM (WORKSHOP, PROCESS, \"DATE\", ");
                    tmpsb1.Append("SHIFT, CATEGORY1, CATEGORY2, ");
                    tmpsb1.Append("CATEGORY3, CATEGORY4, CATEGORY5, ");
                    tmpsb1.Append("PARAMETER_NAME, PARAMETER_VALUE, INSERT_TIME) VALUES(");
                    tmpsb1.AppendFormat("'{0}',", dr["WORKSHOP"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString().Substring(0, 2));
                    tmpsb1.AppendFormat("'{0}',", dr["REPORT_DATE"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "WAFER_QTY" + "_" + dr["WORKHOUR"].ToString().Substring(8, 2));
                    tmpsb1.AppendFormat("'{0}',", dr["PARAMETER_VALUE"].ToString());
                    tmpsb1.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    SqlList.Add(tmpsb1.ToString());
                }
                Console.WriteLine("save" + SqlList.Count().ToString());
                db.Save(SqlList);
                Console.WriteLine("save complete");
                //total
                StringBuilder tmpsb2 = new StringBuilder();

                tmpsb2.AppendFormat("SELECT  WORKSHOP, EQUIPMENT, REPORT_DATE, SHIFT, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb2.Append("WHERE 1=1 ");
                tmpsb2.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb2.AppendFormat("AND PARAMETER_NAME in ('OutputDay','OutputNight')" );
                tmpsb2.Append("GROUP BY WORKSHOP, EQUIPMENT, REPORT_DATE, SHIFT");
                Console.WriteLine(tmpsb2.ToString());
                DataTable tmpdt2 = db.Select(tmpsb2.ToString()).Tables[0];

                Console.WriteLine("Select shift");
                List<string> SqlList2 = new List<string>();
                foreach (DataRow dr in tmpdt2.Rows)
                {
                    StringBuilder tmpsb1 = new StringBuilder();
                    tmpsb1.Append("INSERT INTO  TAPEQDATASUMM (WORKSHOP, PROCESS, \"DATE\", ");
                    tmpsb1.Append("SHIFT, CATEGORY1, CATEGORY2, ");
                    tmpsb1.Append("CATEGORY3, CATEGORY4, CATEGORY5, ");
                    tmpsb1.Append("PARAMETER_NAME, PARAMETER_VALUE, INSERT_TIME) VALUES(");
                    tmpsb1.AppendFormat("'{0}',", dr["WORKSHOP"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString().Substring(0, 2));
                    tmpsb1.AppendFormat("'{0}',", dr["REPORT_DATE"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "WAFER_QTY");
                    tmpsb1.AppendFormat("'{0}',", dr["PARAMETER_VALUE"].ToString());
                    tmpsb1.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    SqlList2.Add(tmpsb1.ToString());
                }
                Console.WriteLine("save2" + SqlList2.Count().ToString());
                db.Save(SqlList2);
                Console.WriteLine("save2 complete");
            }
            catch (Exception ex)
            {
                this.SaveLog(ServiceBase.ERROR_LOG, "SvcRPBODailySummary - SaveWaferQtyByHour", string.Format("Error:{2}", ex.ToString()));
                throw;
            }
        }

        private void SaveWaferQtyByHourPERP2(string TableName)
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();

                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_0730' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTDAY");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}073000' AND INSERT_TIME < '{1}083000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_0830' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTDAY");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}083000' AND INSERT_TIME < '{1}093000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_0930' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTDAY");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}093000' AND INSERT_TIME < '{1}103000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1030' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTDAY");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}103000' AND INSERT_TIME < '{1}113000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1130' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTDAY");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}113000' AND INSERT_TIME < '{1}123000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1230' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTDAY");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}123000' AND INSERT_TIME < '{1}133000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1330' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTDAY");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}133000' AND INSERT_TIME < '{1}143000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1430' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTDAY");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}143000' AND INSERT_TIME < '{1}153000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1530' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTDAY");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}153000' AND INSERT_TIME < '{1}163000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1630' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTDAY");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}163000' AND INSERT_TIME < '{1}173000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1730' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTDAY");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}173000' AND INSERT_TIME < '{1}183000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1830' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTDAY");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}183000' AND INSERT_TIME < '{1}193000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_1930' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTNIGHT");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}193000' AND INSERT_TIME < '{1}203000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_2030' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTNIGHT");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}203000' AND INSERT_TIME < '{1}213000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_2130' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTNIGHT");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}213000' AND INSERT_TIME < '{1}223000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_2230' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTNIGHT");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}223000' AND INSERT_TIME < '{1}233000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_2330' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTNIGHT");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}233000' AND INSERT_TIME < '{1}003000' ", date, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0030' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTNIGHT");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}003000' AND INSERT_TIME < '{1}013000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0130' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTNIGHT");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}013000' AND INSERT_TIME < '{1}023000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0230' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTNIGHT");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}023000' AND INSERT_TIME < '{1}033000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0330' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTNIGHT");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}033000' AND INSERT_TIME < '{1}043000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0430' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTNIGHT");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}043000' AND INSERT_TIME < '{1}053000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0530' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTNIGHT");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}053000' AND INSERT_TIME < '{1}063000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.AppendFormat("SELECT WORKSHOP, EQUIPMENT,  REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0630' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND UPPER(PARAMETER_NAME) = '{0}' ", "OUTPUTNIGHT");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}063000' AND INSERT_TIME < '{1}073000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT,  REPORT_DATE");

                DataTable tmpdt = db.Select(tmpsb.ToString()).Tables[0];

                List<string> SqlList = new List<string>();
                foreach (DataRow dr in tmpdt.Rows)
                {
                    StringBuilder tmpsb1 = new StringBuilder();
                    tmpsb1.Append("INSERT INTO  TAPEQDATASUMM (WORKSHOP, PROCESS, \"DATE\", ");
                    tmpsb1.Append("SHIFT, CATEGORY1, CATEGORY2, ");
                    tmpsb1.Append("CATEGORY3, CATEGORY4, CATEGORY5, ");
                    tmpsb1.Append("PARAMETER_NAME, PARAMETER_VALUE, INSERT_TIME) VALUES(");
                    tmpsb1.AppendFormat("'{0}',", dr["WORKSHOP"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString().Substring(0, 2));
                    tmpsb1.AppendFormat("'{0}',", dr["REPORT_DATE"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", dr["PARAMETER_NAME"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["PARAMETER_VALUE"].ToString());
                    tmpsb1.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    SqlList.Add(tmpsb1.ToString());
                }

                db.Save(SqlList);
            }
            catch (Exception)
            {

                throw;
            }
        }


        private void SaveWaferQtyByHourShift(string ParamaterName, string TableName)
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();
                string date = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
#if ORACLE
                tmpsb.AppendFormat("SELECT  SUBSTR(INSERT_TIME, 0, 10) AS WORKHOUR, WORKSHOP, EQUIPMENT, REPORT_DATE, SHIFT, PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND PARAMETER_NAME in ({0}) ", ParamaterName);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, REPORT_DATE, SHIFT, PARAMETER_NAME, SUBSTR(INSERT_TIME, 0, 10)");
#endif
                DataTable tmpdt = db.Select(tmpsb.ToString()).Tables[0];

                List<string> SqlList = new List<string>();
                foreach (DataRow dr in tmpdt.Rows)
                {
                    StringBuilder tmpsb1 = new StringBuilder();
                    tmpsb1.Append("INSERT INTO  TAPEQDATASUMM (WORKSHOP, PROCESS, \"DATE\", ");
                    tmpsb1.Append("SHIFT, CATEGORY1, CATEGORY2, ");
                    tmpsb1.Append("CATEGORY3, CATEGORY4, CATEGORY5, ");
                    tmpsb1.Append("PARAMETER_NAME, PARAMETER_VALUE, INSERT_TIME) VALUES(");
                    tmpsb1.AppendFormat("'{0}',", dr["WORKSHOP"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString().Substring(0, 2));
                    tmpsb1.AppendFormat("'{0}',", dr["REPORT_DATE"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "WAFER_QTY" + "_" + dr["WORKHOUR"].ToString().Substring(8, 2));
                    tmpsb1.AppendFormat("'{0}',", dr["PARAMETER_VALUE"].ToString());
                    tmpsb1.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    SqlList.Add(tmpsb1.ToString());
                }

                db.Save(SqlList);

                //total
                StringBuilder tmpsb2 = new StringBuilder();
#if ORACLE
                tmpsb.Clear();
                tmpsb.AppendFormat("SELECT  WORKSHOP, EQUIPMENT, REPORT_DATE, SHIFT, PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM {0} ", TableName);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.AppendFormat("AND PARAMETER_NAME = '{0}' ", ParamaterName);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, REPORT_DATE, SHIFT, PARAMETER_NAME");
#endif
                DataTable tmpdt2 = db.Select(tmpsb.ToString()).Tables[0];

                List<string> SqlList2 = new List<string>();
                foreach (DataRow dr in tmpdt2.Rows)
                {
                    StringBuilder tmpsb1 = new StringBuilder();
                    tmpsb1.Append("INSERT INTO  TAPEQDATASUMM (WORKSHOP, PROCESS, \"DATE\", ");
                    tmpsb1.Append("SHIFT, CATEGORY1, CATEGORY2, ");
                    tmpsb1.Append("CATEGORY3, CATEGORY4, CATEGORY5, ");
                    tmpsb1.Append("PARAMETER_NAME, PARAMETER_VALUE, INSERT_TIME) VALUES(");
                    tmpsb1.AppendFormat("'{0}',", dr["WORKSHOP"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString().Substring(0, 2));
                    tmpsb1.AppendFormat("'{0}',", dr["REPORT_DATE"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "WAFER_QTY");
                    tmpsb1.AppendFormat("'{0}',", dr["PARAMETER_VALUE"].ToString());
                    tmpsb1.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    SqlList2.Add(tmpsb1.ToString());
                }

                db.Save(SqlList2);

            }
            catch (Exception)
            {

                throw;
            }
        }


        private void DeleteRobo(string tablename)
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();
                string date = DateTime.Now.AddDays(-2).ToString("yyyyMMdd");

                tmpsb.AppendFormat("DELETE FROM {0} ", tablename);
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE < '{0}' ", date);

                db.Save(new string[] { tmpsb.ToString() });
                
            }
            catch (Exception ex)
            {
                this.SaveLog(ServiceBase.ERROR_LOG, "SvcRPBODailySummary - DeleteRobo", string.Format("Error:{2}", ex.ToString()));
                throw;
            }

        }
    }
}
