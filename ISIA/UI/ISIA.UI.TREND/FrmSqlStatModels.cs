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
using TAP.UI;

namespace ISIA.UI.TREND
{
    public partial class FrmSqlStatModels : DockUIBase1T1
    {

        AwrCommonArgsPack args = new AwrCommonArgsPack();
        BizDataClient bs;

        public FrmSqlStatModels()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.SqlstatServices");
            dateStart.DateTime = DateTime.Now.AddDays(-1);
            dateEnd.DateTime = DateTime.Now;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataSet LoadData()
        {
            try
            {
                args.StartTimeKey = dateStart.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                args.EndTimeKey = dateEnd.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                args.CommandName = cboModules.Text;
                args.CommandType = cboParaName.Text;
                DataSet dataSet = bs.ExecuteDataSet("GetSqlstatModels", args.getPack());
                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DisplayData(DataSet dataSet)
        {
            if (dataSet == null)
            {
                return;
            }
            gridControl1.DataSource = dataSet.Tables[0];
            gridView1.BestFitColumns();
        }
    }
}
