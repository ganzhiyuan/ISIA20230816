using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class BmCodeArgsPack
    {

        #region Field
        private string _AREA = null;
        private string _BMCODE1 = null;
        private string _BMSECTION = null;
        private string _DESCRIPTION = null;
        private string _EQUIPMENTMODEL = null;
        private string _EQUIPMENTTYPE = null;
        private string _INSERTTIME = null;
        private string _INSERTUSER = null;
        private string _REGION = null;
        private string _SEQUENCES = null;
        private string _UPDATETIME = null;
        private DataTable _dt1 = new DataTable();
        private DataTable _dt2 = new DataTable();
        private ArgumentPack argsPack = new ArgumentPack();

        #endregion

        #region Properties

        public string AREA { get { return this._AREA; } set { this._AREA = value; } }
        public string BMCODE1 { get { return this._BMCODE1; } set { this._BMCODE1 = value; } }
        public string BMSECTION { get { return this._BMSECTION; } set { this._BMSECTION = value; } }
        public string DESCRIPTION { get { return this._DESCRIPTION; } set { this._DESCRIPTION = value; } }
        public string EQUIPMENTMODEL { get { return this._EQUIPMENTMODEL; } set { this._EQUIPMENTMODEL = value; } }
        public string EQUIPMENTTYPE { get { return this._EQUIPMENTTYPE; } set { this._EQUIPMENTTYPE = value; } }
        public string INSERTTIME { get { return this._INSERTTIME; } set { this._INSERTTIME = value; } }
        public string INSERTUSER { get { return this._INSERTUSER; } set { this._INSERTUSER = value; } }
        public string REGION { get { return this._REGION; } set { this._REGION = value; } }
        public string SEQUENCES { get { return this._SEQUENCES; } set { this._SEQUENCES = value; } }
        public string UPDATETIME { get { return this._UPDATETIME; } set { this._UPDATETIME = value; } }
        public DataTable Dt1 { get { return _dt1; } set { _dt1 = value; } }
        public DataTable Dt2 { get { return _dt2; } set { _dt2 = value; } }
        #endregion

        #region method
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(BmCodeArgsPack), this);
            return argsPack;
        }
        #endregion

    }

}
