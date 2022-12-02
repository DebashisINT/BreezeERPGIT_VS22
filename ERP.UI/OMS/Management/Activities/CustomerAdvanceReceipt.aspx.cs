using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using System.Linq;
using BusinessLogicLayer;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections;
using DataAccessLayer;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using ERP.OMS.Tax_Details.ClassFile;

namespace ERP.OMS.Management.Activities
{
    public partial class CustomerAdvanceReceipt : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();


        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        PurchaseOrderBL objPurchaseOrderBL = new PurchaseOrderBL();
        CustomerVendorReceiptPaymentBL objCustomerVendorReceiptPaymentBL = new CustomerVendorReceiptPaymentBL();
        GSTtaxDetails _ObjGSTtaxDetails = new GSTtaxDetails();

        public string pageAccess = "";
        string JVNumStr = string.Empty;

        #region Page Events
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (Request.QueryString.AllKeys.Contains("IsTagged"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
            }

            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/CustomerReceiptPayment.aspx");
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                PaymentDetails.doc_type = "CRP";
                Session["CustomerDetailForCRP"] = null;
                Session["AdvanceReceipt_ID"] = null;
                Session["IBRef"] = null;
                Session["AdvanceReceiptDetails"] = null;
                Session["DocumentNoDetails"] = null;

                if (Request.QueryString["IsTagged"] != null)
                {
                    customerReceiptCross.Style.Add("display", "none");
                    customerReceiptPopupCross.Style.Add("display", "block");
                }

                // ComboVoucherType.SelectedValue = "R";
                IsUdfpresent.Value = Convert.ToString(getUdfCount());
                BindBranch();
                Bind_Currency();
                BindSystemSettings();
                string userbranch = "";
                if (Session["userbranchHierarchy"] != null)
                {
                    userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                }
                DataSet dst = BindInclusiveExclusiveDropdown(userbranch);
                if (dst.Tables[4] != null && dst.Tables[4].Rows.Count > 0)
                {
                    ddl_AmountAre.TextField = "taxGrp_Description";
                    ddl_AmountAre.ValueField = "taxGrp_Id";
                    ddl_AmountAre.DataSource = dst.Tables[4];
                    ddl_AmountAre.DataBind();
                    ddl_AmountAre.Value = "2";

                }
                if (!String.IsNullOrEmpty(Convert.ToString(Session["userbranchID"])))
                {
                    string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                    ddlBranch.SelectedValue = strdefaultBranch;
                    BindCashBankAccount(strdefaultBranch);

                }

                if (!String.IsNullOrEmpty(Convert.ToString(Session["LocalCurrency"])))
                {
                    string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);
                    string basedCurrency = Convert.ToString(LocalCurrency.Split('~')[0]);
                    string CurrencyId = Convert.ToString(basedCurrency[0]);
                    CmbCurrency.Value = CurrencyId;
                }

                if (Request.QueryString["IsTagged"] == "Y")
                {
                    IsInvoiceTagged.Value = "Y";
                    gridBatch.Settings.VerticalScrollableHeight = 70;
                }
                else
                {
                    IsInvoiceTagged.Value = "N";
                }


                if (Request.QueryString["key"] == "ADD")
                {
                    ddl_AmountAre.ClientEnabled = true;
                    hdnPageStatus.Value = "first";
                    hdn_Mode.Value = "Entry";
                    dtTDate.Date = DateTime.Now;

                    #region To Show By Default Cursor after SAVE AND NEW

                    if (Session["SaveModeCRP"] != null)  // it has been removed from coding side of Quotation list 
                    {
                        if (Session["schemavalueCRP"] != null)  // it has been removed from coding side of Quotation list 
                        {
                            CmbScheme.Value = Convert.ToString(Session["schemavalueCRP"]); // it has been removed from coding side of Quotation list 
                        }
                        if (Convert.ToString(Session["SaveModeCRP"]) == "A")
                        {
                            dtTDate.Focus();
                            txtVoucherNo.Enabled = false;
                            txtVoucherNo.Text = "Auto";
                        }
                        else if (Convert.ToString(Session["SaveModeCRP"]) == "M")
                        {
                            txtVoucherNo.Enabled = true;
                            txtVoucherNo.Text = "";

                            txtVoucherNo.Focus();

                        }
                    }
                    else
                    {
                    }

                    #endregion To Show By Default Cursor after SAVE AND NEW

                    gridBatch.DataSource = null;
                    gridBatch.DataBind();
                    Keyval_internalId.Value = "Add";

                    #region Customer Receipt/PaymentOpen From Other Module

                    if (Request.QueryString["IsTagged"] == "Y")
                    {
                        string ComponentType = Convert.ToString(Request.QueryString["ComponentType"]);
                        string ComponentID = Convert.ToString(Request.QueryString["ComponentID"]);
                        string PageStatus = Convert.ToString(Request.QueryString["PageStatus"]);
                        btnSaveNew.Visible = false;

                        if (ComponentType == "I")
                        {
                            ddlBranch.Enabled = false;
                            cmbContactPerson.ClientEnabled = false;
                            lookup_Customer.ClientEnabled = false;
                            ddlCashBank.ClientEnabled = false;
                            multipleredio.Visible = false;

                            hdnPageStatus.Value = "tagfirst";
                            hdnInvoicePageStatus.Value = PageStatus;
                            hdnTaggedFrom.Value = ComponentType;

                            Session["AdvanceReceiptDetails"] = GetCustomerReceiptPaymentBatchData_ByInvoice(ComponentID).Tables[0];
                            gridBatch.DataSource = BindInvoiceReceiptPaymentBatch(ComponentID);
                            gridBatch.DataBind();
                        }
                    }

                    #endregion

                    CreateDataTaxTable();
                }
                else
                {
                    gridBatch.JSProperties["cpView"] = (Request.QueryString["req"] != null && Request.QueryString["req"] == "V") ? "1" : "0";
                    gridBatch.JSProperties["cpUnpaidAmountEqual"] = (Request.QueryString["key"] != null && !objCustomerVendorReceiptPaymentBL.IsUnpaidAmountEqual(Convert.ToString(Request.QueryString["key"]))) ? "1" : "0";

                    ddl_AmountAre.ClientEnabled = false;
                    hdnPageStatus.Value = "update";
                    hdn_Mode.Value = "Edit";
                    txtVoucherNo.Enabled = false;
                    ddlBranch.Enabled = false;
                    divNumberingScheme.Style.Add("display", "none");

                    if (Request.QueryString["req"] == "V")
                    {
                        lblHeadTitle.Text = "View Customer Advance Receipt";
                    }
                    else
                    {
                        lblHeadTitle.Text = "Modify Customer Advance Receipt";
                    }

                    Session["AdvanceReceipt_ID"] = Request.QueryString["key"];
                    FillGrid();
                    Session["AdvanceReceiptDetails"] = GetCustomerReceiptPaymentBatchData().Tables[0];
                    gridBatch.DataSource = BindCustomerReceiptPaymentBatch();
                    gridBatch.DataBind();
                    Keyval_internalId.Value = "CustrRecePay" + Session["AdvanceReceipt_ID"];

                    if (IsCRTTransactionExist(Request.QueryString["key"]))
                    {
                        gridBatch.JSProperties["cpBtnVisible"] = "True";
                    }
                    if (IsTypemultiple(Request.QueryString["key"]))
                    {
                        gridBatch.JSProperties["cpMulType"] = "true";
                    }
                    #region Get Tax Details in Edit Mode

                    DataTable TaxTable = GetCRPTaxData().Tables[0];
                    if (TaxTable == null)
                    {
                        CreateDataTaxTable();
                    }
                    else
                    {
                        Session["CRPADV_FinalTaxRecord"] = TaxTable;
                    }

                    #endregion Get Tax Details in Edit Mode
                    if (objCustomerVendorReceiptPaymentBL.IsCRPExist(Request.QueryString["key"]))
                    {
                        gridBatch.JSProperties["cpBtnVisible"] = "True";
                    }
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);
                #region SetData if it is from Tab (POS)
                if (Request.QueryString["basketId"] != null)
                {
                    string basketId = Request.QueryString["basketId"];
                    SetBasketDetails(basketId);
                }
                #endregion
            }
            PopulateCustomerDetail();
            Bind_ReceiptNumberingScheme();

        }
        #endregion

        #region Clasess

        public class CRPDetailsLIST
        {
            public string SrlNo { get; set; }
            public string ReceiptDetail_ID { get; set; }
            public string Type { get; set; }
            public string DocumentID { get; set; }
            public string Receipt { get; set; }
            public string Payment { get; set; }
            public string Remarks { get; set; }
            public string DocumentNo { get; set; }
            public string IsOpening { get; set; }
            public string ProductID { get; set; }
            public string ProductName { get; set; }
            public string TaxAmount { get; set; }
            public string NetAmount { get; set; }
            public string Quantity { get; set; }
            public string TaxableAmount { get; set; }
        }
        public class DocumentType
        {
            public string Type { get; set; }
            public string ID { get; set; }
        }

        #endregion

        #region Database Section

        public DataTable GetCustomerReceiptPaymentEditData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerAdvanceReceiptDetails");
            proc.AddVarcharPara("@Action", 500, "CustomerReceiptPaymentEditDetails");
            proc.AddIntegerPara("@ReceiptPayment_ID", Convert.ToInt32(Session["AdvanceReceipt_ID"]));

            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetProductDetaisByID()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerAdvanceReceiptDetails");
            proc.AddVarcharPara("@Action", 500, "ProductEditDetails");
            proc.AddIntegerPara("@ReceiptPayment_ID", Convert.ToInt32(Session["AdvanceReceipt_ID"]));

            dt = proc.GetTable();
            return dt;
        }
        public IEnumerable BindCustomerReceiptPaymentBatch()
        {
            List<CRPDetailsLIST> CRPDetailsList = new List<CRPDetailsLIST>();
            DataTable CustomerReceiptPaymendt = GetCustomerReceiptPaymentBatchData().Tables[0];

            for (int i = 0; i < CustomerReceiptPaymendt.Rows.Count; i++)
            {
                CRPDetailsLIST CRPDetails = new CRPDetailsLIST();


                CRPDetails.SrlNo = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["SrlNo"]);
                CRPDetails.ReceiptDetail_ID = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["ReceiptDetail_ID"]);
                CRPDetails.Type = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Type"]);
                CRPDetails.DocumentID = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["DocumentID"]);
                CRPDetails.Receipt = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Receipt"]);
                CRPDetails.Payment = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Payment"]);
                CRPDetails.Remarks = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Remarks"]);
                CRPDetails.DocumentNo = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["DocumentNo"]);
                CRPDetails.IsOpening = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["IsOpening"]);
                CRPDetails.ProductID = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["ProductID"]);
                CRPDetails.ProductName = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["ProductName"]);
                CRPDetails.TaxAmount = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["TaxAmount"]);
                CRPDetails.NetAmount = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["NetAmount"]);
                CRPDetails.Quantity = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Quantity"]);
                CRPDetails.TaxableAmount = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["TaxableAmount"]);
                CRPDetailsList.Add(CRPDetails);
            }

            return CRPDetailsList;
        }
        public DataSet GetCustomerReceiptPaymentBatchData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerAdvanceReceiptDetails");
            proc.AddVarcharPara("@Action", 500, "CRPBatchEditDetails");
            proc.AddIntegerPara("@ReceiptPayment_ID", Convert.ToInt32(Session["AdvanceReceipt_ID"]));

            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetCustomerReceiptPaymentBatchData_ByInvoice(string strInvoiceId)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerAdvanceReceiptDetails");
            proc.AddVarcharPara("@Action", 500, "CRPBatchEditDetails_ByInvoice");
            proc.AddVarcharPara("@InvoiceID", 100, strInvoiceId);
            ds = proc.GetDataSet();
            return ds;
        }

        #endregion

        #region Bind Method
        public void BindSystemSettings()
        {
            DataTable dtSystemSettings = new DataTable();
            dtSystemSettings = objCustomerVendorReceiptPaymentBL.GetSystemSettings();
            if (dtSystemSettings.Rows.Count > 0)
            {
                string Variable_Value = Convert.ToString(dtSystemSettings.Rows[0]["Variable_Value"]);
                hdnSalesInvoice.Value = Variable_Value;
            }

        }
        public void BindCashBankAccount(string userbranch)
        {
            DataTable dtCustomer = new DataTable();
            string CompanyId = Convert.ToString(Session["LastCompany"]);
            dtCustomer = objCustomerVendorReceiptPaymentBL.GetCustomerCashBank(userbranch, CompanyId);
            if (dtCustomer.Rows.Count > 0)
            {
                ddlCashBank.TextField = "IntegrateMainAccount";
                ddlCashBank.ValueField = "MainAccount_ReferenceID";
                ddlCashBank.DataSource = dtCustomer;
                ddlCashBank.DataBind();
            }
            else
            {
                ddlCashBank.TextField = "IntegrateMainAccount";
                ddlCashBank.ValueField = "MainAccount_ReferenceID";
                ddlCashBank.DataSource = dtCustomer;
                ddlCashBank.DataBind();
            }

        }
        public void BindBranch()
        {
            dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
            ddlBranch.DataBind();
        }
        public void Bind_Currency()
        {
            SqlCurrencyBind.SelectCommand = "select Currency_ID,Currency_AlphaCode from Master_Currency Order By Currency_ID";
            CmbCurrency.DataBind();
        }
        public void Bind_PaymentNumberingScheme()
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

            // SqlSchematype.SelectCommand = "Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='22' AND IsActive=1)) as x Order By ID asc";
            SqlSchematype.SelectCommand = "Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName" +
                " +(Case When (SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch) Is Null Then '' " +
                " Else ' ('+(SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch)+')' End) From tbl_master_Idschema " +
            "Where TYPE_ID='22' AND IsActive=1 AND Isnull(Branch,'')  in (select s FROM dbo.GetSplit(',','" + userbranchHierarchy + "'))" +
            "AND financial_year_id=(Select Top 1 FinYear_ID FROM Master_FinYear WHERE FinYear_Code='" + FinYear.Trim() + "')  AND Isnull(comapanyInt,'')='" + strCompanyID + "')) as x Order By ID asc";
            CmbScheme.DataBind();
        }
        public void Bind_ReceiptNumberingScheme()
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string strSelectedBranchID = Convert.ToString(ddlBranch.SelectedValue);
            // SqlSchematype.SelectCommand = "Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='21' AND IsActive=1)) as x Order By ID asc";

            if (Convert.ToString(hdnTaggedFrom.Value) == "I")
            {
                SqlSchematype.SelectCommand = "Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName" +
                " +(Case When (SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch) Is Null Then '' " +
                " Else ' ('+(SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch)+')' End) From tbl_master_Idschema " +
                " Where TYPE_ID='21' AND IsActive=1 AND Isnull(Branch,'') in (select s FROM dbo.GetSplit(',','" + strSelectedBranchID + "'))" +
                " AND financial_year_id=(Select Top 1 FinYear_ID FROM Master_FinYear WHERE FinYear_Code='" + FinYear.Trim() + "')  AND Isnull(comapanyInt,'')='" + strCompanyID + "')) as x Order By ID asc";
                CmbScheme.DataBind();
            }
            else
            {
                SqlSchematype.SelectCommand = "Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName" +
                " +(Case When (SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch) Is Null Then '' " +
                " Else ' ('+(SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch)+')' End) From tbl_master_Idschema " +
                " Where TYPE_ID='21' AND IsActive=1 AND Isnull(Branch,'') in (select s FROM dbo.GetSplit(',','" + userbranchHierarchy + "'))" +
                " AND financial_year_id=(Select Top 1 FinYear_ID FROM Master_FinYear WHERE FinYear_Code='" + FinYear.Trim() + "')  AND Isnull(comapanyInt,'')='" + strCompanyID + "')) as x Order By ID asc";
                CmbScheme.DataBind();
            }
        }
        protected void CmbScheme_Callback(object sender, CallbackEventArgsBase e)
        {
            string command = e.Parameter;
            if (command == "P")
            {
                Bind_PaymentNumberingScheme();
                CmbScheme.Value = "0";

            }
            else if (command == "R")
            {
                Bind_ReceiptNumberingScheme();
                CmbScheme.Value = "0";

            }
        }
        public void PopulateCustomerDetail()
        {
            if (Session["CustomerDetailForCRP"] == null)
            {
                DataTable dtCustomer = new DataTable();
                dtCustomer = objCustomerVendorReceiptPaymentBL.PopulateCustomerDetail();

                if (dtCustomer != null && dtCustomer.Rows.Count > 0)
                {
                    lookup_Customer.DataSource = dtCustomer;
                    lookup_Customer.DataBind();

                    if (!string.IsNullOrEmpty(Request.QueryString["key"]) && !string.IsNullOrEmpty(Request.QueryString["SalId"]))
                    {
                        string udfCount = oDBEngine.ExeSclar("select sls_contactlead_id from tbl_trans_sales where sls_id=" + Request.QueryString["SalId"]);
                        if (!string.IsNullOrEmpty(udfCount))
                        {
                            lookup_Customer.GridView.Selection.SelectRowByKey(udfCount);
                        }
                    }
                    Session["CustomerDetailForCRP"] = dtCustomer;
                }
            }
            else
            {
                lookup_Customer.DataSource = (DataTable)Session["CustomerDetailForCRP"];
                lookup_Customer.DataBind();
            }

        }
        protected void CustomerCallBackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string action = e.Parameter.Split('~')[0];

            if (action == "SetCustomer")
            {
                Session["CustomerDetailForCRP"] = null;
                string CustomerInternalId = e.Parameter.Split('~')[1];
                PopulateCustomerDetail();
                // lookup_Customer.DataBind();
                lookup_Customer.GridView.Selection.SelectRowByKey(CustomerInternalId);
            }

        }

        protected void cmbContactPerson_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindContactPerson")
            {
                string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                PopulateContactPersonOfCustomer(InternalId);

                DataTable GSTNTable = objPurchaseOrderBL.GetVendorGSTIN(InternalId);
                string strGSTN = Convert.ToString(GSTNTable.Rows[0]["CNT_GSTIN"]).Trim();
                if (strGSTN != "")
                {
                    cmbContactPerson.JSProperties["cpGSTN"] = "Yes";
                }
                else
                {
                    cmbContactPerson.JSProperties["cpGSTN"] = "No";
                }

                string TransDate = e.Parameter.Split('~')[2];
                string BranchID = e.Parameter.Split('~')[3];
                DataTable TotalAmountInCash = objCustomerVendorReceiptPaymentBL.GetCustomerTotalAmountOnSingleDay(InternalId, TransDate, Convert.ToInt32(BranchID));
                if (TotalAmountInCash != null && TotalAmountInCash.Rows.Count > 0)
                {
                    CustomerCallBackPanel.JSProperties["cpTotalTransectionAmount"] = Convert.ToString(TotalAmountInCash.Rows[0][0]);
                }
                else
                {
                    CustomerCallBackPanel.JSProperties["cpTotalTransectionAmount"] = "0";
                }




            }
        }
        protected void PopulateContactPersonOfCustomer(string InternalId)
        {
            string ContactPerson = "";
            DataTable dtContactPerson = new DataTable();

            dtContactPerson = objCustomerVendorReceiptPaymentBL.PopulateContactPersonOfCustomer(InternalId);
            if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
            {
                cmbContactPerson.TextField = "contactperson";
                cmbContactPerson.ValueField = "add_id";
                cmbContactPerson.DataSource = dtContactPerson;
                cmbContactPerson.DataBind();
                foreach (DataRow dr in dtContactPerson.Rows)
                {
                    if (Convert.ToString(dr["Isdefault"]) == "True")
                    {
                        ContactPerson = Convert.ToString(dr["add_id"]);
                        break;
                    }
                }
                cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(ContactPerson);

            }

        }
        public void FillGrid()
        {
            DataTable CRPOrderEditdt = GetCustomerReceiptPaymentEditData();
            if (CRPOrderEditdt != null && CRPOrderEditdt.Rows.Count > 0)
            {
                string Type = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_TransactionType"]);//0
                if (Type == "P")
                {
                    multipleredio.Visible = false;
                }

                string VoucherNumber = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_VoucherNumber"]);//1
                string TransactionDate = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_TransactionDate"]);//2
                hdnDate.Value = TransactionDate;
                string BranchID = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_BranchID"]);//3
                HdSelectedBranch.Value = BranchID;
                string Customer_Id = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_CustomerID"]);//5

                string Contact_Person_Id = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_ContactPersonID"]);//6

                string CashBankID = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_CashBankID"]);//7
                string Currency_Id = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_Currency"]);//8
                string Rate = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_Rate"]);//9

                string InstrumentType = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_InstrumentType"]);//10
                string InstrumentNumber = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_InstrumentNumber"]);//11
                string InstrumentDate = Convert.ToString(CRPOrderEditdt.Rows[0]["InstrumentDate"]);//12

                string Narration = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_Narration"]);//13
                string VoucherAmount = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_VoucherAmount"]);//14
                int ReceiptPayment_Tax_Option = Convert.ToInt32(CRPOrderEditdt.Rows[0]["ReceiptPayment_Tax_Option"]);//15

                Session["IBRef"] = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_IBRef"]);//16

                if (Type == "P")
                {
                    GetDocumentDetails("DocumentType", "P", "", "");
                }
                else
                {
                    GetDocumentDetails("DocumentType", "", "", "");
                }

                txtVoucherNo.Text = VoucherNumber;
                dtTDate.Date = Convert.ToDateTime(TransactionDate);
                ddlBranch.SelectedValue = BranchID;
                BindCashBankAccount(BranchID);

                lookup_Customer.GridView.Selection.SelectRowByKey(Customer_Id);
                if (Customer_Id != "")
                {
                    hdfLookupCustomer.Value = Customer_Id;
                }
                pageheaderContent.Attributes.Add("style", "display:block");
                divGSTIN.Attributes.Add("style", "display:block");
                DataTable GSTNTable = objPurchaseOrderBL.GetVendorGSTIN(Customer_Id);
                string strGSTN = Convert.ToString(GSTNTable.Rows[0]["CNT_GSTIN"]).Trim();
                if (strGSTN != "")
                {
                    lblGSTIN.Text = "Yes";
                }
                else
                {
                    lblGSTIN.Text = "No";
                }
                PopulateContactPersonOfCustomer(Customer_Id);
                hdnCustomerId.Value = Customer_Id;
                if (Contact_Person_Id != "0")
                {
                    cmbContactPerson.Value = Contact_Person_Id;
                }

                ddlCashBank.Value = CashBankID;
                string strCashBank = ddlCashBank.Text;
                if (strCashBank != "0")
                {
                    string Cash_Bank = Convert.ToString(strCashBank.Split(']')[1]);
                    if (Cash_Bank.Trim() != "Bank")
                    {
                        divInstrumentNo.Style.Add("display", "none");
                        tdIDateDiv.Style.Add("display", "none");
                    }
                }

                CmbCurrency.Value = Currency_Id;
                txtRate.Text = Rate.Trim();
                cmbInstrumentType.Value = InstrumentType.Trim();
                txtInstNobth.Text = InstrumentNumber.Trim();
                if (!String.IsNullOrEmpty(InstrumentDate))
                {
                    InstDate.Date = Convert.ToDateTime(InstrumentDate);
                }

                txtNarration.Text = Narration.Trim();
                txtVoucherAmount.Text = VoucherAmount.Trim();
                ddl_AmountAre.Value = Convert.ToString(ReceiptPayment_Tax_Option);
            }
        }
        private bool IsTypemultiple(string CRTid)
        {
            bool IsExist = false;
            if (CRTid != "" && Convert.ToString(CRTid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = objCustomerVendorReceiptPaymentBL.CheckMultipleType(CRTid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
        }
        private bool IsCRTTransactionExist(string CRTid)
        {
            bool IsExist = false;
            if (CRTid != "" && Convert.ToString(CRTid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = objCustomerVendorReceiptPaymentBL.CheckCRTTraanaction(CRTid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
        }
        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='CRP' and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }

        #endregion

        #region GetRate ,getSchemeType

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
        [WebMethod]
        public static string getSchemeType(string sel_scheme_id)
        {
            //BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //string[] scheme = oDbEngine1.GetFieldValue1("tbl_master_Idschema", "schema_type", "Id = " + Convert.ToInt32(sel_scheme_id), 1);
            //return Convert.ToString(scheme[0]);

            string strschematype = "", strschemalength = "", strschemavalue = "", strschemaBranch = "", strVoucherType = "";


           // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
              BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length,Branch,VoucherType ", " Id = " + Convert.ToInt32(sel_scheme_id));

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strschemaBranch = Convert.ToString(DT.Rows[i]["Branch"]);
                strVoucherType = Convert.ToString(DT.Rows[i]["VoucherType"]);
                strschemavalue = strschematype + "~" + strschemalength + "~" + strschemaBranch + "~" + strVoucherType;
            }
            return Convert.ToString(strschemavalue);
        }

        #endregion

        #region Batch Grid

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
        protected void gridBatch_DataBinding(object sender, EventArgs e)
        {
            if (Session["AdvanceReceiptDetails"] != null)
            {
                //DataTable RPDdt = (DataTable)Session["AdvanceReceiptDetails"];
                //DataView dvData = new DataView(RPDdt);
                //var CPRLink = BindCustomerReceiptPaymentBatch(dvData.ToTable());
                //gridBatch.DataSource = CPRLink;
                //gridBatch.JSProperties["cpTotalAmount"] = Total_Receipt_Payment((List<CRPDetailsLIST>)CPRLink);

                DataTable RPDdt = (DataTable)Session["AdvanceReceiptDetails"];
                DataView dvData = new DataView(RPDdt);
                dvData.RowFilter = "Status <> 'D'";
                var CPRLink = BindCustomerReceiptPaymentBatch(dvData.ToTable());
                gridBatch.DataSource = CPRLink;
                gridBatch.JSProperties["cpTotalAmount"] = Total_Receipt_Payment((List<CRPDetailsLIST>)CPRLink);
            }
            else
            {
                gridBatch.DataSource = BindCustomerReceiptPaymentBatch();
            }
        }
        public string Total_Receipt_Payment(List<CRPDetailsLIST> Vouchers)
        {
            decimal total_receipt = 0, total_payment = 0;

            foreach (CRPDetailsLIST obj in Vouchers)
            {
                total_receipt += Convert.ToDecimal(obj.Receipt);
                total_payment += Convert.ToDecimal(obj.Payment);
            }

            return Convert.ToString(total_receipt) + "~" + Convert.ToString(total_payment);
            //return Convert.ToString(total_receipt);

        }
        private void bindMainAccount(object source, CallbackEventArgsBase e)
        {
            string strVoucherType = e.Parameter.Split('~')[0];
            string strPrement = e.Parameter.Split('~')[1];


            //FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            ASPxComboBox currentCombo = source as ASPxComboBox;
            currentCombo.DataSource = GetDocumentDetails("DocumentType", strVoucherType, Convert.ToString(dtTDate.Date), strPrement);
            currentCombo.DataBind();
        }
        protected void gridBatch_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            //if (e.Column.FieldName == "Type")
            //{

            //    ((ASPxComboBox)e.Editor).Callback += new CallbackEventHandlerBase(bindMainAccount);
            //}
            //e.Editor.ReadOnly = false;
            //e.Editor.Enabled = true;

            if (e.Column.FieldName == "TaxAmount")
            {
                e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "SrlNo")
            {
                e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "NetAmount")
            {
                e.Editor.ReadOnly = true;
            }
            else
            {
                e.Editor.ReadOnly = false;
            }
        }
        protected void gridBatch_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];

            if (strSplitCommand == "Display")
            {
                string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
                if (IsDeleteFrom == "D" || IsDeleteFrom == "C")
                {
                    DataTable AdvanceReceiptDetailsDT = (DataTable)Session["AdvanceReceiptDetails"];
                    //gridBatch.DataSource = BindCustomerReceiptPaymentBatch(AdvanceReceiptDetailsDT);
                    //gridBatch.DataBind();
                    DataView dvData = new DataView(AdvanceReceiptDetailsDT);
                    dvData.RowFilter = "Status <> 'D'";

                    gridBatch.DataSource = BindCustomerReceiptPaymentBatch(dvData.ToTable());
                    gridBatch.DataBind();
                }
            }
        }
        public IEnumerable BindCustomerReceiptPaymentBatch(DataTable CustomerReceiptPaymendt)
        {
            List<CRPDetailsLIST> CRPDetailsList = new List<CRPDetailsLIST>();
            //DataTable CustomerReceiptPaymendt = GetCustomerReceiptPaymentBatchData().Tables[0];

            for (int i = 0; i < CustomerReceiptPaymendt.Rows.Count; i++)
            {
                CRPDetailsLIST CRPDetails = new CRPDetailsLIST();

                CRPDetails.SrlNo = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["SrlNo"]);
                CRPDetails.ReceiptDetail_ID = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["ReceiptDetail_ID"]) == "" ? Convert.ToString(i) : Convert.ToString(CustomerReceiptPaymendt.Rows[i]["ReceiptDetail_ID"]);
                CRPDetails.Type = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Type"]);
                CRPDetails.DocumentID = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["DocumentID"]);
                CRPDetails.Receipt = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Receipt"]);
                CRPDetails.Payment = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Payment"]);
                CRPDetails.Remarks = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Remarks"]);
                CRPDetails.DocumentNo = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["DocumentNo"]);
                CRPDetails.IsOpening = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["IsOpening"]);
                CRPDetails.ProductID = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["ProductID"]);
                CRPDetails.ProductName = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["ProductName"]);
                CRPDetails.TaxAmount = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["TaxAmount"]);
                CRPDetails.NetAmount = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["NetAmount"]);
                CRPDetails.Quantity = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Quantity"]);
                CRPDetails.TaxableAmount = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["TaxableAmount"]);
                CRPDetailsList.Add(CRPDetails);
            }

            return CRPDetailsList;
        }
        protected string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {
            //oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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

                    sqlQuery = "SELECT max(tjv.ReceiptPayment_VoucherNumber) FROM Trans_CustomerReceiptPayment tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    // sqlQuery += "?$', LTRIM(RTRIM(tjv.ReceiptPayment_VoucherNumber))) = 1";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.ReceiptPayment_VoucherNumber))) = 1 and ReceiptPayment_VoucherNumber like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, ReceiptPayment_CreateDateTime) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.ReceiptPayment_VoucherNumber) FROM Trans_CustomerReceiptPayment tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        // sqlQuery += "?$', LTRIM(RTRIM(tjv.ReceiptPayment_VoucherNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.ReceiptPayment_VoucherNumber))) = 1 and ReceiptPayment_VoucherNumber like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, ReceiptPayment_CreateDateTime) = CONVERT(DATE, GETDATE())";
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
                    sqlQuery = "SELECT ReceiptPayment_VoucherNumber FROM Trans_CustomerReceiptPayment WHERE ReceiptPayment_VoucherNumber LIKE '" + manual_str.Trim() + "'";
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
        protected void gridBatch_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            gridBatch.JSProperties["cpVouvherNo"] = "";
            gridBatch.JSProperties["cpSaveSuccessOrFail"] = null;
            DataTable CustomerPayRecdt = new DataTable();
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
            string VoucherType = "R";
            if (Session["AdvanceReceiptDetails"] != null)
            {
                CustomerPayRecdt = (DataTable)Session["AdvanceReceiptDetails"];
            }
            else
            {
                CustomerPayRecdt.Columns.Add("SrlNo", typeof(string));
                CustomerPayRecdt.Columns.Add("ReceiptDetail_ID", typeof(string));
                CustomerPayRecdt.Columns.Add("Type", typeof(string));
                CustomerPayRecdt.Columns.Add("DocumentID", typeof(string));
                CustomerPayRecdt.Columns.Add("Payment", typeof(string));
                CustomerPayRecdt.Columns.Add("Receipt", typeof(string));
                CustomerPayRecdt.Columns.Add("Remarks", typeof(string));
                CustomerPayRecdt.Columns.Add("Status", typeof(string));
                CustomerPayRecdt.Columns.Add("DocumentNo", typeof(string));
                CustomerPayRecdt.Columns.Add("IsOpening", typeof(string));
                CustomerPayRecdt.Columns.Add("ProductID", typeof(string));
                CustomerPayRecdt.Columns.Add("ProductName", typeof(string));
                CustomerPayRecdt.Columns.Add("TaxAmount", typeof(string));
                CustomerPayRecdt.Columns.Add("NetAmount", typeof(string));
                CustomerPayRecdt.Columns.Add("Quantity", typeof(string));
                CustomerPayRecdt.Columns.Add("TaxableAmount", typeof(string));

            }
            foreach (var args in e.InsertValues)
            {
                string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                //string Type = Convert.ToString(args.NewValues["Type"]);
                string Type = "Advance";
                string Recieve = Convert.ToString(args.NewValues["Receipt"]);
                string Payment = Convert.ToString(args.NewValues["Payment"]);
                string IsOpening = Convert.ToString(args.NewValues["IsOpening"]);
                string ProductID = Convert.ToString(args.NewValues["ProductID"]);
                string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                string DocumentID = Convert.ToString(args.NewValues["DocumentID"]);
                string Remarks = Convert.ToString(args.NewValues["Remarks"]);
                string TaxAmount = Convert.ToString(args.NewValues["TaxAmount"]);
                string NetAmount = Convert.ToString(args.NewValues["NetAmount"]);
                string Quantity = Convert.ToString(args.NewValues["Quantity"]);
                string TaxableAmount = Convert.ToString(args.NewValues["TaxableAmount"]);

                if (ProductID != "")
                {
                    CustomerPayRecdt.Rows.Add(SrlNo, "0", Type, DocumentID, Payment, Recieve, Remarks, "I", "", IsOpening, ProductID, ProductName, TaxAmount, NetAmount, Quantity, TaxableAmount);
                }
            }
            foreach (var args in e.UpdateValues)
            {
                string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                string ReceiptDetail_ID = Convert.ToString(args.Keys["ReceiptDetail_ID"]);
                string Type = "Advance";
                string Recieve = Convert.ToString(args.NewValues["Receipt"]);
                string Payment = Convert.ToString(args.NewValues["Payment"]);
                string IsOpening = Convert.ToString(args.NewValues["IsOpening"]);
                string ProductID = Convert.ToString(args.NewValues["ProductID"]);
                string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                string DocumentID = Convert.ToString(args.NewValues["DocumentID"]);
                string Remarks = Convert.ToString(args.NewValues["Remarks"]);
                string TaxAmount = Convert.ToString(args.NewValues["TaxAmount"]);
                string NetAmount = Convert.ToString(args.NewValues["NetAmount"]);
                string Quantity = Convert.ToString(args.NewValues["Quantity"]);
                string TaxableAmount = Convert.ToString(args.NewValues["TaxableAmount"]);

                bool isDeleted = false;
                foreach (var arg in e.DeleteValues)
                {
                    string DeleteID = Convert.ToString(arg.Keys["ReceiptDetail_ID"]);
                    if (DeleteID == ReceiptDetail_ID)
                    {
                        isDeleted = true;
                        break;
                    }
                }

                if (isDeleted == false)
                {
                    if ((Recieve != "0.0") || (Payment != "0.0"))
                    {
                        DataRow drr = CustomerPayRecdt.Select("ReceiptDetail_ID ='" + ReceiptDetail_ID + "'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                        if (drr != null)
                        {

                            drr["Type"] = Type;
                            drr["DocumentID"] = DocumentID;
                            drr["Payment"] = Payment;
                            drr["Receipt"] = Recieve;
                            drr["Remarks"] = Remarks;
                            drr["Status"] = "U";
                            drr["DocumentNo"] = " ";
                            drr["IsOpening"] = IsOpening;
                            drr["ProductID"] = ProductID;
                            drr["TaxAmount"] = TaxAmount;
                            drr["NetAmount"] = NetAmount;
                            drr["Quantity"] = Quantity;
                            drr["TaxableAmount"] = TaxableAmount;
                        }
                    }

                    //}
                }
            }
            foreach (var args in e.DeleteValues)
            {
                string ReceiptDetail_ID = Convert.ToString(args.Keys["ReceiptDetail_ID"]);
                string SrlNo = "";

                DataRow dr = CustomerPayRecdt.Select("ReceiptDetail_ID ='" + ReceiptDetail_ID + "'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                if (dr != null)
                {
                    dr["Status"] = "D";
                    SrlNo = Convert.ToString(dr["SrlNo"]);
                    DeleteTaxDetails(SrlNo);
                }
            }
            //int j = 1;
            //foreach (DataRow dr in CustomerPayRecdt.Rows)
            //{
            //    string Status = Convert.ToString(dr["Status"]);
            //    dr["ReceiptDetail_ID"] = j.ToString();

            //    if (Status != "D")
            //    {
            //        if (Status == "I")
            //        {
            //            string strID = Convert.ToString("Q~" + j);
            //            dr["ReceiptDetail_ID"] = strID;
            //        }
            //        j++;
            //    }
            //}
            int j = 1;
            foreach (DataRow dr in CustomerPayRecdt.Rows)
            {
                string Status = Convert.ToString(dr["Status"]);
                string oldSrlNo = Convert.ToString(dr["SrlNo"]);
                string newSrlNo = j.ToString();

                dr["SrlNo"] = j.ToString();

                UpdateTaxDetails(oldSrlNo, newSrlNo);

                if (Status != "D")
                {
                    if (Status == "I")
                    {
                        string strID = Convert.ToString("Q~" + j);
                        dr["ReceiptDetail_ID"] = strID;
                    }
                    j++;
                }
            }

            CustomerPayRecdt.AcceptChanges();
            Session["AdvanceReceiptDetails"] = CustomerPayRecdt;
            
            if (IsDeleteFrom != "D" && IsDeleteFrom != "C")
            {
                #region Save Zone Start
                string validate = "";

                if (hdn_Mode.Value == "Entry")
                {
                    validate = checkNMakeJVCode(Convert.ToString(txtVoucherNo.Text), Convert.ToInt32(CmbScheme.Value));
                }
                foreach (DataRow dr in CustomerPayRecdt.Rows)
                {
                    string DocumentID = Convert.ToString(dr["DocumentID"]);
                    string strType = Convert.ToString(dr["Type"]).Trim();
                    string Status = Convert.ToString(dr["Status"]);
                    string Recieve = Convert.ToString(dr["Receipt"]);
                    string Payment = Convert.ToString(dr["Payment"]);
                    if (Status != "D")
                    {
                        if (DocumentID == "" && strType != "Advance")
                        {
                            validate = "nullQuantity";
                            break;
                        }

                        if (Convert.ToDecimal(Recieve) == 0 && Convert.ToDecimal(Payment) == 0)
                        {
                            validate = "nullReceiptPayment";
                            break;
                        }
                    }
                }
                #region ##### Added By : Samrat Roy -- to get BillingShipping user control data
                DataTable tempBillAddress = new DataTable();
                tempBillAddress = BillingShippingControl.SaveBillingShippingControlData();
                #endregion

                String Product_IDS = "";
                #region BS mandatory

                if (tempBillAddress == null || tempBillAddress.Rows.Count == 0)
                {
                    validate = "BSMandatory";
                }

                #endregion
                if (validate == "outrange" || validate == "duplicate" || validate == "nullQuantity" || validate == "nullReceiptPayment" || validate == "BSMandatory")
                {
                    gridBatch.JSProperties["cpSaveSuccessOrFail"] = validate;
                }
                else
                {
                    string ActionType = "", strIBRef = "";
                    if (hdn_Mode.Value == "Entry")
                    {

                        ActionType = "ADD";
                        strIBRef = "";

                    }
                    else
                    {
                        ActionType = "Edit";
                        strIBRef = Convert.ToString(Session["IBRef"]);
                    }

                    string strEditCashBankID = Convert.ToString(Session["AdvanceReceipt_ID"]);

                    string strCustomer = Convert.ToString(hdfLookupCustomer.Value);
                    string strContactName = Convert.ToString(cmbContactPerson.Value);
                    if (strContactName == "")
                    {
                        strContactName = "0";
                    }
                    string strCashBankBranchID = Convert.ToString(ddlBranch.SelectedValue);
                    string strTransactionDate = Convert.ToString(dtTDate.Date);
                    string paymenttype = Convert.ToString(rdl_MultipleType.SelectedValue);
                    string strCashBankID = "";
                    string strInstrumentType = "";
                    if (paymenttype == "S")
                    {
                        strCashBankID = Convert.ToString(ddlCashBank.Value);
                        strInstrumentType = Convert.ToString(cmbInstrumentType.Value);
                    }
                    else
                    {
                        strCashBankID = "0";
                        strInstrumentType = "";
                    }

                    string strExchangeSegmentID = "1";
                    string strTransactionType = "R";
                    string strEntryUserProfile = "F";
                    string strNarration = txtNarration.Text;
                    string strCurrency = Convert.ToString(CmbCurrency.Value);

                    string strVoucherAmount = txtVoucherAmount.Text.Trim();

                    string strInstrumentNumber = txtInstNobth.Text.Trim();
                    string strInstrumentDate = (InstDate.Date.ToString("yyyy-MM-dd") == "0001-01-01") ? " " : InstDate.Date.ToString("yyyy-MM-dd");
                    string strrate = txtRate.Text.Trim();
                    int Tax_Option = Convert.ToInt32(ddl_AmountAre.Value);

                    DataTable tempCustomerPayRecdtn = CustomerPayRecdt.Copy();
                    foreach (DataRow dr in tempCustomerPayRecdtn.Rows)
                    {
                        string Status = Convert.ToString(dr["Status"]);

                        if (Status == "I")
                        {
                            dr["ReceiptDetail_ID"] = "0";

                        }
                        else if (Status == "U" || Status == "")
                        {
                            string ReceiptDetailID = Convert.ToString(dr["ReceiptDetail_ID"]);
                            if (ReceiptDetailID.Contains("~") == true)
                            {
                                dr["ReceiptDetail_ID"] = "0";
                            }
                        }
                    }
                    tempCustomerPayRecdtn.AcceptChanges();

                    string TaxType = "", ShippingState = "";
                    ShippingState = Convert.ToString(BillingShippingControl.GetShippingStateCode());

                    if (ddl_AmountAre.Value == "1") TaxType = "E";
                    else if (ddl_AmountAre.Value == "2") TaxType = "I";

                    DataTable TaxDetailTable = new DataTable();
                    if (Session["CRPADV_FinalTaxRecord"] != null)
                    {
                        TaxDetailTable = (DataTable)Session["CRPADV_FinalTaxRecord"];
                    }
                    //TaxDetailTable = _ObjGSTtaxDetails.SetTaxTableDataWithProductSerialRoundOff(ref tempCustomerPayRecdtn, "SrlNo", "ProductID", "Amount", "TaxAmount", TaxDetailTable, "S", dtTDate.Date.ToString("yyyy-MM-dd"), strCashBankBranchID, ShippingState, TaxType);


                    if (Save_Record(ActionType, strEditCashBankID, JVNumStr, strCashBankBranchID, strTransactionDate, strCashBankID, strExchangeSegmentID, strTransactionType,
                        strEntryUserProfile, strVoucherAmount, strCustomer, strContactName,
                         strNarration, strIBRef, strCurrency, strInstrumentType, strInstrumentNumber, strInstrumentDate, strrate, Product_IDS, tempCustomerPayRecdtn, tempBillAddress, TaxDetailTable, Tax_Option) == true)
                    {

                        Session["IBRef"] = null;
                        // hdnEditRfid.Value = "";
                        hdndocumentno.Value = "";
                        hdn_Mode.Value = "";
                        txtVoucherAmount.Text = "0.0";
                        txtTotalAmount.Text = "0.0";
                        txtTotalPayment.Text = "0.0";

                        gridBatch.JSProperties["cpSaveSuccessOrFail"] = "successInsert";
                        gridBatch.JSProperties["cpVouvherNo"] = JVNumStr;
                        if (Session["AdvanceReceiptDetails"] != null)
                        {
                            Session["AdvanceReceiptDetails"] = null;
                        }

                        #region To Show By Default Cursor after SAVE AND NEW
                        if (ActionType == "ADD") // session has been removed from quotation list page working good
                        {
                            //string[] schemaid = new string[] { };
                            string schemavalue = Convert.ToString(CmbScheme.Value);
                            Session["schemavalueCRP"] = schemavalue;        // session has been removed from quotation list page working good
                            //schemaid = ddl_numberingScheme.SelectedValue.Split('~');

                            //string schematype = schemaid[1];
                            if (hdnRefreshType.Value == "N")
                            {
                                string schematype = txtVoucherNo.Text.Trim();
                                if (schematype == "Auto")
                                {
                                    Session["SaveModeCRP"] = "A";
                                }
                                else
                                {
                                    Session["SaveModeCRP"] = "M";
                                }
                            }
                            else
                            {
                                Session["SaveModeCRP"] = null;
                            }
                        }

                        if (Request.QueryString["basketId"] != null)
                        {
                            string basketId = Request.QueryString["basketId"];
                            DeleteFormbasketTable(basketId);
                        }
                        #endregion
                    }
                    else
                    {
                        gridBatch.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                    }
                }
                #endregion
            }
            else
            {
                DataView dvData = new DataView(CustomerPayRecdt);
                dvData.RowFilter = "Status <> 'D'";

                gridBatch.DataSource = BindCustomerReceiptPaymentBatch(dvData.ToTable());
                gridBatch.DataBind();
            }
            
        }
        public DataTable GetDocumentDetails(string TypeID, String VoucherType, string receiptdate, string strProduct)
        {
            //string ReceiptPayment_ID = Convert.ToString(Session["AdvanceReceipt_ID"]);

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_getDocumentDetails");
            proc.AddVarcharPara("@DocumentType", 20, TypeID);
            proc.AddVarcharPara("@DiscardIds", 100, hdndocumentno.Value);
            proc.AddVarcharPara("@VoucherType", 20, VoucherType);
            proc.AddVarcharPara("@Receiptdate", 50, Convert.ToString(receiptdate));
            proc.AddVarcharPara("@IsProduct", 50, Convert.ToString(strProduct));
            proc.AddVarcharPara("@CustomerId", 250, Convert.ToString(hdfLookupCustomer.Value));
            proc.AddVarcharPara("@BranchId", 250, Convert.ToString(ddlBranch.SelectedValue));

            dt = proc.GetTable();
            return dt;
        }
        protected void Page_Init(object sender, EventArgs e)
        {

            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCurrencyBind.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ProductDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            PaymentDetails.doc_type = "CRP";
            if (Request.QueryString["key"] != "ADD")
            {
                PaymentDetails.StorePaymentDetailsToHiddenfield(Convert.ToInt32(Request.QueryString["key"]));
            }

            if (!IsPostBack)
            {
                gridBatch.DataBind();
            }
        }
        private bool Save_Record(string ActionType, string strEditCashBankID, string strVoucherNumber, string strCashBankBranchID, string strTransactionDate,
            string strCashBankID,
             string strExchangeSegmentID, string strTransactionType, string strEntryUserProfile, string strVoucherAmount, string strCustomer, string strContactName,
            string strNarration, string strIBRef,
          string strCurrency, string strInstrumentType, string strInstrumentNumber, string strInstrumentDate, string strrate, string Product_IDS,
          DataTable strReceiptPaymentdt, DataTable tempBillAddress, DataTable TaxDetailTable, int Tax_Option)
        {
            try
            {
                gridBatch.JSProperties["cpExitNew"] = null;

                if (hdn_Mode.Value == "Entry")
                {

                    strIBRef = "CPR_" + Session["userid"].ToString() + "_" + strTransactionType + "_" + JVNumStr.Replace("/", "");

                }
                DataSet dsInst = new DataSet();

                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_CustomerAdvanceReceiptInsertUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", ActionType);//1
                cmd.Parameters.AddWithValue("@EditReceiptPaymentID", strEditCashBankID);//2
                cmd.Parameters.AddWithValue("@VoucherNumber", strVoucherNumber);//3

                cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));//4
                cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(Session["LastFinYear"]));//5
                cmd.Parameters.AddWithValue("@CreateUser", Convert.ToString(Session["userid"]));//6

                cmd.Parameters.AddWithValue("@ReceiptPaymentBranchID", strCashBankBranchID);//7
                cmd.Parameters.AddWithValue("@TransactionDate", strTransactionDate);//8

                cmd.Parameters.AddWithValue("@ReceiptPaymentCashBankID", strCashBankID);//9

                cmd.Parameters.AddWithValue("@ExchangeSegmentID", strExchangeSegmentID);//10
                cmd.Parameters.AddWithValue("@TransactionType", strTransactionType);//11


                cmd.Parameters.AddWithValue("@Narration", strNarration);//12
                cmd.Parameters.AddWithValue("@IBRef", strIBRef);//13
                cmd.Parameters.AddWithValue("@CurrencyID", strCurrency);//14
                cmd.Parameters.AddWithValue("@rate", strrate);         //15


                cmd.Parameters.AddWithValue("@ReceiptPaymentInstrumentType", strInstrumentType);//16
                cmd.Parameters.AddWithValue("@ReceiptPaymentInstrumentNumber", strInstrumentNumber);//17
                cmd.Parameters.AddWithValue("@ReceiptPaymentInstrumentDate", strInstrumentDate);//18

                cmd.Parameters.AddWithValue("@ReceiptPaymentCustomerID", strCustomer);//19
                cmd.Parameters.AddWithValue("@ReceiptPaymentContactPersonID", strContactName);//20
                cmd.Parameters.AddWithValue("@ReceiptPaymentVoucherAmount", strVoucherAmount);//21

                cmd.Parameters.AddWithValue("@Product_IDS", Product_IDS);//22

                cmd.Parameters.AddWithValue("@ReceiptPaymentDetails", strReceiptPaymentdt);//23

                string paymenttype = rdl_MultipleType.SelectedValue;
                DataTable dtMultiType;
                if (paymenttype != "S")
                {
                    dtMultiType = PaymentDetails.GetPaymentTable();

                }
                else
                {
                    dtMultiType = CreatePaymentDataTable();
                }

                cmd.Parameters.AddWithValue("@PaymentType", paymenttype);//24
                cmd.Parameters.AddWithValue("@paymentDetails", dtMultiType);//25
                cmd.Parameters.AddWithValue("@BillAddress", tempBillAddress); //26
                cmd.Parameters.AddWithValue("@TaxDetail", TaxDetailTable); //27
                cmd.Parameters.AddWithValue("@ReceiptPayment_Tax_Option", Tax_Option); //28

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
                    Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("CRP", "CustrRecePay" + Convert.ToString(output.Value), udfTable, Convert.ToString(Session["userid"]));
                }
                int strCPRID = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                if (strCPRID > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

                //  gridBatch.JSProperties["cpExitNew"] = "YES";


            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public DataTable CreatePaymentDataTable()
        {
            DataTable paymentDetails = new DataTable();
            paymentDetails.Columns.Add("Doc_type", typeof(System.String));
            paymentDetails.Columns.Add("Payment_type", typeof(System.String));
            paymentDetails.Columns.Add("PaymentType_details", typeof(System.String));
            paymentDetails.Columns.Add("cardType", typeof(System.String));
            paymentDetails.Columns.Add("AuthNo", typeof(System.String));
            paymentDetails.Columns.Add("payment_remarks", typeof(System.String));
            paymentDetails.Columns.Add("paymentAmount", typeof(System.String));
            paymentDetails.Columns.Add("payment_date", typeof(System.String));
            paymentDetails.Columns.Add("Drawee_date", typeof(System.String));
            paymentDetails.Columns.Add("Payment_mainAccount", typeof(System.String));

            return paymentDetails;
        }

        #endregion

        #region Tax
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
        public DataSet GetCRPTaxData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerAdvanceReceiptDetails");
            proc.AddVarcharPara("@Action", 500, "CRPEditedTaxDetails");
            proc.AddIntegerPara("@ReceiptPayment_Id", Convert.ToInt32(Session["AdvanceReceipt_ID"]));
            ds = proc.GetDataSet();
            return ds;
        }
        protected void cmbGstCstVat_Callback(object sender, CallbackEventArgsBase e)
        {
            DateTime quoteDate = Convert.ToDateTime(dtTDate.Date.ToString("yyyy-MM-dd"));

            PopulateGSTCSTVATCombo(quoteDate.ToString("yyyy-MM-dd"));
            CreateDataTaxTable();
            //DataTable taxTableItemLvl = (DataTable)Session["CRPADV_FinalTaxRecord"];
            //foreach (DataRow dr in taxTableItemLvl.Rows)
            //    dr.Delete();
            //taxTableItemLvl.AcceptChanges();
            //Session["CRPADV_FinalTaxRecord"] = taxTableItemLvl;
        }
        public void CreateDataTaxTable()
        {
            DataTable TaxRecord = new DataTable();

            TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
            TaxRecord.Columns.Add("TaxCode", typeof(System.String));
            TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
            TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
            TaxRecord.Columns.Add("Amount", typeof(System.Decimal));
            Session["CRPADV_FinalTaxRecord"] = TaxRecord;
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
            proc.AddIntegerPara("@branchId", Convert.ToInt32(Session["userbranchID"]));
            DataTable DT = proc.GetTable();
            cmbGstCstVat.DataSource = DT;
            cmbGstCstVat.TextField = "Taxes_Name";
            cmbGstCstVat.ValueField = "Taxes_ID";
            cmbGstCstVat.DataBind();
        }
        protected void taxgrid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {

            int slNo = Convert.ToInt32(HdSerialNo.Value);
            DataTable TaxRecord = (DataTable)Session["CRPADV_FinalTaxRecord"];
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


            Session["CRPADV_FinalTaxRecord"] = TaxRecord;


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
        protected void cgridTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string retMsg = "";
            string taxType = ddl_AmountAre.Value.ToString();
            if (e.Parameters.Split('~')[0] == "SaveGST")
            {
                DataTable TaxRecord = (DataTable)Session["CRPADV_FinalTaxRecord"];
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

                Session["CRPADV_FinalTaxRecord"] = TaxRecord;
            }
            else
            {
                #region fetch All data For Tax

                DataTable taxDetail = new DataTable();
                DataTable MainTaxDataTable = (DataTable)Session["CRPADV_FinalTaxRecord"];
                DataTable databaseReturnTable = (DataTable)Session["SI_QuotationTaxDetails"];

                //if (Convert.ToInt32(e.Parameters.Split('~')[1]) == 1)
                //    taxDetail = oDBEngine.GetDataTable("select TaxRates_ID as Taxes_ID,TaxRatesSchemeName as Taxes_Name,TaxCalculateMethods,tm.Taxes_Name as taxCodeName,tm.Taxes_ApplicableOn as 'ApplicableOn',isnull((select TaxRatesSchemeName from Config_TaxRates where TaxRates_ID=tm.Taxes_OtherTax),'')  as dependOn from Master_taxes tm inner join Config_TaxRates ct on tm.Taxes_ID=ct.TaxRates_TaxCode where tm.TaxTypeCode not in('G','V','C') and tm.TaxItemlevel='Y'");
                //else if (Convert.ToInt32(e.Parameters.Split('~')[1]) == 2)
                //taxDetail = oDBEngine.GetDataTable("select TaxRates_ID as Taxes_ID,TaxRatesSchemeName as Taxes_Name,TaxCalculateMethods,tm.Taxes_Name as taxCodeName,tm.Taxes_ApplicableOn as 'ApplicableOn',isnull((select TaxRatesSchemeName from Config_TaxRates where TaxRates_ID=tm.Taxes_OtherTax),'')  as dependOn from Master_taxes tm inner join Config_TaxRates ct on tm.Taxes_ID=ct.TaxRates_TaxCode where tm.TaxTypeCode not in('G','V','C') and tm.TaxItemlevel='Y'");

                ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
                proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                proc.AddVarcharPara("@ProductID", 10, Convert.ToString(setCurrentProdCode.Value));
                proc.AddVarcharPara("@S_quoteDate", 10, dtTDate.Date.ToString("yyyy-MM-dd"));
                taxDetail = proc.GetTable();

                //Get Company Gstin 09032017
                string CompInternalId = Convert.ToString(Session["LastCompany"]);
                string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);

                //Get BranchStateCode
                string BranchStateCode = "", BranchGSTIN = "";
                DataTable BranchTable = oDBEngine.GetDataTable("select StateCode,branch_GSTIN   from tbl_master_branch branch inner join tbl_master_state st on branch.branch_state=st.id where branch_id=" + Convert.ToString(ddlBranch.SelectedValue));
                if (BranchTable != null)
                {
                    BranchStateCode = Convert.ToString(BranchTable.Rows[0][0]);
                    BranchGSTIN = Convert.ToString(BranchTable.Rows[0][1]);
                }


                string ShippingState = "";

                #region ##### Added By : Samrat Roy -- For BillingShippingUserControl ######
                string sstateCode = BillingShippingControl.GetShippingStateCode(Request.QueryString["key"]);
                ShippingState = sstateCode;
                if (ShippingState.Trim() != "")
                {
                    ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
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



                if (ShippingState.Trim() != "" && BranchStateCode != "")
                {


                    if (BranchStateCode == ShippingState)
                    {
                        //Check if the state is in union territories then only UTGST will apply
                        //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU      Lakshadweep              PONDICHERRY
                        if (ShippingState == "4" || ShippingState == "26" || ShippingState == "25" || ShippingState == "35" || ShippingState == "31" || ShippingState == "34")
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

                //If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
                if (compGstin[0].Trim() == "" && BranchGSTIN == "")
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


                ////Check If any TaxScheme Set Against that Product Then update there rate 22-03-2017 and rate
                //string[] schemeIDViaProdID = oDBEngine.GetFieldValue1("master_sproducts", "isnull(sProduct_TaxSchemeSale,0)sProduct_TaxSchemeSale", "sProducts_ID='" + Convert.ToString(setCurrentProdCode.Value) + "'", 1);
                ////&& schemeIDViaProdID[0] != ""
                //if (schemeIDViaProdID.Length > 0)
                //{

                //    if (taxDetail.Select("Taxes_ID='" + schemeIDViaProdID[0] + "'").Length > 0)
                //    {
                //        foreach (DataRow dr in taxDetail.Rows)
                //        {
                //            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                //            {
                //                if (Convert.ToString(dr["Taxes_ID"]).Trim() != schemeIDViaProdID[0].Trim())
                //                    dr["TaxRates_Rate"] = 0;
                //            }
                //        }
                //    }
                //}



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
                        if (Convert.ToString(taxType) == "2")
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                            {
                                decimal finalCalCulatedOn = 0;
                                decimal backProcessRate = (1 + (totalParcentage / 100));
                                finalCalCulatedOn = obj.calCulatedOn / backProcessRate;
                                obj.calCulatedOn = finalCalCulatedOn;
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

                    DataTable TaxRecord = (DataTable)Session["CRPADV_FinalTaxRecord"];


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
                        if (Convert.ToString(taxType) == "2")
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                            {
                                decimal finalCalCulatedOn = 0;
                                decimal backProcessRate = (1 + (totalParcentage / 100));
                                finalCalCulatedOn = obj.calCulatedOn / backProcessRate;
                                obj.calCulatedOn = finalCalCulatedOn;
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


                            //DataRow[] filtr = databaseReturnTable.Select("ProductTax_ProductId ='" + keyValue + "' and ProductTax_QuoteId=" +Convert.ToString( Session["SI_InvoiceID"] )+ " and ProductTax_TaxTypeId=" + obj.Taxes_ID);
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

                        //      DataRow[] filtrIndex = databaseReturnTable.Select("ProductTax_ProductId ='" + keyValue + "' and ProductTax_QuoteId=" + Session["SI_InvoiceID"] + " and ProductTax_TaxTypeId=0");
                        DataRow[] filtrIndex = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode=0");
                        if (filtrIndex.Length > 0)
                        {
                            aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(filtrIndex[0]["AltTaxCode"]);
                        }
                    }
                    Session["CRPADV_FinalTaxRecord"] = TaxRecord;

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
        protected void aspxGridTax_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {

            if (e.Column.FieldName == "Taxes_Name")
            {
                e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "taxCodeName")
            {
                e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "calCulatedOn")
            {
                e.Editor.ReadOnly = true;
            }
            //else if (e.Column.FieldName == "Percentage")
            //{
            //    e.Editor.ReadOnly = false;
            //}
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
        public void DeleteTaxDetails(string SrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["CRPADV_FinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["CRPADV_FinalTaxRecord"];

                var rows = TaxDetailTable.Select("SlNo ='" + SrlNo + "'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                TaxDetailTable.AcceptChanges();

                Session["CRPADV_FinalTaxRecord"] = TaxDetailTable;
            }
        }
        public void UpdateTaxDetails(string oldSrlNo, string newSrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["CRPADV_FinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["CRPADV_FinalTaxRecord"];

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

                Session["CRPADV_FinalTaxRecord"] = TaxDetailTable;
            }
        }
        #endregion

        #region Events
        public IEnumerable BindInvoiceReceiptPaymentBatch(string strInvoiceId)
        {
            List<CRPDetailsLIST> CRPDetailsList = new List<CRPDetailsLIST>();
            DataTable CustomerReceiptPaymendt = GetCustomerReceiptPaymentBatchData_ByInvoice(strInvoiceId).Tables[0];

            if (CustomerReceiptPaymendt != null)
            {
                for (int i = 0; i < CustomerReceiptPaymendt.Rows.Count; i++)
                {
                    CRPDetailsLIST CRPDetails = new CRPDetailsLIST();

                    CRPDetails.ReceiptDetail_ID = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["ReceiptDetail_ID"]);
                    CRPDetails.Type = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Type"]);
                    CRPDetails.DocumentID = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["DocumentID"]);
                    CRPDetails.Receipt = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Receipt"]);
                    CRPDetails.Payment = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Payment"]);
                    CRPDetails.Remarks = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Remarks"]);
                    CRPDetails.DocumentNo = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["DocumentNo"]);
                    CRPDetailsList.Add(CRPDetails);
                }
            }

            return CRPDetailsList;
        }
        public void DeleteFormbasketTable(string basketId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_posListingDetails");
            proc.AddVarcharPara("@Action", 500, "DeleteFromBasketAdvance");
            proc.AddIntegerPara("@basketId", Convert.ToInt32(basketId));
            proc.AddIntegerPara("@UserID", Convert.ToInt32(Session["userid"]));
            proc.RunActionQuery();
        }
        public void SetBasketDetails(string Id)
        {
            isFromTab.Value = "1";
            DataTable dt = new DataTable();
            DataSet basketDS = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_posListingDetails");
            proc.AddVarcharPara("@Action", 500, "GetAdvanceBasketDetails");
            proc.AddIntegerPara("@basketId", Convert.ToInt32(Id));
            basketDS = proc.GetDataSet();

            string customerId = "", branch = "";
            decimal amount = 0;

            dt = basketDS.Tables[0];

            DataTable ProductBasketDetail = basketDS.Tables[1];

            if (dt != null)
            {
                customerId = Convert.ToString(dt.Rows[0]["CustomerId"]);
                amount = Convert.ToDecimal(dt.Rows[0]["Amount"]);
                HdBasketTotalAmount.Value = Convert.ToString(dt.Rows[0]["Amount"]);
                branch = Convert.ToString(dt.Rows[0]["user_branchId"]);
            }

            DataTable Schematable = oDBEngine.GetDataTable("select Id,Branch,startno  from  tbl_master_idschema");

            for (int i = 1; i < CmbScheme.Items.Count; i++)
            {
                string scheme = Convert.ToString(CmbScheme.Items[i].Value);

                DataRow[] row = Schematable.Select("Id=" + scheme);
                if (row.Length > 0)
                {
                    if (Convert.ToString(row[0]["Branch"]) == branch && Convert.ToString(row[0]["startno"]) == "1")
                    {
                        CmbScheme.Value = scheme;
                        txtVoucherNo.Text = "Auto";
                        txtVoucherNo.Enabled = false;
                        ddlBranch.SelectedValue = branch;
                        break;
                    }
                }


            }
            lookup_Customer.DataBind();
            lookup_Customer.GridView.Selection.SelectRowByKey(customerId);
            hdnCustomerId.Value = customerId;

            string ProductID = string.Empty;
        }
        private DataSet BindInclusiveExclusiveDropdown(string userbranch)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForSalesOrder");
            proc.AddVarcharPara("@userbranch", 4000, userbranch);
            ds = proc.GetDataSet();
            return ds;
        }
        protected void acbpCrpUdf_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (Request.QueryString["key"] == "ADD")
            {
                if (reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "CRP") == false)
                {
                    acbpCrpUdf.JSProperties["cpUDF"] = "false";

                }
                else
                {
                    acbpCrpUdf.JSProperties["cpUDF"] = "true";
                }
            }
            else
            {
                acbpCrpUdf.JSProperties["cpUDF"] = "true";
            }
        }
        protected void acpCheckAmount_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            DataTable dtAmount = new DataTable();
            string strType = e.Parameter.Split('~')[0];
            string strDocumentId = e.Parameter.Split('~')[1];
            string strIsOpening = e.Parameter.Split('~')[2];
            if (Request.QueryString["key"] == "ADD")
            {
                dtAmount = objCustomerVendorReceiptPaymentBL.GetExactAmount(strType, strDocumentId, strIsOpening);
                string UnPaidAmount = string.Empty;
                if (dtAmount != null && dtAmount.Rows.Count > 0)
                {
                    UnPaidAmount = Convert.ToString(dtAmount.Rows[0]["UnPaidAmount"]);
                    acpCheckAmount.JSProperties["cpUnPaidAmount"] = UnPaidAmount;
                }
            }
            else
            {
                string ReceiptPayment_ID = Request.QueryString["key"];
                dtAmount = objCustomerVendorReceiptPaymentBL.GetExactAmountForEdit(strType, strDocumentId, strIsOpening, ReceiptPayment_ID);
                string UnPaidAmount = string.Empty;
                if (dtAmount != null && dtAmount.Rows.Count > 0)
                {
                    UnPaidAmount = Convert.ToString(dtAmount.Rows[0]["UnPaidAmount"]);
                    acpCheckAmount.JSProperties["cpUnPaidAmount"] = UnPaidAmount;
                }
            }

        }
        protected void ddlCashBank_Callback(object sender, CallbackEventArgsBase e)
        {
            string userbranch = e.Parameter.Split('~')[0];
            BindCashBankAccount(userbranch);
        }

        #endregion

    }
}