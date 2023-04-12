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
    public class PerformaceEvaluationTrend : TAP.Remoting.Server.Biz.BizComponentBase
    {
        public void GetLoadSQL(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"with load_v as (select sy.stat_name, sy.snap_id, sn.end_interval_time, nvl(sy.value, 0) - nvl(lag(sy.value) over(partition by sy.stat_name order by sy.snap_id), 0) delta_val,
                         case
                           when(extract(day from
                                        sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 86400 + extract(hour from
                                         sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 3600 + extract(minute from
                                         sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 60 + extract(second from
                                         sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id))) > 0 then round((nvl(sy.value, 0) - nvl(lag(sy.value)
                       over(partition by sy.stat_name order by sy.snap_id),
                       0)) /  (extract(day from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 86400 + extract(hour from
                           sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 3600 + extract(minute from
                           sn.end_interval_time -lag(sn.end_interval_time)
                           over(partition by sy.stat_name order by sy.snap_id)) * 60 +
                  extract(second from
                           sn.end_interval_time -lag(sn.end_interval_time)
                           over(partition by sy.stat_name order by sy.snap_id))),
                                  2)
                           else
                                    null
                         end delta,
                         nvl(wa.wait_cnt, 0) -
                         nvl(lag(wa.wait_cnt) over(order by wa.snap_id), 0) delta_wa,
                         nvl(libcache.pins, 0) -
                         nvl(lag(libcache.pins) over(order by libcache.snap_id), 0) delta_pins,
                         nvl(libcache.pinhits, 0) -
                         nvl(lag(libcache.pinhits) over(order by libcache.snap_id), 0) delta_pinhits,
                         nvl(latch.gets, 0) -
                         nvl(lag(latch.gets) over(order by latch.snap_id), 0) delta_gets,
                         nvl(latch.misses, 0) -
                         nvl(lag(latch.misses) over(order by latch.snap_id), 0) delta_misses
                    from raw_dba_hist_sysstat_isfa sy,
                         dba_hist_snapshot sn,
                         (select snap_id,
                                 instance_number,
                                 dbid,
                                 sum(nvl(wait_count, 0)) wait_cnt
                            from raw_dba_hist_waitstat_isfa
                           group by snap_id, instance_number, dbid) wa,
                         (select snap_id,
                                 instance_number,
                                 dbid,
                                 sum(nvl(pins, 0)) pins,
                                 sum(nvl(pinhits, 0)) pinhits
                            from raw_dba_hist_librarycache_isfa
                           group by snap_id, instance_number, dbid) libcache,
                         (select snap_id,
                                 instance_number,
                                 dbid,
                                 sum(nvl(gets, 0)) gets,
                                 sum(nvl(misses, 0)) misses
                            from raw_dba_hist_latch_ISFA
                           group by snap_id, instance_number, dbid) latch
                   where sy.snap_id = sn.snap_id
                     and sy.instance_number = sn.instance_number
                     and sy.dbid = sn.dbid
                     and wa.snap_id = sn.snap_id
                     and wa.instance_number = sn.instance_number
                     and wa.dbid = sn.dbid
                     and libcache.snap_id = sn.snap_id
                     and libcache.instance_number = sn.instance_number
                     and libcache.dbid = sn.dbid
                     and latch.snap_id = sn.snap_id
                     and latch.instance_number = sn.instance_number
                     and latch.dbid = sn.dbid
                     and sy.stat_name in ('redo size',
                                          'session logical reads',
                                          'db block changes',
                                          'physical reads',
                                          'physical reads direct',
                                          'physical writes',
                                          'physical writes direct',
                                          'user calls',
                                          'parse count (total)',
                                          'parse count (hard)',
                                          'sorts (memory)',
                                          'sorts (disk)',
                                          'logons cumulative',
                                          'execute count',
                                          'user commits',
                                          'user rollbacks',
                                          'consistent gets from cache',
                                          'db block gets from cache',
                                          'redo log space requests',
                                          'redo entries',
                                          'physical reads cache',
                                          'parse time cpu',
                                          'parse time elapsed',
                                          'CPU used by this session')");
                tmpSql.AppendFormat("and sn.snap_id between {0} and {1}", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat("     and sn.dbid = {0}", arguments.DbId);
                tmpSql.AppendFormat("      and sn.instance_number = {0})", arguments.InstanceNumber);
                tmpSql.Append(@"select snap_id,
                       to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",
                       max(decode(stat_name, 'redo size', delta, 0)) \""Redo size\"",
                       max(decode(stat_name, 'session logical reads', delta, 0)) \""Logical reads\"",
                       max(decode(stat_name, 'db block changes', delta, 0)) \""Block changes\"",
                       max(decode(stat_name, 'physical reads', delta, 0)) \""Physical reads\"",
                       max(decode(stat_name, 'physical reads direct', delta, 0)) \""Physical reads direct\"",
                       max(decode(stat_name, 'physical writes', delta, 0)) \""Physical writes\"",
                       max(decode(stat_name, 'physical writes direct', delta, 0)) \""Physical writes direct\"",
                       max(decode(stat_name, 'user calls', delta, 0)) \""User calls\"",
                       max(decode(stat_name, 'parse count (total)', delta, 0)) \""Parses(total)\"",
                       max(decode(stat_name, 'parse count (hard)', delta, 0)) \""Parses(hard)\"",
                       max(decode(stat_name, 'sorts (memory)', delta, 0)) \""Sorts (memory)\"",
                       max(decode(stat_name, 'sorts (disk)', delta, 0)) \""Sorts (disk)\"",
                       max(decode(stat_name, 'logons cumulative', delta, 0)) \""Logons\"",
                       max(decode(stat_name, 'execute count', delta, 0)) \""Executes\"",
                       max(decode(stat_name, 'user commits', delta, 0)) +
                       max(decode(stat_name, 'user rollbacks', delta, 0)) \""Transactions\"",
                       decode(max(decode(stat_name,
                                         'consistent gets from cache',
                                         delta_val,
                                         0)) + max(decode(stat_name,
                                                          'db block gets from cache',
                                                          delta_val,
                                                          0)),
                              0,
                              0,
                              round((1 -
                                    (max(delta_wa) / (max(decode(stat_name,
                                                                  'consistent gets from cache',
                                                                  delta_val,
                                                                  0)) +
                                    max(decode(stat_name,
                                                                  'db block gets from cache',
                                                                  delta_val,
                                                                  0))))),
                                    2)) \""Buffer Nowait %\"",
                       decode(max(decode(stat_name, 'redo entries', delta_val, 0)),
                              0,
                              0,
                              round((1 -
                                    (max(decode(stat_name,
                                                 'redo log space requests',
                                                 delta_val,
                                                 0)) /
                                    max(decode(stat_name, 'redo entries', delta_val, 0)))),
                                    2)) \""Redo NoWait %\"",
                       decode(max(decode(stat_name,
                                         'consistent gets from cache',
                                         delta_val,
                                         0)) + max(decode(stat_name,
                                                          'db block gets from cache',
                                                          delta_val,
                                                          0)),
                              0,
                              0,
                              round((1 - (max(decode(stat_name,
                                                     'physical reads cache',
                                                     delta_val,
                                                     0)) /
                                    (max(decode(stat_name,
                                                      'consistent gets from cache',
                                                      delta_val,
                                                      0)) +
                                    max(decode(stat_name,
                                                      'db block gets from cache',
                                                      delta_val,
                                                      0))))),
                                    2)) \""Buffer Hits %\"",
                       decode(max(decode(stat_name, 'sorts (memory)', delta_val, 0)) +
                              max(decode(stat_name, 'sorts (disk)', delta_val, 0)),
                              0,
                              0,
                              round((max(decode(stat_name, 'sorts (memory)', delta_val, 0)) /
                                    (max(decode(stat_name, 'sorts (memory)', delta_val, 0)) +
                                    max(decode(stat_name, 'sorts (disk)', delta_val, 0)))),
                                    2)) \""In -memory Sort %\"",
                       decode(max(delta_pins),
                              0,
                              0,
                              round((max(delta_pinhits) / max(delta_pins)), 2)) \""Library Hit %\"",
                       decode(max(decode(stat_name, 'parse count (total)', delta_val, 0)),
                              0,
                              0,
                              round((1 -
                                    (max(decode(stat_name,
                                                 'parse count (hard)',
                                                 delta_val,
                                                 0)) / max(decode(stat_name,
                                                                    'parse count (total)',
                                                                    delta_val,
                                                                    0)))),
                                    2)) \""Soft Parse %\"",
                       decode(max(decode(stat_name, 'execute count', delta_val, 0)),
                              0,
                              0,
                              round((1 -
                                    (max(decode(stat_name,
                                                 'parse count (total)',
                                                 delta_val,
                                                 0)) /
                                    max(decode(stat_name, 'execute count', delta_val, 0)))),
                                    2)) \""Execute to Parse %\"",
                       decode(max(delta_gets),
                              0,
                              0,
                              round((1 - (max(delta_misses) / max(delta_gets))), 2)) \""Latch Hit %\"",
                       decode(max(decode(stat_name, 'parse time elapsed', delta_val, 0)),
                              0,
                              0,
                              round((max(decode(stat_name, 'parse time cpu', delta_val, 0)) /
                                    max(decode(stat_name,
                                                'parse time elapsed',
                                                delta_val,
                                                0))),
                                    2)) \""Parse CPU to Parse Elapsd %\"",
                       decode(max(decode(stat_name,
                                         'CPU used by this session',
                                         delta_val,
                                         0)), 0, 0,
                              round((1 - (max(decode(stat_name, 'parse time cpu', delta_val, 0)) / max(decode(stat_name,
                                                 'CPU used by this session', delta_val, 0)))),
                                    2)) \"" % Non-Parse CPU\"",
                       max(decode(stat_name, 'parse time cpu', delta, 0)) / 100 \""parse cpu\"",
                       max(decode(stat_name, 'parse time elapsed', delta, 0)) / 100 \""Parse ela\""
                  from load_v");
                tmpSql.AppendFormat(" where snap_id > {0} ", arguments.SnapId);
                tmpSql.Append(" group by snap_id, to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss')  order by snap_id");

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



        public void GetRacLoadSql(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"With load_rac_v AS (
                select  	
                sy.stat_name,
                sy.snap_id,
                sn.end_interval_time,
                nvl(sy.value, 0) - nvl(lag(sy.value) over(partition by sy.stat_name order by sy.snap_id), 0) delta_val,
                case when
                  (extract(day    from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 86400 +
                   extract(hour   from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 3600 +
                   extract(minute from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 60 +
                   extract(second from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id))) > 0
                  then
                   round(
                    (nvl(sy.value, 0) - nvl(lag(sy.value) over(partition by sy.stat_name order by sy.snap_id), 0))
                    / (extract(day    from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 86400 +
                      extract(hour   from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 3600 +
                      extract(minute from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 60 +
                      extract(second from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id))
                     ), 2)
                  else null
                end delta,
                dm.name,
                case when
                  (extract(day    from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 86400 +
                   extract(hour   from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 3600 +
                   extract(minute from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 60 +
                   extract(second from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id))) > 0
                  then
                   round(
                         (nvl(dm.value, 0) - nvl(lag(dm.value) over(partition by dm.name order by dm.snap_id), 0))
                         / (extract(day    from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 86400 +
                           extract(hour   from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 3600 +
                           extract(minute from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 60 +
                           extract(second from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id))), 2)
                       else null
                     end delta_dm,
                     nvl(dm.value, 0)  -nvl(lag(dm.value) over(partition by dm.name order by dm.snap_id), 0) delta_dm_val,
                     nvl(cbs.flushes, 0) - nvl(lag(cbs.flushes) over(order by cbs.snap_id), 0) delta_cbs,
                     nvl(cus.flush1, 0) - nvl(lag(cus.flush1) over(order by cus.snap_id), 0) delta_cusf1,
                     nvl(cus.flush10, 0) - nvl(lag(cus.flush10) over(order by cus.snap_id), 0) delta_cusf10,
                     nvl(cus.flush100, 0) - nvl(lag(cus.flush100) over(order by cus.snap_id), 0) delta_cusf100,
                     nvl(cus.flush1000, 0) - nvl(lag(cus.flush1000) over(order by cus.snap_id), 0) delta_cusf1000,
                     nvl(cus.flush10000, 0) - nvl(lag(cus.flush10000) over(order by cus.snap_id), 0) delta_cusf10000
                 from dba_hist_snapshot sn,
                      dba_hist_dlm_misc dm,
                     dba_hist_current_block_server cus,
                     dba_hist_cr_block_server cbs,
                     dba_hist_sysstat sy

                --from dba_hist_sysstat sy,
                --dba_hist_snapshot sn,
                             --dba_hist_dlm_misc dm,
                             --dba_hist_cr_block_server cbs,
                             --dba_hist_current_block_server cus
                       where sy.snap_id = sn.snap_id
                              and sy.instance_number = sn.instance_number
                              and sy.dbid = sn.dbid
                              and dm.snap_id = sn.snap_id
                              and dm.instance_number = sn.instance_number
                              and dm.dbid = sn.dbid
                              and cbs.snap_id(+) = sn.snap_id
                              and cbs.instance_number(+) = sn.instance_number
                              and cbs.dbid(+) = sn.dbid
                              and cus.snap_id(+) = sn.snap_id
                              and cus.instance_number(+) = sn.instance_number
                              and cus.dbid(+) = sn.dbid
                              and sy.stat_name in (
                                           'DBWR fusion writes',
                                           'gcs messages sent',
                                           'ges messages sent',
                                           'global enqueue gets sync',
                                           'global enqueue gets async',
                                           'global enqueue get time',
                                           'gc cr blocks received',
                                           'gc cr block receive time',
                                           'gc current blocks received',
                                           'gc current block receive time',
                                           'gc cr blocks served',
                                           'gc cr block build time',
                                           'gc cr block flush time',
                                           'gc cr block send time',
                                           'gc current blocks served',
                                           'gc current block pin time',
                                           'gc current block flush time',
                                           'gc current block send time',
                                           'gc blocks lost',
                                           'gc blocks corrupt',
                                           'consistent gets from cache',
                                           'db block gets from cache',
                                           'physical reads cache'
                                          )
                              and dm.name in (
                                           'messages sent directly',
                                           'messages flow controlled',
                                           'messages sent indirectly',
                                           'gcs msgs received',
                                           'gcs msgs process time(ms)',
                                           'ges msgs received',
                                           'ges msgs process time(ms)',
                                           'msgs sent queued',
                                           'msgs sent queue time (ms)',
                                           'msgs sent queued on ksxp',
                                           'msgs sent queue time on ksxp (ms)',
                                           'msgs received queue time (ms)',
                                           'msgs received queued'
                                          )");
                tmpSql.AppendFormat("          and sn.snap_id between {0} and {1} ",arguments.SnapId,arguments.SnapId);
                tmpSql.AppendFormat(" and sn.dbid = {0}",arguments.DbId);
                tmpSql.AppendFormat(" and sn.instance_number = {0} ",arguments.InstanceNumber);
                tmpSql.Append(@"  )   select snap_id \""SnapID\"",
                           to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",
                           max(decode(stat_name, 'gc cr blocks received', delta, 0)) +
                           max(decode(stat_name, 'gc current blocks received', delta, 0)) \""GC blocks received\"",
                           max(decode(stat_name, 'gc cr blocks served', delta, 0)) +
                           max(decode(stat_name, 'gc current blocks served', delta, 0)) \""GC blocks served\"",
                           max(decode(name, 'gcs msgs received', delta_dm, 0)) +
                           max(decode(name, 'ges msgs received', delta_dm, 0)) \""GCS /GES msg received\"",
                           max(decode(stat_name, 'gcs messages sent', delta, 0)) +
                           max(decode(stat_name, 'ges messages sent', delta, 0)) \""GCS /GES msg sent\"",
                           max(decode(stat_name, 'DBWR fusion writes', delta, 0)) \""DBWR fusion writes\"",
                           ((max(decode(stat_name, 'gc cr blocks received', delta, 0)) +
                             max(decode(stat_name, 'gc current blocks received', delta, 0)) +
                             max(decode(stat_name, 'gc cr blocks served', delta, 0)) +
                             max(decode(stat_name, 'gc current blocks served', delta, 0))
                            ) * (select to_number(value) from dba_hist_parameter
                                  where parameter_name = 'db_block_size' ");
                tmpSql.AppendFormat("   and dbid = {0} ",arguments.DbId);
                tmpSql.AppendFormat("    and instance_number = {0}",arguments.InstanceNumber);
                tmpSql.AppendFormat("and snap_id = {0}) ",arguments.SnapId);
                tmpSql.Append(@"+ (max(decode(stat_name, 'gcs messages sent', delta, 0)) + 
                      max(decode(stat_name, 'ges messages sent', delta, 0)) +
                             max(decode(name, 'gcs msgs received', delta_dm, 0)) +
                             max(decode(name, 'ges msgs received', delta_dm, 0))
                            ) * 200) / 1048576 \""Interconnect traffic(Mb)\"",
                           max(decode(stat_name, 'gc blocks lost', delta_val, 0)) \""gc lost\"",
                           max(decode(stat_name, 'gc blocks corrupt', delta_val, 0)) \""gc corrupt\""
                     from load_rac_v ");
                tmpSql.AppendFormat("    where snap_id > {0} ", arguments.SnapId);
                tmpSql.Append("group by snap_id, to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') order by snap_id");

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
        public void GetRacGcEfficiency(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"With load_rac_v AS (select sy.stat_name,
              sy.snap_id,
              sn.end_interval_time,
              nvl(sy.value, 0) - nvl(lag(sy.value) over(partition by sy.stat_name order by sy.snap_id), 0) delta_val,
              case when
                (extract(day    from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 86400 +
                 extract(hour   from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 3600 +
                 extract(minute from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 60 +
                 extract(second from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id))) > 0
                then
                 round(
                  (nvl(sy.value, 0) - nvl(lag(sy.value) over(partition by sy.stat_name order by sy.snap_id), 0))
                  / (extract(day    from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 86400 +
                    extract(hour   from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 3600 +
                    extract(minute from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 60 +
                    extract(second from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id))
                   ), 2)
                else null
              end delta,
              dm.name,
              case when
                (extract(day    from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 86400 +
                 extract(hour   from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 3600 +
                 extract(minute from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 60 +
                 extract(second from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id))) > 0
                then
                 round(
                  (nvl(dm.value, 0) - nvl(lag(dm.value) over(partition by dm.name order by dm.snap_id), 0))
                  / (extract(day    from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 86400 +
                    extract(hour   from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 3600 +
                    extract(minute from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 60 +
                    extract(second from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id))), 2)
                else null
              end delta_dm,
              nvl(dm.value, 0)  -nvl(lag(dm.value) over(partition by dm.name order by dm.snap_id), 0) delta_dm_val,
              nvl(cbs.flushes, 0) - nvl(lag(cbs.flushes) over(order by cbs.snap_id), 0) delta_cbs,
              nvl(cus.flush1, 0) - nvl(lag(cus.flush1) over(order by cus.snap_id), 0) delta_cusf1,
              nvl(cus.flush10, 0) - nvl(lag(cus.flush10) over(order by cus.snap_id), 0) delta_cusf10,
              nvl(cus.flush100, 0) - nvl(lag(cus.flush100) over(order by cus.snap_id), 0) delta_cusf100,
              nvl(cus.flush1000, 0) - nvl(lag(cus.flush1000) over(order by cus.snap_id), 0) delta_cusf1000,
              nvl(cus.flush10000, 0) - nvl(lag(cus.flush10000) over(order by cus.snap_id), 0) delta_cusf10000
          from dba_hist_snapshot sn,
               dba_hist_dlm_misc dm,
              dba_hist_current_block_server cus,
              dba_hist_cr_block_server cbs,
              dba_hist_sysstat sy
   where sy.snap_id = sn.snap_id
          and sy.instance_number = sn.instance_number
          and sy.dbid = sn.dbid
          and dm.snap_id = sn.snap_id
          and dm.instance_number = sn.instance_number
          and dm.dbid = sn.dbid
          and cbs.snap_id(+) = sn.snap_id
          and cbs.instance_number(+) = sn.instance_number
          and cbs.dbid(+) = sn.dbid
          and cus.snap_id(+) = sn.snap_id
          and cus.instance_number(+) = sn.instance_number
          and cus.dbid(+) = sn.dbid
          and sy.stat_name in (
                       'DBWR fusion writes',
                       'gcs messages sent',
                       'ges messages sent',
                       'global enqueue gets sync',
                       'global enqueue gets async',
                       'global enqueue get time',
                       'gc cr blocks received',
                       'gc cr block receive time',
                       'gc current blocks received',
                       'gc current block receive time',
                       'gc cr blocks served',
                       'gc cr block build time',
                       'gc cr block flush time',
                       'gc cr block send time',
                       'gc current blocks served',
                       'gc current block pin time',
                       'gc current block flush time',
                       'gc current block send time',
                       'gc blocks lost',
                       'gc blocks corrupt',
                       'consistent gets from cache',
                       'db block gets from cache',
                       'physical reads cache'
                      )
          and dm.name in (
                       'messages sent directly',
                       'messages flow controlled',
                       'messages sent indirectly',
                       'gcs msgs received',
                       'gcs msgs process time(ms)',
                       'ges msgs received',
                       'ges msgs process time(ms)',
                       'msgs sent queued',
                       'msgs sent queue time (ms)',
                       'msgs sent queued on ksxp',
                       'msgs sent queue time on ksxp (ms)',
                       'msgs received queue time (ms)',
                       'msgs received queued'
                      ) ");
                tmpSql.AppendFormat(" and sn.snap_id between {0} and {1} ",arguments.SnapId,arguments.SnapId);
                tmpSql.AppendFormat(" and sn.dbid = {0} ",arguments.DbId);
                tmpSql.AppendFormat(" and sn.instance_number = {0} ",arguments.InstanceNumber);
                tmpSql.AppendFormat(@" ) select  snap_id,   to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",");
                tmpSql.Append(@"decode(max(decode(stat_name, 'consistent gets from cache', delta_val, 0))
                 + max(decode(stat_name, 'db block gets from cache', delta_val, 0)), 0, 0,
                  round(100 * (1 -
                              ((max(decode(stat_name, 'physical reads cache', delta_val, 0))
                               + max(decode(stat_name, 'gc cr blocks received', delta_val, 0))
                               + max(decode(stat_name, 'gc current blocks received', delta_val, 0)))
                               / (max(decode(stat_name, 'consistent gets from cache', delta_val, 0))
                                + max(decode(stat_name, 'db block gets from cache', delta_val, 0))))), 2)
                             ) \""Buffer access - local cache %\"",
                       decode(max(decode(stat_name, 'consistent gets from cache', delta_val, 0))
                                 + max(decode(stat_name, 'db block gets from cache', delta_val, 0)), 0, 0,
                                  round(100 * ((max(decode(stat_name, 'gc cr blocks received', delta_val, 0))
                                               + max(decode(stat_name, 'gc current blocks received', delta_val, 0)))
                                               / (max(decode(stat_name, 'consistent gets from cache', delta_val, 0))
                                                + max(decode(stat_name, 'db block gets from cache', delta_val, 0)))), 2)
                             ) \""Buffer access - remote cache %\"",
                       decode(max(decode(stat_name, 'consistent gets from cache', delta_val, 0))
                                 + max(decode(stat_name, 'db block gets from cache', delta_val, 0)), 0, 0,
                                  round(100 * (max(decode(stat_name, 'physical reads cache', delta_val, 0))
                                               / (max(decode(stat_name, 'consistent gets from cache', delta_val, 0))
                                                + max(decode(stat_name, 'db block gets from cache', delta_val, 0)))), 2)
                             ) \""Buffer access - disk %\""
                 from load_rac_v ");
                tmpSql.AppendFormat(@" where snap_id > {0} ",arguments.SnapId);
                tmpSql.AppendFormat(@"group by snap_id, to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') order by snap_id");

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
        public void GetRacChar(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"With load_rac_v AS (select 		
              sy.stat_name,
              sy.snap_id,
              sn.end_interval_time,
              nvl(sy.value, 0) - nvl(lag(sy.value) over(partition by sy.stat_name order by sy.snap_id), 0) delta_val,
              case when
                (extract(day    from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 86400 +
                 extract(hour   from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 3600 +
                 extract(minute from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 60 +
                 extract(second from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id))) > 0
                then
                 round(
                  (nvl(sy.value, 0) - nvl(lag(sy.value) over(partition by sy.stat_name order by sy.snap_id), 0))
                  / (extract(day    from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 86400 +
                    extract(hour   from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 3600 +
                    extract(minute from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id)) * 60 +
                    extract(second from sn.end_interval_time -lag(sn.end_interval_time) over(partition by sy.stat_name order by sy.snap_id))
                   ), 2)
                else null
              end delta,
              dm.name,
              case when
                (extract(day    from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 86400 +
                 extract(hour   from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 3600 +
                 extract(minute from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 60 +
                 extract(second from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id))) > 0
                then
                 round(
                  (nvl(dm.value, 0) - nvl(lag(dm.value) over(partition by dm.name order by dm.snap_id), 0))
                  / (extract(day    from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 86400 +
                    extract(hour   from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 3600 +
                    extract(minute from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id)) * 60 +
                    extract(second from sn.end_interval_time -lag(sn.end_interval_time) over(partition by dm.name order by dm.snap_id))), 2)
                else null
              end delta_dm,
              nvl(dm.value, 0)  -nvl(lag(dm.value) over(partition by dm.name order by dm.snap_id), 0) delta_dm_val,
              nvl(cbs.flushes, 0) - nvl(lag(cbs.flushes) over(order by cbs.snap_id), 0) delta_cbs,
              nvl(cus.flush1, 0) - nvl(lag(cus.flush1) over(order by cus.snap_id), 0) delta_cusf1,
              nvl(cus.flush10, 0) - nvl(lag(cus.flush10) over(order by cus.snap_id), 0) delta_cusf10,
              nvl(cus.flush100, 0) - nvl(lag(cus.flush100) over(order by cus.snap_id), 0) delta_cusf100,
              nvl(cus.flush1000, 0) - nvl(lag(cus.flush1000) over(order by cus.snap_id), 0) delta_cusf1000,
              nvl(cus.flush10000, 0) - nvl(lag(cus.flush10000) over(order by cus.snap_id), 0) delta_cusf10000
          from dba_hist_snapshot sn,
               dba_hist_dlm_misc dm,
              dba_hist_current_block_server cus,
              dba_hist_cr_block_server cbs,
              dba_hist_sysstat sy
   where sy.snap_id = sn.snap_id
          and sy.instance_number = sn.instance_number
          and sy.dbid = sn.dbid
          and dm.snap_id = sn.snap_id
          and dm.instance_number = sn.instance_number
          and dm.dbid = sn.dbid
          and cbs.snap_id(+) = sn.snap_id
          and cbs.instance_number(+) = sn.instance_number
          and cbs.dbid(+) = sn.dbid
          and cus.snap_id(+) = sn.snap_id
          and cus.instance_number(+) = sn.instance_number
          and cus.dbid(+) = sn.dbid
          and sy.stat_name in (
                       'DBWR fusion writes',
                       'gcs messages sent',
                       'ges messages sent',
                       'global enqueue gets sync',
                       'global enqueue gets async',
                       'global enqueue get time',
                       'gc cr blocks received',
                       'gc cr block receive time',
                       'gc current blocks received',
                       'gc current block receive time',
                       'gc cr blocks served',
                       'gc cr block build time',
                       'gc cr block flush time',
                       'gc cr block send time',
                       'gc current blocks served',
                       'gc current block pin time',
                       'gc current block flush time',
                       'gc current block send time',
                       'gc blocks lost',
                       'gc blocks corrupt',
                       'consistent gets from cache',
                       'db block gets from cache',
                       'physical reads cache'
                      )
          and dm.name in (
                       'messages sent directly',
                       'messages flow controlled',
                       'messages sent indirectly',
                       'gcs msgs received',
                       'gcs msgs process time(ms)',
                       'ges msgs received',
                       'ges msgs process time(ms)',
                       'msgs sent queued',
                       'msgs sent queue time (ms)',
                       'msgs sent queued on ksxp',
                       'msgs sent queue time on ksxp (ms)',
                       'msgs received queue time (ms)',
                       'msgs received queued'
                      ) ");
                tmpSql.AppendFormat(" and sn.snap_id between {0} and {1} ",arguments.SnapId,arguments.SnapId);
                tmpSql.AppendFormat(" and sn.dbid = {0} ",arguments.DbId);

                tmpSql.AppendFormat(" and sn.instance_number = {0} ",arguments.InstanceNumber);
                tmpSql.Append(@" ) select  snap_id,
                
                       to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",
       decode(max(decode(stat_name, 'global enqueue gets async', delta_val, 0))
                 + max(decode(stat_name, 'global enqueue gets sync', delta_val, 0)), 0, 0,
                  round((max(decode(stat_name, 'global enqueue get time', delta_val, 0))
                    / (max(decode(stat_name, 'global enqueue gets async', delta_val, 0))
                     + max(decode(stat_name, 'global enqueue gets sync', delta_val, 0)))) * 10, 1)
             ) \""Avg GE get time(ms)\"",
       decode(max(decode(stat_name, 'gc cr blocks received', delta_val, 0)), 0, 0,
                  round((max(decode(stat_name, 'gc cr block receive time', delta_val, 0))
                    / max(decode(stat_name, 'gc cr blocks received', delta_val, 0))) * 10, 1)
             ) \""Avg GC cr blk rcv time(ms)\"",
       decode(max(decode(stat_name, 'gc current blocks received', delta_val, 0)), 0, 0,
                  round((max(decode(stat_name, 'gc current block receive time', delta_val, 0))
                     / max(decode(stat_name, 'gc current blocks received', delta_val, 0))) * 10, 1)
             ) \""Avg GC cur blk rcv time(ms)\"",
       decode(max(decode(stat_name, 'gc cr blocks served', delta_val, 0)), 0, 0,
                  round((max(decode(stat_name, 'gc cr block build time', delta_val, 0))
                    / max(decode(stat_name, 'gc cr blocks served', delta_val, 0))) * 10, 1)
             ) \""Avg GC cr blk bld time(ms)\"",
       decode(max(decode(stat_name, 'gc cr blocks served', delta_val, 0)), 0, 0,
                  round((max(decode(stat_name, 'gc cr block send time', delta_val, 0))
                    / max(decode(stat_name, 'gc cr blocks served', delta_val, 0))) * 10, 1)
             ) \""Avg GC cr blk snd time(ms)\"",
       decode(max(delta_cbs), 0, 0,
                  round((max(decode(stat_name, 'gc cr block flush time', delta_val, 0))
                    / max(delta_cbs)) * 10, 1)
             ) \""Avg GC cr blk flush time(ms)\"",
       decode(max(decode(stat_name, 'gc cr blocks served', delta_val, 0)), 0, 0,
                  round((max(delta_cbs)
                    / max(decode(stat_name, 'gc cr blocks served', delta_val, 0))) * 100, 1)
             ) \""GC log flsh for cr blk srvd%\"",
       decode(max(decode(stat_name, 'gc current blocks served', delta_val, 0)), 0, 0,
                  round((max(decode(stat_name, 'gc current block pin time', delta_val, 0))
                    / max(decode(stat_name, 'gc current blocks served', delta_val, 0))) * 10, 1)
             ) \""Avg GC cur blk pin time(ms)\"",
       decode(max(decode(stat_name, 'gc current blocks served', delta_val, 0)), 0, 0,
                  round((max(decode(stat_name, 'gc current block send time', delta_val, 0))
                    / max(decode(stat_name, 'gc current blocks served', delta_val, 0))) * 10, 1)
             ) \""Avg GC cur blk snd time(ms)\"",
       decode(max(delta_cusf1) + max(delta_cusf10) + max(delta_cusf100) + max(delta_cusf1000) + max(delta_cusf10000), 0, 0,
                  round((max(decode(stat_name, 'gc current block flush time', delta_val, 0))
                    / (max(delta_cusf1) + max(delta_cusf10) + max(delta_cusf100) + max(delta_cusf1000) + max(delta_cusf10000))) * 10, 1)
             ) \""Avg GC cu blk flush time(ms)\"",
       decode(max(decode(stat_name, 'gc current blocks served', delta_val, 0)), 0, 0,
                  round(((max(delta_cusf1) + max(delta_cusf10) + max(delta_cusf100) + max(delta_cusf1000) + max(delta_cusf10000))
                    / max(decode(stat_name, 'gc current blocks served', delta_val, 0))) * 100, 1)
             ) \""GC log flsh for cu blk srvd%\""
 from load_rac_v ");
                tmpSql.AppendFormat(" where snap_id > {0}    ",arguments.SnapId);
                tmpSql.Append(" group by snap_id, to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') order by snap_id");

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


        public void GetWait5_01(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"select sn.snap_id,
       to_char(sn.end_interval_time,'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",
       v_dbtime.delta / 1000000 \""DB time\"",
       decode(sum(case when v_call.stat_name in ('user calls', 'recursive calls') then v_call.delta else 0 end),0,0,
        (v_dbtime.delta / 1000) / sum(case when v_call.stat_name in ('user calls', 'recursive calls') then v_call.delta else 0 end)) \""db time per call\"",
       decode(sum(case when v_call.stat_name = 'execute count' then v_call.delta else 0 end),0,0,
        (v_dbtime.delta / 1000) / sum(case when v_call.stat_name = 'execute count' then v_call.delta else 0 end)) \""db time per execute\"",
       max(decode(rank, 1, v1.delta_wait, 0)) wait_1,
       max(decode(rank, 2, v1.delta_wait, 0)) wait_2,
       max(decode(rank, 3, v1.delta_wait, 0)) wait_3,
       max(decode(rank, 4, v1.delta_wait, 0)) wait_4,
       max(decode(rank, 5, v1.delta_wait, 0)) wait_5,
       max(decode(rank, 1, v1.delta_time / 1000000, 0)) time_1,
       max(decode(rank, 2, v1.delta_time / 1000000, 0)) time_2,
       max(decode(rank, 3, v1.delta_time / 1000000, 0)) time_3,
       max(decode(rank, 4, v1.delta_time / 1000000, 0)) time_4,
       max(decode(rank, 5, v1.delta_time / 1000000, 0)) time_5,
       max(decode(rank, 1, v1.avg_wait, 0)) avg_wait_1,
       max(decode(rank, 2, v1.avg_wait, 0)) avg_wait_2,
       max(decode(rank, 3, v1.avg_wait, 0)) avg_wait_3,
       max(decode(rank, 4, v1.avg_wait, 0)) avg_wait_4,
       max(decode(rank, 5, v1.avg_wait, 0)) avg_wait_5,
       max(decode(rank, 1, decode(v_dbtime.delta, 0, 0, v1.delta_time / v_dbtime.delta), 0)) pctwait_1,
       max(decode(rank, 2, decode(v_dbtime.delta, 0, 0, v1.delta_time / v_dbtime.delta), 0)) pctwait_2,
       max(decode(rank, 3, decode(v_dbtime.delta, 0, 0, v1.delta_time / v_dbtime.delta), 0)) pctwait_3,
       max(decode(rank, 4, decode(v_dbtime.delta, 0, 0, v1.delta_time / v_dbtime.delta), 0)) pctwait_4,
       max(decode(rank, 5, decode(v_dbtime.delta, 0, 0, v1.delta_time / v_dbtime.delta), 0)) pctwait_5
  from(select v_wait.snap_id,
                 v_rank.rank,
                 v_wait.event_name,
                 v_wait.delta_wait,
                 v_wait.delta_time,
                 decode(nvl(v_wait.delta_wait, 0), 0, 0, round(v_wait.delta_time / v_wait.delta_wait / 1000)) avg_wait
            from(select event_name, rownum rank
                     from
                        (select event_name, sum(nvl(delta, 0))
                            from
                               (select snap_id,
                                        event_name,
                                        nvl(time_waited_micro -
                                            lag(time_waited_micro) over(partition by event_name order by snap_id), 0) delta
                                   from dba_hist_system_event
                                  where wait_class != 'Idle' ");
                tmpSql.AppendFormat(" and snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@" union all
                                 select snap_id,
                                        decode(stat_name, 'DB CPU', 'CPU time', stat_name) event_name,
                                        nvl(value - lag(value) over(partition by stat_name order by snap_id), 0)  delta
                                   from dba_hist_sys_time_model
                                  where stat_name = 'DB CPU' ");
                tmpSql.AppendFormat("                    and snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat("                    and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("                     and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@"               )  group by event_name order by sum(delta) desc
                       )
                    where rownum <= 5
                 ) v_rank,
                 (select snap_id,
                          event_name,
                          nvl(total_waits - lag(total_waits) over(partition by event_name order by snap_id), 0) delta_wait,
                          nvl(time_waited_micro - lag(time_waited_micro) over(partition by event_name order by snap_id), 0) delta_time
                     from dba_hist_system_event
                    where wait_class != 'Idle' ");
                tmpSql.AppendFormat(" and snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@" union all
                   select snap_id,
                          decode(stat_name, 'DB CPU', 'CPU time', stat_name) event_name,
                          NULL,
                          nvl(value - lag(value) over(partition by stat_name order by snap_id), 0)  delta
                     from dba_hist_sys_time_model
                    where stat_name = 'DB CPU' ");
                tmpSql.AppendFormat(" and snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat("       and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0}", arguments.DbId);
                tmpSql.Append(@" ) v_wait
     
                where v_wait.event_name = v_rank.event_name
          ) v1,
          (select snap_id,
                   nvl(value - lag(value) over(partition by stat_name order by snap_id), 0)  delta
           from dba_hist_sys_time_model
          where stat_name = 'DB time' ");
                tmpSql.AppendFormat(" and snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@" ) v_dbtime,
               (select snap_id,
                   stat_name,
                   nvl(value - lag(value) over(partition by stat_name order by snap_id), 0)  delta
           from dba_hist_sysstat
          where stat_name in ('user calls', 'recursive calls', 'execute count') ");
                tmpSql.AppendFormat(" and snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);

                tmpSql.AppendFormat(" and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@" ) v_call,
               (select snap_id,
                   end_interval_time
              from dba_hist_snapshot");
                tmpSql.AppendFormat("  where snap_id between 1 + {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@" ) sn     
       where v1.snap_id = sn.snap_id
   and v_call.snap_id = sn.snap_id
   and v_dbtime.snap_id = sn.snap_id
 group by sn.snap_id,
       to_char(sn.end_interval_time, 'yyyy-mm-dd hh24:mi:ss'),
       v_dbtime.delta
 order by sn.snap_id");

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

        public void GetWait5_02(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@" select event_name||' ('|| decode(event_name,'CPU time','N/A',wait_class)||')' AS \""Wait5_Rank\"", rownum rank
           from
              (select event_name, wait_class, sum(nvl(delta, 0))
                  from
                    (select snap_id,
                             event_name,
                             wait_class,
                             nvl(time_waited_micro -
                              lag(time_waited_micro) over(partition by event_name order by snap_id), 0) delta
                        from dba_hist_system_event
                       where wait_class != 'Idle' ");
                tmpSql.AppendFormat(" and snap_id between {0} and {1} ",arguments.SnapId,arguments.SnapId);
                tmpSql.AppendFormat(" and instance_number = {0}",arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0}",arguments.DbId);
                tmpSql.Append(@" union all  select snap_id,decode(stat_name, 'DB CPU', 'CPU time', stat_name) event_name,null,
                             nvl(value - lag(value) over(partition by stat_name order by snap_id), 0)  delta
                        from dba_hist_sys_time_model
                       where stat_name = 'DB CPU'");
                tmpSql.AppendFormat(" and snap_id between {0} and {1} ",arguments.SnapId,arguments.SnapId);
                tmpSql.AppendFormat(" and instance_number = {0}",arguments.InstanceNumber);
                tmpSql.AppendFormat("and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(" )   group by event_name, wait_class order by sum(delta) desc )  where rownum <= 5 order by rank; ");

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


        public void GetTimeModel(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"select  snap_id,
        to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",
        sum(decode(stat_name, 'DB time', delta / 1000000, 0)) \""DB time\"",
        sum(decode(stat_name, 'DB CPU', delta / 1000000, 0)) \""DB CPU\"",
        sum(decode(stat_name, 'background elapsed time', delta / 1000000, 0)) \""background\"",
        sum(decode(stat_name, 'background cpu time', delta / 1000000, 0)) \""background cpu\"",
        sum(decode(stat_name, 'sequence load elapsed time', delta / 1000000, 0)) \""sequence load\"",
        sum(decode(stat_name, 'parse time elapsed', delta / 1000000, 0)) \""parse\"",
        sum(decode(stat_name, 'hard parse elapsed time', delta / 1000000, 0)) \""hard parse\"",
        sum(decode(stat_name, 'sql execute elapsed time', delta / 1000000, 0)) \""sql execute\"",
        sum(decode(stat_name, 'connection management call elapsed time', delta / 1000000, 0)) \""CM call\"",
        sum(decode(stat_name, 'failed parse elapsed time', delta / 1000000, 0)) \""failed parse\"",
        sum(decode(stat_name, 'failed parse (out of shared memory) elapsed time', delta / 1000000, 0)) \""failed parse(OutOfSM)\"",
        sum(decode(stat_name, 'hard parse (sharing criteria) elapsed time', delta / 1000000, 0)) \""hard parse(SC)\"",
        sum(decode(stat_name, 'hard parse (bind mismatch) elapsed time', delta / 1000000, 0)) \""hard parse(BM)\"",
        sum(decode(stat_name, 'PL/SQL execution elapsed time', delta / 1000000, 0)) \""PL/SQL execution\"",
        sum(decode(stat_name, 'inbound PL/SQL rpc elapsed time', delta / 1000000, 0)) \""inbound PL/SQL rpc\"",
        sum(decode(stat_name, 'PL/SQL compilation elapsed time', delta / 1000000, 0)) \""PL/SQL compilation\"",
        sum(decode(stat_name, 'Java execution elapsed time', delta / 1000000, 0)) \""Java execution\"",
        sum(decode(stat_name, 'DB CPU', pct, 0)) \""%DB CPU\"",
        sum(decode(stat_name, 'sequence load elapsed time', pct, 0)) \""%sequence load\"",
        sum(decode(stat_name, 'parse time elapsed', pct, 0)) \""%parse\"",
        sum(decode(stat_name, 'hard parse elapsed time', pct, 0)) \""%hard parse\"",
        sum(decode(stat_name, 'sql execute elapsed time', pct, 0)) \""%sql execute\"",
        sum(decode(stat_name, 'connection management call elapsed time', pct, 0)) \""%CM call\"",
        sum(decode(stat_name, 'failed parse elapsed time', pct, 0)) \""%failed parse\"",
        sum(decode(stat_name, 'failed parse (out of shared memory) elapsed time', pct, 0)) \""%failed parse(OutOfSM)\"",
        sum(decode(stat_name, 'hard parse (sharing criteria) elapsed time', pct, 0)) \""%hard parse(SC)\"",
        sum(decode(stat_name, 'hard parse (bind mismatch) elapsed time', pct, 0)) \""%hard parse(BM)\"",
        sum(decode(stat_name, 'PL/SQL execution elapsed time', pct, 0)) \""%PL/SQL execution\"",
        sum(decode(stat_name, 'inbound PL/SQL rpc elapsed time', pct, 0)) \""%inbound PL/SQL rpc\"",
        sum(decode(stat_name, 'PL/SQL compilation elapsed time', pct, 0)) \""%PL/SQL compilation\"",
        sum(decode(stat_name, 'Java execution elapsed time', pct, 0)) \""%Java execution\""
  from(
         select snap_id,
                end_interval_time,
                stat_name,
                delta,
                decode(sum(decode(stat_name, 'DB time', delta, 0)) over(partition by snap_id), 0, 0,
                       delta / sum(decode(stat_name, 'DB time', delta, 0)) over(partition by snap_id)) pct
          from
               (
                select sn.snap_id,
                       sn.end_interval_time,
                       stm.stat_name,
                      (nvl(value - lag(value)
                         over(partition by stm.stat_name order by stm.snap_id), 0)) delta
                  from dba_hist_sys_time_model stm,
                       dba_hist_snapshot sn
                 where stm.snap_id = sn.snap_id
                   and stm.instance_number = sn.instance_number
                   and stm.dbid = sn.dbid");
                tmpSql.AppendFormat(" and sn.snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and sn.instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and sn.dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(" ) ) where snap_id >{0} ", arguments.SnapId);
                tmpSql.Append("group by snap_id, to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') order by snap_id");

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

        public void GetOsstat(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"select  snap_id,
                    to_char(end_interval_time,'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",
                    max(decode(stat_name, 'BUSY_TIME', delta, 0)) / 100 \""BUSY_TIME\"",
                    max(decode(stat_name, 'IDLE_TIME', delta, 0)) / 100 \""IDLE_TIME\"",
                    max(decode(stat_name, 'USER_TIME', delta, 0)) / 100 \""USER_TIME\"",
                    max(decode(stat_name, 'SYS_TIME', delta, 0)) / 100 \""SYS_TIME\"",
                    max(decode(stat_name, 'IOWAIT_TIME', delta, 0)) / 100 \""IOWAIT_TIME\"",
                    max(decode(stat_name, 'NICE_TIME', delta, 0)) / 100 \""NICE_TIME\"",
                    max(decode(stat_name, 'LOAD', delta, 0)) \""LOAD\"",
                    max(decode(stat_name, 'RSRC_MGR_CPU_WAIT_TIME', delta, 0)) / 100 \""RSRC_MGR_CPU_WAIT_TIME\"",
                    max(decode(stat_name, 'PHYSICAL_MEMORY_BYTES', value, 0)) \""PHYSICAL_MEMORY_BYTES\"",
                    max(decode(stat_name, 'NUM_CPUS', value, 0)) \""NUM_CPUS\"",
                    max(decode(stat_name, 'NUM_CPU_CORES', value, 0)) \""NUM_CPU_CORES\"",
                    max(decode(stat_name, 'NUM_CPU_SOCKETS', value, 0)) \""NUM_CPU_SOCKETS\"",
                    decode((max(decode(stat_name, 'IDLE_TIME', delta, 0)) + max(decode(stat_name, 'BUSY_TIME', delta, 0))), 0, 0,
                     (max(decode(stat_name, 'USER_TIME', delta, 0)) + max(decode(stat_name, 'NICE_TIME', delta, 0)))
                    / (max(decode(stat_name, 'IDLE_TIME', delta, 0)) + max(decode(stat_name, 'BUSY_TIME', delta, 0)))) \""%usr\"",
                    decode((max(decode(stat_name, 'IDLE_TIME', delta, 0)) + max(decode(stat_name, 'BUSY_TIME', delta, 0))), 0, 0,
                     max(decode(stat_name, 'SYS_TIME', delta, 0))
                    / (max(decode(stat_name, 'IDLE_TIME', delta, 0)) + max(decode(stat_name, 'BUSY_TIME', delta, 0)))) \""%sys\"",
                    decode((max(decode(stat_name, 'IDLE_TIME', delta, 0)) + max(decode(stat_name, 'BUSY_TIME', delta, 0))), 0, 0,
                     (max(decode(stat_name, 'IOWAIT_TIME', delta, 0)))
                    / (max(decode(stat_name, 'IDLE_TIME', delta, 0)) + max(decode(stat_name, 'BUSY_TIME', delta, 0)))) \""%wio\"",
                    decode((max(decode(stat_name, 'IDLE_TIME', delta, 0)) + max(decode(stat_name, 'BUSY_TIME', delta, 0))), 0, 0,
                     max(decode(stat_name, 'IDLE_TIME', delta, 0))
                    / (max(decode(stat_name, 'IDLE_TIME', delta, 0)) + max(decode(stat_name, 'BUSY_TIME', delta, 0)))) \""%idle\""
                    from(
                     select sn.snap_id,
                            sn.end_interval_time,
                            os.stat_name,
                            (nvl(value - lag(value)
                              over(partition by stat_name order by os.snap_id), 0)) delta,
                            os.value value
                      from dba_hist_osstat os,
                           dba_hist_snapshot sn
                     where os.snap_id = sn.snap_id
                       and os.instance_number = sn.instance_number
                       and os.dbid = sn.dbid ");
                tmpSql.AppendFormat(" and sn.snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and sn.instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and sn.dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(" ) where snap_id > {0} ", arguments.SnapId);
                tmpSql.Append("group by snap_id, to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') order by snap_id");

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

        public void GetInstActivity(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"select  snap_id,
                        to_char(end_interval_time,'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",
                        max(decode(stat_name, 'logons current', value, 0)) \""logons current\"",
                        max(decode(stat_name, 'opened cursors current', value, 0)) \""opened cursors current\"",
                        max(decode(stat_name, 'workarea memory allocated', value, 0)) \""workarea memory allocated\"",
                        max(decode(stat_name, 'session cursor cache count', value, 0)) \""session cursor cache count\"",
                        max(decode(stat_name, 'open threads (derived)', value, 0)) \""open threads\""
                  from(
                         select sn.snap_id,
                                sn.end_interval_time,
                                v1.stat_name,
                                v1.value
                          from(select snap_id,
                                       instance_number,
                                       dbid,
                                       stat_name,
                                       value
                                  from dba_hist_sysstat sy
                                 where sy.stat_name in ('logons current',
                                                         'opened cursors current',
                                                         'workarea memory allocated',
                                                         'session cursor cache count')
                                union all
                                select snap_id,
                                       instance_number,
                                       dbid,
                                       'open threads (derived)' stat_name,
                                       count(thread#) value
                                  from dba_hist_thread
                                 where status = 'OPEN'
                                  group by snap_id, instance_number, dbid
                               ) v1,
                               dba_hist_snapshot sn
                         where v1.snap_id = sn.snap_id
                           and v1.instance_number = sn.instance_number
                           and v1.dbid = sn.dbid ");
                tmpSql.AppendFormat(" and sn.snap_id between 1 + {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat("           and sn.instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("and sn.dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(") group by snap_id, to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') order by snap_id");

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


        public void GetSqlElap_01(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"select sn.snap_id,
       to_char(sn.end_interval_time,'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",
       max(decode(rank, 1, elap / 1000000, 0)) rank1_elap,
       max(decode(rank, 2, elap / 1000000, 0)) rank2_elap,
       max(decode(rank, 3, elap / 1000000, 0)) rank3_elap,
       max(decode(rank, 4, elap / 1000000, 0)) rank4_elap,
       max(decode(rank, 5, elap / 1000000, 0)) rank5_elap,
       max(decode(rank, 1, cput / 1000000, 0)) rank1_cput,
       max(decode(rank, 2, cput / 1000000, 0)) rank2_cput,
       max(decode(rank, 3, cput / 1000000, 0)) rank3_cput,
       max(decode(rank, 4, cput / 1000000, 0)) rank4_cput,
       max(decode(rank, 5, cput / 1000000, 0)) rank5_cput,
       max(decode(rank, 1, exec, 0)) rank1_exec,
       max(decode(rank, 2, exec, 0)) rank2_exec,
       max(decode(rank, 3, exec, 0)) rank3_exec,
       max(decode(rank, 4, exec, 0)) rank4_exec,
       max(decode(rank, 5, exec, 0)) rank5_exec,
       decode(max(decode(rank, 1, exec, 0)), 0, 0, max(decode(rank, 1, elap / 1000000, 0)) / max(decode(rank, 1, exec, 0))) rank1_elap_per_exec,
       decode(max(decode(rank, 2, exec, 0)), 0, 0, max(decode(rank, 2, elap / 1000000, 0)) / max(decode(rank, 2, exec, 0))) rank2_elap_per_exec,
       decode(max(decode(rank, 3, exec, 0)), 0, 0, max(decode(rank, 3, elap / 1000000, 0)) / max(decode(rank, 3, exec, 0))) rank3_elap_per_exec,
       decode(max(decode(rank, 4, exec, 0)), 0, 0, max(decode(rank, 4, elap / 1000000, 0)) / max(decode(rank, 4, exec, 0))) rank4_elap_per_exec,
       decode(max(decode(rank, 5, exec, 0)), 0, 0, max(decode(rank, 5, elap / 1000000, 0)) / max(decode(rank, 5, exec, 0))) rank5_elap_per_exec,
       decode(max(delta_dbtime), 0, 0, max(decode(rank, 1, elap, 0)) / max(delta_dbtime)) rank1_elap_per_dbtim,
       decode(max(delta_dbtime), 0, 0, max(decode(rank, 2, elap, 0)) / max(delta_dbtime)) rank2_elap_per_dbtim,
       decode(max(delta_dbtime), 0, 0, max(decode(rank, 3, elap, 0)) / max(delta_dbtime)) rank3_elap_per_dbtim,
       decode(max(delta_dbtime), 0, 0, max(decode(rank, 4, elap, 0)) / max(delta_dbtime)) rank4_elap_per_dbtim,
       decode(max(delta_dbtime), 0, 0, max(decode(rank, 5, elap, 0)) / max(delta_dbtime)) rank5_elap_per_dbtim,
       max(decode(rank, 1, sql_id, null)) rank1,
       max(decode(rank, 2, sql_id, null)) rank2,
       max(decode(rank, 3, sql_id, null)) rank3,
       max(decode(rank, 4, sql_id, null)) rank4,
       max(decode(rank, 5, sql_id, null)) rank5
  from(select v_sqlstat.snap_id,
                 v_rank.rank,
                 v_sqlstat.sql_id,
                 v_sqlstat.module,
                 v_sqlstat.elap,
                 v_sqlstat.cput,
                 v_sqlstat.exec
            from(select sql_id, rownum rank
                     from
                        (select sql_id, sum(nvl(elapsed_time_delta, 0))
                            from dba_hist_sqlstat ");
                tmpSql.AppendFormat(" where snap_id between 1 + {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat("  and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@" group by sql_id order by sum(nvl(elapsed_time_delta, 0)) desc)
                    where rownum <= 5
                 ) v_rank,
                 (select snap_id,
                          sql_id,
                          module,
                          elapsed_time_delta elap,
                          cpu_time_delta cput,
                          executions_delta exec
                     from dba_hist_sqlstat ");
                tmpSql.AppendFormat(" where snap_id between 1 + {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@") v_sqlstat 
            where v_sqlstat.sql_id = v_rank.sql_id
          ) v1,
          (select sn.snap_id,
                   sn.end_interval_time,
                   nvl(value - lag(value) over(partition by stm.stat_name order by stm.snap_id), 0)  delta_dbtime
             from dba_hist_snapshot sn,
                  dba_hist_sys_time_model stm
            where sn.snap_id = stm.snap_id
               and sn.instance_number = stm.instance_number
               and sn.dbid = stm.dbid
               and stm.stat_name = 'DB time' ");
                tmpSql.AppendFormat(" and stm.snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and stm.instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and stm.dbid = {0} ", arguments.DbId);
                tmpSql.Append(" ) sn where v1.snap_id(+) = sn.snap_id ");
                tmpSql.AppendFormat("and sn.snap_id > {0}  group by sn.snap_id, to_char(sn.end_interval_time, 'yyyy-mm-dd hh24:mi:ss') order by sn.snap_id", arguments.SnapId);

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

        public void GetSqlElap_02(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"select sql_id, rownum rank
                  from (select sql_id, sum(nvl(elapsed_time_delta, 0))
                          from dba_hist_sqlstat ");
                tmpSql.AppendFormat(" where snap_id between 1 + {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("           and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat("         group by sql_id order by sum(nvl(elapsed_time_delta, 0)) desc) ");
                tmpSql.AppendFormat(" where rownum <= 5 order by rank");

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

        public void GetSqlElap_03(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"select rank,
                     v1.sql_id,
                     v1.module,
                     nvl(replace(replace(replace(to_char(substr(st.sql_text, 1, 2500)),
                                                 chr(9),
                                                 ' '),
                                         chr(10),
                                         ' '),
                                 chr(13),
                                 ' '),
                         '** Not Found **') \""SQL Text\""
                from(select sql_id, dbid, module, rownum rank
                        from(select sql_id,
                                     dbid,
                                     module,
                                     sum(nvl(elapsed_time_delta, 0))
                                from dba_hist_sqlstat ");
                tmpSql.AppendFormat(" where snap_id between 1 + {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat("                  and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@" group by sql_id, dbid, module  
                                 order by sum(nvl(elapsed_time_delta, 0)) desc)
                       where rownum <= 5) v1,
                     dba_hist_sqltext st
                where st.sql_id(+) = v1.sql_id
                and st.dbid(+) = v1.dbid ");

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

        public void GetSql_cpu_01(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"select sn.snap_id,
                       to_char(sn.end_interval_time,'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",
                       max(decode(rank, 1, cput / 1000000, 0)) rank1_cput,
                       max(decode(rank, 2, cput / 1000000, 0)) rank2_cput,
                       max(decode(rank, 3, cput / 1000000, 0)) rank3_cput,
                       max(decode(rank, 4, cput / 1000000, 0)) rank4_cput,
                       max(decode(rank, 5, cput / 1000000, 0)) rank5_cput,
                       max(decode(rank, 1, elap / 1000000, 0)) rank1_elap,
                       max(decode(rank, 2, elap / 1000000, 0)) rank2_elap,
                       max(decode(rank, 3, elap / 1000000, 0)) rank3_elap,
                       max(decode(rank, 4, elap / 1000000, 0)) rank4_elap,
                       max(decode(rank, 5, elap / 1000000, 0)) rank5_elap,
                       max(decode(rank, 1, exec, 0)) rank1_exec,
                       max(decode(rank, 2, exec, 0)) rank2_exec,
                       max(decode(rank, 3, exec, 0)) rank3_exec,
                       max(decode(rank, 4, exec, 0)) rank4_exec,
                       max(decode(rank, 5, exec, 0)) rank5_exec,
                       decode(max(decode(rank, 1, exec, 0)), 0, 0, max(decode(rank, 1, cput / 1000000, 0)) / max(decode(rank, 1, exec, 0))) rank1_cput_per_exec,
                       decode(max(decode(rank, 2, exec, 0)), 0, 0, max(decode(rank, 2, cput / 1000000, 0)) / max(decode(rank, 2, exec, 0))) rank2_cput_per_exec,
                       decode(max(decode(rank, 3, exec, 0)), 0, 0, max(decode(rank, 3, cput / 1000000, 0)) / max(decode(rank, 3, exec, 0))) rank3_cput_per_exec,
                       decode(max(decode(rank, 4, exec, 0)), 0, 0, max(decode(rank, 4, cput / 1000000, 0)) / max(decode(rank, 4, exec, 0))) rank4_cput_per_exec,
                       decode(max(decode(rank, 5, exec, 0)), 0, 0, max(decode(rank, 5, cput / 1000000, 0)) / max(decode(rank, 5, exec, 0))) rank5_cput_per_exec,
                       decode(max(delta_dbtime), 0, 0, max(decode(rank, 1, elap, 0)) / max(delta_dbtime)) rank1_elap_per_dbtim,
                       decode(max(delta_dbtime), 0, 0, max(decode(rank, 2, elap, 0)) / max(delta_dbtime)) rank2_elap_per_dbtim,
                       decode(max(delta_dbtime), 0, 0, max(decode(rank, 3, elap, 0)) / max(delta_dbtime)) rank3_elap_per_dbtim,
                       decode(max(delta_dbtime), 0, 0, max(decode(rank, 4, elap, 0)) / max(delta_dbtime)) rank4_elap_per_dbtim,
                       decode(max(delta_dbtime), 0, 0, max(decode(rank, 5, elap, 0)) / max(delta_dbtime)) rank5_elap_per_dbtim,
                       max(decode(rank, 1, sql_id, null)) rank1,
                       max(decode(rank, 2, sql_id, null)) rank2,
                       max(decode(rank, 3, sql_id, null)) rank3,
                       max(decode(rank, 4, sql_id, null)) rank4,
                       max(decode(rank, 5, sql_id, null)) rank5
                  from(select v_sqlstat.snap_id,
                                 v_rank.rank,
                                 v_sqlstat.sql_id,
                                 v_sqlstat.module,
                                 v_sqlstat.elap,
                                 v_sqlstat.cput,
                                 v_sqlstat.exec
                            from(select sql_id, rownum rank
                                     from
                                        (select sql_id, sum(nvl(cpu_time_delta, 0))
                                            from dba_hist_sqlstat ");
                tmpSql.AppendFormat(" where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.AppendFormat("  and instance_number = &inst_no ");
                tmpSql.AppendFormat(" and dbid = &dbid ");
                tmpSql.Append(@" group by sql_id order by sum(nvl(cpu_time_delta, 0)) desc
                )
                                    where rownum <= 5
                                 ) v_rank,
                                 (select snap_id,
                                          sql_id,
                                          module,
                                          elapsed_time_delta elap,
                                          cpu_time_delta cput,
                                          executions_delta exec
                                     from dba_hist_sqlstat ");
                tmpSql.AppendFormat(" where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.AppendFormat(" and instance_number = &inst_no ");
                tmpSql.AppendFormat("  and dbid = &dbid ");
                tmpSql.Append(@" ) v_sqlstat 
                            where v_sqlstat.sql_id = v_rank.sql_id
                          ) v1,
                          (select sn.snap_id,
                                   sn.end_interval_time,
                                   nvl(value - lag(value) over(partition by stm.stat_name order by stm.snap_id), 0)  delta_dbtime
                             from dba_hist_snapshot sn,
                                  dba_hist_sys_time_model stm
                            where sn.snap_id = stm.snap_id
                               and sn.instance_number = stm.instance_number
                               and sn.dbid = stm.dbid
                               and stm.stat_name = 'DB time' ");
                tmpSql.AppendFormat(" and stm.snap_id between &snap_fr and & snap_to ");
                tmpSql.AppendFormat(" and stm.instance_number = &inst_no ");
                tmpSql.AppendFormat("  and stm.dbid = &dbid ");
                tmpSql.Append(@") sn   
                    where v1.snap_id(+) = sn.snap_id
                   and sn.snap_id > &snap_fr
                 group by sn.snap_id,
                       to_char(sn.end_interval_time, 'yyyy-mm-dd hh24:mi:ss')
                 order by sn.snap_id");

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

        public void GetSql_cpu_02(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"");

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

        public void GetSql_cpu_03(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"");

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
