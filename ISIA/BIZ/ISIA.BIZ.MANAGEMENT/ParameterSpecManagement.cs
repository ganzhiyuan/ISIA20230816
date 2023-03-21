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

namespace ISIA.BIZ.MANAGEMENT
{
    class ParameterSpecManagement : TAP.Remoting.Server.Biz.BizComponentBase
    {


        public void GetDB()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("  SELECT ROWID RID, ROWNUM ID, DBID, PARAMETERID, PARAMETERNAME, RULENAME, RULENO, DAYS, SPECUPPERLIMIT, SPECLOWERLIMIT, CONTROLUPPERLIMIT, ");
                tmpSql.Append("  CONTROLLOWERLIMIT , CHARTUSED , MAILUSED ,MMSUSED , SPECLIMITUSED , ISALIVE FROM  TAPCTPARAMETERRULESPEC WHERE 1=1 ");


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



        public void GetTCODE()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("  SELECT DISTINCT DBNAME ,DBID FROM TAPCTDATABASE");
                tmpSql.Append("  WHERE 1=1");

                
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

        public void GetParameterdef()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("  SELECT * FROM  TAPCTPARAMETERDEF");
                tmpSql.Append("  WHERE 1=1   ORDER BY PARAMETERNAME ");


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




        public void UpdateParameterSpec(ParameterSpecManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("UPDATE TAPCTPARAMETERRULESPEC SET ");
                if (!string.IsNullOrEmpty(arguments.DBID))
                {
                    tmpSql.AppendFormat("  DBID = '{0}' ,", arguments.DBID);
                }
                if (!string.IsNullOrEmpty(arguments.PARAMETERID))
                {
                    tmpSql.AppendFormat("  PARAMETERID = '{0}' ,", arguments.PARAMETERID);
                }
                if (!string.IsNullOrEmpty(arguments.PARAMETERNAME))
                {
                    tmpSql.AppendFormat("  PARAMETERNAME = '{0}' ,", arguments.PARAMETERNAME);
                }
                if (!string.IsNullOrEmpty(arguments.RULENAME))
                {
                    tmpSql.AppendFormat("  RULENAME = '{0}' ,", arguments.RULENAME);
                }
                if (!string.IsNullOrEmpty(arguments.RULENO))
                {
                    tmpSql.AppendFormat("  RULENO = '{0}' ,", arguments.RULENO);
                }
                if (!string.IsNullOrEmpty(arguments.DAYS))
                {
                    tmpSql.AppendFormat("  DAYS = '{0}' ,", arguments.DAYS);
                }
                if (!string.IsNullOrEmpty(arguments.SPECUPPERLIMIT))
                {
                    tmpSql.AppendFormat("  SPECUPPERLIMIT = '{0}' ,", arguments.SPECUPPERLIMIT);
                }
                if (!string.IsNullOrEmpty(arguments.SPECLOWERLIMIT))
                {
                    tmpSql.AppendFormat("  SPECLOWERLIMIT = '{0}' ,", arguments.SPECLOWERLIMIT);
                }
                if (!string.IsNullOrEmpty(arguments.CONTROLUPPERLIMIT))
                {
                    tmpSql.AppendFormat("  CONTROLUPPERLIMIT = '{0}' ,", arguments.CONTROLUPPERLIMIT);
                }
                if (!string.IsNullOrEmpty(arguments.CONTROLLOWERLIMIT))
                {
                    tmpSql.AppendFormat("  CONTROLLOWERLIMIT = '{0}' ,", arguments.CONTROLLOWERLIMIT);
                }
                if (!string.IsNullOrEmpty(arguments.CHARTUSED))
                {
                    tmpSql.AppendFormat("  CHARTUSED = '{0}' ,", arguments.CHARTUSED);
                }
                if (!string.IsNullOrEmpty(arguments.MAILUSED))
                {
                    tmpSql.AppendFormat("  MAILUSED = '{0}' ,", arguments.MAILUSED);
                }
                if (!string.IsNullOrEmpty(arguments.MMSUSED))
                {
                    tmpSql.AppendFormat("  MMSUSED = '{0}' ,", arguments.MMSUSED);
                }
                if (!string.IsNullOrEmpty(arguments.SPECLIMITUSED))
                {
                    tmpSql.AppendFormat("  SPECLIMITUSED = '{0}' ,", arguments.SPECLIMITUSED);
                }
                if (!string.IsNullOrEmpty(arguments.ISALIVE))
                {
                    tmpSql.AppendFormat("  ISALIVE = '{0}' ", arguments.ISALIVE);
                }
                
                tmpSql.AppendFormat(" WHERE ROWID ='{0}'", arguments.ROWID);
                
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


        public void NewParameterSpec(ParameterSpecManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("Insert INTO TAPCTPARAMETERRULESPEC (  ");

                if (!string.IsNullOrEmpty(arguments.DBID))
                {
                    tmpSql.AppendFormat("  DBID ,");
                }
                if (!string.IsNullOrEmpty(arguments.PARAMETERID))
                {
                    tmpSql.AppendFormat("  PARAMETERID ,");
                }
                if (!string.IsNullOrEmpty(arguments.PARAMETERNAME))
                {
                    tmpSql.AppendFormat("  PARAMETERNAME ,");
                }
                if (!string.IsNullOrEmpty(arguments.RULENAME))
                {
                    tmpSql.AppendFormat("  RULENAME ,");
                }
                if (!string.IsNullOrEmpty(arguments.RULENO))
                {
                    tmpSql.AppendFormat("  RULENO ,");
                }
                if (!string.IsNullOrEmpty(arguments.DAYS))
                {
                    tmpSql.AppendFormat("  DAYS ,");
                }
                if (!string.IsNullOrEmpty(arguments.SPECUPPERLIMIT))
                {
                    tmpSql.AppendFormat("  SPECUPPERLIMIT ,");
                }

                if (!string.IsNullOrEmpty(arguments.SPECLOWERLIMIT))
                {
                    tmpSql.Append("  SPECLOWERLIMIT ,");
                }
                if (!string.IsNullOrEmpty(arguments.CONTROLUPPERLIMIT))
                {
                    tmpSql.Append("  CONTROLUPPERLIMIT ,");
                }
                if (!string.IsNullOrEmpty(arguments.CONTROLLOWERLIMIT))
                {
                    tmpSql.Append("  CONTROLLOWERLIMIT ,");
                }
                if (!string.IsNullOrEmpty(arguments.CHARTUSED))
                {
                    tmpSql.Append("  CHARTUSED ,");
                }
                if (!string.IsNullOrEmpty(arguments.MAILUSED))
                {
                    tmpSql.Append("  MAILUSED ,");
                }
                if (!string.IsNullOrEmpty(arguments.MMSUSED))
                {
                    tmpSql.Append("  MMSUSED ,");
                }
                if (!string.IsNullOrEmpty(arguments.SPECLIMITUSED))
                {
                    tmpSql.Append("  SPECLIMITUSED ,");
                }
                if (!string.IsNullOrEmpty(arguments.ISALIVE))
                {
                    tmpSql.Append("  ISALIVE ,");
                }

                if (tmpSql.ToString().Substring(tmpSql.Length - 1, 1).Equals(","))
                {
                    tmpSql = new StringBuilder(tmpSql.ToString().Substring(0, tmpSql.Length - 1));
                }

                tmpSql.Append(") values (");

                if (!string.IsNullOrEmpty(arguments.DBID))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.DBID);
                }
                if (!string.IsNullOrEmpty(arguments.PARAMETERID))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.PARAMETERID);
                }
                if (!string.IsNullOrEmpty(arguments.PARAMETERNAME))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.PARAMETERNAME);
                }
                if (!string.IsNullOrEmpty(arguments.RULENAME))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.RULENAME);
                }
                if (!string.IsNullOrEmpty(arguments.RULENO))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.RULENO);
                }
                if (!string.IsNullOrEmpty(arguments.DAYS))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.DAYS);
                }
                if (!string.IsNullOrEmpty(arguments.SPECUPPERLIMIT))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.SPECUPPERLIMIT);
                }
                if (!string.IsNullOrEmpty(arguments.SPECLOWERLIMIT))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.SPECLOWERLIMIT);
                }
                if (!string.IsNullOrEmpty(arguments.CONTROLUPPERLIMIT))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.CONTROLUPPERLIMIT);
                }
                if (!string.IsNullOrEmpty(arguments.CONTROLLOWERLIMIT))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.CONTROLLOWERLIMIT);
                }
                if (!string.IsNullOrEmpty(arguments.CHARTUSED))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.CHARTUSED);
                }
                if (!string.IsNullOrEmpty(arguments.MAILUSED))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.MAILUSED);
                }
                if (!string.IsNullOrEmpty(arguments.MMSUSED))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.MMSUSED);
                }
                if (!string.IsNullOrEmpty(arguments.SPECLIMITUSED))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.SPECLIMITUSED);
                }
                if (!string.IsNullOrEmpty(arguments.ISALIVE))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.ISALIVE);
                }


                if (tmpSql.ToString().Substring(tmpSql.Length - 1, 1).Equals(","))
                {
                    tmpSql = new StringBuilder(tmpSql.ToString().Substring(0, tmpSql.Length - 1));
                }


                tmpSql.Append(" )");

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




        public void DelteParameterSpec(ParameterSpecManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("DELETE FROM TAPCTPARAMETERRULESPEC WHERE ");

                if (!string.IsNullOrEmpty(arguments.ROWID))
                {
                    tmpSql.AppendFormat("ROWID =  '{0}'",  arguments.ROWID);
                }
                else {
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
        public void GetParmNameByType(AwrArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat("select parametername from TAPIAPARAMETERLIST where parametertype in ({0})", Utils.MakeSqlQueryIn2(arguments.ParamType));


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

        
        

      
       
    }
}
