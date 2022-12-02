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

namespace Manufacturing.ControllersGetPIList
{
    public class ProductionReceiptController : Controller
    {
        BOMEntryModel objdata = null;
        ProductionReceiptViewModel objWC = null;
        DBEngine oDBEngine = new DBEngine();
        WorkOrderModel objWO = null;
        string JVNumStr = string.Empty;
        UserRightsForPage rights = new UserRightsForPage();
        ProductionOrderModel objPM = null;
        IssueReturnModel objIR = null;
        ProductionIssueModel objPI = null;
        ProductionReceiptModel objPR = null;
        CommonBL cSOrder = new CommonBL();

        string ReceiptNo = string.Empty;
        public ProductionReceiptController()
        {
            objdata = new BOMEntryModel();
            objWC = new ProductionReceiptViewModel();
            objWO = new WorkOrderModel();
            objPM = new ProductionOrderModel();
            objIR = new IssueReturnModel();
            objPI = new ProductionIssueModel();
            objPR = new ProductionReceiptModel();
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
                        DataTable objData = objPR.GetProductionReceiptData("GetProductionReceiptData", objWC.ProductionReceiptID, 0);
                        if (objData != null && objData.Rows.Count > 0)
                        {
                            DataTable dt = objData;
                            foreach (DataRow row in dt.Rows)
                            {
                                objWC.ProductionOrderID = Convert.ToInt64(row["ProductionOrderID"]);
                                objWC.ProductionIssueID = Convert.ToInt64(row["ProductionIssueID"]);
                                objWC.WorkOrderID = Convert.ToInt64(row["WorkOrderID"]);
                                objWC.Production_ID = Convert.ToInt64(row["ProductionOrderID"]);
                                objWC.WorkCenterID = Convert.ToString(row["WorkCenterID"]);
                                objWC.OrderNo = Convert.ToString(row["Receipt_No"]);
                                objWC.Order_SchemaID = Convert.ToInt64(row["Receipt_SchemaID"]);
                                objWC.OrderDate = Convert.ToDateTime(row["Receipt_Date"]).ToString("dd-MM-yyyy");
                                objWC.dtOrderDate = Convert.ToDateTime(row["Receipt_Date"]);
                                objWC.BRANCH_ID = Convert.ToInt64(row["BRANCH_ID"]);
                                objWC.Order_Qty = Convert.ToDecimal(row["Receipt_Qty"]);
                                objWC.BOMNo = Convert.ToString(row["BOM_No"]);
                                objWC.RevNo = Convert.ToString(row["REV_No"]);
                                objWC.FinishedItem = Convert.ToString(row["ProductName"]);
                                objWC.FinishedUom = Convert.ToString(row["FinishedUom"]);
                                objWC.Finished_Qty = Convert.ToDecimal(row["Finished_Qty"]);
                                objWC.Warehouse = Convert.ToString(row["Warehouse"]);
                                objWC.ProductionOrderNo = Convert.ToString(row["ProductionOrderNo"]);
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
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            TempData["Count"] = 1;
            TempData.Keep();
            return View(objWC);
            
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
            DataTable Schemadt = objdata.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "104", "Y");
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
            BOMProduct bomproductdataobj = new BOMProduct();
            List<BOMProduct> bomproductdata = new List<BOMProduct>();
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
                        objData = objPI.GetProductionIssueData("GetBOMProductionIssueData", Convert.ToInt64(TempData["ProductionIssueID"]), DetailsID);
                    }
                    else if (TempData["ProductionReceiptID"] != null)
                    {
                        objData = objPR.GetProductionReceiptData("GetBOMProductionReceiptData", Convert.ToInt64(TempData["ProductionReceiptID"]), DetailsID);
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

                            if (TempData["Production_ID"] != null && TempData["WorkOrderID"] == null)
                            {
                                bomproductdataobj.ProductQty = Convert.ToString(row["BalQty"]);
                                bomproductdataobj.BalQty = Convert.ToString(row["BalQty"]);
                                bomproductdataobj.Amount = Convert.ToString(Math.Round((Convert.ToDecimal(bomproductdataobj.BalQty) * Convert.ToDecimal(bomproductdataobj.Price)), 2));
                            }
                            else
                            {
                                bomproductdataobj.BalQty = Convert.ToString(row["BalQty"]);
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


        [ValidateInput(false)]
        public ActionResult BatchEditingProductionReceipt(DevExpress.Web.Mvc.MVCxGridViewBatchUpdateValues<BOMProduct, int> updateValues, ProductionReceiptViewModel options)
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
                            //NumberScheme = checkNMakePOCode(options.OrderNo, Convert.ToInt32(options.Order_SchemaID), Convert.ToDateTime(options.OrderDate));
                            //if (NumberScheme == "ok")
                            //{
                                IsProcess = ProductionReceiptBOMProductInsertUpdate(udtlist, options);
                            //}
                            //else
                            //{
                            //    Message = NumberScheme;
                            //}
                        }

                        TempData["DetailsID"] = null;
                        TempData["ProductionIssueID"] = null;
                    }
                }


                TempData["Count"] = 1;
                TempData.Keep();
                ViewData["OrderNo"] = ReceiptNo;
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
                        sqlQuery = "SELECT max(tjv.Receipt_No) FROM ProductionReceipt tjv WHERE dbo.RegexMatch('";
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
                            sqlQuery = "SELECT max(tjv.Receipt_No) FROM ProductionReceipt tjv WHERE dbo.RegexMatch('";
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
                            sqlQuery = "SELECT max(tjv.Receipt_No) FROM ProductionReceipt tjv WHERE dbo.RegexMatch('";
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


                                sqlQuery = "SELECT max(tjv.Receipt_No) FROM ProductionReceipt tjv WHERE dbo.RegexMatch('";
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
                                sqlQuery = "SELECT max(tjv.Receipt_No) FROM ProductionReceipt tjv WHERE dbo.RegexMatch('";
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
                            sqlQuery = "SELECT max(tjv.Receipt_No) FROM ProductionReceipt tjv WHERE dbo.RegexMatch('";
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
                    sqlQuery = "SELECT Receipt_No FROM ProductionReceipt WHERE Receipt_No LIKE '" + manual_str.Trim() + "'";
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


        public Boolean ProductionReceiptBOMProductInsertUpdate(List<udtProductionOrderDetails> obj, ProductionReceiptViewModel obj2)
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
                    dt = objPR.ProductionReceiptBOMProductInsertUpdate("INSERTRECEIPTBOM", obj2.ProductionReceiptID, obj2.ProductionIssueID, obj2.ProductionOrderID, obj2.Details_ID, obj2.WorkOrderID, obj2.Production_ID, Convert.ToInt64(obj2.WorkCenterID), JVNumStr, obj2.Order_SchemaID, Convert.ToDateTime(obj2.OrderDate),
                        obj2.Order_Qty, obj2.ActualAdditionalCost, obj2.TotalCost, obj2.BRANCH_ID, Convert.ToInt64(Session["userid"]), obj2.strRemarks, obj2.FGPrice,obj2.TotalAmount,
                        dtBOM_PRODUCTS);
                }
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        Success = Convert.ToBoolean(row["Success"]);
                        ReceiptNo = Convert.ToString(row["ReceiptNo"]);
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

        public ActionResult ProductionReceiptList()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ProductionReceiptList", "ProductionReceipt");
            ProductionReceiptViewModel obj = new ProductionReceiptViewModel();
            TempData.Clear();
            ViewBag.CanAdd = rights.CanAdd;
            return View("ProductionReceiptList", obj);
        }

        public ActionResult GetProductionReceiptList()
        {
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            List<ProductionReceiptViewModel> list = new List<ProductionReceiptViewModel>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/ProductionReceiptList", "ProductionReceipt");
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
                        dt = oDBEngine.GetDataTable("select * from V_ProductionReceiptList where BRANCH_ID =" + BranchID + " AND (Receipt_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') ");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("select * from V_ProductionReceiptList where Receipt_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' ");
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
                    ProductionReceiptViewModel obj = new ProductionReceiptViewModel();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new ProductionReceiptViewModel();
                        obj.WorkOrderID = Convert.ToInt64(item["WorkOrderID"]);
                        // obj.ProductionOrderID = Convert.ToInt64(item["ProductionOrderID"]);
                        obj.ProductionReceiptID = Convert.ToInt64(item["ProductionReceiptID"]);
                        obj.WorkOrderID = Convert.ToInt64(item["WorkOrderID"]);
                        obj.ProductionOrderID = Convert.ToInt64(item["ProductionOrderID"]);
                        obj.WorkCenterID = Convert.ToString(item["WorkCenterID"]);
                        obj.WorkCenterID = Convert.ToString(item["WorkCenterID"]);
                        obj.Receipt_No = Convert.ToString(item["Receipt_No"]);
                        obj.Receipt_Date = Convert.ToDateTime(item["Receipt_Date"]);
                        obj.ProductionIssueID = Convert.ToInt64(item["ProductionIssueID"]);
                        obj.ProductionIssueNo = Convert.ToString(item["ProductionIssueNo"]);
                        obj.ProductionIssueDate = Convert.ToDateTime(item["ProductionIssueDate"]);
                        obj.WorkOrderNo = Convert.ToString(item["WorkOrderNo"]);
                        obj.Order_Qty = Convert.ToDecimal(item["Receipt_Qty"]);
                        obj.WorkCenterCode = Convert.ToString(item["WorkCenterCode"]);
                        obj.WorkCenterDescription = Convert.ToString(item["WorkCenterDescription"]);
                        obj.BRANCH_ID = Convert.ToInt64(item["BRANCH_ID"]);
                        obj.BOMNo = Convert.ToString(item["BOM_No"]);
                        obj.RevNo = Convert.ToString(item["REV_No"]);
                        obj.BOM_Date = Convert.ToDateTime(item["BOM_Date"]);
                        obj.ProductionOrderNo = Convert.ToString(item["ProductionOrderNo"]);
                        obj.ProductionOrderDate = Convert.ToDateTime(item["ProductionOrderDate"]);
                        obj.WorkOrderDate = Convert.ToDateTime(item["WorkOrderDate"]);
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
            return PartialView("_ProductionReceiptDataList", list);
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
                    datacolumn.ColumnName == "WorkOrderNo" || datacolumn.ColumnName == "WorkOrderDate" || datacolumn.ColumnName == "ProductionOrderNo" || datacolumn.ColumnName == "ProductionOrderDate"
                    || datacolumn.ColumnName == "BOM_No" || datacolumn.ColumnName == "BOM_Date" || datacolumn.ColumnName == "REV_No" || datacolumn.ColumnName == "REV_Date"||
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

                        if (datacolumn.ColumnName == "WorkOrderNo")
                        {
                            column.Caption = "Work Order No";
                            column.VisibleIndex = 4;
                        }
                        else if (datacolumn.ColumnName == "WorkOrderDate")
                        {
                            column.Caption = "Work Order Date";
                            column.VisibleIndex = 5;
                        }
                        else if (datacolumn.ColumnName == "ProductionOrderNo")
                        {
                            column.Caption = "Production Order No";
                            column.VisibleIndex = 6;
                        }
                        else if (datacolumn.ColumnName == "ProductionOrderDate")
                        {
                            column.Caption = "Production Order Date";
                            column.VisibleIndex = 7;
                        }
                        else if (datacolumn.ColumnName == "BOM_No")
                        {
                            column.Caption = "BOM No";
                            column.VisibleIndex = 8;

                        }
                        else if (datacolumn.ColumnName == "BOM_Date")
                        {
                            column.Caption = "BOM Date";
                            column.VisibleIndex = 9;
                        }
                        else if (datacolumn.ColumnName == "REV_No")
                        {
                            column.Caption = "Rev No.";
                            column.VisibleIndex = 10;
                        }
                        else if (datacolumn.ColumnName == "REV_Date")
                        {
                            column.Caption = "Rev Date";
                            column.VisibleIndex = 11;
                        }
                        else if (datacolumn.ColumnName == "CreatedBy")
                        {
                            column.Caption = "Entered By";
                            column.VisibleIndex = 12;
                        }
                        else if (datacolumn.ColumnName == "CreateDate")
                        {
                            column.Caption = "Entered On";
                            column.VisibleIndex = 13;
                        }
                        else if (datacolumn.ColumnName == "ModifyBy")
                        {
                            column.Caption = "Modified By";
                            column.VisibleIndex = 14;
                        }
                        else if (datacolumn.ColumnName == "ModifyDate")
                        {
                            column.Caption = "Modified On";
                            column.VisibleIndex = 15;
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
                var datasetobj = objPR.GetProductionReceiptData("RemovePRData", productionreceiptid, 0);
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

        public ActionResult GetPIList()
        {
            List<IssueReturnViewModel> list = new List<IssueReturnViewModel>();
            try
            {
                IssueReturnViewModel objWOL = new IssueReturnViewModel();
                DataTable objData = objIR.GetIssueReturnData("GetAllProductionIssueReciptModuleData", 0, 0);

                if (objData != null && objData.Rows.Count > 0)
                {
                    DataTable dt = objData;
                    foreach (DataRow row in dt.Rows)
                    {
                        objWOL = new IssueReturnViewModel();
                        objWOL.ProductionIssueID = Convert.ToInt64(row["ProductionIssueID"]);
                        objWOL.Issue_No = Convert.ToString(row["Issue_No"]);
                        objWOL.WorkOrderID = Convert.ToInt64(row["WorkOrderID"]);
                        objWOL.OrderNo = Convert.ToString(row["OrderNo"]);
                        objWOL.ProductionOrderNo = Convert.ToString(row["ProductionOrderNo"]);
                        objWOL.ProductionOrderID = Convert.ToInt64(row["ProductionOrderID"]);
                        objWOL.Details_ID = Convert.ToInt64(row["Details_ID"]);
                        objWOL.BOMNo = Convert.ToString(row["BOM_No"]);
                        objWOL.Issue_Date = Convert.ToDateTime(row["Issue_Date"]);
                        objWOL.RevNo = Convert.ToString(row["REV_No"]);
                        objWOL.FinishedItem = Convert.ToString(row["ProductName"]);
                        objWOL.FinishedUom = Convert.ToString(row["FinishedUom"]);
                        objWOL.ProductionOrderDate = Convert.ToDateTime(row["ProductionOrderDate"]).ToString("dd-MM-yyyy");
                        objWOL.Warehouse = Convert.ToString(row["Warehouse"]);
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
    }
}