using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Data.DataBase.Communicators;
using TAP.Models.SystemBasic;

namespace ISIA.SERVICE
{
    class SvcDailyFileInterface : TAP.WinService.ServiceBase
    {
        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            GetCSVData();
        }
        DataTable dtSummaryA = new DataTable();
        DataTable dtSummaryB = new DataTable();
        private DataTable GetAddress()
        {

            DBCommunicator db = new DBCommunicator();
            StringBuilder tmpsb = new StringBuilder();
            tmpsb.Append("SELECT CATEGORY,SUBCATEGORY,NAME,CUSTOM01,CUSTOM02,CUSTOM03 FROM TAPCTCODES ");
            tmpsb.Append(" WHERE CATEGORY='ADDRESS' AND NAME='TS'");
            DataTable dtAddress = db.Select(tmpsb.ToString()).Tables[0];
            return dtAddress;

        }
        private void GetCSVData()
        {
            try
            {

                //实例化一个datatable用来存储数据
                dtSummaryA = new DataTable();
                dtSummaryA.Columns.Add("WORKSHOP", typeof(string));
                dtSummaryA.Columns.Add("LINE", typeof(string));
                dtSummaryA.Columns.Add("PROCESS", typeof(string));
                dtSummaryA.Columns.Add("EQUIPMENT", typeof(string));
                dtSummaryA.Columns.Add("SHIFT", typeof(string));
                dtSummaryB = new DataTable();
                dtSummaryB.Columns.Add("WORKSHOP", typeof(string));
                dtSummaryB.Columns.Add("LINE", typeof(string));
                dtSummaryB.Columns.Add("PROCESS", typeof(string));
                dtSummaryB.Columns.Add("EQUIPMENT", typeof(string));
                dtSummaryB.Columns.Add("SHIFT", typeof(string));
                //dtSummary.Columns.Add("TYPE", typeof(string));
                bool isFirstA = true;
                bool isFirstB = true;
                DateTime time = DateTime.Now;
                DataTable dtAddress = GetAddress();
                string file = "";
                string shift = "";
                for (int i = 0; i < dtAddress.Rows.Count; i++)
                {
                    if (time.Hour == 8)
                    {
                        string date = time.AddDays(-1).ToString("yyyyMMdd");
                        shift = "N";
                        file = @"\\" + dtAddress.Rows[i]["CUSTOM02"].ToString() + "\\Data\\" + date + "\\19\\Sum19.csv";
                    }
                    else if (time.Hour == 20)
                    {
                        string date = time.ToString("yyyyMMdd");
                        shift = "D";
                        file = @"\\" + dtAddress.Rows[i]["CUSTOM02"].ToString() + "\\Data\\" + date + "\\7\\Sum7.csv";
                    }
                    try
                    {
                        //文件流读取
                        System.IO.FileStream fs = new FileStream(@file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        System.IO.StreamReader sr = new System.IO.StreamReader(fs, Encoding.GetEncoding("gb2312"));

                        string tempText = "";
                        string[] arrs = sr.ReadLine().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        //一般第一行为标题，所以取出来作为标头
                        if (isFirstA || isFirstB)
                        {
                            foreach (string str in arrs)
                            {
                                if (dtAddress.Rows[i]["CUSTOM03"].ToString() == "A面")
                                {
                                    dtSummaryA.Columns.Add(str);
                                    isFirstA = false;
                                }
                                else
                                {
                                    dtSummaryB.Columns.Add(str);
                                    isFirstB = false;
                                }
                            }
                        }
                        while ((tempText = sr.ReadLine()) != null)
                        {
                            if (sr.EndOfStream)
                            {
                                string[] arr = tempText.Split(new char[] { ',' }, StringSplitOptions.None);
                                //从第二行开始添加到datatable数据行
                                DataRow dr = null;
                                DataTable sumDt = new DataTable();
                                if (dtAddress.Rows[i]["CUSTOM03"].ToString() == "A面")
                                {
                                    dr = dtSummaryA.NewRow();
                                    sumDt = dtSummaryA;
                                }
                                else
                                {
                                    dr = dtSummaryB.NewRow();
                                    sumDt = dtSummaryB;
                                }
                                dr["WORKSHOP"] = dtAddress.Rows[i]["SUBCATEGORY"].ToString();
                                dr["LINE"] = dtAddress.Rows[i]["NAME"].ToString();
                                dr["PROCESS"] = dtAddress.Rows[i]["NAME"].ToString();
                                dr["EQUIPMENT"] = dtAddress.Rows[i]["CUSTOM01"].ToString();
                                dr["SHIFT"] = shift;
                                for (int j = 5; j < sumDt.Columns.Count; j++)
                                {
                                    dr[j] = j - 5 < arr.Length ? arr[j - 5] : "";
                                }
                                if (dtAddress.Rows[i]["CUSTOM03"].ToString() == "A面")
                                {
                                    dtSummaryA.Rows.Add(dr);
                                }
                                else
                                {
                                    dtSummaryB.Rows.Add(dr);
                                }
                            }
                        }
                        //关闭流
                        sr.Close(); fs.Close();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                if (dtSummaryA.Rows.Count > 1) SaveTSSummaryA_Data(dtSummaryA);
                if (dtSummaryB.Rows.Count > 1) SaveTSSummaryB_Data(dtSummaryB);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable CreateTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("WORKSHOP", typeof(string));
            table.Columns.Add("WAFERTYPE", typeof(string));
            table.Columns.Add("CATEGORY", typeof(string));
            table.Columns.Add("BUSBAR", typeof(string));
            table.Columns.Add("WORKDATE", typeof(string));
            table.Columns.Add("VERSION", typeof(string));
            table.Columns.Add("VALUE", typeof(double));
            table.Columns.Add("UNITID", typeof(string));
            table.Columns.Add("ACTIVITY", typeof(string));
            table.Columns.Add("PREVACTIVITY", typeof(string));
            table.Columns.Add("CUSTOMACTIVITY", typeof(string));
            table.Columns.Add("PREVCUSTOMACTIVITY", typeof(string));
            table.Columns.Add("ISUSABLE", typeof(string));
            table.Columns.Add("SITEID", typeof(string));
            table.Columns.Add("DESCRIPTION", typeof(string));
            table.Columns.Add("REASONCODE", typeof(string));
            table.Columns.Add("COMMENTS", typeof(string));
            table.Columns.Add("CREATOR", typeof(string));
            table.Columns.Add("CREATETIME", typeof(DateTime));
            table.Columns.Add("MODIFIER", typeof(string));
            table.Columns.Add("MODIFYTIME", typeof(DateTime));
            table.Columns.Add("LASTEVENTTIME", typeof(DateTime));
            table.Columns.Add("TID", typeof(string));

            return table;
        }
        private void SaveTSSummaryA_Data(DataTable dtSum)
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                List<string> SqlList = new List<string>();
                for (int i = 0; i < dtSum.Rows.Count; i++)
                {
                    for (int j = 0; j < dtSum.Columns.Count; j++)
                    {
                        if (string.IsNullOrEmpty(dtSum.Rows[i][j].ToString()))
                            dtSum.Rows[i][j] = 0;
                    }
                }
                foreach (DataRow dr in dtSum.Rows)
                {
                    StringBuilder tmpsb = new StringBuilder();
#if MSSQL
                    tmpsb.Append("INSERT INTO  TAPEQP_TS_A_LOADFILE (");
                    tmpsb.Append("WORKSHOP,LINE,PROCESS,EQUIPMENT,DATE,SHIFT,TOTAL");
                    tmpsb.Append(",INTERMEDIATE_FINE_GRID_S_ROOT,SINGLE_BORDER_GRILLE,SINGLE_LEAKAGE_SLURRY,EDGE_DEFICIENCY,DE_CRYSTALLIZATION");
                    tmpsb.Append(",INTERMEDIATE_FINE_GRID,BORDER_FINE_GRILLE,INTERMEDIATE_MAIN_GRID,IMAGINARY_MAIN_GRID,BORDER_MAIN_GRID");
                    tmpsb.Append(",ANGULAR_BORDER_BREAK,CLEAR_FALSE_IMPRESSION,SLIGHT_FALSE_IMPRESSION,EXTRA_THICKNESS_FINE_GRILLE,ROUGH_BORDER_GRATING,FINE_FENCE_TURNS_WHITE");
                    tmpsb.Append(",COARSE_KNOT_OF_THREAD,NOTICEABLE_LEAKAGE,MARK_POINT_LEAKAGE,HAIR_LOSS_SLURRY,SCRATCH,DISCOLORATION,OIL_STAIN,DIRT");
                    tmpsb.Append(",CHROMATIC_DIFFERENCE,DEAD_CENTER,ROUNDABOUT,IMPRINT_SHIP,WHITE_SPOT,BATTERY_SLICE_DIMENSIONS,EDGE_BATTERY_WAFER,GRID_LINE_INFORMATION,THICKNESS_FENCE");
                    tmpsb.Append(",COLOR_DETECTION,DARKEN,QUALITY_OTHER,QUALITY_MIXED,QUALITY_A,QUALITY_B,QUALITY_C");
                    tmpsb.Append(",COLOUR,COLOUR_UW,COLOUR_ULL,COLOUR_UL,COLOUR_A2,COLOUR_A1,COLOUR_UD,COLOUR_UD1,INSERT_TIME");
                    tmpsb.Append(")VALUES(");
                    tmpsb.AppendFormat("'{0}',", dr["WORKSHOP"].ToString());
                    tmpsb.AppendFormat("'{0}',", dr["WORKSHOP"].ToString().Substring(0, 1));
                    tmpsb.AppendFormat("'{0}',", dr["PROCESS"].ToString());
                    tmpsb.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString());
                    //tmpsb.AppendFormat("'{0}',", dr["时间"].ToString());
                    tmpsb.AppendFormat("'{0}',", Convert.ToDateTime(dr["时间"].ToString()).ToString("yyyyMMdd"));
                    tmpsb.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpsb.AppendFormat("{0},", dr["总数"].ToString());
                    tmpsb.AppendFormat("{0},", dr["中间细栅断-单根"].ToString());
                    tmpsb.AppendFormat("{0},", dr["单根边框细栅断"].ToString());
                    tmpsb.AppendFormat("{0},", dr["单个漏浆"].ToString());
                    tmpsb.AppendFormat("{0},", dr["崩边缺角"].ToString());
                    tmpsb.AppendFormat("{0},", dr["脱晶"].ToString());
                    tmpsb.AppendFormat("{0},", dr["中间细栅断"].ToString());
                    tmpsb.AppendFormat("{0},", dr["边框细栅断"].ToString());
                    tmpsb.AppendFormat("{0},", dr["中间主栅断"].ToString());
                    tmpsb.AppendFormat("{0},", dr["虚断主栅断"].ToString());
                    tmpsb.AppendFormat("{0},", dr["边框主栅断"].ToString());
                    tmpsb.AppendFormat("{0},", dr["角边框断"].ToString());
                    tmpsb.AppendFormat("{0},", dr["明显虚印"].ToString());
                    tmpsb.AppendFormat("{0},", dr["轻微虚印"].ToString());
                    tmpsb.AppendFormat("{0},", dr["细栅特粗"].ToString());
                    tmpsb.AppendFormat("{0},", dr["边框粗栅"].ToString());
                    tmpsb.AppendFormat("{0},", dr["细栅发白"].ToString());
                    tmpsb.AppendFormat("{0},", dr["线粗结点"].ToString());
                    tmpsb.AppendFormat("{0},", dr["明显漏浆"].ToString());
                    tmpsb.AppendFormat("{0},", dr["Mark点漏浆"].ToString());
                    tmpsb.AppendFormat("{0},", dr["拉毛漏浆"].ToString());
                    tmpsb.AppendFormat("{0},", dr["划痕"].ToString());
                    tmpsb.AppendFormat("{0},", dr["色斑"].ToString());
                    tmpsb.AppendFormat("{0},", dr["油污"].ToString());
                    tmpsb.AppendFormat("{0},", dr["脏污"].ToString());
                    tmpsb.AppendFormat("{0},", dr["色差"].ToString());
                    tmpsb.AppendFormat("{0},", dr["卡点"].ToString());
                    tmpsb.AppendFormat("{0},", dr["绕度"].ToString());
                    tmpsb.AppendFormat("{0},", dr["舟印"].ToString());
                    tmpsb.AppendFormat("{0},", dr["白点"].ToString());
                    tmpsb.AppendFormat("{0},", dr["电池片尺寸"].ToString());
                    tmpsb.AppendFormat("{0},", dr["电池片边角"].ToString());
                    tmpsb.AppendFormat("{0},", dr["栅线信息"].ToString());
                    tmpsb.AppendFormat("{0},", dr["栅粗"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色检测"].ToString());
                    tmpsb.AppendFormat("{0},", dr["发暗"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|Other"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|Mixed"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|A"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|B"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|C"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色|"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色|UW"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色|ULL"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色|UL"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色|A2"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色|A1"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色|UD"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色|UD1"].ToString());
                    tmpsb.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
#endif
#if ORACLE
                    tmpsb.Append("INSERT INTO  TAPEQP_TS_A_LOADFILE (");
                    tmpsb.Append("WORKSHOP,LINE,PROCESS,EQUIPMENT,\"DATE\",SHIFT,TOTAL");
                    tmpsb.Append(",INTERMEDIATE_FINE_GRID_S_ROOT,SINGLE_BORDER_GRILLE,SINGLE_LEAKAGE_SLURRY,EDGE_DEFICIENCY,DE_CRYSTALLIZATION");
                    tmpsb.Append(",INTERMEDIATE_FINE_GRID,BORDER_FINE_GRILLE,INTERMEDIATE_MAIN_GRID,IMAGINARY_MAIN_GRID,BORDER_MAIN_GRID");
                    tmpsb.Append(",ANGULAR_BORDER_BREAK,CLEAR_FALSE_IMPRESSION,SLIGHT_FALSE_IMPRESSION,EXTRA_THICKNESS_FINE_GRILLE,ROUGH_BORDER_GRATING,FINE_FENCE_TURNS_WHITE");
                    tmpsb.Append(",COARSE_KNOT_OF_THREAD,NOTICEABLE_LEAKAGE,MARK_POINT_LEAKAGE,HAIR_LOSS_SLURRY,SCRATCH,DISCOLORATION,OIL_STAIN,DIRT");
                    tmpsb.Append(",CHROMATIC_DIFFERENCE,DEAD_CENTER,ROUNDABOUT,IMPRINT_SHIP,WHITE_SPOT,BATTERY_SLICE_DIMENSIONS,EDGE_BATTERY_WAFER,GRID_LINE_INFORMATION,THICKNESS_FENCE");
                    tmpsb.Append(",COLOR_DETECTION,DARKEN,QUALITY_OTHER,QUALITY_MIXED,QUALITY_A,QUALITY_B,QUALITY_C");
                    tmpsb.Append(",COLOUR,COLOUR_UW,COLOUR_ULL,COLOUR_UL,COLOUR_A2,COLOUR_A1,COLOUR_UD,COLOUR_UD1,INSERT_TIME");
                    tmpsb.Append(")VALUES(");
                    tmpsb.AppendFormat("'{0}',", dr["WORKSHOP"].ToString());
                    tmpsb.AppendFormat("'{0}',", dr["WORKSHOP"].ToString().Substring(0, 1));
                    tmpsb.AppendFormat("'{0}',", dr["PROCESS"].ToString());
                    tmpsb.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString());
                    //tmpsb.AppendFormat("'{0}',", dr["时间"].ToString());
                    tmpsb.AppendFormat("'{0}',", Convert.ToDateTime(dr["时间"].ToString()).ToString("yyyyMMdd"));
                    tmpsb.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpsb.AppendFormat("{0},", dr["总数"].ToString());
                    tmpsb.AppendFormat("{0},", dr["中间细栅断-单根"].ToString());
                    tmpsb.AppendFormat("{0},", dr["单根边框细栅断"].ToString());
                    tmpsb.AppendFormat("{0},", dr["单个漏浆"].ToString());
                    tmpsb.AppendFormat("{0},", dr["崩边缺角"].ToString());
                    tmpsb.AppendFormat("{0},", dr["脱晶"].ToString());
                    tmpsb.AppendFormat("{0},", dr["中间细栅断"].ToString());
                    tmpsb.AppendFormat("{0},", dr["边框细栅断"].ToString());
                    tmpsb.AppendFormat("{0},", dr["中间主栅断"].ToString());
                    tmpsb.AppendFormat("{0},", dr["虚断主栅断"].ToString());
                    tmpsb.AppendFormat("{0},", dr["边框主栅断"].ToString());
                    tmpsb.AppendFormat("{0},", dr["角边框断"].ToString());
                    tmpsb.AppendFormat("{0},", dr["明显虚印"].ToString());
                    tmpsb.AppendFormat("{0},", dr["轻微虚印"].ToString());
                    tmpsb.AppendFormat("{0},", dr["细栅特粗"].ToString());
                    tmpsb.AppendFormat("{0},", dr["边框粗栅"].ToString());
                    tmpsb.AppendFormat("{0},", dr["细栅发白"].ToString());
                    tmpsb.AppendFormat("{0},", dr["线粗结点"].ToString());
                    tmpsb.AppendFormat("{0},", dr["明显漏浆"].ToString());
                    tmpsb.AppendFormat("{0},", dr["Mark点漏浆"].ToString());
                    tmpsb.AppendFormat("{0},", dr["拉毛漏浆"].ToString());
                    tmpsb.AppendFormat("{0},", dr["划痕"].ToString());
                    tmpsb.AppendFormat("{0},", dr["色斑"].ToString());
                    tmpsb.AppendFormat("{0},", dr["油污"].ToString());
                    tmpsb.AppendFormat("{0},", dr["脏污"].ToString());
                    tmpsb.AppendFormat("{0},", dr["色差"].ToString());
                    tmpsb.AppendFormat("{0},", dr["卡点"].ToString());
                    tmpsb.AppendFormat("{0},", dr["绕度"].ToString());
                    tmpsb.AppendFormat("{0},", dr["舟印"].ToString());
                    tmpsb.AppendFormat("{0},", dr["白点"].ToString());
                    tmpsb.AppendFormat("{0},", dr["电池片尺寸"].ToString());
                    tmpsb.AppendFormat("{0},", dr["电池片边角"].ToString());
                    tmpsb.AppendFormat("{0},", dr["栅线信息"].ToString());
                    tmpsb.AppendFormat("{0},", dr["栅粗"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色检测"].ToString());
                    tmpsb.AppendFormat("{0},", dr["发暗"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|Other"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|Mixed"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|A"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|B"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|C"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色|"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色|UW"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色|ULL"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色|UL"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色|A2"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色|A1"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色|UD"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色|UD1"].ToString());
                    tmpsb.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
#endif
                    SqlList.Add(tmpsb.ToString());
                }
                db.Save(SqlList);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void SaveTSSummaryB_Data(DataTable dtSum)
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                List<string> SqlList = new List<string>();
                for (int i = 0; i < dtSum.Rows.Count; i++)
                {
                    for (int j = 0; j < dtSum.Columns.Count; j++)
                    {
                        if (string.IsNullOrEmpty(dtSum.Rows[i][j].ToString()))
                            dtSum.Rows[i][j] = 0;
                    }
                }
                foreach (DataRow dr in dtSum.Rows)
                {
                    StringBuilder tmpsb = new StringBuilder();
#if MSSQL
                    tmpsb.Append("INSERT INTO  TAPEQP_TS_B_LOADFILE ( ");
                    tmpsb.Append("WORKSHOP,LINE,PROCESS,EQUIPMENT,DATE,SHIFT,TOTAL");
                    tmpsb.Append(",EDGE_DEFICIENCY,DE_CRYSTALLIZATION,NODE,COARSE_GRILLE,SINGLE_STRAND_THICK_THREAD,BROKEN_FENCE,FALSE_SEAL");
                    tmpsb.Append(",ELECTRODE_ABSENCE,ELECTRODE_DEFLECTION,ELECTRODE_LEAKAGE,ELECTRODE_OXIDATION,LASER_LITHOGRAPHY,LEFT_RIGHT_BORDER_LINES");
                    tmpsb.Append(",CHAMFERED_GRID_LINE_DEFLECTION,DEAD_CENTER,DIRT,CHROMATIC_DIFFERENCE,LEAK_STARCH,LINE_MARK,SCRATCH");
                    tmpsb.Append(",DISCOLORATION,WHITE_SPOT,C_RATED_FILM,BATTERY_SLICE_DIMENSIONS,EDGE_BATTERY_WAFER,GRID_LINE_INFORMATION");
                    tmpsb.Append(",COLOR_DETECTION,LASER_DEFLECTION,LASER_OFFSET_MIDDLE,LASER_DEFLECTION_ROTATION,QUALITY_OTHER,QUALITY_MIXED,QUALITY_A,QUALITY_B,INSERT_TIME");
                    tmpsb.Append(") VALUES(");
                    tmpsb.AppendFormat("'{0}',", dr["WORKSHOP"].ToString());
                    tmpsb.AppendFormat("'{0}',", dr["WORKSHOP"].ToString().Substring(0, 1));
                    tmpsb.AppendFormat("'{0}',", dr["PROCESS"].ToString());
                    tmpsb.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString());
                    tmpsb.AppendFormat("'{0}',", Convert.ToDateTime(dr["时间"].ToString()).ToString("yyyyMMdd"));
                    tmpsb.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpsb.AppendFormat("{0},", dr["总数"].ToString());
                    tmpsb.AppendFormat("{0},", dr["崩边缺角"].ToString());
                    tmpsb.AppendFormat("{0},", dr["脱晶"].ToString());
                    tmpsb.AppendFormat("{0},", dr["节点"].ToString());
                    tmpsb.AppendFormat("{0},", dr["粗栅"].ToString());
                    tmpsb.AppendFormat("{0},", dr["单根粗线"].ToString());
                    tmpsb.AppendFormat("{0},", dr["断栅"].ToString());
                    tmpsb.AppendFormat("{0},", dr["虚印"].ToString());
                    tmpsb.AppendFormat("{0},", dr["电极缺失"].ToString());
                    tmpsb.AppendFormat("{0},", dr["电极偏移"].ToString());
                    tmpsb.AppendFormat("{0},", dr["电极漏浆"].ToString());
                    tmpsb.AppendFormat("{0},", dr["电极氧化"].ToString());
                    tmpsb.AppendFormat("{0},", dr["激光虚刻"].ToString());
                    tmpsb.AppendFormat("{0},", dr["左右边框粗线"].ToString());
                    tmpsb.AppendFormat("{0},", dr["倒角栅线偏移"].ToString());
                    tmpsb.AppendFormat("{0},", dr["卡点"].ToString());
                    tmpsb.AppendFormat("{0},", dr["脏污"].ToString());
                    tmpsb.AppendFormat("{0},", dr["色差"].ToString());
                    tmpsb.AppendFormat("{0},", dr["漏浆"].ToString());
                    tmpsb.AppendFormat("{0},", dr["线痕"].ToString());
                    tmpsb.AppendFormat("{0},", dr["划痕"].ToString());
                    tmpsb.AppendFormat("{0},", dr["色斑"].ToString());
                    tmpsb.AppendFormat("{0},", dr["白点"].ToString());
                    tmpsb.AppendFormat("{0},", dr["C级片"].ToString());
                    tmpsb.AppendFormat("{0},", dr["电池片尺寸"].ToString());
                    tmpsb.AppendFormat("{0},", dr["电池片边角"].ToString());
                    tmpsb.AppendFormat("{0},", dr["栅线信息"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色检测"].ToString());
                    tmpsb.AppendFormat("{0},", dr["激光偏移"].ToString());
                    tmpsb.AppendFormat("{0},", dr["激光偏移-中部"].ToString());
                    tmpsb.AppendFormat("{0},", dr["激光偏移-旋转"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|Other"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|Mixed"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|A"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|B"].ToString());
                    tmpsb.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
#endif
#if ORACLE
                    tmpsb.Append("INSERT INTO  TAPEQP_TS_B_LOADFILE ( ");
                    tmpsb.Append("WORKSHOP,LINE,PROCESS,EQUIPMENT,\"DATE\",SHIFT,TOTAL");
                    tmpsb.Append(",EDGE_DEFICIENCY,DE_CRYSTALLIZATION,NODE,COARSE_GRILLE,SINGLE_STRAND_THICK_THREAD,BROKEN_FENCE,FALSE_SEAL");
                    tmpsb.Append(",ELECTRODE_ABSENCE,ELECTRODE_DEFLECTION,ELECTRODE_LEAKAGE,ELECTRODE_OXIDATION,LASER_LITHOGRAPHY,LEFT_RIGHT_BORDER_LINES");
                    tmpsb.Append(",CHAMFERED_GRID_LINE_DEFLECTION,DEAD_CENTER,DIRT,CHROMATIC_DIFFERENCE,LEAK_STARCH,LINE_MARK,SCRATCH");
                    tmpsb.Append(",DISCOLORATION,WHITE_SPOT,C_RATED_FILM,BATTERY_SLICE_DIMENSIONS,EDGE_BATTERY_WAFER,GRID_LINE_INFORMATION");
                    tmpsb.Append(",COLOR_DETECTION,LASER_DEFLECTION,LASER_OFFSET_MIDDLE,LASER_DEFLECTION_ROTATION,QUALITY_OTHER,QUALITY_MIXED,QUALITY_A,QUALITY_B,INSERT_TIME");
                    tmpsb.Append(") VALUES(");
                    tmpsb.AppendFormat("'{0}',", dr["WORKSHOP"].ToString());
                    tmpsb.AppendFormat("'{0}',", dr["WORKSHOP"].ToString().Substring(0, 1));
                    tmpsb.AppendFormat("'{0}',", dr["PROCESS"].ToString());
                    tmpsb.AppendFormat("'{0}',", dr["EQUIPMENT"].ToString());
                    tmpsb.AppendFormat("'{0}',", Convert.ToDateTime(dr["时间"].ToString()).ToString("yyyyMMdd"));
                    tmpsb.AppendFormat("'{0}',", dr["SHIFT"].ToString());
                    tmpsb.AppendFormat("{0},", dr["总数"].ToString());
                    tmpsb.AppendFormat("{0},", dr["崩边缺角"].ToString());
                    tmpsb.AppendFormat("{0},", dr["脱晶"].ToString());
                    tmpsb.AppendFormat("{0},", dr["节点"].ToString());
                    tmpsb.AppendFormat("{0},", dr["粗栅"].ToString());
                    tmpsb.AppendFormat("{0},", dr["单根粗线"].ToString());
                    tmpsb.AppendFormat("{0},", dr["断栅"].ToString());
                    tmpsb.AppendFormat("{0},", dr["虚印"].ToString());
                    tmpsb.AppendFormat("{0},", dr["电极缺失"].ToString());
                    tmpsb.AppendFormat("{0},", dr["电极偏移"].ToString());
                    tmpsb.AppendFormat("{0},", dr["电极漏浆"].ToString());
                    tmpsb.AppendFormat("{0},", dr["电极氧化"].ToString());
                    tmpsb.AppendFormat("{0},", dr["激光虚刻"].ToString());
                    tmpsb.AppendFormat("{0},", dr["左右边框粗线"].ToString());
                    tmpsb.AppendFormat("{0},", dr["倒角栅线偏移"].ToString());
                    tmpsb.AppendFormat("{0},", dr["卡点"].ToString());
                    tmpsb.AppendFormat("{0},", dr["脏污"].ToString());
                    tmpsb.AppendFormat("{0},", dr["色差"].ToString());
                    tmpsb.AppendFormat("{0},", dr["漏浆"].ToString());
                    tmpsb.AppendFormat("{0},", dr["线痕"].ToString());
                    tmpsb.AppendFormat("{0},", dr["划痕"].ToString());
                    tmpsb.AppendFormat("{0},", dr["色斑"].ToString());
                    tmpsb.AppendFormat("{0},", dr["白点"].ToString());
                    tmpsb.AppendFormat("{0},", dr["C级片"].ToString());
                    tmpsb.AppendFormat("{0},", dr["电池片尺寸"].ToString());
                    tmpsb.AppendFormat("{0},", dr["电池片边角"].ToString());
                    tmpsb.AppendFormat("{0},", dr["栅线信息"].ToString());
                    tmpsb.AppendFormat("{0},", dr["颜色检测"].ToString());
                    tmpsb.AppendFormat("{0},", dr["激光偏移"].ToString());
                    tmpsb.AppendFormat("{0},", dr["激光偏移-中部"].ToString());
                    tmpsb.AppendFormat("{0},", dr["激光偏移-旋转"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|Other"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|Mixed"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|A"].ToString());
                    tmpsb.AppendFormat("{0},", dr["质量|B"].ToString());
                    tmpsb.AppendFormat("'{0}')", DateTime.Now.ToString("yyyyMMddHHmmss"));
#endif
                    SqlList.Add(tmpsb.ToString());
                }
                db.Save(SqlList);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}