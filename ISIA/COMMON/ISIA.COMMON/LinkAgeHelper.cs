using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Data.Client;

namespace ISIA.COMMON
{
    public static class LinkAgeHelper
    {
        public static List<string> Linkage(string frmname ) {

            DataClient tmpDataClient = new DataClient();
            StringBuilder tmpSql = new StringBuilder();
            tmpSql.AppendFormat("SELECT * FROM TAPCTLINKAGE WHERE GROUPNAME = '{0}'", frmname);
            DataTable linktable = tmpDataClient.SelectData(tmpSql.ToString(), "LINK").Tables[0];
            List<string> listUI =  linktable.AsEnumerable().Select(row => row.Field<string>("UI")).ToList();
            return listUI;
        }


        public static DataTable LinkUI(string linkuiname)
        {

            DataClient tmpDataClient = new DataClient();
            StringBuilder tmpSql = new StringBuilder();
            tmpSql.AppendFormat("SELECT * FROM TAPSTBUI WHERE NAME = '{0}' AND ISALIVE = 'YES'", linkuiname);
            DataTable dtlinkui = tmpDataClient.SelectData(tmpSql.ToString(), "LINK").Tables[0];
            return dtlinkui;
            
        }
    }
}
