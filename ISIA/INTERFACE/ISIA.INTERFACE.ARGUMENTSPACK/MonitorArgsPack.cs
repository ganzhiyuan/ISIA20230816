using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAP;
namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class MonitorArgsPack
    {


        private string _StartTime = null;
        private string _EndTime = null;
        private string _DataBase = null;
        private string _Params = null;
        private string _ParamsType = null;


        private ArgumentPack argsPack = new ArgumentPack();

       

        public string StartTime { get { return _StartTime; } set { _StartTime = value; } }
        public string EndTime { get { return _EndTime; } set { _EndTime = value; } }
        public string DataBase { get { return _DataBase; } set { _DataBase = value; } }
        public string Params { get { return _Params; } set { _Params = value; } }
        public string ParamsType { get { return _ParamsType; } set { _ParamsType = value; } }



        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(MonitorArgsPack), this);
            return argsPack;
        }
    }
}
