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
    public class ProductionPlannArgsPack
    {
        private ArgumentPack argsPack = new ArgumentPack();


        private string _Workshop = null;
        private string _WaferType = null;
        private string _Busbar = null;
        private string _Category = null;
        private string _Workdate = null;
        private int _Version = 0;
        private float _Value = 0f;
        private string _Unitid = null;
        private string _Activity = null;
        private string _PrevActivity = null;
        private string _CustomActivity = null;
        private string _PrevCustomActivity = null;
        private string _IsUsable = null;
        private string _SiteId = null;
        private string _Description = null;
        private string _Reasoncode = null;
        private string _Comments = null;
        private string _Creator = null;
        private DateTime _CreateTime = new DateTime();
        private string _Modifier = null;
        private DateTime _ModifyTime = new DateTime();
        private DateTime _LasteventTime = new DateTime();
        private string _Tid = null;
        private string _Device = null;
        private DataTable _Dt = null;
        private List<ProductionPlannArgsPack> _proPlanList = new List<ProductionPlannArgsPack>();
        private List<DataTable> _DtPlanList = new List<DataTable>();

        public string Workshop
        {
            get
            {
                return _Workshop;
            }

            set
            {
                _Workshop = value;
            }
        }

        public string WaferType
        {
            get
            {
                return _WaferType;
            }

            set
            {
                _WaferType = value;
            }
        }

        public string Busbar
        {
            get
            {
                return _Busbar;
            }

            set
            {
                _Busbar = value;
            }
        }

        public string Category
        {
            get
            {
                return _Category;
            }

            set
            {
                _Category = value;
            }
        }

        public string Workdate
        {
            get
            {
                return _Workdate;
            }

            set
            {
                _Workdate = value;
            }
        }

        public int Version
        {
            get
            {
                return _Version;
            }

            set
            {
                _Version = value;
            }
        }

        public float Value
        {
            get
            {
                return _Value;
            }

            set
            {
                _Value = value;
            }
        }

        public string Unitid
        {
            get
            {
                return _Unitid;
            }

            set
            {
                _Unitid = value;
            }
        }

        public string Activity
        {
            get
            {
                return _Activity;
            }

            set
            {
                _Activity = value;
            }
        }

        public string PrevActivity
        {
            get
            {
                return _PrevActivity;
            }

            set
            {
                _PrevActivity = value;
            }
        }

        public string CustomActivity
        {
            get
            {
                return _CustomActivity;
            }

            set
            {
                _CustomActivity = value;
            }
        }

        public string PrevCustomActivity
        {
            get
            {
                return _PrevCustomActivity;
            }

            set
            {
                _PrevCustomActivity = value;
            }
        }

        public string IsUsable
        {
            get
            {
                return _IsUsable;
            }

            set
            {
                _IsUsable = value;
            }
        }

        public string SiteId
        {
            get
            {
                return _SiteId;
            }

            set
            {
                _SiteId = value;
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        public string Reasoncode
        {
            get
            {
                return _Reasoncode;
            }

            set
            {
                _Reasoncode = value;
            }
        }

        public string Comments
        {
            get
            {
                return _Comments;
            }

            set
            {
                _Comments = value;
            }
        }

        public string Creator
        {
            get
            {
                return _Creator;
            }

            set
            {
                _Creator = value;
            }
        }

        public DateTime CreateTime
        {
            get
            {
                return _CreateTime;
            }

            set
            {
                _CreateTime = value;
            }
        }

        public string Modifier
        {
            get
            {
                return _Modifier;
            }

            set
            {
                _Modifier = value;
            }
        }

        public DateTime ModifyTime
        {
            get
            {
                return _ModifyTime;
            }

            set
            {
                _ModifyTime = value;
            }
        }

        public DateTime LasteventTime
        {
            get
            {
                return _LasteventTime;
            }

            set
            {
                _LasteventTime = value;
            }
        }

        public string Tid
        {
            get
            {
                return _Tid;
            }

            set
            {
                _Tid = value;
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

        public DataTable Dt
        {
            get
            {
                return _Dt;
            }

            set
            {
                _Dt = value;
            }
        }

        public List<ProductionPlannArgsPack> ProPlanList
        {
            get
            {
                return _proPlanList;
            }

            set
            {
                _proPlanList = value;
            }
        }

        public List<DataTable> DtPlanList
        {
            get
            {
                return _DtPlanList;
            }

            set
            {
                _DtPlanList = value;
            }
        }

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(ProductionPlannArgsPack), this);
            return argsPack;
        }

    }
}
