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
    public class RequirementStatusController : Controller
    {
        UserRightsForPage rights = new UserRightsForPage();
        RequirementStatusBL bl = new RequirementStatusBL();
        public ActionResult RequirementIndex()
        {
            RequirementViewModel objPO = new RequirementViewModel();

            List<Units> listUnits = new List<Units>();
            List<reqStatus> listreqStatus = new List<reqStatus>();

            var datasetobj = bl.DropDownDetailForRole();

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
            if (datasetobj.Tables[4].Rows.Count > 0)
            {
                reqStatus objREQ = new reqStatus();
                foreach (DataRow item in datasetobj.Tables[4].Rows)
                {
                    objREQ = new reqStatus();
                    objREQ.TypeID = Convert.ToString(item["TypeID"]);
                    objREQ.TypeNAME = Convert.ToString(item["TypeNAME"]);
                    listreqStatus.Add(objREQ);
                }
            }

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/RequirementIndex", "RequirementStatus");
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
            ViewBag.Verified = rights.Verified;
            objPO.ReqStatusList = listreqStatus;
            objPO.BranchList = listUnits;

            return View("~/Views/PMS/RequirementStatus/RequirementIndex.cshtml", objPO);
        }

        [HttpPost]
        public JsonResult SaveData(RequirementViewModel Req)
        {
            string returns = "Data not save please try again later.";
            DataTable val = bl.InsertRequirement(Req.ReqID, Req.ReqName, Req.ReqStatus, Req.Branch);
            if (val != null && val.Rows.Count > 0)
            {
                if (val.Rows[0]["counts"].ToString() == "10")
                {
                    returns = "Update Successfully.";
                }
                else if (val.Rows[0]["counts"].ToString() == "1")
                {
                    returns = "Saved Successfully.";
                }
                else if (val.Rows[0]["counts"].ToString() == "100")
                {
                    returns = "Name already exists.";
                }
            }
            return Json(returns);
        }

        public ActionResult GetRequirementPartial()
        {
            try
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/RequirementIndex", "RequirementStatus");
                ViewBag.CanExport = rights.CanExport;
                ViewBag.CanView = rights.CanView;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
                ViewBag.Verified = rights.Verified;

                String weburl = System.Configuration.ConfigurationSettings.AppSettings["SiteURL"];
                List<RequirementList> omel = new List<RequirementList>();

                DataTable dt = new DataTable();

                dt = bl.GetRequirementList();

                if (dt != null)
                {
                    omel = APIHelperMethods.ToModelList<RequirementList>(dt);
                    TempData["ExportReqirement"] = omel;
                    TempData.Keep();
                }
                else
                {
                    TempData["ExportReqirement"] = null;
                    TempData.Keep();
                }
                return PartialView("~/Views/PMS/RequirementStatus/PartialRequirementStatusGrid.cshtml", omel);
            }
            catch
            {
                //   return Redirect("~/OMS/Signoff.aspx");
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        [HttpPost]
        public JsonResult ViewDataShow(string reqID)
        {
            RequirementViewModel viewMDL = new RequirementViewModel();
            DataTable Reqdt = bl.ViewRequirement(reqID);
            if (Reqdt != null && Reqdt.Rows.Count > 0)
            {
                viewMDL.ReqName = Reqdt.Rows[0]["ReqName"].ToString();
                viewMDL.ReqStatus = Reqdt.Rows[0]["ReqStatus"].ToString();
                viewMDL.Branch = Reqdt.Rows[0]["Branch"].ToString();
            }
            return Json(viewMDL);
        }

        [HttpPost]
        public JsonResult DeleteData(string reqID)
        {
            string returns = "Data not Deleted please try again later.";
            int val = bl.DeleteRequirement(reqID);
            if (val > 0)
            {
                returns = "Deleted Successfully.";
            }
            return Json(returns);
        }

        public ActionResult ExportRequirementStatuslist(int type)
        {
            // List<AttendancerecordModel> model = new List<AttendancerecordModel>();
            ViewData["ExportReqirement"] = TempData["ExportReqirement"];
            TempData.Keep();

            if (ViewData["ExportReqirement"] != null)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetRoleGridViewSettings(), ViewData["ExportReqirement"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetRoleGridViewSettings(), ViewData["ExportReqirement"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetRoleGridViewSettings(), ViewData["ExportReqirement"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetRoleGridViewSettings(), ViewData["ExportReqirement"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetRoleGridViewSettings(), ViewData["ExportReqirement"]);
                    default:
                        break;
                }
            }
            //TempData["Exportcounterist"] = TempData["Exportcounterist"];
            //TempData.Keep();
            return null;
        }

        private GridViewSettings GetRoleGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "Requirement Status";
            // settings.CallbackRouteValues = new { Controller = "Employee", Action = "ExportEmployee" };
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Requirement Status";

            settings.Columns.Add(column =>
            {
                column.Caption = "Name";
                column.FieldName = "ReqName";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Status";
                column.FieldName = "TypeNAME";

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