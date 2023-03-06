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
    class DataBaseManagement : TAP.Remoting.Server.Biz.BizComponentBase
    {


        public void GetDB()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("SELECT ROWID ID , CATEGORY , SUBCATEGORY ,NAME , USED , CUSTOM01 ,CUSTOM02,CUSTOM03,CUSTOM04,CUSTOM05,CUSTOM06,CUSTOM07   FROM TAPCTCODES  ");


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


        public void UpdateTCODE(DataBaseManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("UPDATE TAPCTCODES SET ");
                if (!string.IsNullOrEmpty(arguments.CATEGORY))
                {
                    tmpSql.AppendFormat("  CATEGORY = '{0}' ,", arguments.CATEGORY);
                }
                if (!string.IsNullOrEmpty(arguments.SUBCATEGORY))
                {
                    tmpSql.AppendFormat("  SUBCATEGORY = '{0}' ,", arguments.SUBCATEGORY);
                }
                if (!string.IsNullOrEmpty(arguments.NAME))
                {
                    tmpSql.AppendFormat("  NAME = '{0}' ,", arguments.NAME);
                }
                if (!string.IsNullOrEmpty(arguments.USED))
                {
                    tmpSql.AppendFormat("  USED = '{0}' ,", arguments.USED);
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM01))
                {
                    tmpSql.AppendFormat("  CUSTOM01 = '{0}' ,", arguments.CUSTOM01);
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM02))
                {
                    tmpSql.AppendFormat("  CUSTOM02 = '{0}' ,", arguments.CUSTOM02);
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM03))
                {
                    tmpSql.AppendFormat("  CUSTOM03 = '{0}' ,", arguments.CUSTOM03);
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM04))
                {
                    tmpSql.AppendFormat("  CUSTOM04 = '{0}' ,", arguments.CUSTOM04);
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM05))
                {
                    tmpSql.AppendFormat("  CUSTOM05 = '{0}' ,", arguments.CUSTOM05);
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM06))
                {
                    tmpSql.AppendFormat("  CUSTOM06 = '{0}' ,", arguments.CUSTOM06);
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM07))
                {
                    tmpSql.AppendFormat("  CUSTOM07 = '{0}' ", arguments.CUSTOM06);
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


        public void NewTCODE(DataBaseManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("Insert INTO TAPCTCODES (  ");



                if (!string.IsNullOrEmpty(arguments.CATEGORY))
                {
                    tmpSql.AppendFormat("  CATEGORY ,");
                }
                if (!string.IsNullOrEmpty(arguments.SUBCATEGORY))
                {
                    tmpSql.AppendFormat("  SUBCATEGORY ,");
                }
                if (!string.IsNullOrEmpty(arguments.NAME))
                {
                    tmpSql.AppendFormat("  NAME ,");
                }
                if (!string.IsNullOrEmpty(arguments.USED))
                {
                    tmpSql.AppendFormat("  USED ,");
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM01))
                {
                    tmpSql.AppendFormat("  CUSTOM01 ,");
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM02))
                {
                    tmpSql.AppendFormat("  CUSTOM02 ,");
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM03))
                {
                    tmpSql.AppendFormat("  CUSTOM03 ,");
                }

                if (!string.IsNullOrEmpty(arguments.CUSTOM04))
                {
                    tmpSql.Append("  CUSTOM04 ,");
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM05))
                {
                    tmpSql.Append("  CUSTOM05 ,");
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM06))
                {
                    tmpSql.Append("  CUSTOM06 ,");
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM07))
                {
                    tmpSql.Append("  CUSTOM07 ,");
                }
               
                if (tmpSql.ToString().Substring(tmpSql.Length - 1, 1).Equals(","))
                {
                    tmpSql = new StringBuilder(tmpSql.ToString().Substring(0, tmpSql.Length - 1));
                }

                tmpSql.Append(") values (");

                if (!string.IsNullOrEmpty(arguments.CATEGORY))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.CATEGORY);
                }
                if (!string.IsNullOrEmpty(arguments.SUBCATEGORY))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.SUBCATEGORY);
                }
                if (!string.IsNullOrEmpty(arguments.NAME))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.NAME);
                }
                if (!string.IsNullOrEmpty(arguments.USED))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.USED);
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM01))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.CUSTOM01);
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM02))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.CUSTOM02);
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM03))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.CUSTOM03);
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM04))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.CUSTOM04);
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM05))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.CUSTOM05);
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM06))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.CUSTOM06);
                }
                if (!string.IsNullOrEmpty(arguments.CUSTOM07))
                {
                    tmpSql.AppendFormat("'{1}'" + "{0}", " ,", arguments.CUSTOM07);
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




        public void DelteTCODE(DataBaseManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("DELETE FROM TAPCTCODES WHERE ");

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
