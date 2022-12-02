using BusinessLogicLayer;
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
    public class WastageController : Controller
    {
        WastageViewModel WVM = null;
        BOMEntryModel objdata = null;
        WorkOrderModel objWO = null;
        StockReceiptModel objSRM = null;
        StockReceiptViewModel objSR = null;
        WastageModel WM = null;
        ProductionIssueModel objPI = null;
        String JVNumStr = String.Empty;
        DBEngine oDBEngine = new DBEngine();
        UserRightsForPage rights = new UserRightsForPage();
        CommonBL cSOrder = new CommonBL();
        public WastageController()
        {
            WVM = new WastageViewModel();
            objdata = new BOMEntryModel();
            objWO = new WorkOrderModel();
            objSRM = new StockReceiptModel();
            objSR = new StockReceiptViewModel();
            WM = new WastageModel();
            objPI = new ProductionIssueModel();
        }
        //
        // GET: /Wastage/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WastageEntry()
        {
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");        
            //TempData["StockReceiptID"] = null;
            //TempData.Clear();
            List<BranchUnit> list = new List<BranchUnit>();
            var datasetobj = objdata.DropDownDetailForBOM("GetUnitDropDownData", Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["userbranchHierarchy"]), 0, 0);
            if (datasetobj.Tables[0].Rows.Count > 0)
            {
                BranchUnit obj = new BranchUnit();
                foreach (DataRow item in datasetobj.Tables[0].Rows)
                {
                    obj = new BranchUnit();
                    obj.BranchID = Convert.ToString(item["BANKBRANCH_ID"]);
                    obj.BankBranchName = Convert.ToString(item["BANKBRANCH_NAME"]);
                    list.Add(obj);
                }
            }
            WVM.UnitList = list;

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

            try
            {
                if (TempData["WastageID"] != null)
                {
                    WVM.WastageID = Convert.ToInt64(TempData["WastageID"]);
                    TempData.Keep();

                    if (WVM.WastageID > 0)
                    {
                        DataTable objData = WM.GetWastageData("GetWastageData", WVM.WastageID, 0);
                        if (objData != null && objData.Rows.Count > 0)
                        {
                            DataTable dt = objData;
                            foreach (DataRow row in dt.Rows)
                            {
                                WVM.WastageID = Convert.ToInt64(row["WastageID"]);
                                WVM.WastageSchemaID = Convert.ToInt64(row["SchemaID"]);
                                WVM.Wastage_No = Convert.ToString(row["Wastage_No"]);
                                WVM.dtWastageDate = Convert.ToDateTime(row["Wastage_Date"]);
                                WVM.WastageWarehouseID = Convert.ToInt64(row["WastageWarehouseID"]);
                                WVM.WarehouseQty = Convert.ToDecimal(row["WastageQty"]);
                                WVM.StockReceiptID = Convert.ToInt64(row["StockReceiptID"]);
                                WVM.QualityControlID = Convert.ToInt64(row["QualityControlID"]);
                                WVM.ProductID = Convert.ToInt64(row["sProducts_ID"]);
                                WVM.InventoryType = Convert.ToString(row["InventoryType"]);
                                WVM.FGReceiptQty = Convert.ToDecimal(row["FG_Qty"]);
                                WVM.FreshQuantity = Convert.ToDecimal(row["Fresh_ReceiptQty"]);
                                WVM.RejectedQuantity = Convert.ToDecimal(row["Rejected_ReceiptQty"]);
                                WVM.BalFreshQuantity = Convert.ToDecimal(row["Fresh_Qty"]);
                                WVM.BalRejectedQuantity = Convert.ToDecimal(row["Rejected_Qty"]);
                                WVM.Product_NegativeStock = Convert.ToString(row["sProduct_NegativeStock"]);
                                WVM.AvlStk = Convert.ToDecimal(row["AvlStk"]);
                                WVM.WarehouseID = Convert.ToInt64(row["WarehouseID"]);
                                WVM.QC_SchemaID = Convert.ToInt64(row["QC_SchemaID"]);
                                WVM.QC_No = Convert.ToString(row["QC_No"]);
                                WVM.dtOrderDate = Convert.ToDateTime(row["QC_Date"]);
                                WVM.ProductionReceiptID = Convert.ToInt64(row["ProductionReceiptID"]);
                                WVM.WorkOrderID = Convert.ToInt64(row["WorkOrderID"]);
                                WVM.ProductionOrderID = Convert.ToInt64(row["ProductionOrderID"]);
                                WVM.WorkCenterID = Convert.ToString(row["WorkCenterID"]);
                                WVM.StockReceipt_No = Convert.ToString(row["Receipt_No"]);
                                WVM.dtStockReceiptDate = Convert.ToDateTime(row["Receipt_Date"]);
                                WVM.StockReceipt_Date = Convert.ToDateTime(row["Receipt_Date"]).ToString("dd-MM-yyyy");
                                WVM.Receipt_No = Convert.ToString(row["ProductionReceiptNo"]);
                                WVM.Receipt_Date = Convert.ToDateTime(row["ProductionReceiptDate"]);
                                WVM.ProductionIssueID = Convert.ToInt64(row["ProductionIssueID"]);
                                WVM.ProductionIssueNo = Convert.ToString(row["ProductionIssueNo"]);
                                WVM.ProductionIssueDate = Convert.ToDateTime(row["ProductionIssueDate"]);
                                WVM.WorkOrderNo = Convert.ToString(row["WorkOrderNo"]);
                                //objSR.Order_Qty = Convert.ToDecimal(row["Receipt_Qty"]);
                                WVM.WorkCenterCode = Convert.ToString(row["WorkCenterCode"]);
                                WVM.WorkCenterDescription = Convert.ToString(row["WorkCenterDescription"]);
                                WVM.BRANCH_ID = Convert.ToInt64(row["BRANCH_ID"]);
                                WVM.BOMNo = Convert.ToString(row["BOM_No"]);
                                WVM.RevNo = Convert.ToString(row["REV_No"]);
                                WVM.BOM_Date = Convert.ToDateTime(row["BOM_Date"]);
                                WVM.ProductionOrderNo = Convert.ToString(row["ProductionOrderNo"]);
                                WVM.ProductionOrderDate = Convert.ToDateTime(row["ProductionOrderDate"]);
                                WVM.WorkOrderDate = Convert.ToDateTime(row["WorkOrderDate"]);
                                WVM.FinishedItem = Convert.ToString(row["ProductName"]);
                                WVM.FinishedUom = Convert.ToString(row["FinishedUom"]);
                                WVM.Warehouse = Convert.ToString(row["Warehouse"]);
                                WVM.TotalAmount = Convert.ToDecimal(row["TotalAmount"]);
                                WVM.FGPrice = Convert.ToDecimal(row["FGPrice"]);
                                WVM.strRemarks = Convert.ToString(row["Remarks"]);
                                WVM.ProductDescription = Convert.ToString(row["ProductDescription"]);
                                WVM.Details_ID = Convert.ToInt64(row["Details_ID"]);
                                WVM.ReceiptSchemaID = Convert.ToInt64(row["SchemaID"]);
                                if (Convert.ToString(row["REV_Date"]) != "")
                                {
                                    WVM.REV_Date = Convert.ToDateTime(row["REV_Date"]);
                                }
                                else
                                {
                                    WVM.REV_Date = null;
                                }
                                WVM.CreatedBy = Convert.ToString(row["CreatedBy"]);
                                WVM.ModifyBy = Convert.ToString(row["ModifyBy"]);
                                WVM.CreateDate = Convert.ToDateTime(row["CreateDate"]);
                                if (Convert.ToString(row["ModifyDate"]) != "")
                                {
                                    WVM.ModifyDate = Convert.ToDateTime(row["ModifyDate"]);
                                }
                                else
                                {
                                    WVM.ModifyDate = null;
                                }
                                WVM.PartNoName = Convert.ToString(row["PartNoName"]);
                                WVM.DesignNo = Convert.ToString(row["DesignNo"]);
                                WVM.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                                WVM.Proj_Code = Convert.ToString(row["Proj_Code"]);
                                WVM.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);
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

                if (WVM.WastageID < 1)
                {
                    WVM.dtWastageDate = DateTime.Now;
                }

            }
            catch { }

            if (TempData["IsView"] != null)
            {
                ViewBag.IsView = Convert.ToInt16(TempData["IsView"]);
            }
            else
            {
                ViewBag.IsView = 0;
            }
            TempData["Count"] = 1;
            TempData.Keep();
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            ViewBag.LastCompany = Convert.ToString(Session["LastCompany"]);
            ViewBag.LastFinancialYear = Convert.ToString(Session["LastFinYear"]);

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


            return View(WVM);
        }

        public bool IsBarcodeGeneratete()
        {
            bool IsGeneratete = false;

            try
            {
                DBEngine objEngine = new DBEngine();
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

        public ActionResult GetWastageProductList()
        {
            BOMProduct bomproductdataobj = new BOMProduct();
            List<BOMProduct> bomproductdata = new List<BOMProduct>();
            try
            {
                Int64 StockReceiptID = 0;
                DataTable objData = new DataTable();
                if (TempData["StockReceiptID"] != null)
                {
                    StockReceiptID = (Int64)TempData["StockReceiptID"];
                }
                if (StockReceiptID > 0)
                {
                    objData = WM.GetWastageData("GetBOMProductionReceiptData", Convert.ToInt64(TempData["StockReceiptID"]), 0);

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

                            if (TempData["Production_ID"] != null && TempData["WorkOrderID"] == null)
                            {
                                bomproductdataobj.ProductQty = Convert.ToString(row["BalQty"]);
                                bomproductdataobj.BalQty = Convert.ToString(row["BalQty"]);
                                bomproductdataobj.Amount = Convert.ToString(Math.Round((Convert.ToDecimal(bomproductdataobj.BalQty) * Convert.ToDecimal(bomproductdataobj.Price)), 2));
                            }
                            //else
                            //{
                            //    bomproductdataobj.BalQty = Convert.ToString(row["StkQty"]);
                            //}

                            if (TempData["WorkOrderID"] != null)
                            {
                                bomproductdataobj.OLDQty = Convert.ToString(row["OLDQty"]);
                            }
                            else
                            {
                                bomproductdataobj.OLDQty = Convert.ToString(row["StkQty"]);
                            }
                            if (TempData["WorkOrderID"] != null)
                            {
                                bomproductdataobj.OLDAmount = Convert.ToString(row["OLDAmount"]);
                            }
                            else
                            {
                                bomproductdataobj.OLDAmount = Convert.ToString(row["Amount"]);
                            }


                            bomproductdata.Add(bomproductdataobj);
                        }
                        ViewData["BOMEntryProductsTotalAm"] = bomproductdata.Sum(x => Convert.ToDecimal(x.Amount)).ToString();
                    }

                }

                //BOMProduct obj = new BOMProduct();
                //obj.SlNO = "1";
                //bomproductdata.Add(obj);

            }
            catch { }
            TempData.Keep();
            return PartialView("_WastageProductGrid", bomproductdata);
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

        public JsonResult getNumberingSchemeRecord()
        {
            List<SchemaNumber> list = new List<SchemaNumber>();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            DataTable Schemadt = objdata.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "108", "Y");
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

        public ActionResult GetSRecList()
        {
            List<StockReceiptViewModel> list = new List<StockReceiptViewModel>();
            try
            {

                DataTable objData = objSRM.GetStockReceiptData("GetAllStockReceiptData", 0, 0);

                if (objData != null && objData.Rows.Count > 0)
                {
                    DataTable dt = objData;
                    foreach (DataRow row in dt.Rows)
                    {
                        objSR = new StockReceiptViewModel();
                        objSR.StockReceiptID = Convert.ToInt64(row["StockReceiptID"]);
                        objSR.FreshQuantity = Convert.ToDecimal(row["Fresh_ReceiptQty"]);
                        objSR.RejectedQuantity = Convert.ToDecimal(row["Rejected_ReceiptQty"]);
                        objSR.QualityControlID = Convert.ToInt64(row["QualityControlID"]);
                        objSR.FGQty = Convert.ToDecimal(row["FG_Qty"]);
                        objSR.ProductionReceiptID = Convert.ToInt64(row["ProductionReceiptID"]);
                        objSR.ProductionIssueID = Convert.ToInt64(row["ProductionIssueID"]);
                        objSR.ProductionIssueNo = Convert.ToString(row["ProductionIssueNo"]);
                        objSR.WorkOrderID = Convert.ToInt64(row["WorkOrderID"]);
                        //objWOL.OrderNo = Convert.ToString(row["OrderNo"]);
                        objSR.ProductionOrderNo = Convert.ToString(row["ProductionOrderNo"]);
                        objSR.ProductionOrderID = Convert.ToInt64(row["ProductionOrderID"]);
                        //objQVC.Details_ID = Convert.ToInt64(row["Details_ID"]);
                        objSR.BOMNo = Convert.ToString(row["BOM_No"]);
                        objSR.QC_No = Convert.ToString(row["QC_No"]);
                        objSR.dtOrderDate = Convert.ToDateTime(row["QC_Date"]);
                        objSR.ProductionIssueDate = Convert.ToDateTime(row["ProductionIssueDate"]);
                        objSR.RevNo = Convert.ToString(row["REV_No"]);
                        objSR.FinishedItem = Convert.ToString(row["ProductName"]);
                        objSR.FinishedUom = Convert.ToString(row["FinishedUom"]);
                        objSR.ProductionOrderDate = Convert.ToDateTime(row["ProductionOrderDate"]);
                        objSR.Warehouse = Convert.ToString(row["Warehouse"]);
                        //objQVC.ProductionIssueQty = Convert.ToDecimal(row["ProductionIssueQty"]);
                        objSR.WorkCenterID = Convert.ToString(row["WorkCenterID"]);
                        objSR.WorkCenterCode = Convert.ToString(row["WorkCenterCode"]);
                        objSR.WorkCenterDescription = Convert.ToString(row["WorkCenterDescription"]);
                        objSR.TotalAmount = Convert.ToDecimal(row["TotalAmount"]);
                        objSR.FGPrice = Convert.ToDecimal(row["FGPrice"]);
                        objSR.ProductDescription = Convert.ToString(row["ProductDescription"]);
                        objSR.strRemarks = Convert.ToString(row["Remarks"]);
                        objSR.BRANCH_ID = Convert.ToInt64(row["BRANCH_ID"]);
                        objSR.Receipt_No = Convert.ToString(row["Receipt_No"]);

                        objSR.ProductionReceiptNo = Convert.ToString(row["ProductionReceiptNo"]);
                        if (Convert.ToString(row["ProductionReceiptDate"]) != "")
                        {
                            objSR.ProductionReceiptDate = Convert.ToDateTime(row["ProductionReceiptDate"]);
                        }
                        else
                        {
                            objSR.ProductionReceiptDate = null;
                        }
                       // objSR.ProductionReceiptDate = Convert.ToDateTime(row["ProductionReceiptDate"]);
                        //objQVC.Order_Qty = Convert.ToDecimal(row["Receipt_Qty"]);
                        //objSR.Receipt_Date = Convert.ToDateTime(row["Receipt_Date"]);
                        if (Convert.ToString(row["Receipt_Date"]) != "")
                        {
                            objSR.Receipt_Date = Convert.ToDateTime(row["Receipt_Date"]);
                        }
                        else
                        {
                            objSR.Receipt_Date = null;
                        }


                        objSR.WorkOrderNo = Convert.ToString(row["WorkOrderNo"]);
                        objSR.ProductID = Convert.ToInt64(row["sProducts_ID"]);
                        objSR.InventoryType = Convert.ToString(row["InventoryType"]);
                        //objSR.FreshQuantity = Convert.ToDecimal(row["Fresh_Qty"]);
                        //objSR.RejectedQuantity = Convert.ToDecimal(row["Rejected_Qty"]);
                        //objSR.Bal_Fresh_Qty = Convert.ToDecimal(row["Bal_Fresh_Qty"]);
                        //objSR.Bal_Rejected_Qty = Convert.ToDecimal(row["Bal_Rejected_Qty"]);
                        objSR.Product_NegativeStock = Convert.ToString(row["sProduct_NegativeStock"]);
                        objSR.AvlStk = Convert.ToDecimal(row["AvlStk"]);
                        objSR.PartNoName = Convert.ToString(row["PartNoName"]);
                        objSR.DesignNo = Convert.ToString(row["DesignNo"]);
                        objSR.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                        objSR.WarehouseID = Convert.ToInt64(row["WarehouseID"]);
                        objSR.Proj_Code = Convert.ToString(row["Proj_Code"]);
                        objSR.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);

                        list.Add(objSR);
                    }
                }

            }
            catch { }
            return PartialView("_StockReceiptList", list);
        }

        [HttpPost]
        public Boolean SetStockReceiptID(Int64 StockReceiptID)
        {
            TempData["StockReceiptID"] = StockReceiptID;
            TempData.Keep();
            return true;
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
        public JsonResult setStockWarehouseList(List<udtStockProduct> items, String SrlNo, Int64 Unit)
        {
            Boolean Success = false;
            String Message = "";
            Boolean IsProcess = false;
            //SrlNo = "0";
            try
            {
                List<udtStockProduct> templist = items.Where(x => x.Product_SrlNo == SrlNo).ToList();
                DataTable dt = ToDataTable(templist);
                if (SrlNo == "0" && templist.Count > 0)
                {
                    Session["PIssue_WarehouseData"] = dt;
                }
                SrlNo = "1";
                templist = items.Where(x => x.Product_SrlNo == SrlNo).ToList();
                dt = ToDataTable(templist);
                if (SrlNo == "1" && templist.Count > 0)
                {
                    Session["PIssue_WarehouseDataFresh"] = dt;
                }

                DataTable dtWarehouse = ToDataTable(templist.Where(x => x.Product_SrlNo == SrlNo).ToList());
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
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public JsonResult WastageInsertUpdate(WastageViewModel obj)
        {
            ReturnData objRD = new ReturnData();
            String NumberScheme = "";
            DataTable dtWarehouse = new DataTable();
            DataTable dtWarehouseFresh = new DataTable();
            try
            {
                obj.UserID = Convert.ToInt64(Session["userid"]);
                //if (!String.IsNullOrEmpty(obj.Wastage_No) && obj.Wastage_No.ToLower() != "auto")
                if (!String.IsNullOrEmpty(obj.Wastage_No) )
                {
                    JVNumStr = obj.Wastage_No;
                    NumberScheme = "ok";
                }

                if (Session["PIssue_WarehouseData"] != null)
                {
                    dtWarehouse = (DataTable)Session["PIssue_WarehouseData"];
                }

                if (Session["PIssue_WarehouseDataFresh"] != null)
                {
                    dtWarehouseFresh = (DataTable)Session["PIssue_WarehouseDataFresh"];
                }
                else
                {
                    dtWarehouseFresh = dtWarehouse;
                    if (dtWarehouseFresh != null)
                    {
                        if (dtWarehouseFresh.Rows.Count > 0)
                        {
                            Int64 rowcount = 0;
                            foreach (DataRow row in dtWarehouseFresh.Rows)
                            {
                                if (rowcount > 0)
                                {
                                    row.Delete();
                                }
                                else
                                {
                                    row["Quantity"] = obj.WarehouseQty;
                                    row["WarehouseID"] = obj.WastageWarehouseID;
                                    rowcount++;
                                }
                            }
                        }

                    }
                }

                //if (obj.WastageID == 0)
                //{
                //    NumberScheme = checkNMakePOCode(obj.Wastage_No, Convert.ToInt32(obj.WastageSchemaID), Convert.ToDateTime(obj.WastageDate));
                //}
                if (NumberScheme == "ok")
                {
                    obj.Wastage_No = JVNumStr;
                    var datasetobj = WM.WastageInsertUpdate("InsertUpdate", obj, Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), dtWarehouse, dtWarehouseFresh);

                    if (datasetobj.Rows.Count > 0)
                    {

                        foreach (DataRow item in datasetobj.Rows)
                        {
                            objRD.Success = Convert.ToBoolean(item["Success"]);
                            //objRD.Message = JVNumStr;
                            objRD.Message = Convert.ToString(item["WastageNo"]);   
                        }
                    }
                }
                else
                {
                    objRD.Message = NumberScheme;
                }


                Session["PIssue_WarehouseData"] = null;
                Session["PIssue_WarehouseDataFresh"] = null;
            }
            catch { }
            return Json(objRD);
        }

        public string checkNMakePOCode(string manual_str, int sel_schema_Id, DateTime RevisionDate)
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
                        sqlQuery = "SELECT max(tjv.Wastage_No) FROM Wastage tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Wastage_No))) = 1 and Wastage_No like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.Wastage_No) FROM Wastage tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Wastage_No))) = 1 and Wastage_No like '%" + sufxCompCode + "'";
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
                            sqlQuery = "SELECT max(tjv.Wastage_No) FROM Wastage tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + paddCounter + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Wastage_No))) = 1 and Wastage_No like '" + prefCompCode + "%'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }
                        else
                        {
                            int i = startNo.Length;
                            while (i < paddCounter)
                            {


                                sqlQuery = "SELECT max(tjv.Wastage_No) FROM Wastage tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + i + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.Wastage_No))) = 1 and Wastage_No like '" + prefCompCode + "%'";

                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.Wastage_No)=" + i;
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
                                sqlQuery = "SELECT max(tjv.Wastage_No) FROM Wastage tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + (i - 1) + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.Wastage_No))) = 1 and Wastage_No like '" + prefCompCode + "%'";
                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.Wastage_No)=" + (i - 1);
                                }
                                dtC = oDBEngine.GetDataTable(sqlQuery);
                            }

                        }

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.Wastage_No) FROM Wastage tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Wastage_No))) = 1 and Wastage_No like '" + prefCompCode + "%'";
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
                    sqlQuery = "SELECT Wastage_No FROM Wastage WHERE Wastage_No LIKE '" + manual_str.Trim() + "'";
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

        public ActionResult WastageList()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/WastageList", "Wastage");
            TempData.Clear();
            ViewBag.CanAdd = rights.CanAdd;
            return View(WVM);
        }

        public ActionResult GetWastageList()
        {
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            List<WastageViewModel> list = new List<WastageViewModel>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/WastageList", "Wastage");
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
                        dt = oDBEngine.GetDataTable("select * from V_WastageList where BRANCH_ID =" + BranchID + " AND (Wastage_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') ORDER BY WastageID DESC");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("select * from V_WastageList where Wastage_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' ORDER BY WastageID DESC");
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
                    foreach (DataRow item in dt.Rows)
                    {
                        WVM = new WastageViewModel();

                        WVM.WastageID = Convert.ToInt64(item["WastageID"]);
                        WVM.WastageSchemaID = Convert.ToInt64(item["WastageSchemaID"]);
                        WVM.Wastage_No = Convert.ToString(item["Wastage_No"]);
                        WVM.dtWastageDate = Convert.ToDateTime(item["Wastage_Date"]);
                        WVM.WastageWarehouseID = Convert.ToInt64(item["WastageWarehouseID"]);
                        WVM.WarehouseQty = Convert.ToDecimal(item["WastageQty"]);

                        WVM.StockReceiptID = Convert.ToInt64(item["StockReceiptID"]);

                        WVM.QualityControlID = Convert.ToInt64(item["QualityControlID"]);


                        WVM.Fresh_ReceiptQty = Convert.ToDecimal(item["Fresh_ReceiptQty"]);
                        WVM.Rejected_ReceiptQty = Convert.ToDecimal(item["Rejected_ReceiptQty"]);

                        WVM.FGQty = Convert.ToDecimal(item["FG_Qty"]);
                        WVM.FreshQuantity = Convert.ToDecimal(item["Fresh_Qty"]);
                        WVM.RejectedQuantity = Convert.ToDecimal(item["Rejected_Qty"]);

                        WVM.QC_No = Convert.ToString(item["QC_No"]);
                        WVM.dtOrderDate = Convert.ToDateTime(item["QC_Date"]);

                        WVM.ProductionReceiptID = Convert.ToInt64(item["ProductionReceiptID"]);
                        WVM.WorkOrderID = Convert.ToInt64(item["WorkOrderID"]);
                        WVM.ProductionOrderID = Convert.ToInt64(item["ProductionOrderID"]);
                        //objSR.WorkCenterID = Convert.ToString(item["WorkCenterID"]);
                        WVM.Receipt_No = Convert.ToString(item["Receipt_No"]);
                        WVM.Receipt_Date = Convert.ToDateTime(item["Receipt_Date"]);

                        WVM.ProductionReceiptNo = Convert.ToString(item["ProductionReceiptNo"]);
                        WVM.ProductionReceiptDate = Convert.ToDateTime(item["ProductionReceiptDate"]);

                        WVM.ProductionIssueID = Convert.ToInt64(item["ProductionIssueID"]);
                        WVM.ProductionIssueNo = Convert.ToString(item["ProductionIssueNo"]);
                        WVM.ProductionIssueDate = Convert.ToDateTime(item["ProductionIssueDate"]);
                        WVM.WorkOrderNo = Convert.ToString(item["WorkOrderNo"]);
                        //objQVC.Order_Qty = Convert.ToDecimal(item["Receipt_Qty"]);
                        WVM.WorkCenterCode = Convert.ToString(item["WorkCenterCode"]);
                        WVM.WorkCenterDescription = Convert.ToString(item["WorkCenterDescription"]);
                        WVM.BRANCH_ID = Convert.ToInt64(item["BRANCH_ID"]);
                        WVM.BOMNo = Convert.ToString(item["BOM_No"]);
                        WVM.RevNo = Convert.ToString(item["REV_No"]);
                        WVM.BOM_Date = Convert.ToDateTime(item["BOM_Date"]);
                        WVM.ProductionOrderNo = Convert.ToString(item["ProductionOrderNo"]);
                        WVM.ProductionOrderDate = Convert.ToDateTime(item["ProductionOrderDate"]);
                        WVM.WorkOrderDate = Convert.ToDateTime(item["WorkOrderDate"]);

                        if (Convert.ToString(item["REV_Date"]) != "")
                        {
                            WVM.REV_Date = Convert.ToDateTime(item["REV_Date"]);
                        }
                        else
                        {
                            WVM.REV_Date = null;
                        }



                        WVM.CreatedBy = Convert.ToString(item["CreatedBy"]);
                        WVM.ModifyBy = Convert.ToString(item["ModifyBy"]);
                        WVM.CreateDate = Convert.ToDateTime(item["CreateDate"]);

                        if (Convert.ToString(item["ModifyDate"]) != "")
                        {
                            WVM.ModifyDate = Convert.ToDateTime(item["ModifyDate"]);
                        }
                        else
                        {
                            WVM.ModifyDate = null;
                        }
                        WVM.PartNoName = Convert.ToString(item["PartNoName"]);
                        WVM.DesignNo = Convert.ToString(item["DesignNo"]);
                        WVM.ItemRevNo = Convert.ToString(item["ItemRevisionNo"]);
                        WVM.FinishedItemDescription = Convert.ToString(item["sProducts_Name"]);
                        WVM.Proj_Code = Convert.ToString(item["Proj_Code"]);
                        WVM.Proj_Name = Convert.ToString(item["Proj_Name"]);
                        list.Add(WVM);
                    }
                }
            }
            catch { }
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            return PartialView("_WastageDataList", list);
        }

        public JsonResult RemoveDataByID(Int32 WastageID)
        {
            Boolean Success = false;
            try
            {
                var datasetobj = WM.GetWastageData("RemoveData", WastageID, 0); 
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

        public JsonResult SetDataByID(Int64 WastageID = 0, Int16 IsView = 0)
        {
            Boolean Success = false;
            try
            {
                TempData.Clear();

                TempData["WastageID"] = WastageID;
                TempData["IsView"] = IsView;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }


        public JsonResult SetDateFilter(Int64 unitid, string FromDate, string ToDate)
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


        [HttpPost]
        public JsonResult GetWarehouseList(Int64 WastageID = 0)
        {
            List<WastageStkWarehouse> list = new List<WastageStkWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                if (WastageID > 0)
                {
                    dt = WM.GetWastageData("GetWarehouseData", WastageID, 0);
                    if (dt.Rows.Count > 0)
                    {
                        WastageStkWarehouse obj = null;
                        foreach (DataRow item in dt.Rows)
                        {
                            obj = new WastageStkWarehouse();
                            obj.StkWarehouseID = Convert.ToInt64(item["StkWarehouseID"]);
                            obj.WastageID = Convert.ToInt64(item["WastageID"]);
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

                            list.Add(obj);
                        }
                    }
                }
            }
            catch { }
            return Json(list);
        }

        [HttpPost]
        public JsonResult setStockWarehouseTempDataList(List<udtStockProduct> items)
        {
            Boolean Success = false;
            String Message = "";
            Boolean IsProcess = false;
            String SrlNo = "0";
            try
            {
                List<udtStockProduct> templist = items.Where(x => x.Product_SrlNo == SrlNo).ToList();
                DataTable dt = ToDataTable(templist);
                if (SrlNo == "0" && templist.Count > 0)
                {
                    Session["PIssue_WarehouseData"] = dt;
                }
                SrlNo = "1";
                templist = items.Where(x => x.Product_SrlNo == SrlNo).ToList();
                dt = ToDataTable(templist);
                if (SrlNo == "1" && templist.Count > 0)
                {
                    Session["PIssue_WarehouseDataFresh"] = dt;
                }

                Success = true;
            }
            catch { }
            Message = Success.ToString();
            return Json(Message);
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
            var settings = new GridViewSettings();
            settings.Name = "Wastage";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Wastage";
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "Wastage_No" || datacolumn.ColumnName == "Wastage_Date" || datacolumn.ColumnName == "WastageQty" ||
                    datacolumn.ColumnName == "Fresh_ReceiptQty" || datacolumn.ColumnName == "Rejected_ReceiptQty" || datacolumn.ColumnName == "QC_No" || datacolumn.ColumnName == "FG_Qty" || datacolumn.ColumnName == "Fresh_Qty" || datacolumn.ColumnName == "Rejected_Qty" ||
                    datacolumn.ColumnName == "Receipt_No" || datacolumn.ColumnName == "QC_Date" || datacolumn.ColumnName == "Receipt_Date" || datacolumn.ColumnName == "ProductionIssueNo" || datacolumn.ColumnName == "ProductionIssueDate" ||
                    datacolumn.ColumnName == "WorkOrderNo" || datacolumn.ColumnName == "WorkOrderDate" || datacolumn.ColumnName == "ProductionOrderNo" || datacolumn.ColumnName == "ProductionOrderDate"
                    || datacolumn.ColumnName == "ProductionReceiptNo" || datacolumn.ColumnName == "ProductionReceiptDate"
                    || datacolumn.ColumnName == "BOM_No" || datacolumn.ColumnName == "BOM_Date" || datacolumn.ColumnName == "REV_No" || datacolumn.ColumnName == "REV_Date" ||
                    datacolumn.ColumnName == "CreatedBy" || datacolumn.ColumnName == "ModifyBy" || datacolumn.ColumnName == "CreateDate" || datacolumn.ColumnName == "ModifyDate")
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "Wastage_No")
                        {
                            column.Caption = "Wastage No";
                            column.VisibleIndex = 0;
                        }

                        if (datacolumn.ColumnName == "Wastage_Date")
                        {
                            column.Caption = "Wastage Date";
                            column.VisibleIndex = 1;
                        }

                        if (datacolumn.ColumnName == "WastageQty")
                        {
                            column.Caption = "Wastage Qty.";
                            column.VisibleIndex = 2;
                        }

                        if (datacolumn.ColumnName == "Receipt_No")
                        {
                            column.Caption = "Production Receipt No";
                            column.VisibleIndex = 3;
                        }

                        if (datacolumn.ColumnName == "Receipt_Date")
                        {
                            column.Caption = "Production Receipt Date";
                            column.VisibleIndex = 4;
                        }

                        if (datacolumn.ColumnName == "Fresh_ReceiptQty")
                        {
                            column.Caption = "Stock Fresh Receipt Qty";
                            column.VisibleIndex = 5;
                        }

                        if (datacolumn.ColumnName == "Rejected_ReceiptQty")
                        {
                            column.Caption = "Stock Rejected Receipt Qty";
                            column.VisibleIndex = 6;
                        }

                        if (datacolumn.ColumnName == "QC_No")
                        {
                            column.Caption = "Quality Control No";
                            column.VisibleIndex = 7;
                        }

                        if (datacolumn.ColumnName == "FG_Qty")
                        {
                            column.Caption = "QC FG Quantity";
                            column.VisibleIndex = 8;
                        }

                        if (datacolumn.ColumnName == "Fresh_Qty")
                        {
                            column.Caption = "QC Fresh Quantity";
                            column.VisibleIndex = 9;
                        }

                        if (datacolumn.ColumnName == "Rejected_Qty")
                        {
                            column.Caption = "QC Rejected Quantity";
                            column.VisibleIndex = 10;
                        }

                        if (datacolumn.ColumnName == "QC_Date")
                        {
                            column.Caption = "Quality Control Date";
                            column.VisibleIndex = 11;
                        }

                        if (datacolumn.ColumnName == "ProductionReceiptNo")
                        {
                            column.Caption = "Production Receipt No";
                            column.VisibleIndex = 12;
                        }

                        if (datacolumn.ColumnName == "ProductionReceiptDate")
                        {
                            column.Caption = "Production Receipt Date";
                            column.VisibleIndex = 13;
                        }


                        if (datacolumn.ColumnName == "ProductionIssueNo")
                        {
                            column.Caption = "Production Issue No";
                            column.VisibleIndex = 14;
                        }

                        if (datacolumn.ColumnName == "ProductionIssueDate")
                        {
                            column.Caption = "Production Issue Date";
                            column.VisibleIndex = 15;
                        }

                        if (datacolumn.ColumnName == "WorkOrderNo")
                        {
                            column.Caption = "Work Order No";
                            column.VisibleIndex = 16;
                        }
                        else if (datacolumn.ColumnName == "WorkOrderDate")
                        {
                            column.Caption = "Work Order Date";
                            column.VisibleIndex = 17;
                        }
                        else if (datacolumn.ColumnName == "ProductionOrderNo")
                        {
                            column.Caption = "Production Order No";
                            column.VisibleIndex = 18;
                        }
                        else if (datacolumn.ColumnName == "ProductionOrderDate")
                        {
                            column.Caption = "Production Order Date";
                            column.VisibleIndex = 19;
                        }
                        else if (datacolumn.ColumnName == "BOM_No")
                        {
                            column.Caption = "BOM No";
                            column.VisibleIndex = 20;

                        }
                        else if (datacolumn.ColumnName == "BOM_Date")
                        {
                            column.Caption = "BOM Date";
                            column.VisibleIndex = 21;
                        }
                        else if (datacolumn.ColumnName == "REV_No")
                        {
                            column.Caption = "Rev No.";
                            column.VisibleIndex = 22;
                        }
                        else if (datacolumn.ColumnName == "REV_Date")
                        {
                            column.Caption = "Rev Date";
                            column.VisibleIndex = 23;
                        }
                        else if (datacolumn.ColumnName == "CreatedBy")
                        {
                            column.Caption = "Entered By";
                            column.VisibleIndex = 24;
                        }
                        else if (datacolumn.ColumnName == "CreateDate")
                        {
                            column.Caption = "Entered On";
                            column.VisibleIndex = 25;
                        }
                        else if (datacolumn.ColumnName == "ModifyBy")
                        {
                            column.Caption = "Modified By";
                            column.VisibleIndex = 26;
                        }
                        else if (datacolumn.ColumnName == "ModifyDate")
                        {
                            column.Caption = "Modified On";
                            column.VisibleIndex = 27;
                        }

                        column.FieldName = datacolumn.ColumnName;
                        if (datacolumn.DataType.FullName == "System.Decimal")
                        {
                            column.PropertiesEdit.DisplayFormatString = "0.0000";
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
    }
}