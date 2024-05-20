/***********************************************************************************************************************************
 * Rev 1.0      V2.0.43     18-01-2024   Sanchita   ERP setting page should integrated in the settings module of ERP - Mantis: 27176
 * Rev 2.0      V2.0.43     20-05-2024   Priti      0027443: Global Search and Page Size Increase Decrease are not working in ERP Settings
 * *****************************************************************************************************************************/
using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using NewCompany.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace NewCompany.Controllers
{
    public class ERPSettingsController : Controller
    {
        DBEngine oDBEngine = new DBEngine();
        // Rev 1.0
        UserRightsForPage rights = new UserRightsForPage();
        // End of Rev 1.0

        public ActionResult ERPGridBind()
        {
            // Rev 1.0
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ERPGridBind", "ERPSettings");
            ViewBag.CanEdit = rights.CanEdit;
            // End of Rev 1.0

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
            //Rev 2.0
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ERPGridBind", "ERPSettings");
            ViewBag.CanEdit = rights.CanEdit;
            //Rev 2.0 End

            List<ErpSettProp> erpset = new List<ErpSettProp>();
            ErpSettingList ErpSetL = new ErpSettingList();
            MasterDbEngine mdb = new MasterDbEngine();
            DataTable dt = new DataTable();
            dt = mdb.GetDataTable("select [Key],Value,Description from ERP_SETTINGS");
            if (dt.Rows.Count > 0)
            {
                ErpSettProp obj = new ErpSettProp();
                foreach (DataRow item in dt.Rows)
                {
                    obj = new ErpSettProp();
                    obj.Key = Convert.ToString(item["Key"]);
                    obj.Value = Convert.ToString(item["Value"]);
                    obj.Description = Convert.ToString(item["Description"]);
                    erpset.Add(obj);
                }
            }


           
            //erpset = APIHelperMethods.ToModelList<ErpSettProp>(dt);
            //ErpSetL.ErpSettProp = erpset;
            //return PartialView("~/Views/NewCompany/ERPSettings/_ERPSettingsGridView.cshtml", ErpSetL.ErpSettProp);
            return PartialView("~/Views/NewCompany/ERPSettings/_ERPSettingsGridView.cshtml", erpset);
            
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

        //Rev 2.0 
        public JsonResult EDITERPSETTINGSBYKEY(String _KEY = "", String _VALUE = "")
        {
            ReturnData obj = new ReturnData();
            try
            {
                DataSet datasetobj = EDITERPSETTINGS("EDIT", _KEY, _VALUE);
                if (datasetobj.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in datasetobj.Tables[0].Rows)
                    {
                        obj.Success = Convert.ToBoolean(item["Success"]);
                        obj.Message = Convert.ToString(item["Message"]);
                    }
                }
            }
            catch { }
            return Json(obj);
        }
        public DataSet EDITERPSETTINGS(String Action, String _KEY, String _VALUE)
        {
            string masterdbname = Convert.ToString(ConfigurationSettings.AppSettings["MasterDBName"]);
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_ERPSETTINGS");
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@ERPKEY", 500, _KEY);
            proc.AddVarcharPara("@ERPVALUE", 500, _VALUE);
            proc.AddVarcharPara("@Masterdbname", 100, masterdbname);
            ds = proc.GetDataSet();
            return ds;
        }
        //Rev 2.0 End
    }
}

