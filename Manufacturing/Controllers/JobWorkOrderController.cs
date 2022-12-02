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
using UtilityLayer;

namespace Manufacturing.Controllers
{
    public class JobWorkOrderController : Controller
    {

        BOMEntryModel objdata = null;
        JobWorkOrderViewModel objWC = null;
        DBEngine oDBEngine = new DBEngine();
        JobWorkOrderModel objWO = null;
        string JVNumStr = string.Empty;
        UserRightsForPage rights = new UserRightsForPage();
        ProductionOrderModel objPM = null;
        CommonBL cSOrder = new CommonBL();
        public JobWorkOrderController()
        {
            objdata = new BOMEntryModel();
            objWC = new JobWorkOrderViewModel();
            objWO = new JobWorkOrderModel();
            objPM = new ProductionOrderModel();
        }
        //
        // GET: /WorkOrder/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WorkOrderEntry()
        {
            DataTable dtJobOrderexists = new DataTable();
            string JobOrderexists = "";
            ViewBag.TaggedData = "NO";

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
            TempData["ProductsDetails"] = null;
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

                if (TempData["WorkOrderID"] != null)
                {
                    objWC.WorkOrderID = Convert.ToString(TempData["WorkOrderID"]);
                    TempData.Keep();


                    dtJobOrderexists = oDBEngine.GetDataTable("select top 1 isnull(JobWorkOrderID,0) JobWorkOrderID  from JobWorkOrder  where JobWorkOrderID in (select Job_OrderId from Materials_FinishItemDetails) and JobWorkOrderID='" + Convert.ToInt64(TempData["WorkOrderID"]) + "'");

                    if (dtJobOrderexists != null && dtJobOrderexists.Rows.Count > 0)
                    {
                        JobOrderexists = Convert.ToString(dtJobOrderexists.Rows[0]["JobWorkOrderID"]);
                        if (JobOrderexists != "" && JobOrderexists != "0")
                        {
                            ViewBag.TaggedData = "YES";
                        }
                        else
                        {
                            ViewBag.TaggedData = "NO";
                        }
                    }
                    else
                    {
                        ViewBag.TaggedData = "NO";
                    }

                    if (Convert.ToInt64(objWC.WorkOrderID) > 0)
                    {
                        DataTable objData = objWO.GetWorkOrderData("GetWorkOrderData", objWC.WorkOrderID);
                        if (objData != null && objData.Rows.Count > 0)
                        {
                            DataTable dt = objData;


                            foreach (DataRow row in dt.Rows)
                            {
                                objWC.WorkOrderID = Convert.ToString(row["WorkOrderID"]);
                                //objWC.Production_ID = Convert.ToInt64(row["ProductionOrderID"]);
                                objWC.WorkCenterID = Convert.ToString(row["WorkCenterID"]);
                                //Mantis Issue 24213
                                Session["WorkCenterID"] = Convert.ToString(row["WorkCenterID"]);
                                //End of Mantis Issue 24213
                                objWC.OrderNo = Convert.ToString(row["OrderNo"]);
                                objWC.Order_SchemaID = Convert.ToString(row["Order_SchemaID"]);
                                objWC.OrderDate = Convert.ToDateTime(row["OrderDate"]).ToString("dd-MM-yyyy");
                                objWC.dtOrderDate = Convert.ToDateTime(row["OrderDate"]);
                                objWC.BRANCH_ID = Convert.ToString(row["BRANCH_ID"]);
                                objWC.Order_Qty = Convert.ToString(row["Order_Qty"]);
                                objWC.JobWorkRate = Convert.ToString(row["JobWorkRate"]);
                                //objWC.ActualAdditionalCost = Convert.ToDecimal(row["ActualAdditionalCost"]);
                               // objWC.ActualComponentCost = Convert.ToDecimal(row["ActualComponentCost"]);
                               // objWC.ActualProductCost = Convert.ToDecimal(row["ActualProductCost"]);
                                //objWC.ProductionOrderQty = Convert.ToDecimal(row["ProductionOrderQty"]);
                                //objWC.FGReceiptQty = Convert.ToDecimal(row["FGReceiptQty"]);
                              //  objWC.TotalCost = Convert.ToDecimal(row["TotalCost"]);
                               // objWC.BOMNo = Convert.ToString(row["BOM_No"]);
                               // objWC.RevNo = Convert.ToString(row["REV_No"]);
                                objWC.PartProductId = Convert.ToString(row["PartProductId"]);
                                objWC.warehouseId = Convert.ToString(row["warehouseId"]);
                                ViewBag.HdnPartProductId = Convert.ToInt16(row["PartProductId"]);
                                ViewBag.HdnPartWarehouseId = Convert.ToInt16(row["warehouseId"]);
                                objWC.FinishedItem = Convert.ToString(row["ProductName"]);
                                objWC.FinishedUom = Convert.ToString(row["FinishedUom"]);
                                objWC.Finished_Qty = Convert.ToString(row["Order_Qty"]);
                                objWC.Warehouse = Convert.ToString(row["Warehouse"]);
                                //objWC.ProductionOrderNo = Convert.ToString(row["ProductionOrderNo"]);
                                objWC.WorkCenterCode = Convert.ToString(row["WorkCenterCode"]);
                                objWC.WorkCenterDescription = Convert.ToString(row["WorkCenterDescription"]);
                                objWC.strRemarks = Convert.ToString(row["Remarks"]);
                                //objWC.TotalResourceCost = Convert.ToString(row["TotalResourceCost"]);
                                objWC.PartNo = Convert.ToString(row["PartNo"]);
                                objWC.PartNoName = Convert.ToString(row["PartNoName"]);
                                objWC.DrawingheaderNo = Convert.ToString(row["DesignNo"]);
                                objWC.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                                //TempData["DetailsID"] = Convert.ToString(row["Details_ID"]);
                                objWC.Description = Convert.ToString(row["sProducts_Name"]);
                                objWC.Proj_Code = Convert.ToString(row["Proj_Code"]);
                                objWC.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);
                                objWC.ProjectID = Convert.ToString(row["Proj_Id"]);
                                ViewBag.ProjectID = Convert.ToString(row["Proj_Id"]);
                                ViewBag.Unit = Convert.ToString(row["BRANCH_ID"]);
                            }
                        }
                    }
                }
                else
                {
                    TempData["DetailsID"] = null;
                    TempData.Clear();
                }

                if (Convert.ToInt64(objWC.WorkOrderID) < 1)
                {
                    objWC.OrderDate = DateTime.Now.ToString();
                }

            }
            catch { }

            //obj.BOMDate = DateTime.Now.ToString();
            if (TempData["IsView"] != null)
            {
                ViewBag.IsView = Convert.ToInt16(TempData["IsView"]);
            }
            else
            {
                ViewBag.IsView = 0;
            }
            TempData["Count"] = 0;
            TempData.Keep();
            ViewBag.ProjectShow = ProjectSelectInEntryModule;          
            return View(objWC);
        }

        public ActionResult GetPOList()
        {
            List<ProductionOrderViewModel> list = new List<ProductionOrderViewModel>();
            try
            {
                ProductionOrderViewModel objPO = new ProductionOrderViewModel();
                // DataTable dt = lstuser.Getdesiglist();
                DataTable objData = objWO.GetWorkOrderData("GetPOList");

                // modeldesig = APIHelperMethods.ToModelList<BOMEntryViewModel>(objData);

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
                        objPO.ActualAdditionalCost = Convert.ToDecimal(row["ActualAdditionalCost"]);
                        objPO.TotalResourceCost = Convert.ToString(row["TotalResourceCost"]);
                        objPO.Order_Qty = Convert.ToDecimal(row["Order_Qty"]);
                        objPO.BRANCH_ID = Convert.ToInt64(row["BRANCH_ID"]);
                        objPO.PartNo = Convert.ToString(row["PartNo"]);
                        objPO.PartNoName = Convert.ToString(row["PartNoName"]);
                        objPO.DesignNo = Convert.ToString(row["DesignNo"]);
                        objPO.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                        objPO.Description = Convert.ToString(row["sProducts_Name"]);

                        objPO.Proj_Code = Convert.ToString(row["Proj_Code"]);
                        objPO.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);
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
            DataTable Schemadt = objdata.GetNumberingSchemaJobWorkOrder(strCompanyID, userbranchHierarchy, FinYear, "151", "Y");
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

        public ActionResult GetWorkOrderDetailsProductList(Int64 DetailsID = 0)
        {
            BOMProduct bomproductdataobj = new BOMProduct();
            List<BOMProduct> bomproductdata = new List<BOMProduct>();
            try
            {
                if (TempData["WorkOrderID"] != null)
                {
                    DetailsID = Convert.ToInt64(TempData["WorkOrderID"]);
                    TempData.Keep();
                }
                if (DetailsID > 0)
                {
                    DataTable objData = new DataTable();
                      if (TempData["WorkOrderID"] != null)
                    {
                        objData = objWO.GetWorkOrderData("GetBOMWorkOrderData", Convert.ToString(TempData["WorkOrderID"]));
                    }
                    //else if (TempData["Production_ID"] != null)
                    //{
                    //    objData = objPM.GetProductionOrderData("GetBOMProductionOrderDataWorkOrder", Convert.ToInt64(TempData["Production_ID"]), DetailsID);
                    //}
                    //else
                    //{
                    //    objData = objdata.GetBOMProductEntryListByID("GetBOMEntryProductsData", DetailsID);
                    //}
                    if (objData != null && objData.Rows.Count > 0)
                    {
                        DataTable dt = objData;
                        foreach (DataRow row in dt.Rows)
                        {
                            bomproductdataobj = new BOMProduct();
                            bomproductdataobj.SlNO = Convert.ToString(row["SlNO"]);
                            bomproductdataobj.JobWorkID = Convert.ToString(row["JobWorkID"]);
                            bomproductdataobj.BOMProductsID = Convert.ToString(row["BOMProductsID"]);
                            bomproductdataobj.Details_ID = Convert.ToString(row["Details_ID"]);
                            bomproductdataobj.ProductName = Convert.ToString(row["ProductName"]);
                            bomproductdataobj.ProductId = Convert.ToString(row["BOMProductsID"]);
                            bomproductdataobj.ProductDescription = Convert.ToString(row["ProductDescription"]);
                            bomproductdataobj.DesignNo = Convert.ToString(row["DesignNo"]);
                            bomproductdataobj.ItemRevisionNo = Convert.ToString(row["ItemRevisionNo"]);
                            bomproductdataobj.ProductQty = Convert.ToString(row["ProductQty"]);
                            bomproductdataobj.ProductUOM = Convert.ToString(row["ProductUOM"]);
                            bomproductdataobj.Warehouse = Convert.ToString(row["Warehouse"]);
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
                            bomproductdataobj.ProductsWarehouseID = Convert.ToString(row["BOMProductsID"]);
                            bomproductdataobj.GridWarehouseId = Convert.ToInt64(row["GridWarehouseId"]);
                            bomproductdataobj.UOmId = Convert.ToInt64(row["UOmId"]);
                            bomproductdataobj.RevNo = Convert.ToString(row["RevNo"]);

                            //if (TempData["Production_ID"] != null && TempData["WorkOrderID"]==null)
                            //{
                            //    bomproductdataobj.ProductQty = Convert.ToString(row["BalQty"]);
                            //    bomproductdataobj.BalQty = Convert.ToString(row["BalQty"]);
                            //    bomproductdataobj.Amount = Convert.ToString(Math.Round((Convert.ToDecimal(bomproductdataobj.BalQty) * Convert.ToDecimal(bomproductdataobj.Price)),2));
                            //}
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

                            bomproductdataobj.Remarks = Convert.ToString(row["Remarks"]);
                            bomproductdata.Add(bomproductdataobj);
                        }
                       // ViewData["BOMEntryProductsTotalAm"] = bomproductdata.Sum(x => Convert.ToDecimal(x.Amount)).ToString();
                    }
                }
            }
            catch { }
            return PartialView("_WorkOrderBOMProductGrid", bomproductdata);
        }

        public JsonResult GetJobDesignerList(Int64 ID = 0)
        {
            List<DesignList> list = new List<DesignList>();
            try
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\JobWorkOrder\DocDesign\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\JobWorkOrder\DocDesign\";
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

        public ActionResult GetWorkOrderFinishItemDetailsDetailsProductList()
        {
            FinishItemDetails FinishItemDetailsobj = new FinishItemDetails();
            List<FinishItemDetails> FinishItemDetailsproductdata = new List<FinishItemDetails>();
            Int64 DetailsID = 0;
            try
            {

                if (TempData["WorkOrderID"] != null)
                {
                    DetailsID = Convert.ToInt64(TempData["WorkOrderID"]);
                    TempData.Keep();
                }
                DataTable dt = new DataTable();
                if (DetailsID > 0 && TempData["ProductsDetails"] == null)
                {
                    DataTable objData = objWO.GetWorkOrderData("FinishItemList", Convert.ToString(DetailsID));
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
                        dtable.Columns.Add("JobWorkID", typeof(System.String));
                        dtable.Columns.Add("FinishUOMId", typeof(System.String));
                        dtable.Columns.Add("FinishProductsID", typeof(System.String));
                     
                        String Gid = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            Gid = Guid.NewGuid().ToString();
                            FinishItemDetailsobj = new FinishItemDetails();
                            FinishItemDetailsobj.SrlNO = Convert.ToString(row["SrlNO"]);
                            FinishItemDetailsobj.FinishItemName = Convert.ToString(row["FinishItemName"]);
                            FinishItemDetailsobj.FinishItemDescription = Convert.ToString(row["FinishItemDescription"]);
                            FinishItemDetailsobj.FinishDrawingNo = Convert.ToString(row["FinishDrawingNo"]);
                            FinishItemDetailsobj.FinishItemRevNo = Convert.ToString(row["FinishItemRevNo"]);
                            FinishItemDetailsobj.Qty = Convert.ToString(row["Qty"]);
                            FinishItemDetailsobj.FinishUOM = Convert.ToString(row["FinishUOM"]);
                            FinishItemDetailsobj.FinishPrice = Convert.ToString(row["FinishPrice"]);
                            FinishItemDetailsobj.FinishAmount = Convert.ToString(row["FinishAmount"]);

                            FinishItemDetailsobj.JobWorkID = Convert.ToString(row["JobWorkID"]);
                            FinishItemDetailsobj.FinishUOMId = Convert.ToString(row["FinishUOMId"]);
                            FinishItemDetailsobj.FinishProductsID = Convert.ToString(row["FinishProductsID"]);
                            FinishItemDetailsobj.Guids = Gid;

                            FinishItemDetailsproductdata.Add(FinishItemDetailsobj);


                            object[] trow = { Gid, row["SrlNO"],Convert.ToString(row["FinishItemName"]),Convert.ToString(row["FinishItemDescription"]),
                                                Convert.ToString(row["FinishDrawingNo"]),Convert.ToString(row["FinishItemRevNo"]),Convert.ToString(row["Qty"]),
                                   Convert.ToString(row["FinishUOM"]),Convert.ToString(row["FinishPrice"]),Convert.ToString(row["FinishAmount"]),"1",DetailsID,
                                    Convert.ToString(row["FinishUOMId"]),Convert.ToString(row["FinishProductsID"]) };
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
                           
                            FinishItemDetailsobj = new FinishItemDetails();
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
                           
                            FinishItemDetailsproductdata.Add(FinishItemDetailsobj);
                        }
                       
                    }
                }
                TempData["ProductsDetails"] = dt;
                TempData.Keep();

            }
            catch { }

            return PartialView("_JobFinishItemProductEntryGrid", FinishItemDetailsproductdata);
            //return PartialView("~/Views/PMS/Estimate/EstimateProductList.cshtml", FinishItemDetailsproductdata);
        }


        [ValidateInput(false)]
        public ActionResult BatchEditingWorkOrder(DevExpress.Web.Mvc.MVCxGridViewBatchUpdateValues<BOMProduct, int> updateValues, JobWorkOrderViewModel options)
        {
            TempData["Count"] = (int)TempData["Count"] + 1;
            TempData.Keep();
            String NumberScheme = "";
            String Message = "";

            if ((int)TempData["Count"] != 2)
            {
                Boolean IsProcess = false;
                List<BOMProduct> list = new List<BOMProduct>();
                List<udtJobProductionOrderDetails> udtlist1 = new List<udtJobProductionOrderDetails>();
                List<udtJobProductionOrderDetails> udtlist = new List<udtJobProductionOrderDetails>();
                if (updateValues.Insert.Count > 0 && Convert.ToInt64(options.Details_ID) < 1)
                {
                   
                    
                    udtJobProductionOrderDetails obj = null;
                  
                    updateValues.Insert = updateValues.Insert.OrderBy(x => x.SlNO).ToList();
                    foreach (var item in updateValues.Insert)
                    {
                        if (Convert.ToInt64(item.BOMProductsID) > 0)
                        {
                            

                         
                            obj = new udtJobProductionOrderDetails();
                            obj.SlNO = Convert.ToString(item.SlNO);
                            obj.JobWorkID = Convert.ToString(item.JobWorkID);
                            obj.Details_ID = Convert.ToString(item.Details_ID);
                            obj.ProductsID = Convert.ToString(item.BOMProductsID);
                            obj.Description = Convert.ToString(item.ProductDescription);
                            obj.DrawingNo = Convert.ToString(item.DesignNo);
                            obj.DrawingRevNo = Convert.ToString(item.ItemRevisionNo);
                            obj.UOM = Convert.ToString(item.UOmId);
                            obj.Warehouse = Convert.ToString(item.GridWarehouseId);
                            obj.Price = Convert.ToString(item.Price);
                            obj.Qty = Convert.ToString(item.ProductQty);
                            obj.Amount = Convert.ToString(item.Amount);
                            obj.Remarks = Convert.ToString(item.Remarks);
                           

                            udtlist.Add(obj);
                        }
                    }
                    //if (udtlist.Count > 0)
                    //{
                    //   // SaveDataArea = 1;
                    //    //if (options.BOMNo)
                    //    NumberScheme = checkNMakePOCode(options.OrderNo, Convert.ToInt32(options.Order_SchemaID), Convert.ToDateTime(options.OrderDate));
                    //    if (NumberScheme == "ok")
                    //    {
                    //        udtlist = udtlist.OrderBy(x => x.SlNO).ToList();
                    //        foreach (var item in udtlist)
                    //        {
                    //            udtJobProductionOrderDetails obj1 = new udtJobProductionOrderDetails();
                    //            obj1.JobWorkID = Convert.ToString(item.JobWorkID);
                    //            obj1.Details_ID = Convert.ToString(item.Details_ID);
                    //            obj1.ProductsID = Convert.ToString(item.ProductsID);
                    //            obj1.Description = Convert.ToString(item.Description);
                    //            obj1.DrawingNo = Convert.ToString(item.DrawingNo);
                    //            obj1.DrawingRevNo = Convert.ToString(item.DrawingRevNo);
                    //            obj1.UOM = Convert.ToString(item.UOM);
                    //            obj1.Warehouse = Convert.ToString(item.Warehouse);
                    //            obj1.Price = Convert.ToString(item.Price);
                    //            obj1.Qty = Convert.ToString(item.Qty);
                    //            obj1.Amount = Convert.ToString(item.Amount);

                    //            udtlist1.Add(obj1);
                    //        }
                    //        IsProcess = WorkOrderBOMProductInsertUpdate(udtlist1, options);
                    //    }
                    //    else
                    //    {
                    //        Message = NumberScheme;
                    //    }
                    //}

                    udtlist = udtlist.OrderBy(x => x.SlNO).ToList();
                 
                }
            
                if (updateValues.Update.Count > 0)
                {

                    udtJobProductionOrderDetails obj = null;

                    foreach (var item in updateValues.Update)
                    {
                        if (Convert.ToInt64(item.BOMProductsID) > 0)
                        {
                            obj = new udtJobProductionOrderDetails();
                            obj.JobWorkID = Convert.ToString(item.JobWorkID);
                            obj.Details_ID = Convert.ToString(item.Details_ID);
                            obj.ProductsID = Convert.ToString(item.BOMProductsID);
                            obj.Description = Convert.ToString(item.ProductDescription);
                            obj.DrawingNo = Convert.ToString(item.DesignNo);
                            obj.DrawingRevNo = Convert.ToString(item.ItemRevisionNo);
                            obj.UOM = Convert.ToString(item.UOmId);
                            obj.Warehouse = Convert.ToString(item.GridWarehouseId);
                            obj.Price = Convert.ToString(item.Price);
                            obj.Qty = Convert.ToString(item.ProductQty);
                            obj.Amount = Convert.ToString(item.Amount);
                            obj.Remarks = Convert.ToString(item.Remarks);
                            udtlist.Add(obj);
                        }
                    }
                    //if (udtlist.Count > 0)
                    //{
                    //    if (Convert.ToInt64(options.WorkOrderID) > 0)
                    //    {
                    //        IsProcess = WorkOrderBOMProductInsertUpdate(udtlist, options);
                    //    }
                    //    else
                    //    {
                    //        NumberScheme = checkNMakePOCode(options.OrderNo, Convert.ToInt32(options.Order_SchemaID), Convert.ToDateTime(options.OrderDate));
                    //        if (NumberScheme == "ok")
                    //        {
                    //            IsProcess = WorkOrderBOMProductInsertUpdate(udtlist, options);
                    //        }
                    //        else
                    //        {
                    //            Message = NumberScheme;
                    //        }
                    //    }

                    //    TempData["DetailsID"] = null;
                    //}
                }
            
                if (udtlist.Count > 0)
                {
                    
                    
                        foreach (var item in udtlist)
                        {
                            udtJobProductionOrderDetails obj1 = new udtJobProductionOrderDetails();
                            obj1.JobWorkID = Convert.ToString(item.JobWorkID);
                            obj1.Details_ID = Convert.ToString(item.Details_ID);
                            obj1.ProductsID = Convert.ToString(item.ProductsID);
                            obj1.Description = Convert.ToString(item.Description);
                            obj1.DrawingNo = Convert.ToString(item.DrawingNo);
                            obj1.DrawingRevNo = Convert.ToString(item.DrawingRevNo);
                            obj1.UOM = Convert.ToString(item.UOM);
                            obj1.Warehouse = Convert.ToString(item.Warehouse);
                            obj1.Price = Convert.ToString(item.Price);
                            obj1.Qty = Convert.ToString(item.Qty);
                            obj1.Amount = Convert.ToString(item.Amount);
                            obj1.Remarks = Convert.ToString(item.Remarks);

                            udtlist1.Add(obj1);
                        }

                    if (Convert.ToInt64(options.WorkOrderID) > 0)
                    {
                        IsProcess = WorkOrderBOMProductInsertUpdate(udtlist1, options,"","");
                    }
                    else
                    {
                        NumberScheme = "ok";
                        if (NumberScheme == "ok")
                        {
                            IsProcess = WorkOrderBOMProductInsertUpdate(udtlist1, options, options.OrderNo, Convert.ToString(options.Order_SchemaID));
                        }
                        else
                        {
                            Message = NumberScheme;
                        }
                    }
                   
                }



               TempData["Count"] = 1;
                TempData.Keep();
                ViewData["OrderNo"] = JVNumStr;
                ViewData["Success"] = IsProcess;
                ViewData["Message"] = Message;
           }
            return PartialView("_WorkOrderBOMProductGrid", updateValues.Update);
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
                        sqlQuery = "SELECT max(tjv.OrderNo) FROM JobWorkOrder tjv WHERE dbo.RegexMatch('";
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
                            sqlQuery = "SELECT max(tjv.OrderNo) FROM JobWorkOrder tjv WHERE dbo.RegexMatch('";
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
                            sqlQuery = "SELECT max(tjv.OrderNo) FROM JobWorkOrder tjv WHERE dbo.RegexMatch('";
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


                                sqlQuery = "SELECT max(tjv.OrderNo) FROM JobWorkOrder tjv WHERE dbo.RegexMatch('";
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
                                sqlQuery = "SELECT max(tjv.OrderNo) FROM JobWorkOrder tjv WHERE dbo.RegexMatch('";
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
                            sqlQuery = "SELECT max(tjv.OrderNo) FROM JobWorkOrder tjv WHERE dbo.RegexMatch('";
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
                    sqlQuery = "SELECT OrderNo FROM JobWorkOrder WHERE OrderNo LIKE '" + manual_str.Trim() + "'";
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


        public Boolean WorkOrderBOMProductInsertUpdate(List<udtJobProductionOrderDetails> obj, JobWorkOrderViewModel obj2,string Order_id,string Schema_id)
        {
            Boolean Success = false;


            try
            {
                DataTable dtBOM_PRODUCTS = new DataTable();
                dtBOM_PRODUCTS = ToDataTable(obj);

                if(dtBOM_PRODUCTS.Columns.Contains("SlNO"))
                {
                    dtBOM_PRODUCTS.Columns.Remove("SlNO");
                    dtBOM_PRODUCTS.AcceptChanges();
                }

                DataTable dtProductFinish = new DataTable();
                dtProductFinish=(DataTable)TempData["ProductsDetails"];



                List<FinishItemDetails> udt = new List<FinishItemDetails>();
                if (dtProductFinish != null && dtProductFinish.Rows.Count > 0)
                {
                    foreach (DataRow item in dtProductFinish.Rows)
                    {
                        FinishItemDetails obj1 = new FinishItemDetails();
                        obj1.SrlNO = Convert.ToString(item["SrlNO"]);
                        obj1.FinishItemName = Convert.ToString(item["FinishItemName"]);
                        obj1.FinishItemDescription = Convert.ToString(item["FinishItemDescription"]);
                        obj1.FinishDrawingNo = Convert.ToString(item["FinishDrawingNo"]);
                        obj1.FinishItemRevNo = Convert.ToString(item["FinishItemRevNo"]);
                        obj1.Qty = Convert.ToString(item["Qty"]);
                        if (obj1.Qty=="")
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

                        obj1.JobWorkID = Convert.ToString(item["JobWorkID"]);
                        if (obj1.JobWorkID == "")
                        {
                            obj1.JobWorkID = "0";
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
                        udt.Add(obj1);
                    }

                }

                DataTable dt_PRODUCTS = new DataTable();
                dt_PRODUCTS = ToDataTable(udt);
                if(dt_PRODUCTS.Columns.Contains("Guids"))
                {
                    dt_PRODUCTS.Columns.Remove("Guids");
                    dt_PRODUCTS.AcceptChanges();
                }

                if (dt_PRODUCTS.Columns.Contains("FinishUpdateEdit"))
                {
                    dt_PRODUCTS.Columns.Remove("FinishUpdateEdit");
                    dt_PRODUCTS.AcceptChanges();
                }

                DataSet dt = new DataSet();
                //if (Convert.ToInt64(obj2.Details_ID) > 0)
                //{
                    if (!String.IsNullOrEmpty(obj2.OrderNo) && obj2.OrderNo.ToLower() != "auto")
                    {
                        JVNumStr = obj2.OrderNo;
                    }
                    dt = objWO.WorkOrderBOMProductInsertUpdate("INSERTWORKORDERBOM", obj2.WorkOrderID, obj2.Production_ID, Convert.ToString(obj2.WorkCenterID), JVNumStr, obj2.Order_SchemaID, Convert.ToDateTime(obj2.OrderDate),
                        obj2.Order_Qty, obj2.ActualAdditionalCost, obj2.TotalCost,obj2.BRANCH_ID, Convert.ToString(Session["userid"]),obj2.strRemarks,obj2.PartNo,
                        dtBOM_PRODUCTS, obj2.Description, obj2.DrawingheaderNo, obj2.ItemRevNo, obj2.ProjectID, obj2.JobWorkRate, dt_PRODUCTS, Convert.ToString(Session["LastFinYear"]),Order_id,Schema_id);
                    //}
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
                        JVNumStr = Convert.ToString(row["OrderNumber"]);
                    }
                }
            }
            catch { }
            return Success;
        }

        public JsonResult ClosedJobDataByID(Int64 detailsid, String ClosedJobRemarks = "")
        {
            ReturnDataClosed obj = new ReturnDataClosed();
            try
            {
                DataTable datasetobj = objWO.ClosedWorkOrderData("ClosedJobData", detailsid, ClosedJobRemarks);
                if (datasetobj != null && datasetobj.Rows.Count > 0)
                {
                        obj.Success = Convert.ToInt64(datasetobj.Rows[0]["Status"]);
                     
                }
            }
            catch { }
            return Json(obj);
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

        public JsonResult SetTempID(Int64 DetailsID, Int64 Production_ID)
        {
            if (DetailsID > 0)
            {
                TempData["DetailsID"] = DetailsID;
                TempData["Production_ID"] = Production_ID;
                TempData.Keep();
            }
            else
            {
                TempData["DetailsID"] = null;
                TempData["Production_ID"] = null;
                TempData.Clear();
            }
            return Json(true);
        }

        public ActionResult WorkOrderList()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/WorkorderList", "JobWorkOrder");
            JobWorkOrderViewModel obj = new JobWorkOrderViewModel();
            TempData.Clear();
            ViewBag.CanAdd = rights.CanAdd;
            return View("WorkOrderList", obj);
        }

        public ActionResult GetWorkOrderList()
        {
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            List<JobWorkOrderViewModel> list = new List<JobWorkOrderViewModel>();
            DataTable dt = new DataTable();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/WorkorderList", "JobWorkOrder");
            try
            {
                Int64 BranchID = 0;
                DateTime? FromDate = null;
                DateTime? ToDate = null;
             
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
                        dt = oDBEngine.GetDataTable("select * from V_JobWorkOrderList where BRANCH_ID =" + BranchID + " AND (OrderDate BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') order by WorkOrderID  desc ");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("select * from V_JobWorkOrderList where OrderDate BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' order by WorkOrderID  desc ");
                    }

                }
                //else
                //{
                //    dt = oDBEngine.GetDataTable("select * from V_ProductionOrderList");
                //}

                
                TempData["DetailsListDataTable"] = dt;

                //var dt = oDBEngine.GetDataTable("select * from V_ProductionOrderList");
               // if (dt.Rows.Count > 0)
                //{
                //    JobWorkOrderViewModel obj = new JobWorkOrderViewModel();
                //    foreach (DataRow item in dt.Rows)
                //    {
                //        obj = new JobWorkOrderViewModel();
                //        obj.WorkOrderID = Convert.ToString(item["WorkOrderID"]);
                      
                //        obj.OrderNo = Convert.ToString(item["OrderNo"]);
                //        obj.Order_Qty = Convert.ToString(item["Order_Qty"]);
                //        obj.WorkCenterCode = Convert.ToString(item["WorkCenterCode"]);
                //        obj.WorkCenterDescription = Convert.ToString(item["WorkCenterDescription"]);
                //        obj.BRANCH_ID = Convert.ToString(item["BRANCH_ID"]);
                       

                //        if (Convert.ToString(item["OrderDate"]) != "")
                //        {
                //            obj.OrderDate = Convert.ToDateTime(item["OrderDate"]).ToString("dd-MM-yyyy");
                //        }
                //        else
                //        {
                //            obj.OrderDate = null;
                //        }


                //        obj.CreatedBy = Convert.ToString(item["CreatedBy"]);
                //        obj.ModifyBy = Convert.ToString(item["ModifyBy"]);

                //        if (Convert.ToString(item["CreateDate"]) != "")
                //        {
                //            obj.CreateDate = Convert.ToDateTime(item["CreateDate"]);
                //        }
                //        else
                //        {
                //            obj.CreateDate = null;
                //        }

                       

                //        if (Convert.ToString(item["ModifyDate"]) != "")
                //        {
                //            obj.ModifyDate = Convert.ToDateTime(item["ModifyDate"]);
                //        }
                //        else
                //        {
                //            obj.ModifyDate = null;
                //        }
                //        obj.PartNoName = Convert.ToString(item["PartNoName"]);
                //        obj.DesignNo = Convert.ToString(item["DesignNo"]);
                //        obj.ItemRevNo = Convert.ToString(item["ItemRevisionNo"]);
                //        obj.Description = Convert.ToString(item["sProducts_Name"]);
                //        obj.Proj_Code = Convert.ToString(item["Proj_Code"]);
                //        obj.Proj_Name = Convert.ToString(item["Proj_Name"]);
                //        list.Add(obj);
                //    }
                //}
            }
            catch { }
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.CanClose = rights.CanClose;
            ViewBag.CanPrint = rights.CanPrint;
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            return PartialView("_WorkOrderDataList", dt);
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
        public ActionResult GetProjectCode(BOMEntryViewModel model, string Project_ID, String Branchs, String Hierarchy)
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
                //if (BOM_ID != null && BOM_ID != "")
                //{
                //    dtProj = objdata.GetProjectCodeFromBOM(Branch, BOM_ID);
                //}
                //else
                //{
                    dtProj = objdata.GetProjectCode(Branch);
               // }
                List<ProjectList> modelProj = new List<ProjectList>();
                modelProj = APIHelperMethods.ToModelList<ProjectList>(dtProj);
                ViewBag.ProjectID = Project_ID;
                ViewBag.Hierarchy = Hierarchy;

                return PartialView("~/Views/JobWorkOrder/_PartialProjectCode.cshtml", modelProj);
            }
            catch
            {
                return RedirectToAction("Logout", "Login", new { Area = "" });
            }
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
            settings.Name = "Job Work Order";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Job Work Order";
            //String ID = Convert.ToString(TempData["EmployeesTargetListDataTable"]);
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "OrderNo" || datacolumn.ColumnName == "OrderDate" || datacolumn.ColumnName == "PartNoName" || datacolumn.ColumnName == "Description"
                    || datacolumn.ColumnName == "DesignNo" || datacolumn.ColumnName == "ItemRevNo" || datacolumn.ColumnName == "Proj_Code" || datacolumn.ColumnName == "CreatedBy" || datacolumn.ColumnName == "CreateDate" || datacolumn.ColumnName == "ModifyBy" || datacolumn.ColumnName == "ModifyDate"
                    )
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "OrderNo")
                        {
                            column.Caption = "Work Order No";
                            column.VisibleIndex = 0;
                        }
                        else if (datacolumn.ColumnName == "OrderDate")
                        {
                            column.Caption = "Order Date";
                            column.VisibleIndex = 1;
                        }
                        else if (datacolumn.ColumnName == "PartNoName")
                        {
                            column.Caption = "Item No.";
                            column.VisibleIndex = 2;
                        }
                        else if (datacolumn.ColumnName == "Description")
                        {
                            column.Caption = "Description";
                            column.VisibleIndex = 3;
                        }

                        else if (datacolumn.ColumnName == "DesignNo")
                        {
                            column.Caption = "Drawing No.";
                            column.VisibleIndex = 4;
                        }
                        else if (datacolumn.ColumnName == "ItemRevNo")
                        {
                            column.Caption = "Drawing Rev. No";
                            column.VisibleIndex = 5;
                        }
                        else if (datacolumn.ColumnName == "Proj_Code")
                        {
                            column.Caption = "Project Code";
                            column.VisibleIndex = 6;

                        }
                        else if (datacolumn.ColumnName == "CreatedBy")
                        {
                            column.Caption = "CreatedBy";
                            column.VisibleIndex = 7;

                        }
                        else if (datacolumn.ColumnName == "CreateDate")
                        {
                            column.Caption = "CreateDate";
                            column.VisibleIndex = 8;

                        }
                        else if (datacolumn.ColumnName == "ModifyBy")
                        {
                            column.Caption = "CreatedBy";
                            column.VisibleIndex = 9;

                        }
                        else if (datacolumn.ColumnName == "ModifyDate")
                        {
                            column.Caption = "CreateDate";
                            column.VisibleIndex = 10;

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

        public JsonResult SetWODataByID(Int64 workorderid = 0, Int16 IsView = 0)
        {
            Boolean Success = false;
            try
            {
                TempData["WorkOrderID"] = workorderid;
                TempData["IsView"] = IsView;
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }

        public JsonResult RemoveWODataByID(Int32 workorderid)
        {
            //Boolean Success = false;
            ReturnData obj = new ReturnData();
            try
            {
                var datasetobj = objWO.GetWorkOrderdeleteData("RemoveWOData", Convert.ToString(workorderid), Convert.ToString(Session["userid"]));
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

        [WebMethod]
        public JsonResult AddProduct(FinishItemDetails prod)
        {
            DataTable dt = (DataTable)TempData["ProductsDetails"];
            DataTable dt2 = new DataTable();
            int duplicateId = 0;

            // Mantis Issue 24855
            //if (dt == null)
            if (dt == null || dt.Rows.Count==0)
            // End of Mantis Issue 24855
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
                dtable.Columns.Add("JobWorkID", typeof(System.String));
                dtable.Columns.Add("FinishUOMId", typeof(System.String));
                dtable.Columns.Add("FinishProductsID", typeof(System.String));

                object[] trow = { Guid.NewGuid(), 1,prod.FinishItemName,prod.FinishItemDescription,prod.FinishDrawingNo,prod.FinishItemRevNo,prod.Qty,prod.FinishUOM,prod.FinishPrice,prod.FinishAmount,prod.FinishUpdateEdit,
                                    prod.JobWorkID,prod.FinishUOMId,prod.FinishProductsID };
                dtable.Rows.Add(trow);
                TempData["ProductsDetails"] = dtable;
                TempData.Keep();
            }
            else
            {
                if (string.IsNullOrEmpty(prod.Guids))
                {
                    foreach(DataRow dtw  in dt.Rows)
                   {
                       string FProductId = Convert.ToString(dtw["FinishProductsID"]);
                       if (Convert.ToString(prod.FinishProductsID) != FProductId)
                       {
                           object[] trow = { Guid.NewGuid(), Convert.ToInt32(dt.Rows.Count)+1,prod.FinishItemName,prod.FinishItemDescription,prod.FinishDrawingNo,prod.FinishItemRevNo,prod.Qty,prod.FinishUOM,prod.FinishPrice,prod.FinishAmount,prod.FinishUpdateEdit,
                                    prod.JobWorkID,prod.FinishUOMId,prod.FinishProductsID };// Add new parameter Here
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
                                item["JobWorkID"] = prod.JobWorkID;
                                item["FinishUOMId"] = prod.FinishUOMId;
                                item["FinishProductsID"] = prod.FinishProductsID;
                               
                            }
                        }
                    }
                }
                TempData["ProductsDetails"] = dt;
                TempData.Keep();
            }


            if (duplicateId == 1)
            {
                return Json("Duplicate");
            }
            else
            {
                return Json("");
            }
            //return Json("");
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

        [WebMethod]
        public JsonResult EditData(String HiddenID)
        {
            FinishItemDetails ret = new FinishItemDetails();

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
                        ret.JobWorkID = item["JobWorkID"].ToString();
                        ret.FinishUOMId = item["FinishUOMId"].ToString();
                        ret.FinishProductsID = item["FinishProductsID"].ToString();
                       
                        break;
                    }
                }
            }
            TempData["ProductsDetails"] = dt;
            TempData.Keep();
            return Json(ret);
        }

	}
}