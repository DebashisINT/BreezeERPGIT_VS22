using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using PMS.Models;
using PMS.Models.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace PMS.Controllers
{
    public class ExpenseBookingController : Controller
    {
        ExpenseBookingModel bl = new ExpenseBookingModel();
        UserRightsForPage rights = new UserRightsForPage();

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult ExpenseBookingView()
        {
            ExpenseBookingViewModel objList = new ExpenseBookingViewModel();
            List<Units> listUnits = new List<Units>();
            List<Projects> listProjects = new List<Projects>();
            List<Currency> listCurrency = new List<Currency>();
            List<ExpenseCategory> listExpenseCategory = new List<ExpenseCategory>();

            var datasetobj = bl.DropDownDetailForExpenseBooking();

            if (datasetobj.Tables[0].Rows.Count > 0)
            {
                Projects obj = new Projects();
                foreach (DataRow item in datasetobj.Tables[0].Rows)
                {
                    obj = new Projects();
                    obj.Proj_Id = Convert.ToString(item["Proj_Id"]);
                    obj.Proj_Name = Convert.ToString(item["Proj_Name"]);
                    listProjects.Add(obj);
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

            if (datasetobj.Tables[2].Rows.Count > 0)
            {
                Currency obj = new Currency();
                foreach (DataRow item in datasetobj.Tables[2].Rows)
                {
                    obj = new Currency();
                    obj.Currency_id = Convert.ToString(item["Currency_id"]);
                    obj.Currency_Name = Convert.ToString(item["Currency_Name"]);
                    listCurrency.Add(obj);
                }
            }

            if (datasetobj.Tables[3].Rows.Count > 0)
            {
                ExpenseCategory obj = new ExpenseCategory();
                foreach (DataRow item in datasetobj.Tables[3].Rows)
                {
                    obj = new ExpenseCategory();
                    obj.ExpenseCategory_id = Convert.ToString(item["ExpenseCategory_id"]);
                    obj.ExpenseCategory_Name = Convert.ToString(item["ExpenseCategory_Name"]);
                    listExpenseCategory.Add(obj);
                }
            }
            TempData["ExpenseBookingList"] = null;
            objList.ProjectsList = listProjects;
            objList.UnitsList = listUnits;
            objList.ExpenseCategoryList = listExpenseCategory;
            objList.CurrencyList = listCurrency;
            objList.Currency = "1";
            return View("~/Views/PMS/ExpenseBooking/ExpenseBookingView.cshtml", objList);
        }

        [HttpPost]
        public ActionResult GetProjectCode(string BranchId)
        {
            List<Projects> listStatues = new List<Projects>();
            var datasetobj = bl.DropDownDetailForProject(BranchId);
            if (datasetobj.Tables[0].Rows.Count > 0)
            {
                Projects obj = new Projects();
                foreach (DataRow item in datasetobj.Tables[0].Rows)
                {
                    obj = new Projects();
                    obj.Proj_Id = Convert.ToString(item["Proj_Id"]);
                    obj.Proj_Name = Convert.ToString(item["Proj_Name"]);
                    listStatues.Add(obj);
                }
            }
            return Json(listStatues, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SaveData(ExpenseBookingViewModel model)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            try
            {
                bl.AddEditExpenseBooking(model.Action_type, model.TransactionDate, model.BranchID, model.Projects, model.ExpensePurpose, model.ExpenseCategory,
                    model.Basis, model.Quantity, model.Price, model.Currency, model.Amount, model.SalesTaxAmount, model.ExternalComments, model.ExpenseBooking_Id, ref ReturnCode, ref ReturnMsg);
                if (ReturnMsg == "Success" && ReturnCode == 1)
                {
                    model.response_code = "Success";
                    model.response_msg = "Success";
                }
                else if (ReturnMsg != "Success" && ReturnCode == -10)
                {
                    model.response_code = "Error";
                    model.response_msg = ReturnMsg;
                }
                else if (ReturnMsg == "Update" && ReturnCode == 1)
                {
                    model.response_code = "Update";
                    model.response_msg = "Update";
                }
                else
                {
                    model.response_code = "Error";
                    model.response_msg = "Please try again later";
                }
            }

            catch (Exception ex)
            {
                model.response_code = "CatchError";
                model.response_msg = "Please try again later";
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExpenseBookingPartial()
        {
            try
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ExpenseBookingView", "ExpenseBooking");
                ViewBag.CanExport = rights.CanExport;
                ViewBag.CanView = rights.CanView;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
                ViewBag.Verified = rights.Verified;

                //if (BranchID == "0")
                //{
                //    BranchID = Convert.ToString(Session["userbranchHierarchy"]);
                //}
                List<ExpenseBookingView> omel = new List<ExpenseBookingView>();
                DataTable dt = new DataTable();
                //dt = bl.GetExpenseBookingList(BranchID, FromDate, ToDate);
                dt = (DataTable)TempData["ExpenseBookingList"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    omel = APIHelperMethods.ToModelList<ExpenseBookingView>(dt);
                    TempData["ExpenseBookingList"] = dt;
                    TempData.Keep();
                }
                else
                {
                    TempData["ExpenseBookingList"] = null;
                    TempData.Keep();
                }
                return PartialView("~/Views/PMS/ExpenseBooking/_PartialExpenseBookingListGrid.cshtml", omel);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        [HttpPost]
        public JsonResult ViewDataset(string ExpenseBooking_id)
        {
            ExpenseBookingViewModel objList = new ExpenseBookingViewModel();
            DataTable dt = bl.ViewExpenseBooking(ExpenseBooking_id);
            if (dt != null && dt.Rows.Count > 0)
            {
                objList.ExpenseBooking_Id = dt.Rows[0]["ExpenseBooking_Id"].ToString();
                objList.Projects = dt.Rows[0]["Project_Id"].ToString();
                objList.BranchID = dt.Rows[0]["Branch_Id"].ToString();
                objList.Currency = dt.Rows[0]["Currency"].ToString();
                objList.ExpenseCategory = dt.Rows[0]["ExpenseCategory_Id"].ToString();
                objList.ExpensePurpose = dt.Rows[0]["ExpensePurpose"].ToString();
                objList.Basis = dt.Rows[0]["Basis"].ToString();
                objList.Quantity = dt.Rows[0]["Quantity"].ToString();
                objList.Price = dt.Rows[0]["Price"].ToString();
                objList.Amount = dt.Rows[0]["Amount"].ToString();
                objList.SalesTaxAmount = dt.Rows[0]["SalesTaxAmount"].ToString();
                objList.ExternalComments = dt.Rows[0]["ExternalComments"].ToString();
                objList.Transaction_Date = Convert.ToDateTime(dt.Rows[0]["TransactionDate"].ToString());
            }
            return Json(objList);
        }

        [HttpPost]
        public JsonResult DeleteData(string ExpenseBooking_id)
        {
            String ReturnMsg = "";
            string returns = "Data not Deleted please try again later.";
            //int val = 
            bl.DeleteExpenseBooking(ExpenseBooking_id, ref ReturnMsg);
            if (ReturnMsg == "Success")
            {
                returns = "Deleted Successfully.";
            }
            else if (ReturnMsg == "Used in Other Modules. cannot Delete.")
            {
                returns = "Used in Other Modules. Cannot Delete.";
            }
            else
            {
                returns = ReturnMsg;
            }
            return Json(returns);
        }

        [HttpPost]
        public ActionResult ShowExpenseBookingPartial(ExpenseBookingPartial model)
        {
            try
            {
                if (model.BranchID == "0")
                {
                    model.BranchID = Convert.ToString(Session["userbranchHierarchy"]);
                }

                DataTable dt = new DataTable();
                dt = bl.GetExpenseBookingList(model.BranchID, model.FromDate, model.ToDate);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //omel = APIHelperMethods.ToModelList<ExpenseBookingView>(dt);
                    TempData["ExpenseBookingList"] = dt;
                    TempData.Keep();
                }
                else
                {
                    TempData["ExpenseBookingList"] = null;
                    TempData.Keep();
                }
                return Json("Sucess", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public JsonResult PopulateBranchByHierchy()
        {
            List<UnitList> list = new List<UnitList>();
            string userbranchHierachy = Convert.ToString(Session["userbranchHierarchy"]);
            DataTable branchtable = bl.getBranchListByHierchy(userbranchHierachy);
            if (branchtable.Rows.Count > 0)
            {
                UnitList obj = new UnitList();
                foreach (DataRow item in branchtable.Rows)
                {
                    obj = new UnitList();
                    obj.ID = Convert.ToString(item["branch_id"]);
                    obj.Name = Convert.ToString(item["branch_description"]);
                    list.Add(obj);
                }
            }
            return Json(list);
        }

        //public IEnumerable GetFormulaList(int is_pageload, string FromDate, string ToDate, string ListBranch, string UserwiseTimeSheet)
        //{

        //    string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        //    if (is_pageload != 0)
        //    {
        //        PMS.Models.DataContext.PMSDataClassesDataContext dc = new PMS.Models.DataContext.PMSDataClassesDataContext(connectionString);
        //        if (UserwiseTimeSheet == "No")
        //        {
        //            if (Convert.ToInt16(ListBranch) > 0)
        //            {
        //                var q = from d in dc.V_PMSExpenseBookings
        //                        where
        //                        d.CREATED_ON >= Convert.ToDateTime(FromDate) && d.CREATED_ON <= Convert.ToDateTime(ToDate)
        //                        && d.branch_id == Convert.ToInt16(ListBranch)
        //                        orderby d.CREATED_ON descending
        //                        select d;
        //                return q;
        //            }
        //            else
        //            {
        //                var q = from d in dc.V_PMSExpenseBookings
        //                        where d.CREATED_ON >= Convert.ToDateTime(FromDate) &&
        //                              d.CREATED_ON <= Convert.ToDateTime(ToDate)
        //                        //&& d.branch_id = Convert.ToInt16(ListBranch)
        //                        orderby d.CREATED_ON descending
        //                        select d;
        //                return q;
        //            }
        //        }
        //        else
        //        {
        //            if (Convert.ToInt16(ListBranch) > 0)
        //            {
        //                var q = from d in dc.V_PMSExpenseBookings
        //                        where
        //                        d.CREATED_ON >= Convert.ToDateTime(FromDate) && d.CREATED_ON <= Convert.ToDateTime(ToDate)
        //                        && d.branch_id == Convert.ToInt16(ListBranch)
        //                        && d.Create_By == Convert.ToInt64(Session["userid"])
        //                        orderby d.CREATED_ON descending
        //                        select d;
        //                return q;
        //            }
        //            else
        //            {
        //                var q = from d in dc.V_PMSExpenseBookings
        //                        where d.CREATED_ON >= Convert.ToDateTime(FromDate) &&
        //                              d.CREATED_ON <= Convert.ToDateTime(ToDate)
        //                                && d.Create_By == Convert.ToInt64(Session["userid"])
        //                        //&& d.branch_id = Convert.ToInt16(ListBranch)
        //                        orderby d.CREATED_ON descending
        //                        select d;
        //                return q;
        //            }
        //        }

        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public ActionResult ExportExpenseBookingGridList(int type)
        {
            ViewData["ExpenseBookingList"] = TempData["ExpenseBookingList"];

            TempData.Keep();
            DataTable dt = (DataTable)TempData["ExpenseBookingList"];
            if (ViewData["ExpenseBookingList"] != null && dt.Rows.Count > 0)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetExpenseBookingGridView(ViewData["ExpenseBookingList"]), ViewData["ExpenseBookingList"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetExpenseBookingGridView(ViewData["ExpenseBookingList"]), ViewData["ExpenseBookingList"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetExpenseBookingGridView(ViewData["ExpenseBookingList"]), ViewData["ExpenseBookingList"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetExpenseBookingGridView(ViewData["ExpenseBookingList"]), ViewData["ExpenseBookingList"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetExpenseBookingGridView(ViewData["ExpenseBookingList"]), ViewData["ExpenseBookingList"]);
                    default:
                        break;
                }
                return null;
            }
            else
            {
                return this.RedirectToAction("ExpenseBookingView", "ExpenseBooking");
            }
        }
        private GridViewSettings GetExpenseBookingGridView(object datatable)
        {
            var settings = new GridViewSettings();
            settings.Name = "ExpenseBooking";
            //    settings.CallbackRouteValues = new { Controller = "Report", Action = "GetRegisterreporttatusList" };
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Expense Booking";


            settings.Columns.Add(x =>
            {
                x.FieldName = "TransactionDate";
                x.Caption = "Transaction Date";
                x.VisibleIndex = 1;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);
                x.ColumnType = DevExpress.Web.Mvc.MVCxGridViewColumnType.DateEdit;
                x.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
                (x.PropertiesEdit as DevExpress.Web.DateEditProperties).EditFormatString = "dd-MM-yyyy";
                x.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                x.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "branch_description";
                x.Caption = "Unit";
                x.VisibleIndex = 2;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);
                x.ColumnType = DevExpress.Web.Mvc.MVCxGridViewColumnType.TextBox;
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "ExpensePurpose";
                x.Caption = "Expense Purpose";
                x.VisibleIndex = 3;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
                x.ColumnType = DevExpress.Web.Mvc.MVCxGridViewColumnType.TextBox;
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "Basis";
                x.Caption = "Basis";
                x.VisibleIndex = 4;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);
                x.ColumnType = DevExpress.Web.Mvc.MVCxGridViewColumnType.TextBox;
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "Quantity";
                x.Caption = "Quantity";
                x.VisibleIndex = 5;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(80);
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "Price";
                x.Caption = "Price";
                x.VisibleIndex = 6;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "Amount";
                x.Caption = "Amount";
                x.VisibleIndex = 7;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "SalesTaxAmount";
                x.Caption = "Sales Tax...";
                x.VisibleIndex = 8;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "Currency_Name";
                x.Caption = "Currency";
                x.VisibleIndex = 9;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "Proj_Name";
                x.Caption = "Project";
                x.VisibleIndex = 10;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "Expense_Name";
                x.Caption = "Expense Category";
                x.VisibleIndex = 11;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(200);
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "ExternalComments";
                x.Caption = "Comments";
                x.VisibleIndex = 12;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(250);
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "CREATE_USER";
                x.Caption = "Entered By";
                x.VisibleIndex = 13;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(150);
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "CREATED_ON";
                x.Caption = "Entered On";
                x.VisibleIndex = 14;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);
                x.ColumnType = DevExpress.Web.Mvc.MVCxGridViewColumnType.DateEdit;
                x.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
                (x.PropertiesEdit as DevExpress.Web.DateEditProperties).EditFormatString = "dd-MM-yyyy";
                x.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                x.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            });

            settings.Columns.Add(x =>
            {
                x.FieldName = "UPDATE_USER";
                x.Caption = "Modified By";
                x.VisibleIndex = 15;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(150);
                //  x.Width = System.Web.UI.WebControls.Unit.Percentage(30);
            });


            settings.Columns.Add(x =>
            {
                x.FieldName = "UPDATED_ON";
                x.Caption = "Modified On";
                x.VisibleIndex = 16;
                x.Width = System.Web.UI.WebControls.Unit.Pixel(100);
                x.ColumnType = DevExpress.Web.Mvc.MVCxGridViewColumnType.DateEdit;
                x.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy hh:mm:ss";
                (x.PropertiesEdit as DevExpress.Web.DateEditProperties).EditFormatString = "dd-MM-yyyy hh:mm:ss";
                x.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                x.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;

            });


            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }

        public class ExpenseBookingPartial
        {
            public String BranchID { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
        }
    }
}