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
    public class SQLStatisticsBySqlID : TAP.Remoting.Server.Biz.BizComponentBase
    {



        public void GetSnap(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();


                tmpSql.AppendFormat(" SELECT t.end_time as end_interval_time, T.{0},T.sql_id ,t.action,t.module,t.INSTANCE_NUMBER,t.PARSING_SCHEMA_NAME", arguments.ParameterName);
                tmpSql.AppendFormat(@" FROM raw_dba_hist_sqlstat_{0} T   ", arguments.DbName);
                tmpSql.AppendFormat(" where 1=1 and t.begin_time>to_date('{0}','yyyy-MM-dd HH24:mi:ss')", arguments.StartTimeKey);
                tmpSql.AppendFormat(" and    t.begin_time<=to_date('{0}','yyyy-MM-dd HH24:mi:ss' ) ", arguments.EndTimeKey);
                tmpSql.AppendFormat(" and    t.INSTANCE_NUMBER = {0} ", arguments.InstanceNumber);
                tmpSql.AppendFormat("  and T.dbid in ('{0}') order by end_interval_time ", arguments.DbId);



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
