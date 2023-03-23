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
    public class SqlstatServices : TAP.Remoting.Server.Biz.BizComponentBase
    {
        public void GetSnap(EquipmentArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"SELECT  ROWNUM,t.snap_id,t.dbid,t.sql_id,{0}
                                          FROM raw_dba_hist_sqlstat_isfa T
                                         where t.snap_id in
                                               (SELECT T.Snap_Id
                                                  FROM raw_dba_hist_snapshot_isfa T
                                                 WHERE T.END_INTERVAL_TIME >=
                                                       TO_DATE('{1}', 'yyyy-MM-dd HH24:mi:ss')
                                                   and t.end_interval_time <=
                                                       TO_DATE('{2}', 'yyyy-MM-dd HH24:mi:ss'))", arguments.ParameterName,arguments.StartTime,arguments.EndTime);


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

        public void GetSqlstatPara(EquipmentArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"SELECT T.* FROM raw_dba_hist_sqlstat_isfa T where t.snap_id='{0}' and t.sql_id='{1}'",  arguments.ParameterName, arguments.MainEqp);


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
