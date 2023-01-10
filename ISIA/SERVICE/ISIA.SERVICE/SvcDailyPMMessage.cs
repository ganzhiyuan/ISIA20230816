using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Models.SystemBasic;
using TAP.Base.Communication;
using System.IO;
using TAP.Data.DataBase.Communicators;
using System.Data;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ISIA.SERVICE
{
    class SvcDailyPMMessage : TAP.WinService.ServiceBase
    {
        //private string MessageUrl = "http://10.140.1.38:9000/api/equipment/SendMessage";
        private string MessageUrl = "http://ISIAm.hanwha-qcells.com:8000//api/equipment/SendMessage";
        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            try
            {
                DataTable tmpdt = GetPMSchedule();
                foreach (DataRow dr in tmpdt.Rows)
                {
                    MessageM msg = new MessageM();
                    msg.MESSAGETYPE = "DAILYPM";
                    EquipmentM tmpeq = new EquipmentM();
                    tmpeq.NAME = dr["EQUIPMENT"].ToString();
                    tmpeq.AREA = dr["AREA"].ToString();

                    msg.EQUIPMENT = tmpeq;

                    msg.PMSCHEDULE = dr["PMSCHEDULE"].ToString();
                    msg.PMTIME = dr["PMTIME"].ToString();
                    SendMessage(msg);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetPMSchedule()
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();

                tmpsb.Append("SELECT REGION, FACILITY, LINE, AREA, MAINEQUIPMENT, EQUIPMENT, PMSCHEDULE, PMTIME FROM  TAPFTPMSCHEDULE ");
                tmpsb.AppendFormat("WHERE PMTIME LIKE  '{0}%' ", DateTime.Now.ToString("yyyyMMdd"));

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
    }
}
