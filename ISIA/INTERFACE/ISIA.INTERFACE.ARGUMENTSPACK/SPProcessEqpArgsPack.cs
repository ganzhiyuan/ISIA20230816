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
    public class SPProcessEqpArgsPack
    {
        private ArgumentPack argsPack = new ArgumentPack();

        private string _Workshop;
        private string _Line;
        private string _Report_Date;
        private string _Report_SatrtDate;
        private string _Report_EndDate;
        private string _Shift;
        private string _Process_Type;
        private string _Parameter_Name;
        private string _Parameter_Value;
        private string _Update_Time;
        private string _Insert_Time;

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

        public string Line
        {
            get
            {
                return _Line;
            }

            set
            {
                _Line = value;
            }
        }

        public string Report_Date
        {
            get
            {
                return _Report_Date;
            }

            set
            {
                _Report_Date = value;
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

        public string Process_Type
        {
            get
            {
                return _Process_Type;
            }

            set
            {
                _Process_Type = value;
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

        public string Report_SatrtDate
        {
            get
            {
                return _Report_SatrtDate;
            }

            set
            {
                _Report_SatrtDate = value;
            }
        }

        public string Report_EndDate
        {
            get
            {
                return _Report_EndDate;
            }

            set
            {
                _Report_EndDate = value;
            }
        }

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(SPProcessEqpArgsPack), this);
            return argsPack;
        }

    }
}
