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
    class WorkloadTrendChart : TAP.Remoting.Server.Biz.BizComponentBase
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

        public void GetWorkLoadTrend(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("SELECT T.* FROM sum_workload T where 1=1 ");
                tmpSql.AppendFormat(" and END_TIME >=TO_DATE('{0}', 'yyyy-MM-dd HH24:mi:ss')", arguments.StartTime);
                tmpSql.AppendFormat(" and END_TIME <TO_DATE('{0}', 'yyyy-MM-dd HH24:mi:ss')", arguments.EndTime);



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
                tmpSql.AppendFormat("" +
                    "WITH\r\n" +
                    "t1_sysmetric_summary\r\n" +
                    "AS\r\n" +
                    "( SELECT /*+ MATERIALIZE */\r\n" +
                    "MIN (begin_interval_time)\r\n" +
                    "begin_time,\r\n" +
                    "MAX (end_interval_time)\r\n" +
                    "end_time,\r\n" +
                    "dbid,\r\n" +
                    "(SELECT instance_name\r\n" +
                    "FROM gv$instance\r\n" +
                    "WHERE INSTANCE_NUMBER = s.instance_number)\r\n" +
                    "dbname,\r\n" +
                    "snap_id,\r\n" +
                    "s.instance_number\r\n" +
                    "AS inst_id,\r\n" +
                    "MIN (NUM_INTERVAL)\r\n" +
                    "NUM_INTERVAL,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (metric_name,\r\n" +
                    "'Host CPU Utilization (%)', average,\r\n" +
                    "0))\r\n" +
                    "CPU_Util_pct,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (metric_name,\r\n" +
                    "'Host CPU Utilization (%)', maxval,\r\n" +
                    "0))\r\n" +
                    "CPU_Util_pct_max,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (metric_name, 'Logical Reads Per Sec', average, 0))\r\n" +
                    "Logical_Reads_psec,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (metric_name,\r\n" +
                    "'Physical Reads Per Sec', average,\r\n" +
                    "0))\r\n" +
                    "Physical_Reads_psec,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (metric_name,\r\n" +
                    "'Physical Writes Per Sec', average,\r\n" +
                    "0))\r\n" +
                    "Physical_Writes_psec,\r\n" +
                    "SUM (DECODE (metric_name, 'Executions Per Sec', average, 0))\r\n" +
                    "Execs_psec_avg,\r\n" +
                    "SUM (DECODE (metric_name, 'Executions Per Sec', maxval, 0))\r\n" +
                    "Execs_psec_max,\r\n" +
                    "SUM (DECODE (metric_name, 'User Calls Per Sec', average, 0))\r\n" +
                    "User_Calls_psec,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (metric_name,\r\n" +
                    "'DB Block Changes Per Sec', average,\r\n" +
                    "0))\r\n" +
                    "DB_Block_Changes_psec,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (metric_name,\r\n" +
                    "'SQL Service Response Time', average,\r\n" +
                    "0))\r\n" +
                    "SQL_Service_Response_Time,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (metric_name, 'User Commits Per Sec', average, 0))\r\n" +
                    "User_Commits_psec,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (metric_name,\r\n" +
                    "'Redo Generated Per Sec', average,\r\n" +
                    "0))\r\n" +
                    "Redo_Generated_psec,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (metric_name,\r\n" +
                    "'Hard Parse Count Per Sec', average,\r\n" +
                    "0))\r\n" +
                    "Hard_Parse_Cnt_psec\r\n" +
                    "FROM (SELECT /*+ LEADING(sn sm) USE_HASH(sn sm) USE_HASH(sm.sn sm.m sn.mn) no_merge(sm) */\r\n" +
                    "sm.*, sn.begin_interval_time, sn.end_interval_time\r\n" +
                    "FROM ISIA.RAW_DBA_HIST_SYSMETRIC_SUMMARY_{3} sm, -- DBA_HIST_METRIC_NAME mn,\r\n" +
                    "ISIA.RAW_DBA_HIST_SNAPSHOT_{3} sn\r\n" +
                    "WHERE 1 = 1\r\n" +
                    "-- and sm.dbid = mn.dbid and sm.group_id = mn.group_id and sm.metric_id = mn.metric_id\r\n" +
                    "AND sm.dbid = sn.dbid\r\n" +
                    "AND sm.INSTANCE_NUMBER = sn.INSTANCE_NUMBER\r\n" +
                    "AND sm.snap_id = sn.snap_id\r\n" +
                    "AND sn.INSTANCE_NUMBER IN (1) --<< 조회 대상 instance number 입력\r\n" +
                    "AND TO_CHAR (sn.BEGIN_INTERVAL_TIME, 'yyyyMMddHH24miss') BETWEEN '{1}'\r\n" +
                    "AND '{2}' --<< 조회 기간 입력\r\n" +
                    ")\r\n" +
                    "s\r\n" +
                    "WHERE 1 = 1\r\n" +
                    "GROUP BY dbid, s.instance_number, snap_id),\r\n" +

                    "t1_sysstat\r\n" +
                    "AS\r\n" +
                    "(SELECT /*+ MATERIALIZE */\r\n" +
                    "dbid,\r\n" +
                    "INSTANCE_NUMBER\r\n" +
                    "AS inst_id,\r\n" +
                    "snap_id,\r\n" +
                    "begin_interval_time\r\n" +
                    "AS begin_time,\r\n" +
                    "end_interval_time\r\n" +
                    "AS end_time,\r\n" +
                    "(SELECT EXTRACT (\r\n" +
                    "HOUR FROM ( END_INTERVAL_TIME\r\n" +
                    "- BEGIN_INTERVAL_TIME))\r\n" +
                    "* 60\r\n" +
                    "* 60\r\n" +
                    "+ EXTRACT (\r\n" +
                    "MINUTE FROM ( END_INTERVAL_TIME\r\n" +
                    "- BEGIN_INTERVAL_TIME))\r\n" +
                    "* 60\r\n" +
                    "+ EXTRACT (\r\n" +
                    "SECOND FROM ( END_INTERVAL_TIME\r\n" +
                    "- BEGIN_INTERVAL_TIME))\r\n" +
                    "FROM ISIA.RAW_DBA_HIST_SNAPSHOT\r\n" +
                    "WHERE dbid = s.dbid\r\n" +
                    "AND SNAP_ID = s.SNAP_ID\r\n" +
                    "AND INSTANCE_NUMBER = s.INSTANCE_NUMBER)\r\n" +
                    "snap_time,\r\n" +
                    "ROUND (\r\n" +
                    "( NET_Cnt_Client\r\n" +
                    "- LAG (NET_Cnt_Client, 1)\r\n" +
                    "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
                    "ORDER BY snap_id)))\r\n" +
                    "NET_Cnt_Client,\r\n" +
                    "ROUND (\r\n" +
                    "( NET_B_To_Client\r\n" +
                    "- LAG (NET_B_To_Client, 1)\r\n" +
                    "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
                    "ORDER BY snap_id)))\r\n" +
                    "NET_B_To_Client,\r\n" +
                    "ROUND (\r\n" +
                    "( NET_B_From_Client\r\n" +
                    "- LAG (NET_B_From_Client, 1)\r\n" +
                    "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
                    "ORDER BY snap_id)))\r\n" +
                    "NET_B_From_Client,\r\n" +
                    "ROUND (\r\n" +
                    "( NET_Cnt_DBLink\r\n" +
                    "- LAG (NET_Cnt_DBLink, 1)\r\n" +
                    "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
                    "ORDER BY snap_id)))\r\n" +
                    "NET_Cnt_DBLink,\r\n" +
                    "ROUND (\r\n" +
                    "( NET_B_From_DBLink\r\n" +
                    "- LAG (NET_B_From_DBLink, 1)\r\n" +
                    "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
                    "ORDER BY snap_id)))\r\n" +
                    "NET_B_From_DBLink,\r\n" +
                    "ROUND (\r\n" +
                    "( NET_B_To_DBLink\r\n" +
                    "- LAG (NET_B_To_DBLink, 1)\r\n" +
                    "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
                    "ORDER BY snap_id)))\r\n" +
                    "NET_B_To_DBLink,\r\n" +
                    "ROUND (\r\n" +
                    "( gc_recv\r\n" +
                    "- LAG (gc_recv, 1)\r\n" +
                    "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
                    "ORDER BY snap_id)),\r\n" +
                    "2)\r\n" +
                    "gc_recv,\r\n" +
                    "ROUND (\r\n" +
                    "( gc_send\r\n" +
                    "- LAG (gc_send, 1)\r\n" +
                    "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
                    "ORDER BY snap_id)),\r\n" +
                    "2)\r\n" +
                    "gc_send,\r\n" +
                    "ROUND (\r\n" +
                    "( gcs_msg_send\r\n" +
                    "- LAG (gcs_msg_send, 1)\r\n" +
                    "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
                    "ORDER BY snap_id)),\r\n" +
                    "2)\r\n" +
                    "gcs_msg_send\r\n" +

                    "FROM ( SELECT dbid,\r\n" +
                    "snap_id,\r\n" +
                    "INSTANCE_NUMBER,\r\n" +
                    "MIN (begin_interval_time)\r\n" +
                    "AS begin_interval_time,\r\n" +
                    "MAX (end_interval_time)\r\n" +
                    "AS end_interval_time,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (\r\n" +
                    "stat_name,\r\n" +
                    "'SQL*Net roundtrips to/from client', VALUE,\r\n" +
                    "0))\r\n" +
                    "NET_Cnt_Client,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (\r\n" +
                    "stat_name,\r\n" +
                    "'bytes sent via SQL*Net to client', VALUE,\r\n" +
                    "0))\r\n" +
                    "NET_B_To_Client,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (\r\n" +
                    "stat_name,\r\n" +
                    "'bytes received via SQL*Net from client', VALUE,\r\n" +
                    "0))\r\n" +
                    "NET_B_From_Client,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (\r\n" +
                    "stat_name,\r\n" +
                    "'SQL*Net roundtrips to/from dblink', VALUE,\r\n" +
                    "0))\r\n" +
                    "NET_Cnt_DBLink,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (\r\n" +
                    "stat_name,\r\n" +
                    "'bytes received via SQL*Net from dblink', VALUE,\r\n" +
                    "0))\r\n" +
                    "NET_B_From_DBLink,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (\r\n" +
                    "stat_name,\r\n" +
                    "'bytes sent via SQL*Net to dblink', VALUE,\r\n" +
                    "0))\r\n" +
                    "NET_B_To_DBLink,\r\n" +
                    "SUM (\r\n" +
                    "DECODE (stat_name,\r\n" +
                    "'gc cr blocks received', VALUE,\r\n" +
                    "'gc current blocks received', VALUE,\r\n" +
                    "0))\r\n" +
                    "gc_recv -- Global Cache blocks received\r\n" +
                    ",\r\n" +
                    "SUM (\r\n" +
                    "DECODE (stat_name,\r\n" +
                    "'gc cr blocks served', VALUE,\r\n" +
                    "'gc current blocks served', VALUE,\r\n" +
                    "0))\r\n" +
                    "gc_send -- Global Cache blocks served\r\n" +
                    ",\r\n" +
                    "SUM (\r\n" +
                    "DECODE (stat_name,\r\n" +
                    "'gcs messages sent', VALUE,\r\n" +
                    "'ges messages sent', VALUE,\r\n" +
                    "0))\r\n" +
                    "gcs_msg_send -- GCS/GES messages sent\r\n" +
                    "FROM (SELECT /*+ LEADING(sn ss) USE_HASH(sn ss) USE_HASH(ss.sn ss.s ss.nm) no_merge(ss) */\r\n" +
                    "ss.dbid,\r\n" +
                    "ss.instance_number,\r\n" +
                    "ss.snap_id,\r\n" +
                    "ss.VALUE,\r\n" +
                    "ss.stat_name,\r\n" +
                    "sn.begin_interval_time,\r\n" +
                    "sn.end_interval_time\r\n" +
                    "FROM ISIA.RAW_DBA_HIST_SYSSTAT_{3} ss, -- DBA_HIST_STAT_NAME nm,\r\n" +
                    "ISIA.RAW_DBA_HIST_SNAPSHOT_{3} sn\r\n" +
                    "WHERE 1 = 1\r\n" +
                    "-- and ss.dbid = nm.dbid and ss.stat_id = nm.stat_id\r\n" +
                    "AND ss.dbid = sn.dbid\r\n" +
                    "AND ss.INSTANCE_NUMBER = sn.INSTANCE_NUMBER\r\n" +
                    "AND ss.snap_id = sn.snap_id\r\n" +
                    "AND sn.INSTANCE_NUMBER IN (1) --<< 조회 대상 instance number 입력))\r\n" +
                    "AND TO_CHAR (sn.BEGIN_INTERVAL_TIME,\r\n" +
                    "'yyyyMMddHH24miss') BETWEEN '{1}'\r\n" +
                    "AND '{2}' --<< 조회 기간 입력\r\n" +
                    ")\r\n" +
                    "WHERE 1 = 1\r\n" +
                    "GROUP BY dbid, INSTANCE_NUMBER, snap_id) s),\r\n" +
                    "t2_sysmetric_summary\r\n" +
                    "AS\r\n" +
                    "( SELECT dbid,\r\n" +
                    "inst_id,\r\n" +
                    "MIN (snap_id)\r\n" +
                    "snap_id_min,\r\n" +
                    "TO_CHAR (BEGIN_TIME, '{0}')\r\n" +
                    "workdate,\r\n" +
                    "MIN (BEGIN_TIME)\r\n" +
                    "BEGIN_TIME,\r\n" +
                    "MAX (END_TIME)\r\n" +
                    "END_TIME,\r\n" +
                    "ROUND (AVG (CPU_Util_pct), 2)\r\n" +
                    "CPU_Util_pct,\r\n" +
                    "ROUND (MAX (CPU_Util_pct_max), 2)\r\n" +
                    "CPU_Util_pct_max,\r\n" +
                    "ROUND (AVG (LOGICAL_READS_PSEC))\r\n" +
                    "LOGICAL_READS_PSEC,\r\n" +
                    "ROUND (AVG (Physical_Reads_psec))\r\n" +
                    "PHYSICAL_READS_PSEC,\r\n" +
                    "ROUND (AVG (Physical_Writes_psec))\r\n" +
                    "Physical_Writes_psec,\r\n" +
                    "ROUND (AVG (Execs_psec_avg))\r\n" +
                    "Execs_psec_avg,\r\n" +
                    "ROUND (MAX (Execs_psec_max))\r\n" +
                    "Execs_psec_max,\r\n" +
                    "ROUND (AVG (USER_CALLS_PSEC))\r\n" +
                    "USER_CALLS_PSEC,\r\n" +
                    "ROUND (AVG (DB_BLOCK_CHANGES_PSEC))\r\n" +
                    "DB_BLOCK_CHANGES_PSEC,\r\n" +
                    "ROUND (AVG (SQL_Service_Response_Time), 4)\r\n" +
                    "SQL_Service_Response_Time,\r\n" +
                    "ROUND (AVG (User_Commits_psec), 2)\r\n" +
                    "Commit_psec_avg,\r\n" +
                    "ROUND (AVG (Redo_Generated_psec / 1024 / 1024), 2)\r\n" +
                    "Redo_mb_psec_avg,\r\n" +
                    "ROUND (AVG (Hard_Parse_Cnt_psec), 2)\r\n" +
                    "Hard_Parse_Cnt_psec\r\n" +
                    "FROM t1_sysmetric_summary s\r\n" +
                    "WHERE 1 = 1\r\n" +
                    "GROUP BY dbid, inst_id, TO_CHAR (BEGIN_TIME, '{0}')),\r\n" +

                    "t2_sysstat\r\n" +
                    "AS\r\n" +
                    "( SELECT dbid,\r\n" +
                    "inst_id,\r\n" +
                    "MIN (snap_id)\r\n" +
                    "AS snap_id_min,\r\n" +
                    "TO_CHAR (BEGIN_TIME, '{0}')\r\n" +
                    "workdate,\r\n" +
                    "MIN (BEGIN_TIME)\r\n" +
                    "BEGINE_TIME,\r\n" +
                    "MAX (END_TIME)\r\n" +
                    "END_TIME,\r\n" +
                    "ROUND (\r\n" +
                    "AVG (\r\n" +
                    "((gc_recv + gc_send) * 8192 + gcs_msg_send * 200)\r\n" +
                    "/ 1024\r\n" +
                    "/ 1024\r\n" +
                    "/ SNAP_TIME),\r\n" +
                    "3)\r\n" +
                    "DLM_MB_psec,\r\n" +
                    "ROUND (AVG (NET_B_To_Client / 1024 / 1024 / SNAP_TIME), 3)\r\n" +
                    "NET_MB_To_Client_psec,\r\n" +
                    "ROUND (AVG (NET_B_From_Client / 1024 / 1024 / SNAP_TIME), 3)\r\n" +
                    "NET_MB_From_Client_psec,\r\n" +
                    "ROUND (AVG (NET_B_From_DBLink / 1024 / 1024 / SNAP_TIME), 3)\r\n" +
                    "NET_MB_From_DBLink_psec,\r\n" +
                    "ROUND (AVG (NET_B_To_DBLink / 1024 / 1024 / SNAP_TIME), 3)\r\n" +
                    "NET_MB_To_DBLink_psec\r\n" +
                    "FROM t1_sysstat s\r\n" +
                    "WHERE 1 = 1\r\n" +
                    "GROUP BY dbid, inst_id, TO_CHAR (BEGIN_TIME, '{0}'))\r\n" +

                    "SELECT sm.dbid,\r\n" +
                    "sm.inst_id,\r\n" +
                    "sm.snap_id_min,\r\n" +
                    "sm.workdate AS workdate,\r\n" +
                    "sm.begin_time,\r\n" +
                    "sm.end_time,\r\n" +
                    "sm.CPU_Util_pct,\r\n" +
                    "sm.CPU_Util_pct_max,\r\n" +
                    "sm.LOGICAL_READS_PSEC,\r\n" +
                    "sm.PHYSICAL_READS_PSEC,\r\n" +
                    "sm.Physical_Writes_psec,\r\n" +
                    "sm.Execs_psec_avg,\r\n" +
                    "sm.Execs_psec_max,\r\n" +
                    "sm.USER_CALLS_PSEC,\r\n" +
                    "sm.Hard_Parse_Cnt_psec,\r\n" +
                    "sm.DB_BLOCK_CHANGES_PSEC,\r\n" +
                    "sm.SQL_Service_Response_Time,\r\n" +
                    "sm.Commit_psec_avg,\r\n" +
                    "sm.Redo_mb_psec_avg,\r\n" +
                    "ss.DLM_MB_psec,\r\n" +
                    "ss.NET_MB_To_Client_psec,\r\n" +
                    "ss.NET_MB_From_Client_psec,\r\n" +
                    "ss.NET_MB_From_DBLink_psec,\r\n" +
                    "ss.NET_MB_To_DBLink_psec\r\n" +
                    "FROM t2_sysmetric_summary sm, t2_sysstat ss\r\n" +
                    "WHERE sm.dbid = ss.dbid(+)\r\n" +
                    "AND sm.inst_id = ss.inst_id(+)\r\n" +
                    "AND sm.workdate = ss.workdate(+)\r\n" +
                    "ORDER BY dbid, workdate\r\n"
                     , arguments.GroupingDateFormat, arguments.StartTime, arguments.EndTime,arguments.DBName
                    );
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
            main.Append(append).Append("\r\n");
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
