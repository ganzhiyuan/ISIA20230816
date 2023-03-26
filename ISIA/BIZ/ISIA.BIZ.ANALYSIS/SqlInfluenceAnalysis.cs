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

               
                    tmpSql.AppendFormat(
                        "select si.* , ts.sql_text\r\n" +
                        "from\r\n" +
                        "(select stat.sql_id, min(begin_interval_time) begin_interval_time,max(end_interval_time) end_interval_time, \r\n" +
                        "NVL(sum({0}),0) \"{0}\"\r\n" +
                        "from ISIA.RAW_DBA_HIST_SQLSTAT_{3} stat \r\n" +
                        "left join ISIA.RAW_DBA_HIST_SNAPSHOT_{3} snap on \r\n" +
                        "stat.snap_id=snap.snap_id  \r\n" +
                        "where  TO_CHAR (snap.end_INTERVAL_TIME, 'yyyyMMddHH24miss') BETWEEN '{1}' and '{2}'\r\n" +
                        "group by stat.sql_id\r\n" +
                        ") si \r\n" +
                        "left join  ISIA.RAW_DBA_HIST_SQLTEXT_{3}  ts \r\n" +
                        "on si.sql_id=ts.sql_id\r\n" +
                        "order by \"{0}\" desc\r\n" 
                        , arguments.WorkloadSqlParm,arguments.StartTime,arguments.EndTime,arguments.DBName
                     );
               
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
