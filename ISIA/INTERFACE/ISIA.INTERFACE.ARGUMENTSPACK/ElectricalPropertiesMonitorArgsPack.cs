using System;
using System.Collections.Generic;
using TAP;
using TAP.Models;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class ElectricalPropertiesMonitorArgsPack
    {
        private ArgumentPack argsPack = new ArgumentPack();

        private List<string> _Workshop = null;
        private string _Parameter = null;
        private string _TimeType = null;
        private string _Value = null;
        private DateTime _TestTimeDate = new DateTime();
        private DateTime _StartTime = new DateTime();
        private DateTime _EndTime = new DateTime();
        private Dictionary<string, string> _SqlConnection = new Dictionary<string, string>();

        
        public string Parameter
        {
            get
            {
                return _Parameter;
            }

            set
            {
                _Parameter = value;
            }
        }

        public string Value
        {
            get
            {
                return _Value;
            }

            set
            {
                _Value = value;
            }
        }

      
        public string TimeType
        {
            get
            {
                return _TimeType;
            }

            set
            {
                _TimeType = value;
            }
        }

        public List<string> Workshop
        {
            get
            {
                return _Workshop;
            }

            set
            {
                _Workshop = value;
            }
        }

        public Dictionary<string, string> SqlConnection
        {
            get
            {
                return _SqlConnection;
            }

            set
            {
                _SqlConnection = value;
            }
        }

        public DateTime TestTimeDate
        {
            get
            {
                return _TestTimeDate;
            }

            set
            {
                _TestTimeDate = value;
            }
        }

        public DateTime StartTime
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

        public DateTime EndTime
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

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(ElectricalPropertiesMonitorArgsPack), this);
            return argsPack;
        }
    }
}
