using CRM.Models;
using CRM.Models.DataContext;
using CRM.Repostiory.Opportunity;
using DataAccessLayer;
using EntityLayer.CommonELS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace CRM.Controllers.CRMS
{
    public class CRMOpportunitiesController : Controller
    {
        UserRightsForPage rights = new UserRightsForPage();
        private IOpportunity _opportunity;

        public ActionResult Index()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
            crmOpportunitiesModel CRMOpportunitiesobj = new crmOpportunitiesModel();

            List<crm_CampaignType> crm_CampaignTypelist = new List<Models.DataContext.crm_CampaignType>();
            crm_CampaignTypelist.Add(new crm_CampaignType { Id = 0, Campaign_Code = "Select" });
            var querycrm_CampaignTypes = (from u in crm_CampaignTypelist
                                          select u).Union(from c in dcon.crm_CampaignTypes
                                                          select c);
            CRMOpportunitiesobj.Campaign_Type = querycrm_CampaignTypes.ToList();
            List<crm_StatusDetail> lstcrm_StatusDetail = new List<crm_StatusDetail>();
            lstcrm_StatusDetail.Add(new crm_StatusDetail { Id = 0, Status_Code = "Select" });
            var querycrm_StatusDetails = (from u in lstcrm_StatusDetail
                                          select u).Union(from c in dcon.crm_StatusDetails
                                                          select c);

            var querycrm_users = (from c in dcon.V_UserLIsts
                                  select c);
            List<v_ContactSource> lstv_ContactSource = new List<v_ContactSource>();
            lstv_ContactSource.Add(new v_ContactSource { SourceID = 0, cntsrc_sourceType = "Select" });
            var querycrm_contactsource = (from u in lstv_ContactSource
                                          select u).Union(from c in dcon.v_ContactSources
                                                          select c);
            List<V_Rating> lstV_Rating = new List<V_Rating>();
            lstV_Rating.Add(new V_Rating { rat_id = 0, rat_LeadRating = "Select" });
            var querycrm_rating = (from u in lstV_Rating
                                   select u).Union(from c in dcon.V_Ratings
                                                   select c);

            List<v_PurchaseTimeframe> lstV_PurchaseTimeframe = new List<v_PurchaseTimeframe>();
            lstV_PurchaseTimeframe.Add(new v_PurchaseTimeframe { TIMEFRAME_ID = 0, TIMEFRAME_NAME = "Select" });
            var querycrm_PurchaseTimeframe = (from u in lstV_PurchaseTimeframe
                                              select u).Union(from c in dcon.v_PurchaseTimeframes
                                                              select c);

            List<v_PurchaseProcess> lstV_PurchaseProcess = new List<v_PurchaseProcess>();
            lstV_PurchaseProcess.Add(new v_PurchaseProcess { ProcessID = 0, Process_NAME = "Select" });
            var querycrm_PurchaseProcess = (from u in lstV_PurchaseProcess
                                            select u).Union(from c in dcon.v_PurchaseProcesses
                                                            select c);

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "CRMOpportunities");

            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanExport = rights.CanExport;

            CRMOpportunitiesobj.ContactSource = querycrm_contactsource.ToList();
            CRMOpportunitiesobj.Users = querycrm_users.ToList();
            CRMOpportunitiesobj.Status_Details = querycrm_StatusDetails.ToList();
            CRMOpportunitiesobj.WonerID = Convert.ToString(Session["userid"]);
            CRMOpportunitiesobj.AssignedID = Convert.ToString(Session["userid"]);
            CRMOpportunitiesobj.Rating_List = querycrm_rating.ToList();
            CRMOpportunitiesobj.Timeframe_List = querycrm_PurchaseTimeframe.ToList();
            CRMOpportunitiesobj.PurchaseProcess_List = querycrm_PurchaseProcess.ToList();
            CRMOpportunitiesobj.Address1 = "";
            CRMOpportunitiesobj.Address2 = "";
            CRMOpportunitiesobj.Address3 = "";
            CRMOpportunitiesobj.LandMark = "";
            CRMOpportunitiesobj.city = "";

            CRMOpportunitiesobj.state = "";
            CRMOpportunitiesobj.Website = "";
            CRMOpportunitiesobj.country = "";
            CRMOpportunitiesobj.Pin = "";
            //CRMOpportunitiesobj.custdetails = new CustomerAddress();
            CRMOpportunitiesobj.Woner = Convert.ToString(System.Web.HttpContext.Current.Session["UserName"]);
            TempData["QoutDetails"] = null;
            return View(@"~/Views/CRMS/Opportunities/Opportunities.cshtml", CRMOpportunitiesobj);
        }

        public ActionResult GetAccount(string SearchKey)
        {
            List<AccountModel> listCust = new List<AccountModel>();
            if (Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = new DataTable();

                cust = oDBEngine.GetDataTable(" select distinct top 10  cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing from v_pos_AccountDetailsForCRM where Name like '%" + SearchKey + "%' ");
                listCust = (from DataRow dr in cust.Rows
                            select new AccountModel()
                            {
                                id = dr["cnt_internalid"].ToString(),
                                Na = dr["Name"].ToString(),
                                UId = Convert.ToString(dr["uniquename"]),
                                add = Convert.ToString(dr["Billing"])
                            }).ToList();
            }

            return Json(listCust);
        }

        public ActionResult GetContact(string SearchKey)
        {
            List<ContactModel> listCust = new List<ContactModel>();
            if (Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = new DataTable();

                cust = oDBEngine.GetDataTable(" select distinct top 10  cnt_internalid  ,Replace(Name,'''','&#39;') as Name ,Billing from v_ContactDetailsForCRM where Name like '%" + SearchKey + "%'");
                listCust = (from DataRow dr in cust.Rows
                            select new ContactModel()
                            {
                                id = dr["cnt_internalid"].ToString(),
                                Na = dr["Name"].ToString(),
                                add = Convert.ToString(dr["Billing"])
                            }).ToList();
            }

            return Json(listCust);
        }

        public JsonResult AddressByPin(string PinCode)
        {

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
            crmLeads crmLeadsobj = new crmLeads();

            List<CustomerDetails> addByPin = new List<CustomerDetails>();
            if (Convert.ToInt64(Session["userid"]) != null)
            {

                ProcedureExecute pro = new ProcedureExecute("Prc_ServiceBillingShipping");
                pro.AddVarcharPara("@Action", 100, "CustomAddressByPin");
                pro.AddVarcharPara("@pin_code", 10, PinCode);
                DataTable addtab = pro.GetTable();

                addByPin = (from DataRow dr in addtab.Rows
                            select new CustomerDetails
                            {
                                PinCode = Convert.ToString(dr["PinCode"]),
                                PinId = Convert.ToInt32(dr["PinId"]),
                                CountryId = Convert.ToInt32(dr["CountryId"]),
                                CountryName = Convert.ToString(dr["CountryName"]),
                                StateId = Convert.ToInt32(dr["StateId"]),
                                StateName = Convert.ToString(dr["StateName"]),
                                StateCode = Convert.ToString(dr["StateCode"]),
                                CityId = Convert.ToInt32(dr["CityId"]),
                                CityName = Convert.ToString(dr["CityName"])
                            }).ToList();
            }

            //return View(@"~/Views/CRMS/Leads/CRMLeads.cshtml", crmLeadsobj);
            return Json(addByPin);
        }

        public ActionResult GetCompany(string SearchKey, string contactType)
        {
            List<CompanyModel> listCust = new List<CompanyModel>();
            if (Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cust = new DataTable();

                cust = oDBEngine.GetDataTable(" select distinct top 10  cnt_internalid ,uniquename ,Replace(Name,'''','&#39;') as Name ,Billing from v_pos_customerDetailsForPMS where Name like '%" + SearchKey + "%' AND Type='" + contactType + "'");
                listCust = (from DataRow dr in cust.Rows
                            select new CompanyModel()
                            {
                                id = dr["cnt_internalid"].ToString(),
                                Na = dr["Name"].ToString(),
                                UId = Convert.ToString(dr["uniquename"]),
                                add = Convert.ToString(dr["Billing"])
                            }).ToList();
            }

            return Json(listCust);
        }

        public JsonResult AddressByCompany(string cntID)
        {

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
            crmLeads crmLeadsobj = new crmLeads();

            List<CustomerAddress> addByPin = new List<CustomerAddress>();
            if (Convert.ToInt64(Session["userid"]) != null)
            {
                DataTable addtab = null;
                ProcedureExecute pro = new ProcedureExecute("PRC_AddressDetailsOpportunity");
                pro.AddVarcharPara("@cntID", 10, cntID);
                addtab = pro.GetTable();

                if (addtab != null && addtab.Rows.Count > 0)
                {
                    if (addtab.Rows[0]["pinID"] != DBNull.Value)
                    addByPin = (from DataRow dr in addtab.Rows
                                select new CustomerAddress
                                {
                                    WebSite = Convert.ToString(dr["add_Website"]),
                                    Address1 = Convert.ToString(dr["address1"]),
                                    Address2 = Convert.ToString(dr["address2"]),
                                    // Rev Mantis issue 20684 (27/05/2021)
                                    Address3 = Convert.ToString(dr["address3"]),
                                    // End of Rev Mantis issue 20684 (27/05/2021)
                                    Landmark = Convert.ToString(dr["add_landMark"]),
                                    PinCode = Convert.ToString(dr["pin_code"]),
                                    CountryName = Convert.ToString(dr["cou_country"]),
                                    StateName = Convert.ToString(dr["state"]),
                                    CityName = Convert.ToString(dr["city_name"]),
                                    PinId = Convert.ToInt32(dr["pinID"]),
                                    CityId = Convert.ToInt32(dr["CityID"]),
                                    StateId = Convert.ToInt32(dr["StateID"]),
                                    CountryId = Convert.ToInt32(dr["CountryID"])
                                }).ToList();
                }

            }

            //return View(@"~/Views/CRMS/Leads/CRMLeads.cshtml", crmLeadsobj);
            return Json(addByPin);
        }

        public PartialViewResult PartialQuotationGrid(string AccountNo)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Dashboard", "Enquiries");

            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.SupervisorFeedback = rights.SupervisorFeedback;
            ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
            ViewBag.Verified = rights.Verified;


            ViewBag.CanCreateActivity = rights.CanCreateActivity;
            ViewBag.CanProducts = rights.CanProducts;
            ViewBag.CanSharing = rights.CanSharing;
            ViewBag.CanLiterature = rights.CanLiterature;


            _opportunity = new OpportunityRepo();
            List<QuotationDetailsList> OpportunityVerify = new List<QuotationDetailsList>();
            if (TempData["QuotList"] != null)
            {
                OpportunityVerify = TempData["QuotList"] as List<QuotationDetailsList>;
            }
            else
            {
                OpportunityVerify = _opportunity.getVerifyId(AccountNo);
                TempData["QuotList"] = OpportunityVerify;
            }
            TempData.Keep();
            return PartialView("/Views/CRMS/Opportunities/PartialQuotationGrid.cshtml", OpportunityVerify);
        }

        public JsonResult PartialQuotationBind(string AccountNo)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Dashboard", "Enquiries");

            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.SupervisorFeedback = rights.SupervisorFeedback;
            ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
            ViewBag.Verified = rights.Verified;

            _opportunity = new OpportunityRepo();
            List<QuotationDetailsList> OpportunityVerify = new List<QuotationDetailsList>();
            OpportunityVerify = _opportunity.getVerifyId(AccountNo);

            TempData["QuotList"] = OpportunityVerify;
            TempData.Keep();
            // return PartialView("/Views/CRMS/Opportunities/PartialQuotationGrid.cshtml", OpportunityVerify);
            return Json("Sucess");
        }

        public PartialViewResult PartialQuotationListGrid()
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "CRMOpportunities");

            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.SupervisorFeedback = rights.SupervisorFeedback;
            ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
            ViewBag.Verified = rights.Verified;

            List<QuotationDetails> OpportunityVerify = new List<QuotationDetails>();
            if (TempData["QoutDetails"] != null)
            {
                OpportunityVerify = TempData["QoutDetails"] as List<QuotationDetails>;
            }
            TempData.Keep();
            return PartialView("/Views/CRMS/Opportunities/PartialQuotationListGrid.cshtml", OpportunityVerify);
        }

        public JsonResult PartialQuotationDetails(String Quot_ID, string QuoteNumber, string QuoteDate, string QuoteTotalAmount, string QuoteExpiry, string Created_Date)
        {
            DataTable dt = TempData["QoutDetailsTemp"] as DataTable;

            if (dt == null)
            {
                DataTable dtable = new DataTable();

                dtable.Clear();
                dtable.Columns.Add("Quote_Id", typeof(System.String));
                dtable.Columns.Add("Quote_Number", typeof(System.String));
                dtable.Columns.Add("Quote_Date", typeof(System.DateTime));
                dtable.Columns.Add("Quote_TotalAmount", typeof(System.Decimal));
                dtable.Columns.Add("Quote_Expiry", typeof(System.DateTime));
                dtable.Columns.Add("CreatedDate", typeof(System.DateTime));
                dtable.Columns.Add("Customer_Id", typeof(System.String));
                object[] trow = { Quot_ID, QuoteNumber, Convert.ToDateTime(QuoteDate), Convert.ToDecimal(QuoteTotalAmount), Convert.ToDateTime(QuoteExpiry), Convert.ToDateTime(Created_Date), "" };// Add new parameter Here
                dtable.Rows.Add(trow);
                TempData["QoutDetailsTemp"] = dtable;
            }
            else
            {
                object[] trow = { Quot_ID, QuoteNumber, Convert.ToDateTime(QuoteDate), Convert.ToDecimal(QuoteTotalAmount), Convert.ToDateTime(QuoteExpiry), Convert.ToDateTime(Created_Date), "" };// Add new parameter Here
                dt.Rows.Add(trow);
                TempData["QoutDetailsTemp"] = dt;
            }
            QuotationDetails _applyverify = new QuotationDetails();
            List<QuotationDetails> _applyverifyList = new List<QuotationDetails>();
            if (TempData["QoutDetailsTemp"] != null)
            {
                DataTable dtes = TempData["QoutDetailsTemp"] as DataTable;
                foreach (DataRow dr in dtes.Rows)
                {
                    _applyverify = new QuotationDetails();
                    _applyverify.Quote_Number = dr["Quote_Number"].ToString();
                    _applyverify.Customer_Id = dr["Customer_Id"].ToString();
                    _applyverify.Quote_Date = Convert.ToDateTime(dr["Quote_Date"]);
                    _applyverify.Quote_Expiry = Convert.ToDateTime(dr["Quote_Expiry"]);
                    _applyverify.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    _applyverify.Quote_TotalAmount = Convert.ToDecimal(dr["Quote_TotalAmount"]);
                    _applyverify.Quote_Id = dr["Quote_Id"].ToString();
                    _applyverifyList.Add(_applyverify);
                }
            }
            TempData["QoutDetails"] = _applyverifyList;

            // TempData["QuotList"] = OpportunityVerify;
            TempData.Keep();
            // return PartialView("/Views/CRMS/Opportunities/PartialQuotationGrid.cshtml", OpportunityVerify);
            return Json("Sucess");
        }

        public ActionResult SaveCRMOpportunities(crmOpportunitiesModel newcrmOpportunitiesobj)
        {
            crmOpportunitiesModel CRMOpportunitiesobj = new crmOpportunitiesModel();
            string output = "";
            output = CRMOpportunitiesobj.SaveOpportunities(newcrmOpportunitiesobj);
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PopulateGrid(string frmdate)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "CRMOpportunities");

            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanConvertTo = rights.CanConvertTo;
            ViewBag.CanCreateActivity = rights.CanSalesActivity;

            ViewBag.CanQualify = rights.CanQualify;
            ViewBag.CanCancelLost = rights.CanCancelLost;
            ViewBag.CanProducts = rights.CanProducts;
            ViewBag.CanSharing = rights.CanSharing;
            ViewBag.CanLiterature = rights.CanLiterature;
            return PartialView(@"~/Views/CRMS/Opportunities/PartialOpportunityList.cshtml", GetSalesSummary(frmdate));
        }
        public IEnumerable GetSalesSummary(string frmdate)
        {

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);

            if (frmdate != "Ispageload")
            {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                // Rev Mantis issue 24057
                //var q = from d in dc.v_crmOpportunities
                //        where Convert.ToString(d.Assign_To) == Userid || Convert.ToString(d.Owner_Id) == Userid
                //        orderby d.ID descending
                //        select d;
                //return q;

                BusinessLogicLayer.CommonBL ComBL = new BusinessLogicLayer.CommonBL();
                string ShowOpportunitiesDataForAllUsers = ComBL.GetSystemSettingsResult("ShowOpportunitiesDataForAllUsers");

                if (ShowOpportunitiesDataForAllUsers.ToUpper() == "NO")
                {
                    var q = from d in dc.v_crmOpportunities
                            where Convert.ToString(d.Assign_To) == Userid || Convert.ToString(d.Owner_Id) == Userid
                            orderby d.ID descending
                            select d;
                    return q;
                }
                else
                {
                    var q = from d in dc.v_crmOpportunities
                            orderby d.ID descending
                            select d;
                    return q;
                }
                // End of Rev Mantis issue 24057  
            }
            else
            {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                var q = from d in dc.v_crmOpportunities
                        where 1 == 0
                        select d;
                return q;
            }



        }


        public ActionResult EditOpportunities(crmOpportunitiesModel OBJ)
        {
            crmOpportunitiesModel crmOpportunitiesModel = new crmOpportunitiesModel();
            DataSet output = crmOpportunitiesModel.EditOpportunities(OBJ);
            crmOpportunitiesModel = APIHelperMethods.ToModel<crmOpportunitiesModel>(output.Tables[0]);

            crmOpportunitiesModel.cntids = output.Tables[1].AsEnumerable()
                           .Select(r => r.Field<string>("CONTACT_ID"))
                           .ToList();

            return Json(crmOpportunitiesModel);
        }

        public ActionResult DeleteCRMOpportunities(crmOpportunitiesModel OBJ)
        {
            crmOpportunitiesModel crmOpportunitiesModel = new crmOpportunitiesModel();
            string output = "";
            output = crmOpportunitiesModel.Delete(OBJ);
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PopulateContactGrid()
        {
            return PartialView(@"~/Views/CRMS/Leads/_PartialConTact.cshtml", GetContactData());
        }

        public IEnumerable GetContactData()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dc = new CRMClassDataContext(connectionString);
            var q = from d in dc.v_crmContacts
                    orderby d.FirstName
                    select d;
            return q;
        }
    }
}