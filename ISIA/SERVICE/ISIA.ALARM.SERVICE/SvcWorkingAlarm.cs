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
using TAP.Models;
using TAP.Models.Factories.Facilities;
using TAP;

namespace ISEM.ALARM.SERVICE
{
    class SvcWorkingAlarm : TAP.WinService.ServiceBase
    {
        //private string MessageUrl = "http://10.140.1.38:9000/api/equipment/SendMessage";
        private string MessageUrl = "http://isemm.hanwha-qcells.com:8000/api/equipment/SendMessage";
        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            try
            {
                DataTable tmpdt = GetEquipment();
                foreach (DataRow dr in tmpdt.Rows)
                {
                    try
                    {
                        DateTime dt_status = DateTime.ParseExact(dr["LASTEVENTTIME"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        if (dt_status.AddMinutes(10) < DateTime.Now)
                        {
                            MessageM msg = new MessageM();
                            if (dr["EQUIPMENTSTATUS"].ToString() == "CHECK DOWN")
                            {
                                msg.MESSAGETYPE = "CHECKDOWNNALARM";
                            }
                            else if (dr["EQUIPMENTSTATUS"].ToString() == "BM WAIT")
                            {
                                msg.MESSAGETYPE = "BMWAITALARM";
                            }
                            else if (dr["EQUIPMENTSTATUS"].ToString() == "PROCESS DOWN")
                            {
                                msg.MESSAGETYPE = "PROCESSDOWNALARM";
                            }

                            EquipmentM tmpeq = new EquipmentM();
                            tmpeq.NAME = dr["NAME"].ToString();
                            tmpeq.LINE = dr["LINE"].ToString();
                            tmpeq.AREA = dr["AREA"].ToString();

                            msg.EQUIPMENT = tmpeq;
                            msg.TIMEDIFF = Math.Round(DateTime.Now.Subtract(dt_status).TotalMinutes).ToString();
                            SendMessage(msg);

                        }
                    }
                    catch (AggregateException ex1)
                    {
                        continue;
                    }
                    catch (Exception)
                    {
                        continue;
                        //throw;
                    }
                    
                }
            }
            catch (Exception ex)
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
            catch (AggregateException ex1)
            {
                return;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable GetEquipment()
        {
            try
            {
                EquipmentModel eqModel = new EquipmentModel();
                DataTable tmpdt = eqModel.LoadModelDataList(new ArgumentPack());
                tmpdt.DefaultView.RowFilter = "EQUIPMENTSTATUS IN ('CHECK DOWN','BM WAIT','PROCESS DOWN')";
                tmpdt = tmpdt.DefaultView.ToTable();
                return tmpdt;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
