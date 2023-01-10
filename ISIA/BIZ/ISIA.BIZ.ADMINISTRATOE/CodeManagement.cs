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

namespace ISIA.BIZ.ADMINISTRATOE
{
    class CodeManagement : TAP.Remoting.Server.Biz.BizComponentBase
    {
        public void GetSpec(PMArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT CATEGORY, SUBCATEGORY, NAME, DESCRIPTION,CUSTOM01,CUSTOM02,CUSTOM03,CUSTOM04, INSERTTIME, UPDATETIME, INSERTUSER, UPDATEUSER, SEQUENCES  FROM  TAPCTCODES WHERE USED='YES' AND CATEGORY <>'MESSAGE' ");

                if (!string.IsNullOrEmpty(arguments.PMCategory))
                {
                    tmpSql.AppendFormat(" AND CATEGORY IN ({0})", Utils.MakeSqlQueryIn(arguments.PMCategory, ','));
                }

                if (!string.IsNullOrEmpty(arguments.PMItem))
                {
                    tmpSql.AppendFormat(" AND SUBCATEGORY IN ({0})", Utils.MakeSqlQueryIn(arguments.PMItem, ','));
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
