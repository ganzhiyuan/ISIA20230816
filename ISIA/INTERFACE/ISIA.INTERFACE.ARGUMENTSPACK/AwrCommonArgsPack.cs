using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP;


namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class AwrCommonArgsPack
    {
        #region Field
        private string _snapId = null;
        private string _dbId = null;
        private string _dbName = null;
        private string _instanceNumber = null;
        private string _dbLinkName = null;
        private string _serviceName = null;
        private string _ipAddress = null;
        private string _parameterId = null;
        private string _parameterName = null;
        private string _parameterType = null;
        private string _days = null;
        private string _chartUsed = null;
        private string _mailUsed = null;
        private string _mmsUsed = null;
        private string _specLimitUsed = null;
        private string _commandType = null;
        private string _commandName = null;
        private string _reportDate = null;
        private string _startTimeKey = null;
        private string _endTimeKey = null;
        private string _ruleName = null;
        private string _ruleNo = null;
        private string _sqlId = null;


        private ArgumentPack argsPack = new ArgumentPack();
        #endregion

        #region Properties
        public string SnapId { get { return _snapId; } set { _snapId = value; } }

        public string DbId { get { return _dbId; } set { _dbId = value; } }

        public string DbName { get { return _dbName; } set { _dbName = value; } }

        public string InstanceNumber { get { return _instanceNumber; } set { _instanceNumber = value; } }

        public string DbLinkName { get { return _dbLinkName; } set { _dbLinkName = value; } }

        public string ServiceName { get { return _serviceName; } set { _serviceName = value; } }

        public string IpAddress { get { return _ipAddress; } set { _ipAddress = value; } }

        public string ParameterId { get { return _parameterId; } set { _parameterId = value; } }

        public string ParameterName { get { return _parameterName; } set { _parameterName = value; } }

        public string ParameterType { get { return _parameterType; } set { _parameterType = value; } }

        public string Days { get { return _days; } set { _days = value; } }

        public string ChartUsed { get { return _chartUsed; } set { _chartUsed = value; } }

        public string MailUsed { get { return _mailUsed; } set { _mailUsed = value; } }

        public string MmsUsed { get { return _mmsUsed; } set { _mmsUsed = value; } }

        public string SpecLimitUsed { get { return _specLimitUsed; } set { _specLimitUsed = value; } }

        public string CommandType { get { return _commandType; } set { _commandType = value; } }

        public string CommandName { get { return _commandName; } set { _commandName = value; } }

        public string ReportDate { get { return _reportDate; } set { _reportDate = value; } }

        public string StartTimeKey { get { return _startTimeKey; } set { _startTimeKey = value; } }

        public string EndTimeKey { get { return _endTimeKey; } set { _endTimeKey = value; } }

        public string RuleName { get { return _ruleName; } set { _ruleName = value; } }

        public string RuleNo { get { return _ruleNo; } set { _ruleNo = value; } }

        public string SqlId { get { return _sqlId; } set { _sqlId = value; } }
        public string ChartName { get; set; }



        #endregion

        #region Method
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(AwrCommonArgsPack), this);
            return argsPack;
        }
        #endregion
    }
}
