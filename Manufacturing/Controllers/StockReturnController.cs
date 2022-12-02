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
    public class StockReturnController : Controller
    {
        QualityControlModel objQC = null;
        BOMEntryModel objdata = null;
        QualityControlViewModel objQVC = null;
        WorkOrderModel objWO = null;
        String JVNumStr = String.Empty;
        DBEngine oDBEngine = new DBEngine();
        UserRightsForPage rights = new UserRightsForPage();
        StockReceiptViewModel objSR = null;
        StockReceiptModel objSRM = null;
        ProductionIssueModel objPI = null;
        IssueReturnModel objIR = null;
        StockReturnViewModel objSRN = null;
        StockReturnModel objSRNM = null;
        CommonBL cSOrder = new CommonBL();
        public StockReturnController()
        {
            objQC = new QualityControlModel();
            objQVC = new QualityControlViewModel();
            objdata = new BOMEntryModel();
            objWO = new WorkOrderModel();
            objSR = new StockReceiptViewModel();
            objSRM = new StockReceiptModel();
            objPI = new ProductionIssueModel();
            objIR = new IssueReturnModel();
            objSRN = new StockReturnViewModel();
            objSRNM = new StockReturnModel();
        }

        //
        // GET: /StockReturn/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StockReturnEntry()
        {
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule"); 
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
            objSRN.UnitList = list;

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
                if (TempData["StockReturnID"] != null)
                {
                    objSRN.StockReturnID = Convert.ToInt64(TempData["StockReturnID"]);
                    TempData.Keep();

                    if (objSRN.StockReturnID > 0)
                    {
                        DataTable objData = objSRNM.GetStockReturnData("GetStockReturnData", objSRN.StockReturnID, 0);
                        if (objData != null && objData.Rows.Count > 0)
                        {
                            DataTable dt = objData;
                            foreach (DataRow row in dt.Rows)
                            {
                                objSRN.StockReturnID = Convert.ToInt64(row["StockReturnID"]);
                                objSRN.ReturnSchemaID = Convert.ToInt64(row["SchemaID"]);

                                objSRN.StockReceiptID = Convert.ToInt64(row["StockReceiptID"]);

                                objSRN.StockReturn_No = Convert.ToString(row["Return_No"]);
                                objSRN.dtStockReturnDate = Convert.ToDateTime(row["Return_Date"]);

                                objSRN.QualityControlID = Convert.ToInt64(row["QualityControlID"]);
                                objSRN.ProductID = Convert.ToInt64(row["sProducts_ID"]);
                                objSRN.InventoryType = Convert.ToString(row["InventoryType"]);
                                objSRN.FGReceiptQty = Convert.ToDecimal(row["FG_Qty"]);
                                objSRN.FreshQuantity = Convert.ToDecimal(row["Fresh_ReceiptQty"]);
                                objSRN.RejectedQuantity = Convert.ToDecimal(row["Rejected_ReceiptQty"]);

                                objSRN.BalFreshQuantity = Convert.ToDecimal(row["Fresh_Qty"]);
                                objSRN.BalRejectedQuantity = Convert.ToDecimal(row["Rejected_Qty"]);

                                objSRN.Product_NegativeStock = Convert.ToString(row["sProduct_NegativeStock"]);

                                objSRN.AvlStk = Convert.ToDecimal(row["AvlStk"]);

                                objSRN.WarehouseID = Convert.ToInt64(row["WarehouseID"]);

                                
                                objSRN.QC_No = Convert.ToString(row["QC_No"]);
                                //objSRN.dtOrderDate = Convert.ToDateTime(row["QC_Date"]);

                                objSRN.ProductionReceiptID = Convert.ToInt64(row["ProductionReceiptID"]);
                                objSRN.WorkOrderID = Convert.ToInt64(row["WorkOrderID"]);
                                objSRN.ProductionOrderID = Convert.ToInt64(row["ProductionOrderID"]);
                                objSRN.WorkCenterID = Convert.ToString(row["WorkCenterID"]);

                                objSRN.StockReceipt_No = Convert.ToString(row["Receipt_No"]);
                                objSRN.StockReceiptDate = Convert.ToDateTime(row["Receipt_Date"]).ToString("dd-MM-yyyy");

                                objSRN.Receipt_No = Convert.ToString(row["ProductionReceiptNo"]);
                                objSRN.Receipt_Date = Convert.ToDateTime(row["ProductionReceiptDate"]);

                                objSRN.ProductionIssueID = Convert.ToInt64(row["ProductionIssueID"]);
                                objSRN.ProductionIssueNo = Convert.ToString(row["ProductionIssueNo"]);
                                objSRN.ProductionIssueDate = Convert.ToDateTime(row["ProductionIssueDate"]);
                                objSRN.WorkOrderNo = Convert.ToString(row["WorkOrderNo"]);
                                //objSR.Order_Qty = Convert.ToDecimal(row["Receipt_Qty"]);
                                objSRN.WorkCenterCode = Convert.ToString(row["WorkCenterCode"]);
                                objSRN.WorkCenterDescription = Convert.ToString(row["WorkCenterDescription"]);
                                objSRN.BRANCH_ID = Convert.ToInt64(row["BRANCH_ID"]);
                                objSRN.BOMNo = Convert.ToString(row["BOM_No"]);
                                objSRN.RevNo = Convert.ToString(row["REV_No"]);
                                objSRN.BOM_Date = Convert.ToDateTime(row["BOM_Date"]);
                                objSRN.ProductionOrderNo = Convert.ToString(row["ProductionOrderNo"]);
                                objSRN.ProductionOrderDate = Convert.ToDateTime(row["ProductionOrderDate"]);
                                objSRN.WorkOrderDate = Convert.ToDateTime(row["WorkOrderDate"]);

                                objSRN.FinishedItem = Convert.ToString(row["ProductName"]);
                                objSRN.FinishedUom = Convert.ToString(row["FinishedUom"]);
                                objSRN.Warehouse = Convert.ToString(row["Warehouse"]);
                                objSRN.TotalAmount = Convert.ToDecimal(row["TotalAmount"]);
                                objSRN.FGPrice = Convert.ToDecimal(row["FGPrice"]);
                                objSRN.strRemarks = Convert.ToString(row["Remarks"]);
                                objSRN.ProductDescription = Convert.ToString(row["ProductDescription"]);
                                objSRN.Details_ID = Convert.ToInt64(row["Details_ID"]);
                                objSRN.ReceiptSchemaID = Convert.ToInt64(row["SchemaID"]);

                                if (Convert.ToString(row["REV_Date"]) != "")
                                {
                                    objSRN.REV_Date = Convert.ToDateTime(row["REV_Date"]);
                                }
                                else
                                {
                                    objSRN.REV_Date = null;
                                }



                                objSRN.CreatedBy = Convert.ToString(row["CreatedBy"]);
                                objSRN.ModifyBy = Convert.ToString(row["ModifyBy"]);
                                objSRN.CreateDate = Convert.ToDateTime(row["CreateDate"]);

                                if (Convert.ToString(row["ModifyDate"]) != "")
                                {
                                    objSRN.ModifyDate = Convert.ToDateTime(row["ModifyDate"]);
                                }
                                else
                                {
                                    objSRN.ModifyDate = null;
                                }
                                objSRN.PartNoName = Convert.ToString(row["PartNoName"]);
                                objSRN.DesignNo = Convert.ToString(row["DesignNo"]);
                                objSRN.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                                objSRN.Proj_Code = Convert.ToString(row["Proj_Code"]);
                                objSRN.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);
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

                if (objSRN.StockReturnID < 1)
                {
                    objSRN.dtStockReturnDate = DateTime.Now;
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

            }
            else
            {
                ViewBag.hdfIsBarcodeActive = "N";
                ViewBag.hdfIsBarcodeGenerator = "N";
            }

            #endregion

            return View(objSRN);
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

        public JsonResult getNumberingSchemeRecord()
        {
            List<SchemaNumber> list = new List<SchemaNumber>();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            DataTable Schemadt = objdata.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "107", "Y");
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
                        objSR.Details_ID = Convert.ToInt64(row["Details_ID"]);
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

                        objSR.ProductionReceiptDate = Convert.ToDateTime(row["ProductionReceiptDate"]);
                        //objQVC.Order_Qty = Convert.ToDecimal(row["Receipt_Qty"]);
                        objSR.Receipt_Date = Convert.ToDateTime(row["Receipt_Date"]);
                        objSR.WorkOrderNo = Convert.ToString(row["WorkOrderNo"]);

                        objSR.ProductID = Convert.ToInt64(row["sProducts_ID"]);

                        objSR.InventoryType = Convert.ToString(row["InventoryType"]);

                        //objSR.FreshQuantity = Convert.ToDecimal(row["Fresh_Qty"]);
                        //objSR.RejectedQuantity = Convert.ToDecimal(row["Rejected_Qty"]);

                        //objSR.Bal_Fresh_Qty = Convert.ToDecimal(row["Bal_Fresh_Qty"]);
                        //objSR.Bal_Rejected_Qty = Convert.ToDecimal(row["Bal_Rejected_Qty"]);

                        objSR.Product_NegativeStock = Convert.ToString(row["sProduct_NegativeStock"]);

                        objSR.AvlStk = Convert.ToDecimal(row["AvlStk"]);

                        objSR.WarehouseID = Convert.ToInt64(row["WarehouseID"]);
                        objSR.PartNoName = Convert.ToString(row["PartNoName"]);
                        objSR.DesignNo = Convert.ToString(row["DesignNo"]);
                        objSR.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);

                        objSR.Proj_Code = Convert.ToString(row["Proj_Code"]);
                        objSR.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);
                        list.Add(objSR);
                    }
                }

            }
            catch { }
            return PartialView("_StockReceiptList", list);
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

        public JsonResult StockReturnInsertUpdate(StockReturnViewModel obj)
        {
            ReturnData objRD = new ReturnData();
            String NumberScheme = "";
            DataTable dtWarehouse = new DataTable();
            DataTable dtWarehouseFresh = new DataTable();
            DataTable dtWarehouseWC = new DataTable();
            try
            {
                obj.UserID = Convert.ToInt64(Session["userid"]);
                //if (!String.IsNullOrEmpty(obj.StockReturn_No) && obj.StockReturn_No.ToLower() != "auto")
                if (!String.IsNullOrEmpty(obj.StockReturn_No))
                {
                    JVNumStr = obj.StockReturn_No;
                    NumberScheme = "ok";
                }

                if (Session["PIssue_WarehouseData"] != null)
                {
                    dtWarehouse = (DataTable)Session["PIssue_WarehouseData"];
                    dtWarehouseWC = dtWarehouse;
                }

                if (Session["PIssue_WarehouseDataFresh"] != null)
                {
                    dtWarehouseFresh = (DataTable)Session["PIssue_WarehouseDataFresh"];
                    dtWarehouseWC = dtWarehouseFresh;
                }


                if (dtWarehouseWC != null)
                {
                    if (dtWarehouseWC.Rows.Count > 0)
                    {
                        int LoopCount = 0;
                        foreach (DataRow row in dtWarehouseWC.Rows)
                        {
                            if (LoopCount == 0)
                            {
                                row["Quantity"] = Convert.ToDecimal(obj.FreshQuantity + obj.RejectedQuantity);
                                row["WarehouseID"] = obj.WorkCenterID;
                            }
                            else
                            {
                                row.Delete();
                            }
                            LoopCount++;
                        }
                    }

                }

                //if (obj.StockReturnID == 0)
                //{
                //    NumberScheme = checkNMakePOCode(obj.StockReturn_No, Convert.ToInt32(obj.ReturnSchemaID), Convert.ToDateTime(obj.StockReturnDate));
                //}
                if (NumberScheme == "ok")
                {
                    obj.StockReturn_No = JVNumStr;
                    var datasetobj = objSRNM.StockReturnInsertUpdate("InsertUpdate", obj, Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), dtWarehouse, dtWarehouseFresh, dtWarehouseWC);

                    if (datasetobj.Rows.Count > 0)
                    {

                        foreach (DataRow item in datasetobj.Rows)
                        {
                            objRD.Success = Convert.ToBoolean(item["Success"]);
                            objRD.Message = Convert.ToString(item["ReturnNo"]);                            
                            //objRD.Message = JVNumStr;
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
                        sqlQuery = "SELECT max(tjv.Return_No) FROM StockReturn tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_No))) = 1 and Return_No like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.Return_No) FROM StockReturn tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_No))) = 1 and Return_No like '%" + sufxCompCode + "'";
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
                            sqlQuery = "SELECT max(tjv.Return_No) FROM StockReturn tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + paddCounter + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_No))) = 1 and Return_No like '" + prefCompCode + "%'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }
                        else
                        {
                            int i = startNo.Length;
                            while (i < paddCounter)
                            {


                                sqlQuery = "SELECT max(tjv.Return_No) FROM StockReturn tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + i + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_No))) = 1 and Return_No like '" + prefCompCode + "%'";

                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.Return_No)=" + i;
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
                                sqlQuery = "SELECT max(tjv.Return_No) FROM StockReturn tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + (i - 1) + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_No))) = 1 and Return_No like '" + prefCompCode + "%'";
                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.Return_No)=" + (i - 1);
                                }
                                dtC = oDBEngine.GetDataTable(sqlQuery);
                            }

                        }

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.Return_No) FROM StockReturn tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_No))) = 1 and Return_No like '" + prefCompCode + "%'";
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
                    sqlQuery = "SELECT Return_No FROM StockReturn WHERE Return_No LIKE '" + manual_str.Trim() + "'";
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

        public ActionResult StockReturnList()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/StockReturnList", "StockReturn");
            TempData.Clear();
            ViewBag.CanAdd = rights.CanAdd;
            return View(objSRN);
        }

        public ActionResult GetStockReturnList()
        {
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            List<StockReturnViewModel> list = new List<StockReturnViewModel>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/StockReturnList", "StockReturn");
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
                        dt = oDBEngine.GetDataTable("select * from V_StockReturnList where BRANCH_ID =" + BranchID + " AND (Return_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') ORDER BY StockReturnID DESC");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("select * from V_StockReturnList where Return_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' ORDER BY StockReturnID DESC");
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
                        objSRN = new StockReturnViewModel();

                        objSRN.StockReturnID = Convert.ToInt64(item["StockReturnID"]);
                        objSRN.StockReturn_No = Convert.ToString(item["Return_No"]);
                        objSRN.dtStockReturnDate = Convert.ToDateTime(item["Return_Date"]);

                        objSRN.StockReceiptID = Convert.ToInt64(item["StockReceiptID"]);

                        objSRN.QualityControlID = Convert.ToInt64(item["QualityControlID"]);


                        objSRN.Fresh_ReceiptQty = Convert.ToDecimal(item["Fresh_ReceiptQty"]);
                        objSRN.Rejected_ReceiptQty = Convert.ToDecimal(item["Rejected_ReceiptQty"]);

                        objSRN.FGQty = Convert.ToDecimal(item["FG_Qty"]);
                        objSRN.FreshQuantity = Convert.ToDecimal(item["Fresh_Qty"]);
                        objSRN.RejectedQuantity = Convert.ToDecimal(item["Rejected_Qty"]);

                        objSRN.QC_No = Convert.ToString(item["QC_No"]);
                        objSRN.dtOrderDate = Convert.ToDateTime(item["QC_Date"]);

                        objSRN.ProductionReceiptID = Convert.ToInt64(item["ProductionReceiptID"]);
                        objSRN.WorkOrderID = Convert.ToInt64(item["WorkOrderID"]);
                        objSRN.ProductionOrderID = Convert.ToInt64(item["ProductionOrderID"]);
                        //objSR.WorkCenterID = Convert.ToString(item["WorkCenterID"]);
                        objSRN.Receipt_No = Convert.ToString(item["Receipt_No"]);
                        objSRN.Receipt_Date = Convert.ToDateTime(item["Receipt_Date"]);

                        objSRN.ProductionReceiptNo = Convert.ToString(item["ProductionReceiptNo"]);
                        objSRN.ProductionReceiptDate = Convert.ToDateTime(item["ProductionReceiptDate"]);

                        objSRN.ProductionIssueID = Convert.ToInt64(item["ProductionIssueID"]);
                        objSRN.ProductionIssueNo = Convert.ToString(item["ProductionIssueNo"]);
                        objSRN.ProductionIssueDate = Convert.ToDateTime(item["ProductionIssueDate"]);
                        objSRN.WorkOrderNo = Convert.ToString(item["WorkOrderNo"]);
                        //objQVC.Order_Qty = Convert.ToDecimal(item["Receipt_Qty"]);
                        objSRN.WorkCenterCode = Convert.ToString(item["WorkCenterCode"]);
                        objSRN.WorkCenterDescription = Convert.ToString(item["WorkCenterDescription"]);
                        objSRN.BRANCH_ID = Convert.ToInt64(item["BRANCH_ID"]);
                        objSRN.BOMNo = Convert.ToString(item["BOM_No"]);
                        objSRN.RevNo = Convert.ToString(item["REV_No"]);
                        objSRN.BOM_Date = Convert.ToDateTime(item["BOM_Date"]);
                        objSRN.ProductionOrderNo = Convert.ToString(item["ProductionOrderNo"]);
                        objSRN.ProductionOrderDate = Convert.ToDateTime(item["ProductionOrderDate"]);
                        objSRN.WorkOrderDate = Convert.ToDateTime(item["WorkOrderDate"]);

                        if (Convert.ToString(item["REV_Date"]) != "")
                        {
                            objSRN.REV_Date = Convert.ToDateTime(item["REV_Date"]);
                        }
                        else
                        {
                            objSRN.REV_Date = null;
                        }



                        objSRN.CreatedBy = Convert.ToString(item["CreatedBy"]);
                        objSRN.ModifyBy = Convert.ToString(item["ModifyBy"]);
                        objSRN.CreateDate = Convert.ToDateTime(item["CreateDate"]);

                        if (Convert.ToString(item["ModifyDate"]) != "")
                        {
                            objSRN.ModifyDate = Convert.ToDateTime(item["ModifyDate"]);
                        }
                        else
                        {
                            objSRN.ModifyDate = null;
                        }
                        objSRN.PartNoName = Convert.ToString(item["PartNoName"]);
                        objSRN.DesignNo = Convert.ToString(item["DesignNo"]);
                        objSRN.ItemRevNo = Convert.ToString(item["ItemRevisionNo"]);
                        objSRN.FinishedItemDescription = Convert.ToString(item["sProducts_Name"]);
                        objSRN.Proj_Code = Convert.ToString(item["Proj_Code"]);
                        objSRN.Proj_Name = Convert.ToString(item["Proj_Name"]);
                        list.Add(objSRN);
                    }
                }
            }
            catch { }
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            return PartialView("_StockReturnDataList", list);
        }

        public JsonResult SetSRDateFilter(Int64 unitid, string FromDate, string ToDate)
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
            var settings = new GridViewSettings();
            settings.Name = "Stock Return";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Stock Return";
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "Return_No" || datacolumn.ColumnName == "Return_Date" || datacolumn.ColumnName == "Fresh_ReceiptQty" || datacolumn.ColumnName == "Rejected_ReceiptQty" || datacolumn.ColumnName == "QC_No" || datacolumn.ColumnName == "FG_Qty" || datacolumn.ColumnName == "Fresh_Qty" || datacolumn.ColumnName == "Rejected_Qty" ||
                    datacolumn.ColumnName == "Receipt_No" || datacolumn.ColumnName == "QC_Date" || datacolumn.ColumnName == "Receipt_Date" || datacolumn.ColumnName == "ProductionIssueNo" || datacolumn.ColumnName == "ProductionIssueDate" ||
                    datacolumn.ColumnName == "WorkOrderNo" || datacolumn.ColumnName == "WorkOrderDate" || datacolumn.ColumnName == "ProductionOrderNo" || datacolumn.ColumnName == "ProductionOrderDate"
                    || datacolumn.ColumnName == "ProductionReceiptNo" || datacolumn.ColumnName == "ProductionReceiptDate"
                    || datacolumn.ColumnName == "BOM_No" || datacolumn.ColumnName == "BOM_Date" || datacolumn.ColumnName == "REV_No" || datacolumn.ColumnName == "REV_Date" ||
                    datacolumn.ColumnName == "CreatedBy" || datacolumn.ColumnName == "ModifyBy" || datacolumn.ColumnName == "CreateDate" || datacolumn.ColumnName == "ModifyDate")
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "Return_No")
                        {
                            column.Caption = "Stock Return No";
                            column.VisibleIndex = 0;
                        }

                        if (datacolumn.ColumnName == "Return_Date")
                        {
                            column.Caption = "Stock Return Date";
                            column.VisibleIndex = 1;
                        }

                        if (datacolumn.ColumnName == "Receipt_No")
                        {
                            column.Caption = "Production Receipt No";
                            column.VisibleIndex = 2;
                        }

                        if (datacolumn.ColumnName == "Receipt_Date")
                        {
                            column.Caption = "Production Receipt Date";
                            column.VisibleIndex = 3;
                        }

                        if (datacolumn.ColumnName == "Fresh_ReceiptQty")
                        {
                            column.Caption = "Stock Fresh Receipt Qty";
                            column.VisibleIndex = 4;
                        }

                        if (datacolumn.ColumnName == "Rejected_ReceiptQty")
                        {
                            column.Caption = "Stock Rejected Receipt Qty";
                            column.VisibleIndex = 5;
                        }

                        if (datacolumn.ColumnName == "QC_No")
                        {
                            column.Caption = "Quality Control No";
                            column.VisibleIndex = 6;
                        }

                        if (datacolumn.ColumnName == "FG_Qty")
                        {
                            column.Caption = "QC FG Quantity";
                            column.VisibleIndex = 7;
                        }

                        if (datacolumn.ColumnName == "Fresh_Qty")
                        {
                            column.Caption = "QC Fresh Quantity";
                            column.VisibleIndex = 8;
                        }

                        if (datacolumn.ColumnName == "Rejected_Qty")
                        {
                            column.Caption = "QC Rejected Quantity";
                            column.VisibleIndex = 9;
                        }

                        if (datacolumn.ColumnName == "QC_Date")
                        {
                            column.Caption = "Quality Control Date";
                            column.VisibleIndex = 10;
                        }

                        if (datacolumn.ColumnName == "ProductionReceiptNo")
                        {
                            column.Caption = "Production Receipt No";
                            column.VisibleIndex = 11;
                        }

                        if (datacolumn.ColumnName == "ProductionReceiptDate")
                        {
                            column.Caption = "Production Receipt Date";
                            column.VisibleIndex = 12;
                        }


                        if (datacolumn.ColumnName == "ProductionIssueNo")
                        {
                            column.Caption = "Production Issue No";
                            column.VisibleIndex = 13;
                        }

                        if (datacolumn.ColumnName == "ProductionIssueDate")
                        {
                            column.Caption = "Production Issue Date";
                            column.VisibleIndex = 14;
                        }

                        if (datacolumn.ColumnName == "WorkOrderNo")
                        {
                            column.Caption = "Work Order No";
                            column.VisibleIndex = 15;
                        }
                        else if (datacolumn.ColumnName == "WorkOrderDate")
                        {
                            column.Caption = "Work Order Date";
                            column.VisibleIndex = 16;
                        }
                        else if (datacolumn.ColumnName == "ProductionOrderNo")
                        {
                            column.Caption = "Production Order No";
                            column.VisibleIndex = 17;
                        }
                        else if (datacolumn.ColumnName == "ProductionOrderDate")
                        {
                            column.Caption = "Production Order Date";
                            column.VisibleIndex = 18;
                        }
                        else if (datacolumn.ColumnName == "BOM_No")
                        {
                            column.Caption = "BOM No";
                            column.VisibleIndex = 19;

                        }
                        else if (datacolumn.ColumnName == "BOM_Date")
                        {
                            column.Caption = "BOM Date";
                            column.VisibleIndex = 20;
                        }
                        else if (datacolumn.ColumnName == "REV_No")
                        {
                            column.Caption = "Rev No.";
                            column.VisibleIndex = 21;
                        }
                        else if (datacolumn.ColumnName == "REV_Date")
                        {
                            column.Caption = "Rev Date";
                            column.VisibleIndex = 22;
                        }
                        else if (datacolumn.ColumnName == "CreatedBy")
                        {
                            column.Caption = "Entered By";
                            column.VisibleIndex = 23;
                        }
                        else if (datacolumn.ColumnName == "CreateDate")
                        {
                            column.Caption = "Entered On";
                            column.VisibleIndex = 24;
                        }
                        else if (datacolumn.ColumnName == "ModifyBy")
                        {
                            column.Caption = "Modified By";
                            column.VisibleIndex = 25;
                        }
                        else if (datacolumn.ColumnName == "ModifyDate")
                        {
                            column.Caption = "Modified On";
                            column.VisibleIndex = 26;
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

        public JsonResult RemoveSRDataByID(Int32 StockReturnID)
        {
            Boolean Success = false;
            try
            {
                var datasetobj = objSRNM.GetStockReturnData("RemoveData", StockReturnID, 0);
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

        public JsonResult SetSRDataByID(Int64 StockReturnID = 0, Int16 IsView = 0)
        {
            Boolean Success = false;
            try
            {
                TempData.Clear();

                TempData["StockReturnID"] = StockReturnID;
                TempData["IsView"] = IsView;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }

        [HttpPost]
        public JsonResult GetWarehouseList(Int64 StockReturnID = 0)
        {
            List<StockReturnStkWarehouse> list = new List<StockReturnStkWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                if (StockReturnID > 0)
                {
                    dt = objSRNM.GetStockReturnData("GetWarehouseData", StockReturnID, 0);
                    if (dt.Rows.Count > 0)
                    {
                        StockReturnStkWarehouse obj = new StockReturnStkWarehouse();
                        foreach (DataRow item in dt.Rows)
                        {
                            obj = new StockReturnStkWarehouse();
                            obj.StkWarehouseID = Convert.ToInt64(item["StkWarehouseID"]);
                            obj.StockReturnID = Convert.ToInt64(item["StockReturnID"]);
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
    }
}