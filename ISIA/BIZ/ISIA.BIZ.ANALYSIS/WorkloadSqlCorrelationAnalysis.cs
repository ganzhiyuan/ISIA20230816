﻿using ISIA.COMMON;
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


        public void GetWorkloadSqlCorrelationData(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(
                    "with sql\r\n" +
                   "as\r\n" +
                   "(select stat.snap_id, max(begin_interval_time) begin_interval_time,max(end_interval_time) end_interval_time, \r\n");
                foreach (string sqlParm in AwrArgsPack.SqlParmsList)
                {
                    tmpSql.AppendFormat("sum({0}) \"{0}\",", sqlParm);
                }
                tmpSql.Remove(tmpSql.Length - 1, 1);
                tmpSql.AppendFormat("from ISIA.RAW_DBA_HIST_SQLSTAT_{2} stat left join ISIA.RAW_DBA_HIST_SNAPSHOT_{2} snap on  \r\n" +
                   "stat.snap_id=snap.snap_id \r\n" +
                   "where  TO_CHAR (snap.end_INTERVAL_TIME, 'yyyyMMddHH24miss') BETWEEN '{0}' and '{1}'\r\n" +
                   "group by stat.snap_id\r\n" +
                   "order by snap_id),\r\n" +
                   "workload as\r\n" +
                   "(SELECT /*+ MATERIALIZE */\r\n" +
                   "                dbid,\r\n" +
                   "                INSTANCE_NUMBER,\r\n" +
                   "                   \r\n" +
                   "                snap_id,\r\n" +
                   "                \"begin_interval_time\",\r\n" +
                   "                \"end_interval_time\",\r\n" +
                   "                NVL(ROUND (\r\n" +
                   "                    (  \"{4}\"\r\n" +
                   "                     - LAG (\"{4}\", 1)\r\n" +
                   "                           OVER (PARTITION BY dbid, INSTANCE_NUMBER\r\n" +
                   "                                 ORDER BY snap_id))),0)\r\n" +
                   "                    \"{4}\"\r\n" +
                   "from\r\n" +
                   "(select\r\n" +
                   "/*+MATERIALIZE */\r\n" +
                   "MIN(begin_interval_time) \"begin_interval_time\",\r\n" +
                   "MAX(end_interval_time) \"end_interval_time\",\r\n" +
                   "dbid,\r\n" +
                   "snap_id,\r\n" +
                   "instance_number ,\r\n" +
                   "SUM(DECODE(STAT_NAME,'{4}',value ,0))\r\n" +
                   "\"{4}\"\r\n" +
                   "FROM\r\n" +
                   "(select /*+  LEADING(sn ss) USE_HASH(sn ss) USE_HASH(ss.sn ss.s ss.nm) no_merge(ss) */\r\n" +
                   "ss.dbid,\r\n" +
                   "ss.instance_number,\r\n" +
                   "ss.snap_id,\r\n" +
                   "ss.VALUE,\r\n" +
                   "ss.stat_name,sn.begin_interval_time, sn.end_interval_time from ISIA.RAW_DBA_HIST_SYSSTAT_{2} ss,ISIA.RAW_DBA_HIST_SNAPSHOT_{2} sn  \r\n" +
                   "where 1=1 and ss.dbid=sn.dbid and ss.INSTANCE_NUMBER=SN.INSTANCE_NUMBER and ss.snap_id=sn.snap_id and STAT_NAME='{4}' --configurable\r\n" +
                   "and sn.INSTANCE_NUMBER IN (1)\r\n" +
                   "and TO_CHAR(sn.end_INTERVAL_TIME, 'yyyyMMddHH24miss') between '{0}' and '{1}') t\r\n" +
                   " where 1=1\r\n" +
                   "group by dbid, instance_number ,snap_id) s)\r\n" +
                   "\r\n" +
                   "select sql.* ,workload.\"{4}\"\r\n" +
                   "from sql left join workload on sql.SNAP_ID=workload.snap_id\r\n", arguments.StartTime, arguments.EndTime,
                    arguments.DBName, AwrArgsPack.WorkloadSqlRelationMapping[arguments.WorkloadSqlParm], arguments.WorkloadSqlParm);

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
