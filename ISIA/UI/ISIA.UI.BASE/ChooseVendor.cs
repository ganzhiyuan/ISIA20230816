using DevExpress.Utils.DragDrop;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using ISIA.INTERFACE.ARGUMENTSPACK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP;
using TAP.Data.Client;
using TAP.Models;
using TAP.Models.Factories.Facilities;
using TAP.UI;
using TAP.UIControls.BasicControlsDEV;
using ISIA.COMMON;

namespace ISIA.UI.BASE
{
    public partial class ChooseVendor : Form
    {
        public ChooseVendor()
        {
            InitializeComponent();                   
        }

        #region Feild
        public string vendor;
        public bool isConfirm;
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.cboVendor.Text = "";
            this.txtVendor.Text = "";
            isConfirm = false;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            isConfirm = true;
            if (this.cboVendor.SelectedIndex<0)
            {
                TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.WARNING, "You must Choose one vendor.");
            }
            else
            {
                vendor = cboVendor.SelectedValue();
                this.Close();
            }
        }

        private void cboVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtVendor.Text= cboVendor.SelectedValue();
        }
    }
}
