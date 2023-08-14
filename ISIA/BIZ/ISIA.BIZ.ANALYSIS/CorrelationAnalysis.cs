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
    class CorrelationAnalysis : TAP.Remoting.Server.Biz.BizComponentBase
    {


        public void GetDBName(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("select name from v$database ");


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

        public void GetParmType(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("select distinct(parametertype) from TAPIAPARAMETERLIST");


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
        public void GetParmNameByType(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("select parametername from TAPIAPARAMETERLIST where parametertype in ({0})", Utils.MakeSqlQueryIn2(arguments.ParamType));


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
                foreach(DataTable dt in tableSplitTotal)
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
            tmpSql.AppendFormat("select distinct(parametername) from TAPCTPARAMETERDEF where parametertype = {0} and parametername in ({1})", Utils.MakeSqlQueryIn2(type), Utils.MakeSqlQueryIn2(paramNames));
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
            main.AppendFormat("SUM(DECODE({2},'{0}',{1} ,0)) \"{3}\"", HandleSpecialCharacter(colunm), decodeMode, paramType,colunm);
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
            tmpSql.AppendFormat("(SELECT /*+  LEADING(sn sm) USE_HASH(sn sm) USE_HASH(sm.sn sm.m sn.mn) no_merge(sm) */ ");
            tmpSql.AppendFormat(" sm.*,sn.begin_interval_time, sn.end_interval_time FROM ISIA.RAW_DBA_HIST_SYSMETRIC_SUMMARY_{0} sm,ISIA.RAW_DBA_HIST_SNAPSHOT_{0} sn ",arguments.DBName);
            tmpSql.AppendFormat("WHERE  1=1 AND SM.dbid=sn.dbid AND sm.INSTANCE_NUMBER = sn.INSTANCE_NUMBER AND sm.snap_id = sn.snap_id AND sn.INSTANCE_NUMBER IN (1)     ");
            tmpSql.AppendFormat("AND TO_CHAR (sn.BEGIN_INTERVAL_TIME, '{0}')", arguments.GroupingDateFormat); 
            tmpSql.AppendFormat(" BETWEEN '{0}'", arguments.StartTime);
            tmpSql.AppendFormat(" AND '{0}') s ", arguments.EndTime);
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
            tmpSql.AppendFormat("TO_CHAR(BEGIN_TIME,'{0}') workdate,",arguments.GroupingDateFormat);
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
            tmpSql.AppendFormat("(select /*+  LEADING(sn ss) USE_HASH(sn ss) USE_HASH(ss.sn ss.s ss.nm) no_merge(ss) */ ");
            tmpSql.AppendFormat("ss.*,sn.begin_interval_time, sn.end_interval_time from ISIA.RAW_DBA_HIST_SYSSTAT_{0} ss,ISIA.RAW_DBA_HIST_SNAPSHOT_{0} sn ",arguments.DBName);
            tmpSql.AppendFormat(" where 1=1 and ss.dbid=sn.dbid and ss.INSTANCE_NUMBER=SN.INSTANCE_NUMBER and ss.snap_id=sn.snap_id and sn.INSTANCE_NUMBER IN (1) "); 
            tmpSql.AppendFormat(" and TO_CHAR(sn.BEGIN_INTERVAL_TIME, '{0}') ", arguments.GroupingDateFormat);
            tmpSql.AppendFormat("between '{0}' ", arguments.StartTime);
            tmpSql.AppendFormat("and '{0}') s ", arguments.EndTime);
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
            tmpSql.AppendFormat("TO_CHAR(BEGIN_TIME,'{0}') workdate,",arguments.GroupingDateFormat);
            AppendMin(tmpSql, "BEGIN_TIME", "BEGIN_TIME");
            AppendMax(tmpSql, "END_TIME", "END_TIME");
            foreach (string param in statisticParamNames)
            {
                AppendRoundWithAVG(tmpSql, param);
                AppendWithComma(tmpSql, "");
            }
            tmpSql.Remove(tmpSql.Length - 1, 1);
            tmpSql.AppendFormat("FROM t1_sysstat s where 1=1 group by dbid, inst_id, TO_CHAR(BEGIN_TIME,'{0}')",arguments.GroupingDateFormat);
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
            ;


            RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                   tmpSql.ToString(), false);

            return db.Select(tmpSql.ToString()).Tables[0];
        }

    }
}
