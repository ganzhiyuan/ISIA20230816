using ISIA.COMMON;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TAP;
using TAP.Data.DataBase.Communicators;
using TAP.Remoting;

namespace ISIA.BIZ.TREND
{
    class WorkloadAnalysis : TAP.Remoting.Server.Biz.BizComponentBase
    {


        public void GetDBName(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("select DBID,  DBNAME  from ISIA.TAPCTDATABASE");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetWorkloadDataByParams(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                //TODO row 60 may have trouble.
                tmpSql.AppendFormat("WITH t1_sysmetric_summary AS ( SELECT  MIN (begin_interval_time) begin_time, MAX (end_interval_time)");
                tmpSql.Append(" end_time, dbid, (SELECT instance_name FROM gv$instance WHERE INSTANCE_NUMBER = s.instance_number)");
                tmpSql.Append(" dbname, snap_id, s.instance_number AS inst_id, MIN (NUM_INTERVAL) NUM_INTERVAL, SUM ( DECODE (metric_name,");
                tmpSql.Append("'Host CPU Utilization (%)', average, 0)) CPU_Util_pct, SUM ( DECODE (metric_name, 'Host CPU Utilization (%)', maxval,");
                tmpSql.Append("0)) CPU_Util_pct_max, SUM ( DECODE (metric_name, 'Logical Reads Per Sec', average, 0)) Logical_Reads_psec,");
                tmpSql.Append(" SUM (DECODE (metric_name, 'Physical Reads Per Sec', average, 0)) Physical_Reads_psec, SUM ( DECODE (metric_name,");
                tmpSql.Append("'Physical Writes Per Sec', average, 0)) Physical_Writes_psec, SUM (DECODE (metric_name, 'Executions Per Sec', average, 0))");
                tmpSql.Append(" Execs_psec_avg, SUM (DECODE (metric_name, 'Executions Per Sec', maxval, 0)) Execs_psec_max, SUM (DECODE (metric_name, 'User Calls Per Sec', average, 0))");
                tmpSql.Append(" User_Calls_psec, SUM ( DECODE (metric_name, 'DB Block Changes Per Sec', average, 0)) DB_Block_Changes_psec, SUM (");
                tmpSql.Append(" DECODE (metric_name, 'SQL Service Response Time', average, 0)) SQL_Service_Response_Time, SUM (");
                tmpSql.Append(" DECODE (metric_name, 'User Commits Per Sec', average, 0)) User_Commits_psec, SUM ( DECODE (metric_name, 'Redo Generated Per Sec', average,");
                tmpSql.Append("0)) Redo_Generated_psec, SUM ( DECODE (metric_name, 'Hard Parse Count Per Sec', average, 0)) Hard_Parse_Cnt_psec");
                tmpSql.AppendFormat(" FROM (SELECT  sm.*, sn.begin_interval_time, sn.end_interval_time FROM ISIA.RAW_DBA_HIST_SYSMETRIC_SUMMARY_{0} sm,", arguments.DBName);
                tmpSql.AppendFormat(" ISIA.RAW_DBA_HIST_SNAPSHOT_{0} sn WHERE 1 = 1 AND sm.dbid = sn.dbid AND sm.INSTANCE_NUMBER = sn.INSTANCE_NUMBER AND sm.snap_id = sn.snap_id", arguments.DBName);
                tmpSql.AppendFormat(" AND sn.INSTANCE_NUMBER IN (1)  AND TO_CHAR (sn.BEGIN_INTERVAL_TIME, 'yyyyMMddHH24miss') BETWEEN '{0}'", arguments.StartTime);
                tmpSql.Append(" AND '{2}' ) s WHERE 1 = 1 GROUP BY dbid, s.instance_number, snap_id), t1_sysstat AS (SELECT  dbid, INSTANCE_NUMBER AS inst_id, snap_id,");
                tmpSql.Append(" begin_interval_time AS begin_time, end_interval_time AS end_time, (SELECT EXTRACT ( HOUR FROM ( END_INTERVAL_TIME - BEGIN_INTERVAL_TIME))");
                tmpSql.Append(" * 60 * 60 + EXTRACT ( MINUTE FROM ( END_INTERVAL_TIME - BEGIN_INTERVAL_TIME)) * 60 + EXTRACT ( SECOND FROM ( END_INTERVAL_TIME - BEGIN_INTERVAL_TIME))");
                tmpSql.Append(" FROM DBA_HIST_SNAPSHOT WHERE dbid = s.dbid AND SNAP_ID = s.SNAP_ID AND INSTANCE_NUMBER = s.INSTANCE_NUMBER) snap_time, ROUND ( ( NET_Cnt_Client");
                tmpSql.Append("- LAG (NET_Cnt_Client, 1) OVER (PARTITION BY dbid, INSTANCE_NUMBER ORDER BY snap_id))) NET_Cnt_Client, ROUND ( ( NET_B_To_Client - LAG (NET_B_To_Client, 1)");
                tmpSql.Append(" OVER (PARTITION BY dbid, INSTANCE_NUMBER ORDER BY snap_id))) NET_B_To_Client, ROUND ( ( NET_B_From_Client - LAG (NET_B_From_Client, 1) OVER (PARTITION BY dbid, INSTANCE_NUMBER");
                tmpSql.Append(" ORDER BY snap_id))) NET_B_From_Client, ROUND ( ( NET_Cnt_DBLink - LAG (NET_Cnt_DBLink, 1) OVER (PARTITION BY dbid, INSTANCE_NUMBER ORDER BY snap_id)))");
                tmpSql.Append(" NET_Cnt_DBLink, ROUND ( ( NET_B_From_DBLink - LAG (NET_B_From_DBLink, 1) OVER (PARTITION BY dbid, INSTANCE_NUMBER ORDER BY snap_id))) NET_B_From_DBLink,ROUND (");
                tmpSql.Append("( NET_B_To_DBLink - LAG (NET_B_To_DBLink, 1) OVER (PARTITION BY dbid, INSTANCE_NUMBER ORDER BY snap_id))) NET_B_To_DBLink, ROUND ( ( gc_recv - LAG (gc_recv, 1)");
                tmpSql.Append("OVER (PARTITION BY dbid, INSTANCE_NUMBER ORDER BY snap_id)), 2) gc_recv, ROUND ( ( gc_send - LAG (gc_send, 1) OVER (PARTITION BY dbid, INSTANCE_NUMBER");
                tmpSql.Append("ORDER BY snap_id)), 2) gc_send, ROUND ( ( gcs_msg_send - LAG (gcs_msg_send, 1) OVER (PARTITION BY dbid, INSTANCE_NUMBER ORDER BY snap_id)), 2) gcs_msg_send");
                tmpSql.Append("FROM ( SELECT dbid,snap_id,INSTANCE_NUMBER,MIN (begin_interval_time)AS begin_interval_time, MAX (end_interval_time)");
                tmpSql.Append("AS end_interval_time,SUM (DECODE (stat_name,'SQL*Net roundtrips to/from client', VALUE, 0)) NET_Cnt_Client, SUM ( DECODE (stat_name, 'bytes sent via SQL*Net to client', VALUE, ");
                tmpSql.Append("0)) NET_B_To_Client, SUM ( DECODE ( stat_name, 'bytes received via SQL*Net from client', VALUE, 0)) NET_B_From_Client, SUM ( DECODE ( stat_name, 'SQL*Net roundtrips to/from dblink', VALUE, ");
                tmpSql.Append("0)) NET_Cnt_DBLink, SUM ( DECODE ( stat_name, 'bytes received via SQL*Net from dblink', VALUE, 0)) NET_B_From_DBLink, SUM (DECODE ( stat_name, 'bytes sent via SQL*Net to dblink', VALUE, ");
                tmpSql.Append("0)) NET_B_To_DBLink,SUM ( DECODE (stat_name, 'gc cr blocks received', VALUE, 'gc current blocks received', VALUE, 0)) gc_recv , SUM (DECODE (stat_name,'gc cr blocks served', VALUE, ");
                tmpSql.Append("'gc current blocks served', VALUE, 0)) gc_send , SUM ( DECODE (stat_name, 'gcs messages sent', VALUE, 'ges messages sent', VALUE, 0)) gcs_msg_send ");
                tmpSql.Append("FROM (SELECT  ss.dbid, ss.instance_number, ss.snap_id, ss.VALUE, ss.stat_name, sn.begin_interval_time, sn.end_interval_time ");
                tmpSql.AppendFormat("FROM ISIA.RAW_DBA_HIST_SYSSTAT_{0} ss,", arguments.DBName);
                tmpSql.AppendFormat("ISIA.RAW_DBA_HIST_SNAPSHOT_{0} sn ", arguments.DBName);
                tmpSql.AppendFormat("WHERE 1 = 1 AND ss.dbid = sn.dbid AND ss.INSTANCE_NUMBER = sn.INSTANCE_NUMBER AND ss.snap_id = sn.snap_id AND sn.INSTANCE_NUMBER IN (1) AND TO_CHAR (sn.BEGIN_INTERVAL_TIME, ");
                tmpSql.AppendFormat("'yyyyMMddHH24miss') BETWEEN '{0}' ", arguments.StartTime);
                tmpSql.AppendFormat("AND '{0}'", arguments.EndTime);
                tmpSql.AppendFormat(@") WHERE 1 = 1 GROUP BY dbid, INSTANCE_NUMBER, snap_id) s), t2_sysmetric_summary AS ( SELECT dbid, inst_id, MIN (snap_id) snap_id_min, TO_CHAR (BEGIN_TIME, '{0}') ", arguments.GroupingDateFormat);
                tmpSql.Append("workdate, MIN (BEGIN_TIME) BEGIN_TIME, MAX (END_TIME) END_TIME, ROUND (AVG (CPU_Util_pct), 2) CPU_Util_pct, ROUND (MAX (CPU_Util_pct_max), 2) CPU_Util_pct_max, ");
                tmpSql.Append("ROUND (AVG (LOGICAL_READS_PSEC)) LOGICAL_READS_PSEC, ROUND (AVG (Physical_Reads_psec)) PHYSICAL_READS_PSEC, ROUND (AVG (Physical_Writes_psec)) Physical_Writes_psec, ");
                tmpSql.Append("ROUND (AVG (Execs_psec_avg)) Execs_psec_avg, ROUND (MAX (Execs_psec_max)) Execs_psec_max, ROUND (AVG (USER_CALLS_PSEC)) USER_CALLS_PSEC, ROUND (AVG (DB_BLOCK_CHANGES_PSEC)) ");
                tmpSql.Append("DB_BLOCK_CHANGES_PSEC, ROUND (AVG (SQL_Service_Response_Time), 4) SQL_Service_Response_Time, ROUND (AVG (User_Commits_psec), 2) Commit_psec_avg, ROUND (AVG (Redo_Generated_psec / 1024 / 1024), 2) ");
                tmpSql.AppendFormat("Redo_mb_psec_avg, ROUND (AVG (Hard_Parse_Cnt_psec), 2) Hard_Parse_Cnt_psec FROM t1_sysmetric_summary s WHERE 1 = 1 GROUP BY dbid, inst_id, TO_CHAR (BEGIN_TIME, '{0}')), ", arguments.GroupingDateFormat);
                tmpSql.AppendFormat("t2_sysstat AS ( SELECT dbid, inst_id, MIN (snap_id) AS snap_id_min, TO_CHAR (BEGIN_TIME, '{0}') workdate, MIN (BEGIN_TIME) BEGINE_TIME, MAX (END_TIME) ", arguments.GroupingDateFormat);
                tmpSql.Append("END_TIME, ROUND ( AVG ( ((gc_recv + gc_send) * 8192 + gcs_msg_send * 200) / 1024 / 1024 / SNAP_TIME), 3) DLM_MB_psec, ROUND (AVG (NET_B_To_Client / 1024 / 1024 / SNAP_TIME), 3) ");
                tmpSql.Append("NET_MB_To_Client_psec, ROUND (AVG (NET_B_From_Client / 1024 / 1024 / SNAP_TIME), 3) NET_MB_From_Client_psec, ROUND (AVG (NET_B_From_DBLink / 1024 / 1024 / SNAP_TIME), 3) ");
                tmpSql.AppendFormat("NET_MB_From_DBLink_psec, ROUND (AVG (NET_B_To_DBLink / 1024 / 1024 / SNAP_TIME), 3) NET_MB_To_DBLink_psec FROM t1_sysstat s WHERE 1 = 1 GROUP BY dbid, inst_id, TO_CHAR (BEGIN_TIME, '{0}')) ", arguments.GroupingDateFormat);
                tmpSql.Append("SELECT sm.dbid, sm.inst_id, sm.snap_id_min, sm.workdate AS workdate, sm.begin_time, sm.end_time, sm.CPU_Util_pct, sm.CPU_Util_pct_max, sm.LOGICAL_READS_PSEC, ");
                tmpSql.Append("sm.PHYSICAL_READS_PSEC, sm.Physical_Writes_psec, sm.Execs_psec_avg, sm.Execs_psec_max, sm.USER_CALLS_PSEC, sm.Hard_Parse_Cnt_psec, sm.DB_BLOCK_CHANGES_PSEC, ");
                tmpSql.Append("sm.SQL_Service_Response_Time, sm.Commit_psec_avg, sm.Redo_mb_psec_avg, ss.DLM_MB_psec, ss.NET_MB_To_Client_psec, ss.NET_MB_From_Client_psec, ss.NET_MB_From_DBLink_psec, ");
                tmpSql.Append("ss.NET_MB_To_DBLink_psec FROM t2_sysmetric_summary sm, t2_sysstat ss WHERE sm.dbid = ss.dbid(+) AND sm.inst_id = ss.inst_id(+) AND sm.workdate = ss.workdate(+) ORDER BY dbid, workdate ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }



        private void AppendWithCRLF(StringBuilder main, string append)
        {
            main.Append(append).Append(" ");
        }

        private void AppendSumWithDeode(StringBuilder main, string colunm, string decodeMode, string paramType)
        {
            main.AppendFormat("SUM(DECODE({2},'{0}',{1} ,0)) \"{3}\"", HandleSpecialCharacter(colunm), decodeMode, paramType, colunm);
        }

        private void AppendMin(StringBuilder main, string colunm, string alias)
        {
            main.AppendFormat("MIN({0}) \"{1}\"", colunm, alias).Append(",");
        }

        private void AppendMax(StringBuilder main, string colunm, string alias)
        {
            main.AppendFormat("MAX({0}) \"{1}\"", colunm, alias).Append(",");
        }

        private void AppendWithComma(StringBuilder main, string append)
        {
            main.Append(append).Append(",");
        }

        private void AppendRoundWithAVG(StringBuilder main, string colunm)
        {
            main.AppendFormat("ROUND(AVG(\"{0}\")) \"{0}\"", colunm);
        }
        private string HandleSpecialCharacter(string str)
        {
            StringBuilder sb = new StringBuilder("");
            sb.Append(str);
            if (str.ToString().Contains("'"))
            {
                string specialStr = str.ToString();
                sb.Insert(specialStr.IndexOf("'"), "'");
            }
            return sb.ToString();
        }

        private int GetCutCount(int total, int everyMaxCount)
        {
            int cutCount = total / everyMaxCount;
            if (cutCount == 0)
            {
                return 1;
            }
            if (total % everyMaxCount != 0)
            {
                cutCount++;
            }
            return cutCount;
        }

        private List<string> CutListByCutCount(List<string> origin, int cutCount, int cutIndex)
        {
            List<string> temp = new List<string>();
            if (origin.Count == 0)
            {
                return temp;
            }
            int listTotolCount = origin.Count;
            int everyInterval = listTotolCount / cutCount + 1;
            if (((cutIndex - 1) * everyInterval) <= listTotolCount - 1 && (cutIndex * everyInterval - 1) <= listTotolCount - 1)
            {
                temp = origin.GetRange((cutIndex - 1) * everyInterval, everyInterval);

            }
            else if (((cutIndex - 1) * everyInterval) <= listTotolCount - 1 && (cutIndex * everyInterval - 1) > listTotolCount - 1)
            {
                temp = origin.GetRange((cutIndex - 1) * everyInterval, listTotolCount % everyInterval);
            }
            return temp;
        }

    }
}
