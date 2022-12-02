using DevExpress.Web;
using DevExpress.Web.Mvc;
using Payroll.Models;
using Payroll.Models.DataContext;
using Payroll.Repostiory.SalaryReport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// Mantis Issue 24246
﻿using BusinessLogicLayer;
// End of Mantis Issue 24246

namespace Payroll.Controllers.HRPayroll
{
    [Payroll.Models.Attributes.SessionTimeout]
    public class SalaryReportController : Controller
    {
        private ISalaryReport _SalaryReport;

        public ActionResult DashBoard()
        {
            return View("~/Views/HRPayroll/SalaryReport/DashBoard.cshtml");
        }
        public JsonResult GetActivePeriodGeneration(string ID)
        {
            var jsontable = (String)null; ;
            Msg _msg = new Msg();
            try
            {
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataTable dt = objEngine.GetDataTable(@"select proll_PayStructureMaster.StructureID,proll_PayrollClass_Master.PayrollClassID,proll_PayrollClass_Master.PeriodFrom,
                                                        proll_PayrollClass_Master.PeriodTo,a.YYMM,a.Period from proll_PayStructureMaster
                                                        inner join proll_PayrollClass_Master
                                                        on proll_PayStructureMaster.ClassId=proll_PayrollClass_Master.PayrollClassID
                                                        left join(select proll_PeriodGeneration.PayrollClassID,proll_PeriodGeneration.YYMM,proll_PeriodGeneration.Period 
                                                        from proll_PeriodGeneration where IsActive=1)a
                                                        on a.PayrollClassID=proll_PayrollClass_Master.PayrollClassID
                                                        where proll_PayStructureMaster.StructureID='" + ID + "'");

                if (dt != null)
                {
                    _msg.response_code = "Success";
                    _msg.response_msg = "Success";

                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    jsontable = serializer.Serialize(rows);
                }
            }
            catch (Exception ex)
            {
                _msg.response_code = Convert.ToString(ex);
                _msg.response_msg = "Please try again later";
            }

            var result = new { data = jsontable, data2 = _msg };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult PartialGrid(string StructureCode, string YYMM)
        {
            string ReturnMsg = string.Empty;
            SalaryReportEngine _salaryrprt = new SalaryReportEngine();
            DataTable dt = new DataTable();
            _SalaryReport = new SalaryReport();

            dt = _SalaryReport.PopulateSalaryReport(StructureCode, YYMM, ref ReturnMsg);
            if (dt != null)
            {
                _salaryrprt.SalaryReportList = ToDynamicList(dt);
                TempData["dt_SalaryReporta"] = dt;
            }
            else
            {
                TempData["dt_SalaryReporta"] = null;
            }

            // Mantis Issue 24246
            PageRetentionBL objPageRetain = new BusinessLogicLayer.PageRetentionBL();
            DataTable dtColmn = objPageRetain.GetPageRetention(Session["userid"].ToString(), "SALARY REGISTER");
            if (dtColmn != null && dtColmn.Rows.Count > 0)
            {
                ViewBag.RetentionColumn = dtColmn;
            }
            // End of Mantis Issue 24246

            return PartialView("~/Views/HRPayroll/SalaryReport/PartialGrid.cshtml", _salaryrprt.SalaryReportList);
        }
        public List<dynamic> ToDynamicList(DataTable dt)
        {
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];

                }
            }
            return dynamicDt;
        }

        #region Grid Export

        public ActionResult ExportAllCandidateList(int type)
        {
            SalaryReportEngine _salaryrprt = new SalaryReportEngine();
            string ReturnMsg = string.Empty;

            DataTable dt = (DataTable)TempData["dt_SalaryReporta"];
            TempData.Keep("dt_SalaryReporta");
            ViewData["dt_SalaryReporta"] = TempData["dt_SalaryReporta"];

            if (dt != null)
            {
                _salaryrprt.SalaryReportList = ToDynamicList(dt);
            }

            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToPdf(GetAllCandidateGridSettings(ViewData["dt_SalaryReporta"]), _salaryrprt.SalaryReportList);
                    break;
                case 2:
                    return GridViewExtension.ExportToXlsx(GetAllCandidateGridSettings(ViewData["dt_SalaryReporta"]), ViewData["dt_SalaryReporta"]);
                    break;
                case 3:
                    return GridViewExtension.ExportToXls(GetAllCandidateGridSettings(ViewData["dt_SalaryReporta"]), _salaryrprt.SalaryReportList);
                    break;
                case 4:
                    return GridViewExtension.ExportToRtf(GetAllCandidateGridSettings(ViewData["dt_SalaryReporta"]), _salaryrprt.SalaryReportList);
                    break;
                case 5:
                    return GridViewExtension.ExportToCsv(GetAllCandidateGridSettings(ViewData["dt_SalaryReporta"]), _salaryrprt.SalaryReportList);
                    break;
                default:
                    break;
            }
            return null;
        }
        private GridViewSettings GetAllCandidateGridSettings(object datatable)
        {
            var settings = new GridViewSettings();
            settings.Name = "SalaryReportgridView";
            settings.CallbackRouteValues = new { Controller = "SalaryReport", Action = "PartialGrid" };

            // Export-specific settings
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;

            settings.SettingsExport.FileName = "Salary Report";
            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;

            TempData.Keep();
            DataTable dt = (DataTable)datatable;


            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                settings.Columns.Add(column =>
                   {
                       string field_Name = datacolumn.ColumnName;
                       column.Caption = field_Name.Remove(field_Name.Length - 2);
                       column.FieldName = datacolumn.ColumnName;
                       if (datacolumn.DataType.FullName == "System.Decimal")
                       {
                           column.PropertiesEdit.DisplayFormatString = "0.00";
                           column.ExportCellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;                          
                          
                       }
                       if (datacolumn.DataType.FullName == "System.DateTime")
                       {
                           column.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
                       }
                   });
                
            }
            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            return settings;
        }

        // Mantis Issue 24246
        public ActionResult PageRetention(List<String> Columns)
        {
            try
            {
                String Col = "";
                int i = 1;
                if (Columns != null && Columns.Count > 0)
                {
                    Col = string.Join(",", Columns);
                }

                PageRetentionBL objPageRetention = new BusinessLogicLayer.PageRetentionBL();
                int k = objPageRetention.InsertPageRetention(Col, Session["userid"].ToString(), "SALARY REGISTER");

                return Json(k, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }
        // End of Mantis Issue 24246
        #endregion
    }
}