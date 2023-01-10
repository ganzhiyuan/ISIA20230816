using System;
using System.Collections.Generic;
using TAP;


namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class UPTDataArgsPack
    {
        #region Fields
        private ArgumentPack argsPack = new ArgumentPack();
        private string _Factory;
        private string _Table_Name;
        private string _Table_Description;
        private string _Key1;
        private string _Key2;
        private string _Data1;
        private string _Data2;
        private string _Data3;
        private string _Data4;
        private string _Data5;
        private string _Data6;
        private string _Data7;
        private string _Data8;
        private string _Data9;
        private string _Data10;
        private string _Update_User;
        private List<UPTDataArgsPack> _UPTDataArgsPacks = new List<UPTDataArgsPack>();
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

        public string Key1
        {
            get
            {
                return _Key1;
            }

            set
            {
                _Key1 = value;
            }
        }

        public string Key2
        {
            get
            {
                return _Key2;
            }

            set
            {
                _Key2 = value;
            }
        }

        public string Data1
        {
            get
            {
                return _Data1;
            }

            set
            {
                _Data1 = value;
            }
        }

        public string Data2
        {
            get
            {
                return _Data2;
            }

            set
            {
                _Data2 = value;
            }
        }

        public string Data3
        {
            get
            {
                return _Data3;
            }

            set
            {
                _Data3 = value;
            }
        }

        public string Data4
        {
            get
            {
                return _Data4;
            }

            set
            {
                _Data4 = value;
            }
        }

        public string Data5
        {
            get
            {
                return _Data5;
            }

            set
            {
                _Data5 = value;
            }
        }

        public string Data6
        {
            get
            {
                return _Data6;
            }

            set
            {
                _Data6 = value;
            }
        }

        public string Data7
        {
            get
            {
                return _Data7;
            }

            set
            {
                _Data7 = value;
            }
        }

        public string Data8
        {
            get
            {
                return _Data8;
            }

            set
            {
                _Data8 = value;
            }
        }

        public string Data9
        {
            get
            {
                return _Data9;
            }

            set
            {
                _Data9 = value;
            }
        }

        public string Data10
        {
            get
            {
                return _Data10;
            }

            set
            {
                _Data10 = value;
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

        public List<UPTDataArgsPack> UPTDataArgsPacks
        {
            get
            {
                return _UPTDataArgsPacks;
            }

            set
            {
                _UPTDataArgsPacks = value;
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
        #endregion
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(UPTDataArgsPack), this);
            return argsPack;
        }
    }
}
