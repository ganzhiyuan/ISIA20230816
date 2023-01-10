using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TAP;
using TAP.Data.DataBase.Communicators;
using TAP.Remoting;
using TAP.Models.Factories.Facilities;
using TAP.Models.Codes;
using System.Data;
using ISIA.COMMON;
using ISIA.INTERFACE.ARGUMENTSPACK;

namespace ISIA.BIZ.ADMINISTRATOE
{
    class ShiftManagement : TAP.Remoting.Server.Biz.BizComponentBase
    {
        #region Cons

        private string _region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;

        #endregion

        public void GetShift(ShiftArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("SELECT * FROM TAPFTSHIFT WHERE 1 = 1 ");
                tmpSql.AppendFormat("AND REGION = '{0}'", _region);
                               
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Select(tmpSql.ToString()).Tables[0];
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format("Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetWorkGroup(WorkGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("SELECT * FROM TAPFTWORKGROUP WHERE 1 = 1 ");
                tmpSql.AppendFormat("AND REGION = '{0}'", _region);
                tmpSql.Append(" ORDER BY SEQUENCES");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Select(tmpSql.ToString()).Tables[0];
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format("Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetWorkGroupUser(WorkGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("SELECT A.*,B.USERNAME FROM TAPFTWORKGROUPMEMBER A, TAPUTUSERS B WHERE A.USERID = B.NAME ");
                tmpSql.AppendFormat("AND A.REGION = '{0}'", _region);

                if (!string.IsNullOrEmpty(arguments.Name))
                {
                    tmpSql.AppendFormat(" AND A.GROUPNAME IN ({0})", Utils.MakeSqlQueryIn(arguments.Name, ','));
                }

                tmpSql.Append(" ORDER BY A.USERID");

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
                       tmpSql.ToString(), false);

                this.ExecutingValue = db.Select(tmpSql.ToString()).Tables[0];
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format("Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void UpdateWorkGroupSeqUp(WorkGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<string> querys = new List<string>();
                StringBuilder tmpSql = new StringBuilder();
                StringBuilder tmpSql2 = new StringBuilder();

                WorkGroupArgsPack tempArgs = arguments;

                tmpSql.Append("UPDATE TAPFTWORKGROUP SET ");
                tmpSql.AppendFormat("SEQUENCES = {0} WHERE REGION = '{1}' AND SEQUENCES={2}-1",
                    tempArgs.Sequences, tempArgs.Region, tempArgs.Sequences);

                tmpSql2.Append("UPDATE TAPFTWORKGROUP SET ");
                tmpSql2.AppendFormat("SEQUENCES = {0}-1 WHERE REGION = '{1}' AND SEQUENCES={2} AND NAME='{3}'",
                    tempArgs.Sequences, tempArgs.Region, tempArgs.Sequences, tempArgs.Name);

                querys.Add(tmpSql.ToString());
                querys.Add(tmpSql2.ToString());

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
                   tmpSql.ToString(), false);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
                   tmpSql2.ToString(), false);
                //}

                this.ExecutingValue = db.Save(querys);
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void UpdateWorkGroupSeqDown(WorkGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<string> querys = new List<string>();
                StringBuilder tmpSql = new StringBuilder();
                StringBuilder tmpSql2 = new StringBuilder();

                WorkGroupArgsPack tempArgs = arguments;

                tmpSql.Append("UPDATE TAPFTWORKGROUP SET ");
                tmpSql.AppendFormat("SEQUENCES = {0} WHERE REGION = '{1}' AND SEQUENCES={2}+1",
                    tempArgs.Sequences, tempArgs.Region, tempArgs.Sequences);

                tmpSql2.Append("UPDATE TAPFTWORKGROUP SET ");
                tmpSql2.AppendFormat("SEQUENCES = {0}+1 WHERE REGION = '{1}' AND SEQUENCES={2} AND NAME='{3}'",
                    tempArgs.Sequences, tempArgs.Region, tempArgs.Sequences, tempArgs.Name);

                querys.Add(tmpSql.ToString());
                querys.Add(tmpSql2.ToString());

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
                   tmpSql.ToString(), false);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
                   tmpSql2.ToString(), false);
                //}

                this.ExecutingValue = db.Save(querys);
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void InsertWorkGroup(WorkGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<string> querys = new List<string>();

                //for (int i = 0; i < arguments.Count; i++)
                //{
                StringBuilder tmpSql = new StringBuilder();
                //  WorkFlowArgsPack tempArgs = (WorkFlowArgsPack)arguments[i].ArgumentValue;
                WorkGroupArgsPack tempArgs = arguments;
                tmpSql.Append("INSERT INTO TAPFTWORKGROUP (");
                tmpSql.Append("REGION, NAME, SHIFT, DESCRIPTION, SEQUENCES, INSERTTIME, INSERTUSER, ISALIVE ) VALUES (");
#if MSSQL
                tmpSql.AppendFormat("'{0}', '{1}', '{2}', '{3}',(select isnull(max(SEQUENCES)+1,0) from TAPFTWORKGROUP),'{4}', '{5}', '{6}')",
                    tempArgs.Region, tempArgs.Name, tempArgs.Shift, tempArgs.Description, tempArgs.InsertTime, tempArgs.InsertUser, tempArgs.IsAlive);
#endif
#if ORACLE
                //ORACLE
                tmpSql.AppendFormat("'{0}', '{1}', '{2}', '{3}',(select NVL(max(SEQUENCES)+1,0) from TAPFTWORKGROUP),'{4}', '{5}', '{6}')",
                    tempArgs.Region, tempArgs.Name, tempArgs.Shift, tempArgs.Description, tempArgs.InsertTime, tempArgs.InsertUser, tempArgs.IsAlive);
#endif
                querys.Add(tmpSql.ToString());

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
                   tmpSql.ToString(), false);

                //}

                this.ExecutingValue = db.Save(querys);
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void InsertWorkGroupUser(WorkGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<string> querys = new List<string>();

                //for (int i = 0; i < arguments.Count; i++)
                //{
                StringBuilder tmpSql = new StringBuilder();
                //  WorkFlowArgsPack tempArgs = (WorkFlowArgsPack)arguments[i].ArgumentValue;
                WorkGroupArgsPack tempArgs = arguments;
                tmpSql.Append("INSERT INTO TAPFTWORKGROUPMEMBER (");
                tmpSql.Append("REGION, GROUPNAME, USERID, INSERTTIME, INSERTUSER, ISALIVE ) VALUES (");
                tmpSql.AppendFormat("'{0}', '{1}', '{2}','{3}','{4}', '{5}')",
                    tempArgs.Region, tempArgs.Name, tempArgs.UserId, tempArgs.InsertTime, tempArgs.InsertUser, tempArgs.IsAlive);

                querys.Add(tmpSql.ToString());

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
                   tmpSql.ToString(), false);

                //}

                this.ExecutingValue = db.Save(querys);
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void DeleteWorkGroupUser(WorkGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("DELETE TAPFTWORKGROUPMEMBER WHERE 1 = 1 ");

                if (!string.IsNullOrEmpty(arguments.Region))
                {
                    tmpSql.AppendFormat(" AND REGION IN ({0})", Utils.MakeSqlQueryIn(arguments.Region, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Name))
                {
                    tmpSql.AppendFormat(" AND GROUPNAME IN ({0})", Utils.MakeSqlQueryIn(arguments.Name, ','));
                }
                if (!string.IsNullOrEmpty(arguments.UserId))
                {
                    tmpSql.AppendFormat(" AND USERID IN ({0})", Utils.MakeSqlQueryIn(arguments.UserId, ','));
                }

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
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


        public void DeleteWorkGroup(WorkGroupArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                StringBuilder tmpSql2 = new StringBuilder();
                StringBuilder tmpSql3 = new StringBuilder();
                List<string> querys = new List<string>();

                tmpSql.Append("DELETE TAPFTWORKGROUP WHERE 1 = 1 ");

                if (!string.IsNullOrEmpty(arguments.Region))
                {
                    tmpSql.AppendFormat(" AND REGION IN ({0})", Utils.MakeSqlQueryIn(arguments.Region, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Name))
                {
                    tmpSql.AppendFormat(" AND NAME IN ({0})", Utils.MakeSqlQueryIn(arguments.Name, ','));
                }

                tmpSql2.Append("UPDATE TAPFTWORKGROUP SET ");
                tmpSql2.AppendFormat("SEQUENCES = SEQUENCES-1 WHERE REGION = '{0}' AND SEQUENCES > {1}",arguments.Region, arguments.Sequences);

                tmpSql3.Append("DELETE TAPFTWORKGROUPMEMBER WHERE 1 = 1 ");

                if (!string.IsNullOrEmpty(arguments.Region))
                {
                    tmpSql3.AppendFormat(" AND REGION IN ({0})", Utils.MakeSqlQueryIn(arguments.Region, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Name))
                {
                    tmpSql3.AppendFormat(" AND GROUPNAME IN ({0})", Utils.MakeSqlQueryIn(arguments.Name, ','));
                }


                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
                       tmpSql.ToString(), false);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
                       tmpSql2.ToString(), false);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
                       tmpSql3.ToString(), false);

                querys.Add(tmpSql.ToString());
                querys.Add(tmpSql2.ToString());
                querys.Add(tmpSql3.ToString());

                this.ExecutingValue = db.Save(querys);
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void DeleteShift(ShiftArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("DELETE TAPFTSHIFT WHERE 1 = 1 ");

                if (!string.IsNullOrEmpty(arguments.Region))
                {
                    tmpSql.AppendFormat(" AND REGION IN ({0})", Utils.MakeSqlQueryIn(arguments.Region, ','));
                }                

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
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


        public void InsertShift(ShiftArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<string> querys = new List<string>();

                //for (int i = 0; i < arguments.Count; i++)
                //{
                    StringBuilder tmpSql = new StringBuilder();
                //  WorkFlowArgsPack tempArgs = (WorkFlowArgsPack)arguments[i].ArgumentValue;
                ShiftArgsPack tempArgs = arguments;
                    tmpSql.Append("INSERT INTO TAPFTSHIFT (");
                    tmpSql.Append("REGION, SHIFTCOUNT, GROUPCOUNT, SHIFTSTARTDATE, SHIFTSTARTTIME, SHIFTINTERVALTIME, SHIFTINTERVALDAY, SHIFTINTERVALOPTION, INSERTTIME, INSERTUSER, ISALIVE ) VALUES (");
                    tmpSql.AppendFormat("'{0}', {1}, {2},'{3}','{4}', {5}, {6},'{7}','{8}','{9}','{10}')",
                        tempArgs.Region, tempArgs.ShiftCount, tempArgs.GroupCount, tempArgs.ShiftStartDate, tempArgs.ShiftStartTime, tempArgs.ShiftIntervalTime,
                        tempArgs.ShiftIntervalDay, tempArgs.ShiftIntervalOption, tempArgs.InsertTime, tempArgs.InsertUser, tempArgs.IsAlive);

                    querys.Add(tmpSql.ToString());

                    RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
                       tmpSql.ToString(), false);

                //}
                
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
