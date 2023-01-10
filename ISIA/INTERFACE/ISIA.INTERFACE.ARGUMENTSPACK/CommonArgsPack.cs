using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class CommonArgsPack
    {
        #region Field
        private string _GroupName = null;
        private string _Region = null;
        private string _Facility = null;
        private string _Name = null;
        private string _Description = null;
        private string _InsertTime = null;
        private string _InsertUser = null;
        private string _IsAlive = null;
        private string _UserName = null;
        private string _UserID = null;
        private string _Department = null;
        private string _Position = null;
        private string _Line = "";
        private string _Area = null;
        private string _Bay = null;
        private string _MainEqp = null;
        private string _Eqp = null;
        private string _EqpGroup = null;
        private string _UpdateTime = null;
        private string _UpdateUser = null;
        private DataTable _dt = new DataTable();
        private DataTable _dtPMItem = new DataTable();
        private string _LastEvent = null;
        private string _LastEventTime = null;
        private string _LastEventCode = null;
        private string _LastJobCode = null;
        private string _InstalledTime = null;
        private DataTable _dtInvParts = new DataTable();
        private string _PartType = null;
        private string _Vendor = null;
        private string _PartModel = null;
        private string _StockInLocation = null;
        private string _Grade = null;
        private float _UnitPrice = 0;
        private string _ProductionDate = null;
        private string _StockInTime = null;
        private string _StockOutTime = null;
        private string _MDI = null;
        private int _Sequence =0;
        private string _UserLastName = null;
        private string _CurrentModel = null;
        private string _LastEventComment = null;
        private string _LastEventFlag = null;
        private string _Sequences;
        private string _Modellevels;
        private string _Language;
        private uint _Levels;
        private string _UserMiddleName;
        private string _Contactno;
        private string _Mobileno;
        private string _MailAddress;
        private string _Password;
        private string _UserGroupName;
        private string __EquipmentType;
        private string __EqpTestType;
        private string _DateTimeStart = null;
        private string _DateTimeEnd = null;
        private string _TimeKey = null;
        private string _TestType = null;
        private string _MessageType = null;
        private string _MessageName = null;
        private string _Custom01 = null;
        private string _Custom02 = null;
        private string _Custom03 = null;
        private string _Custom04 = null;
        private string _Custom05 = null;
        private string _Custom06 = null;
        private string _Custom07 = null;
        private string _Custom08 = null;
        private string _Custom09 = null;
        private string _Shift = "";
        private string _Production = "";
        private List<FmbArgsPack> _eqps;
        private Dictionary<string, string> _EQNames;



        private TAP.Models.User.EnumAuthorityOwnerType _MemberType;
        private string _PartName = null;

        private ArgumentPack argsPack = new ArgumentPack();
        #endregion
        #region Properties
        public string GroupName { get { return _GroupName; } set { _GroupName = value; } }
        public string Region { get { return _Region; } set { _Region = value; } }
        public string Facility { get { return _Facility; } set { _Facility = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string Description { get { return _Description; } set { _Description = value; } }
        public string InsertTime { get { return _InsertTime; } set { _InsertTime = value; } }
        public string InsertUser { get { return _InsertUser; } set { _InsertUser = value; } }
        public string IsAlive { get { return _IsAlive; } set { _IsAlive = value; } }
        public string UserName { get { return _UserName; } set { _UserName = value; } }
        public string UserID { get { return _UserID; } set { _UserID = value; } }
        public string Department { get { return _Department; } set { _Department = value; } }
        public string Position { get { return _Position; } set { _Position = value; } }
        public string Line { get { return _Line; } set { _Line = value; } }
        public string Area { get { return _Area; } set { _Area = value; } }
        public string Bay { get { return _Bay; } set { _Bay = value; } }
        public string MainEqp { get { return _MainEqp; } set { _MainEqp = value; } }
        public string Eqp { get { return _Eqp; } set { _Eqp = value; } }
        public string EqpGroup { get { return _EqpGroup; } set { _EqpGroup = value; } }
        public string UpdateTime { get { return _UpdateTime; } set { _UpdateTime = value; } }
        public string UpdateUser { get { return _UpdateUser; } set { _UpdateUser = value; } }
        public DataTable Dt { get { return _dt; } set { _dt = value; } }
        public DataTable DtPMItem { get { return _dtPMItem; } set { _dtPMItem = value; } }
        public string LastEvent { get { return _LastEvent; } set { _LastEvent = value; } }
        public string LastEventTime { get { return _LastEventTime; } set { _LastEventTime = value; } }
        public string LastEventCode { get { return _LastEventCode; } set { _LastEventCode = value; } }
        public string LastJobCode { get { return _LastJobCode; } set { _LastJobCode = value; } }
        public string InstalledTime { get { return _InstalledTime; } set { _InstalledTime = value; } }
        public DataTable DtInvParts { get { return _dtInvParts; } set { _dtInvParts = value; } }
        public string PartType { get { return _PartType; } set { _PartType = value; } }
        public string Vendor { get { return _Vendor; } set { _Vendor = value; } }
        public string PartModel { get { return _PartModel; } set { _PartModel = value; } }
        public string StockInLocation { get { return _StockInLocation; } set { _StockInLocation = value; } }
        public string Grade { get { return _Grade; } set { _Grade = value; } }
        public float UnitPrice { get { return _UnitPrice; } set { _UnitPrice = value; } }
        public string ProductionDate { get { return _ProductionDate; } set { _ProductionDate = value; } }
        public string StockInTime { get { return _StockInTime; } set { _StockInTime = value; } }
        public string StockOutTime { get { return _StockOutTime; } set { _StockOutTime = value; } }
        public string MDI { get { return _MDI; } set { _MDI = value; } }
        public TAP.Models.User.EnumAuthorityOwnerType MemberType { get { return _MemberType; } set { _MemberType = value; } }
        public int Sequence { get { return _Sequence; } set { _Sequence = value; } }
        public string PartName { get { return _PartName; } set { _PartName = value; } }
        public string Shift { get { return _Shift; } set { _Shift = value; } }
        public string Production { get { return _Production; } set { _Production = value; } }

        public string UserLastName
        {
            get
            {
                return _UserLastName;
            }

            set
            {
                _UserLastName = value;
            }
        }

        public string CurrentModel
        {
            get
            {
                return _CurrentModel;
            }

            set
            {
                _CurrentModel = value;
            }
        }

        public string LastEventComment
        {
            get
            {
                return _LastEventComment;
            }

            set
            {
                _LastEventComment = value;
            }
        }

        public string LastEventFlag
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

        public string Sequences
        {
            get
            {
                return _Sequences;
            }

            set
            {
                _Sequences = value;
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

        public string Language
        {
            get
            {
                return _Language;
            }

            set
            {
                _Language = value;
            }
        }

        public uint Levels
        {
            get
            {
                return _Levels;
            }

            set
            {
                _Levels = value;
            }
        }

        public string UserMiddleName
        {
            get
            {
                return _UserMiddleName;
            }

            set
            {
                _UserMiddleName = value;
            }
        }

        public string Contactno
        {
            get
            {
                return _Contactno;
            }

            set
            {
                _Contactno = value;
            }
        }

        public string Mobileno
        {
            get
            {
                return _Mobileno;
            }

            set
            {
                _Mobileno = value;
            }
        }

        public string MailAddress
        {
            get
            {
                return _MailAddress;
            }

            set
            {
                _MailAddress = value;
            }
        }

        public string Password
        {
            get
            {
                return _Password;
            }

            set
            {
                _Password = value;
            }
        }

        public string UserGroupName
        {
            get
            {
                return _UserGroupName;
            }

            set
            {
                _UserGroupName = value;
            }
        }

        public string EquipmentType
        {
            get
            {
                return __EquipmentType;
            }

            set
            {
                __EquipmentType = value;
            }
        }

        public string EqpTestType
        {
            get
            {
                return __EqpTestType;
            }

            set
            {
                __EqpTestType = value;
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

        public string TimeKey
        {
            get
            {
                return _TimeKey;
            }

            set
            {
                _TimeKey = value;
            }
        }

        public string TestType
        {
            get
            {
                return _TestType;
            }

            set
            {
                _TestType = value;
            }
        }

        public string Custom09
        {
            get
            {
                return _Custom09;
            }

            set
            {
                _Custom09 = value;
            }
        }

        public string MessageType
        {
            get
            {
                return _MessageType;
            }

            set
            {
                _MessageType = value;
            }
        }

        public string MessageName
        {
            get
            {
                return _MessageName;
            }

            set
            {
                _MessageName = value;
            }
        }

        public string Custom01
        {
            get
            {
                return _Custom01;
            }

            set
            {
                _Custom01 = value;
            }
        }

        public string Custom02
        {
            get
            {
                return _Custom02;
            }

            set
            {
                _Custom02 = value;
            }
        }

        public string Custom03
        {
            get
            {
                return _Custom03;
            }

            set
            {
                _Custom03 = value;
            }
        }

        public string Custom04
        {
            get
            {
                return _Custom04;
            }

            set
            {
                _Custom04 = value;
            }
        }

        public string Custom05
        {
            get
            {
                return _Custom05;
            }

            set
            {
                _Custom05 = value;
            }
        }

        public string Custom06
        {
            get
            {
                return _Custom06;
            }

            set
            {
                _Custom06 = value;
            }
        }

        public string Custom07
        {
            get
            {
                return _Custom07;
            }

            set
            {
                _Custom07 = value;
            }
        }

        public string Custom08
        {
            get
            {
                return _Custom08;
            }

            set
            {
                _Custom08 = value;
            }
        }

    
        public List<FmbArgsPack> Eqps
        {
            get
            {
                return _eqps;
            }

            set
            {
                _eqps = value;
            }
        }

        public Dictionary<string, string> EQNames
        {
            get
            {
                return _EQNames;
            }

            set
            {
                _EQNames = value;
            }
        }



        #endregion
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(CommonArgsPack), this);
            return argsPack;
        }
    }
}
