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
    public class PartitionJobAlertArgsPack
    {
        #region Field
        private string _RowAlertCount = null;
        private string _RowJobCount = null;
      


        private ArgumentPack argsPack = new ArgumentPack();

        

        public string RowAlertCount
        {
            get
            {
                return _RowAlertCount;
            }

            set
            {
                _RowAlertCount = value;
            }
        }

        public string RowJobCount
        {
            get
            {
                return _RowJobCount;
            }

            set
            {
                _RowJobCount = value;
            }
        }
        #endregion






        #region Method
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(PartitionJobAlertArgsPack), this);
            return argsPack;
        }
        #endregion
    }
}
