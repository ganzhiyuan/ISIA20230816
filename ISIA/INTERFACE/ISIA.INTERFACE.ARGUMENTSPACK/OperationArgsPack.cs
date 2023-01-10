using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAP;
namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class OperationArgsPack
    {
        private ArgumentPack argsPack = new ArgumentPack();

        private string _Factory = null;
        private string _Name = null;
        private string _Short_description = null;
        private string _Description = null;
        private string _Operation_group1 = null;
        private string _Operation_group2 = null;
        private string _Operation_group3 = null;
        private string _Operation_group4 = null;
        private string _Operation_group5 = null;
        private string _Intransit_operation_yn = null;
        private string _Shipping_operation_yn = null;
        private string _Store_operation_yn = null;
        private string _Start_require_operation_yn = null;
        private string _End_operation_yn = null;
        private string _Loss_tbl = null;
        private string _Bonus_tbl = null;
        private string _Rework_tbl = null;
        private string _Line_id = null;
        private string _Area_id = null;
        private string _Operation_status_code = null;
        private int _Operation_display_sequence = 0;
        private string _Update_time = null;
        private string _Update_user = null;
        private string _LblOp_group1 = null;
        private string _LblOp_group2 = null;
        private string _LblOp_group3 = null;
        private string _LblOp_group4 = null;
        private string _LblOp_group5 = null;
        private string _Opr_Customized_Field1;
        private string _Opr_Customized_Field2;
        private string _Opr_Customized_Field3;
        private string _Opr_Customized_Field4;
        private string _Opr_Customized_Field5;
        private string _Opr_Customized_Field6;
        private string _Opr_Customized_Field7;
        private string _Opr_Customized_Field8;
        private string _Opr_Customized_Field9;
        private string _Opr_Customized_Field10;
        private string _Operation_Customized_Field1;
        private string _Operation_Customized_Field2;
        private string _Operation_Customized_Field3;
        private string _Operation_Customized_Field4;
        private string _Operation_Customized_Field5;
        private string _Operation_Customized_Field6;
        private string _Operation_Customized_Field7;
        private string _Operation_Customized_Field8;
        private string _Operation_Customized_Field9;
        private string _Operation_Customized_Field10;

        public string Factory { get { return _Factory; } set { _Factory = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string Short_description { get { return _Short_description; } set { _Short_description = value; } }
        public string Description { get { return _Description; } set { _Description = value; } }
        public string Operation_group1 { get { return _Operation_group1; } set { _Operation_group1 = value; } }
        public string Operation_group2 { get { return _Operation_group2; } set { _Operation_group2 = value; } }
        public string Operation_group3 { get { return _Operation_group3; } set { _Operation_group3 = value; } }
        public string Operation_group4 { get { return _Operation_group4; } set { _Operation_group4 = value; } }
        public string Operation_group5 { get { return _Operation_group5; } set { _Operation_group5 = value; } }
        public string Intransit_operation_yn { get { return _Intransit_operation_yn; } set { _Intransit_operation_yn = value; } }
        public string Shipping_operation_yn { get { return _Shipping_operation_yn; } set { _Shipping_operation_yn = value; } }
        public string Store_operation_yn { get { return _Store_operation_yn; } set { _Store_operation_yn = value; } }
        public string Start_require_operation_yn { get { return _Start_require_operation_yn; } set { _Start_require_operation_yn = value; } }
        public string End_operation_yn { get { return _End_operation_yn; } set { _End_operation_yn = value; } }
        public string Loss_tbl { get { return _Loss_tbl; } set { _Loss_tbl = value; } }
        public string Bonus_tbl { get { return _Bonus_tbl; } set { _Bonus_tbl = value; } }
        public string Rework_tbl { get { return _Rework_tbl; } set { _Rework_tbl = value; } }
        public string Line_id { get { return _Line_id; } set { _Line_id = value; } }
        public string Area_id { get { return _Area_id; } set { _Area_id = value; } }
        public string Operation_status_code { get { return _Operation_status_code; } set { _Operation_status_code = value; } }
        public int Operation_display_sequence { get { return _Operation_display_sequence; } set { _Operation_display_sequence = value; } }
        public string Update_time { get { return _Update_time; } set { _Update_time = value; } }
        public string Update_user { get { return _Update_user; } set { _Update_user = value; } }

        public string LblOp_group1
        {
            get
            {
                return _LblOp_group1;
            }

            set
            {
                _LblOp_group1 = value;
            }
        }

        public string LblOp_group2
        {
            get
            {
                return _LblOp_group2;
            }

            set
            {
                _LblOp_group2 = value;
            }
        }

        public string LblOp_group3
        {
            get
            {
                return _LblOp_group3;
            }

            set
            {
                _LblOp_group3 = value;
            }
        }

        public string LblOp_group4
        {
            get
            {
                return _LblOp_group4;
            }

            set
            {
                _LblOp_group4 = value;
            }
        }

        public string LblOp_group5
        {
            get
            {
                return _LblOp_group5;
            }

            set
            {
                _LblOp_group5 = value;
            }
        }

        public string Opr_Customized_Field1
        {
            get
            {
                return _Opr_Customized_Field1;
            }

            set
            {
                _Opr_Customized_Field1 = value;
            }
        }

        public string Opr_Customized_Field2
        {
            get
            {
                return _Opr_Customized_Field2;
            }

            set
            {
                _Opr_Customized_Field2 = value;
            }
        }

        public string Opr_Customized_Field3
        {
            get
            {
                return _Opr_Customized_Field3;
            }

            set
            {
                _Opr_Customized_Field3 = value;
            }
        }

        public string Opr_Customized_Field4
        {
            get
            {
                return _Opr_Customized_Field4;
            }

            set
            {
                _Opr_Customized_Field4 = value;
            }
        }

        public string Opr_Customized_Field5
        {
            get
            {
                return _Opr_Customized_Field5;
            }

            set
            {
                _Opr_Customized_Field5 = value;
            }
        }

        public string Opr_Customized_Field6
        {
            get
            {
                return _Opr_Customized_Field6;
            }

            set
            {
                _Opr_Customized_Field6 = value;
            }
        }

        public string Opr_Customized_Field7
        {
            get
            {
                return _Opr_Customized_Field7;
            }

            set
            {
                _Opr_Customized_Field7 = value;
            }
        }

        public string Opr_Customized_Field8
        {
            get
            {
                return _Opr_Customized_Field8;
            }

            set
            {
                _Opr_Customized_Field8 = value;
            }
        }

        public string Opr_Customized_Field9
        {
            get
            {
                return _Opr_Customized_Field9;
            }

            set
            {
                _Opr_Customized_Field9 = value;
            }
        }

        public string Opr_Customized_Field10
        {
            get
            {
                return _Opr_Customized_Field10;
            }

            set
            {
                _Opr_Customized_Field10 = value;
            }
        }

        public string Operation_Customized_Field1
        {
            get
            {
                return _Operation_Customized_Field1;
            }

            set
            {
                _Operation_Customized_Field1 = value;
            }
        }

        public string Operation_Customized_Field2
        {
            get
            {
                return _Operation_Customized_Field2;
            }

            set
            {
                _Operation_Customized_Field2 = value;
            }
        }

        public string Operation_Customized_Field3
        {
            get
            {
                return _Operation_Customized_Field3;
            }

            set
            {
                _Operation_Customized_Field3 = value;
            }
        }

        public string Operation_Customized_Field4
        {
            get
            {
                return _Operation_Customized_Field4;
            }

            set
            {
                _Operation_Customized_Field4 = value;
            }
        }

        public string Operation_Customized_Field5
        {
            get
            {
                return _Operation_Customized_Field5;
            }

            set
            {
                _Operation_Customized_Field5 = value;
            }
        }

        public string Operation_Customized_Field6
        {
            get
            {
                return _Operation_Customized_Field6;
            }

            set
            {
                _Operation_Customized_Field6 = value;
            }
        }

        public string Operation_Customized_Field7
        {
            get
            {
                return _Operation_Customized_Field7;
            }

            set
            {
                _Operation_Customized_Field7 = value;
            }
        }

        public string Operation_Customized_Field8
        {
            get
            {
                return _Operation_Customized_Field8;
            }

            set
            {
                _Operation_Customized_Field8 = value;
            }
        }

        public string Operation_Customized_Field9
        {
            get
            {
                return _Operation_Customized_Field9;
            }

            set
            {
                _Operation_Customized_Field9 = value;
            }
        }

        public string Operation_Customized_Field10
        {
            get
            {
                return _Operation_Customized_Field10;
            }

            set
            {
                _Operation_Customized_Field10 = value;
            }
        }

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(OperationArgsPack), this);
            return argsPack;
        }
    }
}
