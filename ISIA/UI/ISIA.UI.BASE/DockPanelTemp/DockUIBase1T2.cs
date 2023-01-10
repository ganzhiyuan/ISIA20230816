using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ISIA.UI.BASE.DockPanelTemp;
using TAP.UI;
namespace ISIA.UI.BASE
{
    [Designer(typeof(TAP.UI.UIBaseDesigner), typeof(IRootDesigner))]
    public partial class DockUIBase1T2 : UIBase
    {
        public DockUIBase1T2()
        {
            InitializeComponent();
            InitializeDockpanle();
        }
        DockPanleOption dockPanleOption = new DockPanleOption();


        #region Initialize

        public void InitializeDockpanle()
        {
            dockPanleOption.DockPanelDefDock(this.dpnlLeft);
        }

        /// <summary>
        /// This method initializes this UI.
        /// </summary>
        protected override void Initialize()
        {
            #region Initialize

            try
            {
                base.InitializeUI();
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
