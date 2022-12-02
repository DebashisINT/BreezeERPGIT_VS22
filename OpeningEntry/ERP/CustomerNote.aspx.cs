using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OpeningEntry.ERP
{
    public partial class CustomerNote : System.Web.UI.Page
    {
        #region Global Veriable

        BusinessLogicLayer.OtherTasks oOtherTasks = new BusinessLogicLayer.OtherTasks();
        BusinessLogicLayer.DailyTaskOther oDailyTaskOther = new BusinessLogicLayer.DailyTaskOther();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        DebitCreditNoteBL objDebitCreditBL = new DebitCreditNoteBL();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        Converter oconverter = new Converter();
        string JVNumStr = string.Empty;
        public static EntityLayer.CommonELS.UserRightsForPage rights;

        #endregion


        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
        
            if (Request.QueryString.AllKeys.Contains("IsTagged"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
                //divcross.Visible = false;
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
                //divcross.Visible = true;
            }
          
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsCustomer.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
         SqlDataSourceapplicable.ConnectionString=Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            
            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!IsPostBack)
                {
                    GetFinancialYearstartandEnddate();
                       //tDate.MinDate=
                       // tDate.MaxDate=

                    rights = new UserRightsForPage();
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/ERP/CustomerNote.aspx");

                    #region Note Date

                    tDate.EditFormatString = objConverter.GetDateFormat("Date");
                    string fDate = null;

                    string[] FinYEnd = Convert.ToString(Session["FinYearEnd"]).Split(' ');
                    string FinYearEnd = FinYEnd[0];
                    DateTime date3 = DateTime.ParseExact(FinYearEnd, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    string ForJournalDate = Convert.ToString(date3);
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

                ///    tDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    #endregion

                    FillSearchGrid();
                    BindBranchFrom();
                    BindCurrency();

                    string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                    ddlBranch.SelectedValue = strdefaultBranch;

                    hdnNotelNo.Value = "";

                    Session["exportval"] = null;
                    IsUdfpresent.Value = Convert.ToString(getUdfCount());
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
        }

        #region Main Grid

        public void GetFinancialYearstartandEnddate()
        {
            CustomerSales oms = new CustomerSales();
            DataTable  dtsale=new DataTable();
            if (Session["LastFinYear"] != null)
            {
                dtsale = oms.GetCustomersalesFinancialyear(Convert.ToString(Session["LastFinYear"]));
                if (dtsale.Rows.Count > 0)
                {
                  //  tDate.MinDate = Convert.ToDateTime(Convert.ToString(dtsale.Rows[0]["FinYear_StartDate"])).AddYears(-); ;
                    tDate.MaxDate = Convert.ToDateTime(Convert.ToString(dtsale.Rows[0]["FinYear_StartDate"])).AddDays(-1);
                    //tDate.Value = Convert.ToString(Convert.ToDateTime(Convert.ToString(dtsale.Rows[0]["FinYear_StartDate"])).AddDays(-1));
                    string fyrrrr = Convert.ToString(Convert.ToDateTime(Convert.ToString(dtsale.Rows[0]["FinYear_StartDate"])).AddDays(-1));
                    string[] FinYEnd = Convert.ToString(fyrrrr).Split(' ');
                    string FinYearEnd = FinYEnd[0];
                    DateTime date3 = DateTime.ParseExact(FinYearEnd, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    string ForJournalDate = Convert.ToString(date3);
                  
                    string fDate = null;
            
                    fDate = Convert.ToString(Convert.ToDateTime(ForJournalDate).Month) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Day) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Year);


                        tDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
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
                    dtsale = Convert.ToString(dttab.Rows[0]["FinYear_Code"]) ;
                  
                }

            }
            return dtsale;

        }



        public void BindCurrency()
        {
            DataSet dst = new DataSet();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            string userbranch = Convert.ToString(Session["userbranchHierarchy"]);

            dst = objSlaesActivitiesBL.GetAllDropDownDetailForSalesQuotation(userbranch);

            #region Currency Drop Down Start

            if (dst.Tables[3] != null && dst.Tables[3].Rows.Count > 0)
            {
                ddl_Currency.DataTextField = "Currency_Name";
                ddl_Currency.DataValueField = "Currency_ID";
                ddl_Currency.DataSource = dst.Tables[3];
                ddl_Currency.DataBind();
            }
            int currencyindex = 1;
            int no = 0;
            if (Session["LocalCurrency"] != null)
            {
                if (ddl_Currency.Items.Count > 0)
                {
                    string[] ActCurrency = new string[] { };
                    string currency = Convert.ToString(HttpContext.Current.Session["LocalCurrency"]);
                    ActCurrency = currency.Split('~');
                    foreach (ListItem li in ddl_Currency.Items)
                    {
                        if (li.Value == Convert.ToString(ActCurrency[0]))
                        {
                            no = 1;
                            break;
                        }
                        else
                        {
                            currencyindex += 1;
                        }
                    }
                }
                ddl_Currency.Items.Insert(0, new ListItem("Select", "0"));
                if (no == 1)
                {
                    ddl_Currency.SelectedIndex = currencyindex;
                }
                else
                {
                    ddl_Currency.SelectedIndex = no;
                }
            }

            #endregion Currency Drop Down End
        }
        public void BindBranchFrom()
        {
            dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
            ddlBranch.DataBind();
        }
        protected void GvJvSearch_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            int RowIndex;
            string PCBCommandName = e.Parameters.Split('~')[0];

            GvJvSearch.JSProperties["cpJVDelete"] = null;

            if (PCBCommandName == "PCB_BindAfterDelete")
            {
                FillSearchGrid();
            }
            else if (PCBCommandName == "PCB_DeleteBtnOkE")
            {
                int strIsComplete = 0;
                RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                string NoteID = GvJvSearch.GetRowValues(RowIndex, "DCNote_ID").ToString();

                objDebitCreditBL.DeleteDrCrNote("Delete", Convert.ToString(NoteID), ref strIsComplete);

                if (strIsComplete == 1)
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Successfully Deleted";
                }
                else if (strIsComplete == -1)
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Customer Debit/Credit Note already used in Customer Receipt/Payment.";
                }
                else
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Problem in Deleting. Sry for Inconvenience";
                }
            }
        }
        protected void GvJvSearch_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (!rights.CanDelete)
            {
                if (e.ButtonID == "CustomBtnDelete")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }
            if (!rights.CanEdit)
            {
                if (e.ButtonID == "CustomBtnEdit")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }
            if (!rights.CanPrint)
            {
                if (e.ButtonID == "CustomBtnPrint")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }
        }
        protected void GvJvSearch_DataBinding(object sender, EventArgs e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
            string CompanyID = Convert.ToString(Session["LastCompany"]).Trim();
            string branchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

            DataTable dtdata = objDebitCreditBL.GetSearchGridData(branchHierarchy, CompanyID, FinYear, "GetCustomerSearchGridforOpening");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                GvJvSearch.DataSource = dtdata;
            }
            else
            {
                GvJvSearch.DataSource = null;
            }
        }
        void FillSearchGrid()
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
            string CompanyID = Convert.ToString(Session["LastCompany"]).Trim();
            string branchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

            DataTable dtdata = GetSearchGridData(branchHierarchy, CompanyID, FinYear, "GetCustomerSearchGridforOpening");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                GvJvSearch.DataSource = dtdata;
                GvJvSearch.DataBind();
            }
            else
            {
                GvJvSearch.DataSource = null;
                GvJvSearch.DataBind();
            }
        }

        #endregion

        #region Classes

        public class MainAccount
        {
            public string CountryID { get; set; }
            public string CountryName { get; set; }
        }
        public class SubAccount
        {
            public string CityID { get; set; }
            public string CityName { get; set; }
        }
        public class VOUCHERLIST
        {
            public string CashReportID { get; set; }
            public string CountryID { get; set; }
            public string CityID { get; set; }
            public string WithDrawl { get; set; }
            public string Receipt { get; set; }
            public string Narration { get; set; }
        }

        #endregion

        #region Get Mainaccount, Subaccount & Journal Details

        public IEnumerable GetAllMainAccount()
        {
            List<MainAccount> MainAccountList = new List<MainAccount>();
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = objEngine.GetDataTable("Master_MainAccount", " MainAccount_ReferenceID as CountryID,MainAccount_Name+' [ '+rtrim(ltrim(MainAccount_AccountCode))+' ]' as CountryName ", " MainAccount_BankCashType Not In ('Bank','Cash')");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                MainAccount MainAccounts = new MainAccount();
                MainAccounts.CountryID = Convert.ToString(DT.Rows[i]["CountryID"]);
                MainAccounts.CountryName = Convert.ToString(DT.Rows[i]["CountryName"]);
                MainAccountList.Add(MainAccounts);
            }

            return MainAccountList;
        }
        public IEnumerable GetMainAccount(string branchId)
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);

            List<MainAccount> MainAccountList = new List<MainAccount>();
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = objEngine.GetDataTable("Master_MainAccount", " MainAccount_ReferenceID as CountryID,MainAccount_Name+' [ '+rtrim(ltrim(MainAccount_AccountCode))+' ]' as CountryName ", " MainAccount_BankCashType Not In ('Bank','Cash') AND MainAccount_branchId in ('" + branchId + "','0') AND MainAccount_BankCompany in ('" + strCompanyID + "','')");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                MainAccount MainAccounts = new MainAccount();
                MainAccounts.CountryID = Convert.ToString(DT.Rows[i]["CountryID"]);
                MainAccounts.CountryName = Convert.ToString(DT.Rows[i]["CountryName"]);
                MainAccountList.Add(MainAccounts);
            }

            return MainAccountList;
        }
        public IEnumerable GetSubAccount(string strMainAccount, string strBranch, string strType)
        {
            DataTable DT = GetSubAccountTable(strMainAccount, strBranch, strType);

            List<SubAccount> SubAccountList = new List<SubAccount>();

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                SubAccount SubAccounts = new SubAccount();
                SubAccounts.CityID = Convert.ToString(DT.Rows[i][1]);
                SubAccounts.CityName = Convert.ToString(DT.Rows[i][0]);
                SubAccountList.Add(SubAccounts);
            }

            return SubAccountList;
        }
        public DataTable GetSubAccountTable(string strMainAccount, string strBranch, string strType)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_FethSubAccount");
            proc.AddVarcharPara("@CashBank_MainAccountID", 500, strMainAccount);
            proc.AddVarcharPara("@clause", 500, "");
            proc.AddVarcharPara("@branch", 500, strBranch);
            proc.AddVarcharPara("@SelectionType", 500, strType);
            dt = proc.GetTable();
            return dt;
        }
        public IEnumerable GetVoucher()
        {
            DataSet DsOnLoad = new DataSet();
            DataTable tempdt = new DataTable();
            List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();

            string VoucherNumber = Convert.ToString(hdnNotelNo.Value);
            DataTable Voucherdt = GetNoteDetails("Details", VoucherNumber);

            if (Voucherdt.Rows.Count > 0 && Voucherdt != null)
            {
                for (int i = 0; i < Voucherdt.Rows.Count; i++)
                {
                    VOUCHERLIST Vouchers = new VOUCHERLIST();
                    Vouchers.CashReportID = Convert.ToString(Voucherdt.Rows[i][0]);
                    Vouchers.CountryID = Convert.ToString(Voucherdt.Rows[i][1]).Trim();
                    Vouchers.CityID = Convert.ToString(Voucherdt.Rows[i][2]).Trim();
                    Vouchers.WithDrawl = Convert.ToString(Voucherdt.Rows[i][3]);
                    Vouchers.Receipt = Convert.ToString(Voucherdt.Rows[i][4]);
                    Vouchers.Narration = Convert.ToString(Voucherdt.Rows[i][5]);
                    VoucherList.Add(Vouchers);
                }
            }

            return VoucherList;
        }
        public DataTable GetNoteDetails(string Action, string NotelNo)
        {
            DataTable dt = objDebitCreditBL.GetNoteDetails(Action, NotelNo);
            return dt;
        }

        #endregion

        #region Grid Events

        protected void Page_Init(object sender, EventArgs e)
        
        {
            ((GridViewDataComboBoxColumn)grid.Columns["CountryID"]).PropertiesComboBox.DataSource = GetAllMainAccount();
            ((GridViewDataComboBoxColumn)grid.Columns["CityID"]).PropertiesComboBox.DataSource = GetSubAccount("", Convert.ToString(Session["userbranchID"]), "ALL");

            if (!IsPostBack)
            {
                grid.DataBind();
            }
        }
        protected void CityCmb_Callback(object sender, CallbackEventArgsBase e)
        {
            string countryID = Convert.ToString(e.Parameter);
            ASPxComboBox c = sender as ASPxComboBox;
            c.DataSource = GetSubAccount(Convert.ToString(countryID), "", "");//DataProvider.GetCities(countryID);
            c.DataBind();
        }
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            grid.DataSource = GetVoucher();
        }
        public void BindVoucherGrid()
        {
            grid.DataSource = GetVoucher();
            grid.DataBind();
        }
        protected void CityCmb_Init(object sender, EventArgs e)
        {
            ASPxComboBox cityCombo = sender as ASPxComboBox;
            GridViewEditItemTemplateContainer container = cityCombo.NamingContainer as GridViewEditItemTemplateContainer;
            string countryID = Convert.ToString(container.Grid.GetRowValues(container.Grid.VisibleStartIndex, "CountryID"));
            grid.JSProperties["cplastCountryID"] = countryID;
            cityCombo.DataSource = GetSubAccount(Convert.ToString(countryID), "", "");
        }
        private void bindMainAccount(object source, CallbackEventArgsBase e)
        {
            ASPxComboBox currentCombo = source as ASPxComboBox;
            currentCombo.DataSource = GetMainAccount(e.Parameter);
            currentCombo.DataBind();
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
        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "CountryID")
            {
                ((ASPxComboBox)e.Editor).Callback += new CallbackEventHandlerBase(bindMainAccount);
            }

            e.Editor.ReadOnly = false;
        }
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string type = Convert.ToString(e.Parameters.Split('~')[0]);
            grid.JSProperties["cpSaveSuccessOrFail"] = null;

            if (type == "Edit")
            {
                int RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                string NoteID = Convert.ToString(GvJvSearch.GetRowValues(RowIndex, "DCNote_ID")).Trim();

                hdnNotelNo.Value = NoteID;

                DataTable Voucherdt = GetNoteDetails("Details", NoteID);
                string Credit = Convert.ToString(Voucherdt.Compute("Sum(Receipt)", ""));
                string Debit = Convert.ToString(Voucherdt.Compute("Sum(WithDrawl)", ""));


                DataTable Detailsdt = GetNoteDetails("Header", NoteID);
                if (Detailsdt != null && Detailsdt.Rows.Count > 0)
                {
                    string BranchId = Convert.ToString(Detailsdt.Rows[0]["BranchID"]);
                    string BillNumber = Convert.ToString(Detailsdt.Rows[0]["DocumentNumber"]);
                    string TransactionDate = Convert.ToString(Detailsdt.Rows[0]["DocumentDate"]);
                    string Narration = Convert.ToString(Detailsdt.Rows[0]["Narration"]);

                    string NoteType = Convert.ToString(Detailsdt.Rows[0]["NoteType"]);
                    string CustomerID = Convert.ToString(Detailsdt.Rows[0]["CustomerID"]);
                    string CurrencyId = Convert.ToString(Detailsdt.Rows[0]["CurrencyId"]);
                    string Rate = Convert.ToString(Detailsdt.Rows[0]["Rate"]);

                    grid.JSProperties["cpEdit"] = BillNumber + "~" + Narration + "~" + BranchId + "~" + Credit + "~" + Debit + "~" + TransactionDate + "~" + NoteType + "~" + CustomerID + "~" + CurrencyId + "~" + Rate + "~" + NoteID;
                }

                grid.DataSource = GetVoucher();
                grid.DataBind();
            }
            else if (type == "BlanckEdit")
            {
                grid.DataSource = null;
                grid.DataBind();
            }
        }
        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            grid.JSProperties["cpVouvherNo"] = "";
            grid.JSProperties["cpSaveSuccessOrFail"] = null;

            string Action = Convert.ToString(hdnMode.Value);
            DataTable Journaldt = new DataTable();

            if (Action == "0")
            {
                Journaldt.Columns.Add("CashReportID", typeof(string));
                Journaldt.Columns.Add("MainAccount", typeof(string));
                Journaldt.Columns.Add("SubAccount", typeof(string));
                Journaldt.Columns.Add("Amount", typeof(string));
                Journaldt.Columns.Add("Receipt", typeof(string));
                Journaldt.Columns.Add("Narration", typeof(string));
                Journaldt.Columns.Add("Status", typeof(string));
            }
            else
            {
                string VoucherNo = Convert.ToString(hdnNotelNo.Value);

                Journaldt = GetNoteDetails("Details", VoucherNo);
            }

            foreach (var args in e.InsertValues)
            {
                string MainAccount = Convert.ToString(args.NewValues["CountryID"]);
                string SubAccount = Convert.ToString(args.NewValues["CityID"]);
                string WithDrawl = Convert.ToString(args.NewValues["WithDrawl"]);
                string Receipt = "0";
                string Narration = Convert.ToString(args.NewValues["Narration"]);

                if ((Convert.ToDecimal(WithDrawl) > 0 && MainAccount != "" && MainAccount != null) || (Convert.ToDecimal(Receipt) > 0 && MainAccount != "" && MainAccount != null))
                {
                    Journaldt.Rows.Add("0", MainAccount, SubAccount, WithDrawl, Receipt, Narration, "I");
                }
            }

            foreach (var args in e.UpdateValues)
            {
                string CashReportID = Convert.ToString(args.Keys["CashReportID"]);
                string MainAccount = Convert.ToString(args.NewValues["CountryID"]);
                string SubAccount = Convert.ToString(args.NewValues["CityID"]);
                string WithDrawl = Convert.ToString(args.NewValues["WithDrawl"]);
                string Receipt = "0";
                string Narration = Convert.ToString(args.NewValues["Narration"]);

                bool isDeleted = false;

                foreach (var arg in e.DeleteValues)
                {
                    string DeleteID = Convert.ToString(arg.Keys["CashReportID"]);
                    if (DeleteID == CashReportID)
                    {
                        isDeleted = true;
                        break;
                    }
                }

                if (isDeleted == false)
                {
                    if ((Convert.ToDecimal(WithDrawl) > 0 && MainAccount != "" && MainAccount != null) || (Convert.ToDecimal(Receipt) > 0 && MainAccount != "" && MainAccount != null))
                    {
                        DataRow dr = Journaldt.Select("CashReportID ='" + CashReportID + "'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                        if (dr != null)
                        {
                            dr["MainAccount"] = MainAccount;
                            dr["SubAccount"] = SubAccount;
                            dr["WithDrawl"] = WithDrawl;
                            dr["Receipt"] = Receipt;
                            dr["Narration"] = Narration;
                            dr["Status"] = "U";
                        }
                    }
                }
            }

            foreach (var args in e.DeleteValues)
            {
                string CashReportID = Convert.ToString(args.Keys["CashReportID"]);
                DataRow dr = Journaldt.Select("CashReportID ='" + CashReportID + "'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                if (dr != null)
                {
                    dr["Status"] = "D";
                }
            }

            string SchemaID = (Convert.ToString(hdnSchemaID.Value) == "") ? "0" : Convert.ToString(hdnSchemaID.Value);

            string validate = checkNMakeJVCode(Convert.ToString(txtBillNo.Text), Convert.ToInt32(SchemaID));
            if (validate == "outrange" || validate == "duplicate")
            {
                grid.JSProperties["cpSaveSuccessOrFail"] = validate;
            }
            else
            {
                if (Action == "0")
                {
                    DataTable dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + Convert.ToInt32(SchemaID));
                    int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);
                    if (scheme_type != 0)
                    {
                        grid.JSProperties["cpVouvherNo"] = JVNumStr;
                    }
                }

                string strFinYear= GetFinancialYearCheckAccordingDaterange(Convert.ToDateTime(tDate.Value));

              //  string strFinYear = Convert.ToString(Session["LastFinYear"]);


                string strCompanyID = Convert.ToString(Session["LastCompany"]);
                string strBranchID = Convert.ToString(ddlBranch.SelectedValue);
                string strCurrency = Convert.ToString(Session["LocalCurrency"]).Split('~')[0];
                string strUserID = Convert.ToString(Session["userid"]);

                string NoteType = Convert.ToString(ddlNoteType.SelectedValue);
                string NoteDate = Convert.ToString(tDate.Value);
                string MainNarration = Convert.ToString(txtNarration.Text);
                string CustomerName = Convert.ToString(hdfLookupCustomer.Value);
                string Currency = Convert.ToString(ddl_Currency.SelectedValue);
                decimal Rate = Convert.ToDecimal(txt_Rate.Value);

                string NotelNo = "";
                if (Action == "0")
                {
                    Action = "Add";
                    NotelNo = "";
                }
                else
                {
                    Action = "Edit";
                    NotelNo = Convert.ToString(hdnNotelNo.Value);
                    JVNumStr = Convert.ToString(txtBillNo.Text);
                }

                int IsComplete = 0, OutNotelNo = 0;



                ModifyDrCrNote(Action, NotelNo, JVNumStr, strFinYear, strCompanyID, strBranchID, NoteDate, strCurrency, MainNarration, NoteType, CustomerName, Currency, Rate, strUserID, Journaldt, ref IsComplete, ref OutNotelNo);

                if (IsComplete == 1)
                {
                    hdnNotelNo.Value = "";
                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                    if (udfTable != null)
                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("CNOTE", "CustomerNote" + Convert.ToString(OutNotelNo), udfTable, Convert.ToString(Session["userid"]));


                    grid.JSProperties["cpSaveSuccessOrFail"] = "successInsert";
                }
                else
                {
                    grid.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                }
            }
        }

        #endregion


        public void ModifyDrCrNote(string ActionType, string NotelNo, string BillNo, string FinYear, string CompanyID, string BranchID, string NoteDate,
                                   string CurrencyID, string Narration, string NoteType, string CustomerName, string Currency, decimal Rate, string UserID, DataTable JournalDetails,
                                   ref int strIsComplete, ref int strOutNotelNo)
        {
            try
            {
                DataSet dsInst = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_InsertDebitCreditNoteEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@NoteID", NotelNo);
                cmd.Parameters.AddWithValue("@BillNo", BillNo);
                cmd.Parameters.AddWithValue("@FinYear", FinYear);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@NoteDate", Convert.ToDateTime(NoteDate));
                cmd.Parameters.AddWithValue("@CurrencyID", CurrencyID);
                cmd.Parameters.AddWithValue("@Narration", Narration);
                cmd.Parameters.AddWithValue("@NoteType", NoteType);
                cmd.Parameters.AddWithValue("@CustomerName", CustomerName);
                cmd.Parameters.AddWithValue("@DCNote_CurrencyId", Currency);
                cmd.Parameters.AddWithValue("@Rate", Rate);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@JournalDetails", JournalDetails);

                cmd.Parameters.Add("@OutNoteID", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);

                cmd.Parameters["@OutNoteID"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                strOutNotelNo = Convert.ToInt32(cmd.Parameters["@OutNoteID"].Value.ToString());

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }




        #region Database Function

        void BindGrid(ASPxGridView Grid)
        {
            Grid.DataSource = null;
            Grid.DataBind();
        }
        void BindGrid(ASPxGridView Grid, DataSet Ds)
        {
            if (Ds.Tables.Count > 0)
            {
                Grid.DataSource = Ds;
                Grid.DataBind();
            }
            else
            {
                Grid.DataSource = null;
                Grid.DataBind();
            }
        }
        void BindGrid(ASPxGridView Grid, DataTable Dt)
        {
            if (Dt.Rows.Count > 0)
            {
                Grid.DataSource = Dt;
                Grid.DataBind();
            }
            else
            {
                Grid.DataSource = null;
                Grid.DataBind();
            }
        }

        #endregion

        #region WebMethod

        [WebMethod]
        public static bool CheckUniqueName(string VoucherNo, string Type)
        {
            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();

            if (VoucherNo != "" && Convert.ToString(VoucherNo).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(Convert.ToString(VoucherNo).Trim(), Type, "CustomerNote_Check");
            }
            return status;
        }
        [WebMethod]
        public static string getSchemeType(string sel_scheme_id)
        {
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

            //BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //string[] scheme = oDbEngine1.GetFieldValue1("tbl_master_Idschema", " schema_type,length ", "Id = " + Convert.ToInt32(sel_scheme_id), 1);

            return Convert.ToString(strschemavalue);
        }

        #endregion

        #region Others

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedValue));
            drdExport.SelectedValue = "0";

            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        public void bindexport(int Filter)
        {
          //  GvJvSearch.Columns[11].Visible = false;

            string filename = "Customer Debit/Credit Note";

            
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Debit/Credit Note";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

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
        }
        protected string checkNMakeJVCode(string manual_str, int sel_schema_Id)
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
                    prefCompCode = dtSchema.Rows[0]["prefix"].ToString();
                    sufxCompCode = dtSchema.Rows[0]["suffix"].ToString();
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);

                    sqlQuery = "SELECT max(tjv.DCNote_DocumentNumber) FROM Trans_CustDebitCreditNote tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.DCNote_DocumentNumber))) = 1";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.DCNote_DocumentNumber) FROM Trans_CustDebitCreditNote tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.DCNote_DocumentNumber))) = 1";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
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
                    sqlQuery = "SELECT DCNote_DocumentNumber FROM Trans_CustDebitCreditNote WHERE DCNote_DocumentNumber LIKE '" + manual_str.Trim() + "'";
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
        protected object GetSummaryValue(string fieldName)
        {
            ASPxSummaryItem summaryItem = grid.TotalSummary.FirstOrDefault(i => i.Tag == fieldName + "_Sum");
            return grid.GetTotalSummaryValue(summaryItem);
        }
        protected object GetTotalSummaryValue()
        {
            ASPxSummaryItem summaryItem = grid.TotalSummary.First(i => i.Tag == "C2_Sum");
            return grid.GetTotalSummaryValue(summaryItem);
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("CustomerNote.aspx");
        }
        [WebMethod]
        public static List<ListItem> GetScheme(string sel_type_id)
        {
            string LastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string userbranchID = Convert.ToString(HttpContext.Current.Session["userbranchID"]);

            string query = "Select ID,SchemaName From tbl_master_Idschema  Where TYPE_ID=(Case When '" + sel_type_id + "'='Dr' Then '25' Else '26' End) AND IsActive=1 AND Isnull(Branch,'')='" + userbranchID + "' AND Isnull(comapanyInt,'')='" + LastCompany + "'";
            //string constr = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;MULTI
            string constr = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    List<ListItem> customers = new List<ListItem>();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            customers.Add(new ListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["SchemaName"].ToString()
                            });
                        }
                    }
                    con.Close();
                    return customers;
                }
            }
        }
        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='CNOTE'  and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }

        #endregion

        #region Grid Bind Fror  Opening
        public DataTable GetSearchGridData(string BranchList, string CompanyID, string FinYear, string Action)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
            proc.AddVarcharPara("@BranchList", 500, BranchList);
            proc.AddVarcharPara("@CompanyID", 50, CompanyID);
            proc.AddVarcharPara("@FinYear", 50, FinYear);
            proc.AddVarcharPara("@Action", 500, Action);
            dt = proc.GetTable();
            return dt;
        }
        #endregion
    }
}