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
    public class ProficiencyController : Controller
    {
        UserRightsForPage rights = new UserRightsForPage();
        ProficiencyBL bl = new ProficiencyBL();
        public ActionResult ProficiencyIndex()
        {
            try
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ProficiencyIndex", "Proficiency");
                ViewBag.CanExport = rights.CanExport;
                ViewBag.CanView = rights.CanView;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
                ViewBag.Verified = rights.Verified;

                ProficiencyViewModel objList = new ProficiencyViewModel();
                List<MaxRates> listRatinx = new List<MaxRates>();
                List<CharacteristicType> listCharacteristic = new List<CharacteristicType>();
                var datasetobj = bl.DropDownDetailForProficiency();
                if (datasetobj.Tables[0].Rows.Count > 0)
                {
                    MaxRates obj = new MaxRates();
                    foreach (DataRow item in datasetobj.Tables[0].Rows)
                    {
                        obj = new MaxRates();
                        obj.RATING_ID = Convert.ToString(item["RATING_ID"]);
                        obj.RATING = Convert.ToString(item["RATING"]);
                        listRatinx.Add(obj);
                    }
                }

                if (datasetobj.Tables[1].Rows.Count > 0)
                {
                    CharacteristicType obj = new CharacteristicType();
                    foreach (DataRow item in datasetobj.Tables[1].Rows)
                    {
                        obj = new CharacteristicType();
                        obj.Characteristic_ID = Convert.ToString(item["ID"]);
                        obj.Characteristic = Convert.ToString(item["NAME"]);
                        listCharacteristic.Add(obj);
                    }
                }

                objList.Max_RateList = listRatinx;
                objList.CharacteristicTypeList = listCharacteristic;
                return View("~/Views/PMS/Proficiency/ProficiencyIndex.cshtml", objList);
                //DropDownDetailForProficiency
               // return View();
            }
            catch
            {
                //   return Redirect("~/OMS/Signoff.aspx");
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        [HttpPost]
        public JsonResult SaveData(ProficiencyViewModel prof)
        {
            string returns = "Data not save please try again later.";
            DataTable val = bl.InsertProficiency(prof.ProficiencyID, prof.ProficiencyNAME, prof.Ratable_Entity, prof.Min_Rate, prof.Max_Rate, prof.RatingName, prof.Rating_Value, prof.IsDefault,
                 prof.RATING1, prof.RATING2, prof.RATING3, prof.RATING4, prof.RATING5, prof.RATING6, prof.RATING7, prof.RATING8, prof.RATING9, prof.RATING10, prof.DEFAULTVALU);
            if (val != null && val.Rows.Count > 0)
            {
                if (val.Rows[0]["counts"].ToString() == "10")
                {
                    returns = "Update Successfully.";
                }
                else if (val.Rows[0]["counts"].ToString() == "20")
                {
                    returns = "Saved Successfully.";
                }
                else if (val.Rows[0]["counts"].ToString() == "100")
                {
                    returns = "Proficiency name already exists.";
                }
            }
            return Json(returns);
        }

        public ActionResult GetProficiencyPartial()
        {
            try
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ProficiencyIndex", "Proficiency");
                ViewBag.CanExport = rights.CanExport;
                ViewBag.CanView = rights.CanView;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.SalesmanFeedback = rights.SalesmanFeedback;
                ViewBag.Verified = rights.Verified;

                String weburl = System.Configuration.ConfigurationSettings.AppSettings["SiteURL"];
                List<ProficiencyList> omel = new List<ProficiencyList>();

                DataTable dt = new DataTable();

                dt = bl.GetProficiencyList();

                if (dt!=null)
                {
                    omel = APIHelperMethods.ToModelList<ProficiencyList>(dt);
                    TempData["ExportProficiency"] = omel;
                    TempData.Keep();
                }
                else
                {
                    TempData["ExportProficiency"] = null;
                    TempData.Keep();
                }
                return PartialView("~/Views/PMS/Proficiency/PartialProficiencyGrid.cshtml", omel);
            }
            catch
            {
                //   return Redirect("~/OMS/Signoff.aspx");
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        [HttpPost]
        public JsonResult ViewDataShow(string ProficiencyID)
        {
            ProficiencyViewModel viewMDL = new ProficiencyViewModel();
            DataTable Profdt = bl.ViewProficiency(ProficiencyID);
            if (Profdt != null && Profdt.Rows.Count > 0)
            {
                viewMDL.ProficiencyNAME = Profdt.Rows[0]["ProficiencyNAME"].ToString();
                viewMDL.Min_Rate = Profdt.Rows[0]["Min_Rate"].ToString();
                viewMDL.Max_Rate = Profdt.Rows[0]["Max_Rate"].ToString();
                viewMDL.Ratable_Entity = Profdt.Rows[0]["Ratable_Entity"].ToString();
                //viewMDL.RatingName = Profdt.Rows[0]["RatingName"].ToString();
                //viewMDL.Rating_Value = Profdt.Rows[0]["Rating_Value"].ToString();
                //viewMDL.IsDefault = Profdt.Rows[0]["IsDefault"].ToString();
                viewMDL.RATING1 = Profdt.Rows[0]["RATING1"].ToString();
                viewMDL.RATING2 = Profdt.Rows[0]["RATING2"].ToString();
                viewMDL.RATING3 = Profdt.Rows[0]["RATING3"].ToString();
                viewMDL.RATING4 = Profdt.Rows[0]["RATING4"].ToString();
                viewMDL.RATING5 = Profdt.Rows[0]["RATING5"].ToString();
                viewMDL.RATING6 = Profdt.Rows[0]["RATING6"].ToString();
                viewMDL.RATING7 = Profdt.Rows[0]["RATING7"].ToString();
                viewMDL.RATING8 = Profdt.Rows[0]["RATING8"].ToString();
                viewMDL.RATING9 = Profdt.Rows[0]["RATING9"].ToString();
                viewMDL.RATING10 = Profdt.Rows[0]["RATING10"].ToString();
                viewMDL.DEFAULTVALU = Convert.ToInt32(Profdt.Rows[0]["DEFAULTVALU"]);
            }
            return Json(viewMDL);
        }

        [HttpPost]
        public JsonResult DeleteData(string ProficiencyID)
        {
            string returns = "Data not Deleted please try again later.";
            int val = bl.DeleteProficiency(ProficiencyID);
            if (val > 0)
            {
                returns = "Deleted Successfully.";
            }
            return Json(returns);
        }

        public ActionResult ExportProficiencylist(int type)
        {
            // List<AttendancerecordModel> model = new List<AttendancerecordModel>();
            ViewData["ExportProficiency"] = TempData["ExportProficiency"];
            TempData.Keep();

            if (ViewData["ExportProficiency"] != null)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetRoleGridViewSettings(), ViewData["ExportProficiency"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetRoleGridViewSettings(), ViewData["ExportProficiency"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetRoleGridViewSettings(), ViewData["ExportProficiency"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetRoleGridViewSettings(), ViewData["ExportProficiency"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetRoleGridViewSettings(), ViewData["ExportProficiency"]);
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
            settings.Name = "Proficiency Model";
            // settings.CallbackRouteValues = new { Controller = "Employee", Action = "ExportEmployee" };
            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Proficiency Model";

            settings.Columns.Add(column =>
            {
                column.Caption = "Proficiency Name";
                column.FieldName = "ProficiencyNAME";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Ratable Entity";
                column.FieldName = "Ratable_Entity";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Min Rate";
                column.FieldName = "Min_Rate";

            });
            settings.Columns.Add(column =>
            {
                column.Caption = "Max Rate";
                column.FieldName = "Max_Rate";

            });

            //settings.Columns.Add(column =>
            //{
            //    column.Caption = "Rating Name";
            //    column.FieldName = "RatingName";

            //});

            //settings.Columns.Add(column =>
            //{
            //    column.Caption = "Rating Value";
            //    column.FieldName = "Rating_Value";

            //});

            //settings.Columns.Add(column =>
            //{
            //    column.Caption = "Status";
            //    column.FieldName = "IsDefault";

            //});

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