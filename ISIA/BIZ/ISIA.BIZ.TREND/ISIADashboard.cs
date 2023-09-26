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
                List<DbLinkInfo> dbLinkList = GetDbNames(db, arguments);
                foreach (DbLinkInfo ele in dbLinkList)
                {//刚开始添加新的数据库的时候，可能不存在数据所以返回null；
                    DbInfo info = GetSingleDbInfo(db, ele);
                    if (info == null)
                    {
                        continue;
                    }
                    dbInfos.Add(info);
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

        public void GetDBFetchAwrDataStatus(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@" SELECT process_name dbname
                                   FROM log_tab
                                   WHERE start_time >= TO_CHAR (SYSDATE - {0} / 24, 'yyyymmddhh24miss')
                                   GROUP BY process_name, success_flag
                                   having success_flag='N'", arguments.StartTime);

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

        public void GetSnapAlreadyFetchCount(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@" select   to_char(begin_interval_time, 'yyyy-mm-dd') workdate, count(*) count
 from raw_dba_hist_snapshot_{0} 
 group by to_char(begin_interval_time, 'yyyy-mm-dd')
 having to_char(begin_interval_time, 'yyyy-mm-dd')>= to_char(sysdate-{1}, 'yyyy-mm-dd')
order by to_char(begin_interval_time, 'yyyy-mm-dd')", arguments.DBName,arguments.StartTime);

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

        public void GetFetchProcedureErrorMessage(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat(@"SELECT process_name processname, start_time starttime, end_time endtime, error_msg errormessage
 FROM log_tab
WHERE start_time >= TO_CHAR (SYSDATE - {0} / 24, 'yyyymmddhh24miss') and  success_flag='N'
order by start_time desc ",  arguments.StartTime);

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



        private List<DbLinkInfo> GetDbNames(DBCommunicator db, AwrArgsPack argsPack)
        {
            StringBuilder dbNameSql = new StringBuilder();
            dbNameSql.Append(@"select dbname, dbid, DBLINKNAME DBLINK from ISIA.TAPCTDATABASE where isalive='YES'");
            if (!string.IsNullOrEmpty(argsPack.DBName))
            {
                dbNameSql.AppendFormat(" and  dbname like '%{0}%'", argsPack.DBName);
            }
            RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                      dbNameSql.ToString(), false);
            DataSet dbNameDs = db.Select(dbNameSql.ToString());
            DataTable dt = dbNameDs.Tables[0];
            List<DbLinkInfo> list = Utils.DataTableToList<DbLinkInfo>(dt);
            
            return list;
        }

        private DbInfo GetSingleDbInfo(DBCommunicator db, DbLinkInfo dbLinkInfo)
        {
            StringBuilder dbInfoSql = new StringBuilder();
            dbInfoSql.AppendFormat($@"SELECT *
  FROM (
select dbInfo.*, db.dbname  cdbname,
case when cdbid is null then null else 'pdb' end targettype
from (select distinct p.value version, d.dbname, 
       case when p.con_dbid = p.dbid then null
       else
       (select distinct dbid from tapctdatabase where dbid = p.con_dbid)
       end cdbid
       ,decode(isalive,'YES', 1,0) STATUS, s.mintime, s.maxtime, s.cnt ,d.instantcnt instancecount 
from RAW_DBA_HIST_PARAMETER_{dbLinkInfo.DBNAME.ToUpper()} p,
     tapctdatabase d,
     (select to_char(min(s.begin_interval_time), 'YYYY-MM-DD HH24:MI:SS') mintime, to_char(max(s.end_interval_time), 'YYYY-MM-DD HH24:MI:SS') maxtime,count(s.snap_id) cnt
from raw_dba_hist_snapshot_{dbLinkInfo.DBNAME.ToUpper()} s) s
where p.snap_id = (select min(snap_id)
                    from raw_dba_hist_snapshot_{dbLinkInfo.DBNAME.ToUpper()}
                    where begin_interval_time > sysdate - 2/24  )
and p.parameter_name = 'compatible'
and d.dbname='{dbLinkInfo.DBNAME.ToUpper()}'
and p.dbid = d.dbid) dbInfo  LEFT JOIN tapctdatabase db ON dbinfo.cdbid = db.dbid
         WHERE ROWNUM <= 1) d,
       (SELECT EXTRACT (DAY FROM retention)           RETENTIONDAYS,
               EXTRACT(MINUTE FROM snap_interval)    INTERVALMINUTES
          FROM dba_hist_wr_control@{dbLinkInfo.DBLINK}
         WHERE dbid = {dbLinkInfo.DBID}) f");
            RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                      dbInfoSql.ToString(), false);
            DataSet dbInfoSqlDs = db.Select(dbInfoSql.ToString());
            try
            {
                DbInfo dbInfo = Utils.DataTableToList<DbInfo>(dbInfoSqlDs.Tables[0])[0];
                return dbInfo;

            }
            catch
            {
                return null;
            }

        }

        public class DbLinkInfo
        {

            private string dbName;
            private string dbId;
            private string dbLink;



            public DbLinkInfo()
            {

            }

            public string DBNAME { get => dbName; set => dbName = value; }
            public string DBID { get => dbId; set => dbId = value; }
            public string DBLINK { get => dbLink; set => dbLink = value; }
        }
        public class DbInfo
        {

            public DbInfo()
            {

            }

           
            private string version;
            private string dbName;
            private string retentionDays;
            private string cdbName;
            private string cdbId;
            private string retentionPeriod;
            private string uploadInterval;
            private string minTime;
            private string maxTime;
            private int cnt;
            private string targetType;
            private int status;
            private int instanceCount;
            private string intervalMinutes;



            public string VERSION { get => version; set => version = value; }
            public string DBNAME { get => dbName; set => dbName = value; }
            public string CDBNAME { get => cdbName; set => cdbName = value; }
            public string RETENTIONPERIOD { get => retentionPeriod; set => retentionPeriod = value; }
            public string UPLOADINTERVAL { get => uploadInterval; set => uploadInterval = value; }
            public string MINTIME { get => minTime; set => minTime = value; }
            public string MAXTIME { get => maxTime; set => maxTime = value; }
            public int CNT { get => cnt; set => cnt = value; }
            public string TARGETTYPE { get => targetType; set => targetType = value; }
            public int STATUS { get => status; set => status = value; }
            public int INSTANCECOUNT { get => instanceCount; set => instanceCount = value; }
            public string CDBID { get => cdbId; set => cdbId = value; }
            public string INTERVALMINUTES { get => intervalMinutes; set => intervalMinutes = value; }
            public string RETENTIONDAYS { get => retentionDays; set => retentionDays = value; }
        }
    }
}
