using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAP;
namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class DataBaseManagementArgsPack
    {
        private string _Category = null;
        private string _Subcategory = null;
        private string _Name = null;
        private string _Used = null;
        private string _Custom01 = null;
        private string _Custom02 = null;
        private string _Custom03 = null;
        private string _Custom04 = null;
        private string _Custom05 = null;
        private string _Custom06 = null;
        private string _Custom07 = null;
        private string _RowId = null;
        





        private ArgumentPack argsPack = new ArgumentPack();

        public string CATEGORY { get { return _Category; } set { _Category = value; } }
        public string SUBCATEGORY { get { return _Subcategory; } set { _Subcategory = value; } }
        public string NAME { get { return _Name; } set { _Name = value; } }
        public string USED { get { return _Used; } set { _Used = value; } }
        public string CUSTOM01 { get { return _Custom01; } set { _Custom01 = value; } }
        public string CUSTOM02 { get { return _Custom02; } set { _Custom02 = value; } }
        public string CUSTOM03 { get { return _Custom03; } set { _Custom03 = value; } }
        public string CUSTOM04 { get { return _Custom04; } set { _Custom04 = value; } }
        public string CUSTOM05 { get { return _Custom05; } set { _Custom05 = value; } }

        public string CUSTOM06 { get { return _Custom06; } set { _Custom06 = value; } }
        public string CUSTOM07 { get { return _Custom07; } set { _Custom07 = value; } }
        public string ROWID { get { return _RowId; } set { _RowId = value; } }

        public string SEQUENCES { get; set; }
        public string ISALIVE { get; set; }
        public string DESCRIPTION { get; set; }



        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(DataBaseManagementArgsPack), this);
            return argsPack;
        }
    }
}
