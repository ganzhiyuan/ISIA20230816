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
using NPOI.SS.Formula.Functions;
using static NPOI.HSSF.Util.HSSFColor;
using Excel = Microsoft.Office.Interop.Excel;

namespace ISIA.SERVICE
{
    class SvcDailyMailLama : TAP.WinService.ServiceBase
    {
        private const int _MailCount = 50;
        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            SendDailyMailLama();
        }
        private void SendDailyMailLama()
        {
            //test
            DailyLamaRatio dr = new DailyLamaRatio();
            string filename = dr.EqCompute();
            Mail mail = new TAP.Base.Communication.Mail();

            if (File.Exists(filename))
            {
                string htmlpath = filename.Replace("XLSX", "html");

                string fileContent = null; 
                using (StreamReader sr = new StreamReader(htmlpath))
                { 
                    fileContent = sr.ReadToEnd(); 
                    sr.Close(); 
                }
            
                string message = fileContent;
                List<string> fileNames = new List<string>();
                List<string> Maillist = new List<string>();
                List<string> ccList = new List<string>();
                fileNames.Add(filename);

                string imagepath = filename.Replace(".XLSX", "_files");
                //fileNames.Add(filename.Replace(".XLSX", ".Jpeg"));
                fileNames.Add(imagepath + @"\Image1.png");
                fileNames.Add(imagepath + @"\Image2.png");

                string[] tempname = imagepath.Split('\\');
                message = message.Replace(tempname[4] + "/Image1.png", "cid:Image1.png");
                message = message.Replace(tempname[4] + "/Image2.png", "cid:Image2.png");

                //Maillist.Add("kim.seoil@iset-da.com");
                Maillist.Add("haiyang.xu@qcells.com");
                //ccList.Add("kim.seoil@iset-da.com");

                Maillist.Add("huihui.xu@qcells.com");
                Maillist.Add("jin.su@qcells.com");
                Maillist.Add("qingqing.chen@qcells.com");
                Maillist.Add("haiyang.xiang@qcells.com");
                Maillist.Add("chuankou.li@qcells.com");
                Maillist.Add("huangsheng.hu@qcells.com");
                Maillist.Add("aihua.xia@qcells.com");
                Maillist.Add("jun.yin@qcells.com");
                Maillist.Add("xiangrong.zhang@qcells.com");
                Maillist.Add("yan.zhang3@qcells.com");
                Maillist.Add("weige.dang@qcells.com");

                ccList.Add("zhongjian.zhang@qcells.com");
                ccList.Add("kookhyoun.lim@qcells.com");
                ccList.Add("zhenguang.bao@qcells.com");

                mail.SendMail("EQUIPMENT LAMA RATIO REPORT - " + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), TAP.Base.Configuration.ConfigurationManager.Instance.SMTPSection.SMTPServerList["DEFAULT"].UserID, message, Maillist, Encoding.UTF8, true, fileNames, true, ccList);
                                    
                //Mail mail = new TAP.Base.Communication.Mail();
                //List<string> tmpmaillist = new List<string>();
                //tmpmaillist.Add("906353071@qq.com");
            }
        }
        public static String ConvertImageFileToBase64(String filename)
        {
            string base64Image = "";

            Image img = Image.FromFile(filename);

            MemoryStream mstream = new MemoryStream();

            img.Save(mstream, img.RawFormat);

            byte[] imgBytes = mstream.ToArray();

            base64Image = Convert.ToBase64String(imgBytes);

            mstream.Close();

            return base64Image;
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
