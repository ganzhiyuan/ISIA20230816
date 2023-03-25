using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class ChartServiceArgsPack
    {
        private string _dbId = null;
        private string _instance_Number = null;
        private string _reportDate = null;
        private string _parameterId = null;
        private string _parameterName = null;
        private string _ruleName = null;
        private string _ruleNo = null;
        private string _startTimeKey = null;
        private string _endTimeKey = null;
        private string _imagePath = null;
        private string _detectionFlag = null;

        private ArgumentPack argsPack = new ArgumentPack();

        public string DbId { get { return _dbId; } set { _dbId = value; } }

        public string Instance_Number { get { return _instance_Number; } set { _instance_Number = value; } }

        public string ReportDate { get { return _reportDate; } set { _reportDate = value; } }

        public string ParameterId { get { return _parameterId; } set { _parameterId = value; } }

        public string ParameterName { get { return _parameterName; } set { _parameterName = value; } }

        public string RuleName { get { return _ruleName; } set { _ruleName = value; } }

        public string RuleNo { get { return _ruleNo; } set { _ruleNo = value; } }

        public string StartTimeKey { get { return _startTimeKey; } set { _startTimeKey = value; } }

        public string EndTimeKey { get { return _endTimeKey; } set { _endTimeKey = value; } }

        public string ImagePath { get { return _imagePath; } set { _imagePath = value; } }

        public string DetectionFlag { get { return _detectionFlag; } set { _detectionFlag = value; } }

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(ChartServiceArgsPack), this);
            return argsPack;
        }
    }
}
