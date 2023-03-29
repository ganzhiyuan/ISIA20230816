using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIA.UI.TREND.Dto
{
    public class SnapshotDto
    {
        public decimal SNAP_ID { get; set; }
        public decimal DBID { get; set; }
        public string PARAMENT_NAME { get; set; }
        public decimal PARAMENT_VALUE { get; set; }
        //public string DBID { get; set; }
        public DateTime END_INTERVAL_TIME { get; set; }

        //public string SQL_ID { get; set; }
        //public string Value { get; set; }
    }
}
