using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using Manufacturing.Models;
using Manufacturing.Models.ViewModel;
using Manufacturing.Models.ViewModel.BOMEntryModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Manufacturing.Controllers
{
    public class IssueReturnController : Controller
    {
        IssueReturnViewModel obj = new IssueReturnViewModel();
        BOMEntryModel objdata = null;
        WorkOrderModel objWO = null;
        IssueReturnModel objIR = null;
        DBEngine oDBEngine = new DBEngine();
        string JVNumStr = string.Empty;
        UserRightsForPage rights = new UserRightsForPage();
        ProductionIssueModel objPI = null;
        CommonBL cSOrder = new CommonBL();
        //
        // GET: /IssueReturn/
        public IssueReturnController()
        {
            objdata = new BOMEntryModel();
            objWO = new WorkOrderModel();
            objIR = new IssueReturnModel();
            objPI = new ProductionIssueModel();
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult IssueReturnList()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/IssueReturnList", "IssueReturn");
            TempData.Clear();
            ViewBag.CanAdd = rights.CanAdd;
            return View(obj);
        }

        public ActionResult IssueReturnEntry()
        {
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            string HierarchySelectInEntryModule = cSOrder.GetSystemSettingsResult("Show_Hierarchy");
            if (!String.IsNullOrEmpty(HierarchySelectInEntryModule))
            {
                if (HierarchySelectInEntryModule.ToUpper().Trim() == "YES")
                {
                    ViewBag.Hierarchy = "1";
                }
                else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    ViewBag.Hierarchy = "0";
                }
            }
            List<BranchUnit> list = new List<BranchUnit>();
            var datasetobj = objdata.DropDownDetailForBOM("GetUnitDropDownData", Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["userbranchHierarchy"]), 0, 0);
            if (datasetobj.Tables[0].Rows.Count > 0)
            {
                BranchUnit objUnit = new BranchUnit();
                foreach (DataRow item in datasetobj.Tables[0].Rows)
                {
                    objUnit = new BranchUnit();
                    objUnit.BranchID = Convert.ToString(item["BANKBRANCH_ID"]);
                    objUnit.BankBranchName = Convert.ToString(item["BANKBRANCH_NAME"]);
                    list.Add(objUnit);
                }
            }
            obj.UnitList = list;
            //obj.Issue_Date = DateTime.Now;

            try
            {

                if (TempData["IssueReturnID"] != null)
                {
                    obj.IssueReturnID = Convert.ToInt64(TempData["IssueReturnID"]);
                    if (obj.IssueReturnID > 0)
                    {
                        DataTable objData = objIR.GetIssueReturnData("GetIssueReturnData", obj.IssueReturnID, 0);
                        if (objData != null && objData.Rows.Count > 0)
                        {
                            DataTable dt = objData;


                            foreach (DataRow row in dt.Rows)
                            {
                                obj.IssueReturnID = Convert.ToInt64(row["IssueReturnID"]);
                                obj.ProductionIssueID = Convert.ToInt64(row["ProductionIssueID"]);
                                obj.WorkOrderID = Convert.ToInt64(row["WorkOrderID"]);
                                obj.ProductionOrderID = Convert.ToInt64(row["ProductionOrderID"]);
                                obj.Details_ID = Convert.ToInt64(row["Details_ID"]);
                                obj.Issue_SchemaID = Convert.ToInt64(row["Issue_SchemaID"]);
                                obj.Issue_No = Convert.ToString(row["Issue_No"]);
                                obj.Issue_Date = Convert.ToDateTime(row["Issue_Date"]);
                                obj.BRANCH_ID = Convert.ToInt64(row["BRANCH_ID"]);
                                obj.OrderNo = Convert.ToString(row["OrderNo"]);
                                obj.Issue_Qty = Convert.ToDecimal(row["Issue_Qty"]);
                                obj.BRANCH_ID = Convert.ToInt64(row["BRANCH_ID"]);
                                obj.WorkCenterID = Convert.ToInt64(row["WorkCenterID"]);
                                obj.BOMNo = Convert.ToString(row["BOM_No"]);
                                obj.RevNo = Convert.ToString(row["REV_No"]);
                                obj.FinishedItem = Convert.ToString(row["ProductName"]);
                                obj.FinishedUom = Convert.ToString(row["FinishedUom"]);
                                obj.Finished_Qty = Convert.ToDecimal(row["Finished_Qty"]);
                                obj.Warehouse = Convert.ToString(row["Warehouse"]);
                                obj.ProductionOrderNo = Convert.ToString(row["ProductionOrderNo"]);
                                obj.WorkCenterCode = Convert.ToString(row["WorkCenterCode"]);
                                obj.ProductionIssueNo = Convert.ToString(row["ProductionIssueNo"]);
                                obj.txtProductionIssueDate = Convert.ToDateTime(row["ProductionIssueDate"]).ToString("dd-MM-yyyy");
                                obj.strRemarks = Convert.ToString(row["Remarks"]); 
                                obj.WorkCenterDescription = Convert.ToString(row["WorkCenterDescription"]);
                                obj.ProductionOrderDate = Convert.ToDateTime(row["ProductionOrderDate"]).ToString("dd-MM-yyyy");
                                obj.MaxQty = Convert.ToDecimal(row["MaxQty"]);

                                obj.PartNo = Convert.ToString(row["PartNo"]);
                                obj.PartNoName = Convert.ToString(row["PartNoName"]);
                                obj.DesignNo = Convert.ToString(row["DesignNo"]);
                                obj.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                                obj.Description = Convert.ToString(row["sProducts_Name"]);
                                obj.Proj_Code = Convert.ToString(row["Proj_Code"]);
                                obj.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);
                                TempData["DetailsID"] = Convert.ToString(row["Details_ID"]);
                            }
                        }
                    }
                }
                else
                {
                    TempData["DetailsID"] = null;
                    TempData.Clear();
                }

                if (obj.ProductionIssueID < 1)
                {
                    obj.Issue_Date = DateTime.Now;
                }

            }
            catch { }


            ViewBag.LastCompany = Convert.ToString(Session["LastCompany"]);
            ViewBag.LastFinancialYear = Convert.ToString(Session["LastFinYear"]);
            ViewBag.ProjectShow = ProjectSelectInEntryModule;    
            TempData["Count"] = 1;
            TempData.Keep();

            #region Barcode Section

            if (IsBarcodeGeneratete() == true)
            {
                ViewBag.hdfIsBarcodeActive = "Y";
                ViewBag.hdfIsBarcodeGenerator = "Y";
                //MultiWarehouceuc.uchdfIsBarcodeActive.Value = "Y";
                //MultiWarehouceuc.uchdfIsBarcodeGenerator.Value = "Y";

            }
            else
            {
                ViewBag.hdfIsBarcodeActive = "N";
                ViewBag.hdfIsBarcodeGenerator = "N";
                //MultiWarehouceuc.uchdfIsBarcodeActive.Value = "N";
                //MultiWarehouceuc.uchdfIsBarcodeGenerator.Value = "N";
            }

            #endregion

            return View(obj);
        }

        public bool IsBarcodeGeneratete()
        {
            bool IsGeneratete = false;

            try
            {
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataTable DT_TC = objEngine.GetDataTable("tbl_Master_SystemControl", " BarcodeGeneration ", null);
                if (DT_TC != null && DT_TC.Rows.Count > 0)
                {
                    IsGeneratete = Convert.ToBoolean(DT_TC.Rows[0]["BarcodeGeneration"]);
                }

                return IsGeneratete;
            }
            catch
            {
                return IsGeneratete;
            }
        }

        public ActionResult GetIssueReturnProductList(Int64 DetailsID = 0)
        {
            BOMProduct bomproductdataobj = new BOMProduct();
            List<BOMProduct> bomproductdata = new List<BOMProduct>();
            try
            {
                //bomproductdataobj.SlNO = "1";
                //bomproductdata.Add(bomproductdataobj);
            }
            catch { }
            return PartialView("_IssueReturnBOMProductGrid", bomproductdata);
        }

        public JsonResult getNumberingSchemeRecord()
        {
            List<SchemaNumber> list = new List<SchemaNumber>();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            DataTable Schemadt = objdata.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "103", "Y");
            if (Schemadt.Rows.Count > 0)
            {
                SchemaNumber obj = new SchemaNumber();
                foreach (DataRow item in Schemadt.Rows)
                {
                    obj = new SchemaNumber();
                    obj.SchemaID = Convert.ToString(item["Id"]);
                    obj.SchemaName = Convert.ToString(item["SchemaName"]);
                    list.Add(obj);
                }
            }

            return Json(list);
        }

        public ActionResult GetWCList()
        {
            List<WorkCenterViewModel> list = new List<WorkCenterViewModel>();
            try
            {
                WorkCenterViewModel obj = new WorkCenterViewModel();
                DataTable objData = objWO.GetWorkOrderData("GetWCList");
                if (objData != null && objData.Rows.Count > 0)
                {
                    DataTable dt = objData;
                    foreach (DataRow row in dt.Rows)
                    {
                        obj = new WorkCenterViewModel();
                        obj.WorkCenterID = Convert.ToInt64(row["WorkCenterID"]);
                        obj.WorkCenterCode = Convert.ToString(row["WorkCenterCode"]);
                        obj.WorkCenterDescription = Convert.ToString(row["WorkCenterDescription"]);
                        obj.Remarks = Convert.ToString(row["Remarks"]);
                        list.Add(obj);
                    }
                }

            }
            catch { }
            return PartialView("_WorkCenterList", list);
        }

        public ActionResult GetPIList()
        {
            List<IssueReturnViewModel> list = new List<IssueReturnViewModel>();
            try
            {
                IssueReturnViewModel objWOL = new IssueReturnViewModel();
                DataTable objData = objIR.GetIssueReturnData("GetAllProductionIssueData", 0, 0);

                if (objData != null && objData.Rows.Count > 0)
                {
                    DataTable dt = objData;
                    foreach (DataRow row in dt.Rows)
                    {
                        objWOL = new IssueReturnViewModel();
                        objWOL.ProductionIssueID = Convert.ToInt64(row["ProductionIssueID"]);
                        objWOL.Issue_No = Convert.ToString(row["Issue_No"]);
                        objWOL.WorkOrderID = Convert.ToInt64(row["WorkOrderID"]);
                        objWOL.OrderNo = Convert.ToString(row["OrderNo"]);
                        objWOL.ProductionOrderNo = Convert.ToString(row["ProductionOrderNo"]);
                        objWOL.ProductionOrderID = Convert.ToInt64(row["ProductionOrderID"]);
                        objWOL.Details_ID = Convert.ToInt64(row["Details_ID"]);
                        objWOL.BOMNo = Convert.ToString(row["BOM_No"]);
                        objWOL.Issue_Date = Convert.ToDateTime(row["Issue_Date"]);
                        objWOL.RevNo = Convert.ToString(row["REV_No"]);
                        objWOL.FinishedItem = Convert.ToString(row["ProductName"]);
                        objWOL.FinishedUom = Convert.ToString(row["FinishedUom"]);
                        objWOL.ProductionOrderDate = Convert.ToDateTime(row["ProductionOrderDate"]).ToString("dd-MM-yyyy");
                        objWOL.Warehouse = Convert.ToString(row["Warehouse"]);
                        objWOL.Issue_Qty = Convert.ToDecimal(row["Issue_Qty"]);
                        objWOL.WorkCenterID = Convert.ToInt64(row["WorkCenterID"]);
                        objWOL.WorkCenterCode = Convert.ToString(row["WorkCenterCode"]);
                        objWOL.WorkCenterDescription = Convert.ToString(row["WorkCenterDescription"]);
                        objWOL.PartNo = Convert.ToString(row["PartNo"]);
                        objWOL.PartNoName = Convert.ToString(row["PartNoName"]);
                        objWOL.DesignNo = Convert.ToString(row["DesignNo"]);
                        objWOL.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                        objWOL.Description = Convert.ToString(row["sProducts_Name"]);
                        objWOL.Proj_Code = Convert.ToString(row["Proj_Code"]);
                        objWOL.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);
                        list.Add(objWOL);
                    }
                }

            }
            catch { }
            return PartialView("_ProductionIssueList", list);
        }

        public JsonResult SetTempID(Int64 DetailsID, Int64 ProductionIssueID)
        {
            if (DetailsID > 0)
            {
                TempData["DetailsID"] = DetailsID;
                TempData["ProductionIssueID"] = ProductionIssueID;
                TempData.Keep();
            }
            else
            {
                TempData["DetailsID"] = null;
                TempData["ProductionIssueID"] = null;
                TempData.Clear();
            }
            return Json(true);
        }

        public ActionResult GetProductionIssueDetailsProductList(Int64 DetailsID = 0)
        {
            BOMProduct bomproductdataobj = new BOMProduct();
            List<BOMProduct> bomproductdata = new List<BOMProduct>();
            try
            {
                if (TempData["DetailsID"] != null)
                {
                    DetailsID = Convert.ToInt64(TempData["DetailsID"]);
                    TempData.Keep();
                }
                if (DetailsID > 0)
                {
                    DataTable objData = new DataTable();
                    if (TempData["IssueReturnID"] != null)
                    {
                        objData = objIR.GetIssueReturnData("GetBOMIssueReturnData", Convert.ToInt64(TempData["IssueReturnID"]), DetailsID);
                    }
                    else if (TempData["ProductionIssueID"] != null)
                    {
                        objData = objIR.GetIssueReturnData("GetBOMProductionIssueData", Convert.ToInt64(TempData["ProductionIssueID"]), DetailsID);
                    }
                    else
                    {
                        objData = objdata.GetBOMProductEntryListByID("GetBOMEntryProductsData", DetailsID);
                    }
                    if (objData != null && objData.Rows.Count > 0)
                    {
                        DataTable dt = objData;
                        foreach (DataRow row in dt.Rows)
                        {
                            bomproductdataobj = new BOMProduct();
                            bomproductdataobj.SlNO = Convert.ToString(row["SlNO"]);
                            bomproductdataobj.BOMProductsID = Convert.ToString(row["BOMProductsID"]);
                            bomproductdataobj.Details_ID = Convert.ToString(row["Details_ID"]);
                            bomproductdataobj.ProductName = Convert.ToString(row["sProducts_Code"]);
                            bomproductdataobj.ProductId = Convert.ToString(row["ProductID"]);
                            bomproductdataobj.ProductDescription = Convert.ToString(row["sProducts_Name"]);
                            bomproductdataobj.DesignNo = Convert.ToString(row["DesignNo"]);
                            bomproductdataobj.ItemRevisionNo = Convert.ToString(row["ItemRevisionNo"]);
                            bomproductdataobj.ProductQty = Convert.ToString(row["StkQty"]);
                            bomproductdataobj.ProductUOM = Convert.ToString(row["StkUOM"]);
                            bomproductdataobj.Warehouse = Convert.ToString(row["WarehouseName"]);
                            bomproductdataobj.Price = Convert.ToString(row["Price"]);
                            bomproductdataobj.Amount = Convert.ToString(row["Amount"]);
                            bomproductdataobj.BOMNo = Convert.ToString(row["BOMNo"]);
                            bomproductdataobj.RevNo = Convert.ToString(row["RevNo"]);
                            if (row["RevDate"] != null && Convert.ToString(row["RevDate"]) != "" && Convert.ToString(row["RevDate"]) != " " && Convert.ToString(row["RevDate"]) != null)
                            {
                                bomproductdataobj.RevDate = Convert.ToDateTime(row["RevDate"]).ToString("dd-MM-yyyy");
                            }
                            else
                            {
                                bomproductdataobj.RevDate = " ";
                            }
                            bomproductdataobj.Remarks = Convert.ToString(row["Remarks"]);
                            bomproductdataobj.ProductsWarehouseID = Convert.ToString(row["WarehouseID"]);
                            bomproductdataobj.Tag_Details_ID = Convert.ToString(row["Tag_Details_ID"]);
                            bomproductdataobj.Tag_Production_ID = Convert.ToString(row["Tag_Production_ID"]);
                            bomproductdataobj.RevNo = Convert.ToString(row["RevNo"]);
                            bomproductdataobj.Product_NegativeStock = Convert.ToString(row["Product_NegativeStock"]);
                            bomproductdataobj.AvlStk = Convert.ToString(row["AvlStk"]);

                            if (TempData["ProductionIssueID"] != null || TempData["IssueReturnID"] != null)
                            {
                                bomproductdataobj.OLDQty = Convert.ToString(row["OLDQty"]);
                                bomproductdataobj.BalQty = Convert.ToString(row["BalQty"]);
                            }
                            else
                            {
                                bomproductdataobj.OLDQty = Convert.ToString(row["StkQty"]);
                            }
                            if (TempData["ProductionIssueID"] != null || TempData["IssueReturnID"] != null)
                            {
                                bomproductdataobj.OLDAmount = Convert.ToString(row["OLDAmount"]);
                            }
                            else
                            {
                                bomproductdataobj.OLDAmount = Convert.ToString(row["Amount"]);
                            }
                            bomproductdataobj.InventoryType = Convert.ToString(row["InventoryType"]);

                            bomproductdata.Add(bomproductdataobj);
                        }
                        ViewData["BOMEntryProductsTotalAm"] = bomproductdata.Sum(x => Convert.ToDecimal(x.Amount)).ToString();
                    }
                }
            }
            catch { }
            return PartialView("_IssueReturnBOMProductGrid", bomproductdata);
        }

        [HttpPost]
        public JsonResult setStockWarehouseList(List<udtStockProduct> items, String SrlNo, Int64 Unit)
        {
            Boolean Success = false;
            String Message = "";
            Boolean IsProcess = false;

            try
            {
                DataTable dt = ToDataTable(items);
                Session["PIssue_WarehouseData"] = dt;

                DataTable dtWarehouse = ToDataTable(items.Where(x => x.Product_SrlNo == SrlNo).ToList());
                if (dtWarehouse.Columns.Contains("ViewMfgDate"))
                {
                    dtWarehouse.Columns.Remove("ViewMfgDate");
                }
                if (dtWarehouse.Columns.Contains("ViewExpiryDate"))
                {
                    dtWarehouse.Columns.Remove("ViewExpiryDate");
                }
                dtWarehouse.AcceptChanges();
                DataTable result = objPI.GetProductionIssueData("StockWarehouseBalCheck", 0, Unit, dtWarehouse);
                foreach (DataRow dr in result.Rows)
                {
                    IsProcess = Convert.ToBoolean(dr["IsProcess"]);
                }

                Success = true;
            }
            catch { }
            Message = Success.ToString() + "|" + IsProcess.ToString();
            return Json(Message);
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        [HttpPost]
        public JsonResult getProductSerialNo(String productid = "0", String warehouse = "0", String Batch = "0", String PostingDate = null)
        {
            List<ProductSerial> list = new List<ProductSerial>();
            try
            {
                DataTable dt = new DataTable();
                dt = objIR.GetManufacturingProductionIssue("GetSerialByProductID", productid, Convert.ToString(Session["LastFinYear"]), null, Convert.ToString(Session["LastCompany"]), null, warehouse, Batch, "0", PostingDate);
                if (dt.Rows.Count > 0)
                {
                    ProductSerial obj = new ProductSerial();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new ProductSerial();
                        obj.SerialID = Convert.ToString(item["SerialID"]);
                        obj.SerialValue = Convert.ToString(item["SerialName"]);
                        list.Add(obj);
                    }
                }

            }
            catch { }
            return Json(list);
        }


        [HttpPost]
        public JsonResult getProductWiseWarehouseRecord(String branchid = null, String productid = null)
        {
            List<BranchWarehouse> list = new List<BranchWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                dt = objIR.GetManufacturingProductionIssue("GetWareHouseByProductID", productid, Convert.ToString(Session["LastFinYear"]), branchid, Convert.ToString(Session["LastCompany"]), multiwarehouse);
                if (dt.Rows.Count > 0)
                {
                    BranchWarehouse obj = new BranchWarehouse();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new BranchWarehouse();
                        obj.WarehouseID = Convert.ToString(item["WarehouseID"]);
                        obj.WarehouseName = Convert.ToString(item["WarehouseName"]);
                        list.Add(obj);
                    }
                }
            }
            catch { }
            return Json(list);
        }


        [HttpPost]
        public JsonResult getBatchRecord(String warehouseid = null, String ProductID = null)
        {
            List<BatchWarehouse> list = new List<BatchWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                dt = objPI.GetManufacturingProductionIssue("GetBatchByProductIDWarehouse", ProductID, Convert.ToString(Session["LastFinYear"]), null, Convert.ToString(Session["LastCompany"]), multiwarehouse, warehouseid);
                if (dt.Rows.Count > 0)
                {
                    BatchWarehouse obj = new BatchWarehouse();
                    foreach (DataRow item in dt.Rows) 
                    {
                        obj = new BatchWarehouse();
                        obj.BatchID = Convert.ToString(item["BatchID"]);
                        obj.BatchName = Convert.ToString(item["BatchName"]);
                        list.Add(obj);
                    }
                }
            }
            catch { }
            return Json(list);
        }

        [HttpPost]
        public String GetConfigSettingRights(string VariableName)
        {
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable dt = objSlaesActivitiesBL.GetConfigSettingsFIFOWise(VariableName);
            string Variable_Val = "";
            string TempVar = "";
            if (dt.Rows.Count > 0)
            {
                TempVar = Convert.ToString(dt.Rows[0]["Variable_Value"]);
                if (TempVar.ToUpper() == "YES")
                {
                    Variable_Val = "1";
                }
                else
                {
                    Variable_Val = "0";
                }
            }

            return Variable_Val;
        }

        [HttpPost]
        public String GetMfgDate(string Batch = "0", string ProductionIssueID="0")
        {
            string MfgDate = "", ExpiryDate = "";
            ProcedureExecute proc = new ProcedureExecute("prc_ManufacturingProductionIssue_Get");
            proc.AddVarcharPara("@Action", 500, "GetBatchDetails");
            proc.AddVarcharPara("@BatchID", 100, Convert.ToString(Batch));
            proc.AddVarcharPara("@ProductionIssueID", 100, Convert.ToString(ProductionIssueID));
            DataTable Batchdt = proc.GetTable();

            if (Batchdt != null && Batchdt.Rows.Count > 0)
            {
                MfgDate = Convert.ToString(Batchdt.Rows[0]["MfgDate"]);
                ExpiryDate = Convert.ToString(Batchdt.Rows[0]["ExpiryDate"]);
            }

            return MfgDate + "~" + ExpiryDate;
        }


        [HttpPost]
        public JsonResult getWarehouseRecord(Int32 branchid = 0)
        {
            List<BranchWarehouse> list = new List<BranchWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                string strBranch = branchid.ToString();
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                if (multiwarehouse != "1")
                {
                    dt = oDBEngine.GetDataTable("select  bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + strBranch + "') order by bui_Name");
                }
                else
                {
                    dt = oDBEngine.GetDataTable("EXEC [GET_BRANCHWISEWAREHOUSE] '1','" + strBranch + "'");
                }
                if (dt.Rows.Count > 0)
                {
                    BranchWarehouse obj = new BranchWarehouse();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new BranchWarehouse();
                        obj.WarehouseID = Convert.ToString(item["WarehouseID"]);
                        obj.WarehouseName = Convert.ToString(item["WarehouseName"]);
                        list.Add(obj);
                    }
                }
            }
            catch { }
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetWarehouseList(Int64 IssueReturnID = 0)
        {
            List<ProductionIssueStkWarehouse> list = new List<ProductionIssueStkWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                if (IssueReturnID > 0)
                {
                    dt = objIR.GetIssueReturnData("GetIssueReturnWarehouseData", IssueReturnID, 0);
                    if (dt.Rows.Count > 0)
                    {
                        ProductionIssueStkWarehouse obj = new ProductionIssueStkWarehouse();
                        foreach (DataRow item in dt.Rows)
                        {
                            obj = new ProductionIssueStkWarehouse();
                            obj.StkWarehouseID = Convert.ToInt64(item["StkWarehouseID"]);
                            obj.ProductionIssueID = Convert.ToInt64(item["IssueReturnID"]);
                            obj.ProductionIssueDetailsID = Convert.ToInt64(item["IssueReturnDetailsID"]);
                            obj.Batch = Convert.ToString(item["Batch"]);
                            obj.IsOutStatus = Convert.ToString(item["IsOutStatus"]);
                            obj.LoopID = Convert.ToString(item["LoopID"]);
                            obj.Product_SrlNo = Convert.ToString(item["Product_SrlNo"]);
                            obj.Quantity = Convert.ToString(item["Quantity"]);

                            obj.SalesQuantity = Convert.ToString(item["SalesQuantity"]);
                            obj.SrlNo = Convert.ToString(item["SrlNo"]);
                            obj.Status = Convert.ToString(item["Status"]);
                            obj.WarehouseID = Convert.ToString(item["WarehouseID"]);

                            obj.WarehouseName = Convert.ToString(item["WarehouseName"]);
                            obj.SerialNo = Convert.ToString(item["SerialNo"]);
                            obj.ProductID = Convert.ToString(item["ProductID"]);

                            obj.MfgDate = Convert.ToString(item["MfgDate"]);
                            obj.ExpiryDate = Convert.ToString(item["ExpiryDate"]);

                            list.Add(obj);
                        }
                    }
                }

            }
            catch { }
            return Json(list);
        }

        public JsonResult SetPIDataByID(Int64 IssueReturnID = 0)
        {
            Boolean Success = false;
            try
            {
                TempData.Clear();

                TempData["IssueReturnID"] = IssueReturnID;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }

        public JsonResult SetBOMDateFilter(Int64 unitid, string FromDate, string ToDate)
        {
            Boolean Success = false;
            try
            {
                TempData["BranchID"] = unitid;
                TempData["FromDate"] = FromDate;
                TempData["ToDate"] = ToDate;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }

        public ActionResult ExportGridList(int type)
        {
            ViewData["DetailsListDataTable"] = TempData["DetailsListDataTable"];

            TempData.Keep();

            if (ViewData["DetailsListDataTable"] != null)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetBOMGridView(ViewData["DetailsListDataTable"]), ViewData["DetailsListDataTable"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetBOMGridView(ViewData["DetailsListDataTable"]), ViewData["DetailsListDataTable"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetBOMGridView(ViewData["DetailsListDataTable"]), ViewData["DetailsListDataTable"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetBOMGridView(ViewData["DetailsListDataTable"]), ViewData["DetailsListDataTable"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetBOMGridView(ViewData["DetailsListDataTable"]), ViewData["DetailsListDataTable"]);
                    default:
                        break;
                }
            }
            return null;
        }

        private GridViewSettings GetBOMGridView(object datatable)
        {
            //List<EmployeesTargetSetting> obj = (List<EmployeesTargetSetting>)datatablelist;
            //ListtoDataTable lsttodt = new ListtoDataTable();
            //DataTable datatable = ConvertListToDataTable(obj); 
            var settings = new GridViewSettings();
            settings.Name = "Issue Return";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "IssueForProduction";
            //String ID = Convert.ToString(TempData["EmployeesTargetListDataTable"]);
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "Issue_No" || datacolumn.ColumnName == "Issue_Date" || datacolumn.ColumnName == "WorkOrderNo" || datacolumn.ColumnName == "WorkOrderDate"
                    || datacolumn.ColumnName == "ProductionIssueNo" || datacolumn.ColumnName == "ProductionIssueDate"
                    || datacolumn.ColumnName == "ProductionOrderNo" || datacolumn.ColumnName == "ProductionOrderDate" || datacolumn.ColumnName == "BOM_No" || datacolumn.ColumnName == "BOM_Date"
                    || datacolumn.ColumnName == "REV_No"
                    || datacolumn.ColumnName == "REV_Date" || datacolumn.ColumnName == "CreatedBy" || datacolumn.ColumnName == "ModifyBy" || datacolumn.ColumnName == "CreateDate" || datacolumn.ColumnName == "ModifyDate")
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "Issue_No")
                        {
                            column.Caption = "Issue No";
                            column.VisibleIndex = 0;
                        }
                        else if (datacolumn.ColumnName == "Issue_Date")
                        {
                            column.Caption = "Issue Date";
                            column.VisibleIndex = 1;
                        }
                        else if (datacolumn.ColumnName == "WorkOrderNo")
                        {
                            column.Caption = "Work Order No";
                            column.VisibleIndex = 2;
                        }
                        else if (datacolumn.ColumnName == "WorkOrderDate")
                        {
                            column.Caption = "Work Order Date";
                            column.VisibleIndex = 3;
                        }
                        else if (datacolumn.ColumnName == "ProductionOrderNo")
                        {
                            column.Caption = "Production Order No";
                            column.VisibleIndex = 4;

                        }
                        else if (datacolumn.ColumnName == "ProductionOrderDate")
                        {
                            column.Caption = "Production Order Date";
                            column.VisibleIndex = 5;
                        }
                        else if (datacolumn.ColumnName == "ProductionIssueNo")
                        {
                            column.Caption = "Production Issue No";
                            column.VisibleIndex = 6;

                        }
                        else if (datacolumn.ColumnName == "ProductionIssueDate")
                        {
                            column.Caption = "Production Issue Date";
                            column.VisibleIndex = 7;
                        }
                        else if (datacolumn.ColumnName == "BOM_No")
                        {
                            column.Caption = "BOM No";
                            column.VisibleIndex = 8;
                        }
                        else if (datacolumn.ColumnName == "BOM_Date")
                        {
                            column.Caption = "BOM Date";
                            column.VisibleIndex = 9;
                        }
                        else if (datacolumn.ColumnName == "REV_No")
                        {
                            column.Caption = "Rev No.";
                            column.VisibleIndex = 10;
                        }
                        else if (datacolumn.ColumnName == "REV_Date")
                        {
                            column.Caption = "Rev Date";
                            column.VisibleIndex = 11;
                        }
                        else if (datacolumn.ColumnName == "CreatedBy")
                        {
                            column.Caption = "Entered By";
                            column.VisibleIndex = 12;
                        }
                        else if (datacolumn.ColumnName == "ModifyBy")
                        {
                            column.Caption = "Modified By";
                            column.VisibleIndex = 13;
                        }
                        else if (datacolumn.ColumnName == "CreateDate")
                        {
                            column.Caption = "Entered On";
                            column.VisibleIndex = 14;
                        }
                        else if (datacolumn.ColumnName == "ModifyDate")
                        {
                            column.Caption = "Modified On";
                            column.VisibleIndex = 15;
                        }
                        //else
                        //{
                        //    column.Caption = datacolumn.ColumnName;
                        //}
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

        public JsonResult RemovePIDataByID(Int32 IssueReturnID)
        {
            Boolean Success = false;
            try
            {
                var datasetobj = objIR.GetIssueReturnData("RemoveIRData", IssueReturnID, 0);
                if (datasetobj.Rows.Count > 0)
                {

                    foreach (DataRow item in datasetobj.Rows)
                    {
                        Success = Convert.ToBoolean(item["Success"]);
                    }
                }
            }
            catch { }
            return Json(Success);
        }

        public ActionResult GetIssueReturnList()
        {
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            List<IssueReturnViewModel> list = new List<IssueReturnViewModel>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/IssueReturnList", "IssueReturn");
            try
            {
                Int64 BranchID = 0;
                DateTime? FromDate = null;
                DateTime? ToDate = null;
                DataTable dt = new DataTable();
                if (TempData["BranchID"] != null && TempData["FromDate"] != null && TempData["ToDate"] != null)
                {
                    BranchID = Convert.ToInt64(TempData["BranchID"]);
                    FromDate = Convert.ToDateTime(TempData["FromDate"]);
                    ToDate = Convert.ToDateTime(TempData["ToDate"]);
                    TempData.Keep();
                }
                if (TempData["BranchID"] != null && TempData["FromDate"] != null && TempData["ToDate"] != null)
                {
                    if (BranchID > 0)
                    {
                        dt = oDBEngine.GetDataTable("select * from V_IssueReturnList where BRANCH_ID =" + BranchID + " AND (Issue_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') ORDER BY IssueReturnID DESC");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("select * from V_IssueReturnList where Issue_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' ORDER BY IssueReturnID DESC");
                    }

                }
                //else
                //{
                //    dt = oDBEngine.GetDataTable("select * from V_ProductionOrderList");
                //}


                TempData["DetailsListDataTable"] = dt;

                //var dt = oDBEngine.GetDataTable("select * from V_ProductionOrderList");
                if (dt.Rows.Count > 0)
                {
                    IssueReturnViewModel obj = new IssueReturnViewModel();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new IssueReturnViewModel();
                        obj.IssueReturnID = Convert.ToInt64(item["IssueReturnID"]);
                        obj.ProductionIssueID = Convert.ToInt64(item["ProductionIssueID"]);
                        obj.ProductionOrderID = Convert.ToInt64(item["ProductionOrderID"]);
                        obj.WorkCenterID = Convert.ToInt64(item["WorkCenterID"]);
                        obj.Issue_No = Convert.ToString(item["Issue_No"]);
                        obj.Issue_Qty = Convert.ToDecimal(item["Issue_Qty"]);
                        obj.WorkCenterCode = Convert.ToString(item["WorkCenterCode"]);
                        obj.WorkCenterDescription = Convert.ToString(item["WorkCenterDescription"]);
                        obj.BRANCH_ID = Convert.ToInt64(item["BRANCH_ID"]);
                        obj.BOMNo = Convert.ToString(item["BOM_No"]);
                        obj.RevNo = Convert.ToString(item["REV_No"]);

                        obj.ProductionIssueNo = Convert.ToString(item["ProductionIssueNo"]);
                        obj.ProductionIssueDate = Convert.ToDateTime(item["ProductionIssueDate"]);

                        obj.WorkOrderNo = Convert.ToString(item["WorkOrderNo"]);
                       // obj.WorkOrderDate = Convert.ToDateTime(item["WorkOrderDate"]);
                        if (Convert.ToString(item["WorkOrderDate"]) != "")
                        {
                            obj.WorkOrderDate = Convert.ToDateTime(item["WorkOrderDate"]);
                        }
                        else
                        {
                            obj.WorkOrderDate = null;
                        }


                        obj.BOM_Date = Convert.ToDateTime(item["BOM_Date"]);
                        obj.ProductionOrderNo = Convert.ToString(item["ProductionOrderNo"]);
                        obj.dtProductionOrderDate = Convert.ToDateTime(item["ProductionOrderDate"]);

                        if (Convert.ToString(item["REV_Date"]) != "")
                        {
                            obj.REV_Date = Convert.ToDateTime(item["REV_Date"]);
                        }
                        else
                        {
                            obj.REV_Date = null;
                        }

                        if (Convert.ToString(item["Issue_Date"]) != "")
                        {
                            obj.Issue_Date = Convert.ToDateTime(item["Issue_Date"]);
                        }

                        obj.CreatedBy = Convert.ToString(item["CreatedBy"]);
                        obj.ModifyBy = Convert.ToString(item["ModifyBy"]);
                        obj.CreateDate = Convert.ToDateTime(item["CreateDate"]);

                        if (Convert.ToString(item["ModifyDate"]) != "")
                        {
                            obj.ModifyDate = Convert.ToDateTime(item["ModifyDate"]);
                        }
                        else
                        {
                            obj.ModifyDate = null;
                        }
                        obj.PartNoName = Convert.ToString(item["PartNoName"]);
                        obj.DesignNo = Convert.ToString(item["DesignNo"]);
                        obj.ItemRevNo = Convert.ToString(item["ItemRevisionNo"]);
                        obj.Description = Convert.ToString(item["sProducts_Name"]);
                        obj.Proj_Code = Convert.ToString(item["Proj_Code"]);
                        obj.Proj_Name = Convert.ToString(item["Proj_Name"]);
                        ViewBag.ProjectShow = ProjectSelectInEntryModule;
                        list.Add(obj);
                    }
                }
            }
            catch { }
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            return PartialView("_IssueReturnDataList", list);
        }

        [ValidateInput(false)]
        public ActionResult BatchEditingIssueReturn(DevExpress.Web.Mvc.MVCxGridViewBatchUpdateValues<BOMProduct, int> updateValues, IssueReturnViewModel options)
        {
            TempData["Count"] = (int)TempData["Count"] + 1;
            TempData.Keep();
            String NumberScheme = "";
            String Message = "";

            if ((int)TempData["Count"] != 2)
            {
                Boolean IsProcess = false;
                List<BOMProduct> list = new List<BOMProduct>();
                if (updateValues.Update.Count > 0 && Convert.ToInt64(options.Details_ID) > 0)
                {
                    List<udtProductionIssueDetails> udtlist = new List<udtProductionIssueDetails>();
                    udtProductionIssueDetails obj = null;

                    foreach (var item in updateValues.Update)
                    {
                        if (Convert.ToInt64(item.BOMProductsID) > 0)
                        {
                            obj = new udtProductionIssueDetails();
                            obj.ProductsID = Convert.ToInt64(item.ProductId);
                            obj.BOMProductsID = Convert.ToInt64(item.BOMProductsID);
                            obj.Qty = Convert.ToDecimal(item.ProductQty);
                            obj.Amount = Convert.ToDecimal(item.Amount);
                            udtlist.Add(obj);
                        }
                    }
                    if (udtlist.Count > 0)
                    {
                        if (options.IssueReturnID > 0)
                        {
                            IsProcess = IssueReturnBOMProductInsertUpdate(udtlist, options);
                        }
                        else
                        {
                            NumberScheme = checkNMakeIRCode(options.Issue_No, Convert.ToInt32(options.Issue_SchemaID), Convert.ToDateTime(options.Issue_Date));
                            if (NumberScheme == "ok")
                            {
                                IsProcess = IssueReturnBOMProductInsertUpdate(udtlist, options);
                            }
                            else
                            {
                                Message = NumberScheme;
                            }
                        }

                        TempData["DetailsID"] = null;
                    }
                }


                TempData["Count"] = 1;
                TempData.Keep();
                ViewData["OrderNo"] = JVNumStr;
                ViewData["Success"] = IsProcess;
                ViewData["Message"] = Message;
            }
            return PartialView("_IssueReturnBOMProductGrid", updateValues.Update);
        }

        public string checkNMakeIRCode(string manual_str, int sel_schema_Id, DateTime RevisionDate)
        {
            DataTable dtSchema = new DataTable();
            DataTable dtC = new DataTable();
            string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
            int EmpCode, prefLen, sufxLen, paddCounter;
            bool suppressZero = false;

            if (sel_schema_Id > 0)
            {
                dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type,suppressZero", "id=" + sel_schema_Id);
                int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);

                if (scheme_type != 0)
                {
                    startNo = dtSchema.Rows[0]["startno"].ToString();
                    paddCounter = Convert.ToInt32(dtSchema.Rows[0]["digit"]);
                    paddedStr = startNo.PadLeft(paddCounter, '0');
                    prefCompCode = (dtSchema.Rows[0]["prefix"].ToString() == "TCURDATE/") ? (RevisionDate.ToString("ddMMyyyy") + "/") : (dtSchema.Rows[0]["prefix"].ToString());
                    sufxCompCode = (dtSchema.Rows[0]["suffix"].ToString() == "/TCURDATE") ? ("/" + RevisionDate.ToString("ddMMyyyy")) : (dtSchema.Rows[0]["suffix"].ToString());
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);
                    suppressZero = Convert.ToBoolean(dtSchema.Rows[0]["suppressZero"]);

                    if ((dtSchema.Rows[0]["prefix"].ToString() == "TCURDATE/") || (dtSchema.Rows[0]["suffix"].ToString() == "/TCURDATE"))
                    {
                        sqlQuery = "SELECT max(tjv.Issue_No) FROM IssueReturn tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Issue_No))) = 1 and Issue_No like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.Issue_No) FROM IssueReturn tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Issue_No))) = 1 and Issue_No like '%" + sufxCompCode + "'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }

                        if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                        {
                            string uccCode = dtC.Rows[0][0].ToString().Trim();
                            int UCCLen = uccCode.Length;
                            int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                            string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                            EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
                            // out of range journal scheme
                            if (EmpCode.ToString().Length > paddCounter)
                            {
                                return "outrange";
                            }
                            else
                            {
                                paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                                JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                                return "ok";
                            }
                        }
                        else
                        {
                            JVNumStr = startNo.PadLeft(paddCounter, '0');
                            JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {

                        if (!suppressZero)
                        {
                            sqlQuery = "SELECT max(tjv.Issue_No) FROM IssueReturn tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + paddCounter + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Issue_No))) = 1 and Issue_No like '" + prefCompCode + "%'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }
                        else
                        {
                            int i = startNo.Length;
                            while (i < paddCounter)
                            {


                                sqlQuery = "SELECT max(tjv.Issue_No) FROM IssueReturn tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + i + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.Issue_No))) = 1 and Issue_No like '" + prefCompCode + "%'";

                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.Issue_No)=" + i;
                                }

                                dtC = oDBEngine.GetDataTable(sqlQuery);
                                if (dtC.Rows[0][0].ToString() == "")
                                {
                                    break;
                                }
                                i++;
                            }
                            if (i != 1)
                            {
                                sqlQuery = "SELECT max(tjv.Issue_No) FROM IssueReturn tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + (i - 1) + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.Issue_No))) = 1 and Issue_No like '" + prefCompCode + "%'";
                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.Issue_No)=" + (i - 1);
                                }
                                dtC = oDBEngine.GetDataTable(sqlQuery);
                            }

                        }

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.Issue_No) FROM IssueReturn tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Issue_No))) = 1 and Issue_No like '" + prefCompCode + "%'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }

                        if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                        {
                            string uccCode = dtC.Rows[0][0].ToString().Trim();
                            int UCCLen = uccCode.Length;
                            int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                            string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                            EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
                            // out of range journal scheme
                            if (EmpCode.ToString().Length > paddCounter)
                            {
                                return "outrange";
                            }
                            else
                            {
                                if (!suppressZero)
                                    paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                                else
                                    paddedStr = EmpCode.ToString();

                                JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                                return "ok";
                            }
                        }
                        else
                        {
                            if (!suppressZero)
                                paddedStr = startNo.PadLeft(paddCounter, '0');
                            else
                                paddedStr = startNo;

                            JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                }
                else
                {
                    sqlQuery = "SELECT Issue_No FROM IssueReturn WHERE Issue_No LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    JVNumStr = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }


        public Boolean IssueReturnBOMProductInsertUpdate(List<udtProductionIssueDetails> obj, IssueReturnViewModel obj2)
        {
            Boolean Success = false;
            try
            {
                DataTable dtBOM_PRODUCTS = new DataTable();
                dtBOM_PRODUCTS = ToDataTable(obj);
                DataTable dtWarehouse = new DataTable();
                DataTable dtWarehouseFresh = new DataTable();

                if (Session["PIssue_WarehouseData"] != null)
                {
                    dtWarehouse = (DataTable)Session["PIssue_WarehouseData"];
                    dtWarehouseFresh = dtWarehouse;
                }

                if (dtWarehouseFresh != null)
                {
                    if (dtWarehouseFresh.Rows.Count > 0)
                    {
                        int LoopCount = 0;
                        foreach (DataRow row in dtWarehouseFresh.Rows)
                        {
                            row["Quantity"] = Convert.ToDecimal(obj[LoopCount].Qty);
                            row["WarehouseID"] = obj2.WorkCenterID;
                            LoopCount++;
                        }
                    }

                }


                DataSet dt = new DataSet();
                if (Convert.ToInt64(obj2.Details_ID) > 0)
                {
                    if (!String.IsNullOrEmpty(obj2.Issue_No) && obj2.Issue_No.ToLower() != "auto")
                    {
                        JVNumStr = obj2.Issue_No;
                    }
                    dt = objIR.IssueReturnBOMProductInsertUpdate("INSERTIRBOM", obj2.IssueReturnID, obj2.ProductionIssueID, obj2.WorkOrderID, obj2.ProductionOrderID, obj2.Details_ID, Convert.ToInt64(obj2.WorkCenterID), JVNumStr, obj2.Issue_SchemaID, Convert.ToDateTime(obj2.Issue_Date),
                        obj2.Issue_Qty, obj2.TotalCost, obj2.BRANCH_ID, Convert.ToInt64(Session["userid"]), Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]),
                        obj2.strRemarks,obj2.PartNo,
                        dtBOM_PRODUCTS,dtWarehouseFresh, dtWarehouse);
                }
                Session["PIssue_WarehouseData"] = null;
                //else
                //{
                //    dt = objdata.BOMProductEntryInsertUpdate("INSERTMAINPRODUCT", JVNumStr, Convert.ToDateTime(obj2.BOMDate), Convert.ToInt64(obj2.FinishedItem), Convert.ToDecimal(obj2.FinishedQty), obj2.FinishedUom, obj2.BOMType, obj2.RevisionNo, Convert.ToDateTime(obj2.RevisionDate), Convert.ToInt32(obj2.Unit), Convert.ToInt32(obj2.WarehouseID)
                //        , dtBOM_PRODUCTS, new DataTable(), Convert.ToInt32(obj2.BOM_SCHEMAID), Convert.ToDecimal(obj2.ActualAdditionalCost), Convert.ToInt64(Session["userid"]));
                //    //dt = objemployee.EmployeesTargetByCodeInsertUpdate(obj.EmployeeTargetSettingID, obj2.EmpTypeID, obj2.CounterType, obj.EmployeeCode, obj2.SettingMonth, obj2.SettingYear, obj.OrderValue, obj.NewCounter, obj.Collection, obj.Revisit);
                //}
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        Success = Convert.ToBoolean(row["Success"]);
                    }
                }
            }
            catch { }
            return Success;
        }
    }
}