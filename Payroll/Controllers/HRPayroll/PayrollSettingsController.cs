using DevExpress.Web;
using DevExpress.Web.Mvc;
using Payroll.Models;
using Payroll.Models.DataContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers.HRPayroll
{
    public class PayrollSettingsController : Controller
    {
        //
        // GET: /PayrollSettings/
        public ActionResult Index(string StructureID = "STRUCT0000000005")
        {
            Session["StructureID"] = StructureID;
            PayrollSettings objPsettings = new PayrollSettings();

            DataSet ds = objPsettings.GetAllData(StructureID);

            List<Payheads> pHead = new List<Payheads>();

            pHead = (from DataRow dr in ds.Tables[0].Rows
                     select new Payheads()
                     {
                         PayHeadID = Convert.ToString(dr["PayHeadID"]),
                         PayHeadName = Convert.ToString(dr["PayHeadName"])
                     }).ToList();

            objPsettings.Payheads = pHead;



            List<MainAccount> pMainAccount = new List<MainAccount>();

            pMainAccount = (from DataRow dr in ds.Tables[1].Rows
                            select new MainAccount()
                            {
                                Code = Convert.ToString(dr["Code"]),
                                Name = Convert.ToString(dr["Name"])
                            }).ToList();

            objPsettings.MainAccounts = pMainAccount;

            return View("~/Views/HRPayroll/payrollSettings/payrollSettings.cshtml", objPsettings);
        }
        public PartialViewResult PartialPayrollSettingsGrid()
        {
            return PartialView("~/Views/HRPayroll/payrollSettings/PartialPayrollSettingsGrid.cshtml", GetList());
        }
        public IEnumerable GetList()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            PayRollDataClassDataContext dc = new PayRollDataClassDataContext(connectionString);
            var q = from d in dc.V_PayrollSettings
                    where Convert.ToString(d.StructureId) == Convert.ToString(Session["StructureId"])
                    select d;
            return q;

        }
        public JsonResult Save(InputModal input)
        {
            PayrollSettings objPsettings = new PayrollSettings();
            string output = objPsettings.Save(input);

            return Json("");
        }
        public JsonResult Edit(InputModal input)
        {
            PayrollSettings objPsettings = new PayrollSettings();
            DataSet output = objPsettings.Edit(input);
            if (output != null)
            {
                DataTable dtHeader = output.Tables[0];
                DataTable dtMap = output.Tables[0];

                input.AccountCode = Convert.ToString(dtHeader.Rows[0]["MainAccountCode"]);
                input.Subaacount = Convert.ToString(dtHeader.Rows[0]["SubAccountType"]);
                input.PostingType = Convert.ToString(dtHeader.Rows[0]["PostingType"]);
                input.Payheadids = Convert.ToString(dtHeader.Rows[0]["Payheadids"]);
            }
            return Json(input);
        }
        public JsonResult DeleteRow(InputModal input)
        {
            PayrollSettings objPsettings = new PayrollSettings();
            string output = objPsettings.Delete(input);
            return Json("");

        }
        public ActionResult Export(int type)
        {


            switch (type)
            {
                case 1:
                    return GridViewExtension.ExportToPdf(GetGridViewSettings(), GetList());
                //break;
                case 2:
                    return GridViewExtension.ExportToXlsx(GetGridViewSettings(), GetList());
                //break;
                case 3:
                    return GridViewExtension.ExportToXls(GetGridViewSettings(), GetList());
                case 4:
                    return GridViewExtension.ExportToRtf(GetGridViewSettings(), GetList());
                case 5:
                    return GridViewExtension.ExportToCsv(GetGridViewSettings(), GetList());
                //break;

                default:
                    break;
            }

            return null;
        }
        private GridViewSettings GetGridViewSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "Account Map";
            settings.CallbackRouteValues = new { Controller = "PayrollSettings", Action = "PartialPayrollSettingsGrid" };
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.SettingsPager.FirstPageButton.Visible = true;
            settings.SettingsPager.LastPageButton.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Visible = true;
            settings.SettingsPager.PageSizeItemSettings.Items = new string[] { "10", "20", "50" };
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "serial";


            settings.Columns.Add(column =>
            {
                column.FieldName = "PostingAccount";
                column.Caption = "Posting Account";
                //column.Visible = false;
                column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                column.Width = System.Web.UI.WebControls.Unit.Percentage(30);

            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "SubAccount";
                column.Caption = "Sub Account";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = System.Web.UI.WebControls.Unit.Percentage(10);
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "PayHeads";
                column.Caption = "Pay Heads";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = System.Web.UI.WebControls.Unit.Percentage(50);
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "DRCR";
                column.Caption = "DR/CR";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.Width = System.Web.UI.WebControls.Unit.Percentage(10);
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