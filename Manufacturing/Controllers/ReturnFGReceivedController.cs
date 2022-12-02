using BusinessLogicLayer;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using Manufacturing.Models;
using Manufacturing.Models.ViewModel;
using Manufacturing.Models.ViewModel.BOMEntryModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace Manufacturing.Controllers
{
    public class ReturnFGReceivedController : Controller
    {
       
         BOMEntryModel objdata = null;
         ReturnFGReceivedViewModel objWC = null;
        DBEngine oDBEngine = new DBEngine();
        WorkOrderModel objWO = null;
        string JVNumStr = string.Empty;
        UserRightsForPage rights = new UserRightsForPage();
        ProductionOrderModel objPM = null;
        IssueReturnModel objIR = null;
        ProductionIssueModel objPI = null;
        ReturnFGReceivedModel objPR = null;
        CommonBL cSOrder = new CommonBL();
        public ReturnFGReceivedController()
        {
            objdata = new BOMEntryModel();
            objWC = new ReturnFGReceivedViewModel();
            objWO = new WorkOrderModel();
            objPM = new ProductionOrderModel();
            objIR = new IssueReturnModel();
            objPI = new ProductionIssueModel();
            objPR = new ReturnFGReceivedModel();
        }
        //
        // GET: /WorkOrder/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductionReceiptEntry()
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
            objWC.UnitList = list;

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
                if (TempData["ProductionReceiptID"] != null)
                {
                    objWC.ProductionReceiptID = Convert.ToInt64(TempData["ProductionReceiptID"]);
                    TempData.Keep();
                  
                    if (objWC.ProductionReceiptID > 0)
                    {
                        DataTable objData = objPR.GetProductionReceiptData("GetReturnFGEditData", objWC.ProductionReceiptID, 0);
                        if (objData != null && objData.Rows.Count > 0)
                        {
                            DataTable dt = objData;
                            foreach (DataRow row in dt.Rows)
                            {
                                //objWC.ProductionOrderID = Convert.ToInt64(row["ProductionOrderID"]);
                                objWC.ProductionIssueID = Convert.ToInt64(row["ProductionIssueID"]);
                                objWC.WorkOrderID = Convert.ToInt64(row["WorkOrderID"]);
                                //objWC.Production_ID = Convert.ToInt64(row["ProductionOrderID"]);
                                objWC.WorkCenterID = Convert.ToString(row["WorkCenterID"]);
                                objWC.OrderNo = Convert.ToString(row["Receipt_No"]);
                                objWC.Order_SchemaID = Convert.ToInt64(row["Receipt_SchemaID"]);
                                objWC.OrderDate = Convert.ToDateTime(row["Receipt_Date"]).ToString("dd-MM-yyyy");
                                objWC.dtOrderDate = Convert.ToDateTime(row["Receipt_Date"]);
                                objWC.BRANCH_ID = Convert.ToInt64(row["BRANCH_ID"]);
                                objWC.Order_Qty = Convert.ToDecimal(row["Receipt_Qty"]);
                                //objWC.BOMNo = Convert.ToString(row["BOM_No"]);
                                //objWC.RevNo = Convert.ToString(row["REV_No"]);
                                objWC.FinishedItem = Convert.ToString(row["ProductName"]);
                                objWC.FinishedUom = Convert.ToString(row["FinishedUom"]);
                              //  objWC.Finished_Qty = Convert.ToDecimal(row["Finished_Qty"]);
                                objWC.Warehouse = Convert.ToString(row["Warehouse"]);
                                //objWC.ProductionOrderNo = Convert.ToString(row["ProductionOrderNo"]);
                                objWC.ProductionIssueNo = Convert.ToString(row["ProductionIssueNo"]);
                                objWC.WorkCenterCode = Convert.ToString(row["WorkCenterCode"]);
                                objWC.WorkCenterDescription = Convert.ToString(row["WorkCenterDescription"]);
                                objWC.ProductionIssueQty = Convert.ToDecimal(row["ProductionIssueQty"]);
                                objWC.IssueDate = Convert.ToDateTime(row["ProductionIssueDate"]).ToString("dd-MM-yyyy");
                                objWC.WorkOrderNo = Convert.ToString(row["WorkOrderNo"]);
                                objWC.FGPrice = Convert.ToDecimal(row["FGPrice"]);
                                objWC.TotalAmount = Convert.ToDecimal(row["TotalAmount"]);                                
                                objWC.strRemarks = Convert.ToString(row["Remarks"]);

                                objWC.PartNoName = Convert.ToString(row["PartNoName"]);
                                objWC.DesignNo = Convert.ToString(row["DesignNo"]);
                                objWC.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                                objWC.Description = Convert.ToString(row["sProducts_Name"]);
                                objWC.Proj_Code = Convert.ToString(row["Proj_Code"]);
                                objWC.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);
                                objWC.WarehouseID = Convert.ToInt64(row["WarehouseID"]);
                                objWC.Warehouse = Convert.ToString(row["Warehouse"]);
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

                if (objWC.ProductionReceiptID < 1)
                {
                    objWC.OrderDate = DateTime.Now.ToString();
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

            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            TempData["Count"] = 1;
            TempData.Keep();
            return View(objWC);
            
        }

        public ActionResult GetWorkOrderFinishItemDetailsDetailsProductList()
        {
            ReturnFGReceivedFinishItemDetails FinishItemDetailsobj = new ReturnFGReceivedFinishItemDetails();
            List<ReturnFGReceivedFinishItemDetails> FinishItemDetailsproductdata = new List<ReturnFGReceivedFinishItemDetails>();
            Int64 DetailsID = 0;
            Int64 MaterialsId = 0;
            try
            {

                if (TempData["ProductionReceiptID"] != null)
                {
                    DetailsID = Convert.ToInt64(TempData["ProductionReceiptID"]);
                    TempData.Keep();
                }
                else if (TempData["ProductionIssueID"] != null)
                {
                    DetailsID = Convert.ToInt64(TempData["ProductionIssueID"]);
                    MaterialsId = DetailsID = Convert.ToInt64(TempData["ProductionIssueID"]);
                    TempData.Keep();
                }
                DataTable dt = new DataTable();
                if (DetailsID > 0 && TempData["ProductsDetails"] == null)
                {
                    DataTable objData = new DataTable();
                    if (TempData["ProductionReceiptID"] != null)
                    {
                        objData = objPR.GetProductionReceiptFinishData("ReturnFGReceivedEditFinishItemList", Convert.ToInt64(DetailsID));
                    }
                    else if (TempData["ProductionIssueID"] != null)
                    {
                        objData = objPR.GetProductionReceiptFinishData("ReturnFGReceivedAddFinishItemList", Convert.ToInt64(DetailsID));
                    }
                    if (objData != null && objData.Rows.Count > 0)
                    {
                        dt = objData;
                        //TempData["ProdAddlDesc"] = null;
                        //if (prodAddlDesc == null || prodAddlDesc.Rows.Count == 0)
                        //{
                        //    prodAddlDesc.Columns.Add("SrlNo", typeof(string));
                        //    prodAddlDesc.Columns.Add("AdditionRemarks", typeof(string));
                        //}
                        DataTable dtable = new DataTable();

                        dtable.Clear();
                        dtable.Columns.Add("HIddenID", typeof(System.Guid));
                        dtable.Columns.Add("SrlNO", typeof(System.String));
                        dtable.Columns.Add("FinishItemName", typeof(System.String));
                        dtable.Columns.Add("FinishItemDescription", typeof(System.String));
                        dtable.Columns.Add("FinishDrawingNo", typeof(System.String));
                        dtable.Columns.Add("FinishItemRevNo", typeof(System.String));
                        dtable.Columns.Add("Qty", typeof(System.String));
                        dtable.Columns.Add("FinishUOM", typeof(System.String));
                        dtable.Columns.Add("FinishPrice", typeof(System.String));
                        dtable.Columns.Add("FinishAmount", typeof(System.String));
                        dtable.Columns.Add("FinishUpdateEdit", typeof(System.String));
                        dtable.Columns.Add("FGReturn_Id", typeof(System.String));
                        dtable.Columns.Add("FGReceivedID", typeof(System.String));
                        dtable.Columns.Add("FinishUOMId", typeof(System.String));
                        dtable.Columns.Add("FinishProductsID", typeof(System.String));
                        dtable.Columns.Add("FinishWareHouseId", typeof(System.String));
                        dtable.Columns.Add("OldFGQuantity", typeof(System.String));
                        dtable.Columns.Add("MaxBalFGQuantity", typeof(System.String));
                        dtable.Columns.Add("FGWareHouseName", typeof(System.String));
                        dtable.Columns.Add("FinishInventoryType", typeof(System.String));

                        String Gid = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            Gid = Guid.NewGuid().ToString();
                            FinishItemDetailsobj = new ReturnFGReceivedFinishItemDetails();
                            FinishItemDetailsobj.SrlNO = Convert.ToString(row["SrlNO"]);
                            FinishItemDetailsobj.FinishItemName = Convert.ToString(row["FinishItemName"]);
                            FinishItemDetailsobj.FinishItemDescription = Convert.ToString(row["FinishItemDescription"]);
                            FinishItemDetailsobj.FinishDrawingNo = Convert.ToString(row["FinishDrawingNo"]);
                            FinishItemDetailsobj.FinishItemRevNo = Convert.ToString(row["FinishItemRevNo"]);
                            FinishItemDetailsobj.Qty = Convert.ToString(row["Qty"]);
                            FinishItemDetailsobj.FinishUOM = Convert.ToString(row["FinishUOM"]);
                            FinishItemDetailsobj.FinishPrice = Convert.ToString(row["FinishPrice"]);
                            FinishItemDetailsobj.FinishAmount = Convert.ToString(row["FinishAmount"]);

                            FinishItemDetailsobj.FGReturn_Id = Convert.ToString(row["FGReturn_Id"]);
                            FinishItemDetailsobj.FGReceivedID = Convert.ToString(row["FGReceivedID"]);
                            FinishItemDetailsobj.FinishUOMId = Convert.ToString(row["FinishUOMId"]);
                            FinishItemDetailsobj.FinishProductsID = Convert.ToString(row["FinishProductsID"]);
                            FinishItemDetailsobj.FinishWareHouseId = Convert.ToString(row["FinishWareHouseId"]);
                            FinishItemDetailsobj.OldFGQuantity = Convert.ToString(row["OldFGQuantity"]);
                            FinishItemDetailsobj.MaxBalFGQuantity = Convert.ToString(row["MaxBalFGQuantity"]);
                            FinishItemDetailsobj.FGWareHouseName = Convert.ToString(row["FGWareHouseName"]);
                            FinishItemDetailsobj.FinishInventoryType = Convert.ToString(row["FinishInventoryType"]);
                            FinishItemDetailsobj.Guids = Gid;

                            FinishItemDetailsproductdata.Add(FinishItemDetailsobj);


                            object[] trow = { Gid, row["SrlNO"],Convert.ToString(row["FinishItemName"]),Convert.ToString(row["FinishItemDescription"]),
                                                Convert.ToString(row["FinishDrawingNo"]),Convert.ToString(row["FinishItemRevNo"]),Convert.ToString(row["Qty"]),
                                   Convert.ToString(row["FinishUOM"]),Convert.ToString(row["FinishPrice"]),Convert.ToString(row["FinishAmount"]),"1",MaterialsId,DetailsID,
                                    Convert.ToString(row["FinishUOMId"]),Convert.ToString(row["FinishProductsID"]),Convert.ToString(row["FinishWareHouseId"]),Convert.ToString(row["OldFGQuantity"]),
                                    Convert.ToString(row["MaxBalFGQuantity"]),Convert.ToString(row["FGWareHouseName"]),Convert.ToString(row["FinishInventoryType"]) };
                            dtable.Rows.Add(trow);
                        }
                        //TempData["ProdAddlDesc"] = prodAddlDesc;
                        //TempData.Keep();

                        dt = dtable;


                    }
                }
                else
                {
                    dt = (DataTable)TempData["ProductsDetails"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {

                            FinishItemDetailsobj = new ReturnFGReceivedFinishItemDetails();
                            FinishItemDetailsobj.Guids = Convert.ToString(row["HIddenID"]);
                            FinishItemDetailsobj.SrlNO = Convert.ToString(row["SrlNO"]);
                            FinishItemDetailsobj.FinishItemName = Convert.ToString(row["FinishItemName"]);
                            FinishItemDetailsobj.FinishItemDescription = Convert.ToString(row["FinishItemDescription"]);
                            FinishItemDetailsobj.FinishDrawingNo = Convert.ToString(row["FinishDrawingNo"]);
                            FinishItemDetailsobj.FinishItemRevNo = Convert.ToString(row["FinishItemRevNo"]);
                            FinishItemDetailsobj.Qty = Convert.ToString(row["Qty"]);
                            FinishItemDetailsobj.FinishUOM = Convert.ToString(row["FinishUOM"]);
                            FinishItemDetailsobj.FinishPrice = Convert.ToString(row["FinishPrice"]);
                            FinishItemDetailsobj.FinishAmount = Convert.ToString(row["FinishAmount"]);

                            FinishItemDetailsobj.FinishUOMId = Convert.ToString(row["FinishUOMId"]);
                            FinishItemDetailsobj.FinishProductsID = Convert.ToString(row["FinishProductsID"]);
                            FinishItemDetailsobj.FinishWareHouseId = Convert.ToString(row["FinishWareHouseId"]);
                            FinishItemDetailsobj.OldFGQuantity = Convert.ToString(row["OldFGQuantity"]);
                            FinishItemDetailsobj.MaxBalFGQuantity = Convert.ToString(row["MaxBalFGQuantity"]);
                            FinishItemDetailsobj.FGWareHouseName = Convert.ToString(row["FGWareHouseName"]);
                            FinishItemDetailsobj.FinishInventoryType = Convert.ToString(row["FinishInventoryType"]);
                            FinishItemDetailsproductdata.Add(FinishItemDetailsobj);
                        }

                    }
                }
                TempData["ProductsDetails"] = dt;
                TempData.Keep();
                ViewBag.FinishTotalQty = FinishItemDetailsproductdata.Sum(x => Convert.ToDecimal(x.Qty)).ToString();

                string FinishToAmount = FinishItemDetailsproductdata.Sum(x => Convert.ToDecimal(x.FinishAmount)).ToString();
                if (FinishToAmount == "0" || FinishToAmount == "")
                {
                    ViewBag.FinishAddlPrice = "0.00";
                }
                else
                {
                    decimal FinishAddlPrice = Convert.ToDecimal(FinishToAmount) / Convert.ToDecimal(ViewBag.FinishTotalQty);
                    ViewBag.FinishAddlPrice = Convert.ToString(FinishAddlPrice);

                }


            }
            catch { }

            return PartialView("_FinishItemGrid", FinishItemDetailsproductdata);
            //return PartialView("~/Views/PMS/Estimate/EstimateProductList.cshtml", FinishItemDetailsproductdata);
        }
        public ActionResult GetPOList()
        {
            List<ProductionOrderViewModel> list = new List<ProductionOrderViewModel>();
            try
            {
                ProductionOrderViewModel objPO = new ProductionOrderViewModel();
                DataTable objData = objWO.GetWorkOrderData("GetPOList");
                if (objData != null && objData.Rows.Count > 0)
                {
                    DataTable dt = objData;
                    foreach (DataRow row in dt.Rows)
                    {
                        objPO = new ProductionOrderViewModel();
                        objPO.ProductionOrderID = Convert.ToInt64(row["ProductionOrderID"]);
                        objPO.Production_ID = Convert.ToInt64(row["Production_ID"]);
                        objPO.Details_ID = Convert.ToInt64(row["Details_ID"]);
                        objPO.BOMNo = Convert.ToString(row["BOM_No"]);
                        objPO.OrderNo = Convert.ToString(row["OrderNo"]);
                        objPO.RevNo = Convert.ToString(row["REV_No"]);
                        objPO.FinishedItem = Convert.ToString(row["ProductName"]);
                        objPO.FinishedUom = Convert.ToString(row["UOM_Name"]);
                        objPO.BalQty = Convert.ToDecimal(row["BalQty"]);
                        objPO.Unit = Convert.ToString(row["BranchName"]);
                        objPO.Warehouse = Convert.ToString(row["WarehouseName"]);
                        objPO.OrderDate = Convert.ToDateTime(row["OrderDate"]).ToString("dd-MM-yyyy");
                        if (Convert.ToString(row["REV_Date"]) == "")
                        {
                            objPO.REV_Date = null;
                        }
                        else
                        {
                            objPO.REV_Date = Convert.ToDateTime(row["REV_Date"]);
                        }
                        objPO.Order_Qty = Convert.ToDecimal(row["Order_Qty"]);
                        list.Add(objPO);
                    }
                }

            }
            catch { }
            return PartialView("_ProductionOrderList", list);
        }

        public JsonResult getNumberingSchemeRecord()
        {
            List<SchemaNumber> list = new List<SchemaNumber>();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            DataTable Schemadt = objdata.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "156", "Y");
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

        public ActionResult GetProductionReceiptDetailsProductList(Int64 DetailsID = 0)
        {
            FGReceivedProduct bomproductdataobj = new FGReceivedProduct();
            List<FGReceivedProduct> bomproductdata = new List<FGReceivedProduct>();
            try
            {
                if (TempData["DetailsID"] != null)
                {
                    DetailsID = Convert.ToInt64(TempData["DetailsID"]);
                    
                }
                if (DetailsID > 0)
                {
                    DataTable objData = new DataTable();
                    if (TempData["ProductionIssueID"] != null)
                    {
                        objData = objPI.GetMaterialsIssueData("GetFGData", Convert.ToInt64(TempData["ProductionIssueID"]), DetailsID);
                    }
                    else if (TempData["ProductionReceiptID"] != null)
                    {
                        objData = objPR.GetProductionReceiptData("GetReturnFGDataData", Convert.ToInt64(TempData["ProductionReceiptID"]), DetailsID);
                    }
                    //else
                    //{
                    //    objData = objdata.GetBOMProductEntryListByID("GetBOMEntryProductsData", DetailsID);
                    //}
                    if (objData != null && objData.Rows.Count > 0)
                    {
                        DataTable dt = objData;
                        foreach (DataRow row in dt.Rows)
                        {
                            bomproductdataobj = new FGReceivedProduct();
                            bomproductdataobj.SlNO = Convert.ToString(row["SlNO"]);
                            bomproductdataobj.BOMProductsID = Convert.ToString(row["BOMProductsID"]);
                            bomproductdataobj.Details_ID = Convert.ToString(row["Details_ID"]);
                            bomproductdataobj.ProductName = Convert.ToString(row["sProducts_Code"]);
                           // bomproductdataobj.ProductId = Convert.ToString(row["ProductID"]);
                            bomproductdataobj.ProductDescription = Convert.ToString(row["sProducts_Name"]);
                            bomproductdataobj.DesignNo = Convert.ToString(row["DesignNo"]);
                            bomproductdataobj.ItemRevisionNo = Convert.ToString(row["ItemRevisionNo"]);
                           
                            bomproductdataobj.ProductUOM = Convert.ToString(row["StkUOM"]);
                            bomproductdataobj.StockUOMId = Convert.ToInt64(row["StockUOMId"]);
                            bomproductdataobj.Warehouse = Convert.ToString(row["WarehouseName"]);
                            bomproductdataobj.Price = Convert.ToString(row["Price"]);
                            //bomproductdataobj.Amount = Convert.ToString(row["Amount"]);
                            //bomproductdataobj.BOMNo = Convert.ToString(row["BOMNo"]);
                            //bomproductdataobj.RevNo = Convert.ToString(row["RevNo"]);
                            //if (row["RevDate"] != null && Convert.ToString(row["RevDate"]) != "" && Convert.ToString(row["RevDate"]) != " " && Convert.ToString(row["RevDate"]) != null)
                            //{
                            //    bomproductdataobj.RevDate = Convert.ToDateTime(row["RevDate"]).ToString("dd-MM-yyyy");
                            //}
                            //else
                            //{
                            //    bomproductdataobj.RevDate = " ";
                            //}
                            bomproductdataobj.Remarks = Convert.ToString(row["Remarks"]);
                            bomproductdataobj.ProductsWarehouseID = Convert.ToString(row["WarehouseID"]);
                            //bomproductdataobj.Tag_Details_ID = Convert.ToString(row["Tag_Details_ID"]);
                            //bomproductdataobj.Tag_Production_ID = Convert.ToString(row["Tag_Production_ID"]);
                            //bomproductdataobj.RevNo = Convert.ToString(row["RevNo"]);



                            //if (TempData["Production_ID"] != null && TempData["WorkOrderID"] == null)
                            //{
                               // bomproductdataobj.ProductQty = Convert.ToString(row["BalQty"]);
                                bomproductdataobj.BalQty = Convert.ToString(row["BalQty"]);
                               // bomproductdataobj.Amount = Convert.ToString(Math.Round((Convert.ToDecimal(bomproductdataobj.BalQty) * Convert.ToDecimal(bomproductdataobj.Price)), 2));
                           // }
                            //else
                            //{
                            //    bomproductdataobj.BalQty = Convert.ToString(row["StkQty"]);
                            //}
                                if (TempData["ProductionReceiptID"] != null)
                                {
                                    bomproductdataobj.ProductQty = Convert.ToString(row["StkQty"]);
                                    bomproductdataobj.Amount = Convert.ToString(Math.Round((Convert.ToDecimal(bomproductdataobj.ProductQty) * Convert.ToDecimal(bomproductdataobj.Price)), 2));
                                }
                                else if (TempData["ProductionIssueID"] != null)
                                {
                                    bomproductdataobj.ProductQty = Convert.ToString(row["BalQty"]);
                                    bomproductdataobj.Amount = Convert.ToString(Math.Round((Convert.ToDecimal(bomproductdataobj.ProductQty) * Convert.ToDecimal(bomproductdataobj.Price)), 2));
                                }


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
                            bomproductdataobj.Remarks = Convert.ToString(row["Remarks"]);
                            bomproductdataobj.InventoryType = Convert.ToString(row["InventoryType"]);
                            bomproductdata.Add(bomproductdataobj);
                        }
                        ViewData["BOMEntryProductsTotalAm"] = bomproductdata.Sum(x => Convert.ToDecimal(x.Amount)).ToString();
                    }
                }
            }
            catch { }
            TempData.Keep();
            return PartialView("_ProductionReceiptBOMProductGrid", bomproductdata);
        }

        [HttpPost]
        public JsonResult getProductSerialNo(String productid = "0", String warehouse = "0", String Batch = "0", String PostingDate = null)
        {
            List<ProductSerial> list = new List<ProductSerial>();
            try
            {
                DataTable dt = new DataTable();
                dt = objPI.GetManufacturingProductionIssue("GetSerialByProductID", productid, Convert.ToString(Session["LastFinYear"]), null, Convert.ToString(Session["LastCompany"]), null, warehouse, Batch, "0", PostingDate);
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
        public JsonResult getProductWiseWarehouseRecord(String branchid = null, String productid = null, string WorkcenterId = "0")
        {
            List<BranchWarehouse> list = new List<BranchWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                dt = objPI.GetManufacturingProductionIssue("GetFGReturnWareHouseByProductID", productid, Convert.ToString(Session["LastFinYear"]), branchid, Convert.ToString(Session["LastCompany"]), multiwarehouse, WorkcenterId);
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
        public JsonResult HeadergetProductWiseWarehouseRecord(String branchid = null, String productid = null,string ProductionIssueID=null)
        {
            List<BranchWarehouse> list = new List<BranchWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                dt = objPR.GetManufacturingFGReturn("GetHeaderFinishFGReturnWareHouseByProductID", productid, Convert.ToString(Session["LastFinYear"]), branchid, Convert.ToString(Session["LastCompany"]), multiwarehouse, null, null, null, null, ProductionIssueID);
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
        public JsonResult getBatchRecord(String warehouseid = null, String ProductID = null, string ProductionIssueID=null)
        {
            List<BatchWarehouse> list = new List<BatchWarehouse>();
            try
            {
               
                DataTable dt = new DataTable();
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                dt = objPR.GetManufacturingFGReturn("GetBatchByProductIDWarehouseFGReturnRawmaterials", ProductID, Convert.ToString(Session["LastFinYear"]), null, Convert.ToString(Session["LastCompany"]), multiwarehouse, warehouseid, null, null, null, ProductionIssueID);
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
        public bool IsBarcodeGeneratete()
        {
            bool IsGeneratete = false;

            try
            {
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
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

        [HttpPost]
        public JsonResult AvailableStockCheck(String WarehouseID = null, String ProductID = null, string ProductionIssueID = null, string ViewBatch = null, string Branch = null)
        {
            List<BatchWarehouse> list = new List<BatchWarehouse>();
            string balncevalue = "";
            try
            {
                DataTable dt = new DataTable();
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                if (ViewBatch != "" && ViewBatch != "0")
                {
                    dt = objPR.GetManufacturingProductionIssueWarehousedetailsForFGReturn("AvailableStockCheckBalanceBatchFGReturn", ProductID, Convert.ToString(Session["LastFinYear"]), Branch, Convert.ToString(Session["LastCompany"]), multiwarehouse, WarehouseID, ViewBatch, null, null, ProductionIssueID);
                }
                else
                {
                    dt = objPR.GetManufacturingProductionIssueWarehousedetailsForFGReturn("AvailableStockCheckBalanceWithoutBatchFGReturn", ProductID, Convert.ToString(Session["LastFinYear"]), Branch, Convert.ToString(Session["LastCompany"]), multiwarehouse, WarehouseID, ViewBatch, null, null, ProductionIssueID);
                }
                if (dt.Rows.Count > 0)
                {
                    //BatchWarehouse obj = new BatchWarehouse();
                    //foreach (DataRow item in dt.Rows)
                    //{
                    //    obj = new BatchWarehouse();
                    //    obj.BatchID = Convert.ToString(item["BatchID"]);
                    //    obj.BatchName = Convert.ToString(item["BatchName"]);
                    //    list.Add(obj);
                    //}
                    balncevalue = Convert.ToString(dt.Rows[0]["BalanceBatchValue"]);
                }
            }
            catch { }
            return Json(balncevalue);
        }

        [HttpPost]
        public JsonResult HeadergetBatchRecord(String warehouseid = null, String ProductID = null, string ProductionIssueID=null)
        {
            List<BatchWarehouse> list = new List<BatchWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                dt = objPR.GetManufacturingFGReturn("GetBatchByProductIDWarehouseFGReturn", ProductID, Convert.ToString(Session["LastFinYear"]), null, Convert.ToString(Session["LastCompany"]), multiwarehouse, warehouseid, null, null, null, ProductionIssueID);
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
        public JsonResult HeadergetWarehouseRecord(Int32 branchid = 0, Int64 ProductionIssueID=0, Int64 ProductId = 0)
        {
            List<BranchWarehouse> list = new List<BranchWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                string strBranch = branchid.ToString();
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                //if (multiwarehouse != "1")
                //{
                    dt = oDBEngine.GetDataTable("select  bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building inner join Trans_StockBranchWarehouseDetails  on bui_id=StockBranchWarehouseDetail_WarehouseId Where Doc_Type='FGRecIN' and isnull(In_Doc_Number,0)='" + ProductionIssueID + "' and StockBranchWarehouseDetail_ProductId='" + ProductId + "' and IsNull(bui_BranchId,0) in ('0','" + strBranch + "') order by bui_Name");
                //}
                //else
                //{
                //    dt = oDBEngine.GetDataTable("EXEC [GET_BRANCHWISEWAREHOUSE] '1','" + strBranch + "'");
                //}
                if (dt.Rows.Count > 0 && dt !=null)
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
        public JsonResult getWarehouseRecord(Int32 branchid = 0, Int64 WorkCenterId = 0)
        {
            List<BranchWarehouse> list = new List<BranchWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                string strBranch = branchid.ToString();
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                //if (multiwarehouse != "1")
                //{
                    dt = oDBEngine.GetDataTable("select  bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building Where ISNULL(tbl_master_building.IsWorkCenter,0)=1 and WorkCenterID='" + Convert.ToInt64(WorkCenterId) + "' and IsNull(bui_BranchId,0) in ('0','" + strBranch + "') order by bui_Name");
                //}
                //else
                //{
                //    dt = oDBEngine.GetDataTable("EXEC [GET_MFCBRANCHWISEWAREHOUSE] '1','" + strBranch + "', '" + WorkCenterId + "'");
                //}
                    if (dt.Rows.Count > 0 && dt !=null )
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
        public JsonResult GetAvalStockValue(String branchid = null, String productid = null, string WarehouseId = null, string BatchId = null)
        {
            MaterialIssueModel objPI = new MaterialIssueModel();
            string StockValue = "0.0000";
            try
            {
                DataTable dt = new DataTable();

                if (BatchId != null && BatchId != "" && BatchId != "0")
                {
                    dt = objPI.GetAvailStockData("AvailableStockBatch", branchid, productid, WarehouseId, BatchId);
                }
                else
                {
                    dt = objPI.GetAvailStockData("AvailableStock", branchid, productid, WarehouseId);
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    StockValue = Convert.ToString(dt.Rows[0]["AvlStk"]);
                }
            }
            catch { }
            return Json(StockValue);
        }

        [HttpPost]
        public JsonResult setStockWarehouseList(List<udtMaterialStockProduct> items, String SrlNo, Int64 Unit)
        {
            Boolean Success = false;
            String Message = "";
            Boolean IsProcess = false;
            try
            {
                DataTable dt = ToDataTable(items);
                Session["PIssue_WarehouseData"] = dt;

                DataTable dtWarehouse = ToDataTable(items.Where(x => x.Product_SrlNo == SrlNo).ToList());
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

        [HttpPost]
        public JsonResult HeadersetStockWarehouseList(List<FGFinishudtMaterialStockProduct> items, String SrlNo, Int64 Unit)
        {
            Boolean Success = false;
            String Message = "";
            Boolean IsProcess = false;
            try
            {
                DataTable dt = ToDataTable(items);
                Session["HeaderPIssue_WarehouseData"] = dt;

                //DataTable dtWarehouse = ToDataTable(items.Where(x => x.Product_SrlNo == SrlNo).ToList());
                //DataTable result = objPI.GetProductionIssueData("StockWarehouseBalCheck", 0, Unit, dtWarehouse);
                //foreach (DataRow dr in result.Rows)
                //{
                //    IsProcess = Convert.ToBoolean(dr["IsProcess"]);
                //}
                IsProcess = true;
                Success = true;

            }
            catch { }
            Message = Success.ToString() + "|" + IsProcess.ToString();
            return Json(Message);
        }

        [WebMethod]
        public JsonResult EditWarehouseData(String SrlNO)
        {
            ReturnFGReceivedFinishItemDetails ret = new ReturnFGReceivedFinishItemDetails();

            DataTable dt = (DataTable)TempData["ProductsDetails"];

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (SrlNO.ToString() == item["SrlNO"].ToString())
                    {
                        ret.Guids = item["HIddenID"].ToString();
                        ret.SrlNO = item["SrlNO"].ToString();
                        ret.FinishItemName = item["FinishItemName"].ToString();
                        ret.FinishItemDescription = item["FinishItemDescription"].ToString();
                        ret.FinishDrawingNo = item["FinishDrawingNo"].ToString();
                        ret.FinishItemRevNo = item["FinishItemRevNo"].ToString();
                        ret.Qty = item["Qty"].ToString();
                        ret.FinishUOM = item["FinishUOM"].ToString();
                        ret.FinishPrice = item["FinishPrice"].ToString();
                        ret.FinishAmount = item["FinishAmount"].ToString();
                        ret.FGReturn_Id = item["FGReturn_Id"].ToString();
                        ret.FGReceivedID = item["FGReceivedID"].ToString();
                        ret.FinishUOMId = item["FinishUOMId"].ToString();
                        ret.FinishProductsID = item["FinishProductsID"].ToString();
                        ret.FinishWareHouseId = item["FinishWareHouseId"].ToString();
                        ret.OldFGQuantity = item["OldFGQuantity"].ToString();
                        ret.MaxBalFGQuantity = item["MaxBalFGQuantity"].ToString();
                        ret.FGWareHouseName = item["FGWareHouseName"].ToString();
                        ret.FinishInventoryType = item["FinishInventoryType"].ToString();
                        break;
                    }
                }
            }
            TempData["ProductsDetails"] = dt;
            TempData.Keep();
            return Json(ret);
        }

        [ValidateInput(false)]
        public ActionResult BatchEditingProductionReceipt(DevExpress.Web.Mvc.MVCxGridViewBatchUpdateValues<FGReceivedProduct, int> updateValues, ReturnFGReceivedViewModel options)
        {
            TempData["Count"] = (int)TempData["Count"] + 1;
            TempData.Keep();
            String NumberScheme = "";
            String Message = "";

            if ((int)TempData["Count"] != 2)
            {
                Boolean IsProcess = false;
                List<FGReceivedProduct> list = new List<FGReceivedProduct>();
                
                if (updateValues.Update.Count > 0 && Convert.ToInt64(options.Details_ID) > 0)
                {
                    List<udtFGREceivedDetails> udtlist = new List<udtFGREceivedDetails>();
                    udtFGREceivedDetails obj = null;

                    foreach (var item in updateValues.Update)
                    {
                        if (Convert.ToInt64(item.BOMProductsID) > 0)
                        {
                            obj = new udtFGREceivedDetails();
                            obj.BOMProductsID = Convert.ToInt64(item.BOMProductsID);
                            obj.Qty = Convert.ToDecimal(item.ProductQty);
                            obj.UomId = Convert.ToInt64(item.StockUOMId);
                            obj.Price = Convert.ToDecimal(item.Price);
                            obj.Amount = Convert.ToDecimal(item.Amount);
                            obj.Remarks = Convert.ToString(item.Remarks);
                            udtlist.Add(obj);
                        }
                    }
                    if (udtlist.Count > 0)
                    {
                        if (options.ProductionReceiptID > 0)
                        {
                            IsProcess = ProductionReceiptBOMProductInsertUpdate(udtlist, options);
                        }
                        else
                        {
                            NumberScheme = checkNMakePOCode(options.OrderNo, Convert.ToInt32(options.Order_SchemaID), Convert.ToDateTime(options.OrderDate));
                            if (NumberScheme == "ok")
                            {
                                IsProcess = ProductionReceiptBOMProductInsertUpdate(udtlist, options);
                            }
                            else
                            {
                                Message = NumberScheme;
                            }
                        }

                        TempData["DetailsID"] = null;
                        TempData["ProductionIssueID"] = null;
                    }
                }


                TempData["Count"] = 1;
                TempData.Keep();
                ViewData["OrderNo"] = JVNumStr;
                ViewData["Success"] = IsProcess;
                ViewData["Message"] = Message;
            }
            return PartialView("_ProductionReceiptBOMProductGrid", updateValues.Update);
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
                        sqlQuery = "SELECT max(tjv.ReturnFGReceived_No) FROM Mfc_ReturnFGReceived tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.ReturnFGReceived_No))) = 1 and ReturnFGReceived_No like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.ReturnFGReceived_No) FROM Mfc_ReturnFGReceived tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.ReturnFGReceived_No))) = 1 and ReturnFGReceived_No like '%" + sufxCompCode + "'";
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
                            sqlQuery = "SELECT max(tjv.ReturnFGReceived_No) FROM Mfc_ReturnFGReceived tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + paddCounter + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.ReturnFGReceived_No))) = 1 and ReturnFGReceived_No like '" + prefCompCode + "%'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }
                        else
                        {
                            int i = startNo.Length;
                            while (i < paddCounter)
                            {


                                sqlQuery = "SELECT max(tjv.ReturnFGReceived_No) FROM Mfc_ReturnFGReceived tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + i + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.ReturnFGReceived_No))) = 1 and ReturnFGReceived_No like '" + prefCompCode + "%'";

                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.ReturnFGReceived_No)=" + i;
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
                                sqlQuery = "SELECT max(tjv.ReturnFGReceived_No) FROM Mfc_ReturnFGReceived tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + (i - 1) + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.ReturnFGReceived_No))) = 1 and ReturnFGReceived_No like '" + prefCompCode + "%'";
                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.ReturnFGReceived_No)=" + (i - 1);
                                }
                                dtC = oDBEngine.GetDataTable(sqlQuery);
                            }

                        }

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.ReturnFGReceived_No) FROM Mfc_ReturnFGReceived tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.ReturnFGReceived_No))) = 1 and ReturnFGReceived_No like '" + prefCompCode + "%'";
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
                    sqlQuery = "SELECT ReturnFGReceived_No FROM Mfc_ReturnFGReceived WHERE ReturnFGReceived_No LIKE '" + manual_str.Trim() + "'";
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
        public JsonResult EditModeWarehouseLiust(List<udtMaterialStockProduct> items, List<FGFinishudtMaterialStockProduct> Finishitems)
        {
            Boolean Success = false;
            String Message = "";
            Boolean IsProcess = false;
            try
            {
                DataTable dt = ToDataTable(items);
                Session["PIssue_WarehouseData"] = dt;
                DataTable dtFinish = ToDataTable(Finishitems);
                Session["HeaderPIssue_WarehouseData"] = dtFinish;


                Success = true;

            }
            catch { }
            Message = Success.ToString() + "|" + IsProcess.ToString();
            return Json(Message);
        }
        public Boolean ProductionReceiptBOMProductInsertUpdate(List<udtFGREceivedDetails> obj, ReturnFGReceivedViewModel obj2)
        {
            Boolean Success = false;


            try
            {
                DataTable dtBOM_PRODUCTS = new DataTable();
                dtBOM_PRODUCTS = ToDataTable(obj);


                DataTable dtWarehouse = new DataTable();
              

                if (Session["PIssue_WarehouseData"] != null)
                {
                    dtWarehouse = (DataTable)Session["PIssue_WarehouseData"];
                }

                DataTable dtFinishWarehouse = new DataTable();
               

                if (Session["HeaderPIssue_WarehouseData"] != null)
                {
                    dtFinishWarehouse = (DataTable)Session["HeaderPIssue_WarehouseData"];
                }

                if (dtFinishWarehouse.Columns.Contains("MfgDate"))
                {
                    dtFinishWarehouse.Columns.Remove("MfgDate");
                    dtFinishWarehouse.AcceptChanges();
                }
                if (dtFinishWarehouse.Columns.Contains("ExpiryDate"))
                {
                    dtFinishWarehouse.Columns.Remove("ExpiryDate");
                    dtFinishWarehouse.AcceptChanges();
                }

                DataTable dtProductFinish = new DataTable();
                dtProductFinish = (DataTable)TempData["ProductsDetails"];

                List<ReturnFGReceivedFinishItemDetails> udt = new List<ReturnFGReceivedFinishItemDetails>();
                if (dtProductFinish != null && dtProductFinish.Rows.Count > 0)
                {
                    foreach (DataRow item in dtProductFinish.Rows)
                    {
                        ReturnFGReceivedFinishItemDetails obj1 = new ReturnFGReceivedFinishItemDetails();
                        obj1.SrlNO = Convert.ToString(item["SrlNO"]);
                        obj1.FinishItemName = Convert.ToString(item["FinishItemName"]);
                        obj1.FinishItemDescription = Convert.ToString(item["FinishItemDescription"]);
                        obj1.FinishDrawingNo = Convert.ToString(item["FinishDrawingNo"]);
                        obj1.FinishItemRevNo = Convert.ToString(item["FinishItemRevNo"]);
                        obj1.Qty = Convert.ToString(item["Qty"]);
                        if (obj1.Qty == "")
                        {
                            obj1.Qty = "0.0000";
                        }
                        obj1.FinishUOM = Convert.ToString(item["FinishUOM"]);
                        obj1.FinishPrice = Convert.ToString(item["FinishPrice"]);
                        obj1.FinishAmount = Convert.ToString(item["FinishAmount"]);
                        if (obj1.FinishPrice == "")
                        {
                            obj1.FinishPrice = "0.00";
                        }
                        if (obj1.FinishAmount == "")
                        {
                            obj1.FinishAmount = "0.00";
                        }

                        obj1.FGReceivedID = Convert.ToString(item["FGReceivedID"]);
                        if (obj1.FGReceivedID == "")
                        {
                            obj1.FGReceivedID = "0";
                        }

                        obj1.FGReturn_Id = Convert.ToString(item["FGReturn_Id"]);
                        if (obj1.FGReturn_Id == "")
                        {
                            obj1.FGReturn_Id = "0";
                        }
                        obj1.FinishUOMId = Convert.ToString(item["FinishUOMId"]);
                        if (obj1.FinishUOMId == "")
                        {
                            obj1.FinishUOMId = "0";
                        }
                        obj1.FinishProductsID = Convert.ToString(item["FinishProductsID"]);
                        if (obj1.FinishProductsID == "")
                        {
                            obj1.FinishProductsID = "0";
                        }

                        obj1.FinishWareHouseId = Convert.ToString(item["FinishWareHouseId"]);
                        if (obj1.FinishWareHouseId == "")
                        {
                            obj1.FinishWareHouseId = "0";
                        }

                        obj1.OldFGQuantity = Convert.ToString(item["OldFGQuantity"]);
                        if (obj1.OldFGQuantity == "")
                        {
                            obj1.OldFGQuantity = "0.0000";
                        }
                        obj1.MaxBalFGQuantity = Convert.ToString(item["MaxBalFGQuantity"]);
                        if (obj1.MaxBalFGQuantity == "")
                        {
                            obj1.MaxBalFGQuantity = "0.0000";
                        }


                        udt.Add(obj1);
                    }

                }

                DataTable dt_PRODUCTS = new DataTable();
                dt_PRODUCTS = ToDataTable(udt);
                if (dt_PRODUCTS.Columns.Contains("Guids"))
                {
                    dt_PRODUCTS.Columns.Remove("Guids");
                    dt_PRODUCTS.AcceptChanges();
                }

                if (dt_PRODUCTS.Columns.Contains("FinishUpdateEdit"))
                {
                    dt_PRODUCTS.Columns.Remove("FinishUpdateEdit");
                    dt_PRODUCTS.AcceptChanges();
                }
                if (dt_PRODUCTS.Columns.Contains("MaxBalFGQuantity"))
                {
                    dt_PRODUCTS.Columns.Remove("MaxBalFGQuantity");
                    dt_PRODUCTS.AcceptChanges();
                }
                if (dt_PRODUCTS.Columns.Contains("OldFGQuantity"))
                {
                    dt_PRODUCTS.Columns.Remove("OldFGQuantity");
                    dt_PRODUCTS.AcceptChanges();
                }
                if (dt_PRODUCTS.Columns.Contains("FGWareHouseName"))
                {
                    dt_PRODUCTS.Columns.Remove("FGWareHouseName");
                    dt_PRODUCTS.AcceptChanges();
                }
                if (dt_PRODUCTS.Columns.Contains("FinishInventoryType"))
                {
                    dt_PRODUCTS.Columns.Remove("FinishInventoryType");
                    dt_PRODUCTS.AcceptChanges();
                }

                DataSet dt = new DataSet();
                if (Convert.ToInt64(obj2.Details_ID) > 0)
                {
                    if (!String.IsNullOrEmpty(obj2.OrderNo) && obj2.OrderNo.ToLower() != "auto")
                    {
                        JVNumStr = obj2.OrderNo;
                    }
                   // Rev Sanchita [ parameter obj2.ProjectID added]
                    dt = objPR.ProductionReceiptBOMProductInsertUpdate("INSERTRECEIPTBOM", obj2.ProductionReceiptID, obj2.ProductionIssueID, obj2.ProductionOrderID, obj2.Details_ID, obj2.WorkOrderID, obj2.Production_ID, Convert.ToInt64(obj2.WorkCenterID), JVNumStr, obj2.Order_SchemaID, Convert.ToDateTime(obj2.OrderDate),
                        obj2.Order_Qty, obj2.ActualAdditionalCost, obj2.TotalCost, obj2.BRANCH_ID, Convert.ToInt64(Session["userid"]), obj2.strRemarks, obj2.FGPrice,obj2.TotalAmount,
                        dtBOM_PRODUCTS, obj2.WarehouseID, Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), dt_PRODUCTS, dtWarehouse, dtFinishWarehouse,
                        obj2.ProjectID);
                }
                Session["PIssue_WarehouseData"] = null;
                Session["dtFinishWarehouse"] = null;
                Session["HeaderPIssue_WarehouseData"] = null;
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

        public ActionResult FGReceivedList()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/FGReceivedList", "ReturnFGReceived");
            ReturnFGReceivedViewModel obj = new ReturnFGReceivedViewModel();
            TempData.Clear();
            ViewBag.CanAdd = rights.CanAdd;
            return View("FGReceivedList", obj);
        }

        public ActionResult GetProductionReceiptList()
        {
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            List<ReturnFGReceivedViewModel> list = new List<ReturnFGReceivedViewModel>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/FGReceivedList", "ReturnFGReceived");
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
                        dt = oDBEngine.GetDataTable("select * from V_ReturnFGReceivedList where BRANCH_ID =" + BranchID + " AND (Receipt_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "')  order by ProductionReceiptID desc ");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("select * from V_ReturnFGReceivedList where Receipt_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' order by ProductionReceiptID desc ");
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
                    ReturnFGReceivedViewModel obj = new ReturnFGReceivedViewModel();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new ReturnFGReceivedViewModel();
                       // obj.WorkOrderID = Convert.ToInt64(item["WorkOrderID"]);
                        // obj.ProductionOrderID = Convert.ToInt64(item["ProductionOrderID"]);
                        obj.ProductionReceiptID = Convert.ToInt64(item["ProductionReceiptID"]);
                       // obj.WorkOrderID = Convert.ToInt64(item["WorkOrderID"]);
                       // obj.ProductionOrderID = Convert.ToInt64(item["ProductionOrderID"]);
                        obj.WorkCenterID = Convert.ToString(item["WorkCenterID"]);
                        obj.WorkCenterID = Convert.ToString(item["WorkCenterID"]);
                        obj.Receipt_No = Convert.ToString(item["Receipt_No"]);
                        obj.Receipt_Date = Convert.ToDateTime(item["Receipt_Date"]);
                        obj.ProductionIssueID = Convert.ToInt64(item["ProductionIssueID"]);
                        obj.ProductionIssueNo = Convert.ToString(item["ProductionIssueNo"]);
                        obj.ProductionIssueDate = Convert.ToDateTime(item["ProductionIssueDate"]);
                        //obj.WorkOrderNo = Convert.ToString(item["WorkOrderNo"]);
                        obj.Order_Qty = Convert.ToDecimal(item["Receipt_Qty"]);
                        obj.WorkCenterCode = Convert.ToString(item["WorkCenterCode"]);
                        obj.WorkCenterDescription = Convert.ToString(item["WorkCenterDescription"]);
                        obj.BRANCH_ID = Convert.ToInt64(item["BRANCH_ID"]);
                       // obj.BOMNo = Convert.ToString(item["BOM_No"]);
                       // obj.RevNo = Convert.ToString(item["REV_No"]);
                       // obj.BOM_Date = Convert.ToDateTime(item["BOM_Date"]);
                       // obj.ProductionOrderNo = Convert.ToString(item["ProductionOrderNo"]);
                       // obj.ProductionOrderDate = Convert.ToDateTime(item["ProductionOrderDate"]);
                        //obj.WorkOrderDate = Convert.ToDateTime(item["WorkOrderDate"]);
                        //if (Convert.ToString(item["REV_Date"]) != "")
                        //{
                        //    obj.REV_Date = Convert.ToDateTime(item["REV_Date"]);
                        //}
                        //else
                        //{
                        //    obj.REV_Date = null;
                        //}
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
                        //obj.PartNoName = Convert.ToString(item["PartNoName"]);
                       // obj.DesignNo = Convert.ToString(item["DesignNo"]);
                      //  obj.ItemRevNo = Convert.ToString(item["ItemRevisionNo"]);
                      //  obj.Description = Convert.ToString(item["sProducts_Name"]);
                        obj.Proj_Code = Convert.ToString(item["Proj_Code"]);
                        obj.Proj_Name = Convert.ToString(item["Proj_Name"]);
                        list.Add(obj);
                    }
                }
            }
            catch { }
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            return PartialView("_ProductionReceiptDataList", list);
        }
        [HttpPost]
        public JsonResult GetWarehouseList(Int64 ProductionIssueID = 0)
        {
            List<MaterialIssueStkWarehouse> list = new List<MaterialIssueStkWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                if (ProductionIssueID > 0)
                {
                    dt = objPI.GetFGReceivedWarehouseData("GetReturnFGWarehouseData", ProductionIssueID, 0, new DataTable());
                    if (dt.Rows.Count > 0)
                    {
                        MaterialIssueStkWarehouse obj = new MaterialIssueStkWarehouse();
                        foreach (DataRow item in dt.Rows)
                        {
                            obj = new MaterialIssueStkWarehouse();
                            obj.StkWarehouseID = Convert.ToInt64(item["StkWarehouseID"]);
                            obj.ProductionIssueID = Convert.ToInt64(item["ProductionIssueID"]);
                            obj.ProductionIssueDetailsID = Convert.ToInt64(item["ProductionIssueDetailsID"]);
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
        public JsonResult getBatchRecordForProduct(String warehouseid = null, String ProductID = null, string BatchID = null)
        {
            List<BatchWarehouse> list = new List<BatchWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                dt = objPI.GetManufacturingProductionIssue("GetReturnFGBatchByProductIDWarehouse", ProductID, Convert.ToString(Session["LastFinYear"]), null, Convert.ToString(Session["LastCompany"]), multiwarehouse, warehouseid, BatchID);
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
        public JsonResult HeaderGetWarehouseList(Int64 ProductionIssueID = 0)
        {
            List<MaterialIssueStkWarehouse> list = new List<MaterialIssueStkWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                if (ProductionIssueID > 0)
                {
                    dt = objPI.GetFGReceivedWarehouseData("GetReturnFGFinishWarehouseData", ProductionIssueID, 0, new DataTable());
                    if (dt.Rows.Count > 0)
                    {
                        MaterialIssueStkWarehouse obj = new MaterialIssueStkWarehouse();
                        foreach (DataRow item in dt.Rows)
                        {
                            obj = new MaterialIssueStkWarehouse();
                            obj.StkWarehouseID = Convert.ToInt64(item["StkWarehouseID"]);
                            obj.ProductionIssueID = Convert.ToInt64(item["ProductionIssueID"]);
                            obj.ProductionIssueDetailsID = Convert.ToInt64(item["ProductionIssueDetailsID"]);
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
        public JsonResult HeadergetBatchRecordForFGFinishBatch(String warehouseid = null, String ProductID = null, string BatchID = null)
        {
            List<BatchWarehouseForFGFinishItem> list = new List<BatchWarehouseForFGFinishItem>();
            try
            {
                DataTable dt = new DataTable();
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                dt = objPI.GetManufacturingProductionIssue("GetBatchByProductIDWarehouseForReturnFGFinishItem", ProductID, Convert.ToString(Session["LastFinYear"]), null, Convert.ToString(Session["LastCompany"]), multiwarehouse, warehouseid, BatchID);
                if (dt.Rows.Count > 0)
                {
                    BatchWarehouseForFGFinishItem obj = new BatchWarehouseForFGFinishItem();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new BatchWarehouseForFGFinishItem();
                        obj.BatchID = Convert.ToString(item["BatchID"]);
                        obj.BatchName = Convert.ToString(item["BatchName"]);
                        obj.MfgDate = Convert.ToString(item["MfgDate"]);
                        obj.ExpiryDate = Convert.ToString(item["ExpiryDate"]);
                        list.Add(obj);
                    }
                }
            }
            catch { }
            return Json(list);
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
            settings.Name = "Receipt from Production";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Receipt from Production";
            //String ID = Convert.ToString(TempData["EmployeesTargetListDataTable"]);
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "Receipt_No" || datacolumn.ColumnName == "Receipt_Date" || datacolumn.ColumnName == "ProductionIssueNo" || datacolumn.ColumnName == "ProductionIssueDate" ||
                    datacolumn.ColumnName == "CreatedBy" || datacolumn.ColumnName == "ModifyBy" || datacolumn.ColumnName == "CreateDate" || datacolumn.ColumnName == "ModifyDate"
                    )
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

                        if (datacolumn.ColumnName == "ProductionIssueNo")
                        {
                            column.Caption = "Production Issue No";
                            column.VisibleIndex = 2;
                        }

                        if (datacolumn.ColumnName == "ProductionIssueDate")
                        {
                            column.Caption = "Production Issue Date";
                            column.VisibleIndex = 3;
                        }

                        else if (datacolumn.ColumnName == "CreatedBy")
                        {
                            column.Caption = "Entered By";
                            column.VisibleIndex = 6;
                        }
                        else if (datacolumn.ColumnName == "CreateDate")
                        {
                            column.Caption = "Entered On";
                            column.VisibleIndex = 7;
                        }
                        else if (datacolumn.ColumnName == "ModifyBy")
                        {
                            column.Caption = "Modified By";
                            column.VisibleIndex = 8;
                        }
                        else if (datacolumn.ColumnName == "ModifyDate")
                        {
                            column.Caption = "Modified On";
                            column.VisibleIndex = 9;
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

        public JsonResult SetPRDataByID(Int64 productionreceiptid = 0, Int16 IsView = 0)
        {
            Boolean Success = false;
            try
            {
                TempData["ProductionReceiptID"] = productionreceiptid;
                TempData["IsView"] = IsView;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }

        public JsonResult RemovePRDataByID(Int32 productionreceiptid)
        {
            //Boolean Success = false;
            ReturnData obj = new ReturnData();
            try
            {
                var datasetobj = objPR.GetREturnFGReceiptDeleteData("RemoveRetFGData", productionreceiptid, 0, Convert.ToInt64(Session["userid"]));
                if (datasetobj.Rows.Count > 0)
                {

                    foreach (DataRow item in datasetobj.Rows)
                    {
                        obj.Success = Convert.ToBoolean(item["Success"]);
                        obj.Message = Convert.ToString(item["Message"]);
                    }
                }
            }
            catch { }
            return Json(obj);
        }

        public ActionResult GetPIList(ReturnFGReceivedViewModel ReturnFGReceivedViewModel)
        {
            List<FGIssueReturnViewModel> list = new List<FGIssueReturnViewModel>();
            try
            {
                FGIssueReturnViewModel objWOL = new FGIssueReturnViewModel();
             
                string Issueid = Convert.ToString(TempData["ProductionReceiptID"]);
                if (Issueid == "")
                {
                    Issueid = "0";
                }

                DataTable objData = objIR.GetFGReturnData("GetAllMaterialsIssueReciptModuleData", Convert.ToInt64(Issueid), ReturnFGReceivedViewModel.dtOrderDate);

                if (objData != null && objData.Rows.Count > 0)
                {
                    DataTable dt = objData;
                    foreach (DataRow row in dt.Rows)
                    {
                        objWOL = new FGIssueReturnViewModel();
                        objWOL.ProductionIssueID = Convert.ToInt64(row["ProductionIssueID"]);
                        objWOL.Issue_No = Convert.ToString(row["Issue_No"]);
                        objWOL.WorkOrderID = Convert.ToInt64(row["WorkOrderID"]);
                        objWOL.OrderNo = Convert.ToString(row["OrderNo"]);
                        //objWOL.ProductionOrderNo = Convert.ToString(row["ProductionOrderNo"]);
                        //objWOL.ProductionOrderID = Convert.ToInt64(row["ProductionOrderID"]);
                        objWOL.Details_ID = Convert.ToInt64(row["Details_ID"]);
                       // objWOL.BOMNo = Convert.ToString(row["BOM_No"]);
                        objWOL.Issue_Date = Convert.ToDateTime(row["Issue_Date"]);
                        //objWOL.RevNo = Convert.ToString(row["REV_No"]);
                        objWOL.FinishedItem = Convert.ToString(row["ProductName"]);
                        objWOL.FinishedUom = Convert.ToString(row["FinishedUom"]);
                        //objWOL.ProductionOrderDate = Convert.ToDateTime(row["ProductionOrderDate"]).ToString("dd-MM-yyyy");
                        objWOL.Warehouse = Convert.ToString(row["Warehouse"]);
                        objWOL.HeaderWarehouseId = Convert.ToInt64(row["HeaderWarehouseId"]);
                        objWOL.WarehouseID = Convert.ToInt64(row["HeaderWarehouseId"]);
                        objWOL.Issue_Qty = Convert.ToDecimal(row["Issue_Qty"]);
                        objWOL.WorkCenterID = Convert.ToInt64(row["WorkCenterID"]);
                        objWOL.WorkCenterCode = Convert.ToString(row["WorkCenterCode"]);
                        objWOL.WorkCenterDescription = Convert.ToString(row["WorkCenterDescription"]);
                        objWOL.BalQty = Convert.ToDecimal(row["BalQty"]);
                        objWOL.PartNoName = Convert.ToString(row["PartNoName"]);
                        objWOL.DesignNo = Convert.ToString(row["DesignNo"]);
                        objWOL.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                        objWOL.Description = Convert.ToString(row["sProducts_Name"]);
                        objWOL.Proj_Code = Convert.ToString(row["Proj_Code"]);
                        objWOL.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);
                        // Rev Sanchita
                        objWOL.ProjectID = Convert.ToInt64(row["ProjectID"]);
                        // End of Re Sanchita
                        list.Add(objWOL);
                    }
                }

            }
            catch { }
            return PartialView("_ProductionIssueList", list);
        }

        public JsonResult GetPRDesignerList(Int64 ID = 0)
        {
            List<DesignList> list = new List<DesignList>();
            try
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\ManufacturingPR\DocDesign\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\ManufacturingPR\DocDesign\";
                }
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string DesignFullPath = fullpath + DesignPath;
                filePaths = Directory.GetFiles(DesignFullPath, "*.repx");
                DesignList obj = null;
                foreach (string filename in filePaths)
                {
                    obj = new DesignList();
                    string reportname = Path.GetFileNameWithoutExtension(filename);
                    string name = "";
                    if (reportname.Split('~').Length > 1)
                    {
                        name = reportname.Split('~')[0];
                    }
                    else
                    {
                        name = reportname;
                    }
                    string reportValue = reportname;
                    obj.name = name;
                    obj.reportValue = reportValue;
                    list.Add(obj);
                }
            }
            catch { }
            return Json(list);
        }
        [WebMethod]
        public JsonResult AddProduct(ReturnFGReceivedFinishItemDetails prod)
        {
            DataTable dt = (DataTable)TempData["ProductsDetails"];
            DataTable dt2 = new DataTable();
            int duplicateId = 0;
            if (dt == null)
            {
                DataTable dtable = new DataTable();

                dtable.Clear();
                dtable.Columns.Add("HIddenID", typeof(System.Guid));
                dtable.Columns.Add("SrlNO", typeof(System.String));
                dtable.Columns.Add("FinishItemName", typeof(System.String));
                dtable.Columns.Add("FinishItemDescription", typeof(System.String));
                dtable.Columns.Add("FinishDrawingNo", typeof(System.String));
                dtable.Columns.Add("FinishItemRevNo", typeof(System.String));
                dtable.Columns.Add("Qty", typeof(System.String));
                dtable.Columns.Add("FinishUOM", typeof(System.String));
                dtable.Columns.Add("FinishPrice", typeof(System.String));
                dtable.Columns.Add("FinishAmount", typeof(System.String));
                dtable.Columns.Add("FinishUpdateEdit", typeof(System.String));
                dtable.Columns.Add("FGReturn_Id", typeof(System.String));
                dtable.Columns.Add("FGReceivedID", typeof(System.String));
                dtable.Columns.Add("FinishUOMId", typeof(System.String));
                dtable.Columns.Add("FinishProductsID", typeof(System.String));
                dtable.Columns.Add("FinishWareHouseId", typeof(System.String));
                dtable.Columns.Add("OldFGQuantity", typeof(System.String));
                dtable.Columns.Add("MaxBalFGQuantity", typeof(System.String));
                dtable.Columns.Add("FGWareHouseName", typeof(System.String));
                dtable.Columns.Add("FinishInventoryType", typeof(System.String));
                object[] trow = { Guid.NewGuid(), 1,prod.FinishItemName,prod.FinishItemDescription,prod.FinishDrawingNo,prod.FinishItemRevNo,prod.Qty,prod.FinishUOM,prod.FinishPrice,prod.FinishAmount,prod.FinishUpdateEdit,
                                    prod.FGReturn_Id,prod.FGReceivedID,prod.FinishUOMId,prod.FinishProductsID,prod.FinishWareHouseId,prod.OldFGQuantity,prod.MaxBalFGQuantity,prod.FGWareHouseName,prod.FinishInventoryType };
                dtable.Rows.Add(trow);
                TempData["ProductsDetails"] = dtable;
                TempData.Keep();
            }
            else
            {
                if (string.IsNullOrEmpty(prod.Guids))
                {
                    foreach (DataRow dtw in dt.Rows)
                    {
                        string FProductId = Convert.ToString(dtw["FinishProductsID"]);

                        if (Convert.ToString(prod.FinishProductsID) != FProductId)
                        {
                            object[] trow = { Guid.NewGuid(), Convert.ToInt32(dt.Rows.Count)+1,prod.FinishItemName,prod.FinishItemDescription,prod.FinishDrawingNo,prod.FinishItemRevNo,prod.Qty,prod.FinishUOM,prod.FinishPrice,prod.FinishAmount,prod.FinishUpdateEdit,
                                    prod.FGReturn_Id,prod.FGReceivedID,prod.FinishUOMId,prod.FinishProductsID,prod.FinishWareHouseId,prod.OldFGQuantity,prod.MaxBalFGQuantity ,prod.FGWareHouseName,prod.FinishInventoryType };// Add new parameter Here
                            dt.Rows.Add(trow);
                            TempData["ProductsDetails"] = dt;
                            TempData.Keep();
                            break;
                        }
                        else
                        {
                            duplicateId = 1;
                            break;
                        }
                    }


                }
                else
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            if (prod.Guids.ToString() == item["HIddenID"].ToString())
                            {
                                // item["SlNO"] = prod.SlNO;
                                item["FinishItemName"] = prod.FinishItemName;
                                item["FinishItemDescription"] = prod.FinishItemDescription;
                                item["FinishDrawingNo"] = prod.FinishDrawingNo;
                                item["FinishItemRevNo"] = prod.FinishItemRevNo;
                                item["Qty"] = prod.Qty;
                                item["FinishUOM"] = prod.FinishUOM;
                                item["FinishPrice"] = prod.FinishPrice;
                                item["FinishAmount"] = prod.FinishAmount;
                                item["FinishUpdateEdit"] = "1";
                                item["FGReturn_Id"] = prod.FGReturn_Id;
                                item["FGReceivedID"] = prod.FGReceivedID;
                                item["FinishUOMId"] = prod.FinishUOMId;
                                item["FinishProductsID"] = prod.FinishProductsID;
                                item["FinishWareHouseId"] = prod.FinishWareHouseId;
                                item["OldFGQuantity"] = prod.OldFGQuantity;
                                item["MaxBalFGQuantity"] = prod.MaxBalFGQuantity;
                                item["FGWareHouseName"] = prod.FGWareHouseName;
                                item["FinishInventoryType"] = prod.FinishInventoryType;

                            }
                        }
                    }
                }
                TempData["ProductsDetails"] = dt;
                TempData.Keep();
            }
            decimal TotalFinmishQty = 0;
            DataTable ProductsDetails = new DataTable();
            ProductsDetails = (DataTable)TempData["ProductsDetails"];
            if (ProductsDetails != null && ProductsDetails.Rows.Count > 0)
            {

                foreach (DataRow drw in ProductsDetails.Rows)
                {
                    TotalFinmishQty = TotalFinmishQty + Convert.ToDecimal(drw["Qty"]);
                }
            }

            if (duplicateId == 1)
            {
                return Json("Duplicate~" + TotalFinmishQty);
            }
            else
            {
                return Json("Success~" + TotalFinmishQty);
            }
        }
        [WebMethod]
        public JsonResult EditData(String HiddenID)
        {
            ReturnFGReceivedFinishItemDetails ret = new ReturnFGReceivedFinishItemDetails();

            DataTable dt = (DataTable)TempData["ProductsDetails"];

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (HiddenID.ToString() == item["HIddenID"].ToString())
                    {
                        ret.Guids = item["HIddenID"].ToString();
                        ret.SrlNO = item["SrlNO"].ToString();
                        ret.FinishItemName = item["FinishItemName"].ToString();
                        ret.FinishItemDescription = item["FinishItemDescription"].ToString();
                        ret.FinishDrawingNo = item["FinishDrawingNo"].ToString();
                        ret.FinishItemRevNo = item["FinishItemRevNo"].ToString();
                        ret.Qty = item["Qty"].ToString();
                        ret.FinishUOM = item["FinishUOM"].ToString();
                        ret.FinishPrice = item["FinishPrice"].ToString();
                        ret.FinishAmount = item["FinishAmount"].ToString();
                        ret.FGReturn_Id = item["FGReturn_Id"].ToString();
                        ret.FGReceivedID = item["FGReceivedID"].ToString();
                        ret.FinishUOMId = item["FinishUOMId"].ToString();
                        ret.FinishProductsID = item["FinishProductsID"].ToString();
                        ret.FinishWareHouseId = item["FinishWareHouseId"].ToString();
                        ret.OldFGQuantity = item["OldFGQuantity"].ToString();
                        ret.MaxBalFGQuantity = item["MaxBalFGQuantity"].ToString();
                        ret.FGWareHouseName = item["FGWareHouseName"].ToString();
                        ret.FinishInventoryType = item["FinishInventoryType"].ToString();
                        break;
                    }
                }
            }
            TempData["ProductsDetails"] = dt;
            TempData.Keep();
            return Json(ret);
        }
        [WebMethod]
        public JsonResult DeleteData(string HiddenID)
        {
            string SRlId = "";
            string SRlQty = "0";
            DataTable dt = (DataTable)TempData["ProductsDetails"];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (HiddenID.ToString() == item["HIddenID"].ToString())
                    {
                        SRlId = Convert.ToString(item["SrlNO"]);
                        SRlQty = Convert.ToString(item["Qty"]);
                    }
                    if (HiddenID.ToString() == item["HIddenID"].ToString())
                    {
                        dt.Rows.Remove(item);
                        break;
                    }
                }
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                int conut = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["SrlNO"] = conut;
                    conut++;
                }
            }

            TempData["ProductsDetails"] = dt;
            TempData.Keep();
            return Json(SRlId + '~' + SRlQty);
        }
    }
}