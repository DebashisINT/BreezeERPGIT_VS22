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
    public class StockReceiptController : Controller
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
        CommonBL cSOrder = new CommonBL();
        public StockReceiptController()
        {
            objQC = new QualityControlModel();
            objQVC = new QualityControlViewModel();
            objdata = new BOMEntryModel();
            objWO = new WorkOrderModel();
            objSR = new StockReceiptViewModel();
            objSRM = new StockReceiptModel();
            objPI = new ProductionIssueModel();
            objIR = new IssueReturnModel();
        }

        //
        // GET: /StockReceipt/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getNumberingSchemeRecord()
        {
            List<SchemaNumber> list = new List<SchemaNumber>();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            DataTable Schemadt = objdata.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "106", "Y");
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

        public ActionResult StockReceiptEntry()
        {
            string WorkOrderModuleSkipped = cSOrder.GetSystemSettingsResult("WorkOrderModuleSkipped");
            string QualityControlModuleSkipped = cSOrder.GetSystemSettingsResult("QualityControlModuleSkipped");
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
            objSR.UnitList = list;

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
                if (TempData["StockReceiptID"] != null)
                {
                    objSR.StockReceiptID = Convert.ToInt64(TempData["StockReceiptID"]);
                    objSR.Doctype = Convert.ToString(TempData["Doctype"]);
                    TempData.Keep();
                    DataTable objData;
                    if (objSR.StockReceiptID > 0)
                    {
                        if (objSR.Doctype == "BOM")
                        {
                            objData = objSRM.GetStockReceiptData("GetStockReceiptDataFromInvoice", objSR.StockReceiptID, 0);
                        }
                        else
                        {
                            if (QualityControlModuleSkipped == "No")
                            {
                                objData = objSRM.GetStockReceiptData("GetStockReceiptData", objSR.StockReceiptID, 0);
                            }
                            else
                            {
                                objData = objSRM.GetStockReceiptData("GetStockReceiptDataSettingsWise", objSR.StockReceiptID, 0);
                            }
                        }
                        
                         
                        if (objData != null && objData.Rows.Count > 0)
                        {
                            DataTable dt = objData;
                            foreach (DataRow row in dt.Rows)
                            {
                                objSR.QualityControlID = Convert.ToInt64(row["QualityControlID"]);
                                objSR.ProductID = Convert.ToInt64(row["sProducts_ID"]);
                                objSR.InventoryType = Convert.ToString(row["InventoryType"]);
                                objSR.FGReceiptQty = Convert.ToDecimal(row["FG_Qty"]);
                                objSR.FreshQuantity = Convert.ToDecimal(row["Fresh_ReceiptQty"]);
                                objSR.RejectedQuantity = Convert.ToDecimal(row["Rejected_ReceiptQty"]);
                                objSR.BalFreshQuantity = Convert.ToDecimal(row["Fresh_Qty"]);
                                objSR.BalRejectedQuantity = Convert.ToDecimal(row["Rejected_Qty"]);
                                objSR.Product_NegativeStock = Convert.ToString(row["sProduct_NegativeStock"]);
                                objSR.AvlStk = Convert.ToDecimal(row["AvlStk"]);
                                objSR.WarehouseID = Convert.ToInt64(row["WarehouseID"]);
                                objSR.QC_SchemaID = Convert.ToInt64(row["QC_SchemaID"]);
                                objSR.QC_No = Convert.ToString(row["QC_No"]);
                                if (Convert.ToString(row["QC_Date"]) != "")
                                {
                                    objSR.dtOrderDate = Convert.ToDateTime(row["QC_Date"]);
                                }
                                else
                                {
                                    objSR.dtOrderDate = null;
                                }
                               // objSR.dtOrderDate = Convert.ToDateTime(row["QC_Date"]);
                                objSR.ProductionReceiptID = Convert.ToInt64(row["ProductionReceiptID"]);
                                objSR.WorkOrderID = Convert.ToInt64(row["WorkOrderID"]);
                                objSR.ProductionOrderID = Convert.ToInt64(row["ProductionOrderID"]);
                                objSR.WorkCenterID = Convert.ToString(row["WorkCenterID"]);
                                objSR.StockReceipt_No = Convert.ToString(row["Receipt_No"]);
                                if (Convert.ToString(row["Receipt_Date"]) != "")
                                {
                                    objSR.dtStockReceiptDate = Convert.ToDateTime(row["Receipt_Date"]);
                                }
                                else
                                {
                                    objSR.dtStockReceiptDate = null;
                                }                                
                                objSR.Receipt_No = Convert.ToString(row["ProductionReceiptNo"]);
                                if (Convert.ToString(row["ProductionReceiptDate"]) != "")
                                {
                                    objSR.Receipt_Date = Convert.ToDateTime(row["ProductionReceiptDate"]);
                                }
                                else
                                {
                                    objSR.Receipt_Date = null;
                                }  
                              
                                objSR.ProductionIssueID = Convert.ToInt64(row["ProductionIssueID"]);
                                objSR.ProductionIssueNo = Convert.ToString(row["ProductionIssueNo"]);
                                if (Convert.ToString(row["ProductionIssueDate"]) != "")
                                {
                                    objSR.ProductionIssueDate = Convert.ToDateTime(row["ProductionIssueDate"]);
                                }
                                else
                                {
                                    objSR.ProductionIssueDate = null;
                                }  
                                
                                objSR.WorkOrderNo = Convert.ToString(row["WorkOrderNo"]);
                                //objSR.Order_Qty = Convert.ToDecimal(row["Receipt_Qty"]);
                                objSR.WorkCenterCode = Convert.ToString(row["WorkCenterCode"]);
                                objSR.WorkCenterDescription = Convert.ToString(row["WorkCenterDescription"]);
                                objSR.BRANCH_ID = Convert.ToInt64(row["BRANCH_ID"]);
                                objSR.BOMNo = Convert.ToString(row["BOM_No"]);
                                objSR.RevNo = Convert.ToString(row["REV_No"]);
                                objSR.BOM_Date = Convert.ToDateTime(row["BOM_Date"]);
                                objSR.ProductionOrderNo = Convert.ToString(row["ProductionOrderNo"]);
                                if (Convert.ToString(row["ProductionOrderDate"]) != "")
                                {
                                    objSR.ProductionOrderDate = Convert.ToDateTime(row["ProductionOrderDate"]);
                                }
                                else
                                {
                                    objSR.ProductionOrderDate = null;
                                }  
                               
                               
                                objSR.FinishedItem = Convert.ToString(row["ProductName"]);
                                objSR.FinishedUom = Convert.ToString(row["FinishedUom"]);
                                objSR.Warehouse = Convert.ToString(row["Warehouse"]);
                                objSR.TotalAmount = Convert.ToDecimal(row["TotalAmount"]);
                                objSR.FGPrice = Convert.ToDecimal(row["FGPrice"]);
                                objSR.strRemarks = Convert.ToString(row["Remarks"]);
                                objSR.ProductDescription = Convert.ToString(row["ProductDescription"]);
                                objSR.Details_ID = Convert.ToInt64(row["Details_ID"]);
                                objSR.ReceiptSchemaID = Convert.ToInt64(row["SchemaID"]);


                                if (Convert.ToString(row["WorkOrderDate"]) != "")
                                {
                                    objSR.WorkOrderDate = Convert.ToDateTime(row["WorkOrderDate"]);
                                }
                                else
                                {
                                    objSR.WorkOrderDate = null;
                                }


                                if (Convert.ToString(row["REV_Date"]) != "")
                                {
                                    objSR.REV_Date = Convert.ToDateTime(row["REV_Date"]);
                                }
                                else
                                {
                                    objSR.REV_Date = null;
                                }
                                objSR.CreatedBy = Convert.ToString(row["CreatedBy"]);
                                objSR.ModifyBy = Convert.ToString(row["ModifyBy"]);
                                objSR.CreateDate = Convert.ToDateTime(row["CreateDate"]);

                                if (Convert.ToString(row["ModifyDate"]) != "")
                                {
                                    objSR.ModifyDate = Convert.ToDateTime(row["ModifyDate"]);
                                }
                                else
                                {
                                    objSR.ModifyDate = null;
                                }

                                TempData["DetailsID"] = Convert.ToString(row["Details_ID"]);
                                objSR.PartNoName = Convert.ToString(row["PartNoName"]);
                                objSR.DesignNo = Convert.ToString(row["DesignNo"]);
                                objSR.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                                objSR.Proj_Code = Convert.ToString(row["Proj_Code"]);
                                objSR.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);
                            }
                        }
                    }
                }
                else
                {
                    TempData["DetailsID"] = null;
                    TempData.Clear();
                }

                if (objSR.ProductionReceiptID < 1)
                {
                    objSR.OrderDate = DateTime.Now.ToString();
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

            ViewBag.WorkOrderModuleSkipped = WorkOrderModuleSkipped;
            ViewBag.LastCompany = Convert.ToString(Session["LastCompany"]);
            ViewBag.LastFinancialYear = Convert.ToString(Session["LastFinYear"]);
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            ViewBag.QualityControlModuleSkipped = QualityControlModuleSkipped;
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

            return View(objSR);
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

        public ActionResult GetQCList()
        {
            string QualityControlModuleSkipped = cSOrder.GetSystemSettingsResult("QualityControlModuleSkipped");
            List<QualityControlViewModel> list = new List<QualityControlViewModel>();
            try
            {
                DataTable objData;
                if (QualityControlModuleSkipped == "No")
                {
                    objData = objQC.GetQCData("GetAllQualityControlData", 0, 0);
                }
                else
                {
                    objData = objQC.GetQCData("GetAllQualityControlDataSettingsWise", 0, 0);
                }
               

                if (objData != null && objData.Rows.Count > 0)
                {
                    DataTable dt = objData;
                    foreach (DataRow row in dt.Rows)
                    {
                        objQVC = new QualityControlViewModel();
                        objQVC.SerialNO = Convert.ToInt64(row["serial"]);
                        objQVC.QualityControlID = Convert.ToInt64(row["QualityControlID"]);
                        objQVC.FGQty = Convert.ToDecimal(row["FG_Qty"]);
                        objQVC.ProductionReceiptID = Convert.ToInt64(row["ProductionReceiptID"]);
                        objQVC.ProductionIssueID = Convert.ToInt64(row["ProductionIssueID"]);
                        objQVC.ProductionIssueNo = Convert.ToString(row["ProductionIssueNo"]);
                        objQVC.WorkOrderID = Convert.ToInt64(row["WorkOrderID"]);
                        //objWOL.OrderNo = Convert.ToString(row["OrderNo"]);
                        objQVC.ProductionOrderNo = Convert.ToString(row["ProductionOrderNo"]);
                        objQVC.ProductionOrderID = Convert.ToInt64(row["ProductionOrderID"]);
                        objQVC.Details_ID = Convert.ToInt64(row["Details_ID"]);
                        objQVC.BOMNo = Convert.ToString(row["BOM_No"]);
                        objQVC.QC_No = Convert.ToString(row["QC_No"]);
                        objQVC.dtOrderDate = Convert.ToDateTime(row["QC_Date"]);
                        objQVC.ProductionIssueDate = Convert.ToDateTime(row["ProductionIssueDate"]);
                        objQVC.RevNo = Convert.ToString(row["REV_No"]);
                        objQVC.FinishedItem = Convert.ToString(row["ProductName"]);
                        objQVC.FinishedUom = Convert.ToString(row["FinishedUom"]);
                        objQVC.ProductionOrderDate = Convert.ToDateTime(row["ProductionOrderDate"]);
                        objQVC.Warehouse = Convert.ToString(row["Warehouse"]);
                        //objQVC.ProductionIssueQty = Convert.ToDecimal(row["ProductionIssueQty"]);
                        objQVC.WorkCenterID = Convert.ToString(row["WorkCenterID"]);
                        objQVC.WorkCenterCode = Convert.ToString(row["WorkCenterCode"]);
                        objQVC.WorkCenterDescription = Convert.ToString(row["WorkCenterDescription"]);
                        objQVC.TotalAmount = Convert.ToDecimal(row["TotalAmount"]);
                        objQVC.FGPrice = Convert.ToDecimal(row["FGPrice"]);
                        objQVC.ProductDescription = Convert.ToString(row["ProductDescription"]);
                        objQVC.strRemarks = Convert.ToString(row["Remarks"]);
                        objQVC.BRANCH_ID = Convert.ToInt64(row["BRANCH_ID"]);
                        objQVC.Receipt_No = Convert.ToString(row["Receipt_No"]);
                        //objQVC.Order_Qty = Convert.ToDecimal(row["Receipt_Qty"]);
                        if (Convert.ToString(row["Receipt_Date"]) != "")
                        {
                            objQVC.Receipt_Date = Convert.ToDateTime(row["Receipt_Date"]);
                        }
                        else
                        {
                            objQVC.Receipt_Date = null;
                        }
                    
                        objQVC.WorkOrderNo = Convert.ToString(row["WorkOrderNo"]);

                        objQVC.ProductID = Convert.ToInt64(row["sProducts_ID"]);

                        objQVC.InventoryType = Convert.ToString(row["InventoryType"]);

                        objQVC.FreshQuantity = Convert.ToDecimal(row["Fresh_Qty"]);
                        objQVC.RejectedQuantity = Convert.ToDecimal(row["Rejected_Qty"]);

                        objQVC.Bal_Fresh_Qty = Convert.ToDecimal(row["Bal_Fresh_Qty"]);
                        objQVC.Bal_Rejected_Qty = Convert.ToDecimal(row["Bal_Rejected_Qty"]);

                        objQVC.Product_NegativeStock = Convert.ToString(row["sProduct_NegativeStock"]);

                        objQVC.AvlStk = Convert.ToDecimal(row["AvlStk"]);

                        objQVC.WarehouseID = Convert.ToInt64(row["WarehouseID"]);
                        objQVC.PartNoName = Convert.ToString(row["PartNoName"]);
                        objQVC.DesignNo = Convert.ToString(row["DesignNo"]);
                        objQVC.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                        objQVC.Proj_Code = Convert.ToString(row["Proj_Code"]);
                        objQVC.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);
                        
                        list.Add(objQVC);
                    }
                }

            }
            catch { }
            ViewBag.QualityControlModuleSkipped = QualityControlModuleSkipped;
            return PartialView("_QualityControlList", list);

        }

        public ActionResult GetWCList()
        {
            List<WorkCenterViewModel> list = new List<WorkCenterViewModel>();
            try
            {
                WorkCenterViewModel obj = new WorkCenterViewModel();
                // DataTable dt = lstuser.Getdesiglist();
                DataTable objData = objWO.GetWorkOrderData("GetWCList");

                // modeldesig = APIHelperMethods.ToModelList<BOMEntryViewModel>(objData);

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
        public JsonResult GetWarehouseList(Int64 StockReceiptID = 0)
        {
            List<StockReceiptStkWarehouse> list = new List<StockReceiptStkWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                if (StockReceiptID > 0)
                {
                    dt = objSRM.GetStockReceiptData("GetWarehouseData", StockReceiptID, 0);
                    if (dt.Rows.Count > 0)
                    {
                        StockReceiptStkWarehouse obj = new StockReceiptStkWarehouse();
                        foreach (DataRow item in dt.Rows)
                        {
                            obj = new StockReceiptStkWarehouse();
                            obj.StkWarehouseID = Convert.ToInt64(item["StkWarehouseID"]);
                            obj.StockReceiptID = Convert.ToInt64(item["StockReceiptID"]);
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

                    //if (list.Count > 0)
                    //{
                    //    List<udtStockProduct> templist = list.Where(x => x.Product_SrlNo == "0").ToList();
                    //    DataTable dt = new DataTable();
                    //    if (templist.Count > 0)
                    //    {
                    //        dt = ToDataTable(templist);
                    //        Session["PIssue_WarehouseData"] = dt;
                    //    }

                    //    templist = list.Where(x => x.Product_SrlNo == "1").ToList();
                    //    if (templist.Count > 0)
                    //    {
                    //        dt = ToDataTable(templist);
                    //        Session["PIssue_WarehouseDataFresh"] = dt;
                    //    }
                    //}

                }



            }
            catch { }
            return Json(list);
        }

        public JsonResult StockReceiptInsertUpdate(StockReceiptViewModel obj)
        {
            ReturnData objRD = new ReturnData();
            String NumberScheme = "";
            DataTable dtWarehouse = new DataTable();
            DataTable dtWarehouseFresh = new DataTable();
            DataTable dtWarehouseWC = new DataTable();
            try
            {
                obj.UserID = Convert.ToInt64(Session["userid"]);
                ////if (!String.IsNullOrEmpty(obj.StockReceipt_No) && obj.StockReceipt_No.ToLower() != "auto")
                if (!String.IsNullOrEmpty(obj.StockReceipt_No))
                {
                    JVNumStr = obj.StockReceipt_No;
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



                //if (dtWarehouseWC != null)
                //{
                //    if (dtWarehouseWC.Rows.Count > 0)
                //    {
                //        int LoopCount = 0;
                //        foreach (DataRow row in dtWarehouseWC.Rows)
                //        {
                //            if (LoopCount == 0)
                //            {
                //                row["Quantity"] = Convert.ToDecimal(obj.FreshQuantity + obj.RejectedQuantity);
                //                row["WarehouseID"] = obj.WorkCenterID;
                //            }
                //            else
                //            {
                //                row.Delete();
                //            }
                //            LoopCount++;
                //        }
                //    }

                //}


                //if (obj.StockReceiptID == 0)
                //{
                //    NumberScheme = checkNMakePOCode(obj.StockReceipt_No, Convert.ToInt32(obj.ReceiptSchemaID), Convert.ToDateTime(obj.StockReceiptDate));
                //}
                if (NumberScheme == "ok")
                {
                    obj.StockReceipt_No = JVNumStr;
                    var datasetobj = objSRM.StockReceiptInsertUpdate("InsertUpdate", obj, Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), dtWarehouse, dtWarehouseFresh, dtWarehouseWC);

                    if (datasetobj.Rows.Count > 0)
                    {

                        foreach (DataRow item in datasetobj.Rows)
                        {
                            objRD.Success = Convert.ToBoolean(item["Success"]);
                            objRD.Message = Convert.ToString(item["ReceiptNo"]);
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
                        sqlQuery = "SELECT max(tjv.Receipt_No) FROM StockReceipt tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Receipt_No))) = 1 and Receipt_No like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.Receipt_No) FROM StockReceipt tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Receipt_No))) = 1 and Receipt_No like '%" + sufxCompCode + "'";
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
                            sqlQuery = "SELECT max(tjv.Receipt_No) FROM StockReceipt tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + paddCounter + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Receipt_No))) = 1 and Receipt_No like '" + prefCompCode + "%'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }
                        else
                        {
                            int i = startNo.Length;
                            while (i < paddCounter)
                            {


                                sqlQuery = "SELECT max(tjv.Receipt_No) FROM StockReceipt tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + i + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.Receipt_No))) = 1 and Receipt_No like '" + prefCompCode + "%'";

                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.Receipt_No)=" + i;
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
                                sqlQuery = "SELECT max(tjv.Receipt_No) FROM StockReceipt tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + (i - 1) + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.Receipt_No))) = 1 and Receipt_No like '" + prefCompCode + "%'";
                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.Receipt_No)=" + (i - 1);
                                }
                                dtC = oDBEngine.GetDataTable(sqlQuery);
                            }

                        }

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.Receipt_No) FROM StockReceipt tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Receipt_No))) = 1 and Receipt_No like '" + prefCompCode + "%'";
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
                    sqlQuery = "SELECT Receipt_No FROM StockReceipt WHERE Receipt_No LIKE '" + manual_str.Trim() + "'";
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
            Message = Success.ToString() ;
            return Json(Message);
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

        public ActionResult StockReceiptList()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/StockReceiptList", "StockReceipt");
            TempData.Clear();
            ViewBag.CanAdd = rights.CanAdd;
            return View(objSR);
        }


        public ActionResult GetStockReceiptList()
        {
            string QualityControlModuleSkipped = cSOrder.GetSystemSettingsResult("QualityControlModuleSkipped");
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            List<StockReceiptViewModel> list = new List<StockReceiptViewModel>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/StockReceiptList", "StockReceipt");
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
                    //if (QualityControlModuleSkipped=="No")
                    //{
                        if (BranchID > 0)
                        {
                            dt = oDBEngine.GetDataTable("select * from V_StockReceiptList where BRANCH_ID =" + BranchID + " AND (ReceiptDate BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') ORDER BY StockReceiptID DESC");
                        }
                        else
                        {
                            dt = oDBEngine.GetDataTable("select * from V_StockReceiptList where ReceiptDate BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' ORDER BY StockReceiptID DESC");
                        }
                    //}
                    //else
                    //{
                    //    if (BranchID > 0)
                    //    {
                    //        dt = oDBEngine.GetDataTable("select * from V_StockReceiptListSettingsWise where BRANCH_ID =" + BranchID + " AND (Receipt_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') ORDER BY StockReceiptID DESC");
                    //    }
                    //    else
                    //    {
                    //        dt = oDBEngine.GetDataTable("select * from V_StockReceiptListSettingsWise where Receipt_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' ORDER BY StockReceiptID DESC");
                    //    }
                    //}
                    

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
                        objSR = new StockReceiptViewModel();

                        objSR.StockReceiptID = Convert.ToInt64(item["StockReceiptID"]);
                        objSR.QualityControlID = Convert.ToInt64(item["QualityControlID"]);

                        objSR.Fresh_ReceiptQty = Convert.ToDecimal(item["Fresh_ReceiptQty"]);
                        objSR.Rejected_ReceiptQty = Convert.ToDecimal(item["Rejected_ReceiptQty"]);
                        objSR.FGQty = Convert.ToDecimal(item["FG_Qty"]);
                        objSR.FreshQuantity = Convert.ToDecimal(item["Fresh_Qty"]);
                        objSR.RejectedQuantity = Convert.ToDecimal(item["Rejected_Qty"]);
                        objSR.QC_No = Convert.ToString(item["QC_No"]);
                        if (Convert.ToString(item["QC_Date"]) != "")
                        {
                            objSR.dtOrderDate = Convert.ToDateTime(item["QC_Date"]);
                        }
                        else
                        {
                            objSR.dtOrderDate = null;
                        }
                        objSR.ProductionReceiptID = Convert.ToInt64(item["ProductionReceiptID"]);
                        objSR.WorkOrderID = Convert.ToInt64(item["WorkOrderID"]);
                        objSR.ProductionOrderID = Convert.ToInt64(item["ProductionOrderID"]);
                        //objSR.WorkCenterID = Convert.ToString(item["WorkCenterID"]);
                        objSR.Receipt_No = Convert.ToString(item["Receipt_No"]);

                        if (Convert.ToString(item["Receipt_Date"]) != "")
                        {
                            objSR.Receipt_Date = Convert.ToDateTime(item["Receipt_Date"]);
                        }
                        else
                        {
                            objSR.Receipt_Date = null;
                        }

                        //objSR.Receipt_Date = Convert.ToDateTime(item["Receipt_Date"]);
                        objSR.ProductionReceiptNo = Convert.ToString(item["ProductionReceiptNo"]);

                        if (Convert.ToString(item["ProductionReceiptDate"]) != "")
                        {
                            objSR.ProductionReceiptDate = Convert.ToDateTime(item["ProductionReceiptDate"]);
                        }
                        else
                        {
                            objSR.ProductionReceiptDate = null;
                        }
                        //objSR.ProductionReceiptDate = Convert.ToDateTime(item["ProductionReceiptDate"]);

                        objSR.ProductionIssueID = Convert.ToInt64(item["ProductionIssueID"]);
                        objSR.ProductionIssueNo = Convert.ToString(item["ProductionIssueNo"]);
                        //objSR.ProductionIssueDate = Convert.ToDateTime(item["ProductionIssueDate"]);
                        if (Convert.ToString(item["ProductionIssueDate"]) != "")
                        {
                            objSR.ProductionIssueDate = Convert.ToDateTime(item["ProductionIssueDate"]);
                        }
                        else
                        {
                            objSR.ProductionIssueDate = null;
                        }

                        objSR.WorkOrderNo = Convert.ToString(item["WorkOrderNo"]);
                        //objQVC.Order_Qty = Convert.ToDecimal(item["Receipt_Qty"]);
                        objSR.WorkCenterCode = Convert.ToString(item["WorkCenterCode"]);
                        objSR.WorkCenterDescription = Convert.ToString(item["WorkCenterDescription"]);
                        objSR.BRANCH_ID = Convert.ToInt64(item["BRANCH_ID"]);
                        objSR.BOMNo = Convert.ToString(item["BOM_No"]);
                        objSR.RevNo = Convert.ToString(item["REV_No"]);
                        objSR.BOM_Date = Convert.ToDateTime(item["BOM_Date"]);
                        objSR.ProductionOrderNo = Convert.ToString(item["ProductionOrderNo"]);

                        if (Convert.ToString(item["ProductionOrderDate"]) != "")
                        {
                            objSR.ProductionOrderDate = Convert.ToDateTime(item["ProductionOrderDate"]);
                        }
                        else
                        {
                            objSR.ProductionOrderDate = null;
                        }

                        //objSR.ProductionOrderDate = Convert.ToDateTime(item["ProductionOrderDate"]);
                        //objSR.WorkOrderDate = Convert.ToDateTime(item["WorkOrderDate"]);

                        if (Convert.ToString(item["WorkOrderDate"]) != "")
                        {
                            objSR.WorkOrderDate = Convert.ToDateTime(item["WorkOrderDate"]);
                        }
                        else
                        {
                            objSR.WorkOrderDate = null;
                        }


                        if (Convert.ToString(item["REV_Date"]) != "")
                        {
                            objSR.REV_Date = Convert.ToDateTime(item["REV_Date"]);
                        }
                        else
                        {
                            objSR.REV_Date = null;
                        }

                        objSR.CreatedBy = Convert.ToString(item["CreatedBy"]);
                        objSR.ModifyBy = Convert.ToString(item["ModifyBy"]);
                        objSR.CreateDate = Convert.ToDateTime(item["CreateDate"]);

                        if (Convert.ToString(item["ModifyDate"]) != "")
                        {
                            objSR.ModifyDate = Convert.ToDateTime(item["ModifyDate"]);
                        }
                        else
                        {
                            objSR.ModifyDate = null;
                        }
                        objSR.PartNoName = Convert.ToString(item["PartNoName"]);
                        objSR.DesignNo = Convert.ToString(item["DesignNo"]);
                        objSR.ItemRevNo = Convert.ToString(item["ItemRevisionNo"]);
                        objSR.FinishedItemDescription = Convert.ToString(item["sProducts_Name"]);
                        objSR.Proj_Code = Convert.ToString(item["Proj_Code"]);
                        objSR.Proj_Name = Convert.ToString(item["Proj_Name"]);
                        objSR.Doctype = Convert.ToString(item["Doctype"]);
                        list.Add(objSR);
                    }
                }
            }
            catch { }
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            ViewBag.QualityControlModuleSkipped = QualityControlModuleSkipped;
            return PartialView("_StockReceiptDataList", list);
        }


        public JsonResult SetSRDataByID(Int64 StockReceiptID = 0, Int16 IsView = 0, string Doctype="")
        {
            Boolean Success = false;
            try
            {
                TempData.Clear();

                TempData["StockReceiptID"] = StockReceiptID;
                TempData["IsView"] = IsView;
                TempData["Doctype"] = Doctype;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
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
            settings.Name = "Stock Receipt";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Stock Receipt";
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "Fresh_ReceiptQty" || datacolumn.ColumnName == "Rejected_ReceiptQty" || datacolumn.ColumnName == "QC_No" || datacolumn.ColumnName == "FG_Qty" || datacolumn.ColumnName == "Fresh_Qty" || datacolumn.ColumnName == "Rejected_Qty" ||
                    datacolumn.ColumnName == "Receipt_No" || datacolumn.ColumnName == "QC_Date" || datacolumn.ColumnName == "Receipt_Date" || datacolumn.ColumnName == "ProductionIssueNo" || datacolumn.ColumnName == "ProductionIssueDate" ||
                    datacolumn.ColumnName == "WorkOrderNo" || datacolumn.ColumnName == "WorkOrderDate" || datacolumn.ColumnName == "ProductionOrderNo" || datacolumn.ColumnName == "ProductionOrderDate"
                    || datacolumn.ColumnName == "ProductionReceiptNo" || datacolumn.ColumnName == "ProductionReceiptDate"
                    || datacolumn.ColumnName == "BOM_No" || datacolumn.ColumnName == "BOM_Date" || datacolumn.ColumnName == "REV_No" || datacolumn.ColumnName == "REV_Date" ||
                    datacolumn.ColumnName == "CreatedBy" || datacolumn.ColumnName == "ModifyBy" || datacolumn.ColumnName == "CreateDate" || datacolumn.ColumnName == "ModifyDate")
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "Receipt_No")
                        {
                            column.Caption = "Production Receipt No";
                            column.VisibleIndex = 0;
                        }

                        if (datacolumn.ColumnName == "Receipt_Date")
                        {
                            column.Caption = "Production Receipt Date";
                            column.VisibleIndex = 1;
                        }

                        if (datacolumn.ColumnName == "Fresh_ReceiptQty")
                        {
                            column.Caption = "Stock Fresh Receipt Qty";
                            column.VisibleIndex = 2;
                        }

                        if (datacolumn.ColumnName == "Rejected_ReceiptQty")
                        {
                            column.Caption = "Stock Rejected Receipt Qty";
                            column.VisibleIndex = 3;
                        }

                        if (datacolumn.ColumnName == "QC_No")
                        {
                            column.Caption = "Quality Control No";
                            column.VisibleIndex = 4;
                        }

                        if (datacolumn.ColumnName == "FG_Qty")
                        {
                            column.Caption = "QC FG Quantity";
                            column.VisibleIndex = 5;
                        }

                        if (datacolumn.ColumnName == "Fresh_Qty")
                        {
                            column.Caption = "QC Fresh Quantity";
                            column.VisibleIndex = 6;
                        }

                        if (datacolumn.ColumnName == "Rejected_Qty")
                        {
                            column.Caption = "QC Rejected Quantity";
                            column.VisibleIndex = 7;
                        }

                        if (datacolumn.ColumnName == "QC_Date")
                        {
                            column.Caption = "Quality Control Date";
                            column.VisibleIndex = 8;
                        }

                        if (datacolumn.ColumnName == "ProductionReceiptNo")
                        {
                            column.Caption = "Production Receipt No";
                            column.VisibleIndex = 9;
                        }

                        if (datacolumn.ColumnName == "ProductionReceiptDate")
                        {
                            column.Caption = "Production Receipt Date";
                            column.VisibleIndex = 10;
                        }


                        if (datacolumn.ColumnName == "ProductionIssueNo")
                        {
                            column.Caption = "Production Issue No";
                            column.VisibleIndex = 11;
                        }

                        if (datacolumn.ColumnName == "ProductionIssueDate")
                        {
                            column.Caption = "Production Issue Date";
                            column.VisibleIndex = 12;
                        }

                        if (datacolumn.ColumnName == "WorkOrderNo")
                        {
                            column.Caption = "Work Order No";
                            column.VisibleIndex = 13;
                        }
                        else if (datacolumn.ColumnName == "WorkOrderDate")
                        {
                            column.Caption = "Work Order Date";
                            column.VisibleIndex = 14;
                        }
                        else if (datacolumn.ColumnName == "ProductionOrderNo")
                        {
                            column.Caption = "Production Order No";
                            column.VisibleIndex = 15;
                        }
                        else if (datacolumn.ColumnName == "ProductionOrderDate")
                        {
                            column.Caption = "Production Order Date";
                            column.VisibleIndex = 16;
                        }
                        else if (datacolumn.ColumnName == "BOM_No")
                        {
                            column.Caption = "BOM No";
                            column.VisibleIndex = 17;

                        }
                        else if (datacolumn.ColumnName == "BOM_Date")
                        {
                            column.Caption = "BOM Date";
                            column.VisibleIndex = 18;
                        }
                        else if (datacolumn.ColumnName == "REV_No")
                        {
                            column.Caption = "Rev No.";
                            column.VisibleIndex = 19;
                        }
                        else if (datacolumn.ColumnName == "REV_Date")
                        {
                            column.Caption = "Rev Date";
                            column.VisibleIndex = 20;
                        }
                        else if (datacolumn.ColumnName == "CreatedBy")
                        {
                            column.Caption = "Entered By";
                            column.VisibleIndex = 21;
                        }
                        else if (datacolumn.ColumnName == "CreateDate")
                        {
                            column.Caption = "Entered On";
                            column.VisibleIndex = 22;
                        }
                        else if (datacolumn.ColumnName == "ModifyBy")
                        {
                            column.Caption = "Modified By";
                            column.VisibleIndex = 23;
                        }
                        else if (datacolumn.ColumnName == "ModifyDate")
                        {
                            column.Caption = "Modified On";
                            column.VisibleIndex = 24;
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

        public JsonResult RemoveSRDataByID(Int32 StockReceiptID)
        {
            Boolean Success = false;
            try
            {
                var datasetobj = objSRM.GetStockReceiptData("RemoveSRData", StockReceiptID, 0);
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
    }
}