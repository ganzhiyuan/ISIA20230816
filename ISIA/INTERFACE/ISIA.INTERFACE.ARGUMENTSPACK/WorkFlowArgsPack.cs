using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class WorkFlowArgsPack
    {
        #region Field
        private string _Region = null;
        private string _Facility = null;
        private string _Line = null;
        private string _Area = null;
        private string _EquipmentType = null;
        private string _Bay = null;
        private string _MainEquipment = null;
        private string _Equipment = null;
        private string _PmSchedule = null;
        private string _Worker = null;
        private string _WorkerRole = null;        
        private string _Department = null;
        private string _ShiftName = null;
        private string _EqpGroup = null;
        private string _Position = null;
        private string _Name = null;
        private string _Model = null;

        private ArgumentPack argsPack = new ArgumentPack();

        #endregion

        #region Properties

        public string Region { get { return this._Region; } set { this._Region = value; } }
        public string Facility { get { return this._Facility; } set { this._Facility = value; } }
        public string Line { get { return this._Line; } set { this._Line = value; } }
        public string Area { get { return this._Area; } set { this._Area = value; } }
        public string EquipmentType { get { return this._EquipmentType; } set { this._EquipmentType = value; } }
        public string Bay { get { return this._Bay; } set { this._Bay = value; } }
        public string MainEquipment { get { return this._MainEquipment; } set { this._MainEquipment = value; } }
        public string Equipment { get { return this._Equipment; } set { this._Equipment = value; } }
        public string PmSchedule { get { return this._PmSchedule; } set { this._PmSchedule = value; } }
        public string Worker { get { return this._Worker; } set { this._Worker = value; } }
        public string WorkerRole { get { return this._WorkerRole; } set { this._WorkerRole = value; } }
        public string Department { get { return this._Department; } set { this._Department = value; } }

        public string ShiftName { get { return this._ShiftName; } set { this._ShiftName = value; } }

        public string EqpGroup { get { return this._EqpGroup; } set { this._EqpGroup = value; } }

        public string Position { get { return this._Position; } set { this._Position = value; } }

        public string Name { get { return this._Name; } set { this._Name = value; } }

        public string Model { get { return this._Model; } set { this._Model = value; } }
        #endregion

        #region method
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(WorkFlowArgsPack), this);
            return argsPack;
        }
        #endregion
    }
}