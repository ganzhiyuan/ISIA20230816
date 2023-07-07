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

namespace ISIA.BIZ.ANALYSIS
{
    class ParameterClusteringAnalysis : TAP.Remoting.Server.Biz.BizComponentBase
    {

        public void GetParaValue(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();


                if (arguments.ParameterType == "STATISTIC")
                {
                    tmpSql.Append(" SELECT T.SNAP_ID,T.DBID, T.stat_name PARAMENT_NAME, (TO_NUMBER (t.VALUE) - t.next_value) N_VALUE,t.end_interval_time from  (");

                    tmpSql.AppendFormat(@" select A.* ,B.end_interval_time ,
                    lag(a.value, 1, null) over (partition by a.stat_name order by a.snap_id )  next_value  from  RAW_DBA_HIST_SYSSTAT_{0}  A 
                    left join RAW_DBA_HIST_SNAPSHOT_{0} ", arguments.DbName);

                    tmpSql.AppendFormat(@" B on A.snap_id = b.snap_id 
                    AND A.INSTANCE_NUMBER = b.INSTANCE_NUMBER
                    AND A.DBID = b.DBID
                    where 1=1 and b.begin_interval_time>to_date('{0}','yyyy-MM-dd HH24:mi:ss')
                    and  b.begin_interval_time <= to_date('{1}','yyyy-MM-dd HH24:mi:ss' ) ", arguments.StartTimeKey, arguments.EndTimeKey);

                    tmpSql.AppendFormat(@" AND A.DBID = '{0}' ", arguments.DbId);

                    tmpSql.AppendFormat(@" AND A.INSTANCE_NUMBER = {0} ", arguments.InstanceNumber);

                    tmpSql.AppendFormat(@" and  stat_name in ( {0})  order by B.end_interval_time ) T ", Utils.MakeSqlQueryIn2(arguments.ParameterName));
                }
                else if (arguments.ParameterType == "OS")
                {
                    tmpSql.Append(" SELECT T.SNAP_ID,T.DBID, T.stat_name PARAMENT_NAME, (TO_NUMBER (t.VALUE) - t.next_value) N_VALUE,t.end_interval_time from  (");

                    tmpSql.AppendFormat(@" select A.* ,B.end_interval_time ,
                    lag(a.value, 1, null) over (partition by a.stat_name order by a.snap_id )  next_value  from  RAW_DBA_HIST_OSSTAT_{0}  A 
                    left join RAW_DBA_HIST_SNAPSHOT_{0} ", arguments.DbName);

                    tmpSql.AppendFormat(@" B on A.snap_id = b.snap_id
                    AND A.INSTANCE_NUMBER = b.INSTANCE_NUMBER
                    AND A.DBID = b.DBID
                    where 1=1 and b.begin_interval_time>to_date('{0}','yyyy-MM-dd HH24:mi:ss')
                    and  b.begin_interval_time <= to_date('{1}','yyyy-MM-dd HH24:mi:ss' ) ", arguments.StartTimeKey, arguments.EndTimeKey);

                    tmpSql.AppendFormat(@" AND A.DBID = '{0}' ", arguments.DbId);

                    tmpSql.AppendFormat(@" AND A.INSTANCE_NUMBER = {0} ", arguments.InstanceNumber);

                    tmpSql.AppendFormat(@" and  stat_name in ( {0})  order by B.end_interval_time ) T ", Utils.MakeSqlQueryIn2(arguments.ParameterName));
                }
                else if (arguments.ParameterType == "METRIC")
                {
                    tmpSql.AppendFormat(@"  SELECT A.SNAP_ID,A.DBID, A.METRIC_NAME PARAMENT_NAME, A.AVERAGE N_VALUE,
                    B.end_interval_time FROM RAW_DBA_HIST_SYSMETRIC_SUMMARY_{0} A LEFT JOIN RAW_DBA_HIST_SNAPSHOT_{0} B
                    ON A.snap_id = b.snap_id 
                    AND A.INSTANCE_NUMBER = b.INSTANCE_NUMBER
                    AND A.DBID = b.DBID
                    ", arguments.DbName);

                    tmpSql.AppendFormat(@" WHERE     1 = 1
                    AND b.begin_interval_time >
                    TO_DATE ('{0}', 'yyyy-MM-dd HH24:mi:ss')
                    AND b.begin_interval_time <=
                    TO_DATE ('{1}', 'yyyy-MM-dd HH24:mi:ss') ", arguments.StartTimeKey, arguments.EndTimeKey);

                    tmpSql.AppendFormat(@" AND A.DBID = '{0}' ", arguments.DbId);

                    tmpSql.AppendFormat(@" AND A.INSTANCE_NUMBER  = {0} ", arguments.InstanceNumber);

                    tmpSql.AppendFormat(@" AND metric_name IN ( {0} ) order by a.metric_name ,B.end_interval_time", Utils.MakeSqlQueryIn2(arguments.ParameterName));
                }
                else {

                    tmpSql.Append("select * from (");

                    tmpSql.Append(" SELECT T.SNAP_ID,T.DBID, T.stat_name PARAMENT_NAME, (TO_NUMBER (t.VALUE) - t.next_value) N_VALUE,t.end_interval_time from  (");

                    tmpSql.AppendFormat(@" select A.* ,B.end_interval_time ,
                    lag(a.value, 1, null) over (partition by a.stat_name order by a.snap_id )  next_value  from  RAW_DBA_HIST_SYSSTAT_{0}  A 
                    left join RAW_DBA_HIST_SNAPSHOT_{0} ", arguments.DbName);

                    tmpSql.AppendFormat(@" B on A.snap_id = b.snap_id  
                    AND A.INSTANCE_NUMBER = b.INSTANCE_NUMBER
                    AND A.DBID = b.DBID
                    where 1=1 and b.begin_interval_time>to_date('{0}','yyyy-MM-dd HH24:mi:ss')
                    and  b.begin_interval_time <= to_date('{1}','yyyy-MM-dd HH24:mi:ss' ) ", arguments.StartTimeKey, arguments.EndTimeKey);

                    tmpSql.AppendFormat(@" AND A.DBID = '{0}' ", arguments.DbId);

                    tmpSql.AppendFormat(@" AND A.INSTANCE_NUMBER  = {0} ", arguments.InstanceNumber);

                    tmpSql.AppendFormat(@" and  stat_name in ( {0})  ) T ", Utils.MakeSqlQueryIn2(arguments.ParameterName));

                    tmpSql.Append(" union all");

                    tmpSql.AppendFormat(@"  SELECT A.SNAP_ID,A.DBID, A.METRIC_NAME PARAMENT_NAME, A.AVERAGE N_VALUE,
                    B.end_interval_time FROM RAW_DBA_HIST_SYSMETRIC_SUMMARY_{0} A LEFT JOIN RAW_DBA_HIST_SNAPSHOT_{0} B
                    ON A.snap_id = b.snap_id 
                    AND A.INSTANCE_NUMBER = b.INSTANCE_NUMBER
                    AND A.DBID = b.DBID ", arguments.DbName);

                    tmpSql.AppendFormat(@"  WHERE     1 = 1
                    AND b.begin_interval_time >
                    TO_DATE ('{0}', 'yyyy-MM-dd HH24:mi:ss')
                    AND b.begin_interval_time <=
                    TO_DATE ('{1}', 'yyyy-MM-dd HH24:mi:ss') ", arguments.StartTimeKey, arguments.EndTimeKey);

                    tmpSql.AppendFormat(@" AND A.DBID IN '{0}' ", arguments.DbId);

                    tmpSql.AppendFormat(@" AND A.INSTANCE_NUMBER  = {0} ", arguments.InstanceNumber);

                    tmpSql.AppendFormat(@" AND metric_name IN ( {0} ) ", Utils.MakeSqlQueryIn2(arguments.ParameterName));

                    tmpSql.Append(" union all");

                    //OS
                    tmpSql.Append(" SELECT T.SNAP_ID,T.DBID, T.stat_name PARAMENT_NAME, (TO_NUMBER (t.VALUE) - t.next_value) N_VALUE,t.end_interval_time from  ( ");

                    tmpSql.AppendFormat(@" select A.* ,B.end_interval_time ,
                    lag(a.value, 1, null) over (partition by a.stat_name order by a.snap_id )  next_value  from  RAW_DBA_HIST_OSSTAT_{0}  A 
                    left join RAW_DBA_HIST_SNAPSHOT_{0} ", arguments.DbName);

                    tmpSql.AppendFormat(@" B on A.snap_id = b.snap_id
                    AND A.DBID = b.DBID 
                    AND A.INSTANCE_NUMBER = b.INSTANCE_NUMBER where 1=1 and b.end_interval_time>to_date('{0}','yyyy-MM-dd HH24:mi:ss')
                    and  b.end_interval_time <= to_date('{1}','yyyy-MM-dd HH24:mi:ss' ) ", arguments.StartTimeKey, arguments.EndTimeKey);

                    tmpSql.AppendFormat(@" AND A.DBID IN '{0}' ", arguments.DbId);

                    tmpSql.AppendFormat(@" AND A.INSTANCE_NUMBER = {0} ", arguments.InstanceNumber);

                    tmpSql.AppendFormat(@" and  stat_name in ( {0})  order by B.end_interval_time ) T ", Utils.MakeSqlQueryIn2(arguments.ParameterName));


                    tmpSql.Append(" ) order by parament_name ,end_interval_time ");

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


    }
}
