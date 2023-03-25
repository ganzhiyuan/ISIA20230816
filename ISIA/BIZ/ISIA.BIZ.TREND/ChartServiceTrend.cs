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
        public void GetImageData(ChartServiceArgsPack argument)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("SELECT * FROM TAPAWRCHARTSERVICE WHERE 1 = 1 ");

                if (!string.IsNullOrEmpty(argument.ReportDate))
                {
                    tmpSql.AppendFormat(" AND REPORTDATE IN ({0})", Utils.MakeSqlQueryIn2(argument.ReportDate));
                }
                if (!string.IsNullOrEmpty(argument.ReportDate))
                {
                    tmpSql.AppendFormat(" AND DBID IN ({0})", Utils.MakeSqlQueryIn2(argument.ReportDate));
                }
                if (!string.IsNullOrEmpty(argument.ReportDate))
                {
                    tmpSql.AppendFormat(" AND INSTANCE_NUMBER IN ({0})", Utils.MakeSqlQueryIn2(argument.ReportDate));
                }
                if (!string.IsNullOrEmpty(argument.ReportDate))
                {
                    tmpSql.AppendFormat(" AND RULENAME IN ({0})", Utils.MakeSqlQueryIn2(argument.ReportDate));
                }
                if (!string.IsNullOrEmpty(argument.ReportDate))
                {
                    tmpSql.AppendFormat(" AND RULENO IN ({0})", Utils.MakeSqlQueryIn2(argument.ReportDate));
                }
                if (!string.IsNullOrEmpty(argument.ReportDate))
                {
                    tmpSql.AppendFormat(" AND PARAMETERID IN ({0})", Utils.MakeSqlQueryIn2(argument.ReportDate));
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
