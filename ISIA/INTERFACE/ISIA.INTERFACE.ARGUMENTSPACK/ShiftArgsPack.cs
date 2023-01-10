using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class ShiftArgsPack
    {
        #region Field
        private string _Region = null;
        private string _Facility = null;
        private int _ShiftCount = 1;
        private int _GroupCount = 1;
        private string _ShiftStartDate = null;
        private string _ShiftStartTime = null;
        private int _ShiftIntervalTime = 7;
        private int _ShiftIntervalDay = 1;
        private string _ShiftIntervalOption = null;
        private string _InsertTime = null;        
        private string _UpdateTime = null;
        private string _InsertUser = null;
        private string _UpdateUser = null;
        private string _IsAlive = "YES";


        private ArgumentPack argsPack = new ArgumentPack();

        #endregion

        #region Properties

        public string Region { get { return this._Region; } set { this._Region = value; } }
        public string Facility { get { return this._Facility; } set { this._Facility = value; } }
        public int ShiftCount { get { return this._ShiftCount; } set { this._ShiftCount = value; } }
        public int GroupCount { get { return this._GroupCount; } set { this._GroupCount = value; } }
        public string ShiftStartDate { get { return this._ShiftStartDate; } set { this._ShiftStartDate = value; } }
        public string ShiftStartTime { get { return this._ShiftStartTime; } set { this._ShiftStartTime = value; } }
        public int ShiftIntervalTime { get { return this._ShiftIntervalTime; } set { this._ShiftIntervalTime = value; } }
        public int ShiftIntervalDay { get { return this._ShiftIntervalDay; } set { this._ShiftIntervalDay = value; } }
        public string ShiftIntervalOption { get { return this._ShiftIntervalOption; } set { this._ShiftIntervalOption = value; } }
        public string InsertTime { get { return this._InsertTime; } set { this._InsertTime = value; } }
        public string UpdateTime { get { return this._UpdateTime; } set { this._UpdateTime = value; } }
        public string InsertUser { get { return this._InsertUser; } set { this._InsertUser = value; } }
        public string UpdateUser { get { return this._UpdateUser; } set { this._UpdateUser = value; } }
        public string IsAlive { get { return this._IsAlive; } set { this._IsAlive = value; } }
        #endregion

        #region method
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(ShiftArgsPack), this);
            return argsPack;
        }
        #endregion
    }
}