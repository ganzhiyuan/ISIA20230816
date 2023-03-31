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

namespace ISIA.BIZ.ADMINISTRATOE
{
    public class MailGroupManagement : TAP.Remoting.Server.Biz.BizComponentBase
    {
        public void GetMailGroup(MailGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("select t.*,t.ROWID from taputmailgroup t where  1=1 ");
                if (!string.IsNullOrEmpty(arguments.NAME))
                {
                    tmpSql.AppendFormat(" and NAME='{0}'",arguments.NAME);
                }
                tmpSql.Append(" ORDER BY NAME");
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

        public void GetMailGroupMember(MailGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("select * from taputmailgroupmember where 1=1 ");
                if (!string.IsNullOrEmpty(arguments.GROUPNAME))
                {
                    tmpSql.AppendFormat(" and GROUPNAME='{0}' ",arguments.GROUPNAME);
                }
                
                tmpSql.Append(" ORDER BY USERNAME");

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

        public void UpdateMailGroup(MailGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("UPDATE taputmailgroup SET ");
                tmpSql.AppendFormat("REGION = '{0}',", arguments.REGION);
                tmpSql.AppendFormat("NAME='{0}',", arguments.NAME);
                tmpSql.AppendFormat("DESCRIPTION='{0}',", arguments.DESCRIPTION);
                tmpSql.AppendFormat("UPDATETIME='{0}',", arguments.UPDATETIME);
                tmpSql.AppendFormat("UPDATEUSER='{0}'", arguments.UPDATEUSER);
                //tmpSql = new StringBuilder(tmpSql.ToString().Substring(0, tmpSql.Length - 1));
                tmpSql.Append(" WHERE 1=1 ");
                tmpSql.AppendFormat(" AND NAME = '{0}'", arguments.NAME);
                tmpSql.AppendFormat(" AND REGION = '{0}'", arguments.REGION);
                //tmpSql.Append(" AND TAGETUI = '0'");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);
                List<string> querys = new List<string>();
                querys.Add(tmpSql.ToString());
                this.ExecutingValue = db.Save(querys);

            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void UpdateMailGroupMember(MailGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("UPDATE taputmailgroupmember SET ");
                tmpSql.AppendFormat("GROUPNAME = '{0}',", arguments.GROUPNAME);
                tmpSql.AppendFormat("REGION = '{0}',", arguments.REGION);
                tmpSql.AppendFormat("FACILITY='{0}',", arguments.FACILITY);
                tmpSql.AppendFormat("USERID='{0}',", arguments.USERID);
                tmpSql.AppendFormat("USERNAME='{0}',", arguments.USERNAME);
                tmpSql.AppendFormat("DEPARTMENT='{0}',", arguments.DEPARTMENT);
                tmpSql.AppendFormat("POSITION='{0}'", arguments.POSITION);
                tmpSql.AppendFormat("UPDATETIME='{0}',", arguments.UPDATETIME);
                tmpSql.AppendFormat("UPDATEUSER='{0}'", arguments.UPDATEUSER);
                //tmpSql = new StringBuilder(tmpSql.ToString().Substring(0, tmpSql.Length - 1));
                tmpSql.Append(" WHERE 1=1 ");
                tmpSql.AppendFormat(" AND GROUPNAME = '{0}'", arguments.GROUPNAME);
                //tmpSql.Append(" AND TAGETUI = '0'");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);
                List<string> querys = new List<string>();
                querys.Add(tmpSql.ToString());
                this.ExecutingValue = db.Save(querys);

            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void InsertMailGroup(MailGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("insert into taputmailgroup (REGION,NAME,DESCRIPTION,INSERTUSER,INSERTTIME ");
                tmpSql.Append(" )VALUES (");
                tmpSql.AppendFormat("'{0}',", arguments.REGION);
                tmpSql.AppendFormat("'{0}',", arguments.NAME);
                tmpSql.AppendFormat("'{0}',", arguments.DESCRIPTION);
                tmpSql.AppendFormat("'{0}',", arguments.INSERTUSER);
                tmpSql.AppendFormat("'{0}'", arguments.INSERTTIME);
                tmpSql.Append(" ) ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);
                List<string> querys = new List<string>();
                querys.Add(tmpSql.ToString());
                this.ExecutingValue = db.Save(querys);

            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void InsertMailGroupMember(MailGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("insert into taputmailgroupmember (GROUPNAME,REGION,FACILITY,USERID,USERNAME,DEPARTMENT,POSITION,INSERTUSER,INSERTTIME ");
                tmpSql.Append(" ) VALUES (");
                tmpSql.AppendFormat("'{0}',", arguments.GROUPNAME);
                tmpSql.AppendFormat("'{0}',", arguments.REGION);
                tmpSql.AppendFormat("'{0}',", arguments.FACILITY);
                tmpSql.AppendFormat("'{0}',", arguments.USERID);
                tmpSql.AppendFormat("'{0}',", arguments.USERNAME);
                tmpSql.AppendFormat("'{0}',", arguments.DEPARTMENT);
                tmpSql.AppendFormat("'{0}',", arguments.POSITION);
                tmpSql.AppendFormat("'{0}',", arguments.INSERTUSER);
                tmpSql.AppendFormat("'{0}'", arguments.INSERTTIME);
                tmpSql.Append(" ) ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);
                List<string> querys = new List<string>();
                querys.Add(tmpSql.ToString());
                this.ExecutingValue = db.Save(querys);

            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void CheckMailGroupMember(MailGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("select * from taputmailgroupmember where 1=1 ");
                if (!string.IsNullOrEmpty(arguments.GROUPNAME))
                {
                    tmpSql.AppendFormat(" and GROUPNAME='{0}' ", arguments.GROUPNAME);
                }
                if (!string.IsNullOrEmpty(arguments.USERNAME))
                {
                    tmpSql.AppendFormat(" and USERNAME='{0}' ", arguments.USERNAME);
                }
                if (!string.IsNullOrEmpty(arguments.REGION))
                {
                    tmpSql.AppendFormat(" and REGION='{0}' ", arguments.REGION);
                }
                if (!string.IsNullOrEmpty(arguments.FACILITY))
                {
                    tmpSql.AppendFormat(" and FACILITY='{0}' ", arguments.FACILITY);
                }

                tmpSql.Append(" ORDER BY USERNAME");

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

        public void DeleteMailGroupMember(MailGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<string> tmpSqls = new List<string>();
                
                    StringBuilder tmpSql = new StringBuilder();
                    tmpSql.Append("DELETE FROM taputmailgroupmember WHERE 1=1 ");

                    tmpSql.AppendFormat(" AND GROUPNAME = '{0}'", arguments.GROUPNAME);
                    tmpSql.AppendFormat(" AND USERNAME = '{0}'", arguments.USERNAME);
                    tmpSql.AppendFormat(" AND REGION = '{0}'", arguments.REGION);
                    tmpSql.AppendFormat(" AND FACILITY = '{0}'", arguments.FACILITY);
                    tmpSqls.Add(tmpSql.ToString());
                
                this.ExecutingValue = db.Save(tmpSqls);
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void DeleteMailGroup(MailGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<string> tmpSqls = new List<string>();

                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("DELETE FROM taputmailgroup WHERE 1=1 ");

                tmpSql.AppendFormat(" AND NAME = '{0}'", arguments.NAME);
                tmpSqls.Add(tmpSql.ToString());

                this.ExecutingValue = db.Save(tmpSqls);
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
