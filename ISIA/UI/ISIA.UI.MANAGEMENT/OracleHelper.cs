using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace ISIA.UI.MANAGEMENT
{
    class OracleHelper
    {

        private string connectionString = string.Empty; // 数据库连接字符串

        public OracleHelper(string connStr)
        {
            connectionString = connStr;
        }

        // 查询方法，返回 DataTable 对象
        public DataTable ExecuteDataTable(string sql)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql, conn);
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
