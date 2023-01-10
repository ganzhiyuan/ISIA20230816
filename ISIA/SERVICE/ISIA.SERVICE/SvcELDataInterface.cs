using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Data.DataBase.Communicators;
using TAP.Models.SystemBasic;
using TAP.WinService;
using System.Data.SqlClient;
using System.Data.Common;
using static System.Net.Mime.MediaTypeNames;
using TAP.Models;
using System.Reflection;
using TAP.Base.Communication;
using NPOI.SS.Formula.PTG;
using NPOI.SS.UserModel;
using System.Drawing;
using System.Security.Cryptography;
using System.Diagnostics;
using NPOI.SS.Formula.Functions;

namespace ISIA.SERVICE
{
    class SvcELDataInterface : TAP.WinService.ServiceBase
    {
        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            try
            {
                InsertDBData();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private string GetLastTime(string Equipment)
        {
            string retval = "0";

            try
            {
                DBCommunicator db = new DBCommunicator();

                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT DECODE(MAX(TESTTIMEKEY), null,null,MAX(TESTTIMEKEY))  AS TIMEKEY FROM TAPEQP_TS_DBDATA WHERE 1 = 1 ");
                sb.AppendFormat(" AND EQUIPMENT = '{0}'", Equipment);
                //sb.AppendFormat(" AND TESTTIMEKEY > to_char(sysdate-10,'yyyyMMddHH24miss')"); //CurrentDay -10 day

                DataTable dt = db.Select(sb.ToString()).Tables[0];

                if(dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0]["TIMEKEY"].ToString()))
                {
                    string temp = dt.Rows[0]["TIMEKEY"].ToString();
                    retval = temp.Substring(0,4)+"-"+temp.Substring(4, 2) + "-" +temp.Substring(6, 2) + " " + temp.Substring(8, 2) + ":"+temp.Substring(10, 2) + ":" +temp.Substring(12, 2);
                }

                return retval;
            }
            catch(System.Exception)
            {
                return retval;
            }
        }
        private string NullForEmpty(string str)
        {
            string retval = "null";

            if(string.IsNullOrEmpty(str))
            {
                retval = "null";
            }
            else
            {
                retval = str;
            }
            return retval;
        }
        private string ChangeTimekey(string str)
        {
            string retval = "null";

            if (string.IsNullOrEmpty(str))
            {
                retval = "null";
            }
            else
            {
                string Timekey = str.Substring(0, 4) + str.Substring(5, 2) + str.Substring(8, 2) + str.Substring(11, 2) + str.Substring(14, 2) + str.Substring(17, 2);
                retval = Timekey;
            }
            return retval;
        }


        private async Task InsertData(DataRow dr)
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                var sqlConnectionSB = new SqlConnectionStringBuilder();


                string timekey = GetLastTime(dr["CUSTOM01"].ToString());

                // Change these values to your values.  
                sqlConnectionSB.DataSource = string.Format("{0}{1}", "tcp:", dr["CUSTOM02"].ToString()); //["Server"]  
                sqlConnectionSB.InitialCatalog = dr["CUSTOM03"].ToString(); //["Database"]  

                sqlConnectionSB.UserID = "halm";  // "@yourservername"  as suffix sometimes.  
                sqlConnectionSB.Password = "halm";
                sqlConnectionSB.IntegratedSecurity = false;

                // Adjust these values if you like. (ADO.NET 4.5.1 or later.)  
                //sqlConnectionSB.ConnectRetryCount = 3;
                //sqlConnectionSB.ConnectRetryInterval = 10;  // Seconds.  

                // Leave these values as they are.  
                sqlConnectionSB.IntegratedSecurity = false;
                sqlConnectionSB.Encrypt = false;
                sqlConnectionSB.ConnectTimeout = 1000;

                var sqlConnection = new SqlConnection(sqlConnectionSB.ToString());
                var dbCommand = sqlConnection.CreateCommand();
                List<string> sqllist = new List<string>();
                if (timekey == "0")
                {
                    dbCommand.CommandText = string.Format(@"SELECT [UniqueID] ,[CellIDStr] ,CONVERT(CHAR(19), [TestTimeDate], 120) ,[BIN]
                                                                  ,[Uoc] ,[Isc] ,[Rs] ,[Rsh] ,[FF] ,[Eta] ,[IRev1] ,[IRev2] ,[Tcell] ,[Tenv] ,[Tmonicell]
                                                                  ,[Insol] ,[Comment] ,[IRTmax] ,[IRTmaxPosX] ,[IRTmaxPosY] ,[IRCamMaxIrev2] ,[Title] ,[BatchID]
                                                                  ,[Pmpp] ,[Umpp] ,[Impp] ,[Jsc] ,[Iap] ,[Uap] ,[URev1] ,[URev2] ,[CellTyp] ,[Classification]
                                                                  ,[IRBin] ,[AOI_1_Q] ,[AOI_2_R] ,[AOI_2_Q] ,[ELBin] ,[EL2Class] ,[ELMeanGray] ,[EL2CrackDefaultCount]
                                                                  ,[EL2DarkDefaultArea] ,[EL2FingerDefaultArea] ,[EL2FingerDefaultCount] ,[EL2ContactIssue] ,[EL2Contamination]
                                                                  ,[EL2DarkDefaultCount] ,[EL2DarkDefaultMaxArea] ,[EL2DarkDefaultSeverity] ,[EL2EvalRecipe] ,[EL2FingerDefaultSeverity]
                                                                  ,[EL2Firing] ,[EL2GripperMark] ,[EL2OxRing] ,[EL2Sawingmark] ,[EL2ScratchDefaultArea] ,[EL2ScratchDefaultCount] ,[EL2ScratchDefaultSeverity]
                                                                  ,[EL2SpotDefaultArea] ,[EL2SpotDefaultCount] ,[EL2SpotDefaultSeverity] ,[EL2TimeEvaluation] ,[ELBinComment] ,[ELCamExposureTime]
                                                                  ,[ELCamGain] ,[ELCamModel] ,[ELCamSerial] ,[ELCamTemp] ,[ELCurrent] ,[ELRecipeName] ,[ELVoltage]
                                                                  ,[ELVoltageQ3H] FROM [halm_tables].[dbo].[halm_results] WHERE TESTTIMEDATE > DateAdd(Day,-2,GETDATE())");
                }
                else
                {
                    dbCommand.CommandText = string.Format(@"SELECT [UniqueID] ,[CellIDStr] ,CONVERT(CHAR(19), [TestTimeDate], 120) ,[BIN]
                                                                  ,[Uoc] ,[Isc] ,[Rs] ,[Rsh] ,[FF] ,[Eta] ,[IRev1] ,[IRev2] ,[Tcell] ,[Tenv] ,[Tmonicell]
                                                                  ,[Insol] ,[Comment] ,[IRTmax] ,[IRTmaxPosX] ,[IRTmaxPosY] ,[IRCamMaxIrev2] ,[Title] ,[BatchID]
                                                                  ,[Pmpp] ,[Umpp] ,[Impp] ,[Jsc] ,[Iap] ,[Uap] ,[URev1] ,[URev2] ,[CellTyp] ,[Classification]
                                                                  ,[IRBin] ,[AOI_1_Q] ,[AOI_2_R] ,[AOI_2_Q] ,[ELBin] ,[EL2Class] ,[ELMeanGray] ,[EL2CrackDefaultCount]
                                                                  ,[EL2DarkDefaultArea] ,[EL2FingerDefaultArea] ,[EL2FingerDefaultCount] ,[EL2ContactIssue] ,[EL2Contamination]
                                                                  ,[EL2DarkDefaultCount] ,[EL2DarkDefaultMaxArea] ,[EL2DarkDefaultSeverity] ,[EL2EvalRecipe] ,[EL2FingerDefaultSeverity]
                                                                  ,[EL2Firing] ,[EL2GripperMark] ,[EL2OxRing] ,[EL2Sawingmark] ,[EL2ScratchDefaultArea] ,[EL2ScratchDefaultCount] ,[EL2ScratchDefaultSeverity]
                                                                  ,[EL2SpotDefaultArea] ,[EL2SpotDefaultCount] ,[EL2SpotDefaultSeverity] ,[EL2TimeEvaluation] ,[ELBinComment] ,[ELCamExposureTime]
                                                                  ,[ELCamGain] ,[ELCamModel] ,[ELCamSerial] ,[ELCamTemp] ,[ELCurrent] ,[ELRecipeName] ,[ELVoltage]
                                                                  ,[ELVoltageQ3H] FROM [halm_tables].[dbo].[halm_results] WHERE TESTTIMEDATE > '{0}'", timekey);
                }


                sqlConnection.Open();
                var dataReader = dbCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    StringBuilder tmpsb = new StringBuilder();
                    if(string.IsNullOrEmpty(dataReader.GetValue(1).ToString()) || string.IsNullOrWhiteSpace(dataReader.GetValue(1).ToString()))
                    {
                        continue;
                    }
                    tmpsb.Append("INSERT INTO TAPEQP_TS_DBDATA(");
                    tmpsb.Append("UNIQUEID, REGION, FACILITY, SHIFT, ");
                    tmpsb.Append("AREA, LINE, EQUIPMENT,");
                    tmpsb.Append("WAFER_ID, TESTTIMEKEY, BIN,");
                    tmpsb.Append("UOC, ISC, RS,");
                    tmpsb.Append("RSH, FF, ETA,");
                    tmpsb.Append("IREV1, IREV2, TCELL,");
                    tmpsb.Append("TENV, TMONICELL, INSOL,");
                    tmpsb.Append("\"COMMENT\", IRTMAX, IRTMAXPOSX,");
                    tmpsb.Append("IRTMAXPOSY, IRCAMMAXIREV2, TITLE,");
                    tmpsb.Append("BATCHID, PMPP, UMPP,");
                    tmpsb.Append("IMPP, JSC, IAP,");
                    tmpsb.Append("UAP, UREV1, UREV2,");
                    tmpsb.Append("CELLTYP, CLASSIFICATION, IRBIN,");
                    tmpsb.Append("AOI_1_Q, AOI_2_R, AOI_2_Q,");
                    tmpsb.Append("ELBIN, EL2CLASS, ELMEANGRAY,");
                    tmpsb.Append("EL2CRACKDEFAULTCOUNT, EL2DARKDEFAULTAREA, EL2FINGERDEFAULTAREA,");
                    tmpsb.Append("EL2FINGERDEFAULTCOUNT, EL2CONTACTISSUE, EL2CONTAMINATION,");
                    tmpsb.Append("EL2DARKDEFAULTCOUNT, EL2DARKDEFAULTMAXAREA, EL2DARKDEFAULTSEVERITY,");
                    tmpsb.Append("EL2EVALRECIPE, EL2FINGERDEFAULTSEVERITY, EL2FIRING,");
                    tmpsb.Append("EL2GRIPPERMARK, EL2OXRING, EL2SAWINGMARK,");
                    tmpsb.Append("EL2SCRATCHDEFAULTAREA, EL2SCRATCHDEFAULTCOUNT, EL2SCRATCHDEFAULTSEVERITY,");
                    tmpsb.Append("EL2SPOTDEFAULTAREA, EL2SPOTDEFAULTCOUNT, EL2SPOTDEFAULTSEVERITY,");
                    tmpsb.Append("EL2TIMEEVALUATION, ELBINCOMMENT, ELCAMEXPOSURETIME,");
                    tmpsb.Append("ELCAMGAIN, ELCAMMODEL, ELCAMSERIAL,");
                    tmpsb.Append("ELCAMTEMP, ELCURRENT, ELRECIPENAME,");
                    tmpsb.Append("ELVOLTAGE, ELVOLTAGEQ3H, INSERTTIME,");
                    tmpsb.Append("UPDATETIME, INSERTUSER, UPDATEUSER)");
                    tmpsb.Append("VALUES( ");
                    tmpsb.AppendFormat("HALM_SEQ.NEXTVAL, '{0}', '{1}', FN_GET_WORKSHIFT2('{2}')", "CELL", dr["SUBCATEGORY"].ToString().Substring(0, 1), ChangeTimekey(dataReader.GetValue(2).ToString()));
                    tmpsb.AppendFormat(", '{0}', '{1}', '{2}'", dr["NAME"].ToString(), dr["SUBCATEGORY"].ToString(), dr["CUSTOM01"].ToString());
                    tmpsb.AppendFormat(", '{0}', '{1}', {2}", dataReader.GetValue(1).ToString(), ChangeTimekey(dataReader.GetValue(2).ToString()), NullForEmpty(dataReader.GetValue(3).ToString()));
                    tmpsb.AppendFormat(", {0}, {1}, {2}", NullForEmpty(dataReader.GetValue(4).ToString()), NullForEmpty(dataReader.GetValue(5).ToString()), NullForEmpty(dataReader.GetValue(6).ToString()));
                    tmpsb.AppendFormat(", {0}, {1}, {2}", NullForEmpty(dataReader.GetValue(7).ToString()), NullForEmpty(dataReader.GetValue(8).ToString()), NullForEmpty(dataReader.GetValue(9).ToString()));
                    tmpsb.AppendFormat(", {0}, {1}, {2}", NullForEmpty(dataReader.GetValue(10).ToString()), NullForEmpty(dataReader.GetValue(11).ToString()), NullForEmpty(dataReader.GetValue(12).ToString()));
                    tmpsb.AppendFormat(", {0}, {1}, {2}", NullForEmpty(dataReader.GetValue(13).ToString()), NullForEmpty(dataReader.GetValue(14).ToString()), NullForEmpty(dataReader.GetValue(15).ToString()));
                    tmpsb.AppendFormat(", '{0}', {1}, {2}", dataReader.GetValue(16).ToString().Trim(), NullForEmpty(dataReader.GetValue(17).ToString()), NullForEmpty(dataReader.GetValue(18).ToString()));
                    tmpsb.AppendFormat(", {0}, {1}, '{2}'", NullForEmpty(dataReader.GetValue(19).ToString()), NullForEmpty(dataReader.GetValue(20).ToString()), dataReader.GetValue(21).ToString());
                    tmpsb.AppendFormat(", {0}, {1}, {2}", NullForEmpty(dataReader.GetValue(22).ToString()), NullForEmpty(dataReader.GetValue(23).ToString()), NullForEmpty(dataReader.GetValue(24).ToString()));
                    tmpsb.AppendFormat(", {0}, {1}, {2}", NullForEmpty(dataReader.GetValue(25).ToString()), NullForEmpty(dataReader.GetValue(26).ToString()), NullForEmpty(dataReader.GetValue(27).ToString()));
                    tmpsb.AppendFormat(", {0}, {1}, {2}", NullForEmpty(dataReader.GetValue(28).ToString()), NullForEmpty(dataReader.GetValue(29).ToString()), NullForEmpty(dataReader.GetValue(30).ToString()));
                    tmpsb.AppendFormat(", '{0}', '{1}', {2}", dataReader.GetValue(31).ToString(), dataReader.GetValue(32).ToString(), NullForEmpty(dataReader.GetValue(33).ToString()));
                    tmpsb.AppendFormat(", {0}, {1}, {2}", NullForEmpty(dataReader.GetValue(34).ToString()), NullForEmpty(dataReader.GetValue(35).ToString()), NullForEmpty(dataReader.GetValue(36).ToString()));
                    tmpsb.AppendFormat(", {0}, {1}, {2}", NullForEmpty(dataReader.GetValue(37).ToString()), NullForEmpty(dataReader.GetValue(38).ToString()), NullForEmpty(dataReader.GetValue(39).ToString()));
                    tmpsb.AppendFormat(", {0}, {1}, {2}", NullForEmpty(dataReader.GetValue(40).ToString()), NullForEmpty(dataReader.GetValue(41).ToString()), NullForEmpty(dataReader.GetValue(42).ToString()));
                    tmpsb.AppendFormat(", {0}, {1}, {2}", NullForEmpty(dataReader.GetValue(43).ToString()), NullForEmpty(dataReader.GetValue(44).ToString()), NullForEmpty(dataReader.GetValue(45).ToString()));
                    tmpsb.AppendFormat(", {0}, {1}, {2}", NullForEmpty(dataReader.GetValue(46).ToString()), NullForEmpty(dataReader.GetValue(47).ToString()), NullForEmpty(dataReader.GetValue(48).ToString()));
                    tmpsb.AppendFormat(", '{0}', {1}, {2}", dataReader.GetValue(49).ToString(), NullForEmpty(dataReader.GetValue(50).ToString()), NullForEmpty(dataReader.GetValue(51).ToString()));
                    tmpsb.AppendFormat(", {0}, {1}, {2}", NullForEmpty(dataReader.GetValue(52).ToString()), NullForEmpty(dataReader.GetValue(53).ToString()), NullForEmpty(dataReader.GetValue(54).ToString()));
                    tmpsb.AppendFormat(", {0}, {1}, {2}", NullForEmpty(dataReader.GetValue(55).ToString()), NullForEmpty(dataReader.GetValue(56).ToString()), NullForEmpty(dataReader.GetValue(57).ToString()));
                    tmpsb.AppendFormat(", {0}, {1}, {2}", NullForEmpty(dataReader.GetValue(58).ToString()), NullForEmpty(dataReader.GetValue(59).ToString()), NullForEmpty(dataReader.GetValue(60).ToString()));
                    tmpsb.AppendFormat(", {0}, '{1}', {2}", NullForEmpty(dataReader.GetValue(61).ToString()), dataReader.GetValue(62).ToString(), NullForEmpty(dataReader.GetValue(63).ToString()));
                    tmpsb.AppendFormat(", {0}, '{1}', '{2}'", NullForEmpty(dataReader.GetValue(64).ToString()), dataReader.GetValue(65).ToString(), NullForEmpty(dataReader.GetValue(66).ToString()));
                    tmpsb.AppendFormat(", {0}, {1}, '{2}'", NullForEmpty(dataReader.GetValue(67).ToString()), NullForEmpty(dataReader.GetValue(68).ToString()), dataReader.GetValue(69).ToString());
                    tmpsb.AppendFormat(", {0}, {1}, to_char(sysdate,'YYYYMMDDHH24MISS')", NullForEmpty(dataReader.GetValue(70).ToString()), NullForEmpty(dataReader.GetValue(71).ToString()));
                    tmpsb.AppendFormat(", null, 'Service', null)");

                    sqllist.Add(tmpsb.ToString());

                    if (sqllist.Count >= 10000)
                    {
                        db.Save(sqllist);
                        sqllist.Clear();
                    }
                }
                db.Save(sqllist);

                Console.WriteLine(string.Format("Equipment : {0} completed.", dr["CUSTOM01"].ToString()));

                dataReader.Close();
                sqlConnection.Close();
                //int index = 0;
                //int count = 0;
                ////  count = dbb.Save(SqlList.GetRange(0, 1));

                //while (true)
                //{
                //    if ((index + 2000) < sqllist.Count - 1)
                //    {

                //        count += db.Save(sqllist.GetRange(index, 2000));
                //        index += 2000;
                //    }
                //    else
                //    {
                //        count += db.Save(sqllist.GetRange(index, sqllist.Count - index));
                //        break;
                //    }
                //}
            }
            catch(System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                SaveLog(ServiceBase.ERROR_LOG, "TSHalmInterface", ex.ToString());
            }
        }

        private void InsertDBData()
        {
            try
            {
                DBCommunicator db = new DBCommunicator();

                string connectString = "SELECT * FROM TAPCTCODES WHERE CATEGORY='HALMDB_ADDRESS'";

                DataTable connectDB = db.Select(connectString).Tables[0];

                List<Task> tasks = new List<Task>();
                foreach (DataRow dr in connectDB.Rows)
                {
                    //InsertData(dr);

                    Task.Run(() => InsertData(dr)); 
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
