using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAP;
namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class EquipmentArgsPack 
    {
        private string _Facility = null;
        private string _Region = null;
        private string _Name = null;
        private string _Line = null;
        private string _Area = null;
        private string _Bay = null;
        private string _MainEqp = null;
        private string _EquipmentStatus = null;
        private string _CommuntionStatus = null;
        private string _LastEventTimeKey = null;
        private string _StartLastEventTimeKey = null;
        private string _EndLastEventTimeKey = null;
        private string _EqpType = null;
        private string _EqpEvent = null;
        private string _EqpAssetCode = null;
        private string _EqpFunctionLocation = null;
        private string _EqpProductionDate = null;

        private string _StartTime = null;
        private string _EndTime = null;
        private string _ModelLevels = null;

        //
        private string _DataBase = null;
        private string _ParameterName = null;
        private string _DBID = null;


        //

        private ArgumentPack argsPack = new ArgumentPack();

        public string Region { get { return _Region; } set { _Region = value; } }
        public string Facility { get { return _Facility; } set { _Facility = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string Line { get { return _Line; } set { _Line = value; } }
        public string Area { get { return _Area; } set { _Area = value; } }
        public string Bay { get { return _Bay; } set { _Bay = value; } }
        public string MainEqp { get { return _MainEqp; } set { _MainEqp = value; } }
        public string EquipmentStatus { get { return _EquipmentStatus; } set { _EquipmentStatus = value; } }
        public string CommuntionStatus { get { return _CommuntionStatus; } set { _CommuntionStatus = value; } }

        public string LastEventTimeKey { get { return _LastEventTimeKey; } set { _LastEventTimeKey = value; } }
        public string StartLastEventTimeKey { get { return _StartLastEventTimeKey; } set { _StartLastEventTimeKey = value; } }
        public string EndLastEventTimeKey { get { return _EndLastEventTimeKey; } set { _EndLastEventTimeKey = value; } }

        public string EqpType { get { return _EqpType; } set { _EqpType = value; } }
        public string EqpEvent { get { return _EqpEvent; } set { _EqpEvent = value; } }
        public string EqpAssetCode { get { return _EqpAssetCode; } set { _EqpAssetCode = value; } }
        public string EqpFunctionLocation { get { return _EqpFunctionLocation; } set { _EqpFunctionLocation = value; } }
        public string EqpProductionDate { get { return _EqpProductionDate; } set { _EqpProductionDate = value; } }

        public string StartTime { get { return _StartTime; } set { _StartTime = value; } }

        public string EndTime { get { return _EndTime; } set { _EndTime = value; } }

        public string ModelLevels { get { return _ModelLevels; } set { _ModelLevels = value; } }

        //
        public string DataBase { get { return _DataBase; } set { _DataBase = value; } }

        public string ParameterName { get { return _ParameterName; } set { _ParameterName = value; } }
        public string DBID { get { return _DBID; } set { _DBID = value; } }
        //


        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(EquipmentArgsPack), this);
            return argsPack;
        }
    }
}
