using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class ClassicUIArgsPack
    {
        #region Field
        private string _Facility = null;
        private string _MDI = null;
        private string _MainMenu = null;
        private string _SubMenu = null;
        private string _Name = null;
        private string _Region = null;

        private string _UILayout = null;
        private string _UIType = null;


        private string _DisplayName = null;        
        private string _AssemblyName = null; 
        private string _AssemblyFileName = null;
        private string _SubType = null;
        private string _ImageName = null;
        private string _SmallImageName = null;

        private int _Icon = 0;

        private string _Description = null;

        private string _LastEventComment = null;        
        private string _LastEvent = null;
        private string _LastEventFlag = null;
        private string _LastEventTime = null;
        private string _LastEventCode = null;
        private string _LastJobCode = null;


        private string _InsertTime = null;
        private string _UpdateTime = null;

        private string _InsertUser = null;
        private string _UpdateUser = null;

        private string _ISALIVE = null;
        


        #endregion
        #region Properties
        public string Facility { get { return _Facility; } set { _Facility = value; } }
        public string MDI { get { return _MDI; } set { _MDI = value; } }
        public string MainMenu { get { return _MainMenu; } set { _MainMenu = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string Region { get { return _Region; } set { _Region = value; } }
        public string DisplayName { get { return _DisplayName; } set { _DisplayName = value; } }
        public string UILayout { get { return _UILayout; } set { _UILayout = value; } }
        public string AssemblyName { get { return _AssemblyName; } set { _AssemblyName = value; } }
        public string AssemblyFileName { get { return _AssemblyFileName; } set { _AssemblyFileName = value; } }
        public int Icon { get { return _Icon; } set { _Icon = value; } }
        public string Description { get { return _Description; } set { _Description = value; } }
        public string ImageName { get { return _ImageName; } set { _ImageName = value; } }
        public string SmallImageName { get { return _SmallImageName; } set { _SmallImageName = value; } }
        public string InsertTime { get { return _InsertTime; } set { _InsertTime = value; } }
        public string InsertUser { get { return _InsertUser; } set { _InsertUser = value; } }
        public string LastEventComment { get { return _LastEventComment; } set { _LastEventComment = value; } }
        public string SubType { get { return _SubType; } set { _SubType = value; } }
        public string UIType { get { return _UIType; } set { _UIType = value; } }
        public string LastEventFlag { get { return _LastEventFlag; } set { _LastEventFlag = value; } }
        public string UpdateTime { get { return _UpdateTime; } set { _UpdateTime = value; } }
        public string UpdateUser { get { return _UpdateUser; } set { _UpdateUser = value; } }
        public string LastEvent { get { return _LastEvent; } set { _LastEvent = value; } }
        public string LastEventTime { get { return _LastEventTime; } set { _LastEventTime = value; } }
        public string LastEventCode { get { return _LastEventCode; } set { _LastEventCode = value; } }
        public string LastJobCode { get { return _LastJobCode; } set { _LastJobCode = value; } }

        public string ISALIVE { get { return _ISALIVE; } set { _ISALIVE = value; } }

        public string SubMenu
        {
            get
            {
                return _SubMenu;
            }

            set
            {
                _SubMenu = value;
            }
        }

        #endregion
        private ArgumentPack argsPack = new ArgumentPack();
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(ClassicUIArgsPack), this);
            return argsPack;
        }
    }
}
