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

        public void GetSqltext(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("SELECT T.* FROM raw_dba_hist_sqltext_{0} T where t.dbid='{1}' and t.sql_id='{2}' ",arguments.DbName, arguments.DbId, arguments.SqlId);


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

                tmpSql.AppendFormat("SELECT t.end_time as  end_interval_time,t.sql_id,{0} as typNum ", Convert.ToDecimal(arguments.ParameterType));
                tmpSql.AppendFormat("  FROM raw_dba_hist_sqlstat_{0} T  ", arguments.DbName);
                tmpSql.Append(" WHERE T.END_TIME > ");
                tmpSql.AppendFormat(" TO_DATE('{0}', 'yyyy-MM-dd HH24:mi:ss') ", arguments.StartTimeKey);
                tmpSql.Append(" and t.end_time <= ");
                tmpSql.AppendFormat(" TO_DATE('{0}', 'yyyy-MM-dd HH24:mi:ss')", arguments.EndTimeKey);
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

                tmpSql.AppendFormat("SELECT  sql_id,  COUNT(*) AS {0} ,d.module,parsing_schema_name,action,instance_number ", arguments.ParameterName);
                tmpSql.Append(@" FROM(
                        SELECT  DISTINCT  workDate, sql_id,c.module,parsing_schema_name,action,instance_number
                        FROM
                        (SELECT SUBSTR(t.end_time,0,10) workDate, MAX(t.sql_id) sql_id,t.module,t.parsing_schema_name,t.action,t.instance_number ");
                 tmpSql.AppendFormat("     FROM raw_dba_hist_sqlstat_{0} T ", arguments.DbName);
                tmpSql.AppendFormat(" where  t.end_time>TO_DATE('{0}', 'yyyy-MM-dd') ",arguments.StartTimeKey);
                tmpSql.AppendFormat("    AND t.end_time <= TO_DATE('{0}', 'yyyy-MM-dd')",arguments.EndTimeKey);
                tmpSql.AppendFormat("  GROUP BY t.end_time, t.sql_id,t.module,t.parsing_schema_name,t.action,t.instance_number  ORDER BY sql_id) c) d ");
                tmpSql.AppendFormat("  WHERE workDate >= TRUNC(TO_DATE('{0}', 'yyyy-MM-dd')) - 7 ", arguments.EndTimeKey);
                tmpSql.AppendFormat("  GROUP BY sql_id,d.module,parsing_schema_name,action,instance_number  ORDER BY {0} DESC", arguments.ParameterName);
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

        
    }
}
