using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class SqlstatArgsPack
    {    
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(SvcDataSummaryArgsPack), this);
            return argsPack;
        }
        private ArgumentPack argsPack = new ArgumentPack();

        public string _SNAP_ID { get; set; }
        public string _BeginDT { get; set; }
        public string _EndDT { get; set; }
        public string _END_INTERVAL_TIME { get; set; }
        public string _DBID { get; set; }        
        public string _INSTANCE_NUMBER { get; set; }
        public string _SQL_ID { get; set; }
        public string _PLAN_HASH_VALUE { get; set; }
        public string _OPTIMIZER_COST { get; set; }
        public string _OPTIMIZER_MODE { get; set; }
        public string _OPTIMIZER_ENV_HASH_VALUE { get; set; }
        public string _SHARABLE_MEM { get; set; }
        public string _LOADED_VERSIONS { get; set; }
        public string _VERSION_COUNT { get; set; }
        public string _PARSING_USER_ID { get; set; }
        public string _PARANAME { get; set; }



    }
}
