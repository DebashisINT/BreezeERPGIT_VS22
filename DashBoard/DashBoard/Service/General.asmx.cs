using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace DashBoard.DashBoard.Service
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class General : System.Web.Services.WebService
    {



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


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetTaskChangeHistory(string taskId)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dtmain = oDBEngine.GetDataTable(@"Select * from TASK_HISTORY where Schedule_Id="+taskId);
            List<GetTaskChangeHistoryClass> Branchs = new List<GetTaskChangeHistoryClass>();
            Branchs = (from DataRow dr in dtmain.Rows
                       select new GetTaskChangeHistoryClass()
                       {
                           ID = Convert.ToString(dr["ID"]),
                           Schedule_Id = Convert.ToString(dr["Schedule_Id"]),
                           CREATED_BY = Convert.ToString(dr["CREATED_BY"]),
                           CREATED_ON = Convert.ToString(dr["CREATED_ON"]),
                           TSTATUS = Convert.ToString(dr["TSTATUS"]),
                           IS_COMPLETED = Convert.ToString(dr["IS_COMPLETED"]),
                           REMARKS = Convert.ToString(dr["REMARKS"]),
                           TASK_TITLE = Convert.ToString(dr["TASK_TITLE"])
                       }).ToList();
            return Branchs;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetUsersAll()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dtmain = oDBEngine.GetDataTable(@"Select user_id, user_name from tbl_master_user");
            List<GetUsersAllClass> Branchs = new List<GetUsersAllClass>();
            Branchs = (from DataRow dr in dtmain.Rows
                       select new GetUsersAllClass()
                       {
                           user_id = Convert.ToString(dr["user_id"]),
                           user_name = Convert.ToString(dr["user_name"])
                           
                       }).ToList();
            return Branchs;
        }

        

        public class GetUsersAllClass
        {
            	
            public string user_id { get; set; }
            public string user_name { get; set; }
        }
        public class GetTaskChangeHistoryClass
        {						
            public string ID { get; set; }
            public string Schedule_Id { get; set; }
            public string CREATED_BY { get; set; }
            public string CREATED_ON { get; set; }
            public string TSTATUS { get; set; }
            public string IS_COMPLETED { get; set; }
            public string REMARKS { get; set; }
            public string TASK_TITLE { get; set; }
        }
        public class MainBranches
        {
            public string ID { get; set; }
            public string NAME { get; set; }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object UpdateTask(string taskId, string tskStatusDrop, string updateRemarks, string TASK_TITLE, string HELP_ID, string HELP_REASON)
        {
            //return false;

            ProcedureExecute proc = new ProcedureExecute("PRC_UPDATETASKWITHHISTORY");
            proc.AddVarcharPara("@userId", 5000, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@taskId", 5000, taskId);
            proc.AddVarcharPara("@tskStatusDrop", 5000, tskStatusDrop);
            proc.AddVarcharPara("@updateRemarks", 5000, updateRemarks);
            proc.AddVarcharPara("@TASK_TITLE", 5000, TASK_TITLE);
            proc.AddVarcharPara("@HELP_ID", 5000, HELP_ID);
            proc.AddVarcharPara("@HELP_REASON", 5000, HELP_REASON);
            DataTable AttData = proc.GetTable();


            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            //oDBEngine.GetDataTable("UPDATE TASK_SCHEDULE SET ISCOMPLETED=CASE WHEN '" + Convert.ToString(tskStatusDrop) + "'='Completed' THEN 1 ELSE 0 END,MODIFIEDON=GETDATE(),MODIFIEDBY='" + Convert.ToString(HttpContext.Current.Session["userid"]) + "', task_status='" + Convert.ToString(tskStatusDrop) + "' REMARKS='" + Convert.ToString(updateRemarks) + "' WHERE SCHEDULE_id='" + Convert.ToString(taskId) + "'; insert into TASK_HISTORY(Schedule_Id,CREATED_BY,CREATED_ON,TSTATUS,IS_COMPLETED, REMARKS) select '" + Convert.ToString(taskId) + "', '" + Convert.ToString(HttpContext.Current.Session["userid"]) + "', GETDATE(), '" + Convert.ToString(tskStatusDrop) + "', case when '" + Convert.ToString(tskStatusDrop) + "'='Completed' THEN 1 ELSE 0 , '" + Convert.ToString(updateRemarks) + "' end");

            
            //if (tskStatusDrop == "Completed") {
            //    oDBEngine.GetDataTable("UPDATE TASK_SCHEDULE SET ISCOMPLETED=CASE WHEN ISCOMPLETED=1 THEN 0 ELSE 1 END,COMPLETEDON=GETDATE(),COMPLETEDBY='" + Convert.ToString(HttpContext.Current.Session["userid"]) + "' WHERE SCHEDULE_id='" + Convert.ToString(taskId) + "' and COMPLETEDON IS NULL");
            //}
            //if (Status == "1")
            //{
            //    oDBEngine.GetDataTable("UPDATE TASK_SCHEDULE SET COMPLETEDON=NULL,COMPLETEDBY=NULL,MODIFIEDON= GETDATE(),MODIFIEDBY='" + Convert.ToString(HttpContext.Current.Session["userid"]) + "' WHERE SCHEDULE_id='" + Convert.ToString(taskId) + "'");

            //}


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
                                    balance = Convert.ToDecimal(dr["BALANCE"]).ToString("0.00"),
                                    balancePercentage = Convert.ToDecimal(dr["BALPERCENTAGE"]).ToString("0.00")
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
                                      AMOUNT = Convert.ToDecimal(dr["AMOUNT"]).ToString("0.00")
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
                                      CURRBAL = Convert.ToDecimal(dr["CURRBAL"]).ToString("0.00"),
                                      YTD = Convert.ToDecimal(dr["YTD"]).ToString("0.00")
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
                                    WARNING = Convert.ToBoolean(dr["WARNING"]),
                                    task_status = Convert.ToString(dr["task_status"]),
                                    HELP_ID = Convert.ToString(dr["HELP_ID"]),
                                    HELP_REASON = Convert.ToString(dr["HELP_REASON"]),
                                    REMARKS = Convert.ToString(dr["REMARKS"])
                                    
                                }).ToList();

            return TodoThisMonth;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetInventoryBox(string date, string branchid, string ProdClass, string Prodid)
        {
            List<invetoryBoxes> TodoThisMonth = new List<invetoryBoxes>();
            ProcedureExecute proc = new ProcedureExecute("PRC_INVENTORYDASHBOARD_REPORT");
            proc.AddVarcharPara("@USERID", 5000, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@COMPANYID", 5000, Convert.ToString(Session["LastCompany"]));
            proc.AddPara("@FINYEAR", Convert.ToString(HttpContext.Current.Session["LastFinYear"]));

            proc.AddVarcharPara("@ACTION", 100, "AllHeader");
            proc.AddVarcharPara("@ASONDATE", 5000, date);
            proc.AddVarcharPara("@BranchId", 1000, branchid);
            proc.AddVarcharPara("@ProdClass", 5000, ProdClass);
            proc.AddVarcharPara("@Prodid", 5000, Prodid);
            DataTable AttData = proc.GetTable();

            TodoThisMonth = (from DataRow dr in AttData.Rows
                             select new invetoryBoxes()
                             {
                                 TOTALINVPROD = Convert.ToString(dr["TOTALINVPROD"]),
                                 TOTALVAL = Convert.ToString(dr["TOTALVAL"]),
                                 TOTALPROD = Convert.ToString(dr["TOTALPROD"]),
                                 TOTALPRODSC = Convert.ToString(dr["TOTALPRODSC"]),
                                 TOTALPRODIN = Convert.ToString(dr["TOTALPRODIN"]),
                                 TOTALPRODOUT = Convert.ToString(dr["TOTALPRODOUT"]),
                             }).ToList();

            return TodoThisMonth;
        }



        public class invetoryBoxes
        {
            public string TOTALINVPROD { get; set; }
            public string TOTALVAL { get; set; }
            public string TOTALPROD { get; set; }
            public string TOTALPRODSC { get; set; }
            public string TOTALPRODIN { get; set; }
            public string TOTALPRODOUT { get; set; }
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetInventoryChartOne(string date, string branchid, string ProdClass, string Prodid)
        {
            List<chartOne> TodoThisMonth = new List<chartOne>();
            ProcedureExecute proc = new ProcedureExecute("PRC_INVENTORYDASHBOARD_REPORT");
            proc.AddVarcharPara("@USERID", 5000, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@COMPANYID", 5000, Convert.ToString(Session["LastCompany"]));
            proc.AddPara("@FINYEAR", Convert.ToString(HttpContext.Current.Session["LastFinYear"]));

            proc.AddVarcharPara("@ACTION", 100, "SI");
            proc.AddVarcharPara("@ASONDATE", 5000, date);
            proc.AddVarcharPara("@BranchId", 1000, branchid);
            proc.AddVarcharPara("@ProdClass", 5000, ProdClass);
            proc.AddVarcharPara("@Prodid", 5000, Prodid);
            DataTable AttData = proc.GetTable();

            TodoThisMonth = (from DataRow dr in AttData.Rows
                             select new chartOne()
                             {
                                 PRODDESC = Convert.ToString(dr["PRODDESC"]),
                                 QTY = Convert.ToString(dr["QTY"]),
                             }).ToList();

            return TodoThisMonth;
        }
        public class chartOne
        {
            public string PRODDESC { get; set; }
            public string QTY { get; set; }
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetInventoryChartTwo(string date, string branchid, string ProdClass, string Prodid)
        {
            List<chartTwo> TodoThisMonth = new List<chartTwo>();
            ProcedureExecute proc = new ProcedureExecute("PRC_INVENTORYDASHBOARD_REPORT");
            proc.AddVarcharPara("@USERID", 5000, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@COMPANYID", 5000, Convert.ToString(Session["LastCompany"]));
            proc.AddPara("@FINYEAR", Convert.ToString(HttpContext.Current.Session["LastFinYear"]));

            proc.AddVarcharPara("@ACTION", 100, "PI");
            proc.AddVarcharPara("@ASONDATE", 5000, date);
            proc.AddVarcharPara("@BranchId", 1000, branchid);
            proc.AddVarcharPara("@ProdClass", 5000, ProdClass);
            proc.AddVarcharPara("@Prodid", 5000, Prodid);
            DataTable AttData = proc.GetTable();

            TodoThisMonth = (from DataRow dr in AttData.Rows
                             select new chartTwo()
                             {
                                 PRODDESC = Convert.ToString(dr["PRODDESC"]),
                                 QTY = Convert.ToString(dr["QTY"]),
                             }).ToList();

            return TodoThisMonth;
        }
        public class chartTwo
        {
            public string PRODDESC { get; set; }
            public string QTY { get; set; }
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetInventoryChartThree(string date, string branchid, string ProdClass, string Prodid)
        {
            List<chartThree> TodoThisMonth = new List<chartThree>();
            ProcedureExecute proc = new ProcedureExecute("PRC_INVENTORYDASHBOARD_REPORT");
            proc.AddVarcharPara("@USERID", 5000, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@COMPANYID", 5000, Convert.ToString(Session["LastCompany"]));
            proc.AddPara("@FINYEAR", Convert.ToString(HttpContext.Current.Session["LastFinYear"]));

            proc.AddVarcharPara("@ACTION", 100, "SRI");
            proc.AddVarcharPara("@ASONDATE", 5000, date);
            proc.AddVarcharPara("@BranchId", 1000, branchid);
            proc.AddVarcharPara("@ProdClass", 5000, ProdClass);
            proc.AddVarcharPara("@Prodid", 5000, Prodid);
            DataTable AttData = proc.GetTable();

            TodoThisMonth = (from DataRow dr in AttData.Rows
                             select new chartThree()
                             {
                                 PRODDESC = Convert.ToString(dr["PRODDESC"]),
                                 QTY = Convert.ToString(dr["QTY"]),
                             }).ToList();

            return TodoThisMonth;
        }

        
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetRequisition(string date, string branchid,  string ProdClass, string Prodid)
        {
            List<StockRequisition> TodoThisMonth = new List<StockRequisition>();
            ProcedureExecute proc = new ProcedureExecute("PRC_STOCKREQDB_REPORT");

            proc.AddVarcharPara("@ASONDATE", 5000, date);
            proc.AddVarcharPara("@BranchId", 5000, branchid);
            proc.AddVarcharPara("@ProdClass", 5000, ProdClass);
            proc.AddVarcharPara("@Prodid", 5000, Prodid);
            proc.AddVarcharPara("@USERID", 500, Convert.ToString(HttpContext.Current.Session["userid"]));
            
            DataTable AttData = proc.GetTable();

            TodoThisMonth = (from DataRow dr in AttData.Rows
                             select new StockRequisition()
                             {
                                 BRANCHREQ = Convert.ToString(dr["BRANCHREQ"]),
                                 APPRPENDING = Convert.ToString(dr["APPRPENDING"]),
                                 OPENREQ = Convert.ToString(dr["OPENREQ"]),
                                 CLOSEREQ = Convert.ToString(dr["CLOSEREQ"]),
                                 APPRREQ = Convert.ToString(dr["APPRREQ"]),
                             }).ToList();

            return TodoThisMonth;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProcurement(string date, string branchid, string ProdClass, string Prodid)
        {
            List<ProcurementRequisition> TodoThisMonth = new List<ProcurementRequisition>();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROCUREMENTREQDB_REPORT");

            proc.AddVarcharPara("@ASONDATE", 5000, date);
            proc.AddVarcharPara("@BranchId", 5000, branchid);
            proc.AddVarcharPara("@ProdClass", 5000, ProdClass);
            proc.AddVarcharPara("@Prodid", 5000, Prodid);
            proc.AddVarcharPara("@USERID", 500, Convert.ToString(HttpContext.Current.Session["userid"]));
            DataTable AttData = proc.GetTable();

            TodoThisMonth = (from DataRow dr in AttData.Rows
                             select new ProcurementRequisition()
                             {
                                 PURCHASEREQ = Convert.ToString(dr["PURCHASEREQ"]),
                                 OPENREQ = Convert.ToString(dr["OPENREQ"]),
                                 TOTPO = Convert.ToString(dr["TOTPO"]),
                                 APPRPO = Convert.ToString(dr["APPRPO"]),
                                 APPRREQ = Convert.ToString(dr["APPRREQ"]),
                             }).ToList();

            return TodoThisMonth;
        }

        public class ProcurementRequisition
        {				

            public string PURCHASEREQ { get; set; }
            public string OPENREQ { get; set; }
            public string TOTPO { get; set; }
            public string APPRPO { get; set; }
            public string APPRREQ { get; set; }
        }
        public class StockRequisition
        {
            public string BRANCHREQ { get; set; }
            public string APPRPENDING { get; set; }
            public string OPENREQ { get; set; }
            public string CLOSEREQ { get; set; }
            public string APPRREQ { get; set; }
        }

        public class chartThree
        {
            public string PRODDESC { get; set; }
            public string QTY { get; set; }
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetInventoryChartFour(string date, string branchid, string ProdClass, string Prodid)
        {
            List<chartFour> TodoThisMonth = new List<chartFour>();
            ProcedureExecute proc = new ProcedureExecute("PRC_INVENTORYDASHBOARD_REPORT");
            proc.AddVarcharPara("@USERID", 5000, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@COMPANYID", 5000, Convert.ToString(Session["LastCompany"]));
            proc.AddPara("@FINYEAR", Convert.ToString(HttpContext.Current.Session["LastFinYear"]));

            proc.AddVarcharPara("@ACTION", 100, "PRI");
            proc.AddVarcharPara("@ASONDATE", 5000, date);
            proc.AddVarcharPara("@BranchId", 5000, branchid);
            proc.AddVarcharPara("@ProdClass", 5000, ProdClass);
            proc.AddVarcharPara("@Prodid", 5000, Prodid);
            DataTable AttData = proc.GetTable();

            TodoThisMonth = (from DataRow dr in AttData.Rows
                             select new chartFour()
                             {
                                 PRODDESC = Convert.ToString(dr["PRODDESC"]),
                                 QTY = Convert.ToString(dr["QTY"]),
                             }).ToList();

            return TodoThisMonth;
        }
        public class chartFour
        {
            public string PRODDESC { get; set; }
            public string QTY { get; set; }
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
            public string task_status { get; set; }
            public string HELP_ID { get; set; }
            public string HELP_REASON { get; set; }
            public string REMARKS { get; set; }
            
        }



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
            public string CURRBAL { get; set; }
            public string YTD { get; set; }
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
            public string AMOUNT { get; set; }
        }



        public class DashboardAccDetailsClass
        {
            public string mainAccountName { get; set; }
            public string balance { get; set; }
            public string balancePercentage { get; set; }
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
