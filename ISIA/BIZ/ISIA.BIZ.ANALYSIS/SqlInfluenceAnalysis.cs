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


                tmpSql.AppendFormat(@" select rownum,t.* from (select T.sql_id sql_id, min(begin_time) begin_interval_time,max(end_time) end_interval_time, NVL(sum({0}),0) {0} ", arguments.WorkloadSqlParm);
                tmpSql.AppendFormat("  from ISIA.RAW_DBA_HIST_SQLSTAT_{0} T ", arguments.DBName);
                tmpSql.AppendFormat(" where  TO_CHAR (T.begin_time, 'yyyyMMddHH24miss') BETWEEN '{0}' and '{1}' ", arguments.StartTime, arguments.EndTime);
                tmpSql.AppendFormat(" and T.dbid IN ('{0}') ", arguments.DBID);
                tmpSql.AppendFormat(" and T.INSTANCE_NUMBER =  {0} ", arguments.INSTANCE_NUMBER);
                tmpSql.AppendFormat(" group by T.sql_id  order by {0} desc) t where rownum<11" , arguments.WorkloadSqlParm);
                
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


                tmpSql.AppendFormat("select t.dbid ,t.INSTANCE_NUMBER ,t.sql_id , trunc(t.end_time ,'DD')  end_interval_time, sum(t.{0}) {0}  ", arguments.WorkloadSqlParm);
                tmpSql.AppendFormat(@" from  RAW_DBA_HIST_SQLSTAT_{0}  t  ", arguments.DBName);
                tmpSql.AppendFormat(" where t.end_time > to_date('{0}', 'yyyy-MM-dd HH24:mi:ss')  and  t.end_time <= to_date('{1}', 'yyyy-MM-dd HH24:mi:ss') ", arguments.StartTime, arguments.EndTime);
                tmpSql.AppendFormat(" and t.dbid = '{0}' ", arguments.DBID);
                tmpSql.AppendFormat(" and t.INSTANCE_NUMBER =  {0} ", arguments.INSTANCE_NUMBER);
                tmpSql.AppendFormat(" and t.sql_id =  '{0}' ", arguments.SQLID);
                tmpSql.Append(" group by  t.dbid ,t.INSTANCE_NUMBER ,t.sql_id , trunc(t.end_time ,'DD')");
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
