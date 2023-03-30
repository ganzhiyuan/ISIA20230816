using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class MailGroupArgsPack
    {
        #region Field
        private ArgumentPack argsPack = new ArgumentPack();
        public string REGION { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string GROUPNAME { get; set; }
        public string FACILITY { get; set; }
        public string USERID { get; set; }
        public string USERNAME { get; set; }
        public string DEPARTMENT { get; set; }
        public string POSITION { get; set; }
        public string INSERTTIME { get; set; }
        public string INSERTUSER { get; set; }
        public string UPDATETIME { get; set; }
        public string UPDATEUSER { get; set; }

        #endregion

        #region method
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(MailGroupArgsPack), this);
            return argsPack;
        }
        #endregion
    }
}
