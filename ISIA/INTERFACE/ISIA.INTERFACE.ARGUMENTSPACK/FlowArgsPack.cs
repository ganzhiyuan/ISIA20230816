using System;
using System.Collections.Generic;
using TAP;


namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class FlowArgsPack
    {
        private ArgumentPack argsPack = new ArgumentPack();
        #region Fields
        private string _Factory;
        private string _Flow;
        private string _Description;
        private string _Rework_Flow_YN;
        private string _Alter_Flow_YN;
        private string _Flow_Group1;
        private string _Flow_Group2;
        private string _Flow_Group3;
        private string _Flow_Group4;
        private string _Flow_Group5;
        private string _Flow_Customized_Field1;
        private string _Flow_Customized_Field2;
        private string _Flow_Customized_Field3;
        private string _Flow_Customized_Field4;
        private string _Flow_Customized_Field5;
        private string _Flow_Customized_Field6;
        private string _Flow_Customized_Field7;
        private string _Flow_Customized_Field8;
        private string _Flow_Customized_Field9;
        private string _Flow_Customized_Field10;
        private string _First_Oper;
        private string _Last_Oper;
        private string _Line_ID;
        private string _Area_ID;
        private string _Flow_Status_Code;
        private string _Update_Time;
        private string _Update_User;
        private string _LblFlw_group1;
        private string _LblFlw_group2;
        private string _LblFlw_group3;
        private string _LblFlw_group4;
        private string _LblFlw_group5;
        private string _Operation;
        private string _Flw_Customized_Field1;
        private string _Flw_Customized_Field2;
        private string _Flw_Customized_Field3;
        private string _Flw_Customized_Field4;
        private string _Flw_Customized_Field5;
        private string _Flw_Customized_Field6;
        private string _Flw_Customized_Field7;
        private string _Flw_Customized_Field8;
        private string _Flw_Customized_Field9;
        private string _Flw_Customized_Field10;
        private string _CurrentFlowOperation;
        private List<FlowArgsPack> _Flow_Operation;
        private List<FlowArgsPack> _FlowArgsPacks = new List<FlowArgsPack>();
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

        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        public string Rework_Flow_YN
        {
            get
            {
                return _Rework_Flow_YN;
            }

            set
            {
                _Rework_Flow_YN = value;
            }
        }

        public string Alter_Flow_YN
        {
            get
            {
                return _Alter_Flow_YN;
            }

            set
            {
                _Alter_Flow_YN = value;
            }
        }

        public string Flow_Group1
        {
            get
            {
                return _Flow_Group1;
            }

            set
            {
                _Flow_Group1 = value;
            }
        }

        public string Flow_Group2
        {
            get
            {
                return _Flow_Group2;
            }

            set
            {
                _Flow_Group2 = value;
            }
        }

        public string Flow_Group3
        {
            get
            {
                return _Flow_Group3;
            }

            set
            {
                _Flow_Group3 = value;
            }
        }

        public string Flow_Group4
        {
            get
            {
                return _Flow_Group4;
            }

            set
            {
                _Flow_Group4 = value;
            }
        }

        public string Flow_Group5
        {
            get
            {
                return _Flow_Group5;
            }

            set
            {
                _Flow_Group5 = value;
            }
        }

        public string Flow_Customized_Field1
        {
            get
            {
                return _Flow_Customized_Field1;
            }

            set
            {
                _Flow_Customized_Field1 = value;
            }
        }

        public string Flow_Customized_Field2
        {
            get
            {
                return _Flow_Customized_Field2;
            }

            set
            {
                _Flow_Customized_Field2 = value;
            }
        }

        public string Flow_Customized_Field3
        {
            get
            {
                return _Flow_Customized_Field3;
            }

            set
            {
                _Flow_Customized_Field3 = value;
            }
        }

        public string Flow_Customized_Field4
        {
            get
            {
                return _Flow_Customized_Field4;
            }

            set
            {
                _Flow_Customized_Field4 = value;
            }
        }

        public string Flow_Customized_Field5
        {
            get
            {
                return _Flow_Customized_Field5;
            }

            set
            {
                _Flow_Customized_Field5 = value;
            }
        }
        public string Flow_Customized_Field6
        {
            get
            {
                return _Flow_Customized_Field6;
            }

            set
            {
                _Flow_Customized_Field6 = value;
            }
        }

        public string Flow_Customized_Field7
        {
            get
            {
                return _Flow_Customized_Field7;
            }

            set
            {
                _Flow_Customized_Field7 = value;
            }
        }

        public string Flow_Customized_Field8
        {
            get
            {
                return _Flow_Customized_Field8;
            }

            set
            {
                _Flow_Customized_Field8 = value;
            }
        }

        public string Flow_Customized_Field9
        {
            get
            {
                return _Flow_Customized_Field9;
            }

            set
            {
                _Flow_Customized_Field9 = value;
            }
        }

        public string Flow_Customized_Field10
        {
            get
            {
                return _Flow_Customized_Field10;
            }

            set
            {
                _Flow_Customized_Field10 = value;
            }
        }
        public string First_Oper
        {
            get
            {
                return _First_Oper;
            }

            set
            {
                _First_Oper = value;
            }
        }

        public string Last_Oper
        {
            get
            {
                return _Last_Oper;
            }

            set
            {
                _Last_Oper = value;
            }
        }

        public string Line_ID
        {
            get
            {
                return _Line_ID;
            }

            set
            {
                _Line_ID = value;
            }
        }

        public string Area_ID
        {
            get
            {
                return _Area_ID;
            }

            set
            {
                _Area_ID = value;
            }
        }
        
        public string Flow_Status_Code
        {
            get
            {
                return _Flow_Status_Code;
            }

            set
            {
                _Flow_Status_Code = value;
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
        public string LblFlw_group1
        {
            get
            {
                return _LblFlw_group1;
            }

            set
            {
                _LblFlw_group1 = value;
            }
        }
        public string LblFlw_group2
        {
            get
            {
                return _LblFlw_group2;
            }

            set
            {
                _LblFlw_group2 = value;
            }
        }

        public string LblFlw_group3
        {
            get
            {
                return _LblFlw_group3;
            }

            set
            {
                _LblFlw_group3 = value;
            }
        }

        public string LblFlw_group4
        {
            get
            {
                return _LblFlw_group4;
            }

            set
            {
                _LblFlw_group4 = value;
            }
        }

        public string LblFlw_group5
        {
            get
            {
                return _LblFlw_group5;
            }

            set
            {
                _LblFlw_group5 = value;
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

      
      
        public List<FlowArgsPack> FlowArgsPacks
        {
            get
            {
                return _FlowArgsPacks;
            }

            set
            {
                _FlowArgsPacks = value;
            }
        }

        public List<FlowArgsPack> Flow_Operation
        {
            get
            {
                return _Flow_Operation;
            }

            set
            {
                _Flow_Operation = value;
            }
        }

        public string CurrentFlowOperation
        {
            get
            {
                return _CurrentFlowOperation;
            }

            set
            {
                _CurrentFlowOperation = value;
            }
        }

        public string Flw_Customized_Field1
        {
            get
            {
                return _Flw_Customized_Field1;
            }

            set
            {
                _Flw_Customized_Field1 = value;
            }
        }

        public string Flw_Customized_Field2
        {
            get
            {
                return _Flw_Customized_Field2;
            }

            set
            {
                _Flw_Customized_Field2 = value;
            }
        }

        public string Flw_Customized_Field3
        {
            get
            {
                return _Flw_Customized_Field3;
            }

            set
            {
                _Flw_Customized_Field3 = value;
            }
        }

        public string Flw_Customized_Field4
        {
            get
            {
                return _Flw_Customized_Field4;
            }

            set
            {
                _Flw_Customized_Field4 = value;
            }
        }

        public string Flw_Customized_Field5
        {
            get
            {
                return _Flw_Customized_Field5;
            }

            set
            {
                _Flw_Customized_Field5 = value;
            }
        }

        public string Flw_Customized_Field6
        {
            get
            {
                return _Flw_Customized_Field6;
            }

            set
            {
                _Flw_Customized_Field6 = value;
            }
        }

        public string Flw_Customized_Field7
        {
            get
            {
                return _Flw_Customized_Field7;
            }

            set
            {
                _Flw_Customized_Field7 = value;
            }
        }

        public string Flw_Customized_Field8
        {
            get
            {
                return _Flw_Customized_Field8;
            }

            set
            {
                _Flw_Customized_Field8 = value;
            }
        }

        public string Flw_Customized_Field9
        {
            get
            {
                return _Flw_Customized_Field9;
            }

            set
            {
                _Flw_Customized_Field9 = value;
            }
        }

        public string Flw_Customized_Field10
        {
            get
            {
                return _Flw_Customized_Field10;
            }

            set
            {
                _Flw_Customized_Field10 = value;
            }
        }

        #endregion

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(FlowArgsPack), this);
            return argsPack;
        }
    }
}
