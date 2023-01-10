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

    public class ELArgsPack
    {
        private List<ELArgsPack> _mROArgsPacks = new List<ELArgsPack>();
        private ArgumentPack argsPack = new ArgumentPack();
        private string _Workshop;
        private string _Line;
        private string _Process;
        private string _Equipment;
        private string _Date;
        private string _Shift;
        private string _Checktimekey;
        private string _Material;
        private string _Waferid;
        private string _StartTime;
        private string _EndtTime;

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

        public string Equipment
        {
            get
            {
                return _Equipment;
            }

            set
            {
                _Equipment = value;
            }
        }

        public string Date
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

        public string Checktimekey
        {
            get
            {
                return _Checktimekey;
            }

            set
            {
                _Checktimekey = value;
            }
        }

        public string Material
        {
            get
            {
                return _Material;
            }

            set
            {
                _Material = value;
            }
        }

        public string Waferid
        {
            get
            {
                return _Waferid;
            }

            set
            {
                _Waferid = value;
            }
        }

        public string StartTime
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

        public string EndtTime
        {
            get
            {
                return _EndtTime;
            }

            set
            {
                _EndtTime = value;
            }
        }
        
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(ELArgsPack), this);
            return argsPack;
        }
    }   
}