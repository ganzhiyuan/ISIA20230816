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
    class UserWorkFlow : TAP.Remoting.Server.Biz.BizComponentBase
    {
        #region Cons

        private string _region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;

        #endregion

        public void GetMainEQ(WorkFlowArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("SELECT * FROM TAPFTMAINEQP WHERE 1 = 1 ");
                tmpSql.AppendFormat("AND REGION = '{0}'", _region);

                if (!string.IsNullOrEmpty(arguments.Facility))
                {
                    tmpSql.AppendFormat(" AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments.Facility, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Line))
                {
                    tmpSql.AppendFormat(" AND LINE IN ({0})", Utils.MakeSqlQueryIn(arguments.Line, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Area))
                {
                    tmpSql.AppendFormat(" AND AREA IN ({0})", Utils.MakeSqlQueryIn(arguments.Area, ','));
                }
                //if (!string.IsNullOrEmpty(arguments.Bay))
                //{
                //    tmpSql.AppendFormat(" AND BAY IN ({0})", Utils.MakeSqlQueryIn(arguments.Bay, ','));
                //}
                tmpSql.Append(" AND BAY IN ('BAY')");
                if (!string.IsNullOrEmpty(arguments.MainEquipment))
                {
                    tmpSql.AppendFormat(" AND NAME IN ({0})", Utils.MakeSqlQueryIn(arguments.MainEquipment, ','));
                }
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
        
        public void GetGroupEQPList(WorkFlowArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("SELECT A.*,B.EQUIPMENTGROUP FROM TAPFTEQUIPMENT A LEFT JOIN TAPFTPMEQUIPMENTGROUPMEMBER B ON A.NAME = B.NAME ");
                tmpSql.AppendFormat("WHERE A.REGION = '{0}'", _region);

                if (!string.IsNullOrEmpty(arguments.Facility))
                {
                    tmpSql.AppendFormat(" AND A.FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments.Facility, ','));
                }                
                if (!string.IsNullOrEmpty(arguments.EqpGroup))
                {
                    tmpSql.AppendFormat(" AND B.EQUIPMENTGROUP IN ({0})", Utils.MakeSqlQueryIn(arguments.EqpGroup, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Model))
                {
                    tmpSql.AppendFormat(" AND A.EQUIPMENTSMODEL IN ({0})", Utils.MakeSqlQueryIn(arguments.Model, ','));
                }

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
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
        public void GetEQ(WorkFlowArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("SELECT * FROM TAPFTEQUIPMENT WHERE 1 = 1 ");
                tmpSql.AppendFormat("AND REGION = '{0}'", _region);

                if (!string.IsNullOrEmpty(arguments.Facility))
                {
                    tmpSql.AppendFormat(" AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments.Facility, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Line))
                {
                    tmpSql.AppendFormat(" AND LINE IN ({0})", Utils.MakeSqlQueryIn(arguments.Line, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Area))
                {
                    tmpSql.AppendFormat(" AND AREA IN ({0})", Utils.MakeSqlQueryIn(arguments.Area, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Model))
                {
                    tmpSql.AppendFormat(" AND EQUIPMENTSMODEL IN ({0})", Utils.MakeSqlQueryIn(arguments.Model, ','));
                }
                //if (!string.IsNullOrEmpty(arguments.Bay))
                //{
                //    tmpSql.AppendFormat(" AND BAY IN ({0})", Utils.MakeSqlQueryIn(arguments.Bay, ','));
                //}
                tmpSql.Append(" AND BAY IN ('BAY')");
                if (!string.IsNullOrEmpty(arguments.MainEquipment))
                {
                    tmpSql.AppendFormat(" AND MAINEQUIPMENT IN ({0})", Utils.MakeSqlQueryIn(arguments.MainEquipment, ','));
                }
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
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

        public void GetUserWorkFlow(WorkFlowArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("SELECT A.*, B.USERNAME FROM TAPFTUSERWORKFLOW A, TAPUTUSERS B WHERE A.WORKER = B.NAME ");
                tmpSql.AppendFormat("AND A.REGION = '{0}'", _region);

                if (!string.IsNullOrEmpty(arguments.Facility))
                {
                    tmpSql.AppendFormat(" AND A.FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments.Facility, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Line))
                {
                    tmpSql.AppendFormat(" AND A.LINE IN ({0})", Utils.MakeSqlQueryIn(arguments.Line, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Area))
                {
                    tmpSql.AppendFormat(" AND A.AREA IN ({0})", Utils.MakeSqlQueryIn(arguments.Area, ','));
                }
                //if (!string.IsNullOrEmpty(arguments.Bay))
                //{
                //    tmpSql.AppendFormat(" AND BAY IN ({0})", Utils.MakeSqlQueryIn(arguments.Bay, ','));
                //}
                tmpSql.Append(" AND A.BAY IN ('BAY')");
                //if (!string.IsNullOrEmpty(arguments.MainEquipment))
                //{
                //    tmpSql.AppendFormat(" AND A.MAINEQUIPMENT IN ({0})", Utils.MakeSqlQueryIn(arguments.MainEquipment, ','));
                //}
                if (!string.IsNullOrEmpty(arguments.EquipmentType))
                {
                    tmpSql.AppendFormat(" AND A.EQUIPMENTTYPE IN ({0})", Utils.MakeSqlQueryIn(arguments.EquipmentType, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Equipment))
                {
                    tmpSql.AppendFormat(" AND A.EQUIPMENT IN ({0})", Utils.MakeSqlQueryIn(arguments.Equipment, ','));
                }

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
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

        public void GetUserList(WorkFlowArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("SELECT * FROM TAPUTUSERS WHERE 1 = 1 ");
                tmpSql.AppendFormat("AND REGION = '{0}'", _region);

                if (!string.IsNullOrEmpty(arguments.Position))
                {
                    tmpSql.AppendFormat(" AND POSITION IN ({0})", Utils.MakeSqlQueryIn(arguments.Position, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Facility))
                {
                    tmpSql.AppendFormat(" AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments.Facility, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Department))
                {
                    tmpSql.AppendFormat(" AND DEPARTMENT IN ({0})", Utils.MakeSqlQueryIn(arguments.Department, ','));
                }
                
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_SQL, this.Requester.IP,
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

        public void DeleteUserWorkFlow(WorkFlowArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("DELETE TAPFTUSERWORKFLOW WHERE 1 = 1 ");

                if (!string.IsNullOrEmpty(arguments.Region))
                {
                    tmpSql.AppendFormat(" AND REGION IN ({0})", Utils.MakeSqlQueryIn(arguments.Region, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Facility))
                {
                    tmpSql.AppendFormat(" AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments.Facility, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Line))
                {
                    tmpSql.AppendFormat(" AND LINE IN ({0})", Utils.MakeSqlQueryIn(arguments.Line, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Area))
                {
                    tmpSql.AppendFormat(" AND AREA IN ({0})", Utils.MakeSqlQueryIn(arguments.Area, ','));
                }
                //if (!string.IsNullOrEmpty(arguments.Bay))
                //{
                //    tmpSql.AppendFormat(" AND BAY IN ({0})", Utils.MakeSqlQueryIn(arguments.Bay, ','));
                //}                
                tmpSql.Append(" AND BAY IN ('BAY')");
                if (!string.IsNullOrEmpty(arguments.MainEquipment))
                {
                    tmpSql.AppendFormat(" AND MAINEQUIPMENT IN ({0})", Utils.MakeSqlQueryIn(arguments.MainEquipment, ','));
                }
                if (!string.IsNullOrEmpty(arguments.Equipment))
                {
                    tmpSql.AppendFormat(" AND EQUIPMENT IN ({0})", Utils.MakeSqlQueryIn(arguments.Equipment, ','));
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


        public void InsertUserWorkFlow(WorkFlowArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                List<string> querys = new List<string>();

                //for (int i = 0; i < arguments.Count; i++)
                //{
                    StringBuilder tmpSql = new StringBuilder();
                  //  WorkFlowArgsPack tempArgs = (WorkFlowArgsPack)arguments[i].ArgumentValue;
                WorkFlowArgsPack tempArgs = arguments;
                    tmpSql.Append("INSERT INTO TAPFTUSERWORKFLOW (");
                    tmpSql.Append("REGION, FACILITY, LINE, AREA, BAY, MAINEQUIPMENT, EQUIPMENT, PMSCHEDULE, WORKER, WORKERROLE ) VALUES (");
                    tmpSql.AppendFormat("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                        tempArgs.Region, tempArgs.Facility, tempArgs.Line, tempArgs.Area, "BAY", tempArgs.MainEquipment,
                        tempArgs.Equipment, tempArgs.PmSchedule, tempArgs.Worker, tempArgs.WorkerRole);

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
