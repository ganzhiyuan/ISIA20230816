﻿using ISIA.COMMON;
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
    class SpecManagement : TAP.Remoting.Server.Biz.BizComponentBase
    {


        public void GetDB()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append(" SELECT ROWID RID , ROWNUM ID , DBID, RULENAME, RULENO, RULETEXT, N_VALUE , M_VALUE , ISALIVE  FROM  TAPCTSPCRULESPEC    ");


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




        public void UpdateSpec(SpecManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("UPDATE TAPCTSPCRULESPEC SET ");
                if (!string.IsNullOrEmpty(arguments.DBID))
                {
                    tmpSql.AppendFormat("  DBID = '{0}' ,", arguments.DBID);
                }
                if (!string.IsNullOrEmpty(arguments.RULENAME))
                {
                    tmpSql.AppendFormat("  RULENAME = '{0}' ,", arguments.RULENAME);
                }
                if (!string.IsNullOrEmpty(arguments.RULENO))
                {
                    tmpSql.AppendFormat("  RULENO = '{0}' ,", arguments.RULENO);
                }
                if (!string.IsNullOrEmpty(arguments.RULETEXT))
                {
                    tmpSql.AppendFormat("  RULETEXT = '{0}' ,", arguments.RULETEXT);
                }
                if (!string.IsNullOrEmpty(arguments.N_VALUE))
                {
                    tmpSql.AppendFormat("  N_VALUE = '{0}' ,", arguments.N_VALUE);
                }
                if (!string.IsNullOrEmpty(arguments.M_VALUE))
                {
                    tmpSql.AppendFormat("  M_VALUE = '{0}' ,", arguments.M_VALUE);
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


        public void NewSPEC(SpecManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("Insert INTO TAPCTSPCRULESPEC (  ");



                if (!string.IsNullOrEmpty(arguments.DBID))
                {
                    tmpSql.AppendFormat("  DBID ,");
                }
                if (!string.IsNullOrEmpty(arguments.RULENAME))
                {
                    tmpSql.AppendFormat("  RULENAME ,");
                }
                if (!string.IsNullOrEmpty(arguments.RULENO))
                {
                    tmpSql.AppendFormat("  RULENO ,");
                }
                if (!string.IsNullOrEmpty(arguments.RULETEXT))
                {
                    tmpSql.AppendFormat("  RULETEXT ,");
                }
                if (!string.IsNullOrEmpty(arguments.N_VALUE))
                {
                    tmpSql.AppendFormat("  N_VALUE ,");
                }
                if (!string.IsNullOrEmpty(arguments.M_VALUE))
                {
                    tmpSql.AppendFormat("  M_VALUE ,");
                }
                if (!string.IsNullOrEmpty(arguments.ISALIVE))
                {
                    tmpSql.AppendFormat("  CUSTOM03 ,");
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
                if (!string.IsNullOrEmpty(arguments.RULENAME))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.RULENAME);
                }
                if (!string.IsNullOrEmpty(arguments.RULENO))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.RULENO);
                }
                if (!string.IsNullOrEmpty(arguments.RULETEXT))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.RULETEXT);
                }
                if (!string.IsNullOrEmpty(arguments.N_VALUE))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.N_VALUE);
                }
                if (!string.IsNullOrEmpty(arguments.M_VALUE))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.M_VALUE);
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




        public void DelteSpec(DataBaseManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("DELETE FROM TAPCTDATABASE WHERE ");

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
