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

namespace ISIA.BIZ.ANALYSIS
{
    class EquipmentAnalysis : TAP.Remoting.Server.Biz.BizComponentBase
    {


        public void GetPara(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                
                tmpSql.Append(" SELECT  A.SNAP_ID , A.DBID , A.INSTANCE_NUMBER , A.STAT_ID , A.STAT_NAME  , A.VALUE , B.BEGIN_INTERVAL_TIME  FROM  RAW_DBA_HIST_SYSSTAT_ISFA  A  JOIN RAW_DBA_HIST_SNAPSHOT_ISFA B ON A.SNAP_ID = B.SNAP_ID ");
                tmpSql.Append(" WHERE 1=1");

                if (!string.IsNullOrEmpty(arguments.DbName))
                {
                    tmpSql.AppendFormat(" AND  A.DBID IN ({0})", Utils.MakeSqlQueryIn2(arguments.DbName));
                }
                if (!string.IsNullOrEmpty(arguments.ParameterName))
                {
                    tmpSql.AppendFormat(" AND  A.STAT_NAME IN ({0})", Utils.MakeSqlQueryIn2(arguments.ParameterName));
                }
                if (!string.IsNullOrEmpty(arguments.StartTimeKey))
                {
                    tmpSql.AppendFormat(" AND b.begin_interval_time > = to_date({0}, 'yyyy-mm-dd hh24:mi:ss')", Utils.MakeSqlQueryIn2(arguments.StartTimeKey));
                }
                if (!string.IsNullOrEmpty(arguments.EndTimeKey))
                {
                    tmpSql.AppendFormat(" AND b.begin_interval_time < = to_date({0}, 'yyyy-mm-dd hh24:mi:ss')", Utils.MakeSqlQueryIn2(arguments.EndTimeKey));
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

        public void GetDAYPara(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(" SELECT  STAT_NAME, ROUND(AVG(VALUE),2) AS VALUE ,TRUNC (BEGIN_INTERVAL_TIME, 'dd') AS BEGIN_INTERVAL_TIME   FROM ( ");
                tmpSql.Append(" SELECT  A.SNAP_ID , A.DBID , A.INSTANCE_NUMBER , A.STAT_ID , A.STAT_NAME  , A.VALUE , B.BEGIN_INTERVAL_TIME  FROM  RAW_DBA_HIST_SYSSTAT_ISFA  A  JOIN RAW_DBA_HIST_SNAPSHOT_ISFA B ON A.SNAP_ID = B.SNAP_ID ");
                tmpSql.Append(" WHERE 1=1");

                if (!string.IsNullOrEmpty(arguments.DbName))
                {
                    tmpSql.AppendFormat(" AND  A.DBID IN ({0})", Utils.MakeSqlQueryIn2(arguments.DbName));
                }
                if (!string.IsNullOrEmpty(arguments.ParameterName))
                {
                    tmpSql.AppendFormat(" AND  A.STAT_NAME IN ({0})", Utils.MakeSqlQueryIn2(arguments.ParameterName));
                }
                if (!string.IsNullOrEmpty(arguments.StartTimeKey))
                {
                    tmpSql.AppendFormat(" AND b.begin_interval_time > = to_date({0}, 'yyyy-mm-dd hh24:mi:ss')", Utils.MakeSqlQueryIn2(arguments.StartTimeKey));
                }
                if (!string.IsNullOrEmpty(arguments.EndTimeKey))
                {
                    tmpSql.AppendFormat(" AND b.begin_interval_time < = to_date({0}, 'yyyy-mm-dd hh24:mi:ss')", Utils.MakeSqlQueryIn2(arguments.EndTimeKey));
                }
                tmpSql.Append(" ) GROUP BY TRUNC (BEGIN_INTERVAL_TIME, 'dd') , STAT_NAME  order by BEGIN_INTERVAL_TIME");
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

        public void GetHOURPara(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(" SELECT  STAT_NAME, ROUND(AVG(VALUE),2) AS VALUE ,TRUNC (BEGIN_INTERVAL_TIME, 'hh') AS BEGIN_INTERVAL_TIME   FROM ( ");
                tmpSql.Append(" SELECT  A.SNAP_ID , A.DBID , A.INSTANCE_NUMBER , A.STAT_ID , A.STAT_NAME  , A.VALUE , B.BEGIN_INTERVAL_TIME  FROM  RAW_DBA_HIST_SYSSTAT_ISFA  A  JOIN RAW_DBA_HIST_SNAPSHOT_ISFA B ON A.SNAP_ID = B.SNAP_ID ");
                tmpSql.Append(" WHERE 1=1");



                if (!string.IsNullOrEmpty(arguments.DbName))
                {
                    tmpSql.AppendFormat(" AND  A.DBID IN ({0})", Utils.MakeSqlQueryIn2(arguments.DbName));
                }
                if (!string.IsNullOrEmpty(arguments.ParameterName))
                {
                    tmpSql.AppendFormat(" AND  A.STAT_NAME IN ({0})", Utils.MakeSqlQueryIn2(arguments.ParameterName));
                }
                if (!string.IsNullOrEmpty(arguments.StartTimeKey))
                {
                    tmpSql.AppendFormat(" AND b.begin_interval_time > = to_date({0}, 'yyyy-mm-dd hh24:mi:ss')", Utils.MakeSqlQueryIn2(arguments.StartTimeKey));
                }
                if (!string.IsNullOrEmpty(arguments.EndTimeKey))
                {
                    tmpSql.AppendFormat(" AND b.begin_interval_time < = to_date({0}, 'yyyy-mm-dd hh24:mi:ss')", Utils.MakeSqlQueryIn2(arguments.EndTimeKey));
                }
                tmpSql.Append(" ) GROUP BY TRUNC (BEGIN_INTERVAL_TIME, 'hh') , STAT_NAME  order by BEGIN_INTERVAL_TIME");
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



        public void GetDBID(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append(" SELECT  * from TAPCTCODES ");
                tmpSql.Append("  WHERE 1=1");

                if (!string.IsNullOrEmpty(arguments.DbId))
                {
                    tmpSql.AppendFormat(" AND  CUSTOM01 IN ({0})", Utils.MakeSqlQueryIn2(arguments.DbId));
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
