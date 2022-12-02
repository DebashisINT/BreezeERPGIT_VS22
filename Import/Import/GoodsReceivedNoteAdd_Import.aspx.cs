using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
//using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections;
using DevExpress.Web.Data;
using DataAccessLayer;
using System.ComponentModel;
using System.Linq;
using EntityLayer;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using UtilityLayer;
using EntityLayer.MailingSystem;
using BusinessLogicLayer.EmailDetails;
using System.Drawing;
using ImportModuleBusinessLayer;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ImportModuleBusinessLayer.Purchaseorder;
using DevExpress.Data;
using ImportModuleBusinessLayer.PurchaseorderAcceptance;
using ImportModuleBusinessLayer.Purchaseinvoice;
using ImportModuleBusinessLayer.GoodReceivedNote;
using System.Reflection;


namespace Import.Import
{
    public partial class GoodsReceivedNoteAdd_Import : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        GoodReceivedNoteBL blLayer = new GoodReceivedNoteBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        string UniquePurchaseNumber = string.Empty;


        ImportSalesActivitiesBL objSlaesActivitiesBL = new ImportSalesActivitiesBL();
        ImportPurchaseInvoice objPurchaseOrderBL = new ImportPurchaseInvoice();


        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        GSTtaxDetails gstTaxDetails = new GSTtaxDetails();
        public string pageAccess = "";
        static string ForJournalDate = null;

        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {

            if (e.Column.FieldName == "gvColUOM")
            {
                e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "gvColTotalAmountINR")
            {
                e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "gvColStockPurchasePriceNetamountbase")
            {
                e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "gvColQuantity")
            {
                e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "ProductName")
            {
                e.Editor.ReadOnly = true;
            }

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlIndentRequisitionNo.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Sqlvendor.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCurrency.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //DS_Branch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DS_AmountAre.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectPin.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Import/GoodsReceivedNoteList_Import.aspx");
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (ddlInventory.SelectedValue == "Y")
            {

                taggingList.ClientEnabled = true;
            }
            else
            {

                taggingList.ClientEnabled = false;
            }
            if (Request.QueryString["key"] != null && Request.QueryString["key"] != "ADD")
            {
                string strOrderId1 = Convert.ToString(Request.QueryString["key"]);
            }
            if (!IsPostBack)
            {
                //Changes 20-06-2018  Sudip Pal

                grid.Columns[16].Caption = "Net Amt [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";
                grid.Columns[10].Caption = "Price [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";
                grid.Columns[12].Caption = "Amt [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";
                grid.Columns[13].Caption = "Amt [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";
                ASPxLabel3.Text = "Document Amount [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";
                ASPxLabel1.Text = "Total Amount [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";
                lbl_totalamtfrgn.Text = "Total Amount [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";
                lblbasecurrency.InnerText = Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim();
                //Changes 20-06-2018  Sudip Pal

                dt_EntryDate.Date = DateTime.Now;

                #region NewTaxblock
                string ItemLevelTaxDetails = string.Empty; string HSNCodewisetaxSchemid = string.Empty; string BranchWiseStateTax = string.Empty; string StateCodeWiseStateIDTax = string.Empty;
                gstTaxDetails.GetTaxData(ref ItemLevelTaxDetails, ref HSNCodewisetaxSchemid, ref BranchWiseStateTax, ref StateCodeWiseStateIDTax, "P");
                HDItemLevelTaxDetails.Value = ItemLevelTaxDetails;
                HDHSNCodewisetaxSchemid.Value = HSNCodewisetaxSchemid;
                HDBranchWiseStateTax.Value = BranchWiseStateTax;
                HDStateCodeWiseStateIDTax.Value = StateCodeWiseStateIDTax;

                #endregion

                // Cross Branch Section by Sam on 10052017 Start
                string branchid = "";
                string branch = "";
                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    DataTable dt = objPurchaseOrderBL.GetBranchIdByPOID(Convert.ToString(Request.QueryString["key"]));
                    branchid = Convert.ToString(dt.Rows[0]["br"]);
                    branch = oDBEngine.getBranch(branchid, "") + branchid;

                    HttpContext.Current.Session["userbranchHierarchy"] = branch;
                    Session["LastCompany"] = Convert.ToString(dt.Rows[0]["comp"]);
                    Session["LastFinYear"] = Convert.ToString(dt.Rows[0]["finyear"]);

                }
                else
                {
                    branchid = Convert.ToString(Session["userbranchID"]);
                    branch = oDBEngine.getBranch(branchid, "") + branchid;
                }
                // Cross Branch Section by Sam on 10052017 End
                #region Sandip Section For Checking User Level for Allow Edit to logging User or Not

                //GetEditablePermission();

                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    if (Convert.ToString(Request.QueryString["status"]) == "PO")
                    {

                        divcross.Visible = false;
                        btn_SaveRecords.ClientVisible = false;
                        ApprovalCross.Visible = false;
                        ddl_Branch.Enabled = false;

                    }
                    else
                    {

                        divcross.Visible = false;
                        btn_SaveRecords.ClientVisible = false;
                        ApprovalCross.Visible = true;
                        ddl_Branch.Enabled = false;

                    }

                }

                else
                {
                    divcross.Visible = true;
                    btn_SaveRecords.ClientVisible = true;
                    ApprovalCross.Visible = false;
                    ddl_Branch.Enabled = true;
                }

                #endregion Sandip Section
                if (Request.QueryString["op"] == "yes")
                {
                    hdnOpening.Value = "Opening";
                    taggingList.ClientEnabled = false;
                }
                this.Session["LastCompany1"] = Session["LastCompany"];
                this.Session["LastFinYear1"] = Session["LastFinYear"];
                this.Session["userbranch"] = Session["userbranchID"];




                string finyear = Convert.ToString(Session["LastFinYear"]);
                Session["CustomerDetail"] = null;
                Session["ImptStockReceiptID"] = null;
                Session["ImptStockReceiptDetails"] = null;
                Session["LoopWarehouse"] = 1;
                Session["POTaxDetails"] = null;
                Session["PurchaseOrderAddressDtl"] = null;

                PopulateGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                PopulateChargeGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                if (Request.QueryString["key"] == "ADD")
                {
                    ddlInventory.SelectedValue = Convert.ToString(Request.QueryString["InvType"]);
                    ddlInventory.Enabled = false;

                    if (!String.IsNullOrEmpty(Convert.ToString(Session["userbranchID"])))
                    {
                        string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                        ddl_Branch.SelectedValue = strdefaultBranch;
                    }

                    #region To Show By Default Cursor after SAVE AND NEW

                    if (Session["SaveModePO"] != null)  // it has been removed from coding side of Quotation list 
                    {
                        if (Session["schemavaluePO"] != null)  // it has been removed from coding side of Quotation list 
                        {
                            ddl_numberingScheme.SelectedValue = Convert.ToString(Session["schemavaluePO"]); // it has been removed from coding side of Quotation list 

                            ///Changes 26-06-2018 Sudip Pal
                            //string branchId = Convert.ToString(ddl_numberingScheme.SelectedValue);

                            string branchId = Convert.ToString(HDNumberingschema.Value);

                            //string strbranchID = branchId.Split('~')[3];
                            //ddl_Branch.SelectedValue = strbranchID;
                            ///Changes 26-06-2018 Sudip Pal
                            ///


                            ddl_Branch.Enabled = false;
                            ddlInventory.SelectedValue = Convert.ToString(Session["POType"]);

                        }
                        if (Convert.ToString(Session["SaveModePO"]) == "A")
                        {
                            dt_PLQuote.Focus();
                            txtVoucherNo.Enabled = false;
                            txtVoucherNo.Text = "Auto";
                        }
                        else if (Convert.ToString(Session["SaveModePO"]) == "M")
                        {
                            txtVoucherNo.Enabled = true;
                            txtVoucherNo.Text = "";
                            txtVoucherNo.Focus();
                        }
                    }
                    else
                    {
                        ddlInventory.Focus();
                    }
                    #endregion To Show By Default Cursor after SAVE AND NEW

                    IndentTaggingMendatory();
                    ViewState["ActionType"] = "Add";
                    Keyval_internalId.Value = "Add";
                    hdnPageStatus1.Value = "update";

                    if (!String.IsNullOrEmpty(Convert.ToString(Session["LocalCurrency"])))
                    {
                        string CompID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                        string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);
                        string basedCurrency = Convert.ToString(LocalCurrency.Split('~')[0]);
                        string CurrencyId = Convert.ToString(basedCurrency[0]);
                        ddl_Currency.SelectedValue = CurrencyId;
                        int ConvertedCurrencyId = Convert.ToInt32(CurrencyId);


                        //Changes 20-06-2018 Sudip pal
                        // DataTable dt = objPurchaseOrderBL.GetCurrentConvertedRate(Convert.ToInt16(CurrencyId), ConvertedCurrencyId, CompID);
                        DataTable dt = objPurchaseOrderBL.GetCurrentConvertedRate(Convert.ToInt16(CurrencyId), ConvertedCurrencyId, CompID, dt_PLQuote.Date.ToString("yyyy-Mm-dd"));
                        //Changes 20-06-2018 Sudip pal

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            txt_Rate.Text = Convert.ToString(dt.Rows[0]["PurchaseRate"]);
                        }
                    }
                    AddmodeExecuted();
                    hdnPageStatus.Value = "first";
                    ddl_AmountAre.Value = "3";
                    ddl_VatGstCst.ClientEnabled = false;
                    //.................Tax..............
                    Session["POTaxDetails"] = null;
                    CreateDataTaxTable();
                    //..........End Tax-----------


                    DataTable Transactiondt = CreateTempTable("Transaction");
                    Transactiondt.Rows.Add("1", "1", "", "", "", "", "", "0.0000", "", "0.00", "0.00", "0.00", "0.00", "0.00", "0", "0", "", "", "I", "0", "", "", "", "");

                    Session["ImptStockReceiptDetails"] = Transactiondt;
                    grid.DataSource = GetStockReceiptBatch(Transactiondt);
                    grid.DataBind();
                    ddl_numberingScheme.Focus(); ;

                }
                else
                {
                    txtVoucherNo.Enabled = false;
                    ddlInventory.Enabled = false;
                    ddl_Branch.Enabled = false;
                    txtVendorName.ClientEnabled = false;
                    dt_PLQuote.ClientEnabled = false;
                    Keyval_internalId.Value = "StockReceipt" + Request.QueryString["key"];
                    hdnPageStatus.Value = "Quoteupdate";
                    lblHeading.Text = "Modify Import Stock Receipt";
                    divNumberingScheme.Style.Add("display", "none");
                    lbl_NumberingScheme.Visible = false;
                    ddl_numberingScheme.Visible = false;
                    Session["ImptStockReceiptID"] = Request.QueryString["key"];
                    ViewState["ActionType"] = "Edit";
                    btn_SaveRecords.ClientVisible = false;

                    DataSet Ds = GetPurchaseOrderData();
                    Session["ImptStockReceiptDetails"] = Ds.Tables[0];

                    Purchase_BillingShipping.SetBillingShippingTable(Ds.Tables[1]);


                    FillGrid();
                    //.................Tax..............
                    Session["POTaxDetails"] = GetTaxDataWithGST(GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd")));
                    DataTable quotetable = GetQuotationEditedTaxData().Tables[0];
                    if (quotetable == null)
                    {
                        CreateDataTaxTable();
                    }
                    else
                    {
                        Session["ImportStockreceiptFinalTaxRecord"] = quotetable;
                    }
                    //..........End Tax-----------
                    DataSet ds_GRNDetails = blLayer.GetStockReceiptEditData(Convert.ToString(Session["ImptStockReceiptID"]));
                    DataTable Warehouse_dt = ds_GRNDetails.Tables[2];

                    var jsonSerialiser = new JavaScriptSerializer();
                    DataTable _Warehouse_dt = GetChallanWarehouseData(Warehouse_dt);
                    var json = jsonSerialiser.Serialize(GetStock(_Warehouse_dt));
                    hdnJsonTempStock.Value = json;

                    grid.DataSource = GetPurchaseOrderBatch();
                    grid.DataBind();

                    BindWarehouse();
                    #region Samrat Roy -- Hide Save Button in Edit Mode
                    if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                    {
                        lblHeading.Text = "View Import Stock Receipt";
                        lbl_quotestatusmsg.Text = "*** View Mode Only";
                        btn_SaveRecords.ClientVisible = false;
                        btnSaveExit.ClientVisible = false;
                        lbl_quotestatusmsg.Visible = true;
                    }
                    #endregion

                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);
                }

                if (IsBarcodeGeneratete() == true) hdfIsBarcodeGenerator.Value = "Y";
                else hdfIsBarcodeGenerator.Value = "N";

                // VisiblitySendEmail();



            }

        }
        public DataTable GetChallanWarehouseData(DataTable dt)
        {
            try
            {
                DataTable tempdt = new DataTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    var CheckStockOut_dt = dt.Select("IsOutStatusMsg='visibility: visible'");
                    if (CheckStockOut_dt.Length > 0)
                    {
                        dt_PLQuote.ClientEnabled = false;
                    }

                    string strNewVal = "", strOldVal = "", strProductType = "";
                    tempdt = dt.Copy();
                    foreach (DataRow drr in tempdt.Rows)
                    {
                        strNewVal = Convert.ToString(drr["QuoteWarehouse_Id"]);
                        strProductType = Convert.ToString(drr["ProductType"]);

                        if (strNewVal == strOldVal)
                        {
                            drr["WarehouseName"] = "";
                            drr["TotalQuantity"] = "0";
                            //drr["BatchNo"] = "";
                            drr["ViewBatch"] = "";
                            drr["SalesQuantity"] = "";
                            drr["ViewMfgDate"] = "";
                            drr["ViewExpiryDate"] = "";
                        }

                        strOldVal = strNewVal;
                    }

                    tempdt.Columns.Remove("QuoteWarehouse_Id");
                    tempdt.Columns.Remove("ProductType");

                    string maxLoopID = GetWarehouseMaxLoopValue(tempdt);
                    Session["PC_LoopWarehouse"] = maxLoopID;
                }
                else
                {
                    tempdt.Columns.Add("Product_SrlNo", typeof(string));
                    tempdt.Columns.Add("SrlNo", typeof(int));
                    tempdt.Columns.Add("WarehouseID", typeof(string));
                    tempdt.Columns.Add("WarehouseName", typeof(string));
                    tempdt.Columns.Add("Quantity", typeof(string));
                    tempdt.Columns.Add("TotalQuantity", typeof(string));
                    tempdt.Columns.Add("SalesQuantity", typeof(string));
                    tempdt.Columns.Add("SalesUOMName", typeof(string));
                    tempdt.Columns.Add("BatchID", typeof(string));
                    tempdt.Columns.Add("BatchNo", typeof(string));
                    tempdt.Columns.Add("MfgDate", typeof(string));
                    tempdt.Columns.Add("ExpiryDate", typeof(string));
                    tempdt.Columns.Add("SerialNo", typeof(string));
                    tempdt.Columns.Add("LoopID", typeof(string));
                    tempdt.Columns.Add("Status", typeof(string));
                    tempdt.Columns.Add("ViewMfgDate", typeof(string));
                    tempdt.Columns.Add("ViewExpiryDate", typeof(string));
                    tempdt.Columns.Add("IsOutStatus", typeof(string));
                    tempdt.Columns.Add("IsOutStatusMsg", typeof(string));

                    Session["PC_LoopWarehouse"] = "1";
                }

                return tempdt;
            }
            catch
            {
                return null;
            }
        }
        public DataTable CreateTempTable(string Type)
        {

            DataTable StockReceiptDT = new DataTable();

            if (Type == "Transaction")
            {
                StockReceiptDT.Columns.Add("SrlNo", typeof(string));//1
                StockReceiptDT.Columns.Add("OrderDetails_Id", typeof(string));//2
                StockReceiptDT.Columns.Add("ProductID", typeof(string));
                StockReceiptDT.Columns.Add("Description", typeof(string));
                StockReceiptDT.Columns.Add("Quantity", typeof(string));
                StockReceiptDT.Columns.Add("UOM", typeof(string));
                StockReceiptDT.Columns.Add("Warehouse", typeof(string));
                StockReceiptDT.Columns.Add("StockQuantity", typeof(string));
                StockReceiptDT.Columns.Add("StockUOM", typeof(string));
                StockReceiptDT.Columns.Add("PurchasePrice", typeof(string));
                StockReceiptDT.Columns.Add("PurchasePriceforgn", typeof(string));//11        

                StockReceiptDT.Columns.Add("Discount", typeof(string));
                StockReceiptDT.Columns.Add("Amount", typeof(string));
                StockReceiptDT.Columns.Add("Amountbase", typeof(string));

                StockReceiptDT.Columns.Add("DutyAmtINR", typeof(string));
                StockReceiptDT.Columns.Add("TaxAmount", typeof(string));
                StockReceiptDT.Columns.Add("TotalAmount", typeof(string));
                StockReceiptDT.Columns.Add("TotalAmountBase", typeof(string));   //18       

                StockReceiptDT.Columns.Add("Status", typeof(string));//19
                StockReceiptDT.Columns.Add("Indent_No", typeof(string));
                StockReceiptDT.Columns.Add("ProductName", typeof(string));
                StockReceiptDT.Columns.Add("IsComponentProduct", typeof(string));
                StockReceiptDT.Columns.Add("IsLinkedProduct", typeof(string));
                StockReceiptDT.Columns.Add("TaxAmountINR", typeof(string));//24
            }
            return StockReceiptDT;
        }
        public void IndentTaggingMendatory()
        {
            DataTable TaggingMen = objPurchaseOrderBL.GetIndentTaggingMendatory();
            if (TaggingMen.Rows.Count > 0)
            {
                string strVariableName = Convert.ToString(TaggingMen.Rows[0]["Variable_Value"]).Trim();
                if (strVariableName == "Yes")
                {
                    hdfTagMendatory.Value = "Yes";
                }
                else
                {
                    hdfTagMendatory.Value = "No";
                }
            }
        }

        private bool IsPITransactionExist(string Poid)
        {
            bool IsExist = false;
            if (Poid != "" && Convert.ToString(Poid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = objPurchaseOrderBL.CheckPOTraanaction(Poid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
        }
        public void Bind_NumberingScheme()
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "97", "N");
            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }
        }
        public void SetFinYearCurrentDate()
        {
            dt_PLQuote.EditFormatString = objConverter.GetDateFormat("Date");
            string fDate = null;

            //DateTime dt = DateTime.ParseExact("3/31/2016", "MM/dd/yyy", CultureInfo.InvariantCulture);
            string[] FinYEnd = Convert.ToString(Session["FinYearEnd"]).Split(' ');
            string FinYearEnd = FinYEnd[0];

            DateTime date3 = DateTime.ParseExact(FinYearEnd, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            ForJournalDate = Convert.ToString(date3);

            //ForJournalDate =Session["FinYearEnd"].ToString();
            int month = oDBEngine.GetDate().Month;
            int date = oDBEngine.GetDate().Day;
            int Year = oDBEngine.GetDate().Year;

            if (date3 < oDBEngine.GetDate().Date)
            {
                fDate = Convert.ToString(Convert.ToDateTime(ForJournalDate).Month) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Day) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Year);
            }
            else
            {
                fDate = Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Month) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Day) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Year);
            }

            dt_PLQuote.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }
        public void PopulateCustomerDetail()
        {
            //if (Session["POVendorsDetail"] == null)
            //{
            //    DataTable dtCustomer = new DataTable();
            //    dtCustomer = objPurchaseOrderBL.PopulateVendorsDetail();

            //    if (dtCustomer != null && dtCustomer.Rows.Count > 0)
            //    {
            //        lookup_Customer.DataSource = dtCustomer;
            //        lookup_Customer.DataBind();
            //        Session["POVendorsDetail"] = dtCustomer;
            //    }
            //}
            //else
            //{
            //    lookup_Customer.DataSource = (DataTable)Session["POVendorsDetail"];
            //    lookup_Customer.DataBind();
            //}

            DataTable dtCustomer = new DataTable();
            dtCustomer = objPurchaseOrderBL.PopulateVendorsDetail();

            if (dtCustomer != null && dtCustomer.Rows.Count > 0)
            {
                //lookup_Customer.DataSource = dtCustomer;
                //lookup_Customer.DataBind();
                Session["POVendorsDetail"] = dtCustomer;
            }

        }

        #region  Class
        public class PurchaseOrderList
        {
            public string SrlNo { get; set; }
            public string OrderDetails_Id { get; set; }
            public string gvColProduct { get; set; }
            public string gvColDiscription { get; set; }
            public string gvColQuantity { get; set; }
            public string gvColUOM { get; set; }
            public string Warehouse { get; set; }
            public string gvColStockQty { get; set; }
            public string gvColStockUOM { get; set; }
            public string gvColStockPurchasePrice { get; set; }
            public string gvColDiscount { get; set; }
            public string gvColAmount { get; set; }

            /// Changes 26-06-2018 Sudip Pal
            public string gvColAmountbase { get; set; }

            /// Changes 26-06-2018 Sudip Pal

            public string gvColDutyAmtINR { get; set; }
            public string gvColTaxAmount { get; set; }
            public string gvColTotalAmountINR { get; set; }
            public string Quotation_No { get; set; }
            public string ProductName { get; set; }
            public string Indent_Num { get; set; }
            public Int64 Indent { get; set; }
            public string IsComponentProduct { get; set; }
            public string IsLinkedProduct { get; set; }


            /// Changes 13-06-2018 Sudip Pal
            public string gvColStockPurchasePriceforeign { get; set; }


            /// Changes 13-06-2018 Sudip Pal


            /// Changes 14-06-2018 Sudip Pal
            public string gvColStockPurchasePriceNetamountbase { get; set; }


            /// Changes 14-06-2018 Sudip Pal

        }


        public class SalesOrder
        {
            public string SrlNo { get; set; }
            public string OrderDetails_Id { get; set; }
            public string gvColProduct { get; set; }
            public string gvColDiscription { get; set; }
            public string gvColQuantity { get; set; }
            public string gvColUOM { get; set; }
            public string Warehouse { get; set; }
            public string gvColStockQty { get; set; }
            public string gvColStockUOM { get; set; }
            public string gvColStockPurchasePrice { get; set; }
            public string gvColDiscount { get; set; }
            public string gvColAmount { get; set; }
            public string gvColTaxAmount { get; set; }
            public string gvColTotalAmountINR { get; set; }
            public Int64 Quotation_No { get; set; }
            public string Quotation_Num { get; set; }
            public string Key_UniqueId { get; set; }
            public string Product_Shortname { get; set; }
            public string ProductName { get; set; }
            public string QuoteDetails_Id { get; set; }
            public string Indent_Num { get; set; }
            public Int64 Indent { get; set; }

            /// Changes 13-06-2018 Sudip Pal
            public string gvColStockPurchasePriceforeign { get; set; }

            /// Changes 13-06-2018 Sudip Pal
            /// 

            /// Changes 14-06-2018 Sudip Pal
            public string gvColStockPurchasePriceNetamountbase { get; set; }

            /// Changes 14-06-2018 Sudip Pal

        }
        public class Product
        {
            public string ProductID { get; set; }
            public string ProductName { get; set; }
        }
        public class TaggedProduct
        {
            public string SrlNo { get; set; }
            public string ComponentID { get; set; }
            public string ComponentDetailsID { get; set; }
            public string ComponentNumber { get; set; }
            public string ProductID { get; set; }
            public string ProductsName { get; set; }
            public string ProductDescription { get; set; }
            public string Quantity { get; set; }

        }
        public class TaxSetailsJson
        {
            public string SchemeName { get; set; }
            public int vissibleIndex { get; set; }
            public string applicableOn { get; set; }
            public string applicableBy { get; set; }
        }
        public class TaxDetails
        {
            public int Taxes_ID { get; set; }
            public string Taxes_Name { get; set; }

            public double Amount { get; set; }
            public string TaxField { get; set; }

            public string taxCodeName { get; set; }

            public decimal calCulatedOn { get; set; }


            /// Changes  18-06-2018                                 

            public decimal calCulatedOnInr { get; set; }
            public double AmountInr { get; set; }
            /// Changes  18-06-2018  
        }
        class taxCode
        {
            public string Taxes_ID { get; set; }
            public string Taxes_Name { get; set; }
        }
        public class Taxes
        {
            public string TaxID { get; set; }
            public string TaxName { get; set; }
            public string Percentage { get; set; }
            public string Amount { get; set; }
            public decimal calCulatedOn { get; set; }
        }
        #endregion

        public void FillGrid()
        {
            DataSet PurchaseOrderEditdtset = blLayer.GetStockReceiptEditData(Convert.ToString(Session["ImptStockReceiptID"]));

            DataTable dst = PurchaseOrderEditdtset.Tables[3];
            if (dst != null && dst.Rows.Count > 0)
            {
                ddl_AmountAre.DataSource = dst;
                ddl_AmountAre.TextField = "taxGrp_Description";
                ddl_AmountAre.ValueField = "taxGrp_Id";
                ddl_AmountAre.DataBind();

                ListEditItem li = new ListEditItem();
                li.Text = "Import";
                li.Value = "4";
                ddl_AmountAre.Items.Insert(3, li);
            }
            DataTable dtBranch = PurchaseOrderEditdtset.Tables[4];
            if (dtBranch != null && dtBranch.Rows.Count > 0)
            {
                ddl_Branch.DataSource = dtBranch;
                ddl_Branch.DataValueField = "branch_id";
                ddl_Branch.DataTextField = "branch_description";
                ddl_Branch.DataBind();
            }

            DataTable PurchaseOrderEditdt = PurchaseOrderEditdtset.Tables[0];
            if (PurchaseOrderEditdt != null && PurchaseOrderEditdt.Rows.Count > 0)
            {
                string PurchaseOrder_Number = Convert.ToString(PurchaseOrderEditdt.Rows[0]["GoodReceivedNote_Number"]);//0
                string PurchaseOrder_IndentId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["ImportBillOfEntry_Ids"]);//1
                string BranchId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["GoodReceivedNote_BranchId"]);//2
                TermsConditionsControl.SetBranchId(BranchId);  // Mantis Issue #16920
                string PurchaseOrder_Date = Convert.ToString(PurchaseOrderEditdt.Rows[0]["GoodReceivedNote_Date"]);//3

                string VendorId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["GoodReceivedNote_VendorId"]);//5
                string Contact_Person_Id = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Contact_Person_Id"]);//6
                string PurchaseOrder_Reference = Convert.ToString(PurchaseOrderEditdt.Rows[0]["GoodReceivedNote_Reference"]);//7
                string PurchaseOrder_Currency_Id = Convert.ToString(PurchaseOrderEditdt.Rows[0]["GoodReceivedNote_Currency_Id"]);//8
                string Currency_Conversion_Rate = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Currency_Conversion_Rate"]);//9
                string Tax_Option = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Tax_Option"]);//10
                string Tax_Code = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Tax_Code"]);//11
                //string PurchaseOrder_SalesmanId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrderBillentry_SalesmanId"]);//12
                string PurchaseOrder_IndentDate = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Billentry_Date"]);//13
                string IsInventory = Convert.ToString(PurchaseOrderEditdt.Rows[0]["IsInventory"]);//13
                string PurchaseOrderDueDate = Convert.ToString(PurchaseOrderEditdt.Rows[0]["DueDate"]);//13
                string CreditDays = Convert.ToString(PurchaseOrderEditdt.Rows[0]["CreditDays"]);//13
                string VendorName = Convert.ToString(PurchaseOrderEditdt.Rows[0]["VendorName"]);//13
                string MainAccount_Name = Convert.ToString(PurchaseOrderEditdt.Rows[0]["MainAccount_Name"]);//13
                string MainAccount_ReferenceID = Convert.ToString(PurchaseOrderEditdt.Rows[0]["GoodsInTransit"]);//13
                string CuatomVendorName = Convert.ToString(PurchaseOrderEditdt.Rows[0]["CuatomVendorName"]);
                string CustomHouseId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["CustomHouseId"]);


                string AcountLedgerName = Convert.ToString(PurchaseOrderEditdt.Rows[0]["AcountLedger"]);
                string AcountLedgerID = Convert.ToString(PurchaseOrderEditdt.Rows[0]["LedgerID"]);


                txtCustomHouse.Text = CuatomVendorName.Trim();
                hdnCustomHouseID.Value = CustomHouseId;


                string ordernum = Convert.ToString(PurchaseOrderEditdt.Rows[0]["OrderNumber"]);//13
                string invoiceNumber = Convert.ToString(PurchaseOrderEditdt.Rows[0]["importorder"]);//13


                txtInvoicenumber.Text = invoiceNumber;
                txtInvoicenumber.Enabled = false;
                txtOrderNumber.Text = ordernum;
                txtOrderNumber.Enabled = false;

                btnGoodsinTransit.Text = MainAccount_Name.Trim();
                hdnMainAccountID.Value = MainAccount_ReferenceID;

                txtLedger.Text = AcountLedgerName.Trim();
                hdnLedgerAccountID.Value = AcountLedgerID;

                string totalamtbase = Convert.ToString(PurchaseOrderEditdt.Rows[0]["GoodReceivedNote_TotalAmountLocal"]);//13
                string docamountbase = Convert.ToString(PurchaseOrderEditdt.Rows[0]["GoodReceivedNote_DocumentAmountLocal"]);//13
                string totalamtforeign = Convert.ToString(PurchaseOrderEditdt.Rows[0]["GoodReceivedNote_TotalAmountforeign"]);//13


                txtpartyStockReceipt.Text = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PartyBENo"]);//13
                txtreceiptnumber.Text = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Receiptno"]);//13


                if (!string.IsNullOrEmpty(Convert.ToString(PurchaseOrderEditdt.Rows[0]["PartyBEDate"])))
                {
                    dt_Dateofdispatch.Date = Convert.ToDateTime(Convert.ToString(PurchaseOrderEditdt.Rows[0]["PartyBEDate"]));
                }
                if (!string.IsNullOrEmpty(Convert.ToString(PurchaseOrderEditdt.Rows[0]["Receiptdate"])))
                {
                    dt_Dateofarrival.Date = Convert.ToDateTime(Convert.ToString(PurchaseOrderEditdt.Rows[0]["Receiptdate"]));
                }



                txt_docnetamt.Text = String.Format("{0:0.00}", docamountbase); ;
                txt_totamont.Text = String.Format("{0:0.00}", totalamtbase);
                txt_totamont_foreign.Text = totalamtforeign;
                string Billentry_Number = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Billentry_Number"]);//13
                string Billentry_Date = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Billentry_Date"]);//13
                taggingList.Text = Billentry_Number;
                lbl_MultipleDate.Text = Billentry_Date;
                taggingList.ClientEnabled = false;
                ddlInventory.SelectedValue = IsInventory;
                txtVoucherNo.Text = PurchaseOrder_Number;
                dt_PLQuote.Date = Convert.ToDateTime(PurchaseOrder_Date);
                dt_Quotation.Text = (Billentry_Date);

                if (!string.IsNullOrEmpty(PurchaseOrder_IndentDate))
                {

                    dt_Quotation.Text = PurchaseOrder_IndentDate;
                }

                if (VendorId != "")
                {
                    hdfLookupCustomer.Value = VendorId;
                }
                hdnCustomerId.Value = VendorId;
                ProcedureExecute GetVendorGstin = new ProcedureExecute("prc_GstTaxDetails");
                GetVendorGstin.AddVarcharPara("@Action", 500, "GetVendorGSTINByBranch");
                GetVendorGstin.AddVarcharPara("@branchId", 10, Convert.ToString(ddl_Branch.SelectedValue));
                GetVendorGstin.AddVarcharPara("@entityId", 10, Convert.ToString(VendorId));
                DataTable VendorGstin = GetVendorGstin.GetTable();

                if (VendorGstin.Rows.Count > 0)
                {
                    if (Convert.ToString(VendorGstin.Rows[0][0]).Trim() != "")
                    {
                        string VendorGst = Convert.ToString(VendorGstin.Rows[0][0]).Trim();
                        this.Purchase_BillingShipping.GetGSTIN(VendorGst);
                    }
                }
                txtVendorName.Text = VendorName.Trim();
                TabPage page = ASPxPageControl1.TabPages.FindByName("Billing/Shipping");
                page.ClientEnabled = true;
                PopulateContactPersonOfCustomer(VendorId);
                if (Contact_Person_Id != "0")
                {
                    cmbContactPerson.Value = Contact_Person_Id;
                    DataTable dtContactPersonPhone = new DataTable();
                    dtContactPersonPhone = objPurchaseOrderBL.PopulateContactPersonPhone(Contact_Person_Id);

                }

                txt_Refference.Text = PurchaseOrder_Reference;
                ddl_Branch.SelectedValue = BranchId;

                ddl_Currency.SelectedValue = PurchaseOrder_Currency_Id;
                txt_Rate.Text = Currency_Conversion_Rate;
                txt_Rate.ClientEnabled = false;
                ddl_Currency.Enabled = false;

                #region Indent Tagging & Product Tagging
                //string Quoids = Convert.ToString(PurchaseOrderEditdt.Rows[0]["ImportBillOfEntry_Ids"]);

                //if (!String.IsNullOrEmpty(Quoids))
                //{
                //    BindLookUp(PurchaseOrder_Date, PurchaseOrder_BranchId, VendorId);
                //    string[] eachQuo = Quoids.Split(',');
                //    string QuoComponent = "", QuoComponentNumber = "", QuoComponentDate = "";

                //    for (int i = 0; i < taggingGrid.VisibleRowCount; i++)
                //    {
                //        string PurchaseOrder_Id = Convert.ToString(taggingGrid.GetRowValues(i, "GoodReceivedNote_Id"));
                //        if (eachQuo.Contains(PurchaseOrder_Id))
                //        {
                //            QuoComponent += "," + Convert.ToString(taggingGrid.GetRowValues(i, "GoodReceivedNote_Id"));
                //            QuoComponentNumber += "," + Convert.ToString(taggingGrid.GetRowValues(i, "PurchaseOrder_Number"));
                //            QuoComponentDate += "," + Convert.ToString(taggingGrid.GetRowValues(i, "PurchaseOrder_Date_RequisitionDate"));

                //            taggingGrid.Selection.SelectRow(i);
                //        }
                //    }
                //    QuoComponent = QuoComponent.TrimStart(',');
                //    QuoComponentNumber = QuoComponentNumber.TrimStart(',');
                //    QuoComponentDate = QuoComponentDate.TrimStart(',');

                //    if (taggingGrid.GetSelectedFieldValues("PurchaseOrder_Id").Count > 0)
                //    {
                //        if (taggingGrid.GetSelectedFieldValues("PurchaseOrder_Id").Count > 1)
                //        {
                //            QuoComponentDate = "Multiple Purchase Order Dates";
                //        }
                //    }
                //    else
                //    {
                //        QuoComponentDate = "";
                //    }




                //    DataTable dt_QuotationDetails = new DataTable();
                //    string IdKey = Convert.ToString(Request.QueryString["key"]);
                //    dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsFromPO(QuoComponent, IdKey, "", "Edit");

                //    grid_Products.DataSource = GetProductsInfo(dt_QuotationDetails);
                //    grid_Products.DataBind();

                //    DataTable Transaction_dt = (DataTable)Session["ProductOrderDetails"];

                //    for (int i = 0; i < grid_Products.VisibleRowCount; i++)
                //    {
                //        string ComponentID = Convert.ToString(grid_Products.GetRowValues(i, "Quotation_No"));
                //        string ProductList = Convert.ToString(grid_Products.GetRowValues(i, "gvColProduct"));

                //        string[] list = ProductList.Split(new string[] { "||@||" }, StringSplitOptions.None);
                //        string ProductID = Convert.ToString(list[0]) + "||@||%";

                //        var Checkdt = Transaction_dt.Select("Indent_No='" + ComponentID + "' AND ProductID LIKE '" + ProductID + "'");
                //        if (Checkdt.Length > 0)
                //        {
                //            grid_Products.Selection.SelectRow(i);
                //        }
                //    }
                //}
                #endregion



                if (Tax_Option != "0")
                {
                    ddl_AmountAre.Value = Tax_Option;
                }
                if (Tax_Option == "3")
                {
                    btn_SaveRecordTaxs.ClientEnabled = false;
                }
                PopulateGSTCSTVAT("2");
                if (Tax_Code != "0")
                {
                    PopulateGSTCSTVAT("2");
                    setValueForHeaderGST(ddl_VatGstCst, Tax_Code);
                }
                else
                {
                    PopulateGSTCSTVAT("2");
                    ddl_VatGstCst.SelectedIndex = 0;
                }
                ddl_AmountAre.ClientEnabled = false;
                ddl_VatGstCst.ClientEnabled = false;
                txtCreditDays.Text = CreditDays;
                if (PurchaseOrderDueDate != "")
                {
                    dt_PODue.Date = Convert.ToDateTime(PurchaseOrderDueDate);
                }
            }

            if (PurchaseOrderEditdtset.Tables[1].Rows.Count > 0)
            {
                ddlPosGstInvoice.DataSource = PurchaseOrderEditdtset.Tables[1];
                ddlPosGstInvoice.ValueField = "ID";
                ddlPosGstInvoice.TextField = "Name";
                ddlPosGstInvoice.DataBind();
                string PosForGst = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PosForGst"]);
                ddlPosGstInvoice.Value = PosForGst;
                ddlPosGstInvoice.ClientEnabled = false;
            }
        }

        public IEnumerable GetPurchaseOrderBatch()
        {
            List<PurchaseOrderList> PurchaseOrderList = new List<PurchaseOrderList>();
            DataTable Quotationdt = GetPurchaseOrderData().Tables[0];

            for (int i = 0; i < Quotationdt.Rows.Count; i++)
            {
                PurchaseOrderList PurchaseOrders = new PurchaseOrderList();

                PurchaseOrders.SrlNo = Convert.ToString(Quotationdt.Rows[i]["SrlNo"]);
                PurchaseOrders.OrderDetails_Id = Convert.ToString(Quotationdt.Rows[i]["OrderDetails_Id"]);
                PurchaseOrders.gvColProduct = Convert.ToString(Quotationdt.Rows[i]["ProductID"]);
                PurchaseOrders.gvColDiscription = Convert.ToString(Quotationdt.Rows[i]["Description"]);
                PurchaseOrders.gvColQuantity = Convert.ToString(Quotationdt.Rows[i]["Quantity"]);
                PurchaseOrders.gvColUOM = Convert.ToString(Quotationdt.Rows[i]["UOM"]);
                PurchaseOrders.Warehouse = "";
                PurchaseOrders.gvColStockQty = Convert.ToString(Quotationdt.Rows[i]["StockQuantity"]);
                PurchaseOrders.gvColStockUOM = Convert.ToString(Quotationdt.Rows[i]["StockUOM"]);
                PurchaseOrders.gvColStockPurchasePrice = Convert.ToString(Quotationdt.Rows[i]["PurchasePrice"]);
                ///Changes 26-06-2018 Sudip Pal
                PurchaseOrders.gvColStockPurchasePriceforeign = Convert.ToString(Quotationdt.Rows[i]["PurchasePriceforgn"]);
                ///Changes 26-06-2018 Sudip Pal
                PurchaseOrders.gvColDiscount = Convert.ToString(Quotationdt.Rows[i]["Discount"]);
                PurchaseOrders.gvColAmount = Convert.ToString(Quotationdt.Rows[i]["Amount"]);
                ////////////A. Changes 27-06-2018   Sudip Pal
                PurchaseOrders.gvColAmountbase = Convert.ToString(Quotationdt.Rows[i]["Amountbase"]);
                ////////////A. Changes 27-06-2018   Sudip Pal
                PurchaseOrders.gvColTaxAmount = Convert.ToString(Quotationdt.Rows[i]["TaxAmount"]);
                PurchaseOrders.gvColTotalAmountINR = Convert.ToString(Quotationdt.Rows[i]["TotalAmount"]);
                ///Changes 26-06-2018 Sudip Pal
                PurchaseOrders.gvColStockPurchasePriceNetamountbase = Convert.ToString(Quotationdt.Rows[i]["TotalAmountbase"]);
                ///Changes 26-06-2018 Sudip Pal
                PurchaseOrders.Quotation_No = Convert.ToString(0);
                PurchaseOrders.ProductName = Convert.ToString(Quotationdt.Rows[i]["ProductName"]);
                PurchaseOrders.IsComponentProduct = Convert.ToString(Quotationdt.Rows[i]["IsComponentProduct"]);
                //PurchaseOrders.gvColTaxAmountINR = Convert.ToString(Quotationdt.Rows[i]["TaxAmountINR"]);
                PurchaseOrderList.Add(PurchaseOrders);
            }

            return PurchaseOrderList;
        }
        public DataSet GetPurchaseOrderData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GoodReceiveNoteDetailsList_Import");
            proc.AddVarcharPara("@Action", 500, "PoBatchEditDetails");
            proc.AddVarcharPara("@PurchaseOrder_Id", 500, Convert.ToString(Session["ImptStockReceiptID"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(Session["LastCompany"]));
            ds = proc.GetDataSet();
            return ds;
        }



        public DataSet GetProductData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList_Import");
            proc.AddVarcharPara("@Action", 500, "ProductDetails");
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(Session["LastCompany"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public IEnumerable GetProduct()
        {
            List<Product> ProductList = new List<Product>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = GetProductData().Tables[0];
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Product Products = new Product();
                Products.ProductID = Convert.ToString(DT.Rows[i]["Products_ID"]);
                Products.ProductName = Convert.ToString(DT.Rows[i]["Products_Name"]);
                ProductList.Add(Products);
            }

            return ProductList;
        }





        #region  WebMethod
        [WebMethod]
        public static bool CheckUniqueName(string VoucherNo)
        {
            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();

            if (VoucherNo != "" && Convert.ToString(VoucherNo).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(Convert.ToString(VoucherNo).Trim(), "0", "PurchaseOrder_Check");
            }
            return status;
        }
        [WebMethod]
        public static string getSchemeType(string sel_scheme_id)
        {
            //BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //string[] scheme = oDbEngine1.GetFieldValue1("tbl_master_Idschema", "schema_type", "Id = " + Convert.ToInt32(sel_scheme_id), 1);
            //return Convert.ToString(scheme[0]);
            string strschematype = "", strschemalength = "", strschemavalue = "", strschemaBranch = "";
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length,Branch ", " Id = " + Convert.ToInt32(sel_scheme_id));

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strschemaBranch = Convert.ToString(DT.Rows[i]["Branch"]);
                strschemavalue = strschematype + "~" + strschemalength + "~" + strschemaBranch;
            }
            return Convert.ToString(strschemavalue);
        }

        [WebMethod]
        public static string getIndentRequisitionDate(string IndentRequisitionNo)
        {
            BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine();
            string[] RequisitionDate = oDbEngine1.GetFieldValue1("tbl_trans_Indent", "Indent_RequisitionDate", "Indent_Id = " + Convert.ToInt32(IndentRequisitionNo), 1);
            return Convert.ToString(RequisitionDate[0]);
        }


        ////Changes 28-06-2018 Sudip Pal
        //[WebMethod]
        //public static String GetRate(string basedCurrency, string Currency_ID, string Campany_ID, string Dateposting)
        //{
        //    ImportPurchaseOrderBL objPurchaseOrderBL = new ImportPurchaseOrderBL();

        //    //Changes 20-06-2018 Sudip pal

        //    // DataTable dt = objPurchaseOrderBL.GetCurrentConvertedRate(Convert.ToInt16(basedCurrency), Convert.ToInt16(Currency_ID), Campany_ID);
        //    DataTable dt = objPurchaseOrderBL.GetCurrentConvertedRate(Convert.ToInt16(basedCurrency), Convert.ToInt16(Currency_ID), Campany_ID, Convert.ToDateTime(Dateposting).ToString("yyyy-MM-dd"));

        //    //Changes 20-06-2018 Sudip pal
        //    string SalesRate = "";
        //    if (dt.Rows.Count > 0)
        //    {
        //        SalesRate = Convert.ToString(dt.Rows[0]["PurchaseRate"]);
        //    }

        //    return SalesRate;
        //}



        [WebMethod]
        public static object GetRate(string basedCurrency, string Currency_ID, string Campany_ID, string Dateposting)
        {
            ImportPurchaseOrderBL objPurchaseOrderBL = new ImportPurchaseOrderBL();
            CurrencyChecking ochecking = new CurrencyChecking();


            //Changes 20-06-2018 Sudip pal

            // DataTable dt = objPurchaseOrderBL.GetCurrentConvertedRate(Convert.ToInt16(basedCurrency), Convert.ToInt16(Currency_ID), Campany_ID);
            DataTable dt = objPurchaseOrderBL.GetCurrentConvertedRate(Convert.ToInt16(basedCurrency), Convert.ToInt16(Currency_ID), Campany_ID, Convert.ToDateTime(Dateposting).ToString("yyyy-MM-dd"));

            //Changes 20-06-2018 Sudip pal


            string SalesRate = "";
            if (dt.Rows.Count > 0)
            {
                ochecking.PurchaseRate = Convert.ToString(dt.Rows[0]["PurchaseRate"]);
                ochecking.Ismatched = Convert.ToInt32(dt.Rows[0]["Ismatched"]);
            }
            else
            {
                ochecking.Ismatched = 3;

            }

            return ochecking;
        }


        ////Changes 28-06-2018 Sudip Pal

        [WebMethod]
        public static string GetVendorReletedData(string VendorId)
        {
            PurchaseOrderBL objPurchaseOrderBL = new PurchaseOrderBL();
            DataSet dtVendor = objPurchaseOrderBL.GetVendorDetails_CDRelated(VendorId);

            string strDueDate = "";
            if (dtVendor.Tables[0] != null && dtVendor.Tables[0].Rows.Count > 0)
            {
                strDueDate = Convert.ToString(dtVendor.Tables[0].Rows[0]["DueDate"]);
            }

            string strcountryId = "";
            if (dtVendor.Tables[1] != null && dtVendor.Tables[1].Rows.Count > 0)
            {
                strcountryId = Convert.ToString(dtVendor.Tables[1].Rows[0]["add_country"]);
            }
            else
            {
                strcountryId = "0";
            }

            string strOutstanding = "";
            if (dtVendor.Tables[2] != null && dtVendor.Tables[2].Rows.Count > 0)
            {
                var convertDecimal = Convert.ToDecimal(Convert.ToString(dtVendor.Tables[2].Rows[0]["NetOutstanding"]));
                if (convertDecimal > 0)
                {
                    strOutstanding = Convert.ToString(convertDecimal) + "(Db)";
                }
                else
                {
                    strOutstanding = Convert.ToString(convertDecimal * -1).ToString() + "(Cr)";
                }
            }
            else
            {
                strOutstanding = "0.0";
            }

            string strGSTN = "";
            if (dtVendor.Tables[3] != null && dtVendor.Tables[3].Rows.Count > 0)
            {
                strGSTN = Convert.ToString(dtVendor.Tables[3].Rows[0]["CNT_GSTIN"]).Trim();
                if (strGSTN != "")
                {
                    strGSTN = "Yes";
                }
                else
                {
                    strGSTN = "No";
                }
            }

            return strDueDate + "~" + strcountryId + "~" + strOutstanding + "~" + strGSTN;
        }



        #endregion Class

        public void AddmodeExecuted()
        {
            DataSet dst = new DataSet();
            dst = blLayer.GetAllDropDownDetailForGRN();

            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                ddl_numberingScheme.DataSource = dst.Tables[0];
                ddl_numberingScheme.DataBind();
            }
            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                ddl_Branch.DataSource = dst.Tables[1];
                ddl_Branch.DataValueField = "branch_id";
                ddl_Branch.DataTextField = "branch_description";
                ddl_Branch.DataBind();
            }

            if (dst.Tables[3] != null && dst.Tables[3].Rows.Count > 0)
            {

                ddl_AmountAre.DataSource = dst.Tables[3];
                ddl_AmountAre.TextField = "taxGrp_Description";
                ddl_AmountAre.ValueField = "taxGrp_Id";
                ddl_AmountAre.DataBind();

                ListEditItem li = new ListEditItem();
                li.Text = "Import";
                li.Value = "4";
                ddl_AmountAre.Items.Insert(3, li);
            }

        }
        protected void cmbContactPerson_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindContactPerson")
            {
                string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                PopulateContactPersonOfCustomer(InternalId);


            }

        }
        protected void acpContactPersonPhone_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "ContactPersonPhone")
            {
                string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                DataTable dtContactPersonPhone = new DataTable();
                dtContactPersonPhone = objPurchaseOrderBL.PopulateContactPersonPhone(InternalId);
                if (dtContactPersonPhone != null && dtContactPersonPhone.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtContactPersonPhone.Rows)
                    {
                        string phone = Convert.ToString(dr["phf_phoneNumber"]);
                        if (!string.IsNullOrEmpty(phone))
                        {
                            acpContactPersonPhone.JSProperties["cpPhone"] = phone;
                            break;
                        }
                    }

                }
            }
        }
        protected void ddl_VatGstCst_Callback(object sender, CallbackEventArgsBase e)
        {
            string type = e.Parameter.Split('~')[0];
            PopulateGSTCSTVAT(type);
        }
        protected void PopulateGSTCSTVAT(string type)
        {
            DataTable dtGSTCSTVAT = new DataTable();

            if (type == "2")
            {
                dtGSTCSTVAT = objPurchaseOrderBL.PopulateGSTCSTVAT(dt_PLQuote.Date.ToString("yyyy-MM-dd"));


                #region Delete Igst,Cgst,Sgst respectively

                string CompInternalId = Convert.ToString(Session["LastCompany"]);
                string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);

                string ShippingState = "";
                #region ##### Added By : Samrat Roy -- For BillingShippingUserControl ######
                string sstateCode = Purchase_BillingShipping.GeteShippingStateCode();
                ShippingState = sstateCode;
                if (ShippingState.Trim() != "")
                {
                    ShippingState = ShippingState;
                }
                #region ##### Old Code -- BillingShipping ######
                ////if (chkBilling.Checked)
                ////{
                ////    if (CmbState.Value != null)
                ////    {
                ////        ShippingState = CmbState.Text;
                ////        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                ////    }
                ////}
                ////else
                ////{
                ////    if (CmbState1.Value != null)
                ////    {
                ////        ShippingState = CmbState1.Text;
                ////        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                ////    }
                ////}
                #endregion

                #endregion




                if (ShippingState.Trim() != "" && compGstin[0].Trim() != "")
                {

                    if (compGstin.Length > 0)
                    {
                        if (compGstin[0].Substring(0, 2) == ShippingState)
                        {
                            //Check if the state is in union territories then only UTGST will apply
                            //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU      Lakshadweep              PONDICHERRY
                            if (ShippingState == "4" || ShippingState == "26" || ShippingState == "25" || ShippingState == "35" || ShippingState == "31" || ShippingState == "34")
                            {
                                foreach (DataRow dr in dtGSTCSTVAT.Rows)
                                {
                                    if (Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "I" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "C" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "S")
                                    {
                                        dr.Delete();
                                    }
                                }

                            }
                            else
                            {
                                foreach (DataRow dr in dtGSTCSTVAT.Rows)
                                {
                                    if (Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "I" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "U")
                                    {
                                        dr.Delete();
                                    }
                                }
                            }
                            dtGSTCSTVAT.AcceptChanges();
                        }
                        else
                        {
                            foreach (DataRow dr in dtGSTCSTVAT.Rows)
                            {
                                if (Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "C" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "S" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "U")
                                {
                                    dr.Delete();
                                }
                            }
                            dtGSTCSTVAT.AcceptChanges();

                        }

                    }
                }

                //If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
                if (compGstin[0].Trim() == "")
                {
                    foreach (DataRow dr in dtGSTCSTVAT.Rows)
                    {
                        if (Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "C" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "S" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "U" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "I")
                        {
                            dr.Delete();
                        }
                    }
                    dtGSTCSTVAT.AcceptChanges();
                }


                #endregion



                if (dtGSTCSTVAT != null && dtGSTCSTVAT.Rows.Count > 0)
                {
                    ddl_VatGstCst.TextField = "Taxes_Name";
                    ddl_VatGstCst.ValueField = "Taxes_ID";
                    ddl_VatGstCst.DataSource = dtGSTCSTVAT;
                    ddl_VatGstCst.DataBind();
                    ddl_VatGstCst.SelectedIndex = 0;
                }
            }
            else
            {
                ddl_VatGstCst.DataSource = null;
                ddl_VatGstCst.DataBind();
            }
        }
        protected void PopulateContactPersonOfCustomer(string InternalId)
        {

            DataTable dtContactPerson = new DataTable();
            dtContactPerson = blLayer.PopulateContactPersonOfCustomer(InternalId);
            if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
            {
                cmbContactPerson.TextField = "cp_name";
                cmbContactPerson.ValueField = "cp_contactId";
                cmbContactPerson.DataSource = dtContactPerson;
                cmbContactPerson.DataBind();

                cmbContactPerson.Value = cmbContactPerson.Items[0].Value;

            }
        }

        #region Shipping DropDown Call Back Function End
        #region All address Dropdown Populated Data and function


        string[,] GetState(int country)
        {
            StateSelect.SelectParameters[0].DefaultValue = Convert.ToString(country);
            DataView view = (DataView)StateSelect.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] = Convert.ToString(view[i][1]);
            }
            return DATA;

        }

        protected void FillStateCombo(ASPxComboBox cmb, int country)
        {

            string[,] state = GetState(country);
            cmb.Items.Clear();

            for (int i = 0; i < state.GetLength(0); i++)
            {
                cmb.Items.Add(state[i, 1], state[i, 0]);
            }
            //cmb.SelectedIndex = -1;
            // Code Commented By Sandip on 08032017 To avoid Select Option By default End
            //cmb.Items.Insert(0, new ListEditItem("Select", "0"));
            // Code Above Commented By Sandip on 08032017 To avoid Select Option By default End
        }


        string[,] GetCities(int state)
        {


            SelectCity.SelectParameters[0].DefaultValue = Convert.ToString(state);
            DataView view = (DataView)SelectCity.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] = Convert.ToString(view[i][1]);
            }
            return DATA;

        }
        protected void FillCityCombo(ASPxComboBox cmb, int state)
        {

            string[,] cities = GetCities(state);
            cmb.Items.Clear();

            for (int i = 0; i < cities.GetLength(0); i++)
            {
                cmb.Items.Add(cities[i, 1], cities[i, 0]);
            }
            //cmb.SelectedIndex = -1;
            // Code Commented By Sandip on 08032017 To avoid Select Option By default End
            //cmb.Items.Insert(0, new ListEditItem("Select", "0"));
            // Code Above Commented By Sandip on 08032017 To avoid Select Option By default End
        }
        protected void FillPinCombo(ASPxComboBox cmb, int city)
        {
            string[,] pin = GetPin(city);
            cmb.Items.Clear();

            for (int i = 0; i < pin.GetLength(0); i++)
            {
                cmb.Items.Add(pin[i, 1], pin[i, 0]);
            }
            //cmb.SelectedIndex = -1;
        }
        string[,] GetPin(int city)
        {
            SelectPin.SelectParameters[0].DefaultValue = Convert.ToString(city);
            DataView view = (DataView)SelectPin.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] = Convert.ToString(view[i][1]);
            }
            return DATA;
        }
        string[,] GetArea(int city)
        {
            SelectArea.SelectParameters[0].DefaultValue = Convert.ToString(city);
            DataView view = (DataView)SelectArea.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] = Convert.ToString(view[i][1]);
            }
            return DATA;
        }

        protected void FillAreaCombo(ASPxComboBox cmb, int city)
        {
            string[,] area = GetArea(city);
            cmb.Items.Clear();

            for (int i = 0; i < area.GetLength(0); i++)
            {
                cmb.Items.Add(area[i, 1], area[i, 0]);
            }
            //cmb.SelectedIndex = -1;
            // Code Commented By Sandip on 08032017 To avoid Select Option By default Start
            //cmb.Items.Insert(0, new ListEditItem("Select", "0"));
            // Code Above Commented By Sandip on 08032017 To avoid Select Option By default End
        }

        #endregion

        #region Shipping DropDown Call Back Function End
        protected void cmbState_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
            //else
            //{
            //    FillStateCombo(source as ASPxComboBox, 0);
            //}
        }
        protected void cmbCity_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillCityCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
            //else
            //{
            //    FillCityCombo(source as ASPxComboBox, 0);
            //}
        }
        protected void cmbPin_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillPinCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
            //else
            //{
            //    FillPinCombo(source as ASPxComboBox, 0);
            //}
        }
        protected void cmbArea_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillAreaCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
            //else
            //{
            //    FillAreaCombo(source as ASPxComboBox, 0);
            //}
        }

        #endregion Billing DropDown Call Back Function End

        #region Shipping DropDown Call Back Function Start
        protected void cmbState1_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
            //else
            //{
            //    FillStateCombo(source as ASPxComboBox, 0);
            //}
        }

        protected void cmbCity1_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillCityCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
            //else
            //{
            //    FillCityCombo(source as ASPxComboBox, 0);
            //}
        }
        protected void cmbArea1_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillAreaCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
            //else
            //{
            //    FillAreaCombo(source as ASPxComboBox, 0);
            //}
        }
        protected void cmbPin1_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillPinCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
            //else
            //{
            //    FillPinCombo(source as ASPxComboBox, 0);
            //}
        }

        #endregion Shipping DropDown Call Back Function End


        #endregion Billing DropDown Call Back Function End

        #region  Batch Grid
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            //if (strSplitCommand == "Display")

            //Changes 20-06-2018  Sudip Pal
            if (strSplitCommand == "GridForeignCurrency")
            {
                grid.Columns[10].Caption = "Price [" + e.Parameters.Split('~')[1] + "]";
                grid.Columns[9].Caption = "Price [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";
                //grid.Columns[15].Caption = "Net Amt [" + e.Parameters.Split('~')[1] + "]";
                grid.Columns[16].Caption = "Net Amt [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";
                grid.Columns[12].Caption = "Amt [" + e.Parameters.Split('~')[1] + "]";
                grid.Columns[13].Caption = "Amt [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";
                grid.JSProperties["cpnull"] = "no";
            }
            //Changes 20-06-2018  Sudip Pal
            if (strSplitCommand == "Display")
            {

                string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
                if (IsDeleteFrom == "D" || IsDeleteFrom == "C")
                {
                    DataTable POdt = (DataTable)Session["ImptStockReceiptDetails"];
                    grid.DataSource = GetStockReceiptBatch(POdt);

                    grid.DataBind();
                }
                if (e.Parameters.Split('~').Length > 1)
                {
                    if (e.Parameters.Split('~')[1] == "fromComponent")
                    {
                        grid.JSProperties["cpComponent"] = "true";
                    }
                }

                grid.Columns[10].Caption = "Price [" + e.Parameters.Split('~')[1] + "]";
                grid.Columns[9].Caption = "Price [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";
                grid.Columns[16].Caption = "Net Amt [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";
                //grid.Columns[15].Caption = "Net Amt [" + e.Parameters.Split('~')[1] + "]";
                grid.Columns[12].Caption = "Amt [" + e.Parameters.Split('~')[1] + "]";
                grid.Columns[13].Caption = "Amt [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";

                grid.JSProperties["cpnull"] = "no";
            }
            else if (strSplitCommand == "GridBlank")
            {
                Session["ImptStockReceiptDetails"] = null;
                grid.DataSource = null;
                grid.DataBind();


                grid.JSProperties["cpnull"] = "no";
            }
            else if (strSplitCommand == "BindGridOnBillEntry")
            {


                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                String BillComponent1 = "";
                string Product_id1 = "";
                string BillDetails_Id = "";

                if (grid_Products.GetSelectedFieldValues("ComponentID").Count > 0)
                {
                    for (int i = 0; i < grid_Products.GetSelectedFieldValues("ComponentID").Count; i++)
                    {

                        BillComponent1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ComponentID")[i]);
                        Product_id1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ProductID")[i]);
                        BillDetails_Id += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ComponentDetailsID")[i]);

                    }
                    BillComponent1 = BillComponent1.TrimStart(',');
                    BillComponent1 = BillComponent1.Split(',')[0];
                    Product_id1 = Product_id1.TrimStart(',');
                    BillDetails_Id = BillDetails_Id.TrimStart(',');


                    string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);

                    if (Quote_Nos != "$")
                    {

                        DataSet dt_BillEntryDetails = new DataSet();
                        string IdKey = Convert.ToString(Request.QueryString["key"]);
                        if (!string.IsNullOrEmpty(IdKey))
                        {

                            if (IdKey != "ADD")
                            {
                                dt_BillEntryDetails = blLayer.GetBillEntryDetailsForGRNGridBind(BillComponent1, BillDetails_Id, Product_id1, "Edit");
                            }
                            else
                            {
                                dt_BillEntryDetails = blLayer.GetBillEntryDetailsForGRNGridBind(BillComponent1, BillDetails_Id, Product_id1, "Add");
                            }

                        }

                        Session["ImptStockReceiptDetails"] = null;

                        grid.DataSource = GetImportPurchaseOrderInfo(dt_BillEntryDetails.Tables[0], dt_BillEntryDetails.Tables[2], IdKey);
                        grid.DataBind();

                        grid.JSProperties["cpcurrencyId"] = dt_BillEntryDetails.Tables[1].Rows[0]["PurchaseOrder_Currency_Id"];
                        grid.JSProperties["cpcurrencyrate"] = dt_BillEntryDetails.Tables[1].Rows[0]["Currency_Conversion_Rate"];

                        grid.JSProperties["cpTax_Option"] = dt_BillEntryDetails.Tables[1].Rows[0]["Tax_Option"];
                        grid.JSProperties["cpCreditDays"] = dt_BillEntryDetails.Tables[1].Rows[0]["CreditDays"];

                        grid.JSProperties["cpRec"] = Convert.ToString(dt_BillEntryDetails.Tables[1].Rows[0]["Receiptno"]) + '~' + Convert.ToString(dt_BillEntryDetails.Tables[1].Rows[0]["Receiptdate"]) + '~' + Convert.ToString(dt_BillEntryDetails.Tables[1].Rows[0]["Currency_Conversion_Rate"]);


                        grid.JSProperties["cpBillEntryHeader"] = Convert.ToString(dt_BillEntryDetails.Tables[1].Rows[0]["GoodsInTransitName"]) + '~' + Convert.ToString(dt_BillEntryDetails.Tables[1].Rows[0]["GoodsInTransit"]) + '~' + dt_BillEntryDetails.Tables[1].Rows[0]["CustomHouse"] + '~' + dt_BillEntryDetails.Tables[1].Rows[0]["CustomHouseId"];






                        if (!string.IsNullOrEmpty(Convert.ToString(dt_BillEntryDetails.Tables[1].Rows[0]["DueDate"])))
                        {
                            grid.JSProperties["cpDueDate"] = Convert.ToDateTime(dt_BillEntryDetails.Tables[1].Rows[0]["DueDate"]).ToString("yyyy-MM-dd");
                        }

                        grid.JSProperties["cpTotalAmountfrgn"] = dt_BillEntryDetails.Tables[1].Rows[0]["PurchaseOrder_TotalAmountforeign"];
                        grid.JSProperties["cpTotalAmountLocal"] = dt_BillEntryDetails.Tables[1].Rows[0]["PurchaseOrder_TotalAmountLocal"];
                        grid.JSProperties["cpTotaldocAmtLocal"] = dt_BillEntryDetails.Tables[1].Rows[0]["PurchaseOrder_DocumentAmountLocal"];
                        grid.JSProperties["cpcurrenynamel"] = dt_BillEntryDetails.Tables[1].Rows[0]["CurrencyName"];


                        //grid.JSProperties["cpIncoterm"] = Convert.ToString(dt_BillEntryDetails.Tables[1].Rows[0]["IncotermsId"]) + '~' + Convert.ToString(dt_BillEntryDetails.Tables[1].Rows[0]["Incoterms"]) + '~' + dt_BillEntryDetails.Tables[1].Rows[0]["Description"];

                        grid.JSProperties["cpRcm"] = Convert.ToString(dt_BillEntryDetails.Tables[1].Rows[0]["PosForGst"]) + '~' + Convert.ToString(dt_BillEntryDetails.Tables[1].Rows[0]["Pos_StateId"]) + '~' + Convert.ToString(dt_BillEntryDetails.Tables[1].Rows[0]["IsRcm"]);

                        grid.Columns[10].Caption = "Price [" + dt_BillEntryDetails.Tables[1].Rows[0]["CurrencyName"] + "]";
                        grid.Columns[9].Caption = "Price [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";
                        grid.Columns[16].Caption = "Net Amt [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";
                        //grid.Columns[15].Caption = "Net Amt [" + dt_BillEntryDetails.Tables[1].Rows[0]["CurrencyName"] + "]";
                        grid.Columns[12].Caption = "Amt [" + dt_BillEntryDetails.Tables[1].Rows[0]["CurrencyName"] + "]";
                        grid.Columns[13].Caption = "Amt [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";

                    }
                    else
                    {
                        grid.DataSource = null;
                        grid.DataBind();
                    }
                }
                else
                {
                    DataTable Transactiondt = CreateTempTable("Transaction");
                    Transactiondt.Rows.Add("1", "1", "", "", "", "", "", "0.0000", "", "0.00", "0.00", "0.00", "0.00", "0.00", "0", "0", "", "", "I", "0", "", "", "", "");

                    Session["ImptStockReceiptDetails"] = Transactiondt;


                    grid.DataSource = null;
                    grid.DataBind();

                    grid.JSProperties["cpBindNullGrid"] = "Y";
                }
                grid.JSProperties["cpnull"] = "yes";
            }
        }
        public IEnumerable GetImportPurchaseOrderInfo(DataTable SalesOrderdt1, DataTable Taxdetails, string Order_Id)
        {
            List<SalesOrder> OrderList = new List<SalesOrder>();
            DataColumnCollection dtC = SalesOrderdt1.Columns;


            for (int i = 0; i < SalesOrderdt1.Rows.Count; i++)
            {

                SalesOrder Orders = new SalesOrder();

                Orders.SrlNo = Convert.ToString(i + 1);
                Orders.OrderDetails_Id = Convert.ToString(i + 1);
                Orders.gvColProduct = Convert.ToString(SalesOrderdt1.Rows[i]["Products_ID"]);
                Orders.gvColDiscription = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductDescription"]);
                Orders.gvColQuantity = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Quantity"]);
                Orders.gvColUOM = Convert.ToString(SalesOrderdt1.Rows[i]["UOM_ShortName"]);
                Orders.Warehouse = "";
                Orders.gvColStockQty = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_StockQty"]);
                Orders.gvColStockUOM = Convert.ToString(SalesOrderdt1.Rows[i]["UOM_ShortName"]);
                Orders.gvColStockPurchasePrice = Convert.ToString(SalesOrderdt1.Rows[i]["SalePrice"]);
                ///Changes 13-06-2018 Sudip Pal
                Orders.gvColStockPurchasePriceforeign = Convert.ToString(SalesOrderdt1.Rows[i]["PurchasePriceforgn"]);
                ///Changes 13-06-2018 Sudip Pal
                ///Changes 14-06-2018 Sudip Pal
                Orders.gvColStockPurchasePriceNetamountbase = Convert.ToString(SalesOrderdt1.Rows[i]["TotalAmountBase"]);
                ///Changes 14-06-2018 Sudip Pal

                Orders.gvColDiscount = "";
                Orders.gvColAmount = "";
                Orders.gvColTaxAmount = "";
                Orders.gvColTotalAmountINR = "";

                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Indent_No"])))//Added on 15-02-2017
                { Orders.Indent = Convert.ToInt64(SalesOrderdt1.Rows[i]["Indent_No"]); }
                else
                { Orders.Indent = 0; }
                if (dtC.Contains("Indent"))
                {
                    Orders.Indent_Num = Convert.ToString(SalesOrderdt1.Rows[i]["Indent"]);//subhabrata on 21-02-2017
                }
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Products_Name"])))
                { Orders.ProductName = Convert.ToString(SalesOrderdt1.Rows[i]["Products_Name"]); }
                else
                {
                    Orders.ProductName = "";
                }

                // Mapping With Warehouse with Product Srl No

                string strQuoteDetails_Id = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Id"]).Trim();



                // End


                OrderList.Add(Orders);
            }

            BindSessionByDatatable(SalesOrderdt1, Taxdetails);

            return OrderList;
        }

        #region Subhabrata/SessionBind
        //Subhabrata on 02-03-2017
        public bool BindSessionByDatatable(DataTable dt, DataTable dttaxdetails)
        {
            bool IsSuccess = false;
            DataTable purChaseDT = new DataTable();

            DataTable purChaseDTtaxdetails = new DataTable();

            purChaseDT.Columns.Add("SrlNo", typeof(string));
            purChaseDT.Columns.Add("OrderDetails_Id", typeof(string));
            purChaseDT.Columns.Add("ProductID", typeof(string));
            purChaseDT.Columns.Add("Description", typeof(string));
            purChaseDT.Columns.Add("Quantity", typeof(string));
            purChaseDT.Columns.Add("UOM", typeof(string));
            purChaseDT.Columns.Add("Warehouse", typeof(string));
            purChaseDT.Columns.Add("StockQuantity", typeof(string));
            purChaseDT.Columns.Add("StockUOM", typeof(string));
            purChaseDT.Columns.Add("PurchasePrice", typeof(string));
            purChaseDT.Columns.Add("PurchasePriceforgn", typeof(string));
            purChaseDT.Columns.Add("Discount", typeof(string));
            purChaseDT.Columns.Add("Amount", typeof(string));
            purChaseDT.Columns.Add("Amountbase", typeof(string));
            purChaseDT.Columns.Add("DutyAmtINR", typeof(string));
            purChaseDT.Columns.Add("TaxAmount", typeof(string));
            purChaseDT.Columns.Add("TotalAmount", typeof(string));
            purChaseDT.Columns.Add("TotalAmountBase", typeof(string));
            purChaseDT.Columns.Add("Status", typeof(string));
            purChaseDT.Columns.Add("Indent_No", typeof(string));
            purChaseDT.Columns.Add("ProductName", typeof(string));
            purChaseDT.Columns.Add("IsComponentProduct", typeof(string));
            purChaseDT.Columns.Add("IsLinkedProduct", typeof(string));
            purChaseDT.Columns.Add("TaxAmountINR", typeof(string));




            purChaseDTtaxdetails.Columns.Add("slNo", typeof(string));
            purChaseDTtaxdetails.Columns.Add("TaxCode", typeof(string));
            purChaseDTtaxdetails.Columns.Add("AltTaxCode", typeof(string));
            purChaseDTtaxdetails.Columns.Add("Percentage", typeof(string));
            purChaseDTtaxdetails.Columns.Add("Amount", typeof(string));
            purChaseDTtaxdetails.Columns.Add("AmountINR", typeof(string));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IsSuccess = true;
                DataColumnCollection dtC = dt.Columns;
                string SalePrice, Discount, Amount, TaxAmount, TotalAmount, Order_Num1, ProductName, IsComponent, IsLinkedProduct, PurchasePriceforgn, TotalAmountBase, Amountbase, DutyAmtINR;

                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["SalePrice"])))
                { SalePrice = Convert.ToString(dt.Rows[i]["SalePrice"]); }
                else
                { SalePrice = ""; }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Discount"])))
                { Discount = Convert.ToString(dt.Rows[i]["Discount"]); }
                else
                { Discount = ""; }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Amount"])))
                { Amount = Convert.ToString(dt.Rows[i]["Amount"]); }
                else { Amount = ""; }

                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TaxAmount"])))
                {
                    TaxAmount = Convert.ToString(dt.Rows[i]["TaxAmount"]);
                }
                else
                {
                    TaxAmount = "";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalAmount"])))
                { TotalAmount = Convert.ToString(dt.Rows[i]["TotalAmount"]); }
                else { TotalAmount = ""; }
                if (dtC.Contains("Indent"))
                {
                    Order_Num1 = Convert.ToString(dt.Rows[i]["Indent"]);//subhabrata on 21-02-2017
                }
                else
                {
                    Order_Num1 = "";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Products_Name"])))
                {
                    ProductName = Convert.ToString(dt.Rows[i]["Products_Name"]);
                }
                else
                {
                    ProductName = "";
                }
                //Subhabrata
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["IsComponentProduct"])))
                {
                    IsComponent = Convert.ToString(dt.Rows[i]["IsComponentProduct"]);
                }
                else
                {
                    IsComponent = "";
                }

                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["IsLinkedProduct"])))
                {
                    IsLinkedProduct = Convert.ToString(dt.Rows[i]["IsLinkedProduct"]);
                }
                else
                {
                    IsLinkedProduct = "";
                }

                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["PurchasePriceforgn"])))
                {
                    PurchasePriceforgn = Convert.ToString(dt.Rows[i]["PurchasePriceforgn"]);
                }
                else
                {
                    PurchasePriceforgn = "";
                }

                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalAmountBase"])))
                {
                    TotalAmountBase = Convert.ToString(dt.Rows[i]["TotalAmountBase"]);
                }
                else
                {
                    TotalAmountBase = "";
                }

                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Amountbase"])))
                {
                    Amountbase = Convert.ToString(dt.Rows[i]["Amountbase"]);
                }
                else
                {
                    Amountbase = "";
                }

                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["DutyAmtINR"])))
                {
                    DutyAmtINR = Convert.ToString(dt.Rows[i]["DutyAmtINR"]);
                }
                else
                {
                    DutyAmtINR = "";
                }





                string TaxAmountINR = "";

                TaxAmountINR = Convert.ToString(Convert.ToDecimal(dt.Rows[i]["TotalAmountBase"]) - Convert.ToDecimal(dt.Rows[i]["Amountbase"]));

                //End

                purChaseDT.Rows.Add(Convert.ToString(i + 1), Convert.ToString(i + 1), Convert.ToString(dt.Rows[i]["Products_ID"]), Convert.ToString(dt.Rows[i]["QuoteDetails_ProductDescription"]),
                  Convert.ToString(dt.Rows[i]["QuoteDetails_Quantity"]), Convert.ToString(dt.Rows[i]["UOM_ShortName"]), "", Convert.ToString(dt.Rows[i]["QuoteDetails_StockQty"]), Convert.ToString(dt.Rows[i]["UOM_ShortName"]),
                             SalePrice, PurchasePriceforgn, Discount, Amount, Amountbase, DutyAmtINR, TaxAmount, TotalAmount, TotalAmountBase, "U", Convert.ToInt64(dt.Rows[i]["Indent_No"]), ProductName, IsComponent, IsLinkedProduct, TaxAmountINR);





                DataRow[] result = dttaxdetails.Select("ProductTax_ProductId =" + Convert.ToString(dt.Rows[i]["QuoteDetails_Id"]));

                foreach (DataRow row in result)
                {
                    purChaseDTtaxdetails.Rows.Add(Convert.ToString(i + 1), row["TaxCode"], 0, row["Percentage"], row["Amount"], row["TaxAmountINR"]);

                }


            }

            Session["ImptStockReceiptDetails"] = purChaseDT;
            Session["ImportStockreceiptFinalTaxRecord"] = purChaseDTtaxdetails;


            return IsSuccess;
        }//End
        #endregion

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
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            //grid.DataSource = GetVoucher();
            if (Session["ImptStockReceiptDetails"] != null)
            {
                DataTable POdt = (DataTable)Session["ImptStockReceiptDetails"];
                DataView dvData = new DataView(POdt);
                dvData.RowFilter = "Status <> 'D'";
                grid.DataSource = GetStockReceiptBatch(dvData.ToTable());
            }
        }

        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {

            ddl_AmountAre.Value = HDNtaxtypeamt.Value;
            grid.JSProperties["cpSaveSuccessOrFail"] = null;
            grid.JSProperties["cpPurchaseOrderNo"] = null;
            DataTable StockReceiptDT = new DataTable();
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);

            if (Session["ImptStockReceiptDetails"] != null)
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["ImptStockReceiptDetails"];
                foreach (DataRow row in dt.Rows)
                {
                    DataColumnCollection dtC = dt.Columns;

                    if (dtC.Contains("Indent_Num"))
                    { dt.Columns.Remove("Indent_Num"); }
                    break;
                }//End
                StockReceiptDT = dt;
            }
            else
            {
                StockReceiptDT.Columns.Add("SrlNo", typeof(string));
                StockReceiptDT.Columns.Add("OrderDetails_Id", typeof(string));
                StockReceiptDT.Columns.Add("ProductID", typeof(string));
                StockReceiptDT.Columns.Add("Description", typeof(string));
                StockReceiptDT.Columns.Add("Quantity", typeof(string));
                StockReceiptDT.Columns.Add("UOM", typeof(string));
                StockReceiptDT.Columns.Add("Warehouse", typeof(string));
                StockReceiptDT.Columns.Add("StockQuantity", typeof(string));
                StockReceiptDT.Columns.Add("StockUOM", typeof(string));
                StockReceiptDT.Columns.Add("PurchasePrice", typeof(string));
                StockReceiptDT.Columns.Add("PurchasePriceforgn", typeof(string));
                StockReceiptDT.Columns.Add("Discount", typeof(string));
                StockReceiptDT.Columns.Add("Amount", typeof(string));
                StockReceiptDT.Columns.Add("Amountbase", typeof(string));
                StockReceiptDT.Columns.Add("DutyAmtINR", typeof(string));
                StockReceiptDT.Columns.Add("TaxAmount", typeof(string));
                StockReceiptDT.Columns.Add("TotalAmount", typeof(string));
                StockReceiptDT.Columns.Add("TotalAmountBase", typeof(string));
                StockReceiptDT.Columns.Add("Status", typeof(string));
                StockReceiptDT.Columns.Add("Indent_No", typeof(string));
                StockReceiptDT.Columns.Add("ProductName", typeof(string));
                StockReceiptDT.Columns.Add("IsComponentProduct", typeof(string));
                StockReceiptDT.Columns.Add("IsLinkedProduct", typeof(string));
            }

            foreach (var args in e.InsertValues)
            {
                string ProductDetails = Convert.ToString(args.NewValues["gvColProduct"]);

                if (ProductDetails != "" && ProductDetails != "0")
                {
                    string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                    string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string ProductID = ProductDetailsList[0];
                    string Description = ProductDetailsList[1];
                    string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                    string Quantity = Convert.ToString(args.NewValues["gvColQuantity"]);
                    string UOM = Convert.ToString(args.NewValues["gvColUOM"]);
                    string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
                    string StockQuantity = Convert.ToString(args.NewValues["gvColStockQty"]);
                    string StockUOM = Convert.ToString(args.NewValues["gvColStockUOM"]);
                    string PurchasePrice = Convert.ToString(args.NewValues["gvColStockPurchasePrice"]);
                    string PurchasePriceforgn = Convert.ToString(args.NewValues["gvColStockPurchasePriceforeign"]);
                    string Discount = Convert.ToString(args.NewValues["gvColDiscount"]);
                    string Amount = (Convert.ToString(args.NewValues["gvColAmount"]) != "") ? Convert.ToString(args.NewValues["gvColAmount"]) : "0";
                    string Amountbase = (Convert.ToString(args.NewValues["gvColAmountbase"]) != "") ? Convert.ToString(args.NewValues["gvColAmountbase"]) : "0";
                    string DutyAmtINR = (Convert.ToString(args.NewValues["gvColDutyAmtINR"]) != "") ? Convert.ToString(args.NewValues["gvColDutyAmtINR"]) : "0";
                    string TaxAmount = (Convert.ToString(args.NewValues["gvColTaxAmount"]) != "") ? Convert.ToString(args.NewValues["gvColTaxAmount"]) : "0";
                    string TotalAmount = (Convert.ToString(args.NewValues["gvColTotalAmountINR"]) != "") ? Convert.ToString(args.NewValues["gvColTotalAmountINR"]) : "0";
                    string TotalAmountbase = (Convert.ToString(args.NewValues["gvColStockPurchasePriceNetamountbase"]) != "") ? Convert.ToString(args.NewValues["gvColStockPurchasePriceNetamountbase"]) : "0";

                    string Indent_Id = (Convert.ToString(args.NewValues["Indent"]) != "") ? Convert.ToString(args.NewValues["Indent"]) : "0";
                    string IsComponentProduct = (Convert.ToString(args.NewValues["IsComponentProduct"]) != "") ? Convert.ToString(args.NewValues["IsComponentProduct"]) : "N";
                    string IsLinkedProduct = (Convert.ToString(args.NewValues["IsLinkedProduct"]) != "") ? Convert.ToString(args.NewValues["IsLinkedProduct"]) : "N";
                    string TaxAmountINR = (Convert.ToString(args.NewValues["gvColTaxAmountINR"]) != "") ? Convert.ToString(args.NewValues["gvColTaxAmountINR"]) : "0";

                    StockReceiptDT.Rows.Add(SrlNo, "0", ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, PurchasePrice, PurchasePriceforgn, Discount,
                        Amount, Amountbase, DutyAmtINR, TaxAmount, TotalAmount, TotalAmountbase, "I", Indent_Id, ProductName, "", IsLinkedProduct, TaxAmountINR);

                    if (IsComponentProduct == "Y")
                    {

                        DataTable ComponentTable = objPurchaseOrderBL.GetComponantProduct(ProductID);
                        foreach (DataRow drr in ComponentTable.Rows)
                        {
                            string sProductsID = Convert.ToString(drr["sProductsID"]);
                            string Products_Description = Convert.ToString(drr["Products_Description"]);
                            string Sales_UOM_Name = Convert.ToString(drr["Sales_UOM_Name"]);
                            string Conversion_Multiplier = Convert.ToString(drr["Conversion_Multiplier"]);
                            string Stk_UOM_Name = Convert.ToString(drr["Stk_UOM_Name"]);
                            string Product_PurPrice = Convert.ToString(drr["Product_PurPrice"]);
                            string Prod_PurchasePriceforgn = Convert.ToString(drr["gvColStockPurchasePriceforeign"]);
                            string Products_Name = Convert.ToString(drr["Products_Name"]);
                            string StkQty = Convert.ToString(Convert.ToDecimal(Quantity) * Convert.ToDecimal(Conversion_Multiplier));


                            StockReceiptDT.Rows.Add(Convert.ToString(Convert.ToInt32(SrlNo) + 1), "0", sProductsID, Products_Description, Quantity, Sales_UOM_Name,
                            Warehouse, StkQty, Stk_UOM_Name, Product_PurPrice, Prod_PurchasePriceforgn, "0.0",
                   "0.0", "0.0", "0.0", "0.0", "0.0", "0.0", "I", Indent_Id, Products_Name, "", "Y", TaxAmountINR);

                        }
                    }
                }
            }
            foreach (var args in e.UpdateValues)
            {
                string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                string OrderDetails_Id = Convert.ToString(args.Keys["OrderDetails_Id"]);
                string ProductDetails = Convert.ToString(args.NewValues["gvColProduct"]);

                bool isDeleted = false;
                foreach (var arg in e.DeleteValues)
                {
                    string DeleteID = Convert.ToString(arg.Keys["OrderDetails_Id"]);
                    if (DeleteID == OrderDetails_Id)
                    {
                        isDeleted = true;
                        break;
                    }
                }

                if (isDeleted == false)
                {
                    if (ProductDetails != "" && ProductDetails != "0")
                    {
                        string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                        string ProductID = Convert.ToString(ProductDetailsList[0]);
                        string Description = ProductDetailsList[1];
                        string Quantity = Convert.ToString(args.NewValues["gvColQuantity"]);
                        string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                        string UOM = Convert.ToString(args.NewValues["gvColUOM"]);
                        string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
                        string StockQuantity = Convert.ToString(args.NewValues["gvColStockQty"]);
                        string StockUOM = Convert.ToString(args.NewValues["gvColStockUOM"]);
                        string PurchasePrice = Convert.ToString(args.NewValues["gvColStockPurchasePrice"]);

                        string PurchasePriceforgn = Convert.ToString(args.NewValues["gvColStockPurchasePriceforeign"]);
                        string Discount = Convert.ToString(args.NewValues["gvColDiscount"]);
                        string Amount = (Convert.ToString(args.NewValues["gvColAmount"]) != "") ? Convert.ToString(args.NewValues["gvColAmount"]) : "0";

                        string Amountbase = (Convert.ToString(args.NewValues["gvColAmountbase"]) != "") ? Convert.ToString(args.NewValues["gvColAmountbase"]) : "0";
                        string DutyAmtINR = (Convert.ToString(args.NewValues["gvColDutyAmtINR"]) != "") ? Convert.ToString(args.NewValues["gvColDutyAmtINR"]) : "0";
                        string TaxAmount = (Convert.ToString(args.NewValues["gvColTaxAmount"]) != "") ? Convert.ToString(args.NewValues["gvColTaxAmount"]) : "0";
                        string TotalAmount = (Convert.ToString(args.NewValues["gvColTotalAmountINR"]) != "") ? Convert.ToString(args.NewValues["gvColTotalAmountINR"]) : "0";

                        string TotalAmountbase = (Convert.ToString(args.NewValues["gvColStockPurchasePriceNetamountbase"]) != "") ? Convert.ToString(args.NewValues["gvColStockPurchasePriceNetamountbase"]) : "0";



                        string Indent_Id = (Convert.ToString(args.NewValues["Indent"]) != "") ? Convert.ToString(args.NewValues["Indent"]) : "0";
                        string IsComponentProduct = (Convert.ToString(args.NewValues["IsComponentProduct"]) != "") ? Convert.ToString(args.NewValues["IsComponentProduct"]) : "N";
                        string IsLinkedProduct = (Convert.ToString(args.NewValues["IsLinkedProduct"]) != "") ? Convert.ToString(args.NewValues["IsLinkedProduct"]) : "N";
                        string TaxAmountINR = (Convert.ToString(args.NewValues["gvColTaxAmountINR"]) != "") ? Convert.ToString(args.NewValues["gvColTaxAmountINR"]) : "0";


                        bool Isexists = false;
                        foreach (DataRow drr in StockReceiptDT.Rows)
                        {
                            string OldOrderDetailsId = Convert.ToString(drr["OrderDetails_Id"]);

                            if (OldOrderDetailsId == OrderDetails_Id)
                            {
                                Isexists = true;

                                drr["ProductID"] = ProductDetails;
                                drr["Description"] = Description;
                                drr["Quantity"] = Quantity;
                                drr["UOM"] = UOM;
                                drr["Warehouse"] = Warehouse;
                                drr["StockQuantity"] = StockQuantity;
                                drr["StockUOM"] = StockUOM;
                                drr["PurchasePrice"] = PurchasePrice;
                                drr["PurchasePriceforgn"] = PurchasePriceforgn;


                                drr["Discount"] = Discount;
                                drr["Amount"] = Amount;
                                drr["Amountbase"] = Amountbase;
                                drr["DutyAmtINR"] = DutyAmtINR;
                                drr["TaxAmount"] = TaxAmount;
                                drr["TotalAmount"] = TotalAmount;
                                drr["TotalAmountBase"] = TotalAmountbase;


                                drr["Status"] = "U";
                                if (!string.IsNullOrEmpty(Indent_Id))
                                { drr["Indent_No"] = Indent_Id; }
                                drr["ProductName"] = ProductName;
                                drr["IsComponentProduct"] = IsComponentProduct;
                                drr["IsLinkedProduct"] = IsLinkedProduct;
                                drr["TaxAmountINR"] = TaxAmountINR;
                                break;
                            }
                        }

                        if (Isexists == false)
                        {
                            StockReceiptDT.Rows.Add(SrlNo, OrderDetails_Id, ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM,
                                PurchasePrice, Discount, Amount, Amountbase, DutyAmtINR, TaxAmount, TotalAmount, TotalAmountbase, "U", Indent_Id, ProductName, IsComponentProduct
                                , IsLinkedProduct, TaxAmountINR);
                        }
                        if (IsComponentProduct == "Y")
                        {
                            DataTable ComponentTable = objPurchaseOrderBL.GetComponantProduct(ProductID);
                            foreach (DataRow drr in ComponentTable.Rows)
                            {
                                string sProductsID = Convert.ToString(drr["sProductsID"]);
                                string Products_Description = Convert.ToString(drr["Products_Description"]);
                                string Sales_UOM_Name = Convert.ToString(drr["Sales_UOM_Name"]);
                                string Conversion_Multiplier = Convert.ToString(drr["Conversion_Multiplier"]);
                                string Stk_UOM_Name = Convert.ToString(drr["Stk_UOM_Name"]);
                                string Product_PurPrice = Convert.ToString(drr["Product_PurPrice"]);

                                string Product_PurchasePriceforgn = Convert.ToString(args.NewValues["gvColStockPurchasePriceforeign"]);

                                string Products_Name = Convert.ToString(drr["Products_Name"]);
                                string StkQty = Convert.ToString(Convert.ToDecimal(Quantity) * Convert.ToDecimal(Conversion_Multiplier));

                                StockReceiptDT.Rows.Add(Convert.ToString(Convert.ToInt32(SrlNo) + 1), "0", sProductsID, Products_Description, Quantity, Sales_UOM_Name,
                                    Warehouse, StkQty, Stk_UOM_Name, Product_PurPrice, Product_PurchasePriceforgn, "0.0",
                           "0.0", "0.0", "0.0", "0.0", "0.0", "0.0", "I", Indent_Id, ProductName, "", "Y", TaxAmountINR);

                            }
                        }
                    }
                }
            }

            foreach (var args in e.DeleteValues)
            {
                string OrderDetails_Id = Convert.ToString(args.Keys["OrderDetails_Id"]);

                for (int i = StockReceiptDT.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = StockReceiptDT.Rows[i];
                    string delOrderDetailsId = Convert.ToString(dr["OrderDetails_Id"]);

                    if (delOrderDetailsId == OrderDetails_Id)
                        dr.Delete();
                }
                StockReceiptDT.AcceptChanges();

                if (OrderDetails_Id.Contains("~") != true)
                {
                    StockReceiptDT.Rows.Add("0", OrderDetails_Id, "0", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "D", "0", "", "", "", "0");
                }
            }

            int j = 1;
            foreach (DataRow dr in StockReceiptDT.Rows)
            {
                string Status = Convert.ToString(dr["Status"]);
                dr["SrlNo"] = j.ToString();

                if (Status != "D")
                {
                    if (Status == "I")
                    {
                        string strID = Convert.ToString("Q~" + j);
                        dr["OrderDetails_Id"] = strID;
                    }
                    j++;
                }
            }
            StockReceiptDT.AcceptChanges();

            Session["ImptStockReceiptDetails"] = StockReceiptDT;
            if (IsDeleteFrom != "D" && IsDeleteFrom != "C")
            {
                string ActionType = Convert.ToString(ViewState["ActionType"]);
                string IsInventory = Convert.ToString(ddlInventory.SelectedValue);
                string StockReceiptID = Convert.ToString(Session["ImptStockReceiptID"]);
                string strSchemeType = Convert.ToString(ddl_numberingScheme.SelectedValue);
                string strPurchaseNumber = Convert.ToString(txtVoucherNo.Text);
                string strPurchaseDate = Convert.ToString(dt_PLQuote.Date);
                String BillEntry_Id = "";
                string BillEntryDate = string.Empty;
                for (int i = 0; i < taggingGrid.GetSelectedFieldValues("BillEntry_Id").Count; i++)
                {
                    BillEntry_Id += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("BillEntry_Id")[i]);
                }
                BillEntry_Id = BillEntry_Id.TrimStart(',');

                if (taggingGrid.GetSelectedFieldValues("BillEntry_Id").Count == 1)
                {
                    BillEntryDate = Convert.ToString(dt_Quotation.Text);
                }

                string strVendor = Convert.ToString(hdfLookupCustomer.Value);
                string strContactName = Convert.ToString(cmbContactPerson.Value);
                string Reference = Convert.ToString(txt_Refference.Text);
                string strBranch = Convert.ToString(HDNbranch.Value);

                string strCurrency = Convert.ToString(ddl_Currency.SelectedValue);
                string strRate = Convert.ToString(txt_Rate.Value);
                string strTaxOption = Convert.ToString(ddl_AmountAre.Value);
                string strTaxCode = Convert.ToString(ddl_VatGstCst.Value).Split('~')[0];
                string CompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string currency = Convert.ToString(HttpContext.Current.Session["LocalCurrency"]);
                string[] ActCurrency = currency.Split('~');
                int BaseCurrencyId = Convert.ToInt32(ActCurrency[0]);
                string strCreditDays = Convert.ToString(txtCreditDays.Text);
                string strPoDate = Convert.ToString(dt_PODue.Date);
                string strexpectddatedispatchDate = null;
                if (!string.IsNullOrEmpty(dt_Dateofdispatch.Text))
                {
                    strexpectddatedispatchDate = Convert.ToString(dt_Dateofdispatch.Date);
                }

                string strexpectddatearriavalDate = null;
                if (!string.IsNullOrEmpty(dt_Dateofarrival.Text))
                {
                    strexpectddatearriavalDate = Convert.ToString(dt_Dateofarrival.Date);
                }

                DataTable tempQuotation = StockReceiptDT.Copy();
                foreach (DataRow dr in tempQuotation.Rows)
                {
                    string Status = Convert.ToString(dr["Status"]);

                    if (Status == "I")
                    {
                        dr["OrderDetails_Id"] = "0";

                        string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                        dr["ProductID"] = Convert.ToString(list[0]);
                        dr["UOM"] = Convert.ToString(list[3]);
                        dr["StockUOM"] = Convert.ToString(list[5]);
                    }
                    else if (Status == "U" || Status == "")
                    {
                        string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                        dr["ProductID"] = Convert.ToString(list[0]);
                        dr["UOM"] = Convert.ToString(list[3]);
                        dr["StockUOM"] = Convert.ToString(list[5]);
                        string OrderDetails_Id = Convert.ToString(dr["OrderDetails_Id"]);
                        if (OrderDetails_Id.Contains("~") == true)
                        {
                            dr["OrderDetails_Id"] = "0";
                        }
                    }
                }

                tempQuotation.AcceptChanges();

                #region Product Stock Generate

                string StockDetails = Convert.ToString(hdnJsonProductStock.Value);
                JavaScriptSerializer _ser = new JavaScriptSerializer();
                _ser.MaxJsonLength = Int32.MaxValue;
                List<ProductStockDetails> StockEntryJson = _ser.Deserialize<List<ProductStockDetails>>(StockDetails);
                DataTable _Stockdt = ToDataTable(StockEntryJson);

                #endregion

                #region DataTable of Warehouse

                DataTable tempWarehousedt = new DataTable();
                //if (Session["Product_StockList"] != null)
                if (_Stockdt != null)
                {
                    //DataTable Warehousedt = (DataTable)Session["Product_StockList"];
                    DataTable Warehousedt = _Stockdt.Copy();
                    tempWarehousedt = Warehousedt.DefaultView.ToTable(false, "Product_SrlNo", "LoopID", "SrlNo", "WarehouseID", "Quantity", "Batch", "SerialNo", "Barcode", "MfgDate", "ExpiryDate");
                }
                else
                {
                    tempWarehousedt.Columns.Add("Product_SrlNo", typeof(string));
                    tempWarehousedt.Columns.Add("SrlNo", typeof(string));
                    tempWarehousedt.Columns.Add("Row_Number", typeof(string));
                    tempWarehousedt.Columns.Add("WarehouseID", typeof(string));
                    tempWarehousedt.Columns.Add("Quantity", typeof(string));
                    tempWarehousedt.Columns.Add("BatchID", typeof(string));
                    tempWarehousedt.Columns.Add("SerialNo", typeof(string));
                    tempWarehousedt.Columns.Add("Barcode", typeof(string));
                    tempWarehousedt.Columns.Add("MfgDate", typeof(string));
                    tempWarehousedt.Columns.Add("ExpiryDate", typeof(string));
                }

                foreach (DataRow wtrow in tempWarehousedt.Rows)
                {
                    string strMfgDate = Convert.ToString(wtrow["MfgDate"]).Trim();
                    string strExpiryDate = Convert.ToString(wtrow["ExpiryDate"]).Trim();

                    if (strMfgDate != "")
                    {
                        string DD = strMfgDate.Substring(0, 2);
                        string MM = strMfgDate.Substring(3, 2);
                        string YYYY = strMfgDate.Substring(6, 4);
                        string Date = YYYY + '-' + MM + '-' + DD;

                        wtrow["MfgDate"] = Date;
                    }

                    if (strExpiryDate != "")
                    {
                        string DD = strExpiryDate.Substring(0, 2);
                        string MM = strExpiryDate.Substring(3, 2);
                        string YYYY = strExpiryDate.Substring(6, 4);
                        string Date = YYYY + '-' + MM + '-' + DD;

                        wtrow["ExpiryDate"] = Date;
                    }
                }

                #endregion

                #region DataTable of Inline Tax

                DataTable TaxDetaildt = new DataTable();
                if (Session["ImportStockreceiptFinalTaxRecord"] != null)
                {
                    TaxDetaildt = (DataTable)Session["ImportStockreceiptFinalTaxRecord"];
                }

                #endregion Inline Tax End

                #region DataTable Of Purchase Order Tax Details

                DataTable POTaxDetailsdt = new DataTable();
                if (Session["POTaxDetails"] != null)
                {
                    POTaxDetailsdt = (DataTable)Session["POTaxDetails"];
                }
                else
                {
                    POTaxDetailsdt.Columns.Add("Taxes_ID", typeof(string));
                    POTaxDetailsdt.Columns.Add("Taxes_Name", typeof(string));
                    POTaxDetailsdt.Columns.Add("Percentage", typeof(string));
                    POTaxDetailsdt.Columns.Add("Amount", typeof(string));
                    POTaxDetailsdt.Columns.Add("AltTax_Code", typeof(string));
                }

                DataTable tempTaxDetailsdt = new DataTable();
                tempTaxDetailsdt = POTaxDetailsdt.DefaultView.ToTable(false, "Taxes_ID", "Percentage", "Amount", "AltTax_Code");

                tempTaxDetailsdt.Columns.Add("SlNo", typeof(string));
                //    tempTaxDetailsdt.Columns.Add("AltTaxCode", typeof(string));

                tempTaxDetailsdt.Columns["SlNo"].SetOrdinal(0);
                tempTaxDetailsdt.Columns["Taxes_ID"].SetOrdinal(1);
                tempTaxDetailsdt.Columns["AltTax_Code"].SetOrdinal(2);
                tempTaxDetailsdt.Columns["Percentage"].SetOrdinal(3);
                tempTaxDetailsdt.Columns["Amount"].SetOrdinal(4);

                foreach (DataRow d in tempTaxDetailsdt.Rows)
                {
                    d["SlNo"] = "0";
                    //d["AltTaxCode"] = "0";
                }

                #endregion DataTable Of Purchase Order Tax Details


                string validate = "";

                // DataTable Of Billing Address
                #region ##### Added By : Samrat Roy -- to get BillingShipping user control data
                DataTable tempBillAddress = new DataTable();

                tempBillAddress = Purchase_BillingShipping.GetBillingShippingTable();


                #endregion


                if (ActionType == "Add")
                {
                    strSchemeType = Convert.ToString(HDNumberingschema.Value);

                    string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);
                    validate = checkNMakeJVCode(strPurchaseNumber, Convert.ToInt32(SchemeList[0]));

                    if (!reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "PO"))
                    {
                        validate = "UdfMandetory";
                    }
                }
                DataView dvData = new DataView(tempQuotation);
                dvData.RowFilter = "Status<>'D'";
                DataTable dt_tempQuotation = dvData.ToTable();

                var duplicateRecords = dt_tempQuotation.AsEnumerable()
               .GroupBy(r => r["ProductID"]) //coloumn name which has the duplicate values
               .Where(gr => gr.Count() > 1)
                .Select(g => g.Key);

                foreach (var d in duplicateRecords)
                {
                    validate = "duplicateProduct";
                }
                if (ddlInventory.SelectedValue != "N")
                {
                    foreach (DataRow dr in tempQuotation.Rows)
                    {
                        decimal ProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                        string Status = Convert.ToString(dr["Status"]);

                        if (Status != "D")
                        {
                            if (ProductQuantity == 0)
                            {
                                validate = "nullQuantity";
                                break;
                            }
                        }
                    }
                }

                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Transporter_IPIMandatory' AND IsActive=1");
                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();
                    if (Convert.ToString(Session["TransporterVisibilty"]).Trim() == "Yes")
                    {
                        if (IsMandatory == "Yes")
                        {
                            if (hfControlData.Value.Trim() == "")
                            {
                                validate = "transporteMandatory";
                            }
                        }
                    }
                }


                #region Terms & Conditions
                DataTable DT_TC = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='TC_IPIMandatory' AND IsActive=1");
                if (DT_TC != null && DT_TC.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT_TC.Rows[0]["Variable_Value"]).Trim();


                    objEngine = new BusinessLogicLayer.DBEngine();
                    DataTable DTVisible = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_TC_IPI' AND IsActive=1");
                    if (Convert.ToString(DTVisible.Rows[0]["Variable_Value"]).Trim() == "Yes")
                    {
                        if (IsMandatory == "Yes")
                        {

                            if (TermsConditionsControl.GetControlVisibility("dtDeliveryDate") && (TermsConditionsControl.GetControlValue("dtDeliveryDate") == "" || TermsConditionsControl.GetControlValue("dtDeliveryDate") == "@"))
                            {
                                validate = "TCMandatory";
                            }
                        }
                    }
                }
                #endregion

                #region Section For Stock Section

                if (IsBarcodeGeneratete() == true)
                {
                    if (ddlInventory.SelectedValue != "N")
                    {
                        foreach (DataRow dr in tempQuotation.Rows)
                        {
                            string ProductID = Convert.ToString(dr["ProductID"]);
                            string strSrlNo = Convert.ToString(dr["SrlNo"]);
                            decimal strProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                            string Status = Convert.ToString(dr["Status"]);
                            string IsLinkedProduct = Convert.ToString(dr["IsLinkedProduct"]);

                            decimal strWarehouseQuantity = 0;
                            GetQuantityBaseOnProduct(_Stockdt, strSrlNo, ref strWarehouseQuantity);

                            if (ProductID != "")
                            {
                                if (CheckProduct_IsInventory(ProductID) == true)
                                {
                                    if (Status != "D")
                                    {
                                        if (IsLinkedProduct != "Y")
                                        {
                                            if (strProductQuantity != strWarehouseQuantity)
                                            {
                                                validate = "checkWarehouse";
                                                grid.JSProperties["cpProductSrlIDCheck"] = strSrlNo;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                DataTable Copy_tempWarehousedt = tempWarehousedt.Copy();
                DataView dvDataSRno = new DataView(Copy_tempWarehousedt);
                dvDataSRno.RowFilter = "SerialNo <> ''";
                DataTable Filter_tempWarehousedt = dvDataSRno.ToTable();

                var duplicateRecordssrno = Filter_tempWarehousedt.AsEnumerable()
               .GroupBy(r => r["SerialNo"])
               .Where(gr => gr.Count() > 1)
               .Select(g => g.Key);
                foreach (var d in duplicateRecordssrno)
                {
                    grid.JSProperties["cpduplicateSerialMsg"] = "Cannot process with duplicate Serial No.";
                    validate = "duplicateSerial";
                }

                DataTable duplicateSerial = GetDuplicateDetails(StockReceiptID, tempQuotation, Filter_tempWarehousedt, "FullFinal");
                if (duplicateSerial != null && duplicateSerial.Rows.Count > 0)
                {
                    foreach (DataRow Serialrow in duplicateSerial.Rows)
                    {
                        string ProductID = Convert.ToString(Serialrow["ProductID"]);
                        string SerialNo = Convert.ToString(Serialrow["SerialNo"]);

                        grid.JSProperties["cpduplicateSerialMsg"] = "(" + SerialNo + ")  Serials already exists for  (" + ProductID + ")  Product.";
                        validate = "duplicateSerial";
                        break;
                    }
                }

                


                string WHMandatory = "YES";
                foreach (DataRow dr in tempQuotation.Rows)
                {
                    string SrlNo = Convert.ToString(dr["SrlNo"]);

                    foreach (DataRow WHdr in tempWarehousedt.Rows)
                    {
                        string WHSrlNo = Convert.ToString(WHdr["Product_SrlNo"]);
                        if (SrlNo == WHSrlNo)
                        {
                            WHMandatory = "NO";
                        }

                        if (WHMandatory == "YES")
                        {
                            validate = "EmptyWarehouse";
                            grid.JSProperties["cpEmptyWarehouse"] = SrlNo;
                            break;
                        }

                    }

                    if (tempWarehousedt.Rows.Count == 0)
                    {
                        validate = "EmptyWarehouse";
                        grid.JSProperties["cpEmptyWarehouse"] = SrlNo;
                        break;

                    }

                }

                foreach (DataRow dr in tempQuotation.Rows)
                {
                    string Is_Inventory = getProductIsInventoryExists(Convert.ToString(dr["ProductID"]));
                    if (Convert.ToString(dr["ProductID"]) != "0")
                    {
                        if (Is_Inventory.ToUpper() != "N")
                        {
                            string strSrlNo = Convert.ToString(dr["SrlNo"]);
                            decimal strProductQuantity = Convert.ToDecimal(dr["Quantity"]);

                            decimal strWarehouseQuantity = 0;
                            GetQuantityBaseOnProduct(_Stockdt, strSrlNo, ref strWarehouseQuantity);

                            //if (strWarehouseQuantity != 0)
                            //{
                            if (strProductQuantity != strWarehouseQuantity)
                            {
                                validate = "checkWarehouseQty";
                                grid.JSProperties["cpProductQtyWHQty"] = strSrlNo;
                                break;
                            }
                            //}
                        }
                    }

                }






                string _ShippingState = Convert.ToString(Purchase_BillingShipping.GetShippingStateId());

                if (_ShippingState == "0")
                {
                    validate = "BillingShippingNotLoaded";
                }


                if (validate == "outrange" || validate == "BillingShippingNotLoaded" || validate == "checkWarehouseQty" || validate == "EmptyWarehouse" || validate == "duplicate" || validate == "duplicateSerial" || validate == "UdfMandetory" || validate == "transporteMandatory" || validate == "TCMandatory" || validate == "nullQuantity" || validate == "duplicateProduct" || validate == "checkWarehouse" || validate == "VendorAddressProblem")
                {
                    grid.JSProperties["cpSaveSuccessOrFail"] = validate;
                }
                else
                {
                    //if (IsPOExist(BillEntry_Id))
                    //{

                    string TaxType = "", ShippingState = "";
                    ShippingState = Convert.ToString(Purchase_BillingShipping.GetShippingStateId());
                    if (ddl_AmountAre.Value == "1") TaxType = "E";
                    else if (ddl_AmountAre.Value == "2") TaxType = "I";

                    #region Delete Product Tax

                    //string TaxDetails = Convert.ToString(hdnJsonProductTax.Value);
                    //JavaScriptSerializer ser = new JavaScriptSerializer();
                    //ser.MaxJsonLength = Int32.MaxValue;
                    //List<ProductTaxDetails> TaxEntryJson = ser.Deserialize<List<ProductTaxDetails>>(TaxDetails);


                    //foreach (DataRow productRow in tempQuotation.Rows)
                    //{
                    //    string _ProductID = Convert.ToString(productRow["ProductID"]);
                    //    string _SrlNo = Convert.ToString(productRow["SrlNo"]);
                    //    string _IsEntry = "";

                    //    if (_ProductID != "")
                    //    {
                    //        var TaxValue = TaxEntryJson.Where(x => x.SrlNo == _SrlNo).ToList().SingleOrDefault();

                    //        if (TaxValue != null)
                    //        {
                    //            _IsEntry = TaxValue.IsTaxEntry;

                    //            if (_IsEntry == "N")
                    //            {
                    //                DataRow[] deletedRow = TaxDetaildt.Select("SlNo=" + _SrlNo);
                    //                if (deletedRow.Length > 0)
                    //                {
                    //                    foreach (DataRow dr in deletedRow)
                    //                    {
                    //                        TaxDetaildt.Rows.Remove(dr);
                    //                    }
                    //                    TaxDetaildt.AcceptChanges();
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    #endregion

                    DataTable ReverseTaxtable = CreateReverseTaxTable();

                    if (Convert.ToString(ddl_AmountAre.Value).Trim() != "4")
                    {
                        if (!chkRCM.Checked)
                        {

                            //if (ddlPosGstInvoice.Value.ToString() == "S")
                            //{
                            //    ShippingState = Purchase_BillingShipping.GeteShippingStateCode();
                            //}
                            //else
                            //{
                            //    ShippingState = Purchase_BillingShipping.GetBillingStateCode();
                            //}

                            //TaxDetaildt = gstTaxDetails.SetTaxTableDataWithProductSerialForImportPurchaseRoundOff(ref tempQuotation, "SrlNo", "ProductID", "Amount", "Amountbase", "TaxAmount", "TaxAmountINR", "TotalAmount", "TotalAmountBase", TaxDetaildt, "P", dt_PLQuote.Date.ToString("yyyy-MM-dd"), strBranch, ShippingState, TaxType, Convert.ToString(hdnCustomerId.Value));

                        }
                        else
                        {
                            string RShippingState = "";
                            if (ddlPosGstInvoice.Value.ToString() == "S")
                            {
                                RShippingState = Purchase_BillingShipping.GeteShippingStateCode();
                            }
                            else
                            {
                                RShippingState = Purchase_BillingShipping.GetBillingStateCode();
                            }
                            ReverseTaxtable = gstTaxDetails.GetReverseTaxTable(tempQuotation, "SrlNo", "ProductID", "Amount", "TaxAmount", ReverseTaxtable, "P", dt_PLQuote.Date.ToString("yyyy-MM-dd"), strBranch, RShippingState, TaxType, Convert.ToString(hdnCustomerId.Value));

                        }
                    }

                    if (AddModifyStockReceipt(StockReceiptID, UniquePurchaseNumber, strPurchaseDate, BillEntry_Id, BillEntryDate, hdnPurchaseInvoiceID.Value, hdnPurchaseOrderID.Value, strVendor, strContactName,
                    Reference, strBranch, strCurrency, strRate, strTaxOption, strTaxCode, CompanyID, BaseCurrencyId, tempQuotation, ActionType, IsInventory,
                    tempWarehousedt, TaxDetaildt, tempTaxDetailsdt, tempBillAddress, strCreditDays, strPoDate, txt_totamont.Text, txt_docnetamt.Text, txt_totamont_foreign.Text,
                    strexpectddatedispatchDate, txtpartyStockReceipt.Text, txtreceiptnumber.Text
                     , strexpectddatearriavalDate, ReverseTaxtable
                    ) == false)
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                    }
                    else
                    {
                        //if (approveStatus != "")
                        //{
                        //    if (approveStatus == "2")
                        //    {
                        //        grid.JSProperties["cpApproverStatus"] = "approve";
                        //    }
                        //    else
                        //    {
                        //        grid.JSProperties["cpApproverStatus"] = "rejected";
                        //    }
                        //}
                        grid.JSProperties["cpPurchaseOrderNo"] = UniquePurchaseNumber;
                        #region To Show By Default Cursor after SAVE AND NEW
                        if (ActionType == "Add")
                        {
                            string schemavalue = ddl_numberingScheme.SelectedValue;
                            Session["schemavaluePO"] = schemavalue;
                            Session["POType"] = IsInventory;
                            string schematype = strPurchaseNumber.Trim();
                            if (schematype == "Auto")
                            {
                                Session["SaveModePO"] = "A";
                            }
                            else
                            {
                                Session["SaveModePO"] = "M";
                            }
                        }


                        #endregion
                    }

                    if (Session["ImptStockReceiptDetails"] != null)
                    {
                        Session["ImptStockReceiptDetails"] = null;
                    }
                    //}
                    //else
                    //{
                    //    grid.JSProperties["cpSaveSuccessOrFail"] = "Ponotexist";
                    //}
                }
            }
            else
            {
                DataView dvData = new DataView(StockReceiptDT);
                dvData.RowFilter = "Status <> 'D'";
                grid.DataSource = GetStockReceiptBatch(dvData.ToTable());
                grid.DataBind();
            }
        }
        
        public string getProductIsInventoryExists(string ProductId)
        {
            string IsInventory = string.Empty;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetIsInventoryFlagByProductID");
            proc.AddVarcharPara("@ProductId", 500, ProductId);
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["sProduct_IsInventory"]).ToUpper() == "TRUE")
                {
                    IsInventory = "Y";
                }
                else
                {
                    IsInventory = "N";
                }
            }
            return IsInventory;
        }
        public void GetQuantityBaseOnProduct(DataTable Warehousedt, string strProductSrlNo, ref decimal WarehouseQty)
        {
            decimal sum = 0;

            if (Warehousedt != null)
            {
                for (int i = 0; i < Warehousedt.Rows.Count; i++)
                {
                    DataRow dr = Warehousedt.Rows[i];
                    string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);

                    if (strProductSrlNo == Product_SrlNo)
                    {
                        string strQuantity = (Convert.ToString(dr["SalesQuantity"]) != "") ? Convert.ToString(dr["SalesQuantity"]) : "0";
                        var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);

                        sum = sum + Convert.ToDecimal(weight);
                    }
                }
            }

            WarehouseQty = sum;
        }
        public DataTable CreateReverseTaxTable()
        {
            DataTable TaxRecord = new DataTable();

            TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
            TaxRecord.Columns.Add("TaxCode", typeof(System.String));
            TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
            TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
            TaxRecord.Columns.Add("Amount", typeof(System.Decimal));
            //TaxRecord.Columns.Add("AmountINR", typeof(System.Decimal));

            return TaxRecord;

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

        public void GetProductType(string ProductID, ref string Type)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetSchemeType");
            proc.AddVarcharPara("@ProductID", 100, Convert.ToString(ProductID));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }
        }
        public bool CheckProduct_IsInventory(string ProductID)
        {
            bool IsInventory = true;

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = objEngine.GetDataTable("Master_sProducts", " sProduct_IsInventory ", " sProducts_ID='" + ProductID + "'");
            if (DT != null && DT.Rows.Count > 0)
            {
                string strIsInventory = Convert.ToString(DT.Rows[0]["sProduct_IsInventory"]).Trim();

                if (strIsInventory == "True") IsInventory = true;
                else IsInventory = false;
            }

            return IsInventory;
        }
        public bool IsBarcodeGeneratete()
        {
            bool IsGeneratete = false;

            try
            {
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
        private bool IsPOExist(string pcid)
        {
            bool IsExist = false;
            if (pcid != "" && Convert.ToString(pcid).Trim() != "")
            {
                DataTable dt = new DataTable();

                var elements = pcid.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string items in elements)
                {
                    dt = oDBEngine.GetDataTable("select Count(*) as isexist from tbl_trans_ImportPurchaseOrder where PurchaseOrder_Id=" + items + "");
                    if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                    {
                        IsExist = true;
                    }
                }

            }
            else
            {
                IsExist = true;
            }

            return IsExist;
        }
        public IEnumerable GetStockReceiptBatch(DataTable Quotationdt)
        {
            List<PurchaseOrderList> PurchaseOrderList = new List<PurchaseOrderList>();
            DataColumnCollection dtC = Quotationdt.Columns;
            for (int i = 0; i < Quotationdt.Rows.Count; i++)
            {
                PurchaseOrderList PurchaseOrders = new PurchaseOrderList();

                PurchaseOrders.SrlNo = Convert.ToString(Quotationdt.Rows[i]["SrlNo"]);
                PurchaseOrders.OrderDetails_Id = Convert.ToString(Quotationdt.Rows[i]["OrderDetails_Id"]);
                PurchaseOrders.gvColProduct = Convert.ToString(Quotationdt.Rows[i]["ProductID"]);
                PurchaseOrders.gvColDiscription = Convert.ToString(Quotationdt.Rows[i]["Description"]);
                PurchaseOrders.gvColQuantity = Convert.ToString(Quotationdt.Rows[i]["Quantity"]);
                PurchaseOrders.gvColUOM = Convert.ToString(Quotationdt.Rows[i]["UOM"]);
                PurchaseOrders.Warehouse = "";
                PurchaseOrders.gvColStockQty = Convert.ToString(Quotationdt.Rows[i]["StockQuantity"]);
                PurchaseOrders.gvColStockUOM = Convert.ToString(Quotationdt.Rows[i]["StockUOM"]);
                PurchaseOrders.gvColStockPurchasePrice = Convert.ToString(Quotationdt.Rows[i]["PurchasePrice"]);
                ////Changes 26-06-2018 Sudip Pal
                PurchaseOrders.gvColStockPurchasePriceforeign = Convert.ToString(Quotationdt.Rows[i]["PurchasePriceforgn"]);
                PurchaseOrders.gvColStockPurchasePriceNetamountbase = Convert.ToString(Quotationdt.Rows[i]["TotalAmountbase"]);
                ////Changes 26-06-2018 Sudip Pal
                PurchaseOrders.gvColDiscount = Convert.ToString(Quotationdt.Rows[i]["Discount"]);
                PurchaseOrders.gvColAmount = Convert.ToString(Quotationdt.Rows[i]["Amount"]);
                ////Changes 27-06-2018 Sudip Pal
                PurchaseOrders.gvColAmountbase = Convert.ToString(Quotationdt.Rows[i]["Amountbase"]);
                ////Changes 27-06-2018 Sudip Pal
                PurchaseOrders.gvColDutyAmtINR = Convert.ToString(Quotationdt.Rows[i]["DutyAmtINR"]);
                PurchaseOrders.gvColTaxAmount = Convert.ToString(Quotationdt.Rows[i]["TaxAmount"]);
                PurchaseOrders.gvColTotalAmountINR = Convert.ToString(Quotationdt.Rows[i]["TotalAmount"]);


                if (!string.IsNullOrEmpty(Convert.ToString(Quotationdt.Rows[i]["Indent_No"])))
                { PurchaseOrders.Indent = Convert.ToInt64(Quotationdt.Rows[i]["Indent_No"]); }
                else
                { PurchaseOrders.Indent = 0; }


                PurchaseOrders.ProductName = Convert.ToString(Quotationdt.Rows[i]["ProductName"]);
                PurchaseOrders.IsComponentProduct = Convert.ToString(Quotationdt.Rows[i]["IsComponentProduct"]);
                PurchaseOrders.IsLinkedProduct = Convert.ToString(Quotationdt.Rows[i]["IsLinkedProduct"]);
                PurchaseOrderList.Add(PurchaseOrders);
            }

            return PurchaseOrderList;
        }
        public bool AddModifyStockReceipt(string StockReceiptID, string UniquePurchaseNumber, string strPurchaseDate, string BillEntryIDS, string BillEntryDate,
        string PurchaseInvoiceIDS, string PurchaseOrderIDS, string strVendor, string strContactName,
       string Reference, string strBranch, string strCurrency, string strRate, string strTaxOption,
       string strTaxCode, string CompanyID, int BaseCurrencyId, DataTable PurchaseOrderdt,
       string ActionType, string IsInventory, DataTable Warehousedt, DataTable TaxDetaildt, DataTable PurchaseOrderTaxdt, DataTable tempBillAddress
      , string strCreditDays, string strPoDate, string totalamount, string docamount, string purtotalforeign,
       string bedate, string beno, string receiptno
       , string receiptdate, DataTable ReverseTaxtable)
        {
            try
            {
                if (PurchaseOrderdt.Rows.Count > 0)  // Cross Branch Section by Sam on 10052017 Start only Condition to prevent Twice Firing
                {
                    DataSet dsInst = new DataSet();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("prc_ImportStockReceipt_AddEdit", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Action", ActionType);
                    cmd.Parameters.AddWithValue("@IsInventory", IsInventory);
                    cmd.Parameters.AddWithValue("@StockReceiptID", StockReceiptID);
                    cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));
                    cmd.Parameters.AddWithValue("@BranchId", strBranch);

                    if (Request.QueryString["op"] == "yes")
                    {
                        cmd.Parameters.AddWithValue("@FinYear", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(Session["LastFinYear"]));
                    }

                    cmd.Parameters.AddWithValue("@DocumentNumber", UniquePurchaseNumber);
                    cmd.Parameters.AddWithValue("@BillEntryIds", BillEntryIDS);
                    if (!String.IsNullOrEmpty(BillEntryDate))
                    { cmd.Parameters.AddWithValue("@BillEntryDate", BillEntryDate); }
                    cmd.Parameters.AddWithValue("@PurchaseInvoiceIDS", PurchaseInvoiceIDS);
                    cmd.Parameters.AddWithValue("@PurchaseOrderIDS", PurchaseOrderIDS);

                    cmd.Parameters.AddWithValue("@PurchaseOrder_Date", strPurchaseDate);
                    cmd.Parameters.AddWithValue("@PurchaseOrder_VendorId", strVendor);
                    cmd.Parameters.AddWithValue("@Contact_Person_Id", strContactName);
                    cmd.Parameters.AddWithValue("@PurchaseOrder_Reference", Reference);
                    cmd.Parameters.AddWithValue("@PurchaseOrder_Currency_Id", Convert.ToInt32(strCurrency));
                    cmd.Parameters.AddWithValue("@Currency_Conversion_Rate", strRate);
                    cmd.Parameters.AddWithValue("@Tax_Option", strTaxOption);
                    cmd.Parameters.AddWithValue("@Tax_Code", 0);
                    //cmd.Parameters.AddWithValue("@PurchaseOrder_SalesmanId", strAgents);
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(Session["userid"]));
                    cmd.Parameters.AddWithValue("@PurchaseOrderDetails", PurchaseOrderdt);
                    // cmd.Parameters.AddWithValue("@TaxDetail", TaxDetaildt);
                    cmd.Parameters.AddWithValue("@PurchaseOrderTax", PurchaseOrderTaxdt);
                    //cmd.Parameters.AddWithValue("@PurchasePaymentDetails", paymentdet);
                    cmd.Parameters.AddWithValue("@PurchaseOrderAddress", tempBillAddress);
                    cmd.Parameters.AddWithValue("@CreditDays", strCreditDays);
                    cmd.Parameters.AddWithValue("@PoDate", strPoDate);
                    cmd.Parameters.AddWithValue("@TotalAmountbase", totalamount);
                    cmd.Parameters.AddWithValue("@TotalDiocumenttamountbase", docamount);
                    //cmd.Parameters.AddWithValue("@PurchaseOrder_ExpdispatchDate", strexpectddatedispatchDate);
                    //cmd.Parameters.AddWithValue("@PurchaseOrder_ExparrivalDate", strexpectddatearriavalDate);
                    cmd.Parameters.AddWithValue("@TotalForeign", purtotalforeign);
                    //cmd.Parameters.AddWithValue("@VesselNumber", vessel);

                    cmd.Parameters.AddWithValue("@BENumber", beno);
                    if (!String.IsNullOrEmpty(bedate))
                    {
                        cmd.Parameters.AddWithValue("@BeDate", bedate);
                    }

                    cmd.Parameters.AddWithValue("@Receipt_Number", receiptno);
                    cmd.Parameters.AddWithValue("@ReceiptDate", receiptdate);
                    cmd.Parameters.AddWithValue("@GoodsInTransit", hdnMainAccountID.Value);
                    cmd.Parameters.AddWithValue("@entrydate", Convert.ToString(dt_EntryDate.Date));
                    cmd.Parameters.AddWithValue("@PosForGst", Convert.ToString(ddlPosGstInvoice.Value));
                    cmd.Parameters.AddWithValue("@CustomHouse", hdnCustomHouseID.Value);
                    cmd.Parameters.AddWithValue("@PBunGSTTaxDetails", ReverseTaxtable);
                    cmd.Parameters.AddWithValue("@NumberingSchemeID", ddl_numberingScheme.SelectedValue.ToString().Split('~')[0]);
                    cmd.Parameters.AddWithValue("@LedgerAccountID", hdnLedgerAccountID.Value);
                    cmd.Parameters.AddWithValue("@WarehouseDetail", Warehousedt);

                    //HiddenField LedgerAccountID = (HiddenField)PaymentDetails.FindControl("hdnLedgerAccountID");

                    //cmd.Parameters.AddWithValue("@LedgerAccountID", LedgerAccountID.Value);



                    //if (Session["POwarehousedetailstemp"] != null)
                    //{
                    //    DataTable temtable = new DataTable();
                    //    DataTable Warehousedtssss = (DataTable)Session["POwarehousedetailstemp"];

                    //    temtable = Warehousedtssss.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "productid", "Inventrytype", "StockID", "isnew");
                    //    cmd.Parameters.AddWithValue("@udt_StockOpeningwarehousentrie", temtable);
                    //}


                    SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    output.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(output);

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);
                    Session["Insert"] = "Y";
                    cmd.Dispose();
                    con.Dispose();

                    grid.JSProperties["cpPurchaseOrderID"] = Convert.ToString(output.Value);
                    //Udf Add mode
                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                    if (udfTable != null)
                    {
                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("IMBILL", "ImportStockReceipt" + Convert.ToString(output.Value), udfTable, Convert.ToString(Session["userid"]));
                    }

                    if (!string.IsNullOrEmpty(hfControlData.Value))
                    {
                        CommonBL objCommonBL = new CommonBL();
                        objCommonBL.InsertTransporterControlDetails(Convert.ToInt32(output.Value), "IMSR".ToUpper(), hfControlData.Value, Convert.ToString(HttpContext.Current.Session["userid"]));
                    }
                    if (!string.IsNullOrEmpty(hfTermsConditionData.Value))
                    {
                        TermsConditionsControl.SaveTC(hfTermsConditionData.Value, Convert.ToString(output.Value), "IMSR");
                    }
                    if (chkmail.Checked)
                    {
                        //   exportToPDF.ExportToPdfforEmail("PO-Default~D", "Porder", Server.MapPath("~"), "", Convert.ToString(output.Value));

                        int retval = 0;
                        if (!string.IsNullOrEmpty(Convert.ToString(output.Value)))
                        {
                            retval = Sendmail_Purchaseorder(output.Value.ToString());
                        }
                    }


                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {

            oDBEngine = new BusinessLogicLayer.DBEngine();
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
                    prefCompCode = dtSchema.Rows[0]["prefix"].ToString();
                    sufxCompCode = dtSchema.Rows[0]["suffix"].ToString();
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);

                    sqlQuery = "SELECT max(tjv.GoodReceivedNote_Number) FROM tbl_trans_ImportGoodReceivedNote tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.PurchaseOrder_Number))) = 1";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.GoodReceivedNote_Number))) = 1 and GoodReceivedNote_Number like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.GoodReceivedNote_Number) FROM tbl_trans_ImportGoodReceivedNote tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.PurchaseOrder_Number))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.GoodReceivedNote_Number))) = 1 and GoodReceivedNote_Number like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
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
                            UniquePurchaseNumber = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        UniquePurchaseNumber = startNo.PadLeft(paddCounter, '0');
                        UniquePurchaseNumber = prefCompCode + paddedStr + sufxCompCode;
                        return "ok";
                    }
                }
                else
                {
                    sqlQuery = "SELECT GoodReceivedNote_Number FROM tbl_trans_ImportGoodReceivedNote WHERE GoodReceivedNote_Number LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    UniquePurchaseNumber = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }
        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            string IsLinkedProduct = Convert.ToString(e.GetValue("IsLinkedProduct"));
            if (IsLinkedProduct == "Y")
                e.Row.ForeColor = Color.Blue;
        }
        #endregion



        #region Taxpopup
        public void setValueForHeaderGST(ASPxComboBox aspxcmb, string taxId)
        {
            for (int i = 0; i < aspxcmb.Items.Count; i++)
            {
                if (Convert.ToString(aspxcmb.Items[i].Value).Split('~')[0] == taxId.Split('~')[0])
                {
                    aspxcmb.Items[i].Selected = true;
                    break;
                }
            }

        }

        protected DataTable GetTaxDataWithGST(DataTable existing)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList_Import");
            proc.AddVarcharPara("@Action", 500, "TaxDetailsForGst");
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["ImptStockReceiptID"]));
            dt = proc.GetTable();
            if (dt.Rows.Count > 0)
            {
                DataRow gstRow = existing.NewRow();
                gstRow["Taxes_ID"] = 0;
                gstRow["Taxes_Name"] = dt.Rows[0]["TaxRatesSchemeName"];
                gstRow["Percentage"] = dt.Rows[0]["OrderTax_Percentage"];
                gstRow["Amount"] = dt.Rows[0]["OrderTax_Amount"];
                gstRow["AltTax_Code"] = dt.Rows[0]["Gst"];

                UpdateGstForCharges(Convert.ToString(dt.Rows[0]["Gst"]));
                // txtGstCstVatCharge.Value = gstRow["Amount"];
                existing.Rows.Add(gstRow);
            }
            SetTotalCharges(existing);
            return existing;
        }

        public void SetTotalCharges(DataTable taxTableFinal)
        {
            decimal totalCharges = 0;
            foreach (DataRow dr in taxTableFinal.Rows)
            {
                if (Convert.ToString(dr["Taxes_ID"]) != "0")
                {
                    if (Convert.ToString(dr["Taxes_Name"]).Contains("(+)"))
                    {
                        totalCharges += Convert.ToDecimal(dr["Amount"]);
                    }
                    else
                    {
                        totalCharges -= Convert.ToDecimal(dr["Amount"]);
                    }
                }
                else
                {//Else part For Gst 
                    totalCharges += Convert.ToDecimal(dr["Amount"]);
                }
            }
            txtQuoteTaxTotalAmt.Value = totalCharges;

        }

        protected void UpdateGstForCharges(string data)
        {
            //for (int i = 0; i < cmbGstCstVatcharge.Items.Count; i++)
            //{
            //    if (Convert.ToString(cmbGstCstVatcharge.Items[i].Value).Split('~')[0] == data)
            //    {
            //        cmbGstCstVatcharge.Items[i].Selected = true;
            //        break;
            //    }
            //}
        }
        protected void taxUpdatePanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string performpara = e.Parameter;
            if (performpara.Split('~')[0] == "DelProdbySl")
            {
                DataTable MainTaxDataTable = (DataTable)Session["ImportStockreceiptFinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["ImportStockreceiptFinalTaxRecord"] = MainTaxDataTable;
                //  GetStock(Convert.ToString(performpara.Split('~')[2]));
                // DeleteWarehouse(Convert.ToString(performpara.Split('~')[1]));
                DataTable taxDetails = (DataTable)Session["POTaxDetails"];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["POTaxDetails"] = taxDetails;
                }
            }
            else if (performpara.Split('~')[0] == "DeleteAllTax")
            {
                CreateDataTaxTable();

                DataTable taxDetails = (DataTable)Session["POTaxDetails"];

                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["POTaxDetails"] = taxDetails;
                }
            }
            else
            {
                DataTable MainTaxDataTable = (DataTable)Session["ImportStockreceiptFinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["ImportStockreceiptFinalTaxRecord"] = MainTaxDataTable;

                DataTable taxDetails = (DataTable)Session["POTaxDetails"];

                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["POTaxDetails"] = taxDetails;
                }
            }
        }
        //public void GetStock(string strProductID)
        //{
        //    string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
        //    acpAvailableStock.JSProperties["cpstock"] = "0.00";

        //    try
        //    {
        //        DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + strProductID + ") as branchopenstock");

        //        if (dt2.Rows.Count > 0)
        //        {
        //            taxUpdatePanel.JSProperties["cpstock"] = Convert.ToString(Math.Round(Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]), 2));
        //        }
        //        else
        //        {
        //            taxUpdatePanel.JSProperties["cpstock"] = "0.00";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //public void DeleteWarehouse(string SrlNo)
        //{
        //    DataTable Warehousedt = new DataTable();
        //    if (Session["WarehouseData"] != null)
        //    {
        //        Warehousedt = (DataTable)Session["WarehouseData"];

        //        var rows = Warehousedt.Select(string.Format("Product_SrlNo ='{0}'", SrlNo));
        //        foreach (var row in rows)
        //        {
        //            row.Delete();
        //        }
        //        Warehousedt.AcceptChanges();

        //        Session["WarehouseData"] = Warehousedt;
        //    }
        //}
        public double ReCalculateTaxAmount(string slno, double amount)
        {
            DataTable MainTaxDataTable = (DataTable)Session["ImportStockreceiptFinalTaxRecord"];
            double totalSum = 0.0;
            //Get The Existing datatable
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "PopulateAllTax");
            DataTable TaxDt = proc.GetTable();

            DataRow[] filterRow = MainTaxDataTable.Select("SlNo=" + slno);

            if (filterRow.Length > 0)
            {
                foreach (DataRow dr in filterRow)
                {
                    if (Convert.ToString(dr["TaxCode"]) != "0")
                    {
                        DataRow[] taxrow = TaxDt.Select("Taxes_ID=" + dr["TaxCode"]);
                        if (taxrow.Length > 0)
                        {
                            if (taxrow[0]["TaxCalculateMethods"] == "A")
                            {
                                dr["Amount"] = (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                                totalSum += (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                            }
                            else
                            {
                                dr["Amount"] = (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                                totalSum -= (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                            }
                        }
                    }
                    else
                    {
                        DataRow[] taxrow = TaxDt.Select("Taxes_ID=" + dr["AltTaxCode"]);
                        if (taxrow.Length > 0)
                        {
                            if (taxrow[0]["TaxCalculateMethods"] == "A")
                            {
                                dr["Amount"] = (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                                totalSum += (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                            }
                            else
                            {
                                dr["Amount"] = (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                                totalSum -= (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                            }
                        }
                    }
                }

            }
            Session["ImportStockreceiptFinalTaxRecord"] = MainTaxDataTable;

            return totalSum;

        }

        public void PopulateGSTCSTVATCombo(string quoteDate)
        {
            string LastCompany = "";
            if (Convert.ToString(Session["LastCompany"]) != null)
            {
                LastCompany = Convert.ToString(Session["LastCompany"]);
            }
            //DataTable dt = new DataTable();
            //dt = objCRMSalesDtlBL.PopulateGSTCSTVATCombo();
            //DataTable DT = oDBEngine.GetDataTable("select cast(td.TaxRates_ID as varchar(5))+'~'+ cast (td.TaxRates_Rate as varchar(25)) 'Taxes_ID',td.TaxRatesSchemeName 'Taxes_Name',th.Taxes_Name as 'TaxCodeName' from Master_Taxes th inner join Config_TaxRates td on th.Taxes_ID=td.TaxRates_TaxCode where (td.TaxRates_Country=0 or td.TaxRates_Country=(select add_country from tbl_master_address where add_cntId ='" + Convert.ToString(Session["LastCompany"]) + "' ))  and th.Taxes_ApplicableFor in ('B','S') and th.TaxTypeCode in('G','V','C')");

            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "LoadGSTCSTVATCombo");
            proc.AddVarcharPara("@S_QuoteAdd_CompanyID", 10, Convert.ToString(LastCompany));
            proc.AddVarcharPara("@S_quoteDate", 10, quoteDate);
            DataTable DT = proc.GetTable();
            cmbGstCstVat.DataSource = DT;
            cmbGstCstVat.TextField = "Taxes_Name";
            cmbGstCstVat.ValueField = "Taxes_ID";
            cmbGstCstVat.DataBind();
        }
        public void CreateDataTaxTable()
        {
            DataTable TaxRecord = new DataTable();

            TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
            TaxRecord.Columns.Add("TaxCode", typeof(System.String));
            TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
            TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
            TaxRecord.Columns.Add("Amount", typeof(System.Decimal));
            TaxRecord.Columns.Add("AmountINR", typeof(System.Decimal));
            Session["ImportStockreceiptFinalTaxRecord"] = TaxRecord;
        }

        //public IEnumerable GetTaxCode()
        //{
        //    List<taxCode> TaxList = new List<taxCode>();
        //    BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //    // DataTable DT = objEngine.GetDataTable("select Taxes_ID,Taxes_Name from dbo.Master_Taxes");
        //    DataTable DT = objEngine.GetDataTable("select cast(th.Taxes_ID as varchar(5))+'~'+ cast (td.TaxRates_Rate as varchar(25)) 'Taxes_ID',th.Taxes_Name 'Taxes_Name' from Master_Taxes th inner join Config_TaxRates td on th.Taxes_ID=td.TaxRates_TaxCode where (td.TaxRates_Country=0 or td.TaxRates_Country=(select add_country from tbl_master_address where add_cntId ='" + Convert.ToString(Session["LastCompany"]) + "' ))  and th.Taxes_ApplicableFor in ('B','S')");


        //    for (int i = 0; i < DT.Rows.Count; i++)
        //    {
        //        taxCode tax = new taxCode();
        //        tax.Taxes_ID = Convert.ToString(DT.Rows[i]["Taxes_ID"]);
        //        tax.Taxes_Name = Convert.ToString(DT.Rows[i]["Taxes_Name"]);
        //        TaxList.Add(tax);
        //    }

        //    return TaxList;
        //}

        public string GetTaxName(int id)
        {
            string taxName = "";
            string[] arr = oDBEngine.GetFieldValue1("Master_taxes", "Taxes_Name", "Taxes_ID=" + Convert.ToString(id), 1);
            if (arr[0] != "n")
            {
                taxName = arr[0];
            }
            return taxName;
        }
        public DataSet GetQuotationTaxData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "ProductTaxDetails");
            proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["QuotationID"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetQuotationEditedTaxData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetailsList_Import");
            proc.AddVarcharPara("@Action", 500, "ProductEditedTaxDetailsForPo");
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["ImptStockReceiptID"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public double GetTotalTaxAmount(List<TaxDetails> tax)
        {
            double sum = 0;
            foreach (TaxDetails td in tax)
            {
                if (td.Taxes_Name.Substring(td.Taxes_Name.Length - 3, 3) == "(+)")
                    sum += td.Amount;
                else
                    sum -= td.Amount;

            }
            return sum;
        }
        protected void cgridTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string retMsg = "";
            if (e.Parameters.Split('~')[0] == "SaveGST")
            {
                DataTable TaxRecord = (DataTable)Session["ImportStockreceiptFinalTaxRecord"];
                int slNo = Convert.ToInt32(HdSerialNo.Value);
                //For GST/CST/VAT
                if (cmbGstCstVat.Value != null)
                {

                    DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='0'");
                    if (finalRow.Length > 0)
                    {
                        finalRow[0]["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
                        finalRow[0]["Amount"] = txtGstCstVat.Text;
                        finalRow[0]["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];

                    }
                    else
                    {
                        DataRow newRowGST = TaxRecord.NewRow();
                        newRowGST["slNo"] = slNo;
                        newRowGST["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
                        newRowGST["TaxCode"] = "0";
                        newRowGST["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];
                        newRowGST["Amount"] = txtGstCstVat.Text;
                        TaxRecord.Rows.Add(newRowGST);
                    }
                }
                //End Here

                aspxGridTax.JSProperties["cpUpdated"] = "";

                Session["ImportStockreceiptFinalTaxRecord"] = TaxRecord;
            }
            else
            {
                #region fetch All data For Tax

                DataTable taxDetail = new DataTable();
                DataTable MainTaxDataTable = (DataTable)Session["ImportStockreceiptFinalTaxRecord"];

                if (Convert.ToString(ddl_AmountAre.Value).Trim() != "4")
                {
                    ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderAcceptanceDetailsList_Import");
                    proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                    proc.AddVarcharPara("@ProductsID", 10, Convert.ToString(setCurrentProdCode.Value));
                    proc.AddVarcharPara("@S_quoteDate", 10, dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                    taxDetail = proc.GetTable();
                }
                else
                {
                    ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderAcceptanceDetailsList_Import");
                    proc.AddVarcharPara("@Action", 500, "LoadImportTaxDetails");
                    proc.AddVarcharPara("@S_quoteDate", 10, dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                    taxDetail = proc.GetTable();
                }

                //Get Company Gstin 09032017
                string CompInternalId = Convert.ToString(Session["LastCompany"]);
                string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);

                //Get BranchStateCode
                string BrancgStateCode = "", BranchGSTIN = "";

                //Changes 15-06-2018 Sudip Pal

                // DataTable BranchTable = oDBEngine.GetDataTable("select StateCode,branch_GSTIN   from tbl_master_branch branch inner join tbl_master_state st on branch.branch_state=st.id where branch_id=" + Convert.ToString(ddl_Branch.SelectedValue));

                DataTable BranchTable = oDBEngine.GetDataTable("select StateCode,branch_GSTIN   from tbl_master_branch branch inner join tbl_master_state st on branch.branch_state=st.id where branch_id=" + HDNbranch.Value);

                //Changes 15-06-2018 Sudip Pal

                if (BranchTable != null)
                {
                    BrancgStateCode = Convert.ToString(BranchTable.Rows[0][0]);
                    BranchGSTIN = Convert.ToString(BranchTable.Rows[0][1]);
                    if (BranchGSTIN.Trim() != "")
                    {
                        BrancgStateCode = BranchGSTIN.Substring(0, 2);
                    }
                }
                if (BranchGSTIN.Trim() == "")
                {
                    BrancgStateCode = compGstin[0].Substring(0, 2);
                }
                string VendorState = "";

                ProcedureExecute GetVendorGstin = new ProcedureExecute("prc_GstTaxDetails");
                GetVendorGstin.AddVarcharPara("@Action", 500, "GetVendorGSTINByBranch");
                GetVendorGstin.AddVarcharPara("@branchId", 10, Convert.ToString(ddl_Branch.SelectedValue));
                GetVendorGstin.AddVarcharPara("@entityId", 10, Convert.ToString(hdnCustomerId.Value));
                DataTable VendorGstin = GetVendorGstin.GetTable();

                if (VendorGstin.Rows.Count > 0)
                {
                    if (Convert.ToString(VendorGstin.Rows[0][0]).Trim() != "")
                    {
                        VendorState = Convert.ToString(VendorGstin.Rows[0][0]).Substring(0, 2);
                    }

                }
                #endregion
                if (Convert.ToString(ddl_AmountAre.Value).Trim() != "4")
                {
                    if (VendorState.Trim() != "" && BrancgStateCode != "")
                    {
                        if (compGstin.Length > 0)
                        {
                            if (BrancgStateCode == VendorState)
                            {
                                //Check if the state is in union territories then only UTGST will apply
                                //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU          Lakshadweep              PONDICHERRY
                                if (VendorState == "4" || VendorState == "26" || VendorState == "25" || VendorState == "7" || VendorState == "31" || VendorState == "34")
                                {
                                    foreach (DataRow dr in taxDetail.Rows)
                                    {
                                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                        {
                                            dr.Delete();
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (DataRow dr in taxDetail.Rows)
                                    {
                                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                        {
                                            dr.Delete();
                                        }
                                    }
                                }
                                taxDetail.AcceptChanges();
                            }
                            else
                            {
                                foreach (DataRow dr in taxDetail.Rows)
                                {
                                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                    {
                                        dr.Delete();
                                    }
                                }
                                taxDetail.AcceptChanges();

                            }

                        }
                    }
                    //If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
                    if ((compGstin[0].Trim() == "" && BranchGSTIN == "") || VendorState == "")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                            {
                                dr.Delete();
                            }
                        }
                        taxDetail.AcceptChanges();
                    }
                }
                else
                {
                    if ((compGstin[0].Trim() == "" && BranchGSTIN == "") || VendorState == "")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                        taxDetail.AcceptChanges();
                    }
                }

                int slNo = Convert.ToInt32(HdSerialNo.Value);
                //Get Gross Amount and Net Amount 
                decimal ProdGrossAmt = Convert.ToDecimal(HdProdGrossAmt.Value);
                decimal ProdNetAmt = Convert.ToDecimal(HdProdNetAmt.Value);
                List<TaxDetails> TaxDetailsDetails = new List<TaxDetails>();
                //Debjyoti 09032017
                decimal totalParcentage = 0;
                foreach (DataRow dr in taxDetail.Rows)
                {
                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                    {
                        totalParcentage += Convert.ToDecimal(dr["TaxRates_Rate"]);
                    }
                }
                if (e.Parameters.Split('~')[0] == "New")
                {
                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        TaxDetails obj = new TaxDetails();
                        obj.Taxes_ID = Convert.ToInt32(dr["Taxes_ID"]);
                        obj.taxCodeName = Convert.ToString(dr["taxCodeName"]);
                        obj.TaxField = Convert.ToString(dr["TaxRates_Rate"]);
                        obj.Amount = 0.0;

                        #region set calculated on
                        //Check Tax Applicable on and set to calculated on
                        if (Convert.ToString(dr["ApplicableOn"]) == "G")
                        {
                            obj.calCulatedOn = ProdGrossAmt;



                            ////Changes 18-06-2018 Sudip Pal
                            if (txt_Rate.Text == "0" || txt_Rate.Text == "0.0")
                            {
                                txt_Rate.Text = "1";

                            }
                            obj.calCulatedOnInr = obj.calCulatedOn * Convert.ToDecimal(txt_Rate.Text);
                            ////Changes 18-06-2018 Sudip Pal
                        }
                        else if (Convert.ToString(dr["ApplicableOn"]) == "N")
                        {
                            obj.calCulatedOn = ProdNetAmt;


                            ////Changes 18-06-2018 Sudip Pal
                            if (txt_Rate.Text == "0" || txt_Rate.Text == "0.0")
                            {
                                txt_Rate.Text = "1";

                            }
                            obj.calCulatedOnInr = obj.calCulatedOn * Convert.ToDecimal(txt_Rate.Text);
                            ////Changes 18-06-2018 Sudip Pal
                        }
                        else
                        {
                            obj.calCulatedOn = 0;
                        }
                        //End Here
                        #endregion

                        //Debjyoti 09032017
                        if (Convert.ToString(ddl_AmountAre.Value) == "2")
                        {
                            if (Convert.ToString(ddl_VatGstCst.Value) == "0~0~X")
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                {
                                    decimal finalCalCulatedOn = 0;
                                    decimal backProcessRate = (1 + (totalParcentage / 100));
                                    finalCalCulatedOn = obj.calCulatedOn / backProcessRate;
                                    //obj.calCulatedOn = Math.Round(finalCalCulatedOn);
                                    obj.calCulatedOn = finalCalCulatedOn;

                                    ////Changes 18-06-2018 Sudip Pal
                                    if (txt_Rate.Text == "0" || txt_Rate.Text == "0.0")
                                    {
                                        txt_Rate.Text = "1";

                                    }
                                    obj.calCulatedOnInr = obj.calCulatedOn * Convert.ToDecimal(txt_Rate.Text);
                                    ////Changes 18-06-2018 Sudip Pal
                                }
                            }
                        }

                        if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
                        {
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";

                        }
                        else
                        {
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
                        }


                        obj.Amount = Convert.ToDouble(obj.calCulatedOn * (Convert.ToDecimal(obj.TaxField) / 100));

                        ////Changes 18-06-2018 Sudip Pal
                        //obj.AmountInr = Convert.ToDouble(obj.calCulatedOnInr * (Convert.ToDecimal(obj.TaxField) / 100));

                        if (txt_Rate.Text == "0" || txt_Rate.Text == "0.0")
                        {
                            txt_Rate.Text = "1";

                        }
                        obj.AmountInr = Convert.ToDouble(Convert.ToDecimal(obj.Amount) * Convert.ToDecimal(txt_Rate.Text));


                        ////Changes 18-06-2018 Sudip Pal



                        DataRow[] filtr = MainTaxDataTable.Select("TaxCode ='" + obj.Taxes_ID + "' and slNo=" + Convert.ToString(slNo));
                        if (filtr.Length > 0)
                        {
                            obj.Amount = Convert.ToDouble(filtr[0]["Amount"]);

                            ////Changes 18-06-2018 Sudip Pal
                            if (txt_Rate.Text == "0" || txt_Rate.Text == "0.0")
                            {
                                txt_Rate.Text = "1";

                            }
                            obj.AmountInr = Convert.ToDouble(Convert.ToDecimal(obj.Amount) * Convert.ToDecimal(txt_Rate.Text));

                            ////Changes 18-06-2018 Sudip Pal


                            if (obj.Taxes_ID == 0)
                            {
                                aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(filtr[0]["AltTaxCode"]);
                            }
                            else
                                obj.TaxField = Convert.ToString(filtr[0]["Percentage"]);
                        }

                        TaxDetailsDetails.Add(obj);
                    }
                }
                else
                {
                    string keyValue = e.Parameters.Split('~')[0];
                    DataTable TaxRecord = (DataTable)Session["ImportStockreceiptFinalTaxRecord"];
                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        TaxDetails obj = new TaxDetails();
                        obj.Taxes_ID = Convert.ToInt32(dr["Taxes_ID"]);
                        obj.taxCodeName = Convert.ToString(dr["taxCodeName"]);

                        if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";
                        else
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
                        obj.TaxField = "";
                        obj.Amount = 0.0;

                        #region set calculated on
                        //Check Tax Applicable on and set to calculated on
                        if (Convert.ToString(dr["ApplicableOn"]) == "G")
                        {
                            obj.calCulatedOn = ProdGrossAmt;

                            ////Changes 18-06-2018 Sudip Pal
                            if (txt_Rate.Text == "0" || txt_Rate.Text == "0.0")
                            {
                                txt_Rate.Text = "1";

                            }
                            obj.calCulatedOnInr = obj.calCulatedOn * Convert.ToDecimal(txt_Rate.Text);
                            ////Changes 18-06-2018 Sudip Pal
                        }
                        else if (Convert.ToString(dr["ApplicableOn"]) == "N")
                        {
                            obj.calCulatedOn = ProdNetAmt;

                            ////Changes 18-06-2018 Sudip Pal
                            if (txt_Rate.Text == "0" || txt_Rate.Text == "0.0")
                            {
                                txt_Rate.Text = "1";

                            }
                            obj.calCulatedOnInr = obj.calCulatedOn * Convert.ToDecimal(txt_Rate.Text);
                            ////Changes 18-06-2018 Sudip Pal
                        }
                        else
                        {
                            obj.calCulatedOn = 0;
                        }
                        //End Here
                        #endregion

                        //Debjyoti 09032017
                        if (Convert.ToString(ddl_AmountAre.Value) == "2")
                        {
                            if (Convert.ToString(ddl_VatGstCst.Value) == "0~0~X")
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                {
                                    decimal finalCalCulatedOn = 0;
                                    decimal backProcessRate = (1 + (totalParcentage / 100));
                                    finalCalCulatedOn = obj.calCulatedOn / backProcessRate;
                                    //obj.calCulatedOn = Math.Round(finalCalCulatedOn);
                                    obj.calCulatedOn = finalCalCulatedOn;

                                    ////Changes 18-06-2018 Sudip Pal
                                    if (txt_Rate.Text == "0" || txt_Rate.Text == "0.0")
                                    {
                                        txt_Rate.Text = "1";

                                    }
                                    obj.calCulatedOnInr = obj.calCulatedOn * Convert.ToDecimal(txt_Rate.Text);
                                    ////Changes 18-06-2018 Sudip Pal

                                }
                            }
                        }
                        DataRow[] filtronexsisting1 = TaxRecord.Select("TaxCode=" + obj.Taxes_ID + " and SlNo=" + Convert.ToString(slNo));
                        if (filtronexsisting1.Length > 0)
                        {
                            if (obj.Taxes_ID == 0)
                            {
                                obj.TaxField = "0";
                            }
                            else
                            {
                                obj.TaxField = Convert.ToString(filtronexsisting1[0]["Percentage"]);
                            }
                            obj.Amount = Convert.ToDouble(filtronexsisting1[0]["Amount"]);

                            ////Changes 18-06-2018 Sudip Pal
                            if (txt_Rate.Text == "0" || txt_Rate.Text == "0.0")
                            {
                                txt_Rate.Text = "1";

                            }
                            obj.AmountInr = Convert.ToDouble(Convert.ToDecimal(obj.Amount) * Convert.ToDecimal(txt_Rate.Text));

                            ////Changes 18-06-2018 Sudip Pal

                        }
                        else
                        {
                            #region checkingFordb







                            #endregion
                            DataRow[] filtronexsisting = TaxRecord.Select("TaxCode=" + obj.Taxes_ID + " and SlNo=" + Convert.ToString(slNo));
                            if (filtronexsisting.Length > 0)
                            {
                                DataRow taxRecordNewRow = TaxRecord.NewRow();
                                taxRecordNewRow["SlNo"] = slNo;
                                taxRecordNewRow["TaxCode"] = obj.Taxes_ID;
                                taxRecordNewRow["AltTaxCode"] = "0";
                                taxRecordNewRow["Percentage"] = 0.0;
                                taxRecordNewRow["Amount"] = "0";

                                TaxRecord.Rows.Add(taxRecordNewRow);
                            }

                        }
                        TaxDetailsDetails.Add(obj);
                        DataRow[] filtrIndex = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode=0");
                        if (filtrIndex.Length > 0)
                        {
                            aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(filtrIndex[0]["AltTaxCode"]);
                        }
                    }
                    Session["ImportStockreceiptFinalTaxRecord"] = TaxRecord;
                }
                //New Changes 170217
                //GstCode should fetch everytime
                DataRow[] finalFiltrIndex = MainTaxDataTable.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode=0");
                if (finalFiltrIndex.Length > 0)
                {
                    aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(finalFiltrIndex[0]["AltTaxCode"]);
                }
                aspxGridTax.JSProperties["cpJsonData"] = createJsonForDetails(taxDetail);
                for (var i = 0; i < TaxDetailsDetails.Count; i++)
                {
                    decimal _Amount = Convert.ToDecimal(TaxDetailsDetails[i].Amount);
                    decimal _calCulatedOn = Convert.ToDecimal(TaxDetailsDetails[i].calCulatedOn);

                    TaxDetailsDetails[i].Amount = GetRoundOfValue(_Amount);

                    ////Changes 18-06-2018 Sudip Pal
                    TaxDetailsDetails[i].AmountInr = GetRoundOfValue(_Amount) * Convert.ToDouble(txt_Rate.Text);
                    ////Changes 18-06-2018 Sudip Pal
                    TaxDetailsDetails[i].calCulatedOn = Convert.ToDecimal(GetRoundOfValue(_calCulatedOn));


                    ////Changes 18-06-2018 Sudip Pal
                    if (txt_Rate.Text == "0" || txt_Rate.Text == "0.0")
                    {
                        txt_Rate.Text = "1";

                    }
                    TaxDetailsDetails[i].calCulatedOnInr = TaxDetailsDetails[i].calCulatedOn * Convert.ToDecimal(txt_Rate.Text);
                    ////Changes 18-06-2018 Sudip Pal

                }

                retMsg = Convert.ToString(GetTotalTaxAmount(TaxDetailsDetails));
                aspxGridTax.JSProperties["cpUpdated"] = "ok~" + retMsg;
                TaxDetailsDetails = setCalculatedOn(TaxDetailsDetails, taxDetail);
                aspxGridTax.DataSource = TaxDetailsDetails;
                aspxGridTax.DataBind();

        #endregion

            }
        }
        public double GetRoundOfValue(decimal Value)
        {
            double RoundOfValue = 0;

            if (Value > 0)
            {
                decimal _Value = Convert.ToDecimal(Value);
                _Value = Math.Round(Value, 2);

                RoundOfValue = Convert.ToDouble(_Value);
            }

            return RoundOfValue;
        }
        #region  Tax Details
        public string createJsonForDetails(DataTable lstTaxDetails)
        {
            List<TaxSetailsJson> jsonList = new List<TaxSetailsJson>();
            TaxSetailsJson jsonObj;
            int visIndex = 0;
            foreach (DataRow taxObj in lstTaxDetails.Rows)
            {
                jsonObj = new TaxSetailsJson();

                jsonObj.SchemeName = Convert.ToString(taxObj["Taxes_Name"]);
                jsonObj.vissibleIndex = visIndex;
                jsonObj.applicableOn = Convert.ToString(taxObj["ApplicableOn"]);
                if (jsonObj.applicableOn == "G" || jsonObj.applicableOn == "N")
                {
                    jsonObj.applicableBy = "None";
                }
                else
                {
                    jsonObj.applicableBy = Convert.ToString(taxObj["dependOn"]);
                }
                visIndex++;
                jsonList.Add(jsonObj);
            }

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return oSerializer.Serialize(jsonList);
        }
        public List<TaxDetails> setCalculatedOn(List<TaxDetails> gridSource, DataTable taxDt)
        {
            foreach (TaxDetails taxObj in gridSource)
            {
                DataRow[] dependOn = taxDt.Select("dependOn='" + taxObj.Taxes_Name.Replace("(+)", "").Replace("(-)", "") + "'");
                if (dependOn.Length > 0)
                {
                    foreach (DataRow dr in dependOn)
                    {
                        //  List<TaxDetails> dependObj = gridSource.Where(r => r.Taxes_Name.Replace("(+)", "").Replace("(-)", "") == Convert.ToString(dependOn[0]["Taxes_Name"])).ToList();
                        foreach (var setCalObj in gridSource.Where(r => r.Taxes_Name.Replace("(+)", "").Replace("(-)", "") == Convert.ToString(dependOn[0]["Taxes_Name"])))
                        {
                            setCalObj.calCulatedOn = Convert.ToDecimal(taxObj.Amount);
                        }
                    }

                }

            }
            return gridSource;
        }
        protected void aspxGridTax_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {

            if (e.Column.FieldName == "Taxes_Name")
            {
                e.Editor.ReadOnly = true;
            }
            if (e.Column.FieldName == "taxCodeName")
            {
                e.Editor.ReadOnly = true;
            }
            if (e.Column.FieldName == "calCulatedOn")
            {
                e.Editor.ReadOnly = true;
            }
            //else if (e.Column.FieldName == "Amount")
            //{
            //    e.Editor.ReadOnly = true;
            //}
            else
            {
                e.Editor.ReadOnly = false;
            }
        }
        protected void aspxGridTax_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {

        }
        protected void taxgrid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {

            int slNo = Convert.ToInt32(HdSerialNo.Value);
            DataTable TaxRecord = (DataTable)Session["ImportStockreceiptFinalTaxRecord"];
            foreach (var args in e.UpdateValues)
            {

                string TaxCodeDes = Convert.ToString(args.NewValues["Taxes_Name"]);
                decimal Percentage = 0;

                Percentage = Convert.ToDecimal(args.NewValues["TaxField"]);

                decimal Amount = Convert.ToDecimal(args.NewValues["Amount"]);
                decimal AmountINR = Convert.ToDecimal(args.NewValues["AmountInr"]);
                string TaxCode = "0";
                if (!Convert.ToString(args.Keys[0]).Contains('~'))
                {
                    TaxCode = Convert.ToString(args.Keys[0]);
                }



                DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='" + TaxCode + "'");
                if (finalRow.Length > 0)
                {
                    finalRow[0]["Percentage"] = Percentage;
                    finalRow[0]["Amount"] = Amount;
                    finalRow[0]["TaxCode"] = args.Keys[0];
                    finalRow[0]["AltTaxCode"] = "0";
                    finalRow[0]["AmountINR"] = AmountINR;
                }
                else
                {
                    DataRow newRow = TaxRecord.NewRow();
                    newRow["slNo"] = slNo;
                    newRow["Percentage"] = Percentage;
                    newRow["TaxCode"] = TaxCode;
                    newRow["AltTaxCode"] = "0";
                    newRow["Amount"] = Amount;
                    newRow["AmountINR"] = AmountINR;
                    TaxRecord.Rows.Add(newRow);
                }


            }

            //For GST/CST/VAT
            //if (cmbGstCstVat.Value != null)
            //{

            //    DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='0'");
            //    if (finalRow.Length > 0)
            //    {
            //        finalRow[0]["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
            //        finalRow[0]["Amount"] = txtGstCstVat.Text;
            //        finalRow[0]["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];

            //    }
            //    else
            //    {
            //        DataRow newRowGST = TaxRecord.NewRow();
            //        newRowGST["slNo"] = slNo;
            //        newRowGST["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
            //        newRowGST["TaxCode"] = "0";
            //        newRowGST["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];
            //        newRowGST["Amount"] = txtGstCstVat.Text;
            //        TaxRecord.Rows.Add(newRowGST);
            //    }
            //}
            //End Here


            Session["ImportStockreceiptFinalTaxRecord"] = TaxRecord;


            #region oldpart


            //DataTable taxdtByProductCode = new DataTable();
            //if (Session["ProdTax" + "_" + Convert.ToString(HdSerialNo.Value)] == null)
            //{

            //    taxdtByProductCode.TableName = "ProdTax"  + "_" + Convert.ToString(HdSerialNo.Value);


            //    taxdtByProductCode.Columns.Add("TaxCode", typeof(System.String));
            //    taxdtByProductCode.Columns.Add("TaxCodeDescription", typeof(System.String));
            //    taxdtByProductCode.Columns.Add("Percentage", typeof(System.Decimal));
            //    taxdtByProductCode.Columns.Add("Amount", typeof(System.Decimal));
            //    DataRow dr;
            //    foreach (var args in e.UpdateValues)
            //    {
            //        dr = taxdtByProductCode.NewRow();
            //        string TaxCodeDes = Convert.ToString(args.NewValues["Taxes_Name"]);
            //        decimal Percentage = 0;
            //        if (TaxCodeDes == "GST/CST/VAT")
            //        {
            //            Percentage = Convert.ToDecimal(Convert.ToString(args.NewValues["TaxField"]).Split('~')[1]);
            //        }
            //        else
            //        {
            //            Percentage = Convert.ToDecimal(args.NewValues["TaxField"]);
            //        }
            //        decimal Amount = Convert.ToDecimal(args.NewValues["Amount"]);

            //        dr["TaxCodeDescription"] = TaxCodeDes;
            //        if (Convert.ToString(args.Keys[0]) == "0")
            //        {
            //            dr["TaxCode"] = "0~" + Convert.ToString(args.NewValues["TaxField"]).Split('~')[0];
            //        }
            //        else
            //        {
            //            dr["TaxCode"] = args.Keys[0];
            //        }
            //        dr["Percentage"] = Percentage;
            //        dr["Amount"] = Amount;

            //        taxdtByProductCode.Rows.Add(dr);
            //    }
            //}
            //else
            //{
            //    taxdtByProductCode = (DataTable)Session["ProdTax"  +"_"+ Convert.ToString(HdSerialNo.Value)];

            //    foreach (var args in e.UpdateValues)
            //    {
            //        DataRow[] filtr ;

            //        if (Convert.ToString(args.Keys[0]) == "0")
            //        {
            //            filtr = taxdtByProductCode.Select("TaxCode like '%0~%'"); ;
            //        }
            //        else
            //        {
            //            filtr = taxdtByProductCode.Select("TaxCode='" + args.Keys[0]+"'");
            //        }

            //        if (filtr.Length > 0)
            //        {
            //            string TaxCodeDes = Convert.ToString(args.NewValues["Taxes_Name"]);
            //        filtr[0]["TaxCodeDescription"] = TaxCodeDes;
            //        if (Convert.ToString(args.Keys[0]) == "0")
            //        {
            //            filtr[0]["TaxCode"] = "0~" + Convert.ToString(args.NewValues["TaxField"]).Split('~')[0];
            //        }
            //        else
            //        {
            //            filtr[0]["TaxCode"] = args.Keys[0];
            //        }

            //        decimal Percentage = 0;
            //        if (TaxCodeDes == "GST/CST/VAT")
            //        {
            //            Percentage = Convert.ToDecimal(Convert.ToString(args.NewValues["TaxField"]).Split('~')[1]);
            //        }
            //        else
            //        {
            //            Percentage = Convert.ToDecimal(args.NewValues["TaxField"]);
            //        }
            //        decimal Amount = Convert.ToDecimal(args.NewValues["Amount"]);
            //        filtr[0]["Percentage"] = Percentage;
            //        filtr[0]["Amount"] = Amount;

            //        }
            //    }


            //}

            #endregion
            //  Session[taxdtByProductCode.TableName] = taxdtByProductCode;

        }
        protected void cmbGstCstVat_Callback(object sender, CallbackEventArgsBase e)
        {
            DateTime quoteDate = Convert.ToDateTime(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
            PopulateGSTCSTVATCombo(quoteDate.ToString("yyyy-MM-dd"));
            CreateDataTaxTable();

        }
        protected void cmbGstCstVatcharge_Callback(object sender, CallbackEventArgsBase e)
        {
            Session["POTaxDetails"] = null;
            DateTime quoteDate = Convert.ToDateTime(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
            PopulateChargeGSTCSTVATCombo(quoteDate.ToString("yyyy-MM-dd"));
        }
        public DataTable getAllTaxDetails(DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable FinalTable = new DataTable();
            FinalTable.Columns.Add("SlNo", typeof(System.Int32));
            FinalTable.Columns.Add("TaxCode", typeof(System.String));
            FinalTable.Columns.Add("AltTaxCode", typeof(System.String));
            FinalTable.Columns.Add("Percentage", typeof(System.Decimal));
            FinalTable.Columns.Add("Amount", typeof(System.Decimal));

            //for insert
            foreach (var args in e.InsertValues)
            {
                string Slno = Convert.ToString(args.NewValues["SrlNo"]);
                DataRow existsRow;
                if (Session["ProdTax_" + Slno] != null)
                {
                    DataTable sessiontable = (DataTable)Session["ProdTax_" + Slno];
                    foreach (DataRow dr in sessiontable.Rows)
                    {
                        existsRow = FinalTable.NewRow();

                        existsRow["SlNo"] = Slno;
                        if (Convert.ToString(dr["taxCode"]).Contains("~"))
                        {
                            existsRow["TaxCode"] = "0";
                            existsRow["AltTaxCode"] = Convert.ToString(dr["taxCode"]).Split('~')[1];
                        }
                        else
                        {
                            existsRow["TaxCode"] = Convert.ToString(dr["taxCode"]);
                            existsRow["AltTaxCode"] = "0";
                        }

                        existsRow["Percentage"] = Convert.ToString(dr["Percentage"]);
                        existsRow["Amount"] = Convert.ToString(dr["Amount"]);

                        FinalTable.Rows.Add(existsRow);
                    }
                    Session.Remove("ProdTax_" + Slno);
                }
            }

            return FinalTable;
        }
        protected void taxgrid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void taxgrid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void taxgrid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        public void DeleteTaxDetails(string SrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["ImportStockreceiptFinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["ImportStockreceiptFinalTaxRecord"];

                var rows = TaxDetailTable.Select("SlNo ='" + SrlNo + "'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                TaxDetailTable.AcceptChanges();

                Session["ImportStockreceiptFinalTaxRecord"] = TaxDetailTable;
            }
        }
        public void UpdateTaxDetails(string oldSrlNo, string newSrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["ImportStockreceiptFinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["ImportStockreceiptFinalTaxRecord"];

                for (int i = 0; i < TaxDetailTable.Rows.Count; i++)
                {
                    DataRow dr = TaxDetailTable.Rows[i];
                    string Product_SrlNo = Convert.ToString(dr["SlNo"]);
                    if (oldSrlNo == Product_SrlNo)
                    {
                        dr["SlNo"] = newSrlNo;
                    }
                }
                TaxDetailTable.AcceptChanges();

                Session["ImportStockreceiptFinalTaxRecord"] = TaxDetailTable;
            }
        }
        public string createJsonForChargesTax(DataTable lstTaxDetails)
        {
            List<TaxSetailsJson> jsonList = new List<TaxSetailsJson>();
            TaxSetailsJson jsonObj;
            int visIndex = 0;
            foreach (DataRow taxObj in lstTaxDetails.Rows)
            {
                jsonObj = new TaxSetailsJson();

                jsonObj.SchemeName = Convert.ToString(taxObj["Taxes_Name"]);
                jsonObj.vissibleIndex = visIndex;
                jsonObj.applicableOn = Convert.ToString(taxObj["ApplicableOn"]);
                if (jsonObj.applicableOn == "G" || jsonObj.applicableOn == "N")
                {
                    jsonObj.applicableBy = "None";
                }
                else
                {
                    jsonObj.applicableBy = Convert.ToString(taxObj["dependOn"]);
                }
                visIndex++;
                jsonList.Add(jsonObj);
            }

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return oSerializer.Serialize(jsonList);
        }
        public List<Taxes> setChargeCalculatedOn(List<Taxes> gridSource, DataTable taxDt)
        {
            foreach (Taxes taxObj in gridSource)
            {
                DataRow[] dependOn = taxDt.Select("dependOn='" + taxObj.TaxName.Replace("(+)", "").Replace("(-)", "").Trim() + "'");
                if (dependOn.Length > 0)
                {
                    foreach (DataRow dr in dependOn)
                    {
                        //  List<TaxDetails> dependObj = gridSource.Where(r => r.Taxes_Name.Replace("(+)", "").Replace("(-)", "") == Convert.ToString(dependOn[0]["Taxes_Name"])).ToList();
                        foreach (var setCalObj in gridSource.Where(r => r.TaxName.Replace("(+)", "").Replace("(-)", "").Trim() == Convert.ToString(dependOn[0]["Taxes_Name"]).Replace("(+)", "").Replace("(-)", "").Trim()))
                        {
                            setCalObj.calCulatedOn = Convert.ToDecimal(taxObj.Amount);
                        }
                    }

                }

            }
            return gridSource;
        }
        public void PopulateChargeGSTCSTVATCombo(string quoteDate)
        {
            string LastCompany = "";
            if (Convert.ToString(Session["LastCompany"]) != null)
            {
                LastCompany = Convert.ToString(Session["LastCompany"]);
            }
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "LoadChargeGSTCSTVATCombo");
            proc.AddVarcharPara("@S_QuoteAdd_CompanyID", 10, Convert.ToString(LastCompany));
            proc.AddVarcharPara("@S_quoteDate", 10, quoteDate);
            DataTable DT = proc.GetTable();
            cmbGstCstVatcharge.DataSource = DT;
            cmbGstCstVatcharge.TextField = "Taxes_Name";
            cmbGstCstVatcharge.ValueField = "Taxes_ID";
            cmbGstCstVatcharge.DataBind();
        }

        #endregion
        #region Inline Tax Details

        public IEnumerable GetTaxes()
        {
            List<Taxes> TaxList = new List<Taxes>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Taxes Taxes = new Taxes();
                Taxes.TaxID = Convert.ToString(DT.Rows[i]["Taxes_ID"]);
                Taxes.TaxName = Convert.ToString(DT.Rows[i]["Taxes_Name"]);
                Taxes.Percentage = Convert.ToString(DT.Rows[i]["Percentage"]);
                Taxes.Amount = Convert.ToString(DT.Rows[i]["Amount"]);
                TaxList.Add(Taxes);
            }

            return TaxList;
        }
        public IEnumerable GetTaxes(DataTable DT)
        {
            List<Taxes> TaxList = new List<Taxes>();

            decimal totalParcentage = 0;
            foreach (DataRow dr in DT.Rows)
            {
                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                {
                    totalParcentage += Convert.ToDecimal(dr["Percentage"]);
                }
            }




            for (int i = 0; i < DT.Rows.Count; i++)
            {
                if (Convert.ToString(DT.Rows[i]["Taxes_ID"]) != "0")
                {
                    Taxes Taxes = new Taxes();
                    Taxes.TaxID = Convert.ToString(DT.Rows[i]["Taxes_ID"]);
                    Taxes.TaxName = Convert.ToString(DT.Rows[i]["Taxes_Name"]);
                    Taxes.Percentage = Convert.ToString(DT.Rows[i]["Percentage"]);
                    Taxes.Amount = Convert.ToString(DT.Rows[i]["Amount"]);
                    if (Convert.ToString(DT.Rows[i]["ApplicableOn"]) == "G")
                    {
                        Taxes.calCulatedOn = Convert.ToDecimal(HdChargeProdAmt.Value);
                    }
                    else if (Convert.ToString(DT.Rows[i]["ApplicableOn"]) == "N")
                    {
                        Taxes.calCulatedOn = Convert.ToDecimal(HdChargeProdNetAmt.Value);
                    }
                    else
                    {
                        Taxes.calCulatedOn = 0;
                    }

                    //Set Amount Value 
                    if (Taxes.Amount == "0.00")
                    {
                        Taxes.Amount = Convert.ToString(Taxes.calCulatedOn * (Convert.ToDecimal(Taxes.Percentage) / 100));
                    }


                    if (Convert.ToString(ddl_AmountAre.Value) == "2")
                    {
                        if (Convert.ToString(ddl_VatGstCst.Value) == "0~0~X")
                        {
                            if (Convert.ToString(DT.Rows[i]["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(DT.Rows[i]["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(DT.Rows[i]["TaxTypeCode"]).Trim() == "SGST")
                            {
                                decimal finalCalCulatedOn = 0;
                                decimal backProcessRate = (1 + (totalParcentage / 100));
                                finalCalCulatedOn = Taxes.calCulatedOn / backProcessRate;
                                Taxes.calCulatedOn = Math.Round(finalCalCulatedOn);
                            }
                        }
                    }

                    TaxList.Add(Taxes);
                }
            }

            return TaxList;
        }
        public DataTable GetTaxData(string quoteDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList_Import");
            proc.AddVarcharPara("@Action", 500, "TaxDetails");
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["ImptStockReceiptID"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"])); 
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@S_quoteDate", 10, quoteDate);
            dt = proc.GetTable();
            return dt;
        }
        protected void gridTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "Display")
            {
                DataTable TaxDetailsdt = new DataTable();
                if (Session["POTaxDetails"] == null)
                {
                    Session["POTaxDetails"] = GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                }

                if (Session["POTaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["POTaxDetails"];


                    #region Delete Igst,Cgst,Sgst respectively
                    //Get Company Gstin 09032017
                    string CompInternalId = Convert.ToString(Session["LastCompany"]);
                    string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);
                    string ShippingState = "";
                    #region ##### Added By : Samrat Roy -- For BillingShippingUserControl ######
                    string sstateCode = Purchase_BillingShipping.GeteShippingStateCode();
                    ShippingState = sstateCode;
                    if (ShippingState.Trim() != "")
                    {
                        ShippingState = ShippingState;
                    }
                    #region ##### Old Code -- BillingShipping ######
                    ////if (chkBilling.Checked)
                    ////{
                    ////    if (CmbState.Value != null)
                    ////    {
                    ////        ShippingState = CmbState.Text;
                    ////        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    ////    }
                    ////}
                    ////else
                    ////{
                    ////    if (CmbState1.Value != null)
                    ////    {
                    ////        ShippingState = CmbState1.Text;
                    ////        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    ////    }
                    ////}
                    #endregion

                    #endregion


                    if (ShippingState.Trim() != "" && compGstin[0].Trim() != "")
                    {
                        if (compGstin.Length > 0)
                        {
                            if (compGstin[0].Substring(0, 2) == ShippingState)
                            {
                                //Check if the state is in union territories then only UTGST will apply
                                //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU      Lakshadweep              PONDICHERRY
                                if (ShippingState == "4" || ShippingState == "26" || ShippingState == "25" || ShippingState == "35" || ShippingState == "31" || ShippingState == "34")
                                {
                                    foreach (DataRow dr in TaxDetailsdt.Rows)
                                    {
                                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                        {
                                            dr.Delete();
                                        }
                                    }

                                }
                                else
                                {
                                    foreach (DataRow dr in TaxDetailsdt.Rows)
                                    {
                                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                        {
                                            dr.Delete();
                                        }
                                    }
                                }
                                TaxDetailsdt.AcceptChanges();
                            }
                            else
                            {
                                foreach (DataRow dr in TaxDetailsdt.Rows)
                                {
                                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                    {
                                        dr.Delete();
                                    }
                                }
                                TaxDetailsdt.AcceptChanges();

                            }

                        }
                    }

                    //If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
                    if (compGstin[0].Trim() == "")
                    {
                        foreach (DataRow dr in TaxDetailsdt.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                            {
                                dr.Delete();
                            }
                        }
                        TaxDetailsdt.AcceptChanges();
                    }

                    #endregion

                    //gridTax.DataSource = GetTaxes();
                    var taxlist = (List<Taxes>)GetTaxes(TaxDetailsdt);
                    var taxChargeDataSource = setChargeCalculatedOn(taxlist, TaxDetailsdt);
                    gridTax.DataSource = taxChargeDataSource;
                    gridTax.DataBind();
                    gridTax.JSProperties["cpJsonChargeData"] = createJsonForChargesTax(TaxDetailsdt);
                }
            }
            else if (strSplitCommand == "SaveGst")
            {
                DataTable TaxDetailsdt = new DataTable();
                if (Session["POTaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["POTaxDetails"];
                }
                else
                {
                    TaxDetailsdt.Columns.Add("Taxes_ID", typeof(string));
                    TaxDetailsdt.Columns.Add("Taxes_Name", typeof(string));
                    TaxDetailsdt.Columns.Add("Percentage", typeof(string));
                    TaxDetailsdt.Columns.Add("Amount", typeof(string));
                    //ForGst
                    TaxDetailsdt.Columns.Add("AltTax_Code", typeof(string));
                }
                DataRow[] existingRow = TaxDetailsdt.Select("Taxes_ID='0'");
                if (Convert.ToString(cmbGstCstVatcharge.Value) == "0")
                {
                    if (existingRow.Length > 0)
                    {
                        TaxDetailsdt.Rows.Remove(existingRow[0]);
                    }
                }
                else
                {
                    if (existingRow.Length > 0)
                    {

                        existingRow[0]["Percentage"] = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1];
                        existingRow[0]["Amount"] = txtGstCstVatCharge.Text;
                        existingRow[0]["AltTax_Code"] = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0]; ;

                    }
                    else
                    {
                        string GstTaxId = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0];
                        string GstPerCentage = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1];
                        string GstAmount = txtGstCstVatCharge.Text;
                        DataRow gstRow = TaxDetailsdt.NewRow();
                        gstRow["Taxes_ID"] = 0;
                        gstRow["Taxes_Name"] = cmbGstCstVatcharge.Text;
                        gstRow["Percentage"] = GstPerCentage;
                        gstRow["Amount"] = GstAmount;
                        gstRow["AltTax_Code"] = GstTaxId;
                        TaxDetailsdt.Rows.Add(gstRow);
                    }

                    Session["POTaxDetails"] = TaxDetailsdt;
                }
            }
        }
        protected void gridTax_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void gridTax_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void gridTax_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void gridTax_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable TaxDetailsdt = new DataTable();
            if (Session["POTaxDetails"] != null)
            {
                TaxDetailsdt = (DataTable)Session["POTaxDetails"];
            }
            else
            {
                TaxDetailsdt.Columns.Add("Taxes_ID", typeof(string));
                TaxDetailsdt.Columns.Add("Taxes_Name", typeof(string));
                TaxDetailsdt.Columns.Add("Percentage", typeof(string));
                TaxDetailsdt.Columns.Add("Amount", typeof(string));
                //ForGst
                TaxDetailsdt.Columns.Add("AltTax_Code", typeof(string));
            }

            foreach (var args in e.UpdateValues)
            {
                string TaxID = Convert.ToString(args.Keys["TaxID"]);
                string TaxName = Convert.ToString(args.NewValues["TaxName"]);
                string Percentage = Convert.ToString(args.NewValues["Percentage"]);
                string Amount = Convert.ToString(args.NewValues["Amount"]);

                bool Isexists = false;
                foreach (DataRow drr in TaxDetailsdt.Rows)
                {
                    string OldTaxID = Convert.ToString(drr["Taxes_ID"]);

                    if (OldTaxID == TaxID)
                    {
                        Isexists = true;

                        drr["Percentage"] = Percentage;
                        drr["Amount"] = Amount;

                        break;
                    }
                }

                if (Isexists == false)
                {
                    TaxDetailsdt.Rows.Add(TaxID, TaxName, Percentage, Amount, 0);
                }
            }

            if (cmbGstCstVatcharge.Value != null)
            {
                DataRow[] existingRow = TaxDetailsdt.Select("Taxes_ID='0'");
                if (Convert.ToString(cmbGstCstVatcharge.Value) == "0")
                {
                    if (existingRow.Length > 0)
                    {
                        TaxDetailsdt.Rows.Remove(existingRow[0]);
                    }
                }
                else
                {
                    if (existingRow.Length > 0)
                    {

                        existingRow[0]["Percentage"] = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1];
                        existingRow[0]["Amount"] = txtGstCstVatCharge.Text;
                        existingRow[0]["AltTax_Code"] = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0]; ;

                    }
                    else
                    {
                        string GstTaxId = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0];
                        string GstPerCentage = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1];
                        string GstAmount = txtGstCstVatCharge.Text;
                        DataRow gstRow = TaxDetailsdt.NewRow();
                        gstRow["Taxes_ID"] = 0;
                        gstRow["Taxes_Name"] = cmbGstCstVatcharge.Text;
                        gstRow["Percentage"] = GstPerCentage;
                        gstRow["Amount"] = GstAmount;
                        gstRow["AltTax_Code"] = GstTaxId;
                        TaxDetailsdt.Rows.Add(gstRow);
                    }
                }
            }

            Session["POTaxDetails"] = TaxDetailsdt;

            gridTax.DataSource = GetTaxes(TaxDetailsdt);
            gridTax.DataBind();
        }
        protected void gridTax_DataBinding(object sender, EventArgs e)
        {
            if (Session["POTaxDetails"] != null)
            {
                DataTable TaxDetailsdt = (DataTable)Session["POTaxDetails"];

                //gridTax.DataSource = GetTaxes();
                var taxlist = (List<Taxes>)GetTaxes(TaxDetailsdt);
                var taxChargeDataSource = setChargeCalculatedOn(taxlist, TaxDetailsdt);
                gridTax.DataSource = taxChargeDataSource;
            }
        }


        #endregion
        #region   Indent/Requisition Number
        //protected void lookup_quotation_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["IndentRequiData"] != null)
        //    {
        //        lookup_quotation.DataSource = (DataTable)Session["IndentRequiData"];
        //    }
        //}

        protected void BindLookUp(string OrderDate, string BranchId, string vendor)
        {
            string status = string.Empty;
            //Subhabrata
            if (Convert.ToString(Request.QueryString["key"]) != "ADD")
            {
                status = "DONE";
            }
            else
            {
                status = "NOT-DONE";
            }//End            

            //string strBranch = Convert.ToString(Session["userbranchHierarchy"]);
            DataTable IndentTable = blLayer.GetBillEntry(OrderDate, status, BranchId, vendor);
            if (IndentTable.Rows.Count > 0)
            {
                taggingGrid.DataSource = IndentTable;
                taggingGrid.DataBind();
            }
            else
            {
                taggingGrid.DataSource = IndentTable;
                taggingGrid.DataBind();
            }
            Session["IndentRequiData"] = IndentTable;
        }

        protected void ComponentDatePanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "BindQuotationDate")
            {
                string Indent_No = Convert.ToString(e.Parameter.Split('~')[1]);
                DataTable dt_indentDetails = objPurchaseOrderBL.GetIndentRequisitionDate(Indent_No);
                if (dt_indentDetails != null && dt_indentDetails.Rows.Count > 0)
                {
                    string quotationdate = Convert.ToString(dt_indentDetails.Rows[0]["Indent_RequisitionDate"]);
                    if (!string.IsNullOrEmpty(quotationdate))
                    {
                        dt_Quotation.Text = Convert.ToString(quotationdate);

                    }
                }
            }
        }

        protected void taggingGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = Convert.ToString(e.Parameters.Split('~')[0]);


            if (strSplitCommand == "BindComponentGrid")
            {

                // string BranchID = Convert.ToString(ddl_Branch.SelectedValue);
                string strBranch = Convert.ToString(e.Parameters.Split('~')[1]);
                //string strBranch = Convert.ToString(Session["userbranchHierarchy"]);
                string FinYear = Convert.ToString(Session["LastFinYear"]);
                string POrderDate = dt_PLQuote.Date.ToString("yyyy-MM-dd");
                string Vendor = Convert.ToString(hdnCustomerId.Value);
                string status = string.Empty;

                if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                {
                    status = "DONE";
                }
                else
                {
                    status = "NOT-DONE";
                }

                DataTable IndentTable = blLayer.GetBillEntry(POrderDate, status, strBranch, Vendor);
                Session["IndentRequiData"] = IndentTable;
                taggingGrid.DataSource = IndentTable;
                taggingGrid.DataBind();


            }
            else if (strSplitCommand == "SelectAndDeSelectProducts")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                if (State == "SelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.SelectRow(i);
                    }
                }
                if (State == "UnSelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.UnselectRow(i);
                    }
                }
                if (State == "Revart")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        if (gv.Selection.IsRowSelected(i))
                            gv.Selection.UnselectRow(i);
                        else
                            gv.Selection.SelectRow(i);
                    }
                }
            }
        }

        protected void taggingGrid_DataBinding(object sender, EventArgs e)
        {
            if (Session["IndentRequiData"] != null)
            {
                taggingGrid.DataSource = (DataTable)Session["IndentRequiData"];
            }
        }
        protected void cgridProducts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "BindProductsDetails")
            {
                String QuoComponent = "", QuoComponentNumber = "", QuoComponentDate = "", PurchaseInvoiceNumber = "", PurchaseInvoiceDate = "", PurchaseOrderNumber = ""
                    , PurchaseOrderDate = "", PurchaseInvoice = "", PurchaseOrder = "";
                if (taggingGrid.GetSelectedFieldValues("BillEntry_Id").Count > 0)
                {
                    for (int i = 0; i < taggingGrid.GetSelectedFieldValues("BillEntry_Id").Count; i++)
                    {
                        QuoComponent += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("BillEntry_Id")[i]);
                        QuoComponentDate += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("BillEntry_Date")[i]);
                        QuoComponentNumber += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("BillEntry_Number")[i]);


                        PurchaseInvoiceNumber += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("PurchaseInvoiceNumber")[i]);
                        PurchaseInvoiceDate += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("PurchaseInvoice_Date")[i]);

                        PurchaseOrderNumber += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("PurchaseOrder_Number")[i]);
                        PurchaseOrderDate += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("PurchaseOrder_Date")[i]);


                        PurchaseInvoice += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("PurchaseInvoice_Id")[i]);
                        PurchaseOrder += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("PurchaseOrder_Id")[i]);


                    }

                    QuoComponent = QuoComponent.TrimStart(',');
                    QuoComponentDate = QuoComponentDate.TrimStart(',');
                    QuoComponentNumber = QuoComponentNumber.TrimStart(',');
                    PurchaseInvoiceNumber = PurchaseInvoiceNumber.TrimStart(',');
                    PurchaseOrderNumber = PurchaseOrderNumber.TrimStart(',');
                    PurchaseInvoice = PurchaseInvoice.TrimStart(',');
                    PurchaseOrder = PurchaseOrder.TrimStart(',');





                    if (taggingGrid.GetSelectedFieldValues("BillEntry_Id").Count > 0)
                    {
                        if (taggingGrid.GetSelectedFieldValues("BillEntry_Id").Count > 1)
                        {
                            QuoComponentDate = "Multiple Purchase Invoice Dates";
                        }
                    }
                    else
                    {
                        QuoComponentDate = "";
                    }

                    DataTable dt_QuotationDetails = new DataTable();
                    string IdKey = Convert.ToString(Request.QueryString["key"]);
                    if (!string.IsNullOrEmpty(IdKey))
                    {
                        if (IdKey != "ADD")
                        {
                            dt_QuotationDetails = blLayer.GetBillOfEntryDetailsForTagg(QuoComponent, IdKey, "", "Edit");
                        }
                        else
                        {
                            dt_QuotationDetails = blLayer.GetBillOfEntryDetailsForTagg(QuoComponent, "0", "", "Add");
                        }

                    }

                    grid_Products.DataSource = GetProductsInfo(dt_QuotationDetails);
                    grid_Products.DataBind();

                    for (int i = 0; i < grid_Products.VisibleRowCount; i++)
                    {
                        grid_Products.Selection.SelectRow(i);
                    }

                    grid_Products.JSProperties["cpComponentDetails"] = QuoComponentNumber + "~" + QuoComponentDate + "~" + PurchaseInvoiceNumber + "~" + PurchaseInvoiceDate + "~" + PurchaseOrderNumber + "~" + PurchaseOrderDate + "~" + PurchaseInvoice + "~" + PurchaseOrder;
                }
                else
                {
                    grid_Products.DataSource = null;
                    grid_Products.DataBind();
                }

            }
            if (strSplitCommand == "SelectAndDeSelectProducts")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                if (State == "SelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.SelectRow(i);
                    }
                }
                if (State == "UnSelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.UnselectRow(i);
                    }
                }
                if (State == "Revart")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        if (gv.Selection.IsRowSelected(i))
                            gv.Selection.UnselectRow(i);
                        else
                            gv.Selection.SelectRow(i);
                    }
                }



            }


        }


        public IEnumerable GetProductsInfo(DataTable SalesOrderdt1)
        {
            List<TaggedProduct> OrderList = new List<TaggedProduct>();
            for (int i = 0; i < SalesOrderdt1.Rows.Count; i++)
            {
                TaggedProduct ImportPOrders = new TaggedProduct();

                ImportPOrders.SrlNo = Convert.ToString(i + 1);
                //ImportPOrders.Key_UniqueId = Convert.ToString(i + 1);
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Billofentry_ID"])))
                { ImportPOrders.ComponentID = Convert.ToString(SalesOrderdt1.Rows[i]["Billofentry_ID"]); }
                else
                { ImportPOrders.ComponentID = "0"; }
                ImportPOrders.ProductID = Convert.ToString(SalesOrderdt1.Rows[i]["ProductId"]);
                ImportPOrders.ProductDescription = Convert.ToString(SalesOrderdt1.Rows[i]["ProductDescription"]);
                ImportPOrders.Quantity = Convert.ToString(SalesOrderdt1.Rows[i]["Quantity"]);
                ImportPOrders.ComponentNumber = Convert.ToString(SalesOrderdt1.Rows[i]["PurchaseOrderBillentry_Number"]);
                ImportPOrders.ProductsName = Convert.ToString(SalesOrderdt1.Rows[i]["ProductsName"]);
                ImportPOrders.ComponentDetailsID = Convert.ToString(SalesOrderdt1.Rows[i]["BillEntryDetails_Id"]);

                OrderList.Add(ImportPOrders);
            }

            return OrderList;
        }
        #endregion
        #region Subhabrata-Products
        protected void Productgrid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Productgrid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Productgrid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void aspxGridProduct_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
           
        }

        protected void Productgrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            ASPxGridView grid = (sender as ASPxGridView);
            if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
                e.Enabled = false;
        }

        [WebMethod]
        public static string getProductType(string Products_ID)
        {
            string Type = "";
            string query = @"Select
                           (Case When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then ''
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then 'W'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'B'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'S'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'WB'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'WS'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'WBS'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'BS'
                           END) as Type
                           from Master_sProducts
                           where sProducts_ID='" + Products_ID + "'";

            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
            DataTable dt = oDbEngine.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }

            return Convert.ToString(Type);
        }
        #endregion
        //#region  Available Stock
        //protected void acpAvailableStock_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    string performpara = e.Parameter;
        //    string strProductID = Convert.ToString(performpara.Split('~')[0]);
        //    string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
        //    acpAvailableStock.JSProperties["cpstock"] = "0.00";

        //    try
        //    {
        //        DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + strProductID + ") as branchopenstock");

        //        if (dt2.Rows.Count > 0)
        //        {
        //            acpAvailableStock.JSProperties["cpstock"] = Convert.ToString(Math.Round(Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]), 2));
        //        }
        //        else
        //        {
        //            acpAvailableStock.JSProperties["cpstock"] = "0.00";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //#endregion Available Stock
        #region Purchase Order Mail
        public int Sendmail_Purchaseorder(string Output)
        {
            int stat = 0;
            if (chkmail.Checked)
            {
                if (cmbContactPerson.Value != null)
                {
                    Employee_BL objemployeebal = new Employee_BL();
                    ExceptionLogging mailobj = new ExceptionLogging();
                    EmailSenderHelperEL emailSenderSettings = new EmailSenderHelperEL();
                    DataTable dt_EmailConfig = new DataTable();
                    DataTable dt_EmailConfigpurchase = new DataTable();

                    DataTable dt_Emailbodysubject = new DataTable();
                    PurchaseorderEmailTags fetchModel = new PurchaseorderEmailTags();
                    string Subject = "";
                    string Body = "";
                    string emailTo = "";
                    int MailStatus = 0;

                    var customerid = cmbContactPerson.Value.ToString();
                    dt_EmailConfig = objemployeebal.Getemailids(customerid);
                    string FilePath = Server.MapPath("~/Reports/RepxReportDesign/PurchaseOrder/DocDesign/PDFFiles/" + "PO-Default-" + Output + ".pdf");
                    string FileName = FilePath;
                    if (dt_EmailConfig.Rows.Count > 0)
                    {
                        emailTo = Convert.ToString(dt_EmailConfig.Rows[0]["eml_email"]);
                        dt_Emailbodysubject = objemployeebal.Getemailtemplates("13");  //for purchase order
                        if (dt_Emailbodysubject.Rows.Count > 0)
                        {
                            Body = Convert.ToString(dt_Emailbodysubject.Rows[0]["body"]);
                            Subject = Convert.ToString(dt_Emailbodysubject.Rows[0]["subjct"]);

                            dt_EmailConfigpurchase = objemployeebal.Getemailtagsforpurchase(Output, "PurchaseOrderEmailTags");  //For Purchase Order Get all Tags Value

                            if (dt_EmailConfigpurchase.Rows.Count > 0)
                            {
                                fetchModel = DbHelpers.ToModel<PurchaseorderEmailTags>(dt_EmailConfigpurchase);
                                Body = Employee_BL.GetFormattedString<PurchaseorderEmailTags>(fetchModel, Body);
                                Subject = Employee_BL.GetFormattedString<PurchaseorderEmailTags>(fetchModel, Subject);
                            }

                            emailSenderSettings = mailobj.GetEmailSettingsforAllreport(emailTo, "", "", FilePath, Body, Subject);
                            if (emailSenderSettings.IsSuccess)
                            {
                                string Message = "";
                                EmailSenderEL obj2 = new EmailSenderEL();
                                stat = SendEmailUL.sendMailInHtmlFormat(emailSenderSettings.ModelCast<EmailSenderEL>(), out Message);

                            }
                        }
                    }
                }

            }
            return stat;
        }
        #endregion
        //#region Visibility of Send Mail

        //public void VisiblitySendEmail()
        //{
        //    Employee_BL objemployeebal = new Employee_BL();
        //    chkmail.Visible = true;
        //    string userid = "";
        //    string branchId = "";
        //    string userfetch = "";
        //    if (HttpContext.Current.Session["userid"] != null)
        //    {
        //        userid = HttpContext.Current.Session["userid"].ToString();

        //    }
        //    if (HttpContext.Current.Session["userbranchID"] != null)
        //    {
        //        branchId = HttpContext.Current.Session["userbranchID"].ToString();
        //    }
        //    if (!Request.QueryString.AllKeys.Contains("status") && Request.QueryString["key"] != "ADD")
        //    {                
        //        chkmail.Visible = false;
        //    }
        //    else
        //    {
        //        DataTable dt2 = new DataTable();
        //        dt2 = objemployeebal.GetSystemsettingmail("Show Email in PO");
        //        if (Convert.ToString(dt2.Rows[0]["Variable_Value"]) == "Yes")
        //        {

        //            DataTable dt = new DataTable();
        //            dt = objemployeebal.GetApporvalMail(branchId, "PO");
        //            if (dt.Rows.Count > 0)
        //            {

        //                userfetch = Convert.ToString(dt.Rows[0]["userid"]);
        //                if (userfetch != "0")
        //                {

        //                    if (userid != userfetch)
        //                    {
        //                        chkmail.Visible = false;

        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            chkmail.Visible = false;

        //        }

        //    }
        //}

        //#endregion
        public class ProductTaxDetails
        {
            public string SrlNo { get; set; }
            public string IsTaxEntry { get; set; }
        }
        public class CurrencyChecking
        {
            public string PurchaseRate { get; set; }
            public int Ismatched { get; set; }
        }
        public class ProductStockDetails
        {
            public string Product_SrlNo { get; set; }
            public string SrlNo { get; set; }
            public string WarehouseID { get; set; }
            public string WarehouseName { get; set; }
            public string Quantity { get; set; }
            public string SalesQuantity { get; set; }
            public string Batch { get; set; }
            public string MfgDate { get; set; }
            public string ExpiryDate { get; set; }
            public string Rate { get; set; }
            public string SerialNo { get; set; }
            public string Barcode { get; set; }
            public string ViewBatch { get; set; }
            public string ViewMfgDate { get; set; }
            public string ViewExpiryDate { get; set; }
            public string ViewRate { get; set; }
            public string IsOutStatus { get; set; }
            public string IsOutStatusMsg { get; set; }
            public string LoopID { get; set; }
            public string Status { get; set; }
        }

        #region Warehouse Details

        protected void WarehousePanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strIsViewMode = "";
            string ProductSrlNo = "";
            string strSplitCommand = e.Parameter.Split('~')[0];
            WarehousePanel.JSProperties["cpIsShow"] = null;

            if (strSplitCommand == "StockDisplay")
            {
                ProductSrlNo = Convert.ToString(hdfProductSrlNo.Value);
                strIsViewMode = "Y";
            }
            else if (strSplitCommand == "StockSave")
            {
                string Sales_UOM_Name = Convert.ToString(hdfUOM.Value);
                string Stk_UOM_Name = Convert.ToString(hdfUOM.Value);
                string Conversion_Multiplier = "1";
                string Type = Convert.ToString(hdfWarehousetype.Value);
                bool IsDelete = false;
                int loopId = Convert.ToInt32(Session["Stock_LoopID"]);

                string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);
                string WarehouseName = Convert.ToString(e.Parameter.Split('~')[2]);
                string BatchName = Convert.ToString(e.Parameter.Split('~')[3]);
                string MfgDate = Convert.ToString(e.Parameter.Split('~')[4]);
                string ExpiryDate = Convert.ToString(e.Parameter.Split('~')[5]);
                string SerialNo = Convert.ToString(e.Parameter.Split('~')[6]);
                string Qty = Convert.ToString(e.Parameter.Split('~')[7]);
                string editWarehouseID = Convert.ToString(e.Parameter.Split('~')[8]);

                string ProductID = Convert.ToString(hdfProductID.Value);
                ProductSrlNo = Convert.ToString(hdfProductSrlNo.Value);

                DataTable Warehousedt = (DataTable)Session["Product_StockList"];

                if (Type == "WBS")
                {
                    int SerialNoCount_Server = CheckSerialNoExists(SerialNo);
                    var SerialNoCount_Local = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND SrlNo<>'" + editWarehouseID + "'");

                    if (editWarehouseID == "0")
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            string maxID = GetWarehouseMaxValue(Warehousedt);
                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                            if (updaterows.Length == 0)
                            {
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                            }
                            else
                            {
                                int newloopID = 0;
                                decimal oldQuantity = 0;

                                foreach (var row in updaterows)
                                {
                                    oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                    row["MfgDate"] = MfgDate;
                                    row["ExpiryDate"] = ExpiryDate;
                                    newloopID = Convert.ToInt32(row["LoopID"]);


                                    if (Convert.ToDecimal(row["TotalQuantity"]) != 0)
                                    {
                                        row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                        row["ViewMfgDate"] = MfgDate;
                                        row["ViewExpiryDate"] = ExpiryDate;
                                    }
                                }
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, "", MfgDate, ExpiryDate, SerialNo, newloopID, "D", "", "");
                            }
                        }
                        else
                        {
                            WarehousePanel.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                    else
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            var rows = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND WarehouseID ='" + WarehouseID + "' AND BatchID ='" + BatchName + "' AND SrlNo='" + editWarehouseID + "'");
                            if (rows.Length == 0)
                            {
                                int oldloopID = 0;
                                decimal oldTotalQuantity = 0;
                                string Pre_batcgid = "", Pre_batchname = "", Pre_warehouse = "", Pre_warehouseName = "", Pre_MfgDate = "", Pre_ExpiryDate = "";

                                var oldrows = Warehousedt.Select("SrlNo='" + editWarehouseID + "'");
                                foreach (var roww in oldrows)
                                {
                                    Pre_warehouse = Convert.ToString(roww["WarehouseID"]);
                                    Pre_warehouseName = Convert.ToString(roww["WarehouseName"]);
                                    Pre_batcgid = Convert.ToString(roww["BatchID"]);
                                    Pre_batchname = Convert.ToString(roww["BatchID"]);
                                    Pre_MfgDate = Convert.ToString(roww["MfgDate"]);
                                    Pre_ExpiryDate = Convert.ToString(roww["ExpiryDate"]);

                                    oldloopID = Convert.ToInt32(roww["LoopID"]);
                                    oldTotalQuantity = Convert.ToDecimal(roww["TotalQuantity"]);
                                }

                                if ((Pre_batcgid.Trim() == BatchName.Trim()) && (Pre_warehouse.Trim() == WarehouseID.Trim()))
                                {
                                    foreach (var rowww in oldrows)
                                    {
                                        rowww["SerialNo"] = SerialNo;
                                    }

                                    var Oldupdaterows = Warehousedt.Select("LoopID='" + oldloopID + "'");
                                    foreach (DataRow updaterow in Oldupdaterows)
                                    {
                                        updaterow["MfgDate"] = MfgDate;
                                        updaterow["ExpiryDate"] = ExpiryDate;

                                        if (Convert.ToString(updaterow["ViewMfgDate"]) != "") updaterow["ViewMfgDate"] = MfgDate;
                                        if (Convert.ToString(updaterow["ViewExpiryDate"]) != "") updaterow["ViewExpiryDate"] = ExpiryDate;
                                    }
                                }
                                else
                                {
                                    foreach (DataRow delrow in oldrows)
                                    {
                                        delrow.Delete();
                                    }
                                    Warehousedt.AcceptChanges();

                                    var Oldupdaterows = Warehousedt.Select("LoopID='" + oldloopID + "'");

                                    if (oldTotalQuantity != 0)
                                    {
                                        foreach (DataRow updaterow in Oldupdaterows)
                                        {
                                            decimal PreQuantity = Convert.ToDecimal(updaterow["Quantity"]);
                                            updaterow["WarehouseName"] = Pre_warehouseName;
                                            updaterow["BatchNo"] = Pre_batchname;
                                            updaterow["SalesQuantity"] = (PreQuantity - Convert.ToDecimal(1)) + " " + Sales_UOM_Name;

                                            updaterow["ViewMfgDate"] = Pre_MfgDate;
                                            updaterow["ViewExpiryDate"] = Pre_ExpiryDate;

                                            break;
                                        }
                                    }

                                    foreach (DataRow updaterow in Oldupdaterows)
                                    {
                                        decimal PreQuantity = Convert.ToDecimal(updaterow["Quantity"]);

                                        if (Convert.ToString(updaterow["SalesQuantity"]) != "")
                                        {
                                            updaterow["Quantity"] = (PreQuantity - Convert.ToDecimal(1));
                                            updaterow["SalesQuantity"] = (PreQuantity - Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                            updaterow["TotalQuantity"] = (PreQuantity - Convert.ToDecimal(1));
                                        }
                                        else
                                        {
                                            updaterow["Quantity"] = (PreQuantity - Convert.ToDecimal(1));
                                            updaterow["TotalQuantity"] = "0";
                                            updaterow["WarehouseName"] = "";
                                            updaterow["BatchNo"] = "";
                                        }
                                    }

                                    string maxID = GetWarehouseMaxValue(Warehousedt);
                                    var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                                    if (updaterows.Length == 0)
                                    {
                                        Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                                    }
                                    else
                                    {
                                        int newloopID = 0;
                                        decimal oldQuantity = 0;

                                        foreach (var row in updaterows)
                                        {
                                            oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                            row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                            row["MfgDate"] = MfgDate;
                                            row["ExpiryDate"] = ExpiryDate;
                                            newloopID = Convert.ToInt32(row["LoopID"]);

                                            if (Convert.ToDecimal(row["TotalQuantity"]) != 0)
                                            {
                                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                                row["ViewMfgDate"] = MfgDate;
                                                row["ViewExpiryDate"] = ExpiryDate;
                                            }
                                        }
                                        Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, "", MfgDate, ExpiryDate, SerialNo, newloopID, "D", "", "");
                                    }
                                }
                            }
                        }
                        else
                        {
                            WarehousePanel.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                }
                else if (Type == "W")
                {
                    if (editWarehouseID == "0")
                    {
                        string maxID = GetWarehouseMaxValue(Warehousedt);
                        var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Convert(Quantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");
                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = GetWarehouseMaxValue(Warehousedt);

                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSrlNo + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                            }
                            else if (editWarehouseID == whID)
                            {
                                foreach (var row in updaterows)
                                {
                                    whID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = Qty;
                                    row["TotalQuantity"] = Qty;
                                    row["SalesQuantity"] = Qty + " " + Sales_UOM_Name;
                                }
                            }
                            else if (editWarehouseID != whID)
                            {
                                IsDelete = true;
                                foreach (var row in updaterows)
                                {
                                    ID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                }
                            }
                        }
                    }
                }
                else if (Type == "WB")
                {
                    if (editWarehouseID == "0")
                    {
                        string maxID = GetWarehouseMaxValue(Warehousedt);
                        var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchName + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                row["MfgDate"] = MfgDate;
                                row["ExpiryDate"] = ExpiryDate;
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchName + "' AND Convert(TotalQuantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");
                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = GetWarehouseMaxValue(Warehousedt);

                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchName + "' AND Product_SrlNo='" + ProductSrlNo + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                            }
                            else if (editWarehouseID == whID)
                            {
                                foreach (var row in updaterows)
                                {
                                    whID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = Qty;
                                    row["TotalQuantity"] = Qty;
                                    row["SalesQuantity"] = Qty + " " + Sales_UOM_Name;
                                    row["MfgDate"] = MfgDate;
                                    row["ExpiryDate"] = ExpiryDate;
                                }
                            }
                            else if (editWarehouseID != whID)
                            {
                                IsDelete = true;
                                foreach (var row in updaterows)
                                {
                                    ID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                    row["MfgDate"] = MfgDate;
                                    row["ExpiryDate"] = ExpiryDate;
                                }
                            }
                        }
                    }
                }
                else if (Type == "B")
                {
                    WarehouseID = "0";

                    if (editWarehouseID == "0")
                    {
                        string maxID = GetWarehouseMaxValue(Warehousedt);
                        var updaterows = Warehousedt.Select("BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                row["MfgDate"] = MfgDate;
                                row["ExpiryDate"] = ExpiryDate;
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("BatchID='" + BatchName + "' AND Convert(TotalQuantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");
                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = GetWarehouseMaxValue(Warehousedt);

                            var updaterows = Warehousedt.Select("BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSrlNo + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                            }
                            else if (editWarehouseID == whID)
                            {
                                foreach (var row in updaterows)
                                {
                                    whID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = Qty;
                                    row["TotalQuantity"] = Qty;
                                    row["SalesQuantity"] = Qty + " " + Sales_UOM_Name;
                                    row["MfgDate"] = MfgDate;
                                    row["ExpiryDate"] = ExpiryDate;
                                }
                            }
                            else if (editWarehouseID != whID)
                            {
                                IsDelete = true;
                                foreach (var row in updaterows)
                                {
                                    ID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                    row["MfgDate"] = MfgDate;
                                    row["ExpiryDate"] = ExpiryDate;
                                }
                            }
                        }
                    }
                }
                else if (Type == "S")
                {
                    int SerialNoCount_Server = CheckSerialNoExists(SerialNo);
                    var SerialNoCount_Local = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND SrlNo<>'" + editWarehouseID + "'");

                    if (editWarehouseID == "0")
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            string maxID = GetWarehouseMaxValue(Warehousedt);

                            int newloopID = 0;
                            decimal oldQuantity = 0;

                            var updaterows = Warehousedt.Select("Product_SrlNo='" + ProductSrlNo + "'");
                            if (updaterows.Length > 0)
                            {
                                foreach (var row in updaterows)
                                {
                                    oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    newloopID = Convert.ToInt32(row["LoopID"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                    if (Convert.ToDecimal(row["TotalQuantity"]) != 0)
                                    {
                                        row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                    }
                                }

                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, newloopID, "D", MfgDate, ExpiryDate);
                            }
                            else
                            {
                                newloopID = loopId;
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal(1) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, newloopID, "D", MfgDate, ExpiryDate);
                            }
                        }
                        else
                        {
                            WarehousePanel.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                    else
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            var rows = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND SrlNo='" + editWarehouseID + "'");
                            if (rows.Length == 0)
                            {
                                DataRow[] updaterows = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                                foreach (DataRow row in updaterows)
                                {
                                    row["SerialNo"] = SerialNo;
                                }
                            }
                        }
                        else
                        {
                            WarehousePanel.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                }
                else if (Type == "WS")
                {
                    int SerialNoCount_Server = CheckSerialNoExists(SerialNo);
                    var SerialNoCount_Local = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND SrlNo<>'" + editWarehouseID + "'");

                    if (editWarehouseID == "0")
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            string maxID = GetWarehouseMaxValue(Warehousedt);
                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                            if (updaterows.Length == 0)
                            {
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                            }
                            else
                            {
                                int newloopID = 0;
                                decimal oldQuantity = 0;

                                foreach (var row in updaterows)
                                {
                                    oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    newloopID = Convert.ToInt32(row["LoopID"]);
                                    break;

                                    //row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                    //row["MfgDate"] = MfgDate;
                                    //row["ExpiryDate"] = ExpiryDate;
                                    //newloopID = Convert.ToInt32(row["LoopID"]);

                                    //if (Convert.ToDecimal(row["TotalQuantity"]) != 0)
                                    //{
                                    //    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                    //}
                                }

                                Warehousedt.Select(string.Format("[WarehouseID]='{0}' and [Product_SrlNo]='{1}' and TotalQuantity<>'0' ", WarehouseID, ProductSrlNo)).ToList<DataRow>().ForEach(
                                  r => { r["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name; r["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(1)); });

                                Warehousedt.Select(string.Format("[WarehouseID]='{0}' and [Product_SrlNo]='{1}' ", WarehouseID, ProductSrlNo)).ToList<DataRow>().ForEach(
                                    r => { r["Quantity"] = (oldQuantity + Convert.ToDecimal(1)); r["MfgDate"] = MfgDate; r["ExpiryDate"] = ExpiryDate; });

                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, newloopID, "D", MfgDate, ExpiryDate);
                            }
                        }
                        else
                        {
                            WarehousePanel.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                    else
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            var rows = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND WarehouseID ='" + WarehouseID + "' AND SrlNo='" + editWarehouseID + "'");
                            if (rows.Length == 0)
                            {
                                int oldloopID = 0;
                                decimal oldTotalQuantity = 0;
                                string Pre_warehouse = "", Pre_warehouseName = "";

                                var oldrows = Warehousedt.Select("SrlNo='" + editWarehouseID + "'");
                                foreach (var roww in oldrows)
                                {
                                    Pre_warehouse = Convert.ToString(roww["WarehouseID"]);
                                    Pre_warehouseName = Convert.ToString(roww["WarehouseName"]);
                                    oldloopID = Convert.ToInt32(roww["LoopID"]);
                                    oldTotalQuantity = Convert.ToDecimal(roww["TotalQuantity"]);
                                }

                                if (Pre_warehouse.Trim() == WarehouseID.Trim())
                                {
                                    foreach (var rowww in oldrows)
                                    {
                                        rowww["SerialNo"] = SerialNo;
                                    }
                                }
                                else
                                {
                                    foreach (DataRow delrow in oldrows)
                                    {
                                        delrow.Delete();
                                    }
                                    Warehousedt.AcceptChanges();

                                    var Oldupdaterows = Warehousedt.Select("LoopID='" + oldloopID + "'");

                                    if (oldTotalQuantity != 0)
                                    {
                                        foreach (DataRow updaterow in Oldupdaterows)
                                        {
                                            decimal PreQuantity = Convert.ToDecimal(updaterow["Quantity"]);
                                            updaterow["WarehouseName"] = Pre_warehouseName;
                                            updaterow["SalesQuantity"] = (PreQuantity - Convert.ToDecimal(1)) + " " + Sales_UOM_Name;

                                            break;
                                        }
                                    }

                                    foreach (DataRow updaterow in Oldupdaterows)
                                    {
                                        decimal PreQuantity = Convert.ToDecimal(updaterow["Quantity"]);

                                        if (Convert.ToString(updaterow["SalesQuantity"]) != "")
                                        {
                                            updaterow["Quantity"] = (PreQuantity - Convert.ToDecimal(1));
                                            updaterow["SalesQuantity"] = (PreQuantity - Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                            updaterow["TotalQuantity"] = (PreQuantity - Convert.ToDecimal(1));
                                        }
                                        else
                                        {
                                            updaterow["Quantity"] = (PreQuantity - Convert.ToDecimal(1));
                                            updaterow["TotalQuantity"] = "0";
                                            updaterow["WarehouseName"] = "";
                                        }
                                    }

                                    string maxID = GetWarehouseMaxValue(Warehousedt);
                                    var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                                    if (updaterows.Length == 0)
                                    {
                                        Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                                    }
                                    else
                                    {
                                        int newloopID = 0;
                                        decimal oldQuantity = 0;

                                        foreach (var row in updaterows)
                                        {
                                            oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                            row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                            row["MfgDate"] = MfgDate;
                                            row["ExpiryDate"] = ExpiryDate;
                                            newloopID = Convert.ToInt32(row["LoopID"]);

                                            if (Convert.ToDecimal(row["TotalQuantity"]) != 0)
                                            {
                                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                            }
                                        }
                                        Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, newloopID, "D", MfgDate, ExpiryDate);
                                    }
                                }
                            }
                        }
                        else
                        {
                            WarehousePanel.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                }
                else if (Type == "BS")
                {
                    int SerialNoCount_Server = CheckSerialNoExists(SerialNo);
                    var SerialNoCount_Local = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND SrlNo<>'" + editWarehouseID + "'");

                    if (editWarehouseID == "0")
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            string maxID = GetWarehouseMaxValue(Warehousedt);
                            var updaterows = Warehousedt.Select("BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                            if (updaterows.Length == 0)
                            {
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                            }
                            else
                            {
                                int newloopID = 0;
                                decimal oldQuantity = 0;

                                foreach (var row in updaterows)
                                {
                                    oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                    row["MfgDate"] = MfgDate;
                                    row["ExpiryDate"] = ExpiryDate;
                                    newloopID = Convert.ToInt32(row["LoopID"]);

                                    if (Convert.ToDecimal(row["TotalQuantity"]) != 0)
                                    {
                                        row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                    }
                                }
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, "", MfgDate, ExpiryDate, SerialNo, newloopID, "D", "", "");
                            }
                        }
                        else
                        {
                            WarehousePanel.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                    else
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            var rows = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND BatchID ='" + BatchName + "' AND SrlNo='" + editWarehouseID + "'");
                            if (rows.Length == 0)
                            {
                                int oldloopID = 0;
                                decimal oldTotalQuantity = 0;
                                string Pre_batcgid = "", Pre_batchname = "";

                                var oldrows = Warehousedt.Select("SrlNo='" + editWarehouseID + "'");
                                foreach (var roww in oldrows)
                                {
                                    Pre_batcgid = Convert.ToString(roww["BatchID"]);
                                    Pre_batchname = Convert.ToString(roww["BatchID"]);
                                    oldloopID = Convert.ToInt32(roww["LoopID"]);
                                    oldTotalQuantity = Convert.ToDecimal(roww["TotalQuantity"]);
                                }

                                if (Pre_batcgid.Trim() == BatchName.Trim())
                                {
                                    foreach (var rowww in oldrows)
                                    {
                                        rowww["SerialNo"] = SerialNo;
                                    }
                                }
                                else
                                {
                                    foreach (DataRow delrow in oldrows)
                                    {
                                        delrow.Delete();
                                    }
                                    Warehousedt.AcceptChanges();

                                    var Oldupdaterows = Warehousedt.Select("LoopID='" + oldloopID + "'");

                                    if (oldTotalQuantity != 0)
                                    {
                                        foreach (DataRow updaterow in Oldupdaterows)
                                        {
                                            decimal PreQuantity = Convert.ToDecimal(updaterow["Quantity"]);
                                            updaterow["BatchNo"] = Pre_batchname;
                                            updaterow["SalesQuantity"] = (PreQuantity - Convert.ToDecimal(1)) + " " + Sales_UOM_Name;

                                            break;
                                        }
                                    }

                                    foreach (DataRow updaterow in Oldupdaterows)
                                    {
                                        decimal PreQuantity = Convert.ToDecimal(updaterow["Quantity"]);

                                        if (Convert.ToString(updaterow["SalesQuantity"]) != "")
                                        {
                                            updaterow["Quantity"] = (PreQuantity - Convert.ToDecimal(1));
                                            updaterow["SalesQuantity"] = (PreQuantity - Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                            updaterow["TotalQuantity"] = (PreQuantity - Convert.ToDecimal(1));
                                        }
                                        else
                                        {
                                            updaterow["Quantity"] = (PreQuantity - Convert.ToDecimal(1));
                                            updaterow["TotalQuantity"] = "0";
                                            updaterow["BatchNo"] = "";
                                        }
                                    }

                                    string maxID = GetWarehouseMaxValue(Warehousedt);
                                    var updaterows = Warehousedt.Select("BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                                    if (updaterows.Length == 0)
                                    {
                                        Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                                    }
                                    else
                                    {
                                        int newloopID = 0;
                                        decimal oldQuantity = 0;

                                        foreach (var row in updaterows)
                                        {
                                            oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                            row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                            row["MfgDate"] = MfgDate;
                                            row["ExpiryDate"] = ExpiryDate;
                                            newloopID = Convert.ToInt32(row["LoopID"]);

                                            if (Convert.ToDecimal(row["TotalQuantity"]) != 0)
                                            {
                                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                            }
                                        }
                                        Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, "", MfgDate, ExpiryDate, SerialNo, newloopID, "D", "", "");
                                    }
                                }
                            }
                        }
                        else
                        {
                            WarehousePanel.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                }


                if (IsDelete == true)
                {
                    DataRow[] delResult = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                    foreach (DataRow delrow in delResult)
                    {
                        delrow.Delete();
                    }
                    Warehousedt.AcceptChanges();
                }

                Session["Product_StockList"] = Warehousedt;
                Session["Stock_LoopID"] = loopId + 1;

                if (Type == "W" || Type == "WB" || Type == "WS")
                {
                    #region Bind WarehouseList with Default Warehouse

                    DataTable dt = GetWarehouseData();
                    string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

                    CmbWarehouseID.Items.Clear();
                    CmbWarehouseID.DataSource = dt;
                    CmbWarehouseID.DataBind();
                    CmbWarehouseID.Value = WarehouseID;

                    #endregion

                    txtBatchName.Text = "";
                    txtQuantity.Text = "0";
                    txtserialID.Text = "";
                    txtStartDate.Value = null;
                    txtEndDate.Value = null;
                }
                else if (Type == "WBS")
                {
                    #region Bind WarehouseList with Default Warehouse

                    DataTable dt = GetWarehouseData();
                    string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

                    CmbWarehouseID.Items.Clear();
                    CmbWarehouseID.DataSource = dt;
                    CmbWarehouseID.DataBind();
                    CmbWarehouseID.Value = WarehouseID;

                    #endregion

                    txtBatchName.Text = "";
                    txtQuantity.Text = "0";
                    txtStartDate.Value = null;
                    txtEndDate.Value = null;
                }
                else if (Type == "B" || Type == "BS")
                {
                    txtQuantity.Text = "0";
                    txtserialID.Text = "";
                }
                else if (Type == "S")
                {
                    txtBatchName.Text = "";
                    txtQuantity.Text = "0";
                    txtserialID.Text = "";
                    txtStartDate.Value = null;
                    txtEndDate.Value = null;
                }

                #region Bind WarehouseList with Default Warehouse

                DataTable _dt = GetWarehouseData();
                string strBranchID = Convert.ToString(ddl_Branch.SelectedValue);

                CmbWarehouseID.Items.Clear();
                CmbWarehouseID.DataSource = _dt;
                CmbWarehouseID.DataBind();

                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

                DataTable dtdefaultStock = objEngine.GetDataTable("tbl_master_building", " top 1 bui_id ", " bui_BranchId='" + strBranchID + "' ");
                if (dtdefaultStock != null && dtdefaultStock.Rows.Count > 0)
                {
                    string defaultID = Convert.ToString(dtdefaultStock.Rows[0]["bui_id"]).Trim();

                    if (defaultID != null || defaultID != "" || defaultID != "0")
                    {
                        CmbWarehouseID.Value = defaultID;
                    }
                }

                #endregion

                txtBatchName.Text = "";
                txtQuantity.Text = "0";
                txtserialID.Text = "";
                txtStartDate.Value = null;
                txtEndDate.Value = null;

                #region Bind Stock Record of the Product

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    Warehousedt.DefaultView.Sort = "LoopID,SrlNo ASC";
                    DataTable sortWarehousedt = Warehousedt.DefaultView.ToTable();

                    DataView dvData = new DataView(sortWarehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + ProductSrlNo + "'";

                    GrdWarehouse.DataSource = dvData;
                    GrdWarehouse.DataBind();
                }
                else
                {
                    GrdWarehouse.DataSource = Warehousedt.DefaultView;
                    GrdWarehouse.DataBind();
                }

                #endregion

                WarehousePanel.JSProperties["cpIsShow"] = "Y";
            }
            else if (strSplitCommand == "WarehouseFinal")
            {
                if (Session["Product_StockList"] != null)
                {
                    DataTable PC_WarehouseData = (DataTable)Session["Product_StockList"];
                    string ProductID = Convert.ToString(hdfProductSrlNo.Value);
                    decimal sum = 0;

                    for (int i = 0; i < PC_WarehouseData.Rows.Count; i++)
                    {
                        DataRow dr = PC_WarehouseData.Rows[i];
                        string delLoopID = Convert.ToString(dr["LoopID"]);
                        string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);

                        if (ProductID == Product_SrlNo)
                        {
                            string strQuantity = (Convert.ToString(dr["SalesQuantity"]) != "") ? Convert.ToString(dr["SalesQuantity"]) : "0";
                            var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);

                            sum = sum + Convert.ToDecimal(weight);
                        }
                    }

                    if (Convert.ToDecimal(sum) == Convert.ToDecimal(hdnProductQuantity.Value))
                    {
                        WarehousePanel.JSProperties["cpIsSave"] = "Y";
                        for (int i = 0; i < PC_WarehouseData.Rows.Count; i++)
                        {
                            DataRow dr = PC_WarehouseData.Rows[i];
                            string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);
                            if (ProductID == Product_SrlNo)
                            {
                                dr["Status"] = "I";
                            }
                        }
                        PC_WarehouseData.AcceptChanges();
                    }
                    else
                    {
                        WarehousePanel.JSProperties["cpIsSave"] = "N";
                    }

                    Session["Product_StockList"] = PC_WarehouseData;
                    WarehousePanel.JSProperties["cpIsShow"] = "Y";
                }
            }
            else if (strSplitCommand == "Delete")
            {
                string strKey = e.Parameter.Split('~')[1];
                string strLoopID = "", strPreLoopID = "";
                DataTable Warehousedt = (DataTable)Session["Product_StockList"];

                DataRow[] result = Warehousedt.Select("SrlNo ='" + strKey + "'");
                foreach (DataRow row in result)
                {
                    strLoopID = row["LoopID"].ToString();
                }

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    int count = 0;
                    bool IsFirst = false, IsAssign = false;
                    string WarehouseName = "", Quantity = "", SalesUOMName = "", BatchNo = "", SalesQuantity = "", MfgDate = "", ExpiryDate = "";

                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        DataRow dr = Warehousedt.Rows[i];
                        string delSrlID = Convert.ToString(dr["SrlNo"]);
                        string delLoopID = Convert.ToString(dr["LoopID"]);

                        if (strPreLoopID != delLoopID)
                        {
                            count = 0;
                        }

                        if ((delLoopID == strLoopID) && (strKey == delSrlID) && count == 0)
                        {
                            IsFirst = true;

                            WarehouseName = Convert.ToString(dr["WarehouseName"]);
                            Quantity = Convert.ToString(dr["Quantity"]);
                            SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                            BatchNo = Convert.ToString(dr["BatchNo"]);
                            SalesQuantity = Convert.ToString(dr["SalesQuantity"]);
                            MfgDate = Convert.ToString(dr["MfgDate"]);
                            ExpiryDate = Convert.ToString(dr["ExpiryDate"]);

                            //dr.Delete();
                        }
                        else
                        {
                            if (delLoopID == strLoopID)
                            {
                                if (strKey == delSrlID)
                                {
                                    //dr.Delete();
                                }
                                else
                                {
                                    decimal S_Quantity = Convert.ToDecimal(dr["Quantity"]);
                                    dr["Quantity"] = S_Quantity - 1;

                                    if (IsFirst == true && IsAssign == false)
                                    {
                                        IsAssign = true;

                                        dr["WarehouseName"] = WarehouseName;
                                        dr["BatchNo"] = BatchNo;
                                        dr["SalesUOMName"] = SalesUOMName;
                                        dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;
                                        dr["MfgDate"] = MfgDate;
                                        dr["ExpiryDate"] = ExpiryDate;
                                        dr["TotalQuantity"] = S_Quantity - 1;
                                        dr["ViewMfgDate"] = MfgDate;
                                        dr["ViewExpiryDate"] = ExpiryDate;
                                    }
                                    else
                                    {
                                        if (IsAssign == false)
                                        {
                                            IsAssign = true;
                                            SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                                            dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;
                                        }
                                    }
                                }
                            }
                        }

                        strPreLoopID = delLoopID;
                        count++;
                    }
                    Warehousedt.AcceptChanges();


                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        DataRow dr = Warehousedt.Rows[i];
                        string delSrlID = Convert.ToString(dr["SrlNo"]);
                        if (strKey == delSrlID)
                        {
                            dr.Delete();
                        }
                    }
                    Warehousedt.AcceptChanges();
                }

                Session["Product_StockList"] = Warehousedt;
                GrdWarehouse.DataSource = Warehousedt.DefaultView;
                GrdWarehouse.DataBind();
                WarehousePanel.JSProperties["cpIsShow"] = "Y";
            }
            else if (strSplitCommand == "EditWarehouse")
            {
                string strKey = e.Parameter.Split('~')[1];

                if (Session["Product_StockList"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["Product_StockList"];

                    string strWarehouse = "", strBatchID = "", strSrlID = "", strQuantity = "0", MfgDate = "", ExpiryDate = "";
                    var rows = Warehousedt.Select(string.Format("SrlNo ='{0}'", strKey));
                    foreach (var dr in rows)
                    {
                        strWarehouse = (Convert.ToString(dr["WarehouseID"]) != "") ? Convert.ToString(dr["WarehouseID"]) : "0";
                        strBatchID = (Convert.ToString(dr["BatchID"]) != "") ? Convert.ToString(dr["BatchID"]) : "";
                        MfgDate = (Convert.ToString(dr["MfgDate"]) != "") ? Convert.ToString(dr["MfgDate"]) : "";
                        ExpiryDate = (Convert.ToString(dr["ExpiryDate"]) != "") ? Convert.ToString(dr["ExpiryDate"]) : "";
                        strSrlID = (Convert.ToString(dr["SerialNo"]) != "") ? Convert.ToString(dr["SerialNo"]) : "";
                        strQuantity = (Convert.ToString(dr["Quantity"]) != "") ? Convert.ToString(dr["Quantity"]) : "0";
                    }

                    #region Bind WarehouseList with Default Warehouse

                    DataTable dt = GetWarehouseData();
                    string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

                    CmbWarehouseID.Items.Clear();
                    CmbWarehouseID.DataSource = dt;
                    CmbWarehouseID.DataBind();

                    #endregion

                    CmbWarehouseID.Value = strWarehouse;
                    txtQuantity.Value = strQuantity;
                    txtBatchName.Value = strBatchID;
                    txtserialID.Value = strSrlID;
                    if (MfgDate != "") txtStartDate.Value = Convert.ToDateTime(DateFormatting(MfgDate));
                    if (ExpiryDate != "") txtEndDate.Value = Convert.ToDateTime(DateFormatting(ExpiryDate));
                    WarehousePanel.JSProperties["cpIsShow"] = "Y";
                }
            }
            else if (strSplitCommand == "WarehouseDelete")
            {
                string ProductID = Convert.ToString(hdfProductSrlNo.Value);
                DeleteUnsaveWarehouse(ProductID);
            }

            if (strIsViewMode == "Y")
            {
                #region Bind WarehouseList with Default Warehouse

                DataTable dt = GetWarehouseData();
                string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

                CmbWarehouseID.Items.Clear();
                CmbWarehouseID.DataSource = dt;
                CmbWarehouseID.DataBind();

                // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

                DataTable dtdefaultStock = objEngine.GetDataTable("tbl_master_building", " top 1 bui_id ", " bui_BranchId='" + strBranch + "' ");
                if (dtdefaultStock != null && dtdefaultStock.Rows.Count > 0)
                {
                    string defaultID = Convert.ToString(dtdefaultStock.Rows[0]["bui_id"]).Trim();

                    if (defaultID != null || defaultID != "" || defaultID != "0")
                    {
                        CmbWarehouseID.Value = defaultID;
                    }
                }

                #endregion

                #region Bind Stock Record of the Product

                DataTable Warehousedt = (DataTable)Session["Product_StockList"];

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    Warehousedt.DefaultView.Sort = "LoopID,SrlNo ASC";
                    DataTable sortWarehousedt = Warehousedt.DefaultView.ToTable();

                    DataView dvData = new DataView(sortWarehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + ProductSrlNo + "'";

                    GrdWarehouse.DataSource = dvData;
                    GrdWarehouse.DataBind();
                }
                else
                {
                    GrdWarehouse.DataSource = Warehousedt.DefaultView;
                    GrdWarehouse.DataBind();
                }

                #endregion

                txtBatchName.Text = "";
                txtQuantity.Text = "0";
                txtserialID.Text = "";
                txtStartDate.Value = null;
                txtEndDate.Value = null;

                WarehousePanel.JSProperties["cpIsShow"] = "Y";
            }
        }
        public void DeleteUnsaveWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["Product_StockList"] != null)
            {
                Warehousedt = (DataTable)Session["Product_StockList"];

                var rows = Warehousedt.Select("Product_SrlNo ='" + SrlNo + "' AND Status='D'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["Product_StockList"] = Warehousedt;
            }
        }
        protected void GrdWarehouse_DataBinding(object sender, EventArgs e)
        {
            if (Session["Product_StockList"] != null)
            {
                #region Bind Stock Record of the Product

                string ProductSrlNo = Convert.ToString(hdfProductSrlNo.Value);
                DataTable Warehousedt = (DataTable)Session["Product_StockList"];

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    Warehousedt.DefaultView.Sort = "LoopID,SrlNo ASC";
                    DataTable sortWarehousedt = Warehousedt.DefaultView.ToTable();

                    DataView dvData = new DataView(sortWarehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + ProductSrlNo + "'";

                    GrdWarehouse.DataSource = dvData;
                }
                else
                {
                    GrdWarehouse.DataSource = Warehousedt.DefaultView;
                }

                #endregion
            }
        }
        public DataTable GetWarehouseData()
        {
            string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

            DataTable dt = new DataTable();
            //dt = oDBEngine.GetDataTable("select  bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + strBranch + "') order by bui_Name");

            //MasterSettings masterBl = new MasterSettings();
            //string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));

            //if (multiwarehouse != "1")
            //{
            dt = oDBEngine.GetDataTable("select  bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + strBranch + "') order by bui_Name");
            //}
            //else
            //{
            //    dt = oDBEngine.GetDataTable("EXEC [GET_BRANCHWISEWAREHOUSE] '1','" + strBranch + "'");
            //}




            return dt;
        }
        public string GetWarehouseMaxValue(DataTable dt)
        {
            string maxValue = "1";
            if (dt != null && dt.Rows.Count > 0)
            {
                List<int> myList = new List<int>();
                foreach (DataRow rrow in dt.Rows)
                {
                    string value = (Convert.ToString(rrow["SrlNo"]) != "") ? Convert.ToString(rrow["SrlNo"]) : "0";
                    myList.Add(Convert.ToInt32(value));
                }

                maxValue = Convert.ToString(Convert.ToInt32(myList.Max()) + 1);
            }

            return maxValue;
        }
        public int CheckSerialNoExists(string SerialNo)
        {
            string ProductID = Convert.ToString(hdfProductID.Value);
            string BranchID = Convert.ToString(ddl_Branch.SelectedValue);

            DataTable SerialCount = CheckDuplicateSerial(SerialNo, ProductID, BranchID, "PurchaseChallan");
            return SerialCount.Rows.Count;
        }
        public DataTable CheckDuplicateSerial(string SerialNo, string ProductID, string BranchID, string Action)
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("proc_CheckDuplicateProduct");
                proc.AddVarcharPara("@Action", 500, Action);
                proc.AddVarcharPara("@SerialNo", 500, SerialNo);
                proc.AddVarcharPara("@ProductID", 500, ProductID);
                proc.AddVarcharPara("@BranchID", 500, BranchID);
                proc.AddVarcharPara("@DocID", 500, Convert.ToString(Session["PurchaseChallan_Id"]));
                dt = proc.GetTable();

                return dt;
            }
            catch
            {
                return null;
            }
        }
        public string GetWarehouseMaxLoopValue(DataTable dt)
        {
            string maxValue = "1";
            if (dt != null && dt.Rows.Count > 0)
            {
                List<int> myList = new List<int>();
                foreach (DataRow rrow in dt.Rows)
                {
                    string value = (Convert.ToString(rrow["LoopID"]) != "") ? Convert.ToString(rrow["LoopID"]) : "0";
                    myList.Add(Convert.ToInt32(value));
                }

                maxValue = Convert.ToString(Convert.ToInt32(myList.Max()) + 1);
            }

            return maxValue;
        }
        public string DateFormatting(string Date)
        {
            string Day = Date.Substring(0, 2);
            string Month = Date.Substring(3, 2);
            string Year = Date.Substring(6, 4);

            string returnDate = Year + "-" + Month + "-" + Day;
            return returnDate;
        }
        public DataTable GetDuplicateDetails(string GRNID, DataTable Productlist, DataTable Stockdt, string Action)
        {
            try
            {
                DataSet dsInst = new DataSet();


                // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


                SqlCommand cmd = new SqlCommand("proc_CheckDuplicateSerial", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@PurchaseChallan_ID", GRNID);
                cmd.Parameters.AddWithValue("@ProductDetails", Productlist);
                cmd.Parameters.AddWithValue("@StockDetail", Stockdt);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                DataTable dt = dsInst.Tables[0];

                return dt;
            }
            catch
            {
                return null;
            }
        }
        public void DeleteWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["Product_StockList"] != null)
            {
                Warehousedt = (DataTable)Session["Product_StockList"];

                var rows = Warehousedt.Select(string.Format("Product_SrlNo ='{0}'", SrlNo));
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["Product_StockList"] = Warehousedt;
            }
        }
        public object GetStock(DataTable dt)
        {

            List<ProductStockDetails> ProductList = new List<ProductStockDetails>();

            ProductList = (from DataRow dr in dt.Rows
                           select new ProductStockDetails()
                           {
                               Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]),
                               SrlNo = Convert.ToString(dr["SrlNo"]),
                               WarehouseID = Convert.ToString(dr["WarehouseID"]),
                               WarehouseName = Convert.ToString(dr["WarehouseName"]),
                               Quantity = Convert.ToString(dr["Quantity"]),
                               SalesQuantity = Convert.ToString(dr["SalesQuantity"]),
                               Batch = Convert.ToString(dr["Batch"]),
                               MfgDate = Convert.ToString(dr["MfgDate"]),
                               ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                               SerialNo = Convert.ToString(dr["SerialNo"]),
                               Barcode = Convert.ToString(dr["Barcode"]),
                               ViewBatch = Convert.ToString(dr["ViewBatch"]),
                               ViewMfgDate = Convert.ToString(dr["ViewMfgDate"]),
                               ViewExpiryDate = Convert.ToString(dr["ViewExpiryDate"]),
                               IsOutStatus = Convert.ToString(dr["IsOutStatus"]),
                               IsOutStatusMsg = Convert.ToString(dr["IsOutStatusMsg"]),
                               LoopID = Convert.ToString(dr["LoopID"]),
                               Status = Convert.ToString(dr["Status"])
                           }).ToList();

            return ProductList;
        }
        public void BindWarehouse()
        {
            DataTable dt = GetWarehouseData();
            string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

            ddlWarehouse.Items.Clear();
            ddlWarehouse.DataSource = dt;
            ddlWarehouse.DataValueField = "WarehouseID";
            ddlWarehouse.DataTextField = "WarehouseName";
            ddlWarehouse.DataBind();

            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //DataTable dtdefaultStock = objEngine.GetDataTable("tbl_master_building", " top 1 bui_id ", " bui_BranchId='" + strBranch + "' ");
            //if (dtdefaultStock != null && dtdefaultStock.Rows.Count > 0)
            //{
            //    string defaultID = Convert.ToString(dtdefaultStock.Rows[0]["bui_id"]).Trim();

            //    if (defaultID != null || defaultID != "" || defaultID != "0")
            //    {
            //        hdndefaultWarehouse.Value = defaultID;
            //    }
            //}
        }

        #endregion

    }
}