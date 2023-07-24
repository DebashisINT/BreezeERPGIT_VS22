//================================================== Revision History =============================================
//Rev Number         DATE              VERSION          DEVELOPER           CHANGES
//1.0                24-07-2023        2.0.39           Priti              0026599: Auto Selection of BOM is required in MPS Based on Settings
//====================================================== Revision History =============================================
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
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using UtilityLayer;

namespace Manufacturing.Controllers
{
    public class MPSController : Controller
    {
        MPSViewModel objEstimate = null;
        MPSModel objdata = null;
        DBEngine oDBEngine = new DBEngine();
        string JVNumStr = string.Empty;
        Int32 ProductionID = 0;
        Int32 DetailsID = 0;
        DataTable prodAddlDesc = new DataTable();
        DataTable ResAddlDesc = new DataTable();
        //Int64 GlobalDetailsID = 0;
        UserRightsForPage rights = new UserRightsForPage();
        CommonBL cSOrder = new CommonBL();
        //
        // GET: /MPS/
        public MPSController()
        {
            objEstimate = new MPSViewModel();
            objdata = new MPSModel();
        }     

        public ActionResult Index(string ActionType, Int64 DetailsID = 0)
        {
            try
            {
                

                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/MPSList", "MPS");
                string EntryType = Request.QueryString["ActionType"];
                try
                {
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

                    //string HierarchySelectInEntryModule = cSOrder.GetSystemSettingsResult("Show_Hierarchy");
                    //if (!String.IsNullOrEmpty(HierarchySelectInEntryModule))
                    //{
                    //    if (HierarchySelectInEntryModule.ToUpper().Trim() == "YES")
                    //    {
                    //        ViewBag.Hierarchy = "1";
                    //    }
                    //    else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                    //    {
                    //        ViewBag.Hierarchy = "0";
                    //    }
                    //}

                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "MPS");
                    List<HierarchyList> objHierarchy = new List<HierarchyList>();

                    List<BranchUnit> list = new List<BranchUnit>();
                    var datasetobj = objdata.DropDownDetailForEstimate("GetUnitDropDownData", Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["userbranchHierarchy"]), 0, 0);
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
                    objEstimate.UnitList = list;
                   // TempData["ProdAddlDesc"] = null;
                   // TempData["ResAddlDesc"] = null;
                    TempData["ProductsDetails"] = null;
                   // TempData["ResourceDetails"] = null;
                    //DataTable hierarchydt = oDBEngine.GetDataTable("select 0 as ID ,'Select' as H_Name union select ID,H_Name from V_HIERARCHY");
                    //if (hierarchydt != null && hierarchydt.Rows.Count > 0)
                    //{
                    //    HierarchyList obj = new HierarchyList();
                    //    foreach (DataRow item in hierarchydt.Rows)
                    //    {
                    //        obj = new HierarchyList();
                    //        obj.Hierarchy_id = Convert.ToString(item["ID"]);
                    //        obj.Hierarchy_Name = Convert.ToString(item["H_Name"]);
                    //        objHierarchy.Add(obj);
                    //    }
                    //}
                    //objEstimate.Hierarchy_List = objHierarchy;

                    if (ActionType != "ADD")
                    {
                        if (DetailsID != null)
                        {
                            if (ActionType == "Approve")
                            {
                                TempData["Approve"] = ActionType;
                            }
                            if (ActionType == "View")
                            {
                                TempData["View"] = ActionType;
                            }


                            TempData["DetailsID"] = DetailsID;
                            objEstimate.DetailsID = Convert.ToString(TempData["DetailsID"]);
                           // objEstimate.Approve = Convert.ToString(TempData["Approve"]);
                            ViewBag.View = Convert.ToString(TempData["View"]);
                            TempData.Keep();

                            if (Convert.ToInt64(objEstimate.DetailsID) > 0)
                            {
                                DataTable objData = objdata.GetEstimateProductEntryListByID("GetMPSEntryDetailsData", Convert.ToInt64(objEstimate.DetailsID));
                                if (objData != null && objData.Rows.Count > 0)
                                {
                                    DataTable dt = objData;
                                    foreach (DataRow row in dt.Rows)
                                    {
                                        objEstimate.DetailsID = Convert.ToString(row["Details_ID"]);
                                        //objEstimate.ProductionID = Convert.ToString(row["Production_ID"]);
                                        objEstimate.Estimate_SCHEMAID = Convert.ToString(row["MPS_SchemaID"]);
                                        objEstimate.EstimateNo = Convert.ToString(row["MPS_No"]);
                                        objEstimate.EstimateDate = Convert.ToString(row["MPS_Date"]);
                                       // objEstimate.RevisionNo = Convert.ToString(row["REV_No"]);
                                       // objEstimate.RevisionDate = Convert.ToString(row["REV_Date"]);
                                        objEstimate.Unit = Convert.ToString(row["BRANCH_ID"]);                                    

                                        objEstimate.dtEstimateDate = Convert.ToDateTime(row["MPS_Date"]);
                                       
                                        objEstimate.Customer_ID = Convert.ToString(row["CUSTOMER_ID"]);
                                        objEstimate.Customer = Convert.ToString(row["CUSTOMER_NAME"]);
                                        if (Convert.ToString(row["EstimateStartDate"]) != "")
                                        objEstimate.EstimateStartDate_dt = Convert.ToString(row["EstimateStartDate"]);
                                        if (Convert.ToString(row["EstimateEndDate"]) != "")
                                        objEstimate.EstimateEndDate_dt = Convert.ToString(row["EstimateEndDate"]);
                                        if (Convert.ToString(row["ActualsStartDate"]) != "")
                                        objEstimate.ActualsStartDate_dt = Convert.ToString(row["ActualsStartDate"]);
                                        if (Convert.ToString(row["ActualsEndDate"]) != "")
                                        objEstimate.ActualsEndDate_dt = Convert.ToString(row["ActualsEndDate"]);


                                        objEstimate.OrderID = Convert.ToString(row["OrderId"]);
                                        objEstimate.OrderNo = Convert.ToString(row["Order_Number"]);

                                        ViewBag.Customer_id = Convert.ToString(row["CUSTOMER_ID"]);
                                        ViewBag.ContractNo = Convert.ToString("");
                                        //ViewBag.ApproveStus = Convert.ToString(row["ApproveStus"]);
                                        ViewBag.Unit = Convert.ToString(row["BRANCH_ID"]);
                                        ViewBag.EstDate = Convert.ToString(row["MPS_Date"]);

                                        
                                    }
                                }
                            }
                        }
                    }
                }
                catch { }

                //ctxtRevisionDate.SetMinDate(new Date(cPLQuoteDate.GetDate().toDateString()))

              //  objEstimate.RevisionDate = DateTime.Now.ToString();
                objEstimate.EstimateDate = DateTime.Now.ToString();
               // objEstimate.ApprovRevSettings = ApprovalRevisionRequireInEstimate;

               // objEstimate.RevisionRequiredEveryAfterApproval = RevisionRequiredEveryAfterApproval;

                objEstimate.TaxID = "1";
                TempData["Count"] = 1;
                TempData.Keep();

                ViewBag.CanView = rights.CanView;
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;
                ViewBag.CanCancel = rights.CanCancel;
                ViewBag.CanApproved = rights.CanApproved;
               // ViewBag.ProjectShow = ProjectSelectInEntryModule;
                //ViewBag.ProjectMandatoryInEntry = ProjectMandatoryInEntry;
               // ViewBag.CanAddUpdateDocuments = rights.CanAddUpdateDocuments;
              //  ViewBag.EntryType = EntryType;
                //if (ApprovalRevisionRequireInEstimate == "No")
                //{
                //    ViewBag.CanApproved = false;
                //}
              //  ViewBag.ApprovalRevisionRequire = ApprovalRevisionRequireInEstimate;

                return View("~/Views/MPS/Index.cshtml", objEstimate);

            }
            catch
            {
                return RedirectToAction("Login", "Index", new { area = "" });
            }
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
            String strType = "160";
            
            DataTable Schemadt = objdata.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, strType, "Y");
           
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

        public ActionResult GetEstimateResources()
        {
            List<EstimateResource> objList = new List<EstimateResource>();
            EstimateResource dataobj = new EstimateResource();
            Int64 DetailsID = 0;
            if (TempData["DetailsID"] != null)
            {
                DetailsID = Convert.ToInt64(TempData["DetailsID"]);
                TempData["DetailsID"] = null;

            }
            DataTable dt = new DataTable();
            if (DetailsID > 0 && TempData["ResourceDetails"] == null)
            {
                DataTable dtable = new DataTable();

                dtable.Clear();
                dtable.Columns.Add("HIddenID", typeof(System.Guid));
                dtable.Columns.Add("SlNO", typeof(System.String));
                dtable.Columns.Add("ProductName", typeof(System.String));
                dtable.Columns.Add("ProductId", typeof(System.String));
                dtable.Columns.Add("ProductDescription", typeof(System.String));
                dtable.Columns.Add("ProductQty", typeof(System.String));
                dtable.Columns.Add("ProductUOM", typeof(System.String));
                dtable.Columns.Add("StockQty", typeof(System.String));
                dtable.Columns.Add("StockUOM", typeof(System.String));
                dtable.Columns.Add("Price", typeof(System.String));
                dtable.Columns.Add("Amount", typeof(System.String));
                dtable.Columns.Add("Remarks", typeof(System.String));
                dtable.Columns.Add("UpdateEdit", typeof(System.String));
                dtable.Columns.Add("Charges", typeof(System.String));
                dtable.Columns.Add("Discount", typeof(System.String));
                dtable.Columns.Add("NetAmount", typeof(System.String));
                dtable.Columns.Add("BudgetedPrice", typeof(System.String));
                dtable.Columns.Add("TaxTypeID", typeof(System.String));
                dtable.Columns.Add("TaxType", typeof(System.String));
                dtable.Columns.Add("ProdHSN", typeof(System.String));
                dtable.Columns.Add("AddlDesc", typeof(System.String));
                dtable.Columns.Add("Sellable", typeof(System.String));
                dtable.Columns.Add("SellableID", typeof(System.String));
                dtable.Columns.Add("ProductDetailsID", typeof(System.String));
                dtable.Columns.Add("BalQty", typeof(System.String));

                DataTable objData = objdata.GetEstimateProductEntryListByID("GetEstimateEntryResourcesData", DetailsID);
                if (objData != null && objData.Rows.Count > 0)
                {
                    //TempData["ResAddlDesc"] = null;
                    //if (ResAddlDesc == null || ResAddlDesc.Rows.Count == 0)
                    //{
                    //    ResAddlDesc.Columns.Add("SrlNo", typeof(string));
                    //    ResAddlDesc.Columns.Add("AdditionRemarks", typeof(string));
                    //}

                    String Gid = "";
                    dt = objData;
                    foreach (DataRow row in dt.Rows)
                    {
                        Gid = Guid.NewGuid().ToString();
                        dataobj = new EstimateResource();
                        dataobj.SlNO = Convert.ToString(row["SlNO"]);
                        dataobj.ProductName = Convert.ToString(row["ProductName"]);
                        dataobj.ProductId = Convert.ToString(row["ProductID"]);
                        dataobj.ProductDescription = Convert.ToString(row["sProducts_Description"]);
                        dataobj.ProductQty = Convert.ToString(row["StkQty"]);
                        dataobj.ProductUOM = Convert.ToString(row["StkUOM"]);
                        //dataobj.Warehouse = Convert.ToString(row["WarehouseName"]);
                        dataobj.Price = Convert.ToString(row["Price"]);
                        dataobj.Amount = Convert.ToString(row["Amount"]);
                        dataobj.Remarks = Convert.ToString(row["Remarks"]);
                        //dataobj.ProductsWarehouseID = Convert.ToString(row["WarehouseID"]);
                        dataobj.ResourceCharges = Convert.ToString(row["Charges"]);

                        dataobj.NetAmount = Convert.ToString(row["NetAmount"]);
                        dataobj.BudgetedPrice = Convert.ToString(row["BudgetedPrice"]);
                        dataobj.TaxTypeID = Convert.ToString(row["TaxTypeID"]);
                        dataobj.Discount = Convert.ToString(row["Discount"]);
                        dataobj.TaxType = Convert.ToString(row["TaxType"]);
                        dataobj.AddlDesc = Convert.ToString(row["ADDITIONAL_REMARKS"]);
                        dataobj.ProdHSN = Convert.ToString(row["PROD_HSN"]);
                        dataobj.Sellable = Convert.ToString(row["Sellable"]);
                        dataobj.ProductDetailsID = Convert.ToString(row["ProductDetailsID"]);
                        dataobj.BalQty = Convert.ToString(row["BalQty"]);
                        //ResAddlDesc.Rows.Add(Convert.ToString(row["SlNO"]), Convert.ToString(row["ADDITIONAL_REMARKS"]));
                        dataobj.Guids = Gid;
                        objList.Add(dataobj);


                        object[] trow = { Gid, Convert.ToString(row["SlNO"]),Convert.ToString(row["ProductName"]),Convert.ToString(row["ProductID"]),
                                            Convert.ToString(row["sProducts_Description"]),Convert.ToString(row["StkQty"]),Convert.ToString(row["StkUOM"]),
                                            null,null, Convert.ToString(row["Price"]),Convert.ToString(row["Amount"]),Convert.ToString(row["Remarks"]),
                                            "1",Convert.ToString(row["Charges"]),Convert.ToString(row["Discount"]),Convert.ToString(row["NetAmount"]),
                                            Convert.ToString(row["BudgetedPrice"]), Convert.ToString(row["TaxTypeID"]),Convert.ToString(row["TaxType"]),
                                            Convert.ToString(row["PROD_HSN"]),Convert.ToString(row["ADDITIONAL_REMARKS"]),Convert.ToString(row["Sellable"]),
                                            Convert.ToString(row["SellableID"]),Convert.ToString(row["ProductDetailsID"]),Convert.ToString(row["BalQty"]) };
                        dtable.Rows.Add(trow);
                    }

                    //TempData["ResAddlDesc"] = ResAddlDesc;
                    //TempData.Keep();

                    dt = dtable;

                    ViewData["EstimateResourcesTotalAm"] = objList.Sum(x => Convert.ToDecimal(x.NetAmount)).ToString();
                }
            }
            else
            {
                dt = (DataTable)TempData["ResourceDetails"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        dataobj = new EstimateResource();
                        dataobj.SlNO = Convert.ToString(row["SlNO"]);
                        dataobj.ProductName = Convert.ToString(row["ProductName"]);
                        dataobj.ProductId = Convert.ToString(row["ProductId"]);
                        dataobj.ProductDescription = Convert.ToString(row["ProductDescription"]);
                        dataobj.ProductQty = Convert.ToString(row["ProductQty"]);
                        dataobj.ProductUOM = Convert.ToString(row["ProductUOM"]);
                        dataobj.Price = Convert.ToString(row["Price"]);
                        dataobj.Amount = Convert.ToString(row["Amount"]);
                        dataobj.Remarks = Convert.ToString(row["Remarks"]);
                        dataobj.ResourceCharges = Convert.ToString(row["Charges"]);

                        dataobj.NetAmount = Convert.ToString(row["NetAmount"]);
                        dataobj.BudgetedPrice = Convert.ToString(row["BudgetedPrice"]);
                        dataobj.TaxTypeID = Convert.ToString(row["TaxTypeID"]);
                        dataobj.Discount = Convert.ToString(row["Discount"]);
                        dataobj.TaxType = Convert.ToString(row["TaxType"]);
                        dataobj.AddlDesc = Convert.ToString(row["AddlDesc"]);
                        dataobj.ProdHSN = Convert.ToString(row["ProdHSN"]);
                        dataobj.Guids = Convert.ToString(row["HIddenID"]);
                        dataobj.Sellable = Convert.ToString(row["Sellable"]);
                        dataobj.ProductDetailsID = Convert.ToString(row["ProductDetailsID"]);
                        dataobj.BalQty = Convert.ToString(row["BalQty"]);
                        objList.Add(dataobj);
                    }
                }
            }
            TempData["ResourceDetails"] = dt;
            TempData.Keep();
            //return PartialView("~/Views/PMS/Estimate/_BOMResourcesGrid.cshtml", objList);
            return PartialView("~/Views/PMS/Estimate/EstimateResourcesList.cshtml", objList);
        }

        public ActionResult GetEstimateProductEntryList()
        {
            EstimateProduct Estimateproductdataobj = new EstimateProduct();
            List<EstimateProduct> Estimateproductdata = new List<EstimateProduct>();
            Int64 DetailsID = 0;
            try
            {

                if (TempData["DetailsID"] != null)
                {
                    DetailsID = Convert.ToInt64(TempData["DetailsID"]);
                    TempData.Keep();
                }
                
                DataTable dt = new DataTable();
                if (DetailsID > 0 && TempData["ProductsDetails"] == null)
                {
                    DataTable objData = objdata.GetEstimateProductEntryListByID("GetEstimateEntryProductsData", DetailsID);
                    if (objData != null && objData.Rows.Count > 0)
                    {
                        dt = objData;                        
                        DataTable dtable = new DataTable();

                        dtable.Clear();
                        dtable.Columns.Add("HIddenID", typeof(System.Guid));
                        dtable.Columns.Add("SlNO", typeof(System.String));
                       // dtable.Columns.Add("EstimateroductsID", typeof(System.String));
                       // dtable.Columns.Add("Details_ID", typeof(System.String));
                        dtable.Columns.Add("ProductName", typeof(System.String));
                        dtable.Columns.Add("ProductId", typeof(System.String));
                        dtable.Columns.Add("ProductDescription", typeof(System.String));
                        dtable.Columns.Add("ProductQty", typeof(System.String));
                        dtable.Columns.Add("ProductUOM", typeof(System.String));                        
                        dtable.Columns.Add("Price", typeof(System.String));
                        dtable.Columns.Add("Amount", typeof(System.String));
                        dtable.Columns.Add("Remarks", typeof(System.String));
                        dtable.Columns.Add("UpdateEdit", typeof(System.String));                      
                        dtable.Columns.Add("UOMID", typeof(System.String));
                        dtable.Columns.Add("BOMID", typeof(System.String));
                        dtable.Columns.Add("BOMNO", typeof(System.String));

                        String Gid = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            Gid = Guid.NewGuid().ToString();
                            Estimateproductdataobj = new EstimateProduct();
                            Estimateproductdataobj.SlNO = Convert.ToString(row["SlNO"]);
                            Estimateproductdataobj.ProductName = Convert.ToString(row["sProducts_Name"]);
                            Estimateproductdataobj.ProductId = Convert.ToString(row["ProductID"]);
                            Estimateproductdataobj.ProductDescription = Convert.ToString(row["sProducts_Description"]);
                            Estimateproductdataobj.ProductQty = Convert.ToString(row["Qty"]);
                            Estimateproductdataobj.ProductUOM = Convert.ToString(row["UOM_Name"]);                         
                            Estimateproductdataobj.Price = Convert.ToString(row["Price"]);
                            Estimateproductdataobj.Amount = Convert.ToString(row["Amount"]);                           
                            Estimateproductdataobj.Remarks = Convert.ToString(row["Remarks"]);                         
                            Estimateproductdataobj.Guids = Gid;
                            Estimateproductdataobj.UOMID = Convert.ToString(row["UOMID"]);
                            Estimateproductdataobj.BOMID = Convert.ToString(row["BOMID"]);
                            Estimateproductdataobj.BOMNO = Convert.ToString(row["BOMNO"]);

                            Estimateproductdata.Add(Estimateproductdataobj);


                            //object[] trow = { Gid, row["SlNO"],null,DetailsID,Convert.ToString(row["sProducts_Name"]),Convert.ToString(row["ProductID"]),
                            //                    Convert.ToString(row["sProducts_Description"]),Convert.ToString(row["StkQty"]),Convert.ToString(row["StkUOM"]),0,
                            //        null,Convert.ToString(row["WarehouseName"]),Convert.ToString(row["WarehouseID"]),Convert.ToString(row["Price"]),Convert.ToString(row["Amount"]),
                            //        Convert.ToString(row["Remarks"]),"1",Convert.ToString(row["Tag_Details_ID"]),Convert.ToString(row["Tag_Production_ID"]),Convert.ToString(row["Charges"]),
                            //        Convert.ToString(row["Discount"]),Convert.ToString(row["NetAmount"]),Convert.ToString(row["BudgetedPrice"]),Convert.ToString(row["TaxTypeID"]),
                            //        Convert.ToString(row["TaxType"]),Convert.ToString(row["PROD_HSN"]),Convert.ToString(row["ADDITIONAL_REMARKS"]),Convert.ToString(row["Sellable"]) 
                            //                ,Convert.ToString(row["SellableID"]),Convert.ToString(row["ProductDetailsID"]),Convert.ToString(row["BalQty"]) };
                            object[] trow = { Gid, row["SlNO"],Convert.ToString(row["sProducts_Name"]),Convert.ToString(row["ProductID"]),
                                                Convert.ToString(row["sProducts_Description"]),Convert.ToString(row["Qty"]),Convert.ToString(row["UOM_Name"]),
                                    Convert.ToString(row["Price"]),Convert.ToString(row["Amount"]),
                                    Convert.ToString(row["Remarks"]),"1" ,  Convert.ToString(row["UOMID"])    
                                    ,  Convert.ToString(row["BOMID"])  
                                    ,  Convert.ToString(row["BOMNO"])  
                             
                                            };
                            dtable.Rows.Add(trow);
                        }
                        //TempData["ProdAddlDesc"] = prodAddlDesc;
                        //TempData.Keep();

                        dt = dtable;

                        //ViewData["EstimateEntryProductsTotalAm"] = Estimateproductdata.Sum(x => Convert.ToDecimal(x.NetAmount)).ToString();
                    }
                }
                else
                {
                    dt = (DataTable)TempData["ProductsDetails"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Estimateproductdataobj = new EstimateProduct();
                            Estimateproductdataobj.SlNO = Convert.ToString(row["SlNO"]);
                            Estimateproductdataobj.ProductName = Convert.ToString(row["ProductName"]);
                            Estimateproductdataobj.ProductId = Convert.ToString(row["ProductId"]);
                            Estimateproductdataobj.ProductDescription = Convert.ToString(row["ProductDescription"]);
                            Estimateproductdataobj.ProductQty = Convert.ToString(row["ProductQty"]);
                            Estimateproductdataobj.ProductUOM = Convert.ToString(row["ProductUOM"]);                      
                            Estimateproductdataobj.Price = Convert.ToString(row["Price"]);
                            Estimateproductdataobj.Amount = Convert.ToString(row["Amount"]);
                            Estimateproductdataobj.Remarks = Convert.ToString(row["Remarks"]);                          
                            Estimateproductdataobj.Guids = Convert.ToString(row["HIddenID"]);                         
                            Estimateproductdataobj.UOMID = Convert.ToString(row["UOMID"]);

                            Estimateproductdataobj.BOMID = Convert.ToString(row["BOMID"]);
                            Estimateproductdataobj.BOMNO = Convert.ToString(row["BOMNO"]);

                            Estimateproductdata.Add(Estimateproductdataobj);
                        }
                        //ViewData["EstimateEntryProductsTotalAm"] = Estimateproductdata.Sum(x => Convert.ToDecimal(x.NetAmount)).ToString();
                    }
                }
                TempData["ProductsDetails"] = dt;
                TempData.Keep();

            }
            catch { }
            // return PartialView("~/Views/PMS/Estimate/_BOMProductEntryGrid.cshtml", Estimateproductdata);
            return PartialView("~/Views/MPS/EstimateProductList.cshtml", Estimateproductdata);
        }

        [ValidateInput(false)]
        public ActionResult BatchEditingUpdateEstimateProductEntry(DevExpress.Web.Mvc.MVCxGridViewBatchUpdateValues<EstimateProduct, int> updateValues, MPSViewModel options)
        {
            TempData["Count"] = (int)TempData["Count"] + 1;
            TempData.Keep();
            String NumberScheme = "";
            String Message = "";
            Int64 SaveDataArea = 0;

            List<udtEstimateProduct> udt = new List<udtEstimateProduct>();

            //if ((int)TempData["Count"] != 2)
            //{
            Boolean IsProcess = false;
            List<EstimateProduct> list = new List<EstimateProduct>();
            //foreach (var product in updateValues.Insert)
            if (updateValues.Insert.Count > 0 && Convert.ToInt64(options.DetailsID) < 1)
            {
                //if (updateValues.IsValid(product))
                //{
                List<udtEstimateProducts> udtlist = new List<udtEstimateProducts>();
                udtEstimateProducts obj = null;
                updateValues.Insert = updateValues.Insert.OrderBy(x => Convert.ToInt64(x.SlNO)).ToList();
                foreach (var item in updateValues.Insert)
                {
                    if (Convert.ToInt64(item.ProductId) > 0)
                    {
                        //if (Convert.ToDecimal(item.ProductQty) > 0)
                        //{
                        //if (!String.IsNullOrEmpty(item.BOMNo) && !String.IsNullOrEmpty(item.RevNo))
                        //{
                        //    DataSet dt = new DataSet();
                        //     dt = objdata.BOMProductEntryInsertUpdate()
                        //}
                        //if (String.IsNullOrEmpty(item.Tag_Production_ID))
                        //{
                        //    item.Tag_Production_ID = "0";
                        //}
                        //if (String.IsNullOrEmpty(item.Tag_Details_ID))
                        //{
                        //    item.Tag_Details_ID = "0";
                        //}

                        obj = new udtEstimateProducts();
                        obj.ProductID = Convert.ToInt64(item.ProductId);
                        obj.StkQty = Convert.ToDecimal(item.ProductQty);
                        obj.StkUOM = (item.ProductUOM);
                      //  obj.IssuesQty = Convert.ToDecimal(0);
                      //  obj.IssuesUOM = (" ");
                       // obj.WarehouseID = Convert.ToInt64(item.ProductsWarehouseID);
                        obj.Price = Convert.ToDecimal(item.Price);
                        obj.Amount = Convert.ToDecimal(item.Amount);
                       // obj.Tag_Details_ID = Convert.ToInt64(item.Tag_Details_ID);
                       // obj.Tag_Production_ID = Convert.ToInt64(item.Tag_Production_ID);
                        //obj.Tag_REV_No = item.RevNo;
                        obj.Remarks = (item.Remarks);
                        obj.SlNo = (item.SlNO);
                       // obj.Charges = (item.Charges);
                      //  obj.Discount = (item.Discount);
                       // obj.NetAmount = (item.NetAmount);
                       // obj.BudgetedPrice = (item.BudgetedPrice);
                        //obj.TaxTypeID = (item.TaxTypeID);
                        //obj.TaxType = (item.TaxType);
                        obj.UOMID = (item.UOMID);
                        udtlist.Add(obj);
                        //}
                    }
                }
                if (udtlist.Count > 0)
                {
                    SaveDataArea = 1;
                    //if (options.BOMNo)
                    NumberScheme = checkNMakeEstimateCode(options.strEstimateNo, Convert.ToInt32(options.Estimate_SCHEMAID), Convert.ToDateTime(options.RevisionDate));
                    if (NumberScheme == "ok")
                    {
                        udtlist = udtlist.OrderBy(x => Convert.ToInt64(x.SlNo)).ToList();
                        foreach (var item in udtlist)
                        {
                            udtEstimateProduct obj1 = new udtEstimateProduct();
                            obj1.ProductID = Convert.ToInt64(item.ProductID);
                            obj1.StkQty = Convert.ToDecimal(item.StkQty);
                            obj1.StkUOM = (item.StkUOM);
                           // obj1.IssuesQty = (item.IssuesQty);
                          //  obj1.IssuesUOM = (" ");
                           // obj1.WarehouseID = Convert.ToInt64(item.WarehouseID);
                            obj1.Price = Convert.ToDecimal(item.Price);
                            obj1.Amount = Convert.ToDecimal(item.Amount);
                          //  obj1.Tag_Details_ID = Convert.ToInt64(item.Tag_Details_ID);
                          //  obj1.Tag_Production_ID = Convert.ToInt64(item.Tag_Production_ID);
                          //  obj1.Tag_REV_No = item.Tag_REV_No;
                            obj1.Remarks = (item.Remarks);
                          //  obj1.Charges = (item.Charges);
                          //  obj1.Discount = (item.Discount);
                          //  obj1.NetAmount = (item.NetAmount);
                          //  obj1.BudgetedPrice = (item.BudgetedPrice);
                          //  obj1.TaxTypeID = (item.TaxTypeID);
                          //  obj1.TaxType = (item.TaxType);
                            obj1.SrlNo = (item.SlNo);
                          //  obj1.AddlDesc = (item.AddlDesc);
                            obj1.UOMID = (item.UOMID);
                            udt.Add(obj1);
                        }
                        IsProcess = EstimateProductInsertUpdate(udt, options);
                    }
                    else
                    {
                        Message = NumberScheme;
                    }
                }
                // list.Add(product);
                //}
            }
            if (((updateValues.Update.Count > 0 && Convert.ToInt64(options.DetailsID) > 0) || (updateValues.Insert.Count > 0 && Convert.ToInt64(options.DetailsID) < 1)) && SaveDataArea == 0)
            {
                List<udtEstimateProducts> udtlist = new List<udtEstimateProducts>();
                udtEstimateProducts obj = null;

                foreach (var item in updateValues.Update)
                {
                    if (Convert.ToInt64(item.ProductId) > 0)
                    {
                        //if (Convert.ToDecimal(item.ProductQty) > 0)
                        //{
                        obj = new udtEstimateProducts();
                        obj.ProductID = Convert.ToInt64(item.ProductId);
                        obj.StkQty = Convert.ToDecimal(item.ProductQty);
                        obj.StkUOM = (item.ProductUOM);
                       // obj.IssuesQty = Convert.ToDecimal(0);
                       // obj.IssuesUOM = (" ");
                        //obj.WarehouseID = Convert.ToInt64(item.ProductsWarehouseID);
                        obj.Price = Convert.ToDecimal(item.Price);
                        obj.Amount = Convert.ToDecimal(item.Amount);
                       // obj.Tag_Details_ID = Convert.ToInt64(item.Tag_Details_ID);
                        //obj.Tag_Production_ID = Convert.ToInt64(item.Tag_Production_ID);
                        //obj.Tag_REV_No = item.RevNo;
                        obj.Remarks = (item.Remarks);
                        obj.SlNo = (item.SlNO);
                       // obj.Charges = (item.Charges);
                       // obj.Discount = (item.Discount);
                       // obj.NetAmount = (item.NetAmount);
                       // obj.BudgetedPrice = (item.BudgetedPrice);
                        //obj.TaxTypeID = (item.TaxTypeID);
                       // obj.TaxType = (item.TaxType);
                       // obj.AddlDesc = (item.AddlDesc);
                        obj.UOMID = (item.UOMID);
                        udtlist.Add(obj);
                        // }
                    }
                }

                foreach (var item in updateValues.Insert)
                {
                    if (Convert.ToInt64(item.ProductId) > 0)
                    {
                        
                        obj = new udtEstimateProducts();
                        obj.ProductID = Convert.ToInt64(item.ProductId);
                        obj.StkQty = Convert.ToDecimal(item.ProductQty);
                        obj.StkUOM = (item.ProductUOM);                     
                        obj.Price = Convert.ToDecimal(item.Price);
                        obj.Amount = Convert.ToDecimal(item.Amount);                      
                        obj.Remarks = (item.Remarks);
                        obj.SlNo = (item.SlNO);                        
                        obj.UOMID = (item.UOMID);
                        udtlist.Add(obj);
                      
                    }
                }              

                if (udtlist.Count > 0)
                {
                    SaveDataArea = 1;                    
                    udtlist = udtlist.OrderBy(x => Convert.ToInt64(x.SlNo)).ToList();
                    foreach (var item in udtlist)
                    {
                        udtEstimateProduct obj1 = new udtEstimateProduct();
                        obj1.ProductID = Convert.ToInt64(item.ProductID);
                        obj1.StkQty = Convert.ToDecimal(item.StkQty);
                        obj1.StkUOM = (item.StkUOM);                       
                        obj1.Price = Convert.ToDecimal(item.Price);
                        obj1.Amount = Convert.ToDecimal(item.Amount);                      
                        obj1.Remarks = (item.Remarks);                     
                        obj1.SrlNo = (item.SlNo);
                        obj1.UOMID = (item.UOMID);
                        udt.Add(obj1);
                    }

                    IsProcess = EstimateProductInsertUpdate(udt, options);
                }
            }


            TempData["Count"] = 1;
            TempData.Keep();
            ViewData["ProductionID"] = ProductionID;
            ViewData["DetailsID"] = DetailsID;
            ViewData["EstimateNo"] = JVNumStr;
            ViewData["Success"] = IsProcess;
            ViewData["Message"] = Message;
            //}
            return PartialView("~/Views/MPS/_BOMProductEntryGrid.cshtml", updateValues.Update);
            //return Json(IsProcess, JsonRequestBehavior.AllowGet);
        }

        public Boolean EstimateProductInsertUpdate(List<udtEstimateProduct> obj, MPSViewModel obj2)
        {
            Boolean Success = false;

            try
            {
                string ContrtNo = "";
                int j = 1;

                if (obj2.ContractNo != null && obj2.ContractNo.Count > 0)
                {
                    foreach (string item in obj2.ContractNo)
                    {
                        if (j > 1)
                            ContrtNo = ContrtNo + "," + item;
                        else
                            ContrtNo = item;
                        j++;
                    }
                }

                DataTable dtEstimate_PRODUCTS = new DataTable();
                dtEstimate_PRODUCTS = ToDataTable(obj);

                DataTable dtEstimate_Resource = new DataTable();
                if (TempData["Resource"] != null)
                {
                    List<udtEstimateResources> obj1 = (List<udtEstimateResources>)TempData["Resource"];
                    dtEstimate_Resource = ToDataTable(obj1);
                    TempData.Keep();
                }
                else
                {
                    dtEstimate_Resource.Columns.Add("ProductID");
                    dtEstimate_Resource.Columns.Add("StkQty");
                    dtEstimate_Resource.Columns.Add("StkUOM");
                    //dtEstimate_Resource.Columns.Add("WarehouseID");
                    dtEstimate_Resource.Columns.Add("Price");
                    dtEstimate_Resource.Columns.Add("Amount");
                    dtEstimate_Resource.Columns.Add("Remarks");
                    //dtEstimate_Resource.Columns.Add("Charges");
                    //dtEstimate_Resource.Columns.Add("NetAmount");
                    //dtEstimate_Resource.Columns.Add("BudgetedPrice");
                    //dtEstimate_Resource.Columns.Add("TaxTypeID");
                    //dtEstimate_Resource.Columns.Add("Discount");
                   // dtEstimate_Resource.Columns.Add("TaxType");
                    dtEstimate_Resource.Columns.Add("SrlNo");
                }

                DataTable dtResourceAddlDesc = new DataTable();
                if (TempData["ResAddlDesc"] != null)
                {
                    dtResourceAddlDesc = (DataTable)TempData["ResAddlDesc"];
                }
                else
                {
                    dtResourceAddlDesc.Columns.Add("SrlNo");
                    dtResourceAddlDesc.Columns.Add("AdditionRemarks");
                }

                DataTable dtProductAddlDesc = new DataTable();
                if (TempData["ProdAddlDesc"] != null)
                {
                    dtProductAddlDesc = (DataTable)TempData["ProdAddlDesc"];
                }
                else
                {
                    dtProductAddlDesc.Columns.Add("SrlNo");
                    dtProductAddlDesc.Columns.Add("AdditionRemarks");
                }

                DataSet dt = new DataSet();
                if (Convert.ToInt64(obj2.DetailsID) > 0)
                {
                    if (!String.IsNullOrEmpty(obj2.strEstimateNo))
                    {
                        JVNumStr = obj2.strEstimateNo;
                    }
                    //dt = objdata.EstimateProductEntryInsertUpdate("UPDATEMAINPRODUCT", JVNumStr, Convert.ToDateTime(obj2.EstimateDate), obj2.RevisionNo, Convert.ToDateTime(obj2.RevisionDate), Convert.ToInt32(obj2.Unit),
                    //    dtEstimate_PRODUCTS, dtEstimate_Resource, Convert.ToInt32(obj2.Estimate_SCHEMAID), Convert.ToDecimal(obj2.ActualAdditionalCost), obj2.Proposal_ID, obj2.Quotation_ID, obj2.HeadRemarks,
                    //    obj2.Customer_ID, ContrtNo, obj2.ProjectID, obj2.TaxID, obj2.Approve, obj2.ApprvRejct, obj2.ApproveRemarks, obj2.ApprovRevSettings, dtProductAddlDesc, dtResourceAddlDesc,
                    //    Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), Convert.ToInt64(Session["userid"]), 0, Convert.ToInt64(obj2.DetailsID));
                }
                else
                {
                    //dt = objdata.EstimateProductEntryInsertUpdate("INSERTMAINPRODUCT", JVNumStr, Convert.ToDateTime(obj2.EstimateDate), obj2.RevisionNo, Convert.ToDateTime(obj2.RevisionDate), Convert.ToInt32(obj2.Unit)
                    //    , dtEstimate_PRODUCTS, dtEstimate_Resource, Convert.ToInt32(obj2.Estimate_SCHEMAID), Convert.ToDecimal(obj2.ActualAdditionalCost), obj2.Proposal_ID, obj2.Quotation_ID, obj2.HeadRemarks,
                    //     obj2.Customer_ID, ContrtNo, obj2.ProjectID, obj2.TaxID, obj2.Approve, obj2.ApprvRejct, obj2.ApproveRemarks, obj2.ApprovRevSettings, dtProductAddlDesc, dtResourceAddlDesc,
                    //     Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), Convert.ToInt64(Session["userid"]));
                    //dt = objemployee.EmployeesTargetByCodeInsertUpdate(obj.EmployeeTargetSettingID, obj2.EmpTypeID, obj2.CounterType, obj.EmployeeCode, obj2.SettingMonth, obj2.SettingYear, obj.OrderValue, obj.NewCounter, obj.Collection, obj.Revisit);
                }
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        Success = Convert.ToBoolean(row["Success"]);
                        ProductionID = Convert.ToInt32(row["ProductionID"]);
                        DetailsID = Convert.ToInt32(row["DetailsID"]);
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

        public string checkNMakeEstimateCode(string manual_str, int sel_schema_Id, DateTime RevisionDate)
        {
            DataTable dtSchema = new DataTable();
            DataTable dtC = new DataTable();
            string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
            int EmpCode, prefLen, sufxLen, paddCounter;

            if (sel_schema_Id > 0)
            {
                dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + sel_schema_Id);
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

                    if ((dtSchema.Rows[0]["prefix"].ToString() == "TCURDATE/") || (dtSchema.Rows[0]["suffix"].ToString() == "/TCURDATE"))
                    {
                        sqlQuery = "SELECT max(tjv.MPS_No) FROM MPS_Details tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.Estimate_No))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.MPS_No))) = 1 and MPS_No like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.MPS_No) FROM MPS_Details tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.Estimate_No))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.MPS_No))) = 1 and MPS_No like '%" + sufxCompCode + "'";
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
                        sqlQuery = "SELECT max(tjv.MPS_No) FROM MPS_Details tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.Estimate_No))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.MPS_No))) = 1 and MPS_No like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.MPS_No) FROM MPS_Details tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.Estimate_No))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.MPS_No))) = 1 and MPS_No like '" + prefCompCode + "%'";
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
                }
                else
                {
                    sqlQuery = "SELECT MPS_No FROM MPS_Details WHERE MPS_No LIKE '" + manual_str.Trim() + "'";
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
        public ActionResult BatchEditingUpdateEstimateResources(DevExpress.Web.Mvc.MVCxGridViewBatchUpdateValues<EstimateResource, int> updateValues, MPSViewModel options)
        {
            Boolean IsProcess = false;
            List<EstimateResource> objList = new List<EstimateResource>();
            List<udtEstimateResources> udt = new List<udtEstimateResources>();
            try
            {
                //foreach (var product in updateValues.Insert)
                if (updateValues.Insert.Count > 0 && updateValues.Update.Count == 0)
                {
                    // if (updateValues.IsValid(product) && Convert.ToInt32(options.ProductionID) > 0 && Convert.ToInt32(options.DetailsID) > 0)
                    //{
                    ProductionID = Convert.ToInt32(options.ProductionID);
                    DetailsID = Convert.ToInt32(options.DetailsID);

                    List<udtEstmtEntryResources> udtlist = new List<udtEstmtEntryResources>();
                    udtEstmtEntryResources obj = null;
                    updateValues.Insert = updateValues.Insert.OrderBy(x => Convert.ToInt64(x.SlNO)).ToList();
                    foreach (var item in updateValues.Insert)
                    {
                        if (Convert.ToInt64(item.ProductId) > 0)
                        {
                            //if (Convert.ToDecimal(item.ProductQty) > 0)
                            //{
                            obj = new udtEstmtEntryResources();
                            obj.ProductID = Convert.ToInt64(item.ProductId);
                            obj.StkQty = Convert.ToDecimal(item.ProductQty);
                            obj.StkUOM = (item.ProductUOM == null ? "" : item.ProductUOM);
                            obj.WarehouseID = Convert.ToInt64(item.ProductsWarehouseID);
                            obj.Price = Convert.ToDecimal(item.Price);
                            obj.Amount = Convert.ToDecimal(item.Amount);
                            obj.Remarks = (item.Remarks);
                            obj.SlNo = (item.SlNO);
                            obj.ResourceCharges = Convert.ToDecimal(item.ResourceCharges);
                            obj.NetAmount = Convert.ToDecimal(item.NetAmount);
                            obj.BudgetedPrice = Convert.ToDecimal(item.BudgetedPrice);
                            obj.TaxTypeID = (item.TaxTypeID);
                            obj.Discount = Convert.ToDecimal(item.Discount);
                            obj.TaxType = (item.TaxType);
                            obj.AddlDesc = (item.AddlDesc);
                            udtlist.Add(obj);
                            // }
                        }
                    }
                    if (udtlist.Count > 0)
                    {
                        udtlist = udtlist.OrderBy(x => Convert.ToInt64(x.SlNo)).ToList();

                        foreach (var item in udtlist)
                        {
                            udtEstimateResources obj1 = new udtEstimateResources();
                            obj1.ProductID = Convert.ToInt64(item.ProductID);
                            obj1.StkQty = Convert.ToDecimal(item.StkQty);
                            obj1.StkUOM = (item.StkUOM);
                            obj1.WarehouseID = Convert.ToInt64(item.WarehouseID);
                            obj1.Price = Convert.ToDecimal(item.Price);
                            obj1.Amount = Convert.ToDecimal(item.Amount);
                            obj1.Remarks = (item.Remarks);
                            obj1.Charges = Convert.ToDecimal(item.ResourceCharges);
                            obj1.NetAmount = Convert.ToDecimal(item.NetAmount);
                            obj1.BudgetedPrice = Convert.ToDecimal(item.BudgetedPrice);
                            obj1.TaxTypeID = (item.TaxTypeID);
                            obj1.Discount = Convert.ToDecimal(item.Discount);
                            obj1.TaxType = (item.TaxType);
                            obj1.SrlNo = (item.SlNo);
                            obj1.AddlDesc = (item.AddlDesc);
                            udt.Add(obj1);
                        }

                        //IsProcess = EstimateResourcesInsertUpdate(udt, ProductionID, DetailsID);

                        TempData["Resource"] = udt;
                        TempData.Keep();
                    }

                    //}
                }


                if (updateValues.Update.Count > 0 && Convert.ToInt64(options.DetailsID) > 0)
                {
                    ProductionID = Convert.ToInt32(options.ProductionID);
                    DetailsID = Convert.ToInt32(options.DetailsID);

                    List<udtEstmtEntryResources> udtlist = new List<udtEstmtEntryResources>();
                    udtEstmtEntryResources obj = null;
                    foreach (var item in updateValues.Insert)
                    {
                        if (Convert.ToInt64(item.ProductId) > 0)
                        {
                            //if (Convert.ToDecimal(item.ProductQty) > 0)
                            //{
                            obj = new udtEstmtEntryResources();
                            obj.ProductID = Convert.ToInt64(item.ProductId);
                            obj.StkQty = Convert.ToDecimal(item.ProductQty);
                            obj.StkUOM = (item.ProductUOM == null ? "" : item.ProductUOM);
                            obj.WarehouseID = Convert.ToInt64(item.ProductsWarehouseID);
                            obj.Price = Convert.ToDecimal(item.Price);
                            obj.Amount = Convert.ToDecimal(item.Amount);
                            obj.Remarks = (item.Remarks);
                            obj.SlNo = (item.SlNO);
                            obj.ResourceCharges = Convert.ToDecimal(item.ResourceCharges);
                            obj.NetAmount = Convert.ToDecimal(item.NetAmount);
                            obj.BudgetedPrice = Convert.ToDecimal(item.BudgetedPrice);
                            obj.TaxTypeID = (item.TaxTypeID);
                            obj.Discount = Convert.ToDecimal(item.Discount);
                            obj.TaxType = (item.TaxType);
                            obj.AddlDesc = (item.AddlDesc);
                            udtlist.Add(obj);
                            //}
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
                            //if (Convert.ToDecimal(item.ProductQty) > 0)
                            //{
                            obj = new udtEstmtEntryResources();
                            obj.ProductID = Convert.ToInt64(item.ProductId);
                            obj.StkQty = Convert.ToDecimal(item.ProductQty);
                            obj.StkUOM = (item.ProductUOM == null ? "" : item.ProductUOM);
                            obj.WarehouseID = Convert.ToInt64(item.ProductsWarehouseID);
                            obj.Price = Convert.ToDecimal(item.Price);
                            obj.Amount = Convert.ToDecimal(item.Amount);
                            obj.Remarks = (item.Remarks);
                            obj.SlNo = (item.SlNO);
                            obj.ResourceCharges = Convert.ToDecimal(item.ResourceCharges);
                            obj.NetAmount = Convert.ToDecimal(item.NetAmount);
                            obj.BudgetedPrice = Convert.ToDecimal(item.BudgetedPrice);
                            obj.TaxTypeID = (item.TaxTypeID);
                            obj.Discount = Convert.ToDecimal(item.Discount);
                            obj.TaxType = (item.TaxType);
                            obj.AddlDesc = (item.AddlDesc);
                            udtlist.Add(obj);
                            // }
                        }
                    }

                    if (udtlist.Count > 0)
                    {
                        udtlist = udtlist.OrderBy(x => Convert.ToInt64(x.SlNo)).ToList();

                        foreach (var item in udtlist)
                        {
                            udtEstimateResources obj1 = new udtEstimateResources();
                            obj1.ProductID = Convert.ToInt64(item.ProductID);
                            obj1.StkQty = Convert.ToDecimal(item.StkQty);
                            obj1.StkUOM = (item.StkUOM);
                            obj1.WarehouseID = Convert.ToInt64(item.WarehouseID);
                            obj1.Price = Convert.ToDecimal(item.Price);
                            obj1.Amount = Convert.ToDecimal(item.Amount);
                            obj1.Remarks = (item.Remarks);
                            obj1.Charges = Convert.ToDecimal(item.ResourceCharges);
                            obj1.NetAmount = Convert.ToDecimal(item.NetAmount);
                            obj1.BudgetedPrice = Convert.ToDecimal(item.BudgetedPrice);
                            obj1.TaxTypeID = (item.TaxTypeID);
                            obj1.Discount = Convert.ToDecimal(item.Discount);
                            obj1.TaxType = (item.TaxType);
                            obj1.SrlNo = (item.SlNo);
                            obj1.AddlDesc = (item.AddlDesc);
                            udt.Add(obj1);
                        }

                        // IsProcess = EstimateResourcesInsertUpdate(udt, ProductionID, DetailsID);
                        TempData["Resource"] = udt;
                        TempData.Keep();
                    }
                }

            }
            catch { }

            return PartialView("~/Views/MPS/_BOMResourcesGrid.cshtml", objList);
        }

     
        public ActionResult MPSList()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/MPSList", "MPS");
            MPSViewModel obj = new MPSViewModel();
            string IsMultiuserApprovalRequired = cSOrder.GetSystemSettingsResult("IsMultiuserApprovalRequired");
            TempData.Clear();
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.IsMultiuserApprovalRequired = IsMultiuserApprovalRequired;
            return View("~/Views/MPS/MPSList.cshtml", obj);
        }

        public ActionResult GetMPSEntryList()
        {            
            List<MPSViewModel> list = new List<MPSViewModel>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/MPSList", "MPS");
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
                        dt = oDBEngine.GetDataTable("select * from V_MPSDetailsList where BRANCH_ID =" + BranchID + " AND (MPS_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "')  ORDER BY Details_ID DESC ");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("select * from V_MPSDetailsList where MPS_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "'  ORDER BY Details_ID DESC ");
                    }

                }
                

                TempData["EstimateDetailsListDataTable"] = dt;

                if (dt.Rows.Count > 0)
                {
                    MPSViewModel obj = new MPSViewModel();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new MPSViewModel();
                        obj.DetailsID = Convert.ToString(item["Details_ID"]);                     
                        obj.Estimate_SCHEMAID = Convert.ToString(item["MPS_SchemaID"]);
                        obj.EstimateNo = Convert.ToString(item["MPS_No"]);
                        obj.dtEstimateDate = Convert.ToDateTime(item["MPS_Date"]);                  

                        obj.Unit = Convert.ToString(item["BranchDescription"]);
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
                        obj.Customer = Convert.ToString(item["CUSTOMER_NAME"]);
                      
                        obj.OrderCode = Convert.ToString(item["Ordercode"]);
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
            return PartialView("~/Views/MPS/_BOMEntryDataList.cshtml", list);
        }

        public JsonResult RemoveEstimateDataByID(Int32 detailsid)
        {
            ReturnData obj = new ReturnData();
            //Boolean Success = false;
            //String Message = String.Empty;
            try
            {
                var datasetobj = objdata.DropDownDetailForEstimate("RemoveMPSData", null, null, null, 0, detailsid);
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

        public JsonResult SetEstimateDataByID(Int64 detailsid = 0, String Approve = "", String View = "")
        {
            Boolean Success = false;
            try
            {
                TempData["DetailsID"] = detailsid;
                TempData["Approve"] = Approve;
                TempData["View"] = View;
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

        public JsonResult SetEstimateDateFilter(Int64 unitid, string FromDate, string ToDate)
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

        public ActionResult ExportMPSGridList(int type)
        {
            ViewData["EstimateDetailsListDataTable"] = TempData["EstimateDetailsListDataTable"];

            TempData.Keep();
            DataTable dt = (DataTable)TempData["EstimateDetailsListDataTable"];
            if (ViewData["EstimateDetailsListDataTable"] != null && dt.Rows.Count > 0)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetEstimateGridView(ViewData["EstimateDetailsListDataTable"]), ViewData["EstimateDetailsListDataTable"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetEstimateGridView(ViewData["EstimateDetailsListDataTable"]), ViewData["EstimateDetailsListDataTable"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetEstimateGridView(ViewData["EstimateDetailsListDataTable"]), ViewData["EstimateDetailsListDataTable"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetEstimateGridView(ViewData["EstimateDetailsListDataTable"]), ViewData["EstimateDetailsListDataTable"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetEstimateGridView(ViewData["EstimateDetailsListDataTable"]), ViewData["EstimateDetailsListDataTable"]);
                    default:
                        break;
                }
                return null;
            }
            else
            {
                return this.RedirectToAction("MPSList", "MPS");
            }
        }

        private GridViewSettings GetEstimateGridView(object datatable)
        {
            //List<EmployeesTargetSetting> obj = (List<EmployeesTargetSetting>)datatablelist;
            //ListtoDataTable lsttodt = new ListtoDataTable();
            //DataTable datatable = ConvertListToDataTable(obj); 
            var settings = new GridViewSettings();
            settings.Name = "MPS";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "MPS";
            //String ID = Convert.ToString(TempData["EmployeesTargetListDataTable"]);
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "MPS_No" 
                    || datacolumn.ColumnName == "MPS_Date" || datacolumn.ColumnName == "CUSTOMER_NAME"
                    || datacolumn.ColumnName == "BranchDescription" || datacolumn.ColumnName == "CreatedBy" || datacolumn.ColumnName == "CreateDate" || datacolumn.ColumnName == "ModifyBy" || datacolumn.ColumnName == "ModifyDate")
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "MPS_No")
                        {
                            column.Caption = "Document No";
                            column.VisibleIndex = 0;
                        }
                        
                        else if (datacolumn.ColumnName == "MPS_Date")
                        {
                            column.Caption = "Document Date";
                            column.VisibleIndex = 2;

                        }
                       
                        
                        else if (datacolumn.ColumnName == "CUSTOMER_NAME")
                        {
                            column.Caption = "Customer";
                            column.VisibleIndex = 5;
                        }
                        else if (datacolumn.ColumnName == "BranchDescription")
                        {
                            column.Caption = "Unit";
                            column.VisibleIndex = 6;
                        }
                       
                       
                        else if (datacolumn.ColumnName == "CreatedBy")
                        {
                            column.Caption = "Entered By";
                            column.VisibleIndex = 10;
                        }
                        else if (datacolumn.ColumnName == "CreateDate")
                        {
                            column.Caption = "Entered On";
                            column.VisibleIndex = 11;
                        }
                        else if (datacolumn.ColumnName == "ModifyBy")
                        {
                            column.Caption = "Modified By";
                            column.VisibleIndex = 12;
                        }
                        else if (datacolumn.ColumnName == "ModifyDate")
                        {
                            column.Caption = "Modified On";
                            column.VisibleIndex = 13;
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
                            if (datacolumn.ColumnName == "ModifyDate")
                            {
                                column.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy HH:mm:ss";
                            }
                            else
                            {
                                column.PropertiesEdit.DisplayFormatString = "dd-MM-yyyy";
                            }
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
                var datasetobj = objdata.DropDownDetailForEstimate("RevisionNumberCheck", null, revisionno, null, 0, detailsid);
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

       

        public ActionResult GetContractCode(MPSViewModel model, String customer_id,String Branchs)
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
                DataTable dtContr = objdata.GetContractCode(model.Customer_ID, Branch);

                List<ContractList> modelContra = new List<ContractList>();
                modelContra = APIHelperMethods.ToModelList<ContractList>(dtContr);
               // ViewBag.ContractNo = Contract;
                return PartialView("~/Views/MPS/_PartialContractOrederList.cshtml", modelContra);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public JsonResult getAmountForRecord(String type = null)
        {
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            List<AmountFor> list = new List<AmountFor>();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

            DataSet dst = new DataSet();
            dst = objSlaesActivitiesBL.GetAllDropDownDetailForSalesChallan(strBranchID);
            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                if (dst.Tables[4] != null && dst.Tables[4].Rows.Count > 0)
                {
                    AmountFor obj = new AmountFor();
                    foreach (DataRow item in dst.Tables[4].Rows)
                    {
                        obj = new AmountFor();
                        obj.taxGrp_Id = Convert.ToString(item["taxGrp_Id"]);
                        obj.taxGrp_Description = Convert.ToString(item["taxGrp_Description"]);
                        list.Add(obj);
                    }
                }
            }
            return Json(list);
        }

        //[WebMethod(EnableSession = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public JsonResult GetTaxDetailsForSale()
        {
            List<TaxDetailsforEntry> returnList = new List<TaxDetailsforEntry>();
            TaxDetailsforEntry returnitem = new TaxDetailsforEntry();

            #region GetGstTaxSchemeByJson
            List<TaxSchemeItemLabel> taxSchemeItemLabelList = new List<TaxSchemeItemLabel>();

            ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
            proc.AddVarcharPara("@action", 50, "GetTaxData");
            proc.AddVarcharPara("@applicableFor", 5, "P");
            proc.AddVarcharPara("@cmp_internalid", 100, Convert.ToString(Session["LastCompany"]));
            DataSet DS = proc.GetDataSet();

            //returnitem.ItemLevelTaxDetails = JsonConvert.SerializeObject(DS.Tables[0]);

            taxSchemeItemLabelList = (from DataRow dr in DS.Tables[0].Rows
                                      select new TaxSchemeItemLabel()
                                      {
                                          TaxRates_ID = Convert.ToInt32(dr["TaxRates_ID"]),
                                          TaxRates_TaxCode = Convert.ToInt32(dr["TaxRates_TaxCode"]),
                                          TaxRatesSchemeName = Convert.ToString(dr["TaxRatesSchemeName"]),
                                          Taxes_Code = Convert.ToString(dr["Taxes_Code"]),
                                          Taxes_ApplicableOn = Convert.ToString(dr["Taxes_ApplicableOn"]),
                                          Taxes_ApplicableFor = Convert.ToString(dr["Taxes_ApplicableFor"]),
                                          TaxCalculateMethods = Convert.ToString(dr["TaxCalculateMethods"]),
                                          TaxRates_Rate = Convert.ToDouble(dr["TaxRates_Rate"])
                                      }).ToList();


            returnitem.ItemLevelTaxDetails = taxSchemeItemLabelList;

            #endregion

            #region GetTaxSchemebyHSN
            List<HSNListwithTaxes> hSNListwithTaxeslist = new List<HSNListwithTaxes>();
            HSNListwithTaxes hSNListwithTaxes;
            foreach (DataRow hsnrow in DS.Tables[2].Rows)
            {
                hSNListwithTaxes = new HSNListwithTaxes();
                hSNListwithTaxes.HSNCODE = Convert.ToString(hsnrow["HsnCode"]);
                DataRow[] taxes = DS.Tables[1].Select("HsnCode='" + hSNListwithTaxes.HSNCODE + "'");
                List<Config_TaxRatesID> config_TaxRatesIDlist = new List<Config_TaxRatesID>();

                if (taxes.Length > 0)
                {
                    Config_TaxRatesID config_TaxRatesID;
                    foreach (DataRow taxScehemCode in taxes)
                    {
                        config_TaxRatesIDlist.Add(new Config_TaxRatesID(Convert.ToInt32(taxScehemCode["TaxRates_ID"]), Convert.ToDecimal(taxScehemCode["TaxRates_Rate"]), Convert.ToString(taxScehemCode["Taxes_ApplicableOn"]), Convert.ToString(taxScehemCode["TaxTypeCode"])));
                    }
                }
                hSNListwithTaxes.config_TaxRatesIDs = config_TaxRatesIDlist;
                hSNListwithTaxeslist.Add(hSNListwithTaxes);
            }
            //   returnitem.HSNCodewiseTaxSchem = JsonConvert.SerializeObject(hSNListwithTaxeslist);
            returnitem.HSNCodewiseTaxSchem = hSNListwithTaxeslist;
            #endregion

            #region GetBranchWiseStateByJson

            List<BranchWiseState> ListBranchWiseState = new List<BranchWiseState>();

            if (DS.Tables[3] != null && DS.Tables[3].Rows.Count > 0)
            {
                foreach (DataRow dr in DS.Tables[3].Rows)
                {
                    BranchWiseState _ObjBranchWiseState = new BranchWiseState();
                    _ObjBranchWiseState.branch_id = Convert.ToInt32(dr["branch_id"]);
                    _ObjBranchWiseState.branch_state = Convert.ToInt32(dr["branch_state"]);
                    _ObjBranchWiseState.BranchGSTIN = Convert.ToString(dr["branch_GSTIN"]);
                    _ObjBranchWiseState.CompanyGSTIN = Convert.ToString(dr["CompGSTIN"]);
                    ListBranchWiseState.Add(_ObjBranchWiseState);
                }
            }
            // returnitem.BranchWiseStateTax = JsonConvert.SerializeObject(ListBranchWiseState);
            returnitem.BranchWiseStateTax = ListBranchWiseState;

            #endregion
            #region GetStateCodeWiseStateIDByJson

            List<StateCodeWiseStateID> ListStateCodeWiseStateID = new List<StateCodeWiseStateID>();

            if (DS.Tables[4] != null && DS.Tables[4].Rows.Count > 0)
            {
                foreach (DataRow dr in DS.Tables[4].Rows)
                {
                    StateCodeWiseStateID _ObjStateCodeWiseStateID = new StateCodeWiseStateID();
                    _ObjStateCodeWiseStateID.id = Convert.ToInt32(dr["id"]);
                    _ObjStateCodeWiseStateID.StateCode = Convert.ToString(dr["StateCode"]);

                    ListStateCodeWiseStateID.Add(_ObjStateCodeWiseStateID);
                }
            }
            //returnitem.StateCodeWiseStateIDTax = JsonConvert.SerializeObject(ListStateCodeWiseStateID);
            returnitem.StateCodeWiseStateIDTax = ListStateCodeWiseStateID;
            #endregion
            returnList.Add(returnitem);
            //  return returnList;


            var jsonResult = Json(returnList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }

        public JsonResult CalcelEstimateDataByID(Int32 detailsid, String Cancel_Remarks)
        {
            ReturnData obj = new ReturnData();
            try
            {
                var datasetobj = objdata.CancelReOpenForEstimate("CancelEstimateData", Cancel_Remarks, Convert.ToInt64(Session["userid"]), detailsid);
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

        public JsonResult ReOpenEstimateDataByID(Int32 detailsid, String Cancel_Remarks)
        {
            ReturnData obj = new ReturnData();
            try
            {
                var datasetobj = objdata.CancelReOpenForEstimate("ReOpenEstimateData", Cancel_Remarks, Convert.ToInt64(Session["userid"]), detailsid);
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

        public JsonResult ApproveEstimateDataByID(Int32 detailsid, String Approve_Remarks, String Action)
        {
            ReturnData obj = new ReturnData();
            try
            {
                var datasetobj = objdata.CancelReOpenForEstimate(Action, Approve_Remarks, Convert.ToInt64(Session["userid"]), detailsid);
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

        public JsonResult ProdAdditionalDesc(String AddlDesc, String ProdAddlDescSl, String Command)
        {
            ReturnData obj = new ReturnData();
            try
            {
                if (Command == "RemarksAdd")
                {
                    if (TempData["ProdAddlDesc"] != null)
                    {
                        prodAddlDesc = (DataTable)TempData["ProdAddlDesc"];
                    }
                    else
                    {
                        if (prodAddlDesc == null || prodAddlDesc.Rows.Count == 0)
                        {
                            prodAddlDesc.Columns.Add("SrlNo", typeof(string));
                            prodAddlDesc.Columns.Add("AdditionRemarks", typeof(string));
                        }
                    }

                    DataRow[] deletedRow = prodAddlDesc.Select("SrlNo='" + ProdAddlDescSl + "'");
                    if (deletedRow.Length > 0)
                    {
                        foreach (DataRow dr in deletedRow)
                        {
                            prodAddlDesc.Rows.Remove(dr);
                        }
                        prodAddlDesc.AcceptChanges();
                    }

                    prodAddlDesc.Rows.Add(ProdAddlDescSl, AddlDesc);
                    TempData["ProdAddlDesc"] = prodAddlDesc;
                    TempData.Keep();
                    obj.Success = Convert.ToBoolean(1);
                    obj.Message = Convert.ToString("Sucess");
                }
                else if (Command == "RemarksDisplay")
                {
                    DataTable Remarksdt = (DataTable)TempData["ProdAddlDesc"];
                    if (Remarksdt != null)
                    {
                        DataView dvData = new DataView(Remarksdt);
                        dvData.RowFilter = "SrlNo = '" + ProdAddlDescSl + "'";
                        if (dvData.Count > 0)
                        {
                            obj.Success = Convert.ToBoolean(1);
                            obj.Message = dvData[0]["AdditionRemarks"].ToString();
                        }
                    }
                    TempData.Keep();
                }
                else if (Command == "RemarksRemove")
                {
                    if (TempData["ProdAddlDesc"] != null)
                    {
                        prodAddlDesc = (DataTable)TempData["ProdAddlDesc"];
                        DataRow[] deletedRow = prodAddlDesc.Select("SrlNo=" + ProdAddlDescSl);
                        if (deletedRow.Length > 0)
                        {
                            foreach (DataRow dr in deletedRow)
                            {
                                prodAddlDesc.Rows.Remove(dr);
                            }
                            prodAddlDesc.AcceptChanges();
                        }
                        DataTable dtDel = new DataTable();
                        dtDel.Columns.Add("SrlNo", typeof(string));
                        dtDel.Columns.Add("AdditionRemarks", typeof(string));
                        int i = 0;
                        foreach (DataRow item in prodAddlDesc.Rows)
                        {
                            dtDel.Rows.Add((i + 1).ToString(), item["AdditionRemarks"].ToString());
                            i = i + 1;
                        }

                        TempData["ProdAddlDesc"] = dtDel;
                        TempData.Keep();
                        dtDel = null;
                    }
                    obj.Success = Convert.ToBoolean(1);
                    obj.Message = Convert.ToString("Sucess");
                }
            }
            catch { }
            return Json(obj);
        }

        public JsonResult ResAdditionalDesc(String AddlDesc, String ResAddlDescSl, String Command)
        {
            ReturnData obj = new ReturnData();
            try
            {
                if (Command == "RemarksAdd")
                {
                    if (TempData["ResAddlDesc"] != null)
                    {
                        ResAddlDesc = (DataTable)TempData["ResAddlDesc"];
                    }
                    else
                    {
                        if (ResAddlDesc == null || ResAddlDesc.Rows.Count == 0)
                        {
                            ResAddlDesc.Columns.Add("SrlNo", typeof(string));
                            ResAddlDesc.Columns.Add("AdditionRemarks", typeof(string));
                        }
                    }

                    DataRow[] deletedRow = ResAddlDesc.Select("SrlNo='" + ResAddlDescSl + "'");
                    if (deletedRow.Length > 0)
                    {
                        foreach (DataRow dr in deletedRow)
                        {
                            ResAddlDesc.Rows.Remove(dr);
                        }
                        ResAddlDesc.AcceptChanges();
                    }

                    ResAddlDesc.Rows.Add(ResAddlDescSl, AddlDesc);
                    TempData["ResAddlDesc"] = ResAddlDesc;
                    TempData.Keep();
                    obj.Success = Convert.ToBoolean(1);
                    obj.Message = Convert.ToString("Sucess");
                }
                else if (Command == "RemarksDisplay")
                {
                    DataTable Remarksdt = (DataTable)TempData["ResAddlDesc"];
                    if (Remarksdt != null)
                    {
                        DataView dvData = new DataView(Remarksdt);
                        dvData.RowFilter = "SrlNo = '" + ResAddlDescSl + "'";
                        if (dvData.Count > 0)
                        {
                            obj.Success = Convert.ToBoolean(1);
                            obj.Message = dvData[0]["AdditionRemarks"].ToString();
                        }
                    }
                    TempData.Keep();
                }
                else if (Command == "RemarksRemove")
                {
                    if (TempData["ResAddlDesc"] != null)
                    {
                        ResAddlDesc = (DataTable)TempData["ResAddlDesc"];
                        DataRow[] deletedRow = ResAddlDesc.Select("SrlNo='" + ResAddlDescSl + "'");
                        if (deletedRow.Length > 0)
                        {
                            foreach (DataRow dr in deletedRow)
                            {
                                ResAddlDesc.Rows.Remove(dr);
                            }
                            ResAddlDesc.AcceptChanges();
                        }

                        DataTable dtDel = new DataTable();
                        dtDel.Columns.Add("SrlNo", typeof(string));
                        dtDel.Columns.Add("AdditionRemarks", typeof(string));
                        int i = 0;
                        foreach (DataRow item in ResAddlDesc.Rows)
                        {
                            dtDel.Rows.Add((i + 1).ToString(), item["AdditionRemarks"].ToString());
                            i = i + 1;
                        }

                        TempData["ResAddlDesc"] = dtDel;
                        TempData.Keep();
                    }
                    obj.Success = Convert.ToBoolean(1);
                    obj.Message = Convert.ToString("Sucess");
                }
            }
            catch { }
            return Json(obj);
        }

        //Tanmoy Hierarchy
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
        //Tanmoy Hierarchy End

        //Tanmoy Product using Datatable
        [WebMethod]
        public JsonResult AddProduct(EstimateProduct prod)
        {
            DataTable dt = (DataTable)TempData["ProductsDetails"];
            DataTable dt2 = new DataTable();

            if (dt == null)
            {
                DataTable dtable = new DataTable();

                dtable.Clear();
                dtable.Columns.Add("HIddenID", typeof(System.Guid));
                dtable.Columns.Add("SlNO", typeof(System.String));              
               
                dtable.Columns.Add("ProductName", typeof(System.String));
                dtable.Columns.Add("ProductId", typeof(System.String));
                dtable.Columns.Add("ProductDescription", typeof(System.String));
                dtable.Columns.Add("ProductQty", typeof(System.String));
                dtable.Columns.Add("ProductUOM", typeof(System.String));         
              
                dtable.Columns.Add("Price", typeof(System.String));
                dtable.Columns.Add("Amount", typeof(System.String));
                dtable.Columns.Add("Remarks", typeof(System.String));
                dtable.Columns.Add("UpdateEdit", typeof(System.String));               
                dtable.Columns.Add("UOMID", typeof(System.String));
                dtable.Columns.Add("BOMID", typeof(System.String));
                dtable.Columns.Add("BOMNO", typeof(System.String));

                object[] trow = { Guid.NewGuid(), 1,prod.ProductName,prod.ProductId,prod.ProductDescription,prod.ProductQty,prod.ProductUOM,prod.Price,prod.Amount,prod.Remarks,prod.UpdateEdit,prod.UOMID,prod.BOMID,prod.BOMNO
                                    };
                dtable.Rows.Add(trow);
                TempData["ProductsDetails"] = dtable;
                TempData.Keep();
            }
            else
            {
                if (string.IsNullOrEmpty(prod.Guids))
                {
                    //object[] trow = { Guid.NewGuid(), Convert.ToInt32(dt.Rows.Count)+1,prod.Details_ID,prod.ProductName,prod.ProductId,prod.ProductDescription,prod.ProductQty,prod.ProductUOM,prod.Price,prod.Amount,prod.Remarks,prod.UpdateEdit,prod.UOMID
                    //                 };// Add new parameter Here
                    object[] trow = { Guid.NewGuid(), Convert.ToInt32(dt.Rows.Count)+1,prod.ProductName,prod.ProductId,prod.ProductDescription,prod.ProductQty,prod.ProductUOM,prod.Price,prod.Amount,prod.Remarks,prod.UpdateEdit,prod.UOMID,prod.BOMID,prod.BOMNO
                                    };
                    dt.Rows.Add(trow);
                    TempData["ProductsDetails"] = dt;
                    TempData.Keep();
                }
                else
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            if (prod.Guids.ToString() == item["HIddenID"].ToString())
                            {                                
                                item["ProductName"] = prod.ProductName;
                                item["ProductId"] = prod.ProductId;
                                item["ProductDescription"] = prod.ProductDescription;
                                item["ProductQty"] = prod.ProductQty;
                                item["ProductUOM"] = prod.ProductUOM;                               
                                item["Price"] = prod.Price;
                                item["Amount"] = prod.Amount;
                                item["Remarks"] = prod.Remarks;
                                item["UpdateEdit"] = "1";                              
                                item["UOMID"] = prod.UOMID;
                                item["BOMID"] = prod.BOMID;
                                item["BOMNO"] = prod.BOMNO;

                            }
                        }
                    }
                }
                TempData["ProductsDetails"] = dt;
                TempData.Keep();
            }


            return Json("");
        }

        [WebMethod]
        public JsonResult DeleteData(string HiddenID)
        {
            DataTable dt = (DataTable)TempData["ProductsDetails"];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
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
                    dr["SlNO"] = conut;
                    conut++;
                }
            }

            TempData["ProductsDetails"] = dt;
            TempData.Keep();
            return Json("Device Remove Successfully.");
        }

        [WebMethod]
        public JsonResult EditData(String HiddenID)
        {
            EstimateProduct ret = new EstimateProduct();

            DataTable dt = (DataTable)TempData["ProductsDetails"];

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (HiddenID.ToString() == item["HIddenID"].ToString())
                    {
                        ret.SlNO = item["SlNO"].ToString();
                        ret.ProductName = item["ProductName"].ToString();
                        ret.ProductId = item["ProductId"].ToString();
                        ret.Guids = item["HIddenID"].ToString();
                        ret.ProductDescription = item["ProductDescription"].ToString();
                        ret.ProductQty = item["ProductQty"].ToString();
                        ret.ProductUOM = item["ProductUOM"].ToString();                      
                        ret.Price = item["Price"].ToString();
                        ret.Amount = item["Amount"].ToString();
                        ret.Remarks = item["Remarks"].ToString();
                        ret.UOMID = item["UOMID"].ToString();
                        ret.BOMID = item["BOMID"].ToString();
                        ret.BOMNO = item["BOMNO"].ToString();

                        ViewBag.ParentBOMID = Convert.ToString(item["BOMID"]);
                        break;

                    }
                }
            }
            TempData["ProductsDetails"] = dt;
            TempData.Keep();
            return Json(ret);
        }

      
       

       

        

        //End

        //Estimate save with data table
        [WebMethod]
        public JsonResult SaveEstimate(MPSViewModel Details)
        {
            String NumberScheme = "";
            String Message = "";
            bool Success = false;
            //string ContrtNo = "";
            //int j = 1;

            //if (Details.ContractNo != null && Details.ContractNo.Count > 0)
            //{
            //    foreach (string item in Details.ContractNo)
            //    {
            //        if (j > 1)
            //            ContrtNo = ContrtNo + "," + item;
            //        else
            //            ContrtNo = item;
            //        j++;
            //    }
            //}

            

            DataTable dtEstimate_Resource = new DataTable();
            dtEstimate_Resource.Columns.Add("ProductID");
            dtEstimate_Resource.Columns.Add("Qty");
            dtEstimate_Resource.Columns.Add("UOM_Name");           
            dtEstimate_Resource.Columns.Add("Price");
            dtEstimate_Resource.Columns.Add("Amount");
            dtEstimate_Resource.Columns.Add("Remarks");           
            dtEstimate_Resource.Columns.Add("SrlNo");        
            dtEstimate_Resource.Columns.Add("UOMID");
            DataTable dt_PRODUCTS = (DataTable)TempData["ProductsDetails"];      

            List<udtEstimateProduct> udt = new List<udtEstimateProduct>();

            foreach (DataRow item in dt_PRODUCTS.Rows)
            {
                udtEstimateProduct obj1 = new udtEstimateProduct();
                obj1.ProductID = Convert.ToInt64(item["ProductID"]);
                obj1.StkQty = Convert.ToDecimal(item["ProductQty"]);
                obj1.StkUOM = (item["ProductUOM"].ToString());              
                obj1.Price = Convert.ToDecimal(item["Price"]);
                obj1.Amount = Convert.ToDecimal(item["Amount"]);              
                obj1.Remarks = (item["Remarks"].ToString());              
                obj1.SrlNo = (item["SlNo"].ToString());             
                obj1.UOMID = (item["UOMID"].ToString());

                obj1.BOMID = (item["BOMID"].ToString());
                obj1.BOMNO = (item["BOMNO"].ToString());
                udt.Add(obj1);
            }


            DataTable dtEstimate_PRODUCTS = new DataTable();
            dtEstimate_PRODUCTS = ToDataTable(udt);          

            if (!String.IsNullOrEmpty(Details.strEstimateNo))
            {
                JVNumStr = Details.strEstimateNo;
            }

            NumberScheme = checkNMakeEstimateCode(Details.strEstimateNo, Convert.ToInt32(Details.Estimate_SCHEMAID), Convert.ToDateTime(Details.RevisionDate));



            DataSet dt = new DataSet();
            if (Convert.ToInt64(Details.DetailsID) > 0)
            {
                if (!String.IsNullOrEmpty(Details.strEstimateNo))
                {
                    JVNumStr = Details.strEstimateNo;
                }
                dt = objdata.MPSProductEntryInsertUpdate("UPDATEMAINPRODUCT", JVNumStr, Convert.ToDateTime(Details.EstimateDate), Convert.ToInt32(Details.Unit),
                    dtEstimate_PRODUCTS, Convert.ToInt32(Details.Estimate_SCHEMAID)
                   , Details.Customer_ID, Details.OrderID,
                    Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), Convert.ToInt64(Session["userid"]), Convert.ToInt64(Details.DetailsID)
                      , Convert.ToString(Details.EstimateStartDate_dt), Convert.ToString(Details.EstimateEndDate_dt), Convert.ToString(Details.ActualsStartDate_dt), Convert.ToString(Details.ActualsEndDate_dt));
            }
            else
            {
                if (NumberScheme == "ok")
                {
                    dt = objdata.MPSProductEntryInsertUpdate("INSERTMAINPRODUCT", JVNumStr, Convert.ToDateTime(Details.EstimateDate), Convert.ToInt32(Details.Unit)
                        , dtEstimate_PRODUCTS, Convert.ToInt32(Details.Estimate_SCHEMAID),
                         Details.Customer_ID, Details.OrderID,
                         Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), Convert.ToInt64(Session["userid"]),0
                         , Convert.ToString(Details.EstimateStartDate_dt), Convert.ToString(Details.EstimateEndDate_dt), Convert.ToString(Details.ActualsStartDate_dt), Convert.ToString(Details.ActualsEndDate_dt)
                         );
                }
                else
                {
                    Message = NumberScheme;
                }
            }

            if (dt != null && dt.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dt.Tables[0].Rows)
                {
                    Success = Convert.ToBoolean(row["Success"]);
                   // ProductionID = Convert.ToInt32(row["ProductionID"]);
                    DetailsID = Convert.ToInt32(row["DetailsID"]);
                }
            }


           // ViewData["ProductionID"] = ProductionID;
            ViewData["DetailsID"] = DetailsID;
            ViewData["EstimateNo"] = JVNumStr;
            ViewData["Success"] = Success;
            ViewData["Message"] = Message;
            String retuenMsg = Success + "~" + DetailsID  + "~" + JVNumStr + "~" + Message;
            return Json(retuenMsg);
        }

        //End

        public JsonResult GetEstimateDesignerList(Int64 ID = 0)
        {
            List<DesignList> list = new List<DesignList>();
            try
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\EstimateCosting\DocDesign\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\EstimateCosting\DocDesign\";
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

        [WebMethod]
        public JsonResult EstimateApproval(String detailsid)
        {
            Boolean Success = false;
            try
            {
                DataTable dtContr = objdata.EstimateApproval(detailsid, Convert.ToString(Session["userid"]));
                if (dtContr != null && dtContr.Rows.Count > 0)
                {
                    Success = true;
                }

            }
            catch { }
            return Json(Success);
        }

        public ActionResult GetParentBOM(BomRelationshipViewModel model, string ParentBOMID, String Branchs)
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
                String ProductID = model.ProductId;

                List<MRPBOMList> modelParentBOM = new List<MRPBOMList>();
                //Rev 1.0
                //DataTable ParentBOMdt = objdata.GetParentBOM(Branch);
                DataTable ParentBOMdt = objdata.GetParentBOM(Branch, ProductID);
                //Rev 1.0 End
                if (ParentBOMdt != null && ParentBOMdt.Rows.Count > 0)
                {
                    modelParentBOM = APIHelperMethods.ToModelList<MRPBOMList>(ParentBOMdt);
                    ViewBag.ParentBOMID = ParentBOMID;
                }

                return PartialView("~/Views/MPS/_PartialParentBOM.cshtml", modelParentBOM);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }
        public class MRPBOMList
        {
            public Int64 Details_ID { get; set; }
            public string BOM_No { get; set; }
            public string ProductsName { get; set; }
            public string REVNo { get; set; }

        }
	}
}