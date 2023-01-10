using System;
using System.Collections.Generic;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class CustomizedFieldArgsPack
    {
        #region Fields
        private ArgumentPack argsPack = new ArgumentPack();
        private string _Factory;
        private string _Customized_Field_Item;
        private string _Prompt1;
        private string _Prompt2;
        private string _Prompt3;
        private string _Prompt4;
        private string _Prompt5;
        private string _Prompt6;
        private string _Prompt7;
        private string _Prompt8;
        private string _Prompt9;
        private string _Prompt10;
        private string _Prompt11;
        private string _Prompt12;
        private string _Prompt13;
        private string _Prompt14;
        private string _Prompt15;
        private string _Prompt16;
        private string _Prompt17;
        private string _Prompt18;
        private string _Prompt19;
        private string _Prompt20;
        private string _Table1;
        private string _Table2;
        private string _Table3;
        private string _Table4;
        private string _Table5;
        private string _Table6;
        private string _Table7;
        private string _Table8;
        private string _Table9;
        private string _Table10;
        private string _Table11;
        private string _Table12;
        private string _Table13;
        private string _Table14;
        private string _Table15;
        private string _Table16;
        private string _Table17;
        private string _Table18;
        private string _Table19;
        private string _Table20;
        private string _Reserved_Field1;
        private string _Reserved_Field2;
        private string _Reserved_Field3;
        private string _Reserved_Field4;
        private string _Reserved_Field5;
        private string _Update_Time;
        private string _Update_User;
        private List<CustomizedFieldArgsPack> _CustomizedFieldArgsPacks = new List<CustomizedFieldArgsPack>();
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

        public string Customized_Field_Item
        {
            get
            {
                return _Customized_Field_Item;
            }

            set
            {
                _Customized_Field_Item = value;
            }
        }

        public string Prompt1
        {
            get
            {
                return _Prompt1;
            }

            set
            {
                _Prompt1 = value;
            }
        }

        public string Prompt2
        {
            get
            {
                return _Prompt2;
            }

            set
            {
                _Prompt2 = value;
            }
        }

        public string Prompt3
        {
            get
            {
                return _Prompt3;
            }

            set
            {
                _Prompt3 = value;
            }
        }

        public string Prompt4
        {
            get
            {
                return _Prompt4;
            }

            set
            {
                _Prompt4 = value;
            }
        }

        public string Prompt5
        {
            get
            {
                return _Prompt5;
            }

            set
            {
                _Prompt5 = value;
            }
        }

        public string Prompt6
        {
            get
            {
                return _Prompt6;
            }

            set
            {
                _Prompt6 = value;
            }
        }

        public string Prompt7
        {
            get
            {
                return _Prompt7;
            }

            set
            {
                _Prompt7 = value;
            }
        }

        public string Prompt8
        {
            get
            {
                return _Prompt8;
            }

            set
            {
                _Prompt8 = value;
            }
        }

        public string Prompt9
        {
            get
            {
                return _Prompt9;
            }

            set
            {
                _Prompt9 = value;
            }
        }

        public string Prompt10
        {
            get
            {
                return _Prompt10;
            }

            set
            {
                _Prompt10 = value;
            }
        }

        public string Prompt11
        {
            get
            {
                return _Prompt11;
            }

            set
            {
                _Prompt11 = value;
            }
        }

        public string Prompt12
        {
            get
            {
                return _Prompt12;
            }

            set
            {
                _Prompt12 = value;
            }
        }

        public string Prompt13
        {
            get
            {
                return _Prompt13;
            }

            set
            {
                _Prompt13 = value;
            }
        }

        public string Prompt14
        {
            get
            {
                return _Prompt14;
            }

            set
            {
                _Prompt14 = value;
            }
        }

        public string Prompt15
        {
            get
            {
                return _Prompt15;
            }

            set
            {
                _Prompt15 = value;
            }
        }

        public string Prompt16
        {
            get
            {
                return _Prompt16;
            }

            set
            {
                _Prompt16 = value;
            }
        }

        public string Prompt17
        {
            get
            {
                return _Prompt17;
            }

            set
            {
                _Prompt17 = value;
            }
        }

        public string Prompt18
        {
            get
            {
                return _Prompt18;
            }

            set
            {
                _Prompt18 = value;
            }
        }

        public string Prompt19
        {
            get
            {
                return _Prompt19;
            }

            set
            {
                _Prompt19 = value;
            }
        }

        public string Prompt20
        {
            get
            {
                return _Prompt20;
            }

            set
            {
                _Prompt20 = value;
            }
        }

        public string Table1
        {
            get
            {
                return _Table1;
            }

            set
            {
                _Table1 = value;
            }
        }

        public string Table2
        {
            get
            {
                return _Table2;
            }

            set
            {
                _Table2 = value;
            }
        }

        public string Table3
        {
            get
            {
                return _Table3;
            }

            set
            {
                _Table3 = value;
            }
        }

        public string Table4
        {
            get
            {
                return _Table4;
            }

            set
            {
                _Table4 = value;
            }
        }

        public string Table5
        {
            get
            {
                return _Table5;
            }

            set
            {
                _Table5 = value;
            }
        }

        public string Table6
        {
            get
            {
                return _Table6;
            }

            set
            {
                _Table6 = value;
            }
        }

        public string Table7
        {
            get
            {
                return _Table7;
            }

            set
            {
                _Table7 = value;
            }
        }

        public string Table8
        {
            get
            {
                return _Table8;
            }

            set
            {
                _Table8 = value;
            }
        }

        public string Table9
        {
            get
            {
                return _Table9;
            }

            set
            {
                _Table9 = value;
            }
        }

        public string Table10
        {
            get
            {
                return _Table10;
            }

            set
            {
                _Table10 = value;
            }
        }

        public string Table11
        {
            get
            {
                return _Table11;
            }

            set
            {
                _Table11 = value;
            }
        }

        public string Table12
        {
            get
            {
                return _Table12;
            }

            set
            {
                _Table12 = value;
            }
        }

        public string Table13
        {
            get
            {
                return _Table13;
            }

            set
            {
                _Table13 = value;
            }
        }

        public string Table14
        {
            get
            {
                return _Table14;
            }

            set
            {
                _Table14 = value;
            }
        }

        public string Table15
        {
            get
            {
                return _Table15;
            }

            set
            {
                _Table15 = value;
            }
        }

        public string Table16
        {
            get
            {
                return _Table16;
            }

            set
            {
                _Table16 = value;
            }
        }

        public string Table17
        {
            get
            {
                return _Table17;
            }

            set
            {
                _Table17 = value;
            }
        }

        public string Table18
        {
            get
            {
                return _Table18;
            }

            set
            {
                _Table18 = value;
            }
        }

        public string Table19
        {
            get
            {
                return _Table19;
            }

            set
            {
                _Table19 = value;
            }
        }

        public string Table20
        {
            get
            {
                return _Table20;
            }

            set
            {
                _Table20 = value;
            }
        }

        public string Reserved_Field1
        {
            get
            {
                return _Reserved_Field1;
            }

            set
            {
                _Reserved_Field1 = value;
            }
        }

        public string Reserved_Field2
        {
            get
            {
                return _Reserved_Field2;
            }

            set
            {
                _Reserved_Field2 = value;
            }
        }

        public string Reserved_Field3
        {
            get
            {
                return _Reserved_Field3;
            }

            set
            {
                _Reserved_Field3 = value;
            }
        }

        public string Reserved_Field4
        {
            get
            {
                return _Reserved_Field4;
            }

            set
            {
                _Reserved_Field4 = value;
            }
        }

        public string Reserved_Field5
        {
            get
            {
                return _Reserved_Field5;
            }

            set
            {
                _Reserved_Field5 = value;
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

        public List<CustomizedFieldArgsPack> CustomizedFieldArgsPacks
        {
            get
            {
                return _CustomizedFieldArgsPacks;
            }

            set
            {
                _CustomizedFieldArgsPacks = value;
            }
        }
        #endregion
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(CustomizedFieldArgsPack), this);
            return argsPack;
        }
    }
}
