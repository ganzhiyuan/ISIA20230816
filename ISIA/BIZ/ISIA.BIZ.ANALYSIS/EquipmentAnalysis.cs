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


        public void GetPara(EquipmentArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                
                tmpSql.Append("SELECT A.DBID , A.STAT_NAME , A.VALUE , B.BEGIN_INTERVAL_TIME  FROM  RAW_DBA_HIST_SYSSTAT_ISFA  A  JOIN RAW_DBA_HIST_SNAPSHOT_ISFA B ON A.SNAP_ID = B.SNAP_ID ");
                tmpSql.Append(" WHERE 1=1");

                if (!string.IsNullOrEmpty(arguments.DataBase))
                {
                    tmpSql.AppendFormat(" AND  A.DBID IN ({0})", Utils.MakeSqlQueryIn2(arguments.DataBase));
                }
                if (!string.IsNullOrEmpty(arguments.ParameterName))
                {
                    tmpSql.AppendFormat(" AND  A.STAT_NAME IN ({0})", Utils.MakeSqlQueryIn2(arguments.ParameterName));
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
