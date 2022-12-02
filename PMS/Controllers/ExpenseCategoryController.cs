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
    public class ExpenseCategoryController : Controller
    {
        UserRightsForPage rights = new UserRightsForPage();
        ExpenseCategoryBL bl = new ExpenseCategoryBL();

        public ActionResult ExpenseCategoryIndex()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ExpenseCategoryIndex", "ExpenseCategory");
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
            ViewBag.Verified = rights.Verified;

            ExpenseCategoryViewModel objPO = new ExpenseCategoryViewModel();
            List<ExpenseTypes> ExpenseTypesList = new List<ExpenseTypes>();
            List<Units> listUnits = new List<Units>();
            List<TransactionCategorys> TransactionCategoryss = new List<TransactionCategorys>();
            List<ReceiptReqs> ReceiptReqss = new List<ReceiptReqs>();
            var datasetobj = bl.DropDownDetailForExpe();
            if (datasetobj.Tables[0].Rows.Count > 0)
            {
                ReceiptReqs obj = new ReceiptReqs();
                foreach (DataRow item in datasetobj.Tables[0].Rows)
                {
                    obj = new ReceiptReqs();
                    obj.ReceiptRequiredID = Convert.ToString(item["ReceiptRequiredID"]);
                    obj.ReceiptRequiredName = Convert.ToString(item["ReceiptRequiredName"]);
                    ReceiptReqss.Add(obj);
                }
            }
            if (datasetobj.Tables[1].Rows.Count > 0)
            {
                ExpenseTypes obj = new ExpenseTypes();
                foreach (DataRow item in datasetobj.Tables[1].Rows)
                {
                    obj = new ExpenseTypes();
                    obj.Expense_TypeID = Convert.ToString(item["Expense_TypeID"]);
                    obj.Expense_Type_Name = Convert.ToString(item["Expense_Type_Name"]);
                    ExpenseTypesList.Add(obj);
                }
            }
            if (datasetobj.Tables[2].Rows.Count > 0)
            {
                Units obj = new Units();
                foreach (DataRow item in datasetobj.Tables[2].Rows)
                {
                    obj = new Units();
                    obj.branch_id = Convert.ToString(item["branch_id"]);
                    obj.branch_description = Convert.ToString(item["branch_description"]);
                    listUnits.Add(obj);
                }
            }
            if (datasetobj.Tables[3].Rows.Count > 0)
            {
                TransactionCategorys obj = new TransactionCategorys();
                foreach (DataRow item in datasetobj.Tables[3].Rows)
                {
                    obj = new TransactionCategorys();
                    obj.TRANS_ID = Convert.ToString(item["TRANS_ID"]);
                    obj.TRANS_NAME = Convert.ToString(item["TRANS_NAME"]);
                    TransactionCategoryss.Add(obj);
                }
            }

            objPO.ExpenseTypeList = ExpenseTypesList;
            objPO.ReceiptReqList = ReceiptReqss;
            objPO.BranchList = listUnits;
            objPO.TransactionCategoryList = TransactionCategoryss;
            return View("~/Views/PMS/ExpenseCategory/ExpenseCategoryIndex.cshtml", objPO);
        }

        [HttpPost]
        public JsonResult SaveData(ExpenseCategoryViewModel Expenc)
        {
            string returns = "Data not save please try again later.";
            string valu = bl.SaveExpenseData(Expenc.ExpenseID, Expenc.Expense_Name, Expenc.Expense_Type, Expenc.TransactionCategory, Expenc.ReceiptReq, Expenc.BRANCH);
            if (valu == "Data save")
            {
                returns = "Saved Successfully.";
            }
            return Json(returns);
        }

        public ActionResult GetExpenceStatusPartial()
        {
            try
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ExpenseCategoryIndex", "ExpenseCategory");
                ViewBag.CanView = rights.CanView;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
                ViewBag.Verified = rights.Verified;

                List<ExpenseCategoryList> omel = new List<ExpenseCategoryList>();

                DataTable dt = new DataTable();

                dt = bl.GetExpenseList();

                if (dt.Rows.Count > 0)
                {
                    omel = APIHelperMethods.ToModelList<ExpenseCategoryList>(dt);
                    TempData["ExportExpenceStatus"] = omel;
                    TempData.Keep();
                }
                else
                {
                    TempData["ExportExpenceStatus"] = null;
                    TempData.Keep();
                }
                return PartialView("~/Views/PMS/ExpenseCategory/PartialExpenceStatusGrid.cshtml", omel);
            }
            catch
            {
                //   return Redirect("~/OMS/Signoff.aspx");
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        [HttpPost]
        public JsonResult ViewDataShow(string ExpenseID)
        {
            ExpenseCategoryViewModel viewMDL = new ExpenseCategoryViewModel();
            DataTable Expdt = bl.ViewExpense(ExpenseID);
            if (Expdt != null && Expdt.Rows.Count > 0)
            {
                viewMDL.Expense_Name = Expdt.Rows[0]["Expense_Name"].ToString();
                viewMDL.Expense_Type = Expdt.Rows[0]["Expense_Type"].ToString();
                viewMDL.TransactionCategory = Expdt.Rows[0]["TransactionCategory"].ToString();
                viewMDL.ReceiptReq = Expdt.Rows[0]["ReceiptReq"].ToString();
                viewMDL.BRANCH = Expdt.Rows[0]["BRANCH"].ToString();
                
            }
            return Json(viewMDL);
        }

        [HttpPost]
        public JsonResult DeleteData(string ExpenseID)
        {
            string returns = "Data not Deleted please try again later.";
            int val = bl.DeleteExpense(ExpenseID);
            if (val > 0)
            {
                returns = "Deleted Successfully.";
            }
            return Json(returns);
        }

        public ActionResult ExportExpenseCategorylist(int type)
        {
            // List<AttendancerecordModel> model = new List<AttendancerecordModel>();
            ViewData["ExportExpenceStatus"] = TempData["ExportExpenceStatus"];
            TempData.Keep();

            if (ViewData["ExportExpenceStatus"] != null)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetExpenceGridViewSettings(), ViewData["ExportExpenceStatus"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetExpenceGridViewSettings(), ViewData["ExportExpenceStatus"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetExpenceGridViewSettings(), ViewData["ExportExpenceStatus"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetExpenceGridViewSettings(), ViewData["ExportExpenceStatus"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetExpenceGridViewSettings(), ViewData["ExportExpenceStatus"]);
                    default:
                        break;
                }
            }
            //TempData["Exportcounterist"] = TempData["Exportcounterist"];
            //TempData.Keep();
            return null;
        }

        private GridViewSettings GetExpenceGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "Expense Category";
            // settings.CallbackRouteValues = new { Controller = "Employee", Action = "ExportEmployee" };
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Expense Category";

            settings.Columns.Add(column =>
            {
                column.Caption = "Expense Name";
                column.FieldName = "Expense_Name";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Expense Type";
                column.FieldName = "Expense_Type_Name";

            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Transaction Category";
                column.FieldName = "TRANS_NAME";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Receipt Required";
                column.FieldName = "ReceiptRequiredName";

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
                column.FieldName = "Create_Date";
                column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy hh:mm:ss";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Modified By";
                column.FieldName = "UPDATE_NAME";
            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Modified On";
                column.FieldName = "Update_Date";
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