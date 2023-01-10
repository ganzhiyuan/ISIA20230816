using System;
using System.Collections.Generic;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class UPTArgsPack
    {
        #region Fields
        private ArgumentPack argsPack = new ArgumentPack();
        private string _Factory;
        private string _Table_Name;
        private string _Table_Description;
        private string _Key1_Prompt;
        private string _Key2_Prompt;
        private string _Data1_Prompt;
        private string _Data2_Prompt;
        private string _Data3_Prompt;
        private string _Data4_Prompt;
        private string _Data5_Prompt;
        private string _Data6_Prompt;
        private string _Data7_Prompt;
        private string _Data8_Prompt;
        private string _Data9_Prompt;
        private string _Data10_Prompt;
        private string _Update_User;
        private List<UPTArgsPack> _UPTArgsPacks = new List<UPTArgsPack>();
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

        public string Table_Name
        {
            get
            {
                return _Table_Name;
            }

            set
            {
                _Table_Name = value;
            }
        }

        public string Table_Description
        {
            get
            {
                return _Table_Description;
            }

            set
            {
                _Table_Description = value;
            }
        }

        public string Key1_Prompt
        {
            get
            {
                return _Key1_Prompt;
            }

            set
            {
                _Key1_Prompt = value;
            }
        }

        public string Key2_Prompt
        {
            get
            {
                return _Key2_Prompt;
            }

            set
            {
                _Key2_Prompt = value;
            }
        }

        public string Data1_Prompt
        {
            get
            {
                return _Data1_Prompt;
            }

            set
            {
                _Data1_Prompt = value;
            }
        }

        public string Data2_Prompt
        {
            get
            {
                return _Data2_Prompt;
            }

            set
            {
                _Data2_Prompt = value;
            }
        }

        public string Data3_Prompt
        {
            get
            {
                return _Data3_Prompt;
            }

            set
            {
                _Data3_Prompt = value;
            }
        }

        public string Data4_Prompt
        {
            get
            {
                return _Data4_Prompt;
            }

            set
            {
                _Data4_Prompt = value;
            }
        }

        public string Data5_Prompt
        {
            get
            {
                return _Data5_Prompt;
            }

            set
            {
                _Data5_Prompt = value;
            }
        }

        public string Data6_Prompt
        {
            get
            {
                return _Data6_Prompt;
            }

            set
            {
                _Data6_Prompt = value;
            }
        }

        public string Data7_Prompt
        {
            get
            {
                return _Data7_Prompt;
            }

            set
            {
                _Data7_Prompt = value;
            }
        }

        public string Data8_Prompt
        {
            get
            {
                return _Data8_Prompt;
            }

            set
            {
                _Data8_Prompt = value;
            }
        }

        public string Data9_Prompt
        {
            get
            {
                return _Data9_Prompt;
            }

            set
            {
                _Data9_Prompt = value;
            }
        }

        public string Data10_Prompt
        {
            get
            {
                return _Data10_Prompt;
            }

            set
            {
                _Data10_Prompt = value;
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

        public List<UPTArgsPack> UPTArgsPacks
        {
            get
            {
                return _UPTArgsPacks;
            }

            set
            {
                _UPTArgsPacks = value;
            }
        }
        #endregion

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(UPTArgsPack), this);
            return argsPack;
        }
    }
}
