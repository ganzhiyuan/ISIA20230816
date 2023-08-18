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

namespace ISIA.BIZ.ANALYSIS
{
    class WorkloadSqlRelationAnalysis : TAP.Remoting.Server.Biz.BizComponentBase
    {


        public void GetIdName()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append(" SELECT TO_CHAR(PARAMETERID) PARAMETERID ,PARAMETERNAME FROM TAPCTPARAMETERDEF WHERE PARAMETERTYPE = 'METRIC'  ORDER BY PARAMETERNAME  ");

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


        public void GetWorkpara(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();


                tmpSql.AppendFormat(@" 
                select * from(
                select T.snap_id , T.DBID, T.INSTANCE_NUMBER, sum(T.{0}) {0}, T.end_time ", arguments.WorkloadSqlParm);


                tmpSql.AppendFormat(@" 
                from RAW_DBA_HIST_SQLSTAT_{0} T 
                where 1 = 1 ", arguments.DBName);

                tmpSql.AppendFormat(@" 
                 and T.begin_time > to_date('{0}', 'yyyy-MM-dd HH24:mi:ss')
                 and T.begin_time <= to_date('{1}', 'yyyy-MM-dd HH24:mi:ss') ", arguments.StartTime , arguments.EndTime);

                tmpSql.AppendFormat(@" 
                 AND T.DBID = '{0}' ", arguments.DBID);

                

                tmpSql.AppendFormat(@"  AND T.INSTANCE_NUMBER  = {0} ", arguments.INSTANCE_NUMBER);

                tmpSql.Append(@" 
                  group by T.snap_id , T.DBID , T.INSTANCE_NUMBER , T.end_time
                  order by T.end_time
                  )
                  UNPIVOT(
                  VALUE FOR PARAMETER IN(");

                tmpSql.AppendFormat(@" 
                 {0} AS '{0}'", arguments.WorkloadSqlParm);

                tmpSql.Append(@" 
                 )
                 )");

               /* tmpSql.AppendFormat(@"
                select * from(
                select T.snap_id , T.DBID, T.INSTANCE_NUMBER, sum(T.FETCHES_TOTAL) FETCHES_TOTAL, A.end_interval_time 
                from RAW_DBA_HIST_SQLSTAT_ISFA T 
                left join RAW_DBA_HIST_SNAPSHOT_ISFA A 
                on T.snap_id  = A.Snap_id where 1 = 1 
                and A.end_interval_time > to_date('2023-04-16 17:02:00', 'yyyy-MM-dd HH24:mi:ss')
                        and A.end_interval_time <= to_date('2023-04-17 17:02:40', 'yyyy-MM-dd HH24:mi:ss')
                        AND A.DBID IN '1643782569'
                        AND A.INSTANCE_NUMBER IN '1'
                        group by T.snap_id , T.DBID , T.INSTANCE_NUMBER , A.end_interval_time
                        order by A.end_interval_time
                    )
                    UNPIVOT(
                      VALUE FOR PARAMETER IN(
                        FETCHES_TOTAL AS 'FETCHES_TOTAL'
                      )
                    )");*/


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


        public void GetParmDEF(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"  SELECT A.SNAP_ID,A.DBID, A.METRIC_NAME PARAMETER, A.AVERAGE VALUE,
                    A.end_time FROM RAW_DBA_HIST_SYSMETRIC_SUMMARY_{0} A ", arguments.DBName);

                tmpSql.AppendFormat(@" WHERE     1 = 1
                    AND A.begin_time >
                    TO_DATE ('{0}', 'yyyy-MM-dd HH24:mi:ss')
                    AND A.begin_time <=
                    TO_DATE ('{1}', 'yyyy-MM-dd HH24:mi:ss') ", arguments.StartTime, arguments.EndTime);

                tmpSql.AppendFormat(@" AND A.DBID IN ('{0}') ", arguments.DBID);

                tmpSql.AppendFormat(@" AND A.INSTANCE_NUMBER = {0} ", arguments.INSTANCE_NUMBER);

                if (string.IsNullOrEmpty(arguments.PARADEF))
                {
                    tmpSql.Append(@" order by A.end_time");
                }
                else
                {
                    tmpSql.AppendFormat(@" AND metric_name IN ( {0} ) order by a.metric_name ,A.end_time", Utils.MakeSqlQueryIn2(arguments.PARADEF));
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



        public void GetParmNameByTypeAll()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("select t.* from TAPCTPARAMETERDEF t where t.parametertype = 'METRIC'");


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


        public void GetSqlId(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("select distinct(SQL_ID) " +
                    "from ISIA.RAW_DBA_HIST_SQLSTAT_{2} stat left join ISIA.RAW_DBA_HIST_SNAPSHOT_{2} snap on stat.snap_id = snap.snap_id " +
                    "where TO_CHAR(snap.end_INTERVAL_TIME, 'yyyyMMddHH24miss') BETWEEN '{0}' and '{1}'", arguments.StartTime, arguments.EndTime
                    ,arguments.DBName);


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

        public void GetWorkloadSqlRelationData(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder getSqlIdDataSql = new StringBuilder();
                StringBuilder getWorkloadDtaSql = new StringBuilder();

                DataSet resultSet = new DataSet();

                //get Sql parm data 
                getSqlIdDataSql.AppendFormat(
                    "select stat.snap_id, max(begin_interval_time) begin_interval_time,max(end_interval_time) end_interval_time, " +
                    "sum({0}) \"{0}\" " +
                    "from " +
                    "ISIA.RAW_DBA_HIST_SQLSTAT_{3} stat left join " +
                    "ISIA.RAW_DBA_HIST_SNAPSHOT_{3} snap on stat.snap_id = snap.snap_id " +
                    "where TO_CHAR(snap.end_INTERVAL_TIME, 'yyyyMMddHH24miss') BETWEEN '{1}' and '{2}' "
                   , AwrArgsPack.WorkloadSqlRelationMapping[arguments.WorkloadSqlParm], arguments.StartTime, arguments.EndTime, arguments.DBName);
                //if (arguments.SqlIdList.Count != 0)
                //{
                //    getSqlIdDataSql.AppendFormat("and sql_id in({0}) ", Utils.MakeSqlQueryIn2(arguments.SqlIdList));
                //}
                getSqlIdDataSql.Append(" group by stat.snap_id ORDER BY SNAP_ID");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       getSqlIdDataSql.ToString(), false);
                DataTable dtSql = db.Select(getSqlIdDataSql.ToString()).Tables[0];
                dtSql.TableName = AwrArgsPack.WorkloadSqlRelationMapping[arguments.WorkloadSqlParm];
                resultSet.Tables.Add(dtSql.Copy());

                //get workload parm data 
                string parmColumnName = "STAT_NAME";
                string parmTableSuffix = "ISIA.RAW_DBA_HIST_SYSSTAT";
                string valueName = "value";
                if (AwrArgsPack.WorkloadBelonging[arguments.WorkloadSqlParm].Equals(AwrArgsPack.METRIC))
                {
                    getWorkloadDtaSql.AppendFormat(
                   "select\r\n" +
                   "/*+MATERIALIZE */\r\n" +
                   "MIN(begin_interval_time) \"begin_interval_time\", \r\n" +
                   "MAX(end_interval_time) \"end_interval_time\", \r\n" +
                   "dbid,\r\n" +
                   "(select dbname from TAPCTDATABASE  where dbid=s.dbid) \r\n" +
                   "dbname,\r\n" +
                   "snap_id,\r\n" +
                   "s.instance_number as inst_id,\r\n" +
                   "SUM(DECODE(metric_name,'{0}',average ,0)) \r\n" +
                   "\"{0}\"\r\n" +
                   "FROM\r\n" +
                   "(select /*+  LEADING(sn ss) USE_HASH(sn ss) USE_HASH(ss.sn ss.s ss.nm) no_merge(ss) */ \r\n" +
                   "ss.*,sn.begin_interval_time, sn.end_interval_time from ISIA.RAW_DBA_HIST_SYSMETRIC_SUMMARY_{3} ss,ISIA.RAW_DBA_HIST_SNAPSHOT_{3} sn  \r\n" +
                   "where 1=1 and ss.dbid=sn.dbid and ss.INSTANCE_NUMBER=SN.INSTANCE_NUMBER and ss.snap_id=sn.snap_id and metric_name='{0}' --configurable\r\n" +
                   "and sn.INSTANCE_NUMBER IN (1) \r\n" +
                   "and TO_CHAR(sn.BEGIN_INTERVAL_TIME, 'yyyyMMddHH24miss') between '{1}' and '{2}') s \r\n" +
                   " where 1=1\r\n" +
                   "group by dbid,s.instance_number ,snap_id\r\n", arguments.WorkloadSqlParm, arguments.StartTime, arguments.EndTime,
                   arguments.DBName);
                }
                else
                {
                    getWorkloadDtaSql.AppendFormat("SELECT dbid,INSTANCE_NUMBER, snap_id, begin_interval_time,end_interval_time,ROUND ((\"{0}\"", arguments.WorkloadSqlParm);
                    getWorkloadDtaSql.AppendFormat(" - LAG (\"{0}\", 1) OVER (PARTITION BY dbid, INSTANCE_NUMBER  ORDER BY snap_id)))    \"{0}\" from (select MIN(begin_interval_time) begin_interval_time,", arguments.WorkloadSqlParm);
                    getWorkloadDtaSql.AppendFormat(" MAX(end_interval_time) end_interval_time,dbid,snap_id,instance_number ,SUM(DECODE(STAT_NAME,'{0}',value ,0))\"{0}\"", arguments.WorkloadSqlParm);
                    getWorkloadDtaSql.Append(" FROM(select ss.dbid,ss.instance_number,ss.snap_id,ss.VALUE,");
                    getWorkloadDtaSql.AppendFormat(" ss.stat_name,sn.begin_interval_time, sn.end_interval_time from ISIA.RAW_DBA_HIST_SYSSTAT_{0} ss,ISIA.RAW_DBA_HIST_SNAPSHOT_{0} sn  ", arguments.DBName);
                    getWorkloadDtaSql.AppendFormat(" where 1=1 and ss.dbid=sn.dbid and ss.INSTANCE_NUMBER=SN.INSTANCE_NUMBER and ss.snap_id=sn.snap_id and STAT_NAME='{0}' ", arguments.WorkloadSqlParm);
                    getWorkloadDtaSql.Append(" and sn.INSTANCE_NUMBER IN (1)");
                    getWorkloadDtaSql.AppendFormat(" and TO_CHAR(sn.BEGIN_INTERVAL_TIME, 'yyyyMMddHH24miss') between '{0}' ", arguments.StartTime);
                    getWorkloadDtaSql.AppendFormat(" and '{0}') t", arguments.EndTime);
                    getWorkloadDtaSql.Append(" where 1=1");
                    getWorkloadDtaSql.Append(" group by dbid, instance_number ,snap_id) s");

                }


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                      getWorkloadDtaSql.ToString(), false);
                DataTable dtWorkload = db.Select(getWorkloadDtaSql.ToString()).Tables[0];
                dtWorkload.TableName = arguments.WorkloadSqlParm;
                resultSet.Tables.Add(dtWorkload.Copy());

                this.ExecutingValue = resultSet;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetParmDailyTrendData(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<object> paramNames = arguments.ParamNamesList;
                int interval = 500;
                List<List<object>> paramNamesList = new List<List<object>>();
                for (int i = 0; i < paramNames.Count / interval + 1; i++)
                {
                    List<object> temp = new List<object>();
                    if (i == paramNames.Count / interval)
                    {
                        temp = paramNames.GetRange(i * interval, paramNames.Count % interval);
                        paramNamesList.Add(temp);
                        break;
                    }
                    temp = paramNames.GetRange(i * interval, interval);
                    paramNamesList.Add(temp);
                }


                List<string> metricParamNames = new List<string>();
                List<string> statisticParamNames = new List<string>();
                for (int i = 0; i < paramNamesList.Count; i++)
                {
                    metricParamNames.AddRange(GetFilterParamNameByType(db, "METRIC", paramNamesList[i]));
                    statisticParamNames.AddRange(GetFilterParamNameByType(db, "STATISTIC", paramNamesList[i]));
                }

                //Batch data
                int sqlDailyTrendDataInterval = 990;
                int cutCount = GetCutCount(paramNames.Count, sqlDailyTrendDataInterval);
                List<DataTable> tableSplitTotal = new List<DataTable>();

                for (int i = 1; i <= cutCount; i++)
                {

                    List<string> tempMetricParamNames = CutListByCutCount(metricParamNames, cutCount, i);
                    List<string> tempStatisticParamNames = CutListByCutCount(statisticParamNames, cutCount, i);
                    List<string> total = new List<string>();
                    total.AddRange(tempMetricParamNames);
                    total.AddRange(tempStatisticParamNames);
                    string[] paramStrings = total.ToArray();
                    DataTable[] tableSplit = new DataTable[paramStrings.Length];
                    if (tempMetricParamNames.Count == 0 && tempStatisticParamNames.Count == 0) break;
                    DataTable dt = GetTrendDataByParams(db, tempMetricParamNames, tempStatisticParamNames, arguments);


                    for (int j = 0; j < paramStrings.Length; j++)
                    {
                        tableSplit[j] = new DataTable();
                        DataTable tempDt = tableSplit[j];
                        string parmName = (string)paramStrings[j];
                        tempDt.Columns.Add(parmName, dt.Columns[parmName].DataType);
                        tempDt.Columns.Add("BEGIN_TIME", dt.Columns["BEGIN_TIME"].DataType);

                        tempDt.TableName = parmName;
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int j = 0; j < paramStrings.Length; j++)
                        {
                            string parmName = (string)paramStrings[j];
                            DataTable tempDt = tableSplit[j];
                            tempDt.Rows.Add(dr[parmName], dr["BEGIN_TIME"]);
                        }
                    }
                    tableSplitTotal.AddRange(tableSplit.ToList());

                }
                DataSet resultSet = new DataSet();
                foreach (DataTable dt in tableSplitTotal)
                {
                    resultSet.Tables.Add(dt);
                }
                this.ExecutingValue = resultSet;
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

        private DataTable GetTrendDataByParams(DBCommunicator db, List<string> metricParamNames, List<string> statisticParamNames, AwrArgsPack arguments)
        {
            StringBuilder tmpSql = new StringBuilder();
            //t1_sysmetric_summary
            AppendWithCRLF(tmpSql, "with");
            AppendWithCRLF(tmpSql, "t1_sysmetric_summary");
            AppendWithCRLF(tmpSql, "as");
            AppendWithCRLF(tmpSql, "(");
            AppendWithCRLF(tmpSql, "select"); AppendWithCRLF(tmpSql, "/*+MATERIALIZE */");
            AppendMin(tmpSql, "begin_interval_time", "BEGIN_TIME");
            AppendMax(tmpSql, "end_interval_time", "END_TIME");
            AppendWithComma(tmpSql, "dbid");
            AppendWithComma(tmpSql, "(select instance_name from gv$instance where instance_number=s.instance_number) dbname");
            AppendWithComma(tmpSql, "snap_id");
            AppendWithComma(tmpSql, "s.instance_number as inst_id");
            AppendMin(tmpSql, "NUM_INTERVAL", "NUM_INTERVAL");
            foreach (string param in metricParamNames)
            {
                AppendSumWithDeode(tmpSql, param, "average", "METRIC_NAME");
                AppendWithComma(tmpSql, "");
            }
            tmpSql.Remove(tmpSql.Length - 1, 1);
            AppendWithCRLF(tmpSql, "");
            AppendWithCRLF(tmpSql, "FROM");
            tmpSql.AppendFormat("(SELECT /*+  LEADING(sn sm) USE_HASH(sn sm) USE_HASH(sm.sn sm.m sn.mn) no_merge(sm) */ " +
                " sm.*,sn.begin_interval_time, sn.end_interval_time FROM ISIA.RAW_DBA_HIST_SYSMETRIC_SUMMARY_{3} sm,ISIA.RAW_DBA_HIST_SNAPSHOT_{3} sn " +
                 "WHERE  1=1 AND SM.dbid=sn.dbid AND sm.INSTANCE_NUMBER = sn.INSTANCE_NUMBER AND sm.snap_id = sn.snap_id AND sn.INSTANCE_NUMBER IN ({0})      " +
                 "AND TO_CHAR (sn.BEGIN_INTERVAL_TIME, 'yyyyMMddHH24miss') BETWEEN '{1}' AND '{2}') s ", 1, arguments.StartTime, arguments.EndTime, arguments.DBName);
            AppendWithCRLF(tmpSql, "where 1=1");
            AppendWithCRLF(tmpSql, "group by dbid,s.instance_number, snap_id");
            AppendWithCRLF(tmpSql, ")");
            AppendWithComma(tmpSql, "");


            //t2_sysmetric_summary
            AppendWithCRLF(tmpSql, "t2_sysmetric_summary");
            AppendWithCRLF(tmpSql, "as");
            AppendWithCRLF(tmpSql, "(");
            AppendWithCRLF(tmpSql, "select");
            AppendWithComma(tmpSql, "dbid");
            AppendWithComma(tmpSql, "inst_id");
            AppendMin(tmpSql, "snap_id", "SNAP_ID_MIN");
            tmpSql.AppendFormat("TO_CHAR(BEGIN_TIME,'{0}') workdate,", arguments.GroupingDateFormat);
            AppendMin(tmpSql, "BEGIN_TIME", "BEGIN_TIME");
            AppendMax(tmpSql, "END_TIME", "END_TIME");
            foreach (string param in metricParamNames)
            {
                AppendRoundWithAVG(tmpSql, param);
                AppendWithComma(tmpSql, "");
            }
            tmpSql.Remove(tmpSql.Length - 1, 1);
            tmpSql.AppendFormat("FROM t1_sysmetric_summary s where 1=1 group by dbid, inst_id, TO_CHAR(BEGIN_TIME,'{0}')", arguments.GroupingDateFormat);
            AppendWithCRLF(tmpSql, "");
            AppendWithCRLF(tmpSql, ")");
            AppendWithCRLF(tmpSql, "");
            //t1_sysstat
            AppendWithComma(tmpSql, "");
            AppendWithCRLF(tmpSql, "t1_sysstat");
            AppendWithCRLF(tmpSql, "as");
            AppendWithCRLF(tmpSql, "(");
            AppendWithCRLF(tmpSql, "select"); AppendWithCRLF(tmpSql, "/*+MATERIALIZE */");
            AppendMin(tmpSql, "begin_interval_time", "BEGIN_TIME");
            AppendMax(tmpSql, "end_interval_time", "END_TIME");
            AppendWithComma(tmpSql, "dbid");
            AppendWithComma(tmpSql, "(select instance_name from gv$instance where instance_number=s.instance_number) dbname");
            AppendWithComma(tmpSql, "snap_id");
            AppendWithComma(tmpSql, "s.instance_number as inst_id");
            foreach (string param in statisticParamNames)
            {
                AppendSumWithDeode(tmpSql, param, "value", "STAT_NAME");
                AppendWithComma(tmpSql, "");
            }
            tmpSql.Remove(tmpSql.Length - 1, 1);
            AppendWithCRLF(tmpSql, "");
            AppendWithCRLF(tmpSql, "FROM");
            tmpSql.AppendFormat("(select /*+  LEADING(sn ss) USE_HASH(sn ss) USE_HASH(ss.sn ss.s ss.nm) no_merge(ss) */ " +
                "ss.*,sn.begin_interval_time, sn.end_interval_time from ISIA.RAW_DBA_HIST_SYSSTAT_{3} ss,ISIA.RAW_DBA_HIST_SNAPSHOT_{3} sn " +
                " where 1=1 and ss.dbid=sn.dbid and ss.INSTANCE_NUMBER=SN.INSTANCE_NUMBER and ss.snap_id=sn.snap_id and sn.INSTANCE_NUMBER IN ({0}) " +
                " and TO_CHAR(sn.BEGIN_INTERVAL_TIME, 'yyyyMMddHH24miss') between '{1}' and '{2}') s ", 1, arguments.StartTime, arguments.EndTime, arguments.DBName);
            AppendWithCRLF(tmpSql, "where 1=1");
            AppendWithCRLF(tmpSql, "group by dbid,s.instance_number, snap_id");
            AppendWithCRLF(tmpSql, ")");
            AppendWithComma(tmpSql, "");
            //t2_sysstat
            AppendWithCRLF(tmpSql, "t2_sysstat");
            AppendWithCRLF(tmpSql, "as");
            AppendWithCRLF(tmpSql, "(");
            AppendWithCRLF(tmpSql, "select");
            AppendWithComma(tmpSql, "dbid");
            AppendWithComma(tmpSql, "inst_id");
            AppendMin(tmpSql, "snap_id", "SNAP_ID_MIN");
            tmpSql.AppendFormat("TO_CHAR(BEGIN_TIME,'{0}') workdate,", arguments.GroupingDateFormat);
            AppendMin(tmpSql, "BEGIN_TIME", "BEGIN_TIME");
            AppendMax(tmpSql, "END_TIME", "END_TIME");
            foreach (string param in statisticParamNames)
            {
                AppendRoundWithAVG(tmpSql, param);
                AppendWithComma(tmpSql, "");
            }
            tmpSql.Remove(tmpSql.Length - 1, 1);
            tmpSql.AppendFormat("FROM t1_sysstat s where 1=1 group by dbid, inst_id, TO_CHAR(BEGIN_TIME,'{0}')", arguments.GroupingDateFormat);
            AppendWithCRLF(tmpSql, ")");
            AppendWithCRLF(tmpSql, "");
            //stat end
            AppendWithCRLF(tmpSql, "select");
            AppendWithComma(tmpSql, "sm.dbid");
            AppendWithComma(tmpSql, "sm.inst_id");
            AppendWithComma(tmpSql, "sm.snap_id_min");
            AppendWithComma(tmpSql, "sm.workdate as workdate");
            AppendWithComma(tmpSql, "sm.begin_time");
            AppendWithComma(tmpSql, "sm.end_time");
            foreach (string param in metricParamNames)
            {
                tmpSql.AppendFormat("sm.\"{0}\"", param);
                AppendWithComma(tmpSql, "");

            }
            foreach (string param in statisticParamNames)
            {
                tmpSql.AppendFormat("ss.\"{0}\"", param);
                AppendWithComma(tmpSql, "");
            }
            tmpSql.Remove(tmpSql.Length - 1, 1);
            AppendWithCRLF(tmpSql, "from t2_sysmetric_summary sm,t2_sysstat ss " +
                "where sm.dbid=ss.dbid(+) and sm.inst_id= ss.inst_id(+) and sm.workdate=ss.workdate(+) " +
                " order by dbid, workdate");


            RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                   tmpSql.ToString(), false);

            return db.Select(tmpSql.ToString()).Tables[0];
        }


        

    }
}
