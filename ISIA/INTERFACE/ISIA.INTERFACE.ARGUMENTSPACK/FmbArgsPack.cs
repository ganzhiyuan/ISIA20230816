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
    public class FmbArgsPack
    {
        private ArgumentPack argsPack = new ArgumentPack();

        private string _Region;
        private string _Facility;
        private string _Area;
        private string _Line;
        private string _Bay;
        private string _Mainequipment;
        private string _Equipment;
        private string _Partname;
        private string _Start_address_x;
        private string _Start_address_y;
        private string _Width;
        private string _Height;
        private string _Control_type;
        private string _Control_name;
        private string _Default_color;
        private string _Pagenumber;
        private string _Isalive;

        public string Region
        {
            get
            {
                return _Region;
            }

            set
            {
                _Region = value;
            }
        }

        public string Facility
        {
            get
            {
                return _Facility;
            }

            set
            {
                _Facility = value;
            }
        }

        public string Area
        {
            get
            {
                return _Area;
            }

            set
            {
                _Area = value;
            }
        }

        public string Line
        {
            get
            {
                return _Line;
            }

            set
            {
                _Line = value;
            }
        }

        public string Bay
        {
            get
            {
                return _Bay;
            }

            set
            {
                _Bay = value;
            }
        }

        public string Mainequipment
        {
            get
            {
                return _Mainequipment;
            }

            set
            {
                _Mainequipment = value;
            }
        }

        public string Equipment
        {
            get
            {
                return _Equipment;
            }

            set
            {
                _Equipment = value;
            }
        }

        public string Partname
        {
            get
            {
                return _Partname;
            }

            set
            {
                _Partname = value;
            }
        }

        public string Start_address_x
        {
            get
            {
                return _Start_address_x;
            }

            set
            {
                _Start_address_x = value;
            }
        }

        public string Start_address_y
        {
            get
            {
                return _Start_address_y;
            }

            set
            {
                _Start_address_y = value;
            }
        }

        public string Width
        {
            get
            {
                return _Width;
            }

            set
            {
                _Width = value;
            }
        }

        public string Height
        {
            get
            {
                return _Height;
            }

            set
            {
                _Height = value;
            }
        }

        public string Control_type
        {
            get
            {
                return _Control_type;
            }

            set
            {
                _Control_type = value;
            }
        }

        public string Control_name
        {
            get
            {
                return _Control_name;
            }

            set
            {
                _Control_name = value;
            }
        }

        public string Default_color
        {
            get
            {
                return _Default_color;
            }

            set
            {
                _Default_color = value;
            }
        }

        public string Pagenumber
        {
            get
            {
                return _Pagenumber;
            }

            set
            {
                _Pagenumber = value;
            }
        }

        public string Isalive
        {
            get
            {
                return _Isalive;
            }

            set
            {
                _Isalive = value;
            }
        }

     

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(FmbArgsPack), this);
            return argsPack;
        }

    }
}
