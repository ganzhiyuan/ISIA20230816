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
        public void GetWorkLoadTrendForInterval(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"WITH t1_sysmetric_summary AS
 ( SELECT a.* ,
 ss.ELAPSED_TIME_DELTA,
 ss.EXECUTIONS_DELTA,
ss.CPU_TIME_DELTA,
ss.BUFFER_GETS_DELTA,
ss.DISK_READS_DELTA,
ss.PARSE_CALLS_DELTA 
from
 (SELECT /*+ MATERIALIZE */
   MIN(begin_interval_time) begin_time
  ,MAX(end_interval_time) end_time
  ,dbid
  ,(SELECT instance_name
      FROM gv$instance
     WHERE INSTANCE_NUMBER = s.instance_number) dbname
  ,snap_id
  ,s.instance_number AS inst_id
  ,MIN(NUM_INTERVAL) NUM_INTERVAL
  ,SUM(DECODE(metric_name, 'Host CPU Utilization (%)', average, 0)) CPU_Util_pct
  ,SUM(DECODE(metric_name, 'Host CPU Utilization (%)', maxval, 0)) CPU_Util_pct_max
  ,SUM(DECODE(metric_name, 'Logical Reads Per Sec', average, 0)) Logical_Reads_psec
  ,SUM(DECODE(metric_name, 'Physical Reads Per Sec', average, 0)) Physical_Reads_psec
  ,SUM(DECODE(metric_name, 'Physical Writes Per Sec', average, 0)) Physical_Writes_psec
  ,SUM(DECODE(metric_name, 'Executions Per Sec', average, 0)) Execs_psec_avg
  ,SUM(DECODE(metric_name, 'Executions Per Sec', maxval, 0)) Execs_psec_max
  ,SUM(DECODE(metric_name, 'User Calls Per Sec', average, 0)) User_Calls_psec
  ,SUM(DECODE(metric_name, 'DB Block Changes Per Sec', average, 0)) DB_Block_Changes_psec
  ,SUM(DECODE(metric_name, 'SQL Service Response Time', average, 0)) SQL_Service_Response_Time
  ,SUM(DECODE(metric_name, 'User Commits Per Sec', average, 0)) User_Commits_psec
  ,SUM(DECODE(metric_name, 'User Commits Per Sec', maxval, 0)) User_Commits_max_psec
  ,SUM(DECODE(metric_name, 'Redo Generated Per Sec', average, 0)) Redo_Generated_psec
  ,SUM(DECODE(metric_name, 'Redo Generated Per Sec', maxval, 0)) Redo_Generated_max_psec
    FROM (SELECT /*+  LEADING(sn sm) USE_HASH(sn sm) USE_HASH(sm.sn sm.m sn.mn) no_merge(sm) */
           sm.*, sn.begin_interval_time, sn.end_interval_time ");
                tmpSql.AppendFormat(" FROM RAW_DBA_HIST_SYSMETRIC_SUMMARY_{0} sm ", arguments.DBName);
                tmpSql.AppendFormat("  , RAW_DBA_HIST_SNAPSHOT_{0} sn ", arguments.DBName);

                tmpSql.Append(@"  WHERE 1 = 1
     
                      AND sm.dbid = sn.dbid AND
                 sm.INSTANCE_NUMBER = sn.INSTANCE_NUMBER AND ");
                tmpSql.AppendFormat(" sm.snap_id = sn.snap_id AND sn.INSTANCE_NUMBER in ({0})  ", Utils.MakeSqlQueryIn2(arguments.INSTANCE_NUMBER));
                 tmpSql.Append(@"  AND TO_CHAR(sn.BEGIN_INTERVAL_TIME, 'YYYYMMDD-HH24MI') BETWEEN
                 ");

                tmpSql.AppendFormat("  '{0}-0000' AND '{1}-2400' ", arguments.StartTime,arguments.EndTime);
                tmpSql.AppendFormat(@") s
    WHERE 1 = 1
    GROUP BY dbid, s.instance_number, snap_id) a  left join raw_dba_hist_sqlstat_{0} ss on a.SNAP_ID  = ss.SNAP_ID  ), ", arguments.DBName);

                    tmpSql.Append(@"
t1_sysstat AS
 (SELECT 
   dbid
  ,INSTANCE_NUMBER AS inst_id
  ,snap_id
  ,begin_interval_time AS begin_time
  ,end_interval_time AS end_time
  ,(SELECT EXTRACT(HOUR FROM(END_INTERVAL_TIME - BEGIN_INTERVAL_TIME)) * 60 * 60 +
           EXTRACT(MINUTE FROM(END_INTERVAL_TIME - BEGIN_INTERVAL_TIME)) * 60 +
           EXTRACT(SECOND FROM(END_INTERVAL_TIME - BEGIN_INTERVAL_TIME)) ");
                tmpSql.AppendFormat("   FROM RAW_DBA_HIST_SNAPSHOT_{0} ", arguments.DBName);

                tmpSql.Append(@" WHERE dbid = s.dbid AND SNAP_ID = s.SNAP_ID AND
           INSTANCE_NUMBER = s.INSTANCE_NUMBER) snap_time
  ,ROUND((NET_Cnt_Client - LAG(NET_Cnt_Client, 1)
          OVER(PARTITION BY dbid, INSTANCE_NUMBER ORDER BY snap_id))) NET_Cnt_Client
  ,ROUND((NET_B_To_Client - LAG(NET_B_To_Client, 1)
          OVER(PARTITION BY dbid, INSTANCE_NUMBER ORDER BY snap_id))) NET_B_To_Client
  ,ROUND((NET_B_From_Client - LAG(NET_B_From_Client, 1)
          OVER(PARTITION BY dbid, INSTANCE_NUMBER ORDER BY snap_id))) NET_B_From_Client
  ,ROUND((NET_Cnt_DBLink - LAG(NET_Cnt_DBLink, 1)
          OVER(PARTITION BY dbid, INSTANCE_NUMBER ORDER BY snap_id))) NET_Cnt_DBLink
  ,ROUND((NET_B_From_DBLink - LAG(NET_B_From_DBLink, 1)
          OVER(PARTITION BY dbid, INSTANCE_NUMBER ORDER BY snap_id))) NET_B_From_DBLink
  ,ROUND((NET_B_To_DBLink - LAG(NET_B_To_DBLink, 1)
          OVER(PARTITION BY dbid, INSTANCE_NUMBER ORDER BY snap_id))) NET_B_To_DBLink
  ,ROUND((gc_recv - LAG(gc_recv, 1)
          OVER(PARTITION BY dbid, INSTANCE_NUMBER ORDER BY snap_id))
        ,2) gc_recv
  ,ROUND((gc_send - LAG(gc_send, 1)
          OVER(PARTITION BY dbid, INSTANCE_NUMBER ORDER BY snap_id))
        ,2) gc_send
  ,ROUND((gcs_msg_send - LAG(gcs_msg_send, 1)
          OVER(PARTITION BY dbid, INSTANCE_NUMBER ORDER BY snap_id))
        ,2) gcs_msg_send
    FROM (SELECT dbid
                ,snap_id
                ,INSTANCE_NUMBER
                ,MIN(begin_interval_time) AS begin_interval_time
                ,MAX(end_interval_time) AS end_interval_time
                ,SUM(DECODE(stat_name
                           ,'SQL*Net roundtrips to/from client'
                           ,VALUE
                           ,0)) NET_Cnt_Client
                ,SUM(DECODE(stat_name
                           ,'bytes sent via SQL*Net to client'
                           ,VALUE
                           ,0)) NET_B_To_Client
                ,SUM(DECODE(stat_name
                           ,'bytes received via SQL*Net from client'
                           ,VALUE
                           ,0)) NET_B_From_Client
                ,SUM(DECODE(stat_name
                           ,'SQL*Net roundtrips to/from dblink'
                           ,VALUE
                           ,0)) NET_Cnt_DBLink
                ,SUM(DECODE(stat_name
                           ,'bytes received via SQL*Net from dblink'
                           ,VALUE
                           ,0)) NET_B_From_DBLink
                ,SUM(DECODE(stat_name
                           ,'bytes sent via SQL*Net to dblink'
                           ,VALUE
                           ,0)) NET_B_To_DBLink
                ,SUM(DECODE(stat_name
                           ,'gc cr blocks received'
                           ,VALUE
                           ,'gc current blocks received'
                           ,VALUE
                           ,0)) gc_recv 
                ,SUM(DECODE(stat_name
                           ,'gc cr blocks served'
                           ,VALUE
                           ,'gc current blocks served'
                           ,VALUE
                           ,0)) gc_send 
                ,SUM(DECODE(stat_name
                           ,'gcs messages sent'
                           ,VALUE
                           ,'ges messages sent'
                           ,VALUE
                           ,0)) gcs_msg_send
            FROM (SELECT /*+  LEADING(sn ss) USE_HASH(sn ss) USE_HASH(ss.sn ss.s ss.nm) no_merge(ss) */
                   ss.dbid
                  ,ss.instance_number
                  ,ss.snap_id
                  ,ss.VALUE
                  ,ss.stat_name
                  ,sn.begin_interval_time
                  ,sn.end_interval_time ");
                tmpSql.AppendFormat("   FROM raw_DBA_HIST_SYSSTAT_{0} ss ", arguments.DBName);
                tmpSql.AppendFormat("  ,  RAW_DBA_HIST_SNAPSHOT_{0} sn ", arguments.DBName);
                tmpSql.Append(@"    WHERE 1 = 1
                         AND ss.dbid = sn.dbid AND
                         ss.INSTANCE_NUMBER = sn.INSTANCE_NUMBER AND ");
                tmpSql.AppendFormat("  ss.snap_id = sn.snap_id AND sn.INSTANCE_NUMBER in ({0})", Utils.MakeSqlQueryIn2(arguments.INSTANCE_NUMBER));
                tmpSql.Append(@"  AND TO_CHAR(sn.BEGIN_INTERVAL_TIME, 'YYYYMMDD-HH24MI') BETWEEN ");
                tmpSql.AppendFormat("  '{0}-0000' AND '{1}-2400' ", arguments.StartTime,arguments.EndTime);
                tmpSql.Append(@")
           WHERE 1 = 1
           GROUP BY dbid, INSTANCE_NUMBER, snap_id) s),
t2_sysmetric_summary AS
 (SELECT dbid
        ,inst_id
        ,snap_id
        ,MIN(BEGIN_TIME) BEGIN_TIME
        ,MAX(END_TIME) END_TIME
        ,ROUND(AVG(CPU_Util_pct), 2) CPU_Util_pct
        ,ROUND(MAX(CPU_Util_pct_max), 2) CPU_Util_pct_max
        ,ROUND(AVG(LOGICAL_READS_PSEC)) LOGICAL_READS_PSEC
        ,ROUND(AVG(Physical_Reads_psec)) PHYSICAL_READS_PSEC
        ,ROUND(AVG(Physical_Writes_psec)) Physical_Writes_psec
        ,ROUND(AVG(Execs_psec_avg)) Execs_psec_avg
        ,ROUND(MAX(Execs_psec_max)) Execs_psec_max
        ,ROUND(AVG(USER_CALLS_PSEC)) USER_CALLS_PSEC
        ,ROUND(AVG(DB_BLOCK_CHANGES_PSEC)) DB_BLOCK_CHANGES_PSEC
        ,ROUND(AVG(SQL_Service_Response_Time), 4) SQL_Service_Response_Time
        ,ROUND(AVG(User_Commits_psec), 2) Commit_psec_avg
        ,ROUND(AVG(User_Commits_max_psec), 2) Commit_psec_max
        ,ROUND(AVG(Redo_Generated_psec / 1024 / 1024), 2) Redo_mb_psec_avg
        ,ROUND(AVG(Redo_Generated_max_psec / 1024 / 1024), 2) Redo_mb_psec_max

        ,ROUND(AVG(ELAPSED_TIME_DELTA), 2) ELAPSED_TIME_DELTA
        ,ROUND(AVG(EXECUTIONS_DELTA), 2) EXECUTIONS_DELTA
        ,ROUND(AVG(CPU_TIME_DELTA), 2) CPU_TIME_DELTA
        ,ROUND(AVG(BUFFER_GETS_DELTA), 2) BUFFER_GETS_DELTA
        ,ROUND(AVG(DISK_READS_DELTA), 2) DISK_READS_DELTA
        ,ROUND(AVG(PARSE_CALLS_DELTA), 2) PARSE_CALLS_DELTA


    FROM t1_sysmetric_summary s
   WHERE 1 = 1
   GROUP BY dbid, inst_id, snap_id),
t2_sysstat AS
 (SELECT dbid
        ,inst_id
        ,snap_id
        ,MIN(BEGIN_TIME) BEGIN_TIME
        ,MAX(END_TIME) END_TIME
        ,ROUND(AVG(((gc_recv + gc_send) * 8192 + gcs_msg_send * 200) / 1024 / 1024 /
                   SNAP_TIME)
              ,3) DLM_MB_psec
        ,ROUND(AVG(NET_B_To_Client / 1024 / 1024 / SNAP_TIME), 3) NET_MB_To_Client_psec
        ,ROUND(AVG(NET_B_From_Client / 1024 / 1024 / SNAP_TIME), 3) NET_MB_From_Client_psec
        ,ROUND(AVG(NET_B_From_DBLink / 1024 / 1024 / SNAP_TIME), 3) NET_MB_From_DBLink_psec
        ,ROUND(AVG(NET_B_To_DBLink / 1024 / 1024 / SNAP_TIME), 3) NET_MB_To_DBLink_psec
    FROM t1_sysstat s
   WHERE 1 = 1
   GROUP BY dbid, inst_id, snap_id)
SELECT sm.dbid
      ,sm.inst_id as INSTANCE_NUMBER
      ,sm.snap_id
      ,TO_CHAR(sm.begin_time, 'yyyyMMDDHH24MI') AS WORKDATE
      ,sm.begin_time
      ,sm.end_time
      ,sm.CPU_Util_pct
      ,sm.CPU_Util_pct_max
      ,sm.LOGICAL_READS_PSEC
      ,sm.PHYSICAL_READS_PSEC
      ,sm.Physical_Writes_psec
      ,sm.Execs_psec_avg
      ,sm.Execs_psec_max
      ,sm.USER_CALLS_PSEC
      ,sm.DB_BLOCK_CHANGES_PSEC
      ,sm.SQL_Service_Response_Time
      ,sm.Commit_psec_avg
      ,sm.Commit_psec_max
      ,sm.Redo_mb_psec_avg
      ,sm.Redo_mb_psec_max
      ,sm.ELAPSED_TIME_DELTA ELAPSED_TIME,
      sm.EXECUTIONS_DELTA EXECUTIONS ,
      sm.CPU_TIME_DELTA CPU_TIME,
      sm.BUFFER_GETS_DELTA BUFFER_GETS,
      sm.DISK_READS_DELTA DISK_READS,
      sm.PARSE_CALLS_DELTA PARSE_CALLS
      ,ss.DLM_MB_psec
      ,ss.NET_MB_To_Client_psec
      ,ss.NET_MB_From_Client_psec
      ,ss.NET_MB_From_DBLink_psec
      ,ss.NET_MB_To_DBLink_psec
  FROM t2_sysmetric_summary sm, t2_sysstat ss
 WHERE sm.dbid = ss.dbid(+) AND sm.inst_id = ss.inst_id(+) AND
       sm.snap_id = ss.snap_id(+)
 ORDER BY dbid, snap_id");
                //if (!string.IsNullOrEmpty(arguments.INSTANCE_NUMBER))
                //{
                //    tmpSql.AppendFormat(" and INSTANCE_NUMBER in ('1','2')", Utils.MakeSqlQueryIn2(arguments.INSTANCE_NUMBER));
                //}
                //tmpSql.AppendFormat(" and workdate >='{0}'", arguments.StartTime);
                //tmpSql.AppendFormat(" and workdate <='{0}'", arguments.EndTime);



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
                tmpSql.Append("SELECT t.dbid,t.snap_id_min,t.workdate, t.instance_number,");
                tmpSql.Append(@"round(avg(t.cpu_util_pct),2) CPU_UTIL_PCT,
                                   round(avg(t.cpu_util_pct_max),2) CPU_UTIL_PCT_MAX,
                                   round(avg(t.cpu_util_pct),2) CPU_UTIL_PCT,
                                   round(avg(t.cpu_util_pct_max), 2) CPU_UTIL_PCT_MAX,
                                   round(avg(LOGICAL_READS_PSEC), 2) LOGICAL_READS_PSEC,
                                   round(avg(PHYSICAL_READS_PSEC), 2) PHYSICAL_READS_PSEC,
                                   round(avg(PHYSICAL_WRITES_PSEC), 2) PHYSICAL_WRITES_PSEC,
                                   round(avg(EXECS_PSEC_AVG), 2) EXECS_PSEC_AVG,
                                   round(avg(EXECS_PSEC_MAX), 2) EXECS_PSEC_MAX,
                                   round(avg(USER_CALLS_PSEC), 2) USER_CALLS_PSEC,
                                   round(avg(HARD_PARSE_CNT_PSEC), 2) HARD_PARSE_CNT_PSEC,
                                   round(avg(DB_BLOCK_CHANGES_PSEC), 2) DB_BLOCK_CHANGES_PSEC,
                                   round(avg(SQL_SERVICE_RESPONSE_TIME), 2) SQL_SERVICE_RESPONSE_TIME,
                                   round(avg(COMMIT_PSEC_AVG), 2) COMMIT_PSEC_AVG,
                                   round(avg(REDO_MB_PSEC_AVG), 2) REDO_MB_PSEC_AVG,
                                   round(avg(DLM_MB_PSEC), 2) DLM_MB_PSEC,
                                   round(avg(NET_MB_TO_CLIENT_PSEC), 2) NET_MB_TO_CLIENT_PSEC,
                                   round(avg(NET_MB_FROM_CLIENT_PSEC), 2) NET_MB_FROM_CLIENT_PSEC,
                                   round(avg(NET_MB_FROM_DBLINK_PSEC), 2) NET_MB_FROM_DBLINK_PSEC,
                                   round(avg(NET_MB_TO_DBLINK_PSEC), 2) NET_MB_TO_DBLINK_PSEC,
                                   round(avg(EXECUTIONS), 2) EXECUTIONS,
                                   round(avg(ELAPSED_TIME), 2) ELAPSED_TIME,
                                   round(avg(CPU_TIME), 2) CPU_TIME,
                                   round(avg(BUFFER_GETS), 2) BUFFER_GETS,
                                   round(avg(DISK_READS), 2) DISK_READS,
                                   round(avg(PARSE_CALL), 2) PARSE_CALL
                                ");
                tmpSql.Append(" FROM sum_workload T where 1=1 ");
                tmpSql.AppendFormat(" and DBID='{0}'", arguments.DBID);
                if (!string.IsNullOrEmpty(arguments.INSTANCE_NUMBER))
                {
                    tmpSql.AppendFormat(" and INSTANCE_NUMBER in ({0})", Utils.MakeSqlQueryIn2(arguments.INSTANCE_NUMBER));
                }                
                tmpSql.AppendFormat(" and workdate >='{0}'", arguments.StartTime);
                tmpSql.AppendFormat(" and workdate <='{0}'", arguments.EndTime);
                tmpSql.Append(" group by t.dbid, t.snap_id_min, t.workdate, t.instance_number");
                tmpSql.Append(" order by workdate");



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

        public void GetParamentRelation()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("SELECT T.* FROM tapctparamentrelation T where CONFIG_TYPE='2' ");



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
        
        public void GetWorkLoadLagestSql(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat("SELECT ROWNUM,e.* FROM ( ");
                tmpSql.AppendFormat("SELECT c.sql_id,c.{0}, c.instance_number,d.sql_text FROM (", arguments.ParamNamesString);
                tmpSql.AppendFormat("select sql_id,ROUND(avg({0}),0) {0},instance_number from (", arguments.ParamNamesString);
                tmpSql.AppendFormat("SELECT  t.snap_id, t.dbid, t.sql_id, t.{0},t.instance_number", arguments.ParamNamesString);
                tmpSql.AppendFormat("  FROM raw_dba_hist_sqlstat_{0} T ", arguments.DBName);
                tmpSql.AppendFormat("    left join raw_dba_hist_snapshot_{0} a on t.snap_id = a.snap_id and t.dbid＝a.dbid and t.instance_number=a.instance_number ", arguments.DBName);
                tmpSql.Append(" where t.snap_id in ");
                tmpSql.Append("       (SELECT T.Snap_Id ");
                tmpSql.AppendFormat("          FROM raw_dba_hist_snapshot_{0} T ", arguments.DBName);
                tmpSql.Append("         WHERE T.END_INTERVAL_TIME > ");
                tmpSql.AppendFormat("               TO_DATE('{0}', 'yyyy-MM-dd HH24:mi:ss') ", arguments.StartTime);
                tmpSql.Append("           and t.end_interval_time <= ");
                tmpSql.AppendFormat("               TO_DATE('{0}', 'yyyy-MM-dd HH24:mi:ss') ", arguments.EndTime);
                tmpSql.AppendFormat("          )) b ");
                tmpSql.AppendFormat("          where {0} is not null", arguments.ParamNamesString);
                tmpSql.AppendFormat("            group by   sql_id,instance_number order by {0} desc", arguments.ParamNamesString);
                tmpSql.AppendFormat("  ) c LEFT JOIN raw_dba_hist_sqltext_{0} d ON c.sql_id=d.sql_id", arguments.DBName);
                tmpSql.AppendFormat("                where c.instance_number in ({0}) ", Utils.MakeSqlQueryIn2(arguments.INSTANCE_NUMBER));
                tmpSql.AppendFormat("   ORDER BY c.{0} DESC) e", arguments.ParamNamesString);
                tmpSql.AppendFormat("    where ROWNUM<={0}", arguments.ClustersNumber);


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

        public void GetWorkloadNaerTwoM(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                if (arguments.ParamNamesString == "ELAPSED_TIME_DELTA" || arguments.ParamNamesString == "CPU_TIME_DELTA")
                {
                    tmpSql.AppendFormat("SELECT to_date(d.workdate,'yyyy-MM-dd') workdate,ROUND(avg(d.{0})/1000000,0) {0},d.sql_id,d.instance_number  FROM ( ", arguments.ParamNamesString);
                    tmpSql.AppendFormat(" SELECT t.sql_id, TO_CHAR(a.begin_interval_time, 'yyyy-MM-dd') workDate, T.{0},t.instance_number", arguments.ParamNamesString);
                    tmpSql.AppendFormat(" FROM raw_dba_hist_sqlstat_{0} T ", arguments.DBName);
                    tmpSql.AppendFormat("  LEFT JOIN raw_dba_hist_snapshot_{0} a ON t.snap_id = a.snap_id  and t.dbid＝a.dbid and t.instance_number=a.instance_number ", arguments.DBName);
                    tmpSql.AppendFormat(" WHERE a.begin_interval_time > TO_DATE('{0}', 'yyyy-MM-dd HH24:mi:ss')  ", arguments.StartTime);
                    tmpSql.AppendFormat("AND t.sql_id = '{0}' ", arguments.ParamType);
                    tmpSql.Append(" ORDER BY a.begin_interval_time) d ");
                    tmpSql.AppendFormat("where d.instance_number = '{0}' ", arguments.INSTANCE_NUMBER);
                    tmpSql.Append(" GROUP BY d.workdate,d.sql_id,d.instance_number ORDER BY d.workdate");
                }
                else
                {
                    tmpSql.AppendFormat("SELECT to_date(d.workdate,'yyyy-MM-dd') workdate,ROUND(avg(d.{0}),0) {0},d.sql_id,d.instance_number  FROM ( ", arguments.ParamNamesString);
                    tmpSql.AppendFormat(" SELECT t.sql_id, TO_CHAR(a.begin_interval_time, 'yyyy-MM-dd') workDate, T.{0},t.instance_number", arguments.ParamNamesString);
                    tmpSql.AppendFormat(" FROM raw_dba_hist_sqlstat_{0} T ", arguments.DBName);
                    tmpSql.AppendFormat("  LEFT JOIN raw_dba_hist_snapshot_{0} a ON t.snap_id = a.snap_id  and t.dbid＝a.dbid and t.instance_number=a.instance_number ", arguments.DBName);
                    tmpSql.AppendFormat(" WHERE a.begin_interval_time > TO_DATE('{0}', 'yyyy-MM-dd HH24:mi:ss')  ", arguments.StartTime);
                    tmpSql.AppendFormat("AND t.sql_id = '{0}' ", arguments.ParamType);
                    tmpSql.Append(" ORDER BY a.begin_interval_time) d ");
                    tmpSql.AppendFormat("where d.instance_number = '{0}' ", arguments.INSTANCE_NUMBER);
                    tmpSql.Append(" GROUP BY d.workdate,d.sql_id,d.instance_number ORDER BY d.workdate");
                }
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

        public void GetSqlTextBySqlID(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("SELECT ROWNUM, T.SQL_ID,T.SQL_TEXT FROM raw_dba_hist_sqltext_{0} T where t.sql_id IN ({1})", arguments.DBName , Utils.MakeSqlQueryIn2(arguments.WorkloadSqlParm));



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

        //public void GetWorkloadDataByParams(AwrArgsPack arguments)
        //{
        //    DBCommunicator db = new DBCommunicator();
        //    try
        //    {
        //        StringBuilder tmpSql = new StringBuilder();
        //        //TODO row 60 may have trouble.
        //        tmpSql.AppendFormat("" +
        //            "WITH\r\n" +
        //            "t1_sysmetric_summary\r\n" +
        //            "AS\r\n" +
        //            "( SELECT /*+ MATERIALIZE */\r\n" +
        //            "MIN (begin_interval_time)\r\n" +
        //            "begin_time,\r\n" +
        //            "MAX (end_interval_time)\r\n" +
        //            "end_time,\r\n" +
        //            "dbid,\r\n" +
        //            "(SELECT instance_name\r\n" +
        //            "FROM gv$instance\r\n" +
        //            "WHERE INSTANCE_NUMBER = s.instance_number)\r\n" +
        //            "dbname,\r\n" +
        //            "snap_id,\r\n" +
        //            "s.instance_number\r\n" +
        //            "AS inst_id,\r\n" +
        //            "MIN (NUM_INTERVAL)\r\n" +
        //            "NUM_INTERVAL,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (metric_name,\r\n" +
        //            "'Host CPU Utilization (%)', average,\r\n" +
        //            "0))\r\n" +
        //            "CPU_Util_pct,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (metric_name,\r\n" +
        //            "'Host CPU Utilization (%)', maxval,\r\n" +
        //            "0))\r\n" +
        //            "CPU_Util_pct_max,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (metric_name, 'Logical Reads Per Sec', average, 0))\r\n" +
        //            "Logical_Reads_psec,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (metric_name,\r\n" +
        //            "'Physical Reads Per Sec', average,\r\n" +
        //            "0))\r\n" +
        //            "Physical_Reads_psec,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (metric_name,\r\n" +
        //            "'Physical Writes Per Sec', average,\r\n" +
        //            "0))\r\n" +
        //            "Physical_Writes_psec,\r\n" +
        //            "SUM (DECODE (metric_name, 'Executions Per Sec', average, 0))\r\n" +
        //            "Execs_psec_avg,\r\n" +
        //            "SUM (DECODE (metric_name, 'Executions Per Sec', maxval, 0))\r\n" +
        //            "Execs_psec_max,\r\n" +
        //            "SUM (DECODE (metric_name, 'User Calls Per Sec', average, 0))\r\n" +
        //            "User_Calls_psec,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (metric_name,\r\n" +
        //            "'DB Block Changes Per Sec', average,\r\n" +
        //            "0))\r\n" +
        //            "DB_Block_Changes_psec,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (metric_name,\r\n" +
        //            "'SQL Service Response Time', average,\r\n" +
        //            "0))\r\n" +
        //            "SQL_Service_Response_Time,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (metric_name, 'User Commits Per Sec', average, 0))\r\n" +
        //            "User_Commits_psec,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (metric_name,\r\n" +
        //            "'Redo Generated Per Sec', average,\r\n" +
        //            "0))\r\n" +
        //            "Redo_Generated_psec,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (metric_name,\r\n" +
        //            "'Hard Parse Count Per Sec', average,\r\n" +
        //            "0))\r\n" +
        //            "Hard_Parse_Cnt_psec\r\n" +
        //            "FROM (SELECT /*+ LEADING(sn sm) USE_HASH(sn sm) USE_HASH(sm.sn sm.m sn.mn) no_merge(sm) */\r\n" +
        //            "sm.*, sn.begin_interval_time, sn.end_interval_time\r\n" +
        //            "FROM ISIA.RAW_DBA_HIST_SYSMETRIC_SUMMARY_{3} sm, -- DBA_HIST_METRIC_NAME mn,\r\n" +
        //            "ISIA.RAW_DBA_HIST_SNAPSHOT_{3} sn\r\n" +
        //            "WHERE 1 = 1\r\n" +
        //            "-- and sm.dbid = mn.dbid and sm.group_id = mn.group_id and sm.metric_id = mn.metric_id\r\n" +
        //            "AND sm.dbid = sn.dbid\r\n" +
        //            "AND sm.INSTANCE_NUMBER = sn.INSTANCE_NUMBER\r\n" +
        //            "AND sm.snap_id = sn.snap_id\r\n" +
        //            "AND sn.INSTANCE_NUMBER IN (1) --<< 조회 대상 instance number 입력\r\n" +
        //            "AND TO_CHAR (sn.BEGIN_INTERVAL_TIME, 'yyyyMMddHH24miss') BETWEEN '{1}'\r\n" +
        //            "AND '{2}' --<< 조회 기간 입력\r\n" +
        //            ")\r\n" +
        //            "s\r\n" +
        //            "WHERE 1 = 1\r\n" +
        //            "GROUP BY dbid, s.instance_number, snap_id),\r\n" +

        //            "t1_sysstat\r\n" +
        //            "AS\r\n" +
        //            "(SELECT /*+ MATERIALIZE */\r\n" +
        //            "dbid,\r\n" +
        //            "INSTANCE_NUMBER\r\n" +
        //            "AS inst_id,\r\n" +
        //            "snap_id,\r\n" +
        //            "begin_interval_time\r\n" +
        //            "AS begin_time,\r\n" +
        //            "end_interval_time\r\n" +
        //            "AS end_time,\r\n" +
        //            "(SELECT EXTRACT (\r\n" +
        //            "HOUR FROM ( END_INTERVAL_TIME\r\n" +
        //            "- BEGIN_INTERVAL_TIME))\r\n" +
        //            "* 60\r\n" +
        //            "* 60\r\n" +
        //            "+ EXTRACT (\r\n" +
        //            "MINUTE FROM ( END_INTERVAL_TIME\r\n" +
        //            "- BEGIN_INTERVAL_TIME))\r\n" +
        //            "* 60\r\n" +
        //            "+ EXTRACT (\r\n" +
        //            "SECOND FROM ( END_INTERVAL_TIME\r\n" +
        //            "- BEGIN_INTERVAL_TIME))\r\n" +
        //            "FROM ISIA.RAW_DBA_HIST_SNAPSHOT\r\n" +
        //            "WHERE dbid = s.dbid\r\n" +
        //            "AND SNAP_ID = s.SNAP_ID\r\n" +
        //            "AND INSTANCE_NUMBER = s.INSTANCE_NUMBER)\r\n" +
        //            "snap_time,\r\n" +
        //            "ROUND (\r\n" +
        //            "( NET_Cnt_Client\r\n" +
        //            "- LAG (NET_Cnt_Client, 1)\r\n" +
        //            "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
        //            "ORDER BY snap_id)))\r\n" +
        //            "NET_Cnt_Client,\r\n" +
        //            "ROUND (\r\n" +
        //            "( NET_B_To_Client\r\n" +
        //            "- LAG (NET_B_To_Client, 1)\r\n" +
        //            "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
        //            "ORDER BY snap_id)))\r\n" +
        //            "NET_B_To_Client,\r\n" +
        //            "ROUND (\r\n" +
        //            "( NET_B_From_Client\r\n" +
        //            "- LAG (NET_B_From_Client, 1)\r\n" +
        //            "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
        //            "ORDER BY snap_id)))\r\n" +
        //            "NET_B_From_Client,\r\n" +
        //            "ROUND (\r\n" +
        //            "( NET_Cnt_DBLink\r\n" +
        //            "- LAG (NET_Cnt_DBLink, 1)\r\n" +
        //            "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
        //            "ORDER BY snap_id)))\r\n" +
        //            "NET_Cnt_DBLink,\r\n" +
        //            "ROUND (\r\n" +
        //            "( NET_B_From_DBLink\r\n" +
        //            "- LAG (NET_B_From_DBLink, 1)\r\n" +
        //            "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
        //            "ORDER BY snap_id)))\r\n" +
        //            "NET_B_From_DBLink,\r\n" +
        //            "ROUND (\r\n" +
        //            "( NET_B_To_DBLink\r\n" +
        //            "- LAG (NET_B_To_DBLink, 1)\r\n" +
        //            "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
        //            "ORDER BY snap_id)))\r\n" +
        //            "NET_B_To_DBLink,\r\n" +
        //            "ROUND (\r\n" +
        //            "( gc_recv\r\n" +
        //            "- LAG (gc_recv, 1)\r\n" +
        //            "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
        //            "ORDER BY snap_id)),\r\n" +
        //            "2)\r\n" +
        //            "gc_recv,\r\n" +
        //            "ROUND (\r\n" +
        //            "( gc_send\r\n" +
        //            "- LAG (gc_send, 1)\r\n" +
        //            "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
        //            "ORDER BY snap_id)),\r\n" +
        //            "2)\r\n" +
        //            "gc_send,\r\n" +
        //            "ROUND (\r\n" +
        //            "( gcs_msg_send\r\n" +
        //            "- LAG (gcs_msg_send, 1)\r\n" +
        //            "OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
        //            "ORDER BY snap_id)),\r\n" +
        //            "2)\r\n" +
        //            "gcs_msg_send\r\n" +

        //            "FROM ( SELECT dbid,\r\n" +
        //            "snap_id,\r\n" +
        //            "INSTANCE_NUMBER,\r\n" +
        //            "MIN (begin_interval_time)\r\n" +
        //            "AS begin_interval_time,\r\n" +
        //            "MAX (end_interval_time)\r\n" +
        //            "AS end_interval_time,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (\r\n" +
        //            "stat_name,\r\n" +
        //            "'SQL*Net roundtrips to/from client', VALUE,\r\n" +
        //            "0))\r\n" +
        //            "NET_Cnt_Client,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (\r\n" +
        //            "stat_name,\r\n" +
        //            "'bytes sent via SQL*Net to client', VALUE,\r\n" +
        //            "0))\r\n" +
        //            "NET_B_To_Client,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (\r\n" +
        //            "stat_name,\r\n" +
        //            "'bytes received via SQL*Net from client', VALUE,\r\n" +
        //            "0))\r\n" +
        //            "NET_B_From_Client,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (\r\n" +
        //            "stat_name,\r\n" +
        //            "'SQL*Net roundtrips to/from dblink', VALUE,\r\n" +
        //            "0))\r\n" +
        //            "NET_Cnt_DBLink,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (\r\n" +
        //            "stat_name,\r\n" +
        //            "'bytes received via SQL*Net from dblink', VALUE,\r\n" +
        //            "0))\r\n" +
        //            "NET_B_From_DBLink,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (\r\n" +
        //            "stat_name,\r\n" +
        //            "'bytes sent via SQL*Net to dblink', VALUE,\r\n" +
        //            "0))\r\n" +
        //            "NET_B_To_DBLink,\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (stat_name,\r\n" +
        //            "'gc cr blocks received', VALUE,\r\n" +
        //            "'gc current blocks received', VALUE,\r\n" +
        //            "0))\r\n" +
        //            "gc_recv -- Global Cache blocks received\r\n" +
        //            ",\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (stat_name,\r\n" +
        //            "'gc cr blocks served', VALUE,\r\n" +
        //            "'gc current blocks served', VALUE,\r\n" +
        //            "0))\r\n" +
        //            "gc_send -- Global Cache blocks served\r\n" +
        //            ",\r\n" +
        //            "SUM (\r\n" +
        //            "DECODE (stat_name,\r\n" +
        //            "'gcs messages sent', VALUE,\r\n" +
        //            "'ges messages sent', VALUE,\r\n" +
        //            "0))\r\n" +
        //            "gcs_msg_send -- GCS/GES messages sent\r\n" +
        //            "FROM (SELECT /*+ LEADING(sn ss) USE_HASH(sn ss) USE_HASH(ss.sn ss.s ss.nm) no_merge(ss) */\r\n" +
        //            "ss.dbid,\r\n" +
        //            "ss.instance_number,\r\n" +
        //            "ss.snap_id,\r\n" +
        //            "ss.VALUE,\r\n" +
        //            "ss.stat_name,\r\n" +
        //            "sn.begin_interval_time,\r\n" +
        //            "sn.end_interval_time\r\n" +
        //            "FROM ISIA.RAW_DBA_HIST_SYSSTAT_{3} ss, -- DBA_HIST_STAT_NAME nm,\r\n" +
        //            "ISIA.RAW_DBA_HIST_SNAPSHOT_{3} sn\r\n" +
        //            "WHERE 1 = 1\r\n" +
        //            "-- and ss.dbid = nm.dbid and ss.stat_id = nm.stat_id\r\n" +
        //            "AND ss.dbid = sn.dbid\r\n" +
        //            "AND ss.INSTANCE_NUMBER = sn.INSTANCE_NUMBER\r\n" +
        //            "AND ss.snap_id = sn.snap_id\r\n" +
        //            "AND sn.INSTANCE_NUMBER IN (1) --<< 조회 대상 instance number 입력))\r\n" +
        //            "AND TO_CHAR (sn.BEGIN_INTERVAL_TIME,\r\n" +
        //            "'yyyyMMddHH24miss') BETWEEN '{1}'\r\n" +
        //            "AND '{2}' --<< 조회 기간 입력\r\n" +
        //            ")\r\n" +
        //            "WHERE 1 = 1\r\n" +
        //            "GROUP BY dbid, INSTANCE_NUMBER, snap_id) s),\r\n" +
        //            "t2_sysmetric_summary\r\n" +
        //            "AS\r\n" +
        //            "( SELECT dbid,\r\n" +
        //            "inst_id,\r\n" +
        //            "MIN (snap_id)\r\n" +
        //            "snap_id_min,\r\n" +
        //            "TO_CHAR (BEGIN_TIME, '{0}')\r\n" +
        //            "workdate,\r\n" +
        //            "MIN (BEGIN_TIME)\r\n" +
        //            "BEGIN_TIME,\r\n" +
        //            "MAX (END_TIME)\r\n" +
        //            "END_TIME,\r\n" +
        //            "ROUND (AVG (CPU_Util_pct), 2)\r\n" +
        //            "CPU_Util_pct,\r\n" +
        //            "ROUND (MAX (CPU_Util_pct_max), 2)\r\n" +
        //            "CPU_Util_pct_max,\r\n" +
        //            "ROUND (AVG (LOGICAL_READS_PSEC))\r\n" +
        //            "LOGICAL_READS_PSEC,\r\n" +
        //            "ROUND (AVG (Physical_Reads_psec))\r\n" +
        //            "PHYSICAL_READS_PSEC,\r\n" +
        //            "ROUND (AVG (Physical_Writes_psec))\r\n" +
        //            "Physical_Writes_psec,\r\n" +
        //            "ROUND (AVG (Execs_psec_avg))\r\n" +
        //            "Execs_psec_avg,\r\n" +
        //            "ROUND (MAX (Execs_psec_max))\r\n" +
        //            "Execs_psec_max,\r\n" +
        //            "ROUND (AVG (USER_CALLS_PSEC))\r\n" +
        //            "USER_CALLS_PSEC,\r\n" +
        //            "ROUND (AVG (DB_BLOCK_CHANGES_PSEC))\r\n" +
        //            "DB_BLOCK_CHANGES_PSEC,\r\n" +
        //            "ROUND (AVG (SQL_Service_Response_Time), 4)\r\n" +
        //            "SQL_Service_Response_Time,\r\n" +
        //            "ROUND (AVG (User_Commits_psec), 2)\r\n" +
        //            "Commit_psec_avg,\r\n" +
        //            "ROUND (AVG (Redo_Generated_psec / 1024 / 1024), 2)\r\n" +
        //            "Redo_mb_psec_avg,\r\n" +
        //            "ROUND (AVG (Hard_Parse_Cnt_psec), 2)\r\n" +
        //            "Hard_Parse_Cnt_psec\r\n" +
        //            "FROM t1_sysmetric_summary s\r\n" +
        //            "WHERE 1 = 1\r\n" +
        //            "GROUP BY dbid, inst_id, TO_CHAR (BEGIN_TIME, '{0}')),\r\n" +

        //            "t2_sysstat\r\n" +
        //            "AS\r\n" +
        //            "( SELECT dbid,\r\n" +
        //            "inst_id,\r\n" +
        //            "MIN (snap_id)\r\n" +
        //            "AS snap_id_min,\r\n" +
        //            "TO_CHAR (BEGIN_TIME, '{0}')\r\n" +
        //            "workdate,\r\n" +
        //            "MIN (BEGIN_TIME)\r\n" +
        //            "BEGINE_TIME,\r\n" +
        //            "MAX (END_TIME)\r\n" +
        //            "END_TIME,\r\n" +
        //            "ROUND (\r\n" +
        //            "AVG (\r\n" +
        //            "((gc_recv + gc_send) * 8192 + gcs_msg_send * 200)\r\n" +
        //            "/ 1024\r\n" +
        //            "/ 1024\r\n" +
        //            "/ SNAP_TIME),\r\n" +
        //            "3)\r\n" +
        //            "DLM_MB_psec,\r\n" +
        //            "ROUND (AVG (NET_B_To_Client / 1024 / 1024 / SNAP_TIME), 3)\r\n" +
        //            "NET_MB_To_Client_psec,\r\n" +
        //            "ROUND (AVG (NET_B_From_Client / 1024 / 1024 / SNAP_TIME), 3)\r\n" +
        //            "NET_MB_From_Client_psec,\r\n" +
        //            "ROUND (AVG (NET_B_From_DBLink / 1024 / 1024 / SNAP_TIME), 3)\r\n" +
        //            "NET_MB_From_DBLink_psec,\r\n" +
        //            "ROUND (AVG (NET_B_To_DBLink / 1024 / 1024 / SNAP_TIME), 3)\r\n" +
        //            "NET_MB_To_DBLink_psec\r\n" +
        //            "FROM t1_sysstat s\r\n" +
        //            "WHERE 1 = 1\r\n" +
        //            "GROUP BY dbid, inst_id, TO_CHAR (BEGIN_TIME, '{0}'))\r\n" +

        //            "SELECT sm.dbid,\r\n" +
        //            "sm.inst_id,\r\n" +
        //            "sm.snap_id_min,\r\n" +
        //            "sm.workdate AS workdate,\r\n" +
        //            "sm.begin_time,\r\n" +
        //            "sm.end_time,\r\n" +
        //            "sm.CPU_Util_pct,\r\n" +
        //            "sm.CPU_Util_pct_max,\r\n" +
        //            "sm.LOGICAL_READS_PSEC,\r\n" +
        //            "sm.PHYSICAL_READS_PSEC,\r\n" +
        //            "sm.Physical_Writes_psec,\r\n" +
        //            "sm.Execs_psec_avg,\r\n" +
        //            "sm.Execs_psec_max,\r\n" +
        //            "sm.USER_CALLS_PSEC,\r\n" +
        //            "sm.Hard_Parse_Cnt_psec,\r\n" +
        //            "sm.DB_BLOCK_CHANGES_PSEC,\r\n" +
        //            "sm.SQL_Service_Response_Time,\r\n" +
        //            "sm.Commit_psec_avg,\r\n" +
        //            "sm.Redo_mb_psec_avg,\r\n" +
        //            "ss.DLM_MB_psec,\r\n" +
        //            "ss.NET_MB_To_Client_psec,\r\n" +
        //            "ss.NET_MB_From_Client_psec,\r\n" +
        //            "ss.NET_MB_From_DBLink_psec,\r\n" +
        //            "ss.NET_MB_To_DBLink_psec\r\n" +
        //            "FROM t2_sysmetric_summary sm, t2_sysstat ss\r\n" +
        //            "WHERE sm.dbid = ss.dbid(+)\r\n" +
        //            "AND sm.inst_id = ss.inst_id(+)\r\n" +
        //            "AND sm.workdate = ss.workdate(+)\r\n" +
        //            "ORDER BY dbid, workdate\r\n"
        //             , arguments.GroupingDateFormat, arguments.StartTime, arguments.EndTime,arguments.DBName
        //            );
        //        RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
        //               tmpSql.ToString(), false);

        //        this.ExecutingValue = db.Select(tmpSql.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
        //               string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
        //        throw ex;
        //    }
        //}



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
