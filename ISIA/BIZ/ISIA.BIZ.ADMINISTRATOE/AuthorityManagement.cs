using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TAP;
using TAP.Data.DataBase.Communicators;
using TAP.Models.Factories.Facilities;
using TAP.Models.UIBasic;
using TAP.Remoting;

namespace ISIA.BIZ.ADMINISTRATOE
{
    public class AuthorityManagement : TAP.Remoting.Server.Biz.BizComponentBase
    {
        public void GetUIName()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.AppendFormat("  SELECT * FROM TAPSTBUI WHERE MDI='ISIA' ");
                
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
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

        public void UpdateAuthority(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("UPDATE TAPSTBUIAUTHORITY SET \n");
                tmpSql.AppendFormat(" FACILITY = '{0}' \n", arguments.Facility);

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

        public void DeleteAuthority(CommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("  DELETE FROM TAPSTBUIAUTHORITY WHERE ISALIVE='YES' \n");

                tmpSql.AppendFormat(" AND NAME = '{0}' \n", arguments.Name);
                tmpSql.AppendFormat(" AND MEMBERTYPE = '{0}' \n", arguments.MemberType);
                tmpSql.AppendFormat(" AND MDI = '{0}' \n", arguments.MDI);

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

        public void SaveAuthority(CommonArgsPack arguments)
        {
            UIBasicDefaultInfo uiBasicDefaultInfo = new UIBasicDefaultInfo();
            UIAuthorityBasicModel uiAuthorityBasicModel = new UIAuthorityBasicModel();
            try
            {
                for (int i = 0; i < arguments.Dt.Rows.Count; i++)
                {
                    uiBasicDefaultInfo.MDI = arguments.MDI;
                    uiBasicDefaultInfo.MainMenu = arguments.Dt.Rows[i]["MAINMENU"].ToString();
                    uiBasicDefaultInfo.UI = arguments.Dt.Rows[i]["UI"].ToString();
                    uiBasicDefaultInfo.Container = "BASED";
                    uiBasicDefaultInfo.UIFunction = "SAVE";
                    uiBasicDefaultInfo.UIAuthority = arguments.UserID;

                    uiBasicDefaultInfo.Region = arguments.Region;
                    uiBasicDefaultInfo.Facility = "T1";

                    uiAuthorityBasicModel = new UIAuthorityBasicModel(uiBasicDefaultInfo);
                    uiAuthorityBasicModel.CommandType = EnumCommandType.COMMAND;
                    uiAuthorityBasicModel.MemberType = arguments.MemberType;
                    uiAuthorityBasicModel.InsertUser = arguments.InsertUser;
                    this.ExecutingValue = uiAuthorityBasicModel.Save(arguments.InsertUser);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
