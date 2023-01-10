using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
   public class ReworkAlterFlowArgsPack
   {
        private ArgumentPack argsPack = new ArgumentPack();
        #region Fields
        private string _Factory = null;
        private string _Device = null;
        private string _Flow = null;
        private string _Update_Time = null;
        private string _Update_User = null;
        private List<ReworkAlterFlowArgsPack> _Rework_Alter_Flow = null;
        private List<ReworkAlterFlowArgsPack> _ReworkAlterFlowArgsPacks = new List<ReworkAlterFlowArgsPack>();
        #endregion

        #region Properties
        public string Factory
        {
            get
            {
                return _Factory;
            }

            set
            {
                _Factory = value;
            }
        }

        public string Device
        {
            get
            {
                return _Device;
            }

            set
            {
                _Device = value;
            }
        }

        public string Flow
        {
            get
            {
                return _Flow;
            }

            set
            {
                _Flow = value;
            }
        }

        public string Update_Time
        {
            get
            {
                return _Update_Time;
            }

            set
            {
                _Update_Time = value;
            }
        }

        public string Update_User
        {
            get
            {
                return _Update_User;
            }

            set
            {
                _Update_User = value;
            }
        }

        public List<ReworkAlterFlowArgsPack> ReworkAlterFlowArgsPacks
        {
            get
            {
                return _ReworkAlterFlowArgsPacks;
            }

            set
            {
                _ReworkAlterFlowArgsPacks = value;
            }
        }

        public List<ReworkAlterFlowArgsPack> Rework_Alter_Flow
        {
            get
            {
                return _Rework_Alter_Flow;
            }

            set
            {
                _Rework_Alter_Flow = value;
            }
        }


        #endregion
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(ReworkAlterFlowArgsPack), this);
            return argsPack;
        }


   }
}
