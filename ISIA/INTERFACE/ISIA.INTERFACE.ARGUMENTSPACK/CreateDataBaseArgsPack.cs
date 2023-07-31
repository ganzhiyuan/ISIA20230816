using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class CreateDataBaseArgsPack
    {
        private string _UserID = null;
        private string _Password = null;
        private string _IPAddress = null;
        private string _IPPort = null;
        private string _ServiceName = null;
        private string _DBLinkName = null;
        private string _DataTableName = null;
        private string _DataTablePKName = null;
        private DateTime _DateNow ;
        private string _ProcedureName = null;
        private string _detectionFlag = null;

        private ArgumentPack argsPack = new ArgumentPack();

        public string UserID { get { return _UserID; } set { _UserID = value; } }

        public string Password { get { return _Password; } set { _Password = value; } }

        public string IPAddress { get { return _IPAddress; } set { _IPAddress = value; } }

        public string IPPort { get { return _IPPort; } set { _IPPort = value; } }

        public string ServiceName { get { return _ServiceName; } set { _ServiceName = value; } }

        public string DBLinkName { get { return _DBLinkName; } set { _DBLinkName = value; } }

        public string DataTableName { get { return _DataTableName; } set { _DataTableName = value; } }

        public string DataTablePKName { get { return _DataTablePKName; } set { _DataTablePKName = value; } }

        public DateTime DateNow { get { return _DateNow; } set { _DateNow = value; } }

        public string ProcedureName { get { return _ProcedureName; } set { _ProcedureName = value; } }

        public string DetectionFlag { get { return _detectionFlag; } set { _detectionFlag = value; } }

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(CreateDataBaseArgsPack), this);
            return argsPack;
        }
    }
}
