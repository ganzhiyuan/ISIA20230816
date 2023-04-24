﻿using ISIA.COMMON;
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

namespace ISIA.BIZ.ANALYSIS
{
    public class SQLAnalysisChart : TAP.Remoting.Server.Biz.BizComponentBase
    {



        public void GetSnap(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@" SELECT t.snap_id,a.end_interval_time,T.DBID,
                                sum(t.FETCHES_TOTAL) FETCHES_TOTAL , sum(t.END_OF_FETCH_COUNT_TOTAL) END_OF_FETCH_COUNT_TOTAL,sum(t.SORTS_TOTAL) SORTS_TOTAL,
                                sum(t.EXECUTIONS_TOTAL) EXECUTIONS_TOTAL,sum(t.PX_SERVERS_EXECS_TOTAL) PX_SERVERS_EXECS_TOTAL,sum(t.LOADS_TOTAL) LOADS_TOTAL,
                                sum(t.INVALIDATIONS_TOTAL) INVALIDATIONS_TOTAL,sum(t.PARSE_CALLS_TOTAL) PARSE_CALLS_TOTAL,sum(t.DISK_READS_TOTAL) DISK_READS_TOTAL,
                                sum(t.BUFFER_GETS_TOTAL) BUFFER_GETS_TOTAL,sum(t.ROWS_PROCESSED_TOTAL) ROWS_PROCESSED_TOTAL,sum(t.CPU_TIME_TOTAL) CPU_TIME_TOTAL,
                                sum(t.ELAPSED_TIME_TOTAL) ELAPSED_TIME_TOTAL,
                                sum(t.IOWAIT_TOTAL) IOWAIT_TOTAL,
                                sum(t.CLWAIT_TOTAL) CLWAIT_TOTAL,
                                sum(t.APWAIT_TOTAL) APWAIT_TOTAL,
                                sum(t.CCWAIT_TOTAL) CCWAIT_TOTAL,
                                sum(t.DIRECT_WRITES_TOTAL) DIRECT_WRITES_TOTAL,
                                sum(t.PLSEXEC_TIME_TOTAL) PLSEXEC_TIME_TOTAL,
                                sum(t.JAVEXEC_TIME_TOTAL) JAVEXEC_TIME_TOTAL,
                                sum(t.IO_OFFLOAD_ELIG_BYTES_TOTAL) IO_OFFLOAD_ELIG_BYTES_TOTAL,
                                sum(t.IO_INTERCONNECT_BYTES_TOTAL) IO_INTERCONNECT_BYTES_TOTAL,
                                sum(t.PHYSICAL_READ_REQUESTS_TOTAL) PHYSICAL_READ_REQUESTS_TOTAL,
                                sum(t.PHYSICAL_READ_BYTES_TOTAL) PHYSICAL_READ_BYTES_TOTAL,
                                sum(t.PHYSICAL_WRITE_REQUESTS_TOTAL) PHYSICAL_WRITE_REQUESTS_TOTAL,
                                sum(t.PHYSICAL_WRITE_BYTES_TOTAL) PHYSICAL_WRITE_BYTES_TOTAL,
                                sum(t.OPTIMIZED_PHYSICAL_READS_TOTAL) OPTIMIZED_PHYSICAL_READS_TOTAL,
                                sum(t.CELL_UNCOMPRESSED_BYTES_TOTAL) CELL_UNCOMPRESSED_BYTES_TOTAL,
                                sum(t.IO_OFFLOAD_RETURN_BYTES_TOTAL) IO_OFFLOAD_RETURN_BYTES_TOTAL");
                                /*FROM raw_dba_hist_sqlstat_isfa T  
                                left join raw_dba_hist_snapshot_isfa a on t.snap_id=a.snap_id
                                where t.snap_id in
                                (SELECT T.Snap_Id
                                      FROM raw_dba_hist_snapshot_isfa T");
                                      WHERE T.END_INTERVAL_TIME >=
                                      TO_DATE('{0}', 'yyyy-MM-dd HH24:mi:ss')
                                      and t.end_interval_time <=
                                      TO_DATE('{1}', 'yyyy-MM-dd HH24:mi:ss')) 
                                      and T.dbid in ('{2}')              
                                 group by t.snap_id ,a.end_interval_time,T.DBID order by a.end_interval_time ");*/

                tmpSql.AppendFormat(@" FROM raw_dba_hist_sqlstat_{0} T  
                                left join raw_dba_hist_snapshot_{0} a on t.snap_id = a.snap_id
                                where t.snap_id in
                                (SELECT T.Snap_Id
                                      FROM raw_dba_hist_snapshot_{0} T", arguments.DbName);

                tmpSql.AppendFormat(" WHERE T.END_INTERVAL_TIME >= TO_DATE('{0}', 'yyyy-MM-dd HH24:mi:ss')", arguments.StartTimeKey);

                tmpSql.AppendFormat(" and t.end_interval_time <= TO_DATE('{0}', 'yyyy-MM-dd HH24:mi:ss'))", arguments.EndTimeKey);

                tmpSql.AppendFormat(" and T.dbid = {0}", arguments.DbId);

                tmpSql.AppendFormat(" and T.INSTANCE_NUMBER in ({0})", Utils.MakeSqlQueryIn2(arguments.InstanceNumber));

                tmpSql.Append(" group by t.snap_id ,a.end_interval_time,T.DBID order by a.end_interval_time ");

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
