using CRM.Models;
using CRM.Models.DataContext;
using DataAccessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;
using CRM.DAL;
using DevExpress.Web.Mvc;
using DevExpress.Web;
using EntityLayer.CommonELS;

namespace CRM.Controllers.CRMS
{
    public class CRMAccountController : Controller
    {
            UserRightsForPage rights = new UserRightsForPage();
        
        //
        // GET: /CRMAccount/
        public ActionResult Index()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
            crmAccount crmAccountobj = new crmAccount();

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "CRMAccount");

            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanExport = rights.CanExport;

           


            var querycrm_users = from c in dcon.V_UserLIsts
                                 select c;
            crmAccountobj.Users = querycrm_users.ToList();

            var querycrm_StatusDetails = from c in dcon.v_StatusDetails
                                         where c.Status_Code != "Qualify"
                                         select c;
            crmAccountobj.Status_Details = querycrm_StatusDetails.ToList();

            var querycrm_IndustryDetails = from c in dcon.V_Industries
                                           select c;
            crmAccountobj.Industries = querycrm_IndustryDetails.ToList();

            var querycrm_RelationshipType = from c in dcon.v_RelationshipTypes
                                            select c;
            crmAccountobj.listRelationshipType = querycrm_RelationshipType.ToList();


            var querycrm_Ownership = from c in dcon.v_Ownerships
                                            select c;
            crmAccountobj.listOwnership = querycrm_Ownership.ToList();


            var querycrm_PaymentTerms = from c in dcon.v_PaymentTerms
                                     select c;
            crmAccountobj.listPaymentTerm = querycrm_PaymentTerms.ToList();


            var querycrm_ContactMethod = from c in dcon.v_ContactMethods
                                        select c;
            crmAccountobj.listContactMethod = querycrm_ContactMethod.ToList();

            var querycrm_Email = from c in dcon.v_Emails
                                         select c;
            crmAccountobj.listEmail = querycrm_Email.ToList();


            var querycrm_Phone = from c in dcon.v_Phones
                                 select c;
            crmAccountobj.listPhone = querycrm_Phone.ToList();

            var querycrm_Fax = from c in dcon.v_Faxes
                                 select c;
            crmAccountobj.listFax = querycrm_Fax.ToList();

            var querycrm_ShippingMethod = from c in dcon.v_ShippingMethods
                               select c;
            crmAccountobj.listShippingMethod = querycrm_ShippingMethod.ToList();

            var querycrm_Freight = from c in dcon.v_Freights
                                          select c;
            crmAccountobj.listFreight = querycrm_Freight.ToList();

            var queryContList = from c in dcon.v_crmContactSelects
                                   select c;
            crmAccountobj.contactList = queryContList.ToList();

            var queryoppList = from c in dcon.v_crmOppSelects
                                   select c;
            crmAccountobj.oppList = queryoppList.ToList();





            crmAccountobj.OwnerID = Convert.ToInt64(Session["userid"]);
            crmAccountobj.AssignedID = Convert.ToInt64(Session["userid"]);

            crmAccountobj.acccustdetails = new AccountCustomerDetails();

            return View(@"~/Views/CRMS/Account/Account.cshtml", crmAccountobj);
        }
        public ActionResult GetcrmParentAccount(string SearchKey)
        {
            List<crmParentAccountModel> listCust = new List<crmParentAccountModel>();
            if (Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");


                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                //DataTable cust = oDBEngine.GetDataTable("select top 10  sProductsID as Products_ID,Products_Name ,Products_Description ,HSNSAC  from v_Product_MargeDetailsPOS where Products_Name like '%" + SearchKey + "%'  or Products_Description  like '%" + SearchKey + "%' order by Products_Name,Products_Description");
                DataTable cust = oDBEngine.GetDataTable("select cnt_internalId as Id,cnt_UCC as Code,cnt_firstName as Name  from tbl_master_contact where CONVERT_TYPE='AC' and cnt_firstName like '%" + SearchKey + "%' order by cnt_firstName");



                listCust = (from DataRow dr in cust.Rows
                            select new crmParentAccountModel()
                            {
                                id = dr["Id"].ToString(),
                                Na = dr["Name"].ToString(),
                               
                              
                            }).ToList();
            }

            return Json(listCust);
        }
        public JsonResult checkValidation(ValidCondition vcondition)
         {
            Validation validation = new Validation();


            List<ValidCondition> listvalidation = new List<ValidCondition>();
            listvalidation.Add(vcondition);

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);
            int isexists = validation.CheckExistancy("tbl_master_contact", "cnt_shortName", listvalidation);


            return Json(isexists);
        }
        [ValidateInput(false)]
        public ActionResult SaveCRMAccount(crmAccount newcrmAccountsobj)
        {
            
            crmAccount CRMAccountsobj = new crmAccount();
            string output = "";
            output = CRMAccountsobj.SaveAccount(newcrmAccountsobj);
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PopulateLeadGrid(string frmdate)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "CRMAccount");

            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;


            ViewBag.CanCreateActivity = rights.CanSalesActivity;
            ViewBag.CanProducts = rights.CanProducts;
            ViewBag.CanSharing = rights.CanSharing;
            ViewBag.CanLiterature = rights.CanLiterature;

            return PartialView(@"~/Views/CRMS/Account/PartialAccountGridView.cshtml", GetAccounts(frmdate));
        }
        public IEnumerable GetAccounts(string frmdate)
        {

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);

            if (frmdate != "Ispageload")
            {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                // Mantis Issue 24055
                //var q = from d in dc.V_CRMAccounts
                //        where Convert.ToString(d.Assign_ToID) == Userid || Convert.ToString(d.Owner_Id) == Userid
                //        orderby d.ID descending
                //        select d;
                //return q;

                BusinessLogicLayer.CommonBL ComBL = new BusinessLogicLayer.CommonBL();
                string ShowAccountsDataForAllUsers = ComBL.GetSystemSettingsResult("ShowAccountsDataForAllUsers");

                if (ShowAccountsDataForAllUsers.ToUpper() == "NO")
                {
                    var q = from d in dc.V_CRMAccounts
                            where Convert.ToString(d.Assign_ToID) == Userid || Convert.ToString(d.Owner_Id) == Userid
                            orderby d.ID descending
                            select d;
                    return q;
                }
                else
                {
                    var q = from d in dc.V_CRMAccounts
                            orderby d.ID descending
                            select d;
                    return q;
                }
                // End of Rev Mantis issue 24055  
            }
            else
            {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                var q = from d in dc.V_CRMAccounts
                        where 1==0
                        orderby d.ID descending
                        select d;
                return q;
            }



        }

        public ActionResult EditCRMAccounts(crmAccount newcrmAccountsobj)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);

            crmAccount CRMAccountsobj = new crmAccount();
            DataSet output = CRMAccountsobj.EditCRMeAccount(newcrmAccountsobj);
            AccountCustomerDetails clsObj = new AccountCustomerDetails();
            CRMAccountsobj = APIHelperMethods.ToModel<crmAccount>(output.Tables[0]);
            clsObj = APIHelperMethods.ToModel<AccountCustomerDetails>(output.Tables[1]);
            CRMAccountsobj.cntids = output.Tables[2].AsEnumerable()
                           .Select(r => r.Field<string>("CONTACT_ID"))
                           .ToList(); ;
            CRMAccountsobj.acccustdetails = clsObj;

            CRMAccountsobj.oppids = output.Tables[3].AsEnumerable()
               .Select(r => r.Field<string>("OPPORTUNITY_ID"))
               .ToList(); ;
            CRMAccountsobj.acccustdetails = clsObj;

            //var querycrm_StatusDetails = from c in dcon.v_StatusDetails
            //                             where c.Status_Code != "Close" 
            //                             select c;
            //CRMLeadsobj.Status_Details = querycrm_StatusDetails.ToList();
            //var querycrm_StatusDetails = from c in dcon.v_StatusDetails
            //                             where c.Status_Code == "New" || c.Status_Code == "Cancelled/Lost" || c.Status_Code == "Qualify"
            //                             select c;
            //CRMLeadsobj.Status_Details = querycrm_StatusDetails.ToList();




            return Json(CRMAccountsobj);
        }

        public ActionResult DeleteCRMAccounts(crmAccount newcrmAccountsobj)
        {
            crmAccount CRMAccountobj = new crmAccount();
            string output = "";
            output = CRMAccountobj.DeleteAccounts(newcrmAccountsobj);
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportCRMAccountsList(int type)
        {


            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToPdf(GetEmployeeBatchGridViewSettings(), GetAccounts("1"));
                //break;
                case 2:
                    return GridViewExtension.ExportToXlsx(GetEmployeeBatchGridViewSettings(), GetAccounts("1"));
                //break;
                case 3:
                    return GridViewExtension.ExportToXls(GetEmployeeBatchGridViewSettings(), GetAccounts("1"));
                case 4:
                    return GridViewExtension.ExportToRtf(GetEmployeeBatchGridViewSettings(), GetAccounts("1"));
                case 5:
                    return GridViewExtension.ExportToCsv(GetEmployeeBatchGridViewSettings(), GetAccounts("1"));
                //break;

                default:
                    break;
            }

            return null;
        }
        private GridViewSettings GetEmployeeBatchGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "gridcrmAccount";
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Account";

            settings.Columns.Add(column =>
            {
                column.Caption = "Account Name";
                column.FieldName = "Account_Name";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Account_Code";
                column.Caption = "Account Code";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Contact_Name";
                column.Caption = "Contact Name";
            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "PhoneNo";
                column.Caption = "Phone No";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "countryName";
                column.Caption = "Country";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "PinCode";
                column.Caption = "Pin Code";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "CityName";
                column.Caption = "City";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Status";
                column.Caption = "Status";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Owner";
                column.Caption = "Owner";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "AssignTo";
                column.Caption = "Assign To";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "EnteredBy";
                column.Caption = "Entered By";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "EnteredOn";
                column.Caption = "Entered On";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ModifiedBy";
                column.Caption = "Updated By";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ModifiedOn";
                column.Caption = "Updated On";
            });

            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }

        public ActionResult PopulateContactGrid()
        {
            return PartialView(@"~/Views/CRMS/Account/_PartialConTact.cshtml", GetContactData());
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

        public ActionResult PopulateOpportunityGrid()
        {
            return PartialView(@"~/Views/CRMS/Account/_PartialOpportunity.cshtml", GetOpportunityData());
        }

        public IEnumerable GetOpportunityData()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dc = new CRMClassDataContext(connectionString);
            var q = from d in dc.v_crmOpportunities
                    orderby d.FirstName
                    select d;
            return q;
        }

	}
}