﻿using ISIA.INTERFACE.ARGUMENTSPACK;
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
    public class SnapTrendChart : TAP.Remoting.Server.Biz.BizComponentBase
    {
        public void GetSnap()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@" SELECT t.snap_id,a.end_interval_time,
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
                                sum(t.IO_OFFLOAD_RETURN_BYTES_TOTAL) IO_OFFLOAD_RETURN_BYTES_TOTAL
                                FROM raw_dba_hist_sqlstat_isfa T  
                                left join raw_dba_hist_snapshot_isfa a on t.snap_id=a.snap_id
                                where t.snap_id in
                                (SELECT T.Snap_Id
                                      FROM raw_dba_hist_snapshot_isfa T
                                      WHERE T.END_INTERVAL_TIME >=
                                      TO_DATE('2023-03-09 00:00:00', 'yyyy-MM-dd HH24:mi:ss')
                                      and t.end_interval_time <=
                                      TO_DATE('2023-03-10 00:00:00', 'yyyy-MM-dd HH24:mi:ss'))               
                                 group by t.snap_id ,a.end_interval_time order by a.end_interval_time ");


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

        public void GetSqlstatPara(EquipmentArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"SELECT T.* FROM raw_dba_hist_sqlstat_isfa T where t.snap_id='{0}' and t.sql_id='{1}'",  arguments.ParameterName, arguments.MainEqp);


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
