using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISIA.COMMON;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System.Data;
using System.Reflection;
using TAP;
using TAP.Data.DataBase.Communicators;
using TAP.Remoting;

namespace ISIA.BIZ.ADMINISTRATOE
{
    public class SubMenu : TAP.Remoting.Server.Biz.BizComponentBase
    {

        public void GetSubMenu(MainMenuArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT * FROM TAPSTBSUBMENU WHERE 1 = 1 AND ISALIVE='YES'");
                if (!string.IsNullOrEmpty(arguments.MAINMENU))
                {
                    tmpSql.AppendFormat(" AND MAINMENU IN ({0})", Utils.MakeSqlQueryIn(arguments.MAINMENU, ','));
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

        public void SaveSubMenu(MainMenuArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {

                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("INSERT INTO TAPSTBSUBMENU(MDI,NAME,REGION,FACILITY,DISPLAYNAME,SHORTCUTKEY,SHORTCUTDISPLAYSTRING,SHORTCUTCHARACTER,SHORTCUTKEYS,DESCRIPTION,INSERTTIME,ISALIVE,INSERTUSER,SEQUENCES,IMAGE,MAINMENU) VALUES(");
                tmpSql.AppendFormat("'{0}',", arguments.MDI);
                tmpSql.AppendFormat("'{0}',", arguments.NAME);
                tmpSql.AppendFormat("'{0}',", arguments.REGION);
                tmpSql.AppendFormat("'{0}',", arguments.FACILITY);
                tmpSql.AppendFormat("'{0}',", arguments.DISPLAYNAME);
                tmpSql.AppendFormat("'{0}',", arguments.SHORTCUTKEY);
                tmpSql.AppendFormat("'{0}',", arguments.SHORTCUTDISPLAYSTRING);
                tmpSql.AppendFormat("'{0}',", arguments.SHORTCUTCHARACTER);
                tmpSql.AppendFormat("'{0}',", arguments.SHORTCUTKEYS);
                tmpSql.AppendFormat("'{0}',", arguments.DESCRIPTION);
                tmpSql.AppendFormat("'{0}',", arguments.INSERTTIME);
                tmpSql.AppendFormat("'{0}',", arguments.ISALIVE);
                tmpSql.AppendFormat("'{0}',", arguments.INSERTUSER);
                tmpSql.AppendFormat("'{0}',", arguments.SEQUENCES);
                tmpSql.AppendFormat("'{0}',", arguments.ICON);
                tmpSql.AppendFormat("'{0}'", arguments.MAINMENU);
                tmpSql.AppendFormat(")");


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                     tmpSql.ToString(), false);

                string[] query = { tmpSql.ToString() };

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
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

        public void UpdateSubMenu(MainMenuArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("UPDATE TAPSTBSUBMENU  SET ");

                tmpSql.AppendFormat(" DISPLAYNAME='{0}',", arguments.DISPLAYNAME);
                tmpSql.AppendFormat("SHORTCUTKEY='{0}',", arguments.SHORTCUTKEY);
                tmpSql.AppendFormat("SHORTCUTCHARACTER='{0}',", arguments.SHORTCUTCHARACTER);
                tmpSql.AppendFormat("SHORTCUTDISPLAYSTRING='{0}',", arguments.SHORTCUTDISPLAYSTRING);   
                tmpSql.AppendFormat("SHORTCUTKEYS='{0}',", arguments.SHORTCUTKEYS);
                tmpSql.AppendFormat("IMAGE='{0}',", arguments.ICON);
                tmpSql.AppendFormat("DESCRIPTION='{0}',", arguments.DESCRIPTION);
                tmpSql.AppendFormat("UPDATETIME='{0}',", arguments.UPDATETIME1);
                tmpSql.AppendFormat("UPDATEUSER='{0}'", arguments.UPDATEUSER);

                tmpSql.Append(" WHERE 1=1 ");
                tmpSql.AppendFormat(" AND MDI = '{0}'", arguments.MDI);
                tmpSql.AppendFormat(" AND MAINMENU = '{0}'", arguments.MAINMENU);
                tmpSql.AppendFormat(" AND NAME = '{0}'", arguments.NAME);
                tmpSql.AppendFormat(" AND REGION = '{0}'", arguments.REGION);
                tmpSql.AppendFormat(" AND FACILITY = '{0}'", arguments.FACILITY);


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

        public void DeleteSubMenu(MainMenuArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("UPDATE TAPSTBSUBMENU  SET ");
                tmpSql.AppendFormat(" ISALIVE='NO'");

                tmpSql.Append(" WHERE 1=1 ");
                tmpSql.AppendFormat(" AND MDI = '{0}'", arguments.MDI);
                tmpSql.AppendFormat(" AND MAINMENU = '{0}'", arguments.MAINMENU);
                tmpSql.AppendFormat(" AND NAME = '{0}'", arguments.NAME);
                tmpSql.AppendFormat(" AND REGION = '{0}'", arguments.REGION);
                tmpSql.AppendFormat(" AND FACILITY = '{0}'", arguments.FACILITY);


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
      
    }


}
