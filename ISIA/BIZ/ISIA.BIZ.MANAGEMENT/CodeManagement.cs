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
    class CodeManagement : TAP.Remoting.Server.Biz.BizComponentBase
    {
        public void GetDB(CodeManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("SELECT T.*,T.ROWID   FROM tapctdatabase T where 1=1 ");
                if (!string.IsNullOrEmpty(arguments.DBID))
                {
                    tmpSql.AppendFormat(" and DBID='{0}' ", arguments.DBID);
                }
                if (!string.IsNullOrEmpty(arguments.DBNAME))
                {
                    tmpSql.AppendFormat(" and DBNAME='{0}' ", arguments.DBNAME);
                }
                if (!string.IsNullOrEmpty(arguments.DBLINKNAME))
                {
                    tmpSql.AppendFormat(" and DBLINKNAME='{0}' ", arguments.DBLINKNAME);
                }
                if (!string.IsNullOrEmpty(arguments.SERVICENAME))
                {
                    tmpSql.AppendFormat(" and SERVICENAME='{0}' ", arguments.SERVICENAME);
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
                tmpSql.AppendFormat("  RETENTIONDAYS = '{0}', ", arguments.RETENTIONDAYS);
                tmpSql.AppendFormat("  ISAUTOPARTITIONDROP = '{0}', ", arguments.ISAUTOPARTITIONDROP);
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
                tmpSql.Append("Insert INTO tapctdatabase (DBID,DBNAME,DBLINKNAME,SERVICENAME,IPADDRESS,INSTANTCNT,SEQUENCES,ISALIVE,DESCRIPTION ,RETENTIONDAYS,ISAUTOPARTITIONDROP ) values (  ");
                tmpSql.AppendFormat(" '{0}',", arguments.DBID);
                tmpSql.AppendFormat(" '{0}',", arguments.DBNAME);
                tmpSql.AppendFormat(" '{0}',", arguments.DBLINKNAME);
                tmpSql.AppendFormat(" '{0}',", arguments.SERVICENAME);
                tmpSql.AppendFormat(" '{0}',", arguments.IPADDRESS);
                tmpSql.AppendFormat(" '{0}',", arguments.INSTANTCNT);
                tmpSql.AppendFormat(" '{0}',", arguments.SEQUENCES);
                tmpSql.AppendFormat(" '{0}',", arguments.ISALIVE);
                tmpSql.AppendFormat(" '{0}' , ", arguments.DESCRIPTION);
                tmpSql.AppendFormat(" '{0}' ,", arguments.RETENTIONDAYS);
                tmpSql.AppendFormat(" '{0}' )", arguments.ISAUTOPARTITIONDROP);

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

        public void SaveDbInfo(CodeManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("Insert INTO tapctdatabase (DBID,DBNAME,DBLINKNAME,SERVICENAME,IPADDRESS,INSTANTCNT,SEQUENCES,ISALIVE,DESCRIPTION,RETENTIONDAYS,INSERTUSER,INSERTTIME,ISAUTOPARTITIONDROP) values (  ");
                tmpSql.AppendFormat(" '{0}',", arguments.DBID);
                tmpSql.AppendFormat(" '{0}',", arguments.DBNAME);
                tmpSql.AppendFormat(" '{0}',", arguments.DBLINKNAME);
                tmpSql.AppendFormat(" '{0}',", arguments.SERVICENAME);
                tmpSql.AppendFormat(" '{0}',", arguments.IPADDRESS);
                tmpSql.AppendFormat(" '{0}',", arguments.INSTANTCNT);
                tmpSql.AppendFormat(" '{0}',", arguments.SEQUENCES);
                tmpSql.AppendFormat(" '{0}',", arguments.ISALIVE);
                tmpSql.AppendFormat(" '{0}', ", arguments.DESCRIPTION);
                tmpSql.AppendFormat(" '{0}',", arguments.RETENTIONDAYS);

                tmpSql.AppendFormat(" '{0}',", arguments.INSERTUSER);

                tmpSql.AppendFormat(" '{0}',", arguments.INSERTTIME);
                tmpSql.AppendFormat(" '{0}')", arguments.ISAUTOPARTITIONDROP);



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

        public void DeleteDBInfoByDBName(CodeManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("DELETE FROM tapctdatabase WHERE ");

                if (!string.IsNullOrEmpty(arguments.DBNAME))
                {
                    tmpSql.AppendFormat("DBNAME =  '{0}'", arguments.DBNAME);
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
