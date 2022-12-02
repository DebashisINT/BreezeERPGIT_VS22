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
using System.Web;
using System.Web.Mvc;

namespace Manufacturing.Controllers
{
    public class QCController : Controller
    {
        QualityControlModel objQC = null;
        BOMEntryModel objdata = null;
        QualityControlViewModel objQVC = null;
        WorkOrderModel objWO = null;
        string JVNumStr = string.Empty;
        DBEngine oDBEngine = new DBEngine();
        UserRightsForPage rights = new UserRightsForPage();
        CommonBL cSOrder = new CommonBL();
        public QCController()
        {
            objQC = new QualityControlModel();
            objQVC = new QualityControlViewModel();
            objdata = new BOMEntryModel();
            objWO = new WorkOrderModel();
        }
        //
        // GET: /QC/
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
            DataTable Schemadt = objdata.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "105", "Y");
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

        public ActionResult QCEntry()
        {
            string ReceiptFromProductionSkipped = cSOrder.GetSystemSettingsResult("ReceiptFromProductionSkipped");
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
            objQVC.UnitList = list;

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
                if (TempData["QualityControlID"] != null)
                {
                    objQVC.QualityControlID = Convert.ToInt64(TempData["QualityControlID"]);
                    TempData.Keep();
                    DataTable objData;
                    if (objQVC.QualityControlID > 0)
                    {
                        if(ReceiptFromProductionSkipped=="No")
                        {
                             objData = objQC.GetQCData("GetQualityControlData", objQVC.QualityControlID, 0);
                        }
                        else
                        {
                            objData = objQC.GetQCData("GetQualityControlDataSettingsWise", objQVC.QualityControlID, 0);
                        }
                        
                        if (objData != null && objData.Rows.Count > 0)
                        {
                            DataTable dt = objData;
                            foreach (DataRow row in dt.Rows)
                            {
                                objQVC.QualityControlID = Convert.ToInt64(row["QualityControlID"]);

                                objQVC.FGQty = Convert.ToDecimal(row["FG_Qty"]);
                                objQVC.FreshQuantity = Convert.ToDecimal(row["Fresh_Qty"]);
                                objQVC.RejectedQuantity = Convert.ToDecimal(row["Rejected_Qty"]);
                                objQVC.QC_SchemaID = Convert.ToInt64(row["QC_SchemaID"]);
                                objQVC.QC_No = Convert.ToString(row["QC_No"]);
                                objQVC.dtOrderDate = Convert.ToDateTime(row["QC_Date"]);

                                objQVC.ProductionReceiptID = Convert.ToInt64(row["ProductionReceiptID"]);
                                objQVC.WorkOrderID = Convert.ToInt64(row["WorkOrderID"]);
                                objQVC.ProductionOrderID = Convert.ToInt64(row["ProductionOrderID"]);
                                objQVC.WorkCenterID = Convert.ToString(row["WorkCenterID"]);
                                objQVC.Receipt_No = Convert.ToString(row["Receipt_No"]);
                                objQVC.Receipt_Date = Convert.ToDateTime(row["Receipt_Date"]);
                                objQVC.ProductionIssueID = Convert.ToInt64(row["ProductionIssueID"]);
                                objQVC.ProductionIssueNo = Convert.ToString(row["ProductionIssueNo"]);
                                objQVC.ProductionIssueDate = Convert.ToDateTime(row["ProductionIssueDate"]);
                                objQVC.WorkOrderNo = Convert.ToString(row["WorkOrderNo"]);
                                //objQVC.Order_Qty = Convert.ToDecimal(row["Receipt_Qty"]);
                                objQVC.WorkCenterCode = Convert.ToString(row["WorkCenterCode"]);
                                objQVC.WorkCenterDescription = Convert.ToString(row["WorkCenterDescription"]);
                                objQVC.BRANCH_ID = Convert.ToInt64(row["BRANCH_ID"]);
                                objQVC.BOMNo = Convert.ToString(row["BOM_No"]);
                                objQVC.RevNo = Convert.ToString(row["REV_No"]);
                                objQVC.BOM_Date = Convert.ToDateTime(row["BOM_Date"]);
                                objQVC.ProductionOrderNo = Convert.ToString(row["ProductionOrderNo"]);
                                objQVC.ProductionOrderDate = Convert.ToDateTime(row["ProductionOrderDate"]);
                                objQVC.WorkOrderDate = Convert.ToDateTime(row["WorkOrderDate"]);
                                objQVC.ReceiptDate = Convert.ToString(row["ReceiptDate"]);
                                objQVC.FinishedItem = Convert.ToString(row["ProductName"]);
                                objQVC.FinishedUom = Convert.ToString(row["FinishedUom"]);
                                objQVC.Warehouse = Convert.ToString(row["Warehouse"]);
                                objQVC.TotalAmount = Convert.ToDecimal(row["TotalAmount"]);
                                objQVC.FGPrice = Convert.ToDecimal(row["FGPrice"]);
                                objQVC.strRemarks = Convert.ToString(row["Remarks"]);
                                objQVC.ProductDescription = Convert.ToString(row["ProductDescription"]);
                                objQVC.PartNoName = Convert.ToString(row["PartNoName"]);
                                objQVC.DesignNo = Convert.ToString(row["DesignNo"]);
                                objQVC.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                                if (Convert.ToString(row["REV_Date"]) != "")
                                {
                                    objQVC.REV_Date = Convert.ToDateTime(row["REV_Date"]);
                                }
                                else
                                {
                                    objQVC.REV_Date = null;
                                }
                                objQVC.CreatedBy = Convert.ToString(row["CreatedBy"]);
                                objQVC.ModifyBy = Convert.ToString(row["ModifyBy"]);
                                objQVC.CreateDate = Convert.ToDateTime(row["CreateDate"]);

                                if (Convert.ToString(row["ModifyDate"]) != "")
                                {
                                    objQVC.ModifyDate = Convert.ToDateTime(row["ModifyDate"]);
                                }
                                else
                                {
                                    objQVC.ModifyDate = null;
                                }
                                objQVC.Proj_Code = Convert.ToString(row["Proj_Code"]);
                                objQVC.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);
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

                if (objQVC.ProductionReceiptID < 1)
                {
                    objQVC.OrderDate = DateTime.Now.ToString();
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
            ViewBag.ReceiptFromProductionSkipped = ReceiptFromProductionSkipped;
            return View(objQVC);
        }

        public ActionResult GetPRList()
        {
            string ReceiptFromProductionSkipped = cSOrder.GetSystemSettingsResult("ReceiptFromProductionSkipped");
            string WorkOrderModuleSkipped = cSOrder.GetSystemSettingsResult("WorkOrderModuleSkipped");
            List<ProductionReceiptViewModel> list = new List<ProductionReceiptViewModel>();
            try
            {
                DataTable objData;
                ProductionReceiptViewModel objWOL = null;
                if(ReceiptFromProductionSkipped=="No")
                {
                     objData = objQC.GetQCData("GetProductionReciptData", 0, 0);
                }
                else
                {
                     objData = objQC.GetQCData("GetProductionIssueDataSettingsWise", 0, 0);
                }
                

                if (objData != null && objData.Rows.Count > 0)
                {
                    DataTable dt = objData;
                    foreach (DataRow row in dt.Rows)
                    {
                        objWOL = new ProductionReceiptViewModel();
                        objWOL.ProductionReceiptID = Convert.ToInt64(row["ProductionReceiptID"]);
                        objWOL.ProductionIssueID = Convert.ToInt64(row["ProductionIssueID"]);
                        objWOL.ProductionIssueNo = Convert.ToString(row["ProductionIssueNo"]);
                        objWOL.WorkOrderID = Convert.ToInt64(row["WorkOrderID"]);
                        //objWOL.OrderNo = Convert.ToString(row["OrderNo"]);
                        objWOL.ProductionOrderNo = Convert.ToString(row["ProductionOrderNo"]);
                        objWOL.ProductionOrderID = Convert.ToInt64(row["ProductionOrderID"]);
                        objWOL.Details_ID = Convert.ToInt64(row["Details_ID"]);
                        objWOL.BOMNo = Convert.ToString(row["BOM_No"]);
                        objWOL.ProductionIssueDate = Convert.ToDateTime(row["ProductionIssueDate"]);
                        objWOL.RevNo = Convert.ToString(row["REV_No"]);
                        objWOL.FinishedItem = Convert.ToString(row["ProductName"]);
                        objWOL.FinishedUom = Convert.ToString(row["FinishedUom"]);
                        objWOL.ProductionOrderDate = Convert.ToDateTime(row["ProductionOrderDate"]);
                        objWOL.Warehouse = Convert.ToString(row["Warehouse"]);
                        objWOL.ProductionIssueQty = Convert.ToDecimal(row["ProductionIssueQty"]);
                        objWOL.WorkCenterID = Convert.ToString(row["WorkCenterID"]);
                        objWOL.WorkCenterCode = Convert.ToString(row["WorkCenterCode"]);
                        objWOL.WorkCenterDescription = Convert.ToString(row["WorkCenterDescription"]);
                        objWOL.TotalAmount = Convert.ToDecimal(row["TotalAmount"]);
                        objWOL.FGPrice = Convert.ToDecimal(row["FGPrice"]);
                        objWOL.ProductDescription = Convert.ToString(row["ProductDescription"]);
                        objWOL.strRemarks = Convert.ToString(row["Remarks"]);
                        objWOL.BRANCH_ID = Convert.ToInt64(row["BRANCH_ID"]);
                        objWOL.Receipt_No = Convert.ToString(row["Receipt_No"]);
                        objWOL.Order_Qty = Convert.ToDecimal(row["Receipt_Qty"]);
                        objWOL.Receipt_Date = Convert.ToDateTime(row["Receipt_Date"]);
                        objWOL.WorkOrderNo = Convert.ToString(row["WorkOrderNo"]);
                        objWOL.PartNoName = Convert.ToString(row["PartNoName"]);
                        objWOL.DesignNo = Convert.ToString(row["DesignNo"]);
                        objWOL.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);
                        objWOL.Proj_Code = Convert.ToString(row["Proj_Code"]);
                        objWOL.Hierarchy = Convert.ToString(row["HIERARCHY_NAME"]);
                        list.Add(objWOL);
                    }
                }

            }
            catch { }
            ViewBag.ReceiptFromProductionSkipped = ReceiptFromProductionSkipped;
            ViewBag.WorkOrderModuleSkipped = WorkOrderModuleSkipped;
            
            return PartialView("_ProductionReciptList", list);
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


        public JsonResult QCInsertUpdate(QualityControlViewModel obj)
        {
            ReturnData objRD = new ReturnData();
            String NumberScheme = "";
            Boolean IsProcess = false;
            try
            {
                obj.UserID = Convert.ToInt64(Session["userid"]);
                //if (!String.IsNullOrEmpty(obj.QC_No) && obj.QC_No.ToLower() != "auto"
                if (!String.IsNullOrEmpty(obj.QC_No))
                {
                    JVNumStr = obj.QC_No;
                    NumberScheme = "ok";
                }

                //if (obj.QualityControlID == 0)
                //{
                //    NumberScheme = checkNMakePOCode(obj.QC_No, Convert.ToInt32(obj.Order_SchemaID), Convert.ToDateTime(obj.OrderDate));
                //}
                if (NumberScheme == "ok")
                {
                    obj.QC_No = JVNumStr;
                    var datasetobj = objQC.QCInsertUpdate("InsertUpdate", obj);
                    
                    if (datasetobj.Rows.Count > 0)
                    {

                        foreach (DataRow item in datasetobj.Rows)
                        {
                            objRD.Success = Convert.ToBoolean(item["Success"]);
                            objRD.Message = Convert.ToString(item["QC_No"]);

                            //objRD.Message = JVNumStr;
                        }
                    }
                }
                else
                {
                    objRD.Message = NumberScheme;
                }

                
                
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
                        sqlQuery = "SELECT max(tjv.QC_No) FROM QualityControl tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.QC_No))) = 1 and QC_No like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.QC_No) FROM QualityControl tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.QC_No))) = 1 and QC_No like '%" + sufxCompCode + "'";
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
                            sqlQuery = "SELECT max(tjv.QC_No) FROM QualityControl tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + paddCounter + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.QC_No))) = 1 and QC_No like '" + prefCompCode + "%'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }
                        else
                        {
                            int i = startNo.Length;
                            while (i < paddCounter)
                            {


                                sqlQuery = "SELECT max(tjv.QC_No) FROM QualityControl tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + i + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.QC_No))) = 1 and QC_No like '" + prefCompCode + "%'";

                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.QC_No)=" + i;
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
                                sqlQuery = "SELECT max(tjv.QC_No) FROM QualityControl tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + (i - 1) + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.QC_No))) = 1 and QC_No like '" + prefCompCode + "%'";
                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.QC_No)=" + (i - 1);
                                }
                                dtC = oDBEngine.GetDataTable(sqlQuery);
                            }

                        }

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.QC_No) FROM QualityControl tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.OrderNo))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.QC_No))) = 1 and QC_No like '" + prefCompCode + "%'";
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
                    sqlQuery = "SELECT QC_No FROM QualityControl WHERE QC_No LIKE '" + manual_str.Trim() + "'";
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

        public ActionResult QCList()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/QCList", "QC");
            TempData.Clear();
            ViewBag.CanAdd = rights.CanAdd;
            return View("QCList", objQVC);
        }

        public ActionResult GetQCList()
        {
            string WorkOrderModuleSkipped = cSOrder.GetSystemSettingsResult("WorkOrderModuleSkipped");
            string ReceiptFromProductionSkipped = cSOrder.GetSystemSettingsResult("ReceiptFromProductionSkipped");
            string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            List<QualityControlViewModel> list = new List<QualityControlViewModel>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/QCList", "QC");
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

                    if (ReceiptFromProductionSkipped == "No")
                    {
                        if (BranchID > 0)
                        {
                            dt = oDBEngine.GetDataTable("select * from V_QualityControlList where BRANCH_ID =" + BranchID + " AND (QC_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') ");
                        }
                        else
                        {
                            dt = oDBEngine.GetDataTable("select * from V_QualityControlList where QC_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' ");
                        }
                    }
                    else
                    {
                        if (BranchID > 0)
                        {
                            dt = oDBEngine.GetDataTable("select * from V_QualityControlListSettingsWise where BRANCH_ID =" + BranchID + " AND (QC_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') ");
                        }
                        else
                        {
                            dt = oDBEngine.GetDataTable("select * from V_QualityControlListSettingsWise where QC_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' ");
                        }
                    }

                }

                TempData["DetailsListDataTable"] = dt;

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        objQVC = new QualityControlViewModel();
                       
                        objQVC.QualityControlID = Convert.ToInt64(item["QualityControlID"]);
                        objQVC.FGQty = Convert.ToDecimal(item["FG_Qty"]);
                        objQVC.FreshQuantity = Convert.ToDecimal(item["Fresh_Qty"]);
                        objQVC.RejectedQuantity = Convert.ToDecimal(item["Rejected_Qty"]);
                        objQVC.QC_No = Convert.ToString(item["QC_No"]);
                        objQVC.dtOrderDate = Convert.ToDateTime(item["QC_Date"]);
                        objQVC.ProductionReceiptID = Convert.ToInt64(item["ProductionReceiptID"]);
                        objQVC.WorkOrderID = Convert.ToInt64(item["WorkOrderID"]);
                        objQVC.ProductionOrderID = Convert.ToInt64(item["ProductionOrderID"]);
                        objQVC.WorkCenterID = Convert.ToString(item["WorkCenterID"]);
                        objQVC.Receipt_No = Convert.ToString(item["Receipt_No"]);
                        objQVC.Receipt_Date = Convert.ToDateTime(item["Receipt_Date"]);
                        objQVC.ProductionIssueID = Convert.ToInt64(item["ProductionIssueID"]);
                        objQVC.ProductionIssueNo = Convert.ToString(item["ProductionIssueNo"]);
                        objQVC.ProductionIssueDate = Convert.ToDateTime(item["ProductionIssueDate"]);
                        objQVC.WorkOrderNo = Convert.ToString(item["WorkOrderNo"]);
                        //objQVC.Order_Qty = Convert.ToDecimal(item["Receipt_Qty"]);
                        objQVC.WorkCenterCode = Convert.ToString(item["WorkCenterCode"]);
                        objQVC.WorkCenterDescription = Convert.ToString(item["WorkCenterDescription"]);
                        objQVC.BRANCH_ID = Convert.ToInt64(item["BRANCH_ID"]);
                        objQVC.BOMNo = Convert.ToString(item["BOM_No"]);
                        objQVC.RevNo = Convert.ToString(item["REV_No"]);
                        objQVC.BOM_Date = Convert.ToDateTime(item["BOM_Date"]);
                        objQVC.ProductionOrderNo = Convert.ToString(item["ProductionOrderNo"]);
                        objQVC.ProductionOrderDate = Convert.ToDateTime(item["ProductionOrderDate"]);

                        objQVC.WorkOrderDate = Convert.ToDateTime(item["WorkOrderDate"]);
                        if (Convert.ToString(item["REV_Date"]) != "")
                        {
                            objQVC.REV_Date = Convert.ToDateTime(item["REV_Date"]);
                        }
                        else
                        {
                            objQVC.REV_Date = null;
                        }
                        objQVC.CreatedBy = Convert.ToString(item["CreatedBy"]);
                        objQVC.ModifyBy = Convert.ToString(item["ModifyBy"]);
                        objQVC.CreateDate = Convert.ToDateTime(item["CreateDate"]);
                        if (Convert.ToString(item["ModifyDate"]) != "")
                        {
                            objQVC.ModifyDate = Convert.ToDateTime(item["ModifyDate"]);
                        }
                        else
                        {
                            objQVC.ModifyDate = null;
                        }
                        objQVC.PartNoName = Convert.ToString(item["PartNoName"]);
                        objQVC.DesignNo = Convert.ToString(item["DesignNo"]);
                        objQVC.ItemRevNo = Convert.ToString(item["ItemRevisionNo"]);
                        objQVC.FinishedItemDescription = Convert.ToString(item["sProducts_Name"]);
                        objQVC.Proj_Code = Convert.ToString(item["Proj_Code"]);
                        objQVC.Proj_Name = Convert.ToString(item["Proj_Name"]);
                        list.Add(objQVC);
                    }
                }
            }
            catch { }
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            ViewBag.ProjectShow = ProjectSelectInEntryModule;
            ViewBag.ReceiptFromProductionSkipped = ReceiptFromProductionSkipped;
            ViewBag.WorkOrderModuleSkipped = WorkOrderModuleSkipped;
          
            return PartialView("_QCDataList", list);
        }

        public JsonResult SetQCDateFilter(Int64 unitid, string FromDate, string ToDate)
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

        public JsonResult RemoveQCDataByID(Int32 qualitycontrolid)
        {
            ReturnData obj = new ReturnData();
            try
            {
                var datasetobj = objQC.GetQCData("RemoveQCData", qualitycontrolid, 0);
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

        public JsonResult SetQCDataByID(Int64 qualitycontrolid = 0, Int16 IsView = 0)
        {
            Boolean Success = false;
            try
            {
                TempData["QualityControlID"] = qualitycontrolid;
                TempData["IsView"] = IsView;
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
            settings.Name = "Quality Control";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Quality Control";
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "QC_No" || datacolumn.ColumnName == "FG_Qty" || datacolumn.ColumnName == "Fresh_Qty" || datacolumn.ColumnName == "Rejected_Qty" ||
                    datacolumn.ColumnName == "Receipt_No" || datacolumn.ColumnName == "QC_Date" || datacolumn.ColumnName == "Receipt_Date" || datacolumn.ColumnName == "ProductionIssueNo" || datacolumn.ColumnName == "ProductionIssueDate" ||
                    datacolumn.ColumnName == "WorkOrderNo" || datacolumn.ColumnName == "WorkOrderDate" || datacolumn.ColumnName == "ProductionOrderNo" || datacolumn.ColumnName == "ProductionOrderDate"
                    || datacolumn.ColumnName == "BOM_No" || datacolumn.ColumnName == "BOM_Date" || datacolumn.ColumnName == "REV_No" || datacolumn.ColumnName == "REV_Date" ||
                    datacolumn.ColumnName == "CreatedBy" || datacolumn.ColumnName == "ModifyBy" || datacolumn.ColumnName == "CreateDate" || datacolumn.ColumnName == "ModifyDate"
                    )
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "QC_No")
                        {
                            column.Caption = "Quality Control No";
                            column.VisibleIndex = 0;
                        }

                        if (datacolumn.ColumnName == "FG_Qty")
                        {
                            column.Caption = "FG Quantity";
                            column.VisibleIndex = 1;
                        }

                        if (datacolumn.ColumnName == "Fresh_Qty")
                        {
                            column.Caption = "Fresh Quantity";
                            column.VisibleIndex = 2;
                        }

                        if (datacolumn.ColumnName == "Rejected_Qty")
                        {
                            column.Caption = "Rejected Quantity";
                            column.VisibleIndex = 3;
                        }

                        if (datacolumn.ColumnName == "QC_Date")
                        {
                            column.Caption = "Quality Control Date";
                            column.VisibleIndex = 4;
                        }

                        if (datacolumn.ColumnName == "Receipt_No")
                        {
                            column.Caption = "Production Receipt No";
                            column.VisibleIndex = 5;
                        }

                        if (datacolumn.ColumnName == "Receipt_Date")
                        {
                            column.Caption = "Production Receipt Date";
                            column.VisibleIndex = 6;
                        }

                        if (datacolumn.ColumnName == "ProductionIssueNo")
                        {
                            column.Caption = "Production Issue No";
                            column.VisibleIndex = 7;
                        }

                        if (datacolumn.ColumnName == "ProductionIssueDate")
                        {
                            column.Caption = "Production Issue Date";
                            column.VisibleIndex = 8;
                        }

                        if (datacolumn.ColumnName == "WorkOrderNo")
                        {
                            column.Caption = "Work Order No";
                            column.VisibleIndex = 9;
                        }
                        else if (datacolumn.ColumnName == "WorkOrderDate")
                        {
                            column.Caption = "Work Order Date";
                            column.VisibleIndex = 10;
                        }
                        else if (datacolumn.ColumnName == "ProductionOrderNo")
                        {
                            column.Caption = "Production Order No";
                            column.VisibleIndex = 11;
                        }
                        else if (datacolumn.ColumnName == "ProductionOrderDate")
                        {
                            column.Caption = "Production Order Date";
                            column.VisibleIndex = 12;
                        }
                        else if (datacolumn.ColumnName == "BOM_No")
                        {
                            column.Caption = "BOM No";
                            column.VisibleIndex = 13;

                        }
                        else if (datacolumn.ColumnName == "BOM_Date")
                        {
                            column.Caption = "BOM Date";
                            column.VisibleIndex = 14;
                        }
                        else if (datacolumn.ColumnName == "REV_No")
                        {
                            column.Caption = "Rev No.";
                            column.VisibleIndex = 15;
                        }
                        else if (datacolumn.ColumnName == "REV_Date")
                        {
                            column.Caption = "Rev Date";
                            column.VisibleIndex = 16;
                        }
                        else if (datacolumn.ColumnName == "CreatedBy")
                        {
                            column.Caption = "Entered By";
                            column.VisibleIndex = 18;
                        }
                        else if (datacolumn.ColumnName == "CreateDate")
                        {
                            column.Caption = "Entered On";
                            column.VisibleIndex = 17;
                        }
                        else if (datacolumn.ColumnName == "ModifyBy")
                        {
                            column.Caption = "Modified By";
                            column.VisibleIndex = 19;
                        }
                        else if (datacolumn.ColumnName == "ModifyDate")
                        {
                            column.Caption = "Modified On";
                            column.VisibleIndex = 20;
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

    }
}