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
    public class AwrArgsPack
    {
        private ArgumentPack argsPack = new ArgumentPack();


        private string _ParamType;

        private List<object> _ParamNamesList;

        private string _ParamNamesString;


        private string _StartTime;

        private string _EndTime;

        private string _GroupingDateFormat;


        public string ParamType
        {
            get
            {
                return _ParamType;
            }

            set
            {
                _ParamType = value;
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

        public string EndTime
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

        public List<object> ParamNamesList
        {
            get
            {
                return _ParamNamesList;
            }

            set
            {
                _ParamNamesList = value;
            }
        }

        public string ParamNamesString
        {
            get
            {
                return _ParamNamesString;
            }

            set
            {
                _ParamNamesString = value;
            }
        }

        public string GroupingDateFormat
        {
            get
            {
                return _GroupingDateFormat;
            }

            set
            {
                _GroupingDateFormat = value;
            }
        }

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(AwrArgsPack), this);
            return argsPack;
        }

    }
}
