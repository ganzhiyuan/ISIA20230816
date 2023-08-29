using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TAP;
using TAP.Data.DataBase.Communicators;
using TAP.Remoting;

namespace ISIA.BIZ.MANAGEMENT
{
    class CreateDataTableManagement : TAP.Remoting.Server.Biz.BizComponentBase
    {
        public void GetDataTable()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append(@"WITH
                                PART_TAB
                                AS
                                    (SELECT TABLE_NAME
                                       FROM USER_PART_TABLES
                                      WHERE TABLE_NAME NOT LIKE 'BIN%')
                                SELECT '' IS_PARTITION, TABLE_NAME
                                  FROM USER_TABLES T
                                 WHERE     TABLE_NAME NOT LIKE 'BIN%'
                                       AND NOT EXISTS
                                               (SELECT 1
                                                  FROM PART_TAB
                                                 WHERE TABLE_NAME = T.TABLE_NAME)
                                UNION ALL
                                SELECT 'P' IS_PARTITION, TABLE_NAME
                                  FROM USER_TABLES T
                                 WHERE     TABLE_NAME NOT LIKE 'BIN%'
                                       AND EXISTS
                                               (SELECT 1
                                                  FROM PART_TAB
                                                 WHERE TABLE_NAME = T.TABLE_NAME)
                                ORDER BY TABLE_NAME");
                

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

        public void getPartitioning(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"select max(pt.partitioning_type) partitioning_type, count(*) partition_count, max(pt.interval) interval,
                                            max(pkc.column_name) column_name  
                                    from USER_PART_TABLES pt,
                                            USER_TAB_PARTITIONS tp,
                                            USER_PART_KEY_COLUMNS pkc
                                    where pt.table_name = '{0}'
                                    and pt.table_name = tp.table_name
                                    and pt.table_name = pkc.name", arguments.TABLENAME);


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


        public void getPartition(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"
                                        select tp.partition_name,  tp.partition_position, high_value, 
                                               tp.tablespace_name, tp.compression, tp.num_rows, tp.blocks, tp.avg_space, tp.last_analyzed
                                        from USER_TAB_PARTITIONS tp
                                        where table_name = '{0}'
                                        order by tp.partition_position", arguments.TABLENAME);


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
