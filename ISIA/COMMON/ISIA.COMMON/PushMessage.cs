using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json;

namespace ISIA.COMMON
{
    public class PushMessage
    {
        #region field
        //private string urlBase = TAP.App.Base.AppConfig.ConfigManager.HostCollection["URLBASE"]["VALUE"];

        #endregion
        public static void SendMessage(MessageM msgm, string urlbase)
        {
            string url = urlbase + "api/equipment/sendmessage";
            string content = JsonConvert.SerializeObject(msgm);
            HttpContent httpContent = new StringContent(content);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;
            //return response.IsSuccessStatusCode;
        }
    }

    public class EQPartsM
    {
        public string AREA { get; set; }
        public string LINE { get; set; }
        public string MAINEQUIPMENT { get; set; }
        public string NAME { get; set; }
        public string REGION { get; set; }
        public string FACILITY { get; set; }
        public string BAY { get; set; }
        public string EQUIPMENT { get; set; }
        public string ISREUSED { get; set; }
        public string ISINSTALLED { get; set; }
        public string PARTTYPE { get; set; }
        public string VENDOR { get; set; }
        public string PARTMODEL { get; set; }
        public string STOCKINLOCATION { get; set; }
        public string GRADE { get; set; }
        public float UNITPRICE { get; set; }
        public DateTime PRODUCTIONDATE { get; set; }
        public DateTime STOCKINTIME { get; set; }
        public DateTime STOCKOUTTIME { get; set; }
        public DateTime INSTALLEDTIME { get; set; }
        public string USER { get; set; }

        public string ISALIVE_PART { get; set; }
        public string PARTSSTATUS { get; set; }
        public string PARTSERIALNO { get; set; }
        public string UNIT { get; set; }
        public int REPAIRTIMES { get; set; }
        public DateTime LASTREPAIRINTIME { get; set; }
        public DateTime LASTREPAIROUTTIME { get; set; }
        public bool NEEDREPAIR { get; set; } = false;



    }

   
    public class MessageM
    {
        public string MESSAGETYPE { get; set; }
        public string APPTYPE { get; set; }
        public EquipmentM EQUIPMENT { get; set; }
        public EQPartsM PART { get; set; }
    }


    public class EquipmentM
    {
        public string REGION { get; set; }
        public string FACILITY { get; set; }
        public string AREA { get; set; }
        public string LINE { get; set; }
        public string BAY { get; set; }
        public string MAINEQUIPMENT { get; set; }
        public string NAME { get; set; }
        public string EQUIPMENTSTATUS { get; set; }
        public string EQUIPMENTEVENT { get; set; }
        public string EQUIPMENTTYPE { get; set; }
        public string VENDOR { get; set; }
        public string SERIALNO { get; set; }
        public string PROCESSCOUNT { get; set; }
        public string DESCRIPTION { get; set; }
        public string INSTALLDATE { get; set; }
        public string EQUIPMENTSMODEL { get; set; }
        public DateTime LASTEVENTTIME { get; set; }
        public string CURRENTUSER { get; set; }
        public string UPDATEUSER { get; set; }
    }
}