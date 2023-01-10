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
    public class MainMenuArgsPack
    {

        #region Field
        private string  _CURRENTMODEL = null;
        private string  _DESCRIPTION = null;
        private string  _DISPLAYNAME = null;
        private string  _FACILITY = null;
        private string  _ICON = null;
        private string  _INSERTTIME = null; 
        private string  _INSERTUSER = null;
        private string  _ISALIVE = null;
        private string  _LASTEVENT = null;
        private string  _LASTEVENTCODE = null;
        private string  _LASTEVENTCOMMENT = null;
        private string  _LASTEVENTFLAG = null;
        private string  _LASTEVENTTIME = null;
        private string  _LASTJOBCODE = null;
        private string  _MDI = null;
        private string  _MODELLEVELS = null;
        private string  _NAME = null;
        private string  _REGION = null;
        private string  _SEQUENCES = null;
        private string  _SHORTCUTCHARACTER = null;
        private string  _SHORTCUTDISPLAYSTRING = null;
        private string  _SHORTCUTKEY = null;
        private string  _SHORTCUTKEYS = null;
        private string  _UPDATETIME = null;
        private string  _UPDATEUSER = null;
        private string  _MAINMENU = null;




        private ArgumentPack argsPack = new ArgumentPack();

        #endregion

        #region Properties
        public string CURRENTMODEL
        {
            get
            {
                return _CURRENTMODEL;
            }

            set
            {
                _CURRENTMODEL = value;
            }
        }

        public string DESCRIPTION
        {
            get
            {
                return _DESCRIPTION;
            }

            set
            {
                _DESCRIPTION = value;
            }
        }

        public string DISPLAYNAME
        {
            get
            {
                return _DISPLAYNAME;
            }

            set
            {
                _DISPLAYNAME = value;
            }
        }

        public string FACILITY
        {
            get
            {
                return _FACILITY;
            }

            set
            {
                _FACILITY = value;
            }
        }

        public string ICON
        {
            get
            {
                return _ICON;
            }

            set
            {
                _ICON = value;
            }
        }

        public string INSERTTIME
        {
            get
            {
                return _INSERTTIME;
            }

            set
            {
                _INSERTTIME = value;
            }
        }

        public string INSERTUSER
        {
            get
            {
                return _INSERTUSER;
            }

            set
            {
                _INSERTUSER = value;
            }
        }

        public string ISALIVE
        {
            get
            {
                return _ISALIVE;
            }

            set
            {
                _ISALIVE = value;
            }
        }

        public string LASTEVENT
        {
            get
            {
                return _LASTEVENT;
            }

            set
            {
                _LASTEVENT = value;
            }
        }

        public string LASTEVENTCODE
        {
            get
            {
                return _LASTEVENTCODE;
            }

            set
            {
                _LASTEVENTCODE = value;
            }
        }

        public string LASTEVENTCOMMENT
        {
            get
            {
                return _LASTEVENTCOMMENT;
            }

            set
            {
                _LASTEVENTCOMMENT = value;
            }
        }

        public string LASTEVENTFLAG
        {
            get
            {
                return _LASTEVENTFLAG;
            }

            set
            {
                _LASTEVENTFLAG = value;
            }
        }

        public string LASTEVENTTIME
        {
            get
            {
                return _LASTEVENTTIME;
            }

            set
            {
                _LASTEVENTTIME = value;
            }
        }

        public string LASTJOBCODE
        {
            get
            {
                return _LASTJOBCODE;
            }

            set
            {
                _LASTJOBCODE = value;
            }
        }

        public string MDI
        {
            get
            {
                return _MDI;
            }

            set
            {
                _MDI = value;
            }
        }

        public string MODELLEVELS
        {
            get
            {
                return _MODELLEVELS;
            }

            set
            {
                _MODELLEVELS = value;
            }
        }

        public string NAME
        {
            get
            {
                return _NAME;
            }

            set
            {
                _NAME = value;
            }
        }

        public string REGION
        {
            get
            {
                return _REGION;
            }

            set
            {
                _REGION = value;
            }
        }

        public string SEQUENCES
        {
            get
            {
                return _SEQUENCES;
            }

            set
            {
                _SEQUENCES = value;
            }
        }

        public string SHORTCUTCHARACTER
        {
            get
            {
                return _SHORTCUTCHARACTER;
            }

            set
            {
                _SHORTCUTCHARACTER = value;
            }
        }

        public string SHORTCUTDISPLAYSTRING
        {
            get
            {
                return _SHORTCUTDISPLAYSTRING;
            }

            set
            {
                _SHORTCUTDISPLAYSTRING = value;
            }
        }

        public string SHORTCUTKEY
        {
            get
            {
                return _SHORTCUTKEY;
            }

            set
            {
                _SHORTCUTKEY = value;
            }
        }

        public string SHORTCUTKEYS
        {
            get
            {
                return _SHORTCUTKEYS;
            }

            set
            {
                _SHORTCUTKEYS = value;
            }
        }

        public string UPDATETIME1
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

        public string UPDATEUSER
        {
            get
            {
                return _UPDATEUSER;
            }

            set
            {
                _UPDATEUSER = value;
            }
        }

        public string MAINMENU
        {
            get
            {
                return _MAINMENU;
            }

            set
            {
                _MAINMENU = value;
            }
        }
        #endregion

        #region method
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(MainMenuArgsPack), this);
            return argsPack;
        }
        #endregion

    }

}
