using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Models.SystemBasic;
using TAP.Base.Communication;
using TAP.Data.DataBase.Communicators;
using System.Data;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using TAP.Models.Factories.Facilities;
using TAP;

namespace ISIA.SERVICE
{
    class SvcDailyPmAlarm : TAP.WinService.ServiceBase
    {

        //private string MessageUrl = "http://10.140.1.38:9000/api/equipment/SendMessage";
        private string MessageUrl = "http://ISIAm.hanwha-qcells.com:8000//api/equipment/SendMessage";
        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            try
            {
                string Shift_Now = CheckTime();
                if (Shift_Now == "")
                {
                    return;
                }

                DataTable dtEQ = GetEquipmentShift();
                foreach (DataRow dr in dtEQ.Rows)
                {
                    if (Shift_Now == "NIGHT" && dr["CUSTOM01"].ToString() == "DAILY")
                    {
                        continue;
                    }
                    else if(Shift_Now == "DAY")
                    {
                        string time = DateTime.Now.ToString("yyyyMMddHHmmss").Substring(0,8) + "073000";
                        DataTable tmpdt = GetPMResult(time, dr);
                        if (tmpdt.Rows.Count == 0)
                        {
                            CreateMessage(dr);
                        }
                    }
                    else if(Shift_Now == "NIGHT" && dr["CUSTOM01"].ToString() == "SHIFT")
                    {
                        string time = DateTime.Now.ToString("yyyyMMddHHmmss").Substring(0, 8) + "193000";
                        DataTable tmpdt = GetPMResult(time, dr);
                        if (tmpdt.Rows.Count == 0)
                        {
                            CreateMessage(dr);
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public DataTable GetPMResult(string time, DataRow dr)
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();

                tmpsb.Append("SELECT * FROM TAPFTPMRESULT ");
                tmpsb.AppendFormat("WHERE TIMEKEY > '{0}' ", time);
                tmpsb.AppendFormat("AND  REGION = '{0}' ", dr["REGION"]);
                tmpsb.AppendFormat("AND  FACILITY = '{0}' ", dr["FACILITY"]);
                tmpsb.AppendFormat("AND  LINE = '{0}' ", dr["LINE"]);
                tmpsb.AppendFormat("AND  AREA = '{0}' ", dr["AREA"]);
                tmpsb.AppendFormat("AND  BAY = '{0}' ", dr["BAY"]);
                tmpsb.AppendFormat("AND  MAINEQUIPMENT = '{0}' ", dr["MAINEQUIPMENT"]);
                tmpsb.AppendFormat("AND  EQUIPMENT = '{0}' ", dr["NAME"]);
                tmpsb.Append("AND PMSCHEDULE = 'DAILY' ");

                DataTable tmpdt = db.Select(tmpsb.ToString()).Tables[0];

                return tmpdt;
            }
            catch (Exception)
            {

                throw;
            }

        }

        async public void SendMessage(MessageM msg)
        {
            try
            {
                string content = JsonConvert.SerializeObject(msg);
                HttpContent httpContent = new StringContent(content);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpClient httpClient = new HttpClient();
                string retVal = await httpClient.PostAsync(MessageUrl, httpContent).Result.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string CheckTime()
        {
            string Shift = "";
            try
            {
                if (DateTime.Now.Hour == 13)
                {
                    Shift = "DAY";
                }
                else if (DateTime.Now.Hour == 22)
                {
                    Shift = "NIGHT";
                }
                return Shift;
            }
            catch (Exception)
            {
                return "";
            }


            
        }

        public DataTable GetEquipmentShift()
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();

                tmpsb.Append("SELECT A.REGION, A.FACILITY,A.LINE, A.AREA, A.BAY, A.MAINEQUIPMENT,A.NAME, B.CUSTOM01 FROM TAPFTEQUIPMENT A, TAPCTCODES B ");
                tmpsb.Append("WHERE A.ISALIVE = 'YES' ");
                tmpsb.Append("AND B.CATEGORY = 'DAILYPMTYPE' ");
                tmpsb.Append("AND A.AREA = B.SUBCATEGORY ");
                tmpsb.Append("AND A.EQUIPMENTTYPE = B.NAME ");

                DataTable tmpdt = db.Select(tmpsb.ToString()).Tables[0];

                return tmpdt;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void CreateMessage(DataRow dr)
        {
            MessageM msg = new MessageM();
            msg.MESSAGETYPE = "DAILYPM";
            EquipmentM tmpeq = new EquipmentM();
            tmpeq.NAME = dr["NAME"].ToString();
            tmpeq.AREA = dr["AREA"].ToString();

            msg.EQUIPMENT = tmpeq;

            msg.PMSCHEDULE = "DAILY";
            msg.PMTIME = DateTime.Now.ToString("yyyyMMdd");
            SendMessage(msg);
        }

    }


}

