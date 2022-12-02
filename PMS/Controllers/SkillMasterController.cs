using BusinessLogicLayer.PMS;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using PMS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace PMS.Controllers
{
    public class SkillMasterController : Controller
    {
        SkillMasterBL bl = new SkillMasterBL();
        UserRightsForPage rights = new UserRightsForPage();
        public ActionResult SkillIndex()
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);


            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/SkillIndex", "SkillMaster");

            SkillMasterViewModel objPO = new SkillMasterViewModel();
            List<Units> listUnits = new List<Units>();

            var datasetobj = bl.DropDownDetailForSkill();
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
            objPO.BranchList = listUnits;
            ViewBag.CanExport = rights.CanExport;
            objPO.UserRightsForPage = rights;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
            ViewBag.Verified = rights.Verified;
            return View("~/Views/PMS/SkillMaster/SkillIndex.cshtml", objPO);
        }

        [HttpPost]
        public JsonResult SaveData(SkillMasterViewModel skills)
        {
            String ReturnMsg = "";
            DataTable skilss = CreateDataTable(skills.SkillList);
            string returns = "Data not save please try again later";
            //string valu = 
            bl.SaveSkillData(skills.skill_id, skills.SkillName, skills.Description, skills.Charecteristic_Type, skills.Branch, skilss, ref ReturnMsg);
            if (ReturnMsg == "Success")
            {
                returns = "Saved Successfully.";
            }
            else if (ReturnMsg == "Update")
            {
                returns = "Updated Successfully.";
            }
            else if (ReturnMsg == "Skill Set Used in Other Modules. cannot Modify.")
            {
                returns = "Skill Set Used in Other Modules. Cannot Modify.";
            }
            else
            {
                returns = ReturnMsg;
            }
            return Json(returns);
        }

        public ActionResult GetSkillMasterPartial()
        {
            try
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/SkillIndex", "SkillMaster");
                ViewBag.CanExport = rights.CanExport;
                ViewBag.CanView = rights.CanView;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
                ViewBag.Verified = rights.Verified;

                List<SkillMasterDetailsView> omel = new List<SkillMasterDetailsView>();

                DataTable dt = new DataTable();



                dt = bl.GetSkillMasterList();

                if (dt.Rows.Count > 0)
                {
                    omel = APIHelperMethods.ToModelList<SkillMasterDetailsView>(dt);
                    TempData["ExportSkillMaster"] = omel;
                    TempData.Keep();
                }
                else
                {
                    TempData["ExportSkillMaster"] = null;
                    TempData.Keep();
                }
                return PartialView("~/Views/PMS/SkillMaster/PartialSkillMasterGrid.cshtml", omel);
            }
            catch
            {
                //   return Redirect("~/OMS/Signoff.aspx");
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public ActionResult ExportLocationlist(int type)
        {
            // List<AttendancerecordModel> model = new List<AttendancerecordModel>();
            ViewData["ExportSkillMaster"] = TempData["ExportSkillMaster"];
            TempData.Keep();

            if (ViewData["ExportSkillMaster"] != null)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetEmployeeBatchGridViewSettings(), ViewData["ExportSkillMaster"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetEmployeeBatchGridViewSettings(), ViewData["ExportSkillMaster"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetEmployeeBatchGridViewSettings(), ViewData["ExportSkillMaster"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetEmployeeBatchGridViewSettings(), ViewData["ExportSkillMaster"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetEmployeeBatchGridViewSettings(), ViewData["ExportSkillMaster"]);
                    default:
                        break;
                }
            }
            //TempData["Exportcounterist"] = TempData["Exportcounterist"];
            //TempData.Keep();
            return null;
        }

        private GridViewSettings GetEmployeeBatchGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "Skill Master";
            // settings.CallbackRouteValues = new { Controller = "Employee", Action = "ExportEmployee" };
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Skill Master";

            settings.Columns.Add(column =>
            {
                column.Caption = "Name";
                column.FieldName = "SkillName";

            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Description";
                column.FieldName = "DESCRIPTION";

            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Characteristic Type";
                column.FieldName = "Charecteristic_Type";

            });

            settings.Columns.Add(column =>
            {
                column.Caption = "Unit";
                column.FieldName = "Branch";

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

        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();
            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, typeof(System.String)));
            }
            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                } dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        [HttpPost]
        public JsonResult ViewDataset(string Skill_id)
        {
            SkillMasterViewModel viewMDL = new SkillMasterViewModel();
            DataTable Roledt = bl.ViewSkillMaster(Skill_id);
            if (Roledt != null && Roledt.Rows.Count > 0)
            {
                viewMDL.skill_id = Roledt.Rows[0]["SkillMaster_ID"].ToString();
                viewMDL.Description = Roledt.Rows[0]["Description"].ToString();
                viewMDL.SkillName = Roledt.Rows[0]["SkillMaster_NAME"].ToString();
                viewMDL.Charecteristic_Type = Roledt.Rows[0]["Charecteristic_Type"].ToString();
                viewMDL.Branch = Roledt.Rows[0]["Branch_ID"].ToString();
                viewMDL.skillDetails = Roledt.Rows[0]["skill_set"].ToString();

            }
            return Json(viewMDL);
        }

        [HttpPost]
        public JsonResult DeleteData(string skillID)
        {
            String ReturnMsg = "";
            string returns = "Data not Deleted please try again later.";
            //int val = 
            bl.DeleteSkillMaster(skillID, ref ReturnMsg);
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


    }
}