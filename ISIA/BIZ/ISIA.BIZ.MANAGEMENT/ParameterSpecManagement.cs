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


        public void GetIdName()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append(" SELECT  PARAMETERTYPE, TO_CHAR(PARAMETERID) PARAMETERID ,PARAMETERNAME  FROM TAPCTPARAMETERDEF WHERE PARAMETERTYPE IN ('STATISTIC','METRIC')   ORDER BY PARAMETERNAME  ");
               
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

        public void GetParameterType(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.AppendFormat(" SELECT * FROM TAPCTPARAMETERDEF WHERE 1=1 AND  PARAMETERNAME = '{0}' AND PARAMETERID = '{1}' " , arguments.ParameterName, arguments.ParameterId);

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



        public void GetDB(ParameterSpecManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("  SELECT ROWID, ROWNUM ID, DBID,INSTANCE_NUMBER, PARAMETERID, PARAMETERNAME, RULENAME, RULENO, DAYS,TARGET,STD_VALUE, SPECUPPERLIMIT, SPECLOWERLIMIT, CONTROLUPPERLIMIT, ");
                tmpSql.Append("  CONTROLLOWERLIMIT ,PARAVAL1,PARAVAL2,PARAVAL3,PARAVAL4,PARAVAL5, DETECTINGUSED, CHARTUSED , MAILUSED ,MMSUSED , SPECLIMITUSED , ISALIVE FROM  TAPCTPARAMETERRULESPEC WHERE 1=1 ");
                if (!string.IsNullOrEmpty(arguments.PARAMETERNAME))
                {
                    tmpSql.AppendFormat(" and PARAMETERNAME='{0}' ", arguments.PARAMETERNAME);
                }
                if (!string.IsNullOrEmpty(arguments.RULENAME))
                {
                    tmpSql.AppendFormat(" and RULENAME='{0}' ", arguments.RULENAME);
                }
                if (!string.IsNullOrEmpty(arguments.DBID))
                {
                    tmpSql.AppendFormat(" and DBID='{0}' ", arguments.DBID);
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

        public void CheckTcode(ParameterSpecManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                StringBuilder sbSel = new StringBuilder();
                sbSel.Append("select * from TAPCTPARAMETERRULESPEC");
                sbSel.Append(" where 1=1 ");
                sbSel.AppendFormat(" and DBID='{0}' ", arguments.DBID);
                sbSel.AppendFormat(" and INSTANCE_NUMBER = '{0}' ", arguments.INSTANCE_NUMBER);
                sbSel.AppendFormat(" and PARAMETERID='{0}' ", arguments.PARAMETERID);
                sbSel.AppendFormat(" and RULENAME='{0}' ", arguments.RULENAME);
                sbSel.AppendFormat(" and RULENO='{0}' ", arguments.RULENO);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                      sbSel.ToString(), false);

                this.ExecutingValue = db.Select(sbSel.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void UpdateTcode(ParameterSpecManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("UPDATE TAPCTPARAMETERRULESPEC SET ");
                tmpSql.AppendFormat("  DBID = '{0}' ,", arguments.DBID);
                tmpSql.AppendFormat("  INSTANCE_NUMBER = '{0}' ,", arguments.INSTANCE_NUMBER);

                tmpSql.AppendFormat("  PARAMETERID = '{0}' ,", arguments.PARAMETERID);
                tmpSql.AppendFormat("  PARAMETERNAME = '{0}' ,", arguments.PARAMETERNAME);
                tmpSql.AppendFormat("  RULENAME = '{0}' ,", arguments.RULENAME);
                tmpSql.AppendFormat("  RULENO = '{0}' ,", arguments.RULENO);
                tmpSql.AppendFormat("  DAYS = '{0}' ,", arguments.DAYS);
                tmpSql.AppendFormat("  SPECUPPERLIMIT = '{0}'  ,", arguments.SPECUPPERLIMIT);
                tmpSql.AppendFormat("  SPECLOWERLIMIT = '{0}' , ", arguments.SPECLOWERLIMIT);
                tmpSql.AppendFormat("  CONTROLUPPERLIMIT = '{0}'  ,", arguments.CONTROLUPPERLIMIT);
                tmpSql.AppendFormat("  CONTROLLOWERLIMIT = '{0}'  ,", arguments.CONTROLLOWERLIMIT);
                tmpSql.AppendFormat("  TARGET = {0}  ,", Convert.ToDecimal(arguments.TARGET));
                tmpSql.AppendFormat("  STD_VALUE = '{0}'  ,", arguments.STD_VALUE);
                tmpSql.AppendFormat("  PARAVAL1 = '{0}'  ,", arguments.PARAVAL1);
                tmpSql.AppendFormat("  PARAVAL2 = '{0}'  ,", arguments.PARAVAL2);
                tmpSql.AppendFormat("  PARAVAL3 = '{0}'  ,", arguments.PARAVAL3);
                tmpSql.AppendFormat("  PARAVAL4 = '{0}'  ,", arguments.PARAVAL4);
                tmpSql.AppendFormat("  PARAVAL5 = '{0}'  ,", arguments.PARAVAL5);

                tmpSql.AppendFormat("  CHARTUSED = '{0}'  ,", arguments.CHARTUSED);
                tmpSql.AppendFormat("  DETECTINGUSED = '{0}'  ,", arguments.DETECTINGUSED);
                tmpSql.AppendFormat("  MAILUSED = '{0}'  ,", arguments.MAILUSED);
                tmpSql.AppendFormat("  MMSUSED = '{0}'  ,", arguments.MMSUSED);
                tmpSql.AppendFormat("  SPECLIMITUSED = '{0}'  ,", arguments.SPECLIMITUSED);
                tmpSql.AppendFormat("  ISALIVE = '{0}' ", arguments.ISALIVE);
                tmpSql.Append(" where 1=1 ");
                tmpSql.AppendFormat(" and DBID='{0}' ", arguments.DBID);
                tmpSql.AppendFormat(" and INSTANCE_NUMBER = '{0}' ", arguments.INSTANCE_NUMBER);
                tmpSql.AppendFormat(" and PARAMETERID='{0}' ", arguments.PARAMETERID);
                tmpSql.AppendFormat(" and RULENAME='{0}' ", arguments.RULENAME);
                tmpSql.AppendFormat(" and RULENO='{0}' ", arguments.RULENO);

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
        public void SaveTCode(ParameterSpecManagementArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("Insert INTO TAPCTPARAMETERRULESPEC (DBID,INSTANCE_NUMBER,PARAMETERID,PARAMETERNAME,RULENAME,RULENO,DAYS,TARGET,SPECUPPERLIMIT,SPECLOWERLIMIT,");
                tmpSql.Append("CONTROLUPPERLIMIT,CONTROLLOWERLIMIT,STD_VALUE,PARAVAL1,PARAVAL2,PARAVAL3,PARAVAL4,PARAVAL5,DETECTINGUSED,CHARTUSED,MAILUSED,MMSUSED,SPECLIMITUSED,ISALIVE) values (  ");
                tmpSql.AppendFormat(" '{0}',", arguments.DBID);
                tmpSql.AppendFormat(" '{0}',", arguments.INSTANCE_NUMBER);
                tmpSql.AppendFormat(" '{0}',", arguments.PARAMETERID);
                tmpSql.AppendFormat(" '{0}',", arguments.PARAMETERNAME);
                tmpSql.AppendFormat(" '{0}',", arguments.RULENAME);
                tmpSql.AppendFormat(" '{0}',", arguments.RULENO);
                tmpSql.AppendFormat(" '{0}',", arguments.DAYS);
                tmpSql.AppendFormat(" {0},", Convert.ToDecimal(arguments.TARGET));
                tmpSql.AppendFormat(" '{0}',", arguments.SPECUPPERLIMIT);
                
                
                tmpSql.AppendFormat(" '{0}',", arguments.SPECLOWERLIMIT);
                tmpSql.AppendFormat(" '{0}',", arguments.CONTROLUPPERLIMIT);
                tmpSql.AppendFormat(" '{0}',", arguments.CONTROLLOWERLIMIT);
                tmpSql.AppendFormat(" '{0}',", arguments.STD_VALUE);
                tmpSql.AppendFormat(" '{0}',", arguments.PARAVAL1);
                tmpSql.AppendFormat(" '{0}',", arguments.PARAVAL2);
                tmpSql.AppendFormat(" '{0}',", arguments.PARAVAL3);
                tmpSql.AppendFormat(" '{0}',", arguments.PARAVAL4);
                tmpSql.AppendFormat(" '{0}',", arguments.PARAVAL5);
                tmpSql.AppendFormat(" '{0}',", arguments.DETECTINGUSED);
                tmpSql.AppendFormat(" '{0}',", arguments.CHARTUSED);
                tmpSql.AppendFormat(" '{0}',", arguments.MAILUSED);
                tmpSql.AppendFormat(" '{0}',", arguments.MMSUSED);
                tmpSql.AppendFormat(" '{0}',", arguments.SPECLIMITUSED);
                tmpSql.AppendFormat(" '{0}' )", arguments.ISALIVE);

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

        public void GetParaValue(AwrCommonArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();


                if (arguments.ParameterType == "STATISTIC")
                {
                    tmpSql.Append(" SELECT T.SNAP_ID,T.DBID, T.stat_name PARAMENT_NAME, (TO_NUMBER (t.VALUE) - t.next_value) N_VALUE,t.end_interval_time from  (");

                    tmpSql.AppendFormat(@" select A.* ,B.end_interval_time ,
                    lag(a.value, 1, null) over (partition by a.stat_name order by a.snap_id )  next_value  from  RAW_DBA_HIST_SYSSTAT_{0}  A 
                    left join RAW_DBA_HIST_SNAPSHOT_{0} ", arguments.DbName);

                    tmpSql.AppendFormat(@" B on A.snap_id = b.snap_id 
                    AND A.INSTANCE_NUMBER = b.INSTANCE_NUMBER
                    AND A.DBID = b.DBID
                    where 1=1 and b.end_interval_time>to_date('{0}','yyyy-MM-dd HH24:mi:ss')
                    and  b.end_interval_time <= to_date('{1}','yyyy-MM-dd HH24:mi:ss' ) ", arguments.StartTimeKey, arguments.EndTimeKey);

                    tmpSql.AppendFormat(@" AND A.DBID IN ('{0}') ", arguments.DbId);

                    tmpSql.AppendFormat(@" AND A.INSTANCE_NUMBER = {0} ", arguments.InstanceNumber);

                    tmpSql.AppendFormat(@" and  stat_name in ( {0})  order by B.end_interval_time ) T ", Utils.MakeSqlQueryIn2(arguments.ParameterName));
                }
                else if (arguments.ParameterType == "OS")
                {
                    tmpSql.Append(" SELECT T.SNAP_ID,T.DBID, T.stat_name PARAMENT_NAME, (TO_NUMBER (t.VALUE) - t.next_value) N_VALUE,t.end_interval_time from  (");

                    tmpSql.AppendFormat(@" select A.* ,B.end_interval_time ,
                    lag(a.value, 1, null) over (partition by a.stat_name order by a.snap_id )  next_value  from  RAW_DBA_HIST_OSSTAT_{0}  A 
                    left join RAW_DBA_HIST_SNAPSHOT_{0} ", arguments.DbName);

                    tmpSql.AppendFormat(@" B on A.snap_id = b.snap_id
                    AND A.INSTANCE_NUMBER = b.INSTANCE_NUMBER
                    AND A.DBID = b.DBID
                    where 1=1 and b.end_interval_time>to_date('{0}','yyyy-MM-dd HH24:mi:ss')
                    and  b.end_interval_time <= to_date('{1}','yyyy-MM-dd HH24:mi:ss' ) ", arguments.StartTimeKey, arguments.EndTimeKey);

                    tmpSql.AppendFormat(@" AND A.DBID = ('{0}') ", arguments.DbId);

                    tmpSql.AppendFormat(@" AND A.INSTANCE_NUMBER = {0} ", arguments.InstanceNumber);

                    tmpSql.AppendFormat(@" and  stat_name in ( {0})  order by B.end_interval_time ) T ", Utils.MakeSqlQueryIn2(arguments.ParameterName));
                }
                else if (arguments.ParameterType == "METRIC")
                {
                    tmpSql.AppendFormat(@"  SELECT A.SNAP_ID,A.DBID, A.METRIC_NAME PARAMENT_NAME, A.AVERAGE N_VALUE,
                    B.end_interval_time FROM RAW_DBA_HIST_SYSMETRIC_SUMMARY_{0} A LEFT JOIN RAW_DBA_HIST_SNAPSHOT_{0} B
                    ON A.snap_id = b.snap_id 
                    AND A.INSTANCE_NUMBER = b.INSTANCE_NUMBER
                    AND A.DBID = b.DBID
                    ", arguments.DbName);

                    tmpSql.AppendFormat(@" WHERE     1 = 1
                    AND b.end_interval_time >
                    TO_DATE ('{0}', 'yyyy-MM-dd HH24:mi:ss')
                    AND b.end_interval_time <=
                    TO_DATE ('{1}', 'yyyy-MM-dd HH24:mi:ss') ", arguments.StartTimeKey, arguments.EndTimeKey);

                    tmpSql.AppendFormat(@" AND A.DBID IN ('{0}') ", arguments.DbId);

                    tmpSql.AppendFormat(@" AND A.INSTANCE_NUMBER  = {0} ", arguments.InstanceNumber);

                    tmpSql.AppendFormat(@" AND metric_name IN ( {0} ) order by a.metric_name ,B.end_interval_time", Utils.MakeSqlQueryIn2(arguments.ParameterName));
                }
                else
                {

                    tmpSql.Append("select * from (");

                    tmpSql.Append(" SELECT T.SNAP_ID,T.DBID, T.stat_name PARAMENT_NAME, (TO_NUMBER (t.VALUE) - t.next_value) N_VALUE,t.end_interval_time from  (");

                    tmpSql.AppendFormat(@" select A.* ,B.end_interval_time ,
                    lag(a.value, 1, null) over (partition by a.stat_name order by a.snap_id )  next_value  from  RAW_DBA_HIST_SYSSTAT_{0}  A 
                    left join RAW_DBA_HIST_SNAPSHOT_{0} ", arguments.DbName);

                    tmpSql.AppendFormat(@" B on A.snap_id = b.snap_id  
                    AND A.INSTANCE_NUMBER = b.INSTANCE_NUMBER
                    AND A.DBID = b.DBID
                    where 1=1 and b.end_interval_time>to_date('{0}','yyyy-MM-dd HH24:mi:ss')
                    and  b.end_interval_time <= to_date('{1}','yyyy-MM-dd HH24:mi:ss' ) ", arguments.StartTimeKey, arguments.EndTimeKey);

                    tmpSql.AppendFormat(@" AND A.DBID IN '{0}' ", arguments.DbId);

                    tmpSql.AppendFormat(@" AND A.INSTANCE_NUMBER  = {0} ", arguments.InstanceNumber);

                    tmpSql.AppendFormat(@" and  stat_name in ( {0})  ) T ", Utils.MakeSqlQueryIn2(arguments.ParameterName));

                    tmpSql.Append(" union all");

                    tmpSql.AppendFormat(@"  SELECT A.SNAP_ID,A.DBID, A.METRIC_NAME PARAMENT_NAME, A.AVERAGE N_VALUE,
                    B.end_interval_time FROM RAW_DBA_HIST_SYSMETRIC_SUMMARY_{0} A LEFT JOIN RAW_DBA_HIST_SNAPSHOT_{0} B
                    ON A.snap_id = b.snap_id 
                    AND A.INSTANCE_NUMBER = b.INSTANCE_NUMBER
                    AND A.DBID = b.DBID ", arguments.DbName);

                    tmpSql.AppendFormat(@"  WHERE     1 = 1
                    AND b.end_interval_time >
                    TO_DATE ('{0}', 'yyyy-MM-dd HH24:mi:ss')
                    AND b.end_interval_time <=
                    TO_DATE ('{1}', 'yyyy-MM-dd HH24:mi:ss') ", arguments.StartTimeKey, arguments.EndTimeKey);

                    tmpSql.AppendFormat(@" AND A.DBID IN '{0}' ", arguments.DbId);

                    tmpSql.AppendFormat(@" AND A.INSTANCE_NUMBER  = {0} ", arguments.InstanceNumber);

                    tmpSql.AppendFormat(@" AND metric_name IN ( {0} ) ", Utils.MakeSqlQueryIn2(arguments.ParameterName));

                    tmpSql.Append(" union all");

                    //OS
                    tmpSql.Append(" SELECT T.SNAP_ID,T.DBID, T.stat_name PARAMENT_NAME, (TO_NUMBER (t.VALUE) - t.next_value) N_VALUE,t.end_interval_time from  ( ");

                    tmpSql.AppendFormat(@" select A.* ,B.end_interval_time ,
                    lag(a.value, 1, null) over (partition by a.stat_name order by a.snap_id )  next_value  from  RAW_DBA_HIST_OSSTAT_{0}  A 
                    left join RAW_DBA_HIST_SNAPSHOT_{0} ", arguments.DbName);

                    tmpSql.AppendFormat(@" B on A.snap_id = b.snap_id
                    AND A.DBID = b.DBID 
                    AND A.INSTANCE_NUMBER = b.INSTANCE_NUMBER where 1=1 and b.end_interval_time>to_date('{0}','yyyy-MM-dd HH24:mi:ss')
                    and  b.end_interval_time <= to_date('{1}','yyyy-MM-dd HH24:mi:ss' ) ", arguments.StartTimeKey, arguments.EndTimeKey);

                    tmpSql.AppendFormat(@" AND A.DBID IN '{0}' ", arguments.DbId);

                    tmpSql.AppendFormat(@" AND A.INSTANCE_NUMBER = {0} ", arguments.InstanceNumber);

                    tmpSql.AppendFormat(@" and  stat_name in ( {0})  order by B.end_interval_time ) T ", Utils.MakeSqlQueryIn2(arguments.ParameterName));


                    tmpSql.Append(" ) order by parament_name ,end_interval_time ");

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




    }
}
