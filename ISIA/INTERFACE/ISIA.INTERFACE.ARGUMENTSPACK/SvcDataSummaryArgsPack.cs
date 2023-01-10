using System;
using DevExpress.Xpo;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class SvcDataSummaryArgsPack 
    {
        private ArgumentPack argsPack = new ArgumentPack();

        private string _FabId = null;
        private string _DateTimeStart = null;
        private string _DateTimeEnd = null;
        private string _WorkShop = null;
        private string _Line = null;
        private string _Process = null;
        private string _Equipment = null;

        public string FabId
        {
            get
            {
                return _FabId;
            }

            set
            {
                _FabId = value;
            }
        }

        public string DateTimeStart
        {
            get
            {
                return _DateTimeStart;
            }

            set
            {
                _DateTimeStart = value;
            }
        }

        public string DateTimeEnd
        {
            get
            {
                return _DateTimeEnd;
            }

            set
            {
                _DateTimeEnd = value;
            }
        }

        public string WorkShop
        {
            get
            {
                return _WorkShop;
            }

            set
            {
                _WorkShop = value;
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

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(SvcDataSummaryArgsPack), this);
            return argsPack;
        }

    }

}