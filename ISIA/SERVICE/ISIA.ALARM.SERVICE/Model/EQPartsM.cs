using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISEM.ALARM.SERVICE
{
    public class EQPartsM
    {
        public string AREA { get; set; }
        public string LINE { get; set; }
        public string MAINEQUIPMENT { get; set; }
        public string NAME { get; set; }
        public string REGION { get; set; }
        public string FACILITY { get; set; }
        public string BAY { get; set; }
        public string EQUIPMENT { get; set; }
        public string ISREUSED { get; set; }
        public string ISINSTALLED { get; set; }
        public string PARTTYPE { get; set; }
        public string VENDOR { get; set; }
        public string PARTMODEL { get; set; }
        public string STOCKINLOCATION { get; set; }
        public string GRADE { get; set; }
        public float UNITPRICE { get; set; }
        public DateTime PRODUCTIONDATE { get; set; }
        public DateTime STOCKINTIME { get; set; }
        public DateTime STOCKOUTTIME { get; set; }
        public DateTime INSTALLEDTIME { get; set; }
        public string USER { get; set; }
        public string ISALIVE_PART { get; set; }
        public string PARTSSTATUS { get; set; }
        public string PARTSERIALNO { get; set; }
        public string UNIT { get; set; }
        public int REPAIRTIMES { get; set; }
        public DateTime LASTREPAIRINTIME { get; set; }
        public DateTime LASTREPAIROUTTIME { get; set; }
        public bool NEEDREPAIR { get; set; } = false;
    }
}
