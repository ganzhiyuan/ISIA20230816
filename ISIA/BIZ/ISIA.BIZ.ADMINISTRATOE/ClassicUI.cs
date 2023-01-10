using ISIA.COMMON;
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
    public class ClassicUI : TAP.Remoting.Server.Biz.BizComponentBase
    {
        public void GetUI(ClassicUIArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT *  FROM  TAPSTBUI WHERE 1 = 1  AND ISALIVE ='YES'");

                //if (!string.IsNullOrEmpty(arguments.Facility))
                //{
                //    tmpSql.AppendFormat(" AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments.Facility, ','));
                //}

                if (!string.IsNullOrEmpty(arguments.MDI))
                {
                    tmpSql.AppendFormat(" AND MDI IN ({0})", Utils.MakeSqlQueryIn(arguments.MDI, ','));
                }
                if (!string.IsNullOrEmpty(arguments.MainMenu))
                {
                    tmpSql.AppendFormat(" AND MAINMENU IN ({0})", Utils.MakeSqlQueryIn(arguments.MainMenu, ','));
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
        public void UpdateUI(ClassicUIArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {            
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("UPDATE TAPSTBUI SET   ");

                tmpSql.AppendFormat("  UILAYOUT = '{0}'", arguments.UILayout);
                tmpSql.AppendFormat("  ,UITYPE = '{0}'", arguments.UIType);
                tmpSql.AppendFormat("  ,DISPLAYNAME = '{0}'", arguments.DisplayName);
                tmpSql.AppendFormat("  ,ASSEMBLYFILENAME = '{0}'", arguments.AssemblyFileName);
                tmpSql.AppendFormat("  ,ASSEMBLYNAME = '{0}'", arguments.AssemblyName);
                tmpSql.AppendFormat("  ,SUBTYPE = '{0}'", arguments.SubType);
                tmpSql.AppendFormat("  ,IMAGENAME = '{0}'", arguments.ImageName);
                tmpSql.AppendFormat("  ,SMALLIMAGENAME = '{0}'", arguments.SmallImageName);
                tmpSql.AppendFormat("  ,ICON = '{0}' ", arguments.Icon);
                tmpSql.AppendFormat("  ,DESCRIPTION = '{0}' ", arguments.Description);
                tmpSql.AppendFormat("  ,LASTEVENTCOMMENT = '{0}' ", arguments.LastEventComment);
                tmpSql.AppendFormat("  ,UPDATETIME = '{0}' ", arguments.UpdateTime);
                tmpSql.AppendFormat("  ,UPDATEUSER = '{0}' ", arguments.UpdateUser);
                tmpSql.AppendFormat("  ,SUBMENU = '{0}' ", arguments.SubMenu);
                tmpSql.Append("WHERE 1 = 1 ");
                tmpSql.AppendFormat(" AND MDI = '{0}'", arguments.MDI);
                tmpSql.AppendFormat(" AND MAINMENU = '{0}'", arguments.MainMenu);
                tmpSql.AppendFormat(" AND NAME = '{0}'", arguments.Name);
                tmpSql.AppendFormat(" AND REGION = '{0}'", arguments.Region);
                tmpSql.AppendFormat(" AND FACILITY = '{0}'", arguments.Facility);


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);

                string[] query = { tmpSql.ToString() };
                this.ExecutingValue = db.Save(query);
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void AddUI(ClassicUIArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
     
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("INSERT INTO  TAPSTBUI (");
                tmpSql.Append("MDI,MAINMENU,NAME,REGION,FACILITY,UILAYOUT,DISPLAYNAME,ASSEMBLYFILENAME,ASSEMBLYNAME,DESCRIPTION,LASTEVENTTIME,IMAGENAME,SMALLIMAGENAME  ");
                tmpSql.Append(" ,INSERTTIME,INSERTUSER,ISALIVE,SUBMENU");
                tmpSql.Append(")VALUES(");
                tmpSql.AppendFormat("'{0}'", arguments.MDI);
                tmpSql.AppendFormat(",'{0}'", arguments.MainMenu);
                tmpSql.AppendFormat(",'{0}'", arguments.Name);
                tmpSql.AppendFormat(",'{0}'", arguments.Region);
                tmpSql.AppendFormat(",'{0}'", arguments.Facility);
                tmpSql.AppendFormat(",'{0}'", arguments.UILayout);
                tmpSql.AppendFormat(",'{0}'", arguments.DisplayName);
                tmpSql.AppendFormat(",'{0}'", arguments.AssemblyFileName);
                tmpSql.AppendFormat(",'{0}'", arguments.AssemblyName);
                tmpSql.AppendFormat(",'{0}'", arguments.Description);
                tmpSql.AppendFormat(",'{0}'", arguments.LastEventTime);
                tmpSql.AppendFormat(",'{0}'", arguments.ImageName);
                tmpSql.AppendFormat(",'{0}'", arguments.SmallImageName);
                tmpSql.AppendFormat(",'{0}'", arguments.InsertTime);
                tmpSql.AppendFormat(",'{0}'", arguments.InsertUser);
                tmpSql.AppendFormat(",'{0}'", arguments.ISALIVE);
                tmpSql.AppendFormat(",'{0}'", arguments.SubMenu);
                tmpSql.Append(")");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);
                string[] query = { tmpSql.ToString() };
                this.ExecutingValue = db.Save(query);
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void DeleteBUI(ClassicUIArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("UPDATE TAPSTBUI  SET ");
                tmpSql.AppendFormat(" ISALIVE='NO'");

                tmpSql.Append(" WHERE 1=1 ");
                tmpSql.AppendFormat(" AND MDI = '{0}'", arguments.MDI);
                tmpSql.AppendFormat(" AND NAME = '{0}'", arguments.Name);
                tmpSql.AppendFormat(" AND REGION = '{0}'", arguments.Region);
                tmpSql.AppendFormat(" AND FACILITY = '{0}'", arguments.Facility);
                tmpSql.AppendFormat(" AND MAINMENU = '{0}'", arguments.MainMenu);


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

        public void AddTapstbuiAuthorty(ClassicUIArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {

                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("INSERT INTO  TAPSTBUIAUTHORITY (");
                tmpSql.Append("MDI, MAINMENU, UI, CONTAINER, UIFUNCTION, NAME, REGION, FACILITY, COMMANDTYPE, MEMBERTYPE, SHORTCUTKEY, SHORTCUTCHARACTER, SHORTCUTDISPLAYSTRING, SHORTCUTKEYS, ICON, CURRENTMODEL, DESCRIPTION, LASTEVENTCOMMENT, LASTEVENT, LASTEVENTFLAG, LASTEVENTTIME, LASTEVENTCODE, LASTJOBCODE, INSERTTIME, UPDATETIME, INSERTUSER, UPDATEUSER, SEQUENCES, ISALIVE, MODELLEVELS");             
                tmpSql.Append(")VALUES(");
                tmpSql.AppendFormat("'{0}'", arguments.MDI);
                tmpSql.AppendFormat(",'{0}'", arguments.MainMenu);
                tmpSql.AppendFormat(",'{0}'", arguments.Name);
                tmpSql.AppendFormat(", 'BASED', 'SAVE', 'MUDURI', 'CELL', 'ALL', 'COMMAND', 'USER', 'NONE', '', '', 'A', CAST(0 AS Numeric(38, 0)), 'YES', '', '', 'CREATE', 'D', '00010101000000', '', '', '20201208112128', '20201208112128', 'Muduri', 'Muduri', CAST(0 AS Numeric(38, 0)), 'YES', NULL");

                tmpSql.Append(")");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                       tmpSql.ToString(), false);
                string[] query = { tmpSql.ToString() };
                this.ExecutingValue = db.Save(query);
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
