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

    public class PartArgsPack
    {

        private ArgumentPack argsPack = new ArgumentPack();

        private string _FabId = null;
        private string _Region = null;
        private string _Line = null;
        private string _Area = null;
        private string _Bay = null;
        private string _MainEqp = null;
        private string _Unit = null;
        private string _Seq = null;
        private string _Isreused = null;
        private string _Isinstalled = null;
        private string _PartType = null;
        private string _Vendor = null;
        private string _Stockinlocation = null;
        private string _Grade = null;
        private string _PartsStatus = null;
        private string _PartSerialno = null;
        private int _RepairTimes = 0;
        private float _UnitPrice = 0.0f;
        private string _LastEvent = null;
        private EnumEventFlag _LastEventFlag = 0;
        private string _LastEventCode = null;
        private string _LastJobCode = null;
        private DateTime _ProductionDate = new DateTime();
        private DateTime _StockinTime = new DateTime();
        private DateTime _StockoutTime = new DateTime();
        private DateTime _InstalledTime = new DateTime();
        private DateTime _LastRepairoutTime = new DateTime();
        private DateTime _LastRepairinTime = new DateTime();
        private string _Isalive = null;
        private string _Modellevels = null;
        private string _ChangedDefaultInfolevel = null;
        private string _PartsName = null;
        private string _EQPartsName = null;
        private string _User = null;
        private string _PartModel = null;
        private bool _NeedRepair = false;
        private string _Name = null;
        private string _OAID = null;
        private string _Timekey = null;
        private string _InsertTime = null;
        private string _InsertUser = null;
        private string _UpdateTime = null;
        private string _UpdateUser = null;
        private string _LastEventTime = null;
        private DataTable _Dt = null;
        private DataTable _dtInvParts = new DataTable();
        private string _Eqp = null;
        private string _Tube = null;
        private string _IsRedo = null;
        private string _ReInstallTime = null;
        private string _ReInstallEquipment = null;
        private string _ReInstallTube = null;
        private string _UnInstallTime        = null;
        private string _UnInstallEquipment   = null;
        private string _UnInstallTube = null;
        private string _Comment = null;
        private string _IsCheck = null;
        private int _Price = 0;
        private string _DateTimeStart = null;
        private string _DateTimeEnd = null;

        public string Unit
        {
            get
            {
                return _Unit;
            }

            set
            {
                _Unit = value;
            }
        }

        public string Seq
        {
            get
            {
                return _Seq;
            }

            set
            {
                _Seq = value;
            }
        }

        public string Isreused
        {
            get
            {
                return _Isreused;
            }

            set
            {
                _Isreused = value;
            }
        }

        public string Isinstalled
        {
            get
            {
                return _Isinstalled;
            }

            set
            {
                _Isinstalled = value;
            }
        }

        public string PartType
        {
            get
            {
                return _PartType;
            }

            set
            {
                _PartType = value;
            }
        }

        public string Vendor
        {
            get
            {
                return _Vendor;
            }

            set
            {
                _Vendor = value;
            }
        }

        public string Stockinlocation
        {
            get
            {
                return _Stockinlocation;
            }

            set
            {
                _Stockinlocation = value;
            }
        }

        public string Grade
        {
            get
            {
                return _Grade;
            }

            set
            {
                _Grade = value;
            }
        }

        public string PartsStatus
        {
            get
            {
                return _PartsStatus;
            }

            set
            {
                _PartsStatus = value;
            }
        }

        public string PartSerialno
        {
            get
            {
                return _PartSerialno;
            }

            set
            {
                _PartSerialno = value;
            }
        }

        public int RepairTimes
        {
            get
            {
                return _RepairTimes;
            }

            set
            {
                _RepairTimes = value;
            }
        }

        public float UnitPrice
        {
            get
            {
                return _UnitPrice;
            }

            set
            {
                _UnitPrice = value;
            }
        }

        public string LastEvent
        {
            get
            {
                return _LastEvent;
            }

            set
            {
                _LastEvent = value;
            }
        }

        public EnumEventFlag LastEventFlag
        {
            get
            {
                return _LastEventFlag;
            }

            set
            {
                _LastEventFlag = value;
            }
        }

        public string LastEventCode
        {
            get
            {
                return _LastEventCode;
            }

            set
            {
                _LastEventCode = value;
            }
        }

        public string LastJobCode
        {
            get
            {
                return _LastJobCode;
            }

            set
            {
                _LastJobCode = value;
            }
        }

        public DateTime ProductionDate
        {
            get
            {
                return _ProductionDate;
            }

            set
            {
                _ProductionDate = value;
            }
        }

        public DateTime StockinTime
        {
            get
            {
                return _StockinTime;
            }

            set
            {
                _StockinTime = value;
            }
        }

        public DateTime StockoutTime
        {
            get
            {
                return _StockoutTime;
            }

            set
            {
                _StockoutTime = value;
            }
        }

        public DateTime InstalledTime
        {
            get
            {
                return _InstalledTime;
            }

            set
            {
                _InstalledTime = value;
            }
        }

        public DateTime LastRepairoutTime
        {
            get
            {
                return _LastRepairoutTime;
            }

            set
            {
                _LastRepairoutTime = value;
            }
        }

        public DateTime LastRepairinTime
        {
            get
            {
                return _LastRepairinTime;
            }

            set
            {
                _LastRepairinTime = value;
            }
        }

        public string Isalive
        {
            get
            {
                return _Isalive;
            }

            set
            {
                _Isalive = value;
            }
        }

        public string Modellevels
        {
            get
            {
                return _Modellevels;
            }

            set
            {
                _Modellevels = value;
            }
        }

        public string ChangedDefaultInfolevel
        {
            get
            {
                return _ChangedDefaultInfolevel;
            }

            set
            {
                _ChangedDefaultInfolevel = value;
            }
        }

        public string PartsName
        {
            get
            {
                return _PartsName;
            }

            set
            {
                _PartsName = value;
            }
        }

        public string EQPartsName
        {
            get
            {
                return _EQPartsName;
            }

            set
            {
                _EQPartsName = value;
            }
        }

        public string FabId
        {
            get
            {
                return _FabId;
            }

            set
            {
                _FabId = value;
            }
        }

        public string Region
        {
            get
            {
                return _Region;
            }

            set
            {
                _Region = value;
            }
        }

        public string User
        {
            get
            {
                return _User;
            }

            set
            {
                _User = value;
            }
        }

        public string PartModel
        {
            get
            {
                return _PartModel;
            }

            set
            {
                _PartModel = value;
            }
        }

        public bool NeedRepair
        {
            get
            {
                return _NeedRepair;
            }

            set
            {
                _NeedRepair = value;
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

        public string Timekey
        {
            get
            {
                return _Timekey;
            }

            set
            {
                _Timekey = value;
            }
        }

        public string InsertTime
        {
            get
            {
                return _InsertTime;
            }

            set
            {
                _InsertTime = value;
            }
        }

        public string InsertUser
        {
            get
            {
                return _InsertUser;
            }

            set
            {
                _InsertUser = value;
            }
        }

        public string UpdateTime
        {
            get
            {
                return _UpdateTime;
            }

            set
            {
                _UpdateTime = value;
            }
        }

        public string UpdateUser
        {
            get
            {
                return _UpdateUser;
            }

            set
            {
                _UpdateUser = value;
            }
        }

        public string LastEventTime
        {
            get
            {
                return _LastEventTime;
            }

            set
            {
                _LastEventTime = value;
            }
        }

        public DataTable Dt
        {
            get
            {
                return _Dt;
            }

            set
            {
                _Dt = value;
            }
        }

        public string Eqp
        {
            get
            {
                return _Eqp;
            }

            set
            {
                _Eqp = value;
            }
        }

        public string Tube
        {
            get
            {
                return _Tube;
            }

            set
            {
                _Tube = value;
            }
        }

        public string IsRedo
        {
            get
            {
                return _IsRedo;
            }

            set
            {
                _IsRedo = value;
            }
        }

        public string ReInstallTime
        {
            get
            {
                return _ReInstallTime;
            }

            set
            {
                _ReInstallTime = value;
            }
        }

        public string ReInstallEquipment
        {
            get
            {
                return _ReInstallEquipment;
            }

            set
            {
                _ReInstallEquipment = value;
            }
        }

        public string ReInstallTube
        {
            get
            {
                return _ReInstallTube;
            }

            set
            {
                _ReInstallTube = value;
            }
        }

        public string Line
        {
            get
            {
                return _Line;
            }

            set
            {
                _Line = value;
            }
        }

        public string Area
        {
            get
            {
                return _Area;
            }

            set
            {
                _Area = value;
            }
        }

        public string Bay
        {
            get
            {
                return _Bay;
            }

            set
            {
                _Bay = value;
            }
        }

        public string MainEqp
        {
            get
            {
                return _MainEqp;
            }

            set
            {
                _MainEqp = value;
            }
        }

        public DataTable DtInvParts
        {
            get
            {
                return _dtInvParts;
            }

            set
            {
                _dtInvParts = value;
            }
        }

        public string UnInstallTime
        {
            get
            {
                return _UnInstallTime;
            }

            set
            {
                _UnInstallTime = value;
            }
        }

        public string UnInstallEquipment
        {
            get
            {
                return _UnInstallEquipment;
            }

            set
            {
                _UnInstallEquipment = value;
            }
        }

        public string UnInstallTube
        {
            get
            {
                return _UnInstallTube;
            }

            set
            {
                _UnInstallTube = value;
            }
        }

        public int Price
        {
            get
            {
                return _Price;
            }

            set
            {
                _Price = value;
            }
        }

        public string Comment
        {
            get
            {
                return _Comment;
            }

            set
            {
                _Comment = value;
            }
        }

        public string IsCheck
        {
            get
            {
                return _IsCheck;
            }

            set
            {
                _IsCheck = value;
            }
        }

        public string DateTimeStart
        {
            get
            {
                return _DateTimeStart;
            }

            set
            {
                _DateTimeStart = value;
            }
        }

        public string DateTimeEnd
        {
            get
            {
                return _DateTimeEnd;
            }

            set
            {
                _DateTimeEnd = value;
            }
        }

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(PartArgsPack), this);
            return argsPack;
        }
    }   
}