using ISIA.COMMON;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using TAP;
using TAP.Data.DataBase.Communicators;
using TAP.Remoting;

namespace ISIA.BIZ.ANALYSIS
{
    class SqlInfluenceAnalysis : TAP.Remoting.Server.Biz.BizComponentBase
    {


        public void GetDBName(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("select DBID, DBNAME   from ISIA.TAPCTDATABASE ");


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


        public void GetSqlInfluenceData(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();


                tmpSql.AppendFormat(@" select rownum,t.* from (select stat.sql_id sql_id, min(begin_interval_time) begin_interval_time,max(end_interval_time) end_interval_time, NVL(sum({0}),0) {0} ", arguments.WorkloadSqlParm);
                tmpSql.AppendFormat("  from ISIA.RAW_DBA_HIST_SQLSTAT_{0} stat left join ISIA.RAW_DBA_HIST_SNAPSHOT_{0} snap on  stat.snap_id = snap.snap_id and stat.dbid = snap.dbid and stat.INSTANCE_NUMBER = snap.INSTANCE_NUMBER ", arguments.DBName);
                tmpSql.AppendFormat(" where  TO_CHAR (snap.begin_interval_time, 'yyyyMMddHH24miss') BETWEEN '{0}' and '{1}' ", arguments.StartTime, arguments.EndTime);
                tmpSql.AppendFormat(" and stat.dbid IN ('{0}') ", arguments.DBID);
                tmpSql.AppendFormat(" and stat.INSTANCE_NUMBER =  {0} ", arguments.INSTANCE_NUMBER);
                tmpSql.AppendFormat(" group by stat.sql_id  order by {0} desc) t where rownum<11" , arguments.WorkloadSqlParm);
                
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);
                DataSet resultDs = db.Select(tmpSql.ToString());
                this.ExecutingValue = resultDs;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void Getsqlid(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();


                tmpSql.AppendFormat("select snap.dbid ,stat.INSTANCE_NUMBER ,stat.sql_id , trunc(snap.end_interval_time ,'DD')  end_interval_time, sum(stat.{0}) {0}  ", arguments.WorkloadSqlParm);
                tmpSql.AppendFormat(@" from  RAW_DBA_HIST_SQLSTAT_{0}  stat left join 
                                        ISIA.RAW_DBA_HIST_SNAPSHOT_{0} snap
                                        ON  stat.snap_id = snap.snap_id
                                        AND stat.dbid = snap.dbid
                                        AND stat.INSTANCE_NUMBER = snap.INSTANCE_NUMBER ", arguments.DBName);
                tmpSql.AppendFormat(" where snap.end_interval_time > to_date('{0}', 'yyyy-MM-dd HH24:mi:ss')  and  snap.end_interval_time <= to_date('{1}', 'yyyy-MM-dd HH24:mi:ss') ", arguments.StartTime, arguments.EndTime);
                tmpSql.AppendFormat(" and stat.dbid = '{0}' ", arguments.DBID);
                tmpSql.AppendFormat(" and stat.INSTANCE_NUMBER =  {0} ", arguments.INSTANCE_NUMBER);
                tmpSql.AppendFormat(" and stat.sql_id =  '{0}' ", arguments.SQLID);
                tmpSql.Append(" group by  snap.dbid ,stat.INSTANCE_NUMBER ,stat.sql_id , trunc(snap.end_interval_time ,'DD')");
                tmpSql.Append(" order by  end_interval_time ");

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

        public void Getsqlidtext(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();


                tmpSql.AppendFormat("select * from RAW_DBA_HIST_SQLTEXT_{1} where sql_id = '{0}'", arguments.SQLID , arguments.DBName);
                

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
