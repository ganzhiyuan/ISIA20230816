using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class WorkOrderArgsPack
    {
        #region Field
        private string _WorkShop_Id= null;
        private string _WorkOrder_Id= null;
        private string _Wafer_Id= null;
        private string _Prod_Id= null;
        private string _Date_Start = null;
        private string _Date_End = null;
        private string _Finish_Date_Start = null;
        private string _Finish_Date_End  = null;
        private string _Lama_Id  = null;

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

        public string Date_Start
        {
            get
            {
                return _Date_Start;
            }

            set
            {
                _Date_Start = value;
            }
        }

        public string Date_End
        {
            get
            {
                return _Date_End;
            }

            set
            {
                _Date_End = value;
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

        public string Lama_Id
        {
            get
            {
                return _Lama_Id;
            }

            set
            {
                _Lama_Id = value;
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



        #endregion

        #region method
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(WorkOrderArgsPack), this);
            return argsPack;
        }
        #endregion
    }
}