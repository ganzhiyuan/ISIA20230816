using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class DeviceSetupArgsPack
    {

        private string _Factory = null;
        private string _Flow = null;
        private string _Name = null;
        private string _Description = null;
        private string _Device_Status_Code = null;
        private string _Customer = null;
        private string _Customer_Device = null;
        private string _Device_Group1 = null;
        private string _Device_Group2 = null;
        private string _Device_Group3 = null;
        private string _Device_Group4 = null;
        private string _Device_Group5 = null;
        private string _Device_Customized_Field1 = null;
        private string _Device_Customized_Field2 = null;
        private string _Device_Customized_Field3 = null;
        private string _Device_Customized_Field4 = null;
        private string _Device_Customized_Field5 = null;
        private string _Device_Customized_Field6 = null;
        private string _Device_Customized_Field7 = null;
        private string _Device_Customized_Field8 = null;
        private string _Device_Customized_Field9 = null;
        private string _Device_Customized_Field10 = null;
        private int _Target_Yield = 0;
        private int _Target_Due_Day = 0;
        private int _Target_Qty1 = 0;
        private int _Target_Qty2 = 0;
        private int _Target_Qty3 = 0;
        private int _Default_Qty1 = 0;
        private int _Default_Qty2 = 0;
        private int _Default_Qty3 = 0;
        private string _Update_User = null;
        private string _LblDvc_group1 = null;
        private string _LblDvc_group2 = null;
        private string _LblDvc_group3 = null;
        private string _LblDvc_group4 = null;
        private string _LblDvc_group5 = null;
        private string _Device = null;
        private List<DeviceSetupArgsPack> _Device_Flow = null;
        private string _Dvc_Customized_Field1;
        private string _Dvc_Customized_Field2;
        private string _Dvc_Customized_Field3;
        private string _Dvc_Customized_Field4;
        private string _Dvc_Customized_Field5;
        private string _Dvc_Customized_Field6;
        private string _Dvc_Customized_Field7;
        private string _Dvc_Customized_Field8;
        private string _Dvc_Customized_Field9;
        private string _Dvc_Customized_Field10;
        private List<DeviceSetupArgsPack> _DevArgsPacks = new List<DeviceSetupArgsPack>();

        private ArgumentPack argsPack = new ArgumentPack();

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

        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
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

        public string Device_Status_Code
        {
            get
            {
                return _Device_Status_Code;
            }

            set
            {
                _Device_Status_Code = value;
            }
        }

        public string Customer
        {
            get
            {
                return _Customer;
            }

            set
            {
                _Customer = value;
            }
        }

        public string Customer_Device
        {
            get
            {
                return _Customer_Device;
            }

            set
            {
                _Customer_Device = value;
            }
        }

        public string Device_Group1
        {
            get
            {
                return _Device_Group1;
            }

            set
            {
                _Device_Group1 = value;
            }
        }

        public string Device_Group2
        {
            get
            {
                return _Device_Group2;
            }

            set
            {
                _Device_Group2 = value;
            }
        }

        public string Device_Group3
        {
            get
            {
                return _Device_Group3;
            }

            set
            {
                _Device_Group3 = value;
            }
        }

        public string Device_Group4
        {
            get
            {
                return _Device_Group4;
            }

            set
            {
                _Device_Group4 = value;
            }
        }

        public string Device_Group5
        {
            get
            {
                return _Device_Group5;
            }

            set
            {
                _Device_Group5 = value;
            }
        }

        public string Device_Customized_Field1
        {
            get
            {
                return _Device_Customized_Field1;
            }

            set
            {
                _Device_Customized_Field1 = value;
            }
        }

        public string Device_Customized_Field2
        {
            get
            {
                return _Device_Customized_Field2;
            }

            set
            {
                _Device_Customized_Field2 = value;
            }
        }

        public string Device_Customized_Field3
        {
            get
            {
                return _Device_Customized_Field3;
            }

            set
            {
                _Device_Customized_Field3 = value;
            }
        }

        public string Device_Customized_Field4
        {
            get
            {
                return _Device_Customized_Field4;
            }

            set
            {
                _Device_Customized_Field4 = value;
            }
        }

        public string Device_Customized_Field5
        {
            get
            {
                return _Device_Customized_Field5;
            }

            set
            {
                _Device_Customized_Field5 = value;
            }
        }

        public string Device_Customized_Field6
        {
            get
            {
                return _Device_Customized_Field6;
            }

            set
            {
                _Device_Customized_Field6 = value;
            }
        }

        public string Device_Customized_Field7
        {
            get
            {
                return _Device_Customized_Field7;
            }

            set
            {
                _Device_Customized_Field7 = value;
            }
        }

        public string Device_Customized_Field8
        {
            get
            {
                return _Device_Customized_Field8;
            }

            set
            {
                _Device_Customized_Field8 = value;
            }
        }

        public string Device_Customized_Field9
        {
            get
            {
                return _Device_Customized_Field9;
            }

            set
            {
                _Device_Customized_Field9 = value;
            }
        }

        public string Device_Customized_Field10
        {
            get
            {
                return _Device_Customized_Field10;
            }

            set
            {
                _Device_Customized_Field10 = value;
            }
        }

        public int Target_Yield
        {
            get
            {
                return _Target_Yield;
            }

            set
            {
                _Target_Yield = value;
            }
        }

        public int Target_Due_Day
        {
            get
            {
                return _Target_Due_Day;
            }

            set
            {
                _Target_Due_Day = value;
            }
        }

        public int Target_Qty1
        {
            get
            {
                return _Target_Qty1;
            }

            set
            {
                _Target_Qty1 = value;
            }
        }

        public int Target_Qty2
        {
            get
            {
                return _Target_Qty2;
            }

            set
            {
                _Target_Qty2 = value;
            }
        }

        public int Target_Qty3
        {
            get
            {
                return _Target_Qty3;
            }

            set
            {
                _Target_Qty3 = value;
            }
        }

        public int Default_Qty1
        {
            get
            {
                return _Default_Qty1;
            }

            set
            {
                _Default_Qty1 = value;
            }
        }

        public int Default_Qty2
        {
            get
            {
                return _Default_Qty2;
            }

            set
            {
                _Default_Qty2 = value;
            }
        }

        public int Default_Qty3
        {
            get
            {
                return _Default_Qty3;
            }

            set
            {
                _Default_Qty3 = value;
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

        public List<DeviceSetupArgsPack> DevArgsPacks
        {
            get
            {
                return _DevArgsPacks;
            }

            set
            {
                _DevArgsPacks = value;
            }
        }

        public string LblDvc_group1
        {
            get
            {
                return _LblDvc_group1;
            }

            set
            {
                _LblDvc_group1 = value;
            }
        }

        public string LblDvc_group2
        {
            get
            {
                return _LblDvc_group2;
            }

            set
            {
                _LblDvc_group2 = value;
            }
        }

        public string LblDvc_group3
        {
            get
            {
                return _LblDvc_group3;
            }

            set
            {
                _LblDvc_group3 = value;
            }
        }

        public string LblDvc_group4
        {
            get
            {
                return _LblDvc_group4;
            }

            set
            {
                _LblDvc_group4 = value;
            }
        }

        public string LblDvc_group5
        {
            get
            {
                return _LblDvc_group5;
            }

            set
            {
                _LblDvc_group5 = value;
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

  

        public string Dvc_Customized_Field1
        {
            get
            {
                return _Dvc_Customized_Field1;
            }

            set
            {
                _Dvc_Customized_Field1 = value;
            }
        }

        public string Dvc_Customized_Field2
        {
            get
            {
                return _Dvc_Customized_Field2;
            }

            set
            {
                _Dvc_Customized_Field2 = value;
            }
        }

        public string Dvc_Customized_Field3
        {
            get
            {
                return _Dvc_Customized_Field3;
            }

            set
            {
                _Dvc_Customized_Field3 = value;
            }
        }

        public string Dvc_Customized_Field4
        {
            get
            {
                return _Dvc_Customized_Field4;
            }

            set
            {
                _Dvc_Customized_Field4 = value;
            }
        }

        public string Dvc_Customized_Field5
        {
            get
            {
                return _Dvc_Customized_Field5;
            }

            set
            {
                _Dvc_Customized_Field5 = value;
            }
        }

        public string Dvc_Customized_Field6
        {
            get
            {
                return _Dvc_Customized_Field6;
            }

            set
            {
                _Dvc_Customized_Field6 = value;
            }
        }

        public string Dvc_Customized_Field7
        {
            get
            {
                return _Dvc_Customized_Field7;
            }

            set
            {
                _Dvc_Customized_Field7 = value;
            }
        }

        public string Dvc_Customized_Field8
        {
            get
            {
                return _Dvc_Customized_Field8;
            }

            set
            {
                _Dvc_Customized_Field8 = value;
            }
        }

        public string Dvc_Customized_Field9
        {
            get
            {
                return _Dvc_Customized_Field9;
            }

            set
            {
                _Dvc_Customized_Field9 = value;
            }
        }

        public string Dvc_Customized_Field10
        {
            get
            {
                return _Dvc_Customized_Field10;
            }

            set
            {
                _Dvc_Customized_Field10 = value;
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

        public List<DeviceSetupArgsPack> Device_Flow
        {
            get
            {
                return _Device_Flow;
            }

            set
            {
                _Device_Flow = value;
            }
        }

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(DeviceSetupArgsPack), this);
            return argsPack;
        }
    }
}
