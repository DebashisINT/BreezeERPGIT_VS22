using BusinessLogicLayer;
using DevExpress.Web.Mvc;
using NewCompany.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace NewCompany.Controllers
{
    public class ERPSettingsController : Controller
    {
        public ActionResult ERPGridBind()
        {
            ErpSettingList ErpSetL = new ErpSettingList();
            MasterDbEngine mdb = new MasterDbEngine();
            DataTable dt = new DataTable();
            dt = mdb.GetDataTable("select [Key],Value,Description from ERP_SETTINGS");
            List<ErpSettProp> erpset = new List<ErpSettProp>();
            erpset = APIHelperMethods.ToModelList<ErpSettProp>(dt);
            ErpSetL.ErpSettProp = erpset;
            return View("~/Views/NewCompany/ERPSettings/ERPSettings.cshtml", ErpSetL);
        }

        public ActionResult PartialGridBind()
        {
            ErpSettingList ErpSetL = new ErpSettingList();
            MasterDbEngine mdb = new MasterDbEngine();
            DataTable dt = new DataTable();
            dt = mdb.GetDataTable("select [Key],Value,Description from ERP_SETTINGS");
            List<ErpSettProp> erpset = new List<ErpSettProp>();
            erpset = APIHelperMethods.ToModelList<ErpSettProp>(dt);
            ErpSetL.ErpSettProp = erpset;
            return PartialView("~/Views/NewCompany/ERPSettings/_ERPSettingsGridView.cshtml", ErpSetL.ErpSettProp);
        }

        [ValidateInput(false)]
        public ActionResult SaveERPSettings(MVCxGridViewBatchUpdateValues<ErpSettProp, int> updateValues, ErpSettProp options)
        {
            if (options != null)
            {
                ViewBag.Message = "Successfully Updated";
            }
            else
            {
                ViewBag.Message = "";
            }
            MasterDbEngine mdb = new MasterDbEngine();
            DataTable dt = new DataTable();
            if (updateValues != null)
            {
              
                foreach (ErpSettProp item in updateValues.Update)
                {
                    dt = mdb.GetDataTable("update dbo.ERP_SETTINGS set Value='" + item.Value + "',Description='" + item .Description+ "'  where [Key]='" + item.Key + "'");
                }
            }

            return Json(ViewBag.Message, JsonRequestBehavior.AllowGet);
        }
 
     

    }
}

