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

namespace ISIA.BIZ.TREND
{

    class ISIADashboard : TAP.Remoting.Server.Biz.BizComponentBase
    {
        public void GetDashBoardData(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            DataSet result = new DataSet();
            List<DbInfo> dbInfos = new List<DbInfo>();
            try
            {
                if (string.IsNullOrEmpty(arguments.DBName))
                {

                    List<string> dbNameList = GetDbNames(db);
                    foreach (string dbName in dbNameList)
                    {
                        dbInfos.Add(GetSingleDbInfo(db, dbName));
                    }
                }
                else
                {
                    dbInfos.Add(GetSingleDbInfo(db, arguments.DBName));
                }
                result = Utils.ConvertToDataSet<DbInfo>(dbInfos);
                this.ExecutingValue = result;

            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        

        //DataTable转List
        public static List<T> GetList<T>(DataTable table)
        {
            List<T> list = new List<T>();
            T t = default(T);
            PropertyInfo[] propertypes = null;
            string tempName = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                t = Activator.CreateInstance<T>();
                propertypes = t.GetType().GetProperties();
                foreach (PropertyInfo pro in propertypes)
                {
                    tempName = pro.Name;
                    if (table.Columns.Contains(tempName))
                    {
                        object value = row[tempName];
                        if (!value.ToString().Equals(""))
                        {
                            pro.SetValue(t, value, null);
                        }
                    }
                }
                list.Add(t);
            }
            return list.Count == 0 ? null : list;
        }

        private List<string> GetDbNames(DBCommunicator db)
        {
            StringBuilder dbNameSql = new StringBuilder();
            dbNameSql.Append(@"select dbname from ISIA.TAPCTDATABASE where isalive='YES'");
            RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                      dbNameSql.ToString(), false);
            DataSet dbNameDs = db.Select(dbNameSql.ToString());
            DataTable dt = dbNameDs.Tables[0];
            //取出第一列
            DataColumn dc = dt.Columns[0];
            //取出第一列的值
            List<string> list = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr[dc].ToString());
            }
            return list;
        }

        private DbInfo GetSingleDbInfo(DBCommunicator db, string dbName)
        {
            StringBuilder dbInfoSql = new StringBuilder();
            dbInfoSql.AppendFormat(@"select dbInfo.*,case when cdbname is null then null else 'pdb' end targettype
from (select distinct p.value version, d.dbname, d.retentiondays,
       case when p.con_dbid = p.dbid then null
       else
       (select dbname from tapctdatabase where dbid = p.con_dbid)
       end cdbname
       ,decode(isalive,'YES', 1,0) STATUS, '10 min'  uploadinterval, s.mintime, s.maxtime, s.cnt
from RAW_DBA_HIST_PARAMETER_{0} p,
     tapctdatabase d,
     (select to_char(min(s.begin_interval_time), 'YYYY-MM-DD HH24:MI:SS') mintime, to_char(max(s.end_interval_time), 'YYYY-MM-DD HH24:MI:SS') maxtime,count(s.snap_id) cnt
from raw_dba_hist_snapshot_{0} s) s
where p.snap_id = (select snap_id
                    from RAW_DBA_HIST_PARAMETER_{0}
                   where begin_time > sysdate - 2/24 
                   and rownum = 1 )
and p.parameter_name = 'compatible'
and p.dbid = d.dbid) dbInfo", dbName);
            RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                      dbInfoSql.ToString(), false);
            DataSet dbInfoSqlDs = db.Select(dbInfoSql.ToString());

            return Utils.DataTableToList<DbInfo>(dbInfoSqlDs.Tables[0])[0];
        }
        public class DbInfo
        {

            public DbInfo()
            {

            }

            public DbInfo(string version, string dbName, int retentionDays, string cdbName, string retentionPeriod, string uploadInterval, string minTime, string maxTime, int cnt, string targetType)
            {
                this.version = version;
                this.dbName = dbName;
                this.retentionDays = retentionDays;
                this.cdbName = cdbName;
                this.retentionPeriod = retentionPeriod;
                this.uploadInterval = uploadInterval;
                this.minTime = minTime;
                this.maxTime = maxTime;
                this.cnt = cnt;
                this.targetType = targetType;
            }

            private string version;
            private string dbName;
            private int retentionDays;
            private string cdbName;
            private string retentionPeriod;
            private string uploadInterval;
            private string minTime;
            private string maxTime;
            private int cnt;
            private string targetType;
            private int status;


            public string VERSION { get => version; set => version = value; }
            public string DBNAME { get => dbName; set => dbName = value; }
            public int RETENTIONDAYS { get => retentionDays; set => retentionDays = value; }
            public string CDBNAME { get => cdbName; set => cdbName = value; }
            public string RETENTIONPERIOD { get => retentionPeriod; set => retentionPeriod = value; }
            public string UPLOADINTERVAL { get => uploadInterval; set => uploadInterval = value; }
            public string MINTIME { get => minTime; set => minTime = value; }
            public string MAXTIME { get => maxTime; set => maxTime = value; }
            public int CNT { get => cnt; set => cnt = value; }
            public string TARGETTYPE { get => targetType; set => targetType = value; }
            public int STATUS { get => status; set => status = value; }
        }
    }
}
