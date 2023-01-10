using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using Org.BouncyCastle.Utilities.Collections;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Data.DataBase.Communicators;
using TAP.Models.SystemBasic;
using TAP.WinService;
using TAP.Base.Office;


namespace ISIA.SERVICE
{
    class SvcProdectPlanExcelInterface : TAP.WinService.ServiceBase
    {
        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            GetExcelData();
        }
        DataTable dtSummaryA = new DataTable();
        DataTable dtSummaryB = new DataTable();
        private DataTable GetAddress()
        {

            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpsb = new StringBuilder();
            tmpsb.Append("SELECT CATEGORY,SUBCATEGORY,NAME,CUSTOM01,CUSTOM02,CUSTOM03 FROM TAPCTCODES ");
            tmpsb.Append(" WHERE CATEGORY='PRODUCTPLAN' AND NAME='PATH'");
            DataTable dtAddress = db.Select(tmpsb.ToString()).Tables[0];
            return dtAddress;

        }
        private void GetExcelData()
        {
            try
            { 
                DateTime time = DateTime.Now;
                DBCommunicator db = new DBCommunicator();
                DataTable dtAddress = GetAddress();
                string file = "";
                string shift = "";
                string savefile = "";
                for (int i = 0; i < dtAddress.Rows.Count; i++)
                {
                    
                    string date = time.ToString("MM");
                    if(date.Substring(0,1) == "0")
                    {
                        date = date.Substring(1, 1);
                    }

                    file = dtAddress.Rows[i]["CUSTOM01"].ToString() + "\\" + date  + dtAddress.Rows[i]["SUBCATEGORY"].ToString();
                    savefile = @"D:\Report\ProductPlan\" + date + "月MES车间目标模板.xlsx";
                    //file = @"D:\Report\ProductPlan\" + date + "月MES车间目标模板.xlsx";
                    try
                    {
                        //link update save;
                        Console.WriteLine("Excel Run");
                        ExcelAutomation excel = new ExcelAutomation(file, 3);
                        Console.WriteLine("Excel Open");
                        excel.BaseSaveFile(savefile);
                        //wb.Save();
                        Console.WriteLine("Excel UpdateLink Save.");
                        //wb.Close(true);
                        //excel.Quit();
                        Console.WriteLine("Excel Exit");
                        Workbook workbook = new Workbook();
                        workbook.LoadFromFile(savefile, true);
                        workbook.ActiveSheetIndex = 0;
                        DataTable dt = new DataTable();
                        List<string> sqllist = new List<string>();
                        foreach(Worksheet wr in workbook.Worksheets)
                        {
                            int columnCount = 0;
                            string category = "";
                            if (wr.Name == "Production MW")
                            {
                                category = "Production M/W"; //1
                            }
                            else if (wr.Name == "Production QTY")
                            {
                                category = "Production QTY"; //2
                            }
                            else if (wr.Name == "Production P3 QTY")
                            {
                                category = "Production P3_QTY"; //3
                            }
                            else if (wr.Name == "Efficienty")
                            {
                                category = "Efficiency"; //3
                            }
                            else if (wr.Name == "Breakage")
                            {
                                category = "Breakage"; //3
                            }
                            else if (wr.Name == "NMR")
                            {
                                category = "NMR"; //3
                            }
                            else if (wr.Name == "RFM")
                            {
                                category = "RFM"; //3
                            }
                            Console.WriteLine(wr.Name);
                            Console.WriteLine(category);
                            List<string> columns = new List<string>();
                            foreach(CellRange cr in wr.Rows)
                            {
                                if(cr.DisplayedText.Contains("WORKSHOP")) // First Row Check
                                {
                                    for (int j = 0; j < cr.CellsCount; j++)
                                    {
                                        if(string.IsNullOrEmpty(cr.Cells[j].DisplayedText))
                                        {
                                            continue;
                                        }
                                        columnCount++;
                                        columns.Add(cr.Cells[j].DisplayedText);
                                    }

                                    continue;
                                }
                                else if(string.IsNullOrEmpty(cr.DisplayedText)) // End Row Check
                                {
                                    continue;
                                }
                                

                                for (int z = 4; z < columns.Count; z++)
                                {
                                    StringBuilder temp = new StringBuilder();
                                    string[] monthdate = columns[z].Split('/');
                                    string tempdate = time.ToString("yyyy") +monthdate[0].PadLeft(2, '0') + monthdate[1].PadLeft(2, '0');

                                    string value = "0";
                                    if(!string.IsNullOrEmpty(cr.Cells[z].DisplayedText))
                                    {
                                        value = cr.Cells[z].DisplayedText;
                                    }
                                    temp.Append("MERGE INTO TAPWIP_PRODUCTIONPLANBYDATE d ");
                                    temp.AppendFormat("USING DUAL ON (d.WORKSHOP = '{0}' AND d.WAFERTYPE = '{1}' AND d.CATEGORY = '{2}' AND d.WORKDATE = '{3}')", cr.Cells[0].DisplayedText, cr.Cells[1].DisplayedText, category, tempdate);
                                    temp.Append("WHEN MATCHED ");
                                    temp.Append("THEN ");
                                    temp.Append("UPDATE SET ");
                                    temp.AppendFormat("d.VALUE = {0}, ", value);
                                    temp.AppendFormat("d.DESCRIPTION = '{0}', ", cr.Cells[3].DisplayedText);
                                    temp.Append("d.MODIFYTIME = TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS'), ");
                                    temp.Append("d.MODIFIER = 'Service' ");
                                    temp.Append("WHEN NOT MATCHED ");
                                    temp.Append("THEN ");
                                    temp.Append("INSERT( ");
                                    temp.Append("WORKSHOP, WAFERTYPE, BUSBAR, CATEGORY, WORKDATE, VERSION, VALUE, SITEID, DESCRIPTION, CREATOR, CREATETIME, MODIFIER, MODIFYTIME ) ");
                                    temp.AppendFormat("VALUES('{0}', '{1}', '{2}', '{3}', '{4}', 0, {5}, '1013', '{6}', 'Service', TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS'), 'Service', TO_CHAR(SYSDATE,'YYYYMMDDHH24MISS')) ", cr.Cells[0].DisplayedText, cr.Cells[1].DisplayedText,
                                        cr.Cells[2].DisplayedText, category, tempdate, value, cr.Cells[3].DisplayedText);
                                    sqllist.Add(temp.ToString());

                                    //Console.WriteLine(temp.ToString());
                                }
                            }
                        }
                        Console.WriteLine(string.Format("Data Mege Count : {0}", db.Save(sqllist)));
                        System.IO.File.Delete(savefile);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        this.SaveLog(ServiceBase.ERROR_LOG, "SvcProdectPlanExcelInterface", string.Format("Error:{2}", ex.ToString()));
                        continue;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}