using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIA.UI.TREND.Dto
{
    public class SnapshotDto
    {
        public string SNAP_ID { get; set; }
        public string PARAMENT_NAME { get; set; }
        public string PARAMENT_VALUE { get; set; }
        public string DBID { get; set; }
        public DateTime END_INTERVAL_TIME { get; set; }

        public string SQL_ID { get; set; }
        public string Value { get; set; }
    }
}
