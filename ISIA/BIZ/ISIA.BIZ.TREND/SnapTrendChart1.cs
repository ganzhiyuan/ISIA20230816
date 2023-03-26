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
        //public void GetSnap(AwrCommonArgsPack arguments)
        //{
        //    DBCommunicator db = new DBCommunicator();
        //    try
        //    {
        //        StringBuilder tmpSql = new StringBuilder();

        //        tmpSql.AppendFormat(@" SELECT a.end_interval_time, t.snap_id,t.dbid,t.sql_id, T.{0}
        //                                        FROM raw_dba_hist_sqlstat_isfa T left join raw_dba_hist_snapshot_isfa a on t.snap_id=a.snap_id 
        //                                        where t.snap_id in
        //                                       (SELECT T.Snap_Id
        //                                          FROM raw_dba_hist_snapshot_isfa T
        //                                         WHERE T.END_INTERVAL_TIME >=
        //                                               TO_DATE('{1}', 'yyyy-MM-dd HH24:mi:ss')
        //                                           and t.end_interval_time <=
        //                                               TO_DATE('{2}', 'yyyy-MM-dd HH24:mi:ss'))", arguments.ParameterName,arguments.StartTimeKey,arguments.EndTimeKey);


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

        //public void GetSqlstatPara(AwrCommonArgsPack arguments)
        //{
        //    DBCommunicator db = new DBCommunicator();
        //    try
        //    {
        //        StringBuilder tmpSql = new StringBuilder();

        //        tmpSql.AppendFormat(@"SELECT T.* FROM raw_dba_hist_sqlstat_isfa T where t.snap_id='{0}' and t.sql_id='{1}'",  arguments.ParameterName, arguments.SqlId);


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

        //public void GetSqlstatByUnit(AwrCommonArgsPack arguments)
        //{
        //    DBCommunicator db = new DBCommunicator();
        //    try
        //    {
        //        StringBuilder tmpSql = new StringBuilder();

        //        tmpSql.AppendFormat(@"SELECT a.end_interval_time,t.sql_id,{0} as typNum
        //          FROM raw_dba_hist_sqlstat_isfa T left join raw_dba_hist_snapshot_isfa a on t.snap_id=a.snap_id 
        //         where t.snap_id in
        //               (SELECT T.Snap_Id
        //                  FROM raw_dba_hist_snapshot_isfa T
        //                 WHERE T.END_INTERVAL_TIME >
        //                       TO_DATE('{1}', 'yyyy-MM-dd HH24:mi:ss')
        //                   and t.end_interval_time <=
        //                       TO_DATE('{2}', 'yyyy-MM-dd HH24:mi:ss'))", Convert.ToDecimal(arguments.ParameterType), arguments.StartTimeKey, arguments.EndTimeKey);
        //        //arguments.ParameterType 

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

        //public void GetSqlstatModels(EquipmentArgsPack arguments)
        //{
        //    DBCommunicator db = new DBCommunicator();
        //    try
        //    {
        //        StringBuilder tmpSql = new StringBuilder();

        //        tmpSql.AppendFormat(@"SELECT b.end_interval_time, a.command_type,T.* FROM raw_dba_hist_sqlstat_isfa T 
        //            left join raw_dba_hist_sqltext_isfa a 
        //            on t.sql_id=a.sql_id and t.dbid=a.dbid 
        //            left join raw_dba_hist_snapshot_isfa b on t.snap_id=b.snap_id
        //                where 1=1 and b.end_interval_time>to_date('{0}','yyyy-MM-dd HH24:mi:ss')
        //                and    b.end_interval_time<=to_date('{1}','yyyy-MM-dd HH24:mi:ss')
        //                ",arguments.StartTime,arguments.EndTime);
        //        if (!string.IsNullOrEmpty(arguments.CommuntionStatus))
        //        {
        //            tmpSql.AppendFormat(@" and a.command_type in ({0})", Utils.MakeSqlQueryIn(arguments.CommuntionStatus,','));
        //        }
        //        if (!string.IsNullOrEmpty(arguments.ModelLevels))
        //        {
        //            tmpSql.AppendFormat(@" and t.module in ({0}) ", Utils.MakeSqlQueryIn(arguments.ModelLevels,','));
        //        }


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
    }
}
