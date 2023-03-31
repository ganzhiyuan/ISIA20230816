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
    public class SQLAnalysisBySQL_ID : TAP.Remoting.Server.Biz.BizComponentBase
    {



        public void GetSnap(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@" SELECT b.end_interval_time, a.command_type,T.{0},T.sql_id FROM raw_dba_hist_sqlstat_{0} T", arguments.ParameterName,arguments.DbName);

                tmpSql.AppendFormat(@"    left join raw_dba_hist_sqltext_isfa a
                    on t.sql_id=a.sql_id and t.dbid=a.dbid
                    left join raw_dba_hist_snapshot_isfa b on t.snap_id=b.snap_id
                        where 1=1 and b.end_interval_time>to_date('{1}','yyyy-MM-dd HH24:mi:ss')
                        and    b.end_interval_time<=to_date('{2}','yyyy-MM-dd HH24:mi:ss' ) 
                        and T.dbid in ('{3}')   
                        order by b.end_interval_time
                        " , arguments.StartTimeKey, arguments.EndTimeKey, arguments.DbId);


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
