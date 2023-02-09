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
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace ISIA.BIZ.COMMON
{
    class ComboBoxData : TAP.Remoting.Server.Biz.BizComponentBase
    {
        #region Cons
        private string _region = TAP.Base.Configuration.ConfigurationManager.Instance.EnvironmentSection.Region;
        #endregion

        public void GetFab(ArgumentPack arguments)
        {
            #region Bind Facility
            FacilityCodeModel tmpFacilities = null;

            try
            {
                tmpFacilities = new FacilityCodeModel(_region, EnumFlagYN.YES);

                string[] tmpArr = tmpFacilities.CodeList.CreateSeqenceArray();

                this.ExecutingValue = Utils.ArrayToDataSet(tmpArr, "NAME");
                return;
            }
            catch (System.Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
            #endregion           
        }

        public void GetFab()
        {
            #region Bind Facility
            FacilityCodeModel tmpFacilities = null;

            try
            {
                tmpFacilities = new FacilityCodeModel(_region, EnumFlagYN.YES);

                string[] tmpArr = tmpFacilities.CodeList.CreateSeqenceArray();

                this.ExecutingValue = Utils.ArrayToDataSet(tmpArr, "NAME");
                return;
            }
            catch (System.Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
            #endregion
        }

        public void GetRegion()
        {
            try
            {
                DataTable tmpdt = new DataTable();

                tmpdt.Columns.Add("NAME", typeof(string));
                tmpdt.Rows.Add(_region);

                this.ExecutingValue = Utils.DataTableToDataSet(tmpdt);

                return;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetLine(ArgumentPack arguments)
        {
            LineModel tmpLine = null;

            try
            {
                arguments.AddArgument("REGION", typeof(string), _region);

                tmpLine = new LineModel();

                DataTable tmpdt = tmpLine.LoadModelDataList(new ArgumentPack());
                StringBuilder tmpsb = new StringBuilder();
                tmpsb.AppendFormat("REGION IN ('{0}') ", _region);
                if (!string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                {
                    tmpsb.AppendFormat("AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments["FACILITY"].ArgumentValue.ToString(), ','));
                }
                tmpdt.DefaultView.RowFilter = tmpsb.ToString();

                this.ExecutingValue = Utils.DataTableToDataSet(tmpdt.DefaultView.ToTable(true, new string[] { "NAME" }));

                return;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetLine()
        {
            LineModel tmpLine = null;

            try
            {
                tmpLine = new LineModel();

                DataTable tmpdt = tmpLine.LoadModelDataList(new ArgumentPack());
                StringBuilder tmpsb = new StringBuilder();
                //tmpsb.AppendFormat("REGION IN ('{0}') ", _region);

                tmpdt.DefaultView.RowFilter = tmpsb.ToString();

                this.ExecutingValue = Utils.DataTableToDataSet(tmpdt.DefaultView.ToTable(true, new string[] { "NAME" }));

                return;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetArea(ArgumentPack arguments)
        {
            AreaModel tmpArea = null;

            try
            {
                arguments.AddArgument("REGION", typeof(string), _region);

                tmpArea = new AreaModel();

                DataTable tmpdt = tmpArea.LoadModelDataList(new ArgumentPack());
                StringBuilder tmpsb = new StringBuilder();
                tmpsb.AppendFormat("REGION IN ('{0}') ", _region);

                if (arguments.IsContainArgument("FACILITY"))
                {
                    if (!string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                    {
                        tmpsb.AppendFormat("AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments["FACILITY"].ArgumentValue.ToString(), ','));
                    }
                }

                if (!string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                {
                    tmpsb.AppendFormat("AND LINE IN ({0})", Utils.MakeSqlQueryIn(arguments["LINE"].ArgumentValue.ToString(), ','));
                }

                tmpdt.DefaultView.RowFilter = tmpsb.ToString();

                this.ExecutingValue = Utils.DataTableToDataSet(tmpdt.DefaultView.ToTable(true, new string[] { "NAME" }));

                return;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetBay(ArgumentPack arguments)
        {
            BayModel tmpBay = null;

            try
            {
                arguments.AddArgument("REGION", typeof(string), _region);

                tmpBay = new BayModel();

                DataTable tmpdt = tmpBay.LoadModelDataList(new ArgumentPack());
                StringBuilder tmpsb = new StringBuilder();
                tmpsb.AppendFormat("REGION IN ('{0}') ", _region);
                if (!string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                {
                    tmpsb.AppendFormat("AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments["FACILITY"].ArgumentValue.ToString(), ','));
                }
                if (!string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                {
                    tmpsb.AppendFormat("AND LINE IN ({0})", Utils.MakeSqlQueryIn(arguments["LINE"].ArgumentValue.ToString(), ','));
                }
                if (!string.IsNullOrEmpty((string)arguments["AREA"].ArgumentValue))
                {
                    tmpsb.AppendFormat("AND AREA IN ({0})", Utils.MakeSqlQueryIn(arguments["AREA"].ArgumentValue.ToString(), ','));
                }

                tmpdt.DefaultView.RowFilter = tmpsb.ToString();

                this.ExecutingValue = Utils.DataTableToDataSet(tmpdt.DefaultView.ToTable(true, new string[] { "NAME" }));

                return;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetMainEQ()
        {
            MainEquipmentModel tmpMainEQ = null;

            try
            {
                tmpMainEQ = new MainEquipmentModel();
                DataTable tmpdt = tmpMainEQ.LoadModelDataList(new ArgumentPack());
                StringBuilder tmpsb = new StringBuilder();
                tmpsb.AppendFormat("REGION IN ('{0}') ", _region);
                tmpsb.Append("AND BAY = 'BAY' ");
                tmpdt.DefaultView.RowFilter = tmpsb.ToString();
                this.ExecutingValue = Utils.DataTableToDataSet(tmpdt.DefaultView.ToTable(true, new string[] { "NAME" }));

                return;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetMainEQ(ArgumentPack arguments)
        {
            MainEquipmentModel tmpMainEQ = null;

            try
            {
                arguments.AddArgument("REGION", typeof(string), _region);

                tmpMainEQ = new MainEquipmentModel();

                DataTable tmpdt = tmpMainEQ.LoadModelDataList(new ArgumentPack());
                StringBuilder tmpsb = new StringBuilder();
                tmpsb.AppendFormat("REGION IN ('{0}') ", _region);

                if (arguments.IsContainArgument("FACILITY") && !string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                {
                    tmpsb.AppendFormat("AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments["FACILITY"].ArgumentValue.ToString(), ','));
                }
                if (arguments.IsContainArgument("LINE") && !string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                {
                    tmpsb.AppendFormat("AND LINE IN ({0})", Utils.MakeSqlQueryIn(arguments["LINE"].ArgumentValue.ToString(), ','));
                }
                if (arguments.IsContainArgument("AREA") && !string.IsNullOrEmpty((string)arguments["AREA"].ArgumentValue))
                {
                    tmpsb.AppendFormat("AND AREA IN ({0})", Utils.MakeSqlQueryIn(arguments["AREA"].ArgumentValue.ToString(), ','));
                }
                if (arguments.IsContainArgument("EQUIPMENTTYPE"))
                {
                    if (!string.IsNullOrEmpty((string)arguments["EQUIPMENTTYPE"].ArgumentValue))
                    {
                        tmpsb.AppendFormat("AND EQUIPMENTTYPE IN ({0})", Utils.MakeSqlQueryIn(arguments["EQUIPMENTTYPE"].ArgumentValue.ToString(), ','));
                    }
                }
                //if (arguments.IsContainArgument("EQMODEL"))
                //{
                //    if (!string.IsNullOrEmpty((string)arguments["EQMODEL"].ArgumentValue))
                //    {
                //        tmpsb.AppendFormat("AND EQUIPMENTMODEL IN ({0})", Utils.MakeSqlQueryIn(arguments["EQMODEL"].ArgumentValue.ToString(), ','));
                //    }
                //}
                //if (!string.IsNullOrEmpty((string)arguments["BAY"].ArgumentValue))
                //{
                //    tmpsb.AppendFormat("AND BAY IN ({0})", Utils.MakeSqlQueryIn(arguments["BAY"].ArgumentValue.ToString(), ','));
                //}
                tmpsb.Append("AND BAY = 'BAY' ");

                tmpdt.DefaultView.RowFilter = tmpsb.ToString();

                this.ExecutingValue = Utils.DataTableToDataSet(tmpdt.DefaultView.ToTable(true, new string[] { "NAME" }));

                return;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetMainEqType(ArgumentPack arguments)
        {
            MainEquipmentModel tmpMainEQ = null;

            try
            {
                arguments.AddArgument("REGION", typeof(string), _region);

                tmpMainEQ = new MainEquipmentModel();

                DataTable tmpdt = tmpMainEQ.LoadModelDataList(new ArgumentPack());
                StringBuilder tmpsb = new StringBuilder();
                tmpsb.AppendFormat("REGION IN ('{0}') ", _region);
                if (arguments.IsContainArgument("FACILITY") && !string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                {
                    tmpsb.AppendFormat("AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments["FACILITY"].ArgumentValue.ToString(), ','));
                }
                if (arguments.IsContainArgument("LINE") && !string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                {
                    tmpsb.AppendFormat("AND LINE IN ({0})", Utils.MakeSqlQueryIn(arguments["LINE"].ArgumentValue.ToString(), ','));
                }
                if (arguments.IsContainArgument("AREA") && !string.IsNullOrEmpty((string)arguments["AREA"].ArgumentValue))
                {
                    tmpsb.AppendFormat("AND AREA IN ({0})", Utils.MakeSqlQueryIn(arguments["AREA"].ArgumentValue.ToString(), ','));
                }
                //if (arguments.IsContainArgument("EQMODEL"))
                //{
                //    if (!string.IsNullOrEmpty((string)arguments["EQMODEL"].ArgumentValue))
                //    {
                //        tmpsb.AppendFormat("AND EQUIPMENTMODEL IN ({0})", Utils.MakeSqlQueryIn(arguments["EQMODEL"].ArgumentValue.ToString(), ','));
                //    }
                //}
                //if (!string.IsNullOrEmpty((string)arguments["BAY"].ArgumentValue))
                //{
                //    tmpsb.AppendFormat("AND BAY IN ({0})", Utils.MakeSqlQueryIn(arguments["BAY"].ArgumentValue.ToString(), ','));
                //}
                tmpsb.Append("AND BAY = 'BAY' ");

                tmpdt.DefaultView.RowFilter = tmpsb.ToString();

                this.ExecutingValue = Utils.DataTableToDataSet(tmpdt.DefaultView.ToTable(true, new string[] { "EQUIPMENTTYPE" }));

                return;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetEQ(ArgumentPack arguments)
        {
            EquipmentModel tmpEQ = null;

            try
            {
                arguments.AddArgument("REGION", typeof(string), _region);

                tmpEQ = new EquipmentModel();

                DataTable tmpdt = tmpEQ.LoadModelDataList(new ArgumentPack());
                StringBuilder tmpsb = new StringBuilder();
                tmpsb.AppendFormat("REGION IN ('{0}') ", _region);
                if (arguments.IsContainArgument("FACILITY"))
                {
                    if (!string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                    {
                        tmpsb.AppendFormat("AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments["FACILITY"].ArgumentValue.ToString(), ','));
                    }
                }
                if (arguments.IsContainArgument("LINE"))
                {
                    if (!string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                    {
                        tmpsb.AppendFormat("AND LINE IN ({0})", Utils.MakeSqlQueryIn(arguments["LINE"].ArgumentValue.ToString(), ','));
                    }
                }
                if (arguments.IsContainArgument("AREA"))
                {
                    if (!string.IsNullOrEmpty((string)arguments["AREA"].ArgumentValue))
                    {
                        tmpsb.AppendFormat("AND AREA IN ({0})", Utils.MakeSqlQueryIn(arguments["AREA"].ArgumentValue.ToString(), ','));
                    }
                }
                //if (!string.IsNullOrEmpty((string)arguments["BAY"].ArgumentValue))
                //{
                //    tmpsb.AppendFormat("AND BAY IN ({0})", Utils.MakeSqlQueryIn(arguments["BAY"].ArgumentValue.ToString(), ','));
                //}
                if (arguments.IsContainArgument("EQUIPMENTTYPE"))
                {
                    if (!string.IsNullOrEmpty((string)arguments["EQUIPMENTTYPE"].ArgumentValue))
                    {
                        tmpsb.AppendFormat("AND EQUIPMENTTYPE IN ({0})", Utils.MakeSqlQueryIn(arguments["EQUIPMENTTYPE"].ArgumentValue.ToString(), ','));
                    }
                }
                //if (arguments.IsContainArgument("EQMODEL"))
                //{
                //    if (!string.IsNullOrEmpty((string)arguments["EQMODEL"].ArgumentValue))
                //    {
                //        tmpsb.AppendFormat("AND EQUIPMENTSMODEL IN ({0})", Utils.MakeSqlQueryIn(arguments["EQMODEL"].ArgumentValue.ToString(), ','));
                //    }
                //}
                tmpsb.Append("AND BAY = 'BAY' ");
                if (arguments.IsContainArgument("MAINEQUIPMENT"))
                {
                    if (!string.IsNullOrEmpty((string)arguments["MAINEQUIPMENT"].ArgumentValue))
                    {
                        tmpsb.AppendFormat("AND MAINEQUIPMENT IN ({0})", Utils.MakeSqlQueryIn(arguments["MAINEQUIPMENT"].ArgumentValue.ToString(), ','));
                    }
                }

                tmpdt.DefaultView.RowFilter = tmpsb.ToString();

                this.ExecutingValue = Utils.DataTableToDataSet(tmpdt.DefaultView.ToTable(true, new string[] { "NAME" }));

                return;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetParts(ArgumentPack arguments)
        {
            PartsModel tmpEQ = null;

            try
            {
                arguments.AddArgument("REGION", typeof(string), _region);

                tmpEQ = new PartsModel();

                DataTable tmpdt = tmpEQ.LoadModelDataList(new ArgumentPack());
                StringBuilder tmpsb = new StringBuilder();
                tmpsb.AppendFormat("REGION IN ('{0}') ", _region);
                tmpsb.AppendFormat("AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments["FACILITY"].ArgumentValue.ToString(), ','));
                tmpsb.AppendFormat("AND LINE IN ({0})", Utils.MakeSqlQueryIn(arguments["LINE"].ArgumentValue.ToString(), ','));
                tmpsb.AppendFormat("AND AREA IN ({0})", Utils.MakeSqlQueryIn(arguments["AREA"].ArgumentValue.ToString(), ','));
                //tmpsb.AppendFormat("AND BAY IN ({0})", Utils.MakeSqlQueryIn(arguments["BAY"].ArgumentValue.ToString(), ','));
                tmpsb.Append("AND BAY = 'BAY' ");
                tmpsb.AppendFormat("AND MAINEQUIPMENT IN ({0})", Utils.MakeSqlQueryIn(arguments["MAINEQUIPMENT"].ArgumentValue.ToString(), ','));
                tmpsb.AppendFormat("AND EQUIPMENT IN ({0})", Utils.MakeSqlQueryIn(arguments["EQUIPMENT"].ArgumentValue.ToString(), ','));

                tmpdt.DefaultView.RowFilter = tmpsb.ToString();

                this.ExecutingValue = Utils.DataTableToDataSet(tmpdt.DefaultView.ToTable(true, new string[] { "NAME" }));

                return;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetMailGroup()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME FROM TAPUTMAILGROUP WHERE 1=1 ");
                tmpSql.AppendFormat(" AND REGION IN ('{0}')", _region);

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

        public void GetEqpGroup()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT EQUIPMENTGROUP FROM TAPFTPMEQUIPMENTGROUP WHERE 1=1 ");
                //if (!string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                //{
                //    tmpSql.AppendFormat(" AND FACILITY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACILITY"].ArgumentValue, ','));
                //}
                tmpSql.AppendFormat(" AND REGION IN ('{0}')", _region);

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

        public void GetCategory()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT CATEGORY FROM TAPCTCODES  WHERE USED='YES' ");

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
        public void GetSparePartsType()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME AS SPAREPARTSTYPE FROM TAPCTCODES WHERE CATEGORY='SPAREPARTSTYPE' AND USED='YES' ");

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
        public void GetIngotType()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME AS SPAREPARTSTYPE FROM TAPCTCODES WHERE CATEGORY='INGOT_TYPE' AND USED='YES' ");

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
        public void GetSPLine()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME AS SPAREPARTSTYPE FROM TAPCTCODES WHERE CATEGORY='SPLINE' AND USED='YES' ");

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
        public void GetSPLine(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT CUSTOM01 FROM TAPCTCODES WHERE CATEGORY='SPLINE' AND USED='YES' ");

                if (!string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND SUBCATEGORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["LINE"].ArgumentValue, ','));
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
        public void GetMessageType()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT MESSAGETYPE FROM TAPUTMESSAGEGROUP ");

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
        public void GetPMCategory()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT PMCATEGORY FROM TAPFTPMMASTER  WHERE 1=1 ");

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
        public void GetPartsVendors(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME FROM TAPFTPARTSVENDORS  WHERE 1=1 ");

                if (!string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACILITY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACILITY"].ArgumentValue, ','));
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

        public void GetPartsVendorsCategory()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT CATEGORY AS VENDORSCATEGORY FROM TAPFTPARTSVENDORS  WHERE 1=1 ");

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

        public void GetPartsVendorsOperation()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT OPERATION FROM TAPFTPARTSVENDORS  WHERE 1=1 ");

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

        public void GetPartsVendorsContry()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT CONTRY FROM TAPFTPARTSVENDORS  WHERE 1=1 ");

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

        public void GetPartsVendorsProvince(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT PROVINCE FROM TAPFTPARTSVENDORS WHERE 1=1 ");

                if (!string.IsNullOrEmpty((string)arguments["CONTRY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CONTRY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["CONTRY"].ArgumentValue, ','));
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

        public void GetPartsVendorsCity(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT CITY FROM TAPFTPARTSVENDORS WHERE 1=1 ");

                if (!string.IsNullOrEmpty((string)arguments["CONTRY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CONTRY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["CONTRY"].ArgumentValue, ','));
                }

                if (!string.IsNullOrEmpty((string)arguments["PROVINCE"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND PROVINCE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["PROVINCE"].ArgumentValue, ','));
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

        public void GetPartsType()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT PARTTYPE FROM TAPFTINVENTORYPARTS  WHERE 1=1 ");

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

        public void GetEqpPartsType()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT PARTTYPE FROM TAPFTEQPPARTS  WHERE 1=1 ");

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

        public void GetEqpGrade()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT GRADE FROM TAPFTEQPPARTS  WHERE 1=1 ");

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

        public void GetEqpPartModel()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT PARTMODEL FROM TAPFTEQPPARTS  WHERE 1=1 ");

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

        public void GetEqpStockInLocation()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT STOCKINLOCATION FROM TAPFTEQPPARTS  WHERE 1=1 ");

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

        public void GetEqpParts(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME AS PARTS FROM TAPFTEQPPARTS  WHERE 1=1 ");

                if (!string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACILITY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACILITY"].ArgumentValue, ','));
                }

                if (!string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND LINE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["LINE"].ArgumentValue, ','));
                }

                if (!string.IsNullOrEmpty((string)arguments["AREA"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND AREA IN ({0})", Utils.MakeSqlQueryIn((string)arguments["AREA"].ArgumentValue, ','));
                }

                if (!string.IsNullOrEmpty((string)arguments["MAINEQUIPMENT"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND MAINEQUIPMENT IN ({0})", Utils.MakeSqlQueryIn((string)arguments["MAINEQUIPMENT"].ArgumentValue, ','));
                }

                if (!string.IsNullOrEmpty((string)arguments["EQUIPMENT"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND EQUIPMENT IN ({0})", Utils.MakeSqlQueryIn((string)arguments["EQUIPMENT"].ArgumentValue, ','));
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

        public void GetEqpPartsEquipment(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT EQUIPMENT FROM TAPFTEQPPARTS  WHERE 1=1 ");

                if (!string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACILITY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACILITY"].ArgumentValue, ','));
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

        public void GetVendor()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT VENDOR FROM TAPFTINVENTORYPARTS  WHERE 1=1 ");

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

        public void GetPartModel()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT PARTMODEL FROM TAPFTINVENTORYPARTS  WHERE 1=1 ");

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

        public void GetInventoryName()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME AS INVENTORYNAME FROM TAPFTINVENTORYPARTS  WHERE 1=1 ");

                //if (!string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                //{
                //    tmpSql.AppendFormat(" AND FACILITY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACILITY"].ArgumentValue, ','));
                //}

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

        public void GetOAIDParts()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME FROM TAPFTPARTSOA  WHERE OAID<>'' AND (COMPANY ='' OR COMPANY IS NULL) ");

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

        public void GetSubCategory(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT SUBCATEGORY FROM TAPCTCODES WHERE USED='YES' ");

                if (!string.IsNullOrEmpty((string)arguments["CATEGORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CATEGORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["CATEGORY"].ArgumentValue, ','));
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

        public void GetName(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME FROM TAPCTCODES WHERE USED='YES' ");

                if (!string.IsNullOrEmpty((string)arguments["CATEGORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CATEGORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["CATEGORY"].ArgumentValue, ','));
                }

                if (!string.IsNullOrEmpty((string)arguments["SUBCATEGORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND SUBCATEGORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["SUBCATEGORY"].ArgumentValue, ','));
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

        public void GetPMSchedule()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME AS PMSCHEDULE FROM TAPCTCODES WHERE CATEGORY='PMSCHEDULE' ORDER BY SEQUENCES ");

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
        public void GetPMSchedule(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT PMSCHEDULE FROM TAPFTPMMASTER WHERE 1=1  ");
                if (arguments.IsContainArgument("AREA") && !string.IsNullOrEmpty((string)arguments["AREA"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND PROCESS IN ({0})", Utils.MakeSqlQueryIn(arguments["AREA"].ArgumentValue.ToString(), ','));
                }
                if (arguments.IsContainArgument("EQUIPMENTSMODEL") && !string.IsNullOrEmpty((string)arguments["EQUIPMENTSMODEL"].ArgumentValue))
                {
                    tmpSql.AppendFormat("AND PMCATEGORY IN ({0})", Utils.MakeSqlQueryIn(arguments["EQUIPMENTSMODEL"].ArgumentValue.ToString(), ','));
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
        public void GetEqpType()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME AS EQUIPMENTTYPE FROM TAPCTCODES WHERE CATEGORY='EQUIPMENTTYPE' ");

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
        public void GetEqpProcess()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME AS PROCESS FROM TAPCTCODES WHERE CATEGORY='OPER' ");

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

        public void GetEqpType(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                if (1 == 2
                //string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue) &&
                //    string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue) &&
                //    string.IsNullOrEmpty((string)arguments["AREA"].ArgumentValue) &&
                //    string.IsNullOrEmpty((string)arguments["MAINEQUIPMENT"].ArgumentValue) &&
                //    string.IsNullOrEmpty((string)arguments["EQUIPMENT"].ArgumentValue)
                )
                {
                    //tmpSql.Append("SELECT DISTINCT NAME AS EQUIPMENTTYPE FROM TAPCTCODES WHERE CATEGORY='EQUIPMENTTYPE' ");
                }
                else
                {
                    tmpSql.Append("SELECT DISTINCT EQUIPMENTTYPE FROM TAPFTEQUIPMENT WHERE ISALIVE='YES' \n");

                    if (arguments.IsContainArgument("FACILITY") && !string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                    {
                        tmpSql.AppendFormat(" AND FACILITY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACILITY"].ArgumentValue, ','));
                    }

                    if (arguments.IsContainArgument("LINE") && !string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                    {
                        tmpSql.AppendFormat(" AND LINE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["LINE"].ArgumentValue, ','));
                    }

                    if (arguments.IsContainArgument("AREA") && !string.IsNullOrEmpty((string)arguments["AREA"].ArgumentValue))
                    {
                        tmpSql.AppendFormat(" AND AREA IN ({0})", Utils.MakeSqlQueryIn((string)arguments["AREA"].ArgumentValue, ','));
                    }

                    //if (arguments .IsContainArgument("MAINEQUIPMENT") && !string.IsNullOrEmpty((string)arguments["MAINEQUIPMENT"].ArgumentValue))
                    //{
                    //    tmpSql.AppendFormat(" AND MAINEQUIPMENT IN ({0})", Utils.MakeSqlQueryIn((string)arguments["MAINEQUIPMENT"].ArgumentValue, ','));
                    //}

                    if (arguments.IsContainArgument("EQUIPMENT") && !string.IsNullOrEmpty((string)arguments["EQUIPMENT"].ArgumentValue))
                    {
                        tmpSql.AppendFormat(" AND NAME IN ({0})", Utils.MakeSqlQueryIn((string)arguments["EQUIPMENT"].ArgumentValue, ','));
                    }
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

        public void GetEqpStatus()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME AS EQUIPMENTSTATUS FROM TAPCTCODES WHERE CATEGORY='EQUIPMENTSTATUS' AND ISALIVE='YES'");

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
        /***Name Value(CATEGORY='EQUIPMENTSTATUS' AND ISALIVE='YES')
      * 1． Idle
      * 2． Process down
      * 3． Utility down
      * 4.CHEMICAL CHANGE
      * 5.BOAT PRE-COATING
      * 6.SCREEN CHANGE
      * 7.EQUIPMENT MODIFY
      **/
        public void GetEqpStatusFilter()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME AS EQUIPMENTSTATUS FROM TAPCTCODES WHERE CATEGORY='EQUIPMENTSTATUS' AND ISALIVE='YES'AND NAME IN('IDLE','PROCESS DOWN','UTILITY DOWN','CHEMICAL CHANGE','BOAT PRE-COATING','SCREEN CHANGE','EQUIPMENT MODIFY','PASTE CHANGE')");

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
        public void GetEqpEvent()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME AS EQUIPMENTEVENT FROM TAPCTCODES WHERE CATEGORY='EQUIPMENTEVENT' ");

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

        public void GetEquipmentGroup()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT EQUIPMENTGROUP FROM TAPFTEQUIPMENT");

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

        public void GetEquipmentVendor()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT VENDOR FROM TAPFTEQUIPMENT");

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

        public void GetEquipmentSerialNo()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT SERIALNO FROM TAPFTEQUIPMENT");

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

        public void GetEquipmentAssetCode()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT ASSETCODE FROM TAPFTEQUIPMENT");

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

        public void GetEquipmentFunctionLocation()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT FUNCTIONALLOCATION FROM TAPFTEQUIPMENT");

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

        public void GetEquipmentWorkOrderName()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT WORKORDERNAME FROM TAPFTEQUIPMENT");

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

        public void GetDepartment(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                arguments.AddArgument("REGION", typeof(string), _region);

                StringBuilder tmpsb = new StringBuilder();

                tmpsb.Append("SELECT DISTINCT DEPARTMENT FROM TAPUTUSERS  WHERE 1=1 and DEPARTMENT IS NOT NULL ");
                tmpsb.AppendFormat("AND REGION IN ('{0}') ", _region);
                if (!string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                {
                    tmpsb.AppendFormat("AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments["FACILITY"].ArgumentValue.ToString(), ','));
                }

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                      tmpsb.ToString(), false);

                this.ExecutingValue = db.Select(tmpsb.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetDepartment()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {

                StringBuilder tmpsb = new StringBuilder();

                tmpsb.Append("SELECT DISTINCT DEPARTMENT FROM TAPUTUSERS  WHERE 1=1 and DEPARTMENT IS NOT NULL ");
                tmpsb.AppendFormat("AND REGION IN ('{0}') ", _region);

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                      tmpsb.ToString(), false);

                this.ExecutingValue = db.Select(tmpsb.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetPosition(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                arguments.AddArgument("REGION", typeof(string), _region);

                StringBuilder tmpsb = new StringBuilder();

                tmpsb.Append("SELECT DISTINCT POSITION FROM TAPUTUSERS  WHERE 1=1 and POSITION IS NOT NULL ");
                tmpsb.AppendFormat("AND REGION IN ('{0}') ", _region);
                if (!string.IsNullOrEmpty((string)arguments["DEPARTMENT"].ArgumentValue))
                {
                    tmpsb.AppendFormat("AND DEPARTMENT IN ({0})", Utils.MakeSqlQueryIn(arguments["DEPARTMENT"].ArgumentValue.ToString(), ','));
                }

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                      tmpsb.ToString(), false);

                this.ExecutingValue = db.Select(tmpsb.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetPosition()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {

                StringBuilder tmpsb = new StringBuilder();

                tmpsb.Append("SELECT DISTINCT POSITION FROM TAPUTUSERS  WHERE 1=1 and POSITION IS NOT NULL ");
                tmpsb.AppendFormat("AND REGION IN ('{0}') ", _region);

                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP,
                      tmpsb.ToString(), false);

                this.ExecutingValue = db.Select(tmpsb.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetArea()
        {
            AreaModel tmpArea = null;

            try
            {
                tmpArea = new AreaModel();
                DataTable tmpdt = tmpArea.LoadModelDataList(new ArgumentPack());
                StringBuilder tmpsb = new StringBuilder();
                tmpdt.DefaultView.RowFilter = tmpsb.ToString();
                this.ExecutingValue = Utils.DataTableToDataSet(tmpdt.DefaultView.ToTable(true, new string[] { "NAME" }));
                return;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }

        }

        public void GetEQModel(ArgumentPack arguments)
        {
            EquipmentModel tmpEQ = null;

            try
            {
                arguments.AddArgument("REGION", typeof(string), _region);

                tmpEQ = new EquipmentModel();

                DataTable tmpdt = tmpEQ.LoadModelDataList(new ArgumentPack());
                StringBuilder tmpsb = new StringBuilder();
                tmpsb.AppendFormat("REGION IN ('{0}') ", _region);
                if (arguments.IsContainArgument("AREA"))
                {
                    if (!string.IsNullOrEmpty((string)arguments["AREA"].ArgumentValue))
                    {
                        tmpsb.AppendFormat("AND AREA IN ({0})", Utils.MakeSqlQueryIn(arguments["AREA"].ArgumentValue.ToString(), ','));
                    }
                }
                if (arguments.IsContainArgument("LINE"))
                {
                    if (!string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                    {
                        tmpsb.AppendFormat("AND LINE IN ({0})", Utils.MakeSqlQueryIn(arguments["LINE"].ArgumentValue.ToString(), ','));
                    }
                }
                if (arguments.IsContainArgument("FACILITY"))
                {
                    if (!string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                    {
                        tmpsb.AppendFormat("AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments["FACILITY"].ArgumentValue.ToString(), ','));
                    }
                }
                if (arguments.IsContainArgument("EQUIPMENTTYPE"))
                {
                    if (!string.IsNullOrEmpty((string)arguments["EQUIPMENTTYPE"].ArgumentValue))
                    {
                        tmpsb.AppendFormat("AND EQUIPMENTTYPE IN ({0})", Utils.MakeSqlQueryIn(arguments["EQUIPMENTTYPE"].ArgumentValue.ToString(), ','));
                    }
                }
                tmpsb.AppendFormat(" AND EQUIPMENTSMODEL is not null");


                tmpdt.DefaultView.RowFilter = tmpsb.ToString();

                this.ExecutingValue = Utils.DataTableToDataSet(tmpdt.DefaultView.ToTable(true, new string[] { "EQUIPMENTSMODEL" }));

                return;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetEQType()
        {
            EquipmentModel tmpEQ = null;

            try
            {
                tmpEQ = new EquipmentModel();

                DataTable tmpdt = tmpEQ.LoadModelDataList(new ArgumentPack());
                StringBuilder tmpsb = new StringBuilder();
                tmpsb.AppendFormat("REGION IN ('{0}') ", _region);
                tmpdt.DefaultView.RowFilter = tmpsb.ToString();

                this.ExecutingValue = Utils.DataTableToDataSet(tmpdt.DefaultView.ToTable(true, new string[] { "EQUIPMENTTYPE" }));

                return;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }


        public void GetEQModel()
        {
            EquipmentModel tmpEQ = null;

            try
            {

                tmpEQ = new EquipmentModel();

                DataTable tmpdt = tmpEQ.LoadModelDataList(new ArgumentPack());
                StringBuilder tmpsb = new StringBuilder();
                tmpsb.AppendFormat("REGION IN ('{0}') ", _region);
                tmpsb.AppendFormat("AND EQUIPMENTSMODEL is not null");
                tmpdt.DefaultView.RowFilter = tmpsb.ToString();

                this.ExecutingValue = Utils.DataTableToDataSet(tmpdt.DefaultView.ToTable(true, new string[] { "EQUIPMENTSMODEL" }));

                return;
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetPMItem(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT PMITEM FROM TAPFTPMMASTER WHERE 1=1 \n");

                tmpSql.AppendFormat(" AND PMCATEGORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["EQMODEL"].ArgumentValue, ','));
                tmpSql.AppendFormat(" AND PROCESS IN ({0})", Utils.MakeSqlQueryIn((string)arguments["AREA"].ArgumentValue, ','));

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

        public void GetTableColum()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("select name from syscolumns where id=object_id('TAPFTINVENTORYPARTS')");


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

        public void GetBmCodeModel()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("select DISTINCT EQUIPMENTMODEL from TAPFTBMCODE ORDER BY EQUIPMENTMODEL ");


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

        public void GetBmCodeModel(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("select DISTINCT EQUIPMENTMODEL from TAPFTBMCODE  WHERE 1=1 ");
                if (arguments.IsContainArgument("AREA") && !string.IsNullOrEmpty(arguments["AREA"].ArgumentValue.ToString()))
                {
                    tmpSql.AppendFormat(" AND AREA IN ({0})", Utils.MakeSqlQueryIn((string)arguments["AREA"].ArgumentValue, ','));
                }
                if (arguments.IsContainArgument("EQUIPMENTTYPE") && !string.IsNullOrEmpty(arguments["EQUIPMENTTYPE"].ArgumentValue.ToString()))
                {
                    tmpSql.AppendFormat(" AND EQUIPMENTTYPE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["EQUIPMENTTYPE"].ArgumentValue, ','));
                }
                tmpSql.Append(" ORDER BY EQUIPMENTMODEL  ");

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


        public void GetInventoryPart()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME FROM TAPFTINVENTORYPARTS WHERE ISINSTALLED = 'YES' AND PARTSSTATUS = 'STOCKOUT' AND ISALIVE = 'NO'");


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

        public void GetMDI()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT MDI FROM TAPSTBUI");


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
        public void GetMainMenu()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT MAINMENU FROM TAPSTBUI");


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

        public void GetMdiMAINMENU()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT  DISTINCT MDI FROM TAPSTBMAINMENU ORDER BY MDI ");


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
        public void GetMobileRole()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME  FROM TAPCTCODES WHERE CATEGORY='APP'AND  SUBCATEGORY='MOBILEROLE' AND USED='YES' AND ISALIVE='YES'");

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
        public void GetMAINMENU()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {

                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT  NAME FROM TAPSTBMAINMENU WHERE ISALIVE='YES' AND MDI='ISIA' ORDER BY NAME ");


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

        public void GetSUBMENU()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {

                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT  NAME FROM TAPSTBSUBMENU WHERE ISALIVE='YES' AND MDI='ISIA' ORDER BY NAME ");


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

        public void GetEquipmentTestType()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT TYPE FROM TAPFTEQINVOKE WHERE TYPE LIKE '%TEST'");

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
        public void GetEquipmentType()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT TYPE FROM TAPFTEQINVOKE WHERE TYPE NOT LIKE '%TEST'");

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

        public void GetFactory()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME FROM TAPWIP_FAC ");

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

        public void GetFactory(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT DISTINCT NAME AS FACTORY ");
                tmpSql.Append("FROM TAPWIP_FAC ");
                tmpSql.Append("WHERE 1 = 1 ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }

        public void GetUPTTableName(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT TABLE_NAME FROM TAPUPT_NAM WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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

        public void GetNewUPTTableName(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT TABLE_NAME FROM TAPUPT_NAM WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
                }
                //tmpSql.Append(" EXCEPT ");
                //tmpSql.Append("SELECT DISTINCT TABLE_NAME FROM TAPUPT_DAT WHERE 1=1 ");
                //if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                //{
                //    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
                //}

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

        public void GetFactoryType()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT FACTORY_TYPE FROM TAPWIP_FAC ");

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
        public void GetOpeationName(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME FROM TAPWIP_OPR WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetOpeationLoss()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT LOSS_TBL FROM TAPWIP_OPR WHERE LOSS_TBL IS NOT NULL ");

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
        public void GetOpeationBonus()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT BONUS_TBL FROM TAPWIP_OPR WHERE BONUS_TBL IS NOT NULL ");

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
        public void GetOpeationRework()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT REWORK_TBL FROM TAPWIP_OPR WHERE REWORK_TBL IS NOT NULL ");

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

        public void OperationGroup1(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT KEY1  AS OPER_GRP1  FROM TAPUPT_DAT WHERE TABLE_NAME ='OPER_GRP1'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void OperationGroup2(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT KEY1  AS OPER_GRP2  FROM TAPUPT_DAT WHERE TABLE_NAME ='OPER_GRP2'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void OperationGroup3(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT KEY1  AS OPER_GRP3  FROM TAPUPT_DAT WHERE TABLE_NAME ='OPER_GRP3'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void OperationGroup4(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT KEY1  AS OPER_GRP4  FROM TAPUPT_DAT WHERE TABLE_NAME ='OPER_GRP4'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void OperationGroup5(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT KEY1  AS OPER_GRP5  FROM TAPUPT_DAT WHERE TABLE_NAME ='OPER_GRP5'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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

        public void GetReportProcessCode()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT(NAME) AS PROCESS  FROM TAPCTCODES WHERE CATEGORY ='REPORT_PROCESSCODE'");
                tmpSql.Append(" ORDER BY NAME ");
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

        public void GetReportProcessCode(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT(NAME) AS PROCESS FROM TAPCTCODES WHERE CATEGORY ='REPORT_PROCESSCODE'");
                if (!string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND SUBCATEGORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["LINE"].ArgumentValue, ','));
                }
                tmpSql.Append(" ORDER BY NAME ");
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

        public void GetReportProdType()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT(NAME) AS PROCESS  FROM TAPCTCODES WHERE CATEGORY ='REPORT_PRODTYPE'");
                tmpSql.Append(" ORDER BY NAME ");
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
        public void GetReportProdType(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT(NAME) AS PROCESS  FROM TAPCTCODES WHERE CATEGORY ='REPORT_PRODTYPE'");
                if (!string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND SUBCATEGORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["LINE"].ArgumentValue, ','));
                }
                tmpSql.Append(" ORDER BY NAME ");
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


        public void GetDeviceName(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME FROM TAPWIP_DVC WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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

        public void GetDeviceName()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME FROM TAPWIP_DVC WHERE 1=1 ");
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

        public void GetCustomer()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT KEY1 AS CUSTOMER FROM  TAPUPT_DAT WHERE TABLE_NAME = 'CUSTOMER_CODE' ");

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
        public void GetDeviceStatusCode()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME AS DEVICE_STATUS_CODE FROM TAPCTCODES WHERE CATEGORY='DEVICE_STATUS_CODE'  AND USED='YES' ");

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
        //flow comm
        #region
        public void GetFlowName(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME FROM TAPWIP_FLW WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetFlowStatusCode()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME AS FLOW_STATUS_CODE FROM TAPCTCODES WHERE CATEGORY='FLOW_STATUS_CODE'  AND USED='YES' ");

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

        public void GetFlowGroup1(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT KEY1 AS FLOW_GRP1 FROM TAPUPT_DAT WHERE TABLE_NAME ='FLOW_GRP1'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetFlowGroup2(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT KEY1 AS FLOW_GRP2 FROM TAPUPT_DAT WHERE TABLE_NAME ='FLOW_GRP2'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetFlowGroup3(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT KEY1 AS FLOW_GRP3 FROM TAPUPT_DAT WHERE TABLE_NAME ='FLOW_GRP3'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetFlowGroup4(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT KEY1 AS FLOW_GRP4 FROM TAPUPT_DAT WHERE TABLE_NAME ='FLOW_GRP4'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetFlowGroup5(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT KEY1 AS FLOW_GRP5 FROM TAPUPT_DAT WHERE TABLE_NAME ='FLOW_GRP5'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetFlow_customized_Field1(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS FLOW_CUSTOMIZED_FIELD1 FROM TAPUPT_DAT A, TAPWIP_CMF B WHERE A.TABLE_NAME=B.TABLE1 AND CUSTOMIZED_FIELD_ITEM = 'FLOW'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND B.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetFlow_customized_Field2(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS FLOW_CUSTOMIZED_FIELD2 FROM TAPUPT_DAT A, TAPWIP_CMF B WHERE A.TABLE_NAME=B.TABLE2 AND CUSTOMIZED_FIELD_ITEM = 'FLOW'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND B.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetFlow_customized_Field3(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS FLOW_CUSTOMIZED_FIELD3 FROM TAPUPT_DAT A, TAPWIP_CMF B WHERE A.TABLE_NAME=B.TABLE3 AND CUSTOMIZED_FIELD_ITEM = 'FLOW'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND B.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetFlow_customized_Field4(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS FLOW_CUSTOMIZED_FIELD4 FROM TAPUPT_DAT A, TAPWIP_CMF B WHERE A.TABLE_NAME=B.TABLE4 AND CUSTOMIZED_FIELD_ITEM = 'FLOW'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND B.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetFlow_customized_Field5(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS FLOW_CUSTOMIZED_FIELD5 FROM TAPUPT_DAT A, TAPWIP_CMF B WHERE A.TABLE_NAME=B.TABLE5 AND CUSTOMIZED_FIELD_ITEM = 'FLOW'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND B.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetFlow_customized_Field6(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS FLOW_CUSTOMIZED_FIELD6 FROM TAPUPT_DAT A, TAPWIP_CMF B WHERE A.TABLE_NAME=B.TABLE6 AND CUSTOMIZED_FIELD_ITEM = 'FLOW'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND B.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetFlow_customized_Field7(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS FLOW_CUSTOMIZED_FIELD7 FROM TAPUPT_DAT A, TAPWIP_CMF B WHERE A.TABLE_NAME=B.TABLE7 AND CUSTOMIZED_FIELD_ITEM = 'FLOW'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND B.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetFlow_customized_Field8(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS FLOW_CUSTOMIZED_FIELD8 FROM TAPUPT_DAT A, TAPWIP_CMF B WHERE A.TABLE_NAME=B.TABLE8 AND CUSTOMIZED_FIELD_ITEM = 'FLOW'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND B.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetFlow_customized_Field9(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS FLOW_CUSTOMIZED_FIELD9 FROM TAPUPT_DAT A, TAPWIP_CMF B WHERE A.TABLE_NAME=B.TABLE9 AND CUSTOMIZED_FIELD_ITEM = 'FLOW'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND B.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetFlow_customized_Field10(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS FLOW_CUSTOMIZED_FIELD10 FROM TAPUPT_DAT A, TAPWIP_CMF B WHERE A.TABLE_NAME=B.TABLE10 AND CUSTOMIZED_FIELD_ITEM = 'FLOW'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND B.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        #endregion
        public void GetDeviceGroup1(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT KEY1  AS DEVICE_GRP1  FROM TAPUPT_DAT WHERE TABLE_NAME ='DEVICE_GRP1'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetDeviceGroup2(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT KEY1  AS DEVICE_GRP2  FROM TAPUPT_DAT WHERE TABLE_NAME ='DEVICE_GRP2'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetDeviceGroup3(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT KEY1  AS DEVICE_GRP3  FROM TAPUPT_DAT WHERE TABLE_NAME ='DEVICE_GRP3'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetDeviceGroup4(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT KEY1  AS DEVICE_GRP4  FROM TAPUPT_DAT WHERE TABLE_NAME ='DEVICE_GRP4'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetDeviceGroup5(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT KEY1  AS DEVICE_GRP5  FROM TAPUPT_DAT WHERE TABLE_NAME ='DEVICE_GRP5'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetDevice_customized_Field1(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS DEVICE_CUSTOMIZED_FIELD1 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE1 AND CUSTOMIZED_FIELD_ITEM = 'DEVICE'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetDevice_customized_Field2(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS DEVICE_CUSTOMIZED_FIELD2 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE2 AND CUSTOMIZED_FIELD_ITEM = 'DEVICE'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetDevice_customized_Field3(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS DEVICE_CUSTOMIZED_FIELD3 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE3 AND CUSTOMIZED_FIELD_ITEM = 'DEVICE'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetDevice_customized_Field4(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS DEVICE_CUSTOMIZED_FIELD4 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE4 AND CUSTOMIZED_FIELD_ITEM = 'DEVICE'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetDevice_customized_Field5(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS DEVICE_CUSTOMIZED_FIELD5 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE5 AND CUSTOMIZED_FIELD_ITEM = 'DEVICE'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetDevice_customized_Field6(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS DEVICE_CUSTOMIZED_FIELD6 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE6 AND CUSTOMIZED_FIELD_ITEM = 'DEVICE'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetDevice_customized_Field7(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS DEVICE_CUSTOMIZED_FIELD7 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE7 AND CUSTOMIZED_FIELD_ITEM = 'DEVICE'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetDevice_customized_Field8(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS DEVICE_CUSTOMIZED_FIELD8 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE8 AND CUSTOMIZED_FIELD_ITEM = 'DEVICE'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetDevice_customized_Field9(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS DEVICE_CUSTOMIZED_FIELD9 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE9 AND CUSTOMIZED_FIELD_ITEM = 'DEVICE'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetDevice_customized_Field10(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS DEVICE_CUSTOMIZED_FIELD10 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE10 AND CUSTOMIZED_FIELD_ITEM = 'DEVICE'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetOperation_customized_Field1(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS OPERATION_CUSTOMIZED_FIELD1 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE1 AND CUSTOMIZED_FIELD_ITEM = 'OPER'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetOperation_customized_Field2(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS OPERATION_CUSTOMIZED_FIELD2 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE2 AND CUSTOMIZED_FIELD_ITEM = 'OPER'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetOperation_customized_Field3(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS OPERATION_CUSTOMIZED_FIELD3 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE3 AND CUSTOMIZED_FIELD_ITEM = 'OPER'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetOperation_customized_Field4(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS OPERATION_CUSTOMIZED_FIELD4 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE4 AND CUSTOMIZED_FIELD_ITEM = 'OPER'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetOperation_customized_Field5(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS OPERATION_CUSTOMIZED_FIELD5 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE5 AND CUSTOMIZED_FIELD_ITEM = 'OPER'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetOperation_customized_Field6(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS OPERATION_CUSTOMIZED_FIELD6 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE6 AND CUSTOMIZED_FIELD_ITEM = 'OPER'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetOperation_customized_Field7(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS OPERATION_CUSTOMIZED_FIELD7 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE7 AND CUSTOMIZED_FIELD_ITEM = 'OPER'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetOperation_customized_Field8(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS OPERATION_CUSTOMIZED_FIELD8 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE8 AND CUSTOMIZED_FIELD_ITEM = 'OPER'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetOperation_customized_Field9(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS OPERATION_CUSTOMIZED_FIELD9 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE9 AND CUSTOMIZED_FIELD_ITEM = 'OPER'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        public void GetOperation_customized_Field10(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("   SELECT DISTINCT KEY1 AS OPERATION_CUSTOMIZED_FIELD10 FROM  TAPUPT_DAT UPT, TAPWIP_CMF CMF WHERE UPT.TABLE_NAME=CMF.TABLE10 AND CUSTOMIZED_FIELD_ITEM = 'OPER'");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND CMF.FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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

        public void GetCustomItem(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT CUSTOMIZED_FIELD_ITEM FROM TAPWIP_CMF WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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

        public void GetMessageGroup()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME FROM TAPMSG_GRP WHERE 1=1 ");
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

        public void GetMessage(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME FROM TAPMSG_DAT WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["MESSAGEGROUP"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND MESSAGE_GROUP_ID IN ({0})", Utils.MakeSqlQueryIn((string)arguments["MESSAGEGROUP"].ArgumentValue, ','));
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

        public void GetDeviceFlow(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT FLOW FROM TAPWIP_DFR WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
                }
                if (!string.IsNullOrEmpty((string)arguments["DEVICE"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND DEVICE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["DEVICE"].ArgumentValue, ','));
                }
                tmpSql.Append("ORDER BY  FLOW_SEQUENCE ASC");
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

        public void GetFlowOperation(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT OPERATION FROM TAPWIP_FWO WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
                }
                if (!string.IsNullOrEmpty((string)arguments["FLOW"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FLOW IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FLOW"].ArgumentValue, ','));
                }
                tmpSql.Append("ORDER BY  OPERATION_SEQUENCE ASC");
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

        public void GetReworkCode(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();

                tmpSql.Append("SELECT KEY1 AS REWORK_CODE ");
                tmpSql.Append("FROM TAPUPT_DAT ");
                tmpSql.Append("WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
                }
                tmpSql.Append("AND TABLE_NAME = 'REWORK_CODE' ");

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
        public void GetNoReworkDevice(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME  FROM TAPWIP_DVC WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
                }
                tmpSql.Append("EXCEPT SELECT DEVICE  FROM TAPWIP_DFA WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACTORY IN ({0})", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
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
        #region LYS
        public void GetDevice(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT DISTINCT NAME AS DEVICE ");
                tmpSql.Append("FROM TAPWIP_DVC ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.Factory))
                    tmpSql.AppendFormat("  AND FACTORY = '{0}' ", arguments.Factory);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetDevice()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT DISTINCT NAME AS DEVICE ");
                tmpSql.Append("FROM TAPWIP_DVC ");
                tmpSql.Append("WHERE 1 = 1 ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetDevice(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT DISTINCT NAME AS DEVICE ");
                tmpSql.Append("FROM TAPWIP_DVC ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat("  AND FACTORY IN ({0}) ", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
                }
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetFlow(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT DISTINCT NAME AS FLOW ");
                tmpSql.Append("FROM TAPWIP_FLW ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.Factory))
                    tmpSql.AppendFormat("  AND FACTORY = '{0}' ", arguments.Factory);
                if (!string.IsNullOrEmpty(arguments.Rework_Flow_YN))
                    tmpSql.AppendFormat("  AND REWORK_FLOW_YN = '{0}' ", arguments.Rework_Flow_YN);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetFlow()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT DISTINCT NAME AS FLOW ");
                tmpSql.Append("FROM TAPWIP_FLW ");
                tmpSql.Append("WHERE 1 = 1 ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetFlow(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT DISTINCT NAME AS FLOW ");
                tmpSql.Append("FROM TAPWIP_FLW ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat("  AND FACTORY IN ({0}) ", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
                }
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetOperation(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS OPERATION ");
                tmpSql.Append("FROM TAPWIP_OPR ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.Factory))
                    tmpSql.AppendFormat("  AND FACTORY = '{0}' ", arguments.Factory);
                if (!string.IsNullOrEmpty(arguments.ReworkYN))
                    tmpSql.AppendFormat("  AND REWORK_TBL = '{0}' ", arguments.ReworkYN);
                tmpSql.Append("ORDER BY OPERATION_DISPLAY_SEQUENCE ASC ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetOperation()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS OPERATION ");
                tmpSql.Append("FROM TAPWIP_OPR ");
                tmpSql.Append("WHERE 1 = 1 ");
                tmpSql.Append("ORDER BY OPERATION_DISPLAY_SEQUENCE ASC ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetOperation(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS OPERATION ");
                tmpSql.Append("FROM TAPWIP_OPR ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty((string)arguments["FACTORY"].ArgumentValue))
                {
                    tmpSql.AppendFormat("  AND FACTORY IN ({0}) ", Utils.MakeSqlQueryIn((string)arguments["FACTORY"].ArgumentValue, ','));
                }
                tmpSql.Append("ORDER BY OPERATION_DISPLAY_SEQUENCE ASC ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetEqp(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS EQP ");
                tmpSql.Append("FROM TAPFTEQUIPMENT ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.Factory))
                    tmpSql.AppendFormat("  AND REGION = '{0}' ", arguments.Factory);
                if (!string.IsNullOrEmpty(arguments.Operation))
                    tmpSql.AppendFormat("  AND AREA = '{0}' ", arguments.Operation);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetEqp()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS EQP ");
                tmpSql.Append("FROM TAPFTEQUIPMENT ");
                tmpSql.Append("WHERE 1 = 1 ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetLot(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS LOT, CARRIER, FACTORY, DEVICE, FLOW, OPERATION, QTY, LOTSTATE, RUNSTATE, EQP, HOLDCODE, CREATE_TIME, CREATE_USER, UPDATE_TIME, UPDATE_USER, REWORK_FLAG, REWORK_CODE, REWORK_COUNT, REWORK_RETURN_FLOW, REWORK_RETURN_OPERATION, WORKORDER ");
                tmpSql.Append("FROM TAPWIP_LOT ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.Lot))
                    tmpSql.AppendFormat("  AND NAME IN ({0}) ", Utils.MakeSqlQueryIn(arguments.Lot, ','));
                if (!string.IsNullOrEmpty(arguments.Factory))
                    tmpSql.AppendFormat("  AND FACTORY = '{0}' ", arguments.Factory);
                if (!string.IsNullOrEmpty(arguments.Device))
                    tmpSql.AppendFormat("  AND DEVICE = '{0}' ", arguments.Device);
                if (!string.IsNullOrEmpty(arguments.Flow))
                    tmpSql.AppendFormat("  AND FLOW = '{0}' ", arguments.Flow);
                if (!string.IsNullOrEmpty(arguments.Operation))
                    tmpSql.AppendFormat("  AND OPERATION = '{0}' ", arguments.Operation);
                if (!string.IsNullOrEmpty(arguments.Where))
                    tmpSql.AppendFormat("{0}", arguments.Where);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetLotList(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS LOT ");
                tmpSql.Append("FROM TAPWIP_LOT ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.Lot))
                    tmpSql.AppendFormat("  AND NAME IN ({0}) ", Utils.MakeSqlQueryIn(arguments.Lot, ','));
                if (!string.IsNullOrEmpty(arguments.Factory))
                    tmpSql.AppendFormat("  AND FACTORY = '{0}' ", arguments.Factory);
                if (!string.IsNullOrEmpty(arguments.Device))
                    tmpSql.AppendFormat("  AND DEVICE = '{0}' ", arguments.Device);
                if (!string.IsNullOrEmpty(arguments.Flow))
                    tmpSql.AppendFormat("  AND FLOW = '{0}' ", arguments.Flow);
                if (!string.IsNullOrEmpty(arguments.Operation))
                    tmpSql.AppendFormat("  AND OPERATION = '{0}' ", arguments.Operation);
                if (!string.IsNullOrEmpty(arguments.Where))
                    tmpSql.AppendFormat("{0}", arguments.Where);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetLotList()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS LOT ");
                tmpSql.Append("FROM TAPWIP_LOT ");
                tmpSql.Append("WHERE 1 = 1 ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetWafer(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS WAFER, LOT, FACTORY, DEVICE, FLOW, OPERATION, SLOT, WFRSTATE, RUNSTATE, EQP, CREATE_TIME, CREATE_USER, UPDATE_TIME, UPDATE_USER ");
                tmpSql.Append("FROM TAPWIP_WFR ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.Lot))
                    tmpSql.AppendFormat("  AND LOT IN ({0}) ", Utils.MakeSqlQueryIn(arguments.Lot, ','));
                if (!string.IsNullOrEmpty(arguments.Factory))
                    tmpSql.AppendFormat("  AND FACTORY = '{0}' ", arguments.Factory);
                if (!string.IsNullOrEmpty(arguments.Device))
                    tmpSql.AppendFormat("  AND DEVICE = '{0}' ", arguments.Device);
                if (!string.IsNullOrEmpty(arguments.Flow))
                    tmpSql.AppendFormat("  AND FLOW = '{0}' ", arguments.Flow);
                if (!string.IsNullOrEmpty(arguments.Operation))
                    tmpSql.AppendFormat("  AND OPERATION = '{0}' ", arguments.Operation);
                if (!string.IsNullOrEmpty(arguments.Where))
                    tmpSql.AppendFormat("{0} ", arguments.Where);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetWaferList(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS WAFER ");
                tmpSql.Append("FROM TAPWIP_WFR ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.Lot))
                    tmpSql.AppendFormat("  AND NAME IN ({0}) ", Utils.MakeSqlQueryIn(arguments.Lot, ','));
                if (!string.IsNullOrEmpty(arguments.Factory))
                    tmpSql.AppendFormat("  AND FACTORY = '{0}' ", arguments.Factory);
                if (!string.IsNullOrEmpty(arguments.Device))
                    tmpSql.AppendFormat("  AND DEVICE = '{0}' ", arguments.Device);
                if (!string.IsNullOrEmpty(arguments.Flow))
                    tmpSql.AppendFormat("  AND FLOW = '{0}' ", arguments.Flow);
                if (!string.IsNullOrEmpty(arguments.Operation))
                    tmpSql.AppendFormat("  AND OPERATION = '{0}' ", arguments.Operation);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetWaferList()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS WAFER ");
                tmpSql.Append("FROM TAPWIP_WFR ");
                tmpSql.Append("WHERE 1 = 1 ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetWaferList(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS WAFER ");
                tmpSql.Append("FROM TAPWIP_WFR ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty((string)arguments["LOT"].ArgumentValue))
                {
                    tmpSql.AppendFormat("  AND LOT IN ({0}) ", Utils.MakeSqlQueryIn((string)arguments["LOT"].ArgumentValue, ','));
                }
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetSplitLot(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS LOT, '' AS WAFER, FACTORY, DEVICE, FLOW, OPERATION, QTY ");
                tmpSql.Append("FROM TAPWIP_LOT ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.Lot))
                    tmpSql.AppendFormat("  AND NAME = '{0}' ", arguments.Lot);
                if (!string.IsNullOrEmpty(arguments.Factory))
                    tmpSql.AppendFormat("  AND FACTORY = '{0}' ", arguments.Factory);
                if (!string.IsNullOrEmpty(arguments.Device))
                    tmpSql.AppendFormat("  AND DEVICE = '{0}' ", arguments.Device);
                if (!string.IsNullOrEmpty(arguments.Flow))
                    tmpSql.AppendFormat("  AND FLOW = '{0}' ", arguments.Flow);
                if (!string.IsNullOrEmpty(arguments.Operation))
                    tmpSql.AppendFormat("  AND OPERATION = '{0}' ", arguments.Operation);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetSplitWafer(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT LOT, NAME AS WAFER, FACTORY, DEVICE, FLOW, OPERATION, '' AS QTY ");
                tmpSql.Append("FROM TAPWIP_WFR ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.Lot))
                    tmpSql.AppendFormat("  AND LOT = '{0}' ", arguments.Lot);
                if (!string.IsNullOrEmpty(arguments.Factory))
                    tmpSql.AppendFormat("  AND FACTORY = '{0}' ", arguments.Factory);
                if (!string.IsNullOrEmpty(arguments.Device))
                    tmpSql.AppendFormat("  AND DEVICE = '{0}' ", arguments.Device);
                if (!string.IsNullOrEmpty(arguments.Flow))
                    tmpSql.AppendFormat("  AND FLOW = '{0}' ", arguments.Flow);
                if (!string.IsNullOrEmpty(arguments.Operation))
                    tmpSql.AppendFormat("  AND OPERATION = '{0}' ", arguments.Operation);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetChildLot(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS LOT, CARRIER, FACTORY, DEVICE, FLOW, OPERATION, QTY, LOTSTATE, RUNSTATE, EQP, HOLDCODE, CREATE_TIME, CREATE_USER, UPDATE_TIME, UPDATE_USER, REWORK_FLAG, REWORK_CODE, REWORK_COUNT, REWORK_RETURN_FLOW, REWORK_RETURN_OPERATION ");
                tmpSql.Append("FROM TAPWIP_LOT ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.Lot))
                {
                    tmpSql.AppendFormat("  AND NAME LIKE '{0}.%' ", arguments.Lot.Split('.')[0]);
                    tmpSql.AppendFormat("  AND NAME <> '{0}' ", arguments.Lot);
                }
                if (!string.IsNullOrEmpty(arguments.Where))
                    tmpSql.AppendFormat("{0} ", arguments.Where);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetWorkOrder(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT PYEAR, PMONTH, WORKORDER, QTY, CREATE_TIME, CREATE_USER, UPDATE_TIME, UPDATE_USER ");
                tmpSql.Append("FROM TAPWIP_PLN ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.Year))
                    tmpSql.AppendFormat("  AND PYEAR = '{0}' ", arguments.Year);
                if (!string.IsNullOrEmpty(arguments.Month))
                    tmpSql.AppendFormat("  AND PMONTH = '{0}' ", arguments.Month);
                if (!string.IsNullOrEmpty(arguments.WorkOrder))
                    tmpSql.AppendFormat("  AND WORKORDER IN ({0}) ", Utils.MakeSqlQueryIn(arguments.WorkOrder, ','));
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetWorkOrderList(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT WORKORDER ");
                tmpSql.Append("FROM TAPWIP_PLN ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.Year))
                    tmpSql.AppendFormat("  AND PYEAR = '{0}' ", arguments.Year);
                if (!string.IsNullOrEmpty(arguments.Month))
                    tmpSql.AppendFormat("  AND PMONTH = '{0}' ", arguments.Month);
                if (!string.IsNullOrEmpty(arguments.WorkOrder))
                    tmpSql.AppendFormat("  AND WORKORDER IN ({0}) ", Utils.MakeSqlQueryIn(arguments.WorkOrder, ','));
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetWorkOrderList()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT WORKORDER ");
                tmpSql.Append("FROM TAPWIP_PLN ");
                tmpSql.Append("WHERE 1 = 1 ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetBomCodeName(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT BOMCODENAME ");
                tmpSql.Append("FROM TAPBOM_COD ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.BomCodeName))
                    tmpSql.AppendFormat("  AND BOMCODENAME = '{0}' ", arguments.BomCodeName);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetBomCodeName()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT BOMCODENAME ");
                tmpSql.Append("FROM TAPBOM_COD ");
                tmpSql.Append("WHERE 1 = 1 ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetBomPartName(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT BOMPARTNAME ");
                tmpSql.Append("FROM TAPBOM_PAT ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.BomPartName))
                    tmpSql.AppendFormat("  AND BOMPARTNAME = '{0}' ", arguments.BomPartName);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetBomPartName()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT BOMPARTNAME ");
                tmpSql.Append("FROM TAPBOM_PAT ");
                tmpSql.Append("WHERE 1 = 1 ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetBomPartGroup()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT DISTINCT BOMPARTGROUP ");
                tmpSql.Append("FROM TAPBOM_PAT ");
                tmpSql.Append("WHERE 1 = 1 ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetBomPartGroup(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT DISTINCT BOMPARTGROUP ");
                tmpSql.Append("FROM TAPBOM_PAT ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty((string)arguments["BOMPARTNAME"].ArgumentValue))
                {
                    tmpSql.AppendFormat("  AND BOMPARTNAME IN ({0}) ", Utils.MakeSqlQueryIn((string)arguments["BOMPARTNAME"].ArgumentValue, ','));
                }
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetLabelName(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT SUBCATEGORY, NAME, DESCRIPTION ");
                tmpSql.Append("FROM TAPCTCODES ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.Where))
                    tmpSql.AppendFormat("{0} ", arguments.Where);
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetSpcRuleName()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT SPCRULENAME ");
                tmpSql.Append("FROM TAPSPC ");
                tmpSql.Append("WHERE 1 = 1 ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetMappingType()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME ");
                tmpSql.Append("FROM TAPCTCODES ");
                tmpSql.Append("WHERE 1 = 1 ");
                tmpSql.Append("  AND CATEGORY = 'MAPPING' ");
                tmpSql.Append("  AND SUBCATEGORY = 'TYPE' ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetFlagYN()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME ");
                tmpSql.Append("FROM TAPCTCODES ");
                tmpSql.Append("WHERE 1 = 1 ");
                tmpSql.Append("  AND CATEGORY = 'FLAG' ");
                tmpSql.Append("  AND SUBCATEGORY = 'TYPE' ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetParaName()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME ");
                tmpSql.Append("FROM TAPCTCODES ");
                tmpSql.Append("WHERE 1 = 1 ");
                tmpSql.Append("  AND CATEGORY = 'EDCOL' ");
                tmpSql.Append("  AND SUBCATEGORY = 'PARANAME' ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetEDC(MessageParaPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT LOT, WAFER, FACTORY, DEVICE, FLOW, OPERATION, EQP, PARANAME, PARAVALUE, CREATE_TIME, CREATE_USER, UPDATE_TIME, UPDATE_USER ");
                tmpSql.Append("FROM TAPWIP_EDC ");
                tmpSql.Append("WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments.Lot))
                    tmpSql.AppendFormat("  AND LOT IN ({0}) ", Utils.MakeSqlQueryIn(arguments.Lot, ','));
                if (!string.IsNullOrEmpty(arguments.Wafer))
                    tmpSql.AppendFormat("  AND WAFER IN ({0}) ", Utils.MakeSqlQueryIn(arguments.Wafer, ','));
                if (!string.IsNullOrEmpty(arguments.Factory))
                    tmpSql.AppendFormat("  AND FACTORY IN ({0}) ", Utils.MakeSqlQueryIn(arguments.Factory, ','));
                if (!string.IsNullOrEmpty(arguments.Device))
                    tmpSql.AppendFormat("  AND DEVICE IN ({0}) ", Utils.MakeSqlQueryIn(arguments.Device, ','));
                if (!string.IsNullOrEmpty(arguments.Flow))
                    tmpSql.AppendFormat("  AND FLOW IN ({0}) ", Utils.MakeSqlQueryIn(arguments.Flow, ','));
                if (!string.IsNullOrEmpty(arguments.Operation))
                    tmpSql.AppendFormat("  AND OPERATION IN ({0}) ", Utils.MakeSqlQueryIn(arguments.Operation, ','));
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetHoldCode()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS HOLDCODE ");
                tmpSql.Append("FROM TAPCTCODES ");
                tmpSql.Append("WHERE 1 = 1 ");
                tmpSql.Append("  AND CATEGORY = 'CODE' ");
                tmpSql.Append("  AND SUBCATEGORY = 'HOLD' ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetReleaseCode()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS RELEASECODE ");
                tmpSql.Append("FROM TAPCTCODES ");
                tmpSql.Append("WHERE 1 = 1 ");
                tmpSql.Append("  AND CATEGORY = 'CODE' ");
                tmpSql.Append("  AND SUBCATEGORY = 'RELEASE' ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        public void GetReworkCode()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpSql = new StringBuilder();
            try
            {
                tmpSql.Append("SELECT NAME AS REWORKCODE ");
                tmpSql.Append("FROM TAPCTCODES ");
                tmpSql.Append("WHERE 1 = 1 ");
                tmpSql.Append("  AND CATEGORY = 'CODE' ");
                tmpSql.Append("  AND SUBCATEGORY = 'REWORK' ");
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_INFO, this.Requester.IP, tmpSql.ToString(), false);
                this.ExecutingValue = db.Select(tmpSql.ToString());
            }
            catch (Exception ex)
            {
                RemotingLog.Instance.WriteServerLog(MethodInfo.GetCurrentMethod().Name, LogBase._LOGTYPE_TRACE_ERROR, this.Requester.IP,
                       string.Format(" Biz Component Exception occured: {0}", ex.ToString()), false);
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// GET SHIFT STATUS 班次（白班 夜班）
        /// </summary>
        public void GetShiftStatusCode()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME AS SHIFT_STATUS_CODE FROM TAPCTCODES WHERE CATEGORY='SHIFT_STATUS_CODE'  AND USED='YES' ");

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
        /// <summary>
        /// GET Approval STATUS 审核状态
        /// </summary>
        public void GetApprovalStatusCode()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME AS APPROVAL_STATUS_CODE FROM TAPCTCODES WHERE CATEGORY='APPROVAL_STATUS_CODE'  AND USED='YES' ");

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
        public void GetPackingQualityGroup()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT QUALITYTYPE FROM TAPCT_PACKINGPOWERGRADE WHERE 1 = 1 GROUP BY QUALITYTYPE");

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
        public void GetPackingPowerGradeGroup()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT POWERGRADE FROM TAPCT_PACKINGPOWERGRADE WHERE 1 = 1 ");
                tmpSql.Append(" GROUP BY POWERGRADE ORDER BY POWERGRADE");
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
        public void GetPackingPowerGradeGroup(string arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT POWERGRADE FROM TAPCT_PACKINGPOWERGRADE WHERE 1 = 1 ");
                if (!string.IsNullOrEmpty(arguments))
                {
                    tmpSql.AppendFormat(" AND QUALITYTYPE IN ({0})", Utils.MakeSqlQueryIn(arguments, ','));
                }
                tmpSql.Append(" GROUP BY POWERGRADE ORDER BY POWERGRADE");
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

        public void GetPackingCellSize(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME FROM TAPCTCODES WHERE 1 = 1 AND CATEGORY = 'PACKINGINFO' AND SUBCATEGORY = '1' ");
                if (!string.IsNullOrEmpty((string)arguments["QUALITYTYPE"].ArgumentValue))
                {
                    //tmpSql.AppendFormat(" AND QUALITYTYPE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["QUALITYTYPE"].ArgumentValue, ','));
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
        public void GetPackingCellSize()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME FROM TAPCTCODES WHERE 1 = 1 AND CATEGORY = 'PACKINGINFO' AND SUBCATEGORY = '1' ");

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

        public void GetPackingFribtAgPaste(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME FROM TAPCTCODES WHERE 1 = 1 AND CATEGORY = 'PACKINGINFO' AND SUBCATEGORY = '2' ");
                if (!string.IsNullOrEmpty((string)arguments["QUALITYTYPE"].ArgumentValue))
                {
                    //tmpSql.AppendFormat(" AND QUALITYTYPE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["QUALITYTYPE"].ArgumentValue, ','));
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
        public void GetPackingFribtAgPaste()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME FROM TAPCTCODES WHERE 1 = 1 AND CATEGORY = 'PACKINGINFO' AND SUBCATEGORY = '2' ");

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

        public void GetPackingBackAgPaste(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME FROM TAPCTCODES WHERE 1 = 1 AND CATEGORY = 'PACKINGINFO' AND SUBCATEGORY = '3' ");
                if (!string.IsNullOrEmpty((string)arguments["QUALITYTYPE"].ArgumentValue))
                {
                    //tmpSql.AppendFormat(" AND QUALITYTYPE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["QUALITYTYPE"].ArgumentValue, ','));
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
        public void GetPackingBackAgPaste()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME FROM TAPCTCODES WHERE 1 = 1 AND CATEGORY = 'PACKINGINFO' AND SUBCATEGORY = '3' ");

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

        public void GetPackingBackAlPaste(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME FROM TAPCTCODES WHERE 1 = 1 AND CATEGORY = 'PACKINGINFO' AND SUBCATEGORY NOT IN ('1','2','3','5') ");
                if (!string.IsNullOrEmpty((string)arguments["QUALITYTYPE"].ArgumentValue))
                {
                    //tmpSql.AppendFormat(" AND QUALITYTYPE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["QUALITYTYPE"].ArgumentValue, ','));
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
        public void GetPackingBackAlPaste()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME FROM TAPCTCODES WHERE 1 = 1 AND CATEGORY = 'PACKINGINFO' AND SUBCATEGORY NOT IN ('1','2','3','5') ");

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

        public void GetPackingCommentsA(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME FROM TAPCTCODES WHERE 1 = 1 AND CATEGORY = 'PACKINGINFO' AND SUBCATEGORY = '5' ");
                if (!string.IsNullOrEmpty((string)arguments["QUALITYTYPE"].ArgumentValue))
                {
                    //tmpSql.AppendFormat(" AND QUALITYTYPE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["QUALITYTYPE"].ArgumentValue, ','));
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
        public void GetPackingCommentsA()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME FROM TAPCTCODES WHERE 1 = 1 AND CATEGORY = 'PACKINGINFO' AND SUBCATEGORY = '5' ");

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

        public void GetPackingColor()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT CHARACTER_VALUE_DESC FROM IFR_ATTRIBUTE_INFO WHERE 1 = 1 AND CHARACTER_NAME = 'CHAR_CELL_COLOR' ");
                tmpSql.Append(" GROUP BY CHARACTER_VALUE_DESC");
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
        public void GetPackingColor(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT CHARACTER_VALUE_DESC FROM IFR_ATTRIBUTE_INFO WHERE 1 = 1 AND CHARACTER_NAME = 'CHAR_CELL_COLOR' ");
                if (!string.IsNullOrEmpty((string)arguments["QUALITYTYPE"].ArgumentValue))
                {
                    //tmpSql.AppendFormat(" AND QUALITYTYPE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["QUALITYTYPE"].ArgumentValue, ','));
                }
                tmpSql.Append(" GROUP BY CHARACTER_VALUE_DESC");
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

        public void GetPackingComments()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT CHARACTER_VALUE_DESC FROM IFR_ATTRIBUTE_INFO WHERE 1 = 1 AND CHARACTER_NAME = 'CHAR_CELL_DEFECT_TYPES' ");
                tmpSql.Append(" GROUP BY CHARACTER_VALUE_DESC");
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
        public void GetPackingComments(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT CHARACTER_VALUE_DESC FROM IFR_ATTRIBUTE_INFO WHERE 1 = 1 AND CHARACTER_NAME = 'CHAR_CELL_DEFECT_TYPES' ");
                if (!string.IsNullOrEmpty((string)arguments["QUALITYTYPE"].ArgumentValue))
                {
                    //tmpSql.AppendFormat(" AND QUALITYTYPE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["QUALITYTYPE"].ArgumentValue, ','));
                }
                tmpSql.Append(" GROUP BY CHARACTER_VALUE_DESC");
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

        #region Modify !!! 수정해야 함. 2-26 seoil. 코드로 빼던지 정의 테이블에서 가져와야함. 느림.
        public void GetAlmLine()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT(CATEGORY1) LINE FROM TAPEQDATASUMM WHERE PROCESS='SP' ORDER BY CATEGORY1");
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

        public void GetAlmEqp()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT(CATEGORY1) LINE FROM TAPEQDATASUMM WHERE PROCESS NOT IN ('SP') ORDER BY CATEGORY1");
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

        public void GetAlmPro()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT(PROCESS) PROCESS FROM TAPEQDATASUMM WHERE PROCESS NOT IN ('SP') ORDER BY PROCESS");
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
        public void GetTSLine()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT LINE FROM TAPEQP_TS_A_LOADFILE");
                tmpSql.Append(" UNION ");
                tmpSql.Append("SELECT LINE FROM TAPEQP_TS_B_LOADFILE");
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
        public void GetTSWorkshop(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT WORKSHOP FROM TAPEQP_TS_A_LOADFILE");
                tmpSql.Append(" WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND LINE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["LINE"].ArgumentValue, ','));
                }
                tmpSql.Append(" UNION ");
                tmpSql.Append(" SELECT WORKSHOP FROM TAPEQP_TS_B_LOADFILE");
                tmpSql.Append(" WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND LINE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["LINE"].ArgumentValue, ','));
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
        public void GetTSProcess(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT PROCESS FROM TAPEQP_TS_A_LOADFILE");
                tmpSql.Append(" WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND LINE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["LINE"].ArgumentValue, ','));
                }
                if (!string.IsNullOrEmpty((string)arguments["WORKSHOP"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND WORKSHOP IN ({0})", Utils.MakeSqlQueryIn((string)arguments["WORKSHOP"].ArgumentValue, ','));
                }
                tmpSql.Append(" UNION");
                tmpSql.Append(" SELECT PROCESS FROM TAPEQP_TS_B_LOADFILE");
                tmpSql.Append(" WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND LINE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["LINE"].ArgumentValue, ','));
                }
                if (!string.IsNullOrEmpty((string)arguments["WORKSHOP"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND WORKSHOP IN ({0})", Utils.MakeSqlQueryIn((string)arguments["WORKSHOP"].ArgumentValue, ','));
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
        public void GetTSEquipment(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT EQUIPMENT FROM TAPEQP_TS_A_LOADFILE");
                tmpSql.Append(" WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND LINE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["LINE"].ArgumentValue, ','));
                }
                if (!string.IsNullOrEmpty((string)arguments["WORKSHOP"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND WORKSHOP IN ({0})", Utils.MakeSqlQueryIn((string)arguments["WORKSHOP"].ArgumentValue, ','));
                }
                if (!string.IsNullOrEmpty((string)arguments["PROCESS"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND PROCESS IN ({0})", Utils.MakeSqlQueryIn((string)arguments["PROCESS"].ArgumentValue, ','));
                }
                tmpSql.Append(" UNION");
                tmpSql.Append(" SELECT EQUIPMENT FROM TAPEQP_TS_B_LOADFILE");
                tmpSql.Append(" WHERE 1=1 ");
                if (!string.IsNullOrEmpty((string)arguments["LINE"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND LINE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["LINE"].ArgumentValue, ','));
                }
                if (!string.IsNullOrEmpty((string)arguments["WORKSHOP"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND WORKSHOP IN ({0})", Utils.MakeSqlQueryIn((string)arguments["WORKSHOP"].ArgumentValue, ','));
                }
                if (!string.IsNullOrEmpty((string)arguments["PROCESS"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND PROCESS IN ({0})", Utils.MakeSqlQueryIn((string)arguments["PROCESS"].ArgumentValue, ','));
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
        public void GetAlmLineAuto()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT(CATEGORY1) EQUIPMENT FROM TAPEQDATASUMM WHERE AND  CATEGORY1 LIKE '%A%'");
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
        public void GetELWORKSHOP()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT(WORKSHOP) WORKSHOP FROM TAPEQP_EL_LOADFILE");
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

        public void GetELLINE()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT(LINE) LINE FROM TAPEQP_EL_LOADFILE");
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
        public void GetELShift()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT(SHIFT) SHIFT FROM TAPEQP_EL_LOADFILE");
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
        public void GetRephistoryVendor(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT(VENDOR) VENDOR FROM TAPFTPARTSREPAIRHISTORY WHERE VENDOR IS NOT NULL");

                if (!string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments["FACILITY"].ArgumentValue.ToString(), ','));
                }

                tmpSql.Append(" order by VENDOR ");

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
        public void GetGetFacility()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT(FACILITY) VENDOR FROM TAPFTPARTSREPAIRHISTORY");
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
        public void GetReinstallequipment(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT(REINSTALLEQUIPMENT) VENDOR FROM TAPFTPARTSREPAIRHISTORY WHERE REINSTALLEQUIPMENT is not null ");
                 
                if (!string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments["FACILITY"].ArgumentValue.ToString(), ','));
                }

                tmpSql.Append(" order by REINSTALLEQUIPMENT ");
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

        public void GetUninstallequipment(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT(UNINSTALLEQUIPMENT) VENDOR FROM TAPFTPARTSREPAIRHISTORY WHERE UNINSTALLEQUIPMENT is not null ");

                if (!string.IsNullOrEmpty((string)arguments["FACILITY"].ArgumentValue))
                {
                    tmpSql.AppendFormat(" AND FACILITY IN ({0})", Utils.MakeSqlQueryIn(arguments["FACILITY"].ArgumentValue.ToString(), ','));
                }

                tmpSql.Append(" order by UNINSTALLEQUIPMENT ");

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


        #endregion

        /// <summary>
        /// GET WareHouse
        /// </summary>
        public void GetWareHouseCode()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT NAME AS WareHouseCode FROM TAPCTCODES WHERE CATEGORY='WAREHOUSE' ");

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

        /// <summary>
        /// GET ShelfNo
        /// </summary>
        public void GetShelfByWareHouse(MROArgsPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT subcategory AS Shelf FROM TAPCTCODES WHERE CATEGORY='WAREHOUSESHELF' "); 
                if (!string.IsNullOrEmpty(arguments.WareHouse))
                {
                    tmpSql.AppendFormat(" AND NAME = '{0}'", arguments.WareHouse);
                }
                tmpSql.Append(" AND used=0 ");
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
        public void GetMROShelf(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT SUBCATEGORY FROM TAPCTCODES WHERE CATEGORY='WAREHOUSESHELF'");

                if (!string.IsNullOrEmpty((string)arguments["NAME"].ArgumentValue))
                {
                    tmpSql.AppendFormat("AND NAME IN ({0})", Utils.MakeSqlQueryIn(arguments["NAME"].ArgumentValue.ToString(), ','));
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
        public void GetMROShelf()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT SUBCATEGORY FROM TAPCTCODES WHERE CATEGORY='WAREHOUSESHELF'");
                
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
        public void GetEquipmentLine()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT LINE FROM TAPFTEQUIPMENT");

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
        public void GetEquipmentArea()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT AREA FROM TAPFTEQUIPMENT");

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



        //ISIA.BIZ
        //--------------------------------------------------------------------------------------------------

        public void getDataBase()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT NAME ,CUSTOM01 FROM TAPCTCODES WHERE 1=1 GROUP BY CUSTOM01，NAME");

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

        public void GetParaType()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT PARAMETERTYPE FROM TAPIAPARAMETERLIST WHERE 1 = 1 GROUP BY PARAMETERTYPE");
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

        


        public void GetPara()
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT PARAMETERNAME FROM TAPIAPARAMETERLIST WHERE 1 = 1 GROUP BY PARAMETERNAME");
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
        public void GetPara(ArgumentPack arguments)
        {
            DBCommunicator db = new DBCommunicator();
            try
            {
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append("SELECT DISTINCT PARAMETERNAME FROM TAPIAPARAMETERLIST WHERE 1 = 1  ");
                if (!string.IsNullOrEmpty((string)arguments["PARAMETERTYPE"].ArgumentValue))
                {
                    //tmpSql.AppendFormat(" AND QUALITYTYPE IN ({0})", Utils.MakeSqlQueryIn((string)arguments["QUALITYTYPE"].ArgumentValue, ','));
                    tmpSql.AppendFormat(" AND PARAMETERTYPE IN ({0})", Utils.MakeSqlQueryIn2((string)arguments["PARAMETERTYPE"].ArgumentValue));
                }
                tmpSql.Append(" GROUP BY PARAMETERNAME");
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
