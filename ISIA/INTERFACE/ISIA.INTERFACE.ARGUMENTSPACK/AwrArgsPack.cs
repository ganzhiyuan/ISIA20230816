using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TAP;
using TAP.Models;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class AwrArgsPack
    {
        private ArgumentPack argsPack = new ArgumentPack();


        private string _ParamType;

        public string ParamType
        {
            get
            {
                return _ParamType;
            }

            set
            {
                _ParamType = value;
            }
        }

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(AwrArgsPack), this);
            return argsPack;
        }

    }
}
