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
    class ParameterTClusteringAnalysis : TAP.Remoting.Server.Biz.BizComponentBase
    {

        public void GetParaValue(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("select T.*,  (to_number(t.value) - t.next_value) N_VALUE  from  (");


                tmpSql.AppendFormat(@" select A.* ,B.end_interval_time ,
                lag(a.value, 1, null) over (partition by a.stat_name order by a.snap_id )  next_value  from  RAW_DBA_HIST_SYSSTAT_{0}  A 
                 left join RAW_DBA_HIST_SNAPSHOT_{0} ", arguments.DbName);


                tmpSql.AppendFormat(@" B on A.snap_id = b.snap_id where 1=1 and b.end_interval_time>to_date('{0}','yyyy-MM-dd HH24:mi:ss')
                        and  b.end_interval_time <= to_date('{1}','yyyy-MM-dd HH24:mi:ss' ) ", arguments.StartTimeKey, arguments.EndTimeKey);

                tmpSql.AppendFormat(@" and  stat_name in ( {0})  order by B.end_interval_time ) T", Utils.MakeSqlQueryIn2(arguments.ParameterName));


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
