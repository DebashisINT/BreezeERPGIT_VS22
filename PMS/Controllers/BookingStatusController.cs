using BusinessLogicLayer.PMS;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using PMS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;


namespace PMS.Controllers
{
    public class BookingStatusController : Controller
    {
        UserRightsForPage rights = new UserRightsForPage();
        BookingStatusBL bl = new BookingStatusBL();
        public ActionResult BookingStatusIndex()
        {
            BookingStatusViewModel objPO = new BookingStatusViewModel();
            List<Types> listTypes = new List<Types>();
            List<Units> listUnits = new List<Units>();
            List<Statues> listStatues = new List<Statues>();
            var datasetobj = bl.DropDownDetailForRole();
            if (datasetobj.Tables[2].Rows.Count > 0)
            {
                Types obj = new Types();
                foreach (DataRow item in datasetobj.Tables[2].Rows)
                {
                    obj = new Types();
                    obj.TYPEID = Convert.ToString(item["TYPEID"]);
                    obj.TYPENAME = Convert.ToString(item["TYPENAME"]);
                    listTypes.Add(obj);
                }
            }
            if (datasetobj.Tables[1].Rows.Count > 0)
            {
                Units obj = new Units();
                foreach (DataRow item in datasetobj.Tables[1].Rows)
                {
                    obj = new Units();
                    obj.branch_id = Convert.ToString(item["branch_id"]);
                    obj.branch_description = Convert.ToString(item["branch_description"]);
                    listUnits.Add(obj);
                }
            }
            if (datasetobj.Tables[0].Rows.Count > 0)
            {
                Statues obj = new Statues();
                foreach (DataRow item in datasetobj.Tables[0].Rows)
                {
                    obj = new Statues();
                    obj.STATUS_ID = Convert.ToString(item["STATUS_ID"]);
                    obj.STATUS_NAME = Convert.ToString(item["STATUS_NAME"]);
                    listStatues.Add(obj);
                }
            }

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/BookingStatusIndex", "BookingStatus");
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
            ViewBag.Verified = rights.Verified;

            objPO.BOOKING_TYPEList = listTypes;
            objPO.STATUSList = listStatues;
            objPO.BranchList = listUnits;
            return View("~/Views/PMS/BookingStatus/BookingStatusIndex.cshtml", objPO);
        }

        [HttpPost]
        public JsonResult SaveData(BookingStatusViewModel booking)
        {
            string returns = "Data not save please try again later.";
            string valu = bl.SaveBookingData(booking.BOOKING_ID, booking.BOOKING_NAME, booking.BOOKING_TYPE, booking.STATUS, booking.DESCRIPTION, booking.BRANCH, booking.COLOR);
            if (valu != "")
            {
                returns = valu;
            }
            return Json(returns);
        }

        public ActionResult GetBookingStatusPartial()
        {
            try
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/BookingStatusIndex", "BookingStatus");
                ViewBag.CanExport = rights.CanExport;
                ViewBag.CanView = rights.CanView;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
                ViewBag.Verified = rights.Verified;

                List<bookingList> omel = new List<bookingList>();

                DataTable dt = new DataTable();

                dt = bl.GetBookingList();

                if (dt != null)
                {
                    omel = APIHelperMethods.ToModelList<bookingList>(dt);
                    TempData["ExportBookingStatus"] = omel;
                    TempData.Keep();
                }
                else
                {
                    TempData["ExportBookingStatus"] = null;
                    TempData.Keep();
                }
                return PartialView("~/Views/PMS/BookingStatus/PartialBookingStatusGrid.cshtml", omel);
            }
            catch
            {
                //   return Redirect("~/OMS/Signoff.aspx");
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        [HttpPost]
        public JsonResult ViewDataShow(string BOOKING_ID)
        {
            BookingStatusViewModel viewMDL = new BookingStatusViewModel();
            DataTable Roledt = bl.ViewTranscatin(BOOKING_ID);
            if (Roledt != null && Roledt.Rows.Count > 0)
            {
                viewMDL.BOOKING_NAME = Roledt.Rows[0]["BOOKING_NAME"].ToString();
                viewMDL.DESCRIPTION = Roledt.Rows[0]["DESCRIPTION"].ToString();
                viewMDL.BOOKING_TYPE = Roledt.Rows[0]["BOOKING_TYPE"].ToString();
                viewMDL.STATUS = Roledt.Rows[0]["STATUS"].ToString();
                viewMDL.BRANCH = Roledt.Rows[0]["BRANCH"].ToString();
                viewMDL.COLOR = Roledt.Rows[0]["COLOR"].ToString();

            }
            return Json(viewMDL);
        }

        [HttpPost]
        public JsonResult DeleteData(string BOOKING_ID)
        {
            string returns = "Not Deleted please try again later.";
            int val = bl.DeleteBooking(BOOKING_ID);
            if (val > 0)
            {
                returns = "Deleted Successfully.";
            }
            return Json(returns);
        }

        [HttpPost]
        public ActionResult GetStatus(string TypeId)
        {
            BookingStatusViewModel objPO = new BookingStatusViewModel();
            List<Statues> listStatues = new List<Statues>();
            var datasetobj = bl.DropDownDetailForStatus(TypeId);
            if (datasetobj.Tables[0].Rows.Count > 0)
            {
                Statues obj = new Statues();
                foreach (DataRow item in datasetobj.Tables[0].Rows)
                {
                    obj = new Statues();
                    obj.STATUS_ID = Convert.ToString(item["STATUS_ID"]);
                    obj.STATUS_NAME = Convert.ToString(item["STATUS_NAME"]);
                    listStatues.Add(obj);
                }
            }
            return Json(listStatues, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ExportBookingStatuslist(int type)
        {
            // List<AttendancerecordModel> model = new List<AttendancerecordModel>();
            ViewData["ExportBookingStatus"] = TempData["ExportBookingStatus"];
            TempData.Keep();

            if (ViewData["ExportBookingStatus"] != null)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetBookingGridViewSettings(), ViewData["ExportBookingStatus"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetBookingGridViewSettings(), ViewData["ExportBookingStatus"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetBookingGridViewSettings(), ViewData["ExportBookingStatus"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetBookingGridViewSettings(), ViewData["ExportBookingStatus"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetBookingGridViewSettings(), ViewData["ExportBookingStatus"]);
                    default:
                        break;
                }
            }
            //TempData["Exportcounterist"] = TempData["Exportcounterist"];
            //TempData.Keep();
            return null;
        }

        private GridViewSettings GetBookingGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "Booking Status";
            // settings.CallbackRouteValues = new { Controller = "Employee", Action = "ExportEmployee" };
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Booking Status";

            settings.Columns.Add(column =>
            {
                column.Caption = "Name";
                column.FieldName = "BOOKING_NAME";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Type";
                column.FieldName = "TYPENAME";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Status";
                column.FieldName = "STATUS_NAME";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Description";
                column.FieldName = "DESCRIPTION";

            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Unit";
                column.FieldName = "BRANCH_NAME";

            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Entered By";
                column.FieldName = "CREATE_NAME";
            });


            settings.Columns.Add(column =>
            {
                column.Caption = "Entered On";
                column.FieldName = "CREATE_DATE";
                column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Modified By";
                column.FieldName = "UPDATE_NAME";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Modified On";
                column.FieldName = "UPDATE_DATE";
                column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy hh:mm:ss";
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