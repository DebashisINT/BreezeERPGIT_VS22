
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;

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


namespace OpeningEntry.ERP
{
    public partial class PurchaseOrder : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
     
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        string UniquePurchaseNumber = string.Empty;
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        PurchaseOrderBL objPurchaseOrderBL = new PurchaseOrderBL();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        public string pageAccess = "";
        static string ForJournalDate = null;
        #region Sandip Section For Approval Section Start
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        #endregion Sandip Section For Approval Dtl Section End

        #region Sandip Section For Approval Section Start
        public void IsExistsDocumentInERPDocApproveStatus(string orderId)
        {
            string editable = "";
            string status = "";
            DataTable dt = new DataTable();
            int Id = Convert.ToInt32(orderId);
            dt = objERPDocPendingApproval.IsExistsDocumentInERPDocApproveStatus(Id, 7); // 7 for Purchase Order
            if (dt.Rows.Count > 0)
            {
                editable = Convert.ToString(dt.Rows[0]["editable"]);
                if (editable == "0")
                {
                    lbl_quotestatusmsg.Visible = true;
                    status = Convert.ToString(dt.Rows[0]["Status"]);
                    if (status == "Approved")
                    {
                        lbl_quotestatusmsg.Text = "Document already Approved";
                    }
                    if (status == "Rejected")
                    {
                        lbl_quotestatusmsg.Text = "Document already Rejected";
                    }
                    btn_SaveRecords.Visible = false;
                    btn_SaveRecords.Visible = false;
                }
                else
                {
                    lbl_quotestatusmsg.Visible = false;
                    btn_SaveRecords.Visible = true;
                    btn_SaveRecords.Visible = true;
                }
            }
        }

        //public void GetEditablePermission()
        //{
        //    if (Request.QueryString["Permission"] != null)
        //    {
        //        if (Convert.ToString(Request.QueryString["Permission"]) == "1")
        //        {

        //            btn_SaveRecords.Visible = false;
        //            btn_SaveRecords.Visible = false;
        //        }
        //        else if (Convert.ToString(Request.QueryString["Permission"]) == "2")
        //        {

        //            btn_SaveRecords.Visible = true;
        //            btn_SaveRecords.Visible = true;
        //        }
        //        else if (Convert.ToString(Request.QueryString["Permission"]) == "3")
        //        {

        //            btn_SaveRecords.Visible = true;
        //            ASPxButton1.Visible = true;
        //        }
        //    }
        //}

        #endregion Sandip Section For Approval Dtl Section End
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            #region Sandip Section For Approval Section Start
            if (Request.QueryString.AllKeys.Contains("status"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
                //divcross.Visible = false;
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
                //divcross.Visible = true;
            }
            #endregion Sandip Section For Approval Dtl Section End
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);


                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "SrlNo")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "Indent_Num")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "ProductName")
            {
                e.Editor.Enabled = true;
            } 
            else if (e.Column.FieldName == "gvColDiscription")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "gvColUOM")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "gvColAmount")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "gvColTotalAmountINR")
            {
                e.Editor.Enabled = true;
            }   
            else
            {
                e.Editor.ReadOnly = false;
            }
          

        }
        protected void Page_Load(object sender, EventArgs e)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/ERP/PurchaseOrder.aspx");
            ProductDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlIndentRequisitionNo.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Sqlvendor.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCurrency.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DS_Branch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DS_SalesAgent.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DS_AmountAre.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectPin.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (Request.QueryString["key"] != null && Request.QueryString["key"] != "ADD")
            {
                string strOrderId1 = Convert.ToString(Request.QueryString["key"]);
                #region Sandip Section For Approval Dtl Section Start
                if (Request.QueryString["status"] == null)
                {
                    IsExistsDocumentInERPDocApproveStatus(strOrderId1);
                }
                #endregion Sandip Section For Approval Dtl Section End
                grid.JSProperties["cpView"] = (Request.QueryString["req"] != null && Request.QueryString["req"] == "V") ? "1" : "0";
               
                Session["PurchaseOrder_Id"] = strOrderId1;
                DataTable PurchaseOrderEditdt = GetPurchaseOrderEditData();
                if (PurchaseOrderEditdt != null && PurchaseOrderEditdt.Rows.Count > 0)
                {                 
                    
                    string PurchaseOrder_Date = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_Date"]);
                    string Quoids = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_IndentIds"]);
                    string PurchaseOrder_BranchId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_BranchId"]);
                    if (!String.IsNullOrEmpty(Quoids))
                    {
                        string[] eachQuo = Quoids.Split(',');
                        if (eachQuo.Length > 1)//More than one quotation
                        {
                            dt_Quotation.Text = "Multiple Select Quotation Dates";
                            BindLookUp(PurchaseOrder_Date, PurchaseOrder_BranchId);
                            foreach (string val in eachQuo)
                            {
                                lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));

                            }
                           
                        }
                        else if (eachQuo.Length == 1)//Single Quotation
                        { 
                            BindLookUp(PurchaseOrder_Date, PurchaseOrder_BranchId);
                            foreach (string val in eachQuo)
                            {
                                lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                        else//No Quotation selected
                        {
                            BindLookUp(PurchaseOrder_Date, PurchaseOrder_BranchId);
                        }
                    }
                }

            }


            if (!IsPostBack)
            {

                #region Sandip Section For Checking User Level for Allow Edit to logging User or Not
                //GetEditablePermission();
                if (Request.QueryString.AllKeys.Contains("status"))
                {


                    divcross.Visible = false;
                    btn_SaveRecords.Visible = false;
                    ApprovalCross.Visible = true;
                    ddl_Branch.Enabled = false;
                }
                else
                {
                    divcross.Visible = true;
                    btn_SaveRecords.Visible = true;
                    ApprovalCross.Visible = false;
                    ddl_Branch.Enabled = true;
                }
                #endregion Sandip Section
                if (Request.QueryString["op"] == "yes")
                {
                    hdnOpening.Value = "Opening";
                    //indentRequisition.Disabled = false;
                    ComponentQuotationPanel.Enabled = false;
                }
                this.Session["LastCompany1"] = Session["LastCompany"];
                this.Session["LastFinYear1"] = Session["LastFinYear"];
                this.Session["userbranch"] = Session["userbranchID"];
                Bind_NumberingScheme();

                //Sudip Pal 
             //   SetFinYearCurrentDate();
                GetFinancialYearstartandEnddate();

                //Sudip Pal 


                PopulateCustomerDetail();
                GetAllDropDownDetailForPurchaseOrder();
                BindBranch();
              
                string finyear = Convert.ToString(Session["LastFinYear"]);

                Session["CustomerDetail"] = null;
                Session["PurchaseOrder_Id"] = null;
                Session["ProductOrderDetails"] = null;
                Session["LoopWarehouse"] = 1;
                Session["POTaxDetails"] = null;
                Session["PurchaseOrderAddressDtl"] = null;
                Session["POwarehousedetailstemp"] = null;
                Session["POwarehousedetailstempUpdate"] = null;
                Session["POwarehousedetailstempDelete"] = null;
               
                PopulateGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                PopulateChargeGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                if (Request.QueryString["key"] == "ADD")
                {
                    #region To Show By Default Cursor after SAVE AND NEW
                    if (Session["SaveModePO"] != null)  // it has been removed from coding side of Quotation list 
                    {
                        if (Session["schemavaluePO"] != null)  // it has been removed from coding side of Quotation list 
                        {
                            ddl_numberingScheme.SelectedValue = Convert.ToString(Session["schemavaluePO"]); // it has been removed from coding side of Quotation list 
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
                        ddl_numberingScheme.Focus();
                    }
                    #endregion To Show By Default Cursor after SAVE AND NEW
                    IndentTaggingMendatory();
                    ViewState["ActionType"] = "Add";
                    Keyval_internalId.Value = "Add";
                    hdnPageStatus1.Value = "update";
                    if (!String.IsNullOrEmpty(Convert.ToString(Session["userbranchID"])))
                    {
                        string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                        ddl_Branch.SelectedValue = strdefaultBranch;
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(Session["LocalCurrency"])))
                    {
                        string CompID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                        string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);
                        string basedCurrency = Convert.ToString(LocalCurrency.Split('~')[0]);
                        string CurrencyId = Convert.ToString(basedCurrency[0]);
                        ddl_Currency.SelectedValue = CurrencyId;
                        int ConvertedCurrencyId = Convert.ToInt32(CurrencyId);
                       
                        DataTable dt = objPurchaseOrderBL.GetCurrentConvertedRate(Convert.ToInt16(CurrencyId), ConvertedCurrencyId, CompID);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            txt_Rate.Text = Convert.ToString(dt.Rows[0]["PurchaseRate"]);
                        }
                    }
                    hdnPageStatus.Value = "first";
                    ddl_AmountAre.Value = "1";
                    ddl_VatGstCst.ClientEnabled = false;
                    //.................Tax..............
                    Session["POTaxDetails"] = null;
                    CreateDataTaxTable();
                    //..........End Tax-----------

                }
                else
                {
                    txtVoucherNo.Enabled = false;
                    Keyval_internalId.Value = "PurchaseOrder" + Request.QueryString["key"];
                    hdnPageStatus.Value = "Quoteupdate";
                    lblHeading.Text = "Modify Purchase Order";
                    divNumberingScheme.Style.Add("display", "none");
                    lbl_NumberingScheme.Visible = false;
                    ddl_numberingScheme.Visible = false;
                    Session["PurchaseOrder_Id"] = Request.QueryString["key"];
                    ViewState["ActionType"] = "Edit";
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
                    Session["ProductOrderDetails"] = GetPurchaseOrderData().Tables[0];
                    grid.DataSource = GetPurchaseOrderBatch();
                    grid.DataBind();
                    Session["PurchaseOrderAddressDtl"] = GetBillingAddress();
                    if (IsPITransactionExist(Request.QueryString["key"]))
                    {
                        grid.JSProperties["cpBtnVisible"] = "false";
                    }
                  
                }

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);
               
            }
            else
            {
                PopulateCustomerDetail();
               // Bind_NumberingScheme();
               
            }

        }
        public void IndentTaggingMendatory()
        {
            DataTable TaggingMen = objPurchaseOrderBL.GetIndentTaggingMendatory();
            if (TaggingMen.Rows.Count > 0)
            {
                string strVariableName = Convert.ToString(TaggingMen.Rows[0]["Variable_Value"]).Trim();
                if(strVariableName=="Yes")
                {
                  ///  hdfTagMendatory.Value = "Yes";
                }
                else
                {
                 //   hdfTagMendatory.Value = "No";
                }               
            }
        }
        public void BindBranch()
        {
            DS_Branch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
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



            ///Sudip Pal
            ///
            ///Financial Year
            //DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "17", "N");


            //Opening Entry
            DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchemaOpening(strCompanyID, userbranchHierarchy, FinYear, "17", "N");


            ///Sudip Pal


            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }
        }
        public DataTable GetBillingAddress()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "BillingAddress");
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["PurchaseOrder_Id"]));
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows.Count == 2)
                {
                    if (Convert.ToString(dt.Rows[0]["OrderAdd_addressType"]) == "Shipping")
                    {
                        string countryid = Convert.ToString(dt.Rows[0]["OrderAdd_countryId"]);
                        CmbCountry1.Value = countryid;
                        FillStateCombo(CmbState1, Convert.ToInt32(countryid));
                        string stateid = Convert.ToString(dt.Rows[0]["OrderAdd_stateId"]);
                        CmbState1.Value = stateid;
                    }
                    else if (Convert.ToString(dt.Rows[1]["OrderAdd_addressType"]) == "Shipping")
                    {
                        string countryid = Convert.ToString(dt.Rows[0]["OrderAdd_countryId"]);
                        CmbCountry1.Value = countryid;
                        FillStateCombo(CmbState1, Convert.ToInt32(countryid));
                        string stateid = Convert.ToString(dt.Rows[0]["OrderAdd_stateId"]);
                        CmbState1.Value = stateid;
                    }
                }
                else if (dt.Rows.Count == 1)
                {
                    if (Convert.ToString(dt.Rows[0]["OrderAdd_addressType"]) == "Shipping")
                    {
                        string countryid = Convert.ToString(dt.Rows[0]["OrderAdd_countryId"]);
                        CmbCountry1.Value = countryid;
                        FillStateCombo(CmbState1, Convert.ToInt32(countryid));
                        string stateid = Convert.ToString(dt.Rows[0]["OrderAdd_stateId"]);
                        CmbState1.Value = stateid;
                    }

                }
            }
            return dt;
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
                lookup_Customer.DataSource = dtCustomer;
                lookup_Customer.DataBind();
                Session["POVendorsDetail"] = dtCustomer;
            }

        }
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
            public string gvColTaxAmount { get; set; }
            public string gvColTotalAmountINR { get; set; }
            public string Quotation_No { get; set; }
            public string ProductName { get; set; }
            public string Indent_Num { get; set; }
            public Int64 Indent { get; set; }


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
        }
        public void FillGrid()
        {
            DataTable PurchaseOrderEditdt = GetPurchaseOrderEditData();
            if (PurchaseOrderEditdt != null && PurchaseOrderEditdt.Rows.Count > 0)
            {
                string PurchaseOrder_Number = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_Number"]);//0
                string PurchaseOrder_IndentId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_IndentIds"]);//1
                string PurchaseOrder_BranchId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_BranchId"]);//2
                string PurchaseOrder_Date = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_Date"]);//3

                string Customer_Id = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_VendorId"]);//5
                string Contact_Person_Id = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Contact_Person_Id"]);//6
                string PurchaseOrder_Reference = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_Reference"]);//7
                string PurchaseOrder_Currency_Id = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_Currency_Id"]);//8
                string Currency_Conversion_Rate = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Currency_Conversion_Rate"]);//9
                string Tax_Option = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Tax_Option"]);//10
                string Tax_Code = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Tax_Code"]);//11
                string PurchaseOrder_SalesmanId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_SalesmanId"]);//12
                string PurchaseOrder_IndentDate = Convert.ToString(PurchaseOrderEditdt.Rows[0]["IndentDate"]);//13


                txtVoucherNo.Text = PurchaseOrder_Number;
                dt_PLQuote.Date = Convert.ToDateTime(PurchaseOrder_Date);
                // ddl_IndentRequisitionNo.SelectedValue = PurchaseOrder_IndentId;
                if (!string.IsNullOrEmpty(PurchaseOrder_IndentDate))
                { 
                    //txtDateIndentRequis.Date = Convert.ToDateTime(PurchaseOrder_IndentDate); 
                    dt_Quotation.Text = PurchaseOrder_IndentDate; 
                }
                
                // ddl_Vendor.SelectedValue = Customer_Id;
                lookup_Customer.GridView.Selection.SelectRowByKey(Customer_Id);
                if (Customer_Id != "")
                {
                    hdfLookupCustomer.Value = Customer_Id;
                    DataTable GSTNTable = objPurchaseOrderBL.GetVendorGSTIN(Customer_Id);
                    string strGSTN = Convert.ToString(GSTNTable.Rows[0]["CNT_GSTIN"]).Trim();
                    if (strGSTN != "")
                    {
                     //   lblGSTIN.Text = "Yes";
                    }
                    else
                    {
                      //  lblGSTIN.Text  = "No";
                    }
                    DataTable OutStandingTable = objPurchaseOrderBL.GetVendorOutStanding(Customer_Id);
                    if (OutStandingTable.Rows.Count > 0)
                    {
                        pageheaderContent.Attributes.Add("style", "display:block");
                        divOutstanding.Attributes.Add("style", "display:block");

                        var convertDecimal = Convert.ToDecimal(Convert.ToString(OutStandingTable.Rows[0]["NetOutstanding"]));
                        if (convertDecimal > 0)
                        {
                            lblTotalPayable.Text = Convert.ToString(convertDecimal) + "(Db)";
                        }
                        else
                        {

                            lblTotalPayable.Text = Convert.ToString(convertDecimal * -1).ToString() + "(Cr)";
                        }


                    }
                    else
                    {
                        cmbContactPerson.JSProperties["cpOutstanding"] = "0.0";
                    }
                }
                hdnCustomerId.Value = Customer_Id;
                TabPage page = ASPxPageControl1.TabPages.FindByName("Billing/Shipping");
                page.ClientEnabled = true;
                PopulateContactPersonOfCustomer(Customer_Id);
                if (Contact_Person_Id!="0")
                {
                    cmbContactPerson.Value = Contact_Person_Id;

                    DataTable dtContactPersonPhone = new DataTable();
                    dtContactPersonPhone = objPurchaseOrderBL.PopulateContactPersonPhone(Contact_Person_Id);
                    if (dtContactPersonPhone != null && dtContactPersonPhone.Rows.Count > 0)
                    {
                         pageheaderContent.Attributes.Add("style", "display:block");
                         divContactPhone.Attributes.Add("style", "display:block");
                        
                        foreach (DataRow dr in dtContactPersonPhone.Rows)
                        {
                            string phone = Convert.ToString(dr["phf_phoneNumber"]);
                            if (!string.IsNullOrEmpty(phone))
                            {
                                lblContactPhone.Text = phone;
                                break;
                            }
                        }

                    }
                   
                }
               
                txt_Refference.Text = PurchaseOrder_Reference;
                ddl_Branch.SelectedValue = PurchaseOrder_BranchId;
                ddl_SalesAgent.SelectedValue = PurchaseOrder_SalesmanId;
                ddl_Currency.SelectedValue = PurchaseOrder_Currency_Id;
                txt_Rate.Text = Currency_Conversion_Rate;

                //Subhabrata
                string Quoids = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_IndentIds"]);

                if (!String.IsNullOrEmpty(Quoids))
                {
                    string[] eachQuo = Quoids.Split(',');
                    if (eachQuo.Length > 1)//More tha one quotation
                    {
                        dt_Quotation.Text = "Multiple Select Indent Dates";
                        BindLookUp(Convert.ToString(dt_PLQuote.Date), PurchaseOrder_BranchId);
                        foreach (string val in eachQuo)
                        {
                            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }
                        // lbl_MultipleDate.Attributes.Add("style", "display:block");
                    }
                    else if (eachQuo.Length == 1)//Single Quotation
                    { //lbl_MultipleDate.Attributes.Add("style", "display:none"); }
                        BindLookUp(Convert.ToString(dt_PLQuote.Date), PurchaseOrder_BranchId);
                        foreach (string val in eachQuo)
                        {
                            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }
                    }
                    else//No Quotation selected
                    {
                        BindLookUp(Convert.ToString(dt_PLQuote.Date), PurchaseOrder_BranchId);
                    }
                }
                //End

                if (Tax_Option != "0")
                {
                    ddl_AmountAre.Value = Tax_Option;
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
                PurchaseOrders.gvColDiscount = Convert.ToString(Quotationdt.Rows[i]["Discount"]);
                PurchaseOrders.gvColAmount = Convert.ToString(Quotationdt.Rows[i]["Amount"]);
                PurchaseOrders.gvColTaxAmount = Convert.ToString(Quotationdt.Rows[i]["TaxAmount"]);
                PurchaseOrders.gvColTotalAmountINR = Convert.ToString(Quotationdt.Rows[i]["TotalAmount"]);
                PurchaseOrders.Quotation_No = Convert.ToString(0);
                PurchaseOrders.ProductName = Convert.ToString(Quotationdt.Rows[i]["ProductName"]);
                PurchaseOrderList.Add(PurchaseOrders);
            }

            return PurchaseOrderList;
        }
        public DataSet GetPurchaseOrderData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "PoBatchEditDetails");
            proc.AddVarcharPara("@PurchaseOrder_Id", 500, Convert.ToString(Session["PurchaseOrder_Id"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(Session["LastCompany"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable GetPurchaseOrderEditData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "PurchaseOrderEditDetails");
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["PurchaseOrder_Id"]));
            //proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            //proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }
        public class Product
        {
            public string ProductID { get; set; }
            public string ProductName { get; set; }
        }
        public DataSet GetProductData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "ProductDetails");
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(Session["LastCompany"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public IEnumerable GetProduct()
        {
            List<Product> ProductList = new List<Product>();
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
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
        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    ((GridViewDataComboBoxColumn)grid.Columns["gvColProduct"]).PropertiesComboBox.DataSource = GetProduct();
        //}

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
        public void GetAllDropDownDetailForPurchaseOrder()
        {
            DataSet dst = new DataSet();
            dst = objPurchaseOrderBL.GetAllDropDownDetailForPurchaseOrder();


            //if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            //{
            //    ddl_SalesAgent.DataTextField = "Name";
            //    ddl_SalesAgent.DataValueField = "cnt_id";
            //    ddl_SalesAgent.DataSource = dst.Tables[1];
            //    ddl_SalesAgent.DataBind();
            //}

            if (dst.Tables[3] != null && dst.Tables[3].Rows.Count > 0)
            {
                ddl_AmountAre.TextField = "taxGrp_Description";
                ddl_AmountAre.ValueField = "taxGrp_Id";
                ddl_AmountAre.DataSource = dst.Tables[3];
                ddl_AmountAre.DataBind();
            }


            //if (Session["userbranchID"] != null)
            //{
            //    if (ddl_Branch.Items.Count > 0)
            //    {
            //        int branchindex = 0;
            //        int cnt = 0;
            //        foreach (ListItem li in ddl_Branch.Items)
            //        {
            //            if (li.Value == Convert.ToString(Session["userbranchID"]))
            //            {
            //                cnt = 1;
            //                break;
            //            }
            //            else
            //            {
            //                branchindex += 1;
            //            }
            //        }
            //        if (cnt == 1)
            //        {
            //            ddl_Branch.SelectedIndex = branchindex;
            //        }
            //        else
            //        {
            //            ddl_Branch.SelectedIndex = cnt;
            //        }
            //    }
            //}

            ddl_SalesAgent.Items.Insert(0, new ListItem("Select", "0"));
            // ddl_AmountAre.Items.Insert(0, new ListItem("Select", "0"));

            //if (Session["ActiveCurrency"] != null)
            //{
            //    if (ddl_Currency.Items.Count > 0)
            //    {
            //        string[] ActCurrency = new string[] { };
            //        string currency = Convert.ToString(HttpContext.Current.Session["ActiveCurrency"]);
            //        ActCurrency = currency.Split('~');
            //        foreach (ListItem li in ddl_Currency.Items)
            //        {
            //            if (li.Value == Convert.ToString(ActCurrency[0]))
            //            {
            //                ddl_Currency.Items.Remove(li);
            //                break;
            //            }
            //        }
            //    }
            //    ddl_Currency.Items.Insert(0, new ListItem("Select", "0"));
            //    ddl_Currency.SelectedIndex = 0;
            //}
        }


        [WebMethod]
        public static string getSchemeType(string sel_scheme_id)
        {
            //BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //string[] scheme = oDbEngine1.GetFieldValue1("tbl_master_Idschema", "schema_type", "Id = " + Convert.ToInt32(sel_scheme_id), 1);
            //return Convert.ToString(scheme[0]);
            string strschematype = "", strschemalength = "", strschemavalue = "";
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length ", " Id = " + Convert.ToInt32(sel_scheme_id));

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strschemavalue = strschematype + "~" + strschemalength;
            }
            return Convert.ToString(strschemavalue);
        }
        [WebMethod]
        public static string getIndentRequisitionDate(string IndentRequisitionNo)
        {
            //BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine();
            string[] RequisitionDate = oDbEngine1.GetFieldValue1("tbl_trans_Indent", "Indent_RequisitionDate", "Indent_Id = " + Convert.ToInt32(IndentRequisitionNo), 1);
            return Convert.ToString(RequisitionDate[0]);
        }
        [WebMethod]
        public static String GetRate(string basedCurrency, string Currency_ID, string Campany_ID)
        {

            PurchaseOrderBL objPurchaseOrderBL = new PurchaseOrderBL();
            DataTable dt = objPurchaseOrderBL.GetCurrentConvertedRate(Convert.ToInt16(basedCurrency), Convert.ToInt16(Currency_ID), Campany_ID);
            string SalesRate = "";
            if (dt.Rows.Count > 0)
            {
                SalesRate = Convert.ToString(dt.Rows[0]["PurchaseRate"]);
            }

            return SalesRate;
        }
        protected void cmbContactPerson_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindContactPerson")
            {
                string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                PopulateContactPersonOfCustomer(InternalId);

                DataTable OutStandingTable = objPurchaseOrderBL.GetVendorOutStanding(InternalId);
                if (OutStandingTable.Rows.Count > 0)
                {
                    var convertDecimal = Convert.ToDecimal(Convert.ToString(OutStandingTable.Rows[0]["NetOutstanding"]));
                    if (convertDecimal > 0)
                    {
                        cmbContactPerson.JSProperties["cpOutstanding"] = Convert.ToString(convertDecimal) + "(Db)";
                    }
                    else
                    {
                        cmbContactPerson.JSProperties["cpOutstanding"] = Convert.ToString(convertDecimal * -1).ToString() + "(Cr)";
                    }
                }
                else
                {
                    cmbContactPerson.JSProperties["cpOutstanding"] = "0.0";
                }

                DataTable GSTNTable = objPurchaseOrderBL.GetVendorGSTIN(InternalId);
                string strGSTN = Convert.ToString(GSTNTable.Rows[0]["CNT_GSTIN"]).Trim();
                if (strGSTN!="")
                 {
                     cmbContactPerson.JSProperties["cpGSTN"] = "Yes";
                 }
                else
                {
                    cmbContactPerson.JSProperties["cpGSTN"] = "No";
                }
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
                if (chkBilling.Checked)
                {
                    if (CmbState.Value != null)
                    {
                        ShippingState = CmbState.Text;
                        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    }
                }
                else
                {
                    if (CmbState1.Value != null)
                    {
                        ShippingState = CmbState1.Text;
                        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    }
                }



                if (ShippingState.Trim() != "" && compGstin[0].Trim() != "")
                {

                    if (compGstin.Length > 0)
                    {
                        if (compGstin[0].Substring(0, 2) == ShippingState)
                        {
                            //Check if the state is in union territories then only UTGST will apply
                            //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU     Lakshadweep              PONDICHERRY
                            if (ShippingState == "4" || ShippingState == "35" || ShippingState == "26" || ShippingState == "25" || ShippingState == "31" || ShippingState == "34")
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



                //foreach (DataRow dr in dtContactPerson.Rows)
                //{
                //    if (Convert.ToString(dr["Isdefault"]) == "True")
                //    {
                //        ContactPerson = Convert.ToString(dr["add_id"]);
                //        break;
                //    }
                //}
                //cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(ContactPerson);

            }            
        }

        #region Billing Address
        public DataTable StoreSalesOrderAddressDetail()
        {


            DataTable AddressDetaildt = new DataTable();

            AddressDetaildt.Columns.Add("OrderAdd_OrderId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("OrderAdd_CompanyID", typeof(System.String));
            AddressDetaildt.Columns.Add("OrderAdd_BranchId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("OrderAdd_FinYear", typeof(System.String));

            AddressDetaildt.Columns.Add("OrderAdd_ContactPerson", typeof(System.String));
            AddressDetaildt.Columns.Add("OrderAdd_addressType", typeof(System.String));
            AddressDetaildt.Columns.Add("OrderAdd_address1", typeof(System.String));
            AddressDetaildt.Columns.Add("OrderAdd_address2", typeof(System.String));
            AddressDetaildt.Columns.Add("OrderAdd_address3", typeof(System.String));


            AddressDetaildt.Columns.Add("OrderAdd_landMark", typeof(System.String));
            AddressDetaildt.Columns.Add("OrderAdd_countryId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("OrderAdd_stateId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("OrderAdd_cityId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("OrderAdd_areaId", typeof(System.Int32));


            AddressDetaildt.Columns.Add("OrderAdd_pin", typeof(System.String));
            AddressDetaildt.Columns.Add("OrderAdd_CreatedDate", typeof(System.DateTime));
            AddressDetaildt.Columns.Add("OrderAdd_CreatedUser", typeof(System.Int32));
            AddressDetaildt.Columns.Add("OrderAdd_LastModifyDate", typeof(System.DateTime));
            AddressDetaildt.Columns.Add("OrderAdd_LastModifyUser", typeof(System.Int32));
            return AddressDetaildt;


        }
        protected void ComponentPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            Populatecountry();
            #region Addresss Lookup Section Start
            //DataSet dst = new DataSet();
            //CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            //dst = objCRMSalesDtlBL.PopulateBillingandShippingDetailByCustomerID(hdnCustomerId.Value);
            //billingAddress.DataSource = dst.Tables[0];
            //billingAddress.DataBind();
            //if (dst.Tables[0].Rows.Count > 0)
            //{
            //    Session["POBillingAddressLookup"] = dst.Tables[0];
            //}
            //shippingAddress.DataSource = dst.Tables[1];
            //shippingAddress.DataBind();
            //if (dst.Tables[1].Rows.Count > 0)
            //{
            //    Session["POShippingAddressLookup"] = dst.Tables[1];
            //}


            #endregion Addresss Lookup Section End

            #region Variable Declaration to send value using jsproperties Start
            string add_addressType = "";
            string add_address1 = "";
            string add_address2 = "";
            string add_address3 = "";
            string add_landMark = "";
            string add_country = "";
            string add_state = "";
            string add_city = "";
            string add_pin = "";
            string add_area = "";

            ///// shipping variable

            string add_saddressType = "";
            string add_saddress1 = "";
            string add_saddress2 = "";
            string add_saddress3 = "";
            string add_slandMark = "";
            string add_scountry = "";
            string add_sstate = "";
            string add_scity = "";
            string add_spin = "";
            string add_sarea = "";

            #endregion Variable Declaration to send value using jsproperties Start
            ComponentPanel.JSProperties["cpshow"] = null;
            ComponentPanel.JSProperties["cpshowShip"] = null;
            string WhichCall = e.Parameter.Split('~')[0];

            #region BillingLookup Edit Section Start
            if (WhichCall == "BlookupEdit")
            {
                //int BillingAddressID = Convert.ToInt32(e.Parameter.Split('~')[1]);
                //DataTable dt = objCRMSalesDtlBL.PopulateAddressDetailByAddressId(BillingAddressID);
                //if (dt.Rows.Count > 0 && dt != null)
                //{
                   
                //    add_addressType = Convert.ToString(dt.Rows[0]["add_addressType"]);
                //    add_address1 = Convert.ToString(dt.Rows[0]["add_address1"]);
                //    add_address2 = Convert.ToString(dt.Rows[0]["add_address2"]);
                //    add_address3 = Convert.ToString(dt.Rows[0]["add_address3"]);
                //    add_landMark = Convert.ToString(dt.Rows[0]["add_landMark"]);
                //    add_country = Convert.ToString(dt.Rows[0]["add_country"]);
                //    add_state = Convert.ToString(dt.Rows[0]["add_state"]);
                //    add_city = Convert.ToString(dt.Rows[0]["add_city"]);
                //    add_pin = Convert.ToString(dt.Rows[0]["add_pin"]);
                //    add_area = Convert.ToString(dt.Rows[0]["add_area"]);

                //    ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                //                                       + add_address1 + "~"
                //                                       + add_address2 + "~"
                //                                       + add_address3 + "~"
                //                                       + add_landMark + "~"
                //                                       + add_country + "~"
                //                                       + add_state + "~"
                //                                       + add_city + "~"
                //                                       + add_pin + "~"
                //                                       + add_area + "~"
                //                                       + "Y" + "~";

                //}
                //else
                //{
                //    ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                //                                      + add_address1 + "~"
                //                                      + add_address2 + "~"
                //                                      + add_address3 + "~"
                //                                      + add_landMark + "~"
                //                                      + add_country + "~"
                //                                      + add_state + "~"
                //                                      + add_city + "~"
                //                                      + add_pin + "~"
                //                                      + add_area + "~"
                //                                       + "N" + "~";
                //}

            }



            #endregion BillingLookup Edit Section Start

            #region ShippingLookup Edit Section Start
            if (WhichCall == "SlookupEdit")
            {
                //int AddressID = Convert.ToInt32(e.Parameter.Split('~')[1]);
                //DataTable dt = objCRMSalesDtlBL.PopulateAddressDetailByAddressId(AddressID);
                //if (dt.Rows.Count > 0 && dt != null)
                //{
                //    add_saddressType = Convert.ToString(dt.Rows[0]["add_addressType"]);
                //    add_saddress1 = Convert.ToString(dt.Rows[0]["add_address1"]);
                //    add_saddress2 = Convert.ToString(dt.Rows[0]["add_address2"]);
                //    add_saddress3 = Convert.ToString(dt.Rows[0]["add_address3"]);
                //    add_slandMark = Convert.ToString(dt.Rows[0]["add_landMark"]);
                //    add_scountry = Convert.ToString(dt.Rows[0]["add_country"]);
                //    add_sstate = Convert.ToString(dt.Rows[0]["add_state"]);
                //    add_scity = Convert.ToString(dt.Rows[0]["add_city"]);
                //    add_spin = Convert.ToString(dt.Rows[0]["add_pin"]);
                //    add_sarea = Convert.ToString(dt.Rows[0]["add_area"]);

                //    ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                //                                      + add_saddress1 + "~"
                //                                      + add_saddress2 + "~"
                //                                      + add_saddress3 + "~"
                //                                      + add_slandMark + "~"
                //                                      + add_scountry + "~"
                //                                      + add_sstate + "~"
                //                                      + add_scity + "~"
                //                                      + add_spin + "~"
                //                                      + add_sarea + "~"
                //                                      + "Y" + "~";

                //}
                //else
                //{
                //    ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                //                                      + add_saddress1 + "~"
                //                                      + add_saddress2 + "~"
                //                                      + add_saddress3 + "~"
                //                                      + add_slandMark + "~"
                //                                      + add_scountry + "~"
                //                                      + add_sstate + "~"
                //                                      + add_scity + "~"
                //                                      + add_spin + "~"
                //                                      + add_sarea + "~"
                //                                       + "N" + "~";
                //}

            }

            #endregion ShippingLookup Edit Section End
            #region Edit Section of Address Start

            if (WhichCall == "Edit")
            {

                //if (Session["PurchaseOrderAddressDtl"] == null)
                //{
                //    string customerid = hdnCustomerId.Value;
                //    #region Billing Detail fillup
                //    DataTable dtaddbill = oDBEngine.GetDataTable("select add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_pin,add_area from tbl_master_address where add_cntId='" + customerid + "' and add_addressType='Billing' and Isdefault='1' ");

                //    #region Function To get All Detail

                //    if (dtaddbill.Rows.Count > 0 && dtaddbill != null)
                //    {


                //        add_addressType = Convert.ToString(dtaddbill.Rows[0]["add_addressType"]);
                //        add_address1 = Convert.ToString(dtaddbill.Rows[0]["add_address1"]);
                //        add_address2 = Convert.ToString(dtaddbill.Rows[0]["add_address2"]);
                //        add_address3 = Convert.ToString(dtaddbill.Rows[0]["add_address3"]);
                //        add_landMark = Convert.ToString(dtaddbill.Rows[0]["add_landMark"]);
                //        add_country = Convert.ToString(dtaddbill.Rows[0]["add_country"]);
                //        add_state = Convert.ToString(dtaddbill.Rows[0]["add_state"]);
                //        add_city = Convert.ToString(dtaddbill.Rows[0]["add_city"]);
                //        add_pin = Convert.ToString(dtaddbill.Rows[0]["add_pin"]);
                //        add_area = Convert.ToString(dtaddbill.Rows[0]["add_area"]);



                //        ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                //                                           + add_address1 + "~"
                //                                           + add_address2 + "~"
                //                                           + add_address3 + "~"
                //                                           + add_landMark + "~"
                //                                           + add_country + "~"
                //                                           + add_state + "~"
                //                                           + add_city + "~"
                //                                           + add_pin + "~"
                //                                           + add_area + "~"
                //                                           + "Y" + "~";

                //    }
                //    else
                //    {
                //        ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                //                                          + add_address1 + "~"
                //                                          + add_address2 + "~"
                //                                          + add_address3 + "~"
                //                                          + add_landMark + "~"
                //                                          + add_country + "~"
                //                                          + add_state + "~"
                //                                          + add_city + "~"
                //                                          + add_pin + "~"
                //                                          + add_area + "~"
                //                                           + "N" + "~";
                //    }
                //    #endregion Function Calling End
                //    #endregion Billing Detail fillup end
                //    #region Shipping Detail fillup
                //    DataTable dtaship = oDBEngine.GetDataTable("select add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_pin,add_area from tbl_master_address where add_cntId='" + customerid + "' and add_addressType='Shipping' and Isdefault='1' ");
                //    if (dtaship.Rows.Count > 0 && dtaship != null)
                //    {
                //        add_saddressType = Convert.ToString(dtaship.Rows[0]["add_addressType"]);
                //        add_saddress1 = Convert.ToString(dtaship.Rows[0]["add_address1"]);
                //        add_saddress2 = Convert.ToString(dtaship.Rows[0]["add_address2"]);
                //        add_saddress3 = Convert.ToString(dtaship.Rows[0]["add_address3"]);
                //        add_slandMark = Convert.ToString(dtaship.Rows[0]["add_landMark"]);
                //        add_scountry = Convert.ToString(dtaship.Rows[0]["add_country"]);
                //        add_sstate = Convert.ToString(dtaship.Rows[0]["add_state"]);
                //        add_scity = Convert.ToString(dtaship.Rows[0]["add_city"]);
                //        add_spin = Convert.ToString(dtaship.Rows[0]["add_pin"]);
                //        add_sarea = Convert.ToString(dtaship.Rows[0]["add_area"]);

                //        ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                //                                          + add_saddress1 + "~"
                //                                          + add_saddress2 + "~"
                //                                          + add_saddress3 + "~"
                //                                          + add_slandMark + "~"
                //                                          + add_scountry + "~"
                //                                          + add_sstate + "~"
                //                                          + add_scity + "~"
                //                                          + add_spin + "~"
                //                                          + add_sarea + "~"
                //                                          + "Y" + "~";

                //    }
                //    else
                //    {
                //        ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                //                                          + add_saddress1 + "~"
                //                                          + add_saddress2 + "~"
                //                                          + add_saddress3 + "~"
                //                                          + add_slandMark + "~"
                //                                          + add_scountry + "~"
                //                                          + add_sstate + "~"
                //                                          + add_scity + "~"
                //                                          + add_spin + "~"
                //                                          + add_sarea + "~"
                //                                           + "N" + "~";
                //    }
                //    #endregion Shipping detail Fillup
                //}
                //else 
                if (Session["PurchaseOrderAddressDtl"] != null)
                {
                    DataTable dt = (DataTable)Session["PurchaseOrderAddressDtl"];
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows.Count == 2) // when 2 row  data found in edit mode
                        {
                            #region billing Address Dtl using session
                            add_addressType = Convert.ToString(dt.Rows[0]["OrderAdd_addressType"]);
                            add_address1 = Convert.ToString(dt.Rows[0]["OrderAdd_address1"]);
                            add_address2 = Convert.ToString(dt.Rows[0]["OrderAdd_address2"]);
                            add_address3 = Convert.ToString(dt.Rows[0]["OrderAdd_address3"]);
                            add_landMark = Convert.ToString(dt.Rows[0]["OrderAdd_landMark"]);
                            add_country = Convert.ToString(dt.Rows[0]["OrderAdd_countryId"]);
                            add_state = Convert.ToString(dt.Rows[0]["OrderAdd_stateId"]);
                            add_city = Convert.ToString(dt.Rows[0]["OrderAdd_cityId"]);
                            add_pin = Convert.ToString(dt.Rows[0]["OrderAdd_pin"]);
                            add_area = Convert.ToString(dt.Rows[0]["OrderAdd_areaId"]);

                            PopulateBilling(add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_pin, add_area);

                            ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                                  + add_address1 + "~"
                                                                  + add_address2 + "~"
                                                                  + add_address3 + "~"
                                                                  + add_landMark + "~"
                                                                  + add_country + "~"
                                                                  + add_state + "~"
                                                                  + add_city + "~"
                                                                  + add_pin + "~"
                                                                  + add_area + "~"
                                                                  + "Y" + "~";
                            #endregion billing Address Dtl using session
                            #region Shipping Address Dtl using session
                            add_saddressType = Convert.ToString(dt.Rows[1]["OrderAdd_addressType"]);
                            add_saddress1 = Convert.ToString(dt.Rows[1]["OrderAdd_address1"]);
                            add_saddress2 = Convert.ToString(dt.Rows[1]["OrderAdd_address2"]);
                            add_saddress3 = Convert.ToString(dt.Rows[1]["OrderAdd_address3"]);
                            add_slandMark = Convert.ToString(dt.Rows[1]["OrderAdd_landMark"]);
                            add_scountry = Convert.ToString(dt.Rows[1]["OrderAdd_countryId"]);
                            add_sstate = Convert.ToString(dt.Rows[1]["OrderAdd_stateId"]);
                            add_scity = Convert.ToString(dt.Rows[1]["OrderAdd_cityId"]);
                            add_spin = Convert.ToString(dt.Rows[1]["OrderAdd_pin"]);
                            add_sarea = Convert.ToString(dt.Rows[1]["OrderAdd_areaId"]);

                            PopulateShipping(add_saddress1, add_saddress2, add_saddress3, add_slandMark, add_scountry, add_sstate, add_scity, add_spin, add_sarea);

                            ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                                 + add_saddress1 + "~"
                                                                 + add_saddress2 + "~"
                                                                 + add_saddress3 + "~"
                                                                 + add_slandMark + "~"
                                                                 + add_scountry + "~"
                                                                 + add_sstate + "~"
                                                                 + add_scity + "~"
                                                                 + add_spin + "~"
                                                                 + add_sarea + "~"
                                                                 + "Y" + "~";
                            #endregion Shipping Address Dtl using session end

                        }
                        else if (dt.Rows.Count == 1) // when 1 row  data found in edit mode
                        {
                            if (Convert.ToString(dt.Rows[0]["OrderAdd_addressType"]) == "Billing")
                            {
                                #region billing Address Dtl using session
                                add_addressType = Convert.ToString(dt.Rows[0]["OrderAdd_addressType"]);
                                add_address1 = Convert.ToString(dt.Rows[0]["OrderAdd_address1"]);
                                add_address2 = Convert.ToString(dt.Rows[0]["OrderAdd_address2"]);
                                add_address3 = Convert.ToString(dt.Rows[0]["OrderAdd_address3"]);
                                add_landMark = Convert.ToString(dt.Rows[0]["OrderAdd_landMark"]);
                                add_country = Convert.ToString(dt.Rows[0]["OrderAdd_countryId"]);
                                add_state = Convert.ToString(dt.Rows[0]["OrderAdd_stateId"]);
                                add_city = Convert.ToString(dt.Rows[0]["OrderAdd_cityId"]);
                                add_pin = Convert.ToString(dt.Rows[0]["OrderAdd_pin"]);
                                add_area = Convert.ToString(dt.Rows[0]["OrderAdd_areaId"]);

                                PopulateBilling(add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_pin, add_area);

                                ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                                      + add_address1 + "~"
                                                                      + add_address2 + "~"
                                                                      + add_address3 + "~"
                                                                      + add_landMark + "~"
                                                                      + add_country + "~"
                                                                      + add_state + "~"
                                                                      + add_city + "~"
                                                                      + add_pin + "~"
                                                                      + add_area + "~"
                                                                      + "Y" + "~";

                                ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                     + add_saddress1 + "~"
                                                     + add_saddress2 + "~"
                                                     + add_saddress3 + "~"
                                                     + add_slandMark + "~"
                                                     + add_scountry + "~"
                                                     + add_sstate + "~"
                                                     + add_scity + "~"
                                                     + add_spin + "~"
                                                     + add_sarea + "~"
                                                      + "N" + "~";

                                #endregion billing Address Dtl using session
                            }
                            if (Convert.ToString(dt.Rows[0]["OrderAdd_addressType"]) == "Shipping")
                            {
                                #region Shipping Address Dtl using session
                                ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                     + add_address1 + "~"
                                                     + add_address2 + "~"
                                                     + add_address3 + "~"
                                                     + add_landMark + "~"
                                                     + add_country + "~"
                                                     + add_state + "~"
                                                     + add_city + "~"
                                                     + add_pin + "~"
                                                     + add_area + "~"
                                                      + "N" + "~";

                                add_saddressType = Convert.ToString(dt.Rows[0]["OrderAdd_addressType"]);
                                add_saddress1 = Convert.ToString(dt.Rows[0]["OrderAdd_address1"]);
                                add_saddress2 = Convert.ToString(dt.Rows[0]["OrderAdd_address2"]);
                                add_saddress3 = Convert.ToString(dt.Rows[0]["OrderAdd_address3"]);
                                add_slandMark = Convert.ToString(dt.Rows[0]["OrderAdd_landMark"]);
                                add_scountry = Convert.ToString(dt.Rows[0]["OrderAdd_countryId"]);
                                add_sstate = Convert.ToString(dt.Rows[0]["OrderAdd_stateId"]);
                                add_scity = Convert.ToString(dt.Rows[0]["OrderAdd_cityId"]);
                                add_spin = Convert.ToString(dt.Rows[0]["OrderAdd_pin"]);
                                add_sarea = Convert.ToString(dt.Rows[0]["OrderAdd_areaId"]);

                                PopulateShipping(add_saddress1, add_saddress2, add_saddress3, add_slandMark, add_scountry, add_sstate, add_scity, add_spin, add_sarea);

                                ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                                     + add_saddress1 + "~"
                                                                     + add_saddress2 + "~"
                                                                     + add_saddress3 + "~"
                                                                     + add_slandMark + "~"
                                                                     + add_scountry + "~"
                                                                     + add_sstate + "~"
                                                                     + add_scity + "~"
                                                                     + add_spin + "~"
                                                                     + add_sarea + "~"
                                                                     + "Y" + "~";
                                #endregion Shipping Address Dtl using session end
                            }
                        }
                        else // when no data found in edit mode
                        {
                            #region billing Address Dtl using session

                            ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                                  + add_address1 + "~"
                                                                  + add_address2 + "~"
                                                                  + add_address3 + "~"
                                                                  + add_landMark + "~"
                                                                  + add_country + "~"
                                                                  + add_state + "~"
                                                                  + add_city + "~"
                                                                  + add_pin + "~"
                                                                  + add_area + "~"
                                                                  + "Y" + "~";
                            #endregion billing Address Dtl using session
                            #region Shipping Address Dtl using session

                            ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                                 + add_saddress1 + "~"
                                                                 + add_saddress2 + "~"
                                                                 + add_saddress3 + "~"
                                                                 + add_slandMark + "~"
                                                                 + add_scountry + "~"
                                                                 + add_sstate + "~"
                                                                 + add_scity + "~"
                                                                 + add_spin + "~"
                                                                 + add_sarea + "~"
                                                                 + "Y" + "~";

                            #endregion Shipping Address Dtl using session end

                        }
                    }
                }
            }
            #endregion Edit Section of Address End

            #region Save Section of Address Start
            if (WhichCall == "save")
            {
                #region Global Data for Address Start
                string companyId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                int branchid = Convert.ToInt32(HttpContext.Current.Session["userbranchID"]);
                string fin_year = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                #endregion Global Data for Address End
                string AddressStatus = e.Parameter.Split('~')[1];
                if (AddressStatus == "1") // Both Billing and Shipping Address Available
                {
                    #region Billing Address Detail Start
                    string contactperson = "";


                    //int insertcount = 0;

                    //string AddressType = Convert.ToString(CmbAddressType.SelectedItem.Value);
                    string AddressType = "Billing";
                    string address1 = txtAddress1.Text;
                    string address2 = txtAddress2.Text;
                    string address3 = txtAddress3.Text;
                    string landmark = txtlandmark.Text;
                    int country = Convert.ToInt32(Convert.ToString( CmbCountry.Value).TrimStart(','));
                    int State = Convert.ToInt32(Convert.ToString(CmbState.Value).TrimStart(','));
                    int city = Convert.ToInt32(Convert.ToString(CmbCity.Value).TrimStart(','));
                    int area = Convert.ToInt32(Convert.ToString(CmbArea.Value).TrimStart(',') == "" ? "0" : Convert.ToString(CmbArea.Value).TrimStart(','));
                    string pin = Convert.ToString(Convert.ToString(CmbPin.Value).TrimStart(',') == "" ? "0" : Convert.ToString(CmbPin.Value).TrimStart(','));
                    DataTable dt = StoreSalesOrderAddressDetail();
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", AddressType, address1, address2, address3, landmark, country, State, city, area, pin, System.DateTime.Now, userid, System.DateTime.Now, userid);




                    #endregion Billing Address Detail Start end

                    #region Shipping Address Detail Start
                    // CRMSalesAddressEL objCRMSalesSAddress  = new CRMSalesAddressEL();
                    string scontactperson = "";
                    //string sAddressType = Convert.ToString(CmbAddressType1.SelectedItem.Value);
                    string sAddressType = "Shipping";
                    string saddress1 = txtsAddress1.Text;
                    string saddress2 = txtsAddress2.Text;
                    string saddress3 = txtsAddress3.Text;
                    string slandmark = txtslandmark.Text;
                    int scountry = Convert.ToInt32(CmbCountry1.SelectedItem.Value);
                    int sState = Convert.ToInt32(CmbState1.Value);
                    int scity = Convert.ToInt32(CmbCity1.Value);
                    int sarea = Convert.ToInt32(CmbArea1.Value);
                    string spin = Convert.ToString(CmbPin1.Value);
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", sAddressType, saddress1, saddress2, saddress3, slandmark, scountry, sState, scity, sarea, spin, System.DateTime.Now, userid, System.DateTime.Now, userid);


                    Session["PurchaseOrderAddressDtl"] = dt;
                    #endregion Shipping Address Detail Start end
                }
                else if (AddressStatus == "2") // Copy Billing to Shipping Address
                {
                    //string AddressType = Convert.ToString(CmbAddressType.SelectedItem.Value);
                    string AddressType = "Billing";
                    string address1 = txtAddress1.Text;
                    string address2 = txtAddress2.Text;
                    string address3 = txtAddress3.Text;
                    string landmark = txtlandmark.Text;
                    int country = Convert.ToInt32(CmbCountry.SelectedItem.Value);
                    int State = Convert.ToInt32(CmbState.Value);
                    int city = Convert.ToInt32(CmbCity.Value);
                    int area = Convert.ToInt32(CmbArea.Value);
                    string pin = Convert.ToString(CmbPin.Value);
                    DataTable dt = StoreSalesOrderAddressDetail();
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", AddressType, address1, address2, address3, landmark, country, State, city, area, pin, System.DateTime.Now, userid, System.DateTime.Now, userid);
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", "Shipping", address1, address2, address3, landmark, country, State, city, area, pin, System.DateTime.Now, userid, System.DateTime.Now, userid);
                    Session["PurchaseOrderAddressDtl"] = dt;
                }
                else if (AddressStatus == "3") // Copy  Shipping to Billing  Address
                {
                    string scontactperson = "";
                    //string sAddressType = Convert.ToString(CmbAddressType1.SelectedItem.Value);
                    string sAddressType = "Shipping";
                    string saddress1 = txtsAddress1.Text;
                    string saddress2 = txtsAddress2.Text;
                    string saddress3 = txtsAddress3.Text;
                    string slandmark = txtslandmark.Text;
                    int scountry = Convert.ToInt32(CmbCountry1.SelectedItem.Value);
                    int sState = Convert.ToInt32(CmbState1.Value);
                    int scity = Convert.ToInt32(CmbCity1.Value);
                    int sarea = Convert.ToInt32(CmbArea1.Value);
                    string spin = Convert.ToString(CmbPin1.Value);
                    DataTable dt = StoreSalesOrderAddressDetail();
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", "Billing", saddress1, saddress2, saddress3, slandmark, scountry, sState, scity, sarea, spin, System.DateTime.Now, userid, System.DateTime.Now, userid);
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", sAddressType, saddress1, saddress2, saddress3, slandmark, scountry, sState, scity, sarea, spin, System.DateTime.Now, userid, System.DateTime.Now, userid);
                    Session["PurchaseOrderAddressDtl"] = dt;
                }

            }

            #endregion Save Section of Address Start

        }
        #endregion



        #region populate Billing Address
        public void PopulateBilling(string add_address1, string add_address2, string add_address3, string add_landMark, string add_country, string add_state, string add_city, string add_pin, string add_area)
        {
            DataTable dtcountry = new DataTable();

            dtcountry = oDBEngine.GetDataTable("SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country");

            CmbCountry.DataSource = dtcountry;
            CmbCountry.TextField = "Country";
            CmbCountry.ValueField = "cou_id";
            CmbCountry.DataBind();
            if (!string.IsNullOrEmpty(add_country))
            {
                CmbCountry.Value = add_country;
            }
            DataTable dtstate = new DataTable();

            dtstate = oDBEngine.GetDataTable("SELECT s.id as ID,s.state+' (State Code:' +s.StateCode+')' as State from tbl_master_state s where (s.countryId = " + add_country + ") ORDER BY s.state");



            CmbState.DataSource = dtstate;
            CmbState.TextField = "State";
            CmbState.ValueField = "ID";
            CmbState.DataBind();
            if (!string.IsNullOrEmpty(add_state))
            {
                CmbState.Value = add_state;
            }
            DataTable dtcity = new DataTable();

            dtcity = oDBEngine.GetDataTable("SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=" + add_state + " order by c.city_name");


            CmbCity.DataSource = dtcity;
            CmbCity.TextField = "City";
            CmbCity.ValueField = "CityId";
            CmbCity.DataBind();

            if (!string.IsNullOrEmpty(add_city))
            {
                CmbCity.Value = add_city;

            }
            DataTable dtpin = new DataTable();
            dtpin = oDBEngine.GetDataTable("select pin_id,pin_code from tbl_master_pinzip where city_id=" + add_city + " order by pin_code");


            CmbPin.DataSource = dtpin;
            CmbPin.TextField = "pin_code";
            CmbPin.ValueField = "pin_id";
            CmbPin.DataBind();

            if (!string.IsNullOrEmpty(add_pin))
            {
                CmbPin.Value = add_pin;
            }

            DataTable dtarea = new DataTable();
            dtarea = oDBEngine.GetDataTable("SELECT area_id, area_name from tbl_master_area where (city_id = " + add_city + ") ORDER BY area_name");

            CmbArea.DataSource = dtarea;
            CmbArea.TextField = "area_name";
            CmbArea.ValueField = "area_id";
            CmbArea.DataBind();

            CmbArea.Value = add_area;



            txtAddress1.Text = add_address1;
            txtAddress2.Text = add_address2;
            txtAddress3.Text = add_address3;
            txtlandmark.Text = add_landMark;

        }
        #endregion

        #region populate Shipping Address
        public void PopulateShipping(string add_saddress1, string add_saddress2, string add_saddress3, string add_slandMark, string add_scountry, string add_sstate, string add_scity, string add_spin, string add_sarea)
        {
            DataTable dtcountry1 = new DataTable();

            dtcountry1 = oDBEngine.GetDataTable("SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country");

            CmbCountry1.DataSource = dtcountry1;
            CmbCountry1.TextField = "Country";
            CmbCountry1.ValueField = "cou_id";
            CmbCountry1.DataBind();
            if (!string.IsNullOrEmpty(add_scountry))
            {
                CmbCountry1.Value = add_scountry;
            }
            DataTable dtstate1 = new DataTable();

            dtstate1 = oDBEngine.GetDataTable("SELECT s.id as ID,s.state+' (State Code:' +s.StateCode+')' as State from tbl_master_state s where (s.countryId = " + add_scountry + ") ORDER BY s.state");


            CmbState1.DataSource = dtstate1;
            CmbState1.TextField = "State";
            CmbState1.ValueField = "ID";
            CmbState1.DataBind();
            if (!string.IsNullOrEmpty(add_sstate))
            {
                CmbState1.Value = add_sstate;
            }

            DataTable dtcity1 = new DataTable();
            dtcity1 = oDBEngine.GetDataTable("SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=" + add_sstate + " order by c.city_name");

            CmbCity1.DataSource = dtcity1;
            CmbCity1.TextField = "City";
            CmbCity1.ValueField = "CityId";
            CmbCity1.DataBind();
            if (!string.IsNullOrEmpty(add_scity))
            {
                CmbCity1.Value = add_scity;
            }


            DataTable dtpin1 = new DataTable();
            dtpin1 = oDBEngine.GetDataTable("select pin_id,pin_code from tbl_master_pinzip where city_id=" + add_scity + " order by pin_code");


            CmbPin1.DataSource = dtpin1;
            CmbPin1.TextField = "pin_code";
            CmbPin1.ValueField = "pin_id";
            CmbPin1.DataBind();
            if (!string.IsNullOrEmpty(add_spin))
            {
                CmbPin1.Value = add_spin;
            }

            DataTable dtarea1 = new DataTable();
            dtarea1 = oDBEngine.GetDataTable("SELECT area_id, area_name from tbl_master_area where (city_id = " + add_scity + ") ORDER BY area_name");

            CmbArea1.DataSource = dtarea1;
            CmbArea1.TextField = "area_name";
            CmbArea1.ValueField = "area_id";
            CmbArea1.DataBind();

            if (!string.IsNullOrEmpty(add_sarea))
            {
                CmbArea1.Value = add_sarea;
            }


            txtsAddress1.Text = add_saddress1;
            txtsAddress2.Text = add_saddress2;
            txtsAddress3.Text = add_saddress3;
            txtslandmark.Text = add_slandMark;

        }
        #endregion

        #region Populate Country
        public void Populatecountry()
        {
            DataTable dtcountry = new DataTable();
            dtcountry = oDBEngine.GetDataTable("SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country");
            CmbCountry.DataSource = dtcountry;
            CmbCountry.TextField = "Country";
            CmbCountry.ValueField = "cou_id";
            CmbCountry.DataBind();

            CmbCountry1.DataSource = dtcountry;
            CmbCountry1.TextField = "Country";
            CmbCountry1.ValueField = "cou_id";
            CmbCountry1.DataBind();
        }

        #endregion




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
            if (strSplitCommand == "Display")
            {
                string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
                if (IsDeleteFrom == "D")
                {
                    DataTable POdt = (DataTable)Session["ProductOrderDetails"];
                    grid.DataSource = GetPurchaseOrderBatch(POdt);
                    grid.DataBind();
                }
            }
            else if (strSplitCommand == "GridBlank")
            {
                Session["ProductOrderDetails"] = null;
                grid.DataSource = null;
                grid.DataBind();
            }
            else if (strSplitCommand == "BindGridOnQuotation")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                String QuoComponent1 = "";
                string Product_id1 = "";
                string QuoteDetails_Id = "";

                if (grid_Products.GetSelectedFieldValues("Quotation_No").Count > 0)
                {
                    for (int i = 0; i < grid_Products.GetSelectedFieldValues("Quotation_No").Count; i++)
                    {

                        QuoComponent1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("Quotation_No")[i]);
                        Product_id1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("gvColProduct")[i]);
                        QuoteDetails_Id += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("QuoteDetails_Id")[i]);

                    }
                    QuoComponent1 = QuoComponent1.TrimStart(',');
                    Product_id1 = Product_id1.TrimStart(',');
                    QuoteDetails_Id = QuoteDetails_Id.TrimStart(',');


                    string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);

                    if (Quote_Nos != "$")
                    {

                        DataTable dt_QuotationDetails = new DataTable();
                        string IdKey = Convert.ToString(Request.QueryString["key"]);
                        if (!string.IsNullOrEmpty(IdKey))
                        {
                            if (IdKey != "ADD")
                            {
                                dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsForPOGridBind(QuoComponent1, QuoteDetails_Id, Product_id1, "Edit").Tables[0];
                            }
                            else
                            {
                                dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsForPOGridBind(QuoComponent1, QuoteDetails_Id, Product_id1, "Add").Tables[0];
                            }

                        }
                        else
                        {
                            //dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsForPOGridBind(QuoComponent1, QuoteDetails_Id, Product_id1);
                        }
                        Session["ProductOrderDetails"] = null;
                        grid.DataSource = GetSalesOrderInfo(dt_QuotationDetails, IdKey);
                        grid.DataBind();
                    }
                    else
                    {
                        grid.DataSource = null;
                        grid.DataBind();
                    }
                }
                else
                {
                    grid.DataSource = null;
                    grid.DataBind();
                }

            }
        }
        public IEnumerable GetSalesOrderInfo(DataTable SalesOrderdt1, string Order_Id)
        {
            List<SalesOrder> OrderList = new List<SalesOrder>();
            DataColumnCollection dtC = SalesOrderdt1.Columns;

        

            for (int i = 0; i < SalesOrderdt1.Rows.Count; i++)
            {
                SalesOrder Orders = new SalesOrder();

                Orders.SrlNo = Convert.ToString(i + 1);
                Orders.OrderDetails_Id = Convert.ToString(i + 1);
                Orders.gvColProduct = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductId"]);
                Orders.gvColDiscription = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductDescription"]);
                Orders.gvColQuantity = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Quantity"]);
                Orders.gvColUOM = Convert.ToString(SalesOrderdt1.Rows[i]["UOM_ShortName"]);
                Orders.Warehouse = "";
                Orders.gvColStockQty = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_StockQty"]);
                Orders.gvColStockUOM = Convert.ToString(SalesOrderdt1.Rows[i]["UOM_ShortName"]);

                Orders.gvColStockPurchasePrice = Convert.ToString(SalesOrderdt1.Rows[i]["SalePrice"]);
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

            BindSessionByDatatable(SalesOrderdt1);

            return OrderList;
        }

        #region Subhabrata/SessionBind
        //Subhabrata on 02-03-2017
        public bool BindSessionByDatatable(DataTable dt)
        {
            bool IsSuccess = false;
            DataTable purChaseDT = new DataTable();
            purChaseDT.Columns.Add("SrlNo", typeof(string));
            purChaseDT.Columns.Add("OrderDetails_Id", typeof(string));
            purChaseDT.Columns.Add("ProductID", typeof(string));
            purChaseDT.Columns.Add("Description", typeof(string));
            //SalesOrderdt.Columns.Add("Quotation", typeof(string));//Added By:subhabrata on 21-02-2017               
            purChaseDT.Columns.Add("Quantity", typeof(string));
            purChaseDT.Columns.Add("UOM", typeof(string));
            purChaseDT.Columns.Add("Warehouse", typeof(string));
            purChaseDT.Columns.Add("StockQuantity", typeof(string));
            purChaseDT.Columns.Add("StockUOM", typeof(string));
            //purChaseDT.Columns.Add("SalePrice", typeof(string));
            purChaseDT.Columns.Add("PurchasePrice", typeof(string));
            purChaseDT.Columns.Add("Discount", typeof(string));
            purChaseDT.Columns.Add("Amount", typeof(string));
            purChaseDT.Columns.Add("TaxAmount", typeof(string));
            purChaseDT.Columns.Add("TotalAmount", typeof(string));
            purChaseDT.Columns.Add("Status", typeof(string));
            purChaseDT.Columns.Add("Indent_No", typeof(string));
            purChaseDT.Columns.Add("Indent_Num", typeof(string));
            purChaseDT.Columns.Add("ProductName", typeof(string));
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IsSuccess = true;
                DataColumnCollection dtC = dt.Columns;
                string SalePrice, Discount, Amount, TaxAmount, TotalAmount, Order_Num1, ProductName;
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
                purChaseDT.Rows.Add(Convert.ToString(i + 1), Convert.ToString(i + 1), Convert.ToString(dt.Rows[i]["QuoteDetails_ProductId"]), Convert.ToString(dt.Rows[i]["QuoteDetails_ProductDescription"]),
                    Convert.ToString(dt.Rows[i]["QuoteDetails_Quantity"]), Convert.ToString(dt.Rows[i]["UOM_ShortName"]), "", Convert.ToString(dt.Rows[i]["QuoteDetails_StockQty"]), Convert.ToString(dt.Rows[i]["UOM_ShortName"]),
                               SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", Convert.ToInt64(dt.Rows[i]["Indent_No"]), Order_Num1, ProductName);
            }

            Session["ProductOrderDetails"] = purChaseDT;

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
            if (Session["ProductOrderDetails"] != null)
            {
                DataTable POdt = (DataTable)Session["ProductOrderDetails"];
                DataView dvData = new DataView(POdt);
                dvData.RowFilter = "Status <> 'D'";
                grid.DataSource = GetPurchaseOrderBatch(dvData.ToTable());
            }
        }

        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            grid.JSProperties["cpSaveSuccessOrFail"] = null;
            grid.JSProperties["cpPurchaseOrderNo"] = null;
            DataTable PurchaseOrderdt = new DataTable();
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);

            if (Session["ProductOrderDetails"] != null)
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["ProductOrderDetails"];
                foreach (DataRow row in dt.Rows)
                {
                    DataColumnCollection dtC = dt.Columns;

                    if (dtC.Contains("Indent_Num"))
                    { dt.Columns.Remove("Indent_Num"); }
                    break;
                }//End
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
                PurchaseOrderdt.Columns.Add("Discount", typeof(string));
                PurchaseOrderdt.Columns.Add("Amount", typeof(string));
                PurchaseOrderdt.Columns.Add("TaxAmount", typeof(string));
                PurchaseOrderdt.Columns.Add("TotalAmount", typeof(string));
                PurchaseOrderdt.Columns.Add("Status", typeof(string));
                PurchaseOrderdt.Columns.Add("Indent_No", typeof(string));
                PurchaseOrderdt.Columns.Add("ProductName", typeof(string));
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
                    string UOM = Convert.ToString(ProductDetailsList[3]);
                    // string UOM = Convert.ToString(args.NewValues["gvColUOM"]);
                    string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
                    string StockQuantity = Convert.ToString(args.NewValues["gvColStockQty"]);
                    //string StockUOM = Convert.ToString(args.NewValues["gvColStockUOM"]);
                    string StockUOM = Convert.ToString(ProductDetailsList[5]);
                    string PurchasePrice = Convert.ToString(args.NewValues["gvColStockPurchasePrice"]);
                    // string PurchasePrice = Convert.ToString(ProductDetailsList[6]);
                    string Discount = Convert.ToString(args.NewValues["gvColDiscount"]);
                    string Amount = (Convert.ToString(args.NewValues["gvColAmount"]) != "") ? Convert.ToString(args.NewValues["gvColAmount"]) : "0";
                    string TaxAmount = (Convert.ToString(args.NewValues["gvColTaxAmount"]) != "") ? Convert.ToString(args.NewValues["gvColTaxAmount"]) : "0";
                    string TotalAmount = (Convert.ToString(args.NewValues["gvColTotalAmountINR"]) != "") ? Convert.ToString(args.NewValues["gvColTotalAmountINR"]) : "0";
                    string Indent_Id = (Convert.ToString(args.NewValues["Indent"]) != "") ? Convert.ToString(args.NewValues["Indent"]) : "0";
                    PurchaseOrderdt.Rows.Add(SrlNo, "0", ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, PurchasePrice, Discount,
                        Amount, TaxAmount, TotalAmount, "I",Indent_Id, ProductName);
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
                        string UOM = Convert.ToString(ProductDetailsList[3]);
                        string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                        // string UOM = Convert.ToString(args.NewValues["gvColUOM"]);
                        string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
                        string StockQuantity = Convert.ToString(args.NewValues["gvColStockQty"]);
                        //string StockUOM = Convert.ToString(args.NewValues["gvColStockUOM"]);
                        string StockUOM = Convert.ToString(ProductDetailsList[5]);
                        string PurchasePrice = Convert.ToString(args.NewValues["gvColStockPurchasePrice"]);
                        // string PurchasePrice = Convert.ToString(ProductDetailsList[6]);
                        string Discount = Convert.ToString(args.NewValues["gvColDiscount"]);
                        string Amount = (Convert.ToString(args.NewValues["gvColAmount"]) != "") ? Convert.ToString(args.NewValues["gvColAmount"]) : "0";
                        string TaxAmount = (Convert.ToString(args.NewValues["gvColTaxAmount"]) != "") ? Convert.ToString(args.NewValues["gvColTaxAmount"]) : "0";
                        string TotalAmount = (Convert.ToString(args.NewValues["gvColTotalAmountINR"]) != "") ? Convert.ToString(args.NewValues["gvColTotalAmountINR"]) : "0";
                        string Indent_Id = (Convert.ToString(args.NewValues["Indent"]) != "") ? Convert.ToString(args.NewValues["Indent"]) : "0";
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
                                drr["Discount"] = Discount;
                                drr["Amount"] = Amount;
                                drr["TaxAmount"] = TaxAmount;
                                drr["TotalAmount"] = TotalAmount;
                                drr["Status"] = "U";
                                if (!string.IsNullOrEmpty(Indent_Id))
                                { drr["Indent_No"] = Indent_Id; }
                                drr["ProductName"] = ProductName;
                                break;
                            }
                        }

                        if (Isexists == false)
                        {
                            PurchaseOrderdt.Rows.Add(SrlNo, OrderDetails_Id, ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM,
                                PurchasePrice, Discount, Amount, TaxAmount, TotalAmount, "U", Indent_Id, ProductName);
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
                    PurchaseOrderdt.Rows.Add("0", OrderDetails_Id, "0", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "D", "0", "");
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

            Session["ProductOrderDetails"] = PurchaseOrderdt;
            if (IsDeleteFrom != "D")
            {
                string ActionType = Convert.ToString(ViewState["ActionType"]);
                string PurchaseOrder_Id = Convert.ToString(Session["PurchaseOrder_Id"]);
                string strSchemeType = Convert.ToString(ddl_numberingScheme.SelectedValue);
                string strPurchaseNumber = Convert.ToString(txtVoucherNo.Text);
                string strPurchaseDate = Convert.ToString(dt_PLQuote.Date);
                // string IndentRequisitionNo = Convert.ToString(ddl_IndentRequisitionNo.SelectedValue);
                String IndentRequisitionNo = "";
                List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("Indent_Id");
                foreach (object Quo in QuoList)
                {
                    IndentRequisitionNo += "," + Quo;
                }
                IndentRequisitionNo = IndentRequisitionNo.TrimStart(',');
                //string IndentRequisitionDate = Convert.ToString(txtDateIndentRequis.Date);
                string[] eachQuo = IndentRequisitionNo.Split(',');
                string IndentRequisitionDate = string.Empty;
                if (eachQuo.Length == 1)
                {
                    IndentRequisitionDate = dt_Quotation.Text;
                }
                else
                {
                    IndentRequisitionDate = "";
                }
                //string strVendor = Convert.ToString(ddl_Vendor.SelectedValue);
                string strVendor = Convert.ToString(hdfLookupCustomer.Value);
                string strContactName = Convert.ToString(cmbContactPerson.Value);
                string Reference = Convert.ToString(txt_Refference.Text);
                string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
                string strAgents = Convert.ToString(ddl_SalesAgent.SelectedValue);
                string strCurrency = Convert.ToString(ddl_Currency.SelectedValue);
                string strRate = Convert.ToString(txt_Rate.Value);
                string strTaxOption = Convert.ToString(ddl_AmountAre.Value);
                string strTaxCode = Convert.ToString(ddl_VatGstCst.Value).Split('~')[0]; ;



                string CompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string currency = Convert.ToString(HttpContext.Current.Session["LocalCurrency"]);
                string[] ActCurrency = currency.Split('~');
                int BaseCurrencyId = Convert.ToInt32(ActCurrency[0]);



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
                // DataTable Of Billing Address

                DataTable tempBillAddress = new DataTable();
                if (Session["PurchaseOrderAddressDtl"] != null)
                {
                    tempBillAddress = (DataTable)Session["PurchaseOrderAddressDtl"];
                }
                else
                {
                    tempBillAddress = StoreSalesOrderAddressDetail();
                }

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
                    string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);
                    validate = checkNMakeJVCode(strPurchaseNumber, Convert.ToInt32(SchemeList[0]));

                    if (!reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "PO"))
                    {
                        validate = "UdfMandetory";
                    }
                }

                //// ############# Added By : Samrat Roy -- 02/05/2017 -- To check Transporter Mandatory Control 
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Transporter_Mandatory' AND IsActive=1");
                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();
                    //objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                    objEngine = new BusinessLogicLayer.DBEngine();
                    DataTable DTVisible = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_Transporter' AND IsActive=1");
                    if (Convert.ToString(DTVisible.Rows[0]["Variable_Value"]).Trim() == "Yes")
                    {
                        if (IsMandatory == "Yes")
                        {
                            //if (VehicleDetailsControl.GetControlValue("cmbTransporter") == "")
                            if (hfControlData.Value.Trim() == "")
                            {
                                validate = "transporteMandatory";
                            }
                        }
                    }
                }

                //// ############# Added By : Samrat Roy -- 02/05/2017 -- To check Transporter Mandatory Control 

                if (validate == "outrange" || validate == "duplicate" || validate == "UdfMandetory" || validate == "transporteMandatory" || validate == "nullQuantity")
                {
                    grid.JSProperties["cpSaveSuccessOrFail"] = validate;
                }
                else
                {
                    if (IsPOExist(IndentRequisitionNo))
                    {

                        string strFinYear = GetFinancialYearCheckAccordingDaterange(Convert.ToDateTime(dt_PLQuote.Value)); 
                        if (AddModifyPurchaseOrder(PurchaseOrder_Id, UniquePurchaseNumber, strPurchaseDate, IndentRequisitionNo, IndentRequisitionDate, strVendor, strContactName,
                        Reference, strBranch, strAgents, strCurrency, strRate, strTaxOption, strTaxCode, CompanyID, BaseCurrencyId, tempQuotation, ActionType,
                        tempWarehousedt, TaxDetaildt, tempTaxDetailsdt, tempBillAddress, approveStatus, strFinYear) == false)
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
                            if (ActionType == "Add") // session has been removed from quotation list page working good
                            {
                                //string[] schemaid = new string[] { };
                                string schemavalue = ddl_numberingScheme.SelectedValue;
                                Session["schemavaluePO"] = schemavalue;        // session has been removed from quotation list page working good
                                //schemaid = ddl_numberingScheme.SelectedValue.Split('~');

                                //string schematype = schemaid[1];
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

                        if (Session["ProductOrderDetails"] != null)
                        {
                            Session["ProductOrderDetails"] = null;

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
        private bool IsPOExist(string pcid)
        {
            bool IsExist = false;
            if (pcid != "" && Convert.ToString(pcid).Trim() != "")
            {
                DataTable dt = new DataTable();

                var elements = pcid.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string items in elements)
                {
                    dt = oDBEngine.GetDataTable("select Count(*) as isexist from tbl_trans_Indent where Indent_Id=" + items + "");
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
            // DataTable Quotationdt = GetPurchaseOrderData().Tables[0];
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
                PurchaseOrders.gvColDiscount = Convert.ToString(Quotationdt.Rows[i]["Discount"]);
                PurchaseOrders.gvColAmount = Convert.ToString(Quotationdt.Rows[i]["Amount"]);
                PurchaseOrders.gvColTaxAmount = Convert.ToString(Quotationdt.Rows[i]["TaxAmount"]);
                PurchaseOrders.gvColTotalAmountINR = Convert.ToString(Quotationdt.Rows[i]["TotalAmount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(Quotationdt.Rows[i]["Indent_No"])))
                { PurchaseOrders.Indent = Convert.ToInt64(Quotationdt.Rows[i]["Indent_No"]); }
                else
                { PurchaseOrders.Indent = 0; }
                if (dtC.Contains("Indent_Num"))
                {
                    PurchaseOrders.Indent_Num = Convert.ToString(Quotationdt.Rows[i]["Indent_Num"]);
                }
                PurchaseOrders.ProductName = Convert.ToString(Quotationdt.Rows[i]["ProductName"]);

                PurchaseOrderList.Add(PurchaseOrders);
            }

            return PurchaseOrderList;
        }
        public bool AddModifyPurchaseOrder(string PurchaseOrder_Id, string UniquePurchaseNumber, string strPurchaseDate, string IndentRequisitionNo, string IndentRequisitionDate,

            string strVendor, string strContactName,
                                   string Reference, string strBranch, string strAgents, string strCurrency, string strRate, string strTaxOption,
            string strTaxCode, string CompanyID, int BaseCurrencyId, DataTable PurchaseOrderdt,
                                    string ActionType, DataTable Warehousedt, DataTable TaxDetaildt, DataTable PurchaseOrderTaxdt,DataTable tempBillAddress,
            
            string approveStatus,string Finyear)
        {
            try
            {
                DataSet dsInst = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_PurchaseOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
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
                    cmd.Parameters.AddWithValue("@PurchaseOrder_FinYear", Finyear);
                }
                

                cmd.Parameters.AddWithValue("@PurchaseOrder_Number", UniquePurchaseNumber);
                cmd.Parameters.AddWithValue("@PurchaseOrder_IndentIds", IndentRequisitionNo);
                if (!String.IsNullOrEmpty(IndentRequisitionDate))
                { cmd.Parameters.AddWithValue("@PurchaseOrder_IndentDate", IndentRequisitionDate); }
                
                cmd.Parameters.AddWithValue("@PurchaseOrder_Date", strPurchaseDate);
                cmd.Parameters.AddWithValue("@PurchaseOrder_VendorId", strVendor);
                cmd.Parameters.AddWithValue("@Contact_Person_Id", strContactName);
                cmd.Parameters.AddWithValue("@PurchaseOrder_Reference", Reference);
                cmd.Parameters.AddWithValue("@PurchaseOrder_Currency_Id", BaseCurrencyId);
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

                if (Session["POwarehousedetailstemp"] != null)
                {
                    DataTable temtable = new DataTable();
                    DataTable Warehousedtssss = (DataTable)Session["POwarehousedetailstemp"];

                    temtable = Warehousedtssss.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "productid", "Inventrytype", "StockID", "isnew");
                    cmd.Parameters.AddWithValue("@udt_StockOpeningwarehousentrie", temtable);
                }

                SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(output);


                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                //Udf Add mode
                DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                if (udfTable != null)
                {
                    Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("PO", "PurchaseOrder" + Convert.ToString(output.Value), udfTable, Convert.ToString(Session["userid"]));
                }

                if (!string.IsNullOrEmpty(hfControlData.Value))
                {
                    CommonBL objCommonBL = new CommonBL();
                    objCommonBL.InsertTransporterControlDetails(Convert.ToInt32(output.Value), "PO".ToUpper(), hfControlData.Value, Convert.ToString(HttpContext.Current.Session["userid"]));
                }

                #region warehouse Update and delete

                updatewarehouse();
                deleteALL();

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {

            //oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
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

                    sqlQuery = "SELECT max(tjv.PurchaseOrder_Number) FROM tbl_trans_PurchaseOrder tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.PurchaseOrder_Number))) = 1";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.PurchaseOrder_Number) FROM tbl_trans_PurchaseOrder tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.PurchaseOrder_Number))) = 1";
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
                    sqlQuery = "SELECT PurchaseOrder_Number FROM tbl_trans_PurchaseOrder WHERE PurchaseOrder_Number LIKE '" + manual_str.Trim() + "'";
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
        #endregion

        #region warehousepopup
        protected void GrdWarehousePC_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];


            if (hdniswarehouse.Value == "true")
            {
                GrdWarehousePC.Columns["viewWarehouseName"].Visible = true;
                GrdWarehousePC.Columns["Quantity"].Visible = true;
                GrdWarehousePC.Columns["viewQuantity"].Visible = false;

            }
            else
            {
                GrdWarehousePC.Columns["viewWarehouseName"].Visible = false;
                GrdWarehousePC.Columns["Quantity"].Visible = false;
                GrdWarehousePC.Columns["viewQuantity"].Visible = true;
            }
            if (hdnisbatch.Value == "true")
            {
                GrdWarehousePC.Columns["viewBatchNo"].Visible = true;
                GrdWarehousePC.Columns["viewMFGDate"].Visible = true;
                GrdWarehousePC.Columns["viewExpiryDate"].Visible = true;
                GrdWarehousePC.Columns["Quantity"].Visible = false;
                GrdWarehousePC.Columns["viewQuantity"].Visible = true;

            }
            else
            {
                GrdWarehousePC.Columns["viewBatchNo"].Visible = false;
                GrdWarehousePC.Columns["viewMFGDate"].Visible = false;
                GrdWarehousePC.Columns["viewExpiryDate"].Visible = false;
                GrdWarehousePC.Columns["Quantity"].Visible = true;
                GrdWarehousePC.Columns["viewQuantity"].Visible = false;
            }
            if (hdnisserial.Value == "true")
            {
                GrdWarehousePC.Columns["viewSerialNo"].Visible = true;
                GrdWarehousePC.Columns["Quantity"].Visible = false;
                GrdWarehousePC.Columns["viewQuantity"].Visible = true;
            }
            else
            {
                GrdWarehousePC.Columns["viewSerialNo"].Visible = false;
            }


            #region Savenew
            if (strSplitCommand == "Display")
            {
                string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);

                string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]).Trim();

                string BatchName = Convert.ToString(e.Parameters.Split('~')[3]);

                string SerialName = Convert.ToString(e.Parameters.Split('~')[4]);
                string ProductID = Convert.ToString(hdfProductIDPC.Value);
                string stockid = Convert.ToString(hdfstockidPC.Value);
                decimal openingstock = Convert.ToDecimal(txtqnty.Text);
                string branchid = Convert.ToString(hdbranchIDPC.Value);
                string qnty = Convert.ToString(e.Parameters.Split('~')[5]);

                decimal totalopeining = Convert.ToDecimal(hdfopeningstockPC.Value);
                decimal oldtotalopeining = Convert.ToDecimal(oldopeningqntity.Value);

                if (qnty == "0.0000" && openingstock <= 0)
                {
                    qnty = batchqnty.Text;
                    openingstock = Convert.ToDecimal(qnty);
                }

                if (!string.IsNullOrEmpty(BatchName))
                {
                    BatchName = BatchName.Trim();
                }
                if (!string.IsNullOrEmpty(SerialName))
                {
                    SerialName = SerialName.Trim();
                }
                if (WarehouseID == "null")
                {
                    WarehouseID = "0";
                }

                bool isserialunique = false;
                if (hdnisserial.Value == "true")
                {
                    isserialunique = CheckUniqueSerial(SerialName, "new");
                }
                if (Convert.ToDecimal(openingstock) > totalopeining)
                {
                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");

                }
                else if (isserialunique == false)
                {
                    DataTable Warehousedt = new DataTable();
                    DataTable Warehousedtnew = new DataTable();
                    if (Session["POCWarehouseData"] != null)
                    {
                        Warehousedt = (DataTable)Session["POCWarehouseData"];
                    }
                    else
                    {
                        Warehousedt.Columns.Add("SrlNo", typeof(Int32));
                        Warehousedt.Columns.Add("WarehouseID", typeof(string));
                        Warehousedt.Columns.Add("WarehouseName", typeof(string));

                        Warehousedt.Columns.Add("BatchNo", typeof(string));
                        Warehousedt.Columns.Add("SerialNo", typeof(string));

                        Warehousedt.Columns.Add("MFGDate", typeof(DateTime));
                        Warehousedt.Columns.Add("ExpiryDate", typeof(DateTime));
                        Warehousedt.Columns.Add("Quantity", typeof(decimal));

                        Warehousedt.Columns.Add("BatchWarehouseID", typeof(string));
                        Warehousedt.Columns.Add("BatchWarehousedetailsID", typeof(string));
                        Warehousedt.Columns.Add("BatchID", typeof(string));
                        Warehousedt.Columns.Add("SerialID", typeof(string));


                        Warehousedt.Columns.Add("viewWarehouseName", typeof(string));

                        Warehousedt.Columns.Add("viewBatchNo", typeof(string));
                        Warehousedt.Columns.Add("viewQuantity", typeof(string));
                        Warehousedt.Columns.Add("viewSerialNo", typeof(string));



                        Warehousedt.Columns.Add("viewMFGDate", typeof(DateTime));
                        Warehousedt.Columns.Add("viewExpiryDate", typeof(DateTime));

                        Warehousedt.Columns.Add("Quantitysum", typeof(decimal));
                        Warehousedt.Columns.Add("isnew", typeof(string));
                        Warehousedt.Columns.Add("productid", typeof(string));
                        Warehousedt.Columns.Add("Inventrytype", typeof(string));
                        Warehousedt.Columns.Add("StockID", typeof(string));
                        Warehousedt.Columns.Add("pcslno", typeof(Int32));

                    }



                    DateTime dtmgh = txtmkgdate.Date;
                    DateTime dtexp = txtexpirdate.Date;

                    string isedited = hdnisedited.Value;
                    decimal inputqnty = 0;
                    int noofserial = 0;
                    int oldrowcount = Convert.ToInt32(hdnoldrowcount.Value);
                    int newrowcount = 0;
                    decimal hdntotalqntyPCs = Convert.ToDecimal(hdntotalqntyPC.Value);


                    if (Session["POCWarehouseData"] != null)
                    {
                        DataTable Warehousedts = (DataTable)Session["POCWarehouseData"];

                        newrowcount = Warehousedts.Select("isnew = 'new'").Count<DataRow>();

                        if (newrowcount != null && hdnisserial.Value == "true")
                        {

                            var inputqntys = Warehousedts.Select("WarehouseName= '" + WarehouseName + "' AND BatchNo = '" + BatchName + "' AND isnew = 'new'").Count<DataRow>();
                            if (inputqntys != null && !string.IsNullOrEmpty(Convert.ToString(inputqntys)))
                            {
                                noofserial = Convert.ToInt32(inputqntys + 1);

                            }
                            else
                            {
                                noofserial = 0;
                            }
                        }
                    }

                    if (hidencountforserial.Value != "2")
                    {
                        if (Warehousedt.Rows.Count > 0)
                        {
                            var inputqntys = Warehousedt.Compute("sum(Quantitysum)", "isnew = 'new'");
                            if (inputqntys != null && !string.IsNullOrEmpty(Convert.ToString(inputqntys)))
                            {
                                inputqnty = Convert.ToDecimal(inputqntys);

                            }
                            else
                            {
                                inputqnty = 0;
                            }

                        }
                        //commentout below line for it should not add.
                        if (hdnisserial.Value == "false" && hdniswarehouse.Value == "true" && hdnisbatch.Value == "false")
                        {
                            inputqnty = inputqnty + Convert.ToDecimal(openingstock);
                        }
                        if (hdnisserial.Value == "false" && hdniswarehouse.Value == "true" && hdnisbatch.Value == "true")
                        {
                            inputqnty = inputqnty + Convert.ToDecimal(openingstock);
                        }
                        if (hdnisserial.Value == "false" && hdniswarehouse.Value == "false" && hdnisbatch.Value == "true")
                        {
                            inputqnty = inputqnty + Convert.ToDecimal(openingstock);
                        }


                    }

                    //checking quantity only for warehouse is true.
                    if (inputqnty == 0 && hdnisserial.Value == "false")
                    {
                        inputqnty = Convert.ToDecimal(openingstock);
                    }

                    if (hidencountforserial.Value == "1" && hdnisserial.Value == "true" && noofserial != openingstock && hdnisbatch.Value == "true" && hdniswarehouse.Value == "true")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetdisblebatch"] = Convert.ToString("disable");
                    }
                    if (hidencountforserial.Value == "2" && hdnisserial.Value == "true" && noofserial == openingstock && hdnisbatch.Value == "true" && hdniswarehouse.Value == "true")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetenablebatch"] = Convert.ToString("enable");
                    }
                    if (hidencountforserial.Value == "1" && hdnisserial.Value == "true" && noofserial != openingstock && hdnisbatch.Value == "false" && hdniswarehouse.Value == "true")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetdisblebatch"] = Convert.ToString("disable");
                    }
                    if (hidencountforserial.Value == "2" && hdnisserial.Value == "true" && noofserial == openingstock && hdnisbatch.Value == "false" && hdniswarehouse.Value == "true")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetenablebatch"] = Convert.ToString("enable");
                    }

                    ///batch and serial only
                    if (hidencountforserial.Value == "1" && hdnisserial.Value == "true" && noofserial != openingstock && hdnisbatch.Value == "true" && hdniswarehouse.Value == "false")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetdisblebatch"] = Convert.ToString("disable");
                    }

                    if (hidencountforserial.Value == "2" && hdnisserial.Value == "true" && noofserial == openingstock && hdnisbatch.Value == "true" && hdniswarehouse.Value == "false")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetenablebatch"] = Convert.ToString("enable");
                    }

                    ///end batch and serial only



                    //

                    //if (hdnisserial.Value == "true" && ((Convert.ToDecimal(Convert.ToString((oldrowcount - newrowcount)).Replace('-', '0')) == hdntotalqntyPCs) || (Convert.ToDecimal(Convert.ToString((oldrowcount - newrowcount)).Replace('-', '0')) == (hdntotalqntyPCs * Convert.ToDecimal(hdnbatchchanged.Value)))) && ((hdniswarehouse.Value == "true" && hdnisbatch.Value == "true") && (hdnoldwarehousname.Value == WarehouseName && hdnoldbatchno.Value == BatchName)) || (hdniswarehouse.Value == "true" && hdnisbatch.Value == "false") && (hdnoldwarehousname.Value == WarehouseName) || (hdniswarehouse.Value == "false" && hdnisbatch.Value == "true") && (hdnoldbatchno.Value == BatchName))
                    //{
                    //    GrdWarehousePC.JSProperties["cpupdatemssgserial"] = Convert.ToString("Please make sure quantity and no of Serial are equal or not.");
                    //}
                    if (hdnisserial.Value == "true" && noofserial > openingstock)
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserial"] = Convert.ToString("Please make sure quantity and no of Serial are equal or not.");
                    }
                    else
                    {

                        if (inputqnty <= totalopeining || isedited == "true")
                        {

                            if (hdniswarehouse.Value == "true" && hdnisbatch.Value == "true" && hidencountforserial.Value == "2" && hdnisserial.Value == "true" && hdnoldwarehousname.Value == WarehouseName && hdnoldbatchno.Value == BatchName)
                            {
                                if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
                                {

                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                                    GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                                }
                                else
                                {
                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));

                                    GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                                }
                            }
                            else if (hdniswarehouse.Value == "false" && hdnisbatch.Value == "false" && hdnisserial.Value == "true")
                            {

                                if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
                                {

                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", WarehouseName, BatchName, openingstock, SerialName, dtmgh, dtexp, openingstock, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                                    GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");
                                }
                                else
                                {
                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", WarehouseName, BatchName, openingstock, SerialName, null, null, openingstock, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                                    GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                                }

                            }
                            else if (hdniswarehouse.Value == "true" && hidencountforserial.Value == "2" && hdnisbatch.Value == "false" && hdnisserial.Value == "true")
                            {
                                if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
                                {

                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                                    GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                                }
                                else
                                {
                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));

                                    GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                                }
                            }
                            //batch only with serial

                            else if (hdniswarehouse.Value == "false" && hidencountforserial.Value == "2" && hdnisbatch.Value == "true" && hdnisserial.Value == "true")
                            {
                                if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
                                {

                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                                    GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                                }
                                else
                                {
                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));

                                    GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                                }
                            }
                            //batch only.
                            else
                            {
                                if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
                                {

                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", WarehouseName, BatchName, openingstock, SerialName, dtmgh, dtexp, openingstock, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));

                                }
                                else
                                {
                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", WarehouseName, BatchName, openingstock, SerialName, null, null, openingstock, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));


                                }
                                if (hdnisserial.Value == "true")
                                {
                                    GrdWarehousePC.JSProperties["cpinsertmssg"] = Convert.ToString("Inserted.");
                                }
                                if (hdnisserial.Value == "false" && hdniswarehouse.Value == "true" && hdnisbatch.Value == "true")
                                {
                                    GrdWarehousePC.JSProperties["cpbatchinsertmssg"] = Convert.ToString("Inserted.");
                                }

                            }


                            Session["POCWarehouseData"] = Warehousedt;


                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                            GrdWarehousePC.DataBind();
                        }
                        else
                        {
                            GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");
                        }
                    }
                }
                //
            }
            #endregion
            #region SaveAll
            if (strSplitCommand == "Saveall")
            {
                string openingstock = Convert.ToString(hdfopeningstockPC.Value);

                if (Session["POCWarehouseData"] == null && hdnisolddeleted.Value == "false")
                {
                    if (!string.IsNullOrEmpty(hdnvalue.Value) && !string.IsNullOrEmpty(hdnrate.Value))
                    {

                    }
                    else
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Nothing to Saved .");
                    }

                }
                else if (Session["POCWarehouseData"] == null && openingstock != "0.0")
                {

                    //deleteALL();

                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("You can not save blank data.");
                }
                else
                {
                    string ProductID = Convert.ToString(hdfProductIDPC.Value);
                    string stockid = Convert.ToString(hdfstockidPC.Value);
                     openingstock = Convert.ToString(hdfopeningstockPC.Value);
                    string branchid = Convert.ToString(hdbranchIDPC.Value);
                    string isolddeletd = hdnisolddeleted.Value;

                    int oldrowcount = Convert.ToInt32(hdnoldrowcount.Value);
                    int newrowcount = 0;
                    int updaterows = 0;
                    int deleted = 0;
                    decimal hdntotalqntyPCs = Convert.ToDecimal(hdntotalqntyPC.Value);

                    DataTable Warehousedts = (DataTable)Session["POCWarehouseData"];

                    newrowcount = Warehousedts.Select("isnew = 'new'").Count<DataRow>();
                    updaterows = Warehousedts.Select("isnew = 'Updated'").Count<DataRow>();


                    if (newrowcount != 0 || updaterows != 0 || Session["POWarehouseDataDelete"] != null)
                    {
                        decimal inputqnty = 0;
                        decimal totalopeining = Convert.ToDecimal(hdfopeningstockPC.Value);
                        decimal oldtotalopeining = Convert.ToDecimal(oldopeningqntity.Value);

                        var inputqntys = Warehousedts.Compute("sum(Quantitysum)", "isnew = 'new'");
                        var updateqnty = Warehousedts.Compute("sum(Quantitysum)", "isnew = 'Updated'");
                        var oldeqnty = Warehousedts.Compute("sum(Quantitysum)", "isnew = 'old'");
                        decimal deletd = Convert.ToDecimal(hdndeleteqnity.Value);

                        if (inputqntys != null && !string.IsNullOrEmpty(Convert.ToString(inputqntys)))
                        {
                            inputqnty = Convert.ToDecimal(inputqntys);

                        }
                        if (updateqnty != null && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                        {
                            inputqnty = inputqnty + Convert.ToDecimal(updateqnty);


                        }
                        if (oldeqnty != null && !string.IsNullOrEmpty(Convert.ToString(oldeqnty)))
                        {
                            inputqnty = inputqnty + Convert.ToDecimal(oldeqnty);


                        }
                        //commnet for allowing reduce from main page
                        //if ((oldeqnty != null && !string.IsNullOrEmpty(Convert.ToString(oldeqnty))) || (updateqnty != null && !string.IsNullOrEmpty(Convert.ToString(updateqnty))))
                        //{

                        //    //totalopeining = totalopeining + oldtotalopeining; //commnet for allowing reduce from main page
                        //    //if (hdnisreduing.Value == "true")
                        //    //{
                        //    //    totalopeining = oldtotalopeining;
                        //    //}
                        //    //else
                        //    //{
                        //    //    totalopeining = totalopeining + oldtotalopeining;
                        //    //}
                        //    totalopeining = oldtotalopeining;
                        //}
                        //if (deletd > 0 & isolddeletd == "true")
                        //{
                        //    totalopeining = Convert.ToDecimal(oldopeningqntity.Value) - deletd;
                        //}

                        if (inputqnty == totalopeining)
                        {
                            //if (hdnisserial.Value == "true" && ((Convert.ToDecimal(Convert.ToString((oldrowcount - newrowcount)).Replace('-', '0')) == hdntotalqntyPCs) || (Convert.ToDecimal(Convert.ToString((oldrowcount - newrowcount)).Replace('-', '0')) == (hdntotalqntyPCs * Convert.ToDecimal(hdnbatchchanged.Value)))))
                            if (hdnisserial.Value == "true" && hdnisbatch.Value == "true" && hdniswarehouse.Value == "true")
                            {

                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["POCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["POCWarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }
                            }
                            else if (hdnisserial.Value != "true" && hdnisbatch.Value != "true" && hdniswarehouse.Value == "true")
                            {
                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["POCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["POCWarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }
                            }
                            else if (hdnisserial.Value != "true" && hdnisbatch.Value == "true")
                            {
                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["POCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["POCWarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }

                            }
                            else if (hdnisserial.Value == "true" && hdnisbatch.Value != "true" && hdniswarehouse.Value != "true")
                            {
                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["POCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["POCWarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }
                            }
                            else if (hdnisserial.Value == "true" && hdnisbatch.Value != "true" && hdniswarehouse.Value == "true")
                            {

                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["POCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["POCWarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }
                            }
                            else if (hdnisserial.Value == "true" && hdnisbatch.Value == "true" && hdniswarehouse.Value != "true")
                            {

                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["POCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["POCWarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }
                            }
                            else
                            {
                                GrdWarehousePC.JSProperties["cpupdatemssgserial"] = Convert.ToString("Please make sure quantity and no of Serial are equal or not.");

                            }

                        }
                        else
                        {
                            GrdWarehousePC.JSProperties["cpupdatemssgserial"] = Convert.ToString("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");
                        }

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(hdnvalue.Value) && !string.IsNullOrEmpty(hdnrate.Value))
                        {
                            int olfdatterows = Warehousedts.Select("isnew = 'old'").Count<DataRow>();
                            if (olfdatterows != 0)
                            {
                                var oldeqnty = Warehousedts.Compute("sum(Quantitysum)", "isnew = 'old'");
                                if (Convert.ToDecimal(hdfopeningstockPC.Value) < Convert.ToDecimal(oldeqnty))
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Opening quantity and enterd quantity are mismatch.");
                                }
                                else
                                {

                                }

                            }
                            else
                            {

                            }

                        }
                        else
                        {
                            GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Nothing to Saved .");
                        }

                    }
                }

            }

            #endregion
            #region CheckDataExist
            if (strSplitCommand == "checkdataexist")
            {
                Session["POCWarehouseData"] = null;

                GrdWarehousePC.DataSource = null;

                string strProductname = Convert.ToString(e.Parameters.Split('~')[4]);
                string name = strProductname;
                name = name.Replace("squot", "'");
                name = name.Replace("coma", ",");
                name = name.Replace("slash", "/");

                Session["WarehouseUpdatedData"] = null;
                Session["POWarehouseDataDelete"] = null;

                GrdWarehousePC.JSProperties["cpproductname"] = Convert.ToString(name);


                string strProductID = Convert.ToString(e.Parameters.Split('~')[1]);

                string stockids = Convert.ToString(e.Parameters.Split('~')[2]);

                string branchid = Convert.ToString(e.Parameters.Split('~')[3]);

                DataTable Warehousedt = new DataTable();
                Warehousedt = GetRecord(stockids);
                if (Warehousedt.Rows.Count > 0)
                {
                    Session["POCWarehouseData"] = Warehousedt;

                    GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                    GrdWarehousePC.DataBind();
                }
                else
                {

                    Session["POCWarehouseData"] = null;

                    GrdWarehousePC.DataSource = null;
                    GrdWarehousePC.DataBind();

                }


            }
            //if (strSplitCommand == "UpdatedataReffresh")
            //{
            //    if (Session["POCWarehouseData"] != null)
            //    {

            //        GrdWarehousePC.DataSource = Session["POCWarehouseData"];
            //        GrdWarehousePC.DataBind();
            //    }
            //}
            #endregion
            #region updatedata
            if (strSplitCommand == "Updatedata")
            {
                string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]);

                string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);

                string BatchName = Convert.ToString(e.Parameters.Split('~')[3]);

                string SerialName = Convert.ToString(e.Parameters.Split('~')[4]);

                string slno = Convert.ToString(e.Parameters.Split('~')[5]);
                string qntity = Convert.ToString(e.Parameters.Split('~')[6]);
                string isviewqntitynull = hdnisviewqntityhas.Value;
                string isserialenable = hdnisserial.Value;
                DateTime expdate = txtexpirdate.Date;
                DateTime mkgdate = txtmkgdate.Date;
                DataTable Warehousedt = new DataTable();

                decimal openingstock = Convert.ToDecimal(txtqnty.Text);
                if (qntity == "0.0000" && openingstock <= 0)
                {
                    qntity = batchqnty.Text;
                    openingstock = Convert.ToDecimal(qntity);
                }

                if (WarehouseID == "null")
                {
                    WarehouseID = "0";
                    WarehouseName = "";
                }

                bool isserialunique = false;
                if (hdnisserial.Value == "true")
                {
                    isserialunique = CheckUniqueSerial(SerialName, slno);
                }

                if (Session["POCWarehouseData"] != null && isserialunique == false)
                {
                    Warehousedt = (DataTable)Session["POCWarehouseData"];
                    DataSet dataSet1 = new DataSet();
                    DataTable dt1 = new DataTable();
                    dt1 = Warehousedt.Copy();
                    dataSet1.Tables.Add(dt1);

                    if (dataSet1.Tables[0].Rows.Count > 0)
                    {
                        DataRow[] customerRow = dataSet1.Tables[0].Select("SrlNo ='" + slno + "'");
                        if (isserialenable == "false" && isviewqntitynull != "false")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;

                            if (Convert.ToString(expdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(mkgdate) != "1/1/0001 12:00:00 AM")
                            {
                                customerRow[0]["viewMFGDate"] = mkgdate;
                                customerRow[0]["viewExpiryDate"] = expdate;
                            }

                            customerRow[0]["viewWarehouseName"] = WarehouseName;
                            customerRow[0]["viewBatchNo"] = BatchName;
                            customerRow[0]["viewSerialNo"] = SerialName;
                            customerRow[0]["viewQuantity"] = qntity;
                            customerRow[0]["Quantitysum"] = openingstock;
                            customerRow[0]["isnew"] = "Updated";
                        }
                        if (isserialenable == "true" && isviewqntitynull == "false")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;

                            if (Convert.ToString(expdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(mkgdate) != "1/1/0001 12:00:00 AM")
                            {
                                customerRow[0]["viewMFGDate"] = mkgdate;
                                customerRow[0]["viewExpiryDate"] = expdate;
                            }

                            customerRow[0]["viewWarehouseName"] = WarehouseName;
                            customerRow[0]["viewBatchNo"] = BatchName;
                            customerRow[0]["viewSerialNo"] = SerialName;
                            customerRow[0]["viewQuantity"] = openingstock;
                            customerRow[0]["Quantitysum"] = openingstock;
                            customerRow[0]["isnew"] = "Updated";
                        }
                        if (isserialenable == "true" && isviewqntitynull == "true")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;



                            customerRow[0]["viewSerialNo"] = SerialName;

                            customerRow[0]["Quantitysum"] = 0;
                            customerRow[0]["isnew"] = "Updated";
                        }
                        if (isserialenable == "false" && isviewqntitynull == "false")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;

                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;
                            if (Convert.ToString(expdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(mkgdate) != "1/1/0001 12:00:00 AM")
                            {
                                customerRow[0]["viewMFGDate"] = mkgdate;
                                customerRow[0]["viewExpiryDate"] = expdate;
                            }
                            customerRow[0]["viewWarehouseName"] = WarehouseName;
                            customerRow[0]["viewBatchNo"] = BatchName;
                            customerRow[0]["viewSerialNo"] = SerialName;
                            customerRow[0]["viewQuantity"] = openingstock;
                            customerRow[0]["Quantitysum"] = openingstock;
                            customerRow[0]["isnew"] = "Updated";
                        }

                        GrdWarehousePC.JSProperties["cpupdateexistingdata"] = Convert.ToString("Saved.");
                    }
                    Warehousedt = null;
                    Warehousedt = dataSet1.Tables[0];
                    Session["POCWarehouseData"] = Warehousedt;

                    Session["WarehouseUpdatedData"] = Warehousedt;

                    GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                    GrdWarehousePC.DataBind();

                }
            }


            #endregion
            #region newrowupdate
            if (strSplitCommand == "NewUpdatedata")
            {
                string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]);

                string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);

                string BatchName = Convert.ToString(e.Parameters.Split('~')[3]);

                string SerialName = Convert.ToString(e.Parameters.Split('~')[4]);

                string slno = Convert.ToString(e.Parameters.Split('~')[5]);
                string qntity = Convert.ToString(e.Parameters.Split('~')[6]);
                string isviewqntitynull = hdnisviewqntityhas.Value;
                string isserialenable = hdnisserial.Value;
                DataTable Warehousedt = new DataTable();
                decimal openingstock = Convert.ToDecimal(txtqnty.Text);
                if (qntity == "0.0000" && openingstock <= 0)
                {
                    qntity = batchqnty.Text;
                    openingstock = Convert.ToDecimal(qntity);
                }
                bool isserialunique = false;
                if (hdnisserial.Value == "true")
                {
                    isserialunique = CheckUniqueSerial(SerialName, slno);
                }


                if (Session["POCWarehouseData"] != null && isserialunique == false)
                {
                    Warehousedt = (DataTable)Session["POCWarehouseData"];
                    DataSet dataSet1 = new DataSet();
                    DataTable dt1 = new DataTable();
                    dt1 = Warehousedt.Copy();
                    dataSet1.Tables.Add(dt1);

                    if (dataSet1.Tables[0].Rows.Count > 0)
                    {
                        DataRow[] customerRow = dataSet1.Tables[0].Select("SrlNo ='" + slno + "'");
                        if (isserialenable == "false" && isviewqntitynull != "false")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;

                            customerRow[0]["viewWarehouseName"] = WarehouseName;
                            customerRow[0]["viewBatchNo"] = BatchName;
                            customerRow[0]["viewSerialNo"] = SerialName;
                            customerRow[0]["viewQuantity"] = openingstock;
                            customerRow[0]["Quantitysum"] = openingstock;
                            customerRow[0]["isnew"] = "new";
                        }
                        if (isserialenable == "true" && isviewqntitynull == "false")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;

                            customerRow[0]["viewWarehouseName"] = WarehouseName;
                            customerRow[0]["viewBatchNo"] = BatchName;
                            customerRow[0]["viewSerialNo"] = SerialName;
                            customerRow[0]["viewQuantity"] = openingstock;
                            customerRow[0]["Quantitysum"] = openingstock;
                            customerRow[0]["isnew"] = "new";
                        }
                        if (isserialenable == "true" && isviewqntitynull == "true")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;



                            customerRow[0]["viewSerialNo"] = SerialName;

                            customerRow[0]["Quantitysum"] = 0;
                            customerRow[0]["isnew"] = "new";
                        }
                        if (isserialenable == "false" && isviewqntitynull == "false")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;

                            customerRow[0]["viewWarehouseName"] = WarehouseName;
                            customerRow[0]["viewBatchNo"] = BatchName;
                            customerRow[0]["viewSerialNo"] = SerialName;
                            customerRow[0]["viewQuantity"] = openingstock;
                            customerRow[0]["Quantitysum"] = openingstock;
                            customerRow[0]["isnew"] = "new";
                        }

                        GrdWarehousePC.JSProperties["cpupdatenewdata"] = Convert.ToString("Saved.");
                    }
                    Warehousedt = null;
                    Warehousedt = dataSet1.Tables[0];
                    Session["POCWarehouseData"] = Warehousedt;

                    GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                    GrdWarehousePC.DataBind();

                }


            }
            #endregion
            #region Delete
            if (strSplitCommand == "Delete")
            {
                string slno = Convert.ToString(e.Parameters.Split('~')[1]);

                string viewQuantity = Convert.ToString(e.Parameters.Split('~')[2]);

                string Quantity = Convert.ToString(e.Parameters.Split('~')[3]);
                string warehouseid = Convert.ToString(e.Parameters.Split('~')[4]);
                string batch = Convert.ToString(e.Parameters.Split('~')[5]);
                DataTable Warehousedt = new DataTable();

                // string isviewqntitynull = hdnisviewqntityhas.Value;
                string isserialenable = hdnisserial.Value;
                string isbatch = hdnisbatch.Value;
                string iswarehouse = hdniswarehouse.Value;
                string isolddeletd = hdnisolddeleted.Value;
                if (Session["POCWarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["POCWarehouseData"];
                    DataSet dataSet1 = new DataSet();

                    DataTable dt1 = new DataTable();
                    dt1 = Warehousedt.Copy();



                    dataSet1.Tables.Add(dt1);

                    if (dataSet1.Tables[0].Rows.Count > 0)
                    {

                        if (isserialenable == "true" && (viewQuantity == "" || viewQuantity == "0") && isbatch == "true" && iswarehouse == "true")
                        {
                            DataRow[] customerRows = dataSet1.Tables[0].Select("WarehouseID= '" + warehouseid + "' AND BatchNo='" + batch + "'");

                            for (int i = 0; i <= customerRows.Count() - 1; i++)
                            {
                                if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["isnew"] = "DeleteSL";
                                }
                                else if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && !string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {

                                    customerRows[i]["isnew"] = "DeleteWHBTSL";

                                }

                                if (!string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["viewQuantity"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;
                                    customerRows[i]["Quantity"] = Convert.ToDecimal(customerRows[i]["Quantity"]) - 1;
                                    customerRows[i]["Quantitysum"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;

                                    if (Convert.ToString(customerRows[i]["isnew"]) != "new")
                                    {
                                        customerRows[i]["isnew"] = "Updated";
                                    }
                                    else
                                    {
                                        customerRows[i]["isnew"] = "new";
                                    }

                                }
                                //else
                                //{
                                //    customerRows[i]["Quantity"] = 0;
                                //    customerRows[i]["Quantitysum"] = 0;
                                //}
                            }


                            GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(1);

                        }
                        else if (isserialenable == "true" && (viewQuantity == "" || viewQuantity == "0") && isbatch != "true" && iswarehouse == "true")
                        {
                            DataRow[] customerRows = dataSet1.Tables[0].Select("WarehouseID= '" + warehouseid + "'");

                            for (int i = 0; i <= customerRows.Count() - 1; i++)
                            {
                                if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["isnew"] = "DeleteSL";
                                }
                                else if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && !string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {

                                    customerRows[i]["isnew"] = "DeleteWHSL";


                                }

                                if (!string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["viewQuantity"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;
                                    customerRows[i]["Quantity"] = Convert.ToDecimal(customerRows[i]["Quantity"]) - 1;
                                    customerRows[i]["Quantitysum"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;

                                    if (Convert.ToString(customerRows[i]["isnew"]) != "new")
                                    {
                                        customerRows[i]["isnew"] = "Updated";
                                    }
                                    else
                                    {
                                        customerRows[i]["isnew"] = "new";
                                    }

                                }
                                else
                                {
                                    customerRows[i]["Quantity"] = 0;
                                    customerRows[i]["Quantitysum"] = 0;
                                }
                            }


                            GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(1);

                        }

                        else if (isserialenable == "true" && (viewQuantity == "" || viewQuantity == "0") && isbatch == "true" && iswarehouse != "true")
                        {
                            DataRow[] customerRows = dataSet1.Tables[0].Select("BatchNo= '" + batch + "'");

                            for (int i = 0; i <= customerRows.Count() - 1; i++)
                            {
                                if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["isnew"] = "DeleteSL";
                                }
                                else if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && !string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {

                                    customerRows[i]["isnew"] = "DeleteWHBTSL";

                                    //customerRows[i]["isnew"] = "DeleteWHBTSL";
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["viewQuantity"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;
                                    customerRows[i]["Quantity"] = Convert.ToDecimal(customerRows[i]["Quantity"]) - 1;
                                    customerRows[i]["Quantitysum"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;

                                    if (Convert.ToString(customerRows[i]["isnew"]) != "new")
                                    {
                                        customerRows[i]["isnew"] = "Updated";
                                    }
                                    else
                                    {
                                        customerRows[i]["isnew"] = "new";
                                    }

                                }
                                else
                                {

                                    customerRows[i]["Quantity"] = 0;
                                    customerRows[i]["Quantitysum"] = 0;

                                }

                            }

                            GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(1);


                        }
                        else
                        {

                            DataRow[] customerRow = dataSet1.Tables[0].Select("SrlNo ='" + slno + "'");
                            if (isserialenable != "true" && isbatch == "true" && iswarehouse == "true")
                            {
                                customerRow[0]["isnew"] = "DeleteWHBT";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(viewQuantity);
                            }
                            if (isserialenable != "true" && isbatch != "true" && iswarehouse == "true")
                            {
                                customerRow[0]["isnew"] = "DeleteWH";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(Quantity);
                            }
                            if (isserialenable != "true" && isbatch == "true" && iswarehouse != "true")
                            {
                                customerRow[0]["isnew"] = "DeleteWHBT";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(viewQuantity);
                            }
                            if (isserialenable == "true" && isbatch == "true" && iswarehouse == "true")
                            {
                                customerRow[0]["isnew"] = "DeleteWHBTSL";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(Quantity);
                            }
                            if (isserialenable == "true" && isbatch != "true" && iswarehouse == "true")
                            {
                                customerRow[0]["isnew"] = "DeleteWHBTSL";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(1);
                            }
                            if (isserialenable == "true" && isbatch == "true" && iswarehouse != "true")
                            {
                                customerRow[0]["isnew"] = "DeleteWHBTSL";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(1);
                            }
                        }

                    }

                    Warehousedt = null;
                    DataTable setdeletedate = new DataTable();
                    DataTable getdeletedate = dataSet1.Tables[0];

                    DataRow[] drs = getdeletedate.Select("isnew = 'DeleteWHBT' OR isnew = 'DeleteWH' OR isnew = 'DeleteWHBT' OR isnew = 'DeleteBTSL' OR isnew = 'DeleteSL' OR isnew = 'DeleteWHSL' OR isnew = 'DeleteWHBTSL'");
                    if (drs.Count() != 0)
                    {
                        if (Session["POWarehouseDataDelete"] != null)
                        {
                            setdeletedate = drs.CopyToDataTable();
                            DataTable dtmp = (DataTable)Session["POWarehouseDataDelete"];
                            dtmp.Merge(setdeletedate);
                            Session["POWarehouseDataDelete"] = dtmp;
                        }
                        else
                        {
                            setdeletedate = drs.CopyToDataTable();
                            Session["POWarehouseDataDelete"] = setdeletedate;
                        }


                    }


                    DataTable Warehousedts = dataSet1.Tables[0];
                    DataRow[] dr = Warehousedts.Select("isnew = 'new' OR isnew = 'Updated' OR isnew = 'old'");
                    if (dr.Count() > 0)
                    {

                        Warehousedt = dr.CopyToDataTable();
                        Warehousedt.DefaultView.Sort = "SrlNo asc";
                        Warehousedt = Warehousedt.DefaultView.ToTable(true);

                        Session["POCWarehouseData"] = Warehousedt;

                        GrdWarehousePC.DataSource = Session["POCWarehouseData"];
                        GrdWarehousePC.DataBind();
                    }
                    else
                    {
                        Session["POCWarehouseData"] = null;

                        //Warehousedt.Columns.Add("SrlNo", typeof(string));
                        //Warehousedt.Columns.Add("WarehouseID", typeof(string));
                        //Warehousedt.Columns.Add("WarehouseName", typeof(string));

                        //Warehousedt.Columns.Add("BatchNo", typeof(string));
                        //Warehousedt.Columns.Add("SerialNo", typeof(string));

                        //Warehousedt.Columns.Add("MFGDate", typeof(DateTime));
                        //Warehousedt.Columns.Add("ExpiryDate", typeof(DateTime));
                        //Warehousedt.Columns.Add("Quantity", typeof(decimal));

                        //Warehousedt.Columns.Add("BatchWarehouseID", typeof(string));
                        //Warehousedt.Columns.Add("BatchWarehousedetailsID", typeof(string));
                        //Warehousedt.Columns.Add("BatchID", typeof(string));
                        //Warehousedt.Columns.Add("SerialID", typeof(string));


                        //Warehousedt.Columns.Add("viewWarehouseName", typeof(string));

                        //Warehousedt.Columns.Add("viewBatchNo", typeof(string));
                        //Warehousedt.Columns.Add("viewQuantity", typeof(string));
                        //Warehousedt.Columns.Add("viewSerialNo", typeof(string));



                        //Warehousedt.Columns.Add("viewMFGDate", typeof(DateTime));
                        //Warehousedt.Columns.Add("viewExpiryDate", typeof(DateTime));

                        //Warehousedt.Columns.Add("Quantitysum", typeof(decimal));
                        //Warehousedt.Columns.Add("isnew", typeof(string));

                        GrdWarehousePC.DataSource = Warehousedt;
                        GrdWarehousePC.DataBind();



                    }


                }
            #endregion
            }


        }

        public bool CheckUniqueSerial(string serial, string slno)
        {
            bool ISexist = false;

            if (Session["POCWarehouseData"] != null && hdnisserial.Value == "true")
            {
                DataTable Warehousedts = (DataTable)Session["POCWarehouseData"];
                //
                DataRow[] dr = Warehousedts.Select(" productid = '" + Convert.ToString(hdfProductIDPC.Value) + "'and isnew = 'new' OR isnew = 'Updated' OR isnew = 'old'");

                if (dr.Count() > 0)
                {
                    DataTable dt = dr.CopyToDataTable();
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            if (slno == "new")
                            {
                                if (Convert.ToString(dt.Rows[i]["viewSerialNo"]).ToUpper() == serial.ToUpper())
                                {
                                    ISexist = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (Convert.ToString(dt.Rows[i]["viewSerialNo"]).ToUpper() == serial.ToUpper() && Convert.ToString(dt.Rows[i]["SrlNo"]) != slno)
                                {
                                    ISexist = true;
                                    break;
                                }
                            }


                        }

                    }
                }


            }
            return ISexist;

        }
        public DataTable GetRecord(string stockids)
        {
            Session["POCWarehouseData"] = null;
            DataTable Warehousedt = new DataTable();

            if (Session["POCWarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["POCWarehouseData"];
            }
            else
            {
                Warehousedt.Columns.Add("SrlNo", typeof(Int32));
                Warehousedt.Columns.Add("WarehouseID", typeof(string));
                Warehousedt.Columns.Add("WarehouseName", typeof(string));

                Warehousedt.Columns.Add("BatchNo", typeof(string));
                Warehousedt.Columns.Add("SerialNo", typeof(string));

                Warehousedt.Columns.Add("MFGDate", typeof(DateTime));
                Warehousedt.Columns.Add("ExpiryDate", typeof(DateTime));
                Warehousedt.Columns.Add("Quantity", typeof(decimal));

                Warehousedt.Columns.Add("BatchWarehouseID", typeof(string));
                Warehousedt.Columns.Add("BatchWarehousedetailsID", typeof(string));
                Warehousedt.Columns.Add("BatchID", typeof(string));
                Warehousedt.Columns.Add("SerialID", typeof(string));


                Warehousedt.Columns.Add("viewWarehouseName", typeof(string));

                Warehousedt.Columns.Add("viewBatchNo", typeof(string));
                Warehousedt.Columns.Add("viewQuantity", typeof(string));
                Warehousedt.Columns.Add("viewSerialNo", typeof(string));



                Warehousedt.Columns.Add("viewMFGDate", typeof(DateTime));
                Warehousedt.Columns.Add("viewExpiryDate", typeof(DateTime));

                Warehousedt.Columns.Add("Quantitysum", typeof(decimal));
                Warehousedt.Columns.Add("isnew", typeof(string));

                Warehousedt.Columns.Add("productid", typeof(string));
                Warehousedt.Columns.Add("Inventrytype", typeof(string));
                Warehousedt.Columns.Add("StockID", typeof(string));
                Warehousedt.Columns.Add("pcslno", typeof(Int32));
            }


            DataTable dts = GetStockWarehouseData(stockids);

            string oldwarehousename = string.Empty;
            string oldbatchname = string.Empty;
            string oldquantity = string.Empty;
            string isoldornew = string.Empty;
            string BatchWarehouseID = string.Empty;

            if (dts.Rows.Count > 0)
            {
                for (int i = 0; i <= dts.Rows.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["BatchWarehouseID"])) && Convert.ToString(dts.Rows[i]["BatchWarehouseID"]) != "0")
                    {
                        isoldornew = "old";
                    }
                    else
                    {
                        isoldornew = "new";
                    }

                    if (hdnisserial.Value == "true" && i != 0)
                    {
                        oldwarehousename = Convert.ToString(dts.Rows[i - 1]["warehouseID"]);
                        oldbatchname = Convert.ToString(dts.Rows[i - 1]["batchNO"]);
                        oldquantity = Convert.ToString(dts.Rows[i - 1]["Quantitysum"]);
                        BatchWarehouseID = Convert.ToString(dts.Rows[i - 1]["BatchWarehouseID"]);

                        if (oldwarehousename == Convert.ToString(dts.Rows[i]["warehouseID"]) && oldbatchname == Convert.ToString(dts.Rows[i]["batchNO"]) && (oldquantity == Convert.ToString(dts.Rows[i]["Quantitysum"]) || Convert.ToString(dts.Rows[i]["Quantitysum"]) == "0") && BatchWarehouseID == Convert.ToString(dts.Rows[i]["BatchWarehouseID"]))
                        {
                            if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                        }
                        //else if (oldwarehousename == Convert.ToString(dts.Rows[i]["warehouseID"]) && oldbatchname == Convert.ToString(dts.Rows[i]["batchNO"]))
                        //{
                        //    if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
                        //    {
                        //        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, "old");
                        //    }
                        //    else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                        //    {
                        //        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, "old");
                        //    }
                        //    else
                        //    {

                        //        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, "old");
                        //    }
                        //}
                        else
                        {
                            //string viewqunatity = (Convert.ToString(dts.Rows[i]["viewQuantity"])!=string.Empty)? Convert.ToString(dts.Rows[i]["viewQuantity"]):"0";
                            string viewqunatity = Convert.ToString(dts.Rows[i]["viewQuantity"]);
                            string sumquantiy = (Convert.ToString(dts.Rows[i]["Quantitysum"]) != string.Empty) ? Convert.ToString(dts.Rows[i]["Quantitysum"]) : "0";
                            if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(sumquantiy), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), viewqunatity, Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(sumquantiy), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(sumquantiy), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), viewqunatity, Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(sumquantiy), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), viewqunatity, Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToString(sumquantiy), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }

                        }

                    }
                    else if (hdnisserial.Value == "false" && hdniswarehouse.Value == "true" && hdnisbatch.Value == "true")
                    {
                        //string oldquantity=string.Empty;
                        if (i != 0)
                        {
                            oldwarehousename = Convert.ToString(dts.Rows[i - 1]["warehouseID"]);
                            oldquantity = Convert.ToString(dts.Rows[i - 1]["viewQuantity"]);
                        }



                        if (oldwarehousename == Convert.ToString(dts.Rows[i]["warehouseID"]))
                        {
                            if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["viewQuantity"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["viewQuantity"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                        }
                        else
                        {
                            if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                        }

                    }
                    else
                    {

                        if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
                        {
                            Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                        }
                        else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                        {
                            Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                        }
                        else
                        {

                            Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                        }

                    }


                    //Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, "", "", openingstock, "isnew");

                }
            }

            hdnoldrowcount.Value = Convert.ToString(Warehousedt.Rows.Count);

            return Warehousedt;
        }

        private int Insertupdatedata(string ProductID, string stockid, string branchid)
        {
            DataSet dsEmail = new DataSet();

            string IsserialActive = "false";

            int newrowcount = 0;

            #region Insert

            DataTable temtable = new DataTable();
            DataTable temtable2 = new DataTable();
            DataTable temtable23 = new DataTable();
            if (Session["POCWarehouseData"] != null)
            {
                DataTable Warehousedt = (DataTable)Session["POCWarehouseData"];

                temtable = Warehousedt.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "isnew", "viewQuantity", "productid", "Inventrytype", "StockID", "pcslno");

                if (Session["POwarehousedetailstemp"] != null)
                {
                    temtable23 = (DataTable)Session["POwarehousedetailstemp"];
                    DataRow[] rows;
                    rows = temtable23.Select("pcslno = '" + hdnpcslno.Value + "'");  //'UserName' is ColumnName
                    foreach (DataRow row in rows)
                        temtable23.Rows.Remove(row);

                    temtable23.Merge(temtable);
                    Session["POwarehousedetailstemp"] = temtable23;
                }
                else
                {
                    Session["POwarehousedetailstemp"] = temtable;
                }

            }


            #endregion
            #region Update
            if (Session["POCWarehouseData"] != null)
            {
                DataTable dt = new DataTable();
                DataTable Warehousedtups = (DataTable)Session["POCWarehouseData"];
                DataRow[] dr = Warehousedtups.Select("isnew = 'Updated'");
                if (dr.Count() != 0)
                {
                    dt = dr.CopyToDataTable();
                    if (Session["POwarehousedetailstempUpdate"] != null)
                    {
                        temtable23 = (DataTable)Session["POwarehousedetailstempUpdate"];
                        DataRow[] rows;
                        rows = temtable23.Select("pcslno = '" + hdnpcslno.Value + "'");  //'UserName' is ColumnName
                        foreach (DataRow row in rows)
                            temtable23.Rows.Remove(row);

                        temtable23.Merge(dt);
                        Session["POwarehousedetailstempUpdate"] = temtable23;
                    }
                    else
                    {
                        Session["POwarehousedetailstempUpdate"] = dt;
                    }


                }
            }

            #endregion
            #region delete

            if (Session["POWarehouseDataDelete"] != null)
            {
                DataTable dt = new DataTable();
                DataTable Warehousedtups = (DataTable)Session["POWarehouseDataDelete"];
                DataRow[] dr = Warehousedtups.Select("isnew = 'DeleteWHBT' OR isnew = 'DeleteWH' OR isnew = 'DeleteWHBT' OR isnew = 'DeleteBTSL' OR isnew = 'DeleteSL' OR isnew = 'DeleteWHSL' OR isnew = 'DeleteWHBTSL'");
                if (dr.Count() != 0)
                {
                    dt = dr.CopyToDataTable();
                    if (Session["POwarehousedetailstempDelete"] != null)
                    {
                        temtable23 = (DataTable)Session["POwarehousedetailstempDelete"];
                        DataRow[] rows;
                        rows = temtable23.Select("pcslno = '" + hdnpcslno.Value + "'");  //'UserName' is ColumnName
                        foreach (DataRow row in rows)
                            temtable23.Rows.Remove(row);

                        temtable23.Merge(dt);
                        Session["POwarehousedetailstempDelete"] = temtable23;
                    }
                    else
                    {
                        Session["POwarehousedetailstempDelete"] = dt;
                    }


                }

                //deleteALL(stockid);

            }
            #endregion

            return 1;


        }
        protected void GrdWarehousePC_DataBinding(object sender, EventArgs e)
        {
            if (Session["POCWarehouseData"] != null)
            {
                DataTable Warehousedt = (DataTable)Session["POCWarehouseData"];

                GrdWarehousePC.DataSource = Warehousedt;
            }
        }

        protected void CmbWarehouse_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];

            if (WhichCall == "BindWarehouse")
            {
                DataTable dt = GetWarehouseData();
                GetAvailableStock();

                CmbWarehouse.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CmbWarehouse.Items.Add(Convert.ToString(dt.Rows[i]["WarehouseName"]), Convert.ToString(dt.Rows[i]["WarehouseID"]));
                }
                if (hdndefaultID != null || hdndefaultID.Value != "")
                    CmbWarehouse.Value = hdndefaultID.Value;
            }

        }

        public DataTable GetWarehouseData()
        {
            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("select  b.bui_id as WarehouseID,b.bui_Name as WarehouseName from tbl_master_building b order by b.bui_Name");
            return dt;
        }

        public void GetAvailableStock()
        {
            //DataTable dt2 = oDBEngine.GetDataTable("select ISnull(ISNULL(tblwarehous.StockBranchWarehouse_StockIn,0)- isnull(tblwarehous.StockBranchWarehouse_StockOut,0),0) as branchopenstock from Trans_StockBranchWarehouse tblwarehous where tblwarehous.StockBranchWarehouse_StockId=" + hdfstockidPC.Value + " and tblwarehous.StockBranchWarehouse_CompanyId='" + Convert.ToString(Session["LastCompany"]) + "' and tblwarehous.StockBranchWarehouse_FinYear='" + Convert.ToString(Session["LastFinYear"]) + "' and tblwarehous.StockBranchWarehouse_BranchId=" + hdbranchIDPC.Value + "");
            try
            {
                DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockOpening(" + hdbranchIDPC.Value + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + hdfstockidPC.Value + ") as branchopenstock");

                if (dt2.Rows.Count > 0)
                {
                    CmbWarehouse.JSProperties["cpstock"] = Convert.ToString(dt2.Rows[0]["branchopenstock"]);
                }
                else
                {

                    CmbWarehouse.JSProperties["cpstock"] = "0.0000";
                }
            }
            catch (Exception ex)
            {

            }
        }
        public DataTable GetStockWarehouseData(string stockid)
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            if (Convert.ToString(ViewState["ActionType"]) == "Edit")
            {
                DataSet dsInst = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_POGetwarehousentry", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProductID", Convert.ToInt32(hdfProductIDPC.Value));
                cmd.Parameters.AddWithValue("@branchID", Convert.ToInt32(hdbranchIDPC.Value));
                cmd.Parameters.AddWithValue("@compnay", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@PONumber", Convert.ToString(Session["PurchaseOrder_Id"]));
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                if (dsInst.Tables != null)
                {


                    if (Session["POwarehousedetailstemp"] != null)
                    {
                        DataTable Warehousedtss = (DataTable)Session["POwarehousedetailstemp"];

                        DataRow[] dr = Warehousedtss.Select("productid = '" + Convert.ToString(hdfProductIDPC.Value) + "' AND pcslno = '" + hdnpcslno.Value + "'");
                        if (dr.Count() > 0)
                        {

                            Warehousedtss = dr.CopyToDataTable();
                            Warehousedtss.DefaultView.Sort = "SrlNo asc";
                            Warehousedtss = Warehousedtss.DefaultView.ToTable(true);

                            Session["PCWarehouseDatabyslno"] = Warehousedtss;

                            dt2 = (DataTable)Session["PCWarehouseDatabyslno"];
                            DataTable dtmp = dsInst.Tables[0];

                            if (Session["POwarehousedetailstempUpdate"] != null)
                            {
                                DataRow[] drs = Warehousedtss.Select("productid = '" + Convert.ToString(hdfProductIDPC.Value) + "' AND pcslno = '" + hdnpcslno.Value + "' AND isnew='Updated'");
                                if (drs.Count() <= 0)
                                {
                                    dt2.Merge(dtmp);
                                }
                            }

                            dt = dt2;

                        }
                        else
                        {
                            dt = dsInst.Tables[0];
                        }

                    }
                    else
                    {
                        dt = dsInst.Tables[0];
                    }

                }
            }
            else if (Session["POwarehousedetailstemp"] != null)
            {
                DataTable Warehousedtss = (DataTable)Session["POwarehousedetailstemp"];

                DataRow[] dr = Warehousedtss.Select("productid = '" + Convert.ToString(hdfProductIDPC.Value) + "' AND pcslno = '" + hdnpcslno.Value + "'");
                if (dr.Count() > 0)
                {

                    Warehousedtss = dr.CopyToDataTable();
                    Warehousedtss.DefaultView.Sort = "SrlNo asc";
                    Warehousedtss = Warehousedtss.DefaultView.ToTable(true);

                    Session["PCWarehouseDatabyslno"] = Warehousedtss;

                    dt = (DataTable)Session["PCWarehouseDatabyslno"];

                }

            }

            return dt;
        }

        private string getinventry()
        {

            string inventryType = string.Empty;
            if (hdniswarehouse.Value == "true")
            {
                inventryType = "WH";

            }
            if (hdnisbatch.Value == "true")
            {

                inventryType += "BT";
            }
            if (hdnisserial.Value == "true")
            {
                inventryType += "SL";

            }
            return inventryType;
        }

        public string SetVisibilityStock(object container)
        {
            string vs = string.Empty;
            GridViewDataItemTemplateContainer c = container as GridViewDataItemTemplateContainer;
            if (!string.IsNullOrEmpty(Convert.ToString(c.KeyValue)))
            {
                //if (isexist)
                //{
                vs = "display:block";
                //}
                //else
                //{
                //    vs = "display:none";
                //}

            }
            else
            {
                vs = "display:block";
            }

            return vs;


        }

        public void deleteALL()
        {

            #region delete

            if (Session["POwarehousedetailstempDelete"] != null)
            {
                string stockid = string.Empty;
                DataTable Warehousedtups = (DataTable)Session["POwarehousedetailstempDelete"];
                for (int i = 0; i <= Warehousedtups.Rows.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"])) && Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) != "0")
                    {
                        stockid = Convert.ToString(Warehousedtups.Rows[i]["StockID"]);

                        if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteSL")
                        {
                            string sqls = "delete Trans_StockSerialMapping where PurchaseOrder_MapId=" + Convert.ToString(Warehousedtups.Rows[i]["SerialID"]);

                            oDBEngine.GetDataTable(sqls);
                        }
                        else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHBTSL")
                        {

                            string sqls = "delete from tbl_trans_PurchaseOrderWarehouseDetails where PurchaseOrderWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderWarehouse where PurchaseOrderWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderBatch where PurchaseOrderBatch_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderSerialMapping where PurchaseOrder_BatchId=" + Convert.ToString(Warehousedtups.Rows[i]["BatchID"]);


                            oDBEngine.GetDataTable(sqls);


                        }
                        else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHSL")
                        {

                            string sqls = "delete from tbl_trans_PurchaseOrderWarehouseDetails where PurchaseOrderWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from Trans_StockBranchWarehouse where PurchaseOrderWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderBatch where PurchaseOrderBatch_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderSerialMapping where PurchaseOrder_BatchId=" + Convert.ToString(Warehousedtups.Rows[i]["BatchID"]);


                        }
                        else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHBT")
                        {
                            string sqls = string.Empty;
                            var updateqnty = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'DeleteWHBT' AND BatchWarehouseID='" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + "'");

                            if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            { updateqnty = 0.0; }


                            if (stockid != "0")
                            {
                                DataSet dsEmail = new DataSet();

                                //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                                String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                                SqlConnection con = new SqlConnection(conn);
                                SqlCommand cmd3 = new SqlCommand("prc_POwarehousbatchDeleteWHBT", con);
                                cmd3.CommandType = CommandType.StoredProcedure;

                                cmd3.Parameters.AddWithValue("@compnay", Convert.ToString(Session["LastCompany"]));
                                cmd3.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));
                                cmd3.Parameters.AddWithValue("@StockID", Convert.ToInt32(stockid));

                                cmd3.Parameters.AddWithValue("@BatchWarehouseID", Convert.ToInt32(Warehousedtups.Rows[i]["BatchWarehouseID"]));
                                cmd3.Parameters.AddWithValue("@BatchWarehousedetailsID", Convert.ToInt32(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]));
                                cmd3.Parameters.AddWithValue("@BatchID", Convert.ToInt32(Warehousedtups.Rows[i]["BatchID"]));


                                //cmd3.Parameters.AddWithValue("@branchID", Convert.ToInt32(hdnselectedbranch.Value));
                                cmd3.Parameters.AddWithValue("@modifiedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                                cmd3.Parameters.AddWithValue("@rate", Convert.ToDecimal(hdnrate.Value));
                                cmd3.Parameters.AddWithValue("@updateqnty", Convert.ToDecimal(updateqnty));

                                SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                                output.Direction = ParameterDirection.Output;
                                cmd3.Parameters.Add(output);
                                cmd3.CommandTimeout = 0;
                                SqlDataAdapter Adap = new SqlDataAdapter();
                                Adap.SelectCommand = cmd3;
                                Adap.Fill(dsEmail);
                                dsEmail.Clear();
                                cmd3.Dispose();
                                con.Dispose();
                                GC.Collect();
                            }
                        }
                        else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWH")
                        {



                            string sqls = "delete from tbl_trans_PurchaseOrderWarehouseDetails where PurchaseOrderWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderWarehouse where PurchaseOrderWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderBatch where PurchaseOrderBatch_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]);

                            oDBEngine.GetDataTable(sqls);

                        }
                    }


                }


            }
            #endregion

        }


        private int Insertwaredataalldata(string ProductID, string stockid, string branchid)
        {
            DataSet dsEmail = new DataSet();
            string inventryType = string.Empty;
            string IsserialActive = "false";

            int newrowcount = 0;

            #region Insert
            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
            String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd3 = new SqlCommand("prc_StockPChallanwarehousentry", con);
            cmd3.CommandType = CommandType.StoredProcedure;

            cmd3.Parameters.AddWithValue("@compnay", Convert.ToString(Session["LastCompany"]));
            cmd3.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));
            cmd3.Parameters.AddWithValue("@StockID", Convert.ToInt32(stockid));
            cmd3.Parameters.AddWithValue("@ProductID", Convert.ToInt32(ProductID));
            cmd3.Parameters.AddWithValue("@branchID", Convert.ToInt32(branchid));
            cmd3.Parameters.AddWithValue("@createdBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            cmd3.Parameters.AddWithValue("@PCNumber", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            cmd3.Parameters.AddWithValue("@totalqntity", Convert.ToString(hdntotalqntyPC.Value));


            if (hdniswarehouse.Value == "true")
            {
                inventryType = "WH";

            }
            if (hdnisbatch.Value == "true")
            {

                inventryType += "BT";
            }
            if (hdnisserial.Value == "true")
            {
                inventryType += "SL";
                IsserialActive = "true";
            }

            cmd3.Parameters.AddWithValue("@IsSerialActive", IsserialActive);


            cmd3.Parameters.AddWithValue("@Inventrytype", inventryType);

            DataTable temtable = new DataTable();
            DataTable temtable2 = new DataTable();
            DataTable temtable23 = new DataTable();
            if (Session["POCWarehouseData"] != null)
            {
                DataTable Warehousedt = (DataTable)Session["POCWarehouseData"];

                temtable = Warehousedt.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "productid", "Inventrytype", "StockID", "isnew");
                cmd3.Parameters.AddWithValue("@udt_StockOpeningwarehousentried", temtable);
            }
            if (Session["POWarehouseDataDelete"] != null)
            {
                DataTable Warehousedts = (DataTable)Session["POWarehouseDataDelete"];

                temtable2 = Warehousedts.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantity", "isnew");
                cmd3.Parameters.AddWithValue("@udt_StockOpeningwarehousentrieDelete", null);
            }

            #endregion

            return 1;


        }
        private int updatewarehouse()
        {
            if (Session["POwarehousedetailstempUpdate"] != null)
            {
                DataTable dt = new DataTable();
                DataTable Warehousedtups = (DataTable)Session["POwarehousedetailstempUpdate"];
                DataRow[] dr = Warehousedtups.Select("isnew = 'Updated'");
                if (dr.Count() != 0)
                {
                    dt = dr.CopyToDataTable();


                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        string iswarehouse = string.Empty;
                        string isbatch = string.Empty;
                        string isserial = string.Empty;



                        #region WHBTSL
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "WHBTSL")
                        {
                            DateTime? mfgex;
                            DateTime? exdate;
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
                            {
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
                                {
                                    mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
                                    exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
                                }
                                else
                                {
                                    mfgex = null;
                                    exdate = null;
                                }

                            }
                            else
                            {
                                mfgex = null;
                                exdate = null;
                            }

                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])))
                            {
                                string sql = string.Empty;
                                if (mfgex == null && exdate == null)
                                {
                                    if (hdnisolddeleted.Value == "true")
                                    {

                                        var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");
                                        if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                                        {
                                            updateqnty = Convert.ToDecimal(dt.Rows[i]["Quantitysum"]);
                                        }

                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                 
                                                             "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                           "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                      "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                      "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                                      " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                      " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                    else
                                    {
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                   
                                                               "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                             "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                             "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                             "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                        "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                        "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                                        " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                        " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                }
                                else
                                {
                                    if (hdnisolddeleted.Value == "true")
                                    {

                                        var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");
                                        if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                                        {
                                            updateqnty = Convert.ToDecimal(dt.Rows[i]["Quantitysum"]);
                                        }
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                    
                                                                 "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                               "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                               "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                               "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                               "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                               "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                          "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                          "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                                          " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                          " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                    else
                                    {
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                
                                                            "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                          "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                          "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                          "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                          "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                          "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                          "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                          "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                                          " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                          " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }


                                }


                                oDBEngine.GetDataTable(sql);

                            }
                            else
                            {
                                //var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");


                                string sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                            

                                                  "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                        " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                        " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

                                oDBEngine.GetDataTable(sql);
                            }
                        }
                        #endregion
                        #region WHBT
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "WHBT")
                        {
                            DateTime? mfgex;
                            DateTime? exdate;

                            var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");
                            var OLDSAMEwarehouse = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'old' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");
                            if (!string.IsNullOrEmpty(Convert.ToString(OLDSAMEwarehouse)) && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            {

                                updateqnty = Convert.ToDecimal(updateqnty) + Convert.ToDecimal(OLDSAMEwarehouse);
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
                            {
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
                                {
                                    mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
                                    exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
                                }
                                else
                                {
                                    mfgex = null;
                                    exdate = null;
                                }

                            }
                            else
                            {
                                mfgex = null;
                                exdate = null;
                            }

                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])) && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            {
                                string sql = string.Empty;
                                if (mfgex == null && exdate == null)
                                {
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                             
                                                         "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                       "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                       "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                       "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                  "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);


                                }
                                else
                                {
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                
                                                             "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                           "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                           "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                           "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                      "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);
                                }


                                oDBEngine.GetDataTable(sql);

                            }


                        }
                        #endregion
                        #region WHSL
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "WHSL")
                        {
                            DateTime? mfgex;
                            DateTime? exdate;
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
                            {
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
                                {
                                    mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
                                    exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
                                }
                                else
                                {
                                    mfgex = null;
                                    exdate = null;
                                }

                            }
                            else
                            {
                                mfgex = null;
                                exdate = null;
                            }

                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])))
                            {
                                string sql = string.Empty;
                                if (mfgex == null && exdate == null)
                                {
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                              "  update tbl_trans_PurchaseOrderWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                         "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                         "ModifiedDate=GETDATE()" +
                                                         "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                         "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                       "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                       "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                       "PurchaseOrderBatch_ModifiedDate=GETDATE()" +
                                                  "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                  "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                        " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                        " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

                                }
                                else
                                {
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                  "  update tbl_trans_PurchaseOrderWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                             "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                             "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                             "ModifiedDate=GETDATE()" +
                                                             "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                             "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                           "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                           "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                           "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "PurchaseOrderBatch_ModifiedDate=GETDATE()" +
                                                      "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                      "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                            " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                            " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                }


                                oDBEngine.GetDataTable(sql);

                            }
                            else
                            {
                                //var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");


                                string sql = "update tbl_trans_PurchaseOrderWarehouse set StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                              "  update tbl_trans_PurchaseOrderWarehouseDetails set " +
                                                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                         "ModifiedDate=GETDATE()" +
                                                         "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +

                                                  "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                        " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                        " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

                                oDBEngine.GetDataTable(sql);
                            }
                        }
                        #endregion

                        #region WH
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "WH")
                        {
                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Quantity"])))
                            {

                                string sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantity"]) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                            
                                                       "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                     "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantity"]) + "," +


                                                     "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                     "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                " where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);




                                oDBEngine.GetDataTable(sql);

                            }


                        }
                        #endregion

                        #region BTSL
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "BTSL")
                        {
                            DateTime? mfgex;
                            DateTime? exdate;
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
                            {
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
                                {
                                    mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
                                    exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
                                }
                                else
                                {
                                    mfgex = null;
                                    exdate = null;
                                }

                            }
                            else
                            {
                                mfgex = null;
                                exdate = null;
                            }

                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])))
                            {
                                string sql = string.Empty;
                                if (mfgex == null && exdate == null)
                                {
                                    if (hdnisolddeleted.Value == "true")
                                    {

                                        var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated'  AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");
                                        if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                                        {
                                            updateqnty = Convert.ToDecimal(dt.Rows[i]["Quantitysum"]);
                                        }

                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                  
                                                             "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                           "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                      "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                      "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                            " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                            " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                    else
                                    {
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                   
                                                               "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                             "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                             "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                             "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                        "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                        "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                              " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                              " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                }
                                else
                                {
                                    if (hdnisolddeleted.Value == "true")
                                    {

                                        var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");
                                        if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                                        {
                                            updateqnty = Convert.ToDecimal(dt.Rows[i]["Quantitysum"]);
                                        }
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                     
                                                                 "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                               "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                               "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                               "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                               "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                               "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                          "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                          "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                                " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                    else
                                    {
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                 
                                                            "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                          "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                          "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                          "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                          "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                          "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                     "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                     "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                           " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                           " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }


                                }


                                oDBEngine.GetDataTable(sql);

                            }
                            else
                            {
                                //var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");


                                string sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                              "  update tbl_trans_PurchaseOrderWarehouseDetails set " +
                                                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                         "ModifiedDate=GETDATE() " +
                                                         "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +

                                                  "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                        " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                        " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

                                oDBEngine.GetDataTable(sql);
                            }
                        }
                        #endregion

                        #region BT
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "BTSL")
                        {
                            DateTime? mfgex;
                            DateTime? exdate;

                            var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");
                            var OLDSAMEwarehouse = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'old' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");
                            if (!string.IsNullOrEmpty(Convert.ToString(OLDSAMEwarehouse)) && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            {

                                updateqnty = Convert.ToDecimal(updateqnty) + Convert.ToDecimal(OLDSAMEwarehouse);
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
                            {
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
                                {
                                    mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
                                    exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
                                }
                                else
                                {
                                    mfgex = null;
                                    exdate = null;
                                }

                            }
                            else
                            {
                                mfgex = null;
                                exdate = null;
                            }

                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])) && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            {
                                string sql = string.Empty;
                                if (mfgex == null && exdate == null)
                                {
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                             
                                                         "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                       "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                       "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                       "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                  "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);


                                }
                                else
                                {
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                 
                                                             "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                           "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                           "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                           "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                      "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);
                                }


                                oDBEngine.GetDataTable(sql);

                            }


                        }
                        #endregion

                    }
                }
            }

            return 0;
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
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "TaxDetailsForGst");
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["PurchaseOrder_Id"]));
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
                GetStock(Convert.ToString(performpara.Split('~')[2]));
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
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "ProductEditedTaxDetailsForPo");
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["PurchaseOrder_Id"]));
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
                // DataTable databaseReturnTable = (DataTable)Session["QuotationTaxDetails"];

                //if (Convert.ToInt32(e.Parameters.Split('~')[1]) == 1)
                //    taxDetail = oDBEngine.GetDataTable("select TaxRates_ID as Taxes_ID,TaxRatesSchemeName as Taxes_Name,TaxCalculateMethods,tm.Taxes_Name as taxCodeName,tm.Taxes_ApplicableOn as 'ApplicableOn',isnull((select TaxRatesSchemeName from Config_TaxRates where TaxRates_ID=tm.Taxes_OtherTax),'')  as dependOn from Master_taxes tm inner join Config_TaxRates ct on tm.Taxes_ID=ct.TaxRates_TaxCode where tm.TaxTypeCode not in('G','V','C') and tm.TaxItemlevel='Y'");
                //else if (Convert.ToInt32(e.Parameters.Split('~')[1]) == 2)
                //taxDetail = oDBEngine.GetDataTable("select TaxRates_ID as Taxes_ID,TaxRatesSchemeName as Taxes_Name,TaxCalculateMethods,tm.Taxes_Name as taxCodeName,tm.Taxes_ApplicableOn as 'ApplicableOn',isnull((select TaxRatesSchemeName from Config_TaxRates where TaxRates_ID=tm.Taxes_OtherTax),'')  as dependOn from Master_taxes tm inner join Config_TaxRates ct on tm.Taxes_ID=ct.TaxRates_TaxCode where tm.TaxTypeCode not in('G','V','C') and tm.TaxItemlevel='Y'");
                ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
                proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                proc.AddVarcharPara("@S_quoteDate", 10, dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                taxDetail = proc.GetTable();

                //Get Company Gstin 09032017
                string CompInternalId = Convert.ToString(Session["LastCompany"]);
                string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);

                string ShippingState = "";
                if (chkBilling.Checked)
                {
                    if (CmbState.Value != null)
                    {
                        ShippingState = CmbState.Text;
                        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    }
                }
                else
                {
                    if (CmbState1.Value != null)
                    {
                        ShippingState = CmbState1.Text;
                        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    }
                }



                if (ShippingState.Trim() != "" && compGstin[0].Trim() != "")
                {

                    if (compGstin.Length > 0)
                    {
                        if (compGstin[0].Substring(0, 2) == ShippingState)
                        {
                            //Check if the state is in union territories then only UTGST will apply
                            //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU    Lakshadweep              PONDICHERRY
                            if (ShippingState == "4" || ShippingState == "35" || ShippingState == "26" || ShippingState == "25" || ShippingState == "31" || ShippingState == "34")
                            {
                                foreach (DataRow dr in taxDetail.Rows)
                                {
                                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
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
                if (compGstin[0].Trim() == "")
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

                //Check If any TaxScheme Set Against that Product Then update there rate 22-03-2017 and rate
                string[] schemeIDViaProdID = oDBEngine.GetFieldValue1("master_sproducts", "isnull(sProduct_TaxSchemeSale,0)sProduct_TaxSchemeSale", "sProducts_ID='" + Convert.ToString(setCurrentProdCode.Value) + "'", 1);
                //&& schemeIDViaProdID[0] != ""
                if (schemeIDViaProdID.Length > 0)
                {

                    if (taxDetail.Select("Taxes_ID='" + schemeIDViaProdID[0] + "'").Length > 0)
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                            {
                                if (Convert.ToString(dr["Taxes_ID"]).Trim() != schemeIDViaProdID[0].Trim())
                                    dr["TaxRates_Rate"] = 0;
                            }
                        }
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
                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
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
                        }
                        else if (Convert.ToString(dr["ApplicableOn"]) == "N")
                        {
                            obj.calCulatedOn = ProdNetAmt;
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
                                    obj.calCulatedOn = Math.Round(finalCalCulatedOn);
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




                        DataRow[] filtr = MainTaxDataTable.Select("TaxCode ='" + obj.Taxes_ID + "' and slNo=" + Convert.ToString(slNo));
                        if (filtr.Length > 0)
                        {
                            obj.Amount = Convert.ToDouble(filtr[0]["Amount"]);
                            if (obj.Taxes_ID == 0)
                            {
                                //   obj.TaxField = GetTaxName(Convert.ToInt32(Convert.ToString(filtr[0]["AltTaxCode"])));
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
                        }
                        else if (Convert.ToString(dr["ApplicableOn"]) == "N")
                        {
                            obj.calCulatedOn = ProdNetAmt;
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
                                    obj.calCulatedOn = Math.Round(finalCalCulatedOn);
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
                        }
                        else
                        {
                            #region checkingFordb


                            //DataRow[] filtr = databaseReturnTable.Select("ProductTax_ProductId ='" + keyValue + "' and ProductTax_QuoteId=" +Convert.ToString( Session["QuotationID"] )+ " and ProductTax_TaxTypeId=" + obj.Taxes_ID);
                            //if (filtr.Length > 0)
                            //{
                            //    obj.Amount = Convert.ToDouble(filtr[0]["ProductTax_Amount"]);
                            //    if (obj.Taxes_ID == 0)
                            //    {
                            //        //obj.TaxField = GetTaxName();
                            //        obj.TaxField = "0";
                            //    }
                            //    else
                            //    {
                            //        obj.TaxField = Convert.ToString(filtr[0]["ProductTax_Percentage"]);
                            //    }


                            //    DataRow[] filtronexsisting = TaxRecord.Select("TaxCode=" + obj.Taxes_ID + " and SlNo=" + Convert.ToString(slNo));
                            //    if (filtronexsisting.Length > 0)
                            //    {
                            //        filtronexsisting[0]["Amount"] = obj.Amount;
                            //        if (obj.Taxes_ID == 0)
                            //        {
                            //            filtronexsisting[0]["Percentage"] = 0;
                            //        }
                            //        else
                            //        {
                            //            filtronexsisting[0]["Percentage"] = obj.TaxField;
                            //        }

                            //    }
                            //    else
                            //    {

                            //        DataRow taxRecordNewRow = TaxRecord.NewRow();
                            //        taxRecordNewRow["SlNo"] = slNo;
                            //        taxRecordNewRow["TaxCode"] = obj.Taxes_ID;
                            //        taxRecordNewRow["AltTaxCode"] = "0";
                            //        taxRecordNewRow["Percentage"] = obj.TaxField;
                            //        taxRecordNewRow["Amount"] = obj.Amount;

                            //        TaxRecord.Rows.Add(taxRecordNewRow);
                            //    }

                            //}
                            //else
                            //{
                            //    DataRow[] filtronexsisting = TaxRecord.Select("TaxCode=" + obj.Taxes_ID + " and SlNo=" + Convert.ToString(slNo));
                            //    if (filtronexsisting.Length > 0)
                            //    {
                            //        DataRow taxRecordNewRow = TaxRecord.NewRow();
                            //        taxRecordNewRow["SlNo"] = slNo;
                            //        taxRecordNewRow["TaxCode"] = obj.Taxes_ID;
                            //        taxRecordNewRow["AltTaxCode"] = "0";
                            //        taxRecordNewRow["Percentage"] = 0.0;
                            //        taxRecordNewRow["Amount"] = "0";

                            //        TaxRecord.Rows.Add(taxRecordNewRow);
                            //    }
                            //}




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

                        //      DataRow[] filtrIndex = databaseReturnTable.Select("ProductTax_ProductId ='" + keyValue + "' and ProductTax_QuoteId=" + Session["QuotationID"] + " and ProductTax_TaxTypeId=0");
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

                retMsg = Convert.ToString(GetTotalTaxAmount(TaxDetailsDetails));
                aspxGridTax.JSProperties["cpUpdated"] = "ok~" + retMsg;

                TaxDetailsDetails = setCalculatedOn(TaxDetailsDetails, taxDetail);
                aspxGridTax.DataSource = TaxDetailsDetails;
                aspxGridTax.DataBind();

                #endregion
            }
        }


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
            //DataTable taxTableItemLvl = (DataTable)Session["PurchaseOrderFinalTaxRecord"];
            //foreach (DataRow dr in taxTableItemLvl.Rows)
            //    dr.Delete();
            //taxTableItemLvl.AcceptChanges();
            //Session["PurchaseOrderFinalTaxRecord"] = taxTableItemLvl;
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
        #region Quotation Tax Details

        public IEnumerable GetTaxes()
        {
            List<Taxes> TaxList = new List<Taxes>();
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
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
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "TaxDetails");
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["PurchaseOrder_Id"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
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
                    if (chkBilling.Checked)
                    {
                        if (CmbState.Value != null)
                        {
                            ShippingState = CmbState.Text;
                            ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                        }
                    }
                    else
                    {
                        if (CmbState1.Value != null)
                        {
                            ShippingState = CmbState1.Text;
                            ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                        }
                    }

                    if (ShippingState.Trim() != "" && compGstin[0].Trim() != "")
                    {
                        if (compGstin.Length > 0)
                        {
                            if (compGstin[0].Substring(0, 2) == ShippingState)
                            {
                                //Check if the state is in union territories then only UTGST will apply
                                //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU     Lakshadweep              PONDICHERRY
                                if (ShippingState == "4" || ShippingState == "35" || ShippingState == "26" || ShippingState == "25" || ShippingState == "31" || ShippingState == "34")
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
        protected void lookup_quotation_DataBinding(object sender, EventArgs e)
        {
            if (Session["IndentRequiData"] != null)
            {
                lookup_quotation.DataSource = (DataTable)Session["IndentRequiData"];
            }
        }

        protected void BindLookUp(string OrderDate, string BranchId)
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



            DataTable QuotationTable;

            string strBranch = Convert.ToString(BranchId);
            DataTable IndentTable = objPurchaseOrderBL.GetIndentOnPO(OrderDate, status, strBranch);
            if (IndentTable.Rows.Count > 0)
            {
                lookup_quotation.GridView.Selection.CancelSelection();
                lookup_quotation.DataSource = IndentTable;
                lookup_quotation.DataBind();
            }
            else
            {
                //lookup_quotation.GridView.Selection.CancelSelection();
                lookup_quotation.DataSource = null;
                lookup_quotation.DataBind();
            }



            Session["IndentRequiData"] = IndentTable;
        }
        protected void ComponentQuotation_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string status = string.Empty;
            string POrderDate = string.Empty;
            string customer = string.Empty;
            string IndentIds = string.Empty;
            if (e.Parameter.Split('~')[0] == "BindNullGrid")
            {
                lookup_quotation.GridView.Selection.UnselectAll();
                lookup_quotation.DataSource = null;
                lookup_quotation.DataBind();
                dt_Quotation.Text = "";
                ComponentQuotationPanel.JSProperties["cpNullGrid"] = "YES";
            }
            if (e.Parameter.Split('~')[0] == "BindIndentGrid")
            {

                if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                {
                    status = "DONE";
                }
                else
                {
                    status = "NOT-DONE";
                }

                lookup_quotation.GridView.Selection.UnselectAll();
                POrderDate = e.Parameter.Split('~')[1];
                string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
                DataTable IndentTable = objPurchaseOrderBL.GetIndentOnPO(POrderDate, status, strBranch);
                if (IndentTable.Rows.Count > 0)
                {
                    lookup_quotation.GridView.Selection.CancelSelection();
                    lookup_quotation.DataSource = IndentTable;
                    lookup_quotation.DataBind();
                }
                else
                {
                    //lookup_quotation.GridView.Selection.CancelSelection();
                    lookup_quotation.DataSource = null;
                    lookup_quotation.DataBind();
                }


                Session["IndentRequiData"] = IndentTable;
            }
            else if (e.Parameter.Split('~')[0] == "BindQuotationGridOnSelection")
            {
                if (grid_Products.GetSelectedFieldValues("Quotation_No").Count != 0)
                {
                    for (int i = 0; i < grid_Products.GetSelectedFieldValues("Quotation_No").Count; i++)
                    {
                        IndentIds += "," + grid_Products.GetSelectedFieldValues("Quotation_No")[i];
                    }
                    IndentIds = IndentIds.TrimStart(',');
                    lookup_quotation.GridView.Selection.UnselectAll();
                    if (!String.IsNullOrEmpty(IndentIds))
                    {
                        string[] eachQuo = IndentIds.Split(',');
                        if (eachQuo.Length > 1)//More tha one quotation
                        {
                            dt_Quotation.Text = "Multiple Select Quotation Dates";
                            //BindLookUp(Customer_Id, Order_Date);
                            foreach (string val in eachQuo)
                            {
                                lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                        else if (eachQuo.Length == 1)//Single Quotation
                        {
                            foreach (string val in eachQuo)
                            {
                                lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                        else//No Quotation selected
                        {
                            dt_Quotation.Text = "";
                            lookup_quotation.GridView.Selection.UnselectAll();
                        }
                    }
                }
                else if (grid_Products.GetSelectedFieldValues("Quotation_No").Count == 0)
                {
                    lookup_quotation.GridView.Selection.UnselectAll();
                    dt_Quotation.Text = "";
                }
            }

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

        protected void cgridProducts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "BindProductsDetails")
            {

                string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);
                String QuoComponent = "";
                List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("Indent_Id");
                foreach (object Quo in QuoList)
                {
                    QuoComponent += "," + Quo;
                }
                QuoComponent = QuoComponent.TrimStart(',');
                if (Quote_Nos != "$")
                {
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
                            dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsFromPO(QuoComponent, "", "", "Add");
                        }

                    }
                    else
                    {
                        dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsFromPO(QuoComponent, "", "", "Add");
                    }
                    Session["ProductOrderDetails"] = null;

                    grid_Products.DataSource = GetProductsInfo(dt_QuotationDetails);
                    grid_Products.DataBind();
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
            List<SalesOrder> OrderList = new List<SalesOrder>();
            for (int i = 0; i < SalesOrderdt1.Rows.Count; i++)
            {
                SalesOrder Orders = new SalesOrder();

                Orders.SrlNo = Convert.ToString(i + 1);
                Orders.Key_UniqueId = Convert.ToString(i + 1);
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Indent_No"])))
                { Orders.Quotation_No = Convert.ToInt64(SalesOrderdt1.Rows[i]["Indent_No"]); }
                else
                { Orders.Quotation_No = 0; }
                Orders.gvColProduct = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductId"]);
                Orders.gvColDiscription = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductDescription"]);
                Orders.gvColQuantity = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Quantity"]);
                Orders.Quotation_Num = Convert.ToString(SalesOrderdt1.Rows[i]["Indent"]);
                Orders.Product_Shortname = Convert.ToString(SalesOrderdt1.Rows[i]["Product_Name"]);
                Orders.QuoteDetails_Id = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Id"]);

                OrderList.Add(Orders);
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


        //Sudip Pal
//        [WebMethod]
//        public static string getProductType(string Products_ID)
//        {
//            string Type = "";
//            string query = @"Select
//                           (Case When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then ''
//                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then 'W'
//                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'B'
//                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'S'
//                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'WB'
//                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'WS'
//                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'WBS'
//                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'BS'
//                           END) as Type
//                           from Master_sProducts
//                           where sProducts_ID='" + Products_ID + "'";

//            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
//            DataTable dt = oDbEngine.GetDataTable(query);
//            if (dt != null && dt.Rows.Count > 0)
//            {
//                Type = Convert.ToString(dt.Rows[0]["Type"]);
//            }

//            return Convert.ToString(Type);
//        }

        //Sudip Pal
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





        ///Sudip Pal
        #region Opening Purchase Order

        public void GetFinancialYearstartandEnddate()
        {
            CustomerSales oms = new CustomerSales();
            DataTable dtsale = new DataTable();
            if (Session["LastFinYear"] != null)
            {
                dtsale = oms.GetCustomersalesFinancialyear(Convert.ToString(Session["LastFinYear"]));
                if (dtsale.Rows.Count > 0)
                {
                    //  tDate.MinDate = Convert.ToDateTime(Convert.ToString(dtsale.Rows[0]["FinYear_StartDate"])).AddYears(-); ;
                    dt_PLQuote.MaxDate = Convert.ToDateTime(Convert.ToString(dtsale.Rows[0]["FinYear_StartDate"])).AddDays(-1);
                    //tDate.Value = Convert.ToString(Convert.ToDateTime(Convert.ToString(dtsale.Rows[0]["FinYear_StartDate"])).AddDays(-1));
                    string fyrrrr = Convert.ToString(Convert.ToDateTime(Convert.ToString(dtsale.Rows[0]["FinYear_StartDate"])).AddDays(-1));
                    string[] FinYEnd = Convert.ToString(fyrrrr).Split(' ');
                    string FinYearEnd = FinYEnd[0];
                    DateTime date3 = DateTime.ParseExact(FinYearEnd, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    string ForJournalDate = Convert.ToString(date3);

                    string fDate = null;

                    fDate = Convert.ToString(Convert.ToDateTime(ForJournalDate).Month) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Day) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Year);


                    dt_PLQuote.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }

            }


        }

        public string GetFinancialYearCheckAccordingDaterange(DateTime dtdate)
        {
            CustomerSales oms = new CustomerSales();
            string dtsale = String.Empty;
            DataTable dttab = new DataTable();

            if (Session["LastFinYear"] != null)
            {
                dttab = oms.GetCustomersalesFinancialyearCode(dtdate);
                if (dttab.Rows.Count > 0)
                {
                    dtsale = Convert.ToString(dttab.Rows[0]["FinYear_Code"]);
                }

            }
            return dtsale;
        }
        #endregion


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

            //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
            DataTable dt = oDbEngine.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }

            return Convert.ToString(Type);
        }

        ///Sudip Pal
        

    }
}