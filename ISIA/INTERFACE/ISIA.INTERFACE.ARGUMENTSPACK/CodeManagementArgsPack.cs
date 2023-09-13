using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class CodeManagementArgsPack
    {
        private ArgumentPack argsPack = new ArgumentPack();

        public string ROWID { get; set; }
        public string DBID { get; set; }
        public string DBNAME { get; set; }
        public string DBLINKNAME { get; set; }
        public string SERVICENAME { get; set; }
        public string IPADDRESS { get; set; }
        public decimal INSTANTCNT { get; set; }
        public string INSERTTIME { get; set; }
        public string UPDATETIME { get; set; }
        public string INSERTUSER { get; set; }
        public string UPDATEUSER { get; set; }
        public decimal SEQUENCES { get; set; }
        public string ISALIVE { get; set; }
        public string DESCRIPTION { get; set; }
        public string RETENTIONDAYS { get; set; }




        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(CodeManagementArgsPack), this);
            return argsPack;
        }
    }
}
