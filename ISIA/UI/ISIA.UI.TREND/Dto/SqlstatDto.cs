using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIA.UI.TREND.Dto
{
    public class SqlstatDto
    {
        public string SNAP_ID { get; set; }
        public string DBID { get; set; }
        public string INSTANCE_NUMBER { get; set; }
        public string SQL_ID { get; set; }
        public string PLAN_HASH_VALUE { get; set; }
        public string OPTIMIZER_COST { get; set; }
        public string OPTIMIZER_MODE { get; set; }
        public string OPTIMIZER_ENV_HASH_VALUE { get; set; }
        public string SHARABLE_MEM { get; set; }
        public string LOADED_VERSIONS { get; set; }
        public string VERSION_COUNT { get; set; }
        public string PARSING_USER_ID { get; set; }
        #region parament
        public string FETCHES_TOTAL { get; set; }
        public string END_OF_FETCH_COUNT_TOTAL { get; set; }
        public string SORTS_TOTAL { get; set; }
        public string EXECUTIONS_TOTAL { get; set; }
        public string PX_SERVERS_EXECS_TOTAL { get; set; }
        public string LOADS_TOTAL { get; set; }
        public string INVALIDATIONS_TOTAL { get; set; }
        public string PARSE_CALLS_TOTAL { get; set; }
        public string DISK_READS_TOTAL { get; set; }
        public string BUFFER_GETS_TOTAL { get; set; }
        public string ROWS_PROCESSED_TOTAL { get; set; }
        public string CPU_TIME_TOTAL { get; set; }
        public string ELAPSED_TIME_TOTAL { get; set; }
        public string IOWAIT_TOTAL { get; set; }
        public string CLWAIT_TOTAL { get; set; }
        public string APWAIT_TOTAL { get; set; }
        public string CCWAIT_TOTAL { get; set; }
        public string DIRECT_WRITES_TOTAL { get; set; }
        public string PLSEXEC_TIME_TOTAL { get; set; }
        public string JAVEXEC_TIME_TOTAL { get; set; }
        public string IO_OFFLOAD_ELIG_BYTES_TOTAL { get; set; }
        public string IO_INTERCONNECT_BYTES_TOTAL { get; set; }
        public string PHYSICAL_READ_REQUESTS_TOTAL { get; set; }
        public string PHYSICAL_READ_BYTES_TOTAL { get; set; }
        public string PHYSICAL_WRITE_REQUESTS_TOTAL { get; set; }
        public string PHYSICAL_WRITE_BYTES_TOTAL { get; set; }

        #endregion
    }
}
