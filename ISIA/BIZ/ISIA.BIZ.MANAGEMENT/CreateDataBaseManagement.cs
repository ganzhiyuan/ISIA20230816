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

        public void CheckTcode(CodeManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                StringBuilder sbSel = new StringBuilder();
                sbSel.Append("select * from tapctdatabase");
                sbSel.Append(" where 1=1 ");
                sbSel.AppendFormat(" and DBID='{0}' ", arguments.DBID);
                sbSel.AppendFormat(" and DBNAME='{0}' ", arguments.DBNAME);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                      sbSel.ToString(), false);

                this.ExecutingValue = db.Select(sbSel.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void UpdateTcode(CodeManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("UPDATE tapctdatabase SET ");
                tmpSql.AppendFormat("  DBID = '{0}' ,", arguments.DBID);
                tmpSql.AppendFormat("  DBNAME = '{0}' ,", arguments.DBNAME);
                tmpSql.AppendFormat("  DBLINKNAME = '{0}' ,", arguments.DBLINKNAME);
                tmpSql.AppendFormat("  SERVICENAME = '{0}' ,", arguments.SERVICENAME);
                tmpSql.AppendFormat("  IPADDRESS = '{0}' ,", arguments.IPADDRESS);
                tmpSql.AppendFormat("  INSTANTCNT = '{0}' ,", arguments.INSTANTCNT);
                tmpSql.AppendFormat("  SEQUENCES = '{0}' ,", arguments.SEQUENCES);
                tmpSql.AppendFormat("  ISALIVE = '{0}' ,", arguments.ISALIVE);
                tmpSql.AppendFormat("  DESCRIPTION = '{0}' ", arguments.DESCRIPTION);


                tmpSql.Append(" where 1=1 ");
                tmpSql.AppendFormat(" and DBID='{0}' ", arguments.DBID);
                tmpSql.AppendFormat(" and DBNAME='{0}' ", arguments.DBNAME);

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

        public void SaveTCode(CodeManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("Insert INTO tapctdatabase (DBID,DBNAME,DBLINKNAME,SERVICENAME,IPADDRESS,INSTANTCNT,SEQUENCES,ISALIVE,DESCRIPTION) values (  ");
                tmpSql.AppendFormat(" '{0}',", arguments.DBID);
                tmpSql.AppendFormat(" '{0}',", arguments.DBNAME);
                tmpSql.AppendFormat(" '{0}',", arguments.DBLINKNAME);
                tmpSql.AppendFormat(" '{0}',", arguments.SERVICENAME);
                tmpSql.AppendFormat(" '{0}',", arguments.IPADDRESS);
                tmpSql.AppendFormat(" '{0}',", arguments.INSTANTCNT);
                tmpSql.AppendFormat(" '{0}',", arguments.SEQUENCES);
                tmpSql.AppendFormat(" '{0}',", arguments.ISALIVE);
                tmpSql.AppendFormat(" '{0}' )", arguments.DESCRIPTION);

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

        public void DelteTCODE(CodeManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("DELETE FROM tapctdatabase WHERE ");

                if (!string.IsNullOrEmpty(arguments.ROWID))
                {
                    tmpSql.AppendFormat("ROWID =  '{0}'", arguments.ROWID);
                }
                else
                {
                    return;
                }

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
