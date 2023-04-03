using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIA.UI.ANALYSIS.Dto
{
    class WorkloadDto
    {
        private string _WorkloadParm;
        private string _WorkloadValue;
        private DateTime _Time;
        private string _DbName;

        public string WorkloadParm { get => _WorkloadParm; set => _WorkloadParm = value; }
        public DateTime Time { get => _Time; set => _Time = value; }
        public string WorkloadValue { get => _WorkloadValue; set => _WorkloadValue = value; }
        public string DbName { get => _DbName; set => _DbName = value; }
    }
}
