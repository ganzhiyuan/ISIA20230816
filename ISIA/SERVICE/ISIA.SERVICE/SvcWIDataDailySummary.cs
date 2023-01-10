using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Models.SystemBasic;
using TAP.Base.Communication;
using System.IO;
using TAP.Data.DataBase.Communicators;
using System.Data;

namespace ISIA.SERVICE
{
    class SvcWIDataDailySummary : TAP.WinService.ServiceBase
    {
        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            DataTable dt = GetWISummaryData();
            SaveWISummaryData(dt);
            SaveWaferQtyByHour();
            SaveWaferQtyByHour2();
            DeleteWIRawData();
        }

        private DataTable GetWISummaryData()
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();
                string date = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
#if MSSQL
                tmpsb.Append("SELECT  WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, SHIFT, PARAMETER_NAME, MAX((CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_LDEQP ");
                tmpsb.AppendFormat("WHERE  REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME IN( 'PRODUCTION','RUNTIME','STOPTIME','IDLETIME') ");
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, SHIFT, PARAMETER_NAME ");
#endif
#if ORACLE
                tmpsb.Append("SELECT  WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, SHIFT, PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.AppendFormat("WHERE  REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME IN( 'TotalQtyOut','TotalStandbyTime','TotalProductive','TotalUnscheduledDown','TotalScheduledDown') ");
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, SHIFT, PARAMETER_NAME ");
#endif
                DataTable tmpdt = db.Select(tmpsb.ToString()).Tables[0];

                return tmpdt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SaveWISummaryData(DataTable dtSum)
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                List<string> SqlList = new List<string>();

                foreach (DataRow dr in dtSum.Rows)
                {
                    string strParameter = "";

                    if(dr["PARAMETER_NAME"].ToString() == "TotalProductive")
                    {
                        strParameter = "RUN_TIME";
                    }
                    else if (dr["PARAMETER_NAME"].ToString() == "TotalUnscheduledDown")
                    {
                        strParameter = "DOWN_TIME";
                    }
                    else if (dr["PARAMETER_NAME"].ToString() == "TotalScheduledDown")
                    {
                        strParameter = "PM_TIME";
                    }
                    else if (dr["PARAMETER_NAME"].ToString() == "TotalStandbyTime")
                    {
                        strParameter = "IDLE_TIME";
                    }
                    else if (dr["PARAMETER_NAME"].ToString() == "TotalQtyOut")
                    {
                        strParameter = "WAFER_QTY";
                    }
                    else
                    {
                        strParameter = dr["PARAMETER_NAME"].ToString();
                    }


                    StringBuilder tmpsb = new StringBuilder();
                    tmpsb.Append("INSERT INTO  TAPEQDATASUMM (WORKSHOP, PROCESS, \"DATE\", ");
                    tmpsb.Append("SHIFT, CATEGORY1, CATEGORY2, ");
                    tmpsb.Append("CATEGORY3, CATEGORY4, CATEGORY5, ");
                    tmpsb.Append("PARAMETER_NAME, PARAMETER_VALUE, INSERT_TIME) VALUES(");
                    tmpsb.AppendFormat("'{0}',", dr["WORKSHOP"].ToString());
                    tmpsb.AppendFormat("'{0}',", "WI");
                    tmpsb.AppendFormat("'{0}',", dr["REPORT_DATE"].ToString());
                    tmpsb.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpsb.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString());
                    tmpsb.AppendFormat("'{0}',", dr["SIDE"].ToString());
                    tmpsb.AppendFormat("'{0}',", "ALL");
                    tmpsb.AppendFormat("'{0}',", "ALL");
                    tmpsb.AppendFormat("'{0}',", "ALL");
                    tmpsb.AppendFormat("'{0}',", strParameter);
                    tmpsb.AppendFormat("'{0}',", dr["PARAMETER_VALUE"].ToString());
                    tmpsb.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    SqlList.Add(tmpsb.ToString());
                }

                db.Save(SqlList);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void DeleteWIRawData()
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();
                string date = DateTime.Now.AddDays(-2).ToString("yyyyMMdd");

                tmpsb.Append("DELETE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE < '{0}' ", date);

                db.Save(new string[] { tmpsb.ToString() });
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SaveWaferQtyByHour()
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();
                string date = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
#if MSSQL
                tmpsb.Append("SELECT  SUBSTRING(INSERT_TIME, 0, 11) AS WORKHOUR, WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, SHIFT, PARAMETER_NAME, MAX((CASE WHEN ISNUMERIC(PARAMETER_VALUE)=1 AND PARAMETER_VALUE NOT LIKE '%[^0-9.]%'THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_LDEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'PRODUCTION' ");
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, SHIFT, PARAMETER_NAME, SUBSTRING(INSERT_TIME, 0, 11)");
#endif
#if ORACLE
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, SHIFT, PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME like '%TotalNumberOfWafer' ");
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, SHIFT, PARAMETER_NAME");
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
                    tmpsb1.AppendFormat("'{0}',", "WI");
                    tmpsb1.AppendFormat("'{0}',", dr["REPORT_DATE"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["SIDE"].ToString());
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "ALL");
                    tmpsb1.AppendFormat("'{0}',", "WAFER_QTY" + "_" + dr["PARAMETER_NAME"].ToString().Substring(0, 2));
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

        private void SaveWaferQtyByHour2()
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();
                string date = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                string date2 = DateTime.Now.ToString("yyyyMMdd");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'D' as SHIFT, 'OUTPUT_0730' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}073000' AND INSERT_TIME < '{1}083000' ",date ,date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE" );
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'D' as SHIFT, 'OUTPUT_0830' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}083000' AND INSERT_TIME < '{1}093000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'D' as SHIFT, 'OUTPUT_0930' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}093000' AND INSERT_TIME < '{1}103000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1030' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}103000' AND INSERT_TIME < '{1}113000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1130' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}113000' AND INSERT_TIME < '{1}123000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1230' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}123000' AND INSERT_TIME < '{1}133000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1330' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}133000' AND INSERT_TIME < '{1}143000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1430' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}143000' AND INSERT_TIME < '{1}153000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1530' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}153000' AND INSERT_TIME < '{1}163000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1630' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}163000' AND INSERT_TIME < '{1}173000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1730' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}173000' AND INSERT_TIME < '{1}183000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'D' as SHIFT, 'OUTPUT_1830' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}183000' AND INSERT_TIME < '{1}193000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE ");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'N' as SHIFT, 'OUTPUT_1930' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}193000' AND INSERT_TIME < '{1}203000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'N' as SHIFT, 'OUTPUT_2030' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}203000' AND INSERT_TIME < '{1}213000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'N' as SHIFT, 'OUTPUT_2130' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}213000' AND INSERT_TIME < '{1}223000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE");                
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'N' as SHIFT, 'OUTPUT_2230' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}223000' AND INSERT_TIME < '{1}233000' ", date, date);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'N' as SHIFT, 'OUTPUT_2330' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}233000' AND INSERT_TIME < '{1}003000' ", date, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0030' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}003000' AND INSERT_TIME < '{1}013000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0130' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}013000' AND INSERT_TIME < '{1}023000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0230' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}023000' AND INSERT_TIME < '{1}033000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0330' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}033000' AND INSERT_TIME < '{1}043000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0430' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}043000' AND INSERT_TIME < '{1}053000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0530' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}053000' AND INSERT_TIME < '{1}063000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE");
                tmpsb.Append(" UNION ALL ");
                tmpsb.Append("SELECT WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE, 'N' as SHIFT, 'OUTPUT_0630' AS PARAMETER_NAME, MAX((CASE WHEN REGEXP_INSTR(PARAMETER_VALUE, '^[+-]?\\d*(\\.?\\d*)$') = 1 THEN CAST(PARAMETER_VALUE AS DECIMAL(38,2))    ELSE 0.0 END)) AS PARAMETER_VALUE FROM TAPIFTB_WIEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE = '{0}' ", date);
                tmpsb.Append("AND PARAMETER_NAME = 'TotalQtyIn' ");
                tmpsb.AppendFormat(" AND INSERT_TIME >= '{0}063000' AND INSERT_TIME < '{1}073000' ", date2, date2);
                tmpsb.Append("GROUP BY WORKSHOP, EQUIPMENT, SIDE, REPORT_DATE");

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
                    tmpsb1.AppendFormat("'{0}',", "WI");
                    tmpsb1.AppendFormat("'{0}',", dr["REPORT_DATE"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString());
                    tmpsb1.AppendFormat("'{0}',", dr["SIDE"].ToString());
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
    }
}
