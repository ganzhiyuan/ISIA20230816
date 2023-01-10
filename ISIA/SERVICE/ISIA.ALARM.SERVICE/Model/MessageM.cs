using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISEM.ALARM.SERVICE
{
    public class MessageM
    {
        public string MESSAGETYPE { get; set; }
        public string APPTYPE { get; set; }
        public EquipmentM EQUIPMENT { get; set; }
        public EQPartsM PART { get; set; }
        public string PMTIME { get; set; }
        public string PMSCHEDULE { get; set; }
        public string TIMEDIFF { get; set; }
    }
}
