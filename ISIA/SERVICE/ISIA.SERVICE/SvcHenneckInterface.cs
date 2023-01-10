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

namespace ISIA.SERVICE
{
    class SvcHenneckInterface : TAP.WinService.ServiceBase
    {
        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            GetCSVData();
        }
        private DataTable GetAddress()
        {

            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpsb = new StringBuilder();
            tmpsb.Append("SELECT CATEGORY,SUBCATEGORY,NAME,CUSTOM01,CUSTOM02,CUSTOM03 FROM TAPCTCODES ");
            tmpsb.Append(" WHERE CATEGORY='HENNECK' AND ISALIVE='YES' ");
            DataTable dtAddress = db.Select(tmpsb.ToString()).Tables[0];
            return dtAddress;

        }
        private void DataInsert(List<string> sqllist)
        {
            try
            {
                //数据太多无法使用，需要修改配置timeout
                //int count = db.Save(SqlList);
                int index = 0;
                int count = 0;
                //  count = dbb.Save(SqlList.GetRange(0, 1));
                DBCommunicator db = new DBCommunicator();
                int cnt = 0;

                while (true)
                {
                    try
                    {
                        if ((index + 500) < sqllist.Count - 1)
                        {

                            count += db.Save(sqllist.GetRange(index, 500));
                            index += 500;
                        }
                        else
                        {
                            count += db.Save(sqllist.GetRange(index, sqllist.Count - index));
                            break;
                        }
                    }
                    catch(System.Exception ex)
                    {
                        cnt++;

                        if(cnt > 20)
                        {
                            throw;
                        }
                        ex.ToString();
                        continue;
                    }
                }
            }
            catch(System.Exception ex)
            {

            }
        }
        private void GetCSVData()
        {
            try
            {                
                DateTime time = DateTime.Now;
                DataTable dtAddress = GetAddress();
                string file = "";
                string shift = "";
                for (int i = 0; i < dtAddress.Rows.Count; i++)
                {
                    if (time.Hour == 8)
                    {
                        //Result_5110120220329D_PMP.txt
                        string date = time.AddDays(-1).ToString("yyyyMMdd");
                        shift = "N";
                        file = dtAddress.Rows[i]["CUSTOM02"].ToString() + @"\Result_51101"+date + shift + "_PMP.txt";
                    }
                    else if (time.Hour == 20)
                    {
                        string date = time.ToString("yyyyMMdd");
                        shift = "D";
                        file = dtAddress.Rows[i]["CUSTOM02"].ToString() + @"\Result_51101"+ date + shift + "_PMP.txt";
                    }

                    //file = @"C:\Users\Kim.Seoil\Desktop\Result_5110120220328N_PMP.txt";

                    try
                    {                
                        //文件流读取
                        System.IO.FileStream fs = new FileStream(@file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        System.IO.StreamReader sr = new System.IO.StreamReader(fs, Encoding.GetEncoding("gb2312"));

                        string NextLine = "";
                        string[] arrs = sr.ReadLine().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        List<string> sqllist = new List<string>();
                        
                        
                        while ((NextLine = sr.ReadLine()) != null)
                        {
                            string[] arr = NextLine.Split(new char[] { ';' });

                            DateTime tempdt;
                            tempdt = DateTime.ParseExact(arr[0], "dd.MM.yyyy HH:mm:ss.fff",System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None);
                            
                            StringBuilder sb = new StringBuilder();
                            sb.Append("INSERT INTO TAPEQP_HENNECK_DATA(");
                            sb.Append("TIMEKEY, INPUTWAFERID, WAFERID,");
                            sb.Append("JOBCODE, INGOT_NR, RECIPE,");
                            sb.Append("Q, QNAME, Q_THICKN,");
                            sb.Append("Q_TTV, Q_SM, Q_WAV,");
                            sb.Append("Q_CHIPS, Q_BREAKAGES, Q_HOLES,");
                            sb.Append("Q_NVCD, Q_INCLUSION, Q_TRANSFL_CRACKS,");
                            sb.Append("Q_BOW, Q_SIZE, Q_MM_SIZE,");
                            sb.Append("Q_D_SIZE, Q_RECT, Q_CH_LEN,");
                            sb.Append("Q_CH_KAT, Q_UNP, Q_RESIST,");
                            sb.Append("Q_PN, Q_STAIN, THICKN,");
                            sb.Append("TTV, RESISTIVITY, PN,");
                            sb.Append("BOW, CHIPS, BREAKAGES,");
                            sb.Append("HOLES, NVCD, INCLUSIONS,");
                            sb.Append("TRANSFL_CRACKS, STAINTOP, STAINBOTTOM,");
                            sb.Append("MAX_SIZE, MIN_SIZE, SIZE_X_M,");
                            sb.Append("SIZE_Y_M, DIAGONAL_L_R, DIAGONAL_R_L,");
                            sb.Append("RECT_MAX, MAX_CHAMFER, MIN_CHAMFER,");
                            sb.Append("UNPARA_H, UNPARA_V, MAXCHAMFERCATHETUS,");
                            sb.Append("MINCHAMFERCATHETUS, BRK_DEPTH, BRK_WIDTH,");
                            sb.Append("CHIP_DEPTH, CHIP_WIDTH, RES_MIN,");
                            sb.Append("RES_MAX, SM_MAX, SM_TL,");
                            sb.Append("SM_TM, SM_TR, SM_BL,");
                            sb.Append("SM_BM, SM_BR, W_MAX,");
                            sb.Append("W_TL, W_TM, W_TR,");
                            sb.Append("W_BL, W_BM, W_BR , INSERTTIME, INSERTUSER, SHIFT, WORKSHOP)");
                            sb.Append("VALUES( ");
                            sb.AppendFormat("'{0}','{1}','{2}'", tempdt.ToString("yyyyMMddHHmmssfff"), arr[1], arr[2]); /* TIMEKEY */ /* INPUTWAFERID */ /* WAFERID */
                            sb.AppendFormat(",'{0}','{1}','{2}'", arr[3], arr[4], arr[5]); /* JOBCODE */ /* INGOT_NR */ /* RECIPE */ /* Q */
                            sb.AppendFormat(",{0},'{1}',{2}", arr[6], arr[7], arr[8]); /* QNAME */ /* Q_THICKN */ /* Q_TTV */
                            sb.AppendFormat(",{0},{1},{2}", arr[9], arr[10], arr[11]); /* Q_SM */ /* Q_WAV */ /* Q_CHIPS */
                            sb.AppendFormat(",{0},{1},{2}", arr[12], arr[13], arr[14]); /* Q_BREAKAGES */ /* Q_HOLES */ /* Q_NVCD */
                            sb.AppendFormat(",{0},{1},{2}", arr[15], arr[16], arr[17]); /* Q_INCLUSION */ /* Q_TRANSFL_CRACKS */ /* Q_BOW */
                            sb.AppendFormat(",{0},{1},{2}", arr[18], arr[19], arr[20]); /* Q_SIZE */ /* Q_MM_SIZE */ /* Q_D_SIZE */
                            sb.AppendFormat(",{0},{1},{2}", arr[21], arr[22], arr[23]); /* Q_RECT */ /* Q_CH_LEN */ /* Q_CH_KAT */
                            sb.AppendFormat(",{0},{1},{2}", arr[24], arr[25], arr[26]); /* Q_UNP */ /* Q_RESIST */ /* Q_PN */
                            sb.AppendFormat(",{0},{1},{2}", arr[27], arr[28], arr[29]); /* Q_STAIN */ /* THICKN */ /* TTV */
                            sb.AppendFormat(",{0},{1},{2}", arr[30], arr[31], arr[32]); /* RESISTIVITY */ /* PN */ /* BOW */
                            sb.AppendFormat(",{0},{1},{2}", arr[33], arr[34], arr[35]); /* CHIPS */ /* BREAKAGES */ /* HOLES */
                            sb.AppendFormat(",{0},{1},{2}", arr[36], arr[37], arr[38]); /* NVCD */ /* INCLUSIONS */ /* TRANSFL_CRACKS */
                            sb.AppendFormat(",{0},{1},{2}", arr[39], arr[40], arr[41]); /* STAINTOP */ /* STAINBOTTOM */ /* MAX_SIZE */
                            sb.AppendFormat(",{0},{1},{2}", arr[42], arr[43], arr[44]); /* MIN_SIZE */ /* SIZE_X_M */ /* SIZE_Y_M */
                            sb.AppendFormat(",{0},{1},{2}", arr[45], arr[46], arr[47]); /* DIAGONAL_L_R */ /* DIAGONAL_R_L */ /* RECT_MAX */
                            sb.AppendFormat(",{0},{1},{2}", arr[48], arr[49], arr[50]); /* MAX_CHAMFER */ /* MIN_CHAMFER */ /* UNPARA_H */
                            sb.AppendFormat(",{0},{1},{2}", arr[51], arr[52], arr[53]); /* UNPARA_V */ /* MAXCHAMFERCATHETUS */ /* MINCHAMFERCATHETUS */
                            sb.AppendFormat(",{0},{1},{2}", arr[54], arr[55], arr[56]); /* BRK_DEPTH */ /* BRK_WIDTH */ /* CHIP_DEPTH */
                            sb.AppendFormat(",{0},{1},{2}", arr[57], arr[58], arr[59]); /* CHIP_WIDTH */ /* RES_MIN */ /* RES_MAX */
                            sb.AppendFormat(",{0},{1},{2}", arr[60], arr[61], arr[62]); /* SM_MAX */ /* SM_TL */ /* SM_TM */
                            sb.AppendFormat(",{0},{1},{2}", arr[63], arr[64], arr[65]); /* SM_TR */ /* SM_BL */ /* SM_BM */
                            sb.AppendFormat(",{0},{1},{2}", arr[66], arr[67], arr[68]); /* SM_BR */ /* W_MAX */ /* W_TL */
                            sb.AppendFormat(",{0},{1},{2}", arr[69], arr[70], arr[71]); /* W_TM */ /* W_TR */ /* W_BL */
                            sb.AppendFormat(",{0},{1},{2}, to_char(sysdate,'yyyyMMddHH24miss'), 'Service', FN_GET_WORKSHIFT2('{3}'), SUBSTR('{4}',0,3))", arr[72], arr[73], arr[74] , tempdt.ToString("yyyyMMddHHmmssfff"), arr[3]);/* W_BM */ /* W_BR */
                            sqllist.Add(sb.ToString());

                        }
                        //db.Save(sqllist);
                        //关闭流
                        sr.Close(); fs.Close();

                        DataInsert(sqllist);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                        continue;
                    }
                }                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}