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

namespace ISIA.BIZ.ADMINISTRATOE
{
    class UserMailManagement : TAP.Remoting.Server.Biz.BizComponentBase
    {
        #region Cons
        private string _region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
        #endregion
        public void GetGroupUser(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {

                DataSet ds = new DataSet();

                StringBuilder groupdtsql = new StringBuilder();
                StringBuilder userdtsql = new StringBuilder();
                groupdtsql.Append("SELECT B.USERID, B.USERNAME FROM TAPUTMAILGROUP A, TAPUTMAILGROUPMEMBER B WHERE A.NAME=B.GROUPNAME  AND A.REGION=B.REGION");
                if (!string.IsNullOrEmpty(arguments.GroupName))
                {
                    groupdtsql.AppendFormat(" AND A.NAME='{0}'", arguments.GroupName);
                }
                userdtsql.Append(string.Format("SELECT NAME ,REGION ,FACILITY ,DEPARTMENT ,POSITION, USERNAME FROM TAPUTUSERS WHERE  ISALIVE='YES'"));
                DataTable groupdt = db.Select(groupdtsql.ToString()).Tables[0];
                DataTable userdt = db.Select(userdtsql.ToString()).Tables[0];
                groupdt.TableName = "GROUPUSER";
                ds.Tables.Add(groupdt.Copy());
                userdt.TableName = "USER";
                ds.Tables.Add(userdt.Copy());

                this.ExecutingValue = ds;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }

        }

        public void SaveMailGroup(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                arguments.Region = _region;
                tmpSql.Append("insert into TAPUTMAILGROUP (");
                if (!string.IsNullOrEmpty(arguments.Region)) { tmpSql.AppendFormat("REGION,"); }
                if (!string.IsNullOrEmpty(arguments.Name)) { tmpSql.AppendFormat("NAME,"); }
                if (!string.IsNullOrEmpty(arguments.Description)) { tmpSql.AppendFormat("DESCRIPTION,"); }
                if (!string.IsNullOrEmpty(arguments.InsertTime)) { tmpSql.AppendFormat("INSERTTIME,"); }
                if (!string.IsNullOrEmpty(arguments.InsertUser)) { tmpSql.AppendFormat("INSERTUSER,"); }
                if (tmpSql.ToString().Substring(tmpSql.Length - 1, 1).Equals(","))
                {
                    tmpSql = new StringBuilder(tmpSql.ToString().Substring(0, tmpSql.Length - 1));
                }
                tmpSql.Append(")values(");
                if (!string.IsNullOrEmpty(arguments.Region))
                {
                    tmpSql.AppendFormat("'{0}',", arguments.Region);
                }
                if (!string.IsNullOrEmpty(arguments.Name))
                {
                    tmpSql.AppendFormat("'{0}',", arguments.Name);
                }
                if (!string.IsNullOrEmpty(arguments.Description))
                {
                    tmpSql.AppendFormat("'{0}',", arguments.Description);
                }
                if (!string.IsNullOrEmpty(arguments.InsertTime))
                {
                    tmpSql.AppendFormat("'{0}',", arguments.InsertTime);
                }
                if (!string.IsNullOrEmpty(arguments.InsertUser))
                {
                    tmpSql.AppendFormat("'{0}',", arguments.InsertUser);
                }

                if (tmpSql.ToString().Substring(tmpSql.Length - 1, 1).Equals(","))
                {
                    tmpSql = new StringBuilder(tmpSql.ToString().Substring(0, tmpSql.Length - 1));
                }
                tmpSql.Append(")");
                string[] query = { tmpSql.ToString() };
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                      tmpSql.ToString(), false);
                this.ExecutingValue = db.Save(query);
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                     string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void DeleteGroupMember(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("DELETE  FROM TAPUTMAILGROUPMEMBER WHERE 1=1 ");
                if (!string.IsNullOrEmpty(arguments.GroupName))
                {
                    tmpSql.AppendFormat(" AND GROUPNAME = '{0}'", arguments.GroupName);
                }
                if (!string.IsNullOrEmpty(arguments.UserName))
                {
                    tmpSql.AppendFormat(" AND USERID = '{0}'", arguments.UserName);
                }

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
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

        public void SaveGroupMember(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("insert into TAPUTMAILGROUPMEMBER (");
                if (!string.IsNullOrEmpty(arguments.GroupName)) { tmpSql.AppendFormat("GROUPNAME,"); }
                if (!string.IsNullOrEmpty(arguments.Region)) { tmpSql.AppendFormat("REGION,"); }
                if (!string.IsNullOrEmpty(arguments.Facility)) { tmpSql.AppendFormat("FACILITY,"); }
                if (!string.IsNullOrEmpty(arguments.Department)) { tmpSql.AppendFormat("DEPARTMENT,"); }
                if (!string.IsNullOrEmpty(arguments.Position)) { tmpSql.AppendFormat("POSITION,"); }
                if (!string.IsNullOrEmpty(arguments.UserName)) { tmpSql.AppendFormat("USERNAME,"); }
                if (!string.IsNullOrEmpty(arguments.UserID)) { tmpSql.AppendFormat("USERID,"); }
                if (!string.IsNullOrEmpty(arguments.InsertTime)) { tmpSql.AppendFormat("INSERTTIME,"); }
                if (!string.IsNullOrEmpty(arguments.InsertUser)) { tmpSql.AppendFormat("INSERTUSER,"); }
                if (tmpSql.ToString().Substring(tmpSql.Length - 1, 1).Equals(","))
                {
                    tmpSql = new StringBuilder(tmpSql.ToString().Substring(0, tmpSql.Length - 1));
                }
                tmpSql.Append(")values(");
                if (!string.IsNullOrEmpty(arguments.GroupName))
                {
                    tmpSql.AppendFormat("'{0}',", arguments.GroupName);
                }
                if (!string.IsNullOrEmpty(arguments.Region))
                {
                    tmpSql.AppendFormat("'{0}',", arguments.Region);
                }
                if (!string.IsNullOrEmpty(arguments.Facility))
                {
                    tmpSql.AppendFormat("'{0}',", arguments.Facility);
                }
                if (!string.IsNullOrEmpty(arguments.Department))
                {
                    tmpSql.AppendFormat("'{0}',", arguments.Department);
                }
                if (!string.IsNullOrEmpty(arguments.Position))
                {
                    tmpSql.AppendFormat("'{0}',", arguments.Position);
                }
                if (!string.IsNullOrEmpty(arguments.UserName))
                {
                    tmpSql.AppendFormat("'{0}',", arguments.UserName);
                }
                if (!string.IsNullOrEmpty(arguments.UserName))
                {
                    tmpSql.AppendFormat("'{0}',", arguments.UserID);
                }
                if (!string.IsNullOrEmpty(arguments.InsertTime))
                {
                    tmpSql.AppendFormat("'{0}',", arguments.InsertTime);
                }
                if (!string.IsNullOrEmpty(arguments.InsertUser))
                {
                    tmpSql.AppendFormat("'{0}',", arguments.InsertUser);
                }
                if (tmpSql.ToString().Substring(tmpSql.Length - 1, 1).Equals(","))
                {
                    tmpSql = new StringBuilder(tmpSql.ToString().Substring(0, tmpSql.Length - 1));
                }
                tmpSql.Append(")");
                string[] query = { tmpSql.ToString() };
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                      tmpSql.ToString(), false);
                this.ExecutingValue = db.Save(query);
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                     string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetDescription(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                DataSet ds = new DataSet();

                StringBuilder tmpsql = new StringBuilder();
                tmpsql.AppendFormat("SELECT REGION, DESCRIPTION  FROM TAPUTMAILGROUP WHERE NAME = '{0}'",arguments.GroupName);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                    tmpsql.ToString(), false);
                this.ExecutingValue = db.Select(tmpsql.ToString());
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
