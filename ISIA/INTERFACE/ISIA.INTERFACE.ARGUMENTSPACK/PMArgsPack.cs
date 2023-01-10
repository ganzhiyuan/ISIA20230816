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
    public class PMArgsPack
    {
        private ArgumentPack argsPack = new ArgumentPack();

        private string _FabId = null;
        private string _DateTimeStart = null;
        private string _DateTimeEnd = null;
        private string _Region = null;
        private string _Line = null;
        private string _Bay = null;
        private string _Area = null;
        private string _MainEquipment = null;
        private string _Equipment = null;
        private string _PMSchedule = null;
        private string _PMType = null;
        private string _PMItem = null;
        private string _Worker = null;
        private string _ResultYN = null;
        private string _ResultParts = null;
        private double _ResultNum = 0;
        private string _PartModel = null;
        private string _Description = null;
        private string _LasteventComment = null;
        private string _Comment = null;
        private string _TimeKey = null;
        private string _InsertTime = null;
        private string _InsertUser = null;
        private string _NextDPMTime = null;
        private string _EquipmentStatus = null;
        private string _IsComplete = null;
        private string _PMCategory = null;
        private string _PMMethod = null;
        private string _Unit = null;
        private string _Specul = null;
        private string _Specll = null;
        private string _UpdateTime = null;
        private string _UpdateUser = null;
        private string _EquipmentGroup = null;
        private string _OldPMSchedule = null;
        private string _IsGroup = null;
        private string _PMTime = null;
        private string _EquipmentsModel = null;
        private string _File_Name = null;
        private string _Process = null;
        private string _Seq = null;
        private string _Tolerance = null;
        private DateTime _LastEventTime = new DateTime();
        private DataTable _dt = new DataTable();
        private DataTable _dtPMGroupMember = new DataTable();
        private DataTable _dtFullYearDate = new DataTable();

        private string _EquipmentType = null;
        private string _PMStartTime = null;
        private string _StartTime = null;
        private string _EndTime = null;
        private string _User = null;

        private PartArgsPack _NewPart = null;
        private PartArgsPack _OldPart = null;




        public string FabId { get { return _FabId; } set { _FabId = value; } }
        public string Region { get { return _Region; } set { _Region = value; } }
        public string Line { get { return _Line; } set { _Line = value; } }
        public string Area { get { return _Area; } set { _Area = value; } }
        public string MainEquipment { get { return _MainEquipment; } set { _MainEquipment = value; } }
        public string Equipment { get { return _Equipment; } set { _Equipment = value; } }
        public string PMSchedule { get { return _PMSchedule; } set { _PMSchedule = value; } }
        public string PMType { get { return _PMType; } set { _PMType = value; } }
        public string PMItem { get { return _PMItem; } set { _PMItem = value; } }
        public string Worker { get { return _Worker; } set { _Worker = value; } }
        public string ResultYN { get { return _ResultYN; } set { _ResultYN = value; } }
        public string ResultParts { get { return _ResultParts; } set { _ResultParts = value; } }
        public double ResultNum { get { return _ResultNum; } set { _ResultNum = value; } }
        public string PartModel { get { return _PartModel; } set { _PartModel = value; } }
        public string Description { get { return _Description; } set { _Description = value; } }
        public string LasteventComment { get { return _LasteventComment; } set { _LasteventComment = value; } }
        public string Comment { get { return _Comment; } set { _Comment = value; } }
        public string DateTimeStart { get { return _DateTimeStart; } set { _DateTimeStart = value; } }
        public string DateTimeEnd { get { return _DateTimeEnd; } set { _DateTimeEnd = value; } }
        public string Bay { get { return _Bay; } set { _Bay = value; } }
        public string TimeKey { get { return _TimeKey; } set { _TimeKey = value; } }
        public string InsertTime { get { return _InsertTime; } set { _InsertTime = value; } }
        public string InsertUser { get { return _InsertUser; } set { _InsertUser = value; } }
        public string NextDPMTime { get { return _NextDPMTime; } set { _NextDPMTime = value; } }
        public string EquipmentStatus { get { return _EquipmentStatus; } set { _EquipmentStatus = value; } }
        public string IsComplete { get { return _IsComplete; } set { _IsComplete = value; } }
        public string PMCategory { get { return _PMCategory; } set { _PMCategory = value; } }
        public string PMMethod { get { return _PMMethod; } set { _PMMethod = value; } }
        public string Unit { get { return _Unit; } set { _Unit = value; } }
        public string UpdateTime { get { return _UpdateTime; } set { _UpdateTime = value; } }
        public string UpdateUser { get { return _UpdateUser; } set { _UpdateUser = value; } }
        public string EquipmentGroup { get { return _EquipmentGroup; } set { _EquipmentGroup = value; } }
        public string OldPMSchedule { get { return _OldPMSchedule; } set { _OldPMSchedule = value; } }
        public string IsGroup { get { return _IsGroup; } set { _IsGroup = value; } }
        public string PMTime { get { return _PMTime; } set { _PMTime = value; } }
        public DataTable Dt { get { return _dt; } set { _dt = value; } }
        public DataTable DtPMGroupMember { get { return _dtPMGroupMember; } set { _dtPMGroupMember = value; } }
        public DataTable DtFullYearDate { get { return _dtFullYearDate; } set { _dtFullYearDate = value; } }
        public string Process { get { return _Process; } set {_Process = value; } }
        public DateTime LastEventTime { get { return _LastEventTime; } set {  _LastEventTime = value;    }  }
        public string Seq {get { return _Seq; } set { _Seq = value;  }}
        public string Specul {get  {  return _Specul;  } set   {  _Specul = value; } }
        public string Specll{  get{ return _Specll;}  set  {    _Specll = value;  }  }
        public string EquipmentsModel { get { return _EquipmentsModel; } set { _EquipmentsModel = value; } }

        public string File_Name { get { return _File_Name; } set { _File_Name = value; } }

        public string StartTime { get { return _StartTime; } set { _StartTime = value; } }
        public string EndTime { get { return _EndTime; } set { _EndTime = value; } }

        public string Tolerance
        {
            get
            {
                return _Tolerance;
            }

            set
            {
                _Tolerance = value;
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

        public string PMStartTime
        {
            get
            {
                return _PMStartTime;
            }

            set
            {
                _PMStartTime = value;
            }
        }

        public string EquipmentType
        {
            get
            {
                return _EquipmentType;
            }

            set
            {
                _EquipmentType = value;
            }
        }

        public PartArgsPack NewPart
        {
            get
            {
                return _NewPart;
            }

            set
            {
                _NewPart = value;
            }
        }

        public PartArgsPack OldPart
        {
            get
            {
                return _OldPart;
            }

            set
            {
                _OldPart = value;
            }
        }

       
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(PMArgsPack), this);
            return argsPack;
        }

    }
}
