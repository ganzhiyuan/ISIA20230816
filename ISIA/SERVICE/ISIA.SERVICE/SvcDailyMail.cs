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
using System.Drawing;

namespace ISIA.SERVICE
{
    class SvcDailyMail : TAP.WinService.ServiceBase
    {
        private const int _MailCount = 50;
        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            SendDailyMail();
        }
        private void SendDailyMail()
        {
            //test
            DailyRatioReropt dr = new DailyRatioReropt();
            string filename = dr.EqCompute();
            DataTable mailGroupDt = GetMailGroup();
            mailGroupDt.DefaultView.RowFilter = "MAILADDRESS <> '' AND MAILADDRESS IS NOT NULL";
            DataTable tmpdt = mailGroupDt.DefaultView.ToTable();
            Mail mail = new TAP.Base.Communication.Mail();
            var mailGroupList = (from d in tmpdt.AsEnumerable() select d.Field<string>("MAILADDRESS")).ToList();
            if (File.Exists(filename))
            {
                int cnt = mailGroupList.Count;
                List<string> tmpmaillist;
                for (int i = 0; i <= cnt; i += _MailCount)
                {
                    if (i <= cnt - _MailCount)
                    {
                        tmpmaillist = mailGroupList.GetRange(i, _MailCount);

                    }
                    else
                    {
                        tmpmaillist = mailGroupList.GetRange(i, cnt - i);
                    }
                    string message = "<img src=\"cid:{0}\"/>";
                    List<string> fileNames = new List<string>();
                    fileNames.Add(filename);
                    string imagepath = filename.Replace("XLSX", "Jpeg");
                    fileNames.Add(imagepath);
                    if (tmpmaillist.Count > 0)
                    {
                        mail.SendMail("EQUIPMENT OPERATING RATIO  REPORT(SP I/F DATA) - " + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), TAP.Base.Configuration.ConfigurationManager.Instance.SMTPSection.SMTPServerList["DEFAULT"].UserID, message, tmpmaillist, Encoding.UTF8, true, fileNames, true);
                    }
                    
                }
                //Mail mail = new TAP.Base.Communication.Mail();
                //List<string> tmpmaillist = new List<string>();
                //tmpmaillist.Add("906353071@qq.com");
            }
        }
        private DataTable GetMailGroup()
        {
            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpsb = new StringBuilder();
            tmpsb.Append("SELECT  MAILADDRESS FROM TAPUTUSERS U,(");
            tmpsb.Append(" SELECT USERID, USERNAME FROM TAPUTMAILGROUP G, TAPUTMAILGROUPMEMBER N WHERE G.NAME = N.GROUPNAME  AND G.REGION = N.REGION AND G.NAME in (");
            tmpsb.Append(" SELECT NAME FROM TAPCTCODES WHERE CATEGORY = 'MAIL' AND SUBCATEGORY = 'DAILY' AND ISALIVE = 'YES') )GN WHERE U.NAME = GN.USERID");
            //tmpsb.Append(" AND U.USERNAME = GN.USERNAME");
            DataTable mailGroupDt = db.Select(tmpsb.ToString()).Tables[0];
            //mailGroupDt.Rows.Add(new object[] { "1066307152@qq.com" });
            //mailGroupDt.Rows.Add(new object[] { "906353071@qq.com" });
            //mailGroupDt.Rows.Add(new object[] { "kim.seoil@iset-da.com" });
            //mailGroupDt.Rows.Add(new object[] { "876160944@qq.com" });
            //mailGroupDt.Rows.Add(new object[] { "juan.ye@hanwha-qcells.com" });
            //mailGroupDt.Rows.Add(new object[] { "kookhyoun.lim@hanwha-qcells.com" });
            return mailGroupDt;
        }
    }
}
