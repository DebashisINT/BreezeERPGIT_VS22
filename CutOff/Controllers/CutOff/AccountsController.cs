using BusinessLogicLayer;
using BusinessLogicLayer.YearEnding;
using CutOff.Models;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.EntityClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CutOff.Controllers.CutOff
{
    public class AccountsController : Controller
    {
        YearEndingBL YearEndingBL = new YearEndingBL();

        AccountsBL AccntcBL = new AccountsBL();
        
        public ActionResult PartialCashBAnkView(GridViewSelectAllCheckBoxMode selectAllMode = GridViewSelectAllCheckBoxMode.AllPages)
        {
            DateTime CutOffDate = YearEndingBL.GetCutOffDate();
            string CutOffDbname = YearEndingBL.GetCutOffDBName();
          
            DataTable  dtCashBank=new DataTable();
            CashBankModel CBModel = new CashBankModel();
         
            JournalModel JNModel = new JournalModel();
            ViewBag.SelectAllCheckBoxMode = selectAllMode;
            string ConDate = CutOffDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            ViewBag.CutoffDate = ConDate;
            TempData["CutOffDbname"] = CutOffDbname;
            TempData["Cutoffdate"] = CutOffDate;
            DBEngine objDb = new DBEngine();
            string MasterDbname = ConfigurationSettings.AppSettings["MasterDBName"];
            int Status = objDb.ExeInteger("update " + MasterDbname + ".dbo.Master_YearEnding SET Custoff_StartPage='Cashbank'");
            DataTable UpdateStatus = objDb.GetDataTable("select Custoff_StartPage from " + MasterDbname + ".dbo.Master_YearEnding");
            

            if (Convert.ToString(UpdateStatus.Rows[0]["Custoff_StartPage"])=="Journal")
            {
               

                return View("~/Views/CutOff/Accounts/JournalImport.cshtml", JNModel.GetJournals(CutOffDate));
            }
            else if (Convert.ToString(UpdateStatus.Rows[0]["Custoff_StartPage"]) == "Customer")

            {
                 YearEndingBL yrBl = new YearEndingBL();
                DateTime Con = yrBl.GetCutOffDate();
                ViewBag.CutoffDate = Con;
                return View("~/Views/CutOff/CustomerSales/CutoffCustomerSales.cshtml");
            }
            else if (Convert.ToString(UpdateStatus.Rows[0]["Custoff_StartPage"]) == "Vendor")
            {
                YearEndingBL yrBl = new YearEndingBL();
                DateTime Con = yrBl.GetCutOffDate();
                ViewBag.CutoffDate = Con;
                return View("~/Views/CutOff/VendorPurchase/CutoffVendorPurchase.cshtml");
            }
            else if (Convert.ToString(UpdateStatus.Rows[0]["Custoff_StartPage"]) == "Stock")
            {
                YearEndingBL yrBl = new YearEndingBL();
                DateTime Con = yrBl.GetCutOffDate();
                ViewBag.CutoffDate = Con;
                return View("~/Views/CutOff/StockValue/CutOffStockValue.cshtml");
            }
            else if (Convert.ToString(UpdateStatus.Rows[0]["Custoff_StartPage"]) == "Ledger")
            {
                YearEndingBL yrBl = new YearEndingBL();
                DateTime Con = yrBl.GetCutOffDate();
                ViewBag.CutoffDate = Con;
                return View("~/Views/CutOff/Accounts/CutoffLedger.cshtml");
            }
            //else if (Convert.ToString(UpdateStatus.Rows[0]["Custoff_StartPage"]) == "SubLedger")
            //{
            //    YearEndingBL yrBl = new YearEndingBL();
            //    DateTime Con = yrBl.GetCutOffDate();
            //    ViewBag.CutoffDate = Con;
            //    return View("~/Views/CutOff/Accounts/SubLedgerCutoff.cshtml");
            //}
            else
            {
                //var query = CBModel.GetCustomers(Convert.ToDateTime(ConDate));
                //ViewBag.query = query;
                return View("~/Views/CutOff/Accounts/CashBankImport.cshtml", CBModel.GetCustomers(CutOffDate));
            }

           
        }
        public ActionResult AdvancedSelectionPartial(GridViewSelectAllCheckBoxMode selectAllMode = GridViewSelectAllCheckBoxMode.AllPages)
        {
            CashBankModel CBModel = new CashBankModel();
            ViewBag.SelectAllCheckBoxMode = selectAllMode;
            DateTime stCutoff = YearEndingBL.GetCutOffDate();
            string stDBname = YearEndingBL.GetCutOffDBName();
            TempData.Keep("Cutoffdate");
            TempData.Keep("CutOffDbname");
            return PartialView("~/Views/CutOff/Accounts/PartialCashBankView.cshtml", CBModel.GetCustomers(stCutoff));
            
        }
        //[HttpPost]
        //public JsonResult PerformCashBankCutOff(FormCollection frm, string CashBankIds)
        //{

        //   String str="";
        //   int i = 0;
        //   //foreach (string item in CashBankIds)
        //   //{
        //   //    if(i!=0)
        //   //        str = str + "," + Convert.ToString(item);
        //   //    else
        //   //        str = Convert.ToString(item);
        //   //    i++;
        //   //}

         
        //   return Json(str);

        //}
        [HttpPost]
        public JsonResult PerformCashBankCutOff(List<int> CashBankIds)
        {
             String str = "";
             if (CashBankIds!=null && CashBankIds.Count > 0)
             {
                 DateTime stCutoff = (DateTime)TempData["Cutoffdate"];
                 //string stDBname = Convert.ToString(TempData["CutOffDbname"]);
                 string CutOffDbname = YearEndingBL.GetCutOffDBName();
                 TempData.Keep();

                 int i = 0;
                 foreach (var item in CashBankIds)
                 {
                     if (i != 0)
                         str = str + "," + Convert.ToString(item);
                     else
                         str = Convert.ToString(item);
                     i++;
                 }

                 AccntcBL.CashBankCutOff(str, CutOffDbname, stCutoff);
                 //object yourOjbect = new JavaScriptSerializer().DeserializeObject(CashBankIds);
             }
            DBEngine objDb = new DBEngine();
            string MasterDbname = ConfigurationSettings.AppSettings["MasterDBName"];
            //int UpdateStatus = objDb.ExeInteger("update " + MasterDbname + ".dbo.Master_YearEnding SET Custoff_StartPage='Journal'");

            
            return Json(str);

        }
        public ActionResult PartialJournalView(GridViewSelectAllCheckBoxMode selectAllMode = GridViewSelectAllCheckBoxMode.AllPages)
        {
            DateTime CutOffDate = YearEndingBL.GetCutOffDate();
            string CutOffDbname = YearEndingBL.GetCutOffDBName();
            DataTable dtCashBank = new DataTable();
            JournalModel JNModel = new JournalModel();
            ViewBag.SelectAllCheckBoxMode = selectAllMode;
            string ConDate = CutOffDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            ViewBag.CutoffDate = ConDate;

            BusinessLogicLayer.DBEngine objDb = new BusinessLogicLayer.DBEngine();
            string MasterDbname = ConfigurationSettings.AppSettings["MasterDBName"];
            int UpdateStatus = objDb.ExeInteger("update " + MasterDbname + ".dbo.Master_YearEnding SET Custoff_StartPage='Journal'");



            TempData["CutOffDbname"] = CutOffDbname;
            TempData["Cutoffdate"] = CutOffDate;
            return View("~/Views/CutOff/Accounts/JournalImport.cshtml", JNModel.GetJournals(CutOffDate));

        }
        public ActionResult AdvancedSelectionJournalPartial(GridViewSelectAllCheckBoxMode selectAllMode = GridViewSelectAllCheckBoxMode.AllPages)
        {

            JournalModel JNModel = new JournalModel();

            ViewBag.SelectAllCheckBoxMode = selectAllMode;
            DateTime stCutoff = YearEndingBL.GetCutOffDate();
            string stDBname = YearEndingBL.GetCutOffDBName();
            TempData.Keep("Cutoffdate");
            TempData.Keep("CutOffDbname");
            return PartialView("~/Views/CutOff/Accounts/PartialJournalView.cshtml", JNModel.GetJournals(stCutoff));

        }
        [HttpPost]
        public JsonResult PerformJournalCutOff(List<int> JournalIds)
        {
            String str = "";
            if (JournalIds != null && JournalIds.Count() > 0)
            {
                DateTime stCutoff = (DateTime)TempData["Cutoffdate"];
                //string stDBname = Convert.ToString(TempData["CutOffDbname"]);
                string CutOffDbname = YearEndingBL.GetCutOffDBName();
                TempData.Keep();

                int i = 0;
                foreach (var item in JournalIds)
                {
                    if (i != 0)
                        str = str + "," + Convert.ToString(item);
                    else
                        str = Convert.ToString(item);
                    i++;
                }
                AccntcBL.JournalCutOff(str, CutOffDbname, stCutoff);
                //object yourOjbect = new JavaScriptSerializer().DeserializeObject(CashBankIds);
            }
           
            return Json(str);

        }

        public ActionResult CutoffLedger()
        {
            DBEngine objDb = new DBEngine();
            string MasterDbname = ConfigurationSettings.AppSettings["MasterDBName"];
           // int UpdateStatus = objDb.ExeInteger("update " + MasterDbname + ".dbo.Master_YearEnding SET Custoff_StartPage='Ledger'");
            YearEndingBL yrBl = new YearEndingBL();
            DateTime Con = yrBl.GetCutOffDate();
            ViewBag.CutoffDate = Con;
            return View("~/Views/CutOff/Accounts/CutoffLedger.cshtml");
        }

        public JsonResult StartLedgerCutoff(DateTime Cutoffdate, int CloseStock)
        {
            string MasterDbname = ConfigurationSettings.AppSettings["MasterDBName"];
            string msg = "";
            Ledger obj = new Ledger();

            string output = obj.BranchWiseLedgerBalance(Cutoffdate, CloseStock);
            //string lastStatus = YearEndingBL.YearendingLastStage();
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CutoffSubLedger()
        {
            DBEngine objDb = new DBEngine();
            string MasterDbname = ConfigurationSettings.AppSettings["MasterDBName"];
            int UpdateStatus = objDb.ExeInteger("update " + MasterDbname + ".dbo.Master_YearEnding SET Custoff_StartPage='SubLedger'");
            YearEndingBL yrBl = new YearEndingBL();
            DateTime Con = yrBl.GetCutOffDate();
            ViewBag.CutoffDate = Con;
            return View("~/Views/CutOff/Accounts/SubLedgerCutoff.cshtml");
        }

        public JsonResult StartSubLedgerCutoff(DateTime Cutoffdate)
        {
            string msg = "";
            Ledger obj = new Ledger();

            string output = obj.BranchWiseSubLedgerBalance(Cutoffdate);

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinalCutoff()
        {
            YearEndingBL yrBl = new YearEndingBL();
            string lastStatus = yrBl.YearendingLastStage();
            return View("~/Views/CutOff/Accounts/FinalCutoff.cshtml");
        }

	}
   
}