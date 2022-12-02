using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using Manufacturing.Models;
using Manufacturing.Models.ViewModel.BOMEntryModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using UtilityLayer;


namespace Manufacturing.Controllers
{
    public class BOMEntryController : Controller
    {
        // GET: BOMEntry
        BOMEntryViewModel objBom = null;
        BOMEntryModel objdata = null;
        DBEngine oDBEngine = new DBEngine();
        string JVNumStr = string.Empty;
        Int32 ProductionID = 0;
        Int32 DetailsID = 0;
        string BOMNO = string.Empty;

        //Int64 GlobalDetailsID = 0;
        UserRightsForPage rights = new UserRightsForPage();
        CommonBL cSOrder = new CommonBL();
        

        public BOMEntryController()
        {
            objBom = new BOMEntryViewModel();
            objdata = new BOMEntryModel();
        }
        public ActionResult Index(Int64 DetailsID = 0)
        {
            string hdnWSTAutoPrint = "0";
            
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/BOMEntryList", "BOMEntry");
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            string ProjectMandatoryInEntry = cSOrder.GetSystemSettingsResult("ProjectMandatoryInEntry");
            string BOMSalesInvoiceCumChallan = cSOrder.GetSystemSettingsResult("BOMSalesInvoiceCumChallan");

            //rev Pratik
            string MultiUOMSelectionForManufacturing = cSOrder.GetSystemSettingsResult("MultiUOMSelectionForManufacturing");
            //End of rev Pratik

            string BOMRevisionRequired = cSOrder.GetSystemSettingsResult("BOMRevisionRequired");

            try
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

                DBEngine odbeng = new DBEngine();
                DataTable WatermarkDt = odbeng.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='AutoPrintBOM'");
                if (Convert.ToString(WatermarkDt.Rows[0]["Variable_Value"]).ToUpper() == "NO")
                {
                    hdnWSTAutoPrint = "0";
                }
                else
                {
                    hdnWSTAutoPrint = "1";
                }
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
                string BOMRateRequiredEntryModule = cSOrder.GetSystemSettingsResult("BOMRateRequired");
                if (!String.IsNullOrEmpty(BOMRateRequiredEntryModule))
                {
                    if (BOMRateRequiredEntryModule.ToUpper().Trim() == "YES")
                    {
                        ViewBag.IsBOMRateRequired = "1";
                    }
                    else if (BOMRateRequiredEntryModule.ToUpper().Trim() == "NO")
                    {
                        ViewBag.IsBOMRateRequired = "0";
                    }
                }
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "BOMEntry");

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
                objBom.UnitList = list;
                List<HierarchyList> objHierarchy = new List<HierarchyList>();
                DataTable hierarchydt = oDBEngine.GetDataTable("select 0 as ID ,'Select' as H_Name union select ID,H_Name from V_HIERARCHY");
                if (hierarchydt != null && hierarchydt.Rows.Count > 0)
                {
                    HierarchyList obj = new HierarchyList();
                    foreach (DataRow item in hierarchydt.Rows)
                    {
                        obj = new HierarchyList();
                        obj.Hierarchy_id = Convert.ToString(item["ID"]);
                        obj.Hierarchy_Name = Convert.ToString(item["H_Name"]);
                        objHierarchy.Add(obj);
                    }
                }
                objBom.Hierarchy_List = objHierarchy;

                if (TempData["DetailsID"] != null)
                {
                    objBom.DetailsID = Convert.ToString(TempData["DetailsID"]);
                    TempData.Keep();

                    if (Convert.ToInt64(objBom.DetailsID) > 0)
                    {
                        DataTable objData = objdata.GetBOMProductEntryListByID("GetBOMEntryDetailsData", Convert.ToInt64(objBom.DetailsID));
                        if (objData != null && objData.Rows.Count > 0)
                        {
                            DataTable dt = objData;


                            foreach (DataRow row in dt.Rows)
                            {
                                objBom.DetailsID = Convert.ToString(row["Details_ID"]);
                                objBom.ProductionID = Convert.ToString(row["Production_ID"]);
                                objBom.BOM_SCHEMAID = Convert.ToString(row["BOM_SchemaID"]);
                                //Rev work start 29.07.2022 mantise no:0025098: Copy feature is required in Bill of Material Module
                                //objBom.BOMNo = Convert.ToString(row["BOM_No"]);
                                if (Convert.ToInt16(TempData["IsView"]) == 2)
                                {
                                    objBom.BOMNo = "";
                                }
                                else
                                {
                                    objBom.BOMNo = Convert.ToString(row["BOM_No"]);
                                }
                                //Rev work close 29.07.2022 mantise no:0025098: Copy feature is required in Bill of Material Module
                                objBom.BOMDate = Convert.ToString(row["BOM_Date"]);
                                objBom.BOMType = Convert.ToString(row["BOM_Type"]);
                                objBom.FinishedItem = Convert.ToString(row["Finished_ProductID"]);
                                objBom.FinishedQty = Convert.ToString(row["Finished_Qty"]);
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
                                    objBom.dtREVDate = DateTime.Now;
                                }
                                else
                                {
                                    objBom.dtREVDate = Convert.ToDateTime(row["REV_Date"]);
                                }
                                objBom.ProductionOrderQty = Convert.ToString(row["ProductionOrderQty"]);
                                objBom.FGReceiptQty = Convert.ToString(row["FGReceiptQty"]);
                                objBom.FinishedUom = Convert.ToString(row["UOM_Name"]);
                                objBom.FinishedItemName = Convert.ToString(row["ProductName"]);
                                objBom.strRemarks = Convert.ToString(row["Remarks"]);
                               // objBom.TotalResourceCost1 = Convert.ToString(row["TotalRecourceCost"]);
                                objBom.PartNo= Convert.ToString(row["PartNo"]);
                                objBom.PartNoName=Convert.ToString(row["PartNoName"]);
                                objBom.DesignNo = Convert.ToString(row["DesignNo"]);
                                objBom.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                                objBom.Description = Convert.ToString(row["sProducts_Name"]);
                                objBom.ProjectID = Convert.ToString(row["ProjectID"]);
                                objBom.Proj_Code = Convert.ToString(row["Proj_Code"]);
                                objBom.Hierarchy = Convert.ToString(row["Hierarchy_ID"]);
                                objBom.MPS_ID = Convert.ToString(row["MPS_ID"]);                               
                                objBom.MPSDate = Convert.ToString(row["MPS_Date"]);
                                ViewBag.ProjectID = Convert.ToString(row["ProjectID"]);
                                ViewBag.Unit = Convert.ToString(row["BRANCH_ID"]);
                                //Rev work start 05.08.2022 mantise no:0025098: Copy feature is required in Bill of Material Module
                                //ViewBag.MPS_ID = Convert.ToString(row["MPS_ID"]);
                                if (Convert.ToInt16(TempData["IsView"]) == 2)
                                {
                                   ViewBag.MPS_ID ="";
                                }
                                else
                                {
                                    ViewBag.MPS_ID = Convert.ToString(row["MPS_ID"]);
                                }
                                //Rev work close 05.08.2022 mantise no:0025098: Copy feature is required in Bill of Material Module
                            }
                        }
                    }
                }

            }
            catch { }
            objBom.RevisionDate = DateTime.Now.ToString();
            //objBom.BOMDate = DateTime.Now.ToString();

            TempData["Count"] = 1;
            TempData.Keep();
            //rev Pratik
            TempData["MultiUom"] = null;
            //TempData.Keep();
            //End of rev Pratik

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
            ViewBag.AutoPrint = hdnWSTAutoPrint;
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            ViewBag.ProjectMandatoryInEntry = ProjectMandatoryInEntry;
            ViewBag.BOMSalesInvoiceCumChallan = BOMSalesInvoiceCumChallan;

            //rev Pratik
            ViewBag.MultiUOMSelectionForManufacturing = MultiUOMSelectionForManufacturing;
            //End of rev Pratik

            ViewBag.BOMRevisionRequired = BOMRevisionRequired;

            return View("~/Views/BOM/BOMEntry/Index.cshtml", objBom);

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

        public JsonResult getNumberingSchemeRecord(String type = null)
        {
            List<SchemaNumber> list = new List<SchemaNumber>();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            String strType = "90";
            if (!String.IsNullOrEmpty(type))
            {
                if (type.ToLower() == "production")
                {
                    strType = "99";
                }
                if (type.ToLower() == "sales")
                {
                    strType = "94";
                }
                if (type.ToLower() == "assembly")
                {
                    strType = "95";
                }

            }
            DataTable Schemadt = objdata.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, strType, "Y");
            //if (Schemadt != null && Schemadt.Rows.Count > 0)
            //{
            //    ddl_numberingScheme.DataTextField = "SchemaName";
            //    ddl_numberingScheme.DataValueField = "Id";
            //    ddl_numberingScheme.DataSource = Schemadt;
            //    ddl_numberingScheme.DataBind();
            //}
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

        public ActionResult GetBOMResources()
        {
            List<POSSales> objList = new List<POSSales>();
            POSSales dataobj = new POSSales();
            Int64 DetailsID = 0;
            if (TempData["DetailsID"] != null)
            {
                DetailsID = Convert.ToInt64(TempData["DetailsID"]);
                TempData["DetailsID"] = null;

            }

            if (DetailsID > 0)
            {
                DataTable objData = objdata.GetBOMProductEntryListByID("GetBOMEntryResourcesData", DetailsID);
                if (objData != null && objData.Rows.Count > 0)
                {
                    DataTable dt = objData;
                    foreach (DataRow row in dt.Rows)
                    {
                        dataobj = new POSSales();
                        dataobj.SlNO = Convert.ToString(row["SlNO"]);
                        dataobj.ProductName = Convert.ToString(row["ProductName"]);
                        dataobj.ProductId = Convert.ToString(row["ProductID"]);
                        dataobj.ProductDescription = Convert.ToString(row["sProducts_Description"]);
                        dataobj.ProductQty = Convert.ToString(row["StkQty"]);
                        dataobj.ProductUOM = Convert.ToString(row["StkUOM"]);
                        dataobj.Warehouse = Convert.ToString(row["WarehouseName"]);
                        dataobj.Price = Convert.ToString(row["Price"]);
                        dataobj.Amount = Convert.ToString(row["Amount"]);
                        dataobj.Remarks = Convert.ToString(row["Remarks"]);
                        dataobj.ProductsWarehouseID = Convert.ToString(row["WarehouseID"]);
                        objList.Add(dataobj);

                    }

                    ViewData["BOMResourcesTotalAm"] = objList.Sum(x => Convert.ToDecimal(x.Amount)).ToString();
                }
            }
            return PartialView("~/Views/BOM/BOMEntry/_BOMResourcesGrid.cshtml", objList);
        }


        public ActionResult GetBOMProductEntryList()
        {
            BOMProduct bomproductdataobj = new BOMProduct();
            List<BOMProduct> bomproductdata = new List<BOMProduct>();
            Int64 DetailsID = 0;
            try
            {

                if (TempData["DetailsID"] != null)
                {
                    DetailsID = Convert.ToInt64(TempData["DetailsID"]);
                    TempData.Keep();
                }

                if (DetailsID > 0)
                {
                    //rev Pratik
                    //DataTable objData = objdata.GetBOMProductEntryListByID("GetBOMEntryProductsData", DetailsID);
                    DataTable objData = objdata.GetBOMProductEntryListByID("GETBOMENTRYPRODUCTSDATA_NEW", DetailsID);

                    TempData["MultiUom"] = objdata.GetBOMProductEntryListByID("BOMMultiUOMDetails", DetailsID);
                    
                    //End of rev Pratik
                    
                    if (objData != null && objData.Rows.Count > 0)
                    {
                        DataTable dt = objData;


                        foreach (DataRow row in dt.Rows)
                        {
                            bomproductdataobj = new BOMProduct();
                            bomproductdataobj.SlNO = Convert.ToString(row["SlNO"]);
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
                            if (!String.IsNullOrEmpty(Convert.ToString(row["RevDate"])))
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
                            //Rev Pratik
                            bomproductdataobj.AltQuantity = Convert.ToString(row["AltQuantity"]);
                            bomproductdataobj.AltUom = Convert.ToString(row["AltUom"]);
                            bomproductdataobj.MultiUOMSelectionForManufacturing = cSOrder.GetSystemSettingsResult("MultiUOMSelectionForManufacturing");
                            //End of rev Pratik
                            bomproductdata.Add(bomproductdataobj);

                        }
                        ViewData["BOMEntryProductsTotalAm"] = bomproductdata.Sum(x => Convert.ToDecimal(x.Amount)).ToString();
                    }
                }

            }
            catch { }
            TempData["MultiUOMSelectionForManufacturing"] = cSOrder.GetSystemSettingsResult("MultiUOMSelectionForManufacturing");
            TempData.Keep();
            //ViewData["Yes"] = "Yes";
            return PartialView("~/Views/BOM/BOMEntry/_BOMProductEntryGrid.cshtml", bomproductdata);
        }

        [ValidateInput(false)]
        public ActionResult BatchEditingUpdateBOMProductEntry(DevExpress.Web.Mvc.MVCxGridViewBatchUpdateValues<BOMProduct, int> updateValues, BOMEntryViewModel options)
        {
            TempData["Count"] = (int)TempData["Count"] + 1;
            TempData.Keep();
            String NumberScheme = "";
            String Message = "";
            Int64 SaveDataArea = 0;

            List<udtProducts> udt = new List<udtProducts>();

            if ((int)TempData["Count"] != 2)
            {
                Boolean IsProcess = false;
                List<BOMProduct> list = new List<BOMProduct>();
                //foreach (var product in updateValues.Insert)
                if (updateValues.Insert.Count > 0 && Convert.ToInt64(options.DetailsID) < 1)
                {
                    //if (updateValues.IsValid(product))
                    //{
                    List<udtEntryProducts> udtlist = new List<udtEntryProducts>();
                    udtEntryProducts obj = null;
                    //Rev work start 03.08.2022 mantise no:0025098 code retification
                    //updateValues.Insert = updateValues.Insert.OrderBy(x => x.SlNO).ToList();
                    //Rev work close 03.08.2022 mantise no:0025098 code retification
                    foreach (var item in updateValues.Insert)
                    {
                        if (Convert.ToInt64(item.ProductId) > 0)
                        {                           
                            if (String.IsNullOrEmpty(item.Tag_Production_ID))
                            {
                                item.Tag_Production_ID = "0";
                            }
                            if (String.IsNullOrEmpty(item.Tag_Details_ID))
                            {
                                item.Tag_Details_ID = "0";
                            }

                            obj = new udtEntryProducts();
                            obj.ProductID = Convert.ToInt64(item.ProductId);
                            obj.StkQty = Convert.ToDecimal(item.ProductQty);
                            obj.StkUOM = (item.ProductUOM);
                            obj.IssuesQty = Convert.ToDecimal(0);
                            obj.IssuesUOM = (" ");
                            obj.WarehouseID = Convert.ToInt64(item.ProductsWarehouseID);
                            obj.Price = Convert.ToDecimal(item.Price);
                            obj.Amount = Convert.ToDecimal(item.Amount);
                            obj.Tag_Details_ID = Convert.ToInt64(item.Tag_Details_ID);
                            obj.Tag_Production_ID = Convert.ToInt64(item.Tag_Production_ID);
                            obj.Tag_REV_No = item.RevNo;
                            obj.Remarks = (item.Remarks);

                            //Rev Pratik 
                            obj.AltQuantity = Convert.ToDecimal(item.AltQuantity);
                            obj.AltUom = (item.AltUom);
                            //End Rev Pratik
                            obj.SlNo = (item.SlNO);

                            udtlist.Add(obj);
                        }
                    }
                    if (udtlist.Count > 0)
                    {
                        SaveDataArea = 1;
                        //if (options.BOMNo)
                       // NumberScheme = checkNMakeBOMCode(options.strBOMNo, Convert.ToInt32(options.BOM_SCHEMAID), Convert.ToDateTime(options.RevisionDate));
                       // if (NumberScheme == "ok")
                       // {
                        //Rev work start 03.08.2022 mantise no:0025098 code retification
                            //udtlist = udtlist.OrderBy(x => x.SlNo).ToList();
                        //Rev work close 03.08.2022 mantise no:0025098 code retification
                            foreach (var item in udtlist)
                            {
                                udtProducts obj1 = new udtProducts();
                                obj1.ProductID = Convert.ToInt64(item.ProductID);
                                obj1.StkQty = Convert.ToDecimal(item.StkQty);
                                obj1.StkUOM = (item.StkUOM);
                                obj1.IssuesQty = (item.IssuesQty);
                                obj1.IssuesUOM = (" ");
                                obj1.WarehouseID = Convert.ToInt64(item.WarehouseID);
                                obj1.Price = Convert.ToDecimal(item.Price);
                                obj1.Amount = Convert.ToDecimal(item.Amount);
                                obj1.Tag_Details_ID = Convert.ToInt64(item.Tag_Details_ID);
                                obj1.Tag_Production_ID = Convert.ToInt64(item.Tag_Production_ID);
                                obj1.Tag_REV_No = item.Tag_REV_No;
                                obj1.Remarks = (item.Remarks);
                                //Rev Pratik 
                                obj1.AltQuantity = Convert.ToDecimal(item.AltQuantity);
                                obj1.AltUom = (item.AltUom);
                                //End Rev Pratik
                                //Rev work start 03.08.2022 mantise no:0025098 code retification
                                obj1.SlNo = Convert.ToInt32(item.SlNo);
                                //Rev work close 03.08.2022 mantise no:0025098 code retification
                                udt.Add(obj1);
                            }
                            IsProcess = BOMProductInsertUpdate(udt, options);
                        //}
                        //else
                        //{
                        //    Message = NumberScheme;
                        //}
                    }
                    // list.Add(product);
                    //}
                }
                if (((updateValues.Update.Count > 0 && Convert.ToInt64(options.DetailsID) > 0) || (updateValues.Insert.Count > 0 && Convert.ToInt64(options.DetailsID) < 1)) && SaveDataArea == 0)
                {
                    List<udtEntryProducts> udtlist = new List<udtEntryProducts>();
                    udtEntryProducts obj = null;                  
                    foreach (var item in updateValues.Update)
                    {
                        if (Convert.ToInt64(item.ProductId) > 0)
                        {
                            obj = new udtEntryProducts();
                            obj.ProductID = Convert.ToInt64(item.ProductId);
                            obj.StkQty = Convert.ToDecimal(item.ProductQty);
                            obj.StkUOM = (item.ProductUOM);
                            obj.IssuesQty = Convert.ToDecimal(0);
                            obj.IssuesUOM = (" ");
                            obj.WarehouseID = Convert.ToInt64(item.ProductsWarehouseID);
                            obj.Price = Convert.ToDecimal(item.Price);
                            obj.Amount = Convert.ToDecimal(item.Amount);
                            obj.Tag_Details_ID = Convert.ToInt64(item.Tag_Details_ID);
                            obj.Tag_Production_ID = Convert.ToInt64(item.Tag_Production_ID);
                            obj.Tag_REV_No = item.RevNo;
                            obj.Remarks = (item.Remarks);
                            obj.SlNo = (item.SlNO);
                            //Rev Pratik 
                            obj.AltQuantity = Convert.ToDecimal(item.AltQuantity);
                            obj.AltUom = (item.AltUom);
                            //End Rev Pratik

                            udtlist.Add(obj);
                        }
                    }

                    foreach (var item in updateValues.Insert)
                    {
                        if (Convert.ToInt64(item.ProductId) > 0)
                        {
                            obj = new udtEntryProducts();
                            obj.ProductID = Convert.ToInt64(item.ProductId);
                            obj.StkQty = Convert.ToDecimal(item.ProductQty);
                            obj.StkUOM = (item.ProductUOM);
                            obj.IssuesQty = Convert.ToDecimal(0);
                            obj.IssuesUOM = (" ");
                            obj.WarehouseID = Convert.ToInt64(item.ProductsWarehouseID);
                            obj.Price = Convert.ToDecimal(item.Price);
                            obj.Amount = Convert.ToDecimal(item.Amount);
                            obj.Tag_Details_ID = Convert.ToInt64(item.Tag_Details_ID);
                            obj.Tag_Production_ID = Convert.ToInt64(item.Tag_Production_ID);
                            obj.Tag_REV_No = item.RevNo;
                            obj.Remarks = (item.Remarks);
                            obj.SlNo = (item.SlNO);
                            //Rev Pratik 
                            obj.AltQuantity = Convert.ToDecimal(item.AltQuantity);
                            obj.AltUom = (item.AltUom);
                            //End Rev Pratik

                            udtlist.Add(obj);
                        }
                    }                  

                    if (udtlist.Count > 0)
                    {
                        SaveDataArea = 1;
                        //Rev work start 03.08.2022    mantise no:0025098 code retification
                        //udtlist = udtlist.OrderBy(x => x.SlNo).ToList();                       
                        //Rev work close 03.08.2022 mantise no:0025098 code retification
                        foreach (var item in udtlist)
                        {
                            udtProducts obj1 = new udtProducts();
                            obj1.ProductID = Convert.ToInt64(item.ProductID);
                            obj1.StkQty = Convert.ToDecimal(item.StkQty);
                            obj1.StkUOM = (item.StkUOM);
                            obj1.IssuesQty = (item.IssuesQty);
                            obj1.IssuesUOM = (" ");
                            obj1.WarehouseID = Convert.ToInt64(item.WarehouseID);
                            obj1.Price = Convert.ToDecimal(item.Price);
                            obj1.Amount = Convert.ToDecimal(item.Amount);
                            obj1.Tag_Details_ID = Convert.ToInt64(item.Tag_Details_ID);
                            obj1.Tag_Production_ID = Convert.ToInt64(item.Tag_Production_ID);
                            obj1.Tag_REV_No = item.Tag_REV_No;
                            obj1.Remarks = (item.Remarks);
                            //Rev Pratik 
                            obj1.AltQuantity = Convert.ToDecimal(item.AltQuantity);
                            obj1.AltUom = (item.AltUom);
                            //End Rev Pratik
                            //Rev work start 03.08.2022 mantise no:0025098 code retification
                                obj1.SlNo = Convert.ToInt32(item.SlNo);
                                //Rev work close 03.08.2022 mantise no:0025098 code retification
                            udt.Add(obj1);
                        }

                        IsProcess = BOMProductInsertUpdate(udt, options);
                    }
                }


                TempData["Count"] = 1;
                TempData.Keep();
                ViewData["ProductionID"] = ProductionID;
                ViewData["DetailsID"] = DetailsID;
                //ViewData["BOMNo"] = JVNumStr;
                ViewData["BOMNo"] = BOMNO;
                ViewData["Success"] = IsProcess;
                ViewData["Message"] = Message;
            }
            return PartialView("~/Views/BOM/BOMEntry/_BOMProductEntryGrid.cshtml", updateValues.Update);
            //return Json(IsProcess, JsonRequestBehavior.AllowGet);
        }

        public Boolean BOMProductInsertUpdate(List<udtProducts> obj, BOMEntryViewModel obj2)
        {
            Boolean Success = false;
            try
            {
                DataTable dtBOM_PRODUCTS = new DataTable();
                dtBOM_PRODUCTS = ToDataTable(obj);

                DataColumnCollection dtC = dtBOM_PRODUCTS.Columns;


                if (dtC.Contains("BOMProductsID"))
                {
                    dtBOM_PRODUCTS.Columns.Remove("BOMProductsID");
                }
               

                string FinYear = Convert.ToString(Session["LastFinYear"]);
                string strCompanyID = Convert.ToString(Session["LastCompany"]);

                //rev Pratik
                DataTable MultiUOMDetails = new DataTable();

                if (TempData["MultiUom"] != null)
                {
                    DataTable MultiUOM = (DataTable)TempData["MultiUom"];
                    // Mantis Issue 24428
                    // MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId");
                    MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId", "BaseRate", "AltRate", "UpdateRow");
                    // End of Mantis Issue 24428
                }
                else
                {
                    MultiUOMDetails.Columns.Add("SrlNo", typeof(string));
                    MultiUOMDetails.Columns.Add("Quantity", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UOM", typeof(string));
                    MultiUOMDetails.Columns.Add("AltUOM", typeof(string));
                    MultiUOMDetails.Columns.Add("AltQuantity", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UomId", typeof(Int64));
                    MultiUOMDetails.Columns.Add("AltUomId", typeof(Int64));
                    MultiUOMDetails.Columns.Add("ProductId", typeof(Int64));

                    // Mantis Issue 24428
                    MultiUOMDetails.Columns.Add("BaseRate", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("AltRate", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UpdateRow", typeof(string));
                    // End of Mantis Issue 24428

                }
                //End of rev Pratik
                DataSet dt = new DataSet();
                //Rev work start 29.07.2022 mantise no:0025098: Copy feature is required in Bill of Material Module
                //if (Convert.ToInt64(obj2.DetailsID) > 0)
                if (Convert.ToInt64(obj2.DetailsID) > 0 && Convert.ToInt16(TempData["IsView"])==0)
                {
                    //Rev work close 29.07.2022 mantise no:0025098: Copy feature is required in Bill of Material Module
                    if (!String.IsNullOrEmpty(obj2.strBOMNo))
                    {
                        JVNumStr = obj2.strBOMNo;                        
                    }
                    //rev Pratik
                    //dt = objdata.BOMProductEntryInsertUpdate("UPDATEMAINPRODUCT", JVNumStr, Convert.ToDateTime(obj2.BOMDate), Convert.ToInt64(obj2.FinishedItem), Convert.ToDecimal(obj2.FinishedQty), obj2.FinishedUom, obj2.BOMType, obj2.RevisionNo, Convert.ToDateTime(obj2.RevisionDate), Convert.ToInt32(obj2.Unit), Convert.ToInt32(obj2.WarehouseID)
                    //   , dtBOM_PRODUCTS, new DataTable(), Convert.ToInt32(obj2.BOM_SCHEMAID), Convert.ToDecimal(obj2.ActualAdditionalCost), Convert.ToInt64(Session["userid"]), 0, Convert.ToInt64(obj2.DetailsID), Convert.ToDecimal(obj2.TotalResourceCost1), obj2.strRemarks, obj2.PartNo, obj2.ProjectID, strCompanyID, FinYear, Convert.ToInt32(obj2.MPS_ID));
                    
                    dt = objdata.BOMProductEntryInsertUpdate("UPDATEMAINPRODUCT", JVNumStr, Convert.ToDateTime(obj2.BOMDate), Convert.ToInt64(obj2.FinishedItem), Convert.ToDecimal(obj2.FinishedQty), obj2.FinishedUom, obj2.BOMType, obj2.RevisionNo, Convert.ToDateTime(obj2.RevisionDate), Convert.ToInt32(obj2.Unit), Convert.ToInt32(obj2.WarehouseID)
                           , dtBOM_PRODUCTS, new DataTable(), Convert.ToInt32(obj2.BOM_SCHEMAID), Convert.ToDecimal(obj2.ActualAdditionalCost), Convert.ToInt64(Session["userid"]), 0, Convert.ToInt64(obj2.DetailsID), Convert.ToDecimal(obj2.TotalResourceCost1), obj2.strRemarks, obj2.PartNo, obj2.ProjectID, strCompanyID, FinYear, Convert.ToInt32(obj2.MPS_ID), MultiUOMDetails);
                    //End of rev Pratik
                }
                //Rev work start 29.07.2022 mantise no:0025098: Copy feature is required in Bill of Material Module
                else if(Convert.ToInt64(obj2.DetailsID) > 0 && Convert.ToInt16(TempData["IsView"])==2)
                {
                    JVNumStr = obj2.strBOMNo;
                   dt = objdata.BOMProductEntryInsertUpdate("INSERTMAINPRODUCT", JVNumStr, Convert.ToDateTime(obj2.BOMDate), Convert.ToInt64(obj2.FinishedItem), Convert.ToDecimal(obj2.FinishedQty), obj2.FinishedUom, obj2.BOMType, obj2.RevisionNo, Convert.ToDateTime(obj2.RevisionDate), Convert.ToInt32(obj2.Unit), Convert.ToInt32(obj2.WarehouseID)
                       , dtBOM_PRODUCTS, new DataTable(), Convert.ToInt32(obj2.BOM_SCHEMAID), Convert.ToDecimal(obj2.ActualAdditionalCost), Convert.ToInt64(Session["userid"]), 0, 0, Convert.ToDecimal(obj2.TotalResourceCost1), obj2.strRemarks, obj2.PartNo, obj2.ProjectID, strCompanyID, FinYear, Convert.ToInt32(obj2.MPS_ID), MultiUOMDetails);
                }
                //Rev work close 29.07.2022 mantise no:0025098: Copy feature is required in Bill of Material Module
                else
                {
                    JVNumStr = obj2.strBOMNo;       
                    //rev Pratik
                    //dt = objdata.BOMProductEntryInsertUpdate("INSERTMAINPRODUCT", JVNumStr, Convert.ToDateTime(obj2.BOMDate), Convert.ToInt64(obj2.FinishedItem), Convert.ToDecimal(obj2.FinishedQty), obj2.FinishedUom, obj2.BOMType, obj2.RevisionNo, Convert.ToDateTime(obj2.RevisionDate), Convert.ToInt32(obj2.Unit), Convert.ToInt32(obj2.WarehouseID)
                    //    , dtBOM_PRODUCTS, new DataTable(), Convert.ToInt32(obj2.BOM_SCHEMAID), Convert.ToDecimal(obj2.ActualAdditionalCost), Convert.ToInt64(Session["userid"]), 0, 0, Convert.ToDecimal(obj2.TotalResourceCost1), obj2.strRemarks, obj2.PartNo, obj2.ProjectID, strCompanyID, FinYear, Convert.ToInt32(obj2.MPS_ID));
                    
                    dt = objdata.BOMProductEntryInsertUpdate("INSERTMAINPRODUCT", JVNumStr, Convert.ToDateTime(obj2.BOMDate), Convert.ToInt64(obj2.FinishedItem), Convert.ToDecimal(obj2.FinishedQty), obj2.FinishedUom, obj2.BOMType, obj2.RevisionNo, Convert.ToDateTime(obj2.RevisionDate), Convert.ToInt32(obj2.Unit), Convert.ToInt32(obj2.WarehouseID)
                       , dtBOM_PRODUCTS, new DataTable(), Convert.ToInt32(obj2.BOM_SCHEMAID), Convert.ToDecimal(obj2.ActualAdditionalCost), Convert.ToInt64(Session["userid"]), 0, 0, Convert.ToDecimal(obj2.TotalResourceCost1), obj2.strRemarks, obj2.PartNo, obj2.ProjectID, strCompanyID, FinYear, Convert.ToInt32(obj2.MPS_ID), MultiUOMDetails);
                    //End of rev Pratik
                    //dt = objemployee.EmployeesTargetByCodeInsertUpdate(obj.EmployeeTargetSettingID, obj2.EmpTypeID, obj2.CounterType, obj.EmployeeCode, obj2.SettingMonth, obj2.SettingYear, obj.OrderValue, obj.NewCounter, obj.Collection, obj.Revisit);
                }
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        Success = Convert.ToBoolean(row["Success"]);
                        ProductionID = Convert.ToInt32(row["ProductionID"]);
                        DetailsID = Convert.ToInt32(row["DetailsID"]);
                        BOMNO = Convert.ToString(row["BOMNO"]);
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



        public string checkNMakeBOMCode(string manual_str, int sel_schema_Id, DateTime RevisionDate)
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
                        sqlQuery = "SELECT max(tjv.BOM_No) FROM BOM_Production tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.BOM_No))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.BOM_No))) = 1 and BOM_No like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.BOM_No) FROM BOM_Production tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.BOM_No))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.BOM_No))) = 1 and BOM_No like '%" + sufxCompCode + "'";
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
                            sqlQuery = "SELECT max(tjv.BOM_No) FROM BOM_Production tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + paddCounter + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.BOM_No))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.BOM_No))) = 1 and BOM_No like '" + prefCompCode + "%'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }
                        else
                        {
                            int i = startNo.Length;
                            while (i < paddCounter)
                            {


                                sqlQuery = "SELECT max(tjv.BOM_No) FROM BOM_Production tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + i + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.BOM_No))) = 1 and BOM_No like '" + prefCompCode + "%'";

                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.BOM_No)=" + i;
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
                                sqlQuery = "SELECT max(tjv.BOM_No) FROM BOM_Production tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + (i - 1) + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.BOM_No))) = 1 and BOM_No like '" + prefCompCode + "%'";
                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.BOM_No)=" + (i - 1);
                                }
                                dtC = oDBEngine.GetDataTable(sqlQuery);
                            }

                        }

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.BOM_No) FROM BOM_Production tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.BOM_No))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.BOM_No))) = 1 and BOM_No like '" + prefCompCode + "%'";
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
                    sqlQuery = "SELECT BOM_No FROM BOM_Production WHERE BOM_No LIKE '" + manual_str.Trim() + "'";
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

        [ValidateInput(false)]
        public ActionResult BatchEditingUpdateBOMResources(DevExpress.Web.Mvc.MVCxGridViewBatchUpdateValues<POSSales, int> updateValues, BOMEntryViewModel options)
        {
            Boolean IsProcess = false;
            List<POSSales> objList = new List<POSSales>();
            List<udtResources> udt = new List<udtResources>();
            try
            {                
                if (updateValues.Insert.Count > 0 && updateValues.Update.Count == 0)
                {                  
                    ProductionID = Convert.ToInt32(options.ProductionID);
                    DetailsID = Convert.ToInt32(options.DetailsID);

                    List<udtEntryResources> udtlist = new List<udtEntryResources>();
                    udtEntryResources obj = null;
                    updateValues.Insert = updateValues.Insert.OrderBy(x => x.SlNO).ToList();
                    foreach (var item in updateValues.Insert)
                    {
                        if (Convert.ToInt64(item.ProductId) > 0)
                        {
                            obj = new udtEntryResources();
                            obj.ProductID = Convert.ToInt64(item.ProductId);
                            obj.StkQty = Convert.ToDecimal(item.ProductQty);
                            obj.StkUOM = (item.ProductUOM == null ? "" : item.ProductUOM);
                            obj.WarehouseID = Convert.ToInt64(item.ProductsWarehouseID);
                            obj.Price = Convert.ToDecimal(item.Price);
                            obj.Amount = Convert.ToDecimal(item.Amount);
                            obj.Remarks = (item.Remarks);
                            obj.SlNo = (item.SlNO);

                            udtlist.Add(obj);
                        }
                    }
                    if (udtlist.Count > 0)
                    {
                        udtlist = udtlist.OrderBy(x => x.SlNo).ToList();

                        foreach (var item in udtlist)
                        {
                            udtResources obj1 = new udtResources();
                            obj1.ProductID = Convert.ToInt64(item.ProductID);
                            obj1.StkQty = Convert.ToDecimal(item.StkQty);
                            obj1.StkUOM = (item.StkUOM);
                            obj1.WarehouseID = Convert.ToInt64(item.WarehouseID);
                            obj1.Price = Convert.ToDecimal(item.Price);
                            obj1.Amount = Convert.ToDecimal(item.Amount);
                            obj1.Remarks = (item.Remarks);

                            udt.Add(obj1);
                        }

                        IsProcess = BOMResourcesInsertUpdate(udt, ProductionID, DetailsID);
                    }

                    //}
                }


                if (updateValues.Update.Count > 0 && Convert.ToInt64(options.DetailsID) > 0)
                {
                    ProductionID = Convert.ToInt32(options.ProductionID);
                    DetailsID = Convert.ToInt32(options.DetailsID);

                    List<udtEntryResources> udtlist = new List<udtEntryResources>();
                    udtEntryResources obj = null;
                    foreach (var item in updateValues.Insert)
                    {
                        if (Convert.ToInt64(item.ProductId) > 0)
                        {
                            obj = new udtEntryResources();
                            obj.ProductID = Convert.ToInt64(item.ProductId);
                            obj.StkQty = Convert.ToDecimal(item.ProductQty);
                            obj.StkUOM = (item.ProductUOM == null ? "" : item.ProductUOM);
                            obj.WarehouseID = Convert.ToInt64(item.ProductsWarehouseID);
                            obj.Price = Convert.ToDecimal(item.Price);
                            obj.Amount = Convert.ToDecimal(item.Amount);
                            obj.Remarks = (item.Remarks);
                            obj.SlNo = (item.SlNO);

                            udtlist.Add(obj);
                        }
                    }

                    //if (updateValues.Insert.Count > 0)
                    //{
                    //    foreach (var item in updateValues.Insert)
                    //    {
                    //        if (Convert.ToInt64(item.ProductId) > 0)
                    //        {
                    //            obj = new udtEntryResources();
                    //            obj.ProductID = Convert.ToInt64(item.ProductId);
                    //            obj.StkQty = Convert.ToDecimal(item.ProductQty);
                    //            obj.StkUOM = (item.ProductUOM == null ? "" : item.ProductUOM);
                    //            obj.WarehouseID = Convert.ToInt64(item.ProductsWarehouseID);
                    //            obj.Price = Convert.ToDecimal(item.Price);
                    //            obj.Amount = Convert.ToDecimal(item.Amount);
                    //            obj.Remarks = (item.Remarks);

                    //            udtlist.Add(obj);
                    //        }
                    //    }
                    //}


                    foreach (var item in updateValues.Update)
                    {
                        if (Convert.ToInt64(item.ProductId) > 0)
                        {
                            obj = new udtEntryResources();
                            obj.ProductID = Convert.ToInt64(item.ProductId);
                            obj.StkQty = Convert.ToDecimal(item.ProductQty);
                            obj.StkUOM = (item.ProductUOM == null ? "" : item.ProductUOM);
                            obj.WarehouseID = Convert.ToInt64(item.ProductsWarehouseID);
                            obj.Price = Convert.ToDecimal(item.Price);
                            obj.Amount = Convert.ToDecimal(item.Amount);
                            obj.Remarks = (item.Remarks);
                            obj.SlNo = (item.SlNO);

                            udtlist.Add(obj);
                        }
                    }

                    if (udtlist.Count > 0)
                    {
                        udtlist = udtlist.OrderBy(x => x.SlNo).ToList();

                        foreach (var item in udtlist)
                        {
                            udtResources obj1 = new udtResources();
                            obj1.ProductID = Convert.ToInt64(item.ProductID);
                            obj1.StkQty = Convert.ToDecimal(item.StkQty);
                            obj1.StkUOM = (item.StkUOM);
                            obj1.WarehouseID = Convert.ToInt64(item.WarehouseID);
                            obj1.Price = Convert.ToDecimal(item.Price);
                            obj1.Amount = Convert.ToDecimal(item.Amount);
                            obj1.Remarks = (item.Remarks);

                            udt.Add(obj1);
                        }

                        IsProcess = BOMResourcesInsertUpdate(udt, ProductionID, DetailsID);
                    }
                }
            }
            catch { }

            return PartialView("~/Views/BOM/BOMEntry/_BOMResourcesGrid.cshtml", objList);
        }


        public Boolean BOMResourcesInsertUpdate(List<udtResources> obj, Int32 ProductionID, Int32 DetailsID)
        {
            Boolean Success = false;
            try
            {
                DataSet dt = new DataSet();
                dt = objdata.BOMProductEntryInsertUpdate("INSERTRESOURCES", JVNumStr, DateTime.Now, 0, 0, "", "", "", DateTime.Now, 0, 0
                    , new DataTable(), ToDataTable(obj), 0, 0, Convert.ToInt64(Session["userid"]), ProductionID, DetailsID,0);
                //dt = objemployee.EmployeesTargetByCodeInsertUpdate(obj.EmployeeTargetSettingID, obj2.EmpTypeID, obj2.CounterType, obj.EmployeeCode, obj2.SettingMonth, obj2.SettingYear, obj.OrderValue, obj.NewCounter, obj.Collection, obj.Revisit);

                ProductionID = 0;
                DetailsID = 0;

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

        public ActionResult BOMEntryList()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/BOMEntryList", "BOMEntry");
            BOMEntryViewModel obj = new BOMEntryViewModel();
            TempData.Clear();
            ViewBag.CanAdd = rights.CanAdd;
            return View("~/Views/BOM/BOMEntry/BOMEntryList.cshtml", obj);
        }

        public ActionResult GetBOMEntryList()
        {
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            List<BOMEntryViewModel> list = new List<BOMEntryViewModel>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/BOMEntryList", "BOMEntry");
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
                        dt = oDBEngine.GetDataTable("select * from V_BOMDetailsList where BRANCH_ID =" + BranchID + " AND (BOM_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') ");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("select * from V_BOMDetailsList where BOM_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' ");
                    }
                }
                //else
                //{
                //    dt = oDBEngine.GetDataTable("select * from V_BOMDetailsList");
                //}

                TempData["BOMDetailsListDataTable"] = dt;
                if (dt.Rows.Count > 0)
                {
                    BOMEntryViewModel obj = new BOMEntryViewModel();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new BOMEntryViewModel();
                        obj.DetailsID = Convert.ToString(item["Details_ID"]);
                        obj.ProductionID = Convert.ToString(item["Production_ID"]);
                        obj.BOM_SCHEMAID = Convert.ToString(item["BOM_SchemaID"]);
                        obj.BOMNo = Convert.ToString(item["BOM_No"]);
                        obj.FinishedItem = Convert.ToString(item["FinishedProductName"]);
                        obj.BOMType = Convert.ToString(item["BOM_Type"]);
                        obj.dtBOMDate = Convert.ToDateTime(item["BOM_Date"]);
                        obj.RevisionNo = Convert.ToString(item["REV_No"]);
                        if (Convert.ToString(item["REV_Date"]) != "")
                        {
                            obj.RevisionDate = Convert.ToDateTime(item["REV_Date"]).ToString("dd-MM-yyyy");
                            obj.dtREVDate = Convert.ToDateTime(item["REV_Date"]);
                        }
                        else
                        {
                            obj.RevisionDate = "";
                            obj.dtREVDate = null;
                        }
                        obj.Unit = Convert.ToString(item["BranchDescription"]);
                        obj.Warehouse = Convert.ToString(item["WarehouseName"]);
                        obj.PartNoName = Convert.ToString(item["PartNoName"]);
                        obj.Description = Convert.ToString(item["sProducts_Name"]);
                        obj.DesignNo = Convert.ToString(item["DesignNo"]);
                        obj.ItemRevNo = Convert.ToString(item["ItemRevisionNo"]);
                        obj.Proj_Code = Convert.ToString(item["Proj_Code"]);
                        obj.Proj_Name = Convert.ToString(item["Proj_Name"]);
                        obj.MPS_No = Convert.ToString(item["MPS_No"]);
                        obj.MPSDate = Convert.ToString(item["MPS_Date"]);

                        obj.CreatedBy = Convert.ToString(item["CreatedBy"]);
                        obj.ModifyBy = Convert.ToString(item["ModifyBy"]);
                        obj.CreateDate = Convert.ToDateTime(item["CreateDate"]);
                        obj.Status = Convert.ToString(item["Status"]);
                        if (Convert.ToString(item["ModifyDate"]) != "")
                        {
                            obj.ModifyDate = Convert.ToDateTime(item["ModifyDate"]);
                        }
                        else
                        {
                            obj.ModifyDate = null;
                        }                     
                        //obj.ModifyDate = Convert.ToDateTime(item["ModifyDate"]);
                        list.Add(obj);
                    }
                }
            }
            catch { }
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanAddUpdateDocuments = rights.CanAddUpdateDocuments;
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            return PartialView("~/Views/BOM/BOMEntry/_BOMEntryDataList.cshtml", list);
        }

        public JsonResult RemoveBOMDataByID(Int32 detailsid)
        {
            ReturnData obj = new ReturnData();
            //Boolean Success = false;
            //String Message = String.Empty;
            try
            {
                var datasetobj = objdata.DropDownDetailForBOM("RemoveBOMData", null, null, null, 0, detailsid);
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

        public JsonResult SetBOMDataByID(Int64 detailsid = 0, Int16 IsView = 0)
        {
            Boolean Success = false;
            try
            {
                TempData["DetailsID"] = detailsid;
                TempData["IsView"] = IsView;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
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

        public ActionResult ExportBOMGridList(int type)
        {
            ViewData["BOMDetailsListDataTable"] = TempData["BOMDetailsListDataTable"];

            TempData.Keep();
            DataTable dt = (DataTable)TempData["BOMDetailsListDataTable"];
            if (ViewData["BOMDetailsListDataTable"] != null && dt.Rows.Count > 0)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetBOMGridView(ViewData["BOMDetailsListDataTable"]), ViewData["BOMDetailsListDataTable"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetBOMGridView(ViewData["BOMDetailsListDataTable"]), ViewData["BOMDetailsListDataTable"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetBOMGridView(ViewData["BOMDetailsListDataTable"]), ViewData["BOMDetailsListDataTable"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetBOMGridView(ViewData["BOMDetailsListDataTable"]), ViewData["BOMDetailsListDataTable"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetBOMGridView(ViewData["BOMDetailsListDataTable"]), ViewData["BOMDetailsListDataTable"]);
                    default:
                        break;
                }
                return null;
            }
            else
            {
                return this.RedirectToAction("BOMEntryList", "BOMEntry");
            }
        }

        private GridViewSettings GetBOMGridView(object datatable)
        {
            //List<EmployeesTargetSetting> obj = (List<EmployeesTargetSetting>)datatablelist;
            //ListtoDataTable lsttodt = new ListtoDataTable();
            //DataTable datatable = ConvertListToDataTable(obj); 
            var settings = new GridViewSettings();
            settings.Name = "Bill of Materials(BOM)";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Bill of Materials(BOM)";
            //String ID = Convert.ToString(TempData["EmployeesTargetListDataTable"]);
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "BOM_No" || datacolumn.ColumnName == "BOM_Type"
                    || datacolumn.ColumnName == "BOM_Date" || datacolumn.ColumnName == "REV_No" || datacolumn.ColumnName == "REV_Date" || datacolumn.ColumnName == "FinishedProductName"
                    || datacolumn.ColumnName == "BranchDescription" || datacolumn.ColumnName == "WarehouseName" || datacolumn.ColumnName == "Status" || datacolumn.ColumnName == "CreatedBy" || datacolumn.ColumnName == "CreateDate" || datacolumn.ColumnName == "ModifyBy" || datacolumn.ColumnName == "ModifyDate")
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "BOM_No")
                        {
                            column.Caption = "BOM No";
                            column.VisibleIndex = 0;
                        }
                        else if (datacolumn.ColumnName == "BOM_Type")
                        {
                            column.Caption = "BOM Type";
                            column.VisibleIndex = 1;
                        }
                        else if (datacolumn.ColumnName == "BOM_Date")
                        {
                            column.Caption = "BOM Date";
                            column.VisibleIndex = 2;

                        }
                        else if (datacolumn.ColumnName == "REV_No")
                        {
                            column.Caption = "Revision No";
                            column.VisibleIndex = 3;
                        }
                        else if (datacolumn.ColumnName == "REV_Date")
                        {
                            column.Caption = "Revision Date";
                            column.VisibleIndex = 4;
                        }
                        else if (datacolumn.ColumnName == "FinishedProductName")
                        {
                            column.Caption = "Finished Item";
                            column.VisibleIndex = 5;
                        }
                        else if (datacolumn.ColumnName == "BranchDescription")
                        {
                            column.Caption = "Unit";
                            column.VisibleIndex = 6;
                        }
                        else if (datacolumn.ColumnName == "WarehouseName")
                        {
                            column.Caption = "Warehouse";
                            column.VisibleIndex = 7;
                        }
                        else if (datacolumn.ColumnName == "CreatedBy")
                        {
                            column.Caption = "Entered By";
                            column.VisibleIndex = 8;
                        }
                        else if (datacolumn.ColumnName == "CreateDate")
                        {
                            column.Caption = "Entered On";
                            column.VisibleIndex = 9;
                        }
                        else if (datacolumn.ColumnName == "ModifyBy")
                        {
                            column.Caption = "Modified By";
                            column.VisibleIndex = 10;
                        }
                        else if (datacolumn.ColumnName == "ModifyDate")
                        {
                            column.Caption = "Modified On";
                            column.VisibleIndex = 11;
                        }
                        else if (datacolumn.ColumnName == "Status")
                        {
                            column.Caption = "Status";
                            column.VisibleIndex = 12;
                        }
                        else if (datacolumn.ColumnName == "Status")
                        {
                            column.Caption = "Status";
                            column.VisibleIndex = 12;
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


        public JsonResult ProcessWithRevisionNumber(Int32 detailsid = 0, String revisionno = null)
        {
            Boolean Success = false;
            try
            {
                var datasetobj = objdata.DropDownDetailForBOM("RevisionNumberCheck", null, revisionno, null, 0, detailsid);
                if (datasetobj.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow item in datasetobj.Tables[0].Rows)
                    {
                        Success = Convert.ToBoolean(item["Success"]);
                    }
                }
                //Success = true;
            }
            catch { }
            return Json(Success);
        }

        public JsonResult GetBOMDesignerList(Int64 ID = 0)
        {
            List<DesignList> list = new List<DesignList>();
            try
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\ManufacturingBOM\DocDesign\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\ManufacturingBOM\DocDesign\";
                }
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string DesignFullPath = fullpath + DesignPath;
                filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");
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
                    //obj.Add(name, reportValue);
                    obj.name = name;
                    obj.reportValue = reportValue;
                    list.Add(obj);
                }
            }
            catch { }
            return Json(list);
        }


        public JsonResult ClosedBOMDataByID(Int32 detailsid, String ClosedBOMRemarks = "")
        {
            ReturnData obj = new ReturnData();
            try
            {
                var datasetobj = objdata.DropDownDetailForBOM("ClosedBOMData", null, null, null, 0, detailsid, ClosedBOMRemarks);
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


        public ActionResult GetProjectCode(BOMEntryViewModel model, string Project_ID, String Branchs, String Hierarchy, string BOM_ID)
        {
            try
            {
                String Branch = "";
                if (model.Unit != null)
                {
                    Branch = model.Unit;
                }
                else
                {
                    Branch = Branchs;
                }

                DataTable dtProj = new DataTable();
                if (BOM_ID != null && BOM_ID != "")
                {
                    dtProj = objdata.GetProjectCodeFromBOM(Branch, BOM_ID);
                }
                else
                {
                    dtProj = objdata.GetProjectCode(Branch);
                }
                List<ProjectList> modelProj = new List<ProjectList>();
                modelProj = APIHelperMethods.ToModelList<ProjectList>(dtProj);
                ViewBag.ProjectID = Project_ID;
                ViewBag.Hierarchy = Hierarchy;

                return PartialView("~/Views/BOM/BOMEntry/_PartialProjectCode.cshtml", modelProj);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public ActionResult GetMPSNO(BOMEntryViewModel model, string MPS_ID, String Branchs)
        {
            try
            {
                String Branch = "";
                if (model.Unit != null)
                {
                    Branch = model.Unit;
                }
                else
                {
                    Branch = Branchs;
                }

                DataTable dtMPS = new DataTable();
                
                dtMPS = objdata.GetMPSNO(Branch);

                List<MPSNOList> modelMPS = new List<MPSNOList>();
                modelMPS = APIHelperMethods.ToModelList<MPSNOList>(dtMPS);
                ViewBag.MPS_ID = MPS_ID;


                return PartialView("~/Views/BOM/BOMEntry/_PartialMPSNO.cshtml", modelMPS);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        [WebMethod]
        public JsonResult getHierarchyID(string ProjID)
        {
            ReturnData obj = new ReturnData();
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string Hierarchy_ID = "";
            DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Code='" + ProjID + "'");
            if (dt2.Rows.Count > 0)
            {
                Hierarchy_ID = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                obj.Success = true;
                obj.Message = Hierarchy_ID;
                // return Hierarchy_ID;
            }
            else
            {
                Hierarchy_ID = "0";
                //return Hierarchy_ID;
                obj.Success = true;
                obj.Message = Hierarchy_ID;
            }
            return Json(obj);
        }

        [HttpPost]
        public String getStandardCostData(string Details_ID)
        {
            string Variable_Val = "0";
            BOMEntryModel objdata = new BOMEntryModel(); 
            //DataTable datasetobj;
            var datasetobj = objdata.GetBOMStandardCost("GetStandardCost", Convert.ToInt64(Details_ID));
            if (datasetobj.Rows.Count > 0)
            {
                DataTable dt = datasetobj;
                foreach (DataRow row in dt.Rows)
                {
                    Variable_Val = Convert.ToString(row["StandardCost"]);
                }
            }

            return Variable_Val;
        }
        [HttpPost]
        public String getGetActualCostData(string Details_ID)
        {
            string Variable_Val = "0";
            BOMEntryModel objdata = new BOMEntryModel();
            //DataTable datasetobj;
            var datasetobj = objdata.GetBOMStandardCost("GetActualCost", Convert.ToInt64(Details_ID));
            if (datasetobj.Rows.Count > 0)
            {
                DataTable dt = datasetobj;
                foreach (DataRow row in dt.Rows)
                {
                    Variable_Val = Convert.ToString(row["StandardCost"]);
                }
            }

            return Variable_Val;
        }
        //rev Pratik
        public JsonResult GetUOMDDL()
        {
            List<UOMBOM> UOM_list = new List<UOMBOM>();
            DataTable dt_uom = oDBEngine.GetDataTable("select UOM_ID,UOM_Name from Master_UOM");
            UOM_list = (from DataRow dr in dt_uom.Rows
                        select new UOMBOM()
                           {
                               UOM_ID = Convert.ToInt32(dr["UOM_ID"]),
                               UOM_Name = dr["UOM_Name"].ToString()
                           }).ToList();
            return Json(UOM_list, JsonRequestBehavior.AllowGet);
        }
        public class UOMBOM
        {
            public int UOM_ID { get; set; }
            public string UOM_Name { get; set; }

        }

        public JsonResult GetPackingQuantity(Int32 UomId, Int32 AltUomId, Int64 ProductID)
        {
            List<MultiUOMPacking> RateLists = new List<MultiUOMPacking>();

            decimal packing_quantity = 0;
            decimal sProduct_quantity = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "PackingQuantityDetails");
            proc.AddIntegerPara("@UomId", UomId);
            proc.AddIntegerPara("@AltUomId", AltUomId);
            proc.AddBigIntegerPara("@PackingProductId", ProductID);
            DataTable dt = proc.GetTable();
            RateLists = DbHelpers.ToModelList<MultiUOMPacking>(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                packing_quantity = Convert.ToDecimal(dt.Rows[0]["packing_quantity"]);
                sProduct_quantity = Convert.ToDecimal(dt.Rows[0]["sProduct_quantity"]);
            }
            //return packing_quantity + '~' + sProduct_quantity;
            //return RateLists;
            return Json(RateLists, JsonRequestBehavior.AllowGet);
        }

        public class MultiUOMPacking
        {
            public decimal packing_quantity { get; set; }
            public decimal sProduct_quantity { get; set; }

            public Int32 AltUOMId { get; set; }
            //rev Pratik
            public Int32 UOMId { get; set; }
            //End of rev Pratik
        }

        public JsonResult AutoPopulateAltQuantity(Int64 ProductID)
        {
            List<MultiUOMPacking> RateLists = new List<MultiUOMPacking>();

            decimal packing_quantity = 0;
            decimal sProduct_quantity = 0;
            Int32 AltUOMId = 0;
            Int32 UOMId = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "AutoPopulateAltQuantityDetails");
            proc.AddBigIntegerPara("@PackingProductId", ProductID);
            DataTable dt = proc.GetTable();
            RateLists = DbHelpers.ToModelList<MultiUOMPacking>(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                packing_quantity = Convert.ToDecimal(dt.Rows[0]["packing_quantity"]);
                sProduct_quantity = Convert.ToDecimal(dt.Rows[0]["sProduct_quantity"]);
                AltUOMId = Convert.ToInt32(dt.Rows[0]["AltUOMId"]);
                UOMId = Convert.ToInt32(dt.Rows[0]["UOMId"]);
            }
            //return packing_quantity + '~' + sProduct_quantity;
           // return RateLists;
            return Json(RateLists, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MultiUomList()
        {
            //try
            //{
            //String weburl = System.Configuration.ConfigurationSettings.AppSettings["SiteURL"];
            List<MultiUomModel> omel = new List<MultiUomModel>();

            DataTable dt = new DataTable();

            //if (model.Fromdate == null)
            //{
            //    model.Fromdate = DateTime.Now.ToString("dd-MM-yyyy");
            //}

            //if (model.Todate == null)
            //{
            //    model.Todate = DateTime.Now.ToString("dd-MM-yyyy");
            //}

            //ViewData["ModelData"] = model;

            //string datfrmat = model.Fromdate.Split('-')[2] + '-' + model.Fromdate.Split('-')[1] + '-' + model.Fromdate.Split('-')[0];
            //string dattoat = model.Todate.Split('-')[2] + '-' + model.Todate.Split('-')[1] + '-' + model.Todate.Split('-')[0];

            //double days = (Convert.ToDateTime(dattoat) - Convert.ToDateTime(datfrmat)).TotalDays;
            //if (days <= 30)
            //{
            //    dt = objgps.GetGpsStatusShop(datfrmat, dattoat, model.selectedusrid, "Summary");
            //}


            //if (dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        omel.Add(new MultiUomModel()
            //        {

            //            name = Convert.ToString(dt.Rows[i]["name"]),
            //            total_shop_visited = Convert.ToString(dt.Rows[i]["total_shop_visited"]),
            //            active_hrs = Convert.ToString(dt.Rows[i]["active_hrs"]),
            //            user_id = Convert.ToString(dt.Rows[i]["user_id"]),
            //            inactive_hrs = Convert.ToString(dt.Rows[i]["inactive_hrs"]),
            //            idle_percentage = Convert.ToString(dt.Rows[i]["idle_percentage"])

            //        });
            //    }

                //  omel = APIHelperMethods.ToModelList<GpsStatusClasstOutput>(dt);
                TempData["MultiUom"] = omel;
                TempData.Keep();

                return PartialView("_PartialGridMultiUom", omel);
            //}
            //catch
            //{
            //    return RedirectToAction("Logout", "Login", new { Area = "" });

            //}
        }
        public class MultiUomModel
        {
            public string SrlNo { get; set; }
            public string Quantity { get; set; }
            public string UOM { get; set; }
            public string BaseRate { get; set; }
            public string AltUOM { get; set; }
            public string AltQuantity { get; set; }
            public string UomId { get; set; }
            public string AltUomId { get; set; }
            public string AltRate { get; set; }
            public string UpdateRow { get; set; }
            public string ProductId { get; set; }

            public string MultiUOMSR { get; set; }

        }
        [HttpPost]
        public PartialViewResult MultiUomGrid(string Type, string srlNo, string qnty, string UomName, string AltUomName, string AltQnty, string UomId, string AltUomId, string ProductID, string BaseRate, string AltRate, string UpdateRow)
        {
            //try
            //{
            //String weburl = System.Configuration.ConfigurationSettings.AppSettings["SiteURL"];
            List<MultiUomModel> omel = new List<MultiUomModel>();

            DataTable dt = new DataTable();

            //if (model.Fromdate == null)
            //{
            //    model.Fromdate = DateTime.Now.ToString("dd-MM-yyyy");
            //}

            //if (model.Todate == null)
            //{
            //    model.Todate = DateTime.Now.ToString("dd-MM-yyyy");
            //}

            //ViewData["ModelData"] = model;

            //string datfrmat = model.Fromdate.Split('-')[2] + '-' + model.Fromdate.Split('-')[1] + '-' + model.Fromdate.Split('-')[0];
            //string dattoat = model.Todate.Split('-')[2] + '-' + model.Todate.Split('-')[1] + '-' + model.Todate.Split('-')[0];

            //double days = (Convert.ToDateTime(dattoat) - Convert.ToDateTime(datfrmat)).TotalDays;
            //if (days <= 30)
            //{
            //    dt = objgps.GetGpsStatusShop(datfrmat, dattoat, model.selectedusrid, "Summary");
            //}


            omel.Add(new MultiUomModel()
            {

                SrlNo = Convert.ToString(srlNo),
                Quantity = Convert.ToString(qnty),
                UOM = Convert.ToString(UomName),
                AltUOM = Convert.ToString(AltUomName),
                AltQuantity = Convert.ToString(AltQnty),
                UomId = Convert.ToString(UomId),
                AltUomId = Convert.ToString(AltUomId),
                //ProductId = Convert.ToString(e.Parameters.Split('~')[8]),
                BaseRate = Convert.ToString(BaseRate),
                AltRate = Convert.ToString(AltRate),
                UpdateRow = Convert.ToString(UpdateRow)

            });

              //omel = APIHelperMethods.ToModelList<GpsStatusClasstOutput>(dt);
            TempData["MultiUom"] = omel;
            TempData.Keep();

            return PartialView("_PartialGridMultiUom", omel);
            //}
            //catch
            //{
            //    return RedirectToAction("Logout", "Login", new { Area = "" });

            //}
        }

        [HttpPost]
        public JsonResult MultiUomGridTemp(string Type, string srlNo, string qnty, string UomName, string AltUomName, string AltQnty, string UomId, string AltUomId, string ProductID, string BaseRate, string AltRate, string UpdateRow, string MultiUOMSR_id)
        {
            //try
            //{
            //String weburl = System.Configuration.ConfigurationSettings.AppSettings["SiteURL"];
            List<MultiUomModel> omel = new List<MultiUomModel>();

            //DataTable dt = new DataTable();

            if (Type == "SaveDisplay")
            {

                string Validcheck = "";
                DataTable MultiUOMSaveData = new DataTable();

                int MultiUOMSR = 1;
                //string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                //string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                //string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                //string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                //string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                //string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                //string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                //string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);

                //// Mantis Issue 24428
                //string BaseRate = Convert.ToString(e.Parameters.Split('~')[9]);
                //string AltRate = Convert.ToString(e.Parameters.Split('~')[10]);
                //string UpdateRow = Convert.ToString(e.Parameters.Split('~')[11]);

                // End of Mantis Issue 24428

                DataTable allMultidataDetails = (DataTable)TempData["MultiUom"];



                if (allMultidataDetails != null && allMultidataDetails.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = allMultidataDetails.Select("SrlNo ='" + srlNo + "'");

                    foreach (DataRow item in MultiUoMresult)
                    {
                        if ((AltUomId == item["AltUomId"].ToString()) || (UomId == item["AltUomId"].ToString()))
                        {
                            if (AltQnty == item["AltQuantity"].ToString())
                            {
                                Validcheck = "DuplicateUOM";
                                //grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "DuplicateAltUOM";
                                return Json("Duplicate Alt UOM", JsonRequestBehavior.AllowGet);
                                //break;
                            }
                        }
                        // Mantis Issue 24428  [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                        if (UpdateRow == "True")
                        {
                            item["UpdateRow"] = "False";
                        }
                        // End of Mantis Issue 24428 
                    }
                }

                if (Validcheck != "DuplicateUOM")
                {
                    if (TempData["MultiUom"] != null)
                    {

                        MultiUOMSaveData = (DataTable)TempData["MultiUom"];
                        TempData.Keep();
                        //if (MultiUOMSaveData.Rows.Count > 0)
                        //{
                        //    for (int i = 0; i < MultiUOMSaveData.Rows.Count; i++)
                        //    {
                        //        omel.Add(new MultiUomModel()
                        //        {

                        //            SrlNo = Convert.ToString(MultiUOMSaveData.Rows[i]["SrlNo"]),
                        //            Quantity = Convert.ToString(MultiUOMSaveData.Rows[i]["Quantity"]),
                        //            UOM = Convert.ToString(MultiUOMSaveData.Rows[i]["UOM"]),
                        //            BaseRate = Convert.ToString(MultiUOMSaveData.Rows[i]["BaseRate"]),
                        //            AltUOM = Convert.ToString(MultiUOMSaveData.Rows[i]["AltUOM"]),
                        //            AltQuantity = Convert.ToString(MultiUOMSaveData.Rows[i]["AltQuantity"]),
                        //            UomId = Convert.ToString(MultiUOMSaveData.Rows[i]["UomId"]),
                        //            AltUomId = Convert.ToString(MultiUOMSaveData.Rows[i]["AltUomId"]),
                        //            AltRate = Convert.ToString(MultiUOMSaveData.Rows[i]["AltRate"]),
                        //            UpdateRow = Convert.ToString(MultiUOMSaveData.Rows[i]["UpdateRow"]),
                        //            ProductId = Convert.ToString(MultiUOMSaveData.Rows[i]["ProductId"])

                        //        });
                        //    }
                        //}

                    }
                    else
                    {
                         
                        MultiUOMSaveData.Columns.Add("SrlNo", typeof(string));
                        MultiUOMSaveData.Columns.Add("Quantity", typeof(string));
                        MultiUOMSaveData.Columns.Add("UOM", typeof(string));
                        MultiUOMSaveData.Columns.Add("AltUOM", typeof(string));
                        MultiUOMSaveData.Columns.Add("AltQuantity", typeof(string));
                        MultiUOMSaveData.Columns.Add("UomId", typeof(string));
                        MultiUOMSaveData.Columns.Add("AltUomId", typeof(string));
                        MultiUOMSaveData.Columns.Add("ProductId", typeof(string));

                        // Mantis Issue 24428
                        MultiUOMSaveData.Columns.Add("BaseRate", typeof(string));
                        MultiUOMSaveData.Columns.Add("AltRate", typeof(string));
                        MultiUOMSaveData.Columns.Add("UpdateRow", typeof(string));
                        MultiUOMSaveData.Columns.Add("MultiUOMSR", typeof(string));

                        //DataColumn myDataColumn = new DataColumn();
                        //myDataColumn.AllowDBNull = false;
                        //myDataColumn.AutoIncrement = true;
                        //myDataColumn.AutoIncrementSeed = 1;
                        //myDataColumn.AutoIncrementStep = 1;
                        //myDataColumn.ColumnName = "MultiUOMSR";
                        //myDataColumn.DataType = System.Type.GetType("string");
                        //myDataColumn.Unique = true;
                        //MultiUOMSaveData.Columns.Add(myDataColumn);

                        // End of Mantis Issue 24428
                    }
                    DataRow thisRow;
                    if (MultiUOMSaveData.Rows.Count > 0)
                    {

                        // Rev Sanchita
                        //thisRow = (DataRow)MultiUOMSaveData.Rows[MultiUOMSaveData.Rows.Count - 1];
                        MultiUOMSR = Convert.ToInt32(MultiUOMSaveData.Compute("max([MultiUOMSR])", string.Empty)) + 1;
                        // End of Rev Sanchita
                         //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);
                        // Rev Sanchita 
                        //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, BaseRate, AltRate, UpdateRow, (Convert.ToInt16(thisRow["MultiUOMSR"]) + 1));
                        MultiUOMSaveData.Rows.Add(srlNo, qnty, UomName, AltUomName, AltQnty, UomId, AltUomId, ProductID, BaseRate, AltRate, UpdateRow,MultiUOMSR);
                        // End of Rev Sanchita
                        // End of Mantis Issue 24428
                    }
                    else
                    {
                        MultiUOMSaveData.Rows.Add(srlNo, qnty, UomName, AltUomName, AltQnty, UomId, AltUomId, ProductID, BaseRate, AltRate, UpdateRow, MultiUOMSR);
                    }
                    // Mantis Issue 24428
                  

                   
                    MultiUOMSaveData.AcceptChanges();
                    TempData["MultiUom"] = MultiUOMSaveData;
                    TempData.Keep();
                    //DataTable dtnew=new DataTable();
                    //dtnew
                    //omel = null;
                   // GC.Collect();
                    if (MultiUOMSaveData.Rows.Count > 0)
                    {
                        for (int i = 0; i < MultiUOMSaveData.Rows.Count; i++)
                        {
                            if(srlNo==Convert.ToString(MultiUOMSaveData.Rows[i]["SrlNo"])){
                                omel.Add(new MultiUomModel()
                                {
                                
                                    SrlNo = Convert.ToString(MultiUOMSaveData.Rows[i]["SrlNo"]),
                                    Quantity = Convert.ToString(MultiUOMSaveData.Rows[i]["Quantity"]),
                                    UOM = Convert.ToString(MultiUOMSaveData.Rows[i]["UOM"]),
                                    BaseRate = Convert.ToString(MultiUOMSaveData.Rows[i]["BaseRate"]),
                                    AltUOM = Convert.ToString(MultiUOMSaveData.Rows[i]["AltUOM"]),
                                    AltQuantity = Convert.ToString(MultiUOMSaveData.Rows[i]["AltQuantity"]),
                                    UomId = Convert.ToString(MultiUOMSaveData.Rows[i]["UomId"]),
                                    AltUomId = Convert.ToString(MultiUOMSaveData.Rows[i]["AltUomId"]),
                                    AltRate = Convert.ToString(MultiUOMSaveData.Rows[i]["AltRate"]),
                                    UpdateRow = Convert.ToString(MultiUOMSaveData.Rows[i]["UpdateRow"]),
                                    ProductId = Convert.ToString(MultiUOMSaveData.Rows[i]["ProductId"]),
                                    MultiUOMSR = Convert.ToString(MultiUOMSaveData.Rows[i]["MultiUOMSR"])
                               
                               
                                });
                           }
                        }
                    }

                    //if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                    //{
                    //    DataView dvData = new DataView(MultiUOMSaveData);
                    //    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";

                    //    grid_MultiUOM.DataSource = dvData;
                    //    grid_MultiUOM.DataBind();
                    //}
                    //else
                    //{
                    //    //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);
                    //    //Session["MultiUom"] = MultiUOMSaveData;
                    //    grid_MultiUOM.DataSource = MultiUOMSaveData.DefaultView;
                    //    grid_MultiUOM.DataBind();
                    //}
                }
                return Json(omel, JsonRequestBehavior.AllowGet);
            }
            else if (Type == "UpdateRow")
            {
                string muid = MultiUOMSR_id;
                string SrlNo = "0";
                string Validcheck = "";
                string Quantity = "";
                string UOM = "";
                string AltUOM = "";
                string AltQuantity = "";
                //string UomId = "";
                //string AltUomId = "";
                string ProductId = "";
                string MultiUOMSR = "";
                //string AltRate = Convert.ToString(e.Parameters.Split('~')[10]);
                //string UpdateRow = Convert.ToString(e.Parameters.Split('~')[11]);
                

                DataTable MultiUOMSaveData = new DataTable();

                DataTable dt = (DataTable)TempData["MultiUom"];


                //omel.Add(new MultiUomModel()
                //{
                

                    SrlNo = srlNo;
                    Quantity = qnty;
                    UOM = UomName;
                    
                    AltUOM = AltUomName;
                    AltQuantity = AltQnty;
                    ProductId = ProductID;
                    MultiUOMSR = MultiUOMSR_id;
                //});


                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                //    foreach (DataRow item in MultiUoMresult)
                //    {
                //        SrlNo = Convert.ToString(item["SrlNo"]);
                //        Quantity = Convert.ToString(item["Quantity"]);
                //        UOM = Convert.ToString(item["UOM"]);
                //        AltUOM = Convert.ToString(item["AltUOM"]);
                //        AltQuantity = Convert.ToString(item["AltQuantity"]);
                //        UomId = Convert.ToString(item["UomId"]);
                //        AltUomId = Convert.ToString(item["AltUomId"]);
                //        ProductId = Convert.ToString(item["ProductId"]);
                //        BaseRate = Convert.ToString(item["BaseRate"]);
                //        AltRate = Convert.ToString(item["AltRate"]);
                //        UpdateRow = Convert.ToString(item["UpdateRow"]);
                //        MultiUOMSR = Convert.ToString(item["MultiUOMSR"]);
                //    }
                //}
                //string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                //string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                //string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                //string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                //string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                //string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                //string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);

                //string BaseRate = Convert.ToString(e.Parameters.Split('~')[9]);
                //string AltRate = Convert.ToString(e.Parameters.Split('~')[10]);
                //string UpdateRow = Convert.ToString(e.Parameters.Split('~')[11]);

                DataRow[] MultiUoMresultResult = dt.Select("SrlNo ='" + SrlNo + "' and  MultiUOMSR <>'" + muid + "'");

                foreach (DataRow item in MultiUoMresultResult)
                {
                    if ((AltUomId == item["AltUomId"].ToString()) || (UomId == item["AltUomId"].ToString()))
                    {
                        if (AltQuantity == item["AltQuantity"].ToString())
                        {
                            Validcheck = "DuplicateUOM";
                            //grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "DuplicateAltUOM";
                            return Json("Duplicate Alt UOM", JsonRequestBehavior.AllowGet);
                            //break;
                        }
                    }
                    // Mantis Issue 24428  [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                    if (UpdateRow == "True")
                    {
                        item["UpdateRow"] = "False";
                    }
                    // End of Mantis Issue 24428 
                }


                if (Validcheck != "DuplicateUOM")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                        foreach (DataRow item in MultiUoMresult)
                        {
                            // Rev SAnchita
                            SrlNo = Convert.ToString(item["SrlNo"]);
                            // End of Rev Sanchita
                            item.Table.Rows.Remove(item);
                            break;

                        }
                    }
                    dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, BaseRate, AltRate, UpdateRow, muid);
                }
                //End Rev Sanchita

                TempData["MultiUom"] = dt;
                MultiUOMSaveData = (DataTable)TempData["MultiUom"];
                TempData.Keep();

                MultiUOMSaveData.AcceptChanges();
                TempData["MultiUom"] = MultiUOMSaveData;
                TempData.Keep();
                if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                {
                    DataView dvData = new DataView(MultiUOMSaveData);
                    // dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    // Rev Sanchita
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    // End of Rev Sanchita
                    MultiUOMSaveData = dvData.ToTable();
                    //grid_MultiUOM.DataSource = dvData;
                    //grid_MultiUOM.DataBind();
                    if (MultiUOMSaveData.Rows.Count > 0)
                    {
                        for (int i = 0; i < MultiUOMSaveData.Rows.Count; i++)
                        {
                            omel.Add(new MultiUomModel()
                            {

                                SrlNo = Convert.ToString(MultiUOMSaveData.Rows[i]["SrlNo"]),
                                Quantity = Convert.ToString(MultiUOMSaveData.Rows[i]["Quantity"]),
                                UOM = Convert.ToString(MultiUOMSaveData.Rows[i]["UOM"]),
                                BaseRate = Convert.ToString(MultiUOMSaveData.Rows[i]["BaseRate"]),
                                AltUOM = Convert.ToString(MultiUOMSaveData.Rows[i]["AltUOM"]),
                                AltQuantity = Convert.ToString(MultiUOMSaveData.Rows[i]["AltQuantity"]),
                                UomId = Convert.ToString(MultiUOMSaveData.Rows[i]["UomId"]),
                                AltUomId = Convert.ToString(MultiUOMSaveData.Rows[i]["AltUomId"]),
                                AltRate = Convert.ToString(MultiUOMSaveData.Rows[i]["AltRate"]),
                                UpdateRow = Convert.ToString(MultiUOMSaveData.Rows[i]["UpdateRow"]),
                                ProductId = Convert.ToString(MultiUOMSaveData.Rows[i]["ProductId"]),
                                MultiUOMSR = Convert.ToString(MultiUOMSaveData.Rows[i]["MultiUOMSR"])
                            });
                            
                        }
                    }
                }

            }


            else if (Type == "Delete")
            {
                string SrlNo = MultiUOMSR_id;
                //string SrlNo = "0";
                //string Validcheck = "";
                //string Quantity = "";
                //string UOM = "";
                //string AltUOM = "";
                //string AltQuantity = "";
                //string ProductId = "";
                //string MultiUOMSR = "";


                //DataTable MultiUOMSaveData = new DataTable();

                DataTable MultiUOMSaveData = (DataTable)TempData["MultiUom"];

                //string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                //string AltUOMKeyValue = AltUOMKeyValuewithqnty.Split('|')[0];
                //string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                //string SrlNo = Convert.ToString(e.Parameters.Split('~')[2]);
                //DataTable dt = (DataTable)Session["MultiUOMData"];

                if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = MultiUOMSaveData.Select("MultiUOMSR='" + SrlNo + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                       
                          item.Table.Rows.Remove(item);
                                break;
                            
                    }
                }
                if (MultiUOMSaveData.Rows.Count > 0)
                {
                    for (int i = 0; i < MultiUOMSaveData.Rows.Count; i++)
                    {
                        omel.Add(new MultiUomModel()
                        {

                            SrlNo = Convert.ToString(MultiUOMSaveData.Rows[i]["SrlNo"]),
                            Quantity = Convert.ToString(MultiUOMSaveData.Rows[i]["Quantity"]),
                            UOM = Convert.ToString(MultiUOMSaveData.Rows[i]["UOM"]),
                            BaseRate = Convert.ToString(MultiUOMSaveData.Rows[i]["BaseRate"]),
                            AltUOM = Convert.ToString(MultiUOMSaveData.Rows[i]["AltUOM"]),
                            AltQuantity = Convert.ToString(MultiUOMSaveData.Rows[i]["AltQuantity"]),
                            UomId = Convert.ToString(MultiUOMSaveData.Rows[i]["UomId"]),
                            AltUomId = Convert.ToString(MultiUOMSaveData.Rows[i]["AltUomId"]),
                            AltRate = Convert.ToString(MultiUOMSaveData.Rows[i]["AltRate"]),
                            UpdateRow = Convert.ToString(MultiUOMSaveData.Rows[i]["UpdateRow"]),
                            ProductId = Convert.ToString(MultiUOMSaveData.Rows[i]["ProductId"]),
                            MultiUOMSR = Convert.ToString(MultiUOMSaveData.Rows[i]["MultiUOMSR"])
                        });
                    }
                }
                TempData["MultiUom"] = MultiUOMSaveData;
                TempData.Keep();

            }
            else if (Type == "MultiUOMDisPlay")
            {
                string SrlNo = srlNo;
                //string SrlNo = "0";
                //string Validcheck = "";
                //string Quantity = "";
                //string UOM = "";
                //string AltUOM = "";
                //string AltQuantity = "";
                //string ProductId = "";
                //string MultiUOMSR = "";


                //DataTable MultiUOMSaveData = new DataTable();

                DataTable MultiUOMSaveData = (DataTable)TempData["MultiUom"];
                TempData.Keep();
                //string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                //string AltUOMKeyValue = AltUOMKeyValuewithqnty.Split('|')[0];
                //string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                //string SrlNo = Convert.ToString(e.Parameters.Split('~')[2]);
                //DataTable dt = (DataTable)Session["MultiUOMData"];

               // grid_MultiUOM.JSProperties["cpOpenFocus"] = "";
                DataTable MultiUOMData = new DataTable();

                if (TempData["MultiUom"] != null)
                {
                    MultiUOMData = (DataTable)TempData["MultiUom"];
                }
                else
                {
                    MultiUOMData.Columns.Add("SrlNo", typeof(string));
                    MultiUOMData.Columns.Add("Quantity", typeof(string));
                    MultiUOMData.Columns.Add("UOM", typeof(string));
                    MultiUOMData.Columns.Add("AltUOM", typeof(string));
                    MultiUOMData.Columns.Add("AltQuantity", typeof(string));
                    MultiUOMData.Columns.Add("UomId", typeof(string));
                    MultiUOMData.Columns.Add("AltUomId", typeof(string));
                    MultiUOMData.Columns.Add("ProductId", typeof(string));
                    MultiUOMData.Columns.Add("BaseRate", typeof(string));
                    MultiUOMData.Columns.Add("AltRate", typeof(string));
                    MultiUOMData.Columns.Add("UpdateRow", typeof(string));

                }
                if (MultiUOMData != null && MultiUOMData.Rows.Count > 0)
                {
                    //string Srl_No = srlNo;
                    DataView dvData = new DataView(MultiUOMData);
                    //dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    //MultiUOMData.RowFilter = "SrlNo = '" + Srl_No + "'";

                    omel = dvData.ToTable().Rows.Cast<DataRow>()
                     .Select(r => new MultiUomModel()
                     {
                         SrlNo = r.Field<string>("SrlNo"),
                         Quantity = r.Field<string>("Quantity"),
                         UOM = r.Field<string>("UOM"),
                         BaseRate = r.Field<string>("BaseRate"),
                         AltUOM = r.Field<string>("AltUOM"),
                         AltQuantity = r.Field<string>("AltQuantity"),
                         UomId = r.Field<string>("UomId"),
                         AltUomId = r.Field<string>("AltUomId"),
                         AltRate = r.Field<string>("AltRate"),
                         UpdateRow = r.Field<string>("UpdateRow"),
                         ProductId = r.Field<string>("ProductId"),
                         MultiUOMSR = r.Field<string>("MultiUOMSR")
                     }).ToList();

                   // return Json(omel, JsonRequestBehavior.AllowGet);
                    //if (MultiUOMSaveData.Rows.Count > 0)
                    //{
                    //    for (int i = 0; i < MultiUOMSaveData.Rows.Count; i++)
                    //    {
                    //        omel.Add(new MultiUomModel()
                    //        {

                    //            SrlNo = Convert.ToString(MultiUOMSaveData.Rows[i]["SrlNo"]),
                    //            Quantity = Convert.ToString(MultiUOMSaveData.Rows[i]["Quantity"]),
                    //            UOM = Convert.ToString(MultiUOMSaveData.Rows[i]["UOM"]),
                    //            BaseRate = Convert.ToString(MultiUOMSaveData.Rows[i]["BaseRate"]),
                    //            AltUOM = Convert.ToString(MultiUOMSaveData.Rows[i]["AltUOM"]),
                    //            AltQuantity = Convert.ToString(MultiUOMSaveData.Rows[i]["AltQuantity"]),
                    //            UomId = Convert.ToString(MultiUOMSaveData.Rows[i]["UomId"]),
                    //            AltUomId = Convert.ToString(MultiUOMSaveData.Rows[i]["AltUomId"]),
                    //            AltRate = Convert.ToString(MultiUOMSaveData.Rows[i]["AltRate"]),
                    //            UpdateRow = Convert.ToString(MultiUOMSaveData.Rows[i]["UpdateRow"]),
                    //            ProductId = Convert.ToString(MultiUOMSaveData.Rows[i]["ProductId"]),
                    //            MultiUOMSR = Convert.ToString(MultiUOMSaveData.Rows[i]["MultiUOMSR"])
                    //        });
                    //    }
                    //}
                }
                else
                {
                    //return Json(omel, JsonRequestBehavior.AllowGet);
                    //grid_MultiUOM.DataSource = MultiUOMData.DefaultView;
                    //grid_MultiUOM.DataBind();
                }
                //grid_MultiUOM.JSProperties["cpOpenFocus"] = "OpenFocus";

            }
            return Json(omel, JsonRequestBehavior.AllowGet);
            
        }

        [HttpPost]
        public JsonResult MultiUomGridTempEdit(string MultiUOMSR)
        {
            string AltUOMKeyValue = MultiUOMSR;
            List<MultiUomModel> omel = new List<MultiUomModel>();
            DataTable dt = (DataTable)TempData["MultiUom"];
            TempData.Keep();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + AltUOMKeyValue + "'");

                Decimal BaseQty = Convert.ToDecimal(MultiUoMresult[0]["Quantity"]);
                Decimal BaseRate = Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]);

                Decimal AltQty = Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]);
                Decimal AltRate = Convert.ToDecimal(MultiUoMresult[0]["AltRate"]);
                Decimal AltUom = Convert.ToDecimal(MultiUoMresult[0]["AltUomId"]);
                bool UpdateRow = Convert.ToBoolean(MultiUoMresult[0]["UpdateRow"]);

                omel.Add(new MultiUomModel()
                {

                    //SrlNo = Convert.ToString(MultiUOMSaveData.Rows[i]["SrlNo"]),
                    Quantity = Convert.ToString(MultiUoMresult[0]["Quantity"]),
                    BaseRate = Convert.ToString(MultiUoMresult[0]["BaseRate"]),
                    AltQuantity = Convert.ToString(MultiUoMresult[0]["AltQuantity"]),
                    AltUomId = Convert.ToString(MultiUoMresult[0]["AltUomId"]),
                    AltRate = Convert.ToString(MultiUoMresult[0]["AltRate"]),
                    UpdateRow = Convert.ToString(MultiUoMresult[0]["UpdateRow"]),
                });
                //grid_MultiUOM.JSProperties["cpAllDetails"] = "EditData";

                //grid_MultiUOM.JSProperties["cpBaseQty"] = BaseQty;
                //grid_MultiUOM.JSProperties["cpBaseRate"] = BaseRate;


                //grid_MultiUOM.JSProperties["cpAltQty"] = AltQty;
                //grid_MultiUOM.JSProperties["cpAltUom"] = AltUom;
                //grid_MultiUOM.JSProperties["cpAltRate"] = AltRate;
                //grid_MultiUOM.JSProperties["cpUpdatedrow"] = UpdateRow;
                //grid_MultiUOM.JSProperties["cpuomid"] = AltUOMKeyValue;
            }
            TempData["MultiUom"] = dt;
            TempData.Keep();
            return Json(omel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AltQntyMultiUomGrid(string ProductID)
        {
            string SrlNo = ProductID;
            List<MultiUomModel> omel = new List<MultiUomModel>();
            DataTable dt = (DataTable)TempData["MultiUom"];
            TempData.Keep();
            Decimal BaseQty=0;
            Decimal AltQty=0;
            string AltUom="";
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] MultiUoMresult = dt.Select("UpdateRow ='True' and SrlNo ='" + SrlNo + "'");
                if (MultiUoMresult.Count()>0)
                {
                    BaseQty = Convert.ToDecimal(MultiUoMresult[0]["Quantity"]);
                    AltQty = Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]);
                    AltUom = Convert.ToString(MultiUoMresult[0]["AltUom"]);
                }
                else
                {
                    return Json("Invalid", JsonRequestBehavior.AllowGet);
                }
                
            }
         
            BOMProduct bomproductdataobj = new BOMProduct();
            List<BOMProduct> bomproductdata = new List<BOMProduct>();
           omel.Add(new MultiUomModel()
                        {

                            //SrlNo = Convert.ToString(MultiUOMSaveData.Rows[i]["SrlNo"]),
                            Quantity = Convert.ToString(BaseQty),
                            //UOM = Convert.ToString(MultiUOMSaveData.Rows[i]["UOM"]),
                            //BaseRate = Convert.ToString(MultiUOMSaveData.Rows[i]["BaseRate"]),
                            AltUOM = Convert.ToString(AltUom),
                            AltQuantity = Convert.ToString(AltQty),
                            //UomId = Convert.ToString(MultiUOMSaveData.Rows[i]["UomId"]),
                            //AltUomId = Convert.ToString(MultiUOMSaveData.Rows[i]["AltUomId"]),
                            //AltRate = Convert.ToString(MultiUOMSaveData.Rows[i]["AltRate"]),
                            //UpdateRow = Convert.ToString(MultiUOMSaveData.Rows[i]["UpdateRow"]),
                            //ProductId = Convert.ToString(MultiUOMSaveData.Rows[i]["ProductId"]),
                            //MultiUOMSR = Convert.ToString(MultiUOMSaveData.Rows[i]["MultiUOMSR"])
                        });

           
            //try
            //{
            //    if (TempData["DetailsID"] != null)
            //    {
            //        DetailsID = Convert.ToInt64(TempData["DetailsID"]);
            //        TempData.Keep();
            //    }
              
            //        if (DetailsID > 0)
            //        {
            //        //rev Pratik
            //            // DataTable objData = objdata.GetBOMProductEntryListByID("GetBOMEntryProductsData", DetailsID);
                  
            //        DataTable objData = objdata.GetBOMProductEntryListByID("GETBOMENTRYPRODUCTSDATA_NEW", DetailsID);
            //              //End of rev Pratik
            //        if (objData != null && objData.Rows.Count > 0)
            //        {
            //             dt = objData;
                        
            //          DataRow[]  dtb=dt.Select("ProductID='"+ProductID+"'");
                      
                       
                        

            //            foreach (DataRow row in dt.Rows)
            //            {
            //                bomproductdataobj = new BOMProduct();
            //                bomproductdataobj.SlNO = Convert.ToString(row["SlNO"]);
            //                bomproductdataobj.ProductName = Convert.ToString(row["sProducts_Code"]);
            //                bomproductdataobj.ProductId = Convert.ToString(row["ProductID"]);
            //                bomproductdataobj.ProductDescription = Convert.ToString(row["sProducts_Name"]);
            //                bomproductdataobj.DesignNo = Convert.ToString(row["DesignNo"]);
            //                bomproductdataobj.ItemRevisionNo = Convert.ToString(row["ItemRevisionNo"]);
            //                bomproductdataobj.ProductQty = Convert.ToString(row["StkQty"]);
            //                bomproductdataobj.ProductUOM = Convert.ToString(row["StkUOM"]);
            //                bomproductdataobj.Warehouse = Convert.ToString(row["WarehouseName"]);
            //                bomproductdataobj.Price = Convert.ToString(row["Price"]);
            //                bomproductdataobj.Amount = Convert.ToString(row["Amount"]);
            //                bomproductdataobj.BOMNo = Convert.ToString(row["BOMNo"]);
            //                bomproductdataobj.RevNo = Convert.ToString(row["RevNo"]);
            //                if (!String.IsNullOrEmpty(Convert.ToString(row["RevDate"])))
            //                {
            //                    bomproductdataobj.RevDate = Convert.ToDateTime(row["RevDate"]).ToString("dd-MM-yyyy");
            //                }
            //                else
            //                {
            //                    bomproductdataobj.RevDate = " ";
            //                }
            //                bomproductdataobj.Remarks = Convert.ToString(row["Remarks"]);
            //                bomproductdataobj.ProductsWarehouseID = Convert.ToString(row["WarehouseID"]);
            //                bomproductdataobj.Tag_Details_ID = Convert.ToString(row["Tag_Details_ID"]);
            //                bomproductdataobj.Tag_Production_ID = Convert.ToString(row["Tag_Production_ID"]);
            //                bomproductdataobj.RevNo = Convert.ToString(row["RevNo"]);
            //                 bomproductdataobj.AltQuantity=   AltQty;

            //                bomproductdata.Add(bomproductdataobj);

            //            }
            //            ViewData["BOMEntryProductsTotalAm"] = bomproductdata.Sum(x => Convert.ToDecimal(x.Amount)).ToString();
            //        }
            //        }
              

          
            //catch { }

            return Json(omel, JsonRequestBehavior.AllowGet);
        }
        //End of rev Pratik
    }
}