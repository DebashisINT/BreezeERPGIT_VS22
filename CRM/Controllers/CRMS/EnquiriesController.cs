using CRM.Models;
using CRM.Repostiory.Enquiries;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;
using EntityLayer.CommonELS;
using DataAccessLayer;
using DataAccessLayer.Model;
using System.Net.Http;
using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;

namespace CRM.Controllers.CRMS
{
    [CRM.Models.Attributes.SessionTimeout]
    public class EnquiriesController : Controller
    {
        private IEnquiries _enquiries;
        UserRightsForPage rights = new UserRightsForPage();

        public ActionResult Dashboard()
        {

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
            

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Dashboard", "Enquiries");


            List<EnquriesFrom> listenquriesfrom = new List<EnquriesFrom>();
            EnquiriesDet enqdet = new EnquiriesDet();
            EnquriesFrom enquriesfrom = new EnquriesFrom();
            TempData["AssignIndustryName"] = null;
            TempData["PriorSalesmanName"] = null;
            // Rev Mantis Issue 24878
            TempData["AssignBulkIndustryName"] = null;
            // End of Rev Mantis Issue 24878
            TempData.Keep();

            TempData["SearchKey"] = (EnquiriesDet)TempData["SearchKey"];

            if (TempData["SearchKey"] != null)
            {
                ViewBag.Fromdate = ((EnquiriesDet)TempData["SearchKey"]).FromDate;
                ViewBag.Todate = ((EnquiriesDet)TempData["SearchKey"]).ToDate;
                ViewBag.EnqFrom = ((EnquiriesDet)TempData["SearchKey"]).EnquiriesFrom;
                enqdet = (EnquiriesDet)TempData["SearchKey"];
            }
            else
            {
                ViewBag.Fromdate = "";
                ViewBag.Todate = "";
                ViewBag.EnqFrom = "";
            }


            enquriesfrom.EnqName = "All";
            enquriesfrom.EnqId = 0;
            listenquriesfrom.Add(enquriesfrom);

            enquriesfrom = new EnquriesFrom();
            enquriesfrom.EnqName = "Justdial";
            enquriesfrom.EnqId = 1;
            listenquriesfrom.Add(enquriesfrom);

            enquriesfrom = new EnquriesFrom();
            enquriesfrom.EnqName = "Tradeindia";
            enquriesfrom.EnqId = 2;
            listenquriesfrom.Add(enquriesfrom);

            enquriesfrom = new EnquriesFrom();
            enquriesfrom.EnqName = "Indiamart";
            enquriesfrom.EnqId = 3;
            listenquriesfrom.Add(enquriesfrom);

            enquriesfrom = new EnquriesFrom();
            enquriesfrom.EnqName = "Manual";
            enquriesfrom.EnqId = 4;
            listenquriesfrom.Add(enquriesfrom);

            enquriesfrom = new EnquriesFrom();
            enquriesfrom.EnqName = "Import";
            enquriesfrom.EnqId = 5;
            listenquriesfrom.Add(enquriesfrom);

            enqdet.enqfrom = listenquriesfrom;
            enqdet.Date = DateTime.Now;



            //Rev 2.0 Subhra (Peekay's requirement) 12-03-2019
            //if ((string)TempData["isshowclicked"] == "1")
            //{
            //End of Rev 
                
           //Rev 2.0 Subhra (Peekay's requirement) 12-03-2019
            //}
            //else
            //{
            //    ViewBag.Fromdate = "";
            //    ViewBag.Todate = "";
            //    ViewBag.EnqFrom = "";
            //}
            //TempData.Remove("SearchKey");
            //TempData.Remove("isshowclicked");
           //End of Rev 
            
           
            
            enqdet.UserRightsForPage = rights;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.SupervisorFeedback = rights.SupervisorFeedback;
            ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
            ViewBag.Verified = rights.Verified;
            return View("~/Views/CRMS/Enquiries/Dashboard.cshtml", enqdet);

        }


        public PartialViewResult PartialEnquiriesGrid(EnquiriesDet modelenquiriesdet, string isshowclicked)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Dashboard", "Enquiries");

            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.SupervisorFeedback = rights.SupervisorFeedback;
            ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
            ViewBag.Verified = rights.Verified;
            // Mantis Issue 24748
            ViewBag.ReassignSalesman = rights.CanReassignSalesman;
            // End of Mantis Issue 24748




            if (modelenquiriesdet.is_pageload != 0)
            {
                string datfrmat = modelenquiriesdet.FromDate.Split('-')[2] + '-' + modelenquiriesdet.FromDate.Split('-')[1] + '-' + modelenquiriesdet.FromDate.Split('-')[0];
                string dattoat = modelenquiriesdet.ToDate.Split('-')[2] + '-' + modelenquiriesdet.ToDate.Split('-')[1] + '-' + modelenquiriesdet.ToDate.Split('-')[0];
                _enquiries = new EnquiriesRepo();
                _enquiries.GetListing(modelenquiriesdet.EnquiriesFrom, datfrmat, dattoat);
            }
            //if (isshowclicked == "1")
            //{
                TempData["SearchKey"] = modelenquiriesdet;
                
                
            //}
            TempData["isshowclicked"] = isshowclicked;
            TempData.Keep();
            return PartialView("~/Views/CRMS/Enquiries/PartialEnquiriesGrid.cshtml", GetFormulaList(modelenquiriesdet.is_pageload));
        }

        //Rev Subhra 12-04-2019
        public PartialViewResult PartialMass()
        {
            return PartialView("~/Views/CRMS/Enquiries/_EnquriesMass.cshtml");
        }
        public PartialViewResult PartialMassDelete()
        {
            return PartialView("~/Views/CRMS/Enquiries/_EnquiriesMassDelete.cshtml", GetMassDeleteListing(1));
        }
        public IEnumerable GetMassDeleteListing(int is_pageload)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);

            if (is_pageload != 0)
            {
                CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                var q = from d in DC.ENQUIRIES_LISTINGs
                        where d.USERID == Convert.ToInt32(Userid) && d.SUPERVISOR == false && d.SALESMAN==false && d.VERIFY==false
                        orderby d.SEQ
                        select d;
                return q;

            }
            else
            {
                //CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                //var q = from d in DC.ENQUIRIES_LISTINGs
                //        where d.USERID == Convert.ToInt32(Userid) && d.SEQ == 11111111119
                //        orderby d.SEQ descending
                //        select d;
                //return q;
                return null;
            }
        }
        [HttpPost]
        public JsonResult EnquiriesMassDelete(string ActionType, string Uniqueid)
        {

            string output_msg = string.Empty;
            int ReturnCode = 0;
            _enquiries = new EnquiriesRepo();
            Msg _msg = new Msg();
            try
            {
                output_msg = _enquiries.MassDelete(ActionType, Uniqueid, ref ReturnCode);
                if (output_msg == "Deleted" && ReturnCode == 1)
                {
                    _msg.response_code = "Success";
                    _msg.response_msg = "Success";
                }
                else if (output_msg != "Success" && ReturnCode == -1)
                {
                    _msg.response_code = "Error";
                    _msg.response_msg = output_msg;
                }
                else
                {
                    _msg.response_code = "Error";
                    _msg.response_msg = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                _msg.response_code = "CatchError";
                _msg.response_msg = "Please try again later";
            }

            return Json(_msg, JsonRequestBehavior.AllowGet);
        }

        //End of Rev Subhra 12-04-2019
        // Mantis Issue 24748
        public PartialViewResult PartialBulk()
        {
            EnquiriesDet enquiriesDet = new EnquiriesDet();

            //enquiriesDet.selectedEnquirisList = Uniqueid;

            _enquiries = new EnquiriesRepo();

            // enquiriesDet.listAssignedSalesman = _enquiries.getAssignedSalesman();
            //enquiriesDet.AssIndustrylist = _enquiries.getIndustry();

            DataSet _getAssIndustry = new DataSet();
            List<AssIndustry> AssIndustrylist = new List<AssIndustry>();
            AssIndustry slind = null;
            // EnquiriesDet _apply = new EnquiriesDet();
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", "GetAssIndustry"));
                execProc.param = paramList;
                _getAssIndustry = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getAssIndustry.Tables[0].Rows)
                {
                    slind = new AssIndustry();
                    slind.ind_id = Convert.ToInt32(dr[0]);
                    slind.ind_industry = dr[1].ToString();
                    AssIndustrylist.Add(slind);
                }

            }
            catch (Exception ex)
            {

            }
            enquiriesDet.listAssIndustry = AssIndustrylist;

            TempData["AssignIndustryName"] = null;
            // Rev Mantis Issue 24878
            TempData["AssignBulkIndustryName"] = null;
            // End of Rev Mantis Issue 24878
            TempData.Keep();

            return PartialView("~/Views/CRMS/Enquiries/_EnquriesBulk.cshtml", enquiriesDet);
        }
        public PartialViewResult PartialBulkAssign()
        {
            return PartialView("~/Views/CRMS/Enquiries/_EnquiriesBulkAssign.cshtml", GetBulkAssignListing(1));
        }
        public IEnumerable GetBulkAssignListing(int is_pageload)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);
            string AssignIndustryName = "";
            if (TempData["AssignIndustryName"] != null)
            {
                AssignIndustryName = Convert.ToString(TempData["AssignIndustryName"]);
                TempData.Keep();
            }
            if (is_pageload != 0)
            {
                if (AssignIndustryName == "" || AssignIndustryName=="All")
                {
                    CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                    var q = from d in DC.ENQUIRIES_LISTINGs
                            // where d.USERID == Convert.ToInt32(Userid) && d.SUPERVISOR == false && d.SALESMAN == false && d.VERIFY == false && d.ASSIGNEDSALESMAN==""
                            where d.USERID == Convert.ToInt32(Userid) && d.ASSIGNEDSALESMAN == ""
                            orderby d.SEQ
                            select d;
                    return q;
                }
                else
                {
                    CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                    var q = from d in DC.ENQUIRIES_LISTINGs
                            // where d.USERID == Convert.ToInt32(Userid) && d.SUPERVISOR == false && d.SALESMAN == false && d.VERIFY == false && d.ASSIGNEDSALESMAN==""
                            where d.USERID == Convert.ToInt32(Userid) && d.ASSIGNEDSALESMAN == "" && d.INDUSTRY == AssignIndustryName
                            orderby d.SEQ
                            select d;
                    return q;
                }
                

            }
            else
            {
                //CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                //var q = from d in DC.ENQUIRIES_LISTINGs
                //        where d.USERID == Convert.ToInt32(Userid) && d.SEQ == 11111111119
                //        orderby d.SEQ descending
                //        select d;
                //return q;
                return null;
            }
        }
        public JsonResult SetAssindustryID(string Assindustry = "")
        {
            Boolean Success = false;
            try
            {
                TempData["AssignIndustryName"] = Assindustry;               
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }

        
        public JsonResult SetSalesmanID(string PriorSalesman = "")
        {
            Boolean Success = false;
            try
            {
                TempData["PriorSalesmanName"] = PriorSalesman;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }

        public PartialViewResult PartialPriority()
        {
            TempData["PriorSalesmanName"] = null;
            TempData.Keep();

            EnquiriesDet enquiriesDet = new EnquiriesDet();

            _enquiries = new EnquiriesRepo();

            DataSet _getSalesman = new DataSet();
            List<AssignedSalesman> Salesmanlist = new List<AssignedSalesman>();
            AssignedSalesman slman = null;
           
            try
            {
                //ExecProcedure execProc = new ExecProcedure();
                //List<KeyObj> paramList = new List<KeyObj>();
                //execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                //paramList.Add(new KeyObj("@ACTION_TYPE", "GetAssIndustry"));
                //execProc.param = paramList;
                //_getAssIndustry = execProc.ExecuteProcedureGetDataSet();
                //foreach (DataRow dr in _getAssIndustry.Tables[0].Rows)
                //{
                //    slind = new AssIndustry();
                //    slind.ind_id = Convert.ToInt32(dr[0]);
                //    slind.ind_industry = dr[1].ToString();
                //    AssIndustrylist.Add(slind);
                //}

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", "GetSalesmanlist"));
                execProc.param = paramList;
                _getSalesman = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getSalesman.Tables[0].Rows)
                {
                    slman = new AssignedSalesman();
                    slman.Salesman_Id = Convert.ToInt32(dr["Salesman_Id"]);
                    slman.Salesman_Name = Convert.ToString(dr["Salesman_Name"]);
                    Salesmanlist.Add(slman);
                }
            }
            catch (Exception ex)
            {

            }
            enquiriesDet.listAssignedSalesman = Salesmanlist;
            TempData["AssignIndustryName"] = null;
            // Rev Mantis Issue 24878
            TempData["AssignBulkIndustryName"] = null;
            // End of Rev Mantis Issue 24878
            TempData.Keep();

            return PartialView("~/Views/CRMS/Enquiries/_EnquriesPriority.cshtml", enquiriesDet);
        }

        public PartialViewResult PartialSetPriority()
        {
            return PartialView("~/Views/CRMS/Enquiries/_EnquriesSetPriority.cshtml", GetSetPriorityListing(1));
        }
        public IEnumerable GetSetPriorityListing(int is_pageload)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);
            string PriorSalesmanName = "";
            if (TempData["PriorSalesmanName"] != null)
            {
                PriorSalesmanName = Convert.ToString(TempData["PriorSalesmanName"]);
                TempData.Keep();
            }
            if (is_pageload != 0)
            {
                if (PriorSalesmanName == "" || PriorSalesmanName == "All")
                {
                    //CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                    //var q = from d in DC.ENQUIRIES_LISTINGs
                    //        // where d.USERID == Convert.ToInt32(Userid) && d.SUPERVISOR == false && d.SALESMAN == false && d.VERIFY == false && d.ASSIGNEDSALESMAN==""
                    //        where d.USERID == Convert.ToInt32(Userid) && d.ASSIGNEDSALESMAN != "" && d.SETPRIORITY == ""
                    //        orderby d.SEQ
                    //        select d;
                    //return q;
                    return null;
                }
                else
                {
                    CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                    var q = from d in DC.ENQUIRIES_LISTINGs
                            // where d.USERID == Convert.ToInt32(Userid) && d.SUPERVISOR == false && d.SALESMAN == false && d.VERIFY == false && d.ASSIGNEDSALESMAN==""
                            where d.USERID == Convert.ToInt32(Userid) && d.ASSIGNEDSALESMAN != "" && d.SETPRIORITY == "" && d.ASSIGNEDSALESMAN == PriorSalesmanName
                            orderby d.SEQ
                            select d;
                    return q;
                }


            }
            else
            {
                //CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                //var q = from d in DC.ENQUIRIES_LISTINGs
                //        where d.USERID == Convert.ToInt32(Userid) && d.SEQ == 11111111119
                //        orderby d.SEQ descending
                //        select d;
                //return q;
                return null;
            }
        }
        // End of Mantis Issue 24748
        public IEnumerable GetFormulaList(int is_pageload)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);

            if (is_pageload != 0)
            {
                CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                var q = from d in DC.ENQUIRIES_LISTINGs
                        where d.USERID == Convert.ToInt32(Userid)
                        orderby d.SEQ 
                        select d;
                return q;

            }
            else
            {
                //CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                //var q = from d in DC.ENQUIRIES_LISTINGs
                //        where d.USERID == Convert.ToInt32(Userid) && d.SEQ == 11111111119
                //        orderby d.SEQ descending
                //        select d;
                //return q;
                return null;
            }
        }

        //susanta chart
        public JsonResult GetChartBar(string id, string ACTION_TYPE, string Month)
        {
            DataSet _getSalesman = new DataSet();
            List<barModel> chartoneList = new List<barModel>();
            barModel slman = null;
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", "TotalEnqVSOrderWON"));
                paramList.Add(new KeyObj("@ASSIGNEDSALESMAN", id));
                paramList.Add(new KeyObj("@Month", Month));
                execProc.param = paramList;
                _getSalesman = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getSalesman.Tables[0].Rows)
                {
                    slman = new barModel();
                    slman.TOTAL = Convert.ToString(dr["TOTAL"]);
                    slman.TOTAL_WON = Convert.ToString(dr["TOTAL_WON"]);
                    chartoneList.Add(slman);
                }
                //return Salesmanlist
                	
            }
            catch (Exception ex)
            {
                //return message;
            }
            return Json(chartoneList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalesmanlist()
        {
            DataSet _getSalesman = new DataSet();
            List<salesmanModel> Salesmanlist = new List<salesmanModel>();
            salesmanModel slman = null;
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", "GetSalesmanlist"));
                execProc.param = paramList;
                _getSalesman = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getSalesman.Tables[0].Rows)
                {
                    slman = new salesmanModel();
                    slman.id = Convert.ToString(dr["Salesman_Id"]);
                    slman.name = Convert.ToString(dr["Salesman_Name"]);
                    Salesmanlist.Add(slman);
                }
                //return Salesmanlist

            }
            catch (Exception ex)
            {
                //return message;
            }
            return Json(Salesmanlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSalesmanlistH()
        {
            int user_id = Convert.ToInt32(Session["userid"]);
            DataSet _getSalesman = new DataSet();
            List<salesmanModel> Salesmanlist = new List<salesmanModel>();
            salesmanModel slman = null;
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", "GetSalesmanlistH"));
                paramList.Add(new KeyObj("@USERID", user_id));
                execProc.param = paramList;
                _getSalesman = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getSalesman.Tables[0].Rows)
                {
                    slman = new salesmanModel();
                    slman.id = Convert.ToString(dr["Salesman_Id"]);
                    slman.name = Convert.ToString(dr["Salesman_Name"]);
                    Salesmanlist.Add(slman);
                }
                //return Salesmanlist

            }
            catch (Exception ex)
            {
                //return message;
            }
            return Json(Salesmanlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetChartTwo(string id, string ACTION_TYPE)
        {
            DataSet _getSalesman = new DataSet();
            List<chartModel> charttwoList = new List<chartModel>();
            chartModel slman = null;
            try
            {
                	
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", ACTION_TYPE));
                paramList.Add(new KeyObj("@ASSIGNEDSALESMAN", id));
                execProc.param = paramList;
                _getSalesman = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getSalesman.Tables[0].Rows)
                {
                    slman = new chartModel();
                    slman.title = Convert.ToString(dr["Close_reason"]);
                    slman.value = Convert.ToString(dr["cnt_rec"]);
                    charttwoList.Add(slman);
                }
                //return Salesmanlist

            }
            catch (Exception ex)
            {
                //return message;
            }
            return Json(charttwoList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetChartOne(string id, string ACTION_TYPE)
        {
            DataSet _getSalesman = new DataSet();
            List<chartModel> chartoneList = new List<chartModel>();
            chartModel slman = null;
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", ACTION_TYPE));
                paramList.Add(new KeyObj("@ASSIGNEDSALESMAN", id));
                execProc.param = paramList;
                _getSalesman = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getSalesman.Tables[0].Rows)
                {
                    slman = new chartModel();
                    slman.title = Convert.ToString(dr["Close_Order"]);
                    slman.value = Convert.ToString(dr["cnt_rec"]);
                    chartoneList.Add(slman);
                }
                //return Salesmanlist
                	
            }
            catch (Exception ex)
            {
                //return message;
            }
            return Json(chartoneList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChartOneH()
        {
            int user_id = Convert.ToInt32(Session["userid"]);
            DataSet _getSalesman = new DataSet();
            List<chartModel> charttwoList = new List<chartModel>();
            chartModel slman = null;
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", "ClosureOfEnquiriesH"));
                paramList.Add(new KeyObj("@USERID", user_id));
                execProc.param = paramList;
                _getSalesman = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getSalesman.Tables[0].Rows)
                {
                    slman = new chartModel();
                    slman.title = Convert.ToString(dr["Close_Order"]);
                    slman.value = Convert.ToString(dr["cnt_rec"]);
                    charttwoList.Add(slman);
                }
                //return Salesmanlist

            }
            catch (Exception ex)
            {
                //return message;
            }
            return Json(charttwoList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetChartTwoH()
        {
            int user_id = Convert.ToInt32(Session["userid"]);
            DataSet _getSalesman = new DataSet();
            List<chartModel> charttwoList = new List<chartModel>();
            chartModel slman = null;
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", "LostReasonH"));
                paramList.Add(new KeyObj("@USERID", user_id));
                execProc.param = paramList;
                _getSalesman = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getSalesman.Tables[0].Rows)
                {
                    slman = new chartModel();
                    slman.title = Convert.ToString(dr["Close_reason"]);
                    slman.value = Convert.ToString(dr["cnt_rec"]);
                    charttwoList.Add(slman);
                }
                //return Salesmanlist

            }
            catch (Exception ex)
            {
                //return message;
            }
            return Json(charttwoList, JsonRequestBehavior.AllowGet);
        }
        // Mantis Issue 24835

        public JsonResult GetChartNew(string SalesmanId, string Month, string ProductClass, bool ConsiderReturn)
        {
            DataSet _getSalesman = new DataSet();
            List<targetModel> charttwoList = new List<targetModel>();
            targetModel slman = null;
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", "TARGET_ACHIEVED"));
                paramList.Add(new KeyObj("@SalesmanId", SalesmanId));
                paramList.Add(new KeyObj("@Month", Month));
                paramList.Add(new KeyObj("@ProductClass", ProductClass));
                paramList.Add(new KeyObj("@ConsiderReturn", ConsiderReturn));
                execProc.param = paramList;
                _getSalesman = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getSalesman.Tables[0].Rows)
                {
                    slman = new targetModel();
                    slman.Target = Convert.ToString(dr["Target"]);
                    slman.Achieved = Convert.ToString(dr["Achieved"]);
                    slman.ProductClass_Name = Convert.ToString(dr["ProductClass_Name"]);
                    charttwoList.Add(slman);
                }
                //return Salesmanlist

            }
            catch (Exception ex)
            {
                //return message;TargetAmount	TargetAchieved
            }
            return Json(charttwoList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChartNewHirarchy(string SalesmanId, string Month, string ProductClass, bool ConsiderReturn)
        { 
            DataSet _getSalesman = new DataSet();
            List<targetModel> charttwoList = new List<targetModel>();
            targetModel slman = null;
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", "TARGET_ACHIEVED"));
                paramList.Add(new KeyObj("@SalesmanId", SalesmanId));
                paramList.Add(new KeyObj("@Month", Month));
                paramList.Add(new KeyObj("@ProductClass", ProductClass));
                paramList.Add(new KeyObj("@ConsiderReturn", ConsiderReturn));
                execProc.param = paramList;
                _getSalesman = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getSalesman.Tables[0].Rows)
                {
                    slman = new targetModel();
                    slman.Target = Convert.ToString(dr["Target"]);
                    slman.Achieved = Convert.ToString(dr["Achieved"]);
                    slman.ProductClass_Name = Convert.ToString(dr["ProductClass_Name"]);
                    charttwoList.Add(slman);
                }
                //return Salesmanlist

            }
            catch (Exception ex)
            {
                //return message;TargetAmount	TargetAchieved
            }
            return Json(charttwoList, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetProductClass(string action)
        {
            DataSet _getProductClass = new DataSet();
            List<ProductClass> ProductClasslist = new List<ProductClass>();
            ProductClass pclass = null;
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", action));
                execProc.param = paramList;
                _getProductClass = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getProductClass.Tables[0].Rows)
                {
                    pclass = new ProductClass();
                    pclass.ProductClass_ID = Convert.ToInt32(dr["ProductClass_ID"]);
                    pclass.ProductClass_Name = Convert.ToString(dr["ProductClass_Name"]);
                    ProductClasslist.Add(pclass);
                }
                //return Salesmanlist

            }
            catch (Exception ex)
            {
                //return message;
            }
            return Json(ProductClasslist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTargetSalesmanlist()
        {
            DataSet _getSalesman = new DataSet();
            List<salesmanModel> Salesmanlist = new List<salesmanModel>();
            salesmanModel slman = null;
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", "GetTargetSalesmanlist"));
                execProc.param = paramList;
                _getSalesman = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getSalesman.Tables[0].Rows)
                {
                    slman = new salesmanModel();
                    slman.id = Convert.ToString(dr["Salesman_Id"]);
                    slman.name = Convert.ToString(dr["Salesman_Name"]);
                    Salesmanlist.Add(slman);
                }
                //return Salesmanlist

            }
            catch (Exception ex)
            {
                //return message;
            }
            return Json(Salesmanlist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTargetSalesmanlistH()
        {
            int user_id = Convert.ToInt32(Session["userid"]);
            DataSet _getSalesman = new DataSet();
            List<salesmanModel> Salesmanlist = new List<salesmanModel>();
            salesmanModel slman = null;
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", "GetTargetSalesmanlistH"));
                paramList.Add(new KeyObj("@USERID", user_id));
                execProc.param = paramList;
                _getSalesman = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getSalesman.Tables[0].Rows)
                {
                    slman = new salesmanModel();
                    slman.id = Convert.ToString(dr["Salesman_Id"]);
                    slman.name = Convert.ToString(dr["Salesman_Name"]);
                    Salesmanlist.Add(slman);
                }
                //return Salesmanlist

            }
            catch (Exception ex)
            {
                //return message;
            }
            return Json(Salesmanlist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSalesmanBudget(int SalesmanId, int Month, int ProductClass)
        {
            DataSet _getSalesmanBudget = new DataSet();
            List<SalesmanBudget> SalesmanBudgetlist = new List<SalesmanBudget>();
            SalesmanBudget slmanBud = null;
            
            try
            {

                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", "GetSalesmanBudget"));
                paramList.Add(new KeyObj("@SalesmanId", SalesmanId));
                paramList.Add(new KeyObj("@Month", Month));
                paramList.Add(new KeyObj("@ProductClass", ProductClass));
                execProc.param = paramList;
                _getSalesmanBudget = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getSalesmanBudget.Tables[0].Rows)
                {
                    slmanBud = new SalesmanBudget();
                    slmanBud.BudgetAmount = Convert.ToDecimal(dr["TotalBudget"]);
                    slmanBud.TargetAmount = Convert.ToDecimal(dr["TargetAmount"]);
                    slmanBud.TargetPercent = Convert.ToDecimal(dr["TargetPercent"]);
                    SalesmanBudgetlist.Add(slmanBud);
                    //TotalBudget = Convert.ToDecimal(dr["TotalBudget"]);
                }
                //return Salesmanlist

            }
            catch (Exception ex)
            {
                //return message;
            }

            return Json(SalesmanBudgetlist, JsonRequestBehavior.AllowGet);
            //return TotalBudget;
        }
        // End of Mantis Issue 24835
        public ActionResult Index(string ActionType, string _uniqueid)
        {
            TempData["SearchKey"] = (EnquiriesDet)TempData["SearchKey"];
            TempData.Keep();
            _enquiries = new EnquiriesRepo();
            EnquiriesDet _apply = new EnquiriesDet();
            //P_formula_header _header = new P_formula_header();
            //_apply.header = _header;

            if (ActionType == "ADD" && (_uniqueid == "" || _uniqueid == null))
            {
                _apply.HeaderName = "Add Enquiries";
                _apply.Action_type = ActionType;
                _apply.Date = DateTime.Today;
                ViewBag.Action = "ADD";
                ViewBag.title = "Enquiries";
            }
            else if (ActionType == "EDIT" && _uniqueid != "")
            {
                _apply = _enquiries.getEnquiryById(_uniqueid, ActionType);
                // ViewBag.dtls = _apply.dtls;
                ViewBag.uniqueid = _uniqueid;
                ViewBag.Action = "EDIT";
                _apply.HeaderName = "Modify Enquiries";
                ViewBag.title = "Enquiries";

            }
            //EnquiriesDet enquiriesmodel = new EnquiriesDet();

            return View("~/Views/CRMS/Enquiries/Index.cshtml", _apply);

        }
        [HttpPost]
        public JsonResult Apply(EnquiriesDet apply, string uniqueid)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _enquiries = new EnquiriesRepo();
            try
            {
                _enquiries.save(apply, uniqueid, ref ReturnCode, ref ReturnMsg);
                if (ReturnMsg == "Success" && ReturnCode == 1)
                {
                    apply.response_code = "Success";
                    apply.response_msg = "Success";
                }
                else if (ReturnMsg != "Success" && ReturnCode == -1)
                {
                    apply.response_code = "Error";
                    apply.response_msg = ReturnMsg;
                }
                else
                {
                    apply.response_code = "Error";
                    apply.response_msg = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                apply.response_code = "CatchError";
                apply.response_msg = "Please try again later";
            }

            return Json(apply, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SupervisorApply(EnquiriesSupervisorFeedback supervisorapply)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _enquiries = new EnquiriesRepo();
            try
            {
                _enquiries.Supervisorsave(supervisorapply, ref ReturnCode, ref ReturnMsg);
                if (ReturnMsg == "Success" && ReturnCode == 1)
                {
                    supervisorapply.response_code = "Success";
                    supervisorapply.response_msg = "Success";
                }
                else if (ReturnMsg != "Success" && ReturnCode == -1)
                {
                    supervisorapply.response_code = "Error";
                    supervisorapply.response_msg = ReturnMsg;
                }
                else
                {
                    supervisorapply.response_code = "Error";
                    supervisorapply.response_msg = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                supervisorapply.response_code = "CatchError";
                supervisorapply.response_msg = "Please try again later";
            }

            return Json(supervisorapply, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EnquiriesDelete(string ActionType, string Uniqueid)
        {

            string output_msg = string.Empty;
            int ReturnCode = 0;
            _enquiries = new EnquiriesRepo();
            Msg _msg = new Msg();
            try
            {
                output_msg = _enquiries.Delete(ActionType, Uniqueid, ref ReturnCode);
                if (output_msg == "Deleted" && ReturnCode == 1)
                {
                    _msg.response_code = "Success";
                    _msg.response_msg = "Success";
                }
                //else if (output_msg != "Success" && ReturnCode == -1)
                //{
                //    _msg.response_code = "Error";
                //    _msg.response_msg = output_msg;
                //}
                else
                {
                    //_msg.response_code = "Error";
                    //_msg.response_msg = "Please try again later";
                    _msg.response_code = "Error";
                    _msg.response_msg = output_msg;
                }

            }

            catch (Exception ex)
            {
                _msg.response_code = "CatchError";
                _msg.response_msg = "Please try again later";
            }

            return Json(_msg, JsonRequestBehavior.AllowGet);
        }

        //Rev Subhra  08-04-2019 Mail Reference(please remove this restriction)

        [HttpPost]
        public JsonResult EnquiriesRestor(string ActionType, string Uniqueid)
        {

            string output_msg = string.Empty;
            int ReturnCode = 0;
            _enquiries = new EnquiriesRepo();
            Msg _msg = new Msg();
            try
            {
                output_msg = _enquiries.Delete(ActionType, Uniqueid, ref ReturnCode);
                if (output_msg == "Deleted" && ReturnCode == 1)
                {
                    _msg.response_code = "Success";
                    _msg.response_msg = "Success";
                }
                else if (output_msg != "Success" && ReturnCode == -1)
                {
                    _msg.response_code = "Error";
                    _msg.response_msg = output_msg;
                }
                else
                {
                    _msg.response_code = "Error";
                    _msg.response_msg = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                _msg.response_code = "CatchError";
                _msg.response_msg = "Please try again later";
            }

            return Json(_msg, JsonRequestBehavior.AllowGet);
        }
        //End of Rev Subhra  08-04-2019
        public ActionResult EnquirisSupervisor(string Unique_ID, string vend_type)
        {
            ViewBag.supervisoruniqueid = Unique_ID;
            EnquiriesSupervisorFeedback enquiriesSupervisorFeedback = new EnquiriesSupervisorFeedback();
            _enquiries = new EnquiriesRepo();

            enquiriesSupervisorFeedback = _enquiries.getSuperVisorById(Unique_ID);
            enquiriesSupervisorFeedback.source = vend_type;
            enquiriesSupervisorFeedback.Unique_ID = Unique_ID;

            enquiriesSupervisorFeedback.listIndustry = _enquiries.getIndustry();

            List<Priority> listofpriority = new List<Priority>();
            //EnquiriesDet enqdet = new EnquiriesDet();
            Priority priority = new Priority();

            priority.priorityName = "Low";
            priority.priorityID = 0;
            listofpriority.Add(priority);

            priority = new Priority();
            priority.priorityName = "Normal";
            priority.priorityID = 1;
            listofpriority.Add(priority);

            priority = new Priority();
            priority.priorityName = "High";
            priority.priorityID = 2;
            listofpriority.Add(priority);

            priority = new Priority();
            priority.priorityName = "Urgent";
            priority.priorityID = 3;
            listofpriority.Add(priority);

            priority = new Priority();
            priority.priorityName = "Immediate";
            priority.priorityID = 4;
            listofpriority.Add(priority);

            enquiriesSupervisorFeedback.listpriority = listofpriority;

            List<ExistOrNotCustomer> isexistcust = new List<ExistOrNotCustomer>();
            ExistOrNotCustomer existcust = new ExistOrNotCustomer();
            existcust.Id = "Y";
            existcust.Name = "Yes";
            isexistcust.Add(existcust);

            existcust = new ExistOrNotCustomer();
            existcust.Id = "N";
            existcust.Name = "No";
            isexistcust.Add(existcust);
            enquiriesSupervisorFeedback.listExistOrNotCustomer = isexistcust;

            return PartialView("/Views/CRMS/Enquiries/_EnquirisSupervisor.cshtml", enquiriesSupervisorFeedback);
        }

        public ActionResult EnquirisSalesman(string Unique_ID, string SOURCE, string INDUSTRY, string MISC_COMMENTS, string PRIORITYS, string EXIST_CUST)
        {
            ViewBag.salesmanuniqueid = Unique_ID;
            EnquiriesSalesmanFeedback enquiriesSalesmanFeedback = new EnquiriesSalesmanFeedback();

            _enquiries = new EnquiriesRepo();
            enquiriesSalesmanFeedback = _enquiries.getSalesmanById(Unique_ID);
           
                if (enquiriesSalesmanFeedback.last_contactdate == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                {
                    enquiriesSalesmanFeedback.next_contactdate = DateTime.Now.AddDays(15);
                }
              

            enquiriesSalesmanFeedback.listIndustry = _enquiries.getIndustry();
            enquiriesSalesmanFeedback.Unique_ID = Unique_ID;
            enquiriesSalesmanFeedback.source = SOURCE;
            enquiriesSalesmanFeedback.Industry = INDUSTRY;
            enquiriesSalesmanFeedback.Misc_comments = MISC_COMMENTS;
            enquiriesSalesmanFeedback.priorityName = PRIORITYS;
            enquiriesSalesmanFeedback.checkedcustomer = EXIST_CUST;

            List<Priority> listofpriority = new List<Priority>();


            List<Usefull> listisUsefull = new List<Usefull>();
            Usefull isusefull = new Usefull();
            isusefull.Id = "Y";
            isusefull.Name = "Usefull";
            listisUsefull.Add(isusefull);

            isusefull = new Usefull();
            isusefull.Id = "N";
            isusefull.Name = "No Use";
            listisUsefull.Add(isusefull);
            enquiriesSalesmanFeedback.isusefull = listisUsefull;
            enquiriesSalesmanFeedback.listsalesman = _enquiries.getSalesman();
            return PartialView("/Views/CRMS/Enquiries/_EnquirisSalesman.cshtml", enquiriesSalesmanFeedback);
        }
        [HttpPost]
        public JsonResult SalemanApply(EnquiriesSalesmanFeedback salesmanapply)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _enquiries = new EnquiriesRepo();
            try
            {
                _enquiries.Salesmansave(salesmanapply, ref ReturnCode, ref ReturnMsg);
                if (ReturnMsg == "Success" && ReturnCode == 1)
                {
                    salesmanapply.response_code = "Success";
                    salesmanapply.response_msg = "Success";
                }
                else if (ReturnMsg != "Success" && ReturnCode == -1)
                {
                    salesmanapply.response_code = "Error";
                    salesmanapply.response_msg = ReturnMsg;
                }
                else
                {
                    salesmanapply.response_code = "Error";
                    salesmanapply.response_msg = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                salesmanapply.response_code = "CatchError";
                salesmanapply.response_msg = "Please try again later";
            }

            return Json(salesmanapply, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult EnquirisSalesmanShowHistory(string Unique_ID)
        {
            //ViewBag.salesmanuniqueid = Unique_ID;
            EnquiriesShowHistorySalesman enquiriesSalesmanFeedback = new EnquiriesShowHistorySalesman();
            List<EnquiriesShowHistorySalesman> objList = new List<EnquiriesShowHistorySalesman>();

            _enquiries = new EnquiriesRepo();
            objList = _enquiries.getShowHistorySalesmanById(Unique_ID);
            return PartialView("~/Views/CRMS/Enquiries/_EnquirisShowHistorySalesman.cshtml", objList);
        }

        public ActionResult EnquirisVerified(string Unique_ID)
        {
            _enquiries = new EnquiriesRepo();
            ViewBag.verifyuniqueid = Unique_ID;
            EnquiriesVerify enquiriesVerify = new EnquiriesVerify();
            enquiriesVerify = _enquiries.getVerifyId(Unique_ID);
            enquiriesVerify.Unique_ID = Unique_ID;
            _enquiries = new EnquiriesRepo();
            enquiriesVerify.listemployee = _enquiries.getEmployee();


            return PartialView("/Views/CRMS/Enquiries/_EnquirisVerify.cshtml", enquiriesVerify);
        }

        [HttpPost]
        public JsonResult VerifyApply(EnquiriesVerify verifyapply)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _enquiries = new EnquiriesRepo();
            try
            {
                _enquiries.Verifiedsave(verifyapply, ref ReturnCode, ref ReturnMsg);
                if (ReturnMsg == "Success" && ReturnCode == 1)
                {
                    verifyapply.response_code = "Success";
                    verifyapply.response_msg = "Success";
                }
                else if (ReturnMsg != "Success" && ReturnCode == -1)
                {
                    verifyapply.response_code = "Error";
                    verifyapply.response_msg = ReturnMsg;
                }
                else
                {
                    verifyapply.response_code = "Error";
                    verifyapply.response_msg = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                verifyapply.response_code = "CatchError";
                verifyapply.response_msg = "Please try again later";
            }

            return Json(verifyapply, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportEnquiriesList(int type)
        {


            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToPdf(GetEmployeeBatchGridViewSettings(), GetFormulaList(1));
                //break;
                case 2:
                    return GridViewExtension.ExportToXlsx(GetEmployeeBatchGridViewSettings(), GetFormulaList(1));
                //break;
                case 3:
                    return GridViewExtension.ExportToXls(GetEmployeeBatchGridViewSettings(), GetFormulaList(1));
                case 4:
                    return GridViewExtension.ExportToRtf(GetEmployeeBatchGridViewSettings(), GetFormulaList(1));
                case 5:
                    return GridViewExtension.ExportToCsv(GetEmployeeBatchGridViewSettings(), GetFormulaList(1));
                //break;

                default:
                    break;
            }

            return null;
        }

        private GridViewSettings GetEmployeeBatchGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "gvPaging";
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Enquiries List";

            settings.Columns.Add(column =>
            {
                column.Caption = "Date & Time";
                column.FieldName = "DATE";
                column.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy hh:mm:ss";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "CUSTOMER_NAME";
                column.Caption = "Customer";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "CONTACT_PERSON";
                column.Caption = "Contact Person";
            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "PHONENO";
                column.Caption = "Contact No";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "EMAIL";
                column.Caption = "Email";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "LOCATION";
                column.Caption = "Location";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "PRODUCT_REQUIRED";
                column.Caption = "Product Req.";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "QTY";
                column.Caption = "Qty.";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ORDER_VALUE";
                column.Caption = "Order Value";
                column.PropertiesEdit.DisplayFormatString = "0.00";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ENQ_DETAILS";
                column.Caption = "Enquiry Details";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "VEND_TYPE";
                column.Caption = "Provided By";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ENQ_NUMBER";
                column.Caption = "Enq. No."; 
            });

           
            //settings.Columns.Add(column =>
            //{
            //    column.FieldName = "SOURCE";
            //    column.Caption = "Source";
            //});


            settings.Columns.Add(column =>
            {
                column.FieldName = "INDUSTRY";
                column.Caption = "Industry";
            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "MISC_COMMENTS";
                column.Caption = "Misc Comments";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "PRIORITYS";
                column.Caption = "Priority";
                column.VisibleIndex = 16;
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 100;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "EXIST_CUST";
                column.Caption = "Existing Customer";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "LAST_CONTACT_DATE";
                column.Caption = "Last Contact Date";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "NEXT_CONTACT_DATE";
                column.Caption = "Next Contact Date";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "CONTACTEDBY";
                column.Caption = "Contacted By";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ENQ_PRODREQ";
                column.Caption = "Product Required";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "FEEDBACK";
                column.Caption = "Feedback";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "FINAL_INDUSTRY";
                column.Caption = "Final Industry";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ACTIVITY";
                column.Caption = "Activity";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "VERIFY_BY";
                column.Caption = "Verified By";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "VERIFY_ON";
                column.Caption = "Verified On";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "VERIFY_CLOSUREDATE";
                column.Caption = "Closure Date";
            });
            // Mantis Issue 24748
            settings.Columns.Add(column =>
            {
                column.FieldName = "ASSIGNEDSALESMAN";
                column.Caption = "Assigned user";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = 110;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "SALESMANASSIGNEDBY";
                column.Caption = "Assigned By";
                column.Width = 96;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "SALESMANASSIGNREM";
                column.Caption = "Remarks";
                column.Width = 96;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "SALESMANASSIGNDT";
                column.Caption = "Assigned Date & Time";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy hh:mm:ss";
                //column.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
                column.Width = 150;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "SETPRIORITY";
                column.Caption = "Priority";
                column.Width = 90;
            });

            // End of Mantis Issue 24748

            // Mantis Issue 24993
            settings.Columns.Add(column =>
            {
                column.FieldName = "CLOSE_ORDER";
                column.Caption = "Order Outcome";
                column.Width = 90;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "CLOSE_REASON";
                column.Caption = "Close Reason";
                column.Width = 90;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "CLOSE_REMARKS";
                column.Caption = "Close Remarks";
                column.Width = 100;
            });
            //Mantis Issue 25025
            settings.Columns.Add(column =>
            {
                column.FieldName = "CLOSEDDT";
                column.Caption = "Closed Date";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy hh:mm:ss";
                //column.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
                column.Width = 80;
                //column.FixedStyle = GridViewColumnFixedStyle.Left;
            });
            //End of Mantis Issue 25025
            settings.Columns.Add(column =>
            {
                column.FieldName = "REOPEN_FEEDBACK";
                column.Caption = "Reopen Feedback";
                column.Width = 100;
            });
            // End of Mantis Issue 24993
            //Mantis Issue 24999
            settings.Columns.Add(column =>
            {
                column.FieldName = "CREATED_DATE";
                column.Caption = "Created Date";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy hh:mm:ss";
                //column.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
                column.Width = 80;
                //column.FixedStyle = GridViewColumnFixedStyle.Left;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "CREATED_BY";
                column.Caption = "Created By";
                column.Width = 100;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "MODIFIED_DATE";
                column.Caption = "Edited Date";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy hh:mm:ss";
                //column.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
                column.Width = 80;
                //column.FixedStyle = GridViewColumnFixedStyle.Left;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "MODIFIED_BY";
                column.Caption = "Edited By";
                column.Width = 100;
            });
            //End of Mantis Issue 24999

            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }

        // Mantis Issue 24748
        public ActionResult BulkAssignSalesman(string ActionType, string Uniqueid)
        {
            //ViewBag.selectedEnquirisList = Uniqueid;
            EnquiriesDet enquiriesDet = new EnquiriesDet();

            enquiriesDet.selectedEnquirisList = Uniqueid;

            _enquiries = new EnquiriesRepo();

           // enquiriesDet.listAssignedSalesman = _enquiries.getAssignedSalesman();

            DataSet _getSalesman = new DataSet();
            List<AssignedSalesman> Salesmanlist = new List<AssignedSalesman>();
            AssignedSalesman slman = null;
            // EnquiriesDet _apply = new EnquiriesDet();
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", "GetSalesmanlist"));
                execProc.param = paramList;
                _getSalesman = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getSalesman.Tables[0].Rows)
                {
                    slman = new AssignedSalesman();
                    slman.Salesman_Id = Convert.ToInt32(dr[0]);
                    slman.Salesman_Name = dr[1].ToString();
                    Salesmanlist.Add(slman);
                }

            }
            catch (Exception ex)
            {

            }
            enquiriesDet.listAssignedSalesman = Salesmanlist;

            // Rev Mantis Issue 24878
            DataSet _getAssIndustry = new DataSet();
            List<AssIndustry> AssIndustrylist = new List<AssIndustry>();
            AssIndustry slind = null;
            // EnquiriesDet _apply = new EnquiriesDet();
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", "GetAssIndustryBulk"));
                execProc.param = paramList;
                _getAssIndustry = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getAssIndustry.Tables[0].Rows)
                {
                    slind = new AssIndustry();
                    slind.ind_id = Convert.ToInt32(dr[0]);
                    slind.ind_industry = dr[1].ToString();
                    AssIndustrylist.Add(slind);
                }

            }
            catch (Exception ex)
            {

            }
            enquiriesDet.listAssIndustry = AssIndustrylist;
            // End of Rev Mantis Issue 24878

            return PartialView("/Views/CRMS/Enquiries/_EnquirisAssignedSalesman.cshtml", enquiriesDet);
        }
        public JsonResult SaveAssignedSalesman(EnquiriesDet AssignedSalesman)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _enquiries = new EnquiriesRepo();
            try
            {
                //_enquiries.SaveAssignedSalesman(AssignedSalesman, ref ReturnCode, ref ReturnMsg);

                string action = string.Empty;
                DataTable formula_dtls = new DataTable();
                DataSet dsInst = new DataSet();

                try
                {

                    if (Session["userid"] != null)
                    {
                        int user_id = Convert.ToInt32(Session["userid"]);
                        ExecProcedure execProc = new ExecProcedure();
                        List<KeyObj> paramList = new List<KeyObj>();
                        execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";

                        paramList.Add(new KeyObj("@USERID", user_id));
                        paramList.Add(new KeyObj("@ACTION_TYPE", "BULKASSIGN"));
                        paramList.Add(new KeyObj("@ENQ_CRM_ID", AssignedSalesman.selectedEnquirisList));
                        paramList.Add(new KeyObj("@ASSIGNEDSALESMAN", AssignedSalesman.AssignedSalesmanId));
                        paramList.Add(new KeyObj("@ASSIGNEDSALEREMARKS", AssignedSalesman.AssignedSaleRemarks));
                        // Rev Mantis Issue 24878
                        paramList.Add(new KeyObj("@ASSIGNINDUSTRY", AssignedSalesman.AssignIndustryID));
                        // End of Rev Mantis Issue 24878
                        paramList.Add(new KeyObj("@RETURNMESSAGE", ReturnMsg, true));
                        paramList.Add(new KeyObj("@RETURNCODE", ReturnCode, true));

                        execProc.param = paramList;
                        execProc.ExecuteProcedureNonQuery();
                        paramList.Clear();
                        ReturnMsg = Convert.ToString(execProc.outputPara[0].value);
                        ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);

                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

                if (ReturnMsg == "Success" && ReturnCode == 1)
                {
                    AssignedSalesman.response_code = "Success";
                    AssignedSalesman.response_msg = "Success";
                }
                else if (ReturnMsg != "Success" && ReturnCode == -1)
                {
                    AssignedSalesman.response_code = "Error";
                    AssignedSalesman.response_msg = ReturnMsg;
                }
                else
                {
                    AssignedSalesman.response_code = "Error";
                    AssignedSalesman.response_msg = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                AssignedSalesman.response_code = "CatchError";
                AssignedSalesman.response_msg = "Please try again later";
            }

            return Json(AssignedSalesman, JsonRequestBehavior.AllowGet);
        }


        public string SavePriority(string Uniqueid)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _enquiries = new EnquiriesRepo();
            try
            {
                //_enquiries.SaveAssignedSalesman(AssignedSalesman, ref ReturnCode, ref ReturnMsg);

                string action = string.Empty;
                DataTable formula_dtls = new DataTable();
                DataSet dsInst = new DataSet();

                try
                {

                    if (Session["userid"] != null)
                    {
                        int user_id = Convert.ToInt32(Session["userid"]);
                        ExecProcedure execProc = new ExecProcedure();
                        List<KeyObj> paramList = new List<KeyObj>();
                        execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";

                        paramList.Add(new KeyObj("@USERID", user_id));
                        paramList.Add(new KeyObj("@ACTION_TYPE", "SAVEPRIORITY"));
                        paramList.Add(new KeyObj("@ENQ_CRM_ID", Uniqueid));
                        paramList.Add(new KeyObj("@RETURNMESSAGE", ReturnMsg, true));
                        paramList.Add(new KeyObj("@RETURNCODE", ReturnCode, true));

                        execProc.param = paramList;
                        execProc.ExecuteProcedureNonQuery();
                        paramList.Clear();
                        ReturnMsg = Convert.ToString(execProc.outputPara[0].value);
                        ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);

                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

                if (ReturnMsg == "Success" && ReturnCode == 1)
                {
                    //AssignedSalesman.response_code = "Success";
                    ReturnMsg = "Success";
                }
                else if (ReturnMsg != "Success" && ReturnCode == -1)
                {
                   // AssignedSalesman.response_code = "Error";
                    //ReturnMsg = ReturnMsg;
                }
                else
                {
                   // AssignedSalesman.response_code = "Error";
                    ReturnMsg = "Please try again later";
                }

            }

            catch (Exception ex)
            {
               // AssignedSalesman.response_code = "CatchError";
                ReturnMsg = "Please try again later";
            }

            return ReturnMsg;
        }
        // End of Mantis Issue 24748
        // Mantis Issue 24835
        public string SaveSalesmanTarget(int SalesmanId, int Month, int ProductClass, decimal target, decimal TotalBudget, decimal MonthlyBudget, decimal TargetAmount)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            //  _enquiries = new EnquiriesRepo();
            try
            {
                //_enquiries.SaveAssignedSalesman(AssignedSalesman, ref ReturnCode, ref ReturnMsg);

                string action = string.Empty;
                DataTable formula_dtls = new DataTable();
                DataSet dsInst = new DataSet();

                try
                {

                    if (Session["userid"] != null)
                    {
                        int user_id = Convert.ToInt32(Session["userid"]);
                        ExecProcedure execProc = new ExecProcedure();
                        List<KeyObj> paramList = new List<KeyObj>();
                        execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";

                        paramList.Add(new KeyObj("@USERID", user_id));
                        paramList.Add(new KeyObj("@ACTION_TYPE", "SAVESALESMANTARGET"));
                        paramList.Add(new KeyObj("@SalesmanId", SalesmanId));
                        paramList.Add(new KeyObj("@Month", Month));
                        paramList.Add(new KeyObj("@ProductClass", ProductClass));
                        paramList.Add(new KeyObj("@target", target));
                        paramList.Add(new KeyObj("@TotalBudget", TotalBudget));
                        paramList.Add(new KeyObj("@MonthlyBudget", MonthlyBudget));
                        paramList.Add(new KeyObj("@TargetAmount", TargetAmount));
                        paramList.Add(new KeyObj("@RETURNMESSAGE", ReturnMsg, true));
                        paramList.Add(new KeyObj("@RETURNCODE", ReturnCode, true));

                        execProc.param = paramList;
                        execProc.ExecuteProcedureNonQuery();
                        paramList.Clear();
                        ReturnMsg = Convert.ToString(execProc.outputPara[0].value);
                        ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);

                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

                if (ReturnMsg == "Success" && ReturnCode == 1)
                {
                    //AssignedSalesman.response_code = "Success";
                    ReturnMsg = "Success";
                }
                else if (ReturnMsg != "Success" && ReturnCode == -1)
                {
                    // AssignedSalesman.response_code = "Error";
                    //ReturnMsg = ReturnMsg;
                }
                else
                {
                    // AssignedSalesman.response_code = "Error";
                    ReturnMsg = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                // AssignedSalesman.response_code = "CatchError";
                ReturnMsg = "Please try again later";
            }

            return ReturnMsg;
        }
        // End of Mantis Issue 24835

        // Mantis Issue 24903
        public string chkIndustryExists(string selectedEnquirisList)
        {
            //int ReturnCode = 0;
            string ReturnMsg = "";

            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";

                paramList.Add(new KeyObj("@ACTION_TYPE", "chkIndustryExists"));
                paramList.Add(new KeyObj("@ENQ_CRM_ID", selectedEnquirisList));
                paramList.Add(new KeyObj("@RETURNMESSAGE", ReturnMsg, true));
                //paramList.Add(new KeyObj("@RETURNCODE", ReturnCode, true));

                execProc.param = paramList;
                execProc.ExecuteProcedureNonQuery();
                paramList.Clear();
                ReturnMsg = Convert.ToString(execProc.outputPara[0].value);
               // ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);

            }
            catch (Exception ex)
            {
                throw;
            }

            return ReturnMsg;

        }
        // End of Mantis Issue 24903

        // Mantis Issue 24990
        public string RecalculateBudget(int SalesmanId, int Month)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            //  _enquiries = new EnquiriesRepo();
            try
            {
                //_enquiries.SaveAssignedSalesman(AssignedSalesman, ref ReturnCode, ref ReturnMsg);

                string action = string.Empty;
                DataTable formula_dtls = new DataTable();
                DataSet dsInst = new DataSet();

                try
                {

                    if (Session["userid"] != null)
                    {
                        int user_id = Convert.ToInt32(Session["userid"]);
                        ExecProcedure execProc = new ExecProcedure();
                        List<KeyObj> paramList = new List<KeyObj>();
                        execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";

                        paramList.Add(new KeyObj("@USERID", user_id));
                        paramList.Add(new KeyObj("@ACTION_TYPE", "RecalculateBudget"));
                        paramList.Add(new KeyObj("@SalesmanId", SalesmanId));
                        paramList.Add(new KeyObj("@Month", Month));
                        paramList.Add(new KeyObj("@RETURNMESSAGE", ReturnMsg, true));
                        paramList.Add(new KeyObj("@RETURNCODE", ReturnCode, true));

                        execProc.param = paramList;
                        execProc.ExecuteProcedureNonQuery();
                        paramList.Clear();
                        ReturnMsg = Convert.ToString(execProc.outputPara[0].value);
                        ReturnCode = Convert.ToInt32(execProc.outputPara[1].value);

                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

                if (ReturnMsg == "Success" && ReturnCode == 1)
                {
                    //AssignedSalesman.response_code = "Success";
                    ReturnMsg = "Success";
                }
                else if (ReturnMsg != "Success" && ReturnCode == -1)
                {
                    // AssignedSalesman.response_code = "Error";
                    //ReturnMsg = ReturnMsg;
                }
                else
                {
                    // AssignedSalesman.response_code = "Error";
                    ReturnMsg = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                // AssignedSalesman.response_code = "CatchError";
                ReturnMsg = "Please try again later";
            }

            return ReturnMsg;
        }
        // End of Mantis Issue 24990

        //Mantis Issue 24852
        public ActionResult DownloadFormat()
        {
            string FileName = "EnquiriesList.xlsx";
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "image/jpeg";
            response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
            response.TransmitFile(Server.MapPath("~/CommonFolder/EnquiriesList.xlsx"));
            response.Flush();
            response.End();

            return null;
        }
        public ActionResult ImportExcel()
        {

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        String extension = Path.GetExtension(fname);
                        fname = DateTime.Now.Ticks.ToString() + extension;
                        fname = Path.Combine(Server.MapPath("~/Temporary/"), fname);
                        file.SaveAs(fname);
                        Import_To_Grid(fname, extension, file);
                    }
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        public Int32 Import_To_Grid(string FilePath, string Extension, HttpPostedFileBase file)
        {
            Boolean Success = false;
            Int32 HasLog = 0;

            if (file.FileName.Trim() != "")
            {
                if (Extension.ToUpper() == ".XLS" || Extension.ToUpper() == ".XLSX")
                {
                    DataTable dt = new DataTable();

                    string conString = string.Empty;
                    conString = ConfigurationManager.AppSettings["ExcelConString"];
                    conString = string.Format(conString, FilePath);
                    using (OleDbConnection excel_con = new OleDbConnection(conString))
                    {
                        excel_con.Open();
                        string sheet1 = "List$";

                        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                        {
                            oda.Fill(dt);
                        }
                        excel_con.Close();
                    }


                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataTable dtExcelData = new DataTable();
                        dtExcelData.Columns.Add("Date", typeof(string));
                        dtExcelData.Columns.Add("Customer Name", typeof(string));
                        dtExcelData.Columns.Add("Contact Person", typeof(string));
                        dtExcelData.Columns.Add("Phone No", typeof(string));
                        dtExcelData.Columns.Add("Email", typeof(string));
                        dtExcelData.Columns.Add("Location", typeof(string));
                        dtExcelData.Columns.Add("Product Required", typeof(string));
                        dtExcelData.Columns.Add("Quantity", typeof(string));
                        dtExcelData.Columns.Add("Order Value", typeof(string));
                        dtExcelData.Columns.Add("Enquiry Details", typeof(string));

                        foreach (DataRow row in dt.Rows)
                        {

                            if (Convert.ToString(row["Date"]) != "" || Convert.ToString(row["Customer Name"]) != "")
                            {
                                dtExcelData.Rows.Add(Convert.ToString(row["Date"]), Convert.ToString(row["Customer Name"]), Convert.ToString(row["Contact Person"]), Convert.ToString(row["Phone No"]), Convert.ToString(row["Email"]),
                                                Convert.ToString(row["Location"]), Convert.ToString(row["Product Required"]), Convert.ToString(row["Quantity"]), Convert.ToString(row["Order Value"]), Convert.ToString(row["Enquiry Details"])
                                                );
                            }

                        }

                        try
                        {
                            TempData["PartyEnquiryLog"] = dtExcelData;
                            TempData.Keep();

                            DataTable dtCmb = new DataTable();
                            ProcedureExecute proc = new ProcedureExecute("PRC_ImportNewEnquiry");
                            proc.AddPara("@Enquiry_TABLE", dtExcelData);
                            proc.AddPara("@CreateUser_Id", Convert.ToInt32(Session["userid"]));
                            dtCmb = proc.GetTable();

                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            //GetEnquiryImportLog();
            return HasLog;
        }
        public ActionResult GetEnquiryImportLog()
        {
            List<EnquiryDetailsModel> list = new List<EnquiryDetailsModel>();
            DataTable dt = new DataTable();
            try
            {
                if (TempData["PartyEnquiryLog"] != null)
                {
                    DataTable dtCmb = new DataTable();
                    ProcedureExecute proc = new ProcedureExecute("PRC_ImportEnquiryLog");
                    proc.AddPara("@Enquiry_TABLE", (DataTable)TempData["PartyEnquiryLog"]);
                    proc.AddPara("@CreateUser_Id", Convert.ToInt32(Session["userid"]));
                    dt = proc.GetTable();
                    TempData.Keep();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        EnquiryDetailsModel data = null;
                        foreach (DataRow row in dt.Rows)
                        {
                            data = new EnquiryDetailsModel();
                            data.Date = Convert.ToString(row["Date"]);
                            data.Customer_Name = Convert.ToString(row["Customer Name"]);
                            data.Contact_Person = Convert.ToString(row["Contact Person"]);
                            data.PhoneNo = Convert.ToString(row["Phone No"]);
                            data.Email = Convert.ToString(row["Email"]);
                            data.Location = Convert.ToString(row["Location"]);
                            data.Product_Required = Convert.ToString(row["Product Required"]);
                            data.Qty = Convert.ToString(row["Quantity"]);
                            data.Order_Value = Convert.ToString(row["Order Value"]);
                            data.Enq_Details = Convert.ToString(row["Enquiry Details"]);
                            data.Created_Date = Convert.ToString(row["ImportDate"]);
                            data.ImportMsg = Convert.ToString(row["ImportMsg"]);
                            list.Add(data);
                        }
                    }
                    //TempData["PartyImportLog"] = dt;
                }

            }
            catch (Exception ex)
            {

            }
            TempData.Keep();
            return PartialView(list);
        }
        //End of Mantis Issue 24852
    }
}