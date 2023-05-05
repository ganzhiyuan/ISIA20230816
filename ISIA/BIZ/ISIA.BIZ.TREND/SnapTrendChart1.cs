using ISIA.COMMON;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TAP;
using TAP.Data.DataBase.Communicators;
using TAP.Remoting;

namespace ISIA.BIZ.TREND
{
    public class SnapTrendChart1 : TAP.Remoting.Server.Biz.BizComponentBase
    {
        public void GetSnap(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(" SELECT a.end_interval_time, t.snap_id,t.dbid,t.sql_id, T.{0}", arguments.ParameterName);
                tmpSql.AppendFormat(" FROM raw_dba_hist_sqlstat_{0} T left join raw_dba_hist_snapshot_isfa a on t.snap_id=a.snap_id ", arguments.DbName);
                tmpSql.Append(" where t.snap_id in");
                tmpSql.Append(" (SELECT T.Snap_Id");
                tmpSql.Append(" FROM raw_dba_hist_snapshot_isfa T");
                tmpSql.Append(" WHERE T.END_INTERVAL_TIME >=");
                tmpSql.AppendFormat(" TO_DATE('{0}', 'yyyy-MM-dd HH24:mi:ss')", arguments.StartTimeKey);
                tmpSql.Append(" and t.end_interval_time <=");
                tmpSql.AppendFormat(" TO_DATE('{0}', 'yyyy-MM-dd HH24:mi:ss'))", arguments.EndTimeKey);



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

        public void GetSqlstatPara(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("SELECT T.* FROM raw_dba_hist_sqlstat_isfa T where t.snap_id='{0}' and t.sql_id='{1}'", arguments.ParameterName, arguments.SqlId);


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

        public void GetSqlstatByUnit(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("SELECT a.end_interval_time,t.sql_id,{0} as typNum ", Convert.ToDecimal(arguments.ParameterType));
                tmpSql.AppendFormat("  FROM raw_dba_hist_sqlstat_{0} T left join raw_dba_hist_snapshot_{0} a on t.snap_id=a.snap_id  ", arguments.DbName);
                tmpSql.Append(" where t.snap_id in ");
                tmpSql.Append(" (SELECT T.Snap_Id ");
                tmpSql.AppendFormat(" FROM raw_dba_hist_snapshot_{0} T ", arguments.DbName);
                tmpSql.Append(" WHERE T.END_INTERVAL_TIME > ");
                tmpSql.AppendFormat(" TO_DATE('{0}', 'yyyy-MM-dd HH24:mi:ss') ", arguments.StartTimeKey);
                tmpSql.Append(" and t.end_interval_time <= ");
                tmpSql.AppendFormat(" TO_DATE('{0}', 'yyyy-MM-dd HH24:mi:ss'))", arguments.EndTimeKey);
                //arguments.ParameterType 

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

        public void GetSqlStatByWeek(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("SELECT  sql_id,  COUNT(*) AS {0} ",arguments.ParameterName);
                tmpSql.Append(@"FROM(
                        SELECT DISTINCT  workDate, sql_id
                        FROM
                        (SELECT SUBSTR(a.end_interval_time,0,10) workDate, MAX(t.sql_id) sql_id ");
                 tmpSql.AppendFormat("     FROM raw_dba_hist_sqlstat_{0} T ", arguments.DbName);
                tmpSql.AppendFormat("     LEFT JOIN raw_dba_hist_snapshot_{0} a ", arguments.DbName);
                tmpSql.AppendFormat(@"   ON t.snap_id = a.snap_id and t.instance_number=a.instance_number and t.dbid=a.dbid
                     WHERE t.snap_id IN
                           (SELECT T.Snap_Id ");
                tmpSql.AppendFormat(" FROM raw_dba_hist_snapshot_{0} T )",arguments.DbName);
                tmpSql.AppendFormat(" AND a.end_interval_time>TO_DATE('{0}', 'yyyy-MM-dd') ",arguments.StartTimeKey);
                tmpSql.AppendFormat("          AND a.end_interval_time <= TO_DATE('{0}', 'yyyy-MM-dd')",arguments.EndTimeKey);
                tmpSql.AppendFormat("  GROUP BY a.end_interval_time, t.sql_id  ORDER BY sql_id)) ");
                tmpSql.AppendFormat("  WHERE workDate >= TRUNC(TO_DATE('{0}', 'yyyy-MM-dd')) - 7 ", arguments.EndTimeKey);
                tmpSql.AppendFormat("  GROUP BY sql_id  ORDER BY {0} DESC", arguments.ParameterName);
                //arguments.ParameterType 

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

        public void GetSqlstatModels(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("SELECT b.end_interval_time, a.command_type,T.* FROM raw_dba_hist_sqlstat_isfa T ");
                tmpSql.AppendFormat(" left join raw_dba_hist_sqltext_{0} a  ", arguments.DbName);
                tmpSql.Append(" on t.sql_id=a.sql_id and t.dbid=a.dbid  ");
                tmpSql.AppendFormat(" left join raw_dba_hist_snapshot_isfa b on t.snap_id=b.snap_id ");
                tmpSql.AppendFormat(" where 1=1 and b.end_interval_time>to_date('{0}','yyyy-MM-dd HH24:mi:ss') ", arguments.StartTimeKey);
                tmpSql.AppendFormat(" and    b.end_interval_time<=to_date('{0}','yyyy-MM-dd HH24:mi:ss') ", arguments.EndTimeKey);
                //if (!string.IsNullOrEmpty(arguments.CommandType))
                //{
                //    tmpSql.AppendFormat(@" and a.command_type in ({0})", Utils.MakeSqlQueryIn(arguments.CommandType,','));
                //}
                if (!string.IsNullOrEmpty(arguments.CommandName))
                {
                    tmpSql.AppendFormat(@" and t.module in ({0}) ", Utils.MakeSqlQueryIn(arguments.CommandName, ','));
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

        public void GetSqlStatModuleAll(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("select w.module, sql_id,sum(FETCHES_TOTAL) FETCHES_TOTAL,sum(END_OF_FETCH_COUNT_TOTAL) END_OF_FETCH_COUNT_TOTAL ,sum(SORTS_TOTAL) SORTS_TOTAL, ");
                tmpSql.Append("sum(EXECUTIONS_TOTAL) EXECUTIONS_TOTAL,sum(PX_SERVERS_EXECS_TOTAL) PX_SERVERS_EXECS_TOTAL,sum(LOADS_TOTAL) LOADS_TOTAL, ");
                tmpSql.Append("sum(INVALIDATIONS_TOTAL) INVALIDATIONS_TOTAL,sum(PARSE_CALLS_TOTAL) PARSE_CALLS_TOTAL, sum(DISK_READS_TOTAL) DISK_READS_TOTAL, ");
                tmpSql.Append("sum(BUFFER_GETS_TOTAL) BUFFER_GETS_TOTAL,sum(ROWS_PROCESSED_TOTAL) ROWS_PROCESSED_TOTAL,sum(CPU_TIME_TOTAL) CPU_TIME_TOTAL ");
                tmpSql.Append(", sum(ELAPSED_TIME_TOTAL) ELAPSED_TIME_TOTAL,sum(IOWAIT_TOTAL) IOWAIT_TOTAL,sum(CLWAIT_TOTAL) CLWAIT_TOTAL, ");
                tmpSql.Append("sum(APWAIT_TOTAL) APWAIT_TOTAL,sum(CCWAIT_TOTAL) CCWAIT_TOTAL,sum(DIRECT_WRITES_TOTAL) DIRECT_WRITES_TOTAL ");
                tmpSql.Append(", sum(PLSEXEC_TIME_TOTAL) PLSEXEC_TIME_TOTAL,sum(JAVEXEC_TIME_TOTAL) JAVEXEC_TIME_TOTAL,sum(IO_OFFLOAD_ELIG_BYTES_TOTAL) IO_OFFLOAD_ELIG_BYTES_TOTAL ");
                tmpSql.Append(", sum(IO_INTERCONNECT_BYTES_TOTAL) IO_INTERCONNECT_BYTES_TOTAL,sum(PHYSICAL_READ_REQUESTS_TOTAL) PHYSICAL_READ_REQUESTS_TOTAL, ");
                tmpSql.Append("sum(PHYSICAL_READ_BYTES_TOTAL) PHYSICAL_READ_BYTES_TOTAL,sum(PHYSICAL_WRITE_REQUESTS_TOTAL) PHYSICAL_WRITE_REQUESTS_TOTAL ");
                tmpSql.Append(", sum(PHYSICAL_WRITE_BYTES_TOTAL) PHYSICAL_WRITE_BYTES_TOTAL,sum(OPTIMIZED_PHYSICAL_READS_TOTAL) OPTIMIZED_PHYSICAL_READS_TOTAL ");
                tmpSql.Append(", sum(CELL_UNCOMPRESSED_BYTES_TOTAL) CELL_UNCOMPRESSED_BYTES_TOTAL,sum(IO_OFFLOAD_RETURN_BYTES_TOTAL) IO_OFFLOAD_RETURN_BYTES_TOTAL ");

                tmpSql.Append("  from  ");

                tmpSql.Append("(SELECT b.end_interval_time end_interval_time, a.command_type command_type, t.snap_id, t.dbid, t.sql_id, "); 
                tmpSql.Append("t.module, sum(t.FETCHES_TOTAL) FETCHES_TOTAL, sum(END_OF_FETCH_COUNT_TOTAL) END_OF_FETCH_COUNT_TOTAL, sum(SORTS_TOTAL) SORTS_TOTAL, ");
                tmpSql.Append("sum(EXECUTIONS_TOTAL) EXECUTIONS_TOTAL, sum(PX_SERVERS_EXECS_TOTAL) PX_SERVERS_EXECS_TOTAL, sum(LOADS_TOTAL) LOADS_TOTAL, ");
                tmpSql.Append("sum(INVALIDATIONS_TOTAL) INVALIDATIONS_TOTAL, sum(PARSE_CALLS_TOTAL) PARSE_CALLS_TOTAL, sum(DISK_READS_TOTAL) DISK_READS_TOTAL, ");
                tmpSql.Append("sum(BUFFER_GETS_TOTAL) BUFFER_GETS_TOTAL, sum(ROWS_PROCESSED_TOTAL) ROWS_PROCESSED_TOTAL, sum(CPU_TIME_TOTAL) CPU_TIME_TOTAL ");
                tmpSql.Append(", sum(ELAPSED_TIME_TOTAL) ELAPSED_TIME_TOTAL, sum(IOWAIT_TOTAL) IOWAIT_TOTAL, sum(CLWAIT_TOTAL) CLWAIT_TOTAL, ");
                tmpSql.Append("sum(APWAIT_TOTAL) APWAIT_TOTAL, sum(CCWAIT_TOTAL) CCWAIT_TOTAL, sum(DIRECT_WRITES_TOTAL) DIRECT_WRITES_TOTAL ");
                tmpSql.Append(", sum(PLSEXEC_TIME_TOTAL) PLSEXEC_TIME_TOTAL, sum(JAVEXEC_TIME_TOTAL) JAVEXEC_TIME_TOTAL, sum(IO_OFFLOAD_ELIG_BYTES_TOTAL) IO_OFFLOAD_ELIG_BYTES_TOTAL ");
                tmpSql.Append(", sum(IO_INTERCONNECT_BYTES_TOTAL) IO_INTERCONNECT_BYTES_TOTAL, sum(PHYSICAL_READ_REQUESTS_TOTAL) PHYSICAL_READ_REQUESTS_TOTAL, ");
                tmpSql.Append("sum(PHYSICAL_READ_BYTES_TOTAL) PHYSICAL_READ_BYTES_TOTAL, sum(PHYSICAL_WRITE_REQUESTS_TOTAL) PHYSICAL_WRITE_REQUESTS_TOTAL ");
                tmpSql.Append(", sum(PHYSICAL_WRITE_BYTES_TOTAL) PHYSICAL_WRITE_BYTES_TOTAL, sum(OPTIMIZED_PHYSICAL_READS_TOTAL) OPTIMIZED_PHYSICAL_READS_TOTAL ");
                tmpSql.Append(", sum(CELL_UNCOMPRESSED_BYTES_TOTAL) CELL_UNCOMPRESSED_BYTES_TOTAL, sum(IO_OFFLOAD_RETURN_BYTES_TOTAL) IO_OFFLOAD_RETURN_BYTES_TOTAL ");
                tmpSql.AppendFormat("FROM raw_dba_hist_sqlstat_{0} T ", arguments.DbName);
                tmpSql.AppendFormat("left join raw_dba_hist_sqltext_{0} a   on t.sql_id = a.sql_id and t.dbid = a.dbid  ", arguments.DbName);
                tmpSql.AppendFormat("left join raw_dba_hist_snapshot_{0} b on t.snap_id = b.snap_id ", arguments.DbName);
                tmpSql.AppendFormat("where  b.end_interval_time > to_date('{0}', 'yyyy-MM-dd HH24:mi:ss')", arguments.StartTimeKey); 
                tmpSql.AppendFormat("  and b.end_interval_time <= to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') ", arguments.EndTimeKey);


                tmpSql.AppendFormat("  AND T.DBID IN ('{0}')   ", arguments.DbId);
                tmpSql.AppendFormat("  AND T.INSTANCE_NUMBER = {0}  ", arguments.InstanceNumber);



                tmpSql.Append("group by b.end_interval_time, a.command_type, t.snap_id, t.dbid, t.sql_id, t.module) w ");
                tmpSql.AppendFormat("    where 1=1 ");
                if (!string.IsNullOrEmpty(arguments.CommandName))
                    tmpSql.AppendFormat(" and   w.module in ({0}) ", Utils.MakeSqlQueryIn(arguments.CommandName,','));
                if (!string.IsNullOrEmpty(arguments.CommandName) && !string.IsNullOrEmpty(arguments.ChartUsed))
                    tmpSql.Append(" or   w.module is null ");
                if (string.IsNullOrEmpty(arguments.CommandName) && !string.IsNullOrEmpty(arguments.ChartUsed))
                    tmpSql.Append(" and w.module is null ");
                tmpSql.Append("group by sql_id,w.module order by module ");

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
    }
}
