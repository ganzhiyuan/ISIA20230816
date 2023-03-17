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

        private static Dictionary<string, string> _WorkloadBelonging = new Dictionary<string, string>();

        public const string METRIC= "METRIC";

        public const string SYSSTAT = "SYSSTAT";


        public const string GET_ALL = "ALL";

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
            //_WorkloadSqlRelationMapping
            WorkloadSqlRelationMapping.Add("CPU Usage Per Sec", "CPU_TIME_total");
            WorkloadSqlRelationMapping.Add("DB time", "ELAPSED_TIME_total");
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

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(AwrArgsPack), this);
            return argsPack;
        }

    }
}
