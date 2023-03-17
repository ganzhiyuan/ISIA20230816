using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAP;
namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class ParameterSpecManagementArgsPack
    {
        private string _DBID  = null;
        private string _RULENAME = null;
        private string _RULENO = null;
        private string _RULETEXT = null;
        private string _N_VALUE = null;
        private string _M_VALUE = null;
        private string _UPDATETIME = null;
        private string _UPDATEUSER = null;
        private string _INSERTTIME = null;
        private string _INSERTUSER = null;
        private string _SEQUENCES = null;
        private string _ISALIVE = null;
        private string _RowId = null;
        



        private ArgumentPack argsPack = new ArgumentPack();

        public string DBID { get { return _DBID; } set { _DBID = value; } }
        public string RULENAME { get { return _RULENAME; } set { _RULENAME = value; } }
        public string RULENO { get { return _RULENO; } set { _RULENO = value; } }
        public string RULETEXT { get { return _RULETEXT; } set { _RULETEXT = value; } }
        public string N_VALUE { get { return _N_VALUE; } set { _N_VALUE = value; } }
        public string M_VALUE { get { return _M_VALUE; } set { _M_VALUE = value; } }
        public string UPDATETIME { get { return _UPDATETIME; } set { _UPDATETIME = value; } }
        public string UPDATEUSER { get { return _UPDATEUSER; } set { _UPDATEUSER = value; } }
        public string CUSTOM05 { get { return _INSERTTIME; } set { _INSERTTIME = value; } }
        public string INSERTUSER { get { return _INSERTUSER; } set { _INSERTUSER = value; } }
        public string SEQUENCES { get { return _SEQUENCES; } set { _SEQUENCES = value; } }
        public string ISALIVE { get { return _ISALIVE; } set { _ISALIVE = value; } }
        public string ROWID { get { return _RowId; } set { _RowId = value; } }
       


        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(ParameterSpecManagementArgsPack), this);
            return argsPack;
        }
    }
}
