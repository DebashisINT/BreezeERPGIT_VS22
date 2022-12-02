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


namespace Import.Import
{
    public partial class purchaseOrderAcceptance : System.Web.UI.Page
    {

        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
       
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        string UniquePurchaseNumber = string.Empty;


        ImportSalesActivitiesBL objSlaesActivitiesBL = new ImportSalesActivitiesBL();
        ImportPurchaseOrderacceptanceBL objPurchaseOrderBL = new ImportPurchaseOrderacceptanceBL();


        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        GSTtaxDetails gstTaxDetails = new GSTtaxDetails();
        public string pageAccess = "";
        static string ForJournalDate = null;   
       

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {            
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }


        protected void Page_Init(object sender, EventArgs e) // lead add
        {
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlIndentRequisitionNo.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Sqlvendor.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCurrency.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DS_Branch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DS_AmountAre.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectPin.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
           
            if (e.Column.FieldName == "gvColUOM")
            {
                e.Editor.ReadOnly = true;
            }

            if (e.Column.FieldName == "gvColTotalAmountINR")
            {
                e.Editor.ReadOnly = true;
            }

            if (e.Column.FieldName == "gvColStockPurchasePriceNetamountbase")
            {
                e.Editor.ReadOnly = true;
            }

            if (e.Column.FieldName == "gvColQuantity")
            {
                e.Editor.ReadOnly = true;
            }

            if (e.Column.FieldName == "ProductName")
            {
                e.Editor.ReadOnly = true;
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {

            //rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Import/PurchaseOrderList-Import.aspx");
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (ddlInventory.SelectedValue == "Y")
            {
                //lookup_quotation.ClientEnabled = true;
                taggingList.ClientEnabled = true;
            }
            else
            {
                ///lookup_quotation.ClientEnabled = false;
                taggingList.ClientEnabled = false;
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
                SetFinYearCurrentDate();
                string branchHierchy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);

                GetAllDropDownDetailForPurchaseOrder(branchHierchy);

                BindBranch();
                //string finyear = Convert.ToString(Session["LastFinYear"]);
                ///Session["CustomerDetail"] = null;
                Session["OrderAcceptanceId"] = null;
                Session["OrderAcceptancerDetails"] = null;
                //Session["LoopWarehouse"] = 1;
                Session["POTaxDetails"] = null;
                Session["PurchaseOrderAddressDtl"] = null;
                Session["POwarehousedetailstemp"] = null;
                Session["POwarehousedetailstempUpdate"] = null;
                Session["POwarehousedetailstempDelete"] = null;

                PopulateGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                PopulateChargeGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                if (Request.QueryString["key"] == "ADD")
                {
                    ddlInventory.SelectedValue = Convert.ToString(Request.QueryString["InvType"]);
                    ddlInventory.Enabled = false;

                    //if (Convert.ToString(Request.QueryString["InvType"]) == "Y") taggingList.ClientEnabled = true;
                    //else taggingList.ClientEnabled = false;

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
                    hdnPageStatus.Value = "first";
                    ddl_AmountAre.Value = "3";
                    ddl_VatGstCst.ClientEnabled = false;
                    //.................Tax..............
                    Session["POTaxDetails"] = null;
                    CreateDataTaxTable();
                    //..........End Tax-----------


                    DataTable Transactiondt = CreateTempTable("Transaction");
                    Transactiondt.Rows.Add("1", "1", "", "", "", "", "", "0.0000", "", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0", "0", "", "", "", "", "I");

                    Session["OrderAcceptancerDetails"] = Transactiondt;
                    grid.DataSource = GetPurchaseOrderBatch(Transactiondt);
                    grid.DataBind();

                }
                else
                {
                    txtVoucherNo.Enabled = false;
                    ddlInventory.Enabled = false;
                    ddl_Branch.Enabled = false;
                    txtVendorName.ClientEnabled = false;
                    dt_PLQuote.ClientEnabled = false;


                    ddl_AmountAre.ClientEnabled = false;
                    txt_Rate.ReadOnly = true;


                    Keyval_internalId.Value = "PurchaseOrder" + Request.QueryString["key"];
                    hdnPageStatus.Value = "Quoteupdate";
                    lblHeading.Text = "Modify Import Order Acceptance(Indent)";
                    divNumberingScheme.Style.Add("display", "none");
                    lbl_NumberingScheme.Visible = false;
                    ddl_numberingScheme.Visible = false;
                    Session["OrderAcceptanceId"] = Request.QueryString["key"];
                    ViewState["ActionType"] = "Edit";
                    btn_SaveRecords.ClientVisible = false;                   
                    DataSet Ds = GetPurchaseOrderData();
                    Session["OrderAcceptancerDetails"] = Ds.Tables[0];

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
                        Session["PurchaseOrderFinalTaxRecord"] = quotetable;
                    }
                    //..........End Tax-----------
                    grid.DataSource = GetPurchaseOrderBatch();
                    grid.DataBind();                   

                    #region Samrat Roy -- Hide Save Button in Edit Mode
                    if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                    {
                        lblHeading.Text = "View Purchase Order";
                        lbl_quotestatusmsg.Text = "*** View Mode Only";
                        btn_SaveRecords.ClientVisible = false;
                        btnSaveExit.ClientVisible = false;
                        lbl_quotestatusmsg.Visible = true;
                    }
                    #endregion

                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);
                }

                ///Mail Visibility
               // VisiblitySendEmail();



            }

        }
        public DataTable CreateTempTable(string Type)
        {

            DataTable PurchaseOrderdt = new DataTable();

            if (Type == "Transaction")
            {
                PurchaseOrderdt.Columns.Add("SrlNo", typeof(string));//1
                PurchaseOrderdt.Columns.Add("OrderDetails_Id", typeof(string));//2
                PurchaseOrderdt.Columns.Add("ProductID", typeof(string));//3
                PurchaseOrderdt.Columns.Add("Description", typeof(string));//4
                PurchaseOrderdt.Columns.Add("Quantity", typeof(string));//5
                PurchaseOrderdt.Columns.Add("UOM", typeof(string));//6
                PurchaseOrderdt.Columns.Add("Warehouse", typeof(string));//7
                PurchaseOrderdt.Columns.Add("StockQuantity", typeof(string));//8
                PurchaseOrderdt.Columns.Add("StockUOM", typeof(string));//9
                PurchaseOrderdt.Columns.Add("PurchasePrice", typeof(string));//10
                ////////////1. Changes 25-06-2018   Sudip Pal
                PurchaseOrderdt.Columns.Add("PurchasePriceforgn", typeof(string));//11
                ////////////1. Changes 25-06-2018   Sudip Pal
                PurchaseOrderdt.Columns.Add("Discount", typeof(string));//12
                PurchaseOrderdt.Columns.Add("Amount", typeof(string));//13
                ////////////A. Changes 27-06-2018   Sudip Pal
                PurchaseOrderdt.Columns.Add("Amountbase", typeof(string));//14
                ////////////A. Changes 27-06-2018   Sudip Pal
                PurchaseOrderdt.Columns.Add("TaxAmount", typeof(string));//15
                PurchaseOrderdt.Columns.Add("TotalAmount", typeof(string));//16
                ////////////3. Changes 25-06-2018   Sudip Pal
                PurchaseOrderdt.Columns.Add("TotalAmountBase", typeof(string));//17
                ////////////3. Changes 25-06-2018   Sudip Pal
                PurchaseOrderdt.Columns.Add("ImportPurOrder_ID", typeof(string));//18
                PurchaseOrderdt.Columns.Add("ImportPurOrdeDetails_Id", typeof(string));//19
                PurchaseOrderdt.Columns.Add("ProductName", typeof(string));//20
                PurchaseOrderdt.Columns.Add("IsComponentProduct", typeof(string));//21
                PurchaseOrderdt.Columns.Add("IsLinkedProduct", typeof(string));//22
                PurchaseOrderdt.Columns.Add("Status", typeof(string));//23
               

            }
            return PurchaseOrderdt;
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
        public void BindBranch()
        {
            DS_Branch.SelectCommand = "select BANKBRANCH_ID,BANKBRANCH_NAME from ( SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + "))tbl" +
            " order by BANKBRANCH_NAME asc";
            ddl_Branch.DataBind();
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
            DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "70", "N");
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


            public string gvColTaxAmount { get; set; }
            public string gvColTotalAmountINR { get; set; }
            public string Quotation_No { get; set; }
            public string ProductName { get; set; }
            public string Indent_Num { get; set; }
            public string ImportPurOrder_ID { get; set; }
            public string ImportPurOrdeDetails_Id { get; set; }
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
            public string ImportPurOrder_ID { get; set; }
            public string ImportPurOrdeDetails_Id { get; set; }
            
            public string gvColStockPurchasePriceforeign { get; set; }

           
            public string gvColStockPurchasePriceNetamountbase { get; set; }

          

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
            DataSet PurchaseOrderEditdtset = GetPurchaseOrderEditData();
            DataTable PurchaseOrderEditdt = PurchaseOrderEditdtset.Tables[0];
            if (PurchaseOrderEditdt != null && PurchaseOrderEditdt.Rows.Count > 0)
            {
                string PurchaseOrder_Number = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrderAcceptance_Number"]);//0
                string PurchaseOrder_IndentId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_Ids"]);//1
                string PurchaseOrder_BranchId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrderAcceptance_BranchId"]);//2
                TermsConditionsControl.SetBranchId(PurchaseOrder_BranchId);  // Mantis Issue #16920
                string PurchaseOrder_Date = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrderAcceptance_Date"]);//3

                string Customer_Id = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrderAcceptance_VendorId"]);//5
                string Contact_Person_Id = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Contact_Person_Id"]);//6
                string PurchaseOrder_Reference = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrderAcceptance_Reference"]);//7
                string PurchaseOrder_Currency_Id = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrderAcceptance_Currency_Id"]);//8
                string Currency_Conversion_Rate = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Currency_Conversion_Rate"]);//9
                string Tax_Option = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Tax_Option"]);//10
                string Tax_Code = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Tax_Code"]);//11
                string PurchaseOrder_SalesmanId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrderAcceptance_SalesmanId"]);//12
                string PurchaseOrder_IndentDate = Convert.ToString(PurchaseOrderEditdt.Rows[0]["IndentDate"]);//13
                string IsInventory = Convert.ToString(PurchaseOrderEditdt.Rows[0]["IsInventory"]);//13
                string PurchaseOrderDueDate = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PODueDate"]);//13
                string CreditDays = Convert.ToString(PurchaseOrderEditdt.Rows[0]["CreditDays"]);//13
                string VendorName = Convert.ToString(PurchaseOrderEditdt.Rows[0]["VendorName"]);//13


                string PurchaseOrderArrival_Date = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrderAcceptance_Expectdtdispatch"]);//3
                string PurchaseOrderdispatch_Date = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrderAcceptance_Expectdtarrival"]);//3
                string vesselvoygeno = Convert.ToString(PurchaseOrderEditdt.Rows[0]["VesselNumber"]);//3

                ///Changes 26-06-2018 Sudip Pal

                string VendorId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrderAcceptance_VendorId"]);//13

                string totalamtbase = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrderAcceptance_TotalAmountLocal"]);//13
                string docamountbase = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrderAcceptance_DocumentAmountLocal"]);//13
                string totalamtforeign = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrderAcceptance_TotalAmountforeign"]);//13


                string rcmchk = Convert.ToString(PurchaseOrderEditdt.Rows[0]["IsRcm"]);//19
                string entrydate = Convert.ToString(PurchaseOrderEditdt.Rows[0]["CreatedDate"]);

                if (rcmchk == "False")
                {
                    chkRCM.Checked = false;
                }
                else
                {
                    chkRCM.Checked = true;
                }
                dt_EntryDate.Date = Convert.ToDateTime(entrydate);
                dt_EntryDate.ClientEnabled = false;
                txt_docnetamt.Text = String.Format("{0:0.00}", docamountbase); ;
                txt_totamont.Text = String.Format("{0:0.00}", totalamtbase); 
                txt_totamont_foreign.Text = totalamtforeign;
                ///Changes 26-06-2018 Sudip Pal

                ///Changes 23-07-2018 Sudip Pal
                string Numberimportorder = Convert.ToString(PurchaseOrderEditdt.Rows[0]["importorder"]);//13
                string Numberimportorderdate = Convert.ToString(PurchaseOrderEditdt.Rows[0]["importdate"]);//13
                taggingList.Text = Numberimportorder;
                txtvessalno.Text = vesselvoygeno;
                ///Changes 23-07-2018 Sudip Pal


                taggingList.ClientEnabled = false;
                ddlInventory.SelectedValue = IsInventory;
                txtVoucherNo.Text = PurchaseOrder_Number;
                dt_PLQuote.Date = Convert.ToDateTime(PurchaseOrder_Date);
                dt_Quotation.Text = (Numberimportorderdate);

                if (!string.IsNullOrEmpty(PurchaseOrderArrival_Date))
                {
                    dt_Dateofdispatch.Date = Convert.ToDateTime(PurchaseOrderArrival_Date);
                }
                if (!string.IsNullOrEmpty(PurchaseOrderdispatch_Date))
                {
                    dt_Dateofarrival.Date = Convert.ToDateTime(PurchaseOrderdispatch_Date);
                }
              
                if (!string.IsNullOrEmpty(PurchaseOrder_IndentDate))
                {                    
                    dt_Quotation.Text = PurchaseOrder_IndentDate;
                }
              
                if (Customer_Id != "")
                {
                    hdfLookupCustomer.Value = Customer_Id;                  

                }
                hdnCustomerId.Value = Customer_Id;
                ProcedureExecute GetVendorGstin = new ProcedureExecute("prc_GstTaxDetails");
                GetVendorGstin.AddVarcharPara("@Action", 500, "GetVendorGSTINByBranch");
                GetVendorGstin.AddVarcharPara("@branchId", 10, Convert.ToString(ddl_Branch.SelectedValue));
                GetVendorGstin.AddVarcharPara("@entityId", 10, Convert.ToString(Customer_Id));
                DataTable VendorGstin = GetVendorGstin.GetTable();

                if (VendorGstin.Rows.Count > 0)
                {
                    if (Convert.ToString(VendorGstin.Rows[0][0]).Trim() != "")
                    {
                        //this.BillingShippingControl.hfVendorGSTIN.Value = Convert.ToString(VendorGstin.Rows[0][0]).Trim();
                        string VendorGst = Convert.ToString(VendorGstin.Rows[0][0]).Trim();
                        this.Purchase_BillingShipping.GetGSTIN(VendorGst);
                    }
                }
                txtVendorName.Text = VendorName.Trim();
                TabPage page = ASPxPageControl1.TabPages.FindByName("Billing/Shipping");
                page.ClientEnabled = true;
                PopulateContactPersonOfCustomer(Customer_Id);
                if (Contact_Person_Id != "0")
                {
                    cmbContactPerson.Value = Contact_Person_Id;

                    DataTable dtContactPersonPhone = new DataTable();

                    ///Changes  03-07-2018  Sudip Pal
                    dtContactPersonPhone = objPurchaseOrderBL.PopulateContactPersonPhone(Contact_Person_Id);
                   
                    ///Changes  03-07-2018  Sudip Pal

                }

                txt_Refference.Text = PurchaseOrder_Reference;
                ddl_Branch.SelectedValue = PurchaseOrder_BranchId;
                //ddl_SalesAgent.SelectedValue = PurchaseOrder_SalesmanId;
                ddl_Currency.SelectedValue = PurchaseOrder_Currency_Id;
                txt_Rate.Text = Currency_Conversion_Rate;

                #region Indent Tagging & Product Tagging
                string Quoids = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_Ids"]);

                if (!String.IsNullOrEmpty(Quoids))
                {
                    BindLookUp(PurchaseOrder_Date, PurchaseOrder_BranchId, VendorId);
                    string[] eachQuo = Quoids.Split(',');
                    string QuoComponent = "", QuoComponentNumber = "", QuoComponentDate = "";

                    for (int i = 0; i < taggingGrid.VisibleRowCount; i++)
                    {
                        string PurchaseOrder_Id = Convert.ToString(taggingGrid.GetRowValues(i, "ComponentID"));
                        if (eachQuo.Contains(PurchaseOrder_Id))
                        {
                            QuoComponent += "," + Convert.ToString(taggingGrid.GetRowValues(i, "ComponentID"));
                            QuoComponentNumber += "," + Convert.ToString(taggingGrid.GetRowValues(i, "ComponentNumber"));
                            QuoComponentDate += "," + Convert.ToString(taggingGrid.GetRowValues(i, "ComponentDate"));

                            taggingGrid.Selection.SelectRow(i);
                        }
                    }
                    QuoComponent = QuoComponent.TrimStart(',');
                    QuoComponentNumber = QuoComponentNumber.TrimStart(',');
                    QuoComponentDate = QuoComponentDate.TrimStart(',');

                    if (taggingGrid.GetSelectedFieldValues("ComponentID").Count > 0)
                    {
                        if (taggingGrid.GetSelectedFieldValues("ComponentID").Count > 1)
                        {
                            QuoComponentDate = "Multiple Purchase Order Dates";
                        }
                    }
                    else
                    {
                        QuoComponentDate = "";
                    }

                    //dt_Quotation.Text = QuoComponentDate;
                    //taggingList.Text = QuoComponentNumber;


                    DataTable dt_QuotationDetails = new DataTable();
                    string IdKey = Convert.ToString(Request.QueryString["key"]);
                    dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsFromPO(QuoComponent, IdKey, "", "Edit");

                    grid_Products.DataSource = GetProductsInfo(dt_QuotationDetails);
                    grid_Products.DataBind();

                    //DataTable Transaction_dt = (DataTable)Session["OrderAcceptancerDetails"];

                    //for (int i = 0; i < grid_Products.VisibleRowCount; i++)
                    //{
                    //    string ComponentID = Convert.ToString(grid_Products.GetRowValues(i, "Quotation_No"));
                    //    string ProductList = Convert.ToString(grid_Products.GetRowValues(i, "gvColProduct"));

                    //    string[] list = ProductList.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    //    string ProductID = Convert.ToString(list[0]) + "||@||%";

                    //    var Checkdt = Transaction_dt.Select("Indent_No='" + ComponentID + "' AND ProductID LIKE '" + ProductID + "'");
                    //    if (Checkdt.Length > 0)
                    //    {
                    //        grid_Products.Selection.SelectRow(i);
                    //    }
                    //}
                }
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
                ddlPosGstInvoice.DataSource = PurchaseOrderEditdtset.Tables[1];
                ddlPosGstInvoice.ValueField = "ID";
                ddlPosGstInvoice.TextField = "Name";
                ddlPosGstInvoice.DataBind();
                string PosForGst = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PosForGst"]);
                ddlPosGstInvoice.Value = PosForGst;
                ddlPosGstInvoice.ClientEnabled = false;
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
                PurchaseOrderList.Add(PurchaseOrders);
            }

            return PurchaseOrderList;
        }
        public DataSet GetPurchaseOrderData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderAcceptanceDetailsList_Import");
            proc.AddVarcharPara("@Action", 500, "PoBatchEditDetails");
            proc.AddVarcharPara("@PurchaseOrder_Id", 500, Convert.ToString(Session["OrderAcceptanceId"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(Session["LastCompany"]));
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetPurchaseOrderEditData()
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderAcceptanceDetailsList_Import");
            proc.AddVarcharPara("@Action", 500, "PurchaseOrderEditDetails");
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["OrderAcceptanceId"]));
            dt = proc.GetDataSet();
            return dt;
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
        public void GetAllDropDownDetailForPurchaseOrder(string branchHierchy)
        {
            DataSet dst = new DataSet();
            dst = objPurchaseOrderBL.GetAllDropDownDetailForPurchaseOrder(branchHierchy);          

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
            if (dst.Tables[4] != null && dst.Tables[4].Rows.Count > 0)
            {
                ddl_numberingScheme.DataSource = dst.Tables[4];
                ddl_numberingScheme.DataBind();
            }

            //ddl_SalesAgent.Items.Insert(0, new ListItem("Select", "0"));

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
            //string ContactPerson = "";
            DataTable dtContactPerson = new DataTable();
            dtContactPerson = objPurchaseOrderBL.PopulateContactPersonOfCustomer(InternalId);
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
                grid.Columns[15].Caption = "Net Amt [" + e.Parameters.Split('~')[1] + "]";
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
                    DataTable POdt = (DataTable)Session["OrderAcceptancerDetails"];
                    grid.DataSource = GetPurchaseOrderBatch(POdt);

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
                grid.Columns[15].Caption = "Net Amt [" + e.Parameters.Split('~')[1] + "]";
                grid.Columns[12].Caption = "Amt [" + e.Parameters.Split('~')[1] + "]";
                grid.Columns[13].Caption = "Amt [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";

                grid.JSProperties["cpnull"] = "no";
            }
            else if (strSplitCommand == "GridBlank")
            {
                Session["OrderAcceptancerDetails"] = null;
                grid.DataSource = null;
                grid.DataBind();


                grid.JSProperties["cpnull"] = "no";
            }
            else if (strSplitCommand == "BindGridOnQuotation")
            {

                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                String QuoComponent1 = "";
                string Product_id1 = "";
                string QuoteDetails_Id = "";

                if (grid_Products.GetSelectedFieldValues("ComponentID").Count > 0)


                {
                    for (int i = 0; i < grid_Products.GetSelectedFieldValues("ComponentID").Count; i++)
                    {

                        QuoComponent1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ComponentID")[i]);
                        Product_id1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ProductID")[i]);
                        QuoteDetails_Id += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ComponentDetailsID")[i]);

                    }



                    QuoComponent1 = QuoComponent1.TrimStart(',');
                    ///Changes 20-07-2018   Sudip Pal
                    QuoComponent1 = QuoComponent1.Split(',')[0];
                    ///Changes 20-07-2018   Sudip Pal
                    Product_id1 = Product_id1.TrimStart(',');
                    QuoteDetails_Id = QuoteDetails_Id.TrimStart(',');


                    string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);

                    if (Quote_Nos != "$")
                    {

                        DataSet dt_QuotationDetails = new DataSet();
                        string IdKey = Convert.ToString(Request.QueryString["key"]);
                        if (!string.IsNullOrEmpty(IdKey))
                        {

                            if (IdKey != "ADD")
                            {
                                dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsForPOGridBind(QuoComponent1, QuoteDetails_Id, Product_id1, "Edit");
                            }
                            else
                            {
                                dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsForPOGridBind(QuoComponent1, QuoteDetails_Id, Product_id1, "Add");
                            }

                        }
                        else
                        {
                            //dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsForPOGridBind(QuoComponent1, QuoteDetails_Id, Product_id1);
                        }
                        Session["OrderAcceptancerDetails"] = null;

                        grid.DataSource = GetImportPurchaseOrderInfo(dt_QuotationDetails.Tables[0], dt_QuotationDetails.Tables[2], IdKey);
                        grid.DataBind();

                        grid.JSProperties["cpcurrencyId"] = dt_QuotationDetails.Tables[1].Rows[0]["PurchaseOrder_Currency_Id"];
                        grid.JSProperties["cpcurrencyrate"] = dt_QuotationDetails.Tables[1].Rows[0]["Currency_Conversion_Rate"];
                        grid.JSProperties["cpTax_Option"] = dt_QuotationDetails.Tables[1].Rows[0]["Tax_Option"];
                        grid.JSProperties["cpCreditDays"] = dt_QuotationDetails.Tables[1].Rows[0]["CreditDays"];

                        if (!string.IsNullOrEmpty(Convert.ToString(dt_QuotationDetails.Tables[1].Rows[0]["DueDate"])))
                        {
                            grid.JSProperties["cpDueDate"] = Convert.ToDateTime(dt_QuotationDetails.Tables[1].Rows[0]["DueDate"]).ToString("yyyy-MM-dd");
                        }

                        grid.JSProperties["cpTotalAmountfrgn"] = dt_QuotationDetails.Tables[1].Rows[0]["PurchaseOrder_TotalAmountforeign"];
                        grid.JSProperties["cpTotalAmountLocal"] = dt_QuotationDetails.Tables[1].Rows[0]["PurchaseOrder_TotalAmountLocal"];
                        grid.JSProperties["cpTotaldocAmtLocal"] = dt_QuotationDetails.Tables[1].Rows[0]["PurchaseOrder_DocumentAmountLocal"];
                        grid.JSProperties["cpcurrenynamel"] = dt_QuotationDetails.Tables[1].Rows[0]["CurrencyName"];

                        grid.JSProperties["cpNewField"] = dt_QuotationDetails.Tables[1].Rows[0]["PlaceOfSupply"] + "~" + dt_QuotationDetails.Tables[1].Rows[0]["CreatedDate"] + "~" + dt_QuotationDetails.Tables[1].Rows[0]["IsRcm"] + "~" + dt_QuotationDetails.Tables[1].Rows[0]["PosForGst"];

                        grid.Columns[10].Caption = "Price [" + dt_QuotationDetails.Tables[1].Rows[0]["CurrencyName"] + "]";
                        grid.Columns[9].Caption = "Price [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";
                        grid.Columns[16].Caption = "Net Amt [" + Convert.ToString(Session["LocalCurrency"]).Split('~')[1].Trim() + "]";
                        grid.Columns[15].Caption = "Net Amt [" + dt_QuotationDetails.Tables[1].Rows[0]["CurrencyName"] + "]";
                        grid.Columns[12].Caption = "Amt [" + dt_QuotationDetails.Tables[1].Rows[0]["CurrencyName"] + "]";
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
                    //Transactiondt.Rows.Add("1", "1", "", "", "", "", "", "0.0000", "", "0.00", "0.00", "0.00", "0.00", "0.00", "0", "0", "0", "", "", "I");
                    Transactiondt.Rows.Add("1", "1", "", "", "", "", "", "0.0000", "", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0", "0", "", "", "", "", "I");
                    Session["OrderAcceptancerDetails"] = Transactiondt;


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

                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["ImportPurOrder_ID"])))//Added on 15-02-2017
                { 
                    Orders.ImportPurOrder_ID = Convert.ToString(SalesOrderdt1.Rows[i]["ImportPurOrder_ID"]);
                    Orders.ImportPurOrdeDetails_Id = Convert.ToString(SalesOrderdt1.Rows[i]["ImportPurOrdeDetails_Id"]); 
                }
                else
                { 
                    Orders.ImportPurOrder_ID = "";
                    Orders.ImportPurOrdeDetails_Id = "";
                }
                if (dtC.Contains("ImportPurOrder_ID"))
                {
                    Orders.Indent_Num = Convert.ToString(SalesOrderdt1.Rows[i]["ImportPurOrder_ID"]);//subhabrata on 21-02-2017
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
            purChaseDT.Columns.Add("TaxAmount", typeof(string));
            purChaseDT.Columns.Add("TotalAmount", typeof(string));
            purChaseDT.Columns.Add("TotalAmountBase", typeof(string));
            purChaseDT.Columns.Add("Status", typeof(string));
            purChaseDT.Columns.Add("ImportPurOrder_ID", typeof(string));
            purChaseDT.Columns.Add("ImportPurOrdeDetails_Id", typeof(string));
            purChaseDT.Columns.Add("ProductName", typeof(string));
            purChaseDT.Columns.Add("IsComponentProduct", typeof(string));
            purChaseDT.Columns.Add("IsLinkedProduct", typeof(string));         
            purChaseDTtaxdetails.Columns.Add("slNo", typeof(string));
            purChaseDTtaxdetails.Columns.Add("TaxCode", typeof(string));
            purChaseDTtaxdetails.Columns.Add("AltTaxCode", typeof(string));
            purChaseDTtaxdetails.Columns.Add("Percentage", typeof(string));
            purChaseDTtaxdetails.Columns.Add("Amount", typeof(string));


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IsSuccess = true;
                DataColumnCollection dtC = dt.Columns;
                string SalePrice, Discount, Amount, TaxAmount, TotalAmount, Order_Num1,IPODetailsId ,ProductName, IsComponent, IsLinkedProduct, PurchasePriceforgn, TotalAmountBase, Amountbase;
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
                { TaxAmount = Convert.ToString(dt.Rows[i]["TaxAmount"]); }
                else { TaxAmount = ""; }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalAmount"])))
                { TotalAmount = Convert.ToString(dt.Rows[i]["TotalAmount"]); }
                else { TotalAmount = ""; }
                if (dtC.Contains("ImportPurOrder_ID"))
                {
                    Order_Num1 = Convert.ToString(dt.Rows[i]["ImportPurOrder_ID"]);
                    IPODetailsId = Convert.ToString(dt.Rows[i]["ImportPurOrdeDetails_Id"]);
                }
                else
                {
                    Order_Num1 = "";
                    IPODetailsId = "";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Products_Name"])))
                {
                    ProductName = Convert.ToString(dt.Rows[i]["Products_Name"]);
                }
                else
                {
                    ProductName = "";
                }              
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

                purChaseDT.Rows.Add(Convert.ToString(i + 1), Convert.ToString(i + 1), Convert.ToString(dt.Rows[i]["Products_ID"]), Convert.ToString(dt.Rows[i]["QuoteDetails_ProductDescription"]),
                  Convert.ToString(dt.Rows[i]["QuoteDetails_Quantity"]), Convert.ToString(dt.Rows[i]["UOM_ShortName"]), "", Convert.ToString(dt.Rows[i]["QuoteDetails_StockQty"]), Convert.ToString(dt.Rows[i]["UOM_ShortName"]),
                  SalePrice, PurchasePriceforgn, Discount, Amount, Amountbase, TaxAmount, TotalAmount, TotalAmountBase, "U", Convert.ToString(dt.Rows[i]["ImportPurOrder_ID"]), Convert.ToString(dt.Rows[i]["ImportPurOrdeDetails_Id"]), ProductName, IsComponent, IsLinkedProduct);

                DataRow[] result = dttaxdetails.Select("ProductTax_ProductId =" + Convert.ToString(dt.Rows[i]["QuoteDetails_Id"]));

                foreach (DataRow row in result)
                {
                    purChaseDTtaxdetails.Rows.Add(Convert.ToString(i + 1),row["TaxCode"],0,row["Percentage"], row["Amount"]);

                }
            }
            Session["OrderAcceptancerDetails"] = purChaseDT;
            Session["PurchaseOrderFinalTaxRecord"] = purChaseDTtaxdetails;
            return IsSuccess;
        }
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
           
            if (Session["OrderAcceptancerDetails"] != null)
            {
                DataTable POdt = (DataTable)Session["OrderAcceptancerDetails"];
                DataView dvData = new DataView(POdt);
                dvData.RowFilter = "Status <> 'D'";
                grid.DataSource = GetPurchaseOrderBatch(dvData.ToTable());
            }
        }
        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {

            ddl_AmountAre.Value = HDNtaxtypeamt.Value;

            grid.JSProperties["cpSaveSuccessOrFail"] = null;
            grid.JSProperties["cpPurchaseOrderNo"] = null;
            DataTable PurchaseOrderdt = new DataTable();
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);

            if (Session["OrderAcceptancerDetails"] != null)
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["OrderAcceptancerDetails"];
                foreach (DataRow row in dt.Rows)
                {
                    DataColumnCollection dtC = dt.Columns;

                    if (dtC.Contains("Indent_Num"))
                    { dt.Columns.Remove("Indent_Num"); }
                    break;
                }
                PurchaseOrderdt = dt;
            }
            else
            {
                PurchaseOrderdt.Columns.Add("SrlNo", typeof(string));
                PurchaseOrderdt.Columns.Add("OrderDetails_Id", typeof(string));
                PurchaseOrderdt.Columns.Add("ProductID", typeof(string));
                PurchaseOrderdt.Columns.Add("Description", typeof(string));
                PurchaseOrderdt.Columns.Add("Quantity", typeof(string));
                PurchaseOrderdt.Columns.Add("UOM", typeof(string));
                PurchaseOrderdt.Columns.Add("Warehouse", typeof(string));
                PurchaseOrderdt.Columns.Add("StockQuantity", typeof(string));
                PurchaseOrderdt.Columns.Add("StockUOM", typeof(string));
                PurchaseOrderdt.Columns.Add("PurchasePrice", typeof(string));

                ////////////1. Changes 25-06-2018   Sudip Pal
                PurchaseOrderdt.Columns.Add("PurchasePriceforgn", typeof(string));
                ////////////1. Changes 25-06-2018   Sudip Pal

                PurchaseOrderdt.Columns.Add("Discount", typeof(string));
                PurchaseOrderdt.Columns.Add("Amount", typeof(string));

                ////////////A. Changes 27-06-2018   Sudip Pal
                PurchaseOrderdt.Columns.Add("Amountbase", typeof(string));
                ////////////A. Changes 27-06-2018   Sudip Pal


                PurchaseOrderdt.Columns.Add("TaxAmount", typeof(string));
                PurchaseOrderdt.Columns.Add("TotalAmount", typeof(string));


                ////////////3. Changes 25-06-2018   Sudip Pal
                PurchaseOrderdt.Columns.Add("TotalAmountBase", typeof(string));
                ////////////3. Changes 25-06-2018   Sudip Pal


                PurchaseOrderdt.Columns.Add("Status", typeof(string));

                PurchaseOrderdt.Columns.Add("ImportPurOrder_ID", typeof(string));
                PurchaseOrderdt.Columns.Add("ImportPurOrdeDetails_Id", typeof(string));

                PurchaseOrderdt.Columns.Add("ProductName", typeof(string));
                PurchaseOrderdt.Columns.Add("IsComponentProduct", typeof(string));
                PurchaseOrderdt.Columns.Add("IsLinkedProduct", typeof(string));
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
                    //string UOM = Convert.ToString(ProductDetailsList[3]);
                    string UOM = Convert.ToString(args.NewValues["gvColUOM"]);
                    string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
                    string StockQuantity = Convert.ToString(args.NewValues["gvColStockQty"]);
                    string StockUOM = Convert.ToString(args.NewValues["gvColStockUOM"]);
                    // string StockUOM = Convert.ToString(ProductDetailsList[5]);
                    string PurchasePrice = Convert.ToString(args.NewValues["gvColStockPurchasePrice"]);

                    ////////////4. Changes 25-06-2018   Sudip Pal
                    string PurchasePriceforgn = Convert.ToString(args.NewValues["gvColStockPurchasePriceforeign"]);
                    ////////////3. Changes 25-06-2018   Sudip Pal

                    // string PurchasePrice = Convert.ToString(ProductDetailsList[6]);
                    string Discount = Convert.ToString(args.NewValues["gvColDiscount"]);
                    string Amount = (Convert.ToString(args.NewValues["gvColAmount"]) != "") ? Convert.ToString(args.NewValues["gvColAmount"]) : "0";


                    ////////////A. Changes 27-06-2018   Sudip Pal
                    string Amountbase = (Convert.ToString(args.NewValues["gvColAmountbase"]) != "") ? Convert.ToString(args.NewValues["gvColAmountbase"]) : "0";
                    ////////////A. Changes 27-06-2018   Sudip Pal


                    string TaxAmount = (Convert.ToString(args.NewValues["gvColTaxAmount"]) != "") ? Convert.ToString(args.NewValues["gvColTaxAmount"]) : "0";
                    string TotalAmount = (Convert.ToString(args.NewValues["gvColTotalAmountINR"]) != "") ? Convert.ToString(args.NewValues["gvColTotalAmountINR"]) : "0";



                    ////////////5. Changes 25-06-2018   Sudip Pal
                    string TotalAmountbase = (Convert.ToString(args.NewValues["gvColStockPurchasePriceNetamountbase"]) != "") ? Convert.ToString(args.NewValues["gvColStockPurchasePriceNetamountbase"]) : "0";
                    ////////////5. Changes 25-06-2018   Sudip Pal



                    string ImportPurOrder_ID = (Convert.ToString(args.NewValues["ImportPurOrder_ID"]) != "") ? Convert.ToString(args.NewValues["ImportPurOrder_ID"]) : "0";
                    string ImportPurOrdeDetails_Id = (Convert.ToString(args.NewValues["ImportPurOrdeDetails_Id"]) != "") ? Convert.ToString(args.NewValues["ImportPurOrdeDetails_Id"]) : "0";
                    
                    string IsComponentProduct = (Convert.ToString(args.NewValues["IsComponentProduct"]) != "") ? Convert.ToString(args.NewValues["IsComponentProduct"]) : "N";
                    string IsLinkedProduct = (Convert.ToString(args.NewValues["IsLinkedProduct"]) != "") ? Convert.ToString(args.NewValues["IsLinkedProduct"]) : "N";


                    ////////////6. Changes 25-06-2018   Sudip Pal
                    //PurchaseOrderdt.Rows.Add(SrlNo, "0", ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, PurchasePrice, Discount,
                    //    Amount, TaxAmount, TotalAmount, "I", Indent_Id, ProductName, "", IsLinkedProduct);


                    PurchaseOrderdt.Rows.Add(SrlNo, "0", ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, PurchasePrice, PurchasePriceforgn, Discount,
                        Amount, Amountbase, TaxAmount, TotalAmount, TotalAmountbase, "I", ImportPurOrder_ID,ImportPurOrdeDetails_Id, ProductName, "", IsLinkedProduct);

                    ////////////5. Changes 25-06-2018   Sudip Pal

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

                            ////////////6. Changes 25-06-2018   Sudip Pal
                            string Prod_PurchasePriceforgn = Convert.ToString(drr["gvColStockPurchasePriceforeign"]);
                            ////////////6. Changes 25-06-2018   Sudip Pal



                            string Products_Name = Convert.ToString(drr["Products_Name"]);
                            string StkQty = Convert.ToString(Convert.ToDecimal(Quantity) * Convert.ToDecimal(Conversion_Multiplier));

                            //  PurchaseOrderdt.Rows.Add(Convert.ToString(Convert.ToInt32(SrlNo) + 1), "0", sProductsID, Products_Description, Quantity, Sales_UOM_Name, "", 
                            // StkQty, Stk_UOM_Name, Product_SalePrice, "0.0", "0.0", "0.0", "0.0", "I", Products_Name, ComponentID, ComponentName, "0.0", "0.0", "", "Y");

                            ////////////7. Changes 25-06-2018   Sudip Pal
                            //     PurchaseOrderdt.Rows.Add(Convert.ToString(Convert.ToInt32(SrlNo) + 1), "0", sProductsID, Products_Description, Quantity, Sales_UOM_Name,
                            //         Warehouse, StkQty, Stk_UOM_Name, Product_PurPrice, "0.0",
                            //"0.0", "0.0", "0.0", "I", Indent_Id, Products_Name, "", "Y");



                            PurchaseOrderdt.Rows.Add(Convert.ToString(Convert.ToInt32(SrlNo) + 1), "0", sProductsID, Products_Description, Quantity, Sales_UOM_Name,
                            Warehouse, StkQty, Stk_UOM_Name, Product_PurPrice, Prod_PurchasePriceforgn, "0.0",
                   "0.0", "0.0", "0.0", "0.0", "0.0", "I", ImportPurOrder_ID, ImportPurOrdeDetails_Id, Products_Name, "", "Y");
                            ////////////6. Changes 25-06-2018   Sudip Pal
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
                        //string UOM = Convert.ToString(ProductDetailsList[3]);
                        string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                        string UOM = Convert.ToString(args.NewValues["gvColUOM"]);
                        string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
                        string StockQuantity = Convert.ToString(args.NewValues["gvColStockQty"]);
                        string StockUOM = Convert.ToString(args.NewValues["gvColStockUOM"]);
                        //string StockUOM = Convert.ToString(ProductDetailsList[5]);
                        string PurchasePrice = Convert.ToString(args.NewValues["gvColStockPurchasePrice"]);

                        ////////////1. Changes 25-06-2018   Sudip Pal

                        string PurchasePriceforgn = Convert.ToString(args.NewValues["gvColStockPurchasePriceforeign"]);


                        ////////////Changes 25-06-2018   Sudip Pal


                        // string PurchasePrice = Convert.ToString(ProductDetailsList[6]);
                        string Discount = Convert.ToString(args.NewValues["gvColDiscount"]);
                        string Amount = (Convert.ToString(args.NewValues["gvColAmount"]) != "") ? Convert.ToString(args.NewValues["gvColAmount"]) : "0";


                        ////////////2. Changes 25-06-2018   Sudip Pal

                        string Amountbase = (Convert.ToString(args.NewValues["gvColAmountbase"]) != "") ? Convert.ToString(args.NewValues["gvColAmountbase"]) : "0";

                        ////////////Changes 25-06-2018   Sudip Pal



                        string TaxAmount = (Convert.ToString(args.NewValues["gvColTaxAmount"]) != "") ? Convert.ToString(args.NewValues["gvColTaxAmount"]) : "0";
                        string TotalAmount = (Convert.ToString(args.NewValues["gvColTotalAmountINR"]) != "") ? Convert.ToString(args.NewValues["gvColTotalAmountINR"]) : "0";

                        ////////////2. Changes 25-06-2018   Sudip Pal

                        string TotalAmountbase = (Convert.ToString(args.NewValues["gvColStockPurchasePriceNetamountbase"]) != "") ? Convert.ToString(args.NewValues["gvColStockPurchasePriceNetamountbase"]) : "0";


                        ////////////Changes 25-06-2018   Sudip Pal

                        string ImportPurOrder_ID = (Convert.ToString(args.NewValues["ImportPurOrder_ID"]) != "") ? Convert.ToString(args.NewValues["ImportPurOrder_ID"]) : "0";
                        string ImportPurOrdeDetails_Id = (Convert.ToString(args.NewValues["ImportPurOrdeDetails_Id"]) != "") ? Convert.ToString(args.NewValues["ImportPurOrdeDetails_Id"]) : "0";
                    

                        //string Indent_Id = (Convert.ToString(args.NewValues["Indent"]) != "") ? Convert.ToString(args.NewValues["Indent"]) : "0";
                        string IsComponentProduct = (Convert.ToString(args.NewValues["IsComponentProduct"]) != "") ? Convert.ToString(args.NewValues["IsComponentProduct"]) : "N";
                        string IsLinkedProduct = (Convert.ToString(args.NewValues["IsLinkedProduct"]) != "") ? Convert.ToString(args.NewValues["IsLinkedProduct"]) : "N";
                        bool Isexists = false;
                        foreach (DataRow drr in PurchaseOrderdt.Rows)
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


                                ////////////Changes 25-06-2018   Sudip Pal
                                drr["PurchasePriceforgn"] = PurchasePriceforgn;
                                ////////////Changes 25-06-2018   Sudip Pal


                                drr["Discount"] = Discount;
                                drr["Amount"] = Amount;


                                ////////////A. Changes 27-06-2018   Sudip Pal
                                drr["Amountbase"] = Amountbase;
                                ////////////A. Changes 27-06-2018   Sudip Pal


                                drr["TaxAmount"] = TaxAmount;
                                drr["TotalAmount"] = TotalAmount;

                                ////////////Changes 25-06-2018   Sudip Pal
                                drr["TotalAmountBase"] = TotalAmountbase;
                                ////////////Changes 25-06-2018   Sudip Pal


                                drr["Status"] = "U";
                                if (!string.IsNullOrEmpty(ImportPurOrder_ID))
                                { drr["ImportPurOrder_ID"] = ImportPurOrder_ID; }
                                drr["ImportPurOrdeDetails_Id"] = ImportPurOrdeDetails_Id;
                                
                                drr["ProductName"] = ProductName;
                                drr["IsComponentProduct"] = IsComponentProduct;
                                drr["IsLinkedProduct"] = IsLinkedProduct;
                                break;
                            }
                        }

                        if (Isexists == false)
                        {
                            PurchaseOrderdt.Rows.Add(SrlNo, OrderDetails_Id, ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM,
                                PurchasePrice, Discount, Amount, Amountbase, TaxAmount, TotalAmount, TotalAmountbase, "U", ImportPurOrder_ID, ImportPurOrdeDetails_Id, ProductName, IsComponentProduct, IsLinkedProduct);
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

                                ////////////9. Changes 25-06-2018   Sudip Pal

                                string Product_PurchasePriceforgn = Convert.ToString(args.NewValues["gvColStockPurchasePriceforeign"]);


                                ////////////Changes 25-06-2018   Sudip Pal




                                string Products_Name = Convert.ToString(drr["Products_Name"]);
                                string StkQty = Convert.ToString(Convert.ToDecimal(Quantity) * Convert.ToDecimal(Conversion_Multiplier));


                                ////////////10. Changes 25-06-2018   Sudip Pal

                                //     PurchaseOrderdt.Rows.Add(Convert.ToString(Convert.ToInt32(SrlNo) + 1), "0", sProductsID, Products_Description, Quantity, Sales_UOM_Name,
                                //         Warehouse, StkQty, Stk_UOM_Name, Product_PurPrice, "0.0",
                                //"0.0", "0.0", "0.0", "I", Indent_Id, ProductName, "", "Y");


                                PurchaseOrderdt.Rows.Add(Convert.ToString(Convert.ToInt32(SrlNo) + 1), "0", sProductsID, Products_Description, Quantity, Sales_UOM_Name,
                                    Warehouse, StkQty, Stk_UOM_Name, Product_PurPrice, Product_PurchasePriceforgn, "0.0",
                           "0.0", "0.0", "0.0", "0.0", "0.0", "I", ImportPurOrder_ID, ImportPurOrdeDetails_Id, ProductName, "", "Y");

                                ////////////9. Changes 25-06-2018   Sudip Pal

                            }
                        }
                    }
                }
            }

            foreach (var args in e.DeleteValues)
            {
                string OrderDetails_Id = Convert.ToString(args.Keys["OrderDetails_Id"]);

                for (int i = PurchaseOrderdt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = PurchaseOrderdt.Rows[i];
                    string delOrderDetailsId = Convert.ToString(dr["OrderDetails_Id"]);

                    if (delOrderDetailsId == OrderDetails_Id)
                        dr.Delete();
                }
                PurchaseOrderdt.AcceptChanges();

                if (OrderDetails_Id.Contains("~") != true)
                {
                    PurchaseOrderdt.Rows.Add("0", OrderDetails_Id, "0", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "D", "0","0", "", "", "");
                }
            }

            ///////////////////////

            int j = 1;
            foreach (DataRow dr in PurchaseOrderdt.Rows)
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
            PurchaseOrderdt.AcceptChanges();

            Session["OrderAcceptancerDetails"] = PurchaseOrderdt;
            if (IsDeleteFrom != "D" && IsDeleteFrom != "C")
            {
                string ActionType = Convert.ToString(ViewState["ActionType"]);
                string IsInventory = Convert.ToString(ddlInventory.SelectedValue);
                string PurchaseOrder_Id = Convert.ToString(Session["OrderAcceptanceId"]);
                string strSchemeType = Convert.ToString(ddl_numberingScheme.SelectedValue);
                string strPurchaseNumber = Convert.ToString(txtVoucherNo.Text);
                string strPurchaseDate = Convert.ToString(dt_PLQuote.Date);


                String IndentRequisitionNo = "";
                string IndentRequisitionDate = string.Empty;
                for (int i = 0; i < taggingGrid.GetSelectedFieldValues("ComponentID").Count; i++)
                {
                    IndentRequisitionNo += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("ComponentID")[i]);
                }
                IndentRequisitionNo = IndentRequisitionNo.TrimStart(',');

                if (taggingGrid.GetSelectedFieldValues("ComponentID").Count == 1)
                {
                    IndentRequisitionDate = Convert.ToString(dt_Quotation.Text);
                }

                //string strVendor = Convert.ToString(ddl_Vendor.SelectedValue);
                string strVendor = Convert.ToString(hdfLookupCustomer.Value);
                string strContactName = Convert.ToString(cmbContactPerson.Value);
                string Reference = Convert.ToString(txt_Refference.Text);

                //////Changes 12-06-2018 Sudip Pal
                string strBranch = Convert.ToString(HDNbranch.Value);
                //     string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

                //////Changes 12-06-2018 Sudip Pal


                //string strAgents = Convert.ToString(ddl_SalesAgent.SelectedValue);
                string strAgents = Convert.ToString("0");
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

                string PosForGst = Convert.ToString(ddlPosGstInvoice.Value);
                string entrydate = Convert.ToString(dt_EntryDate.Date);

                //////Changes 19-07-2018 Sudip Pal
                 string strexpectddatedispatchDate=null;
                if(!string.IsNullOrEmpty(dt_Dateofdispatch.Text))
                {

                    strexpectddatedispatchDate = Convert.ToString(dt_Dateofdispatch.Date);
                }

                string strexpectddatearriavalDate = null;
                if (!string.IsNullOrEmpty(dt_Dateofarrival.Text))
                {

                    strexpectddatearriavalDate = Convert.ToString(dt_Dateofarrival.Date);
                }
               
                //////Changes 12-06-2018 Sudip Pal


                DataTable tempQuotation = PurchaseOrderdt.Copy();



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

                #region DataTable of Inline Tax

                DataTable TaxDetaildt = new DataTable();
                if (Session["PurchaseOrderFinalTaxRecord"] != null)
                {
                    TaxDetaildt = (DataTable)Session["PurchaseOrderFinalTaxRecord"];
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

                #region DataTable of Warehouse

                DataTable tempWarehousedt = new DataTable();
                //if (Session["POWarehouseData"] != null)
                //{
                //    DataTable Warehousedt = (DataTable)Session["POWarehouseData"];
                //    tempWarehousedt = Warehousedt.DefaultView.ToTable(false, "Product_SrlNo", "LoopID", "WarehouseID", "TotalQuantity", "BatchID", "SerialID");
                //}
                //else
                //{
                //    tempWarehousedt.Columns.Add("Product_SrlNo", typeof(string));
                //    tempWarehousedt.Columns.Add("SrlNo", typeof(string));
                //    tempWarehousedt.Columns.Add("WarehouseID", typeof(string));
                //    tempWarehousedt.Columns.Add("TotalQuantity", typeof(string));
                //    tempWarehousedt.Columns.Add("BatchID", typeof(string));
                //    tempWarehousedt.Columns.Add("SerialID", typeof(string));
                //}

                #endregion Warehouse End
                string validate = "";

                // DataTable Of Billing Address
                #region ##### Added By : Samrat Roy -- to get BillingShipping user control data
                DataTable tempBillAddress = new DataTable();
                //tempBillAddress = Purchase_BillingShipping.SaveBillingShippingControlData();
                tempBillAddress = Purchase_BillingShipping.GetBillingShippingTable();
                #region #### Old_Process ####
                ////DataTable tempBillAddress = new DataTable();
                ////if (Session["PurchaseOrderAddressDtl"] != null)
                ////{
                ////    tempBillAddress = (DataTable)Session["PurchaseOrderAddressDtl"];
                ////}
                ////else
                ////{
                ////    tempBillAddress = StoreSalesOrderAddressDetail();
                ////}

                #endregion

                #endregion
                // End

                #region Sandip Section For Approval Section Start
                string approveStatus = "";
                if (Request.QueryString["status"] != null)
                {
                    approveStatus = Convert.ToString(Request.QueryString["status"]);
                }

                #endregion Sandip Section For Approval Section end


                if (ActionType == "Add")
                {

                    ///Changes Done 12-06-2018 Sudip Pal///////
                    strSchemeType = Convert.ToString(HDNumberingschema.Value);
                    ///Changes Done 12-06-2018 Sudip Pal///////
                    ///
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
                //// ############# Added By : Samrat Roy -- 02/05/2017 -- To check Transporter Mandatory Control 
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
                            //if (TermsConditionsControl.GetControlValue("dtDeliveryDate") == "" || TermsConditionsControl.GetControlValue("dtDeliveryDate") == "@")
                            if (TermsConditionsControl.GetControlVisibility("dtDeliveryDate") && (TermsConditionsControl.GetControlValue("dtDeliveryDate") == "" || TermsConditionsControl.GetControlValue("dtDeliveryDate") == "@"))
                            {
                                validate = "TCMandatory";
                            }
                        }
                    }
                }
                #endregion

                #region Section For Stock Section

                //if (IsBarcodeGeneratete() == true)
                //{
                //    if (ddlInventory.SelectedValue != "N")
                //    {
                //        if (Session["POwarehousedetailstemp"] != null)
                //        {
                //            foreach (DataRow dr in tempQuotation.Rows)
                //            {
                //                string ProductID = Convert.ToString(dr["ProductID"]);
                //                string strSrlNo = Convert.ToString(dr["SrlNo"]);
                //                decimal strProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                //                string Status = Convert.ToString(dr["Status"]);
                //                string IsLinkedProduct = Convert.ToString(dr["IsLinkedProduct"]);

                //                decimal strWarehouseQuantity = 0;
                //                DataTable _Stockdt = (DataTable)Session["POwarehousedetailstemp"];
                //                GetQuantityBaseOnProduct(_Stockdt, ProductID, strSrlNo, ref strWarehouseQuantity);

                //                if (ProductID != "")
                //                {
                //                    if (CheckProduct_IsInventory(ProductID) == true)
                //                    {
                //                        if (Status != "D")
                //                        {
                //                            if (IsLinkedProduct != "Y")
                //                            {
                //                                if (strProductQuantity != strWarehouseQuantity)
                //                                {
                //                                    validate = "checkWarehouse";
                //                                    grid.JSProperties["cpProductSrlIDCheck"] = strSrlNo;
                //                                    break;
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            validate = "nullWarehouse";
                //        }
                //    }
                //}

                #endregion

                string _ShippingState = Convert.ToString(Purchase_BillingShipping.GetShippingStateId());

                if (_ShippingState == "0")
                {
                    validate = "BillingShippingNotLoaded";
                }

                #region Checking of Registered Billing/Shipping of Vendor if not exist will not allow to save

                PurchaseInvoiceBL objPurchaseInvoice = new PurchaseInvoiceBL();


                ///Changes 12-06-2018 Sudip pal////
                //int cnt = objPurchaseInvoice.CheckBillingShippingofVendor(Convert.ToString(hdnCustomerId.Value));
                //if (cnt != 1)
                //{
                //    validate = "VendorAddressProblem";
                //}
                ///Changes 12-06-2018 Sudip pal////

                #endregion Checking of Registered Billing/Shipping of Vendor if not exist will not allow to save

                ///Changes 12-06-2018 Sudip pal////
                ///
                /// if (validate == "outrange" || validate == "BillingShippingNotLoaded" || validate == "duplicate" || validate == "UdfMandetory" || validate == "transporteMandatory" || validate == "TCMandatory" || validate == "nullQuantity" || validate == "duplicateProduct" || validate == "checkWarehouse" || validate == "nullWarehouse" || validate == "VendorAddressProblem")

                if (validate == "outrange" || validate == "BillingShippingNotLoaded" || validate == "duplicate" || validate == "UdfMandetory" || validate == "transporteMandatory" || validate == "TCMandatory" || validate == "nullQuantity" || validate == "duplicateProduct" || validate == "checkWarehouse" || validate == "VendorAddressProblem")

                ///Changes 12-06-2018 Sudip pal////
                {
                    grid.JSProperties["cpSaveSuccessOrFail"] = validate;
                }
                else
                {
                    if (IsPOExist(IndentRequisitionNo))
                    {

                        string TaxType = "", ShippingState = "";
                        ShippingState = Convert.ToString(Purchase_BillingShipping.GetShippingStateId());


                        if (ddl_AmountAre.Value == "1") TaxType = "E";
                        else if (ddl_AmountAre.Value == "2") TaxType = "I";

                        #region Delete Product Tax

                        string TaxDetails = Convert.ToString(hdnJsonProductTax.Value);
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        ser.MaxJsonLength = Int32.MaxValue;
                        List<ProductTaxDetails> TaxEntryJson = ser.Deserialize<List<ProductTaxDetails>>(TaxDetails);


                        foreach (DataRow productRow in tempQuotation.Rows)
                        {
                            string _ProductID = Convert.ToString(productRow["ProductID"]);
                            string _SrlNo = Convert.ToString(productRow["SrlNo"]);
                            string _IsEntry = "";

                            if (_ProductID != "")
                            {
                                var TaxValue = TaxEntryJson.Where(x => x.SrlNo == _SrlNo).ToList().SingleOrDefault();

                                if (TaxValue != null)
                                {
                                    _IsEntry = TaxValue.IsTaxEntry;

                                    if (_IsEntry == "N")
                                    {
                                        DataRow[] deletedRow = TaxDetaildt.Select("SlNo=" + _SrlNo);
                                        if (deletedRow.Length > 0)
                                        {
                                            foreach (DataRow dr in deletedRow)
                                            {
                                                TaxDetaildt.Rows.Remove(dr);
                                            }
                                            TaxDetaildt.AcceptChanges();
                                        }
                                    }
                                }
                            }
                        }

                        #endregion

                        TaxDetaildt = gstTaxDetails.SetTaxTableDataWithProductSerialForPurchaseRoundOff(ref tempQuotation, "SrlNo", "ProductID", "Amount", "TaxAmount", "TotalAmount", TaxDetaildt, "P", dt_PLQuote.Date.ToString("yyyy-MM-dd"), strBranch, ShippingState, TaxType, Convert.ToString(hdnCustomerId.Value));


                        if (AddModifyPurchaseOrder(PurchaseOrder_Id, UniquePurchaseNumber, strPurchaseDate, IndentRequisitionNo, IndentRequisitionDate, strVendor, strContactName,
                        Reference, strBranch, strAgents, strCurrency, strRate, strTaxOption, strTaxCode, CompanyID, BaseCurrencyId, tempQuotation, ActionType, IsInventory,
                        tempWarehousedt, TaxDetaildt, tempTaxDetailsdt, tempBillAddress, approveStatus, strCreditDays, strPoDate, txt_totamont.Text, txt_docnetamt.Text, txt_totamont_foreign.Text,
                         strexpectddatedispatchDate, strexpectddatearriavalDate, txtvessalno.Text, entrydate, PosForGst
                        ) == false)
                        {
                            grid.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                        }
                        else
                        {

                            if (approveStatus != "")
                            {
                                if (approveStatus == "2")
                                {
                                    grid.JSProperties["cpApproverStatus"] = "approve";
                                }
                                else
                                {
                                    grid.JSProperties["cpApproverStatus"] = "rejected";
                                }
                            }

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

                        if (Session["OrderAcceptancerDetails"] != null)
                        {
                            Session["OrderAcceptancerDetails"] = null;
                        }
                    }
                    else
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = "Ponotexist";
                    }
                }
            }
            else
            {
                DataView dvData = new DataView(PurchaseOrderdt);
                dvData.RowFilter = "Status <> 'D'";
                grid.DataSource = GetPurchaseOrderBatch(dvData.ToTable());
                grid.DataBind();
            }
        }
        public void GetQuantityBaseOnProduct(DataTable Warehousedt, string ProductID, string strProductSrlNo, ref decimal WarehouseQty)
        {
            decimal sum = 0;
            string Type = "";
            GetProductType(ProductID, ref Type);

            if (Warehousedt != null)
            {
                if (Type == "WS" || Type == "WBS" || Type == "BS" || Type == "S")
                {
                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        DataRow dr = Warehousedt.Rows[i];
                        string Product_SrlNo = Convert.ToString(dr["pcslno"]);

                        if (strProductSrlNo == Product_SrlNo)
                        {
                            sum = sum + 1;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        DataRow dr = Warehousedt.Rows[i];
                        string Product_SrlNo = Convert.ToString(dr["pcslno"]);

                        if (strProductSrlNo == Product_SrlNo)
                        {
                            string strQuantity = (Convert.ToString(dr["Quantitysum"]) != "") ? Convert.ToString(dr["Quantitysum"]) : "0";
                            sum = sum + Convert.ToDecimal(strQuantity);
                        }
                    }
                }
            }


            WarehouseQty = sum;
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
        //public bool IsBarcodeGeneratete()
        //{
        //    bool IsGeneratete = false;

        //    try
        //    {
        //        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //        DataTable DT_TC = objEngine.GetDataTable("tbl_Master_SystemControl", " BarcodeGeneration ", null);
        //        if (DT_TC != null && DT_TC.Rows.Count > 0)
        //        {
        //            IsGeneratete = Convert.ToBoolean(DT_TC.Rows[0]["BarcodeGeneration"]);
        //        }

        //        return IsGeneratete;
        //    }
        //    catch
        //    {
        //        return IsGeneratete;
        //    }
        //}
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
        public IEnumerable GetPurchaseOrderBatch(DataTable Quotationdt)
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
                PurchaseOrders.gvColTaxAmount = Convert.ToString(Quotationdt.Rows[i]["TaxAmount"]);
                PurchaseOrders.gvColTotalAmountINR = Convert.ToString(Quotationdt.Rows[i]["TotalAmount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(Quotationdt.Rows[i]["ImportPurOrder_ID"])))
                { 
                    PurchaseOrders.ImportPurOrder_ID = Convert.ToString(Quotationdt.Rows[i]["ImportPurOrder_ID"]);
                    PurchaseOrders.ImportPurOrdeDetails_Id = Convert.ToString(Quotationdt.Rows[i]["ImportPurOrdeDetails_Id"]);
                }
                else
                { 
                    PurchaseOrders.ImportPurOrder_ID = "";
                    PurchaseOrders.ImportPurOrdeDetails_Id = ""; 
                }                
                PurchaseOrders.ProductName = Convert.ToString(Quotationdt.Rows[i]["ProductName"]);
                PurchaseOrders.IsComponentProduct = Convert.ToString(Quotationdt.Rows[i]["IsComponentProduct"]);
                PurchaseOrders.IsLinkedProduct = Convert.ToString(Quotationdt.Rows[i]["IsLinkedProduct"]);
                PurchaseOrderList.Add(PurchaseOrders);
            }

            return PurchaseOrderList;
        }
        public bool AddModifyPurchaseOrder(string PurchaseOrder_Id, string UniquePurchaseNumber, string strPurchaseDate, string IndentRequisitionNo, string IndentRequisitionDate,

        string strVendor, string strContactName,
        string Reference, string strBranch, string strAgents, string strCurrency, string strRate, string strTaxOption,
        string strTaxCode, string CompanyID, int BaseCurrencyId, DataTable PurchaseOrderdt,
        string ActionType, string IsInventory, DataTable Warehousedt, DataTable TaxDetaildt, DataTable PurchaseOrderTaxdt, DataTable tempBillAddress,
        string approveStatus, string strCreditDays, string strPoDate, string totalamount, string docamount,string purtotalforeign,
         string strexpectddatedispatchDate, string strexpectddatearriavalDate, string vessel, string entrydate, string PosForGst)
        {
            try
            {
                if (PurchaseOrderdt.Rows.Count > 0)  // Cross Branch Section by Sam on 10052017 Start only Condition to prevent Twice Firing
                {
                    DataSet dsInst = new DataSet();
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("prc_PurchaseOrderAcceptance_import", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", ActionType);
                    cmd.Parameters.AddWithValue("@IsInventory", IsInventory);
                    cmd.Parameters.AddWithValue("@PurchaseOrderEdit_Id", PurchaseOrder_Id);
                    cmd.Parameters.AddWithValue("@approveStatus", approveStatus);
                    cmd.Parameters.AddWithValue("@PurchaseOrder_CompanyID", Convert.ToString(Session["LastCompany"]));
                    cmd.Parameters.AddWithValue("@PurchaseOrder_BranchId", strBranch);
                    if (Request.QueryString["op"] == "yes")
                    {
                        cmd.Parameters.AddWithValue("@PurchaseOrder_FinYear", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PurchaseOrder_FinYear", Convert.ToString(Session["LastFinYear"]));
                    }
                    cmd.Parameters.AddWithValue("@PurchaseOrder_Number", UniquePurchaseNumber);
                    cmd.Parameters.AddWithValue("@PurchaseOrder_IndentIds", IndentRequisitionNo);
                    if (!String.IsNullOrEmpty(IndentRequisitionDate))
                    { cmd.Parameters.AddWithValue("@PurchaseOrder_IndentDate", IndentRequisitionDate); }
                    cmd.Parameters.AddWithValue("@PurchaseOrder_Date", strPurchaseDate);
                    cmd.Parameters.AddWithValue("@PurchaseOrder_VendorId", strVendor);
                    cmd.Parameters.AddWithValue("@Contact_Person_Id", strContactName);
                    cmd.Parameters.AddWithValue("@PurchaseOrder_Reference", Reference);
                    cmd.Parameters.AddWithValue("@PurchaseOrder_Currency_Id", Convert.ToInt32(strCurrency));
                    cmd.Parameters.AddWithValue("@Currency_Conversion_Rate", strRate);
                    cmd.Parameters.AddWithValue("@Tax_Option", strTaxOption);
                    //cmd.Parameters.AddWithValue("@Tax_Code", strTaxCode);
                    cmd.Parameters.AddWithValue("@Tax_Code", 0);
                    cmd.Parameters.AddWithValue("@PurchaseOrder_SalesmanId", strAgents);
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(Session["userid"]));
                    cmd.Parameters.AddWithValue("@PurchaseOrderDetails", PurchaseOrderdt);
                    cmd.Parameters.AddWithValue("@TaxDetail", TaxDetaildt);
                    cmd.Parameters.AddWithValue("@PurchaseOrderTax", PurchaseOrderTaxdt);
                    cmd.Parameters.AddWithValue("@PurchaseOrderAddress", tempBillAddress);
                    cmd.Parameters.AddWithValue("@CreditDays", strCreditDays);
                    cmd.Parameters.AddWithValue("@PoDate", strPoDate);
                    cmd.Parameters.AddWithValue("@TotalAmountbase", totalamount);
                    cmd.Parameters.AddWithValue("@TotalDiocumenttamountbase", docamount);
                    cmd.Parameters.AddWithValue("@PurchaseOrder_ExpdispatchDate", strexpectddatedispatchDate);
                    cmd.Parameters.AddWithValue("@PurchaseOrder_ExparrivalDate", strexpectddatearriavalDate);
                    cmd.Parameters.AddWithValue("@TotalForeign", purtotalforeign);
                    cmd.Parameters.AddWithValue("@VesselNumber", vessel);


                    cmd.Parameters.AddWithValue("@RcmChecked", chkRCM.Checked);
                    cmd.Parameters.AddWithValue("@entrydate", entrydate);
                    cmd.Parameters.AddWithValue("@PosForGst", PosForGst);


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
                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("PO", "PurchaseOrder" + Convert.ToString(output.Value), udfTable, Convert.ToString(Session["userid"]));
                    }

                    if (!string.IsNullOrEmpty(hfControlData.Value))
                    {
                        CommonBL objCommonBL = new CommonBL();
                        objCommonBL.InsertTransporterControlDetails(Convert.ToInt32(output.Value), "IPI".ToUpper(), hfControlData.Value, Convert.ToString(HttpContext.Current.Session["userid"]));
                    }
                    if (!string.IsNullOrEmpty(hfTermsConditionData.Value))
                    {
                        TermsConditionsControl.SaveTC(hfTermsConditionData.Value, Convert.ToString(output.Value), "IPI");
                    }
                    //if (chkmail.Checked)
                    //{
                    //    //   exportToPDF.ExportToPdfforEmail("PO-Default~D", "Porder", Server.MapPath("~"), "", Convert.ToString(output.Value));

                    //    int retval = 0;
                    //    if (!string.IsNullOrEmpty(Convert.ToString(output.Value)))
                    //    {
                    //        retval = Sendmail_Purchaseorder(output.Value.ToString());
                    //    }
                    //}
                    #region warehouse Update and delete

                    //updatewarehouse();
                   // deleteALL();

                    #endregion

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

                    sqlQuery = "SELECT max(tjv.PurchaseOrderAcceptance_Number) FROM tbl_trans_ImportPurchaseOrder_Indent tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.PurchaseOrder_Number))) = 1";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.PurchaseOrderAcceptance_Number))) = 1 and PurchaseOrderAcceptance_Number like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.PurchaseOrderAcceptance_Number) FROM tbl_trans_ImportPurchaseOrder_Indent tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.PurchaseOrder_Number))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.PurchaseOrderAcceptance_Number))) = 1 and PurchaseOrderAcceptance_Number like '" + prefCompCode + "%'";
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
                    sqlQuery = "SELECT PurchaseOrderAcceptance_Number FROM tbl_trans_ImportPurchaseOrder_Indent WHERE PurchaseOrderAcceptance_Number LIKE '" + manual_str.Trim() + "'";
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
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["OrderAcceptanceId"]));
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
                DataTable MainTaxDataTable = (DataTable)Session["PurchaseOrderFinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["PurchaseOrderFinalTaxRecord"] = MainTaxDataTable;
               // GetStock(Convert.ToString(performpara.Split('~')[2]));
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
                DataTable MainTaxDataTable = (DataTable)Session["PurchaseOrderFinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["PurchaseOrderFinalTaxRecord"] = MainTaxDataTable;

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
        public void GetStock(string strProductID)
        {
            string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
            acpAvailableStock.JSProperties["cpstock"] = "0.00";

            try
            {
                DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + strProductID + ") as branchopenstock");

                if (dt2.Rows.Count > 0)
                {
                    taxUpdatePanel.JSProperties["cpstock"] = Convert.ToString(Math.Round(Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]), 2));
                }
                else
                {
                    taxUpdatePanel.JSProperties["cpstock"] = "0.00";
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void DeleteWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["WarehouseData"];

                var rows = Warehousedt.Select(string.Format("Product_SrlNo ='{0}'", SrlNo));
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["WarehouseData"] = Warehousedt;
            }
        }
        public double ReCalculateTaxAmount(string slno, double amount)
        {
            DataTable MainTaxDataTable = (DataTable)Session["PurchaseOrderFinalTaxRecord"];
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
            Session["PurchaseOrderFinalTaxRecord"] = MainTaxDataTable;

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
            Session["PurchaseOrderFinalTaxRecord"] = TaxRecord;
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
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderAcceptanceDetailsList_Import");
            proc.AddVarcharPara("@Action", 500, "ProductEditedTaxDetailsForPo");
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["OrderAcceptanceId"]));
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
                DataTable TaxRecord = (DataTable)Session["PurchaseOrderFinalTaxRecord"];
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

                Session["PurchaseOrderFinalTaxRecord"] = TaxRecord;
            }
            else
            {
                #region fetch All data For Tax

                DataTable taxDetail = new DataTable();
                DataTable MainTaxDataTable = (DataTable)Session["PurchaseOrderFinalTaxRecord"];

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
                    DataTable TaxRecord = (DataTable)Session["PurchaseOrderFinalTaxRecord"];
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
                    Session["PurchaseOrderFinalTaxRecord"] = TaxRecord;
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
            DataTable TaxRecord = (DataTable)Session["PurchaseOrderFinalTaxRecord"];
            foreach (var args in e.UpdateValues)
            {

                string TaxCodeDes = Convert.ToString(args.NewValues["Taxes_Name"]);
                decimal Percentage = 0;

                Percentage = Convert.ToDecimal(args.NewValues["TaxField"]);

                decimal Amount = Convert.ToDecimal(args.NewValues["Amount"]);
                string TaxCode = "0";
                if (!Convert.ToString(args.Keys[0]).Contains('~'))
                {
                    TaxCode = Convert.ToString(args.Keys[0]);
                }



                DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='" + TaxCode + "'");
                if (finalRow.Length > 0)
                {
                    finalRow[0]["Percentage"] = Percentage;
                    // finalRow[0]["TaxCode"] = args.NewValues["TaxField"]; 
                    finalRow[0]["Amount"] = Amount;

                    finalRow[0]["TaxCode"] = args.Keys[0];
                    finalRow[0]["AltTaxCode"] = "0";

                }
                else
                {
                    DataRow newRow = TaxRecord.NewRow();
                    newRow["slNo"] = slNo;
                    newRow["Percentage"] = Percentage;
                    newRow["TaxCode"] = TaxCode;
                    newRow["AltTaxCode"] = "0";
                    newRow["Amount"] = Amount;
                    TaxRecord.Rows.Add(newRow);
                }


            }

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


            Session["PurchaseOrderFinalTaxRecord"] = TaxRecord;


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
            if (Session["PurchaseOrderFinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["PurchaseOrderFinalTaxRecord"];

                var rows = TaxDetailTable.Select("SlNo ='" + SrlNo + "'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                TaxDetailTable.AcceptChanges();

                Session["PurchaseOrderFinalTaxRecord"] = TaxDetailTable;
            }
        }
        public void UpdateTaxDetails(string oldSrlNo, string newSrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["PurchaseOrderFinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["PurchaseOrderFinalTaxRecord"];

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

                Session["PurchaseOrderFinalTaxRecord"] = TaxDetailTable;
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
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["OrderAcceptanceId"]));
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

           // string strBranch = Convert.ToString(Session["userbranchHierarchy"]);
            string PurchaseOrder_Id = Convert.ToString(Session["OrderAcceptanceId"]);
            DataTable IndentTable = objPurchaseOrderBL.GetIndentOnPO(OrderDate, status, BranchId, vendor, PurchaseOrder_Id);
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
                string strBranch = Convert.ToString(e.Parameters.Split('~')[1]);
                //string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
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

                string PurchaseOrder_Id = Convert.ToString(Session["OrderAcceptanceId"]);
                DataTable IndentTable = objPurchaseOrderBL.GetIndentOnPO(POrderDate, status, strBranch, Vendor, PurchaseOrder_Id);
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
                String QuoComponent = "", QuoComponentNumber = "", QuoComponentDate = "";
                if (taggingGrid.GetSelectedFieldValues("ComponentID").Count > 0)
                {
                    for (int i = 0; i < taggingGrid.GetSelectedFieldValues("ComponentID").Count; i++)
                    {
                        QuoComponent += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("ComponentID")[i]);
                        QuoComponentDate += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("ComponentDate")[i]);
                        QuoComponentNumber += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("ComponentNumber")[i]);
                    }

                    QuoComponent = QuoComponent.TrimStart(',');
                    QuoComponentDate = QuoComponentDate.TrimStart(',');
                    QuoComponentNumber = QuoComponentNumber.TrimStart(',');
                    if (taggingGrid.GetSelectedFieldValues("ComponentID").Count > 0)
                    {
                        if (taggingGrid.GetSelectedFieldValues("ComponentID").Count > 1)
                        {
                            QuoComponentDate = "Multiple Purchase Order Dates";
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
                            dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsFromPO(QuoComponent, IdKey, "", "Edit");
                        }
                        else
                        {
                            dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsFromPO(QuoComponent, "0", "", "Add");
                        }

                    }
                    //else
                    //{
                    //    dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsFromPO(QuoComponent, "", "");
                    //}
                    //Session["OrderAcceptancerDetails"] = null;

                    grid_Products.DataSource = GetProductsInfo(dt_QuotationDetails);
                    grid_Products.DataBind();
                    for (int i = 0; i < grid_Products.VisibleRowCount; i++)
                    {
                        grid_Products.Selection.SelectRow(i);
                    }

                    grid_Products.JSProperties["cpComponentDetails"] = QuoComponentNumber + "~" + QuoComponentDate;
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
                ImportPOrders.SrlNo =Convert.ToString(SalesOrderdt1.Rows[i]["SrlNo"]);
                ImportPOrders.ComponentID = Convert.ToString(SalesOrderdt1.Rows[i]["ComponentID"]);
                ImportPOrders.ComponentDetailsID = Convert.ToString(SalesOrderdt1.Rows[i]["ComponentDetailsID"]);
                ImportPOrders.ProductID = Convert.ToString(SalesOrderdt1.Rows[i]["ProductID"]);
                ImportPOrders.ComponentNumber = Convert.ToString(SalesOrderdt1.Rows[i]["ComponentNumber"]);
                ImportPOrders.ProductsName = Convert.ToString(SalesOrderdt1.Rows[i]["ProductsName"]);
                ImportPOrders.ProductDescription = Convert.ToString(SalesOrderdt1.Rows[i]["ProductDescription"]);
                ImportPOrders.Quantity = Convert.ToString(SalesOrderdt1.Rows[i]["Quantity"]);              

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
        #region  Available Stock
        protected void acpAvailableStock_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            string strProductID = Convert.ToString(performpara.Split('~')[0]);
            string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
            acpAvailableStock.JSProperties["cpstock"] = "0.00";

            try
            {
                DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + strProductID + ") as branchopenstock");

                if (dt2.Rows.Count > 0)
                {
                    acpAvailableStock.JSProperties["cpstock"] = Convert.ToString(Math.Round(Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]), 2));
                }
                else
                {
                    acpAvailableStock.JSProperties["cpstock"] = "0.00";
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Available Stock
        #region Purchase Order Mail
        //public int Sendmail_Purchaseorder(string Output)
        //{
        //    int stat = 0;
        //    if (chkmail.Checked)
        //    {
        //        if (cmbContactPerson.Value != null)
        //        {
        //            Employee_BL objemployeebal = new Employee_BL();
        //            ExceptionLogging mailobj = new ExceptionLogging();
        //            EmailSenderHelperEL emailSenderSettings = new EmailSenderHelperEL();
        //            DataTable dt_EmailConfig = new DataTable();
        //            DataTable dt_EmailConfigpurchase = new DataTable();

        //            DataTable dt_Emailbodysubject = new DataTable();
        //            PurchaseorderEmailTags fetchModel = new PurchaseorderEmailTags();
        //            string Subject = "";
        //            string Body = "";
        //            string emailTo = "";
        //            int MailStatus = 0;

        //            var customerid = cmbContactPerson.Value.ToString();
        //            dt_EmailConfig = objemployeebal.Getemailids(customerid);
        //            string FilePath = Server.MapPath("~/Reports/RepxReportDesign/PurchaseOrder/DocDesign/PDFFiles/" + "PO-Default-" + Output + ".pdf");
        //            string FileName = FilePath;
        //            if (dt_EmailConfig.Rows.Count > 0)
        //            {
        //                emailTo = Convert.ToString(dt_EmailConfig.Rows[0]["eml_email"]);
        //                dt_Emailbodysubject = objemployeebal.Getemailtemplates("13");  //for purchase order
        //                if (dt_Emailbodysubject.Rows.Count > 0)
        //                {
        //                    Body = Convert.ToString(dt_Emailbodysubject.Rows[0]["body"]);
        //                    Subject = Convert.ToString(dt_Emailbodysubject.Rows[0]["subjct"]);

        //                    dt_EmailConfigpurchase = objemployeebal.Getemailtagsforpurchase(Output, "PurchaseOrderEmailTags");  //For Purchase Order Get all Tags Value

        //                    if (dt_EmailConfigpurchase.Rows.Count > 0)
        //                    {
        //                        fetchModel = DbHelpers.ToModel<PurchaseorderEmailTags>(dt_EmailConfigpurchase);
        //                        Body = Employee_BL.GetFormattedString<PurchaseorderEmailTags>(fetchModel, Body);
        //                        Subject = Employee_BL.GetFormattedString<PurchaseorderEmailTags>(fetchModel, Subject);
        //                    }

        //                    emailSenderSettings = mailobj.GetEmailSettingsforAllreport(emailTo, "", "", FilePath, Body, Subject);
        //                    if (emailSenderSettings.IsSuccess)
        //                    {
        //                        string Message = "";
        //                        EmailSenderEL obj2 = new EmailSenderEL();
        //                        stat = SendEmailUL.sendMailInHtmlFormat(emailSenderSettings.ModelCast<EmailSenderEL>(), out Message);

        //                    }
        //                }
        //            }
        //        }

        //    }
        //    return stat;
        //}
        #endregion
        #region Visibility of Send Mail

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

        #endregion
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

    }
}