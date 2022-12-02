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
using System.Data.SqlClient;
using EntityLayer.CommonELS;

namespace CRM.Controllers.CRMS
{
    public class CRMLeadsController : Controller
    {
        UserRightsForPage rights = new UserRightsForPage();
        //
        // GET: /CRMLeads/
        public ActionResult Index()
        {

            BusinessLogicLayer.CommonBL ComBL = new BusinessLogicLayer.CommonBL();
            string DisableOwnerLead = ComBL.GetSystemSettingsResult("DisableOwnerLead");
            string MultipleKeyPersons = ComBL.GetSystemSettingsResult("MultipleKeyPersons");
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "CRMLeads");

            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanExport = rights.CanExport;
            ViewBag.DisableOwnerLead = DisableOwnerLead;
            ViewBag.MultipleKeyPersons = MultipleKeyPersons;



            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
            crmLeads crmLeadsobj = new crmLeads();


            var ModeOfVisit = from c in dcon.V_ModeOfVisits
                                 select c;
            crmLeadsobj.Mode_Visit = ModeOfVisit.ToList();


            var BusinessActivities = from c in dcon.V_BusinessActivities
                              select c;
            crmLeadsobj.Business_Activities = BusinessActivities.ToList();


            var BusinessPresences = from c in dcon.V_BusinessPresences
                              select c;
            crmLeadsobj.Business_Presence = BusinessPresences.ToList();


            var Brand = from c in dcon.V_Brands
                              select c;
            crmLeadsobj.Brand = Brand.ToList();

            var Designation = from c in dcon.V_Designations
                        select c;
            crmLeadsobj.Person_Designation = Designation.ToList();


            var CurrentRequirement = from c in dcon.V_CurrentRequirements
                                     select c;
            crmLeadsobj.Current_Requirement = CurrentRequirement.ToList();


            var Product_Applications = from c in dcon.V_ProductApplications
                                    select c;
            crmLeadsobj.Product_Application = Product_Applications.ToList();

            var PainArea = from c in dcon.V_PainAreas
                                select c;
            crmLeadsobj.Pain_Area = PainArea.ToList();
            


            var querycrm_users = from c in dcon.V_UserLIsts
                                 select c;
            crmLeadsobj.Users = querycrm_users.ToList();

            var querycrm_contactsource =
                                        (from c in dcon.v_ContactSources
                                         select c);
            crmLeadsobj.ContactSource = querycrm_contactsource.ToList();



            var querycrm_cntsource =
                                        (from c in dcon.v_crmContacts
                                         select c);
            crmLeadsobj.crmcontacts = querycrm_cntsource.ToList();



            //var querycrm_StatusDetails = from c in dcon.v_StatusDetails
            //                             where c.Status_Code != "Close"
            //                             select c;

            var querycrm_StatusDetails = from c in dcon.v_StatusDetails
                                         where c.Status_Code == "New"
                                         select c;
            crmLeadsobj.Status_Details = querycrm_StatusDetails.ToList();

            var querycrm_RatingsDetails = from c in dcon.V_Ratings
                                         select c;
            crmLeadsobj.Ratings = querycrm_RatingsDetails.ToList();

            var querycrm_IndustryDetails = from c in dcon.V_Industries
                                          select c;
            crmLeadsobj.Industries = querycrm_IndustryDetails.ToList();

            var querycrm_jobTitleDetails = from c in dcon.v_jobResponsibilities
                                          select c;
            crmLeadsobj.jobtitles = querycrm_jobTitleDetails.ToList();

            var querycrm_Campaign = from c in dcon.v_Campaigns
                                    select c;
            crmLeadsobj.CampaignList = querycrm_Campaign.ToList();


            var _ProductClass = from c in dcon.V_ProductClasses
                                    select c;
            crmLeadsobj.Product_Class = _ProductClass.ToList();

            var Sector = from c in dcon.V_SectorLists
                                select c;
            crmLeadsobj.Sector_List = Sector.ToList();


            var Assign_To = from c in dcon.V_AssigneLists
                         select c;
            crmLeadsobj.AssignTo = Assign_To.ToList();


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

            crmLeadsobj.marketingmaterials = listmarketing;
            
            crmLeadsobj.OwnerID = Convert.ToInt64(Session["userid"]);
            crmLeadsobj.AssignedID = Convert.ToInt64(Session["userid"]);
            crmLeadsobj.Owner = Convert.ToString(System.Web.HttpContext.Current.Session["UserName"]);
            crmLeadsobj.Status_Id = 7; //status_id=7 => New

            ViewBag.CurrentUser = crmLeadsobj.OwnerID;

            ViewBag.OwnerAssignID = Convert.ToInt64(Session["userid"]);
            crmLeadsobj.custdetails = new CustomerDetails();
            return View(@"~/Views/CRMS/Leads/CRMLeads.cshtml", crmLeadsobj);
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
                pro.AddVarcharPara("@Action", 50, "CustomAddressByPin");
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
        [HttpPost]

        [ValidateInput(false)]
        public ActionResult SaveCRMLeads(crmLeads newcrmLeadsobj)
        {
            crmLeads CRMLeadsobj = new crmLeads();
            string output = "";
            output = CRMLeadsobj.SaveLeads(newcrmLeadsobj);

            if (CRMLeadsobj.Action == "Update")
            {
                if (CRMLeadsobj.Status_Id == 13) //Qualify
                {
                    string outputqualified = "";
                    CRMLeadsobj.Status = "Qualify";
                    // Rev Mantis Issue 23438 [ to be done from SP "PROC_CRM_LEADCONTACTINSERT" at the time of save from CRMLeadsobj.SaveLeads(newcrmLeadsobj)
                    //outputqualified = CRMLeadsobj.QualifiedLeads(newcrmLeadsobj);
                    // End of Rev Mantis Issue 23438
                }
                else if (CRMLeadsobj.Status_Id == 9) //Cancelled/Lost
                {
                    string outputcancelled = "";
                    CRMLeadsobj.Status = "Cancelled/Lost";
                    outputcancelled = CRMLeadsobj.LostLeads(newcrmLeadsobj);
                }

            }
            
            //if (CRMLeadsobj.Status == "Qualify")
            //{
            //    string outputqualified = "";
            //    outputqualified = CRMLeadsobj.QualifiedLeads(newcrmLeadsobj);
            //}
            //else if (CRMLeadsobj.Status == "Cancelled/Lost")
            //{
            //    string outputcancelled = "";
            //    outputcancelled = CRMLeadsobj.LostLeads(newcrmLeadsobj);
            //}

            return Json(output, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditCRMLeads(crmLeads newcrmLeadsobj)
         {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
             
            crmLeads CRMLeadsobj = new crmLeads();
            DataSet output = CRMLeadsobj.EditCRMLeads(newcrmLeadsobj);
            CustomerDetails clsObj = new CustomerDetails();
            CRMLeadsobj = APIHelperMethods.ToModel<crmLeads>(output.Tables[0]);
            clsObj = APIHelperMethods.ToModel<CustomerDetails>(output.Tables[1]);
            CRMLeadsobj.cntids = output.Tables[2].AsEnumerable()
                           .Select(r => r.Field<string>("CONTACT_ID"))
                           .ToList(); ;
            CRMLeadsobj.custdetails = clsObj;



            //var querycrm_StatusDetails = from c in dcon.v_StatusDetails
            //                             where c.Status_Code != "Close" 
            //                             select c;
            //CRMLeadsobj.Status_Details = querycrm_StatusDetails.ToList();
            var querycrm_StatusDetails = from c in dcon.v_StatusDetails
                                         where c.Status_Code == "New" || c.Status_Code == "Cancelled/Lost" || c.Status_Code == "Qualify" 
                                         select c;
            CRMLeadsobj.Status_Details = querycrm_StatusDetails.ToList();



          
            return Json(CRMLeadsobj);
        }

        public ActionResult GetCRMProductsDetails(string Lead_Id)
        {
            crmLeads pro = new crmLeads();

            DataTable dtProds = pro.GetCRMProductsDetails(Lead_Id);
            List<crmKey> finalResult = new List<crmKey>();
            if (dtProds != null)
            {
                finalResult = DbHelpers.ToModelList<crmKey>(dtProds);
            }

            return Json(finalResult);
        }

        public ActionResult DeleteCRMLeads(crmLeads newcrmLeadsobj)
        {
            crmLeads CRMLeadsobj = new crmLeads();
            string output = "";
            output = CRMLeadsobj.DeleteLeads(newcrmLeadsobj);
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveCRMKeyDetails(string list, string InternalId)
        {
            string output = "";
            crmLeads CRMLeadsobj = new crmLeads();
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<crmKey> finalResult = jsSerializer.Deserialize<List<crmKey>>(list);
            DataTable dt_KeyDetails = new DataTable();
            dt_KeyDetails.Columns.Add("Id", typeof(string));           
            dt_KeyDetails.Columns.Add("Name", typeof(String));
            dt_KeyDetails.Columns.Add("Description", typeof(String));
            dt_KeyDetails.Columns.Add("PhoneNumber", typeof(String));
            dt_KeyDetails.Columns.Add("IsDefault", typeof(bool));
            string validate = "";
            foreach (var item in finalResult)
            {                
                dt_KeyDetails.Rows.Add(item.guid, item.Name, item.DesignationID, item.PhoneNumber, item.IsDefault);                
            }
            var duplicateRecords = dt_KeyDetails.AsEnumerable()
               .GroupBy(r => r["IsDefault"]) //coloumn name which has the duplicate values
               .Where(gr => gr.Count() > 1)
                .Select(g => g.Key);

            foreach (var d in duplicateRecords)
            {
                validate = "duplicateDefault";
            }

            if (validate == "duplicateDefault")
            {
                //item.IsDefault = "false";
                return Json("duplicateDefault", JsonRequestBehavior.AllowGet);
            }
           
            output = CRMLeadsobj.SaveKeyDetails(dt_KeyDetails, InternalId);

            return Json(output, JsonRequestBehavior.AllowGet);

        }
        public ActionResult PopulateLeadGrid(string frmdate)
        {
            BusinessLogicLayer.CommonBL ComBL = new BusinessLogicLayer.CommonBL();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "CRMLeads");
            string MultipleKeyPersons = ComBL.GetSystemSettingsResult("MultipleKeyPersons");
            
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanConvertTo = rights.CanConvertTo;
            ViewBag.CanCreateActivity = rights.CanSalesActivity;

            ViewBag.CanQualify = rights.CanQualify;
            ViewBag.CanCancelLost = rights.CanCancelLost;
            ViewBag.CanProducts = rights.CanProducts;
            ViewBag.CanSharing = rights.CanSharing;
            ViewBag.CanLiterature = rights.CanLiterature;
            ViewBag.MultipleKeyPersons = MultipleKeyPersons;          

            return PartialView(@"~/Views/CRMS/Leads/PartialLeadGridView.cshtml", GetLeads(frmdate));
        }
        
        public IEnumerable GetLeads(string frmdate)
        {
            crmLeads CRMLeadsobj = new crmLeads();

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);

            DateTime? FromDate = null;
            DateTime? ToDate = null;
            string ActivityFilter = "";
            if (TempData["FromDate"] != null && TempData["ToDate"] != null)
            {
                FromDate = Convert.ToDateTime(TempData["FromDate"]);
                ToDate = Convert.ToDateTime(TempData["ToDate"]);
                TempData.Keep();
            }
            if (TempData["ActivityFilter"] != "" )
            {
                ActivityFilter = Convert.ToString(TempData["ActivityFilter"]);
                TempData.Keep();
            }

            if (frmdate != "Ispageload")
            {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                // Rev Mantis issue 24049
                //var q = from d in dc.V_CRMLeads
                //        where Convert.ToString(d.Assign_To) == Userid || Convert.ToString(d.Owner_Id) == Userid
                //        orderby d.CreateDate descending
                //        select d;
                //return q;

                BusinessLogicLayer.CommonBL ComBL = new BusinessLogicLayer.CommonBL();
                string ShowLeadDataForAllUsers = ComBL.GetSystemSettingsResult("ShowLeadDataForAllUsers");              
               
                    if (ShowLeadDataForAllUsers.ToUpper() == "NO")
                    {
                            var q = from d in dc.V_CRMLeads
                                    where Convert.ToString(d.Assign_To) == Userid || Convert.ToString(d.Owner_Id) == Userid && d.Status == ActivityFilter                                  
                                    orderby d.CreateDate descending
                                    select d;
                            

                        if (ActivityFilter == "DateFilter")
                        {
                             q = from d in dc.V_CRMLeads
                                where d.CreateDate >= Convert.ToDateTime(FromDate) && d.CreateDate <= Convert.ToDateTime(ToDate) && Convert.ToString(d.Assign_To) == Userid || Convert.ToString(d.Owner_Id) == Userid

                                orderby d.CreateDate descending
                                select d;
                            //return qq;
                        }
                        if (ActivityFilter == "")
                        {
                            q = from d in dc.V_CRMLeads
                                where Convert.ToString(d.Assign_To) == Userid || Convert.ToString(d.Owner_Id) == Userid
                                orderby d.CreateDate descending
                                select d;
                            
                        }
                        
                        return q;
                        
                    }
                    else
                    {
                        var q = from d in dc.V_CRMLeads
                                    where  d.Status == ActivityFilter                                  
                                    orderby d.CreateDate descending
                                    select d;
                          

                            if (ActivityFilter == "DateFilter")
                            {
                                q = from d in dc.V_CRMLeads
                                    where d.CreateDate >= Convert.ToDateTime(FromDate) && d.CreateDate <= Convert.ToDateTime(ToDate)
                                    orderby d.CreateDate descending
                                    select d;
                                //return q;
                            }

                            if (ActivityFilter == "")
                            {
                                q = from d in dc.V_CRMLeads                                   
                                    orderby d.CreateDate descending
                                    select d;

                            }

                            return q;
                       
                    }
              //  }
                // End of Rev Mantis issue 24049  
            }
            else
            {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                var q = from d in dc.V_CRMLeads
                        where 1==0
                        orderby d.CreateDate descending
                        select d;
                return q;
            }



        }

        //public JsonResult checkValidation(Validation validation)
        //{
        //    ValidCondition vcondition=new ValidCondition();

        //    List<ValidCondition> listvalidation = new List<ValidCondition>();

        //    string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        //    string Userid = Convert.ToString(Session["userid"]);
        //    int isexists = validation.CheckExistancy("tbl_master_contact", "cnt_shortName", listvalidation);


        //    return Json(Userid);
        //}

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

        public ActionResult QualifyCRMLeads(crmLeads newcrmLeadsobj)
        {
            crmLeads CRMLeadsobj = new crmLeads();
            string output = "";
            output = CRMLeadsobj.QualifiedLeads(newcrmLeadsobj);
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LostCRMLeads(crmLeads newcrmLeadsobj)
        {
            crmLeads CRMLeadsobj = new crmLeads();
            string output = "";
            output = CRMLeadsobj.LostLeads(newcrmLeadsobj);
            return Json(output, JsonRequestBehavior.AllowGet);
        }

       public ActionResult  SelectedSourceType(crmLeads newcrmLeadsobj)
       {
            crmLeads CRMLeadsobj = new crmLeads();
            string output = "";
            output = CRMLeadsobj.SelectedSourceType(newcrmLeadsobj);
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportCRMLeadsList(int type)
        {


            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToPdf(GetEmployeeBatchGridViewSettings(), GetLeads("1"));
                //break;
                case 2:
                    return GridViewExtension.ExportToXlsx(GetEmployeeBatchGridViewSettings(), GetLeads("1"));
                //break;
                case 3:
                    return GridViewExtension.ExportToXls(GetEmployeeBatchGridViewSettings(), GetLeads("1"));
                case 4:
                    return GridViewExtension.ExportToRtf(GetEmployeeBatchGridViewSettings(), GetLeads("1"));
                case 5:
                    return GridViewExtension.ExportToCsv(GetEmployeeBatchGridViewSettings(), GetLeads("1"));
                //break;

                default:
                    break;
            }

            return null;
        }
        private GridViewSettings GetEmployeeBatchGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "gridcrmLead";
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Lead";

            settings.Columns.Add(x =>
            {
                x.Caption = "Unique Id";
                x.FieldName = "UniqueID";
             });
            settings.Columns.Add(x =>
            {
                x.Caption = "Name";
                x.FieldName = "Name";
            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Key Person Name";
                x.FieldName = "KeyName";
            });
            // Rev Sayantani
            settings.Columns.Add(x =>
            {
                x.Caption = "Designation";
                x.FieldName = "KeyDescription";
            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Phone Number";
                x.FieldName = "PhoneNumber";
            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Company Name";
                x.FieldName = "CompanyName";
            });
            // Rev Sayantani
            settings.Columns.Add(x =>
            {
                x.Caption = "Business #";
                x.FieldName = "BusinessPhone";
            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Mobile #";
                x.FieldName = "MobileNo";
            });
            // Rev Sayantani
            settings.Columns.Add(x =>
            {
                x.Caption = "Topic/Purpose";
                x.FieldName = "Topic";
            });


            settings.Columns.Add(x =>
            {
                x.Caption = "City";
                x.FieldName = "CityName";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Country";
                x.FieldName = "CountryName";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "State Name";
                x.FieldName = "StateName";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Pin";
                x.FieldName = "PinCode";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Owner";
                x.FieldName = "OwnerName";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Assign To";
                x.FieldName = "AssignToName";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Lead Source";
                x.FieldName = "LeadSourceName";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Lead Values";
                x.FieldName = "LeadSourceValue";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Rating";
                x.FieldName = "rat_LeadRating";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Status";
                x.FieldName = "Status";
            });
          
            settings.Columns.Add(x =>
            {
                x.Caption = "Sector";
                x.FieldName = "Lead_Sector";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Mode Of Visit";
                x.FieldName = "Lead_ModeOfVisit";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Credential Submitted";
                x.FieldName = "CredentialSubmitted";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Business Activities";
                x.FieldName = "Lead_BusinessActivities";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Product Application";
                x.FieldName = "Lead_ProductApplication";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Type of Material";
                x.FieldName = "TypeMaterial";
            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Volume of Business";
                x.FieldName = "VolumeBusiness";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Value Business expected";
                x.FieldName = "ValueBusinessExpected";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Business Presence";
                x.FieldName = "Lead_BusinessPresence";
            });

            
            settings.Columns.Add(x =>
            {
                x.Caption = "Brand";
                x.FieldName = "Brand_Name";
            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Competitor";
                x.FieldName = "Competitor";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Pain Area";
                x.FieldName = "Lead_PainArea";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Current requirement";
                x.FieldName = "Lead_CurrentRequirements";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Financial Status";
                x.FieldName = "FinancialStatus";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Financial Status";
                x.FieldName = "EnquiryExpected";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Remarks";
                x.FieldName = "Remarks";
            });
            
            settings.Columns.Add(x =>
            {
                x.Caption = "Entered By";
                x.FieldName = "EnteredByName";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Entered On";
                x.FieldName = "EnteredOn";
            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Updated By";
                x.FieldName = "ModifiedByName";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Updated On";
                x.FieldName = "ModifiedOn";
            });


            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }

        public ActionResult GetLeadValues(string SearchKey, string ContactType)
        {
            List<crmLeadValuesModel> listCust = new List<crmLeadValuesModel>();
            if (Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable user = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("GetContactBind_Search", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.Parameters.AddWithValue("@ContactType", ContactType);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(user);

                cmd.Dispose();
                con.Dispose();

                listCust = (from DataRow dr in user.Rows
                            select new crmLeadValuesModel()
                            {
                                id = dr["Id"].ToString(),
                                Name = dr["Name"].ToString()
                               
                            }).ToList();
            }

            return Json(listCust);
        }

        public ActionResult AddStatusPopulate()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);

            crmLeads CRMLeadsobj = new crmLeads();
            
            var querycrm_StatusDetails = from c in dcon.v_StatusDetails
                                         where c.Status_Code == "New"
                                         select c;
            CRMLeadsobj.Status_Details = querycrm_StatusDetails.ToList();
            return Json(CRMLeadsobj);
        }


        public JsonResult GetLeadData(string MobileNo)
        {
            crmLeads CRMLeadsobj = new crmLeads();
            List<crmLeadMobileDetails> objlst = new List<crmLeadMobileDetails>();
            objlst = CRMLeadsobj.GetLeadMobileDetails(MobileNo);
            return Json(objlst);
        }



        public ActionResult PopulateContactGrid()
        {
            //if (TempData["CONTACT_IDs"] != null)
            //{
            //    ViewBag.CONTACT_IDs = TempData["CONTACT_IDs"];
            //}
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

        public JsonResult SetLeadDateFilter(string FromDate, string ToDate, string ActivityFilter)
        {
            Boolean Success = false;
            try
            {
                TempData["ActivityFilter"] = ActivityFilter;    
                TempData["FromDate"] = FromDate;
                TempData["ToDate"] = ToDate;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }

        public JsonResult SetActivityFilter(string ActivityFilter)
        {
            Boolean Success = false;
            try
            {                
                TempData["ActivityFilter"] = ActivityFilter;                
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }


        public JsonResult ButtonCountShow()
        {
            BusinessLogicLayer.DBEngine odfng = new BusinessLogicLayer.DBEngine(string.Empty);
            string Success = "";
            try
            {
                string NewLead = "0";
                string QualifyLead = "0";
                string Cancelled = "0";
                DateTime nowdate = DateTime.Now;


                DataTable dtNewLead = new DataTable();
                DataTable dtQualify = new DataTable();
                DataTable dtCancelled = new DataTable();
                dtNewLead = odfng.GetDataTable("select COUNT(Status) as NewLead from V_CRMLeads where Status= 'New'");
                dtQualify = odfng.GetDataTable("select COUNT(Status) as QualifyLead from V_CRMLeads where Status= 'Qualify'");
                dtCancelled = odfng.GetDataTable("select COUNT(Status) as CancelledLead from V_CRMLeads where Status= 'Cancelled/Lost'");

                if (dtNewLead != null && dtNewLead.Rows.Count > 0)
                {
                    NewLead = Convert.ToString(dtNewLead.Rows[0]["NewLead"]);
                }
                if (dtQualify != null && dtQualify.Rows.Count > 0)
                {
                    QualifyLead = Convert.ToString(dtQualify.Rows[0]["QualifyLead"]);
                }
                if (dtCancelled != null && dtCancelled.Rows.Count > 0)
                {
                    Cancelled = Convert.ToString(dtCancelled.Rows[0]["CancelledLead"]);
                }
                Success = NewLead + '~' + QualifyLead + '~' + Cancelled;
            }
            catch { }
            return Json(Success);
        }

        public JsonResult SetLeadID(string LeadID)
        {
            Boolean Success = false;
            try
            {
                TempData["LeadID"] = LeadID;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }

        public ActionResult GetKeyPersonList()
        {
            crmLeads pro = new crmLeads();
            Int64 LeadID = 0;
            // string WorkOrderModuleSkipped = cSOrder.GetSystemSettingsResult("WorkOrderModuleSkipped");
            crmKey bomproductdataobj = new crmKey();
            List<crmKey> bomproductdata = new List<crmKey>();
            try
            {
                if (TempData["LeadID"] != null)
                {
                    LeadID = Convert.ToInt64(TempData["LeadID"]);
                    TempData.Keep();
                }

                if (LeadID > 0)
                {
                    DataTable dtProds = pro.GetCRMProductsDetails(Convert.ToString(LeadID));

                    if (dtProds != null && dtProds.Rows.Count > 0)
                    {
                        DataTable dt = dtProds;
                        foreach (DataRow row in dt.Rows)
                        {

                            bomproductdataobj = new crmKey();
                            bomproductdataobj.guid = Convert.ToString(row["guid"]);
                            bomproductdataobj.Name = Convert.ToString(row["Name"]);
                            bomproductdataobj.Designation = Convert.ToString(row["Designation"]);
                            bomproductdataobj.PhoneNumber = Convert.ToString(row["PhoneNumber"]);
                            bomproductdata.Add(bomproductdataobj);
                        }

                    }
                }
            }
            catch { }
            return PartialView(@"~/Views/CRMS/Leads/_KeyPersonDetailsList.cshtml", bomproductdata);
        }
       
	}
}