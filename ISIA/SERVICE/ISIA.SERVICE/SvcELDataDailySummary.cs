using Npgsql;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Data.DataBase.Communicators;
using TAP.Models.SystemBasic;
using TAP.WinService;

namespace ISIA.SERVICE
{
    class SvcELDataDailySummary : TAP.WinService.ServiceBase
    {
        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            if (addressDt == null)
                addressDt = GetELAddress();
            InsertDBData();
        }
        private string GetLastTime(string Equipment)
        {
            string retval = "0";

            try
            {
                DBCommunicator db = new DBCommunicator();

                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT DECODE(MAX(CHECKTIMEKEY), null,null,MAX(CHECKTIMEKEY))  AS TIMEKEY FROM TAPEQP_EL_LOADFILE WHERE 1 = 1 ");
                sb.AppendFormat(" AND EQUIPMENT = '{0}'", Equipment);
                //sb.AppendFormat(" AND TESTTIMEKEY > to_char(sysdate-10,'yyyyMMddHH24miss')"); //CurrentDay -10 day

                DataTable dt = db.Select(sb.ToString()).Tables[0];

                if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0]["TIMEKEY"].ToString()))
                {
                    string temp = dt.Rows[0]["TIMEKEY"].ToString();
                    retval = temp;//temp.Substring(0, 4) + "-" + temp.Substring(4, 2) + "-" + temp.Substring(6, 2) + " " + temp.Substring(8, 2) + ":" + temp.Substring(10, 2) + ":" + temp.Substring(12, 2);
                }

                return retval;
            }
            catch (System.Exception)
            {
                return retval;
            }
        }
        private string NullForEmpty(string str)
        {
            string retval = "null";

            if (string.IsNullOrEmpty(str))
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
                string strConnect = string.Format("Host={0};Username=postgres;Password=MAXWELL;Database=ELDB", dr["CUSTOM02"].ToString());

                var conn = new NpgsqlConnection(strConnect);
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                string strSql = "";

                if(string.IsNullOrEmpty(timekey))
                {
                    string table = DateTime.Now.AddDays(-2).ToString("yyyy_MM");
                    string todate = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
                    strSql = string.Format("SELECT * FROM cell_{0} where checkdate > '{1}'", table, todate);
                }
                else
                {
                    string table = DateTime.Now.AddDays(-1).ToString("yyyy_MM");
                    strSql = string.Format("SELECT * FROM cell_{0} where replace(checkdate,'-','')||replace(checktime,':','') > '{1}'", table, timekey);
                }

                cmd = new NpgsqlCommand(strSql, conn);

                var reader = cmd.ExecuteReader();
                
                string workshop = dr["SUBCATEGORY"].ToString();
                string line = dr["CUSTOM03"].ToString();
                string address = dr["CUSTOM02"].ToString();
                string equipment = dr["CUSTOM01"].ToString();

                List<string> sqllist = new List<string>();
                                
                while (reader.Read())
                {
                    StringBuilder tmpsb1 = new StringBuilder();
                    if (string.IsNullOrEmpty(workshop) ||
                        string.IsNullOrEmpty(line) ||
                        string.IsNullOrEmpty(reader.GetString(2)) ||
                        string.IsNullOrEmpty(reader.GetString(3)) ||
                        string.IsNullOrEmpty(reader.GetString(5)))
                    {
                        continue;
                    }
                    string temptime = reader.GetString(2).Replace("-", "") + reader.GetString(3).Replace(":", "");

                    tmpsb1.Append("INSERT INTO  TAPEQP_EL_LOADFILE VALUES(");
                    tmpsb1.AppendFormat("'{0}',", workshop);
                    tmpsb1.AppendFormat("'{0}',", line);
                    tmpsb1.AppendFormat("'{0}',", "EL");
                    tmpsb1.AppendFormat("'{0}',", equipment);
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(2));
                    tmpsb1.AppendFormat("FN_GET_WORKSHIFT2('{0}'),", temptime);
                    tmpsb1.AppendFormat("'{0}',", temptime);
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(4));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(5));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(6));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(7) == "OK" ? "OK" : "NG");
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(7));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(9));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(8));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(10));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(11));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(12));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(13));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(14));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(15));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(16));
                    tmpsb1.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    sqllist.Add(tmpsb1.ToString());

                }
                reader.Close();
                conn.Close();

                int index = 0;
                int count = 0;
                //  count = dbb.Save(SqlList.GetRange(0, 1));

                while (true)
                {
                    if ((index + 1000) < sqllist.Count - 1)
                    {

                        count += db.Save(sqllist.GetRange(index, 1000));
                        index += 1000;
                    }
                    else
                    {
                        count += db.Save(sqllist.GetRange(index, sqllist.Count - index));
                        break;
                    }
                }

                InsertDataNextMonth(dr);

                Console.WriteLine(string.Format("Equipment : {0} completed.", dr["CUSTOM01"].ToString()));

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                SaveLog(ServiceBase.ERROR_LOG, "ELDataDaulySummary", ex.ToString());
            }
        }
        private void InsertDataNextMonth(DataRow dr)
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                var sqlConnectionSB = new SqlConnectionStringBuilder();

                string timekey = GetLastTime(dr["CUSTOM01"].ToString());

                // Change these values to your values.  
                string strConnect = string.Format("Host={0};Username=postgres;Password=MAXWELL;Database=ELDB", dr["CUSTOM02"].ToString());

                var conn = new NpgsqlConnection(strConnect);
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                string strSql = "";

                if (string.IsNullOrEmpty(timekey))
                {
                    string table = DateTime.Now.AddDays(-2).ToString("yyyy_MM");
                    string todate = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
                    strSql = string.Format("SELECT * FROM cell_{0} where checkdate > '{1}'", table, todate);
                }
                else
                {
                    string table = DateTime.Now.ToString("yyyy_MM");
                    strSql = string.Format("SELECT * FROM cell_{0} where replace(checkdate,'-','')||replace(checktime,':','') > '{1}'", table, timekey);
                }

                cmd = new NpgsqlCommand(strSql, conn);

                var reader = cmd.ExecuteReader();

                string workshop = dr["SUBCATEGORY"].ToString();
                string line = dr["CUSTOM03"].ToString();
                string address = dr["CUSTOM02"].ToString();
                string equipment = dr["CUSTOM01"].ToString();

                List<string> sqllist = new List<string>();

                while (reader.Read())
                {
                    StringBuilder tmpsb1 = new StringBuilder();
                    if (string.IsNullOrEmpty(workshop) ||
                        string.IsNullOrEmpty(line) ||
                        string.IsNullOrEmpty(reader.GetString(2)) ||
                        string.IsNullOrEmpty(reader.GetString(3)) ||
                        string.IsNullOrEmpty(reader.GetString(5)))
                    {
                        continue;
                    }
                    string temptime = reader.GetString(2).Replace("-", "") + reader.GetString(3).Replace(":", "");

                    tmpsb1.Append("INSERT INTO  TAPEQP_EL_LOADFILE VALUES(");
                    tmpsb1.AppendFormat("'{0}',", workshop);
                    tmpsb1.AppendFormat("'{0}',", line);
                    tmpsb1.AppendFormat("'{0}',", "EL");
                    tmpsb1.AppendFormat("'{0}',", equipment);
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(2));
                    tmpsb1.AppendFormat("FN_GET_WORKSHIFT2('{0}'),", temptime);
                    tmpsb1.AppendFormat("'{0}',", temptime);
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(4));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(5));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(6));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(7) == "OK" ? "OK" : "NG");
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(7));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(9));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(8));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(10));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(11));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(12));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(13));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(14));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(15));
                    tmpsb1.AppendFormat("'{0}',", reader.GetString(16));
                    tmpsb1.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    sqllist.Add(tmpsb1.ToString());

                }
                reader.Close();
                conn.Close();

                int index = 0;
                int count = 0;
                //  count = dbb.Save(SqlList.GetRange(0, 1));

                while (true)
                {
                    if ((index + 1000) < sqllist.Count - 1)
                    {

                        count += db.Save(sqllist.GetRange(index, 1000));
                        index += 1000;
                    }
                    else
                    {
                        count += db.Save(sqllist.GetRange(index, sqllist.Count - index));
                        break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                SaveLog(ServiceBase.ERROR_LOG, "ELDataDaulySummary", ex.ToString());
            }
        }
        private void InsertDBData()
        {
            try
            {               
                foreach (DataRow dr in addressDt.Rows)
                {
                    //InsertData(dr);

                    Task.Run(() => InsertData(dr));
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// key ：fileName， value ：address 每小时添加一次数据时用到
        /// </summary>
        ConcurrentDictionary<string, string> dics = new ConcurrentDictionary<string, string>();
        DBCommunicator db = new DBCommunicator();
        DataTable addressDt = null;
        //    private void Conn(DataTable addressDt)
        //    {
        //        string strConnect = string.Format("Host=172.16.54.189;Username=postgres;Password=MAXWELL;Database=ELDB");

        //        var conn = new NpgsqlConnection(strConnect);

        //        conn.Open();

        //        NpgsqlCommand cmd = new NpgsqlCommand();
        //        string strSql = "";

        //        string table = DateTime.Now.AddDays(-2).ToString("yyyy_MM");
        //        string todate = DateTime.Now.AddDays(-8).ToString("yyyy-MM-dd");
        //        strSql = string.Format("SELECT * FROM cell_{0} where checkdate > '{1}'", table, todate);


        //        cmd = new NpgsqlCommand(strSql, conn);

        //        var reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            Console.WriteLine(reader.GetString(0));
        //        }

        //        DateTime time = DateTime.Now;
        //        DateTime Lastday = DateTime.Now.AddDays(-1);
        //        for (int i = 0; i < addressDt.Rows.Count; i++)
        //        {
        //            string workshop = addressDt.Rows[i]["SUBCATEGORY"].ToString();
        //            string line = addressDt.Rows[i]["CUSTOM01"].ToString();
        //            string address = addressDt.Rows[i]["CUSTOM02"].ToString();
        //            /** 每小时导入一次，暂时不用，现在是按班次导入（每12小时导入一次）
        //            if (time.Hour == 7)
        //            {
        //                string fileName = Lastday.Year + "-" + Lastday.Month.ToString().PadLeft(2, '0') + "-" + Lastday.Day.ToString().PadLeft(2, '0') + "--D" + "_EL.accdb";
        //                string rowid = "";
        //                if (dics.ContainsKey(fileName))
        //                {
        //                    rowid = dics[fileName];
        //                }
        //                string count = GetAccessData(address, fileName, workshop, line, "N", rowid);
        //                dics.AddOrUpdate(fileName, count, (key, value) => count);
        //                fileName = time.Year + "-" + time.Month.ToString().PadLeft(2, '0') + "-" + time.Day.ToString().PadLeft(2, '0') + "--A" + "_EL.accdb";
        //                if (dics.ContainsKey(fileName))
        //                {
        //                    rowid = dics[fileName];
        //                }
        //                count = GetAccessData(address, fileName, workshop, line, "D", rowid);
        //                dics.AddOrUpdate(fileName, count, (key, value) => count);

        //            }
        //            else if (time.Hour == 19)
        //            {
        //                string fileName = time.Year + "-" + time.Month.ToString().PadLeft(2, '0') + "-" + time.Day.ToString().PadLeft(2, '0') + "--A" + "_EL.accdb";
        //                string rowid = "";
        //                if (dics.ContainsKey(fileName))
        //                {
        //                    rowid = dics[fileName];
        //                }
        //                string count = GetAccessData(address, fileName, workshop, line, "D", rowid);
        //                dics.AddOrUpdate(fileName, count, (key, value) => count);

        //                fileName = time.Year + "-" + time.Month.ToString().PadLeft(2, '0') + "-" + time.Day.ToString().PadLeft(2, '0') + "--D" + "_EL.accdb";
        //                if (dics.ContainsKey(fileName))
        //                {
        //                    rowid = dics[fileName];
        //                }
        //                count = GetAccessData(address, fileName, workshop, line, "N", rowid);
        //                dics.AddOrUpdate(fileName, count, (key, value) => count);

        //            }
        //            else if (time.Hour > 19 && time.Hour < 24)
        //            {
        //                string fileName = time.Year + "-" + time.Month.ToString().PadLeft(2, '0') + "-" + time.Day.ToString().PadLeft(2, '0') + "--D" + "_EL.accdb";
        //                string rowid = "";
        //                if (dics.ContainsKey(fileName))
        //                {
        //                    rowid = dics[fileName];
        //                }
        //                string count = GetAccessData(address, fileName, workshop, line, "N", rowid);
        //                dics.AddOrUpdate(fileName, count, (key, value) => count);

        //            }
        //            else if (time.Hour < 7)
        //            {
        //                string fileName = Lastday.Year + "-" + Lastday.Month.ToString().PadLeft(2, '0') + "-" + Lastday.Day.ToString().PadLeft(2, '0') + "--D" + "_EL.accdb";
        //                string rowid = "";
        //                if (dics.ContainsKey(fileName))
        //                {
        //                    rowid = dics[fileName];
        //                }
        //                string count = GetAccessData(address, fileName, workshop, line, "N", rowid);
        //                dics.AddOrUpdate(fileName, count, (key, value) => count);

        //            }
        //            else if (time.Hour > 7 && time.Hour < 19)
        //            {
        //                string fileName = time.Year + "-" + time.Month.ToString().PadLeft(2, '0') + "-" + time.Day.ToString().PadLeft(2, '0') + "--A" + "_EL.accdb";
        //                string rowid = "";
        //                if (dics.ContainsKey(fileName))
        //                {
        //                    rowid = dics[fileName];
        //                }
        //                string count = GetAccessData(address, fileName, workshop, line, "D", rowid);
        //                dics.AddOrUpdate(fileName, count, (key, value) => count);

        //            }
        //*/
        //            if (time.Hour == 8)
        //            {
        //                string fileName = Lastday.Year + "-" + Lastday.Month.ToString().PadLeft(2, '0') + "-" + Lastday.Day.ToString().PadLeft(2, '0') + "--N" + "_EL.accdb";
        //                string count = GetAccessData(address, fileName, workshop, line, "N", "");
        //            }
        //            else if (time.Hour == 20)
        //            {
        //                string fileName = time.Year + "-" + time.Month.ToString().PadLeft(2, '0') + "-" + time.Day.ToString().PadLeft(2, '0') + "--D" + "_EL.accdb";
        //                string count = GetAccessData(address, fileName, workshop, line, "D", "");
        //            }
        //        }
        //    }
        //    public string GetAccessData(string address, string fileName, string workshop, string line, string shift, string rowid)
        //    {

        //        OleDbConnectionStringBuilder oleString = new OleDbConnectionStringBuilder();
        //        //为了使大家更清楚使用这个类，制造一个连接字符串 
        //        oleString.Provider = "Microsoft.ACE.OleDB.16.0";
        //        //使用刚刚安装的数据库引擎，大家不要写错了
        //        string filePath = @"\\" + address + @"\ELspyer_WPF\ELspyer\ELspyer\bin\DataBase\" + fileName;
        //        oleString.DataSource = filePath;
        //        // oleString.DataSource = @address;
        //        //这里写你数据库连接的位置 
        //        ////创建OleDb连接对象
        //        OleDbConnection conn = new OleDbConnection();
        //        conn.ConnectionString = oleString.ToString();
        //        ////将生成的字符串传入
        //        try
        //        {
        //            //判断有没有该文件
        //            if (File.Exists(filePath))
        //                conn.Open();
        //            OleDbCommand cmd = conn.CreateCommand();
        //            //if (string.IsNullOrWhiteSpace(rowid))
        //            //{
        //            cmd.CommandText = "select * from cell ";
        //            //}
        //            //else
        //            //{
        //            //    cmd.CommandText = string.Format("select * from cell where id >{0}", rowid);
        //            //}
        //            OleDbDataReader dr = cmd.ExecuteReader();
        //            DataTable dt = new DataTable();
        //            if (dr.HasRows)
        //            {
        //                for (int j = 0; j < dr.FieldCount; j++)
        //                {
        //                    dt.Columns.Add(dr.GetName(j));
        //                }
        //                dt.Rows.Clear();
        //            }


        //            while (dr.Read())
        //            {
        //                DataRow row = dt.NewRow();
        //                for (int j = 0; j < dr.FieldCount; j++)
        //                {
        //                    row[j] = dr[j];
        //                }
        //                dt.Rows.Add(row);
        //            }
        //            cmd.Dispose();
        //            conn.Close();
        //            return AddDate(dt, workshop, line, shift).ToString();
        //        }
        //        catch (Exception ex)
        //        {                
        //            this.SaveLog(ServiceBase.ERROR_LOG, ServiceName, string.Format("Address:{0} File:{1} Error:{2}", address, fileName, ex.ToString()));
        //            return "0";
        //        }
        //    }
        //    public int AddDate(DataTable dt, string workshop, string line, string shift)
        //    {

        //        try
        //        {
        //            DBCommunicator dbb = new DBCommunicator();
        //            List<string> SqlList = new List<string>();
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                StringBuilder tmpsb1 = new StringBuilder();
        //                if (string.IsNullOrEmpty(workshop) ||
        //                    string.IsNullOrEmpty(line) ||
        //                    string.IsNullOrEmpty(dr["CheckDate"].ToString()) ||
        //                    string.IsNullOrEmpty(dr["CHECKTIME"].ToString()) ||
        //                    string.IsNullOrEmpty(shift) ||
        //                    string.IsNullOrEmpty(dr["WAFERID"].ToString()) ||
        //                    string.IsNullOrEmpty("CHECKTIMEKEY"))
        //                {
        //                    continue;
        //                }
        //                tmpsb1.Append("INSERT INTO  TAPEQP_EL_LOADFILE VALUES(");
        //                tmpsb1.AppendFormat("'{0}',", workshop);
        //                tmpsb1.AppendFormat("'{0}',", line);
        //                tmpsb1.AppendFormat("'{0}',", "EL");
        //                tmpsb1.AppendFormat("FN_GET_EQPID_IN_TAPIFTB_SPEQP('{0}','{1}'),",workshop,line);
        //                tmpsb1.AppendFormat("'{0}',", dr["CheckDate"].ToString());
        //                tmpsb1.AppendFormat("'{0}',", shift);
        //                tmpsb1.AppendFormat("'{0}',", dr["CheckDate"].ToString().Replace("-", "") + dr["CHECKTIME"].ToString().Replace(":", ""));
        //                tmpsb1.AppendFormat("'{0}',", dr["MATERIAL"].ToString());
        //                tmpsb1.AppendFormat("'{0}',", dr["WAFERID"].ToString());
        //                tmpsb1.AppendFormat("'{0}',", dr["RANK"].ToString());
        //                tmpsb1.AppendFormat("'{0}',", dr["NG"].ToString() == "OK" ? "OK" : "NG");
        //                tmpsb1.AppendFormat("'{0}',", dr["NG"].ToString());
        //                tmpsb1.AppendFormat("'{0}',", dr["ENNGCATALOG"].ToString());
        //                tmpsb1.AppendFormat("'{0}',", dr["CNNGCATALOG"].ToString());
        //                tmpsb1.AppendFormat("'{0}',", dr["PLCBIN"].ToString());
        //                tmpsb1.AppendFormat("'{0}',", dr["ELBIN"].ToString());
        //                tmpsb1.AppendFormat("'{0}',", dr["BINETA"].ToString());
        //                tmpsb1.AppendFormat("'{0}',", dr["BINCOLOR"].ToString());
        //                tmpsb1.AppendFormat("'{0}',", dr["AVGGRAY"].ToString());
        //                tmpsb1.AppendFormat("'{0}',", dr["COMMENT"].ToString());
        //                tmpsb1.AppendFormat("'{0}',", dr["IMAGEPATH"].ToString());
        //                tmpsb1.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
        //                SqlList.Add(tmpsb1.ToString());
        //            }
        //            //数据太多无法使用，需要修改配置timeout
        //            //int count = db.Save(SqlList);
        //            int index = 0;
        //            int count = 0;
        //            //  count = dbb.Save(SqlList.GetRange(0, 1));

        //            while (true)
        //            {
        //                if ((index + 500) < SqlList.Count - 1)
        //                {

        //                    count += dbb.Save(SqlList.GetRange(index, 500));
        //                    index += 500;
        //                }
        //                else
        //                {
        //                    count += dbb.Save(SqlList.GetRange(index, SqlList.Count - index));
        //                    break;
        //                }
        //            }
        //            return count;
        //        }
        //        catch (Exception)
        //        {
        //            return 0;
        //        }
        //    }
        public DataTable GetELAddress()
        {
            try
            {
                StringBuilder tmpsb = new StringBuilder();
                tmpsb.Append("SELECT * FROM TAPCTCODES WHERE CATEGORY='ADDRESS' ");
                tmpsb.Append("AND  USED='YES' AND ISALIVE='YES' AND NAME='EL' ");
                DataTable tmpdt = db.Select(tmpsb.ToString()).Tables[0];
                return tmpdt;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
