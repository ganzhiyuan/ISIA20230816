using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Models.SystemBasic;
using TAP.Base.Communication;
using TAP.Data.DataBase.Communicators;
using System.Data;
using System.Text.RegularExpressions;

namespace ISIA.SERVICE
{
    class SvcPMSchedule : TAP.WinService.ServiceBase
    {
        private const int weekly = 7;
        private const int WeekNumber = 10;
        private const int MonthsNumber = 31;
        string InsertPmTime;
        int num = 1;
        bool _break = false;



        protected override void StartMainProcess(SystemBasicDefaultInfo defaultInfo)
        {
            if (DateTime.Now.Day == Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM-01")).AddDays(-1).Day)
            {
                AddPMSCHEDULE();
            }

        }

        private void AddPMSCHEDULE()
        {
            try
            {
                DBCommunicator db = new DBCommunicator();
                StringBuilder strSql = new StringBuilder();
                DataTable dataTable = new DataTable();

                dataTable = db.Select("SELECT * FROM TAPFTPMSCHEDULE").Tables[0];
                string Month = (DateTime.Now.Month).ToString().PadLeft(2, '0');
                string Year = DateTime.Now.Year.ToString();

                strSql.AppendFormat("SELECT * FROM(SELECT A.*, ROW_NUMBER() OVER(PARTITION BY MAINEQUIPMENT, PMSCHEDULE ORDER BY PMTIME DESC) AS DISORDER FROM(SELECT * FROM TAPFTPMSCHEDULE  WHERE PMTIME LIKE '{0}%')A )T WHERE T.DISORDER = 1", Year + Month);

                DataTable PMSCHEDULEDT = db.Select(strSql.ToString()).Tables[0];

                if (PMSCHEDULEDT.Rows.Count > 0)
                {
                    List<string> tmpSaveList = new List<string>();

                    foreach (DataRow dr in PMSCHEDULEDT.Rows)
                    {
                        strSql.Clear();
                        DateTime dateTime = DateTime.ParseExact(dr["PMTIME"].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

                        if (dr["PMSCHEDULE"].ToString().Contains("WEEKLY"))
                        {
                            //DateTime.Now.DayOfWeek 判断是否是周末
                            //case "Saturday": weekstr = "星期六"; break;
                            //case "Sunday": weekstr = "星期日"; break;

                            if (Regex.IsMatch(dr["PMSCHEDULE"].ToString().Substring(0, 1), @"^[+-]?\d*[.]?\d*$"))
                            {

                                num = int.Parse(dr["PMSCHEDULE"].ToString().Substring(0, 1)) * weekly;

                                if (num >= MonthsNumber)
                                {
                                    if (dateTime.AddDays(num).DayOfWeek.ToString().ToUpper() == "SATURDAY")
                                    {
                                        InsertPmTime = dateTime.AddDays(num).AddDays(2).ToString("yyyyMMdd");
                                    }
                                    else if (dateTime.AddDays(num).DayOfWeek.ToString().ToUpper() == "SUNDAY")
                                    {

                                        InsertPmTime = dateTime.AddDays(num).AddDays(1).ToString("yyyyMMdd");
                                    }
                                    else
                                    {
                                        InsertPmTime = dateTime.AddDays(num).ToString("yyyyMMdd");
                                    }
                                    strSql.Clear();
                                    strSql.Append("INSERT INTO TAPFTPMSCHEDULE(REGION,MAINEQUIPMENT,PMSCHEDULE,AREA,BAY,DESCRIPTION,EQUIPMENT,FACILITY,INSERTTIME,INSERTUSER,LINE,UPDATETIME,UPDATEUSER,PMTIME)  VALUES('");
                                    strSql.Append(dr["REGION"].ToString() + "','" + dr["MAINEQUIPMENT"].ToString() + "','" + dr["PMSCHEDULE"].ToString() + "','");
                                    strSql.Append(dr["AREA"].ToString() + "','" + dr["BAY"].ToString() + "','" + dr["DESCRIPTION"].ToString() + "','" + dr["EQUIPMENT"].ToString() + "','");
                                    strSql.Append(dr["FACILITY"].ToString() + "','" + DateTime.Now.ToString("yyyyMMdd") + "','" + dr["INSERTUSER"].ToString() + "','" + dr["LINE"].ToString() + "','");
                                    strSql.Append(dr["UPDATETIME"].ToString() + "','" + dr["UPDATEUSER"].ToString() + "','");
                                    strSql.Append(InsertPmTime + "')");

                                    if (dataTable.Select("PMTIME='" + InsertPmTime + "'" + "and MAINEQUIPMENT = '" + dr["MAINEQUIPMENT"].ToString() + "'" + " and AREA ='" + dr["AREA"].ToString() + "'" + " and LINE = '" + dr["LINE"].ToString() + "'").Length == 0)
                                    {
                                        tmpSaveList.Add(strSql.ToString());

                                    }
                                    _break = true;
                                }
                                else
                                {
                                    for (int i = 1; i < WeekNumber; i++)
                                    {
                                        if (dateTime.AddDays(num).Month == DateTime.Now.AddMonths(1).Month)
                                        {
                                            if (dateTime.AddDays(num).DayOfWeek.ToString().ToUpper() == "SATURDAY")
                                            {
                                                InsertPmTime = dateTime.AddDays(num).AddDays(2).ToString("yyyyMMdd");
                                            }
                                            else if (dateTime.AddDays(num).DayOfWeek.ToString().ToUpper() == "SUNDAY")
                                            {

                                                InsertPmTime = dateTime.AddDays(num * i).AddDays(1).ToString("yyyyMMdd");
                                            }
                                            else
                                            {
                                                InsertPmTime = dateTime.AddDays(num).ToString("yyyyMMdd");
                                            }
                                            strSql.Clear();
                                            strSql.Append("INSERT INTO TAPFTPMSCHEDULE(REGION,MAINEQUIPMENT,PMSCHEDULE,AREA,BAY,DESCRIPTION,EQUIPMENT,FACILITY,INSERTTIME,INSERTUSER,LINE,UPDATETIME,UPDATEUSER,PMTIME)  VALUES('");
                                            strSql.Append(dr["REGION"].ToString() + "','" + dr["MAINEQUIPMENT"].ToString() + "','" + dr["PMSCHEDULE"].ToString() + "','");
                                            strSql.Append(dr["AREA"].ToString() + "','" + dr["BAY"].ToString() + "','" + dr["DESCRIPTION"].ToString() + "','" + dr["EQUIPMENT"].ToString() + "','");
                                            strSql.Append(dr["FACILITY"].ToString() + "','" + DateTime.Now.ToString("yyyyMMdd") + "','" + dr["INSERTUSER"].ToString() + "','" + dr["LINE"].ToString() + "','");
                                            strSql.Append(dr["UPDATETIME"].ToString() + "','" + dr["UPDATEUSER"].ToString() + "','");
                                            strSql.Append(InsertPmTime + "')");

                                            if (dataTable.Select("PMTIME='" + InsertPmTime + "'" + "and MAINEQUIPMENT = '" + dr["MAINEQUIPMENT"].ToString() + "'" + " and AREA ='" + dr["AREA"].ToString() + "'" + " and LINE = '" + dr["LINE"].ToString() + "'").Length == 0)
                                            {
                                                tmpSaveList.Add(strSql.ToString());

                                            }
                                            dateTime = DateTime.ParseExact(InsertPmTime, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

                                        }
                                        else if (dateTime.AddDays(num).Month == DateTime.Now.AddMonths(2).Month || dateTime.AddDays(num).Month == DateTime.Now.AddMonths(3).Month)
                                        {
                                            _break = true;
                                            break;
                                        }
                                        else
                                        {
                                            dateTime = dateTime.AddDays(num);
                                        }

                                    }

                                }

                            }
                            else
                            {
                                for (int i = 1; i < WeekNumber; i++)
                                {
                                    if (dateTime.AddDays(weekly).Month == DateTime.Now.AddMonths(1).Month)
                                    {
                                        if (dateTime.AddDays(weekly).DayOfWeek.ToString().ToUpper() == "SATURDAY")
                                        {

                                            InsertPmTime = dateTime.AddDays(weekly).AddDays(2).ToString("yyyyMMdd");
                                        }
                                        else if (dateTime.AddDays(weekly).DayOfWeek.ToString().ToUpper() == "SUNDAY")
                                        {

                                            InsertPmTime = dateTime.AddDays(weekly).AddDays(1).ToString("yyyyMMdd");
                                        }
                                        else
                                        {
                                            InsertPmTime = dateTime.AddDays(weekly).ToString("yyyyMMdd");
                                        }
                                        strSql.Clear();
                                        strSql.Append("INSERT INTO TAPFTPMSCHEDULE(REGION,MAINEQUIPMENT,PMSCHEDULE,AREA,BAY,DESCRIPTION,EQUIPMENT,FACILITY,INSERTTIME,INSERTUSER,LINE,UPDATETIME,UPDATEUSER,PMTIME)  VALUES('");
                                        strSql.Append(dr["REGION"].ToString() + "','" + dr["MAINEQUIPMENT"].ToString() + "','" + dr["PMSCHEDULE"].ToString() + "','");
                                        strSql.Append(dr["AREA"].ToString() + "','" + dr["BAY"].ToString() + "','" + dr["DESCRIPTION"].ToString() + "','" + dr["EQUIPMENT"].ToString() + "','");
                                        strSql.Append(dr["FACILITY"].ToString() + "','" + DateTime.Now.ToString("yyyyMMdd") + "','" + dr["INSERTUSER"].ToString() + "','" + dr["LINE"].ToString() + "','");
                                        strSql.Append(dr["UPDATETIME"].ToString() + "','" + dr["UPDATEUSER"].ToString() + "','");
                                        strSql.Append(InsertPmTime + "')");

                                        if (dataTable.Select("PMTIME='" + InsertPmTime + "'" + "and MAINEQUIPMENT = '" + dr["MAINEQUIPMENT"].ToString() + "'" + " and AREA ='" + dr["AREA"].ToString() + "'" + " and LINE = '" + dr["LINE"].ToString() + "'").Length == 0)
                                        {
                                            tmpSaveList.Add(strSql.ToString());
                                        }

                                        dateTime = DateTime.ParseExact(InsertPmTime, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                                    }
                                    else if (dateTime.AddDays(weekly).Month == DateTime.Now.AddMonths(2).Month || dateTime.AddDays(weekly).Month == DateTime.Now.AddMonths(3).Month)
                                    {
                                        _break = true;
                                        break;
                                    }
                                    else
                                    {
                                        dateTime = dateTime.AddDays(weekly);

                                    }
                                }
                            }
                        }
                        else if (dr["PMSCHEDULE"].ToString().Contains("MONTHLY"))
                        {

                            if (Regex.IsMatch(dr["PMSCHEDULE"].ToString().Substring(0, 1), @"^[+-]?\d*[.]?\d*$"))
                            {
                                num = int.Parse(dr["PMSCHEDULE"].ToString().Substring(0, 1));

                                if (dateTime.AddMonths(num).DayOfWeek.ToString().ToUpper() == "SATURDAY")
                                {

                                    InsertPmTime = dateTime.AddMonths(num).AddDays(2).ToString("yyyyMMdd");
                                }
                                else if (dateTime.AddMonths(num).DayOfWeek.ToString().ToUpper() == "SUNDAY")
                                {

                                    InsertPmTime = dateTime.AddMonths(num).AddDays(1).ToString("yyyyMMdd");
                                }
                                else
                                {

                                    InsertPmTime = dateTime.AddMonths(num).ToString("yyyyMMdd");
                                }

                            }
                            else
                            {

                                if (dateTime.AddMonths(1).DayOfWeek.ToString().ToUpper() == "SATURDAY")
                                {

                                    InsertPmTime = dateTime.AddMonths(1).AddDays(2).ToString("yyyyMMdd");
                                }
                                else if (dateTime.AddMonths(1).DayOfWeek.ToString().ToUpper() == "SUNDAY")
                                {

                                    InsertPmTime = dateTime.AddMonths(1).AddDays(1).ToString("yyyyMMdd");
                                }
                                else
                                {

                                    InsertPmTime = dateTime.AddMonths(1).ToString("yyyyMMdd");
                                }

                            }

                        }
                        else if (dr["PMSCHEDULE"].ToString().Contains("YEARLY"))
                        {
                            if (Regex.IsMatch(dr["PMSCHEDULE"].ToString().Substring(0, 1), @"^[+-]?\d*[.]?\d*$"))
                            {
                                num = int.Parse(dr["PMSCHEDULE"].ToString().Substring(0, 1));

                                if (dateTime.AddYears(num).DayOfWeek.ToString().ToUpper() == "SATURDAY")
                                {

                                    InsertPmTime = dateTime.AddYears(num).AddDays(2).ToString("yyyyMMdd");
                                }
                                else if (dateTime.AddYears(num).DayOfWeek.ToString().ToUpper() == "SUNDAY")
                                {

                                    InsertPmTime = dateTime.AddYears(num).AddDays(1).ToString("yyyyMMdd");
                                }
                                else
                                {

                                    InsertPmTime = dateTime.AddYears(num).ToString("yyyyMMdd");
                                }

                            }
                            else
                            {

                                if (dateTime.AddYears(1).DayOfWeek.ToString().ToUpper() == "SATURDAY")
                                {

                                    InsertPmTime = dateTime.AddYears(1).AddDays(2).ToString("yyyyMMdd");
                                }
                                else if (dateTime.AddYears(1).DayOfWeek.ToString().ToUpper() == "SUNDAY")
                                {

                                    InsertPmTime = dateTime.AddYears(1).AddDays(1).ToString("yyyyMMdd");
                                }
                                else
                                {

                                    InsertPmTime = dateTime.AddYears(1).ToString("yyyyMMdd");
                                }
                            }

                        }
                        else
                        {
                            continue;
                        }
                        if (_break)
                        {
                            _break = false;
                            continue;
                        }

                        strSql.Append("INSERT INTO TAPFTPMSCHEDULE(REGION,MAINEQUIPMENT,PMSCHEDULE,AREA,BAY,DESCRIPTION,EQUIPMENT,FACILITY,INSERTTIME,INSERTUSER,LINE,UPDATETIME,UPDATEUSER,PMTIME)  VALUES('");
                        strSql.Append(dr["REGION"].ToString() + "','" + dr["MAINEQUIPMENT"].ToString() + "','" + dr["PMSCHEDULE"].ToString() + "','");
                        strSql.Append(dr["AREA"].ToString() + "','" + dr["BAY"].ToString() + "','" + dr["DESCRIPTION"].ToString() + "','" + dr["EQUIPMENT"].ToString() + "','");
                        strSql.Append(dr["FACILITY"].ToString() + "','" + DateTime.Now.ToString("yyyyMMdd") + "','" + dr["INSERTUSER"].ToString() + "','" + dr["LINE"].ToString() + "','");
                        strSql.Append(dr["UPDATETIME"].ToString() + "','" + dr["UPDATEUSER"].ToString() + "','");
                        strSql.Append(InsertPmTime + "')");

                        if (dataTable.Select("PMTIME='" + InsertPmTime + "'" + "and MAINEQUIPMENT = '" + dr["MAINEQUIPMENT"].ToString() + "'" + " and AREA ='" + dr["AREA"].ToString() + "'" + " and LINE = '" + dr["LINE"].ToString() + "'").Length == 0)
                        {
                            tmpSaveList.Add(strSql.ToString());
                        }
                    }
                    int sqlnumb = db.Save(tmpSaveList);

                }

            }
            catch (Exception)
            {
                throw;
            }
        }





    }
}
