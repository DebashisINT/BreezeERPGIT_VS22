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
    public class RolesMasterController : Controller
    {
        UserRightsForPage rights = new UserRightsForPage();
        RoleMasterBL bl = new RoleMasterBL();

        public ActionResult IndexOne()
        {
            return View("~/Views/PMS/Master/RolesMaster.cshtml");
        }
        public ActionResult RolesMasterView()
        {
            RolesMasterViewModel objPO = new RolesMasterViewModel();
            List<BillingTypes> listBill = new List<BillingTypes>();
            List<Units> listUnits = new List<Units>();
            List<SkillCategories> listSkill = new List<SkillCategories>();
            List<SkillSets> listSkillSet = new List<SkillSets>();
            var datasetobj = bl.DropDownDetailForRole();
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
            if (datasetobj.Tables[2].Rows.Count > 0)
            {
                SkillCategories obj = new SkillCategories();
                foreach (DataRow item in datasetobj.Tables[2].Rows)
                {
                    obj = new SkillCategories();
                    obj.SkillCategoryID = Convert.ToString(item["SkillCategorieID"]);
                    obj.SkillCategoryName = Convert.ToString(item["SkillCategoryName"]);
                    listSkill.Add(obj);
                }
            }
            if (datasetobj.Tables[3].Rows.Count > 0)
            {
                SkillSets obj = new SkillSets();
                foreach (DataRow item in datasetobj.Tables[3].Rows)
                {
                    obj = new SkillSets();
                    obj.SkillSetID = Convert.ToString(item["SkillsetID"]);
                    obj.SkillSetName = Convert.ToString(item["SkillsetName"]);
                    listSkillSet.Add(obj);
                }
            }
            objPO.BillingTypeList = listBill;
            objPO.UnitList = listUnits;
            objPO.SkillCategoryList = listSkill;
            objPO.SkillSetList = listSkillSet;

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/RolesMasterView", "RolesMaster");
            ViewBag.CanExport = rights.CanExport;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
            ViewBag.Verified = rights.Verified;

            return View("~/Views/PMS/RolesMaster/RolesMasterView.cshtml", objPO);
        }

        [HttpPost]
        public JsonResult SaveData(RolesMasterViewModel role)
        {
            string returns = "Data not save please try again later";
            int val = bl.InsertRoleMaster(role.RoleID, role.RoleName, role.Description, role.BillingType, role.Unit, role.SkillCategory, role.SkillSet, "0", null);
            if (val > 0)
            {
                returns = "Saved Successfully.";
            }
            else if (val==-1)
            {
                returns = "Name already exists.";
            }
            return Json(returns);
        }

        public ActionResult GetRoleMasterPartial()
        {
            try
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/RolesMasterView", "RolesMaster");
                ViewBag.CanExport = rights.CanExport;
                ViewBag.CanView = rights.CanView;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
                ViewBag.Verified = rights.Verified;

                String weburl = System.Configuration.ConfigurationSettings.AppSettings["SiteURL"];
                List<RolesMasterDetailsView> omel = new List<RolesMasterDetailsView>();

                DataTable dt = new DataTable();

                dt = bl.GetRoleMasterList();

                if (dt.Rows.Count > 0)
                {
                    omel = APIHelperMethods.ToModelList<RolesMasterDetailsView>(dt);
                    TempData["ExportRoleMaster"] = omel;
                    TempData.Keep();
                }
                else
                {
                    TempData["ExportRoleMaster"] = null;
                    TempData.Keep();
                }
                return PartialView("~/Views/PMS/RolesMaster/PartialGridRoleView.cshtml", omel);
            }
            catch
            {
                //   return Redirect("~/OMS/Signoff.aspx");
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        [HttpPost]
        public JsonResult ViewDatashow(string roleID)
        {
            RolesMasterDetailsView viewMDL = new RolesMasterDetailsView();
            DataTable Roledt = bl.ViewRoleMaster(roleID);
            if (Roledt != null && Roledt.Rows.Count > 0)
            {
                viewMDL.BILLING_NAME = Roledt.Rows[0]["BILLING_TYPE"].ToString();
                viewMDL.DESCRIPTION = Roledt.Rows[0]["DESCRIPTION"].ToString();
                viewMDL.ROLE_ID = Convert.ToInt32(Roledt.Rows[0]["ROLE_ID"].ToString());
                viewMDL.ROLE_NAME = Roledt.Rows[0]["ROLE_NAME"].ToString();
                viewMDL.SKILL_SET = Roledt.Rows[0]["SKILL_SET"].ToString();
                viewMDL.UnitName = Roledt.Rows[0]["UNIT"].ToString();
                viewMDL.SkillCategoryName = Roledt.Rows[0]["SKILL_CATEGORY"].ToString();
            }
            return Json(viewMDL);
        }

        [HttpPost]
        public JsonResult DeleteData(string roleID)
        {
            String ReturnMsg = "";
            string returns = "Data not Deleted please try again later";
            //  int val = 
            bl.DeleteRoleMaster(roleID, ref ReturnMsg);
            if (ReturnMsg == "Success")
            {
                returns = "Deleted Successfully.";
            }
            else if (ReturnMsg == "Used in Other Modules. cannot Delete.")
            {
                returns = "Used in Other Modules. cannot Delete.";
            }
            else
            {
                returns = ReturnMsg;
            }
            return Json(returns);
        }

        [HttpPost]
        public JsonResult ViewSkillSet(string SkillID)
        {
            List<SkillSets> listSkillSet = new List<SkillSets>();
            DataTable Skilldt = bl.ViewSkillSetr(SkillID);
            if (Skilldt != null && Skilldt.Rows.Count > 0)
            {
                SkillSets obj = new SkillSets();
                foreach (DataRow item in Skilldt.Rows)
                {
                    obj = new SkillSets();
                    obj.SkillSetID = Convert.ToString(item["SkillsetID"]);
                    obj.SkillSetName = Convert.ToString(item["SkillsetName"]);
                    listSkillSet.Add(obj);
                }
            }
            return Json(listSkillSet);
        }

        public ActionResult ExportLocationlist(int type)
        {
            // List<AttendancerecordModel> model = new List<AttendancerecordModel>();
            ViewData["ExportRoleMaster"] = TempData["ExportRoleMaster"];
            TempData.Keep();

            if (ViewData["ExportRoleMaster"] != null)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetRoleGridViewSettings(), ViewData["ExportRoleMaster"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetRoleGridViewSettings(), ViewData["ExportRoleMaster"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetRoleGridViewSettings(), ViewData["ExportRoleMaster"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetRoleGridViewSettings(), ViewData["ExportRoleMaster"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetRoleGridViewSettings(), ViewData["ExportRoleMaster"]);
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
            settings.Name = "Role Master";
            // settings.CallbackRouteValues = new { Controller = "Employee", Action = "ExportEmployee" };
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Role Master";

            settings.Columns.Add(column =>
            {
                column.Caption = "Role";
                column.FieldName = "ROLE_NAME";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Description";
                column.FieldName = "DESCRIPTION";

            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Unit";
                column.FieldName = "UnitName";

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