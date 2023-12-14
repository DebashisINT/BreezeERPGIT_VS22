//==================================================== Revision History =========================================================================
// 1.0  Priti  V2.0.38    19-06-2023  0026367:In Production Order Qty:  1.A New field required in Production Order Module called 'BOMProductionQty'
// 2.0  Priti  V2.0.39    29-06-2023  0026384:Show valuation rate feature is required in Production Order module
// 3.0  Priti  V2.0.41    14-12-2023  0027086:System is allowing to edit tagged documents in Manufacturing module
//====================================================End Revision History=====================================================================

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
using UtilityLayer;
using System.Reflection;
using System.Web.Services;

namespace Manufacturing.Controllers
{
    public class ProductionOrderController : Controller
    {
        BOMEntryModel objdata = null;
        ProductionOrderViewModel objPO = null;
        DBEngine oDBEngine = new DBEngine();
        ProductionOrderModel objPM = null;
        string JVNumStr = string.Empty;
        string OrderNo = string.Empty;
        UserRightsForPage rights = new UserRightsForPage();
        CommonBL cSOrder = new CommonBL();
        public ProductionOrderController()
        {
            objdata = new BOMEntryModel();
            objPO = new ProductionOrderViewModel();
            objPM = new ProductionOrderModel();
        }

        //
        // GET: /ProductionOrder/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductionOrderEntry(Int64 DetailsID = 0)
        {
            //Rev 1.0
            string IsConsiderProductPackagingQtyInProductionOrder = cSOrder.GetSystemSettingsResult("IsConsiderProductPackagingQtyInProductionOrder");
            //Rev 1.0 End
            //Rev 2.0
            string IsRateEditableinProductionOrder = cSOrder.GetSystemSettingsResult("IsRateEditableinProductionOrder");
            //Rev 2.0 End

            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");        
            try
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
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

                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ProductionOrderEntry", "ProductionOrder");
                
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
                objPO.UnitList = list;

                if (TempData["ProductionOrderID"] != null)
                {
                    objPO.ProductionOrderID = Convert.ToInt64(TempData["ProductionOrderID"]);
                    TempData.Keep();

                    if (objPO.ProductionOrderID > 0)
                    {
                        DataTable objTagg = objPM.GetProductionOrderData("POTaggedOrNot", objPO.ProductionOrderID);
                        if (objTagg != null && objTagg.Rows.Count > 0)
                        {                            
                            foreach (DataRow item in objTagg.Rows)
                            {
                                //int Count= Convert.ToInt16(item["Exist"]);
                                int IsExist = Convert.ToInt16(item["Exist"]);
                                //if (Count>0)
                                if (IsExist > 0)  
                                {
                                    ViewBag.IsTagg = "Yes"; 
                                }
                                else
                                {
                                    ViewBag.IsTagg = "No"; 
                                }
                                
                            }
                        }
                        DataTable objData = objPM.GetProductionOrderData("GetProductionOrderData", objPO.ProductionOrderID);
                        if (objData != null && objData.Rows.Count > 0)
                        {
                            DataTable dt = objData;
                            foreach (DataRow row in dt.Rows)
                            {
                                objPO.ProductionOrderID = Convert.ToInt64(row["ProductionOrderID"]);
                                objPO.Production_ID = Convert.ToInt64(row["Production_ID"]);
                                objPO.Details_ID = Convert.ToInt64(row["Details_ID"]);
                                objPO.OrderNo = Convert.ToString(row["OrderNo"]);
                                objPO.Order_SchemaID = Convert.ToInt64(row["Order_SchemaID"]);
                                objPO.OrderDate = Convert.ToDateTime(row["OrderDate"]).ToString();
                                objPO.BRANCH_ID = Convert.ToInt64(row["BRANCH_ID"]);
                                objPO.WarehouseID = Convert.ToInt64(row["WarehouseID"]);
                                objPO.Order_Qty = Convert.ToDecimal(row["Order_Qty"]);
                                objPO.ActualAdditionalCost = Convert.ToDecimal(row["ActualAdditionalCost"]);
                                objPO.ActualComponentCost = Convert.ToDecimal(row["ActualComponentCost"]);
                                objPO.ActualProductCost = Convert.ToDecimal(row["ActualProductCost"]);
                                objPO.ProductionOrderQty = Convert.ToDecimal(row["Order_Qty"]);
                                objPO.FGReceiptQty = Convert.ToDecimal(row["FGReceiptQty"]);
                                objPO.TotalCost = Convert.ToDecimal(row["TotalCost"]);
                                objPO.BOMNo = Convert.ToString(row["BOM_No"]);
                                objPO.RevNo = Convert.ToString(row["REV_No"]);
                                objPO.FinishedItem = Convert.ToString(row["ProductName"]);
                                objPO.FinishedUom = Convert.ToString(row["FinishedUom"]);
                                objPO.Finished_Qty = Convert.ToDecimal(row["Finished_Qty"]);
                                objPO.strRemarks = Convert.ToString(row["Remarks"]);
                                objPO.TotalResourceCost = Convert.ToString(row["TotalResourceCost"]);
                                objPO.PartNo = Convert.ToString(row["PartNo"]);
                                objPO.PartNoName = Convert.ToString(row["PartNoName"]);
                                objPO.DesignNo = Convert.ToString(row["DesignNo"]);
                                objPO.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                                objPO.Description = Convert.ToString(row["sProducts_Name"]);
                                objPO.Proj_Code = Convert.ToString(row["Proj_Code"]);
                                objPO.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);
                            }
                        }
                    }
                }
                else
                {
                    TempData["DetailsID"] = null;
                    TempData.Clear();
                }

            }
            catch { }
            if (objPO.ProductionOrderID < 1)
            {
                objPO.OrderDate = DateTime.Now.ToString();
            }
           
            if (TempData["IsView"] != null)
            {
                ViewBag.IsView = Convert.ToInt16(TempData["IsView"]);
            }
            else
            {
                ViewBag.IsView = 0;
            }
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            //Rev 1.0
            ViewBag.IsConsiderProductPackagingQtyInProductionOrder = IsConsiderProductPackagingQtyInProductionOrder;
            //Rev 1.0 End

            //Rev 2.0
            ViewBag.IsRateEditableinProductionOrder = IsRateEditableinProductionOrder;
            //Rev 2.0 End
            

            TempData["Count"] = 1;
            TempData.Keep();
            return View("ProductionOrderEntry", objPO);

        }

        public ActionResult GetProductionBOMProductList(Int64 DetailsID = 0)
        {
            BOMProduct bomproductdataobj = new BOMProduct();
            List<BOMProduct> bomproductdata = new List<BOMProduct>();
            string IsConsiderProductPackagingQtyInProductionOrder = cSOrder.GetSystemSettingsResult("IsConsiderProductPackagingQtyInProductionOrder");

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
                    if (TempData["ProductionOrderID"] != null)
                    {
                        objData = objPM.GetProductionOrderData("GetBOMProductionOrderData", Convert.ToInt64(TempData["ProductionOrderID"]), DetailsID);
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

                            if (TempData["ProductionOrderID"] != null)
                            {
                                bomproductdataobj.OLDQty = Convert.ToString(row["OLDQty"]);
                            }
                            else
                            {
                                //Rev 1.0
                                if(IsConsiderProductPackagingQtyInProductionOrder=="Yes")
                                {
                                    bomproductdataobj.OLDQty = Convert.ToString(row["BOMProductionQty"]);
                                }
                                //Rev 1.0 End
                                else
                                {
                                    bomproductdataobj.OLDQty = Convert.ToString(row["StkQty"]);
                                }
                                
                            }
                            if (TempData["ProductionOrderID"] != null)
                            {
                                bomproductdataobj.OLDAmount = Convert.ToString(row["OLDAmount"]);
                            }
                            else
                            {
                                bomproductdataobj.OLDAmount = Convert.ToString(row["Amount"]);
                            }

                            bomproductdataobj.IsActive = Convert.ToBoolean(row["IsActive"]);
                            //Rev 1.0
                            bomproductdataobj.BOMProductionQty = Convert.ToString(row["BOMProductionQty"]);
                            bomproductdataobj.sProduct_packageqty = Convert.ToString(row["sProduct_packageqty"]);
                            //Rev 1.0 End
                            bomproductdata.Add(bomproductdataobj);
                        }

                        //bomproductdata = bomproductdata.Where(x => x.IsActive != true).ToList();
                        ViewData["BOMEntryProductsTotalAm"] = bomproductdata.Sum(x => Convert.ToDecimal(x.Amount)).ToString();
                    }
                }
            }
            catch { }
           
            return PartialView("_ProductionBOMProductGrid", bomproductdata);
        }

        public JsonResult SetTempDetailsID(Int64 DetailsID)
        {
            if (DetailsID > 0)
            {
                TempData["DetailsID"] = DetailsID;
                TempData.Keep();
            }
            else
            {
                TempData["DetailsID"] = null;
                TempData.Clear();
            }
            return Json(true);
        }

        public JsonResult getNumberingSchemeRecord()
        {
            List<SchemaNumber> list = new List<SchemaNumber>();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            DataTable Schemadt = objdata.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "92", "Y");
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

        [HttpPost]
        public JsonResult getWarehouseRecord(Int32 branchid = 0)
        {
            List<BranchWarehouse> list = new List<BranchWarehouse>();
            try
            {
                //var datasetobj = objdata.DropDownDetailForBOM("GetWarehouseDropDownData", null, null, null, branchid);
                DataTable dt = new DataTable();
                //dt = objPurchaseInvoice.PopulateWarehouseByBranchList(Convert.ToString(ddl_Branch.SelectedValue));
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

        public ActionResult GetBOMList()
        {
            List<BOMEntryViewModel> list = new List<BOMEntryViewModel>();
            try
            {
                BOMEntryViewModel objBom = new BOMEntryViewModel();
                // DataTable dt = lstuser.Getdesiglist();
                DataTable objData = objPM.GetProductionOrderData("GetBOMList");
                // modeldesig = APIHelperMethods.ToModelList<BOMEntryViewModel>(objData);

                if (objData != null && objData.Rows.Count > 0)
                {
                    DataTable dt = objData;


                    foreach (DataRow row in dt.Rows)
                    {
                        objBom = new BOMEntryViewModel();
                        objBom.DetailsID = Convert.ToString(row["Details_ID"]);
                        objBom.ProductionID = Convert.ToString(row["Production_ID"]);
                        objBom.BOM_SCHEMAID = Convert.ToString(row["BOM_SchemaID"]);
                        objBom.BOMNo = Convert.ToString(row["BOM_No"]);
                        objBom.BOMDate = Convert.ToString(row["BOM_Date"]);
                        objBom.BOMType = Convert.ToString(row["BOM_Type"]);
                        objBom.FinishedItem = Convert.ToString(row["Finished_ProductID"]);
                        objBom.FinishedQty = Convert.ToString(row["Finished_Qty"]);//7
                        objBom.RevisionNo = Convert.ToString(row["REV_No"]);
                        objBom.RevisionDate = Convert.ToString(row["REV_Date"]);
                        objBom.Unit = Convert.ToString(row["BRANCH_ID"]);
                        objBom.WarehouseID = Convert.ToString(row["WarehouseID"]);
                        objBom.ActualAdditionalCost = Convert.ToString(row["ActualAdditionalCost"]);
                        objBom.ActualComponentCost = Convert.ToString(row["ActualComponentCost"]);
                        objBom.ActualProductCost = Convert.ToString(row["ActualProductCost"]);
                        objBom.dtBOMDate = Convert.ToDateTime(row["BOM_Date"]);
                        if (Convert.ToString(row["REV_Date"]) == "")
                        {
                            objBom.dtREVDate = null;
                        }
                        else
                        {
                            objBom.dtREVDate = Convert.ToDateTime(row["REV_Date"]);
                        }                       
                        objBom.ProductionOrderQty = Convert.ToString(row["ProductionOrderQty"]);
                        objBom.FGReceiptQty = Convert.ToString(row["FGReceiptQty"]);
                        objBom.FinishedUom = Convert.ToString(row["UOM_Name"]);
                        objBom.FinishedItemName = Convert.ToString(row["ProductName"]);
                        objBom.IsActive = Convert.ToBoolean(row["IsActive"]);
                        objBom.TotalResourceCost1 = Convert.ToString(row["TotalResourceCost"]);//22
                        objBom.PartNo = Convert.ToString(row["PartNo"]);
                        objBom.PartNoName = Convert.ToString(row["PartNoName"]);
                        objBom.DesignNo = Convert.ToString(row["DesignNo"]);
                        objBom.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                        objBom.Description = Convert.ToString(row["sProducts_Name"]);

                        objBom.Proj_Code = Convert.ToString(row["Proj_Code"]);
                        objBom.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);
                        objBom.strRemarks = Convert.ToString(row["Remarks"]);
                        list.Add(objBom);
                    }

                    list = list.Where(x => x.IsActive != true).ToList();
                }

            }
            catch { }
            return PartialView("_BOMList", list);
        }

        [ValidateInput(false)]
        public ActionResult BatchEditingProductionBOMProduct(DevExpress.Web.Mvc.MVCxGridViewBatchUpdateValues<BOMProduct, int> updateValues, ProductionOrderViewModel options)
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
                    List<udtProductionOrderDetails> udtlist = new List<udtProductionOrderDetails>();
                    udtProductionOrderDetails obj = null;

                    foreach (var item in updateValues.Update)
                    {
                        if (Convert.ToInt64(item.BOMProductsID) > 0)
                        {
                            obj = new udtProductionOrderDetails();
                            obj.BOMProductsID = Convert.ToInt64(item.BOMProductsID);
                            obj.Qty = Convert.ToDecimal(item.ProductQty);
                            obj.Amount = Convert.ToDecimal(item.Amount);
                            //Rev 1.0
                            obj.BOMProductionQty = Convert.ToDecimal(item.BOMProductionQty);
                            obj.sProduct_packageqty = Convert.ToDecimal(item.sProduct_packageqty);
                            //Rev 1.0 End
                            //Rev 2.0
                            obj.Price = Convert.ToDecimal(item.Price);
                            //Rev 2.0 End
                            udtlist.Add(obj);
                        }
                    }
                    if (udtlist.Count > 0)
                    {
                        if (options.ProductionOrderID > 0)
                        {
                            IsProcess = ProductionBOMProductInsertUpdate(udtlist, options);
                        }
                        else
                        {
                            ////NumberScheme = checkNMakePOCode(options.OrderNo, Convert.ToInt32(options.Order_SchemaID), Convert.ToDateTime(options.OrderDate));
                            ////if (NumberScheme == "ok")
                            ////{
                                IsProcess = ProductionBOMProductInsertUpdate(udtlist, options);
                            //}
                            //else
                            //{
                            //    Message = NumberScheme;
                            //}
                        }

                        TempData["DetailsID"] = null;
                    }
                }


                TempData["Count"] = 1;
                TempData.Keep();
                //ViewData["ProductionID"] = ProductionID;
                //ViewData["DetailsID"] = DetailsID;
                ViewData["OrderNo"] = OrderNo;
                ViewData["Success"] = IsProcess;
                ViewData["Message"] = Message;
            }
            // return PartialView("~/Views/BOM/BOMEntry/_BOMProductEntryGrid.cshtml", updateValues.Update);
            return PartialView("_ProductionBOMProductGrid", updateValues.Update);
            //return Json(IsProcess, JsonRequestBehavior.AllowGet);
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
                        sqlQuery = "SELECT max(tjv.OrderNo) FROM ProductionOrder tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1 and OrderNo like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.OrderNo) FROM ProductionOrder tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1 and OrderNo like '%" + sufxCompCode + "'";
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
                            sqlQuery = "SELECT max(tjv.OrderNo) FROM ProductionOrder tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + paddCounter + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1 and OrderNo like '" + prefCompCode + "%'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }
                        else
                        {
                            int i = startNo.Length;
                            while (i < paddCounter)
                            {


                                sqlQuery = "SELECT max(tjv.OrderNo) FROM ProductionOrder tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + i + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1 and OrderNo like '" + prefCompCode + "%'";

                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.OrderNo)=" + i;
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
                                sqlQuery = "SELECT max(tjv.OrderNo) FROM ProductionOrder tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + (i - 1) + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1 and OrderNo like '" + prefCompCode + "%'";
                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.OrderNo)=" + (i - 1);
                                }
                                dtC = oDBEngine.GetDataTable(sqlQuery);
                            }

                        }

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.OrderNo) FROM ProductionOrder tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1 and OrderNo like '" + prefCompCode + "%'";
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
                                JVNumStr = startNo.PadLeft(paddCounter, '0');
                            else
                                paddedStr = startNo;

                            
                            JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                }
                else
                {
                    sqlQuery = "SELECT OrderNo FROM ProductionOrder WHERE OrderNo LIKE '" + manual_str.Trim() + "'";
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


        public Boolean ProductionBOMProductInsertUpdate(List<udtProductionOrderDetails> obj, ProductionOrderViewModel obj2)
        {
            Boolean Success = false;

            try
            {
                DataTable dtBOM_PRODUCTS = new DataTable();
                dtBOM_PRODUCTS = ToDataTable(obj);

                DataSet dt = new DataSet();
                if (Convert.ToInt64(obj2.Details_ID) > 0)
                {
                    //if (!String.IsNullOrEmpty(obj2.OrderNo) && obj2.OrderNo.ToLower() != "auto")
                    //{
                    //    JVNumStr = obj2.OrderNo;
                    //}

                    if (!String.IsNullOrEmpty(obj2.OrderNo))
                    {
                        JVNumStr = obj2.OrderNo;
                    }


                    dt = objPM.ProductionBOMProductInsertUpdate("INSERTPRODUCTIONBOM", obj2.ProductionOrderID, obj2.Production_ID, obj2.Details_ID, JVNumStr, obj2.Order_SchemaID, Convert.ToDateTime(obj2.OrderDate),
                        Convert.ToInt64(obj2.BRANCH_ID), Convert.ToInt64(obj2.WarehouseID), obj2.Order_Qty, obj2.ActualAdditionalCost, obj2.TotalCost, Convert.ToInt64(Session["userid"]),obj2.strRemarks,obj2.PartNo,obj2.TotalResourceCost,
                        dtBOM_PRODUCTS);
                }
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
                        OrderNo = Convert.ToString(row["OrderNo"]);
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

        public ActionResult ProductionOrderList()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ProductionOrderList", "ProductionOrder");
            ProductionOrderViewModel obj = new ProductionOrderViewModel();
            TempData.Clear();
            ViewBag.CanAdd = rights.CanAdd;
            return View("ProductionOrderList", obj);
        }

        public ActionResult GetProductionOrderList()
        {
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");

            
            List<ProductionOrderViewModel> list = new List<ProductionOrderViewModel>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ProductionOrderList", "ProductionOrder");
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
                        dt = oDBEngine.GetDataTable("select * from V_ProductionOrderList where BRANCH_ID =" + BranchID + " AND (OrderDate BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') ");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("select * from V_ProductionOrderList where OrderDate BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' ");
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
                    ProductionOrderViewModel obj = new ProductionOrderViewModel();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new ProductionOrderViewModel();
                        obj.ProductionOrderID = Convert.ToInt64(item["ProductionOrderID"]);
                        obj.Production_ID = Convert.ToInt64(item["Production_ID"]);
                        obj.Details_ID = Convert.ToInt64(item["Details_ID"]);
                        obj.OrderNo = Convert.ToString(item["OrderNo"]);
                        obj.Order_SchemaID = Convert.ToInt64(item["Order_SchemaID"]);
                        obj.dtOrderDate = Convert.ToDateTime(item["OrderDate"]);
                        obj.BRANCH_ID = Convert.ToInt64(item["BRANCH_ID"]);
                        obj.BOMNo = Convert.ToString(item["BOM_No"]);
                        obj.RevNo = Convert.ToString(item["REV_No"]);
                        obj.BOM_Date = Convert.ToDateTime(item["BOM_Date"]);
                        if (Convert.ToString(item["REV_Date"]) != "")
                        {
                            obj.REV_Date = Convert.ToDateTime(item["REV_Date"]);
                        }
                        else
                        {
                            obj.REV_Date = null;
                        }

                        obj.CreatedBy = Convert.ToString(item["CreatedBy"]);
                        obj.ModifyBy = Convert.ToString(item["ModifyBy"]);
                        obj.CreateDate = Convert.ToDateTime(item["CreateDate"]);

                        obj.Status = Convert.ToString(item["Status"]);
                        obj.ClosedRemarks = Convert.ToString(item["ClosedRemarks"]);

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
                        list.Add(obj);
                    }
                }
            }
            catch { }
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            return PartialView("_ProductionOrderDataList", list);
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
            DataTable dt = (DataTable)TempData["DetailsListDataTable"];
            if (ViewData["DetailsListDataTable"] != null && dt.Rows.Count > 0)
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
                return null;
            }
            else
            {
                return this.RedirectToAction("ProductionOrderList", "ProductionOrder");
            }
          
        }

        private GridViewSettings GetBOMGridView(object datatable)
        {
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");    
            var settings = new GridViewSettings();
            settings.Name = "Production Order (Demand Order)";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Production Order (Demand Order)";
            //String ID = Convert.ToString(TempData["EmployeesTargetListDataTable"]);
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "OrderNo" || datacolumn.ColumnName == "OrderDate"
                    || datacolumn.ColumnName == "BOM_No" || datacolumn.ColumnName == "BOM_Date" || datacolumn.ColumnName == "REV_No" || datacolumn.ColumnName == "REV_Date"
                    || datacolumn.ColumnName == "PartNoName" || datacolumn.ColumnName == "sProducts_Name" || datacolumn.ColumnName == "DesignNo" || datacolumn.ColumnName == "ItemRevisionNo"
                   || datacolumn.ColumnName == "CreateDate" || datacolumn.ColumnName == "CreatedBy" || datacolumn.ColumnName == "ModifyDate"
                   || datacolumn.ColumnName == "ModifyBy" || datacolumn.ColumnName == "Status" || datacolumn.ColumnName == "ClosedRemarks" || datacolumn.ColumnName == "Proj_Code" || datacolumn.ColumnName == "Proj_Name" 
                    )
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "OrderNo")
                        {
                            column.Caption = "Production Order No";
                            column.VisibleIndex = 0;
                        }
                        else if (datacolumn.ColumnName == "OrderDate")
                        {
                            column.Caption = "Order Date";
                            column.VisibleIndex = 1;
                        }
                        else if (datacolumn.ColumnName == "BOM_No")
                        {
                            column.Caption = "BOM No";
                            column.VisibleIndex = 2;

                        }
                        else if (datacolumn.ColumnName == "BOM_Date")
                        {
                            column.Caption = "BOM Date";
                            column.VisibleIndex = 3;
                        }
                        else if (datacolumn.ColumnName == "REV_No")
                        {
                            column.Caption = "Rev No.";
                            column.VisibleIndex = 4;
                        }
                        else if (datacolumn.ColumnName == "REV_Date")
                        {
                            column.Caption = "Rev Date";
                            column.VisibleIndex = 5;
                        }
                        else if (datacolumn.ColumnName == "PartNoName")
                        {
                            column.Caption = "Part No.";
                            column.VisibleIndex = 6;
                        }
                        else if (datacolumn.ColumnName == "sProducts_Name")
                        {
                            column.Caption = "Description";
                            column.VisibleIndex = 7;

                        }
                        else if (datacolumn.ColumnName == "DesignNo")
                        {
                            column.Caption = "Drawing No.";
                            column.VisibleIndex = 3;
                        }
                        else if (datacolumn.ColumnName == "ItemRevisionNo")
                        {
                            column.Caption = "Drawing Rev. No";
                            column.VisibleIndex = 8;
                        }
                        else if (datacolumn.ColumnName == "CreatedBy")
                        {
                            column.Caption = "Entered By";
                            column.VisibleIndex = 9;
                        }
                        else if (datacolumn.ColumnName == "CreateDate")
                        {
                            column.Caption = "Entered On";
                            column.VisibleIndex = 10;
                        }
                        else if (datacolumn.ColumnName == "ModifyBy")
                        {
                            column.Caption = "Modified By";
                            column.VisibleIndex = 11;
                        }
                        else if (datacolumn.ColumnName == "ModifyDate")
                        {
                            column.Caption = "Modified On";
                            column.VisibleIndex = 12;
                        }
                        else if (datacolumn.ColumnName == "Status")
                        {
                            column.Caption = "Status";
                            column.VisibleIndex = 13;

                        }
                        else if (datacolumn.ColumnName == "ClosedRemarks")
                        {
                            column.Caption = "Closed Remarks";
                            column.VisibleIndex = 14;
                        }
                        else if (ProjectSelectInEntryModule == "Yes")
                        {
                            if (datacolumn.ColumnName == "Proj_Code")
                            {
                                column.Caption = "Proj Code";
                                column.VisibleIndex = 15;
                            }
                            else if (datacolumn.ColumnName == "Proj_Name")
                            {
                                column.Caption = "Project Name";
                                column.VisibleIndex = 16;
                            }
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

        public JsonResult SetPODataByID(Int64 productionorderid = 0, Int16 IsView = 0)
        {
            Boolean Success = false;
            try
            {
                TempData["ProductionOrderID"] = productionorderid;
                TempData["IsView"] = IsView;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }

        public JsonResult RemovePODataByID(Int32 productionorderid)
        {
            ReturnData obj = new ReturnData();
            try
            {
                var datasetobj = objPM.GetProductionOrderData("RemovePOData", productionorderid,0);
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


        public JsonResult ClosedORNot(Int32 productionorderid, String ClosedPORemarks = "")
        {
            ReturnData obj = new ReturnData();
            try
            {
                var datasetobj = objPM.GetProductionOrderData("ClosedPOCheck", productionorderid, 0, ClosedPORemarks);
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
    

        public JsonResult ClosedPODataByID(Int32 productionorderid, String ClosedPORemarks = "")
        {
            ReturnData obj = new ReturnData();
            try
            {
                var datasetobj = objPM.GetProductionOrderData("ClosedPOData", productionorderid, 0, ClosedPORemarks);
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

        public JsonResult PopulateBranchByHierchy()
        {
            List<UnitList> list = new List<UnitList>();

            string userbranchHierachy = Convert.ToString(Session["userbranchHierarchy"]);

            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchHierachy);


            if (branchtable.Rows.Count > 0)
            {
                UnitList obj = new UnitList();
                foreach (DataRow item in branchtable.Rows)
                {
                    obj = new UnitList();
                    obj.ID = Convert.ToString(item["branch_id"]);
                    obj.Name = Convert.ToString(item["branch_description"]);
                    list.Add(obj);
                }
            }

            return Json(list);
        }

        //Rev 2.0
        [WebMethod]
        public JsonResult GetStockValuation(string ProductId)
        {
            StockValuationData obj = new StockValuationData();
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            DataTable dt = objCRMSalesOrderDtlBL.GetProductFifoValuation(Convert.ToInt32(ProductId));
            string Stock_Valuation = "";
            if (dt.Rows.Count > 0)
            {
                Stock_Valuation = Convert.ToString(dt.Rows[0]["Stockvaluation"]);
            }
            obj.StockValuation = Stock_Valuation;
            return Json(obj);
        }
        [WebMethod]
        public JsonResult GetStockValuationAmount(string Pro_Id, string Qty, string Valuationsign, string Fromdate, string BranchId)
        {
            ValuationAmountData obj = new ValuationAmountData();
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            DataTable dt = objCRMSalesOrderDtlBL.GetValueForProductFifoValuation(Convert.ToInt32(Pro_Id),
                                                Convert.ToDecimal(Qty), Valuationsign, Fromdate,
                                                Fromdate, BranchId);
            Decimal ValuationAmount =0;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ValuationAmount = Convert.ToDecimal(dt.Rows[0]["VALUE"]);
                }
            }

            obj.ValuationAmount = Math.Round(ValuationAmount,2);
            return Json(obj);
        }

        //Rev 2.0 End
    }
    //Rev 2.0
    public class StockValuationData
    {  
        public String StockValuation { get; set; }
    }
    public class ValuationAmountData
    {
        public Decimal ValuationAmount { get; set; }
    }
    //Rev 2.0 End

}