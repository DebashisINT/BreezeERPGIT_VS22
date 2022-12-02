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
    public class CampaignController : Controller
    {

        UserRightsForPage rights = new UserRightsForPage();
        //
        // GET: /Campaign/
        public ActionResult Index()
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "Campaign");

            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanExport = rights.CanExport;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CRMClassDataContext dcon = new CRMClassDataContext(connectionString);
            CRMCampaign CRMCampaignobj = new CRMCampaign();

            List<crm_CampaignType> crm_CampaignTypelist = new List<Models.DataContext.crm_CampaignType>();
            crm_CampaignTypelist.Add(new crm_CampaignType { Id = 0, Campaign_Code = "Select" });
            var querycrm_CampaignTypes = (from u in crm_CampaignTypelist
                                          select u).Union(from c in dcon.crm_CampaignTypes
                                                          select c);
            CRMCampaignobj.Campaign_Type = querycrm_CampaignTypes.ToList(); 
            List<crm_StatusDetail> lstcrm_StatusDetail = new List<crm_StatusDetail>();
            lstcrm_StatusDetail.Add(new crm_StatusDetail { Id = 0, Status_Code = "Select" });
            var querycrm_StatusDetails = (from u in lstcrm_StatusDetail
                                          select u).Union(from c in dcon.crm_StatusDetails
                                         select c);

            var querycrm_users = (from c in dcon.V_UserLIsts
                                         select c);
            List<v_ContactSource> lstv_ContactSource = new List<v_ContactSource>();
            lstv_ContactSource.Add(new v_ContactSource { SourceID = 0,  cntsrc_sourceType= "Select" });
            var querycrm_contactsource = (from u in lstv_ContactSource
                                          select u).Union(from c in dcon.v_ContactSources
                                         select c);
            CRMCampaignobj.ContactSource = querycrm_contactsource.ToList();
            CRMCampaignobj.Users = querycrm_users.ToList();
            CRMCampaignobj.Status_Details = querycrm_StatusDetails.ToList();
            CRMCampaignobj.WonerID = Convert.ToInt64(Session["userid"]);
            CRMCampaignobj.AssignedID = Convert.ToInt64(Session["userid"]);
            CRMCampaignobj.Woner = Convert.ToString(System.Web.HttpContext.Current.Session["UserName"]);
            return View(@"~/Views/CRMS/Campaign/Dashboard.cshtml", CRMCampaignobj);
        }


        public ActionResult SaveCampaign(CRMCampaign newCRMCampaignobj)
        {
            CRMCampaign CRMCampaignobj = new CRMCampaign();
            string output = CRMCampaignobj.SaveCampaign(newCRMCampaignobj);
            return Json(output, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PopulateGrid(string frmdate)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "Campaign");
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;


            ViewBag.CanCreateActivity = rights.CanSalesActivity;
            ViewBag.CanProducts = rights.CanProducts;
            ViewBag.CanSharing = rights.CanSharing;
            ViewBag.CanLiterature = rights.CanLiterature;


            return PartialView(@"~/Views/CRMS/Campaign/PartialCampaignGrid.cshtml", GetSalesSummary(frmdate));
        }

        public IEnumerable GetSalesSummary(string frmdate)
        {

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);

            if (frmdate != "Ispageload")
            {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                // Mantis Issue 24050
                //var q = from d in dc.v_CRMCampaigns

                //        select d;
                //return q;

                BusinessLogicLayer.CommonBL ComBL = new BusinessLogicLayer.CommonBL();
                string ShowCampaignDataForAllUsers = ComBL.GetSystemSettingsResult("ShowCampaignDataForAllUsers");

                if (ShowCampaignDataForAllUsers.ToUpper() == "NO")
                {
                    var q = from d in dc.v_CRMCampaigns
                            where Convert.ToString(d.Assign_Id) == Userid || Convert.ToString(d.Owner_Id) == Userid
                            select d;
                    return q;
                }
                else
                {
                    var q = from d in dc.v_CRMCampaigns
                            select d;
                    return q;
                }
                // End of Rev Mantis issue 24050
            }
            else
            {
                CRMClassDataContext dc = new CRMClassDataContext(connectionString);
                var q = from d in dc.v_CRMCampaigns where 1==0

                        select d;
                return q;
            }



        }


        public ActionResult EditCampaign(CRMCampaign newCRMCampaignobj)
        {
            CRMCampaign CRMCampaignobj = new CRMCampaign();
            DataSet output = CRMCampaignobj.EditCampaign(newCRMCampaignobj);
            CRMCampaignobj = APIHelperMethods.ToModel<CRMCampaign>(output.Tables[0]);
            return Json(CRMCampaignobj);
        }

        public ActionResult DeleteCampaign(CRMCampaign newCRMCampaignobj)
        {
            CRMCampaign CRMCampaignobj = new CRMCampaign();
            string output = CRMCampaignobj.DeleteCampaign(newCRMCampaignobj);
            return Json(output);
        }


        public ActionResult ExportCRmCampaignList(int type)
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
            settings.Name = "gridcrmCampaign";
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Campaign";

            settings.Columns.Add(x =>
            {
                x.Caption = "ID";
                x.FieldName = "ID";
                x.Visible = false;
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Campaign Name";
                x.FieldName = "Campaign_Name";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
                x.FixedStyle = GridViewColumnFixedStyle.Left;
            });


            settings.Columns.Add(x =>
            {
                x.Caption = "Campaign Code";
                x.FieldName = "Campaign_Code";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
                x.FixedStyle = GridViewColumnFixedStyle.Left;
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Campaign Status";
                x.FieldName = "Campaign_Status";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });


            settings.Columns.Add(x =>
            {
                x.Caption = "Type";
                x.FieldName = "CAMPAIGN_TYPE";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Assign To";
                x.FieldName = "Assign_To";
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Actual Start Date";
                x.FieldName = "Expected_Start";
                x.ColumnType = DevExpress.Web.Mvc.MVCxGridViewColumnType.DateEdit;
                //x.DisplayFormatString = "dd-MM-yyyy";           
                (x.PropertiesEdit as DateEditProperties).DisplayFormatString = "dd-MM-yyyy";
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Actual End Date";
                x.FieldName = "Expected_End";
                x.ColumnType = DevExpress.Web.Mvc.MVCxGridViewColumnType.DateEdit;
                //x.DisplayFormatString = "dd-MM-yyyy";
                (x.PropertiesEdit as DateEditProperties).DisplayFormatString = "dd-MM-yyyy";
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Created By";
                x.FieldName = "Created_by";
                x.ColumnType = MVCxGridViewColumnType.TextBox;

                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Created On";
                x.FieldName = "Created_On";
                x.ColumnType = DevExpress.Web.Mvc.MVCxGridViewColumnType.DateEdit;
                //x.DisplayFormatString = "dd-MM-yyyy";           
                (x.PropertiesEdit as DateEditProperties).DisplayFormatString = "dd-MM-yyyy";
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });


            settings.Columns.Add(x =>
            {
                x.Caption = "Updated By";
                x.FieldName = "Updated_By";
                x.ColumnType = MVCxGridViewColumnType.TextBox;

                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });

            settings.Columns.Add(x =>
            {
                x.Caption = "Updated On";
                x.FieldName = "Updated_On";
                x.ColumnType = DevExpress.Web.Mvc.MVCxGridViewColumnType.DateEdit;
                //x.DisplayFormatString = "dd-MM-yyyy";           
                (x.PropertiesEdit as DateEditProperties).DisplayFormatString = "dd-MM-yyyy";
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });
            settings.Columns.Add(x =>
            {
                x.Caption = "Action";
                x.Width = 0;
                x.ColumnType = MVCxGridViewColumnType.TextBox;
                x.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });


            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }

        


    }
}