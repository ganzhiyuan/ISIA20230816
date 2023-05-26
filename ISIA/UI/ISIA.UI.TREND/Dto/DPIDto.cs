using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIA.UI.TREND.Dto
{
    public class DPIDto
    {
        public string DPIFileName { get; set; }
        public string Xvalue { get; set; }
        public string HeaderText { get; set; }
        public int YRValueType { get; set; }
        public int YLValueType { get; set; }
        public List<DPIAboutY> FileNameList { get; set; }
    }

    public class DPIAboutY
    {
        public string FileNameParament { get; set; }
        public bool IsLeftY { get; set; }
    }

}
