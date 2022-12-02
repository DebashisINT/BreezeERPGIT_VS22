using BusinessLogicLayer;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using Manufacturing.Models;
using Manufacturing.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manufacturing.Controllers
{
    public class ManufacturingProcessMasterController : Controller
    {
        //
        // GET: /ManufacturingProcessMaster/
        ManufacturingProcessMasterViewModel WCM = null;
        ManufacturingProcessMasterModel WCMOBJ = null;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        WarehouseConfigMasterBL objWarehouseConfigMaster = new WarehouseConfigMasterBL();
        UserRightsForPage rights = new UserRightsForPage();
        public ManufacturingProcessMasterController()
        {
            WCM = new ManufacturingProcessMasterViewModel();
            WCMOBJ = new ManufacturingProcessMasterModel();
        }
        //
        // GET: /WorkOrder/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProcessEntry()
        {
            if (TempData["WorkCenterID"] != null)
            {
                WCM.WorkCenterID = Convert.ToInt64(TempData["WorkCenterID"]);
                TempData.Keep();

                if (Convert.ToInt64(WCM.WorkCenterID) > 0)
                {
                    DataTable objData =  oDBEngine.GetDataTable("select * from V_WorkCenterList  where WorkCenterID =" + WCM.WorkCenterID + "");
                    if (objData != null && objData.Rows.Count > 0)
                    {
                        DataTable dt = objData;


                        foreach (DataRow row in dt.Rows)
                        {
                            //WCM.WorkCenterID = Convert.ToString(row["WorkCenterID"]);
                            WCM.WorkCenterCode = Convert.ToString(row["WorkCenterCode"]);
                            WCM.WorkCenterDescription = Convert.ToString(row["WorkCenterDescription"]);
                            WCM.Remarks = Convert.ToString(row["Remarks"]);
                            WCM.WorkCenterAddress1 = Convert.ToString(row["Address1"]);
                            WCM.WorkCenterAddress2 = Convert.ToString(row["Address2"]);
                            WCM.WorkCenterAddress3 = Convert.ToString(row["Address3"]);
                            WCM.WorkCenterLandmark = Convert.ToString(row["Landmark"]);
                            WCM.WorkCenterCountry = Convert.ToString(row["CountryID"]);
                            WCM.WorkCenterState = Convert.ToString(row["StateID"]);
                            WCM.WorkCenterCity = Convert.ToString(row["CityID"]);
                            WCM.WorkCenterPin = Convert.ToString(row["PinID"]);
                            WCM.WorkCenterBranch = Convert.ToInt16(row["BranchID"]);

                            WCM.pin_code = Convert.ToString(row["pin_code"]);
                            WCM.city = Convert.ToString(row["city_name"]);
                            WCM.state = Convert.ToString(row["state"]);
                            WCM.country = Convert.ToString(row["cou_country"]);

                        }
                    }
                
                }
            }
            if (TempData["View"] != null)
            {
                ViewBag.View = Convert.ToInt32(TempData["View"]);
            }
            else
            {
                ViewBag.View = 0;
            }
            return View(WCM);
        }

        public JsonResult GetWorkCenterCountry()
        {
            DataTable DT = new DataTable();
            List<Country> list = new List<Country>();
            try
            {
                DT = oDBEngine.GetDataTable("tbl_master_country", "  ltrim(rtrim(cou_country)) Name,ltrim(rtrim(cou_id)) Code ", null);
                if (DT.Rows.Count > 0)
                {
                    Country obj = new Country();
                    foreach (DataRow item in DT.Rows)
                    {
                        obj = new Country();
                        obj.Name = Convert.ToString(item["Name"]);
                        obj.Code = Convert.ToString(item["Code"]);
                        list.Add(obj);
                    }
                }
            }
            catch { }
            return Json(list);
        }

        public JsonResult GetWorkCenterStates(string CountryCode)
        {
            DataTable DT = new DataTable();
            if (CountryCode != "")
            {
                DT = oDBEngine.GetDataTable("tbl_master_state", " ltrim(rtrim(state)) Name,ltrim(rtrim(id)) Code", "countryId=" + CountryCode);
            }
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Code"]));
            }
            return Json(obj);
        }

        public JsonResult GetWorkCenterCities(string StateCode)
        {
            DataTable DT = new DataTable();
            if (StateCode != "")
            {
                DT = oDBEngine.GetDataTable("tbl_master_city", " ltrim(rtrim(city_name)) Name,ltrim(rtrim(city_id))Code", "state_id=" + StateCode);
            }
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Code"]));
            }
            return Json(obj);
        }

        public JsonResult GetWorkCenterPin(string CityCode)
        {
            DataTable DT = new DataTable();
            if (CityCode != "")
            {
                DT = oDBEngine.GetDataTable("tbl_master_pinzip", " pin_id,pin_code", "city_id=" + CityCode, "pin_code");
            }
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["pin_code"]) + "|" + Convert.ToString(dr["pin_id"]));
            }
            return Json(obj);
        }


        public JsonResult GetWorkCenterBranch()
        {
            DataTable dt = new DataTable();
            List<Branch> list = new List<Branch>();
            string userbranch = "";
            try
            {
                if (Session["userbranchHierarchy"] != null)
                {
                    userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                }
                dt = objWarehouseConfigMaster.PopulateBranchByBranchHierarchy(userbranch);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Branch obj = new Branch();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new Branch();
                        obj.branch_id = Convert.ToString(item["branch_id"]);
                        obj.branch_description = Convert.ToString(item["branch_description"]);
                        list.Add(obj);
                    }
                    obj = new Branch();
                    obj.branch_description = "--ALL--";
                    obj.branch_id = "0";
                    list.Insert(0, obj);
                }
            }
            catch { }
            return Json(list);
        }

        public JsonResult WorkCenterInsertUpdate(ManufacturingProcessMasterViewModel obj)
        {
            InsertUpdate objIU = new InsertUpdate();
            obj.UserID = Convert.ToInt64(Session["userid"]);
            try
            {
                DataTable dt = new DataTable();
                dt = WCMOBJ.WorkCenterInsertUpdate(obj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        objIU.Success = Convert.ToBoolean(row["Success"]);
                        objIU.Message = Convert.ToString(row["Message"]);

                    }
                }
            }
            catch { }
            return Json(objIU);
        }

        public ActionResult ProcessList()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ProcessList", "ManufacturingProcessMaster");
            TempData.Clear();
            ViewBag.CanAdd = rights.CanAdd;
            return View(WCM);
        }

        public ActionResult GetWorkCenterList()
        {
            List<ManufacturingProcessMasterViewModel> list = new List<ManufacturingProcessMasterViewModel>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ProcessList", "ManufacturingProcessMaster");
            try
            {

                DataTable dt = new DataTable();

                dt = oDBEngine.GetDataTable("select * from V_WorkCenterList");


                TempData["WCDataTable"] = dt;

                if (dt.Rows.Count > 0)
                {
                    ManufacturingProcessMasterViewModel obj = new ManufacturingProcessMasterViewModel();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new ManufacturingProcessMasterViewModel();
                        obj.WorkCenterID = Convert.ToInt64(item["WorkCenterID"]);
                        obj.WorkCenterCode = Convert.ToString(item["WorkCenterCode"]);
                        obj.WorkCenterDescription = Convert.ToString(item["WorkCenterDescription"]);
                        obj.Remarks = Convert.ToString(item["Remarks"]);
                        obj.WorkCenterBranch = Convert.ToInt16(item["BranchID"]);
                        obj.BranchName = Convert.ToString(item["branch_description"]);
                       

                        //obj.ModifyDate = Convert.ToDateTime(item["ModifyDate"]);
                        list.Add(obj);
                    }
                }
            }
            catch { }
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            return PartialView("_WorkCenterDataList", list);
        }

        public JsonResult SetWorkCenterByID(Int64 workcenterid = 0)
        {
            Boolean Success = false;
            try
            {
                TempData["WorkCenterID"] = workcenterid;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }

        public JsonResult SetWorkCenterByIDView(Int64 workcenterid = 0)
        {
            Boolean Success = false;
            try
            {
                TempData["WorkCenterID"] = workcenterid;
                TempData["View"] = 1;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }


        public JsonResult RemoveWCDataByID(Int32 workcenterid)
        {
            Boolean Success = false;
            try
            {
                if (workcenterid > 0)
                {
                    DataTable objData = oDBEngine.GetDataTable("delete from Master_WorkCenter where WorkCenterID = " + workcenterid + "");
                    Success = true;
                }
            }
            catch { }
            return Json(Success);
        }

        public ActionResult ExportWCGridList(int type)
        {
            ViewData["WCDataTable"] = TempData["WCDataTable"];

            TempData.Keep();

            if (ViewData["WCDataTable"] != null)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetWCGridView(ViewData["WCDataTable"]), ViewData["WCDataTable"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetWCGridView(ViewData["WCDataTable"]), ViewData["WCDataTable"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetWCGridView(ViewData["WCDataTable"]), ViewData["WCDataTable"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetWCGridView(ViewData["WCDataTable"]), ViewData["WCDataTable"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetWCGridView(ViewData["WCDataTable"]), ViewData["WCDataTable"]);
                    default:
                        break;
                }
            }
            return null;
        }

        private GridViewSettings GetWCGridView(object datatable)
        {
            //List<EmployeesTargetSetting> obj = (List<EmployeesTargetSetting>)datatablelist;
            //ListtoDataTable lsttodt = new ListtoDataTable();
            //DataTable datatable = ConvertListToDataTable(obj); 
            var settings = new GridViewSettings();
            settings.Name = "Work Center";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Work Center";
            //String ID = Convert.ToString(TempData["EmployeesTargetListDataTable"]);
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "WorkCenterCode" || datacolumn.ColumnName == "WorkCenterDescription"
                    || datacolumn.ColumnName == "Remarks" || datacolumn.ColumnName == "branch_description")
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "WorkCenterCode")
                        {
                            column.Caption = "Code";
                            column.VisibleIndex = 0;
                        }
                        else if (datacolumn.ColumnName == "WorkCenterDescription")
                        {
                            column.Caption = "Description";
                            column.VisibleIndex = 1;
                        }
                        else if (datacolumn.ColumnName == "Remarks")
                        {
                            column.Caption = "Remarks";
                            column.VisibleIndex = 2;

                        }
                        else if (datacolumn.ColumnName == "branch_description")
                        {
                            column.Caption = "Branch";
                            column.VisibleIndex = 3;
                        }
                        column.FieldName = datacolumn.ColumnName;
                        if (datacolumn.DataType.FullName == "System.Decimal")
                        {
                            column.PropertiesEdit.DisplayFormatString = "0.00";
                        }
                        if (datacolumn.DataType.FullName == "System.DateTime")
                        {
                            column.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
                        }
                    });
                }

            }

            settings.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A4;
            settings.SettingsExport.LeftMargin = 20;
            settings.SettingsExport.RightMargin = 20;
            settings.SettingsExport.TopMargin = 20;
            settings.SettingsExport.BottomMargin = 20;

            return settings;
        }

        public JsonResult GetAddressByPin(string PinCode)
        {
            List<AddressDetails> addByPin = new List<AddressDetails>();
            if (Session["userid"] != null)
            {
                DataTable dt = new DataTable();
                dt = WCMOBJ.GetWorkCenterData(PinCode);

                addByPin = (from DataRow dr in dt.Rows
                            select new AddressDetails
                            {
                                PinCode = Convert.ToString(dr["PinCode"]),
                                PinId = Convert.ToInt32(dr["PinId"]),
                                CountryId = Convert.ToInt32(dr["CountryId"]),
                                CountryName = Convert.ToString(dr["CountryName"]),
                                StateId = Convert.ToInt32(dr["StateId"]),
                                StateName = Convert.ToString(dr["StateName"]),
                                StateCode = Convert.ToString(dr["StateCode"]),
                                CityId = Convert.ToInt32(dr["CityId"]),
                                CityName = Convert.ToString(dr["CityName"])
                            }).ToList();
                return Json(addByPin);
            }
            return null;
        }
	}
}