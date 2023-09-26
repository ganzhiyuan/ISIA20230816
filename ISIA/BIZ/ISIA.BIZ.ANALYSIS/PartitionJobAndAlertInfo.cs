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
    public class PartitionJobAndAlertInfo : TAP.Remoting.Server.Biz.BizComponentBase
    {

        public void GetJobInfo(PartitionJobAlertArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendLine($@"select *
from 
(
select START_DATE, END_DATE, INSTANCE_NAME,
OWNER, TABLE_NAME, PART_NAME, STATUS,
JOB_TYPE, JOB_NAME, DETAIL, STEP_INFO,
ERR_MSG, HOST, IP_ADDRESS, MODULE, TERMINAL
from SKHY_PKG_LOG@UMSDB
order by START_DATE desc
)
where rownum <= {arguments.RowJobCount}");


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

        public void GetAlertInfo(PartitionJobAlertArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendLine($@"select *
from 
(
select to_char(ORIGINATING_TIMESTAMP, 'YYYY/MM/DD HH24:MI:SS FF6') ORIGINATING_TIMESTAMP, trunc(ORIGINATING_TIMESTAMP),
        HOST_ID,
        HOST_ADDRESS,
        MESSAGE_TYPE,
        MESSAGE_LEVEL,
        MESSAGE_ID,
        MESSAGE_GROUP,
        MODULE_ID,
        PROCESS_ID,
        MESSAGE_TEXT
from V$DIAG_ALERT_EXT@UMSDB
where 1=1
--and trunc(ORIGINATING_TIMESTAMP) = trunc(sysdate)
--and (MESSAGE_TEXT like 'ORA-75%' OR MESSAGE_TEXT like 'INF-%')
order by ORIGINATING_TIMESTAMP desc
)
where rownum <= {arguments.RowAlertCount}");


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