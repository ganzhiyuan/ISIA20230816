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
using TAP.WinService;

namespace ISIA.SERVICE
{
    class SvcSTATUSHISTORYDataDailySummary : TAP.WinService.ServiceBase
    {
        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            DataTable dt = GetSummaryData();
            SaveSummaryData(dt);

            DeleteData();

        }
        string date = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");

        private void DeleteData()
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();
                string date = DateTime.Now.AddDays(-2).ToString("yyyyMMdd");

                tmpsb.Append("DELETE FROM TAPIFTB_OXEQP ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.AppendFormat("AND REPORT_DATE < '{0}' ", date);

                db.Save(new string[] { tmpsb.ToString() });

                StringBuilder tmpsb1 = new StringBuilder();
                tmpsb1.Append("DELETE FROM TAPIFTB_DFEQP ");
                tmpsb1.Append("WHERE 1=1 ");
                tmpsb1.AppendFormat("AND REPORT_DATE < '{0}' ", date);

                db.Save(new string[] { tmpsb1.ToString() });

                StringBuilder tmpsb2 = new StringBuilder();
                tmpsb2.Append("DELETE FROM TAPIFTB_TSEQP ");
                tmpsb2.Append("WHERE 1=1 ");
                tmpsb2.AppendFormat("AND REPORT_DATE < '{0}' ", date);

                db.Save(new string[] { tmpsb2.ToString() });

            }
            catch (Exception ex)
            {
                this.SaveLog(ServiceBase.ERROR_LOG, "SvcSTATUSHISTORYDataDailySummary - DeleteData", string.Format("Error:{2}", ex.ToString()));
                throw;
            }

        }
        private DataTable GetSummaryData()
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();
                tmpsb.Append("SELECT AREA,LINE,NAME,REGION,FACILITY,SHIFT,OLDEQUIPMENTSTATUS AS PARAMETER_NAME, ");
                tmpsb.Append("TRIM(TO_CHAR(SUM(RUNNINGTIME),999999)) AS PARAMETER_VALUE FROM TAPEQP_STATUSHISTORY ");
                tmpsb.AppendFormat("WHERE REPORT_DATE like '{0}%' ", date);
                tmpsb.Append("AND OLDEQUIPMENTSTATUS IN ('IDLE','DOWN','RUN')");
                tmpsb.Append("AND TUBE ='ALL' ");
                tmpsb.Append("GROUP BY  AREA,LINE,NAME,REGION,FACILITY,SHIFT,OLDEQUIPMENTSTATUS ");
                tmpsb.Append("UNION ALL ");
                tmpsb.Append("SELECT AREA,LINE,NAME,REGION,FACILITY,SHIFT,OLDEQUIPMENTSTATUS AS PARAMETER_NAME, ");
                tmpsb.Append("TRIM(TO_CHAR(SUM(RUNNINGTIME)/5,999999)) AS PARAMETER_VALUE FROM TAPEQP_STATUSHISTORY ");
                tmpsb.AppendFormat("WHERE REPORT_DATE like '{0}%' ", date);
                tmpsb.Append("AND OLDEQUIPMENTSTATUS IN ('IDLE','DOWN','RUN')");
                tmpsb.Append("AND TUBE !='ALL' ");
                tmpsb.Append("GROUP BY  AREA,LINE,NAME,REGION,FACILITY,SHIFT,OLDEQUIPMENTSTATUS ");
                tmpsb.Append("ORDER BY NAME ASC ");

                DataTable tmpdt = db.Select(tmpsb.ToString()).Tables[0];

                return tmpdt;
            }
            catch (Exception ex)
            {
                this.SaveLog(ServiceBase.ERROR_LOG, "SvcSTATUSHISTORYDataDailySummary - GetSummaryData", string.Format("Error:{2}", ex.ToString()));
                throw;
            }
        }

        private void SaveSummaryData(DataTable dtSum)
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                List<string> SqlList = new List<string>();

                foreach (DataRow dr in dtSum.Rows)
                {
                    StringBuilder tmpsb = new StringBuilder();
                    tmpsb.Append("INSERT INTO  TAPEQDATASUMM VALUES(");
                    tmpsb.AppendFormat("'{0}',", dr["LINE"].ToString());
                    tmpsb.AppendFormat("'{0}',", dr["AREA"].ToString());
                    tmpsb.AppendFormat("'{0}',", date);
                    tmpsb.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpsb.AppendFormat("'{0}',", dr["NAME"].ToString());
                    tmpsb.AppendFormat("'{0}',", "ALL");
                    tmpsb.AppendFormat("'{0}',", "ALL");
                    tmpsb.AppendFormat("'{0}',", "ALL");
                    tmpsb.AppendFormat("'{0}',", "ALL");
                    tmpsb.AppendFormat("'{0}',", dr["PARAMETER_NAME"].ToString() + "_TIME");
                    tmpsb.AppendFormat("'{0}',", dr["PARAMETER_VALUE"].ToString());
                    tmpsb.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    SqlList.Add(tmpsb.ToString());
                }

                db.Save(SqlList);
            }
            catch (Exception ex)
            {
                this.SaveLog(ServiceBase.ERROR_LOG, "SvcSTATUSHISTORYDataDailySummary - SaveSummaryData", string.Format("Error:{2}", ex.ToString()));
                throw;
            }
        }
    }
}
