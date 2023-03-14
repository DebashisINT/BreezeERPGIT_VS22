//==================================================== Revision History =========================================================================
// 1.0  Priti   V2.0.36    23-01-2023  0025610:MRP Close Feature required
// 2.0  Priti   V2.0.36    01-02-2023  0025634:Available Stock to be calculated in MRP product Wise
// 3.0  Priti   V2.0.37    28-02-2023  0025703:Avl Stk, Phy Stk, Indent Qty & Pur Qty to be implemented in Preview Line Items in MRP
// 4.0  Priti   V2.0.37    13-03-2023  save Avl Stk in table
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

    public class MRPController : Controller
    {
        MRPViewModel objBom = null;
        MRPModel objdata = null;
        DBEngine oDBEngine = new DBEngine();
        string JVNumStr = string.Empty;
        Int32 ProductionID = 0;
        Int32 DetailsID = 0;
        //Int64 GlobalDetailsID = 0;
        UserRightsForPage rights = new UserRightsForPage();
        CommonBL cSOrder = new CommonBL();

        public MRPController()
        {
            objBom = new MRPViewModel();
            objdata = new MRPModel();
        }
        public ActionResult MRPAdd(string ActionType,Int64 MRP_ID = 0)
        {
            string hdnWSTAutoPrint = "0";
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/MRP", "MRP");

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

                TempData["DetailsID"] =null;
                TempData["MRP_ID"] = null;
                TempData["MPSID"] = null;
                TempData["FGProductID"] = null;
                TempData["FGQTY"] = null;
                

                if (ActionType != "ADD")
                {
                   

                    if (ActionType == "View")
                    {
                        TempData["IsView"] = 1;
                       // ViewBag.View = Convert.ToString(TempData["IsView"]);
                    }
                    else if (ActionType == "EDIT")
                    {
                            TempData["Edit"] = ActionType;
                            ViewBag.View = Convert.ToString(TempData["Edit"]);
                     }                  

                    if (MRP_ID >0)
                    {
                        TempData["MRP_ID"] = MRP_ID;
                        objBom.MRP_ID = Convert.ToString(MRP_ID);
                        TempData.Keep();

                        if (Convert.ToInt64(objBom.MRP_ID) > 0)
                        {
                            DataTable objData = objdata.GetBOMHeaderEntryListByID("GET_MRP_ENTRYDETAILSDATA", Convert.ToInt64(objBom.MRP_ID));
                            if (objData != null && objData.Rows.Count > 0)
                            {
                                DataTable dt = objData;


                                foreach (DataRow row in dt.Rows)
                                {
                                    objBom.MRP_ID = Convert.ToString(row["MRP_ID"]);                                   
                                    objBom.BOM_SCHEMAID = Convert.ToString(row["MRP_SchemaID"]);
                                    objBom.MRPNo = Convert.ToString(row["MRP_No"]);
                                    objBom.MRPDate = Convert.ToString(row["MRP_Date"]);
                                    objBom.BOMNo = Convert.ToString(row["BOM_No"]);
                                    objBom.BOMDate = Convert.ToString(row["BOM_Date"]);


                                    objBom.FinishedItem = Convert.ToString(row["FinishedProductName"]);
                                    objBom.FinishedQty = Convert.ToString(row["Finished_Qty"]);
                                    objBom.RevisionNo = Convert.ToString(row["REV_No"]);
                                    objBom.RevisionDate = Convert.ToString(row["REV_Date"]);
                                    objBom.Unit = Convert.ToString(row["BRANCH_ID"]);
                                    objBom.PartNoName = Convert.ToString(row["PartNoName"]);
                                    objBom.DesignNo = Convert.ToString(row["DesignNo"]);
                                    objBom.ItemRevNo = Convert.ToString(row["ItemRevisionNo"]);

                                    objBom.BOMRelationshipID = Convert.ToString(row["BOMRelation_ID"]);
                                    objBom.BOMRelationshipNo = Convert.ToString(row["BOMRelation_No"]);
                                    objBom.strRemarks = Convert.ToString(row["Remarks"]);
                                    objBom.FinishedUom = Convert.ToString(row["UOM_Name"]);
                                    objBom.ParentBOMID = Convert.ToString(row["Details_ID"]);
                                    objBom.MPS_ID = Convert.ToString(row["MPS_ID"]);
                                    objBom.MPSDate = Convert.ToString(row["MPS_Date"]);
                                    objBom.FinishedItemName = Convert.ToString(row["FinishedProductName"]);
                                    objBom.strRemarks = Convert.ToString(row["Remarks"]);
                                    objBom.FG_ID = Convert.ToString(row["Finished_ProductID"]);
                                    ViewBag.MPS_ID = Convert.ToString(row["MPS_ID"]);
                                    ViewBag.ParentBOMID = Convert.ToString(row["Details_ID"]);
                                    ViewBag.Unit = Convert.ToString(row["BRANCH_ID"]);
                                }
                            }
                        }
                    }

                }
            }
            catch { }
           // objBom.RevisionDate = DateTime.Now.ToString();
            //objBom.BOMDate = DateTime.Now.ToString();

            TempData["Count"] = 1;
            TempData.Keep();

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

            return View("~/Views/MRP/MRPAdd.cshtml", objBom);

        }

        public JsonResult getNumberingSchemeRecord(String type = null)
        {
            List<SchemaNumber> list = new List<SchemaNumber>();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            String strType = "161";
            
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

        public ActionResult GetMRPBOMProductList()
        {
            MRPProduct bomproductdataobj = new MRPProduct();
            List<MRPProduct> bomproductdata = new List<MRPProduct>();
            Int64 DetailsID = 0;
            Int64 MRP_ID = 0;
            Int64 MPS_ID = 0;
            Int64 FGProductID = 0;
            string FGQTY = "0";

            try
            {

                if (TempData["DetailsID"] != null)
                {
                    DetailsID = Convert.ToInt64(TempData["DetailsID"]);
                    TempData.Keep();
                }
                else if (TempData["MRP_ID"] != null)
                {
                    MRP_ID = Convert.ToInt64(TempData["MRP_ID"]);
                    TempData.Keep();
                }
                else if (TempData["MPSID"] != null)
                {
                    MPS_ID = Convert.ToInt64(TempData["MPSID"]);
                    FGProductID = Convert.ToInt64(TempData["FGProductID"]);
                    FGQTY = Convert.ToString(TempData["FGQTY"]);
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
                            bomproductdataobj = new MRPProduct();
                            bomproductdataobj.SlNO = Convert.ToString(row["SlNO"]);
                            bomproductdataobj.ProductName = Convert.ToString(row["sProducts_Code"]);
                            bomproductdataobj.ProductId = Convert.ToString(row["ProductID"]);
                            bomproductdataobj.ProductDescription = Convert.ToString(row["sProducts_Name"]);
                            bomproductdataobj.DesignNo = Convert.ToString(row["DesignNo"]);
                            bomproductdataobj.ItemRevisionNo = Convert.ToString(row["ItemRevisionNo"]);
                            bomproductdataobj.ProductQty = Convert.ToString(row["FactorQty"]);
                            bomproductdataobj.ProductUOM = Convert.ToString(row["StkUOM"]);                       
                            bomproductdataobj.Price = Convert.ToString(row["Price"]);
                            bomproductdataobj.Amount = Convert.ToString(row["Amount"]);
                            bomproductdataobj.AvlStk = Convert.ToString(row["AVLSTK"]);
                            bomproductdataobj.IndentQty = Convert.ToString(row["IndentQty"]);                       
                           
                            bomproductdataobj.PkgQty = Convert.ToString(row["PkgQty"]);
                            bomproductdataobj.PurchaseQty = Convert.ToString(row["PurchaseQty"]);
                            bomproductdataobj.AltQty = Convert.ToString(row["AltQty"]);
                            bomproductdataobj.AltUOM = Convert.ToString(row["AltUOM"]);
                            bomproductdataobj.AltUOMID = Convert.ToString(row["AltUOMID"]);
                            bomproductdataobj.UOmId = Convert.ToString(row["UOM_ID"]);
                            bomproductdataobj.OLDQty = Convert.ToString(row["FactorQty"]);
                            bomproductdataobj.OldAltQty = Convert.ToString(row["packing_quantity"]);
                            bomproductdataobj.VendorName = Convert.ToString(row["Vendor_Name"]);
                            bomproductdata.Add(bomproductdataobj);

                        }
                        //ViewData["BOMEntryProductsTotalAm"] = bomproductdata.Sum(x => Convert.ToDecimal(x.Amount)).ToString();
                    }
                }

                if (MRP_ID > 0)
                {
                    DataTable objData = objdata.GetBOMHeaderEntryListByID("GetBOMEntryProductsEditData", MRP_ID);
                    if (objData != null && objData.Rows.Count > 0)
                    {
                        DataTable dt = objData;


                        foreach (DataRow row in dt.Rows)
                        {
                            bomproductdataobj = new MRPProduct();
                            bomproductdataobj.SlNO = Convert.ToString(row["SlNO"]);
                            bomproductdataobj.ProductName = Convert.ToString(row["sProducts_Code"]);
                            bomproductdataobj.ProductId = Convert.ToString(row["ProductID"]);
                            bomproductdataobj.ProductDescription = Convert.ToString(row["sProducts_Name"]);
                            bomproductdataobj.DesignNo = Convert.ToString(row["DesignNo"]);
                            bomproductdataobj.ItemRevisionNo = Convert.ToString(row["ItemRevisionNo"]);
                            bomproductdataobj.ProductQty = Convert.ToString(row["StkQty"]);
                            bomproductdataobj.ProductUOM = Convert.ToString(row["StkUOM"]);
                            bomproductdataobj.Price = Convert.ToString(row["Price"]);
                            bomproductdataobj.Amount = Convert.ToString(row["Amount"]);
                            bomproductdataobj.AvlStk = Convert.ToString(row["AVLSTK"]);
                            bomproductdataobj.IndentQty = Convert.ToString(row["IndentQty"]);

                            bomproductdataobj.PkgQty = Convert.ToString(row["PkgQty"]);
                            bomproductdataobj.PurchaseQty = Convert.ToString(row["PurchaseQty"]);
                            bomproductdataobj.AltQty = Convert.ToString(row["AltQty"]);
                            bomproductdataobj.AltUOM = Convert.ToString(row["AltUOM"]);
                            bomproductdataobj.AltUOMID = Convert.ToString(row["AltUOMID"]);
                            bomproductdataobj.UOmId = Convert.ToString(row["UOM_ID"]);
                            bomproductdataobj.OLDQty = Convert.ToString(row["BalQty"]);
                            bomproductdataobj.OldAltQty = Convert.ToString(row["packing_quantity"]);
                            bomproductdataobj.VendorName = Convert.ToString(row["Vendor_Name"]);
                            bomproductdataobj.NewAvlStk = Convert.ToString(row["NewAvlStock"]);//2.0
                            bomproductdata.Add(bomproductdataobj);

                        }
                       // ViewData["BOMEntryProductsTotalAm"] = bomproductdata.Sum(x => Convert.ToDecimal(x.Amount)).ToString();
                    }
                }
                if (MPS_ID > 0)
                {
                    DataTable objData;
                    if(FGProductID==0)
                    {
                        objData = objdata.GetMPSEntryListByFGQtyID("GetMPSEntryAllFGProductsData", MPS_ID, FGQTY);
                    }
                    else
                    {
                         objData = objdata.GetMPSEntryListByID("GetMPSEntryProductsData", MPS_ID, FGProductID, FGQTY);
                    }
                    
                   
                    if (objData != null && objData.Rows.Count > 0)
                    {
                        DataTable dt = objData;


                        foreach (DataRow row in dt.Rows)
                        {
                            bomproductdataobj = new MRPProduct();
                            bomproductdataobj.SlNO = Convert.ToString(row["SlNO"]);
                            bomproductdataobj.ProductName = Convert.ToString(row["sProducts_Code"]);
                            bomproductdataobj.ProductId = Convert.ToString(row["ProductID"]);
                            bomproductdataobj.ProductDescription = Convert.ToString(row["sProducts_Name"]);
                            bomproductdataobj.DesignNo = Convert.ToString(row["DesignNo"]);
                            bomproductdataobj.ItemRevisionNo = Convert.ToString(row["ItemRevisionNo"]);
                            bomproductdataobj.ProductQty = Convert.ToString(row["tempFactorQty"]);
                            bomproductdataobj.ProductUOM = Convert.ToString(row["StkUOM"]);
                            bomproductdataobj.Price = Convert.ToString(row["Price"]);
                            bomproductdataobj.Amount = Convert.ToString(row["Amount"]);
                            bomproductdataobj.AvlStk = Convert.ToString(row["AVLSTK"]);
                            bomproductdataobj.IndentQty = Convert.ToString(row["IndentQty"]);

                            bomproductdataobj.PkgQty = Convert.ToString(row["PkgQty"]);
                            bomproductdataobj.PurchaseQty = Convert.ToString(row["PurchaseQty"]);
                            bomproductdataobj.AltQty = Convert.ToString(row["AltQty"]);
                            bomproductdataobj.AltUOM = Convert.ToString(row["AltUOM"]);
                            bomproductdataobj.AltUOMID = Convert.ToString(row["AltUOMID"]);
                            bomproductdataobj.UOmId = Convert.ToString(row["UOM_ID"]);
                            bomproductdataobj.OLDQty = Convert.ToString(row["FactorQty"]);
                            bomproductdataobj.OldAltQty = Convert.ToString(row["packing_quantity"]);
                            bomproductdataobj.VendorName = Convert.ToString(row["Vendor_Name"]);
                            bomproductdataobj.NewAvlStk = Convert.ToString(row["NewAvlStock"]);//2.0

                            bomproductdata.Add(bomproductdataobj);

                        }
                        
                    }
                }
            }
            catch { }
            return PartialView("~/Views/MRP/_BOMProductEntryGrid.cshtml", bomproductdata);
        }

        [ValidateInput(false)]
        public ActionResult BatchEditingUpdateBOMProductEntry(DevExpress.Web.Mvc.MVCxGridViewBatchUpdateValues<MRPProduct, int> updateValues, MRPViewModel options)
        {
            TempData["Count"] = (int)TempData["Count"] + 1;
            TempData.Keep();
            String NumberScheme = "";
            String Message = "";
            Int64 SaveDataArea = 0;

            List<udtMRPProducts> udt = new List<udtMRPProducts>();

            if ((int)TempData["Count"] != 2)
            {
                Boolean IsProcess = false;
                List<MRPProduct> list = new List<MRPProduct>();
                //foreach (var product in updateValues.Insert)
                if (updateValues.Insert.Count > 0 )
                {
                    //if (updateValues.IsValid(product))
                    //{
                    List<udtMRPEntryProducts> udtlist = new List<udtMRPEntryProducts>();
                    udtMRPEntryProducts obj = null;
                    updateValues.Insert = updateValues.Insert.OrderBy(x => x.SlNO).ToList();
                    foreach (var item in updateValues.Insert)
                    {
                        if (Convert.ToInt64(item.ProductId) > 0)
                        {


                            obj = new udtMRPEntryProducts();
                            obj.ProductID = Convert.ToInt64(item.ProductId);
                            obj.ProductDescription = item.ProductDescription;
                            obj.DesignNo = item.DesignNo;
                            obj.ItemRevisionNo = item.ItemRevisionNo;
                            obj.ProductQty = Convert.ToDecimal(item.ProductQty);
                            obj.UOmId = Convert.ToInt64(item.UOmId);
                            obj.StkQty = Convert.ToDecimal(item.StockQty);                           
                            obj.Price = Convert.ToDecimal(item.Price);
                            obj.Amount = Convert.ToDecimal(item.Amount);
                            obj.IndentQty = Convert.ToDecimal(item.IndentQty);
                            obj.PkgQty = Convert.ToDecimal(item.PkgQty);
                            obj.PurchaseQty = Convert.ToDecimal(item.PurchaseQty);
                            obj.AltQty = Convert.ToDecimal(item.AltQty);
                            obj.AltUOMID = Convert.ToInt64(item.AltUOMID);
                            obj.OLDQty = Convert.ToDecimal(item.OLDQty);
                            obj.OldAltQty = Convert.ToInt64(item.OldAltQty);
                            //Rev 4.0
                            obj.NewAvlStk = Convert.ToDecimal(item.NewAvlStk);
                            //Rev 4.0 End
                            obj.SlNo = (item.SlNO);

                            udtlist.Add(obj);
                        }
                    }
                    if (udtlist.Count > 0)
                    {
                        SaveDataArea = 1;
                        //if (options.BOMNo)
                        NumberScheme = checkNMakeBOMCode(options.MRPNo, Convert.ToInt32(options.BOM_SCHEMAID), Convert.ToDateTime(options.MRPDate));
                        if (NumberScheme == "ok")
                        {
                            udtlist = udtlist.OrderBy(x => x.SlNo).ToList();
                            foreach (var item in udtlist)
                            {
                                udtMRPProducts obj1 = new udtMRPProducts();
                                obj.ProductID = Convert.ToInt64(item.ProductID);
                                obj.ProductDescription = item.ProductDescription;
                                obj.DesignNo = item.DesignNo;
                                obj.ItemRevisionNo = item.ItemRevisionNo;
                                obj.ProductQty = Convert.ToDecimal(item.ProductQty);
                                obj.UOmId = Convert.ToInt64(item.UOmId);
                                obj.StkQty = Convert.ToDecimal(item.StkQty);
                                obj.Price = Convert.ToDecimal(item.Price);
                                obj.Amount = Convert.ToDecimal(item.Amount);
                                obj.IndentQty = Convert.ToDecimal(item.IndentQty);
                                obj.PkgQty = Convert.ToDecimal(item.PkgQty);
                                obj.PurchaseQty = Convert.ToDecimal(item.PurchaseQty);
                                obj.AltQty = Convert.ToDecimal(item.AltQty);
                                obj.AltUOMID = Convert.ToInt64(item.AltUOMID);
                                obj.OLDQty = Convert.ToDecimal(item.OLDQty);
                                obj.OldAltQty = Convert.ToInt64(item.OldAltQty);
                                //Rev 4.0
                                obj.NewAvlStk = Convert.ToDecimal(item.NewAvlStk);
                                //Rev 4.0 End
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
                if ((updateValues.Update.Count > 0 ) || (updateValues.Insert.Count > 0 ) && (SaveDataArea == 0))
                {
                    List<udtMRPEntryProducts> udtlist = new List<udtMRPEntryProducts>();
                    udtMRPEntryProducts obj = null;

                    foreach (var item in updateValues.Update)
                    {
                        if (Convert.ToInt64(item.ProductId) > 0)
                        {
                            obj = new udtMRPEntryProducts();
                            obj.ProductID = Convert.ToInt64(item.ProductId);
                            obj.ProductDescription = item.ProductDescription;
                            obj.DesignNo = item.DesignNo;
                            obj.ItemRevisionNo = item.ItemRevisionNo;
                            obj.ProductQty = Convert.ToDecimal(item.ProductQty);
                            obj.UOmId = Convert.ToInt64(item.UOmId);
                            obj.StkQty = Convert.ToDecimal(item.StockQty);
                            obj.Price = Convert.ToDecimal(item.Price);
                            obj.Amount = Convert.ToDecimal(item.Amount);
                            obj.IndentQty = Convert.ToDecimal(item.IndentQty);
                            obj.PkgQty = Convert.ToDecimal(item.PkgQty);
                            obj.PurchaseQty = Convert.ToDecimal(item.PurchaseQty);
                            obj.AltQty = Convert.ToDecimal(item.AltQty);
                            obj.AltUOMID = Convert.ToInt64(item.AltUOMID);
                            obj.OLDQty = Convert.ToDecimal(item.OLDQty);
                            obj.OldAltQty = Convert.ToDecimal(item.OldAltQty);
                            //Rev 4.0
                            obj.NewAvlStk = Convert.ToDecimal(item.NewAvlStk);
                            //Rev 4.0 End
                            udtlist.Add(obj);
                        }
                    }

                    foreach (var item in updateValues.Insert)
                    {
                        if (Convert.ToInt64(item.ProductId) > 0)
                        {
                            obj.ProductID = Convert.ToInt64(item.ProductId);
                            obj.ProductDescription = item.ProductDescription;
                            obj.DesignNo = item.DesignNo;
                            obj.ItemRevisionNo = item.ItemRevisionNo;
                            obj.ProductQty = Convert.ToDecimal(item.ProductQty);
                            obj.UOmId = Convert.ToInt64(item.UOmId);
                            obj.StkQty = Convert.ToDecimal(item.StockQty);
                            obj.Price = Convert.ToDecimal(item.Price);
                            obj.Amount = Convert.ToDecimal(item.Amount);
                            obj.IndentQty = Convert.ToDecimal(item.IndentQty);
                            obj.PkgQty = Convert.ToDecimal(item.PkgQty);
                            obj.PurchaseQty = Convert.ToDecimal(item.PurchaseQty);
                            obj.AltQty = Convert.ToDecimal(item.AltQty);
                            obj.AltUOMID = Convert.ToInt64(item.AltUOMID);
                            obj.SlNo = (item.SlNO);
                            obj.OLDQty = Convert.ToDecimal(item.OLDQty);
                            obj.OldAltQty = Convert.ToInt64(item.OldAltQty);
                            //Rev 4.0
                            obj.NewAvlStk = Convert.ToDecimal(item.NewAvlStk);
                            //Rev 4.0 End
                            udtlist.Add(obj);
                        }
                    }

                    if (udtlist.Count > 0)
                    {
                        SaveDataArea = 1;
                        udtlist = udtlist.OrderBy(x => x.SlNo).ToList();
                        foreach (var item in udtlist)
                        {
                            udtMRPProducts obj1 = new udtMRPProducts();
                            obj1.ProductID = Convert.ToInt64(item.ProductID);
                            obj1.ProductDescription = item.ProductDescription;
                            obj1.DesignNo = item.DesignNo;
                            obj1.ItemRevisionNo = item.ItemRevisionNo;
                            obj1.ProductQty = Convert.ToDecimal(item.ProductQty);
                            obj1.UOmId = Convert.ToInt64(item.UOmId);
                            obj1.StkQty = Convert.ToDecimal(item.StkQty);
                            obj1.Price = Convert.ToDecimal(item.Price);
                            obj1.Amount = Convert.ToDecimal(item.Amount);
                            obj1.IndentQty = Convert.ToDecimal(item.IndentQty);
                            obj1.PkgQty = Convert.ToDecimal(item.PkgQty);
                            obj1.PurchaseQty = Convert.ToDecimal(item.PurchaseQty);
                            obj1.AltQty = Convert.ToDecimal(item.AltQty);
                            obj1.AltUOMID = Convert.ToInt64(item.AltUOMID);
                            obj1.OLDQty = Convert.ToDecimal(item.OLDQty);
                            obj1.OldAltQty = Convert.ToInt64(item.OldAltQty);
                            //Rev 4.0
                            obj1.NewAvlStk = Convert.ToDecimal(item.NewAvlStk);
                            //Rev 4.0 End
                            udt.Add(obj1);
                        }
                        if (Convert.ToUInt64(options.MRP_ID) > 0)
                        {
                            IsProcess = BOMProductInsertUpdate(udt, options);
                        }
                        else
                        {

                            NumberScheme = checkNMakeBOMCode(options.MRPNo, Convert.ToInt32(options.BOM_SCHEMAID), Convert.ToDateTime(options.MRPDate));
                            if (NumberScheme == "ok")
                            {
                                IsProcess = BOMProductInsertUpdate(udt, options);
                            }
                            else
                            {
                                Message = NumberScheme;
                            }
                        }
                       
                    }
                }


                TempData["Count"] = 1;
                TempData.Keep();
               // ViewData["ProductionID"] = ProductionID;
                ViewData["DetailsID"] = DetailsID;
                ViewData["BOMNo"] = JVNumStr;
                ViewData["Success"] = IsProcess;
                ViewData["Message"] = Message;
            }
            return PartialView("~/Views/MRP/_BOMProductEntryGrid.cshtml", updateValues.Update);
            
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
                        sqlQuery = "SELECT max(tjv.MRP_No) FROM MRP_Header tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.MRP_No))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.MRP_No))) = 1 and MRP_No like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.MRP_No) FROM MRP_Header tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.MRP_No))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.MRP_No))) = 1 and MRP_No like '%" + sufxCompCode + "'";
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
                            sqlQuery = "SELECT max(tjv.MRP_No) FROM MRP_Header tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + paddCounter + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.MRP_No))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.MRP_No))) = 1 and MRP_No like '" + prefCompCode + "%'";
                            if (scheme_type == 2)
                                sqlQuery += " AND CONVERT(DATE, CreateDate) = CONVERT(DATE, GETDATE())";
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }
                        else
                        {
                            int i = startNo.Length;
                            while (i < paddCounter)
                            {


                                sqlQuery = "SELECT max(tjv.MRP_No) FROM MRP_Header tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + i + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.MRP_No))) = 1 and MRP_No like '" + prefCompCode + "%'";

                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.MRP_No)=" + i;
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
                                sqlQuery = "SELECT max(tjv.MRP_No) FROM MRP_Header tjv WHERE dbo.RegexMatch('";
                                if (prefLen > 0)
                                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                                sqlQuery += "[0-9]{" + (i - 1) + "}";
                                if (sufxLen > 0)
                                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                                sqlQuery += "?$', LTRIM(RTRIM(tjv.MRP_No))) = 1 and MRP_No like '" + prefCompCode + "%'";
                                if (prefLen == 0 && sufxLen == 0)
                                {
                                    sqlQuery += " and LEN(tjv.MRP_No)=" + (i - 1);
                                }
                                dtC = oDBEngine.GetDataTable(sqlQuery);
                            }

                        }

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.MRP_No) FROM MRP_Header tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.MRP_No))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.MRP_No))) = 1 and MRP_No like '" + prefCompCode + "%'";
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
                    sqlQuery = "SELECT MRP_No FROM MRP_Header WHERE MRP_No LIKE '" + manual_str.Trim() + "'";
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
        public Boolean BOMProductInsertUpdate(List<udtMRPProducts> obj, MRPViewModel obj2)
        {
            Boolean Success = false;
            try
            {
                DataTable dtBOM_PRODUCTS = new DataTable();
                dtBOM_PRODUCTS = ToDataTable(obj);

                //DataColumnCollection dtC = dtBOM_PRODUCTS.Columns;
                //if (dtC.Contains("BOMProductsID"))
                //{
                //    dtBOM_PRODUCTS.Columns.Remove("BOMProductsID");
                //}


                string FinYear = Convert.ToString(Session["LastFinYear"]);
                string strCompanyID = Convert.ToString(Session["LastCompany"]);

                DataSet dt = new DataSet();
                if (Convert.ToInt64(obj2.MRP_ID) > 0)
                {
                    if (!String.IsNullOrEmpty(obj2.MRPNo))
                    {
                        JVNumStr = obj2.MRPNo;
                    }
                    //dt = objdata.BOMProductEntryInsertUpdate("UPDATEMAINPRODUCT", JVNumStr, Convert.ToDateTime(obj2.BOMDate), Convert.ToInt64(obj2.FinishedItem), Convert.ToDecimal(obj2.FinishedQty), obj2.FinishedUom, obj2.BOMType, obj2.RevisionNo, Convert.ToDateTime(obj2.RevisionDate), Convert.ToInt32(obj2.Unit)
                    //   , dtBOM_PRODUCTS, new DataTable(), Convert.ToInt32(obj2.BOM_SCHEMAID), Convert.ToInt64(Session["userid"]), 0, Convert.ToInt64(obj2.DetailsID), obj2.strRemarks, obj2.PartNo, strCompanyID, FinYear);
                    dt = objdata.MRPProductEntryInsertUpdate("UPDATEMAINPRODUCT",Convert.ToInt64(obj2.MRP_ID), Convert.ToInt64(obj2.BOM_SCHEMAID), JVNumStr, Convert.ToDateTime(obj2.MRPDate), Convert.ToInt64(obj2.ParentBOMID), Convert.ToInt32(obj2.Unit)
                       , dtBOM_PRODUCTS, Convert.ToDecimal(obj2.FinishedQty), Convert.ToInt64(Session["userid"]), obj2.strRemarks, strCompanyID, FinYear, Convert.ToInt64(obj2.MPS_ID), Convert.ToInt64(obj2.FG_ID));
                }
                else
                {
                    dt = objdata.MRPProductEntryInsertUpdate("INSERTMAINPRODUCT",Convert.ToInt64(obj2.MRP_ID), Convert.ToInt64(obj2.BOM_SCHEMAID), JVNumStr, Convert.ToDateTime(obj2.MRPDate), Convert.ToInt64(obj2.ParentBOMID), Convert.ToInt32(obj2.Unit)
                        , dtBOM_PRODUCTS, Convert.ToDecimal(obj2.FinishedQty), Convert.ToInt64(Session["userid"]), obj2.strRemarks, strCompanyID, FinYear, Convert.ToInt64(obj2.MPS_ID), Convert.ToInt64(obj2.FG_ID));
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
        public ActionResult MRP()
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/MRP", "MRP");
            BOMEntryViewModel obj = new BOMEntryViewModel();
            TempData.Clear();
            ViewBag.CanAdd = rights.CanAdd;
            return View("~/Views/MRP/MRP.cshtml", obj);
        }

        public ActionResult GetMRPEntryList()
        {
          //  string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            List<MRPViewModel> list = new List<MRPViewModel>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/MRP", "MRP");
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
                        dt = oDBEngine.GetDataTable("select * from V_MRPDetailsList where BRANCH_ID =" + BranchID + " AND (MRP_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') ");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("select * from V_MRPDetailsList where MRP_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' ");
                    }
                }
               

                TempData["MRPDetailsListDataTable"] = dt;
                if (dt.Rows.Count > 0)
                {
                    MRPViewModel obj = new MRPViewModel();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new MRPViewModel();
                        obj.MRP_ID = Convert.ToString(item["MRP_ID"]);
                        obj.MRPNo = Convert.ToString(item["MRP_No"]);
                        obj.dtMRPDate = Convert.ToDateTime(item["MRP_Date"]);
                        obj.BOM_SCHEMAID = Convert.ToString(item["MRP_SchemaID"]);
                        obj.BOMNo = Convert.ToString(item["BOM_No"]);
                        obj.FinishedItem = Convert.ToString(item["FinishedProductName"]);
                        obj.FinishedQty = Convert.ToString(item["MRPFinishedQty"]);
                        obj.BOMType = Convert.ToString(item["BOM_Type"]);
                        if (Convert.ToString(item["BOM_Date"])!="")
                        {
                            obj.dtBOMDate = Convert.ToDateTime(item["BOM_Date"]);
                        }
                        
                        
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
                        obj.PartNoName = Convert.ToString(item["PartNoName"]);
                        obj.Description = Convert.ToString(item["sProducts_Name"]);
                        obj.DesignNo = Convert.ToString(item["DesignNo"]);
                        obj.ItemRevNo = Convert.ToString(item["ItemRevisionNo"]);
                        obj.MPSNo = Convert.ToString(item["MPS_No"]);
                        obj.MPSDate = Convert.ToString(item["MPS_Date"]);
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
                      //  obj.BOMRelationshipID = Convert.ToString(item["BOMRelation_ID"]);
                        obj.BOMRelationshipNo = Convert.ToString(item["BOMRelation_No"]);
                        obj.UOM_Name =Convert.ToString(item["UOM_Name"]);
                        // REV 1.0
                        obj.Status = Convert.ToString(item["Status"]);
                        //END REV 1.0
                        list.Add(obj);
                    }
                }
            }
            catch { }
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            // REV 1.0
            ViewBag.CanClose = rights.CanClose;
            //END REV 1.0
            // ViewBag.CanAddUpdateDocuments = rights.CanAddUpdateDocuments;

            return PartialView("~/Views/MRP/MRPList.cshtml", list);
        }
        public JsonResult SetMRPID(Int64 Details_ID)
        {
            Boolean Success = false;
            try
            {
                TempData["MRPID"] = Details_ID;                
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }
        public ActionResult GetMRPProductDetailsList()
        {
            //  string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            List<MRPViewModel> list = new List<MRPViewModel>();
            List<MRPProduct> bomproductdata = new List<MRPProduct>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/MRP", "MRP");
            try
            {
                Int64 MRPID = 0;
                MRPProduct bomproductdataobj = new MRPProduct();
                DataTable dt = new DataTable();
                if (TempData["MRPID"] != null )
                {
                    MRPID = Convert.ToInt64(TempData["MRPID"]);                   
                    TempData.Keep();
                }
                if (TempData["MRPID"] != null )
                {
                    if (MRPID > 0)
                    {
                        dt = objdata.GetBOMHeaderEntryListByID("GetBOMEntryProductsEditData", MRPID);
                    }
                    
                }


                TempData["MRPProductDetailsdt"] = dt;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        bomproductdataobj = new MRPProduct();
                        bomproductdataobj.SlNO = Convert.ToString(row["SlNO"]);
                        bomproductdataobj.ProductName = Convert.ToString(row["sProducts_Code"]);
                        bomproductdataobj.ProductId = Convert.ToString(row["ProductID"]);
                        bomproductdataobj.ProductDescription = Convert.ToString(row["sProducts_Name"]);
                        bomproductdataobj.DesignNo = Convert.ToString(row["DesignNo"]);
                        bomproductdataobj.ItemRevisionNo = Convert.ToString(row["ItemRevisionNo"]);
                        bomproductdataobj.ProductQty = Convert.ToString(row["StkQty"]);
                        bomproductdataobj.ProductUOM = Convert.ToString(row["StkUOM"]);
                        bomproductdataobj.Price = Convert.ToString(row["Price"]);
                        bomproductdataobj.Amount = Convert.ToString(row["Amount"]);
                        bomproductdataobj.AvlStk = Convert.ToString(row["AVLSTK"]);
                        bomproductdataobj.IndentQty = Convert.ToString(row["IndentQty"]);

                        bomproductdataobj.PkgQty = Convert.ToString(row["PkgQty"]);
                        bomproductdataobj.PurchaseQty = Convert.ToString(row["PurchaseQty"]);
                        bomproductdataobj.AltQty = Convert.ToString(row["AltQty"]);
                        bomproductdataobj.AltUOM = Convert.ToString(row["AltUOM"]);
                        bomproductdataobj.AltUOMID = Convert.ToString(row["AltUOMID"]);
                        bomproductdataobj.UOmId = Convert.ToString(row["UOM_ID"]);
                        bomproductdataobj.OLDQty = Convert.ToString(row["BalQty"]);
                        bomproductdataobj.OldAltQty = Convert.ToString(row["packing_quantity"]);
                        bomproductdataobj.VendorName = Convert.ToString(row["Vendor_Name"]);
                        //Rev 3.0
                        bomproductdataobj.NewAvlStk = Convert.ToString(row["NewAvlStock"]);
                        //Rev 3.0 End
                        bomproductdata.Add(bomproductdataobj);

                    }
                }
            }
            catch { }
            //ViewBag.CanAdd = rights.CanAdd;
            //ViewBag.CanView = rights.CanView;
            //ViewBag.CanEdit = rights.CanEdit;
            //ViewBag.CanDelete = rights.CanDelete;


            return PartialView("~/Views/MRP/_PartialPreviewLineItem.cshtml", bomproductdata);
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
            ViewData["BOMDetailsListDataTable"] = TempData["MRPDetailsListDataTable"];

            TempData.Keep();
            DataTable dt = (DataTable)TempData["MRPDetailsListDataTable"];
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
                if (datacolumn.ColumnName == "MRP_No" || datacolumn.ColumnName == "BOM_Type"
                    || datacolumn.ColumnName == "BOM_Date" || datacolumn.ColumnName == "REV_No" || datacolumn.ColumnName == "REV_Date" || datacolumn.ColumnName == "FinishedProductName"
                    || datacolumn.ColumnName == "BranchDescription" || datacolumn.ColumnName == "WarehouseName" || datacolumn.ColumnName == "Status" || datacolumn.ColumnName == "CreatedBy" || datacolumn.ColumnName == "CreateDate" || datacolumn.ColumnName == "ModifyBy" || datacolumn.ColumnName == "ModifyDate")
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "MRP_No")
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
                List<MRPBOMList> modelParentBOM = new List<MRPBOMList>();
                DataTable ParentBOMdt = objdata.GetParentBOM(Branch);
                if (ParentBOMdt!=null && ParentBOMdt.Rows.Count > 0)
                {
                    modelParentBOM = APIHelperMethods.ToModelList<MRPBOMList>(ParentBOMdt);
                    ViewBag.ParentBOMID = ParentBOMID;
                }

                return PartialView("~/Views/MRP/_PartialParentBOM.cshtml", modelParentBOM);
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

        [WebMethod]
        public JsonResult GetBOMDetails(String BOMID)
        {
            BOMDetailsList ret = new BOMDetailsList();
            DataTable dt = objdata.GetBOMDetails(BOMID);            
            if (dt != null && dt.Rows.Count > 0)
            {
                ret.Finished_ProductID = dt.Rows[0]["Finished_ProductID"].ToString();
                ret.sProducts_Name = dt.Rows[0]["sProducts_Name"].ToString();
                ret.REV_No = dt.Rows[0]["REV_No"].ToString();
                ret.BOM_Date = dt.Rows[0]["BOM_Date"].ToString();
                ret.Finished_Qty = dt.Rows[0]["Finished_Qty"].ToString();
                ret.REV_Date = dt.Rows[0]["REV_Date"].ToString();

                ret.PartNo = dt.Rows[0]["PartNo"].ToString();
                ret.DESIGNNO = dt.Rows[0]["DESIGNNO"].ToString();
                ret.REVISIONNO = dt.Rows[0]["REVISIONNO"].ToString();
                ret.UOM_NAME = dt.Rows[0]["UOM_NAME"].ToString();
                ret.BOMRelation_No = dt.Rows[0]["BOMRelation_No"].ToString();
                   
            }
            

            return Json(ret);
        }
        public class BOMDetailsList
        {
            public String Finished_ProductID { get; set; }
            public String sProducts_Name { get; set; }
            public String REV_No { get; set; }
            public String BOM_Date { get; set; }
            public String Finished_Qty { get; set; }
            public String REV_Date { get; set; }
            public String PartNo { get; set; }

            public String DESIGNNO { get; set; }
            public String REVISIONNO { get; set; }
            public String UOM_NAME { get; set; }
            public String BOMRelation_No { get; set; }
            
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
        public JsonResult SetTempMPSID(Int64 MPSID, Int64 FGProductID, string FGQTY)
        {
            if (MPSID > 0)
            {
                TempData["MPSID"] = MPSID;
                TempData["FGProductID"] = FGProductID;
                TempData["FGQTY"] = FGQTY;
                TempData.Keep();
            }
            else
            {
                TempData["MPSID"] = null;
                TempData["FGProductID"] = null;
                TempData["FGQTY"] = null;
                TempData.Clear();
            }
            return Json(true);
        }
        public JsonResult SetTempMPSIDAllFG(Int64 MPSID, string FGQTY)
        {
            if (MPSID > 0)
            {
                TempData["MPSID"] = MPSID;               
                TempData["FGQTY"] = FGQTY;
                TempData.Keep();
            }
            else
            {
                TempData["MPSID"] = null;                
                TempData["FGQTY"] = null;
                TempData.Clear();
            }
            return Json(true);
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
        public JsonResult GetMPSDetails(String MPSID)
        {
            BOMDetailsList ret = new BOMDetailsList();
            DataTable dt = objdata.GetMPSDetails(MPSID);
            if (dt != null && dt.Rows.Count > 0)
            {
                //ret.Finished_ProductID = dt.Rows[0]["Finished_ProductID"].ToString();
                ret.sProducts_Name = dt.Rows[0]["sProducts_Name"].ToString();
                //ret.REV_No = dt.Rows[0]["REV_No"].ToString();
                //ret.BOM_Date = dt.Rows[0]["BOM_Date"].ToString();
                ret.Finished_Qty = dt.Rows[0]["Finished_Qty"].ToString();
                //ret.REV_Date = dt.Rows[0]["REV_Date"].ToString();

               // ret.PartNo = dt.Rows[0]["PartNo"].ToString();
                //ret.DESIGNNO = dt.Rows[0]["DESIGNNO"].ToString();
               // ret.REVISIONNO = dt.Rows[0]["REVISIONNO"].ToString();
               // ret.UOM_NAME = dt.Rows[0]["UOM_NAME"].ToString();
                ret.BOMRelation_No = dt.Rows[0]["BOMRelation_No"].ToString();

            }


            return Json(ret);
        }

        public ActionResult GetMRPDetailsList()
        {
            //  string ProjectSelectInEntryModule = cSOrder.GetSystemSettingsResult("ProjectSelectInEntryModule");
            List<MRPViewModel> list = new List<MRPViewModel>();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionMVC("/MRP", "MRP");
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
                        dt = oDBEngine.GetDataTable("select * from V_MRPDetailsList where BRANCH_ID =" + BranchID + " AND (MRP_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "') ");
                    }
                    else
                    {
                        dt = oDBEngine.GetDataTable("select * from V_MRPDetailsList where MRP_Date BETWEEN '" + Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd") + "'  AND '" + Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd") + "' ");
                    }
                }


                TempData["MRPDetailsListDataTable"] = dt;
                if (dt.Rows.Count > 0)
                {
                    MRPViewModel obj = new MRPViewModel();
                    foreach (DataRow item in dt.Rows)
                    {
                        obj = new MRPViewModel();
                        obj.MRP_ID = Convert.ToString(item["MRP_ID"]);
                        obj.MRPNo = Convert.ToString(item["MRP_No"]);
                        obj.dtMRPDate = Convert.ToDateTime(item["MRP_Date"]);
                        obj.BOM_SCHEMAID = Convert.ToString(item["MRP_SchemaID"]);
                        obj.BOMNo = Convert.ToString(item["BOM_No"]);
                        obj.FinishedItem = Convert.ToString(item["FinishedProductName"]);
                        obj.FinishedQty = Convert.ToString(item["MRPFinishedQty"]);
                        obj.BOMType = Convert.ToString(item["BOM_Type"]);
                        if (Convert.ToString(item["BOM_Date"]) != "")
                        {
                            obj.dtBOMDate = Convert.ToDateTime(item["BOM_Date"]);
                        }


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
                        obj.PartNoName = Convert.ToString(item["PartNoName"]);
                        obj.Description = Convert.ToString(item["sProducts_Name"]);
                        obj.DesignNo = Convert.ToString(item["DesignNo"]);
                        obj.ItemRevNo = Convert.ToString(item["ItemRevisionNo"]);
                        obj.MPSNo = Convert.ToString(item["MPS_No"]);
                        obj.MPSDate = Convert.ToString(item["MPS_Date"]);
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
                        
                        obj.BOMRelationshipNo = Convert.ToString(item["BOMRelation_No"]);
                        obj.UOM_Name = Convert.ToString(item["UOM_Name"]);
                        //REV 1.0
                        obj.Status = Convert.ToString(item["Status"]);
                        //END REV 1.0
                        list.Add(obj);
                    }
                }
            }
            catch { }
            ViewBag.CanAdd = rights.CanAdd;
            ViewBag.CanView = rights.CanView;
            ViewBag.CanEdit = rights.CanEdit;
            ViewBag.CanDelete = rights.CanDelete;
            //REV 1.0
            ViewBag.CanClose = rights.CanClose;
            //END REV 1.0
            // ViewBag.CanAddUpdateDocuments = rights.CanAddUpdateDocuments;

            return PartialView("~/Views/MRP/MRPList.cshtml", list);
        }

        public ActionResult ExportPreviewLineGridList(int type)
        {
            ViewData["MRPProductDetailsdt"] = TempData["MRPProductDetailsdt"];

            TempData.Keep();
            DataTable dt = (DataTable)TempData["MRPProductDetailsdt"];
            if (ViewData["MRPProductDetailsdt"] != null && dt.Rows.Count > 0)
            {

                switch (type)
                {
                    case 1:
                        return GridViewExtension.ExportToPdf(GetPreviewLineGridView(ViewData["MRPProductDetailsdt"]), ViewData["MRPProductDetailsdt"]);
                    //break;
                    case 2:
                        return GridViewExtension.ExportToXlsx(GetPreviewLineGridView(ViewData["MRPProductDetailsdt"]), ViewData["MRPProductDetailsdt"]);
                    //break;
                    case 3:
                        return GridViewExtension.ExportToXls(GetPreviewLineGridView(ViewData["MRPProductDetailsdt"]), ViewData["MRPProductDetailsdt"]);
                    //break;
                    case 4:
                        return GridViewExtension.ExportToRtf(GetPreviewLineGridView(ViewData["MRPProductDetailsdt"]), ViewData["MRPProductDetailsdt"]);
                    //break;
                    case 5:
                        return GridViewExtension.ExportToCsv(GetPreviewLineGridView(ViewData["MRPProductDetailsdt"]), ViewData["MRPProductDetailsdt"]);
                    default:
                        break;
                }
                return null;
            }
            else
            {
                return this.RedirectToAction("MRP", "MRP");
            }
        }

        private GridViewSettings GetPreviewLineGridView(object datatable)
        {           
            var settings = new GridViewSettings();
            settings.Name = "Preview Line Items";
            settings.SettingsExport.ExportedRowType = GridViewExportedRowType.All;
            settings.SettingsExport.FileName = "Material Requirement Planning (MRP)";           
            TempData.Keep();
            DataTable dt = (DataTable)datatable;

            foreach (System.Data.DataColumn datacolumn in dt.Columns)
            {
                if (datacolumn.ColumnName == "SLNO" || datacolumn.ColumnName == "SPRODUCTS_CODE"
                    || datacolumn.ColumnName == "SPRODUCTS_NAME" || datacolumn.ColumnName == "DESIGNNO" || datacolumn.ColumnName == "ITEMREVISIONNO" || datacolumn.ColumnName == "STKQTY"
                    || datacolumn.ColumnName == "STKUOM" || datacolumn.ColumnName == "PRICE" || datacolumn.ColumnName == "Amount" || datacolumn.ColumnName == "IndentQty"
                    || datacolumn.ColumnName == "PkgQty" || datacolumn.ColumnName == "PurchaseQty" || datacolumn.ColumnName == "AltQty"
                    || datacolumn.ColumnName == "AltUOM" || datacolumn.ColumnName == "Vendor_Name" || datacolumn.ColumnName == "AVLSTK"
                    //rev 3.0
                    || datacolumn.ColumnName == "NewAvlStock"
                    //rev 3.0 end
                    )
                {
                    settings.Columns.Add(column =>
                    {
                        if (datacolumn.ColumnName == "SLNO")
                        {
                            column.Caption = "SLNO";
                            column.VisibleIndex = 0;
                        }
                        else if (datacolumn.ColumnName == "SPRODUCTS_CODE")
                        {
                            column.Caption = "PRODUCTS CODE";
                            column.VisibleIndex = 1;
                        }
                        else if (datacolumn.ColumnName == "SPRODUCTS_NAME")
                        {
                            column.Caption = "PRODUCTS NAME";
                            column.VisibleIndex = 2;

                        }
                        else if (datacolumn.ColumnName == "DESIGNNO")
                        {
                            column.Caption = "DESIGN NO";
                            column.VisibleIndex = 3;
                        }
                        else if (datacolumn.ColumnName == "ITEMREVISIONNO")
                        {
                            column.Caption = "ITEM REVISION NO";
                            column.VisibleIndex = 4;
                        }
                        else if (datacolumn.ColumnName == "STKQTY")
                        {
                            column.Caption = "QTY";
                            column.VisibleIndex = 5;
                        }
                        else if (datacolumn.ColumnName == "STKUOM")
                        {
                            column.Caption = "UOM";
                            column.VisibleIndex = 6;
                        }
                        else if (datacolumn.ColumnName == "PRICE")
                        {
                            column.Caption = "PRICE";
                            column.VisibleIndex = 7;
                        }
                        else if (datacolumn.ColumnName == "Amount")
                        {
                            column.Caption = "Amount";
                            column.VisibleIndex = 8;
                        }
                        //rev 3.0
                        else if (datacolumn.ColumnName == "AVLSTK")
                        {
                            column.Caption = "Phy Stk";
                            column.VisibleIndex = 9;

                        }
                        else if (datacolumn.ColumnName == "NewAvlStock")
                        {
                            column.Caption = "Avl Stk";
                            column.VisibleIndex = 10;
                        }
                        //rev 3.0 end
                        else if (datacolumn.ColumnName == "IndentQty")
                        {
                            column.Caption = "Indent Qty";
                            column.VisibleIndex = 11;
                        }
                        else if (datacolumn.ColumnName == "PkgQty")
                        {
                            column.Caption = "Pkg Qty";
                            column.VisibleIndex = 12;
                        }
                        else if (datacolumn.ColumnName == "PurchaseQty")
                        {
                            column.Caption = "Purchase Qty";
                            column.VisibleIndex = 13;
                        }
                        else if (datacolumn.ColumnName == "AltQty")
                        {
                            column.Caption = "Alt Qty";
                            column.VisibleIndex = 14;
                        }
                        else if (datacolumn.ColumnName == "AltUOM")
                        {
                            column.Caption = "Alt UOM";
                            column.VisibleIndex = 15;
                        }
                        else if (datacolumn.ColumnName == "Vendor_Name")
                        {
                            column.Caption = "Vendor Name";
                            column.VisibleIndex = 16;
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

        //REV 1.0

        [WebMethod]
        public JsonResult ClosedMRPDataByID(Int32 detailsid, String ClosedMRPRemarks = "")
        {
            ReturnData obj = new ReturnData();
            try
            {
                var datasetobj = objdata.DropDownDetailForBOM("ClosedMRPData", null, null, null, 0, detailsid, ClosedMRPRemarks);
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
        //END REV 1.0
    }
    
}