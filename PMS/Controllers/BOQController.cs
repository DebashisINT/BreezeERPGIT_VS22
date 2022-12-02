using BusinessLogicLayer;
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

namespace PMS.Controllers
{
    public class BOQController : Controller
    {
         // GET: BOMEntry
        BOQViewModel objBom = null;
        BOQModel objdata = null;
        DBEngine oDBEngine = new DBEngine();
        string JVNumStr = string.Empty;
        Int32 ProductionID = 0;
        Int32 DetailsID = 0;
        //Int64 GlobalDetailsID = 0;
        UserRightsForPage rights = new UserRightsForPage();
        public BOQController()
        {
            objBom = new BOQViewModel();
            objdata = new BOQModel();
        }
        public ActionResult Index(Int64 DetailsID = 0)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/BOQEntryList", "BOQ");
            try
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);


                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/Index", "BOQ");

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
                objBom.UnitList = list;

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
                                objBom.BOMNo = Convert.ToString(row["BOM_No"]);
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

                            }
                        }
                    }
                }

            }
            catch { }
            objBom.RevisionDate = DateTime.Now.ToString();
            objBom.BOMDate = DateTime.Now.ToString();

            TempData["Count"] = 1;
            TempData.Keep();

            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;

            return View(objBom);

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
            return PartialView("_BOMResourcesGrid", objList);
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
                    DataTable objData = objdata.GetBOMProductEntryListByID("GetBOMEntryProductsData", DetailsID);
                    if (objData != null && objData.Rows.Count > 0)
                    {
                        DataTable dt = objData;


                        foreach (DataRow row in dt.Rows)
                        {
                            bomproductdataobj = new BOMProduct();
                            bomproductdataobj.SlNO = Convert.ToString(row["SlNO"]);
                            bomproductdataobj.ProductName = Convert.ToString(row["sProducts_Name"]);
                            bomproductdataobj.ProductId = Convert.ToString(row["ProductID"]);
                            bomproductdataobj.ProductDescription = Convert.ToString(row["sProducts_Description"]);
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
                            bomproductdata.Add(bomproductdataobj);

                        }
                        ViewData["BOMEntryProductsTotalAm"] = bomproductdata.Sum(x => Convert.ToDecimal(x.Amount)).ToString();
                    }
                }

            }
            catch { }
            return PartialView("_BOMProductEntryGrid", bomproductdata);
        }

        [ValidateInput(false)]
        public ActionResult BatchEditingUpdateBOMProductEntry(DevExpress.Web.Mvc.MVCxGridViewBatchUpdateValues<BOMProduct, int> updateValues, BOQViewModel options)
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
                    updateValues.Insert = updateValues.Insert.OrderBy(x => x.SlNO).ToList();
                    foreach (var item in updateValues.Insert)
                    {
                        if (Convert.ToInt64(item.ProductId) > 0)
                        {
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

                            udtlist.Add(obj);
                        }
                    }
                    if (udtlist.Count > 0)
                    {
                        SaveDataArea = 1;
                        //if (options.BOMNo)
                        NumberScheme = checkNMakeBOMCode(options.strBOMNo, Convert.ToInt32(options.BOM_SCHEMAID), Convert.ToDateTime(options.RevisionDate));
                        if (NumberScheme == "ok")
                        {
                            udtlist = udtlist.OrderBy(x => x.SlNo).ToList();
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

                                udt.Add(obj1);
                            }
                            IsProcess = BOMProductInsertUpdate(udt, options);
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

                            udtlist.Add(obj);
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
                        //    NumberScheme = checkNMakeBOMCode(options.strBOMNo, Convert.ToInt32(options.BOM_SCHEMAID), Convert.ToDateTime(options.RevisionDate));
                        //    if (NumberScheme == "ok")
                        //    {
                        //        IsProcess = BOMProductInsertUpdate(udtlist, options);
                        //    }
                        //    else
                        //    {
                        //        Message = NumberScheme;
                        //    }
                        //}

                        //if (options.BOMNo)
                        // NumberScheme = checkNMakeBOMCode(options.strBOMNo, Convert.ToInt32(options.BOM_SCHEMAID), Convert.ToDateTime(options.RevisionDate));
                        udtlist = udtlist.OrderBy(x => x.SlNo).ToList();
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

                            udt.Add(obj1);
                        }

                        IsProcess = BOMProductInsertUpdate(udt, options);
                    }
                }


                TempData["Count"] = 1;
                TempData.Keep();
                ViewData["ProductionID"] = ProductionID;
                ViewData["DetailsID"] = DetailsID;
                ViewData["BOMNo"] = JVNumStr;
                ViewData["Success"] = IsProcess;
                ViewData["Message"] = Message;
            }
            return PartialView("_BOMProductEntryGrid", updateValues.Update);
            //return Json(IsProcess, JsonRequestBehavior.AllowGet);
        }

        public Boolean BOMProductInsertUpdate(List<udtProducts> obj, BOQViewModel obj2)
        {
            Boolean Success = false;


            try
            {
                DataTable dtBOM_PRODUCTS = new DataTable();
                dtBOM_PRODUCTS = ToDataTable(obj);

                DataSet dt = new DataSet();
                if (Convert.ToInt64(obj2.DetailsID) > 0)
                {
                    if (!String.IsNullOrEmpty(obj2.strBOMNo))
                    {
                        JVNumStr = obj2.strBOMNo;
                    }
                    dt = objdata.BOMProductEntryInsertUpdate("UPDATEMAINPRODUCT", JVNumStr, Convert.ToDateTime(obj2.BOMDate), Convert.ToInt64(obj2.FinishedItem), Convert.ToDecimal(obj2.FinishedQty), obj2.FinishedUom, obj2.BOMType, obj2.RevisionNo, Convert.ToDateTime(obj2.RevisionDate), Convert.ToInt32(obj2.Unit), Convert.ToInt32(obj2.WarehouseID)
                       , dtBOM_PRODUCTS, new DataTable(), Convert.ToInt32(obj2.BOM_SCHEMAID), Convert.ToDecimal(obj2.ActualAdditionalCost), Convert.ToInt64(Session["userid"]), 0, Convert.ToInt64(obj2.DetailsID));
                }
                else
                {
                    dt = objdata.BOMProductEntryInsertUpdate("INSERTMAINPRODUCT", JVNumStr, Convert.ToDateTime(obj2.BOMDate), Convert.ToInt64(obj2.FinishedItem), Convert.ToDecimal(obj2.FinishedQty), obj2.FinishedUom, obj2.BOMType, obj2.RevisionNo, Convert.ToDateTime(obj2.RevisionDate), Convert.ToInt32(obj2.Unit), Convert.ToInt32(obj2.WarehouseID)
                        , dtBOM_PRODUCTS, new DataTable(), Convert.ToInt32(obj2.BOM_SCHEMAID), Convert.ToDecimal(obj2.ActualAdditionalCost), Convert.ToInt64(Session["userid"]));
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



        public string checkNMakeBOMCode(string manual_str, int sel_schema_Id, DateTime RevisionDate)
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
        public ActionResult BatchEditingUpdateBOMResources(DevExpress.Web.Mvc.MVCxGridViewBatchUpdateValues<POSSales, int> updateValues, BOQViewModel options)
        {
            Boolean IsProcess = false;
            List<POSSales> objList = new List<POSSales>();
            List<udtResources> udt = new List<udtResources>();
            try
            {
                //foreach (var product in updateValues.Insert)
                if (updateValues.Insert.Count > 0 && updateValues.Update.Count == 0)
                {
                    // if (updateValues.IsValid(product) && Convert.ToInt32(options.ProductionID) > 0 && Convert.ToInt32(options.DetailsID) > 0)
                    //{
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

            return PartialView("_BOMResourcesGrid", objList);
        }


        public Boolean BOMResourcesInsertUpdate(List<udtResources> obj, Int32 ProductionID, Int32 DetailsID)
        {
            Boolean Success = false;
            try
            {
                DataSet dt = new DataSet();
                dt = objdata.BOMProductEntryInsertUpdate("INSERTRESOURCES", JVNumStr, DateTime.Now, 0, 0, "", "", "", DateTime.Now, 0, 0
                    , new DataTable(), ToDataTable(obj), 0, 0, Convert.ToInt64(Session["userid"]), ProductionID, DetailsID);
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

        public ActionResult BOQEntryList()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/BOQEntryList", "BOQ");
            BOQViewModel obj = new BOQViewModel();
            TempData.Clear();
            ViewBag.CanAdd = rights.CanAdd;
            return View("BOQEntryList", obj);
        }

        public ActionResult GetBOMEntryList()
        {
            List<BOQViewModel> list = new List<BOQViewModel>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/BOQEntryList", "BOQ");
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
                        dt = oDBEngine.GetDataTable("select * from V_BOQDetailsList where BRANCH_ID =" + BranchID + " AND (BOM_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') ");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("select * from V_BOQDetailsList where BOM_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' ");
                    }

                }
                //else
                //{
                //    dt = oDBEngine.GetDataTable("select * from V_BOQDetailsList");
                //}

                TempData["BOMDetailsListDataTable"] = dt;

                if (dt.Rows.Count > 0)
                {
                    BOQViewModel obj = new BOQViewModel();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new BOQViewModel();
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
                        }
                        else
                        {
                            obj.RevisionDate = "";
                        }

                        obj.Unit = Convert.ToString(item["BranchDescription"]);
                        obj.Warehouse = Convert.ToString(item["WarehouseName"]);

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
            return PartialView("_BOMEntryDataList", list);
        }

        public JsonResult RemoveBOMDataByID(Int32 detailsid)
        {
            ReturnData obj = new ReturnData();
            //Boolean Success = false;
            //String Message = String.Empty;
            try
            {
                var datasetobj = objdata.DropDownDetailForBOQ("RemoveBOMData", null, null, null, 0, detailsid);
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

        public JsonResult SetBOMDataByID(Int64 detailsid = 0)
        {
            Boolean Success = false;
            try
            {
                TempData["DetailsID"] = detailsid;
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
                return this.RedirectToAction("BOQEntryList", "BOQ");
            }
        }

        private GridViewSettings GetBOMGridView(object datatable)
        {
            //List<EmployeesTargetSetting> obj = (List<EmployeesTargetSetting>)datatablelist;
            //ListtoDataTable lsttodt = new ListtoDataTable();
            //DataTable datatable = ConvertListToDataTable(obj); 
            var settings = new GridViewSettings();
            settings.Name = "Bill of Quantities (BOQ)";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Bill of Quantities (BOQ)";
            //String ID = Convert.ToString(TempData["EmployeesTargetListDataTable"]);
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "BOM_No" || datacolumn.ColumnName == "BOM_Type"
                    || datacolumn.ColumnName == "BOM_Date" || datacolumn.ColumnName == "REV_No" || datacolumn.ColumnName == "REV_Date" || datacolumn.ColumnName == "FinishedProductName"
                    || datacolumn.ColumnName == "BranchDescription" || datacolumn.ColumnName == "WarehouseName" || datacolumn.ColumnName == "CreatedBy" || datacolumn.ColumnName == "CreateDate" || datacolumn.ColumnName == "ModifyBy" || datacolumn.ColumnName == "ModifyDate")
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "BOM_No")
                        {
                            column.Caption = "Document No";
                            column.VisibleIndex = 0;
                        }
                        else if (datacolumn.ColumnName == "BOM_Type")
                        {
                            column.Caption = "Document Type";
                            column.VisibleIndex = 1;
                        }
                        else if (datacolumn.ColumnName == "BOM_Date")
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
	}
}