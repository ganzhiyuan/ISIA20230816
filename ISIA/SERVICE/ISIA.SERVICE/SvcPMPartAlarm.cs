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
    class SvcPMPartAlarm : TAP.WinService.ServiceBase
    {
        private string MessageUrl = "http://ISIAm.hanwha-qcells.com:8000//api/equipment/SendMessage";
        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            try
            {
                string dateTom = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
                string date2Mon = DateTime.Now.AddMonths(2).ToString("yyyyMMdd");
                DataTable tmpdt = GetPMAlarm(dateTom, date2Mon);
                foreach (DataRow dr in tmpdt.Rows)
                {
                    MessageM msg = new MessageM();
                    msg.MESSAGETYPE = "PMPARTSALARM";
                    EquipmentM tmpeq = new EquipmentM();
                    tmpeq.NAME = dr["EQUIPMENT"].ToString();                    
                    tmpeq.SERIALNO = dr["SAPCODE"].ToString();
                    tmpeq.PROCESSCOUNT = dr["COUNT"].ToString();
                    tmpeq.DESCRIPTION = dr["PARTNAME"].ToString();
                    

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

        private DataTable GetPMAlarm(string dateTom, string date2Mon)
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();

                tmpsb.Append("SELECT C.EQUIPMENT, C.PMSCHEDULE, C.PMTIME, D.SAPCODE, D.PARTNAME, D.COUNT FROM ");
                tmpsb.Append("( ");
                tmpsb.Append("SELECT A.EQUIPMENT, A.PMSCHEDULE, A.PMTIME, B.EQUIPMENTSMODEL, B.AREA FROM  TAPFTPMSCHEDULE A, TAPFTEQUIPMENT B ");
                tmpsb.Append("WHERE A.EQUIPMENT = B.NAME ");
                tmpsb.AppendFormat("AND (PMTIME LIKE '{0}' OR PMTIME LIKE '{1}') ", dateTom, date2Mon);
                tmpsb.Append(") C , TAPFTPMPARTSALARM D ");
                tmpsb.Append("WHERE C.EQUIPMENTSMODEL = D.EQUIPMENTMODEL ");
                tmpsb.Append("AND C.AREA = D.AREA ");
                tmpsb.Append("AND C.PMSCHEDULE = D.PMSCHEDULE ");

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
