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
    public class LinkageManagement : TAP.Remoting.Server.Biz.BizComponentBase
    {
        public void GetListName()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("select NAME,DISPLAYNAME from TAPSTBUI where mdi = 'ISIA' and ASSEMBLYFILENAME like 'ISIA%'and isalive = 'YES' ORDER BY NAME");

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

        public void GetCtLinkageParent()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("select distinct groupname from tapctlinkage where isalive = 'YES' and TAGETUI='0'");

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

        public void GetCtLinkageByGroupName(CommonArgsPack argument)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("select *  from tapctlinkage where GROUPNAME='{0}' and isalive = 'YES'", argument.GroupName);

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

        public void CheckLinkAge(CommonArgsPack argument)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("select *  from tapctlinkage where GROUPNAME='{0}' and TAGETUINAME='{1}' and TAGETUI='{2}'", 
                    argument.GroupName,argument.MessageName,argument.Custom01);

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

        public void GetCtLinkageByMaxID()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("SELECT max(to_number(groupid)) FROM tapctlinkage");

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

        public void UpdateUser(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("UPDATE tapctlinkage SET ");
                tmpSql.AppendFormat("GROUPNAME = '{0}',", arguments.GroupName);
                tmpSql.AppendFormat("TAGETUINAME = '{0}',", arguments.MessageName);
                tmpSql.AppendFormat("PARAMETERLIST='{0}',", arguments.PartName);
                tmpSql.AppendFormat("ISALIVE='{0}',", arguments.IsAlive);
                tmpSql.AppendFormat("DESCRIPTION='{0}',", arguments.Description);
                tmpSql.AppendFormat("UPDATETIME='{0}',", arguments.UpdateTime);
                tmpSql.AppendFormat("UPDATEUSER='{0}'", arguments.UpdateUser);
                //tmpSql = new StringBuilder(tmpSql.ToString().Substring(0, tmpSql.Length - 1));
                tmpSql.Append(" WHERE 1=1 ");
                tmpSql.AppendFormat(" AND GROUPID = '{0}'", arguments.UserID);
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


        public void SaveUIGroup(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("insert into tapctlinkage (");
                tmpSql.Append(" GROUPID,GROUPNAME,UI,TAGETUI,TAGETUINAME,PARAMETERLIST,ISALIVE,DESCRIPTION,LASTEVENT");
                tmpSql.Append(" ,INSERTTIME,UPDATETIME,INSERTUSER,UPDATEUSER");
                tmpSql.Append(" )VALUES (");
                tmpSql.AppendFormat("'{0}',", arguments.EqpGroup);
                tmpSql.AppendFormat("'{0}',", arguments.GroupName);
                tmpSql.AppendFormat("'{0}',", string.IsNullOrEmpty(arguments.Custom02) ? "0" : arguments.Custom02);
                tmpSql.AppendFormat("'{0}',", string.IsNullOrEmpty(arguments.Custom01)?"0": arguments.Custom01);
                tmpSql.AppendFormat("'{0}',", arguments.MessageName);
                tmpSql.AppendFormat("'{0}',", string.IsNullOrEmpty(arguments.PartName)?"0": arguments.PartName);
                tmpSql.AppendFormat("'{0}',", arguments.IsAlive);
                tmpSql.AppendFormat("'{0}',", arguments.Description);
                tmpSql.AppendFormat("'{0}',", arguments.LastEvent);
                tmpSql.AppendFormat("'{0}',", arguments.InsertTime);
                tmpSql.AppendFormat("'{0}',", arguments.UpdateTime);
                tmpSql.AppendFormat("'{0}',", arguments.InsertUser);
                tmpSql.AppendFormat("'{0}'", arguments.UpdateUser);
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

        public void DeleteLinkAge(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<string> tmpSqls = new List<string>();
                StringBuilder tmpSqlgroup = new StringBuilder();
                tmpSqlgroup.Append("DELETE FROM tapctlinkage WHERE 1=1 ");
                if (!string.IsNullOrEmpty(arguments.Name))
                {

                    tmpSqlgroup.AppendFormat(" AND GROUPID = '{0}'", arguments.Name);
                }
                else //只在删除group时候调用到此(根据groupname则删除包括子集）
                {
                    tmpSqlgroup.AppendFormat(" AND GROUPNAME = '{0}'", arguments.GroupName);
                }


                tmpSqls.Add(tmpSqlgroup.ToString());
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
