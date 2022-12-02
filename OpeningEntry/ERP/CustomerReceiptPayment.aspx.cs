using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DevExpress.Web;
using System.Linq;
using BusinessLogicLayer;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections;
using DataAccessLayer;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;

namespace OpeningEntry.ERP
{
    public partial class CustomerReceiptPayment : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        CustomerVendorReceiptPaymentBL objCustomerVendorReceiptPaymentBL = new CustomerVendorReceiptPaymentBL();

        public string pageAccess = "";
        string JVNumStr = string.Empty;
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

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/ERP/CustomerReceiptPayment.aspx");
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCurrencyBind.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                //Sudip pal
                GetFinancialYearstartandEnddate();
                //Sudip pal
                PaymentDetails.doc_type = "CRP";
                Session["CustomerDetailForCRP"] = null;
                Session["ReceiptPayment_ID"] = null;
                Session["IBRef"] = null;
                Session["ReceiptPaymentDetails"] = null;
               // ComboVoucherType.SelectedValue = "R";
                IsUdfpresent.Value = Convert.ToString(getUdfCount());
                BindBranch();
                Bind_Currency();
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
                    //if(CmbCurrency.Text.Trim()=="INR")
                    //{
                    //    txtRate.Enabled = false;
                    //}
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
                    hdnPageStatus.Value = "first";
                    hdn_Mode.Value = "Entry";


                    //Sudip Pal
                    //  dtTDate.Date = DateTime.Now;
                    //Sudip Pal

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
                       // CmbScheme.Focus();
                        ComboVoucherType.Focus();
                    }
                    #endregion To Show By Default Cursor after SAVE AND NEW
                    gridBatch.DataSource = null;
                    gridBatch.DataBind();
                    Keyval_internalId.Value = "Add";
                }
                else
                {
                    gridBatch.JSProperties["cpView"] = (Request.QueryString["req"] != null && Request.QueryString["req"] == "V") ? "1" : "0";
                    hdnPageStatus.Value = "update";
                    hdn_Mode.Value = "Edit";
                    txtVoucherNo.Enabled = false;
                    ComboVoucherType.Enabled = false;
                   // dtTDate.Enabled = false;
                   // lookup_Customer.Enabled = false;
                    divNumberingScheme.Style.Add("display", "none");
                    if (Request.QueryString["req"] == "V")
                    {
                        lblHeadTitle.Text = "View Customer Payment/Receipt";
                    }
                    else
                    {
                        lblHeadTitle.Text = "Modify Customer Receipt/Payment";
                    }
                   
                    Session["ReceiptPayment_ID"] = Request.QueryString["key"];
                    FillGrid();
                    GetDocumentType();
                    Session["ReceiptPaymentDetails"] = GetCustomerReceiptPaymentBatchData().Tables[0];
                    gridBatch.DataSource = BindCustomerReceiptPaymentBatch();
                    gridBatch.DataBind();
                    Keyval_internalId.Value = "CustrRecePay" + Session["ReceiptPayment_ID"];
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);
            }
            PopulateCustomerDetail();
            //if (CmbCurrency.Text.Trim() != "INR")
            //{
            //    txtRate.Enabled = true;
            //}
            Bind_ReceiptNumberingScheme();
        }
        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='CRP' and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }

        public class CRPDetailsLIST
        {
            public string ReceiptDetail_ID { get; set; }
            public string Type { get; set; }
            public string DocumentID { get; set; }
            public string Receipt { get; set; }
            public string Payment { get; set; }
            public string Remarks { get; set; }
            public string DocumentNo { get; set; }
        }
        public class DocumentType
        {
            public string Type { get; set; }
            public string ID { get; set; }
        }
        public void FillGrid()
        {
            DataTable CRPOrderEditdt = GetCustomerReceiptPaymentEditData();
            if (CRPOrderEditdt != null && CRPOrderEditdt.Rows.Count > 0)
            {
                string Type = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_TransactionType"]);//0
                if (Type=="P")
                {
                    multipleredio.Visible = false;
                }
               
                string VoucherNumber = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_VoucherNumber"]);//1
                string TransactionDate = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_TransactionDate"]);//2
                hdnDate.Value = TransactionDate;
                string BranchID = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_BranchID"]);//3

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

                Session["IBRef"] = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_IBRef"]);//15

                ComboVoucherType.SelectedValue= Type;
                if(Type=="P")
                {
                     GetDocumentDetails("DocumentType","P","");
                }
                else
                {
                    GetDocumentDetails("DocumentType", "","");
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
                PopulateContactPersonOfCustomer(Customer_Id);
                hdnCustomerId.Value = Customer_Id;
                if (Contact_Person_Id != "0")
                {
                    cmbContactPerson.Value = Contact_Person_Id;
                }
               
                ddlCashBank.Value = CashBankID;
                string strCashBank = ddlCashBank.Text;
                string Cash_Bank=Convert.ToString(strCashBank.Split(']')[1]);
                if (Cash_Bank.Trim()!="Bank")
                {
                    divInstrumentNo.Visible = false;
                    tdIDateDiv.Visible = false;
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


            }


        }
        public DataTable GetCustomerReceiptPaymentEditData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerReceiptPaymentDetails");
            proc.AddVarcharPara("@Action", 500, "CustomerReceiptPaymentEditDetails");
            proc.AddIntegerPara("@ReceiptPayment_ID", Convert.ToInt32(Session["ReceiptPayment_ID"]));
           
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


                CRPDetails.ReceiptDetail_ID = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["ReceiptDetail_ID"]);
                CRPDetails.Type = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Type"]);
                CRPDetails.DocumentID = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["DocumentID"]);
                CRPDetails.Receipt = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Receipt"]);
                CRPDetails.Payment = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Payment"]);
                CRPDetails.Remarks = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Remarks"]);
                CRPDetails.DocumentNo = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["DocumentNo"]);
                CRPDetailsList.Add(CRPDetails);
            }

            return CRPDetailsList;
        }
        public DataSet GetCustomerReceiptPaymentBatchData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerReceiptPaymentDetails");
            proc.AddVarcharPara("@Action", 500, "CRPBatchEditDetails");
            proc.AddIntegerPara("@ReceiptPayment_ID", Convert.ToInt32(Session["ReceiptPayment_ID"]));
            //proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(Session["LastCompany"]));
            ds = proc.GetDataSet();
            return ds;
        }
        #region Bind Method
        public void BindCashBankAccount(string userbranch)
        {
            DataTable dtCustomer = new DataTable();
            string CompanyId = Convert.ToString(Session["LastCompany"]);
            dtCustomer = objCustomerVendorReceiptPaymentBL.GetCustomerCashBank(userbranch, CompanyId);
            if (dtCustomer.Rows.Count>0)
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

        //Sudip Pal
        public void Bind_PaymentNumberingScheme()
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

           // SqlSchematype.SelectCommand = "Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='22' AND IsActive=1)) as x Order By ID asc";
            SqlSchematype.SelectCommand = "Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema " +
            "Where TYPE_ID='22' AND IsActive=1 AND Isnull(Branch,'')  in (select s FROM dbo.GetSplit(',','" + userbranchHierarchy + "'))" +
            "AND financial_year_id NOT IN (Select Top 1 FinYear_ID FROM Master_FinYear WHERE FinYear_Code='" + FinYear.Trim() +"')  AND Isnull(comapanyInt,'')='" + strCompanyID + "')) as x Order By ID asc";
            CmbScheme.DataBind();
        }
        public void Bind_ReceiptNumberingScheme()
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
           // SqlSchematype.SelectCommand = "Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID='21' AND IsActive=1)) as x Order By ID asc";
            SqlSchematype.SelectCommand = "Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName From tbl_master_Idschema " +
           "Where TYPE_ID='21' AND IsActive=1 AND Isnull(Branch,'') in (select s FROM dbo.GetSplit(',','" + userbranchHierarchy + "'))" +
           "AND financial_year_id NOT IN (Select Top 1 FinYear_ID FROM Master_FinYear WHERE FinYear_Code='" + FinYear.Trim() + "')  AND Isnull(comapanyInt,'')='" + strCompanyID + "')) as x Order By ID asc";
            CmbScheme.DataBind();
        }
        //Sudip Pal
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
     
        protected void cmbContactPerson_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindContactPerson")
            {
                string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                PopulateContactPersonOfCustomer(InternalId);
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

            string strschematype = "", strschemalength = "", strschemavalue = "";
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
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
        #endregion

        #region Batch Grid

        protected void CallbackPanelDocumentNo_Callback(object sender, CallbackEventArgsBase e)
        {
            DataTable dtDocument = new DataTable();
            string strType = e.Parameter.Split('~')[1];
            dtDocument = GetDocumentDetails(strType, "", Convert.ToString(dtTDate.Date));

            if (dtDocument != null && dtDocument.Rows.Count > 0)
            {
                documentLookUp.DataSource = dtDocument;
                documentLookUp.DataBind();
               
            }
            else
            {
                documentLookUp.DataSource = null;
                documentLookUp.DataBind();
                gridBatch.JSProperties["DocumentVisible"] = "true";
            }
        }
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
            if (Session["ReceiptPaymentDetails"] != null)
            {
                DataTable RPDdt = (DataTable)Session["ReceiptPaymentDetails"];
                DataView dvData = new DataView(RPDdt);             
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
               // total_payment += Convert.ToDecimal(obj.Payment);
            }

            //return Convert.ToString(total_receipt) + "~" + Convert.ToString(total_payment);
            return Convert.ToString(total_receipt);

        }
        private void bindMainAccount(object source, CallbackEventArgsBase e)
        {
            //FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            ASPxComboBox currentCombo = source as ASPxComboBox;
            currentCombo.DataSource = GetDocumentDetails("DocumentType",e.Parameter,Convert.ToString(dtTDate.Date));
            currentCombo.DataBind();
        }
        protected void gridBatch_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "Type")
            {

                ((ASPxComboBox)e.Editor).Callback += new CallbackEventHandlerBase(bindMainAccount);
            }
            e.Editor.ReadOnly = false;
            e.Editor.Enabled = true;
        }
        protected void gridBatch_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            
            if (strSplitCommand == "BindGridOnSalesInvoice")
            {
                string command = e.Parameters.ToString();               
                string ComponentDetailsIDs = string.Empty;
                string strAction = "";

                for (int i = 0; i < grid_SalesInvoice.GetSelectedFieldValues("DocumentID").Count; i++)
                {
                    ComponentDetailsIDs += "," + Convert.ToString(grid_SalesInvoice.GetSelectedFieldValues("DocumentID")[i]);
                }
                ComponentDetailsIDs = ComponentDetailsIDs.TrimStart(',');

                DataTable dtInvoices = new DataTable();
                dtInvoices = (DataTable)Session["AllInvoices"];
                var invoice = dtInvoices.Select("DocumentID in (" + ComponentDetailsIDs + ")").CopyToDataTable();
                if (invoice != null)
                {
                    Session["ReceiptPaymentDetails"] = invoice;
                     gridBatch.DataSource = BindCustomerReceiptPaymentBatch(invoice);
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


                CRPDetails.ReceiptDetail_ID = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["ReceiptDetail_ID"]) == "" ? Convert.ToString(i) : Convert.ToString(CustomerReceiptPaymendt.Rows[i]["ReceiptDetail_ID"]);
                CRPDetails.Type = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Type"]);
                CRPDetails.DocumentID = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["DocumentID"]);
                CRPDetails.Receipt = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Receipt"]);
                CRPDetails.Payment = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Payment"]);
                CRPDetails.Remarks = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["Remarks"]);
                CRPDetails.DocumentNo = Convert.ToString(CustomerReceiptPaymendt.Rows[i]["DocumentNo"]);
                CRPDetailsList.Add(CRPDetails);
            }

            return CRPDetailsList;
        } 
        protected string checkNMakeJVCode(string manual_str, int sel_schema_Id)
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

                    sqlQuery = "SELECT max(tjv.ReceiptPayment_VoucherNumber) FROM Trans_CustomerReceiptPayment tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.ReceiptPayment_VoucherNumber))) = 1";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, ReceiptPayment_CreateDateTime) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.ReceiptPayment_VoucherNumber) FROM Trans_CustomerReceiptPayment tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.ReceiptPayment_VoucherNumber))) = 1";
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
            string VoucherType = Convert.ToString(ComboVoucherType.SelectedValue);
            if (Session["ReceiptPaymentDetails"] != null)
            {
                CustomerPayRecdt = (DataTable)Session["ReceiptPaymentDetails"];
            }
            else
            {
                CustomerPayRecdt.Columns.Add("ReceiptDetail_ID", typeof(string));
                CustomerPayRecdt.Columns.Add("Type", typeof(string));
                CustomerPayRecdt.Columns.Add("DocumentID", typeof(string));
                CustomerPayRecdt.Columns.Add("Payment", typeof(string));
                CustomerPayRecdt.Columns.Add("Receipt", typeof(string));
                CustomerPayRecdt.Columns.Add("Remarks", typeof(string));
                CustomerPayRecdt.Columns.Add("Status", typeof(string));
                CustomerPayRecdt.Columns.Add("DocumentNo", typeof(string));

            }
            foreach (var args in e.InsertValues)
            {
                string Type = Convert.ToString(args.NewValues["Type"]);
                string Recieve = Convert.ToString(args.NewValues["Receipt"]);
                string Payment = Convert.ToString(args.NewValues["Payment"]);
                //if (MainAccount != "" && MainAccount != "0")
                //{
                    //if ((VoucherType=="R" && Recieve != "0.0") ||(VoucherType=="P" &&  Payment != "0.0"))
                    if ((Recieve != "0.0") || (Payment != "0.0"))
                    {
                        string DocumentID = Convert.ToString(args.NewValues["DocumentID"]);
                        string Remarks = Convert.ToString(args.NewValues["Remarks"]);

                        CustomerPayRecdt.Rows.Add("0", Type, DocumentID, Payment, Recieve, Remarks, "I","");
                    }
                //}
            }
            foreach (var args in e.UpdateValues)
            {
                string ReceiptDetail_ID = Convert.ToString(args.Keys["ReceiptDetail_ID"]);
                string Type = Convert.ToString(args.NewValues["Type"]);
                string DocumentID = Convert.ToString(args.NewValues["DocumentID"]);

                string Recieve = Convert.ToString(args.NewValues["Receipt"]);
                string Payment = Convert.ToString(args.NewValues["Payment"]);
                string Remarks = Convert.ToString(args.NewValues["Remarks"]);
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
                    //if (MainAccount != "" && MainAccount != "0")
                    //{
                        // if ((VoucherType == "R" && Recieve != "0.0") || (VoucherType == "P" && Payment != "0.0"))
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
                            }
                        }

                    //}
                }
            }
            foreach (var args in e.DeleteValues)
            {
                string ReceiptDetail_ID = Convert.ToString(args.Keys["ReceiptDetail_ID"]);
                DataRow dr = CustomerPayRecdt.Select("ReceiptDetail_ID ='" + ReceiptDetail_ID + "'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                if (dr != null)
                {
                    dr["Status"] = "D";
                }
            }
            Session["ReceiptPaymentDetails"] = CustomerPayRecdt;
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

                if (Status != "D")
                {
                    if (DocumentID == "" && strType!="Advance")
                    {
                        validate = "nullQuantity";
                        break;
                    }
                }
            }
            if (validate == "outrange" || validate == "duplicate" || validate == "nullQuantity")
            {
                gridBatch.JSProperties["cpSaveSuccessOrFail"] = validate;
            }
            else
            {
                string ActionType = "", strIBRef = "";
                if (hdn_Mode.Value == "Entry")
                {
                    //DataTable dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + Convert.ToInt32(CmbScheme.Value));
                    //int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);
                    //if (scheme_type != 0)
                    //{
                    //    gridBatch.JSProperties["cpVouvherNo"] = JVNumStr;
                    //}
                    ActionType = "ADD";
                    strIBRef = "";
                  
                }
                else
                {
                    ActionType = "Edit";
                    strIBRef = Convert.ToString(Session["IBRef"]);
                }

                string strEditCashBankID = Convert.ToString(Session["ReceiptPayment_ID"]);
              
                string strCustomer = Convert.ToString(hdfLookupCustomer.Value);
                string strContactName = Convert.ToString(cmbContactPerson.Value);
                if(strContactName=="")
                {
                    strContactName = "0";
                }
                string strCashBankBranchID = Convert.ToString(ddlBranch.SelectedValue);
                string strTransactionDate = Convert.ToString(dtTDate.Date);
                string paymenttype = Convert.ToString(rdl_MultipleType.SelectedValue);
                string strCashBankID = "";
                if(paymenttype=="S")
                {
                    strCashBankID = Convert.ToString(ddlCashBank.Value);
                }
                else
                {
                    strCashBankID = "0";
                }
               
                string strExchangeSegmentID = "1";
                string strTransactionType = Convert.ToString(ComboVoucherType.SelectedValue);
                string strEntryUserProfile = "F";
                string strNarration = txtNarration.Text;
                string strCurrency = Convert.ToString(CmbCurrency.Value);
                string strInstrumentType= Convert.ToString(cmbInstrumentType.Value);

                string strVoucherAmount = txtVoucherAmount.Text.Trim();
               


                string strInstrumentNumber = txtInstNobth.Text.Trim();
                string strInstrumentDate = (InstDate.Date.ToString("yyyy-MM-dd")=="0001-01-01")?" ":InstDate.Date.ToString("yyyy-MM-dd");
                string strrate = txtRate.Text.Trim();

                //Sudip Pal

                string strFinYear = GetFinancialYearCheckAccordingDaterange(Convert.ToDateTime(dtTDate.Value));
                if (Save_Record(ActionType, strEditCashBankID, JVNumStr, strCashBankBranchID, strTransactionDate, strCashBankID, strExchangeSegmentID, strTransactionType,
                    strEntryUserProfile, strVoucherAmount, strCustomer, strContactName,
                     strNarration, strIBRef, strCurrency, strInstrumentType, strInstrumentNumber, strInstrumentDate, strrate, CustomerPayRecdt, strFinYear) == true)
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
                    if (Session["ReceiptPaymentDetails"]!=null)
                    {
                        Session["ReceiptPaymentDetails"] = null;
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

                    #endregion
                }
                else
                {
                    gridBatch.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                }
            }
        }
        //protected void DocumentCmb_Init(object sender, EventArgs e)
        //{
        //    ASPxComboBox documentCombo = sender as ASPxComboBox;
        //    GridViewEditItemTemplateContainer container = documentCombo.NamingContainer as GridViewEditItemTemplateContainer;
        //    string TypeID = Convert.ToString(container.Grid.GetRowValues(container.Grid.VisibleStartIndex, "Type"));
        //    gridBatch.JSProperties["cplastDocumentType"] = TypeID;
        //    documentCombo.DataSource = GetDocumentDetails(Convert.ToString(TypeID));
        //}
        //protected void DocumentCmb_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    string TypeID = Convert.ToString(e.Parameter);
        //    ASPxComboBox documentCombo = sender as ASPxComboBox;
        //    documentCombo.DataSource = GetDocumentDetails(Convert.ToString(TypeID));
        //    documentCombo.DataBind();
        //}
        public DataTable GetDocumentDetails(string TypeID, String VoucherType, string receiptdate)
        {
            //string ReceiptPayment_ID = Convert.ToString(Session["ReceiptPayment_ID"]);

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_getDocumentDetails");
            proc.AddVarcharPara("@DocumentType", 20, TypeID);
            proc.AddVarcharPara("@DiscardIds", 100, hdndocumentno.Value);
            proc.AddVarcharPara("@VoucherType", 20, VoucherType);
            proc.AddVarcharPara("@Receiptdate", 50, Convert.ToString(receiptdate));
            dt = proc.GetTable();
            return dt;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            PaymentDetails.doc_type = "CRP";
            if (Request.QueryString["key"] != "ADD")
            {
                PaymentDetails.StorePaymentDetailsToHiddenfield(Convert.ToInt32(Request.QueryString["key"]));
            }
            if (Request.QueryString["key"] == "ADD")
            {
                ((GridViewDataComboBoxColumn)gridBatch.Columns["Type"]).PropertiesComboBox.DataSource = GetDocumentType();
                //string Type = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_TransactionType"]);//0
                //((GridViewDataComboBoxColumn)gridBatch.Columns["Type"]).PropertiesComboBox.DataSource = GetDocumentType(Type);
            }
            else
            {
                 Session["ReceiptPayment_ID"] = Request.QueryString["key"];
                 DataTable CRPOrderEditdt = GetCustomerReceiptPaymentEditData();
                 if (CRPOrderEditdt != null && CRPOrderEditdt.Rows.Count > 0)
                 {
                     string Type = Convert.ToString(CRPOrderEditdt.Rows[0]["ReceiptPayment_TransactionType"]);//0
                     ((GridViewDataComboBoxColumn)gridBatch.Columns["Type"]).PropertiesComboBox.DataSource = GetDocumentType(Type);
                 }
            }
           
            //((GridViewDataComboBoxColumn)gridBatch.Columns["DocumentID"]).PropertiesComboBox.DataSource = GetDocumentDetails("All");

            if (!IsPostBack)
            {
                gridBatch.DataBind();
            }
        }
        public IEnumerable GetDocumentType(string VoucherType)
        {
            List<DocumentType> DocumentTypeList = new List<DocumentType>();
            DataTable DT = GetDocumentDetails("DocumentType", Convert.ToString(VoucherType), "");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                DocumentType DocumentTypes = new DocumentType();
                DocumentTypes.Type = Convert.ToString(DT.Rows[i]["Type"]);
                DocumentTypes.ID = Convert.ToString(DT.Rows[i]["ID"]);
                DocumentTypeList.Add(DocumentTypes);
            }

            return DocumentTypeList;
        }
        public IEnumerable GetDocumentType()
        {
            List<DocumentType> DocumentTypeList = new List<DocumentType>();
            DataTable DT = GetDocumentDetails("DocumentType", Convert.ToString(ComboVoucherType.SelectedValue),"");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                DocumentType DocumentTypes = new DocumentType();
                DocumentTypes.Type = Convert.ToString(DT.Rows[i]["Type"]);
                DocumentTypes.ID = Convert.ToString(DT.Rows[i]["ID"]);
                DocumentTypeList.Add(DocumentTypes);
            }

            return DocumentTypeList;
        }
        private bool Save_Record(string ActionType, string strEditCashBankID, string strVoucherNumber, string strCashBankBranchID, string strTransactionDate,
            string strCashBankID,
             string strExchangeSegmentID, string strTransactionType, string strEntryUserProfile,string strVoucherAmount,string strCustomer, string strContactName, 
            string strNarration, string strIBRef,
          string strCurrency, string strInstrumentType, string strInstrumentNumber, string strInstrumentDate, string strrate,
          DataTable strReceiptPaymentdt,string Finyear)
        {
            try
            {
                gridBatch.JSProperties["cpExitNew"] = null;

                if (hdn_Mode.Value == "Entry")
                {

                    strIBRef = "CPR_" + Session["userid"].ToString() + "_" + strTransactionType + "_" + JVNumStr.Replace("/", "");

                }
                DataSet dsInst = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_CustomerPaymentReceiptInsertUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", ActionType);//1
                cmd.Parameters.AddWithValue("@EditReceiptPaymentID", strEditCashBankID);//2
                cmd.Parameters.AddWithValue("@VoucherNumber", strVoucherNumber);//3

                cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));//4
                cmd.Parameters.AddWithValue("@FinYear", Finyear);//5
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

               
                
                cmd.Parameters.AddWithValue("@ReceiptPaymentDetails", strReceiptPaymentdt);//22

                string paymenttype = rdl_MultipleType.SelectedValue;
                DataTable dtMultiType;
                if(paymenttype!="S")
                {
                     dtMultiType = PaymentDetails.GetPaymentTable();
                    
                }
                else
                {
                     dtMultiType = CreatePaymentDataTable();
                }
              
                cmd.Parameters.AddWithValue("@PaymentType", paymenttype);//23
                cmd.Parameters.AddWithValue("@paymentDetails", dtMultiType);//23

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
                    

                //  gridBatch.JSProperties["cpExitNew"] = "YES";
                return true;

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

        protected void grid_SalesInvoice_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
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
            if(strSplitCommand == "BindSalesInvoiceDetails")
            {
                DataTable dtInvoice = new DataTable();
                string strAmount = e.Parameters.Split('~')[1];
                if (hdfLookupCustomer.Value!="")
                {                    
                    dtInvoice = objCustomerVendorReceiptPaymentBL.GetSalesInvoiceDetails(Convert.ToString(hdfLookupCustomer.Value), strAmount);
                    if (dtInvoice != null && dtInvoice.Rows.Count > 0)
                    {
                        Session["AllInvoices"] = dtInvoice;
                        grid_SalesInvoice.DataSource = dtInvoice;
                        grid_SalesInvoice.DataBind();
                    }
                }
                }
               

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
            else {
                acbpCrpUdf.JSProperties["cpUDF"] = "true";
            }
        }

        protected void acpCheckAmount_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            DataTable dtAmount = new DataTable();
            string strType = e.Parameter.Split('~')[0];
            string strDocumentId = e.Parameter.Split('~')[1];


            dtAmount = GetExactAmount(strType, strDocumentId,"OP");
            string UnPaidAmount = string.Empty;
            if (dtAmount != null && dtAmount.Rows.Count > 0)
            {
                UnPaidAmount = Convert.ToString(dtAmount.Rows[0]["UnPaidAmount"]);
                acpCheckAmount.JSProperties["cpUnPaidAmount"] = UnPaidAmount;
            }

        }
        public DataTable GetExactAmount(string Type, string documentId, string strIsOpening)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerReceiptPaymentDetails");
            proc.AddVarcharPara("@Action", 500, "GetExactAmount");
            proc.AddVarcharPara("@Type", 500, Type);
            proc.AddIntegerPara("@DocumentId", Convert.ToInt32(documentId));
            proc.AddVarcharPara("@IsOpening", 50, strIsOpening);
            ds = proc.GetTable();
            return ds;
        }

        protected void ddlCashBank_Callback(object sender, CallbackEventArgsBase e)
        {
            string userbranch = e.Parameter.Split('~')[0];
            BindCashBankAccount(userbranch);           
            
        }


        #region Opening Sales Customer Payment Receipt
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
                    dtTDate.MaxDate = Convert.ToDateTime(Convert.ToString(dtsale.Rows[0]["FinYear_StartDate"])).AddDays(-1);
                    //tDate.Value = Convert.ToString(Convert.ToDateTime(Convert.ToString(dtsale.Rows[0]["FinYear_StartDate"])).AddDays(-1));
                    string fyrrrr = Convert.ToString(Convert.ToDateTime(Convert.ToString(dtsale.Rows[0]["FinYear_StartDate"])).AddDays(-1));
                    string[] FinYEnd = Convert.ToString(fyrrrr).Split(' ');
                    string FinYearEnd = FinYEnd[0];
                    DateTime date3 = DateTime.ParseExact(FinYearEnd, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    string ForJournalDate = Convert.ToString(date3);

                    string fDate = null;

                    fDate = Convert.ToString(Convert.ToDateTime(ForJournalDate).Month) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Day) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Year);


                    dtTDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
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
    }
}