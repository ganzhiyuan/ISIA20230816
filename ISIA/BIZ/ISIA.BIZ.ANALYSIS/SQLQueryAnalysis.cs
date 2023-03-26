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
    public class SQLQueryAnalysis : TAP.Remoting.Server.Biz.BizComponentBase
    {



        public void GetSnap()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@" SELECT b.end_interval_time, a.command_type,T.CPU_TIME_TOTAL,T.sql_id FROM raw_dba_hist_sqlstat_isfa T
                    left join raw_dba_hist_sqltext_isfa a
                    on t.sql_id=a.sql_id and t.dbid=a.dbid
                    left join raw_dba_hist_snapshot_isfa b on t.snap_id=b.snap_id
                        where 1=1 and b.end_interval_time>to_date('2023-03-20 09:55:16','yyyy-MM-dd HH24:mi:ss')
                        and    b.end_interval_time<=to_date('2023-03-24 09:55:16','yyyy-MM-dd HH24:mi:ss' )  order by b.end_interval_time
                        ");


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



        public void GetSqlstat_isfa()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append(" SELECT rownum, t.DBID,t.SQL_ID,t.CPU_TIME_DELTA FROM raw_dba_hist_sqlstat_isfa T where rownum<11 ");
                tmpSql.Append(" WHERE 1=1");

                

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

        public void GetSqltext_isfa(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(" SELECT T.Sql_Text FROM raw_dba_hist_sqltext_isfa T where t.sql_id='{0}' and t.dbid='{1}' ",
                    arguments.Custom02, arguments.Custom02);



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
