using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using PMS.Models;
using PMS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using UtilityLayer;

namespace PMS.Controllers
{
    public class BillofQuantitiesController : Controller
    {
        BillofQuantitiesVM objBOQ = null;
        BillofQuantitiesModel objdata = null;
        DBEngine oDBEngine = new DBEngine();
        string JVNumStr = string.Empty;
        Int32 ProductionID = 0;
        Int32 DetailsID = 0;
        DataTable prodAddlDesc = new DataTable();
        DataTable ResAddlDesc = new DataTable();
        //Int64 GlobalDetailsID = 0;
        UserRightsForPage rights = new UserRightsForPage();
        CommonBL cSOrder = new CommonBL();

        public BillofQuantitiesController()
        {
            objBOQ = new BillofQuantitiesVM();
            objdata = new BillofQuantitiesModel();
        }
        public ActionResult Index(Int64 DetailsID = 0)
        {
            try
            {
                string ApprovalRevisionRequireInBOQ = cSOrder.GetSystemSettingsResult("ApprovalRevisionRequireInBOQ");
                string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
                string ProjectMandatoryInEntry = cSOrder.GetSystemSettingsResult("ProjectMandatoryInEntry");
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/BillofQuantitiesList", "BillofQuantities");
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

                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "BillofQuantities");
                    List<HierarchyList> objHierarchy = new List<HierarchyList>();

                    List<BranchUnit> list = new List<BranchUnit>();
                    var datasetobj = objdata.DropDownDetailForBOQ("GetUnitDropDownData", Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["userbranchHierarchy"]), 0, 0);
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
                    objBOQ.UnitList = list;

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
                    objBOQ.Hierarchy_List = objHierarchy;

                    TempData["ProdAddlDesc"] = null;
                    TempData["ResAddlDesc"] = null;

                    if (TempData["DetailsID"] != null)
                    {
                        objBOQ.DetailsID = Convert.ToString(TempData["DetailsID"]);
                        objBOQ.Approve = Convert.ToString(TempData["Approve"]);
                        ViewBag.View = Convert.ToString(TempData["View"]);
                        TempData.Keep();

                        if (Convert.ToInt64(objBOQ.DetailsID) > 0)
                        {
                            DataTable objData = objdata.GetBOQProductEntryListByID("GetBOQEntryDetailsData", Convert.ToInt64(objBOQ.DetailsID));
                            if (objData != null && objData.Rows.Count > 0)
                            {
                                DataTable dt = objData;


                                foreach (DataRow row in dt.Rows)
                                {
                                    objBOQ.DetailsID = Convert.ToString(row["Details_ID"]);
                                    objBOQ.ProductionID = Convert.ToString(row["Production_ID"]);
                                    objBOQ.BOQ_SCHEMAID = Convert.ToString(row["BOQ_SchemaID"]);
                                    objBOQ.BOQNo = Convert.ToString(row["BOQ_No"]);
                                    objBOQ.BOQDate = Convert.ToString(row["BOQ_Date"]);
                                    objBOQ.RevisionNo = Convert.ToString(row["REV_No"]);
                                    objBOQ.RevisionDate = Convert.ToString(row["REV_Date"]);
                                    objBOQ.Unit = Convert.ToString(row["BRANCH_ID"]);
                                    objBOQ.ActualAdditionalCost = Convert.ToString(row["ActualAdditionalCost"]);
                                    objBOQ.ActualComponentCost = Convert.ToString(row["ActualComponentCost"]);
                                    objBOQ.ActualProductCost = Convert.ToString(row["ActualProductCost"]);

                                    objBOQ.dtBOQDate = Convert.ToDateTime(row["BOQ_Date"]);
                                    if (Convert.ToString(row["REV_No"]) == " ")
                                    {
                                        objBOQ.strREVDate = Convert.ToDateTime(row["BOQ_Date"]).ToShortDateString();
                                        objBOQ.dtREVDate = Convert.ToDateTime(row["BOQ_Date"]);
                                        ViewBag.RevDate = Convert.ToString(row["BOQ_Date"]);
                                    }
                                    else
                                    {
                                        objBOQ.strREVDate = Convert.ToDateTime(row["REV_Date"]).ToShortDateString();
                                        objBOQ.dtREVDate = Convert.ToDateTime(row["REV_Date"]);
                                        ViewBag.RevDate = Convert.ToString(row["REV_Date"]);
                                    }

                                    objBOQ.ProductionOrderQty = Convert.ToString(row["ProductionOrderQty"]);
                                    objBOQ.FGReceiptQty = Convert.ToString(row["FGReceiptQty"]);
                                    objBOQ.HeadRemarks = Convert.ToString(row["HeadRemarks"]);

                                    objBOQ.Proposal = Convert.ToString(row["Proposal"]);
                                    objBOQ.Quotation = Convert.ToString(row["Quotation"]);
                                    objBOQ.Proposal_ID = Convert.ToString(row["Proposal_ID"]);
                                    objBOQ.Quotation_ID = Convert.ToString(row["Quotation_ID"]);
                                    objBOQ.Customer_ID = Convert.ToString(row["CUSTOMER_ID"]);
                                    objBOQ.Customer = Convert.ToString(row["CUSTOMER_NAME"]);

                                    objBOQ.ProjectID = Convert.ToString(row["ProjectID"]);
                                    //objBOQ.ContractNo = Convert.ToString(row["ContractNo"]);
                                    objBOQ.TaxID = Convert.ToString(row["TaxID"]);
                                    objBOQ.Proj_Code = Convert.ToString(row["Proj_Code"]);
                                    objBOQ.ApproveRemarks = Convert.ToString(row["ApproveRemarks"]);
                                    if (Convert.ToString(row["ApproveStus"]) == "1")
                                    {
                                        objBOQ.APPROVE_NAME = "Already Approved " + Convert.ToString(row["APPROVE_NAME"]);
                                    }

                                    ViewBag.ProjectID = Convert.ToString(row["ProjectID"]);
                                    ViewBag.Customer_id = Convert.ToString(row["CUSTOMER_ID"]);
                                    ViewBag.ContractNo = Convert.ToString(row["ContractNo"]);
                                    ViewBag.ApproveStus = Convert.ToString(row["ApproveStus"]);
                                    ViewBag.Unit = Convert.ToString(row["BRANCH_ID"]);
                                    ViewBag.Estimateid = Convert.ToString(row["EstimateID"]);
                                    ViewBag.Proposal_ID = Convert.ToString(row["Proposal_ID"]);
                                    ViewBag.BOQDate = Convert.ToString(row["BOQ_Date"]);
                                    if (Convert.ToString(TempData["Approve"]) == "Approve")
                                    {
                                        ViewBag.ApproveStusEdit = "";
                                    }
                                    else
                                    {
                                        ViewBag.ApproveStusEdit = Convert.ToString(row["ApproveStus"]);
                                    }
                                }
                            }
                        }
                    }

                }
                catch { }


                objBOQ.RevisionDate = DateTime.Now.ToString();
                objBOQ.BOQDate = DateTime.Now.ToString();
                objBOQ.ApprovRevSettings = ApprovalRevisionRequireInBOQ;
                objBOQ.TaxID = "1";
                TempData["Count"] = 1;
                TempData.Keep();

                ViewBag.CanView = rights.CanView;
                ViewBag.CanAdd = rights.CanAdd;
                ViewBag.CanEdit = rights.CanEdit;
                ViewBag.CanDelete = rights.CanDelete;
                ViewBag.CanCancel = rights.CanCancel;
                ViewBag.CanApproved = rights.CanApproved;
                ViewBag.ProjectShow = ProjectSelectInEntryModule;
                ViewBag.ProjectMandatoryInEntry = ProjectMandatoryInEntry;
                ViewBag.CanAddUpdateDocuments = rights.CanAddUpdateDocuments;

                if (ApprovalRevisionRequireInBOQ == "No")
                {
                    ViewBag.CanApproved = false;
                }
                ViewBag.ApprovalRevisionRequire = ApprovalRevisionRequireInBOQ;

                return View("~/Views/PMS/BillofQuantities/Index.cshtml", objBOQ);

            }
            catch
            {
                return RedirectToAction("login", "PMSLogin", new { area = "" });
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
            String strType = "114";
            if (!String.IsNullOrEmpty(type))
            {
                if (type.ToLower() == "production")
                {
                    strType = "94";
                }
                if (type.ToLower() == "sales")
                {
                    strType = "95";
                }
                if (type.ToLower() == "assembly")
                {
                    strType = "96";
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

        public ActionResult GetBOQResources(BillofQuantitiesVM model, String IsTaggingModule)
        {
            List<BillofQuantitiesResource> objList = new List<BillofQuantitiesResource>();
            BillofQuantitiesResource dataobj = new BillofQuantitiesResource();
            Int64 DetailsID = 0;
            DataTable objData = null;
            if (TempData["DetailsID"] != null)
            {
                DetailsID = Convert.ToInt64(TempData["DetailsID"]);
                TempData["DetailsID"] = null;

            }

            if (DetailsID > 0)
            {
                objData = objdata.GetBOQProductEntryListByID("GetBOQEntryResourcesData", DetailsID);
            }
            if (model.TagingID != null)
            {
                String ids = "";

                int i = 1;

                if (model.TagingID != null && model.TagingID.Count > 0)
                {
                    foreach (string item in model.TagingID)
                    {
                        if (i > 1)
                            ids = ids + "," + item;
                        else
                            ids = item;
                        i++;
                    }
                }
                if (IsTaggingModule=="Estimate")
                {
                    objData = objdata.GetBOQProductTagListByID("GetTagEntryResourcesData", ids);
                }
                else if (IsTaggingModule=="Proposal")
                {
                     objData = objdata.GetBOQProductTagListByID("GetTagProposalEntryResourcesData", ids);
                }
            }
            if (objData != null && objData.Rows.Count > 0)
            {
                TempData["ResAddlDesc"] = null;
                if (ResAddlDesc == null || ResAddlDesc.Rows.Count == 0)
                {
                    ResAddlDesc.Columns.Add("SrlNo", typeof(string));
                    ResAddlDesc.Columns.Add("AdditionRemarks", typeof(string));
                }

                DataTable dt = objData;
                foreach (DataRow row in dt.Rows)
                {
                    dataobj = new BillofQuantitiesResource();
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
                    dataobj.ResourceCharges = Convert.ToString(row["Charges"]);

                    dataobj.NetAmount = Convert.ToString(row["NetAmount"]);
                    dataobj.BudgetedPrice = Convert.ToString(row["BudgetedPrice"]);
                    dataobj.TaxTypeID = Convert.ToString(row["TaxTypeID"]);
                    dataobj.Discount = Convert.ToString(row["Discount"]);
                    dataobj.TaxType = Convert.ToString(row["TaxType"]);
                    dataobj.AddlDesc = "";
                    dataobj.EstBalID = Convert.ToString(row["EstBalID"]);
                    dataobj.BalQty = Convert.ToString(row["BalQty"]);

                    ResAddlDesc.Rows.Add(Convert.ToString(row["SlNO"]), Convert.ToString(row["ADDITIONAL_REMARKS"]));

                    objList.Add(dataobj);
                }
                TempData["ResUpdateDate"] = objList;
                TempData["ResAddlDesc"] = ResAddlDesc;
                TempData.Keep();

                ViewData["BOQResourcesTotalAm"] = objList.Sum(x => Convert.ToDecimal(x.NetAmount)).ToString();
            }

            return PartialView("~/Views/PMS/BillofQuantities/_PartialBOQResourceGrid.cshtml", objList);
        }

        public ActionResult GetBOQProductEntryList(BillofQuantitiesVM model, String IsTaggingModule)
        {
            BillofQuantitiesProduct BOQproductdataobj = new BillofQuantitiesProduct();
            List<BillofQuantitiesProduct> BOQproductdata = new List<BillofQuantitiesProduct>();
            Int64 DetailsID = 0;
            try
            {
                DataTable objData = null;
                if (TempData["DetailsID"] != null)
                {
                    DetailsID = Convert.ToInt64(TempData["DetailsID"]);
                    TempData.Keep();
                }

                if (DetailsID > 0)
                {
                    objData = objdata.GetBOQProductEntryListByID("GetBOQEntryProductsData", DetailsID);
                }

                if (model.TagingID != null)
                {
                    String ids = "";

                    int i = 1;

                    if (model.TagingID != null && model.TagingID.Count > 0)
                    {
                        foreach (string item in model.TagingID)
                        {
                            if (i > 1)
                                ids = ids + "," + item;
                            else
                                ids = item;
                            i++;
                        }
                    }
                    if (IsTaggingModule=="Estimate")
                    {
                        objData = objdata.GetBOQProductTagListByID("GetBOQTagProductsData", ids);
                    }
                    else if (IsTaggingModule=="Proposal")
                    {
                        objData = objdata.GetBOQProductTagListByID("GetBOQProposalTagProductsData", ids);
                    }
                    
                }
                if (objData != null && objData.Rows.Count > 0)
                {
                    DataTable dt = objData;
                    TempData["ProdAddlDesc"] = null;
                    if (prodAddlDesc == null || prodAddlDesc.Rows.Count == 0)
                    {
                        prodAddlDesc.Columns.Add("SrlNo", typeof(string));
                        prodAddlDesc.Columns.Add("AdditionRemarks", typeof(string));
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        BOQproductdataobj = new BillofQuantitiesProduct();
                        BOQproductdataobj.SlNO = Convert.ToString(row["SlNO"]);
                        BOQproductdataobj.ProductName = Convert.ToString(row["sProducts_Name"]);
                        BOQproductdataobj.ProductId = Convert.ToString(row["ProductID"]);
                        BOQproductdataobj.ProductDescription = Convert.ToString(row["sProducts_Description"]);
                        BOQproductdataobj.ProductQty = Convert.ToString(row["StkQty"]);
                        BOQproductdataobj.ProductUOM = Convert.ToString(row["StkUOM"]);
                        BOQproductdataobj.Warehouse = Convert.ToString(row["WarehouseName"]);
                        BOQproductdataobj.Price = Convert.ToString(row["Price"]);
                        BOQproductdataobj.Amount = Convert.ToString(row["Amount"]);
                        // BOQproductdataobj.BOQNo = Convert.ToString(row["BOQNo"]);
                        //BOQproductdataobj.RevNo = Convert.ToString(row["RevNo"]);
                        //if (!String.IsNullOrEmpty(Convert.ToString(row["RevDate"])))
                        //{
                        //    BOQproductdataobj.RevDate = Convert.ToDateTime(row["RevDate"]).ToString("dd-MM-yyyy");
                        //}
                        //else
                        //{
                        //    BOQproductdataobj.RevDate = " ";
                        //}
                        BOQproductdataobj.Remarks = Convert.ToString(row["Remarks"]);
                        BOQproductdataobj.ProductsWarehouseID = Convert.ToString(row["WarehouseID"]);
                        BOQproductdataobj.Tag_Details_ID = Convert.ToString(row["Tag_Details_ID"]);
                        BOQproductdataobj.Tag_Production_ID = Convert.ToString(row["Tag_Production_ID"]);
                        BOQproductdataobj.RevNo = Convert.ToString(row["RevNo"]);
                        BOQproductdataobj.Charges = Convert.ToString(row["Charges"]);
                        BOQproductdataobj.Discount = Convert.ToString(row["Discount"]);

                        BOQproductdataobj.NetAmount = Convert.ToString(row["NetAmount"]);
                        BOQproductdataobj.BudgetedPrice = Convert.ToString(row["BudgetedPrice"]);
                        BOQproductdataobj.TaxTypeID = Convert.ToString(row["TaxTypeID"]);
                        BOQproductdataobj.TaxType = Convert.ToString(row["TaxType"]);
                        BOQproductdataobj.AddlDesc = "";
                        BOQproductdataobj.EstBalID = Convert.ToString(row["EstBalID"]);
                        BOQproductdataobj.BalQty = Convert.ToString(row["BalQty"]);
                        prodAddlDesc.Rows.Add(Convert.ToString(row["SlNO"]), Convert.ToString(row["ADDITIONAL_REMARKS"]));

                        BOQproductdata.Add(BOQproductdataobj);


                    }
                    TempData["ProdUpdateDate"] = BOQproductdata;
                    TempData["ProdAddlDesc"] = prodAddlDesc;
                    TempData.Keep();

                    ViewData["BOQEntryProductsTotalAm"] = BOQproductdata.Sum(x => Convert.ToDecimal(x.NetAmount)).ToString();
                }
            }

            catch { }
            return PartialView("~/Views/PMS/BillofQuantities/_PartialBOQProductGrid.cshtml", BOQproductdata);
        }

        [ValidateInput(false)]
        public ActionResult BatchEditingUpdateBOQProductEntry(DevExpress.Web.Mvc.MVCxGridViewBatchUpdateValues<BillofQuantitiesProduct, int> updateValues, BillofQuantitiesVM options)
        {
            TempData["Count"] = (int)TempData["Count"] + 1;
            TempData.Keep();
            String NumberScheme = "";
            String Message = "";
            Int64 SaveDataArea = 0;

            List<udtBillofQuantitiesProduct> udt = new List<udtBillofQuantitiesProduct>();

            if ((int)TempData["Count"] != 2)
            {
                Boolean IsProcess = false;
                List<BillofQuantitiesProduct> list = new List<BillofQuantitiesProduct>();
                //foreach (var product in updateValues.Insert)

                //if (TempData["ProdUpdateDate"] == null)
                //{
                if (updateValues.Insert.Count > 0 && Convert.ToInt64(options.DetailsID) < 1 && Convert.ToString(options.IsTagging) != "true")
                {
                    //if (updateValues.IsValid(product))
                    //{
                    List<udtBillofQuantitiesProducts> udtlist = new List<udtBillofQuantitiesProducts>();
                    udtBillofQuantitiesProducts obj = null;
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
                            if (String.IsNullOrEmpty(item.Tag_Production_ID))
                            {
                                item.Tag_Production_ID = "0";
                            }
                            if (String.IsNullOrEmpty(item.Tag_Details_ID))
                            {
                                item.Tag_Details_ID = "0";
                            }

                            obj = new udtBillofQuantitiesProducts();
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
                            obj.Charges = (item.Charges);
                            obj.Discount = (item.Discount);
                            obj.NetAmount = (item.NetAmount);
                            obj.BudgetedPrice = (item.BudgetedPrice);
                            obj.TaxTypeID = (item.TaxTypeID);
                            obj.TaxType = (item.TaxType);
                            obj.EstBalID = (item.EstBalID);
                            udtlist.Add(obj);
                            //}
                        }
                    }
                    if (udtlist.Count > 0)
                    {
                        SaveDataArea = 1;
                        //if (options.BOMNo)
                        NumberScheme = checkNMakeBOQCode(options.strBOQNo, Convert.ToInt32(options.BOQ_SCHEMAID), Convert.ToDateTime(options.RevisionDate));
                        if (NumberScheme == "ok")
                        {
                            udtlist = udtlist.OrderBy(x => Convert.ToInt64(x.SlNo)).ToList();
                            foreach (var item in udtlist)
                            {
                                udtBillofQuantitiesProduct obj1 = new udtBillofQuantitiesProduct();
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
                                obj1.Charges = (item.Charges);
                                obj1.Discount = (item.Discount);
                                obj1.NetAmount = (item.NetAmount);
                                obj1.BudgetedPrice = (item.BudgetedPrice);
                                obj1.TaxTypeID = (item.TaxTypeID);
                                obj1.TaxType = (item.TaxType);
                                obj1.SrlNo = (item.SlNo);
                                obj1.EstBalID = (item.EstBalID);
                                udt.Add(obj1);
                            }
                            IsProcess = BOQProductInsertUpdate(udt, options);
                        }
                        else
                        {
                            Message = NumberScheme;
                        }
                    }
                    // list.Add(product);
                    //}
                }
                //}

                if (((updateValues.Update.Count > 0 && Convert.ToInt64(options.DetailsID) > 0 && Convert.ToString(options.IsTagging) == "false") || (updateValues.Insert.Count > 0 && Convert.ToInt64(options.DetailsID) < 1 && Convert.ToString(options.IsTagging) == "false")) && SaveDataArea == 0)
                {
                    List<udtBillofQuantitiesProducts> udtlist = new List<udtBillofQuantitiesProducts>();
                    udtBillofQuantitiesProducts obj = null;

                    foreach (var item in updateValues.Update)
                    {
                        if (Convert.ToInt64(item.ProductId) > 0)
                        {
                            //if (Convert.ToDecimal(item.ProductQty) > 0)
                            //{
                            obj = new udtBillofQuantitiesProducts();
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
                            obj.Charges = (item.Charges);
                            obj.Discount = (item.Discount);
                            obj.NetAmount = (item.NetAmount);
                            obj.BudgetedPrice = (item.BudgetedPrice);
                            obj.TaxTypeID = (item.TaxTypeID);
                            obj.TaxType = (item.TaxType);
                            obj.EstBalID = (item.EstBalID);
                            udtlist.Add(obj);
                            // }
                        }
                    }

                    foreach (var item in updateValues.Insert)
                    {
                        if (Convert.ToInt64(item.ProductId) > 0)
                        {
                            //if (Convert.ToDecimal(item.ProductQty) > 0)
                            //{
                            obj = new udtBillofQuantitiesProducts();
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
                            obj.Charges = (item.Charges);
                            obj.Discount = (item.Discount);
                            obj.NetAmount = (item.NetAmount);
                            obj.BudgetedPrice = (item.BudgetedPrice);
                            obj.TaxTypeID = (item.TaxTypeID);
                            obj.TaxType = (item.TaxType);
                            obj.EstBalID = (item.EstBalID);
                            udtlist.Add(obj);
                            // }
                        }
                    }

                    //if (updateValues.Insert.Count > 0 && Convert.ToInt64(options.DetailsID) > 0)
                    //{
                    //    foreach (var item in updateValues.Insert)
                    //    {
                    //        if (Convert.ToInt64(item.ProductId) > 0)
                    //        {
                    //            //if (!String.IsNullOrEmpty(item.BOMNo) && !String.IsNullOrEmpty(item.RevNo))
                    //            //{
                    //            //    DataSet dt = new DataSet();
                    //            //     dt = objdata.BOMProductEntryInsertUpdate()
                    //            //}
                    //            if (String.IsNullOrEmpty(item.Tag_Production_ID))
                    //            {
                    //                item.Tag_Production_ID = "0";
                    //            }
                    //            if (String.IsNullOrEmpty(item.Tag_Details_ID))
                    //            {
                    //                item.Tag_Details_ID = "0";
                    //            }

                    //            obj = new udtEntryProducts();
                    //            obj.ProductID = Convert.ToInt64(item.ProductId);
                    //            obj.StkQty = Convert.ToDecimal(item.ProductQty);
                    //            obj.StkUOM = (item.ProductUOM);
                    //            obj.IssuesQty = Convert.ToDecimal(0);
                    //            obj.IssuesUOM = (" ");
                    //            obj.WarehouseID = Convert.ToInt64(item.ProductsWarehouseID);
                    //            obj.Price = Convert.ToDecimal(item.Price);
                    //            obj.Amount = Convert.ToDecimal(item.Amount);
                    //            obj.Tag_Details_ID = Convert.ToInt64(item.Tag_Details_ID);
                    //            obj.Tag_Production_ID = Convert.ToInt64(item.Tag_Production_ID);
                    //            obj.Tag_REV_No = item.RevNo;
                    //            obj.Remarks = (item.Remarks);
                    //            obj.SlNo = (item.SlNO);

                    //            udtlist.Add(obj);
                    //        }
                    //    }
                    //}

                    if (udtlist.Count > 0)
                    {
                        SaveDataArea = 1;

                        //if (JVNumStr != "" && Convert.ToInt64(options.DetailsID) < 1)
                        //{
                        //    //if (options.BOMNo)
                        //    NumberScheme = checkNMakeEstimateCode(options.strBOMNo, Convert.ToInt32(options.BOM_SCHEMAID), Convert.ToDateTime(options.RevisionDate));
                        //    if (NumberScheme == "ok")
                        //    {
                        //        IsProcess = EstimateProductInsertUpdate(udtlist, options);
                        //    }
                        //    else
                        //    {
                        //        Message = NumberScheme;
                        //    }
                        //}

                        //if (options.BOMNo)
                        // NumberScheme = checkNMakeEstimateCode(options.strBOMNo, Convert.ToInt32(options.BOM_SCHEMAID), Convert.ToDateTime(options.RevisionDate));
                        udtlist = udtlist.OrderBy(x => Convert.ToInt64(x.SlNo)).ToList();
                        foreach (var item in udtlist)
                        {
                            udtBillofQuantitiesProduct obj1 = new udtBillofQuantitiesProduct();
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
                            obj1.Charges = (item.Charges);
                            obj1.Discount = (item.Discount);
                            obj1.NetAmount = (item.NetAmount);
                            obj1.BudgetedPrice = (item.BudgetedPrice);
                            obj1.TaxTypeID = (item.TaxTypeID);
                            obj1.TaxType = (item.TaxType);
                            obj1.SrlNo = (item.SlNo);
                            obj1.EstBalID = (item.EstBalID);
                            udt.Add(obj1);
                        }

                        IsProcess = BOQProductInsertUpdate(udt, options);
                    }
                }


                if (((updateValues.Update.Count > 0 && Convert.ToString(options.IsTagging) == "true") || (updateValues.Insert.Count > 0 && Convert.ToString(options.IsTagging) == "true")) && SaveDataArea == 0)
                {
                    List<udtBillofQuantitiesProducts> udtlist = new List<udtBillofQuantitiesProducts>();
                    udtBillofQuantitiesProducts obj = null;

                    foreach (var item in updateValues.Update)
                    {
                        if (Convert.ToInt64(item.ProductId) > 0)
                        {
                            //if (Convert.ToDecimal(item.ProductQty) > 0)
                            //{
                            obj = new udtBillofQuantitiesProducts();
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
                            obj.Charges = (item.Charges);
                            obj.Discount = (item.Discount);
                            obj.NetAmount = (item.NetAmount);
                            obj.BudgetedPrice = (item.BudgetedPrice);
                            obj.TaxTypeID = (item.TaxTypeID);
                            obj.TaxType = (item.TaxType);
                            obj.EstBalID = (item.EstBalID);
                            udtlist.Add(obj);
                            // }
                        }
                    }

                    foreach (var item in updateValues.Insert)
                    {
                        if (Convert.ToInt64(item.ProductId) > 0)
                        {
                            //if (Convert.ToDecimal(item.ProductQty) > 0)
                            //{
                            obj = new udtBillofQuantitiesProducts();
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
                            obj.Charges = (item.Charges);
                            obj.Discount = (item.Discount);
                            obj.NetAmount = (item.NetAmount);
                            obj.BudgetedPrice = (item.BudgetedPrice);
                            obj.TaxTypeID = (item.TaxTypeID);
                            obj.TaxType = (item.TaxType);
                            obj.EstBalID = (item.EstBalID);
                            udtlist.Add(obj);
                            // }
                        }
                    }

                    NumberScheme = checkNMakeBOQCode(options.strBOQNo, Convert.ToInt32(options.BOQ_SCHEMAID), Convert.ToDateTime(options.RevisionDate));
                    if (NumberScheme == "ok")
                    {
                        if (udtlist.Count > 0)
                        {
                            SaveDataArea = 1;
                            udtlist = udtlist.OrderBy(x => Convert.ToInt64(x.SlNo)).ToList();
                            foreach (var item in udtlist)
                            {
                                udtBillofQuantitiesProduct obj1 = new udtBillofQuantitiesProduct();
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
                                obj1.Charges = (item.Charges);
                                obj1.Discount = (item.Discount);
                                obj1.NetAmount = (item.NetAmount);
                                obj1.BudgetedPrice = (item.BudgetedPrice);
                                obj1.TaxTypeID = (item.TaxTypeID);
                                obj1.TaxType = (item.TaxType);
                                obj1.SrlNo = (item.SlNo);
                                obj1.EstBalID = (item.EstBalID);
                                udt.Add(obj1);
                            }
                            IsProcess = BOQProductInsertUpdate(udt, options);
                        }
                    }
                }


                TempData["Count"] = 1;
                TempData.Keep();
                ViewData["ProductionID"] = ProductionID;
                ViewData["DetailsID"] = DetailsID;
                ViewData["BOQNo"] = JVNumStr;
                ViewData["Success"] = IsProcess;
                ViewData["Message"] = Message;
            }
            return PartialView("~/Views/PMS/BillofQuantities/_PartialBOQProductGrid.cshtml", updateValues.Update);
            //return Json(IsProcess, JsonRequestBehavior.AllowGet);
        }

        public Boolean BOQProductInsertUpdate(List<udtBillofQuantitiesProduct> obj, BillofQuantitiesVM obj2)
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

                DataTable dtBOQ_PRODUCTS = new DataTable();
                dtBOQ_PRODUCTS = ToDataTable(obj);

                DataTable dtBOQ_Resource = new DataTable();
                if (TempData["Resource"] != null)
                {
                    List<udtBillofQuantitiesResources> obj1 = (List<udtBillofQuantitiesResources>)TempData["Resource"];
                    dtBOQ_Resource = ToDataTable(obj1);
                    TempData.Keep();
                }
                else
                {
                    dtBOQ_Resource.Columns.Add("ProductID");
                    dtBOQ_Resource.Columns.Add("StkQty");
                    dtBOQ_Resource.Columns.Add("StkUOM");
                    dtBOQ_Resource.Columns.Add("WarehouseID");
                    dtBOQ_Resource.Columns.Add("Price");
                    dtBOQ_Resource.Columns.Add("Amount");
                    dtBOQ_Resource.Columns.Add("Remarks");
                    dtBOQ_Resource.Columns.Add("Charges");
                    dtBOQ_Resource.Columns.Add("NetAmount");
                    dtBOQ_Resource.Columns.Add("BudgetedPrice");
                    dtBOQ_Resource.Columns.Add("TaxTypeID");
                    dtBOQ_Resource.Columns.Add("Discount");
                    dtBOQ_Resource.Columns.Add("TaxType");
                    dtBOQ_Resource.Columns.Add("SrlNo");
                    dtBOQ_Resource.Columns.Add("EstBalID");
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
                    if (!String.IsNullOrEmpty(obj2.strBOQNo))
                    {
                        JVNumStr = obj2.strBOQNo;
                    }
                    dt = objdata.BOQProductEntryInsertUpdate("UPDATEMAINPRODUCT", JVNumStr, Convert.ToDateTime(obj2.BOQDate), obj2.RevisionNo, Convert.ToDateTime(obj2.RevisionDate), Convert.ToInt32(obj2.Unit),
                        dtBOQ_PRODUCTS, dtBOQ_Resource, Convert.ToInt32(obj2.BOQ_SCHEMAID), Convert.ToDecimal(obj2.ActualAdditionalCost), obj2.Proposal_ID, obj2.Quotation_ID, obj2.HeadRemarks,
                        obj2.Customer_ID, ContrtNo, obj2.ProjectID, obj2.TaxID, obj2.Approve, obj2.ApprvRejct, obj2.ApproveRemarks,obj2.ApprovRevSettings, dtProductAddlDesc, dtResourceAddlDesc, obj2.EstimateID,obj2.TaggingModuleSave, Convert.ToInt64(Session["userid"]), 0, Convert.ToInt64(obj2.DetailsID));
                }
                else
                {
                    dt = objdata.BOQProductEntryInsertUpdate("INSERTMAINPRODUCT", JVNumStr, Convert.ToDateTime(obj2.BOQDate), obj2.RevisionNo, Convert.ToDateTime(obj2.RevisionDate), Convert.ToInt32(obj2.Unit)
                        , dtBOQ_PRODUCTS, dtBOQ_Resource, Convert.ToInt32(obj2.BOQ_SCHEMAID), Convert.ToDecimal(obj2.ActualAdditionalCost), obj2.Proposal_ID, obj2.Quotation_ID, obj2.HeadRemarks,
                         obj2.Customer_ID, ContrtNo, obj2.ProjectID, obj2.TaxID, obj2.Approve, obj2.ApprvRejct, obj2.ApproveRemarks, obj2.ApprovRevSettings, dtProductAddlDesc, dtResourceAddlDesc, obj2.EstimateID, obj2.TaggingModuleSave, Convert.ToInt64(Session["userid"]));
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

        public string checkNMakeBOQCode(string manual_str, int sel_schema_Id, DateTime RevisionDate)
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
                        sqlQuery = "SELECT max(tjv.BOQ_No) FROM PMS_BOQProduction tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.BOQ_No))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.BOQ_No))) = 1 and BOQ_No like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.BOQ_No) FROM PMS_BOQProduction tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.BOQ_No))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.BOQ_No))) = 1 and BOQ_No like '%" + sufxCompCode + "'";
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
                        sqlQuery = "SELECT max(tjv.BOQ_No) FROM PMS_BOQProduction tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.BOQ_No))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.BOQ_No))) = 1 and BOQ_No like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.BOQ_No) FROM PMS_BOQProduction tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.BOQ_No))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.BOQ_No))) = 1 and BOQ_No like '" + prefCompCode + "%'";
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
                    sqlQuery = "SELECT BOQ_No FROM PMS_BOQProduction WHERE BOQ_No LIKE '" + manual_str.Trim() + "'";
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
        public ActionResult BatchEditingUpdateBOQResources(DevExpress.Web.Mvc.MVCxGridViewBatchUpdateValues<BillofQuantitiesResource, int> updateValues, BillofQuantitiesVM options)
        {
            Boolean IsProcess = false;
            List<BillofQuantitiesResource> objList = new List<BillofQuantitiesResource>();
            List<udtBillofQuantitiesResources> udt = new List<udtBillofQuantitiesResources>();
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
                            obj.EstBalID = (item.EstBalID);

                            udtlist.Add(obj);
                            // }
                        }
                    }
                    if (udtlist.Count > 0)
                    {
                        udtlist = udtlist.OrderBy(x => Convert.ToInt64(x.SlNo)).ToList();

                        foreach (var item in udtlist)
                        {
                            udtBillofQuantitiesResources obj1 = new udtBillofQuantitiesResources();
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
                            obj1.EstBalID = (item.EstBalID);

                            udt.Add(obj1);
                        }

                        //IsProcess = EstimateResourcesInsertUpdate(udt, ProductionID, DetailsID);

                        TempData["Resource"] = udt;
                        TempData.Keep();
                    }

                    //}
                }


                if (updateValues.Update.Count > 0 && Convert.ToInt64(options.DetailsID) > 0 && options.IsTagging != "true")
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
                            obj.EstBalID = (item.EstBalID);

                            udtlist.Add(obj);
                            //}
                        }
                    }


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
                            obj.EstBalID = (item.EstBalID);

                            udtlist.Add(obj);
                            // }
                        }
                    }

                    if (udtlist.Count > 0)
                    {
                        udtlist = udtlist.OrderBy(x => Convert.ToInt64(x.SlNo)).ToList();

                        foreach (var item in udtlist)
                        {
                            udtBillofQuantitiesResources obj1 = new udtBillofQuantitiesResources();
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
                            obj1.EstBalID = (item.EstBalID);

                            udt.Add(obj1);
                        }

                        // IsProcess = EstimateResourcesInsertUpdate(udt, ProductionID, DetailsID);
                        TempData["Resource"] = udt;
                        TempData.Keep();
                    }
                }


                if (updateValues.Update.Count > 0 && Convert.ToString(options.IsTagging) == "true")
                {
                    //ProductionID = Convert.ToInt32(options.ProductionID);
                    //DetailsID = Convert.ToInt32(options.DetailsID);

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
                            obj.EstBalID = (item.EstBalID);

                            udtlist.Add(obj);
                            //}
                        }
                    }

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
                            obj.EstBalID = (item.EstBalID);

                            udtlist.Add(obj);
                            // }
                        }
                    }

                    if (udtlist.Count > 0)
                    {
                        udtlist = udtlist.OrderBy(x => Convert.ToInt64(x.SlNo)).ToList();

                        foreach (var item in udtlist)
                        {
                            udtBillofQuantitiesResources obj1 = new udtBillofQuantitiesResources();
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
                            obj1.EstBalID = (item.EstBalID);

                            udt.Add(obj1);
                        }

                        // IsProcess = EstimateResourcesInsertUpdate(udt, ProductionID, DetailsID);
                        TempData["Resource"] = udt;
                        TempData.Keep();
                    }
                }



            }
            catch { }

            return PartialView("~/Views/PMS/BillofQuantities/_PartialBOQResourceGrid.cshtml", objList);
        }

        public Boolean BOQResourcesInsertUpdate(List<udtBillofQuantitiesResources> obj, Int32 ProductionID, Int32 DetailsID)
        {
            Boolean Success = false;
            try
            {
                DataSet dt = new DataSet();
                dt = objdata.BOQProductEntryInsertUpdate("INSERTRESOURCES", JVNumStr, DateTime.Now, "", DateTime.Now, 0
                    , new DataTable(), ToDataTable(obj), 0, 0, "", "", "", "", "0", "0", "", "", "", "","", new DataTable(), new DataTable(), "","", Convert.ToInt64(Session["userid"]), ProductionID, DetailsID);
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

        public ActionResult BillofQuantitiesList()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/BillofQuantitiesList", "BillofQuantities");
            BillofQuantitiesVM obj = new BillofQuantitiesVM();
            TempData.Clear();
            string ApprovalRevisionRequireInBOQ = cSOrder.GetSystemSettingsResult("ApprovalRevisionRequireInBOQ");
            ViewBag.ApprovRevSettings=ApprovalRevisionRequireInBOQ;
            ViewBag.CanAdd = rights.CanAdd;
            return View("~/Views/PMS/BillofQuantities/BillofQuantitiesList.cshtml", obj);
        }

        public ActionResult GetBOQEntryList()
        {
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");

            List<BillofQuantitiesVM> list = new List<BillofQuantitiesVM>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/BillofQuantitiesList", "BillofQuantities");
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
                        dt = oDBEngine.GetDataTable("select * from V_PMSBOQOrderDetailsList where BRANCH_ID =" + BranchID + " AND (BOQ_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "')  ORDER BY Details_ID DESC ");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("select * from V_PMSBOQOrderDetailsList where BOQ_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "'  ORDER BY Details_ID DESC ");
                    }

                }
                //else
                //{
                //    dt = oDBEngine.GetDataTable("select * from V_QuotationDetailsList");
                //}

                TempData["BOQDetailsListDataTable"] = dt;

                if (dt.Rows.Count > 0)
                {
                    BillofQuantitiesVM obj = new BillofQuantitiesVM();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new BillofQuantitiesVM();
                        obj.DetailsID = Convert.ToString(item["Details_ID"]);
                        obj.ProductionID = Convert.ToString(item["Production_ID"]);
                        obj.BOQ_SCHEMAID = Convert.ToString(item["BOQ_SchemaID"]);
                        obj.BOQNo = Convert.ToString(item["BOQ_No"]);
                        obj.dtBOQDate = Convert.ToDateTime(item["BOQ_Date"]);
                        obj.RevisionNo = Convert.ToString(item["REV_No"]);
                        if (Convert.ToString(item["REV_Date"]) != "")
                        {
                            obj.RevisionDate = Convert.ToDateTime(item["REV_Date"]).ToString("dd-MM-yyyy");
                        }
                        else
                        {
                            obj.RevisionDate = "";
                        }

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
                        obj.Cancel = Convert.ToString(item["Cancel"]);
                        obj.ReOpen = Convert.ToString(item["ReOpen"]);

                        obj.EstStatus = Convert.ToString(item["EstStatus"]);
                        obj.EstCalcen = Convert.ToString(item["EstCalcen"]);
                        obj.Proj_Code = Convert.ToString(item["Proj_Code"]);
                        //obj.ModifyDate = Convert.ToDateTime(item["ModifyDate"]);
                        obj.Reject = Convert.ToString(item["EDITVALUE"]);
                        obj.OrderCode = Convert.ToString(item["Ordercode"]);

                        if (Convert.ToString(item["Estimate_Date"]) != "")
                        {
                            obj.Estimate_Date = Convert.ToDateTime(item["Estimate_Date"]).ToString("dd-MM-yyyy");
                        }
                        else
                        {
                            obj.Estimate_Date = "";
                        }

                        if (Convert.ToString(item["ProposalDate"]) != "")
                        {
                            obj.ProposalDate = Convert.ToDateTime(item["ProposalDate"]).ToString("dd-MM-yyyy");
                        }
                        else
                        {
                            obj.ProposalDate = "";
                        }
                        obj.Estimate_No = Convert.ToString(item["Estimate_No"]);
                        obj.Proposal = Convert.ToString(item["Proposal"]);

                        list.Add(obj);
                    }
                }
            }
            catch { }
            string ApprovalRevisionRequireInBOQ = cSOrder.GetSystemSettingsResult("ApprovalRevisionRequireInBOQ");
            ViewBag.ApprovRevSettings = ApprovalRevisionRequireInBOQ;
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanAddUpdateDocuments = rights.CanAddUpdateDocuments;
            ViewBag.CanCancel = rights.CanCancel;
            ViewBag.CanApproved = rights.CanApproved;
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            ViewBag.CanAddUpdateDocuments = rights.CanAddUpdateDocuments;
            if (ApprovalRevisionRequireInBOQ == "No")
            {
                ViewBag.CanApproved = false;
            }
            return PartialView("~/Views/PMS/BillofQuantities/BillofQuantitiesEntryListGrid.cshtml", list);
        }

        public JsonResult RemoveBOQDataByID(Int32 detailsid)
        {
            ReturnData obj = new ReturnData();
            //Boolean Success = false;
            //String Message = String.Empty;
            try
            {
                var datasetobj = objdata.DropDownDetailForBOQ("RemoveBOQData", null, null, null, 0, detailsid);
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

        public JsonResult SetBOQDataByID(Int64 detailsid = 0, String Approve = "", String View = "")
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

        public JsonResult SetBOQDateFilter(Int64 unitid, string FromDate, string ToDate)
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

        public ActionResult ExportBOQGridList(int type)
        {
            ViewData["BOQDetailsListDataTable"] = TempData["BOQDetailsListDataTable"];

            TempData.Keep();
            DataTable dt = (DataTable)TempData["BOQDetailsListDataTable"];
            if (ViewData["BOQDetailsListDataTable"] != null && dt.Rows.Count > 0)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetBOQGridView(ViewData["BOQDetailsListDataTable"]), ViewData["BOQDetailsListDataTable"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetBOQGridView(ViewData["BOQDetailsListDataTable"]), ViewData["BOQDetailsListDataTable"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetBOQGridView(ViewData["BOQDetailsListDataTable"]), ViewData["BOQDetailsListDataTable"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetBOQGridView(ViewData["BOQDetailsListDataTable"]), ViewData["BOQDetailsListDataTable"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetBOQGridView(ViewData["BOQDetailsListDataTable"]), ViewData["BOQDetailsListDataTable"]);
                    default:
                        break;
                }
                return null;
            }
            else
            {
                return this.RedirectToAction("BillofQuantitiesList", "BillofQuantities");
            }
        }

        private GridViewSettings GetBOQGridView(object datatable)
        {
            //List<EmployeesTargetSetting> obj = (List<EmployeesTargetSetting>)datatablelist;
            //ListtoDataTable lsttodt = new ListtoDataTable();
            //DataTable datatable = ConvertListToDataTable(obj); 
            var settings = new GridViewSettings();
            settings.Name = "BillofQuantities";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "BillofQuantities";
            //String ID = Convert.ToString(TempData["EmployeesTargetListDataTable"]);
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "BOQ_No" || datacolumn.ColumnName == "BOQ_Type" || datacolumn.ColumnName == "EstCalcen" || datacolumn.ColumnName == "Proj_Code" || datacolumn.ColumnName == "EstStatus"
                    || datacolumn.ColumnName == "BOQ_Date" || datacolumn.ColumnName == "REV_No" || datacolumn.ColumnName == "REV_Date" || datacolumn.ColumnName == "CUSTOMER_NAME"
                    || datacolumn.ColumnName == "BranchDescription" || datacolumn.ColumnName == "CreatedBy" || datacolumn.ColumnName == "CreateDate" || datacolumn.ColumnName == "ModifyBy" || datacolumn.ColumnName == "ModifyDate")
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "BOQ_No")
                        {
                            column.Caption = "Document No";
                            column.VisibleIndex = 0;
                        }
                        else if (datacolumn.ColumnName == "BOQ_Type")
                        {
                            column.Caption = "Document Type";
                            column.VisibleIndex = 1;
                        }
                        else if (datacolumn.ColumnName == "BOQ_Date")
                        {
                            column.Caption = "Document Date";
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
                        else if (datacolumn.ColumnName == "Proj_Code")
                        {
                            column.Caption = "Project Name";
                            column.VisibleIndex = 7;
                        }
                        else if (datacolumn.ColumnName == "EstStatus")
                        {
                            column.Caption = "Status";
                            column.VisibleIndex = 8;
                        }
                        else if (datacolumn.ColumnName == "EstCalcen")
                        {
                            column.Caption = "Cancel";
                            column.VisibleIndex = 9;
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
                var datasetobj = objdata.DropDownDetailForBOQ("RevisionNumberCheck", null, revisionno, null, 0, detailsid);
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

        public ActionResult GetProjectCode(BillofQuantitiesVM model, string Project_ID, string Customer_id, String Branchs, string TagProject_ID, String Hierarchy)
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
                DataTable dtProj = objdata.GetProjectCode(Customer_id, Branch);

                List<ProjectList> modelProj = new List<ProjectList>();
                modelProj = APIHelperMethods.ToModelList<ProjectList>(dtProj);
                ViewBag.ProjectID = Project_ID;
                ViewBag.ProjectTagID = TagProject_ID;
                ViewBag.Hierarchy = Hierarchy;

                return PartialView("~/Views/PMS/BillofQuantities/_PartialProjectCode.cshtml", modelProj);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public ActionResult GetContractCode(BillofQuantitiesVM model, String customer_id, String Contract, String Branchs, String ProjectID)
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
                DataTable dtContr = objdata.GetContractCode(model.Customer_ID, Branch, ProjectID);

                List<ContractList> modelContra = new List<ContractList>();
                modelContra = APIHelperMethods.ToModelList<ContractList>(dtContr);
                ViewBag.ContractNo = Contract;
                ViewBag.ApprovRevSettings = model.ApprovRevSettings;
                return PartialView("~/Views/PMS/BillofQuantities/_PartialContractOrederList.cshtml", modelContra);
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

        public JsonResult GetTaxDetailsForSale()
        {
            List<TaxDetailsforEntry> returnList = new List<TaxDetailsforEntry>();
            TaxDetailsforEntry returnitem = new TaxDetailsforEntry();

            #region GetGstTaxSchemeByJson
            List<TaxSchemeItemLabel> taxSchemeItemLabelList = new List<TaxSchemeItemLabel>();

            ProcedureExecute proc = new ProcedureExecute("prc_GstTaxDetails");
            proc.AddVarcharPara("@action", 50, "GetTaxData");
            proc.AddVarcharPara("@applicableFor", 5, "S");
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

        public JsonResult CalcelBOQDataByID(Int32 detailsid, String Cancel_Remarks)
        {
            ReturnData obj = new ReturnData();
            try
            {
                var datasetobj = objdata.CancelReOpenForBOQ("CancelBOQData", Cancel_Remarks, Convert.ToInt64(Session["userid"]), detailsid);
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

        public JsonResult ReOpenBOQDataByID(Int32 detailsid, String Cancel_Remarks)
        {
            ReturnData obj = new ReturnData();
            try
            {
                var datasetobj = objdata.CancelReOpenForBOQ("ReOpenBOQData", Cancel_Remarks, Convert.ToInt64(Session["userid"]), detailsid);
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

        public JsonResult ApproveBOQDataByID(Int32 detailsid, String Approve_Remarks, String Action)
        {
            ReturnData obj = new ReturnData();
            try
            {
                var datasetobj = objdata.CancelReOpenForBOQ(Action, Approve_Remarks, Convert.ToInt64(Session["userid"]), detailsid);
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
                        TempData["ProdAddlDesc"] = prodAddlDesc;
                        TempData.Keep();
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
                        TempData["ResAddlDesc"] = ResAddlDesc;
                        TempData.Keep();
                    }
                    obj.Success = Convert.ToBoolean(1);
                    obj.Message = Convert.ToString("Sucess");
                }
            }
            catch { }
            return Json(obj);
        }

        public ActionResult GetEstimateCode(BillofQuantitiesVM model, String ESTIMATE, String Customer_id, String Branchs)
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

                String BOQdT = DateTime.Now.ToString("yyyy-MM-dd");
                if (model.BOQDate != null)
                {
                    BOQdT = model.BOQDate;
                }

                DataTable dtProj = null;
                dtProj = objdata.GetEstimateCode(Customer_id, Branch, ESTIMATE, Convert.ToDateTime(BOQdT));

                List<EstimateList> modelProj = new List<EstimateList>();
                modelProj = APIHelperMethods.ToModelList<EstimateList>(dtProj);
                ViewBag.Estimateid = ESTIMATE;
                ViewBag.ApprovRevSettings = model.ApprovRevSettings;

                return PartialView("~/Views/PMS/BillofQuantities/_PartialEstimateList.cshtml", modelProj);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public ActionResult DoActivity(String Module_Name, String Module_id, String Estimate_code)
        {
            //crmActivity crmAct = new crmActivity();
            //DataSet ddVal = crmAct.GetAllDropdown();

            //List<Contact_Type> Contact_Type = APIHelperMethods.ToModelList<Contact_Type>(ddVal.Tables[5]);
            //List<Activity> Activity = APIHelperMethods.ToModelList<Activity>(ddVal.Tables[0]);

            //Lead_ActivityType act = new Lead_ActivityType();
            //act.Lead_ActivityTypeName = "Select";
            //act.Id = 0;
            //List<Lead_ActivityType> ActivityType = new List<Lead_ActivityType>();
            //ActivityType.Add(act);
            //List<AssignTo> AssignTo = APIHelperMethods.ToModelList<AssignTo>(ddVal.Tables[4]);
            //List<Duration> Duration = APIHelperMethods.ToModelList<Duration>(ddVal.Tables[3]);
            //List<Prioritys> Priority = APIHelperMethods.ToModelList<Prioritys>(ddVal.Tables[2]);





            //crmAct.Contact_Type = Contact_Type;
            //crmAct.Activity = Activity;
            //crmAct.ActivityType = ActivityType;
            //crmAct.AssignTo = AssignTo;
            //crmAct.Duration = Duration;
            //crmAct.Priority = Priority;





            //ViewBag.cActivity_Date = null;
            //ViewBag.ddlContactType = "";
            //ViewBag.cbtnEntity = "";
            //ViewBag.cddlActivity = "";
            //ViewBag.cddlActivityType = "";
            //ViewBag.ctxt_Subject = "";
            //ViewBag.cmemo_Details = "";
            //ViewBag.AssignTo = "";
            //ViewBag.Duration = "";
            //ViewBag.ddlPriority = "";
            //ViewBag.cDue_dt = null;



            //DataSet output = crmAct.EditCRMActivity(Module_Name, Module_id);

            //if (output != null && output.Tables[0] != null && output.Tables[0].Rows.Count > 0)
            //{
            //    ViewBag.cActivity_Date = Convert.ToDateTime(output.Tables[0].Rows[0]["ActivityDate"]);
            //    ViewBag.ddlContactType = Convert.ToString(output.Tables[0].Rows[0]["ContactType"]);
            //    ViewBag.cbtnEntity = Convert.ToString(output.Tables[0].Rows[0]["ContactType"]);
            //    ViewBag.cddlActivity = Convert.ToString(output.Tables[0].Rows[0]["Lead_activityid"]);
            //    ViewBag.cddlActivityType = Convert.ToString(output.Tables[0].Rows[0]["Typeid"]);
            //    ViewBag.ctxt_Subject = Convert.ToString(output.Tables[0].Rows[0]["Leadsubject"]);
            //    ViewBag.cmemo_Details = Convert.ToString(output.Tables[0].Rows[0]["Leaddetails"]);
            //    ViewBag.AssignTo = Convert.ToString(output.Tables[0].Rows[0]["Assignto"]);
            //    ViewBag.Duration = Convert.ToString(output.Tables[0].Rows[0]["Durationid"]);
            //    ViewBag.ddlPriority = Convert.ToString(output.Tables[0].Rows[0]["Priorityid"]);
            //    ViewBag.cDue_dt = Convert.ToDateTime(output.Tables[0].Rows[0]["Duedate"]);


            //    string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //    CRMClassDataContext dcon = new CRMClassDataContext(connectionString);

            //    var Lead_ActivityTypes = from c in dcon.Lead_ActivityTypes
            //                             where c.LeadActivityId == Convert.ToInt32(output.Tables[0].Rows[0]["Lead_activityid"])
            //                             select c;
            //    crmAct.ActivityType = Lead_ActivityTypes.ToList();


            //}
            ViewBag.Estimate_code = Estimate_code;
            ViewBag.Module = Module_Name;

            return PartialView(@"~/Views/PMS/BillofQuantities/PartialTagProductPopup.cshtml", "Values");
        }

        public ActionResult GetTagProductCode(BillofQuantitiesVM model, String Estimatecode, String Module, String ProposalId)
        {
            try
            {
                DataTable dtProj = null;
                if (Module == "Estimate")
                {
                    dtProj = objdata.GetTagProducts(Estimatecode);
                }
                else if (Module == "Proposal")
                {
                    dtProj = objdata.GetTagProposalProducts(Estimatecode);
                }

                List<TagProductList> modelProj = new List<TagProductList>();
                modelProj = APIHelperMethods.ToModelList<TagProductList>(dtProj);
                //ViewBag.ProjectID = Project_ID;

                return PartialView("~/Views/PMS/BillofQuantities/PartialTagProductGrid.cshtml", modelProj);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
        }

        public JsonResult GetTagDetails(String detailsid, List<String> Tagid)
        {
            ReturnData obj = new ReturnData();
            String ids = "";
            try
            {
                if (Tagid != null)
                {
                    int i = 1;

                    if (Tagid != null && Tagid.Count > 0)
                    {
                        foreach (string item in Tagid)
                        {
                            if (i > 1)
                                ids = ids + "," + item;
                            else
                                ids = item;
                            i++;
                        }
                    }
                }

                var datasetobj = objdata.TaggingDetails("TaggingDetails", Convert.ToInt32(detailsid), ids);

                if (datasetobj.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in datasetobj.Tables[0].Rows)
                    {
                        obj.Success = Convert.ToBoolean(item["Success"]);
                        obj.Message = Convert.ToString(item["Message"]);
                        ViewBag.ProjectTagID = Convert.ToString(item["proj_id"]);
                    }
                }
            }
            catch { }
            return Json(obj);
        }

        public JsonResult EstimateBalQty(String detailsid)
        {
            ReturnData obj = new ReturnData();
            String ids = "";
            try
            {
                var datasetobj = objdata.CheckEstimateBalQty("CheckEstimateBalQty", Convert.ToInt32(detailsid));
                if (datasetobj.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in datasetobj.Tables[0].Rows)
                    {
                        obj.Success = Convert.ToBoolean(item["Success"]);
                        obj.Message = Convert.ToString(item["BALANCE_QTY"]);
                    }
                }
            }
            catch
            {

            }
            return Json(obj);
        }

        public ActionResult GetProposalCode(BillofQuantitiesVM model, String Proposal, String Customer_id, String Branchs)
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

                String BOQdT = DateTime.Now.ToString("yyyy-MM-dd");
                if (model.BOQDate != null)
                {
                    BOQdT = model.BOQDate;
                }

                DataTable dtProj = null;
                dtProj = objdata.GetProposalCode(Customer_id, Branch, Proposal, Convert.ToDateTime(BOQdT));

                List<ProposalList> modelProj = new List<ProposalList>();
                modelProj = APIHelperMethods.ToModelList<ProposalList>(dtProj);
                ViewBag.Proposal_ID = Proposal;

                return PartialView("~/Views/PMS/BillofQuantities/_PartialProposalLookup.cshtml", modelProj);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
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
    }
}