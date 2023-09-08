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

namespace ISIA.BIZ.MANAGEMENT
{
    class CreateDataBaseManagement : TAP.Remoting.Server.Biz.BizComponentBase
    {
        public void GetDBLink(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("SELECT * FROM ALL_DB_LINKS WHERE DB_LINK = '{0}'", arguments.DBLinkName);
                

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

        public void GetToCreateAwrTableMessage(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("select TABLENAME \"Table Name\", ISPARTITIONING \"Partitioning Yn\",PARTITIONUNIT \"Partitionin" +
                    "g Unit\",RETENTIONMONTH \"Retention Month\",DATATBSPNAME \"Data TBSP\",INDEXTBSPNAME \"Index TBSP\" " +
                    "from tapctretentionpolicy a order by sequences ");
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

        public void DropDBlink(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("DROP DATABASE LINK {0} ", arguments.DBLinkName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }



        public void CreateDBLink(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append(arguments.Script);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);
                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });

            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void CreateTable(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append(arguments.Script);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);
                string[] cmds = tmpSql.ToString().Split(';');
                foreach(string cmd in cmds)
                {
                    try
                    {
                        this.ExecutingValue = db.Save(new string[] { cmd });
                    }
                    catch(Exception ex)
                    {
                        RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                    }
                }
                

            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetDataTable(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("SELECT table_name  FROM all_tables WHERE table_name = '{0}'", arguments.DataTableName);

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

        public void DropDataTable(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("DROP TABLE {0} ", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetTableColumnDesc(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("select column_name \"Column\", Data_type \"Type\", Data_length \"DataLength\" from dba_tab_columns where table_name='{0}'order by column_id ", arguments.DataTableName);

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

        public void GetTableIndexKey(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("select prev12ckeycolumns ,later12ckeycolumns from tapctretentionpolicy where  tablename='{0}' ", arguments.DataTableName);

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

        public void GetDbLinkVersion(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                try
                {
                    StringBuilder tmpSql = new StringBuilder();

                    tmpSql.AppendFormat(@"	select DBID, NAME SERVICENAME, version VERSION, 
                             (select count(instance_number) from gv$instance@{0}) instancecount, 'yes' ispdb
                              from v$pdbs@{0} d,
                              v$instance@{0} i ", arguments.DBLinkName);

                    RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                           tmpSql.ToString(), false);
                    DataSet res=db.Select(tmpSql.ToString());
                    if (res.Tables[0].Rows.Count == 0)
                    {
                        throw new Exception();
                    }
                    this.ExecutingValue = res;
                    return;
                }
                catch
                {
                    StringBuilder tmpSql = new StringBuilder();

                    tmpSql.AppendFormat(@"select DBID, NAME SERVICENAME, version, 
                                 (select count(*) from v$active_instances@{0}) instancecount, 'no' ispdb
                                   from v$database@{0} d,
                                    v$instance@{0} i   ", arguments.DBLinkName);

                    RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                           tmpSql.ToString(), false);

                    this.ExecutingValue = db.Select(tmpSql.ToString());
                }
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        #region RAW_DBA_HIST_ACTIVE_SESS_HISTORY
        public void CreateRAWDBAHISTACTIVESESSHISTORY(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                    (
                                      SNAP_ID                      NUMBER,
                                      DBID                         NUMBER,
                                      INSTANCE_NUMBER              NUMBER,
                                      SAMPLE_ID                    NUMBER,
                                      SAMPLE_TIME                  TIMESTAMP(3),
                                      SAMPLE_TIME_UTC              TIMESTAMP(3),
                                      USECS_PER_ROW                NUMBER,
                                      SESSION_ID                   NUMBER,
                                      SESSION_SERIAL#              NUMBER,
                                      SESSION_TYPE                 VARCHAR2(10 BYTE),
                                      FLAGS                        NUMBER,
                                      USER_ID                      NUMBER,
                                      SQL_ID                       VARCHAR2(13 BYTE),
                                      IS_SQLID_CURRENT             VARCHAR2(1 BYTE),
                                      SQL_CHILD_NUMBER             NUMBER,
                                      SQL_OPCODE                   NUMBER,
                                      SQL_OPNAME                   VARCHAR2(64 BYTE),
                                      FORCE_MATCHING_SIGNATURE     NUMBER,
                                      TOP_LEVEL_SQL_ID             VARCHAR2(13 BYTE),
                                      TOP_LEVEL_SQL_OPCODE         NUMBER,
                                      SQL_PLAN_HASH_VALUE          NUMBER,
                                      SQL_FULL_PLAN_HASH_VALUE     NUMBER,
                                      SQL_ADAPTIVE_PLAN_RESOLVED   NUMBER,
                                      SQL_PLAN_LINE_ID             NUMBER,
                                      SQL_PLAN_OPERATION           VARCHAR2(64 BYTE),
                                      SQL_PLAN_OPTIONS             VARCHAR2(64 BYTE),
                                      SQL_EXEC_ID                  NUMBER,
                                      SQL_EXEC_START               DATE,
                                      PLSQL_ENTRY_OBJECT_ID        NUMBER,
                                      PLSQL_ENTRY_SUBPROGRAM_ID    NUMBER,
                                      PLSQL_OBJECT_ID              NUMBER,
                                      PLSQL_SUBPROGRAM_ID          NUMBER,
                                      QC_INSTANCE_ID               NUMBER,
                                      QC_SESSION_ID                NUMBER,
                                      QC_SESSION_SERIAL#           NUMBER,
                                      PX_FLAGS                     NUMBER,
                                      EVENT                        VARCHAR2(64 BYTE),
                                      EVENT_ID                     NUMBER,
                                      SEQ#                         NUMBER,
                                      P1TEXT                       VARCHAR2(64 BYTE),
                                      P1                           NUMBER,
                                      P2TEXT                       VARCHAR2(64 BYTE),
                                      P2                           NUMBER,
                                      P3TEXT                       VARCHAR2(64 BYTE),
                                      P3                           NUMBER,
                                      WAIT_CLASS                   VARCHAR2(64 BYTE),
                                      WAIT_CLASS_ID                NUMBER,
                                      WAIT_TIME                    NUMBER,
                                      SESSION_STATE                VARCHAR2(7 BYTE),
                                      TIME_WAITED                  NUMBER,
                                      BLOCKING_SESSION_STATUS      VARCHAR2(11 BYTE),
                                      BLOCKING_SESSION             NUMBER,
                                      BLOCKING_SESSION_SERIAL#     NUMBER,
                                      BLOCKING_INST_ID             NUMBER,
                                      BLOCKING_HANGCHAIN_INFO      VARCHAR2(1 BYTE),
                                      CURRENT_OBJ#                 NUMBER,
                                      CURRENT_FILE#                NUMBER,
                                      CURRENT_BLOCK#               NUMBER,
                                      CURRENT_ROW#                 NUMBER,
                                      TOP_LEVEL_CALL#              NUMBER,
                                      TOP_LEVEL_CALL_NAME          VARCHAR2(64 BYTE),
                                      CONSUMER_GROUP_ID            NUMBER,
                                      XID                          RAW(8),
                                      REMOTE_INSTANCE#             NUMBER,
                                      TIME_MODEL                   NUMBER,
                                      IN_CONNECTION_MGMT           VARCHAR2(1 BYTE),
                                      IN_PARSE                     VARCHAR2(1 BYTE),
                                      IN_HARD_PARSE                VARCHAR2(1 BYTE),
                                      IN_SQL_EXECUTION             VARCHAR2(1 BYTE),
                                      IN_PLSQL_EXECUTION           VARCHAR2(1 BYTE),
                                      IN_PLSQL_RPC                 VARCHAR2(1 BYTE),
                                      IN_PLSQL_COMPILATION         VARCHAR2(1 BYTE),
                                      IN_JAVA_EXECUTION            VARCHAR2(1 BYTE),
                                      IN_BIND                      VARCHAR2(1 BYTE),
                                      IN_CURSOR_CLOSE              VARCHAR2(1 BYTE),
                                      IN_SEQUENCE_LOAD             VARCHAR2(1 BYTE),
                                      IN_INMEMORY_QUERY            VARCHAR2(1 BYTE),
                                      IN_INMEMORY_POPULATE         VARCHAR2(1 BYTE),
                                      IN_INMEMORY_PREPOPULATE      VARCHAR2(1 BYTE),
                                      IN_INMEMORY_REPOPULATE       VARCHAR2(1 BYTE),
                                      IN_INMEMORY_TREPOPULATE      VARCHAR2(1 BYTE),
                                      IN_TABLESPACE_ENCRYPTION     VARCHAR2(1 BYTE),
                                      CAPTURE_OVERHEAD             VARCHAR2(1 BYTE),
                                      REPLAY_OVERHEAD              VARCHAR2(1 BYTE),
                                      IS_CAPTURED                  VARCHAR2(1 BYTE),
                                      IS_REPLAYED                  VARCHAR2(1 BYTE),
                                      IS_REPLAY_SYNC_TOKEN_HOLDER  VARCHAR2(1 BYTE),
                                      SERVICE_HASH                 NUMBER,
                                      PROGRAM                      VARCHAR2(64 BYTE),
                                      MODULE                       VARCHAR2(64 BYTE),
                                      ACTION                       VARCHAR2(64 BYTE),
                                      CLIENT_ID                    VARCHAR2(64 BYTE),
                                      MACHINE                      VARCHAR2(64 BYTE),
                                      PORT                         NUMBER,
                                      ECID                         VARCHAR2(64 BYTE),
                                      DBREPLAY_FILE_ID             NUMBER,
                                      DBREPLAY_CALL_COUNTER        NUMBER,
                                      TM_DELTA_TIME                NUMBER,
                                      TM_DELTA_CPU_TIME            NUMBER,
                                      TM_DELTA_DB_TIME             NUMBER,
                                      DELTA_TIME                   NUMBER,
                                      DELTA_READ_IO_REQUESTS       NUMBER,
                                      DELTA_WRITE_IO_REQUESTS      NUMBER,
                                      DELTA_READ_IO_BYTES          NUMBER,
                                      DELTA_WRITE_IO_BYTES         NUMBER,
                                      DELTA_INTERCONNECT_IO_BYTES  NUMBER,
                                      PGA_ALLOCATED                NUMBER,
                                      TEMP_SPACE_ALLOCATED         NUMBER,
                                      DBOP_NAME                    VARCHAR2(64 BYTE),
                                      DBOP_EXEC_ID                 NUMBER,
                                      CON_DBID                     NUMBER,
                                      CON_ID                       NUMBER,
                                      BEGIN_TIME                   DATE,
                                      END_TIME                     DATE
                                    )
                                    NOCOMPRESS 
                                    TABLESPACE DEFAULT_TS
                                    PCTUSED    0
                                    PCTFREE    10
                                    INITRANS   1
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )
                                    PARTITION BY RANGE (BEGIN_TIME)
                                    INTERVAL( NUMTOYMINTERVAL(1, 'MONTH'))
                                    ( " , arguments.DataTableName);

                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00
                
                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime =  dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;

                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }

                    string res = $"{year.ToString().Substring(2)}{month-1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION ACTIVE_SESS_HISTORY_{0}_{1} VALUES LESS THAN 
                                        (TO_DATE(' {2}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))
                                        LOGGING
                                        NOCOMPRESS
                                        TABLESPACE PART_TS_DATA
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE(
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                         )," ,arguments.DBLinkName , res, formatDateTime);

                    if (i ==11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }
                                    tmpSql.Append(@"              )
                                                    NOCACHE
                                                    MONITORING");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTACTIVESESSHISTORY(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, SAMPLE_ID, SESSION_ID, 
                                    CON_DBID, BEGIN_TIME, END_TIME)
                                      TABLESPACE PART_TS_IDX
                                      PCTFREE    10
                                      INITRANS   2
                                      MAXTRANS   255
                                      STORAGE    (
                                                  BUFFER_POOL      DEFAULT
                                                 )
                                    LOCAL (  
                                      
                                    ", arguments.DataTableName, arguments.DataTablePKName);


                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;

                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }

                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION ACTIVE_SESS_HISTORY_{0}_{1}
                                            LOGGING
                                            NOCOMPRESS 
                                            TABLESPACE PART_TS_IDX
                                            INITRANS   2
                                            MAXTRANS   255
                                            STORAGE    (
                                                        INITIAL          64K
                                                        NEXT             1M
                                                        MINEXTENTS       1
                                                        MAXEXTENTS       UNLIMITED
                                                        BUFFER_POOL      DEFAULT
                                                       ),", arguments.DBLinkName, res);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }

                tmpSql.Append("    )");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTACTIVESESSHISTORY(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, SAMPLE_ID, SESSION_ID, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_BUFFER_POOL_STAT
        public void CreateRAWDBAHISTBUFFERPOOLSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                        (
                                          SNAP_ID                  NUMBER,
                                          DBID                     NUMBER,
                                          INSTANCE_NUMBER          NUMBER,
                                          ID                       NUMBER,
                                          NAME                     VARCHAR2(20 BYTE),
                                          BLOCK_SIZE               NUMBER,
                                          SET_MSIZE                NUMBER,
                                          CNUM_REPL                NUMBER,
                                          CNUM_WRITE               NUMBER,
                                          CNUM_SET                 NUMBER,
                                          BUF_GOT                  NUMBER,
                                          SUM_WRITE                NUMBER,
                                          SUM_SCAN                 NUMBER,
                                          FREE_BUFFER_WAIT         NUMBER,
                                          WRITE_COMPLETE_WAIT      NUMBER,
                                          BUFFER_BUSY_WAIT         NUMBER,
                                          FREE_BUFFER_INSPECTED    NUMBER,
                                          DIRTY_BUFFERS_INSPECTED  NUMBER,
                                          DB_BLOCK_CHANGE          NUMBER,
                                          DB_BLOCK_GETS            NUMBER,
                                          CONSISTENT_GETS          NUMBER,
                                          PHYSICAL_READS           NUMBER,
                                          PHYSICAL_WRITES          NUMBER,
                                          CON_DBID                 NUMBER,
                                          CON_ID                   NUMBER,
                                          BEGIN_TIME               DATE,
                                          END_TIME                 DATE
                                        )
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                   )
                                        LOGGING 
                                        NOCOMPRESS 
                                        NOCACHE
                                        MONITORING", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTBUFFERPOOLSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, ID, CON_DBID, 
                                    BEGIN_TIME, END_TIME)
                                    LOGGING
                                    TABLESPACE DEFAULT_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )", arguments.DataTableName, arguments.DataTablePKName);


               

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTBUFFERPOOLSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, ID, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX {1}
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_CR_BLOCK_SERVER
        public void CreateRAWDBAHISTCRBLOCKSERVER(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                        (
                                          SNAP_ID                 NUMBER,
                                          DBID                    NUMBER,
                                          INSTANCE_NUMBER         NUMBER,
                                          CR_REQUESTS             NUMBER,
                                          CURRENT_REQUESTS        NUMBER,
                                          DATA_REQUESTS           NUMBER,
                                          UNDO_REQUESTS           NUMBER,
                                          TX_REQUESTS             NUMBER,
                                          CURRENT_RESULTS         NUMBER,
                                          PRIVATE_RESULTS         NUMBER,
                                          ZERO_RESULTS            NUMBER,
                                          DISK_READ_RESULTS       NUMBER,
                                          FAIL_RESULTS            NUMBER,
                                          FAIRNESS_DOWN_CONVERTS  NUMBER,
                                          FLUSHES                 NUMBER,
                                          BUILDS                  NUMBER,
                                          LIGHT_WORKS             NUMBER,
                                          ERRORS                  NUMBER,
                                          CON_DBID                NUMBER,
                                          CON_ID                  NUMBER,
                                          BEGIN_TIME              DATE,
                                          END_TIME                DATE
                                        )
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                   )
                                        LOGGING 
                                        NOCOMPRESS 
                                        NOCACHE
                                        MONITORING", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTCRBLOCKSERVER(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, CON_DBID, BEGIN_TIME, 
                                    END_TIME)
                                    LOGGING
                                    TABLESPACE DEFAULT_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )", arguments.DataTableName, arguments.DataTablePKName);




                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTCRBLOCKSERVER(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX {1}
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_CURRENT_BLOCK_SERVER
        public void CreateRAWDBAHISTCURRENTBLOCKSERVER(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                        (
                                          SNAP_ID          NUMBER,
                                          DBID             NUMBER,
                                          INSTANCE_NUMBER  NUMBER,
                                          PIN0             NUMBER,
                                          PIN1             NUMBER,
                                          PIN10            NUMBER,
                                          PIN100           NUMBER,
                                          PIN1000          NUMBER,
                                          PIN10000         NUMBER,
                                          FLUSH0           NUMBER,
                                          FLUSH1           NUMBER,
                                          FLUSH10          NUMBER,
                                          FLUSH100         NUMBER,
                                          FLUSH1000        NUMBER,
                                          FLUSH10000       NUMBER,
                                          CON_DBID         NUMBER,
                                          CON_ID           NUMBER,
                                          BEGIN_TIME       DATE,
                                          END_TIME         DATE
                                        )
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                   )
                                        LOGGING 
                                        NOCOMPRESS 
                                        NOCACHE
                                        MONITORING", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTCURRENTBLOCKSERVER(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, CON_DBID, BEGIN_TIME, 
                                    END_TIME)
                                    LOGGING
                                    TABLESPACE DEFAULT_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )", arguments.DataTableName, arguments.DataTablePKName);




                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTCURRENTBLOCKSERVER(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX {1}
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_DLM_MISC
        public void CreateRAWDBAHISTDLMMISC(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                    (
                                          SNAP_ID          NUMBER,
                                          DBID             NUMBER,
                                          INSTANCE_NUMBER  NUMBER,
                                          STATISTIC#       NUMBER,
                                          NAME             VARCHAR2(38 BYTE),
                                          VALUE            NUMBER,
                                          CON_DBID         NUMBER,
                                          CON_ID           NUMBER,
                                          BEGIN_TIME       DATE,
                                          END_TIME         DATE
                                        )
                                        NOCOMPRESS 
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                   )
                                        PARTITION BY RANGE (BEGIN_TIME)
                                        INTERVAL( NUMTOYMINTERVAL(1, 'MONTH'))
                                        ( ", arguments.DataTableName);

                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;

                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }

                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION DLM_MISC_{0}_{1} VALUES LESS THAN (TO_DATE(' {2}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_DATA
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res, formatDateTime);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }
                tmpSql.Append(@"  )
                                NOCACHE
                                MONITORING");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTDLMMISC(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, STATISTIC#, CON_DBID, 
                                    BEGIN_TIME, END_TIME)
                                      TABLESPACE PART_TS_IDX
                                      PCTFREE    10
                                      INITRANS   2
                                      MAXTRANS   255
                                      STORAGE    (
                                                  BUFFER_POOL      DEFAULT
                                                 )
                                    LOCAL (  
                                    ", arguments.DataTableName, arguments.DataTablePKName);


                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION DLM_MISC_{0}_{1}
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_IDX
                                        INITRANS   2
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }

                tmpSql.Append("    )");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTDLMMISC(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, STATISTIC#, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_ENQUEUE_STAT
        public void CreateRAWDBAHISTENQUEUESTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                        (
                                          SNAP_ID          NUMBER,
                                          DBID             NUMBER,
                                          INSTANCE_NUMBER  NUMBER,
                                          EQ_TYPE          VARCHAR2(2 BYTE),
                                          REQ_REASON       VARCHAR2(64 BYTE),
                                          TOTAL_REQ#       NUMBER,
                                          TOTAL_WAIT#      NUMBER,
                                          SUCC_REQ#        NUMBER,
                                          FAILED_REQ#      NUMBER,
                                          CUM_WAIT_TIME    NUMBER,
                                          EVENT#           NUMBER,
                                          CON_DBID         NUMBER,
                                          CON_ID           NUMBER,
                                          BEGIN_TIME       DATE,
                                          END_TIME         DATE
                                        )
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                   )
                                        LOGGING 
                                        NOCOMPRESS 
                                        NOCACHE
                                        MONITORING", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTENQUEUESTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, EQ_TYPE, REQ_REASON, 
                                    CON_DBID, BEGIN_TIME, END_TIME)
                                    LOGGING
                                    TABLESPACE DEFAULT_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )", arguments.DataTableName ,arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTENQUEUESTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, EQ_TYPE, REQ_REASON, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX {1}
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_LATCH_MISSES_SUMMARY
        public void CreateRAWDBAHISTLATCHMISSESSUMMARY(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                    (
                                        SNAP_ID          NUMBER,
                                        DBID             NUMBER,
                                        INSTANCE_NUMBER  NUMBER,
                                        PARENT_NAME      VARCHAR2(50 BYTE),
                                        WHERE_IN_CODE    VARCHAR2(64 BYTE),
                                        NWFAIL_COUNT     NUMBER,
                                        SLEEP_COUNT      NUMBER,
                                        WTR_SLP_COUNT    NUMBER,
                                        CON_DBID         NUMBER,
                                        CON_ID           NUMBER,
                                        BEGIN_TIME       DATE,
                                        END_TIME         DATE
                                    )
                                    NOCOMPRESS 
                                    TABLESPACE DEFAULT_TS
                                    PCTUSED    0
                                    PCTFREE    10
                                    INITRANS   1
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                                )
                                    PARTITION BY RANGE (BEGIN_TIME)
                                    INTERVAL( NUMTOYMINTERVAL(1, 'MONTH'))
                                    ( ", arguments.DataTableName);

                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION LATCH_MISSES_SUMMARY_{0}_{1} VALUES LESS THAN (TO_DATE(' {2}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_DATA
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                    ),", arguments.DBLinkName, res, formatDateTime);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }
                tmpSql.Append(@"  )
                                NOCACHE
                                MONITORING");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTLATCHMISSESSUMMARY(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, PARENT_NAME, WHERE_IN_CODE, 
                                    CON_DBID, BEGIN_TIME, END_TIME)
                                    TABLESPACE PART_TS_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                    BUFFER_POOL      DEFAULT
                                                 )
                                    LOCAL (  
                                    ", arguments.DataTableName, arguments.DataTablePKName);


                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    //string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION LATCH_MISSES_SUMMARY_{0}_{1}
                                            LOGGING
                                            NOCOMPRESS 
                                            TABLESPACE PART_TS_IDX
                                            INITRANS   2
                                            MAXTRANS   255
                                            STORAGE    (
                                                        INITIAL          64K
                                                        NEXT             1M
                                                        MINEXTENTS       1
                                                        MAXEXTENTS       UNLIMITED
                                                        BUFFER_POOL      DEFAULT
                                                       ),", arguments.DBLinkName, res);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }

                tmpSql.Append("    )");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTLATCHMISSESSUMMARY(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, PARENT_NAME, WHERE_IN_CODE, CON_DBID, BEGIN_TIME, END_TIME )
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_LATCH
        public void CreateRAWDBAHISTLATCH(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                    (
                                        SNAP_ID           NUMBER,
                                        DBID              NUMBER,
                                        INSTANCE_NUMBER   NUMBER,
                                        LATCH_HASH        NUMBER,
                                        LATCH_NAME        VARCHAR2(64 BYTE),
                                        LEVEL#            NUMBER,
                                        GETS              NUMBER,
                                        MISSES            NUMBER,
                                        SLEEPS            NUMBER,
                                        IMMEDIATE_GETS    NUMBER,
                                        IMMEDIATE_MISSES  NUMBER,
                                        SPIN_GETS         NUMBER,
                                        SLEEP1            NUMBER,
                                        SLEEP2            NUMBER,
                                        SLEEP3            NUMBER,
                                        SLEEP4            NUMBER,
                                        WAIT_TIME         NUMBER,
                                        CON_DBID          NUMBER,
                                        CON_ID            NUMBER,
                                        BEGIN_TIME        DATE,
                                        END_TIME          DATE
                                    )
                                    NOCOMPRESS 
                                    TABLESPACE DEFAULT_TS
                                    PCTUSED    0
                                    PCTFREE    10
                                    INITRANS   1
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                                )
                                    PARTITION BY RANGE (BEGIN_TIME)
                                    INTERVAL( NUMTOYMINTERVAL(1, 'MONTH'))
                                    (", arguments.DataTableName);

                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION LATCH_{0}_{1} VALUES LESS THAN (TO_DATE(' {2}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_DATA
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                    ),", arguments.DBLinkName, res, formatDateTime);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }
                tmpSql.Append(@"  )
                                NOCACHE
                                MONITORING");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTLATCH(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, LATCH_HASH, CON_DBID, 
                                    BEGIN_TIME, END_TIME)
                                    TABLESPACE PART_TS_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                BUFFER_POOL      DEFAULT
                                                )
                                    LOCAL ( 
                                    ", arguments.DataTableName, arguments.DataTablePKName);


                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    //string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION LATCH_{0}_{1}
                                            LOGGING
                                            NOCOMPRESS 
                                            TABLESPACE PART_TS_IDX
                                            INITRANS   2
                                            MAXTRANS   255
                                            STORAGE    (
                                                        INITIAL          64K
                                                        NEXT             1M
                                                        MINEXTENTS       1
                                                        MAXEXTENTS       UNLIMITED
                                                        BUFFER_POOL      DEFAULT
                                                       ),", arguments.DBLinkName, res);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }

                tmpSql.Append("    )");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTLATCH(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, LATCH_HASH, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_LIBRARYCACHE
        public void CreateRAWDBAHISTLIBRARYCACHE(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                        (
                                            SNAP_ID                    NUMBER,
                                            DBID                       NUMBER,
                                            INSTANCE_NUMBER            NUMBER,
                                            NAMESPACE                  VARCHAR2(15 BYTE),
                                            GETS                       NUMBER,
                                            GETHITS                    NUMBER,
                                            PINS                       NUMBER,
                                            PINHITS                    NUMBER,
                                            RELOADS                    NUMBER,
                                            INVALIDATIONS              NUMBER,
                                            DLM_LOCK_REQUESTS          NUMBER,
                                            DLM_PIN_REQUESTS           NUMBER,
                                            DLM_PIN_RELEASES           NUMBER,
                                            DLM_INVALIDATION_REQUESTS  NUMBER,
                                            DLM_INVALIDATIONS          NUMBER,
                                            CON_DBID                   NUMBER,
                                            CON_ID                     NUMBER,
                                            BEGIN_TIME                 DATE,
                                            END_TIME                   DATE
                                        )
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        LOGGING 
                                        NOCOMPRESS 
                                        NOCACHE
                                        MONITORING", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTLIBRARYCACHE(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, NAMESPACE, CON_DBID, 
                                    BEGIN_TIME, END_TIME)
                                    LOGGING
                                    TABLESPACE DEFAULT_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )", arguments.DataTableName, arguments.DataTablePKName);




                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTLIBRARYCACHE(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, NAMESPACE, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX {1}
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_OSSTAT
        public void CreateRAWDBAHISTOSSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                    (
                                        SNAP_ID          NUMBER,
                                        DBID             NUMBER,
                                        INSTANCE_NUMBER  NUMBER,
                                        STAT_ID          NUMBER,
                                        STAT_NAME        VARCHAR2(64 BYTE),
                                        VALUE            NUMBER,
                                        CON_DBID         NUMBER,
                                        CON_ID           NUMBER,
                                        BEGIN_TIME       DATE,
                                        END_TIME         DATE
                                    )
                                    NOCOMPRESS 
                                    TABLESPACE DEFAULT_TS
                                    PCTUSED    0
                                    PCTFREE    10
                                    INITRANS   1
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                                )
                                    PARTITION BY RANGE (BEGIN_TIME)
                                    INTERVAL( NUMTOYMINTERVAL(1, 'MONTH'))
                                    (  ", arguments.DataTableName);

                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION OSSTAT_{0}_{1} VALUES LESS THAN (TO_DATE(' {2}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_DATA
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res, formatDateTime);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }
                tmpSql.Append(@"  )
                                NOCACHE
                                MONITORING");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTOSSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, STAT_ID, CON_DBID, 
                                    BEGIN_TIME, END_TIME)
                                    TABLESPACE PART_TS_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                BUFFER_POOL      DEFAULT
                                                )
                                    LOCAL (   
                                    ", arguments.DataTableName, arguments.DataTablePKName);


                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    //string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION OSSTAT_{0}_{1}
                                            LOGGING
                                            NOCOMPRESS 
                                            TABLESPACE PART_TS_IDX
                                            INITRANS   2
                                            MAXTRANS   255
                                            STORAGE    (
                                                        INITIAL          64K
                                                        NEXT             1M
                                                        MINEXTENTS       1
                                                        MAXEXTENTS       UNLIMITED
                                                        BUFFER_POOL      DEFAULT
                                                       ),", arguments.DBLinkName, res);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }

                tmpSql.Append("    )");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTOSSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, STAT_ID, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_PARAMETER
        public void CreateRAWDBAHISTPARAMETER(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                    (
                                        SNAP_ID          NUMBER,
                                        DBID             NUMBER,
                                        INSTANCE_NUMBER  NUMBER,
                                        PARAMETER_HASH   NUMBER,
                                        PARAMETER_NAME   VARCHAR2(64 BYTE),
                                        VALUE            VARCHAR2(512 BYTE),
                                        ISDEFAULT        VARCHAR2(9 BYTE),
                                        ISMODIFIED       VARCHAR2(10 BYTE),
                                        CON_DBID         NUMBER,
                                        CON_ID           NUMBER,
                                        BEGIN_TIME       DATE,
                                        END_TIME         DATE
                                        )
                                        NOCOMPRESS 
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                   )
                                        PARTITION BY RANGE (BEGIN_TIME)
                                        INTERVAL( NUMTOYMINTERVAL(1, 'MONTH'))
                                        (  ", arguments.DataTableName);

                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION PARAMETER_{0}_{1} VALUES LESS THAN (TO_DATE(' {2}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_DATA
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res, formatDateTime);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }
                tmpSql.Append(@"  )
                                NOCACHE
                                MONITORING");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTPARAMETER(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, PARAMETER_HASH, CON_DBID, 
                                    BEGIN_TIME, END_TIME)
                                      TABLESPACE PART_TS_IDX
                                      PCTFREE    10
                                      INITRANS   2
                                      MAXTRANS   255
                                      STORAGE    (
                                                  BUFFER_POOL      DEFAULT
                                                 )
                                    LOCAL (     
                                    ", arguments.DataTableName, arguments.DataTablePKName);


                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    //string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION PARAMETER_{0}_{1}
                                            LOGGING
                                            NOCOMPRESS 
                                            TABLESPACE PART_TS_IDX
                                            INITRANS   2
                                            MAXTRANS   255
                                            STORAGE    (
                                                        INITIAL          64K
                                                        NEXT             1M
                                                        MINEXTENTS       1
                                                        MAXEXTENTS       UNLIMITED
                                                        BUFFER_POOL      DEFAULT
                                                       ),", arguments.DBLinkName, res);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }

                tmpSql.Append("    )");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTPARAMETER(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, PARAMETER_HASH, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_PGASTAT
        public void CreateRAWDBAHISTPGASTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                        (
                                            SNAP_ID          NUMBER,
                                            DBID             NUMBER,
                                            INSTANCE_NUMBER  NUMBER,
                                            NAME             VARCHAR2(64 BYTE),
                                            VALUE            NUMBER,
                                            CON_DBID         NUMBER,
                                            CON_ID           NUMBER,
                                            BEGIN_TIME       DATE,
                                            END_TIME         DATE
                                            )
                                            TABLESPACE DEFAULT_TS
                                            PCTUSED    0
                                            PCTFREE    10
                                            INITRANS   1
                                            MAXTRANS   255
                                            STORAGE    (
                                                        INITIAL          64K
                                                        NEXT             1M
                                                        MINEXTENTS       1
                                                        MAXEXTENTS       UNLIMITED
                                                        PCTINCREASE      0
                                                        BUFFER_POOL      DEFAULT
                                                       )
                                            LOGGING 
                                            NOCOMPRESS 
                                            NOCACHE
                                            MONITORING", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTPGASTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, NAME, CON_DBID, 
                                    BEGIN_TIME, END_TIME)
                                    LOGGING
                                    TABLESPACE DEFAULT_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )", arguments.DataTableName, arguments.DataTablePKName);




                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTPGASTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, NAME, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX {1}
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_RESOURCE_LIMIT
        public void CreateRAWDBAHISTRESOURCELIMIT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                        (
                                            SNAP_ID              NUMBER,
                                            DBID                 NUMBER,
                                            INSTANCE_NUMBER      NUMBER,
                                            RESOURCE_NAME        VARCHAR2(30 BYTE),
                                            CURRENT_UTILIZATION  NUMBER,
                                            MAX_UTILIZATION      NUMBER,
                                            INITIAL_ALLOCATION   VARCHAR2(10 BYTE),
                                            LIMIT_VALUE          VARCHAR2(10 BYTE),
                                            CON_DBID             NUMBER,
                                            CON_ID               NUMBER,
                                            BEGIN_TIME           DATE,
                                            END_TIME             DATE
                                            )
                                            TABLESPACE DEFAULT_TS
                                            PCTUSED    0
                                            PCTFREE    10
                                            INITRANS   1
                                            MAXTRANS   255
                                            STORAGE    (
                                                        INITIAL          64K
                                                        NEXT             1M
                                                        MINEXTENTS       1
                                                        MAXEXTENTS       UNLIMITED
                                                        PCTINCREASE      0
                                                        BUFFER_POOL      DEFAULT
                                                       )
                                            LOGGING 
                                            NOCOMPRESS 
                                            NOCACHE
                                            MONITORING", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTRESOURCELIMIT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, RESOURCE_NAME, CON_DBID, 
                                    BEGIN_TIME, END_TIME)
                                    LOGGING
                                    TABLESPACE DEFAULT_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )", arguments.DataTableName, arguments.DataTablePKName);




                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTRESOURCELIMIT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, RESOURCE_NAME, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX {1}
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_ROWCACHE_SUMMARY
        public void CreateRAWDBAHISTROWCACHESUMMARY(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                    (
                                        SNAP_ID          NUMBER,
                                        DBID             NUMBER,
                                        INSTANCE_NUMBER  NUMBER,
                                        PARAMETER        VARCHAR2(32 BYTE),
                                        TOTAL_USAGE      NUMBER,
                                        USAGE            NUMBER,
                                        GETS             NUMBER,
                                        GETMISSES        NUMBER,
                                        SCANS            NUMBER,
                                        SCANMISSES       NUMBER,
                                        SCANCOMPLETES    NUMBER,
                                        MODIFICATIONS    NUMBER,
                                        FLUSHES          NUMBER,
                                        DLM_REQUESTS     NUMBER,
                                        DLM_CONFLICTS    NUMBER,
                                        DLM_RELEASES     NUMBER,
                                        CON_DBID         NUMBER,
                                        CON_ID           NUMBER,
                                        BEGIN_TIME       DATE,
                                        END_TIME         DATE
                                        )
                                        NOCOMPRESS 
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                   )
                                        PARTITION BY RANGE (BEGIN_TIME)
                                        INTERVAL( NUMTOYMINTERVAL(1, 'MONTH'))
                                        (  ", arguments.DataTableName);

                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION ROWCACHE_SUMMARY_{0}_{1} VALUES LESS THAN (TO_DATE(' {2}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_DATA
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res, formatDateTime);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }
                tmpSql.Append(@"  )
                                NOCACHE
                                MONITORING");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTROWCACHESUMMARY(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, PARAMETER, CON_DBID, 
                                    BEGIN_TIME, END_TIME)
                                      TABLESPACE PART_TS_IDX
                                      PCTFREE    10
                                      INITRANS   2
                                      MAXTRANS   255
                                      STORAGE    (
                                                  BUFFER_POOL      DEFAULT
                                                 )
                                    LOCAL ( 
                                    ", arguments.DataTableName, arguments.DataTablePKName);


                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    //string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION ROWCACHE_SUMMARY_{0}_{1}
                                            LOGGING
                                            NOCOMPRESS 
                                            TABLESPACE PART_TS_IDX
                                            INITRANS   2
                                            MAXTRANS   255
                                            STORAGE    (
                                                        INITIAL          64K
                                                        NEXT             1M
                                                        MINEXTENTS       1
                                                        MAXEXTENTS       UNLIMITED
                                                        BUFFER_POOL      DEFAULT
                                                       ),", arguments.DBLinkName, res);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }

                tmpSql.Append("    )");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTROWCACHESUMMARY(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, PARAMETER, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_SEG_STAT_OBJ
        public void CreateRAWDBAHISTSEGSTATOBJ(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                        (
                                            DBID             NUMBER,
                                            TS#              NUMBER,
                                            OBJ#             NUMBER,
                                            DATAOBJ#         NUMBER,
                                            OWNER            VARCHAR2(128 BYTE),
                                            OBJECT_NAME      VARCHAR2(128 BYTE),
                                            SUBOBJECT_NAME   VARCHAR2(128 BYTE),
                                            OBJECT_TYPE      VARCHAR2(18 BYTE),
                                            TABLESPACE_NAME  VARCHAR2(30 BYTE),
                                            PARTITION_TYPE   VARCHAR2(8 BYTE),
                                            CON_DBID         NUMBER,
                                            CON_ID           NUMBER,
                                            INSERT_TIME      DATE
                                            )
                                            TABLESPACE DEFAULT_TS
                                            PCTUSED    0
                                            PCTFREE    10
                                            INITRANS   1
                                            MAXTRANS   255
                                            STORAGE    (
                                                        INITIAL          64K
                                                        NEXT             1M
                                                        MINEXTENTS       1
                                                        MAXEXTENTS       UNLIMITED
                                                        PCTINCREASE      0
                                                        BUFFER_POOL      DEFAULT
                                                       )
                                            LOGGING 
                                            NOCOMPRESS 
                                            NOCACHE
                                            MONITORING", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTSEGSTATOBJ(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, TS#, OBJ#, DATAOBJ#, CON_DBID, 
                                    INSERT_TIME)
                                    LOGGING
                                    TABLESPACE DEFAULT_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )", arguments.DataTableName, arguments.DataTablePKName);




                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTSEGSTATOBJ(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, TS#, OBJ#, DATAOBJ#, CON_DBID, INSERT_TIME)
                                      USING INDEX {1}
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_SEG_STAT
        public void CreateRAWDBAHISTSEGSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                    (
                                          SNAP_ID                         NUMBER,
                                          DBID                            NUMBER,
                                          INSTANCE_NUMBER                 NUMBER,
                                          TS#                             NUMBER,
                                          OBJ#                            NUMBER,
                                          DATAOBJ#                        NUMBER,
                                          LOGICAL_READS_TOTAL             NUMBER,
                                          LOGICAL_READS_DELTA             NUMBER,
                                          BUFFER_BUSY_WAITS_TOTAL         NUMBER,
                                          BUFFER_BUSY_WAITS_DELTA         NUMBER,
                                          DB_BLOCK_CHANGES_TOTAL          NUMBER,
                                          DB_BLOCK_CHANGES_DELTA          NUMBER,
                                          PHYSICAL_READS_TOTAL            NUMBER,
                                          PHYSICAL_READS_DELTA            NUMBER,
                                          PHYSICAL_WRITES_TOTAL           NUMBER,
                                          PHYSICAL_WRITES_DELTA           NUMBER,
                                          PHYSICAL_READS_DIRECT_TOTAL     NUMBER,
                                          PHYSICAL_READS_DIRECT_DELTA     NUMBER,
                                          PHYSICAL_WRITES_DIRECT_TOTAL    NUMBER,
                                          PHYSICAL_WRITES_DIRECT_DELTA    NUMBER,
                                          ITL_WAITS_TOTAL                 NUMBER,
                                          ITL_WAITS_DELTA                 NUMBER,
                                          ROW_LOCK_WAITS_TOTAL            NUMBER,
                                          ROW_LOCK_WAITS_DELTA            NUMBER,
                                          GC_CR_BLOCKS_SERVED_TOTAL       NUMBER,
                                          GC_CR_BLOCKS_SERVED_DELTA       NUMBER,
                                          GC_CU_BLOCKS_SERVED_TOTAL       NUMBER,
                                          GC_CU_BLOCKS_SERVED_DELTA       NUMBER,
                                          GC_BUFFER_BUSY_TOTAL            NUMBER,
                                          GC_BUFFER_BUSY_DELTA            NUMBER,
                                          GC_CR_BLOCKS_RECEIVED_TOTAL     NUMBER,
                                          GC_CR_BLOCKS_RECEIVED_DELTA     NUMBER,
                                          GC_CU_BLOCKS_RECEIVED_TOTAL     NUMBER,
                                          GC_CU_BLOCKS_RECEIVED_DELTA     NUMBER,
                                          GC_REMOTE_GRANTS_TOTAL          NUMBER,
                                          GC_REMOTE_GRANTS_DELTA          NUMBER,
                                          SPACE_USED_TOTAL                NUMBER,
                                          SPACE_USED_DELTA                NUMBER,
                                          SPACE_ALLOCATED_TOTAL           NUMBER,
                                          SPACE_ALLOCATED_DELTA           NUMBER,
                                          TABLE_SCANS_TOTAL               NUMBER,
                                          TABLE_SCANS_DELTA               NUMBER,
                                          CHAIN_ROW_EXCESS_TOTAL          NUMBER,
                                          CHAIN_ROW_EXCESS_DELTA          NUMBER,
                                          PHYSICAL_READ_REQUESTS_TOTAL    NUMBER,
                                          PHYSICAL_READ_REQUESTS_DELTA    NUMBER,
                                          PHYSICAL_WRITE_REQUESTS_TOTAL   NUMBER,
                                          PHYSICAL_WRITE_REQUESTS_DELTA   NUMBER,
                                          OPTIMIZED_PHYSICAL_READS_TOTAL  NUMBER,
                                          OPTIMIZED_PHYSICAL_READS_DELTA  NUMBER,
                                          IM_SCANS_TOTAL                  NUMBER,
                                          IM_SCANS_DELTA                  NUMBER,
                                          POPULATE_CUS_TOTAL              NUMBER,
                                          POPULATE_CUS_DELTA              NUMBER,
                                          REPOPULATE_CUS_TOTAL            NUMBER,
                                          REPOPULATE_CUS_DELTA            NUMBER,
                                          IM_DB_BLOCK_CHANGES_TOTAL       NUMBER,
                                          IM_DB_BLOCK_CHANGES_DELTA       NUMBER,
                                          IM_MEMBYTES                     NUMBER,
                                          CON_DBID                        NUMBER,
                                          CON_ID                          NUMBER,
                                          BEGIN_TIME                      DATE,
                                          END_TIME                        DATE
                                        )
                                        NOCOMPRESS 
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                   )
                                        PARTITION BY RANGE (BEGIN_TIME)
                                        INTERVAL( NUMTOYMINTERVAL(1, 'MONTH'))
                                        (  ", arguments.DataTableName);

                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION SEG_STAT_{0}_{1} VALUES LESS THAN (TO_DATE(' {2}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))
                                            LOGGING
                                            NOCOMPRESS 
                                            TABLESPACE PART_TS_DATA
                                            PCTFREE    10
                                            INITRANS   1
                                            MAXTRANS   255
                                            STORAGE    (
                                                        INITIAL          64K
                                                        NEXT             1M
                                                        MINEXTENTS       1
                                                        MAXEXTENTS       UNLIMITED
                                                        BUFFER_POOL      DEFAULT
                                                       ),", arguments.DBLinkName, res, formatDateTime);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }
                tmpSql.Append(@"  )
                                NOCACHE
                                MONITORING");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTSEGSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, TS#, OBJ#, 
                                    DATAOBJ#, CON_DBID, BEGIN_TIME, END_TIME)
                                      TABLESPACE PART_TS_IDX
                                      PCTFREE    10
                                      INITRANS   2
                                      MAXTRANS   255
                                      STORAGE    (
                                                  BUFFER_POOL      DEFAULT
                                                 )
                                    LOCAL (  
                                    ", arguments.DataTableName, arguments.DataTablePKName);


                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    //string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION SEG_STAT_{0}_{1}
                                            LOGGING
                                            NOCOMPRESS 
                                            TABLESPACE PART_TS_IDX
                                            INITRANS   2
                                            MAXTRANS   255
                                            STORAGE    (
                                                        INITIAL          64K
                                                        NEXT             1M
                                                        MINEXTENTS       1
                                                        MAXEXTENTS       UNLIMITED
                                                        BUFFER_POOL      DEFAULT
                                                       ),", arguments.DBLinkName, res);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }

                tmpSql.Append("    )");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTSEGSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, TS#, OBJ#, DATAOBJ#, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_SGASTAT
        public void CreateRAWDBAHISTSGASTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                    (
                                            SNAP_ID          NUMBER,
                                            DBID             NUMBER,
                                            INSTANCE_NUMBER  NUMBER,
                                            NAME             VARCHAR2(64 BYTE),
                                            POOL             VARCHAR2(30 BYTE),
                                            BYTES            NUMBER,
                                            CON_DBID         NUMBER,
                                            CON_ID           NUMBER,
                                            BEGIN_TIME       DATE,
                                            END_TIME         DATE
                                        )
                                        NOCOMPRESS 
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        PARTITION BY RANGE (BEGIN_TIME)
                                        INTERVAL( NUMTOYMINTERVAL(1, 'MONTH'))
                                        (  ", arguments.DataTableName);

                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION SGASTAT_{0}_{1} VALUES LESS THAN (TO_DATE(' {2}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_DATA
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                    ),", arguments.DBLinkName, res, formatDateTime);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }
                tmpSql.Append(@"  )
                                NOCACHE
                                MONITORING");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTSGASTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, NAME, POOL, 
                                    CON_DBID, BEGIN_TIME, END_TIME)
                                    TABLESPACE PART_TS_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                BUFFER_POOL      DEFAULT
                                                 )
                                    LOCAL ( 
                                    ", arguments.DataTableName, arguments.DataTablePKName);


                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    //string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION SGASTAT_{0}_{1}
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_IDX
                                        INITRANS   2
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                    ),", arguments.DBLinkName, res);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }

                tmpSql.Append("    )");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTSGASTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, NAME, POOL, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_SGA
        public void CreateRAWDBAHISTSGA(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                        (
                                            SNAP_ID          NUMBER,
                                            DBID             NUMBER,
                                            INSTANCE_NUMBER  NUMBER,
                                            NAME             VARCHAR2(64 BYTE),
                                            VALUE            NUMBER,
                                            CON_DBID         NUMBER,
                                            CON_ID           NUMBER,
                                            BEGIN_TIME       DATE,
                                            END_TIME         DATE
                                        )
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        LOGGING 
                                        NOCOMPRESS 
                                        NOCACHE
                                        MONITORING", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTSGA(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, NAME, CON_DBID, 
                                    BEGIN_TIME, END_TIME)
                                    LOGGING
                                    TABLESPACE DEFAULT_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )", arguments.DataTableName, arguments.DataTablePKName);




                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTSGA(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, NAME, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX {1}
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_SNAPSHOT
        public void CreateRAWDBAHISTSNAPSHOT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                        (
                                            SNAP_ID                 NUMBER,
                                            DBID                    NUMBER,
                                            INSTANCE_NUMBER         NUMBER,
                                            STARTUP_TIME            TIMESTAMP(3),
                                            BEGIN_INTERVAL_TIME     TIMESTAMP(3),
                                            END_INTERVAL_TIME       TIMESTAMP(3),
                                            FLUSH_ELAPSED           INTERVAL DAY(5) TO SECOND(1),
                                            SNAP_LEVEL              NUMBER,
                                            ERROR_COUNT             NUMBER,
                                            SNAP_FLAG               NUMBER,
                                            SNAP_TIMEZONE           INTERVAL DAY(0) TO SECOND(0),
                                            BEGIN_INTERVAL_TIME_TZ  TIMESTAMP(3) WITH TIME ZONE,
                                            END_INTERVAL_TIME_TZ    TIMESTAMP(3) WITH TIME ZONE,
                                            CON_ID                  NUMBER
                                        )
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        LOGGING 
                                        NOCOMPRESS 
                                        NOCACHE
                                        MONITORING", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTSNAPSHOT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER)
                                    LOGGING
                                    TABLESPACE DEFAULT_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )", arguments.DataTableName, arguments.DataTablePKName);




                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTSNAPSHOT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER)
                                      USING INDEX {1}
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_SQLBIND
        public void CreateRAWDBAHISTSQLBIND(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                        (
                                            SNAP_ID          NUMBER,
                                            DBID             NUMBER,
                                            INSTANCE_NUMBER  NUMBER,
                                            SQL_ID           VARCHAR2(13 BYTE),
                                            NAME             VARCHAR2(128 BYTE),
                                            POSITION         NUMBER,
                                            DUP_POSITION     NUMBER,
                                            DATATYPE         NUMBER,
                                            DATATYPE_STRING  VARCHAR2(15 BYTE),
                                            CHARACTER_SID    NUMBER,
                                            PRECISION        NUMBER,
                                            SCALE            NUMBER,
                                            MAX_LENGTH       NUMBER,
                                            WAS_CAPTURED     VARCHAR2(3 BYTE),
                                            LAST_CAPTURED    DATE,
                                            VALUE_STRING     VARCHAR2(4000 BYTE),
                                            CON_DBID         NUMBER,
                                            CON_ID           NUMBER,
                                            BEGIN_TIME       DATE,
                                            END_TIME         DATE
                                        )
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        LOGGING 
                                        NOCOMPRESS 
                                        NOCACHE
                                        MONITORING", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTSQLBIND(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, SQL_ID, POSITION, 
                                    CON_DBID, BEGIN_TIME, END_TIME)
                                    LOGGING
                                    TABLESPACE DEFAULT_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )", arguments.DataTableName, arguments.DataTablePKName);




                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTSQLBIND(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, SQL_ID, POSITION, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX {1}
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_SQLSTAT
        public void CreateRAWDBAHISTSQLSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                    (
                                            SNAP_ID                         NUMBER,
                                            DBID                            NUMBER,
                                            INSTANCE_NUMBER                 NUMBER,
                                            SQL_ID                          VARCHAR2(13 BYTE),
                                            PLAN_HASH_VALUE                 NUMBER,
                                            OPTIMIZER_COST                  NUMBER,
                                            OPTIMIZER_MODE                  VARCHAR2(10 BYTE),
                                            OPTIMIZER_ENV_HASH_VALUE        NUMBER,
                                            SHARABLE_MEM                    NUMBER,
                                            LOADED_VERSIONS                 NUMBER,
                                            VERSION_COUNT                   NUMBER,
                                            MODULE                          VARCHAR2(64 BYTE),
                                            ACTION                          VARCHAR2(64 BYTE),
                                            SQL_PROFILE                     VARCHAR2(64 BYTE),
                                            FORCE_MATCHING_SIGNATURE        NUMBER,
                                            PARSING_SCHEMA_ID               NUMBER,
                                            PARSING_SCHEMA_NAME             VARCHAR2(128 BYTE),
                                            PARSING_USER_ID                 NUMBER,
                                            FETCHES_TOTAL                   NUMBER,
                                            FETCHES_DELTA                   NUMBER,
                                            END_OF_FETCH_COUNT_TOTAL        NUMBER,
                                            END_OF_FETCH_COUNT_DELTA        NUMBER,
                                            SORTS_TOTAL                     NUMBER,
                                            SORTS_DELTA                     NUMBER,
                                            EXECUTIONS_TOTAL                NUMBER,
                                            EXECUTIONS_DELTA                NUMBER,
                                            PX_SERVERS_EXECS_TOTAL          NUMBER,
                                            PX_SERVERS_EXECS_DELTA          NUMBER,
                                            LOADS_TOTAL                     NUMBER,
                                            LOADS_DELTA                     NUMBER,
                                            INVALIDATIONS_TOTAL             NUMBER,
                                            INVALIDATIONS_DELTA             NUMBER,
                                            PARSE_CALLS_TOTAL               NUMBER,
                                            PARSE_CALLS_DELTA               NUMBER,
                                            DISK_READS_TOTAL                NUMBER,
                                            DISK_READS_DELTA                NUMBER,
                                            BUFFER_GETS_TOTAL               NUMBER,
                                            BUFFER_GETS_DELTA               NUMBER,
                                            ROWS_PROCESSED_TOTAL            NUMBER,
                                            ROWS_PROCESSED_DELTA            NUMBER,
                                            CPU_TIME_TOTAL                  NUMBER,
                                            CPU_TIME_DELTA                  NUMBER,
                                            ELAPSED_TIME_TOTAL              NUMBER,
                                            ELAPSED_TIME_DELTA              NUMBER,
                                            IOWAIT_TOTAL                    NUMBER,
                                            IOWAIT_DELTA                    NUMBER,
                                            CLWAIT_TOTAL                    NUMBER,
                                            CLWAIT_DELTA                    NUMBER,
                                            APWAIT_TOTAL                    NUMBER,
                                            APWAIT_DELTA                    NUMBER,
                                            CCWAIT_TOTAL                    NUMBER,
                                            CCWAIT_DELTA                    NUMBER,
                                            DIRECT_WRITES_TOTAL             NUMBER,
                                            DIRECT_WRITES_DELTA             NUMBER,
                                            PLSEXEC_TIME_TOTAL              NUMBER,
                                            PLSEXEC_TIME_DELTA              NUMBER,
                                            JAVEXEC_TIME_TOTAL              NUMBER,
                                            JAVEXEC_TIME_DELTA              NUMBER,
                                            IO_OFFLOAD_ELIG_BYTES_TOTAL     NUMBER,
                                            IO_OFFLOAD_ELIG_BYTES_DELTA     NUMBER,
                                            IO_INTERCONNECT_BYTES_TOTAL     NUMBER,
                                            IO_INTERCONNECT_BYTES_DELTA     NUMBER,
                                            PHYSICAL_READ_REQUESTS_TOTAL    NUMBER,
                                            PHYSICAL_READ_REQUESTS_DELTA    NUMBER,
                                            PHYSICAL_READ_BYTES_TOTAL       NUMBER,
                                            PHYSICAL_READ_BYTES_DELTA       NUMBER,
                                            PHYSICAL_WRITE_REQUESTS_TOTAL   NUMBER,
                                            PHYSICAL_WRITE_REQUESTS_DELTA   NUMBER,
                                            PHYSICAL_WRITE_BYTES_TOTAL      NUMBER,
                                            PHYSICAL_WRITE_BYTES_DELTA      NUMBER,
                                            OPTIMIZED_PHYSICAL_READS_TOTAL  NUMBER,
                                            OPTIMIZED_PHYSICAL_READS_DELTA  NUMBER,
                                            CELL_UNCOMPRESSED_BYTES_TOTAL   NUMBER,
                                            CELL_UNCOMPRESSED_BYTES_DELTA   NUMBER,
                                            IO_OFFLOAD_RETURN_BYTES_TOTAL   NUMBER,
                                            IO_OFFLOAD_RETURN_BYTES_DELTA   NUMBER,
                                            BIND_DATA                       RAW(2000),
                                            FLAG                            NUMBER,
                                            OBSOLETE_COUNT                  NUMBER,
                                            CON_DBID                        NUMBER,
                                            CON_ID                          NUMBER,
                                            BEGIN_TIME                      DATE,
                                            END_TIME                        DATE
                                        )
                                        NOCOMPRESS 
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        PARTITION BY RANGE (BEGIN_TIME)
                                        INTERVAL( NUMTOYMINTERVAL(1, 'MONTH'))
                                        (  ", arguments.DataTableName);

                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION SQLSTAT_{0}_{1} VALUES LESS THAN (TO_DATE(' {2}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_DATA
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res, formatDateTime);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }
                tmpSql.Append(@"  )
                                NOCACHE
                                MONITORING");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTSQLSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE  INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, SQL_ID, PLAN_HASH_VALUE, 
                                    CON_DBID, BEGIN_TIME, END_TIME)
                                      TABLESPACE PART_TS_IDX
                                      PCTFREE    10
                                      INITRANS   2
                                      MAXTRANS   255
                                      STORAGE    (
                                                  BUFFER_POOL      DEFAULT
                                                 )
                                    LOCAL (  
                                    ", arguments.DataTableName, arguments.DataTablePKName);


                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    //string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION SQLSTAT_{0}_{1}
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_IDX
                                        INITRANS   2
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }

                tmpSql.Append("    )");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTSQLSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, SQL_ID, PLAN_HASH_VALUE, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_SQLTEXT
        public void CreateRAWDBAHISTSQLTEXT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                    (
                                            DBID          NUMBER,
                                            SQL_ID        VARCHAR2(13 BYTE),
                                            SQL_TEXT      CLOB,
                                            COMMAND_TYPE  NUMBER,
                                            CON_DBID      NUMBER,
                                            CON_ID        NUMBER,
                                            INSERT_TIME   DATE
                                        )
                                        LOB (SQL_TEXT) STORE AS SECUREFILE (
                                            TABLESPACE  PART_TS_DATA
                                            ENABLE      STORAGE IN ROW
                                            CHUNK       8192
                                            NOCACHE
                                            LOGGING)
                                        NOCOMPRESS 
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        PARTITION BY RANGE (INSERT_TIME)
                                        INTERVAL( NUMTOYMINTERVAL(1, 'MONTH'))
                                        (  ", arguments.DataTableName);

                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION SQLTEXT_{0}_{1} VALUES LESS THAN (TO_DATE(' {2}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_DATA
                                        LOB (SQL_TEXT) STORE AS SECUREFILE ( 
                                            TABLESPACE  PART_TS_DATA
                                            ENABLE      STORAGE IN ROW
                                            CHUNK       8192
                                            RETENTION
                                            NOCACHE
                                            LOGGING
                                                STORAGE    (
                                                            INITIAL          104K
                                                            NEXT             1M
                                                            MINEXTENTS       1
                                                            MAXEXTENTS       UNLIMITED
                                                            PCTINCREASE      0
                                                            BUFFER_POOL      DEFAULT
                                                            ))
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                    ),", arguments.DBLinkName, res, formatDateTime);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }
                tmpSql.Append(@"  )
                                NOCACHE
                                MONITORING");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTSQLTEXT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SQL_ID, CON_DBID, INSERT_TIME)
                                    TABLESPACE PART_TS_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                BUFFER_POOL      DEFAULT
                                                )
                                LOCAL (  
                                    ", arguments.DataTableName, arguments.DataTablePKName);


                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    //string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION SQLTEXT_{0}_{1}
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_IDX
                                        INITRANS   2
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }

                tmpSql.Append("    )");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTSQLTEXT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SQL_ID, CON_DBID, INSERT_TIME)
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_SQL_BIND_METADATA
        public void CreateRAWDBAHISTSQLBINDMETADATA(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                        (
                                            DBID             NUMBER                       NOT NULL,
                                            SQL_ID           VARCHAR2(13 BYTE)            NOT NULL,
                                            NAME             VARCHAR2(128 BYTE),
                                            POSITION         NUMBER                       NOT NULL,
                                            DUP_POSITION     NUMBER,
                                            DATATYPE         NUMBER,
                                            DATATYPE_STRING  VARCHAR2(15 BYTE),
                                            CHARACTER_SID    NUMBER,
                                            PRECISION        NUMBER,
                                            SCALE            NUMBER,
                                            MAX_LENGTH       NUMBER,
                                            CON_DBID         NUMBER,
                                            CON_ID           NUMBER,
                                            INSERT_TIME      DATE
                                        )
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        LOGGING 
                                        NOCOMPRESS 
                                        NOCACHE
                                        MONITORING", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTSQLBINDMETADATA(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SQL_ID, POSITION, CON_DBID, INSERT_TIME)
                                    LOGGING
                                    TABLESPACE DEFAULT_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )", arguments.DataTableName, arguments.DataTablePKName);




                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTSQLBINDMETADATA(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SQL_ID, POSITION, CON_DBID, INSERT_TIME)
                                      USING INDEX {1}
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_SQL_PLAN
        public void CreateRAWDBAHISTSQLPLAN(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                    (
                                            DBID               NUMBER                     NOT NULL,
                                            SQL_ID             VARCHAR2(13 BYTE)          NOT NULL,
                                            PLAN_HASH_VALUE    NUMBER                     NOT NULL,
                                            ID                 NUMBER                     NOT NULL,
                                            OPERATION          VARCHAR2(30 BYTE),
                                            OPTIONS            VARCHAR2(30 BYTE),
                                            OBJECT_NODE        VARCHAR2(128 BYTE),
                                            OBJECT#            NUMBER,
                                            OBJECT_OWNER       VARCHAR2(128 BYTE),
                                            OBJECT_NAME        VARCHAR2(128 BYTE),
                                            OBJECT_ALIAS       VARCHAR2(261 BYTE),
                                            OBJECT_TYPE        VARCHAR2(20 BYTE),
                                            OPTIMIZER          VARCHAR2(20 BYTE),
                                            PARENT_ID          NUMBER,
                                            DEPTH              NUMBER,
                                            POSITION           NUMBER,
                                            SEARCH_COLUMNS     NUMBER,
                                            COST               NUMBER,
                                            CARDINALITY        NUMBER,
                                            BYTES              NUMBER,
                                            OTHER_TAG          VARCHAR2(35 BYTE),
                                            PARTITION_START    VARCHAR2(64 BYTE),
                                            PARTITION_STOP     VARCHAR2(64 BYTE),
                                            PARTITION_ID       NUMBER,
                                            OTHER              VARCHAR2(4000 BYTE),
                                            DISTRIBUTION       VARCHAR2(20 BYTE),
                                            CPU_COST           NUMBER,
                                            IO_COST            NUMBER,
                                            TEMP_SPACE         NUMBER,
                                            ACCESS_PREDICATES  VARCHAR2(4000 BYTE),
                                            FILTER_PREDICATES  VARCHAR2(4000 BYTE),
                                            PROJECTION         VARCHAR2(4000 BYTE),
                                            TIME               NUMBER,
                                            QBLOCK_NAME        VARCHAR2(128 BYTE),
                                            REMARKS            VARCHAR2(4000 BYTE),
                                            TIMESTAMP          DATE,
                                            OTHER_XML          CLOB,
                                            CON_DBID           NUMBER,
                                            CON_ID             NUMBER,
                                            INSERT_TIME        DATE
                                        )
                                        LOB (OTHER_XML) STORE AS SECUREFILE (
                                            TABLESPACE  PART_TS_DATA
                                            ENABLE      STORAGE IN ROW
                                            CHUNK       8192
                                            NOCACHE
                                            LOGGING)
                                        NOCOMPRESS 
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        PARTITION BY RANGE (INSERT_TIME)
                                        INTERVAL( NUMTOYMINTERVAL(1, 'MONTH'))
                                        (  ", arguments.DataTableName);

                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION SQL_PLAN_{0}_{1} VALUES LESS THAN (TO_DATE(' {2}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_DATA
                                        LOB (OTHER_XML) STORE AS SECUREFILE (
                                          TABLESPACE  PART_TS_DATA
                                          ENABLE      STORAGE IN ROW
                                          CHUNK       8192
                                          RETENTION
                                          NOCACHE
                                          LOGGING
                                              STORAGE    (
                                                          INITIAL          104K
                                                          NEXT             1M
                                                          MINEXTENTS       1
                                                          MAXEXTENTS       UNLIMITED
                                                          PCTINCREASE      0
                                                          BUFFER_POOL      DEFAULT
                                                         ))
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res, formatDateTime);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }
                tmpSql.Append(@"  )
                                NOCACHE
                                MONITORING");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTSQLPLAN(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SQL_ID, PLAN_HASH_VALUE, ID, CON_DBID, 
                                    INSERT_TIME)
                                      TABLESPACE PART_TS_IDX
                                      PCTFREE    10
                                      INITRANS   2
                                      MAXTRANS   255
                                      STORAGE    (
                                                  BUFFER_POOL      DEFAULT
                                                 )
                                    LOCAL (  
                                    ", arguments.DataTableName, arguments.DataTablePKName);


                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    //string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION SQL_PLAN_{0}_{1}
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_IDX
                                        INITRANS   2
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }

                tmpSql.Append("    )");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTSQLPLAN(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SQL_ID, PLAN_HASH_VALUE, ID, CON_DBID, INSERT_TIME)
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_SQL_SUMMARY
        public void CreateRAWDBAHISTSQLSUMMARY(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                        (
                                            SNAP_ID             NUMBER,
                                            DBID                NUMBER,
                                            INSTANCE_NUMBER     NUMBER,
                                            TOTAL_SQL           NUMBER,
                                            TOTAL_SQL_MEM       NUMBER,
                                            SINGLE_USE_SQL      NUMBER,
                                            SINGLE_USE_SQL_MEM  NUMBER,
                                            CON_DBID            NUMBER,
                                            CON_ID              NUMBER,
                                            BEGIN_TIME          DATE,
                                            END_TIME            DATE
                                        )
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        LOGGING 
                                        NOCOMPRESS 
                                        NOCACHE
                                        MONITORING", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTSQLSUMMARY(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, CON_DBID, BEGIN_TIME, 
                                    END_TIME)
                                    LOGGING
                                    TABLESPACE DEFAULT_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )", arguments.DataTableName, arguments.DataTablePKName);




                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTSQLSUMMARY(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX {1}
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_SQL_WORKAREA_HSTGRM
        public void CreateRAWDBAHISTSQLWORKAREAHSTGRM(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                        (
                                            SNAP_ID                 NUMBER,
                                            DBID                    NUMBER,
                                            INSTANCE_NUMBER         NUMBER,
                                            LOW_OPTIMAL_SIZE        NUMBER,
                                            HIGH_OPTIMAL_SIZE       NUMBER,
                                            OPTIMAL_EXECUTIONS      NUMBER,
                                            ONEPASS_EXECUTIONS      NUMBER,
                                            MULTIPASSES_EXECUTIONS  NUMBER,
                                            TOTAL_EXECUTIONS        NUMBER,
                                            CON_DBID                NUMBER,
                                            CON_ID                  NUMBER,
                                            BEGIN_TIME              DATE,
                                            END_TIME                DATE
                                        )
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        LOGGING 
                                        NOCOMPRESS 
                                        NOCACHE
                                        MONITORING", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTSQLWORKAREAHSTGRM(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, LOW_OPTIMAL_SIZE, HIGH_OPTIMAL_SIZE, 
                                    CON_DBID, BEGIN_TIME, END_TIME)
                                    LOGGING
                                    TABLESPACE DEFAULT_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )", arguments.DataTableName, arguments.DataTablePKName);




                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTSQLWORKAREAHSTGRM(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, LOW_OPTIMAL_SIZE, HIGH_OPTIMAL_SIZE, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX {1}
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_SYSMETRIC_SUMMARY
        public void CreateRAWDBAHISTSYSMETRICSUMMARY(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                        (
                                            SNAP_ID             NUMBER,
                                            DBID                NUMBER,
                                            INSTANCE_NUMBER     NUMBER,
                                            BEGIN_TIME          DATE,
                                            END_TIME            DATE,
                                            INTSIZE             NUMBER,
                                            GROUP_ID            NUMBER,
                                            METRIC_ID           NUMBER,
                                            METRIC_NAME         VARCHAR2(64 BYTE),
                                            METRIC_UNIT         VARCHAR2(64 BYTE),
                                            NUM_INTERVAL        NUMBER,
                                            MINVAL              NUMBER,
                                            MAXVAL              NUMBER,
                                            AVERAGE             NUMBER,
                                            STANDARD_DEVIATION  NUMBER,
                                            SUM_SQUARES         NUMBER,
                                            CON_DBID            NUMBER,
                                            CON_ID              NUMBER
                                        )
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        LOGGING 
                                        NOCOMPRESS 
                                        NOCACHE
                                        MONITORING", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTSYSMETRICSUMMARY(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, GROUP_ID, METRIC_ID, 
                                    CON_DBID, BEGIN_TIME, END_TIME)
                                    LOGGING
                                    TABLESPACE DEFAULT_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )", arguments.DataTableName, arguments.DataTablePKName);




                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTSYSMETRICSUMMARY(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, GROUP_ID, METRIC_ID, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX {1}
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_SYSSTAT
        public void CreateRAWDBAHISTSYSSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                    (
                                            SNAP_ID          NUMBER,
                                            DBID             NUMBER,
                                            INSTANCE_NUMBER  NUMBER,
                                            STAT_ID          NUMBER,
                                            STAT_NAME        VARCHAR2(64 BYTE),
                                            VALUE            NUMBER,
                                            CON_DBID         NUMBER,
                                            CON_ID           NUMBER,
                                            BEGIN_TIME       DATE,
                                            END_TIME         DATE
                                        )
                                        NOCOMPRESS 
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        PARTITION BY RANGE (BEGIN_TIME)
                                        INTERVAL( NUMTOYMINTERVAL(1, 'MONTH'))
                                        (  ", arguments.DataTableName);

                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION SYSSTAT_{0}_{1} VALUES LESS THAN (TO_DATE(' {2}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_DATA
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res, formatDateTime);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }
                tmpSql.Append(@"  )
                                NOCACHE
                                MONITORING");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTSYSSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, STAT_ID, CON_DBID, 
                                    BEGIN_TIME, END_TIME)
                                      TABLESPACE PART_TS_IDX
                                      PCTFREE    10
                                      INITRANS   2
                                      MAXTRANS   255
                                      STORAGE    (
                                                  BUFFER_POOL      DEFAULT
                                                 )
                                    LOCAL (  
                                    ", arguments.DataTableName, arguments.DataTablePKName);


                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    //string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION SYSSTAT_{0}_{1}
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_IDX
                                        INITRANS   2
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }

                tmpSql.Append("    )");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTSYSSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, STAT_ID, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_SYSTEM_EVENT
        public void CreateRAWDBAHISTSYSTEMEVENT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                    (
                                            SNAP_ID               NUMBER,
                                            DBID                  NUMBER,
                                            INSTANCE_NUMBER       NUMBER,
                                            EVENT_ID              NUMBER,
                                            EVENT_NAME            VARCHAR2(64 BYTE),
                                            WAIT_CLASS_ID         NUMBER,
                                            WAIT_CLASS            VARCHAR2(64 BYTE),
                                            TOTAL_WAITS           NUMBER,
                                            TOTAL_TIMEOUTS        NUMBER,
                                            TIME_WAITED_MICRO     NUMBER,
                                            TOTAL_WAITS_FG        NUMBER,
                                            TOTAL_TIMEOUTS_FG     NUMBER,
                                            TIME_WAITED_MICRO_FG  NUMBER,
                                            CON_DBID              NUMBER,
                                            CON_ID                NUMBER,
                                            BEGIN_TIME            DATE,
                                            END_TIME              DATE
                                        )
                                        NOCOMPRESS 
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        PARTITION BY RANGE (BEGIN_TIME)
                                        INTERVAL( NUMTOYMINTERVAL(1, 'MONTH'))
                                        (  ", arguments.DataTableName);

                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION SYSTEM_EVENT_{0}_{1} VALUES LESS THAN (TO_DATE(' {2}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_DATA
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res, formatDateTime);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }
                tmpSql.Append(@"  )
                                NOCACHE
                                MONITORING");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTSYSTEMEVENT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, EVENT_ID, CON_DBID, 
                                    BEGIN_TIME, END_TIME)
                                      TABLESPACE PART_TS_IDX
                                      PCTFREE    10
                                      INITRANS   2
                                      MAXTRANS   255
                                      STORAGE    (
                                                  BUFFER_POOL      DEFAULT
                                                 )
                                    LOCAL (  
                                    ", arguments.DataTableName, arguments.DataTablePKName);


                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    //string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION SYSTEM_EVENT_{0}_{1}
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_IDX
                                        INITRANS   2
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }

                tmpSql.Append("    )");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTSYSTEMEVENT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, EVENT_ID, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_SYS_TIME_MODEL
        public void CreateRAWDBAHISTSYSTIMEMODEL(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                    (
                                            SNAP_ID          NUMBER,
                                            DBID             NUMBER,
                                            INSTANCE_NUMBER  NUMBER,
                                            STAT_ID          NUMBER,
                                            STAT_NAME        VARCHAR2(64 BYTE),
                                            VALUE            NUMBER,
                                            CON_DBID         NUMBER,
                                            CON_ID           NUMBER,
                                            BEGIN_TIME       DATE,
                                            END_TIME         DATE
                                        )
                                        NOCOMPRESS 
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        PARTITION BY RANGE (BEGIN_TIME)
                                        INTERVAL( NUMTOYMINTERVAL(1, 'MONTH'))
                                        (  ", arguments.DataTableName);

                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION SYS_TIME_{0}_{1} VALUES LESS THAN (TO_DATE(' {2}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_DATA
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res, formatDateTime);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }
                tmpSql.Append(@"  )
                                NOCACHE
                                MONITORING");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTSYSTIMEMODEL(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, STAT_ID, CON_DBID, 
                                    BEGIN_TIME, END_TIME)
                                      TABLESPACE PART_TS_IDX
                                      PCTFREE    10
                                      INITRANS   2
                                      MAXTRANS   255
                                      STORAGE    (
                                                  BUFFER_POOL      DEFAULT
                                                 )
                                    LOCAL (  
                                    ", arguments.DataTableName, arguments.DataTablePKName);


                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    //string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION SYS_TIME_{0}_{1}
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_IDX
                                        INITRANS   2
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }

                tmpSql.Append("    )");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTSYSTIMEMODEL(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, STAT_ID, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_THREAD
        public void CreateRAWDBAHISTTHREAD(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                        (
                                            SNAP_ID                 NUMBER,
                                            DBID                    NUMBER,
                                            INSTANCE_NUMBER         NUMBER,
                                            THREAD#                 NUMBER,
                                            THREAD_INSTANCE_NUMBER  NUMBER,
                                            STATUS                  VARCHAR2(6 BYTE),
                                            OPEN_TIME               DATE,
                                            CURRENT_GROUP#          NUMBER,
                                            SEQUENCE#               NUMBER,
                                            CON_DBID                NUMBER,
                                            CON_ID                  NUMBER,
                                            BEGIN_TIME              DATE,
                                            END_TIME                DATE
                                        )
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        LOGGING 
                                        NOCOMPRESS 
                                        NOCACHE
                                        MONITORING", arguments.DataTableName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTTHREAD(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, THREAD#, CON_DBID, 
                                    BEGIN_TIME, END_TIME)
                                    LOGGING
                                    TABLESPACE DEFAULT_IDX
                                    PCTFREE    10
                                    INITRANS   2
                                    MAXTRANS   255
                                    STORAGE    (
                                                INITIAL          64K
                                                NEXT             1M
                                                MINEXTENTS       1
                                                MAXEXTENTS       UNLIMITED
                                                PCTINCREASE      0
                                                BUFFER_POOL      DEFAULT
                                               )", arguments.DataTableName, arguments.DataTablePKName);




                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTTHREAD(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, THREAD#, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX {1}
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        #region RAW_DBA_HIST_WAITSTAT
        public void CreateRAWDBAHISTWAITSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE TABLE {0}
                                    (
                                            SNAP_ID          NUMBER,
                                            DBID             NUMBER,
                                            INSTANCE_NUMBER  NUMBER,
                                            CLASS            VARCHAR2(18 BYTE),
                                            WAIT_COUNT       NUMBER,
                                            TIME             NUMBER,
                                            CON_DBID         NUMBER,
                                            CON_ID           NUMBER,
                                            BEGIN_TIME       DATE,
                                            END_TIME         DATE
                                        )
                                        NOCOMPRESS 
                                        TABLESPACE DEFAULT_TS
                                        PCTUSED    0
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    PCTINCREASE      0
                                                    BUFFER_POOL      DEFAULT
                                                    )
                                        PARTITION BY RANGE (BEGIN_TIME)
                                        INTERVAL( NUMTOYMINTERVAL(1, 'MONTH'))
                                        (  ", arguments.DataTableName);

                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION WAITSTAT_{0}_{1} VALUES LESS THAN (TO_DATE(' {2}', 'SYYYY-MM-DD HH24:MI:SS', 'NLS_CALENDAR=GREGORIAN'))
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_DATA
                                        PCTFREE    10
                                        INITRANS   1
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                    ),", arguments.DBLinkName, res, formatDateTime);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }
                tmpSql.Append(@"  )
                                NOCACHE
                                MONITORING");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreateIndexRAWDBAHISTWAITSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"CREATE UNIQUE INDEX {1} ON {0}
                                    (DBID, SNAP_ID, INSTANCE_NUMBER, CLASS, CON_DBID, 
                                    BEGIN_TIME, END_TIME)
                                      TABLESPACE PART_TS_IDX
                                      PCTFREE    10
                                      INITRANS   2
                                      MAXTRANS   255
                                      STORAGE    (
                                                  BUFFER_POOL      DEFAULT
                                                 )
                                    LOCAL (  
                                    ", arguments.DataTableName, arguments.DataTablePKName);


                DateTime dateTime = arguments.DateNow;//获取当前时间 格式为2023/07/01 00:00:00

                for (int i = 0; i < 12; i++)
                {
                    DateTime AdddateTime = dateTime.AddMonths(i);
                    //string formatDateTime = AdddateTime.ToString("yyyy-MM-dd HH:mm:ss");//转格式为2023-07-01 00:00:00
                    int year = AdddateTime.Year;
                    int month = AdddateTime.Month;
                    if (month - 1 == 0)
                    {
                        month = 13;
                        year = year-1;
                    }
                    string res = $"{year.ToString().Substring(2)}{month - 1:00}";//拼接字符串为
                    tmpSql.AppendFormat(@"PARTITION WAITSTAT_{0}_{1}
                                        LOGGING
                                        NOCOMPRESS 
                                        TABLESPACE PART_TS_IDX
                                        INITRANS   2
                                        MAXTRANS   255
                                        STORAGE    (
                                                    INITIAL          64K
                                                    NEXT             1M
                                                    MINEXTENTS       1
                                                    MAXEXTENTS       UNLIMITED
                                                    BUFFER_POOL      DEFAULT
                                                   ),", arguments.DBLinkName, res);

                    if (i == 11)
                    {
                        tmpSql = tmpSql.Remove(tmpSql.Length - 1, 1);
                    }

                }

                tmpSql.Append("    )");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void CreatePKRAWDBAHISTWAITSTAT(CreateDataBaseArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(@"ALTER TABLE {0} ADD (
                                      CONSTRAINT {1}
                                      PRIMARY KEY
                                      (DBID, SNAP_ID, INSTANCE_NUMBER, CLASS, CON_DBID, BEGIN_TIME, END_TIME)
                                      USING INDEX LOCAL
                                      ENABLE VALIDATE)", arguments.DataTableName, arguments.DataTablePKName);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion
    }
}
