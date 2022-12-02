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
using System.Web.Services;

namespace Manufacturing.Controllers
{
    public class ReturnMaterialsIssueController : Controller
    {

        ReturnMaterialIssueViewModel obj = null;
        BOMEntryModel objdata = null;
        WorkOrderModel objWO = null;
        UserRightsForPage rights = new UserRightsForPage();
        DBEngine oDBEngine = new DBEngine();
        string JVNumStr = string.Empty;
        ReturnMaterialIssueModel objPI = null;
        CommonBL cSOrder = new CommonBL();
        Int32 Details_ID = 0;

        public ReturnMaterialsIssueController()
        {
            obj = new ReturnMaterialIssueViewModel();
            objdata = new BOMEntryModel();
            objWO = new WorkOrderModel();
            objPI = new ReturnMaterialIssueModel();
        }
        //
        // GET: /ProductionIssue/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MaterialIssueList()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/MaterialIssueList", "ReturnMaterialsIssue");
            //WorkOrderViewModel obj = new WorkOrderViewModel();
            TempData.Clear();
            ViewBag.CanAdd = rights.CanAdd;
            return View(obj);
        }

        [HttpGet]
        public ActionResult MaterialIssueEntry()
        {
            DataTable dtJobOrderexists=new DataTable();
            string JobOrderexists = "";
            ViewBag.TaggedData = "NO";
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
           // string WorkOrderModuleSkipped = "Yes";// cSOrder.GetSystemSettingsResult("WorkOrderModuleSkipped");
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
            try
            {
                DataTable objData;
                if (TempData["ProductionIssueID"] != null)
                {
                    obj.ProductionIssueID = Convert.ToInt64(TempData["ProductionIssueID"]);

                    if (TempData["Doctype"] != null)
                   

                    if (TempData["Doctype"] != null)
                    {
                        obj.Doctype = Convert.ToString(TempData["Doctype"]);
                    }

                    if (obj.ProductionIssueID > 0)
                    {
                        if (obj.Doctype == "BOM")
                        {
                            objData = objPI.GetProductionIssueData("GetProductionIssueDataFromInvoice", obj.ProductionIssueID, 0);
                        }
                        else
                        {
                            ////if (WorkOrderModuleSkipped == "No")
                            ////{
                              objData = objPI.GetProductionIssueData("GetProductionIssueData", obj.ProductionIssueID, 0, new DataTable());
                            ////}
                            ////else
                            ////{
                            //    objData = objPI.GetProductionIssueData("GetProductionIssueDataSttingsWise", obj.ProductionIssueID, 0, new DataTable());
                            ////}
                        }
                        if (objData != null && objData.Rows.Count > 0)
                        {
                            DataTable dt = objData;
                            foreach (DataRow row in dt.Rows)
                            {
                                obj.ProductionIssueID = Convert.ToInt64(row["ProductionIssueID"]);
                                obj.WorkOrderID = Convert.ToInt64(row["WorkOrderID"]);
                               // obj.ProductionOrderID = Convert.ToInt64(row["ProductionOrderID"]);
                                obj.Details_ID = Convert.ToInt64(row["Details_ID"]);
                                obj.Issue_SchemaID = Convert.ToInt64(row["Issue_SchemaID"]);
                                obj.Issue_No = Convert.ToString(row["Issue_No"]);
                                obj.Issue_Date = Convert.ToDateTime(row["Issue_Date"]);
                                obj.BRANCH_ID = Convert.ToInt64(row["BRANCH_ID"]);
                                obj.OrderNo = Convert.ToString(row["OrderNo"]);
                                obj.Issue_Qty = Convert.ToDecimal(row["Issue_Qty"]);
                                obj.BRANCH_ID = Convert.ToInt64(row["BRANCH_ID"]);
                                obj.WorkCenterID = Convert.ToInt64(row["WorkCenterID"]);
                               // obj.BOMNo = Convert.ToString(row["BOM_No"]);
                               // obj.RevNo = Convert.ToString(row["REV_No"]);
                                obj.FinishedItem = Convert.ToString(row["ProductName"]);
                                obj.FinishedUom = Convert.ToString(row["FinishedUom"]);
                                obj.Finished_Qty = Convert.ToDecimal(row["Finished_Qty"]);
                                obj.Warehouse = Convert.ToString(row["Warehouse"]);
                               // obj.ProductionOrderNo = Convert.ToString(row["ProductionOrderNo"]);
                                obj.WorkCenterCode = Convert.ToString(row["WorkCenterCode"]);
                                obj.WorkCenterDescription = Convert.ToString(row["WorkCenterDescription"]);

                                //if (Convert.ToString(row["ProductionOrderDate"]) != "")
                                //{
                                //    obj.ProductionOrderDate = Convert.ToDateTime(row["ProductionOrderDate"]).ToString("dd-MM-yyyy");
                                //}
                                //else
                                //{
                                //    obj.ProductionOrderDate = null;
                                //}
                                obj.MaxQty = Convert.ToDecimal(row["MaxQty"]);
                                obj.strRemarks = Convert.ToString(row["Remarks"]);
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

            if (TempData["IsView"] != null)
            {
                ViewBag.IsView = Convert.ToInt16(TempData["IsView"]);
            }
            else
            {
                ViewBag.IsView = 0;
            }
            ViewBag.LastCompany = Convert.ToString(Session["LastCompany"]);
            ViewBag.LastFinancialYear = Convert.ToString(Session["LastFinYear"]);
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
           // ViewBag.WorkOrderModuleSkipped = WorkOrderModuleSkipped;
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

        public JsonResult getNumberingSchemeRecord()
        {
            List<SchemaNumber> list = new List<SchemaNumber>();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            DataTable Schemadt = objdata.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "154", "Y");
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

        public ActionResult GetWOList(ReturnMaterialIssueViewModel ReturnMaterialIssueViewModel)
        {

            //string WorkOrderModuleSkipped = cSOrder.GetSystemSettingsResult("WorkOrderModuleSkipped");
            List<WorkOrderViewModel> list = new List<WorkOrderViewModel>();
            try
            {
                WorkOrderViewModel objWOL = new WorkOrderViewModel();
                DataTable objData;
                ////if (WorkOrderModuleSkipped == "No")
                ////{

                string Issueid = Convert.ToString(TempData["ProductionIssueID"]);
                if (Issueid=="")
                {
                    Issueid = "0";
                }

                objData = objPI.GetJobWorkOrderData("GetAllMaterialIssueData", Convert.ToInt64(Issueid), Convert.ToDateTime(ReturnMaterialIssueViewModel.WorkOrderDate));
                
              //  //}
              //  //else
              //  //{
              //      objData = objWO.GetWorkOrderData("GetPODataSettingsWise");
              ////  }


                if (objData != null && objData.Rows.Count > 0)
                {
                    DataTable dt = objData;
                    foreach (DataRow row in dt.Rows)
                    {
                        objWOL = new WorkOrderViewModel();
                        objWOL.WorkOrderID = Convert.ToInt64(row["WorkOrderID"]);
                        objWOL.OrderNo = Convert.ToString(row["OrderNo"]);
                        objWOL.Details_ID = Convert.ToInt64(row["Details_ID"]);
                        objWOL.OrderDate = Convert.ToDateTime(row["OrderDate"]).ToString("dd-MM-yyyy");
                        objWOL.FinishedItem = Convert.ToString(row["ProductName"]);
                        objWOL.FinishedUom = Convert.ToString(row["FinishedUom"]);
                        objWOL.Warehouse = Convert.ToString(row["Warehouse"]);
                        objWOL.WorkCenterID = Convert.ToString(row["WorkCenterID"]);
                        objWOL.WorkCenterCode = Convert.ToString(row["WorkCenterCode"]);
                        objWOL.WorkCenterDescription = Convert.ToString(row["WorkCenterDescription"]);
                        objWOL.Order_Qty = Convert.ToDecimal(row["Order_Qty"]);
                        objWOL.PartNo = Convert.ToString(row["PartNo"]);
                        objWOL.PartNoName = Convert.ToString(row["PartNoName"]);
                        objWOL.DesignNo = Convert.ToString(row["DesignNo"]);
                        objWOL.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                        objWOL.Description = Convert.ToString(row["sProducts_Name"]);
                        objWOL.Proj_Code = Convert.ToString(row["Proj_Code"]);
                        objWOL.WorkordrWarehouseId = Convert.ToInt64(row["WorkordrWarehouseId"]);
                        objWOL.Proj_Id = Convert.ToInt64(row["Proj_Id"]);
                        objWOL.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);

                        list.Add(objWOL);


                    }
                }

            }
            catch { }
           // ViewBag.WorkOrderModuleSkipped = WorkOrderModuleSkipped;
            return PartialView("_WorkOrderList", list);
        }

        public ActionResult GetWCList()
        {
            List<WorkCenterViewModel> list = new List<WorkCenterViewModel>();
            try
            {
                WorkCenterViewModel obj = new WorkCenterViewModel();
                DataTable objData = objPI.GetWorkOrderData("GetWCList");
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

        public ActionResult GetProductionIssueDetailsProductList(Int64 DetailsID = 0)
        {
          //  string WorkOrderModuleSkipped = cSOrder.GetSystemSettingsResult("WorkOrderModuleSkipped");
            MateialIssueProduct bomproductdataobj = new MateialIssueProduct();
            List<MateialIssueProduct> bomproductdata = new List<MateialIssueProduct>();
            try
            {
                if (TempData["DetailsID"] != null)
                {
                    DetailsID = Convert.ToInt64(TempData["DetailsID"]);
                    TempData.Keep();
                }
                //if (TempData["Doctype"] != null)
                //{
                //    obj.Doctype = Convert.ToString(TempData["Doctype"]);
                //}
                if (DetailsID > 0)
                {
                    DataTable objData = new DataTable();
                    if (TempData["ProductionIssueID"] != null)
                    {
                        //if (obj.Doctype == "BOM")
                        //{
                        //    objData = objPI.GetProductionIssueData("GetBOMProductionIssueDataFromInvoice", Convert.ToInt64(TempData["ProductionIssueID"]), DetailsID);
                        //}
                        //else
                        //{
                            ////if (WorkOrderModuleSkipped == "No")
                            ////{
                               objData = objPI.GetProductionIssueData("GetBOMProductionIssueData", Convert.ToInt64(TempData["ProductionIssueID"]), DetailsID);
                            ////}
                            ////else
                            ////{
                            //    objData = objPI.GetProductionIssueData("GetBOMProductionIssueDataSettingsWise", Convert.ToInt64(TempData["ProductionIssueID"]), DetailsID);
                            ////}
                       // }

                    }
                    else if (TempData["WorkOrderID"] != null)
                    {
                        ////if (WorkOrderModuleSkipped == "No")
                        ////{
                        objData = objPI.GetWorkOrderData("GetMaterialWorkOrderData", Convert.ToInt64(TempData["WorkOrderID"]), DetailsID);
                        ////}
                        ////else
                        ////{
                        //    objData = objWO.GetWorkOrderData("GetBOMPOData", Convert.ToInt64(TempData["WorkOrderID"]), DetailsID);
                        ////}
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
                            bomproductdataobj = new MateialIssueProduct();
                            bomproductdataobj.WorkOrderID = Convert.ToString(row["WorkOrderID"]);
                            bomproductdataobj.JobWorkID = Convert.ToString(row["WorkOrderID"]);
                            bomproductdataobj.SlNO = Convert.ToString(row["SlNO"]);
                            bomproductdataobj.BOMProductsID = Convert.ToString(row["BOMProductsID"]);
                            bomproductdataobj.Details_ID = Convert.ToString(row["Details_ID"]);
                            bomproductdataobj.ProductName = Convert.ToString(row["sProducts_Code"]);
                            bomproductdataobj.ProductId = Convert.ToString(row["BOMProductsID"]);
                            bomproductdataobj.ProductDescription = Convert.ToString(row["sProducts_Name"]);
                            bomproductdataobj.DesignNo = Convert.ToString(row["DesignNo"]);
                            bomproductdataobj.ItemRevisionNo = Convert.ToString(row["ItemRevisionNo"]);
                            
                            bomproductdataobj.ProductUOM = Convert.ToString(row["StkUOM"]);
                            bomproductdataobj.Warehouse = Convert.ToString(row["WarehouseName"]);
                            bomproductdataobj.Price = Convert.ToString(row["Price"]);
                          //  bomproductdataobj.Amount = Convert.ToString(row["Amount"]);
                           
                            bomproductdataobj.ProductUOMId = Convert.ToInt64(row["ProductUOMId"]);
                           // bomproductdataobj.BOMNo = Convert.ToString(row["BOMNo"]);
                           // bomproductdataobj.RevNo = Convert.ToString(row["RevNo"]);
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
                           // bomproductdataobj.Tag_Production_ID = Convert.ToString(row["Tag_Production_ID"]);
                           // bomproductdataobj.RevNo = Convert.ToString(row["RevNo"]);
                            bomproductdataobj.Product_NegativeStock = Convert.ToString(row["Product_NegativeStock"]);
                            bomproductdataobj.AvlStk = Convert.ToString(row["AvlStk"]);

                            bomproductdataobj.StkMsg = "0";
                            bomproductdataobj.IsInventory = Convert.ToString(row["IsInventory"]);
                            if (TempData["WorkOrderID"] != null)
                            {
                                //bomproductdataobj.BalQty = Convert.ToString(row["StkQty"]);
                                bomproductdataobj.BalQty = Convert.ToString(row["BalQty"]);
                            }

                            if (TempData["ProductionIssueID"] != null)
                            {
                                bomproductdataobj.OLDQty = Convert.ToString(row["OLDQty"]);
                                bomproductdataobj.BalQty = Convert.ToString(row["BalQty"]);
                            }
                            else
                            {
                                bomproductdataobj.OLDQty = Convert.ToString(row["StkQty"]);
                            }

                            if (TempData["ProductionIssueID"] != null)
                            {
                                bomproductdataobj.ProductQty = Convert.ToString(row["StkQty"]);
                                bomproductdataobj.Amount = Convert.ToString(Math.Round((Convert.ToDecimal(bomproductdataobj.ProductQty) * Convert.ToDecimal(bomproductdataobj.Price)), 2));
                            }
                            else if (TempData["WorkOrderID"] != null)
                            {
                                bomproductdataobj.ProductQty = Convert.ToString(row["BalQty"]);
                                bomproductdataobj.Amount = Convert.ToString(Math.Round((Convert.ToDecimal(bomproductdataobj.BalQty) * Convert.ToDecimal(bomproductdataobj.Price)), 2));
                            }
                            //if (TempData["WorkOrderID"] != null)
                            //{
                            //    bomproductdataobj.BalQty = Convert.ToString(row["BalQty"]);
                            //}

                            if (TempData["ProductionIssueID"] != null)
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


                        if (TempData["ProductionIssueID"] != null)
                        {
                            ViewData["ViewProductionIssueID"] = TempData["ProductionIssueID"];
                        }


                    }
                }
            }
            catch { }
            return PartialView("_MaterialIssueBOMProductGrid", bomproductdata);
        }

        public JsonResult SetTempID(Int64 DetailsID, Int64 WorkOrderId)
        {
            if (DetailsID > 0)
            {
                TempData["DetailsID"] = DetailsID;
                TempData["WorkOrderID"] = WorkOrderId;
                TempData.Keep();
            }
            else
            {
                TempData["DetailsID"] = null;
                TempData["WorkOrderID"] = null;
                TempData.Clear();
            }
            return Json(true);
        }

        [ValidateInput(false)]
        public ActionResult BatchEditingProductionIssue(DevExpress.Web.Mvc.MVCxGridViewBatchUpdateValues<MateialIssueProduct, int> updateValues, ReturnMaterialIssueViewModel options)
        {
            TempData["Count"] = (int)TempData["Count"] + 1;
            TempData.Keep();
            String NumberScheme = "";
            String Message = "";

            if ((int)TempData["Count"] != 2)
            {
                Boolean IsProcess = false;
                List<MateialIssueProduct> list = new List<MateialIssueProduct>();
                if (updateValues.Update.Count > 0 && Convert.ToInt64(options.Details_ID) > 0)
                {
                    List<udt_ReturnMaterialsDetails> udtlist = new List<udt_ReturnMaterialsDetails>();
                    udt_ReturnMaterialsDetails obj = null;

                    foreach (var item in updateValues.Update)
                    {
                        if (Convert.ToInt64(item.BOMProductsID) > 0)
                        {
                            obj = new udt_ReturnMaterialsDetails();

                            obj.DetProductId = Convert.ToInt64(item.BOMProductsID);
                            obj.DetDrawingNo = Convert.ToString(item.DesignNo);
                            obj.DetDrawingRevNO = Convert.ToString(item.ItemRevisionNo);
                            obj.DetQty = Convert.ToDecimal(item.ProductQty);
                            obj.Uom = Convert.ToInt64(item.ProductUOMId);
                            obj.Price = Convert.ToDecimal(item.Price);
                            obj.Amount = Convert.ToDecimal(item.Amount);
                            obj.Remarks = Convert.ToString(item.Remarks);
                            udtlist.Add(obj);
                        }
                    }
                    if (udtlist.Count > 0)
                    {
                        if (options.ProductionIssueID > 0)
                        {
                            IsProcess = WorkOrderBOMProductInsertUpdate(udtlist, options);
                        }
                        else
                        {
                            NumberScheme = checkNMakePICode(options.Issue_No, Convert.ToInt32(options.Issue_SchemaID), Convert.ToDateTime(options.Issue_Date));
                            if (NumberScheme == "ok")
                            {
                                IsProcess = WorkOrderBOMProductInsertUpdate(udtlist, options);
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

                ViewData["DetailsID"] = Details_ID;

            }
            return PartialView("_MaterialIssueBOMProductGrid", updateValues.Update);
        }


        public ActionResult GetWorkOrderFinishItemDetailsDetailsProductList()
        {
            ReturnMaterialsFinishItemDetails FinishItemDetailsobj = new ReturnMaterialsFinishItemDetails();
            List<ReturnMaterialsFinishItemDetails> FinishItemDetailsproductdata = new List<ReturnMaterialsFinishItemDetails>();
            Int64 DetailsID = 0;
            Int64 JobId = 0;
            try
            {

                if (TempData["ProductionIssueID"] != null)
                {
                    DetailsID = Convert.ToInt64(TempData["ProductionIssueID"]);
                    TempData.Keep();
                }
                else if (TempData["WorkOrderID"] != null)
                {
                    DetailsID = Convert.ToInt64(TempData["WorkOrderID"]);
                    JobId = DetailsID = Convert.ToInt64(TempData["WorkOrderID"]);
                    TempData.Keep();
                }
                DataTable dt = new DataTable();
                if (DetailsID > 0 && TempData["ProductsDetails"] == null)
                {
                    DataTable objData = new DataTable();
                    if (TempData["ProductionIssueID"] != null)
                    {
                        objData = objPI.GetJobWorkOrderMultipleFinishdata("ReturnMaterissueEditFinishItemList", Convert.ToInt64(DetailsID));
                    }
                    else if (TempData["WorkOrderID"] != null)
                    {
                        objData = objPI.GetJobWorkOrderMultipleFinishdata("ReturnMaterissueAddFinishItemList", Convert.ToInt64(DetailsID));
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
                        dtable.Columns.Add("MaterialIssueID", typeof(System.String));
                        dtable.Columns.Add("ReturnMaterialIssueID", typeof(System.String));
                        dtable.Columns.Add("FinishUOMId", typeof(System.String));
                        dtable.Columns.Add("FinishProductsID", typeof(System.String));
                        dtable.Columns.Add("OldFGQuantity", typeof(System.String));
                        dtable.Columns.Add("MaxBalFGQuantity", typeof(System.String));

                        String Gid = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            Gid = Guid.NewGuid().ToString();
                            FinishItemDetailsobj = new ReturnMaterialsFinishItemDetails();
                            FinishItemDetailsobj.SrlNO = Convert.ToString(row["SrlNO"]);
                            FinishItemDetailsobj.FinishItemName = Convert.ToString(row["FinishItemName"]);
                            FinishItemDetailsobj.FinishItemDescription = Convert.ToString(row["FinishItemDescription"]);
                            FinishItemDetailsobj.FinishDrawingNo = Convert.ToString(row["FinishDrawingNo"]);
                            FinishItemDetailsobj.FinishItemRevNo = Convert.ToString(row["FinishItemRevNo"]);
                            FinishItemDetailsobj.Qty = Convert.ToString(row["Qty"]);
                            FinishItemDetailsobj.FinishUOM = Convert.ToString(row["FinishUOM"]);
                            FinishItemDetailsobj.FinishPrice = Convert.ToString(row["FinishPrice"]);
                            FinishItemDetailsobj.FinishAmount = Convert.ToString(row["FinishAmount"]);

                            FinishItemDetailsobj.MaterialIssueID = Convert.ToString(row["MaterialIssueID"]);
                            FinishItemDetailsobj.ReturnMaterialIssueID = Convert.ToString(row["ReturnMaterialIssueID"]);
                            FinishItemDetailsobj.FinishUOMId = Convert.ToString(row["FinishUOMId"]);
                            FinishItemDetailsobj.FinishProductsID = Convert.ToString(row["FinishProductsID"]);
                            FinishItemDetailsobj.OldFGQuantity = Convert.ToString(row["OldFGQuantity"]);
                            FinishItemDetailsobj.MaxBalFGQuantity = Convert.ToString(row["MaxBalFGQuantity"]);
                            FinishItemDetailsobj.Guids = Gid;

                            FinishItemDetailsproductdata.Add(FinishItemDetailsobj);


                            object[] trow = { Gid, row["SrlNO"],Convert.ToString(row["FinishItemName"]),Convert.ToString(row["FinishItemDescription"]),
                                                Convert.ToString(row["FinishDrawingNo"]),Convert.ToString(row["FinishItemRevNo"]),Convert.ToString(row["Qty"]),
                                   Convert.ToString(row["FinishUOM"]),Convert.ToString(row["FinishPrice"]),Convert.ToString(row["FinishAmount"]),"1",JobId,DetailsID,
                                    Convert.ToString(row["FinishUOMId"]),Convert.ToString(row["FinishProductsID"]),Convert.ToString(row["OldFGQuantity"]),Convert.ToString(row["MaxBalFGQuantity"]) };
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

                            FinishItemDetailsobj = new ReturnMaterialsFinishItemDetails();
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
                            FinishItemDetailsobj.OldFGQuantity = Convert.ToString(row["OldFGQuantity"]);
                            FinishItemDetailsobj.MaxBalFGQuantity = Convert.ToString(row["MaxBalFGQuantity"]);
                            FinishItemDetailsproductdata.Add(FinishItemDetailsobj);
                        }

                    }
                }
                TempData["ProductsDetails"] = dt;
                TempData.Keep();

            }
            catch { }

            return PartialView("_ReturnIssueFinishItemGrid", FinishItemDetailsproductdata);
            //return PartialView("~/Views/PMS/Estimate/EstimateProductList.cshtml", FinishItemDetailsproductdata);
        }


        public string checkNMakePICode(string manual_str, int sel_schema_Id, DateTime RevisionDate)
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
                        sqlQuery = "SELECT max(tjv.ReturenMaterialIssue_No) FROM Mfc_ReturnMaterialIssue tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.ReturenMaterialIssue_No))) = 1 and ReturenMaterialIssue_No like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.ReturenMaterialIssue_No) FROM Mfc_ReturnMaterialIssue tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.ReturenMaterialIssue_No))) = 1 and ReturenMaterialIssue_No like '%" + sufxCompCode + "'";
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
                            sqlQuery = "SELECT max(tjv.ReturenMaterialIssue_No) FROM Mfc_ReturnMaterialIssue tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + paddCounter + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.ReturenMaterialIssue_No))) = 1 and ReturenMaterialIssue_No like '" + prefCompCode + "%'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }
                        else
                        {
                            int i = startNo.Length;
                            while (i < paddCounter)
                            {


                                sqlQuery = "SELECT max(tjv.ReturenMaterialIssue_No) FROM Mfc_ReturnMaterialIssue tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + i + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.ReturenMaterialIssue_No))) = 1 and ReturenMaterialIssue_No like '" + prefCompCode + "%'";

                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.ReturenMaterialIssue_No)=" + i;
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
                                sqlQuery = "SELECT max(tjv.ReturenMaterialIssue_No) FROM Mfc_ReturnMaterialIssue tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + (i - 1) + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.ReturenMaterialIssue_No))) = 1 and ReturenMaterialIssue_No like '" + prefCompCode + "%'";
                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.ReturenMaterialIssue_No)=" + (i - 1);
                                }
                                dtC = oDBEngine.GetDataTable(sqlQuery);
                            }

                        }

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.ReturenMaterialIssue_No) FROM Mfc_ReturnMaterialIssue tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.ReturenMaterialIssue_No))) = 1 and ReturenMaterialIssue_No like '" + prefCompCode + "%'";
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
                    sqlQuery = "SELECT ReturenMaterialIssue_No FROM Mfc_ReturnMaterialIssue WHERE ReturenMaterialIssue_No LIKE '" + manual_str.Trim() + "'";
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


        public Boolean WorkOrderBOMProductInsertUpdate(List<udt_ReturnMaterialsDetails> obj, ReturnMaterialIssueViewModel obj2)
        {
          //  string WorkOrderModuleSkipped = cSOrder.GetSystemSettingsResult("WorkOrderModuleSkipped");
            Boolean Success = false;
            try
            {
                Session["dtForReal"]=null;
                DataTable dtForReal = new DataTable(); 
                DataTable dtBOM_PRODUCTS = new DataTable();
                dtBOM_PRODUCTS = ToDataTable(obj);
                DataTable dtWarehouse = new DataTable();
                DataTable dtWarehouseFresh = new DataTable();

                if (Session["PIssue_WarehouseData"] != null)
                {
                    dtWarehouse = (DataTable)Session["PIssue_WarehouseData"];
                    dtWarehouseFresh = (DataTable)Session["PIssue_WarehouseData"];
                }
                
                //if (Session["PIssue_WarehouseData"] != null)
                //{
                //    Session["dtForReal"] = dtWarehouse;
                //}

                //if (Session["dtForReal"] !=null)
                //{
                //    dtForReal = (DataTable)Session["dtForReal"];
                //}
                if (dtWarehouseFresh != null)
                {
                    //if (dtWarehouseFresh.Rows.Count > 0)
                    //{
                    //    int LoopCount = 0;
                    //    foreach (DataRow row in dtWarehouseFresh.Rows)
                    //    {
                    //        DataTable dtcFresh = new DataTable();
                    //        dtcFresh = ToDataTable(obj.Where(x => x.DetProductId == Convert.ToInt64(row["ProductID"])).ToList());
                    //        if (dtcFresh != null && dtcFresh.Rows.Count > 0)
                    //        {
                    //            row["Quantity"] = Convert.ToDecimal(dtcFresh.Rows[0]["DetQty"]);
                    //        }
                    //        else
                    //        {
                    //            row["Quantity"] = Convert.ToDecimal(obj[LoopCount].DetQty);
                    //        }
                    //        row["WarehouseID"] = obj2.WorkCenterID;
                    //        LoopCount++;
                    //    }
                    //}

                }

                string DocType = "";
                Int64 WorkOrderID = 0;
                //if (WorkOrderModuleSkipped == "No")
                //{
                DocType = "JobWorkOrder";
                    WorkOrderID = obj2.WorkOrderID;
                //}
                //else
                //{
                //    DocType = "ProductionOrder";
                //    WorkOrderID = 0;
                //}

                    DataTable dtProductFinish = new DataTable();
                    dtProductFinish = (DataTable)TempData["ProductsDetails"];



                    List<ReturnMaterialsFinishItemDetails> udt = new List<ReturnMaterialsFinishItemDetails>();
                    if (dtProductFinish != null && dtProductFinish.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtProductFinish.Rows)
                        {
                            ReturnMaterialsFinishItemDetails obj1 = new ReturnMaterialsFinishItemDetails();
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

                            obj1.MaterialIssueID = Convert.ToString(item["MaterialIssueID"]);
                            if (obj1.MaterialIssueID == "")
                            {
                                obj1.MaterialIssueID = "0";
                            }

                            obj1.ReturnMaterialIssueID = Convert.ToString(item["ReturnMaterialIssueID"]);
                            if (obj1.ReturnMaterialIssueID == "")
                            {
                                obj1.ReturnMaterialIssueID = "0";
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



                DataSet dt = new DataSet();
                if (Convert.ToInt64(obj2.Details_ID) > 0)
                {
                    if (!String.IsNullOrEmpty(obj2.Issue_No) && obj2.Issue_No.ToLower() != "auto")
                    {
                        JVNumStr = obj2.Issue_No;
                    }
                    dt = objPI.ProductionIssueBOMProductInsertUpdate("INSERTPIBOM", obj2.ProductionIssueID, WorkOrderID, obj2.ProductionOrderID, obj2.Details_ID, Convert.ToInt64(obj2.WorkCenterID), JVNumStr, obj2.Issue_SchemaID, Convert.ToDateTime(obj2.Issue_Date),
                        obj2.Issue_Qty, obj2.TotalCost, obj2.BRANCH_ID, Convert.ToInt64(Session["userid"]), Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]),
                        obj2.strRemarks, obj2.PartNo,
                        dtBOM_PRODUCTS, dtWarehouseFresh, dtWarehouse, DocType, dt_PRODUCTS);
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
        public JsonResult getProductWiseWarehouseRecord(String branchid = null, String productid = null,string MaterialsIssue=null)
        {
            List<BranchWarehouse> list = new List<BranchWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                dt = objPI.GetManufacturingProductionIssueWarehousedetails("GetReturnmaterialsWareHouseByProductID", productid, Convert.ToString(Session["LastFinYear"]), branchid, Convert.ToString(Session["LastCompany"]), multiwarehouse, null, null, null, null, MaterialsIssue);
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
        public JsonResult getBatchRecordReturnMaterials(String warehouseid = null, String ProductID = null, string MaterialsIssue=null)
        {
            List<BatchWarehouse> list = new List<BatchWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                dt = objPI.GetManufacturingProductionIssueWarehousedetails("GetBatchByProductIDWarehouseReturnmaterials", ProductID, Convert.ToString(Session["LastFinYear"]), null, Convert.ToString(Session["LastCompany"]), multiwarehouse, warehouseid, null, null, null, MaterialsIssue);
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
        public JsonResult AvailableStockCheck(String WarehouseID = null, String ProductID = null, string MaterialsIssue = null, string ViewBatch = null, string Branch = null)
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
                    dt = objPI.GetManufacturingProductionIssueWarehousedetails("AvailableStockCheckBalanceBatch", ProductID, Convert.ToString(Session["LastFinYear"]), Branch, Convert.ToString(Session["LastCompany"]), multiwarehouse, WarehouseID, ViewBatch, null, null, MaterialsIssue);
                }
                else
                {
                    dt = objPI.GetManufacturingProductionIssueWarehousedetails("AvailableStockCheckBalanceWithoutBatch", ProductID, Convert.ToString(Session["LastFinYear"]), Branch, Convert.ToString(Session["LastCompany"]), multiwarehouse, WarehouseID, ViewBatch, null, null, MaterialsIssue);
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
        public JsonResult setStockWarehouseList(List<udtReturnMaterialStockProduct> items, String SrlNo, Int64 Unit)
        {
            Boolean Success = false;
            String Message = "";
            Boolean IsProcess = false;
            try
            {
                DataTable dt = ToDataTable(items);
                Session["PIssue_WarehouseData"] = dt;

                DataTable dtWarehouse = ToDataTable(items.Where(x => x.Product_SrlNo == SrlNo).ToList());
                //DataTable result = objPI.GetProductionIssueData("StockWarehouseBalCheck", 0, Unit, dtWarehouse);
                //foreach (DataRow dr in result.Rows)
                //{
                IsProcess = true; //Convert.ToBoolean(dr["IsProcess"]);
               // }

                Success = true;

            }
            catch { }
            Message = Success.ToString() + "|" + IsProcess.ToString();
            return Json(Message);
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

        public ActionResult GetMaterialIssueList()
        {
          //  string WorkOrderModuleSkipped = cSOrder.GetSystemSettingsResult("WorkOrderModuleSkipped");
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            List<ReturnMaterialIssueViewModel> list = new List<ReturnMaterialIssueViewModel>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/MaterialIssueList", "ReturnMaterialsIssue");
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
                        //if (WorkOrderModuleSkipped == "No")
                        //{
                        dt = oDBEngine.GetDataTable("select * from V_ReturnMaterialsIssueList where BRANCH_ID =" + BranchID + " AND (Issue_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') ORDER BY ProductionIssueID DESC");
                        //}
                        //else
                        //{
                        //    dt = oDBEngine.GetDataTable("select * from V_ProductionIssueListFromPO where BRANCH_ID =" + BranchID + " AND (Issue_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') ORDER BY ProductionIssueID DESC");

                        //}
                    }
                    else
                    {
                        //if (WorkOrderModuleSkipped == "No")
                        //{
                        dt = oDBEngine.GetDataTable("select * from V_ReturnMaterialsIssueList where Issue_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' ORDER BY ProductionIssueID DESC");

                        //}
                        //else
                        //{
                        //    dt = oDBEngine.GetDataTable("select * from V_ProductionIssueListFromPO where  Issue_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "'  ORDER BY ProductionIssueID DESC");

                        //}
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
                    ReturnMaterialIssueViewModel obj = new ReturnMaterialIssueViewModel();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new ReturnMaterialIssueViewModel();
                        obj.ProductionIssueID = Convert.ToInt64(item["ProductionIssueID"]);
                       // obj.ProductionOrderID = Convert.ToInt64(item["ProductionOrderID"]);
                        obj.WorkCenterID = Convert.ToInt64(item["WorkCenterID"]);
                        obj.Issue_No = Convert.ToString(item["Issue_No"]);
                        obj.Issue_Qty = Convert.ToDecimal(item["Issue_Qty"]);
                        obj.WorkCenterCode = Convert.ToString(item["WorkCenterCode"]);
                        obj.WorkCenterDescription = Convert.ToString(item["WorkCenterDescription"]);
                        obj.BRANCH_ID = Convert.ToInt64(item["BRANCH_ID"]);
                       // obj.BOMNo = Convert.ToString(item["BOM_No"]);
                      //  obj.RevNo = Convert.ToString(item["REV_No"]);

                        obj.WorkOrderNo = Convert.ToString(item["WorkOrderNo"]);

                        if (Convert.ToString(item["WorkOrderDate"]) != "")
                        {
                            obj.WorkOrderDate = Convert.ToDateTime(item["WorkOrderDate"]);
                        }
                        else
                        {
                            obj.WorkOrderDate = null;
                        }
                        // obj.WorkOrderDate = Convert.ToDateTime(item["WorkOrderDate"]);

                        //obj.BOM_Date = Convert.ToDateTime(item["BOM_Date"]);
                        //obj.ProductionOrderNo = Convert.ToString(item["ProductionOrderNo"]);
                        //obj.dtProductionOrderDate = Convert.ToDateTime(item["ProductionOrderDate"]);

                        //if (Convert.ToString(item["ProductionOrderDate"]) != "")
                        //{
                        //    obj.dtProductionOrderDate = Convert.ToDateTime(item["ProductionOrderDate"]);
                        //}
                        //else
                        //{
                        //    obj.dtProductionOrderDate = null;
                        //}


                        //if (Convert.ToString(item["REV_Date"]) != "")
                        //{
                        //    obj.REV_Date = Convert.ToDateTime(item["REV_Date"]);
                        //}
                        //else
                        //{
                        //    obj.REV_Date = null;
                        //}

                        if (Convert.ToString(item["Issue_Date"]) != "")
                        {
                            obj.Issue_Date = Convert.ToDateTime(item["Issue_Date"]);
                        }
                        else
                        {
                            obj.Issue_Date = null;
                        }

                        obj.CreatedBy = Convert.ToString(item["CreatedBy"]);
                        obj.ClosedStatus = Convert.ToString(item["ClosedStatus"]);
                        obj.TaggedStatus = Convert.ToString(item["TaggedStatus"]);
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
                        //obj.DesignNo = Convert.ToString(item["DesignNo"]);
                        //obj.ItemRevNo = Convert.ToString(item["ItemRevisionNo"]);
                        //obj.Description = Convert.ToString(item["sProducts_Name"]);
                        obj.Proj_Code = Convert.ToString(item["Proj_Code"]);
                        obj.Proj_Name = Convert.ToString(item["Proj_Name"]);
                        obj.Doctype = Convert.ToString(item["Doctype"]);
                        list.Add(obj);
                    }
                }
            }
            catch { }
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
          
            ViewBag.CanPrint = rights.CanPrint;
           
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            return PartialView("_MaterialIssueDataList", list);
        }


        public JsonResult ClosedJobDataByID(Int64 detailsid, String ClosedJobRemarks = "")
        {
            ReturnDataClosed obj = new ReturnDataClosed();
            try
            {
                DataTable datasetobj = objPI.ClosedMateriuals("ClosedMaterialsData", detailsid, ClosedJobRemarks);
                if (datasetobj != null && datasetobj.Rows.Count > 0)
                {
                    obj.Success = Convert.ToInt64(datasetobj.Rows[0]["Status"]);

                }
            }
            catch { }
            return Json(obj);
        }

        public JsonResult CancelJobDataByID(Int64 detailsid, String ClosedJobRemarks = "")
        {
            ReturnDataClosed obj = new ReturnDataClosed();
            try
            {
                DataTable datasetobj = objPI.ClosedMateriuals("CancelMaterialsData", detailsid, ClosedJobRemarks);
                if (datasetobj != null && datasetobj.Rows.Count > 0)
                {
                    obj.Success = Convert.ToInt64(datasetobj.Rows[0]["Status"]);

                }
            }
            catch { }
            return Json(obj);
        }

        public JsonResult SetPIDataByID(Int64 productionissueid = 0, Int16 IsView = 0, string Doctype = "")
        {
            Boolean Success = false;
            try
            {
                TempData.Clear();
                TempData["ProductionIssueID"] = productionissueid;
                TempData["IsView"] = IsView;
                TempData["Doctype"] = Doctype;
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
            settings.Name = "Material Production";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "MaterialProduction";
            //String ID = Convert.ToString(TempData["EmployeesTargetListDataTable"]);
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "Issue_No" || datacolumn.ColumnName == "Issue_Date" || datacolumn.ColumnName == "WorkOrderNo" || datacolumn.ColumnName == "WorkOrderDate"
                   || datacolumn.ColumnName == "CreatedBy" || datacolumn.ColumnName == "ModifyBy" || datacolumn.ColumnName == "CreateDate" || datacolumn.ColumnName == "ModifyDate")
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
                       
                        else if (datacolumn.ColumnName == "CreatedBy")
                        {
                            column.Caption = "Entered By";
                            column.VisibleIndex = 4;
                        }
                        else if (datacolumn.ColumnName == "ModifyBy")
                        {
                            column.Caption = "Modified By";
                            column.VisibleIndex = 5;
                        }
                        else if (datacolumn.ColumnName == "CreateDate")
                        {
                            column.Caption = "Entered On";
                            column.VisibleIndex = 6;
                        }
                        else if (datacolumn.ColumnName == "ModifyDate")
                        {
                            column.Caption = "Modified On";
                            column.VisibleIndex = 7;
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

        [HttpPost]
        public JsonResult GetWarehouseList(Int64 ProductionIssueID = 0)
        {
            List<ReturnMaterialIssueStkWarehouse> list = new List<ReturnMaterialIssueStkWarehouse>();
            try
            {
                DataTable dt = new DataTable();
                if (ProductionIssueID > 0)
                {
                    dt = objPI.GetProductionIssueData("GetProductionIssueWarehouseData", ProductionIssueID, 0, new DataTable());
                    if (dt.Rows.Count > 0)
                    {
                        ReturnMaterialIssueStkWarehouse obj = new ReturnMaterialIssueStkWarehouse();
                        foreach (DataRow item in dt.Rows)
                        {
                            obj = new ReturnMaterialIssueStkWarehouse();
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

        public JsonResult RemovePIDataByID(Int32 ProductionIssueID)
        {
            //Boolean Success = false;
            ReturnData obj = new ReturnData();
            try
            {
                var datasetobj = objPI.GetReturnmatIssueDeleteData("RemovePIData", ProductionIssueID, 0, new DataTable(), Convert.ToInt64(Session["userid"]));
                if (datasetobj.Rows.Count > 0)
                {

                    foreach (DataRow item in datasetobj.Rows)
                    {
                        //Success = Convert.ToBoolean(item["Success"]);
                        obj.Success = Convert.ToBoolean(item["Success"]);
                        obj.Message = Convert.ToString(item["Message"]);
                    }
                }
            }
            catch { }
            return Json(obj);
        }

        [WebMethod]
        public JsonResult AddProduct(ReturnMaterialsFinishItemDetails prod)
        {
            DataTable dt = (DataTable)TempData["ProductsDetails"];
            DataTable dt2 = new DataTable();

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
                dtable.Columns.Add("MaterialIssueID", typeof(System.String));
                dtable.Columns.Add("ReturnMaterialIssueID", typeof(System.String));
                dtable.Columns.Add("FinishUOMId", typeof(System.String));
                dtable.Columns.Add("FinishProductsID", typeof(System.String));
                dtable.Columns.Add("OldFGQuantity", typeof(System.String));
                dtable.Columns.Add("MaxBalFGQuantity", typeof(System.String));
                object[] trow = { Guid.NewGuid(), 1,prod.FinishItemName,prod.FinishItemDescription,prod.FinishDrawingNo,prod.FinishItemRevNo,prod.Qty,prod.FinishUOM,prod.FinishPrice,prod.FinishAmount,prod.FinishUpdateEdit,
                                    prod.MaterialIssueID,prod.ReturnMaterialIssueID,prod.FinishUOMId,prod.FinishProductsID,prod.OldFGQuantity,prod.MaxBalFGQuantity };
                dtable.Rows.Add(trow);
                TempData["ProductsDetails"] = dtable;
                TempData.Keep();
            }
            else
            {
                if (string.IsNullOrEmpty(prod.Guids))
                {
                    object[] trow = { Guid.NewGuid(), Convert.ToInt32(dt.Rows.Count)+1,prod.FinishItemName,prod.FinishItemDescription,prod.FinishDrawingNo,prod.FinishItemRevNo,prod.Qty,prod.FinishUOM,prod.FinishPrice,prod.FinishAmount,prod.FinishUpdateEdit,
                                    prod.MaterialIssueID,prod.ReturnMaterialIssueID,prod.FinishUOMId,prod.FinishProductsID,prod.OldFGQuantity,prod.MaxBalFGQuantity };// Add new parameter Here
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
                                item["MaterialIssueID"] = prod.MaterialIssueID;
                                item["ReturnMaterialIssueID"] = prod.ReturnMaterialIssueID;
                                item["FinishUOMId"] = prod.FinishUOMId;
                                item["FinishProductsID"] = prod.FinishProductsID;
                                item["OldFGQuantity"] = prod.OldFGQuantity;
                                item["MaxBalFGQuantity"] = prod.MaxBalFGQuantity;

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
        public JsonResult EditData(String HiddenID)
        {
            ReturnMaterialsFinishItemDetails ret = new ReturnMaterialsFinishItemDetails();

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
                        ret.MaterialIssueID = item["MaterialIssueID"].ToString();
                        ret.ReturnMaterialIssueID = item["ReturnMaterialIssueID"].ToString();
                        ret.FinishUOMId = item["FinishUOMId"].ToString();
                        ret.FinishProductsID = item["FinishProductsID"].ToString();
                        ret.OldFGQuantity = item["OldFGQuantity"].ToString();
                        ret.MaxBalFGQuantity = item["MaxBalFGQuantity"].ToString();
                        break;
                    }
                }
            }
            TempData["ProductsDetails"] = dt;
            TempData.Keep();
            return Json(ret);
        }

        [WebMethod]
        public JsonResult ChangeFinishDataData()
        {
            DataTable dt = (DataTable)TempData["ProductsDetails"];

            dt = null;
            TempData["ProductsDetails"] = dt;
            TempData.Keep();
            return Json("Changes of Material issue done.");
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
                    dr["SrlNO"] = conut;
                    conut++;
                }
            }

            TempData["ProductsDetails"] = dt;
            TempData.Keep();
            return Json("Product Remove Successfully.");
        }

    }
}