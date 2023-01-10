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
    public class GroupUserManagement : TAP.Remoting.Server.Biz.BizComponentBase
    {
        public void GetGroup()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT * FROM TAPUTGROUP");

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
        public void GetUsers()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT * FROM TAPUTUSERS");

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
        public void GetGroupMember(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT * FROM TAPUTGROUPMEMBER ");
                tmpSql.AppendFormat(" WHERE USERGROUP='{0}'", arguments.GroupName);

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
        public void GetGroupMemberByUser(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT * FROM TAPUTGROUPMEMBER ");
                tmpSql.AppendFormat(" WHERE USERGROUP='{0}'", arguments.GroupName);
                tmpSql.AppendFormat(" AND NAME = '{0}'", arguments.Name);
                tmpSql.AppendFormat(" AND REGION = '{0}'", arguments.Region);
                tmpSql.AppendFormat(" AND FACILITY = '{0}'", arguments.Facility);
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
        public void DeleteGroupAndGroupmember(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<string> tmpSqls = new List<string>();
                StringBuilder tmpSqlgroup = new StringBuilder();
                tmpSqlgroup.Append("DELETE FROM TAPUTGROUP WHERE 1=1 ");
                tmpSqlgroup.AppendFormat(" AND NAME = '{0}'", arguments.Name);
               
                StringBuilder tmpSqlmember = new StringBuilder();
                tmpSqlmember.Append("DELETE FROM TAPUTGROUPMEMBER WHERE 1=1 ");
                tmpSqlmember.AppendFormat(" AND USERGROUP = '{0}'", arguments.GroupName);
              
                tmpSqls.Add(tmpSqlgroup.ToString());
                tmpSqls.Add(tmpSqlmember.ToString());
                this.ExecutingValue = db.Save(tmpSqls);
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void DeleteGroupmember(List<CommonArgsPack> arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<string> tmpSqls = new List<string>();
                for (int i = 0; i < arguments.Count; i++)
                {
                    StringBuilder tmpSql = new StringBuilder();
                    tmpSql.Append("DELETE FROM TAPUTGROUPMEMBER WHERE 1=1 ");

                    tmpSql.AppendFormat(" AND USERGROUP = '{0}'", arguments[i].GroupName);
                    tmpSql.AppendFormat(" AND NAME = '{0}'", arguments[i].Name);
                    tmpSql.AppendFormat(" AND REGION = '{0}'", arguments[i].Region);
                    tmpSql.AppendFormat(" AND FACILITY = '{0}'", arguments[i].Facility);
                    tmpSqls.Add(tmpSql.ToString());
                }
                this.ExecutingValue = db.Save(tmpSqls);
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void DeleteUsers(List<CommonArgsPack> arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<string> tmpSqls = new List<string>();
                for (int i = 0; i < arguments.Count; i++)
                {
                    StringBuilder tmpSql = new StringBuilder();
                    tmpSql.Append("DELETE FROM TAPUTUSERS WHERE 1=1 ");
                    tmpSql.AppendFormat(" AND NAME = '{0}'", arguments[i].Name);
                    tmpSql.AppendFormat(" AND REGION = '{0}'", arguments[i].Region);
                    tmpSql.AppendFormat(" AND FACILITY = '{0}'", arguments[i].Facility);
                    tmpSqls.Add(tmpSql.ToString());
                }
                this.ExecutingValue = db.Save(tmpSqls);
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void SaveGroupmember(List<CommonArgsPack> arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<string> tmpSqls = new List<string>();
                for (int i = 0; i < arguments.Count; i++)
                {
                    StringBuilder tmpSql = new StringBuilder();
                    tmpSql.Append("INSERT INTO TAPUTGROUPMEMBER ( ");
                    tmpSql.Append("USERGROUP, NAME, REGION, FACILITY, DEPARTMENT, POSITION,  ");
                    tmpSql.Append("USERNAME, USERLASTNAME, CURRENTMODEL, DESCRIPTION, LASTEVENTCOMMENT, ");
                    tmpSql.Append("LASTEVENT, LASTEVENTFLAG, LASTEVENTTIME, LASTEVENTCODE, LASTJOBCODE, ");
                    tmpSql.Append(" INSERTTIME, UPDATETIME, INSERTUSER, UPDATEUSER, SEQUENCES, ISALIVE, ");
                    tmpSql.Append("MODELLEVELS, LANGUAGE )VALUES ( ");
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].GroupName);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].Name);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].Region);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].Facility);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].Department);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].Position);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].UserName);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].UserLastName);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].CurrentModel);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].Description);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].LastEventComment);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].LastEvent);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].LastEventFlag);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].LastEventTime);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].LastEventCode);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].LastJobCode);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].InsertTime);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].UpdateTime);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].InsertUser);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].UpdateUser);
                    tmpSql.AppendFormat("  {0} ,", arguments[i].Sequences);
                    tmpSql.AppendFormat(" '{0}' ,", arguments[i].IsAlive);
                    tmpSql.AppendFormat(" {0} ,", arguments[i].Modellevels);
                    tmpSql.AppendFormat(" '{0}'", arguments[i].Language);
                    tmpSql.Append(" )");
                    tmpSqls.Add(tmpSql.ToString());
                }

                this.ExecutingValue = db.Save(tmpSqls);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateGroupmember(List<CommonArgsPack> arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<string> tmpSqls = new List<string>();
                for (int i = 0; i < arguments.Count; i++)
                {
                    StringBuilder tmpSql = new StringBuilder();
                    tmpSql.Append("UPDATE TAPUTGROUPMEMBER SET ");
                    tmpSql.AppendFormat(" DEPARTMENT ='{0}' ", arguments[i].Department);
                    tmpSql.AppendFormat(" , POSITION ='{0}' ", arguments[i].Position);
                    tmpSql.AppendFormat(" , USERNAME ='{0}' ", arguments[i].UserName);
                    tmpSql.AppendFormat(" , USERLASTNAME ='{0}' ", arguments[i].UserLastName);
                    tmpSql.AppendFormat(" , CURRENTMODEL ='{0}' ", arguments[i].CurrentModel);
                    tmpSql.AppendFormat(" , DESCRIPTION ='{0}' ", arguments[i].Description);
                    tmpSql.AppendFormat(" , LASTEVENTCOMMENT='{0}' ", arguments[i].LastEventComment);
                    tmpSql.AppendFormat(" , LASTEVENT ='{0}' ", arguments[i].LastEvent);
                    tmpSql.AppendFormat(" , LASTEVENTFLAG ='{0}' ", arguments[i].LastEventFlag);
                    tmpSql.AppendFormat(" , LASTEVENTTIME ='{0}' ", arguments[i].LastEventTime);
                    tmpSql.AppendFormat(" , LASTEVENTCODE ='{0}' ", arguments[i].LastEventCode);
                    tmpSql.AppendFormat(" , LASTJOBCODE ='{0}' ", arguments[i].LastJobCode);
                    tmpSql.AppendFormat(" , INSERTTIME ='{0}' ", arguments[i].InsertTime);
                    tmpSql.AppendFormat(" , UPDATETIME ='{0}' ", arguments[i].UpdateTime);
                    tmpSql.AppendFormat(" , INSERTUSER ='{0}' ", arguments[i].InsertUser);
                    tmpSql.AppendFormat(" , UPDATEUSER ='{0}' ", arguments[i].UpdateUser);
                    tmpSql.AppendFormat(" , SEQUENCES = {0} ", arguments[i].Sequences);
                    tmpSql.AppendFormat(" , ISALIVE ='{0}' ", arguments[i].IsAlive);
                    tmpSql.AppendFormat(" , MODELLEVELS ={0} ", arguments[i].Modellevels);
                    tmpSql.AppendFormat(" , LANGUAGE ='{0}' ", arguments[i].Language);
                    tmpSql.AppendFormat(" WHERE ");
                    tmpSql.AppendFormat(" USERGROUP ='{0}' ", arguments[i].GroupName);
                    tmpSql.AppendFormat(" AND NAME ='{0}' ", arguments[i].Name);
                    tmpSql.AppendFormat(" AND REGION ='{0}' ", arguments[i].Region);
                    tmpSql.AppendFormat(" AND FACILITY ='{0}' ", arguments[i].Facility);
                    tmpSqls.Add(tmpSql.ToString());
                }

                this.ExecutingValue = db.Save(tmpSqls);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateGroup(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("UPDATE TAPUTGROUP SET ");
                tmpSql.AppendFormat("UPDATETIME = '{0}' ,", arguments.UpdateTime);
                tmpSql.AppendFormat("UPDATEUSER = '{0}' ,", arguments.UpdateUser);
                tmpSql.AppendFormat("CURRENTMODEL = '{0}' ,", arguments.CurrentModel);
                tmpSql.AppendFormat("DESCRIPTION = '{0}' ,", arguments.Description);
                tmpSql.AppendFormat("ISALIVE = '{0}' ,", arguments.IsAlive);
                tmpSql.AppendFormat("LEVELS = '{0}' ", arguments.Levels);
                tmpSql.Append(" WHERE 1=1");
                tmpSql.AppendFormat(" AND NAME = '{0}' ", arguments.Name);
                tmpSql.AppendFormat(" AND REGION = '{0}' ", arguments.Region);
                tmpSql.AppendFormat(" AND FACILITY = '{0}' ", arguments.Facility);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                      tmpSql.ToString(), false);
                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveGroup(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("INSERT INTO TAPUTGROUP (NAME, REGION,FACILITY,LEVELS,CURRENTMODEL,DESCRIPTION,INSERTTIME,INSERTUSER");
                tmpSql.AppendFormat(")VALUES(");
                tmpSql.AppendFormat("'{0}' , ", arguments.Name);
                tmpSql.AppendFormat("'{0}' , ", arguments.Region);
                tmpSql.AppendFormat("'{0}' , ", arguments.Facility);
                tmpSql.AppendFormat("'{0}' , ", arguments.Levels);
                tmpSql.AppendFormat("'{0}' , ", arguments.CurrentModel);
                tmpSql.AppendFormat("'{0}' , ", arguments.Description);
                tmpSql.AppendFormat("'{0}' , ", arguments.InsertTime);
                tmpSql.AppendFormat("'{0}'", arguments.InsertUser);
                tmpSql.AppendFormat(" )");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                      tmpSql.ToString(), false);
                this.ExecutingValue = db.Save(new string[] { tmpSql.ToString() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateUser(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("UPDATE TAPUTUSERS SET ");
                tmpSql.AppendFormat("name = '{0}',", arguments.Name);
                tmpSql.AppendFormat("region = '{0}',", arguments.Region);
                tmpSql.AppendFormat("facility='{0}',", arguments.Facility);
                tmpSql.AppendFormat("department='{0}',", arguments.Department);
                tmpSql.AppendFormat("position='{0}',", arguments.Position);
                tmpSql.AppendFormat("username='{0}',", arguments.UserName);
                tmpSql.AppendFormat("usermiddlename='{0}',", arguments.UserMiddleName);
                tmpSql.AppendFormat("userlastname='{0}',", arguments.UserLastName);
                tmpSql.AppendFormat("contactno='{0}',", arguments.Contactno);
                tmpSql.AppendFormat("mobileno='{0}',", arguments.Mobileno);
                tmpSql.AppendFormat("mailaddress='{0}',", arguments.MailAddress);
                tmpSql.AppendFormat("password='{0}',", arguments.Password);
                tmpSql.AppendFormat("usergroupname='{0}',", arguments.UserGroupName);
                tmpSql.AppendFormat("language='{0}',", arguments.Language);
                tmpSql.AppendFormat("currentmodel='{0}',", arguments.CurrentModel);
                tmpSql.AppendFormat("description='{0}',", arguments.Description);
                tmpSql.AppendFormat("UPDATETIME = '{0}',", arguments.UpdateTime);
                tmpSql.AppendFormat("UPDATEUSER='{0}',", arguments.UpdateUser);
                tmpSql.AppendFormat("isalive='{0}',", arguments.IsAlive);
                tmpSql = new StringBuilder(tmpSql.ToString().Substring(0, tmpSql.Length - 1));
                tmpSql.Append(" WHERE 1=1 ");
                tmpSql.AppendFormat("AND name = '{0}'", arguments.Name);
                tmpSql.AppendFormat("AND region = '{0}'", arguments.Region);
                tmpSql.AppendFormat("AND facility='{0}'", arguments.Facility);
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
        public void SaveUser(CommonArgsPack arguments)
        {

            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("insert into TAPUTUSERS (");
                tmpSql.Append(" name,region,facility,department,position,");
                tmpSql.Append("username,usermiddlename,userlastname,contactno,");
                tmpSql.Append("mobileno,mailaddress,password,usergroupname,");
                tmpSql.Append("language,currentmodel,description,INSERTTIME,INSERTUSER,IsAlive,sequences )VALUES (");
                tmpSql.AppendFormat("'{0}',", arguments.Name);
                tmpSql.AppendFormat("'{0}',", arguments.Region);
                tmpSql.AppendFormat("'{0}',", arguments.Facility);
                tmpSql.AppendFormat("'{0}',", arguments.Department);
                tmpSql.AppendFormat("'{0}',", arguments.Position);
                tmpSql.AppendFormat("'{0}',", arguments.UserName);
                tmpSql.AppendFormat("'{0}',", arguments.UserMiddleName);
                tmpSql.AppendFormat("'{0}',", arguments.UserLastName);
                tmpSql.AppendFormat("'{0}',", arguments.Contactno);
                tmpSql.AppendFormat("'{0}',", arguments.Mobileno);
                tmpSql.AppendFormat("'{0}',", arguments.MailAddress);
                tmpSql.AppendFormat("'{0}',", arguments.Password);
                tmpSql.AppendFormat("'{0}',", arguments.UserGroupName);
                tmpSql.AppendFormat("'{0}',", arguments.Language);
                tmpSql.AppendFormat("'{0}',", arguments.CurrentModel);
                tmpSql.AppendFormat("'{0}',", arguments.Description);
                tmpSql.AppendFormat("'{0}',", arguments.InsertTime);
                tmpSql.AppendFormat("'{0}',", arguments.InsertUser);
                tmpSql.AppendFormat("'{0}',", arguments.IsAlive);
                tmpSql.AppendFormat("{0}", arguments.Sequence);
                tmpSql.Append(")");
                string[] query = { tmpSql.ToString() };
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                      tmpSql.ToString(), false);
                this.ExecutingValue = db.Save(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
