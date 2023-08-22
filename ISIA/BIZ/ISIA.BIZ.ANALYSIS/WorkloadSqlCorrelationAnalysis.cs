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
    class WorkloadSqlCorrelationAnalysis : TAP.Remoting.Server.Biz.BizComponentBase
    {


        public void GetDBName(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("select DBID, DBNAME   from TAPCTDATABASE ");


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


        public void GetIdName()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append(" SELECT TO_CHAR(PARAMETERID) PARAMETERID ,PARAMETERNAME FROM TAPCTPARAMETERDEF  ORDER BY PARAMETERNAME  ");

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

        public void GetWorkloadSqlCorrelationData(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                if (AwrArgsPack.WorkloadBelonging[arguments.WorkloadSqlParm] == AwrArgsPack.SYSSTAT)
                {
                    tmpSql.AppendFormat(
                        "with sql\r\n" +
                       "as\r\n" +
                       "(select stat.snap_id, stat.dbid, stat.instance_number, max(begin_time) begin_interval_time,max(end_time) end_interval_time, \r\n");
                    foreach (string sqlParm in AwrArgsPack.SqlParmsList)
                    {
                        tmpSql.AppendFormat("sum({0}) \"{0}\",", sqlParm);
                    }
                    tmpSql.Remove(tmpSql.Length - 1, 1);
                    tmpSql.AppendFormat("from RAW_DBA_HIST_SQLSTAT_{2} stat \r\n" +
                       "where  TO_CHAR (stat.BEGIN_TIME, 'yyyyMMddHH24miss') BETWEEN '{0}' and '{1}'\r\n" +
                       " AND stat.DBID IN '{4}'  AND stat.INSTANCE_NUMBER IN  '{5}' " +
                       "group by stat.snap_id,stat.dbid,stat.instance_number\r\n" +
                       "order by snap_id),\r\n" +
                       "workload as\r\n" +
                       "(SELECT \r\n" +
                       "                dbid,\r\n" +
                       "                INSTANCE_NUMBER,\r\n" +
                       "                   \r\n" +
                       "                snap_id,\r\n" +
                       "                \"begin_time\",\r\n" +
                       "                \"endl_time\",\r\n" +
                       "                NVL(ROUND (\r\n" +
                       "                    (  \"{3}\"\r\n" +
                       "                     - LAG (\"{3}\", 1)\r\n" +
                       "                           OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
                       "                                 ORDER BY snap_id))),0)\r\n" +
                       "                    \"{3}\"\r\n" +
                       "from\r\n" +
                       "(select\r\n" +
                       
                       "MIN(begin_time) \"begin_interval_time\",\r\n" +
                       "MAX(end_time) \"end_interval_time\",\r\n" +
                       "dbid,\r\n" +
                       "snap_id,\r\n" +
                       "instance_number ,\r\n" +
                       "SUM(DECODE(STAT_NAME,'{3}',value ,0))\r\n" +
                       "\"{3}\"\r\n" +
                       "FROM\r\n" +
                       "(select \r\n" +
                       "ss.dbid,\r\n" +
                       "ss.instance_number,\r\n" +
                       "ss.snap_id,\r\n" +
                       "ss.VALUE,\r\n" +
                       "ss.stat_name,ss.begin_time, ss.end_time from RAW_DBA_HIST_SYSSTAT_{2} ss \r\n" +
                       "where 1=1  and STAT_NAME='{3}'\r\n" +

                       " and TO_CHAR(ss.BEGIN_TIME, 'yyyyMMddHH24miss') between '{0}' and '{1}'  " +
                       "  AND ss.DBID IN  ('{4}')  AND ss.INSTANCE_NUMBER  = {5}  " +
                       " ) t\r\n" +
                       " where 1=1\r\n" +
                       "group by dbid, instance_number ,snap_id) s)\r\n" +
                       "\r\n" +
                       "select sql.* ,workload.\"{3}\"\r\n" +
                       "from sql left join workload on sql.SNAP_ID=workload.snap_id  AND sql.dbid = workload.dbid AND sql.INSTANCE_NUMBER = workload.INSTANCE_NUMBER \r\n", arguments.StartTime, arguments.EndTime,
                        arguments.DBName, arguments.WorkloadSqlParm,arguments.DBID,arguments.INSTANCE_NUMBER);
                }else if(AwrArgsPack.WorkloadBelonging[arguments.WorkloadSqlParm] == AwrArgsPack.METRIC)
                {
                    tmpSql.AppendFormat(
                        "with sql\r\n" +
                       "as\r\n" +
                       "(select stat.snap_id, stat.dbid, stat.instance_number, max(begin_time) begin_interval_time,max(end_time) end_interval_time, \r\n");
                    foreach (string sqlParm in AwrArgsPack.SqlParmsList)
                    {
                        tmpSql.AppendFormat("sum({0}) \"{0}\",", sqlParm);
                    }
                    tmpSql.Remove(tmpSql.Length - 1, 1);
                    tmpSql.AppendFormat("from RAW_DBA_HIST_SQLSTAT_{2} stat    \r\n" +
                       "where  TO_CHAR (stat.BEGIN_TIME, 'yyyyMMddHH24miss') BETWEEN '{0}' and '{1}'\r\n" +
                       " AND stat.DBID IN '{4}'  AND stat.INSTANCE_NUMBER IN  '{5}' " +
                       "group by stat.snap_id  ,stat.dbid, stat.instance_number\r\n" +
                       "order by snap_id),\r\n" +
                       "workload as\r\n" +
                       "(select\r\n" +
                       "MIN(begin_time) \"begin_interval_time\",\r\n" +
                       "MAX(end_time) \"end_interval_time\",\r\n" +
                       "dbid,\r\n" +
                       "snap_id,\r\n" +
                       "instance_number ,\r\n" +
                       "SUM(DECODE(METRIC_NAME,'{3}',average ,0))\r\n" +
                       "\"{3}\"\r\n" +
                       "FROM\r\n" +
                       "(select \r\n" +
                       "ss.dbid,\r\n" +
                       "ss.instance_number,\r\n" +
                       "ss.snap_id,\r\n" +
                       "ss.average,\r\n" +
                       "ss.METRIC_NAME,ss.begin_time, ss.end_time from RAW_DBA_HIST_SYSMETRIC_SUMMARY_{2} ss  \r\n" +
                       "where 1=1  and METRIC_NAME='{3}' \r\n" +

                       "and TO_CHAR(ss.BEGIN_TIME, 'yyyyMMddHH24miss') between '{0}' and '{1}'" +
                       "  AND ss.DBID IN '{4}'  AND ss.INSTANCE_NUMBER IN '{5}'  "  +
                       ") t\r\n" +
                       " where 1=1\r\n" +
                       "group by dbid, instance_number ,snap_id)\r\n" +
                       "\r\n" +
                       "select sql.* ,workload.\"{3}\"\r\n" +
                       "from sql left join workload on sql.SNAP_ID=workload.snap_id  AND sql.dbid = workload.dbid AND sql.INSTANCE_NUMBER = workload.INSTANCE_NUMBER \r\n", arguments.StartTime, arguments.EndTime,
                        arguments.DBName, arguments.WorkloadSqlParm, arguments.DBID, arguments.INSTANCE_NUMBER);
                }
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


        private void AppendWithCRLF(StringBuilder main, string append)
        {
            main.Append(append).Append("\r\n");
        }
        private List<string> GetFilterParamNameByType(DBCommunicator db, string type, List<object> paramNames)
        {
            List<string> resultList = new List<string>();
            StringBuilder tmpSql = new StringBuilder();
            tmpSql.AppendFormat("select parametername from TAPCTPARAMETERDEF where parametertype = {0} and parametername in ({1})", Utils.MakeSqlQueryIn2(type), Utils.MakeSqlQueryIn2(paramNames));
            DataSet ds = (DataSet)db.Select(tmpSql.ToString());
            RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                resultList.Add(dr["parametername"].ToString());
            }
            return resultList;
        }


        private void AppendSumWithDeode(StringBuilder main, string colunm, string decodeMode, string paramType)
        {
            main.AppendFormat("SUM(DECODE({2},'{0}',{1} ,0)) \"{3}\"", HandleSpecialCharacter(colunm), decodeMode, paramType, colunm);
        }

        private void AppendMin(StringBuilder main, string colunm, string alias)
        {
            main.AppendFormat("MIN({0}) \"{1}\"", colunm, alias).Append(",");
        }

        private void AppendMax(StringBuilder main, string colunm, string alias)
        {
            main.AppendFormat("MAX({0}) \"{1}\"", colunm, alias).Append(",");
        }

        private void AppendWithComma(StringBuilder main, string append)
        {
            main.Append(append).Append(",");
        }

        private void AppendRoundWithAVG(StringBuilder main, string colunm)
        {
            main.AppendFormat("ROUND(AVG(\"{0}\")) \"{0}\"", colunm);
        }
        private string HandleSpecialCharacter(string str)
        {
            StringBuilder sb = new StringBuilder("");
            sb.Append(str);
            if (str.ToString().Contains("'"))
            {
                string specialStr = str.ToString();
                sb.Insert(specialStr.IndexOf("'"), "'");
            }
            return sb.ToString();
        }

        private int GetCutCount(int total, int everyMaxCount)
        {
            int cutCount = total / everyMaxCount;
            if (cutCount == 0)
            {
                return 1;
            }
            if (total % everyMaxCount != 0)
            {
                cutCount++;
            }
            return cutCount;
        }

        private List<string> CutListByCutCount(List<string> origin, int cutCount, int cutIndex)
        {
            List<string> temp = new List<string>();
            if (origin.Count == 0)
            {
                return temp;
            }
            int listTotolCount = origin.Count;
            int everyInterval = listTotolCount / cutCount + 1;
            if (((cutIndex - 1) * everyInterval) <= listTotolCount - 1 && (cutIndex * everyInterval - 1) <= listTotolCount - 1)
            {
                temp = origin.GetRange((cutIndex - 1) * everyInterval, everyInterval);

            }
            else if (((cutIndex - 1) * everyInterval) <= listTotolCount - 1 && (cutIndex * everyInterval - 1) > listTotolCount - 1)
            {
                temp = origin.GetRange((cutIndex - 1) * everyInterval, listTotolCount % everyInterval);
            }
            return temp;
        }


    }
}
