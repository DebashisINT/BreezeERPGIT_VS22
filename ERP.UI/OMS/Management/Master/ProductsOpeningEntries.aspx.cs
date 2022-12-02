using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using ERP.OMS.Management.Activities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class ProductsOpeningEntries : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public static EntityLayer.CommonELS.UserRightsForPage rights;
        MasterSettings objmaster = new MasterSettings();

        //Rev Bapi
        protected void Page_Init(object sender, EventArgs e)
        {


          
            UomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            AltUomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        //Rev Bapi End
        protected void Page_Load(object sender, EventArgs e)
        {

           
            if (HttpContext.Current.Session["userid"] != null)
            {
                CommonBL ComBL = new CommonBL();
                if (!IsPostBack)
                {
                    rights = new UserRightsForPage();
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/ProductsOpeningEntries.aspx");


                    MasterSettings masterBl = new MasterSettings();
                    hdnmultiwarehouse.Value = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                    hdnShowUOMConversionInEntry.Value = Convert.ToString(masterBl.GetSettings("ShowUOMConversionInEntry"));

                    //Bapi added for MUltiUOM settings start           
                    string MultiUOMSelection = ComBL.GetSystemSettingsResult("MultiUOMSelection");
                    if (!String.IsNullOrEmpty(MultiUOMSelection))
                    {
                        if (MultiUOMSelection.ToUpper().Trim() == "YES")
                        {
                            hddnMultiUOMSelection.Value = "1";
                            multiuom.Style.Add("display", "block");
                        }
                        else if (MultiUOMSelection.ToUpper().Trim() == "NO")
                        {
                            hddnMultiUOMSelection.Value = "0";
                            multiuom.Style.Add("display", "none");
                            
                        }
                    }
                    //End


                    Session["MultiUOMData"] = null;
                    GetBranchDetails();
                    BindWarehouse();

                    #region To Get Sotal Opening Value

                    string TotalSum = "0.00";
                    //DataTable computeDT = GetOpeningStockDetails();
                    //if (computeDT != null && computeDT.Rows.Count > 0)
                    //{
                    //    object sumObject = computeDT.Compute("Sum(OpeningValue)", "");
                    //    TotalSum = Convert.ToString(sumObject);
                    //}
                    //lblTotalSum.Text = TotalSum;

                    #endregion

                    OpeningGrid.DataSource = GetGriddata();
                    OpeningGrid.DataBind();

                    #region Barcode Section

                    if (IsBarcodeGeneratete() == true)
                    {
                        btnGenerate.Visible = true;
                        btnPrint.Visible = true;

                        OpeningGrid.Columns["IsAllBarcodeGenerate"].Visible = true;
                        OpeningGrid.Columns["IsAllPrint"].Visible = true;

                        hdfIsBarcodeActive.Value = "Y";
                        hdfIsBarcodeGenerator.Value = "Y";


                        MultiWarehouceuc.uchdfIsBarcodeActive.Value = "Y";
                        MultiWarehouceuc.uchdfIsBarcodeGenerator.Value = "Y";

                    }
                    else
                    {
                        btnGenerate.Visible = false;
                        btnPrint.Visible = false;

                        OpeningGrid.Columns["IsAllBarcodeGenerate"].Visible = false;
                        OpeningGrid.Columns["IsAllPrint"].Visible = false;

                        hdfIsBarcodeActive.Value = "N";
                        hdfIsBarcodeGenerator.Value = "N";


                        MultiWarehouceuc.uchdfIsBarcodeActive.Value = "N";
                        MultiWarehouceuc.uchdfIsBarcodeGenerator.Value = "N";
                    }

                    //btnGenerate.Visible = false;
                    //btnPrint.Visible = false;
                    //OpeningGrid.Columns["IsAllBarcodeGenerate"].Visible = false;
                    //OpeningGrid.Columns["IsAllPrint"].Visible = false;
                    //hdfIsBarcodeActive.Value = "N";

                    //if (IsBarcodeGeneratete() == true) hdfIsBarcodeGenerator.Value = "Y";
                    //else hdfIsBarcodeGenerator.Value = "N";

                    #endregion

                   
                    hdnConvertionOverideVisible.Value = objmaster.GetSettings("ConvertionOverideVisible");
                    hdnShowUOMConversionInEntry.Value = objmaster.GetSettings("ShowUOMConversionInEntry");
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        #region Other Event

        public void GetBranchDetails()
        {
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string userbranchID = Convert.ToString(Session["userbranchID"]);

            DataTable dt = GetBranchDetails(userbranchHierarchy);
            if (dt != null && dt.Rows.Count > 0)
            {
                cmbbranch.DataSource = dt;
                cmbbranch.DataBind();

                if (Session["userbranchID"] != null)
                {
                    cmbbranch.Value = userbranchID;
                }
            }
        }
        public DataTable GetBranchDetails(string userbranchHierarchy)
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_GetOpeningStockEntrys");
                proc.AddVarcharPara("@Action", 3000, "GetBranch");
                proc.AddVarcharPara("@BranchList", 3000, userbranchHierarchy);
                dt = proc.GetTable();
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region Export Event

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport(Filter);
            }
        }
        public void bindexport(int Filter)
        {
            if (IsBarcodeGeneratete() == true)
            {
                openingGridExport.Columns[5].Visible = true;
                openingGridExport.Columns[6].Visible = true;
            }
            else
            {
                openingGridExport.Columns[5].Visible = false;
                openingGridExport.Columns[6].Visible = false;
            }

            string filename = "Opening Balances - Product";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Opening Balances - Product";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            exporter.GridViewID = "openingGridExport";
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
            drdExport.SelectedIndex = 0;
        }

        #endregion

        #region Grid Event

        protected void Grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void OpeningGrid_DataBinding(object sender, EventArgs e)
        {
            OpeningGrid.DataSource = GetGriddata();
        }
        protected void openingGridExport_DataBinding(object sender, EventArgs e)
        {
            openingGridExport.DataSource = GetOpeningStockDetailsForExport();
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
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
        protected void OpeningGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];

            if (strSplitCommand == "FinalSubmit")
            {
                string ProductID = Convert.ToString(hdfProductID.Value);
                string CompanyID = Convert.ToString(Session["LastCompany"]);
                string FinYear = Convert.ToString(Session["LastFinYear"]);
                string BranchID = Convert.ToString(cmbbranch.Value);
                string ProductQuantity = "0", validate = "";
                decimal strWarehouseQuantity = 0;
                
                if (Convert.ToString(hdnJsonProductStock.Value).Trim() != "")
                {
                    string StockDetails = Convert.ToString(hdnJsonProductStock.Value);
                    JavaScriptSerializer _ser = new JavaScriptSerializer();
                    _ser.MaxJsonLength = Int32.MaxValue;
                    List<ProductStockDetails> StockEntryJson = _ser.Deserialize<List<ProductStockDetails>>(StockDetails);
                    DataTable _Stockdt = ToDataTable(StockEntryJson);

                    DataTable tempWarehousedt = new DataTable();
                    DataTable tempBarcodedt = new DataTable();

                    if (_Stockdt != null)
                    {
                        DataTable Warehousedt = _Stockdt.Copy();
                        if (hdnaddeditwhensave.Value == "Edit")
                        {
                            tempWarehousedt = Warehousedt.DefaultView.ToTable(false, "Product_SrlNo", "LoopID", "WarehouseID", "Quantity", "ViewBatch", "SerialNo", "ViewMfgDate", "ViewExpiryDate", "Value", "Rate", "AlterQty", "AltUOM");//Rajdip                        
                       
                        
                           foreach (DataRow wtrow in tempWarehousedt.Rows)
                            {
                                string strAltQty = Convert.ToString(wtrow["AlterQty"]).Trim();
                                string strAltUOM = Convert.ToString(wtrow["AltUOM"]).Trim();

                                if (strAltQty == "")
                                {
                                    int DD = 0;


                                    wtrow["AlterQty"] = DD;
                                }
                                if(strAltUOM=="")
                                {

                                    decimal dd = Convert.ToDecimal(0.00);
                                    wtrow["AltUOM"] = dd;
                                }
                            }
                        
                        
                        }
                        else
                        {
                            tempWarehousedt = Warehousedt.DefaultView.ToTable(false, "Product_SrlNo", "LoopID", "WarehouseID", "Quantity", "Batch", "SerialNo", "MfgDate", "ExpiryDate", "Value", "Rate", "AlterQty", "AltUOM");//Rajdip


                            foreach (DataRow wtrow in tempWarehousedt.Rows)
                            {
                                string strAltQty = Convert.ToString(wtrow["AlterQty"]).Trim();
                                string strAltUOM = Convert.ToString(wtrow["AltUOM"]).Trim();

                                if (strAltQty == "")
                                {
                                    int DD = 0;


                                    wtrow["AlterQty"] = DD;
                                }
                                if(strAltUOM=="")
                                {

                                    //int dd = 1;//Convert.ToDecimal(0.00);
                                    //wtrow["AltUOM"] = dd;

                                    int dd = Convert.ToInt16(0);//0023355
                                    wtrow["AltUOM"] = dd;
                                }
                            }
                        
                        }
                           
                        tempBarcodedt = Warehousedt.DefaultView.ToTable(false, "Barcode");
                    }
                    else
                    {
                        tempWarehousedt.Columns.Add("Product_SrlNo", typeof(string));
                        tempWarehousedt.Columns.Add("SrlNo", typeof(string));
                        tempWarehousedt.Columns.Add("WarehouseID", typeof(string));
                        tempWarehousedt.Columns.Add("Quantity", typeof(string));
                        tempWarehousedt.Columns.Add("Batch", typeof(string));
                        tempWarehousedt.Columns.Add("SerialNo", typeof(string));
                        tempWarehousedt.Columns.Add("MfgDate", typeof(string));
                        tempWarehousedt.Columns.Add("ExpiryDate", typeof(string));
                        tempWarehousedt.Columns.Add("Rate", typeof(string));

                        tempWarehousedt.Columns.Add("Barcode", typeof(string));
                    }


                    //GetQuantityBaseOnProduct(ProductID, ref strWarehouseQuantity);
                    ProductQuantity = Convert.ToString(strWarehouseQuantity);

                    if (validate == "checkWarehouse")
                    {
                        OpeningGrid.JSProperties["cpfinalMsg"] = validate;
                    }
                    else
                    {
                        int strIsComplete = 0;
                        if (hdnaddeditwhensave.Value == "Edit")
                        {
                            foreach (DataRow dr in tempWarehousedt.Rows)
                            {
                                string MfgDate = Convert.ToString(dr["ViewMfgDate"]);
                                string ExpiryDate = Convert.ToString(dr["ViewExpiryDate"]);

                                dr["ViewMfgDate"] = DateFormat(MfgDate);
                                dr["ViewExpiryDate"] = DateFormat(ExpiryDate);
                            }
                            tempWarehousedt.AcceptChanges();
                         
                        }
                        else
                        { 
                        foreach (DataRow dr in tempWarehousedt.Rows)
                        {
                            string MfgDate = Convert.ToString(dr["MfgDate"]);
                            string ExpiryDate = Convert.ToString(dr["ExpiryDate"]);

                            dr["MfgDate"] = DateFormat(MfgDate);
                            dr["ExpiryDate"] = DateFormat(ExpiryDate);
                        }
                        tempWarehousedt.AcceptChanges();
                        }

                        #region Add New Filed To Check from Table

                        DataTable duplicatedt2 = new DataTable();
                        duplicatedt2.Columns.Add("productid", typeof(Int64));
                        duplicatedt2.Columns.Add("slno", typeof(Int32));
                        duplicatedt2.Columns.Add("Quantity", typeof(Decimal));
                        duplicatedt2.Columns.Add("packing", typeof(Decimal));
                        duplicatedt2.Columns.Add("PackingUom", typeof(Int32));
                        duplicatedt2.Columns.Add("PackingSelectUom", typeof(Int32));

                        if (HttpContext.Current.Session["SessionPackingDetails"] != null)
                        {
                            List<ProductQuantity> obj = new List<ProductQuantity>();
                            obj = (List<ProductQuantity>)HttpContext.Current.Session["SessionPackingDetails"];
                            foreach (var item in obj)
                            {
                                duplicatedt2.Rows.Add(item.productid, item.slno, item.Quantity, item.packing, item.PackingUom, item.PackingSelectUom);
                            }
                        }
                        HttpContext.Current.Session["SessionPackingDetails"] = null;
                        #endregion
                        //Rev Bapi

                        DataTable MultiUOMDetails = new DataTable();

                        if (Session["MultiUOMData"] != null)
                        {
                            DataTable MultiUOM = (DataTable)Session["MultiUOMData"];
                            // Rev Sanchita
                            //MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId");
                            MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId", "BaseRate", "AltRate", "UpdateRow");
                            // End of Rev Sanchita
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
                            // Rev sanchita
                            MultiUOMDetails.Columns.Add("BaseRate", typeof(Decimal));
                            MultiUOMDetails.Columns.Add("AltRate", typeof(Decimal));
                            MultiUOMDetails.Columns.Add("UpdateRow", typeof(string));
                            // End of Rev Sanchita
                        }

                        if (hddnMultiUOMSelection.Value == "1")
                        {
                            if (Session["MultiUOMData"] != null && MultiUOMDetails.Rows.Count > 0)
                            {

                                ModifyStockOpening(ProductID, CompanyID, FinYear, BranchID, ProductQuantity, tempWarehousedt, tempBarcodedt, ref strIsComplete, duplicatedt2, MultiUOMDetails);

                            }
                            else 
                            {
                                validate = "checkMultiUOMData";
                              
                                OpeningGrid.DataSource = GetGriddata();
                                OpeningGrid.DataBind();
                                strIsComplete = 6;


                            }
                        }
                        

                        else
                        {
                            ModifyStockOpening(ProductID, CompanyID, FinYear, BranchID, ProductQuantity, tempWarehousedt, tempBarcodedt, ref strIsComplete, duplicatedt2, MultiUOMDetails);

                        }
                        //End Rev Bapi
                        hdnaddeditwhensave.Value = "Add";
                        if (tempWarehousedt.Rows.Count > 0)
                        {
                            if (strIsComplete == 1)
                            {
                                OpeningGrid.JSProperties["cpfinalMsg"] = "SuccesInsert";
                            }
                            else if (strIsComplete==3)
                            {
                                OpeningGrid.JSProperties["cpfinalMsg"] = "negativestock";
                             }
                                //Rev Bapi
                            else if (strIsComplete == 6)
                            {
                                OpeningGrid.JSProperties["cpfinalMsg"] = "checkMultiUOMData";
                            }
                                //End rev Bapi
                            else
                            {
                                OpeningGrid.JSProperties["cpfinalMsg"] = "errorrInsert";
                            }
                        }
                        else if (strIsComplete == 3)
                        {
                            OpeningGrid.JSProperties["cpfinalMsg"] = "negativestock";
                        }

                                //Rev Bapi
                            
                        else if (strIsComplete == 6)
                        {
                            OpeningGrid.JSProperties["cpfinalMsg"] = "checkMultiUOMData";
                        }
                        //End rev Bapi
                        else
                        {
                            OpeningGrid.JSProperties["cpfinalMsg"] = "nullStock";
                        }

                        OpeningGrid.DataSource = GetGriddata();
                        OpeningGrid.DataBind();
                    }
                }
                else
                {
                    OpeningGrid.JSProperties["cpfinalMsg"] = "nullStock";
                }
            }
            else if (strSplitCommand == "ReBindGrid")
            {
                OpeningGrid.DataSource = GetGriddata();
                OpeningGrid.DataBind();
            }
            else if (strSplitCommand == "GenerateBarcode")
             {
                string strProductID = Convert.ToString(e.Parameters.Split('~')[1]);
                string strBranchID = Convert.ToString(cmbbranch.Value);

                if (IsBarcodeActive(strProductID) == true)
                {
                    int strIsComplete = 0;
                    GenerateBarcode(strProductID, strBranchID, ref strIsComplete);

                    if (strIsComplete == 1)
                    {
                        OpeningGrid.JSProperties["cpMessage"] = "Generated";
                    }
                    else if (strIsComplete == -10)
                    {
                        OpeningGrid.JSProperties["cpMessage"] = "Error";
                    }
                    else if (strIsComplete == -20)
                    {
                        OpeningGrid.JSProperties["cpMessage"] = "NullStock";
                    }
                    else if (strIsComplete == -30)
                    {
                        OpeningGrid.JSProperties["cpMessage"] = "NullBarcode";
                    }
                    else if (strIsComplete == -30)
                    {
                        OpeningGrid.JSProperties["cpMessage"] = "NullBarcode";
                    }
                    else
                    {
                        OpeningGrid.JSProperties["cpMessage"] = "Error";
                    }
                }
                else
                {
                    OpeningGrid.JSProperties["cpMessage"] = "BarcodeInactive";
                }

                OpeningGrid.DataSource = GetGriddata();
                OpeningGrid.DataBind();
            }
            else if (strSplitCommand == "PrintBarcode")
            {

                //Session["S_UPDATEFORBARCODE"] = 0;
                string ProductID = Convert.ToString(e.Parameters.Split('~')[1]);
                string BranchID = Convert.ToString(cmbbranch.Value);
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                dt = oDBEngine.GetDataTable("select * from tbl_master_productbarcode where ProductID='" + ProductID + "' and BranchID='" + BranchID + "' and IsPrint=0 ");
                if (dt.Rows.Count == 0)
                {
                    OpeningGrid.JSProperties["cpMessage"] = "BarcodeNotPresent";
                }
                else
                {
                    dt1 = oDBEngine.GetDataTable("select sum(sbw.StockBranchWarehouse_StockIn) from Trans_StockBranchWarehouse sbw inner join Trans_StockBranchWarehouseDetails sbwd on sbw.StockBranchWarehouse_Id=sbwd.StockBranchWarehouse_Id where sbwd.Doc_Type='OP' and sbwd.StockBranchWarehouseDetail_ProductId='" + ProductID + "' and sbw.StockBranchWarehouse_BranchId='" + BranchID + "'");
                    if (dt1.Rows.Count == 0)
                    {
                        OpeningGrid.JSProperties["cpMessage"] = "StockNotPresent";
                    }
                    else
                    {
                        OpeningGrid.JSProperties["cpMessage"] = "BarcodeStockPresent";
                    }
                }

                OpeningGrid.DataSource = GetGriddata();
                OpeningGrid.DataBind();
            }

            #region To Get Sotal Opening Value

            string TotalSum = "0.00";
            //DataTable computeDT = GetOpeningStockDetails();

            //if (computeDT != null && computeDT.Rows.Count > 0)
            //{
            //    object sumObject = computeDT.Compute("Sum(OpeningValue)", "");
            //    TotalSum = Convert.ToString(sumObject);
            //}

            OpeningGrid.JSProperties["cpTotalSum"] = TotalSum;

            #endregion
        }
        public string DateFormat(string Date)
        {
            if (Date != "")
            {
                string Day = Date.Substring(0, 2);
                string Month = Date.Substring(3, 2);
                string Year = Date.Substring(6, 4);

                Date = Year + "-" + Month + "-" + Day;
            }

            return Date;
        }
        private IEnumerable GetGriddata()
        {
            List<Openinglist> Openinglists = new List<Openinglist>();
            DataTable dt = GetOpeningStockDetails();

            if (dt != null)
            {
                Openinglists = (from DataRow dr in dt.Rows
                                select new Openinglist()
                                {
                                    StockID = Convert.ToString(dr["StockID"]),
                                    ProductID = Convert.ToString(dr["ProductID"]),
                                    ProductCode = Convert.ToString(dr["ProductCode"]),
                                    ViewProductName = Convert.ToString(dr["ProductName"]),
                                    ProductName = Convert.ToString(dr["ProductName"]).Replace("'", "squot").Replace(",", "coma").Replace("/", "slash"),
                                    //ProductName = dr["ProductName"].ToString().Replace("'", "squot").Replace(",", "coma").Replace("/", "slash"),

                                    UOM = Convert.ToString(dr["UOM"]),
                                    AvailableStock = Convert.ToDecimal(dr["OpeningQty"]),
                                    OpeningStock = Convert.ToDecimal(dr["OpeningQty"]),
                                    OpeningValue = Convert.ToDecimal(dr["OpeningValue"]),
                                    InventoryType = Convert.ToString(dr["InventoryType"]),
                                    IsInventoryEnable = Convert.ToString(dr["IsInventoryEnable"]),
                                    DefaultWarehouse = Convert.ToString(dr["DefaultWarehouse"]),
                                    IsAllBarcodeGenerate = Convert.ToString(dr["IsAllBarcodeGenerate"]),
                                    IsAllPrint = Convert.ToString(dr["IsAllPrint"]),
                                    Alterqty=Convert.ToString(dr["StockBranchWarehouse_AltStockIn"]),
                                    AltUOM=Convert.ToString(dr["UOM"])
                                }).ToList();

            }

            return Openinglists;
        }
        public int GetvisibleIndex(object container)
        {
            GridViewDataItemTemplateContainer c = container as GridViewDataItemTemplateContainer;
            return c.VisibleIndex;
        }
        public DataTable GetOpeningStockDetails()
        {
            try
            {
                string BranchID = Convert.ToString(cmbbranch.Value);
                string CompanyID = Convert.ToString(Session["LastCompany"]);
                string FinYear = Convert.ToString(Session["LastFinYear"]);

                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_GetOpeningStockEntrys");
                proc.AddVarcharPara("@Action", 3000, "GetOpeningStock");
                proc.AddVarcharPara("@BranchID", 3000, BranchID);
                proc.AddVarcharPara("@CompanyID", 3000, CompanyID);
                proc.AddVarcharPara("@FinYear", 3000, FinYear);
                dt = proc.GetTable();
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetOpeningStockDetailsForExport()
        {
            try
            {
                string BranchID = Convert.ToString(cmbbranch.Value);
                string CompanyID = Convert.ToString(Session["LastCompany"]);
                string FinYear = Convert.ToString(Session["LastFinYear"]);

                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_GetOpeningStockEntrys");
                proc.AddVarcharPara("@Action", 3000, "GetOpeningStockForExport");
                proc.AddVarcharPara("@BranchID", 3000, BranchID);
                proc.AddVarcharPara("@CompanyID", 3000, CompanyID);
                proc.AddVarcharPara("@FinYear", 3000, FinYear);
                dt = proc.GetTable();
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public void ModifyStockOpening(string ProductID, string CompanyID, string FinYear, string BranchID, string ProductQuantity, DataTable tempWarehousedt, DataTable tempBarcodedt, ref int strIsComplete, DataTable QuotationPackingDetailsdt, DataTable MultiUOMDetails)
        {
            try
            {
                DataSet dsInst = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proc_OpeningStock_Modify", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProductID", ProductID);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@FinYear", FinYear);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@ProductQuantity", ProductQuantity);
                cmd.Parameters.AddWithValue("@WarehouseDetail", tempWarehousedt);
                cmd.Parameters.AddWithValue("@BarcodeList", tempBarcodedt);

                cmd.Parameters.AddWithValue("@QuotationPackingDetails", QuotationPackingDetailsdt); //Surojit 20-03-2019
                //Rev Bapi
                cmd.Parameters.AddWithValue("@MultiUOMDetails", MultiUOMDetails);
                //End Rev Bapi

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);

                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        public bool GenerateBarcode(string strProductID, string strBranchID, ref int strIsComplete)
        {
            try
            {
                DataSet dsInst = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MUlti
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("proc_GenerateBarcode_DocumentWise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductID", strProductID);
                cmd.Parameters.AddWithValue("@BranchID", strBranchID);
                cmd.Parameters.AddWithValue("@Doc_Type", "OP");
                cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(Session["userid"]));

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());

                cmd.Dispose();
                con.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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
        public bool IsBarcodeActive(string Products_ID)
        {
            bool IsActive = false;
            string query = @"Select 'Y' From Master_sProducts Where sProducts_ID='" + Products_ID + "' AND IsNull(Is_BarCode_Active,0)=1";

            //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
            DataTable dt = oDbEngine.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                IsActive = true;
            }

            return IsActive;
        }
        public class Openinglist
        {
            public string StockID { get; set; }
            public string ProductID { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string ViewProductName { get; set; }
            public string UOM { get; set; }
            public decimal AvailableStock { get; set; }
            public decimal OpeningStock { get; set; }
            public decimal OpeningValue { get; set; }
            public string InventoryType { get; set; }
            public string IsInventoryEnable { get; set; }
            public string DefaultWarehouse { get; set; }
            public string IsAllBarcodeGenerate { get; set; }
            public string IsAllPrint { get; set; }
            public string Alterqty { get; set; }
            public string AltUOM { get; set; }
        }

        #endregion

        #region Barcode Section

        protected void CallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];

            if (strSplitCommand == "GridBindByBranch")
            {
                OpeningGrid.DataSource = GetGriddata();
                OpeningGrid.DataBind();

                BindWarehouse();
            }
        }
        public void BindWarehouse()
        {
            try
            {
                string strBranch = Convert.ToString(cmbbranch.Value);

                DataTable dt = new DataTable();

                if (hdnmultiwarehouse.Value != "1")
                {
                    dt = oDBEngine.GetDataTable("select  bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + strBranch + "') order by bui_Name");
                }
                else
                {
                    dt = oDBEngine.GetDataTable("EXEC [GET_BRANCHWISEWAREHOUSE] '1','" + strBranch + "'");
                }

                ddlWarehouse.Items.Clear();
                ddlWarehouse.DataSource = dt;
                ddlWarehouse.DataBind();

                dt = new DataTable();
                dt = oDBEngine.GetDataTable("EXEC [GET_BRANCHWISEWAREHOUSE] '1','" + strBranch + "'");


                DropDownList ucddlWarehouse = (DropDownList)MultiWarehouceuc.ucddlWarehouse;

                ucddlWarehouse.Items.Clear();
                ucddlWarehouse.DataSource = dt;
                ucddlWarehouse.DataBind();





            }
            catch { }
        }
        public DataTable GetWarehouseRate(string ProductID)
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_GetOpeningStockEntrys");
                proc.AddVarcharPara("@Action", 500, "GetWarehouseRate");
                proc.AddVarcharPara("@BranchID", 3000, Convert.ToString(cmbbranch.Value));
                proc.AddVarcharPara("@FinYear", 3000, Convert.ToString(Session["LastFinYear"]));
                proc.AddVarcharPara("@CompanyID", 3000, Convert.ToString(Session["LastCompany"]));
                proc.AddVarcharPara("@ProductID", 3000, Convert.ToString(ProductID));
                dt = proc.GetTable();

                return dt;
            }
            catch
            {
                return null;
            }
        }
        public string GetWarehouseRateList(string ProductID)
        {
            List<WarehouseRate> warehouseRateList = new List<WarehouseRate>();

            DataTable dt = GetWarehouseRate(ProductID);

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WarehouseRate WarehouseRates = new WarehouseRate();

                    string WarehouseID = Convert.ToString(dt.Rows[i]["WarehouseID"]);
                    string OpeningRate = Convert.ToString(dt.Rows[i]["OpeningRate"]);

                    WarehouseRates.WarehouseID = WarehouseID;
                    WarehouseRates.Rate = Convert.ToDecimal(OpeningRate);

                    warehouseRateList.Add(WarehouseRates);
                }
            }

            return JsonConvert.SerializeObject(warehouseRateList);
        }

        #endregion

        #region Set session For Packing Quantity
        [WebMethod]
        public static string SetSessionPacking(string list)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            List<ProductQuantity> finalResult = jsSerializer.Deserialize<List<ProductQuantity>>(list);
            HttpContext.Current.Session["SessionPackingDetails"] = finalResult;
            return null;

        }



        [WebMethod]
        public static string SaveSecondUOMDetails(string list)
        {
            SecondUOMDetailsBL uomBL = new SecondUOMDetailsBL();
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<SecondUOMDetails> finalResult = jsSerializer.Deserialize<List<SecondUOMDetails>>(list);

            DataTable dtoutput = uomBL.SaveSencondUOMDetails(finalResult, "Opening Entry", "IN", null);

            return null;

        }



        [WebMethod]
        public static object GetSecondUOMDetails(string ProductID, string branchid)
        {
            SecondUOMDetailsBL uomBL = new SecondUOMDetailsBL();
            List<SecondUOMDetails> finalResult = uomBL.GetSencondUOMDetails(ProductID, branchid, "Opening Entry", "IN", null, null);

            return finalResult;

        }




        #endregion

        //Rev Bapi
        protected void MultiUOM_DataBinding(object sender, EventArgs e)
        {
            //DataTable dt = (DataTable)Session["MultiUOMData"];
            //if(dt !=null && dt.Rows.Count >0 )
            //{
            //    DataView dvData = new DataView(dt);
            //    //dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";

            //    grid_MultiUOM.DataSource = dvData;
            //    grid_MultiUOM.DataBind();
            //}
            //else
            //{
            //    grid_MultiUOM.DataSource = null;
            //    grid_MultiUOM.DataBind();
            //}
        }
        protected void MultiUOM_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string SpltCmmd = e.Parameters.Split('~')[0];

            // Mantis Issue 24428/24429
            grid_MultiUOM.JSProperties["cpSetBaseQtyRateInGrid"] = "";
            // End of Mantis Issue 24428/24429
            grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "";

            if (SpltCmmd == "MultiUOMDisPlay")
            {
                Int64 ProductID = Convert.ToInt64(e.Parameters.Split('~')[1]);
                // Mantis Issue 24871
                //Session["MultiUOMData"] = GetMultiUOMData(ProductID);

                Int64 BranchID = Convert.ToInt64(e.Parameters.Split('~')[2]);
                Session["MultiUOMData"] = GetMultiUOMData(ProductID, BranchID);
                // End of Mantis Issue 24871
                grid_MultiUOM.JSProperties["cpOpenFocus"] = "";
                DataTable MultiUOMData = new DataTable();

                if (Session["MultiUOMData"] != null)
                {
                    MultiUOMData = (DataTable)Session["MultiUOMData"];
                }
                else
                {
                    MultiUOMData.Columns.Add("SrlNo", typeof(string));
                    MultiUOMData.Columns.Add("Quantity", typeof(Decimal));
                    MultiUOMData.Columns.Add("UOM", typeof(string));
                    MultiUOMData.Columns.Add("AltUOM", typeof(string));
                    MultiUOMData.Columns.Add("AltQuantity", typeof(Decimal));
                    MultiUOMData.Columns.Add("UomId", typeof(Int64));
                    MultiUOMData.Columns.Add("AltUomId", typeof(Int64));
                    MultiUOMData.Columns.Add("ProductId", typeof(Int64));

                }
                if (MultiUOMData != null && MultiUOMData.Rows.Count > 0)
                {
                    string SrlNo = e.Parameters.Split('~')[1];
                    DataView dvData = new DataView(MultiUOMData);
                    //dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }
                else
                {
                    grid_MultiUOM.DataSource = MultiUOMData.DefaultView;
                    grid_MultiUOM.DataBind();
                }
                grid_MultiUOM.JSProperties["cpOpenFocus"] = "OpenFocus";
            }

            else if (SpltCmmd == "SaveDisplay")
            {

                string Validcheck = "";
                DataTable MultiUOMSaveData = new DataTable();
                // Mantis Issue 24428
                int MultiUOMSR = 1;
                // End of Mantis Issue 24428

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);
                // Rev Sanchita
                string BaseRate = Convert.ToString(e.Parameters.Split('~')[9]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[11]);
                // End of Rev Sanchita


                DataTable allMultidataDetails = (DataTable)Session["MultiUOMData"];



                if (allMultidataDetails != null && allMultidataDetails.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = allMultidataDetails.Select("SrlNo ='" + SrlNo + "'");

                    foreach (DataRow item in MultiUoMresult)
                    {
                        if ((AltUomId == item["AltUomId"].ToString()) || (UomId == item["AltUomId"].ToString()))
                        {
                            if (AltQuantity == item["AltQuantity"].ToString())
                            {
                                Validcheck = "DuplicateUOM";
                                grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "DuplicateAltUOM";
                                break;
                            }
                        }
                        // Rev Sanchita [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                        if (UpdateRow == "True")
                        {
                            item["UpdateRow"] = "False";
                        }
                        // End of Rev Sanchita
                    }
                }

                if (Validcheck != "DuplicateUOM")
                {
                    if (Session["MultiUOMData"] != null)
                    {

                        MultiUOMSaveData = (DataTable)Session["MultiUOMData"];

                    }
                    else
                    {
                        MultiUOMSaveData.Columns.Add("SrlNo", typeof(string));
                        MultiUOMSaveData.Columns.Add("Quantity", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UOM", typeof(string));
                        MultiUOMSaveData.Columns.Add("AltUOM", typeof(string));
                        MultiUOMSaveData.Columns.Add("AltQuantity", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UomId", typeof(Int64));
                        MultiUOMSaveData.Columns.Add("AltUomId", typeof(Int64));
                        MultiUOMSaveData.Columns.Add("ProductId", typeof(Int64));
                        // Rev Sanchita
                        MultiUOMSaveData.Columns.Add("BaseRate", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("AltRate", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UpdateRow", typeof(string));

                        // Mantis Issue 24428
                        DataColumn myDataColumn = new DataColumn();
                        myDataColumn.AllowDBNull = false;
                        myDataColumn.AutoIncrement = true;
                        myDataColumn.AutoIncrementSeed = 1;
                        myDataColumn.AutoIncrementStep = 1;
                        myDataColumn.ColumnName = "MultiUOMSR";
                        myDataColumn.DataType = System.Type.GetType("System.Int32");
                        myDataColumn.Unique = true;
                        MultiUOMSaveData.Columns.Add(myDataColumn);

                        // End of Mantis Issue 24428

                        // End of Rev Sanchita
                    }

                    // Mantis Issue 24428
                      DataRow thisRow;
                      if (MultiUOMSaveData.Rows.Count > 0)
                      {
                          MultiUOMSR = Convert.ToInt32(MultiUOMSaveData.Compute("max([MultiUOMSR])", string.Empty)) + 1;
                          MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, BaseRate, AltRate, UpdateRow, MultiUOMSR);
                      }
                      else
                      {
                          MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, BaseRate, AltRate, UpdateRow, MultiUOMSR);
                      }
                    //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);
                    //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, BaseRate, AltRate, UpdateRow);
                    // End of Mantis Issue 24428
                    MultiUOMSaveData.AcceptChanges();
                    Session["MultiUOMData"] = MultiUOMSaveData;
                    hdmultiuomsessiondata.Value =Convert.ToString(MultiUOMSaveData.Rows.Count);
                    if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                    {
                        DataView dvData = new DataView(MultiUOMSaveData);
                        dvData.RowFilter = "SrlNo = '" + SrlNo + "'";

                        grid_MultiUOM.DataSource = dvData;
                        grid_MultiUOM.DataBind();
                    }
                    else
                    {
                        //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);
                        //Session["MultiUOMData"] = MultiUOMSaveData;
                        grid_MultiUOM.DataSource = MultiUOMSaveData.DefaultView;
                        grid_MultiUOM.DataBind();
                    }
                }
            }

            else if (SpltCmmd == "MultiUomDelete")
            {
                string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = AltUOMKeyValuewithqnty.Split('|')[0];
                string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[2]);
                DataTable dt = (DataTable)Session["MultiUOMData"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        if (AltUOMKeyValue.ToString() == item["AltUomId"].ToString())
                        {
                            //dt.Rows.Remove(item);
                            if (AltUOMKeyqnty.ToString() == item["AltQuantity"].ToString())
                            {
                                item.Table.Rows.Remove(item);
                                break;
                            }
                        }
                    }
                }
                Session["MultiUOMData"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(dt);
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }
                else
                {
                    grid_MultiUOM.DataSource = null;
                    grid_MultiUOM.DataBind();
                }
            }

            else if (SpltCmmd == "CheckMultiUOmDetailsQuantity")
            {
                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                DataTable dt = (DataTable)Session["MultiUOMData"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        item.Table.Rows.Remove(item);
                    }
                }
                Session["MultiUOMData"] = dt;
            }

            // Rev Sanchita

         // Mantis Issue 24428
            else if (SpltCmmd == "EditData")
            {
                string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[2];
                string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                DataTable dt = (DataTable)Session["MultiUOMData"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + AltUOMKeyValue + "'");

                    Decimal BaseQty = Convert.ToDecimal(MultiUoMresult[0]["Quantity"]);
                    Decimal BaseRate = Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]);

                    Decimal AltQty = Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]);
                    Decimal AltRate = Convert.ToDecimal(MultiUoMresult[0]["AltRate"]);
                    Decimal AltUom = Convert.ToDecimal(MultiUoMresult[0]["AltUomId"]);
                    bool UpdateRow = Convert.ToBoolean(MultiUoMresult[0]["UpdateRow"]);

                    grid_MultiUOM.JSProperties["cpAllDetails"] = "EditData";

                    grid_MultiUOM.JSProperties["cpBaseQty"] = BaseQty;
                    grid_MultiUOM.JSProperties["cpBaseRate"] = BaseRate;


                    grid_MultiUOM.JSProperties["cpAltQty"] = AltQty;
                    grid_MultiUOM.JSProperties["cpAltUom"] = AltUom;
                    grid_MultiUOM.JSProperties["cpAltRate"] = AltRate;
                    grid_MultiUOM.JSProperties["cpUpdatedrow"] = UpdateRow;
                    grid_MultiUOM.JSProperties["cpuomid"] = AltUOMKeyValue;
                }
                Session["MultiUOMData"] = dt;
            }



            else if (SpltCmmd == "UpdateRow")
            {

                string Validcheck = "";
                string SrlNoR = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[7];
                string AltUOMKeyqnty = e.Parameters.Split('~')[5];
                string muid = e.Parameters.Split('~')[12];

                string SrlNo = "0";

                DataTable MultiUOMSaveData = new DataTable();

                DataTable dt = (DataTable)Session["MultiUOMData"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {

                        SrlNo = Convert.ToString(item["SrlNo"]);

                    }
                }


                //Rev Sanchita
               // string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                //End Rev Sanchita
                string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);

                // Mantis Issue 24428
                string BaseRate = Convert.ToString(e.Parameters.Split('~')[9]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[11]);
                // End of Mantis Issue 24428

                DataRow[] MultiUoMresultResult = dt.Select("SrlNo ='" + SrlNo + "' and  MultiUOMSR <>'" + muid + "'");

                foreach (DataRow item in MultiUoMresultResult)
                {
                    if ((AltUomId == item["AltUomId"].ToString()) || (UomId == item["AltUomId"].ToString()))
                    {
                        if (AltQuantity == item["AltQuantity"].ToString())
                        {
                            Validcheck = "DuplicateUOM";
                            grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "DuplicateAltUOM";
                            break;
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
                            SrlNo = Convert.ToString(item["SrlNo"]);
                            item.Table.Rows.Remove(item);
                            break;

                        }
                    }
                    dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, BaseRate, AltRate, UpdateRow, muid);
                }

                Session["MultiUOMData"] = dt;
                MultiUOMSaveData = (DataTable)Session["MultiUOMData"];

                MultiUOMSaveData.AcceptChanges();
                Session["MultiUOMData"] = MultiUOMSaveData;

                if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                {
                    DataView dvData = new DataView(MultiUOMSaveData);
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";

                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }

            }








            // End of Mantis Issue 24428







            else if (SpltCmmd == "SetBaseQtyRateInGrid")
            {
                DataTable dt = new DataTable();

                if (Session["MultiUOMData"] != null)
                {
                    dt = (DataTable)HttpContext.Current.Session["MultiUOMData"];
                    DataRow[] MultiUoMresult = dt.Select("UpdateRow ='True'");

                    Int64 SelNo = Convert.ToInt64(MultiUoMresult[0]["SrlNo"]);
                    Decimal BaseQty = Convert.ToDecimal(MultiUoMresult[0]["Quantity"]);
                    Decimal BaseRate = Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]);

                    // Mantis Issue 24428

                    Decimal AltQty = Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]);
                    string AltUom = Convert.ToString(MultiUoMresult[0]["AltUOM"]);
                    // End of Mantis Issue 24428
                    grid_MultiUOM.JSProperties["cpSetBaseQtyRateInGrid"] = "1";
                    grid_MultiUOM.JSProperties["cpBaseQty"] = BaseQty;
                    grid_MultiUOM.JSProperties["cpBaseRate"] = BaseRate;


                    // Mantis Issue 24428

                    grid_MultiUOM.JSProperties["cpAltQty"] = AltQty;

                    int AltUOMID = GetUom(AltUom);

                    grid_MultiUOM.JSProperties["cpAltUom"] = AltUOMID;
                    // End of Mantis Issue 24428
                 string valcount =Convert.ToString(MultiUoMresult.ToList().Count());
                 grid_MultiUOM.JSProperties["cpCountMultiUOM"] = valcount;

                    //if (Session["OrderDetails"] != null)
                    //{
                    //    DataTable SalesOrderdt = (DataTable)Session["OrderDetails"];

                    //    DataRow[] drSalesOrder = SalesOrderdt.Select("SrlNo ='" + SelNo + "'");
                    //    if (drSalesOrder.Length > 0)
                    //    {
                    //        drSalesOrder[0]["Quantity"] = BaseQty;
                    //        drSalesOrder[0]["SalePrice"] = BaseRate;
                    //    }

                    //}

                }
            }
            // End of Rev Sanchita
        }


        [WebMethod]
        public static object GetPackingQuantity(Int32 UomId, Int32 AltUomId, Int64 ProductID)
        {
            List<MultiUOMPacking> RateLists = new List<MultiUOMPacking>();

            //decimal packing_quantity = 0;
            //decimal sProduct_quantity = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "PackingQuantityDetails");
            proc.AddIntegerPara("@UomId", UomId);
            proc.AddIntegerPara("@AltUomId", AltUomId);
            proc.AddBigIntegerPara("@PackingProductId", ProductID);
            DataTable dt = proc.GetTable();
            RateLists = DbHelpers.ToModelList<MultiUOMPacking>(dt);
            // Mantis Issue 24428
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    packing_quantity = Convert.ToDecimal(dt.Rows[0]["packing_quantity"]);
            //    sProduct_quantity = Convert.ToDecimal(dt.Rows[0]["sProduct_quantity"]);
            //}
            //End Mantis Issue 24428
            //return packing_quantity + '~' + sProduct_quantity;
            return RateLists;
        }

        [WebMethod]
        public static object AutoPopulateAltQuantity(Int64 ProductID)
        {
            List<MultiUOMPacking> RateLists = new List<MultiUOMPacking>();

            //decimal packing_quantity = 0;
            //decimal sProduct_quantity = 0;
            //Int32 AltUOMId = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "AutoPopulateAltQuantityDetails");
            proc.AddBigIntegerPara("@PackingProductId", ProductID);
            DataTable dt = proc.GetTable();
            RateLists = DbHelpers.ToModelList<MultiUOMPacking>(dt);
            // Mantis Issue 24428
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    packing_quantity = Convert.ToDecimal(dt.Rows[0]["packing_quantity"]);
            //    sProduct_quantity = Convert.ToDecimal(dt.Rows[0]["sProduct_quantity"]);
            //    AltUOMId = Convert.ToInt32(dt.Rows[0]["AltUOMId"]);
            //}
            //End Mantis Issue 24428
            //return packing_quantity + '~' + sProduct_quantity;
            return RateLists;
        }
        //-------------------------------Rev Rajdip--------------------------------------------
        // Mantis Issue 24428
       [WebMethod]
        public int GetUom(string UOMNAME)
        {
           
           
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                int dtUOM = oDBEngine.ExeInteger("select UOM_ID  from Master_UOM where UOM_Name='" + UOMNAME + "'");
                return dtUOM;

           
        }
       //End Mantis Issue 24428

        [WebMethod]
        public static Int32 GetQuantityfromSL(string SLNo)
        {

            DataTable dt = new DataTable();
            int SLVal = 0;
            if (HttpContext.Current.Session["MultiUOMData"] != null)
            {
                dt = (DataTable)HttpContext.Current.Session["MultiUOMData"];
                // Rev Sanchita
                //DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "'");
                DataRow[] MultiUoMresult = dt.Select("UpdateRow ='True'");
                // End of Rev Sanchita

                SLVal = MultiUoMresult.Length;


            }

            return SLVal;
        }

        // Mantis Issue 24871
        //public DataTable GetMultiUOMData(Int64 ProductID)
        public DataTable GetMultiUOMData(Int64 ProductID, Int64 BranchId)
            // End of Mantis Issue 24871
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetOpeningStockEntrys");
            proc.AddVarcharPara("@Action", 500, "MultiUOMQuotationDetails");
            proc.AddPara("@ProductID", ProductID);
            // Mantis Issue 24871
            proc.AddPara("@BranchID", BranchId);
            // End of Mantis Issue 24871
            ds = proc.GetTable();
            return ds;
        }
        //End Rev Bapi

    }

    public class WarehouseRate
    {
        public string WarehouseID { get; set; }
        public decimal Rate { get; set; }
    }
    public class StockBlock
    {
        public string IsStockBlock { get; set; }
        public decimal AvailableQty { get; set; }
    }
    public class MultiUOMPackingPro
    {
        public decimal PACKING_QUANTITY { get; set; }
        public decimal SPRODUCT_QUANTITY { get; set; }

        public Int32 ALTUOMID { get; set; }
    }
    public class MultiUOMPacking
    {
        public decimal packing_quantity { get; set; }
        public decimal sProduct_quantity { get; set; }

        public Int32 AltUOMId { get; set; }
    }




}