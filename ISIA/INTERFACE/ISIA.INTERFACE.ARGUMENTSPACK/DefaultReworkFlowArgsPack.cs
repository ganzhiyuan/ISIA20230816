using System;
using System.Collections.Generic;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class DefaultReworkFlowArgsPack
    {
        #region Fields
        private ArgumentPack argsPack = new ArgumentPack();
        private string _Factory;
        private string _Device;
        private string _Flow;
        private string _Operation;
        private string _Rework_Code;
        private string _Rework_Flow;
        private string _Rework_Operation;
        private string _Rework_Ret_Flow;
        private string _Rework_Ret_Operation;
        private string _Ret_Rework_Clear_Yn;
        private string _Update_Time;
        private string _Update_User;
        private List<DefaultReworkFlowArgsPack> _DefaultReworkFlowArgsPacks = new List<DefaultReworkFlowArgsPack>();
        #endregion

        #region Properties
        public string Factory
        {
            get
            {
                return _Factory;
            }

            set
            {
                _Factory = value;
            }
        }

        public string Device
        {
            get
            {
                return _Device;
            }

            set
            {
                _Device = value;
            }
        }

        public string Flow
        {
            get
            {
                return _Flow;
            }

            set
            {
                _Flow = value;
            }
        }

        public string Operation
        {
            get
            {
                return _Operation;
            }

            set
            {
                _Operation = value;
            }
        }

        public string Rework_Code
        {
            get
            {
                return _Rework_Code;
            }

            set
            {
                _Rework_Code = value;
            }
        }

        public string Rework_Flow
        {
            get
            {
                return _Rework_Flow;
            }

            set
            {
                _Rework_Flow = value;
            }
        }

        public string Rework_Operation
        {
            get
            {
                return _Rework_Operation;
            }

            set
            {
                _Rework_Operation = value;
            }
        }

        public string Rework_Ret_Flow
        {
            get
            {
                return _Rework_Ret_Flow;
            }

            set
            {
                _Rework_Ret_Flow = value;
            }
        }

        public string Rework_Ret_Operation
        {
            get
            {
                return _Rework_Ret_Operation;
            }

            set
            {
                _Rework_Ret_Operation = value;
            }
        }

        public string Ret_Rework_Clear_Yn
        {
            get
            {
                return _Ret_Rework_Clear_Yn;
            }

            set
            {
                _Ret_Rework_Clear_Yn = value;
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

        public string Update_User
        {
            get
            {
                return _Update_User;
            }

            set
            {
                _Update_User = value;
            }
        }

        public List<DefaultReworkFlowArgsPack> DefaultReworkFlowArgsPacks
        {
            get
            {
                return _DefaultReworkFlowArgsPacks;
            }

            set
            {
                _DefaultReworkFlowArgsPacks = value;
            }
        }
        #endregion
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(DefaultReworkFlowArgsPack), this);
            return argsPack;
        }
    }
}
