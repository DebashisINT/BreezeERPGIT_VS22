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
using BusinessLogicLayer;
namespace CRM.Controllers.CRMS
{
    public class InquiryController : Controller
    {
        //
        // GET: /Inquiry/
        UserRightsForPage rights = new UserRightsForPage();
        //
        // GET: /CRMInquirys/
        public ActionResult Index()
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "CRMLeads");

            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanExport = rights.CanExport;



            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
            crmInquirys crmInquirysobj = new crmInquirys();

            var querycrm_users = from c in dcon.V_UserLIsts
                                 select c;
            crmInquirysobj.Users = querycrm_users.ToList();

            var querycrm_contactsource =
                                        (from c in dcon.v_ContactSources
                                         select c);
            crmInquirysobj.ContactSource = querycrm_contactsource.ToList();



            var querycrm_cntsource =
                                        (from c in dcon.v_crmContacts
                                         select c);
            crmInquirysobj.crmcontacts = querycrm_cntsource.ToList();



            var querycrm_multigroup =
                                        (from c in dcon.V_MultiGroups
                                         select c);
            crmInquirysobj.groupList = querycrm_multigroup.ToList();

            //var querycrm_StatusDetails = from c in dcon.v_StatusDetails
            //                             where c.Status_Code != "Close"
            //                             select c;

            var querycrm_StatusDetails = from c in dcon.v_StatusDetails
                                         where c.Status_Code == "New"
                                         select c;
            crmInquirysobj.Status_Details = querycrm_StatusDetails.ToList();

            var querycrm_RatingsDetails = from c in dcon.V_Ratings
                                          select c;
            crmInquirysobj.Ratings = querycrm_RatingsDetails.ToList();

            var querycrm_IndustryDetails = from c in dcon.V_Industries
                                           select c;
            crmInquirysobj.Industries = querycrm_IndustryDetails.ToList();

            var querycrm_jobTitleDetails = from c in dcon.v_jobResponsibilities
                                           select c;
            crmInquirysobj.jobtitles = querycrm_jobTitleDetails.ToList();

            var querycrm_Campaign = from c in dcon.v_Campaigns
                                    select c;
            crmInquirysobj.CampaignList = querycrm_Campaign.ToList();

            //Add Rev Tanmoy for User and Branch Bind
            var querycrm_AccountManager = from c in dcon.V_UserLIsts
                                         select c;
            crmInquirysobj.AccountManagerList = querycrm_AccountManager.ToList();

            var querycrm_BranchDetails = from c in dcon.v_BranchLists
                                          select c;
            crmInquirysobj.BranchList = querycrm_BranchDetails.ToList();
            //End of Rev

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

            crmInquirysobj.marketingmaterials = listmarketing;

            crmInquirysobj.OwnerID = Convert.ToInt64(Session["userid"]);
            crmInquirysobj.AssignedID = Convert.ToInt64(Session["userid"]);
            crmInquirysobj.Owner = Convert.ToString(System.Web.HttpContext.Current.Session["UserName"]);
            crmInquirysobj.Status_Id = 7; //status_id=7 => New
            ViewBag.OwnerAssignID = Convert.ToInt64(Session["userid"]);
            crmInquirysobj.custdetails = new CustomerDetails();
            crmInquirysobj.GroupId = "0";

            crmInquirysobj.BranchId = "0";
            crmInquirysobj.AccountManagerId = "0";

            return View(@"~/Views/CRMS/Inquiry/CRMinquiry.cshtml", crmInquirysobj);
        }
        public JsonResult AddressByPin(string PinCode)
        {

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
            crmInquirys crmInquirysobj = new crmInquirys();

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

            //return View(@"~/Views/CRMS/Inquirys/CRMInquirys.cshtml", crmInquirysobj);
            return Json(addByPin);
        }
        [HttpPost]

        [ValidateInput(false)]
        public ActionResult SaveCRMInquirys(crmInquirys newcrmInquirysobj)
        {
            crmInquirys CRMInquirysobj = new crmInquirys();
            string output = "";

            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //List<crmProd> finalResult = jsSerializer.Deserialize<List<crmProd>>(newcrmInquirysobj.Productlist);
            DataTable dt_activityproducts = new DataTable();
            dt_activityproducts.Columns.Add("Id", typeof(string));
            dt_activityproducts.Columns.Add("ProdId", typeof(Int32));
            dt_activityproducts.Columns.Add("Act_Prod_Qty", typeof(Decimal));
            dt_activityproducts.Columns.Add("Act_Prod_Rate", typeof(Decimal));
            dt_activityproducts.Columns.Add("Act_Prod_Remarks", typeof(String));
            dt_activityproducts.Columns.Add("Act_Prod_Frequency", typeof(String));
            dt_activityproducts.Columns.Add("Act_Prod_Amount", typeof(Decimal));
            if (newcrmInquirysobj.Productlist!=null && newcrmInquirysobj.Productlist.Count > 0)
            {
                foreach (var item in newcrmInquirysobj.Productlist)
                {
                    dt_activityproducts.Rows.Add(item.guid, item.ProductId, item.Quantity, item.Rate, item.Remarks, item.Frequency, item.Amount);
                }
            }



            output = CRMInquirysobj.SaveInquirys(newcrmInquirysobj, dt_activityproducts);

            if (CRMInquirysobj.Action == "Update")
            {
                if (CRMInquirysobj.Status_Id == 13) //Qualify
                {
                    string outputqualified = "";
                    CRMInquirysobj.Status = "Qualify";
                    outputqualified = CRMInquirysobj.QualifiedInquirys(newcrmInquirysobj);
                }
                else if (CRMInquirysobj.Status_Id == 9) //Cancelled/Lost
                {
                    string outputcancelled = "";
                    CRMInquirysobj.Status = "Cancelled/Lost";
                    outputcancelled = CRMInquirysobj.LostInquirys(newcrmInquirysobj);
                }

            }

            //if (CRMInquirysobj.Status == "Qualify")
            //{
            //    string outputqualified = "";
            //    outputqualified = CRMInquirysobj.QualifiedInquirys(newcrmInquirysobj);
            //}
            //else if (CRMInquirysobj.Status == "Cancelled/Lost")
            //{
            //    string outputcancelled = "";
            //    outputcancelled = CRMInquirysobj.LostInquirys(newcrmInquirysobj);
            //}

            return Json(output, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditCRMInquirys(crmInquirys newcrmInquirysobj)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);

            crmInquirys CRMInquirysobj = new crmInquirys();
            DataSet output = CRMInquirysobj.EditCRMInquirys(newcrmInquirysobj);
            CustomerDetails clsObj = new CustomerDetails();
            CRMInquirysobj = APIHelperMethods.ToModel<crmInquirys>(output.Tables[0]);
            clsObj = APIHelperMethods.ToModel<CustomerDetails>(output.Tables[1]);
            //CRMInquirysobj.cntids = output.Tables[2].AsEnumerable()
            //               .Select(r => r.Field<string>("CONTACT_ID"))
            //               .ToList(); ;
            CRMInquirysobj.custdetails = clsObj;

            if (output.Tables[2] != null && output.Tables[2].Rows.Count > 0)
            {
                CRMInquirysobj.s_Address1 = Convert.ToString(output.Tables[2].Rows[0]["Address1"]);
                CRMInquirysobj.s_Address2 = Convert.ToString(output.Tables[2].Rows[0]["Address2"]);
                CRMInquirysobj.s_Address3 = Convert.ToString(output.Tables[2].Rows[0]["Address3"]);
                CRMInquirysobj.s_Landmark = Convert.ToString(output.Tables[2].Rows[0]["Landmark"]);
                CRMInquirysobj.s_Pin = Convert.ToString(output.Tables[2].Rows[0]["PinCode"]);
                CRMInquirysobj.s_City = Convert.ToString(output.Tables[2].Rows[0]["CityName"]);
                CRMInquirysobj.s_State = Convert.ToString(output.Tables[2].Rows[0]["StateName"]);
                CRMInquirysobj.s_Country = Convert.ToString(output.Tables[2].Rows[0]["CountryName"]);
                CRMInquirysobj.s_Pinid = Convert.ToString(output.Tables[2].Rows[0]["PinId"]);
                CRMInquirysobj.s_Cityid = Convert.ToString(output.Tables[2].Rows[0]["CityId"]);
                CRMInquirysobj.s_Stateid = Convert.ToString(output.Tables[2].Rows[0]["StateId"]);
                CRMInquirysobj.s_Countryid = Convert.ToString(output.Tables[2].Rows[0]["CountryId"]);

            }

            List<crmAddProd> finalResult = new List<crmAddProd>();
            if (output.Tables[3] != null && output.Tables[3].Rows.Count > 0)
            {
                finalResult = DbHelpers.ToModelList<crmAddProd>(output.Tables[3]);
            }

            CRMInquirysobj.Productlist = finalResult;
            //var querycrm_StatusDetails = from c in dcon.v_StatusDetails
            //                             where c.Status_Code != "Close" 
            //                             select c;
            //CRMInquirysobj.Status_Details = querycrm_StatusDetails.ToList();
            var querycrm_StatusDetails = from c in dcon.v_StatusDetails
                                         where c.Status_Code == "New" || c.Status_Code == "Cancelled/Lost" || c.Status_Code == "Qualify"
                                         select c;
            CRMInquirysobj.Status_Details = querycrm_StatusDetails.ToList();




            return Json(CRMInquirysobj);
        }

        public ActionResult DeleteCRMInquirys(crmInquirys newcrmInquirysobj)
        {
            crmInquirys CRMInquirysobj = new crmInquirys();
            string output = "";
            output = CRMInquirysobj.DeleteInquirys(newcrmInquirysobj);
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PopulateInquiryGrid(string frmdate)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "CRMLeads");


            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanConvertTo = rights.CanConvertTo;
            ViewBag.CanCreateActivity = rights.CanSalesActivity;

            ViewBag.CanQualify = rights.CanQualify;
            ViewBag.CanCancelLost = rights.CanCancelLost;
            ViewBag.CanProducts = rights.CanProducts;
            ViewBag.CanSharing = rights.CanSharing;
            ViewBag.CanLiterature = rights.CanLiterature;





            return PartialView(@"~/Views/CRMS/Inquiry/PartialInquiryGridView.cshtml", GetInquirys(frmdate));
        }
        public IEnumerable GetInquirys(string frmdate)
        {

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);

            if (frmdate != "Ispageload")
            {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                var q = from d in dc.V_CRMInquiries
                        where Convert.ToString(d.Assign_To) == Userid || Convert.ToString(d.Owner_Id) == Userid
                        orderby d.CreateDate descending
                        select d;
                return q;
            }
            else
            {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                var q = from d in dc.V_CRMInquiries
                        where 1 == 0
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

        public ActionResult QualifyCRMInquirys(crmInquirys newcrmInquirysobj)
        {
            crmInquirys CRMInquirysobj = new crmInquirys();
            string output = "";
            output = CRMInquirysobj.QualifiedInquirys(newcrmInquirysobj);
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LostCRMInquirys(crmInquirys newcrmInquirysobj)
        {
            crmInquirys CRMInquirysobj = new crmInquirys();
            string output = "";
            output = CRMInquirysobj.LostInquirys(newcrmInquirysobj);
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectedSourceType(crmInquirys newcrmInquirysobj)
        {
            crmInquirys CRMInquirysobj = new crmInquirys();
            string output = "";
            output = CRMInquirysobj.SelectedSourceType(newcrmInquirysobj);
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportCRMInquirysList(int type)
        {


            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToPdf(GetEmployeeBatchGridViewSettings(), GetInquirys("1"));
                //break;
                case 2:
                    return GridViewExtension.ExportToXlsx(GetEmployeeBatchGridViewSettings(), GetInquirys("1"));
                //break;
                case 3:
                    return GridViewExtension.ExportToXls(GetEmployeeBatchGridViewSettings(), GetInquirys("1"));
                case 4:
                    return GridViewExtension.ExportToRtf(GetEmployeeBatchGridViewSettings(), GetInquirys("1"));
                case 5:
                    return GridViewExtension.ExportToCsv(GetEmployeeBatchGridViewSettings(), GetInquirys("1"));
                //break;

                default:
                    break;
            }

            return null;
        }
        private GridViewSettings GetEmployeeBatchGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "gridcrmInquiry";
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Inquiry";

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
                x.Caption = "Inquiry Source";
                x.FieldName = "InquirySourceName";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Inquiry Values";
                x.FieldName = "InquirySourceValue";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Rating";
                x.FieldName = "rat_InquiryRating";
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Status";
                x.FieldName = "Status";
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

        public ActionResult GetInquiryValues(string SearchKey, string ContactType)
        {
            List<crmInquiryValuesModel> listCust = new List<crmInquiryValuesModel>();
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
                            select new crmInquiryValuesModel()
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

            crmInquirys CRMInquirysobj = new crmInquirys();

            var querycrm_StatusDetails = from c in dcon.v_StatusDetails
                                         where c.Status_Code == "New"
                                         select c;
            CRMInquirysobj.Status_Details = querycrm_StatusDetails.ToList();
            return Json(CRMInquirysobj);
        }


        public JsonResult GetInquiryData(string MobileNo)
        {
            crmInquirys CRMInquirysobj = new crmInquirys();
            List<crmInquiryMobileDetails> objlst = new List<crmInquiryMobileDetails>();
            objlst = CRMInquirysobj.GetInquiryMobileDetails(MobileNo);
            return Json(objlst);
        }


        public JsonResult GetGroupData(string group_id)
        {
            DBEngine obj = new DBEngine();
            DataTable dt = obj.GetDataTable("select isnull(IsEmailMandatory,0) IsEmailMandatory,ISNULL(IsMobileMandatory,0) IsMobileMandatory,ISNULL(IsIndivisual,0) IsIndivisual,ISNULL(IsPANIndia,0) IsPANIndia from tbl_master_groupMaster where gpm_id=" + group_id);
            if (dt != null && dt.Rows.Count > 0)
                return Json(new { emailmandatory = Convert.ToString(dt.Rows[0]["IsEmailMandatory"]), mobilemandatory = Convert.ToString(dt.Rows[0]["IsMobileMandatory"]), IsIndivisual = Convert.ToString(dt.Rows[0]["IsIndivisual"]), IsPANIndia = Convert.ToString(dt.Rows[0]["IsPANIndia"]) });
            else
                return Json(new { emailmandatory = "0", mobilemandatory = "0", IsIndivisual = "0", IsPANIndia = "0" });

        }



        public ActionResult PopulateContactGrid()
        {
            //if (TempData["CONTACT_IDs"] != null)
            //{
            //    ViewBag.CONTACT_IDs = TempData["CONTACT_IDs"];
            //}
            return PartialView(@"~/Views/CRMS/Inquirys/_PartialConTact.cshtml", GetContactData());
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


        public ActionResult GetEntityData(string Entity_id)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);

            crmInquirys CRMLeadsobj = new crmInquirys();
            DataSet output = CRMLeadsobj.EditCRMLeadsInquiry(Entity_id);
            CustomerDetails clsObj = new CustomerDetails();
            CRMLeadsobj = APIHelperMethods.ToModel<crmInquirys>(output.Tables[0]);
            clsObj = APIHelperMethods.ToModel<CustomerDetails>(output.Tables[1]);
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

    }
}