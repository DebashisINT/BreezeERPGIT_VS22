using CRM.DAL;
using CRM.Models;
using CRM.Models.DataContext;
using DevExpress.Web;
using DevExpress.Web.Mvc;
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
    public class CRMContactController : Controller
    {
     
            UserRightsForPage rights = new UserRightsForPage();
        
        //
        // GET: /CRMContact/
        public ActionResult Index()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
            crmContact CRMContactobj = new crmContact();


            List<crm_CampaignType> crm_CampaignTypelist = new List<Models.DataContext.crm_CampaignType>();
            crm_CampaignTypelist.Add(new crm_CampaignType { Id = 0, Campaign_Code = "Select" });
            var querycrm_CampaignTypes = (from u in crm_CampaignTypelist
                                          select u).Union(from c in dcon.crm_CampaignTypes
                                                          select c);
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

            List<v_jobResponsibility> lstv_jobResponsibilitie = new List<v_jobResponsibility>();
            lstv_jobResponsibilitie.Add(new v_jobResponsibility { job_id = 0, job_responsibility = "Select" });
            var querycrm_Job = (from u in lstv_jobResponsibilitie
                                select u).Union(from c in dcon.v_jobResponsibilities
                                                select c);

            List<V_CNTACCOUNTLIST> lstV_CNTACCOUNTLIST = new List<V_CNTACCOUNTLIST>();
            lstV_CNTACCOUNTLIST.Add(new V_CNTACCOUNTLIST { cnt_internalId = "", Name = "Select" });
            var queryV_CNTACCOUNTLIST = (from u in lstV_CNTACCOUNTLIST
                                         select u).Union(from c in dcon.V_CNTACCOUNTLISTs
                                                select c);


            List<MarketingMaterials> listmarketing = new List<MarketingMaterials>();
            MarketingMaterials mrkmat = new MarketingMaterials();

            mrkmat.material_id = 0;
            mrkmat.material_Name = "None";
            listmarketing.Add(mrkmat);
            mrkmat = new MarketingMaterials();
            mrkmat.material_id = 1;
            mrkmat.material_Name = "Sent";
            listmarketing.Add(mrkmat);
            mrkmat = new MarketingMaterials();
            mrkmat.material_id = 2;
            mrkmat.material_Name = "Do Not Sent";
            listmarketing.Add(mrkmat);

            CRMContactobj.marketingmaterials = listmarketing;



            var querycrm_Marital = from c in dcon.v_maritalstatus
                                                select c;


            List<Master_PaymentTerm> lstMaster_PaymentTerm = new List<Master_PaymentTerm>();
            lstMaster_PaymentTerm.Add(new Master_PaymentTerm { ID = 0, Terms = "Select" });
            var queryMaster_PaymentTerm = (from u in lstMaster_PaymentTerm
                                select u).Union(from c in dcon.Master_PaymentTerms
                                                select c);


            List<Master_ShippingMethod> lstMaster_ShippingMethod = new List<Master_ShippingMethod>();
            lstMaster_ShippingMethod.Add(new Master_ShippingMethod { ID = 0, Methods = "Select" });
            var queryMaster_ShippingMethod = (from u in lstMaster_ShippingMethod
                                           select u).Union(from c in dcon.Master_ShippingMethods
                                                           select c);

            List<Master_PrefferedMethodofContact> lstMaster_PrefferedMethodofContact = new List<Master_PrefferedMethodofContact>();
            lstMaster_PrefferedMethodofContact.Add(new Master_PrefferedMethodofContact { ID = 0, Contact_Methods = "Select" });
            var queryMaster_PrefferedMethodofContact = (from u in lstMaster_PrefferedMethodofContact
                                           select u).Union(from c in dcon.Master_PrefferedMethodofContacts
                                                           select c);
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "CRMContact");

            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanExport = rights.CanExport;



            CRMContactobj.PrefferedMethodofContact_List = queryMaster_PrefferedMethodofContact.ToList();
            CRMContactobj.PaymentTerm_List = queryMaster_PaymentTerm.ToList();
            CRMContactobj.ShippingMethod_List = queryMaster_ShippingMethod.ToList();
            CRMContactobj.maritalstatus_List = querycrm_Marital.ToList();
            CRMContactobj.ContactSource = querycrm_contactsource.ToList();
            CRMContactobj.Users = querycrm_users.ToList();
            CRMContactobj.Status_Details = querycrm_StatusDetails.ToList();
            CRMContactobj.WonerID = Convert.ToInt64(Session["userid"]);
            CRMContactobj.AssignedID = Convert.ToInt64(Session["userid"]);
            CRMContactobj.Rating_List = querycrm_rating.ToList();
            CRMContactobj.jobResponsibility_List = querycrm_Job.ToList();
            CRMContactobj.V_CNTACCOUNTLISTS = queryV_CNTACCOUNTLIST.ToList();
            return View(@"~/Views/CRMS/Contact/crmContact.cshtml", CRMContactobj);
        }

        public ActionResult SaveContact(crmContact crmContactobj)
        {
            crmContact cntObj = new crmContact();

            string Output=cntObj.SaveContact(crmContactobj);

            return Json(Output,JsonRequestBehavior.AllowGet);
        }

        public ActionResult PopulateGrid(string frmdate)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "CRMContact");

            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.SupervisorFeedback = rights.SupervisorFeedback;
            ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
            ViewBag.Verified = rights.Verified;
            ViewBag.CanCreateActivity = rights.CanSalesActivity;
            ViewBag.CanProducts = rights.CanProducts;
            ViewBag.CanSharing = rights.CanSharing;
            ViewBag.CanLiterature = rights.CanLiterature;
            return PartialView(@"~/Views/CRMS/Contact/PartialContactGrid.cshtml", GetSalesSummary(frmdate));
        }
        public IEnumerable GetSalesSummary(string frmdate)
        {

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);

            if (frmdate != "Ispageload")
            {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                // Rev Mantis Issue 24056
                //var q = from d in dc.v_crmContacts
                //        where Convert.ToString(d.AssignToID) == Userid || Convert.ToString(d.OwnerID) == Userid orderby d.order_date descending
                //        select d ;
                //return q;

                BusinessLogicLayer.CommonBL ComBL = new BusinessLogicLayer.CommonBL();
                string ShowContactsDataForAllUsers = ComBL.GetSystemSettingsResult("ShowContactsDataForAllUsers");

                if (ShowContactsDataForAllUsers.ToUpper() == "NO")
                {
                    var q = from d in dc.v_crmContacts
                            where Convert.ToString(d.AssignToID) == Userid || Convert.ToString(d.OwnerID) == Userid
                            orderby d.order_date descending
                            select d;
                    return q;
                }
                else
                {
                    var q = from d in dc.v_crmContacts
                            orderby d.order_date descending
                            select d;
                    return q;
                }
                // End of Rev Mantis issue 24056  
            }
            else
            {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                var q = from d in dc.v_crmContacts
                        where 1==0
                        select d;
                return q;
            }



        }


        public ActionResult EditContact(crmContact newCRMContactobj)
        {
            crmContact crmContactobj = new crmContact();
            DataSet output = crmContactobj.EditCampaign(newCRMContactobj);
            crmContactobj = APIHelperMethods.ToModel<crmContact>(output.Tables[0]);
            crmContactobj.LEAD_LIST = output.Tables[1].AsEnumerable()
                           .Select(r => r.Field<string>("LEAD_ID"))
                           .ToList(); 

            // Mantis Issue 21677,21676,23104 (03/06/2021)
            crmContactobj.oppids = output.Tables[2].AsEnumerable()
               .Select(r => r.Field<string>("OPPORTUNITY_ID"))
               .ToList(); ;
           
            // End of Mantis Issue 21677,21676,23104 (03/06/2021)

            return Json(crmContactobj);
        }

        //Mantis Issue 21677,21676,23104 (03/06/2021)
        public ActionResult PopulateOpportunityGrid()
        {
            return PartialView(@"~/Views/CRMS/Contact/PartialOpportunityView.cshtml", GetOpportunityData());
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
        // End of Mantis Issue 21677,21676,23104 (03/06/2021)

        public ActionResult DeleteContact(crmContact newCRMContactobj)
        {
            crmContact crmContactobj = new crmContact();
            string output = crmContactobj.DeleteContact(newCRMContactobj);
            return Json(output);
        }


        public ActionResult ExportCRMContactList(int type)
        {


            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToPdf(GetEmployeeBatchGridViewSettings(), GetSalesSummary("1"));
                //break;
                case 2:
                    return GridViewExtension.ExportToXlsx(GetEmployeeBatchGridViewSettings(), GetSalesSummary("1"));
                //break;
                case 3:
                    return GridViewExtension.ExportToXls(GetEmployeeBatchGridViewSettings(), GetSalesSummary("1"));
                case 4:
                    return GridViewExtension.ExportToRtf(GetEmployeeBatchGridViewSettings(), GetSalesSummary("1"));
                case 5:
                    return GridViewExtension.ExportToCsv(GetEmployeeBatchGridViewSettings(), GetSalesSummary("1"));
                //break;

                default:
                    break;
            }

            return null;
        }
        private GridViewSettings GetEmployeeBatchGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "gridcrmContact";
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Contact";

            settings.Columns.Add(x =>
            {
                x.Caption = "Name";
                x.FieldName = "Name";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);
                x.FixedStyle = GridViewColumnFixedStyle.Left;
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Job Title";
                x.FieldName = "job_responsibility";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);
                x.FixedStyle = GridViewColumnFixedStyle.Left;

            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Account Name";
                x.FieldName = "Acc_Name";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            });



            settings.Columns.Add(x =>
            {
                x.Caption = "Email";
                x.FieldName = "Email_Id";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Fax";
                x.FieldName = "Fax";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            });


            settings.Columns.Add(x =>
            {
                x.Caption = "Business Phone";
                x.FieldName = "Business_Phone";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Mobile";
                x.FieldName = "Mobile";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Gender";
                x.FieldName = "Gender";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            });

            //settings.Columns.Add(x =>
            //{
            //    x.Caption = "Status Code";
            //    x.FieldName = "Status_Code";
            //    x.ColumnType = MVCxGridViewColumnType.TextBox;
            //    x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            //    x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            //});


            settings.Columns.Add(x =>
            {
                x.Caption = "Mobile";
                x.FieldName = "Mobile";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Lead Name";
                x.FieldName = "Lead_Name";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            });



            settings.Columns.Add(x =>
            {
                x.Caption = "Opportunity Name";
                x.FieldName = "Lead_Opp";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            });



            settings.Columns.Add(x =>
            {
                x.Caption = "Assign User";
                x.FieldName = "Assign_User";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Created By";
                x.FieldName = "Created_By";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Created On";
                x.FieldName = "Created_On";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            });


            settings.Columns.Add(x =>
            {
                x.Caption = "Modified By";
                x.FieldName = "Modified_By";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Modified On";
                x.FieldName = "Modified_On";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);

            });

            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }


        public JsonResult checkValidation(ValidCondition vcondition)
        {
            Validation validation = new Validation();


            List<ValidCondition> listvalidation = new List<ValidCondition>();
            listvalidation.Add(vcondition);

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);
            int isexists = validation.CheckExistancy("crm_contact", "cont_shortName", listvalidation);


            return Json(isexists);
        }

        public ActionResult PopulateLeadGrid()
        {

            return PartialView(@"~/Views/CRMS/Contact/_PartialLead.cshtml", GetLeadData());
        }

        public IEnumerable GetLeadData()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dc = new CRMClassDataContext(connectionString);
            var q = from d in dc.V_CRMLeads
                    orderby d.Name
                    select d;
            return q;
        }

        


    }
}