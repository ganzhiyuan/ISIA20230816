using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TAP;
using TAP.Models;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class AwrArgsPack
    {
        private ArgumentPack argsPack = new ArgumentPack();


        private string _ParamType;

        private List<object> _ParamNamesList;

        private string _ParamNamesString;


        private string _StartTime;

        private string _EndTime;

        private string _GroupingDateFormat;

        private int _ClustersNumber;

        private string _DBName;

        private List<object> _SqlIdList;

        private string _WorkloadSqlParm;

        private static List<string> _WorkloadParamNamesList = new List<string>();

        private static Dictionary<string, string> _WorkloadSqlRelationMapping = new Dictionary<string, string>();

        private static Dictionary<string, string> _WorkloadRealParmMapping = new Dictionary<string, string>();


        private static Dictionary<string, string> _WorkloadBelonging = new Dictionary<string, string>();

        private static List<string> _SqlParmsList = new List<string>();

        public const string METRIC = "METRIC";

        public const string SYSSTAT = "SYSSTAT";

        public const string GET_ALL = "ALL";

        public string DBID { get; set; }
        public string INSTANCE_NUMBER { get; set; }

        static AwrArgsPack()
        {
            //_WorkloadParamNamesList
            WorkloadParamNamesList.Add("CPU_Util_pct");
            WorkloadParamNamesList.Add("CPU_Util_pct_max");
            WorkloadParamNamesList.Add("LOGICAL_READS_PSEC");
            WorkloadParamNamesList.Add("PHYSICAL_READS_PSEC");
            WorkloadParamNamesList.Add("Physical_Writes_psec");
            WorkloadParamNamesList.Add("Execs_psec_avg");
            WorkloadParamNamesList.Add("Execs_psec_max");
            WorkloadParamNamesList.Add("USER_CALLS_PSEC");
            WorkloadParamNamesList.Add("Hard_Parse_Cnt_psec");
            WorkloadParamNamesList.Add("DB_BLOCK_CHANGES_PSEC");
            WorkloadParamNamesList.Add("SQL_Service_Response_Time");
            WorkloadParamNamesList.Add("Commit_psec_avg");
            WorkloadParamNamesList.Add("Redo_mb_psec_avg");

            WorkloadParamNamesList.Add("DLM_MB_psec");
            WorkloadParamNamesList.Add("NET_MB_To_Client_psec");
            WorkloadParamNamesList.Add("NET_MB_From_Client_psec");
            WorkloadParamNamesList.Add("NET_MB_From_DBLink_psec");
            WorkloadParamNamesList.Add("NET_MB_To_DBLink_psec");
            WorkloadParamNamesList.Add("EXECUTIONS");
            WorkloadParamNamesList.Add("ELAPSED_TIME");
            WorkloadParamNamesList.Add("CPU_TIME");
            WorkloadParamNamesList.Add("BUFFER_GETS");
            WorkloadParamNamesList.Add("DISK_READS");
            WorkloadParamNamesList.Add("PARSE_CALL");
            //_WorkloadSqlRelationMapping
            WorkloadSqlRelationMapping.Add("CPU Usage Per Sec", "CPU_TIME_delta");
            WorkloadSqlRelationMapping.Add("DB time", "ELAPSED_TIME_delta");
            WorkloadSqlRelationMapping.Add("physical reads", "PHYSICAL_READ_BYTES_delta");
            WorkloadSqlRelationMapping.Add("parse count (total)", "PARSE_CALLS_delta");
            WorkloadSqlRelationMapping.Add("redo size", "ROWS_PROCESSED_delta");
            WorkloadSqlRelationMapping.Add("db block changes", "ROWS_PROCESSED_delta");
            //_WorkloadBelonging
            WorkloadBelonging.Add("CPU Usage Per Sec", METRIC);
            WorkloadBelonging.Add("DB time", SYSSTAT);
            WorkloadBelonging.Add("physical reads", SYSSTAT);
            WorkloadBelonging.Add("parse count (total)", SYSSTAT);
            WorkloadBelonging.Add("redo size", SYSSTAT);
            WorkloadBelonging.Add("db block changes", SYSSTAT);

            WorkloadBelonging.Add("Host CPU Utilization (%)", METRIC);
            WorkloadBelonging.Add("Logical Reads Per Sec", METRIC);
            WorkloadBelonging.Add("Physical Reads Per Sec", METRIC);
            WorkloadBelonging.Add("Physical Writes Per Sec", METRIC);
            WorkloadBelonging.Add("Executions Per Sec", METRIC);
            WorkloadBelonging.Add("Execs_psec_max", METRIC);
            WorkloadBelonging.Add("User Calls Per Sec", METRIC);
            WorkloadBelonging.Add("DB Block Changes Per Sec", METRIC);
            WorkloadBelonging.Add("SQL Service Response Time", METRIC);
            WorkloadBelonging.Add("Hard Parse Count Per Sec", METRIC);
            WorkloadBelonging.Add("User Commits Per Sec", METRIC);
            WorkloadBelonging.Add("Redo Generated Per Sec", METRIC);
            //WorkloadBelonging.Add("NET_Cnt_DBLink", SYSSTAT);
            //WorkloadBelonging.Add("NET_Cnt_Client", SYSSTAT);
            //WorkloadBelonging.Add("NET_B_To_Client", SYSSTAT);
            //WorkloadBelonging.Add("NET_B_From_Client", SYSSTAT);
            //WorkloadBelonging.Add("NET_B_From_DBLink", SYSSTAT);
            //WorkloadBelonging.Add("NET_B_To_DBLink", SYSSTAT);
            //WorkloadBelonging.Add("gc_recv", SYSSTAT);
            //WorkloadBelonging.Add("gc_send", SYSSTAT);
            //WorkloadBelonging.Add("gcs_msg_send", SYSSTAT);

            //WorkloadRealParmMapping
            WorkloadRealParmMapping.Add("CPU_Util_pct", "Host CPU Utilization (%)");
            WorkloadRealParmMapping.Add("CPU_Util_pct_max", "Host CPU Utilization (%)");
            WorkloadRealParmMapping.Add("LOGICAL_READS_PSEC", "Logical Reads Per Sec");
            WorkloadRealParmMapping.Add("Physical_Reads_psec", "Physical Reads Per Sec");
            WorkloadRealParmMapping.Add("Physical_Writes_psec", "Physical Writes Per Sec");
            WorkloadRealParmMapping.Add("Execs_psec_avg", "Executions Per Sec");
            WorkloadRealParmMapping.Add("Execs_psec_max", "Executions Per Sec");
            WorkloadRealParmMapping.Add("User_Calls_psec", "User Calls Per Sec");
            WorkloadRealParmMapping.Add("DB_Block_Changes_psec", "DB Block Changes Per Sec");
            WorkloadRealParmMapping.Add("SQL_Service_Response_Time", "SQL Service Response Time");
            WorkloadRealParmMapping.Add("User_Commits_psec", "User Commits Per Sec");
            WorkloadRealParmMapping.Add("Redo_Generated_psec", "Redo Generated Per Sec");
            WorkloadRealParmMapping.Add("Hard_Parse_Cnt_psec", "Hard Parse Count Per Sec");
            //WorkloadRealParmMapping.Add("NET_Cnt_Client", "NET_Cnt_Client");
            //WorkloadRealParmMapping.Add("DLM_MB_psec", "");
            //WorkloadRealParmMapping.Add("NET_MB_To_Client_psec", "");
            //WorkloadRealParmMapping.Add("NET_MB_From_Client_psec", "");
            //WorkloadRealParmMapping.Add("NET_MB_From_DBLink_psec", "");
            //WorkloadRealParmMapping.Add("NET_MB_To_DBLink_psec", "");




            //SqlParmList
            SqlParmsList.Add("FETCHES_TOTAL");
            SqlParmsList.Add("FETCHES_DELTA");
            SqlParmsList.Add("END_OF_FETCH_COUNT_TOTAL");
            SqlParmsList.Add("END_OF_FETCH_COUNT_DELTA");
            SqlParmsList.Add("SORTS_TOTAL");
            SqlParmsList.Add("SORTS_DELTA");
            SqlParmsList.Add("EXECUTIONS_TOTAL");
            SqlParmsList.Add("EXECUTIONS_DELTA");
            SqlParmsList.Add("PX_SERVERS_EXECS_TOTAL");
            SqlParmsList.Add("PX_SERVERS_EXECS_DELTA");
            SqlParmsList.Add("LOADS_TOTAL");
            SqlParmsList.Add("LOADS_DELTA");
            SqlParmsList.Add("INVALIDATIONS_TOTAL");
            SqlParmsList.Add("INVALIDATIONS_DELTA");
            SqlParmsList.Add("PARSE_CALLS_TOTAL");
            SqlParmsList.Add("PARSE_CALLS_DELTA");
            SqlParmsList.Add("DISK_READS_TOTAL");
            SqlParmsList.Add("DISK_READS_DELTA");
            SqlParmsList.Add("BUFFER_GETS_TOTAL");
            SqlParmsList.Add("BUFFER_GETS_DELTA");
            SqlParmsList.Add("ROWS_PROCESSED_TOTAL");
            SqlParmsList.Add("ROWS_PROCESSED_DELTA");
            SqlParmsList.Add("CPU_TIME_TOTAL");
            SqlParmsList.Add("CPU_TIME_DELTA");
            SqlParmsList.Add("ELAPSED_TIME_TOTAL");
            SqlParmsList.Add("ELAPSED_TIME_DELTA");
            SqlParmsList.Add("IOWAIT_TOTAL");
            SqlParmsList.Add("IOWAIT_DELTA");
            SqlParmsList.Add("CLWAIT_TOTAL");
            SqlParmsList.Add("CLWAIT_DELTA");
            SqlParmsList.Add("APWAIT_TOTAL");
            SqlParmsList.Add("APWAIT_DELTA");
            SqlParmsList.Add("CCWAIT_TOTAL");
            SqlParmsList.Add("CCWAIT_DELTA");
            SqlParmsList.Add("DIRECT_WRITES_TOTAL");
            SqlParmsList.Add("DIRECT_WRITES_DELTA");
            SqlParmsList.Add("PLSEXEC_TIME_TOTAL");
            SqlParmsList.Add("PLSEXEC_TIME_DELTA");
            SqlParmsList.Add("JAVEXEC_TIME_TOTAL");
            SqlParmsList.Add("JAVEXEC_TIME_DELTA");
            SqlParmsList.Add("IO_OFFLOAD_ELIG_BYTES_TOTAL");
            SqlParmsList.Add("IO_OFFLOAD_ELIG_BYTES_DELTA");
            SqlParmsList.Add("IO_INTERCONNECT_BYTES_TOTAL");
            SqlParmsList.Add("IO_INTERCONNECT_BYTES_DELTA");
            SqlParmsList.Add("PHYSICAL_READ_REQUESTS_TOTAL");
            SqlParmsList.Add("PHYSICAL_READ_REQUESTS_DELTA");
            SqlParmsList.Add("PHYSICAL_READ_BYTES_TOTAL");
            SqlParmsList.Add("PHYSICAL_READ_BYTES_DELTA");
            SqlParmsList.Add("PHYSICAL_WRITE_REQUESTS_TOTAL");
            SqlParmsList.Add("PHYSICAL_WRITE_REQUESTS_DELTA");
            SqlParmsList.Add("PHYSICAL_WRITE_BYTES_TOTAL");
            SqlParmsList.Add("PHYSICAL_WRITE_BYTES_DELTA");
            SqlParmsList.Add("OPTIMIZED_PHYSICAL_READS_TOTAL");
            SqlParmsList.Add("OPTIMIZED_PHYSICAL_READS_DELTA");
            SqlParmsList.Add("CELL_UNCOMPRESSED_BYTES_TOTAL");
            SqlParmsList.Add("CELL_UNCOMPRESSED_BYTES_DELTA");
            SqlParmsList.Add("IO_OFFLOAD_RETURN_BYTES_TOTAL");
            SqlParmsList.Add("IO_OFFLOAD_RETURN_BYTES_DELTA");

        }

        public string ParamType
        {
            get
            {
                return _ParamType;
            }

            set
            {
                _ParamType = value;
            }
        }



        public string StartTime
        {
            get
            {
                return _StartTime;
            }

            set
            {
                _StartTime = value;
            }
        }

        public string EndTime
        {
            get
            {
                return _EndTime;
            }

            set
            {
                _EndTime = value;
            }
        }

        public List<object> ParamNamesList
        {
            get
            {
                return _ParamNamesList;
            }

            set
            {
                _ParamNamesList = value;
            }
        }

        public string ParamNamesString
        {
            get
            {
                return _ParamNamesString;
            }

            set
            {
                _ParamNamesString = value;
            }
        }

        public string GroupingDateFormat
        {
            get
            {
                return _GroupingDateFormat;
            }

            set
            {
                _GroupingDateFormat = value;
            }
        }

        public static List<string> WorkloadParamNamesList
        {
            get
            {
                return _WorkloadParamNamesList;
            }

            set
            {
                _WorkloadParamNamesList = value;
            }
        }

        public int ClustersNumber
        {
            get
            {
                return _ClustersNumber;
            }

            set
            {
                _ClustersNumber = value;
            }
        }

        public string DBName
        {
            get
            {
                return _DBName;
            }

            set
            {
                _DBName = value;
            }
        }

        public static Dictionary<string, string> WorkloadSqlRelationMapping
        {
            get
            {
                return _WorkloadSqlRelationMapping;
            }

            set
            {
                _WorkloadSqlRelationMapping = value;
            }
        }



        public string WorkloadSqlParm
        {
            get
            {
                return _WorkloadSqlParm;
            }

            set
            {
                _WorkloadSqlParm = value;
            }
        }

        public List<object> SqlIdList
        {
            get
            {
                return _SqlIdList;
            }

            set
            {
                _SqlIdList = value;
            }
        }

        public static Dictionary<string, string> WorkloadBelonging
        {
            get
            {
                return _WorkloadBelonging;
            }

            set
            {
                _WorkloadBelonging = value;
            }
        }

        public static List<string> SqlParmsList
        {
            get
            {
                return _SqlParmsList;
            }

            set
            {
                _SqlParmsList = value;
            }
        }

        public static Dictionary<string, string> WorkloadRealParmMapping
        {
            get
            {
                return _WorkloadRealParmMapping;
            }

            set
            {
                _WorkloadRealParmMapping = value;
            }
        }

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(AwrArgsPack), this);
            return argsPack;
        }

    }
}