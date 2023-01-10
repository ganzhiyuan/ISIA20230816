using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class WorkGroupArgsPack
    {
        #region Field
        private string _Region = null;
        private string _Sequences = null;
        private string _Shift = null;
        private string _Name = null;
        private string _Description = null;
        private string _UserId = null;
        private string _InsertTime = null;        
        private string _UpdateTime = null;
        private string _InsertUser = null;
        private string _UpdateUser = null;
        private string _IsAlive = "YES";


        private ArgumentPack argsPack = new ArgumentPack();

        #endregion

        #region Properties

        public string Region { get { return this._Region; } set { this._Region = value; } }
        public string Sequences { get { return this._Sequences; } set { this._Sequences = value; } }
        public string Shift { get { return this._Shift; } set { this._Shift = value; } }
        public string Name { get { return this._Name; } set { this._Name = value; } }
        public string UserId { get { return this._UserId; } set { this._UserId = value; } }
        public string Description { get { return this._Description; } set { this._Description = value; } }
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
            argsPack.AddArgument("arguments", typeof(WorkGroupArgsPack), this);
            return argsPack;
        }
        #endregion
    }
}