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
        private string _INSTANCE_NUMBER = null;
        private string _PARAMETERID = null;
        private string _PARAMETERNAME = null;
        private string _DAYS = null;
        private string _RULENAME = null;
        private string _RULENO = null;
        private string _SPECUPPERLIMIT = null;
        private string _SPECLOWERLIMIT = null;
        private string _CONTROLUPPERLIMIT = null;
        private string _CONTROLLOWERLIMIT = null;
        private string _CHARTUSED = null;
        private string _MAILUSED = null;
        private string _MMSUSED = null;
        private string _SPECLIMITUSED = null;
        private string _ISALIVE = null;
        private string _RowId = null;
        private string _DETECTINGUSED = null;
        private string _TARGET = null;
        private string _STD_VALUE = null;
        private string _PARAVAL1 = null;
        private string _PARAVAL2 = null;
        private string _PARAVAL3 = null;
        private string _PARAVAL4 = null;
        private string _PARAVAL5 = null;
        private string _USER = null;
        private string _UPDATETIME = null;
        private string _CREATETIME = null;







        private ArgumentPack argsPack = new ArgumentPack();

        public string DBID { get { return _DBID; } set { _DBID = value; } }
        public string INSTANCE_NUMBER { get { return _INSTANCE_NUMBER; } set { _INSTANCE_NUMBER = value; } }
        public string PARAMETERID { get { return _PARAMETERID; } set { _PARAMETERID = value; } }
        public string PARAMETERNAME { get { return _PARAMETERNAME; } set { _PARAMETERNAME = value; } }
        public string DAYS { get { return _DAYS; } set { _DAYS = value; } }
        public string RULENAME { get { return _RULENAME; } set { _RULENAME = value; } }
        public string RULENO { get { return _RULENO; } set { _RULENO = value; } }
        public string SPECUPPERLIMIT { get { return _SPECUPPERLIMIT; } set { _SPECUPPERLIMIT = value; } }
        public string SPECLOWERLIMIT { get { return _SPECLOWERLIMIT; } set { _SPECLOWERLIMIT = value; } }
        public string CONTROLUPPERLIMIT { get { return _CONTROLUPPERLIMIT; } set { _CONTROLUPPERLIMIT = value; } }
        public string CONTROLLOWERLIMIT { get { return _CONTROLLOWERLIMIT; } set { _CONTROLLOWERLIMIT = value; } }
        public string CHARTUSED { get { return _CHARTUSED; } set { _CHARTUSED = value; } }
        public string MAILUSED { get { return _MAILUSED; } set { _MAILUSED = value; } }
        public string MMSUSED { get { return _MMSUSED; } set { _MMSUSED = value; } }
        public string SPECLIMITUSED { get { return _SPECLIMITUSED; } set { _SPECLIMITUSED = value; } }
        public string ISALIVE { get { return _ISALIVE; } set { _ISALIVE = value; } }
        public string ROWID { get { return _RowId; } set { _RowId = value; } }
        public string DETECTINGUSED { get { return _DETECTINGUSED; } set { _DETECTINGUSED = value; } }
        public string TARGET { get { return _TARGET; } set { _TARGET = value; } }
        public string STD_VALUE { get { return _STD_VALUE; } set { _STD_VALUE = value; } }
        public string PARAVAL1 { get { return _PARAVAL1; } set { _PARAVAL1 = value; } }
        public string PARAVAL2 { get { return _PARAVAL2; } set { _PARAVAL2 = value; } }
        public string PARAVAL3 { get { return _PARAVAL3; } set { _PARAVAL3 = value; } }
        public string PARAVAL4 { get { return _PARAVAL4; } set { _PARAVAL4 = value; } }
        public string PARAVAL5 { get { return _PARAVAL5; } set { _PARAVAL5 = value; } }

        public string USER
        {
            get
            {
                return _USER;
            }

            set
            {
                _USER = value;
            }
        }

        public string UPDATETIME
        {
            get
            {
                return _UPDATETIME;
            }

            set
            {
                _UPDATETIME = value;
            }
        }

        public string CREATETIME
        {
            get
            {
                return _CREATETIME;
            }

            set
            {
                _CREATETIME = value;
            }
        }

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(ParameterSpecManagementArgsPack), this);
            return argsPack;
        }
    }
}
