using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;

namespace Payroll.Models
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class p_WebServiceList : System.Web.Services.WebService
    {
        string ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPayStructureList(string SearchKey)
        {
            List<PayStructureList> list = new List<PayStructureList>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                DataTable dt = oDBEngine.GetDataTable(" SELECT StructureID,StructureName From proll_PayStructureMaster Where IsDeleted<>'Y' AND (StructureName like '%" + SearchKey + "%' or StructureCode like '%" + SearchKey + "%')   ORDER BY StructureName");


                list = (from DataRow dr in dt.Rows
                        select new PayStructureList()
                        {
                            StructureID = Convert.ToString(dr["StructureID"]),
                            StructureName = Convert.ToString(dr["StructureName"]),
                        }).ToList();
            }

            return list;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPayStructureDetailsList(string SearchKey)
        {
            List<PayStructureDetails> list = new List<PayStructureDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);

                string Query = @"   SELECT StructureID,StructureName,IsNull(PayrollClassName,'') as PayrollClass,
                                    IsNull(Convert(nvarchar(10),PeriodFrom,105),'') as PeriodFrom,IsNull(Convert(nvarchar(10),PeriodTo,105),'') as PeriodTo
                                    From proll_PayStructureMaster
                                    left Outer Join proll_PayrollClass_Master On ClassId=PayrollClassID
                                    Where proll_PayStructureMaster.IsDeleted<>'Y' AND (StructureName like '%" + SearchKey + "%' or StructureCode like '%" + SearchKey + "%')  ORDER BY StructureName";

                DataTable dt = oDBEngine.GetDataTable(Query);
                list = (from DataRow dr in dt.Rows
                        select new PayStructureDetails()
                        {
                            StructureID = Convert.ToString(dr["StructureID"]),
                            StructureName = Convert.ToString(dr["StructureName"]),
                            PayrollClass = Convert.ToString(dr["PayrollClass"]),
                            PeriodFrom = Convert.ToString(dr["PeriodFrom"]),
                            PeriodTo = Convert.ToString(dr["PeriodTo"]),
                        }).ToList();
            }

            return list;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPayClassList(string SearchKey)
        {
            List<PayClassList> list = new List<PayClassList>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                DataTable dt = oDBEngine.GetDataTable(" Select PayrollClassID,PayrollClassName,Convert(nvarchar(10),PeriodFrom,105) as PeriodFrom,Convert(nvarchar(10),PeriodTo,105) as PeriodTo From proll_PayrollClass_Master Where IsDeleted<>'Y' AND PayrollClassName like '%" + SearchKey + "%' ORDER BY PayrollClassName ASC");


                list = (from DataRow dr in dt.Rows
                        select new PayClassList()
                        {
                            PayrollClassID = Convert.ToString(dr["PayrollClassID"]),
                            PayrollClassName = Convert.ToString(dr["PayrollClassName"]),
                            PeriodFrom = Convert.ToString(dr["PeriodFrom"]),
                            PeriodTo = Convert.ToString(dr["PeriodTo"]),
                        }).ToList();
            }

            return list;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetLeaveStructureList(string SearchKey)
        {
            List<LeaveStructureList> list = new List<LeaveStructureList>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                DataTable dt = oDBEngine.GetDataTable(" SELECT LeaveStructureID,LeaveStructureCode,LeaveStructureName From proll_LeaveStructureMaster Where IsDeleted<>'Y' AND LeaveStructureName like '%" + SearchKey + "%' or LeaveStructureCode like '%" + SearchKey + "%'   ORDER BY LeaveStructureName");


                list = (from DataRow dr in dt.Rows
                        select new LeaveStructureList()
                        {
                            StructureID = Convert.ToString(dr["LeaveStructureID"]),
                            StructureCode = Convert.ToString(dr["LeaveStructureCode"]),
                            StructureName = Convert.ToString(dr["LeaveStructureName"]),
                        }).ToList();
            }

            return list;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetLeaveStructureDetailsList(string SearchKey)
        {
            List<LeaveStructureDetailsList> list = new List<LeaveStructureDetailsList>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                DataTable dt = oDBEngine.GetDataTable(" SELECT LeaveStructureID,LeaveStructureCode,LeaveStructureName,Convert(nvarchar(10),LeavePeriodFrom,105) as PeriodFrom,Convert(nvarchar(10),LeavePeriodTo,105) as PeriodTo From proll_LeaveStructureMaster Where IsDeleted<>'Y' AND (LeaveStructureName like '%" + SearchKey + "%' or LeaveStructureCode like '%" + SearchKey + "%')   ORDER BY LeaveStructureName");


                list = (from DataRow dr in dt.Rows
                        select new LeaveStructureDetailsList()
                        {
                            StructureID = Convert.ToString(dr["LeaveStructureID"]),
                            StructureCode = Convert.ToString(dr["LeaveStructureCode"]),
                            StructureName = Convert.ToString(dr["LeaveStructureName"]),
                            PeriodFrom = Convert.ToString(dr["PeriodFrom"]),
                            PeriodTo = Convert.ToString(dr["PeriodTo"]),
                        }).ToList();
            }

            return list;
        }

        //-- Rev Surojit --//
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetRosterDetailsList(string SearchKey)
        {
            List<RosterMasterList> list = new List<RosterMasterList>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                DataTable dt = oDBEngine.GetDataTable(" SELECT RosterID,RosterCode,RosterName,RosterType,ClassID FROM Proll_RosterMaster Where (RosterCode like '%" + SearchKey + "%' or RosterName like '%" + SearchKey + "%')   ORDER BY RosterID DESC");


                list = (from DataRow dr in dt.Rows
                        select new RosterMasterList()
                        {
                            RosterID = Convert.ToString(dr["RosterID"]),
                            RosterCode = Convert.ToString(dr["RosterCode"]),
                            RosterName = Convert.ToString(dr["RosterName"]),
                            RosterType = Convert.ToString(dr["RosterType"]),
                            ClassID = Convert.ToString(dr["ClassID"]),
                        }).ToList();
            }

            return list;
        }
        //-- End of rev Surojit --//

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetEmployeeList(string SearchKey, string LeaveStructureID)
        {
            List<EmployeeList> list = new List<EmployeeList>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);

                string strQuery = @"SELECT Distinct Employee_Code,EmployeeUniqueCode,Employee_Name 
                                    From proll_EmployeeAttactchment Inner Join v_proll_EmployeeList On Employee_Code=EmployeeCode
                                    Where (LeaveStructureCode='" + LeaveStructureID + "' AND  Employee_Name like '%" + SearchKey + "%') or " +
                                    "(LeaveStructureCode='" + LeaveStructureID + "' AND EmployeeUniqueCode like '%" + SearchKey + "%') ORDER BY Employee_Name";

                DataTable dt = oDBEngine.GetDataTable(strQuery);
                list = (from DataRow dr in dt.Rows
                        select new EmployeeList()
                        {
                            EmployeeID = Convert.ToString(dr["Employee_Code"]),
                            EmployeeCode = Convert.ToString(dr["EmployeeUniqueCode"]),
                            EmployeeName = Convert.ToString(dr["Employee_Name"]),
                        }).ToList();
            }

            return list;
        }


        // Rev Deep
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetEmpLeaveRegisterLists(string SearchKey)
        {
            List<EmpLeaveRegList> list = new List<EmpLeaveRegList>();
            string ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                string cntType = "EM";
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                DataTable dt = oDBEngine.GetDataTable(@"select distinct  top 20  tmc.cnt_internalId ID,(tmc.cnt_firstName +' '+ tmc.cnt_lastName)  as Name from tbl_master_contact tmc
                                                        inner join tbl_master_employee tme on tmc.cnt_internalId = tme.emp_contactId
                                                        inner join proll_Leave_Transaction plt on tme.emp_contactId = plt.EmployeeCode
                                                        Where cnt_contactType='" + cntType + "' AND cnt_firstName like '%" + SearchKey + "%'");


                list = (from DataRow dr in dt.Rows
                        select new EmpLeaveRegList()
                        {
                            id = Convert.ToString(dr["ID"]),
                            Name = Convert.ToString(dr["Name"]),
                        }).ToList();
            }

            return list;
        }
        // End Rev Deep
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object PopulateLeaveTypeWithBalance(string EmployeeID, string StructureID)
        {
            List<LeaveTypeBalance> list = new List<LeaveTypeBalance>();
            //List<LeaveTypeBalance> templist = new List<LeaveTypeBalance>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                string strQuery = @"Select LeaveID,LeaveName,(Case When LeaveType='HP' Then 1 Else 0 End) Active_DayPart,
                                    SUM(IsNull((Case When Tr_Type='1' Then (No_Day*1) Else (No_Day*-1) End),0)) Balance
                                    ,Isnull(Is_AllowLeaveCaseZeroBalance,0)Is_AllowLeaveCaseZeroBalance
                                    From proll_LeaveDefinition
                                    Left Outer Join (Select * From proll_Leave_Transaction Where EmployeeCode='" + EmployeeID + "') as tbl " +
                                    "On Leave_ID=LeaveID " +
                                    "Where LeaveStructureID='" + StructureID + "' " +
                                    "Group By LeaveID,LeaveName,LeaveType,Is_AllowLeaveCaseZeroBalance";

                DataTable dt = oDBEngine.GetDataTable(strQuery);
                list = (from DataRow dr in dt.Rows
                            select new LeaveTypeBalance()
                            {
                                LeaveID = Convert.ToString(dr["LeaveID"]),
                                LeaveName = Convert.ToString(dr["LeaveName"]),
                                Active_DayPart = Convert.ToString(dr["Active_DayPart"]),
                                Balance = Convert.ToString(dr["Balance"]),

                                //Is_WeeklyOffPerperiod = Convert.ToBoolean(dr["Is_WeeklyOffPerperiod"]),
                                //Is_PublicHolidayPeriod = Convert.ToBoolean(dr["Is_PublicHolidayPeriod"]),
                                //Is_CarryForwardNextPeriod = Convert.ToBoolean(dr["Is_CarryForwardNextPeriod"]),
                                //WeeklyOffDays = Convert.ToString(dr["WeeklyOffDays"]),
                                //NoOfPublicHoliday = Convert.ToInt16(dr["NoOfPublicHoliday"])
                                Is_AllowLeaveCaseZeroBalance=Convert.ToBoolean(dr["Is_AllowLeaveCaseZeroBalance"])
                            }).ToList();

                //foreach (var item in templist)
                //{
                //    if (item.Is_WeeklyOffPerperiod)
                //    {
                //        var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                //        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                //        int DaysCount = 0;
                //        foreach (var day in item.WeeklyOffDays.Split(','))
                //        {
                //            if (day == "Sunday")
                //            {
                //                DaysCount = DaysCount + CountDays(DayOfWeek.Sunday, firstDayOfMonth, lastDayOfMonth);
                //            }
                //            else if (day == "Monday")
                //            {
                //                DaysCount = DaysCount + CountDays(DayOfWeek.Monday, firstDayOfMonth, lastDayOfMonth);
                //            }
                //            else if (day == "Tuesday")
                //            {
                //                DaysCount = DaysCount + CountDays(DayOfWeek.Tuesday, firstDayOfMonth, lastDayOfMonth);
                //            }
                //            else if (day == "Wednesday")
                //            {
                //                DaysCount = DaysCount + CountDays(DayOfWeek.Wednesday, firstDayOfMonth, lastDayOfMonth);
                //            }
                //            else if (day == "Thursday")
                //            {
                //                DaysCount = DaysCount + CountDays(DayOfWeek.Thursday, firstDayOfMonth, lastDayOfMonth);
                //            }
                //            else if (day == "Friday")
                //            {
                //                DaysCount = DaysCount + CountDays(DayOfWeek.Friday, firstDayOfMonth, lastDayOfMonth);
                //            }
                //            else if (day == "Saturday")
                //            {
                //                DaysCount = DaysCount + CountDays(DayOfWeek.Saturday, firstDayOfMonth, lastDayOfMonth);
                //            }
                //        }
                //        item.Balance = Convert.ToString(Math.Round(Convert.ToDecimal(item.Balance) + Convert.ToDecimal(DaysCount),2));

                //    }
                //    if (item.Is_PublicHolidayPeriod)
                //    {
                //        item.Balance = Convert.ToString(Math.Round(Convert.ToDecimal(item.Balance) + Convert.ToDecimal(item.NoOfPublicHoliday),2));
                //    }

                //    list.Add(item);
                //}

            }

            return list;
        }

        public static int CountDays(DayOfWeek day, DateTime start, DateTime end)
        {
            TimeSpan ts = end - start;                       // Total duration
            int count = (int)Math.Floor(ts.TotalDays / 7);   // Number of whole weeks
            int remainder = (int)(ts.TotalDays % 7);         // Number of remaining days
            int sinceLastDay = (int)(end.DayOfWeek - day);   // Number of days since last [day]
            if (sinceLastDay < 0) sinceLastDay += 7;         // Adjust for negative days since last [day]

            // If the days in excess of an even week are greater than or equal to the number days since the last [day], then count this one, too.
            if (remainder >= sinceLastDay) count++;

            return count;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPayrollPeriodList(string SearchKey, string PayClassID)
        {
            List<PayrollPeriodList> list = new List<PayrollPeriodList>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                DataTable dt = oDBEngine.GetDataTable(" Select YYMM,Period,Convert(nvarchar(10),PeriodFrom,105) PeriodFrom,Convert(nvarchar(10),PeriodTo,105) PeriodTo From proll_PeriodGeneration Where PayrollClassID='" + PayClassID + "' AND  Period like '%" + SearchKey + "%' ORDER BY YYMM ASC");


                list = (from DataRow dr in dt.Rows
                        select new PayrollPeriodList()
                        {
                            YYMM = Convert.ToString(dr["YYMM"]),
                            Period = Convert.ToString(dr["Period"]),
                            PeriodFrom = Convert.ToString(dr["PeriodFrom"]),
                            PeriodTo = Convert.ToString(dr["PeriodTo"]),
                        }).ToList();
            }

            return list;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPayrollSalaryPeriodList(string SearchKey, string PayStructureID)
        {
            List<PayrollPeriodList> list = new List<PayrollPeriodList>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                DataTable dt = oDBEngine.GetDataTable(@"select proll_PayStructureMaster.StructureID,proll_PayrollClass_Master.PayrollClassID,proll_PayrollClass_Master.PeriodFrom,
                                                        proll_PayrollClass_Master.PeriodTo,a.YYMM,a.Period from proll_PayStructureMaster
                                                        inner join proll_PayrollClass_Master
                                                        on proll_PayStructureMaster.ClassId=proll_PayrollClass_Master.PayrollClassID
                                                        left join(select proll_PeriodGeneration.PayrollClassID,proll_PeriodGeneration.YYMM,proll_PeriodGeneration.Period 
                                                        from proll_PeriodGeneration)a
                                                        on a.PayrollClassID=proll_PayrollClass_Master.PayrollClassID
                                                        where proll_PayStructureMaster.StructureID='" + PayStructureID + "'");


                list = (from DataRow dr in dt.Rows
                        select new PayrollPeriodList()
                        {
                            YYMM = Convert.ToString(dr["YYMM"]),
                            Period = Convert.ToString(dr["Period"]),
                            PeriodFrom = Convert.ToString(dr["PeriodFrom"]),
                            PeriodTo = Convert.ToString(dr["PeriodTo"]),
                        }).ToList();
            }

            return list;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPayHeadList(string SearchKey, string PayHeadType, string PayHeadList)
        {
            List<PayHeadList> list = new List<PayHeadList>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                string StructureID = Convert.ToString(Session["StructureID"]);

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
                DataTable dt = oDBEngine.GetDataTable("Select PayHeadID,PayHeadCode,PayHeadName From proll_PayHeadMaster " +
                                                      "Where " +
                                                      "(StructureID='" + StructureID + "' AND PayType='" + PayHeadType + "' AND PayHeadCode like '%" + SearchKey + "%' AND PayHeadID Not In (select s FROM dbo.GetSplit(',','" + PayHeadList + "'))) " +
                                                      "OR " +
                                                      "(StructureID='" + StructureID + "' AND PayType='" + PayHeadType + "' AND PayHeadName like '%" + SearchKey + "%' AND PayHeadID Not In (select s FROM dbo.GetSplit(',','" + PayHeadList + "')))");

                list = (from DataRow dr in dt.Rows
                        select new PayHeadList()
                        {
                            PayHeadID = Convert.ToString(dr["PayHeadID"]),
                            PayHeadCode = Convert.ToString(dr["PayHeadCode"]),
                            PayHeadName = Convert.ToString(dr["PayHeadName"])
                        }).ToList();
            }

            return list;
        }

        public class EmpLeaveRegList
        {
            public string id { get; set; }
            public string Name { get; set; }
        }
        public class PayStructureList
        {
            public string StructureID { get; set; }
            public string StructureName { get; set; }
        }
        public class PayStructureDetails
        {
            public string StructureID { get; set; }
            public string StructureName { get; set; }
            public string PayrollClass { get; set; }
            public string PeriodFrom { get; set; }
            public string PeriodTo { get; set; }
        }
        public class PayClassList
        {
            public string PayrollClassID { get; set; }
            public string PayrollClassName { get; set; }
            public string PeriodFrom { get; set; }
            public string PeriodTo { get; set; }
        }
        public class LeaveStructureList
        {
            public string StructureID { get; set; }
            public string StructureCode { get; set; }
            public string StructureName { get; set; }
        }
        public class LeaveStructureDetailsList
        {
            public string StructureID { get; set; }
            public string StructureCode { get; set; }
            public string StructureName { get; set; }
            public string PeriodFrom { get; set; }
            public string PeriodTo { get; set; }
        }

        public class RosterMasterList
        {
            public string RosterID { get; set; }
            public string RosterCode { get; set; }
            public string RosterName { get; set; }
            public string RosterType { get; set; }
            public string ClassID { get; set; }
        }
        public class EmployeeList
        {
            public string EmployeeID { get; set; }
            public string EmployeeCode { get; set; }
            public string EmployeeName { get; set; }
        }
        public class LeaveTypeBalance
        {
            public string LeaveID { get; set; }
            public string LeaveName { get; set; }
            public string Active_DayPart { get; set; }
            public string Balance { get; set; }

            public bool Is_WeeklyOffPerperiod { get; set; }
            public bool Is_PublicHolidayPeriod { get; set; }
            public bool Is_CarryForwardNextPeriod { get; set; }
            public String WeeklyOffDays { get; set; }

            public int NoOfPublicHoliday { get; set; }

            public bool Is_AllowLeaveCaseZeroBalance { get; set; }
        }
        public class PayrollPeriodList
        {
            public string YYMM { get; set; }
            public string Period { get; set; }
            public string PeriodFrom { get; set; }
            public string PeriodTo { get; set; }
        }
        public class PayHeadList
        {
            public string PayHeadID { get; set; }
            public string PayHeadCode { get; set; }
            public string PayHeadName { get; set; }
        }
    }
}
