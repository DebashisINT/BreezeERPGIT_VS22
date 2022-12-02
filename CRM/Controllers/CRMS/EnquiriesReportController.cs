
using CRM.Models;
using CRM.Repostiory.EnquiriesReport;
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



namespace CRM.Controllers.CRMS
{
   [CRM.Models.Attributes.SessionTimeout]
    public class EnquiriesReportController : Controller
    {
        //
        // GET: /EnquiriesReport/
        private IEnquiriesReport _enquiriesreport;
        UserRightsForPage rights = new UserRightsForPage();
        public ActionResult Index()
        {
            //Rev Subhra 12-04-2019
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "EnquiriesReport");
            //End of Rev Subhra 12-04-2019
            List<EnquriesFrom> listenquriesfrom = new List<EnquriesFrom>();
            EnquiriesReport enqdet = new EnquiriesReport();
            EnquriesFrom enquriesfrom = new EnquriesFrom();
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

            enqdet.enqfrom = listenquriesfrom;
            enqdet.Date = DateTime.Now;
            //Rev Subhra 12-04-2019
            enqdet.UserRightsForPage = rights;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanRestore = rights.CanRestore;
            //End of Rev Subhra 12-04-2019
            return View("~/Views/CRMS/Report/Enquiries/Index.cshtml", enqdet);
        }
        public PartialViewResult PartialEnquiriesReportGrid(EnquiriesReport modelenquiriesdet)
        {
            if (modelenquiriesdet.is_pageload != 0)
            {
                //Rev Subhra 12-04-2019
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "EnquiriesReport");
                ViewBag.CanDelete = rights.CanDelete;
                ViewBag.CanRestore = rights.CanRestore;
                //End of Rev Subhra 12-04-2019
                string datfrmat = modelenquiriesdet.FromDate.Split('-')[2] + '-' + modelenquiriesdet.FromDate.Split('-')[1] + '-' + modelenquiriesdet.FromDate.Split('-')[0];
                string dattoat = modelenquiriesdet.ToDate.Split('-')[2] + '-' + modelenquiriesdet.ToDate.Split('-')[1] + '-' + modelenquiriesdet.ToDate.Split('-')[0];
                _enquiriesreport = new EnquiriesReportRepo();
                _enquiriesreport.GetEnqListing(modelenquiriesdet.EnquiriesFrom, datfrmat, dattoat);
            }
            return PartialView("~/Views/CRMS/Report/Enquiries/PartialEnquiriesReportGrid.cshtml", GetEnquiriesList(modelenquiriesdet.is_pageload));
        }
        public IEnumerable GetEnquiriesList(int is_pageload)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string Userid = Convert.ToString(Session["userid"]);

            if (is_pageload != 0)
            {
                CRM.Models.DataContext.CRMClassDataContext DC = new CRM.Models.DataContext.CRMClassDataContext(connectionString);
                var q = from d in DC.ENQUIRIES_REPORTs
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

        public ActionResult ExportEnquiriesList(int type)
        {


            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToPdf(GetEmployeeBatchGridViewSettings(), GetEnquiriesList(1));
                //break;
                case 2:
                    return GridViewExtension.ExportToXlsx(GetEmployeeBatchGridViewSettings(), GetEnquiriesList(1));
                //break;
                case 3:
                    return GridViewExtension.ExportToXls(GetEmployeeBatchGridViewSettings(), GetEnquiriesList(1));
                case 4:
                    return GridViewExtension.ExportToRtf(GetEmployeeBatchGridViewSettings(), GetEnquiriesList(1));
                case 5:
                    return GridViewExtension.ExportToCsv(GetEmployeeBatchGridViewSettings(), GetEnquiriesList(1));
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
            settings.CallbackRouteValues = new { Controller = "PartialEnquiriesReportGrid", Action = "EnquiriesReport" };
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Enquiries List";

            settings.Columns.Add(column =>
            {
                column.Caption = "Date";
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
                column.Caption = "Ph No.";
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

          

            settings.Columns.Add(column =>
            {
                column.FieldName = "SOURCE";
                column.Caption = "Source";
            });

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
                column.Caption = "Verify By";
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

            //Rev 1.0
            settings.Columns.Add(column =>
            {
                column.FieldName = "MARK_AS_DELETED";
                column.Caption = "Mark as Deleted";
            });
            //End of Rev 1.0
            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }
     
       //Rev Subhra 11-04-2019
        [HttpPost]
        public JsonResult EnquiriesRestore(string ActionType, string Uniqueid)
        {

            string output_msg = string.Empty;
            int ReturnCode = 0;
            _enquiriesreport = new EnquiriesReportRepo();
            Msg _msg = new Msg();
            try
            {
                output_msg = _enquiriesreport.Restore(ActionType, Uniqueid, ref ReturnCode);
                if (output_msg == "Restored" && ReturnCode == 1)
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

        [HttpPost]
        public JsonResult EnquiriesPDelete(string ActionType, string Uniqueid)
        {

            string output_msg = string.Empty;
            int ReturnCode = 0;
            _enquiriesreport = new EnquiriesReportRepo();
            Msg _msg = new Msg();
            try
            {
                output_msg = _enquiriesreport.PermanentDelete(ActionType, Uniqueid, ref ReturnCode);
                if (output_msg == "Restored" && ReturnCode == 1)
                {
                    _msg.response_code = "Success";
                    _msg.response_msg = "Success";
                }
                //Rev Subhra 06-05-2019
                else if (output_msg == "PDeleted" && ReturnCode == 1)
                {
                    _msg.response_code = "Success";
                    _msg.response_msg = "Success";
                }
                //End of Rev 
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
       //End of Rev 11-04-2019
	}
}