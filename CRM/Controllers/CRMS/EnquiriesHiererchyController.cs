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

namespace CRM.Controllers.CRMS
{
     [CRM.Models.Attributes.SessionTimeout]

    public class EnquiriesHiererchyController : Controller
    {
        private IEnquiries _enquiries;
        UserRightsForPage rights = new UserRightsForPage();

        public ActionResult Dashboard()
        {

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);


            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Dashboard", "EnquiriesHiererchy");


            List<EnquriesFrom> listenquriesfrom = new List<EnquriesFrom>();
            EnquiriesDet enqdet = new EnquiriesDet();
            EnquriesFrom enquriesfrom = new EnquriesFrom();
            TempData["AssignIndustryName"] = null;
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
            ViewBag.CloseOpportunities = rights.CanCloseOpportunities;
            ViewBag.ReopenOpportunities = rights.CanReopenOpportunities;

            return View("~/Views/CRMS/EnquiriesHiererchy/Dashboard.cshtml", enqdet);

        }


        public PartialViewResult PartialEnquiriesGrid(EnquiriesDet modelenquiriesdet, string isshowclicked)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Dashboard", "EnquiriesHiererchy");

            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.SupervisorFeedback = rights.SupervisorFeedback;
            ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
            ViewBag.Verified = rights.Verified;
            ViewBag.CloseOpportunities = rights.CanCloseOpportunities;
            ViewBag.ReopenOpportunities = rights.CanReopenOpportunities;

            if (modelenquiriesdet.is_pageload != 0)
            {
                string datfrmat = modelenquiriesdet.FromDate.Split('-')[2] + '-' + modelenquiriesdet.FromDate.Split('-')[1] + '-' + modelenquiriesdet.FromDate.Split('-')[0];
                string dattoat = modelenquiriesdet.ToDate.Split('-')[2] + '-' + modelenquiriesdet.ToDate.Split('-')[1] + '-' + modelenquiriesdet.ToDate.Split('-')[0];
                //_enquiries = new EnquiriesRepo();
                //_enquiries.GetListingHiererchy(modelenquiriesdet.EnquiriesFrom, datfrmat, dattoat);

                string action = string.Empty;
                DataTable formula_dtls = new DataTable();
                DataSet dsInst = new DataSet();

                try
                {
                    int user_id = Convert.ToInt32(Session["userid"]);
                    string cnt_internalId = Convert.ToString(Session["owninternalid"]);

                    ExecProcedure execProc = new ExecProcedure();
                    List<KeyObj> paramList = new List<KeyObj>();
                    execProc.ProcedureName = "PRC_ENQUIRIES_LISTING_HIERERCHYWISE";
                    paramList.Add(new KeyObj("@USERID", user_id));
                    paramList.Add(new KeyObj("@cnt_internalId", cnt_internalId));
                    paramList.Add(new KeyObj("@ENQUIRIESFROM", modelenquiriesdet.EnquiriesFrom));
                    paramList.Add(new KeyObj("@FROMDATE", datfrmat));
                    paramList.Add(new KeyObj("@TODATE", dattoat));

                    execProc.param = paramList;
                    execProc.ExecuteProcedureNonQuery();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            //if (isshowclicked == "1")
            //{
            TempData["SearchKey"] = modelenquiriesdet;


            //}
            TempData["isshowclicked"] = isshowclicked;
            TempData.Keep();
            return PartialView("~/Views/CRMS/EnquiriesHiererchy/PartialEnquiriesGrid.cshtml", GetFormulaList(modelenquiriesdet.is_pageload));
        }

        //Rev Subhra 12-04-2019
        public PartialViewResult PartialMass()
        {
            return PartialView("~/Views/CRMS/EnquiriesHiererchy/_EnquriesMass.cshtml");
        }
        public PartialViewResult PartialMassDelete()
        {
            return PartialView("~/Views/CRMS/EnquiriesHiererchy/_EnquiriesMassDelete.cshtml", GetMassDeleteListing(1));
        }
        public IEnumerable GetMassDeleteListing(int is_pageload)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);

            if (is_pageload != 0)
            {
                CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                var q = from d in DC.ENQUIRIES_LISTING_HIERERCHYWISEs
                        where d.USERID == Convert.ToInt32(Userid) && d.SUPERVISOR == false && d.SALESMAN == false && d.VERIFY == false
                        orderby d.SEQ
                        select d;
                return q;

            }
            else
            {
                //CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                //var q = from d in DC.ENQUIRIES_LISTING_HIERERCHYWISEs
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

            return PartialView("~/Views/CRMS/EnquiriesHiererchy/_EnquriesBulk.cshtml", enquiriesDet);
        }
        public PartialViewResult PartialBulkAssign()
        {
            return PartialView("~/Views/CRMS/EnquiriesHiererchy/_EnquiriesBulkAssign.cshtml", GetBulkAssignListing(1));
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
                if (AssignIndustryName == "" || AssignIndustryName == "All")
                {
                    CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                    var q = from d in DC.ENQUIRIES_LISTING_HIERERCHYWISEs
                            // where d.USERID == Convert.ToInt32(Userid) && d.SUPERVISOR == false && d.SALESMAN == false && d.VERIFY == false && d.ASSIGNEDSALESMAN==""
                            where d.USERID == Convert.ToInt32(Userid) && d.ASSIGNEDSALESMAN == ""
                            orderby d.SEQ
                            select d;
                    return q;
                }
                else
                {
                    CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                    var q = from d in DC.ENQUIRIES_LISTING_HIERERCHYWISEs
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
                //var q = from d in DC.ENQUIRIES_LISTING_HIERERCHYWISEs
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

        public PartialViewResult PartialPriority()
        {
            EnquiriesDet enquiriesDet = new EnquiriesDet();

            _enquiries = new EnquiriesRepo();

            DataSet _getAssIndustry = new DataSet();
            List<AssIndustry> AssIndustrylist = new List<AssIndustry>();
            AssIndustry slind = null;

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

            return PartialView("~/Views/CRMS/EnquiriesHiererchy/_EnquriesPriority.cshtml", enquiriesDet);
        }

        public PartialViewResult PartialSetPriority()
        {
            return PartialView("~/Views/CRMS/EnquiriesHiererchy/_EnquriesSetPriority.cshtml", GetSetPriorityListing(1));
        }
        public IEnumerable GetSetPriorityListing(int is_pageload)
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
                if (AssignIndustryName == "" || AssignIndustryName == "All")
                {
                    CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                    var q = from d in DC.ENQUIRIES_LISTING_HIERERCHYWISEs
                            // where d.USERID == Convert.ToInt32(Userid) && d.SUPERVISOR == false && d.SALESMAN == false && d.VERIFY == false && d.ASSIGNEDSALESMAN==""
                            where d.USERID == Convert.ToInt32(Userid) && d.ASSIGNEDSALESMAN != "" && d.SETPRIORITY == ""
                            orderby d.SEQ
                            select d;
                    return q;
                }
                else
                {
                    CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                    var q = from d in DC.ENQUIRIES_LISTING_HIERERCHYWISEs
                            // where d.USERID == Convert.ToInt32(Userid) && d.SUPERVISOR == false && d.SALESMAN == false && d.VERIFY == false && d.ASSIGNEDSALESMAN==""
                            where d.USERID == Convert.ToInt32(Userid) && d.ASSIGNEDSALESMAN != "" && d.SETPRIORITY == "" && d.INDUSTRY == AssignIndustryName
                            orderby d.SEQ
                            select d;
                    return q;
                }


            }
            else
            {
                //CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                //var q = from d in DC.ENQUIRIES_LISTING_HIERERCHYWISEs
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
                var q = from d in DC.ENQUIRIES_LISTING_HIERERCHYWISEs
                        where d.USERID == Convert.ToInt32(Userid)
                        orderby d.SEQ
                        select d;
                return q;

            }
            else
            {
                //CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                //var q = from d in DC.ENQUIRIES_LISTING_HIERERCHYWISEs
                //        where d.USERID == Convert.ToInt32(Userid) && d.SEQ == 11111111119
                //        orderby d.SEQ descending
                //        select d;
                //return q;
                return null;
            }
        }

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
                ViewBag.title = "Target Opportunities";
            }
            else if (ActionType == "EDIT" && _uniqueid != "")
            {
                _apply = _enquiries.getEnquiryById(_uniqueid, ActionType);
                // ViewBag.dtls = _apply.dtls;
                ViewBag.uniqueid = _uniqueid;
                ViewBag.Action = "EDIT";
                _apply.HeaderName = "Modify Enquiries";
                ViewBag.title = "Target Opportunities";

            }
            //EnquiriesDet enquiriesmodel = new EnquiriesDet();

            return View("~/Views/CRMS/EnquiriesHiererchy/Index.cshtml", _apply);

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

            return PartialView("/Views/CRMS/EnquiriesHiererchy/_EnquirisSupervisor.cshtml", enquiriesSupervisorFeedback);
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
            return PartialView("/Views/CRMS/EnquiriesHiererchy/_EnquirisSalesman.cshtml", enquiriesSalesmanFeedback);
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
            return PartialView("~/Views/CRMS/EnquiriesHiererchy/_EnquirisShowHistorySalesman.cshtml", objList);
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


            return PartialView("/Views/CRMS/EnquiriesHiererchy/_EnquirisVerify.cshtml", enquiriesVerify);
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

            // End of Mantis Issue 24748
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

            return PartialView("/Views/CRMS/EnquiriesHiererchy/_EnquirisAssignedSalesman.cshtml", enquiriesDet);
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

        public ActionResult CloseEnquiry(string Unique_ID)
        {
            //ViewBag.selectedEnquirisList = Uniqueid;
            EnquiriesDet enquiriesDet = new EnquiriesDet();

            enquiriesDet.selectedEnquirisList = Unique_ID;

            _enquiries = new EnquiriesRepo();

            // enquiriesDet.listAssignedSalesman = _enquiries.getAssignedSalesman();

            DataSet _getCloseReason = new DataSet();
            List<CloseReason> CloseReasonlist = new List<CloseReason>();
            CloseReason clReas = null;
            // EnquiriesDet _apply = new EnquiriesDet();
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "PRC_CRUD_ENQUIRIES";
                paramList.Add(new KeyObj("@ACTION_TYPE", "GetCloseReason"));
                execProc.param = paramList;
                _getCloseReason = execProc.ExecuteProcedureGetDataSet();
                foreach (DataRow dr in _getCloseReason.Tables[0].Rows)
                {
                    clReas = new CloseReason();
                    clReas.ID = Convert.ToInt32(dr[0]);
                    clReas.Close_Reason = dr[1].ToString();
                    CloseReasonlist.Add(clReas);
                }

            }
            catch (Exception ex)
            {

            }
            enquiriesDet.listCloseReason = CloseReasonlist;

            return PartialView("/Views/CRMS/EnquiriesHiererchy/_EnquirisCloseReason.cshtml", enquiriesDet);
        }

        public JsonResult SaveCloseReason(EnquiriesDet CloseReason)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _enquiries = new EnquiriesRepo();
            try
            {
                
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
                        paramList.Add(new KeyObj("@ACTION_TYPE", "SAVECLOSEREASON"));
                        paramList.Add(new KeyObj("@ENQ_CRM_ID", CloseReason.selectedEnquirisList));
                        paramList.Add(new KeyObj("@CLOSEORDER", CloseReason.CloseOrder));
                        paramList.Add(new KeyObj("@CLOSEREASONID", CloseReason.CloseReasonId));
                        paramList.Add(new KeyObj("@CLOSEQTY", CloseReason.CloseQty));
                        paramList.Add(new KeyObj("@CLOSEREASONREMARKS", CloseReason.CloseReasonRemarks));
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
                    CloseReason.response_code = "Success";
                    CloseReason.response_msg = "Success";
                }
                else if (ReturnMsg != "Success" && ReturnCode == -1)
                {
                    CloseReason.response_code = "Error";
                    CloseReason.response_msg = ReturnMsg;
                }
                else
                {
                    CloseReason.response_code = "Error";
                    CloseReason.response_msg = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                CloseReason.response_code = "CatchError";
                CloseReason.response_msg = "Please try again later";
            }

            return Json(CloseReason, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReopenEnquiry(string Unique_ID)
        {
            //ViewBag.selectedEnquirisList = Uniqueid;
            EnquiriesDet enquiriesDet = new EnquiriesDet();

            enquiriesDet.selectedEnquirisList = Unique_ID;

            return PartialView("/Views/CRMS/EnquiriesHiererchy/_EnquirisReopen.cshtml", enquiriesDet);
        }

        public JsonResult SaveReopenEnquiry(EnquiriesDet ReopenEnquiry)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _enquiries = new EnquiriesRepo();
            try
            {

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
                        paramList.Add(new KeyObj("@ACTION_TYPE", "SAVEREOPENENQUIRY"));
                        paramList.Add(new KeyObj("@ENQ_CRM_ID", ReopenEnquiry.selectedEnquirisList));
                        paramList.Add(new KeyObj("@REOPENFEEDBACK", ReopenEnquiry.ReopenFeedback));
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
                    ReopenEnquiry.response_code = "Success";
                    ReopenEnquiry.response_msg = "Success";
                }
                else if (ReturnMsg != "Success" && ReturnCode == -1)
                {
                    ReopenEnquiry.response_code = "Error";
                    ReopenEnquiry.response_msg = ReturnMsg;
                }
                else
                {
                    ReopenEnquiry.response_code = "Error";
                    ReopenEnquiry.response_msg = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                ReopenEnquiry.response_code = "CatchError";
                ReopenEnquiry.response_msg = "Please try again later";
            }

            return Json(ReopenEnquiry, JsonRequestBehavior.AllowGet);
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
    }
    
}