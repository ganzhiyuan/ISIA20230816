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

    public class PartsRepairHistoryArgsPack
    {
        private List<PartsRepairHistoryArgsPack> _mRHArgsPacks = new List<PartsRepairHistoryArgsPack>();
        private ArgumentPack argsPack = new ArgumentPack();
        private string _REGION; 
        private string _FACILITY;
        private string _PARTTYPE;
        private string _PART; 
        private string _VENDOR;
        private string _UNINSTALLTIME_begin;
        private string _UNINSTALLTIME_end;
        private string _UNINSTALLEQUIPMENT;
        private string _UNINSTALLTUBE;
        private string _UNINSTALLREASON;
        private string _REPAIRSHIPPINGTIME_begin;
        private string _REPAIRSHIPPINGTIME_end;
        private string _REPAIRRETURNTIME_begin;
        private string _REPAIRRETURNTIME_end;
        private string _REINSTALLTIME_begin;
        private string _REINSTALLTIME_end;
        private string _REINSTALLEQUIPMENT;
        private string _REINSTALLTUBE;
        private string _OAID;
        private string _ISREDO;
        private string _PRICE;
        private string _ISCHECK;
        private string _COMMENT;
        private string _PARTSERIALNO;


        public string UNINSTALLTIME 
        {
            get
            {
                return _UNINSTALLTIME_begin;
            }

            set
            {
                _UNINSTALLTIME_begin = value;
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

        public string PART
        {
            get
            {
                return _PART;
            }

            set
            {
                _PART = value;
            }
        }

        public string VENDOR
        {
            get
            {
                return _VENDOR;
            }

            set
            {
                _VENDOR = value;
            }
        }

        public string UNINSTALLEQUIPMENT
        {
            get
            {
                return _UNINSTALLEQUIPMENT;
            }

            set
            {
                _UNINSTALLEQUIPMENT = value;
            }
        }

        public string UNINSTALLTUBE
        {
            get
            {
                return _UNINSTALLTUBE;
            }

            set
            {
                _UNINSTALLTUBE = value;
            }
        }

        public string UNINSTALLREASON
        {
            get
            {
                return _UNINSTALLREASON;
            }

            set
            {
                _UNINSTALLREASON = value;
            }
        }

        public string REPAIRSHIPPINGTIME_begin
        {
            get
            {
                return _REPAIRSHIPPINGTIME_begin;
            }

            set
            {
                _REPAIRSHIPPINGTIME_begin = value;
            }
        }

        public string REPAIRRETURNTIME_begin
        {
            get
            {
                return _REPAIRRETURNTIME_begin;
            }

            set
            {
                _REPAIRRETURNTIME_begin = value;
            }
        }

        public string REINSTALLTIME
        {
            get
            {
                return _REINSTALLTIME_begin;
            }

            set
            {
                _REINSTALLTIME_begin = value;
            }
        }

        public string REINSTALLEQUIPMENT
        {
            get
            {
                return _REINSTALLEQUIPMENT;
            }

            set
            {
                _REINSTALLEQUIPMENT = value;
            }
        }

        public string REINSTALLTUBE
        {
            get
            {
                return _REINSTALLTUBE;
            }

            set
            {
                _REINSTALLTUBE = value;
            }
        }

        public string OAID
        {
            get
            {
                return _OAID;
            }

            set
            {
                _OAID = value;
            }
        }

        public string ISREDO
        {
            get
            {
                return _ISREDO;
            }

            set
            {
                _ISREDO = value;
            }
        }

        public string PRICE
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

        public string ISCHECK
        {
            get
            {
                return _ISCHECK;
            }

            set
            {
                _ISCHECK = value;
            }
        }

        public string COMMENT
        {
            get
            {
                return _COMMENT;
            }

            set
            {
                _COMMENT = value;
            }
        }

        public List<PartsRepairHistoryArgsPack> MRHArgsPacks
        {
            get
            {
                return _mRHArgsPacks;
            }

            set
            {
                _mRHArgsPacks = value;
            }
        }

        public string UNINSTALLTIME_end
        {
            get
            {
                return _UNINSTALLTIME_end;
            }

            set
            {
                _UNINSTALLTIME_end = value;
            }
        }

        public string REPAIRSHIPPINGTIME_end
        {
            get
            {
                return _REPAIRSHIPPINGTIME_end;
            }

            set
            {
                _REPAIRSHIPPINGTIME_end = value;
            }
        }

        public string REPAIRRETURNTIME_end
        {
            get
            {
                return _REPAIRRETURNTIME_end;
            }

            set
            {
                _REPAIRRETURNTIME_end = value;
            }
        }

        public string REINSTALLTIME_end
        {
            get
            {
                return _REINSTALLTIME_end;
            }

            set
            {
                _REINSTALLTIME_end = value;
            }
        }

        public string PARTSERIALNO
        {
            get
            {
                return _PARTSERIALNO;
            }

            set
            {
                _PARTSERIALNO = value;
            }
        }

        public string PARTTYPE
        {
            get
            {
                return _PARTTYPE;
            }

            set
            {
                _PARTTYPE = value;
            }
        }

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(PartsRepairHistoryArgsPack), this);
            return argsPack;
        }
    }
}