﻿using ISIA.COMMON;
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
    class SqlIDClusteringAnalysis : TAP.Remoting.Server.Biz.BizComponentBase
    {

        public void GetSQLSTAT(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                
                    tmpSql.AppendFormat(" SELECT A.SNAP_ID,A.DBID,A.INSTANCE_NUMBER, A.SQL_ID , ROUND(avg(NVL (A.{0}, 0)) , 4) {0}, A.END_TIME ", arguments.ParameterName);

                    tmpSql.AppendFormat(@" FROM RAW_DBA_HIST_SQLSTAT_{0} A
                                        WHERE  1 = 1 ", arguments.DbName);

                    tmpSql.AppendFormat(@" AND A.BEGIN_TIME > TO_DATE ('{0}', 'yyyy-MM-dd HH24:mi:ss' ) 
                                           AND A.BEGIN_TIME <= TO_DATE ('{1}' , 'yyyy-MM-dd HH24:mi:ss' )",
                                           arguments.StartTimeKey, arguments.EndTimeKey);

                    tmpSql.AppendFormat(@" AND A.INSTANCE_NUMBER = {0} ", arguments.InstanceNumber);

                    tmpSql.AppendFormat(@" AND A.DBID = '{0}' ", arguments.DbId);

                    tmpSql.Append(" group by A.SQL_ID ,A.SNAP_ID, A.DBID,A.INSTANCE_NUMBER,A.END_TIME ");

                    tmpSql.Append(" ORDER BY A.END_TIME ");


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
