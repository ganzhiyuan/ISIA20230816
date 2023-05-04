using ISIA.COMMON;
using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;
using ISIA.UI.ANALYSIS.Dto;
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

namespace ISIA.UI.ANALYSIS
{
    public partial class FrmFindNewSqlID : DockUIBase1T1
    {

        #region Feild 
        BizDataClient bs;
        #endregion

        public FrmFindNewSqlID()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.SnapTrendChart1");
            dtpStartTime.DateTime = DateTime.Now;
        }

        #region Method
        public DataSet LoadData()
        {
            try
            {
                List<SqlShowCl> listCL = new List<SqlShowCl>();
                DateTime dtNow = dtpStartTime.DateTime;
                AwrCommonArgsPack args = new AwrCommonArgsPack();
                args.Days = "WEEK";
                for (int i = 1; i <= 4; i++)//查询4周
                {                    
                    args.StartTimeKey = dtNow.AddDays(-7*i).ToString("yyyy-MM-dd");
                    args.EndTimeKey = dtNow.AddDays(-7 * (i-1)).ToString("yyyy-MM-dd");
                    args.DbName = cmbDbName.Text.Split('(')[0];
                    switch (i)
                    {
                        case 1:
                            args.ParameterName = "day0";
                            break;
                        case 2:
                            args.ParameterName = "day7";
                            break;
                        case 3:
                            args.ParameterName = "day14";
                            break;
                        case 4:
                            args.ParameterName = "day21";
                            break;
                    }
                    DataSet dataSet = bs.ExecuteDataSet("GetSqlStatByWeek", args.getPack());
                    if (dataSet == null||dataSet.Tables[0]==null||dataSet.Tables[0].Rows.Count<1)
                    {
                        continue;
                    }

                    List<SqlStatNew> list = DataTableExtend.GetList<SqlStatNew>(dataSet.Tables[0]);

                    foreach (var item in list)
                    {
                        SqlShowCl cl = listCL.FirstOrDefault(x => x.SQL_ID == item.SQL_ID);
                        if (cl == null)
                        {
                            cl = new SqlShowCl();
                        }
                        switch (i)
                        {
                            case 1:
                                cl.week1 = item.day0;
                                break;
                            case 2:
                                cl.week2 = item.day7;
                                break;
                            case 3:
                                cl.week3 = item.day14;
                                break;
                            case 4:
                                cl.week4 = item.day21;
                                break;
                        }
                        cl.SQL_ID = item.SQL_ID;
                        listCL.Add(cl);
                    }
                }


                
                foreach (var item in listCL)
                {
                    item.AVG=Math.Round((item.week1+item.week2+item.week3+item.week4)/4,1);
                    if (item.AVG<1)
                    {
                        item.SqlType = "NEW SQL";
                    }
                    else if (Convert.ToDecimal((int)(item.AVG))==item.AVG)
                    {
                        item.SqlType = "USUAL";
                    }
                    else
                    {
                        item.SqlType = "UNUSUAL";
                    }
                }

                DataSet dt = DataTableExtend.ConvertToDataSet<SqlShowCl>(listCL);

                return dt;
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

        #endregion

        #region Event


        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!base.ValidateUserInput(this.layoutControl1)) return;


                base.BeginAsyncCall("LoadData", "DisplayData", EnumDataObject.DATASET);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
           
        }

        #endregion
    }
}
