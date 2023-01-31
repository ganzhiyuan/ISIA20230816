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


        public void GetMetric(EquipmentArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                
                tmpSql.Append("SELECT * FROM  RAW_DBA_HIST_SYSMETRIC_SUMMARY_ISFA WHERE 1=1  ");
                

                if (!string.IsNullOrEmpty(arguments.DataBase))
                {
                    tmpSql.AppendFormat("AND  DBID IN ({0})", Utils.MakeSqlQueryIn2(arguments.DataBase));
                }
                if (!string.IsNullOrEmpty(arguments.Metric))
                {
                    tmpSql.AppendFormat("AND  METRIC_NAME IN ({0})", Utils.MakeSqlQueryIn2(arguments.Metric));
                }
                
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Select(tmpSql.ToString()).Tables[0];
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
