using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class WaferArgsPack
    {
        #region Field
        private string _WorkShop_Id= null;
        private string _WorkOrder_Id= null;
        private string _Wafer_Id= null;
        private string _Wafer_Status = null;
        private string _Equipment_Id = null;
        private string _Oper_Id = null;
        private string _Last_Transaction_Time = null;
        private string _Prod_Id= null;
        private string _Start_Time = null;
        private string _End_Time = null;
        private string _Finish_Date_Start = null;
        private string _Finish_Date_End  = null;
        private string _Last_Transaction_Code = null;
        private string _Start_Process = null;
        private string _End_Process = null;

        private ArgumentPack argsPack = new ArgumentPack();
        #endregion

        #region Properties
        public string WorkShop_Id
        {
            get
            {
                return _WorkShop_Id;
            }

            set
            {
                _WorkShop_Id = value;
            }
        }

        public string Prod_Id
        {
            get
            {
                return _Prod_Id;
            }

            set
            {
                _Prod_Id = value;
            }
        }

        public string Finish_Date_Start
        {
            get
            {
                return _Finish_Date_Start;
            }

            set
            {
                _Finish_Date_Start = value;
            }
        }

        public string Finish_Date_End
        {
            get
            {
                return _Finish_Date_End;
            }

            set
            {
                _Finish_Date_End = value;
            }
        }

        public string WorkOrder_Id
        {
            get
            {
                return _WorkOrder_Id;
            }

            set
            {
                _WorkOrder_Id = value;
            }
        }

        public string Wafer_Id
        {
            get
            {
                return _Wafer_Id;
            }

            set
            {
                _Wafer_Id = value;
            }
        }

        public string Wafer_Status
        {
            get
            {
                return _Wafer_Status;
            }

            set
            {
                _Wafer_Status = value;
            }
        }

        public string Equipment_Id
        {
            get
            {
                return _Equipment_Id;
            }

            set
            {
                _Equipment_Id = value;
            }
        }

        public string Oper_Id
        {
            get
            {
                return _Oper_Id;
            }

            set
            {
                _Oper_Id = value;
            }
        }

        public string Last_Transaction_Time
        {
            get
            {
                return _Last_Transaction_Time;
            }

            set
            {
                _Last_Transaction_Time = value;
            }
        }

        public string Start_Time
        {
            get
            {
                return _Start_Time;
            }

            set
            {
                _Start_Time = value;
            }
        }

        public string End_Time
        {
            get
            {
                return _End_Time;
            }

            set
            {
                _End_Time = value;
            }
        }

        public string Last_Transaction_Code
        {
            get
            {
                return _Last_Transaction_Code;
            }

            set
            {
                _Last_Transaction_Code = value;
            }
        }

        public string Start_Process
        {
            get
            {
                return _Start_Process;
            }

            set
            {
                _Start_Process = value;
            }
        }

        public string End_Process
        {
            get
            {
                return _End_Process;
            }

            set
            {
                _End_Process = value;
            }
        }



        #endregion

        #region method
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(WaferArgsPack), this);
            return argsPack;
        }
        #endregion
    }
}