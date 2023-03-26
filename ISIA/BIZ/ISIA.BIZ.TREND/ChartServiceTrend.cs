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
    public class ChartServiceTrend : TAP.Remoting.Server.Biz.BizComponentBase
    {
        public void GetImageData(ChartServiceArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("SELECT * FROM TAPAWRCHARTSERVICE WHERE 1 = 1 ");

                if (!string.IsNullOrEmpty(arguments.ReportDate))
                {
                    tmpSql.AppendFormat(" AND REPORTDATE IN ({0})", Utils.MakeSqlQueryIn2(arguments.ReportDate));
                }
                if (!string.IsNullOrEmpty(arguments.DbId))
                {
                    tmpSql.AppendFormat(" AND DBID IN ({0})", Utils.MakeSqlQueryIn2(arguments.DbId));
                }
                if (!string.IsNullOrEmpty(arguments.Instance_Number))
                {
                    tmpSql.AppendFormat(" AND INSTANCE_NUMBER IN ({0})", Utils.MakeSqlQueryIn2(arguments.Instance_Number));
                }
                if (!string.IsNullOrEmpty(arguments.RuleName))
                {
                    tmpSql.AppendFormat(" AND RULENAME IN ({0})", Utils.MakeSqlQueryIn2(arguments.RuleName));
                }
                if (!string.IsNullOrEmpty(arguments.RuleNo))
                {
                    tmpSql.AppendFormat(" AND RULENO IN ({0})", Utils.MakeSqlQueryIn2(arguments.RuleNo));
                }
                if (!string.IsNullOrEmpty(arguments.ParameterName))
                {
                    tmpSql.AppendFormat(" AND PARAMETERNAME IN ({0})", Utils.MakeSqlQueryIn2(arguments.ParameterName));
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
