using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Data.DataBase.Communicators;
using TAP.Models.SystemBasic;
using TAP.WinService;
using System.Data.SqlClient;
using System.Data.Common;
using static System.Net.Mime.MediaTypeNames;
using TAP.Models;
using System.Reflection;
using TAP.Base.Communication;

namespace ISIA.SERVICE
{
    class SvcUserInterface : TAP.WinService.ServiceBase
    {
        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            try
            {
                InsertDBData();
                UserSync();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void InsertDBData()
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                
                var sqlConnectionSB = new SqlConnectionStringBuilder();

                // Change these values to your values.  
                sqlConnectionSB.DataSource = "tcp:172.16.3.11,1433"; //["Server"]  
                sqlConnectionSB.InitialCatalog = "DEMO"; //["Database"]  

                sqlConnectionSB.UserID = "user_sync";  // "@yourservername"  as suffix sometimes.  
                sqlConnectionSB.Password = "solarone#2020";
                sqlConnectionSB.IntegratedSecurity = false;

                // Adjust these values if you like. (ADO.NET 4.5.1 or later.)  
                //sqlConnectionSB.ConnectRetryCount = 3;
                //sqlConnectionSB.ConnectRetryInterval = 10;  // Seconds.  

                // Leave these values as they are.  
                sqlConnectionSB.IntegratedSecurity = false;
                sqlConnectionSB.Encrypt = false;
                sqlConnectionSB.ConnectTimeout = 300;

                var sqlConnection = new SqlConnection(sqlConnectionSB.ToString());
                var dbCommand = sqlConnection.CreateCommand();
                List<string> sqllist = new List<string>();
                dbCommand.CommandText = @"SELECT * FROM [DEMO].[dbo].[EMPLOYEE_CELL_EQMT_V]";
                sqlConnection.Open();
                var dataReader = dbCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    StringBuilder tmpsb = new StringBuilder();

                    tmpsb.Append("Insert into EMPLOYEE_CELL_EQMT_V ");
                    tmpsb.Append("(EMP_NO, EMP_NM, DEPT_CD, DEPT_NM, MOBILE, ");
                    tmpsb.Append("BS_CD, RANK_BC, RANK_BC_NAME, DUTY_BC, DUTY_BC_NAME, ");
                    tmpsb.Append("EMAIL, QUFEN1_BC, QUFEN2_BC) ");
                    tmpsb.Append("Values(");
                    tmpsb.AppendFormat("'{0}', '{1}', '{2}', '{4}', '{3}',", 
                                        dataReader.GetValue(0).ToString(), 
                                        dataReader.GetValue(1).ToString(), 
                                        dataReader.GetValue(2).ToString(), 
                                        dataReader.GetValue(3).ToString(),
                                        dataReader.GetValue(4).ToString()
                                        );
                    tmpsb.AppendFormat("'{0}', '{1}', '{2}', '{3}', '{4}',",
                                        dataReader.GetValue(5).ToString(),
                                        dataReader.GetValue(6).ToString(),
                                        dataReader.GetValue(7).ToString(),
                                        dataReader.GetValue(8).ToString(),
                                        dataReader.GetValue(9).ToString()
                                        );
                    tmpsb.AppendFormat("'{0}', '{1}', '{2}')",
                                        dataReader.GetValue(10).ToString(),
                                        dataReader.GetValue(11).ToString(),
                                        dataReader.GetValue(12).ToString()
                                        );

                    sqllist.Add(tmpsb.ToString());
                }
                db.SaveWithoutTransaction(new List<string> { "TRUNCATE TABLE EMPLOYEE_CELL_EQMT_V" });
                db.SaveWithoutTransaction(sqllist);
            }
            catch(Exception ex)
            {

            }
        }


        private void UserSync()
        {
            try
            {
                UserSyncDelete();
                UserSyncUpdate();
                UserSyncInsert();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UserSyncDelete()
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();
#if MSSQL
                tmpsb.Append("SELECT NAME COLLATE Korean_Wansung_CI_AS AS USERID FROM TAPUTUSERS ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.Append("AND ISNUMERIC(NAME) = 1 ");
                tmpsb.Append("EXCEPT ");
                tmpsb.Append("SELECT EMP_NO AS USERID FROM OPENQUERY (USERDB, 'SELECT * FROM  DEMO.dbo.EMPLOYEE_CELL_EQMT_V') ");
#endif
#if ORACLE
                tmpsb.Append("SELECT NAME AS USERID FROM TAPUTUSERS ");
                tmpsb.Append("WHERE 1=1 ");
                tmpsb.Append("AND REGEXP_INSTR(NAME, '^[+-]?\\d*(\\.?\\d*)$') = 1 ");
                tmpsb.Append("MINUS ");
                tmpsb.Append("SELECT EMP_NO AS USERID FROM EMPLOYEE_CELL_EQMT_V ");
#endif
                DataTable tmpdt = db.Select(tmpsb.ToString()).Tables[0];

                if (tmpdt.Rows.Count > 0)
                {
                    List<string> tmpSaveList = new List<string>();

                    foreach (DataRow dr in tmpdt.Rows)
                    {
                        tmpsb.Clear();
                        tmpsb.Append("DELETE FROM TAPUTUSERS ");
                        tmpsb.Append("WHERE 1=1 ");
                        tmpsb.AppendFormat("AND NAME ='{0}'", dr["USERID"].ToString());
                        tmpSaveList.Add(tmpsb.ToString());
                    }

                    db.Save(tmpSaveList);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }


        private void UserSyncUpdate()
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpsb = new StringBuilder();

                tmpsb.Append("MERGE INTO TAPUTUSERS A ");
                tmpsb.Append("USING (SELECT * FROM EMPLOYEE_CELL_EQMT_V) B ");
                tmpsb.Append("ON ( A.NAME = B.EMP_NO ) ");
                tmpsb.Append("WHEN MATCHED THEN ");
                tmpsb.Append("UPDATE SET A.USERNAME = B.EMP_NM, A.DEPARTMENT = B.DEPT_NM, A.POSITION = B.RANK_BC_NAME, A.MOBILENO = B.MOBILE, A.MAILADDRESS = B.EMAIL, A.UPDATEUSER = 'USERSERVICE', A.UPDATETIME = TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS') ");
                tmpsb.Append(" ,A.CONTACTNO = (CASE WHEN trim(B.DEPT_NM) = '电池设备技术' AND trim(B.QUFEN1_BC) IN('设备1', '设备2', '设备3', '自动化设备') THEN 'EQENGR'  WHEN  trim(B.DEPT_NM) = '电池生产' THEN 'OPERATOR' WHEN  trim(B.DEPT_NM) = '电池工艺技术' AND trim(B.QUFEN1_BC) IN('工艺','NPI')   THEN 'PRCENGR'  WHEN trim(B.DEPT_NM) = '电池开发' THEN 'RDENGR' END ) ");
                tmpsb.Append("WHEN NOT MATCHED THEN ");
                tmpsb.Append("INSERT (NAME, REGION, FACILITY, DEPARTMENT, POSITION, USERNAME, MOBILENO, MAILADDRESS, PASSWORD, USERGROUPNAME, LANGUAGE,INSERTTIME, INSERTUSER, SEQUENCES, ISALIVE , CONTACTNO) ");
                tmpsb.Append("VALUES(B.EMP_NO, 'CELL', '5', trim(B.DEPT_NM), trim(B.RANK_BC_NAME), trim(B.EMP_NM), B.MOBILE, B.EMAIL, 'vpXarl/sJT1VmyyLm2bdQQ==', 'CELLUSERS', 'CN', TO_CHAR(SYSDATE, 'YYYYMMDDHH24MISS'), 'USERSERVICE', 0, 'YES' ");
                tmpsb.Append(" ,CASE WHEN trim(B.DEPT_NM) = '电池设备技术' AND trim(B.QUFEN1_BC) IN('设备1', '设备2', '设备3', '自动化设备') THEN 'EQENGR'  WHEN  trim(B.DEPT_NM) = '电池生产' THEN 'OPERATOR' WHEN  trim(B.DEPT_NM) = '电池工艺技术' AND trim(B.QUFEN1_BC) IN('工艺','NPI')   THEN 'PRCENGR'  WHEN trim(B.DEPT_NM) = '电池开发' THEN 'RDENGR'  END ");
                tmpsb.Append(" ) ");
                db.SaveWithoutTransaction(new List<string> { tmpsb.ToString() });

            }
            catch (Exception)
            {
                throw;
            }
        }
       
        private void UserSyncInsert()
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder tmpdel = new StringBuilder();
                StringBuilder tmpadd = new StringBuilder();

                tmpdel.Append(" TRUNCATE TABLE TAPFTBMUSER");

                tmpadd.Append(" INSERT INTO TAPFTBMUSER SELECT * FROM EMPLOYEE_CELL_EQMT_V");

                db.SaveWithoutTransaction(new List<string> { tmpdel.ToString(), tmpadd.ToString() });

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
