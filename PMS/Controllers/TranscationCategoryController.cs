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
    public class TranscationCategoryController : Controller
    {
        TranscationCategoryBL bl = new TranscationCategoryBL();
        UserRightsForPage rights = new UserRightsForPage();
        public ActionResult TranscationCategoryIndex()
        {
            TranscationCategoryViewModel objPO = new TranscationCategoryViewModel();
            List<BillingTypes> listBill = new List<BillingTypes>();
            List<Units> listUnits = new List<Units>();
            var datasetobj = bl.DropDownDetailForTrans();
            if (datasetobj.Tables[0].Rows.Count > 0)
            {
                BillingTypes obj = new BillingTypes();
                foreach (DataRow item in datasetobj.Tables[0].Rows)
                {
                    obj = new BillingTypes();
                    obj.BillingID = Convert.ToString(item["ID"]);
                    obj.BillingName = Convert.ToString(item["BILLING_NAME"]);
                    listBill.Add(obj);
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
           
            objPO.BillingTypeList = listBill;
            objPO.BranchList = listUnits;

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/TranscationCategoryIndex", "TranscationCategory");
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
            ViewBag.Verified = rights.Verified;
            return View("~/Views/PMS/TranscationCategory/TranscationCategoryIndex.cshtml", objPO);
        }

        [HttpPost]
        public JsonResult SaveData(TranscationCategoryViewModel Trans)
        {
            string returns = "Data not save please try again later";
            string valu = bl.SaveSkillData(Trans.Trans_id, Trans.TransName, Trans.Branch, Trans.BillingType);
            //if (valu == "Data save")
            //{
            //    returns = "Data Save";
            //}
            return Json(valu);
        }

        [HttpPost]
        public JsonResult ViewDataShow(string Trans_id)
        {
            TranscationCategoryViewModel viewMDL = new TranscationCategoryViewModel();
            DataTable Roledt = bl.ViewTranscatin(Trans_id);
            if (Roledt != null && Roledt.Rows.Count > 0)
            {
                viewMDL.Trans_id = Roledt.Rows[0]["TRANS_ID"].ToString();
                viewMDL.TransName = Roledt.Rows[0]["TRANS_NAME"].ToString();
                viewMDL.Branch = Roledt.Rows[0]["BRANCH"].ToString();
                viewMDL.BillingType = Roledt.Rows[0]["BILLING_TYPE"].ToString();

            }
            return Json(viewMDL);
        }

        [HttpPost]
        public JsonResult DeleteData(string Trans_id)
        {
            String ReturnMsg = "";
            string returns = "Data not Deleted please try again later.";
            int val = bl.DeleteTranscation(Trans_id, ref ReturnMsg);
            if (ReturnMsg == "Success")
            {
                returns = "Deleted Successfully.";
            }
            else if (ReturnMsg == "Used in Other Modules. cannot Delete.")
            {
                returns = "Used in Other Modules. Cannot Delete.";
            }
            //else
            //{
            //    returns = ReturnMsg;
            //}
            return Json(returns);
        }


        public ActionResult GetTransCategoryPartial()
        {
            try
            {

                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/TranscationCategoryIndex", "TranscationCategory");
                ViewBag.CanExport = rights.CanExport;
                ViewBag.CanView = rights.CanView;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
                ViewBag.Verified = rights.Verified;

                List<TranscationCategoryList> omel = new List<TranscationCategoryList>();

                DataTable dt = new DataTable();

                dt = bl.GetTransList();

                if (dt!=null)
                {
                    omel = APIHelperMethods.ToModelList<TranscationCategoryList>(dt);
                    TempData["ExportTranscation"] = omel;
                    TempData.Keep();
                }
                else
                {
                    TempData["ExportTranscation"] = null;
                    TempData.Keep();
                }
                return PartialView("~/Views/PMS/TranscationCategory/PartialTranscationCategoryGrid.cshtml", omel);
            }
            catch
            {
                //   return Redirect("~/OMS/Signoff.aspx");
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public ActionResult ExportTransactions(int type)
        {
            // List<AttendancerecordModel> model = new List<AttendancerecordModel>();
            ViewData["ExportTranscation"] = TempData["ExportTranscation"];
            TempData.Keep();

            if (ViewData["ExportTranscation"] != null)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetTransGridViewSettings(), ViewData["ExportTranscation"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetTransGridViewSettings(), ViewData["ExportTranscation"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetTransGridViewSettings(), ViewData["ExportTranscation"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetTransGridViewSettings(), ViewData["ExportTranscation"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetTransGridViewSettings(), ViewData["ExportTranscation"]);
                    default:
                        break;
                }
            }
            //TempData["Exportcounterist"] = TempData["Exportcounterist"];
            //TempData.Keep();
            return null;
        }

        private GridViewSettings GetTransGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "Transactions Category";
            // settings.CallbackRouteValues = new { Controller = "Employee", Action = "ExportEmployee" };
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Transactions Category";

            settings.Columns.Add(column =>
            {
                column.Caption = "Name";
                column.FieldName = "TransName";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Unit";
                column.FieldName = "Branch";

            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Billing Type";
                column.FieldName = "BillingType";

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