using DevExpress.XtraEditors;
using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP.Data.Client;
using TAP.Fressage;
using TAP.UI;

namespace ISIA.UI.MANAGEMENT
{
    public partial class FrmSQLOperation : DockUIBase1T4
    {
        #region Feild
        BizDataClient bs = null;
        DataBaseManagementArgsPack args =null;

        DataSet ds = new DataSet();

        #endregion
        public FrmSQLOperation()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.MANAGEMENT.DLL", "ISIA.BIZ.MANAGEMENT.DataBaseManagement");
            //dateStart.DateTime = DateTime.Now.AddDays(-1);
            //dateEnd.DateTime = DateTime.Now;
            //dtStart.DateTime = DateTime.Now.AddDays(-1);
            //dtEnd.DateTime = DateTime.Now;
        }

        public DataSet LoadData()
        {

            args = new DataBaseManagementArgsPack();
           /* args.CATEGORY = txtMainCategory.Text;
            args.CUSTOM01 = txtMainCustom.Text;
            args.NAME = txtMainName.Text;
            args.SUBCATEGORY = txtMainSub.Text;*/
            ds = bs.ExecuteDataSet("GetDB", args.getPack());

            return ds;
        }

        public void DisplayData(DataSet ds)
        {
            if (ds == null)
            {
                return;
            }
           
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
               /* if (!base.ValidateUserInput(this.layoutControl1)) return;*/
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

       

       

        
        

       
    }
}
