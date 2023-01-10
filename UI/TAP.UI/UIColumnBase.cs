using System;
using System.Collections.Generic;
using System.Text;

using TAP;
using TAP.Models;
using TAP.Models.UIBasic;

namespace TAP.UI
{
    /// <summary>
    /// This class provides user-friendly column names.
    /// </summary>
    public class UIColumnBase
    {
        #region Fields

        private static UIColumnBase _self = null;

        /// <summary>
        /// Column list
        /// </summary>
        protected ColumnModelSet columns;

        #endregion

        #region Properties

        /// <summary>
        /// Column list
        /// </summary>
        public ColumnModelSet Columns { get { return this.columns; } set { this.columns = value; } }

        /// <summary>
        /// Static accessor of UIColumnBase
        /// </summary>
        public static UIColumnBase Instance
        {
            get
            {
                if (object.Equals(_self, null))
                    _self = new UIColumnBase();

                return _self;
            }
        }

        #endregion

        #region Creator

        /// <summary>
        /// This creates instance of UIColumnBase.
        /// </summary>
        public UIColumnBase()
        {
            try
            {
                this.Initialize();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Methods

        private void Initialize()
        {
            #region Initialize

            try
            {
                columns = new ColumnModelSet();
                columns.LoadAllColumns(InfoBase._USER_INFO.Name);

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

    }
}
