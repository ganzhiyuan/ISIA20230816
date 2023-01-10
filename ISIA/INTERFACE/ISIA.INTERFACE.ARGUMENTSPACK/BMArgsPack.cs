using System;
using TAP;
using TAP.Models;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class BMArgsPack
    {
        private ArgumentPack argsPack = new ArgumentPack();

        private string _FabId = null;
        private string _DateTimeStart = null;
        private string _DateTimeEnd = null;
        private string _Region = null;
        private string _Line = null;
        private string _Area = null;
        private string _MainEquipment = null;
        private string _Equipment = null;
        private string _Bay = null;

        private string _InsertTime = null;
        private string _TimeKey = null;
        private string _Comment = null;
        private string _ResultYN = null;
        private string _User = null;

        private string _ShiftName = null;

        private string _EquipmentStatus = null;
        private string _BMCode = null;
        private string _Image1 = null;
        private string _Image2 = null;
        private string _Image3 = null;
        private string _Image4 = null;
        private string _Image5 = null;

        private string _Phenomenon = null;
        private string _Reason = null;
        private string _Measures = null;
        private string _BMStartTime = null;
        private string _EquipmentsModel = null;
        private string _EquipmentType = null;
        private string _BMSection = null;
        

        private string _UpdateTime = null;
        private DateTime _LastEventTime =new DateTime();

        private string _PartModel = null;
        private PartArgsPack _NewPart = null;
        private PartArgsPack _OldPart = null;

        public string MainEquipment { get { return _MainEquipment; } set { _MainEquipment = value; } }
        public string FabId { get { return _FabId; } set { _FabId = value; } }
        public string DateTimeStart { get { return _DateTimeStart; } set { _DateTimeStart = value; } }
        public string DateTimeEnd { get { return _DateTimeEnd; } set { _DateTimeEnd = value; } }
        public string Region { get { return _Region; } set { _Region = value; } }
        public string Line { get { return _Line; } set { _Line = value; } }
        public string Area { get { return _Area; } set { _Area = value; } }
        public string Equipment { get { return _Equipment; } set { _Equipment = value; } }
        public string Bay { get { return _Bay; } set { _Bay = value; } }
        public string Comment { get { return _Comment; } set { _Comment = value; } }
        public string ResultYN { get { return _ResultYN; } set { _ResultYN = value; } }
        public string User { get { return _User; } set { _User = value; } }
        public string InsertTime { get { return _InsertTime; } set { _InsertTime = value; } }
        public string TimeKey { get { return _TimeKey; } set { _TimeKey = value; } }
        public string ShiftName { get { return _ShiftName; } set { _ShiftName = value; } }
        public string EquipmentStatus { get{ return _EquipmentStatus;}set{ _EquipmentStatus = value; } }
        public string BMCode{get { return _BMCode;}set{  _BMCode = value; } }
        public string UpdateTime{get{ return _UpdateTime; } set{_UpdateTime = value; } }

       

        public DateTime LastEventTime { get{ return _LastEventTime; } set { _LastEventTime = value; } }

        public string Phenomenon { get {return _Phenomenon;}set { _Phenomenon = value; } }

        public string Reason {get { return _Reason; } set { _Reason = value;} }

        public string Measures {get { return _Measures; }set { _Measures = value; } }

        public string BMStartTime {get {return _BMStartTime; } set{ _BMStartTime = value; }}

        public string Image1 { get{ return _Image1; } set {_Image1 = value; }}

        public string Image2 {get{ return _Image2; } set{ _Image2 = value; } }

        public string Image3 {get { return _Image3; }set{ _Image3 = value; }}

        public string Image4 { get{ return _Image4; } set{_Image4 = value;} }

        public string Image5 { get{ return _Image5;}set{ _Image5 = value; }}

        public string EquipmentsModel
        {
            get
            {
                return _EquipmentsModel;
            }

            set
            {
                _EquipmentsModel = value;
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

        public string BMSection
        {
            get
            {
                return _BMSection;
            }

            set
            {
                _BMSection = value;
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

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(BMArgsPack), this);
            return argsPack;
        }


    }

}