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
                
                tmpSql.AppendFormat(" CREATE DATABASE LINK {0}", arguments.DBLinkName);
                tmpSql.AppendFormat("  CONNECT TO {0} ", arguments.UserID);
                tmpSql.AppendFormat("  IDENTIFIED BY {0} ", arguments.Password);
                tmpSql.AppendFormat("USING '(DESCRIPTION =(ADDRESS=(PROTOCOL=TCP)(HOST={0})", arguments.IPAddress);
                tmpSql.AppendFormat("(PORT={0}))", arguments.IPPort);
                tmpSql.AppendFormat("(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = test)))'", arguments.ServiceName);

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

                tmpSql.AppendFormat("DROP TABLE {0}; ", arguments.DataTableName);


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

    }
}
