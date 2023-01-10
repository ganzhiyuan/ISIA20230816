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
    public class EqpDataSumArgsPack
    {
        private ArgumentPack argsPack = new ArgumentPack();

        private string _Workshop;
        private string _Process;
        private string _Date;
        private string _Shift;
        private string _Category1;
        private string _Category2;
        private string _Category3;
        private string _Category4;
        private string _Category5;
        private string _Parameter_Name;
        private string _Parameter_Value;
        private string _Update_Time;
        private string _Insert_Time;
        private string _Serach_Start_Time;
        private string _Serach_End_Time;

        public string Workshop
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

        public string Process
        {
            get
            {
                return _Process;
            }

            set
            {
                _Process = value;
            }
        }

        public string Report_Date
        {
            get
            {
                return _Date;
            }

            set
            {
                _Date = value;
            }
        }

        public string Shift
        {
            get
            {
                return _Shift;
            }

            set
            {
                _Shift = value;
            }
        }

        public string Category1
        {
            get
            {
                return _Category1;
            }

            set
            {
                _Category1 = value;
            }
        }

        public string Parameter_Name
        {
            get
            {
                return _Parameter_Name;
            }

            set
            {
                _Parameter_Name = value;
            }
        }

        public string Parameter_Value
        {
            get
            {
                return _Parameter_Value;
            }

            set
            {
                _Parameter_Value = value;
            }
        }

        public string Update_Time
        {
            get
            {
                return _Update_Time;
            }

            set
            {
                _Update_Time = value;
            }
        }

        public string Insert_Time
        {
            get
            {
                return _Insert_Time;
            }

            set
            {
                _Insert_Time = value;
            }
        }
        
        public string Category2
        {
            get
            {
                return _Category2;
            }

            set
            {
                _Category2 = value;
            }
        }

        public string Category3
        {
            get
            {
                return _Category3;
            }

            set
            {
                _Category3 = value;
            }
        }

        public string Category4
        {
            get
            {
                return _Category4;
            }

            set
            {
                _Category4 = value;
            }
        }

        public string Category5
        {
            get
            {
                return _Category5;
            }

            set
            {
                _Category5 = value;
            }
        }

        public string Serach_Start_Time
        {
            get
            {
                return _Serach_Start_Time;
            }

            set
            {
                _Serach_Start_Time = value;
            }
        }

        public string Serach_End_Time
        {
            get
            {
                return _Serach_End_Time;
            }

            set
            {
                _Serach_End_Time = value;
            }
        }

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(EqpDataSumArgsPack), this);
            return argsPack;
        }

    }
}
