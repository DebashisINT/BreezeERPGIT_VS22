using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ERP.OMS.Management.Service
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class Vendors : System.Web.Services.WebService
    {


        // Rev Bapi
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetVendorsList(string SearchKey)
        {
            List<VendorModel> listVen = new List<VendorModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                string strQuery = @"Select top 10 * From (Select cnt_internalid,IsNull(cnt_shortName,'NA') as uniquename,IsNull(cnt_firstName,'')+IsNull(cnt_middleName,'')+IsNull(cnt_lastName,'') as Name 
                                    From tbl_master_contact Where cnt_contactStatus<>3 AND cnt_contactType ='DV' ) as tbl " +
                                    "Where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'";

                DataTable dt = oDBEngine.GetDataTable(strQuery);
                listVen = (from DataRow dr in dt.Rows
                           select new VendorModel()
                           {
                               ID = dr["cnt_internalid"].ToString(),
                               NAME = dr["Name"].ToString(),

                           }).ToList();
            }

            return listVen;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCustomer(string SearchKey)
        {
            List<CustomerModel> listCust = new List<CustomerModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable cust = oDBEngine.GetDataTable(" select top 10 cnt_internalid ,uniquename ,Name ,Billing   from v_All_customerDetails where uniquename like '%" + SearchKey + "%' or Name like '%" + SearchKey + "%'   order by Name");


                listCust = (from DataRow dr in cust.Rows
                            select new CustomerModel()
                            {
                                id = dr["cnt_internalid"].ToString(),
                                Na = dr["Name"].ToString(),
                                UId = Convert.ToString(dr["uniquename"]),
                                add = Convert.ToString(dr["Billing"])
                            }).ToList();
            }

            return listCust;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetEmployee(string SerarchKey)
        {
            List<CrmDbEmployee> listEmp = new List<CrmDbEmployee>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SerarchKey = SerarchKey.Replace("'", "''");
                ProcedureExecute proc = new ProcedureExecute("Prc_crmdb");
                proc.AddVarcharPara("@Action", 100, "Get10Emp");
                proc.AddVarcharPara("@SearchKey", 100, SerarchKey);
                DataTable cust = proc.GetTable();

                listEmp = (from DataRow dr in cust.Rows
                           select new CrmDbEmployee()
                           {
                               id = dr["cnt_internalId"].ToString(),
                               EmpCode = dr["EmpCode"].ToString(),
                               EmpName = Convert.ToString(dr["Name"])
                           }).ToList();
            }

            return listEmp;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCallDetails(string SalesmanName, string FromDate, string Todate)
        {

            List<callDetails> callDetailsList = new List<callDetails>();
            ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
            proc.AddVarcharPara("@Action", 100, "GetCallDetails");
            proc.AddPara("@FromDate", FromDate);
            proc.AddPara("@Todate", Todate);
            proc.AddPara("@salesman", SalesmanName);
            DataTable CallData = proc.GetTable();

            callDetailsList = (from DataRow dr in CallData.Rows
                               select new callDetails()
                               {
                                   calldate = dr["calldate"].ToString(),
                                   note = dr["note"].ToString(),
                                   prodName = Convert.ToString(dr["prodName"])
                               }).ToList();

            return callDetailsList;

        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetEfficency(string FromDtae, string toDate)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = Convert.ToDateTime(toDate);
            toDate1 = toDate1.Date.AddHours(23);

            List<Efficency> lEfficency = new List<Efficency>();
            ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
            proc.AddVarcharPara("@Action", 100, "GetSefficiency");
            proc.AddPara("@FromDate", FromDtae);
            proc.AddPara("@Todate", toDate);
            DataTable CallData = proc.GetTable();
            var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new Efficency()
                          {
                              SManName = Convert.ToString(dr["SalesManName"]),
                              EF = Convert.ToString(dr["EF"]),
                              color = String.Format("#{0:X6}", random.Next(0x1000000))
                          }).ToList();
            return lEfficency;
        }




        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPendingAct(string toDate, string ActType)
        {
            DateTime toDate1 = new DateTime();
            toDate1 = Convert.ToDateTime(toDate);
            toDate1 = toDate1.Date.AddHours(23);

            List<Efficency> lEfficency = new List<Efficency>();
            ProcedureExecute proc = new ProcedureExecute("prc_crmDb");
            proc.AddVarcharPara("@Action", 100, "GetPendingAct");
            proc.AddPara("@FromDate", toDate1);
            proc.AddPara("@ActivityState", ActType);
            DataTable CallData = proc.GetTable();
            var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new Efficency()
                          {
                              SManName = Convert.ToString(dr["SalesManName"]),
                              EF = Convert.ToString(dr["ActCount"]),
                              color = String.Format("#{0:X6}", random.Next(0x1000000))
                          }).ToList();
            return lEfficency;
        }




        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTodaysAttendance()
        {
            todaysAttendance TodaysAttendance = new todaysAttendance();
            ProcedureExecute proc = new ProcedureExecute("prc_attendanceDB");
            proc.AddVarcharPara("@action", 100, "GetToDaysAttendance");
            DataTable AttData = proc.GetTable();

            TodaysAttendance.TotalEmp = Convert.ToString(AttData.Rows[0]["TotalEmp"]);
            TodaysAttendance.Present = Convert.ToString(AttData.Rows[0]["Present"]);
            TodaysAttendance.todaysAbsent = Convert.ToString(AttData.Rows[0]["todaysAbsent"]);
            TodaysAttendance.LateCount = Convert.ToString(AttData.Rows[0]["LateCount"]);
            TodaysAttendance.LeaveCount = Convert.ToString(AttData.Rows[0]["LeaveCount"]);

            return TodaysAttendance;

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetLastDaysperformance(string forLast)
        {
            List<LeaveRatio> leaveRatio = new List<LeaveRatio>();
            ProcedureExecute proc = new ProcedureExecute("prc_attendanceDB");
            proc.AddVarcharPara("@action", 100, "PerformenceRange");
            proc.AddPara("@ForLastDays", forLast);
            DataTable AttData = proc.GetTable();

            leaveRatio = (from DataRow dr in AttData.Rows
                          select new LeaveRatio()
                          {
                              LeaveName = Convert.ToString(dr["Name"]),
                              unit = Convert.ToDecimal(dr["totCount"])
                          }).ToList();

            return leaveRatio;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object TodaysLate()
        {
            List<LateUser> leaveRatio = new List<LateUser>();
            ProcedureExecute proc = new ProcedureExecute("prc_attendanceDB");
            proc.AddVarcharPara("@action", 100, "TodaysLate");
            DataTable AttData = proc.GetTable();

            leaveRatio = (from DataRow dr in AttData.Rows
                          select new LateUser()
                          {
                              LeaveName = Convert.ToString(dr["Name"]),
                              unit = Convert.ToDecimal(dr["totCount"]),
                              latetime = Convert.ToString(dr["lateinMin"])
                          }).ToList();

            return leaveRatio;
        }

        //susanta 14-02-2019
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetBranch()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dtmain = oDBEngine.GetDataTable(@"SELECT '' branch_id,'ALL' branch_description UNION ALL SELECT cast(branch_id AS varchar(20)),branch_description FROM tbl_master_branch ORDER BY branch_description");
            List<MainBranches> Branchs = new List<MainBranches>();
            Branchs = (from DataRow dr in dtmain.Rows
                  select new MainBranches()
                  {
                      ID = Convert.ToString(dr["branch_id"]),
                      NAME = Convert.ToString(dr["branch_description"])
                  }).ToList();
            return Branchs;
        }


      
        public class EmpLeaveRegList
        {
            public string id { get; set; }
            public string Name { get; set; }
        }
        // End Bapi
        public class MainBranches
        {
            public string ID { get; set; }
            public string NAME { get; set; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object UpdateTask(string taskId,string Status)
        {

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            oDBEngine.GetDataTable("UPDATE TASK_SCHEDULE SET ISCOMPLETED=CASE WHEN ISCOMPLETED=1 THEN 0 ELSE 1 END,MODIFIEDON=GETDATE(),MODIFIEDBY='" + Convert.ToString(HttpContext.Current.Session["userid"]) + "' WHERE SCHEDULE_id='" + Convert.ToString(taskId) + "' and COMPLETEDON IS NOT NULL");
           
            oDBEngine.GetDataTable("UPDATE TASK_SCHEDULE SET ISCOMPLETED=CASE WHEN ISCOMPLETED=1 THEN 0 ELSE 1 END,COMPLETEDON=GETDATE(),COMPLETEDBY='" + Convert.ToString(HttpContext.Current.Session["userid"]) + "' WHERE SCHEDULE_id='" + Convert.ToString(taskId) + "' and COMPLETEDON IS NULL");

            if (Status == "1")
            {
                oDBEngine.GetDataTable("UPDATE TASK_SCHEDULE SET COMPLETEDON=NULL,COMPLETEDBY=NULL,MODIFIEDON= NULL,MODIFIEDBY=NULL WHERE SCHEDULE_id='" + Convert.ToString(taskId) + "'");

            }

            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            //DataTable dtmain = oDBEngine.GetDataTable(@"SELECT '' branch_id,'ALL' branch_description UNION ALL SELECT cast(branch_id AS varchar(20)),branch_description FROM tbl_master_branch ORDER BY branch_description");
            //List<MainBranches> Branchs = new List<MainBranches>();
            //Branchs = (from DataRow dr in dtmain.Rows
            //           select new MainBranches()
            //           {
            //               ID = Convert.ToString(dr["branch_id"]),
            //               NAME = Convert.ToString(dr["branch_description"])
            //           }).ToList();
            return true;
        }
        public class UpdateTaskClass
        {
            public string ID { get; set; }
            public string NAME { get; set; }
        }





        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object DashboardAccDetails(string BRANCHID)
        {
            List<DashboardAccDetailsClass> ExpenseThisMonth = new List<DashboardAccDetailsClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_DBEXPENSESCURRENTMONTH_REPORT");
            proc.AddVarcharPara("@BRANCHID", 5000, BRANCHID);
            DataTable AttData = proc.GetTable();

            ExpenseThisMonth = (from DataRow dr in AttData.Rows
                                select new DashboardAccDetailsClass()
                                {
                                    mainAccountName = Convert.ToString(dr["MainAccount_Name"]),
                                    balance = Convert.ToDecimal(dr["BALANCE"]),
                                    balancePercentage = Convert.ToDecimal(dr["BALPERCENTAGE"])
                                }).ToList();

            return ExpenseThisMonth;
        }

        //susanta 14.02.2019
        //Recent payments
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object DashboardRecentPayments(string BRANCHID)
        {
            List<RecentPaymentsDetailsClass> RpaymentsThisMonth = new List<RecentPaymentsDetailsClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_DBRECENTPAYMENTS_REPORT ");
            proc.AddVarcharPara("@BRANCHID", 5000, BRANCHID);
            DataTable AttData = proc.GetTable();

            RpaymentsThisMonth = (from DataRow dr in AttData.Rows
                                  select new RecentPaymentsDetailsClass()
                                  {
                                      CUSTNAME = Convert.ToString(dr["CUSTNAME"]),
                                      DOCNO = Convert.ToString(dr["DOCNO"]),
                                      DATE_PAID = Convert.ToString(dr["DATE_PAID"]),
                                      AMOUNT = Convert.ToDecimal(dr["AMOUNT"])
                                  }).ToList();

            return RpaymentsThisMonth;
        }
        

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetMainAccount()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dtmain = oDBEngine.GetDataTable(@"SELECT MAINACCOUNT_REFERENCEID AS ID, MAINACCOUNT_NAME AS NAME FROM MASTER_MAINACCOUNT WHERE MAINACCOUNT_BANKCASHTYPE='Bank' AND MAINACCOUNT_PAYMENTTYPE<>'Card' ORDER BY MAINACCOUNT_REFERENCEID");
            List<MainAccount> ma = new List<MainAccount>();
            ma = (from DataRow dr in dtmain.Rows
                  select new MainAccount()
                  {
                      ID = Convert.ToString(dr["ID"]),
                      NAME = Convert.ToString(dr["NAME"])
                  }).ToList();
            return ma;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object getBankAmount(string BRANCHID, string BANKID)
        {
            List<getBankAmountClass> BankAmountThisMonth = new List<getBankAmountClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_DBTODAYBANKBALANCE_REPORT  ");
            proc.AddVarcharPara("@BRANCHID", 5000, BRANCHID);
            proc.AddVarcharPara("@BANKID", 5000, BANKID);
            DataTable AttData = proc.GetTable();

            //BankAmountThisMonth = (from DataRow dr in AttData.Rows
            //                      select new getBankAmountClass()
            //                      {
            //                          MAINACCOUNT_NAME = Convert.ToString(dr["MAINACCOUNT_NAME"]),
            //                          BALANCE = Convert.ToDecimal(dr["BALANCE"]),
            //                          //DATE_PAID = Convert.ToString(dr["DATE_PAID"]),
            //                          //AMOUNT = Convert.ToDecimal(dr["AMOUNT"])
            //                      }).ToList();

            if (AttData != null && AttData.Rows.Count > 0)
                return Convert.ToDecimal(AttData.Rows[0]["BALANCE"])+" "+Convert.ToString(AttData.Rows[0]["BALTYPE"]);
            else
                return 0;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object watchlistDetails(string BRANCHID)
        {
            List<accountWatchlistClass> watchlistThisMonth = new List<accountWatchlistClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_DBACCOUNTWATCHLIST_REPORT ");
            proc.AddVarcharPara("@BRANCHID", 5000, BRANCHID);
            DataTable AttData = proc.GetTable();

            watchlistThisMonth = (from DataRow dr in AttData.Rows
                                  select new accountWatchlistClass()
                                  {
                                      MAINACCOUNT_NAME = Convert.ToString(dr["MAINACCOUNT_NAME"]),
                                      CURRBAL = Convert.ToDecimal(dr["CURRBAL"]),
                                      YTD = Convert.ToDecimal(dr["YTD"])
                                  }).ToList();

            return watchlistThisMonth;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object chartDataPull(string BRANCHID)
        {
            chartDataClass chartDataThisMonth = new chartDataClass();
            ProcedureExecute proc = new ProcedureExecute("PRC_DBINCOMEEXPENSESCURRENTFINYEAR_REPORT ");
            proc.AddVarcharPara("@BRANCHID", 5000, BRANCHID);
            DataTable AttData = proc.GetTable();

            List<string> Month = new List<string>();
            List<decimal> Income = new List<decimal>();
            List<decimal> Expenses = new List<decimal>();
            List<decimal> Profit = new List<decimal>();

            foreach (DataColumn item in AttData.Columns)
            {
                if (item.ColumnName != "ACCOUNTTYPE")
                {
                    Month.Add(item.ColumnName);
                }
            }
            foreach (DataRow item in AttData.Rows)
            {
                foreach (DataColumn dr in AttData.Columns)
                {
                    if (dr.ColumnName != "ACCOUNTTYPE")
                    {
                        if (Convert.ToString(item["ACCOUNTTYPE"]) == "Income")
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(item[dr.ColumnName])))
                            {
                                Income.Add(Convert.ToDecimal(item[dr.ColumnName]));
                            }
                            else
                            {
                                Income.Add(0);
                            }
                        }
                        else if (Convert.ToString(item["ACCOUNTTYPE"]) == "Expense")
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(item[dr.ColumnName])))
                            {
                                Expenses.Add(Convert.ToDecimal(item[dr.ColumnName]));
                            }
                            else
                            {
                                Expenses.Add(0);
                            }
                        }
                        else if (Convert.ToString(item["ACCOUNTTYPE"]) == "Profit")
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(item[dr.ColumnName])))
                            {
                                Profit.Add(Convert.ToDecimal(item[dr.ColumnName]));
                            }
                            else
                            {
                                Profit.Add(0);
                            }
                        }
                    }
                }



            }

            chartDataThisMonth.Month = Month;
            chartDataThisMonth.Income = Income;
            chartDataThisMonth.Expenses = Expenses;
            chartDataThisMonth.profit = Profit;


            return chartDataThisMonth;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object TodoData(string userid)
        {
            List<toDoDataClass> TodoThisMonth = new List<toDoDataClass>();
            ProcedureExecute proc = new ProcedureExecute("TASK_LIST");
            proc.AddVarcharPara("@userid", 5000, Convert.ToString(HttpContext.Current.Session["userid"]));
            DataTable AttData = proc.GetTable();

            TodoThisMonth = (from DataRow dr in AttData.Rows
                                select new toDoDataClass()
                                {
                                    SCHEDULE_id = Convert.ToString(dr["SCHEDULE_id"]),
                                    TASK_SUBJECT = Convert.ToString(dr["TASK_SUBJECT"]),
                                    TASK_DESCRIPTION = Convert.ToString(dr["TASK_DESCRIPTION"]),
                                    TASK_DUEDATE = Convert.ToDateTime(dr["TASK_DUEDATE"]).ToString("yyyy-MM-dd"),
                                    TASK_DUEDATEFor = Convert.ToDateTime(dr["TASK_DUEDATE"]).ToString("dd-MM-yyyy"),
                                    ISCOMPLETED = Convert.ToBoolean(dr["ISCOMPLETED"]),
                                    CompletedBy = Convert.ToString(dr["CompletedBy"]),
                                    completedon = dr["completedon"] == DBNull.Value ? null : Convert.ToDateTime(dr["completedon"]).ToString("yyyy-MM-dd"),
                                    completedonFor = dr["completedon"] == DBNull.Value ? null : Convert.ToDateTime(dr["completedon"]).ToString("dd-MM-yyyy"),
                                    TASK_PRIORITY = Convert.ToString(dr["TASK_PRIORITY"]),
                                    WARNING = Convert.ToBoolean(dr["WARNING"])
                                    
                                }).ToList();

            return TodoThisMonth;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object IsTodoDataShow(string userid)
        {
            List<isTodoDataClasd> TodoThisMonth = new List<isTodoDataClasd>();
            ProcedureExecute proc = new ProcedureExecute("TASK_LIST_COUNT ");
            proc.AddVarcharPara("@userid", 5000, Convert.ToString(HttpContext.Current.Session["userid"]));
            DataTable AttData = proc.GetTable();

            TodoThisMonth = (from DataRow dr in AttData.Rows
                             select new isTodoDataClasd()
                             {
                                 CNT = Convert.ToString(dr["CNT"]),                                 
                             }).ToList();

            return TodoThisMonth;
        }
        public class isTodoDataClasd
        {
            public string CNT { get; set; }
        }
        public class toDoDataClass
        {
            public string SCHEDULE_id { get; set; }
            public string TASK_SUBJECT { get; set; }
            public string TASK_DESCRIPTION { get; set; }
            public string TASK_DUEDATE { get; set; }
            public string TASK_DUEDATEFor { get; set; }
            public Boolean ISCOMPLETED { get; set; }
            public string CompletedBy { get; set; }
            public string completedon { get; set; }
            public string completedonFor { get; set; }
            public string TASK_PRIORITY { get; set; }
            public Boolean WARNING { get; set; }
        }

       //Ref Bapi
        public class VendorModel
        {
            public string ID { get; set; }
            public string NAME { get; set; }
        }
        //Ref Bapi end
        public class chartDataClass
        {
            public List<string> Month { get; set; }
            public List<decimal> Income { get; set; }
            public List<decimal> Expenses { get; set; }
            public List<decimal> profit { get; set; }
        }

        public class accountWatchlistClass
        {
            public string MAINACCOUNT_NAME { get; set; }
            public decimal CURRBAL { get; set; }
            public decimal YTD { get; set; }
        }

        public class getBankAmountClass
        {
            public string MAINACCOUNT_NAME { get; set; }
            public decimal BALANCE { get; set; }
            public string BALTYPE { get; set; }
        }
        public class MainAccount
        {
            public string ID { get; set; }
            public string NAME { get; set; }
        }



        public class RecentPaymentsDetailsClass
        {
            public string CUSTNAME { get; set; }
            public string DOCNO { get; set; }
            public string DATE_PAID { get; set; }
            public decimal AMOUNT { get; set; }
        }



        public class DashboardAccDetailsClass
        {
            public string mainAccountName { get; set; }
            public decimal balance { get; set; }
            public decimal balancePercentage { get; set; }
        }



        public class LeaveRatio
        {
            public string LeaveName { get; set; }
            public decimal unit { get; set; }
        }

        public class LateUser
        {
            public string LeaveName { get; set; }
            public decimal unit { get; set; }
            public string latetime { get; set; }
        }



        public class todaysAttendance
        {
            public string TotalEmp { get; set; }
            public string Present { get; set; }
            public string todaysAbsent { get; set; }
            public string LateCount { get; set; }
            public string LeaveCount { get; set; }

        }





        public class Efficency
        {
            public string SManName { get; set; }
            public string EF { get; set; }
            public string color { get; set; }
        }
        public class CustomerModel
        {
            public string id { get; set; }
            public string Na { get; set; }
            public string UId { get; set; }
            public string add { get; set; }
        }

        public class callDetails
        {
            public string calldate { get; set; }
            public string note { get; set; }
            public string prodName { get; set; }

        }


        public class CrmDbEmployee
        {

            public string id { get; set; }
            public string EmpName { get; set; }
            public string EmpCode { get; set; }


        }

    }
}
