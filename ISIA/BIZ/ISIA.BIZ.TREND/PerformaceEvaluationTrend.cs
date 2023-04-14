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
                tmpSql.AppendFormat(@"with load_v as (select sy.stat_name, sy.snap_id, sn.end_interval_time, nvl(sy.value, 0) - nvl(lag(sy.value) over(partition by sy.stat_name order by sy.snap_id), 0) delta_val,
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
                    from raw_dba_hist_sysstat_{0} sy,
                         raw_dba_hist_snapshot_{0} sn,
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
                                          'CPU used by this session')",arguments.DbName);
                //tmpSql.AppendFormat("and sn.snap_id between {0} and {1}", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and sn.snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
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
                tmpSql.AppendFormat(@"With load_rac_v AS (
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
                 from raw_dba_hist_snapshot_{0} sn,
                      raw_dba_hist_dlm_misc_{0} dm,
                     raw_dba_hist_current_block_server_{0} cus,
                     raw_dba_hist_cr_block_server_{0} cbs,
                     raw_dba_hist_sysstat_{0} sy
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
                                          )",arguments.DbName);
                //tmpSql.AppendFormat("          and sn.snap_id between {0} and {1} ",arguments.SnapId,arguments.SnapId);
                tmpSql.AppendFormat(" and sn.snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and sn.dbid = {0}", arguments.DbId);
                tmpSql.AppendFormat(" and sn.instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(@"  )   select snap_id \""SnapID\"",
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
                            ) * (select to_number(value) from raw_dba_hist_parameter_{0}
                                  where parameter_name = 'db_block_size' ", arguments.DbName);
                tmpSql.AppendFormat("   and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat("    and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("and snap_id = {0}) ", arguments.SnapId);
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
                tmpSql.AppendFormat(@"With load_rac_v AS (select sy.stat_name,
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
          from raw_dba_hist_snapshot_{0} sn,
               raw_dba_hist_dlm_misc_{0} dm,
              raw_dba_hist_current_block_server_{0} cus,
              raw_dba_hist_cr_block_server_{0} cbs,
              raw_dba_hist_sysstat_{0} sy
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
                      ) ", arguments.DbName);
                //tmpSql.AppendFormat(" and sn.snap_id between {0} and {1} ",arguments.SnapId,arguments.SnapId);
                tmpSql.AppendFormat(" and sn.snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and sn.dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(" and sn.instance_number = {0} ", arguments.InstanceNumber);
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
                tmpSql.AppendFormat(@" where snap_id > {0} ", arguments.SnapId);
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
                tmpSql.AppendFormat(@"With load_rac_v AS (select 		
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
          from raw_dba_hist_snapshot_{0} sn,
               raw_dba_hist_dlm_misc_{0} dm,
              raw_dba_hist_current_block_server_{0} cus,
              raw_dba_hist_cr_block_server_{0} cbs,
              raw_dba_hist_sysstat_{0} sy
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
                      ) ", arguments.DbName);
                //tmpSql.AppendFormat(" and sn.snap_id between {0} and {1} ",arguments.SnapId,arguments.SnapId);
                tmpSql.AppendFormat(" and sn.snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and sn.dbid = {0} ", arguments.DbId);

                tmpSql.AppendFormat(" and sn.instance_number = {0} ", arguments.InstanceNumber);
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
                tmpSql.AppendFormat(" where snap_id > {0}    ", arguments.SnapId);
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
                tmpSql.AppendFormat(@"select sn.snap_id,
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
                                   from raw_dba_hist_system_event_{0}
                                  where wait_class != 'Idle' ", arguments.DbName);
                //tmpSql.AppendFormat(" and snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@" union all
                                 select snap_id,
                                        decode(stat_name, 'DB CPU', 'CPU time', stat_name) event_name,
                                        nvl(value - lag(value) over(partition by stat_name order by snap_id), 0)  delta
                                   from raw_dba_hist_sys_time_model_{0}
                                  where stat_name = 'DB CPU' ", arguments.DbName);
                //tmpSql.AppendFormat("                    and snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("                    and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("                     and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@"               )  group by event_name order by sum(delta) desc
                       )
                    where rownum <= 5
                 ) v_rank,
                 (select snap_id,
                          event_name,
                          nvl(total_waits - lag(total_waits) over(partition by event_name order by snap_id), 0) delta_wait,
                          nvl(time_waited_micro - lag(time_waited_micro) over(partition by event_name order by snap_id), 0) delta_time
                     from raw_dba_hist_system_event_{0}
                    where wait_class != 'Idle' ", arguments.DbName);
                //tmpSql.AppendFormat(" and snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@" union all
                   select snap_id,
                          decode(stat_name, 'DB CPU', 'CPU time', stat_name) event_name,
                          NULL,
                          nvl(value - lag(value) over(partition by stat_name order by snap_id), 0)  delta
                     from raw_dba_hist_sys_time_model_{0}
                    where stat_name = 'DB CPU' ", arguments.DbName);
                //tmpSql.AppendFormat(" and snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("       and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0}", arguments.DbId);
                tmpSql.AppendFormat(@" ) v_wait
     
                where v_wait.event_name = v_rank.event_name
          ) v1,
          (select snap_id,
                   nvl(value - lag(value) over(partition by stat_name order by snap_id), 0)  delta
           from raw_dba_hist_sys_time_model_{0}
          where stat_name = 'DB time' ", arguments.DbName);
                //tmpSql.AppendFormat(" and snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@" ) v_dbtime,
               (select snap_id,
                   stat_name,
                   nvl(value - lag(value) over(partition by stat_name order by snap_id), 0)  delta
           from raw_dba_hist_sysstat_{0}
          where stat_name in ('user calls', 'recursive calls', 'execute count') ", arguments.DbName);
                //tmpSql.AppendFormat(" and snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@" ) v_call,
               (select snap_id,
                   end_interval_time
              from raw_dba_hist_snapshot_{0}", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between 1 + {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" where snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
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
                tmpSql.AppendFormat(@" select event_name||' ('|| decode(event_name,'CPU time','N/A',wait_class)||')' AS \""Wait5_Rank\"", rownum rank
           from
              (select event_name, wait_class, sum(nvl(delta, 0))
                  from
                    (select snap_id,
                             event_name,
                             wait_class,
                             nvl(time_waited_micro -
                              lag(time_waited_micro) over(partition by event_name order by snap_id), 0) delta
                        from raw_dba_hist_system_event_{0}
                       where wait_class != 'Idle' ", arguments.DbName);
                //tmpSql.AppendFormat(" and snap_id between {0} and {1} ",arguments.SnapId,arguments.SnapId);
                tmpSql.AppendFormat(" and snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0}", arguments.DbId);
                tmpSql.AppendFormat(@" union all  select snap_id,decode(stat_name, 'DB CPU', 'CPU time', stat_name) event_name,null,
                             nvl(value - lag(value) over(partition by stat_name order by snap_id), 0)  delta
                        from raw_dba_hist_sys_time_model_{0}
                       where stat_name = 'DB CPU'", arguments.DbName);
                //tmpSql.AppendFormat(" and snap_id between {0} and {1} ",arguments.SnapId,arguments.SnapId);
                tmpSql.AppendFormat(" and snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0}", arguments.InstanceNumber);
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
                tmpSql.AppendFormat(@"select  snap_id,
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
                  from raw_dba_hist_sys_time_model_{0} stm,
                       raw_dba_hist_snapshot_{0} sn
                 where stm.snap_id = sn.snap_id
                   and stm.instance_number = sn.instance_number
                   and stm.dbid = sn.dbid", arguments.DbName);
                //tmpSql.AppendFormat(" and sn.snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and sn.snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
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
                tmpSql.AppendFormat(@"select  snap_id,
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
                      from raw_dba_hist_osstat_{0} os,
                           raw_dba_hist_snapshot_{0} sn
                     where os.snap_id = sn.snap_id
                       and os.instance_number = sn.instance_number
                       and os.dbid = sn.dbid ", arguments.DbName);
                //tmpSql.AppendFormat(" and sn.snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and sn.snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
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
                tmpSql.AppendFormat(@"select  snap_id,
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
                                  from raw_dba_hist_sysstat_{0} sy
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
                               raw_dba_hist_snapshot_{0} sn
                         where v1.snap_id = sn.snap_id
                           and v1.instance_number = sn.instance_number
                           and v1.dbid = sn.dbid ", arguments.DbName);
                //tmpSql.AppendFormat(" and sn.snap_id between 1 + {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" where snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
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
                tmpSql.AppendFormat(@"select sn.snap_id,
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
                            from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1 + {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" where snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@" group by sql_id order by sum(nvl(elapsed_time_delta, 0)) desc)
                    where rownum <= 5
                 ) v_rank,
                 (select snap_id,
                          sql_id,
                          module,
                          elapsed_time_delta elap,
                          cpu_time_delta cput,
                          executions_delta exec
                     from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1 + {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" where snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@") v_sqlstat 
            where v_sqlstat.sql_id = v_rank.sql_id
          ) v1,
          (select sn.snap_id,
                   sn.end_interval_time,
                   nvl(value - lag(value) over(partition by stm.stat_name order by stm.snap_id), 0)  delta_dbtime
             from raw_dba_hist_snapshot_{0} sn,
                  raw_dba_hist_sys_time_model_{0} stm
            where sn.snap_id = stm.snap_id
               and sn.instance_number = stm.instance_number
               and sn.dbid = stm.dbid
               and stm.stat_name = 'DB time' ", arguments.DbName);
                //tmpSql.AppendFormat(" and stm.snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and stm.snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
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
                tmpSql.AppendFormat(@"select sql_id, rownum rank
                  from (select sql_id, sum(nvl(elapsed_time_delta, 0))
                          from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1 + {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" where snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
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
                tmpSql.AppendFormat(@"select rank,
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
                                from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1 + {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" where snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
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
                tmpSql.AppendFormat(@"select sn.snap_id,
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
                                            from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1 + {0} and {1} ",arguments.SnapId,arguments.SnapId);
                tmpSql.AppendFormat(" where snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid ={0} ", arguments.DbId);
                tmpSql.AppendFormat(@" group by sql_id order by sum(nvl(cpu_time_delta, 0)) desc
                )
                                    where rownum <= 5
                                 ) v_rank,
                                 (select snap_id,
                                          sql_id,
                                          module,
                                          elapsed_time_delta elap,
                                          cpu_time_delta cput,
                                          executions_delta exec
                                     from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1 + {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" where snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@" ) v_sqlstat 
                            where v_sqlstat.sql_id = v_rank.sql_id
                          ) v1,
                          (select sn.snap_id,
                                   sn.end_interval_time,
                                   nvl(value - lag(value) over(partition by stm.stat_name order by stm.snap_id), 0)  delta_dbtime
                             from raw_dba_hist_snapshot_{0} sn,
                                  raw_dba_hist_sys_time_model_{0} stm
                            where sn.snap_id = stm.snap_id
                               and sn.instance_number = stm.instance_number
                               and sn.dbid = stm.dbid
                               and stm.stat_name = 'DB time' ", arguments.DbName);
                //tmpSql.AppendFormat(" and stm.snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and stm.snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and stm.instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and stm.dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@") sn   
                    where v1.snap_id(+) = sn.snap_id
                   and sn.snap_id > {0}
                 group by sn.snap_id,
                       to_char(sn.end_interval_time, 'yyyy-mm-dd hh24:mi:ss')
                 order by sn.snap_id", arguments.SnapId);

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
                tmpSql.AppendFormat(@" select sql_id, rownum rank
                     from
                        ( select sql_id, sum(nvl(cpu_time_delta,0))
                            from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1+ {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" where snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("   and instance_number ={0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("and dbid = {0}", arguments.DbId);
                tmpSql.Append(" group by sql_id order by sum(nvl(cpu_time_delta,0)) desc) where rownum <= 5 order by rank");

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
                tmpSql.AppendFormat(@"select rank,
                       v1.sql_id,
                       v1.module,
                       nvl(replace(replace(replace(to_char(substr(st.sql_text,1,2500)),chr(9),' '),chr(10),' '),chr(13),' '), '** Not Found **') \""SQL Text\""
                  from
                      (select sql_id, dbid, module, rownum rank
                         from
                            (select sql_id, dbid, module, sum(nvl(cpu_time_delta, 0))
                                from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1 + {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" where snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("                and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@"group by sql_id, dbid, module order by sum(nvl(cpu_time_delta, 0)) desc
                    )
                        where rownum <= 5
                      ) v1,
                      dba_hist_sqltext st
                where st.sql_id(+) = v1.sql_id
                  and st.dbid(+) = v1.dbid");

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

        public void GetSql_get_01(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select sn.snap_id,
       to_char(sn.end_interval_time,'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",
       max(decode(rank, 1, bufget, 0)) rank1_bufget,
       max(decode(rank, 2, bufget, 0)) rank2_bufget,
       max(decode(rank, 3, bufget, 0)) rank3_bufget,
       max(decode(rank, 4, bufget, 0)) rank4_bufget,
       max(decode(rank, 5, bufget, 0)) rank5_bufget,
       max(decode(rank, 1, exec, 0)) rank1_exec,
       max(decode(rank, 2, exec, 0)) rank2_exec,
       max(decode(rank, 3, exec, 0)) rank3_exec,
       max(decode(rank, 4, exec, 0)) rank4_exec,
       max(decode(rank, 5, exec, 0)) rank5_exec,
       decode(max(decode(rank, 1, exec, 0)), 0, 0, max(decode(rank, 1, bufget, 0)) / max(decode(rank, 1, exec, 0))) rank1_bufget_per_exec,
       decode(max(decode(rank, 2, exec, 0)), 0, 0, max(decode(rank, 2, bufget, 0)) / max(decode(rank, 2, exec, 0))) rank2_bufget_per_exec,
       decode(max(decode(rank, 3, exec, 0)), 0, 0, max(decode(rank, 3, bufget, 0)) / max(decode(rank, 3, exec, 0))) rank3_bufget_per_exec,
       decode(max(decode(rank, 4, exec, 0)), 0, 0, max(decode(rank, 4, bufget, 0)) / max(decode(rank, 4, exec, 0))) rank4_bufget_per_exec,
       decode(max(decode(rank, 5, exec, 0)), 0, 0, max(decode(rank, 5, bufget, 0)) / max(decode(rank, 5, exec, 0))) rank5_bufget_per_exec,
       decode(max(delta_bufget_tot), 0, 0, max(decode(rank, 1, bufget, 0)) / max(delta_bufget_tot)) rank1_bufget_per_tot,
       decode(max(delta_bufget_tot), 0, 0, max(decode(rank, 2, bufget, 0)) / max(delta_bufget_tot)) rank2_bufget_per_tot,
       decode(max(delta_bufget_tot), 0, 0, max(decode(rank, 3, bufget, 0)) / max(delta_bufget_tot)) rank3_bufget_per_tot,
       decode(max(delta_bufget_tot), 0, 0, max(decode(rank, 4, bufget, 0)) / max(delta_bufget_tot)) rank4_bufget_per_tot,
       decode(max(delta_bufget_tot), 0, 0, max(decode(rank, 5, bufget, 0)) / max(delta_bufget_tot)) rank5_bufget_per_tot,
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
       max(decode(rank, 1, sql_id, null)) rank1,
       max(decode(rank, 2, sql_id, null)) rank2,
       max(decode(rank, 3, sql_id, null)) rank3,
       max(decode(rank, 4, sql_id, null)) rank4,
       max(decode(rank, 5, sql_id, null)) rank5
  from(select v_sqlstat.snap_id,
                 v_rank.rank,
                 v_sqlstat.sql_id,
                 v_sqlstat.module,
                 v_sqlstat.bufget,
                 v_sqlstat.elap,
                 v_sqlstat.cput,
                 v_sqlstat.exec
            from(select sql_id, rownum rank
                     from
                        (select sql_id, sum(nvl(buffer_gets_delta, 0))
                            from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1 + {0} and {1} ",arguments.SnapId,arguments.SnapId);
                tmpSql.AppendFormat(" where snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("             and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("             and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@"           group by sql_id order by sum(nvl(buffer_gets_delta, 0)) desc
                        )
                    where rownum <= 5
                 ) v_rank,
                 (select snap_id,
                          sql_id,
                          module,
                          buffer_gets_delta bufget,
                          elapsed_time_delta elap,
                          cpu_time_delta cput,
                          executions_delta exec
                     from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("   where snap_id between 1 + {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" where snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("     and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("      and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@" ) v_sqlstat
           where v_sqlstat.sql_id = v_rank.sql_id
          ) v1,
          (select sn.snap_id,
                   sn.end_interval_time,
                   nvl(value - lag(value) over(partition by sy.stat_name order by sy.snap_id), 0) delta_bufget_tot
             from raw_dba_hist_snapshot_{0} sn,
                  raw_dba_hist_sysstat_{0} sy
            where sn.snap_id = sy.snap_id
               and sn.instance_number = sy.instance_number
               and sn.dbid = sy.dbid
               and sy.stat_name = 'session logical reads' ", arguments.DbName);
                //tmpSql.AppendFormat(" and sy.snap_id between {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" and sy.snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and sy.instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and sy.dbid = {0} ", arguments.DbId);
                tmpSql.Append("  ) sn  where v1.snap_id(+) = sn.snap_id ");
                tmpSql.AppendFormat(" and sn.snap_id > {0} ", arguments.SnapId);
                tmpSql.AppendFormat(@"  group by sn.snap_id,  to_char(sn.end_interval_time, 'yyyy-mm-dd hh24:mi:ss') order by sn.snap_id");

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

        public void GetSql_get_02(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select sql_id, rownum rank
                     from
                        ( select sql_id, sum(nvl(buffer_gets_delta,0))
                            from raw_dba_hist_sqlstat_{0}", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1+ {0} and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" where snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.Append("   group by sql_id order by sum(nvl(buffer_gets_delta,0)) desc)  where rownum <= 5 order by rank");

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
        public void GetSql_get_03(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select rank,
                       v1.sql_id,
                       v1.module,
                       nvl(replace(replace(replace(to_char(substr(st.sql_text,1,2500)),chr(9),' '),chr(10),' '),chr(13),' '), '** Not Found **') \""SQL Text\""
                  from
                      (select sql_id, dbid, module, rownum rank
                         from
                            (select sql_id, dbid, module, sum(nvl(buffer_gets_delta, 0))
                                from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("              where snap_id between {0} + 1 and {1} ", arguments.SnapId, arguments.SnapId);
                tmpSql.AppendFormat(" where snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("                and instance_number ={0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("                 and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@" group by sql_id, dbid, module order by sum(nvl(buffer_gets_delta, 0)) desc
                            )
                        where rownum <= 5
                      ) v1,
                      dba_hist_sqltext st
                where st.sql_id(+) = v1.sql_id
                  and st.dbid(+) = v1.dbid");

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

        public void GetSql_read_01(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select sn.snap_id,
                       to_char(sn.end_interval_time,'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",
                       max(decode(rank, 1, phyrds, 0)) rank1_phyrds,
                       max(decode(rank, 2, phyrds, 0)) rank2_phyrds,
                       max(decode(rank, 3, phyrds, 0)) rank3_phyrds,
                       max(decode(rank, 4, phyrds, 0)) rank4_phyrds,
                       max(decode(rank, 5, phyrds, 0)) rank5_phyrds,
                       max(decode(rank, 1, exec, 0)) rank1_exec,
                       max(decode(rank, 2, exec, 0)) rank2_exec,
                       max(decode(rank, 3, exec, 0)) rank3_exec,
                       max(decode(rank, 4, exec, 0)) rank4_exec,
                       max(decode(rank, 5, exec, 0)) rank5_exec,
                       decode(max(decode(rank, 1, exec, 0)), 0, 0, max(decode(rank, 1, phyrds, 0)) / max(decode(rank, 1, exec, 0))) rank1_phyrds_per_exec,
                       decode(max(decode(rank, 2, exec, 0)), 0, 0, max(decode(rank, 2, phyrds, 0)) / max(decode(rank, 2, exec, 0))) rank2_phyrds_per_exec,
                       decode(max(decode(rank, 3, exec, 0)), 0, 0, max(decode(rank, 3, phyrds, 0)) / max(decode(rank, 3, exec, 0))) rank3_phyrds_per_exec,
                       decode(max(decode(rank, 4, exec, 0)), 0, 0, max(decode(rank, 4, phyrds, 0)) / max(decode(rank, 4, exec, 0))) rank4_phyrds_per_exec,
                       decode(max(decode(rank, 5, exec, 0)), 0, 0, max(decode(rank, 5, phyrds, 0)) / max(decode(rank, 5, exec, 0))) rank5_phyrds_per_exec,
                       decode(max(delta_phyrds_tot), 0, 0, max(decode(rank, 1, phyrds, 0)) / max(delta_phyrds_tot)) rank1_phyrds_per_tot,
                       decode(max(delta_phyrds_tot), 0, 0, max(decode(rank, 2, phyrds, 0)) / max(delta_phyrds_tot)) rank2_phyrds_per_tot,
                       decode(max(delta_phyrds_tot), 0, 0, max(decode(rank, 3, phyrds, 0)) / max(delta_phyrds_tot)) rank3_phyrds_per_tot,
                       decode(max(delta_phyrds_tot), 0, 0, max(decode(rank, 4, phyrds, 0)) / max(delta_phyrds_tot)) rank4_phyrds_per_tot,
                       decode(max(delta_phyrds_tot), 0, 0, max(decode(rank, 5, phyrds, 0)) / max(delta_phyrds_tot)) rank5_phyrds_per_tot,
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
                       max(decode(rank, 1, sql_id, null)) rank1,
                       max(decode(rank, 2, sql_id, null)) rank2,
                       max(decode(rank, 3, sql_id, null)) rank3,
                       max(decode(rank, 4, sql_id, null)) rank4,
                       max(decode(rank, 5, sql_id, null)) rank5
                  from(select v_sqlstat.snap_id,
                                 v_rank.rank,
                                 v_sqlstat.sql_id,
                                 v_sqlstat.module,
                                 v_sqlstat.phyrds,
                                 v_sqlstat.elap,
                                 v_sqlstat.cput,
                                 v_sqlstat.exec
                            from(select sql_id, rownum rank
                                     from
                                        (select sql_id, sum(nvl(disk_reads_delta, 0))
                                            from raw_dba_hist_sqlstat_{0} ", arguments.DbName);

                tmpSql.AppendFormat("                       where 1=1 ");
                tmpSql.Append("               AND snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("                             and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("                             and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@"  group by sql_id order by sum(nvl(disk_reads_delta, 0)) desc
                                        )
                                    where rownum <= 5
                                 ) v_rank,
                                 (select snap_id,
                                          sql_id,
                                          module,
                                          disk_reads_delta phyrds,
                                          elapsed_time_delta elap,
                                          cpu_time_delta cput,
                                          executions_delta exec
                                     from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                tmpSql.AppendFormat(" where 1=1 ");
                tmpSql.Append("               AND snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@" ) v_sqlstat
                           where v_sqlstat.sql_id = v_rank.sql_id
                          ) v1,
                          (select sn.snap_id,
                                   sn.end_interval_time,
                                   nvl(value - lag(value) over(partition by sy.stat_name order by sy.snap_id), 0) delta_phyrds_tot
                             from raw_dba_hist_snapshot_{0} sn,
                                  raw_dba_hist_sysstat_{0} sy
                            where sn.snap_id = sy.snap_id
                               and sn.instance_number = sy.instance_number
                               and sn.dbid = sy.dbid
                               and sy.stat_name = 'physical reads' ", arguments.DbName);
                tmpSql.AppendFormat(" and sy.snap_id in( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and sy.instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat(" and sy.dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@"          ) sn
                 where v1.snap_id(+) = sn.snap_id
                   and sn.snap_id > {0}", arguments.SnapId);
                tmpSql.Append(@" group by sn.snap_id,
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

        public void GetSql_read_02(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select sql_id, rownum rank
                                                 from
                                                    ( select sql_id, sum(nvl(disk_reads_delta,0))
                                                        from raw_dba_hist_sqlstat_{0}
                                                       where 1=1 ", arguments.DbName);
                tmpSql.Append("               AND snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("   and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("   and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@" group by sql_id order by sum(nvl(disk_reads_delta,0)) desc) where rownum <= 5 order by rank");

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

        public void GetSql_read_03(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select rank,
                       v1.sql_id,
                       v1.module,
                       nvl(replace(replace(replace(to_char(substr(st.sql_text,1,2500)),chr(9),' '),chr(10),' '),chr(13),' '), '** Not Found **') \""SQL Text\""
                  from
                      (select sql_id, dbid, module, rownum rank
                         from
                            (select sql_id, dbid, module, sum(nvl(disk_reads_delta, 0))
                                from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                tmpSql.AppendFormat("              where 1=1 ");
                tmpSql.Append("               AND snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("                and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("                 and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@"               group by sql_id, dbid, module order by sum(nvl(disk_reads_delta, 0)) desc)
                        where rownum <= 5
                      ) v1,
                      dba_hist_sqltext st
                where st.sql_id(+) = v1.sql_id
                  and st.dbid(+) = v1.dbid");

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

        public void GetSql_excute_01(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select sn.snap_id,
       to_char(sn.end_interval_time,'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",
       max(decode(rank, 1, exec, 0)) rank1_exec,
       max(decode(rank, 2, exec, 0)) rank2_exec,
       max(decode(rank, 3, exec, 0)) rank3_exec,
       max(decode(rank, 4, exec, 0)) rank4_exec,
       max(decode(rank, 5, exec, 0)) rank5_exec,
       max(decode(rank, 1, rows_processed, 0)) rank1_rows_processed,
       max(decode(rank, 2, rows_processed, 0)) rank2_rows_processed,
       max(decode(rank, 3, rows_processed, 0)) rank3_rows_processed,
       max(decode(rank, 4, rows_processed, 0)) rank4_rows_processed,
       max(decode(rank, 5, rows_processed, 0)) rank5_rows_processed,
       decode(max(decode(rank, 1, exec, 0)), 0, 0, max(decode(rank, 1, rows_processed, 0)) / max(decode(rank, 1, exec, 0))) rank1_rows_per_exec,
       decode(max(decode(rank, 2, exec, 0)), 0, 0, max(decode(rank, 2, rows_processed, 0)) / max(decode(rank, 2, exec, 0))) rank2_rows_per_exec,
       decode(max(decode(rank, 3, exec, 0)), 0, 0, max(decode(rank, 3, rows_processed, 0)) / max(decode(rank, 3, exec, 0))) rank3_rows_per_exec,
       decode(max(decode(rank, 4, exec, 0)), 0, 0, max(decode(rank, 4, rows_processed, 0)) / max(decode(rank, 4, exec, 0))) rank4_rows_per_exec,
       decode(max(decode(rank, 5, exec, 0)), 0, 0, max(decode(rank, 5, rows_processed, 0)) / max(decode(rank, 5, exec, 0))) rank5_rows_per_exec,
       decode(max(decode(rank, 1, exec, 0)), 0, 0, max(decode(rank, 1, cput / 1000000, 0)) / max(decode(rank, 1, exec, 0))) rank1_cpu_per_exec,
       decode(max(decode(rank, 2, exec, 0)), 0, 0, max(decode(rank, 2, cput / 1000000, 0)) / max(decode(rank, 2, exec, 0))) rank2_cpu_per_exec,
       decode(max(decode(rank, 3, exec, 0)), 0, 0, max(decode(rank, 3, cput / 1000000, 0)) / max(decode(rank, 3, exec, 0))) rank3_cpu_per_exec,
       decode(max(decode(rank, 4, exec, 0)), 0, 0, max(decode(rank, 4, cput / 1000000, 0)) / max(decode(rank, 4, exec, 0))) rank4_cpu_per_exec,
       decode(max(decode(rank, 5, exec, 0)), 0, 0, max(decode(rank, 5, cput / 1000000, 0)) / max(decode(rank, 5, exec, 0))) rank5_cpu_per_exec,
       decode(max(decode(rank, 1, exec, 0)), 0, 0, max(decode(rank, 1, elap / 1000000, 0)) / max(decode(rank, 1, exec, 0))) rank1_elap_per_exec,
       decode(max(decode(rank, 2, exec, 0)), 0, 0, max(decode(rank, 2, elap / 1000000, 0)) / max(decode(rank, 2, exec, 0))) rank2_elap_per_exec,
       decode(max(decode(rank, 3, exec, 0)), 0, 0, max(decode(rank, 3, elap / 1000000, 0)) / max(decode(rank, 3, exec, 0))) rank3_elap_per_exec,
       decode(max(decode(rank, 4, exec, 0)), 0, 0, max(decode(rank, 4, elap / 1000000, 0)) / max(decode(rank, 4, exec, 0))) rank4_elap_per_exec,
       decode(max(decode(rank, 5, exec, 0)), 0, 0, max(decode(rank, 5, elap / 1000000, 0)) / max(decode(rank, 5, exec, 0))) rank5_elap_per_exec,
       max(decode(rank, 1, sql_id, null)) rank1,
       max(decode(rank, 2, sql_id, null)) rank2,
       max(decode(rank, 3, sql_id, null)) rank3,
       max(decode(rank, 4, sql_id, null)) rank4,
       max(decode(rank, 5, sql_id, null)) rank5
  from(select v_sqlstat.snap_id,
                 v_rank.rank,
                 v_sqlstat.sql_id,
                 v_sqlstat.module,
                 v_sqlstat.rows_processed,
                 v_sqlstat.elap,
                 v_sqlstat.cput,
                 v_sqlstat.exec
            from(select sql_id, rownum rank
                     from
                        (select sql_id, sum(nvl(executions_delta, 0))
                            from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1 + {0} and {1} ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0}", arguments.DbId);

                tmpSql.AppendFormat(@"group by sql_id order by sum(nvl(executions_delta, 0)) desc
                         )
                    where rownum <= 5
                 ) v_rank,
                 (select snap_id,
                          sql_id,
                          module,
                          elapsed_time_delta elap,
                          cpu_time_delta cput,
                          executions_delta exec,
                          rows_processed_delta rows_processed
                     from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat("  and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@") v_sqlstat
 
            where v_sqlstat.sql_id = v_rank.sql_id
          ) v1,
          (select sn.snap_id,
                   sn.end_interval_time
              from raw_dba_hist_snapshot_{0} sn ", arguments.DbName);
                //tmpSql.AppendFormat(" where sn.snap_id between &snap_fr and & snap_to ");
                tmpSql.Append("               where sn.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and sn.instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat(" and sn.dbid = {0} ", arguments.DbId);
                tmpSql.Append(" ) sn  where v1.snap_id(+) - 1 = sn.snap_id ");
                tmpSql.AppendFormat(" and sn.snap_id > {0} ", arguments.SnapId);

                tmpSql.AppendFormat(@" group by sn.snap_id,   
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

        public void GetSql_excute_02(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select sql_id, rownum rank
                     from
                        ( select sql_id, sum(nvl(executions_delta,0))
                            from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("            where snap_id between 1+ &snap_fr and &snap_to ");
                tmpSql.Append("               where sn.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("             and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("             and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@"  group by sql_id order by sum(nvl(executions_delta,0)) desc)
                    where rownum <= 5
                    order by rank");

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

        public void GetSql_excute_03(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select rank,
                       v1.sql_id,
                       v1.module,
                       nvl(replace(replace(replace(to_char(substr(st.sql_text,1,2500)),chr(9),' '),chr(10),' '),chr(13),' '), '** Not Found **') \""SQL Text\""
                  from
                      (select sql_id, dbid, module, rownum rank
                         from
                            (select sql_id, dbid, module, sum(nvl(executions_delta, 0))
                                from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("   where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.Append("               where sn.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("    and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("            and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@"              group by sql_id, dbid, module order by sum(nvl(executions_delta, 0)) desc
                          )
                        where rownum <= 5
                      ) v1,
                      dba_hist_sqltext st
                where st.sql_id(+) = v1.sql_id
                  and st.dbid(+) = v1.dbid");

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

        public void GetSql_parse_01(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select sn.snap_id,
                       to_char(sn.end_interval_time,'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",
                       max(decode(rank, 1, parse, 0)) rank1_parse,
                       max(decode(rank, 2, parse, 0)) rank2_parse,
                       max(decode(rank, 3, parse, 0)) rank3_parse,
                       max(decode(rank, 4, parse, 0)) rank4_parse,
                       max(decode(rank, 5, parse, 0)) rank5_parse,
                       max(decode(rank, 1, exec, 0)) rank1_exec,
                       max(decode(rank, 2, exec, 0)) rank2_exec,
                       max(decode(rank, 3, exec, 0)) rank3_exec,
                       max(decode(rank, 4, exec, 0)) rank4_exec,
                       max(decode(rank, 5, exec, 0)) rank5_exec,
                       decode(max(decode(rank, 1, sn.delta_parse_tot, 0)), 0, 0, max(decode(rank, 1, parse, 0)) / max(decode(rank, 1, sn.delta_parse_tot, 0))) rank1_parse_per_tot,
                       decode(max(decode(rank, 2, sn.delta_parse_tot, 0)), 0, 0, max(decode(rank, 2, parse, 0)) / max(decode(rank, 2, sn.delta_parse_tot, 0))) rank2_parse_per_tot,
                       decode(max(decode(rank, 3, sn.delta_parse_tot, 0)), 0, 0, max(decode(rank, 3, parse, 0)) / max(decode(rank, 3, sn.delta_parse_tot, 0))) rank3_parse_per_tot,
                       decode(max(decode(rank, 4, sn.delta_parse_tot, 0)), 0, 0, max(decode(rank, 4, parse, 0)) / max(decode(rank, 4, sn.delta_parse_tot, 0))) rank4_parse_per_tot,
                       decode(max(decode(rank, 5, sn.delta_parse_tot, 0)), 0, 0, max(decode(rank, 5, parse, 0)) / max(decode(rank, 5, sn.delta_parse_tot, 0))) rank5_parse_per_tot,
                       max(decode(rank, 1, sql_id, null)) rank1,
                       max(decode(rank, 2, sql_id, null)) rank2,
                       max(decode(rank, 3, sql_id, null)) rank3,
                       max(decode(rank, 4, sql_id, null)) rank4,
                       max(decode(rank, 5, sql_id, null)) rank5
                  from(select v_sqlstat.snap_id,
                                 v_rank.rank,
                                 v_sqlstat.sql_id,
                                 v_sqlstat.module,
                                 v_sqlstat.parse,
                                 v_sqlstat.exec
                            from(select sql_id, rownum rank
                                     from
                                        (select sql_id, sum(nvl(parse_calls_delta, 0))
                                            from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("   and instance_number = {0}", arguments.InstanceNumber);

                tmpSql.AppendFormat("                             and dbid = {0} ", arguments.DbId);

                tmpSql.AppendFormat(@" group by sql_id order by sum(nvl(parse_calls_delta, 0)) desc
                                       )
                                    where rownum <= 5
                                 ) v_rank,
                                 (select snap_id,
                                          sql_id,
                                          module,
                                          parse_calls_delta parse,
                                          executions_delta exec
                                     from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("     where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("      and instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat("                     and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@"  ) v_sqlstat
                          where v_sqlstat.sql_id = v_rank.sql_id
                          ) v1,
                          (select sn.snap_id,
                                   sn.end_interval_time,
                                   nvl(value - lag(value) over(partition by sy.stat_name order by sy.snap_id), 0) delta_parse_tot
                             from raw_dba_hist_snapshot_{0} sn,
                                  raw_dba_hist_sysstat_{0} sy
                            where sn.snap_id = sy.snap_id
                               and sn.instance_number = sy.instance_number
                               and sn.dbid = sy.dbid
                               and sy.stat_name = 'parse count (total)' ", arguments.DbName);
                //tmpSql.AppendFormat("  and sy.snap_id between &snap_fr and & snap_to ");
                tmpSql.Append("               and sy.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and sy.instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("   and sy.dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat("   ) sn    where v1.snap_id(+) = sn.snap_id ");
                tmpSql.AppendFormat("   and sn.snap_id > {0} ", arguments.SnapId);
                tmpSql.Append(@"  group by sn.snap_id,
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

        public void GetSql_parse_02(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select sql_id, rownum rank
                     from
                        ( select sql_id, sum(nvl(parse_calls_delta,0))
                            from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("    where snap_id between 1+ &snap_fr and &snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("     and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("          and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@"        group by sql_id order by sum(nvl(parse_calls_delta,0)) desc                       )
                    where rownum <= 5
                order by rank");

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

        public void GetSql_parse_03(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select rank,
                       v1.sql_id,
                       v1.module,
                       nvl(replace(replace(replace(to_char(substr(st.sql_text,1,2500)),chr(9),' '),chr(10),' '),chr(13),' '), '** Not Found **') \""SQL Text\""
                  from
                      (select sql_id, dbid, module, rownum rank
                         from
                            (select sql_id, dbid, module, sum(nvl(parse_calls_delta, 0))
                                from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@" group by sql_id, dbid, module order by sum(nvl(parse_calls_delta, 0)) desc
                           )
                        where rownum <= 5
                      ) v1,
                      dba_hist_sqltext st
                where st.sql_id(+) = v1.sql_id
                  and st.dbid(+) = v1.dbid");

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


        public void GetSql_clu_wait_01(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select sn.snap_id,
                       to_char(sn.end_interval_time,'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",
                       max(decode(rank, 1, clwait / 1000000, 0)) rank1_clwait,
                       max(decode(rank, 2, clwait / 1000000, 0)) rank2_clwait,
                       max(decode(rank, 3, clwait / 1000000, 0)) rank3_clwait,
                       max(decode(rank, 4, clwait / 1000000, 0)) rank4_clwait,
                       max(decode(rank, 5, clwait / 1000000, 0)) rank5_clwait,
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
                       decode(max(decode(rank, 1, elap, 0)), 0, 0, max(decode(rank, 1, clwait, 0)) / max(decode(rank, 1, elap, 0))) rank1_clwait_per_elap,
                       decode(max(decode(rank, 2, elap, 0)), 0, 0, max(decode(rank, 2, clwait, 0)) / max(decode(rank, 2, elap, 0))) rank2_clwait_per_elap,
                       decode(max(decode(rank, 3, elap, 0)), 0, 0, max(decode(rank, 3, clwait, 0)) / max(decode(rank, 3, elap, 0))) rank3_clwait_per_elap,
                       decode(max(decode(rank, 4, elap, 0)), 0, 0, max(decode(rank, 4, clwait, 0)) / max(decode(rank, 4, elap, 0))) rank4_clwait_per_elap,
                       decode(max(decode(rank, 5, elap, 0)), 0, 0, max(decode(rank, 5, clwait, 0)) / max(decode(rank, 5, elap, 0))) rank5_clwait_per_elap,
                       max(decode(rank, 1, sql_id, null)) rank1,
                       max(decode(rank, 2, sql_id, null)) rank2,
                       max(decode(rank, 3, sql_id, null)) rank3,
                       max(decode(rank, 4, sql_id, null)) rank4,
                       max(decode(rank, 5, sql_id, null)) rank5
                  from(select v_sqlstat.snap_id,
                                 v_rank.rank,
                                 v_sqlstat.sql_id,
                                 v_sqlstat.module,
                                 v_sqlstat.clwait,
                                 v_sqlstat.elap,
                                 v_sqlstat.cput,
                                 v_sqlstat.exec
                            from(select sql_id, rownum rank
                                     from
                                        (select sql_id, sum(nvl(clwait_delta, 0))
                                            from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("    where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat(" and dbid ={0} ", arguments.DbId);
                tmpSql.AppendFormat(@"   group by sql_id order by sum(nvl(clwait_delta, 0)) desc
               )
                                    where rownum <= 5
                                 ) v_rank,
                                 (select snap_id,
                                          sql_id,
                                          module,
                                          elapsed_time_delta elap,
                                          cpu_time_delta cput,
                                          executions_delta exec,
                                          clwait_delta clwait
                                     from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1 + &snap_fr and & snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@" ) v_sqlstat   
                              where v_sqlstat.sql_id = v_rank.sql_id
                          ) v1,
                          (select sn.snap_id,
                                   sn.end_interval_time
                              from raw_dba_hist_snapshot_{0} sn ", arguments.DbName);
                //tmpSql.AppendFormat(" where sn.snap_id between &snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and sn.instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat(" and sn.dbid = {0} ", arguments.DbId);
                tmpSql.Append(@" ) sn  where v1.snap_id(+) = sn.snap_id ");
                tmpSql.AppendFormat(" and sn.snap_id > {0} ", arguments.SnapId);

                tmpSql.Append(@"  group by sn.snap_id,   
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

        public void GetSql_clu_wait_02(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select sql_id, rownum rank
                     from
                        ( select sql_id, sum(nvl(clwait_delta,0))
                            from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between 1+ &snap_fr and &snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("   and instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat("   and dbid = {0} ", arguments.DbId);

                tmpSql.AppendFormat(@"  group by sql_id order by sum(nvl(clwait_delta,0)) desc) where rownum <= 5 order by rank");

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

        public void GetSql_clu_wait_03(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select rank,
                       v1.sql_id,
                       v1.module,
                       nvl(replace(replace(replace(to_char(substr(st.sql_text,1,2500)),chr(9),' '),chr(10),' '),chr(13),' '), '** Not Found **') \""SQL Text\""
                  from
                      (select sql_id, dbid, module, rownum rank
                         from
                            (select sql_id, dbid, module, sum(nvl(clwait_delta, 0))
                                from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("    where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@"   group by sql_id, dbid, module order by sum(nvl(clwait_delta, 0)) desc
                           )
                        where rownum <= 5
                      ) v1,
                      dba_hist_sqltext st
                where st.sql_id(+) = v1.sql_id
                  and st.dbid(+) = v1.dbid");

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
        /// <summary>
        /// 54
        /// </summary>
        /// <param name="arguments"></param>
        public void GetPga(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select sn.snap_id \""SnapID\"",
                       sn.snap_time \""Timestamp\"",
                       max(decode(pga.name, 'aggregate PGA target parameter', value / 1024 / 1024, 0))            \""PGA Aggr Target(M)\"",
                       max(decode(pga.name, 'aggregate PGA auto target', value / 1024 / 1024, 0))                 \""Auto PGA Target(M)\"",
                       max(decode(pga.name, 'total PGA allocated', value / 1024 / 1024, 0))                       \""PGA Mem Alloc(M)\"",
                       max(decode(pga.name, 'total PGA used for auto workareas', value / 1024 / 1024, 0))         \""Auto W/A(M)\"",
                       max(decode(pga.name, 'total PGA used for manual workareas', value / 1024 / 1024, 0))       \""Manual W/A(M)\"",
                       decode(max(decode(pga.name, 'total PGA allocated', value, 0)), 0, 0,
                             (max(decode(pga.name, 'total PGA used for auto workareas', value, 0))
                             + max(decode(pga.name, 'total PGA used for manual workareas', value, 0))
                             ) / max(decode(pga.name, 'total PGA allocated', value, 0)))                        \""%PGA W/A Mem\"",
                       decode(max(decode(pga.name, 'total PGA used for auto workareas', value, 0))
                             + max(decode(pga.name, 'total PGA used for manual workareas', value, 0)), 0, 0,
                              max(decode(pga.name, 'total PGA used for auto workareas', value, 0))
                              / (max(decode(pga.name, 'total PGA used for auto workareas', value, 0))
                               + max(decode(pga.name, 'total PGA used for manual workareas', value, 0))))       \""%Auto W/A Mem\"",
                       decode(max(decode(pga.name, 'total PGA used for auto workareas', value, 0))
                             + max(decode(pga.name, 'total PGA used for manual workareas', value, 0)), 0, 0,
                              max(decode(pga.name, 'total PGA used for manual workareas', value, 0))
                              / (max(decode(pga.name, 'total PGA used for auto workareas', value, 0))
                               + max(decode(pga.name, 'total PGA used for manual workareas', value, 0))))       \""%Manual W/A Mem\"",
                       max(decode(pga.name, 'global memory bound', value / 1024, 0))                             \""Global Mem Bound(K)\"",
                       max(case when pga.name = 'maximum PGA used for auto workareas' then value else 0 end)/ 1048576   \""max. auto W/A(M)\"",
                       max(case when pga.name = 'maximum PGA used for manual workareas' then value else 0 end)/ 1048576 \""max. manual W/A(M)\""
                 from(
                       select dbid, instance_number, snap_id, to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time
                         from raw_dba_hist_snapshot_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("   and instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat("         and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@"    ) sn,
                   raw_dba_hist_pgastat_{0} pga
                where pga.snap_id = sn.snap_id
                  and pga.instance_number = sn.instance_number
                  and pga.dbid = sn.dbid
                 group by sn.snap_id, sn.snap_time
                 order by sn.snap_id", arguments.DbName);

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

        /// <summary>
        /// 54
        /// </summary>
        /// <param name="arguments"></param>
        public void GetPga_hit(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select  sn.snap_id \""SnapID\"",
                        sn.snap_time \""Timestamp\"",
                        sum(decode(name, 'bytes processed', delta, 0))
                            / (sum(decode(name, 'bytes processed', delta, 0))
                               + sum(decode(name, 'extra bytes read/written', delta, 0))
                              ) \""PGA Cache Hit %\"",
                        sum(decode(name, 'bytes processed', round(delta / 1024 / 1024), 0)) \""W/A MB Processed\"",
                        sum(decode(name, 'extra bytes read/written', round(delta / 1024 / 1024), 0)) \""Extra W/A MB Read/Write\""
                  from(
                         select snap_id,
                                name,
                                value - lag(value) over(partition by name order by snap_id) delta
                           from raw_dba_hist_pgastat_{0}
                          where name in ('bytes processed', 'extra bytes read/written') ", arguments.DbName);
                //tmpSql.AppendFormat("  and snap_id between {0} and {1} ");
                tmpSql.Append("               and snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("   and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@" ) v1,
                          (
                       select snap_id, to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time
                         from raw_dba_hist_snapshot_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat("        and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@"        ) sn
                 where v1.snap_id = sn.snap_id
                group by sn.snap_id, sn.snap_time
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

        /// <summary>
        /// 55
        /// </summary>
        /// <param name="arguments"></param>
        public void GetWorkarea(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select 	b.snap_id,
		                    b.snap_time,
		                    a.total_executions,
		                    a.optimal_executions,
		                    a.onepass_executions,
		                    a.multipasses_executions
                    from
		                    (select 	snap_id,
        	                    sum(total_executions)   - lag(sum(total_executions)) over (order by snap_id) total_executions,
        	                    sum(optimal_executions) - lag(sum(optimal_executions)) over (order by snap_id) optimal_executions,
        	                    sum(onepass_executions) - lag(sum(onepass_executions)) over (order by snap_id) onepass_executions,
        	                    sum(multipasses_executions) - lag(sum(multipasses_executions)) over (order by snap_id) multipasses_executions
                             from raw_DBA_HIST_SQL_WORKAREA_HSTGRM_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between &snap_fr and &snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);

                tmpSql.AppendFormat(@"  group by snap_id) a,
                         (select snap_id,
                                  to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time,
                                  to_number(substr((end_interval_time-begin_interval_time)*86400,2,9)) interval
                             from raw_dba_hist_snapshot_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1+ &snap_fr and &snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0}) b ", arguments.DbId);
                tmpSql.AppendFormat(" where a.snap_id=b.snap_id  order by b.snap_id");

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

        /// <summary>
        /// 55
        /// </summary>
        /// <param name="arguments"></param>
        public void GetWorkarea_raw(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select sn.snap_id \""SnapID\""
                      , snap_time  \""Timestamp\""
                      ,case when low_optimal_size >= 1024 * 1024 * 1024 * 1024
                            then lpad(round(low_optimal_size / 1024 / 1024 / 1024 / 1024) || 'T', 7)
                            when low_optimal_size >= 1024 * 1024 * 1024
                            then lpad(round(low_optimal_size / 1024 / 1024 / 1024) || 'G', 7)
                            when low_optimal_size >= 1024 * 1024
                            then lpad(round(low_optimal_size / 1024 / 1024) || 'M', 7)
                            when low_optimal_size >= 1024
                            then lpad(round(low_optimal_size / 1024) || 'K', 7)
                            else lpad(low_optimal_size || 'B', 7)
                       end                    \""Low Optimal\""
                      ,case when high_optimal_size >= 1024 * 1024 * 1024 * 1024
                            then lpad(round(high_optimal_size/ 1024 / 1024 / 1024 / 1024) || 'T',7)
                            when high_optimal_size >= 1024 * 1024 * 1024
                            then lpad(round(high_optimal_size/ 1024 / 1024 / 1024) || 'G',7)
                            when high_optimal_size >= 1024 * 1024
                            then lpad(round(high_optimal_size/ 1024 / 1024) || 'M',7)
                            when high_optimal_size >= 1024
                            then lpad(round(high_optimal_size/ 1024) || 'K',7)
                            else high_optimal_size || 'B'
                       end                           \""High Optimal\""
                      ,nvl(total_executions, 0)       \""tot_exe\""
                      ,nvl(optimal_executions, 0)     \""opt_exe\""
                      ,nvl(onepass_executions, 0)     \""one_exe\""
                      ,nvl(multipasses_executions, 0) \""mul_exe\""
                  from(select  snap_id, low_optimal_size, high_optimal_size,
                            total_executions - lag(total_executions) over(partition by low_optimal_size, high_optimal_size order by snap_id) total_executions,
                            optimal_executions - lag(optimal_executions) over(partition by low_optimal_size, high_optimal_size order by snap_id) optimal_executions,
                            onepass_executions - lag(onepass_executions) over(partition by low_optimal_size, high_optimal_size order by snap_id) onepass_executions,
                            multipasses_executions - lag(multipasses_executions) over(partition by low_optimal_size, high_optimal_size order by snap_id) multipasses_executions
                         from raw_DBA_HIST_SQL_WORKAREA_HSTGRM_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between & snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("   and instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat("         and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@"      ) v1,
                      (
                       select snap_id,
                              to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time,
                              to_number(substr((end_interval_time - begin_interval_time) * 86400, 2, 9)) interval
                               from raw_dba_hist_snapshot_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@" ) sn      
                       where v1.snap_id = sn.snap_id
                 order by low_optimal_size");

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

        /// <summary>
        /// 55
        /// </summary>
        /// <param name="arguments"></param>
        public void GetWorkarea_histgram(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select e.snap_id \""SnapID\""
                      , to_char(s.end_interval_time, 'yyyy-mm-dd hh24:mi:ss') \""Timestamp\""
                      ,case when e.low_optimal_size >= 1024 * 1024 * 1024 * 1024
                            then lpad(round(e.low_optimal_size / 1024 / 1024 / 1024 / 1024) || 'T', 7)
                            when e.low_optimal_size >= 1024 * 1024 * 1024
                            then lpad(round(e.low_optimal_size / 1024 / 1024 / 1024) || 'G', 7)
                            when e.low_optimal_size >= 1024 * 1024
                            then lpad(round(e.low_optimal_size / 1024 / 1024) || 'M', 7)
                            when e.low_optimal_size >= 1024
                            then lpad(round(e.low_optimal_size / 1024) || 'K', 7)
                            else lpad(e.low_optimal_size || 'B', 7)
                       end                                             \""Low Optimal\""
                     , case when e.high_optimal_size >= 1024 * 1024 * 1024 * 1024
                            then lpad(round(e.high_optimal_size/ 1024 / 1024 / 1024 / 1024) || 'T',7)
                            when e.high_optimal_size >= 1024 * 1024 * 1024
                            then lpad(round(e.high_optimal_size/ 1024 / 1024 / 1024) || 'G',7)
                            when e.high_optimal_size >= 1024 * 1024
                            then lpad(round(e.high_optimal_size/ 1024 / 1024) || 'M',7)
                            when e.high_optimal_size >= 1024
                            then lpad(round(e.high_optimal_size/ 1024) || 'K',7)
                            else e.high_optimal_size || 'B'
                       end                                              \""High Optimal\""
                     , e.total_executions - nvl(b.total_executions, 0)        \""tot_exe\""
                     , e.optimal_executions - nvl(b.optimal_executions, 0)      \""opt_exe\""
                     , e.onepass_executions - nvl(b.onepass_executions, 0)      \""one_exe\""
                     , e.multipasses_executions - nvl(b.multipasses_executions, 0)  \""mul_exe\""
                  from raw_DBA_HIST_SQL_WORKAREA_HSTGRM_{0} e
                     , raw_DBA_HIST_SQL_WORKAREA_HSTGRM_{0} b
                     , raw_dba_hist_snapshot_{0} S ", arguments.DbName);
                tmpSql.AppendFormat(" where e.snap_id = {0} ", arguments.SnapId);

                tmpSql.AppendFormat("   and e.dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(" and e.instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat(" and b.snap_id(+) = {0} ", arguments.SnapId);
                tmpSql.Append(@" and b.dbid(+) = e.dbid

                and b.instance_number(+) = e.instance_number
                   and b.low_optimal_size(+) = e.low_optimal_size
                   and b.high_optimal_size(+) = e.high_optimal_size
                   and e.snap_id = s.snap_id ");
                tmpSql.AppendFormat(" and s.INSTANCE_NUMBER = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat(" and s.dbid = {0} ", arguments.DbId);
                //tmpSql.AppendFormat(" and s.snap_id between  &snap_fr and & snap_to ");
                tmpSql.Append("               and s.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.Append("order by e.low_optimal_size");

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

        /// <summary>
        ///61
        /// </summary>
        /// <param name="arguments"></param>
        public void GetWaitstatm(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select snap_id \""SnapID\"",
       snap_time \""Timestamp\"",
       max(decode(class, '1st level bmb'        , delta, 0)) \""1st level bmb\"",
       max(decode(class, '2nd level bmb'        , delta, 0)) \""2nd level bmb\"",
       max(decode(class, '3rd level bmb'        , delta, 0)) \""3rd level bmb\"",
       max(decode(class, 'bitmap block'         , delta, 0)) \""bitmap block\"",
       max(decode(class, 'bitmap index block'   , delta, 0)) \""bitmap index block\"",
       max(decode(class, 'data block'           , delta, 0)) \""data block\"",
       max(decode(class, 'extent map'           , delta, 0)) \""extent map\"",
       max(decode(class, 'file header block'    , delta, 0)) \""file header block\"",
       max(decode(class, 'free list'            , delta, 0)) \""free list\"",
       max(decode(class, 'save undo block'      , delta, 0)) \""save undo block\"",
       max(decode(class, 'save undo header'     , delta, 0)) \""save undo header\"",
       max(decode(class, 'segment header'       , delta, 0)) \""segment header\"",
       max(decode(class, 'sort block'           , delta, 0)) \""sort block\"",
       max(decode(class, 'system undo block'    , delta, 0)) \""system undo block\"",
       max(decode(class, 'system undo header'   , delta, 0)) \""system undo header\"",
       max(decode(class, 'undo block'           , delta, 0)) \""undo block\"",
       max(decode(class, 'undo header'          , delta, 0)) \""undo header\"",
       max(decode(class, '1st level bmb'        , delta_cnt, 0)) \""1st level bmb\"",
       max(decode(class, '2nd level bmb'        , delta_cnt, 0)) \""2nd level bmb\"",
       max(decode(class, '3rd level bmb'        , delta_cnt, 0)) \""3rd level bmb\"",
       max(decode(class, 'bitmap block'         , delta_cnt, 0)) \""bitmap block\"",
       max(decode(class, 'bitmap index block'   , delta_cnt, 0)) \""bitmap index block\"",
       max(decode(class, 'data block'           , delta_cnt, 0)) \""data block\"",
       max(decode(class, 'extent map'           , delta_cnt, 0)) \""extent map\"",
       max(decode(class, 'file header block'    , delta_cnt, 0)) \""file header block\"",
       max(decode(class, 'free list'            , delta_cnt, 0)) \""free list\"",
       max(decode(class, 'save undo block'      , delta_cnt, 0)) \""save undo block\"",
       max(decode(class, 'save undo header'     , delta_cnt, 0)) \""save undo header\"",
       max(decode(class, 'segment header'       , delta_cnt, 0)) \""segment header\"",
       max(decode(class, 'sort block'           , delta_cnt, 0)) \""sort block\"",
       max(decode(class, 'system undo block'    , delta_cnt, 0)) \""system undo block\"",
       max(decode(class, 'system undo header'   , delta_cnt, 0)) \""system undo header\"",
       max(decode(class, 'undo block'           , delta_cnt, 0)) \""undo block\"",
       max(decode(class, 'undo header'          , delta_cnt, 0)) \""undo header\""
 from(
      select class,snap_id,snap_time,delta_cnt,delta_time,decode(delta_cnt,0,0, delta_time/delta_cnt)*10 delta
         from(
              select class,
                    ws.snap_id,
                    to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time,
                    time-lag(time) over(partition by class order by ws.snap_id) delta_time,
                    wait_count-lag(wait_count) over(partition by class order by ws.snap_id) delta_cnt
              from raw_dba_hist_waitstat_{0} ws,
                   raw_dba_hist_snapshot_{0} sn
             where ws.snap_id = sn.snap_id
               and ws.instance_number = sn.instance_number
               and ws.dbid = sn.dbid ", arguments.DbName);
                //tmpSql.AppendFormat(" and ws.snap_id between &snap_fr and &snap_to ");
                tmpSql.Append("               and ws.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and ws.instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and ws.dbid = {0} ", arguments.DbId);

                tmpSql.AppendFormat(@" and class in (
 
                        '1st level bmb',
                       '2nd level bmb',
                       '3rd level bmb',
                       'bitmap block',
                       'bitmap index block',
                       'data block',
                       'extent map',
                       'file header block',
                       'free list',
                       'save undo block',
                       'save undo header',
                       'segment header',
                       'sort block',
                       'system undo block',
                       'system undo header',
                       'undo block',
                       'undo header'
                      )
            ) ");
                tmpSql.AppendFormat(" where snap_id > {0} ", arguments.SnapId);
                tmpSql.Append(@" ) group by snap_id, snap_time order by snap_id");

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        public void GetResource(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select sn.snap_id \""SnapID\"",
                       to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",
                       max(decode(r.resource_name, 'processes', current_utilization, 0)) processes_curr,
                       max(decode(r.resource_name, 'processes', max_utilization, 0)) processes_max,
                       max(decode(r.resource_name, 'processes', limit_value, 0)) processes_limit,
                       max(decode(r.resource_name, 'sessions', current_utilization, 0)) sessions_curr,
                       max(decode(r.resource_name, 'sessions', max_utilization, 0)) sessions_max,
                       max(decode(r.resource_name, 'sessions', limit_value, 0)) sessions_limit,
                       max(active_sess.cnt) active_session_cnt,
                       max(decode(r.resource_name, 'enqueue_locks', current_utilization, 0)) enqueue_curr,
                       max(decode(r.resource_name, 'enqueue_locks', max_utilization, 0)) enqueue_max,
                       max(decode(r.resource_name, 'enqueue_locks', limit_value, 0)) enqueue_limit,
                       max(decode(r.resource_name, 'max_rollback_segments', current_utilization, 0)) max_rollback_segments_curr,
                       max(decode(r.resource_name, 'max_rollback_segments', max_utilization, 0)) max_rollback_segments_max,
                       max(decode(r.resource_name, 'max_rollback_segments', limit_value, 0)) max_rollback_segments_limit,
                       max(decode(r.resource_name, 'parallel_max_servers', current_utilization, 0)) parallel_max_servers_curr,
                       max(decode(r.resource_name, 'parallel_max_servers', max_utilization, 0)) parallel_max_servers_max,
                       max(decode(r.resource_name, 'parallel_max_servers', limit_value, 0)) parallel_max_servers_limit,
                       max(decode(r.resource_name, 'gcs_resources', current_utilization, 0)) gcs_resources_curr,
                       max(decode(r.resource_name, 'gcs_resources', max_utilization, 0)) gcs_resources_max,
                       max(decode(r.resource_name, 'gcs_resources', limit_value, 0)) gcs_resources_limit,
                       max(decode(r.resource_name, 'gcs_shadows', current_utilization, 0)) gcs_shadows_curr,
                       max(decode(r.resource_name, 'gcs_shadows', max_utilization, 0)) gcs_shadows_max,
                       max(decode(r.resource_name, 'gcs_shadows', limit_value, 0)) gcs_shadows_limit,
                       max(decode(r.resource_name, 'ges_procs', current_utilization, 0)) ges_procs_curr,
                       max(decode(r.resource_name, 'ges_procs', max_utilization, 0)) ges_procs_max,
                       max(decode(r.resource_name, 'ges_procs', limit_value, 0)) ges_procs_limit
                  from raw_dba_hist_snapshot_{0} sn,
                       dba_hist_resource_limit  r,
                       (select snap_id, count(sample_id) cnt
                          from(
                                select snap_id, sample_id,
                                       max(sample_id) over(partition by snap_id) max_sample_id
                                  from raw_dba_hist_active_sess_history_{0} ash ", arguments.DbName);
                //tmpSql.AppendFormat("   where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0}", arguments.DbId);

                tmpSql.AppendFormat(" and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.Append(@" )

                        where sample_id = max_sample_id
                         group by snap_id) active_sess
                where sn.snap_id = r.snap_id
                  and sn.instance_number = r.instance_number
                  and sn.dbid = r.dbid
                  and sn.snap_id = active_sess.snap_id
                  and r.resource_name in ('processes',
                                          'sessions',
                                          'enqueue_locks',
                                          'max_rollback_segments',
                                          'parallel_max_servers',
                                          'gcs_resources',
                                          'gcs_shadows',
                                          'ges_procs') ");
                //tmpSql.AppendFormat(" and r.snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.Append("               and r.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and r.instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and r.dbid = {0} ", arguments.DbId);

                tmpSql.AppendFormat("  group by sn.snap_id, to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss')  order by sn.snap_id");

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

        /// <summary>
        /// 64
        /// </summary>
        /// <param name="arguments"></param>
        public void GetBuffer_pool(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select 	  sn.snap_id \""SnapID\""
                          , snap_time \""Timestamp\""
                          , block_size / 1024 || 'k' || substr(name, 1, 1) \""Pools\""
                          , lpad(case
                              when set_msize <= 9999
                                   then to_char(set_msize) || ' '
                              when trunc((set_msize) / 1000) <= 9999
                                   then to_char(trunc((set_msize) / 1000)) || 'K'
                              when trunc((set_msize) / 1000000) <= 9999
                                   then to_char(trunc((set_msize) / 1000000)) || 'M'
                              when trunc((set_msize) / 1000000000) <= 9999
                                   then to_char(trunc((set_msize) / 1000000000)) || 'G'
                              when trunc((set_msize) / 1000000000000) <= 9999
                                   then to_char(trunc((set_msize) / 1000000000000)) || 'T'
                              else substr(to_char(trunc((set_msize) / 1000000000000000)) || 'P', 1, 5) end , 7, ' ') \""numbufs\""
                    , decode(db_block_gets + consistent_gets, 0, 0, 1 - (physical_reads / (db_block_gets + consistent_gets))) \""%Hit\""
                         , db_block_gets + consistent_gets \""logical reads\""
                         , physical_reads        \""ph.read\""
                         , physical_writes       \""ph.write\""
                         , free_buffer_wait      \""free buffer wait\""
                         , write_complete_wait   \""write complete wait\""
                         , buffer_busy_wait  \""buffer busy wait\""
                 from(
                     select  snap_id, block_size, name, set_msize,
                         db_block_gets - lag(db_block_gets)    over(partition by block_size order  by snap_id) db_block_gets,
                        consistent_gets - lag(consistent_gets)  over(partition by block_size order  by snap_id) consistent_gets,
                        physical_reads - lag(physical_reads)   over(partition by block_size order  by snap_id) physical_reads,
                        physical_writes - lag(physical_writes)  over(partition by block_size order  by snap_id) physical_writes,
                        free_buffer_wait - lag(free_buffer_wait) over(partition by block_size order  by snap_id) free_buffer_wait,
                        write_complete_wait - lag(write_complete_wait) over(partition by block_size order  by snap_id) write_complete_wait,
                        buffer_busy_wait - lag(buffer_busy_wait) over(partition by block_size order  by snap_id) buffer_busy_wait

                     from raw_DBA_HIST_BUFFER_POOL_STAT_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("    where snap_id between & snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and   instance_number = {0}", arguments.InstanceNumber);

                tmpSql.AppendFormat(" and   dbid = {0} ", arguments.DbId);


                tmpSql.AppendFormat(@" and   name = 'DEFAULT'
                    ) v1,
                       (
                       select snap_id,
                              to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time,
                              to_number(substr((end_interval_time - begin_interval_time) * 86400, 2, 9)) interval
                               from raw_dba_hist_snapshot_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.Append(@" ) sn
                 where v1.snap_id = sn.snap_id
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
        /// <summary>
        /// 64
        /// </summary>
        /// <param name="arguments"></param>
        public void GetBuffer_pool_other(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select e.snap_id \""SnapID\""
                      , to_char(s.end_interval_time, 'yyyy-mm-dd hh24:mi:ss') \""Timestamp\""
                      , e.block_size / 1024 || 'k' || substr(e.name, 1, 1) \""Pools\""
                      , lpad(case
                              when e.set_msize <= 9999
                                   then to_char(e.set_msize) || ' '
                              when trunc((e.set_msize) / 1000) <= 9999
                                   then to_char(trunc((e.set_msize) / 1000)) || 'K'
                              when trunc((e.set_msize) / 1000000) <= 9999
                                   then to_char(trunc((e.set_msize) / 1000000)) || 'M'
                              when trunc((e.set_msize) / 1000000000) <= 9999
                                   then to_char(trunc((e.set_msize) / 1000000000)) || 'G'
                              when trunc((e.set_msize) / 1000000000000) <= 9999
                                   then to_char(trunc((e.set_msize) / 1000000000000)) || 'T'
                              else substr(to_char(trunc((e.set_msize) / 1000000000000000)) || 'P', 1, 5) end
                            , 7, ' ') \""numbufs\""
                            , decode(e.db_block_gets - nvl(b.db_block_gets, 0)
                              + e.consistent_gets - nvl(b.consistent_gets, 0)
                             , 0, 0
                              , (1 - ((e.physical_reads - nvl(b.physical_reads, 0))
                                            / (e.db_block_gets - nvl(b.db_block_gets, 0)
                                              + e.consistent_gets - nvl(b.consistent_gets, 0))
                                     )
                                )
                             )                                                \""%Hit\""
                     ,    e.db_block_gets - nvl(b.db_block_gets, 0)
                       + e.consistent_gets - nvl(b.consistent_gets, 0)       \""buffer gets\""
                     , e.physical_reads - nvl(b.physical_reads, 0)        \""ph. read\""
                     , e.physical_writes - nvl(b.physical_writes, 0)       \""ph. write\""
                     , e.free_buffer_wait - nvl(b.free_buffer_wait, 0)      \""free buffer wait\""
                     , e.write_complete_wait - nvl(b.write_complete_wait, 0)   \""write complete wait\""
                     , e.buffer_busy_wait - nvl(b.buffer_busy_wait, 0)      \""buffer busy wait\""
                 from raw_DBA_HIST_BUFFER_POOL_STAT_{0} b
                     ,raw_DBA_HIST_BUFFER_POOL_STAT_{0} e
                     , raw_dba_hist_snapshot_{0} S
                  where b.snap_id(+) = e.snap_id - 1", arguments.DbName);
                //tmpSql.AppendFormat("  and e.snap_id between  &snap_fr and & snap_to");
                tmpSql.Append("               and e.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and e.dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(" and b.dbid(+) = {0} ", arguments.DbId);
                tmpSql.AppendFormat(" and b.instance_number(+) = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and e.instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat(@" and b.instance_number(+) = e.instance_number
                  and b.id(+) = e.id
                   and e.snap_id = s.snap_id");
                tmpSql.AppendFormat("  and s.INSTANCE_NUMBER = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and s.dbid = {0} ", arguments.DbId);
                //tmpSql.AppendFormat(" and s.snap_id between  &snap_fr and & snap_to ");
                tmpSql.Append("               and s.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and substr(e.name,1,1) <> 'D'     order by e.snap_id");

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
        /// <summary>
        /// 66
        /// </summary>
        /// <param name="arguments"></param>
        public void GetEnq_01(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select snap_id
                          ,Time_stamp
                          ,max(decode(rank, 1,wait_tm, 0))/1000  rank1_wait_tm
                          ,max(decode(rank, 2,wait_tm, 0))/1000  rank2_wait_tm
                          ,max(decode(rank, 3,wait_tm, 0))/1000  rank3_wait_tm
                          ,max(decode(rank, 4,wait_tm, 0))/1000  rank4_wait_tm
                          ,max(decode(rank, 5,wait_tm, 0))/1000  rank5_wait_tm
                    from (
                    select e.snap_id
                          ,to_char(s.end_interval_time,'yyyy-mm-dd hh24:mi:ss') Time_stamp
                          ,to_number(substr((s.end_interval_time-s.begin_interval_time)*86400,2,9)) interval
                          ,rank
                          ,(e.cum_wait_time - nvl(b.cum_wait_time,0))  wait_tm
                    from raw_DBA_HIST_ENQUEUE_STAT_{0} e
                       , raw_DBA_HIST_ENQUEUE_STAT_{0} b
                       , raw_dba_hist_snapshot_{0} S
                       ,(select ety,req_reason,wttm, rownum rank
                           from ( select e.eq_type  ety
                                        ,e.req_reason
                                        ,e.cum_wait_time - b.cum_wait_time  wttm
                                    from raw_DBA_HIST_ENQUEUE_STAT_{0} e
                                        ,raw_DBA_HIST_ENQUEUE_STAT_{0} b ", arguments.DbName);
                tmpSql.AppendFormat("   where b.snap_id         = {0}", arguments.SnapId);
                tmpSql.AppendFormat(" and e.snap_id         = {0} ", arguments.SnapId);
                tmpSql.AppendFormat(" and b.dbid(+)            = {0} ", arguments.DbId);
                tmpSql.AppendFormat(" and e.dbid               = {0} ", arguments.DbId);
                tmpSql.Append(" and b.dbid(+)            = e.dbid");
                tmpSql.AppendFormat(" and b.instance_number(+) = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and e.instance_number    = {0} ", arguments.InstanceNumber);
                tmpSql.Append(@" and b.instance_number(+) = e.instance_number 

                                 and b.eq_type(+)         = e.eq_type
                                     and b.req_reason(+)      = e.req_reason
                                     and e.total_wait# - nvl(b.total_wait#,0) > 0
                                   order by wttm desc )  a
                          where rownum <= 5  ) top_5
                    where b.snap_id(+)         = e.snap_id - 1 ");
                //tmpSql.AppendFormat(" and e.snap_id    between &snap_fr +1 and &snap_to ");
                tmpSql.Append("               and e.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and b.dbid(+)            = {0} ", arguments.DbId);
                tmpSql.AppendFormat(" and e.dbid               ={0} ", arguments.DbId);
                tmpSql.Append(" and b.dbid(+)            = e.dbid ");
                tmpSql.AppendFormat(" and b.instance_number(+) ={0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and e.instance_number    ={0} ", arguments.InstanceNumber);
                tmpSql.Append(@" and b.eq_type            = e.eq_type 

                and b.req_reason         = e.req_reason
                       and e.eq_type = top_5.ety
                       and b.eq_type = top_5.ety
                       and e.req_reason = top_5.req_reason
                       and b.req_reason = top_5.req_reason
                       and e.snap_id = s.snap_id ");
                tmpSql.AppendFormat(" and s.INSTANCE_NUMBER = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and s.dbid = {0} ", arguments.DbId);
                //tmpSql.AppendFormat(" and s.snap_id between  &snap_fr and &snap_to ");
                tmpSql.Append("               and s.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(@" Order by e.snap_id )   
                       Group by snap_id ,Time_stamp
                    order by snap_id");

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

        /// <summary>
        /// 66
        /// </summary>
        /// <param name="arguments"></param>
        public void GetEnq_02(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select rank||':'||ety || '-' || to_char(nvl(l.name,' '))
                                || decode( upper(req_reason)
                                   , 'CONTENTION', null
                                   , '-',          null
                                   , ' ('||req_reason||')')             Enqueu_type
                    from v$lock_type l,
                    (select ety,req_reason,wttm,rownum rank
                      from  (select e.eq_type  ety,e.req_reason req_reason
                                   ,e.cum_wait_time - b.cum_wait_time  wttm
                              from raw_DBA_HIST_ENQUEUE_STAT_{0} e
                                  ,raw_DBA_HIST_ENQUEUE_STAT_{0} b ", arguments.DbName);
                tmpSql.Append("  where b.snap_id         =  ");
                tmpSql.AppendFormat(" (select min(snap_id) from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.Append("   and e.snap_id         =  ");
                tmpSql.AppendFormat(" (select max(snap_id) from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and b.dbid(+)            ={0} ", arguments.DbId);
                tmpSql.AppendFormat(" and e.dbid               = {0} ", arguments.DbId);
                tmpSql.AppendFormat("  and b.dbid(+)            = {0} ", arguments.DbId);
                tmpSql.AppendFormat(" and b.instance_number(+) = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and e.instance_number    ={0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(@" and b.instance_number(+) = e.instance_number   and b.eq_type(+) = e.eq_type
                                     and b.req_reason(+)      = e.req_reason
                                     and e.total_wait# - nvl(b.total_wait#,0) > 0
                                   order by wttm desc )  a
                          where rownum <= 5  ) top_5
                    where l.type(+) = top_5.ety
                    order by rank");

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

        /// <summary>
        /// 66
        /// </summary>
        /// <param name="arguments"></param>
        public void GetEnq_av_01(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select snap_id
                          ,Time_stamp
                          ,max(decode(rank, 1,av_wait_tm, 0))/1000  rank1_wait_tm
                          ,max(decode(rank, 2,av_wait_tm, 0))/1000  rank2_wait_tm
                          ,max(decode(rank, 3,av_wait_tm, 0))/1000  rank3_wait_tm
                          ,max(decode(rank, 4,av_wait_tm, 0))/1000  rank4_wait_tm
                          ,max(decode(rank, 5,av_wait_tm, 0))/1000  rank5_wait_tm
                    from ( select e.snap_id Snap_ID
                          ,to_char(s.end_interval_time,'yyyy-mm-dd hh24:mi:ss') Time_stamp
                          ,rank
                          ,decode((e.total_wait#   - nvl(b.total_wait#,0)), 0, 0,
                                  (e.cum_wait_time - nvl(b.cum_wait_time,0))
                                 /(e.total_wait# - nvl(b.total_wait#,0)))  av_wait_tm
                    from raw_DBA_HIST_ENQUEUE_STAT_{0} e
                       , raw_DBA_HIST_ENQUEUE_STAT_{0} b
                       , raw_dba_hist_snapshot_{0} S
                       ,(select ety , rownum rank
                           from ( select e.eq_type  ety,
                                         decode((e.total_wait# - nvl(b.total_wait#,0)), 0, 0,
                                                (e.cum_wait_time - nvl(b.cum_wait_time,0))
                                               /(e.total_wait#   - nvl(b.total_wait#,0)))  awttm
                                    from raw_DBA_HIST_ENQUEUE_STAT_{0} e
                                        ,raw_DBA_HIST_ENQUEUE_STAT_{0} b ", arguments.DbName);
                tmpSql.AppendFormat(" where b.snap_id         =  ");
                tmpSql.AppendFormat(" (select min(snap_id) from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and e.snap_id         =  ");
                tmpSql.AppendFormat(" (select max(snap_id) from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and b.dbid(+)            = {0} ", arguments.DbId);
                tmpSql.AppendFormat(" and e.dbid               ={0} ", arguments.DbId);
                tmpSql.AppendFormat(" and b.dbid(+)            = e.dbid");
                tmpSql.AppendFormat(" and b.instance_number(+) ={0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat(" and e.instance_number    ={0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and b.instance_number(+) = e.instance_number ");

                tmpSql.AppendFormat(@" and b.eq_type(+)         = e.eq_type
                                    and b.req_reason(+)      = e.req_reason
                                     and e.total_wait# - nvl(b.total_wait#,0) > 0
                                   order by awttm desc )  a
                          where rownum <= 5  ) top_5
                    where b.snap_id(+)         = e.snap_id - 1 ");
                //tmpSql.AppendFormat("  and e.snap_id    between &snap_fr and &snap_to ");
                tmpSql.Append("               and e.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and b.dbid(+)            ={0} ", arguments.DbId);
                tmpSql.AppendFormat("  and e.dbid               ={0} ", arguments.DbId);
                tmpSql.AppendFormat(" and b.dbid(+)            = e.dbid ");

                tmpSql.AppendFormat(" and b.instance_number(+) = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and e.instance_number    ={0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(@"  and b.eq_type          = e.eq_type 
                and b.req_reason         = e.req_reason
                       and e.eq_type = top_5.ety
                       and b.eq_type = top_5.ety
                       and e.snap_id = s.snap_id");
                tmpSql.AppendFormat("    and s.INSTANCE_NUMBER = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("    and s.dbid = {0} ", arguments.DbId);
                //tmpSql.AppendFormat("   and s.snap_id between  &snap_fr +1 and &snap_to");
                tmpSql.Append("               and s.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(@"  Order by e.snap_id ) 
                     Group by snap_id ,Time_stamp
                    order by snap_id");

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

        /// <summary>
        /// 66
        /// </summary>
        /// <param name="arguments"></param>
        public void GetEnq_av_02(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select ety || '-' ||req_reason|| ' ('||nvl(l.name,' ') ||')' Enqueu_type
                    from v$lock_type l,
                    (select ety,req_reason,awttm,rownum rank
                      from  (select e.eq_type  ety,e.req_reason req_reason
                                   ,decode((e.total_wait#   - nvl(b.total_wait#,0)), 0, 0
                                          ,(e.cum_wait_time - nvl(b.cum_wait_time,0))
                                          /(e.total_wait#   - nvl(b.total_wait#,0)))  awttm
                                    from raw_DBA_HIST_ENQUEUE_STAT_{0} e
                                        ,raw_DBA_HIST_ENQUEUE_STAT_{0} b ", arguments.DbName);
                tmpSql.AppendFormat(" where b.snap_id         = {0}");
                tmpSql.AppendFormat(" (select min(snap_id) from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                     arguments.DbName,
                     arguments.StartTimeKey,
                     arguments.EndTimeKey,
                     arguments.InstanceNumber);
                tmpSql.AppendFormat("  and e.snap_id         = {0}");
                tmpSql.AppendFormat(" (select max(snap_id) from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and b.dbid(+)            = {0} ", arguments.DbId);
                tmpSql.AppendFormat("  and e.dbid               = {0} ", arguments.DbId);
                tmpSql.AppendFormat("  and b.dbid(+)            = {0} ", arguments.DbId);
                tmpSql.AppendFormat("  and b.instance_number(+) = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and e.instance_number    = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(@"  and b.instance_number(+) = e.instance_number
                                     and b.eq_type(+)         = e.eq_type
                                     and b.req_reason(+)      = e.req_reason
                                     and e.total_wait# - nvl(b.total_wait#,0) > 0
                                   order by awttm desc )  a
                          where rownum <= 5  ) top_5
                    where l.type(+) = top_5.ety
                    order by rank");

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

        /// <summary>
        /// 66
        /// </summary>
        /// <param name="arguments"></param>
        public void GetEnq_raw(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select  e.snap_id \""SnapID\""
                      , to_char(s.end_interval_time, 'yyyy-mm-dd hh24:mi:ss') \""Timestamp\""
                      , e.eq_type || '-' || to_char(nvl(l.name, ' '))
                      || decode(upper(e.req_reason), 'CONTENTION', null, '-', null, ' (' || e.req_reason || ')')   Enqueu_type
                     , e.total_req#    - nvl(b.total_req#,0)            \""Requests\""
                     , e.succ_req#     - nvl(b.succ_req#,0)             \""Succ gets\""
                     , e.failed_req#   - nvl(b.failed_req#,0)           \""Fail gets\""
                     , e.total_wait#   - nvl(b.total_wait#,0)           \""Waits\""
                     , (e.cum_wait_time - nvl(b.cum_wait_time, 0)) / 1000  \""Wait tm(s)\""
                     , decode((e.total_wait#   - nvl(b.total_wait#,0)), 0,0,
                              (e.cum_wait_time - nvl(b.cum_wait_time, 0))
                             / (e.total_wait#   - nvl(b.total_wait#,0))) \""Avg. Wait tm(ms)\""
                  from raw_DBA_HIST_ENQUEUE_STAT_{0} e
                     , raw_DBA_HIST_ENQUEUE_STAT_{0} b
                     , v$lock_type              l
                     , raw_dba_hist_snapshot_{0} S
                 where b.snap_id(+) = e.snap_id - 1 ", arguments.DbName);
                //tmpSql.AppendFormat("   and e.snap_id    between & snap_fr + 1 and & snap_to ");
                tmpSql.Append("               and e.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("   and b.dbid(+) = {0}", arguments.DbId);
                tmpSql.AppendFormat("   and e.dbid = {0}", arguments.DbId);
                tmpSql.AppendFormat("  and b.dbid(+) = {0}", arguments.DbId);
                tmpSql.AppendFormat("   and b.instance_number(+) = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat(@"   and e.instance_number = {0}", arguments.InstanceNumber);
                tmpSql.Append(@" and b.instance_number(+) = e.instance_number
                   and b.eq_type(+) = e.eq_type
                   and b.req_reason(+) = e.req_reason
                   and e.total_wait# - nvl(b.total_wait#,0) > 0
                   and l.type(+) = e.eq_type
                   and e.snap_id = s.snap_id");
                tmpSql.AppendFormat("   and s.INSTANCE_NUMBER = , arguments.InstanceNumber");
                tmpSql.AppendFormat("    and s.dbid ={0}", arguments.DbId);
                //tmpSql.AppendFormat("   and s.snap_id between & snap_fr and & snap_to");
                tmpSql.Append("               and s.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  order by e.snap_id, 8 desc, 7 desc");

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

        public void GetLatch_01(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select
                           e.snap_id  \""SnapID\"",
                           snap_time \""Timestamp\"",
                           max(decode(rank, 1, (e.WAIT_TIME - b.WAIT_TIME) / interval, 0)) / 1000 rank1_wait_t,
                           max(decode(rank, 2, (e.WAIT_TIME - b.WAIT_TIME) / interval, 0)) / 1000 rank2_wait_t,
                           max(decode(rank, 3, (e.WAIT_TIME - b.WAIT_TIME) / interval, 0)) / 1000 rank3_wait_t,
                           max(decode(rank, 4, (e.WAIT_TIME - b.WAIT_TIME) / interval, 0)) / 1000 rank4_wait_t,
                           max(decode(rank, 5, (e.WAIT_TIME - b.WAIT_TIME) / interval, 0)) / 1000 rank5_wait_t,
                           max(decode(rank, 1, decode((e.gets - b.gets), 0, 0, (e.misses - b.misses) / (e.gets - b.gets)), 0)) rank1_missratio,
                           max(decode(rank, 2, decode((e.gets - b.gets), 0, 0, (e.misses - b.misses) / (e.gets - b.gets)), 0)) rank2_missratio,
                           max(decode(rank, 3, decode((e.gets - b.gets), 0, 0, (e.misses - b.misses) / (e.gets - b.gets)), 0)) rank3_missratio,
                           max(decode(rank, 4, decode((e.gets - b.gets), 0, 0, (e.misses - b.misses) / (e.gets - b.gets)), 0)) rank4_missratio,
                           max(decode(rank, 5, decode((e.gets - b.gets), 0, 0, (e.misses - b.misses) / (e.gets - b.gets)), 0)) rank5_missratio,
                           max(decode(rank, 1, (e.misses - b.misses), 0)) rank1_misses,
                           max(decode(rank, 2, (e.misses - b.misses), 0)) rank2_misses,
                           max(decode(rank, 3, (e.misses - b.misses), 0)) rank3_misses,
                           max(decode(rank, 4, (e.misses - b.misses), 0)) rank4_misses,
                           max(decode(rank, 5, (e.misses - b.misses), 0)) rank5_misses
                    from raw_DBA_HIST_LATCH_{0} b
                       , raw_DBA_HIST_LATCH_{0} e
                       , raw_DBA_HIST_LATCH_NAME_{0} n
                       , (
                           select instance_number, snap_id
                                  , to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time
                                  , to_number(substr((end_interval_time - begin_interval_time) * 86400, 2, 9)) interval
                             from raw_dba_hist_snapshot_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between & snap_fr + 1 and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@" ) s
                         , (select latch_hash, rownum rank
                           from(select b.latch_hash latch_hash
                                       , e.WAIT_TIME - b.WAIT_TIME  WAIT_TIM
                                    from raw_DBA_HIST_LATCH_{0} b
                                       , raw_DBA_HIST_LATCH_{0} e ", arguments.DbName);
                tmpSql.AppendFormat(" where b.snap_id = {0} ", arguments.SnapId);
                tmpSql.AppendFormat(" and e.snap_id = {0} ", arguments.SnapId);
                tmpSql.AppendFormat(" and b.instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("and e.instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and b.dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(" and e.dbid = {0} ", arguments.DbId);

                tmpSql.AppendFormat(@" and b.LATCH_HASH = e.LATCH_HASH 

            order by WAIT_TIM desc) a
                          where rownum <= 5
                         ) top_5
                    where b.snap_id(+) = e.snap_id - 1 ");
                //tmpSql.AppendFormat(" and e.snap_id between &snap_fr and & snap_to ");
                tmpSql.Append("               and e.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and b.dbid(+) ={0} ", arguments.DbId);

                tmpSql.AppendFormat(" and e.dbid ={0} ", arguments.DbId);
                tmpSql.AppendFormat("and b.dbid(+) = e.dbid ");
                tmpSql.AppendFormat(" and b.instance_number(+) ={0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and e.instance_number ={0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(@" and b.LATCH_HASH = e.LATCH_HASH
                   and e.LATCH_HASH = top_5.latch_hash
                       and top_5.latch_hash = n.LATCH_HASH
                       and b.LATCH_HASH = top_5.latch_hash
                       and e.snap_id = s.snap_id
                      Group by e.snap_id, snap_time
                      Order by e.snap_id");

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
        public void GetLatch_02(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select n.latch_name latch5_Rank, rank
                    from raw_DBA_HIST_LATCH_NAME_{0} n,
                         (select latch_hash ,rownum rank
                           from ( select b.latch_hash latch_hash
                                       , e.WAIT_TIME - b.WAIT_TIME  WAIT_TIM
                                    from raw_DBA_HIST_LATCH_{0} b
                                       , raw_DBA_HIST_LATCH_{0} e ", arguments.DbName);
                tmpSql.AppendFormat("    where b.snap_id         = {0} ", arguments.SnapId);
                tmpSql.AppendFormat("  and e.snap_id         = {0} ", arguments.SnapId);
                tmpSql.AppendFormat(" and b.instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and e.instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and b.dbid(+)         = {0} ", arguments.DbId);
                tmpSql.AppendFormat(" and e.dbid            = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@" and b.LATCH_HASH      = e.LATCH_HASH
                    order by WAIT_TIM desc
                                ) a
                           where rownum <= 5
                         ) top_5
                    where top_5.latch_hash = n.LATCH_HASH ");
                tmpSql.AppendFormat(" and n.dbid={0} ", arguments.DbId);

                tmpSql.Append(" order by top_5.rank");

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
        public void GetLatch_sleep(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select e.snap_id \""SnapID\""
                     , to_char(s.end_interval_time, 'yyyy-mm-dd hh24:mi:ss') \""Timestamp\""
                     , b.latch_name                                \""Name\""
                     , e.gets - b.gets                      \""Gets\""
                     , e.misses - b.misses                    \""Misses\""
                     , e.sleeps - b.sleeps                    \""Sleeps\""
                     , e.spin_gets - b.spin_gets                 \""Spin_gets\""
                  from raw_DBA_HIST_LATCH_{0} b
                     , raw_DBA_HIST_LATCH_{0} e
                     , raw_dba_hist_snapshot_{0} S ", arguments.DbName);
                tmpSql.AppendFormat(" where b.snap_id = {0} ", arguments.SnapId);
                tmpSql.AppendFormat("   and e.snap_id = {0} ", arguments.SnapId);
                tmpSql.AppendFormat("    and b.dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat("    and e.dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat("    and b.dbid = e.dbid");
                tmpSql.AppendFormat("    and b.instance_number ={0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("    and e.instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat(@"    and b.instance_number = e.instance_number
                   and b.latch_name = e.latch_name
                   and e.sleeps - b.sleeps > 0
                   and e.snap_id = s.snap_id");
                tmpSql.AppendFormat("    and s.INSTANCE_NUMBER = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("    and s.dbid = {0} ", arguments.DbId);
                //tmpSql.AppendFormat("    and s.snap_id between & snap_fr and & snap_to");
                tmpSql.Append("               and s.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  order by e.snap_id, 5 desc");

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
        public void GetLatch_miss(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select e.snap_id \""SnapID\""
                     , to_char(s.end_interval_time, 'yyyy-mm-dd hh24:mi:ss') \""Timestamp\""
                     , e.parent_name                              parent
                     , e.where_in_code                            \""where from\""
                     , e.nwfail_count - nvl(b.nwfail_count, 0)    \""nowait_misses\""
                     , e.sleep_count - nvl(b.sleep_count, 0)     sleeps
                     , e.wtr_slp_count - nvl(b.wtr_slp_count, 0)   \""waiter_sleeps\""
                  from raw_DBA_HIST_LATCH_MISSES_SUMMARY_{0} b
                     , raw_DBA_HIST_LATCH_MISSES_SUMMARY_{0} e
                     , raw_dba_hist_snapshot_{0} S ", arguments.DbName);
                tmpSql.AppendFormat("  where b.snap_id(+) ={0} ", arguments.SnapId);

                tmpSql.AppendFormat("  and e.snap_id = {0} ", arguments.SnapId);
                tmpSql.AppendFormat("  and b.dbid(+) ={0} ", arguments.DbId);
                tmpSql.AppendFormat("  and e.dbid ={0} ", arguments.DbId);
                tmpSql.AppendFormat("  and b.dbid(+) = {0} ", arguments.DbId);
                tmpSql.AppendFormat("  and b.instance_number(+) ={0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and e.instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat(@"and b.instance_number(+) = e.instance_number
                   and b.parent_name(+) = e.parent_name
                   and b.where_in_code(+) = e.where_in_code
                   and e.sleep_count > nvl(b.sleep_count, 0)
                   and e.snap_id = s.snap_id");
                tmpSql.AppendFormat("  and s.INSTANCE_NUMBER ={0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and s.dbid ={0} ", arguments.DbId);
                //tmpSql.AppendFormat("  and s.snap_id between & snap_fr and & snap_to");
                tmpSql.Append("               and s.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  order by e.snap_id, e.parent_name, sleeps desc");

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
        /// <summary>
        /// 69
        /// </summary>
        /// <param name="arguments"></param>
        public void GetRowcache(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select	b.snap_id     \""SnapID\"",
                        b.snap_time   \""Timestamp\"",
                    decode(gets, 0, 0, getmisses / gets) \""getMiss /gets%\"",
                    decode(scans, 0, 0, scanmisses / scans) \""scanMiss %\"",
                    gets          \""Get\"",
                    getmisses     \""getMiss\"",
                    scans         \""Scan\"",
                    scanmisses    \""scanMiss\"",
                    modifications \""Modify\"",
                    flushes       \""Flush\"",
                    usage         \""Usage\"",
                    dlm_requests  \""GES Request\"",
                    dlm_conflicts \""GES Conflict\"",
                    dlm_releases  \""GES Release\""
                from(
                    select  snap_id,
                        sum(usage) - lag(sum(usage)) over(order by snap_id) usage,
                        sum(gets) - lag(sum(gets)) over(order by snap_id) gets,
                        sum(getmisses) - lag(sum(getmisses)) over(order by snap_id) getmisses,
                        sum(scans) - lag(sum(scans)) over(order by snap_id) scans,
                        sum(scanmisses) - lag(sum(scanmisses)) over(order by snap_id) scanmisses,
                        sum(modifications) - lag(sum(modifications)) over(order by snap_id) modifications,
                        sum(flushes) - lag(sum(flushes)) over(order by snap_id) flushes,
                        sum(dlm_requests) - lag(sum(dlm_requests)) over(order by snap_id) dlm_requests,
                        sum(dlm_conflicts) - lag(sum(dlm_conflicts)) over(order by snap_id) dlm_conflicts,
                        sum(dlm_releases) - lag(sum(dlm_releases)) over(order by snap_id) dlm_releases
                    from raw_dba_hist_rowcache_summary_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(@"  where snap_id between & snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);

                tmpSql.AppendFormat(@" group by snap_id
                   ) a,
                          (select snap_id,
                                   to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time
                              from raw_dba_hist_snapshot_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@" ) b   
                   where   a.snap_id = b.snap_id
                order by 1, 4 desc, 5 desc");

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

        /// <summary>
        /// 69
        /// </summary>
        /// <param name="arguments"></param>
        public void GetRowcache_raw(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select	b.snap_id     \""SnapID\"",
                        b.snap_time   \""Timestamp\"",
                    parameter     \""Dictionary\"",
                    decode(gets, 0, 0, getmisses / gets) \""getMiss/gets%\"",
                    decode(scans, 0, 0, scanmisses / scans) \""scanMiss %\"",
                    gets          \""Get\"",
                    getmisses     \""getMiss\"",
                    scans         \""Scan\"",
                    scanmisses    \""scanMiss\"",
                    modifications \""Modify\"",
                    flushes       \""Flush\"",
                    usage         \""Usage\"",
                    dlm_requests  \""GES Request\"",
                    dlm_conflicts \""GES Conflict\"",
                    dlm_releases  \""GES Release\""
                from(
                    select  snap_id, parameter,
                        usage,
                        gets - lag(gets) over(partition by parameter order by parameter, snap_id) gets,
                        getmisses - lag(getmisses) over(partition by parameter order by parameter, snap_id) getmisses,
                        scans - lag(scans) over(partition by parameter order by parameter, snap_id) scans,
                        scanmisses - lag(scanmisses) over(partition by parameter order by parameter, snap_id) scanmisses,
                        modifications - lag(modifications) over(partition by parameter order by parameter, snap_id) modifications,
                        flushes - lag(flushes) over(partition by parameter order by parameter, snap_id) flushes,
                        dlm_requests - lag(dlm_requests) over(partition by parameter order by parameter, snap_id) dlm_requests,
                        dlm_conflicts - lag(dlm_conflicts) over(partition by parameter order by parameter, snap_id) dlm_conflicts,
                        dlm_releases - lag(dlm_releases) over(partition by parameter order by parameter, snap_id) dlm_releases

                    from raw_dba_hist_rowcache_summary_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(@"  where snap_id between & snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and dbid = {0}", arguments.DbId);
                tmpSql.AppendFormat(@"   ) a,
                (select snap_id,
                                   to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time
                              from raw_dba_hist_snapshot_{0}", arguments.DbName);
                //tmpSql.AppendFormat("   where snap_id between 1 + &snap_fr and & snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("     and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("     and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@"  ) b
                where   a.snap_id = b.snap_id
                order by 1, 4 desc, 5 desc");

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

        /// <summary>
        /// 70
        /// </summary>
        /// <param name="arguments"></param>
        public void GetLibrary_cache_hit(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select 	b.snap_id,
		                b.snap_time, 
		                a.gets,
		                a.gethits,
		                a.pins,
		                a.pinhits,
		                a.reloads,
		                a.invalidations,
		                pinhits/pins \""library_cache_hit % \""
                from
                    (select snap_id,
                                --namespace,
				                sum(gets)-lag(sum(gets),1) over(order by snap_id) gets,
			                sum(gethits)-lag(sum(gethits),1) over(order by snap_id) gethits,
			                sum(pins)-lag(sum(pins),1) over(order by snap_id) pins,
			                sum(pinhits)-lag(sum(pinhits),1) over(order by snap_id) pinhits,
			                sum(reloads)-lag(sum(reloads),1) over(order by snap_id) reloads,
			                sum(invalidations)-lag(sum(invalidations),1) over(order by snap_id) invalidations
                       from    raw_dba_hist_librarycache_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between &snap_fr and &snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and dbid = {0}", arguments.DbId);
                tmpSql.AppendFormat(@"  group by snap_id) a,
                    (select snap_id, 
                                    to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time
                                from raw_dba_hist_snapshot_{0}", arguments.DbName);
                //tmpSql.AppendFormat("   where snap_id between 1+ &snap_fr and &snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat(@"   and dbid = {0}
                            ) b
                    where   a.snap_id=b.snap_id
                    order by snap_time", arguments.DbId);

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

        /// <summary>
        /// 70
        /// </summary>
        /// <param name="arguments"></param>
        public void GetLibrary_cache(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select	b.snap_id     \""SnapID\"",
                        b.snap_time   \""Timestamp\"",
                    max(decode(namespace,'SQL AREA',reloads,0))        \""reload: sql\"",
	                max(decode(namespace,'TABLE/PROCEDURE',reloads,0)) \""reload: tab/proc\"",
	                max(decode(namespace,'TRIGGER',reloads,0))         \""reload: trg\"",
	                max(decode(namespace,'BODY',reloads,0))            \""reload: body\"",
	                max(decode(namespace,'SQL AREA',invalidations,0))        \""invalid: sql\"",
	                max(decode(namespace,'TABLE/PROCEDURE',invalidations,0)) \""invalid: tab/proc\"",
	                max(decode(namespace,'TRIGGER',invalidations,0))         \""invalid: trg\"",
	                max(decode(namespace,'BODY',invalidations,0))            \""invalid: body\""
                from(
                    select snap_id,

                            namespace,
	                        gets-lag(gets,1) over(partition by namespace order by namespace,snap_id) gets,
		                gethits-lag(gethits,1) over(partition by namespace order by namespace,snap_id) gethits,
		                pins-lag(pins,1) over(partition by namespace order by namespace,snap_id) pins,
		                pinhits-lag(pinhits,1) over(partition by namespace order by namespace,snap_id) pinhits,
		                reloads-lag(reloads,1) over(partition by namespace order by namespace,snap_id) reloads,
		                invalidations-lag(invalidations,1) over(partition by namespace order by namespace,snap_id) invalidations
                   from    raw_dba_hist_librarycache_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between &snap_fr and &snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and instance_number = {0}");
                tmpSql.AppendFormat("    and dbid ={}");
                tmpSql.AppendFormat(@"    and namespace in ('SQL AREA','TABLE/PROCEDURE','TRIGGER','BODY')
                    ) a,
                        ( select snap_id,
                                 to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time
                            from raw_dba_hist_snapshot_{0}", arguments.DbName);
                //tmpSql.AppendFormat("    where snap_id between 1+ &snap_fr and &snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("     and instance_number ={0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("      and dbid = {0}", arguments.DbId);
                tmpSql.AppendFormat(@"   ) b
                 where   a.snap_id=b.snap_id
                        group by b.snap_id, b.snap_time
                        order by b.snap_time");

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

        /// <summary>
        /// 70
        /// </summary>
        /// <param name="arguments"></param>
        public void GetLibrary_cache_raw(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select	b.snap_id     \""SnapID\"",
                        b.snap_time   \""Timestamp\"",
                    namespace     \""Library\"",
	                1-decode(gets,0,1, gethits/gets) \""getMiss %\"",
	                1-decode(pins,0,1, pinhits/pins) \""pinMiss%\"",
	                gets		\""Get\"",
	                gethits		\""getHit\"",
	                pins		\""Pin\"",
	                pinhits		\""pinHit\"",
	                reloads		\""Reload\"",
	                invalidations	\""Invalid\"",
	                dlm_lock_requests	\""GES lock req.\"",
	                dlm_pin_requests	\""GES pin req.\"",
	                dlm_pin_releases	\""GES pin release\"",
	                dlm_invalidation_requests \""GES invalid req.\"",
	                dlm_invalidations	\""GES invalid\""
                from(
                    select snap_id,namespace,
		                gets-lag(gets,1) over(partition by namespace order by namespace,snap_id) gets,
		                gethits-lag(gethits,1) over(partition by namespace order by namespace,snap_id) gethits,
		                pins-lag(pins,1) over(partition by namespace order by namespace,snap_id) pins,
		                pinhits-lag(pinhits,1) over(partition by namespace order by namespace,snap_id) pinhits,
		                reloads-lag(reloads,1) over(partition by namespace order by namespace,snap_id) reloads,
		                invalidations-lag(invalidations,1) over(partition by namespace order by namespace,snap_id) invalidations,
		                dlm_lock_requests-lag(dlm_lock_requests,1) over(partition by namespace order by namespace,snap_id) dlm_lock_requests,
		                dlm_pin_requests-lag(dlm_pin_requests,1) over(partition by namespace order by namespace,snap_id) dlm_pin_requests,
		                dlm_pin_releases-lag(dlm_pin_releases,1) over(partition by namespace order by namespace,snap_id) dlm_pin_releases,
		                dlm_invalidation_requests-lag(dlm_invalidation_requests,1) over(partition by namespace order by namespace,snap_id) dlm_invalidation_requests,
		                dlm_invalidations-lag(dlm_invalidations,1) over(partition by namespace order by namespace,snap_id) dlm_invalidations
                   from    raw_dba_hist_librarycache_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between &snap_fr and &snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and dbid ={0}", arguments.DbId);
                tmpSql.AppendFormat(@"  ) a,
                        (select snap_id,
                                 to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time
                            from raw_dba_hist_snapshot_{0}", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between 1+ &snap_fr and &snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("   and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("   and dbid = {0}", arguments.DbId);
                tmpSql.AppendFormat(@"   ) b
                where   a.snap_id=b.snap_id
                order by 1, 4 desc, 5 desc");

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
        /// <summary>
        /// 71
        /// </summary>
        /// <param name="arguments"></param>
        public void GetSga_raw1(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select	b.snap_id     \""SnapID\"",
                        b.snap_time   \""Timestamp\"",
                        pool          \""Pool\"",
                    name          \""Name\"",
                    bytes / 1048576 \""Size(M)\"",
                    (bytes - pre_bytes) / 1048576 \""A∑¨(M)\""
                from(
                    select  snap_id,
                            nvl(pool, 'null') pool,
                            name,
                        bytes,
                        nvl(lag(bytes, 1) over(partition by pool, name order by pool, name, snap_id), 0) pre_bytes

                    from raw_dba_hist_sgastat_{0} ", arguments.DbName);

                //tmpSql.AppendFormat("  where snap_id between & snap_fr and & snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("   and     instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("   and     dbid ={0}", arguments.DbId);
                tmpSql.AppendFormat(@"  and(nvl(pool, 'null') <> 'shared pool' or(pool = 'shared pool' and name < 'h'))
                        ) a,
                        (select snap_id,
                                 to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time
                            from raw_dba_hist_snapshot_{0}", arguments.DbName);
                //tmpSql.AppendFormat("   where snap_id between 1 + &snap_fr and & snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("     and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("     and dbid ={0}", arguments.DbId);
                tmpSql.AppendFormat(@"  ) b
                where   a.snap_id = b.snap_id
                order by pool,name,snap_time");

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
        /// <summary>
        /// 71
        /// </summary>
        /// <param name="arguments"></param>
        public void GetSga_raw2(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select	b.snap_id     \""SnapID\"",
                        b.snap_time   \""Timestamp\"",
                        pool          \""Pool\"",
                    name          \""Name\"",
                    bytes / 1048576 \""Size(M)\"",
                    (bytes - pre_bytes) / 1048576 \""A∑¨(M)\""
                from(
                    select  snap_id,
                            nvl(pool, 'null') pool,
                            name,
                        bytes,
                        nvl(lag(bytes, 1) over(partition by pool, name order by pool, name, snap_id), 0) pre_bytes

                    from raw_dba_hist_sgastat_{0} ", arguments.DbName);

                //tmpSql.AppendFormat("  where snap_id between & snap_fr and & snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and     instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("   and     dbid = {0}", arguments.DbId);
                tmpSql.AppendFormat(@"  and     pool = 'shared pool'
                        and     name >= 'h'
                    ) a,
                        (select snap_id,
                                 to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time
                            from raw_dba_hist_snapshot_{0}", arguments.DbName);
                //tmpSql.AppendFormat("   where snap_id between 1 + &snap_fr and & snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("    and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("   and dbid = {0}", arguments.DbId);
                tmpSql.AppendFormat(@"  ) b
                where   a.snap_id = b.snap_id
                order by pool,name,snap_time");

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
        /// <summary>
        /// 71
        /// </summary>
        /// <param name="arguments"></param>
        public void GetSga(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select 
                    b.snap_id     \""SnapID\"",
                    b.snap_time   \""Timestamp\"",
                    sum(case when pool = 'null' and name = 'buffer_cache' then bytes / 1048576 else 0 end) \""buf.cache(M)\"",
                    sum(case when pool = 'null' and name = 'log_buffer'   then bytes / 1048576 else 0 end) \""log.buf(M)\"",
                    sum(case when pool = 'shared pool'                  then bytes / 1048576 else 0 end) \""shared.pool(M)\"",
                    sum(case when pool = 'java pool'                    then bytes / 1048576 else 0 end) \""java.pool(M)\"",
                    sum(case when pool = 'large pool'                   then bytes / 1048576 else 0 end) \""large.pool(M)\"",
                    sum(case when pool = 'streams pool'                 then bytes / 1048576 else 0 end) \""streams.pool(M)\"",
                    sum(case when pool = 'shared pool'  and name = 'SQLA' then bytes / 1048576 else 0 end) \""sqlarea(M)\"",
                    sum(case when pool = 'shared pool'  and name in ('library cache', 'KGLH0', 'KGLHD', 'KGLNA', 'KGLSG', 'KGLDA', 'KGLA', 'KGLS') then bytes / 1048576 else 0 end) \""lib.cache(M)\"",
                    sum(case when pool = 'shared pool'  and name not in ('SQLA', 'library cache', 'KGLH0', 'KGLHD', 'KGLNA', 'KGLSG', 'KGLDA', 'KGLA', 'KGLS', 'free memory') then bytes / 1048576 else 0 end) \""others(M)\"",
                    sum(case when pool = 'shared pool'  and name = 'free memory' then bytes / 1048576 else 0 end) \""free(M)\"",
                    sum(case when pool = 'large pool'   and name <> 'free memory' then bytes / 1048576 else 0 end) \""largepool:used(M)\"",
                    sum(case when pool = 'large pool'   and name = 'free memory' then bytes / 1048576 else 0 end) \""largepool:free(M)\"",
                    sum(case when pool = 'java pool'    and name <> 'free memory' then bytes / 1048576 else 0 end) \""javapool:used(M)\"",
                    sum(case when pool = 'java pool'    and name = 'free memory' then bytes / 1048576 else 0 end) \""javapool:free(M)\"",
                    sum(case when pool = 'streams pool' and name <> 'free memory' then bytes / 1048576 else 0 end) \""streams:used(M)\"",
                    sum(case when pool = 'streams pool' and name = 'free memory' then bytes / 1048576 else 0 end) \""streams:free(M)\""
                from(
                    select  snap_id,
                            nvl(pool, 'null') pool,
                            name,
                        bytes
                    from raw_dba_hist_sgastat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("   where snap_id between 1 + &snap_fr and & snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("     and     instance_number ={0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("      and     dbid = {0}", arguments.DbId);
                tmpSql.AppendFormat(@"  ) a,
                        (select snap_id,
                                 to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time
                            from raw_dba_hist_snapshot_{0}", arguments.DbName);
                //tmpSql.AppendFormat("   where snap_id between 1 + &snap_fr and & snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("    and instance_number ={0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("    and dbid ={0}", arguments.DbId);
                tmpSql.AppendFormat(@"  ) b
                where   a.snap_id = b.snap_id
                group by b.snap_id, b.snap_time
                order by b.snap_time");

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

        /// <summary>
        /// 72
        /// </summary>
        /// <param name="arguments"></param>
        public void GetLog_info(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"select   a.thread#       	 \""Thread#\""
	                    , a.group# 		 \""Group#\""
                        , ' ' || b.member       \""Logfile\""
                        , round(a.bytes / 1048576, 2) \""Size(Mb)\""
                    from v$log a, v$logfile b
                    where a.group#=b.group#
                    order by 1, 2, 3");

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

        /// <summary>
        /// 72
        /// </summary>
        /// <param name="arguments"></param>
        public void GetLog_ckpt(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"select  a.thread# \""Thread#\"",
                                        min(b.first_time - a.first_time) * 24 * 60    \""Minimum(min)\"",
                                        max(b.first_time - a.first_time) * 24 * 60    \""Maximum(min)\"",
                                        avg(b.first_time - a.first_time) * 24 * 60    \""Average(min)\"",
                                        min(b.first_time - a.first_time) * 24 * 60 * 60 \""Minimum(sec)\"",
                                        max(b.first_time - a.first_time) * 24 * 60 * 60 \""Maximum(sec)\"",
                                        avg(b.first_time - a.first_time) * 24 * 60 * 60 \""Average(sec)\""
                                from v$log_history a, v$log_history b
                                where   a.sequence# + 1 = b.sequence#
                                and     a.sequence# < (select max(sequence#) from v$log_history )
                                and     b.first_time >= trunc(sysdate - 33)
                                and     a.thread# = b.thread#
                                group by a.thread#");

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


        /// <summary>
        /// 72
        /// </summary>
        /// <param name="arguments"></param>
        public void GetLogswitch(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"select   to_char(first_time,'yyyy-mm-dd hh24')		    \""Switch Time\""
                                from v$log_history
                                where   first_time >= sysdate - 7
                                group by to_char(first_time, 'yyyy-mm-dd hh24')
                                order by 1");

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

        /// <summary>
        /// 72
        /// </summary>
        /// <param name="arguments"></param>
        public void GetLog_switch_f(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"select	thread#,
	                        sum(less5)	\""Log Switch interval <= 5 min\"",

                            sum(bet5_10)    \""5 ~ 10 min\"",
                            sum(bet10_30)   \""10 ~ 30 min\"",
                            sum(over30) \"" > 30 min\"",
                            count(*)    \""7 -day Total\""
                        from(
                                select
                                 thread#,
                                 case when(first_time - lag(first_time) over(partition by thread# order by first_time))*24*60 <=  5 then 1
                                      when(first_time - lag(first_time) over(partition by thread# order by first_time)) is null then null else 0 end less5,
                                 case when(first_time - lag(first_time) over(partition by thread# order by first_time))*24*60 >   5
                                       and(first_time - lag(first_time) over(partition by thread# order by first_time))*24*60 <= 10 then 1 else 0 end bet5_10,
                                 case when(first_time - lag(first_time) over(partition by thread# order by first_time))*24*60 >  10
                                       and(first_time - lag(first_time) over(partition by thread# order by first_time))*24*60 <= 30 then 1 else 0 end bet10_30,
                                 case when(first_time - lag(first_time) over(partition by thread# order by first_time))*24*60 >  30 then 1 else 0 end over30
                                from v$log_history
                                where first_time >= trunc(sysdate - 7)
                                )
                        where less5 is not null
                        group by thread#
                        order by thread#");

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


        /// <summary>
        /// 72
        /// </summary>
        /// <param name="arguments"></param>
        public void GetLog_switch(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"select
                                 thread# \""Thread#\"",
                                 substr(to_char(first_time, 'yyyy/mm/dd'), 1, 10) \""Day\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '00', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '00', 1, 0))) \""00\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '01', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '01', 1, 0))) \""01\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '02', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '02', 1, 0))) \""02\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '03', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '03', 1, 0))) \""03\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '04', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '04', 1, 0))) \""04\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '05', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '05', 1, 0))) \""05\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '06', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '06', 1, 0))) \""06\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '07', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '07', 1, 0))) \""07\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '08', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '08', 1, 0))) \""08\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '09', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '09', 1, 0))) \""09\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '10', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '10', 1, 0))) \""10\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '11', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '11', 1, 0))) \""11\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '12', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '12', 1, 0))) \""12\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '13', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '13', 1, 0))) \""13\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '14', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '14', 1, 0))) \""14\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '15', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '15', 1, 0))) \""15\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '16', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '16', 1, 0))) \""16\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '17', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '17', 1, 0))) \""17\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '18', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '18', 1, 0))) \""18\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '19', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '19', 1, 0))) \""19\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '20', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '20', 1, 0))) \""20\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '21', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '21', 1, 0))) \""21\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '22', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '22', 1, 0))) \""22\"",
                                 decode(sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '23', 1, 0)), 0, 0, sum(decode(substr(to_char(first_time, 'HH24'), 1, 2), '23', 1, 0))) \""23\"",
                                 decode(sum(1), 0, 0, sum(1)) \""Per Day\""
                                from v$log_history
                                where first_time >= trunc(sysdate - 33)
                                group by thread#,substr(to_char(first_time,'yyyy/mm/dd'),1,10)
                                order by 1, substr(to_char(first_time, 'yyyy/mm/dd'), 1, 10) desc, thread#");

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
        /// <summary>
        /// 72
        /// </summary>
        /// <param name="arguments"></param>
        public void GetLog_archsize(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"select   to_date(to_char(first_time,'yyyy/mm/dd'),'yyyy/mm/dd') \""per Day\""
                                        , nvl(sum(decode(thread#,1,1,0)),0) \""Log Count(1)\""
                                        , nvl(sum(decode(thread#,1,blocks*block_size)),0)/1048576 \""1_Size(Mb)\""
		                                , nvl(sum(decode(thread#,2,1,0)),0) \""Log Count(2)\""
                                        , nvl(sum(decode(thread#,2,blocks*block_size)),0)/1048576 \""2_Size(Mb)\""
                                from v$archived_log
                                where   first_time >= trunc(sysdate - 33)
                                group by to_char(first_time, 'yyyy/mm/dd')
                                order by 1");

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
        /// <summary>
        /// 74
        /// </summary>
        /// <param name="arguments"></param>
        public void GetSession_cache(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select sn.snap_id \""SnapID\"",
                               sn.snap_time \""Timestamp\"",
                            parse_req_total \""Parse requests\"",
                                cursor_cache_hits   \""Cursor cache hits\"",
                                parse_req_total - cursor_cache_hits   \""ReParsed requests\"",
                            decode(parse_req_total, 0, 0, cursor_cache_hits / parse_req_total) \""Cursor cache hit%\""
                        from
                            (
                            select  snap_id,
		                        case when max(decode(stat_name, 'session cursor cache hits', value)) < 0 then 0

                                    else max(decode(stat_name, 'session cursor cache hits', value)) end cursor_cache_hits,
  		                        case when max(decode(stat_name,'parse count (total)',value)) < 0 then 0
                                    else max(decode(stat_name, 'parse count (total)', value)) end parse_req_total
                            from(
                                select  snap_id,
                                    stat_name,
                                    nvl(value - lag(value) over(partition by stat_name order by snap_id), 0)  value
                                from raw_dba_hist_sysstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between & snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(" and     instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and     dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@" and     stat_name in ('session cursor cache hits', 'parse count (total)')
                                )
                            group by snap_id
	                        ) v1,
                                  (select snap_id,
                                           to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time
                                       from raw_dba_hist_snapshot_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("   and instance_number ={0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@"  ) sn
                         where v1.snap_id = sn.snap_id
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
        /// <summary>
        /// 76
        /// </summary>
        /// <param name="arguments"></param>
        public void GetParameter_value(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select	parameter_name \""Parameter\"",

                                nvl(value, 'n/a') \""Current_value\"",
                                isdefault \""default ?\"",
                                ismodified \""modified ?\""
                            from raw_dba_hist_parameter_{0} ", arguments.DbName);
                tmpSql.AppendFormat("  where snap_id = {0}", arguments.SnapId);
                tmpSql.AppendFormat(" and     instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and     dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(" order by parameter_name");

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
        /// <summary>
        /// 76
        /// </summary>
        /// <param name="arguments"></param>
        public void GetParam_sga(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select	parameter_name \""SGA Parameter\"",
                    nvl(value, 'n/a') \""d\""
                from raw_dba_hist_parameter_{0}
                where parameter_name in (
                             'db_block_buffers', 'db_block_size', 'db_cache_size', 'db_2k_cache_size', 'db_4k_cache_size',
                             'db_8k_cache_size', 'db_16k_cache_size', 'db_32k_cache_size', 'db_keep_cache_size', 'db_recycle_cache_size',
                             'java_pool_size', 'large_pool_size', 'log_buffer',
                             'pga_aggregate_target',
                             'shared_pool_size', 'sga_max_size', 'sga_target',
                             'statistics_level', 'streams_pool_size', 'workarea_size_policy'
                                )", arguments.DbName);
                tmpSql.AppendFormat("  where snap_id = {0}", arguments.SnapId);
                tmpSql.AppendFormat(" and     instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(" and     dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(" order by parameter_name");


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
        /// <summary>
        /// 76
        /// </summary>
        /// <param name="arguments"></param>
        public void GetParam_diff(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select	b.snap_id     \""SnapID\"",
                            b.snap_time   \""Timestamp\"",
                            parameter_name \""parameter\"",
                        nvl(pre_value, 'null') \""value(before)\"",
                        nvl(value, 'null') \""value\""
                    from(
                            select snap_id, parameter_name, value,
                                   lag(value, 1) over(partition by parameter_name order by parameter_name, snap_id) pre_value
                            from raw_dba_hist_parameter_{0} ", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between & snap_fr and & snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and    instance_number ={0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and    dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@"  and    parameter_name in
                                    (
                                    select parameter_name
                                    from raw_dba_hist_parameter_{0}", arguments.DbName);
                //tmpSql.AppendFormat("   where snap_id between & snap_fr and & snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and   instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("   and   dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@"   and   ismodified <> 'FALSE'
                                    )
                            ) a,
                            (select snap_id,
                                     to_char(end_interval_time, 'yyyy-mm-dd hh24:mi:ss') snap_time
                                from raw_dba_hist_snapshot_{0}", arguments.DbName);
                //tmpSql.AppendFormat("   where snap_id between &snap_fr and & snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("     and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("    and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@"   ) b
                    where   a.snap_id = b.snap_id
                    and a.value<> a.pre_value");
                tmpSql.AppendFormat("  and     a.snap_id >  ");
                tmpSql.AppendFormat(" (select min(snap_id) from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  order by parameter_name");

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
        /// <summary>
        /// 78
        /// </summary>
        /// <param name="arguments"></param>
        public void GetDB_Info(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select 	aa.instance_number \""INST_ID\"",
                                hostname,
                                db_name,
                                instance_name Instance,
                                aa.dbid DBID,
                                max(version)      \""Version\"",
                                min(snap_id) \""Start Snap\"",
                                max(snap_id) \""End Snap\"",
                                min(end_interval_time) \""Start Time(Snap)\"",
                                max(end_interval_time) \""End Time(Snap)\"",
                                max(parallel) \""Cluster\"",
                                platform_name \""Platform\"",
                                max(to_char(startup_time, 'yyyy/mm/dd hh24:mi:ss')) \""Instance Startup Time\"",
                                'CPUs ' || max(NUM_CPUS) || ', Cores ' || max(NUM_CPU_CORES) || ', Sockets ' || max(NUM_CPU_SOCKETS) AS \""CPU Info\"",
                                max(round(PHYSICAL_MEMORY_BYTES / 1024 / 1024 / 1024, 2)) || ' GB' AS \""Phy Memory\"",
                                max(sga_current) || ' GB' AS \""SGA Size\"",
                                max(pga) || ' GB' AS \""PGA Size\""
                                from
                                        (select b.instance_number NUM, b.db_name, b.instance_name, replace(b.host_name, '.samsung.com', '') hostname,
                                                a.dbid, a.snap_id, a.instance_number, b.platform_name, b.parallel, b.version,
                                                to_char(begin_interval_time, 'yyyy/mm/dd hh24:mi') begin_interval_time,
                                                to_char(end_interval_time, 'yyyy/mm/dd hh24:mi') end_interval_time,
                                                b.startup_time

                                        from raw_dba_hist_snapshot_{0} a,
                                                raw_DBA_HIST_DATABASE_INSTANCE_{0} b

                                        where   a.dbid = b.dbid

                                        and     a.instance_number = b.instance_number ", arguments.DbName);

                //tmpSql.AppendFormat(" and     a.snap_id between & snap_fr and & snap_to");
                tmpSql.Append("               and a.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(@" ) aa, (select s2.dbid, s2.instance_number, round(s2.SZ / 1024 / 1024 / 1024, 2) sga_current, round((s2.SZ - s1.SZ) / 1024 / 1024 / 1024, 2) sga_diff

                                            from(select dbid, instance_number, sum(value) SZ

                                                    from raw_dba_hist_sga_{0}", arguments.DbName);

                tmpSql.AppendFormat("  where snap_id = ");
                tmpSql.AppendFormat(" (select min(snap_id) from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@"   group by dbid, instance_number
                                                 ) s1,
                                                (select dbid, instance_number, sum(value) SZ
                                                    from raw_dba_hist_sga_{0}", arguments.DbName);
                tmpSql.AppendFormat("  where snap_id = {0} ", arguments.SnapId);
                tmpSql.AppendFormat("  and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(@"  group by dbid, instance_number
                                                ) s2
                                            where s1.dbid = s2.dbid
                                            and s1.instance_number = s2.instance_number   ) c,
		                                (select dbid, instance_number, max(decode(stat_name, 'PHYSICAL_MEMORY_BYTES', value, 0)) PHYSICAL_MEMORY_BYTES,
						                                 max(decode(stat_name, 'NUM_CPUS', value, 0)) NUM_CPUS,
						                                 max(decode(stat_name, 'NUM_CPU_CORES', value, 0)) NUM_CPU_CORES,
						                                 max(decode(stat_name, 'NUM_CPU_SOCKETS', value, 0)) NUM_CPU_SOCKETS
                                                from raw_dba_hist_osstat_{0}", arguments.DbName);
                tmpSql.AppendFormat("  where dbid ={0} ", arguments.DbId);
                tmpSql.AppendFormat("  and snap_id = {0} ", arguments.SnapId);
                tmpSql.AppendFormat(@"  group by dbid, instance_number) d,
                                        (select dbid, instance_number, max(decode(name, 'aggregate PGA target parameter', value / 1024 / 1024 / 1024)) PGA
                                                      from raw_dba_hist_pgastat_{0}", arguments.DbName);
                tmpSql.AppendFormat("     where dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat(" and snap_id = ");
                tmpSql.AppendFormat(" (select max(snap_id) from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                   arguments.DbName,
                   arguments.StartTimeKey,
                   arguments.EndTimeKey,
                   arguments.InstanceNumber);
                tmpSql.AppendFormat(@"   group by dbid, instance_number) e
                                where   aa.dbid = c.dbid
                                and aa.dbid = d.dbid
                                and aa.dbid = e.dbid
                                and aa.instance_number = c.instance_number
                                and aa.instance_number = d.instance_number
                                and aa.instance_number = e.instance_number
                                and parallel in (select  decode(value, 'TRUE', 'YES', 'NO') from raw_dba_hist_parameter_{0}
                                                 where parameter_name = 'cluster_database'", arguments.DbName);
                tmpSql.AppendFormat("  and dbid = {0} ", arguments.DbId);
                tmpSql.AppendFormat("  and snap_id = ");
                tmpSql.AppendFormat(" (select max(snap_id) from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                   arguments.DbName,
                   arguments.StartTimeKey,
                   arguments.EndTimeKey,
                   arguments.InstanceNumber);
                tmpSql.AppendFormat(" ) group by aa.instance_number, hostname, db_name, aa.dbid,platform_name, instance_name order by 1");

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
        /// <summary>
        /// 79
        /// </summary>
        /// <param name="arguments"></param>
        public void GetSql_literal_cnt(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"select   sql_id         \""SQL ID\""
                        , replace(replace(replace(sql_text, chr(9), ' '), chr(10), ' '), chr(13), ' ') \""SQL Text\""
                    , cnt        \""Count\""
                    , executions     \""Exec (Max)\""
                    , sharable_mem   \""Memory (bytes)\""
                    , module     \""Module\""
                    , '\""' || force_matching_signature || '\""' force_matching_signature
                    , hash_value \""Hash_value\""
                from(
                    select max(to_char(substr(sql_fulltext, 1, 2500)))  sql_text
                        , s.force_matching_signature
                        , count(*) cnt
                        , sum(sharable_mem) sharable_mem
                        , max(hash_value) hash_value
                        , max(sql_id) sql_id
                        , max(executions) executions
                        , max(module) module
                    from gv$sql s

                         where s.executions > 0 ");

                tmpSql.AppendFormat("  and inst_id = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat(@"  and s.force_matching_signature <> exact_matching_signature
                         and s.parsing_schema_name not in ('SYS', 'SYSTEM', 'SYSMAN')
                         group by s.force_matching_signature
                         having count(s.exact_matching_signature) >= 2
                         order by cnt desc, sharable_mem desc)
                where rownum <= 123");

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

        /// <summary>
        /// 79
        /// </summary>
        /// <param name="arguments"></param>
        public void GetSql_literal_mem(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(@"select   sql_id         \""SQL ID\""
                        , replace(replace(replace(sql_text, chr(9), ' '), chr(10), ' '), chr(13), ' ') \""SQL Text\""
                    , cnt        \""Count\""
                    , executions     \""Exec (Max)\""
                    , sharable_mem   \""Memory (bytes)\""
                    , module     \""Module\""
                    , '\""' || force_matching_signature || '\""' force_matching_signature
                    , hash_value \""Hash_value\""
                from(
                    select max(to_char(substr(sql_fulltext, 1, 2500)))  sql_text
                        , s.force_matching_signature
                        , count(*) cnt
                        , sum(sharable_mem) sharable_mem
                        , max(hash_value) hash_value
                        , max(sql_id) sql_id
                        , max(executions) executions
                        , max(module) module
                    from gv$sql s

                         where s.executions > 0 ");

                tmpSql.AppendFormat("  and inst_id =  {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat(@"  and s.force_matching_signature <> exact_matching_signature
                         and s.parsing_schema_name not in ('SYS', 'SYSTEM', 'SYSMAN')
                         group by s.force_matching_signature
                         having count(s.exact_matching_signature) >= 2
                         order by cnt desc, sharable_mem desc)
                where rownum <= 123");

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

        /// <summary>
        /// 79
        /// </summary>
        /// <param name="arguments"></param>
        public void GetSql_literal_awr(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select   rownum RANK
		                ,sql_id         \""SQL ID\""
                        , '\""' || force_matching_signature || '\""' force_matching_signature
                        , replace(replace(replace(sql_text, chr(9), ' '), chr(10), ' '), chr(13), ' ') \""SQL Text\""
                    , cnt        \""Count\""
                    , executions     \""Exec (SUM)\""
                    , sharable_mem   \""Memory (bytes)\""
                    , sorts
                    , module     \""Module\""
                from(
                    select max(to_char(substr(b.sql_text, 1, 250)))  sql_text
                        , s.force_matching_signature
                        , count(*) cnt
                        , sum(sharable_mem) sharable_mem
                        , max(s.sql_id) sql_id
                        , sum(executions_delta) executions
                        , sum(sorts_delta) sorts
                        , max(module) module
                    from raw_dba_hist_sqlstat_{0} s, dba_hist_sqltext b

                        where s.dbid = b.dbid

                        and s.sql_id = b.sql_id(+) ", arguments.DbName);

                //tmpSql.AppendFormat("  and snap_id between & snap_fr and & snap_to");
                tmpSql.Append("               and snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("   and instance_number = {0} ", arguments.InstanceNumber);

                tmpSql.AppendFormat("  and s.dbid = {0} ", arguments.DbId);

                tmpSql.AppendFormat(@"  and force_matching_signature > 0

                        and parsing_schema_name not in ('SYS', 'SYSTEM', 'SYSMAN')

                        group by force_matching_signature

                        having count(distinct s.sql_id) > 2
                         order by cnt desc, sharable_mem desc)
                where rownum <= 123");

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

        public void GetSQL_List(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                if (string.IsNullOrEmpty(arguments.DbId) ||
                    string.IsNullOrEmpty(arguments.DbName) ||
                    string.IsNullOrEmpty(arguments.InstanceNumber) ||
                    string.IsNullOrEmpty(arguments.StartTimeKey) ||
                    string.IsNullOrEmpty(arguments.EndTimeKey))
                {
                    this.ExecutingValue = null;
                    return;
                }


                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("select ");
                tmpSql.Append(@"distinct B.sql_id || ' (' || CMD || ')' AS ""CMD"", ");
                tmpSql.Append("B.* ");
                tmpSql.Append(@"from(select a.*, c.COMMAND_NAME ""CMD"" ");
                tmpSql.Append("from(select a.sql_id, ");
                tmpSql.Append("module, ");
                tmpSql.Append(@"to_char(round(sum(nvl(executions_delta, 0)), 2), '999,999,999.99') ""Executions"", ");
                tmpSql.Append(
                    @"to_char(round(sum(nvl(elapsed_time_delta, 0) / 1000000), 2), '999,999,999.99') ""Elapsed_Time"", ");
                tmpSql.Append(
                    @"to_char(round(sum(nvl(cpu_time_delta, 0) / 1000000), 2), '999,999,999.99') ""CPU_Time"", ");
                tmpSql.Append(
                    @"to_char(round(sum(nvl(buffer_gets_delta, 0)), 2), '999,999,999,999.99') ""Buffer_Gets"", ");
                tmpSql.Append(
                    @"to_char(round(sum(nvl(disk_reads_delta, 0)), 2), '999,999,999,999.99') ""Disk_Reads"", ");
                tmpSql.Append(
                    @"to_char(round(sum(nvl(parse_calls_delta, 0)), 2), '999,999,999,999.99') ""Parse_Call"", ");
                tmpSql.Append(
                    @"to_char(round(sum(nvl(disk_reads_delta, 0)) / sum(nvl(executions_delta, 0)), 2), 'FM999,999,999,999.99') ""Reads / exec"", ");
                tmpSql.Append(
                    @"to_char(round(sum(nvl(buffer_gets_delta, 0)) / sum(nvl(executions_delta, 0)), 2), 'FM999,999,999,999.99') ""Gets / exec"", ");
                tmpSql.Append(
                    @"to_char(round(sum(nvl(elapsed_time_delta, 0) / 1000000) / sum(nvl(executions_delta, 0)), 2), 'FM999,999,990.99') ""Elaps / exec"", ");
                tmpSql.Append(
                    @"to_char(round(sum(nvl(executions_delta, 0)) / sum(nvl(elapsed_time_delta, 0) / 1000000), 2), 'FM999,999,990.99') ""exec / Elaps"", ");
                tmpSql.Append(
                    @"to_char(round(sum(nvl(cpu_time_delta, 0) / 1000000) / sum(nvl(executions_delta, 0)), 2), 'FM999,999,990.99') ""CPU / exec"", ");
                tmpSql.Append(
                    @"to_char(round(sum(nvl(rows_processed_delta, 0)) / sum(nvl(executions_delta, 0)), 2), 'FM999,999,990.99') ""Rowsp / exec"", ");
                tmpSql.Append(
                    @"to_char(round(sum(nvl(buffer_gets_delta, 0)) / e.Log_VAL, 2) * 100, '999,999,999.90') "" % Tot Logical Reads"", ");
                tmpSql.Append(
                    @"to_char(round(sum(nvl(disk_reads_delta, 0)) / d.Phy_VAL, 2) * 100, '999,999,999.90') "" % Tot Physical Reads"", ");
                tmpSql.Append(
                    @"to_char(round(sum(nvl(parse_calls_delta, 0)) / f.Parse_VAL, 2) * 100, '999,999,999.90') "" % Tot Parse Calls"" ");
                tmpSql.AppendFormat("   from raw_dba_hist_sqlstat_{0} a, ", arguments.DbName);
                tmpSql.Append("         (select SUM(max(VALUE)) - SUM(min(VALUE)) Phy_VAL ");
                tmpSql.AppendFormat("         from RAW_DBA_HIST_SYSSTAT_{0} ", arguments.DbName);
                tmpSql.AppendFormat("          where instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("         AND dbid = {0} ", arguments.DbId);
                tmpSql.Append("         AND STAT_NAME = 'physical reads' ");
                tmpSql.Append("        and snap_id in ( ");
                tmpSql.AppendFormat(
                    "                   select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey, arguments.InstanceNumber);
                tmpSql.Append("         group by instance_number) d, ");
                tmpSql.Append("         (select SUM(max(VALUE)) - SUM(min(VALUE)) Log_VAL ");
                tmpSql.AppendFormat("         from RAW_DBA_HIST_SYSSTAT_{0} ", arguments.DbName);
                tmpSql.AppendFormat("          where instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("         AND dbid = {0} ", arguments.DbId);
                tmpSql.Append("         AND STAT_NAME = 'session logical reads' ");
                tmpSql.Append("             AND snap_id in ( ");
                tmpSql.AppendFormat(
                    "                   select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey, arguments.InstanceNumber);
                tmpSql.Append("         group by instance_number) e, ");
                tmpSql.Append("         (select SUM(max(VALUE)) - SUM(min(VALUE)) Parse_VAL ");
                tmpSql.AppendFormat("               from RAW_DBA_HIST_SYSSTAT_{0} ", arguments.DbName);
                tmpSql.AppendFormat("                where instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("               AND dbid = {0} ", arguments.DbId);
                tmpSql.Append("               AND STAT_NAME = 'parse count (total)' ");
                tmpSql.Append("               AND snap_id in ( ");
                tmpSql.AppendFormat(
                    "                   select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey, arguments.InstanceNumber);
                tmpSql.Append("               group by instance_number) f ");
                tmpSql.AppendFormat("        where dbid = {0} ", arguments.DbId);
                tmpSql.Append("               AND snap_id in ( ");
                tmpSql.AppendFormat(
                    "                   select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey, arguments.InstanceNumber);
                tmpSql.AppendFormat("          and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.Append("       group by sql_id, module, d.phy_VAL,e.Log_VAL,f.Parse_VAL ");
                tmpSql.Append("       having sum(nvl(executions_delta, 0)) > 0 ) a, ");
                tmpSql.AppendFormat("           RAW_DBA_HIST_SQLTEXT_{0} b, ", arguments.DbName);
                tmpSql.Append("           RAW_DBA_HIST_SQLCOMMAND_NAME c ");
                tmpSql.Append(" where b.dbid = c.DBID ");
                tmpSql.Append("   and a.sql_id = b.sql_id ");
                tmpSql.Append("   and b.COMMAND_TYPE = c.COMMAND_TYPE) B ");
                tmpSql.Append("order by 7 desc ");


                RemotingLog.Instance
                    .WriteServerLog(
                        MethodInfo.GetCurrentMethod().Name,
                        LogBase._LOGTYPE_TRACE_INFO,
                        this.Requester.IP,
                        tmpSql.ToString(),
                        false);

                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance
                    .WriteServerLog(
                        MethodInfo.GetCurrentMethod().Name,
                        LogBase._LOGTYPE_TRACE_ERROR,
                        this.Requester.IP,
                        string.Format(" Biz Component Exception occured: {0}", ex.ToString()),
                        false);
                throw ex;
            }
        }

        //81
        public void GetSQL_List_litera(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                if (string.IsNullOrEmpty(arguments.DbId) ||
                    string.IsNullOrEmpty(arguments.DbName) ||
                    string.IsNullOrEmpty(arguments.InstanceNumber) ||
                    string.IsNullOrEmpty(arguments.StartTimeKey) ||
                    string.IsNullOrEmpty(arguments.EndTimeKey))
                {
                    this.ExecutingValue = null;
                    return;
                }


                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("select distinct B.sql_id ||' ('||CMD||')' AS \"CMD\", B.* ");
                tmpSql.Append("from ( select aa.*, c.COMMAND_NAME \"CMD\" ");
                tmpSql.Append("from (select '\"'||force_matching_signature||'\"' force_matching_signature, ");
                tmpSql.Append("max(a.sql_id) SQL_ID ,");
                tmpSql.Append("count(distinct sql_id), module, ");
                tmpSql.Append("to_char(round(sum(nvl(executions_delta,0)),2),'999,999,999.99') \"Executions\", ");
                tmpSql.Append(
                    "to_char(round(sum(nvl(elapsed_time_delta,0)/1000000),2),'999,999,999.99') \"Elapsed_Time\", ");
                tmpSql.Append("to_char(round(sum(nvl(cpu_time_delta,0)/1000000),2),'999,999,999.99') \"CPU_Time\", ");
                tmpSql.Append("to_char(round(sum(nvl(buffer_gets_delta,0)),2),'999,999,999,999.99') \"Buffer_Gets\", ");
                tmpSql.Append("to_char(round(sum(nvl(disk_reads_delta,0)),2),'999,999,999,999.99') \"Disk_Reads\", ");
                tmpSql.Append("to_char(round(sum(nvl(parse_calls_delta,0)),2),'999,999,999,999.99') \"Parse_Call\", ");
                tmpSql.Append(
                    "to_char(round(sum(nvl(disk_reads_delta,0))/sum(nvl(executions_delta,0)),2),'FM999,999,999,999.99') \"Reads/exec\", ");
                tmpSql.Append(
                    "to_char(round(sum(nvl(buffer_gets_delta,0))/sum(nvl(executions_delta,0)),2),'FM999,999,999,999.99') \"Gets/exec\", ");
                tmpSql.Append(
                    "to_char(round(sum(nvl(elapsed_time_delta,0)/1000000)/sum(nvl(executions_delta,0)),2),'FM999,999,990.99') \"Elaps/exec\", ");
                tmpSql.Append(
                    "to_char(round(sum(nvl(executions_delta,0))/sum(nvl(elapsed_time_delta,0)/1000000),2),'FM999,999,990.99') \"exec/Elaps\", ");
                tmpSql.Append(
                    "to_char(round(sum(nvl(cpu_time_delta,0)/1000000)/sum(nvl(executions_delta,0)),2),'FM999,999,990.99') \"CPU/exec\", ");
                tmpSql.Append(
                    "to_char(round(sum(nvl(rows_processed_delta,0))/sum(nvl(executions_delta,0)),2),'FM999,999,990.99') \"Rowsp/exec\", ");
                tmpSql.Append(
                    "to_char(round(sum(nvl(buffer_gets_delta,0))/e.Log_VAL,2)*100,'999,999,999.90') \"% Tot Logical Reads\", ");
                tmpSql.Append(
                    "to_char(round(sum(nvl(disk_reads_delta,0))/d.Phy_VAL,2)*100,'999,999,999.90') \"% Tot Physical Reads\", ");
                tmpSql.Append(
                    "to_char(round(sum(nvl(parse_calls_delta,0))/f.Parse_VAL,2)*100,'999,999,999.90') \"% Tot Parse Calls\" ");
                tmpSql.AppendFormat("from raw_dba_hist_sqlstat_{0} a, ", arguments.DbName);
                tmpSql.Append("(select SUM(max(VALUE)) - SUM(min(VALUE)) Phy_VAL ");
                tmpSql.AppendFormat("from RAW_DBA_HIST_SYSSTAT_{0} ", arguments.DbName);
                tmpSql.AppendFormat("where instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("AND dbid = {0} ", arguments.DbId);
                tmpSql.Append("AND STAT_NAME = 'physical reads' ");
                tmpSql.Append("               AND snap_id in ( ");
                tmpSql.AppendFormat(
                    "                   select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey, arguments.InstanceNumber);
                tmpSql.Append("group by instance_number) d, ");
                tmpSql.Append("(select SUM(max(VALUE)) - SUM(min(VALUE)) Log_VAL ");
                tmpSql.AppendFormat("from RAW_DBA_HIST_SYSSTAT_{0} ", arguments.DbName);
                tmpSql.AppendFormat("where instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("AND dbid = {0} ", arguments.DbId);
                tmpSql.Append("AND STAT_NAME = 'session logical reads' ");
                tmpSql.Append("               AND snap_id in ( ");
                tmpSql.AppendFormat(
                    "                   select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey, arguments.InstanceNumber);
                tmpSql.Append("group by instance_number) e, ");
                tmpSql.Append("(select SUM(max(VALUE)) - SUM(min(VALUE)) Parse_VAL ");
                tmpSql.AppendFormat("from RAW_DBA_HIST_SYSSTAT_{0} ", arguments.DbName);
                tmpSql.AppendFormat("where instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("AND dbid = {0} ", arguments.DbId);
                tmpSql.Append("AND STAT_NAME = 'parse count (total)' ");
                tmpSql.Append("               AND snap_id in ( ");
                tmpSql.AppendFormat(
                    "                   select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey, arguments.InstanceNumber);
                tmpSql.Append("group by instance_number) f ");
                tmpSql.AppendFormat("where dbid = {0} ", arguments.DbId);
                tmpSql.Append("               AND snap_id in ( ");
                tmpSql.AppendFormat(
                    "                   select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey, arguments.InstanceNumber);
                tmpSql.AppendFormat("and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.Append("and executions_delta > 0 ");
                tmpSql.Append("and force_matching_signature > 0 ");
                tmpSql.Append("group by force_matching_signature, module, d.phy_VAL,e.Log_VAL,f.Parse_VAL ");
                tmpSql.Append("having sum(nvl(executions_delta, 0)) > 0 and count(distinct sql_id) > 2) aa,  ");
                tmpSql.AppendFormat("RAW_DBA_HIST_SQLTEXT_{0} b, ", arguments.DbName);
                tmpSql.Append("RAW_DBA_HIST_SQLCOMMAND_NAME c ");
                tmpSql.Append("where b.dbid = c.DBID ");
                tmpSql.Append("and aa.sql_id = b.sql_id ");
                tmpSql.Append("and b.COMMAND_TYPE = c.COMMAND_TYPE) B ");
                tmpSql.Append("order by 9 desc ");

                RemotingLog.Instance
                    .WriteServerLog(
                        MethodInfo.GetCurrentMethod().Name,
                        LogBase._LOGTYPE_TRACE_INFO,
                        this.Requester.IP,
                        tmpSql.ToString(),
                        false);

                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance
                    .WriteServerLog(
                        MethodInfo.GetCurrentMethod().Name,
                        LogBase._LOGTYPE_TRACE_ERROR,
                        this.Requester.IP,
                        string.Format(" Biz Component Exception occured: {0}", ex.ToString()),
                        false);
                throw ex;
            }
        }

        //80
        public void GetSQL_literal_01(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                if (string.IsNullOrEmpty(arguments.DbId) ||
                    string.IsNullOrEmpty(arguments.DbName) ||
                    string.IsNullOrEmpty(arguments.InstanceNumber) ||
                    string.IsNullOrEmpty(arguments.StartTimeKey) ||
                    string.IsNullOrEmpty(arguments.EndTimeKey))
                {
                    this.ExecutingValue = null;
                    return;
                }

                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("select sn.snap_id, ");
                tmpSql.Append("to_char(sn.end_interval_time, 'yyyy-mm-dd hh24:mi:ss') \"Timestamp\", ");
                tmpSql.Append("max(decode(rank, 1, bufget, 0)) rank1_bufget, ");
                tmpSql.Append("max(decode(rank, 2, bufget, 0)) rank2_bufget, ");
                tmpSql.Append("max(decode(rank, 3, bufget, 0)) rank3_bufget, ");
                tmpSql.Append("max(decode(rank, 4, bufget, 0)) rank4_bufget, ");
                tmpSql.Append("max(decode(rank, 5, bufget, 0)) rank5_bufget, ");
                tmpSql.Append("max(decode(rank, 1, exec, 0)) rank1_exec, ");
                tmpSql.Append("max(decode(rank, 2, exec, 0)) rank2_exec, ");
                tmpSql.Append("max(decode(rank, 3, exec, 0)) rank3_exec, ");
                tmpSql.Append("max(decode(rank, 4, exec, 0)) rank4_exec, ");
                tmpSql.Append("max(decode(rank, 5, exec, 0)) rank5_exec, ");
                tmpSql.Append(
                    "decode(max(decode(rank, 1, exec, 0)), 0, 0, max(decode(rank, 1, bufget, 0)) / max(decode(rank, 1, exec, 0))) rank1_bufget_per_exec, ");
                tmpSql.Append(
                    "decode(max(decode(rank, 2, exec, 0)), 0, 0, max(decode(rank, 2, bufget, 0)) / max(decode(rank, 2, exec, 0))) rank2_bufget_per_exec, ");
                tmpSql.Append(
                    "decode(max(decode(rank, 3, exec, 0)), 0, 0, max(decode(rank, 3, bufget, 0)) / max(decode(rank, 3, exec, 0))) rank3_bufget_per_exec, ");
                tmpSql.Append(
                    "decode(max(decode(rank, 4, exec, 0)), 0, 0, max(decode(rank, 4, bufget, 0)) / max(decode(rank, 4, exec, 0))) rank4_bufget_per_exec, ");
                tmpSql.Append(
                    "decode(max(decode(rank, 5, exec, 0)), 0, 0, max(decode(rank, 5, bufget, 0)) / max(decode(rank, 5, exec, 0))) rank5_bufget_per_exec, ");
                tmpSql.Append(
                    "decode(max(delta_bufget_tot), 0, 0, max(decode(rank, 1, bufget, 0)) / max(delta_bufget_tot)) rank1_bufget_per_tot, ");
                tmpSql.Append(
                    "decode(max(delta_bufget_tot), 0, 0, max(decode(rank, 2, bufget, 0)) / max(delta_bufget_tot)) rank2_bufget_per_tot, ");
                tmpSql.Append(
                    "decode(max(delta_bufget_tot), 0, 0, max(decode(rank, 3, bufget, 0)) / max(delta_bufget_tot)) rank3_bufget_per_tot, ");
                tmpSql.Append(
                    "decode(max(delta_bufget_tot), 0, 0, max(decode(rank, 4, bufget, 0)) / max(delta_bufget_tot)) rank4_bufget_per_tot, ");
                tmpSql.Append(
                    "decode(max(delta_bufget_tot), 0, 0, max(decode(rank, 5, bufget, 0)) / max(delta_bufget_tot)) rank5_bufget_per_tot, ");
                tmpSql.Append("max(decode(rank, 1, cput / 1000000, 0)) rank1_cput, ");
                tmpSql.Append("max(decode(rank, 2, cput / 1000000, 0)) rank2_cput, ");
                tmpSql.Append("max(decode(rank, 3, cput / 1000000, 0)) rank3_cput, ");
                tmpSql.Append("max(decode(rank, 4, cput / 1000000, 0)) rank4_cput, ");
                tmpSql.Append("max(decode(rank, 5, cput / 1000000, 0)) rank5_cput, ");
                tmpSql.Append("max(decode(rank, 1, elap / 1000000, 0)) rank1_elap, ");
                tmpSql.Append("max(decode(rank, 2, elap / 1000000, 0)) rank2_elap, ");
                tmpSql.Append("max(decode(rank, 3, elap / 1000000, 0)) rank3_elap, ");
                tmpSql.Append("max(decode(rank, 4, elap / 1000000, 0)) rank4_elap, ");
                tmpSql.Append("max(decode(rank, 5, elap / 1000000, 0)) rank5_elap, ");
                tmpSql.Append("to_char(max(decode(rank, 1, force_matching_signature, null))) rank1, ");
                tmpSql.Append("to_char(max(decode(rank, 2, force_matching_signature, null))) rank2, ");
                tmpSql.Append("to_char(max(decode(rank, 3, force_matching_signature, null))) rank3, ");
                tmpSql.Append("to_char(max(decode(rank, 4, force_matching_signature, null))) rank4, ");
                tmpSql.Append("to_char(max(decode(rank, 5, force_matching_signature, null))) rank5  ");
                tmpSql.Append("  from(select v_sqlstat.snap_id, ");
                tmpSql.Append("       v_rank.rank, ");
                tmpSql.Append("       v_sqlstat.force_matching_signature, ");
                tmpSql.Append("       v_sqlstat.module, ");
                tmpSql.Append("       v_sqlstat.bufget, ");
                tmpSql.Append("       v_sqlstat.elap, ");
                tmpSql.Append("       v_sqlstat.cput, ");
                tmpSql.Append("       v_sqlstat.exec ");
                tmpSql.Append("            from(select SQL, force_matching_signature, rownum rank ");
                tmpSql.Append("                    from ");
                tmpSql.Append(
                    "                        (select force_matching_signature, max(sql_id) SQL, count(distinct sql_id) CNT, sum(nvl(buffer_gets_delta, 0))  ");
                tmpSql.AppendFormat("                        from raw_dba_hist_sqlstat_{0}  ", arguments.DbName);
                tmpSql.Append("                        where 1 = 1  ");
                tmpSql.Append("               AND snap_id in ( ");
                tmpSql.AppendFormat(
                    "                   select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey, arguments.InstanceNumber);
                tmpSql.AppendFormat("                        and instance_number = {0}  ", arguments.InstanceNumber);
                tmpSql.AppendFormat("                        and dbid = {0} ", arguments.DbId);
                tmpSql.Append("                        and force_matching_signature > 0  ");
                tmpSql.Append("                        and parsing_schema_name not in ('SYS', 'SYSTEM', 'SYSMAN') ");
                tmpSql.Append("                        group by force_matching_signature  ");
                tmpSql.Append("                        having count(distinct sql_id) > 2  ");
                tmpSql.Append("                        order by sum(nvl(buffer_gets_delta, 0)) desc  ");
                tmpSql.Append("                        )  ");
                tmpSql.Append("                    where rownum <= 5  ");
                tmpSql.Append("                 ) v_rank,  ");
                tmpSql.Append("                 (select snap_id,  ");
                tmpSql.Append("                          force_matching_signature,  ");
                tmpSql.Append("                          module,  ");
                tmpSql.Append("                          buffer_gets_delta bufget,  ");
                tmpSql.Append("                          elapsed_time_delta elap,  ");
                tmpSql.Append("                          cpu_time_delta cput,  ");
                tmpSql.Append("                          executions_delta exec  ");
                tmpSql.AppendFormat("                     from raw_dba_hist_sqlstat_{0}  ", arguments.DbName);
                tmpSql.Append("                    where 1 = 1  ");
                tmpSql.Append("               AND snap_id in ( ");
                tmpSql.AppendFormat(
                    "                   select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey, arguments.InstanceNumber);
                tmpSql.AppendFormat("                      and instance_number = {0}  ", arguments.InstanceNumber);
                tmpSql.AppendFormat("                      and dbid = {0}  ", arguments.DbId);
                tmpSql.Append("                      and force_matching_signature <> 0  ");
                tmpSql.Append("                 ) v_sqlstat  ");
                tmpSql.Append("           where v_sqlstat.force_matching_signature = v_rank.force_matching_signature  ");
                tmpSql.Append("          ) v1,  ");
                tmpSql.Append("          (select sn.snap_id,  ");
                tmpSql.Append("                   sn.end_interval_time,  ");
                tmpSql.Append("                   nvl(value - lag(value) over(partition by sy.stat_name order by sy.snap_id), 0) delta_bufget_tot  ");
                tmpSql.AppendFormat("              from raw_dba_hist_snapshot_{0} sn,  ", arguments.DbName);
                tmpSql.AppendFormat("                   raw_dba_hist_sysstat_{0} sy ", arguments.DbName);
                tmpSql.Append("             where sn.snap_id = sy.snap_id ");
                tmpSql.Append("               and sn.instance_number = sy.instance_number ");
                tmpSql.Append("               and sn.dbid = sy.dbid ");
                tmpSql.Append("               and sy.stat_name = 'session logical reads' ");
                tmpSql.Append("               AND sy.snap_id in ( ");
                tmpSql.AppendFormat(
                    "                   select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey, arguments.InstanceNumber);
                tmpSql.AppendFormat("               and sy.instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("               and sy.dbid = {0} ", arguments.DbId);
                tmpSql.Append("          ) sn ");
                tmpSql.Append(" where v1.snap_id(+) = sn.snap_id ");
                tmpSql.Append("   and sn.snap_id >  ");
                tmpSql.AppendFormat(
                    "                  ( select min(snap_id) from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey, arguments.InstanceNumber);
                tmpSql.Append(" group by sn.snap_id, ");
                tmpSql.Append("       to_char(sn.end_interval_time, 'yyyy-mm-dd hh24:mi:ss') ");
                tmpSql.Append(" order by sn.snap_id; ");

                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance
                    .WriteServerLog(
                        MethodInfo.GetCurrentMethod().Name,
                        LogBase._LOGTYPE_TRACE_ERROR,
                        this.Requester.IP,
                        string.Format(" Biz Component Exception occured: {0}", ex.ToString()),
                        false);
                throw ex;
            }
        }

        //80
        public void GetSQL_literal_02(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                if (string.IsNullOrEmpty(arguments.DbId) ||
                    string.IsNullOrEmpty(arguments.DbName) ||
                    string.IsNullOrEmpty(arguments.InstanceNumber) ||
                    string.IsNullOrEmpty(arguments.StartTimeKey) ||
                    string.IsNullOrEmpty(arguments.EndTimeKey))
                {
                    this.ExecutingValue = null;
                    return;
                }

                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("select SQL, '\"'||force_matching_signature||'\"' force_matching_signature, rownum rank  ");
                tmpSql.Append("from ");
                tmpSql.Append("(select force_matching_signature, max(sql_id) SQL, count(distinct sql_id) CNT,sum(nvl(buffer_gets_delta,0)) ");
                tmpSql.AppendFormat("        from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                tmpSql.Append("where 1 = 1 ");
                tmpSql.Append("               AND snap_id in ( ");
                tmpSql.AppendFormat(
                    "                   select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey, arguments.InstanceNumber);
                tmpSql.AppendFormat("and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("and dbid = {0} ", arguments.DbId);
                tmpSql.Append("and force_matching_signature > 0 ");
                tmpSql.Append("and parsing_schema_name not in ('SYS','SYSTEM','SYSMAN') ");
                tmpSql.Append("group by force_matching_signature ");
                tmpSql.Append("having count(distinct sql_id) > 2 ");
                tmpSql.Append("order by sum(nvl(buffer_gets_delta,0)) desc ");
                tmpSql.Append(") ");
                tmpSql.Append("where rownum <=5 ");

                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance
                    .WriteServerLog(
                        MethodInfo.GetCurrentMethod().Name,
                        LogBase._LOGTYPE_TRACE_ERROR,
                        this.Requester.IP,
                        string.Format(" Biz Component Exception occured: {0}", ex.ToString()),
                        false);
                throw ex;
            }
        }

        //80
        public void GetSQL_literal_03(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                if (string.IsNullOrEmpty(arguments.DbId) ||
                    string.IsNullOrEmpty(arguments.DbName) ||
                    string.IsNullOrEmpty(arguments.InstanceNumber) ||
                    string.IsNullOrEmpty(arguments.StartTimeKey) ||
                    string.IsNullOrEmpty(arguments.EndTimeKey))
                {
                    this.ExecutingValue = null;
                    return;
                }

                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("select SQL, '\"'||force_matching_signature||'\"' force_matching_signature, rownum rank  ");
                tmpSql.Append("from ");
                tmpSql.Append("(select force_matching_signature, max(sql_id) SQL, count(distinct sql_id) CNT,sum(nvl(buffer_gets_delta,0)) ");
                tmpSql.AppendFormat("        from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                tmpSql.Append("where 1 = 1 ");
                tmpSql.Append("               AND snap_id in ( ");
                tmpSql.AppendFormat(
                    "                   select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("and instance_number = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("and dbid = {0} ", arguments.DbId);
                tmpSql.Append("and force_matching_signature > 0 ");
                tmpSql.Append("and parsing_schema_name not in ('SYS','SYSTEM','SYSMAN') ");
                tmpSql.Append("group by force_matching_signature ");
                tmpSql.Append("having count(distinct sql_id) > 2 ");
                tmpSql.Append("order by sum(nvl(buffer_gets_delta,0)) desc ");
                tmpSql.Append(") ");
                tmpSql.Append("where rownum <=5 ");

                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance
                    .WriteServerLog(
                        MethodInfo.GetCurrentMethod().Name,
                        LogBase._LOGTYPE_TRACE_ERROR,
                        this.Requester.IP,
                        string.Format(" Biz Component Exception occured: {0}", ex.ToString()),
                        false);
                throw ex;
            }
        }

        public void GetSQL_literal_04()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {

                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("select dbms_utility.get_time time_pre from dual ");
                

                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance
                    .WriteServerLog(
                        MethodInfo.GetCurrentMethod().Name,
                        LogBase._LOGTYPE_TRACE_ERROR,
                        this.Requester.IP,
                        string.Format(" Biz Component Exception occured: {0}", ex.ToString()),
                        false);
                throw ex;
            }
        }

        public void GetSQL_literal_get_01(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {

                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select sn.snap_id,
                       to_char(sn.end_interval_time,'yyyy-mm-dd hh24:mi:ss') \""Timestamp\"",
                       max(decode(rank, 1, bufget, 0)) rank1_bufget,
                       max(decode(rank, 2, bufget, 0)) rank2_bufget,
                       max(decode(rank, 3, bufget, 0)) rank3_bufget,
                       max(decode(rank, 4, bufget, 0)) rank4_bufget,
                       max(decode(rank, 5, bufget, 0)) rank5_bufget,
                       max(decode(rank, 1, exec, 0)) rank1_exec,
                       max(decode(rank, 2, exec, 0)) rank2_exec,
                       max(decode(rank, 3, exec, 0)) rank3_exec,
                       max(decode(rank, 4, exec, 0)) rank4_exec,
                       max(decode(rank, 5, exec, 0)) rank5_exec,
                       decode(max(decode(rank, 1, exec, 0)), 0, 0, max(decode(rank, 1, bufget, 0)) / max(decode(rank, 1, exec, 0))) rank1_bufget_per_exec,
                       decode(max(decode(rank, 2, exec, 0)), 0, 0, max(decode(rank, 2, bufget, 0)) / max(decode(rank, 2, exec, 0))) rank2_bufget_per_exec,
                       decode(max(decode(rank, 3, exec, 0)), 0, 0, max(decode(rank, 3, bufget, 0)) / max(decode(rank, 3, exec, 0))) rank3_bufget_per_exec,
                       decode(max(decode(rank, 4, exec, 0)), 0, 0, max(decode(rank, 4, bufget, 0)) / max(decode(rank, 4, exec, 0))) rank4_bufget_per_exec,
                       decode(max(decode(rank, 5, exec, 0)), 0, 0, max(decode(rank, 5, bufget, 0)) / max(decode(rank, 5, exec, 0))) rank5_bufget_per_exec,
                       decode(max(delta_bufget_tot), 0, 0, max(decode(rank, 1, bufget, 0)) / max(delta_bufget_tot)) rank1_bufget_per_tot,
                       decode(max(delta_bufget_tot), 0, 0, max(decode(rank, 2, bufget, 0)) / max(delta_bufget_tot)) rank2_bufget_per_tot,
                       decode(max(delta_bufget_tot), 0, 0, max(decode(rank, 3, bufget, 0)) / max(delta_bufget_tot)) rank3_bufget_per_tot,
                       decode(max(delta_bufget_tot), 0, 0, max(decode(rank, 4, bufget, 0)) / max(delta_bufget_tot)) rank4_bufget_per_tot,
                       decode(max(delta_bufget_tot), 0, 0, max(decode(rank, 5, bufget, 0)) / max(delta_bufget_tot)) rank5_bufget_per_tot,
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
                       to_char(max(decode(rank, 1, force_matching_signature, null))) rank1,
                       to_char(max(decode(rank, 2, force_matching_signature, null))) rank2,
                       to_char(max(decode(rank, 3, force_matching_signature, null))) rank3,
                       to_char(max(decode(rank, 4, force_matching_signature, null))) rank4,
                       to_char(max(decode(rank, 5, force_matching_signature, null))) rank5
                  from(select v_sqlstat.snap_id,
                                 v_rank.rank,
                                 v_sqlstat.force_matching_signature,
                                 v_sqlstat.module,
                                 v_sqlstat.bufget,
                                 v_sqlstat.elap,
                                 v_sqlstat.cput,
                                 v_sqlstat.exec
                            from(select SQL, force_matching_signature, rownum rank

                                    from
                                        (select force_matching_signature, max(sql_id) SQL, count(distinct sql_id) CNT, sum(nvl(buffer_gets_delta, 0))
                                        from raw_dba_hist_sqlstat_{0} ", arguments.DbName);
                //tmpSql.AppendFormat(" where snap_id between 1 + &snap_fr and & snap_to ");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and instance_number = {0}",arguments.InstanceNumber);


                tmpSql.AppendFormat(" and dbid = {0}",arguments.DbId);

                tmpSql.AppendFormat(@"  and force_matching_signature > 0

                                        and parsing_schema_name not in ('SYS', 'SYSTEM', 'SYSMAN')

                                        group by force_matching_signature

                                        having count(distinct sql_id) > 2

                                        order by sum(nvl(buffer_gets_delta, 0)) desc
                                        )

                                    where rownum <= 5
                                 ) v_rank,
                                 (select snap_id,
                                          force_matching_signature,
                                          module,
                                          buffer_gets_delta bufget,
                                          elapsed_time_delta elap,
                                          cpu_time_delta cput,
                                          executions_delta exec
                                     from raw_dba_hist_sqlstat_{0}", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between 1 + &snap_fr and & snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and instance_number =  {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and dbid = {0}", arguments.DbId);

                tmpSql.AppendFormat(@"   and force_matching_signature <> 0
                                 ) v_sqlstat
                           where v_sqlstat.force_matching_signature = v_rank.force_matching_signature
                          ) v1,
                          (select sn.snap_id,
                                   sn.end_interval_time,
                                   nvl(value - lag(value) over(partition by sy.stat_name order by sy.snap_id), 0) delta_bufget_tot
                             from raw_dba_hist_snapshot_{0} sn,
                                  raw_dba_hist_sysstat_{0} sy
                            where sn.snap_id = sy.snap_id
                               and sn.instance_number = sy.instance_number
                               and sn.dbid = sy.dbid
                               and sy.stat_name = 'session logical reads'", arguments.DbName);
                //tmpSql.AppendFormat("   and sy.snap_id between &snap_fr and & snap_to");
                tmpSql.Append("               and sy.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and sy.instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("   and sy.dbid = {0}", arguments.DbId);
                tmpSql.AppendFormat("  ) sn                 where v1.snap_id(+) = sn.snap_id");
                tmpSql.AppendFormat("    and sn.snap_id > ");
                tmpSql.AppendFormat(" ( select min(snap_id) from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat(@"  group by sn.snap_id,
                to_char(sn.end_interval_time, 'yyyy-mm-dd hh24:mi:ss')
                 order by sn.snap_id ");


                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance
                    .WriteServerLog(
                        MethodInfo.GetCurrentMethod().Name,
                        LogBase._LOGTYPE_TRACE_ERROR,
                        this.Requester.IP,
                        string.Format(" Biz Component Exception occured: {0}", ex.ToString()),
                        false);
                throw ex;
            }
        }

        public void GetSQL_literal_get_02(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {

                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select SQL, '\""'||force_matching_signature||'\""' force_matching_signature, rownum rank  

    from
        (select force_matching_signature, max(sql_id) SQL, count(distinct sql_id) CNT, sum(nvl(buffer_gets_delta, 0))

        from raw_dba_hist_sqlstat_{0}",arguments.DbName);

                //tmpSql.AppendFormat("  where snap_id between 1 + &snap_fr and & snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and instance_number = {0}", arguments.InstanceNumber);

                tmpSql.AppendFormat("  and dbid = {0}", arguments.DbId);

                tmpSql.AppendFormat(@"  and force_matching_signature > 0
        and parsing_schema_name not in ('SYS', 'SYSTEM', 'SYSMAN')
        group by force_matching_signature
        having count(distinct sql_id) > 2
        order by sum(nvl(buffer_gets_delta, 0)) desc
        )
where rownum <= 5 ");


                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance
                    .WriteServerLog(
                        MethodInfo.GetCurrentMethod().Name,
                        LogBase._LOGTYPE_TRACE_ERROR,
                        this.Requester.IP,
                        string.Format(" Biz Component Exception occured: {0}", ex.ToString()),
                        false);
                throw ex;
            }
        }

        public void GetSQL_literal_get_03(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {

                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select rank,SQL,
       '\""' || v1.force_matching_signature || '\""' force_matching_signature,
       v1.module,
       v1.CNT,
       nvl(replace(replace(replace(to_char(substr(st.sql_text, 1, 2500)), chr(9), ' '), chr(10), ' '), chr(13), ' '), '** Not Found **') \""SQL Text\""
  from
      (select SQL, CNT, force_matching_signature, dbid, module, rownum rank

            from
                (select force_matching_signature, max(sql_id) SQL, count(distinct sql_id) CNT, sum(nvl(buffer_gets_delta, 0)), dbid, module

                 from raw_dba_hist_sqlstat_{0}", arguments.DbName);

                //tmpSql.AppendFormat("  where snap_id between 1 + &snap_fr and & snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and instance_number =  {0}", arguments.InstanceNumber);

                tmpSql.AppendFormat("  and dbid = {0}", arguments.DbId);

                tmpSql.AppendFormat(@"  and force_matching_signature > 0
                 and parsing_schema_name not in ('SYS', 'SYSTEM', 'SYSMAN')
                 group by force_matching_signature, dbid, module
                 having count(distinct sql_id) > 2
                 order by sum(nvl(buffer_gets_delta, 0)) desc
                )
        where rownum <= 5
      ) v1,
      dba_hist_sqltext st
where st.sql_id(+) = v1.sql
  and st.dbid(+) = v1.dbid");


                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance
                    .WriteServerLog(
                        MethodInfo.GetCurrentMethod().Name,
                        LogBase._LOGTYPE_TRACE_ERROR,
                        this.Requester.IP,
                        string.Format(" Biz Component Exception occured: {0}", ex.ToString()),
                        false);
                throw ex;
            }
        }

        public void GetSQL_literal_get_04(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {

                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"select sn.snap_id snap_id,
                        to_char(sn.end_interval_time, 'yyyy-mm-dd hh24:mi:ss') Time,
                        sum(bufget) bufget,
                        nvl(sum(bufget) / nullif(max(delta_bufget_tot), 0), 0) bufget_per_tot,
                        sum(cput) / 1000000 cput,
                        sum(elap) / 1000000 elap
                        from(
                                select SQL, force_matching_signature, rownum rank
                                from(
                                        select force_matching_signature, max(sql_id) SQL, count(distinct sql_id) CNT, sum(nvl(buffer_gets_delta, 0))
                                        from raw_dba_hist_sqlstat_{0}", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between 1 + &snap_fr and & snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and instance_number = {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and dbid = {0}", arguments.DbId);
                tmpSql.AppendFormat(@"  and force_matching_signature > 0
                                        and parsing_schema_name not in ('SYS', 'SYSTEM', 'SYSMAN')
                                        group by force_matching_signature
                                        having count(distinct sql_id) > 2
                                        order by sum(nvl(buffer_gets_delta, 0)) desc
                                        )
                                where rownum <= 5
                        ) v_rank,
                        (select snap_id,
                                 force_matching_signature,
                                 module,
                                 buffer_gets_delta bufget,
                                 elapsed_time_delta elap,
                                 cpu_time_delta cput,
                                 executions_delta exec
                         from raw_dba_hist_sqlstat_{0}", arguments.DbName);
                //tmpSql.AppendFormat("  where snap_id between 1 + &snap_fr and & snap_to");
                tmpSql.Append("               where snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and instance_number =  {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and dbid = {0}", arguments.DbId);
                tmpSql.AppendFormat(@"  and force_matching_signature > 0
                        ) v1,
		                (select sn.snap_id,
				                 sn.end_interval_time,
				                 nvl(value - lag(value) over(partition by sy.stat_name order by sy.snap_id), 0) delta_bufget_tot
                        from raw_dba_hist_snapshot_{0} sn,
                             raw_dba_hist_sysstat_{0} sy
                        where sn.snap_id = sy.snap_id
                         and sn.instance_number = sy.instance_number
                         and sn.dbid = sy.dbid
                         and sy.stat_name = 'session logical reads'", arguments.DbName);
                //tmpSql.AppendFormat("  and sy.snap_id between 1 + &snap_fr and & snap_to");
                tmpSql.Append("               where sy.snap_id in ( ");
                tmpSql.AppendFormat(" select snap_id from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  and sy.instance_number =  {0}", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and sy.dbid =  {0}", arguments.DbId);
                tmpSql.AppendFormat("  ) sn                where v1.snap_id(+) = sn.snap_id");
                tmpSql.AppendFormat("  and sn.snap_id > ");
                tmpSql.AppendFormat(" (select min(snap_id) from raw_dba_hist_snapshot_{0} where BEGIN_INTERVAL_TIME >= '{1}' AND BEGIN_INTERVAL_TIME < '{2}' AND INSTANCE_NUMBER = {3} ) ",
                    arguments.DbName,
                    arguments.StartTimeKey,
                    arguments.EndTimeKey,
                    arguments.InstanceNumber);
                tmpSql.AppendFormat("  group by sn.snap_id, to_char(sn.end_interval_time, 'yyyy-mm-dd hh24:mi:ss')                order by 1");
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance
                    .WriteServerLog(
                        MethodInfo.GetCurrentMethod().Name,
                        LogBase._LOGTYPE_TRACE_ERROR,
                        this.Requester.IP,
                        string.Format(" Biz Component Exception occured: {0}", ex.ToString()),
                        false);
                throw ex;
            }
        }
    }
}
