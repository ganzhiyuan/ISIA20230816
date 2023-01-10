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
    public class WareHouseShelfArgsPack
    {

        private string _Category = null;
        private string _WareHouse = null;
        private string _Shelf = null;
        private string _IsForbidden = null;

        private List<WareHouseShelfArgsPack> _WareHouseArgsPacks= new List<WareHouseShelfArgsPack>();

        private ArgumentPack argsPack = new ArgumentPack();

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

        public List<WareHouseShelfArgsPack> WareHouseArgsPacks
        {
            get
            {
                return _WareHouseArgsPacks;
            }

            set
            {
                _WareHouseArgsPacks = value;
            }
        }
        public string WareHouse
        {
            get
            {
                return _WareHouse;
            }

            set
            {
                _WareHouse = value;
            }
        }

        public string Shelf
        {
            get
            {
                return _Shelf;
            }

            set
            {
                _Shelf = value;
            }
        }

        public string IsForbidden
        {
            get
            {
                return _IsForbidden;
            }

            set
            {
                _IsForbidden = value;
            }
        }

        

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(WareHouseShelfArgsPack), this);
            return argsPack;
        }
    }
}
