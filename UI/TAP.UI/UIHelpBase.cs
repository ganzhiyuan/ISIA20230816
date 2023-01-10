using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace TAP.UI
{
    /// <summary>
    /// This class provides method that call method of other UI.
    /// </summary>
    public class UICallBase
    {
        #region Fields

        private static UICallBase _self = null;

        #endregion

        #region Properties

        /// <summary>
        /// Static accessor of UICallBase
        /// </summary>
        public static UICallBase Instance
        {
            get
            {
                if (object.Equals(_self, null))
                    _self = new UICallBase();

                return _self;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// This is eventhandler for open child UI.
        /// </summary>
        public static event OpenChildUIEventHandler OpenChildUI;

        /// <summary>
        /// This is delegate for OpenChildUIEventHandler typed event handler.
        /// </summary>
        /// <param name="mainMenu">Main menu name</param>
        /// <param name="menu">Menu name</param>
        /// <param name="arguments">Argument list for executing</param>
        public delegate void OpenChildUIEventHandler(string mainMenu, string menu, ArgumentPack arguments);

        #endregion

        #region Creator

        /// <summary>
        /// This creates instance of UICallBase.
        /// </summary>
        public UICallBase()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method open UI specified and excute default command.
        /// </summary>
        /// <param name="mainMenu">Main menu name</param>
        /// <param name="menu">Menu name</param>
        /// <param name="arguments">Argument list for executing</param>
        public void OpenAndCommand(string mainMenu, string menu, ArgumentPack arguments)
        {
            OpenChildUI(mainMenu, menu, arguments);
        }

        #endregion
    }
}
