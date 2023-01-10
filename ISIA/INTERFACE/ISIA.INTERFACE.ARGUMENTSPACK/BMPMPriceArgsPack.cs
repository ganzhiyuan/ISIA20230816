using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TAP;
using TAP.Models;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class BMPMPriceArgsPack
    {
        private ArgumentPack argsPack = new ArgumentPack();

        private string _REGION;
        private string _FACILITY;
        //private string _AERA;
        private string _Workshop;
        private string _Process;
        private string _INSERTUSER;
        private string _Report_Date;
        private string _Report_SatrtDate;
        private string _Report_EndDate;
        private string _Report_Satrt;
        private string _Report_End;
        private string _NAME;

        private string _MAINEQUIPMENT;
        private string _EQUIPMENT;
        private string _PRICE;
        private string _QTY;
        private string _BMSTARTTIME;
        private string _BMCODE;
        private string _COUNT;
        private string _MATERIAL;
        private string _MATERIALS;
        private string _MATERIAL2;
        private string _MATERIALS2;
        private string _COUNT2;
        private string _Report_Date2;
        private string _PMSTARTTIME;
        private string _INSERTUSER2;
        private string _PRICE2;
        private string _QTY2;


        private string _USERID;
        private string _USERNAME;
        private string _TOTAL_BM_TIME;
        private string _BM_CNT;
        private string _AVG_BM_TIME;
        private string _PEOPLE_1;
        private string _PEOPLE_2;
        private string _PEOPLE_3;
        private string _PMTIME;
        private string _PM_CNT;
        private string _TOTAL_MATERIALS;
        private string _TOTAL_MATERIALS_PRICE;
        private string _PARAMETER_VALUE;

        public string USERID
        {
            get
            {
                return _USERID;
            }

            set
            {
                _USERID = value;
            }
        }
        public string PARAMETER_VALUE
        {
            get
            {
                return _PARAMETER_VALUE;
            }

            set
            {
                _PARAMETER_VALUE = value;
            }
        }
        public string USERNAME
        {
            get
            {
                return _USERNAME;
            }

            set
            {
                _USERNAME = value;
            }
        }
        public string TOTAL_BM_TIME
        {
            get
            {
                return _TOTAL_BM_TIME;
            }

            set
            {
                _TOTAL_BM_TIME = value;
            }
        }
        public string BM_CNT
        {
            get
            {
                return _BM_CNT;
            }

            set
            {
                _BM_CNT = value;
            }
        }
        public string AVG_BM_TIME
        {
            get
            {
                return _AVG_BM_TIME;
            }

            set
            {
                _AVG_BM_TIME = value;
            }
        }
        public string PEOPLE_1
        {
            get
            {
                return _PEOPLE_1;
            }

            set
            {
                _PEOPLE_1 = value;
            }
        }
        public string PEOPLE_2
        {
            get
            {
                return _PEOPLE_2;
            }

            set
            {
                _PEOPLE_2 = value;
            }
        }
        public string PEOPLE_3
        {
            get
            {
                return _PEOPLE_3;
            }

            set
            {
                _PEOPLE_3 = value;
            }
        }
        public string PMTIME
        {
            get
            {
                return _PMTIME;
            }

            set
            {
                _PMTIME = value;
            }
        }

        public string PM_CNT
        {
            get
            {
                return _PM_CNT;
            }

            set
            {
                _PM_CNT = value;
            }
        }
        public string TOTAL_MATERIALS
        {
            get
            {
                return _TOTAL_MATERIALS;
            }

            set
            {
                _TOTAL_MATERIALS = value;
            }
        }
        public string TOTAL_MATERIALS_PRICE
        {
            get
            {
                return _TOTAL_MATERIALS_PRICE;
            }

            set
            {
                _TOTAL_MATERIALS_PRICE = value;
            }
        }
        public string REGION
        {
            get
            {
                return _REGION;
            }

            set
            {
                _REGION = value;
            }
        }
        public string Workshop
        {
            get
            {
                return _Workshop;
            }

            set
            {
                _Workshop = value;
            }
        }


        public string Report_Date
        {
            get
            {
                return _Report_Date;
            }

            set
            {
                _Report_Date = value;
            }
        }
        public string Report_Date2
        {
            get
            {
                return _Report_Date2;
            }

            set
            {
                _Report_Date2 = value;
            }
        }
        public string PRICRE
        {
            get
            {
                return _PRICE;
            }

            set
            {
                _PRICE = value;
            }
        }
        public string PRICRE2
        {
            get
            {
                return _PRICE2;
            }

            set
            {
                _PRICE2 = value;
            }
        }
        public string FACILITY
        {
            get
            {
                return _FACILITY;
            }

            set
            {
                _FACILITY = value;
            }
        }

        public string Process_Type
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

        public string INSERTUSER
        {
            get
            {
                return _INSERTUSER;
            }

            set
            {
                _INSERTUSER = value;
            }
        }
        public string INSERTUSER2
        {
            get
            {
                return _INSERTUSER2;
            }

            set
            {
                _INSERTUSER2 = value;
            }
        }



        public string Report_SatrtDate
        {
            get
            {
                return _Report_SatrtDate;
            }

            set
            {
                _Report_SatrtDate = value;
            }
        }
        public string Report_Satrt
        {
            get
            {
                return _Report_Satrt;
            }

            set
            {
                _Report_Satrt = value;
            }
        }

        public string Report_EndDate
        {
            get
            {
                return _Report_EndDate;
            }

            set
            {
                _Report_EndDate = value;
            }
        }
        public string Report_End
        {
            get
            {
                return _Report_End;
            }

            set
            {
                _Report_End = value;
            }
        }
        public string NAME
        {
            get
            {
                return _NAME;
            }

            set
            {
                _NAME = value;
            }
        }
        public string MAINEQUIPMENT
        {
            get
            {
                return _MAINEQUIPMENT;
            }

            set
            {
                _MAINEQUIPMENT = value;
            }
        }
        public string EQUIPMENT
        {
            get
            {
                return _EQUIPMENT;
            }

            set
            {
                _EQUIPMENT = value;
            }
        }
        public string QTY
        {
            get
            {
                return _QTY;
            }

            set
            {
                _QTY = value;
            }
        }
        public string QTY2
        {
            get
            {
                return _QTY2;
            }

            set
            {
                _QTY2 = value;
            }
        }
        public string BMSTARTTIME
        {
            get
            {
                return _BMSTARTTIME;
            }

            set
            {
                _BMSTARTTIME = value;
            }
        }
        public string PMSTARTTIME
        {
            get
            {
                return _PMSTARTTIME;
            }

            set
            {
                _PMSTARTTIME = value;
            }
        }
        public string BMCODE
        {
            get
            {
                return _BMCODE;
            }

            set
            {
                _BMCODE = value;
            }
        }
        public string COUNT
        {
            get
            {
                return _COUNT;
            }

            set
            {
                _COUNT = value;
            }
        }
        public string COUNT2
        {
            get
            {
                return _COUNT2;
            }

            set
            {
                _COUNT2 = value;
            }
        }
        public string MATERIAL
        {
            get
            {
                return _MATERIAL;
            }

            set
            {
                _MATERIAL = value;
            }
        }
        public string MATERIAL2
        {
            get
            {
                return _MATERIAL2;
            }

            set
            {
                _MATERIAL2 = value;
            }
        }
        public string MATERIALS
        {
            get
            {
                return _MATERIALS;
            }

            set
            {
                _MATERIALS = value;
            }
        }
        public string MATERIALS2
        {
            get
            {
                return _MATERIALS2;
            }

            set
            {
                _MATERIALS2 = value;
            }
        }
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(BMPMPriceArgsPack), this);
            return argsPack;
        }

    }
}
