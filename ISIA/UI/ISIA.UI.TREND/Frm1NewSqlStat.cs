using ISIA.COMMON;
using ISIA.INTERFACE.ARGUMENTSPACK;
using ISIA.UI.BASE;
using ISIA.UI.TREND.Dto;
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
    public partial class Frm1NewSqlStat : DockUIBase1T1
    {

        #region Feild 
        BizDataClient bs;
        #endregion

        public Frm1NewSqlStat()
        {
            InitializeComponent();
            bs = new BizDataClient("ISIA.BIZ.TREND.DLL", "ISIA.BIZ.TREND.SnapTrendChart1");
            dateStart.DateTime = DateTime.Now;
        }

        #region Method
        public DataSet LoadData()
        {
            try
            {
                DateTime dtNow = dateStart.DateTime;
                AwrCommonArgsPack args = new AwrCommonArgsPack();
                args.Days = "WEEK";
                List<SqlShow> listReturn = new List<SqlShow>();
                for (int i = 1; i <= 4; i++)//查询4周
                {
                    
                    args.StartTimeKey = dtNow.AddDays(-7*i).ToString("yyyy-MM-dd HH:mm:ss");
                    args.EndTimeKey = dtNow.AddDays(-7 * (i-1)).ToString("yyyy-MM-dd HH:mm:ss");
                    args.Days = i.ToString();
                    args.DbName = cmbDbName.Text;
                    DataSet dataSet = bs.ExecuteDataSet("GetSqlstatByUnit", args.getPack());
                    if (dataSet == null||dataSet.Tables[0]==null||dataSet.Tables[0].Rows.Count<1)
                    {
                        continue;
                    }

                    List<SqlStatNew> list = DataTableExtend.GetList<SqlStatNew>(dataSet.Tables[0]);
                    var temp = list.Select(x => x.SQL_ID).Distinct().ToList();                  

                    foreach (var item in temp)
                    {
                        SqlShow info = new SqlShow();
                        info.SQL_ID = item;
                        int countDay = 0;
                        for (int m = 1; m <= 7; m++)
                        {
                            var listDay= list.Where(x => x.END_INTERVAL_TIME > dtNow.AddDays((-1*(i-1)*7)-m)
                                &&x.END_INTERVAL_TIME<=dtNow.AddDays((-1 * (i - 1) * 7) - m+1)
                                &&x.SQL_ID==item);
                            if (listDay!=null&&listDay.Any())
                            {
                                countDay++;
                            }
                        }
                        switch (i)
                        {
                            case 1:
                                info.PARAMENT_NAME = "0-7days";
                                break;
                            case 2:
                                info.PARAMENT_NAME = "7-14days";
                                break;
                            case 3:
                                info.PARAMENT_NAME = "14-21days";
                                break;
                            case 4:
                                info.PARAMENT_NAME = "21-28days";
                                break;
                        }
                        info.PARAMENT_VALUE = countDay;
                        listReturn.Add(info);
                    }

                }
                List<SqlShowCl> listCL = new List<SqlShowCl>();

                var tempCount = listReturn.Select(x => x.SQL_ID).Distinct().ToList();
                foreach (var item in tempCount)
                {
                    var resultList = listReturn.Where(x => x.SQL_ID == item).ToList();
                    SqlShowCl cl = new SqlShowCl();
                    cl.SQL_ID = item;
                    foreach (var result in resultList)
                    {
                        switch (result.PARAMENT_NAME)
                        {
                            case "0-7days":
                                cl.week1 = result.PARAMENT_VALUE;
                                break;
                            case "7-14days":
                                cl.week2 = result.PARAMENT_VALUE;
                                break;
                            case "14-21days":
                                cl.week3 = result.PARAMENT_VALUE;
                                break;
                            case "21-28days":
                                cl.week4 = result.PARAMENT_VALUE;
                                break;
                        }
                    }
                    listCL.Add(cl);
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
