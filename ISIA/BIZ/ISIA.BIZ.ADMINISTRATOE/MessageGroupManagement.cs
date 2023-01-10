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
    class MessageGroupManagement : TAP.Remoting.Server.Biz.BizComponentBase
    {
        public void GetSpec(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT * FROM TAPUTMESSAGEGROUP WHERE 1=1 ");

                if (!string.IsNullOrEmpty(arguments.MessageType))
                {
                    tmpSql.AppendFormat(" AND MESSAGETYPE IN ({0})", Utils.MakeSqlQueryIn(arguments.MessageType, ','));
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
        public void SaveMessageGroup(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<string> querys = new List<string>();
                StringBuilder tmpSql = new StringBuilder();
                CommonArgsPack tempArgs = arguments;
                tmpSql.Append("INSERT INTO TAPUTMESSAGEGROUP (MESSAGETYPE, MESSAGENAME, CUSTOM01, CUSTOM02, CUSTOM03, CUSTOM04, CUSTOM05, CUSTOM06, CUSTOM07, CUSTOM08, CUSTOM09, UPDATEUSER, UPDATETIME) VALUES (");
                tmpSql.AppendFormat("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')",
                    tempArgs.MessageType, tempArgs.MessageName, tempArgs.Custom01, tempArgs.Custom02, tempArgs.Custom03,
                    tempArgs.Custom04, tempArgs.Custom05, tempArgs.Custom06, tempArgs.Custom07, tempArgs.Custom08, tempArgs.Custom09, tempArgs.UpdateUser, tempArgs.UpdateTime);

                querys.Add(tmpSql.ToString());

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
                   tmpSql.ToString(), false);

                this.ExecutingValue = db.Save(querys);
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void DeleteGroupMember(List<CommonArgsPack> arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                List<string> querys = new List<string>();

                for (int i = 0; i < arguments.Count; i++)
                {

                    tmpSql.Append("DELETE  FROM TAPUTMESSAGEGROUP WHERE 1=1 ");
                    tmpSql.AppendFormat(" AND MESSAGETYPE = '{0}'", arguments[i].MessageType);
                    tmpSql.AppendFormat(" AND MESSAGENAME = '{0}'", arguments[i].MessageName);
                    RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                           tmpSql.ToString(), false);
                    querys.Add(tmpSql.ToString());
                }
                this.ExecutingValue = db.Save(querys);
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void UpdateGroupMember(List<CommonArgsPack> arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<string> querys = new List<string>();
                StringBuilder tmpSql = new StringBuilder();
                for (int i = 0; i < arguments.Count; i++)
                {
                    tmpSql.Append("UPDATE TAPUTMESSAGEGROUP SET ");
                    tmpSql.AppendFormat("  CUSTOM01 = '{0}'", arguments[i].Custom01);
                    tmpSql.AppendFormat(" , CUSTOM02 = '{0}'", arguments[i].Custom02);
                    tmpSql.AppendFormat(" , CUSTOM03 = '{0}'", arguments[i].Custom03);
                    tmpSql.AppendFormat(" , CUSTOM04 = '{0}'", arguments[i].Custom04);
                    tmpSql.AppendFormat(" , CUSTOM05 = '{0}'", arguments[i].Custom05);
                    tmpSql.AppendFormat(" , CUSTOM06 = '{0}'", arguments[i].Custom06);
                    tmpSql.AppendFormat(" , CUSTOM07 = '{0}'", arguments[i].Custom07);
                    tmpSql.AppendFormat(" , CUSTOM08 = '{0}'", arguments[i].Custom08);
                    tmpSql.AppendFormat(" , CUSTOM09 = '{0}'", arguments[i].Custom09);
                    tmpSql.AppendFormat(" , UPDATEUSER = '{0}'", arguments[i].UpdateUser);
                    tmpSql.AppendFormat(" , UPDATETIME = '{0}'", arguments[i].UpdateTime);
                    tmpSql.AppendFormat(" WHERE  MESSAGETYPE = '{0}'", arguments[i].MessageType);
                    tmpSql.AppendFormat("AND MESSAGENAME= '{0}'", arguments[i].MessageName);
                    RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                           tmpSql.ToString(), false);
                    querys.Add(tmpSql.ToString());
                }
                this.ExecutingValue = db.Save(querys);

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
