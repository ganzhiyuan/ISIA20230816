using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIA.UI.TREND.Dto
{
    public class SqlStatNew
    {
        public DateTime END_INTERVAL_TIME { get; set; }
        public string SNAP_ID { get; set; }
        public string SQL_ID { get; set; }
        public decimal typNum { get; set; }
        public decimal SQL_Count { get; set; }
    }
    public class SqlShow
    {
        public string SQL_ID { get; set; }
        public string PARAMENT_NAME { get; set; }
        public decimal PARAMENT_VALUE { get; set; }

        public DateTime END_INTERVAL_TIME { get; set; }

    }
    public class SqlShowCl
    {
        public string SQL_ID { get; set; }
        public decimal week1 { get; set; }
        public decimal week2 { get; set; }
        public decimal week3 { get; set; }
        public decimal week4 { get; set; }
        public decimal AVG { get; set; }
        public string SqlType { get; set; }

    }

    public class SqlStatRowDto
    {
        public decimal SNAP_ID { get; set; }
        public decimal DBID { get; set; }
        public decimal INSTANCE_NUMBER { get; set; }
        public string SQL_ID { get; set; }
        public decimal PLAN_HASH_VALUE { get; set; }
        public decimal OPTIMIZER_COST { get; set; }
        public string OPTIMIZER_MODE { get; set; }
        public decimal OPTIMIZER_ENV_HASH_VALUE { get; set; }
        public decimal SHARABLE_MEM { get; set; }
        public decimal LOADED_VERSIONS { get; set; }
        public decimal VERSION_COUNT { get; set; }
        public decimal PARSING_USER_ID { get; set; }
        public string PARAMENT_NAME { get; set; }
        public decimal PARAMENT_VALUE { get; set; }
        public DateTime END_INTERVAL_TIME { get; set; }
    }
    public class SqlstatDto
    {
        public DateTime END_INTERVAL_TIME { get; set; }

        public decimal SNAP_ID { get; set; }
        public decimal DBID { get; set; }
        public decimal INSTANCE_NUMBER { get; set; }
        public string SQL_ID { get; set; }
        public decimal PLAN_HASH_VALUE { get; set; }
        public decimal OPTIMIZER_COST { get; set; }
        public string OPTIMIZER_MODE { get; set; }
        public decimal OPTIMIZER_ENV_HASH_VALUE { get; set; }
        public decimal SHARABLE_MEM { get; set; }
        public decimal LOADED_VERSIONS { get; set; }
        public decimal VERSION_COUNT { get; set; }
        public decimal PARSING_USER_ID { get; set; }
        #region parament
        public decimal FETCHES_TOTAL { get; set; }
        public decimal END_OF_FETCH_COUNT_TOTAL { get; set; }
        public decimal SORTS_TOTAL { get; set; }
        public decimal EXECUTIONS_TOTAL { get; set; }
        public decimal PX_SERVERS_EXECS_TOTAL { get; set; }
        public decimal LOADS_TOTAL { get; set; }
        public decimal INVALIDATIONS_TOTAL { get; set; }
        public decimal PARSE_CALLS_TOTAL { get; set; }
        public decimal DISK_READS_TOTAL { get; set; }
        public decimal BUFFER_GETS_TOTAL { get; set; }
        public decimal ROWS_PROCESSED_TOTAL { get; set; }
        public decimal CPU_TIME_TOTAL { get; set; }
        public decimal ELAPSED_TIME_TOTAL { get; set; }
        public decimal IOWAIT_TOTAL { get; set; }
        public decimal CLWAIT_TOTAL { get; set; }
        public decimal APWAIT_TOTAL { get; set; }
        public decimal CCWAIT_TOTAL { get; set; }
        public decimal DIRECT_WRITES_TOTAL { get; set; }
        public decimal PLSEXEC_TIME_TOTAL { get; set; }
        public decimal JAVEXEC_TIME_TOTAL { get; set; }
        public decimal IO_OFFLOAD_ELIG_BYTES_TOTAL { get; set; }
        public decimal IO_INTERCONNECT_BYTES_TOTAL { get; set; }
        public decimal PHYSICAL_READ_REQUESTS_TOTAL { get; set; }
        public decimal PHYSICAL_READ_BYTES_TOTAL { get; set; }
        public decimal PHYSICAL_WRITE_REQUESTS_TOTAL { get; set; }
        public decimal PHYSICAL_WRITE_BYTES_TOTAL { get; set; }
        public decimal OPTIMIZED_PHYSICAL_READS_TOTAL { get; set; }
        public decimal CELL_UNCOMPRESSED_BYTES_TOTAL { get; set; }
        public decimal IO_OFFLOAD_RETURN_BYTES_TOTAL { get; set; }

        #endregion
    }
}
